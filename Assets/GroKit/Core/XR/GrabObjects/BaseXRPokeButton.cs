using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class BaseXRPokeButton : MonoBehaviour
    {
        [CoreReadOnly]

        [HideInInspector]
        public bool isLocked = false;

        public UnityEvent onEnter; //Hover
        public UnityEvent onExit; //UnHover
        public UnityEvent onPoke; //Grab

        //Give me Actions for each event
        #pragma warning disable CS0067
        public event Action onEnterAction;
        public event Action onExitAction;
        public event Action onPokeAction;
        #pragma warning restore CS0067
        //Event Functions
        public virtual void _Poke()
        {
            if(isLocked) return;
            onPokeAction?.Invoke();
            onPoke.Invoke();
        }

        public virtual void EnterEvent()
        {
            if (isLocked) return;
            onEnterAction?.Invoke();
            onEnter.Invoke();
        }

        public virtual void ExitEvent()
        {
            if (isLocked) return;
            onExitAction?.Invoke();
            onExit.Invoke();
        }
    }
}
