namespace Fundamental.Core
{
    public interface IAudioFormatConverter<T>
    {
        T Convert(IAudioFormat audioFormat);

        IAudioFormat Convert(T audioFormat);
    }
}
