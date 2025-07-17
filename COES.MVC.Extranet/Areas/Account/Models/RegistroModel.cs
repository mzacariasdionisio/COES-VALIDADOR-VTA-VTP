using COES.MVC.Extranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Account.Models
{
    public class RegistroModel
    {
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<ModuloDTO> ListaModulos { get; set; }
        
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int EmpresaId { get; set; }
        public string EmpresaNombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string AreaLaboral { get; set; }
        public string Cargo { get; set; }
        public string Modulos { get; set; }
        public string MotivoContacto { get; set; }
    }
}