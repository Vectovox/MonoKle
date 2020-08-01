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
            Button sut = new Button();

            Assert.IsFalse(sut.IsDown);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsDown);
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsDown);
        }

        [TestMethod]
        public void IsUp()
        {
            Button sut = new Button();

            Assert.IsTrue(sut.IsUp);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.IsFalse(sut.IsUp);
            sut.Update(false, TimeSpan.FromSeconds(1));
            Assert.IsTrue(sut.IsUp);
        }

        [TestMethod]
        public void IsHeld()
        {
            Button sut = new Button();

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
            Button sut = new Button();

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
            Button sut = new Button();

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
            Button sut = new Button();
            TimeSpan duration = TimeSpan.FromSeconds(2);

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
            Button sut = new Button();
            TimeSpan duration = TimeSpan.FromSeconds(2);

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
            Button sut = new Button();

            Assert.AreEqual(TimeSpan.FromSeconds(0), sut.HeldTime);
            sut.Update(true, TimeSpan.FromSeconds(1));
            Assert.AreEqual(TimeSpan.FromSeconds(1), sut.HeldTime);
            sut.Update(true, TimeSpan.FromSeconds(2));
            Assert.AreEqual(TimeSpan.FromSeconds(3), sut.HeldTime);
            sut.Update(false, TimeSpan.FromSeconds(2));
            Assert.AreEqual(TimeSpan.FromSeconds(0), sut.HeldTime);
        }
    }
}
