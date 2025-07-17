using COES.MVC.Extranet.Areas.Account.Helper;
using COES.MVC.Extranet.Areas.Account.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Account.Controllers
{
    public class RepresentanteController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

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
        /// Muestra la ventana de administración de usuarios
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="eid"></param>
        /// <returns></returns>
        public ActionResult Usuarios()
        {
            RepresentanteModel model = new RepresentanteModel();

            if (Session[DatosSesion.SesionUsuario] != null)
            {
                UserDTO entity = (UserDTO)Session[DatosSesion.SesionUsuario];
                List<RolDTO> list = this.seguridad.ObtenerRolPorUsuario(entity.UserCode).Where(x => x.Seleccion > 0).ToList();
                int rolCode = Convert.ToInt32(ConfigurationManager.AppSettings[ConstantesAdmin.RolRepresentanteLegal]);

                if (list.Where(x => x.RolCode == (short)rolCode).Count() > 0)
                {
                    EmpresaDTO dto = this.seguridad.ObtenerEmpresa((int)entity.EmprCodi);
                    model.EmpresaId = dto.EMPRCODI;
                    model.EmpresaNombre = dto.EMPRNOMB;
                    model.IndicadorAcceso = true;
                }
                else
                {
                    model.IndicadorAcceso = false;
                }
            }
            else 
            {
                model.IndicadorAcceso = false;
            }            

            return PartialView(model);
        }

        /// <summary>
        /// Lista los usuarios por cada empresa asociada al representante
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="eid"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Grilla(int idEmpresa)
        {
            RepresentanteModel model = new RepresentanteModel();
            model.ListaUsuarios = this.seguridad.ListarUsuariosPorEmpresa(idEmpresa).Where(x => x.UserState == ConstantesAdmin.EstadoActivo).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Muestra el detalle de empresas
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Edicion(int idUsuario, int idEmpresa)
        {
            RepresentanteModel model = new RepresentanteModel();
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.EMPRCODI > 1).OrderBy(x => x.EMPRNOMB).ToList();
            model.ListaModulo = this.seguridad.ObtenerModulosPorUsuarioSelecion(idUsuario).ToList();

            if (idUsuario == 0)
            {
                model.Entidad = new SeguridadServicio.UserDTO();
                this.ListaEmpresa = new List<EmpresaDTO>();
                model.Entidad.UserCode = 0;
                model.Entidad.EmprCodi = (short)idEmpresa;
                model.Entidad.UserState = ConstantesAdmin.EstadoActivo;
            }
            else
            {
                model.Entidad = this.seguridad.ObtenerUsuario(idUsuario);
                this.ListaEmpresa = this.seguridad.ObtenerEmpresasPorUsuario(model.Entidad.UserLogin).ToList();
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
            RepresentanteModel model = new RepresentanteModel();
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
            RepresentanteModel model = new RepresentanteModel();
            model.ListaEmpresaSeleccionado = this.ListaEmpresa;
            return PartialView(ConstantesAdmin.VistaParcialEmpresa, model);
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

            RepresentanteModel model = new RepresentanteModel();
            model.ListaEmpresaSeleccionado = this.ListaEmpresa;
            return PartialView(ConstantesAdmin.VistaParcialEmpresa, model);
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
        /// Permite grabar los datos del representante
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarUsuario(RepresentanteModel model)
        {
            try
            {
                UserDTO usuario = new UserDTO();
                usuario.UserCode = (short)model.UserCode;
                usuario.UsernName = model.Nombre;
                usuario.UserLogin = model.Email;
                usuario.UserEmail = model.Email;
                usuario.UserState = model.Estado;
                usuario.EmprCodi = (short)model.EmpresaId;
                usuario.UserTlf = model.Telefono;
                usuario.AreaLaboral = model.AreaLaboral;
                usuario.UserCargo = model.Cargo;
                usuario.MotivoContacto = (!string.IsNullOrEmpty(model.MotivoContacto)) ? model.MotivoContacto : string.Empty;
                usuario.LastUser = base.UserName;
                usuario.LastDate = DateTime.Now;
                usuario.ListaEmpresas = this.ListaEmpresa.ToArray();
                usuario.AreaCode = -1;

                List<ModuloDTO> listModulos = this.seguridad.ListarModulos().ToList();
                string modulos = (!string.IsNullOrEmpty(model.Modulos)) ?
                    model.Modulos.Substring(0, model.Modulos.Length - 1) : string.Empty;
                List<int> idModulos = modulos.Split(',').Select(int.Parse).ToList();
                List<int> idRoles = listModulos.Where(x => idModulos.Any(y => y == x.ModCodi)).Select(x => (int)x.RolCode).ToList();
                List<int> idRolesDefecto = this.seguridad.ObtenerRolesPorDefectoExtranet().ToList();
                idRoles.AddRange(idRolesDefecto);
                string roles = string.Join<int>(",", idRoles);
                usuario.Roles = roles;

                bool flag = true;

                if (usuario.UserCode == 0)
                {
                    usuario.UserPass = this.seguridad.GenerarPassword();
                    usuario.UserFCreacion = DateTime.Now;
                    usuario.UserUCreacion = base.UserName;

                    if (this.seguridad.ValidarExistencia(usuario.UserLogin))
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    int resultado = this.seguridad.GrabarUsuario(usuario);
                    this.seguridad.GrabarModuloPorUsuario(resultado, idModulos.ToArray());

                    if (usuario.UserCode == 0)
                    {
                        this.seguridad.EnviarNotificacionUsuario(usuario);
                    }

                    return Json(resultado);
                }
                else
                {
                    return Json(0);
                }
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
