using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WaveProject {
    class WavPackager {

        public WavPackager(String filePath) {
            this.filePath = filePath;
        }

        public void WriteFile(RIFFData riff) {
            try {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                BinaryWriter wr = new BinaryWriter(fs);

                // RIFF Properties
                wr.Write(riff.ChunkID);
                wr.Write(riff.FileSize);
                wr.Write(riff.RIFFType);

                // FMT Properties
                wr.Write(riff.FormatID);
                wr.Write(riff.FormatSize);
                wr.Write(riff.FormatCode);
                wr.Write(riff.Channels);
                wr.Write(riff.SampleRate);
                wr.Write(riff.ByteRate);
                wr.Write(riff.BlockAlign);
                wr.Write(riff.BitsPerSample);
                wr.Write(riff.SubChunkID);
                wr.Write(riff.SubChunkSize);
                wr.Write(riff.Data);

                wr.Close();
                fs.Close();
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        public RIFFData GenerateRIFFData(AudioPanel panel) {
            RIFFData riff = new RIFFData();

            // RIFF Properties
            char[] chunkID = { 'R', 'I', 'F', 'F'};
            riff.ChunkID = Encoding.ASCII.GetBytes(chunkID);
            char[] riffType = { 'W', 'A', 'V', 'E' };
            riff.RIFFType = Encoding.ASCII.GetBytes(riffType);

            // FMT Properties
            char[] formatID = { 'f', 'm', 't', ' ' };
            riff.FormatID = Encoding.ASCII.GetBytes(formatID);
            riff.FormatSize = 16;
            riff.FormatCode = 1;
            if (panel.Menu.IsMono) {
                riff.Channels = 1;
            } else {
                riff.Channels = 2;
            }
            riff.BitsPerSample = panel.Menu.manager.bits[panel.Menu.manager.checkedBits].Value;

            // Check that loaded file is standard samplerate
            // Use the loaded value if not selectable
            if (panel.PanelData.RIFFData.SampleRate != panel.Menu.manager.khz[panel.Menu.manager.checkedKHZ].Value && panel.PanelData.RIFFData.SampleRate != 0)
            {
                riff.SampleRate = panel.PanelData.RIFFData.SampleRate;
            } else
            {
                riff.SampleRate = panel.Menu.manager.khz[panel.Menu.manager.checkedKHZ].Value;
            }
            riff.ByteRate = riff.BitsPerSample / 8 * riff.SampleRate * riff.Channels;
            riff.BlockAlign = (short)(riff.Channels * riff.BitsPerSample / 8);
            char[] subChunkID = { 'd', 'a', 't', 'a' };
            riff.SubChunkID = Encoding.ASCII.GetBytes(subChunkID);

            // Set the data chunk to be loaded data or null
            if (panel.PanelData.RawChannelData != null)
            {
                riff.Data = Interleave_Bytes(panel, riff);
                riff.SubChunkSize = riff.Data.Length;
                Console.WriteLine("Data size:" + riff.Data.Length);
            } else
            {
                riff.SubChunkSize = 0;
                riff.Data = null;
            }
            riff.FileSize = 36 + riff.SubChunkSize;

            return riff;
        }

        public byte[] Interleave_Bytes(AudioPanel panel, RIFFData riff)
        {
            int size = panel.PanelData.RawChannelData[0].Count;
            List<byte> output = new List<byte>();
            int inc = riff.BitsPerSample / 8;
            for (int i = 0; i < size; i += inc)
            {
                for (int ch = 0; ch < riff.Channels; ch++)
                {
                    for (int b = 0; b < inc; b++)
                    {
                        output.Add(panel.PanelData.RawChannelData[ch][i + b]);
                    }
                }
            }

            return output.ToArray();
        }

        private String filePath;
    }
}
