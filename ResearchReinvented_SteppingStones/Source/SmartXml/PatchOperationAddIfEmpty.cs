﻿using PeteTimesSix.ResearchReinvented_SteppingStones.SmartXml;
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

    public class PatchOperationAddIfEmpty : PatchOperationPathed
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
            bool matched = false;
            foreach (XmlNode xmlNode in xml.SelectNodes(xpath).Cast<XmlNode>().ToArray())
            {
                matched = true;
                if(xmlNode.ChildNodes.Count == 0)
                {
                    foreach (XmlNode addNode in node.ChildNodes)
                    {
                        xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(addNode, true));
                    }
                }
            }
            return matched;
        }
    }
}
