//SAV Formatted Input Field - Copyright 2024 ShadowAndVVWorkshop

using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//using UnityEngine.UIElements;

namespace ShadowAndVVWorkshop.SAVFormattedInputField
{
    public class InputFieldWithStringFormat : MonoBehaviour
    {
        [Tooltip("Maximum display characters. Default 0 = all")]
        [SerializeField] private int _MaxCharacters = 0;
        public int MaxCharacters { get { return _MaxCharacters; } set {
                if (isSystemKeyboardActive())
                    return;
                if (value >= 0) // MUST BE POSITIVE
                {
                    _MaxCharacters = value;
                    ProcessText(true);
                }
            } }

        [Tooltip("Maximum display value. Default 0 = all")]
        [SerializeField] private int _UpperLimit = 0;
        public int UpperLimit { get { return _UpperLimit; } set {
                if (isSystemKeyboardActive())
                    return;
                if (value >= _LowerLimit) // MUST BE LARGER THAN THE LOWER LIMIT
                {
                    _UpperLimit = value;
                    ProcessText(true); // FORCE SWITCH
                }
            } }

        [Tooltip("Minimum display value. Default 0 = all")]
        [SerializeField] private int _LowerLimit = 0;
        public int LowerLimit { get { return _LowerLimit; } set {
                if (isSystemKeyboardActive())
                    return;
                if (value <= _UpperLimit) // MUST BE SMALLER THAN THE UPPER LIMIT
                {
                    _LowerLimit = value;
                    ProcessText(true); // FORCE SWITCH
                }
            }
        }

        [Tooltip("Standard or Custom numeric string format. Default '' = no formatting")]
        [SerializeField] private string _formatString = "";
        public string FormatString { get { return _formatString; } set {
                if (isSystemKeyboardActive())
                    return;
                if (value.ToLower().Contains("x"))
                    GetComponent<InputField>().keyboardType = TouchScreenKeyboardType.Default;
                else
                    GetComponent<InputField>().keyboardType = TouchScreenKeyboardType.NumberPad;
                //Debug.Log("!!!format change" + prev_value);
                if (value.ToLower().Contains("x") && !FormatString.ToLower().Contains("x"))
                {
                    GetComponent<InputField>().text = string.Format(value, (int)prev_value);
                    if (InputFieldHandler != null && InputFieldHandler.GetComponent<Text>() != null)
                        InputFieldHandler.GetComponent<Text>().text = GetComponent<InputField>().text;
                }
                else
                {
                    if (!value.ToLower().Contains("x") && FormatString.ToLower().Contains("x"))
                    {
                        GetComponent<InputField>().text = prev_value.ToString();
                        if (InputFieldHandler != null && InputFieldHandler.GetComponent<Text>() != null)
                            InputFieldHandler.GetComponent<Text>().text = GetComponent<InputField>().text;
                    }
                    //else  { }
                }
                _formatString = value;
                ProcessText(true); // FORCE SWITCH
            } }

        [Tooltip("Culture info as string")]
        [SerializeField] protected string _cultureinfostring = "en-US";
        public string cultureinfostring
        {
            get { return _cultureinfostring; }
            set 
            {
                try
                {
                    _culture = CultureInfo.GetCultureInfo(value);
                    ProcessText(true); // FORCE SWITCH
                    _cultureinfostring = value;
                }
                catch {
                    Debug.Log("Invalid Culture Name");
                }
            }
        }

        [SerializeField] public InputFieldHandler InputFieldHandler;

        protected static TouchScreenKeyboard touchScreenKeyboard = null;

        //public UnityEvent OnBeginEdit = new UnityEvent();
        public UnityEvent<string, double> OnSavEndEdit = new UnityEvent<string, double>();
        public UnityEvent<string, double> OnSavValueChanged = new UnityEvent<string, double>();

        public string text { get { return GetComponent<InputField>().text; } set {
                GetComponent<InputField>().text = value;
                ProcessText(true); // DO NOT FORCE
            }
        }

        public string GetKeyboardText() { return GetComponent<InputField>().text; }

        public string GetTextFormatted() { return InputFieldHandler.GetComponent<Text>().text; }

        public double GetDouble() { return prev_value; }

