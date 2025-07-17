using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RTU_ROLTURNO_DETALLE
    /// </summary>
    public class RtuRolturnoDetalleHelper : HelperBase
    {
        public RtuRolturnoDetalleHelper() : base(Consultas.RtuRolturnoDetalleSql)
        {
        }

        public RtuRolturnoDetalleDTO Create(IDataReader dr)
        {
            RtuRolturnoDetalleDTO entity = new RtuRolturnoDetalleDTO();

            int iRtudetcodi = dr.GetOrdinal(this.Rtudetcodi);
            if (!dr.IsDBNull(iRtudetcodi)) entity.Rtudetcodi = Convert.ToInt32(dr.GetValue(iRtudetcodi));

            int iRtudetnrodia = dr.GetOrdinal(this.Rtudetnrodia);
            if (!dr.IsDBNull(iRtudetnrodia)) entity.Rtudetnrodia = Convert.ToInt32(dr.GetValue(iRtudetnrodia));

            int iRtudetmodtrabajo = dr.GetOrdinal(this.Rtudetmodtrabajo);
            if (!dr.IsDBNull(iRtudetmodtrabajo)) entity.Rtudetmodtrabajo = dr.GetString(iRtudetmodtrabajo);

            int iRturolcodi = dr.GetOrdinal(this.Rturolcodi);
            if (!dr.IsDBNull(iRturolcodi)) entity.Rturolcodi = Convert.ToInt32(dr.GetValue(iRturolcodi));

            int iPercodi = dr.GetOrdinal(this.Percodi);
            if (!dr.IsDBNull(iPercodi)) entity.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Rtudetcodi = "RTUDETCODI";
        public string Rtudetnrodia = "RTUDETNRODIA";
        public string Rtudetmodtrabajo = "RTUDETMODTRABAJO";
        public string Rturolcodi = "RTUROLCODI";
        public string Percodi = "PERCODI";

        #endregion
    }
}
