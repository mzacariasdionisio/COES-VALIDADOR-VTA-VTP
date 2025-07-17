using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Medidores.Helpers;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class ValidacionRegistroController : BaseController
    {
        ValidacionReporteAppServicio servicio = new ValidacionReporteAppServicio();

        #region Declaración de variables
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
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

        public ValidacionRegistroController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        #region Variables de sesión

        /// <summary>
        /// Almacena el resultado del reporte
        /// </summary>
        public List<ReporteValidacionMedidor> ListaValores
        {
            get
            {
                return (Session[DatosSesion.ListaValidacionMedidores] != null) ?
                    (List<ReporteValidacionMedidor>)Session[DatosSesion.ListaValidacionMedidores] : new List<ReporteValidacionMedidor>();
            }
            set { Session[DatosSesion.ListaValidacionMedidores] = value; }
        }

        /// <summary>
        /// Almacena el resultado de la FechaHoraMD48
        /// </summary>
        public string FechaHoraMD48
        {
            get
            {
                return (Session[DatosSesion.FechaHoraMD48] != null) ?
                    (string)Session[DatosSesion.FechaHoraMD48] : string.Empty;
            }
            set { Session[DatosSesion.FechaHoraMD48] = value; }
        }
        /// <summary>
        /// Almacena el resultado de la FechaHoraMD48
        /// </summary>
        public string FechaHoraMD96
        {
            get
            {
                return (Session[DatosSesion.FechaHoraMD96] != null) ?
                    (string)Session[DatosSesion.FechaHoraMD96] : string.Empty;
            }
            set { Session[DatosSesion.FechaHoraMD96] = value; }
        }

        #endregion

        /// <summary>
        /// Muestra la página inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ValidacionMedidoresModel model = new ValidacionMedidoresModel();

            model.ListaTipoEmpresas = this.servicio.ListaTipoEmpresas();
            model.ListaTipoGeneracion = this.servicio.ListaTipoGeneracion();
            model.ListaFuenteEnergia = this.servicio.ListaFuenteEnergia(0);
            model.FechaInicio = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            int idOpcion = 0;

            if (Session[DatosSesion.SesionIdOpcion] != null)
            {
                idOpcion = Convert.ToInt32(Session[DatosSesion.SesionIdOpcion]);
            }

            bool flag = Validaciones.ValidarAcceso(idOpcion, Acciones.Grabar, User.Identity.Name);
            if (flag) model.IndConfiguracion = Constantes.SI;
            else model.IndConfiguracion = Constantes.NO;

            return View(model);
        }

        /// <summary>
        /// Permite cargar las empresas por los tipos seleccionados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresas(string tiposEmpresa)
        {
            ValidacionMedidoresModel model = new ValidacionMedidoresModel();
            List<SiEmpresaDTO> entitys = this.servicio.ObteneEmpresasPorTipo(tiposEmpresa);
            model.ListaEmpresas = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Permite cargar las empresas por los tipos seleccionados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresas(string tiposEmpresa)
        {
            List<SiEmpresaDTO> entitys = this.servicio.ObteneEmpresasPorTipo(tiposEmpresa);
            SelectList list = new SelectList(entitys, EntidadPropiedad.Emprcodi, EntidadPropiedad.Emprnomb);
            return Json(list);
        }

        /// <summary>
        /// Filtra el reporte de validación
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Filtro(int id)
        {
            ValidacionMedidoresModel model = new ValidacionMedidoresModel();

            if (this.ListaValores != null)
            {
                List<ReporteValidacionMedidor> list = this.ListaValores;
                if (id > 0)
                    list = this.ListaValores.Where(x => x.Filtro == id).ToList();

                model.ListaReporte = list;
                model.FechaMDDespacho = this.FechaHoraMD48;
                model.FechaMDMedidor = this.FechaHoraMD96;
                model.TotalDespacho = list.Sum(x => x.ValorDespacho);
                model.TotalMedidor = list.Sum(x => x.ValorMedidor);
                model.TotalMDDespacho = list.Sum(x => x.MDDespacho);
                model.TotalMDMedidor = list.Sum(x => x.MDMedidor);
            }

            return PartialView("Reporte", model);
        }

        /// <summary>
        /// Permite mostrar el reporte de medidores resumen
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="fuentesEnergia"></param>
        /// <param name="central"></param>
        /// <param name="indAdjudicado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Reporte(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas,
            string tiposGeneracion, string fuentesEnergia, int central)
        {
            ValidacionMedidoresModel model = new ValidacionMedidoresModel();

            try
            {
                DateTime fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(fuentesEnergia)) fuentesEnergia = Constantes.ParametroDefecto;

                model.FechaInicio = fecInicio.ToString(Constantes.FormatoFecha);
                model.FechaFin = fecFin.ToString(Constantes.FormatoFecha);
                model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);

                ItemReporteMedicionDTO datosMDDespacho = null;
                ItemReporteMedicionDTO datosMDMedidor = null;

                List<ReporteValidacionMedidor> list = this.servicio.ObtenerReporteValidacion(fecInicio, fecFin, tiposEmpresa, empresas,
                    tiposGeneracion, fuentesEnergia, central, out datosMDDespacho, out datosMDMedidor, out string msjValidacion).OrderBy(x => x.DesEmpresa).ToList();

                model.TotalDespacho = list.Sum(x => x.ValorDespacho);
                model.TotalMedidor = list.Sum(x => x.ValorMedidor);
                model.TotalMDDespacho = list.Sum(x => x.MDDespacho);
                model.TotalMDMedidor = list.Sum(x => x.MDMedidor);
                model.MensajeValidacion = msjValidacion;

                if (datosMDDespacho != null)
                    model.FechaMDDespacho = datosMDDespacho.FechaMaximaDemanda.ToString(Constantes.FormatoFecha) + " (" + datosMDDespacho.HoraMaximaDemanda + ")";

                if (datosMDMedidor != null)
                    model.FechaMDMedidor = datosMDMedidor.FechaMaximaDemanda.ToString(Constantes.FormatoFecha) + " (" + datosMDMedidor.HoraMaximaDemanda + ")";

                model.ListaReporte = list;

                this.ListaValores = list;
                this.FechaHoraMD48 = model.FechaMDDespacho;
                this.FechaHoraMD96 = model.FechaMDMedidor;
                ViewBag.Mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                ViewBag.Mensaje = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas,
            string tiposGeneracion, string fuentesEnergia, int central, int id)
        {
            try
            {
                DateTime fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(fuentesEnergia)) fuentesEnergia = Constantes.ParametroDefecto;

                ItemReporteMedicionDTO datosMDDespacho = new ItemReporteMedicionDTO();
                ItemReporteMedicionDTO datosMDMedidor = new ItemReporteMedicionDTO();

                List<ReporteValidacionMedidor> list = this.servicio.ObtenerReporteValidacion(fecInicio, fecFin, tiposEmpresa, empresas,
                    tiposGeneracion, fuentesEnergia, central, out datosMDDespacho, out datosMDMedidor, out string msjValidacion).OrderBy(x => x.DesEmpresa).ToList();

                if (id > 0)
                    list = list.Where(x => x.Filtro == id).ToList();

                MedidorHelper.GenerarReporteValidacionMedidores(list, fechaInicial, fechaFinal, datosMDDespacho, datosMDMedidor);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteValidacionMedidores;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteValidacionMedidores);
        }


        #region Graficos de Comparación

        public ActionResult Grafico()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ValidacionMedidoresModel model = new ValidacionMedidoresModel();
            List<WbMedidoresValidacionDTO> list = this.servicio.ObtenerConfiguracionRelacion(-1);
            model.Listado = list.Select(x => x.Empresamed).Distinct().ToList();

            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoMesAnio);      

            return View(model);
        }

        [HttpPost]
        public JsonResult ObtenerCentrales(string empresa)
        {
            ValidacionMedidoresModel model = new ValidacionMedidoresModel();
            List<WbMedidoresValidacionDTO> list = this.servicio.ObtenerConfiguracionRelacion(-1);
            model.Listado = list.Where(x => x.Empresamed == empresa || string.IsNullOrEmpty(empresa)).Select(x=>x.Centralmed).Distinct().ToList();
            return Json(model.Listado);
        }

        [HttpPost]
        public JsonResult ObtenerDatosGrafico(string empresa, string central, string mes)
        {
            List<ComparativoItemModel> result = this.ProcesarDatos(empresa, central, mes);
            return Json(result);
        }

        [HttpPost]
        public JsonResult ObtenerDatosGraficoMasivo(string empresa, string mes)
        {
            List<ComparativoMasivoModel> result = new List<ComparativoMasivoModel>();
            List<WbMedidoresValidacionDTO> list = this.servicio.ObtenerConfiguracionRelacion(-1);
            List<string> centrales = list.Where(x => x.Empresamed == empresa || string.IsNullOrEmpty(empresa)).Select(x => x.Centralmed).Distinct().ToList();

            foreach (string central in centrales)
            {
                List<ComparativoItemModel> subList = this.ProcesarDatos(empresa, central, mes);
                result.Add(new ComparativoMasivoModel { Central = central, ListaGrafico = subList });
            }

            return Json(result);
        }


        /// <summary>
        /// Permite obtener los datos del reporte
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        private List<ComparativoItemModel> ProcesarDatos(string empresa, string central, string mes)
        {
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, string.Empty, string.Empty, string.Empty);
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);

            List<WbMedidoresValidacionDTO> list = this.servicio.ObtenerConfiguracionRelacion(-1).Where(x => (x.Empresamed == empresa || string.IsNullOrEmpty(empresa))
            && (x.Centralmed == central || string.IsNullOrEmpty(central))).ToList();

            List<int> codigosMedicion = list.Select(x => (int)x.Ptomedicodimed).Distinct().ToList();
            List<int> codigosDespacho = list.Select(x => (int)x.Ptomedicodidesp).Distinct().ToList();

            string medicion = string.Join<int>(Constantes.CaracterComa.ToString(), codigosMedicion);
            string despacho = string.Join<int>(Constantes.CaracterComa.ToString(), codigosDespacho);

            List<MeMedicion96DTO> valoresMediciones = this.servicio.ObtenerDatosMedicionComparativo(fechaIni, fechaFin, medicion);
            List<MeMedicion48DTO> valoresDespacho = this.servicio.ObtenerDatosDespachoComparativo(fechaIni, fechaFin, despacho);

            //- Validaciones de datos
            List<ComparativoItemModel> result = new List<ComparativoItemModel>();

            if (valoresMediciones.Count == valoresDespacho.Count)
            {
                int index = 0;
                foreach (MeMedicion96DTO item in valoresMediciones)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        ComparativoItemModel itemModel = new ComparativoItemModel();
                        itemModel.Hora = ((DateTime)item.Medifecha).AddMinutes(i * 30).ToString(Constantes.FormatoFechaHora);

                        object medidorItem = item.GetType().GetProperty("H" + i * 2).GetValue(item, null);
                        if (medidorItem != null) itemModel.ValorMedidor = Convert.ToDecimal(medidorItem);
                        object despachoItem = valoresDespacho[index].GetType().GetProperty("H" + i).GetValue(valoresDespacho[index], null);
                        if (despachoItem != null) itemModel.ValorDespacho = Convert.ToDecimal(despachoItem);

                        itemModel.Desviacion = (itemModel.ValorDespacho != 0) ? (itemModel.ValorMedidor - itemModel.ValorDespacho) / itemModel.ValorDespacho : 0;

                        result.Add(itemModel);
                    }

                    index++;
                }
            }
            else
            {

                int indexMediciones = 0;
                int indexDespachos = 0;
                bool esMismaFecha = false;
                foreach (MeMedicion96DTO item in valoresMediciones)
                {
                    if (valoresDespacho.Count > 0  && valoresDespacho.Count <= indexDespachos)
                        indexDespachos = valoresDespacho.Count - 1;

                    if (valoresDespacho.Count > 0 && item.Medifecha == valoresDespacho[indexDespachos].Medifecha)
                        esMismaFecha = true;
                    else
                        esMismaFecha = false;

                    for (int i = 1; i <= 48; i++)
                    {
                        ComparativoItemModel itemModel = new ComparativoItemModel();
                        itemModel.Hora = ((DateTime)item.Medifecha).AddMinutes(i * 30).ToString(Constantes.FormatoFechaHora);

                        if (esMismaFecha)
                        {
                            object medidorItem = item.GetType().GetProperty("H" + i * 2).GetValue(item, null);
                            if (medidorItem != null) itemModel.ValorMedidor = Convert.ToDecimal(medidorItem);
                            object despachoItem = valoresDespacho[indexDespachos].GetType().GetProperty("H" + i).GetValue(valoresDespacho[indexDespachos], null);
                            if (despachoItem != null) itemModel.ValorDespacho = Convert.ToDecimal(despachoItem);

                            itemModel.Desviacion = (itemModel.ValorDespacho != 0) ? (itemModel.ValorMedidor - itemModel.ValorDespacho) / itemModel.ValorDespacho : 0;                            
                        }
                        else
                        {
                            object medidorItem = item.GetType().GetProperty("H" + i * 2).GetValue(item, null);
                            if (medidorItem != null) itemModel.ValorMedidor = Convert.ToDecimal(medidorItem);
                            itemModel.ValorDespacho = 0;

                            itemModel.Desviacion = (itemModel.ValorDespacho != 0) ? (itemModel.ValorMedidor - itemModel.ValorDespacho) / itemModel.ValorDespacho : 0;
                        }

                        result.Add(itemModel);
                    }

                    indexDespachos = esMismaFecha ? indexDespachos + 1: indexDespachos;

                }
            }

            return result;
        }


        #endregion
    }
}
