using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MonoKle.Console.Tests
{
    [TestClass]
    public class CommandStringTests
    {
        [DataTestMethod]
        [DataRow("", DisplayName = "Empty command is not valid")]
        [DataRow("-", DisplayName = "Just a flag token is not valid")]
        [DataRow("-myCommand", DisplayName = "Command may not start with flag token")]
        [DataRow("myCommand -n -n", DisplayName = "Flags may not appear twice")]
        [DataRow("myCommand -n 20 -n 20", DisplayName = "Named arguments may not appear twice")]
        [DataRow("myCommand -n -n 20", DisplayName = "Named arguments and flags may not conflict in name")]
        [DataRow("myCommand -", DisplayName = "No hanging flag")]
        [DataRow("myCommand -n -", DisplayName = "No hanging named parameter")]
        public void TryParse_InvalidLine_FailedResult(string line)
        {
            bool result = CommandString.TryParse(line, out var commandString);
            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, commandString.Command);
            Assert.AreEqual(0, commandString.PositionalArguments.Count);
            Assert.AreEqual(0, commandString.Flags.Count);
            Assert.AreEqual(0, commandString.NamedArguments.Count);
        }

        [TestMethod]
        public void TryParse_JustCommand_CommandParsed()
        {
            const string commandLine = "testCommand";
            bool result = CommandString.TryParse(commandLine, out var commandString);
            Assert.IsTrue(result);
            Assert.AreEqual(commandLine, commandString.Command);
            Assert.AreEqual(0, commandString.PositionalArguments.Count);
            Assert.AreEqual(0, commandString.Flags.Count);
            Assert.AreEqual(0, commandString.NamedArguments.Count);
        }

        [TestMethod]
        public void TryParse_Positionals_CommandParsed()
        {
            string commandLine = "testCommand arg1 arg2";
            bool result = CommandString.TryParse(commandLine, out var commandString);
            Assert.IsTrue(result);
            Assert.AreEqual("testCommand", commandString.Command);
            Assert.AreEqual(2, commandString.PositionalArguments.Count);
            Assert.AreEqual("arg1", commandString.PositionalArguments[0]);
            Assert.AreEqual("arg2", commandString.PositionalArguments[1]);
            Assert.AreEqual(0, commandString.Flags.Count);
            Assert.AreEqual(0, commandString.NamedArguments.Count);
        }

        [TestMethod]
        public void TryParse_Flags_CommandParsed()
        {
            string commandLine = "testCommand -flag1 -flag2";
            bool result = CommandString.TryParse(commandLine, out var commandString);
            Assert.IsTrue(result);
            Assert.AreEqual("testCommand", commandString.Command);
            Assert.AreEqual(0, commandString.PositionalArguments.Count);
            Assert.AreEqual(2, commandString.Flags.Count);
            Assert.IsTrue(commandString.Flags.Contains("flag1"));
            Assert.IsTrue(commandString.Flags.Contains("flag2"));
            Assert.AreEqual(0, commandString.NamedArguments.Count);
        }

        [TestMethod]
        public void TryParse_Named_CommandParsed()
        {
            string commandLine = "testCommand -named1 value1 -named2 value2";
            bool result = CommandString.TryParse(commandLine, out var commandString);
            Assert.IsTrue(result);
            Assert.AreEqual("testCommand", commandString.Command);
            Assert.AreEqual(0, commandString.PositionalArguments.Count);
            Assert.AreEqual(0, commandString.Flags.Count);
            Assert.AreEqual(2, commandString.NamedArguments.Count);
            Assert.IsTrue(commandString.NamedArguments.ContainsKey("named1"));
            Assert.IsTrue(commandString.NamedArguments.ContainsKey("named2"));
            Assert.AreEqual("value1", commandString.NamedArguments["named1"]);
            Assert.AreEqual("value2", commandString.NamedArguments["named2"]);
        }

        [TestMethod]
        public void TryParse_CommandAndPositionalsAndFlagsAndNamed_CommandParsed()
        {
            string commandLine = "testCommand arg1 arg2 -named1 value1 -named2 value2 -flag1 -flag2";
            bool result = CommandString.TryParse(commandLine, out var commandString);
            Assert.IsTrue(result);
            Assert.AreEqual("testCommand", commandString.Command);
            Assert.AreEqual(2, commandString.PositionalArguments.Count);
            Assert.AreEqual("arg1", commandString.PositionalArguments[0]);
            Assert.AreEqual("arg2", commandString.PositionalArguments[1]);
            Assert.AreEqual(2, commandString.Flags.Count);
            Assert.IsTrue(commandString.Flags.Contains("flag1"));
            Assert.IsTrue(commandString.Flags.Contains("flag2"));
            Assert.AreEqual(2, commandString.NamedArguments.Count);
            Assert.IsTrue(commandString.NamedArguments.ContainsKey("named1"));
            Assert.IsTrue(commandString.NamedArguments.ContainsKey("named2"));
            Assert.AreEqual("value1", commandString.NamedArguments["named1"]);
            Assert.AreEqual("value2", commandString.NamedArguments["named2"]);
        }

        [TestMethod]
        public void TryParse_FlagsAndNamedIntermixed_CommandParsed()
        {
            string commandLine = "testCommand -named1 value1 -flag1 -named2 value2 -flag2";
            bool result = CommandString.TryParse(commandLine, out var commandString);
            Assert.IsTrue(result);
            Assert.AreEqual("testCommand", commandString.Command);
            Assert.AreEqual(0, commandString.PositionalArguments.Count);
            Assert.AreEqual(2, commandString.Flags.Count);
            Assert.IsTrue(commandString.Flags.Contains("flag1"));
            Assert.IsTrue(commandString.Flags.Contains("flag2"));
            Assert.AreEqual(2, commandString.NamedArguments.Count);
            Assert.IsTrue(commandString.NamedArguments.ContainsKey("named1"));
            Assert.IsTrue(commandString.NamedArguments.ContainsKey("named2"));
            Assert.AreEqual("value1", commandString.NamedArguments["named1"]);
            Assert.AreEqual("value2", commandString.NamedArguments["named2"]);
        }

        [TestMethod]
        public void TryParse_StringArgument_SpacesKept()
        {
            string commandLine = "testCommand \"Testing my string\"";
            bool result = CommandString.TryParse(commandLine, out var commandString);
            Assert.IsTrue(result);
            Assert.AreEqual("testCommand", commandString.Command);
            Assert.AreEqual(1, commandString.PositionalArguments.Count);
            Assert.AreEqual("Testing my string", commandString.PositionalArguments[0]);
            Assert.AreEqual(0, commandString.Flags.Count);
            Assert.AreEqual(0, commandString.NamedArguments.Count);
        }
    }
}
