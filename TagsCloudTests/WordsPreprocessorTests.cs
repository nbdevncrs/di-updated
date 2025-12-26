using FluentAssertions;
using TagsCloudCore.Text.Abstractions;
using TagsCloudCore.Text.Implementations.WordsPreprocessing;
using TagsCloudCore.Text.Implementations.WordsPreprocessing.Filters;

namespace TagsCloudTests;

[TestFixture]
public class WordsPreprocessorTests
{
    [Test]
    public void LowercaseFilter_ShouldConvertWordToLowercase()
    {
        var filter = new LowercaseFilter();

        var result = filter.Filter("АлГоРиТм");

        result.Should().Be("алгоритм");
    }

    [Test]
    public void LowercaseFilter_ShouldReturnEmptyString_ForEmptyInput()
    {
        var filter = new LowercaseFilter();

        var result = filter.Filter(string.Empty);

        result.Should().BeEmpty();
    }

    [Test]
    public void StopWordsFilter_ShouldRemoveStopWord()
    {
        var filter = new StopWordsFilter(["и", "в", "на"]);

        var result = filter.Filter("и");

        result.Should().BeNull();
    }

    [Test]
    public void StopWordsFilter_ShouldIgnoreCase()
    {
        var filter = new StopWordsFilter(["данные"]);

        var result = filter.Filter("ДАННЫЕ");

        result.Should().BeNull();
    }

    [Test]
    public void StopWordsFilter_ShouldKeepNonStopWord()
    {
        var filter = new StopWordsFilter(["и", "в", "на"]);

        var result = filter.Filter("алгоритм");

        result.Should().Be("алгоритм");
    }

    [Test]
    public void WordsPreprocessor_ShouldApplyAllFiltersInOrder()
    {
        IWordsFilter[] filters =
        {
            new LowercaseFilter(),
            new StopWordsFilter(["и", "в", "на"])
        };

        var preprocessor = new WordsPreprocessor(filters);

        var words = new[] { "ДАННЫЕ", "И", "Алгоритм" };

        var result = preprocessor.ProcessWords(words).ToArray();

        result.Should().Equal("данные", "алгоритм");
    }

    [Test]
    public void WordsPreprocessor_ShouldSkipNullWords()
    {
        var preprocessor = new WordsPreprocessor(
            [new LowercaseFilter()]);

        var words = new[] { "Тест", null, "Слово" };

        var result = preprocessor.ProcessWords(words!).ToArray();

        result.Should().Equal("тест", "слово");
    }

    [Test]
    public void WordsPreprocessor_ShouldReturnEmpty_ForEmptyInput()
    {
        var preprocessor = new WordsPreprocessor(
            [new LowercaseFilter()]);

        var result = preprocessor.ProcessWords([]).ToArray();

        result.Should().BeEmpty();
    }

    [Test]
    public void WordsPreprocessor_ShouldRemoveWord_WhenFilterReturnsNull()
    {
        var preprocessor = new WordsPreprocessor(
        [
            new StopWordsFilter(["код"]),
            new LowercaseFilter()
        ]);

        var result = preprocessor.ProcessWords(["код"]).ToArray();

        result.Should().BeEmpty();
    }
}