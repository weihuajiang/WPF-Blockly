using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScratchNet;

namespace UnitTestProject
{
    [TestClass]
    public class TestExpression
    {
        [TestMethod]
        public void IntValueTest()
        {
            ScriptValue v = ScriptValue.New<int>(5);
            Assert.AreEqual(v.GetValue<double>(), 5.0);
            Assert.AreEqual(v.GetValue<string>(), "5");
        }
        [TestMethod]
        public void StringTest()
        {

            ScriptValue a = ScriptValue.New<string>("5");
            Assert.AreEqual(a.GetValue<int>(), 5);
            Assert.AreEqual(a.GetValue<double>(), 5.0);
        }
        [TestMethod]
        public void ArrayTest()
        {
            ScriptValue b = ScriptValue.New<List<string>>(new List<string>() { "a", "b" });
            Assert.AreEqual(b.GetArrayValue<string>(0), "a");
        }


    }
}
