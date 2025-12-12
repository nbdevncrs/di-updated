using System.Drawing;
using Autofac;
using TagsCloudCore;
using TagsCloudCore.Layout.Abstractions;
using TagsCloudCore.Layout.Implementations;
using TagsCloudCore.Text.Abstractions;
using TagsCloudCore.Text.Implementations.WordsFontSizing;
using TagsCloudCore.Text.Implementations.WordsFrequencyAnalyzing;
using TagsCloudCore.Text.Implementations.WordsPreprocessing;
using TagsCloudCore.Text.Implementations.WordsPreprocessing.Filters;
using TagsCloudCore.Text.Implementations.WordsProviding;

namespace TagsCloudClient;

public class TagsCloudModule(
    string filePath,
    int minFontSize = 10,
    int maxFontSize = 100,
    int centerX = 0,
    int centerY = 0) : Module
{
    private readonly Point center = new(centerX, centerY);

    private static readonly string[] parameterValue =
    [
        "и", "в", "во", "на", "с", "со",
        "а", "но", "что", "как", "к", "до",
        "по", "из", "у", "или", "же"
    ];

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register<IWordsProvider>(context =>
            {
                var ext = Path.GetExtension(filePath)?.ToLowerInvariant();

                return ext switch
                {
                    ".txt" => new TxtWordsProvider(filePath),
                    ".docx" => new DocxWordsProvider(filePath),
                    _ => throw new NotSupportedException($"Unsupported file format: {ext}")
                };
            })
            .SingleInstance();

        builder.RegisterType<LowercaseFilter>()
            .As<IWordsFilter>()
            .SingleInstance();

        builder.RegisterType<StopWordsFilter>()
            .As<IWordsFilter>()
            .SingleInstance()
            .WithParameter("stopWords", parameterValue);

        builder.RegisterType<WordsPreprocessor>()
            .As<IWordsPreprocessor>()
            .SingleInstance();

        builder.RegisterType<WordsFrequencyAnalyzer>()
            .As<IWordsFrequencyAnalyzer>()
            .SingleInstance();

        builder.RegisterType<WordsRelativeFontSizer>()
            .As<IWordsFontSizer>()
            .SingleInstance()
            .WithParameter("minFontSize", minFontSize)
            .WithParameter("maxFontSize", maxFontSize);

        builder.RegisterType<ToCenterTightener>()
            .As<IRectangleTightener>()
            .SingleInstance();

        builder.RegisterType<CircularCloudLayouter>()
            .As<ICloudLayouter>()
            .SingleInstance()
            .WithParameter("center", center);

        builder.RegisterType<WordsLayoutGenerator>()
            .As<IWordsLayoutGenerator>()
            .SingleInstance();
    }
}