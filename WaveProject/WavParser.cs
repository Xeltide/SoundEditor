using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WaveProject {
    class WavParser {

        public WavParser(String filePath) {
            this.filePath = filePath;
            LoadBytes();
        }

        public void LoadBytes() {
            try {
                MemoryStream ms = new MemoryStream();
                this.stream = new FileStream(filePath, FileMode.Open);

                stream.CopyTo(ms);
                file = ms.ToArray();

                this.stream.Close();
                ms.Close();
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        public RIFFData ParseBytes() {
            RIFFData output = new RIFFData();

            // RIFF Properties
            output.ChunkID = new byte[4];
            Array.Copy(file, 0, output.ChunkID, 0, 4);
            output.FileSize = BitConverter.ToInt32(file, 4);
            Console.WriteLine(output.FileSize + ", " + (output.FileSize - 44));
            output.RIFFType = new byte[4];
            Array.Copy(file, 8, output.RIFFType, 0, 4);

            // FMT Properties
            output.FormatID = new byte[4];
            Array.Copy(file, 12, output.FormatID, 0, 4);
            output.FormatSize = BitConverter.ToInt32(file, 16);
            output.FormatCode = BitConverter.ToInt16(file, 20);
            output.Channels = BitConverter.ToInt16(file, 22);
            output.SampleRate = BitConverter.ToInt32(file, 24);
            output.ByteRate = BitConverter.ToInt32(file, 28);
            output.BlockAlign = BitConverter.ToInt16(file, 32);
            output.BitsPerSample = BitConverter.ToInt16(file, 34);

            output.SubChunkID = new byte[4];
            Array.Copy(file, 36, output.SubChunkID, 0, 4);
            output.SubChunkSize = BitConverter.ToInt32(file, 40);

            // Skip non 'data' SubChunks
            int i = 44;
            while (!Encoding.ASCII.GetString(output.SubChunkID).Equals("data")) {
                i += output.SubChunkSize;
                Array.Copy(file, i, output.SubChunkID, 0, 4);
                i += 4;
                output.SubChunkSize = BitConverter.ToInt32(file, i);
                i += 4;
            }

            output.Data = new byte[output.SubChunkSize];
            Array.Copy(file, i, output.Data, 0, output.SubChunkSize);

            return output;
        }

        private String filePath;
        private FileStream stream;
        private byte[] file;
    }
}
