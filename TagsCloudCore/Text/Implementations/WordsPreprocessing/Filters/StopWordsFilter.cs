using TagsCloudCore.Text.Abstractions;

namespace TagsCloudCore.Text.Implementations.WordsPreprocessing.Filters;

public sealed class StopWordsFilter : IWordsFilter
{
    private readonly HashSet<string> stopWords;

    public StopWordsFilter(IEnumerable<string> stopWords)
    {
        ArgumentNullException.ThrowIfNull(stopWords);
        this.stopWords = new HashSet<string>(stopWords, StringComparer.OrdinalIgnoreCase);
    }

    public string? Filter(string word)
    {
        ArgumentNullException.ThrowIfNull(word);
        if (word.Length == 0) return word;

        return stopWords.Contains(word) ? null : word;
    }
}