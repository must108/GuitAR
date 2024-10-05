using UnityEngine;

namespace Core3lb
{

    public class MultiTransformSave : MonoBehaviour
    {
        public string groupID;
        public Transform[] transforms;
        [Header("-Settings-")]
        public bool loadOnStart;
        public Transform parentTo;
        public bool useLocal;
        public void Start()
        {
            if (loadOnStart)
            {
                _LoadTransforms();
            }
        }

        [CoreButton]
        public void _LoadTransforms()
        {
            if (!PlayerPrefs.HasKey(groupID + "0"))
            {
                Debug.Log("No Save Data for " + groupID);
                return;
            }
            for (int i = 0; i < transforms.Length; i++)
            {
                var data = TransformSave3lb.LoadTransform(groupID + i);
                if (parentTo)
                {
                    transforms[i].parent = parentTo;
                }
                if (data.isLocal)
                {
                    transforms[i].localPosition = data.position;
                    transforms[i].localRotation = data.rotation;
                    transforms[i].localScale = data.scale;
                }
                else
                {
                    transforms[i].position = data.position;
                    transforms[i].rotation = data.rotation;
                    transforms[i].localScale = data.scale;
                }
            }
        }

        [CoreButton]
        public void _SaveTransforms()
        {
            for (int i = 0; i < transforms.Length; i++)
            {
                TransformSave3lb.SaveTransform(transforms[i], groupID + i, transforms[i].name, useLocal);
            }
        }

        [CoreButton]
        public void _TESTRandomMove()
        {
            foreach (var item in transforms)
            {
                item.position = Random.insideUnitSphere * 5;
                item.rotation = Random.rotation;
                item.localScale = (Random.insideUnitSphere * 5 + Vector3.one) / 2f;
            }
        }
    }
}