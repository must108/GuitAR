//SAV Debug Console - Copyright 2024 ShadowAndVVWorkshop

using UnityEngine;

namespace ShadowAndVVWorkshop.SAVDebugConsole
{
    public class DebugConsoleSettingsForQuest : MonoBehaviour
    {

        [Tooltip("When selected the console will track the Headset/Camera at startup")]
        [SerializeField]
        public bool _trackOnStart = false;
        private bool TrackingOn { get { return _trackOnStart; } set { _trackOnStart = value; } }

        [Tooltip("Control button to turn on/off tracking")]
        [SerializeField]
        public OVRInput.RawButton _trackingOnOff;
        private OVRInput.RawButton TrackingOnOff { get { return _trackingOnOff; } set { _trackingOnOff = value; } }

        [Tooltip("Control button to grow/shrink height")]
        [SerializeField]
        public OVRInput.RawButton _sizeBigSmall;
        private OVRInput.RawButton SizeBigSmall { get { return _sizeBigSmall; } set { _sizeBigSmall = value; } }

        [Tooltip("Control button to show/hide error details")]
        [SerializeField]
        public OVRInput.RawButton _listDetailOnOff;
        private OVRInput.RawButton ListDetailOnOff { get { return _listDetailOnOff; } set { _listDetailOnOff = value; } }

        [Tooltip("Control button to scroll up list")]
        [SerializeField]
        public OVRInput.RawButton _listScrollUp;
        private OVRInput.RawButton ListScrollUp { get { return _listScrollUp; } set { _listScrollUp = value; } }

        [Tooltip("Control button to scroll down list")]
        [SerializeField]
        public OVRInput.RawButton _listScrollDown;
        private OVRInput.RawButton ListScrollDown { get { return _listScrollDown; } set { _listScrollDown = value; } }

        [Tooltip("Control button to clear list")]
        [SerializeField]
        public OVRInput.RawButton _listClear;
        private OVRInput.RawButton ListClear { get { return _listClear; } set { _listClear = value; } }

    }
}