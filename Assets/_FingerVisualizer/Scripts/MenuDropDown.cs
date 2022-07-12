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
	class MenuDropDown
	{
		[SerializeField] private Image backgroundImage;
		[SerializeField] private float backgroundExpandHeight = 500f;

		private RectTransform backgroundRectTr;
		private float backgroundOrigianlHeight;
		private Coroutine backgroundHeightCoroutine;
		
		public bool isExpanded { get; private set; } = false;

		public void Init()
		{
			backgroundRectTr = backgroundImage.rectTransform;
			backgroundOrigianlHeight = backgroundRectTr.rect.height;
		}

		public void Expand(float time)
		{
			ExpandBackground(time);
		}

		public void Condense(float time)
		{
			CondenseBackground(time);
		}

		private void CondenseBackground(float time)
		{
			if (backgroundHeightCoroutine != null) backgroundImage.StopCoroutine(backgroundHeightCoroutine);
			backgroundHeightCoroutine = backgroundImage.StartCoroutine(BackgroundChangeHeightCoroutine(time, backgroundOrigianlHeight));
			isExpanded = false;
		}

		private void ExpandBackground(float time)
		{
			if (backgroundHeightCoroutine != null) backgroundImage.StopCoroutine(backgroundHeightCoroutine);
			backgroundHeightCoroutine = backgroundImage.StartCoroutine(BackgroundChangeHeightCoroutine(time, backgroundExpandHeight));
			isExpanded = true;
		}

		private IEnumerator BackgroundChangeHeightCoroutine(float time, float destHeight)
		{
			var curHeight = backgroundRectTr.rect.height;

			var passedTime = 0f;
			var ratio = 0f;
			while(ratio < 1f)
			{
				passedTime += Time.deltaTime;
				ratio = passedTime / time;
				curHeight = Mathf.Lerp(curHeight, destHeight, ratio);
				backgroundRectTr.sizeDelta = new Vector2(backgroundRectTr.sizeDelta.x, curHeight);
				yield return null;
			}
			backgroundRectTr.sizeDelta = new Vector2(backgroundRectTr.sizeDelta.x, destHeight);
			backgroundHeightCoroutine = null;
		}

	}
}
