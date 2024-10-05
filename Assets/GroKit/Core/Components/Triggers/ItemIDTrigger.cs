using UnityEngine;

namespace Core3lb
{
    public class ItemIDTrigger : BaseTrigger
    {
        [CoreHeader("ItemIDTrigger")]
        public bool useStrings;
        [CoreHideIf("useStrings")]
        public ItemIDConstants.ItemID itemID = ItemIDConstants.ItemID.Any;
        [CoreHideIf("useStrings")]
        public ItemIDConstants.GroupID groupID = ItemIDConstants.GroupID.Any;
        public const string ANY = "ANY";

        [Tooltip("Use strings to identify themselves.")]
        [CoreShowIf("useStrings")]
        public string itemIDString = ANY;
        [CoreShowIf("useStrings")]
        public string groupIDString = ANY;

        protected override bool IsAcceptable(Collider collision, bool isExit = false)
        {
            if(collision.TryGetComponent(out ItemID itemtest))
            {
                if (debugTrigger)
                {
                    
                }
                if(useStrings)
                {
                    Debug.LogError($"Colliders item IDs are {itemtest.itemID} | {itemtest.groupID} Looking for {itemIDString} | {groupIDString}" , gameObject);
                    return ItemID.CheckItemID(itemtest, itemIDString, groupIDString);
                }
                else
                {
                    Debug.LogError($"Colliders item IDs are {itemtest.itemID} | {itemtest.groupID} Looking for {itemID} | {groupID}", gameObject);
                    return ItemID.CheckItemID(itemtest, itemID.ToString(), groupID.ToString());
                }
               
            }
            return false;
        }
    }
}
