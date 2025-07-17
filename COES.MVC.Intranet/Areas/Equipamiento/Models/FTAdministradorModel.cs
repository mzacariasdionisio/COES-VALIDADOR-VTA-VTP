using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using System;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{
    public class FTAdministradorModel
    {
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }
        
        public int IdEstado { get; set; }
        public List<EmpresaCoes> ListaEmpresas { get; set; }
        public int NumeroEmpresas { get; set; }
        public List<FtExtEtapaDTO> ListaEtapas { get; set; }
        public List<ExtEstadoEnvioDTO> ListaEstados { get; set; }
        public List<FtExtProyectoDTO> ListaProyectos { get; set; }
        public List<FtExtCorreoareaDTO> ListaAreas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string StrIdsAreaDelUsuario { get; set; }
        public string StrIdsAreaTotales { get; set; }
        public string FechaPlazo { get; set; }
        public string FechaPlazoRevision { get; set; }
        public int HoraPlazo { get; set; }

        public List<FtExtEnvioDTO> ListadoEnvios { get; set; }
        public string HtmlCarpeta { get; set; }

        public int IdEnvio { get; set; }
        public int IdVersion { get; set; }

        public string CarpetaEstadoDesc { get; set; }
        public string MsgFecMaxRespuesta { get; set; }
        public string EsFTModificada { get; set; }
        public string MsgCancelacion { get; set; }        

        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Ftenvcodi { get; set; }
        public int Ftetcodi { get; set; }
        public string Ftetnombre { get; set; }
        public int Ftprycodi { get; set; }
        public string Ftprynombre { get; set; }
        public int EnvioTipoFormato { get; set; }
        public string TipoOpcion { get; set; }
        //public int IdEstado { get; set; }
        public List<FtExtEnvioEqDTO> ListaEnvioEq { get; set; }
        public string LstFteeqcodis { get; set; }
        public string LstEnviosEqNombres { get; set; }
        public FTReporteExcel ReporteDatoXEq { get; set; }
        public List<FTDatoRevisionParametrosAEnvio> ListaRevisionParametrosAFT{ get; set; }
        public List<DatoRevisionAreasContenido> ListaRevisionAreasContenido { get; set; }
        public List<DatoRevisionAreasFT> ListaRevisionAreasFT { get; set; }
        public List<FTReporteExcel> ListaRevisionImportacion { get; set; }

        public FTValidacionEnvio ValidacionEnvio { get; set; }

        public string CodigoEquipos { get; set; }
        public string NombreEquipos { get; set; }

        public FtExtEventoDTO Evento { get; set; }
        public List<FtExtEventoReqDTO> ListaReqEvento { get; set; }
        public int CodigoModo { get; set; }
        public PrGrupoDTO ModoOperacion { get; set; }
        public List<FtExtEnvioArchivoDTO> ListaArchivo { get; set; }


        public string FechaSubsanacion { get; set; }
        public string PlazoFinSubsanar { get; set; }
        public string AgenteUltimoEvento { get; set; }
        public string OtrosAgentesEmpresaDifUsuarioUltEvento { get; set; }
        public string strFechaSistema { get; set; }
        public string HtmlListadoCVA { get; set; }
        public string HtmlListadoCVAP { get; set; }

        public bool UsarFechaSistemaManual { get; set; }
        public string FechaSistemaFull { get; set; }
        public bool TieneParametrosVacios { get; set; }
        public string EnlaceSistemaIntranet { get; set; }
        public bool FlagFaltaHabilitarPlazo { get; set; }

        public string HtmlParametrosModifAprobados { get; set; }
        public string HtmlParametrosModifDenegados { get; set; }
        public string FechaDerivacion { get; set; }
        public int FlagVersionDerivada { get; set; }
        public List<RegistroCumplimientoAdminFT> ListaReporteCumplimientoAdminFT { get; set; }
        public List<RegistroCumplimientoRevAreas> ListaReporteCumplimientoRevAreas { get; set; }
    }

    public class FTArchivoModel
    {
        public string Fecha { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
        public List<FileData> ListaDocumentosFiltrado { get; set; }

        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
    }
}