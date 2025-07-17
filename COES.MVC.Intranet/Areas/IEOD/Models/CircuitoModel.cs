using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.IEOD.Models
{
    public class CircuitoModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }

        //Propiedades para búsqueda de equipos
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public int FiltroFamilia { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<AreaDTO> ListaArea { get; set; }
        public int TipoFormulario { get; set; }
        public List<EqCircuitoDTO> ListaCircuito { get; set; }
        public EqCircuitoDTO Circuito { get; set; }
        public EqCircuitoDetDTO CircuitoDet { get; set; }
        public bool OpcionEditar { get; set; }

        // -------------------------------------------------------------------------------------
        // Propiedades formulario
        // -------------------------------------------------------------------------------------
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        // -------------------------------------------------------------------------------------
        // Propiedades de Paginado
        // -------------------------------------------------------------------------------------
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }

        // -------------------------------------------------------------------------------------
        // Propiedades formulario
        // -------------------------------------------------------------------------------------
        public int Circodi { get; set; }
        public string Circnomb { get; set; }
        public int Equicodi { get; set; }
        public List<EqCircuitoDetDTO> ListaDetalleCircuito { get; set; }
    }
}