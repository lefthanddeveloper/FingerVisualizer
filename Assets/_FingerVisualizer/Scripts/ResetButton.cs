using System;
using UnityEngine;
using UnityEngine.UI;

namespace FingerVisualizer
{
	public class ResetButton : ButtonEventCaller
	{
		[SerializeField] private Color defaultColor;
		[SerializeField] private Color highlightColor;

		private Image image; 
		private void Awake()
		{
			image = GetComponent<Image>();
			onPointerEnter.AddListener(() => image.color = highlightColor);
			onPointerExit.AddListener(() => image.color = defaultColor);
		}
	}
}
