using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_HOJA
    /// </summary>
    public class MeHojaHelper : HelperBase
    {
        public MeHojaHelper()
            : base(Consultas.MeHojaSql)
        {
        }

        public MeHojaDTO Create(IDataReader dr)
        {
            MeHojaDTO entity = new MeHojaDTO();

            int iHojacodi = dr.GetOrdinal(this.Hojacodi);
            if (!dr.IsDBNull(iHojacodi)) entity.Hojacodi = Convert.ToInt32(dr.GetValue(iHojacodi));

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iCabcodi = dr.GetOrdinal(this.Cabcodi);
            if (!dr.IsDBNull(iCabcodi)) entity.Cabcodi = Convert.ToInt32(dr.GetValue(iCabcodi));

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iHojanombre = dr.GetOrdinal(this.Hojanombre);
            if (!dr.IsDBNull(iHojanombre)) entity.Hojanombre = dr.GetString(iHojanombre);

            int iHojaorden = dr.GetOrdinal(this.Hojaorden);
            if (!dr.IsDBNull(iHojaorden)) entity.Hojaorden = Convert.ToInt32(dr.GetValue(iHojaorden));

            int iHojapadre = dr.GetOrdinal(this.Hojapadre);
            if (!dr.IsDBNull(iHojapadre)) entity.Hojapadre = Convert.ToInt32(dr.GetValue(iHojapadre));

            return entity;
        }


        #region Mapeo de Campos

        public string Hojacodi = "HOJACODI";
        public string Formatcodi = "FORMATCODI";
        public string Cabcodi = "CABCODI";
        public string Lectcodi = "LECTCODI";
        public string Hojanombre = "HOJANOMBRE";
        public string Hojaorden = "HOJAORDEN";
        public string Hojapadre = "HOJAPADRE";

        #endregion

        #region Mapeo de Querys

        public string SqlListPadre
        {
            get { return base.GetSqlXml("ListPadre"); }
        }

        #endregion
    }
}
