using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_INFORME
    /// </summary>
    public class EveInformeHelper : HelperBase
    {
        public EveInformeHelper(): base(Consultas.EveInformeSql)
        {
        }

        public EveInformeDTO Create(IDataReader dr)
        {
            EveInformeDTO entity = new EveInformeDTO();

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iEveninfcodi = dr.GetOrdinal(this.Eveninfcodi);
            if (!dr.IsDBNull(iEveninfcodi)) entity.Eveninfcodi = Convert.ToInt32(dr.GetValue(iEveninfcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iInfestado = dr.GetOrdinal(this.Infestado);
            if (!dr.IsDBNull(iInfestado)) entity.Infestado = dr.GetString(iInfestado);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iInfversion = dr.GetOrdinal(this.Infversion);
            if (!dr.IsDBNull(iInfversion)) entity.Infversion = dr.GetString(iInfversion);

            int iIndestado = dr.GetOrdinal(this.Indestado);
            if (!dr.IsDBNull(iIndestado)) entity.Indestado = dr.GetString(iIndestado);

            int iIndplazo = dr.GetOrdinal(this.Indplazo);
            if (!dr.IsDBNull(iIndplazo)) entity.Indplazo = dr.GetString(iIndplazo);

            int iLastuserrev = dr.GetOrdinal(this.Lastuserrev);
            if (!dr.IsDBNull(iLastuserrev)) entity.Lastuserrev = dr.GetString(iLastuserrev);

            int iLastdaterev = dr.GetOrdinal(this.Lastdaterev);
            if (!dr.IsDBNull(iLastdaterev)) entity.Lastdaterev = dr.GetDateTime(iLastdaterev);

            return entity;
        }


        #region Mapeo de Campos

        public string Evencodi = "EVENCODI";
        public string Eveninfcodi = "EVENINFCODI";
        public string Emprcodi = "EMPRCODI";
        public string Infestado = "INFESTADO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Infversion = "INFVERSION";
        public string Indestado = "INDESTADO";
        public string Indplazo = "INDPLAZO";
        public string Emprnomb = "EMPRNOMB";
        public string Lastuserrev = "LASTUSERREV";
        public string Lastdaterev = "LASTDATEREV";

        public string SqlObtenerInformeEmpresa
        {
            get { return base.GetSqlXml("ObtenerInformeEmpresa"); }
        }

        public string SqlListarEquiposInforme
        {
            get { return base.GetSqlXml("ListarEquiposInforme"); }
        }

        public string SqlFinalizarInforme
        {
            get { return base.GetSqlXml("FinalizarInforme"); }
        }

        public string SqlObtenerReporteEmpresa
        {
            get { return base.GetSqlXml("ObtenerReporteEmpresa"); }
        }

        public string SqlObtenerEstadoReporte
        {
            get { return base.GetSqlXml("ObtenerEstadoReporte"); }
        }

        public string SqlObtenerEmpresaInforme
        {
            get { return base.GetSqlXml("ObtenerEmpresaInforme"); }
        }

        public string SqlObtenerReporteEmpresaGeneral
        {
            get { return base.GetSqlXml("ObtenerReporteEmpresaGeneral"); }
        }

        public string SqlObtenerInformePorTipo
        {
            get { return base.GetSqlXml("ObtenerInformePorTipo"); }
        }

        public string SqlRevisarInforme
        {
            get { return base.GetSqlXml("RevisarInforme"); }
        }

        #endregion
    }
}
