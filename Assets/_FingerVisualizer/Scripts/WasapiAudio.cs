using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerVisualizer
{
	public enum WasapiCaptureType 
	{
		Loopback,
		Microphone
	}

	class WasapiAudio
	{
		private const FftSize CFftSize = FftSize.Fft4096;
		private const float MaxAudioValue = 1.0f;

		private readonly WasapiCaptureType _captureType;
		private readonly Dictionary<SpectrumReceiver, SpectrumInfo> _spectrumInfos = new();

		private WasapiCapture wasapiCapture;
		private SoundInSource soundInSource;
		private SingleBlockNotificationStream singleBlockNotificationStream;
		private IWaveSource realtimeSource;


		public WasapiAudio(WasapiCaptureType captureType)
		{
			switch (captureType)
			{
				case WasapiCaptureType.Loopback:
					wasapiCapture = new WasapiLoopbackCapture();
					break;

				case WasapiCaptureType.Microphone:
					throw new InvalidOperationException("Not Ready yet");
					//break;

				default:
					throw new InvalidOperationException("Unhandled CaptureType");
			}

			wasapiCapture.Initialize();
			soundInSource = new SoundInSource(wasapiCapture);
			_captureType = captureType;

		}

		public void AddReceiver(SpectrumReceiver receiver)
		{
			WindowFunction windowFunction = null;

			switch (receiver.WindowFunctionType)
			{
				case WindowFunctionType.None:
					windowFunction = WindowFunctions.None;
					break;

				case WindowFunctionType.Hamming:
					windowFunction = WindowFunctions.Hamming;
					break;

				case WindowFunctionType.HammingPeriodic:
					windowFunction = WindowFunctions.HammingPeriodic;
					break;

				case WindowFunctionType.Hanning:
					windowFunction = WindowFunctions.Hanning;
					break;

				case WindowFunctionType.HanningPeriodic:
					windowFunction = WindowFunctions.HanningPeriodic;
					break;

				case WindowFunctionType.BlackmannHarris:
					windowFunction = WindowFunctions.BlackmannHarris;
					break;
			}

			var basicSpectrumProvider = new BasicSpectrumProvider(soundInSource.WaveFormat.Channels, soundInSource.WaveFormat.SampleRate, CFftSize, windowFunction);
			
			var lineSpectrum = new LineSpectrum(CFftSize, receiver.MinFreq, receiver.MaxFreq)
			{
				SpectrumProvider = basicSpectrumProvider,
				BarCount = receiver.SpectrumSize,
				UseAverage = true,
				IsXLogScale = true,
				ScalingStrategy = receiver.ScalingStrategy
			};

			_spectrumInfos.Add(receiver, new SpectrumInfo(basicSpectrumProvider, lineSpectrum));
		}

		public void StartCapture()
		{
			wasapiCapture.Start();

			var sampleSource = soundInSource.ToSampleSource();
			singleBlockNotificationStream = new SingleBlockNotificationStream(sampleSource);
			realtimeSource = singleBlockNotificationStream.ToWaveSource();

			var buffer = new byte[realtimeSource.WaveFormat.BytesPerSecond / 2];

			soundInSource.DataAvailable += (obj, ea) =>
			{
				while (realtimeSource.Read(buffer, 0, buffer.Length) > 0)
				{
					foreach(var spectrum in _spectrumInfos.Keys)
					{
						var spectrumInfo = _spectrumInfos[spectrum];
						var spectrumData = spectrumInfo.LineSpectrum.GetSpectrumData(MaxAudioValue);

						if(spectrumData != null)
						{
							spectrum.ReceiveAudio?.Invoke(spectrumData);
						}
					}
				}
			};

			singleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStream_SingleBlockRead;
		}

		public void StopCapture()
		{
			singleBlockNotificationStream.SingleBlockRead -= SingleBlockNotificationStream_SingleBlockRead;
			soundInSource.Dispose();
			realtimeSource.Dispose();
			wasapiCapture.Stop();
			wasapiCapture.Dispose();
		}
		private void SingleBlockNotificationStream_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
		{
			// Feed all of the spectra
			foreach (var spectrumInfo in _spectrumInfos.Values)
			{
				spectrumInfo.Provider.Add(e.Left, e.Right);
			}
		}

		private class SpectrumInfo
		{
			public BasicSpectrumProvider Provider { get; }
			public LineSpectrum LineSpectrum { get; }
			public SpectrumInfo(BasicSpectrumProvider provider, LineSpectrum lineSpectrum)
			{
				Provider = provider;
				LineSpectrum = lineSpectrum;
			}	
		}
	}
}