        protected string prev_text = "";
        protected double prev_value = 0;
        protected Activekeyboard _Activekeyboard = Activekeyboard.None;
        protected enum Activekeyboard
        {
            None = 0,
            TouchScreenKeyboard = 1,
            OVRVirtualKeyboard = 2,
            OverlayKeyboard = 3
        }

        protected CultureInfo _culture;

        private void Awake()
        {
            cultureinfostring = _cultureinfostring;
        }

        protected void Start()
        {
            HandleStart();
        }

        protected virtual void HandleStart()
        {
            // not called if vitual keyboard
            //Debug.Log("Start Activekeyboard.TouchScreenKeyboard");
            if (touchScreenKeyboard == null)
            {
                touchScreenKeyboard = TouchScreenKeyboard.Open("");
                if (touchScreenKeyboard != null)
                {
                    //Debug.Log("TouchScreenKeyboard Active FALSE");
                    touchScreenKeyboard.active = false;
                    _Activekeyboard = Activekeyboard.TouchScreenKeyboard;
                }
            }

            GetComponent<InputField>().onEndEdit.AddListener(OnInputFieldEndEdit);

        }

        public static bool isSystemKeyboardActive()
        {
            if (touchScreenKeyboard != null &&
                touchScreenKeyboard.active
                )
            {
                //Debug.Log("TouchScreenKeyboard Active");
                return true;
            }
            return false;
        }

        protected void OnInputFieldEndEdit(string text)
        {
            //Debug.Log(transform.parent.name + "OnInputFieldEndEdit " + text);
            OnCustomTouchScreenKeyboardInputFieldTextHandlerEndEdit();
        }

        protected void OnCustomTouchScreenKeyboardInputFieldTextHandlerEndEdit()
        {
            //Debug.Log(transform.parent.name + "OnCustomTouchScreenKeyboardInputFieldTextHandlerEndEdit ");
            OnSavEndEdit?.Invoke(prev_text, prev_value);
        }

        void Update()
        {
            ProcessText();
        }

        void ProcessText(bool force = false)
        {
            bool fireevent = false;
            string tempstring = GetComponent<InputField>().text;
            //Debug.Log("Keyboard bound" + tempstring);

            if (MaxCharacters > 0)
            {
                if (tempstring.Length > MaxCharacters)
                {
                    fireevent = true;
                    tempstring = tempstring.Substring(0, MaxCharacters);
                }
            }

            if (force || prev_text != tempstring)
            {
                if (prev_text != tempstring)
                    fireevent = true;

                prev_text = tempstring;
                
                double doublevalue = 0;
                bool conversionresult = false;
                if (!string.IsNullOrEmpty(prev_text))
                {
                    if (FormatString.ToLower().Contains("x"))
                    {
                        int intvalue = 0;
                        conversionresult = int.TryParse(prev_text, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out intvalue);
                        doublevalue = intvalue;
                    }
                    else
                    {
                        conversionresult = double.TryParse(prev_text, out doublevalue);
                    }
                }

                //Debug.Log(transform.parent.name + " value " + doublevalue);

                if (!string.IsNullOrEmpty(prev_text) && (UpperLimit != 0 || LowerLimit != 0))
                {
                    if (doublevalue < LowerLimit)
                        doublevalue = LowerLimit;
                    if (doublevalue > UpperLimit)
                        doublevalue = UpperLimit;
                }

                if (prev_value != doublevalue)
                    fireevent = true;
                prev_value = doublevalue;

                if (InputFieldHandler != null && InputFieldHandler.GetComponent<Text>() != null)
                {
                    if (!string.IsNullOrEmpty(prev_text))
                    {
                        if (FormatString.ToLower().Contains("x"))   // HEX
                            InputFieldHandler.GetComponent<Text>().text = ((int)doublevalue).ToString(FormatString, _culture);
                        else
                        {
                            if (string.IsNullOrEmpty(FormatString)) // PASS THRU STRING
                                InputFieldHandler.GetComponent<Text>().text = prev_text;
                            else
                                InputFieldHandler.GetComponent<Text>().text = doublevalue.ToString(FormatString, _culture);
                        }
                    }
                    else
                      InputFieldHandler.GetComponent<Text>().text = "";

                    if (fireevent)
                        OnSavValueChanged?.Invoke(prev_text, prev_value);
                }
            }
        }
    }
}
//https://blog.stevex.net/string-formatting-in-csharp/
