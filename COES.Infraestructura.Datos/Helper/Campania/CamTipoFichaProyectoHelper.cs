using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamTipoFichaProyectoHelper : HelperBase
    { 
        public CamTipoFichaProyectoHelper() : base(Consultas.CamTipoFichaProyectoSql) { }

        public string SqlGetTipoFichaProyecto
        {
            get { return base.GetSqlXml("GetTipoFichaProyecto"); }
        }



    }
}
