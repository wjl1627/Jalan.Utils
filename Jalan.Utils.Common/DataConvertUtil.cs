using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jalan.Utils.Common
{
    public class DataConvertUtil
    {
        public XmlObjectNode ConvertXmlToObject(string xmlContent)
        {
            var xml = new XmlDocument();
            xml.LoadXml(xmlContent);

            var root = xml.ChildNodes[1] as XmlElement;
            XmlObjectNode rootNode = new XmlObjectNode();
            rootNode.Name = root.Name;
            rootNode.HasChild = root.HasChildNodes;
            if (root.HasChildNodes)
                rootNode.Value = GetChildNodes(root);
            else
                rootNode.Value = "xxx";
            if (root.HasAttributes)
            {
                foreach (XmlAttribute item in root.Attributes)
                {
                    rootNode.Propertes.Add(item.Name, item.Value);
                }
            }
            return rootNode;
        }

        public XmlDocument ConvertObjectToXml(XmlObjectNode objectNode)
        {
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "utf-8", null));
            var rootXml = xml.CreateElement(objectNode.Name);
            if (objectNode.Propertes.Count > 0)
            {
                foreach (var item in objectNode.Propertes)
                {
                    rootXml.SetAttribute(item.Key, item.Value);
                }
            }
            if (objectNode.HasChild)
            {
                CreateChildNodes(objectNode.Value as List<XmlObjectNode>, xml, rootXml);
            }
            else
            {
                var textNode = xml.CreateTextNode(objectNode.Name);
                textNode.InnerText = objectNode.Value.ToString();
                rootXml.AppendChild(textNode);
            }
            xml.AppendChild(rootXml);
            return xml;
        }

        /// <summary>
        /// 获取当前节点子节点
        /// </summary>
        /// <param name="curruntNode">当前节点</param>
        /// <returns>返回子节点集体</returns>
        private static object GetChildNodes(XmlElement curruntNode)
        {
            List<XmlObjectNode> nodes = new List<XmlObjectNode>();
            foreach (var itemNode in curruntNode.ChildNodes)
            {
                XmlObjectNode rootNode = new XmlObjectNode();
                if (itemNode is XmlElement)
                {
                    var xmlNode = itemNode as XmlElement;
                    rootNode.Name = xmlNode.Name;
                    rootNode.HasChild = xmlNode.HasChildNodes;

                    if (xmlNode.HasAttributes)
                    {
                        foreach (XmlAttribute item in xmlNode.Attributes)
                        {
                            rootNode.Propertes.Add(item.Name, item.Value);
                        }
                    }
                    if (xmlNode.HasChildNodes)
                    {
                        rootNode.Value = GetChildNodes(xmlNode);
                    }
                    else
                    {
                        rootNode.Value = xmlNode.InnerText;
                    }
                }
                else if (itemNode is XmlText)
                {
                    var xmlNode = itemNode as XmlText;
                    return xmlNode.InnerText;
                }
                nodes.Add(rootNode);
            }
            return nodes;
        }

        /// <summary>
        /// 根据对象父子结构 生成xml子节点
        /// </summary>
        /// <param name="objChildNodes">对象子节点集合</param>
        /// <param name="xmlDocument">整个xml对象</param>
        /// <param name="currentNode">当前节点</param>
        private static void CreateChildNodes(List<XmlObjectNode> objChildNodes, XmlDocument xmlDocument, XmlNode currentNode)
        {
            foreach (XmlObjectNode item in objChildNodes)
            {
                XmlElement node = xmlDocument.CreateElement(item.Name);
                if (item.HasChild || item.Propertes.Count > 0)
                {
                    foreach (var attr in item.Propertes)
                    {
                        node.SetAttribute(attr.Key, attr.Value);
                    }
                }
                if (item.HasChild && item.Value is List<XmlObjectNode>)
                    CreateChildNodes(item.Value as List<XmlObjectNode>, xmlDocument, node);
                else
                {
                    node.InnerText = item.Value.ToString();
                }
                currentNode.AppendChild(node);
            }
        }
    }
    public class XmlObjectNode
    {
        public string Name { get; set; }
        public Dictionary<string, string> Propertes { get; set; } = new Dictionary<string, string>();
        public bool HasChild { get; set; }
        public object Value { get; set; }
    }
}
