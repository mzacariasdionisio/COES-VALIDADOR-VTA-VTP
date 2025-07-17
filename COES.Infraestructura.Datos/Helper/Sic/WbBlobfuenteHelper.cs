using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_BLOBFUENTE
    /// </summary>
    public class WbBlobfuenteHelper : HelperBase
    {
        public WbBlobfuenteHelper() : base(Consultas.WbBlobfuenteSql)
        {
        }

        public WbBlobfuenteDTO Create(IDataReader dr)
        {
            WbBlobfuenteDTO entity = new WbBlobfuenteDTO();

            int iBlofuecodi = dr.GetOrdinal(this.Blofuecodi);
            if (!dr.IsDBNull(iBlofuecodi)) entity.Blofuecodi = Convert.ToInt32(dr.GetValue(iBlofuecodi));

            int iBlofuenomb = dr.GetOrdinal(this.Blofuenomb);
            if (!dr.IsDBNull(iBlofuenomb)) entity.Blofuenomb = dr.GetString(iBlofuenomb);

            int iBlofueestado = dr.GetOrdinal(this.Blofueestado);
            if (!dr.IsDBNull(iBlofueestado)) entity.Blofueestado = dr.GetString(iBlofueestado);

            int iBlofueusucreacion = dr.GetOrdinal(this.Blofueusucreacion);
            if (!dr.IsDBNull(iBlofueusucreacion)) entity.Blofueusucreacion = dr.GetString(iBlofueusucreacion);

            int iBlofuefeccreacion = dr.GetOrdinal(this.Blofuefeccreacion);
            if (!dr.IsDBNull(iBlofuefeccreacion)) entity.Blofuefeccreacion = dr.GetDateTime(iBlofuefeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Blofuecodi = "BLOFUECODI";
        public string Blofuenomb = "BLOFUENOMB";
        public string Blofueestado = "BLOFUEESTADO";
        public string Blofueusucreacion = "BLOFUEUSUCREACION";
        public string Blofuefeccreacion = "BLOFUEFECCREACION";

        #endregion
    }
}
