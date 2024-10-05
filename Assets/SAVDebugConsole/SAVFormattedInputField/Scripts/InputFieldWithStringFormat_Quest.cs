//SAV Formatted Input Field - Copyright 2024 ShadowAndVVWorkshop

using UnityEngine;
//using UnityEngine.UI;

namespace ShadowAndVVWorkshop.SAVFormattedInputField
{
    public class InputFieldWithStringFormat_Quest : InputFieldWithStringFormat
    {
        [Tooltip("OVRVirtualKeyboard to used. None for TouchScreenKeyboard")]
        [SerializeField] private OVRVirtualKeyboard _OVRVirtualKeyboard;
        public OVRVirtualKeyboard OVRVirtualKeyboard { get { return _OVRVirtualKeyboard; } set { _OVRVirtualKeyboard = value; } }
        [SerializeField] public CustomOVRVirtualKeyboardInputFieldTextHandler oVRVirtualKeyboardInputFieldTextHandler;

        protected override void HandleStart()
        {
            if (_OVRVirtualKeyboard == null)
            {
                base.HandleStart();   // check for system keyboard
            }
            else
            {
                //Debug.Log("Start Activekeyboard.OVRVirtualKeyboard");
                _Activekeyboard = Activekeyboard.OVRVirtualKeyboard;
                oVRVirtualKeyboardInputFieldTextHandler.keyboard = _OVRVirtualKeyboard;
                oVRVirtualKeyboardInputFieldTextHandler.Initialize();
                // to get the gltf's loaded correctly, show 
                _OVRVirtualKeyboard.gameObject.SetActive(true);
                // hide it here
                if (_OVRVirtualKeyboard.gameObject.activeSelf)
                    _OVRVirtualKeyboard.gameObject.SetActive(false);

                oVRVirtualKeyboardInputFieldTextHandler.EndEdit.AddListener(OnCustomTouchScreenKeyboardInputFieldTextHandlerEndEdit);
            }
        }

        void OnDisable()
        {
            //Debug.Log("OVRVirtualKeyboard OnDisable");
            if (OVRVirtualKeyboard != null)
                oVRVirtualKeyboardInputFieldTextHandler.UnbindKeyboard();
            //OnSavEndEdit?.Invoke(prev_text, prev_value);
        }
    }
}