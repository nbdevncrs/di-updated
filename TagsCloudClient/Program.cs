using Autofac;
using TagsCloudCore;
using TagsCloudCore.Visualization;

namespace TagsCloudClient;

internal static class Program
{
    public static void Main()
    {
        GenerateTagsCloud(
            sourceFileName: "input_small.txt",
            outputFileName: "cloud_small.png");

        GenerateTagsCloud(
            sourceFileName: "input_medium.txt",
            outputFileName: "cloud_medium.png");

        GenerateTagsCloud(
            sourceFileName: "input_big.txt",
            outputFileName: "cloud_big.png");
    }

    private static void GenerateTagsCloud(
        string sourceFileName,
        string outputFileName)
    {
        var projectRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        var inputPath = Path.Combine(
            projectRoot,
            "Inputs",
            sourceFileName);

        var outputPath = Path.Combine(
            projectRoot,
            "Outputs",
            outputFileName);

        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

        var builder = new ContainerBuilder();

        builder.RegisterModule(new TagsCloudModule(
            filePath: inputPath,
            minFontSize: 10,
            maxFontSize: 100,
            centerX: 0,
            centerY: 0));

        using var container = builder.Build();
        using var scope = container.BeginLifetimeScope();

        var generator = scope.Resolve<IWordsLayoutGenerator>();

        var layout = generator.GenerateLayout().ToArray();

        CloudVisualizer.SaveLayoutToFile(outputPath, layout);
    }
}