namespace DependencyInjectionChainDummy.Internal
{
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