#if UNITTESTS && UNITY_5
namespace Newtonsoft.Json.UnitTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using UnityEngine;

    [TestClass]
    public class ColorConverterTests
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
                var vector = new Color(1.25f, 2.5f, 3.75f, 0.25f);
                var data = JsonConvert.SerializeObject(vector);
                Assert.AreEqual("{\"r\":1.25,\"g\":2.5,\"b\":3.75,\"a\":0.25}", data);
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
                var obj = new ColorContainer();
                obj.VectorField = new Color(1.25f, 2.5f, 3.75f, 0.25f);
                obj.VectorProperty = new Color(2.25f, 3.5f, 4.25f, 0.5f);
                var data = JsonConvert.SerializeObject(obj);
                Assert.AreEqual("{\"VectorField\":{\"r\":1.25,\"g\":2.5,\"b\":3.75,\"a\":0.25},\"VectorProperty\":{\"r\":2.25,\"g\":3.5,\"b\":4.25,\"a\":0.5}}", data);
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
                var obj = JsonConvert.DeserializeObject<ColorContainer>("{\"VectorField\":{\"r\":1.25,\"g\":2.5,\"b\":3.75,\"a\":0.25},\"VectorProperty\":{\"r\":2.25,\"g\":3.5,\"b\":4.25,\"a\":0.5}}");
                Assert.AreEqual(1.25f, obj.VectorField.r);
                Assert.AreEqual(2.5f, obj.VectorField.g);
                Assert.AreEqual(3.75f, obj.VectorField.b);
                Assert.AreEqual(0.25f, obj.VectorField.a);
                Assert.AreEqual(2.25f, obj.VectorProperty.r);
                Assert.AreEqual(3.5f, obj.VectorProperty.g);
                Assert.AreEqual(4.25f, obj.VectorProperty.b);
                Assert.AreEqual(0.5f, obj.VectorProperty.a);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        private class ColorContainer
        {
            public Color VectorProperty { get; set; }
            public Color VectorField;
        }

        [TestMethod]
        public void Deserialize()
        {
            try
            {
                var vector = JsonConvert.DeserializeObject<Color>("{\"r\":1.25,\"g\":2.5,\"b\":3.75,\"a\":0.25}");
                Assert.AreEqual(1.25f, vector.r);
                Assert.AreEqual(2.5f, vector.g);
                Assert.AreEqual(3.75f, vector.b);
                Assert.AreEqual(0.25f, vector.a);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
}
#endif