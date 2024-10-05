
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class AnimateSpriteSheet : MonoBehaviour
    {
        [CoreHelpBox("This is used to Animate a Spirite sheet showing a tile or animating the whole sheet")]
        [CoreHeader("Required")]
        public int columns = 5;
        public int rows = 5;

        [Header("-Settings-")]
        public bool dontAnimate;
        [CoreShowIf("dontAnimate")]
        public int tileNumber;
        [Header("-Animation-")]
        [CoreHideIf("dontAnimate")]
        public float FramesPerSecond = 10f;
        [CoreHideIf("dontAnimate")]
        public bool RunOnce = true;

        public UnityEvent EndingEvent;

        float RunTimeInSeconds
        {
            get
            {
                return ((1f / FramesPerSecond) * (columns * rows));
            }
        }

        private Material materialCopy = null;
        Renderer myRenderer;

        void Start()
        {
            // Copy its material to itself in order to create an instance not connected to any other
            materialCopy = new Material(GetComponent<Renderer>().sharedMaterial);
            myRenderer = GetComponent<Renderer>();
            myRenderer.sharedMaterial = materialCopy;

            Vector2 size = new Vector2(1f / columns, 1f / rows);
            myRenderer.sharedMaterial.SetTextureScale("_MainTex", size);
        }

        void OnEnable()
        {
            if (dontAnimate)
            {
                _SetTile(tileNumber);
                return;
            }
            _StartAnimation();
        }

        public void _StartAnimation()
        {
            StartCoroutine(UpdateTiling());
        }

        public void _StopAnimation()
        {
            StopCoroutine(UpdateTiling());
        }

        private IEnumerator UpdateTiling()
        {
            float x = 0f;
            float y = 0f;
            Vector2 offset = Vector2.zero;

            while (true)
            {
                for (int i = rows - 1; i >= 0; i--) // y
                {
                    y = (float)i / rows;

                    for (int j = 0; j <= columns - 1; j++) // x
                    {
                        x = (float)j / columns;

                        offset.Set(x, y);
                        offsetDebug = offset;
                        //Have to do this not sure WHY
                        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
                        yield return new WaitForSeconds(1f / FramesPerSecond);
                    }
                }

                if (RunOnce)
                {
                    EndingEvent.Invoke();
                    yield break;
                }
            }
        }



        public void _SetTile(int chg)
        {
            tileNumber = chg;
            float x = 0f;
            float y = 0f;
            Vector2 offset = Vector2.zero;
            //It's CLOSE!
            x = (float)chg / columns;
            y = (float)y / rows;
            //y = (float)chg % columns;
            //y = rows / y;
            offset.Set(x, y);
            offsetDebug = offset;
            myRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }

        [CoreButton]
        public void _NextTile()
        {
            tileNumber++;
            if (tileNumber >= columns * rows)
            {
                tileNumber = 0;
            }

            _SetTile(tileNumber);
        }

        [Header("-Debug-")]
        public int testTile;
        public Vector2 offsetDebug;

        public float math1;
        public float math2;
        public float math3;
        [CoreButton]
        public void DebugTile()
        {

            float x = 0f;
            float y = 0f;
            Vector2 offset = Vector2.zero;
            //It's CLOSE!
            x = (float)testTile / columns;
            x = x / columns;
            y = testTile / rows;
            y = y / rows;
            //y = rows / y;
            offset.Set(x, y);
            offsetDebug = offset;
            myRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
    }
}