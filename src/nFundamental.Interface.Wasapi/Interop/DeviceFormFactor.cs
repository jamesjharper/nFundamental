namespace Fundamental.Interface.Wasapi.Interop
{
    public enum DeviceFormFactor : uint
    {
        RemoteNetworkDevice = 0,
        Speakers = 1,
        LineLevel = 2,
        Headphones = 3,
        Microphone = 4,
        Headset = 5,
        Handset = 6,
        UnknownDigitalPassthrough = 7,
        Spdif = 8,
        DigitalAudioDisplayDevice = 9,
        UnknownFormFactor = 10,
    }
}
