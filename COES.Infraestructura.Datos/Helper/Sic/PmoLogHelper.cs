using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_LOG
    /// </summary>
    public class PmoLogHelper : HelperBase
    {
        public PmoLogHelper() : base(Consultas.PmoLogSql)
        {
        }

        public PmoLogDTO Create(IDataReader dr)
        {
            PmoLogDTO entity = new PmoLogDTO();

            int iPmologcodi = dr.GetOrdinal(this.Pmologcodi);
            if (!dr.IsDBNull(iPmologcodi)) entity.Pmologcodi = Convert.ToInt32(dr.GetValue(iPmologcodi));

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iLogcodi = dr.GetOrdinal(this.Logcodi);
            if (!dr.IsDBNull(iLogcodi)) entity.Logcodi = Convert.ToInt32(dr.GetValue(iLogcodi));

            int iPmologtipo = dr.GetOrdinal(this.Pmologtipo);
            if (!dr.IsDBNull(iPmologtipo)) entity.Pmologtipo = Convert.ToInt32(dr.GetValue(iPmologtipo));

            int iPmftabcodi = dr.GetOrdinal(this.Pmftabcodi);
            if (!dr.IsDBNull(iPmftabcodi)) entity.Pmftabcodi = Convert.ToInt32(dr.GetValue(iPmftabcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Pmologcodi = "PMOLOGCODI";
        public string Enviocodi = "ENVIOCODI";
        public string Logcodi = "LOGCODI";
        public string Pmologtipo = "PMOLOGTIPO";
        public string Pmftabcodi = "PMFTABCODI";

        #endregion

        public string LogFecha = "LOGFECCREACION";
        public string LogUser = "LOGUSUCREACION";
        public string LogDesc = "LOGDESC";
    }
}
