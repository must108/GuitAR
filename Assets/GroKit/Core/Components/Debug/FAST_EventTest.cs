using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class FAST_EventTest : MonoBehaviour
    {
        public UnityEvent FastTestEvent;
        [CoreButton("Event ", true)]
        public void Event()
        {
            FastTestEvent.Invoke();
        }
    }
}
