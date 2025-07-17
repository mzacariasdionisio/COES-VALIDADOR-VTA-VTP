using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_BARRAGENERADOR
    /// </summary>
    public class CmBarrageneradorHelper : HelperBase
    {
        public CmBarrageneradorHelper(): base(Consultas.CmBarrageneradorSql)
        {
        }

        public CmBarrageneradorDTO Create(IDataReader dr)
        {
            CmBarrageneradorDTO entity = new CmBarrageneradorDTO();

            int iBargercodi = dr.GetOrdinal(this.Bargercodi);
            if (!dr.IsDBNull(iBargercodi)) entity.Bargercodi = Convert.ToInt32(dr.GetValue(iBargercodi));

            int iRelacioncodi = dr.GetOrdinal(this.Relacioncodi);
            if (!dr.IsDBNull(iRelacioncodi)) entity.Relacioncodi = Convert.ToInt32(dr.GetValue(iRelacioncodi));

            int iCnfbarcodi = dr.GetOrdinal(this.Cnfbarcodi);
            if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));

            int iBargerfecha = dr.GetOrdinal(this.Bargerfecha);
            if (!dr.IsDBNull(iBargerfecha)) entity.Bargerfecha = dr.GetDateTime(iBargerfecha);

            int iBargerusucreacion = dr.GetOrdinal(this.Bargerusucreacion);
            if (!dr.IsDBNull(iBargerusucreacion)) entity.Bargerusucreacion = dr.GetString(iBargerusucreacion);

            int iBargerfeccreacion = dr.GetOrdinal(this.Bargerfeccreacion);
            if (!dr.IsDBNull(iBargerfeccreacion)) entity.Bargerfeccreacion = dr.GetDateTime(iBargerfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Bargercodi = "BARGERCODI";
        public string Relacioncodi = "RELACIONCODI";
        public string Cnfbarcodi = "CNFBARCODI";
        public string Bargerfecha = "BARGERFECHA";
        public string Bargerusucreacion = "BARGERUSUCREACION";
        public string Bargerfeccreacion = "BARGERFECCREACION";

        #endregion
    }
}
