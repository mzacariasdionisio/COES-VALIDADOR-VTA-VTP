using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Combustibles;
using System.Collections.Generic;


namespace COES.MVC.Extranet.Areas.Combustibles.Models
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
        
        public List<CbEnvioDTO> ListadoEnvios { get; set; }
        public int IdEstado { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public CbEnvioDTO Envio { get; set; }

        public List<GenericoDTO> ListaMes { get; set; }
        public List<GenericoDTO> ListaTipoCentral { get; set; }
        public List<CbLogenvioDTO> ListadoEnvioLog { get; set; }
        public List<CeldaErrorCombustible> ListaError { get; set; }
        public int FlagCentralExistente { get; set; }
        public int FlagCentralNuevo { get; set; }

        public int IdEnvio { get; set; }
        public int IdEnvioTemporal { get; set; }
        public int MinutosAutoguardado { get; set; }
        public int IdEmpresa { get; set; }
        public string Emprnomb { get; set; }
        public string TipoCentral { get; set; }
        public int TipoEnvio { get; set; }
        public string TipoCentralDesc { get; set; }
        public string TipoOpcion { get; set; }

        public int HayAutoguardados { get; set; }
        public int BuscaAutoguardados { get; set; }
        public List<CbVersionDTO> ListaAutoguardados { get; set; }
        public int HabilitarAutoguardado { get; set; }

        public bool EsIntranet { get; set; }
        public bool FlagExisteEnvio { get; set; }

        public List<PR31FormGasCentral> ListaFormularioCentral { get; set; } = new List<PR31FormGasCentral>();
        public PR31FormGasSustento FormularioSustento { get; set; } = new PR31FormGasSustento();
        public List<CbVersionDTO> ListadoVersiones { get; set; } // listaVersiones
        public decimal ValorEnergia { get; set; }
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

        public List<PR31FormGasCentral> ListaFormularioCentral { get; set; } = new List<PR31FormGasCentral>();
        public PR31FormGasSustento FormularioSustento { get; set; } = new PR31FormGasSustento();

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
        public string TipoOpcion { get; set; }
        public string UsuarioGenerador { get; set; }

        public string FechaMaxRpta { get; set; }
        public string CorreosCc { get; set; }
        public string LstCentralesAprob { get; set; }
        public string LstCentralesDesaprob { get; set; }
        public string Plantcontenido { get; set; }
        public int TipoGuardado { get; set; }
        public string DescVersion { get; set; }
        public int CondicionEnvioPrevioTemporal { get; set; } //conexion con servidor del anterior envio temporaral: 0: con error, 1: correcto
    }
}