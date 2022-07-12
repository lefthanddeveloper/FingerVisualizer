using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace FingerVisualizer
{
	[Serializable]
	class MenuButton
	{
		[SerializeField] private ButtonEventCaller eventCaller;
		[SerializeField] private Image iconImage;

		[SerializeField] private Color defaultColor = Color.white;
		[SerializeField] private Color hoverColor = Color.white;

		[SerializeField] private AnimationCurve spinAnimation;

		private Transform buttonTr;
		private Coroutine spinCoroutine = null;

		public void Init(Action onMenuClick)
		{
			if (eventCaller == null)
			{
				Debug.LogError("No Event Caller!");
				return;
			}
			buttonTr = eventCaller.GetComponent<Transform>();

			eventCaller.onClick.AddListener(() =>
			{
				onMenuClick?.Invoke();
			});

			eventCaller.onPointerEnter.AddListener(() => iconImage.color = hoverColor);
			eventCaller.onPointerExit.AddListener(() => iconImage.color = defaultColor);
		}

		public void Spin(float time)
		{
			if (spinCoroutine != null) eventCaller.StopCoroutine(spinCoroutine);
			spinCoroutine = eventCaller.StartCoroutine(StartSpin(time));			
		}

		IEnumerator StartSpin(float time)
		{
			//var curRotZ = buttonTr.localEulerAngles.z;
			//var destRotZ = -360f;
			var passedTime = 0f;
			var ratio = 0f;
			while (ratio < 1f)
			{
				passedTime += Time.deltaTime;
				ratio = passedTime / time;
				//float valueZ = Mathf.Lerp(curRotZ, destRotZ, ratio);
				float valueZ = spinAnimation.Evaluate(ratio);
				buttonTr.localEulerAngles = new Vector3(buttonTr.localEulerAngles.x, buttonTr.localEulerAngles.y, valueZ);
				yield return null;
			}
			buttonTr.transform.localEulerAngles = Vector3.zero;
			spinCoroutine = null;
		}
		
		
	}
}
