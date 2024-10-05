using UnityEngine;

namespace Core3lb
{
    public class DEBUG_ColorChanger : MonoBehaviour
    {
        [Tooltip("Optional")]
        [CoreEmphasize]
        public Renderer myRender;

        public Color customColor1 = Color.cyan;
        public Color customColor2 = Color.magenta;
        public Color customColor3 = Color.grey;

        public Color[] colorArray;

        void GetRenderer()
        {
            myRender = gameObject.GetComponentIfNull<Renderer>(myRender);
        }

        public void _Red()
        {
            ChangeColor(Color.red);
        }
        public void _Green()
        {
            ChangeColor(Color.green);
        }
        public void _Blue()
        {
            ChangeColor(Color.blue);
        }
        //Write Yellow 

        public void _Yellow()
        {
            ChangeColor(Color.yellow);
        }
        public void _White()
        {
            ChangeColor(Color.white);
        }

        public void _aRandom()
        {
            ChangeColor(Random.ColorHSV());
        }

        //Write custom 1 2 and 3 
        public void _zCustom1()
        {
            ChangeColor(customColor1);
        }

        public void _zCustom2()
        {
            ChangeColor(customColor2);
        }

        public void _zCustom3()
        {
            ChangeColor(customColor3);
        }

        void ChangeColor(Color whatColor)
        {
            GetRenderer();
            myRender.material.color = whatColor;
        }

        public void _CustomColorArray(int chg)
        {
            ChangeColor(colorArray[chg]);
        }
    }
}
