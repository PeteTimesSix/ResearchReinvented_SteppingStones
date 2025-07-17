using PeteTimesSix.ResearchReinvented_SteppingStones.SmartXml;
using RR.SmartXml.Conditionals;
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

    public class PatchOperationAddOrReplace : PatchOperationCustomBase
    {
        public bool ignoreAttributesWhenMatching = false;

        public XmlContainer value;

        public override void Complete(string modIdentifier)
        {
            base.Complete(modIdentifier);
        }

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (Skip())
                return true;

            XmlNode node = value.node;
            XmlNode foundNode = null;
            bool matched = false;
            foreach (XmlNode xmlNode in xml.SelectNodes(xpath).Cast<XmlNode>().ToArray())
            {
                matched = true;
                foreach (XmlNode addNode in node.ChildNodes)
                {
                    if (!SharedUtils.ContainsNode(xmlNode, addNode, ref foundNode, ignoreAttributesWhenMatching))
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
    }
}
