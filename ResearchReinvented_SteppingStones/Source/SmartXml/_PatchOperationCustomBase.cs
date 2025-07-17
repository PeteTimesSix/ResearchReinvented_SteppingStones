using RR.SmartXml.Conditionals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RR
{
    public abstract class PatchOperationCustomBase : PatchOperationPathed
    {
        public Type conditionalType = null;
        public string conditionalParam = null;
        public string doesRequire;

        public bool Skip() 
        {
            if (!string.IsNullOrWhiteSpace(doesRequire))
            {
                var split = doesRequire.Split(',');
                foreach (var mod in split)
                {
                    if (!ModsConfig.IsActive(mod.Trim()))
                    {
                        return true; //a required mod isnt loaded, so we dont apply the patch
                    }
                }
            }

            if (conditionalType != null)
            {
                var conditional = SmartXmlConditionalMaker.MakeConditional(conditionalType);
                if (!conditional.ShouldExecute(conditionalParam))
                    return true; //specific condition (probably a settings value) not met
            }
            return false;
        }
    }
}
