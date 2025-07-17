using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_REPORTE_DETALLE
    /// </summary>
    public class CbReporteDetalleHelper : HelperBase
    {
        public CbReporteDetalleHelper(): base(Consultas.CbReporteDetalleSql)
        {
        }

        public CbReporteDetalleDTO Create(IDataReader dr)
        {
            CbReporteDetalleDTO entity = new CbReporteDetalleDTO();

            int iCbrepdcodi = dr.GetOrdinal(this.Cbrepdcodi);
            if (!dr.IsDBNull(iCbrepdcodi)) entity.Cbrepdcodi = Convert.ToInt32(dr.GetValue(iCbrepdcodi));

            int iCcombcodi = dr.GetOrdinal(this.Ccombcodi);
            if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

            int iCbrcencodi = dr.GetOrdinal(this.Cbrcencodi);
            if (!dr.IsDBNull(iCbrcencodi)) entity.Cbrcencodi = Convert.ToInt32(dr.GetValue(iCbrcencodi));

            int iCbrepdvalor = dr.GetOrdinal(this.Cbrepdvalor);
            if (!dr.IsDBNull(iCbrepdvalor)) entity.Cbrepdvalor = dr.GetString(iCbrepdvalor);

            int iCbrepvalordecimal = dr.GetOrdinal(this.Cbrepvalordecimal);
            if (!dr.IsDBNull(iCbrepvalordecimal)) entity.Cbrepvalordecimal = dr.GetDecimal(iCbrepvalordecimal);

            return entity;
        }


        #region Mapeo de Campos

        public string Cbrepdcodi = "CBREPDCODI";
        public string Ccombcodi = "CCOMBCODI";
        public string Cbrcencodi = "CBRCENCODI";
        public string Cbrepdvalor = "CBREPDVALOR";
        public string Cbrepvalordecimal = "CBREPVALORDECIMAL";

        #endregion

        public string SqlGetByIdCentral
        {
            get { return base.GetSqlXml("GetByIdCentral"); }
        }
    }
}
