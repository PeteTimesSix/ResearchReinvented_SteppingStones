using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.SmartXml.Conditionals
{
	public static class SmartXmlConditionalMaker
	{
		private static Dictionary<Type, SmartXmlConditional> buildConditionals = new Dictionary<Type, SmartXmlConditional>();

		public static SmartXmlConditional MakeConditional(Type conditionalType)
		{
			if (!buildConditionals.ContainsKey(conditionalType))
			{
				SmartXmlConditional picker = (SmartXmlConditional)Activator.CreateInstance(conditionalType);
				buildConditionals[conditionalType] = picker;
			}
			return buildConditionals[conditionalType];
		}
	}
}
