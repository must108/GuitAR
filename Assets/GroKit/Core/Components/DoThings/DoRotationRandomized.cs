using UnityEngine;

namespace Core3lb
{
    public class DoRotationRandomized : DoRotation
    {
        Vector3 speed;

        [Header("Randomize Direction")]
        public bool randomValues;
        [CoreShowIf("randomValues")]
        public bool randomX, randomY, randomZ;
        [CoreShowIf("randomValues")]
        public Vector2 timeRange = new Vector2(2, 4);
        [CoreShowIf("randomValues")]
        public Vector2 speedModRange = new Vector2(-2, 2);

        public bool flipOnRandomize = true;
        int flipper = 1;

        [CoreReadOnly]
        public Vector3 desiredValues;

        Quaternion originalRot;

        private void Start()
        {
            speed = rotateDirection * rotateSpeed;
            if (flipOnRandomize)
            {
                flipper = -1;
            }
            originalRot = transform.rotation;
            if (randomValues)
            {
                desiredValues = speed;
                InvokeRepeating(nameof(SetRandomValues), 0, Random.Range(timeRange.x, timeRange.y));
                speed = desiredValues;
            }
        }

        public override void FixedUpdate()
        {
            if (isRotating)
            {
                if (randomValues)
                {
                    speed = Vector3.Lerp(speed, desiredValues, Time.deltaTime);
                }
                transform.Rotate(speed * Time.deltaTime);
            }
        }

        public override void _SetSpeed(float newSpeed)
        {
            rotateSpeed = newSpeed;
            speed = rotateDirection * rotateSpeed;
            if (randomValues)
            {
                CancelInvoke();
                desiredValues = speed;
                InvokeRepeating(nameof(SetRandomValues), 0, Random.Range(timeRange.x, timeRange.y));
                speed = desiredValues;
            }
        }

        public void SetRandomValues()
        {
            if (randomX)
            {
                if (Random.value >= .5f)
                {
                    desiredValues.x = speed.x * flipper + Random.Range(speedModRange.x, speedModRange.y);
                }
            }
            if (randomY)
            {
                if (Random.value >= .5f)
                {
                    desiredValues.y = speed.y * flipper + Random.Range(speedModRange.x, speedModRange.y);
                }
            }
            if (randomZ)
            {
                if (Random.value >= .5f)
                {
                    desiredValues.z = speed.z * flipper + Random.Range(speedModRange.x, speedModRange.y);
                }
            }
        }

        public void _Reset()
        {
            transform.rotation = originalRot;
        }
    }
}
