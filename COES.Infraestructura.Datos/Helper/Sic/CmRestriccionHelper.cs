using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_RESTRICCION
    /// </summary>
    public class CmRestriccionHelper : HelperBase
    {
        public CmRestriccionHelper(): base(Consultas.CmRestriccionSql)
        {
        }

        public CmRestriccionDTO Create(IDataReader dr)
        {
            CmRestriccionDTO entity = new CmRestriccionDTO();

            int iCmrestcodi = dr.GetOrdinal(this.Cmrestcodi);
            if (!dr.IsDBNull(iCmrestcodi)) entity.Cmrestcodi = Convert.ToInt32(dr.GetValue(iCmrestcodi));

            int iCmgncorrelativo = dr.GetOrdinal(this.Cmgncorrelativo);
            if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Cmrestcodi = "CMRESTCODI";
        public string Cmgncorrelativo = "CMGNCORRELATIVO";
        public string Equicodi = "EQUICODI";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Subcausaabrev = "SUBCAUSAABREV";
        public string Equiabrev = "EQUIABREV";

        public string SqlObtenerRestriccionPorCorrida
        {
            get { return base.GetSqlXml("ObtenerRestriccionPorCorrida"); }
        }

        #endregion
    }
}
