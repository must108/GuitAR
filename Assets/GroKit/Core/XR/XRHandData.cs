
using System;
using UnityEngine;

namespace Core3lb
{
    public class XRHandData : MonoBehaviour
    {
        //This class is meant to be extended to get all the hand data you might need and store it.
        public HandTransforms leftHandTransforms;
        public HandTransforms rightHandTransforms;

        public SkinnedMeshRenderer leftMesh;
        public SkinnedMeshRenderer rightMesh;

        public virtual void SetHandVisuals(bool chg)
        {
            leftMesh.enabled = chg;
            rightMesh.enabled = chg;
        }


        [Serializable]
        public struct HandTransforms
        {
            public Transform wrist;
            public Transform index_1;
            public Transform index_2;
            public Transform index_3;
            public Transform middle_1;
            public Transform middle_2;
            public Transform middle_3;
            public Transform ring_1;
            public Transform ring_2;
            public Transform ring_3;
            public Transform pinky_0;
            public Transform pinky_1;
            public Transform pinky_2;
            public Transform pinky_3;
            public Transform thumb_0;
            public Transform thumb_1;
            public Transform thumb_2;
            public Transform thumb_3;
            public Transform index_tip;
            public Transform middle_tip;
            public Transform ring_tip;
            public Transform pinky_tip;
            public Transform thumb_tip;
        }
    }
}
