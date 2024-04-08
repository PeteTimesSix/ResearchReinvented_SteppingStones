using RR.SmartXml.Conditionals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RR.SmartXml.Conditionals
{
    public class Cond_ModsNotLoaded : SmartXmlConditional
    {
        public override bool ShouldExecute(string param)
        {
            string[] paramSplit = param.Split(',');
            foreach (var modId in paramSplit)
            {
                if (ModsConfig.IsActive(modId.Trim()))
                    return false;
            }
            return true;
        }
    }
}
