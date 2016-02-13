#if UNITTESTS && UNITY_5
namespace Newtonsoft.Json.UnitTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using UnityEngine;

    [TestClass]
    public class Vector3ConverterTests
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
                var vector = new Vector3(1.25f, 2.5f, 3.75f);
                var data = JsonConvert.SerializeObject(vector);
                Assert.AreEqual("{\"x\":1.25,\"y\":2.5,\"z\":3.75}", data);
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
                var obj = new Vector3Container();
                obj.VectorField = new Vector3(1.25f, 2.5f, 3.75f);
                obj.VectorProperty = new Vector3(2.25f, 3.5f, 4.25f);
                var data = JsonConvert.SerializeObject(obj);
                Assert.AreEqual("{\"VectorField\":{\"x\":1.25,\"y\":2.5,\"z\":3.75},\"VectorProperty\":{\"x\":2.25,\"y\":3.5,\"z\":4.25}}", data);
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
                var obj = JsonConvert.DeserializeObject<Vector3Container>("{\"VectorField\":{\"x\":1.25,\"y\":2.5,\"z\":3.75},\"VectorProperty\":{\"x\":2.25,\"y\":3.5,\"z\":4.25}}");
                Assert.AreEqual(1.25f, obj.VectorField.x);
                Assert.AreEqual(2.5f, obj.VectorField.y);
                Assert.AreEqual(3.75f, obj.VectorField.z);
                Assert.AreEqual(2.25f, obj.VectorProperty.x);
                Assert.AreEqual(3.5f, obj.VectorProperty.y);
                Assert.AreEqual(4.25f, obj.VectorProperty.z);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        private class Vector3Container
        {
            public Vector3 VectorProperty { get; set; }
            public Vector3 VectorField;
        }

        [TestMethod]
        public void Deserialize()
        {
            try
            {
                var vector = JsonConvert.DeserializeObject<Vector3>("{\"x\":1.25,\"y\":2.5,\"z\":3.75}");
                Assert.AreEqual(1.25f, vector.x);
                Assert.AreEqual(2.5f, vector.y);
                Assert.AreEqual(3.75f, vector.z);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
} 
#endif