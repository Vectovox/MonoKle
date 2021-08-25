using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MonoKle.Tests
{
    [TestClass]
    public class Camera2DTests
    {
        [DataTestMethod]
        [DataRow(2f, 2f, DisplayName = "Lower edge case")]
        [DataRow(5f, 5f, DisplayName = "Upper edge case")]
        [DataRow(1f, 2f, DisplayName = "Lower clamped")]
        [DataRow(6f, 5f, DisplayName = "Upper clamped")]
        public void SetScale_CorrectlyAssigned(float toSet, float expected)
        {
            var sut = new Camera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 3,
            };
            sut.Scale = toSet;
            Assert.AreEqual(expected, sut.Scale);
        }

        [TestMethod]
        public void SetMinScale_ScaleNotChanged()
        {
            var sut = new Camera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 4,
            };
            sut.MinScale = 3;
            Assert.AreEqual(3, sut.MinScale);
            Assert.AreEqual(4, sut.Scale);
        }

        [TestMethod]
        public void SetMinScale_ClampsScale()
        {
            var sut = new Camera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 3,
            };
            sut.MinScale = 4;
            Assert.AreEqual(4, sut.MinScale);
            Assert.AreEqual(sut.MinScale, sut.Scale);
        }

        [TestMethod]
        public void SetMaxScale_ScaleNotChanged()
        {
            var sut = new Camera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 3,
            };
            sut.MaxScale = 4;
            Assert.AreEqual(4, sut.MaxScale);
            Assert.AreEqual(3, sut.Scale);
        }

        [TestMethod]
        public void SetMaxScale_ClampsScale()
        {
            var sut = new Camera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 4,
            };
            sut.MaxScale = 3;
            Assert.AreEqual(3, sut.MaxScale);
            Assert.AreEqual(sut.MaxScale, sut.Scale);
        }

        [DataTestMethod]
        [DataRow(2f, 2f, DisplayName = "Lower edge case")]
        [DataRow(5f, 5f, DisplayName = "Upper edge case")]
        [DataRow(1f, 2f, DisplayName = "Lower clamped")]
        [DataRow(6f, 5f, DisplayName = "Upper clamped")]
        public void ScaleAround_CorrectlyAssigned(float toSet, float expected)
        {
            var sut = new Camera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 3,
            };
            sut.ScaleAround(MVector2.Zero, toSet);
            Assert.AreEqual(expected, sut.Scale);
            // Missing assertion for coordinate
        }

        [DataTestMethod]
        [DataRow(0.5f, 3.5f, DisplayName = "Positive")]
        [DataRow(-0.5f, 2.5f, DisplayName = "Negative")]
        [DataRow(-1.5f, 2f, DisplayName = "Lower clamped")]
        [DataRow(3f, 5f, DisplayName = "Upper clamped")]
        public void ScaleAroundRelative_CorrectlyAssigned(float delta, float expected)
        {
            var sut = new Camera2D(MPoint2.One)
            {
                MinScale = 2,
                MaxScale = 5,
                Scale = 3,
            };
            sut.ScaleAroundRelative(MVector2.Zero, delta);
            Assert.AreEqual(expected, sut.Scale);
            // Missing assertion for coordinate
        }

        [DataTestMethod]
        [DataRow(2f, 6f, DisplayName = "Grow")]
        [DataRow(0.5f, 1.5f, DisplayName = "Shrink")]
        [DataRow(0.1f, 1f, DisplayName = "Lower clamped")]
        [DataRow(3f, 7f, DisplayName = "Upper clamped")]
        public void ZoomAround_CorrectlyAssigned(float zoom, float expected)
        {
            var sut = new Camera2D(MPoint2.One)
            {
                MinScale = 1,
                MaxScale = 7,
                Scale = 3,
            };
            sut.ZoomAround(MVector2.Zero, zoom);
            Assert.AreEqual(expected, sut.Scale);
            // Missing assertion for coordinate
        }
    }
}
