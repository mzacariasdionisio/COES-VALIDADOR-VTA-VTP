using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
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
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE;
using COES.MVC.Extranet.Areas.Resarcimientos.Models;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE.Helper;
using COES.MVC.Extranet.Helper;

namespace COES.MVC.Extranet.Areas.Resarcimientos.Controllers
{
    /// <summary>
    /// Controller: Rechazo de Carga
    /// </summary>
    public class RechazoCargaController : Controller
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
        public RechazoCargaController()
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
                List<ClienteModel> climodel = ListEmpresasClientesRrc(b.EmpresaGeneradora, b.Periodo);
                ViewData["CboCliente"] = new SelectList(climodel, "Emprcodi", "Emprnomb", b.Cliente);
                if (b.Cliente == 0 && climodel.Count != 0) { b.Cliente = climodel[0].Emprcodi; }

                //CboPuntoEntrega
                List<PEntregaModel> pemodel = ListSubEstacionRrc(b.EmpresaGeneradora, b.Periodo, b.Cliente);
                ViewData["CboPuntoEntrega"] = new SelectList(pemodel, "Areacodi", "Areanomb", b.PEntrega);
                if (b.PEntrega == 0 && pemodel.Count != 0) { b.PEntrega = pemodel[0].Areacodi; }

                return View(b);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
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

        #region Métodos RECHAZO CARGA

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

                //CboCliente
                List<ClienteModel> climodel = ListEmpresasClientesRrc(b.EmpresaGeneradora, b.Periodo);
                //climodel.Insert(0, ClienteModel.ListaComboTodos);
                ViewData["CboCliente"] = new SelectList(climodel, "Emprcodi", "Emprnomb", b.Cliente);
                if (b.Cliente == 0 && climodel.Count != 0) { b.Cliente = climodel[0].Emprcodi; }

