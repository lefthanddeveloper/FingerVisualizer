using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FingerVisualizer
{
	[Serializable]
	public class Menu : MonoBehaviour
	{
		[SerializeField] private ButtonEventCaller eventCaller;
		[SerializeField] protected GameObject panel;
		[SerializeField] private Image buttonImage;
		[SerializeField] private float destPosY;
		[SerializeField] private Color defaultColor = Color.white;
		[SerializeField] private Color hoverColor = Color.white;

		private MenuController menuController;
		private float originalPosY;
		private RectTransform rectTr;
		private Coroutine moveCoroutine = null;
		public virtual void Init()
		{
			menuController = GetComponentInParent<MenuController>();

			rectTr = eventCaller.GetComponent<RectTransform>();
			originalPosY = rectTr.anchoredPosition3D.y;

			eventCaller.onClick.AddListener(OnClick);
			eventCaller.onPointerEnter.AddListener(OnPointerEnter);
			eventCaller.onPointerExit.AddListener(OnPointerExit);

			HidePanel();
		}

		protected virtual void OnPointerEnter()
		{
			buttonImage.color = hoverColor;
		}

		protected virtual void OnPointerExit()
		{
			buttonImage.color = defaultColor;
		}

		protected virtual void OnClick()
		{
			menuController.OnMenuSelected(this);
		}

		public void OnExpand(float time)
		{
			if (moveCoroutine != null) StopCoroutine(moveCoroutine);
			moveCoroutine = StartCoroutine(MoveCoroutine(time, destPosY));
		}

		public void OnCondense(float time)
		{
			if (moveCoroutine != null) StopCoroutine(moveCoroutine);
			moveCoroutine = StartCoroutine(MoveCoroutine(time, originalPosY));
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

		public void OnSelected()
		{
			if (panel.activeSelf)
			{
				HidePanel();
			}
			else
			{
				panel.SetActive(true);
			}
		}

		public void HidePanel()
		{
			panel.SetActive(false);
		}
	}
}
