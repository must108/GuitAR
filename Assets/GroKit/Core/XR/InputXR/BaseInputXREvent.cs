using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class BaseInputXREvent : MonoBehaviour
    {
        [HideInInspector]
        public bool isOverridden = false;
        public UnityEvent onGetDown;
        public UnityEvent onGetUp;

        [CoreToggleHeader("Show MultiTap Events")]
        public bool multiTap = false;
        [CoreShowIf("multiTap")]
        public UnityEvent onDoubleTap; // Event for double tap
        [CoreShowIf("multiTap")]
        public UnityEvent onTripleTap; // Event for double tap
        protected bool isInputUsed = false;
        [CoreReadOnly]
        [Tooltip("This allows you to debug your inputs")]
        public InputProcessorCore inputProcessor = new InputProcessorCore();

        [Tooltip("Force using the base XRInput instead of the override this is for multimodal modes")]
        public bool forceBase;

        protected virtual void Update()
        {
            if(isOverridden)
            {
                return;
            }
            ProcessInput(GetInput());
        }

        public void ProcessInput(bool input)
        {
            inputProcessor.Process(input);
            if (inputProcessor.isDown)
            {
                onGetDown.Invoke();
            }
            if (inputProcessor.isUp)
            {
                onGetUp.Invoke();
            }
            if (inputProcessor.isDoubleTap)
            {
                onDoubleTap.Invoke();
            }
            if (inputProcessor.isTripleTap)
            {
                onTripleTap.Invoke();
            }
        }

        public virtual bool GetInput()
        {
            Debug.LogError("GetInput() not implemented");
            return false;   
        }
    }
}
