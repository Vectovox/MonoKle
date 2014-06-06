namespace MonoKle.IO
{
    using System;
    using System.IO;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StreamStructReaderWriterTest
    {
        private const int RANDOM_STRING_MAXLENGTH = 30;
        private const int RANDOM_STRING_MINLENGTH = 0;
        private const int RANDOM_TESTS_AMOUNT = 100;

        Random r = new Random();

        [TestMethod]
        public void TestBasicStruct()
        {
            for(int i = 0; i < RANDOM_TESTS_AMOUNT; i++)
            {
                MemoryStream ws = new MemoryStream();
                BasicStruct initialStruct = new BasicStruct();
                initialStruct.boolValue = r.Next(0, 2) == 0;
                initialStruct.charValue = (char)r.Next(65, 90);
                initialStruct.doubleFloatValue = r.Next(int.MinValue, int.MaxValue);
                initialStruct.floatValue = r.Next(short.MinValue, short.MaxValue);
                initialStruct.intValue = r.Next(int.MinValue, int.MaxValue);
                initialStruct.longValue = r.Next(int.MinValue, int.MaxValue);
                initialStruct.shortValue = (short)r.Next(short.MinValue, short.MaxValue);

                StringBuilder sb = new StringBuilder();
                int stringLength = r.Next(RANDOM_STRING_MINLENGTH, RANDOM_STRING_MAXLENGTH+1);
                for(int j = 0; j <= stringLength; j++)
                {
                    sb.Append((char)r.Next(65, 90));//char.MinValue, char.MaxValue));
                }
                initialStruct.stringValue = sb.ToString();

                StreamStructWriter<BasicStruct> writer = new StreamStructWriter<BasicStruct>(ws);
                writer.WriteStruct(initialStruct);
                writer.Close();

                MemoryStream rs = new MemoryStream(ws.GetBuffer());
                StreamStructReader<BasicStruct> reader = new StreamStructReader<BasicStruct>(rs);

                Assert.IsTrue(reader.CanGetStruct());
                BasicStruct readStruct = reader.GetNextStruct();
                reader.Close();
                Assert.AreEqual(initialStruct.boolValue, readStruct.boolValue);
                Assert.AreEqual(initialStruct.charValue, readStruct.charValue);
                Assert.AreEqual(initialStruct.doubleFloatValue, readStruct.doubleFloatValue);
                Assert.AreEqual(initialStruct.floatValue, readStruct.floatValue);
                Assert.AreEqual(initialStruct.intValue, readStruct.intValue);
                Assert.AreEqual(initialStruct.longValue, readStruct.longValue);
                Assert.AreEqual(initialStruct.shortValue, readStruct.shortValue);
                Assert.AreEqual(initialStruct.stringValue, readStruct.stringValue);
                Assert.IsFalse(reader.CanGetStruct());

                reader.Dispose();
                writer.Dispose();
            }
        }

        [TestMethod]
        public void TestEmptyStruct()
        {
            MemoryStream ws = new MemoryStream();
            EmptyStruct initialStruct = new EmptyStruct();
            StreamStructWriter<EmptyStruct> writer = new StreamStructWriter<EmptyStruct>(ws);
            writer.WriteStruct(initialStruct);
            writer.Close();

            MemoryStream rs = new MemoryStream(ws.GetBuffer());
            StreamStructReader<EmptyStruct> reader = new StreamStructReader<EmptyStruct>(rs);

            Assert.IsTrue(reader.CanGetStruct());
            EmptyStruct readStruct = reader.GetNextStruct();
            reader.Close();
            Assert.AreEqual(initialStruct, readStruct);
            Assert.IsFalse(reader.CanGetStruct());

            reader.Dispose();
            writer.Dispose();
        }

        [TestMethod]
        public void TestMultipleStructs()
        {
            MemoryStream ws = new MemoryStream();
            BasicStruct initialStruct = new BasicStruct();
            initialStruct.intValue = 15;
            initialStruct.stringValue = "One";
            BasicStruct initialStruct2 = new BasicStruct();
            initialStruct2.intValue = 3;
            initialStruct2.stringValue = "Two";
            StreamStructWriter<BasicStruct> writer = new StreamStructWriter<BasicStruct>(ws);
            writer.WriteStruct(initialStruct);
            writer.WriteStruct(initialStruct2);
            writer.Close();

            MemoryStream rs = new MemoryStream(ws.GetBuffer());
            StreamStructReader<BasicStruct> reader = new StreamStructReader<BasicStruct>(rs);

            Assert.IsTrue(reader.CanGetStruct());
            BasicStruct readStruct = reader.GetNextStruct();
            Assert.IsTrue(reader.CanGetStruct());
            BasicStruct readStruct2 = reader.GetNextStruct();
            reader.Close();
            Assert.AreEqual(initialStruct, readStruct);
            Assert.AreEqual(initialStruct2, readStruct2);
            Assert.IsFalse(reader.CanGetStruct());

            reader.Dispose();
            writer.Dispose();
        }

        [TestMethod]
        public void TestNestedStruct()
        {
            MemoryStream ws = new MemoryStream();
            StructStructStruct initialStruct = new StructStructStruct();
            initialStruct.innerStruct = new StructStruct();
            initialStruct.innerStruct.emptyStruct = new EmptyStruct();
            initialStruct.innerStruct.innerStruct.intValue = 17;
            initialStruct.innerStruct.innerStruct.boolValue = false;
            initialStruct.innerStruct.innerStruct.charValue = 'g';
            initialStruct.innerStruct.innerStruct.doubleFloatValue = 0.1;
            initialStruct.innerStruct.innerStruct.floatValue = 111.5f;
            initialStruct.innerStruct.innerStruct.longValue = 1234;
            initialStruct.innerStruct.innerStruct.shortValue = 192;
            initialStruct.innerStruct.innerStruct.stringValue = "banana";
            StreamStructWriter<StructStructStruct> writer = new StreamStructWriter<StructStructStruct>(ws);
            writer.WriteStruct(initialStruct);
            writer.Close();

            MemoryStream rs = new MemoryStream(ws.GetBuffer());
            StreamStructReader<StructStructStruct> reader = new StreamStructReader<StructStructStruct>(rs);

            Assert.IsTrue(reader.CanGetStruct());
            StructStructStruct readStruct = reader.GetNextStruct();
            reader.Close();
            Assert.AreEqual(initialStruct, readStruct);
            Assert.IsFalse(reader.CanGetStruct());

            reader.Dispose();
            writer.Dispose();
        }

        [TestMethod]
        public void TestPrivateStruct()
        {
            MemoryStream ws = new MemoryStream();
            PrivateStruct initialStruct = new PrivateStruct(7);

            StreamStructWriter<PrivateStruct> writer = new StreamStructWriter<PrivateStruct>(ws);
            writer.WriteStruct(initialStruct);
            writer.Close();

            MemoryStream rs = new MemoryStream(ws.GetBuffer());
            StreamStructReader<PrivateStruct> reader = new StreamStructReader<PrivateStruct>(rs);

            Assert.IsTrue(reader.CanGetStruct());
            PrivateStruct readStruct = reader.GetNextStruct();
            reader.Close();
            Assert.AreEqual(initialStruct, readStruct);
            Assert.IsFalse(reader.CanGetStruct());

            reader.Dispose();
            writer.Dispose();
        }

        [TestMethod]
        public void TestPropertyStruct()
        {
            MemoryStream ws = new MemoryStream();
            PropertyStruct initialStruct = new PropertyStruct();
            initialStruct.MyProperty = new BasicStruct();

            BasicStruct basicStruct = new BasicStruct();
            basicStruct.boolValue = true;
            basicStruct.charValue = 'p';
            basicStruct.doubleFloatValue = 12.5;
            basicStruct.floatValue = 3.1f;
            basicStruct.intValue = 19;
            basicStruct.longValue = 1293123;
            basicStruct.shortValue = 213;
            basicStruct.stringValue = "applepie";
            initialStruct.MyProperty = basicStruct;

            StreamStructWriter<PropertyStruct> writer = new StreamStructWriter<PropertyStruct>(ws);
            writer.WriteStruct(initialStruct);
            writer.Close();

            MemoryStream rs = new MemoryStream(ws.GetBuffer());
            StreamStructReader<PropertyStruct> reader = new StreamStructReader<PropertyStruct>(rs);

            Assert.IsTrue(reader.CanGetStruct());
            PropertyStruct readStruct = reader.GetNextStruct();
            reader.Close();
            Assert.AreEqual(initialStruct, readStruct);
            Assert.IsFalse(reader.CanGetStruct());

            reader.Dispose();
            writer.Dispose();
        }

        private struct BasicStruct
        {
            public bool boolValue;
            public char charValue;
            public double doubleFloatValue;
            public float floatValue;
            public int intValue;
            public long longValue;
            public short shortValue;
            public string stringValue;
        }

        private struct EmptyStruct
        {
        }

        private struct PrivateStruct
        {
            private int value;

            public PrivateStruct(int integer)
                : this()
            {
                this.value = integer;
                MyInteger = integer * 2;
            }

            public int MyInteger
            {
                get; private set;
            }
        }

        private struct PropertyStruct
        {
            public BasicStruct MyProperty
            {
                get; set;
            }
        }

        private struct StructStruct
        {
            public EmptyStruct emptyStruct;
            public BasicStruct innerStruct;
        }

        private struct StructStructStruct
        {
            public StructStruct innerStruct;
        }
    }
}