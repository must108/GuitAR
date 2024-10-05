using UnityEngine;

namespace Core3lb
{
    public class ItemID : MonoBehaviour
    {
        public bool useStrings;

        [CoreHideIf("useStrings")]
        public ItemIDConstants.ItemID itemIDEnum = ItemIDConstants.ItemID.Any;

        [CoreHideIf("useStrings")]
        public ItemIDConstants.GroupID groupIDEnum = ItemIDConstants.GroupID.Any;

        public const string ANY = "ANY";

        [Tooltip("Use strings to identify themselves.")]
        [CoreShowIf("useStrings")]
        public string itemIDString = ANY;

        [CoreShowIf("useStrings")]
        public string groupIDString = ANY;
        [CoreHeader("Item Info")]
        [Tooltip("DisplayName can be pulled by other scripts")]
        public string DisplayName;

        [TextArea]
        [Tooltip("Description can be pulled by other scripts")]
        public string Description;


        public string itemID
        {
            get
            {
                return useStrings ? itemIDString : itemIDEnum.ToString();
            }
        }

        public string groupID
        {
            get
            {
                return useStrings ? groupIDString : groupIDEnum.ToString();
            }
        }

        public bool CheckItemID(string acceptedItemID, string acceptedGroupID = ANY)
        {
            if (acceptedGroupID == ANY)
            {
                if (acceptedItemID == ANY)
                {
                    return true;
                }
                else if (acceptedItemID == itemID)
                {
                    return true;
                }
            }
            else
            {
                if (acceptedGroupID == groupID)
                {
                    if (acceptedItemID == ANY)
                    {
                        return true;
                    }
                    else if (acceptedItemID == itemID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CheckItemID(ItemID currentID, string acceptedItemID, string acceptedGroupID = ANY)
        {
            if (acceptedGroupID == ANY)
            {
                if (acceptedItemID == ANY)
                {
                    return true;
                }
                else if (acceptedItemID == currentID.itemID)
                {
                    return true;
                }
            }
            else
            {
                if (acceptedGroupID == currentID.groupID)
                {
                    if (acceptedItemID == ANY)
                    {
                        return true;
                    }
                    else if (acceptedItemID == currentID.itemID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
