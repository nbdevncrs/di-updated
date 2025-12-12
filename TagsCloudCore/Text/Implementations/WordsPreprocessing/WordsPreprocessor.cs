using TagsCloudCore.Text.Abstractions;

namespace TagsCloudCore.Text.Implementations.WordsPreprocessing;

public sealed class WordsPreprocessor : IWordsPreprocessor
{
    private readonly IReadOnlyList<IWordsFilter> filters;

    public WordsPreprocessor(IEnumerable<IWordsFilter> filters)
    {
        ArgumentNullException.ThrowIfNull(filters);
        this.filters = new List<IWordsFilter>(filters);
    }

    public IEnumerable<string> ProcessWords(IEnumerable<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);

        foreach (var word in words)
        {
            if (word is null) continue;

            var current = word;

            foreach (var filter in filters)
            {
                current = filter.Filter(current);
                if (current is null) break;
            }

            if (current is not null)
                yield return current;
        }
    }
}