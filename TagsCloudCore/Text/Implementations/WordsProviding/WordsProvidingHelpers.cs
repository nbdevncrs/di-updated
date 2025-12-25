namespace TagsCloudCore.Text.Implementations.WordsProviding;

internal static class WordsProvidingHelpers
{
    private static readonly char[] Separators =
    [
        ' ', '\t', '.', ',', '!', '?', ';', ':', '[', ']', '(', ')', '"'
    ];

    public static IEnumerable<string> SplitWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            yield break;

        foreach (var token in text.Split(Separators, StringSplitOptions.RemoveEmptyEntries))
            yield return token;
    }
}
