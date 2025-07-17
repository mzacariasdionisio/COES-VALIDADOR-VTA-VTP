using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_REP_CABECERA
    /// </summary>
    public class CbRepCabeceraHelper : HelperBase
    {
        public CbRepCabeceraHelper(): base(Consultas.CbRepCabeceraSql)
        {
        }

        public CbRepCabeceraDTO Create(IDataReader dr)
        {
            CbRepCabeceraDTO entity = new CbRepCabeceraDTO();

            int iCbrcabcodi = dr.GetOrdinal(this.Cbrcabcodi);
            if (!dr.IsDBNull(iCbrcabcodi)) entity.Cbrcabcodi = Convert.ToInt32(dr.GetValue(iCbrcabcodi));

            int iCbrprocodi = dr.GetOrdinal(this.Cbrprocodi);
            if (!dr.IsDBNull(iCbrprocodi)) entity.Cbrprocodi = Convert.ToInt32(dr.GetValue(iCbrprocodi));

            int iCbrepcodi = dr.GetOrdinal(this.Cbrepcodi);
            if (!dr.IsDBNull(iCbrepcodi)) entity.Cbrepcodi = Convert.ToInt32(dr.GetValue(iCbrepcodi));

            int iCbrcabdescripcion = dr.GetOrdinal(this.Cbrcabdescripcion);
            if (!dr.IsDBNull(iCbrcabdescripcion)) entity.Cbrcabdescripcion = dr.GetString(iCbrcabdescripcion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cbrcabcodi = "CBRCABCODI";
        public string Cbrprocodi = "CBRPROCODI";
        public string Cbrepcodi = "CBREPCODI";
        public string Cbrcabdescripcion = "CBRCABDESCRIPCION";

        #endregion

        public string SqlGetByTipoReporte
        {
            get { return base.GetSqlXml("GetByTipoReporte"); }
        }

        public string SqlGetByIdReporte
        {
            get { return base.GetSqlXml("GetByIdReporte"); }
        }

        public string SqlGetByTipoReporteYMesVigencia
        {
            get { return base.GetSqlXml("GetByTipoReporteYMesVigencia"); }
        }
    }
}
