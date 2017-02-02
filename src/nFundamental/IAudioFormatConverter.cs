namespace Fundamental
{
    public interface IAudioFormatConverter<out T>
    {
        T Convert(IAudioFormat audioFormat);
    }
}
