namespace BH_Engine
{
    public interface IDynamicValueHandler<T>
    {
        T Calculate(T x);
    }

    public interface IDynamicFloatHandler : IDynamicValueHandler<float>
    {
    }

    public interface IDynamicIntHandler : IDynamicValueHandler<int>
    {
    }
}