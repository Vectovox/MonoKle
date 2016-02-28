namespace MonoKle
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TimerTest
    {
        [TestMethod]
        public void TestConstructors()
        {
            TimeSpan d = new TimeSpan(1234567);
            Timer t = new Timer(d);
            Timer t2 = new Timer(d.TotalSeconds);
            Assert.AreEqual(t.Duration, t2.Duration);
        }

        [TestMethod]
        public void TestDuration()
        {
            double duration = 0.79;
            Timer t = new Timer(duration);
            Assert.AreEqual(t.Duration, duration);
        }

        [TestMethod]
        public void TestIsDone()
        {
            double duration = 0.79;
            Timer t = new Timer(duration);
            t.Update(duration);
            Assert.IsTrue(t.IsDone());
        }

        [TestMethod]
        public void TestReset()
        {
            double duration = 0.79;
            double subtract = 0.5;
            Timer t = new Timer(duration);
            t.Update(subtract);
            t.Reset();
            Assert.AreEqual(t.GetTimeLeft(), duration);

            t.Update(duration);
            t.Reset();
            Assert.IsFalse(t.IsDone());
        }

        [TestMethod]
        public void TestSet()
        {
            double duration = 0.79;
            double duration2 = 0.65;
            double subtract = 0.5;
            Timer t = new Timer(duration);
            t.Update(subtract);
            t.Set(duration2);
            Assert.AreEqual(t.GetTimeLeft(), duration2);

            t.Update(duration2);
            t.Reset();
            Assert.IsFalse(t.IsDone());
            Assert.AreEqual(t.GetTimeLeft(), duration2);
            Assert.AreEqual(t.Duration, duration2);

            // Test that timespan works as well
            TimeSpan d1 = new TimeSpan(123456);
            t.Set(d1.TotalSeconds);
            double tmp = t.Duration;
            t.Set(d1);
            Assert.AreEqual(tmp, t.Duration);
        }

        [TestMethod]
        public void TestTimeLeft()
        {
            double duration = 0.79;
            double subtract = 0.5;
            Timer t = new Timer(duration);
            t.Update(subtract);
            Assert.AreEqual(t.GetTimeLeft(), duration - subtract);
        }

        [TestMethod]
        public void TestTrigger()
        {
            Timer t = new Timer(500);
            t.Trigger();
            Assert.IsTrue(t.IsDone());
        }

        [TestMethod]
        public void TestUpdate()
        {
            TimeSpan duration = new TimeSpan(123456);
            TimeSpan sub = new TimeSpan(1234);
            Timer t = new Timer(duration);
            Assert.IsTrue(t.Update(duration));
            Assert.IsTrue(t.IsDone());
            Assert.AreEqual(0, t.GetTimeLeft());
            t.Reset();
            Assert.IsTrue(t.Update(duration.TotalSeconds));
            Assert.IsTrue(t.IsDone());
            Assert.AreEqual(0, t.GetTimeLeft());
            t.Reset();
            Assert.IsFalse(t.Update(sub));
            Assert.IsFalse(t.IsDone());
            Assert.AreEqual(duration.TotalSeconds - sub.TotalSeconds, t.GetTimeLeft());
        }
    }
}