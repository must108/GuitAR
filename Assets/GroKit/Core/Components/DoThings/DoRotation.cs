using UnityEngine;

namespace Core3lb
{
    public class DoRotation : MonoBehaviour
    {
        public bool isRotating;
        public Vector3 rotateDirection;
        public float rotateSpeed;

        public virtual void FixedUpdate()
        {
            if(isRotating)
            {
                transform.Rotate(rotateDirection * rotateSpeed * Time.fixedDeltaTime);
            }
        }

        public virtual void _SetSpeed(float speed)
        {
            rotateSpeed = speed;
        }

        public virtual void _AddSpeed(float speed)
        {
            rotateSpeed += speed;
        }

        public virtual void _ToggleRotate()
        {
            isRotating = !isRotating; 
        }

        public virtual void _SetRotate(bool chg)
        {
            isRotating = chg;
        }
    }
}
