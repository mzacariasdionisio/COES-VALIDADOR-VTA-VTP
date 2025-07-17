using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Migraciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.CPPA.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Migraciones.Controllers
{
    public class ReporteController : BaseController
    {
        //
        // GET: /Migraciones/Reporte/
        MigracionesAppServicio servicio = new MigracionesAppServicio();
        GeneralAppServicio _appGeneral = new GeneralAppServicio();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ReporteController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Protected de log de errores page
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
        /// //Descarga el archivo excel exportado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["fi"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }

        #region Generales

        /// <summary>
        /// Listar empresas por tipo y check
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CargarEmpresas(int idTipoEmpresa, int tip)
        {
            var listaTipoEmpresa = this.servicio.ListarEmpresasDemandaBarrra(idTipoEmpresa);
            if (tip == 1) { listaTipoEmpresa = listaTipoEmpresa.Where(x => x.Inddemanda == "S").ToList(); }
            var selectList = new SelectList(listaTipoEmpresa, "Emprcodi", "Emprnomb");
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Reporte Producción CCO

        public ActionResult Produccioncco()
        {
            MigracionesModel model = new MigracionesModel();

            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            model.ListaTipoInfo = servicio.GetListaTipoInfo(ConstantesAppServicio.TipoinfocodiMW + "," + ConstantesAppServicio.TipoinfocodiMVAR);

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        public JsonResult CargarProduccioncco(string fecha, int tipoinfocodi, int xx)
        {
            MigracionesModel model = new MigracionesModel();
            DateTime fecIni = DateTime.MinValue;
            if (fecha != null)
            {
                fecIni = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            }
            var lista = servicio.GetListaProduccioncco(fecIni, ConstantesAppServicio.LectcodiEjecutado, tipoinfocodi);
            if (lista.Count == 0) { lista = servicio.GetListaProduccioncco(fecIni, ConstantesAppServicio.LectcodiEjecutadoHisto, tipoinfocodi); }

            if (xx == 1)
            {
                model.Resultado = servicio.CargarProduccionccoHtml(fecIni, lista);
                model.nRegistros = lista.Count();
            }
            else
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = ConstantesMigraciones.RptProdcco + "_" + fecIni.ToString("yyyyMMdd") + (xx == 2 ? ConstantesAppServicio.ExtensionExcel : ConstantesAppServicio.ExtensionCsv);
                this.servicio.GenerarArchivoExcelProduccioncco(lista, ruta + nameFile);
                model.Resultado = nameFile;
                model.nRegistros = 1;
            }

            return Json(model);
        }

        public ActionResult ProgramacionDiaria()
        {
            MigracionesModel model = new MigracionesModel();

            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult CargarProgramacionDiaria(string fecha)
        {
            MigracionesModel model = new MigracionesModel();
            DateTime fecIni = DateTime.MinValue;
            if (fecha != null)
            {
                fecIni = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            }
            var lista = servicio.GetListaProduccioncco(fecIni, ConstantesAppServicio.LectcodiProgDiario, 0);
            if (lista.Count == 0) { lista = servicio.GetListaProduccioncco(fecIni, ConstantesAppServicio.LectcodiEjecutadoHisto, 0); }

            model.Resultado = servicio.CargarProduccionccoHtml(fecIni, lista);
            model.nRegistros = lista.Count();

            return Json(model);
        }

        #endregion

        #region Reporte Demanda por area - IEOD

        public ActionResult RptDemandaxArea()
        {
            MigracionesModel model = new MigracionesModel();

            List<UserModel> list_ = new List<UserModel>();
            list_.Add(new UserModel() { IdArea = 1, Roles = "Principal" });
            list_.Add(new UserModel() { IdArea = 2, Roles = "Diario" });
            list_.Add(new UserModel() { IdArea = 3, Roles = "Semanal" });
            list_.Add(new UserModel() { IdArea = 4, Roles = "Mensual" });
            list_.Add(new UserModel() { IdArea = 5, Roles = "Anual" });
            model.ListaSelect = list_;

            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            model.Mesanio = DateTime.Now.ToString(ConstantesAppServicio.FormatoMesanio);
            model.Anio = DateTime.Now.ToString(ConstantesAppServicio.FormatoAnio);
            model.IdReporte = ConstantesMigraciones.IdReporteDemandaAreaPrincipal;

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public JsonResult CargarSemanas(string anio)
        {
            MigracionesModel model = new MigracionesModel();

            List<TipoInformacion> entitys = new List<TipoInformacion>();
            DateTime dfecha = new DateTime(Int32.Parse(anio), 12, 31);
            int nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                TipoInformacion reg = new TipoInformacion();
                reg.IdTipoInfo = i;
                reg.NombreTipoInfo = "Sem" + i + "-" + anio;
                entitys.Add(reg);

            }
            model.ListaSemanas = entitys;

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fec1"></param>
        /// <param name="fec2"></param>
        /// <param name="anio"></param>
        /// <param name="sem1"></param>
        /// <param name="sem2"></param>
        /// <param name="mes1"></param>
        /// <param name="mes2"></param>
        /// <param name="anio1"></param>
        /// <param name="anio2"></param>
        /// <returns></returns>
        public JsonResult CargarInformacionDemanda(int tipo, string fec1, string fec2, string anio, string sem1, string sem2,
                                                string mes1, string mes2, string anio1, string anio2, int xx, int yy)
        {
            MigracionesModel model = new MigracionesModel();

            DateTime f_1 = DateTime.MinValue, f_2 = DateTime.MinValue;

            #region seteo de fecha

            switch (tipo)
            {
                case 1:
                case 2:
                    f_1 = DateTime.ParseExact(fec1, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    f_2 = DateTime.ParseExact(fec2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture); break;
                case 3:
                    f_1 = EPDate.f_fechainiciosemana(int.Parse(anio), int.Parse(sem1));
                    f_2 = EPDate.f_fechafinsemana(int.Parse(anio), int.Parse(sem2)); break;
                case 4:
                    f_1 = DateTime.ParseExact("01/" + mes1, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    f_2 = DateTime.ParseExact("01/" + mes2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    f_2 = f_2.AddMonths(1).AddDays(-1); break;
                case 5:
                    f_1 = DateTime.ParseExact("01/01/" + anio1, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    f_2 = DateTime.ParseExact("01/01/" + anio2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    f_2 = f_2.AddYears(1).AddDays(-1); break;
            }

            #endregion

            List<MeMedicion48DTO> listaPrincipal = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaXHorizonte = new List<MeMedicion48DTO>();
            if (tipo == 1)
            {
                listaPrincipal = servicio.ListarDataReporteDemandaXAreaXPrincipal(f_1, f_2);
            }
            else
            {
                listaXHorizonte = servicio.ListarDataReporteDemandaXAreaXHorizonte(tipo, f_1, f_2, anio, sem1, sem2);
            }

            switch (xx)
            {
                case 1:
                    #region disenio de reportes html

                    switch (tipo)
                    {
                        case 1: model.Resultado = servicio.InformacionDemandaHtml(listaPrincipal); break;
                        case 2: model.Resultado = servicio.InformacionDemandaDiaHtml(listaXHorizonte); break;
                        case 3: model.Resultado = servicio.InformacionDemandaSemMesAnioHtml(listaXHorizonte, 1); break;
                        case 4: model.Resultado = servicio.InformacionDemandaSemMesAnioHtml(listaXHorizonte, 2); break;
                        case 5: model.Resultado = servicio.InformacionDemandaSemMesAnioHtml(listaXHorizonte, 3); break;
                    }

                    #endregion
                    model.nRegistros = 1; //si el valor de nRegistros es mayor a 0 entonces muestra la tabla web. Todas las tablas tienen al menos un registro.
                    break;
                case 2:
                    #region disenio de reportes excel

                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    string nameFile = ConstantesMigraciones.RptDemArea + "_";

                    switch (tipo)
                    {
                        case 1:
                            nameFile += f_1.ToString("yyyyMMdd") + "_" + f_2.ToString("yyyyMMdd") + ConstantesAppServicio.ExtensionExcel;
                            this.servicio.GenerarArchivoExcelDemandaArea(listaPrincipal, ruta + nameFile, fec1, fec2, anio, sem1, sem2, mes1, mes2, anio1, anio2, tipo, yy); break;
                        case 2:
                            nameFile += f_1.ToString("yyyyMMdd") + "_" + f_2.ToString("yyyyMMdd") + ConstantesAppServicio.ExtensionExcel;
                            this.servicio.GenerarArchivoExcelDemandaArea(listaXHorizonte, ruta + nameFile, fec1, fec2, anio, sem1, sem2, mes1, mes2, anio1, anio2, tipo, yy); break;
                        case 3:
                            nameFile += sem1 + "-" + anio + "_" + sem2 + "-" + anio + ConstantesAppServicio.ExtensionExcel;
                            this.servicio.GenerarArchivoExcelDemandaArea(listaXHorizonte, ruta + nameFile, fec1, fec2, anio, sem1, sem2, mes1, mes2, anio1, anio2, tipo, yy); break;
                        case 4:
                            nameFile += mes1.Replace("/", "-") + "_" + mes2.Replace("/", "-") + ConstantesAppServicio.ExtensionExcel;
                            this.servicio.GenerarArchivoExcelDemandaArea(listaXHorizonte, ruta + nameFile, fec1, fec2, anio, sem1, sem2, mes1, mes2, anio1, anio2, tipo, yy); break;
                        case 5:
                            nameFile += anio1 + "_" + anio2 + ConstantesAppServicio.ExtensionExcel;
                            this.servicio.GenerarArchivoExcelDemandaArea(listaXHorizonte, ruta + nameFile, fec1, fec2, anio, sem1, sem2, mes1, mes2, anio1, anio2, tipo, yy); break;
                    }

                    #endregion
                    model.nRegistros = 1; //si el valor de nRegistros es mayor a 0 entonces descarga el archivo excel. Todas los excel tienen al menos un registro.
                    model.Resultado = nameFile;
                    break;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return jsonResult;
        }

        #endregion

        #region Reporte de Costo Marginal Corto Plazo

        public ActionResult RptCmgCortoPlazo()
        {
            MigracionesModel model = new MigracionesModel();
            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fec1"></param>
        /// <param name="fec2"></param>
        /// <param name="anio"></param>
        /// <param name="sem1"></param>
        /// <param name="sem2"></param>
        /// <param name="mes1"></param>
        /// <param name="mes2"></param>
        /// <param name="anio1"></param>
        /// <param name="anio2"></param>
        /// <returns></returns>
        public JsonResult CargarRptCmgCortoPlazo(string fec1, string fec2, int xx)
        {
            MigracionesModel model = new MigracionesModel();

            string lectcodi = ConstantesAppServicio.LectcodiEjecutadoHisto;
            int tipoinfocodi = ConstantesAppServicio.TipoinfocodiSoles;
            int ptomedicodi = ConstantesAppServicio.PtomedicodiCmgCP;
            DateTime f_1 = DateTime.MinValue, f_2 = DateTime.MinValue;

            f_1 = DateTime.ParseExact(fec1, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            f_2 = DateTime.ParseExact(fec2, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            var lista = servicio.GetRptCmgCortoPlazo(lectcodi, tipoinfocodi, ptomedicodi, f_1, f_2);
            if (xx == 1)
            {
                model.Resultado = servicio.RptCmgCortoPlazoHtml(lista);
                model.nRegistros = lista.Count;
            }
            else
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = ConstantesMigraciones.RptCmgCortoPlazo + "_" + f_1.ToString("yyyyMMdd") + "_" + f_2.ToString("yyyyMMdd") + ConstantesAppServicio.ExtensionExcel;
                this.servicio.GenerarArchivoExcelCmgCortoPlazo(lista, f_1, f_2, ruta + nameFile);
                model.Resultado = nameFile;
                model.nRegistros = 1;
            }

            return Json(model);
        }

        #endregion

        #region Reporte de medidores de generacion

        public ActionResult MedidoresGeneracion()
        {
            MigracionesModel model = new MigracionesModel();
            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            List<UserModel> list_ = new List<UserModel>();
            list_.Add(new UserModel() { IdArea = 0, Roles = "TODOS" });
            list_.Add(new UserModel() { IdArea = 1, Roles = "COES" });
            list_.Add(new UserModel() { IdArea = 10, Roles = "NO COES" });
            model.ListaSelect = list_;

            return View(model);
        }

        /// <summary>
        /// Permite mostrar el reporte de medidores resumen
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReporteMedidoresGeneracion(string fechaInicial, string fechaFinal, int central, int xx)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                }

                string tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
                string empresas = ConstantesAppServicio.ParametroDefecto;
                string tiposGeneracion = ConstantesAppServicio.ParametroDefecto;
                string fuentesEnergia = ConstantesAppServicio.ParametroDefecto;

                List<MedicionReporteDTO> listCuadrosFE = new List<MedicionReporteDTO>();
                List<MedicionReporteDTO> listCuadrosTG = new List<MedicionReporteDTO>();
                List<MedicionReporteDTO> listCuadrosUnidades = new List<MedicionReporteDTO>();
                MedicionReporteDTO umbrales = new MedicionReporteDTO();
                List<MedicionReporteDTO> listFuenteEnergia = new List<MedicionReporteDTO>();
                List<MeMedicion96DTO> reporteEmpresas = new List<MeMedicion96DTO>();
                List<MedicionReporteDTO> resultadoTG = new List<MedicionReporteDTO>();

                (new ReporteMedidoresAppServicio()).ObtenerReporteMedidores(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion, fuentesEnergia, central,
                        out listCuadrosFE, out listCuadrosTG, out listCuadrosUnidades, out umbrales, out listFuenteEnergia, out reporteEmpresas, out resultadoTG, out List<LogErrorHOPvsMedidores> listaValidacion);

                if (xx == 1)
                {
                    model.Resultado = servicio.ReporteMedidoresGeneracionHtml(umbrales, listCuadrosUnidades);
                }
                else
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    string nameFile = ConstantesMigraciones.RptDespachoMD + "_" + fecInicio.ToString("yyyyMMdd") + "_" + fecFin.ToString("yyyyMMdd") + ConstantesAppServicio.ExtensionExcel;
                    this.servicio.GenerarArchivoExcelMedidoresGeneracion(umbrales, listCuadrosUnidades, fecInicio, fecFin, ruta + nameFile);
                    model.Resultado = nameFile;
                    model.nRegistros = 1;
                }

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region 	Reporte de Demanda en Barras

        public ActionResult DemandaBarras()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = new MigracionesModel()
            {
                ListaTipoEmpresa = this._appGeneral.ListarTiposEmpresa().OrderBy(x => x.Tipoemprdesc).ToList(),
                Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha)
            };

            return View(model);
        }

        /// <summary>
        /// Exportación a excel de demanda en barras
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDemadaBarras(int idTipoEmpresa, string emprcodi, string lectcodi, string tipoinfocodi, string fechaInicio, string fechaFin)
        {
            if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);

            var model = new MigracionesModel();
            var fecInicio = DateTime.Now;
            var fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicio))
                fecInicio = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(fechaFin))
                fecFin = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            if (fecInicio >= new DateTime(2020, 7, 1))
            {
                if (lectcodi.Trim() == "45" ) lectcodi = "103";
                if (lectcodi.Trim() == "46" ) lectcodi = "110";
                if (lectcodi.Trim() == "47") lectcodi = "102";
            }

            if (!string.IsNullOrEmpty(fechaInicio))
                fecInicio = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(fechaFin))
                fecFin = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            List<MePtomedicionDTO> listaPuntosMedicion = this.servicio.GetListaPuntoMedicionDemandaBarra(emprcodi, fecInicio, fecFin);

            if (listaPuntosMedicion.Count > 0)
            {
                var listaDemandaBarras = servicio.GetListaDemandaBarras(string.Join(",", listaPuntosMedicion.Select(x => x.Ptomedicodi).ToList()), lectcodi, fecInicio, fecFin);

                var ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nombreHoja = this.servicio.NombreArchivoReporteDemandaBarra(idTipoEmpresa, emprcodi, Int32.Parse(lectcodi), fecInicio, fecFin);
                string nameFile = nombreHoja + ConstantesAppServicio.ExtensionExcel;
                this.servicio.GenerarArchivoExcelDemandaBarras(listaPuntosMedicion, listaDemandaBarras, ruta + nameFile, nombreHoja, fecInicio, fecFin);

                model.Resultado = nameFile;
                model.nRegistros = 1;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ListaDemandaBarras(string emprcodi, string lectcodi, string tipoinfocodi, string fecha)
        {
            var model = new MigracionesModel();

            var fechaConsulta = DateTime.Now;

            if (!string.IsNullOrEmpty(fecha))
                fechaConsulta = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            List<MePtomedicionDTO> listaPuntosMedicion = this.servicio.GetListaPuntoMedicionDemandaBarra(emprcodi, fechaConsulta, fechaConsulta);

            if (fechaConsulta >= new DateTime(2020, 7, 1))
            {
                if (lectcodi.Trim() == "45") lectcodi = "103";
                if (lectcodi.Trim() == "46") lectcodi = "110";
                if (lectcodi.Trim() == "47") lectcodi = "102";
            }

            if (listaPuntosMedicion.Count > 0)
            {
                var listaDemandaBarras = servicio.GetListaDemandaBarras(string.Join(",", listaPuntosMedicion.Select(x => x.Ptomedicodi).ToList()), lectcodi, fechaConsulta, fechaConsulta);

                model.Resultado = servicio.ReporteDemadaBarrasHtml(listaPuntosMedicion, listaDemandaBarras);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Paginado de demanda en barras
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="lectcodi"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoDemandaBarras(string emprcodi, string lectcodi, string tipoinfocodi, string fechaInicio, string fechaFin)
        {
            var model = new MigracionesModel { IndicadorPagina = false };

            var fecInicio = DateTime.Now;
            var fecFin = DateTime.Now;

            if (fecInicio >= new DateTime(2020, 7, 1))
            {
                if (lectcodi.Trim() == "45") lectcodi = "103";
                if (lectcodi.Trim() == "46") lectcodi = "110";
                if (lectcodi.Trim() == "47") lectcodi = "102";
            }

            if (!string.IsNullOrEmpty(fechaInicio))
                fecInicio = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(fechaFin))
                fecFin = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            List<MePtomedicionDTO> listaPuntosMedicion = this.servicio.GetListaPuntoMedicionDemandaBarra(emprcodi, fecInicio, fecFin);

            if (listaPuntosMedicion.Count > 0)
            {
                var listaDemandaBarras = servicio.GetListaDemandaBarras(string.Join(",", listaPuntosMedicion.Select(x => x.Ptomedicodi).ToList()), lectcodi, fecInicio, fecFin);
                var listaFechas = listaDemandaBarras.Select(x => x.Medifecha).Distinct().ToList();

                model.IndicadorPagina = true;
                model.NroMostrar = 10;
                model.FechasPaginado = listaFechas;
                model.NroPaginas = listaFechas.Count;
            }

            return PartialView(model);
        }

        #endregion

        #region Reporte Producc y programado

        public ActionResult ProduccyProgramado()
        {
            MigracionesModel model = new MigracionesModel();

            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            string lectcodi = ConstantesAppServicio.LectcodiEjecutadoHisto + "," + ConstantesAppServicio.LectcodiProgDiario + ","
                + ConstantesAppServicio.LectcodiReprogDiario + "," + ConstantesAppServicio.LectcodiProgSemanal + "," + ConstantesAppServicio.LectcodiAjusteDiario;
            model.TipoProgramacion = servicio.GetByCriteriaMeLectura(lectcodi);

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public JsonResult ReporteProduccyProgramado(string fechaInicial, string fechaFinal, int lectcodi, int xx)
        {
            int central = 0;
            MigracionesModel model = new MigracionesModel();

            try
            {
                DateTime fecInicio = DateTime.MinValue, fecFin = DateTime.MinValue;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                }

                string tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
                string empresas = ConstantesAppServicio.ParametroDefecto;
                string tiposGeneracion = ConstantesAppServicio.ParametroDefecto;
                string fuentesEnergia = "1,11,6,7,4,3,2,5,10";//ConstantesAppServicio.ParametroDefecto;

                List<MedicionReporteDTO> listCuadros = new List<MedicionReporteDTO>();
                MedicionReporteDTO umbrales = new MedicionReporteDTO();
                List<MedicionReporteDTO> listFuenteEnergia = new List<MedicionReporteDTO>();
                List<MeMedicion48DTO> reporteEmpresas = new List<MeMedicion48DTO>();
                List<MedicionReporteDTO> resultadoTG = new List<MedicionReporteDTO>();

                (new EjecutadoAppServicio()).ObtenerReporteDespacho(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion, fuentesEnergia, central,
                    out listCuadros, out umbrales, out listFuenteEnergia, out reporteEmpresas, out resultadoTG, lectcodi);

                if (xx == 1)
                {
                    model.Resultado = servicio.ReporteDespachoHtml(umbrales, listCuadros);
                }
                else
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    string nameFile = ConstantesMigraciones.RptDespachoMD + "_" + fecInicio.ToString("yyyyMMdd") + "_" + fecFin.ToString("yyyyMMdd") + ConstantesAppServicio.ExtensionExcel;
                    this.servicio.GenerarArchivoExcelDespacho(umbrales, listCuadros, fecInicio, fecFin, ruta + nameFile);
                    model.Resultado = nameFile;
                    model.nRegistros = 1;
                }

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                ViewBag.Mensaje = ex.Message;
            }

            return Json(model);
        }

        #endregion

        #region Reporte Produccion

        public ActionResult RptProduccion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            MigracionesModel model = new MigracionesModel();
            string lectcodiSicoes = ConstantesAppServicio.LectcodiProgSemanal + "," + ConstantesAppServicio.LectcodiProgDiario + "," + ConstantesAppServicio.LectcodiReprogDiario + "," + ConstantesAppServicio.LectcodiEjecutadoHisto;

            DateTime hoy = DateTime.Now;
            DateTime ayer = hoy.AddDays(-1);
            model.FechaIni = (ayer).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = (ayer).ToString(ConstantesAppServicio.FormatoFecha);
            model.TipoProgramacion = servicio.GetByCriteriaMeLectura(lectcodiSicoes);

            DateTime masSieteDias = hoy.AddDays(7);
            DateTime iniSiguienteSemana = EPDate.f_fechainiciosemana(masSieteDias);
            DateTime finSiguienteSemana = EPDate.f_fechafinsemana(masSieteDias);
            model.FechaIniSemana = iniSiguienteSemana.ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFinSemana = finSiguienteSemana.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Muestra listado del reporte de produccion de hidrologia
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarRepProduccion(int tipo, string fechaInicial, string fechaFinal)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Consultar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                DateTime fecMDCoes;
                int hMDcoes;
                model.DatosRepGeneracionCoes = servicio.ListarReporteProduccion(tipo, fecInicial, fecFinal, ConstantesMedicion.IdTipogrupoCOES, null, null, out fecMDCoes, out hMDcoes);
                model.DatosRepGeneracionNoCoes = servicio.ListarReporteProduccion(tipo, fecInicial, fecFinal, ConstantesMedicion.IdTipogrupoNoIntegrante, fecMDCoes, hMDcoes, out DateTime fec2, out int hMD2);
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
        /// Exporta el reporte de produccion
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoRP(int tipo, string fechaInicial, string fechaFinal)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                bool fechasIguales = fechaInicial == fechaFinal;

                string nombArchivo = "";
                string tipoInfo = "";
                switch (tipo.ToString())
                {
                    case ConstantesAppServicio.LectcodiProgSemanal:
                        nombArchivo = fechasIguales ? ("ReporteProgramacionSemanal_" + fecInicial.ToString("ddMMyyyy") + ".xlsx") : ("ReporteProgramacionSemanal_" + fecInicial.ToString("ddMMyyyy") + "_" + fecFinal.ToString("ddMMyyyy") + ".xlsx");
                        tipoInfo = "Programación Semanal";
                        break;
                    case ConstantesAppServicio.LectcodiProgDiario:
                        nombArchivo = fechasIguales ? ("ReporteProgramacionDiaria_" + fecInicial.ToString("ddMMyyyy") + ".xlsx") : ("ReporteProgramacionDiaria_" + fecInicial.ToString("ddMMyyyy") + "_" + fecFinal.ToString("ddMMyyyy") + ".xlsx");
                        tipoInfo = "Programación Diaria";
                        break;
                    case ConstantesAppServicio.LectcodiReprogDiario:
                        nombArchivo = fechasIguales ? ("ReporteReprogramacionDiaria_" + fecInicial.ToString("ddMMyyyy") + ".xlsx") : ("ReporteReprogramacionDiaria_" + fecInicial.ToString("ddMMyyyy") + "_" + fecFinal.ToString("ddMMyyyy") + ".xlsx");
                        tipoInfo = "Reprogramación Diaria";
                        break;
                    case ConstantesAppServicio.LectcodiEjecutadoHisto:
                        nombArchivo = fechasIguales ? ("ReporteEjecutado_" + fecInicial.ToString("ddMMyyyy") + ".xlsx") : ("ReporteEjecutado_" + fecInicial.ToString("ddMMyyyy") + "_" + fecFinal.ToString("ddMMyyyy") + ".xlsx");
                        tipoInfo = "Ejecutado";
                        break;
                }

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                servicio.GenerarExportacionRP(ruta, pathLogo, nombArchivo, tipoInfo, tipo, fecInicial, fecFinal);
                model.Resultado = nombArchivo;
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

        #region Reporte Hidrologia

        /// <summary>
        /// Principal
        /// </summary>
        /// <returns></returns>
        public ActionResult RptHidrologia()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            MigracionesModel model = new MigracionesModel();

            DateTime hoy = DateTime.Now;
            DateTime menosSieteDias = hoy.AddDays(-7);
            DateTime iniAnteriorSemana = EPDate.f_fechainiciosemana(menosSieteDias);
            DateTime finAnteriorSemana = EPDate.f_fechafinsemana(menosSieteDias);
            model.FechaIniSemana = iniAnteriorSemana.ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFinSemana = finAnteriorSemana.ToString(ConstantesAppServicio.FormatoFecha);
            model.IdReporteHidrologiaSemanal = ConstantesMigraciones.ReporteHistoricoSemanalHidrologia;

            return View(model);
        }

        /// <summary>
        /// Muestra listado del reporte de hidrologia
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarRepHidrologia(string fechaInicial, string fechaFinal)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);


                model.ListadoHidrologia = servicio.ListarReporteHidrologia(fecInicial, fecFinal);
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
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesMigraciones.ModuloManualUsuarioSGI;
            string nombreArchivo = ConstantesMigraciones.ArchivoManualUsuarioIntranetSGI;
            string pathDestino = modulo + ConstantesMigraciones.FolderRaizSGIModuloManual
;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        /// <summary>
        /// Genera el archivo a exportar el listado del reporte de hidrologia
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoRH(string fechaInicial, string fechaFinal)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = fechaInicial == fechaFinal ? ("Rpt_HistóricoSemanal_" + fecInicial.ToString("ddMMyyyy") + ".xlsx") :
                    ("Rpt_HistóricoSemanal_" + fecInicial.ToString("ddMMyyyy") + "_" + fecFinal.ToString("ddMMyyyy") + ".xlsx");

                servicio.GenerarExportacionRH(ruta, pathLogo, nameFile, fecInicial, fecFinal);
                model.Resultado = nameFile;
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
        /// Exporta archivo pdf, excel, csv, ...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult ExportarReporte()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        #endregion
    }
}