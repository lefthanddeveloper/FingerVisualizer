using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AspectRatioFixer : MonoBehaviour
{
	private Vector2 aspectRatio = new Vector2(16, 9);
	private float lastWidth;
	private float lastHeight;

	[SerializeField] private TMP_Text text_width;
	[SerializeField] private TMP_Text text_height;
	void Start()
	{
		//Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
		//lastWidth = Screen.width;
		//lastHeight = Screen.height;
	}

	void Update()
	{
		//if (lastWidth != Screen.width)
		//{
		//	Screen.SetResolution(Screen.width, (int)(Screen.width * (9f / 16f)), FullScreenMode.Windowed);
		//}
		//else if (lastHeight != Screen.height)
		//{
		//	Screen.SetResolution((int)(Screen.height * (16f / 9f)), Screen.height, FullScreenMode.Windowed);
		//}

		//lastWidth = Screen.width;
		//lastHeight = Screen.height;
		SetRatio();
	}

	private void SetRatio()
	{
		text_width.text = Screen.width.ToString();
		text_height.text = Screen.height.ToString();

		float curWidth = (float)Screen.width;
		float curHeight = (float)Screen.height;

		if( curWidth/curHeight > 16f / 9f)
		{
			Screen.SetResolution(Screen.width, Screen.width * 9 / 16, false);
		}
		else if(curHeight/curWidth > 9f / 16f)
		{
			Screen.SetResolution(Screen.height * 16/9, Screen.height, false);
		}
	}
}
