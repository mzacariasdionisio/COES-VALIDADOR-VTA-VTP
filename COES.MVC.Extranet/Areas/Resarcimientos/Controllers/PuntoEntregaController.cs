using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
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
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Areas.Resarcimientos.Models;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE.Helper;


namespace COES.MVC.Extranet.Areas.Resarcimientos.Controllers
{
    /// <summary>
    /// Controller: Punto de Entrega
    /// </summary>
    public class PuntoEntregaController : Controller
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
        public PuntoEntregaController()
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

                //CboEmpresasGeneradoras
                EmpresasGeneradorasModel empgmodel = new EmpresasGeneradorasModel();
                empgmodel.ListaEmpresasGeneradoras = ntcse.ListEmpresasGeneradoras();
                empgmodel.ListaEmpresasGeneradoras.Add(empgmodel.ListaComboTodos);
                ViewData["CboEmpresasGeneradoras"] = new SelectList(empgmodel.ListaEmpresasGeneradoras, "Emprcodi", "Emprnomb", b.EmpresaGeneradora);

                //CboPeriodo
                RntPeriodoModel permodel = new RntPeriodoModel();
                permodel.ListaRntPeriodo = ListRntPeriodos();
                permodel.ListaRntPeriodo.Add(permodel.ListaComboTodos);
                //Iniciando Periodo Consulta
                if (permodel != null)
                {
                    b.Periodo = permodel.ListaRntPeriodo[0].PeriodoCodi;
                }
                ViewData["CboPeriodo"] = new SelectList((from s in permodel.ListaRntPeriodo select new { Periodocodi = s.PeriodoCodi, Perdnombre = ((s.PerdAnio != null) ? (s.PerdAnio + "-" + s.PerdSemestre) : s.PerdNombre) }), "Periodocodi", "Perdnombre", b.Periodo);

                //CboCliente
                List<ClienteModel> climodel = ListEmpresasClientes(b.EmpresaGeneradora, b.Periodo);
                ViewData["CboCliente"] = new SelectList(climodel, "Emprcodi", "Emprnomb", b.Cliente);
                if (b.Cliente == 0 && climodel.Count != 0) { b.Cliente = climodel[0].Emprcodi; }

                //CboPuntoEntrega
                List<PEntregaModel> pemodel = ListSubEstacion(b.EmpresaGeneradora, b.Periodo, b.Cliente);
                ViewData["CboPuntoEntrega"] = new SelectList(pemodel, "Areacodi", "Areanomb", b.PEntrega);
                if (b.PEntrega == 0 && pemodel.Count != 0) { b.PEntrega = pemodel[0].Areacodi; }

