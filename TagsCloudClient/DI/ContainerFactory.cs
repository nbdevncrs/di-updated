using Autofac;

namespace TagsCloudClient.DI;

public static class ContainerFactory
{
    public static IContainer Build(Module module)
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule(module);
        return builder.Build();
    }
}