using UnityEngine;

namespace Core3lb
{
    public class DoContinuousMovement : MonoBehaviour
    {
        public bool canMove = true;
        public float speed = 10;
        [CoreEmphasize]
        public Vector3 direction = Vector3.forward;
        public bool isLocal = true; //Figure out Later
        public Transform forwardReference;
        public bool isRigidBody;
        private Rigidbody myBody;



        public void Awake()
        {
            forwardReference = gameObject.GetComponentIfNull<Transform>(forwardReference);
            if (isRigidBody)
            {
                myBody = GetComponent<Rigidbody>();
            }
        }
        public void Update()
        {
            if (canMove)
            {
                Vector3 movement = direction * speed * Time.deltaTime;
                if (isLocal)
                {
                    movement = forwardReference.TransformDirection(movement);
                }
                if (isRigidBody)
                {
                    myBody.MovePosition(myBody.position + movement);
                }
                else
                {
                    transform.Translate(movement, Space.World);
                }
            }
        }
    }
}
