using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MonoKle.Tests
{
    [TestClass]
    public class CyclicTimerTests
    {
        [TestMethod]
        public void Constructor_DurationSet()
        {
            var sut = new CyclicTimer(TimeSpan.FromSeconds(63));
            Assert.AreEqual(63, sut.Duration.TotalSeconds);
        }

        [TestMethod]
        public void Constructor_ZeroDuration_Exception()
        {
            Assert.ThrowsException<ArgumentException>(() => new CyclicTimer(TimeSpan.Zero));
        }

        [TestMethod]
        public void SetDuration_ZeroDuration_Exception()
        {
            var sut = new CyclicTimer(TimeSpan.FromSeconds(1));
            Assert.ThrowsException<ArgumentException>(() => sut.Duration = TimeSpan.Zero);
        }

        [DataTestMethod]
        [DataRow(0, 0, DisplayName = "No time passed")]
        [DataRow(1999, 0, DisplayName = "Not enough time passed")]
        [DataRow(2000, 1, DisplayName = "Exactly one cycle passed")]
        [DataRow(3999, 1, DisplayName = "Almost two cycles passed")]
        [DataRow(4000, 2, DisplayName = "Two cycles passed")]
        [DataRow(6000, 3, DisplayName = "Three cycles passed")]
        public void Update_CorrectCyclesReturned(int millisecondsUpdated, int expectedCycles)
        {
            var sut = new CyclicTimer(TimeSpan.FromSeconds(2));
            Assert.AreEqual(expectedCycles, sut.Update(TimeSpan.FromMilliseconds(millisecondsUpdated)));
        }

        [TestMethod]
        public void Reset_DoesNotTrigger()
        {
            var sut = new CyclicTimer(TimeSpan.FromSeconds(1));
            sut.Update(TimeSpan.FromMilliseconds(999));
            sut.Reset();
            Assert.AreEqual(0, sut.Update(TimeSpan.FromMilliseconds(999)));
        }
    }
}
