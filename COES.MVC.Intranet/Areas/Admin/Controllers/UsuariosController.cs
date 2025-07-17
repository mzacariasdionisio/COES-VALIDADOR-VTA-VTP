using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class UsuariosController : Controller
    {
        /// <summary>
        /// Lista de Empresa del usuario
        /// </summary>
        public List<EmpresaDTO> ListaEmpresa
        {
            get
            {
                return (Session[ConstantesAdmin.SesionEmpresa] != null) ?
                    (List<EmpresaDTO>)Session[ConstantesAdmin.SesionEmpresa] : new List<EmpresaDTO>();
            }
            set { Session[ConstantesAdmin.SesionEmpresa] = value; }
        }

        /// <summary>
        /// Referencia al servicio web de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Muestra la pantalla inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            UsuariosModel model = new UsuariosModel();
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.EMPRCODI > 1).OrderBy(x => x.EMPRNOMB).ToList();
            model.ListaModulos = this.seguridad.ListarModulos().ToList();
            model.FechaInicio = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            

            return View(model);
        }

        /// <summary>
        /// Muestra el listado de usuarios
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idModulo"></param>
        /// <param name="estado"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(int? idEmpresa, int? idModulo, string estado, string fechaInicio, string fechaFin)
        {
            UsuariosModel model = new UsuariosModel();
            DateTime? fecInicio = null;
            DateTime? fecFin = null;

            if (!string.IsNullOrEmpty(fechaInicio))
            {
                fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(fechaFin))
            {
                fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            model.ListaUsuarios = this.seguridad.ListarUsuariosExtranet(idEmpresa, idModulo, estado, fecInicio, fecFin).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Muestra el detalle de la empresa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detalle(int id)
        {
            UsuariosModel model = new UsuariosModel();
            model.IdUsuario = id;
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.EMPRCODI > 1).OrderBy(x => x.EMPRNOMB).ToList();
            return View(model);
        }

        /// <summary>
        /// Muesta los datos del usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Datos(int idUsuario)
        {
            UsuariosModel model = new UsuariosModel();
            model.Entidad = this.seguridad.ObtenerUsuario(idUsuario);            
            model.Entidad.CompanyName = this.seguridad.ObtenerEmpresa((int)model.Entidad.EmprCodi).EMPRNOMB;
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.EMPRCODI > 1).OrderBy(x => x.EMPRNOMB).ToList();
            this.ListaEmpresa = this.seguridad.ObtenerEmpresasPorUsuario(model.Entidad.UserLogin).ToList();

            return PartialView(model);
        }
        
        /// <summary>
        /// Permite actualizar las solicitudes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Solicitudes(int id)
        {
            UsuariosModel model = new UsuariosModel();
            model.ListaSolicitudes = this.seguridad.ObtenerModulosPorUsuario(id, User.Identity.Name).ToList();
            model.IndicadorSolicitud = Constantes.NO;
            if (model.ListaSolicitudes.Where(x => x.Permiso && x.SolicEstado == ConstantesAdmin.EstadoPendiente).Count() > 0)
            {
                model.IndicadorSolicitud = Constantes.SI;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Muestra las empresas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Empresa()
        {
            UsuariosModel model = new UsuariosModel();
            model.ListaEmpresaSeleccionado = this.ListaEmpresa;
            return PartialView(model);
        }

        /// <summary>
        /// Agrega una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEmpresa(int idEmpresa)
        {
            List<EmpresaDTO> lista = this.ListaEmpresa;
            if (lista.Where(x => x.EMPRCODI == idEmpresa).Count() == 0)
            {
                EmpresaDTO dto = this.seguridad.ObtenerEmpresa(idEmpresa);
                lista.Add(dto);
            }
            this.ListaEmpresa = lista;
            UsuariosModel model = new UsuariosModel();
            model.ListaEmpresaSeleccionado = this.ListaEmpresa;
            return PartialView(VistasParciales.Empresa, model);
        }

        /// <summary>
        /// Remueve una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveEmpresa(int idEmpresa)
        {
            List<EmpresaDTO> lista = this.ListaEmpresa;
            EmpresaDTO item = lista.Where(x => x.EMPRCODI == idEmpresa).FirstOrDefault();
            if (item != null)
            {
                lista.Remove(item);
            }

            this.ListaEmpresa = lista;

            UsuariosModel model = new UsuariosModel();
            model.ListaEmpresaSeleccionado = this.ListaEmpresa;
            return PartialView(VistasParciales.Empresa, model);
        }

        /// <summary>
        /// Permite dar acceso al usuario al modulo solictado
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idModulo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DarAcceso(int idUsuario, int idModulo)
        {
            try
            {
                this.seguridad.AprobarSolicitudModulo(idUsuario, idModulo, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite dar de baja al usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DarBajaUsuario(int idUsuario)
        {
            try
            {
                int resultado = this.seguridad.DarDeBajaUsuario(idUsuario, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite activar el usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActivarUsuario(int idUsuario)
        {
            try
            {
                int resultado = this.seguridad.AprobarUsuario(idUsuario, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite activar al usuario y aprobar las solicitudes de los módulos
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="modulos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarUsuario(int idUsuario, string modulos)
        {
            try
            {
                this.seguridad.AprobarUsuarioSolicitud(idUsuario, modulos, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar las empresas del usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarEmpresaUsuario(int idUsuario) 
        {
            try
            {
                this.seguridad.ActualizarEmpresasUsuario(idUsuario, this.ListaEmpresa.ToArray(), User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
