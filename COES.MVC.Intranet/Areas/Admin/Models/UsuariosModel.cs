using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class UsuariosModel
    {
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<ModuloDTO> ListaModulos { get; set; }
        public List<UserDTO> ListaUsuarios { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public UserDTO Entidad { get; set; }
        public List<SolicitudDTO> ListaSolicitudes { get; set; }
        public List<EmpresaDTO> EmpresasUsuario { get; set; }
        public List<EmpresaDTO> ListaEmpresaSeleccionado { get; set; }
        public int IdUsuario { get; set; }
        public string IndicadorSolicitud { get; set; }
    }
}