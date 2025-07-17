using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.ReportesFrecuencia
{
    public class ReporteSegundosFaltantesHelper : HelperBase
    {
        public ReporteSegundosFaltantesHelper() : base(Consultas.ReporteSegundosFaltantesSql)
        {

        }
        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        private object valorReturn(IDataReader dr, string sColumna)
        {
            object resultado = null;
            int iIndex;
            if (columnsExist(sColumna, dr))
            {
                iIndex = dr.GetOrdinal(sColumna);
                if (!dr.IsDBNull(iIndex))
                    resultado = dr.GetValue(iIndex);
            }
            return resultado?.ToString();
        }

        public ReporteSegundosFaltantesDTO Create(IDataReader dr)
        {
            ReporteSegundosFaltantesDTO entity = new ReporteSegundosFaltantesDTO();
            entity.FechaHora = valorReturn(dr, "FECHAHORA")?.ToString();
            entity.GPSCodi = Convert.ToInt32(valorReturn(dr, "GPSCODI") ?? 0);
            entity.Num = Convert.ToInt32(valorReturn(dr, "NUM") ?? 0);
            return entity;
        }
        public string SqlGetReporteSegundosFaltantes
        {
            get { return base.GetSqlXml("GetReporteSegundosFaltantes"); }
        }

        public string SqlGetReporteTotalSegundosFaltantes
        {
            get { return base.GetSqlXml("GetReporteTotalSegundosFaltantes"); }
        }

    }
}

