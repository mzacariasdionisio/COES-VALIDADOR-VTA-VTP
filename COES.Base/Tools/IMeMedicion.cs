using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Framework.Base.Tools
{
    public interface IMeMedicion
    { 
        int Ptomedicodi { get; set; }
        DateTime Medifecha { get; set; }
    }
}
