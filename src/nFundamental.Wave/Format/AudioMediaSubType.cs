using System;

namespace Fundamental.Wave.Format
{
    public static class AudioMediaSubType
    {

        public static Guid Analog    = new Guid("6dba3190-67bd-11cf-a0f7-0020afd156e4");
        public static Guid Pcm       = new Guid("00000001-0000-0010-8000-00aa00389b71");
        public static Guid IeeeFloat = new Guid("00000003-0000-0010-8000-00aa00389b71");
        public static Guid Drm       = new Guid("00000009-0000-0010-8000-00aa00389b71");
        public static Guid ALaw      = new Guid("00000006-0000-0010-8000-00aa00389b71");
        public static Guid MuLaw     = new Guid("00000007-0000-0010-8000-00aa00389b71");
        public static Guid Adpcm     = new Guid("00000002-0000-0010-8000-00aa00389b71");
        public static Guid Mpeg      = new Guid("00000050-0000-0010-8000-00aa00389b71");

    }
}
