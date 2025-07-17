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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE.Helper;

namespace COES.MVC.Intranet.Areas.Resarcimientos.Controllers
{
    /// <summary>
    /// Controller: Rechazo de Carga
    /// </summary>
    public class ReporteCargaController : Controller
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
        public ReporteCargaController()
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
        /// Permite mostrar la vista inicial de la app: Rechazo Carga
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
                ViewData["RCboPeriodo"] = new SelectList((from s in permodel.ListaPeriodo select new { Periodocodi = s.PeriodoCodi, Perdnombre = ((s.PerdAnio != null) ? (s.PerdAnio + "-" + s.PerdSemestre) : s.PerdNombre) }), "Periodocodi", "Perdnombre", 0);

                //CboCliente
                List<RntRegRechazoCargaDTO> listClient = ntcse.ListAllClienteRC();
                listClient.Insert(0, new RntRegRechazoCargaDTO() { RrcCliente = 0, RrcClienteNombre = "(TODOS)" });
                ViewData["CboCliente"] = new SelectList(listClient, "RrcCliente", "RrcClienteNombre");

                //CboPuntoEntrega
                List<RntRegRechazoCargaDTO> listPE = ntcse.ListAllRechazoCarga();
                listPE.Insert(0, new RntRegRechazoCargaDTO() { Barrcodi = 0, BarrNombre = "(TODOS)" });
                ViewData["CboPuntoEntrega"] = new SelectList(listPE, "Barrcodi", "BarrNombre", 0);

