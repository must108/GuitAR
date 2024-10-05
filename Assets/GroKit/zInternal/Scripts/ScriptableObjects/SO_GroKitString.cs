
using UnityEngine;

namespace Core3lb
{
    [CreateAssetMenu(fileName = "GroKitStrings", menuName = "GroKit-Core/String")]
    public class SO_GroKitString : ScriptableObject
    {
        [SerializeField]
        [CoreEmphasize(true)]
        private string storedString = "YOUR_STRING";
        [TextArea]
        [SerializeField]
        private string storedLongText = "LONG_TEXT";


        public string GetString
        {
            get
            {
                if (storedString == "YOUR_STRING")
                {
                    Debug.LogError("Your String Has not Been Set!");
                }
                return storedString;
            }
        }

        public string StoredLongText
        {
            get { return storedLongText; }
        }
    }
}
