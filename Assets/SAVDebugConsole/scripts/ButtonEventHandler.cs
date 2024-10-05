//SAV Debug Console - Copyright 2024 ShadowAndVVWorkshop

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ShadowAndVVWorkshop.SAVDebugConsole
{
    public class ButtonEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Color NormalColor = new Color32(255, 255, 255, 255);
        [SerializeField] private Color PonterEnterColor = new Color32(0, 0, 0, 255);
        public void OnPointerEnter(PointerEventData eventData)
        {
            Image image = this.gameObject.GetComponent<Image>();
            if (image != null)
                image.color = PonterEnterColor;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            Image image = this.gameObject.GetComponent<Image>();
            if (image != null)
                image.color = NormalColor;
        }
    }
}