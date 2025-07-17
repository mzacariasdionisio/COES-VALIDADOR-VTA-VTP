using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class RepresentanteController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Lista de Empresa del usuario
        /// </summary>
        public List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO> ListaEmpresa
        {
            get
            {
                return (Session[ConstantesAdmin.SesionEmpresa] != null) ?
                    (List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO>)Session[ConstantesAdmin.SesionEmpresa] :
                    new List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO>();
            }
            set { Session[ConstantesAdmin.SesionEmpresa] = value; }
        }

        /// <summary>
        /// Pagina inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RepresentanteModel model = new RepresentanteModel();
            model.ListaEmpresaFiltro = (new GeneralAppServicio()).ObtenerEmpresasCOES();
            model.ListaTipoEmpresaFiltro = (new GeneralAppServicio()).ListarTiposEmpresa();
            return View(model);
        }

        /// <summary>
        /// Permite listar las empresas por tipo
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerEmpresas(int idTipoEmpresa)
        {
            List<SiEmpresaDTO> entitys = (new GeneralAppServicio()).ListarEmpresasPorTipo(idTipoEmpresa.ToString());
            return Json(entitys);
        }

        /// <summary>
        /// Muestra la lista de usuarios representantes
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int? idTipoEmpresa, int? idEmpresa)
        {
            RepresentanteModel model = new RepresentanteModel();
            if (idTipoEmpresa == null) idTipoEmpresa = -2;
            if (idEmpresa == null) idEmpresa = -2;
            model.ListaUsuarios = this.seguridad.ObtenerUsuarioRepLegales(idTipoEmpresa, idEmpresa).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el detalle de empresas
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Detalle(int idUsuario)
        {
            RepresentanteModel model = new RepresentanteModel();
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.EMPRCODI > 1).OrderBy(x => x.EMPRNOMB).ToList();

            if (idUsuario == 0)
            {
                model.Entidad = new SeguridadServicio.UserDTO();
                model.Entidad.UserCode = 0;
                model.Entidad.UserState = Constantes.EstadoActivo;
            }
            else
            {
                model.Entidad = this.seguridad.ObtenerUsuario(idUsuario);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del representante
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(RepresentanteModel model)
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
                usuario.Usermovil = model.Celular;
                usuario.MotivoContacto = (!string.IsNullOrEmpty(model.MotivoContacto)) ? model.MotivoContacto.Trim() : null;
                usuario.UserIndReprLeg = Constantes.SI;
                usuario.LastUser = base.UserName;
                usuario.LastDate = DateTime.Now;
                usuario.ListaEmpresas = (new List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO>()).ToArray();
                usuario.AreaCode = -1;
                usuario.Roles = ConfigurationManager.AppSettings[RutaDirectorio.RolRepresentanteLegal];

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

                    if (usuario.UserCode == 0)
                    {
                        this.seguridad.EnviarNotificacionRepresentante(usuario);
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

        /// <summary>
        /// Permite cambiar la clave
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarClave(int idUsuario)
        {
            try
            {
                this.seguridad.EnviarPasswordNuevo(idUsuario, base.UserName);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
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
            model.ListaEmpresas = this.seguridad.ObtenerTotalEmpresas().ToList();
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
            model.ListaUsuarios = this.seguridad.ListarUsuariosPorEmpresa(idEmpresa).ToList();
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
            model.ListaEmpresas = this.seguridad.ObtenerTotalEmpresas().ToList();
            List<ModuloDTO> modulo = this.seguridad.ListarModulos().Where(x => (x.RolName.StartsWith("Usuario Extranet") || x.RolName.StartsWith("Extranet")) && x.ModEstado.Equals("A")).OrderBy(x => x.ModNombre).ToList();
            List<ModuloDTO> moduloSelecc = this.seguridad.ObtenerModulosPorUsuarioSelecion(idUsuario).ToList();
            model.ListaModulo = moduloSelecc.Where(x => modulo.Any(y => y.ModCodi == x.ModCodi) && x.ModCodi != 41).ToList();
            //model.ListaModulo = this.seguridad.ObtenerModulosPorUsuarioSelecion(idUsuario).ToList();

            if (idUsuario == 0)
            {
                model.Entidad = new SeguridadServicio.UserDTO();
                //Session.Remove(ConstantesAdmin.SesionEmpresa);
                this.ListaEmpresa = null;
                model.ListaEmpresaSeleccionado = null;
                model.Entidad.UserCode = 0;
                model.Entidad.EmprCodi = (short)idEmpresa;
                model.Entidad.UserState = Constantes.EstadoActivo;
            }
            else
            {
                //Session.Remove(ConstantesAdmin.SesionEmpresa);
                this.ListaEmpresa = null;
                model.Entidad = this.seguridad.ObtenerUsuario(idUsuario);
                this.ListaEmpresa = this.seguridad.ObtenerEmpresasPorUsuario(model.Entidad.UserLogin).ToList();
                model.ListaEmpresaSeleccionado = this.ListaEmpresa;
                //OBTENER FLAG PERMISO
                model.Permiso = (new IntervencionesAppServicio()).ObtenerFlagUserPermiso(idUsuario);
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
            List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO> lista = this.ListaEmpresa;
            if (lista.Where(x => x.EMPRCODI == idEmpresa).Count() == 0)
            {
                COES.MVC.Intranet.SeguridadServicio.EmpresaDTO dto = this.seguridad.ObtenerEmpresa(idEmpresa);
                lista.Add(dto);
            }
            this.ListaEmpresa = lista;
            RepresentanteModel model = new RepresentanteModel();
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
            List<COES.MVC.Intranet.SeguridadServicio.EmpresaDTO> lista = this.ListaEmpresa;
            COES.MVC.Intranet.SeguridadServicio.EmpresaDTO item = lista.Where(x => x.EMPRCODI == idEmpresa).FirstOrDefault();
            if (item != null)
            {
                lista.Remove(item);
            }

            this.ListaEmpresa = lista;

            RepresentanteModel model = new RepresentanteModel();
            model.ListaEmpresaSeleccionado = this.ListaEmpresa;
            return PartialView(VistasParciales.Empresa, model);
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

                //Notificaciones
                string valorKeyFlagEnviarNotificacionAAgente = ConfigurationManager.AppSettings[NotificacionAplicativo.KeyNotificacionFlagEnviarAAgente];
                bool flagEnviarNotifAgente = valorKeyFlagEnviarNotificacionAAgente == "S";
                if (flagEnviarNotifAgente)
                {
                    this.seguridad.EnviarNotificacionBajaUsuario(idUsuario);
                }

                (new IntervencionesAppServicio()).DarBajaConfiguracionNotificacion(idUsuario, base.UserName);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
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
                usuario.Usermovil = model.Celular;

                List<ModuloDTO> listModulos = this.seguridad.ListarModulos().ToList();
                string modulos = (!string.IsNullOrEmpty(model.Modulos)) ?
                    model.Modulos.Substring(0, model.Modulos.Length - 1) : string.Empty;
                List<int> idModulos = modulos.Split(',').Select(int.Parse).ToList();
                List<int> idRoles = listModulos.Where(x => idModulos.Any(y => y == x.ModCodi)).Select(x => (int)x.RolCode).ToList();
                List<int> idRolesDefecto = this.seguridad.ObtenerRolesPorDefectoExtranet().ToList();
                idRoles.AddRange(idRolesDefecto);
                string roles = string.Join<int>(",", idRoles);
                usuario.Roles = roles;

                List<ModuloDTO> modulosAcceso = listModulos.Where(x => idModulos.Any(y => x.ModCodi == y)).ToList();

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
                    //verificar que hay cambios de checks en módulos para el usuario
                    bool flagModulo = true;
                    if (usuario.UserCode > 0)
                    {
                        List<int> idsModulos = this.seguridad.ObtenerModulosPorUsuarioSelecion(usuario.UserCode).
                                               Where(x => x.Selected > 0).Select(x => (int)x.ModCodi).ToList();

                        if (idModulos.Count == idsModulos.Count)
                        {
                            idsModulos.Sort();
                            idModulos.Sort();

                            bool flagBusqueda = true;
                            for (int i = 0; i < idModulos.Count; i++)
                            {
                                if (idsModulos[i] != idModulos[i])
                                {
                                    flagBusqueda = false;
                                }
                            }

                            if (flagBusqueda)
                                flagModulo = false;
                            else flagModulo = true;
                        }
                    }

                    int resultado = this.seguridad.GrabarUsuario(usuario);
                    this.seguridad.GrabarModuloPorUsuario(resultado, idModulos.ToArray());

                    //>>>>>>>>>>>ACTUALIZAR FLAG PERMISO>>>>>>>>>>>>>>>>
                    var result = (new IntervencionesAppServicio()).UpdateUserPermiso(resultado, model.Permiso);

                    //Notificaciones
                    string valorKeyFlagEnviarNotificacionAAgente = ConfigurationManager.AppSettings[NotificacionAplicativo.KeyNotificacionFlagEnviarAAgente];
                    bool flagEnviarNotifAgente = valorKeyFlagEnviarNotificacionAAgente == "S";

                    if (usuario.UserCode == 0)
                    {
                        if (flagEnviarNotifAgente)
                            this.seguridad.EnviarNotificacionUsuario(usuario, modulosAcceso.ToArray());
                    }
                    else
                    {
                        if (flagModulo)
                        {
                            if (flagEnviarNotifAgente)
                                this.seguridad.EnviarNotificacionModificacionUsuario(usuario, modulosAcceso.ToArray());
                        }
                    }

                    //Agregar registros para el módulo de Intervenciones
                    bool tieneCheckModuloIntervenciones = idModulos.Contains(ConstantesIntervencionesAppServicio.ModcodiIntervenciones);
                    (new IntervencionesAppServicio()).GenerarConfiguracionPorDefecto(resultado, this.ListaEmpresa.Select(x => (int)x.EMPRCODI).ToList(), 
                                                                    tieneCheckModuloIntervenciones, base.UserName);

                    return Json(resultado);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }


    }
}
