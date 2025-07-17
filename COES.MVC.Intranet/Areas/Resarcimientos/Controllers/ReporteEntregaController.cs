using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.Resarcimientos.Models;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE;
using OfficeOpenXml;
using System;
using log4net;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE.Helper;


namespace COES.MVC.Intranet.Areas.Resarcimientos.Controllers
{
    /// <summary>
    /// Controller: Punto de Entrega
    /// </summary>
    public class ReporteEntregaController : Controller
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
        public ReporteEntregaController()
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

        #region Default

        /// <summary>
        /// Valida Session true
        /// </summary>
        public void ValidarSessionUsuarios()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                //true
            }
            else
            {
                Response.Redirect(HttpContext.Server.MapPath("~/") + Constantes.PaginaLogin);
            }
        }

        /// <summary>
        /// Permite mostrar la vista inicial de la app: Punto Entrega
        /// </summary>
        /// <returns></returns>
        //[CustomAuthorize]
        //[HttpPost]
        public ActionResult Default()
        {
            ValidarSessionUsuarios();
            try
            {
                RegistrosModel b = new RegistrosModel();

                b.Titulo = Request["titleOpcion"];
                b.Key = Request["keyOpcion"];

                b.EmpresaGeneradora = Convert.ToInt32(Request["empresa"]);
                b.Periodo = Convert.ToInt32(Request["periodo"]);
                b.Cliente = Convert.ToInt32(Request["cliente"]);
                b.PEntrega = Convert.ToInt32(Request["pentrega"]);
                b.Ntension = Convert.ToInt32(Request["ntension"]);
                int periodo = b.Periodo;

                EmpresasGeneradorasModel empgmodel = new EmpresasGeneradorasModel();
                empgmodel.ListaEmpresasGeneradoras = ntcse.ListEmpresasGeneradoras();
                empgmodel.ListaEmpresasGeneradoras.Insert(0, empgmodel.ListaComboTodos);
                ViewData["RCboEmpresasGeneradoras"] = new SelectList(empgmodel.ListaEmpresasGeneradoras, "Emprcodi", "Emprnomb", 0);

                //CboPeriodo
                PeriodosModel permodel = new PeriodosModel();
                permodel.ListaPeriodo = ntcse.ListRntPeriodos();
                //permodel.ListaPeriodo.Insert(0, permodel.ListaComboTodos);
                ViewData["CboPeriodo"] = new SelectList((from s in permodel.ListaPeriodo select new { Periodocodi = s.PeriodoCodi, Perdnombre = ((s.PerdAnio != null) ? (s.PerdAnio + "-" + s.PerdSemestre) : s.PerdNombre) }), "Periodocodi", "Perdnombre", 0);

                //CboCliente
                //ClienteModel climodel = new ClienteModel();
                //climodel.ListaClientesPE = ntcse.ListAllClientePE();
                //climodel.ListaClientesPE.Insert(0, climodel.ListaComboSeleccionarPE);
                //ViewData["CboCliente"] = new SelectList(climodel.ListaClientesPE, "RpeCliente", "RpeClienteNombre", 0);

                List<RntRegPuntoEntregaDTO> listClient = ntcse.ListAllClientePE();
                listClient.Insert(0, new RntRegPuntoEntregaDTO() { RpeCliente = 0, RpeClienteNombre = "(TODOS)" });
                ViewData["CboCliente"] = new SelectList(listClient, "RpeCliente", "RpeClienteNombre");

                //CboPuntoEntrega
                List<RntRegPuntoEntregaDTO> listPE = ntcse.ListAllPuntoEntrega();
                listPE.Insert(0, new RntRegPuntoEntregaDTO() { Barrcodi = 0, BarrNombre = "(TODOS)" });
                ViewData["CboPuntoEntrega"] = new SelectList(listPE, "Barrcodi", "BarrNombre", 0);

                return View(b);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(404, ConstantesResarcimiento.ErrorDeSistema);
            }

        }

        #region MÉTODOS LISTAR PUNTO ENTREGA

        /// <summary>
        /// Muestra la lista de Punto de Entrega para Reporte
        /// </summary>
        [HttpPost]
        public PartialViewResult DefaultEntrega(FormCollection collection)
        {
            ValidarSessionUsuarios();
            RegistrosModel b = new RegistrosModel();
            try
            {
                b.EmpresaGeneradora = Convert.ToInt32(collection["empresa"]);
                b.Periodo = Convert.ToInt32(collection["periodo"]);
                b.Cliente = Convert.ToInt32(Request["cliente"]);
                b.PEntrega = Convert.ToInt32(Request["pentrega"]);
                b.Ntension = Convert.ToInt32(Request["ntension"]);
                b.NroPaginado = Convert.ToInt32(collection["nroPagina"]);
                b.ListaPuntosEntrega = this.ListadoPuntoEntrega(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega, b.NroPaginado, Constantes.PageSize);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return PartialView(b.ListaPuntosEntrega);
        }

        #endregion

        #endregion

        #region LISTAR EMPRESA RESPONSABLES

        /// <summary>
        /// Muestra la lista de Empresas Responsables
        /// </summary>
        [HttpPost]
        public PartialViewResult EmpResponsable(FormCollection collection)
        {
            ValidarSessionUsuarios();
            List<EmpresasResponsablesModel> b = new List<EmpresasResponsablesModel>();
            try
            {
                int key = Convert.ToInt32(collection["registro"]);
                b = listarEmpresasGeneradoras(key);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return PartialView(b);
        }
        #endregion

        #region EXPORTAR A EXCEL

        /// <summary>
        /// Metodo que Exporta Punto de Entrega
        /// </summary>
        [HttpPost]
        public string ExportarPuntoEntrega(FormCollection collection)
        {
            ValidarSessionUsuarios();
            try
            {
                RegistrosModel b = new RegistrosModel();

                b.EmpresaGeneradora = Convert.ToInt32(collection["empresa"]);
                b.Periodo = Convert.ToInt32(collection["periodo"]);
                b.Cliente = Convert.ToInt32(collection["cliente"]);
                b.PEntrega = Convert.ToInt32(collection["pentrega"]);
                b.Ntension = Convert.ToInt32(collection["ntension"]);

                string fullPath = HttpContext.Server.MapPath("~/") + ConfigurationManager.AppSettings[ConstantesResarcimiento.RepositorioResarcimientos];
                List<RntRegPuntoEntregaDTO> list = ntcse.ListReporteRntRegPuntoEntregas(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega);

                FileInfo template = new FileInfo(fullPath + ConstantesResarcimiento.NombrePlantillaExcelRPE);
                FileInfo newFile = new FileInfo(fullPath + ConstantesResarcimiento.NombreReporteExcelRPE);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(fullPath + ConstantesResarcimiento.NombreReporteExcelRPE);
                }

                int row = 7;

                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];
                    if (row == 7)
                    {
                        //SiEmpresaDTO siem = ntcse.GetByIdSiEmpresa(b.EmpresaGeneradora);
                        //ws.Cells[2, 4].Value = (b.EmpresaGeneradora != 0) ? siem.Emprnomb : string.Empty;
                        RntPeriodoDTO pto = ntcse.GetByIdRntPeriodo(b.Periodo);
                        ws.Cells[3, 4].Value = (b.Periodo != 0) ? (pto.PerdAnio.ToString() + "-" + pto.PerdSemestre) : "(TODOS)";
                    }
                    foreach (RntRegPuntoEntregaDTO item in list)
                    {
                        ws.Cells[row, 2].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws.Cells[row, 3].Value = (item.RpeClienteNombre != null) ? item.RpeClienteNombre : string.Empty;
                        ws.Cells[row, 4].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws.Cells[row, 5].Value = (item.RpeNivelTensionDesc != null) ? item.RpeNivelTensionDesc : string.Empty;
                        ws.Cells[row, 6].Value = (item.RpeEnergSem != 0) ? Convert.ToString(Math.Ceiling(item.RpeEnergSem)) : string.Empty;
                        ws.Cells[row, 7].Value = (item.RpeIncremento != null) ? item.RpeIncremento : string.Empty;
                        ws.Cells[row, 8].Value = (item.TipIntNombre != null) ? item.TipIntNombre : string.Empty;
                        ws.Cells[row, 9].Value = (item.RpeTramFuerMayor != null) ? item.RpeTramFuerMayor : string.Empty;
                        ws.Cells[row, 10].Value = (item.RpeNi != 0) ? string.Format("{0:0.00}", item.RpeNi) : string.Empty;
                        ws.Cells[row, 11].Value = (item.RpeKi != 0) ? string.Format("{0:0.00}", item.RpeKi) : string.Empty;
                        ws.Cells[row, 12].Value = (item.RpeFechaInicio != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RpeFechaInicio) : string.Empty;
                        ws.Cells[row, 13].Value = (item.RpeFechaFin != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RpeFechaFin) : string.Empty;
                        ws.Cells[row, 14].Value = (item.RpePrgFechaInicio != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RpePrgFechaInicio) : string.Empty;
                        ws.Cells[row, 15].Value = (item.RpePrgFechaFin != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RpePrgFechaFin) : string.Empty;
                        ws.Cells[row, 16].Value = (item.RpeCausaInterrupcion != null) ? item.RpeCausaInterrupcion : string.Empty;
                        ws.Cells[row, 17].Value = (item.RpeEiE != 0) ? string.Format("{0:0.00}", item.RpeEiE + "%") : string.Empty;
                        ws.Cells[row, 18].Value = (item.RpeCompensacion != 0) ? string.Format("{0:0.00}", item.RpeCompensacion) : string.Empty;

                        List<RntEmpresaRegptoentregaDTO> listempr = ntcse.ListRntEmpresaRegptoentregas(item.RegPuntoEntCodi);
                        int row2 = row;
                        int fil = 19;
                        foreach (RntEmpresaRegptoentregaDTO itemempr in listempr)
                        {
                            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#366092");
                            ws.Cells[6, fil].Value = "Empresa Responsables";
                            ws.Cells[6, fil].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[6, fil].Style.Fill.BackgroundColor.SetColor(colFromHex);
                            ws.Cells[6, fil].Style.Font.Color.SetColor(Color.White);

                            ws.Cells[row2, fil].Value = (itemempr.EmpRpeNombre != null) ? itemempr.EmpRpeNombre : string.Empty;
                            fil++;
                            ws.Cells[6, fil].Value = "%";
                            ws.Cells[6, fil].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[6, fil].Style.Fill.BackgroundColor.SetColor(colFromHex);
                            ws.Cells[6, fil].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[row2, fil].Value = (itemempr.RegPorcentaje != null) ? Convert.ToString(itemempr.RegPorcentaje * 100) + "%" : string.Empty;
                            fil++;
                        }
                        row++;
                    }

                    xlPackage.Save();
                }
                return ConstantesResarcimiento.UrlGenerarReporte + fullPath;
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
                return ConstantesResarcimiento.ErrorDeSistema;
            }
        }
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            try
            {
                string file = ConstantesResarcimiento.NombreReporteExcelRPE;

                string app = Constantes.AppExcel;

                string fullPath = HttpContext.Server.MapPath("~/") + ConfigurationManager.AppSettings[ConstantesResarcimiento.RepositorioResarcimientos] + file;

                return File(fullPath, app, file);
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
        /// Muestra la lista de Punto de Entrega
        /// </summary>
        private List<RegistroPuntoEntregaModel> ListadoPuntoEntrega(int? empresaGeneradora, int periodo, int cliente, int pEntrega, int NroPaginado, int PageSize)
        {
            List<RegistroPuntoEntregaModel> list = new List<RegistroPuntoEntregaModel>();
            try
            {
                if (User != null)
                {
                    Log.Info("iniciando lista de punto entrega");
                    List<RntRegPuntoEntregaDTO> SortedList = ntcse.ListReportePaginadoRntRegPuntoEntregas(empresaGeneradora, periodo, cliente, pEntrega, NroPaginado, PageSize);
                    List<RntRegPuntoEntregaDTO> opciones = SortedList.OrderBy(o => o.TipoIntCodi).ToList();
                    foreach (RntRegPuntoEntregaDTO item in opciones)
                    {
                        list.Add(new RegistroPuntoEntregaModel()
                        {
                            RPEKEY = item.RegPuntoEntCodi,
                            RPEEMPRESAGENERADORANOMBRE = item.RpeEmpresaGeneradoraNombre,
                            RPECLIENTENOMBRE = item.RpeClienteNombre,
                            RPEPUNTOENTREGANOMBRE = item.BarrNombre,
                            RPETIPOINTCODI = item.TipoIntCodi,
                            RPETIPOINTNOMBRE = item.TipIntNombre,
                            RPEFECHAINICIO = Convert.ToDateTime(item.RpeFechaInicio),
                            RPEFECHAFIN = Convert.ToDateTime(item.RpeFechaFin),
                            RPECOMPENSACION = Convert.ToDouble(item.RpeCompensacion),
                            RPEDURACION = CalculaDuracion(item.RpeFechaInicio, item.RpeFechaFin),
                            RPECAUSAINTERRUPCION = item.RpeCausaInterrupcion,
                            RPETRAMFUERMAYOR = item.RpeTramFuerMayor
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

        [HttpPost]
        public PartialViewResult Paginado(FormCollection collection)
        {
            RegistroPuntoEntregaModel model = new RegistroPuntoEntregaModel();
            RegistrosModel b = new RegistrosModel();
            b.EmpresaGeneradora = Convert.ToInt32(collection["empresa"]);
            b.Periodo = Convert.ToInt32(collection["periodo"]);
            b.Cliente = Convert.ToInt32((collection["cliente"] != "") ? collection["cliente"] : "0");
            b.PEntrega = Convert.ToInt32((collection["pentrega"] != "") ? collection["pentrega"] : "0");
            b.Ntension = Convert.ToInt32((collection["ntension"] != "") ? collection["ntension"] : "0");

            int nroRegistros = ntcse.ListReporteRntRegPuntoEntregas(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega).Count;

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }

        #endregion

        #region Métodos Combo Default


        public string CalculaDuracion(DateTime ini, DateTime fin)
        {
            TimeSpan diff = fin.Subtract(ini);
            int days = diff.Days;
            int hours = diff.Hours;
            int mins = diff.Minutes;

            int hora = days * 24 + hours;
            int minuto = mins;
            //decimal duration = new decimal(days * 24 + hours + 1.0 * mins / 60.0);

            String sDuration = string.Format("{0:0#}", hora) + ":" + string.Format("{0:0#}", minuto);

            return sDuration;
            //DateTime.Parse(fin.ToString()).Subtract(DateTime.Parse(ini.ToString())).ToString(@"hh\.mm");
        }

        public List<EmpresasResponsablesModel> listarEmpresasGeneradoras(int key)
        {
            List<EmpresasResponsablesModel> list = new List<EmpresasResponsablesModel>();
            List<RntEmpresaRegptoentregaDTO> options = ntcse.ListRntEmpresaRegptoentregas(key);
            foreach (RntEmpresaRegptoentregaDTO item in options)
            {

                list.Add(new EmpresasResponsablesModel()
                {
                    Empgencodi = item.EmpGenCodi,
                    Regpuntoentcodi = item.RegPuntoEntCodi,
                    Emprcodi = item.EmprCodi,
                    Regporcentaje = item.RegPorcentaje * 100,
                    Emprpenombre = item.EmpRpeNombre
                });
            }
            return list;
        }

        #endregion

        public JsonResult obtenerPuntoEntregas(int idCliente)
        {

            List<RntRegPuntoEntregaDTO> listCboPE = ntcse.ListChangeClientePE(idCliente);

            return Json(listCboPE);

        }

    }
}
