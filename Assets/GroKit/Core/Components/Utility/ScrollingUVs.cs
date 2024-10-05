using UnityEngine;

namespace Core3lb
{
    public class ScrollingUVs : MonoBehaviour
    {
        public Vector2 Scroll = new Vector2(0.05f, 0.05f);
        Vector2 Offset = new Vector2(0f, 0f);

        void Update()
        {
            Offset += Scroll * Time.deltaTime;
            GetComponent<Renderer>().material.SetTextureOffset("_MainTex", Offset);
        }
    }
}