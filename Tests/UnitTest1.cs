using Godot;
using Moq;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var character  = new Mock<Character>();
        character.Setup(c => c.AnimationPlayer).Returns(new AnimationPlayer());
        Assert.AreEqual(3, Character.Add(1, 2));
    }
}