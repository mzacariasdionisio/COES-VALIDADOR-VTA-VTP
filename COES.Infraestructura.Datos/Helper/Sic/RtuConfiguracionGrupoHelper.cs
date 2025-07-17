using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RTU_CONFIGURACION_GRUPO
    /// </summary>
    public class RtuConfiguracionGrupoHelper : HelperBase
    {
        public RtuConfiguracionGrupoHelper() : base(Consultas.RtuConfiguracionGrupoSql)
        {
        }

        public RtuConfiguracionGrupoDTO Create(IDataReader dr)
        {
            RtuConfiguracionGrupoDTO entity = new RtuConfiguracionGrupoDTO();

            int iRtugrucodi = dr.GetOrdinal(this.Rtugrucodi);
            if (!dr.IsDBNull(iRtugrucodi)) entity.Rtugrucodi = Convert.ToInt32(dr.GetValue(iRtugrucodi));

            int iRtugruindreporte = dr.GetOrdinal(this.Rtugruindreporte);
            if (!dr.IsDBNull(iRtugruindreporte)) entity.Rtugruindreporte = dr.GetString(iRtugruindreporte);

            int iRtugruorden = dr.GetOrdinal(this.Rtugruorden);
            if (!dr.IsDBNull(iRtugruorden)) entity.Rtugruorden = Convert.ToInt32(dr.GetValue(iRtugruorden));

            int iRtuconcodi = dr.GetOrdinal(this.Rtuconcodi);
            if (!dr.IsDBNull(iRtuconcodi)) entity.Rtuconcodi = Convert.ToInt32(dr.GetValue(iRtuconcodi));

            int iRtugrutipo = dr.GetOrdinal(this.Rtugrutipo);
            if (!dr.IsDBNull(iRtugrutipo)) entity.Rtugrutipo = dr.GetString(iRtugrutipo);

            return entity;
        }

        #region Mapeo de Campos

        public string Rtugrucodi = "RTUGRUCODI";
        public string Rtugruindreporte = "RTUGRUINDREPORTE";
        public string Rtugruorden = "RTUGRUORDEN";
        public string Rtuconcodi = "RTUCONCODI";
        public string Rtugrutipo = "RTUGRUTIPO";

        #endregion
    }
}
