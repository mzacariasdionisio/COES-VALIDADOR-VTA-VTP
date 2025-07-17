
using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.ReportesFrecuencia
{
    public class ReportesFrecuenciaAuditHelper : HelperBase
    {
        public ReportesFrecuenciaAuditHelper() : base(Consultas.ReporteFrecuenciaAuditSql)
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
        public static T? ConvertToNull<T>(object x) where T : struct
        {
            return x == null ? null : (T?)Convert.ChangeType(x, typeof(T));
        }
                
        public FrecuenciasAudit Create(IDataReader dr)
        {
            FrecuenciasAudit entity = new FrecuenciasAudit();

            entity.Id = Convert.ToInt32(valorReturn(dr, "Id") ?? 0);
            entity.FechaInicial = Convert.ToDateTime(valorReturn(dr, "FechaInicial") ?? DateTime.MinValue);
            entity.FechaInicialString = valorReturn(dr, "FechaInicial")?.ToString();
            entity.FechaFinal = Convert.ToDateTime(valorReturn(dr, "FechaFinal") ?? DateTime.MinValue);
            entity.FechaFinalString = valorReturn(dr, "FechaFinal")?.ToString();
            //entity.Fecha = Convert.ToDateTime(valorReturn(dr, "Fecha") ?? DateTime.MinValue);
            entity.FechaString = valorReturn(dr, "Fecha")?.ToString();
            entity.Usuario = valorReturn(dr, "Usuario")?.ToString();
            entity.Registros = Convert.ToInt32(valorReturn(dr, "FILASAFECTADAS") ?? 0);
            //entity.FechaReversa = ConvertToNull<DateTime>(valorReturn(dr, "FechaReversa"));
            entity.FechaReversaString = valorReturn(dr, "FechaReversa")?.ToString();
            entity.UsuarioReversa = valorReturn(dr, "UsuarioReversa")?.ToString();
            entity.IdGPS = Convert.ToInt32(valorReturn(dr, "GPScodi") ?? 0);
            return entity;
        }
        #region SQL

        public string GetFrecuenciasAudit
        {
            get { return base.GetSqlXml("GetFrecuenciasAudit"); }
        }
        public string GetFrecuenciaAudit
        {
            get { return base.GetSqlXml("GetFrecuenciaAudit"); }
        }

        public string SQLSave
        {
            get { return base.GetSqlXml("Save"); }
        }
        public string SQLEliminar
        {
            get { return base.GetSqlXml("Eliminar"); }
        }
        #endregion SQL
    }
}
