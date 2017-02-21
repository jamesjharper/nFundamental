using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fundamental.Core.Memory;
using Fundamental.Wave.Container.Riff;
using Fundamental.Wave.Format;
using MiscUtil.IO;

namespace Fundamental.Wave.Container
{
    public class WaveHeader 
    {
        private readonly RiffHeader _header = new RiffHeader();



        public WaveFormat Format { get; set; }



        public void Read(Stream stream, Endianness endianness)
        {
            _header.Read(stream, endianness);

            if(_header.Type != "WAVE")
                throw new FormatException("Wave file header expects WAVE MMIO");



        }

        public void Write(EndianBinaryWriter binaryWriter)
        {
            throw new NotImplementedException();
        }
    }
}
