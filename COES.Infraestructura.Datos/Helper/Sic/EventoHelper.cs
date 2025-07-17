using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EventoHelper : HelperBase
    {
        public EventoHelper()
            : base(Consultas.EventoSql)
        {

        }

        public EventoDTO Create(IDataReader dr)
        {
            EventoDTO entity = new EventoDTO();

            int iEQUIABREV = dr.GetOrdinal(this.EQUIABREV);
            if (!dr.IsDBNull(iEQUIABREV)) entity.EQUIABREV = dr.GetString(iEQUIABREV);

            int iEVENCODI = dr.GetOrdinal(this.EVENCODI);
            if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetValue(iEVENCODI));

            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = Convert.ToInt32(dr.GetValue(iEMPRCODI));

            int iEQUICODI = dr.GetOrdinal(this.EQUICODI);
            if (!dr.IsDBNull(iEQUICODI)) entity.EQUICODI = Convert.ToInt32(dr.GetValue(iEQUICODI));

            int iTIPOEVENCODI = dr.GetOrdinal(this.TIPOEVENCODI);
            if (!dr.IsDBNull(iTIPOEVENCODI)) entity.TIPOEVENCODI = Convert.ToInt32(dr.GetValue(iTIPOEVENCODI));

            int iEVENINI = dr.GetOrdinal(this.EVENINI);
            if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

            int iEVENFIN = dr.GetOrdinal(this.EVENFIN);
            if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

            int iEVENPADRE = dr.GetOrdinal(this.EVENPADRE);
            if (!dr.IsDBNull(iEVENPADRE)) entity.EVENPADRE = Convert.ToInt32(dr.GetValue(iEVENPADRE));

            int iFAMCODI = dr.GetOrdinal(this.FAMCODI);
            if (!dr.IsDBNull(iFAMCODI)) entity.FAMCODI = Convert.ToInt32(dr.GetValue(iFAMCODI));

            int iAREACODI = dr.GetOrdinal(this.AREACODI);
            if (!dr.IsDBNull(iAREACODI)) entity.AREACODI = Convert.ToInt32(dr.GetValue(iAREACODI));

            int iSUBCAUSAABREV = dr.GetOrdinal(this.SUBCAUSAABREV);
            if (!dr.IsDBNull(iSUBCAUSAABREV)) entity.SUBCAUSAABREV = dr.GetString(iSUBCAUSAABREV);

            int iSUBCAUSACODI = dr.GetOrdinal(this.SUBCAUSACODI);
            if (!dr.IsDBNull(iSUBCAUSACODI)) entity.SUBCAUSACODI = Convert.ToInt32(dr.GetValue(iSUBCAUSACODI));

            int iTIPOEVENABREV = dr.GetOrdinal(this.TIPOEVENABREV);
            if (!dr.IsDBNull(iTIPOEVENABREV)) entity.TIPOEVENABREV = dr.GetString(iTIPOEVENABREV);

            int iAREANOMB = dr.GetOrdinal(this.AREANOMB);
            if (!dr.IsDBNull(iAREANOMB)) entity.AREANOMB = dr.GetString(iAREANOMB);

            int iAREADESC = dr.GetOrdinal(this.AREADESC);
            if (!dr.IsDBNull(iAREADESC)) entity.AREADESC = dr.GetString(iAREADESC);

            int iEVENINTERRUP = dr.GetOrdinal(this.EVENINTERRUP);
            if (!dr.IsDBNull(iEVENINTERRUP)) entity.EVENINTERRUP = dr.GetString(iEVENINTERRUP);

            int iFAMABREV = dr.GetOrdinal(this.FAMABREV);
            if (!dr.IsDBNull(iFAMABREV)) entity.FAMABREV = dr.GetString(iFAMABREV);

            //int iFAMNOMB = dr.GetOrdinal(this.FAMNOMB);
            //if (!dr.IsDBNull(iFAMNOMB)) entity.FAMNOMB = dr.GetString(iFAMNOMB);

            int iTAREAABREV = dr.GetOrdinal(this.TAREAABREV);
            if (!dr.IsDBNull(iTAREAABREV)) entity.TAREAABREV = dr.GetString(iTAREAABREV);

            int iEMPRNOMB = dr.GetOrdinal(this.EMPRNOMB);
            if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);

            int iTAREACODI = dr.GetOrdinal(this.TAREACODI);
            if (!dr.IsDBNull(iTAREACODI)) entity.TAREACODI = Convert.ToInt32(dr.GetValue(iTAREACODI));

            int iLASTUSER = dr.GetOrdinal(this.LASTUSER);
            if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);

            int iLASTDATE = dr.GetOrdinal(this.LASTDATE);
            if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);

            int iEVENASUNTO = dr.GetOrdinal(this.EVENASUNTO);
            if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);

            int iEVENPRELIMINAR = dr.GetOrdinal(this.EVENPRELIMINAR);
            if (!dr.IsDBNull(iEVENPRELIMINAR)) entity.EVENPRELIMINAR = dr.GetString(iEVENPRELIMINAR);

            int iCAUSAEVENABREV = dr.GetOrdinal(this.CAUSAEVENABREV);
            if (!dr.IsDBNull(iCAUSAEVENABREV)) entity.CAUSAEVENABREV = dr.GetString(iCAUSAEVENABREV);

            int iEVENRELEVANTE = dr.GetOrdinal(this.EVENRELEVANTE);
            if (!dr.IsDBNull(iEVENRELEVANTE)) entity.EVENRELEVANTE = Convert.ToInt32(dr.GetValue(iEVENRELEVANTE));

            int iTIPOEMPRDESC = dr.GetOrdinal(this.TIPOEMPRDESC);
            if (!dr.IsDBNull(iTIPOEMPRDESC)) entity.TIPOEMPRDESC = dr.GetString(iTIPOEMPRDESC);

            int iEQUITENSION = dr.GetOrdinal(this.EQUITENSION);
            if (!dr.IsDBNull(iEQUITENSION)) entity.EQUITENSION = dr.GetDecimal(iEQUITENSION);

            int iEVENTENSION = dr.GetOrdinal(this.EVENTENSION);
            if (!dr.IsDBNull(iEVENTENSION)) entity.EVENTENSION = dr.GetDecimal(iEVENTENSION);

            int iEVENMWINDISP = dr.GetOrdinal(this.EVENMWINDISP);
            if (!dr.IsDBNull(iEVENMWINDISP)) entity.EVENMWINDISP = dr.GetDecimal(iEVENMWINDISP);

            int iEMPRABREV = dr.GetOrdinal(this.EMPRABREV);
            if (!dr.IsDBNull(iEMPRABREV)) entity.EMPRABREV = dr.GetString(iEMPRABREV);

            int iEVENAOPERA = dr.GetOrdinal(this.EVENAOPERA);
            if (!dr.IsDBNull(iEVENAOPERA)) entity.EVENAOPERA = dr.GetString(iEVENAOPERA);

            int iEVENCOMENTARIOS = dr.GetOrdinal(this.EVENCOMENTARIOS);
            if (!dr.IsDBNull(iEVENCOMENTARIOS)) entity.EVENCOMENTARIOS = dr.GetString(iEVENCOMENTARIOS);

            int iEVENMWGENDESCON = dr.GetOrdinal(this.EVENMWGENDESCON);
            if (!dr.IsDBNull(iEVENMWGENDESCON)) entity.EVENMWGENDESCON = dr.GetDecimal(iEVENMWGENDESCON);

            int iEVENGENDESCON = dr.GetOrdinal(this.EVENGENDESCON);
            if (!dr.IsDBNull(iEVENGENDESCON)) entity.EVENGENDESCON = dr.GetString(iEVENGENDESCON);

            //int iENERGIAINTERRUMPIDA = dr.GetOrdinal(this.ENERGIAINTERRUMPIDA);
            //if (!dr.IsDBNull(iENERGIAINTERRUMPIDA)) entity.ENERGIAINTERRUMPIDA = dr.GetDecimal(iENERGIAINTERRUMPIDA);

            //int iINTERRUPCIONMW = dr.GetOrdinal(this.INTERRUPCIONMW);
            //if (!dr.IsDBNull(iINTERRUPCIONMW)) entity.INTERRUPCIONMW = dr.GetDecimal(iINTERRUPCIONMW);

            //int iDISMINUCIONMW = dr.GetOrdinal(this.DISMINUCIONMW);
            //if (!dr.IsDBNull(iDISMINUCIONMW)) entity.DISMINUCIONMW = dr.GetDecimal(iDISMINUCIONMW);           

            //if (entity.EVENFIN != null)
            //{
            //    TimeSpan ts = ((DateTime)entity.EVENFIN).Subtract((DateTime)entity.EVENINI);
            //    entity.DURACION = ts.TotalMinutes;
            //}


            return entity;
        }

        public string EQUIABREV = "EQUIABREV";
        public string EVENCODI = "EVENCODI";
        public string EMPRCODI = "EMPRCODI";
        public string EQUICODI = "EQUICODI";
        public string TIPOEMPRCODI = "TIPOEMPRCODI";
        public string TIPOEVENCODI = "TIPOEVENCODI";
        public string EVENINI = "EVENINI";
        public string EVENFIN = "EVENFIN";
        public string EVENPADRE = "EVENPADRE";
        public string FAMCODI = "FAMCODI";
        public string FAMNOMB = "FAMNOMB";
        public string AREACODI = "AREACODI";
        public string SUBCAUSAABREV = "SUBCAUSAABREV";
        public string SUBCAUSACODI = "SUBCAUSACODI";
        public string TIPOEVENABREV = "TIPOEVENABREV";
        public string AREANOMB = "AREANOMB";
        public string AREADESC = "AREADESC";
        public string EVENINTERRUP = "EVENINTERRUP";
        public string FAMABREV = "FAMABREV";
        public string TAREAABREV = "TAREAABREV";
        public string EMPRNOMB = "EMPRNOMB";
        public string RAZSOCIAL = "RAZSOCIAL";
        public string TAREACODI = "TAREACODI";
        public string LASTUSER = "LASTUSER";
        public string LASTDATE = "LASTDATE";
        public string EVENASUNTO = "EVENASUNTO";
        public string EVENPRELIMINAR = "EVENPRELIMINAR";
        public string CAUSAEVENABREV = "CAUSAEVENABREV";
        public string EVENRELEVANTE = "EVENRELEVANTE";
        public string EMPRCODIRESPON = "EMPRCODIRESPON";
        public string EVENCLASECODI = "EVENCLASECODI";
        public string EVENMWINDISP = "EVENMWINDISP";
        public string EVENPREINI = "EVENPREINI";
        public string EVENPOSTFIN = "EVENPOSTFIN";
        public string EVENDESC = "EVENDESC";
        public string MWINTERRUMPIDOS = "MWINTERRUMPIDOS";
        public string EVENTENSION = "EVENTENSION";
        public string EVENAOPERA = "EVENAOPERA";
        public string EVENCTAF = "EVENCTAF";
        public string EVENINFFALLA = "EVENINFFALLA";
        public string EVENINFFALLAN2 = "EVENINFFALLAN2";
        public string DELETED = "DELETED";
        public string EVENTIPOFALLA = "EVENTIPOFALLA";
        public string EVENTIPOFALLAFASE = "EVENTIPOFALLAFASE";
        public string SMSENVIADO = "SMSENVIADO";
        public string SMSENVIAR = "SMSENVIAR";
        public string EVENACTUACION = "EVENACTUACION";
        public string EQUINOMB = "EQUINOMB";
        public string EVENCOMENTARIOS = "EVENCOMENTARIOS";
        public string EVENPERTURBACION = "EVENPERTURBACION";
        public string TIPOEMPRDESC = "TIPOEMPRDESC";
        public string EQUITENSION = "EQUITENSION";
        public string EQUIPOT = "EQUIPOT";
        public string ENERGIAINTERRUMPIDA = "ENERGIAINTERRUMPIDA";
        public string INTERRUPCIONMW = "INTERRUPCIONMW";
        public string DISMINUCIONMW = "DISMINUCIONMW";
        public string EMPRABREV = "EMPRABREV";
        public string TIPOEMPRNOMB = "TIPOEMPRNOMB";
        public string TIPOREGISTRO = "TIPOREGISTRO";
        public string VALTIPOREGISTRO = "VALTIPOREGISTRO";
        public string EVENMWGENDESCON = "EVENMWGENDESCON";
        public string EVENGENDESCON = "EVENGENDESCON";
        //- Campos agregados para reporte extendido
        public string Eventipofalla = "Eventipofalla";
        public string Eventipofallafase = "Eventipofallafase";
        public string Interrmwde = "Interrmw_de";
        public string Interrmwa = "Interrmw_a";
        public string Interrminu = "Interrminu";
        public string Interrmw = "Interrmw";
        public string BAJOMW = "BAJOMW";
        public string Interrdesc = "Interrdesc";
        public string Interrupcodi = "Interrupcodi";
        public string Ptointerrcodi = "Ptointerrcodi";
        public string Interrnivel = "Interrnivel";
        public string Interrracmf = "Interrracmf";
        public string Interrmfetapadesc = "Interrmfetapadesc";
        public string Interrmanualr = "Interrmanualr";
        public string Ptointerrupnomb = "Ptointerrupnomb";
        public string Ptoentrenomb = "Ptoentrenomb";
        public string Clientenomb = "Clientenomb";
        public string SCADACODI = "SCADACODI";
        public string EMPRESTADO = "EMPRESTADO";
        public string Causaevendesc = "CAUSAEVENDESC";
        public string EquicodiInvolucrado = "EQUICODIINVOLUCRADO";
        public string Grupotipocogen = "Grupotipocogen";

        //SIOSEIN2
        public string Causaevencodi = "CAUSAEVENCODI";

        //campos interrupción suministros
        public string AFECODI = "SCADACODI";
        public string Afecodi = "AFECODI";
        public string Afefechainterr = "AFEFECHAINTERR";
        public string Afeplazofecha = "AFEPLAZOFECHA";
        public string Afeplazofechaampl = "AFEPLAZOFECHAAMPL";
        public string Afeplazousumodificacion = "AFEPLAZOUSUMODIFICACION";
        public string Afeplazofecmodificacion = "AFEPLAZOFECMODIFICACION";
        public string Afeeracmf = "AFEERACMF";
        public string EVENDESCCTAF = "EVENDESCCTAF";
        #region Mejoras CTAF
        public string CodigoCtaf = "CODIGOCTAF";
        public string CodigoDirector = "DIRCODI";
        public string NombreCompletoDirector = "NOMBRECOMPLETO";
        public string CodigoResponsable = "EVERESPONCODI";
        public string NombreCompletoResponsable = "NOMBRE_RESPONSABLE";
        public string EstadoResponsable = "ESTADO";
        public string NombreArchivoFirma = "REPRUTAFIRMA";
        public string EstadoEditarResponsable = "REPESTADO";
        public string Evendescctaf = "EVENDESCCTAF";

        public string CodigoSenializacion = "EVESENIALACODI";
        public string CodigoEvento = "EVENCODI";
        public string SubEstacion = "SUBESTACION";
        public string Equipo = "EQUIPO";
        public string Codigo = "CODIGO";
        public string Senializaciones = "SENIALIZACIONES";
        public string Interruptor = "INTERRUPTOR";
        public string CodigoAC = "AC";
        public string UsuarioCreacion = "LASTUSER";
        public string FechaCreacion = "LASTDATE";

        public string Dircorreo = "DIRCORREO";
        public string Direstado = "DIRESTADO";
        public string Repfirma = "REPFIRMA";
        #endregion

        public string SqlBusquedaEquipoEvento
        {
            get { return base.GetSqlXml("SqlBuscarEquipo"); }
        }

        public string SqlBusquedaEquipoEventoExtranet
        {
            get { return base.GetSqlXml("SqlBuscarEquipoExtranet"); }
        }

        public string SqlBusquedaEquipoNoPermitidos
        {
            get { return base.GetSqlXml("SqlBuscarEquipoNoPermitidos"); }
        }

        public string SqlBusquedaEquiposIntervencionesActualizados
        {
            get { return base.GetSqlXml("SqlEquiposActualizadosIntervenciones"); }
        }

        public string SqlBusquedaEquipoEventoIntervenciones
        {
            get { return base.GetSqlXml("SqlBuscarEquipoIntervenciones"); }
        }

        public string SqlTotalRecordsEquipo
        {
            get { return base.GetSqlXml("SqlObtenerNroRegistroEquipo"); }
        }

        public string SqlTotalRecordsEquipoExtranet
        {
            get { return base.GetSqlXml("SqlObtenerNroRegistroEquipoExtranet"); }
        }

        public string SqlActualizarInformePerturbacion
        {
            get { return base.GetSqlXml("SqlActualizarInformePerturbacion"); }
        }

        public string SqlListarEmpresas
        {
            get { return base.GetSqlXml("SqlListarEmpresas"); }
        }

        public string SqlListarEmpresasPorTipo
        {
            get { return base.GetSqlXml("SqlListarEmpresasPorTipo"); }
        }

        public string SqlListarFamilias
        {
            get { return base.GetSqlXml("SqlListarFamilias"); }
        }

        public string SqlObtenerEvento
        {
            get { return base.GetSqlXml("SqlObtenerEvento"); }
        }

        public string SqlObtenerResumenEvento
        {
            get { return base.GetSqlXml("SqlObtenerResumenEvento"); }
        }

        public string SqlObtenerAreaPorEmpresa
        {
            get { return base.GetSqlXml("SqlObtenerAreaPorEmpresa"); }
        }

        public string SqlExportarEventos
        {
            get { return base.GetSqlXml("SqlExportarEventos"); }
        }

        public string SqlExportarEventosPortal
        {
            get { return base.GetSqlXml("SqlExportarEventosPortal"); }
        }

        public string SqlGetByCriteriaPortal
        {
            get { return base.GetSqlXml("GetByCriteriaPortal"); }
        }

        public string SqlTotalRecordsPortal
        {
            get { return base.GetSqlXml("TotalRecordsPortal"); }
        }

        public string SqlExportarEventosDetallado
        {
            get { return base.GetSqlXml("ExportarEventosDetallado"); }
        }

        #region FIT - SGOCOES func A

        public string SqlObtenerEmpresaPropietaria
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaPropietaria"); }
        }

        public string SqlObtenerEmpresasInvolucrada
        {
            get { return base.GetSqlXml("SqlObtenerEmpresasInvolucrada"); }
        }
        public string SqlObtenerEmpresasRecomendacion
        {
            get { return base.GetSqlXml("SqlObtenerEmpresasRecomendacion"); }
        }
        public string SqlObtenerEmpresasObservacion
        {
            get { return base.GetSqlXml("SqlObtenerEmpresasObservacion"); }
        }
        public string SqlObtenerTipoEquipo
        {
            get { return base.GetSqlXml("SqlObtenerTipoEquipo"); }
        }
        public string SqlConsultarAnalisisFallas
        {
            get { return base.GetSqlXml("SqlConsultarAnalisisFallas"); }
        }
        
        public string SqlBuscarEmpresa
        {
            get { return base.GetSqlXml("SqlBuscarEmpresa"); }
        }
        public string SqlObtenerEmpresa
        {
            get { return base.GetSqlXml("SqlObtenerEmpresa"); }
        }
        public string SqlBuscarEmpresaPropietaria
        {
            get { return base.GetSqlXml("SqlBuscarEmpresaPropietaria"); }
        }
        public string SqlObtenerResponsableEvento
        {
            get { return base.GetSqlXml("SqlObtenerResponsableEvento"); }
        }
        public string SqlObtenerReunionResponsable
        {
            get { return base.GetSqlXml("SqlObtenerReunionResponsable"); }
        }

        public string SqlObtenerAnalisisFalla
        {
            get { return base.GetSqlXml("SqlObtenerAnalisisFalla"); }
        }

        public string SqlObtenerCodigosEventosPorFechaNominal
        {
            get { return base.GetSqlXml("SqlObtenerCodigosEventosPorFechaNominal"); }
        }

        public string SqlObtenerCodigosEventosPorFechaReunion
        {
            get { return base.GetSqlXml("SqlObtenerCodigosEventosPorFechaReunion"); }
        }

        public string SqlObtenerCodigosEventosPorFechaElaboracionInformeTecnico
        {
            get { return base.GetSqlXml("SqlObtenerCodigosEventosPorFechaElaboracionInformeTecnico"); }
        }

        public string SqlObtenerCodigosEventosPorFechaNominalSemanal
        {
            get { return base.GetSqlXml("SqlObtenerCodigosEventosPorFechaNominalSemanal"); }
        }

        public string SqlObtenerCodigosEventosPorFechaReunionSemanal
        {
            get { return base.GetSqlXml("SqlObtenerCodigosEventosPorFechaReunionSemanal"); }
        }

        public string SqlObtenerCodigosEventosPorFechaElaboracionInformeTecnicoSemanal
        {
            get { return base.GetSqlXml("SqlObtenerCodigosEventosPorFechaElaboracionInformeTecnicoSemanal"); }
        }

        public string SqlObtenerAnalisisFallaCompleto
        {
            get { return base.GetSqlXml("SqlObtenerAnalisisFallaCompleto"); }
        }

        public string SqlObtenerAnalisisFallaCompleto2
        {
            get { return base.GetSqlXml("SqlObtenerAnalisisFallaCompleto2"); }
        }

        public string SqlObtenerAnalisisFalla2
        {
            get { return base.GetSqlXml("SqlObtenerAnalisisFalla2"); }
        }

        public string SqlObtenerEquipoPorEvento
        {
            get { return base.GetSqlXml("SqlObtenerEquipoPorEvento"); }
        }
        public string SqlObtenerEmpresaInvolucrada
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaInvolucrada"); }
        }
        public string SqlObtenerEmpresaInvolucradaReunion
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaInvolucradaReunion"); }
        }
        public string SqlObtenerListaReunionResponsable
        {
            get { return base.GetSqlXml("SqlObtenerListaReunionResponsable"); }
        }
        public string SqlObtenerListaSecuenciaEvento
        {
            get { return base.GetSqlXml("SqlObtenerListaSecuenciaEvento"); }
        }
        public string SqlObtenerEmpresaRecomendacionInformeTecnico
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaRecomendacionInformeTecnico"); }
        }
        public string SqlObtenerEmpresaObservacionInformeTecnico
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaObservacionInformeTecnico"); }
        }
        public string SqlObtenerEmpresaFuerzaMayorInformeTecnico
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaFuerzaMayorInformeTecnico"); }
        }
        public string SqlObtenerEmpresaResponsableCompensacion
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaResponsableCompensacion"); }
        }
        public string SqlObtenerEmpresaCompensadaCompensacion
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaCompensadaCompensacion"); }
        }
        public string SqlObtenerReclamoReconsideracionReconsideracion
        {
            get { return base.GetSqlXml("SqlObtenerReclamoReconsideracionReconsideracion"); }
        }
        public string SqlObtenerReclamoApelacionReconsideracion
        {
            get { return base.GetSqlXml("SqlObtenerReclamoApelacionReconsideracion"); }
        }
        public string SqlObtenerReclamoArbitrajeReconsideracion
        {
            get { return base.GetSqlXml("SqlObtenerReclamoArbitrajeReconsideracion"); }
        }
        public string SqlInsertarEmpresaInvolucrada
        {
            get { return base.GetSqlXml("SqlInsertarEmpresaInvolucrada"); }
        }

        public string SqlInsertarSecuenciaEvento
        {
            get { return base.GetSqlXml("SqlInsertarSecuenciaEvento"); }
        }

        public string SqlInsertarReunionResponsable
        {
            get { return base.GetSqlXml("SqlInsertarReunionResponsable"); }
        }
        
        public string SqlObservacionConsultar
        {
            get { return base.GetSqlXml("SqlObservacionConsultar"); }
        }
        public string SqlRecomendacionesConsultar
        {
            get { return base.GetSqlXml("SqlRecomendacionesConsultar"); }
        }
        public string SqlEliminarEmpresaInvolucrada
        {
            get { return base.GetSqlXml("SqlEliminarEmpresaInvolucrada"); }
        }
        public string SqlEliminarSecuenciaEvento
        {
            get { return base.GetSqlXml("SqlEliminarSecuenciaEvento"); }
        }
        public string SqlEliminarEmpresaInvolucradaReunion
        {
            get { return base.GetSqlXml("SqlEliminarEmpresaInvolucradaReunion"); }
        }
        public string SqlEliminarReunionResponsable
        {
            get { return base.GetSqlXml("SqlEliminarReunionResponsable"); }
        }

        public string SqlEliminarAsistenteResponsable
        {
            get { return base.GetSqlXml("SqlEliminarAsistenteResponsable"); }
        }

        public string SqlActualizarFechaConvocatoriaCitacionReunion
        {
            get { return base.GetSqlXml("SqlActualizarFechaConvocatoriaCitacionReunion"); }
        }

        public string SqlActualizarFechaActaReunion
        {
            get { return base.GetSqlXml("SqlActualizarFechaActaReunion"); }
        }

        public string SqlActualizarFechaInformeCTAFReunion
        {
            get { return base.GetSqlXml("SqlActualizarFechaInformeCTAFReunion"); }
        }

        public string SqlExisteRecomendacionInformeTecnico
        {
            get { return base.GetSqlXml("SqlExisteRecomendacionInformeTecnico"); }
        }
        public string SqlExisteEmpresaResponsableCompensacion
        {
            get { return base.GetSqlXml("SqlExisteEmpresaResponsableCompensacion"); }
        }
        public string SqlExisteEmpresaCompensadaCompensacion
        {
            get { return base.GetSqlXml("SqlExisteEmpresaCompensadaCompensacion"); }
        }
        public string SqlExisteReunionResponsable
        {
            get { return base.GetSqlXml("SqlExisteReunionResponsable"); }
        }
        public string SqlExisteAsistenteResponsable
        {
            get { return base.GetSqlXml("SqlExisteAsistenteResponsable"); }
        }


        public string SqlObtenerMaxIdRecomendacionInformeTecnico
        {
            get { return base.GetSqlXml("SqlObtenerMaxIdRecomendacionInformeTecnico"); }
        }

        public string SqlObtenerMaxIdReunionResponsable
        {
            get { return base.GetSqlXml("SqlObtenerMaxIdReunionResponsable"); }
        }

        public string SqlObtenerMaxIdSecuenciaEvento
        {
            get { return base.GetSqlXml("SqlObtenerMaxIdSecuenciaEvento"); }
        }

        public string SqlInsertarRecomendacionInformeTecnico
        {
            get { return base.GetSqlXml("SqlInsertarRecomendacionInformeTecnico"); }
        }

        public string SqlEliminarRecomendacionInformeTecnico
        {
            get { return base.GetSqlXml("SqlEliminarRecomendacionInformeTecnico"); }
        }
        public string SqlObtenerMaxIdObservacionInformeTecnico
        {
            get { return base.GetSqlXml("SqlObtenerMaxIdObservacionInformeTecnico"); }
        }

        public string SqlInsertarObservacionInformeTecnico
        {
            get { return base.GetSqlXml("SqlInsertarObservacionInformeTecnico"); }
        }

        public string SqlEliminarObservacionInformeTecnico
        {
            get { return base.GetSqlXml("SqlEliminarObservacionInformeTecnico"); }
        }

        public string SqlActualizarPublicacionInformeTecnicoAnualInformeTecnico
        {
            get { return base.GetSqlXml("SqlActualizarPublicacionInformeTecnicoAnualInformeTecnico"); }
        }

        public string SqlActualizarPublicacionDesicionEventoInformeTecnico
        {
            get { return base.GetSqlXml("SqlActualizarPublicacionDesicionEventoInformeTecnico"); }
        }

        public string SqlObtenerMaxIdReclamoInformeTecnico
        {
            get { return base.GetSqlXml("SqlObtenerMaxIdReclamoInformeTecnico"); }
        }
        public string SqlObtenerMaxIdReclamoRespuestaInformeTecnico
        {
            get { return base.GetSqlXml("SqlObtenerMaxIdReclamoRespuestaInformeTecnico"); }
        }
        public string SqlInsertarFuerzaMayorInformeTecnico
        {
            get { return base.GetSqlXml("SqlInsertarFuerzaMayorInformeTecnico"); }
        }
        public string SqlActualizarFuerzaMayorInformeTecnico
        {
            get { return base.GetSqlXml("SqlActualizarFuerzaMayorInformeTecnico"); }
        }
        public string SqlEliminarFuerzaMayorInformeTecnico
        {
            get { return base.GetSqlXml("SqlEliminarFuerzaMayorInformeTecnico"); }
        }
        public string SqlInsertarEmpresaResponsableCompensacion
        {
            get { return base.GetSqlXml("SqlInsertarEmpresaResponsableCompensacion"); }
        }

        public string SqlEliminarEmpresaResponsableCompensacion
        {
            get { return base.GetSqlXml("SqlEliminarEmpresaResponsableCompensacion"); }
        }
        public string SqlActualizarInformeCompensaciones
        {
            get { return base.GetSqlXml("SqlActualizarInformeCompensaciones"); }
        }

        public string SqlInsertarReclamoRecApe
        {
            get { return base.GetSqlXml("SqlInsertarReclamoRecApe"); }
        }
        public string SqlEliminarReclamoRecApe
        {
            get { return base.GetSqlXml("SqlEliminarReclamoRecApe"); }
        }
        public string SqlActualizarReclamoRecApe
        {
            get { return base.GetSqlXml("SqlActualizarReclamoRecApe"); }
        }
        public string SqlActualizarEvento
        {
            get { return base.GetSqlXml("SqlActualizarEvento"); }
        }
        public string SqlObtenerMedidasAdoptadas
        {
            get { return base.GetSqlXml("SqlObtenerMedidasAdoptadas"); }
        }
        public string SqlActualizarRecomendacionMA
        {
            get { return base.GetSqlXml("SqlActualizarRecomendacionMA"); }
        }
        public string SqlActualizarRecomendacionMAG
        {
            get { return base.GetSqlXml("SqlActualizarRecomendacionMAG"); }
        }
        public string SqlActualizarCartaRecomendacionCOES
        {
            get { return base.GetSqlXml("SqlActualizarCartaRecomendacionCOES"); }
        }
        public string SqlActualizarCartaRespuesta
        {
            get { return base.GetSqlXml("SqlActualizarCartaRespuesta"); }
        }


        public string SqlObtenerCTAFINFORMEREPORTE
        {
            get { return base.GetSqlXml("SqlObtenerCTAFINFORMEREPORTE"); }
        }

        public string SqlObtenerSecuenciaEventoREPORTE
        {
            get { return base.GetSqlXml("SqlObtenerSecuenciaEventoREPORTE"); }
        }

        public string SqlObtenerSecuenciaEventoREPORTEv2
        {
            get { return base.GetSqlXml("SqlObtenerSecuenciaEventoREPORTEv2"); }
        }

        public string SqlObtenerSecuenciaEventoREPORTEv3
        {
            get { return base.GetSqlXml("SqlObtenerSecuenciaEventoREPORTEv3"); }
        }

        public string SqlObtenerSenalizacionREPORTE
        {
            get { return base.GetSqlXml("SqlObtenerSenalizacionREPORTE"); }
        }

        public string SqlObtenerSuministroREPORTE
        {
            get { return base.GetSqlXml("SqlObtenerSuministroREPORTE"); }
        }
        public string SqlObtenerEventoCitacion
        {
            get { return base.GetSqlXml("SqlObtenerEventoCitacion"); }
        }

        public string SqlExisteIEI
        {
            get { return base.GetSqlXml("SqlExisteIEI"); }
        }

        public string SqlObtenerListaDirectores
        {
            get { return base.GetSqlXml("SqlObtenerListaDirectores"); }
        }

        public string SqlObtenerListaResponsables
        {
            get { return base.GetSqlXml("SqlObtenerListaResponsables"); }
        }

        public string SqlGuardarNuevoResponsable
        {
            get { return base.GetSqlXml("SqlGuardarNuevoResponsable"); }
        }

        public string SqlGuardarEditarResponsable
        {
            get { return base.GetSqlXml("SqlGuardarEditarResponsable"); }
        }

        public string SqlObtenerMaxIdResponsable
        {
            get { return base.GetSqlXml("SqlObtenerMaxIdResponsable"); }
        }

        public string SqlEliminarResponsable
        {
            get { return base.GetSqlXml("SqlEliminarResponsable"); }
        }

        public string SqlObtenerResponsable
        {
            get { return base.GetSqlXml("SqlObtenerResponsable"); }
        }
        #endregion

        #region FIT - SGOCOES func A - Soporte
        public string ValidaCorrelativo
        {
            get { return base.GetSqlXml("SqlValidaCorrelativo"); }
        }

        public string ValidaRespuestaCOES
        {
            get { return base.GetSqlXml("SqlValidaRespuestaCOES"); }
        }

        public string ValidaRespuestaCOES1
        {
            get { return base.GetSqlXml("SqlValidaRespuestaCOES1"); }
        }


        public string InsertarRecomendacionInformeTecnicoMaximoCorrelativo
        {
            get { return base.GetSqlXml("SqlInsertarRecomendacionInformeTecnicoMaximoCorrelativo"); }
        }

        public string EliminarEmpresaCompensadaCompensacion
        {
            get { return base.GetSqlXml("SqlEliminarEmpresaCompensadaCompensacion"); }
        }

        #endregion

        #region SIOSEIN2
        public string SqlObtenerEventosFallas
        {
            get { return base.GetSqlXml("ObtenerEventosFallas"); }
        }
        #endregion

        #region Aplicativo Extranet CTAF

        public string SqlConsultarInterrupcionSuministros
        {
            get { return base.GetSqlXml("ListarInterrupcionSuministros"); }
        }

        public string SqlEditarAfEvento
        {
            get { return base.GetSqlXml("EditarAfEvento"); }
        }

        public string SqlObtenerInterrupcionSuministro
        {
            get { return base.GetSqlXml("ObtenerInterrupcionSuministro"); }
        }

        public string SqlObtenerInterrupcionCTAF
        {
            get { return base.GetSqlXml("ObtenerInterrupcionCTAF"); }
        }

        public string SqlObtenerInterrupcionSuministroByEvencodi
        {
            get { return base.GetSqlXml("ObtenerInterrupcionSuministroByEvencodi"); }
        }

        public string SqlListarInterrupcionPorEventoSCO
        {
            get { return base.GetSqlXml("ListarInterrupcionPorEventoSCO"); }
        }
        public string SqlObtenerMaxIdSenializacionesProteccion
        {
            get { return base.GetSqlXml("SqlObtenerMaxIdSenializacionesProteccion"); }
        }

        public string SqlListarSenializacionesProteccion
        {
            get { return base.GetSqlXml("SqlListarSenializacionesProteccion"); }
        }
        public string SqlListarSenializacionesProteccionAgrupado
        {
            get { return base.GetSqlXml("SqlListarSenializacionesProteccionAgrupado"); }
        }
        public string SqlGrabarSenializacionesProteccion
        {
            get { return base.GetSqlXml("SqlGrabarSenializacionesProteccion"); }
        }
        public string SqlEliminarSenializacionProteccion
        {
            get { return base.GetSqlXml("SqlEliminarSenializacionProteccion"); }
        }
        #endregion

        #region Mejoras CTAF
        public string SqlLstEventosSco
        {
            get { return base.GetSqlXml("SqlLstEventosSco"); }
        }

        public string SqlEditarEventoAf
        {
            get { return base.GetSqlXml("EditarEventoAf"); }
        }
        public string SqlListarEventosSCO
        {
            get { return base.GetSqlXml("ListarEventosSCO"); }
        }
        public string SqlActualizarCodEvento
        {
            get { return base.GetSqlXml("SqlActualizarCodEvento"); }
        }
        public string SqlActualizarEventoxAfecodi
        {
            get { return base.GetSqlXml("SqlActualizarEventoxAfecodi"); }
        }
        public string SqlActualizarEventoAF
        {
            get { return base.GetSqlXml("SqlActualizarEventoAF"); }
        }
        public string SqlObtieneCantInformesCtaf
        {
            get { return base.GetSqlXml("SqlObtieneCantInformesCtaf"); }
        }
        public string SqlListadoEventoDTOAsoCtaf
        {
            get { return base.GetSqlXml("SqlListadoEventoDTOAsoCtaf"); }
        }
        public string SqlInterrupcionAsoCtaf
        {
            get { return base.GetSqlXml("SqlInterrupcionAsoCtaf"); }
        }
        public string SqlObtenerEmpresaInvolucradaxEvencodi
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaInvolucradaxEvencodi"); }
        }
        public string SqlObtenerAnalisisFallaxEvento
        {
            get { return base.GetSqlXml("SqlObtenerAnalisisFallaxEvento"); }
        }
        public string SqlActualizarRecomendacionAO
        {
            get { return base.GetSqlXml("SqlActualizarRecomendacionAO"); }
        }
        public string SqlObtenerEmpresaRecomendacion
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaRecomendacion"); }
        }
        public string SqlActualizarEventoAO
        {
            get { return base.GetSqlXml("SqlActualizarEventoAO"); }
        }
        #endregion

        public string SqlObtenerDetalleEventosInformeSemanal
        { 
            get { return base.GetSqlXml("ObtenerDetalleEventosInformeSemanal"); }
        }
        public string SqlUpdateDesEventoAF
        {
            get { return base.GetSqlXml("SqlUpdateDesEventoAF"); }
        }

        public string SqlObtenerDirector
        {
            get { return base.GetSqlXml("SqlObtenerDirector"); }
        }
        public string SqlConsultarAnalisisFallasxAnio
        {
            get { return base.GetSqlXml("ConsultarAnalisisFallasxAnio"); }
        }
        public string SqlListarAnalisisFallaxEvento
        {
            get { return base.GetSqlXml("SqlListarAnalisisFallaxEvento"); }
        }
    }
}
