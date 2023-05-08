using PeteTimesSix.ResearchReinvented_SteppingStones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RR.SmartXml.Conditionals
{
	public class Cond_Surgery : SmartXmlConditional
	{
		public override bool ShouldExecute(string param)
		{
			string[] paramSplit = param.Split(',');
			foreach(var flag in paramSplit) 
			{
				switch (flag.ToLowerInvariant())
				{
					case "off":
						if (ResearchReinvented_SteppingStonesMod.Settings.surgeryPreregsMode.HasFlag(SurgeryPreregsMode.Off))
							return true;
						break;
					case "easy":
						if (ResearchReinvented_SteppingStonesMod.Settings.surgeryPreregsMode.HasFlag(SurgeryPreregsMode.Easy))
							return true;
						break;
					case "normal":
						if (ResearchReinvented_SteppingStonesMod.Settings.surgeryPreregsMode.HasFlag(SurgeryPreregsMode.Normal))
							return true;
						break;
				}
			}
			return false;
		}
	}
}
