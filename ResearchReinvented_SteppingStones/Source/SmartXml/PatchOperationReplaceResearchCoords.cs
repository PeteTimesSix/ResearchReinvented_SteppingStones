using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Verse;

namespace RR
{
    public class PatchOperationReplaceResearchCoords : PatchOperationCustomBase
	{

		public XmlContainer researchViewX;
		public XmlContainer researchViewY;

        public PatchOperationReplaceResearchCoords()
        {
        }

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (Skip())
                return true;

            string valueX = researchViewX.node.InnerText;
			string valueY = researchViewY.node.InnerText;
			//Log.Message($"running research coord replace patch on {xpath}, values: {valueX} {valueY}");
			bool matched = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(xpath).Cast<XmlNode>().ToArray())
			{
				matched = true;
				var presentNodeX = xmlNode.SelectSingleNode("researchViewX");
				var presentNodeY = xmlNode.SelectSingleNode("researchViewY");

				if (presentNodeX != null)
					presentNodeX.InnerText = valueX;
				else
				{
					var newNode = xmlNode.OwnerDocument.CreateNode(XmlNodeType.Element, "researchViewX", "");
					newNode.InnerText = valueX;
					xmlNode.AppendChild(newNode);
				}

				if (presentNodeY != null)
					presentNodeY.InnerText = valueY;
				else
				{
					var newNode = xmlNode.OwnerDocument.CreateNode(XmlNodeType.Element, "researchViewY", "");
					newNode.InnerText = valueY;
					xmlNode.AppendChild(newNode);
				}
			}
			return matched;
		}
	}
}
