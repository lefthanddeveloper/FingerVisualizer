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

        public FingerType FingerType => fingerType;

        
        
        
        
        private Finger finger;
        
        public void Init(Finger finger)
		{
            this.finger = finger;

            InitFreqDropdown((int)finger.targetFreq);
		}

        
        void LateUpdate()
        {
            float fillHeight = fingerImgTr.sizeDelta.y * finger.GetCurrentBuffer();
            fillImgTr.sizeDelta = new Vector2(fillImgTr.sizeDelta.x, fillHeight);
        }

        private void InitFreqDropdown(int initIndex)
		{
            freqDropDown.value = initIndex;
            freqDropDown.onValueChanged.AddListener(OnFreqDropDownChanged);
        }

        private void OnFreqDropDownChanged(int index)
		{
            TargetFrequency newFreq = (TargetFrequency)index;
            finger.ChangeTargetFreq(newFreq);
		}

        
    }
}

