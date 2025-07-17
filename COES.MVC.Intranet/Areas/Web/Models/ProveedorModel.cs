using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Web.Models
{
    /// <summary>
    /// Model para proveedores
    /// </summary> 
    public class ProveedorModel
    {
        public List<WbProveedorDTO> ListaProveedor { get; set; }
        public List<string> ListaTipoProveedor { get; set; }
        public WbProveedorDTO Entidad { get; set; }
    }
}