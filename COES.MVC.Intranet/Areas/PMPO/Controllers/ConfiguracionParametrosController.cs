using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class ConfiguracionParametrosController : BaseController
    {
        ProgramacionAppServicio pmpo = new ProgramacionAppServicio();
        SeguridadServicioClient servSeguridad = new SeguridadServicioClient();

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region Asuntos de Correos

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ListaConfiguracionCorreo()
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaConfig = pmpo.ListarConfiguracionCorreo();
                model.Resultado = "1";
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

        [HttpPost]
        public JsonResult GuardarConfCorreo(int confpmcodi, string asunto)
        {
            RemisionModel modelResultado = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                var regParam = pmpo.GetByIdPmpoConfiguracion(confpmcodi);

                regParam.Confpmvalor = (asunto ?? "").Trim();
                regParam.Confpmusumodificacion = base.UserName;
                if (string.IsNullOrEmpty(regParam.Confpmvalor)) throw new ArgumentException("Debe ingresar asunto.");

                pmpo.UpdatePmpoConfiguracion(regParam);

                modelResultado.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResultado.Resultado = "-1";
                modelResultado.Mensaje = ex.Message;
                modelResultado.Detalle = ex.StackTrace;
            }

            return Json(modelResultado);
        }

        #endregion

        #region Parámetros de Plazos

        public ActionResult ConfParametroPlazos()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            RemisionModel model = new RemisionModel();

            model.Mes = pmpo.GetMesElaboracionDefecto().ToString(ConstantesAppServicio.FormatoMes);

            return View(model);
        }

        [HttpPost]
        public JsonResult ObtenerConfParametroPlazo()
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                var regParam1 = pmpo.GetByIdPmpoConfiguracion(ConstantesPMPO.ConfpmcodiPlazoIni);
                var regParam2 = pmpo.GetByIdPmpoConfiguracion(ConstantesPMPO.ConfpmcodiPlazoFin);
                var regParam3 = pmpo.GetByIdPmpoConfiguracion(ConstantesPMPO.ConfpmcodiFueraPlazoIni);

                int[] lParam1 = (regParam1.Confpmvalor ?? "").Trim().Split('|').Select(x => Convert.ToInt32(x)).ToArray();
                int[] lParam2 = (regParam2.Confpmvalor ?? "").Trim().Split('|').Select(x => Convert.ToInt32(x)).ToArray();
                int[] lParam3 = (regParam3.Confpmvalor ?? "").Trim().Split('|').Select(x => Convert.ToInt32(x)).ToArray();

                model.Mesplazo = lParam1[0];
                model.Mesfinplazo = lParam2[0];
                model.Mesfinfueraplazo = lParam3[0];

                model.DiaPlazo = lParam1[1];
                model.DiaFinPlazo = lParam2[1];
                model.DiaFinFueraPlazo = lParam3[1];

                model.MinutoPlazo = lParam1[2];
                model.MinutoFinPlazo = lParam2[2];
                model.MinutoFinFueraPlazo = lParam3[2];

                model.Resultado = "1";
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

        [HttpPost]
        public JsonResult GuardarConfParametroPlazo(RemisionModel model)
        {
            RemisionModel modelResultado = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                var regParam1 = pmpo.GetByIdPmpoConfiguracion(ConstantesPMPO.ConfpmcodiPlazoIni);
                var regParam2 = pmpo.GetByIdPmpoConfiguracion(ConstantesPMPO.ConfpmcodiPlazoFin);
                var regParam3 = pmpo.GetByIdPmpoConfiguracion(ConstantesPMPO.ConfpmcodiFueraPlazoIni);

                int[] lParam1 = new int[3];
                int[] lParam2 = new int[3];
                int[] lParam3 = new int[3];

                lParam1[0] = model.Mesplazo;
                lParam2[0] = model.Mesfinplazo;
                lParam3[0] = model.Mesfinfueraplazo;

                lParam1[1] = model.DiaPlazo;
                lParam2[1] = model.DiaFinPlazo;
                lParam3[1] = model.DiaFinFueraPlazo;

                lParam1[2] = model.MinutoPlazo;
                lParam2[2] = model.MinutoFinPlazo;
                lParam3[2] = model.MinutoFinFueraPlazo;

                regParam1.Confpmvalor = string.Join("|", lParam1);
                regParam2.Confpmvalor = string.Join("|", lParam2);
                regParam3.Confpmvalor = string.Join("|", lParam3);
                regParam1.Confpmusumodificacion = base.UserName;
                regParam2.Confpmusumodificacion = base.UserName;
                regParam3.Confpmusumodificacion = base.UserName;

                pmpo.UpdatePmpoConfiguracion(regParam1);
                pmpo.UpdatePmpoConfiguracion(regParam2);
                pmpo.UpdatePmpoConfiguracion(regParam3);

                modelResultado.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResultado.Resultado = "-1";
                modelResultado.Mensaje = ex.Message;
                modelResultado.Detalle = ex.StackTrace;
            }

            return Json(modelResultado);
        }

        #endregion

        #region FileServer Pmpo

        public ActionResult IndexFileServer()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            RemisionModel model = new RemisionModel();

            var objUsuario = this.servSeguridad.ObtenerUsuarioPorLogin(User.Identity.Name);
            model.TienePermisoDTI = objUsuario != null && objUsuario.AreaCode == 1;

            //Archivos
            model.PathPrincipal = this.pmpo.GetPathPrincipal();
            model.PathAplicativo = this.pmpo.GetPathPrincipal() + ConstantesPMPO.CarpetaFileServerPmpo;
            model.Resultado = string.Join("|", pmpo.ListarSubcarpetaFromPrincipal());

            return View(model);
        }

        /// <summary>
        /// Permite mostrar los archivos relacionado a la subcarpeta
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="subcarpeta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerFolder(string subcarpeta)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                string path = this.pmpo.GetPathSubcarpeta(subcarpeta);
                model.PathSubcarpeta = path;
                model.Resultado = "1";
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
        /// Permite mostrar los archivos relacionado a la subcarpeta
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarANuevaEstructuraFS()
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                this.pmpo.EjecutarCopiaANuevaEstructuraFS();
                model.Resultado = "1";
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

        #endregion
    }
}