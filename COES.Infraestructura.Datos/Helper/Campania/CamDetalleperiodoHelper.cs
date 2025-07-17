using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDetallePeriodoHelper : HelperBase
    {
       public CamDetallePeriodoHelper() : base(Consultas.CamDetallePeriodoSql) { }
        public string SQLSaveDetalle
        {
            get { return base.GetSqlXml("SaveDetalle"); }
        }
        public string SqlGetDetalleId
        {
            get { return base.GetSqlXml("GetLastDetPeriID"); }
        }

        public string SqlGetDetalleByPericodi
        {
            get { return base.GetSqlXml("GetDetPeriodoById"); }
        }

        public string SqlDeleteDetalleById
        {
            get { return base.GetSqlXml("DeleteDetPeriodoById"); }
        }

    }
}
