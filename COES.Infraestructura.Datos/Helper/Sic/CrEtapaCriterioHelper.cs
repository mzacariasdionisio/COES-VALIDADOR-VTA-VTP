using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class CrEtapaCriterioHelper : HelperBase
    {
        public CrEtapaCriterioHelper() : base(Consultas.CrEtapaCriterioSql)
        {
        }
        public CrEtapaCriterioDTO Create(IDataReader dr)
        {
            CrEtapaCriterioDTO entity = new CrEtapaCriterioDTO();

            int iCretapacricodi = dr.GetOrdinal(this.Cretapacricodi);
            if (!dr.IsDBNull(iCretapacricodi)) entity.CRETAPACRICODI = dr.GetInt32(iCretapacricodi);

            int iCCretapacodi = dr.GetOrdinal(this.Cretapacodi);
            if (!dr.IsDBNull(iCCretapacodi)) entity.CRETAPACODI = dr.GetInt32(iCCretapacodi);

            int iCrcriteriocodi = dr.GetOrdinal(this.Crcriteriocodi);
            if (!dr.IsDBNull(iCrcriteriocodi)) entity.CRCRITERIOCODI = dr.GetInt32(iCrcriteriocodi);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            return entity;
        }

        #region Mapeo de Campos

        public string Cretapacricodi = "CRETAPACRICODI";
        public string Cretapacodi = "CRETAPACODI";
        public string Crcriteriocodi = "CRCRITERIOCODI";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Credescripcion = "CREDESCRIPCION";

        #endregion

        public string ListaCriteriosEtapa
        {
            get { return base.GetSqlXml("ListaCriteriosEtapa"); }
        }
        public string SqlListaCriteriosEvento
        {
            get { return base.GetSqlXml("ListaCriteriosEvento"); }
        }  
    }
}
