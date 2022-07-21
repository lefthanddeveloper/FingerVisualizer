using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FingerVisualizer
{
    public class FingerBandUI : MonoBehaviour
    {
        [SerializeField] private FingerType fingerType;
        [SerializeField] private RectTransform fingerImgTr;
        [SerializeField] private RectTransform fillImgTr;

        [SerializeField] private TMP_Dropdown freqDropDown;
        [SerializeField] private Slider intensitySlider;
        [SerializeField] private AnimationCurve intensityConverter;

        private Finger finger;
        private int originalFingerIndex;
        private float originalIntensity;


        public FingerType FingerType => fingerType;
        
        public void Init(Finger finger)
		{
            this.finger = finger;

            originalFingerIndex = (int)finger.targetFreq;
            originalIntensity = intensitySlider.value;

            InitFreqDropdown(originalFingerIndex);

            InitIntensitySlider();
		}

		private void InitIntensitySlider()
		{
            intensitySlider.onValueChanged.AddListener((newValue) =>
            {
                //newValue = intensityConverter.Evaluate(newValue);
                float newIntensity = Mathf.Pow(newValue, 2f);
                finger.ChangeIntensity(newIntensity);
            });
		}

		
		void LateUpdate()
        {
            float fillHeight = fingerImgTr.sizeDelta.y * finger.GetCurrentFingerVisualValue();
            fillImgTr.sizeDelta = new Vector2(fillImgTr.sizeDelta.x, fillHeight);
        }

        private void InitFreqDropdown(int initIndex)
		{
            freqDropDown.value = initIndex;
            freqDropDown.onValueChanged.AddListener(OnFreqDropDownChanged);
        }

        private void OnFreqDropDownChanged(int index)
		{
            print("OnFreqDropDOwnChanged called!");
            TargetFrequency newFreq = (TargetFrequency)index;
            finger.ChangeTargetFreq(newFreq);
		}

        public void OnResetFreq()
		{
            freqDropDown.value = originalFingerIndex;
        }

        public void OnResetIntensity()
		{
            intensitySlider.value = originalIntensity;
		}
    }
}

