using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_PROVEEDOR
    /// </summary>
    public class WbProveedorHelper : HelperBase
    {
        public WbProveedorHelper(): base(Consultas.WbProveedorSql)
        {
        }

        public WbProveedorDTO Create(IDataReader dr)
        {
            WbProveedorDTO entity = new WbProveedorDTO();

            int iProvcodi = dr.GetOrdinal(this.Provcodi);
            if (!dr.IsDBNull(iProvcodi)) entity.Provcodi = Convert.ToInt32(dr.GetValue(iProvcodi));

            int iProvnombre = dr.GetOrdinal(this.Provnombre);
            if (!dr.IsDBNull(iProvnombre)) entity.Provnombre = dr.GetString(iProvnombre);

            int iProvtipo = dr.GetOrdinal(this.Provtipo);
            if (!dr.IsDBNull(iProvtipo)) entity.Provtipo = dr.GetString(iProvtipo);

            int iProvfechainscripcion = dr.GetOrdinal(this.Provfechainscripcion);
            if (!dr.IsDBNull(iProvfechainscripcion)) entity.Provfechainscripcion = dr.GetDateTime(iProvfechainscripcion);

            return entity;
        }


        #region Mapeo de Campos

        public string Provcodi = "PROVCODI";
        public string Provnombre = "PROVNOMBRE";
        public string Provtipo = "PROVTIPO";
        public string Provfechainscripcion = "PROVFECHAINSCRIPCION";

        #endregion

        public string SqlListarTipoProveedor
        {
            get { return base.GetSqlXml("GetByCriteriaTipo"); }
        }
    }
}
