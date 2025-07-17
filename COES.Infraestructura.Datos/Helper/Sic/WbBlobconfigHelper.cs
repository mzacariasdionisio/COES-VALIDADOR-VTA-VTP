using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_BLOBCONFIG
    /// </summary>
    public class WbBlobconfigHelper : HelperBase
    {
        public WbBlobconfigHelper() : base(Consultas.WbBlobconfigSql)
        {
        }

        public WbBlobconfigDTO Create(IDataReader dr)
        {
            WbBlobconfigDTO entity = new WbBlobconfigDTO();

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iUsercreate = dr.GetOrdinal(this.Usercreate);
            if (!dr.IsDBNull(iUsercreate)) entity.Usercreate = dr.GetString(iUsercreate);

            int iDatecreate = dr.GetOrdinal(this.Datecreate);
            if (!dr.IsDBNull(iDatecreate)) entity.Datecreate = dr.GetDateTime(iDatecreate);

            int iUserupdate = dr.GetOrdinal(this.Userupdate);
            if (!dr.IsDBNull(iUserupdate)) entity.Userupdate = dr.GetString(iUserupdate);

            int iDateupdate = dr.GetOrdinal(this.Dateupdate);
            if (!dr.IsDBNull(iDateupdate)) entity.Dateupdate = dr.GetDateTime(iDateupdate);

            int iConfigname = dr.GetOrdinal(this.Configname);
            if (!dr.IsDBNull(iConfigname)) entity.Configname = dr.GetString(iConfigname);

            int iConfigstate = dr.GetOrdinal(this.Configstate);
            if (!dr.IsDBNull(iConfigstate)) entity.Configstate = dr.GetString(iConfigstate);

            int iConfigdefault = dr.GetOrdinal(this.Configdefault);
            if (!dr.IsDBNull(iConfigdefault)) entity.Configdefault = dr.GetString(iConfigdefault);

            int iConfigorder = dr.GetOrdinal(this.Configorder);
            if (!dr.IsDBNull(iConfigorder)) entity.Configorder = dr.GetString(iConfigorder);

            int iConfigespecial = dr.GetOrdinal(this.Configespecial);
            if (!dr.IsDBNull(iConfigespecial)) entity.Configespecial = dr.GetString(iConfigespecial);

            int iColumncodi = dr.GetOrdinal(this.Columncodi);
            if (!dr.IsDBNull(iColumncodi)) entity.Columncodi = Convert.ToInt32(dr.GetValue(iColumncodi));

            int iBlofuecodi = dr.GetOrdinal(this.Blofuecodi);
            if (!dr.IsDBNull(iBlofuecodi)) entity.Blofuecodi = Convert.ToInt32(dr.GetValue(iBlofuecodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Configcodi = "CONFIGCODI";
        public string Usercreate = "USERCREATE";
        public string Datecreate = "DATECREATE";
        public string Userupdate = "USERUPDATE";
        public string Dateupdate = "DATEUPDATE";
        public string Configname = "CONFIGNAME";
        public string Configstate = "CONFIGSTATE";
        public string Configdefault = "CONFIGDEFAULT";
        public string Configorder = "CONFIGORDER";
        public string Configespecial = "CONFIGESPECIAL";
        public string Columncodi = "COLUMNCODI";
        public string Blofuecodi = "BLOFUECODI";

        #endregion
    }
}
