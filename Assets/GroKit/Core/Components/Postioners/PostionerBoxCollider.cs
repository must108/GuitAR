using UnityEngine;

namespace Core3lb
{
    public class PositionerBoxCollider : BasePositioner
    {
        [CoreHeader("Box Collider")]
        [SerializeField] BoxCollider myCollider;

        public override Vector3 WhatPosition(Transform whereNow)
        {
            Vector3 holder = GetFromCollider();
            return CalculateSpread(holder);
        }


        Vector3 GetFromCollider()
        {
            return myCollider.GetRandomPointInsideCollider();
        }
    }
}
