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
                wr.Write(44 + riff.SubChunkSize);
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
            riff.FileSize = 44;
            char[] riffType = { 'W', 'A', 'V', 'E' };
            riff.RIFFType = Encoding.ASCII.GetBytes(riffType);

            // FMT Properties
            char[] formatID = { 'f', 'm', 't', ' ' };
            riff.FormatID = Encoding.ASCII.GetBytes(formatID);
            riff.FormatSize = 24;
            riff.FormatCode = 1;
            if (panel.Menu.IsMono) {
                riff.Channels = 1;
            } else {
                riff.Channels = 2;
            }
            //riff.SampleRate = 
            

            return riff;
        }

        private String filePath;
    }
}