                return View(b);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(404, ConstantesResarcimiento.ErrorDeSistema);
            }

        }

        /// <summary>
        /// Permite Validar las Empresas Generadoras Por Usuario
        /// </summary>
        [HttpPost]
        public JsonResult ValidaEmpresasGeneradoras(FormCollection collection)
        {
            try
            {
                int result = 0;
                int emprgen = Convert.ToInt32(collection["empresa"]);
                //asignar empresa generadora por usuario
                string user = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                COES.MVC.Extranet.SeguridadServicio.EmpresaDTO[] op = servicio.ObtenerEmpresasPorUsuario(user);
                foreach (COES.MVC.Extranet.SeguridadServicio.EmpresaDTO item in op)
                {
                    if (item.EMPRCODI == emprgen)
                    {
                        result = 1;
                        break;
                    }
                }
                return Json(result);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
                return Json(ConstantesResarcimiento.ErrorDeSistema);
            }
        }

        #endregion

        #region Métodos PUNTO ENTREGA

        /// <summary>
        /// Muestra Lista de Punto Entrega
        /// </summary>
        [HttpPost]
        public PartialViewResult Optionselect(FormCollection collection)
        {
            ValidarSessionUsuarios();
            try
            {
                RegistrosModel b = new RegistrosModel();
                b.EmpresaGeneradora = Convert.ToInt32((collection["empresa"] != "") ? collection["empresa"] : "0");
                b.Periodo = Convert.ToInt32((collection["periodo"] != "") ? collection["periodo"] : "0");
                b.Cliente = Convert.ToInt32((collection["cliente"] != "") ? collection["cliente"] : "0");
                b.PEntrega = Convert.ToInt32((collection["pentrega"] != "") ? collection["pentrega"] : "0");
                b.Ntension = Convert.ToInt32((collection["ntension"] != "") ? collection["ntension"] : "0");
                int periodo = b.Periodo;

                //CboCliente
                List<ClienteModel> climodel = ListEmpresasClientes(b.EmpresaGeneradora, periodo);
                ViewData["CboCliente"] = new SelectList(climodel, "Emprcodi", "Emprnomb", b.Cliente);
                if (b.Cliente == 0 && climodel.Count != 0) { b.Cliente = climodel[0].Emprcodi; }

                //CboPuntoEntrega
                List<PEntregaModel> pemodel = ListSubEstacion(b.EmpresaGeneradora, periodo, b.Cliente);
                ViewData["CboPuntoEntrega"] = new SelectList(pemodel, "Areacodi", "Areanomb", b.PEntrega);
                if (b.PEntrega == 0 && pemodel.Count != 0) { b.PEntrega = pemodel[0].Areacodi; }

            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return PartialView();
        }

        /// <summary>
        /// Muestra Lista de Punto Entrega
        /// </summary>
        public PartialViewResult DefaultEntrega(FormCollection collection)
        {
            ValidarSessionUsuarios();
            RegistrosModel b = new RegistrosModel();
            try
            {
                b.EmpresaGeneradora = Convert.ToInt32(collection["empresa"]);
                b.Periodo = Convert.ToInt32(collection["periodo"]);
                b.Cliente = Convert.ToInt32((collection["cliente"] != "") ? collection["cliente"] : "0");
                b.PEntrega = Convert.ToInt32((collection["pentrega"] != "") ? collection["pentrega"] : "0");
                b.Ntension = Convert.ToInt32((collection["ntension"] != "") ? collection["ntension"] : "0");
                b.NroPaginado = Convert.ToInt32(collection["nroPagina"]);
                b.ListaPuntosEntrega = this.ListadoPuntoEntrega(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega, b.NroPaginado, Constantes.PageSize);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return PartialView(b.ListaPuntosEntrega);
        }

        /// <summary>
        /// Metodo de Listar de Punto de Entrega
        /// </summary>
        private List<RegistroPuntoEntregaModel> ListadoPuntoEntrega(int? empresaGeneradora, int periodo, int cliente, int pEntrega, int NroPaginado, int PageSize)
        {
            List<RegistroPuntoEntregaModel> list = new List<RegistroPuntoEntregaModel>();
            try
            {
                if (User != null)
                {
                    List<RntRegPuntoEntregaDTO> SortedList = ntcse.ListPaginadoRntRegPuntoEntregas(empresaGeneradora, periodo, cliente, pEntrega, NroPaginado, PageSize);
                    List<RntRegPuntoEntregaDTO> opciones = SortedList.OrderBy(o => o.Concatenar).ToList();
                    foreach (RntRegPuntoEntregaDTO item in opciones)
                    {
                        list.Add(new RegistroPuntoEntregaModel()
                        {
                            RPEKEY = item.RegPuntoEntCodi,
                            RPEEMPRESAGENERADORANOMBRE = item.RpeEmpresaGeneradoraNombre,
                            RPECLIENTENOMBRE = item.RpeClienteNombre,
                            RPEPUNTOENTREGANOMBRE = item.AreaCodiNombre,
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

        /// <summary>
        /// Metodo de Listar por Registro de Punto entrega
        /// </summary>
        private RegistroPuntoEntregaModel ListadoDatosNRegEntrega(int nReg)
        {
            RegistroPuntoEntregaModel list = new RegistroPuntoEntregaModel();
            try
            {
                if (User != null)
                {
                    RntRegPuntoEntregaDTO item = ntcse.GetByIdRntRegPuntoEntrega(nReg);
                    if (item != null)
                    {
                        list.RPEKEY = item.RegPuntoEntCodi;
                        list.RPEEMPRESAGENERADORA = item.RpeEmpresaGeneradora;
                        list.RPEEMPRESAGENERADORANOMBRE = item.RpeEmpresaGeneradoraNombre;
                        list.RPECLIENTE = item.RpeCliente;//item.RpeCliente.Value;
                        list.RPECLIENTENOMBRE = item.RpeClienteNombre;
                        list.RPEPUNTOENTREGA = item.AreaCodi;
                        list.RPEPUNTOENTREGANOMBRE = item.AreaCodiNombre;
                        list.RPEPERDCODI = item.PeriodoCodi;
                        list.RPEPERDNOMB = GetByPeriodo(item.PeriodoCodi);
                        list.RPENIVELTENSION = Convert.ToInt32(item.RpeNivelTension);
                        list.RPENIVELTENSIONNOMB = GetByNivelTension(item.RpeNivelTension);
                        list.RPETIPOINTCODI = item.TipoIntCodi;
                        list.RPETIPOINTNOMBRE = item.TipIntNombre;
                        list.RPEFECHAINICIO = Convert.ToDateTime(item.RpeFechaInicio);
                        list.RPEFECHAFIN = Convert.ToDateTime(item.RpeFechaFin);
                        list.RPECOMPENSACION = Convert.ToDouble(item.RpeCompensacion);
                        list.RPECAUSAINTERRUPCION = item.RpeCausaInterrupcion;
                        list.RPETRAMFUERMAYOR = item.RpeTramFuerMayor;
                        list.RPEESTADO = Convert.ToInt32(item.RpeEstado);
                        list.RPEUSUARIOCREACION = item.RpeUsuarioCreacion;
                        list.RPEFECHACREACION = Convert.ToDateTime(item.RpeFechaCreacion);
                        list.RPEUSUARIOUPDATE = item.RpeUsuarioUpdate;
                        list.RPEFECHAUPDATE = Convert.ToDateTime(item.RpeFechaUpdate);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return list;
        }

        #endregion

        #region Métodos EMPRESAS RESPONSABLES POR REGISTRO

        public bool ValidarEmpresasResponsablesREG(int codi, int emprCodi, decimal porct)
        {
            bool res = true;
            try
            {
                List<EmpresasResponsablesModel> b = new List<EmpresasResponsablesModel>();
                b = ListarEmpresasGeneradoras(codi);
                foreach (EmpresasResponsablesModel e in b)
                {
                    if (Convert.ToInt32(emprCodi) == e.Emprcodi && porct == e.Regporcentaje)
                    {
                        res = false;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return res;
        }

        /// <summary>
        /// Muestra Ventana de Lista de Empresas responsables por Registro
        /// </summary>
        [HttpPost]
        public PartialViewResult EmpResponsable(FormCollection collection)
        {
            ValidarSessionUsuarios();
            int key = Convert.ToInt32(collection["registro"]);
            List<EmpresasResponsablesModel> b = new List<EmpresasResponsablesModel>();
            try
            {
                b = ListarEmpresasGeneradoras(key);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return PartialView(b);
        }

        /// <summary>
        /// Metodo de Lista de Empresas Generadoras
        /// </summary>
        public List<EmpresasResponsablesModel> ListarEmpresasGeneradoras(int key)
        {
            List<EmpresasResponsablesModel> list = new List<EmpresasResponsablesModel>();
            try
            {
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
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return list;
        }

        /// <summary>
        /// Permite Cargar  Datos del Registro Selecionado
        /// </summary>
        [HttpPost]
        public JsonResult BuscaRegistroParaCombobox(FormCollection collection)
        {
            try
            {
                RegistrosModel b = new RegistrosModel();
                b.Registro = collection["registro"];

                RegistroPuntoEntregaModel rpe = new RegistroPuntoEntregaModel();
                rpe = this.ListadoDatosNRegEntrega(Convert.ToInt32(b.Registro));
                return Json(rpe, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
                return Json(ConstantesResarcimiento.ErrorDeSistema);
            }
        }

        #endregion

        #region Métodos EXPORTAR

        /// <summary>
        /// Metodo de EXPORTAR de Punto de Entrega
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
                b.Cliente = Convert.ToInt32((collection["cliente"] != "") ? collection["cliente"] : "0");
                b.PEntrega = Convert.ToInt32((collection["pentrega"] != "") ? collection["pentrega"] : "0");
                b.Ntension = Convert.ToInt32((collection["ntension"] != "") ? collection["ntension"] : "0");

                string fullPath = HttpContext.Server.MapPath("~/") + ConfigurationManager.AppSettings[ConstantesResarcimiento.RepositorioResarcimientos];
                List<RntRegPuntoEntregaDTO> opciones = ntcse.ListRntRegPuntoEntregas(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega);
                List<RntRegPuntoEntregaDTO> list = opciones.OrderBy(o => o.Concatenar).ToList();
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
                        RntPeriodoDTO pto = ntcse.GetByIdRntPeriodo(b.Periodo);
                        ws.Cells[3, 4].Value = (b.Periodo != 0) ? (pto.PerdAnio.ToString() + "-" + pto.PerdSemestre) : "(TODOS)";
                    }
                    foreach (RntRegPuntoEntregaDTO item in list)
                    {
                        ws.Cells[row, 2].Value = (item.RpeEmpresaGeneradoraNombre != null) ? item.RpeEmpresaGeneradoraNombre : string.Empty;
                        ws.Cells[row, 3].Value = (item.RpeGrupoEnvio != 0) ? Convert.ToString(item.BarrNombre) : string.Empty;
                        ws.Cells[row, 4].Value = (item.RpeClienteNombre != null) ? item.RpeClienteNombre : string.Empty;
                        ws.Cells[row, 5].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws.Cells[row, 6].Value = (item.RpeNivelTensionDesc != null) ? item.RpeNivelTensionDesc : string.Empty;
                        ws.Cells[row, 7].Value = (item.RpeEnergSem != 0) ? Convert.ToString(Math.Ceiling(item.RpeEnergSem)) : string.Empty;
                        ws.Cells[row, 8].Value = (item.RpeIncremento != null) ? item.RpeIncremento : string.Empty;
                        ws.Cells[row, 9].Value = (item.TipIntNombre != null) ? item.TipIntNombre : string.Empty;
                        ws.Cells[row, 10].Value = (item.RpeTramFuerMayor != null) ? item.RpeTramFuerMayor : string.Empty;
                        ws.Cells[row, 11].Value = string.Format("{0:0.00}", item.RpeNi);
                        ws.Cells[row, 12].Value = string.Format("{0:0.00}", item.RpeKi);
                        ws.Cells[row, 13].Value = (item.RpeFechaInicio != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RpeFechaInicio) : string.Empty;
                        ws.Cells[row, 14].Value = (item.RpeFechaFin != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RpeFechaFin) : string.Empty;
                        ws.Cells[row, 15].Value = (item.RpePrgFechaInicio != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RpePrgFechaInicio) : string.Empty;
                        ws.Cells[row, 16].Value = (item.RpePrgFechaFin != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RpePrgFechaFin) : string.Empty;
                        ws.Cells[row, 17].Value = (item.RpeCausaInterrupcion != null) ? item.RpeCausaInterrupcion : string.Empty;
                        ws.Cells[row, 18].Value = (item.RpeEiE != 0) ? string.Format("{0:0.00}", item.RpeEiE + "%") : string.Empty;
                        ws.Cells[row, 19].Value = (item.RpeCompensacion != 0) ? string.Format("{0:0.00}", item.RpeCompensacion) : string.Empty;

                        List<RntEmpresaRegptoentregaDTO> listempr = ntcse.ListRntEmpresaRegptoentregas(item.RegPuntoEntCodi);
                        int row2 = row;
                        int fil = 20;
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

            int nroRegistros = ntcse.ListRntRegPuntoEntregas(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega).Count;
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

        public List<ClienteModel> ListEmpresasClientes(int emprgen, int periodo)
        {
            List<ClienteModel> result = new List<ClienteModel>();
            ClienteModel ad = new ClienteModel();
            List<RntRegPuntoEntregaDTO> Listrpe = ntcse.ListRntRegPuntoEntregas(emprgen, periodo, 0, 0);

            var opciones = Listrpe.GroupBy(u => u.RpeCliente).ToList();
            foreach (IGrouping<int, RntRegPuntoEntregaDTO> item in opciones)
            {
                result.Add(new ClienteModel()
                {
                    Emprcodi = item.ElementAt<RntRegPuntoEntregaDTO>(0).RpeCliente,
                    Emprnomb = item.ElementAt<RntRegPuntoEntregaDTO>(0).RpeClienteNombre
                });
            }
            return result;
        }

        public List<PEntregaModel> ListSubEstacion(int emprgen, int periodo, int cliente)
        {
            List<PEntregaModel> result = new List<PEntregaModel>();
            List<RntRegPuntoEntregaDTO> Listrpe = ntcse.ListRntRegPuntoEntregas(emprgen, periodo, cliente, 0);

            var opciones = Listrpe.GroupBy(u => u.Barrcodi).ToList();
            foreach (IGrouping<int, RntRegPuntoEntregaDTO> item in opciones)
            {
                result.Add(new PEntregaModel()
                {
                    //Areacodi = item.ElementAt<RntRegPuntoEntregaDTO>(0).AreaCodi,
                    //Areanomb = Convert.ToString(item.ElementAt<RntRegPuntoEntregaDTO>(0).AreaCodi)
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

        public string GetByPeriodo(int periodo)
        {
            RntPeriodoDTO conf = ntcse.GetByIdRntPeriodo(periodo);
            if (conf != null)
            {
                return conf.PerdAnio + "-" + conf.PerdSemestre;
            }
            else
            {
                return "";
            }
        }

        public string GetByNivelTension(int tension)
        {
            RntConfiguracionDTO conf = ntcse.GetByIdRntConfiguracion(tension);
            if (conf != null)
            {
                return conf.ConfValor;
            }
            else
            {
                return "";
            }
        }

        public List<RntPeriodoDTO> ListRntPeriodos()
        {
            List<RntPeriodoDTO> list = ntcse.ListComboRntPeriodos();
            return list;
        }

        #endregion

    }
}
