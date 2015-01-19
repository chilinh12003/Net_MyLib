using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Collections.Generic;
namespace MyLib.MyUtility
{
    public class JsonFriendlyNameAttribute : Attribute
    {
        private string name;

        public JsonFriendlyNameAttribute()
        {
        }

        public JsonFriendlyNameAttribute(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    public class MyJSON
    {
        private readonly StringBuilder _output = new StringBuilder();

        public static string ToJSON(object obj)
        {
            return new MyJSON().ConvertToJSON(obj);
        }

        public static string ToJSON(object obj, bool isLower)
        {
            return new MyJSON().ConvertToJSON(obj).ToLower();
        }

        private string ConvertToJSON(object obj)
        {
            WriteValue(obj);

            return _output.ToString();
        }

        private void WriteValue(object obj)
        {
            if (obj == null)
                _output.Append("null");
            else if (obj is sbyte || obj is byte || obj is short || obj is ushort || obj is int ||
                     obj is uint || obj is ulong || obj is decimal || obj is double || obj is float)
                _output.Append(Convert.ToString(obj, NumberFormatInfo.InvariantInfo));
            else if (obj is bool)
                _output.Append(obj.ToString().ToLower());
            else if (obj is char || obj is Enum || obj is Guid)
                WriteString("" + obj);
            else if (obj is DateTime)
                WriteString(((DateTime)obj).ToString("dd/MM/yyyy").ToLower());
            else if (obj is long)
                WriteString(obj.ToString());
            else if (obj is string)
                WriteString((string)obj);
            else if (obj is IDictionary)
                WriteDictionary((IDictionary)obj);
            else if (obj is Array || obj is IList || obj is ICollection)
                WriteArray((IEnumerable)obj);
            else
                WriteObject(obj);
        }

        private void WriteObject(object obj)
        {
            _output.Append("{ ");

            bool pendingSeparator = false;

            foreach (FieldInfo field in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (pendingSeparator)
                    _output.Append(" , ");

                WritePair(field.Name, field.GetValue(obj));

                pendingSeparator = true;
            }

            foreach (PropertyInfo property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                //if (property.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                //{
                //    continue;
                //}

                string name = property.Name;

                if (property.GetCustomAttributes(typeof(JsonFriendlyNameAttribute), true).Length > 0)
                {
                    var friendlyName =
                        (JsonFriendlyNameAttribute)
                        property.GetCustomAttributes(typeof(JsonFriendlyNameAttribute), true)[0];
                    name = friendlyName.Name;
                }

                if (!property.CanRead)
                    continue;

                if (pendingSeparator)
                    _output.Append(" , ");

                WritePair(name, property.GetValue(obj, null));

                pendingSeparator = true;
            }

            _output.Append(" }");
        }

        private void WritePair(string name, object value)
        {
            WriteString(name);

            _output.Append(" : ");

            WriteValue(value);
        }

        private void WriteArray(IEnumerable array)
        {
            _output.Append("[ ");

            bool pendingSeperator = false;

            foreach (object obj in array)
            {
                if (pendingSeperator)
                    _output.Append(',');

                WriteValue(obj);

                pendingSeperator = true;
            }

            _output.Append(" ]");
        }

        private void WriteDictionary(IDictionary dic)
        {
            _output.Append("{ ");

            bool pendingSeparator = false;

            foreach (DictionaryEntry entry in dic)
            {
                if (pendingSeparator)
                    _output.Append(" , ");

                WritePair(entry.Key.ToString(), entry.Value);

                pendingSeparator = true;
            }

            _output.Append(" }");
        }

        private void WriteString(string s)
        {
            _output.Append('\"');

            foreach (char c in s)
            {
                switch (c)
                {
                    case '\t':
                        _output.Append("\\t");
                        break;
                    case '\r':
                        _output.Append("\\r");
                        break;
                    case '\n':
                        _output.Append("\\n");
                        break;
                    case '"':
                    case '\\':
                        _output.Append("\\" + c);
                        break;
                    default:
                        {
                            if (c >= ' ' && c < 128)
                                _output.Append(c);
                            else
                                _output.Append("\\u" + ((int)c).ToString("X4"));
                        }
                        break;
                }
            }

            _output.Append('\"');
        }


        public static string XmlToJSON(XmlDocument xmlDoc)
        {
            StringBuilder sbJSON = new StringBuilder();
            sbJSON.Append("{ ");
            XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, true);
            sbJSON.Append("}");
            return sbJSON.ToString();
        }

        //  XmlToJSONnode:  Output an XmlElement, possibly as part of a higher array
        private static void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName)
        {
            if (showNodeName)
                sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
            sbJSON.Append("{");
            // Build a sorted list of key-value pairs
            //  where   key is case-sensitive nodeName
            //          value is an ArrayList of string or XmlElement
            //  so that we know whether the nodeName is an array or not.
            SortedList childNodeNames = new SortedList();

            //  Add in all node attributes
            if (node.Attributes != null)
                foreach (XmlAttribute attr in node.Attributes)
                    StoreChildNode(childNodeNames, attr.Name, attr.InnerText);

            //  Add in all nodes
            foreach (XmlNode cnode in node.ChildNodes)
            {
                if (cnode is XmlText)
                    StoreChildNode(childNodeNames, "value", cnode.InnerText);
                else if (cnode is XmlElement)
                    StoreChildNode(childNodeNames, cnode.Name, cnode);
            }

            // Now output all stored info
            foreach (string childname in childNodeNames.Keys)
            {
                ArrayList alChild = (ArrayList)childNodeNames[childname];
                if (alChild.Count == 1)
                    OutputNode(childname, alChild[0], sbJSON, true);
                else
                {
                    sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                    foreach (object Child in alChild)
                        OutputNode(childname, Child, sbJSON, false);
                    sbJSON.Remove(sbJSON.Length - 2, 2);
                    sbJSON.Append(" ], ");
                }
            }
            sbJSON.Remove(sbJSON.Length - 2, 2);
            sbJSON.Append(" }");
        }

