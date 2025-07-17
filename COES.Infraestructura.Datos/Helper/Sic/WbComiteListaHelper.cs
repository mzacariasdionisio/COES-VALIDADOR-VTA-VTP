using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_COMITE_LISTA
    /// </summary>
    public class WbComiteListaHelper : HelperBase
    {
        public WbComiteListaHelper() : base(Consultas.WbComiteListaSql)
        {
        }

        public WbComiteListaDTO Create(IDataReader dr)
        {
            WbComiteListaDTO entity = new WbComiteListaDTO();

            int iComitecodi = dr.GetOrdinal(this.Comitecodi);
            if (!dr.IsDBNull(iComitecodi)) entity.Comitecodi = Convert.ToInt32(dr.GetValue(iComitecodi));

            int iComitelistacodi = dr.GetOrdinal(this.Comitelistacodi);
            if (!dr.IsDBNull(iComitelistacodi)) entity.Comitelistacodi = Convert.ToInt32(dr.GetValue(iComitelistacodi));

            int iComitelistaname = dr.GetOrdinal(this.Comitelistaname);
            if (!dr.IsDBNull(iComitelistaname)) entity.Comitelistaname = dr.GetString(iComitelistaname);

            int iComitelistaestado = dr.GetOrdinal(this.Comitelistaestado);
            if (!dr.IsDBNull(iComitelistaestado)) entity.Comitelistaestado = dr.GetString(iComitelistaestado);

            int iComitelistausucreacion = dr.GetOrdinal(this.Comitelistausucreacion);
            if (!dr.IsDBNull(iComitelistausucreacion)) entity.Comitelistausucreacion = dr.GetString(iComitelistausucreacion);

            int iComitelistafeccreacion = dr.GetOrdinal(this.Comitelistafeccreacion);
            if (!dr.IsDBNull(iComitelistafeccreacion)) entity.Comitelistafeccreacion = dr.GetDateTime(iComitelistafeccreacion);


            return entity;
        }


        #region Mapeo de Campos

        public string Comitecodi = "COMITECODI";
        public string Comitelistacodi = "COMITELISTACODI";
        public string Comitelistaname = "COMITELISTANAME";
        public string Comitelistaestado = "COMITELISTAESTADO";
        public string Comitelistausucreacion = "COMITELISTAUSUCREACION";
        public string Comitelistafeccreacion = "COMITELISTAFECCREACION";

        #endregion

        public string SqlGetListaByComite
        {
            get { return base.GetSqlXml("ListByComite"); }
        }
    }
}
