using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_FICTECITEM_NOTA
    /// </summary>
    public class FtFictecItemNotaHelper : HelperBase
    {
        public FtFictecItemNotaHelper()
            : base(Consultas.FtFictecItemNotaSql)
        {
        }

        public FtFictecItemNotaDTO Create(IDataReader dr)
        {
            FtFictecItemNotaDTO entity = new FtFictecItemNotaDTO();

            int iFtitntcodi = dr.GetOrdinal(this.Ftitntcodi);
            if (!dr.IsDBNull(iFtitntcodi)) entity.Ftitntcodi = Convert.ToInt32(dr.GetValue(iFtitntcodi));

            int iFtitcodi = dr.GetOrdinal(this.Ftitcodi);
            if (!dr.IsDBNull(iFtitcodi)) entity.Ftitcodi = Convert.ToInt32(dr.GetValue(iFtitcodi));

            int iFtnotacodi = dr.GetOrdinal(this.Ftnotacodi);
            if (!dr.IsDBNull(iFtnotacodi)) entity.Ftnotacodi = Convert.ToInt32(dr.GetValue(iFtnotacodi));

            int iFtitntfecha = dr.GetOrdinal(this.Ftitntfecha);
            if (!dr.IsDBNull(iFtitntfecha)) entity.Ftitntfecha = dr.GetDateTime(iFtitntfecha);

            return entity;
        }

        #region Mapeo de Campos

        public string Ftitntcodi = "FTITNTCODI";
        public string Ftitcodi = "FTITCODI";
        public string Ftnotacodi = "FTNOTACODI";
        public string Ftitntfecha = "FTITNTFECHA";

        public string Ftnotanum = "FTNOTANUM";

        #endregion

        #region Mapeo de Consultas

        public string SqlListByFteqcodi
        {
            get { return base.GetSqlXml("ListByFteqcodi"); }
        }

        public string SqlDeleteByFtitcodi
        {
            get { return base.GetSqlXml("DeleteByFtitcodi"); }
        }

        #endregion
    }
}
