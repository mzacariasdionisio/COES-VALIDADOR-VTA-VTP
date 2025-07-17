using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Intervenciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using DevExpress.XtraRichEdit;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Intervenciones.Controllers
{
    public class BandejaController : BaseController
    {

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        IntervencionesAppServicio servicio = new IntervencionesAppServicio();

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

        /// <summary>
        /// Permite mostrar la pantalla de configuración de notificaciones
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            BandejaModel model = new BandejaModel();
            model.ListaTiposProgramacion = servicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta).Where(x => x.Evenclasecodi != 1).ToList();
            model.FechaInicio = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Combo en cascada Programaciones x Tipo Programacion
        /// </summary>
        [HttpPost]
        public JsonResult ListarProgramaciones(int idTipoProgramacion)
        {
            Programacion model = new Programacion();

            model.IdTipoProgramacion = idTipoProgramacion;
            model.ListaProgramaciones = servicio.ListarProgramacionesRegistro(idTipoProgramacion);
            model.Entidad = servicio.GetProgramacionDefecto(idTipoProgramacion, model.ListaProgramaciones, DateTime.Today, false);

            return Json(model);
        }

        /// <summary>
        /// Permite  obtener las fechas de programacion de acuerdo al tipo de programacion y programacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerFechaProgramacion(int progCodi)
        {
            Intervencion model = new Intervencion();
            model.IdProgramacion = progCodi;

            if (progCodi > 0)
            {
                var regProg = servicio.ObtenerProgramacionesPorIdSinPlazo(progCodi);

                model.ProgrfechainiDesc = regProg.ProgrfechainiDesc;
                model.ProgrfechafinDesc = regProg.ProgrfechafinDesc;
                model.Progrfechaini = regProg.Progrfechaini.ToString(ConstantesAppServicio.FormatoFecha);
                model.ProgrfechainiH = regProg.Progrfechaini.ToString("HH");
                model.ProgrfechainiM = regProg.Progrfechaini.ToString("mm");
                model.Progrfechafin = regProg.Progrfechafin.ToString(ConstantesAppServicio.FormatoFecha);
                model.ProgrfechafinH = regProg.Progrfechafin.ToString("HH");
                model.ProgrfechafinM = regProg.Progrfechafin.ToString("mm");
            }

            return Json(model);
        }

        /// <summary>
        /// Carga el formulario principal de operaciones con intervenciones al haber seleccionado la programación.
        /// </summary>        
        [HttpPost]
        public PartialViewResult Mensajes(int tipoProgramacion, int indicadorFecha, int programa, string fechaInicio, string fechaFin)
        {
            BandejaModel model = new BandejaModel();
            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListaMensajes = servicio.ObtenerMensajeBandeja(tipoProgramacion, indicadorFecha, programa, fecInicio, fecFin);

            return PartialView(model);
        }

        /// <summary>
        /// Permite visualizar el detalle de mensajes
        /// </summary>
        /// <param name="intercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Detalle(int intercodi, int msgcodi, int indUpdate = 0)
        {
            BandejaModel model = new BandejaModel();

            try
            {
                base.ValidarSesionJsonResult();
                InIntervencionDTO intervencion = servicio.GetByIdInIntervencion(intercodi);
                List<int> listaEmprcodiLectura = new List<int>() { 1 }; //verificar la lectura de COES
                model.ListaMensajes = servicio.ListSiMensajesXIntervencion(intercodi, ConstantesIntervencionesAppServicio.AmbienteIntranet,
                                                            listaEmprcodiLectura, (-1).ToString(), (-1).ToString());
                model.Progcodi = intervencion.Progrcodi;

                if (indUpdate == 1)
                {
                    int emprcodiLectura = 1;
                    servicio.MarcarMensajeComoLeido(intercodi, msgcodi, ConstantesIntervencionesAppServicio.AmbienteIntranet, emprcodiLectura, base.UserName, base.UserEmail, -1);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.ListaMensajes = new List<SiMensajeDTO>();
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el PDF de los mensajes de una intervencion
        /// </summary>
        /// <param name="intercodi"></param>
        /// <returns></returns>
        public ActionResult GenerarPDF(int intercodi)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
            string html = servicio.GenerarPDFMensajes(ConstantesIntervencionesAppServicio.AmbienteIntranet, intercodi, new List<int>() { 1 }, out string filename);

            UtilDevExpressIntervenciones.GenerarPDFdeHtml(html, path, filename);

            return base.DescargarArchivoTemporalYEliminarlo(path, filename);

        }

        /// <summary>
        /// Descargar mensajes y sus adjuntos de la intervención
        /// </summary>
        /// <param name="intercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarMensajes(string intercodi)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();
                List<int> listaIntercodiChecked = new List<int>();
                if (!string.IsNullOrEmpty(intercodi)) listaIntercodiChecked = intercodi.Split(';').Select(x => Int32.Parse(x)).Distinct().ToList();

                //Validar que las intervenciones seleccionadas tengan mensajes
                var intervencion = servicio.GetByIdInIntervencion(Int32.Parse(intercodi)); // obtener intervención
                //listaIntercodiChecked = servicio.ObtenerIntercodisConMensajes(listaIntercodiChecked, intervencion.Progrcodi);
                //if (listaIntercodiChecked.Count < 1)
                //    throw new ArgumentException("La intervención no tiene mensajes");

                InProgramacionDTO objProgramacion = servicio.ObtenerProgramacionesPorId(intervencion.Progrcodi);

                int numeroAleatorio = (int)DateTime.Now.Ticks;
                servicio.DescargarZipMensajesMasivos(numeroAleatorio.ToString(), listaIntercodiChecked, objProgramacion.CarpetaProgDefault, out string nameFile);
                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }
    }
}