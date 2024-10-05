using Oculus.Platform.Models;
using Oculus.Platform;
using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class MetaPlatform : MonoBehaviour
    {
        [CoreHeader("EntitlementCheck")]
        public bool doEntitlementOnStart;

        [Space]
        public UnityEvent entitlementSuccess;
        public UnityEvent entitlementFailed;

        [CoreHeader("UserName | ID")]
        public bool getUser;
        public ulong userId;
        public string displayName;
        [Space]

        public UnityEvent<string> gotDisplayName;
        public UnityEvent<ulong> gotUserId;
        public UnityEvent gotNamesFailed;

        public void Start()
        {
            if(doEntitlementOnStart)
            {
                _EntitlementCheck();
            }
        }



        public void _EntitlementCheck()
        {
            Oculus.Platform.Core.Initialize();
            Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCheckCallback);
        }

        private void EntitlementCheckCallback(Oculus.Platform.Message message)
        {
            if(message.IsError)
            {
                entitlementFailed.Invoke();
            }
            else
            {
                entitlementSuccess.Invoke();
            }
            GetInfoInternal();
        }
        public void GetInfoInternal()
        {
            if (!Core.IsInitialized())
            {
                Core.Initialize();
            }
            Users.GetLoggedInUser().OnComplete(GetMetaUserID);
        }

        private void GetMetaUserID(Oculus.Platform.Message msg)
        {
            if (!msg.IsError)
            {
                User user = msg.GetUser();
                userId = user.ID;
                displayName = user.DisplayName;
                gotDisplayName.Invoke(displayName);
                gotUserId.Invoke(userId);
            }
            else
            {
                Debug.LogError("Unable to get UserName");
                gotNamesFailed.Invoke();
            }
        }
    }   
}
