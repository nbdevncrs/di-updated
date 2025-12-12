using TagsCloudCore.Text.Abstractions;

namespace TagsCloudCore.Text.Implementations.WordsFontSizing;

public sealed class WordsRelativeFontSizer : IWordsFontSizer
{
    private readonly int minFontSize;
    private readonly int maxFontSize;

    public WordsRelativeFontSizer(int minFontSize = 10, int maxFontSize = 100)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minFontSize);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxFontSize, minFontSize);

        this.minFontSize = minFontSize;
        this.maxFontSize = maxFontSize;
    }

    public int GetFontSize(string word, int frequency)
    {
        ArgumentNullException.ThrowIfNull(word);

        if (frequency <= 0) return minFontSize;
        
        var scaled = minFontSize + frequency;

        return Math.Clamp(scaled, minFontSize, maxFontSize);
    }
}