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
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Subastas.Controllers
{
    public class ModoOperacionController : BaseController
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
        public ModoOperacionController()
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
        /// Muestra la ventana principal Modo Operación
        /// </summary>
        public ActionResult Default()
        {
            ProcesoModel model = new ProcesoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                Log.Info("Listando el total de registros de Modo Operaciones Validos");
                model.ModoOperacion = this.servicio.ListAllSmaModoOperVals();

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
        /// Método Registrar y Modificar Modo Operación
        /// </summary>
        [HttpPost]
        public JsonResult MantenimientoModoOperacion(FormCollection collection)
        {
            ProcesoModel model = new ProcesoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                SmaModoOperValDTO bdto = new SmaModoOperValDTO();
                string result = "";
                bdto.Grupocodi = Convert.ToInt32(collection["grupocodi"]);

                string accion = collection["accion"];
                int key = Convert.ToInt32(collection["registro"]);

                if (accion == "Editar")
                {
                    int x2 = Convert.ToInt32(collection["grupocodi"]);
                    bdto.Mopvcodi = key;
                    bdto.Grupocodi = x2;
                    bdto.Mopvusumodificacion = User.Identity.Name;
                    try
                    {
                        Log.Info("Modificando Registro Modo Operacion Valido - UpdateSmaModoOperVal");
                        this.servicio.UpdateSmaModoOperVal(bdto);
                        model.Resultado = ConstantesSubasta.Modificar;
                    }
                    catch (Exception e)
                    {
                        if (e.Message == "INFORMACION YA ESTA REGISTRADA")

                            model.Resultado = ConstantesSubasta.Duplicada;
                    }

                }
                else
                {
                    string[] x2 = collection["grupocodi[]"].Split(',');
                    int b = 0;
                    int len = x2.Length;
                    int numVal = this.servicio.GetNumModoOperVal();
                    for (int i = 0; i < len; i++)
                    {
                        bdto.Mopvcodi = 0;
                        bdto.Grupocodi = Convert.ToInt32(x2[i]);

                        bdto.Mopvusucreacion = User.Identity.Name;
                        bdto.Mopvgrupoval = numVal;
                        Log.Info("Registrando Modo Operacion Valido - SaveSmaModoOperVal");
                        result = this.servicio.SaveSmaModoOperVal(bdto);
                        if (result.Substring(0, 2) != "00") break;
                    }

                    model.Resultado = result.Substring(3).ToString();

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
        /// Método Eliminar Modo Operación
        /// </summary>
        [HttpPost]
        public JsonResult EliminarModoOperacion(FormCollection Collection)
        {
            ProcesoModel model = new ProcesoModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                SmaModoOperValDTO dl = new SmaModoOperValDTO();
                dl.Mopvcodi = Convert.ToInt32(Collection["registro"]);
                dl.Mopvusumodificacion = User.Identity.Name;
                Log.Info("Eliminando Registro Modo Operación Valido - DeleteSmaModoOperVal");
                this.servicio.DeleteSmaModoOperVal(dl.Mopvusumodificacion, dl.Mopvcodi);
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

        /// <summary>
        /// Muestra Ventana Editar Modo Operación
        /// </summary>
        public PartialViewResult EditarModoOperacion(FormCollection collection)
        {

            int registro = Convert.ToInt32(collection["registro"]);

            SmaModoOperValDTO bdto = new SmaModoOperValDTO();
            Log.Info("Obteniendo datos de registro seleccionado - GetByIdSmaModoOperVal");
            bdto = this.servicio.GetByIdSmaModoOperVal(registro);

            List<UrsModoOperacionModel> list = new List<UrsModoOperacionModel>();
            try
            {
                if (User != null)
                {
                    Log.Info("Listando registros Modo Operación Disponible por URS - ListSmaModoOperDisponibles");
                    List<SmaModoOperValDTO> MO = this.servicio.ListSmaModoOperDisponibles(bdto.Mopvgrupoval, bdto.Urscodi);
                    foreach (SmaModoOperValDTO item in MO)
                    {
                        list.Add(new UrsModoOperacionModel()
                        {
                            Grupocodi = item.Grupocodi,
                            Gruponomb = item.Gruponomb
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error al Listar Registros Modo Operacion Disponible por URS");
            }
            Json(list);

            ViewBag.ListaMO = (list);
            ViewData["urscodi"] = bdto.Urscodi;
            ViewData["ursnomb"] = bdto.Ursnomb;
            ViewData["grupocodi"] = bdto.Grupocodi;
            ViewData["gruponomb"] = bdto.Gruponomb;

            return PartialView(bdto);
        }

        /// <summary>
        /// Muestra Ventana Nuevo Modo Operación
        /// </summary>
        public PartialViewResult NuevoModoOperacion(FormCollection collection)
        {
            int tipo = Convert.ToInt32(collection["tipo"]);

            UrsModoOperacionModel List = new UrsModoOperacionModel();
            Log.Info("Listando total de registos de URS Modo Operacion/UrsCodi and UrsNomb - ListSmaUrsModoOperacions");
            List.ListaUrsModoOperacion = this.servicio.ListSmaUrsModoOperacions();
            List.ListaUrsModoOperacion.Insert(0, List.ListaComboSeleccione);
            ViewData["urs"] = new SelectList(List.ListaUrsModoOperacion, "Urscodi", "Ursnomb", -1);

            Log.Info("Listando total de registos de URS Modo Operacion/GrupoCodi and GrupoNomb - ListSmaUrsModoOperacions");
            List.ListaUrsModoOperacionMO = servicio.ListSmaUrsModoOperacions_MO(tipo);
            ViewData["atributo"] = new SelectList(List.ListaUrsModoOperacionMO, "Grupocodi", "Gruponomb", -1);

            return PartialView(List);

        }

        /// <summary>
        /// Filtro para cargar Lista de URS por Usuario
        /// </summary>
        public JsonResult ListarMO(FormCollection collection)
        {
            short tipo = (!string.IsNullOrEmpty(collection["tipo"]) ? short.Parse(collection["tipo"]) : (short)0);
            Log.Info("Lista de URS por Usuario - ListSmaUrsModoOperacions_MO");
            List<SmaUrsModoOperacionDTO> MO = this.servicio.ListSmaUrsModoOperacions_MO(tipo);
            return Json(MO, JsonRequestBehavior.AllowGet);

        }

    }
}
