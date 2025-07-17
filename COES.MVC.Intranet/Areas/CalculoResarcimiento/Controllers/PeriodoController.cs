using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
    public class PeriodoController : BaseController
    {
        /// <summary>
        /// Instancia de clase de servicios
        /// </summary>
        CalculoResarcimientoAppServicio servicio = new CalculoResarcimientoAppServicio();
        CalidadProductoAppServicio servicioCalidad = new CalidadProductoAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(PuntoEntregaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Muestra la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PeriodoModel model = new PeriodoModel();
            model.Anio = DateTime.Now.Year;
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return View(model);
        }

        /// <summary>
        /// Permite realizar la consulta de periodos
        /// </summary>
        /// <param name="anioDesde"></param>
        /// <param name="anioHasta"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(int anioDesde, int anioHasta, string estado)
        {
            PeriodoModel model = new PeriodoModel();
            model.Listado = this.servicio.GetByCriteriaRePeriodos(anioDesde, anioHasta, estado);
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el formulario de edicion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Editar(int id)
        {
            PeriodoModel model = new PeriodoModel();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            if (id == 0)
            {
                model.Entidad = new RePeriodoDTO();
                model.Entidad.Reperanio = DateTime.Now.Year;
                model.ListaPadre = this.servicio.ObtenerPeriodosPadre(DateTime.Now.Year);                
            }
            else
            {
                model.Entidad = this.servicio.GetByIdRePeriodo(id);
                model.Entidad.FechaInicio = ((DateTime)(model.Entidad.Reperfecinicio)).ToString(Constantes.FormatoFecha);
                model.Entidad.FechaFin = ((DateTime)(model.Entidad.Reperfecfin)).ToString(Constantes.FormatoFecha);
                model.ListaPadre = this.servicio.ObtenerPeriodosPadre((int)model.Entidad.Reperanio);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener los periodos padres
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPadres(int anio)
        {
            return Json(this.servicio.ObtenerPeriodosPadre(anio));
        }

        /// <summary>
        /// Permite obtener la data de las etapas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Etapas(int id)
        {
            return Json(this.servicio.ObtenerEtapasPorPeriodo(id));
        }

        /// <summary>
        /// Permite grabar el listado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(PeriodoModel model)
        {
            RePeriodoDTO periodo = new RePeriodoDTO
            {
                Reperanio = model.Anio,
                Repertipo = model.TipoPeriodo,
                Reperrevision = model.PeriodoRevision,
                Reperpadre = model.PeriodoPadre,
                Repernombre = model.Nombre,
                Reperfecinicio = DateTime.ParseExact(model.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                Reperfecfin = DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                Reperorden = model.OrdenAnual,
                Repertcambio = model.TipoCambio,
                Reperfactorcomp = model.FactorCompensacion,
                Reperestado = model.Estado,
                Repercodi = model.Codigo,
                Data = model.DataEtapa,
                Reperusucreacion = base.UserName
            };

            return Json(this.servicio.GrabarPeriodo(periodo));
        }

        /// <summary>
        /// Permite eliminar un periodo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            return Json(this.servicio.DeleteRePeriodo(id));
        }

        #region Habilitacion carga Interrupciones

        /// <summary>
        /// Muestra pantalla principal de habilitacion de interrupciones
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult HabilitacionCarga(int id)
        {
            PeriodoModel model = new PeriodoModel();
            RePeriodoDTO periodo = this.servicio.GetByIdRePeriodo(id);

            model.ListaSuministradores = this.servicioCalidad.ObtenerListadoEmpresasHabilitacion(id);
            model.Codigo = id;
            model.Nombre = periodo.Repernombre.Trim();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Guarda Habilitacion de interrupciones
        /// </summary>
        /// <param name="repercodi"></param>
        /// <param name="lstIS"></param>
        /// <param name="lstRC"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult guardarHabilitacion(int repercodi, string lstIS, string lstRC)
        {
            CalidadProductoModel model = new CalidadProductoModel();

            try
            {
                base.ValidarSesionJsonResult();               

                servicioCalidad.guardarHabilitacion(repercodi, lstIS, lstRC, base.UserName);
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

        #region Copiar desde el trimestral

        /// <summary>
        /// Permite mostrar los periodos trimestrales
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPeriodosTrimestrales(int idPeriodo)
        {
            return Json(this.servicio.ObtenerPeriodosTrimestrales(idPeriodo));
        }

        /// <summary>
        /// Permite copiar las interrupciones desdel el trimestral
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idPeriodoTrimestral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarInterrupciones(int idPeriodo, int idPeriodoTrimestral)
        {
            return Json(this.servicio.CopiarInterrupciones(idPeriodo, idPeriodoTrimestral, base.UserName));
        }

        #endregion

    }
}
