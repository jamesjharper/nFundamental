using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fundamental.Core.Memory;
using Fundamental.Core.Tests.Math;
using Fundamental.Wave.Container;
using Fundamental.Core.AudioFormats;
using Fundamental.Wave.Format;
using MiscUtil.Conversion;
using NUnit.Framework;
namespace Fundamental.Core.Tests.Container
{

    //[TestFixture]
    //public class WaveFileFormatTests
    //{

    //    // Read

    //    [Test]
    //    public void CanReadLittleEndianWaveFile()
    //    {
    //        // -> ARRANGE:
    //        var memoryStream = new MemoryStream();

    //        // Write Riff Header

    //        #region Write RIFF header

    //        // Write RIFF type Id
    //        var riffFileSignature = new byte[] { 0x52, 0x49, 0x46, 0x46 };
    //        memoryStream.Write(riffFileSignature);

    //        // Write byte size of the whole format
    //        var contentSizeBytes = EndianHelpers.ToLittleEndianBytes(48);
    //        memoryStream.Write(contentSizeBytes);


    //        // Write WAVE type Id
    //        var waveTypeId = new byte[] { 0x57, 0x41, 0x56, 0x45 };
    //        memoryStream.Write(waveTypeId);

    //        #endregion

    //        #region Write format Chunk

    //        // Write "fmt " type Id
    //        var formatTypeId = new byte[] { 0x66, 0x6d, 0x74, 0x20 };
    //        memoryStream.Write(formatTypeId);

    //        // Write the format bytes to the stream
    //        var formatBytes = WaveFormatHelper.CreateFormatEx(EndianBitConverter.Little, WaveFormatTag.Pcm, 1, SampleRate.Khz32, Depth.Bit16, new byte[] { } );
          
    //          // Write byte size of the audio format
    //        var formatByteSize = EndianHelpers.ToLittleEndianBytes((uint)formatBytes.Length);
    //        memoryStream.Write(formatByteSize);

    //        // Write the format to the stream
    //        memoryStream.Write(formatBytes);

    //        #endregion

    //        #region Write data Chunk

    //        // Write "data" type Id
    //        var dataTypeId = new byte[] { 0x64, 0x61, 0x74, 0x61 };
    //        memoryStream.Write(dataTypeId);

    //        // Write byte size of the audio data
    //        var dataByteSize = EndianHelpers.ToLittleEndianBytes(0);
    //        memoryStream.Write(dataByteSize);

    //        #endregion

    //        memoryStream.Position = 0;

    //        // -> ACT
    //        var fixture = WaveFileFormat.ReadFromStream(memoryStream, Endianness.Little);

    //        // -> ASSERT
    //        Assert.AreEqual(52,                memoryStream.Position);
    //        Assert.AreEqual(0,                 fixture.AudioContentSize);
    //        Assert.AreEqual(WaveFormatTag.Pcm, fixture.Format.FormatTag);
    //        Assert.AreEqual(1,                 fixture.Format.Channels);
    //        Assert.AreEqual(SampleRate.Khz32,  fixture.Format.SamplesPerSec);
    //        Assert.AreEqual(Depth.Bit16,       fixture.Format.BitsPerSample);
    //    }
    //}
}
