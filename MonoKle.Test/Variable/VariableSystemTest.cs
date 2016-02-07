namespace MonoKle.Variable
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Attributes;
    using System;

    [TestClass]
    public class VariableSystemTest
    {
        private VariableSystem system;

        [TestMethod]
        public void Clear_Cleared()
        {
            system.SetValue("a", 1);
            system.SetValue("b", 2);
            system.SetValue("c", 3);
            system.Clear();
            Assert.AreEqual(0, system.Identifiers.Count);
            Assert.AreEqual(null, system.GetValue("a"));
            Assert.AreEqual(null, system.GetValue("b"));
            Assert.AreEqual(null, system.GetValue("c"));
        }

        [TestMethod]
        public void GetValue_BoundVariable_Called()
        {
            MockVariable b = new MockVariable(true);
            system.Bind(b, "a");
            system.GetValue("a");
            Assert.IsTrue(b.getCalled);
            Assert.IsFalse(b.setCalled);
        }

        [TestMethod]
        public void GetValue_Exists_Correct()
        {
            string id = "abc";
            int value = 17;
            system.SetValue(id, value);
            Assert.AreEqual(value, system.GetValue(id));
        }

        [TestMethod]
        public void GetValue_Nonexisting_Null()
        {
            system.SetValue("abcd", 8);
            Assert.AreEqual(null, system.GetValue("abc"));
        }

        [TestMethod]
        public void GetValue_TwoExists_Correct()
        {
            string id = "abc";
            string id2 = "def";
            int value = 17;
            int value2 = 7;
            system.SetValue(id, value);
            system.SetValue(id2, value2);
            Assert.AreEqual(value2, system.GetValue(id2));
        }

        [TestInitialize]
        public void Init()
        {
            this.system = new VariableSystem();
        }

        [TestMethod]
        public void NewSystem_NoVariables()
        {
            Assert.AreEqual(0, system.Identifiers.Count);
        }

        [TestMethod]
        public void Remove_Removed()
        {
            system.SetValue("a", 1);
            system.SetValue("b", 2);
            system.SetValue("c", 3);
            system.Remove("b");
            Assert.AreEqual(2, system.Identifiers.Count);
            Assert.AreEqual(1, system.GetValue("a"));
            Assert.AreEqual(null, system.GetValue("b"));
            Assert.AreEqual(3, system.GetValue("c"));
        }

        [TestMethod]
        public void SetValue_BoundVariable_Called()
        {
            MockVariable b = new MockVariable(true);
            system.Bind(b, "a");
            system.SetValue("a", 5);
            Assert.IsFalse(b.getCalled);
            Assert.IsTrue(b.setCalled);
        }

        [TestMethod]
        public void SetValue_BoundVariable_FalseReturn()
        {
            MockVariable b = new MockVariable(false);
            system.Bind(b, "a");
            Assert.IsFalse(system.SetValue("a", 5));
        }

        [TestMethod]
        public void SetValue_BoundVariable_MockNotUpdated()
        {
            MockVariable b = new MockVariable(false);
            system.SetValue("a", 5);
            system.Bind(b, "a", false);
            Assert.AreEqual(null, b.var);
            Assert.IsFalse(b.getCalled);
            Assert.IsFalse(b.setCalled);
        }

        [TestMethod]
        public void SetValue_BoundVariable_MockUpdated()
        {
            MockVariable b = new MockVariable(false);
            system.SetValue("a", 5);
            system.Bind(b, "a", true);
            Assert.AreEqual(5, b.var);
            Assert.IsFalse(b.getCalled);
            Assert.IsTrue(b.setCalled);
        }

        [TestMethod]
        public void SetValue_IdentifierCountIncremented()
        {
            int amount = 5;
            string s = "a";
            for (int i = 0; i < amount; i++)
            {
                s += s;
                system.SetValue(s, 7);
            }
            Assert.AreEqual(5, system.Identifiers.Count);
        }

        [TestMethod]
        public void BindProperties_CorrectlyBound()
        {
            BoundClass b = new BoundClass() { X = 1, Y = 2, Z = 3 };
            this.system.BindProperties(b);
            this.system.SetValue("z", 17);
            Assert.AreEqual(2, this.system.Identifiers.Count);
            Assert.AreEqual(b.X, 1);
            Assert.AreEqual(b.Z, 17);
            Assert.AreEqual(b.X, this.system.GetValue("x"));
            Assert.AreEqual(b.Z, this.system.GetValue("z"));
        }

        private class BoundClass
        {
            [PropertyVariableAttribute("x")]
            public int X { get; set; }

            public int Y { get; set; }

            [PropertyVariableAttribute("z")]
            public int Z { get; set; }
        }

        private class MockVariable : IVariable
        {
            public bool getCalled;
            public bool setCalled;
            public bool toReturnOnSet;
            public object var;

            public MockVariable(bool toReturnOnSet)
            {
                this.toReturnOnSet = toReturnOnSet;
            }

            public Type Type
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool CanSet()
            {
                return true;
            }

            public object GetValue()
            {
                this.getCalled = true;
                return this.var;
            }

            public bool SetValue(object value)
            {
                this.setCalled = true;
                this.var = value;
                return this.toReturnOnSet;
            }
        }
    }
}