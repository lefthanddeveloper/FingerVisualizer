using System;
using UnityEngine;

namespace FingerVisualizer
{
	public class FingerSetting : Menu
	{
		[SerializeField] private HandUI leftHandUI;
		[SerializeField] private HandUI rightHandUI;
		public override void Init()
		{
			base.Init();


			leftHandUI.Init();
			rightHandUI.Init();
		}
	}
}
