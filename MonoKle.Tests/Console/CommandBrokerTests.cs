using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Console.Tests
{
    [TestClass]
    public class CommandBrokerTests
    {
        [TestMethod]
        public void Register_CommandInstance_CommandFound()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            var command = new InstanceTestCommand(25);
            commandBroker.Register(command);
            CollectionAssert.Contains(commandBroker.Commands.ToList(), "instanceTestCommand");
        }

        [TestMethod]
        public void Register_NewCommand_CommandFound()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<TypeTestCommand>();
            CollectionAssert.Contains(commandBroker.Commands.ToList(), "typeTestCommand");
        }

        [TestMethod]
        public void Register_DoubleRegister_Exception()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<TypeTestCommand>();
            Assert.ThrowsException<ArgumentException>(() => commandBroker.Register<TypeTestCommand>());
        }

        [TestMethod]
        public void Register_CommandWithoutAttribute_Exception()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            Assert.ThrowsException<ArgumentException>(() => commandBroker.Register<NoAttributeTestCommand>());
        }

        [TestMethod]
        public void Register_GapInPositionalArguments_Exception()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            Assert.ThrowsException<ArgumentException>(() => commandBroker.Register<PositionalGapTestCommand>());
        }

        [TestMethod]
        public void Register_RequiredPositionalAfterOptional_Exception()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            Assert.ThrowsException<ArgumentException>(() => commandBroker.Register<RequiredAfterOptionalPositionalTestCommand>());
        }

        [TestMethod]
        public void Register_NoPositionalZero_Exception()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            Assert.ThrowsException<ArgumentException>(() => commandBroker.Register<NoPositionalZeroTestCommand>());
        }

        [TestMethod]
        public void Register_UnregisteredCommand_NoException()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<TypeTestCommand>();
            commandBroker.Unregister<TypeTestCommand>();
            commandBroker.Register<TypeTestCommand>();
        }

        [TestMethod]
        public void Register_CommandWithoutInterface_Exception()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            Assert.ThrowsException<ArgumentException>(() => commandBroker.Register(typeof(NoInterfaceTestCommand)));
        }

        [TestMethod]
        public void Register_NoParameterlessConstructor_Exception()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            Assert.ThrowsException<ArgumentException>(() => commandBroker.Register(typeof(NoParameterlessConstructorTestCommand)));
        }

        [TestMethod]
        public void RegisterCallingAssembly_InvalidCommandsPresent_CommandRegisteredWithNoExceptions()
        {
            // Checks at least one as maintaining assertions for all valid commands will be annoying
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            int result = commandBroker.RegisterCallingAssembly();
            Assert.IsTrue(commandBroker.Contains("emptyTestCommand"));
            Assert.AreEqual(result, commandBroker.Commands.Count());
        }

        [TestMethod]
        public void Unregister_CommandWithoutInterface_Exception()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            Assert.ThrowsException<ArgumentException>(() => commandBroker.Unregister(typeof(NoInterfaceTestCommand)));
        }

        [TestMethod]
        public void Unregister_CommandWithoutParameterlessConstructor_Removed()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register(new NoParameterlessConstructorTestCommand(true));
            commandBroker.Unregister(typeof(NoParameterlessConstructorTestCommand));
            Assert.AreEqual(0, commandBroker.Commands.Count());
        }

        [TestMethod]
        public void Unregister_CommandNotFound()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<TypeTestCommand>();
            commandBroker.Unregister<TypeTestCommand>();
            CollectionAssert.DoesNotContain(commandBroker.Commands.ToList(), "typeTestCommand");
        }

        [TestMethod]
        public void Clear_CommandNotFound()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<TypeTestCommand>();
            commandBroker.Register<EmptyTestCommand>();
            commandBroker.Clear();
            Assert.AreEqual(0, commandBroker.Commands.Count());
        }

        [TestMethod]
        public void GetInformation_ExistingCommand_DescriptionGotten()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<TypeTestCommand>();
            var result = commandBroker.GetInformation("typeTestCommand");
            Assert.AreEqual("testDescription", result.Command.Description);
        }

        [TestMethod]
        public void GetInformation_NonExistingCommand_ThrowsException()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            Assert.ThrowsException<ArgumentException>(() => commandBroker.GetInformation("typeTestCommand"));
        }

        [TestMethod]
        public void Call_CommandInstance_CommandCalled()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            var command = new InstanceTestCommand(25);
            commandBroker.Register(command);
            CommandString.TryParse("instanceTestCommand", out var commandString);
            commandBroker.Call(commandString);
            Assert.AreEqual(25, command.WasCalledWith);
        }

        [TestMethod]
        public void Call_CorrectCommandCalled()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<EmptyTestCommand>();
            commandBroker.Register<TypeTestCommand>();
            EmptyTestCommand.Reset();
            TypeTestCommand.Reset();
            CommandString.TryParse("typeTestCommand", out var commandString);
            var result = commandBroker.Call(commandString);
            Assert.IsTrue(result);
            Assert.IsTrue(TypeTestCommand.WasCalled);
            Assert.IsFalse(EmptyTestCommand.WasCalled);
        }

        [TestMethod]
        public void Call_Positional_WithAndWithoutRequiredInSequence_NonRequiredGetsDefaultValue()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            var command = new DefaultValueTestCommand();
            commandBroker.Register(command);
            CommandString.TryParse("defaultValueTestCommand true", out var commandString);
            commandBroker.Call(commandString);
            CommandString.TryParse("defaultValueTestCommand", out var commandString2);
            commandBroker.Call(commandString2);
            Assert.AreEqual(false, command.NonRequiredPositional);
        }

        [TestMethod]
        public void Call_Argument_WithAndWithoutRequiredInSequence_NonRequiredGetsDefaultValue()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            var command = new DefaultValueTestCommand();
            commandBroker.Register(command);
            CommandString.TryParse("defaultValueTestCommand -arg true", out var commandString);
            commandBroker.Call(commandString);
            CommandString.TryParse("defaultValueTestCommand", out var commandString2);
            commandBroker.Call(commandString2);
            Assert.AreEqual(false, command.NonRequiredArgument);
        }

        [TestMethod]
        public void Call_DifferentTypes_TypesConvertedAndAssigned()
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<TypeTestCommand>();
            TypeTestCommand.Reset();
            CommandString.TryParse("typeTestCommand 25 21 19.0 true", out var commandString);
            var result = commandBroker.Call(commandString);
            Assert.IsTrue(result);
            Assert.IsTrue(TypeTestCommand.WasCalled);
            Assert.AreEqual("25", TypeTestCommand.LastString);
            Assert.AreEqual(21, TypeTestCommand.LastInt32);
            Assert.AreEqual(19f, TypeTestCommand.LastFloat);
            Assert.AreEqual(true, TypeTestCommand.LastBool);
        }

        [DataTestMethod]
        [DataRow("argumentUsageTest pos1 pos2 -named1 val1", DisplayName = "Minimal case")]
        [DataRow("argumentUsageTest pos1 pos2 pos3 -named1 val1 -named2 val2")]
        [DataRow("argumentUsageTest pos1 pos2 pos3 -flag1 -named1 val1 -named2 val2 -flag2", DisplayName = "Flags intermixed")]
        public void Call_ArgumentUsageTest_Success(string input)
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<ArgumentUsageTestCommand>();
            CommandString.TryParse(input, out var commandString);
            var result = commandBroker.Call(commandString);
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("argumentUsageTest pos1 -named1 val1", DisplayName = "Missing positional")]
        [DataRow("argumentUsageTest pos1 pow2 -named2 val2", DisplayName = "Missing named")]
        [DataRow("argumentUsageTest -named2 val2", DisplayName = "Missing required")]
        public void Call_ArgumentUsageTest_Failure(string input)
        {
            var console = new Mock<IGameConsole>();
            console.SetupGet(c => c.Log).Returns(new GameConsoleLogData());
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<ArgumentUsageTestCommand>();
            CommandString.TryParse(input, out var commandString);
            var result = commandBroker.Call(commandString);
            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow("argumentAssignmentTest 5", 5, 0, false)]
        [DataRow("argumentAssignmentTest -named 7", 0, 7, false)]
        [DataRow("argumentAssignmentTest -flag", 0, 0, true)]
        [DataRow("argumentAssignmentTest 3 -named 29 -flag", 3, 29, true)]
        public void Call_ArgumentAssignmentTest_Success(string command, int positional, int named, bool flag)
        {
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<ArgumentAssignmentTestCommand>();
            ArgumentAssignmentTestCommand.Reset();
            CommandString.TryParse(command, out var commandString);
            var result = commandBroker.Call(commandString);
            Assert.IsTrue(result);
            Assert.AreEqual(positional, ArgumentAssignmentTestCommand.LastPosition);
            Assert.AreEqual(named, ArgumentAssignmentTestCommand.LastNamed);
            Assert.AreEqual(flag, ArgumentAssignmentTestCommand.LastFlag);
        }

        [DataTestMethod]
        [DataRow("argumentAssignmentTest -flag true", DisplayName = "Flag can not be assigned a value")]
        [DataRow("argumentAssignmentTest -named 27.9f", DisplayName = "Wrong named value assigned")]
        [DataRow("argumentAssignmentTest \"string\"", DisplayName = "Wrong positional value assigned")]
        [DataRow("argumentAssignmentTest -noFlag", DisplayName = "Non-existing flag")]
        [DataRow("argumentAssignmentTest -noNamed noValue", DisplayName = "Non-existing named argument")]
        [DataRow("argumentAssignmentTest -flag -noFlag", DisplayName = "Non-existing flag")]
        [DataRow("argumentAssignmentTest -flag -noNamed noValue", DisplayName = "Non-existing named argument")]
        [DataRow("argumentAssignmentTest 0 1", DisplayName = "Non-existing positional")]
        public void Call_ArgumentAssignmentTest_Failure(string command)
        {
            var console = new Mock<IGameConsole>();
            console.SetupGet(c => c.Log).Returns(new GameConsoleLogData());
            var commandBroker = new CommandBroker(console.Object);
            commandBroker.Register<ArgumentAssignmentTestCommand>();
            ArgumentAssignmentTestCommand.Reset();
            CommandString.TryParse(command, out var commandString);
            var result = commandBroker.Call(commandString);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Call_EnumParameter_Assigned()
        {
            // Setup
            var console = new Mock<IGameConsole>();
            var commandBroker = new CommandBroker(console.Object);
            var command = new EnumTestCommand();
            commandBroker.Register(command);

            // Assert
            CommandString.TryParse("enumTestCommand One", out var commandString);
            Assert.IsTrue(commandBroker.Call(commandString));
            Assert.AreEqual(EnumTestCommand.TestEnum.One, command.Positional);

            CommandString.TryParse("enumTestCommand 2", out commandString);
            Assert.IsTrue(commandBroker.Call(commandString));
            Assert.AreEqual(EnumTestCommand.TestEnum.Two, command.Positional);
        }

        [ConsoleCommand("NoParameterlessConstructorTestCommand")]
        private class NoParameterlessConstructorTestCommand : IConsoleCommand
        {
            private readonly bool _nothingToSeeHere;
            public NoParameterlessConstructorTestCommand(bool nothingToSeeHere)
            {
                _nothingToSeeHere = nothingToSeeHere;
            }

            public void Call(IGameConsole console) => throw new NotImplementedException(_nothingToSeeHere.ToString());

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => throw new NotImplementedException();
        }

        [ConsoleCommand("NoInterfaceTestCommand")]
        private class NoInterfaceTestCommand
        {
        }

        private class NoAttributeTestCommand : IConsoleCommand
        {
            public void Call(IGameConsole console) => throw new NotImplementedException();

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => throw new NotImplementedException();
        }

        [ConsoleCommand("NoPositionalZeroTestCommand")]
        private class NoPositionalZeroTestCommand : IConsoleCommand
        {
            [ConsolePositional(1)]
            public int Pos0 { get; set; }

            public void Call(IGameConsole console) => throw new NotImplementedException();

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => throw new NotImplementedException();
        }

        [ConsoleCommand("PositionalGapTestCommand")]
        private class PositionalGapTestCommand : IConsoleCommand
        {
            [ConsolePositional(0)]
            public int Pos0 { get; set; }

            [ConsolePositional(2)]
            public int Pos1 { get; set; }

            public void Call(IGameConsole console) => throw new NotImplementedException();

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => throw new NotImplementedException();
        }

        [ConsoleCommand("RequiredAfterOptionalPositionalTestCommand")]
        private class RequiredAfterOptionalPositionalTestCommand : IConsoleCommand
        {
            [ConsolePositional(0, IsRequired = true)]
            public int Pos0 { get; set; }

            [ConsolePositional(1, IsRequired = false)]
            public int Pos1 { get; set; }

            [ConsolePositional(2, IsRequired = true)]
            public int Pos2 { get; set; }

            public void Call(IGameConsole console) => throw new NotImplementedException();

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => throw new NotImplementedException();
        }

        [ConsoleCommand("argumentAssignmentTest")]
        private class ArgumentAssignmentTestCommand : IConsoleCommand
        {
            [ConsolePositional(0)]
            public int Position { get; set; }

            [ConsoleArgument("named")]
            public int Named { get; set; }

            [ConsoleFlag("flag")]
            public bool Flag { get; set; }

            internal static int LastPosition;
            internal static int LastNamed;
            internal static bool LastFlag;

            internal static void Reset()
            {
                LastPosition = 0;
                LastNamed = 0;
                LastFlag = false;
            }

            public void Call(IGameConsole console)
            {
                LastPosition = Position;
                LastNamed = Named;
                LastFlag = Flag;
            }

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
        }

        [ConsoleCommand("argumentUsageTest")]
        private class ArgumentUsageTestCommand : IConsoleCommand
        {
            [ConsolePositional(0, IsRequired = true)]
            public string PositionArg1 { get; set; }

            [ConsolePositional(1, IsRequired = true)]
            public string PositionArg2 { get; set; }

            [ConsolePositional(2)]
            public string PositionArg3 { get; set; }

            [ConsoleArgument("named1", IsRequired = true)]
            public string Named1 { get; set; }

            [ConsoleArgument("named2")]
            public string Named2 { get; set; }

            [ConsoleFlag("flag1")]
            public string Flag1 { get; set; }

            [ConsoleFlag("flag2")]
            public string Flag2 { get; set; }

            public void Call(IGameConsole console)
            {
            }

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
        }

        [ConsoleCommand("typeTestCommand", Description = "testDescription")]
        private class TypeTestCommand : IConsoleCommand
        {
            internal static bool WasCalled = false;
            internal static string LastString = "";
            internal static int LastInt32 = 0;
            internal static float LastFloat = 0.0f;
            internal static bool LastBool = false;

            [ConsolePositional(0)]
            public string String { get; set; }

            [ConsolePositional(1)]
            public int Int32 { get; set; }

            [ConsolePositional(2)]
            public float Float { get; set; }

            [ConsolePositional(3)]
            public bool Bool { get; set; }

            internal static void Reset()
            {
                WasCalled = false;
                LastString = "";
                LastInt32 = 0;
                LastFloat = 0.0f;
                LastBool = false;
            }

            public void Call(IGameConsole console)
            {
                WasCalled = true;
                LastString = String;
                LastInt32 = Int32;
                LastFloat = Float;
                LastBool = Bool;
            }

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
        }

        [ConsoleCommand("defaultValueTestCommand")]
        private class DefaultValueTestCommand : IConsoleCommand
        {
            [ConsolePositional(0, IsRequired = false)]
            public bool NonRequiredPositional { get; set; }

            [ConsoleArgument("arg", IsRequired = false)]
            public bool NonRequiredArgument { get; set; }

            internal static void Reset()
            {
            }

            public void Call(IGameConsole console)
            {
            }

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
        }

        [ConsoleCommand("emptyTestCommand", Description = "emptyDescription")]
        private class EmptyTestCommand : IConsoleCommand
        {
            internal static bool WasCalled = false;

            internal static void Reset()
            {
                WasCalled = false;
            }

            public void Call(IGameConsole console)
            {
                WasCalled = true;
            }

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
        }

        [ConsoleCommand("instanceTestCommand", Description = "emptyDescription")]
        private class InstanceTestCommand : IConsoleCommand
        {
            public int WasCalledWith { get; set; }
            private readonly int _toCallWith;

            public InstanceTestCommand(int callWith)
            {
                _toCallWith = callWith;
            }

            public void Call(IGameConsole console)
            {
                WasCalledWith = _toCallWith;
            }

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();
        }

        [ConsoleCommand("enumTestCommand", Description = "emptyDescription")]
        private class EnumTestCommand : IConsoleCommand
        {
            [ConsolePositional(0, IsRequired = false)]
            public TestEnum Positional { get; set; }

            public EnumTestCommand()
            {
            }

            public void Call(IGameConsole console)
            {
            }

            public ICollection<string> GetPositionalSuggestions(IGameConsole console) => Array.Empty<string>();

            public enum TestEnum
            {
                One = 1,
                Two = 2,
            }
        }
    }
}
