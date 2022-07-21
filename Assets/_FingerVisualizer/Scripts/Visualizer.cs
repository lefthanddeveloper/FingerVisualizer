using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FingerVisualizer
{
    public enum TargetFrequency
    {
        Freq150 =0,
        Freq300 = 1,
        Freq450 = 2,
        Freq600 = 3,
        Freq900 = 4,
        Freq1500 = 5,
        Freq2500 = 6,
        Freq4000 = 7,
        Freq8000 = 8,
        Freq20000 = 9,
	}

	public interface IVisualizer 
    {
        float GetFrequencyBand(TargetFrequency targetFreq);
    }


	public class Visualizer : MonoBehaviour, IVisualizer
    {
        [Header(" [ Visualizer Setting]")]
        
        
        [SerializeField] private WasapiAudioSource wasapiAudioSource;
        [SerializeField] private ScalingStrategy scalingStrategy = ScalingStrategy.Sqrt;
        [SerializeField] private WindowFunctionType windowFunctionType = WindowFunctionType.BlackmannHarris;
        [SerializeField] private int minFreq = 0;
        [SerializeField] private int maxFreq = 20000;
        [SerializeField] private float multiplier = 0.2f;
        public float IntensityMultiplier => multiplier;

        private float[] rawSpectrumData;
        private float[] frequencyBands;

        private int spectrumSize = 512;
        private int numOfBand = 10;
        private int[] numOfSampleForBand;
        private float[] wantedSampleBand = new float[] { 150, 300, 450, 600, 900, 1500, 2500, 4000, 8000, 20000 };
        
		protected virtual void Start()
		{
            if (wasapiAudioSource == null)
			{
                Debug.LogError("WasapiAudioSource needed!");
                return;
			}
            frequencyBands = new float[numOfBand];

            FindNumberofSamplesForEachBand();
            var receiver = new SpectrumReceiver(spectrumSize, scalingStrategy, windowFunctionType, minFreq, maxFreq, (data) =>
             {
                 rawSpectrumData = data;
				 ProcessFrequencyBand();
			 });

            wasapiAudioSource.AddReceiver(receiver);
	    }

        private void FindNumberofSamplesForEachBand()
		{
            // per sample hertz(psh) & sample index relationship
            //0-490 y = 4*10^(-7)*x^3 - 0.0002x^2 + 0.0417x + 4.4173
            //491-512 y = 0.9517x + 44.72

            numOfSampleForBand = new int[numOfBand];

            int targetValueIndex = 0;
            float pshSum=0f;
            for(int i=0; i < spectrumSize; i++)
			{
                if(i < 491)
				{
                    pshSum += (4f * Mathf.Pow(10,-7) * Mathf.Pow(i, 3f)) - (0.0002f * Mathf.Pow(i, 2f)) + (0.0417f*i) + 4.4173f;
				}
				else
				{
                    pshSum += 0.9517f * i + 44.72f;
				}


                if(pshSum >= wantedSampleBand[targetValueIndex])
				{
                    int numOfSamples = GetNumOfSamples(i, targetValueIndex);
                    numOfSampleForBand[targetValueIndex] = numOfSamples;
                    targetValueIndex++;
				}

                if(targetValueIndex == numOfBand - 1) // if last one
				{
                    numOfSampleForBand[targetValueIndex] = 512 - i;
                    return;
				}
			}           
		}

        private int GetNumOfSamples(int cumulativeSamples, int curIndex)
		{
            int result = cumulativeSamples;
            for(int i=0; i< numOfSampleForBand.Length; i++)
			{
                if(i < curIndex)
				{
                    result -= numOfSampleForBand[i];
				}
			}

            return result;
		}

        private void ProcessFrequencyBand()
		{
            // 0 = 156 hz
            // 1 = 300 hz
            // 2 = 450 hz
            // 3 = 600 hz
            // 4 = 900 hz
            // 5 = 1500 hz
            // 6 = 2500 hz
            // 7 = 4000 hz
            // 8 = 8000 hz
            // 9 = 20000 hz

            int overallCount =0;
            for(int i =0;i <numOfBand; i++)
			{
                float average = 0;
                int sampleCount = numOfSampleForBand[i];
                for(int j =0; j< sampleCount; j++)
				{
                    average += rawSpectrumData[overallCount] * (overallCount + 1);
                    overallCount++;
				}

                average /= overallCount;
                frequencyBands[i] = average * multiplier ;
			}
        }

        public float GetFrequencyBand(TargetFrequency targetFreq)
		{
            return frequencyBands[(int)targetFreq];
		}

        public void ChangeOverallMultiplier(float newMultiplier)
        {
            multiplier = newMultiplier;
        }

    }

}
