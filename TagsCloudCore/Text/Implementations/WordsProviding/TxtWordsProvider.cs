using TagsCloudCore.Text.Abstractions;

namespace TagsCloudCore.Text.Implementations.WordsProviding;

public sealed class TxtWordsProvider(string filePath) : IWordsProvider
{
    public IEnumerable<string> GetWords()
    {
        using var reader = new StreamReader(filePath);

        while (reader.ReadLine() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            foreach (var word in WordsProvidingHelpers.SplitWords(line))
                yield return word;
        }
    }
}