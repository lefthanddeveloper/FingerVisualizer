using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FingerVisualizer 
{
    public class HandUI : MonoBehaviour
    {
        [SerializeField] private FingerVisualizer fingerVisualizer;
        private FingerBandUI[] fingerBands;

        public void Init()
        {
            fingerBands = GetComponentsInChildren<FingerBandUI>();

            for(int i=0; i< fingerBands.Length; i++)
			{
                var theFinger = fingerVisualizer.GetFinger(fingerBands[i].FingerType);
                fingerBands[i].Init(theFinger);
			}
        }

        public void OnClickResetFreq()
		{
            foreach(var ui in fingerBands)
			{
                ui.OnResetFreq();
			}
		}

        public void OnClickResetIntensity()
		{
            foreach(var ui in fingerBands)
			{
                ui.OnResetIntensity();
			}
		}

        public void ChangeOverallIntensity(float newValue)
        {
            fingerVisualizer.ChangeOverallMultiplier(newValue);
        }

        

    }

}
