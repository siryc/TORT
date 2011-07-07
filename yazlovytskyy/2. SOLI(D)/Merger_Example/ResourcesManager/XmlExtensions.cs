using System.Linq;
using System.Xml;

namespace ResourceMerger
{
    public static class XmlExtensions
    {
        public static XmlNode FindNode(this XmlNodeList xmlNodeList, XmlNode nodeToFind)
        {
            return xmlNodeList.Cast<XmlNode>().FirstOrDefault(
                    node => node.Name == nodeToFind.Name && node.Attributes.EqualByValues(nodeToFind.Attributes)
                );
        }

        public static bool EqualByValues(this XmlAttributeCollection attributes, XmlAttributeCollection compareWith)
        {
            var thisCount = attributes == null ? 0 : attributes.Count;
            var compareWithCount = compareWith == null ? 0 : compareWith.Count;

            if (thisCount != compareWithCount)
            {
                return false;
            }
            for (var i = 0; i < thisCount; i++)
            {
                if (!attributes[i].EqualByValue(compareWith[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool EqualByValue(this XmlAttribute attribute, XmlAttribute compareWith)
        {
            return attribute.Name == compareWith.Name && attribute.Value == compareWith.Value;
        }

        public static XmlNode CopyTo(this XmlNode thisNode, XmlNode node, int pos)
        {
            XmlNode copyOfNode;
            if (node.OwnerDocument == null)
            {
                copyOfNode = thisNode.Clone();
            }
            else
            {
                copyOfNode = node.OwnerDocument.ImportNode(thisNode, true);
            }

            if (pos<node.ChildNodes.Count)
            {
                node.InsertBefore(copyOfNode, node.ChildNodes[pos]);
            }
            else
            {
                node.AppendChild(copyOfNode);
            }
            
            if (copyOfNode.PreviousSibling is XmlSignificantWhitespace)
            {
                node.RemoveChild(copyOfNode.PreviousSibling);
            }
            return copyOfNode;
        }
    }
}