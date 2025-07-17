using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Monitoreo.Models
{
    public class ParametroModel
    {
        public int Id { get; set; }
        public string FechaInicio { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Motivo { get; set; }
        public MmmVersionDTO Generador { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }

        public List<ParametroTendenciaHHI> ListaParametroHHI { get; set; }
        public ParametroTendenciaHHI ParametroHHI { get; set; }
        public List<EstadoParametro> ListaEstado { get; set; }
    }

    public class BandaToleranciaModel
    {
        public List<MmmBandtolDTO> ListaBanda { get; set; }
        public MmmBandtolDTO Banda { get; set; }
        public List<EstadoParametro> ListaEstado { get; set; }
        public List<MmmIndicadorDTO> ListaIndicador { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
    }

    public class MmmJustificacionModel
    {
        public int Mjustcodi { get; set; }
        public int Immecodi { get; set; }
        public int? Barrcodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Emprcodi { get; set; }
        public string Mjustdescripcion { get; set; }
        public string MjustfechaDesc { get; set; }
    }
}