using System;
using UnityEngine;
using UnityEngine.UI;

namespace FingerVisualizer
{
    public class CopyButton : ButtonEventCaller
    {
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color hoverColor = Color.white;

        private Image image;
        private void Awake()
        {
            image = GetComponent<Image>();
            onPointerEnter.AddListener(() => image.color = hoverColor);
            onPointerExit.AddListener(() => image.color = defaultColor);
        }
    }
}
