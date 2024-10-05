using UnityEngine;


namespace Core3lb
{
    public class FakeParent : MonoBehaviour
    {
        [CoreHeader("Settings")]
        [CoreRequired]
        public Transform parentTransform;
        public bool parentOnStart;

        [Tooltip("If using Zeroing ")]
        [CoreReadOnly]
        public bool isParented;

        [CoreToggleHeader("Position")]
        public bool followPosition = true;
        [CoreShowIf("followPosition")]
        [Tooltip("Instant Follow runs on update so it may have issues with networking")]
        public bool instantFollowPos = true;
        [CoreShowIf("followPosition")] 
        public float positionTime = .5f;
        [CoreShowIf("followPosition")] 
        public bool ignoreX = false;
        [CoreShowIf("followPosition")] 
        public bool ignoreY = false;
        [CoreShowIf("followPosition")] 
        public bool ignoreZ = false;

        [CoreToggleHeader("Relative Rotation")]
        public bool followRelativeRotation = true;
        [Tooltip("If using Zeroing ")]
        [CoreShowIf("followRelativeRotation")]
        public bool instantFollowRot = true;
        [CoreShowIf("followRelativeRotation")]
        public float rotationTime = .5f;
        [CoreShowIf("followRelativeRotation")]
        public bool zeroX = false;
        [CoreShowIf("followRelativeRotation")]
        public bool zeroY = false;
        [CoreShowIf("followRelativeRotation")]
        public bool zeroZ = false;
        private Vector3 velocity = Vector3.zero;
        private Quaternion velQuat = Quaternion.identity;
        private Vector3 childEuler;


        Vector3 startParentPosition;
        Quaternion startParentRotationQ;
        Vector3 startParentScale;
        Vector3 startChildPosition;
        Quaternion startChildRotationQ;
        Matrix4x4 parentMatrix;

        public virtual void Start()
        {
            if (parentOnStart)
            {
                _StartParent();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (isParented)
            {
                parentMatrix = Matrix4x4.TRS(parentTransform.position, parentTransform.rotation, parentTransform.lossyScale);
                var pos = parentMatrix.MultiplyPoint3x4(startChildPosition);
                var quat = (parentTransform.rotation * Quaternion.Inverse(startParentRotationQ)) * startChildRotationQ;

                if(!instantFollowPos)
                {
                    transform.position = PositionSolver(pos);
                }
                transform.rotation = RotationSolver(quat);
            }
        }

        protected virtual void Update()
        {
            if (isParented)
            {
                if(instantFollowPos)
                {
                    parentMatrix = Matrix4x4.TRS(parentTransform.position, parentTransform.rotation, parentTransform.lossyScale);
                    var pos = parentMatrix.MultiplyPoint3x4(startChildPosition);
                    var quat = (parentTransform.rotation * Quaternion.Inverse(startParentRotationQ)) * startChildRotationQ;
                    transform.position = PositionSolver(pos);
                }
            }
        }

        public virtual Vector3 PositionSolver(Vector3 position)
        {
            if (!followPosition)
            {
                return transform.position;
            }
            Vector3 targetPosition = position;
            // Apply the followX, followY, followZ constraints
            if (ignoreX) targetPosition.x = transform.position.x;
            if (ignoreY) targetPosition.y = transform.position.y;
            if (ignoreZ) targetPosition.z = transform.position.z;

            if (instantFollowPos)
            {
                return targetPosition;
            }
            else
            {
                return Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, positionTime);
            }
        }

        public virtual Quaternion RotationSolver(Quaternion quat)
        {
            if (!followRelativeRotation)
            {
                return transform.rotation;
            }

            quat = AdjustQuaternion(quat);

            if (instantFollowRot)
            {
                return quat;
            }
            else
            {
                return transform.rotation.SmoothDamp(quat,ref velQuat,rotationTime, Time.fixedDeltaTime);
            }
        }


        protected virtual Quaternion AdjustQuaternion(Quaternion quat)
        {
            Vector3 eulerAngles = quat.eulerAngles;

            if (zeroX) eulerAngles.x = 0;
            if (zeroY) eulerAngles.y = 0;
            if (zeroZ) eulerAngles.z = 0;

            return Quaternion.Euler(eulerAngles);
        }

        protected virtual Quaternion RotateTowardsMethod4(Quaternion from, Quaternion to, float maxDegreesDelta)
        {
            float angle = Quaternion.Angle(from, to);
            if (angle == 0f) return to;
            float t = Mathf.Min(1f, maxDegreesDelta / angle);
            return Quaternion.SlerpUnclamped(from, to, t);
        }

        float NormalizeAngle(float angle)
        {
            angle = angle % 360;
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }

        protected virtual Quaternion RotateTowardsQuaternion(Quaternion current, Quaternion target, float rotationSpeed, bool instantFollowRot)
        {
            if (instantFollowRot)
            {
                return target;
            }
            else
            {
                Quaternion result = Quaternion.RotateTowards(current, target, rotationSpeed * Time.deltaTime);
                Debug.LogError($"{target.eulerAngles} == {result.eulerAngles}");
                return result;
            }
        }


        [CoreButton]
        public virtual void _StartParent()
        {
            if (parentTransform == null)
            {
                Debug.LogError("No Parent to follow Assigned");
                return;
            }
            isParented = true;
            startParentPosition = parentTransform.position;
            startParentRotationQ = parentTransform.rotation;
            startParentScale = parentTransform.lossyScale;
            startChildPosition = transform.position;
            startChildRotationQ = transform.rotation;
            childEuler = transform.rotation.eulerAngles;

            // Keeps child position from being modified at the start by the parent's initial transform
            startChildPosition = DivideVectors(Quaternion.Inverse(parentTransform.rotation) * (startChildPosition - startParentPosition), startParentScale);
        }

        public virtual void _SetParent(Transform parent)
        {
            parentTransform = parent;
        }

        [CoreButton]
        public virtual void _StopParent()
        {
            isParented = false;
        }

        [CoreButton]
        public virtual void _ResumeParent()
        {
            isParented = true;
        }

        public virtual void GetParentTransform(out Vector3 position, out Quaternion rotation)
        {
            parentMatrix = Matrix4x4.TRS(parentTransform.position, parentTransform.rotation, parentTransform.lossyScale);
            var pos = parentMatrix.MultiplyPoint3x4(startChildPosition);
            var quat = transform.rotation = (parentTransform.rotation * Quaternion.Inverse(startParentRotationQ)) * startChildRotationQ;
            position = PositionSolver(pos);
            rotation = RotationSolver(quat);
        }

        Vector3 DivideVectors(Vector3 num, Vector3 den)
        {
            return new Vector3(num.x / den.x, num.y / den.y, num.z / den.z);
        }
    }
}