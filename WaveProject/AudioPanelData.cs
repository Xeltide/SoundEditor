using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace WaveProject
{
    /* Stores common data between Audio / Frequency Channels.
     * Should be used to load in data when files are opened in a new channel.
     */
    class AudioPanelData
    {

        public AudioPanelData()
        {
            RawChartData = new List<List<int>>();
        }

        public void SetFilePath(String filePath)
        {
            WavParser parser = new WavParser(filePath);
            RIFFData = parser.ParseBytes();
        }

        public void SplitWavData()
        {
            if (RIFFData.ChunkID != null)
            {

                List<List<byte>> output = new List<List<byte>>();

                RIFFData data = RIFFData;
                for (int i = 0; i < data.Channels; i++)
                {
                    List<byte> channel = new List<byte>(data.SubChunkSize / data.Channels);
                    output.Add(channel);
                }

                int inc = data.Channels * (data.BitsPerSample / 8);
                for (int b = 0; b < data.Data.Length; b += inc)
                {
                    for (int ch = 0; ch < data.Channels; ch++)
                    {
                        for (int s = 0; s < data.BitsPerSample / 8; s++)
                        {
                            output[ch].Add(data.Data[b + s + (ch * data.BitsPerSample / 8)]);
                        }
                    }
                }

                RawChannelData = output;
                PreSaveBuffer = new List<List<byte>>(RawChannelData);
            }
        }

        public List<int> GetReadableChannel(int channelIndex)
        {
            List<int> output = new List<int>();
            if (RawChannelData != null)
            {
                byte[] rawChannel = RawChannelData[channelIndex].ToArray();

                switch (RIFFData.BitsPerSample)
                {
                    case 8:
                        for (int i = 0; i < rawChannel.Length; i++)
                        {
                            output.Add((int)rawChannel[i]);
                        }

                        return output;
                    case 16:
                        for (int i = 0; i < rawChannel.Length; i += 2)
                        {
                            output.Add(BitConverter.ToInt16(rawChannel, i));
                        }

                        return output;
                    case 24:
                        for (int i = 0; i < rawChannel.Length; i += 3)
                        {
                            byte[] temp = new byte[4];
                            temp[0] = 0;
                            temp[1] = rawChannel[i];
                            temp[2] = rawChannel[i + 1];
                            temp[3] = rawChannel[i + 2];
                            output.Add(BitConverter.ToInt32(temp, 0));
                        }

                        return output;
                    case 32:
                        for (int i = 0; i < rawChannel.Length; i += 4)
                        {
                            output.Add(BitConverter.ToInt32(rawChannel, i));
                        }

                        return output;
                    default:
                        System.Diagnostics.Debug.WriteLine(RIFFData.BitsPerSample + " bit sample rate");
                        output = new List<int>();
                        return output;
                }
            }
            Console.WriteLine("Null RawChannelData");
            return output;
        }

        public void UpdatePreSaveBuffer(List<int> input, int channelIndex)
        {
            List<byte> output;
            switch (RIFFData.BitsPerSample)
            {
                case 8:
                    output = new List<byte>(input.Count);
                    for (int i = 0; i < input.Count; i++)
                    {
                        output.Add((byte)input[i]);
                    }
                    break;
                case 16:
                    output = new List<byte>(input.Count * 2);
                    for (int i = 0; i < input.Count; i++)
                    {
                        output.AddRange(BitConverter.GetBytes((Int16)input[i]));
                    }
                    RawChannelData[channelIndex] = output;
                    break;
                case 32:
                    break;
                default:
                    break;
            }
        }

        public List<byte> IntToByteList(List<int> input, RIFFData riff)
        {
            List<byte> output = new List<byte>();
            switch (riff.BitsPerSample)
            {
                case 8:
                    for (int i = 0; i < input.Count; i++)
                    {
                        output.Add(Convert.ToByte(input[i]));
                    }
                    break;
                case 16:
                    for (int i = 0; i < input.Count; i++)
                    {
                        output.AddRange(BitConverter.GetBytes((Int16)input[i]));
                    }
                    break;
                case 24:
                    for (int i = 0; i < input.Count; i++)
                    {
                        byte[] temp = BitConverter.GetBytes((Int32)input[i]);
                        output.Add(temp[1]);
                        output.Add(temp[2]);
                        output.Add(temp[3]);
                    }
                    break;
                case 32:
                    for (int i = 0; i < input.Count; i++)
                    {
                        output.AddRange(BitConverter.GetBytes((Int32)input[i]));
                    }
                    break;
            }

            return output;
        }

        public RIFFData RIFFData { get; set; }
        public List<List<byte>> RawChannelData { get; set; }
        public List<List<byte>> PreSaveBuffer { get; set; }
        public List<List<int>> RawChartData { get; set; }
        public List<List<int>> IntBuffer { get; set; }
        public List<byte> ByteBuffer { get; set; }
    }
}
