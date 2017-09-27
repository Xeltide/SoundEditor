using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace WaveProject {
    /* Stores common data between Audio / Frequency Channels.
     * Should be used to load in data when files are opened in a new channel.
     */
    class AudioPanelData {

        public AudioPanelData() {

        }

        public void SetFilePath(String filePath) {
            WavParser parser = new WavParser(filePath);
            RIFFData = parser.ParseBytes();
        }

        public void SplitWavData() {
            if (RIFFData.ChunkID != null) {

                List<List<byte>> output = new List<List<byte>>();

                RIFFData data = RIFFData;
                for (int i = 0; i < data.Channels; i++) {
                    List<byte> channel = new List<byte>(data.SubChunkSize / data.Channels);
                    output.Add(channel);
                }

                int inc = data.Channels * (data.BitsPerSample / 8);
                for (int b = 0; b < data.Data.Length; b += inc) {
                    for (int ch = 0; ch < data.Channels; ch++) {
                        for (int s = 0; s < data.BitsPerSample / 8; s++) {
                            output[ch].Add(data.Data[b + s + (ch * data.BitsPerSample / 8)]);
                        }
                    }
                }

                RawChannelData = output;
            }
        }

        public List<int> GetReadableChannel(int channelIndex) {
            byte[] rawChannel = RawChannelData[channelIndex].ToArray();
            List<int> output;

            switch (RIFFData.BitsPerSample) {
                case 8:
                    output = new List<int>(rawChannel.Length * 8 / RIFFData.BitsPerSample);
                    for (int i = 0; i < rawChannel.Length; i++) {
                        output.Add((int)rawChannel[i]);
                    }

                    return output;
                case 16:
                    output = new List<int>(rawChannel.Length * 8 / RIFFData.BitsPerSample);
                    for (int i = 0; i < rawChannel.Length; i += 2) {
                        output.Add(BitConverter.ToInt16(rawChannel, i));
                    }

                    return output;
                case 32:
                    output = new List<int>(rawChannel.Length * 8 / RIFFData.BitsPerSample);
                    for (int i = 0; i < rawChannel.Length; i += 4) {
                        output.Add(BitConverter.ToInt32(rawChannel, i));
                    }

                    return output;
                default:
                    System.Diagnostics.Debug.WriteLine(RIFFData.BitsPerSample + " bit sample rate");
                    output = new List<int>();
                    return output;
            }
        }

        public RIFFData RIFFData { get; set; }
        public List<List<byte>> RawChannelData { get; set; }
    }
}
