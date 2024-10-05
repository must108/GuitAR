using UnityEngine;

namespace Core3lb
{
    public class Facing : MonoBehaviour
    {
        [Tooltip("If Follow main camera this will be ignored")]
        public Transform faceTo;
        public bool followMainCamera = true;
        public bool isFacing = true;
        [Tooltip("For example text mesh pro needs to be flipped")]
        public bool flipZ;
        public float turnSpeed = 1.0f;

        public bool scaleToDistance;
        public float ScaleMultiplier = 0.5f;
        private Vector3 initialScale;
        Vector3 scaleTo;

        void Awake()
        {
            initialScale = transform.localScale;
        }

        private void FixedUpdate()
        {
            if (isFacing)
            {
                FaceObject();
            }
            if (scaleToDistance)
            {
                ScaleObject();
            }
        }

        void FaceObject()
        {
            Vector3 targetPosition;
            if (followMainCamera && Camera.main)
            {
                targetPosition = Camera.main.transform.position;
            }
            else
            {
                targetPosition = faceTo.transform.position;
            }
            if (flipZ)
            {
                targetPosition *= -1; // Reverse the target position along the Z-axis
            }

            targetPosition.y = transform.position.y; // Keep the object's y-position constant
            var targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }

        void ScaleObject()
        {
            if (followMainCamera && Camera.main)
            {
                scaleTo = Camera.main.transform.position;
            }
            else
            {
                scaleTo = faceTo.transform.position;
            }
            float distance = Vector3.Distance(scaleTo, transform.position);
            transform.localScale = Mathf.Sqrt(distance) * ScaleMultiplier * initialScale;
        }

        public void _FaceTo(GameObject loc)
        {
            faceTo = loc.transform;
            isFacing = true;
        }

        public void _FacingToggle()
        {
            isFacing = !isFacing;
        }

        public void _FacingOff()
        {
            isFacing = false;
        }

        public void _FacingOn()
        {
            isFacing = true;
        }
    }
}