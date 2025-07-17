using COES.Dominio.DTO.Transferencias;
//using COES.MVC.Intranet.Areas.Resarcimientos.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Areas.Resarcimientos.Models;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE.Helper;

namespace COES.MVC.Intranet.Areas.Resarcimientos.Controllers
{
    public class PeriodoController : Controller
    {
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
        public PeriodoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instanciamiento de web Servis
        /// </summary>
        SeguridadServicioClient servicio = new SeguridadServicioClient();

        /// <summary>
        /// Instanciamiento de Servicios de Aplicacion
        /// </summary>
        ResarcimientoNTCSEAppServicio ntcse = new ResarcimientoNTCSEAppServicio();


        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Valida Session true
        /// </summary>
        public void ValidarSessionUsuarios()
        {
            HttpCookie cookie = Request.Cookies[DatosSesion.InicioSesion];

            if (cookie != null)
            {
                if (cookie[DatosSesion.SesionUsuario] != null)
                {
                    Session[DatosSesion.SesionMapa] = Constantes.NodoPrincipal;
                    Session[DatosSesion.SesionUsuario] = this.servicio.ObtenerUsuarioPorLogin(cookie[DatosSesion.SesionUsuario].ToString());
                    FormsAuthentication.SetAuthCookie(cookie[DatosSesion.SesionUsuario].ToString(), false);

                }
            }
            else
            {
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    Session[DatosSesion.SesionMapa] = Constantes.NodoPrincipal;
                }
                else
                {
                    Response.Redirect("~/");
                }
            }
        }

        #region MÉTODOS PERIODO

        /// <summary>
        /// Muestra Ventana Principal de Periodo
        /// </summary>
        public ActionResult Periodo()
        {
            ValidarSessionUsuarios();
            try
            {
                // listar periodo
                List<PeriodosModel> b = new List<PeriodosModel>();
                b = this.ListarPeriodos();

                return View(b);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
                return new HttpStatusCodeResult(404, ConstantesResarcimiento.ErrorDeSistema);
            }
        }

        /// <summary>
        /// Muestra Ventana Principal de Periodo
        /// </summary>
        public string Validacion(FormCollection collection )
        {
            ValidarSessionUsuarios();
            string estado = collection["estado"];
            string periodo = collection["periodo"];
            return "Se ha "+estado+" el periodo: " + periodo;
        }

