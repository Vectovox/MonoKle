namespace MonoKle.Core.Geometry
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class MCircleTest
    {
        [TestMethod]
        public void Area()
        {
            MCircle c = new MCircle(MVector2.Zero, 32f);
            Assert.AreEqual(32f * 32f * (float)Math.PI, c.Area);
        }

        [TestMethod]
        public void Circumference()
        {
            MCircle c = new MCircle(MVector2.Zero, 32f);
            Assert.AreEqual(2 * 32f * (float)Math.PI, c.Circumference);
        }

        [TestMethod]
        public void Contains_Inside()
        {
            MCircle c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Contains(MVector2.Zero));
            Assert.IsTrue(c.Contains(new MVector2(0, 32f)));
            Assert.IsTrue(c.Contains(new MVector2(0, -32f)));
            Assert.IsTrue(c.Contains(new MVector2(32f, 0)));
            Assert.IsTrue(c.Contains(new MVector2(-32f, 0)));
        }

        [TestMethod]
        public void Contains_Outside()
        {
            MCircle c = new MCircle(MVector2.Zero, 32f);
            Assert.IsFalse(c.Contains(new MVector2(32f, 32f)));
            Assert.IsFalse(c.Contains(new MVector2(-32f, -32f)));
            Assert.IsFalse(c.Contains(new MVector2(32f, -32f)));
            Assert.IsFalse(c.Contains(new MVector2(-32f, 32f)));
        }

        [TestMethod]
        public void Intersects_RectangleInside()
        {
            MCircle c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(-1f,-1f, 2f, 2f)));
        }

        [TestMethod]
        public void Intersects_CircleInside()
        {
            MCircle c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(-64f, -64f, 128f, 128f)));
        }

        [TestMethod]
        public void Intersects_Top()
        {
            MCircle c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(-64f, -20f, 128f, 128f)));
        }

        [TestMethod]
        public void Intersects_Bottom()
        {
            MCircle c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(-64f, -64f, 128f, 40f)));
        }

        [TestMethod]
        public void Intersects_Left()
        {
            MCircle c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(20f, -64f, 128f, 128f)));
        }

        [TestMethod]
        public void Intersects_Right()
        {
            MCircle c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(-64f, -64f, 40f, 128f)));
        }

        [TestMethod]
        public void Intersects_OutsideCorner_False()
        {
            MCircle c = new MCircle(new MVector2(-64, -64), 30f);
            Assert.IsFalse(c.Intersects(new MRectangle(-32f, -32f, 64f, 64f)));
        }
    }
}
