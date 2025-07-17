using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamHojaFichaHelper :HelperBase
    {
        public CamHojaFichaHelper() : base(Consultas.CamHojaFichaSql){ }

        public string SqlGetFicha
        {
            get { return base.GetSqlXml("GetHojaFicha"); }
        }


    }
}
