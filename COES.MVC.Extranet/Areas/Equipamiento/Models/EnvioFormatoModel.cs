using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using System.Collections.Generic;

namespace COES.MVC.Extranet.Areas.Equipamiento.Models
{
    public class EnvioFormatoModel
    {
        public int IdEnvio { get; set; }
        public int IdVersion { get; set; }
        public string ClaveCookie { get; set; }
        public string FteeqcodisLimpiar { get; set; }

        public int Ftenvcodi { get; set; }
        public int Ftevercodi { get; set; }
        public string MensajeLog { get; set; }

        //autoguardado
        public int IdEnvioTemporal { get; set; }
        public int IdVersionTemporal { get; set; }
        public int FlagEquipoAutoguardado { get; set; }
        public int MinutosAutoguardado { get; set; }
        public List<FtExtEnvioVersionDTO> ListaAutoguardado { get; set; }

        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }

        public int Ftetcodi { get; set; }
        public string Ftetnombre { get; set; }

        public int Ftprycodi { get; set; }
        public string Ftprynombre { get; set; }

        public string CodigoEquipos { get; set; }
        public string NombreEquipos { get; set; }
        public List<FTRelacionEGP> ListaEquipoEnvio { get; set; }
        public FtExtEventoDTO Evento { get; set; }
        public List<FtExtEventoReqDTO> ListaReqEvento { get; set; }
        public List<FtExtEnvioArchivoDTO> ListaArchivo { get; set; }

        public int EnvioTipoFormato { get; set; }
        public string TipoOpcion { get; set; }
        public string EsFTModificada { get; set; }
        
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }

        public List<EmpresaCoes> ListaEmpresas { get; set; }
        public List<FtExtEtapaDTO> ListaEtapas { get; set; }
        public List<FtExtProyectoDTO> ListaProyectos { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<FtExtEnvioDTO> ListadoEnvios { get; set; }
        public string HtmlCarpeta { get; set; }
        public int IdEstado { get; set; }        
        public bool HabilitarAddEquipo { get; set; }
        public string MsgFecMaxRespuesta { get; set; }
        public string MsgCancelacion { get; set; }
        public int NumeroEmpresas { get; set; }
        public int HabilitarAutoguardado { get; set; }
        public FtExtProyectoDTO Proyecto { get; set; }

        public int Fteeqcodi { get; set; }
        public FTReporteExcel ReporteDatoXEq { get; set; }

        public List<FtExtEnvioVersionDTO> ListaVersion { get; set; }
        public List<FtExtEnvioEqDTO> ListaEnvioEq { get; set; }
        public List<FTCeldaError> ListaErrores { get; set; }
        public List<FTDatoRevisionParametrosAEnvio> ListaRevisionParametrosAFT { get; set; }
        public string LstFteeqcodis { get; set; }
        public string LstEnviosEqNombres { get; set; }
        
        public int AccionVentana { get; set; }        
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