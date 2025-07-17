using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_HTRABAJO_ESTADO
    /// </summary>
    public class PrHtrabajoEstadoHelper : HelperBase
    {
        public PrHtrabajoEstadoHelper(): base(Consultas.PrHtrabajoEstadoSql)
        {
        }

        public PrHtrabajoEstadoDTO Create(IDataReader dr)
        {
            PrHtrabajoEstadoDTO entity = new PrHtrabajoEstadoDTO();

            int iHtestcodi = dr.GetOrdinal(this.Htestcodi);
            if (!dr.IsDBNull(iHtestcodi)) entity.Htestcodi = Convert.ToInt32(dr.GetValue(iHtestcodi));

            int iHtestcolor = dr.GetOrdinal(this.Htestcolor);
            if (!dr.IsDBNull(iHtestcolor)) entity.Htestcolor = dr.GetString(iHtestcolor);

            int iHtestdesc = dr.GetOrdinal(this.Htestdesc);
            if (!dr.IsDBNull(iHtestdesc)) entity.Htestdesc = dr.GetString(iHtestdesc);

            return entity;
        }


        #region Mapeo de Campos

        public string Htestcodi = "HTESTCODI";
        public string Htestcolor = "HTESTCOLOR";
        public string Htestdesc = "HTESTDESC";

        #endregion
    }
}
