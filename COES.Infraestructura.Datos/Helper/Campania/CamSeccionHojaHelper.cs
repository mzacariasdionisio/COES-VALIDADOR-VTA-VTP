using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamSeccionHojaHelper : HelperBase
    {

        public CamSeccionHojaHelper() : base(Consultas.CamSeccionHojaSql) { }

        public string SqlGetSeccionHojaHojaCodi
        {
            get { return base.GetSqlXml("GetSeccionHojaHojaCodi"); }
        }

    }
}
