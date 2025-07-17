using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{
    public class FTAreaRevisionModel
    {
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }

        public int IdEstado { get; set; }
        public List<EmpresaCoes> ListaEmpresas { get; set; }
        public int NumeroEmpresas { get; set; }
        public List<FtExtEtapaDTO> ListaEtapas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string StrIdsAreaDelUsuario { get; set; }
        public string StrIdsAreaTotales { get; set; }
        
        public string NombreAreasDelUsuario { get; set; }
        public int IdAreaRevision { get; set; }
        public string NombreAreaRevision { get; set; }
        
        public string FechaPlazo { get; set; }
        public string FechaPlazoRevision { get; set; }
        public int HoraPlazo { get; set; }

        public List<FtExtEnvioDTO> ListadoEnvios { get; set; }
        public string HtmlCarpeta { get; set; }

        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int IdEnvio { get; set; }
        public int IdVersion { get; set; }
        public int Ftenvcodi { get; set; }
        public int Ftetcodi { get; set; }
        public string Ftetnombre { get; set; }
        public string TipoEnvioDesc { get; set; }
        
        public int Ftprycodi { get; set; }
        public string Ftprynombre { get; set; }
        public int EnvioTipoFormato { get; set; }
        public string CarpetaEstadoDesc { get; set; }
        public int IdCarpetaArea { get; set; }
        public string LstFteeqcodis { get; set; }
        public string LstEnviosEqNombres { get; set; }
        public List<FtExtEnvioEqDTO> ListaEnvioEq { get; set; }
        public FTValidacionEnvio ValidacionEnvio { get; set; }
        public string TipoOpcion { get; set; }
        public string EsFTModificada { get; set; }
        public int PorcentajeAvanceRevision { get; set; }
        public string HtmlPorcentajeAvance { get; set; }
        public List<ErrorRevisionAreas> ListadoErroresRevArea { get; set; }
        public FTReporteExcel ReporteDatoXEq { get; set; }
        public List<FTDatoRevisionParametrosAEnvio> ListaRevisionParametrosAFT { get; set; }
        public List<DatoRevisionAreasFT> ListaRevisionAreasFT { get; set; }
        public List<DatoRevisionAreasContenido> ListaRevisionAreasContenido { get; set; }
        
        public FtExtEventoDTO Evento { get; set; }
        public List<FtExtEventoReqDTO> ListaReqEvento { get; set; }

        public bool HabilitadoEditarInformacion { get; set; }
        public bool HabilitadoDescargaConfidencial { get; set; }

        public string MsgFecMaxRespuesta { get; set; }

    }
}