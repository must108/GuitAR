using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class DoSleep : MonoBehaviour
    {
        [Tooltip("Sleep until woken Up")]
        public bool sleepForever;
        public float sleepTime = 5;
        public UnityEvent onSleep;
        public UnityEvent onWakeUp;

        [CoreRequired]
        public PrefabInfo itemToSleep;
        public bool useList;
        [CoreShowIf("useList")]
        public PrefabInfo[] itemsToSleep;

        public bool startAsleep;
        [CoreReadOnly]
        public bool isSleeping;
        public float sleepingTimer;

        public void FixedUpdate()
        {
            if(sleepForever)
            {
                if (isSleeping)
                {
                    sleepingTimer += Time.deltaTime;
                    if (sleepingTimer > sleepTime)
                    {
                        sleepingTimer = 0;
                        _WakeUp();
                    }
                }
            }
          
        }

        [CoreButton]
        public virtual void _DoSleep()
        {
            isSleeping = true;  
            onSleep.Invoke();
            SleepingLogic(false);
        }

        [CoreButton]
        public virtual void _WakeUp()
        {
            isSleeping = false;
            onWakeUp.Invoke();
            SleepingLogic(true);
        }



        protected virtual void SleepingLogic(bool goToSleep)
        {
            if(useList)
            {
                foreach (var item in itemsToSleep)
                {
                    item._SoftActive(goToSleep);
                }
            }

            else
            {
                itemToSleep._SoftActive(goToSleep);
            }
        }
        
    }
}
