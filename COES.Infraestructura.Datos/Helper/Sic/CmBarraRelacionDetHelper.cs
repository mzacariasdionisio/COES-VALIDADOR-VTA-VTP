using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_BARRA_RELACION_DET
    /// </summary>
    public class CmBarraRelacionDetHelper : HelperBase
    {
        public CmBarraRelacionDetHelper(): base(Consultas.CmBarraRelacionDetSql)
        {
        }

        public CmBarraRelacionDetDTO Create(IDataReader dr)
        {
            CmBarraRelacionDetDTO entity = new CmBarraRelacionDetDTO();

            int iCmbadecodi = dr.GetOrdinal(this.Cmbadecodi);
            if (!dr.IsDBNull(iCmbadecodi)) entity.Cmbadecodi = Convert.ToInt32(dr.GetValue(iCmbadecodi));

            int iCmbarecodi = dr.GetOrdinal(this.Cmbarecodi);
            if (!dr.IsDBNull(iCmbarecodi)) entity.Cmbarecodi = Convert.ToInt32(dr.GetValue(iCmbarecodi));

            int iCnfbarcodi = dr.GetOrdinal(this.Cnfbarcodi);
            if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));

            int iCmbadeestado = dr.GetOrdinal(this.Cmbadeestado);
            if (!dr.IsDBNull(iCmbadeestado)) entity.Cmbadeestado = dr.GetString(iCmbadeestado);

            int iCmbadeusucreacion = dr.GetOrdinal(this.Cmbadeusucreacion);
            if (!dr.IsDBNull(iCmbadeusucreacion)) entity.Cmbadeusucreacion = dr.GetString(iCmbadeusucreacion);

            int iCmbadefeccreacion = dr.GetOrdinal(this.Cmbadefeccreacion);
            if (!dr.IsDBNull(iCmbadefeccreacion)) entity.Cmbadefeccreacion = dr.GetDateTime(iCmbadefeccreacion);

            int iCmbadeusumodificacion = dr.GetOrdinal(this.Cmbadeusumodificacion);
            if (!dr.IsDBNull(iCmbadeusumodificacion)) entity.Cmbadeusumodificacion = dr.GetString(iCmbadeusumodificacion);

            int iCmbadefecmodificacion = dr.GetOrdinal(this.Cmbadefecmodificacion);
            if (!dr.IsDBNull(iCmbadefecmodificacion)) entity.Cmbadefecmodificacion = dr.GetDateTime(iCmbadefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmbadecodi = "CMBADECODI";
        public string Cmbarecodi = "CMBARECODI";
        public string Cnfbarcodi = "CNFBARCODI";
        public string Cmbadeestado = "CMBADEESTADO";
        public string Cmbadeusucreacion = "CMBADEUSUCREACION";
        public string Cmbadefeccreacion = "CMBADEFECCREACION";
        public string Cmbadeusumodificacion = "CMBADEUSUMODIFICACION";
        public string Cmbadefecmodificacion = "CMBADEFECMODIFICACION";
        public string Cnfbarnombre = "CNFBARNOMBRE";

        #endregion
    }
}
