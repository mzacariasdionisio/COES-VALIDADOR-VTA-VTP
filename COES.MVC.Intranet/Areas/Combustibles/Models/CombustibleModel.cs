using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Combustibles;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Combustibles.Models
{
    public class CombustibleModel
    {
        public string FechaInicio { get; set; }
        public string Fecha { get; set; }
        public string FechaFin { get; set; }
        public string FechaActual { get; set; }

        public string FechaPlazo { get; set; }
        public int HoraPlazo { get; set; }

        public int IdEnvio { get; set; }
        public int IdEstado { get; set; }
        public int IdTipoCombustible { get; set; }
        public int IdFenerg { get; set; }
        public int IdEmpresa { get; set; }
        public int IdEquipo { get; set; }
        public int IdGrupo { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Fenergnomb { get; set; }
        public int IdAgrup { get; set; }

        public HandsonCombustible ModeloWeb { get; set; }
        public string HtmlCarpeta { get; set; }
        public string HtmlListado { get; set; }
        public string HtmlLogEnvio { get; set; }

        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqEquipoDTO> ListaCentral { get; set; }
        public List<EqEquipoDTO> ListaCentral2 { get; set; }
        public List<SiFuenteenergiaDTO> ListaCombustible { get; set; }
        public List<ExtEstadoEnvioDTO> ListaEstado { get; set; }

        public string TipoCambio { get; set; }
        public string Observacion { get; set; }

        public CbEnvioDTO Envio { get; set; }
        public List<CbArchivoenvioDTO> LstArchEnvio { get; set; }

        public bool OpAccesoEmpresa { get; set; }
        public bool AccionEditar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }

        public bool EsEnvioAprobado { get; set; }
        public string FechaVigencia { get; set; }
        public string FechaConsulta { get; set; }
        public string FechaObs { get; set; }
        public string To { get; set; }
        public string CCcorreosAgente { get; set; }

        //Parametros
        public List<PrGrupodatDTO> ListaParametros { get; set; }
        public bool TienePermiso { get; set; }
        public bool AccesoEditar { get; internal set; }
        public int Conceptocodi { get; internal set; }
        public CbCentralxfenergDTO Centralxfenerg { get; internal set; }
        public List<CbConceptocombDTO> ListaConceptocomb { get; internal set; }
        public string HtmlListaEnvioRelCv { get; internal set; }
    }

}