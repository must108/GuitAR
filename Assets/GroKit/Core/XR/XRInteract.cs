using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class XRInteract : BaseTrigger
    {
        [CoreHeader("XRInteract")]
        public List<XRHand> hands;
        public UnityEvent interact;


        [CoreEmphasize]
        public InputXREvent overrideEvent;
        [CoreReadOnly]
        public XRHand lastUsedHand;

        public void Update()
        {
            foreach (XRHand hand in hands)
            {
                if(overrideEvent)
                {
                    if (overrideEvent.inputProcessor.isDown)
                    {
                        lastUsedHand = hand;
                        Interact();
                    }
                    return;
                }
                if (hand.GrabProcessor.isDown)
                {
                    lastUsedHand = hand;
                    Interact();
                }
            }
        }

        [CoreButton]
        public virtual void Interact()
        {
            interact.Invoke();
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out XRHand hand))
            {
                _EnterEvent();
                hands.AddIf(hand);
            }
        }

        public override void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out XRHand hand))
            {
                _ExitEvent();
                hands.RemoveIf(hand);
            }
        }
    }
}
