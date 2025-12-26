using FluentAssertions;
using TagsCloudCore.Text.Implementations.WordsFontSizing;

namespace TagsCloudTests;

[TestFixture]
public class WordsRelativeFontSizerTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenMinFontSizeIsZeroOrNegative()
    {
        Action actZero = () => new WordsRelativeFontSizer(0);
        Action actNegative = () => new WordsRelativeFontSizer(-10);

        actZero.Should().Throw<ArgumentOutOfRangeException>();
        actNegative.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void GetFontSize_ShouldThrow_WhenWordIsNull()
    {
        var sizer = new WordsRelativeFontSizer(30);
        Action act = () => sizer.GetFontSize(null!, 1, 10);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetFontSize_ShouldReturnMinFontSize_WhenFrequencyIsZeroOrNegative()
    {
        var sizer = new WordsRelativeFontSizer(30);
        var resultZero = sizer.GetFontSize("слово", 0, 10);
        var resultNegative = sizer.GetFontSize("слово", -5, 10);

        resultZero.Should().Be(30);
        resultNegative.Should().Be(30);
    }

    [Test]
    public void GetFontSize_ShouldReturnMinFontSize_WhenMaxFrequencyIsZeroOrNegative()
    {
        var sizer = new WordsRelativeFontSizer(30);
        var resultZero = sizer.GetFontSize("слово", 5, 0);
        var resultNegative = sizer.GetFontSize("слово", 5, -3);

        resultZero.Should().Be(30);
        resultNegative.Should().Be(30);
    }

    [Test]
    public void GetFontSize_ShouldReturnMinFontSize_WhenFrequencyEqualsMaxFrequencyZeroRatio()
    {
        var sizer = new WordsRelativeFontSizer(30);
        var result = sizer.GetFontSize("слово", 1, 1);

        result.Should().Be(30 + 30 * 5);
    }

    [Test]
    public void GetFontSize_ShouldScaleLinearly_WithFrequencyRatio()
    {
        var sizer = new WordsRelativeFontSizer(30);

        var sizeLow = sizer.GetFontSize("слово", 1, 10);
        var sizeMid = sizer.GetFontSize("слово", 5, 10);
        var sizeHigh = sizer.GetFontSize("слово", 10, 10);

        sizeLow.Should().BeLessThan(sizeMid);
        sizeMid.Should().BeLessThan(sizeHigh);
    }

    [Test]
    public void GetFontSize_ShouldCalculateExpectedValue()
    {
        var sizer = new WordsRelativeFontSizer(20);
        var result = sizer.GetFontSize("данные", 5, 10);

        result.Should().Be(70);
    }

    [Test]
    public void GetFontSize_ShouldRoundResult()
    {
        var sizer = new WordsRelativeFontSizer(25);
        var result = sizer.GetFontSize("слово", 1, 3);

        result.Should().Be(67);
    }
}