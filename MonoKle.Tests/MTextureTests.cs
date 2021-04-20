using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MonoKle.Tests
{
    [TestClass]
    public class MTextureTests
    {
        [TestMethod]
        public void Animate_FirstFrame_CorrectAtlasRectangle()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 10);
            Assert.AreEqual(new MRectangleInt(10, 10, 5, 40), sut.Animate(0).AtlasRectangle);
        }

        [TestMethod]
        public void Animate_LastFrame_CorrectAtlasRectangle()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 10);
            Assert.AreEqual(new MRectangleInt(35, 10, 5, 40), sut.Animate(5).AtlasRectangle);
        }

        [TestMethod]
        public void Animate_AfterLastFrame_LoopsAnimation()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 10);
            Assert.AreEqual(sut.Animate(0).AtlasRectangle, sut.Animate(6).AtlasRectangle);
            Assert.AreEqual(sut.Animate(5).AtlasRectangle, sut.Animate(11).AtlasRectangle);
            Assert.AreEqual(sut.Animate(6).AtlasRectangle, sut.Animate(12).AtlasRectangle);
        }

        [DataTestMethod]
        [DataRow(0, 12, DisplayName = "First frame")]
        [DataRow(1, 21, DisplayName = "Second frame")]
        [DataRow(2, 30, DisplayName = "Last frame")]
        public void Animate_AtlasMargin_CorrectAtlasRectangle(int frame, int expectedPosX)
        {
            const int height = 40;
            const int posY = 10;
            var initialAtlas = new MRectangleInt(10, posY, 27, height);
            var sut = new MTexture(null, initialAtlas, 3, 1, 2);
            var expectedAtlas = new MRectangleInt(expectedPosX, posY, 5, height);
            Assert.AreEqual(expectedAtlas, sut.Animate(frame).AtlasRectangle);
        }

        [TestMethod]
        public void Animate_AtlasMargin_AfterLastFrame_LoopsAnimation()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 10, 2);
            Assert.AreEqual(sut.Animate(0).AtlasRectangle, sut.Animate(6).AtlasRectangle);
            Assert.AreEqual(sut.Animate(5).AtlasRectangle, sut.Animate(11).AtlasRectangle);
            Assert.AreEqual(sut.Animate(6).AtlasRectangle, sut.Animate(12).AtlasRectangle);
        }

        [TestMethod]
        public void Animate_TimeSpan_FirstFrame_FirstAtlasRectangle()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 10);
            Assert.AreEqual(sut.Animate(TimeSpan.FromSeconds(0)).AtlasRectangle, sut.Animate(0).AtlasRectangle);
        }

        [TestMethod]
        public void Animate_TimeSpan_FirstFrame_SomeTimeElapsed_FirstAtlasRectangle()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 10);
            Assert.AreEqual(sut.Animate(TimeSpan.FromSeconds(0.05)).AtlasRectangle, sut.Animate(0).AtlasRectangle);
        }

        [TestMethod]
        public void Animate_TimeSpan_LastFrame_LastAtlasRectangle()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 10);
            Assert.AreEqual(sut.Animate(TimeSpan.FromSeconds(0.5)).AtlasRectangle, sut.Animate(5).AtlasRectangle);
        }

        [TestMethod]
        public void Animate_TimeSpan_AfterLastFrame_LoopsAnimation()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 10);
            Assert.AreEqual(sut.Animate(TimeSpan.FromSeconds(0.6)).AtlasRectangle, sut.Animate(0).AtlasRectangle);
            Assert.AreEqual(sut.Animate(TimeSpan.FromSeconds(1.1)).AtlasRectangle, sut.Animate(5).AtlasRectangle);
            Assert.AreEqual(sut.Animate(TimeSpan.FromSeconds(1.2)).AtlasRectangle, sut.Animate(6).AtlasRectangle);
        }

        [TestMethod]
        public void AnimationDuration_Correct()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 5, 10);
            Assert.AreEqual(TimeSpan.FromMilliseconds(500), sut.AnimationDuration);
        }

        [DataTestMethod]
        [DataRow(14, 2, 2, 3, DisplayName = "2 Margin")] // [][][X][X][X][][][][][Y][Y][Y][][]
        [DataRow(12, 3, 1, 2, DisplayName = "1 Margin")] // [][X][X][][][Y][Y][][][Z][Z][]
        [DataRow(6, 3, 0, 2, DisplayName = "0 Margin")]  // [X][X][Y][Y][Z][Z]
        public void FrameWidth_Correct(int width, int frameCount, int atlasMargin, int expectedResult)
        {
            var sut = new MTexture(null, new MRectangleInt(0, 0, width, 5), frameCount, 1, atlasMargin);
            Assert.AreEqual(expectedResult, sut.FrameWidth);
        }
    }
}
