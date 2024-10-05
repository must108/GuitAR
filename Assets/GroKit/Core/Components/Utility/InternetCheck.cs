using UnityEngine;
using UnityEngine.Events;


namespace Core3lb
{
    public class InternetCheck : MonoBehaviour
    {
        public bool forceOffline;
        public UnityEvent isOfflineEvent;
        public UnityEvent isOnlineEvent;
        public static bool isOffline;

        void Awake()
        {
            _CheckForInternet();
        }

        public void _CheckForInternet()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable || forceOffline)
            {
                isOffline = true;
                isOfflineEvent.Invoke();
            }
            else
            {
                isOffline = false;
                isOnlineEvent.Invoke();
            }
        }
    }
}
