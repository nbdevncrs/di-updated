using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagsCloudCore.Text.Abstractions;

namespace TagsCloudCore.Text.Implementations.WordsProviding;

public sealed class DocxWordsProvider(string filePath) : IWordsProvider
{
    public IEnumerable<string> GetWords()
    {
        using var document = WordprocessingDocument.Open(filePath, false);

        var body = document.MainDocumentPart!.Document.Body;

        if (body == null) yield break;
        
        foreach (var paragraph in body.Elements<Paragraph>())
        {
            var text = paragraph.InnerText;

            if (string.IsNullOrWhiteSpace(text))
                continue;

            foreach (var word in WordsProvidingHelpers.SplitWords(text))
                yield return word;
        }
    }
}