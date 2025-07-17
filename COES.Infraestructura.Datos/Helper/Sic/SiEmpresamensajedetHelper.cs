using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_EMPRESAMENSAJEDET
    /// </summary>
    public class SiEmpresamensajedetHelper : HelperBase
    {
        public SiEmpresamensajedetHelper() : base(Consultas.SiEmpresamensajedetSql)
        {
        }

        public SiEmpresamensajedetDTO Create(IDataReader dr)
        {
            SiEmpresamensajedetDTO entity = new SiEmpresamensajedetDTO();

            int iEmsjdtcodi = dr.GetOrdinal(this.Emsjdtcodi);
            if (!dr.IsDBNull(iEmsjdtcodi)) entity.Emsjdtcodi = Convert.ToInt32(dr.GetValue(iEmsjdtcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmsjdtcorreo = dr.GetOrdinal(this.Emsjdtcorreo);
            if (!dr.IsDBNull(iEmsjdtcorreo)) entity.Emsjdtcorreo = dr.GetString(iEmsjdtcorreo);

            int iEmsjdttipo = dr.GetOrdinal(this.Emsjdttipo);
            if (!dr.IsDBNull(iEmsjdttipo)) entity.Emsjdttipo = dr.GetString(iEmsjdttipo);

            int iEmsjdtfeclectura = dr.GetOrdinal(this.Emsjdtfeclectura);
            if (!dr.IsDBNull(iEmsjdtfeclectura)) entity.Emsjdtfeclectura = dr.GetDateTime(iEmsjdtfeclectura);

            int iEmsjdtusulectura = dr.GetOrdinal(this.Emsjdtusulectura);
            if (!dr.IsDBNull(iEmsjdtusulectura)) entity.Emsjdtusulectura = dr.GetString(iEmsjdtusulectura);

            int iEmpmsjcodi = dr.GetOrdinal(this.Empmsjcodi);
            if (!dr.IsDBNull(iEmpmsjcodi)) entity.Empmsjcodi = Convert.ToInt32(dr.GetValue(iEmpmsjcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Emsjdtcodi = "EMSJDTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Emsjdtcorreo = "EMSJDTCORREO";
        public string Emsjdttipo = "EMSJDTTIPO";
        public string Emsjdtfeclectura = "EMSJDTFECLECTURA";
        public string Emsjdtusulectura = "EMSJDTUSULECTURA";
        public string Empmsjcodi = "EMPMSJCODI";

        public string Msgcodi = "MSGCODI";

        #endregion
    }
}
