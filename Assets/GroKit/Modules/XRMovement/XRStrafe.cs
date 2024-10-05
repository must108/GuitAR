using UnityEngine;
using UnityEngine.InputSystem;

namespace Core3lb
{
    public class XRStrafe : MonoBehaviour
    {
        [CoreEmphasize(true)]
        [Tooltip("takes the default movement speed and * it with this")]
        public float speedMulti = 1;
        [CoreRequired]
        public XRMovement movementRef;
        [CoreRequired]
        public InputActionReference movementAxis;
        [Tooltip("Use controller instead of foward direction of head")]
        public bool useController = false;
        [CoreShowIf("useController")]
        public InputXR.Controller controller;

        void Start()
        {
            movementRef = gameObject.GetComponentIfNull<XRMovement>(movementRef);
        }

        private void Update()
        {
            if (movementRef.canMove)
            {
                FPSMove();
            }
        }

        public virtual Vector2 GetInput()
        {
            return InputXR.GetVector2FromAction(movementAxis);
        }

        void FPSMove()
        {
            var holder = GetInput();
            Vector3 moveDirection = new Vector3(holder.x, 0, holder.y);
            moveDirection *= movementRef.defaultMoveSpeed * speedMulti * Time.deltaTime;
            if (useController)
            {
                switch (controller)
                {
                    case InputXR.Controller.Left:
                        moveDirection = movementRef.leftHand.transform.TransformDirection(moveDirection);
                        break;
                    case InputXR.Controller.Right:
                        moveDirection = movementRef.rightHand.transform.TransformDirection(moveDirection);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                moveDirection = movementRef.head.transform.TransformDirection(moveDirection);
            }
            movementRef.ApplyAndMove(moveDirection);
        }
    }
}

