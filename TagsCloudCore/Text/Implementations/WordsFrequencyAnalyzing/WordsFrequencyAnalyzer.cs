using TagsCloudCore.Text.Abstractions;

namespace TagsCloudCore.Text.Implementations.WordsFrequencyAnalyzing;

public sealed class WordsFrequencyAnalyzer : IWordsFrequencyAnalyzer
{
    public IReadOnlyDictionary<string, int> FindFrequencies(IEnumerable<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);

        var frequencies = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var word in words)
        {
            if (string.IsNullOrWhiteSpace(word)) continue;

            if (frequencies.TryGetValue(word, out var value))
                frequencies[word] = value + 1;
            
            else frequencies[word] = 1;
        }

        return frequencies;
    }
}