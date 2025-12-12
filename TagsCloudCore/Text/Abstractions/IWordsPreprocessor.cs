namespace TagsCloudCore.Text.Abstractions;

public interface IWordsPreprocessor
{
    IEnumerable<string> ProcessWords(IEnumerable<string> words);
}