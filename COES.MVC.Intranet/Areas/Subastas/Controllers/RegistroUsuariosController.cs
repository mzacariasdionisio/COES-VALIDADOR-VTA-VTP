using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Subastas.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Subastas.Controllers
{
    public class RegistroUsuariosController : BaseController
    {
        SubastasAppServicio servicio = new SubastasAppServicio();

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }
        public RegistroUsuariosController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Permite mostrar la vista inicial de la app: Registro de Usuarios Por URS
        /// </summary>
        /// <returns></returns>
        public ActionResult Default()
        {
            UsuariosUrsModel model = new UsuariosUrsModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                Log.Info("Listar el total de registro de usuarios por URS - ListSmaUsuarioUrss");
                model.ListaUsuarioUrs = this.servicio.ListSmaUsuarioUrss();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Método de Listar el combo de Empresas
        /// </summary>
        public List<EmpresaModel> ListaEmpresa()
        {
            List<EmpresaModel> list = new List<EmpresaModel>();
            try
            {
                if (User != null)
                {
                    Log.Info("Listando Empresas - ListEmpresaSmaUserEmpresa");
                    List<SmaUserEmpresaDTO> Empresa = this.servicio.ListEmpresaSmaUserEmpresa(ConstantesSubasta.Todos);
                    foreach (SmaUserEmpresaDTO item in Empresa)
                    {
                        list.Add(new EmpresaModel()
                        {
                            Usercode = item.Usercode,
                            Username = item.Username,
                            Emprcodi = item.Emprcodi,
                            Emprnomb = item.Emprnomb
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error al listar Empresas");
            }
            return list;
        }

        /// <summary>
        /// Método de Listar el combo de usuarios
        /// </summary>
        public List<EmpresaModel> ListaUsuarios()
        {
            List<EmpresaModel> list = new List<EmpresaModel>();
            try
            {
                if (User != null)
                {
                    Log.Info("Listando Usuarios - ListSmaUserEmpresa");
                    List<SmaUserEmpresaDTO> Usuarios = this.servicio.ListSmaUserEmpresa(ConstantesSubasta.Todos);
                    foreach (SmaUserEmpresaDTO item in Usuarios)
                    {
                        list.Add(new EmpresaModel()
                        {
                            Usercode = item.Usercode,
                            Username = item.Username,
                            Emprcodi = item.Emprcodi,
                            Emprnomb = item.Emprnomb
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error al Listar Usuarios");
            }
            return list;
        }

        /// <summary>
        /// Filto para cargar Lista de Empresas por Usuario
        /// </summary>
        public JsonResult ListarUsuarios(FormCollection collection)
        {
            short tipo = (!string.IsNullOrEmpty(collection["tipo"]) ? short.Parse(collection["tipo"]) : (short)0);

            Log.Info("Listando Empresas por Usuario - ListSmaUserEmpresa");
            List<SmaUserEmpresaDTO> Usuario = this.servicio.ListSmaUserEmpresa(tipo);
            return Json(Usuario, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Filto para cargar Lista de Usuario por Empresa
        /// </summary>
        public JsonResult ListarEmpresa(FormCollection collection)
        {
            short tipo = (!string.IsNullOrEmpty(collection["tipo"]) ? short.Parse(collection["tipo"]) : (short)0);

            Log.Info("Listando Usuarios por Empresa - ListEmpresaSmaUserEmpresa");
            List<SmaUserEmpresaDTO> empresa = this.servicio.ListEmpresaSmaUserEmpresa(tipo);

            return Json(empresa, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Filtro para cargar Lista de URS por Usuario
        /// </summary>
        public JsonResult ListarUrs(FormCollection collection)
        {
            short tipo = (!string.IsNullOrEmpty(collection["tipo"]) ? short.Parse(collection["tipo"]) : (short)0);

            Log.Info("Listando URS por Usuarios - ListSmaUrsModoOperacions_Urs");
            List<SmaUrsModoOperacionDTO> urs = this.servicio.ListSmaUrsModoOperacions_Urs(tipo);

            return Json(urs, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Validar Usuario y Urs
        /// </summary>
        [HttpPost]
        public JsonResult ValidarUsuarioUrs(FormCollection collection)
        {
            try
            {
                int registro = Convert.ToInt32(collection["registro"]);
                int usuario = Convert.ToInt32(collection["usercode"]);
                string[] urs = collection["Urscodi[]"].Split(',');
                int len = urs.Length;

                int userUrs = -1;
                if (registro != 0)
                {
                    userUrs = registro;
                }
                Log.Info("Validando Usuarios y URS - ListSmaUsuarioUrss / Registrar");
                List<SmaUsuarioUrsDTO> listPer = this.servicio.ListSmaUsuarioUrss();
                foreach (SmaUsuarioUrsDTO item in listPer)
                {
                    if (registro != 0)
                    {
                        if (item.Uurscodi == registro && item.Urscodi == Convert.ToInt32(urs) && item.Usercode == usuario)
                        {
                            return Json(1);
                        }
                        else if (item.Urscodi == Convert.ToInt32(urs) && item.Usercode == usuario)
                        {
                            return Json(0);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < len; i++)
                        {
                            if (item.Urscodi == Convert.ToInt32(urs[i]) && item.Usercode == usuario)
                            {
                                return Json(0);
                            }
                        }
                    }
                }
                return Json(1);
            }
            catch (Exception e)
            {
                Log.Error("Error al Validar Usuario y URS/Registrar");
                return Json(0);
            }
        }

        /// <summary>
        /// Validar Usuario y Urs si existen al intentar mofidificar
        /// </summary>
        [HttpPost]
        public JsonResult ValidarUsuarioUrsMod(FormCollection collection)
        {
            try
            {
                int registro = Convert.ToInt32(collection["registro"]);
                int usuario = Convert.ToInt32(collection["usercode"]);
                int urs = Convert.ToInt32(collection["Urscodi"]);

                int userUrs = -1;
                if (registro != 0)
                {
                    userUrs = registro;
                }

                Log.Info("Validando Usuarios y URS - ListSmaUsuarioUrss / Modificar");
                List<SmaUsuarioUrsDTO> listPer = this.servicio.ListSmaUsuarioUrss();
                foreach (SmaUsuarioUrsDTO item in listPer)
                {
                    if (registro == 0)
                    {
                        if (item.Uurscodi == registro && item.Urscodi == urs && item.Usercode == usuario)
                        {
                            return Json(1);
                        }
                        else if (item.Urscodi == urs && item.Usercode == usuario)
                        {
                            return Json(0);
                        }
                    }
                    else
                    {
                        if (item.Urscodi == urs && item.Usercode == usuario)
                        {
                            return Json(0);
                        }
                    }
                }
                return Json(1);
            }
            catch (Exception e)
            {
                Log.Error("Error al Validar Usuario y URS / Modificar");
                return Json(0);
            }
        }


        ///<summary>
        /// Método Registrar y Modificar Usuarios
        /// </summary>
        [HttpPost]
        public JsonResult MantenimientoUser(FormCollection collection)
        {
            UsuariosUrsModel model = new UsuariosUrsModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                SmaUsuarioUrsDTO bdto = new SmaUsuarioUrsDTO();
                string result = "";
                bdto.Usercode = Convert.ToInt32(collection["Usercode"]);

                string accion = collection["accion"];
                int key = Convert.ToInt32(collection["registro"]);
                if (accion == "Editar")
                {
                    int x2 = Convert.ToInt32(collection["Urscodi"]);
                    bdto.Uurscodi = key;
                    bdto.Urscodi = x2;
                    bdto.Uursusumodificacion = User.Identity.Name;
                    try
                    {
                        Log.Info("Modificando Registro por Usuario - UpdateSmaUsuarioUrs");
                        this.servicio.UpdateSmaUsuarioUrs(bdto);
                        model.Resultado = ConstantesSubasta.Modificar;
                    }
                    catch (Exception e)
                    {
                        if (e.Message == "INFORMACION YA ESTA REGISTRADA")
                            Log.Error("Error al modificar - Registro Duplicado");
                        model.Resultado = ConstantesSubasta.Duplicada;
                    }

                }
                else
                {
                    string[] x2 = collection["Urscodi[]"].Split(',');
                    int len = x2.Length;
                    for (int i = 0; i < len; i++)
                    {
                        bdto.Uurscodi = 0;
                        bdto.Urscodi = Convert.ToInt32(x2[i]);
                        //insert
                        bdto.Uursusucreacion = User.Identity.Name;

                        Log.Info("Registrando Usuarios Por URS - SaveSmaUsuarioUrs");
                        result = this.servicio.SaveSmaUsuarioUrs(bdto);
                        if (result.Substring(0, 2) != "00") break;
                    }

                    model.Resultado = result.Substring(3);

                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Muestra Ventana de Editar Usuario
        /// </summary>
        [HttpPost]
        public ActionResult NuevoUsuario()
        {
            UrsModoOperacionModel model = new UrsModoOperacionModel();

            EmpresaModel empmodel = new EmpresaModel();
            Log.Info("Obtiendo Empresas para visualizar en el Popup Nuevo - ListEmpresaSmaUserEmpresa");
            empmodel.ListaEmpresaUsuarios = this.servicio.ListEmpresaSmaUserEmpresa(-1);
            empmodel.ListaEmpresaUsuarios.Insert(0, empmodel.ListaComboTodosEmpresa);
            ViewData["empresa"] = new SelectList(empmodel.ListaEmpresaUsuarios, "Emprcodi", "Emprnomb", -1);

            EmpresaModel empgmodel = new EmpresaModel();
            Log.Info("Obtiendo Usuarios para visualizar en el Popup Nuevo - ListSmaUserEmpresa");

            List<SmaUserEmpresaDTO> listUser = this.servicio.ListSmaUserEmpresa(-1)
                                                 .GroupBy(s => s.Usercode)
                                                 .Select(grp => grp.FirstOrDefault())
                                                 .OrderBy(s => s.Username)
                                                 .ToList();

            listUser.Insert(0, empmodel.ListaComboTodosUsuario);
            ViewData["username"] = new SelectList(listUser, "Usercode", "Username", -1);

            ViewData["urs"] = new SelectList(this.servicio.ListSmaUrsModoOperacions_Urs(-1), "Urscodi", "Ursnomb", "Todos");

            return View(model);
        }

        /// <summary>
        /// Muestra Ventana de Editar Usuario
        /// </summary>
        [HttpPost]
        public ActionResult EditarUsuario(FormCollection collection)
        {
            try
            {
                int registro = Convert.ToInt32(collection["registro"]);

                UsuariosUrsModel b = new UsuariosUrsModel();
                Log.Info("Obtiendo Registro selecciondo / Popup Editar - ListarUsuarioReg");
                b = this.ListarUsuarioReg(registro);
                ViewData["usercode"] = b.UserCode;
                ViewData["username"] = b.UserName;
                ViewData["urscodi"] = b.UrsCodi;
                ViewData["ursnomb"] = b.UrsNomb;
                ViewData["uurscodi"] = registro;

                int UserConsulta = Convert.ToInt32(b.UserCode);

                List<UrsModoOperacionModel> list = new List<UrsModoOperacionModel>();
                try
                {
                    if (User != null)
                    {
                        Log.Info("Obtendiendo Lista de URS Modo Operacions_URS - ListSmaUrsModoOperacions_Urs");
                        List<SmaUrsModoOperacionDTO> urs = this.servicio.ListSmaUrsModoOperacions_Urs(UserConsulta);
                        foreach (SmaUrsModoOperacionDTO item in urs)
                        {
                            list.Add(new UrsModoOperacionModel()
                            {
                                UrsCodi = item.Urscodi,
                                UrsNomb = item.Ursnomb

                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error al obtener lista de URS Modo Operacions_URS");
                }
                Json(list);

                EmpresaModel empmodel = new EmpresaModel();
                Log.Info("Listando Empresas por Usuario - ListEmpresaSmaUserEmpresa");
                empmodel.ListaEmpresaUsuarios = this.servicio.ListEmpresaSmaUserEmpresa(UserConsulta);

                ViewData["empresaedit"] = new SelectList(empmodel.ListaEmpresaUsuarios, "Emprcodi", "Emprnomb", empmodel.ListaEmpresaUsuarios);

                ViewBag.Listaurs = (list);

                return View(b);

            }
            catch (Exception e)
            {
                Log.Error("Error al Listar Empresas por Usuario");
                return new HttpStatusCodeResult(404, ConstantesSubasta.ErrorDeSistema);
            }
        }

        /// <summary>
        /// Metodo de Listar Usuarios por URS
        /// </summary>
        private List<UsuariosUrsModel> ListarUsuariosUrs()
        {
            List<UsuariosUrsModel> list = new List<UsuariosUrsModel>();
            try
            {
                if (User != null)
                {
                    Log.Info("Listando el total de Registros Usuarios por URS - ListSmaUsuarioUrss");
                    List<SmaUsuarioUrsDTO> opciones = this.servicio.ListSmaUsuarioUrss();
                    foreach (SmaUsuarioUrsDTO c in opciones)
                    {
                        list.Add(new UsuariosUrsModel()
                        {
                            UursCodi = c.Uurscodi,
                            UrsCodi = c.Urscodi,
                            UursUsucreacion = c.Uursusucreacion,
                            UursUsumodificacion = c.Uursusumodificacion,
                            UursFecmodificacion = c.Uursfecmodificacion,
                            UserCode = c.Usercode,
                            UursEstado = c.Uursestado,
                            UursFeccreacion = c.Uursfeccreacion,
                            UrsNomb = c.Ursnomb,
                            UrsTipo = c.Urstipo,
                            GrupoCodi = c.Grupocodi,
                            GrupoNom = c.Gruponom,
                            UserName = c.Username,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error al Listar eltotal de Registro de Usuarios por URS");
            }
            return list;
        }

        /// <summary>
        /// Metodo Listar Usuario por Registro
        /// </summary>
        public UsuariosUrsModel ListarUsuarioReg(int registro)
        {
            UsuariosUrsModel b = new UsuariosUrsModel();
            try
            {
                SmaUsuarioUrsDTO c = new SmaUsuarioUrsDTO();
                Log.Info("Obteniendo Registro seleccionado - GetByIdSmaUsuarioUrs");
                c = this.servicio.GetByIdSmaUsuarioUrs(registro);
                b.UrsCodi = c.Urscodi;
                b.UserCode = c.Usercode;
                b.UrsNomb = c.Ursnomb;
                b.UserName = c.Username;

            }
            catch (Exception e)
            {
                Log.Error("Error al obtener el registro seleccionado");
            }
            return b;
        }

        /// <summary>
        /// Método Eliminar Usuario
        /// </summary>
        [HttpPost]
        public JsonResult UsuarioEliminado(FormCollection collection)
        {
            UsuariosUrsModel model = new UsuariosUrsModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                SmaUsuarioUrsDTO per = new SmaUsuarioUrsDTO();
                per.Uurscodi = Convert.ToInt32(collection["registro"]);
                per.Uursusumodificacion = User.Identity.Name;
                Log.Info("Eliminando Usuario por URS - DeleteSmaUsuarioUrs");
                this.servicio.DeleteSmaUsuarioUrs(per.Uurscodi, per.Uursusumodificacion);
                model.Resultado = ConstantesSubasta.Eliminar;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }


    }
}
