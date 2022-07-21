using System;
using UnityEngine;
using UnityEngine.UI;

namespace FingerVisualizer
{
	public class FingerSetting : Menu
	{
		[Header("[ Finger Setting ]")]
		[SerializeField] private HandUI leftHandUI;
		[SerializeField] private HandUI rightHandUI;

		[SerializeField] private ResetButton button_ResetFreq;
		[SerializeField] private ResetButton button_ResetIntensity;
		[SerializeField] private Slider overallIntensitySlider;
		private float originalOverallIntensityValue;
		public override void Init()
		{
			base.Init();

			originalOverallIntensityValue = FindObjectOfType<FingerVisualizer>().IntensityMultiplier;

			leftHandUI.Init();
			rightHandUI.Init();

			button_ResetFreq.onClick.AddListener(() =>
			{
				leftHandUI.OnClickResetFreq();
				rightHandUI.OnClickResetFreq();
			});

			button_ResetIntensity.onClick.AddListener(() =>
			{
				leftHandUI.OnClickResetIntensity();
				rightHandUI.OnClickResetIntensity();

				overallIntensitySlider.value = originalOverallIntensityValue;
			});

			overallIntensitySlider.onValueChanged.AddListener((newValue) =>
			{
				leftHandUI.ChangeOverallIntensity(newValue);
				rightHandUI.ChangeOverallIntensity(newValue);
			});
			
		}
	}
}
