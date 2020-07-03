using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;

namespace ScratchNet
{
    public class Serialization
    {
        private static XmlNode CreateClassNode(Class sp, XmlDocument xmlDoc)
        {
            XmlNode spNode = xmlDoc.CreateElement("Class");
            spNode.Attributes.Append(CreateAttribute(xmlDoc, "type", GetTypeName(sp.GetType())));
            PropertyInfo[] pinfo = sp.GetType().GetProperties();
            foreach (PropertyInfo pf in pinfo)
            {
                if (pf.CanRead && pf.CanWrite)
                {
                    Type type = pf.PropertyType;
                    string name = pf.Name;
                    if (name == "Expressions")
                    {
                        XmlNode exps = xmlDoc.CreateElement("Expressions");
                        foreach (Expression e in sp.Expressions)
                        {
                            exps.AppendChild(CreateExpressionNode(e, xmlDoc, sp.Positions[e]));
                        }
                        spNode.AppendChild(exps);
                    }
                    else if (name == "BlockStatements")
                    {
                        XmlNode bsNode = xmlDoc.CreateElement("BlockStatements");
                        foreach (BlockStatement bs in sp.BlockStatements)
                        {
                            if (bs.Body.Count <= 0)
                                continue;
                            bsNode.AppendChild(CreateBlockStatementNode(bs, xmlDoc, sp.Positions[bs]));
                        }
                        spNode.AppendChild(bsNode);
                    }
                    else if (name == "Variables")
                    {
                        XmlNode vNode = xmlDoc.CreateElement("Variables");
                        foreach (Variable v in sp.Variables)
                        {
                            vNode.AppendChild(CreateVariableNode(v, xmlDoc));
                        }
                        spNode.AppendChild(vNode);
                    }
                    else if (name == "Functions")
                    {
                        XmlNode fNode = xmlDoc.CreateElement("Functions");
                        foreach (Function f in sp.Functions)
                        {
                            fNode.AppendChild(CreateFunctionNode(f, xmlDoc, sp.Positions[f]));
                        }
                        spNode.AppendChild(fNode);
                    }
                    else if (name == "Handlers")
                    {
                        XmlNode eNode = xmlDoc.CreateElement("Handlers");
                        foreach (Function f in sp.Handlers)
                        {
                            eNode.AppendChild(CreateFunctionNode(f, xmlDoc, sp.Positions[f]));
                        }
                        spNode.AppendChild(eNode);
                    }
                    else if (name == "Positions")
                    {

                    }
                    else
                    {
                        object vt = pf.GetValue(sp);
                        if (vt == null)
                        {

                        }
                        if (type.IsAssignableFrom(typeof(List<string>)))
                        {
                            XmlNode snode = xmlDoc.CreateElement(name);
                            List<string> list = vt as List<string>;
                            foreach (string s in list)
                                snode.AppendChild(CreateNode(xmlDoc, "String", "value", s));
                            spNode.AppendChild(snode);
                        }
                        else
                            spNode.AppendChild(CreateNode(xmlDoc, pf.Name, "value", vt + ""));
                    }
                }
            }


            return spNode;
        }
        private static XmlNode CreateVariableNode(Variable var, XmlDocument xmlDoc)
        {
            XmlNode node = CreateNode(xmlDoc, "Variable", "type", GetTypeName(var.GetType()));
            PropertyInfo[] pinfo = var.GetType().GetProperties();
            foreach (PropertyInfo pf in pinfo)
            {
                if (pf.CanRead && pf.CanWrite)
                {
                    object v = pf.GetValue(var);
                    if (v != null)
                        node.AppendChild(CreateNode(xmlDoc, pf.Name, "value", v + ""));
                }
            }
            return node;
        }
        private static XmlNode CreateFunctionNode(Function e, XmlDocument xmlDoc, Nullable<System.Windows.Point> point = null)
        {
            XmlNode exNode = xmlDoc.CreateElement("Function");
            exNode.Attributes.Append(CreateAttribute(xmlDoc, "type", GetTypeName(e.GetType())));
            if (point != null)
            {
                exNode.Attributes.Append(CreateAttribute(xmlDoc, "x", point.Value.X + ""));
                exNode.Attributes.Append(CreateAttribute(xmlDoc, "y", point.Value.Y + ""));
            }
            PropertyInfo[] pinfo = e.GetType().GetProperties();
            foreach (PropertyInfo p in pinfo)
            {
                if (p.CanRead && p.CanWrite)
                {
                    object v = p.GetValue(e);
                    if (v is BlockStatement)
                    {
                        XmlNode lnode = CreateNode(xmlDoc, p.Name, "type", v.GetType() + "");
                        lnode.AppendChild(CreateBlockStatementNode(v as BlockStatement, xmlDoc));
                        exNode.AppendChild(lnode);
                    }
                    else if (v is List<Parameter>)
                    {
                        XmlNode lnode = CreateNode(xmlDoc, p.Name, "type", v.GetType() + "");
                        foreach (Parameter pm in (v as List<Parameter>))
                        {
                            lnode.AppendChild(CreateParameterNode(pm, xmlDoc));
                        }
                        exNode.AppendChild(lnode);
                    }
                    else if (v is Expression)
                    {
                        XmlNode lnode = xmlDoc.CreateElement(p.Name);
                        lnode.AppendChild(CreateExpressionNode(v as Expression, xmlDoc));
                        exNode.AppendChild(lnode);
                    }
                    else if (v != null)
                    {
                        exNode.AppendChild(CreateNode(xmlDoc, p.Name, "value", v + ""));
                    }
                }
            }
            return exNode;
        }
        private static XmlNode CreateParameterNode(Parameter p, XmlDocument xmlDoc)
        {
            XmlNode exNode = xmlDoc.CreateElement("Parameter");
            exNode.Attributes.Append(CreateAttribute(xmlDoc, "type", GetTypeName(p.GetType())));
            PropertyInfo[] pinfo = p.GetType().GetProperties();
            foreach (PropertyInfo pf in pinfo)
            {
                if (pf.CanRead && pf.CanWrite)
                {
                    object v = pf.GetValue(p);
                    if (v != null)
                        exNode.AppendChild(CreateNode(xmlDoc, pf.Name, "value", v + ""));
                }
            }
            return exNode;
        }
        private static XmlNode CreateBlockStatementNode(BlockStatement e, XmlDocument xmlDoc, Nullable<System.Windows.Point> point = null)
        {
            XmlNode exNode = CreateNode(xmlDoc, "BlockStatement", "type", GetTypeName(e.GetType()));
            if (point != null)
            {
                exNode.Attributes.Append(CreateAttribute(xmlDoc, "x", point.Value.X + ""));
                exNode.Attributes.Append(CreateAttribute(xmlDoc, "y", point.Value.Y + ""));
            }
            //xmlDoc.CreateElement("BlockStatement");
            XmlNode bNode = xmlDoc.CreateElement("Body");
            exNode.AppendChild(bNode);

            foreach (Statement s in e.Body)
            {
                bNode.AppendChild(CreateStatementNode(s, xmlDoc));
            }
            return exNode;
        }
        private static XmlNode CreateStatementNode(Statement e, XmlDocument xmlDoc)
        {
            XmlNode exNode = xmlDoc.CreateElement("Statement");
            exNode.Attributes.Append(CreateAttribute(xmlDoc, "type", GetTypeName(e.GetType())));
            PropertyInfo[] pinfo = e.GetType().GetProperties();
            foreach (PropertyInfo p in pinfo)
            {
                if (p.CanRead && p.CanWrite)
                {
                    object v = p.GetValue(e);
                    if (v is Expression)
                    {
                        XmlNode lnode = CreateNode(xmlDoc, p.Name, "type", v.GetType() + "");
                        lnode.AppendChild(CreateExpressionNode(v as Expression, xmlDoc));
                        exNode.AppendChild(lnode);
                    }
                    else if (v is Statement)
                    {
                        XmlNode lnode = CreateNode(xmlDoc, p.Name, "type", v.GetType() + "");
                        lnode.AppendChild(CreateStatementNode(v as Statement, xmlDoc));
                        exNode.AppendChild(lnode);
                    }
                    else if (v is List<Expression>)
                    {
                        XmlNode lnode = CreateNode(xmlDoc, p.Name, "type", v.GetType() + "");
                        foreach (Expression exp in (v as List<Expression>))
                        {
                            lnode.AppendChild(CreateExpressionNode(exp, xmlDoc));
                        }
                        exNode.AppendChild(lnode);
                    }
                    else if (v is List<string>)
                    {
                        XmlNode lnode = CreateNode(xmlDoc, p.Name, "type", v.GetType() + "");
                        foreach (string str in (v as List<string>))
                        {
                            lnode.AppendChild(CreateNode(xmlDoc, "String", "value", str + ""));
                        }
                        exNode.AppendChild(lnode);
                    }
                    else if (v is BlockStatement)
                    {
                        XmlNode lnode = CreateNode(xmlDoc, p.Name, "type", v.GetType() + "");
                        lnode.AppendChild(CreateBlockStatementNode(v as BlockStatement, xmlDoc));
                        exNode.AppendChild(lnode);
                    }
                    else if (v != null)
                    {
                        exNode.AppendChild(CreateNode(xmlDoc, p.Name, "value", v + ""));
                    }

                }
            }

            return exNode;
        }
        private static XmlNode CreateExpressionNode(Expression e, XmlDocument xmlDoc, Nullable<System.Windows.Point> point = null)
        {
            XmlNode exNode = xmlDoc.CreateElement("Expression");
            if (e != null)
            {
                exNode.Attributes.Append(CreateAttribute(xmlDoc, "type", GetTypeName(e.GetType())));
                if (point != null)
                {
                    exNode.Attributes.Append(CreateAttribute(xmlDoc, "x", point.Value.X + ""));
                    exNode.Attributes.Append(CreateAttribute(xmlDoc, "y", point.Value.Y + ""));
                }
                PropertyInfo[] pinfo = e.GetType().GetProperties();
                foreach (PropertyInfo p in pinfo)
                {
                    if (p.CanRead && p.CanWrite)
                    {
                        object v = p.GetValue(e);
                        if (v is Expression)
                        {
                            XmlNode ceNode = CreateExpressionNode(v as Expression, xmlDoc);
                            XmlNode node = xmlDoc.CreateElement(p.Name);
                            node.AppendChild(ceNode);
                            exNode.AppendChild(node);
                        }
                        else if (v is List<Expression>)
                        {
                            XmlNode lnode = CreateNode(xmlDoc, p.Name, "type", v.GetType() + "");
                            foreach (Expression exp in (v as List<Expression>))
                            {
                                lnode.AppendChild(CreateExpressionNode(exp, xmlDoc));
                            }
                            exNode.AppendChild(lnode);
                        }
                        else if (v is List<string>)
                        {
                            XmlNode lnode = CreateNode(xmlDoc, p.Name, "type", v.GetType() + "");
                            foreach (string str in (v as List<string>))
                            {
                                lnode.AppendChild(CreateNode(xmlDoc, "String", "value", str + ""));
                            }
                            exNode.AppendChild(lnode);
                        }
                        else if (v != null)
                        {
                            exNode.AppendChild(CreateNode(xmlDoc, p.Name, "value", v + ""));
                        }
                    }
                }
            }
            return exNode;
        }
        private static XmlAttribute CreateAttribute(XmlDocument xmlDoc, string name, string value)
        {
            XmlAttribute attr = xmlDoc.CreateAttribute(name);
            attr.Value = value;
            return attr;
        }
        private static XmlNode CreateNode(XmlDocument xmlDoc, string name, string attName, string value)
        {
            XmlNode node = xmlDoc.CreateElement(name);
            XmlAttribute attr = xmlDoc.CreateAttribute(attName);
            attr.Value = value + "";
            node.Attributes.Append(attr);
            return node;
        }
        private static string GetTypeName(Type t)
        {
            string[] str = t.AssemblyQualifiedName.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            return str[0] + ", " + str[1];
        }
        public static void Save(Class sp, Stream stream)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            XmlNode rootNode = xmlDoc.CreateElement("Script");
            rootNode.AppendChild(CreateClassNode(sp, xmlDoc));
            xmlDoc.AppendChild(rootNode);
            xmlDoc.Save(stream);
        }
        public static void Save(Class sp, string file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            XmlNode rootNode = xmlDoc.CreateElement("Script");
            rootNode.AppendChild(CreateClassNode(sp, xmlDoc));
            xmlDoc.AppendChild(rootNode);
            xmlDoc.Save(file);
        }
        public static Class Load(Stream stream)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);
            XmlNode root = xmlDoc.SelectSingleNode("/Script/Class");
            return LoadClass(root);
        }
        public static Class Load(string file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            XmlNode root = xmlDoc.SelectSingleNode("/Script/Class");
            return LoadClass(root);
        }
        private static Class LoadClass(XmlNode root)
        {
            if (root.Name != "Class")
                throw new FormatException("Wrong data format");
            if (root.Attributes["type"] == null)
                throw new FormatException("Wrong data format");
            string type = root.Attributes["type"].Value;

            Type t = Type.GetType(type);
            Class sp = Activator.CreateInstance(t) as Class;
            PropertyInfo[] pinfo = sp.GetType().GetProperties();
            Dictionary<string, PropertyInfo> pMap = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo p in pinfo)
            {
                pMap.Add(p.Name, p);
            }
            foreach (XmlNode node in root.ChildNodes)
            {
                string name = node.Name;
                if (name == "Variables")
                {
                    foreach (XmlNode eNode in node.ChildNodes)
                    {
                        Variable v = LoadVariable(eNode);
                        sp.Variables.Add(v);
                    }
                }
                else if (name == "Expressions")
                {
                    foreach (XmlNode eNode in node.ChildNodes)
                    {
                        Expression exp = LoadExpression(eNode);
                        sp.Expressions.Add(exp);
                        System.Windows.Point pt = new System.Windows.Point();
                        if (eNode.Attributes["x"] != null && eNode.Attributes["y"] != null)
                            pt = new System.Windows.Point(double.Parse(eNode.Attributes["x"].Value),
                                double.Parse(eNode.Attributes["y"].Value));
                        sp.Positions.Add(exp, pt);
                    }
                }
                else if (name == "BlockStatements")
                {
                    foreach (XmlNode bnode in node.ChildNodes)
                    {
                        BlockStatement bs = LoadBlockStatement(bnode);
                        if (bs.Body.Count <= 0)
                            continue;
                        sp.BlockStatements.Add(bs);
                        System.Windows.Point pt = new System.Windows.Point();
                        if (bnode.Attributes["x"] != null && bnode.Attributes["y"] != null)
                            pt = new System.Windows.Point(double.Parse(bnode.Attributes["x"].Value),
                                double.Parse(bnode.Attributes["y"].Value));
                        sp.Positions.Add(bs, pt);
                    }
                }
                else if (name == "Handlers")
                {
                    foreach (XmlNode bnode in node.ChildNodes)
                    {
                        Function func = LoadFunction(bnode);
                        sp.Handlers.Add(func as ScratchNet.EventHandler);
                        System.Windows.Point pt = new System.Windows.Point();
                        if (bnode.Attributes["x"] != null && bnode.Attributes["y"] != null)
                            pt = new System.Windows.Point(double.Parse(bnode.Attributes["x"].Value),
                                double.Parse(bnode.Attributes["y"].Value));
                        sp.Positions.Add(func, pt);
                    }
                }
                else if (name == "Functions")
                {
                    foreach (XmlNode bnode in node.ChildNodes)
                    {
                        Function func = LoadFunction(bnode);
                        sp.Functions.Add(func);
                        System.Windows.Point pt = new System.Windows.Point();
                        if (bnode.Attributes["x"] != null && bnode.Attributes["y"] != null)
                            pt = new System.Windows.Point(double.Parse(bnode.Attributes["x"].Value),
                                double.Parse(bnode.Attributes["y"].Value));
                        sp.Positions.Add(func, pt);
                    }
                }
                else if (name == "Positions")
                {

                }
                else
                {
                    PropertyInfo property = pMap[name];
                    Type pType = property.PropertyType;
                    if (pType.IsAssignableFrom(typeof(List<string>)))
                    {
                        List<string> list = new List<string>();
                        foreach (XmlNode snode in node.ChildNodes)
                        {
                            list.Add(snode.Attributes["value"].Value);
                        }
                        property.SetValue(sp, list);
                    }
                    else
                    {
                        string value = node.Attributes["value"].Value;
                        object obj = ConvertTo(value, pType);
                        if (obj != null)
                            property.SetValue(sp, obj);
                    }
                }
            }
            return sp;
        }

        private static object ConvertTo(string value, Type t)
        {

            if (t.IsEnum)
            {
                return Enum.Parse(t, value);
            }
            TypeCode c = Type.GetTypeCode(t);
            if (c == TypeCode.String)
                return value;
            IConvertible valueConvert = value as IConvertible;
            switch (c)
            {
                case TypeCode.Boolean:
                    return valueConvert.ToBoolean(null);
                case TypeCode.Int32:
                    return valueConvert.ToInt32(null);
                case TypeCode.Double:
                    return valueConvert.ToDouble(null);
                case TypeCode.String:
                    return value;
            }
            return null;
        }
        private static Variable LoadVariable(XmlNode root)
        {
            if (root.Name != "Variable")
                throw new FormatException("Wrong data format");
            if (root.Attributes["type"] == null)
                throw new FormatException("Wrong data format");
            string type = root.Attributes["type"].Value;

            Type t = Type.GetType(type);
            PropertyInfo[] inf = t.GetProperties();
            Dictionary<string, PropertyInfo> pMap = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo p in inf)
            {
                pMap.Add(p.Name, p);
            }
            Variable vr = Activator.CreateInstance(t) as Variable;
            foreach (XmlNode node in root.ChildNodes)
            {
                string pname = node.Name;
                PropertyInfo property = pMap[pname];
                Type pType = property.PropertyType;
                string value = node.Attributes["value"].Value;
                object obj = ConvertTo(value, pType);
                if (obj != null)
                    property.SetValue(vr, obj);
            }
            return vr;

        }
        private static Parameter LoadParameter(XmlNode root)
        {
            if (root.Name != "Parameter")
                throw new FormatException("Wrong data format");
            if (root.Attributes["type"] == null)
                throw new FormatException("Wrong data format");
            string type = root.Attributes["type"].Value;

            Type t = Type.GetType(type);
            PropertyInfo[] inf = t.GetProperties();
            Dictionary<string, PropertyInfo> pMap = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo p in inf)
            {
                pMap.Add(p.Name, p);
            }
            Parameter parameter = Activator.CreateInstance(t) as Parameter;
            foreach (XmlNode node in root.ChildNodes)
            {
                string pname = node.Name;
                PropertyInfo property = pMap[pname];
                Type pType = property.PropertyType;
                string value = node.Attributes["value"].Value;
                object obj = ConvertTo(value, pType);
                if (obj != null)
                    property.SetValue(parameter, obj);
            }
            return parameter;

        }
        private static Function LoadFunction(XmlNode root)
        {
            if (root.Name != "Function")
                throw new FormatException("Wrong data format");
            if (root.Attributes["type"] == null)
                throw new FormatException("Wrong data format");
            string type = root.Attributes["type"].Value;

            Type t = Type.GetType(type);
            PropertyInfo[] inf = t.GetProperties();
            Dictionary<string, PropertyInfo> pMap = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo p in inf)
            {
                pMap.Add(p.Name, p);
            }
            Function func = Activator.CreateInstance(t) as Function;
            foreach (XmlNode node in root.ChildNodes)
            {
                string pname = node.Name;
                PropertyInfo property = pMap[pname];
                Type pType = property.PropertyType;
                if (pType.IsAssignableFrom(typeof(BlockStatement)))
                {
                    property.SetValue(func, LoadBlockStatement(node.ChildNodes[0]));
                }
                else if (pType.IsAssignableFrom(typeof(Expression)))
                {
                    property.SetValue(func, LoadExpression(node.ChildNodes[0]));
                }
                else if (pType.IsAssignableFrom(typeof(List<Parameter>)))
                {
                    List<Parameter> ps = new List<Parameter>();
                    foreach (XmlNode enode in node.ChildNodes)
                    {
                        ps.Add(LoadParameter(enode));
                    }
                    property.SetValue(func, ps);
                }
                else
                {
                    string value = node.Attributes["value"].Value;
                    object obj = ConvertTo(value, pType);
                    if (obj != null)
                        property.SetValue(func, obj);
                }
            }
            return func;
        }
        private static Expression LoadExpression(XmlNode root)
        {
            if (root.Name != "Expression")
            {
                throw new FormatException("Wrong data format");
            }
            if (root.Attributes["type"] == null)
                throw new FormatException("Wrong data format");
            string type = root.Attributes["type"].Value;
            Type t = Type.GetType(type);
            PropertyInfo[] inf = t.GetProperties();
            Dictionary<string, PropertyInfo> pMap = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo p in inf)
            {
                pMap.Add(p.Name, p);
            }
            Expression exp = Activator.CreateInstance(t) as Expression;
            foreach (XmlNode node in root.ChildNodes)
            {
                string pname = node.Name;
                PropertyInfo property = pMap[pname];
                Type pType = property.PropertyType;
                if (!pType.Equals(typeof(object)) && pType.IsAssignableFrom(typeof(Expression)))
                {
                    property.SetValue(exp, LoadExpression(node.ChildNodes[0]));
                }
                else if (pType.IsAssignableFrom(typeof(List<Expression>)))
                {
                    List<Expression> expressions = new List<Expression>();
                    foreach (XmlNode enode in node.ChildNodes)
                    {
                        expressions.Add(LoadExpression(enode));
                    }
                    property.SetValue(exp, expressions);
                }
                else if (pType.IsAssignableFrom(typeof(List<string>)))
                {
                    List<String> str = new List<string>();
                    foreach (XmlNode snode in node.ChildNodes)
                    {
                        if (snode.Name == "String")
                            str.Add(snode.Attributes["value"].Value);
                    }
                    property.SetValue(exp, str);
                }
                else if (pType.IsAssignableFrom(typeof(Type)))
                {
                    Type tx = Type.GetType(node.Attributes["value"].Value);
                    property.SetValue(exp, tx);
                }
                else if (pType.IsAssignableFrom(typeof(System.Windows.Media.Color)))
                {
                    object tx = ColorConverter.ConvertFromString(node.Attributes["value"].Value);
                    property.SetValue(exp, tx);
                }
                else
                {
                    string value = node.Attributes["value"].Value;
                    object obj = ConvertTo(value, pType);
                    if (obj != null)
                        property.SetValue(exp, obj);
                }
            }

            return exp;
        }
        private static BlockStatement LoadBlockStatement(XmlNode root)
        {
            if (root.Name != "BlockStatement")
                throw new FormatException("Wrong data format");
            if (root.Attributes["type"] == null)
                throw new FormatException("Wrong data format");
            string type = root.Attributes["type"].Value;

            Type t = Type.GetType(type);
            PropertyInfo[] inf = t.GetProperties();
            Dictionary<string, PropertyInfo> pMap = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo p in inf)
            {
                pMap.Add(p.Name, p);
            }
            BlockStatement bst = Activator.CreateInstance(t) as BlockStatement;
            XmlNode bnode = root.ChildNodes[0];
            if (bnode.Name != "Body")
                throw new FormatException("Wrong data format");
            foreach (XmlNode node in bnode.ChildNodes)
            {
                bst.Body.Add(LoadStatement(node));
            }
            return bst;
        }
        private static Statement LoadStatement(XmlNode root)
        {
            if (root.Name != "Statement")
                throw new FormatException("Wrong data format");
            if (root.Attributes["type"] == null)
                throw new FormatException("Wrong data format");
            string type = root.Attributes["type"].Value;

            Type t = Type.GetType(type);
            PropertyInfo[] inf = t.GetProperties();
            Dictionary<string, PropertyInfo> pMap = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo p in inf)
            {
                pMap.Add(p.Name, p);
            }
            Statement st = Activator.CreateInstance(t) as Statement;
            foreach (XmlNode node in root.ChildNodes)
            {
                string pname = node.Name;
                PropertyInfo property = pMap[pname];
                Type pType = property.PropertyType;
                if (pType.IsAssignableFrom(typeof(BlockStatement)))
                {
                    property.SetValue(st, LoadBlockStatement(node.ChildNodes[0]));
                }
                else if (pType.IsAssignableFrom(typeof(Expression)))
                {
                    property.SetValue(st, LoadExpression(node.ChildNodes[0]));
                }
                else if (pType.IsAssignableFrom(typeof(Statement)))
                {
                    property.SetValue(st, LoadStatement(node.ChildNodes[0]));
                }
                else if (pType.IsAssignableFrom(typeof(List<Expression>)))
                {
                    List<Expression> expressions = new List<Expression>();
                    foreach (XmlNode enode in node.ChildNodes)
                    {
                        expressions.Add(LoadExpression(enode));
                    }
                    property.SetValue(st, expressions);
                }
                else if (pType.IsAssignableFrom(typeof(List<string>)))
                {
                    List<String> str = new List<string>();
                    foreach (XmlNode snode in node.ChildNodes)
                    {
                        if (snode.Name == "String")
                            str.Add(snode.Attributes["value"].Value);
                    }
                    property.SetValue(st, str);
                }
                else
                {
                    string value = node.Attributes["value"].Value;
                    object obj = ConvertTo(value, pType);
                    if (obj != null)
                        property.SetValue(st, obj);
                }
            }
            return st;
        }

    }
}
