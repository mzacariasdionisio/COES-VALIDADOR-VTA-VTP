using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IEE_RECENERGETICO_TIPO
    /// </summary>
    public class IeeRecenergeticoTipoHelper : HelperBase
    {
        public IeeRecenergeticoTipoHelper(): base(Consultas.IeeRecenergeticoTipoSql)
        {
        }

        public IeeRecenergeticoTipoDTO Create(IDataReader dr)
        {
            IeeRecenergeticoTipoDTO entity = new IeeRecenergeticoTipoDTO();

            int iRenertipcodi = dr.GetOrdinal(this.Renertipcodi);
            if (!dr.IsDBNull(iRenertipcodi)) entity.Renertipcodi = Convert.ToInt32(dr.GetValue(iRenertipcodi));

            int iRenerabrev = dr.GetOrdinal(this.Renerabrev);
            if (!dr.IsDBNull(iRenerabrev)) entity.Renerabrev = dr.GetString(iRenerabrev);

            int iRenertipnomb = dr.GetOrdinal(this.Renertipnomb);
            if (!dr.IsDBNull(iRenertipnomb)) entity.Renertipnomb = dr.GetString(iRenertipnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Renertipcodi = "RENERTIPCODI";
        public string Renerabrev = "RENERABREV";
        public string Renertipnomb = "RENERTIPNOMB";

        #endregion
    }
}
