using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_HEADCOLUMN
    /// </summary>
    public class MeHeadcolumnHelper : HelperBase
    {
        public MeHeadcolumnHelper(): base(Consultas.MeHeadcolumnSql)
        {
        }

        public MeHeadcolumnDTO Create(IDataReader dr)
        {
            MeHeadcolumnDTO entity = new MeHeadcolumnDTO();

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iHojacodi = dr.GetOrdinal(this.Hojacodi);
            if (!dr.IsDBNull(iHojacodi)) entity.Hojacodi = Convert.ToInt32(dr.GetValue(iHojacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iHeadpos = dr.GetOrdinal(this.Headpos);
            if (!dr.IsDBNull(iHeadpos)) entity.Headpos = Convert.ToInt32(dr.GetValue(iHeadpos));

            int iHeadlen = dr.GetOrdinal(this.Headlen);
            if (!dr.IsDBNull(iHeadlen)) entity.Headlen = Convert.ToInt32(dr.GetValue(iHeadlen));

            int iHeadrow = dr.GetOrdinal(this.Headrow);
            if (!dr.IsDBNull(iHeadrow)) entity.Headrow = Convert.ToInt32(dr.GetValue(iHeadrow));

            int iHeadnombre = dr.GetOrdinal(this.Headnombre);
            if (!dr.IsDBNull(iHeadnombre)) entity.Headnombre = dr.GetString(iHeadnombre);

            return entity;
        }


        #region Mapeo de Campos

        public string Formatcodi = "FORMATCODI";
        public string Hojacodi = "HOJACODI";
        public string Emprcodi = "EMPRCODI";
        public string Headpos = "HEADPOS";
        public string Headlen = "HEADLEN";
        public string Headrow = "HEADROW";
        public string Headnombre = "HEADNOMBRE";

        #endregion
    }
}
