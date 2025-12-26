using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentAssertions;
using TagsCloudCore.Text.Implementations.WordsProviding;

namespace TagsCloudTests;

[TestFixture]
public class WordsProvidersTests
{
    [Test]
    public void TxtWordsProvider_ShouldReadWords_FromFile()
    {
        var tempFile = Path.GetTempFileName();

        File.WriteAllText(tempFile,
            """
            данные алгоритм система
            код модель

            анализ
            """);

        var provider = new TxtWordsProvider(tempFile);

        var words = provider.GetWords().ToArray();

        words.Should().Equal(
            "данные",
            "алгоритм",
            "система",
            "код",
            "модель",
            "анализ");

        File.Delete(tempFile);
    }

    [Test]
    public void TxtWordsProvider_ShouldIgnoreEmptyLines()
    {
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "\n\nслово\n\n");

        var provider = new TxtWordsProvider(tempFile);

        var words = provider.GetWords().ToArray();

        words.Should().ContainSingle()
            .Which.Should().Be("слово");

        File.Delete(tempFile);
    }

    [Test]
    public void DocxWordsProvider_ShouldReadWords_FromParagraphs()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.docx");

        using (var document = WordprocessingDocument.Create(
                   tempFile,
                   WordprocessingDocumentType.Document))
        {
            var mainPart = document.AddMainDocumentPart();

            mainPart.Document = new Document(
                new Body(
                    new Paragraph(new Run(new Text("данные алгоритм система"))),
                    new Paragraph(new Run(new Text("код модель"))),
                    new Paragraph())
            );
        }

        var provider = new DocxWordsProvider(tempFile);

        var words = provider.GetWords().ToArray();

        words.Should().Equal(
            "данные",
            "алгоритм",
            "система",
            "код",
            "модель");

        File.Delete(tempFile);
    }

    [Test]
    public void DocxWordsProvider_ShouldReturnEmpty_ForEmptyDocument()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.docx");

        using (var document = WordprocessingDocument.Create(
                   tempFile,
                   WordprocessingDocumentType.Document))
        {
            document.AddMainDocumentPart()
                .Document = new Document(new Body());
        }

        var provider = new DocxWordsProvider(tempFile);

        var words = provider.GetWords().ToArray();

        words.Should().BeEmpty();

        File.Delete(tempFile);
    }
}