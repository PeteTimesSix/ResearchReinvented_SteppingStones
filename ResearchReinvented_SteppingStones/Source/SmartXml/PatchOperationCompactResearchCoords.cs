using PeteTimesSix.ResearchReinvented_SteppingStones.SmartXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Verse;

namespace RR
{
    public class PatchOperationCompactResearchCoords : PatchOperationCustomBase
	{

#pragma warning disable CS0649 // Filled from xml
		private XmlContainer dividerX;
		private XmlContainer dividerY;
#pragma warning restore CS0649

		protected override bool ApplyWorker(XmlDocument xml)
        {
            if (Skip())
                return true;

            bool hasDivX = float.TryParse(dividerX?.node?.InnerText ?? "", out float divX);
			bool hasDivY = float.TryParse(dividerY?.node?.InnerText ?? "", out float divY);

			if (!hasDivX && !hasDivY)
				return false;

			//Log.Message($"running research coord replace patch on {xpath}, values: {valueX} {valueY}");
			bool matched = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(xpath).Cast<XmlNode>().ToArray())
			{
				matched = true;
                if (hasDivX) 
				{
					var presentNodeX = xmlNode.SelectSingleNode("researchViewX");
					if (presentNodeX != null)
                    {
						bool parsed = float.TryParse(presentNodeX.InnerText, out float val);
						if(parsed)
                        {
							float res = val /= divX;
							presentNodeX.InnerText = $"{res.ToString("F2")}";
						}
                    }
				}

				if (hasDivY)
				{
					var presentNodeY = xmlNode.SelectSingleNode("researchViewY");
					if (presentNodeY != null)
					{
						bool parsed = float.TryParse(presentNodeY.InnerText, out float val);
						if (parsed)
						{
							float res = val /= divY;
							presentNodeY.InnerText = $"{res.ToString("F2")}";
						}
					}
				}
			}
			return matched;
		}

		public PatchOperationCompactResearchCoords()
		{
		}
	}
}
