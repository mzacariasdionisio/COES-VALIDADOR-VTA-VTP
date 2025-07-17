using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PSU_DESVCMGSNC
    /// </summary>
    public class PsuDesvcmgsncHelper : HelperBase
    {
        public PsuDesvcmgsncHelper(): base(Consultas.PsuDesvcmgsncSql)
        {
        }

        public PsuDesvcmgsncDTO Create(IDataReader dr)
        {
            PsuDesvcmgsncDTO entity = new PsuDesvcmgsncDTO();

            int iDesvfecha = dr.GetOrdinal(this.Desvfecha);
            if (!dr.IsDBNull(iDesvfecha)) entity.Desvfecha = dr.GetDateTime(iDesvfecha);

            int iCmgsnc = dr.GetOrdinal(this.Cmgsnc);
            if (!dr.IsDBNull(iCmgsnc)) entity.Cmgsnc = dr.GetDecimal(iCmgsnc);

            int iLastUser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastUser)) entity.Lastuser = dr.GetString(iLastUser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Desvfecha = "DESVFECHA";
        public string Cmgsnc = "CMGSNC";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}
