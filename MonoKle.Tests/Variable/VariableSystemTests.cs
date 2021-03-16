using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MonoKle.Configuration.Tests
{
    [TestClass]
    public class VariableSystemTests
    {
        private CVarSystem _cvarSystem;

        [TestInitialize]
        public void Init() => _cvarSystem = new CVarSystem(new Logging.Logger());

        [TestMethod]
        public void Clear_Cleared()
        {
            _cvarSystem.SetValue("a", 1);
            _cvarSystem.SetValue("b", 2);
            _cvarSystem.SetValue("c", 3);
            _cvarSystem.Clear();
            Assert.AreEqual(0, _cvarSystem.Identifiers.Count);
            Assert.IsFalse(_cvarSystem.Contains("a"));
            Assert.IsFalse(_cvarSystem.Contains("b"));
            Assert.IsFalse(_cvarSystem.Contains("c"));
        }

        [TestMethod]
        public void GetValue_BoundVariable_Called()
        {
            var b = new VariableMock(true);
            _cvarSystem.Bind(b, "a");
            _cvarSystem.GetValue("a");
            Assert.IsTrue(b._getCalled);
            Assert.IsFalse(b._setCalled);
        }

        [TestMethod]
        public void GetValue_Exists_Correct()
        {
            string id = "abc";
            int value = 17;
            _cvarSystem.SetValue(id, value);
            Assert.AreEqual(value, _cvarSystem.GetValue(id));
        }

        [TestMethod]
        public void GetValue_Nonexisting_Exception() =>
            Assert.ThrowsException<ArgumentException>(() => _cvarSystem.GetValue("abc"));

        [TestMethod]
        public void GetValue_TwoExists_Correct()
        {
            string id = "abc";
            string id2 = "def";
            int value = 17;
            int value2 = 7;
            _cvarSystem.SetValue(id, value);
            _cvarSystem.SetValue(id2, value2);
            Assert.AreEqual(value2, _cvarSystem.GetValue(id2));
        }

        [TestMethod]
        public void NewSystem_NoVariables() => Assert.AreEqual(0, _cvarSystem.Identifiers.Count);

        [TestMethod]
        public void SetValue_BoundVariable_Called()
        {
            var b = new VariableMock(true);
            _cvarSystem.Bind(b, "a");
            _cvarSystem.SetValue("a", 5);
            Assert.IsFalse(b._getCalled);
            Assert.IsTrue(b._setCalled);
        }

        [TestMethod]
        public void SetValue_BoundVariable_FalseReturn()
        {
            var b = new VariableMock(false);
            _cvarSystem.Bind(b, "a");
            Assert.IsFalse(_cvarSystem.SetValue("a", 5));
        }

        [TestMethod]
        public void SetValue_BoundVariable_TrueReturn()
        {
            var b = new VariableMock(true);
            _cvarSystem.Bind(b, "a");
            Assert.IsTrue(_cvarSystem.SetValue("a", 5));
        }

        [TestMethod]
        public void SetValue_BoundVariable_MockNotUpdated()
        {
            var b = new VariableMock(false);
            _cvarSystem.SetValue("a", 5);
            _cvarSystem.Bind(b, "a", false);
            Assert.AreEqual(null, b._var);
            Assert.IsFalse(b._getCalled);
            Assert.IsFalse(b._setCalled);
        }

        [TestMethod]
        public void SetValue_BoundVariable_MockUpdated()
        {
            var b = new VariableMock(false);
            _cvarSystem.SetValue("a", 5);
            _cvarSystem.Bind(b, "a", true);
            Assert.AreEqual(5, b._var);
            Assert.IsFalse(b._getCalled);
            Assert.IsTrue(b._setCalled);
        }

        [TestMethod]
        public void SetValue_IdentifierCountIncremented()
        {
            int amount = 5;
            string s = "a";
            for (int i = 0; i < amount; i++)
            {
                s += s;
                _cvarSystem.SetValue(s, 7);
            }
            Assert.AreEqual(5, _cvarSystem.Identifiers.Count);
        }

        [TestMethod]
        public void BindProperties_CorrectlyBound()
        {
            var b = new BoundType() { X = 1, Y = 2, Z = 3 };
            _cvarSystem.BindProperties(b);
            _cvarSystem.SetValue("z", 17);
            Assert.AreEqual(2, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(b.X, 1);
            Assert.AreEqual(b.Z, 17);
            Assert.AreEqual(b.X, _cvarSystem.GetValue("x"));
            Assert.AreEqual(b.Z, _cvarSystem.GetValue("z"));
        }

        [TestMethod]
        public void BindProperties_PrivateProperty_Assigned()
        {
            var sut = new PrivatePropertyType();
            _cvarSystem.BindProperties(sut);
            Assert.AreEqual(1, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(sut.PrivateValue, 0);
            _cvarSystem.SetValue("private", 99);
            Assert.AreEqual(sut.PrivateValue, 99);
        }

        [TestMethod]
        public void BindProperties_StaticProperty_Assigned()
        {
            var sut = new StaticPropertyType();
            _cvarSystem.BindProperties(sut);
            Assert.AreEqual(1, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(StaticPropertyType.Static, 0);
            _cvarSystem.SetValue("static", 78);
            Assert.AreEqual(StaticPropertyType.Static, 78);
        }

        [TestMethod]
        public void BindProperties_StaticClass_Assigned()
        {
            _cvarSystem.BindProperties(typeof(StaticClassType));
            Assert.AreEqual(1, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(StaticClassType.Static, 0);
            _cvarSystem.SetValue("static_class", 78);
            Assert.AreEqual(StaticClassType.Static, 78);
        }

        [TestMethod]
        public void Unbind_Removed()
        {
            _cvarSystem.SetValue("a", 1);
            _cvarSystem.SetValue("b", 2);
            _cvarSystem.SetValue("c", 3);
            _cvarSystem.Unbind("b");
            Assert.AreEqual(2, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(1, _cvarSystem.GetValue("a"));
            Assert.AreEqual(3, _cvarSystem.GetValue("c"));
            Assert.IsFalse(_cvarSystem.Contains("b"));
        }

        [TestMethod]
        public void UnbindProperty_RemovedWithNoSideEffects()
        {
            // Setup
            _cvarSystem.SetValue("a", 1);
            var b = new BoundType() { X = 2, Y = 3, Z = 4 };
            _cvarSystem.BindProperties(b);

            // Test
            var unbound = _cvarSystem.UnbindProperties(b);

            // Assert
            Assert.AreEqual(2, unbound);
            Assert.AreEqual(1, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(1, _cvarSystem.GetValue("a"));
            Assert.IsFalse(_cvarSystem.Contains("x"));
            Assert.IsFalse(_cvarSystem.Contains("z"));
        }

        [TestMethod]
        public void UnbindProperty_StaticClass_RemovedWithNoSideEffects()
        {
            // Setup
            _cvarSystem.SetValue("a", 1);
            _cvarSystem.BindProperties(typeof(StaticClassType));

            // Test
            var unbound = _cvarSystem.UnbindProperties(typeof(StaticClassType));

            // Assert
            Assert.AreEqual(1, unbound);
            Assert.AreEqual(1, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(1, _cvarSystem.GetValue("a"));
            Assert.IsFalse(_cvarSystem.Contains("static_class"));
        }

        private class BoundType
        {
            [CVar("x")]
            public int X { get; set; }

            public int Y { get; set; }

            [CVar("z")]
            public int Z { get; set; }
        }

        private class PrivatePropertyType
        {
            [CVar("private")]
            private int Private { get; set; }
            public int PrivateValue => Private;
        }

        private class StaticPropertyType
        {
            [CVar("static")]
            public static int Static { get; set; }

            public static int NotBound { get; set; }
        }

        private static class StaticClassType
        {
            [CVar("static_class")]
            public static int Static { get; set; }

            public static int NotBound { get; set; }
        }

        /// <summary>
        /// Used to control variable behavior form the tests.
        /// </summary>
        private class VariableMock : ICVar
        {
            public bool _getCalled;
            public bool _setCalled;
            public bool _toReturnOnSet;
            public object _var;

            public VariableMock(bool toReturnOnSet)
            {
                _toReturnOnSet = toReturnOnSet;
            }

            public Type Type => throw new NotImplementedException();

            public bool CanSet() => true;

            public object GetValue()
            {
                _getCalled = true;
                return _var;
            }

            public bool SetValue(object value)
            {
                _setCalled = true;
                _var = value;
                return _toReturnOnSet;
            }
        }
    }
}