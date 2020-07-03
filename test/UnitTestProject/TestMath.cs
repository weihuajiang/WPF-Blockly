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
    public class TestMath
    {
        public Completion Max(object a, object b)
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", a);
            e.RegisterValue("b", b);
            MaxExpression abs = new MaxExpression();
            abs.Args[0] = new Identifier("a");
            abs.Args[1] = new Identifier("b");
            return abs.Execute(e);
        }
        public Completion Min(object a, object b)
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", a);
            e.RegisterValue("b", b);
            MinExpression abs = new MinExpression();
            abs.Args[0] = new Identifier("a");
            abs.Args[1] = new Identifier("b");
            return abs.Execute(e);
        }
        [TestMethod]
        public void TestMax()
        {
            char chara = 'c';
            char charb = 'f';
            int inta = 2;
            int intb = 3;
            float floata = 2;
            float floatb = 3;
            double doublea = 2;
            double doubleb = 3;

            Assert.AreEqual(Max(chara, charb).ReturnValue, (int)Math.Max(chara, charb));
            Assert.AreEqual(Max(chara, intb).ReturnValue, Math.Max(chara, intb));

            Assert.AreEqual(Max(inta, intb).ReturnValue, Math.Max(inta, intb));
            Assert.AreEqual(Max(floata, floatb).ReturnValue, Math.Max(floata, floatb));
            Assert.AreEqual(Max(doublea, doubleb).ReturnValue, Math.Max(doublea, doubleb));
            Assert.AreEqual(Max(inta, floatb).ReturnValue, Math.Max(inta, floatb));
            Assert.AreEqual(Max(floatb, doubleb).ReturnValue, Math.Max(doubleb, floatb));
            Assert.IsTrue(Max(1, "a").Type == CompletionType.Exception);
        }
        [TestMethod]
        public void TestMin()
        {
            char chara = 'c';
            char charb = 'f';
            int inta = 2;
            int intb = 3;
            float floata = 2;
            float floatb = 3;
            double doublea = 2;
            double doubleb = 3;

            Assert.AreEqual(Min(chara, charb).ReturnValue, (int)Math.Min(chara, charb));
            Assert.AreEqual(Min(chara, intb).ReturnValue, Math.Min(chara, intb));
            Assert.AreEqual(Min(inta, intb).ReturnValue, Math.Min(inta, intb));
            Assert.AreEqual(Min(floata, floatb).ReturnValue, Math.Min(floata, floatb));
            Assert.AreEqual(Min(doublea, doubleb).ReturnValue, Math.Min(doublea, doubleb));
            Assert.AreEqual(Min(inta, floatb).ReturnValue, Math.Min(inta, floatb));
            Assert.AreEqual(Min(floatb, doubleb).ReturnValue, Math.Min(doubleb, floatb));
            Assert.IsTrue(Min(1, "a").Type == CompletionType.Exception);
        }
        public Completion Abs(object a)
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", a);
            AbsExpression abs = new AbsExpression();
            abs.Args[0] = new Identifier("a");
            return abs.Execute(e);
        }
        [TestMethod]
        public void TestAbs()
        {
            char a = 'c';
            int b = 2;
            float c = 2;
            double d = 2;

            Assert.AreEqual(Abs(a).ReturnValue, (int)Math.Abs(a));
            Assert.AreEqual(Abs(b).ReturnValue, Math.Abs(b));
            Assert.AreEqual(Abs(c).ReturnValue, Math.Abs(c));
            Assert.AreEqual(Abs(d).ReturnValue, Math.Abs(d));
            Assert.IsTrue(Abs("a").Type == CompletionType.Exception);
        }
        public Completion Sqrt(object a)
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", a);
            SqrtExpression abs = new SqrtExpression();
            abs.Args[0] = new Identifier("a");
            return abs.Execute(e);
        }
        [TestMethod]
        public void TestSqrt()
        {
            int b = 2;
            float c = 2;
            double d = 2;

            Assert.AreEqual(Sqrt(b).ReturnValue, Math.Sqrt(b));
            Assert.AreEqual(Sqrt(c).ReturnValue, Math.Sqrt(c));
            Assert.AreEqual(Sqrt(d).ReturnValue, Math.Sqrt(d));
            Assert.IsTrue(Sqrt("a").Type == CompletionType.Exception);
        }
        public Completion Pow(object a, object b)
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", a);
            e.RegisterValue("b", b);
            PowExpression abs = new PowExpression();
            abs.Args[0] = new Identifier("a");
            abs.Args[1] = new Identifier("b");
            return abs.Execute(e);
        }
        [TestMethod]
        public void TestPow()
        {
            int b = 2;
            float c = 2;
            double d = 2;

            Assert.AreEqual(Pow(b, c).ReturnValue, Math.Pow(b, c));
            Assert.AreEqual(Pow(b, c).ReturnValue, Math.Pow(b, c));

            Assert.AreEqual(Pow(b, d).ReturnValue, Math.Pow(b, d));
            Assert.AreEqual(Pow(c, d).ReturnValue, Math.Pow(c, d));
            Assert.AreEqual(Pow(d, d).ReturnValue, Math.Pow(d, d));
            Assert.IsTrue(Pow("a", 2).Type == CompletionType.Exception);
        }
    }
}
