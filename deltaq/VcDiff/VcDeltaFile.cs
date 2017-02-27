using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace deltaq.VcDiff
{
    class VcDeltaFile
    {
        private byte[] Header;
        private byte HeaderIndicator;

        public VcDeltaFile(Stream stream)
        {

        }

        private void Initialize(Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                Header = reader.ReadBytes(4);
                if(Header[0] != 0xD6 || Header[1] != 0xC3 || Header[2] != 0xC4)
                {
                    throw new InvalidDataException("Header does not match VCDIFF format");
                }

                HeaderIndicator = reader.ReadByte();

                const byte VCD_DECOMPRESS = 1;
                const byte VCD_CODETABLE = 2;
                if((HeaderIndicator & VCD_DECOMPRESS) != 0)
                {
                    //secondary compressor
                    var secondaryCompressorId = reader.ReadByte();
                }

                if((HeaderIndicator & VCD_CODETABLE) != 0)
                {
                    //application defined code table
                    var codeTableLength = reader.ReadInt32();
                    var sizeNear = reader.ReadByte();
                    var sizeSame = reader.ReadByte();
                    var codeTable = reader.ReadBytes(codeTableLength);
                }

                //secondaryCompressorId byte
                //Length of CodeTableData int
                //CodeTable Data
                //  size of near cache byte
                //  size of same cache byte
                //  compressed code table data
            }
        }
    }
}
