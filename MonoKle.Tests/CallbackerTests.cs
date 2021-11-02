using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MonoKle.Tests
{
    [TestClass]
    public class CallbackerTests
    {
        [TestMethod]
        public void Call_NoAction_ReturnsFalse()
        {
            var sut = new Callbacker();
            Assert.IsFalse(sut.CallOne());
        }

        [TestMethod]
        public void Call_OneAction_ActionCalled()
        {
            var called = false;
            var sut = new Callbacker();
            sut.Add(() => called = true);
            Assert.IsFalse(called);
            Assert.IsTrue(sut.CallOne());
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void Call_MultipleActions_Correct()
        {
            var called = false;
            var calledTwo = false;
            var sut = new Callbacker();
            sut.Add(() => called = true);
            sut.Add(() => calledTwo = true);
            Assert.IsTrue(sut.CallOne());
            Assert.IsTrue(called);
            Assert.IsFalse(calledTwo);
            Assert.IsTrue(sut.CallOne());
            Assert.IsTrue(calledTwo);
            Assert.IsFalse(sut.CallOne());
        }

        [TestMethod]
        public void AddWait_WaitsUntilCalled()
        {
            var called = false;
            var sut = new Callbacker();
            int callingThread = 0;

            var task = new Task(() =>
            {
                Thread.Sleep(500);
                sut.CallOne();
            });
            task.Start();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            sut.AddWait(() =>
            {
                called = true;
                callingThread = Thread.CurrentThread.ManagedThreadId;
            });
            stopwatch.Stop();

            Assert.IsTrue(called);
            Assert.IsTrue(stopwatch.ElapsedMilliseconds > 200);
            Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, callingThread);
        }
    }
}
