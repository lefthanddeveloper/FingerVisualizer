using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerVisualizer
{
	public enum ScalingStrategy
	{
		Decibel,
		Linear,
		Sqrt
	}

	public enum WindowFunctionType
	{
		None,
		Hamming,
		HammingPeriodic,
		Hanning,
		HanningPeriodic,
		BlackmannHarris
	}

	public class SpectrumReceiver
	{
		public int SpectrumSize { get; }
		public ScalingStrategy ScalingStrategy { get; }
		public WindowFunctionType WindowFunctionType { get; }
		public int MinFreq { get; }
		public int MaxFreq { get; }
		public Action<float[]> ReceiveAudio { get; }

		public SpectrumReceiver(int spectrumSize, ScalingStrategy scalingStrategy,
								WindowFunctionType windowFunctionType, int minFreq, int maxFreq,
								Action<float[]> receiveAudio)
		{
			SpectrumSize = spectrumSize;
			ScalingStrategy = scalingStrategy;
			WindowFunctionType = windowFunctionType;
			MinFreq = minFreq;
			MaxFreq = maxFreq;
			ReceiveAudio = receiveAudio;
		}
	}
}
