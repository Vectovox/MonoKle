namespace MonoKle.Variable {
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VariablePopulatorTest {
        private VariablePopulator populator;
        private VariableSystem system;

        [TestInitialize()]
        public void Init() {
            this.system = new VariableSystem();
            this.populator = new VariablePopulator(this.system);
        }

        [TestMethod]
        public void LoadText_CommentedLine() {
            this.LoadTextLine("test", "5", true);
            Assert.AreEqual(0, system.Identifiers.Count);
        }

        [TestMethod]
        public void LoadText_MPoint2Line() {
            var p = new MPoint2(-27, 89);
            this.LoadTextLine("test", p.ToString(), false);
            Assert.AreEqual(p, system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_MVector2Line() {
            var p = new MVector2(-27.7f, 89.23f);
            this.LoadTextLine("test", p.ToString(), false);
            Assert.AreEqual(p, system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_FloatLine() {
            this.LoadTextLine("test", "5.8", false);
            Assert.AreEqual(5.8f, system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_IntLine() {
            this.LoadTextLine("test", "5", false);
            Assert.AreEqual(5, system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_Whitespace_Works() {
            this.populator.LoadText("     a     =     7");
            Assert.AreEqual(7, system.GetValue("a"));
        }

        [TestMethod]
        public void LoadText_Multiline() {
            string text = "a = 5\nb=2.5\n" + VariablePopulator.CommentedLineToken + "kaka=5\nc=\"hej\"";
            this.populator.LoadText(text);
            Assert.AreEqual(5, this.system.GetValue("a"));
            Assert.AreEqual(2.5f, this.system.GetValue("b"));
            Assert.AreEqual("hej", this.system.GetValue("c"));
            Assert.AreEqual(3, system.Identifiers.Count);
        }

        [TestMethod]
        public void LoadText_StringLine() {
            this.LoadTextLine("test", "\"apa\"", false);
            Assert.AreEqual("apa", system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_TrueLine_CaseIgnored() {
            this.LoadTextLine("test", "tRuE", false);
            Assert.AreEqual(true, system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_FalseLine_CaseIgnored() {
            this.LoadTextLine("test", "fAlSe", false);
            Assert.AreEqual(false, system.GetValue("test"));
        }

        [TestMethod]
        public void LoadText_EmptyString() {
            this.LoadTextLine("a", "\"\"", false);
            Assert.AreEqual("", system.GetValue("a"));
        }

        private void LoadTextLine(string variable, string value, bool commented) {
            string line = variable + VariablePopulator.VariableValueDivisor + value;
            this.populator.LoadText(commented ? VariablePopulator.CommentedLineToken + line : line);
        }
    }
}