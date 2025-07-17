using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EveInformesScoHelper : HelperBase
    {
        public EveInformesScoHelper()
            : base(Consultas.EveInformesScoSql)
        {
        }
        public EveInformesScoDTO Create(IDataReader dr)
        {
            EveInformesScoDTO entity = new EveInformesScoDTO();

            int iEveinfcodi = dr.GetOrdinal(this.Eveinfcodi);
            if (!dr.IsDBNull(iEveinfcodi)) entity.Eveinfcodi = Convert.ToInt32(dr.GetValue(iEveinfcodi));

            int iEnv_Evencodi = dr.GetOrdinal(this.Env_Evencodi);
            if (!dr.IsDBNull(iEnv_Evencodi)) entity.Env_Evencodi = Convert.ToInt32(dr.GetValue(iEnv_Evencodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iEveinfrutaarchivo = dr.GetOrdinal(this.Eveinfrutaarchivo);
            if (!dr.IsDBNull(iEveinfrutaarchivo)) entity.Eveinfrutaarchivo = dr.GetString(iEveinfrutaarchivo);

            int iEveinfactivo = dr.GetOrdinal(this.Eveinfactivo);
            if (!dr.IsDBNull(iEveinfactivo)) entity.Eveinfactivo = dr.GetString(iEveinfactivo);

            return entity;
        }

        #region Mapeo de Campos
        public string Eveinfcodi = "EVEINFCODI";
        public string Env_Evencodi = "ENV_EVENCODI";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Eveinfrutaarchivo = "EVEINFRUTAARCHIVO";
        public string Eveinfactivo = "EVEINFACTIVO";
        public string Emprnomb = "EMPRNOMB";
        public string Version = "VERSION";
        public string Cumplimiento = "CUMPLIMIENTO";
        public string Portalweb = "PORTALWEB";
        public string Afiversion = "AFIVERSION";
        public string Tipodata = "TIPODATA";
        public string Emprcodi = "EMPRCODI";
        public string Afecodi = "AFECODI";
        public string Anio = "ANIO";
        public string Semestre = "SEMESTRE";
        public string Diames = "DIAMES";
        public string Evencodi = "EVENCODI";
        public string Eveninffalla = "EVENINFFALLA";
        public string Eveninffallan2 = "EVENINFFALLAN2";
        public string Eveinfcodigo = "EVEINFCODIGO";
        #endregion
    }
}
