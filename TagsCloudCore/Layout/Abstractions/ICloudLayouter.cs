using System.Drawing;

namespace TagsCloudCore.Layout.Abstractions;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    IEnumerable<Rectangle> PlacedRectangles { get; }
}