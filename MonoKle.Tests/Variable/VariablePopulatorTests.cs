using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MonoKle.Configuration.Tests
{
    [TestClass]
    public class VariablePopulatorTests
    {
        private CVarFileLoader _populator;
        private CVarSystem _system;

        [TestInitialize()]
        public void Init()
        {
            _system = new CVarSystem(new Logging.Logger());
            _populator = new CVarFileLoader(_system);
        }

        [TestMethod]
        public void LoadText_CommentedLine()
        {
            LoadTextLine("test", "5", true);
            Assert.AreEqual(0, _system.Identifiers.Count);
        }

        [TestMethod]
        public void LoadText_MPoint2Line()
        {
            var p = new MPoint2(-27, 89);
            LoadTextLine("test", p.ToString(), false);
            Assert.AreEqual(p, _system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_MVector2Line()
        {
            var p = new MVector2(-27.7f, 89.23f);
            LoadTextLine("test", p.ToString(), false);
            Assert.AreEqual(p, _system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_FloatLine()
        {
            LoadTextLine("test", "5.8", false);
            Assert.AreEqual(5.8f, _system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_IntLine()
        {
            LoadTextLine("test", "5", false);
            Assert.AreEqual(5, _system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_Whitespace_Works()
        {
            _populator.LoadText("     a     =     7");
            Assert.AreEqual(7, _system.GetValue("a"));
        }

        [TestMethod]
        public void LoadText_Multiline()
        {
            string text = "a = 5\nb=2.5\n" + CVarFileLoader.CommentedLineToken + "kaka=5\nc=\"hej\"";
            _populator.LoadText(text);
            Assert.AreEqual(5, _system.GetValue("a"));
            Assert.AreEqual(2.5f, _system.GetValue("b"));
            Assert.AreEqual("hej", _system.GetValue("c"));
            Assert.AreEqual(3, _system.Identifiers.Count);
        }

        [TestMethod]
        public void LoadText_StringLine()
        {
            LoadTextLine("test", "\"apa\"", false);
            Assert.AreEqual("apa", _system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_TrueLine_CaseIgnored()
        {
            LoadTextLine("test", "tRuE", false);
            Assert.AreEqual(true, _system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_FalseLine_CaseIgnored()
        {
            LoadTextLine("test", "fAlSe", false);
            Assert.AreEqual(false, _system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_EmptyString()
        {
            LoadTextLine("a", "\"\"", false);
            Assert.AreEqual("", _system.GetValue("a"));
        }

        private void LoadTextLine(string variable, string value, bool commented)
        {
            string line = variable + CVarFileLoader.VariableValueDivisor + value;
            _populator.LoadText(commented ? CVarFileLoader.CommentedLineToken + line : line);
        }
    }
}