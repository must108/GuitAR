using UnityEngine;

namespace Core3lb
{
    public class FollowTarget : MonoBehaviour
    {
        [CoreRequired]
        public Transform target;
        [CoreHeader("Settings")]
        Vector3 positionOffset;
        Quaternion rotationOffset = Quaternion.identity;
        
        public bool followOnStart = false;
        [CoreShowIf("followOnStart")]
        public bool offsetPosOnStart = false;
        [CoreShowIf("followOnStart")]
        public bool offsetRotOnStart = false;


        [CoreToggleHeader("Position")]
        public bool followPosition = true;
        [CoreShowIf("followPosition")]
        public bool instantFollowPos = false;
        [CoreShowIf("followPosition"),]
        public float positionTime = .5f;
        [CoreShowIf("followPosition")]
        public bool followX = true;
        [CoreShowIf("followPosition")]
        public bool followY = true;
        [CoreShowIf("followPosition"),]
        public bool followZ = true;


        [CoreToggleHeader("Rotation")]
        public bool followRotation = true;
        [CoreShowIf("followRotation")]
        public bool instantFollowRot = false;
        [CoreShowIf("followRotation")]
        public float rotationTime = .5f;
        [CoreShowIf("followRotation")]
        public bool followXrot = true;
        [CoreShowIf("followRotation")]
        public bool followYrot = true;
        [CoreShowIf("followRotation")]
        public bool followZrot = true;

        [Tooltip("Z Will be foward for look at")]
        public bool lookAtTarget = false;


        private bool isFollowing = false;
        private Vector3 velocity = Vector3.zero;
        private Quaternion velQuat = Quaternion.identity;

        public virtual void Start()
        {
            if (followOnStart)
            {
                if (offsetPosOnStart)
                {
                    _StartFollowWithOffset();
                }
                else
                {
                    _StartFollow();
                }

            }
        }

        void FixedUpdate()
        {
            if (isFollowing && target)
            {
                if(!instantFollowPos)
                {
                    transform.position = PositionSolver();
                }

                transform.rotation = RotationSolver();
            }
        }

        void Update()
        {
            if (isFollowing && target)
            {
                if (instantFollowPos)
                {
                    transform.position = PositionSolver();
                }
            }
        }

        public virtual Vector3 PositionSolver()
        {
            if (!followPosition)
            {
                return transform.position;
            }

            Vector3 targetPosition = target.position + positionOffset;

            // Apply the followX, followY, followZ constraints
            if (!followX) targetPosition.x = transform.position.x;
            if (!followY) targetPosition.y = transform.position.y;
            if (!followZ) targetPosition.z = transform.position.z;

            if (instantFollowPos)
            {
                return targetPosition;
            }
            else
            {
                return Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, positionTime);
            }
        }

        public virtual Quaternion RotationSolver()
        {
            Quaternion result;
            if (!followRotation)
            {
                return transform.rotation;
            }
            if (lookAtTarget)
            {
                Vector3 direction = target.position - transform.position;
                result = Quaternion.LookRotation(direction);
            }
            else
            {
                if(rotationOffset == Quaternion.identity)
                {
                    result = target.rotation;
                }
                else
                {
                    result = target.rotation * rotationOffset;
                }

            }
            // Apply the followX, followY, followZ constraints
            Vector3 eulerResult = result.eulerAngles;
            Vector3 currentEuler = transform.rotation.eulerAngles;

            result = Quaternion.Euler(
                followXrot ? eulerResult.x : currentEuler.x,
                followYrot ? eulerResult.y : currentEuler.y,
                followZrot ? eulerResult.z : currentEuler.z
            );

            if (instantFollowRot)
            {
                return result;
            }
            else
            {
                return transform.rotation.SmoothDamp(result, ref velQuat, rotationTime, Time.deltaTime);
            }
        }

        public void _ChangeTarget(Transform myTarget)
        {
            target = myTarget;
        }

        [CoreButton]
        public void _StartFollow()
        {
            isFollowing = true;
        }
        [CoreButton]
        public void _StopFollow()
        {
            isFollowing = false;
        }
        [CoreButton]
        public void _StartFollowWithOffset()
        {
            positionOffset = transform.position - target.position;
            if(offsetRotOnStart)
            {
                rotationOffset = Quaternion.Inverse(target.rotation) * transform.rotation;
            }
            else
            {
                rotationOffset = Quaternion.identity;
            }
            _StartFollow();
        }
    }
}