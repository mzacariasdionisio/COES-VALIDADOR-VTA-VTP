using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_PARAMETRO
    /// </summary>
    public class CmParametroHelper : HelperBase
    {
        public CmParametroHelper(): base(Consultas.CmParametroSql)
        {
        }

        public CmParametroDTO Create(IDataReader dr)
        {
            CmParametroDTO entity = new CmParametroDTO();

            int iCmparcodi = dr.GetOrdinal(this.Cmparcodi);
            if (!dr.IsDBNull(iCmparcodi)) entity.Cmparcodi = Convert.ToInt32(dr.GetValue(iCmparcodi));

            int iCmparnombre = dr.GetOrdinal(this.Cmparnombre);
            if (!dr.IsDBNull(iCmparnombre)) entity.Cmparnombre = dr.GetString(iCmparnombre);

            int iCmparvalor = dr.GetOrdinal(this.Cmparvalor);
            if (!dr.IsDBNull(iCmparvalor)) entity.Cmparvalor = dr.GetString(iCmparvalor);

            int iCmparlastuser = dr.GetOrdinal(this.Cmparlastuser);
            if (!dr.IsDBNull(iCmparlastuser)) entity.Cmparlastuser = dr.GetString(iCmparlastuser);

            int iCmparlastdate = dr.GetOrdinal(this.Cmparlastdate);
            if (!dr.IsDBNull(iCmparlastdate)) entity.Cmparlastdate = dr.GetDateTime(iCmparlastdate);

            int iCmparinferior = dr.GetOrdinal(this.Cmparinferior);
            if (!dr.IsDBNull(iCmparinferior)) entity.Cmparinferior = dr.GetDecimal(iCmparinferior);

            int iCmparsuperior = dr.GetOrdinal(this.Cmparsuperior);
            if (!dr.IsDBNull(iCmparsuperior)) entity.Cmparsuperior = dr.GetDecimal(iCmparsuperior);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmparcodi = "CMPARCODI";
        public string Cmparnombre = "CMPARNOMBRE";
        public string Cmparvalor = "CMPARVALOR";
        public string Cmparlastuser = "CMPARLASTUSER";
        public string Cmparlastdate = "CMPARLASTDATE";
        public string Cmparinferior = "CMPARINFERIOR";
        public string Cmparsuperior = "CMPARSUPERIOR";

        #endregion
    }
}
