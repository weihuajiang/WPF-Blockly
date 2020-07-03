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
    [TestCategory("Ast Execution")]
    public class AstExectuion
    {
        [TestMethod]
        public void TestUpldateExpression()
        {
            float b = 5;
            ExecutionEnvironment e = new ExecutionEnvironment();
            UpdateExpression add = new UpdateExpression() {IsPrefix=false, Operator = UpdateOperator.Add, Expression = new Identifier() { Variable = "a" } };
            UpdateExpression minus = new UpdateExpression() { IsPrefix = false, Operator = UpdateOperator.Minus, Expression = new Identifier() { Variable = "a" } };
            //test int 
            int a=5; 
            e.RegisterValue("a", a);
            Assert.AreEqual(add.Execute(e).ReturnValue, a++);
            Assert.AreEqual(e.GetValue("a"), a);
            Assert.AreEqual(minus.Execute(e).ReturnValue, a--);
            Assert.AreEqual(e.GetValue("a"), a);
            //test float
            b = 5;
            e.SetValue("a", b);
            Assert.AreEqual(add.Execute(e).ReturnValue, b++);
            Assert.AreEqual(e.GetValue("a"), b);
            Assert.AreEqual(minus.Execute(e).ReturnValue, b--);
            Assert.AreEqual(e.GetValue("a"), b);
            //test double
            double c = 7.1;
            e.SetValue("a", c);
            Assert.AreEqual(add.Execute(e).ReturnValue, c++);
            Assert.AreEqual(e.GetValue("a"), c);
            Assert.AreEqual(minus.Execute(e).ReturnValue, c--);
            Assert.AreEqual(e.GetValue("a"), c);
            //test string
            string s = "5";
            e.SetValue("a", s);
            Assert.AreEqual(add.Execute(e).Type,  CompletionType.Exception);
            Assert.AreEqual(minus.Execute(e).Type, CompletionType.Exception);

        }
        [TestMethod]
        public void TestRandomExpression()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", 1);
            e.RegisterValue("b", 7);
            e.RegisterValue("c", 5);
            Identifier a = new Identifier() { Variable = "a" };
            Identifier b = new Identifier() { Variable = "b" };
            Identifier c = new Identifier() { Variable = "c" };
            RandomExpression s = new RandomExpression() { Min = a, Max = b };
            var c1 = s.Execute(e);
            Assert.AreEqual(c1.Type,  CompletionType.Value);
            e.SetValue("a", 3);
            var c2 = s.Execute(e);
            Assert.AreEqual(c2.Type, CompletionType.Value);
        }
        [TestMethod]
        public void TestCondictionalExpression()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", true);
            e.RegisterValue("b", false);
            e.RegisterValue("c", 5);
            Identifier a = new Identifier() { Variable = "a" };
            Identifier b = new Identifier() { Variable = "b" };
            Identifier c = new Identifier() { Variable = "c" };
            ConditionalExpression s = new ConditionalExpression()
            {
                Test = a,
                Consequent = b,
                Alternate = c
            };
            var c1 = s.Execute(e);
            Assert.AreEqual(c1.ReturnValue, false);
            e.SetValue("a", false);
            var c2 = s.Execute(e);
            Assert.AreEqual(c2.ReturnValue, 5);
        }
        [TestMethod]
        public void TestLogicalExpression()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", true);
            e.RegisterValue("b", false);
            e.RegisterValue("c", 5);
            Identifier a = new Identifier() { Variable = "a" };
            Identifier b = new Identifier() { Variable = "b" };
            Identifier c = new Identifier() { Variable = "c" };

            Assert.AreEqual(new BinaryExpression() { Left = a, Right = b, Operator = Operator.And }.Execute(e).ReturnValue, false);
            Assert.AreEqual(new BinaryExpression() { Left = a, Right = b, Operator = Operator.Or }.Execute(e).ReturnValue, true);
            Assert.AreEqual(new BinaryExpression() { Left = a, Right = c, Operator = Operator.Or }.Execute(e).Type, CompletionType.Exception);
        }
        [TestMethod]
        public void TestNotExpression()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", true);
            Identifier a = new Identifier() { Variable = "a" };
            NotExpression s = new NotExpression() { Argument = a };
            Assert.AreEqual(s.Execute(e).ReturnValue, false);
            e.SetValue("a", 5);
            Assert.AreEqual(s.Execute(e).Type, CompletionType.Exception);
        }
        [TestMethod]
        public void TestICompareExpression()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", 5.2);
            Identifier a = new Identifier() { Variable = "a" };
            Literal l = new Literal();
            l.Raw = "15";
            Literal r = new Literal() { Raw = "5.2" };

            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Great }.Execute(e).ReturnValue, true);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.GreatOrEqual }.Execute(e).ReturnValue, true);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Less }.Execute(e).ReturnValue, false);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.LessOrEqual }.Execute(e).ReturnValue, false);
            Assert.AreEqual(new BinaryExpression() { Left = a, Right = r, Operator = Operator.Equal }.Execute(e).ReturnValue, false);
        }
        [TestMethod]
        public void TestIntAssignmentExpression()
        {
            int a = 5;
            int b = 6;
            ExecutionEnvironment e = new ExecutionEnvironment();
            Identifier ai = new Identifier("a");
            Identifier bi = new Identifier("b");
            e.RegisterValue("a", 5);
            e.RegisterValue("b", 6);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.Equal }.Execute(e).ReturnValue, a=b);
            Assert.AreEqual(e.GetValue("a"), a);
            e.SetValue("a", 5);
            e.SetValue("b", 6);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.AddEqual }.Execute(e).ReturnValue, a+=b);
            Assert.AreEqual(e.GetValue("a"), a);
            e.SetValue("a", 5);
            e.SetValue("b", 6);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.MinusEqual }.Execute(e).ReturnValue, a-=b);
            Assert.AreEqual(e.GetValue("a"), a);
            e.SetValue("a", 5);
            e.SetValue("b", 6);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.MulitiplyEqual }.Execute(e).ReturnValue, a*=b);
            Assert.AreEqual(e.GetValue("a"), a);
            e.SetValue("a", 5);
            e.SetValue("b", 6);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.DivideEqual }.Execute(e).ReturnValue, a/=b);
            Assert.AreEqual(e.GetValue("a"), a);

            e.SetValue("a", 5);
            e.SetValue("b", 6);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitAndEqual }.Execute(e).ReturnValue, a &= b);
            Assert.AreEqual(e.GetValue("a"), a);
            e.SetValue("a", 5);
            e.SetValue("b", 6);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitOrEqual }.Execute(e).ReturnValue, a |= b);
            Assert.AreEqual(e.GetValue("a"), a);
            e.SetValue("a", 5);
            e.SetValue("b", 6);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitExclusiveOrEqual }.Execute(e).ReturnValue, a ^= b);
            Assert.AreEqual(e.GetValue("a"), a);
            e.SetValue("a", 5);
            e.SetValue("b", 6);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitLeftShiftEqual }.Execute(e).ReturnValue, a <<= b);
            Assert.AreEqual(e.GetValue("a"), a);
            e.SetValue("a", 5);
            e.SetValue("b", 6);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitRightShiftEqual }.Execute(e).ReturnValue, a >>= b);
            Assert.AreEqual(e.GetValue("a"), a);
        }
        [TestMethod]
        public void TestFloatAssignmentExpression()
        {
            float a = 5;
            float b = 6;
            ExecutionEnvironment e = new ExecutionEnvironment();
            Identifier ai = new Identifier("a");
            Identifier bi = new Identifier("b");
            e.RegisterValue("a", 5.0);
            e.RegisterValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.IsTrue(Math.Abs((double)(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.Equal }.Execute(e).ReturnValue)-(a = b))<0.01);
            Assert.IsTrue(Math.Abs((double)e.GetValue("a") - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.IsTrue(Math.Abs((double)(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.AddEqual }.Execute(e).ReturnValue) - (a += b)) < 0.01);
            Assert.IsTrue(Math.Abs((double)e.GetValue("a") - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.IsTrue(Math.Abs((double)(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.MinusEqual }.Execute(e).ReturnValue) - (a -= b)) < 0.01);
            Assert.IsTrue(Math.Abs((double)e.GetValue("a") - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.IsTrue(Math.Abs((double)(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.DivideEqual }.Execute(e).ReturnValue) - (a/= b)) < 0.01);
            Assert.IsTrue(Math.Abs((double)e.GetValue("a") - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            Assert.IsTrue(Math.Abs((double)(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.MulitiplyEqual }.Execute(e).ReturnValue)-(a *= b))<0.01);
            Assert.IsTrue(Math.Abs((double)e.GetValue("a") - a) < 0.01);

        }
        [TestMethod]
        public void TestDoubleAssignmentExpression()
        {
            double a = 5;
            double b = 6;
            ExecutionEnvironment e = new ExecutionEnvironment();
            Identifier ai = new Identifier("a");
            Identifier bi = new Identifier("b");
            e.RegisterValue("a", 5.0);
            e.RegisterValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.IsTrue(Math.Abs((double)(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.Equal }.Execute(e).ReturnValue) - (a = b)) < 0.01);
            Assert.IsTrue(Math.Abs((double)e.GetValue("a") - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.IsTrue(Math.Abs((double)(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.AddEqual }.Execute(e).ReturnValue) - (a += b)) < 0.01);
            Assert.IsTrue(Math.Abs((double)e.GetValue("a") - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.IsTrue(Math.Abs((double)(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.MinusEqual }.Execute(e).ReturnValue) - (a -= b)) < 0.01);
            Assert.IsTrue(Math.Abs((double)e.GetValue("a") - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.IsTrue(Math.Abs((double)(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.DivideEqual }.Execute(e).ReturnValue) - (a /= b)) < 0.01);
            Assert.IsTrue(Math.Abs((double)e.GetValue("a") - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            Assert.IsTrue(Math.Abs((double)(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.MulitiplyEqual }.Execute(e).ReturnValue) - (a *= b)) < 0.01);
            Assert.IsTrue(Math.Abs((double)e.GetValue("a") - a) < 0.01);

        }
        [TestMethod]
        public void TestFloatAssignmentExpression2()
        {
            int a = 5;
            int b = 6;
            ExecutionEnvironment e = new ExecutionEnvironment();
            Identifier ai = new Identifier("a");
            Identifier bi = new Identifier("b");
            e.RegisterValue("a", 5.0);
            e.RegisterValue("b", 6.0);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitAndEqual }.Execute(e).ReturnValue, a &= b);
            Assert.IsTrue(Math.Abs((int)(e.GetValue("a"))- a)<0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitOrEqual }.Execute(e).ReturnValue, a |= b);
            Assert.IsTrue(Math.Abs((int)(e.GetValue("a")) - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitExclusiveOrEqual }.Execute(e).ReturnValue, a ^= b);
            Assert.IsTrue(Math.Abs((int)(e.GetValue("a")) - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitLeftShiftEqual }.Execute(e).ReturnValue, a <<= b);
            Assert.IsTrue(Math.Abs((int)(e.GetValue("a")) - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitRightShiftEqual }.Execute(e).ReturnValue, a >>= b);
            Assert.IsTrue(Math.Abs((int)(e.GetValue("a")) - a) < 0.01);
        }
        [TestMethod]
        public void TestDoubleAssignmentExpression2()
        {
            int a = 5;
            int b = 6;
            ExecutionEnvironment e = new ExecutionEnvironment();
            Identifier ai = new Identifier("a");
            Identifier bi = new Identifier("b");
            e.RegisterValue("a", 5.0);
            e.RegisterValue("b", 6.0);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitAndEqual }.Execute(e).ReturnValue, a &= b);
            Assert.IsTrue(Math.Abs((int)(e.GetValue("a")) - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitOrEqual }.Execute(e).ReturnValue, a |= b);
            Assert.IsTrue(Math.Abs((int)(e.GetValue("a")) - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitExclusiveOrEqual }.Execute(e).ReturnValue, a ^= b);
            Assert.IsTrue(Math.Abs((int)(e.GetValue("a")) - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitLeftShiftEqual }.Execute(e).ReturnValue, a <<= b);
            Assert.IsTrue(Math.Abs((int)(e.GetValue("a")) - a) < 0.01);
            e.SetValue("a", 5.0);
            e.SetValue("b", 6.0);
            a = 5;
            b = 6;
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.BitRightShiftEqual }.Execute(e).ReturnValue, a >>= b);
            Assert.IsTrue(Math.Abs((int)(e.GetValue("a")) - a) < 0.01);
        }
        [TestMethod]
        public void TestStringAssignmentExpression2()
        {
            string a ;
            string b ;
            ExecutionEnvironment e = new ExecutionEnvironment();
            Identifier ai = new Identifier("a");
            Identifier bi = new Identifier("b");
            e.RegisterValue("a", "5.0");
            e.RegisterValue("b", "6.0");
            a = "5.0";
            b = "6.0";
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.AddEqual }.Execute(e).ReturnValue, a += b);
            Assert.AreEqual(a, e.GetValue("a"));
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.MinusEqual }.Execute(e).Type, CompletionType.Exception);

            e.SetValue("a", 5);
            e.SetValue("b", "6.0");
            a = "5";
            b = "6.0";
            Assert.AreEqual(new AssignmentExpression() { Left = ai, Right = bi, Operator = AssignmentOperator.AddEqual }.Execute(e).ReturnValue, a += b);
            Assert.AreEqual(a, e.GetValue("a"));
        }
        [TestMethod]
        public void TestIBindaryExpression()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();

            Literal l = new Literal();
            l.Raw = "15";
            Literal r = new Literal() { Raw = "5.2" };

            Assert.IsTrue(Math.Abs((float)(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Add }.Execute(e).ReturnValue)-(15+5.2f))<0.0001);
            Assert.IsTrue(Math.Abs((float)(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Minus }.Execute(e).ReturnValue) - (15 - 5.2f)) < 0.0001);
            Assert.IsTrue(Math.Abs((float)(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Mulitiply }.Execute(e).ReturnValue) - (15 * 5.2f)) < 0.0001);
            Assert.IsTrue(Math.Abs((float)(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Divide }.Execute(e).ReturnValue) - (15 / 5.2f)) < 0.0001);

            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Great }.Execute(e).ReturnValue, true);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.GreatOrEqual }.Execute(e).ReturnValue, true);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Less }.Execute(e).ReturnValue, false);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.LessOrEqual }.Execute(e).ReturnValue, false);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Equal }.Execute(e).ReturnValue, false);
        }
        [TestMethod]
        public void TestCharBindaryExpression()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();

            Literal l = new Literal();
            l.Raw = "'a'";
            Literal r = new Literal() { Raw = "'c'" };
            char a = 'a';
            char c = 'c';
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Add }.Execute(e).ReturnValue, a+c);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Minus }.Execute(e).ReturnValue, a - c);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Mulitiply }.Execute(e).ReturnValue, a * c);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Mod }.Execute(e).ReturnValue, a % c);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.BitAnd }.Execute(e).ReturnValue, a &c);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.BitRightShift }.Execute(e).ReturnValue, a >> c);
            r.Raw = "\"t\"";
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Add }.Execute(e).ReturnValue, a + "t");
        }
        [TestMethod]
        public void TestBindaryExpression2()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();

            Literal l = new Literal();
            l.Raw = "'a'";
            Literal r = new Literal() { Raw = "5" };
            char a = 'a';
            int c = 5;
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Add }.Execute(e).ReturnValue, a + c);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Minus }.Execute(e).ReturnValue, a - c);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Mulitiply }.Execute(e).ReturnValue, a * c);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.Mod }.Execute(e).ReturnValue, a % c);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.BitAnd }.Execute(e).ReturnValue, a & c);
            Assert.AreEqual(new BinaryExpression() { Left = l, Right = r, Operator = Operator.BitRightShift }.Execute(e).ReturnValue, a >> c);
        }
        [TestMethod]
        public void TestIsNumber()
        {
            int a = 5;
            Assert.IsTrue(TypeConverters.IsNumber(a));
            float b = 5;
            double c = 1;
            Assert.IsTrue(TypeConverters.IsNumber(b));
            Assert.IsTrue(TypeConverters.IsNumber(c));
        }
        [TestMethod]
        public void TestAssignmentStatement()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", 5);
            Identifier i = new Identifier();
            i.Variable = "a";
            AssignmentExpression s = new AssignmentExpression();
            s.Left = i;
            Literal l = new Literal() { Raw = "7" };
            s.Right = l;
            var c = s.Execute(e);
            Assert.AreEqual(c.ReturnValue, 7);
            e.RegisterValue("c", 5);
            var dd = new AssignmentExpression() { Left = new Identifier() { Variable = "c" }, Right = new BinaryExpression() { Left = new Identifier() { Variable = "c" }, Operator = Operator.Add, Right = new Literal() { Raw = "2" } } };
            var cx = dd.Execute(e);
            Assert.AreEqual(cx.ReturnValue, 7);
        }
        [TestMethod]
        public void TestExpressionStatement()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", 5);
            Identifier i = new Identifier();
            i.Variable = "a";
            ExpressionStatement s = new ExpressionStatement();
            s.Expression = i;
            var c = s.Execute(e);
            Assert.AreEqual(c.ReturnValue, 5);
        }
        [TestMethod]
        public void TestIndentifier()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", 5);
            Identifier i = new Identifier();
            i.Variable = "a";
            var c = i.Execute(e);
            Assert.AreEqual(c.ReturnValue, 5);
        }
        [TestMethod]
        public void TestVariableDeclarationcs()
        {
            VariableDeclarationcs v = new VariableDeclarationcs();
            v.Name = "v";
            v.Value = 5;
            ExecutionEnvironment e = new ExecutionEnvironment();
            var r=v.Execute(e);
            Assert.AreEqual(r.ReturnValue, 5);
            Assert.AreEqual(r.Type, CompletionType.Value);
            Assert.AreEqual(e.GetValue("v"), 5);
        }
        [TestMethod]
        public void TestLiteral()
        {
            Literal v = new Literal();
            v.Raw = "5";
            ExecutionEnvironment e = new ExecutionEnvironment();
            var r = v.Execute(e);
            Assert.AreEqual(r.ReturnValue, 5);
            {
                Literal a = new Literal() { Raw = "6.0" };
                var ra = a.Execute(e);
                Assert.AreEqual(ra.Type, CompletionType.Value);
                Assert.AreEqual(ra.ReturnValue, 6.0f);
            }
            {
                Literal a = new Literal() { Raw = "\"6.0\"" };
                var ra = a.Execute(e);
                Assert.AreEqual(ra.Type, CompletionType.Value);
                Assert.AreEqual(ra.ReturnValue, "6.0");
            }
        }
    }
}
