using System.Drawing;
using TagsCloudClient.DI;
using TagsCloudCore.Visualization;

namespace TagsCloudClient;

internal static class Program
{
    public static void Main()
    {
        Generate("input_firstsize.txt", "cloud_firstsize.png", 20,
            textColor: Color.BlueViolet,
            backgroundColor: Color.White, 
            fontFamily: "Segoe UI");

        Generate("input_secondsize.txt", "cloud_secondsize.png", 20,
            textColor: Color.Red,
            backgroundColor: Color.Black,
            imageSize: new Size(1920, 1080)
            );

        Generate("input_thirdsize.txt", "cloud_thirdsize.png", 20);
        Generate("input_fourthsize.txt", "cloud_fourthsize.png", 20);
        Generate("input_story.docx", "cloud_story.png", 20);
    }

    private static void Generate(
        string inputFileName,
        string outputFileName,
        int minFontSize,
        IEnumerable<string>? stopWords = null,
        Color? textColor = null,
        Color? backgroundColor = null,
        string fontFamily = "Arial",
        Size? imageSize = null)
    {
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        var inputPath = Path.Combine(projectRoot, "Inputs", inputFileName);
        var outputPath = Path.Combine(projectRoot, "Outputs", outputFileName);

        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

        var module = new TagsCloudModule(
            filePath: inputPath,
            minFontSize: minFontSize,
            stopWords: stopWords);

        using var scope = new TagsCloudScope(module);

        var generator = scope.GetGenerator();
        var layout = generator.GenerateLayout().ToArray();

        CloudVisualizer.SaveLayoutToFile(outputPath, layout, fontFamily, backgroundColor, textColor, imageSize);
    }
}