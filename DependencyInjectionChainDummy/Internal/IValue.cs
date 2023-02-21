namespace DependencyInjectionChainDummy.Internal;

public interface IValue<out T>
{
    T Value { get; }
}