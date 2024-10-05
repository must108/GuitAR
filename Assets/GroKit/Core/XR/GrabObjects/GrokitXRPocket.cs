
using UnityEngine;
using UnityEngine.Events;
using static Core3lb.ItemIDConstants;

namespace Core3lb
{
    public class GroKitXRPocket : BaseTrigger
    {
        [CoreReadOnly]
        public GroKitXRGrabObject myObject;
        [CoreEmphasize]
        public Transform lockObjectTo;

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

        public UnityEvent ObjectLocked;

        public void Start()
        {
            lockObjectTo = gameObject.GetComponentIfNull<Transform>(lockObjectTo);
        }

        public void Update()
        {
            if(myObject == null)
            {
                return;
            }
            if(!myObject.isGrabbed)
            {
                LockObject();
            }
        }

        public void LockObject()
        {
            myObject.transform.position = lockObjectTo.position;
            myObject.transform.rotation = lockObjectTo.rotation;
            ObjectLocked.Invoke();
            myObject = null;
        }

        protected override bool IsAcceptable(Collider collision, bool isExit = false)
        {
            if(collision.TryGetComponent(out GroKitXRGrabObject grabObject))
            {
                myObject = grabObject;
                if(isExit)
                {
                    myObject = null;
                }
            }
            else
            {
                return false;
            }
            if (collision.TryGetComponent(out ItemID itemtest))
            {
                if (useStrings)
                {
                    Debug.LogError($"Colliders item IDs are {itemtest.itemID} | {itemtest.groupID} Looking for {itemIDString} | {groupIDString}", gameObject);
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
