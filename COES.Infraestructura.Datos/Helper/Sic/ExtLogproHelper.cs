using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EXT_LOGPRO
    /// </summary>
    public class ExtLogproHelper : HelperBase
    {
        public ExtLogproHelper(): base(Consultas.ExtLogproSql)
        {
        }

        public ExtLogproDTO Create(IDataReader dr)
        {
            ExtLogproDTO entity = new ExtLogproDTO();

            int iMencodi = dr.GetOrdinal(this.Mencodi);
            if (!dr.IsDBNull(iMencodi)) entity.Mencodi = Convert.ToInt32(dr.GetValue(iMencodi));

            int iLogpdetmen = dr.GetOrdinal(this.Logpdetmen);
            if (!dr.IsDBNull(iLogpdetmen)) entity.Logpdetmen = dr.GetString(iLogpdetmen);

            int iLogpfechor = dr.GetOrdinal(this.Logpfechor);
            if (!dr.IsDBNull(iLogpfechor)) entity.Logpfechor = dr.GetDateTime(iLogpfechor);

            int iLogpsecuen = dr.GetOrdinal(this.Logpsecuen);
            if (!dr.IsDBNull(iLogpsecuen)) entity.Logpsecuen = Convert.ToInt32(dr.GetValue(iLogpsecuen));

            int iEarcodi = dr.GetOrdinal(this.Earcodi);
            if (!dr.IsDBNull(iEarcodi)) entity.Earcodi = Convert.ToInt32(dr.GetValue(iEarcodi));

             return entity;
        }


        #region Mapeo de Campos

        public string Mencodi = "MENCODI";
        public string Logpdetmen = "LOGPDETMEN";
        public string Logpfechor = "LOGPFECHOR";
        public string Logpsecuen = "LOGPSECUEN";
        public string Earcodi = "EARCODI";


        #endregion
    }
}
