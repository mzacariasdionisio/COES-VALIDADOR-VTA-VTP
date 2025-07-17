using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.RegionesDeSeguridad.Models
{
    public class RegionSeguridadModel
    {
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public SegRegionDTO SegRegion { get; set; }
        public List<SegRegionDTO> ListaRegion { get; set; }
        public int Codigo { get; set; }
        //public List<SegCoordenadaDTO> ListaRestricciones { get; set; }
        public SegZonaDTO EntidadZona { get; set; }
        public int Tipo { get; set; }
        public string TipoNombre { get; set; }
        public List<SegRegionequipoDTO> ListaDetalle { get; set; }
        //public SegCoordenadaDTO EntidadCoordenada { get; set; }
    }

    public class RestriccionModel
    {
        public SegCoordenadaDTO Restriccion { get; set; }
        public List<SegCoordenadaDTO> ListaRestricciones { get; set; }
        public List<SegZonaDTO> ListaZona { get; set; }

        public int Segcocodi { get; set; }
        public int Segconro { get; set; }
        public decimal Segcoflujo1 { get; set; }
        public decimal Segcoflujo2 { get; set; }
        public decimal Segcogener1 { get; set; }
        public decimal Segcogener2 { get; set; }
        public int Zoncodi { get; set; }
        public int Regcodi { get; set; }
        public int Segcotipo { get; set; }

    }

    public class DataCargaMasiva
    {
        public List<SegRegionDTO> ListaRegion { get; set; }
        public List<SegCoordenadaDTO> ListaCoordenada { get; set; }
        public List<SegRegionequipoDTO> ListaEquipo { get; set; }
        public List<SegRegiongrupoDTO> ListaGrupo { get; set; }
        public List<CmRegionseguridadDTO> ListaCmRegion { get; set; }
        public List<CmRegionseguridadDetalleDTO> ListaCmRegionDetalle { get; set; }
    }

}