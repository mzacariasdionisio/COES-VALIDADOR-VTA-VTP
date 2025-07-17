using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    /// <summary>
    /// Para para el manejo de datos de generación forzada
    /// </summary>    
    public class ForzadaModel
    {
        public string FechaHidroInicio { get; set; }
        public string FechaHidroFin { get; set; }
        public List<EqRelacionDTO> ListaRelacion { get; set; }
        public List<PrGenforzadaDTO> ListaGeneracionForzada { get; set; }
        public List<EveSubcausaeventoDTO> ListaSubCausa { get; set; }
        public PrGenforzadaDTO Entidad { get; set; }
        public int CodigoRelacion { get; set; }
        public string Simbolo { get; set; }
        public int Codigo { get; set; }
        public int? Subcausa { get; set; }
        public string[][] Datos { get; set; }
        public List<EqRelacionDTO> ListEquipo { get; set; }
        
    }

    /// <summary>
    /// Permite tratar los datos de las configuraciones base de gen forzada
    /// </summary>
    public class ForzadaMaestroModel
    {
        public List<PrGenforzadaMaestroDTO> ListaMaestro { get; set; }
        public PrGenforzadaMaestroDTO Entidad { get; set; }
        public List<EqRelacionDTO> ListaRelacion { get; set; }
        public List<EveSubcausaeventoDTO> ListaSubCausa { get; set; }
        public int Codigo { get; set; }
        public int CodigoRelacion { get; set; }
        public string Estado { get; set; }
        public string Simbolo { get; set; }
        public string Tipo { get; set; }
        public int? Subcausa { get; set; } 
    }
}