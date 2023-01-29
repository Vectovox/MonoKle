using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MonoKle.Configuration.Tests
{
    [TestClass]
    public class CVarFileLoaderTests
    {
        private CVarFileLoader _populator;
        private CVarSystem _system;

        [TestInitialize()]
        public void Init()
        {
            _system = new CVarSystem(Mock.Of<ILogger>());
            _populator = new CVarFileLoader(_system, Mock.Of<ILogger>());
        }

        [TestMethod]
        public void LoadText_CommentedLine_NoChange()
        {
            LoadTextLine("test", "5", true);
            Assert.AreEqual(0, _system.Identifiers.Count);
        }

        [TestMethod]
        public void LoadText_EmptyString_Works()
        {
            LoadTextLine("a", "", false);
            Assert.AreEqual("", _system.GetValue("a"));
        }

        [TestMethod]
        public void LoadText_Whitespace_Trimmed()
        {
            _populator.LoadText("     a     =     bepa");
            Assert.AreEqual("bepa", _system.GetValue("a"));
        }

        [TestMethod]
        public void LoadText_Multiline()
        {
            string text = "a = 5\nb=2.5\n" + CVarFileLoader.CommentedLineToken + "kaka=5\nc=hej";
            _populator.LoadText(text);
            Assert.AreEqual("5", _system.GetValue("a"));
            Assert.AreEqual("2.5", _system.GetValue("b"));
            Assert.AreEqual("hej", _system.GetValue("c"));
            Assert.AreEqual(3, _system.Identifiers.Count);
        }

        private void LoadTextLine(string variable, string value, bool commented)
        {
            string line = variable + CVarFileLoader.VariableValueDivisor + value;
            _populator.LoadText(commented ? CVarFileLoader.CommentedLineToken + line : line);
        }
    }
}