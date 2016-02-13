#if UNITTESTS && UNITY_5
namespace Newtonsoft.Json.UnitTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using UnityEngine;

    [TestClass]
    public class Vector4ConverterTests
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Serialize()
        {
            try
            {
                var vector = new Vector4(1.25f, 2.5f, 3.75f, 0.25f);
                var data = JsonConvert.SerializeObject(vector);
                Assert.AreEqual("{\"x\":1.25,\"y\":2.5,\"z\":3.75,\"w\":0.25}", data);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void SerializeVectorContainer()
        {
            try
            {
                var obj = new Vector4Container();
                obj.VectorField = new Vector4(1.25f, 2.5f, 3.75f, 0.25f);
                obj.VectorProperty = new Vector4(2.25f, 3.5f, 4.25f, 0.5f);
                var data = JsonConvert.SerializeObject(obj);
                Assert.AreEqual("{\"VectorField\":{\"x\":1.25,\"y\":2.5,\"z\":3.75,\"w\":0.25},\"VectorProperty\":{\"x\":2.25,\"y\":3.5,\"z\":4.25,\"w\":0.5}}", data);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void DeserializeVectorContainer()
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<Vector4Container>("{\"VectorField\":{\"x\":1.25,\"y\":2.5,\"z\":3.75,\"w\":0.25},\"VectorProperty\":{\"x\":2.25,\"y\":3.5,\"z\":4.25,\"w\":0.5}}");
                Assert.AreEqual(1.25f, obj.VectorField.x);
                Assert.AreEqual(2.5f, obj.VectorField.y);
                Assert.AreEqual(3.75f, obj.VectorField.z);
                Assert.AreEqual(0.25f, obj.VectorField.w);
                Assert.AreEqual(2.25f, obj.VectorProperty.x);
                Assert.AreEqual(3.5f, obj.VectorProperty.y);
                Assert.AreEqual(4.25f, obj.VectorProperty.z);
                Assert.AreEqual(0.5f, obj.VectorProperty.w);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        private class Vector4Container
        {
            public Vector4 VectorProperty { get; set; }
            public Vector4 VectorField;
        }

        [TestMethod]
        public void Deserialize()
        {
            try
            {
                var vector = JsonConvert.DeserializeObject<Vector4>("{\"x\":1.25,\"y\":2.5,\"z\":3.75,\"w\":0.25}");
                Assert.AreEqual(1.25f, vector.x);
                Assert.AreEqual(2.5f, vector.y);
                Assert.AreEqual(3.75f, vector.z);
                Assert.AreEqual(0.25f, vector.w);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
} 
#endif