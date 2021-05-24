using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MonoKle.Tests
{
    [TestClass]
    public class TimerTests
    {
        [TestMethod]
        public void ParameterlessConstructor_CorrectState()
        {
            var span = TimeSpan.FromSeconds(123);
            var timer = new Timer(span, false);
            var sut = new Timer(span);
            Assert.IsTrue(AreEqual(timer, sut));
        }

        [DataTestMethod]
        [DataRow(false, DisplayName = "Non-triggered constructor")]
        [DataRow(true, DisplayName = "Triggered constructor")]
        public void Constructor_CorrectState(bool triggered)
        {
            var span = TimeSpan.FromSeconds(123);
            var timer = new Timer(span, triggered);
            Assert.AreEqual(span, timer.Duration);
            Assert.AreEqual(triggered ? TimeSpan.Zero : span, timer.TimeLeft);
            Assert.AreEqual(triggered, timer.IsDone);
            Assert.AreEqual(triggered, timer.IsTriggered);
        }

        [TestMethod]
        public void IsDone_TrueWhenDone()
        {
            var span = TimeSpan.FromSeconds(123);
            var timer = new Timer(span);
            timer.Update(span);
            Assert.IsTrue(timer.IsDone);
        }

        [TestMethod]
        public void IsDone_FalseWhenNotDone()
        {
            var span = TimeSpan.FromSeconds(123);
            var timer = new Timer(span);
            timer.Update(span - TimeSpan.FromSeconds(10));
            Assert.IsFalse(timer.IsDone);
        }

        [TestMethod]
        public void Reset_NotDone_ResetToDuration()
        {
            var span = TimeSpan.FromSeconds(123);
            var timer = new Timer(span);
            timer.Reset();
            Assert.IsFalse(timer.IsDone);
            Assert.AreEqual(span, timer.TimeLeft);
        }

        [TestMethod]
        public void Reset_Done_ResetToDuration()
        {
            var span = TimeSpan.FromSeconds(123);
            var timer = new Timer(span);
            timer.Trigger();
            timer.Reset();
            Assert.IsFalse(timer.IsDone);
            Assert.AreEqual(span, timer.TimeLeft);
        }

        [TestMethod]
        public void Set_NotDone_DurationSetAndTimeReset()
        {
            var span = TimeSpan.FromSeconds(123);
            var otherSpan = TimeSpan.FromSeconds(300);
            var timer = new Timer(span);
            timer.Set(otherSpan);
            Assert.IsFalse(timer.IsDone);
            Assert.AreEqual(otherSpan, timer.TimeLeft);
            Assert.AreEqual(otherSpan, timer.Duration);
        }

        [TestMethod]
        public void Set_Done_DurationSetAndTimeReset()
        {
            var span = TimeSpan.FromSeconds(123);
            var otherSpan = TimeSpan.FromSeconds(300);
            var timer = new Timer(span);
            timer.Trigger();
            timer.Set(otherSpan);
            Assert.IsFalse(timer.IsDone);
            Assert.AreEqual(otherSpan, timer.TimeLeft);
            Assert.AreEqual(otherSpan, timer.Duration);
        }

        [TestMethod]
        public void TimeLeft_CorrectResult()
        {
            var span = TimeSpan.FromSeconds(123);
            var updateSpan = TimeSpan.FromSeconds(39);
            var timer = new Timer(span);
            timer.Update(updateSpan);
            Assert.AreEqual(span - updateSpan, timer.TimeLeft);
        }

        [TestMethod]
        public void Trigger_NotRan_DoneAndZeroTimeLeft()
        {
            var span = TimeSpan.FromSeconds(123);
            var timer = new Timer(span);
            timer.Trigger();
            Assert.IsTrue(timer.IsDone);
            Assert.AreEqual(TimeSpan.Zero, timer.TimeLeft);
        }

        [TestMethod]
        public void Trigger_NotDone_DoneAndZeroTimeLeft()
        {
            var span = TimeSpan.FromSeconds(123);
            var timer = new Timer(span);
            timer.Update(TimeSpan.FromSeconds(30));
            timer.Trigger();
            Assert.IsTrue(timer.IsDone);
            Assert.AreEqual(TimeSpan.Zero, timer.TimeLeft);
        }

        [TestMethod]
        public void Trigger_Done_DoneAndZeroTimeLeft()
        {
            var span = TimeSpan.FromSeconds(123);
            var timer = new Timer(span);
            timer.Trigger();
            timer.Trigger();
            Assert.IsTrue(timer.IsDone);
            Assert.AreEqual(TimeSpan.Zero, timer.TimeLeft);
        }

        [TestMethod]
        public void Update_PartialUpdate_CorrectTimeLeft()
        {
            var span = TimeSpan.FromSeconds(123);
            var spanToUpdate = TimeSpan.FromSeconds(10);
            var timer = new Timer(span);

            timer.Update(spanToUpdate);

            Assert.IsFalse(timer.IsDone);
            Assert.AreEqual(span - spanToUpdate, timer.TimeLeft);
        }

        [TestMethod]
        public void Update_LongerElapsedThanSetDuration_ZeroTimeLeft()
        {
            var span = TimeSpan.FromSeconds(123);
            var spanToUpdate = TimeSpan.FromSeconds(300);
            var timer = new Timer(span);

            timer.Update(spanToUpdate);

            Assert.IsTrue(timer.IsDone);
            Assert.AreEqual(TimeSpan.Zero, timer.TimeLeft);
        }

        [TestMethod]
        public void Update_Done_ZeroTimeLeft()
        {
            var span = TimeSpan.FromSeconds(123);
            var spanToUpdate = TimeSpan.FromSeconds(10);
            var timer = new Timer(span);

            timer.Trigger();
            timer.Update(spanToUpdate);

            Assert.IsTrue(timer.IsDone);
            Assert.AreEqual(TimeSpan.Zero, timer.TimeLeft);
        }

        [TestMethod]
        public void UpdateDone_Done_OnlyTrueOnFirstCall()
        {
            var span = TimeSpan.FromSeconds(10);
            var spanToUpdate = TimeSpan.FromSeconds(100);
            var timer = new Timer(span);

            Assert.IsTrue(timer.UpdateDone(spanToUpdate));
            Assert.IsFalse(timer.UpdateDone(spanToUpdate));
        }

        [TestMethod]
        public void Trigger_LogicWorks()
        {
            var span = TimeSpan.FromSeconds(123);
            var spanToUpdate = TimeSpan.FromSeconds(10);
            var timer = new Timer(span);

            Assert.IsFalse(timer.IsTriggered);
            timer.Trigger();
            Assert.IsTrue(timer.IsTriggered);
            Assert.IsTrue(timer.IsTriggered);
            timer.Update(spanToUpdate);
            Assert.IsFalse(timer.IsTriggered);
        }

        private bool AreEqual(Timer first, Timer second)
        {
            return first.Duration == second.Duration && first.TimeLeft == second.TimeLeft &&
                first.IsTriggered == second.IsTriggered && first.IsDone == second.IsDone;
        }
    }
}
