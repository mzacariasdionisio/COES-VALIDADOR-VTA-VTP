using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_REPORTE_CENTRAL
    /// </summary>
    public class CbReporteCentralHelper : HelperBase
    {
        public CbReporteCentralHelper(): base(Consultas.CbReporteCentralSql)
        {
        }

        public CbReporteCentralDTO Create(IDataReader dr)
        {
            CbReporteCentralDTO entity = new CbReporteCentralDTO();

            int iCbrepcodi = dr.GetOrdinal(this.Cbrepcodi);
            if (!dr.IsDBNull(iCbrepcodi)) entity.Cbrepcodi = Convert.ToInt32(dr.GetValue(iCbrepcodi));

            int iCbrcencodi = dr.GetOrdinal(this.Cbrcencodi);
            if (!dr.IsDBNull(iCbrcencodi)) entity.Cbrcencodi = Convert.ToInt32(dr.GetValue(iCbrcencodi));

            int iCbcentcodi = dr.GetOrdinal(this.Cbcentcodi);
            if (!dr.IsDBNull(iCbcentcodi)) entity.Cbcentcodi = Convert.ToInt32(dr.GetValue(iCbcentcodi));

            int iCbrcennombre = dr.GetOrdinal(this.Cbrcennombre);
            if (!dr.IsDBNull(iCbrcennombre)) entity.Cbrcennombre = dr.GetString(iCbrcennombre);

            int iCbrcencoloreado = dr.GetOrdinal(this.Cbrcencoloreado);
            if (!dr.IsDBNull(iCbrcencoloreado)) entity.Cbrcencoloreado = Convert.ToInt32(dr.GetValue(iCbrcencoloreado));

            int iCbrcenorigen = dr.GetOrdinal(this.Cbrcenorigen);
            if (!dr.IsDBNull(iCbrcenorigen)) entity.Cbrcenorigen = Convert.ToInt32(dr.GetValue(iCbrcenorigen));

            int iCbrcenorden = dr.GetOrdinal(this.Cbrcenorden);
            if (!dr.IsDBNull(iCbrcenorden)) entity.Cbrcenorden = Convert.ToInt32(dr.GetValue(iCbrcenorden));

            return entity;
        }


        #region Mapeo de Campos

        public string Cbrepcodi = "CBREPCODI";
        public string Cbrcencodi = "CBRCENCODI";
        public string Cbcentcodi = "CBCENTCODI";
        public string Cbrcennombre = "CBRCENNOMBRE";
        public string Equicodi = "EQUICODI";
        public string Cbrcencoloreado = "CBRCENCOLOREADO";
        public string Cbrcenorigen = "CBRCENORIGEN";
        public string Cbrcenorden= "CBRCENORDEN";

        #endregion

        public string SqlGetByIdReporte
        {
            get { return base.GetSqlXml("GetByIdReporte"); }
        }
    }
}
