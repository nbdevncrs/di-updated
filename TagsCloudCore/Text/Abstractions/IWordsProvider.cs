namespace TagsCloudCore.Text.Abstractions;

public interface IWordsProvider
{
    IEnumerable<string> GetWords();
}
