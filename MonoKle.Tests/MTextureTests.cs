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
            Assert.AreEqual(sut.Animate(0).AtlasRectangle, new MRectangleInt(10, 10, 5, 40));
        }

        [TestMethod]
        public void Animate_LastFrame_CorrectAtlasRectangle()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 10);
            Assert.AreEqual(sut.Animate(5).AtlasRectangle, new MRectangleInt(35, 10, 5, 40));
        }

        [TestMethod]
        public void Animate_AfterLastFrame_LoopsAnimation()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 10);
            Assert.AreEqual(sut.Animate(6).AtlasRectangle, sut.Animate(0).AtlasRectangle);
            Assert.AreEqual(sut.Animate(11).AtlasRectangle, sut.Animate(5).AtlasRectangle);
            Assert.AreEqual(sut.Animate(12).AtlasRectangle, sut.Animate(6).AtlasRectangle);
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
    }
}
