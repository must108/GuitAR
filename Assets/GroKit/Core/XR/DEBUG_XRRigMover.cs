using UnityEngine;

namespace Core3lb
{
    public class DEBUG_XRRigMover : MonoBehaviour
    {
        [CoreEmphasize]
        public Transform whatToMove;
        public float speed = 3;
        public float verticalSpeed = 3;
        public float rotationSpeed = 50; // Speed of rotation
        public bool allowInBuilds = false;
        public bool headBased = true;

        public void Awake()
        {
            whatToMove = gameObject.GetComponentIfNull<Transform>(whatToMove);
        }

        void Update()
        {
            if (Application.isEditor || allowInBuilds)
            {
                float moveX = 0f;
                float moveZ = 0f;

                if (Input.GetKey(KeyCode.A))
                    moveX = -1f;
                else if (Input.GetKey(KeyCode.D))
                    moveX = 1f;

                if (Input.GetKey(KeyCode.W))
                    moveZ = 1f;
                else if (Input.GetKey(KeyCode.S))
                    moveZ = -1f;

                Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

                if (Input.GetKey(KeyCode.Q))
                {
                    moveDirection.y = -verticalSpeed;
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    moveDirection.y = verticalSpeed;
                }

                if (headBased)
                {
                    moveDirection = XRPlayerController.instance.headCamera.transform.TransformDirection(moveDirection);
                }

                whatToMove.Translate(moveDirection * speed * Time.deltaTime);

                // Add rotation
                if (Input.GetKey(KeyCode.Z))
                {
                    whatToMove.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.C))
                {
                    whatToMove.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
