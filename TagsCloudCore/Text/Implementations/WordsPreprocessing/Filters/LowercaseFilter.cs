using TagsCloudCore.Text.Abstractions;

namespace TagsCloudCore.Text.Implementations.WordsPreprocessing.Filters;

public sealed class LowercaseFilter : IWordsFilter
{
    public string Filter(string word)
    {
        ArgumentNullException.ThrowIfNull(word);
        return word.Length == 0 ? word : word.ToLowerInvariant();
    }
}