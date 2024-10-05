using UnityEngine;

namespace Core3lb
{
    public class DoRigidbody : MonoBehaviour
    {
        [CoreEmphasize]
        public Rigidbody body;
        public float force;
        public float forceMulti = 100;

        [CoreHeader("OnEnable Settings")]
        public bool zeroVelocityOnEnable;
        public bool setKinematicOnEnable;
        [CoreShowIf("setKinematicOnEnable")]
        public bool setKinematicTo;

        public void Awake()
        {
            if(body == null)
            {
                body = GetComponent<Rigidbody>();   
            }
            if(body == null)
            {
                Debug.LogError("No Body Set!", gameObject);
            }
        }
        public virtual void _ZeroVelocity()
        {
            if(body)
            {
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
        }

        public virtual void _PushDirectionForward(Transform reference)
        {
            if(body)
            {
                body.AddForce(reference.forward * force * forceMulti, ForceMode.Impulse);
            }
        }

        public virtual void _ChangeForce(float chg)
        {
            force = chg;
        }

        public virtual void OnEnable()
        {
            if(zeroVelocityOnEnable)
            {
                _ZeroVelocity();
            }
        }
    }
}
