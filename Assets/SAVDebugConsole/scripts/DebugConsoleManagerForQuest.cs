//SAV Debug Console - Copyright 2024 ShadowAndVVWorkshop

using System;
using UnityEngine;

namespace ShadowAndVVWorkshop.SAVDebugConsole
{
    public class DebugConsoleManagerForQuest : DebugConsoleManager
    {
        protected bool _trackHMD = true;
        protected DebugConsoleSettingsForQuest debugConsoleQuest;
        protected Transform centerEyeCamera;

        void Start()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            try
            {
                //AddAwakeConsoleEntry("INHERIT START", LogType.Log);

                base.Initialize();

                debugConsoleQuest = DebugConsole.GetComponent<DebugConsoleSettingsForQuest>();
                if (debugConsoleQuest != null)
                {
                    _trackHMD = debugConsoleQuest._trackOnStart;
                }

                centerEyeCamera = GetCenterEyeCamera();
                if (centerEyeCamera != null)
                {
                    if (_trackHMD)
                        transform.parent.parent = centerEyeCamera.transform;
                }
                else
                {
                    _trackHMD = false;
                    _pinned = true;
                }

                if (!_trackHMD)
                {
                    _pinned = true;
                }
            }
            catch (Exception e)
            {
                HandleLog(e.Message, e.StackTrace, LogType.Exception);
                // should log error to console
                // will show if debug console is initialized
                throw (e);
            }
        }

        protected override void HandleUpdate()
        {
            if (debugConsoleQuest != null)
            {
                if (OVRInput.GetDown(debugConsoleQuest._trackingOnOff))
                {
                    HandlePinClick();
                }
                if (OVRInput.GetDown(debugConsoleQuest._sizeBigSmall))
                {
                    HandleSizeClick();
                }
                if (OVRInput.GetDown(debugConsoleQuest._listDetailOnOff))
                {
                    DetailsOnClick();
                }
                if (OVRInput.Get(debugConsoleQuest._listScrollUp))
                {
                    if (ConsoleScrollWindow != null)
                    {
                        float contentHeight = ConsoleScrollWindow.content.sizeDelta.y;
                        float contentShift = 500 * 1 * Time.deltaTime;
                        ConsoleScrollWindow.verticalNormalizedPosition += contentShift / contentHeight;
                    }
                }
                if (OVRInput.Get(debugConsoleQuest._listScrollDown))
                {
                    if (ConsoleScrollWindow != null)
                    {
                        float contentHeight = ConsoleScrollWindow.content.sizeDelta.y;
                        float contentShift = 500 * -1 * Time.deltaTime;
                        ConsoleScrollWindow.verticalNormalizedPosition += contentShift / contentHeight;
                    }
                }
                if (OVRInput.GetDown(debugConsoleQuest._listClear))
                {
                    ClearOnClick();
                }
            }
        }

        private Transform GetCenterEyeCamera()
        {
            //Debug.Assert(OVRManager.instance != null, "Null OVRManager");

            if (OVRManager.instance == null)
                return null;
            OVRCameraRig oVRCameraRig = OVRManager.instance.GetComponentInChildren<OVRCameraRig>();
            if (oVRCameraRig == null)
                return null;
            return oVRCameraRig.centerEyeAnchor;
        }

        protected override void HandlePinClick()
        {
            base.HandlePinClick(); // call base first to change _pinned

            if (_pinned)
            {
                transform.parent.parent = null;
            }
            else
            {
                if (centerEyeCamera != null)
                    transform.parent.parent = centerEyeCamera.transform;
            }
        }
    }
}