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

            foreach (var word in SplitWords(line))
                yield return word;
        }
    }

    private static IEnumerable<string> SplitWords(string text)
    {
        var separators = new[] { ' ', '\t', '.', ',', '!', '?', ';', ':', '[', ']', '(', ')', '"' };

        foreach (var token in text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
            yield return token;
    }
}