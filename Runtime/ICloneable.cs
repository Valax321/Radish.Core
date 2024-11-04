namespace Radish
{
    public interface ICloneable<out T>
    {
        T Clone();
    }
}