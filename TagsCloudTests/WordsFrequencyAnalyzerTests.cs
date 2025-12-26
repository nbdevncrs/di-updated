using FluentAssertions;
using TagsCloudCore.Text.Implementations.WordsFrequencyAnalyzing;

namespace TagsCloudTests;

[TestFixture]
public class WordsFrequencyAnalyzerTests
{
    [Test]
    public void FindFrequencies_ShouldCountSingleWord()
    {
        var analyzer = new WordsFrequencyAnalyzer();

        var result = analyzer.FindFrequencies(new[] { "данные" });

        result.Should().ContainSingle();
        result["данные"].Should().Be(1);
    }

    [Test]
    public void FindFrequencies_ShouldCountRepeatedWords()
    {
        var analyzer = new WordsFrequencyAnalyzer();

        var words = new[] { "код", "код", "код" };

        var result = analyzer.FindFrequencies(words);

        result.Should().ContainKey("код");
        result["код"].Should().Be(3);
    }

    [Test]
    public void FindFrequencies_ShouldBeCaseInsensitive()
    {
        var analyzer = new WordsFrequencyAnalyzer();

        var words = new[] { "Алгоритм", "алгоритм", "АЛГОРИТМ" };

        var result = analyzer.FindFrequencies(words);

        result.Should().ContainSingle();
        result["алгоритм"].Should().Be(3);
    }

    [Test]
    public void FindFrequencies_ShouldIgnoreEmptyAndWhitespaceWords()
    {
        var analyzer = new WordsFrequencyAnalyzer();

        var words = new[] { "данные", "", " ", "\t", "данные" };

        var result = analyzer.FindFrequencies(words);

        result.Should().ContainSingle();
        result["данные"].Should().Be(2);
    }

    [Test]
    public void FindFrequencies_ShouldCountDifferentWordsSeparately()
    {
        var analyzer = new WordsFrequencyAnalyzer();

        var words = new[] { "код", "данные", "код", "модель" };

        var result = analyzer.FindFrequencies(words);

        result.Should().HaveCount(3);
        result["код"].Should().Be(2);
        result["данные"].Should().Be(1);
        result["модель"].Should().Be(1);
    }

    [Test]
    public void FindFrequencies_ShouldReturnEmptyDictionary_ForEmptyInput()
    {
        var analyzer = new WordsFrequencyAnalyzer();

        var result = analyzer.FindFrequencies([]);

        result.Should().BeEmpty();
    }

    [Test]
    public void FindFrequencies_ShouldThrow_WhenWordsIsNull()
    {
        var analyzer = new WordsFrequencyAnalyzer();

        Action act = () => analyzer.FindFrequencies(null!);

        act.Should().Throw<ArgumentNullException>();
    }
}
