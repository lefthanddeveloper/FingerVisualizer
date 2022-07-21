using System;
using UnityEngine;
using UnityEngine.Events;

namespace FingerVisualizer
{
    public class ContactPage : Menu
    {
        [SerializeField] private CopyButton copyButton;

        
        [SerializeField] private CopiedMessage copiedMessage;

        public override void Init()
        {
            base.Init();
            copiedMessage.Init();
            copyButton.onClick.AddListener(OnClickCopyButton);
        }

        private void OnClickCopyButton()
        {
            GUIUtility.systemCopyBuffer = "lefthanddeveloper@gmail.com";
            ShowMessageCopied();
        }


        private void ShowMessageCopied()
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panel.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint);
            copiedMessage.Show(localPoint);
        }
    }
}
