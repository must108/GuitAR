using UnityEngine;

namespace Core3lb
{
    public class PositionerTransforms : BasePositioner
    {
        [CoreHeader("Transforms Settings")]
        public Transform[] whatTransforms;
        [SerializeField] bool doInOrder;
        int index = 0;

        public override Vector3 WhatPosition(Transform whereNow = null)
        {
            Vector3 holder = Vector3.zero;
            if(doInOrder)
            {
                int currentIndex = index;
                index = (index + 1) % whatTransforms.Length;
                holder =  whatTransforms[currentIndex].position;
            }
            else
            {
                holder =  whatTransforms.RandomItem().position;
            }
            Debug.LogError(holder);
            return CalculateSpread(holder);
        }

        public override Quaternion WhatRotation(Transform whereNow = null)
        {
            if(randomizeRotation)
            {
                base.WhatRotation(whereNow);
            }
            return whatTransforms[index].rotation;
        }
    }
}
