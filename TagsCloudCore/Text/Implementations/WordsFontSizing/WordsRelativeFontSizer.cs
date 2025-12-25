using TagsCloudCore.Text.Abstractions;

namespace TagsCloudCore.Text.Implementations.WordsFontSizing;

public sealed class WordsRelativeFontSizer : IWordsFontSizer
{
    private readonly int minFontSize;
    private readonly int maxFontSize;

    public WordsRelativeFontSizer(int minFontSize = 30, int maxFontSize = 200)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minFontSize);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxFontSize, minFontSize);

        this.minFontSize = minFontSize;
        this.maxFontSize = maxFontSize;
    }

    public int GetFontSize(string word, int frequency, int maxFrequency)
    {
        ArgumentNullException.ThrowIfNull(word);

        if (frequency <= 0 || maxFrequency <= 0)
            return minFontSize;

        var ratio = (float)frequency / maxFrequency;

        var size = minFontSize + ratio * (maxFontSize - minFontSize) * 5;

        return (int)Math.Round(size);
    }
}