using System;
using EvilBaschdi.Core;

namespace WpfDependencyInjectionDummy.Internal
{
    public interface IDummyInterface : IValue<string>
    {
    }

    public class DummyClass : IDummyInterface
    {
        public string Value => DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
    }
}