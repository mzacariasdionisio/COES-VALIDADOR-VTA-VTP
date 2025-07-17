using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RTU_CONFIGURACION_PERSONA
    /// </summary>
    public class RtuConfiguracionPersonaHelper : HelperBase
    {
        public RtuConfiguracionPersonaHelper() : base(Consultas.RtuConfiguracionPersonaSql)
        {
        }

        public RtuConfiguracionPersonaDTO Create(IDataReader dr)
        {
            RtuConfiguracionPersonaDTO entity = new RtuConfiguracionPersonaDTO();

            int iRtugrucodi = dr.GetOrdinal(this.Rtugrucodi);
            if (!dr.IsDBNull(iRtugrucodi)) entity.Rtugrucodi = Convert.ToInt32(dr.GetValue(iRtugrucodi));

            int iRtupercodi = dr.GetOrdinal(this.Rtupercodi);
            if (!dr.IsDBNull(iRtupercodi)) entity.Rtupercodi = Convert.ToInt32(dr.GetValue(iRtupercodi));

            int iRtuperorden = dr.GetOrdinal(this.Rtuperorden);
            if (!dr.IsDBNull(iRtuperorden)) entity.Rtuperorden = Convert.ToInt32(dr.GetValue(iRtuperorden));

            int iPercodi = dr.GetOrdinal(this.Percodi);
            if (!dr.IsDBNull(iPercodi)) entity.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Rtugrucodi = "RTUGRUCODI";
        public string Rtupercodi = "RTUPERCODI";
        public string Rtuperorden = "RTUPERORDEN";
        public string Percodi = "PERCODI";

        #endregion
    }
}
