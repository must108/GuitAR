
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core3lb
{
    public class MassResetter : MonoBehaviour
    {
        [SerializeField] List<ResettableObject> myObjects;

        [CoreButton("Get From Childern",true)]
        public virtual void GetFromChildern()
        {
            myObjects = GetComponentsInChildren<ResettableObject>().ToList();
        }

        [CoreButton]
        public virtual void _ResetAllObjects()
        {
            for (int i = 0; i < myObjects.Count; i++)
            {
                myObjects[i]._ResetObject();
            }
        }

        //Will be moved to it's own component or use TriggerReaction
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.TryGetComponent(out ResettableObject holder))
        //    {
        //        holder._ResetObject();
        //    }
        //}

        public virtual void RemoveResettable(ResettableObject resettable)
        {
            if (myObjects.Contains(resettable))
                myObjects.Remove(resettable);
        }
    }
}
