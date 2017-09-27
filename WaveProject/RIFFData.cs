using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveProject {
    struct RIFFData {

        // RIFF Properties
        public byte[] ChunkID { get; set; }
        public int FileSize { get; set; }
        public byte[] RIFFType { get; set; }

        // FMT Properties
        public byte[] FormatID { get; set; }
        public int FormatSize { get; set; }
        public short FormatCode { get; set; }
        public short Channels { get; set; }
        public int SampleRate { get; set; }
        public int ByteRate { get; set; }
        public short BlockAlign { get; set; }
        public short BitsPerSample { get; set; }
        public byte[] SubChunkID { get; set; }
        public int SubChunkSize { get; set; }
        public byte[] Data { get; set; }
    }
}