                return View(b);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
                return new HttpStatusCodeResult(404, ConstantesResarcimiento.ErrorDeSistema);
            }
        }

        #region MÉTODOS LISTAR RECHAZO CARGA

        /// <summary>
        /// Muestra la lista de Rechazo de Carga para Reporte
        /// </summary>
        [HttpPost]
        public PartialViewResult DefaultCarga(FormCollection collection)
        {
            ValidarSessionUsuarios();
            RegistrosModel b = new RegistrosModel();
            try
            {
                b.EmpresaGeneradora = Convert.ToInt32(Request["empresa"]);
                b.Periodo = Convert.ToInt32(Request["periodo"]);
                b.Cliente = Convert.ToInt32(Request["cliente"]);
                b.PEntrega = Convert.ToInt32(Request["pentrega"]);
                b.Ntension = Convert.ToInt32(Request["ntension"]);
                b.NroPaginado = Convert.ToInt32(collection["nroPagina"]);
                b.ListaRechazoCarga = this.ListadoRechazoCarga(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega, b.NroPaginado, Constantes.PageSize);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return PartialView(b.ListaRechazoCarga);
        }

        #endregion

        #endregion

        #region EXPORTAR A EXCEL

        /// <summary>
        /// Metodo para Exportar Rechazo de Carga
        /// </summary>
        [HttpPost]
        public string ExportarRechazoCarga(FormCollection collection)
        {
            ValidarSessionUsuarios();
            try
            {
                RegistrosModel b = new RegistrosModel();
                int empresaGeneradora = 0;

                b.EmpresaGeneradora = Convert.ToInt32(collection["empresa"]);
                b.Periodo = Convert.ToInt32(collection["periodo"]);
                b.Cliente = Convert.ToInt32(collection["cliente"]);
                b.PEntrega = Convert.ToInt32(collection["pentrega"]);
                b.Ntension = Convert.ToInt32(collection["ntension"]);

                string fullPath = HttpContext.Server.MapPath("~/") + ConfigurationManager.AppSettings[ConstantesResarcimiento.RepositorioResarcimientos];
                List<RntRegRechazoCargaDTO> list = ntcse.ListReporteRntRegRechazoCargas(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega);

                FileInfo template = new FileInfo(fullPath + ConstantesResarcimiento.NombrePlantillaExcelRRC);
                FileInfo newFile = new FileInfo(fullPath + ConstantesResarcimiento.NombreReporteExcelRRC);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(fullPath + ConstantesResarcimiento.NombreReporteExcelRRC);
                }

                int row = 7;

                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];
                    if (row == 7)
                    {
                        //SiEmpresaDTO siem = ntcse.GetByIdSiEmpresa(b.EmpresaGeneradora);
                        //ws.Cells[2, 4].Value = (b.EmpresaGeneradora !=0) ? siem.Emprnomb : string.Empty;
                        RntPeriodoDTO pto = ntcse.GetByIdRntPeriodo(b.Periodo);
                        ws.Cells[3, 4].Value = (b.Periodo != 0) ? (pto.PerdAnio.ToString() + "-" + pto.PerdSemestre) : "(TODOS)";
                    }
                    foreach (RntRegRechazoCargaDTO item in list)
                    {
                        ws.Cells[row, 2].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws.Cells[row, 3].Value = (item.RrcClienteNombre != null) ? item.RrcClienteNombre : string.Empty;
                        ws.Cells[row, 4].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws.Cells[row, 5].Value = (item.RrcCodiAlimentador != null) ? item.RrcCodiAlimentador : string.Empty;
                        ws.Cells[row, 6].Value = (item.RrcSubestacionDstrb != null) ? item.RrcSubestacionDstrb : string.Empty;
                        ws.Cells[row, 7].Value = (item.RrcNivelTensionSed != 0) ? string.Format("{0:0.00}", item.RrcNivelTensionSed) : "0.00";
                        ws.Cells[row, 8].Value = (item.RrcEf != 0) ? Convert.ToString(item.RrcEf) : string.Empty;
                        ws.Cells[row, 9].Value = (item.RrcEvenCodiDesc != null) ? item.RrcEvenCodiDesc : string.Empty;
                        ws.Cells[row, 10].Value = (item.RrcFechaInicio != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RrcFechaInicio) : string.Empty;
                        ws.Cells[row, 11].Value = (item.RrcFechaFin != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RrcFechaFin) : string.Empty;
                        ws.Cells[row, 12].Value = (item.RrcPk != 0) ? Convert.ToString(item.RrcPk) : string.Empty;
                        ws.Cells[row, 13].Value = (item.RrcCompensable != null) ? item.RrcCompensable : string.Empty;
                        ws.Cells[row, 14].Value = (item.RrcEnsFk != 0) ? string.Format("{0:0.000}", item.RrcEnsFk) : "0.000";
                        ws.Cells[row, 15].Value = (item.RrcCompensacion != 0) ? string.Format("{0:0.00}", item.RrcCompensacion) : "0.00";


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
                string file = ConstantesResarcimiento.NombreReporteExcelRRC;
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
        /// Muestra la lista de Rechazo de Carga para Reporte
        /// </summary>
        private List<RegistroRechazoCargaModel> ListadoRechazoCarga(int? empresaGeneradora, int periodo, int cliente, int pEntrega, int NroPaginado, int PageSize)
        {
            List<RegistroRechazoCargaModel> list = new List<RegistroRechazoCargaModel>();
            try
            {
                if (User != null)
                {
                    Log.Info("iniciando lista de rechazo carga");
                    List<RntRegRechazoCargaDTO> opciones = ntcse.ListReportePaginadoRntRegRechazoCargas(empresaGeneradora, periodo, cliente, pEntrega, NroPaginado, PageSize);
                    foreach (RntRegRechazoCargaDTO item in opciones)
                    {
                        list.Add(new RegistroRechazoCargaModel()
                        {
                            RRCKEY = Convert.ToInt32(item.RegRechazoCargaCodi),
                            RRCEMPRESAGENERADORANOMBRE = item.RrcEmpresaGeneradoraNombre,
                            RRCEMPRESAGENERADORA = item.RrcEmpresaGeneradora,
                            RRCCLIENTENOMBRE = item.RrcClienteNombre,
                            RRCPUNTOENTREGANOMBRE = item.BarrNombre,
                            RRCEVENCODI = item.EvenCodi,
                            RRCEVENCODINOMB = item.RrcEvenCodiDesc,
                            RRCFECHAINICIO = Convert.ToDateTime(item.RrcFechaInicio),
                            RRCFECHAFIN = Convert.ToDateTime(item.RrcFechaFin),
                            RRCSUBESTACIONDSTRB = item.RrcSubestacionDstrb,
                            RRCNIVELTENSIONSED = Convert.ToDecimal(item.RrcNivelTensionSed),
                            RRCNIVELTENSIONNOMB = item.RrcSubestacionDstrb,
                            RRCCODIALIMENTADOR = item.RrcCodiAlimentador,
                            RRCDURACION = CalculaDuracion(item.RrcFechaInicio, item.RrcFechaFin),
                            RRCNRCF = Convert.ToInt32(item.RrcNrcf),
                            RRCEF = Convert.ToDecimal(item.RrcEf),
                            RRCENERGIAENS = item.RrcEnsFk,
                            RRCCOMPENSACION = Convert.ToDecimal(item.RrcCompensacion)
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
            RegistroRechazoCargaModel model = new RegistroRechazoCargaModel();
            RegistrosModel b = new RegistrosModel();
            b.EmpresaGeneradora = Convert.ToInt32(collection["empresa"]);
            b.Periodo = Convert.ToInt32(collection["periodo"]);
            b.Cliente = Convert.ToInt32((collection["cliente"] != "") ? collection["cliente"] : "0");
            b.PEntrega = Convert.ToInt32((collection["pentrega"] != "") ? collection["pentrega"] : "0");
            b.Ntension = Convert.ToInt32((collection["ntension"] != "") ? collection["ntension"] : "0");

            int nroRegistros = ntcse.ListReporteRntRegRechazoCargas(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega).Count;
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

        public List<PEntregaModel> ListSubEstacion(int emprgen, int periodo, int cliente)
        {
            List<PEntregaModel> result = new List<PEntregaModel>();
            List<RntRegPuntoEntregaDTO> Listrpe = ntcse.ListRntRegPuntoEntregas(emprgen, periodo, cliente, 0);

            var opciones = Listrpe.GroupBy(u => u.Barrcodi).ToList();
            result.Add(new PEntregaModel()
            {
                Areacodi = 0,
                Areanomb = "(TODOS)"
            });
            foreach (IGrouping<int, RntRegPuntoEntregaDTO> item in opciones)
            {
                result.Add(new PEntregaModel()
                {
                    Areacodi = item.ElementAt<RntRegPuntoEntregaDTO>(0).Barrcodi,
                    Areanomb = item.ElementAt<RntRegPuntoEntregaDTO>(0).BarrNombre
                });
            }
            return result;
        }

        public string CalculaDuracion(DateTime ini, DateTime fin)
        {
            TimeSpan diff = fin.Subtract(ini);
            int days = diff.Days;
            int hours = diff.Hours;
            int mins = diff.Minutes;

            int hora = days * 24 + hours;
            int minuto = mins;

            String sDuration = string.Format("{0:0#}", hora) + ":" + string.Format("{0:0#}", minuto);
            return sDuration;
        }

        #endregion

        public JsonResult obtenerPuntoEntregas(int idCliente)
        {

            List<RntRegRechazoCargaDTO> listCboRC = ntcse.ListChangeClienteRC(idCliente);

            return Json(listCboRC);

        }

    }
}
