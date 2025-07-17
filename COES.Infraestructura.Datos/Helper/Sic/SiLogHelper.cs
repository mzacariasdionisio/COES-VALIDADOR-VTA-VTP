using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_LOG
    /// </summary>
    public class SiLogHelper : HelperBase
    {

        public SiLogHelper()
            : base(Consultas.SiLogSql)
        {
        }

        public SiLogDTO Create(IDataReader dr)
        {
            SiLogDTO entity = new SiLogDTO();

            int iLogCodi = dr.GetOrdinal(this.LogCodi);
            if (!dr.IsDBNull(iLogCodi)) entity.LogCodi = Convert.ToInt32(dr.GetValue(iLogCodi));

            int iModCodi = dr.GetOrdinal(this.ModCodi);
            if (!dr.IsDBNull(iModCodi)) entity.ModCodi = Convert.ToInt32(dr.GetValue(iModCodi));

            int iLogDesc = dr.GetOrdinal(this.LogDesc);
            if (!dr.IsDBNull(iLogDesc)) entity.LogDesc = dr.GetString(iLogDesc);

            int iLogFecha = dr.GetOrdinal(this.LogFecha);
            if (!dr.IsDBNull(iLogFecha)) entity.LogFecha = dr.GetDateTime(iLogFecha);

            int iLogUser = dr.GetOrdinal(this.LogUser);
            if (!dr.IsDBNull(iLogUser)) entity.LogUser = dr.GetString(iLogUser);

            int iLogFechaMod = dr.GetOrdinal(this.LogFechaMod);
            if (!dr.IsDBNull(iLogFechaMod)) entity.LogFechaMod = dr.GetDateTime(iLogFechaMod);

            int iLogUserMod = dr.GetOrdinal(this.LogUserMod);
            if (!dr.IsDBNull(iLogUserMod)) entity.LogUserMod = dr.GetString(iLogUserMod);

            return entity;
        }

        #region INTERVENCIONES - REPORTE
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte de historial de actividades del sistema
        /// </summary>
        public SiLogDTO CreateReportHistorial(IDataReader dr)
        {
            SiLogDTO entity = new SiLogDTO();

            int iLogCodi = dr.GetOrdinal(this.LogCodi);
            if (!dr.IsDBNull(iLogCodi)) entity.LogCodi = Convert.ToInt32(dr.GetValue(iLogCodi));

            int iLogDesc = dr.GetOrdinal(this.LogDesc);
            if (!dr.IsDBNull(iLogDesc)) entity.LogDesc = dr.GetString(iLogDesc);

            int iLogFecha = dr.GetOrdinal(this.LogFecha);
            if (!dr.IsDBNull(iLogFecha)) entity.LogFecha = dr.GetDateTime(iLogFecha);

            int iLogUsuCreacion = dr.GetOrdinal(this.LogUser);
            if (!dr.IsDBNull(iLogUsuCreacion)) entity.LogUser = dr.GetString(iLogUsuCreacion);

            return entity;
        }
        #endregion

        #region MAPEO DE CAMPOS
        public string LogCodi = "LOGCODI";
        public string ModCodi = "MODCODI";
        public string LogDesc = "LOGDESC";

        //public string LogFecha = "LOGFECHA";
        //public string LogUser = "LOGUSER";
        //public string LogFechaMod = "LOGFECHAMOD";
        //public string LogUserMod = "LOGUSERMOD";

        public string LogFecha = "LOGFECCREACION";
        public string LogUser = "LOGUSUCREACION";
        public string LogFechaMod = "LOGFECMODIFICACION";
        public string LogUserMod = "LOGUSUMODIFICACION";
        #endregion

        #region Campos para paginado
        public string NROPAGINA = "NROPAGINA";
        public string PAGESIZE = "PAGESIZE";
        #endregion

        public string SqlGetMaxId
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListar
        {
            get { return base.GetSqlXml("Listar"); }
        }

        public string SqlSave
        {
            get { return base.GetSqlXml("Save"); }
        }

        public string SqlUpdate
        {
            get { return base.GetSqlXml("Update"); }
        }
        #region Transferencia de Equipos
        public string SqlListLogByMigracion
        {
            get { return base.GetSqlXml("ListLogByMigracion"); }
        }

        public string SqlSaveTransferencia
        {
            get { return base.GetSqlXml("SaveTransferencia"); }
        }

        public string Logmigtipo = "LOGMIGTIPO";
        public string Miqubamensaje = "MIQUBAMENSAJE";
        public string Miqubaflag = "MIQUBAFLAG";

        #endregion

        #region INTERVENCIONES - QUERYS
        public string SqlRptHistorialIntervenciones
        {
            get { return base.GetSqlXml("RptHistorialIntervenciones"); }
        }
        #endregion
    }
}
