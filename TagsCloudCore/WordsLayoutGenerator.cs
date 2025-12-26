using System.Drawing;
using TagsCloudCore.Layout.Abstractions;
using TagsCloudCore.Text.Abstractions;

namespace TagsCloudCore;

public sealed class WordsLayoutGenerator(
    IWordsProvider provider,
    IWordsPreprocessor preprocessor,
    IWordsFrequencyAnalyzer analyzer,
    IWordsFontSizer sizer,
    ICloudLayouter layouter)
    : IWordsLayoutGenerator
{
    public IEnumerable<(string word, Rectangle rect, int fontSize)> GenerateLayout()
    {
        var words = provider.GetWords();
        var processed = preprocessor.ProcessWords(words);
        var frequencies = analyzer.FindFrequencies(processed).OrderByDescending(f => f.Value).ToList();
        var maxFrequency = frequencies[0].Value;


        foreach (var (word, frequency) in frequencies)
        {
            var fontSize = sizer.GetFontSize(word, frequency, maxFrequency);
            var size = EstimateSize(word, fontSize);
            var rect = layouter.PutNextRectangle(size);

            yield return (word, rect, fontSize);
        }
    }

    private static Size EstimateSize(string word, int fontSize)
    {
        using var bitmap = new Bitmap(1, 1);
        using var graphics = Graphics.FromImage(bitmap);
        using var font = new Font("Arial", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);

        var sizeF = graphics.MeasureString(word, font);
        
        return new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
    }
}