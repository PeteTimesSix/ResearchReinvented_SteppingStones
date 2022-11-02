using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Verse;

namespace RR
{
    //code taken with permission from https://github.com/15adhami/XmlExtensions/blob/main/Source/PatchOperations/PatchOperations/PatchOperationAddOrReplace.cs

    public class PatchOperationAddOrReplace : PatchOperationPathed
    {
        public string doesRequire;

        public XmlContainer value;

        public override void Complete(string modIdentifier)
        {
            base.Complete(modIdentifier);
        }

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (!string.IsNullOrWhiteSpace(doesRequire)) 
            {
                var split = doesRequire.Split(',');
                foreach(var mod in split)
                {
                    if (!ModsConfig.IsActive(mod.Trim()))
                    {
                        return true; //a required mod isnt loaded, so we dont apply the patch
                    }
                }
            }

            XmlNode node = value.node;
            XmlNode foundNode = null;
            bool matched = false;
            foreach (XmlNode xmlNode in xml.SelectNodes(xpath).Cast<XmlNode>().ToArray())
            {
                matched = true;
                foreach (XmlNode addNode in node.ChildNodes)
                {
                    if (!ContainsNode(xmlNode, addNode, ref foundNode))
                    {
                        xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(addNode, true));
                    }
                    else
                    {
                        xmlNode.InsertAfter(xmlNode.OwnerDocument.ImportNode(addNode, true), foundNode);
                        xmlNode.RemoveChild(foundNode);
                    }
                }
            }
            return matched;
        }

        protected bool ContainsNode(XmlNode parent, XmlNode node)
        {
            XmlNode temp = null;
            return ContainsNode(parent, node, ref temp);
        }

        protected bool ContainsNode(XmlNode parent, XmlNode node, ref XmlNode foundNode)
        {
            XmlAttributeCollection attrs = node.Attributes;
            foreach (XmlNode childNode in parent.ChildNodes)
            {
                if (childNode.Name == node.Name)
                {
                    XmlAttributeCollection attrsChild = childNode.Attributes;
                    if (attrs == null && attrsChild == null)
                    {
                        foundNode = childNode;
                        return true;
                    }
                    if (attrs != null && attrsChild != null && attrs.Count == attrsChild.Count)
                    {
                        bool b = true;
                        foreach (XmlAttribute attr in attrs)
                        {
                            XmlNode attrChild = attrsChild.GetNamedItem(attr.Name);
                            if (attrChild == null)
                            {
                                b = false;
                                break;
                            }
                            if (attrChild.Value != attr.Value)
                            {
                                b = false;
                                break;
                            }
                        }
                        if (b)
                        {
                            foundNode = childNode;
                            return true;
                        }
                    }
                }
            }
            foundNode = null;
            return false;
        }
    }
}
