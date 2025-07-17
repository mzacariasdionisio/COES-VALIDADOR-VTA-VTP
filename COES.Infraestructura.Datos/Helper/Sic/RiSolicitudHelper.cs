using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RI_SOLICITUD
    /// </summary>
    public class RiSolicitudHelper : HelperBase
    {
        public RiSolicitudHelper()
            : base(Consultas.RiSolicitudSql)
        {
        }

        public RiSolicitudDTO Create(IDataReader dr)
        {
            RiSolicitudDTO entity = new RiSolicitudDTO();

            int iSolicodi = dr.GetOrdinal(this.Solicodi);
            if (!dr.IsDBNull(iSolicodi)) entity.Solicodi = Convert.ToInt32(dr.GetValue(iSolicodi));

            int iSoliestado = dr.GetOrdinal(this.Soliestado);
            if (!dr.IsDBNull(iSoliestado)) entity.Soliestado = dr.GetString(iSoliestado);

            int iSoliestadoInterno = dr.GetOrdinal(this.SoliestadoInterno);
            if (!dr.IsDBNull(iSoliestadoInterno)) entity.SoliestadoInterno = dr.GetString(iSoliestadoInterno);

            int iSolienviado = dr.GetOrdinal(this.Solienviado);
            if (!dr.IsDBNull(iSolienviado)) entity.Solienviado = dr.GetString(iSolienviado);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iSolifecsolicitud = dr.GetOrdinal(this.Solifecsolicitud);
            if (!dr.IsDBNull(iSolifecsolicitud)) entity.Solifecsolicitud = dr.GetDateTime(iSolifecsolicitud);

            int iSoliusucreacion = dr.GetOrdinal(this.Soliusucreacion);
            if (!dr.IsDBNull(iSoliusucreacion)) entity.Soliusucreacion = dr.GetString(iSoliusucreacion);

            int iSolifeccreacion = dr.GetOrdinal(this.Solifeccreacion);
            if (!dr.IsDBNull(iSolifeccreacion)) entity.Solifeccreacion = dr.GetDateTime(iSolifeccreacion);

            int iSoliusumodificacion = dr.GetOrdinal(this.Soliusumodificacion);
            if (!dr.IsDBNull(iSoliusumodificacion)) entity.Soliusumodificacion = dr.GetString(iSoliusumodificacion);

            int iSolifecmodificacion = dr.GetOrdinal(this.Solifecmodificacion);
            if (!dr.IsDBNull(iSolifecmodificacion)) entity.Solifecmodificacion = dr.GetDateTime(iSolifecmodificacion);

            int iTisocodi = dr.GetOrdinal(this.Tisocodi);
            if (!dr.IsDBNull(iTisocodi)) entity.Tisocodi = Convert.ToInt32(dr.GetValue(iTisocodi));

            int iSolifecproceso = dr.GetOrdinal(this.Solifecproceso);
            if (!dr.IsDBNull(iSolifecproceso)) entity.Solifecproceso = dr.GetDateTime(iSolifecproceso);

            int iSoliususolicitud = dr.GetOrdinal(this.Soliususolicitud);
            if (!dr.IsDBNull(iSoliususolicitud)) entity.Soliususolicitud = Convert.ToInt32(dr.GetValue(iSoliususolicitud));

            int iSoliusuproceso = dr.GetOrdinal(this.Soliusuproceso);
            if (!dr.IsDBNull(iSoliusuproceso)) entity.Soliusuproceso = Convert.ToInt32(dr.GetValue(iSoliusuproceso));

            int iSoliobservacion = dr.GetOrdinal(this.Soliobservacion);
            if (!dr.IsDBNull(iSoliobservacion)) entity.Soliobservacion = dr.GetString(iSoliobservacion);

            return entity;
        }

        public RiSolicitudDTO CreatePend(IDataReader dr)
        {
            RiSolicitudDTO entity = new RiSolicitudDTO();

            int iEmprCodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

            int iSolicodi = dr.GetOrdinal(this.Solicodi);
            if (!dr.IsDBNull(iSolicodi)) entity.Solicodi = Convert.ToInt32(dr.GetValue(iSolicodi));

            int iTisocodi = dr.GetOrdinal(this.Tisocodi);
            if (!dr.IsDBNull(iTisocodi)) entity.Tisocodi = Convert.ToInt32(dr.GetValue(iTisocodi));

            int iTisonombre = dr.GetOrdinal(this.Tisonombre);
            if (!dr.IsDBNull(iTisonombre)) entity.Tisonombre = dr.GetString(iTisonombre);

            int iEmprrazsocial = dr.GetOrdinal(this.Emprrazsocial);
            if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.EmprnombComercial = dr.GetString(iEmprnomb);

            int iEmprabrev = dr.GetOrdinal(this.Emprabrev);
            if (!dr.IsDBNull(iEmprabrev)) entity.Emprsigla = dr.GetString(iEmprabrev);

            int iSolifecSolicitud = dr.GetOrdinal(this.Solifecsolicitud);
            if (!dr.IsDBNull(iSolifecSolicitud)) entity.Solifecsolicitud = dr.GetDateTime(iSolifecSolicitud);

            int iSoliestado = dr.GetOrdinal(this.Soliestado);
            if (!dr.IsDBNull(iSoliestado)) entity.Soliestado = dr.GetString(iSoliestado);

            int iSoliestadoInterno = dr.GetOrdinal(this.SoliestadoInterno);
            if (!dr.IsDBNull(iSoliestadoInterno)) entity.SoliestadoInterno = dr.GetString(iSoliestadoInterno);

            int iHorasTrans = dr.GetOrdinal(this.Horas);
            if (!dr.IsDBNull(iHorasTrans)) entity.HorasTranscurridas = Convert.ToInt32(dr.GetValue(iHorasTrans));

            int iSolienviado = dr.GetOrdinal(this.Solienviado);
            if (!dr.IsDBNull(iSolienviado)) entity.Solienviado = dr.GetString(iSolienviado);

            int iSolifecEnviado = dr.GetOrdinal(this.Solifecenviado);
            if (!dr.IsDBNull(iSolifecEnviado)) entity.Solifecenviado = dr.GetDateTime(iSolifecEnviado);

            int iSoliNotificado = dr.GetOrdinal(this.Solinotificado);
            if (!dr.IsDBNull(iSoliNotificado)) entity.Solinotificado = dr.GetString(iSoliNotificado);

            int iSolifecNotificado = dr.GetOrdinal(this.Solifecnotificado);
            if (!dr.IsDBNull(iSolifecNotificado)) entity.Solifecnotificado = dr.GetDateTime(iSolifecNotificado);


            return entity;
        }

        #region ConsultasSql

        public string SqlListPend
        {
            get { return base.GetSqlXml("ListPend"); }
        }

        public string SqlNroRegListPend
        {
            get { return base.GetSqlXml("NroRegListPend"); }
        }
        public string SqlListPendporEmpresa
        {
            get { return base.GetSqlXml("ListPendporEmpresa"); }
        }

        public string SqlNroRegListPendporEmpresa
        {
            get { return base.GetSqlXml("NroRegListPendporEmpresa"); }
        }

        public string SqlDarConformidad
        {
            get { return base.GetSqlXml("DarConformidad"); }
        }

        public string SqlDarNotificar
        {
            get { return base.GetSqlXml("DarNotificar"); }
        }

        public string SqlFinalizarSolicitud
        {
            get { return base.GetSqlXml("FinalizarSolicitud"); }
        }

        public string SqlActualizarFechaProcesoSolicitud
        {
            get { return base.GetSqlXml("ActualizarFechaProcesoSolicitud"); }
        }

        public string SqlSolicitudEnCurso
        {
            get { return base.GetSqlXml("SolicitudEnCurso"); }
        }


        #endregion

        #region Mapeo de Campos

        public string Solicodi = "SOLICODI";
        public string Soliestado = "SOLIESTADO";
        public string SoliestadoInterno = "SOLIESTADOINTERNO";
        public string Solienviado = "SOLIENVIADO";
        public string Solifecenviado = "SOLIFECENVIADO";
        public string Solinotificado = "SOLINOTIFICADO";
        public string Solifecnotificado = "SOLIFECNOTIFICADO";
        public string Emprcodi = "EMPRCODI";
        public string Solifecsolicitud = "SOLIFECSOLICITUD";
        public string Soliusucreacion = "SOLIUSUCREACION";
        public string Solifeccreacion = "SOLIFECCREACION";
        public string Soliusumodificacion = "SOLIUSUMODIFICACION";
        public string Solifecmodificacion = "SOLIFECMODIFICACION";
        public string Tisocodi = "TISOCODI";
        public string Solifecproceso = "SOLIFECPROCESO";
        public string Soliususolicitud = "SOLIUSUSOLICITUD";
        public string Soliusuproceso = "SOLIUSUPROCESO";
        public string Soliobservacion = "SOLIOBSERVACION";
        public string Tisonombre = "TISONOMBRE";
        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string Emprnomb = "EMPRNOMBRECOMERCIAL";
        public string Emprabrev = "EMPRSIGLA";
        public string Horas = "HORAS";

        #endregion
    }
}
