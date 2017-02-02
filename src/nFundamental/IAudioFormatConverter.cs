namespace Fundamental
{
    public interface IAudioFormatConverter<T>
    {
        T Convert(IAudioFormat audioFormat);

        IAudioFormat Convert(T audioFormat);
    }
}
