using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class TrnPeriodoCnaHelper : HelperBase
    {
        public TrnPeriodoCnaHelper() : base(Consultas.TrnPeriodoCnaSql)
        {
        }

        public TrnPeriodoCnaDTO Create(IDataReader dr)
        {
            TrnPeriodoCnaDTO entity = new TrnPeriodoCnaDTO();

            #region Campos Originales
            // PERCNACODI
            int iPercnacodi = dr.GetOrdinal(this.Percnacodi);
            if (!dr.IsDBNull(iPercnacodi)) entity.Percnacodi = dr.GetInt32(iPercnacodi);

            // DD
            int iDd = dr.GetOrdinal(this.Dd);
            if (!dr.IsDBNull(iDd)) entity.Dd = dr.GetString(iDd);

            // DL
            int iDl = dr.GetOrdinal(this.Dl);
            if (!dr.IsDBNull(iDl)) entity.Dl = dr.GetString(iDl);

            // DM
            int iDm = dr.GetOrdinal(this.Dm);
            if (!dr.IsDBNull(iDm)) entity.Dm = dr.GetString(iDm);

            // DMM
            int iDmm = dr.GetOrdinal(this.Dmm);
            if (!dr.IsDBNull(iDmm)) entity.Dmm = dr.GetString(iDmm);

            // DJ
            int iDj = dr.GetOrdinal(this.Dj);
            if (!dr.IsDBNull(iDj)) entity.Dj = dr.GetString(iDj);

            // DVR
            int iDvr = dr.GetOrdinal(this.Dvr);
            if (!dr.IsDBNull(iDvr)) entity.Dvr = dr.GetString(iDvr);

            // DS
            int iDs = dr.GetOrdinal(this.Ds);
            if (!dr.IsDBNull(iDs)) entity.Ds = dr.GetString(iDs);

            // SEMPERIODO
            int iSemperiodo = dr.GetOrdinal(this.Semperiodo);
            if (!dr.IsDBNull(iSemperiodo)) entity.Semperiodo = dr.GetString(iSemperiodo);

            // LASTUSER
            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            // LASTDATE
            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            #endregion

            return entity;
        }

        #region Campos Originales
        public string Percnacodi = "PERCNACODI";
        public string Dd = "DD";
        public string Dl = "DL";
        public string Dm = "DM";
        public string Dmm = "DMM";
        public string Dj = "DJ";
        public string Dvr = "DVR";
        public string Ds = "DS";
        public string Semperiodo = "SEMPERIODO";    
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        #endregion

        public string SqlObtenerSemana
        {
            get { return base.GetSqlXml("GetObtenerSemana"); }
        }
    }
}
