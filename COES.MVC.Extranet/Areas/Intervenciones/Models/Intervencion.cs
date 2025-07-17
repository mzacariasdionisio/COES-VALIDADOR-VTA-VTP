using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Intervenciones.Models
{
    public class Intervencion
    {
        // -------------------------------------------------------------------------------------
        // Comandos estandar para los permisos (Botonera)
        // -------------------------------------------------------------------------------------
        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
        public bool AccionGrabar { get; set; }
        // -------------------------------------------------------------------------------------

        // -------------------------------------------------------------------------------------
        // Propiedades de Alertas
        // -------------------------------------------------------------------------------------
        public string sMensaje { get; set; }
        // -------------------------------------------------------------------------------------

        // -------------------------------------------------------------------------------------
        // Propiedades de Paginado
        // -------------------------------------------------------------------------------------
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }
        // -------------------------------------------------------------------------------------


        // -------------------------------------------------------------------------------------
        // Propiedades basicas del modelo de Intervenciones.
        // -------------------------------------------------------------------------------------
        public List<InIntervencionDTO> ListaIntervenciones { get; set; }
        public List<InIntervencionDTO> ListaIntervencionesProgramadas { get; set; }
        public List<InIntervencionDTO> ListaIntervencionesEjecutadas { get; set; }
        public List<InIntervencionDTO> ListaIntervencionesEjecutadasFuturas { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacionES { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacionFS { get; set; }
        public List<InIntervencionDTO> ListaIntervencionesFS { get; set; }
        public List<InIntervencionDTO> ListaIntervencionesES { get; set; }
        public List<IndiceF1F2> ListaIndiceF1F2 { get; set; }
        public InIntervencionDTO Entidad { get; set; }

        public int IdIntervencion { get; set; }
        public string Intercodis { get; set; }
        public string CorreoFrom { get; set; }
        public string CorreoTo { get; set; }
        // -------------------------------------------------------------------------------------   


        // -------------------------------------------------------------------------------------
        // Lista para combos.
        // -------------------------------------------------------------------------------------
        public List<EveEvenclaseDTO> ListaTiposProgramacion { get; set; }
        public List<SiEmpresaDTO> ListaCboEmpresa { get; set; }
        public List<EqAreaDTO> ListaCboUbicacion { get; set; }
        public List<GenericoDTO> ListaCboEquipo { get; set; }
        public List<EveTipoeventoDTO> ListaCboIntervencion { get; set; }
        public List<InEstadoDTO> ListacboEstado { get; set; }
        public List<SiMensajeDTO> ListaMensajes { get; set; }
        public List<SiLogDTO> ListaHistorial { get; set; }
        public List<string> ListadoActividad { get; set; }
        public List<InClaseProgDTO> ListaClaseProgramacion { get; set; }
        public List<EveSubcausaeventoDTO> ListaCausas { get; set; }
        public List<EqFamiliaDTO> ListaFamilias { get; set; }
        // -------------------------------------------------------------------------------------

        // -------------------------------------------------------------------------------------
        // Consulta Cruzada.
        // -------------------------------------------------------------------------------------
        public List<HorasIndispo> ListadoHrasIndisponiblidad { get; set; }
        // -------------------------------------------------------------------------------------

        // -------------------------------------------------------------------------------------
        // Propiedades relacionadas al modelo de Programaciones.
        // -------------------------------------------------------------------------------------
        public int IdProgramacion { get; set; }
        public int IdTipoProgramacion { get; set; }
        public string NombreProgramacion { get; set; }
        public int EstadoProgramacion { get; set; }
        public string EstadoProgramacionDesc { get; set; }
        public string Evenclasedesc { get; set; }
        public List<InProgramacionDTO> ListaProgramaciones { get; set; }
        public InProgramacionDTO EntidadProgramacion { get; set; }

        public string FechProgramacion { get; set; }
        public int TieneValidacionEjecIgualProg { get; set; }
        public int TieneValidacionHo { get; set; }
        public int TieneValidaciones { get; set; }//tiene alguna validación 
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string NombPrograDetallado { get; set; }
        public string NombreArchivo { get; set; }
        public string NombreArchivoTmp { get; set; }
        // -------------------------------------------------------------------------------------

        // -------------------------------------------------------------------------------------
        // Variables para validar fechas
        // -------------------------------------------------------------------------------------
        public string Interfechafin { get; set; }
        public string Interfechaini { get; set; }

        public string FechaProceso { get; set; }

        public string InterfechainiH { get; set; }
        public string InterfechainiM { get; set; }
        public string InterfechafinH { get; set; }
        public string InterfechafinM { get; set; }

        public DateTime? InterfechainiD { get; set; }
        public DateTime? InterfechafinD { get; set; }

        public string ProgrfechainiDesc { get; set; }
        public string ProgrfechafinDesc { get; set; }

        public string Progrfechaini { get; set; }
        public string ProgrfechainiH { get; set; }
        public string ProgrfechainiM { get; set; }
        public string Progrfechafin { get; set; }
        public string ProgrfechafinH { get; set; }
        public string ProgrfechafinM { get; set; }
        // -------------------------------------------------------------------------------------

        public string Detalle { get; set; }

        // -------------------------------------------------------------------------------------      

        // -------------------------------------------------------------------------------------
        // Almacenar los meses y años
        // -------------------------------------------------------------------------------------
        public IEnumerable<SelectListItem> ListaMes { get; set; }
        public IEnumerable<SelectListItem> ListaAnio { get; set; }
        // -------------------------------------------------------------------------------------

        public string sIdsEmpresas { get; set; }

        // Almacenar nombre archivo
        public string FileName { get; set; }

        // Almacenar lista Intervenciones correctas y con errores
        public List<InIntervencionDTO> ListaIntervencionesCorrectas { get; set; }
        public List<InIntervencionDTO> ListaIntervencionesErrores { get; set; }
        public List<InIntervencionDTO> ListaExclusion { get; set; }
        public List<InIntervencionDTO> ListaInclusion { get; set; }

        // Parar habilitar o desabilitar los inputs
        public bool EsDesabilitado { get; set; }
        public bool EsDasabilitadoCodSeguimiento { get; set; }

        // Parar habilitar o desabilitar elementos del menu
        public bool EsCerrado { get; set; }

        // Para validar el estado de solo lectura del horizonte o programación
        public bool EsHorizonteSoloLectura { get; set; }

        //Propiedades para búsqueda de equipos
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public int FiltroFamilia { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<AreaDTO> ListaArea { get; set; }

        //propiedades para trabajar fechas
        public string FechaHoy { get; set; }
        public string Anho { get; set; }
        public string AnhoIni { get; set; }
        public string Mes { get; set; }
        public string SemanaIni { get; set; }
        public string FechaInicio { get; set; }
        public List<FechaSemanas> ListaSemanasIni { get; set; }

        //Validaciones con Aplicativos
        public int FlagConfirmarValInterEjec { get; set; }
        public int FlagConfirmarValInterHo { get; set; }
        //public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacion { get; set; }

        //Validaciones con Aplicativos
        public int FlagConfirmarValInter { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacion { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionScada { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionEms { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionIDCC { get; set; }

        //para mensaje de alerta de creación o modificación por el cordinador
        public int ListaIntervCount { get; set; }

        //pr25
        public bool ChkPr25 { get; set; } // S o N
        public List<GenericoDTO> ListaTipoindispPr25 { get; set; }
        public bool Esindisponilidadpr25 { get; set; }

        //cruzadas
        public IntervencionGridExcel GridExcel { get; internal set; }
        public bool EsCruzadas { get; set; }
        public bool TieneCeldaSelec { get; set; }

        //
        public int CarpetaFiles { get; set; }
        public string FamcodiSustentoObligatorio { get; set; }
        public string FamcodiSustentoOpcional { get; set; }
    }

    public class FechaSemanas
    {
        public int IdTipoInfo { get; set; }
        public string NombreTipoInfo { get; set; }
        public string FechaIniSem { get; set; }
        public string FechaFinSem { get; set; }
    }

    public class IntervencionResultado
    {
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Reporte { get; set; }

        public string NombreArchivo { get; set; }
        public string NombreArchivoTmp { get; set; }

        public int Progrcodi { get; set; }
        public int IdTipoProgramacion { get; set; }
        public bool EsCerrado { get; set; }

        public List<IntervencionFila> ListaFilaWeb { get; set; }
        public int Total { get; set; }

        public int PintarAlerta { get; set; } //flag alertahtml
        public bool TieneAlertaNoEjecutado { get; set; }
        public bool TieneAlertaHoraOperacion { get; set; }
        public bool TieneAlertaScada { get; set; }
        public bool TieneAlertaEms { get; set; }
        public bool TieneAlertaIDCC { get; set; }
        public bool TieneAlertaPR21 { get; set; }
        public bool TieneAlertaMedidores { get; set; }

        //para mensaje de alerta de creación o modificación por el agente
        public int ListaIntervCount { get; set; }

        public bool TieneArchivos { get; set; }//flag si tiene almenos un archivo

        public string Asunto { get; set; }
        public string CC { get; set; }

        public List<InIntervencionDTO> ListaNotificaciones { get; set; }
        public List<InIntervencionDTO> ListaExclusion { get; set; }

        public InIntervencionDTO IntervencionImportada { get; set; }
        public InIntervencionDTO IntervencionIncl { get; set; }
        public InIntervencionDTO IntervencionExcl { get; set; }
        public List<string> ListaMensaje { get; set; }

    }

    public class HorasIndispo
    {
        public int id { get; set; }
        public string value { get; set; }
    }
}