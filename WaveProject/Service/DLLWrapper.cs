using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WaveProject.Service
{
    unsafe class DLLWrapper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct WaveFormatEx
        {
            public ushort wFormatTag;
            public ushort nChannels;
            public uint nSamplesPerSec;
            public uint nAvgBytesPerSec;
            public ushort nBlockAlign;
            public ushort wBitsPerSample;
            public ushort cbSize;
        }

        [DllImport("AudioController.dll")]
        private static extern bool BeginRecording();
        [DllImport("AudioController.dll")]
        private static extern bool EndRecording();
        [DllImport("AudioController.dll")]
        private static extern bool PlayBuffer();
        [DllImport("AudioController.dll")]
        private static extern bool PauseBuffer();
        [DllImport("AudioController.dll")]
        private static extern byte* GetSaveBuffer();
        [DllImport("AudioController.dll")]
        private static extern void SetPlayBuffer(byte* buffer, int size);
        [DllImport("AudioController.dll")]
        private static extern int GetBufferSize();
        [DllImport("AudioController.dll")]
        private static extern bool StopBuffer();
        [DllImport("AudioController.dll", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)]
        private static extern WaveFormatEx* GetWaveFormat();
        [DllImport("AudioController.dll")]
        private static extern bool IsPlaying();
        [DllImport("AudioController.dll")]
        private static extern bool IsPaused();

        private DLLWrapper() { }

        public static void Record(RIFFData riff)
        {
            LoadWaveFormat(riff);
            BeginRecording();
        }

        public static byte[] RecordStop()
        {
            Safe = false;
            EndRecording();

            byte* save = GetSaveBuffer();
            int size = GetBufferSize();
            byte[] output = new byte[size];
            for (int i = 0; i < size; i++)
            {
                output[i] = save[i];
            }
            Safe = true;

            return output;
        }

        public static void Play()
        {
            if (GetBufferSize() != 0)
            {
                PlayBuffer();
            }
        }

        public static void Pause()
        {
            PauseBuffer();
        }

        public static void Stop()
        {
            StopBuffer();
        }

        public static bool IsPlayingDLL()
        {
            return IsPlaying();
        }

        public static bool IsPausedDLL()
        {
            return IsPaused();
        }

        public static void SetPlayBuffer(RIFFData riff)
        {
            LoadWaveFormat(riff);
            
            if (!(riff.Data == null || riff.Data.Length == 0))
            {
                fixed (byte* pointer = &riff.Data[0])
                {
                    SetPlayBuffer(pointer, riff.Data.Length);
                }
            }
        }

        private static void LoadWaveFormat(RIFFData riff)
        {
            WaveFormatEx* format = GetWaveFormat();
            format->nChannels = (ushort)riff.Channels;
            format->nSamplesPerSec = (uint)riff.SampleRate;
            format->nAvgBytesPerSec = (uint)(format->nSamplesPerSec * riff.BlockAlign);
            format->nBlockAlign = (ushort)riff.BlockAlign;
            format->wBitsPerSample = (ushort)riff.BitsPerSample;
        }

        private static bool safe = true;
        public static bool Safe {
            get { return safe; }
            set { safe = value; }
        }
    }
}
