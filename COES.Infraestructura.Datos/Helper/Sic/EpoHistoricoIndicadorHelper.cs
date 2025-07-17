using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_HISTORICO_INDICADOR
    /// </summary>
    public class EpoHistoricoIndicadorHelper : HelperBase
    {
        public EpoHistoricoIndicadorHelper(): base(Consultas.EpoHistoricoIndicadorSql)
        {
        }

        public EpoHistoricoIndicadorDTO Create(IDataReader dr)
        {
            EpoHistoricoIndicadorDTO entity = new EpoHistoricoIndicadorDTO();

            int iHincodi = dr.GetOrdinal(this.Hincodi);
            if (!dr.IsDBNull(iHincodi)) entity.Hincodi = Convert.ToInt32(dr.GetValue(iHincodi));

            int iIndcodi = dr.GetOrdinal(this.Indcodi);
            if (!dr.IsDBNull(iIndcodi)) entity.Indcodi = Convert.ToInt32(dr.GetValue(iIndcodi));

            int iHinanio = dr.GetOrdinal(this.Hinanio);
            if (!dr.IsDBNull(iHinanio)) entity.Hinanio = Convert.ToInt32(dr.GetValue(iHinanio));

            int iHinmeta = dr.GetOrdinal(this.Hinmeta);
            if (!dr.IsDBNull(iHinmeta)) entity.Hinmeta = dr.GetDecimal(iHinmeta);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            return entity;
        }


        #region Mapeo de Campos

        public string Hincodi = "HINCODI";
        public string Indcodi = "INDCODI";
        public string Hinanio = "HINANIO";
        public string Hinmeta = "HINMETA";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion
    }
}
