using System;
using UnityEngine;

namespace FingerVisualizer
{
	public class FingerSetting : Menu
	{
		[Header("[ Finger Setting ]")]
		[SerializeField] private HandUI leftHandUI;
		[SerializeField] private HandUI rightHandUI;

		[SerializeField] private ResetButton button_ResetFreq;
		[SerializeField] private ResetButton button_ResetIntensity;
		
		public override void Init()
		{
			base.Init();


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
			});
			
		}
	}
}
