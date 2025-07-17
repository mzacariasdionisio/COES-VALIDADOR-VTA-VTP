using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FW_USERROL
    /// </summary>
    public class FwUserrolHelper : HelperBase
    {
        public FwUserrolHelper(): base(Consultas.FwUserrolSql)
        {
        }

        public FwUserrolDTO Create(IDataReader dr)
        {
            FwUserrolDTO entity = new FwUserrolDTO();

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iRolcode = dr.GetOrdinal(this.Rolcode);
            if (!dr.IsDBNull(iRolcode)) entity.Rolcode = Convert.ToInt32(dr.GetValue(iRolcode));

            int iUserrolvalidate = dr.GetOrdinal(this.Userrolvalidate);
            if (!dr.IsDBNull(iUserrolvalidate)) entity.Userrolvalidate = Convert.ToInt32(dr.GetValue(iUserrolvalidate));

            int iUserrolcheck = dr.GetOrdinal(this.Userrolcheck);
            if (!dr.IsDBNull(iUserrolcheck)) entity.Userrolcheck = Convert.ToInt32(dr.GetValue(iUserrolcheck));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Usercode = "USERCODE";
        public string Rolcode = "ROLCODE";
        public string Userrolvalidate = "USERROLVALIDATE";
        public string Userrolcheck = "USERROLCHECK";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion

        public string SqlGetByRol
        {
            get { return base.GetSqlXml("GetByRol"); }
        }
        
    }
}
