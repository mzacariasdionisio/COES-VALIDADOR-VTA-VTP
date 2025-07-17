using COES.Base.Core;
using COES.MVC.Intranet.Areas.Siosein.Models;
using COES.MVC.Intranet.Areas.Siosein.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.SIOSEIN;
using System.IO.Compression;
using System.IO;
using OfficeOpenXml;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.Sic;
using System.Globalization;

namespace COES.MVC.Intranet.Areas.Siosein.Controllers
{
    public class NumeralesController : BaseController
    {
        //
        // GET: /Siosein/Numerales/
        SIOSEINAppServicio servicio = new SIOSEINAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(NumeralesController));
        private static string NameController = "NumeralesController";
        private static List<EstadoModel> ListaEstadoSistemaA = new List<EstadoModel>();

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

        /// <summary>
        /// listado ventos del controller
        /// </summary>
        public NumeralesController()
        {
            ListaEstadoSistemaA = new List<EstadoModel>();
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "0", EstadoDescripcion = "NO" });
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "SÍ" });
        }

        #region Propiedades
        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[DatosSesionSiosein.SesionNombreArchivo] != null) ?
                    Session[DatosSesionSiosein.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionSiosein.SesionNombreArchivo] = value; }
        }

        public List<DiaValoresCostoMarginal> ListaCostoMarginal
        {
            get
            {
                return (Session[DatosSesionSiosein.SesionListaCostoMarginal] != null) ?
                    (List<DiaValoresCostoMarginal>)Session[DatosSesionSiosein.SesionListaCostoMarginal] : new List<DiaValoresCostoMarginal>();
            }
            set
            {
                Session[DatosSesionSiosein.SesionListaCostoMarginal] = value;
            }
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Descargar archivo excel
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteXls(string nameFile)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesSioSein.PathArchivoExcel;
            string fullPath = ruta + nameFile;
            return File(fullPath, ConstantesSioSein.AppExcel, nameFile);
        }

        #region Costos marginales mensual

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CostoMarginalMensual()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM yyyy");
            model.Fecha = DateTime.Now.ToString(ConstantesBase.FormatoFechaBase);
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult ViewLogAuditoriaMen(string mesAnio)
        {
            SioseinModel model = new SioseinModel();

            DateTime f_ = DateTime.Parse(ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/"));
            var ListaMeenvio = servicio.GetListaMeEnvioByFdat(ConstantesSioSein.FdatcodiCMCPmen, f_).OrderByDescending(x => x.Enviofecha).ToList();
            model.Resultado = servicio.ListaMeenvioHtml(ListaMeenvio, 1);

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesSioSein.PathArchivoExcel;

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        this.DeleteTmpZip(sNombreArchivo, path + sNombreArchivo, path);
                    }
                    file.SaveAs(path + sNombreArchivo);
                    this.NombreFile = sNombreArchivo;
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //log.Error("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Funcion de eliminacion de zip
        /// </summary>
        /// <param name="NombreFile"></param>
        /// <param name="file"></param>
        /// <param name="path"></param>
        public void DeleteTmpZip(string NombreFile, string file, string path)
        {
            #region Eliminamos Carpeta Temporal (Archivo y Carpetas extraidas del ZIP)

            if (NombreFile.ToLower().Contains(".zip"))
            {
                List<string> archivos = new List<string>();
                using (FileStream zipToOpen = new FileStream(file, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        for (int i = 0; i < archive.Entries.Count; i++)
                        {
                            foreach (var item in archive.Entries)
                            {
                                archivos.Add(item.ToString());
                                item.Delete();
                                break; //needed to break out of the loop
                            }

                            i = -1;
                        }
                    }
                }
                System.IO.File.Delete(file);
                List<string> directorys = new List<string>();
                string direc = string.Empty;
                foreach (var d in archivos)
                {
                    if (d.Contains("/"))
                    {
                        if (direc.Split('/')[0] != d.Split('/')[0])
                        {
                            System.IO.DirectoryInfo di = new DirectoryInfo(path + d);

                            foreach (FileInfo de in di.GetFiles())
                            {
                                de.Delete();
                            }
                            foreach (DirectoryInfo dir in di.GetDirectories())
                            {
                                dir.Delete(true);
                            }
                            direc = d;
                            directorys.Add(direc.Split('/')[0]);
                        }
                    }
                    else { System.IO.File.Delete(path + d); }
                }
                foreach (var d in directorys)
                {
                    System.IO.Directory.Delete(path + d);
                }
            }
            #endregion
        }

        /// <summary>
        /// LeerFileUpArchivo - Oliver
        /// Metodo de Lectura ZIP
        /// </summary>
        /// <param name="fecha">fecha de consulta</param>
        /// <returns>1,2,3,4,...</returns>
        public PartialViewResult LeerFileUpArchivo(string fecha)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesSioSein.PathArchivoExcel;
            var arrF = fecha.Split(' ');
            DateTime f_ = new DateTime(int.Parse(arrF[1]), int.Parse(arrF[0]), 1);
            List<BarraDTO> barras;
            SioseinModel model = new SioseinModel();
            string file = path + NombreFile;
            string diasnocargados = "";
            string result1 = string.Empty, result2 = string.Empty, result3 = string.Empty, message_ = string.Empty;
            int nrodias = 0;
            List<string> listaComment = new List<string>();
            List<string> listaDias = new List<string>();
            this.ListaCostoMarginal = new List<DiaValoresCostoMarginal>();
            bool mostrarCarga = true;
            if (NombreFile.Contains(".zip"))
            {
                ZipFile.ExtractToDirectory(file, path);
                model.TipoEmpresas = new List<string>();

                //Leemos el archivo ZIP
                using (ZipArchive archive = ZipFile.OpenRead(file))
                {
                    string mes = f_.ToString("MM");

                    var existeMes = archive.Entries.Where(x => x.FullName.Contains(mes + ".xls")).ToList();
                    if (existeMes.Count > 0)
                    {
                        int nCol = 0;

                        for (int z = 1; z <= f_.AddMonths(1).AddDays(-1).Day; z++)
                        {
                            string dia_ = "0" + z;
                            var existeDia = archive.Entries.Where(x => x.FullName.Contains(dia_.Substring(dia_.Length - 2) + mes + ".xls")).ToList();
                            if (existeDia.Count > 0)
                            {
                                string path_ = path + existeDia.First().FullName;
                                FileInfo fileInfo = new FileInfo(path_);
                                DiaValoresCostoMarginal diacm = new DiaValoresCostoMarginal();
                                diacm.FechaDia = new DateTime(int.Parse(arrF[1]), int.Parse(arrF[0]), z);
                                barras = servicio.ListTrnBarras();
                                ProcesarArchivoDiarioCM(barras, fileInfo, listaComment, diacm, existeDia[0].Name, ref mostrarCarga, ref nCol);
                                this.ListaCostoMarginal.Add(diacm);
                                listaDias.Add(dia_.Substring(dia_.Length - 2));
                                nrodias++;
                            }
                            else
                            {
                                diasnocargados = (diasnocargados == "") ? z.ToString() : diasnocargados + "," + z.ToString();
                            }
                        } // Fin de archivo por dia

                    }
                    else { message_ = "Los archivos no concuerdan con la fecha de consulta"; }
                }
            }
            else { message_ = "El archivo a cargar no es un archivo ZIP"; }

            //Eliminamos Carpeta Temporal (Archivo y Carpetas extraidas del ZIP)
            this.DeleteTmpZip(NombreFile, file, path);

            if (listaComment.Count > 0)
            {
                model.Mensaje = message_;
            }
            if (mostrarCarga && listaDias.Count > 0)
            {
                SaveCM(fecha, string.Join(",", listaDias));
                if (diasnocargados != "")
                {
                    listaComment.Add("<font color='green'>Carga de archivos correcta</font> &nbsp;&nbsp;<font color='red'>Los siguientes días no fueron cargados : " + diasnocargados + " </font>");
                }
                else
                    listaComment.Add("<font color='green'>Carga de archivos correcta</font>");
            }
            model.TipoEmpresas = listaComment;
            model.MostrarCarga = mostrarCarga;
            if (listaDias.Count == 0)
            {
                model.Mensaje = message_;
            }
            else
            {
                model.DiasNoCargados = diasnocargados;
            }
            model.DiasMes = string.Join(",", listaDias);
            model.NRegistros = nrodias;

            System.IO.Directory.CreateDirectory(path);

            return PartialView(model);
        }

        /// <summary>
        /// Carga un archivo xls de barras a Lista de Matriz diaria
        /// </summary>
        /// <param name="cabecera"></param>
        /// <param name="fileInfo"></param>
        /// <param name="listaComment"></param>
        /// <param name="diacm"></param>
        /// <param name="archivo"></param>
        /// <param name="mostrarCarga"></param>
        /// <param name="nCol"></param>
        public void ProcesarArchivoDiarioCM(List<BarraDTO> cabecera, FileInfo fileInfo, List<string> listaComment, DiaValoresCostoMarginal diacm, string archivo, ref bool mostrarCarga, ref int nCol)
        {
            bool cabec = false;
            DateTime fechaFila;
            int nFil = 48, rowsHead = 1, cc = 0, pestanias = 3, idBarra = 0;
            string nombreBarra = string.Empty;
            bool diacorrecto = true;
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                for (int p = 1; p <= pestanias; p++)
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[p];
                    if (p == 1)
                    {
                        nCol = ToolsSiosein.GetNroColumnasExcelCmg(ws) - 1;
                        diacm.IdCabecera = new int[nCol];
                        diacm.Matriz1 = ToolsSiosein.InicializaMatriz(rowsHead, nFil, nCol);
                        diacm.Matriz2 = ToolsSiosein.InicializaMatriz(rowsHead, nFil, nCol);
                        diacm.Matriz3 = ToolsSiosein.InicializaMatriz(rowsHead, nFil, nCol);
                    }
                    else
                    {
                        if (nCol != ToolsSiosein.GetNroColumnasExcelCmg(ws) - 1)
                        {
                            // Error - numero de columnas difieren en cada pestaña.
                        }
                    }

                    /// Verificar Formato

                    for (int i = 3; i < nFil + (cabec ? 3 : 4); i++)
                    {
                        for (int j = 1; j < nCol + 1; j++)
                        {
                            string valor = string.Empty;
                            bool condicion = true;
                            if (((j + 1 == 2 || i + (cabec ? rowsHead : 0) == 3) && ws.Cells[i + (cabec ? rowsHead : 0), j + 1].Value == null))
                                condicion = false;

                            if(condicion)
                            {
                                if (!cabec && i == 3)
                                {
                                    if (j == 1)
                                    {
                                        valor = ws.Cells[i, j + 1].Value.ToString();

                                    }
                                    else /// Lee cabecera barras
                                    {
                                        nombreBarra = ws.Cells[i + (cabec ? rowsHead : 0), j + 1].Value.ToString();
                                        idBarra = ToolsSiosein.GetIdBarra(cabecera, nombreBarra);
                                        if (idBarra != 0)
                                        {
                                            diacm.IdCabecera[j - 1] = idBarra;
                                            valor = nombreBarra;
                                        }
                                        else
                                        {
                                            /// Mensaje -- Barra no existe
                                            listaComment.Add("Barra: " + nombreBarra + " no existe en archivo : " + archivo);
                                            mostrarCarga = false;
                                            diacorrecto = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (j == 1)
                                    {

                                        valor = ws.Cells[i + (cabec ? rowsHead : 0), j + 1].Value.ToString();
                                        // Valida Fecha
                                        try
                                        {
                                            int iinicio = (i - (cabec ? rowsHead : 0) - 3) * 30;
                                            fechaFila = DateTime.Parse(valor);
                                            //DateTime f = DateTime.Parse(fechaFila.ToString(ConstantesBase.FormatoFecha));
                                            DateTime f = diacm.FechaDia.AddMinutes(iinicio);
                                            if (iinicio == 48 * 30)
                                                f = f.AddMinutes(-1);
                                            var segundos = Math.Abs((fechaFila - f).TotalSeconds);
                                            if (segundos > 5)
                                            {
                                                listaComment.Add("Celda: " + ws.Cells[i + (cabec ? rowsHead : 0), j + 1] + " Pestaña: " + (p == 1 ? "Cmg_Barra" : (p == 2 ? "Cmg_Ener" : "Cmg_Cong")) + " -> Fecha-hora errada , archivo: " + archivo);
                                                mostrarCarga = false;
                                                diacorrecto = false;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            listaComment.Add("Celda: " + ws.Cells[i + (cabec ? rowsHead : 0), j + 1] + " Pestaña: " + (p == 1 ? "Cmg_Barra" : (p == 2 ? "Cmg_Ener" : "Cmg_Cong")) + " -> Fecha-hora errada , archivo: " + archivo);
                                            mostrarCarga = false;
                                            diacorrecto = false;
                                        }

                                    }
                                    else
                                    {
                                        //- Debemos hacer esta modificación en esta linea

                                        if (ws.Cells[i + (cabec ? rowsHead : 0), j + 1].Value != null)
                                        {
                                            if (ws.Cells[i + (cabec ? rowsHead : 0), j + 1].Value.ToString() != "")
                                            {
                                                if (Base.Tools.Util.EsNumero(ws.Cells[i + (cabec ? rowsHead : 0), j + 1].Value.ToString()))
                                                {
                                                    var val_ = Convert.ToDecimal(ws.Cells[i + (cabec ? rowsHead : 0), j + 1].Value);
                                                    valor = val_.ToString();
                                                }
                                                else
                                                {
                                                    listaComment.Add("Celda: " + ws.Cells[i + (cabec ? rowsHead : 0), j + 1] + " Pestaña: " + (p == 1 ? "Cmg_Barra" : (p == 2 ? "Cmg_Ener" : "Cmg_Cong")) + " -> Contenido no numerico: \"" + ws.Cells[i + (cabec ? rowsHead : 0), j + 1].Value + "\" (" + archivo + "-" + diacm.FechaDia.ToString("YYYY-MM-dd") + ")");
                                                    mostrarCarga = false;
                                                    diacorrecto = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            valor = null;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                listaComment.Add("Celda: " + ws.Cells[i + (cabec ? rowsHead : 0), j + 1] + " Pestaña: " + (p == 1 ? "Cmg_Barra" : (p == 2 ? "Cmg_Ener" : "Cmg_Cong")) + " -> Celda vacia o sin datos (" + archivo + "-" + diacm.FechaDia.ToString("YYYY-MM-dd") + ")");
                                mostrarCarga = false;
                                diacorrecto = false;
                            }

                            if ((cc + i - 3) <= nFil + 1)
                            {
                                switch (p)
                                {
                                    case 1: diacm.Matriz1[cc + i - 3][j - 1] = valor; break;
                                    case 2: diacm.Matriz2[cc + i - 3][j - 1] = valor; break;
                                    case 3: diacm.Matriz3[cc + i - 3][j - 1] = valor; break;
                                }
                            }
                        }
                    }
                }

                cc += nFil + (cabec ? 0 : 1);
            }

            if (diacorrecto)
            {
                listaComment.Add("Archivo correcto: " + archivo + " ---- " + diacm.FechaDia.ToString("yyyy-MM-dd"));
            }

        }

        /// <summary>
        /// Carga un archivo Excel de costo marginal a lista de DTO SICostomarginalDTO
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="maxSicmcodi"></param>
        /// <returns></returns>
        private List<SiCostomarginalDTO> CargaDiaMatrizBarraACostoMarginalDTO(DiaValoresCostoMarginal registro, ref int maxSicmcodi)
        {
            List<SiCostomarginalDTO> lista = new List<SiCostomarginalDTO>();
            SiCostomarginalDTO Obj = null;
            int col = registro.IdCabecera.Count();
            for (int c = 1; c < col; c++)
            {
                for (var cc = 1; cc <= 48; cc++)
                {
                    if (registro.Matriz1[cc][c].Length != 0)
                    {
                        Obj = new SiCostomarginalDTO();
                        Obj.Cmgrcodi = maxSicmcodi++;
                        Obj.Barrcodi = registro.IdCabecera[c];
                        Obj.Cmgrcorrelativo = cc;
                        Obj.Cmgrfecha = DateTime.Parse(registro.Matriz1[cc][0]);
                        Obj.Cmgrtotal = Decimal.Parse(registro.Matriz1[cc][c]);
                        Obj.Cmgrenergia = Decimal.Parse(registro.Matriz2[cc][c]);
                        Obj.Cmgrcongestion = Decimal.Parse(registro.Matriz3[cc][c]);
                        Obj.Cmgrusucreacion = User.Identity.Name.ToString();
                        Obj.Cmgrfeccreacion = DateTime.Now;
                        Obj.Cmgrtcodi = 1;
                        lista.Add(Obj);
                    }
                }
            }
            return lista;
        }

        private List<SiCostomarginaltempDTO> CargaDiaMatrizBarraACostoMarginalTempDTO(DiaValoresCostoMarginal registro, int enviocodi)
        {
            List<SiCostomarginaltempDTO> lista = new List<SiCostomarginaltempDTO>();
            SiCostomarginaltempDTO Obj = null;
            DateTime fechahora;
            int col = registro.IdCabecera.Count();
            for (int c = 1; c < col; c++)
            {
                for (var cc = 1; cc <= 48; cc++)
                {
                    //- Debemos modificar esta linea
                    //if (registro.Matriz1[cc][c].Length != 0)
                    //{
                        fechahora = DateTime.Parse(registro.Matriz1[cc][0]);
                        fechahora = new DateTime(fechahora.Year, fechahora.Month, fechahora.Day);
                        Obj = new SiCostomarginaltempDTO();
                        Obj.Enviocodi = enviocodi;
                        Obj.Barrcodi = registro.IdCabecera[c];
                        Obj.Cmgtcorrelativo = cc;
                        Obj.Cmgtfecha = (cc == 48) ? fechahora.AddMinutes(cc * 30 - 1) : fechahora.AddMinutes(cc * 30);

                        //- Verificamos que los números no sean nulos

                        if (registro.Matriz1[cc][c] != null)
                            Obj.Cmgttotal = Decimal.Parse(registro.Matriz1[cc][c]);

                        if (registro.Matriz2[cc][c] != null)
                            Obj.Cmgtenergia = Decimal.Parse(registro.Matriz2[cc][c]);

                        if (registro.Matriz3[cc][c] != null)
                            Obj.Cmgtcongestion = Decimal.Parse(registro.Matriz3[cc][c]);

                        lista.Add(Obj);
                    //}
                }
            }
            return lista;
        }
        /// <summary>
        /// Carga varios archivos Excel de costo marginal a lista de DTO SICostomarginalDTO
        /// </summary>
        /// <param name="listaCM"></param>
        /// <returns></returns>
        private List<SiCostomarginaltempDTO> CargaMatrizBarraACostoMarginalTempDTO(List<DiaValoresCostoMarginal> listaCM, int enviocodi)
        {
            List<SiCostomarginaltempDTO> lista = new List<SiCostomarginaltempDTO>();
            List<SiCostomarginaltempDTO> listaDia;

            foreach (var reg in listaCM)
            {
                listaDia = CargaDiaMatrizBarraACostoMarginalTempDTO(reg, enviocodi);
                lista.AddRange(listaDia);
            }
            return lista;
        }

        /// <summary>
        /// Consulta si existe costo marginal del mes ingresado
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ConsultaCM(string mesAnio)
        {
            SioseinModel model = new SioseinModel();

            List<BarraDTO> cabecera = new List<BarraDTO>();

            DateTime f_ = DateTime.Parse(ConstantesSioSein.IniDiaFecha + mesAnio.Replace(" ", "/"));

            var Lista = servicio.GetByCriteriaSiCostomarginals(f_, f_.AddMonths(1).AddDays(-1));

            model.NRegistros = Lista.Count;
            model.FechaInicio = f_.ToString(ConstantesBase.FormatoFechaBase);
            model.FechaFin = f_.AddMonths(1).AddDays(-1).ToString(ConstantesBase.FormatoFechaBase);

            return Json(model);
        }

        /// <summary>
        /// Guardamos la carga en la nueva tabla de CM
        /// </summary>
        /// <returns></returns>
        private void SaveCM(string mesAnio, string nrodias)
        {
            int anho = int.Parse(mesAnio.Substring(3, 4));
            int mes = int.Parse(mesAnio.Substring(0, 2));
            DateTime f_ = new DateTime(anho, mes, 1);
            if (nrodias != null)
            {
                MeEnvioDTO envio = servicio.RegistroLogCargaDatosCMCP(User.Identity.Name.ToString(), f_, ConstantesSioSein.FdatcodiCMCPmen, nrodias);
                List<SiCostomarginaltempDTO> listaCM = CargaMatrizBarraACostoMarginalTempDTO(this.ListaCostoMarginal, envio.Enviocodi);
                if (listaCM.Count > 0)
                {
                    servicio.SaveSiCostomarginaltempMasivo(listaCM);
                    // Pasar datos de tabla temporal a si_costomarginal
                    servicio.ProcesarTempCostoMarginal(envio.Enviocodi, f_, f_.AddMonths(1).AddDays(-1), User.Identity.Name.ToString());
                    // Actualizar envio
                    servicio.UpdateMeEnvio(envio);
                    // borrar datos de tabla temporal
                    servicio.DeleteSiCostomarginaltemp(envio.Enviocodi);
                }
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CostoMarginalDiario()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.Fecha = DateTime.Now.ToString(ConstantesBase.FormatoFechaBase);
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ViewLogAuditoriaDia(string fecha)
        {
            SioseinModel model = new SioseinModel();

            DateTime f_ = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.CurrentCulture);
            var ListaMeenvio = servicio.GetListaMeEnvioByFdat(ConstantesSioSein.FdatcodiCMCPdia, f_);
            model.Resultado = servicio.ListaMeenvioHtml(ListaMeenvio, 0);

            return Json(model);
        }

        /// <summary>
        /// Consulta si existe costo marginal del mes por Dia
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PartialViewResult ConsultaDiaCM(string fecha)
        {
            SioseinModel model = new SioseinModel();
            string barrnomb = "-1";

            List<BarraDTO> cabecera = new List<BarraDTO>();

            DateTime f_ = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);

            var Lista = servicio.GetByCriteriaSiCostomarginals(f_, f_);
            var ListaBarrcodi = Lista.GroupBy(x => x.Barrcodi).Select(x => x.First().Barrcodi).Distinct().ToList();
            if (ListaBarrcodi.Count > 0)
            {
                cabecera = servicio.GetByCriteriaTrnBarras(barrnomb, string.Join(",", ListaBarrcodi));
            }

            model.CabeceraCm = cabecera;
            model.ListaCM = Lista;
            model.Fecha_ = f_;

            return PartialView(model);
        }

        /// <summary>
        /// LeerFileUpArchivo - Oliver
        /// Metodo de Lectura ZIP
        /// </summary>
        /// <param name="fecha">fecha de consulta</param>
        /// <returns>1,2,3,4,...</returns>
        public PartialViewResult LeerFileUpArchivoDiario(string fecha)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesSioSein.PathArchivoExcel;
            DateTime f_ = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.CurrentCulture);
            List<BarraDTO> barras;
            SioseinModel model = new SioseinModel();
            string file = path + NombreFile;
            string result1 = string.Empty, result2 = string.Empty, result3 = string.Empty, message_ = string.Empty;

            int nRegistros = 0;
            bool mostrarCarga = true;
            List<string> listaComment = new List<string>();
            model.TipoEmpresas = new List<string>();
            this.ListaCostoMarginal = new List<DiaValoresCostoMarginal>();
            DiaValoresCostoMarginal diacm = new DiaValoresCostoMarginal();
            diacm.FechaDia = f_;
            this.ListaCostoMarginal.Add(diacm);


            if (NombreFile.Contains(".xls"))
            {
                nRegistros = 1;
                int nFil = 48, nCol = 0, contarDias = 0;
                contarDias++;
                string path_ = path + NombreFile;// + existeMes.First().FullName;

                FileInfo fileInfo = new FileInfo(path_);
                barras = servicio.ListTrnBarras();
                ProcesarArchivoDiarioCM(barras, fileInfo, listaComment, diacm, NombreFile, ref mostrarCarga, ref nCol);

                if (mostrarCarga)
                {
                    result1 = this.servicio.CostoMarginalMensualHtml(diacm.Matriz1, nFil + 1, nCol, 1, 1, 1);
                    result2 = this.servicio.CostoMarginalMensualHtml(diacm.Matriz2, nFil + 1, nCol, 1, 2, 1);
                    result3 = this.servicio.CostoMarginalMensualHtml(diacm.Matriz3, nFil + 1, nCol, 1, 3, 1);
                }

                //Eliminamos archivo excel
                System.IO.File.Delete(path_);
            }
            else { message_ = "El archivo a cargar no es un archivo Excel"; }
            if (mostrarCarga)
            {
                SaveCMdia(fecha);
            }
            model.Resultado = result1;
            model.Resultado2 = result2;
            model.Resultado3 = result3;
            model.TipoEmpresas = listaComment;
            model.MostrarCarga = mostrarCarga;
            model.Mensaje = message_;
            model.NRegistros = nRegistros;

            return PartialView(model);
        }

        /// <summary>
        /// Guardamos la carga en la nueva tabla de CM
        /// </summary>
        /// <returns></returns>
        private void SaveCMdia(string fecha)
        {
            SioseinModel model = new SioseinModel();
            DateTime f_ = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
            MeEnvioDTO envio = servicio.RegistroLogCargaDatosCMCP(User.Identity.Name.ToString(), f_, ConstantesSioSein.FdatcodiCMCPdia, "-1");
            List<SiCostomarginaltempDTO> listaCM = CargaMatrizBarraACostoMarginalTempDTO(this.ListaCostoMarginal, envio.Enviocodi);
            if (listaCM.Count > 0)
            {
                servicio.SaveSiCostomarginaltempMasivo(listaCM);
                // Pasar datos de tabla temporal a si_costomarginal
                servicio.ProcesarTempCostoMarginal(envio.Enviocodi, f_, f_, User.Identity.Name.ToString());
                // Actualizar envio
                servicio.UpdateMeEnvio(envio);
                // borrar datos de tabla temporal
                servicio.DeleteSiCostomarginaltemp(envio.Enviocodi);
            }

        }

        /// <summary>
        /// Exportacion excel Costos marginales
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ExportarExcelCM(string fecha)
        {
            SioseinModel model = new SioseinModel();
            DateTime dtfecha = DateTime.MinValue;
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesSioSein.PathArchivoExcel;
                dtfecha = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
                string nameFile = ConstantesSioSein.RptExcel + "_" + dtfecha.ToString("yyyyMMdd") + ConstantesSioSein.ExtensionExcel;
                this.servicio.GenerarArchivoExcelCM(dtfecha, dtfecha, ruta + nameFile);

                model.Resultado = nameFile;
                model.IdEnvio = 1;
            }
            catch (Exception ex)
            {
                model.IdEnvio = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewCostoMarginalCP()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            SioseinModel model = new SioseinModel();
            model.Fecha = DateTime.Now.ToString(ConstantesBase.FormatoFechaBase);
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public JsonResult ConsultaCMCP(string fecha, string url)
        {
            SioseinModel model = new SioseinModel();

            DateTime f_ = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.CurrentCulture);

            var lista = servicio.GetByCriteriaSiCostomarginals(f_, f_);
            lista = lista.GroupBy(x => x.Cmgrfecha).Select(x => new SiCostomarginalDTO() { Cmgrfecha = x.Key }).ToList();
            model.Resultado = servicio.ConsultaCMCPhtml(lista, url);

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <returns></returns>
        public JsonResult ConsultaCMCPdet(string fecha, string hora)
        {
            SioseinModel model = new SioseinModel();
            var arrF = fecha.Split('/');
            var arrH = hora.Split(':');
            DateTime f_ = new DateTime(int.Parse(arrF[2]), int.Parse(arrF[1]), int.Parse(arrF[0]), int.Parse(arrH[0]), int.Parse(arrH[1]), 0);

            var lista = servicio.GetByCriteriaSiCostomarginalDet(f_);
            var cabecera = servicio.GetListaTrnBarras(string.Join(",", lista.Select(x => x.Barrcodi).ToList()));

            model.Resultado = servicio.ConsultaCMCPdetHtml(lista, cabecera, f_);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Exportacion excel Costos marginales
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ExportarExcelCMmasivo(string fechaini, string fechafin)
        {
            SioseinModel model = new SioseinModel();
            DateTime fec1 = DateTime.MinValue, fec2 = DateTime.MinValue;
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesSioSein.PathArchivoExcel;
                fec1 = DateTime.ParseExact(fechaini, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
                fec2 = DateTime.ParseExact(fechafin, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
                string nameFile = ConstantesSioSein.RptExcel + "_" + fec1.ToString("yyyyMMdd") + "_" + fec2.ToString("yyyyMMdd") + ConstantesSioSein.ExtensionExcel;
                int rr = this.servicio.GenerarArchivoExcelCM(fec1, fec2, ruta + nameFile);

                if (rr > 0)
                {
                    model.Resultado = nameFile;
                    model.IdEnvio = 1;
                }
                else { model.IdEnvio = 0; }
            }
            catch (Exception ex)
            {
                model.IdEnvio = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }


        /// <summary>
        /// Exportacion excel Costos marginales
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ExportarValoresNulos(string fechaini, string fechafin)
        {
            SioseinModel model = new SioseinModel();
            DateTime fec1 = DateTime.MinValue, fec2 = DateTime.MinValue;
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesSioSein.PathArchivoExcel;
                fec1 = DateTime.ParseExact(fechaini, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
                fec2 = DateTime.ParseExact(fechafin, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
                string nameFile = ConstantesSioSein.RptExcelNulos + "_" + fec1.ToString("yyyyMMdd") + "_" + fec2.ToString("yyyyMMdd") + ConstantesSioSein.ExtensionExcel;

                int rr = this.servicio.GenerarArchivoValoresNulos(fec1, fec2, ruta + nameFile);

                if (rr > 0)
                {
                    model.Resultado = nameFile;
                    model.IdEnvio = 1;
                }
                else { model.IdEnvio = 0; }
            }
            catch (Exception ex)
            {
                model.IdEnvio = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Descargar archivo excel
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarValoresNulos(string nameFile)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesSioSein.PathArchivoExcel;
            string fullPath = ruta + nameFile;
            return File(fullPath, ConstantesSioSein.AppExcel, nameFile);
        }


        #endregion
    }
}
