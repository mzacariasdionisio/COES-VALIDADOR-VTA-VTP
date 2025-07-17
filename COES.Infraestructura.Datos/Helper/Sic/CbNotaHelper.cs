using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_NOTA
    /// </summary>
    public class CbNotaHelper : HelperBase
    {
        public CbNotaHelper(): base(Consultas.CbNotaSql)
        {
        }

        public CbNotaDTO Create(IDataReader dr)
        {
            CbNotaDTO entity = new CbNotaDTO();

            int iCbnotacodi = dr.GetOrdinal(this.Cbnotacodi);
            if (!dr.IsDBNull(iCbnotacodi)) entity.Cbnotacodi = Convert.ToInt32(dr.GetValue(iCbnotacodi));

            int iCbrepcodi = dr.GetOrdinal(this.Cbrepcodi);
            if (!dr.IsDBNull(iCbrepcodi)) entity.Cbrepcodi = Convert.ToInt32(dr.GetValue(iCbrepcodi));

            int iCbnotaitem = dr.GetOrdinal(this.Cbnotaitem);
            if (!dr.IsDBNull(iCbnotaitem)) entity.Cbnotaitem = dr.GetString(iCbnotaitem);

            int iCbnotadescripcion = dr.GetOrdinal(this.Cbnotadescripcion);
            if (!dr.IsDBNull(iCbnotadescripcion)) entity.Cbnotadescripcion = dr.GetString(iCbnotadescripcion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cbnotacodi = "CBNOTACODI";
        public string Cbrepcodi = "CBREPCODI";
        public string Cbnotaitem = "CBNOTAITEM";
        public string Cbnotadescripcion = "CBNOTADESCRIPCION";

        #endregion

        public string SqlGetByReporte
        {
            get { return base.GetSqlXml("GetByReporte"); }
        }

        public string SqlGetByTipoReporte
        {
            get { return base.GetSqlXml("GetByTipoReporte"); }
        }
        
    }
}
