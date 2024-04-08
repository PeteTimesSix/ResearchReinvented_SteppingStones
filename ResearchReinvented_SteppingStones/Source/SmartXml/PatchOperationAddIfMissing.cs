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

    public class PatchOperationAddIfMissing : PatchOperationPathed
    {
        public Type conditionalType = null;
        public string conditionalParam = null;
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

            if(conditionalType != null)
            {
                var conditional = SmartXmlConditionalMaker.MakeConditional(conditionalType);
                if(!conditional.ShouldExecute(conditionalParam))
                    return true; //specific condition (probably a settings value) not met
			}

            XmlNode node = value.node;
            XmlNode foundNode = null;
            bool matched = false;
            foreach (XmlNode xmlNode in xml.SelectNodes(xpath).Cast<XmlNode>().ToArray())
            {
                matched = true;
                foreach (XmlNode addNode in node.ChildNodes)
                {
                    if (!SharedUtils.ContainsNode(xmlNode, addNode, ref foundNode))
                    {
                        Log.Message($"1 adding node {addNode} to {xmlNode}");
                        xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(addNode, true));
                    }
                }
            }
            return matched;
        }
    }
}
