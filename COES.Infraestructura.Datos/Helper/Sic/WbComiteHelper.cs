using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_COMITE
    /// </summary>
    public class WbComiteHelper : HelperBase
    {
        public WbComiteHelper(): base(Consultas.WbComiteSql)
        {
        }

        public WbComiteDTO Create(IDataReader dr)
        {
            WbComiteDTO entity = new WbComiteDTO();

            int iComitecodi = dr.GetOrdinal(this.Comitecodi);
            if (!dr.IsDBNull(iComitecodi)) entity.Comitecodi = Convert.ToInt32(dr.GetValue(iComitecodi));

            int iComitename = dr.GetOrdinal(this.Comitename);
            if (!dr.IsDBNull(iComitename)) entity.Comitename = dr.GetString(iComitename);

            int iComiteestado = dr.GetOrdinal(this.Comiteestado);
            if (!dr.IsDBNull(iComiteestado)) entity.Comiteestado = dr.GetString(iComiteestado);

            int iComiteusucreacion = dr.GetOrdinal(this.Comiteusucreacion);
            if (!dr.IsDBNull(iComiteusucreacion)) entity.Comiteusucreacion = dr.GetString(iComiteusucreacion);

            int iComiteusumodificacion = dr.GetOrdinal(this.Comiteusumodificacion);
            if (!dr.IsDBNull(iComiteusumodificacion)) entity.Comiteusumodificacion = dr.GetString(iComiteusumodificacion);

            int iComitefeccreacion = dr.GetOrdinal(this.Comitefeccreacion);
            if (!dr.IsDBNull(iComitefeccreacion)) entity.Comitefeccreacion = dr.GetDateTime(iComitefeccreacion);

            int iComitefecmodificacion = dr.GetOrdinal(this.Comitefecmodificacion);
            if (!dr.IsDBNull(iComitefecmodificacion)) entity.Comitefecmodificacion = dr.GetDateTime(iComitefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Comitecodi = "COMITECODI";
        public string Comitename = "COMITENAME";
        public string Comiteestado = "COMITEESTADO";
        public string Comiteusucreacion = "COMITEUSUCREACION";
        public string Comiteusumodificacion = "COMITEUSUMODIFICACION";
        public string Comitefeccreacion = "COMITEFECCREACION";
        public string Comitefecmodificacion = "COMITEFECMODIFICACION";

        #endregion
    }
}
