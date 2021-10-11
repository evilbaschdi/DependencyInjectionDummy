namespace DependencyInjectionChainDummy.Internal
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Two : ChainHelperFor<string, string>, INumber
    {
        public Two(INumber number)
            : base(number)
        {
        }

        public override bool AmIResponsible => Input.Equals("Two");

        protected override string InnerValueFor(string input)
        {
            return "2";
        }
    }
}