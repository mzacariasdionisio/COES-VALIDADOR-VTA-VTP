using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EveTiposNumeralHelper : HelperBase
    {
        public EveTiposNumeralHelper() : base(Consultas.EveTiposNumeralSql)
        {
        }

        public EveTiposNumeralDTO Create(IDataReader dr)
        {
            EveTiposNumeralDTO entity = new EveTiposNumeralDTO();

            int iEvetipnumcodi = dr.GetOrdinal(this.Evetipnumcodi);
            if (!dr.IsDBNull(iEvetipnumcodi)) entity.EVETIPNUMCODI = dr.GetInt32(iEvetipnumcodi);

            int iEvetipnumdescripcion = dr.GetOrdinal(this.Evetipnumdescripcion);
            if (!dr.IsDBNull(iEvetipnumdescripcion)) entity.EVETIPNUMDESCRIPCION = dr.GetString(iEvetipnumdescripcion);

            int iEvetipnumestado = dr.GetOrdinal(this.Evetipnumestado);
            if (!dr.IsDBNull(iEvetipnumestado)) entity.EVETIPNUMESTADO = dr.GetString(iEvetipnumestado);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            return entity;
        }

        #region Mapeo de Campos

        public string Evetipnumcodi = "EVETIPNUMCODI";
        public string Evetipnumdescripcion = "EVETIPNUMDESCRIPCION";
        public string Evetipnumestado = "EVETIPNUMESTADO";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion

        #region Campos adicionales
        public string Estado = "ESTADO";
        #endregion

    }
}