        /// <summary>
        /// Validar Periodo si existe
        /// </summary>
        [HttpPost]
        public JsonResult ValidarPeriodo(FormCollection collection)
        {
            try
            {
                int registro = Convert.ToInt32(collection["registro"]);
                string semestre = collection["periodo"];
                int anio = Convert.ToInt32(collection["anio"]);
                int periodo = -1;
                if (registro != 0)
                {
                    periodo = registro;
                }
                List<RntRegRechazoCargaDTO> opcionesRrc = ntcse.GetByCriteriaRntRegRechazoCargas("0", 0, periodo, 0, 0, 0, DateTime.MinValue);
                List<RntRegPuntoEntregaDTO> opcionesRpe = ntcse.GetByCriteriaRntRegPuntoEntregas("0", 0, periodo, 0, 0, 0, DateTime.MinValue);
                if (opcionesRrc.Count > 0 || opcionesRpe.Count > 0)
                {
                    return Json(0);
                }
                else
                {
                    List<RntPeriodoDTO> listPer = ntcse.ListRntPeriodos();
                    foreach (RntPeriodoDTO item in listPer)
                    {
                        if (registro != 0)
                        {
                            if (item.PeriodoCodi == registro && item.PerdAnio == anio && item.PerdSemestre == semestre)
                            {
                                return Json(1);
                            }
                            else if (item.PerdAnio == anio && item.PerdSemestre == semestre)
                            {
                                return Json(0);
                            }
                        }
                        else
                        {
                            if (item.PerdAnio == anio && item.PerdSemestre == semestre)
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
                Log.Error("Error", e);
                return Json(0);
            }
        }

        /// <summary>
        /// Permite realizar el Guardar y Modificar Registros de Periodos
        /// </summary>
        [HttpPost]
        public string MantenimientoPeriodo(FormCollection collection)
        {
            ValidarSessionUsuarios();

            RntPeriodoDTO bdto = new RntPeriodoDTO();
            bdto.PerdAnio = Convert.ToInt32(collection["anio"]);
            bdto.PerdSemestre = Convert.ToString(collection["periodo"]);
            bdto.PerdEstado = Convert.ToString(collection["estado"]);


            string accion = collection["accion"];
            int key = Convert.ToInt32(collection["registro"]);
            if (accion == "Editar")
            {
                bdto.PeriodoCodi = key;
                bdto.PerdUsuarioUpdate = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                bdto.PerdFechaUpdate = DateTime.Now;
                try
                {
                    Log.Debug(HelperResarcimientos.ListarPeriodoDebugLog(bdto));
                    ntcse.UpdateRntPeriodo(bdto);
                }
                catch (Exception e)
                {
                    if (e.Message == "INFORMACION YA ESTA REGISTRADA")

                        return ConstantesResarcimiento.Duplicada_Eliminado;
                }
                //Auditoria
                return ConstantesResarcimiento.Modificar;
            }
            else
            {
                bdto.PeriodoCodi = 0;
                bdto.PerdUsuarioCreacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                bdto.PerdFechaCreacion = DateTime.Now;
                try
                {
                    ntcse.SaveRntPeriodo(bdto);
                }
                catch (Exception e)
                {
                    Log.Error("Error", e);
                    if (e.Message == "INFORMACION YA ESTA REGISTRADA")
                        return ConstantesResarcimiento.Duplicada_Eliminado;
                }

                return ConstantesResarcimiento.Registrar;
            }
        }

        /// <summary>
        /// Eliminar Periodo
        /// </summary>
        [HttpPost]
        public string PeriodoEliminado(FormCollection collection)
        {
            ValidarSessionUsuarios();
            try
            {
                RntPeriodoDTO per = new RntPeriodoDTO();
                per.PeriodoCodi = Convert.ToInt32(collection["registro"]);
                per.PerdUsuarioUpdate = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                per.PerdFechaUpdate = DateTime.Now;
                ntcse.DeleteRntPeriodo(per);
                return ConstantesResarcimiento.Eliminar;
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
                return ConstantesResarcimiento.ErrorDeSistema;
            }
        }
        /// <summary>
        /// Permite dar nonbre a estado en Habilitado o Bloquedo
        /// </summary>
        [HttpPost]
        public string EstadoPeriodo(FormCollection collection)
        {
            ValidarSessionUsuarios();

            RntPeriodoDTO bdto = new RntPeriodoDTO();
            try
            {

                int key = Convert.ToInt32(collection["registro"]);
                bdto = ntcse.GetByIdRntPeriodo(key);
                if (bdto.PerdEstado == ConstantesResarcimiento.Periodoestadohabilitado)
                {
                    bdto.PerdEstado = ConstantesResarcimiento.Periodoestadobloqueado;
                }
                else
                {
                    bdto.PerdEstado = ConstantesResarcimiento.Periodoestadohabilitado;
                }

                bdto.PeriodoCodi = key;
                bdto.PerdUsuarioUpdate = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                bdto.PerdFechaUpdate = DateTime.Now;
                ntcse.UpdateRntPeriodo(bdto);
                PeriodosModel p = new PeriodosModel();
                p.Perdestado = bdto.PerdEstado;
                return p.Estado;
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
                return bdto.PerdEstado;
            }

        }

        /// <summary>
        /// Muestra Ventana de Periodo Nuevo
        /// </summary>
        public ActionResult PeriodoNuevo()
        {
            ValidarSessionUsuarios();
            try
            {
                ViewData["periodo"] = new SelectList(ntcse.ListComboConfiguracion(ConstantesResarcimiento.Parametro), "Confvalor", "Confvalor", "SI");
                //CboEstado
                ViewData["CboEstado"] = new SelectList(new[] { new { ID = "0", Name = "Habilitado" }, new { ID = "1", Name = "Bloqueado" } }, "ID", "Name", "0");

                return View();
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
                return new HttpStatusCodeResult(404, ConstantesResarcimiento.ErrorDeSistema);
            }
        }

        /// <summary>
        /// Muestra Ventana de Editar Periodo
        /// </summary>
        [HttpPost]
        public ActionResult EditarPeriodo(FormCollection Collection)
        {
            ValidarSessionUsuarios();
            try
            {
                int registro = Convert.ToInt32(Collection["registro"]);
                PeriodosModel b = new PeriodosModel();
                b = ListarPeriodoREg(registro);

                List<PeriodosModel> list = new List<PeriodosModel>();
                List<PeriodosModel> listp = new List<PeriodosModel>();
                int anio = 0;

                foreach (PeriodosModel a in ListarPeriodos())
                {
                    if (anio == 0 || anio != a.Perdanio)
                    {
                        list.Add(a);
                        anio = Convert.ToInt32(a.Perdanio);
                    }
                }

                ViewData["anio"] = new SelectList(list, "Perdanio", "Perdanio", b.Perdanio);
                ViewData["periodo"] = new SelectList(ntcse.ListComboConfiguracion(ConstantesResarcimiento.Parametro), "Confvalor", "Confvalor", b.Perdsemestre);
                //CboEstado
                ViewData["CboEstado"] = new SelectList(new[] { new { ID = "0", Name = "Habilitado" }, new { ID = "1", Name = "Bloqueado" } }, "ID", "Name", b.Perdestado);


                return View(b);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
                return new HttpStatusCodeResult(404, ConstantesResarcimiento.ErrorDeSistema);
            }
        }

        #endregion

        #region LISTAR

        /// <summary>
        /// Metodo de Listar por Periodo
        /// </summary>
        private List<PeriodosModel> ListarPeriodos()
        {
            List<PeriodosModel> list = new List<PeriodosModel>();
            try
            {
                if (User != null)
                {
                    Log.Info("iniciando Periodo");
                    List<RntPeriodoDTO> opciones = ntcse.ListRntPeriodos();
                    foreach (RntPeriodoDTO item in opciones)
                    {
                        list.Add(new PeriodosModel()
                        {
                            Periodocodi = item.PeriodoCodi,
                            Perdestado = item.PerdEstado,
                            Perdanio = item.PerdAnio,
                            Perdnombre = item.PerdNombre,
                            Perdusuariocreacion = item.PerdUsuarioCreacion,
                            Perdfechacreacion = item.PerdFechaCreacion,
                            Perdusuarioupdate = item.PerdUsuarioUpdate,
                            Perdfechaupdate = item.PerdFechaUpdate,
                            Perdsemestre = item.PerdSemestre,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return list;
        }

        /// <summary>
        /// Metodo Listar Periodo por Registro
        /// </summary>
        public PeriodosModel ListarPeriodoREg(int registro)
        {

            PeriodosModel b = new PeriodosModel();
            try
            {
                RntPeriodoDTO c = new RntPeriodoDTO();
                c = ntcse.GetByIdRntPeriodo(registro);
                b.Periodocodi = c.PeriodoCodi;
                b.Perdanio = c.PerdAnio;
                b.Perdsemestre = c.PerdSemestre;
                b.Perdestado = c.PerdEstado;
                b.Perdusuariocreacion = c.PerdUsuarioCreacion;
                b.Perdfechacreacion = c.PerdFechaCreacion;
                b.Perdusuarioupdate = c.PerdUsuarioUpdate;
                b.Perdfechaupdate = c.PerdFechaUpdate;
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return b;
        }

        #endregion
    }
}
