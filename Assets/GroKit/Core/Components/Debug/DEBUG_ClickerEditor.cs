using UnityEngine;

namespace Core3lb
{
    public class DEBUG_ClickerEditor : MonoBehaviour
    {
        public BaseTrigger myTrigger;
        public BaseActivator myActivator;
        [Space]
        public Transform moveTo;
        public Transform objectToMove;
        public UnityEngine.Events.UnityEvent testEventQuick;

        [CoreButton("Do UnityEvent")]
        void DoTestEvent()
        {
            testEventQuick.Invoke();
        }

        [CoreButton("Do Trigger Enter")]
        void DoTriggerEnter()
        {
            myTrigger = gameObject.GetComponentIfNull<BaseTrigger>(myTrigger);
            myTrigger.enterEvent.Invoke();
        }

        [CoreButton("Do Trigger Exit")]
        void DoTriggerExit()
        {
            myTrigger = gameObject.GetComponentIfNull<BaseTrigger>(myTrigger);
            myTrigger.enterEvent.Invoke();
        }

        [CoreButton("Do Activator On")]
        void DoActivatorOn()
        {
            myActivator = gameObject.GetComponentIfNull<BaseActivator>(myActivator);
            myActivator._OnEvent();
        }
        [CoreButton("Do Activator Off")]
        void DoActivatorOff()
        {
            myActivator = gameObject.GetComponentIfNull<BaseActivator>(myActivator);
            myActivator._OffEvent();
        }

        [CoreButton]
        void MoveObject()
        {
            objectToMove.transform.position = moveTo.position;
        }

        [CoreButton]
        void RotateTo()
        {
            objectToMove.transform.rotation = moveTo.rotation;
        }
    }
}
