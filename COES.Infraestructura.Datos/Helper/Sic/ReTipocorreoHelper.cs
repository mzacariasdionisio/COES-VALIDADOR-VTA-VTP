using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_TIPOCORREO
    /// </summary>
    public class ReTipocorreoHelper : HelperBase
    {
        public ReTipocorreoHelper(): base(Consultas.ReTipocorreoSql)
        {
        }

        public ReTipocorreoDTO Create(IDataReader dr)
        {
            ReTipocorreoDTO entity = new ReTipocorreoDTO();

            int iRetcorcodi = dr.GetOrdinal(this.Retcorcodi);
            if (!dr.IsDBNull(iRetcorcodi)) entity.Retcorcodi = Convert.ToInt32(dr.GetValue(iRetcorcodi));

            int iRetcornombre = dr.GetOrdinal(this.Retcornombre);
            if (!dr.IsDBNull(iRetcornombre)) entity.Retcornombre = dr.GetString(iRetcornombre);

            int iRetcorusucreacion = dr.GetOrdinal(this.Retcorusucreacion);
            if (!dr.IsDBNull(iRetcorusucreacion)) entity.Retcorusucreacion = dr.GetString(iRetcorusucreacion);

            int iRetcorfeccreacion = dr.GetOrdinal(this.Retcorfeccreacion);
            if (!dr.IsDBNull(iRetcorfeccreacion)) entity.Retcorfeccreacion = dr.GetDateTime(iRetcorfeccreacion);

            int iRetcorusumodificacion = dr.GetOrdinal(this.Retcorusumodificacion);
            if (!dr.IsDBNull(iRetcorusumodificacion)) entity.Retcorusumodificacion = dr.GetString(iRetcorusumodificacion);

            int iRetcorfecmodificacion = dr.GetOrdinal(this.Retcorfecmodificacion);
            if (!dr.IsDBNull(iRetcorfecmodificacion)) entity.Retcorfecmodificacion = dr.GetDateTime(iRetcorfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Retcorcodi = "RETCORCODI";
        public string Retcornombre = "RETCORNOMBRE";
        public string Retcorusucreacion = "RETCORUSUCREACION";
        public string Retcorfeccreacion = "RETCORFECCREACION";
        public string Retcorusumodificacion = "RETCORUSUMODIFICACION";
        public string Retcorfecmodificacion = "RETCORFECMODIFICACION";

        #endregion
    }
}
