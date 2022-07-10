using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FingerVisualizer
{
    public class WasapiAudioSource : MonoBehaviour
    {
		[SerializeField] private WasapiCaptureType captureType = WasapiCaptureType.Loopback;


        private WasapiAudio wasapiAudio;

		private void Awake()
		{
			Init();
		}

		private void OnApplicationQuit()
		{
			wasapiAudio?.StopCapture();
		}

		public void AddReceiver(SpectrumReceiver receiver)
		{
			wasapiAudio.AddReceiver(receiver);
		}

		private void Init()
		{
			if(wasapiAudio == null)
			{
				wasapiAudio = new WasapiAudio(captureType);
				wasapiAudio.StartCapture();

			}
		}
	}

}

