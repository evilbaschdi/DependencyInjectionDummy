using EvilBaschdi.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionChainDummy;

/// <inheritdoc />
public interface IConfigureChain : IRunFor<IServiceCollection>
{
}