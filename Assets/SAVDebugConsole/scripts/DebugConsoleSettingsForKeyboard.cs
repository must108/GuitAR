//SAV Debug Console - Copyright 2024 ShadowAndVVWorkshop

using UnityEngine;

namespace ShadowAndVVWorkshop.SAVDebugConsole
{
    public class DebugConsoleSettingsForKeyboard : MonoBehaviour
    {
        [Tooltip("Keyboard key to grow/shirnk height")]
        [SerializeField]
        public KeyCode _sizeBigSmall = KeyCode.Alpha1;
        private KeyCode SizeBigSmall { get { return _sizeBigSmall; } set { _sizeBigSmall = value; } }

        [Tooltip("Keyboard key to show/hide error details")]
        [SerializeField]
        public KeyCode _listDetailOnOff = KeyCode.Space;
        private KeyCode ListDetailOnOff { get { return _listDetailOnOff; } set { _listDetailOnOff = value; } }

        [Tooltip("Keyboard key to scroll up list")]
        [SerializeField]
        public KeyCode _listScrollUp = KeyCode.UpArrow;
        private KeyCode ListScrollUp { get { return _listScrollUp; } set { _listScrollUp = value; } }

        [Tooltip("Keyboard key to scroll down list")]
        [SerializeField]
        public KeyCode _listScrollDown = KeyCode.DownArrow;
        private KeyCode ListScrollDown { get { return _listScrollDown; } set { _listScrollDown = value; } }

        [Tooltip("Keyboard key to clear list")]
        [SerializeField]
        public KeyCode _listClear = KeyCode.Backspace;
        private KeyCode ListClear { get { return _listClear; } set { _listClear = value; } }

    }
}