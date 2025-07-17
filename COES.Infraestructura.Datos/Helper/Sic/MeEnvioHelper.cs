using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_ENVIO
    /// </summary>
    public class MeEnvioHelper : HelperBase
    {
        public MeEnvioHelper()
            : base(Consultas.MeEnvioSql)
        {
        }

        public MeEnvioDTO Create(IDataReader dr)
        {
            MeEnvioDTO entity = new MeEnvioDTO();

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iEnviofecha = dr.GetOrdinal(this.Enviofecha);
            if (!dr.IsDBNull(iEnviofecha)) entity.Enviofecha = dr.GetDateTime(iEnviofecha);

            int iEnviofechaperiodo = dr.GetOrdinal(this.Enviofechaperiodo);
            if (!dr.IsDBNull(iEnviofechaperiodo)) entity.Enviofechaperiodo = dr.GetDateTime(iEnviofechaperiodo);

            int iEnviofechaini = dr.GetOrdinal(this.Enviofechaini);
            if (!dr.IsDBNull(iEnviofechaini)) entity.Enviofechaini = dr.GetDateTime(iEnviofechaini);

            int iEnviofechafin = dr.GetOrdinal(this.Enviofechafin);
            if (!dr.IsDBNull(iEnviofechafin)) entity.Enviofechafin = dr.GetDateTime(iEnviofechafin);

            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            int iEnvioplazo = dr.GetOrdinal(this.Envioplazo);
            if (!dr.IsDBNull(iEnvioplazo)) entity.Envioplazo = dr.GetString(iEnvioplazo);

            int iUserlogin = dr.GetOrdinal(this.Userlogin);
            if (!dr.IsDBNull(iUserlogin)) entity.Userlogin = dr.GetString(iUserlogin);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iCfgenvcodi = dr.GetOrdinal(this.Cfgenvcodi);
            if (!dr.IsDBNull(iCfgenvcodi)) entity.Cfgenvcodi = Convert.ToInt32(dr.GetValue(iCfgenvcodi));

            int iFdatcodi = dr.GetOrdinal(this.Fdatcodi);
            if (!dr.IsDBNull(iFdatcodi)) entity.Fdatcodi = Convert.ToInt32(dr.GetValue(iFdatcodi));

            int iEnvionumbloques = dr.GetOrdinal(this.Envionumbloques);
            if (!dr.IsDBNull(iEnvionumbloques)) entity.Envionumbloques = Convert.ToInt32(dr.GetValue(iEnvionumbloques));

            int iEnvioorigen = dr.GetOrdinal(this.Envioorigen);
            if (!dr.IsDBNull(iEnvioorigen)) entity.Envioorigen = Convert.ToInt32(dr.GetValue(iEnvioorigen));

            int iEnviofechaplazoini = dr.GetOrdinal(this.Enviofechaplazoini);
            if (!dr.IsDBNull(iEnviofechaplazoini)) entity.Enviofechaplazoini = dr.GetDateTime(iEnviofechaplazoini);

            int iEnviofechaplazofin = dr.GetOrdinal(this.Enviofechaplazofin);
            if (!dr.IsDBNull(iEnviofechaplazofin)) entity.Enviofechaplazofin = dr.GetDateTime(iEnviofechaplazofin);

            int iEnviobloquehora = dr.GetOrdinal(this.Enviobloquehora);
            if (!dr.IsDBNull(iEnviobloquehora)) entity.Enviobloquehora = Convert.ToInt32(dr.GetValue(iEnviobloquehora));

            return entity;
        }


        #region Mapeo de Campos

        public string Enviocodi = "ENVIOCODI";
        public string Enviofecha = "ENVIOFECHA";
        public string Enviofechaperiodo = "ENVIOFECHAPERIODO";
        public string Enviofechaini = "ENVIOFECHAINI";
        public string Enviofechafin = "ENVIOFECHAFIN";
        public string Estenvcodi = "ESTENVCODI";
        public string Archcodi = "ARCHCODI";
        public string Envioplazo = "ENVIOPLAZO";
        public string Userlogin = "ENVIOUSUARIO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Emprcodi = "EMPRCODI";
        public string Formatcodi = "FORMATCODI";
        public string Estenvnombre = "ESTENVNOMBRE";
        public string Emprnomb = "EMPRNOMB";
        public string Emprruc = "EMPRRUC";
        public string Formatperiodo = "FORMATPERIODO";
        public string Cfgenvcodi = "CFGENVCODI";
        public string Formatnombre = "FORMATNOMBRE";
        public string Lectnomb = "LECTNOMB";
        public string Username = "USERNAME";
        public string Usertlf = "USERTLF";
        public string Modcodi = "MODCODI";
        public string Fdatcodi = "FDATCODI";

        /// <summary>
        /// Campos para aplicativo PR16
        /// </summary>
        public string EnviocodiAct = "ENVIOCODI_ACT";
        public string EnviocodiAnt = "ENVIOCODI_ANT";
        public string EnviofechaperiodoAct = "ENVIOFECHAPERIODO_ACT";
        public string EnviofechaperiodoAnt = "ENVIOFECHAPERIODO_ANT";
        public string EnviofechainiAct = "ENVIOFECHAINI_ACT";
        public string EnviofechafinAct = "ENVIOFECHAFIN_ACT";
        public string EnviofechainiAnt = "ENVIOFECHAINI_ANT";
        public string EnviofechafinAnt = "ENVIOFECHAFIN_ANT";
        public string IniRemision = "INI_REMISION";
        public string Periodo = "PERIODO";
        public string Item = "ITEM";
        public string Cumplimiento = "CUMPLIMIENTO";
        public string TipoEmpresa = "TIPO_EMPRESA";
        public string RucEmpresa = "RUC_EMPRESA";
        public string NombreEmpresa = "NOMBRE_EMPRESA";
        public string NroEnvios = "NRO_ENVIOS";
        public string FechaPrimerEnvio = "FECHA_PRIMER_ENVIO";
        public string FechaUltimoEnvio = "FECHA_ULTIMO_ENVIO";
        public string FinRemision = "FIN_REMISION";
        public string IniPeriodo = "INI_PERIODO";

        //- Agregado para reporte hidrologia
        public string Areacodi = "AREACODE";
        public string Areanomb = "AREANAME";
        public string Lectcodi = "LECTCODI";
        public string Lectnro = "LECTNRO";
        public string Lectperiodo = "LECTPERIODO";
        public string Formatresolucion = "FORMATRESOLUCION";
        public string Formathorizonte = "FORMATHORIZONTE";
        public string Ptomedicodi = "PTOMEDICODI";
        public string TipoInfoabrev = "TIPOINFOABREV";
        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string Lecttipo = "LECTTIPO";
        public string Formatdiaplazo = "FORMATDIAPLAZO";
        public string Formatminplazo = "FORMATMINPLAZO";
        public string Tipoinfocodi = "TIPOINFOCODI";

        //- Agregado para PMPO
        public string Envionumbloques = "ENVIONUMBLOQUES";
        public string Envioorigen = "ENVIOORIGEN";
        public string Enviofechaplazoini = "ENVIOFECHAPLAZOINI";
        public string Enviofechaplazofin = "ENVIOFECHAPLAZOFIN";
        public string MensajesPendientes = "MENSAJESPENDIENTES";
        public string MensajesPendientesCOES = "MENSAJESPENDIENTESCOES";
        public string ValidComentario = "VALIDCOMENTARIO";
        public string ComentarioOSINERGMIN = "ComentarioOSINERGMIN";
        public string EnvioCumplimiento = "ENVIOCUMP";
        public string Enviodesc = "Enviodesc";

        public string Hptominfila = "HPTOMINFILA";
        public string Hptodiafinplazo = "HPTODIAFINPLAZO";
        public string Hptominfinplazo = "HPTOMINFINPLAZO";
        public string Formatmesplazo = "FORMATMESPLAZO";
        public string Formatmesfinplazo = "FORMATMESFINPLAZO";
        public string Formatdiafinfueraplazo = "FORMATDIAFINFUERAPLAZO";
        public string Formatminfinfueraplazo = "FORMATMINFINFUERAPLAZO";
        public string Formatdiafinplazo = "FORMATDIAFINPLAZO";
        public string Formatminfinplazo = "FORMATMINFINPLAZO";
        public string Formatmesfinfueraplazo = "FORMATMESFINFUERAPLAZO";
        public string Formatcheckplazopunto = "FORMATCHECKPLAZOPUNTO";
        public string Enviobloquehora = "ENVIOBLOQUEHORA";

        //- Agregando para CTAF

        public string Envevencodi = "ENV_EVENCODI";
        public string Evencodi = "EVENCODI";
        public string Evenasunto = "EVENASUNTO";
        public string Evenini = "EVENINI";
        public string TipoInforme = "TIPOINFORME";
        public string Eveinfrutaarchivo = "EVEINFRUTAARCHIVO";
        #region Mejoras-RDO-II
        public string HorarioCodi = "HORARIOCODI";
        #endregion

        #endregion

        public string SqlGetByCriteriaRango
        {
            get { return base.GetSqlXml("GetByCriteriaRango"); }
        }

        public string SqlGetListaMultiple
        {
            get { return base.GetSqlXml("GetListaMultiple"); }
        }

        public string SqlUpdate1
        {
            get { return base.GetSqlXml("Update1"); }
        }

        public string SqlGetListaMultipleXLS
        {
            get { return base.GetSqlXml("GetListaMultipleXLS"); }
        }

        public string SqlTotalListaMultiple
        {
            get { return base.GetSqlXml("TotalListaMultiple"); }
        }

        public string SqlObtenerReporteEnvioCumplimiento
        {
            get { return base.GetSqlXml("ObtenerReporteEnvioCumplimiento"); }
        }

        public string SqlGetByMaxEnvioFormato
        {
            get { return base.GetSqlXml("GetByMaxEnvioFormato"); }
        }

        public string SqlGetByCriteriaRangoFecha
        {
            get { return base.GetSqlXml("GetByCriteriaRangoFecha"); }
        }

        public string SqlObtenerReporteCumplimiento
        {
            get { return base.GetSqlXml("ObtenerReporteCumplimiento"); }
        }

        public string SqlObtenerReporteCumplimientoXBloqueHorario
        {
            get { return base.GetSqlXml("ObtenerReporteCumplimientoXBloqueHorario"); }
        }

        public string SqlGetByMaxEnvioFormatoPeriodo
        {
            get { return base.GetSqlXml("GetByMaxEnvioFormatoPeriodo"); }
        }
       
        public string SqlObtenerListaEnvioActual
        {
            get { return base.GetSqlXml("ObtenerListaEnvioActual"); }
        }

        public string SqlObtenerListaPeriodoReporte
        {
            get { return base.GetSqlXml("ObtenerListaPeriodoReporte"); }
        }       

        public string SqlListReporteCumplimiento
        {
            get { return base.GetSqlXml("ListReporteCumplimiento"); }
        }

        public string SqlObtenerEnvioXModulo
        {
            get { return base.GetSqlXml("ObtenerEnvioXModulo"); }
        }

        public string SqlListaReporteCumplimientoHidrologia
        {
            get { return base.GetSqlXml("ListaReporteCumplimientoDeExtranetHidrologia"); }
        }

        #region SIOSEIN2 - NUMERALES

        public string SqlListaMeEnvioByFdat
        {
            get { return base.GetSqlXml("ListaMeEnvioByFdat"); }
        }

        public string SqlUpdate2
        {
            get { return base.GetSqlXml("Update2"); }
        }

        #endregion

        public string SqlUpdate3
        {
            get { return base.GetSqlXml("Update3"); }
        }

        #region MODIFICACIONES ASSETEC - PARA INTERVENCIONES
        public string SqlEliminarEnvioFisicoPorId
        {
            get { return base.GetSqlXml("EliminarEnvioFisicoPorId"); }
        }
        #endregion

        #region Aplicativo Extranet CTAF
        public string SqlObtenerEnvioInterrupSuministro
        {
            get { return base.GetSqlXml("ObtenerEnvioInterrupSuministro"); }
        }
        #endregion

        #region MEJORAS CTAF
        public string SqlListaEnviosPorEvento
        {
            get { return base.GetSqlXml("ListaEnviosPorEvento"); }
        }

        public string SqlListaInformeEnvios
        {
            get { return base.GetSqlXml("ListaInformeEnvios"); }
        }
        public string SqlListaInformeEnviosLog
        {
            get { return base.GetSqlXml("ListaInformeEnviosLog"); }
        }
        #endregion
    }
}
