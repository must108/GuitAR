using System.Collections.Generic;
using UnityEngine;

namespace Core3lb
{
    public class XRHand : MonoBehaviour
    {
        public InputXR.Controller controller;
        public Transform handVisual;
        public Transform controllerVisual;
        public SkinnedMeshRenderer skinnedMesh;
        [Tooltip("DefaultGrabPoint for spawning things in hand")]
        public Transform defaultGrabPoint;
        
        public BaseXRGrabObject currentHeldObject;
        [HideInInspector]
        public InputProcessorCore GrabProcessor = new InputProcessorCore();
        [HideInInspector]
        public InputProcessorCore InteractProcessor = new InputProcessorCore();
        public Collider myCollider;
        public float sphereCheckRadius = .1f;

        [HideInInspector]
        public bool isOverridden;

        [CoreReadOnly]
        public List<GameObject> blockingObjects = new List<GameObject>();

        public void Awake()
        {
            myCollider = gameObject.GetComponentIfNull<Collider>(myCollider);
            if (defaultGrabPoint == null)
            {
                defaultGrabPoint = transform;
            }
        }

        public virtual void Update()
        {
            if(isOverridden)
            {
                return;
            }
            GrabProcessor.Process(GetGrab());
            InteractProcessor.Process(GetInteract());
        }

        protected virtual bool GetGrab()
        {
            return InputXR.instance.GetGrab(controller);
        }

        protected virtual bool GetInteract()
        {
            return InputXR.instance.GetInteract(controller);
        }

        public bool SphereCheck(GameObject whatObject)
        {
            // Collect all colliders within our Obstacle Check Radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, sphereCheckRadius);
            // Go through each collider collected
            foreach (Collider col in colliders)
            {
                if (col.gameObject == whatObject)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isContextBlocked
        {
            get
            {
                if (blockingObjects.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(isContextBlocker(other))
            {
                blockingObjects.AddIf(other.gameObject);
                blockingObjects.RemoveNullsOrInActive();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isContextBlocker(other))
            {
                blockingObjects.RemoveIf(other.gameObject);
                blockingObjects.RemoveNullsOrInActive();
            }
        }

        public bool isContextBlocker(Collider other)
        {
            if (other.TryGetComponent(out BaseXRGrabObject baseGrabObject))
            {
                return true;
            }
            if (other.TryGetComponent(out XRContextBlock contextBlock))
            {
                return true;
            }
            return false;
        }



        public void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, sphereCheckRadius);
        }
    }
}
