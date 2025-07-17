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
    public class ReporteFrecuenciaHelper : HelperBase
    {
        public ReporteFrecuenciaHelper() : base(Consultas.ReporteFrecuenciaSql)
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

        public ReporteFrecuenciaDescargaDTO Create(IDataReader dr)
        {
            ReporteFrecuenciaDescargaDTO entity = new ReporteFrecuenciaDescargaDTO();

            entity.FechaHora = valorReturn(dr, "FECHAHORA")?.ToString();
            entity.ColumnH = valorReturn(dr, "COLUMN_H")?.ToString();
            entity.Valor = valorReturn(dr, "VALUE")?.ToString();
            entity.Frecuencia = valorReturn(dr, "FRECUENCIA")?.ToString();
            entity.Fecha = valorReturn(dr, "FECHA")?.ToString();
            entity.Hora = valorReturn(dr, "HORA")?.ToString();
            return entity;
        }

        public ReporteFrecuenciaDescargaDTO CreateFrecMinuto(IDataReader dr)
        {
            ReporteFrecuenciaDescargaDTO entity = new ReporteFrecuenciaDescargaDTO();

            entity.FechaHora = valorReturn(dr, "FECHAHORA")?.ToString();
            entity.Subita = valorReturn(dr, "VSF")?.ToString();
            entity.Maximo = valorReturn(dr, "MAXIMO")?.ToString();
            entity.Minimo = valorReturn(dr, "MINIMO")?.ToString();
            entity.Tension = valorReturn(dr, "VOLTAJE")?.ToString();
            entity.NumSeg = valorReturn(dr, "NUM")?.ToString();
            entity.Desv = valorReturn(dr, "DESV")?.ToString();
            entity.DevSec = valorReturn(dr, "DEVSEC")?.ToString();
            entity.H0 = valorReturn(dr, "H0")?.ToString();
            return entity;
        }

        public EquipoGPSDTO CreateGPS(IDataReader dr)
        {
            EquipoGPSDTO entity = new EquipoGPSDTO() { GPSCodi = int.Parse(valorReturn(dr, "gpscodi")?.ToString()), NombreEquipo = valorReturn(dr, "nombre") ?.ToString(), GPSOficial = valorReturn(dr, "gpsoficial") ?.ToString() };
            return entity;
        }

        public string SqlGetFrecuencias
        {
            get { return base.GetSqlXml("GetFrecuencias"); }
        }

        public string SqlGetFrecuenciasMinuto
        {
            get { return base.GetSqlXml("GetFrecuenciasMinuto"); }
        }

        public string SqlGetGPSPorRangoFechas
        {
            get { return base.GetSqlXml("GetGPSPorRangoFechas"); }
        }
        public string SqlGetGPSs
        {
            get { return base.GetSqlXml("GetGPSs"); }
        }
        public string SqlFrecuencia
        {
            get { return base.GetSqlXml("Frecuencia"); }
        }
        public string SqlIndicadores
        {
            get { return base.GetSqlXml("Indicadores"); }
        }
        public string SqOcurrencias
        {
            get { return base.GetSqlXml("Ocurrencias"); }
        }
        public string SqlFrecuenciaMin
        {
            get { return base.GetSqlXml("FrecuenciaMin"); }
        }
        public string SqlTransgresionMensual
        {
            get { return base.GetSqlXml("TransgresionMensual"); }
        }
    }
}
