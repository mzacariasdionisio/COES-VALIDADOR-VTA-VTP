
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class VolumenEmbalseController : BaseController
    {
        private readonly ProgramacionAppServicio pmpoServicio = new ProgramacionAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ParametrosFechasController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Index 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? mtopcodi = 0)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            VolumenEmbalseModel model = new VolumenEmbalseModel();

            model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            model.Mtopcodi = mtopcodi.Value;

            bool valorLayout = false;
            string titulo = "Volumen de Embalse";
            if (mtopcodi > 0)
            {
                valorLayout = true;
                var objEscenario = pmpoServicio.GetByIdMpTopologia(mtopcodi.Value);
                titulo = titulo + ": " + objEscenario.Mtopnomb + " (Versión " + objEscenario.Mtopversion + ")";
            }

            model.UsarLayoutModulo = valorLayout;
            model.Titulo = titulo;

            return View(model);
        }

        /// <summary>
        /// Obtener datos generales del handson para armar la tabla web
        /// </summary>
        /// <param name="anioSerie"></param>
        /// <param name="mesSerie"></param>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <param name="tipo"></param>
        /// <param name="accion"></param>
        /// <param name="qnbenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerHandsonVolumenEmbalse(int mtopcodi, bool mostrarVolExtranet)
        {
            VolumenEmbalseModel model = new VolumenEmbalseModel();

            try
            {
                base.ValidarSesionJsonResult();

                //esBD = true;
                if (mtopcodi == 0) mostrarVolExtranet = true;
                model.Handson = pmpoServicio.ArmarHandsonVolumenEmbalseMP(mtopcodi, mostrarVolExtranet);
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
        /// Guardar handson
        /// </summary>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarHandsonVolumenEmbalse(int mtopcodi, string stringJson)
        {
            VolumenEmbalseModel model = new VolumenEmbalseModel();

            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<PmpoVolumenEmbalse> listaVolumen = stringJson != null ? serializer.Deserialize<List<PmpoVolumenEmbalse>>(stringJson) : new List<PmpoVolumenEmbalse>();

                listaVolumen = pmpoServicio. ActualizarVolumenInicialTotalHandson(mtopcodi, listaVolumen);

                pmpoServicio.GuardarVolumenTotalInicial(mtopcodi, listaVolumen);
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

    }
}