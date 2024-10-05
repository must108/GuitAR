using UnityEngine.Events;
using UnityEngine;

using System;

namespace Core3lb
{
    public abstract class BaseXRGrabObject : MonoBehaviour
    {
        [CoreReadOnly]
        //Bool to check if something is Grabbed
        public bool isGrabbed;
        //Allows for hovering objects that can be thrown
        public bool kinematicOnDrop = true; 
        [CoreReadOnly]
        public XRHand currentHand;

        [CoreReadOnly]
        public Rigidbody body;

        [HideInInspector]
        public bool isLocked = false;

        [CoreToggleHeader("Show Events")]
        public bool showEvents;
        [CoreShowIf("showEvents")]
        public UnityEvent onEnter; //Hover
        [CoreShowIf("showEvents")]
        public UnityEvent onExit; //UnHover
        [CoreShowIf("showEvents")]
        public UnityEvent onGrab; //Grab
        [CoreShowIf("showEvents")]
        public UnityEvent onDrop; //Ungrab

        #pragma warning disable CS0067
        public event Action onEnterAction;
        public event Action onExitAction;
        public event Action onGrabAction;
        public event Action onDropAction;
        #pragma warning restore CS0067
        private UnityEvent handForced;

        //Main Events
        public abstract void ForceDrop();
        public abstract void ForceGrab(XRHand selectedHand);

        //write a get for body
        public Rigidbody myBody
        {
            get
            {
                body = gameObject.GetComponentIfNull<Rigidbody>(body);
                return body;
            }
        }

        public void _SetHand(XRHand hand)
        {
            currentHand = hand;
        }

        public void _SetKinematicOnDrop(bool to)
        {
            kinematicOnDrop = to;
        }

        //Event Functions
        public virtual void Grab()
        {
            onGrab?.Invoke();
        }

        public virtual void Drop()
        {
            onDrop?.Invoke();
        }

        public virtual void EnterEvent()
        {
            onEnter?.Invoke();
        }

        public virtual void ExitEvent()
        {
            onExit?.Invoke();
        }

    }
}
