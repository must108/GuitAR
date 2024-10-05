using Core3lb.CoreSimpleJSON;
using System;
using UnityEngine;

namespace Core3lb
{

    [Serializable]
    public class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public string itemID;
        public bool isLocal;
        public TransformData() { } // Required default constructor for deserialization
    }

    public class TransformSave3lb : MonoBehaviour
    {
        public bool autoLoadOnStart;
        public string mySaveID;
        public string UID = string.Empty;
        string myItemID = string.Empty; //For Later Resource Loading;
        public Transform parentTo;
        public bool useLocal;
        [Header("-Debug-")]
        [CoreReadOnly]
        public string savedString;
        [CoreReadOnly]
        public TransformData myData;

        public string mySaveidfield
        {
            get
            {
                return mySaveID + UID;
            }
        }

        public void Start()
        {
            if (autoLoadOnStart)
            {
                _LoadTransform();
            }
        }

        [CoreButton]
        public void _SaveTransform()
        {
            SaveTransform(transform, mySaveidfield, myItemID, useLocal);
        }

        [CoreButton]
        public void _LoadTransform()
        {
            TransformData data = LoadTransform(mySaveidfield);
            if (data == null)
            {
                return;
            }
            if (parentTo)
            {
                transform.parent = parentTo;
            }
            if (data.isLocal)
            {
                transform.localPosition = data.position;
                transform.localRotation = data.rotation;
                transform.localScale = data.scale;
            }
            else
            {
                transform.position = data.position;
                transform.rotation = data.rotation;
                transform.localScale = data.scale;
            }

        }

        [CoreButton]
        public void _DeleteMySave()
        {
            PlayerPrefs.DeleteKey(mySaveidfield);
        }

        //Static Methods For other ScriptsToUse
        public static void SaveTransform(Transform theThing, string saveID, string ItemID, bool isLocal = false)
        {
            // Create a JSON object and assign the properties of MyData
            JSONNode json = JSON.Parse("{}");
            if (isLocal)
            {
                json["position"] = JSONNode.Parse(JsonUtility.ToJson(theThing.transform.localPosition));
                json["rotation"] = JSONNode.Parse(JsonUtility.ToJson(theThing.transform.localRotation));
                json["scale"] = JSONNode.Parse(JsonUtility.ToJson(theThing.transform.localScale));
            }
            else
            {
                json["position"] = JSONNode.Parse(JsonUtility.ToJson(theThing.transform.position));
                json["rotation"] = JSONNode.Parse(JsonUtility.ToJson(theThing.transform.rotation));
                json["scale"] = JSONNode.Parse(JsonUtility.ToJson(theThing.transform.lossyScale));
            }
            json["isLocal"] = isLocal;
            // Convert the JSON object to a string
            string jsonString = json.ToString();
            PlayerPrefs.SetString(saveID, jsonString);
        }
        public static TransformData LoadTransform(string saveID)
        {
            if (PlayerPrefs.HasKey(saveID))
            {
                return DeserializeData(PlayerPrefs.GetString(saveID));
            }
            else
            {
                Debug.Log("No Save Found For: " + saveID);
                return null;
            }
        }

        private static TransformData DeserializeData(string jsonString)
        {
            // Parse the JSON string into a JSONNode object
            JSONNode json = JSON.Parse(jsonString);
            // Create a new instance of MyData and assign the properties
            TransformData deserializedData = new TransformData();
            deserializedData.position = JsonUtility.FromJson<Vector3>(json["position"].ToString());
            deserializedData.rotation = JsonUtility.FromJson<Quaternion>(json["rotation"].ToString());
            deserializedData.scale = JsonUtility.FromJson<Vector3>(json["scale"].ToString());
            deserializedData.isLocal = json["isLocal"].AsBool;
            return deserializedData;
        }
    }
}
