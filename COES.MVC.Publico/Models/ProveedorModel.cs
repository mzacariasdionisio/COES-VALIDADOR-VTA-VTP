using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Models
{
    public class ProveedorModel
    {
        public int codigo { get; set; }
        public string NombreProveedor { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public DateTime FechaHasta { get; set; }
        public string TipoProveedor { get; set; }
        public List<WbProveedorDTO> ListaProveedor { get; set; }
        public List<string> ListaTipoProveedor { get; set; }       
    }
}