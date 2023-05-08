using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.SmartXml.Conditionals
{
	public abstract class SmartXmlConditional
	{
		public abstract bool ShouldExecute(string param);
	}
}
