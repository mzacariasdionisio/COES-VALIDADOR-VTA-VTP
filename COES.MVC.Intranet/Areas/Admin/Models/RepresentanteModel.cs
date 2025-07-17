using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class RepresentanteModel
    {
        public List<UserDTO> ListaUsuarios { get; set; }
        public UserDTO Entidad { get; set; }
        public List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO> ListaEmpresas { get; set; }
        public List<ModuloDTO> ListaModulo { get; set; }
        public List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO> ListaEmpresaSeleccionado { get; set; }
        public List<SiEmpresaDTO> ListaEmpresaFiltro { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresaFiltro { get; set; }
        public int UserCode { get; set; }
        public string Nombre { get; set; }
        public int EmpresaId { get; set; }        
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string AreaLaboral { get; set; }
        public string Cargo { get; set; }
        public string MotivoContacto { get; set; }
        public string Estado { get; set; }
        public string Modulos { get; set; }
        public string EmpresaNombre { get; set; }
        public string Celular { get; set; }
        public int Permiso { get; set; }
    }
}