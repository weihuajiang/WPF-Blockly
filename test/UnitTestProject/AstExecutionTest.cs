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
    public class AstExecutionTest
    {
        [TestMethod]
        public void TestIfStatement()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", true);
            e.RegisterValue("b", false);
            e.RegisterValue("c", 5);
            e.RegisterValue("d", 6);
            IfStatement s = new IfStatement();
            s.Test = new BinaryExpression() { Left = new Identifier() { Variable = "a" }, Right = new Identifier() { Variable = "b" }, Operator = Operator.And };
            s.Consequent = new BlockStatement();
            s.Consequent.Body.Add(new ExpressionStatement() { Expression = new AssignmentExpression() { Left = new Identifier() { Variable = "c" }, Right = new Identifier() { Variable = "d" } } }); 
            s.Alternate = new BlockStatement();
            s.Alternate.Body.Add(new ExpressionStatement() { Expression = new Identifier() { Variable = "d" } });
            Assert.AreEqual(s.Execute(e).ReturnValue, 6);

            e.SetValue("b", true);
            Assert.AreEqual(s.Execute(e).ReturnValue, 6);
        }
        [TestMethod]
        public void TestLoopStatement()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", true);
            e.RegisterValue("b", true);
            e.RegisterValue("c", 5);
            e.RegisterValue("d", 6);
            LoopStatement s = new LoopStatement();
            s.Loop = new BinaryExpression() { Left = new Identifier() { Variable = "a" }, Right = new Identifier() { Variable = "b" }, Operator = Operator.And };
            s.Body = new BlockStatement();
            s.Body.Body.Add(new ExpressionStatement() { Expression = new AssignmentExpression() { Left = new Identifier() { Variable = "c" }, Right = new BinaryExpression() { Left = new Identifier() { Variable = "c" }, Operator = Operator.Add, Right = new Literal() { Raw = "2" } } } });
            s.Body.Body.Add(new ExpressionStatement() { Expression = new AssignmentExpression() { Left = new Identifier() { Variable = "a" }, Right = new Literal() { Raw = "false" } } });
            var c = s.Execute(e);
            Assert.AreEqual(c.Type,  CompletionType.Value);
        }
        [TestMethod]
        public void TestWhileStatement()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", true);
            e.RegisterValue("b", true);
            e.RegisterValue("c", 5);
            e.RegisterValue("d", 6);
            WhileStatement s = new WhileStatement();
            s.Test = new BinaryExpression() { Left = new Identifier() { Variable = "a" }, Right = new Identifier() { Variable = "b" }, Operator = Operator.And };
            s.Body = new BlockStatement();
            s.Body.Body.Add(new ExpressionStatement() { Expression = new AssignmentExpression() { Left = new Identifier() { Variable = "c" }, Right = new BinaryExpression() { Left = new Identifier() { Variable = "c" }, Operator = Operator.Add, Right = new Literal() { Raw = "2" } } } });
            s.Body.Body.Add(new ExpressionStatement() { Expression = new AssignmentExpression() { Left = new Identifier() { Variable = "a" }, Right = new Literal() { Raw = "false" } } });
            var c = s.Execute(e);
            Assert.AreEqual(e.GetValue("c"), 7);
        }
        [TestMethod]
        public void TestWhileStatement2()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", true);
            e.RegisterValue("b", true);
            e.RegisterValue("c", 5);
            e.RegisterValue("d", 6);
            WhileStatement s = new WhileStatement();
            s.Test = new Identifier() { Variable = "c" };
            s.Body = new BlockStatement();
            s.Body.Body.Add(new ExpressionStatement() { Expression = new AssignmentExpression() { Left = new Identifier() { Variable = "c" }, Right = new BinaryExpression() { Left = new Identifier() { Variable = "c" }, Operator = Operator.Add, Right = new Literal() { Raw = "2" } } } });
            s.Body.Body.Add(new ExpressionStatement() { Expression = new AssignmentExpression() { Left = new Identifier() { Variable = "a" }, Right = new Literal() { Raw = "false" } } });
            var c = s.Execute(e);
            Assert.AreEqual(c.Type, CompletionType.Exception);
        }
        [TestMethod]
        public void TesLoopStatement2()
        {
            ExecutionEnvironment e = new ExecutionEnvironment();
            e.RegisterValue("a", true);
            e.RegisterValue("b", true);
            e.RegisterValue("c", 5);
            e.RegisterValue("d", 6);
            LoopStatement s = new LoopStatement();
            s.Loop =  new Identifier() { Variable = "c" };
            s.Body = new BlockStatement();
            s.Body.Body.Add(new ExpressionStatement() { Expression = new AssignmentExpression() { Left = new Identifier() { Variable = "c" }, Right = new BinaryExpression() { Left = new Identifier() { Variable = "c" }, Operator = Operator.Add, Right = new Literal() { Raw = "2" } } } });
            s.Body.Body.Add(new ExpressionStatement() { Expression = new AssignmentExpression() { Left = new Identifier() { Variable = "a" }, Right = new Literal() { Raw = "false" } } });
            var c = s.Execute(e);
            Assert.AreEqual(e.GetValue("c"), 5+5*2);
        }
    }
}
