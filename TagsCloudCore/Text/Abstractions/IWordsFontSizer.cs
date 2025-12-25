using System.Drawing;

namespace TagsCloudCore.Text.Abstractions;

public interface IWordsFontSizer
{
    int GetFontSize(string word, int frequency, int maxFrequency);
}