using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs
{
	[DefOf]
	public static class ConceptDefOf_Custom
	{
		public static ConceptDef RR_SteppingStones_Intro;

		static ConceptDefOf_Custom()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ConceptDefOf_Custom));
		}
	}
}
