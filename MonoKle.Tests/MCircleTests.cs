using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MonoKle.Tests
{
    [TestClass]
    public class MCircleTests
    {
        [TestMethod]
        public void Area()
        {
            var c = new MCircle(MVector2.Zero, 32f);
            Assert.AreEqual(32f * 32f * (float)Math.PI, c.Area);
        }

        [TestMethod]
        public void Circumference()
        {
            var c = new MCircle(MVector2.Zero, 32f);
            Assert.AreEqual(2 * 32f * (float)Math.PI, c.Circumference);
        }

        [TestMethod]
        public void Contains_Inside()
        {
            var c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Contains(MVector2.Zero));
            Assert.IsTrue(c.Contains(new MVector2(0, 32f)));
            Assert.IsTrue(c.Contains(new MVector2(0, -32f)));
            Assert.IsTrue(c.Contains(new MVector2(32f, 0)));
            Assert.IsTrue(c.Contains(new MVector2(-32f, 0)));
        }

        [TestMethod]
        public void Contains_Outside()
        {
            var c = new MCircle(MVector2.Zero, 32f);
            Assert.IsFalse(c.Contains(new MVector2(32f, 32f)));
            Assert.IsFalse(c.Contains(new MVector2(-32f, -32f)));
            Assert.IsFalse(c.Contains(new MVector2(32f, -32f)));
            Assert.IsFalse(c.Contains(new MVector2(-32f, 32f)));
        }

        [TestMethod]
        public void Intersects_Bottom()
        {
            var c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(-64f, -64f, 128f, 40f)));
        }

        [TestMethod]
        public void Intersects_CircleInside()
        {
            var c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(-64f, -64f, 128f, 128f)));
        }

        [TestMethod]
        public void Intersects_Left()
        {
            var c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(20f, -64f, 128f, 128f)));
        }

        [TestMethod]
        public void Intersects_OutsideCorner_False()
        {
            var c = new MCircle(new MVector2(-64, -64), 30f);
            Assert.IsFalse(c.Intersects(new MRectangle(-32f, -32f, 64f, 64f)));
        }

        [TestMethod]
        public void Intersects_RectangleInside()
        {
            var c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(-1f, -1f, 2f, 2f)));
        }

        [TestMethod]
        public void Intersects_Right()
        {
            var c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(-64f, -64f, 40f, 128f)));
        }

        [TestMethod]
        public void Intersects_Top()
        {
            var c = new MCircle(MVector2.Zero, 32f);
            Assert.IsTrue(c.Intersects(new MRectangle(-64f, -20f, 128f, 128f)));
        }

        [TestMethod]
        public void MoveTo_Correct()
        {
            var c = new MCircle(new MVector2(-64, -64), 30f);
            var position = new MVector2(-32, -32);

            Assert.AreEqual(new MCircle(position, c.Radius), c.MoveTo(position));
        }

        [TestMethod]
        public void Resize_Correct()
        {
            var c = new MCircle(new MVector2(-64, -64), 30f);
            float radius = 20;

            Assert.AreEqual(new MCircle(c.Origin, radius), c.Resize(radius));
        }

        [TestMethod]
        public void Resize_NegativeRadius_Correct()
        {
            var c = new MCircle(new MVector2(-64, -64), 30f);
            float radius = -30;

            Assert.AreEqual(new MCircle(c.Origin, -radius), c.Resize(radius));
        }

        [TestMethod]
        public void Scale_Correct()
        {
            var c = new MCircle(new MVector2(-64, -64), 30f);
            float factor = 2;

            Assert.AreEqual(new MCircle(c.Origin, c.Radius * factor), c.Scale(factor));
        }

        [TestMethod]
        public void Scale_NegativeScaling_Correct()
        {
            var c = new MCircle(new MVector2(-64, -64), 30f);
            float factor = -3;

            Assert.AreEqual(new MCircle(c.Origin, c.Radius * -factor), c.Scale(factor));
        }

        [TestMethod]
        public void Separate_Circle_Separated()
        {
            var c1 = new MCircle(new MVector2(-64, -64), 30f);
            var c2 = new MCircle(new MVector2(-54, -74), 20f);
            MCircle separated = c1.Separate(c2);
            Assert.IsTrue(separated.SeparationVector(c2) == MVector2.Zero);
        }

        [TestMethod]
        public void Separate_MPoint2_Separated()
        {
            var c1 = new MCircle(new MVector2(-64, -64), 30f);
            var v = new MPoint2(-50, -50);
            MCircle separated = c1.Separate(v);
            Assert.IsTrue(separated.SeparationVector(v) == MVector2.Zero);
        }

        [TestMethod]
        public void Separate_MVector2_Separated()
        {
            var c1 = new MCircle(new MVector2(-64, -64), 30f);
            var v = new MVector2(-50, -50);
            MCircle separated = c1.Separate(v);
            Assert.IsTrue(separated.SeparationVector(v) == MVector2.Zero);
        }

        [TestMethod]
        public void Separate_Rectangle_Separated()
        {
            var c1 = new MCircle(new MVector2(-64, -64), 30f);
            var r = new MRectangle(-128, 0, 128, 70);
            MCircle separated = c1.Separate(r);
            Assert.IsTrue(separated.SeparationVector(r) == MVector2.Zero);
        }

        [TestMethod]
        public void SeparationVector_Circle_Correct()
        {
            var c1 = new MCircle(new MVector2(0, 3), 2f);
            var c2 = new MCircle(new MVector2(0, 2), 1f);
            MVector2 v = c1.SeparationVector(c2);
            var expected = new MVector2(0, 2);
            Assert.AreEqual(expected, v);
        }

        [TestMethod]
        public void SeparationVector_Circle_Outside_ZeroVector()
        {
            var c1 = new MCircle(new MVector2(0, 3), 2f);
            var c2 = new MCircle(new MVector2(-300, 2), 1f);
            MVector2 v = c1.SeparationVector(c2);
            Assert.AreEqual(MVector2.Zero, v);
        }

        [TestMethod]
        public void SeparationVector_MPoint2_Correct()
        {
            var c1 = new MCircle(new MVector2(0, 3), 2f);
            var v = new MPoint2(0, 4);
            MVector2 separated = c1.SeparationVector(v);
            var expected = new MVector2(0, -1);
            Assert.AreEqual(expected, separated);
        }

        [TestMethod]
        public void SeparationVector_MPoint2_Outside_ZeroVector()
        {
            var c1 = new MCircle(new MVector2(-300, 3), 2f);
            var v = new MPoint2(0, 4);
            MVector2 separated = c1.SeparationVector(v);
            Assert.AreEqual(MVector2.Zero, separated);
        }

        [TestMethod]
        public void SeparationVector_MVector2_Correct()
        {
            var c1 = new MCircle(new MVector2(0, 3), 2f);
            var v = new MVector2(0, 4);
            MVector2 separated = c1.SeparationVector(v);
            var expected = new MVector2(0, -1);
            Assert.AreEqual(expected, separated);
        }

        [TestMethod]
        public void SeparationVector_MVector2_Outside_ZeroVector()
        {
            var c1 = new MCircle(new MVector2(0, 300), 2f);
            var v = new MVector2(0, 4);
            MVector2 separated = c1.SeparationVector(v);
            Assert.AreEqual(MVector2.Zero, separated);
        }

        [TestMethod]
        public void SeparationVector_Rectangle_BottomLeft_CorrectDirection()
        {
            var c = new MCircle(new MVector2(-2, 2), 2f);
            var r = new MRectangle(-1, -1, 2, 2);
            MVector2 sepVector = c.SeparationVector(r);
            Assert.IsTrue(sepVector.X < 0);
            Assert.IsTrue(sepVector.Y > 0);
        }

        [TestMethod]
        public void SeparationVector_Rectangle_BottomRight_CorrectDirection()
        {
            var c = new MCircle(new MVector2(2, 2), 2f);
            var r = new MRectangle(-1, -1, 2, 2);
            MVector2 sepVector = c.SeparationVector(r);
            Assert.IsTrue(sepVector.X > 0);
            Assert.IsTrue(sepVector.Y > 0);
        }

        [TestMethod]
        public void SeparationVector_Rectangle_Down_Correct()
        {
            var c = new MCircle(new MVector2(0, 2), 2f);
            var r = new MRectangle(-32, 0, 64, 1);
            MVector2 sepVector = c.SeparationVector(r);
            var expected = new MVector2(0, 1);
            Assert.AreEqual(expected, sepVector);
        }

        [TestMethod]
        public void SeparationVector_Rectangle_Left_Correct()
        {
            var c = new MCircle(new MVector2(0, 0), 2f);
            var r = new MRectangle(1, -32, 1, 64);
            MVector2 sepVector = c.SeparationVector(r);
            var expected = new MVector2(-1, 0);
            Assert.AreEqual(expected, sepVector);
        }

        [TestMethod]
        public void SeparationVector_Rectangle_Outside_Corner_ZeroVector()
        {
            var c = new MCircle(new MVector2(-128, 128), 32f);
            var r = new MRectangle(-64, -64, 128, 128);
            MVector2 sepVector = c.SeparationVector(r);
            Assert.AreEqual(MVector2.Zero, sepVector);
        }

        [TestMethod]
        public void SeparationVector_Rectangle_Outside_ZeroVector()
        {
            var c = new MCircle(new MVector2(100, -100), 2f);
            var r = new MRectangle(1, -32, 1, 64);
            MVector2 sepVector = c.SeparationVector(r);
            Assert.AreEqual(MVector2.Zero, sepVector);
        }

        [TestMethod]
        public void SeparationVector_Rectangle_Right_Correct()
        {
            var c = new MCircle(new MVector2(3, 0), 2f);
            var r = new MRectangle(1, -32, 1, 64);
            MVector2 sepVector = c.SeparationVector(r);
            var expected = new MVector2(1, 0);
            Assert.AreEqual(expected, sepVector);
        }

        [TestMethod]
        public void SeparationVector_Rectangle_TopLeft_CorrectDirection()
        {
            var c = new MCircle(new MVector2(-2, -2), 2f);
            var r = new MRectangle(-1, -1, 2, 2);
            MVector2 sepVector = c.SeparationVector(r);
            Assert.IsTrue(sepVector.X < 0);
            Assert.IsTrue(sepVector.Y < 0);
        }

        [TestMethod]
        public void SeparationVector_Rectangle_TopRight_CorrectDirection()
        {
            var c = new MCircle(new MVector2(2, -2), 2f);
            var r = new MRectangle(-1, -1, 2, 2);
            MVector2 sepVector = c.SeparationVector(r);
            Assert.IsTrue(sepVector.X > 0);
            Assert.IsTrue(sepVector.Y < 0);
        }

        [TestMethod]
        public void SeparationVector_Rectangle_Up_Correct()
        {
            var c = new MCircle(new MVector2(0, 0), 2f);
            var r = new MRectangle(-32, 1, 64, 1);
            MVector2 sepVector = c.SeparationVector(r);
            var expected = new MVector2(0, -1);
            Assert.AreEqual(expected, sepVector);
        }

        [TestMethod]
        public void Translate_Correct()
        {
            var c = new MCircle(new MVector2(-64, -64), 30f);
            var translation = new MVector2(-32, -32);

            Assert.AreEqual(new MCircle(c.Origin + translation, c.Radius), c.Translate(translation));
        }
    }
}