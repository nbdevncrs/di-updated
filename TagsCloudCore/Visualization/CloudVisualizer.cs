using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace TagsCloudCore.Visualization;

public static class CloudVisualizer
{
    public static void SaveLayoutToFile(
        string filePath,
        IEnumerable<(string word, Rectangle rect, int fontSize)> layout,
        string fontFamily,
        Color? backgroundColor = null,
        Color? textColor = null,
        Size? imageSize = null,
        int padding = 50)
    {
        var elements = layout.ToArray();

        if (elements.Length == 0)
        {
            SaveEmptyImage(filePath, backgroundColor);
            return;
        }

        var bounds = CalculateBounds(elements, padding);

        var bitmapSize = imageSize ?? new Size(bounds.width, bounds.height);
        using var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);

        using var graphics = CreateGraphics(bitmap, backgroundColor);

        var offsetX = imageSize.HasValue ? (bitmap.Width - bounds.width) / 2 : 0;
        var offsetY = imageSize.HasValue ? (bitmap.Height - bounds.height) / 2 : 0;
        
        WordsDrawer.DrawWords(graphics, elements, bounds.minX, bounds.minY, padding, offsetX, offsetY, textColor, fontFamily);

        bitmap.Save(filePath, ImageFormat.Png);
    }

    private static void SaveEmptyImage(string filePath, Color? backgroundColor)
    {
        using var bitmap = new Bitmap(300, 300);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(backgroundColor ?? Color.Black);
        bitmap.Save(filePath, ImageFormat.Png);
    }

    private static (int minX, int minY, int width, int height) CalculateBounds(
        IEnumerable<(string word, Rectangle rect, int fontSize)> elements,
        int padding)
    {
        var minX = elements.Min(e => e.rect.Left);
        var maxX = elements.Max(e => e.rect.Right);
        var minY = elements.Min(e => e.rect.Top);
        var maxY = elements.Max(e => e.rect.Bottom);

        var width = Math.Min((maxX - minX) + padding * 2, 5000);
        var height = Math.Min((maxY - minY) + padding * 2, 5000);

        return (minX, minY, width, height);
    }

    private static Graphics CreateGraphics(Bitmap bitmap, Color? backgroundColor)
    {
        var graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        graphics.Clear(backgroundColor ?? Color.Black);
        return graphics;
    }
}