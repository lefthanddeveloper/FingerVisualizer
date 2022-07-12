using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace FingerVisualizer
{
    public class ButtonEventCaller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
		public UnityEvent onClick, onPointerEnter, onPointerExit;

		public void OnPointerEnter(PointerEventData eventData)
		{
			onPointerEnter?.Invoke();
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			onPointerExit?.Invoke();
		}
		public void OnPointerClick(PointerEventData eventData)
		{
			onClick?.Invoke();
		}
    }
}

