using System.Drawing;
using NUnit.Framework;
using y1000.code;
using y1000.code.util;

namespace Tests.code.util;

[TestFixture]
[TestOf(typeof(PointUtil))]
public class PointUtilTest
{

    [Test]
    public void Add()
    {
        Point current = new Point(0, 0);
        var next = current.Next(Direction.DOWN);
        Assert.Equals(next.Y, 1);
    }
}