namespace Fundamental.Core.AudioFormats
{
    public static class FormatKeys
    {
        
        public static string Endianness = nameof(Endianness); // Endianness enum 


        public static string Encoding = nameof(Encoding);

        // PCM encoding
        public static class Pcm
        {
            public static string Format        = $"{nameof(Pcm)}";

            public static string SampleRate    = $"{nameof(Pcm)}.{nameof(SampleRate)}";  // int in Hertz
            public static string Depth         = $"{nameof(Pcm)}.{nameof(Depth)}";       // int (Bits per sample)
            public static string Channels      = $"{nameof(Pcm)}.{nameof(Channels)}";    // int (number of channels)
            public static string Speakers      = $"{nameof(Pcm)}.{nameof(Speakers)}";    // Speakers enum
            public static string DataType      = $"{nameof(Pcm)}.{nameof(DataType)}";    // PcmDataType enum
        }
    }
}
