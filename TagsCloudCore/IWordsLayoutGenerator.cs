using System.Drawing;

namespace TagsCloudCore;

public interface IWordsLayoutGenerator
{
    IEnumerable<(string word, Rectangle rect, int fontSize)> GenerateLayout();
}