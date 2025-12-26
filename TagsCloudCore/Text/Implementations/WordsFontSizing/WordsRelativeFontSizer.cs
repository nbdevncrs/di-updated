using TagsCloudCore.Text.Abstractions;

namespace TagsCloudCore.Text.Implementations.WordsFontSizing;

public sealed class WordsRelativeFontSizer : IWordsFontSizer
{
    private readonly int minFontSize;

    public WordsRelativeFontSizer(int minFontSize = 30)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minFontSize);

        this.minFontSize = minFontSize;
    }

    public int GetFontSize(string word, int frequency, int maxFrequency)
    {
        ArgumentNullException.ThrowIfNull(word);

        if (frequency <= 0 || maxFrequency <= 0)
            return minFontSize;

        var ratio = (float)frequency / maxFrequency;

        var size = minFontSize + ratio * minFontSize * 5;

        return (int)Math.Round(size);
    }
}