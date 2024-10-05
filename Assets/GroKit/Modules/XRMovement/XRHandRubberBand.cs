using UnityEngine;

namespace Core3lb
{
    public class XRHandRubberBand : MonoBehaviour
{
        [CoreHeader("Required")]
        [CoreRequired]
        public XRMovement movementRef;
        [Space]
        [CoreRequired]
        public Transform startPointL;              // Drag Point for RubberBand Movement Must be Parented to OVRCamerRig
        [CoreRequired]
        public Transform startPointR;              // Drag Point for RubberBand Movement Must be Parented to OVRCamerRig
        [CoreRequired]
        public Transform endPointL;
        [CoreRequired]
        public Transform endPointR;
        [CoreToggleHeader("RubberBand Lines")]
        public bool showLines = true;
        [CoreShowIf("showLines")]
        [CoreRequired]
        public LineArcSystem rubberBandLineL;
        [CoreShowIf("showLines")]
        [CoreRequired]
        public LineArcSystem rubberBandLineR;
        [CoreHeader("Settings")]
        [Tooltip("takes the default movement speed and * it with this")]
        public float speedMulti = 1;
        public float maxRadius = .2f;

        bool rbLeftOn;
        bool rbRightOn;

        public bool ignoreGravity;
        [CoreHeader("Input Settings")]
        public bool enableLeftHand = true;
        public bool enableRightHand = true;
        public bool useEventForInputs;
        [CoreHideIf("useEventForInputs")]
        public InputXR.Button movementButton = InputXR.Button.Move;

        bool leftInputEvent = false;
        bool rightInputEvent = false;
        void Start()
        {
            movementRef = gameObject.GetComponentIfNull<XRMovement>(movementRef);
            //ParentObjects just in case you forgot
            startPointL.parent = movementRef.controller.transform;
            startPointR.parent = movementRef.controller.transform;
            //Parent Lines
            rubberBandLineL.transform.parent = movementRef.controller.transform;
            rubberBandLineR.transform.parent = movementRef.controller.transform;
            //Hide Drag Points
            startPointL.gameObject.SetActive(false);
            startPointR.gameObject.SetActive(false);
        }

        public void _InputLeft(bool chg)
        {
            Debug.LogError(chg);
            leftInputEvent = chg;
        }

        public void _InputRight(bool chg)
        {
            rightInputEvent = chg;
        }

        //RubberBandControlSystems;
        void Update()
        {
            if (movementRef.canMove)
            {
                if (enableLeftHand)
                {
                    RubberBandMoveL();
                }
                if(enableRightHand)
                {
                    RubberBandMoveR();
                }
                movementRef.DoMovement();
            }
        }

        public bool GetInput(InputXR.Controller controller)
        {
            if(useEventForInputs) 
            {
                if(controller== InputXR.Controller.Left)
                {
                    return leftInputEvent;
                }
                else
                {
                    return rightInputEvent;
                }
            }
            return InputXR.instance.GetButton(movementButton, controller);
        }

        //Late Update is used to show lines and prevent Jittering
        void LateUpdate()
        {
            if (!showLines)
            {
                return;
            }
            if (rbLeftOn && rubberBandLineL)
            {
                rubberBandLineL.CreateLine(startPointL.position, endPointL.position, Color.blue);
            }
            else
            {
                if(rubberBandLineL)
                {
                    rubberBandLineL.HideLine();
                }
            }
            if (rbRightOn && rubberBandLineR)
            {
                rubberBandLineR.CreateLine(startPointR.position, endPointR.position, Color.blue);
            }
            else
            {
                if (rubberBandLineR)
                {
                    rubberBandLineR.HideLine();
                }
            }
        }

        //Left Rubberband Movement
        public void RubberBandMoveL()
        {
            if (!rbLeftOn)
            {
                if (GetInput(InputXR.Controller.Left) && !movementRef.isContextBlocked(InputXR.Controller.Left))
                {
                    startPointL.gameObject.SetActive(true);
                    startPointL.position = endPointL.position;
                    rbLeftOn = true;
                }
            }
            if (rbLeftOn)
            {
                Vector3 holder = endPointL.position - startPointL.position;
                float distance = Vector3.Distance(endPointL.position, startPointL.position);
                MoveWithController(holder, distance);
                if (!GetInput(InputXR.Controller.Left))
                {
                    rbLeftOn = false;
                    startPointL.gameObject.SetActive(false);
                }
            }
        }


        public void RubberBandMoveR()
        {
            if (!rbRightOn)
            {
                if (GetInput(InputXR.Controller.Right) && !movementRef.isContextBlocked(InputXR.Controller.Left))
                {
                    startPointR.gameObject.SetActive(true);
                    startPointR.position = endPointR.position;
                    rbRightOn = true;
                }
            }
            if (rbRightOn)
            {
                Vector3 holder = endPointR.position - startPointR.position;
                float distance = Vector3.Distance(endPointR.position, startPointR.position);
                MoveWithController(holder, distance);
                if (!GetInput(InputXR.Controller.Right))
                {
                    rbRightOn = false;
                    startPointR.gameObject.SetActive(false);
                }
            }
        }

        public void OnDisable()
        {
            rbLeftOn = false;
            rbRightOn = false;
            startPointR.gameObject.SetActive(false);
            startPointL.gameObject.SetActive(false);
            rubberBandLineL.HideLine();
            rubberBandLineR.HideLine();
            leftInputEvent = false;
            rightInputEvent = false;
        }


        public void MoveWithController(Vector3 direction, float distance)
        {
            distance = Mathf.Clamp(distance, -maxRadius / 2, maxRadius / 2);
            distance = ConvertRange(0, maxRadius / 2, 0, 1, distance);
            //Multiply By Distance
            direction = direction.normalized * distance;
            direction = direction * (movementRef.defaultMoveSpeed * speedMulti / 2) * Time.deltaTime;
            if (!movementRef.isFlying)
            {
                direction.y = 0;
            }
            movementRef.controller.Move(direction);
        }

        //Convert Range Helper
        public float ConvertRange(
               float originalStart, float originalEnd,
               float newStart, float newEnd,
               float value)
        {

            float originalDiff = originalEnd - originalStart;
            float newDiff = newEnd - newStart;
            float ratio = newDiff / originalDiff;
            float newProduct = value * ratio;
            float finalValue = newProduct + newStart;
            return finalValue;

        }

    }
}