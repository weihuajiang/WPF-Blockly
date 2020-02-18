using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScratchNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class TestEnvronment
    {
        [TestMethod]
        public void TestVariable()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", null);
            Assert.IsNull(e.GetValue("a"));
            e.RegisterValue("b", "5");
            Assert.AreEqual(e.GetValue("b"), "5");
        }
        [TestMethod]
        public void TestIntConvetion()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", 1);
            Assert.AreEqual(e.GetValue<string>("a"), "1");
            Assert.AreEqual(e.GetValue<float>("a"), 1);
            Assert.AreEqual(e.GetValue<double>("a"), 1);
        }
        [TestMethod]
        public void TestStringConvertion()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("b", "5");
            Assert.AreEqual(e.GetValue("b"), "5");
            Assert.AreEqual(e.GetValue<int>("b"), 5);
            Assert.AreEqual(e.GetValue<float>("b"), 5);
            e.RegisterValue("c", "false");
            Assert.AreEqual(e.GetValue<bool>("c"), false);
        }
        [TestMethod]
        public void TestFunction()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterFunction("a", DelegateFunction.CreateDelegate(this, "Test"));
            e.RegisterFunction("b", DelegateFunction.CreateDelegate(this, "Test2"));
            DelegateFunction f = e.GetFunction("a");
            DelegateFunction f1 = e.GetFunction("b");
            Assert.AreEqual(f.Invoke(), "a");
            Assert.AreEqual(f1.Invoke(2), 2);
        }
        public string Test()
        {
            return "a";
        }
        public int Test2(int a)
        {
            return a;
        }
    }
}
