
namespace Fundamental.Interface
{
    public interface ISupportsInterface<out T> // Note: don't break convariants! It is needed for down casting supported interfaces 
    {
        T GetAudioInterface();
    }
}
