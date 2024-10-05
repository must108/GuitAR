using UnityEngine;

namespace Core3lb
{
    public class QuickAnimator : MonoBehaviour
    {
        public Sprite[] spriteList;

        [SerializeField] float framesPerSecond = 12.0f;
        public bool isSpriteRenderer;

        Renderer myRender;
        SpriteRenderer mySpriteRenderer;

        private void Awake()
        {
            myRender = GetComponent<Renderer>();
            if (isSpriteRenderer)
            {
                mySpriteRenderer = GetComponent<SpriteRenderer>();
                return;
            }
        }
        private void FixedUpdate()
        {
            // Calculate index
            int index = (int)(Time.time * framesPerSecond);
            index = index % spriteList.Length;
            if (isSpriteRenderer)
            {
                mySpriteRenderer.sprite = spriteList[index];
            }
            else
            {
                myRender.material.SetTexture("_MainTex", spriteList[index].texture);
            }
        }
    }
}
