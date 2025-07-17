using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMPO_TIPO_OBRA
    /// </summary>
    public class PmpoTipoObraHelper : HelperBase
    {
        public PmpoTipoObraHelper(): base(Consultas.PmpoTipoObraSql)
        {
        }

        public PmpoTipoObraDTO Create(IDataReader dr)
        {
            PmpoTipoObraDTO entity = new PmpoTipoObraDTO();

            int iTObracodi = dr.GetOrdinal(this.TObracodi);
            if (!dr.IsDBNull(iTObracodi)) entity.TObracodi = Convert.ToInt32(dr.GetValue(iTObracodi));

            int iTObradescipcion = dr.GetOrdinal(this.TObradescripcion);
            if (!dr.IsDBNull(iTObradescipcion)) entity.TObradescripcion = dr.GetString(iTObradescipcion);

            int iTObrausucreacion = dr.GetOrdinal(this.TObrausucreacion);
            if (!dr.IsDBNull(iTObrausucreacion)) entity.TObrausucreacion = dr.GetString(iTObrausucreacion);

            int iTObrafeccreacion = dr.GetOrdinal(this.TObrafeccreacion);
            if (!dr.IsDBNull(iTObrafeccreacion)) entity.TObrafeccreacion = dr.GetDateTime(iTObrafeccreacion);

            int iTObrausumodificacion = dr.GetOrdinal(this.TObrausumodificacion);
            if (!dr.IsDBNull(iTObrausumodificacion)) entity.TObrausumodificacion = dr.GetString(iTObrausumodificacion);

            int iTObrafecmodificacion = dr.GetOrdinal(this.TObrafecmodificacion);
            if (!dr.IsDBNull(iTObrafecmodificacion)) entity.TObrafecmodificacion = dr.GetDateTime(iTObrafecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string TObracodi = "TOBRACODI";
        public string TObradescripcion = "TOBRADESCRIPCION";
        public string TObrausucreacion = "TOBRAUSUCREACION";
        public string TObrafeccreacion = "TOBRAFECCREACION";
        public string TObrausumodificacion = "TOBRAUSUMODIFICACION";
        public string TObrafecmodificacion = "TOBRAFECMODIFICACION";

        #endregion

    }
}
