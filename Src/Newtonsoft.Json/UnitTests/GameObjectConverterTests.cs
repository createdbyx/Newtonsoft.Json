#if UNITTESTS && UNITY_5
namespace Newtonsoft.Json.UnitTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using UnityEngine;

    using Object = UnityEngine.Object;

    [TestClass]
    public class GameObjectConverterTests
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
            GameObject vector = null;
            try
            {
                vector = new GameObject("test");
                var data = JsonConvert.SerializeObject(vector);
                Assert.AreEqual("{\"name\":\"test\",\"isStatic\":false,\"layer\":0,\"tag\":\"Untagged\",\"hideFlags\":0}", data);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            finally
            {
                Object.DestroyImmediate(vector);
            }
        }

        [TestMethod]
        public void Deserialize()
        {
            GameObject obj = null;
            try
            {
                obj = JsonConvert.DeserializeObject<GameObject>("{\"name\":\"test\",\"isStatic\":false,\"layer\":0,\"tag\":\"Untagged\",\"hideFlags\":0}");
                Assert.AreEqual("test", obj.name);
                Assert.AreEqual(false, obj.isStatic);
                Assert.AreEqual(0, obj.layer);
                Assert.AreEqual("Untagged", obj.tag);
                Assert.AreEqual(0, obj.hideFlags);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            finally
            {
                Object.DestroyImmediate(obj);
            }
        }
    }
} 
#endif