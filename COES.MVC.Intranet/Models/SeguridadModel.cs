using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Models
{
    public class UserModel
    {
        public int? UserCode { get; set; }
        public string Indicador { get; set; }
        public int IdArea { get; set; }
        public string Nombre { get; set; }
        public string UserLogin { get; set; }
        public int? UserConn { get; set; }
        public int? MaxUserConn { get; set; }
        public string Password { get; set; }
        public string Confirmar { get; set; }
        public string Estado { get; set; }
        public string Roles { get; set; }
    }

    public class PermisoSeleccionado
    {
        public int IdRol { get; set; }
        public int IdOpcion { get; set; }
        public int IdPermiso { get; set; }
    }

    public class HeaderModel
    {
        public List<MenuModel> ListaFavoritos { get; set; }
    }
}