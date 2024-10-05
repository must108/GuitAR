using UnityEngine;

namespace Core3lb
{
    public class XRDragWorld : MonoBehaviour
    {
        [CoreHeader("-Settings-")]
        [CoreRequired]
        public XRMovement movementRef;
        [Range(.1f, 50)]
        public float multiplier = 5;

        [HideInInspector]
        bool isDragL;
        bool isDragR;
        [CoreHeader("Input Settings")]
        public bool enableLeftHand = true;
        public bool enableRightHand = true;
        public bool useEventForInputs;
        [CoreHideIf("useEventForInputs")]
        public InputXR.Button movementButton = InputXR.Button.Move;

        bool leftInputEvent = false;
        bool rightInputEvent = false;

        Vector3 lastPos;
        Transform storedTransform; //Stores current Controller;

        void Start()
        {
            movementRef = gameObject.GetComponentIfNull<XRMovement>(movementRef);
        }

        private void OnDisable()
        {
            isDragL = false;
            isDragR = false;
        }
        void Update()
        {
            if (movementRef.canMove)
            {
                GrabDragL();
                GrabDragR();
                movementRef.DoMovement();
            }
        }

        public bool GetInput(InputXR.Controller controller)
        {
            if (useEventForInputs)
            {
                if (controller == InputXR.Controller.Left)
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

        //Left Drag
        public void GrabDragL()
        {
            if (!isDragL)
            {
                if (GetInput(InputXR.Controller.Left) && !movementRef.isContextBlocked(InputXR.Controller.Left))
                {
                    lastPos = movementRef.leftHand.transform.position;
                    storedTransform = movementRef.leftHand.transform;
                    isDragL = true;
                }
            }
            if (isDragL)
            {
                MoveWithController();
                if (!GetInput(InputXR.Controller.Left))
                {
                    isDragL = false;
                }
            }
        }

        //Right Drag
        public void GrabDragR()
        {
            if (!isDragR)
            {
                if (GetInput(InputXR.Controller.Right) && !movementRef.isContextBlocked(InputXR.Controller.Right))
                {
                    lastPos = movementRef.rightHand.transform.position;
                    storedTransform = movementRef.rightHand.transform;
                    isDragR = true;
                }
            }
            if (isDragR)
            {
                MoveWithController();
                if (!GetInput(InputXR.Controller.Right))
                {
                    isDragR = false;
                }
            }
        }

        public void MoveWithController()
        {
            Vector3 direction = storedTransform.position - lastPos;

            direction = direction * ((-100 * multiplier)) * Time.deltaTime;
            if (!movementRef.isFlying)
            {
                direction.y = 0;
            }
            movementRef.controller.Move(direction);
            lastPos = storedTransform.position;
        }
    }
}
