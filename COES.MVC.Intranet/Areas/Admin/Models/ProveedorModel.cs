using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class ProveedorModel
    {
        public List<SiEmpresaDTO> ListadoEmpresa { get; set; }
        public List<SiEmpresaCorreoDTO> ListaEmpresaCorreo { get; set; }
        public int IdEmpresa { get; set; }
    }
}