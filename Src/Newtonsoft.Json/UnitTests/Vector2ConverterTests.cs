#if UNITTESTS && UNITY_5
namespace Newtonsoft.Json.UnitTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using UnityEngine;

    [TestClass]
    public class Vector2ConverterTests
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
                var vector = new Vector2(1.25f, 2.5f);
                var data = JsonConvert.SerializeObject(vector);
                Assert.AreEqual("{\"x\":1.25,\"y\":2.5}", data);
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
                var obj = new Vector2Container();
                obj.Vector2Field = new Vector2(1.25f, 2.5f);
                obj.Vector2Property = new Vector2(2.25f, 3.5f);
                var data = JsonConvert.SerializeObject(obj);
                Assert.AreEqual("{\"Vector2Field\":{\"x\":1.25,\"y\":2.5},\"Vector2Property\":{\"x\":2.25,\"y\":3.5}}", data);
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
                var obj = JsonConvert.DeserializeObject<Vector2Container>("{\"Vector2Field\":{\"x\":1.25,\"y\":2.5},\"Vector2Property\":{\"x\":2.25,\"y\":3.5}}");
                Assert.AreEqual(1.25f, obj.Vector2Field.x);
                Assert.AreEqual(2.5f, obj.Vector2Field.y);
                Assert.AreEqual(2.25f, obj.Vector2Property.x);
                Assert.AreEqual(3.5f, obj.Vector2Property.y);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        private class Vector2Container
        {
            public Vector2 Vector2Property { get; set; }
            public Vector2 Vector2Field;
        }

        [TestMethod]
        public void Deserialize()
        {
            try
            {
                var vector = JsonConvert.DeserializeObject<Vector2>("{\"x\":1.25,\"y\":2.5}");
                Assert.AreEqual(1.25f, vector.x);
                Assert.AreEqual(2.5f, vector.y);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
}                                                                                   
#endif