using UnityEngine;

namespace Core3lb
{
    //The stored data holder is a quick scriptable that stores data. for specific needs

    //These are public but should not be changed at runtime
    [CreateAssetMenu(fileName = "DataHolder", menuName = "GroKit-Core/DataHolder")]
    public class SO_DataHolder : ScriptableObject
    {
        [SerializeField]
        private string storedString = "YOUR_STRING";

        [TextArea]
        [SerializeField]
        private string storedLongText = "LONG_TEXT";


        [SerializeField]
        private int storedInt;

        [SerializeField]
        private Vector3 storedVector;
        [SerializeField]
        private bool storedBool;

        public string GetString
        {
            get { return storedString; }
        }

        public string GetLongString
        {
            get { return storedLongText; }
        }

        public int StoredInt
        {
            get { return storedInt; }
        }

        public Vector3 StoredVector
        {
            get { return storedVector; }
        }

        public bool StoredBool
        {
            get { return storedBool; }
        }

    }
}
