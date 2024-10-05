//SAV Formatted Input Field - Copyright 2024 ShadowAndVVWorkshop

using UnityEngine;

namespace ShadowAndVVWorkshop.SAVFormattedInputField
{
    public class FormattedInputFieldSampleHandler : MonoBehaviour
    {
        [SerializeField] private OVRVirtualKeyboard _OVRVirtualKeyboard;
        [SerializeField] private InputFieldWithStringFormat_Quest UserInputOutput;
        [SerializeField] private InputFieldWithStringFormat_Quest CharactersMax;
        [SerializeField] private InputFieldWithStringFormat_Quest LimitsUpper;
        [SerializeField] private InputFieldWithStringFormat_Quest LimitsLower;
        [SerializeField] private InputFieldWithStringFormat_Quest CultureName;
        [SerializeField] private InputFieldWithStringFormat_Quest UIO1;
        [SerializeField] private InputFieldWithStringFormat_Quest UIO2;
        [SerializeField] private InputFieldWithStringFormat_Quest UIO3;
        [SerializeField] private InputFieldWithStringFormat_Quest UIO4;
        [SerializeField] private InputFieldWithStringFormat_Quest UIO5;
        [SerializeField] private InputFieldWithStringFormat_Quest UIO6;
        [SerializeField] private InputFieldWithStringFormat_Quest UIO7;
        [SerializeField] private InputFieldWithStringFormat_Quest UD_UIO;

        // Start is called before the first frame update
        void OnEnable()
        {
            // set 'CharactersMax' max value to 99
            CharactersMax.LowerLimit = 0;
            CharactersMax.UpperLimit = 99;

            // initialize MaxCharacters and limits
            CharactersMax.OnSavEndEdit.AddListener(CharactersMaxChanged);
            CharactersMax.text = "10";
            LimitsUpper.OnSavEndEdit.AddListener(LimitsUpperChanged);
            LimitsUpper.text = "0";
            LimitsLower.OnSavEndEdit.AddListener(LimitsLowerChanged);
            LimitsLower.text = "0";

            // becuase im using OnSavEndEdit, which wont get triggered here
            // initialize io fields
            CharactersMaxChanged("", 10);
            LimitsUpperChanged("", 0);
            LimitsLowerChanged("", 0);

            // event for when the entry field changes
            UserInputOutput.OnSavValueChanged.AddListener(SyncIOFields);

            UserInputOutput.text = "-1234";

            // pre load user defined
            UD_UIO.OnSavEndEdit.AddListener(UserDefinedValueChanged);
            UD_UIO.text = "Positive;Negative;Zero";
            UIO7.FormatString = UD_UIO.text;

            CultureName.OnSavEndEdit.AddListener(CultureNameValueChanged);
            CultureName.text = "en-US";

            // comment this to use the system keyboard
            UserInputOutput.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            CharactersMax.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            LimitsUpper.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            LimitsLower.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            CultureName.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            UIO1.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            UIO2.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            UIO3.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            UIO4.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            UIO5.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            UIO6.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            UIO7.OVRVirtualKeyboard = _OVRVirtualKeyboard;
            UD_UIO.OVRVirtualKeyboard = _OVRVirtualKeyboard;

        }
        void OnDisable()
        {
            CharactersMax.OnSavEndEdit.RemoveListener(CharactersMaxChanged);
            LimitsUpper.OnSavEndEdit.RemoveListener(LimitsUpperChanged);
            LimitsLower.OnSavEndEdit.RemoveListener(LimitsLowerChanged);
            UserInputOutput.OnSavValueChanged.RemoveListener(SyncIOFields);
            UD_UIO.OnSavEndEdit.RemoveListener(UserDefinedValueChanged);
            CultureName.OnSavEndEdit.RemoveListener(CultureNameValueChanged);
        }

        private void SyncIOFields(string newtext, double newvalue)
        {
            UIO1.text = newtext;
            UIO2.text = newtext;
            UIO3.text = newtext;
            UIO4.text = newtext;
            UIO5.text = newtext;
            UIO6.text = newtext;
            UIO7.text = newtext;
        }

        private void CharactersMaxChanged(string newtext, double newvalue)
        {
            //Debug.Log(transform.name + " CharactersMaxChanged " + newvalue);
            if (newvalue < 0) // must be greater or equal 0
            {
                UserInputOutput.MaxCharacters = 0;
                return;
            }

            UserInputOutput.MaxCharacters = (int)newvalue;

            UIO1.MaxCharacters = (int)newvalue;
            UIO2.MaxCharacters = (int)newvalue;
            UIO3.MaxCharacters = (int)newvalue;
            UIO4.MaxCharacters = (int)newvalue;
            UIO5.MaxCharacters = (int)newvalue;
            UIO6.MaxCharacters = (int)newvalue;
            UIO7.MaxCharacters = (int)newvalue;
        }

        private void LimitsUpperChanged(string newtext, double newvalue)
        {
            //Debug.Log(transform.name + " LimitsUpperChanged " + newvalue);
            if (newvalue < LimitsLower.GetDouble()) // Upper limit must be greater than lower limit
            {
                LimitsUpper.text = LimitsLower.text;
                return;
            }

            if (newvalue == 0) // Upper limit must be greater than lower limit
            {
                LimitsUpper.text = "0";
            }

            UserInputOutput.UpperLimit = (int)newvalue;

            UIO1.UpperLimit = (int)newvalue;
            UIO2.UpperLimit = (int)newvalue;
            UIO3.UpperLimit = (int)newvalue;
            UIO4.UpperLimit = (int)newvalue;
            UIO5.UpperLimit = (int)newvalue;
            UIO6.UpperLimit = (int)newvalue;
            UIO7.UpperLimit = (int)newvalue;
        }

        private void LimitsLowerChanged(string newtext, double newvalue)
        {
            //Debug.Log(transform.name + " LimitsUpperChanged " + newvalue);
            if (newvalue > LimitsUpper.GetDouble()) // Lower limit must be less than upper limit
            {
                LimitsLower.text = LimitsUpper.text;
                return;
            }

            if (newvalue == 0) // Upper limit must be greater than lower limit
            {
                LimitsLower.text = "0";
            }

            UserInputOutput.LowerLimit = (int)newvalue;

            UIO1.LowerLimit = (int)newvalue;
            UIO2.LowerLimit = (int)newvalue;
            UIO3.LowerLimit = (int)newvalue;
            UIO4.LowerLimit = (int)newvalue;
            UIO5.LowerLimit = (int)newvalue;
            UIO6.LowerLimit = (int)newvalue;
            UIO7.LowerLimit = (int)newvalue;
        }

        private void UserDefinedValueChanged(string newtext, double newvalue)
        {
            //Debug.Log(transform.name + " UserDefinedValueChanged " + newtext);

            UIO7.FormatString = newtext;
        }

        private void CultureNameValueChanged(string newtext, double newvalue)
        {
            //Debug.Log(transform.name + " CultureNameValueChanged " + newtext);

            UIO1.cultureinfostring = newtext;
            UIO2.cultureinfostring = newtext;
            UIO3.cultureinfostring = newtext;
            UIO4.cultureinfostring = newtext;
            UIO5.cultureinfostring = newtext;
            UIO6.cultureinfostring = newtext;
            UIO7.cultureinfostring = newtext;
        }

    }
}
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings