using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_EVENTO
    /// </summary>
    public class EveEventoHelper : HelperBase
    {
        public EveEventoHelper()
            : base(Consultas.EveEventoSql)
        {
        }

        public EveEventoDTO Create(IDataReader dr)
        {
            EveEventoDTO entity = new EveEventoDTO();

            int iEvencomentarios = dr.GetOrdinal(this.Evencomentarios);
            if (!dr.IsDBNull(iEvencomentarios)) entity.Evencomentarios = dr.GetString(iEvencomentarios);

            int iEvenperturbacion = dr.GetOrdinal(this.Evenperturbacion);
            if (!dr.IsDBNull(iEvenperturbacion)) entity.Evenperturbacion = dr.GetString(iEvenperturbacion);

            int iTwitterenviado = dr.GetOrdinal(this.Twitterenviado);
            if (!dr.IsDBNull(iTwitterenviado)) entity.Twitterenviado = dr.GetString(iTwitterenviado);

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iEmprcodirespon = dr.GetOrdinal(this.Emprcodirespon);
            if (!dr.IsDBNull(iEmprcodirespon)) entity.Emprcodirespon = Convert.ToInt32(dr.GetValue(iEmprcodirespon));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iEvenini = dr.GetOrdinal(this.Evenini);
            if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

            int iEvenmwindisp = dr.GetOrdinal(this.Evenmwindisp);
            if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

            int iEvenfin = dr.GetOrdinal(this.Evenfin);
            if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iEvenasunto = dr.GetOrdinal(this.Evenasunto);
            if (!dr.IsDBNull(iEvenasunto)) entity.Evenasunto = dr.GetString(iEvenasunto);

            int iEvenpadre = dr.GetOrdinal(this.Evenpadre);
            if (!dr.IsDBNull(iEvenpadre)) entity.Evenpadre = Convert.ToInt32(dr.GetValue(iEvenpadre));

            int iEveninterrup = dr.GetOrdinal(this.Eveninterrup);
            if (!dr.IsDBNull(iEveninterrup)) entity.Eveninterrup = dr.GetString(iEveninterrup);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iEvenpreini = dr.GetOrdinal(this.Evenpreini);
            if (!dr.IsDBNull(iEvenpreini)) entity.Evenpreini = dr.GetDateTime(iEvenpreini);

            int iEvenpostfin = dr.GetOrdinal(this.Evenpostfin);
            if (!dr.IsDBNull(iEvenpostfin)) entity.Evenpostfin = dr.GetDateTime(iEvenpostfin);

            int iEvendesc = dr.GetOrdinal(this.Evendesc);
            if (!dr.IsDBNull(iEvendesc)) entity.Evendesc = dr.GetString(iEvendesc);

            int iEventension = dr.GetOrdinal(this.Eventension);
            if (!dr.IsDBNull(iEventension)) entity.Eventension = dr.GetDecimal(iEventension);

            int iEvenaopera = dr.GetOrdinal(this.Evenaopera);
            if (!dr.IsDBNull(iEvenaopera)) entity.Evenaopera = dr.GetString(iEvenaopera);

            int iEvenpreliminar = dr.GetOrdinal(this.Evenpreliminar);
            if (!dr.IsDBNull(iEvenpreliminar)) entity.Evenpreliminar = dr.GetString(iEvenpreliminar);

            int iEvenrelevante = dr.GetOrdinal(this.Evenrelevante);
            if (!dr.IsDBNull(iEvenrelevante)) entity.Evenrelevante = Convert.ToInt32(dr.GetValue(iEvenrelevante));

            int iEvenctaf = dr.GetOrdinal(this.Evenctaf);
            if (!dr.IsDBNull(iEvenctaf)) entity.Evenctaf = dr.GetString(iEvenctaf);

            int iEveninffalla = dr.GetOrdinal(this.Eveninffalla);
            if (!dr.IsDBNull(iEveninffalla)) entity.Eveninffalla = dr.GetString(iEveninffalla);

            int iEveninffallan2 = dr.GetOrdinal(this.Eveninffallan2);
            if (!dr.IsDBNull(iEveninffallan2)) entity.Eveninffallan2 = dr.GetString(iEveninffallan2);

            int iDeleted = dr.GetOrdinal(this.Deleted);
            if (!dr.IsDBNull(iDeleted)) entity.Deleted = dr.GetString(iDeleted);

            int iEventipofalla = dr.GetOrdinal(this.Eventipofalla);
            if (!dr.IsDBNull(iEventipofalla)) entity.Eventipofalla = dr.GetString(iEventipofalla);

            int iEventipofallafase = dr.GetOrdinal(this.Eventipofallafase);
            if (!dr.IsDBNull(iEventipofallafase)) entity.Eventipofallafase = dr.GetString(iEventipofallafase);

            int iSmsenviado = dr.GetOrdinal(this.Smsenviado);
            if (!dr.IsDBNull(iSmsenviado)) entity.Smsenviado = dr.GetString(iSmsenviado);

            int iSmsenviar = dr.GetOrdinal(this.Smsenviar);
            if (!dr.IsDBNull(iSmsenviar)) entity.Smsenviar = dr.GetString(iSmsenviar);

            int iEvenactuacion = dr.GetOrdinal(this.Evenactuacion);
            if (!dr.IsDBNull(iEvenactuacion)) entity.Evenactuacion = dr.GetString(iEvenactuacion);

            int iSubcausacodiop = dr.GetOrdinal(this.Subcausacodiop);
            if (!dr.IsDBNull(iSubcausacodiop)) entity.Subcausacodiop = Convert.ToInt32(dr.GetValue(iSubcausacodiop));

            int iEvenmwgendescon = dr.GetOrdinal(this.Evenmwgendescon);
            if (!dr.IsDBNull(iEvenmwgendescon)) entity.Evenmwgendescon = dr.GetDecimal(iEvenmwgendescon);

            int iEvengendescon = dr.GetOrdinal(this.Evengendescon);
            if (!dr.IsDBNull(iEvengendescon)) entity.Evengendescon = dr.GetString(iEvengendescon);

            int iEveAdjunto = dr.GetOrdinal(this.EveAdjunto);
            if (!dr.IsDBNull(iEveAdjunto)) entity.EveAdjunto = dr.GetString(iEveAdjunto);

            return entity;
        }


        #region Mapeo de Campos

        public string Evencomentarios = "EVENCOMENTARIOS";
        public string Evenperturbacion = "EVENPERTURBACION";
        public string Twitterenviado = "TWITTERENVIADO";
        public string Evencodi = "EVENCODI";
        public string Emprcodirespon = "EMPRCODIRESPON";
        public string Equicodi = "EQUICODI";
        public string Evenclasecodi = "EVENCLASECODI";
        public string Emprcodi = "EMPRCODI";
        public string Tipoevencodi = "TIPOEVENCODI";
        public string Evenini = "EVENINI";
        public string Evenmwindisp = "EVENMWINDISP";
        public string Evenfin = "EVENFIN";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Evenasunto = "EVENASUNTO";
        public string Evenpadre = "EVENPADRE";
        public string Eveninterrup = "EVENINTERRUP";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Evenpreini = "EVENPREINI";
        public string Evenpostfin = "EVENPOSTFIN";
        public string Evendesc = "EVENDESC";
        public string Eventension = "EVENTENSION";
        public string Evenaopera = "EVENAOPERA";
        public string Evenpreliminar = "EVENPRELIMINAR";
        public string Evenrelevante = "EVENRELEVANTE";
        public string Evenctaf = "EVENCTAF";
        public string Eveninffalla = "EVENINFFALLA";
        public string Eveninffallan2 = "EVENINFFALLAN2";
        public string Deleted = "DELETED";
        public string Eventipofalla = "EVENTIPOFALLA";
        public string Eventipofallafase = "EVENTIPOFALLAFASE";
        public string Smsenviado = "SMSENVIADO";
        public string Smsenviar = "SMSENVIAR";
        public string Evenactuacion = "EVENACTUACION";
        public string Equiabrev = "EQUIABREV";
        public string Tipoevenabrev = "TIPOEVENABREV";
        public string Emprnomb = "EMPRNOMB";
        public string Tareaabrev = "TAREAABREV";
        public string Areanomb = "AREANOMB";
        public string Indinforme = "INDINFORME";
        public string Tiporegistro = "TIPOREGISTRO";
        public string Valtiporegistro = "VALTIPOREGISTRO";
        public string Codeve = "CODEVE";
        public string Subcausacodiop = "SUBCAUSACODIOP";
        public string Equinomb = "EQUINOMB";
        public string Famnomb = "FAMNOMB";
        public string Evenmwgendescon = "EVENMWGENDESCON";
        public string Evengendescon = "EVENGENDESCON";
        public string Evenasegoperacion = "EVENASEGOPERACION";
        public string EveAdjunto = "EVEADJUNTO";

        #region PR5
        public string Famcodi = "FAMCODI";
        public string Causaevencodi = "CAUSAEVENCODI";
        public string Causaevendesc = "CAUSAEVENDESC";
        public string Eveninterrupmw = "EVENINTERRUPMW";
        public string Evenenergia = "EVENENERGIA";
        public string Causaevenabrev = "CAUSAEVENABREV";


        #endregion

        #region SIOSEIN

        public string Interrupcion = "INTERRUPCION";
        public string Interrminu = "INTERRMINU";
        public string Interrmw = "INTERRMW";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Osinergcodi = "OSINERGCODI";
        public string Subcausadesc = "SUBCAUSADESC";

        #endregion

        #region SIOSEIN-PRIE-2021
        public string Emprsein = "EMPRSEIN";
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string Emprabrev = "Emprabrev";
        public string Evenbajomw = "Evenbajomw";
        #endregion

        #region Mejoras CTAF
        public string Eveninfplazodiasipi = "EVENINFPLAZODIASIPI";
        public string Eveninfplazodiasif = "EVENINFPLAZODIASIF";
        public string Eveninfplazohoraipi = "EVENINFPLAZOHORAIPI";
        public string Eveninfplazohoraif = "EVENINFPLAZOHORAIF";
        public string Eveninfplazominipi = "EVENINFPLAZOMINIPI";
        public string Eveninfplazominif = "EVENINFPLAZOMINIF";
        public string Eveninfplazodiasipi_N2 = "EVENINFPLAZODIASIPI_N2";
        public string Eveninfplazodiasif_N2 = "EVENINFPLAZODIASIF_N2";
        public string Eveninfplazohoraipi_N2 = "EVENINFPLAZOHORAIPI_N2";
        public string Eveninfplazohoraif_N2 = "EVENINFPLAZOHORAIF_N2";
        public string Eveninfplazominipi_N2 = "EVENINFPLAZOMINIPI_N2";
        public string Eveninfplazominif_N2 = "EVENINFPLAZOMINIF_N2";
        public string Evencodi_as = "EVENCODI_AS";
        public string Evenrcmctaf = "EVENRCMCTAF";
        public string AFECODI = "AFECODI";
        public string CRESPECIALCODI = "CRESPECIALCODI";
        public string CRCRITERIOCODI = "CRCRITERIOCODI";
        public string LASTDATE = "LASTDATE";
        public string LASTUSER = "LASTUSER";
        #endregion

        public string SqlObtenerConsultaExtranet
        {
            get { return base.GetSqlXml("ObtenerConsultaExtranet"); }
        }

        public string SqlObtenerNroRegistrosConsultaExtranet
        {
            get { return base.GetSqlXml("ObtenerNroRegistrosConsultaExtranet"); }
        }

        public string SqlGetDetalleEvento
        {
            get { return base.GetSqlXml("GetDetalleEvento"); }
        }

        public string SqlCambiarVersion
        {
            get { return base.GetSqlXml("CambiarVersion"); }
        }

        public string SqlListEventosCarga
        {
            get { return base.GetSqlXml("ListEventosCarga"); }
        }

        public string SqlListarResumenEventosWeb
        {
            get { return base.GetSqlXml("ListarResumenEventosWeb"); }
        }

        public string SqlGeEventoEquipo
        {
            get { return base.GetSqlXml("GetEventoEquipo"); }
        }

        #endregion

        #region PR5
        public string SqlListarReporteEventoIOED
        {
            get { return base.GetSqlXml("ListarReporteEventoIOED"); }
        }

        public string SqlGetEventosCausaSubCausa
        {
            get { return base.GetSqlXml("GetEventosCausaSubCausa"); }
        }

        #endregion

        #region SIOSEIN

        public string SqlObtenerEventosConInterrupciones
        {
            get { return base.GetSqlXml("ObtenerEventosConInterrupciones"); }
        }

        public string SqlGetListaHechosRelevantes
        {
            get { return base.GetSqlXml("GetListaHechosRelevantes"); }
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListaEventosImportantes
        {
            get { return base.GetSqlXml("ListaEventosImportantes"); }
        }
        #endregion

        public string SqlActualizarEventoAseguramiento
        {
            get { return base.GetSqlXml("ActualizarEventoAseguramiento"); }
        }

        #region Mejoras CTAF
        public string SqlListadoEventoSco
        {
            get { return base.GetSqlXml("ListadoEventoSco"); }
        }
        public string SqlListadoEventosAsoCtaf
        {
            get { return base.GetSqlXml("ListadoEventosAsoCtaf"); }
        }
        public string SqlObtieneCantFileEnviadosSco
        {
            get { return base.GetSqlXml("ObtieneCantFileEnviadosSco"); }
        }
        #endregion
    }

}
