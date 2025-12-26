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

namespace TagsCloudClient.DI;

public class TagsCloudModule(string filePath, int minFontSize, IEnumerable<string>? stopWords = null) : Module
{
    private static readonly Point Center = new(0, 0);

    private static readonly string[] DefaultStopWords =
    [
        "и", "в", "во", "на", "с", "со",
        "а", "но", "что", "как", "к", "до",
        "по", "из", "у", "или", "же", "он", 
        "она", "они", "я", "вы", "ты", "чем", 
        "от", "ее", "её", "за", "все", "всё",
        "был", "его", "еще", "ещё", "было",
        "была", "лишь", "не", "под", "над", "нем",
        "нём", "ней"
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
            .WithParameter("stopWords", (stopWords ?? DefaultStopWords).ToArray());

        builder.RegisterType<WordsPreprocessor>()
            .As<IWordsPreprocessor>()
            .SingleInstance();

        builder.RegisterType<WordsFrequencyAnalyzer>()
            .As<IWordsFrequencyAnalyzer>()
            .SingleInstance();

        builder.RegisterType<WordsRelativeFontSizer>()
            .As<IWordsFontSizer>()
            .SingleInstance()
            .WithParameter("minFontSize", minFontSize);

        builder.RegisterType<ToCenterTightener>()
            .As<IRectangleTightener>()
            .SingleInstance();

        builder.RegisterType<CircularCloudLayouter>()
            .As<ICloudLayouter>()
            .SingleInstance()
            .WithParameter("center", Center);

        builder.RegisterType<WordsLayoutGenerator>()
            .As<IWordsLayoutGenerator>()
            .SingleInstance();
    }
}