using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FingerVisualizer
{
    public enum FingerType
	{
        Thumb,
        Index,
        Middle,
        Ring,
        Pinky
	}


    [Serializable]
    public class Finger
    {
        public FingerType fingerType;
        public TargetFrequency targetFreq;
        public float indivisualMultiplier = 1f;
        public float bufferDecreaseAmount = 0.005f;
        public float bufferDecraseMultiplier = 1.2f;

        public string FingerAnimParam { get; private set; }

        private IVisualizer visualizer;
        private Animator handAnim;

        private float buffer;
        private float smoothVel = 0f;
        public void Init(Animator anim, IVisualizer visualizer)
		{
            this.visualizer = visualizer;
            handAnim = anim;
            SetAnimParam();
		}

        private void SetAnimParam()
		{
            switch (fingerType)
            {
                case FingerType.Thumb:
                    FingerAnimParam = "thumb";
                    break;

                case FingerType.Index:
                    FingerAnimParam = "index";
                    break;

                case FingerType.Middle:
                    FingerAnimParam = "middle";
                    break;

                case FingerType.Ring:
                    FingerAnimParam = "ring";
                    break;

                case FingerType.Pinky:
                    FingerAnimParam = "pinky";
                    break;
            }
        }
        public void ProcessAnimation()
		{
            float curFreq = visualizer.GetFrequencyBand(targetFreq);
            if(curFreq > buffer)
			{
                buffer = curFreq;
				buffer = Mathf.SmoothDamp(buffer, curFreq, ref smoothVel, Time.deltaTime * 20f);
			}
			else
			{

				buffer = Mathf.SmoothDamp(buffer, curFreq, ref smoothVel, Time.deltaTime * 50f);
            }

            handAnim.SetFloat(FingerAnimParam, Mathf.Clamp01(buffer * indivisualMultiplier ));
		}
	}


}
