using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.Resarcimientos.Models;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Extranet.Controllers;
using OfficeOpenXml;
using System;
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
using System.Web.Script.Serialization;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE.Helper;
using log4net;
using COES.Framework.Base.Tools;

namespace COES.MVC.Extranet.Areas.Resarcimientos.Controllers
{
    /// <summary>
    /// Controller: Punto de Entrega
    /// </summary>
    public class ReporteController : BaseController
    {
        int intFilas = 0;
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
        public ReporteController()
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
        private List<RntRegRechazoCargaDTO> resultValidacionRC;
        private List<RntRegPuntoEntregaDTO> resultValidacionPE;
        List<RntListErroresDTO> listErrores = new List<RntListErroresDTO>();
        List<RntListErroresDTO> listErroresRC = new List<RntListErroresDTO>();

        #region [ Reporte - Reporte Historico ]

        #region Validar Sesión Usuarios

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
                Response.Redirect("~/");

            }
        }

        #endregion

        #region [Reporte]

        #region Default

        /// <summary>
        /// Muestra la ventana principal de Reporte.
        /// </summary>
        public ActionResult Reporte()
        {
            ValidarSessionUsuarios();
            try
            {
                RegistrosModel b = new RegistrosModel();

                b.EmpresaGeneradora = Convert.ToInt32(Request["empresa"]);
                b.Periodo = Convert.ToInt32(Request["periodo"]);

                //asignar empresa generadora por usuario
                string user = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                COES.MVC.Extranet.SeguridadServicio.EmpresaDTO[] op = servicio.ObtenerEmpresasPorUsuario(user);

                var empGeneradora = new List<Tuple<int, string>>();
                //empGeneradora.Add(new Tuple<int, string>(0, "(TODOS)"));

                foreach (COES.MVC.Extranet.SeguridadServicio.EmpresaDTO item in op)
                {
                    empGeneradora.Add(new Tuple<int, string>(item.EMPRCODI, item.EMPRNOMB));
                }
                ViewBag.empresaGeneradora = empGeneradora;

                //CboPeriodo
                RntPeriodoModel permodel = new RntPeriodoModel();
                permodel.ListaRntPeriodo = ntcse.ListComboRntPeriodos();
                //permodel.ListaRntPeriodo.Add(permodel.ListaComboTodos);
                //Iniciando Periodo Consulta
                if (permodel != null)
                {
                    b.Periodo = permodel.ListaRntPeriodo[0].PeriodoCodi;
                }
                ViewData["CboPeriodo"] = new SelectList((from s in permodel.ListaRntPeriodo select new { Periodocodi = s.PeriodoCodi, Perdnombre = ((s.PerdAnio != null) ? (s.PerdAnio + "-" + s.PerdSemestre) : s.PerdNombre) }), "Periodocodi", "Perdnombre", 0);

                return View(b);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(404, ConstantesResarcimiento.ErrorDeSistema);
            }

        }

        #endregion

        #region CargarExcel

        /// <summary>
        /// Opción reporte: Carga en la Grilla Handsontable el Excel que contiene la información
        /// de Resarcimientos de Punto de Entrega y Rechazo de Carga.
        /// </summary>
        [HttpPost]
        public JsonResult CargarExcel(FormCollection collection)
        {
            HttpPostedFileBase fileData = Request.Files[0];

            ExcelPackage pck = new ExcelPackage(fileData.InputStream);

            ExcelWorkbook workBook = pck.Workbook;

            string[][][] arrGrilla = new string[3][][];

            if (workBook != null)
            {
                ExcelWorksheet objHoja;

                if (workBook.Worksheets.Count > 0)
                {
                    objHoja = workBook.Worksheets.Single<ExcelWorksheet>(a => a.Name.ToUpper() == "LISTADO");

                    if (objHoja != null)
                    {
                        intFilas = objHoja.Dimension.End.Row - 2;

                        if (intFilas > 0)
                        {
                            arrGrilla[2] = new string[intFilas][];

                            for (int i = 0, j = intFilas; i < j; i++)
                            {
                                arrGrilla[2][i] = new string[16];

                                for (int m = 0, n = arrGrilla[2][i].Length; m < n; m++)
                                {
                                    object objValue = objHoja.GetValue(i + 3, m + 1);

                                    arrGrilla[2][i][m] = (objValue != null ? objValue.ToString() : null);
                                }
                            }
                        }
                    }

                    objHoja = workBook.Worksheets.Single<ExcelWorksheet>(a => a.Name.ToUpper() == "PE");

                    if (objHoja != null)
                    {
                        intFilas = objHoja.Dimension.End.Row - 7;

                        if (intFilas > 0)
                        {
                            arrGrilla[0] = new string[intFilas][];

                            for (int i = 0, j = intFilas; i < j; i++)
                            {
                                arrGrilla[0][i] = new string[27];

                                for (int m = 0, n = arrGrilla[0][i].Length; m < n; m++)
                                {
                                    object objValue = objHoja.GetValue(i + 8, m + 1);

                                    arrGrilla[0][i][m] = (objValue != null ? objValue.ToString() : null);
                                }
                            }
                        }
                    }

                    objHoja = workBook.Worksheets.Single<ExcelWorksheet>(a => a.Name.ToUpper() == "RC");

                    if (objHoja != null)
                    {
                        int intFilas = objHoja.Dimension.End.Row - 7;

                        if (intFilas > 0)
                        {
                            arrGrilla[1] = new string[intFilas][];

                            for (int i = 0, j = intFilas; i < j; i++)
                            {
                                arrGrilla[1][i] = new string[14];

                                for (int m = 0, n = arrGrilla[1][i].Length; m < n; m++)
                                {
                                    object objValue = objHoja.GetValue(i + 8, m + 1);

                                    arrGrilla[1][i][m] = (objValue != null ? objValue.ToString() : null);
                                }
                            }
                        }
                    }
                }
            }

            return Json(arrGrilla);
        }

        #endregion

        #region DescargarPlantillaExcel

        /// <summary>
        /// Opción Reporte: Permite descargar la plantilla Excel con el formato de información
        /// de Resarcimientos de Punto de Entrega y Rechazo de Carga.
        /// </summary>
        [HttpGet]
        public FileResult DescargarPlantillaExcel(FormCollection collection)
        {
            string ruta = base.PathFiles + ConstantesResarcimiento.PlantillaRC;
            byte[] buffer = FileServer.DownloadToArrayByte(ruta, string.Empty);
            string fileName = new FileInfo(ruta).Name;
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        #endregion

        #region ValidarGrillas

        /// <summary>
        /// Opción Reporte: Permite validar la información que se ha cargado a la grilla Handsontable
        /// Esta opción muestra un pop-up conteniendo el listado de las celdas que presentan inconsistencias
        /// para las hojas que contienen información de Punto de Entrega y rechazo de Carga
        /// </summary>
        public JsonResult ValidarGrillas(FormCollection collection)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string[][][] arrGrilla = jsSerializer.Deserialize<string[][][]>(collection[0]);

            int empGeneradora = Convert.ToInt32(collection["emprGeneradora"]);
            int periodo = Convert.ToInt32(collection["periodo"]);
            int puntoEntrega = Convert.ToInt32(collection["puntoEntrega"]);
            int found = 0;

            bool blnExito = true;
            bool[][][] arrEsValido = new bool[2][][];

            //Validar Empresa Generadora del Usuario
            string user = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
            COES.MVC.Extranet.SeguridadServicio.EmpresaDTO[] op = servicio.ObtenerEmpresasPorUsuario(user);
            foreach (COES.MVC.Extranet.SeguridadServicio.EmpresaDTO item in op)
                if (item.EMPRCODI == empGeneradora) found = 1;

            if (found == 0)
                throw new ArgumentNullException("Empresa Generadora no pertenece al Usuario");

            if (arrGrilla == null)
                throw new ArgumentNullException("No se han especificado las grillas.");

            if (arrGrilla.Length != 2)
                throw new ArgumentOutOfRangeException("No se han especificado las 2 grillas.");

            string[][] arrFila = arrGrilla[0];
            arrEsValido[0] = new bool[arrFila.Length][];
            if (arrFila.Length > 0)
            {
                resultValidacionPE = ntcse.ValidarGrillaPE(periodo, empGeneradora, puntoEntrega, arrFila, out  listErrores);

                for (int i = 0, j = arrFila.Length; i < j; i++)
                    arrEsValido[0][i] = resultValidacionPE[i].arrEsValido;
            }
            else
            {
                blnExito = false;
            }

            arrFila = arrGrilla[1];
            arrEsValido[1] = new bool[arrFila.Length][];
            if (arrFila.Length > 0)
            {
                resultValidacionRC = ntcse.ValidarGrillaRC(periodo, empGeneradora, puntoEntrega, arrFila, out  listErroresRC);

                for (int i = 0, j = arrFila.Length; i < j; i++)
                    arrEsValido[1][i] = resultValidacionRC[i].arrEsValido;

            }
            else
            {
                blnExito = false;
            }

            var arrErroresAll = listErrores.Union(listErroresRC).ToArray();

            object[] objResponse = {
                                                       blnExito,
                                                       arrEsValido,
                                                       arrErroresAll,
                                                   };

            return Json(objResponse);
        }

        #endregion

        #region Grabar

        /// <summary>
        /// Opción Reporte: Permite grabar la información de resarcimientos de Punto de Entrega y Rechazo de Carga
        /// que se ha cargado a la grilla Handsontable siempre que no tenga inconsistencias.
        /// </summary>
        public string grabarReporte(FormCollection collection)
        {

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string[][][] arrGrilla = jsSerializer.Deserialize<string[][][]>(collection[0]);
            int empGeneradora = Convert.ToInt32(collection["emprGeneradora"]);
            int periodo = Convert.ToInt32(collection["periodo"]);
            int puntoEntrega = Convert.ToInt32(collection["puntoEntrega"]);
            string user = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
            string result = "";

            if (arrGrilla == null)
            {
                throw new ArgumentNullException("No se han especificado las grillas.");
            }
            else if (arrGrilla.Length != 2)
            {
                throw new ArgumentOutOfRangeException("No se han especificado las 2 grillas.");
            }
            else
            {
                try
                {
                    int? idModulo = base.IdModulo;
                    result = ntcse.SaveReporte((int)idModulo, user, periodo, empGeneradora, puntoEntrega, arrGrilla[0], arrGrilla[1], out  listErrores, out  listErroresRC);
                    //Enviar Correo
                    List<AdminModuloDTO> ListaAdministradores = this.servicio.ObtenerModulo((int)idModulo).ListaAdministradores.ToList();
                    List<string> listaCorreos = new List<string>(ListaAdministradores.Count);
                    for (int i = 0; i < ListaAdministradores.Count; i++)
                        listaCorreos.Add(ListaAdministradores[i].UserEmail);

                    UserDTO userSession = (UserDTO)Session[DatosSesion.SesionUsuario];
                    List<string> userMail = new List<string>();
                    userMail.Add(userSession.UserEmail);

                    //COES.Base.Tools.Util.SendEmail(userMail, listaCorreos, "Notificacion de Carga Masiva Resarcimiento", result);

                }
                catch (Exception e)
                {
                    Log.Error("Error grabando PE y RC Error = " + e);
                    result = "Error grabando formatos Excel cargados";
                    throw new ArgumentNullException("Error grabando formatos Excel cargados!");
                }

            }

            return result;
        }
        #endregion

        #endregion

        #region [Reporte Historico]

        #region Default

        /// <summary>
        /// Muestra la ventana principal de Reporte Historico.
        /// </summary>
        public ActionResult ReporteHistorico(FormCollection collection)
        {

            try
            {
                RechazoCargaListModel listR = new RechazoCargaListModel();

                RegistrosModel b = new RegistrosModel();

                b.Titulo = Request["titleOpcion"];
                b.Key = Request["keyOpcion"];

                b.EmpresaGeneradora = Convert.ToInt32(Request["empresa"]);
                b.Periodo = Convert.ToInt32(Request["periodo"]);
                b.Cliente = Convert.ToInt32(Request["cliente"]);
                b.PEntrega = Convert.ToInt32(Request["pentrega"]);
                b.Ntension = Convert.ToInt32(Request["ntension"]);

                //asignar empresa generadora por usuario
                string user = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                //string user = "ppajan";
                COES.MVC.Extranet.SeguridadServicio.EmpresaDTO[] op = servicio.ObtenerEmpresasPorUsuario(user);

                //Ver todas las empresas
                EmpresasGeneradorasModel ermodel = new EmpresasGeneradorasModel();
                ermodel.ListaEmpresasGeneradoras = ntcse.ListSiEmpresasGeneral();
                ermodel.ListaEmpresasGeneradoras.Insert(0, ermodel.ListaComboTodos);

                //ViewBag.empresaGeneradora = new SelectList(ermodel.ListaEmpresasGeneradoras, "Emprcodi", "Emprnomb", 0);
                ViewData["CboEmpresasGeneradoras"] = new SelectList(ermodel.ListaEmpresasGeneradoras, "Emprcodi", "Emprnomb", 0);

                //CboPeriodo
                RntPeriodoModel permodel = new RntPeriodoModel();
                permodel.ListaRntPeriodo = ntcse.ListComboRntPeriodos();
                permodel.ListaRntPeriodo.Add(permodel.ListaComboTodos);
                //Iniciando Periodo Consulta
                if (permodel != null)
                {
                    b.Periodo = permodel.ListaRntPeriodo[0].PeriodoCodi;
                }
                ViewData["CboPeriodo"] = new SelectList((from s in permodel.ListaRntPeriodo select new { Periodocodi = s.PeriodoCodi, Perdnombre = ((s.PerdAnio != null) ? (s.PerdAnio + "-" + s.PerdSemestre) : s.PerdNombre) }), "Periodocodi", "Perdnombre", 0);

                int? empresaGeneradora = Convert.ToInt32(collection["empGeneradora"]);
                int periodo = Convert.ToInt32(collection["periodo"]);
                int puntoEntrega = Convert.ToInt32(collection["puntoEntrega"]);
                int codigoEnvio = Convert.ToInt32(collection["codigoEnvio"]);

                try
                {
                    if (User != null)
                    {
                        List<RntRegPuntoEntregaDTO> urs = new List<RntRegPuntoEntregaDTO>();

                        urs = ntcse.ListReporteCargaRntRegPuntoEntregas(empresaGeneradora, periodo, puntoEntrega, codigoEnvio);


                        listR.ListTable = new List<RntRegPuntoEntregaDTO>(urs.Count);
                        foreach (RntRegPuntoEntregaDTO item in urs)
                        {
                            RntPeriodoDTO periodoItem = this.ntcse.GetByIdRntPeriodo(item.PeriodoCodi);
                            listR.ListTable.Add(new RntRegPuntoEntregaDTO
                            {
                                EnvioCodi = item.EnvioCodi,
                                PeriodoCodi = item.PeriodoCodi,
                                PeriodoDesc = periodoItem.PerdAnio.ToString() + "-" + periodoItem.PerdSemestre.ToString(),
                                RegPuntoEntCodi = item.RegPuntoEntCodi,
                                BarrNombre = item.BarrNombre,
                                RpeUsuarioCreacion = item.RpeUsuarioCreacion,
                                RpeFechaCreacion = item.RpeFechaCreacion,
                                RpeEmpresaGeneradoraNombre = item.RpeEmpresaGeneradoraNombre

                            });

                        }

                    }
                }

                catch (Exception e)
                {
                    Log.Error("Error al obtener lista de URS Modo Operacions_URS");
                }

                return View(listR);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(404, ConstantesResarcimiento.ErrorDeSistema);
            }

        }
        #endregion

        #region Consultar

        /// <summary>
        /// Opción ReporteHistórico: Carga en la tabla los registros enviados con información 
        /// de Resarcimientos de Punto de Entrega y Rechazo de Carga.
        /// </summary>
        public JsonResult Consultar(FormCollection collection)
        {
            int empresaGeneradora = Convert.ToInt32(Request["empresa"]);
            int periodo = Convert.ToInt32(Request["periodo"]);
            int puntoEntrega = Convert.ToInt32(Request["puntoentrega"]);
            int codigoEnvio = Convert.ToInt32(Request["codigoEnvio"]);
            List<RntRegPuntoEntregaDTO> urs = new List<RntRegPuntoEntregaDTO>();
            string user;

            RechazoCargaListModel listR = new RechazoCargaListModel();
            try
            {
                if (User != null)
                {

                    urs = this.ntcse.ListReporteCargaRntRegPuntoEntregas(empresaGeneradora, periodo, puntoEntrega, codigoEnvio);

                    listR.ListTable = new List<RntRegPuntoEntregaDTO>(urs.Count);
                    foreach (RntRegPuntoEntregaDTO item in urs)
                    {
                        RntPeriodoDTO periodoItem = this.ntcse.GetByIdRntPeriodo(item.PeriodoCodi);
                        listR.ListTable.Add(new RntRegPuntoEntregaDTO
                        {
                            EnvioCodi = item.EnvioCodi,
                            PeriodoCodi = item.PeriodoCodi,
                            PeriodoDesc = periodoItem.PerdAnio.ToString() + "-" + periodoItem.PerdSemestre.ToString(),
                            RegPuntoEntCodi = item.RegPuntoEntCodi,
                            RpeUsuarioCreacion = item.RpeUsuarioCreacion,
                            RpeFechaCreacion = item.RpeFechaCreacion,
                            RpeEmpresaGeneradoraNombre = item.RpeEmpresaGeneradoraNombre

                        });
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error al obtener lista de URS Modo Operacions_URS");
            }

            object[] objResponse = {
                                               listR.ListTable
                                           };


            return Json(objResponse);
        }
        #endregion

        #region LeerGrillaBD

        /// <summary>
        /// Opción ReporteHistórico: Permite traer información de resarcimientos de Punto de Entrega y Rechazo de Carga
        /// de la Base de Datos teniendo como entrada el Código de Envío seleccionado.
        /// </summary>
        [HttpPost]
        public JsonResult LeerGrillaBD(FormCollection collection)
        {

            int codEnvio = Convert.ToInt32(collection["codigoenvio"]);

            string[][][] arrGrilla = new string[2][][];

            arrGrilla[0] = ntcse.ListReporteGrillaRntRegPuntoEntregas(codEnvio);
            arrGrilla[1] = ntcse.ListReporteGrillaRntRechazoCarga(codEnvio);

            return Json(arrGrilla);
        }

        #endregion


        #endregion

        #region [Metodos Generales]

        #region EmpresasDisponiblesParaUsuario

        /// <summary>
        /// Permite validar las empresas generadoras disponibles para el usuario.
        /// </summary>
        public JsonResult ValidarEmpresasGeneradoras(FormCollection collection)
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
                    if (item.EMPRCODI == emprgen || emprgen == 0)
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

        #region TraerParametros

        /// <summary>
        /// Permite traer información de los Combos para las páginas de Reporte.
        /// </summary>
        public JsonResult TraerParametros(FormCollection collection)
        {
            string[][] arrCliente = null;
            arrCliente = new string[2][];
            List<SiEmpresaDTO> listClientes = ntcse.ListEmpresasClientes();
            arrCliente[0] = new string[listClientes.Count];
            arrCliente[1] = new string[listClientes.Count];
            for (int i = 0; i < listClientes.Count; i++)
            {
                arrCliente[0][i] = listClientes[i].Emprnomb;
                arrCliente[1][i] = listClientes[i].Emprcodi.ToString();
            }

            string[] arrNivelTension = null;
            List<RntConfiguracionDTO> listNivelTension = ntcse.GetListParametroRep("REPORTE", "NIVELTENSION");
            arrNivelTension = new string[listNivelTension.Count];
            for (int i = 0; i < listNivelTension.Count; i++)
                arrNivelTension[i] = listNivelTension[i].ConfValor;

            string[] arrTipo = null;
            List<RntConfiguracionDTO> listTipo = ntcse.GetListParametroRep("REPORTE", "TIPOPE");
            arrTipo = new string[listTipo.Count];
            for (int i = 0; i < listTipo.Count; i++)
                arrTipo[i] = listTipo[i].ConfValor;

            string[] arrBool = null;
            List<RntConfiguracionDTO> listExFm = ntcse.GetListParametroRep("REPORTE", "EXONERADO_FM");
            arrBool = new string[listExFm.Count];
            for (int i = 0; i < listExFm.Count; i++)
                arrBool[i] = listExFm[i].ConfValor;

            string[] arrEmpresa = null;
            List<SiEmpresaDTO> listEmpresas = ntcse.ListSiEmpresasGeneral();
            arrEmpresa = new string[listEmpresas.Count];
            for (int i = 0; i < listEmpresas.Count; i++)
                arrEmpresa[i] = listEmpresas[i].Emprnomb;

            string[][] arrBarra = null;
            arrBarra = new string[2][];
            List<RntRegPuntoEntregaDTO> listBarras = ntcse.ListBarras();

            arrBarra[0] = new string[listBarras.Count];
            arrBarra[1] = new string[listBarras.Count];
            for (int i = 0; i < listBarras.Count; i++)
            {
                arrBarra[0][i] = listBarras[i].BarrNombre;
                arrBarra[1][i] = listBarras[i].Barrcodi.ToString();
            }

            string[] arrCodigoEventoCOES = null;
            List<EveEventoDTO> listEventos = ntcse.ListEventos(Convert.ToInt32(collection["puntoEntrega"]));
            arrCodigoEventoCOES = new string[listEventos.Count];
            for (int i = 0; i < listEventos.Count; i++)
                arrCodigoEventoCOES[i] = listEventos[i].CodEve;

            return Json(new object[] { arrCliente, arrBarra, arrNivelTension, arrTipo, arrBool, arrCodigoEventoCOES, arrEmpresa });
        }

        #endregion

        #region DescargarExcel

        /// <summary>
        /// Permite descargar a un archivo Excel la información de resarcimientos de Punto de Entrega y Rechazo de Carga
        /// almacenada y visualizada en la grilla Handsontable.
        /// </summary>
        [HttpGet]
        public FileResult DescargarExcel(FormCollection collection)
        {
            string strArchivoTemporal = Request["archivo"];

            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);


                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("Archivo_{0:yyyymmdd_HHmmss}.xlsx", DateTime.Now);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        #endregion

        #region GenerarExcel

        /// <summary>
        /// Recorre la grilla Handsonatble para recuperar valores de cada celda y luego sean usados 
        /// para generar un archivo Excel.
        /// </summary>
        [HttpPost]

        public string GenerarExcel(FormCollection collection)
        {
            string strArchivoTemporal = "";

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

            string[][][] arrGrilla = jsSerializer.Deserialize<string[][][]>(collection[0]);

            bool[][][] arrEsValido = new bool[2][][];

            if (arrGrilla == null)
            {
                throw new ArgumentNullException("No se han especificado las grillas.");
            }

            if (arrGrilla.Length != 2)
            {
                throw new ArgumentOutOfRangeException("No se han especificado las 2 grillas.");
            }

            using (ExcelPackage pck = new ExcelPackage())
            {

                ExcelWorkbook workBook = pck.Workbook;

                int intFilaInicio = 8,
                    intFilaFin = 0,
                    intColumnaInicio = 1,
                    intColumnaFin = 0;

                #region Hoja - PE

                ExcelWorksheet objHojaPE = workBook.Worksheets.Add("PE");

                objHojaPE.View.ShowGridLines = false;

                intColumnaFin = intColumnaInicio + 26;

                #region Configurar

                for (int i = intColumnaInicio, j = intColumnaFin; i <= j; i++)
                {
                    ExcelColumn xlsColumn = objHojaPE.Column(i);
                    xlsColumn.Style.Font.Name = "arial";
                    xlsColumn.Style.Font.Size = 9;
                    xlsColumn.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    xlsColumn.Style.WrapText = true;



                    if ((i >= intColumnaInicio && i <= intColumnaInicio + 9) || (i >= intColumnaInicio + 24 && i <= intColumnaInicio + 27))
                    {
                        objHojaPE.Cells[intFilaInicio - 2, i, intFilaInicio - 1, i].Merge = true;
                    }
                    else if (i % 2 != 0)
                    {
                        objHojaPE.Cells[intFilaInicio - 2, i, intFilaInicio - 2, i + 1].Merge =
                            objHojaPE.Cells[intFilaInicio - 1, i].Merge =
                            objHojaPE.Cells[intFilaInicio - 1, i + 1].Merge = true;
                    }
                }

                ExcelRange xlsRangeCabeceraPE = objHojaPE.Cells[intFilaInicio - 2, intColumnaInicio, intFilaInicio - 1, intColumnaFin];

                xlsRangeCabeceraPE.Style.Font.Size = 10;
                xlsRangeCabeceraPE.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                xlsRangeCabeceraPE.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(146, 205, 220));
                xlsRangeCabeceraPE.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                xlsRangeCabeceraPE.Style.Border.Top.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                xlsRangeCabeceraPE.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                xlsRangeCabeceraPE.Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                xlsRangeCabeceraPE.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                xlsRangeCabeceraPE.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                xlsRangeCabeceraPE.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                xlsRangeCabeceraPE.Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                xlsRangeCabeceraPE.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                objHojaPE.Column(intColumnaInicio + 0).Width = 9;
                objHojaPE.Column(intColumnaInicio + 1).Width = 24;
                objHojaPE.Column(intColumnaInicio + 2).Width = 18;
                objHojaPE.Column(intColumnaInicio + 3).Width = 8;
                objHojaPE.Column(intColumnaInicio + 4).Width = 16;
                objHojaPE.Column(intColumnaInicio + 5).Width = 16;
                objHojaPE.Column(intColumnaInicio + 6).Width = 22;
                objHojaPE.Column(intColumnaInicio + 7).Width = 13;
                objHojaPE.Column(intColumnaInicio + 8).Width =
                    objHojaPE.Column(intColumnaInicio + 9).Width = 6;
                objHojaPE.Column(intColumnaInicio + 10).Width =
                    objHojaPE.Column(intColumnaInicio + 11).Width =
                    objHojaPE.Column(intColumnaInicio + 12).Width =
                    objHojaPE.Column(intColumnaInicio + 13).Width = 16;
                objHojaPE.Column(intColumnaInicio + 14).Width = 11;
                objHojaPE.Column(intColumnaInicio + 15).Width = 8;
                objHojaPE.Column(intColumnaInicio + 16).Width = 11;
                objHojaPE.Column(intColumnaInicio + 17).Width = 8;
                objHojaPE.Column(intColumnaInicio + 18).Width = 11;
                objHojaPE.Column(intColumnaInicio + 19).Width = 8;
                objHojaPE.Column(intColumnaInicio + 20).Width = 11;
                objHojaPE.Column(intColumnaInicio + 21).Width = 8;
                objHojaPE.Column(intColumnaInicio + 22).Width = 11;
                objHojaPE.Column(intColumnaInicio + 23).Width = 8;

                objHojaPE.Column(intColumnaInicio + 24).Width = 50;
                objHojaPE.Column(intColumnaInicio + 25).Width = 10;
                objHojaPE.Column(intColumnaInicio + 26).Width = 14;

                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 0, "Punto de Entrega");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 1, "CLIENTE");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 2, "Barra");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 3, "Nivel de Tensión");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 4, "Energía Semestral (KW.h)");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 5, "Incremento de Tolerancias - Sector DIstribucion");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 6, "Tipo");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 7, "Exonerado o Fuerza Mayor");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 8, "Ni");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 9, "Ki");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 10, "Tiempo Ejecutado");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 10, "Fecha Hora Inicio");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 11, "Fecha Hora Fin");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 12, "Tiempo Programado");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 12, "Fecha Hora Inicio");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 13, "Fecha Hora Fin");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 14, "Responsable 1");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 14, "Empresa");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 15, "%");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 16, "Responsable 2");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 16, "Empresa");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 17, "%");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 18, "Responsable 3");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 18, "Empresa");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 19, "%");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 20, "Responsable 4");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 20, "Empresa");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 21, "%");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 22, "Responsable 5");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 22, "Empresa");
                objHojaPE.SetValue(intFilaInicio - 1, intColumnaInicio + 23, "%");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 24, "Causa Resumida de Interrupción");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 25, "Ei / E");
                objHojaPE.SetValue(intFilaInicio - 2, intColumnaInicio + 26, "Resarcimiento (US$)");

                #endregion

                #region Poblar Filas

                string[][] arrFila = arrGrilla[0];


                Log.Info("PE arrFila " + arrFila);

                if (arrFila.Length > 0)
                {
                    intFilaFin = arrFila.Length + intFilaInicio - 1;

                    bool blnEsNuevoID = true;
                    string strID = null,
                        strCliente = null,
                        strBarra = null,
                        strNivelTension = null,
                        strEnergiaSemestral = null,
                        strIncrementoTolerancia = null;

                    objHojaPE.Cells[intFilaInicio, intColumnaInicio, intFilaFin, intColumnaInicio].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objHojaPE.Cells[intFilaInicio, intColumnaInicio, intFilaFin, intColumnaInicio].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));

                    for (int i = 0, j = arrFila.Length; i < j; i++)
                    {
                        string[] arrCelda = arrFila[i];

                        for (int m = 0, n = arrCelda.Length; m < n; m++)
                        {
                            string strValue = arrCelda[m];

                            if (strValue != null)
                            {
                                strValue = strValue.Trim();
                            }

                            switch (m)
                            {
                                case 0:
                                    blnEsNuevoID = ((strID != null && strID.Length > 0) && strID != strValue);

                                    if (blnEsNuevoID)
                                    {
                                        objHojaPE.Cells[i + intFilaInicio - 1, intColumnaInicio + m, i + intFilaInicio - 1, intColumnaFin + m].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        objHojaPE.Cells[i + intFilaInicio - 1, intColumnaInicio + m, i + intFilaInicio - 1, intColumnaFin + m].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                                    }

                                    strID = strValue;

                                    break;
                                case 1:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strCliente != null && strCliente == strValue)))
                                    {
                                        objHojaPE.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaPE.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strCliente = strValue;
                                    }

                                    break;
                                case 2:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strBarra != null && strBarra == strValue)))
                                    {
                                        objHojaPE.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaPE.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strBarra = strValue;
                                    }

                                    break;
                                case 3:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strNivelTension != null && strNivelTension == strValue)))
                                    {
                                        objHojaPE.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaPE.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strNivelTension = strValue;
                                    }

                                    break;
                                case 4:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strEnergiaSemestral != null && strEnergiaSemestral == strValue)))
                                    {
                                        objHojaPE.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaPE.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strEnergiaSemestral = strValue;
                                    }

                                    break;
                                case 5:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strIncrementoTolerancia != null && strIncrementoTolerancia == strValue)))
                                    {
                                        objHojaPE.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaPE.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strIncrementoTolerancia = strValue;
                                    }

                                    break;

                            }

                            switch (m)
                            {
                                case 0:
                                    if (strValue != null && strValue.Trim().Length > 0)
                                    {
                                        int intValue;

                                        if (int.TryParse(strValue, out intValue))
                                        {
                                            objHojaPE.SetValue(i + intFilaInicio, m + intColumnaInicio, intValue);
                                        }
                                        else
                                        {
                                            objHojaPE.SetValue(i + intFilaInicio, m + intColumnaInicio, strValue);
                                        }
                                    }
                                    break;
                                case 5:
                                case 7:
                                    if (strValue == "NO") strValue = "No";
                                    if (strValue == "SI") strValue = "Si";
                                    objHojaPE.SetValue(i + intFilaInicio, m + intColumnaInicio, strValue);
                                    break;
                                case 4:
                                case 8:
                                case 9:
                                case 15:
                                case 17:
                                case 19:
                                case 21:
                                case 23:
                                case 25:
                                case 26:
                                    if (strValue != null && strValue.Trim().Length > 0)
                                    {
                                        double dblValue;

                                        if (double.TryParse(strValue, out dblValue))
                                        {
                                            objHojaPE.SetValue(i + intFilaInicio, m + intColumnaInicio, dblValue);
                                        }
                                        else
                                        {
                                            objHojaPE.SetValue(i + intFilaInicio, m + intColumnaInicio, strValue);
                                        }
                                    }
                                    break;
                                default:
                                    objHojaPE.SetValue(i + intFilaInicio, m + intColumnaInicio, strValue);
                                    break;
                            }
                        }
                    }

                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 0, intFilaFin, intColumnaInicio + 0].Style.HorizontalAlignment =
                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 3, intFilaFin, intColumnaInicio + 4].Style.HorizontalAlignment =
                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 7, intFilaFin, intColumnaInicio + 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 4, intFilaFin, intColumnaInicio + 4].Style.Numberformat.Format = "#";

                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 15, intFilaFin, intColumnaInicio + 15].Style.Numberformat.Format =
                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 17, intFilaFin, intColumnaInicio + 17].Style.Numberformat.Format =
                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 19, intFilaFin, intColumnaInicio + 19].Style.Numberformat.Format =
                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 21, intFilaFin, intColumnaInicio + 21].Style.Numberformat.Format =
                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 23, intFilaFin, intColumnaInicio + 23].Style.Numberformat.Format = "#%";

                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 25, intFilaFin, intColumnaInicio + 25].Style.Numberformat.Format = "#0.00%";

                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 26, intFilaFin, intColumnaInicio + 26].Style.Numberformat.Format = "#0.00";

                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 25, intFilaFin, intColumnaFin].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    objHojaPE.Cells[intFilaInicio, intColumnaInicio + 25, intFilaFin, intColumnaFin].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));

                    objHojaPE.Cells[intFilaInicio, intColumnaFin, intFilaFin, intColumnaFin].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objHojaPE.Cells[intFilaInicio, intColumnaFin, intFilaFin, intColumnaFin].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));

                    objHojaPE.Cells[intFilaFin, intColumnaInicio, intFilaFin, intColumnaFin].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objHojaPE.Cells[intFilaFin, intColumnaInicio, intFilaFin, intColumnaFin].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                }

                #endregion

                #endregion

                #region Hoja - RC

                ExcelWorksheet objHojaRC = workBook.Worksheets.Add("RC");

                objHojaRC.View.ShowGridLines = false;

                intColumnaFin = intColumnaInicio + 13;//20 agosto 2016

                #region Configurar

                for (int i = intColumnaInicio, j = intColumnaFin; i <= j; i++)
                {
                    ExcelColumn xlsColumn = objHojaRC.Column(i);
                    xlsColumn.Style.Font.Name = "arial";
                    xlsColumn.Style.Font.Size = 9;
                    xlsColumn.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    xlsColumn.Style.WrapText = true;

                    if ((i >= intColumnaInicio && i <= intColumnaInicio + 3) || i == intColumnaInicio + 6 || i == intColumnaInicio + 7 || (i >= intColumnaInicio + 10 && i <= intColumnaInicio + 13))
                    {
                        objHojaRC.Cells[intFilaInicio - 2, i, intFilaInicio - 1, i].Merge = true;
                    }
                    else if (i == intColumnaInicio + 4 || i == intColumnaInicio + 8)
                    {
                        objHojaRC.Cells[intFilaInicio - 2, i, intFilaInicio - 2, i + 1].Merge = true;
                    }
                }

                ExcelRange xlsRangeCabeceraRC = objHojaRC.Cells[intFilaInicio - 2, intColumnaInicio, intFilaInicio - 1, intColumnaFin];

                xlsRangeCabeceraRC.Style.Font.Size = 10;
                xlsRangeCabeceraRC.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                xlsRangeCabeceraRC.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(146, 205, 220));
                xlsRangeCabeceraRC.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                xlsRangeCabeceraRC.Style.Border.Top.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                xlsRangeCabeceraRC.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                xlsRangeCabeceraRC.Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                xlsRangeCabeceraRC.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                xlsRangeCabeceraRC.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                xlsRangeCabeceraRC.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                xlsRangeCabeceraRC.Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                xlsRangeCabeceraRC.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                objHojaRC.Column(intColumnaInicio + 0).Width = 9;
                objHojaRC.Column(intColumnaInicio + 1).Width = 24;
                objHojaRC.Column(intColumnaInicio + 2).Width = 18;
                objHojaRC.Column(intColumnaInicio + 3).Width = 11;
                objHojaRC.Column(intColumnaInicio + 4).Width =
                objHojaRC.Column(intColumnaInicio + 5).Width =
                objHojaRC.Column(intColumnaInicio + 6).Width = 9;
                objHojaRC.Column(intColumnaInicio + 7).Width = 16;
                objHojaRC.Column(intColumnaInicio + 8).Width =
                objHojaRC.Column(intColumnaInicio + 9).Width = 16;
                objHojaRC.Column(intColumnaInicio + 10).Width = 6.5;
                objHojaRC.Column(intColumnaInicio + 11).Width = 12;
                objHojaRC.Column(intColumnaInicio + 12).Width = 10;
                objHojaRC.Column(intColumnaInicio + 13).Width = 14;

                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 0, "Punto de Entrega");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 1, "CLIENTE");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 2, "Barra");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 3, "Código Alimentador");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 4, "SED");
                objHojaRC.SetValue(intFilaInicio - 1, intColumnaInicio + 4, "Nombre");
                objHojaRC.SetValue(intFilaInicio - 1, intColumnaInicio + 5, "kV");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 6, "ENS f (kW.h)");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 7, "Código COES del Evento");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 8, "Interrupción");
                objHojaRC.SetValue(intFilaInicio - 1, intColumnaInicio + 8, "Inicio");
                objHojaRC.SetValue(intFilaInicio - 1, intColumnaInicio + 9, "Fin");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 10, "Pk (kW)");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 11, "Compensable");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 12, "ENS f, k (kWh)");
                objHojaRC.SetValue(intFilaInicio - 2, intColumnaInicio + 13, "Resarcimiento (US$)");

                #endregion

                #region Poblar Filas

                arrFila = arrGrilla[1];

                if (arrFila.Length > 0)
                {
                    intFilaFin = arrFila.Length + intFilaInicio - 1;

                    bool blnEsNuevoID = true;
                    string strID = null,
                        strCliente = null,
                        strBarra = null,
                        strCodigoAlimentador = null,
                        strSEDNombre = null,
                        strSEDkV = null,
                        strENS = null;

                    objHojaRC.Cells[intFilaInicio, intColumnaInicio, intFilaFin, intColumnaInicio].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objHojaRC.Cells[intFilaInicio, intColumnaInicio, intFilaFin, intColumnaInicio].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));

                    for (int i = 0, j = arrFila.Length; i < j; i++)
                    {
                        string[] arrCelda = arrFila[i];

                        for (int m = 0, n = arrCelda.Length; m < n; m++)
                        {
                            string strValue = arrCelda[m];

                            if (strValue != null)
                            {
                                strValue = strValue.Trim();
                            }

                            switch (m)
                            {
                                case 0:
                                    blnEsNuevoID = ((strID != null && strID.Length > 0) && strID != strValue);

                                    if (blnEsNuevoID)
                                    {
                                        objHojaRC.Cells[i + intFilaInicio - 1, intColumnaInicio + m, i + intFilaInicio - 1, intColumnaFin + m].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        objHojaRC.Cells[i + intFilaInicio - 1, intColumnaInicio + m, i + intFilaInicio - 1, intColumnaFin + m].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                                    }

                                    strID = strValue;

                                    break;
                                case 1:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strCliente != null && strCliente == strValue)))
                                    {
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strCliente = strValue;
                                    }

                                    break;
                                case 2:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strBarra != null && strBarra == strValue)))
                                    {
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strBarra = strValue;
                                    }

                                    break;
                                case 3:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strCodigoAlimentador != null && strCodigoAlimentador == strValue)))
                                    {
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strCodigoAlimentador = strValue;
                                    }

                                    break;
                                case 4:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strSEDNombre != null && strSEDNombre == strValue)))
                                    {
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strSEDNombre = strValue;
                                    }

                                    break;
                                case 5:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strSEDkV != null && strSEDkV == strValue)))
                                    {
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strSEDkV = strValue;
                                    }

                                    break;
                                case 6:
                                    if (!blnEsNuevoID && ((strValue == null || strValue.Length == 0) || (strENS != null && strENS == strValue)))
                                    {
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        objHojaRC.Cells[i + intFilaInicio, intColumnaInicio + m, i + intFilaInicio, intColumnaInicio + m].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));

                                        strValue = null;
                                    }
                                    else
                                    {
                                        strENS = strValue;
                                    }

                                    break;
                            }

                            switch (m)
                            {
                                case 0:
                                    if (strValue != null && strValue.Trim().Length > 0)
                                    {
                                        int intValue;

                                        if (int.TryParse(strValue, out intValue))
                                        {
                                            objHojaRC.SetValue(i + intFilaInicio, m + intColumnaInicio, intValue);
                                        }
                                        else
                                        {
                                            objHojaRC.SetValue(i + intFilaInicio, m + intColumnaInicio, strValue);
                                        }
                                    }
                                    break;
                                case 5:
                                case 6:
                                case 10:
                                case 11:
                                case 12:
                                case 13:
                                    if (strValue != null && strValue.Trim().Length > 0)
                                    {
                                        double dblValue;

                                        if (double.TryParse(strValue, out dblValue))
                                        {
                                            objHojaRC.SetValue(i + intFilaInicio, m + intColumnaInicio, dblValue);
                                        }
                                        else
                                        {
                                            objHojaRC.SetValue(i + intFilaInicio, m + intColumnaInicio, strValue);
                                        }
                                    }
                                    break;
                                default:
                                    objHojaRC.SetValue(i + intFilaInicio, m + intColumnaInicio, strValue);
                                    break;
                            }
                        }
                    }

                    objHojaRC.Cells[intFilaInicio, intColumnaInicio + 0, intFilaFin, intColumnaInicio + 0].Style.HorizontalAlignment =
                    objHojaRC.Cells[intFilaInicio, intColumnaInicio + 8, intFilaFin, intColumnaInicio + 9].Style.HorizontalAlignment =
                    objHojaRC.Cells[intFilaInicio, intColumnaInicio + 11, intFilaFin, intColumnaInicio + 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    objHojaRC.Cells[intFilaInicio, intColumnaInicio + 6, intFilaFin, intColumnaInicio + 6].Style.Numberformat.Format = "#,##0";

                    objHojaRC.Cells[intFilaInicio, intColumnaInicio + 10, intFilaFin, intColumnaInicio + 10].Style.Numberformat.Format = "#";
                    objHojaRC.Cells[intFilaInicio, intColumnaInicio + 11, intFilaFin, intColumnaInicio + 11].Style.Numberformat.Format =
                    objHojaRC.Cells[intFilaInicio, intColumnaInicio + 12, intFilaFin, intColumnaInicio + 12].Style.Numberformat.Format = "#,##0.00";
                    objHojaRC.Cells[intFilaInicio, intColumnaInicio + 13, intFilaFin, intColumnaInicio + 13].Style.Numberformat.Format = "#,##0.00";

                    objHojaRC.Cells[intFilaInicio, intColumnaInicio + 12, intFilaFin, intColumnaFin].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    objHojaRC.Cells[intFilaInicio, intColumnaInicio + 12, intFilaFin, intColumnaFin].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 192, 0));

                    objHojaRC.Cells[intFilaInicio, intColumnaFin, intFilaFin, intColumnaFin].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objHojaRC.Cells[intFilaInicio, intColumnaFin, intFilaFin, intColumnaFin].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));

                    objHojaRC.Cells[intFilaFin, intColumnaInicio, intFilaFin, intColumnaFin].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objHojaRC.Cells[intFilaFin, intColumnaInicio, intFilaFin, intColumnaFin].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0));
                }

                #endregion

                #endregion

                strArchivoTemporal = System.IO.Path.GetTempFileName();

                pck.SaveAs(new FileInfo(strArchivoTemporal));
            }

            return strArchivoTemporal;
        }


        #endregion

        #endregion

        #endregion

    }
}
