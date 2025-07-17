using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMPO_CONFIGURACION
    /// </summary>
    public class PmpoReportOsinergHelper : HelperBase
    {
        public PmpoReportOsinergHelper()
            : base(Consultas.PmpoReportOsinergSql)
        {
        }

        public PmpoReportOsinergDTO Create(IDataReader dr)
        {
            PmpoReportOsinergDTO entity = new PmpoReportOsinergDTO();

            int iRepcodi = dr.GetOrdinal(this.Repcodi);
            if (!dr.IsDBNull(iRepcodi)) entity.Repcodi = Convert.ToInt32(dr.GetValue(iRepcodi));

            int iRepdescripcion = dr.GetOrdinal(this.Repdescripcion);
            if (!dr.IsDBNull(iRepdescripcion)) entity.Repdescripcion = dr.GetString(iRepdescripcion);

            int iRepfecha = dr.GetOrdinal(this.Repfecha);
            if (!dr.IsDBNull(iRepfecha)) entity.Repfecha = dr.GetDateTime(iRepfecha);

            int iRepmeselaboracion = dr.GetOrdinal(this.Repmeselaboracion);
            if (!dr.IsDBNull(iRepmeselaboracion)) entity.Repmeselaboracion = dr.GetString(iRepmeselaboracion);

            int iRepusucreacion = dr.GetOrdinal(this.Repusucreacion);
            if (!dr.IsDBNull(iRepusucreacion)) entity.Repusucreacion = dr.GetString(iRepusucreacion);

            int iRepfeccreacion = dr.GetOrdinal(this.Repfeccreacion);
            if (!dr.IsDBNull(iRepfeccreacion)) entity.Repfeccreacion = dr.GetDateTime(iRepfeccreacion);

            int iRepusumodificacion = dr.GetOrdinal(this.Repusumodificacion);
            if (!dr.IsDBNull(iRepusumodificacion)) entity.Repusumodificacion = dr.GetString(iRepusumodificacion);

            int iRepfecmodificacion = dr.GetOrdinal(this.Repfecmodificacion);
            if (!dr.IsDBNull(iRepfecmodificacion)) entity.Repfecmodificacion = dr.GetDateTime(iRepfecmodificacion);

            int iRepestado = dr.GetOrdinal(this.Repestado);
            if (!dr.IsDBNull(iRepestado)) entity.Repestado = dr.GetString(iRepestado);


            return entity;
        }

        #region Mapeo de Campos

        public string Repcodi = "REPCODI";
        public string Repdescripcion = "REPDESCRIPCION";
        public string Repfecha = "REPFECHA";
        public string Repmeselaboracion = "REPMESELABORACION";
        public string Repusucreacion = "REPUSUCREACION";
        public string Repfeccreacion = "REPFECCREACION";
        public string Repusumodificacion = "REPUSUMODIFICACION";
        public string Repfecmodificacion = "REPFECMODIFICACION";
        public string Repestado = "REPESTADO";

        #endregion

    }
}
