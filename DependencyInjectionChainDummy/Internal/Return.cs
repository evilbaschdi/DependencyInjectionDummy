namespace DependencyInjectionChainDummy.Internal;

public class Return : IReturn
{
    private readonly INumber _chainHelperForString;
    private readonly IDemoInterface _demoInterface;

    public Return(IDemoInterface demoInterface, INumber number)
    {
        _demoInterface = demoInterface ?? throw new ArgumentNullException(nameof(demoInterface));
        _chainHelperForString = number ?? throw new ArgumentNullException(nameof(number));
    }

    public string Value => $"{_demoInterface.Value}: {_chainHelperForString.ValueFor("One")}";
}