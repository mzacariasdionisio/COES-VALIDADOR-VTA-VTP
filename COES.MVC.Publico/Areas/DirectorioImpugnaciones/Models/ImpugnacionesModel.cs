using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.DirectorioImpugnaciones.Models
{
    public class ImpugnacionesModel
    {
        public List<WbImpugnacionDTO> ListaImpugnaciones { get; set; }
        public WbTipoimpugnacionDTO TipoImpugnacion { get; set; }
        public WbImpugnacionDTO Impugnacion { get; set; }

        public int Codigo { get; set; }
        public int TipoImpugn { get; set; }
        public string Titulo { get; set; }
        public int? NumeroMes { get; set; }
        public string RegistroSGDOC { get; set; }
        public string Impugnante { get; set; }
        [AllowHtml]
        public string DecisionImpugnada { get; set; }
        [AllowHtml]
        public string Petitorio { get; set; }
        public string FecRecepcion { get; set; }
        public string FecPublicacion { get; set; }
        public string PlazoIncorporacion { get; set; }
        public string IncorporacionesPresentadas { get; set; }
        [AllowHtml]
        public string Decision { get; set; }
        public string FecDesicion { get; set; }
        public int? DiasTotalesAtencion { get; set; }
        public string FecMes { get; set; }
        public string FecAnio { get; set; }
        public string Extension { get; set; }
        public string ExtensionAntiguo { get; set; }
        public string NombreArchivo { get; set; }

        public string Nuevo { get; set; }
        public string CambiarArchivo { get; set; }
    }
}