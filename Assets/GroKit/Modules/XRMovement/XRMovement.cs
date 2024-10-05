using UnityEngine;


namespace Core3lb
{
    public class XRMovement : MonoBehaviour
    {
        [SerializeField]
        [CoreRequired]
        public CharacterController charController;
        public bool canMove = true;
        [Tooltip("Flying will take into account moving in the Y direction floating upwards you can still go up slopes like a character controller however")]
        public bool isFlying = true; 
        public float defaultMoveSpeed = 5;
        public float playerGravity = 10;
        [CoreReadOnly]
        public Vector3 moveDirection;

        //These many want to be altered to get different movement styles
        [CoreRequired]
        public Transform leftHand;
        [CoreRequired]
        public Transform rightHand;
        [CoreRequired]
        public Transform head;

        [Tooltip("Context Blocking disables movement input when hands are inside of a XRGrabObject or XRContextBlocker an object allowing use of move and grab as the same button")]
        public bool useContextBlocking;

        public bool isContextBlocked(InputXR.Controller whichHand)
        {
            if(!useContextBlocking)
            {
                return false;
            }
            if(whichHand == InputXR.Controller.Left)
            {
                return XRPlayerController.instance.leftXRHand.isContextBlocked;
            }
            if(whichHand == InputXR.Controller.Right)
            {
                return XRPlayerController.instance.rightXRHand.isContextBlocked;
            }
            return false;
        }


        public CharacterController controller
        {
            get { return charController; }
        }

        public void DoMovement()
        {
            if (canMove)
            {
                ApplyMovement();
            }
        }

        public void AddMoveDirection(Vector3 add)
        {
            moveDirection += add;
        }

        void ApplyMovement()
        {
            if (!isFlying)
            {
                moveDirection.y += GetGravity();
            }
            charController.Move(moveDirection);
        }

        public void ApplyAndMove(Vector3 direction)
        {
            moveDirection = direction;
            if (!isFlying)
            {
                moveDirection.y += GetGravity();
            }
            charController.Move(moveDirection);
        }

        public float GetGravity()
        {
            return -playerGravity * Time.deltaTime;
        }

        public float GetHeight()
        {
            float holder;
            if (charController.height > charController.radius)
            {
                holder = charController.height;
            }
            else
            {
                holder = charController.radius / 2;
            }
            return holder;
        }


        #region Moving Player Safely
        public void _MovePlayerSafely(Transform where)
        {
            charController.enabled = false;
            charController.transform.position = where.position;
            charController.transform.rotation = where.rotation;
            charController.enabled = true;
        }

        public void _MovePlayerSafely(Vector3 pos)
        {
            charController.enabled = false;
            charController.transform.position = pos;
            charController.enabled = true;
        }

        public void _RotatePlayerSafely(Quaternion rot)
        {
            charController.enabled = false;
            charController.transform.rotation = rot;
            charController.enabled = true;
        }
        #endregion
    }

}
