namespace TagsCloudCore.Text.Abstractions;

public interface IWordsFrequencyAnalyzer
{
    IReadOnlyDictionary<string, int> FindFrequencies(IEnumerable<string> words);
}
