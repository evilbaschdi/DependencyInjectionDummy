namespace DependencyInjectionChainDummy.Internal;

// ReSharper disable once ClassNeverInstantiated.Global
public class Two(INumber number) : ChainHelperFor<string, string>(number), INumber
{
    public override bool AmIResponsible => Input.Equals("Two");

    protected override string InnerValueFor(string input) => "2";
}