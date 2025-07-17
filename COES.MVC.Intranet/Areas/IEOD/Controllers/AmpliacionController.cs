using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.StockCombustibles;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class AmpliacionController : Controller
    {
        //
        // GET: /IEOD/AmpliacionController/

        PR5ReportesAppServicio logic = new PR5ReportesAppServicio();
        GeneralAppServicio logicGeneral = new GeneralAppServicio();
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();


        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(AmpliacionController));
        private static string NameController = "AmpliacionController";
        private static List<EstadoModel> ListaEstadoSistemaA = new List<EstadoModel>();

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

        public AmpliacionController()
        {
            ListaEstadoSistemaA = new List<EstadoModel>();
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "0", EstadoDescripcion = "NO" });
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "SÍ" });
        }

        /// <summary>
        /// Index de inicio de controller Ampliacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? app)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            model.ListaFormato = servFormato.CargarFormatosAmpliacion(app ?? 0);

            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

            List<int> listaFormatCodi = new List<int>();
            List<int> listaFormatPeriodo = new List<int>();
            foreach (var reg in model.ListaFormato)
            {
                listaFormatCodi.Add(reg.Formatcodi);
                listaFormatPeriodo.Add((int)reg.Formatperiodo);
            }
            model.Anho = DateTime.Now.Year.ToString();
            model.StrFormatCodi = String.Join(",", listaFormatCodi);
            model.StrFormatPeriodo = String.Join(",", listaFormatPeriodo);

            model.CodigoApp = app ?? 0;

            return View(model);
        }

        /// <summary>
        /// Obtiene la lista de todas las ampliaciones de plazo.
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string sEmpresa, string fecha, string sFormato)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            DateTime fechaIni = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (fecha != null)
            {
                fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFin = fechaIni.AddDays(1);

            }

            var lista = logic.ObtenerListaMultipleMeAmpliacionfechas(fechaIni, fechaFin, sEmpresa, sFormato);
            model.ListaAmpliacion = lista;
            return PartialView(model);
        }

        /// <summary>
        /// Obtiene las empresas segun formato
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarEmpresas(int idFormato)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(idFormato);
            return PartialView(model);
        }

        public PartialViewResult CargarEmpresas2(int idFormato)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(idFormato);
            return PartialView(model);
        }

        /// <summary>
        /// Obtiene el model para pintar el popup de ingreso de la nueva ampliacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AgregarAmpliacion(int? app)
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            model.FechaPlazo = DateTime.Now.ToString(Constantes.FormatoFecha);

            model.ListaFormato = servFormato.CargarFormatosAmpliacion(app ?? 0);

            //
            DateTime fechaActual = DateTime.Now.AddDays(1);
            model.Fecha = fechaActual.ToString(Constantes.FormatoFecha);

            //semanas
            Tuple<int, int> tupla = EPDate.f_numerosemana_y_anho(fechaActual.AddDays(6));
            model.NroSemana = tupla.Item1;

            List<GenericoDTO> entitys = new List<GenericoDTO>();

            int nsemanas = EPDate.TotalSemanasEnAnho(tupla.Item2, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + tupla.Item2;
                reg.String2 = i == tupla.Item1 ? "selected" : "";
                entitys.Add(reg);

            }
            model.Anho = tupla.Item2.ToString();
            model.ListaSemanas2 = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanas(string idAnho)
        {
            FormatoModel model = new FormatoModel();
            List<GenericoDTO> entitys = new List<GenericoDTO>();
            if (idAnho == "0")
            {
                idAnho = DateTime.Now.Year.ToString();
            }
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = EPDate.TotalSemanasEnAnho(Int32.Parse(idAnho), FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemanas2 = entitys;
            return PartialView(model);
        }

        /// <summary>
        /// Graba la Ampliacion ingresada.
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarValidacion(int anio, int? nroSemana, string fecha, int hora, int empresa, int idFormato)
        {
            int resultado = 1;
            
            MeFormatoDTO formato = servFormato.GetByIdMeFormato(idFormato);
            DateTime fechaPeriodo = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, "", anio.ToString() + (nroSemana ?? 0), fecha, Constantes.FormatoFecha);

            MeAmpliacionfechaDTO ampliacion = new MeAmpliacionfechaDTO();
            ampliacion.Lastuser = User.Identity.Name;
            ampliacion.Lastdate = DateTime.Now;
            ampliacion.Amplifecha = fechaPeriodo;
            ampliacion.Formatcodi = idFormato;
            ampliacion.Emprcodi = empresa;
            ampliacion.Amplifechaplazo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMinutes(hora * 30);
            if (hora == 48)
                ampliacion.Amplifechaplazo = ampliacion.Amplifechaplazo.AddSeconds(-1);
            try
            {
                var reg = logic.GetByIdMeAmpliacionfecha(fechaPeriodo, empresa, idFormato);
                if (reg == null)
                {
                    ampliacion.Formatcodi = idFormato;
                    logic.SaveMeAmpliacionfecha(ampliacion);
                }
                else
                {
                    logic.UpdateMeAmpliacionfecha(ampliacion);
                }

            }
            catch
            {
                resultado = 0;
            }
            return Json(resultado);
        }


    }
}
