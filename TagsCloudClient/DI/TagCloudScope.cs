using Autofac;
using TagsCloudCore;

namespace TagsCloudClient.DI;

public sealed class TagsCloudScope : IDisposable
{
    private readonly ILifetimeScope scope;

    public TagsCloudScope(TagsCloudModule module)
    {
        var container = ContainerFactory.Build(module);
        scope = container.BeginLifetimeScope();
    }

    public IWordsLayoutGenerator GetGenerator()
    {
        return scope.Resolve<IWordsLayoutGenerator>();
    }

    public void Dispose()
    {
        scope.Dispose();
    }
}