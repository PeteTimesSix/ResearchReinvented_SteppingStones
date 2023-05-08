using PeteTimesSix.ResearchReinvented_SteppingStones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.SmartXml.Conditionals
{
	public class Cond_Power : SmartXmlConditional
	{
		public override bool ShouldExecute(string param)
		{
			return ResearchReinvented_SteppingStonesMod.Settings.doPowerPreregs;
		}
	}
}
