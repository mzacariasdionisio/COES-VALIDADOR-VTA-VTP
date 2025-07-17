using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_SOLICITUDAMPLIACION
    /// </summary>
    public class SiSolicitudAmpliacionHelper : HelperBase
    {
        public SiSolicitudAmpliacionHelper(): base(Consultas.SiSolicitudAmpliacionSql)
        {
        }

        public SiSolicitudAmpliacionDTO Create(IDataReader dr)
        {
            SiSolicitudAmpliacionDTO entity = new SiSolicitudAmpliacionDTO();

            int iSoliCodi = dr.GetOrdinal(this.SoliCodi);
            if (!dr.IsDBNull(iSoliCodi)) entity.SoliCodi = Convert.ToInt32(dr.GetValue(iSoliCodi));

            int iMsgCodi = dr.GetOrdinal(this.MsgCodi);
            if (!dr.IsDBNull(iMsgCodi)) entity.MsgCodi = Convert.ToInt32(dr.GetValue(iMsgCodi));

            int iAmpliFecha = dr.GetOrdinal(this.AmpliFecha);
            if (!dr.IsDBNull(iAmpliFecha)) entity.AmpliFecha = dr.GetDateTime(iAmpliFecha);

            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

            int iAmpliFechaPlazo = dr.GetOrdinal(this.AmpliFechaPlazo);
            if (!dr.IsDBNull(iAmpliFechaPlazo)) entity.AmpliFechaPlazo = dr.GetDateTime(iAmpliFechaPlazo);

            int iLastUser = dr.GetOrdinal(this.LastUser);
            if (!dr.IsDBNull(iLastUser)) entity.LastUser = dr.GetString(iLastUser);

            int iLastDate = dr.GetOrdinal(this.LastDate);
            if (!dr.IsDBNull(iLastDate)) entity.LastDate = dr.GetDateTime(iLastDate);

            int iFormatCodi = dr.GetOrdinal(this.FormatCodi);
            if (!dr.IsDBNull(iFormatCodi)) entity.FormatCodi = Convert.ToInt32(dr.GetValue(iFormatCodi));

            int iFDatCodi = dr.GetOrdinal(this.FDatCodi);
            if (!dr.IsDBNull(iFDatCodi)) entity.FDatCodi = Convert.ToInt32(dr.GetValue(iFDatCodi));

            int iFlagTipo = dr.GetOrdinal(this.FlagTipo);
            if (!dr.IsDBNull(iFlagTipo)) entity.FlagTipo = Convert.ToInt32(dr.GetValue(iFlagTipo));
            
            return entity;
        }

        #region
        public string SoliCodi = "SOLICODI";
        public string MsgCodi = "MSGCODI";
        public string AmpliFecha = "AMPLIFECHA";
        public string EmprCodi = "EMPRCODI";
        public string AmpliFechaPlazo = "AMPLIFECHAPLAZO";
        public string LastUser = "LASTUSER";
        public string LastDate = "LASTDATE";
        public string FormatCodi = "FORMATCODI";
        public string FDatCodi = "FDATCODI";
        public string FlagTipo = "FLAGTIPO";        
        #endregion


        public string SqlSave
        {
            get { return base.GetSqlXml("Save"); }
        }
        public string SqlGetByMsgCodi
        {
            get { return base.GetSqlXml("GetByMsgCodi"); }
        }
    }
}
