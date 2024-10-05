using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{

    public class ResettableObject : MonoBehaviour
    {
        Vector3 position;
        Quaternion rotation;
        [Tooltip("When not using Rigidbody")]
        public bool instantReset = true;
        [CoreEmphasize]
        Rigidbody bodyToReset;
        bool wasKinematic;
        [CoreEmphasize]
        [Tooltip("If reset point is not set it will set where it starts as the reset")]
        public Transform resetPoint;
        [SerializeField] bool generateResetPoint;
        public UnityEvent resetEvent;


        protected virtual void Start()
        {
            bodyToReset = GetComponent<Rigidbody>();
            if (bodyToReset)
            {
                wasKinematic = bodyToReset.isKinematic;
            }
            _SetNewSpawn();
        }

        [CoreButton]
        //Coroutine 
        public virtual void _ResetObject()
        {
            if(instantReset)
            {
                ActualReset();
                return;
            }
            StartCoroutine(WaitForFixed());
        }


        public virtual IEnumerator WaitForFixed()
        {
            yield return new WaitForFixedUpdate();
            ActualReset();
        }

        protected virtual void ActualReset()
        {
            if (bodyToReset)
            {
                bodyToReset.velocity = Vector3.zero;
                bodyToReset.angularVelocity = Vector3.zero;
                bodyToReset.isKinematic = wasKinematic;

            }
            if (resetPoint)
            {
                transform.position = resetPoint.position;
                transform.rotation = resetPoint.rotation;
            }
            else
            {
                transform.position = position;
                transform.rotation = rotation;
            }
            resetEvent.Invoke();
        }


        public virtual void _SetResetPoint(Transform where)
        {
            resetPoint = where;
        }

        [CoreButton]
        public virtual void _SetNewSpawn()
        {
            position = transform.position;
            rotation = transform.rotation;
            if (generateResetPoint)
            {
                GameObject go = new GameObject(name + "_Resetter");
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;
                go.transform.parent = transform.parent;
                resetPoint = go.transform;
            }
        }


    }
}
