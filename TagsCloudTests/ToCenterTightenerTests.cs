using System.Drawing;
using FluentAssertions;
using TagsCloudCore.Layout;
using TagsCloudCore.Layout.Abstractions;
using TagsCloudCore.Layout.Extensions;
using TagsCloudCore.Layout.Implementations;

namespace TagsCloudTests;

public class ToCenterTightenerTests
{
    private ToCenterTightener tightener = null!;
    private Point center;

    [SetUp]
    public void SetUp()
    {
        tightener = new ToCenterTightener();
        center = new Point(0, 0);
    }

    [Test]
    public void ToCenterTightener_ShouldMoveRectangleCloserToCenter_WhenNoObstacles_Test()
    {
        var rectangle = new Rectangle(50, 50, 20, 20);

        var result = tightener.Tighten(rectangle, center, []);

        result.GetDistanceToPoint(center)
            .Should()
            .BeLessThan(rectangle.GetDistanceToPoint(center));
    }

    [Test]
    public void ToCenterTightener_ShouldStopBeforeCollision_WhenObstacleIsPresent_Test()
    {
        var rectangle = new Rectangle(40, 0, 20, 20);
        var obstacle = new Rectangle(10, 0, 20, 20);

        var result = tightener.Tighten(rectangle, center, [obstacle]);

        result.IntersectsAny([obstacle]).Should().BeFalse();
        result.GetDistanceToPoint(center).Should().BeGreaterThan(0);
    }
}