using PeteTimesSix.ResearchReinvented_SteppingStones.SmartXml;
using PeteTimesSix.ResearchReinvented_SteppingStones.Utilities;
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
    public enum TargetPrerequisiteKind
    {
        ProjectPrerequisites,
        ProjectHiddenPrerequisites,
        Prerequisites,
        SowResearchPrerequisites
    }

    public class PatchOperationResearchPrereg : PatchOperationPathed
    {
        public Type conditionalType = null;
        public string conditionalParam = null;
        public string doesRequire;

        public TargetPrerequisiteKind target;
        public List<string> defNames;

        public override void Complete(string modIdentifier)
        {
            base.Complete(modIdentifier);
        }

        protected override bool ApplyWorker(XmlDocument xml)
        {
            Log.Message($"LOOKING FOR {string.Join(",", defNames)} OF {target}, xpath {this.xpath}");

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

            string targetNodeName = target switch
            {
                TargetPrerequisiteKind.ProjectHiddenPrerequisites => "hiddenPrerequisites",
                TargetPrerequisiteKind.Prerequisites => "researchPrerequisites",
                TargetPrerequisiteKind.SowResearchPrerequisites => "sowResearchPrerequisites",
                TargetPrerequisiteKind.ProjectPrerequisites => "prerequisites",
                _ => "prerequisites",
            }; ;

            bool matched = false;
            foreach (XmlNode xmlNode in xml.SelectNodes(xpath).Cast<XmlNode>().ToArray())
            {
                matched = true;
                foreach(XmlNode node in xmlNode.ChildNodes)
                {
                    if(node.Name == targetNodeName)
                    {
                        node.RemoveAll();
                        foreach(var defName in defNames)
                        {
                            var toAdd = DefNameSubstitutor.GetDefNameOrSub(defName);
                            XmlNode newNode = node.OwnerDocument.CreateElement("li");
                            newNode.InnerText = toAdd;
                            node.AppendChild(node.OwnerDocument.ImportNode(newNode, true));
                        }
                    }
                }
            }
            return matched;
        }
    }
}
