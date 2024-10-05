//SAV Debug Console - Copyright 2024 ShadowAndVVWorkshop

using Oculus.Interaction.Surfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowAndVVWorkshop.SAVDebugConsole
{
    public class DebugConsoleManagerForQuestInteractable : DebugConsoleManagerForQuest
    {
        [SerializeField] protected Text PinButton = null;
        [SerializeField] protected Text SizeConsoleButton = null;
        [SerializeField] private GameObject SurfacePatch = null;
        [SerializeField] private float SurfacePatchDepth = .1f;

        protected override void Initialize()
        {
            try
            {
                //AddAwakeConsoleEntry("INHERIT START", LogType.Log);

                base.Initialize(); // call base first it sets the debugConsoleQuest and centerEyeCamera

                if (SurfacePatch != null && ConsoleCanvas_RectTransform != null)
                {
                    Vector3 newpatchsize = ConsoleCanvas_RectTransform.sizeDelta;
                    newpatchsize.z = SurfacePatchDepth;
                    BoundsClipper boundsClipper = SurfacePatch.GetComponent<BoundsClipper>();
                    if (boundsClipper != null)
                        boundsClipper.Size = newpatchsize;
                }

                if (debugConsoleQuest != null)
                {
                    if (PinButton != null)
                        PinButton.gameObject.SetActive(true);
                }
                else
                {
                    if (PinButton != null)
                        PinButton.gameObject.SetActive(false);
                }

                if (centerEyeCamera != null)
                {
                }
                else
                {
                    if (PinButton != null)
                        PinButton.text = "[-)^(-]";
                }

                if (!_trackHMD)
                {
                    if (PinButton != null)
                        PinButton.text = "[-)^(-]";
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

        protected override void HandlePinClick()
        {
            //Debug.Assert(PinButton != null, "PinOnClick - null PinButton");

            base.HandlePinClick();

            if (_pinned)
            {
                if (PinButton != null)
                    PinButton.text = "[-)^(-]";
            }
            else
            {
                if (PinButton != null)
                    PinButton.text = "[]->";
            }
        }

        protected override void HandleSizeClick()
        {
            //Debug.Assert(SizeConsoleButton != null, "SizeOnClick - null SizeConsoleButton");
            Debug.Assert(ConsoleCanvas_RectTransform != null, "SizeOnClick - Null ConsoleCanvas_RectTransform");

            base.HandleSizeClick(); // call base first it sets the ConsoleCanvas.GetComponent<RectTransform>().sizeDelta

            if (_size_flipflop)
            {
                if (SizeConsoleButton != null)
                    SizeConsoleButton.text = "2X";
            }
            else
            {
                if (SizeConsoleButton != null)
                    SizeConsoleButton.text = "1X";
            }

            if (SurfacePatch != null)
            {
                Vector3 newpatchsize = ConsoleCanvas_RectTransform.sizeDelta;
                newpatchsize.z = SurfacePatchDepth;
                BoundsClipper boundsClipper = SurfacePatch.GetComponent<BoundsClipper>();
                if (boundsClipper != null)
                    boundsClipper.Size = newpatchsize;
            }

        }
    }
}