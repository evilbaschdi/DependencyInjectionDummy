namespace DependencyInjectionChainDummy.Internal;

/// <inheritdoc />
public abstract class ChainHelperFor<TIn, TOut> : IChainHelperFor<TIn, TOut>
{
    /// <summary>
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once NotAccessedField.Global
    protected TIn Input;

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
    /// </summary>
    protected ChainHelperFor(IChainHelperFor<TIn, TOut> number)
    {
        NextChain = number ?? throw new ArgumentNullException(nameof(number));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
    /// </summary>
    protected ChainHelperFor()
    {
    }

    /// <inheritdoc />
    public IChainHelperFor<TIn, TOut> NextChain { get; }

    /// <inheritdoc />
    public abstract bool AmIResponsible { get; }

    /// <inheritdoc />
    public TOut ValueFor(TIn input)
    {
        Input = input;
        return AmIResponsible ? InnerValueFor(input) : NextChain != null ? NextChain.ValueFor(input) : default;
    }

    /// <summary>
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    // ReSharper disable once UnusedParameter.Global
    protected abstract TOut InnerValueFor(TIn input);
}