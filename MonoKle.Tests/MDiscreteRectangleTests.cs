using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Tests
{
    [TestClass]
    public class MDiscreteRectangleTests
    {
        private const int RANDOM_TEST_AMOUNT = 10;

        private readonly Random _random = new Random();

        [DataTestMethod]
        [DataRow(5, 6, 0, 0, 4, 5, DisplayName = "Positive size")]
        [DataRow(5, -6, 0, -5, 4, 0, DisplayName = "Positive Width and negative Height")]
        [DataRow(-5, 6, -4, 0, 0, 5, DisplayName = "Negative Width and positive Height")]
        [DataRow(-5, -6, -4, -5, 0, 0, DisplayName = "Negative size")]
        public void Constructor_SizeVariant_Discrete(int width, int height, int tlX, int tlY, int brX, int brY)
        {
            var a = new MDiscreteRectangle(width, height);
            Assert.AreEqual(new MPoint2(tlX, tlY), a.TopLeft);
            Assert.AreEqual(new MPoint2(brX, brY), a.BottomRight);

            // Test second size constructor
            var b = new MDiscreteRectangle(new MPoint2(width, height));
            Assert.AreEqual(a, b);

            // Test size parameter using origin coordinate
            var c = new MDiscreteRectangle(0, 0, width, height);
            Assert.AreEqual(a, c);
        }

        [DataTestMethod]
        [DataRow(2, 3, 5, 6, 2, 3, 6, 8, DisplayName = "Positive values")]
        [DataRow(-2, -3, 5, 7, -2, -3, 2, 3, DisplayName = "Negative coordinate and positive size")]
        [DataRow(2, 3, -5, -7, -2, -3, 2, 3, DisplayName = "Positive coordinate and negative size")]
        [DataRow(-2, -3, -5, -7, -6, -9, -2, -3, DisplayName = "Negative values")]
        public void Constructor_CoordinateVariant_Discrete(int x, int y, int width, int height, int tlX, int tlY, int brX, int brY)
        {
            var a = new MDiscreteRectangle(x, y, width, height);
            Assert.AreEqual(new MPoint2(tlX, tlY), a.TopLeft);
            Assert.AreEqual(new MPoint2(brX, brY), a.BottomRight);
        }

        [DataTestMethod]
        [DataRow(2, 3, 5, 6, 2, 3, 5, 6, DisplayName = "Positive points")]
        [DataRow(-2, -3, 5, 7, -2, -3, 5, 7, DisplayName = "Negative start point and positive end")]
        [DataRow(2, 3, -5, -7, -5, -7, 2, 3, DisplayName = "Positive start point and negative end")]
        [DataRow(-2, -3, -5, -7, -5, -7, -2, -3, DisplayName = "Negative points")]
        public void Constructor_PointsVariant_Discrete(int x, int y, int x2, int y2, int tlX, int tlY, int brX, int brY)
        {
            var a = new MDiscreteRectangle(new MPoint2(x, y), new MPoint2(x2, y2));
            Assert.AreEqual(new MPoint2(tlX, tlY), a.TopLeft);
            Assert.AreEqual(new MPoint2(brX, brY), a.BottomRight);
        }

        [TestMethod]
        public void Constructors_AreEquivalent()
        {
            var a = new MDiscreteRectangle(5, 5);
            var b = new MDiscreteRectangle(new MPoint2(0, 0), new MPoint2(4, 4));
            var c = new MDiscreteRectangle(0, 0, 5, 5);
            var d = new MDiscreteRectangle(new MPoint2(5, 5));

            Assert.AreEqual(a, b);
            Assert.AreEqual(b, c);
            Assert.AreEqual(c, d);
        }

        public void Constructors_NegativeSize_Normalized()
        {
            Assert.AreEqual(new MDiscreteRectangle(0, 0, 10, 10), new MDiscreteRectangle(10, 10, -10, -10));
            Assert.AreEqual(new MDiscreteRectangle(-10, -10, 10, 10), new MDiscreteRectangle(0, 0, -10, -10));
            Assert.AreEqual(new MDiscreteRectangle(-30, -30, 20, 20), new MDiscreteRectangle(-10, -10, -20, -20));
        }

        [TestMethod]
        public void Bottom() => Assert.AreEqual(1, new MDiscreteRectangle(-1, -2, 3, 4).Bottom);

        [TestMethod]
        public void BottomLeft() => Assert.AreEqual(new MPoint2(-1, 1), new MDiscreteRectangle(-1, -2, 3, 4).BottomLeft);

        [TestMethod]
        public void BottomRight() => Assert.AreEqual(new MPoint2(1, 1), new MDiscreteRectangle(-1, -2, 3, 4).BottomRight);

        [TestMethod]
        public void Left() => Assert.AreEqual(-1, new MDiscreteRectangle(-1, -2, 3, 4).Left);

        [TestMethod]
        public void Right() => Assert.AreEqual(1, new MDiscreteRectangle(-1, -2, 3, 4).Right);

        [TestMethod]
        public void Top() => Assert.AreEqual(-2, new MDiscreteRectangle(-1, -2, 3, 4).Top);

        [TestMethod]
        public void TopLeft() => Assert.AreEqual(new MPoint2(-1, -2), new MDiscreteRectangle(-1, -2, 3, 4).TopLeft);

        [TestMethod]
        public void TopRight() => Assert.AreEqual(new MPoint2(1, -2), new MDiscreteRectangle(-1, -2, 3, 4).TopRight);

        [TestMethod]
        public void Width() => Assert.AreEqual(5, new MDiscreteRectangle(5, 6).Width);

        [TestMethod]
        public void Height() => Assert.AreEqual(6, new MDiscreteRectangle(5, 6).Height);

        [TestMethod]
        public void Dimensions() => Assert.AreEqual(new MPoint2(5, 6), new MDiscreteRectangle(5, 6).Dimensions);

        [DataTestMethod]
        [DataRow(0, 0, 3, 4, 1f, 1.5f)]
        [DataRow(0, 0, 5, 6, 2f, 2.5f)]
        [DataRow(-2, -4, 2, 5, -1.5f, -2f)]
        public void Center(int x, int y, int width, int height, float cX, float cY)
            => Assert.AreEqual(new MVector2(cX, cY), new MDiscreteRectangle(x, y, width, height).Center);

        [TestMethod]
        public void Equals_Object()
        {
            Assert.IsTrue(new MDiscreteRectangle(0, 1, 2, 3).Equals((object)new MDiscreteRectangle(0, 1, 2, 3)));
            Assert.IsFalse(new MDiscreteRectangle(0, 1, -2, 3).Equals((object)new MDiscreteRectangle(0, 1, 2, 3)));
            Assert.IsFalse(new MDiscreteRectangle(0, 1, 2, 3).Equals((object)new MDiscreteRectangle(1, 1, 2, 3)));
        }

        [TestMethod]
        public void Equals_AreaInteger()
        {
            Assert.IsTrue(new MDiscreteRectangle(0, 1, 2, 3).Equals(new MDiscreteRectangle(0, 1, 2, 3)));
            Assert.IsFalse(new MDiscreteRectangle(0, 1, -2, 3).Equals(new MDiscreteRectangle(0, 1, 2, 3)));
            Assert.IsFalse(new MDiscreteRectangle(0, 1, 2, 3).Equals(new MDiscreteRectangle(1, 1, 2, 3)));
        }

        [TestMethod]
        public void OperatorEquals()
        {
            Assert.IsTrue(new MDiscreteRectangle(0, 1, 2, 3) == new MDiscreteRectangle(0, 1, 2, 3));
            Assert.IsFalse(new MDiscreteRectangle(0, 1, -2, 3) == new MDiscreteRectangle(0, 1, 2, 3));
            Assert.IsFalse(new MDiscreteRectangle(0, 1, 2, 3) == new MDiscreteRectangle(1, 1, 2, 3));
        }

        [TestMethod]
        public void OperatorNotEquals()
        {
            Assert.IsFalse(new MDiscreteRectangle(0, 1, 2, 3) != new MDiscreteRectangle(0, 1, 2, 3));
            Assert.IsTrue(new MDiscreteRectangle(0, 1, -2, 3) != new MDiscreteRectangle(0, 1, 2, 3));
            Assert.IsTrue(new MDiscreteRectangle(0, 1, 2, 3) != new MDiscreteRectangle(1, 1, 2, 3));
        }

        [TestMethod]
        public void Clamp_Self()
        {
            // Inside
            Assert.AreEqual(new MDiscreteRectangle(-1, -1, 2, 2), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MDiscreteRectangle(-1, -1, 2, 2)));
            Assert.AreEqual(new MDiscreteRectangle(-5, -5, 10, 10), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MDiscreteRectangle(-5, -5, 10, 10)));
            Assert.AreEqual(new MDiscreteRectangle(-5, -5, 10, 10), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MDiscreteRectangle(-10, -10, 20, 20)));

            // Outside
            Assert.AreEqual(new MDiscreteRectangle(-1, -1, 6, 2), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MDiscreteRectangle(-1, -1, 10, 2)));
            Assert.AreEqual(new MDiscreteRectangle(-1, -1, 2, 6), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MDiscreteRectangle(-1, -1, 2, 10)));
            Assert.AreEqual(new MDiscreteRectangle(-5, -1, 3, 5), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MDiscreteRectangle(-7, -1, 5, 5)));
            Assert.AreEqual(new MDiscreteRectangle(-1, -5, 5, 3), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MDiscreteRectangle(-1, -7, 5, 5)));
        }

        [TestMethod]
        public void Clamp_Point()
        {
            // Inside
            Assert.AreEqual(new MPoint2(0, 0), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MPoint2(0, 0)));
            Assert.AreEqual(new MPoint2(-5, -5), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MPoint2(-5, -5)));
            Assert.AreEqual(new MPoint2(4, 4), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MPoint2(4, 4)));

            // Outside
            Assert.AreEqual(new MPoint2(0, 4), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MPoint2(0, 7)));
            Assert.AreEqual(new MPoint2(4, 0), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MPoint2(7, 0)));
            Assert.AreEqual(new MPoint2(0, -5), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MPoint2(0, -7)));
            Assert.AreEqual(new MPoint2(-5, 0), new MDiscreteRectangle(-5, -5, 10, 10).Clamp(new MPoint2(-7, 0)));
        }

        [TestMethod]
        public void Contains_Point()
        {
            // Inside and edges
            Assert.IsTrue(new MDiscreteRectangle(50, 51, 50, 50).Contains(new MPoint2(50, 51)));
            Assert.IsTrue(new MDiscreteRectangle(50, 51, 50, 50).Contains(new MPoint2(99, 100)));
            Assert.IsTrue(new MDiscreteRectangle(50, 51, 50, 50).Contains(new MPoint2(75, 75)));

            // Inside and edges for negative position
            Assert.IsTrue(new MDiscreteRectangle(-50, -51, 50, 50).Contains(new MPoint2(-25, -25)));
            Assert.IsTrue(new MDiscreteRectangle(-50, -51, 50, 50).Contains(new MPoint2(-50, -51)));
            Assert.IsTrue(new MDiscreteRectangle(-50, -51, 50, 50).Contains(new MPoint2(-1, -2)));

            // Outside
            Assert.IsFalse(new MDiscreteRectangle(50, 51, 50, 50).Contains(new MPoint2(49, 51)));
            Assert.IsFalse(new MDiscreteRectangle(50, 51, 50, 50).Contains(new MPoint2(50, 50)));
            Assert.IsFalse(new MDiscreteRectangle(50, 51, 50, 50).Contains(new MPoint2(99, 101)));
            Assert.IsFalse(new MDiscreteRectangle(50, 51, 50, 50).Contains(new MPoint2(101, 100)));
            Assert.IsFalse(new MDiscreteRectangle(50, 51, 50, 50).Contains(new MPoint2(-75, -75)));

            // Outside for negative position
            Assert.IsFalse(new MDiscreteRectangle(-50, -51, 50, 50).Contains(new MPoint2(-50, -52)));
            Assert.IsFalse(new MDiscreteRectangle(-50, -51, 50, 50).Contains(new MPoint2(-51, -51)));
            Assert.IsFalse(new MDiscreteRectangle(-50, -51, 50, 50).Contains(new MPoint2(-1, 0)));
            Assert.IsFalse(new MDiscreteRectangle(-50, -51, 50, 50).Contains(new MPoint2(1, -2)));
        }

        [TestMethod]
        public void Contains_RectangleInt()
        {
            Assert.IsTrue(new MDiscreteRectangle(50, 51, 50, 50).Contains(new MDiscreteRectangle(50, 51, 50, 50)));
            Assert.IsTrue(new MDiscreteRectangle(50, 51, 50, 50).Contains(new MDiscreteRectangle(60, 60, 20, 20)));
            Assert.IsTrue(new MDiscreteRectangle(-10, -10, 20, 20).Contains(new MDiscreteRectangle(-5, -5, 10, 10)));

            Assert.IsFalse(new MDiscreteRectangle(-10, -10, 20, 20).Contains(new MDiscreteRectangle(-5, -5, 30, 10)));
            Assert.IsFalse(new MDiscreteRectangle(-10, -10, 20, 20).Contains(new MDiscreteRectangle(-5, -5, 10, 30)));
            Assert.IsFalse(new MDiscreteRectangle(-10, -10, 20, 20).Contains(new MDiscreteRectangle(-11, -5, 10, 10)));
            Assert.IsFalse(new MDiscreteRectangle(-10, -10, 20, 20).Contains(new MDiscreteRectangle(-5, -11, 10, 10)));
            Assert.IsFalse(new MDiscreteRectangle(-10, -10, 20, 20).Contains(new MDiscreteRectangle(50, 50, 10, 10)));
        }

        [DataTestMethod]
        [DataRow(9, 9, 6, 6, 10, 10, 5, 5, DisplayName = "Top left corner")]
        [DataRow(24, 9, 5, 6, 24, 10, 1, 5, DisplayName = "Top right corner")]
        [DataRow(9, 24, 6, 6, 10, 24, 5, 1, DisplayName = "Bottom left corner")]
        [DataRow(24, 24, 6, 6, 24, 24, 1, 1, DisplayName = "Bottom right corner")]
        [DataRow(9, 9, 20, 20, 10, 10, 15, 15, DisplayName = "Enveloped")]
        [DataRow(9, 11, 20, 5, 10, 11, 15, 5, DisplayName = "Horizontal cross")]
        [DataRow(15, 15, 1, 1, 15, 15, 1, 1, DisplayName = "Enveloped infinitesimal")]
        public void Intersect_CorrectValueCommutative(int x, int y, int w, int h, int ex, int ey, int ew, int eh)
        {
            var testRectangle = new MDiscreteRectangle(10, 10, 15, 15);
            var intersectRectangle = new MDiscreteRectangle(x, y, w, h);
            var expectedRectangle = new MDiscreteRectangle(ex, ey, ew, eh);
            // Assert we get the expected rectangle commutatively
            Assert.AreEqual(expectedRectangle, testRectangle.Intersect(intersectRectangle));
            Assert.AreEqual(expectedRectangle, intersectRectangle.Intersect(testRectangle));
        }

        [TestMethod]
        public void Redimension_CorrectlySet()
        {
            int x = 5;
            int y = 7;
            int newWidth = 50;
            int newHeight = 70;
            MDiscreteRectangle sut = new MDiscreteRectangle(x, y, 11, 31);
            MDiscreteRectangle expected = new MDiscreteRectangle(x, y, newWidth, newHeight);

            Assert.AreEqual(expected, sut.Redimension(newWidth, newHeight));
            Assert.AreEqual(expected, sut.Redimension(new MPoint2(newWidth, newHeight)));
            Assert.AreEqual(expected, sut.RedimensionWidth(newWidth).RedimensionHeight(newHeight));
        }

        [TestMethod]
        public void Resize_CorrectResizing()
        {
            for (int i = 0; i < RANDOM_TEST_AMOUNT; i++)
            {
                int x = _random.Next(-100, 100);
                int y = _random.Next(-100, 100);
                int width = _random.Next(1, 100);
                int height = _random.Next(1, 100);

                int dWidth = _random.Next(-100, 100);
                int dHeight = _random.Next(-100, 100);

                if (dWidth + width == 0)
                {
                    dWidth++;
                }
                if (dHeight + height == 0)
                {
                    dHeight++;
                }

                var sut = new MDiscreteRectangle(x, y, width, height);
                var expected = new MDiscreteRectangle(x, y, width + dWidth, height + dHeight);

                // Assert translation methods                
                Assert.AreEqual(expected, sut.Resize(new MPoint2(dWidth, dHeight)));
                Assert.AreEqual(expected, sut.ResizeWidth(dWidth).ResizeHeight(dHeight));
                Assert.AreEqual(expected, sut.Resize(dWidth, dHeight));
            }
        }

        [TestMethod]
        public void Translate_CorrectTranslation()
        {
            for (int i = 0; i < RANDOM_TEST_AMOUNT; i++)
            {
                int x = _random.Next(-100, 100);
                int y = _random.Next(-100, 100);
                int width = _random.Next(1, 100);
                int height = _random.Next(1, 100);

                int dx = _random.Next(-100, 100);
                int dy = _random.Next(-100, 100);

                var sut = new MDiscreteRectangle(x, y, width, height);
                var expected = new MDiscreteRectangle(x + dx, y + dy, width, height);

                // Assert translation methods                
                Assert.AreEqual(expected, sut.Translate(new MPoint2(dx, dy)));
                Assert.AreEqual(expected, sut.TranslateX(dx).TranslateY(dy));
                Assert.AreEqual(expected, sut.Translate(dx, dy));
            }
        }

        [TestMethod]
        public void Iterate_CorrectValuesReturned()
        {
            var result = new MDiscreteRectangle(-1, -1, 3, 4).Iterate().ToList();
            var expected = new List<MPoint2>
            {
                new MPoint2(-1, -1),
                new MPoint2(0, -1),
                new MPoint2(1, -1),
                new MPoint2(-1, 0),
                new MPoint2(0, 0),
                new MPoint2(1, 0),
                new MPoint2(-1, 1),
                new MPoint2(0, 1),
                new MPoint2(1, 1),
                new MPoint2(-1, 2),
                new MPoint2(0, 2),
                new MPoint2(1, 2),
            };
            CollectionAssert.AreEquivalent(expected, result);
        }

        [DataTestMethod]
        [DataRow(0, 0, 100, 100, 0, 0, 100, 100, DisplayName = "Self intersect")]
        [DataRow(0, 0, 100, 100, 10, 10, 50, 50, DisplayName = "A contains B")]
        [DataRow(10, 10, 50, 50, 0, 0, 100, 100, DisplayName = "B contains A")]
        [DataRow(-50, -50, 75, 75, 0, 0, 100, 100, DisplayName = "A intersects top-left B")]
        [DataRow(25, -25, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects top B")]
        [DataRow(75, -25, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects top-right B")]
        [DataRow(75, 25, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects right B")]
        [DataRow(75, 75, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects bottom-right B")]
        [DataRow(25, 75, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects bottom B")]
        [DataRow(-25, 75, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects bottom-left B")]
        [DataRow(-25, 25, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects left B")]
        [DataRow(-25, -25, 50, 200, 0, 0, 100, 100, DisplayName = "A covers left B")]
        [DataRow(-25, -25, 200, 50, 0, 0, 100, 100, DisplayName = "A covers top B")]
        [DataRow(-25, -25, 26, 26, 0, 0, 100, 100, DisplayName = "A touches B")]
        public void Intersects_TrueAndCommutative(int ax, int ay, int aw, int ah, int bx, int by, int bw, int bh)
        {
            var rectangleA = new MDiscreteRectangle(ax, ay, aw, ah);
            var rectangleB = new MDiscreteRectangle(bx, by, bw, bh);
            Assert.IsTrue(rectangleA.Intersects(rectangleB), "A does not intersect B");
            Assert.IsTrue(rectangleB.Intersects(rectangleA), "B does not intersect A");
        }

        [TestMethod]
        public void Intersects_FalseAndCommutative()
        {
            var rectangleA = new MDiscreteRectangle(0, 0, 25, 25);
            var rectangleB = new MDiscreteRectangle(-25, -25, 15, 15);
            Assert.IsFalse(rectangleA.Intersects(rectangleB), "A does intersect B");
            Assert.IsFalse(rectangleB.Intersects(rectangleA), "B does intersect A");
        }
    }
}