                //CboPuntoEntrega
                List<PEntregaModel> pemodel = ListSubEstacionRrc(b.EmpresaGeneradora, b.Periodo, b.Cliente);
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
        /// Permite Cargar la Lista de Rechazo de carga
        /// </summary>
        [HttpPost]
        public PartialViewResult DefaultCarga(FormCollection collection)
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
                b.ListaRechazoCarga = this.ListadoRechazoCarga(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega, b.NroPaginado, Constantes.PageSize);
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
            }
            return PartialView(b.ListaRechazoCarga);
        }

        /// <summary>
        /// Metodo de Lista de Rechazo de carga
        /// </summary>
        private List<RegistroRechazoCargaModel> ListadoRechazoCarga(int? empresaGeneradora, int periodo, int cliente, int pEntrega, int NroPaginado, int PageSize)
        {
            List<RegistroRechazoCargaModel> list = new List<RegistroRechazoCargaModel>();
            try
            {
                if (User != null)
                {
                    List<RntRegRechazoCargaDTO> SortedList = ntcse.ListPaginadoRntRegRechazoCargas(empresaGeneradora, periodo, cliente, pEntrega, NroPaginado, PageSize);
                    List<RntRegRechazoCargaDTO> opciones = SortedList.OrderBy(o => o.RrcCodiAlimentador).ToList();
                    foreach (RntRegRechazoCargaDTO item in opciones)
                    {
                        list.Add(new RegistroRechazoCargaModel()
                        {
                            RRCKEY = Convert.ToInt32(item.RegRechazoCargaCodi),
                            RRCEMPRESAGENERADORANOMBRE = item.RrcEmpresaGeneradoraNombre,
                            RRCCLIENTENOMBRE = item.RrcClienteNombre,
                            RRCPUNTOENTREGANOMBRE = item.BarrNombre,
                            RRCEVENCODI = item.EvenCodi,
                            RRCEVENCODINOMB = item.RrcEvenCodiDesc,
                            RRCFECHAINICIO = Convert.ToDateTime(item.RrcFechaInicio),
                            RRCFECHAFIN = Convert.ToDateTime(item.RrcFechaFin),
                            RRCSUBESTACIONDSTRB = item.RrcSubestacionDstrb,
                            RRCNIVELTENSIONSED = Convert.ToDecimal(item.RrcNivelTensionSed),
                            RRCCODIALIMENTADOR = item.RrcCodiAlimentador,
                            RRCDURACION = CalculaDuracion(item.RrcFechaInicio, item.RrcFechaFin),
                            RRCNRCF = Convert.ToInt32(item.RrcNrcf),
                            RRCEF = Convert.ToDecimal(item.RrcEf),
                            RRCCOMPENSABLE = item.RrcCompensable,
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

        /// <summary>
        /// Metodo de Lista por registro de Rechazo de carga
        /// </summary>
        public RegistroRechazoCargaModel ListadoDatosNRegCarga(int nReg)
        {
            RegistroRechazoCargaModel list = new RegistroRechazoCargaModel();
            try
            {
                if (User != null)
                {
                    RntRegRechazoCargaDTO item = ntcse.GetByIdRntRegRechazoCarga(nReg);
                    if (item != null)
                    {
                        list.RRCKEY = Convert.ToInt32(item.RegRechazoCargaCodi);
                        list.RRCEMPRESAGENERADORA = item.RrcEmpresaGeneradora;
                        list.RRCEMPRESAGENERADORANOMBRE = item.BarrNombre;
                        list.RRCCLIENTE = item.EmprCodi;
                        list.RRCCLIENTENOMBRE = item.RrcClienteNombre;
                        list.RRCPUNTOENTREGA = item.AreaCodi;
                        list.RRCPUNTOENTREGANOMBRE = item.BarrNombre;
                        list.RRCPERDCODI = item.PeriodoCodi;
                        list.RRCNIVELTENSION = Convert.ToInt32(item.RrcNivelTension);
                        list.RRCNIVELTENSIONNOMB = GetByNivelTension(item.RrcNivelTension);
                        list.RRCEVENCODI = item.EvenCodi;
                        list.RRCEVENCODINOMB = GetByEventoRechazoCarga(item.RrcEmpresaGeneradora, item.EvenCodi);
                        list.RRCFECHAINICIO = Convert.ToDateTime(item.RrcFechaInicio);
                        list.RRCFECHAFIN = Convert.ToDateTime(item.RrcFechaFin);
                        list.RRCSUBESTACIONDSTRB = item.RrcSubestacionDstrb;
                        list.RRCNIVELTENSIONSED = Convert.ToDecimal(item.RrcNivelTensionSed);
                        list.RRCCODIALIMENTADOR = item.RrcCodiAlimentador;
                        list.RRCENERGIAENS = Convert.ToDecimal(item.RrcEnergiaEns);
                        list.RRCNRCF = Convert.ToInt32(item.RrcNrcf);
                        list.RRCEF = item.RrcEf;
                        list.RRCCOMPENSACION = item.RrcCompensacion;
                        list.RRCESTADO = Convert.ToInt32(item.RrcEstado);
                        list.RRCUSUARIOCREACION = item.RrcUsuarioCreacion;
                        list.RRCFECHACREACION = Convert.ToDateTime(item.RrcFechaCreacion);
                        list.RRCUSUARIOUPDATE = item.RrcUsuarioUpdate;
                        list.RRCFECHAUPDATE = Convert.ToDateTime(item.RrcFechaUpdate);
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

        #region Métodos CARGA EVENTOS POR EMPRCLIENTE

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

                RegistroRechazoCargaModel rrc = new RegistroRechazoCargaModel();
                rrc = this.ListadoDatosNRegCarga(Convert.ToInt32(b.Registro));
                return Json(rrc, JsonRequestBehavior.AllowGet);
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
        /// Metodo de EXPOTAR de Rechazo de carga
        /// </summary>
        [HttpPost]
        public string ExportarRechazoCarga(FormCollection collection)
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
                List<RntRegRechazoCargaDTO> opciones = ntcse.ListRntRegRechazoCargas(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega);
                List<RntRegRechazoCargaDTO> list = opciones.OrderBy(o => o.RrcCodiAlimentador).ToList();
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
                        ws.Cells[row, 2].Value = (item.RrcEmpresaGeneradoraNombre != null) ? item.RrcEmpresaGeneradoraNombre : string.Empty;
                        ws.Cells[row, 3].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws.Cells[row, 4].Value = (item.RrcClienteNombre != null) ? item.RrcClienteNombre : string.Empty;
                        ws.Cells[row, 5].Value = (item.BarrNombre != null) ? item.BarrNombre : string.Empty;
                        ws.Cells[row, 6].Value = (item.RrcCodiAlimentador != null) ? item.RrcCodiAlimentador : string.Empty;
                        ws.Cells[row, 7].Value = (item.RrcSubestacionDstrb != null) ? item.RrcSubestacionDstrb : string.Empty;
                        ws.Cells[row, 8].Value = (item.RrcNivelTensionSed != 0) ? string.Format("{0:0.00}", item.RrcNivelTensionSed) : "0.00";
                        ws.Cells[row, 9].Value = (item.RrcEf != 0) ? Convert.ToString(item.RrcEf) : string.Empty;
                        ws.Cells[row, 10].Value = (item.RrcEvenCodiDesc != null) ? item.RrcEvenCodiDesc : string.Empty;
                        ws.Cells[row, 11].Value = (item.RrcFechaInicio != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RrcFechaInicio) : string.Empty;
                        ws.Cells[row, 12].Value = (item.RrcFechaFin != null) ? string.Format("{0:" + Constantes.FormatoFechaFull + "}", item.RrcFechaFin) : string.Empty;
                        ws.Cells[row, 13].Value = (item.RrcPk != 0) ? Convert.ToString(item.RrcPk) : string.Empty;
                        ws.Cells[row, 14].Value = (item.RrcCompensable != null) ? item.RrcCompensable : string.Empty;
                        ws.Cells[row, 15].Value = (item.RrcEnsFk != 0) ? string.Format("{0:0.000}", item.RrcEnsFk) : "0.000";
                        ws.Cells[row, 16].Value = (item.RrcCompensacion != 0) ? string.Format("{0:0.00}", item.RrcCompensacion) : "0.00";


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

            int nroRegistros = ntcse.ListRntRegRechazoCargas(b.EmpresaGeneradora, b.Periodo, b.Cliente, b.PEntrega).Count;
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

        public List<RntPeriodoDTO> ListRntPeriodos()
        {
            List<RntPeriodoDTO> list = ntcse.ListComboRntPeriodos();
            return list;
        }

        public List<ClienteModel> ListEmpresasClientesRrc(int emprgen, int periodo)
        {
            List<ClienteModel> result = new List<ClienteModel>();
            ClienteModel ad = new ClienteModel();
            List<RntRegRechazoCargaDTO> Listrpe = ntcse.ListRntRegRechazoCargas(emprgen, periodo, 0, 0);

            var opciones = Listrpe.GroupBy(u => u.EmprCodi).ToList();
            foreach (IGrouping<int, RntRegRechazoCargaDTO> item in opciones)
            {
                result.Add(new ClienteModel()
                {
                    Emprcodi = item.ElementAt<RntRegRechazoCargaDTO>(0).EmprCodi,
                    Emprnomb = item.ElementAt<RntRegRechazoCargaDTO>(0).RrcClienteNombre
                });
            }
            return result;
        }

        public List<PEntregaModel> ListSubEstacionRrc(int emprgen, int periodo, int cliente)
        {
            List<PEntregaModel> result = new List<PEntregaModel>();
            List<RntRegRechazoCargaDTO> Listrpe = ntcse.ListRntRegRechazoCargas(emprgen, periodo, cliente, 0);

            var opciones = Listrpe.GroupBy(u => u.Barrcodi).ToList();
            foreach (IGrouping<int, RntRegRechazoCargaDTO> item in opciones)
            {
                result.Add(new PEntregaModel()
                {
                    Areacodi = item.ElementAt<RntRegRechazoCargaDTO>(0).Barrcodi,
                    Areanomb = item.ElementAt<RntRegRechazoCargaDTO>(0).BarrNombre
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

        /// <summary>
        /// Buscar Evento By evencodi
        /// </summary>
        public string GetByEventoRechazoCarga(int emprcodi, int evencodi)
        {
            string result = "";
            List<EveEventoDTO> ListaEventos = new List<EveEventoDTO>();
            ListaEventos = ntcse.ListEventos(emprcodi);
            if (evencodi == 0 && ListaEventos.Count != 0)
            {
                result = "0";
            }
            else
            {
                EveEventoDTO obj = ListaEventos.Find(x => x.Evencodi == evencodi);
                if (obj != null)
                {
                    result = obj.CodEve;
                }
                else
                {
                    result = "--";
                }
            }

            return result;
        }

        #endregion

    }
}
