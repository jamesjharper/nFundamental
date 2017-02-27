using System;

namespace Fundamental.Wave.Format
{
    public static class AudioMediaSubType
    {

        public static readonly Guid Analog    = new Guid("6dba3190-67bd-11cf-a0f7-0020afd156e4");
        public static readonly Guid Pcm       = new Guid("00000001-0000-0010-8000-00aa00389b71");
        public static readonly Guid IeeeFloat = new Guid("00000003-0000-0010-8000-00aa00389b71");
        public static readonly Guid Drm       = new Guid("00000009-0000-0010-8000-00aa00389b71");
        public static readonly Guid ALaw      = new Guid("00000006-0000-0010-8000-00aa00389b71");
        public static readonly Guid MuLaw     = new Guid("00000007-0000-0010-8000-00aa00389b71");
        public static readonly Guid Adpcm     = new Guid("00000002-0000-0010-8000-00aa00389b71");
        public static readonly Guid Mpeg      = new Guid("00000050-0000-0010-8000-00aa00389b71");

    }
}
