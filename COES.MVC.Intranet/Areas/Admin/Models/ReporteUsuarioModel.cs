using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class ReporteUsuarioModel
    {
        public string Resultado { get; set; }
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<ReporteUsuarioDTO> ReporteUsuario { get; set; }
        public List<ModuloDTO> ListaModulos { get; set; }
    }
}