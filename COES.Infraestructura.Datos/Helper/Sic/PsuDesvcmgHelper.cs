using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PSU_DESVCMG
    /// </summary>
    public class PsuDesvcmgHelper : HelperBase
    {
        public PsuDesvcmgHelper(): base(Consultas.PsuDesvcmgSql)
        {
        }

        public PsuDesvcmgDTO Create(IDataReader dr)
        {
            PsuDesvcmgDTO entity = new PsuDesvcmgDTO();

            int iDesvfecha = dr.GetOrdinal(this.Desvfecha);
            if (!dr.IsDBNull(iDesvfecha)) entity.Desvfecha = dr.GetDateTime(iDesvfecha);

            int iCmgrpunta = dr.GetOrdinal(this.Cmgrpunta);
            if (!dr.IsDBNull(iCmgrpunta)) entity.Cmgrpunta = dr.GetDecimal(iCmgrpunta);

            int iCmgrmedia = dr.GetOrdinal(this.Cmgrmedia);
            if (!dr.IsDBNull(iCmgrmedia)) entity.Cmgrmedia = dr.GetDecimal(iCmgrmedia);

            int iCmgrbase = dr.GetOrdinal(this.Cmgrbase);
            if (!dr.IsDBNull(iCmgrbase)) entity.Cmgrbase = dr.GetDecimal(iCmgrbase);

            int iLastUser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastUser)) entity.Lastuser = dr.GetString(iLastUser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Desvfecha = "DESVFECHA";
        public string Cmgrpunta = "CMGRPUNTA";
        public string Cmgrmedia = "CMGRMEDIA";
        public string Cmgrbase = "CMGRBASE";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}
