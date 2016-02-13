#if UNITTESTS && UNITY_5
namespace Newtonsoft.Json.UnitTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using UnityEngine;

    [TestClass]
    public class Matrix4x4ConverterTests
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
                var matrix = new Matrix4x4();
                var data = JsonConvert.SerializeObject(matrix);
                Assert.AreEqual("{\"m00\":0.0,\"m01\":0.0,\"m02\":0.0,\"m03\":0.0,\"m10\":0.0,\"m11\":0.0,\"m12\":0.0,\"m13\":0.0,\"m20\":0.0,\"m21\":0.0,\"m22\":0.0,\"m23\":0.0,\"m30\":0.0,\"m31\":0.0,\"m32\":0.0,\"m33\":0.0}", data);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void Deserialize()
        {
            try
            {
                var matrix = JsonConvert.DeserializeObject<Matrix4x4>("{\"m00\":0.0,\"m01\":0.0,\"m02\":0.0,\"m03\":0.0,\"m10\":0.0,\"m11\":0.0,\"m12\":0.0,\"m13\":0.0,\"m20\":0.0,\"m21\":0.0,\"m22\":0.0,\"m23\":0.0,\"m30\":0.0,\"m31\":0.0,\"m32\":0.0,\"m33\":0.0}");
                Assert.AreEqual(0f, matrix.m00);
                Assert.AreEqual(0f, matrix.m01);
                Assert.AreEqual(0f, matrix.m02);
                Assert.AreEqual(0f, matrix.m03);
                Assert.AreEqual(0f, matrix.m10);
                Assert.AreEqual(0f, matrix.m11);
                Assert.AreEqual(0f, matrix.m12);
                Assert.AreEqual(0f, matrix.m13);
                Assert.AreEqual(0f, matrix.m20);
                Assert.AreEqual(0f, matrix.m21);
                Assert.AreEqual(0f, matrix.m22);
                Assert.AreEqual(0f, matrix.m23);
                Assert.AreEqual(0f, matrix.m30);
                Assert.AreEqual(0f, matrix.m31);
                Assert.AreEqual(0f, matrix.m32);
                Assert.AreEqual(0f, matrix.m33);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
} 
#endif