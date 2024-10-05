using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class UsableGroKitXRGrabObject : GroKitXRGrabObject
    {
        [CoreHeader("UsableEvent")]
        public UnityEvent OnUse;

        public void _Use()
        {
            if(isGrabbed)
            {
                Debug.LogError("INTERACT IS DOWN");
                OnUse.Invoke();
            }
        }

        public virtual bool GetInteractDown()
        {
            return currentHand.InteractProcessor.isDown;
        }

        public override void Update()
        {
            base.Update();
            if (isGrabbed)
            {
                if (GetInteractDown())
                {
                    _Use();
                }
            }
        }


    }
}
