using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class IndCapacidadTransporteHelper : HelperBase
    {
        public IndCapacidadTransporteHelper() : base(Consultas.IndCapacidadTransporteSql)
        {
        }

        #region Mapeo de Campos
        public string Cpctnscodi = "CPCTNSCODI";
        public string Emprcodi = "EMPRCODI";
        public string IPericodi = "IPERICODI";
        public string Cpctnsusucreacion = "CPCTNSUSUCREACION";
        public string Cpctnsfeccreacion = "CPCTNSFECCREACION";
        public string Cpctnsusumodificacion = "CPCTNSUSUMODIFICACION";
        public string Cpstnsfecmodificacion = "CPCTNSFECMODIFICACION";
        #endregion

        #region Consultas
        public string SqlListCapacidadTransporte
        {
            get { return base.GetSqlXml("ListCapacidadTransporte"); }
        }
        #endregion
    }
}
