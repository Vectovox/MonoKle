using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MonoKle.Tests
{
    [TestClass]
    public class DynamicCamera2DTests
    {
        private const float Accuary = 0.0001f;

        [TestMethod]
        public void MoveTo_MovesCorrectlyOverTime()
        {
            var sut = new DynamicCamera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 3,
            };
            sut.MoveTo(MVector2.One, TimeSpan.FromSeconds(1));
            Assert.AreEqual(MVector2.Zero, sut.Position);
            sut.Update(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(0.5f, sut.Position.X, Accuary);
            Assert.AreEqual(0.5f, sut.Position.Y, Accuary);
            sut.Update(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(MVector2.One.X, sut.Position.X, Accuary);
            Assert.AreEqual(MVector2.One.Y, sut.Position.Y, Accuary);
        }

        [TestMethod]
        public void ScaleTo_MovesCorrectlyOverTime()
        {
            var sut = new DynamicCamera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 3,
            };
            sut.ScaleTo(4f, TimeSpan.FromSeconds(1));
            Assert.AreEqual(3, sut.Scale);
            sut.Update(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(3.5f, sut.Scale, Accuary);
            sut.Update(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(4f, sut.Scale, Accuary);
            // Missing assertion for coordinate
        }

        [TestMethod]
        public void ScaleAroundTo_MovesCorrectlyOverTime()
        {
            var sut = new DynamicCamera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 3,
            };
            sut.ScaleAroundTo(MVector2.One, 4f, TimeSpan.FromSeconds(1));
            Assert.AreEqual(3, sut.Scale);
            sut.Update(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(3.5f, sut.Scale, Accuary);
            sut.Update(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(4f, sut.Scale, Accuary);
            // Missing assertion for coordinate
        }

        [TestMethod]
        public void ScaleAroundToRelative_Positive_MovesCorrectlyOverTime()
        {
            var sut = new DynamicCamera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 3,
            };
            sut.ScaleAroundToRelative(MVector2.One, 1f, TimeSpan.FromSeconds(1));
            Assert.AreEqual(3, sut.Scale);
            sut.Update(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(3.5f, sut.Scale, Accuary);
            sut.Update(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(4f, sut.Scale, Accuary);
            // Missing assertion for coordinate
        }

        [TestMethod]
        public void ScaleAroundToRelative_Negative_MovesCorrectlyOverTime()
        {
            var sut = new DynamicCamera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 3,
            };
            sut.ScaleAroundToRelative(MVector2.One, -1f, TimeSpan.FromSeconds(1));
            Assert.AreEqual(3, sut.Scale);
            sut.Update(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(2.5f, sut.Scale, Accuary);
            sut.Update(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(2f, sut.Scale, Accuary);
            // Missing assertion for coordinate
        }
    }
}
