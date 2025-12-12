using System.Drawing;

namespace TagsCloudCore.Layout.Abstractions;

public interface IRectangleTightener
{
    Rectangle Tighten(
        Rectangle rectangle,
        Point cloudCenter,
        IReadOnlyCollection<Rectangle> existingRectangles);
}