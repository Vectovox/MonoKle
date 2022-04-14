using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace MonoKle.Configuration.Tests
{
    [TestClass]
    public class CVarSystemTests
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
            Assert.AreEqual(1, b.X);
            Assert.AreEqual(17, b.Z);
            Assert.AreEqual(b.X, _cvarSystem.GetValue("x"));
            Assert.AreEqual(b.Z, _cvarSystem.GetValue("z"));
        }

        [TestMethod]
        public void BindProperties_AssignOld_CorrectlyBound()
        {
            var original = new BoundType() { X = 1, Y = 2, Z = 3 };
            _cvarSystem.BindProperties(original);
            var newInstance = new BoundType() { X = 0, Y = 0, Z = 0 };
            _cvarSystem.BindProperties(newInstance, true);
            // Unaffected old instance
            Assert.AreEqual(1, original.X);
            Assert.AreEqual(2, original.Y);
            Assert.AreEqual(3, original.Z);
            // Correct amount
            Assert.AreEqual(2, _cvarSystem.Identifiers.Count);
            // New instance has taken old values...
            Assert.AreEqual(original.X, newInstance.X);
            Assert.AreEqual(original.Z, newInstance.Z);
            // ...but preserved unbound value
            Assert.AreEqual(0, newInstance.Y);
            // Identifier values correct
            Assert.AreEqual(newInstance.X, _cvarSystem.GetValue("x"));
            Assert.AreEqual(newInstance.Z, _cvarSystem.GetValue("z"));
        }

        [TestMethod]
        public void BindProperties_PrivateProperty_Assigned()
        {
            var sut = new PrivatePropertyType();
            _cvarSystem.BindProperties(sut);
            Assert.AreEqual(1, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(0, sut.PrivateValue);
            _cvarSystem.SetValue("private", 99);
            Assert.AreEqual(99, sut.PrivateValue);
        }

        [TestMethod]
        public void BindProperties_Recursive_Assigned()
        {
            var sut = new RecursiveBoundType
            {
                Tested = new BoundType()
                {
                    X = 1,
                    Y = 2,
                    Z = 3,
                }
            };
            _cvarSystem.BindProperties(sut, false, true);
            _cvarSystem.SetValue("z", 17);
            Assert.AreEqual(2, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(1, sut.Tested.X);
            Assert.AreEqual(17, sut.Tested.Z);
            Assert.AreEqual(sut.Tested.X, _cvarSystem.GetValue("x"));
            Assert.AreEqual(sut.Tested.Z, _cvarSystem.GetValue("z"));
        }

        [TestMethod]
        public void BindProperties_Recursive_Cyclic_NoException()
        {
            var list = new LinkedList<int>();
            _cvarSystem.BindProperties(list, false, true);
            Assert.AreEqual(0, _cvarSystem.Identifiers.Count);
        }

        [TestMethod]
        public void BindProperties_StaticProperty_Assigned()
        {
            var sut = new StaticPropertyType();
            _cvarSystem.BindProperties(sut);
            Assert.AreEqual(1, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(0, StaticPropertyType.Static);
            _cvarSystem.SetValue("static", 78);
            Assert.AreEqual(78, StaticPropertyType.Static);
        }

        [TestMethod]
        public void BindProperties_StaticClass_Assigned()
        {
            _cvarSystem.BindProperties(typeof(StaticClassType));
            Assert.AreEqual(1, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(0, StaticClassType.Static);
            _cvarSystem.SetValue("static_class", 78);
            Assert.AreEqual(78, StaticClassType.Static);
        }

        [TestMethod]
        public void BindProperties_StaticClass_Recursive_Assigned()
        {
            _cvarSystem.BindProperties(typeof(StaticClassType), false, true);
            Assert.AreEqual(3, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(0, StaticClassType.Recursive.X);
            _cvarSystem.SetValue("x", 78);
            Assert.AreEqual(78, StaticClassType.Recursive.X);
        }

        [TestMethod]
        public void Remove_Removed()
        {
            _cvarSystem.SetValue("a", 1);
            _cvarSystem.SetValue("b", 2);
            _cvarSystem.SetValue("c", 3);
            _cvarSystem.Remove("b");
            Assert.AreEqual(2, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(1, _cvarSystem.GetValue("a"));
            Assert.AreEqual(3, _cvarSystem.GetValue("c"));
            Assert.IsFalse(_cvarSystem.Contains("b"));
        }

        [TestMethod]
        public void UnbindProperty_UnboundWithNoSideEffects()
        {
            // Setup
            _cvarSystem.SetValue("a", 1);
            var b = new BoundType() { X = 2, Y = 3, Z = 4 };
            _cvarSystem.BindProperties(b);

            // Test
            var unbound = _cvarSystem.UnbindProperties(b);

            // Assert
            Assert.AreEqual(2, unbound);
            Assert.AreEqual(3, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(1, _cvarSystem.GetValue("a"));
            Assert.AreEqual(2, _cvarSystem.GetValue("x"));
            Assert.AreEqual(4, _cvarSystem.GetValue("z"));
            Assert.AreEqual(2, b.X);
            Assert.AreEqual(3, b.Y);
            Assert.AreEqual(4, b.Z);
        }

        [TestMethod]
        public void UnbindProperty_PropertyNotBound()
        {
            // Setup
            var b = new BoundType() { X = 2, Y = 3, Z = 4 };
            _cvarSystem.BindProperties(b);

            // Test
            _cvarSystem.UnbindProperties(b);
            _cvarSystem.SetValue("x", 1);
            _cvarSystem.SetValue("z", 1);

            // Assert
            Assert.AreEqual(2, b.X);
            Assert.AreEqual(3, b.Y);
            Assert.AreEqual(4, b.Z);
            Assert.AreEqual(1, _cvarSystem.GetValue("x"));
            Assert.AreEqual(1, _cvarSystem.GetValue("z"));
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
            Assert.AreEqual(2, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(1, _cvarSystem.GetValue("a"));
            Assert.IsTrue(_cvarSystem.Contains("static_class"));
        }

        [TestMethod]
        public void UnbindProperty_StaticClass_PropertyNotBound()
        {
            // Setup
            _cvarSystem.BindProperties(typeof(StaticClassType));
            StaticClassType.Static = 0;

            // Test
            _cvarSystem.UnbindProperties(typeof(StaticClassType));
            _cvarSystem.SetValue("static_class", 127);

            // Assert
            Assert.AreEqual(0, StaticClassType.Static);
            Assert.AreEqual(127, _cvarSystem.GetValue("static_class"));
        }

        [TestMethod]
        public void UnbindProperty_Recursively_RemovedWithoutSideEffects()
        {
            // Setup
            _cvarSystem.SetValue("a", 1);
            var b = new RecursiveBoundType { Tested = new BoundType { X = 2, Y = 3, Z = 4 } };
            _cvarSystem.BindProperties(b, false, true);

            // Test
            var unbound = _cvarSystem.UnbindProperties(b, true);

            // Assert
            Assert.AreEqual(2, unbound);
            Assert.AreEqual(3, _cvarSystem.Identifiers.Count);
            Assert.AreEqual(1, _cvarSystem.GetValue("a"));
            Assert.AreEqual(2, _cvarSystem.GetValue("x"));
            Assert.AreEqual(4, _cvarSystem.GetValue("z"));
            Assert.AreEqual(2, b.Tested.X);
            Assert.AreEqual(3, b.Tested.Y);
            Assert.AreEqual(4, b.Tested.Z);
        }

        [TestMethod]
        public void UnbindProperty_Recursively_NotBound()
        {
            // Setup
            var b = new RecursiveBoundType { Tested = new BoundType { X = 2, Y = 3, Z = 4 } };
            _cvarSystem.BindProperties(b, false, true);

            // Test
            _cvarSystem.UnbindProperties(b, true);
            _cvarSystem.SetValue("x", 1);
            _cvarSystem.SetValue("z", 1);

            // Assert
            Assert.AreEqual(2, b.Tested.X);
            Assert.AreEqual(3, b.Tested.Y);
            Assert.AreEqual(4, b.Tested.Z);
            Assert.AreEqual(1, _cvarSystem.GetValue("x"));
            Assert.AreEqual(1, _cvarSystem.GetValue("z"));
        }

        [TestMethod]
        public void SetValue_IntegerFromString_Converted()
        {
            // Setup
            var bound = new StringConversionType();
            _cvarSystem.BindProperties(bound);

            // Test
            var result = _cvarSystem.SetValue("integer", "8");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(8, bound.Integer);
        }

        [TestMethod]
        public void SetValue_FloatFromString_Converted()
        {
            // Setup
            var bound = new StringConversionType();
            _cvarSystem.BindProperties(bound);

            // Test
            var result = _cvarSystem.SetValue("float", "8.98");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(8.98f, bound.Float);
        }

        [TestMethod]
        public void SetValue_FloatFromString_CommaSeparatedLocale_ConvertedWithInvariantCulture()
        {
            // Setup
            var bound = new StringConversionType();
            _cvarSystem.BindProperties(bound);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

            // Test
            var result = _cvarSystem.SetValue("float", "8.98");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(8.98f, bound.Float);
        }

        [TestMethod]
        public void SetValue_StringFromString_Set()
        {
            // Setup
            var bound = new StringConversionType();
            _cvarSystem.BindProperties(bound);
            const string toTest = "My name is ransom";

            // Test

            var result = _cvarSystem.SetValue("string", toTest);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(toTest, bound.String);
        }

        [TestMethod]
        public void SetValue_MPoint2FromString_Converted()
        {
            // Setup
            var bound = new StringConversionType();
            _cvarSystem.BindProperties(bound);
            var mpoint = new MPoint2(8, -11);

            // Test
            var result = _cvarSystem.SetValue("mpoint2", mpoint.ToString());

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(mpoint, bound.MPoint2);
        }

        private class StringConversionType
        {
            [CVar("integer")]
            public int Integer { get; set; }

            [CVar("float")]
            public float Float { get; set; }

            [CVar("string")]
            public string String { get; set; }

            [CVar("mpoint2")]
            public MPoint2 MPoint2 { get; set; }
        }

        private class RecursiveBoundType
        {
            public BoundType Tested { get; set; }
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

            public static BoundType Recursive { get; set; } = new BoundType();
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