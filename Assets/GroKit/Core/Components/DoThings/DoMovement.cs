using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class DoMovement : MonoBehaviour
    {
        [Tooltip("Target for that moves")]
        [CoreEmphasize]
        public Transform objectToMove;
        [CoreEmphasize]
        [CoreToggleHeader("Use Time")]
        public bool useTime = true;
        [CoreShowIf("useTime")]
        [Tooltip("In seconds")]
        public float moveTime = 1;
        [CoreShowIf("useTime")]
        [Tooltip("In seconds")]
        public float rotateTime = 1;
        [CoreShowIf("useTime")]
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1); //Defaults to a Linear Curve
        [CoreHideIf("useTime")]
        public float moveSpeed = 2;
        [CoreHideIf("useTime")]
        public float rotateSpeed = 2;
        [Space]
        public UnityEvent tweenDone;
        Coroutine moveTween;
        Coroutine rotateTween;

        [CoreToggleHeader("Test Movement")]
        public bool debugMovement = false;
        [CoreShowIf("debugMovement")]
        public UnityEvent testMovement;





        public virtual void Start()
        {
            objectToMove = gameObject.GetComponentIfNull<Transform>(objectToMove);
        }

        public virtual void _MoveObjectHere(Transform WhereTo)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);

            }
            moveTween = StartCoroutine(MoveToTarget(transform.position, WhereTo.position));
        }

        public virtual void _TeleportObject(Transform WhereTo)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);

            }
            transform.position = WhereTo.position;
        }

        public virtual void _TeleportObjectWithRotation(Transform WhereTo)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);

            }
            transform.position = WhereTo.position;
            transform.rotation = WhereTo.rotation;
        }

        public virtual void _ChangeRotateSpeed(float chg)
        {
            rotateTime = chg;
        }

        public virtual void _ChangeMoveSpeed(float chg)
        {
            moveTime = chg;
        }

        [CoreButton]
        protected void TestMovement()
        {
            testMovement.Invoke();
        }

        //World Move BY

        public virtual void _MoveByX(float x)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);

            }
            moveTween = StartCoroutine(MoveToTarget(objectToMove.transform.position, objectToMove.transform.position + Vector3.right * x));
        }

        public virtual void _MoveByY(float y)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);
            }
            moveTween = StartCoroutine(MoveToTarget(objectToMove.transform.position, objectToMove.transform.position + Vector3.up * y));
        }

        public virtual void _MoveByZ(float z)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);
            }
            moveTween = StartCoroutine(MoveToTarget(objectToMove.transform.position, objectToMove.transform.position + Vector3.forward * z));
        }

        //Local Move By

        public virtual void _LocalMoveByX(float x)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);

            }
            moveTween = StartCoroutine(MoveToTarget(objectToMove.transform.position, objectToMove.transform.position + objectToMove.transform.TransformDirection(Vector3.right * x)));
        }

        public virtual void _LocalMoveByY(float y)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);
            }
            moveTween = StartCoroutine(MoveToTarget(objectToMove.transform.position, objectToMove.transform.position + objectToMove.transform.TransformDirection(Vector3.up * y)));
        }

        public virtual void _LocalMoveByZ(float z)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);
            }
            moveTween = StartCoroutine(MoveToTarget(objectToMove.transform.position, objectToMove.transform.position + objectToMove.transform.TransformDirection(Vector3.forward * z)));
        }

        public virtual void _LocalMoveToX(float x)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);

            }
            Vector3 holder = transform.localPosition;
            holder.x = x;
            moveTween = StartCoroutine(MoveToTarget(transform.localPosition, holder, true));
        }

        public virtual void _LocalMoveToY(float y)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);
            }
            Vector3 holder = transform.localPosition;
            holder.y = y;
            moveTween = StartCoroutine(MoveToTarget(transform.localPosition, holder, true));
        }

        public virtual void _LocalMoveToZ(float z)
        {
            if (moveTween != null)
            {
                StopCoroutine(moveTween);
            }
            Vector3 holder = transform.localPosition;
            holder.z = z;
            moveTween = StartCoroutine(MoveToTarget(transform.localPosition, holder, true));
        }

        //Rotation

        public virtual void _RotateObjectHere(Quaternion targetRotation)
        {
            if (rotateTween != null)
            {
                StopCoroutine(rotateTween);
            }
            rotateTween = StartCoroutine(RotateToTarget(transform.rotation, targetRotation));
        }

        //World Rotate By

        public virtual void _RotateByX(float angle)
        {
            if (rotateTween != null)
            {
                StopCoroutine(rotateTween);
            }
            rotateTween = StartCoroutine(RotateToTarget(transform.rotation, transform.rotation * Quaternion.Euler(angle, 0, 0)));
        }

        public virtual void _RotateByY(float angle)
        {
            if (rotateTween != null)
            {
                StopCoroutine(rotateTween);
            }
            rotateTween = StartCoroutine(RotateToTarget(transform.localRotation, transform.rotation * Quaternion.Euler(0, angle, 0)));
        }

        public virtual void _RotateByZ(float angle)
        {
            if (rotateTween != null)
            {
                StopCoroutine(rotateTween);
            }
            rotateTween = StartCoroutine(RotateToTarget(transform.localRotation, transform.rotation * Quaternion.Euler(0, 0, angle)));
        }

        //World Rotate To

        public virtual void _RotateToX(float angle)
        {
            if (rotateTween != null)
            {
                StopCoroutine(rotateTween);
            }
            Vector3 holder = transform.rotation.eulerAngles;
            holder.x = angle;
            rotateTween = StartCoroutine(RotateToTarget(transform.localRotation, Quaternion.Euler(holder)));
        }

        public virtual void _RotateToY(float angle)
        {
            if (rotateTween != null)
            {
                StopCoroutine(rotateTween);
            }
            Vector3 holder = transform.rotation.eulerAngles;
            holder.y = angle;
            rotateTween = StartCoroutine(RotateToTarget(transform.localRotation, Quaternion.Euler(holder)));
        }

        public virtual void _RotateToZ(float angle)
        {
            if (rotateTween != null)
            {
                StopCoroutine(rotateTween);
            }
            Vector3 holder = transform.rotation.eulerAngles;
            holder.z = angle;
            rotateTween = StartCoroutine(RotateToTarget(transform.localRotation, Quaternion.Euler(holder)));
        }

        //Local Rotate To

        public virtual void _LocalRotateToX(float angle)
        {
            if (rotateTween != null)
            {
                StopCoroutine(rotateTween);
            }
            Vector3 holder = transform.localRotation.eulerAngles;
            holder.x = angle;
            rotateTween = StartCoroutine(RotateToTarget(transform.localRotation, Quaternion.Euler(holder), true));
        }

        public virtual void _LocalRotateToY(float angle)
        {
            if (rotateTween != null)
            {
                StopCoroutine(rotateTween);
            }
            Vector3 holder = transform.localRotation.eulerAngles;
            holder.y = angle;
            rotateTween = StartCoroutine(RotateToTarget(transform.localRotation, Quaternion.Euler(holder), true));
        }

        public virtual void _LocalRotateToZ(float angle)
        {
            if (rotateTween != null)
            {
                StopCoroutine(rotateTween);
            }
            Vector3 holder = transform.localRotation.eulerAngles;
            holder.z = angle;
            rotateTween = StartCoroutine(RotateToTarget(transform.localRotation, Quaternion.Euler(holder), true));
        }


        protected virtual IEnumerator MoveToTarget(Vector3 startPosition, Vector3 targetPosition, bool useLocal = false)
        {
            Vector3 currentPosition;
            if (useTime)
            {
                float elapsedTime = 0f;
                while (elapsedTime < moveTime)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / moveTime;
                    currentPosition = Vector3.Lerp(startPosition, targetPosition, curve.Evaluate(t));
                    if (useLocal)
                    {
                        transform.localPosition = currentPosition;
                    }
                    else
                    {
                        transform.position = currentPosition;
                    }
                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                while (Vector3.Distance(useLocal ? transform.localPosition : transform.position, targetPosition) > 0.01f)
                {
                    currentPosition = Vector3.MoveTowards(useLocal ? transform.localPosition : transform.position, targetPosition, (moveSpeed * 5) * Time.deltaTime);
                    if (useLocal)
                    {
                        transform.localPosition = currentPosition;
                    }
                    else
                    {
                        transform.position = currentPosition;
                    }
                    yield return new WaitForFixedUpdate();
                }
            }
            tweenDone.Invoke();
        }

        protected virtual IEnumerator RotateToTarget(Quaternion startRotation, Quaternion targetRotation, bool useLocal = false)
        {
            float elapsedTime = 0f;
            if (!useTime)
            {
                while (Quaternion.Angle(useLocal ? objectToMove.localRotation : objectToMove.rotation, targetRotation) > 0.01f)
                {
                    Quaternion holder = Quaternion.RotateTowards(objectToMove.localRotation, targetRotation, rotateSpeed * 10 * Time.deltaTime);
                    if (useLocal)
                    {
                        objectToMove.localRotation = holder;
                    }
                    else
                    {
                        objectToMove.rotation = holder;
                    }
                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                while (elapsedTime < rotateTime)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / rotateTime;
                    Quaternion currentRotation = Quaternion.Lerp(startRotation, targetRotation, curve.Evaluate(t));
                    if (useLocal)
                    {
                        objectToMove.localRotation = currentRotation;
                    }
                    else
                    {
                        objectToMove.rotation = currentRotation;
                    }
                    yield return new WaitForFixedUpdate();
                }
            }
            tweenDone.Invoke();
        }
    }
}
