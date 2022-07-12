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
	class Menu
	{
		[SerializeField] private ButtonEventCaller eventCaller;
		[SerializeField] private Image buttonImage;

		[SerializeField] private float destPosY;

		[SerializeField] private Color defaultColor = Color.white;
		[SerializeField] private Color hoverColor = Color.white;

		private float originalPosY;
		private RectTransform rectTr;
		private Coroutine moveCoroutine = null;
		public void Init(Action onClickEvent)
		{
			rectTr = eventCaller.GetComponent<RectTransform>();
			originalPosY = rectTr.anchoredPosition3D.y;

			eventCaller.onClick.AddListener(()=>onClickEvent?.Invoke());

			eventCaller.onPointerEnter.AddListener(() => buttonImage.color = hoverColor);
			eventCaller.onPointerExit.AddListener(() => buttonImage.color = defaultColor);
		}

		public void OnExpand(float time)
		{
			if (moveCoroutine != null) eventCaller.StopCoroutine(moveCoroutine);
			moveCoroutine = eventCaller.StartCoroutine(MoveCoroutine(time, destPosY));
		}

		public void OnCondense(float time)
		{
			if (moveCoroutine != null) eventCaller.StopCoroutine(moveCoroutine);
			moveCoroutine = eventCaller.StartCoroutine(MoveCoroutine(time, originalPosY));
		}

		private IEnumerator MoveCoroutine(float time, float yPos)
		{
			var curY = rectTr.anchoredPosition.y;

			var passedTime = 0f;
			var ratio = 0f;
			while(ratio < 1f)
			{
				passedTime += Time.deltaTime;
				ratio = passedTime / time;
				curY = Mathf.Lerp(curY, yPos, ratio);
				rectTr.anchoredPosition3D = new Vector3(rectTr.anchoredPosition3D.x, curY, rectTr.anchoredPosition3D.z);
				yield return null;
			}
			rectTr.anchoredPosition3D = new Vector3(rectTr.anchoredPosition3D.x, yPos, rectTr.anchoredPosition3D.z);
			moveCoroutine = null;
		}
	}
}
