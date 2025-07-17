using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamTipoproyectoHelper : HelperBase
    {

        public CamTipoproyectoHelper() : base(Consultas.CamTipoproyectoSql) { }

        public string SqlGetTipoProyecto
        {
            get { return base.GetSqlXml("GetTipoProyecto"); }
        }


    }
}
