namespace WpfDependencyInjectionDummy.Internal;

public class DummyClass : IDummyInterface
{
    public string Value => DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
}