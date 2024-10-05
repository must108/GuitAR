using UnityEngine;

namespace Core3lb
{
    public class PostionerOverlap : BasePositioner
    {
        [CoreHeader("Overlap Detection")]
        public bool useOverlapDetection = true;
        public float obstacleCheckRadius = 3f;
        public int maxAttempts = 10;

        public override Vector3 WhatPosition(Transform whereNow = null)
        {
            return OverlapChecker(base.WhatPosition(whereNow),whereNow);
        }

        public Vector3 OverlapChecker(Vector3 where, Transform whereNow)
        {
            bool validPosition = false;
            int attempts = 0;
            Vector3 position = where;
           // bool inOverlap = true;
            while (!validPosition && attempts < maxAttempts)
            {
                // Increase our spawn attempts
                attempts++;
                position = WhatPosition(whereNow);
                // Pick a random position
                validPosition = true;
                // Collect all colliders within our Obstacle Check Radius
                Collider[] colliders = Physics.OverlapSphere(position, obstacleCheckRadius);
                // Go through each collider collected
                foreach (Collider col in colliders)
                {
                    validPosition = false;
                }
            }
            //inOverlap = false;
            return position;
        }
    }
}
