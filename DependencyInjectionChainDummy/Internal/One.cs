namespace DependencyInjectionChainDummy.Internal;

// ReSharper disable once ClassNeverInstantiated.Global
public class One : ChainHelperFor<string, string>, INumber
{
    public override bool AmIResponsible => Input.Equals("One");

    protected override string InnerValueFor(string input)
    {
        return "1";
    }
}