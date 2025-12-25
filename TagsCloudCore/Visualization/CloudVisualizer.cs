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
        int padding = 50)
    {
        var elements = layout.ToArray();

        if (elements.Length == 0)
        {
            using var emptyBitmap = new Bitmap(300, 300);
            using var g = Graphics.FromImage(emptyBitmap);
            g.Clear(Color.Black);
            emptyBitmap.Save(filePath, ImageFormat.Png);
            return;
        }
        
        var minX = elements.Min(e => e.rect.Left);
        var maxX = elements.Max(e => e.rect.Right);
        var minY = elements.Min(e => e.rect.Top);
        var maxY = elements.Max(e => e.rect.Bottom);

        var width = (maxX - minX) + padding * 2;
        var height = (maxY - minY) + padding * 2;

        width = Math.Min(width, 5000);
        height = Math.Min(height, 5000);

        using var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        graphics.Clear(Color.Black);

        var random = new Random();

        using var rectPen = new Pen(Color.White, 1);

        foreach (var (word, rect, fontSize) in elements)
        {
            var shifted = rect with
            {
                X = rect.Left - minX + padding,
                Y = rect.Top - minY + padding
            };
            
            graphics.DrawRectangle(rectPen, shifted);

            using var font = new Font("Arial", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            using var brush = new SolidBrush(Color.FromArgb(
                255,
                random.Next(40, 255),
                random.Next(40, 255),
                random.Next(40, 255)));
            
            var textSize = graphics.MeasureString(word, font);
            
            var textX = shifted.Left + (shifted.Width - textSize.Width) / 2;
            var textY = shifted.Top + (shifted.Height - textSize.Height) / 2;

            graphics.DrawString(word, font, brush, new PointF(textX, textY));
        }

        bitmap.Save(filePath, ImageFormat.Png);
    }
}
