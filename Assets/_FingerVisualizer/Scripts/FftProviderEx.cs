using CSCore.DSP;
using CSCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerVisualizer
{
	public class FftProviderEx
	{
		private readonly int _channels;
		private readonly FftSize _fftSize;
		private readonly int _fftSizeExponent;
		private readonly Complex[] _storedSamples;
		private int _currentSampleOffset;
		private volatile bool _newDataAvailable;

		private readonly object _lockObject = new object();

		public FftSize Fftsize => _fftSize;

		public virtual bool IsNewDataAvailable => _newDataAvailable;

		public WindowFunction WindowFunction { get; set; }

		public FftProviderEx(int channels, FftSize fftSize)
		{
			if(channels < 1)
			{
				throw new ArgumentOutOfRangeException("channels");
			}

			var exponent = Math.Log((int)fftSize, 2);
			if (exponent % 1 != 0 || exponent == 0) throw new ArgumentOutOfRangeException("fftSize");

			_channels = channels;
			_fftSize = fftSize;
			_fftSizeExponent = (int)exponent;
			_storedSamples = new Complex[(int)fftSize];

			WindowFunction = WindowFunctions.None;
		}

		public virtual void Add(float left, float right)
		{
			lock (_lockObject)
			{
				_storedSamples[_currentSampleOffset].Imaginary = 0f;
				_storedSamples[_currentSampleOffset].Real = (left + right) / 2f;
				_currentSampleOffset++;

				if(_currentSampleOffset >= _storedSamples.Length)
				{
					_currentSampleOffset = 0;
				}
				_newDataAvailable = true;
			}
		}

		public virtual void Add(float[] samples, int count)
		{
			if (samples == null) throw new ArgumentNullException("samples");
			count -= count % _channels;
			if (count > samples.Length) throw new ArgumentOutOfRangeException("count");

			lock (_lockObject)
			{
				int blocksToProcess = count / _channels;
				for(int i=0; i< blocksToProcess;i += _channels)
				{
					_storedSamples[_currentSampleOffset].Imaginary = 0f;
					_storedSamples[_currentSampleOffset].Real = MergeSamples(samples, i, _channels);
					_currentSampleOffset++;

					if(_currentSampleOffset >= _storedSamples.Length)
					{
						_currentSampleOffset = 0;
					}
				}

				_newDataAvailable = blocksToProcess > 0;
			}
		}

		public virtual bool GetFftData(Complex[] fftResultBuffer)
		{
			if (fftResultBuffer == null)
				throw new ArgumentNullException("fftResultBuffer");

			if (fftResultBuffer.Length < (int)_fftSize)
				throw new ArgumentException("Length of array must be at least as long as the specified fft size.", "fftResultBuffer");

			var input = fftResultBuffer;
			bool result;
			lock (_lockObject)
			{
				//copy from block [offset - end] to input buffer
				Array.Copy(_storedSamples, _currentSampleOffset, input, 0,
					_storedSamples.Length - _currentSampleOffset);
				//copy from block [0 - offset] to input buffer
				Array.Copy(_storedSamples, 0, input, _storedSamples.Length - _currentSampleOffset,
					_currentSampleOffset);

				for (int i = 0; i < input.Length; i++)
				{
					var windowFunctionVal = WindowFunction(i, input.Length);
					input[i].Real *= windowFunctionVal;
				}

				result = _newDataAvailable;
				_newDataAvailable = false;
			}

			FastFourierTransformation.Fft(input, _fftSizeExponent);

			return result;
		}

		public virtual bool GetFftData(float[] fftResultBuffer)
		{
			if (fftResultBuffer == null)
				throw new ArgumentNullException("fftResultBuffer");

			if (fftResultBuffer.Length < (int)_fftSize)
				throw new ArgumentException("Length of array must be at least as long as the specified fft size.", "fftResultBuffer");
			var input = new Complex[(int)_fftSize];

			var result = GetFftData(input);

			for (int i = 0; i < input.Length; i++)
			{
				fftResultBuffer[i] = input[i];
			}

			//no need to set _newDataAvailable to false, since it got already set by the GetFftData(Complex[]) method.
			return result;
		}


		private float MergeSamples(float[] samples, int i, int channels)
		{
			if (channels == 1)
				return samples[i];
			if (channels == 2)
				return (samples[i] + samples[i + 1]) / 2f;
			if (channels == 3)
				return (samples[i] + samples[i + 1] + samples[i + 2]) / 3f;
			if (channels == 4)
				return (samples[i] + samples[i + 1] + samples[i + 2] + samples[i + 3]) / 4f;
			if (channels == 5)
				return (samples[i] + samples[i + 1] + samples[i + 2] + samples[i + 3] + samples[i + 4]) / 5f;
			if (channels == 6)
				return (samples[i] + samples[i + 1] + samples[i + 2] + samples[i + 3] + samples[i + 4] + samples[i + 5]) / 6f;

			float sample = 0;
			for (int j = i; j < channels; j++)
			{
				sample += samples[j++];
			}
			return sample / channels;
		}
	}
}