        //  StoreChildNode: Store data associated with each nodeName
        //                  so that we know whether the nodeName is an array or not.
        private static void StoreChildNode(SortedList childNodeNames, string nodeName, object nodeValue)
        {
            // Pre-process contraction of XmlElement-s
            if (nodeValue is XmlElement)
            {
                // Convert  <aa></aa> into "aa":null
                //          <aa>xx</aa> into "aa":"xx"
                XmlNode cnode = (XmlNode)nodeValue;
                if (cnode.Attributes.Count == 0)
                {
                    XmlNodeList children = cnode.ChildNodes;
                    if (children.Count == 0)
                        nodeValue = null;
                    else if (children.Count == 1 && (children[0] is XmlText))
                        nodeValue = ((XmlText)(children[0])).InnerText;
                }
            }
            // Add nodeValue to ArrayList associated with each nodeName
            // If nodeName doesn't exist then add it
            object oValuesAL = childNodeNames[nodeName];
            ArrayList ValuesAL;
            if (oValuesAL == null)
            {
                ValuesAL = new ArrayList();
                childNodeNames[nodeName] = ValuesAL;
            }
            else
                ValuesAL = (ArrayList)oValuesAL;
            ValuesAL.Add(nodeValue);
        }

        private static void OutputNode(string childname, object alChild, StringBuilder sbJSON, bool showNodeName)
        {
            if (alChild == null)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                sbJSON.Append("null");
            }
            else if (alChild is string)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                string sChild = (string)alChild;
                sChild = sChild.Trim();
                sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
            }
            else
                XmlToJSONnode(sbJSON, (XmlElement)alChild, showNodeName);
            sbJSON.Append(", ");
        }

        // Make a string safe for JSON
        private static string SafeJSON(string sIn)
        {
            StringBuilder sbOut = new StringBuilder(sIn.Length);
            foreach (char ch in sIn)
            {
                if (Char.IsControl(ch) || ch == '\'')
                {
                    int ich = (int)ch;
                    sbOut.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                else if (ch == '\"' || ch == '\\' || ch == '/')
                {
                    sbOut.Append('\\');
                }
                sbOut.Append(ch);
            }
            return sbOut.ToString();
        }


        public static string JSonToXML(string JSon)
        {
            try
            {
                XmlDocument doc = (XmlDocument)Newtonsoft.Json.JsonConvert.DeserializeXmlNode(JSon);
                return doc.OuterXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object JSonToObject(string JSon)
        {

            return Newtonsoft.Json.JsonConvert.DeserializeObject(JSon);
        }
    }
}
