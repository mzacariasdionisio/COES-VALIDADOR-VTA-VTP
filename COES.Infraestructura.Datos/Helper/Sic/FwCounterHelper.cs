using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FW_COUNTER
    /// </summary>
    public class FwCounterHelper : HelperBase
    {
        public FwCounterHelper(): base(Consultas.FwCounterSql)
        {
        }

        public FwCounterDTO Create(IDataReader dr)
        {
            FwCounterDTO entity = new FwCounterDTO();

            int iTablename = dr.GetOrdinal(this.Tablename);
            if (!dr.IsDBNull(iTablename)) entity.Tablename = dr.GetString(iTablename);

            int iMaxcount = dr.GetOrdinal(this.Maxcount);
            if (!dr.IsDBNull(iMaxcount)) entity.Maxcount = Convert.ToInt32(dr.GetValue(iMaxcount));

            return entity;
        }


        #region Mapeo de Campos

        public string Tablename = "TABLENAME";
        public string Maxcount = "MAXCOUNT";

        #endregion
        public string SqlUpdateMaxCount
        {
            get { return base.GetSqlXml("UpdateMaxCount"); }
        }
    }
}
