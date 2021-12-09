using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MonoKle.Input.Tests
{
    [TestClass]
    public class ButtonTests
    {
        [TestMethod]
        public void IsDown()
        {
            var sut = new Button();

            Assert.IsFalse(sut.IsDown);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsDown);
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsDown);
        }

        [TestMethod]
        public void IsUp()
        {
            var sut = new Button();

            Assert.IsTrue(sut.IsUp);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsUp);
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsUp);
        }

        [TestMethod]
        public void IsHeld()
        {
            var sut = new Button();

            Assert.IsFalse(sut.IsHeld);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeld);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsHeld);
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeld);
        }

        [TestMethod]
        public void IsPressed()
        {
            var sut = new Button();

            Assert.IsFalse(sut.IsPressed);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsPressed);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsPressed);
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsPressed);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsPressed);
        }

        [TestMethod]
        public void IsReleased()
        {
            var sut = new Button();

            Assert.IsFalse(sut.IsReleased);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsReleased);
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsReleased);
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsReleased);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsReleased);
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsReleased);
        }

        [TestMethod]
        public void IsHeldFor()
        {
            var sut = new Button();
            var duration = TimeSpan.FromSeconds(2);

            Assert.IsFalse(sut.IsHeldFor(duration));
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeldFor(duration));
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeldFor(duration));
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeldFor(duration));
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsHeldFor(duration));
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeldFor(duration));
        }

        [TestMethod]
        public void IsHeldForOnce()
        {
            var sut = new Button();
            var duration = TimeSpan.FromSeconds(2);

            Assert.IsFalse(sut.IsHeldForOnce(duration));
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeldForOnce(duration));
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeldForOnce(duration));
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeldForOnce(duration));
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsHeldForOnce(duration));
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeldForOnce(duration));
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsHeldForOnce(duration));
            sut.Update(true, TimeSpan.FromSeconds(2));
            Assert.IsTrue(sut.IsHeldForOnce(duration));
        }

        [TestMethod]
        public void HeldTime()
        {
            var sut = new Button();

            Assert.AreEqual(TimeSpan.FromSeconds(0), sut.HeldTime);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.AreEqual(TimeSpan.FromSeconds(1), sut.HeldTime);
            sut.Update(true, TimeSpan.FromSeconds(2));
            Assert.AreEqual(TimeSpan.FromSeconds(3), sut.HeldTime);
            sut.Update(false, TimeSpan.FromSeconds(2));
            Assert.AreEqual(TimeSpan.FromSeconds(0), sut.HeldTime);
        }

        [TestMethod]
        public void IsReleasedAfter()
        {
            var sut = new Button();
            var duration = TimeSpan.FromSeconds(2);

            // Initial value
            Assert.IsFalse(sut.IsReleasedAfter(duration));
            // Released before duration
            sut.Update(true, TimeSpan.FromSeconds(1));
            sut.Update(false, TimeSpan.Zero);
            Assert.IsFalse(sut.IsReleasedAfter(duration));
            // Released after duration
            sut.Update(true, TimeSpan.FromSeconds(2));
            sut.Update(false, TimeSpan.Zero);
            Assert.IsTrue(sut.IsReleasedAfter(duration));
            // Not released anymore
            sut.Update(false, TimeSpan.FromSeconds(2));
            Assert.IsFalse(sut.IsReleasedAfter(duration));
            // Duration passed but not released
            sut.Update(true, TimeSpan.FromSeconds(2));
            Assert.IsFalse(sut.IsReleasedAfter(duration));
        }
    }
}
