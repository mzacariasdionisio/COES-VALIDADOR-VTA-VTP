using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_OBS
    /// </summary>
    public class CbObsHelper : HelperBase
    {
        public CbObsHelper() : base(Consultas.CbObsSql)
        {
        }

        public CbObsDTO Create(IDataReader dr)
        {
            CbObsDTO entity = new CbObsDTO();

            int iCbevdacodi = dr.GetOrdinal(this.Cbevdacodi);
            if (!dr.IsDBNull(iCbevdacodi)) entity.Cbevdacodi = Convert.ToInt32(dr.GetValue(iCbevdacodi));

            int iCbobscodi = dr.GetOrdinal(this.Cbobscodi);
            if (!dr.IsDBNull(iCbobscodi)) entity.Cbobscodi = Convert.ToInt32(dr.GetValue(iCbobscodi));

            int iCbobshtml = dr.GetOrdinal(this.Cbobshtml);
            if (!dr.IsDBNull(iCbobshtml)) entity.Cbobshtml = dr.GetString(iCbobshtml);

            return entity;
        }

        #region Mapeo de Campos

        public string Cbevdacodi = "CBEVDACODI";
        public string Cbobscodi = "CBOBSCODI";
        public string Cbobshtml = "CBOBSHTML";

        public string Equicodi = "EQUICODI";
        public string Ccombcodi = "CCOMBCODI";
        public string Ccombnombre = "CCOMBNOMBRE";

        #endregion

        public string SqlListByCbvercodi
        {
            get { return base.GetSqlXml("ListByCbvercodi"); }
        }
    }
}
