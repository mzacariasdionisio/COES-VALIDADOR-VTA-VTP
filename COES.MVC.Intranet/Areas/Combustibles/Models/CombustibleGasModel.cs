using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Combustibles;
using System;
using System.Collections.Generic;


namespace COES.MVC.Intranet.Areas.Combustibles.Models
{
    public class CombustibleGasModel
    {
        public string FechaActual { get; set; }

        public bool TienePermisoAdmin { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string HtmlListado { get; set; }
        public string HtmlCarpeta { get; set; }
        public string HtmlHistorial { get; set; }
        public string HtmlCentrales { get; set; }
        public string HtmlLogEnvios { get; set; }
        public string ModuloArchivo { get; set; }
        public string ListaFilasPintar { get; set; }
        public int IdReporte { get; set; }

        public List<CbEnvioDTO> ListadoEnvios { get; set; }
        public int IdEstado { get; set; }
        public int TipoCorreo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaPlazo { get; set; }
        public int HoraPlazo { get; set; }
        public string MesAnio { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<CbCentralxfenergDTO> ListadoCentrales { get; set; }
        public List<ExtEstadoEnvioDTO> ListadoEstados { get; set; }
        public CbCentralxfenergDTO CentralTermica { get; set; }
        public List<CbFichaDTO> ListadoPlantilla { get; set; }
        public List<CbConceptocombDTO> ListadoPropiedad { get; set; }
        public CbFichaDTO Plantilla { get; set; }
        public List<CbLogenvioDTO> ListadoEnvioLog { get; set; }
        public List<SiPlantillacorreoDTO> ListadoPlantillasCorreo { get; set; }
        public SiPlantillacorreoDTO PlantillaCorreo { get; set; }
        public List<VariableCorreo> ListaVariables { get; set; }
        public CbEnvioDTO Envio { get; set; }
        public string OtrosUsuariosEmpresa { get; set; }
        public int PermiteAmpliacion { get; set; }
        public int EsEnvioDeAsignacion { get; set; } // 0: no, 1: sí

        public List<GenericoDTO> ListaMes { get; set; }
        public List<GenericoDTO> ListaTipoCentral { get; set; }

        public int FlagCentralExistente { get; set; }
        public int FlagCentralNuevo { get; set; }

        public int IdEnvio { get; set; }
        public int IdEmpresa { get; set; }
        public string Emprnomb { get; set; }
        public string TipoCentral { get; set; }
        public string TipoCentralDesc { get; set; }
        public string TipoAccion { get; set; }
        public string TipoOpcion { get; set; }
        public string MesVigencia { get; set; }
        public string UsuarioGenerador { get; set; }
        public int EsPrimeraCarga { get; set; }
        public int EsEditable { get; set; }
        public bool EsIntranet { get; set; }
        public bool FlagExisteEnvio { get; set; }

        public List<PR31FormGasCentral> ListaFormularioCentral { get; set; } = new List<PR31FormGasCentral>();
        public PR31FormGasSustento FormularioSustento { get; set; } = new PR31FormGasSustento();
        public List<CbVersionDTO> ListadoVersiones { get; set; } // listaVersiones
        public List<CbLogenvioDTO> ListaLog { get; set; }
        public List<CeldaErrorCombustible> ListaError { get; set; }
        public decimal ValorEnergia { get; set; }

        public int NumCentralesEnF3 { get; set; }
        public int NumRegistros { get; set; }
        public string DiasParaSubsanar { get; set; }
        public decimal? CostoCombustibleGaseoso { get; set; }

        public bool UsarFechaSistemaManual { get; set; }

        public List<SiCorreoDTO> ListadoCorreosEnviados { get; set; }
        public SiCorreoDTO Correo { get; set; }
        public string[][] Data { get; set; }
        public string[][] Data2 { get; set; }
        public string[][] Data3 { get; set; }
        public int ReporteCodiC1 { get; set; }
        public int ReporteCodiC2 { get; set; }
        public int ReporteCodiC3 { get; set; }
        public string NombreReporteC1 { get; set; }
        public string NombreReporteC2 { get; set; }
        public string NombreReporteC3 { get; set; }
        public string NombreReporteC4 { get; set; }
        public string NotaRC1 { get; set; }
        public string NotaRC2 { get; set; }
        public string NotaRC3 { get; set; }
        public int ExisteVersion1 { get; set; }

        public GraficoWeb Grafico1 { get; set; }
        public GraficoWeb Grafico2 { get; set; }
    }

    public class ArchivoGasModel
    {
        public string Fecha { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
        public List<FileData> ListaDocumentosFiltrado { get; set; }

        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
    }

    public class FormularioGasModel
    {
        public int IdEnvio { get; set; }
        public int IdEmpresa { get; set; }
        public string MesVigencia { get; set; }

        public List<PR31FormGasCentral> ListaFormularioCentral { get; set; }
        public PR31FormGasSustento FormularioSustento { get; set; }

        public int? Equicodi { get; set; }
        public int? CnpSeccion1 { get; set; }
        public int? NumCol { get; set; }
        public string NumColDesp { get; set; }
        public int? CnpSeccion0 { get; set; }
        public string TipoOpcionSeccion { get; set; }
        public string MesAnteriorCentralNueva { get; set; }
        public string TipoAccionForm { get; set; }
        public int EsPrimeraCarga { get; set; }
        public string TipoCentral { get; set; }
        public string UsuarioGenerador { get; set; }

        public string FechaMaxRpta { get; set; }
        public string CorreosCc { get; set; }
        public string LstCentralesAprob { get; set; }
        public string LstCentralesDesaprob { get; set; }
        public string Plantcontenido { get; set; }
        public int TipoGuardado { get; set; }
        public string DescVersion { get; set; }
        public int CondicionEnvioPrevioTemporal { get; set; } //conexion con servidor del anterior envio temporaral: 0: con error, 1: correcto

        public SiPlantillacorreoDTO Correo { get; set; }
    }
}