//SAV Formatted Input Field - Copyright 2024 ShadowAndVVWorkshop

//https://communityforums.atmeta.com/t5/Unity-VR-Development/How-to-Show-and-Hide-the-OVRVirtualKeyboard-Building-Block/td-p/1107237

using UnityEngine.Events;
using UnityEngine.UI;

namespace ShadowAndVVWorkshop.SAVFormattedInputField
{
    public class CustomOVRVirtualKeyboardInputFieldTextHandler : OVRVirtualKeyboardInputFieldTextHandler
    {
        public UnityEvent BeginEdit = new UnityEvent();
        public UnityEvent EndEdit = new UnityEvent();

        public OVRVirtualKeyboard keyboard;
        public bool isBound = false;

        public void Initialize()
        {
            keyboard.KeyboardHiddenEvent.AddListener(Keyboard_KeyboardHidden);
            keyboard?.EnterEvent.AddListener(KeyboardEnter);
            InputField = GetComponent<InputField>();
        }

        private void Keyboard_KeyboardHidden()
        {
            //Debug.Log("Keyboard_KeyboardHidden");
            if (isBound)
                UnbindKeyboard();
        }

        void OnDisable()
        {
            if (isBound)
                UnbindKeyboard();
        }

        public void Update()
        {
            if (IsFocused && !isBound)
            {
                BindKeyboard();
            }
            if (isBound && keyboard != null && keyboard.TextHandler != this)
            {
                //Debug.Log("OVRVirtualKeyboard update " + (keyboard.TextHandler != this).ToString());
                UnbindKeyboard();
            }
        }

        protected void KeyboardEnter()
        {
            if (isBound)
                UnbindKeyboard();
        }

        protected void BindKeyboard()
        {
            //Debug.Log("OVRVirtualKeyboard BindKeyboard");
            if (keyboard != null)
            {
                keyboard.TextHandler = this;
                //Debug.Log("OVRVirtualKeyboard BindKeyboard keyboard.TextHandler " + keyboard.TextHandler);
                keyboard.gameObject.SetActive(true);
                keyboard.enabled = true;
                isBound = true;
                //Debug.Log("OVRVirtualKeyboard BeginEdit?.Invoke ");
                BeginEdit?.Invoke();
            }
        }

        public void UnbindKeyboard()
        {
            //Debug.Log("OVRVirtualKeyboard UNBindKeyboard 1");
            InputField?.DeactivateInputField();
            if (keyboard != null)
            {
                if (keyboard.TextHandler == this)
                {
                    //Debug.Log("OVRVirtualKeyboard UNBindKeyboard keyboard.TextHandler == this");
                    keyboard.TextHandler = null;
                    keyboard.enabled = false;   // do not SetActive(false)
                }
            }
            isBound = false;
            EndEdit?.Invoke();
        }
    }
}