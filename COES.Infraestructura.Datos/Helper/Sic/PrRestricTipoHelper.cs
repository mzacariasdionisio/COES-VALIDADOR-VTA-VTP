using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_RESTRIC_TIPO
    /// </summary>
    public class PrRestricTipoHelper : HelperBase
    {
        public PrRestricTipoHelper(): base(Consultas.PrRestricTipoSql)
        {
        }

        public PrRestricTipoDTO Create(IDataReader dr)
        {
            PrRestricTipoDTO entity = new PrRestricTipoDTO();

            int iRestipcodi = dr.GetOrdinal(this.Restipcodi);
            if (!dr.IsDBNull(iRestipcodi)) entity.Restipcodi = Convert.ToInt32(dr.GetValue(iRestipcodi));

            int iRestipnombre = dr.GetOrdinal(this.Restipnombre);
            if (!dr.IsDBNull(iRestipnombre)) entity.Restipnombre = dr.GetString(iRestipnombre);

            int iRestipusucreacion = dr.GetOrdinal(this.Restipusucreacion);
            if (!dr.IsDBNull(iRestipusucreacion)) entity.Restipusucreacion = dr.GetString(iRestipusucreacion);

            int iRestipfeccreacion = dr.GetOrdinal(this.Restipfeccreacion);
            if (!dr.IsDBNull(iRestipfeccreacion)) entity.Restipfeccreacion = dr.GetDateTime(iRestipfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Restipcodi = "RESTIPCODI";
        public string Restipnombre = "RESTIPNOMBRE";
        public string Restipusucreacion = "RESTIPUSUCREACION";
        public string Restipfeccreacion = "RESTIPFECCREACION";

        #endregion
    }
}
