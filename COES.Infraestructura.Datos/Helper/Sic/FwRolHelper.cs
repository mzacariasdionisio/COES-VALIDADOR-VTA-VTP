using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FW_ROL
    /// </summary>
    public class FwRolHelper : HelperBase
    {
        public FwRolHelper(): base(Consultas.FwRolSql)
        {
        }

        public FwRolDTO Create(IDataReader dr)
        {
            FwRolDTO entity = new FwRolDTO();

            int iRolcode = dr.GetOrdinal(this.Rolcode);
            if (!dr.IsDBNull(iRolcode)) entity.Rolcode = Convert.ToInt32(dr.GetValue(iRolcode));

            int iRolname = dr.GetOrdinal(this.Rolname);
            if (!dr.IsDBNull(iRolname)) entity.Rolname = dr.GetString(iRolname);

            int iRolvalidate = dr.GetOrdinal(this.Rolvalidate);
            if (!dr.IsDBNull(iRolvalidate)) entity.Rolvalidate = Convert.ToInt32(dr.GetValue(iRolvalidate));

            int iRolcheck = dr.GetOrdinal(this.Rolcheck);
            if (!dr.IsDBNull(iRolcheck)) entity.Rolcheck = Convert.ToInt32(dr.GetValue(iRolcheck));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Rolcode = "ROLCODE";
        public string Rolname = "ROLNAME";
        public string Rolvalidate = "ROLVALIDATE";
        public string Rolcheck = "ROLCHECK";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}
