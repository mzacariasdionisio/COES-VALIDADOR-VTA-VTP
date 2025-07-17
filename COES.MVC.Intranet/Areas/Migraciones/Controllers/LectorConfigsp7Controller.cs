using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Migraciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Eventos;

//using COES.MVC.Intranet.ServicioScadasp7;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Scada;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
using DevExpress.Office.Utils;
using log4net;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using static COES.MVC.Intranet.Areas.Siosein.Helper.ConstantesSiosein;

namespace COES.MVC.Intranet.Areas.Migraciones.Controllers
{
    public class LectorConfigsp7Controller : BaseController
    {
        //
        // GET: /Migraciones/LectorConfigsp7/
        MigracionesAppServicio servicio = new MigracionesAppServicio();

        List<string> correosScada = ConfigurationManager.AppSettings["CorreosSCADA"].Split(';').ToList();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(LectorConfigsp7Controller));
        private static string NameController = "LectorConfigsp7Controller";
        private static List<EstadoModel> ListaEstadoSistemaA = new List<EstadoModel>();
        private ScadaServicio scadaServicio = new ScadaServicio();

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
        public LectorConfigsp7Controller()
        {
            ListaEstadoSistemaA = new List<EstadoModel>();
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "0", EstadoDescripcion = "NO" });
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "SÍ" });
        }

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[DatosSesionMigraciones.SesionNombreArchivo] != null) ?
                    Session[DatosSesionMigraciones.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionMigraciones.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Sesion fecha
        /// </summary>
        public String Sfecha
        {
            get
            {
                return (Session[DatosSesionMigraciones.SesionSfecha] != null) ?
                    Session[DatosSesionMigraciones.SesionSfecha].ToString() : null;
            }
            set { Session[DatosSesionMigraciones.SesionSfecha] = value; }
        }

        /// <summary>
        /// Sesion fecha
        /// </summary>
        public String Scanalcodi
        {
            get
            {
                return (Session[DatosSesionMigraciones.SesionScanalcodi] != null) ?
                    Session[DatosSesionMigraciones.SesionScanalcodi].ToString() : null;
            }
            set { Session[DatosSesionMigraciones.SesionScanalcodi] = value; }
        }

        #region "Lector configuracion sp7"

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            string sNombreArchivo = "";

            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;

                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                    this.NombreFile = sNombreArchivo;
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Configuracionsp7()
        {
            MigracionesModel model = new MigracionesModel();

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult SincronizarSp7Coes()
        {
            HttpResponseModel model = new HttpResponseModel();
            ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
            DateTime fechaInicio = DateTime.Now;
            try
            {
                List<CPointsDTO> canalesFromScada = scadaServicio.ObtenerCanalesScada();
                List<TrCanalSp7DTO> canalesFromTrcoes = scadaServicio.ObtenerCanalesTrcoes();
                canalesFromScada = canalesFromScada.Where(x => x.PointType == "A" || x.PointType == "D").ToList();
                List<CPointsDTO> canalesFromScadaActivos = canalesFromScada.Where(x => x.Active == "T").ToList();
                List<CPointsDTO> canalesFromScadaInactivos = canalesFromScada.Where(x => x.Active == "F").ToList();
                List<TrCanalSp7DTO> canalesFromTrcoesQueNoExistenEnScadaComoActivos = canalesFromTrcoes.Where(x => canalesFromScadaActivos.All(a => a.PointNumber != x.Canalcodi)).ToList();
                List<CPointsDTO> canalesFromScadaExistentesEnTrcoes = canalesFromScadaActivos.Where(x => canalesFromTrcoes.Any(a => a.Canalcodi == x.PointNumber)).ToList();
                List<CPointsDTO> canalesFromScadaNoExistentesEnTrcoes = canalesFromScadaActivos.Where(x => canalesFromTrcoes.All(a => a.Canalcodi != x.PointNumber)).ToList();

                List<string> canalesRepetidos = new List<string>();
                foreach (var canalAgrupadoPorPointName in canalesFromScadaActivos.GroupBy(x => x.PointName))
                {
                    if (canalAgrupadoPorPointName.Count() > 1)
                    {
                        canalesRepetidos.Add(canalAgrupadoPorPointName.Key + " (" + string.Join(",", canalAgrupadoPorPointName.ToList().Select(x => x.PointNumber).ToList().ToArray()) + ")");
                    }
                }

                if (canalesRepetidos.Count > 0)
                {
                    model.Mensaje = "Canales se repiten.";
                    model.Detalle = string.Join("<br>", canalesRepetidos);
                    model.NroRegistros = -1;
                    model.Resultado = "";
                    string mensaje = canalesRepetidos.Count > 1 ? string.Format(@"Estimados señores, <br /><br />Se ha identificado {0} señales duplicadas: <br />", canalesRepetidos.Count.ToString()) : @"Estimados señores, <br /><br />Se ha identificado una señal duplicada: <br />";
                    mensaje = mensaje + model.Detalle + "<br /><br /> Favor de realizar la verificación y/o corrección en el Scada Siemens.";
                    COES.Base.Tools.Util.SendEmail(correosScada, null, "Sincronización SGOCOES de señales scada SP7 - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - Señales duplicadas", mensaje);

                    mensaje += string.Format(@"<br /><br /> Para ver el detalle de las señales actualizadas consultar en el siguiente enlace: {0}", "https://www.coes.org.pe/AppIntranet/TiempoReal/ActualizacionSp7/Index");
                    return Json(model);
                }

                List<string> canalesREliminadosLogicamente = new List<string>();
                foreach (CPointsDTO canalInactivo in canalesFromScadaInactivos)
                {
                    if(canalesFromTrcoes.Find(x => x.Canalcodi == canalInactivo.PointNumber) != null)
                    {
                        scadaServicio.EliminadoLogicoDeCanales(User.Identity.Name, (int)canalInactivo.PointNumber);
                        canalesREliminadosLogicamente.Add(canalInactivo.PointName + " (" + canalInactivo.PointNumber + ")");
                    }
                }

                if (canalesREliminadosLogicamente.Count > 0)
                {
                    string detalle = string.Join("<br>", canalesREliminadosLogicamente);
                    string mensaje = canalesREliminadosLogicamente.Count > 1 ? string.Format(@"Estimados señores, <br /><br />Se ha identificado {0} señales eliminadas en SCADA (IMM) pero que existen en el TRCOES: <br /><br />", canalesREliminadosLogicamente.Count.ToString())
                        : @"Estimados señores, <br /><br />Se ha identificado una señal eliminada en SCADA (IMM) pero que existe en el TRCOES: <br />";
                    mensaje = mensaje + detalle + "<br /><br /> Favor de realizar la verificación y/o corrección en el Scada Siemens y TRCOES.";

                    mensaje += string.Format(@"<br /><br /> Para ver el detalle de las señales actualizadas consultar en el siguiente enlace: {0}", "https://www.coes.org.pe/AppIntranet/TiempoReal/ActualizacionSp7/Index");
                    COES.Base.Tools.Util.SendEmail(correosScada, null, "Sincronización SGOCOES de señales scada SP7 - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - Señales eliminadas", mensaje);
                }


                foreach (TrCanalSp7DTO canalNoExistenEnScada in canalesFromTrcoesQueNoExistenEnScadaComoActivos)
                {
                    if (canalNoExistenEnScada.Canaliccp != null && canalNoExistenEnScada.Canaliccp != string.Empty)
                    {
                        scadaServicio.EliminadoLogicoDeCanales(User.Identity.Name, canalNoExistenEnScada.Canalcodi);
                    }
                }

                int totalActualizaciones = 0;
                Parallel.ForEach(canalesFromScadaExistentesEnTrcoes, new ParallelOptions { MaxDegreeOfParallelism = 50 }, (cpoint, state) =>
                {
                    try
                    {
                        TrCanalSp7DTO c = canalesFromTrcoes.Find(x => x.Canalcodi == cpoint.PointNumber);
                        if (c != null && (cpoint.PointName != c.Canalnomb || cpoint.PointType != c.PointType))
                        {
                            Interlocked.Increment(ref totalActualizaciones);
                            scadaServicio.ActualizarCanalConDataDeScada(User.Identity.Name, cpoint);
                        }
                    }
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }
                });

                Parallel.ForEach(canalesFromScadaNoExistentesEnTrcoes, new ParallelOptions { MaxDegreeOfParallelism = 50 }, (cpoint, state) =>
                {
                    try
                    {
                        scadaServicio.CrearCanalConDataDeScada(User.Identity.Name, cpoint);
                    }
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }
                });

                if (!exceptions.IsEmpty)
                {
                    model.Mensaje = "Problemas con la sincronización";
                    model.Detalle = string.Join("<br>", exceptions); ;
                    model.NroRegistros = -1;
                }

                if(totalActualizaciones > 0)
                {
                    int tipo = 1;//SINCRONIZACIÓN CON SIEMENS
                    int idmax = scadaServicio.GetMaxCodigoTrCargaArchXmlSp7();
                    scadaServicio.GenerarRegistroTrCargaArchXmlSp7SiHayActualizacionDeCanales(idmax, fechaInicio, totalActualizaciones, User.Identity.Name, null, tipo);
                    scadaServicio.UpdateCanalCambioSiHayActualizacionDeCanales(idmax, fechaInicio);
                }

                /*
                foreach (CPointsDTO cpoint in canalesFromScadaExistentesEnTrcoes)
                {
                    scadaServicio.ActualizarCanalConDataDeScada(User.Identity.Name, cpoint);
                }

                foreach (CPointsDTO cpoint in canalesFromScadaNoExistentesEnTrcoes)
                {
                    scadaServicio.CrearCanalConDataDeScada(User.Identity.Name, cpoint);
                }*/

                DateTime fechaFinal = DateTime.Now;
                string mensajeFinal = @"Estimados señores, <br /><br />se realizó la sicronizacion SGOCOES de señales con los siguientes detalles:<br />";
                mensajeFinal += string.Format(@"<strong>Fuente</strong>: {0} <br />
                                      <strong>Fecha y hora</strong>: {1} <br />
                                           <strong>Usuario</strong>: {2} <br />", "Sincronizacion directa con Siemens", fechaFinal.ToString("dd/MM/yyyy HH:mm"), User.Identity.Name);

                mensajeFinal += string.Format(@"<br /><br /> Para ver el detalle de las señales actualizadas consultar en el siguiente enlace: {0}", "https://www.coes.org.pe/AppIntranet/TiempoReal/ActualizacionSp7/Index");
                COES.Base.Tools.Util.SendEmail(correosScada, null, "Sincronización SGOCOES de señales scada SP7 - " + fechaFinal.ToString("dd/MM/yyyy HH:mm"), mensajeFinal);


                int total = canalesFromScadaNoExistentesEnTrcoes.Count + totalActualizaciones;
                if(total > 0)
                {
                    model.Mensaje = "Se ha sincronizado los registros con SCADA";
                    model.Detalle = "";
                    model.NroRegistros = total;
                    model.Resultado = "BD SP7: " + model.NroRegistros + " actualizaciones";
                }
                else
                {
                    model.Mensaje = "No hay registros ha sincronizar";
                    model.Detalle = "";
                    model.NroRegistros = total;
                    model.Resultado = "BD SP7: " + model.NroRegistros + " actualizaciones";
                }
            }
            catch (Exception ex)
            {
                model.Mensaje = "Problemas con la sincronización";
                model.Detalle = ex.Message;
                model.NroRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public JsonResult LeerFileUpArchivo(int btn)
        {
            DateTime fechaInicio = DateTime.Now;
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            MigracionesModel model = new MigracionesModel();
            string file = path + this.NombreFile;
            int result = 0;

            string msj = string.Empty;

            XmlDocument xmldoc = new XmlDocument();
            XmlNodeList xmlnode = null;

            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);

            string aor = string.Empty, name = string.Empty, siid = string.Empty, gisid = string.Empty, pathb = string.Empty, zona = string.Empty, descr = string.Empty, rdfid = string.Empty;
            model.ListaString = new List<string>();
            switch (btn)
            {
                case 1:
                    List<TrEmpresaSp7DTO> ListaTrEmpresasp7 = scadaServicio.ObtenerEmpresasDesdeTrcoes();
                    xmlnode = xmldoc.DocumentElement.SelectNodes("/XDF/Instances/Parent/sysCoAreaCategory/sysCoAreaOfResponsibilities/AreaOfResponsibilityGroup/AreaOfResponsibilityGroup/AreaOfResponsibility");

                    foreach (XmlNode d in xmlnode)
                    {
                        aor = d.Attributes["AreaOfResponsibilityId"].Value;
                        name = d.Attributes["Name"].Value;
                        siid = d.Attributes["Siid"].Value;

                        var existe_ = ListaTrEmpresasp7.Find(x => x.Emprcodi == int.Parse(aor));
                        if (existe_ == null) { 
                            scadaServicio.GenerarEmpresasEnTrcoes(Convert.ToInt32(aor), name, siid, User.Identity.Name); result++; 
                        }
                    }

                    break;
                case 2:
                    ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
                    string ls_csv = "";
                    List<TrCanalSp7DTO> canalesFromTrcoes = scadaServicio.ObtenerCanalesTrcoes();
                    xmlnode = xmldoc.DocumentElement.SelectNodes("/XDF/Instances/Parent/sysIccpCategory/IcRemotes/IcRemoteCenter");
                    Parallel.ForEach(xmlnode.Cast<XmlNode>(), new ParallelOptions { MaxDegreeOfParallelism = 150 }, (xm, state) =>
                    {
                        try
                        {
                            string local = xm.LocalName;
                            string nombre = xm.Name;
                            string nodo = xm.NodeType.ToString();

                            string enlace = "";
                            string remota = xm.Attributes["Name"].Value.ToString().Trim();

                            foreach (XmlNode xn2 in xm.ChildNodes)
                            {
                                string local0 = xn2.LocalName;

                                if (local0 == "IcLink")
                                {
                                    enlace = xn2.Attributes["Name"].Value.ToString().Trim();
                                    break;
                                }
                            }

                            foreach (XmlNode xn2 in xm.ChildNodes)
                            {
                                string contenedor = "";
                                string local2 = xn2.LocalName;

                                if (local2 == "IcClientBlock1And2Container")
                                {
                                    contenedor = xn2.Attributes["Name"].Value.ToString();
                                    foreach (XmlNode xn3 in xn2.ChildNodes)
                                    {
                                        string local3 = xn3.LocalName;

                                        if (local3 == "IcClientBlock1And2Object")
                                        {
                                            string ls_siid = xn3.Attributes["Siid"].Value.ToString();
                                            string ls_name = xn3.Attributes["Name"].Value.ToString();

                                            foreach (XmlNode xn4 in xn3.ChildNodes)
                                            {
                                                string local4 = xn4.LocalName;

                                                if (local4 == "Link_IcClientBlock1And2ObjectReceivesFromRemoteInfo")
                                                {
                                                    string ls_child_siid = xn4.Attributes["Siid"].Value.ToString();
                                                    string ls_child_pathb = xn4.Attributes["PathB"].Value.ToString();

                                                    ls_csv += ls_child_siid + "," + ls_name + "," + ls_child_pathb + "\r\n";

                                                    var canalcodi = Int32.Parse(ls_child_siid);
                                                    var canalCoes = canalesFromTrcoes.Find(x => x.Canalcodi == canalcodi);

                                                    if (canalCoes != null)
                                                    {
                                                        bool tieneCambios = false;
                                                        if (canalCoes.Canalestado != ls_name.Trim() || canalCoes.Pathb != ls_child_pathb ||
                                                            canalCoes.CanalRemota != remota || canalCoes.CanalContenedor != contenedor || canalCoes.CanalEnlace != enlace)
                                                        {
                                                            tieneCambios = true;
                                                        }

                                                        if (tieneCambios)
                                                        {
                                                            scadaServicio.ActualizarCanalesIccpXml(ls_name.Trim(), ls_child_pathb, User.Identity.Name, remota, contenedor, enlace, canalcodi);
                                                            result++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        throw new InvalidOperationException(canalcodi.ToString());
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            exceptions.Enqueue(e);
                        }
                    }); 

                    if (result > 0)
                    {
                        int tipo = 2;//SINCRONIZACIÓN CON SIEMENS
                        int idmax = scadaServicio.GetMaxCodigoTrCargaArchXmlSp7();
                        scadaServicio.GenerarRegistroTrCargaArchXmlSp7SiHayActualizacionDeCanales(idmax, fechaInicio, result, User.Identity.Name, this.NombreFile, tipo);
                        scadaServicio.UpdateCanalCambioSiHayActualizacionDeCanales(idmax, fechaInicio);
                    }

                    string mensajeFinal = @"Estimados señores, <br /><br />Se realizó la sicronizacion SGOCOES de señales con los siguientes detalles:<br /><br />";
                    mensajeFinal += string.Format(@"<strong>Fuente</strong>: {0} <br />
                                      <strong>XML</strong>: {1} <br />
                                      <strong>Fecha y hora</strong>: {2} <br />
                                           <strong>Usuario</strong>: {3} <br />", "Sincronizacion a traves de carga de archivos xml", this.NombreFile, fechaInicio.ToString("dd/MM/yyyy HH:mm"), User.Identity.Name);


                    if (!exceptions.IsEmpty)
                    {
                        Log.Error(exceptions);
                        mensajeFinal += "<br /><b>Se presentaron algunas observaciones (revisar ICCP de código Siemens):</b><br />";
                        foreach (Exception e in exceptions)
                        {
                            mensajeFinal += e.Message + "<br />";
                        }
                        mensajeFinal += "<br />";
                        //throw new Exception("Error al intentar sincronizar ICCP - XML");
                    }

                    mensajeFinal += string.Format(@"<br /> Para ver el detalle de las señales actualizadas consultar en el siguiente enlace: {0}", "https://www.coes.org.pe/AppIntranet/TiempoReal/ActualizacionSp7/Index");
                    COES.Base.Tools.Util.SendEmail(correosScada, null, "Sincronización SGOCOES de señales scada SP7 - " + fechaInicio.ToString("dd/MM/yyyy HH:mm"), mensajeFinal);

                    break;
                case 3:
                    int registrosActualizados = 0;
                    string mensaje = "";
                    List<TrCanalSp7DTO> canalesFromTrcoes_3 = scadaServicio.ObtenerCanalesTrcoes();
                    List<TrEmpresaSp7DTO> ListaTrEmpresasp7_3 = scadaServicio.ObtenerEmpresasDesdeTrcoes();
                    //xmlnode = xmldoc.DocumentElement.SelectNodes("/XDF/Instances/Parent/GeographicalRegion/SubGeographicalRegion/Substation");

                    //SUBESTACION                           1
                    mensaje = CIMObtenerZona(file);
                    EscribirTextoLog("CIM (1/7): " + mensaje, this.NombreFile);

                    //CANAL
                    //ANALOGICO (a nivel de subestación)    2
                    mensaje = CIMObtenerSiidAnalogico(file, "XDF/Instances/Parent/GeographicalRegion/SubGeographicalRegion/Substation", canalesFromTrcoes_3, ListaTrEmpresasp7_3, ref registrosActualizados);
                    EscribirTextoLog("CIM (2/7): " + mensaje, this.NombreFile);

                    //ANALOGIGO (a nivel de bahía)          3
                    mensaje = CIMObtenerSiidAnalogicoBay(file, "XDF/Instances/Parent/GeographicalRegion/SubGeographicalRegion/Substation", canalesFromTrcoes_3, ListaTrEmpresasp7_3, ref registrosActualizados);
                    EscribirTextoLog("CIM (3/7): " + mensaje, this.NombreFile);

                    //ANALOGICO (a 1 nivel de subestación)  4
                    mensaje = CIMObtenerSiidAnalogico1nivel(file, "XDF/Instances/Parent/GeographicalRegion/SubGeographicalRegion/Substation", canalesFromTrcoes_3, ListaTrEmpresasp7_3, ref registrosActualizados);
                    EscribirTextoLog("CIM (4/7): " + mensaje, this.NombreFile);


                    //DIGITAL                               5
                    mensaje = CIMObtenerSiidDigital(file, canalesFromTrcoes_3, ListaTrEmpresasp7_3, ref registrosActualizados);
                    EscribirTextoLog("CIM (5/7): " + mensaje, this.NombreFile);

                    //digital a nivel de bahía              6
                    mensaje = CIMObtenerSiidDigitalBay(file, canalesFromTrcoes_3, ListaTrEmpresasp7_3, ref registrosActualizados);
                    EscribirTextoLog("CIM (6/7): " + mensaje, this.NombreFile);

                    //digital power transformer             7
                    mensaje = CIMObtenerSiidDigitalPTransf(file, canalesFromTrcoes_3, ListaTrEmpresasp7_3, ref registrosActualizados);
                    EscribirTextoLog("CIM (7/7): " + mensaje, this.NombreFile);

                    result = registrosActualizados;

                    if (result > 0)
                    {
                        int tipo = 3;//SINCRONIZACIÓN CON SIEMENS
                        int idmax = scadaServicio.GetMaxCodigoTrCargaArchXmlSp7();
                        scadaServicio.GenerarRegistroTrCargaArchXmlSp7SiHayActualizacionDeCanales(idmax, fechaInicio, result, User.Identity.Name, this.NombreFile, tipo);
                        scadaServicio.UpdateCanalCambioSiHayActualizacionDeCanales(idmax, fechaInicio);
                    }

                    string mensajeEtapa3 = @"Estimados señores, <br /><br />Se realizó la sicronizacion SGOCOES de señales con los siguientes detalles:<br /><br />";
                    mensajeEtapa3 += string.Format(@"<strong>Fuente</strong>: {0} <br />
                                      <strong>XML</strong>: {1} <br />
                                      <strong>Fecha y hora</strong>: {2} <br />
                                           <strong>Usuario</strong>: {3} <br />", "Sincronizacion a traves de carga de archivos xml", this.NombreFile, fechaInicio.ToString("dd/MM/yyyy HH:mm"), User.Identity.Name);

                    mensajeEtapa3 += string.Format(@"<br /> Para ver el detalle de las señales actualizadas consultar en el siguiente enlace: {0}", "https://www.coes.org.pe/AppIntranet/TiempoReal/ActualizacionSp7/Index");
                    COES.Base.Tools.Util.SendEmail(correosScada, null, "Sincronización SGOCOES de señales scada SP7 - " + fechaInicio.ToString("dd/MM/yyyy HH:mm"), mensajeEtapa3);


                    break;
            }

            if (btn == 3)
            {
                sendReporteCambios(fechaInicio);
                decimal cc = 0;
                foreach (var d in model.ListaString)
                {
                    cc += decimal.Parse(d);
                }
                model.Resultado = cc.ToString() + "," + DateTime.Now;
            }
            else { model.Resultado = result.ToString() + "," + DateTime.Now; }

            //model.ListaString.Add();
            return Json(model);
        }

        private void sendReporteCambios(DateTime fecha)
        {
            TiempoRealAppServicio appTiempoReal = new TiempoRealAppServicio();
            string filename = $"DATA_{fecha.ToString("ddMMyyyy")}.xlsx";
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathScadaArchivoExcel;
            string absolutePath = path + filename;
            appTiempoReal.ExportarCambiosAlSincronizarCanales(fecha, path, filename, absolutePath);
            FileInfo fileExcel = new FileInfo(absolutePath);

            if (fileExcel.Exists)
            {
                Log.Error("Correo no enviado. Archivo no se ha generado. ");
            }

            COES.Base.Tools.Util.SendEmailSincronizacionXmls(correosScada, string.Empty, "Sincronización SGOCOES de señales scada SP7 - " + fecha.ToString("dd/MM/yyyy HH:mm") + " - Reporte de cambios", string.Empty, absolutePath);
            //return string.Empty;
        }

        /// <summary>
        /// Permite registrar zonas (subestaciones) 
        /// </summary>
        /// <param name="xML">archivo XML</param>
        /// <returns></returns>
        private string CIMObtenerZona(string xML)
        {
            DateTime fechaIni = DateTime.Now;
            int registros = 0;

            ArrayList arrSseeAtributo = new ArrayList();
            arrSseeAtributo.Add("B1Number");
            arrSseeAtributo.Add("Name");
            arrSseeAtributo.Add("Description");
            arrSseeAtributo.Add("AreaOfResponsibilityId");
            arrSseeAtributo.Add("Siid");

            string[,] arrValores = CIMObtenerSiidNodoAtributos(xML, "XDF/Instances/Parent/GeographicalRegion/SubGeographicalRegion/Substation", arrSseeAtributo);

            //INGRESO DE DATOS. 
            for (int li_fila = 0; li_fila < arrValores.GetLength(0); li_fila++)
            {
                string cuenta = scadaServicio.ObtenerTotalZonasPorZonaId(Int32.Parse(arrValores[li_fila, 0]));

                if (cuenta != "1")
                {
                    cuenta = scadaServicio.ObtenerTotalEmpresaPorEmprcodi(Int32.Parse(arrValores[li_fila, 3]));

                    if (cuenta != "0")
                    {
                        scadaServicio.GenerarRegistroZona(Int32.Parse(arrValores[li_fila, 0]), arrValores[li_fila, 1], arrValores[li_fila, 2], Int32.Parse(arrValores[li_fila, 3]), Int32.Parse(arrValores[li_fila, 4]), User.Identity.Name);
                        registros++;
                    }
                    else
                    {
                        scadaServicio.ActualizarRegistroZona(arrValores[li_fila, 1], arrValores[li_fila, 2], Int32.Parse(arrValores[li_fila, 3]), Int32.Parse(arrValores[li_fila, 4]), User.Identity.Name, Int32.Parse(arrValores[li_fila, 0]));
                        registros++;
                    }
                }
                else
                {
                    scadaServicio.ActualizarRegistroZona(arrValores[li_fila, 1], arrValores[li_fila, 2], Int32.Parse(arrValores[li_fila, 3]), Int32.Parse(arrValores[li_fila, 4]), User.Identity.Name, Int32.Parse(arrValores[li_fila, 0]));
                    registros++;
                }
            }

            DateTime fechaFin = DateTime.Now;
            return registros + " registros (Tiempo transcurrido: " + ObtenerDiferenciaTiempo(fechaIni, fechaFin) + " s.)";
        }

        /// <summary>
        /// Permite actualizar señales analógicas con su empresa, area, rdfid, unidad, gisid a nivel de subestación
        /// </summary>
        /// <param name="XML">modelo CIM</param>
        /// <param name="Nodo">nodo </param>
        /// <returns></returns>
        public string CIMObtenerSiidAnalogico(string XML, string Nodo, List<TrCanalSp7DTO> listaCanalesTRCOES, List<TrEmpresaSp7DTO> listaEmpresasTRCOES, ref int registrosActualizados)
        {
            int registros = 0;
            DateTime fechaIni = DateTime.Now;
            XmlDocument xml = new XmlDocument();
            xml.Load(XML);
            XmlNodeList xnList = xml.SelectNodes(Nodo);
            string zonacodi = "";
            string siid = "";
            string unidad = "";
            string rdfId = "";
            string areaOfResponsibilityId = "";
            string gISId = "";

            //Substation
            foreach (XmlNode xm in xnList)
            {
                //B1Number(zonacodi)
                zonacodi = xm.Attributes["B1Number"].Value.ToString();

                siid = "";
                unidad = "";
                rdfId = "";
                areaOfResponsibilityId = "";

                //VoltageLevel
                foreach (XmlNode xm2 in xm.ChildNodes)
                {
                    //SynchronousMachine
                    foreach (XmlNode xm3 in xm2.ChildNodes)
                    {
                        //Analog
                        foreach (XmlNode xm4 in xm3.ChildNodes)
                        {
                            try
                            {
                                if (xm4.Attributes["AreaOfResponsibilityId"] == null)
                                {
                                    continue;
                                }

                                unidad = xm4.Attributes["Name"].Value.ToString();
                                rdfId = xm4.Attributes["RdfId"].Value.ToString();
                                areaOfResponsibilityId = xm4.Attributes["AreaOfResponsibilityId"].Value.ToString();

                                gISId = xm4.Attributes["GISId"].Value.ToString();

                                if (gISId == "")
                                    gISId = "-1";

                                //AnalogValue
                                foreach (XmlNode xm5 in xm4.ChildNodes)
                                {

                                    if (xm5.LocalName == "AnalogValue")
                                    {
                                        siid = xm5.Attributes["Siid"].Value.ToString();
                                        var canalcodi = Int32.Parse(siid); // convertir valor entero
                                        var canalCoes = listaCanalesTRCOES.Find(x => x.Canalcodi == canalcodi);
                                        if (canalCoes != null)
                                        {
                                            var emprcodi = Int32.Parse(areaOfResponsibilityId); // convertir valor entero
                                            var empresaCoes = listaEmpresasTRCOES.Find(x => x.Emprcodi == emprcodi);
                                            if (empresaCoes != null)
                                            {
                                                bool tieneCambios = false;
                                                var codigoZona = Int32.Parse(zonacodi); //convertir valor entero
                                                var codigoGisid = Int32.Parse(gISId); //convertir valor entero
                                                //Validar si existen cambios
                                                if (canalCoes.Emprcodi != emprcodi || canalCoes.Zonacodi != codigoZona ||
                                                    (canalCoes.Canalunidad ?? "").Trim() != (unidad ?? "").Trim())
                                                {
                                                    tieneCambios = true;
                                                }
                                                if (tieneCambios)
                                                {
                                                    scadaServicio.ActualizarCanalSiid(Int32.Parse(areaOfResponsibilityId), Int32.Parse(zonacodi), unidad, User.Identity.Name, Int32.Parse(siid));
                                                    registros++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }

                        }
                    }
                }
            }

            registrosActualizados += registros;
            DateTime fechaFin = DateTime.Now;
            double tiempo = ObtenerDiferenciaTiempo(fechaIni, fechaFin);
            return registros + " registros (Tiempo transcurrido: " + tiempo + " s.)";
        }

        /// <summary>
        /// Permite actualizar señales analógicas con su empresa, area, rdfid, unidad, gisid a nivel de subestación
        /// </summary>
        /// <param name="xML">Modelo XML</param>
        /// <param name="nodo">Nodo de referencia</param>
        /// <returns></returns>
        public string CIMObtenerSiidAnalogicoBay(string xML, string nodo, List<TrCanalSp7DTO> listaCanalesTRCOES, List<TrEmpresaSp7DTO> listaEmpresasTRCOES, ref int registrosActualizados)
        {
            int registros = 0;
            DateTime fechaIni = DateTime.Now;
            XmlDocument xml = new XmlDocument();
            xml.Load(xML);
            XmlNodeList xnList = xml.SelectNodes(nodo);
            //string ls_csv = "";
            string zonacodi = "";
            string siid = "";
            string unidad = "";
            string rdfId = "";
            string areaOfResponsibilityId = "";
            string gISId = "";

            //VoltageLevel
            foreach (XmlNode xm_1 in xnList)
            {
                zonacodi = xm_1.Attributes["B1Number"].Value.ToString();
                ///Bay
                foreach (XmlNode xm_0 in xm_1.ChildNodes)
                {
                    siid = "";
                    unidad = "";
                    rdfId = "";
                    areaOfResponsibilityId = "";

                    //Analog
                    foreach (XmlNode xm4 in xm_0.ChildNodes)
                    {
                        try
                        {
                            if(xm4.Attributes["Name"] == null || xm4.Attributes["RdfId"] == null || xm4.Attributes["AreaOfResponsibilityId"] == null)
                            {
                                continue;
                            }

                            unidad = xm4.Attributes["Name"].Value.ToString();
                            rdfId = xm4.Attributes["RdfId"].Value.ToString();
                            areaOfResponsibilityId = xm4.Attributes["AreaOfResponsibilityId"].Value.ToString();

                            gISId = xm4.Attributes["GISId"].Value.ToString();

                            if (gISId == "")
                                gISId = "-1";


                            //AnalogValue
                            foreach (XmlNode xm5 in xm4.ChildNodes)
                            {

                                if (xm5.LocalName == "AnalogValue")
                                {
                                    siid = xm5.Attributes["Siid"].Value.ToString();

                                    var canalcodi = Int32.Parse(siid); // convertir valor entero
                                    var canalCoes = listaCanalesTRCOES.Find(x => x.Canalcodi == canalcodi);
                                    if (canalCoes != null)
                                    {
                                        var emprcodi = Int32.Parse(areaOfResponsibilityId); // convertir valor entero
                                        var empresaCoes = listaEmpresasTRCOES.Find(x => x.Emprcodi == emprcodi);
                                        if (empresaCoes != null)
                                        {
                                            bool tieneCambios = false;
                                            var codigoZona = Int32.Parse(zonacodi); //convertir valor entero
                                            var codigoGisid = Int32.Parse(gISId); //convertir valor entero
                                                                                  //Validar si existen cambios
                                            if (canalCoes.Emprcodi != emprcodi || canalCoes.Zonacodi != codigoZona ||
                                                (canalCoes.Canalunidad ?? "").Trim() != (unidad ?? "").Trim())
                                            {
                                                tieneCambios = true;
                                            }

                                            if (tieneCambios)
                                            {
                                                scadaServicio.ActualizarCanalSiid(Int32.Parse(areaOfResponsibilityId), Int32.Parse(zonacodi), unidad, User.Identity.Name, Int32.Parse(siid));
                                                registros++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }
            }

            registrosActualizados += registros;
            DateTime fechaFin = DateTime.Now;
            return registros + " registros (Tiempo transcurrido: " + ObtenerDiferenciaTiempo(fechaIni, fechaFin) + " s.)";
        }


        /// <summary>
        /// Permite actualizar un canal con su empresa, area, rdfid, unidad, gisid.
        /// </summary>
        /// <param name="xML">Modelo CIM</param>
        /// <param name="nodo">Nodo de referencia a buscar</param>
        /// <returns></returns>
        public string CIMObtenerSiidAnalogico1nivel(string xML, string nodo, List<TrCanalSp7DTO> listaCanalesTRCOES, List<TrEmpresaSp7DTO> listaEmpresasTRCOES, ref int registrosActualizados)
        {
            int registros = 0;
            DateTime fechaIni = DateTime.Now;
            XmlDocument xml = new XmlDocument();
            xml.Load(xML);
            XmlNodeList xnList = xml.SelectNodes(nodo);
            string zonacodi = "";
            string siid = "";
            string unidad = "";
            string rdfId = "";
            string areaOfResponsibilityId = "";
            string gISId = "";

            //Substation
            foreach (XmlNode xm in xnList)
            {
                //B1Number(zonacodi)
                zonacodi = xm.Attributes["B1Number"].Value.ToString();
                siid = "";
                unidad = "";
                rdfId = "";
                areaOfResponsibilityId = "";

                //Analog
                foreach (XmlNode xm4 in xm.ChildNodes)
                {
                    try
                    {
                        if (xm4.Attributes["Name"] == null || xm4.Attributes["RdfId"] == null || xm4.Attributes["AreaOfResponsibilityId"] == null || xm4.Attributes["GISId"] == null)
                        {
                            continue;
                        }

                        unidad = xm4.Attributes["Name"].Value.ToString();
                        rdfId = xm4.Attributes["RdfId"].Value.ToString();
                        areaOfResponsibilityId = xm4.Attributes["AreaOfResponsibilityId"].Value.ToString();

                        gISId = xm4.Attributes["GISId"].Value.ToString();

                        if (gISId == "")
                            gISId = "-1";

                        //AnalogValue
                        foreach (XmlNode xm5 in xm4.ChildNodes)
                        {

                            if (xm5.LocalName == "AnalogValue")
                            {
                                siid = xm5.Attributes["Siid"].Value.ToString();

                                var canalcodi = Int32.Parse(siid); // convertir valor entero
                                var canalCoes = listaCanalesTRCOES.Find(x => x.Canalcodi == canalcodi);
                                if (canalCoes != null)
                                {
                                    var emprcodi = Int32.Parse(areaOfResponsibilityId); // convertir valor entero
                                    var empresaCoes = listaEmpresasTRCOES.Find(x => x.Emprcodi == emprcodi);
                                    if (empresaCoes != null)
                                    {
                                        bool tieneCambios = false;
                                        var codigoZona = Int32.Parse(zonacodi); //convertir valor entero
                                        var codigoGisid = Int32.Parse(gISId); //convertir valor entero
                                                                              //Validar si existen cambios
                                        if (canalCoes.Emprcodi != emprcodi || canalCoes.Zonacodi != codigoZona ||
                                            (canalCoes.Canalunidad ?? "").Trim() != (unidad ?? "").Trim())
                                        {
                                            tieneCambios = true;
                                        }

                                        if (tieneCambios)
                                        {
                                            scadaServicio.ActualizarCanalSiid(Int32.Parse(areaOfResponsibilityId), Int32.Parse(zonacodi), unidad, User.Identity.Name, Int32.Parse(siid));
                                            registros++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
            }

            registrosActualizados += registros;
            DateTime fechaFin = DateTime.Now;
            double tiempo = ObtenerDiferenciaTiempo(fechaIni, fechaFin);
            return registros + " registros (Tiempo transcurrido: " + tiempo + " s.)";



        }


        /// <summary>
        /// Permite actualizar señales digitales con su empresa, area, rdfid, unidad, gisid a nivel de subestación
        /// </summary>
        /// <param name="xML">Modelo CIM</param>
        /// <returns></returns>
        public string CIMObtenerSiidDigital(string xML, List<TrCanalSp7DTO> listaCanalesTRCOES, List<TrEmpresaSp7DTO> listaEmpresasTRCOES, ref int registrosActualizados)
        {
            int registros = 0;
            DateTime fechaIni = DateTime.Now;
            XmlDocument xml = new XmlDocument();
            xml.Load(xML);
            XmlNodeList xnList = xml.SelectNodes("XDF/Instances/Parent/GeographicalRegion/SubGeographicalRegion/Substation");
            string zonacodi = "";
            string siid = "";
            string unidad = "";
            string rdfId = "";
            string areaOfResponsibilityId = "";

            string csv = "";

            //Substation
            foreach (XmlNode xm in xnList)
            {
                //B1Number(zonacodi)
                zonacodi = xm.Attributes["B1Number"].Value.ToString();
                siid = "";
                unidad = "";
                rdfId = "";
                areaOfResponsibilityId = "";
                string gISId = "";
                //VoltageLevel
                foreach (XmlNode xm2 in xm.ChildNodes)
                {
                    //Bay
                    foreach (XmlNode xm3_ in xm2.ChildNodes)
                    {

                        //Breaker
                        foreach (XmlNode xm3 in xm3_.ChildNodes)
                        {
                            //Discrete
                            foreach (XmlNode xm4 in xm3.ChildNodes)
                            {
                                try
                                {
                                    if(xm4.Attributes["Name"] == null || xm4.Attributes["AreaOfResponsibilityId"] == null || xm4.Attributes["GISId"] == null || xm4.Attributes["RdfId"] == null)
                                    {
                                        continue;
                                    }

                                    unidad = xm4.Attributes["Name"].Value.ToString();
                                    areaOfResponsibilityId = xm4.Attributes["AreaOfResponsibilityId"].Value.ToString();
                                    gISId = xm4.Attributes["GISId"].Value.ToString();

                                    if (gISId == "")
                                        gISId = "-1";

                                    rdfId = xm4.Attributes["RdfId"].Value.ToString();

                                    //DiscreteValue
                                    foreach (XmlNode xm5 in xm4.ChildNodes)
                                    {
                                        if (xm5.LocalName == "DiscreteValue")
                                        {
                                            siid = xm5.Attributes["Siid"].Value.ToString();

                                            var canalcodi = Int32.Parse(siid); // convertir valor entero
                                            var canalCoes = listaCanalesTRCOES.Find(x => x.Canalcodi == canalcodi);
                                            if (canalCoes != null)
                                            {
                                                var emprcodi = Int32.Parse(areaOfResponsibilityId); // convertir valor entero
                                                var empresaCoes = listaEmpresasTRCOES.Find(x => x.Emprcodi == emprcodi);
                                                if (empresaCoes != null)
                                                {
                                                    bool tieneCambios = false;
                                                    var codigoZona = Int32.Parse(zonacodi); //convertir valor entero
                                                    var codigoGisid = Int32.Parse(gISId); //convertir valor entero
                                                                                          //Validar si existen cambios
                                                    if (canalCoes.Emprcodi != emprcodi || canalCoes.Zonacodi != codigoZona ||
                                                        (canalCoes.Canalunidad ?? "").Trim() != (unidad ?? "").Trim())
                                                    {
                                                        tieneCambios = true;
                                                    }

                                                    if (tieneCambios)
                                                    {
                                                        scadaServicio.ActualizarCanalSiid(Int32.Parse(areaOfResponsibilityId), Int32.Parse(zonacodi), unidad, User.Identity.Name, Int32.Parse(siid));
                                                        registros++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            }

            registrosActualizados += registros;
            DateTime fechaFin = DateTime.Now;
            double tiempo = ObtenerDiferenciaTiempo(fechaIni, fechaFin);
            return registros + " registros (Tiempo transcurrido: " + tiempo + " s.)";
        }


        /// <summary>
        /// Permite actualizar señales digitales con su empresa, area, rdfid, unidad, gisid a nivel de subestación
        /// </summary>
        /// <param name="XML">Modelo CIM</param>
        /// <returns></returns>
        public string CIMObtenerSiidDigitalBay(string XML, List<TrCanalSp7DTO> listaCanalesTRCOES, List<TrEmpresaSp7DTO> listaEmpresasTRCOES, ref int registrosActualizados)
        {
            int registros = 0;
            DateTime fechaIni = DateTime.Now;
            XmlDocument xml = new XmlDocument();
            xml.Load(XML);
            XmlNodeList xnList = xml.SelectNodes("XDF/Instances/Parent/GeographicalRegion/SubGeographicalRegion/Substation");
            string zonacodi = "";
            string siid = "";
            string unidad = "";
            string rdfId = "";
            string areaOfResponsibilityId = "";

            string csv = "";

            //Substation
            foreach (XmlNode xm in xnList)
            {
                //B1Number(zonacodi)
                zonacodi = xm.Attributes["B1Number"].Value.ToString();
                siid = "";
                unidad = "";
                rdfId = "";
                areaOfResponsibilityId = "";
                string gISId = "";

                //VoltageLevel
                foreach (XmlNode xm2 in xm.ChildNodes)
                {
                    //Bay
                    foreach (XmlNode xm3_ in xm2.ChildNodes)
                    {
                        //Discrete
                        foreach (XmlNode xm4 in xm3_.ChildNodes)
                        {
                            try
                            {
                                if (xm4.Attributes["Name"] == null || xm4.Attributes["AreaOfResponsibilityId"] == null || xm4.Attributes["GISId"] == null || xm4.Attributes["RdfId"] == null)
                                {
                                    continue;
                                }

                                unidad = xm4.Attributes["Name"].Value.ToString();
                                areaOfResponsibilityId = xm4.Attributes["AreaOfResponsibilityId"].Value.ToString();
                                gISId = xm4.Attributes["GISId"].Value.ToString();

                                if (gISId == "")
                                    gISId = "-1";

                                rdfId = xm4.Attributes["RdfId"].Value.ToString();

                                //DiscreteValue
                                foreach (XmlNode xm5 in xm4.ChildNodes)
                                {
                                    if (xm5.LocalName == "DiscreteValue")
                                    {
                                        siid = xm5.Attributes["Siid"].Value.ToString();

                                        var canalcodi = Int32.Parse(siid); // convertir valor entero
                                        var canalCoes = listaCanalesTRCOES.Find(x => x.Canalcodi == canalcodi);
                                        if (canalCoes != null)
                                        {
                                            var emprcodi = Int32.Parse(areaOfResponsibilityId); // convertir valor entero
                                            var empresaCoes = listaEmpresasTRCOES.Find(x => x.Emprcodi == emprcodi);
                                            if (empresaCoes != null)
                                            {
                                                bool tieneCambios = false;
                                                var codigoZona = Int32.Parse(zonacodi); //convertir valor entero
                                                var codigoGisid = Int32.Parse(gISId); //convertir valor entero
                                                //Validar si existen cambios
                                                if (canalCoes.Emprcodi != emprcodi || canalCoes.Zonacodi != codigoZona ||
                                                    (canalCoes.Canalunidad ?? "").Trim() != (unidad ?? "").Trim())
                                                {
                                                    tieneCambios = true;
                                                }

                                                if (tieneCambios)
                                                {
                                                    scadaServicio.ActualizarCanalSiid(Int32.Parse(areaOfResponsibilityId), Int32.Parse(zonacodi), unidad, User.Identity.Name, Int32.Parse(siid));
                                                    registros++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }
                        }

                    }
                }
            }

            registrosActualizados += registros;
            DateTime fechaFin = DateTime.Now;
            double tiempo = ObtenerDiferenciaTiempo(fechaIni, fechaFin);
            return registros + " registros (Tiempo transcurrido: " + tiempo + " s.)";
        }



        /// <summary>
        /// Permite obtener del modelo CIM las empresas, zona, identificador RdfId, Unidad y GISId.
        /// </summary>
        /// <param name="xML">Modelo CIM</param>
        /// <returns></returns>
        public string CIMObtenerSiidDigitalPTransf(string xML, List<TrCanalSp7DTO> listaCanalesTRCOES, List<TrEmpresaSp7DTO> listaEmpresasTRCOES, ref int registrosActualizados)
        {

            int registros = 0;
            DateTime fechaini = DateTime.Now;
            XmlDocument xml = new XmlDocument();
            xml.Load(xML);
            XmlNodeList xnList = xml.SelectNodes("XDF/Instances/Parent/GeographicalRegion/SubGeographicalRegion/Substation");
            string zonacodi = "";
            string siid = "";
            string unidad = "";
            string rdfId = "";
            string areaOfResponsibilityId = "";
            string csv = "";

            //Substation
            foreach (XmlNode xm in xnList)
            {
                //B1Number(zonacodi)
                zonacodi = xm.Attributes["B1Number"].Value.ToString();
                siid = "";
                unidad = "";
                rdfId = "";
                areaOfResponsibilityId = "";
                string gISId = "";


                //PowerTransformer
                foreach (XmlNode xm3 in xm.ChildNodes)
                {
                    //Discrete
                    foreach (XmlNode xm4 in xm3.ChildNodes)
                    {
                        try
                        {
                            if (xm4.Attributes["Name"] == null || xm4.Attributes["AreaOfResponsibilityId"] == null || xm4.Attributes["GISId"] == null || xm4.Attributes["RdfId"] == null)
                            {
                                continue;
                            }

                            unidad = xm4.Attributes["Name"].Value.ToString();

                            areaOfResponsibilityId = xm4.Attributes["AreaOfResponsibilityId"].Value.ToString();
                            gISId = xm4.Attributes["GISId"].Value.ToString();

                            if (gISId == "")
                                gISId = "-1";

                            rdfId = xm4.Attributes["RdfId"].Value.ToString();

                            //DiscreteValue
                            foreach (XmlNode xm5 in xm4.ChildNodes)
                            {
                                if (xm5.LocalName == "DiscreteValue")
                                {
                                    siid = xm5.Attributes["Siid"].Value.ToString();

                                    var canalcodi = Int32.Parse(siid); // convertir valor entero
                                    var canalCoes = listaCanalesTRCOES.Find(x => x.Canalcodi == canalcodi);
                                    if (canalCoes != null)
                                    {
                                        var emprcodi = Int32.Parse(areaOfResponsibilityId); // convertir valor entero
                                        var empresaCoes = listaEmpresasTRCOES.Find(x => x.Emprcodi == emprcodi);
                                        if (empresaCoes != null)
                                        {
                                            bool tieneCambios = false;
                                            var codigoZona = Int32.Parse(zonacodi); //convertir valor entero
                                            var codigoGisid = Int32.Parse(gISId); //convertir valor entero
                                                                                  //Validar si existen cambios
                                            if (canalCoes.Emprcodi != emprcodi || canalCoes.Zonacodi != codigoZona ||
                                                (canalCoes.Canalunidad ?? "").Trim() != (unidad ?? "").Trim())
                                            {
                                                tieneCambios = true;
                                            }

                                            if (tieneCambios)
                                            {
                                                scadaServicio.ActualizarCanalSiid(Int32.Parse(areaOfResponsibilityId), Int32.Parse(zonacodi), unidad, User.Identity.Name, Int32.Parse(siid));
                                                registros++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }

            }

            registrosActualizados += registros;
            DateTime fechaFin = DateTime.Now;
            double tiempo = ObtenerDiferenciaTiempo(fechaini, fechaFin);
            return registros + " registros (Tiempo transcurrido: " + tiempo + " s.)";
        }

        /// <summary>
        /// Permite obtener atributos de un modelo CIM
        /// </summary>
        /// <param name="xML">modelo CIM</param>
        /// <param name="nodo">Nodo de referencia</param>
        /// <param name="arrAtributos">Lista de atributos</param>
        /// <returns></returns>
        public string[,] CIMObtenerSiidNodoAtributos(string xML, string nodo, ArrayList arrAtributos)
        {
            DateTime fechaIni = DateTime.Now;
            string[,] arrValores;
            XmlDocument xml = new XmlDocument();
            xml.Load(xML);

            XmlNodeList xnList = xml.SelectNodes(nodo);

            if (xnList.Count <= 0)
                return null;

            arrValores = new string[xnList.Count, arrAtributos.Count];


            string val = "";
            int fil = 0;
            foreach (XmlNode xm in xnList)
            {

                int li_col = 0;

                foreach (string atrib in arrAtributos)
                {
                    val = xm.Attributes[atrib].Value.ToString();
                    arrValores[fil, li_col] = val;

                    li_col++;
                }

                fil++;
            }

            return arrValores;
        }

        /// <summary>
        /// Permite obtener la diferencia de tiempo de dos fechas
        /// </summary>
        /// <param name="pdt_fini">Fecha inicial</param>
        /// <param name="pdt_ffin">Fecha final</param>
        /// <returns>diferencia de tiempo (segundos)</returns>
        public double ObtenerDiferenciaTiempo(DateTime pdt_fini, DateTime pdt_ffin)
        {
            TimeSpan timeDifference = pdt_ffin.Subtract(pdt_fini);
            return timeDifference.TotalSeconds;
        }

        #endregion

        #region "Dato Minuto SP7"

        public ActionResult Datofechasp7()
        {
            EventoAppServicio evento = new EventoAppServicio();
            MigracionesModel model = new MigracionesModel();

            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaYYYYMMDD);
            model.GetListaEmpresaRis7 = evento.ListarEmpresas().Where(x => (x.SCADACODI > 0) && (x.EMPRESTADO == "A")).Select(x => new ScEmpresaDTO { Emprcodi = x.EMPRCODI, Emprenomb = x.EMPRRAZSOCIAL, Scadacodi = x.SCADACODI }).OrderBy(x => x.Emprenomb).ToList();

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarZonas(int emprcodi)
        {
            ScadaSp7AppServicio servScada = new ScadaSp7AppServicio();
            MigracionesModel model = new MigracionesModel();
            model.ListaTrzonas = servScada.ListTrZonaSp7sByEmpresaBdTreal(emprcodi).OrderBy(x => x.Zonanomb).ToList();
            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zonacodi"></param>
        /// <returns></returns>
        public JsonResult CargarCanales(int zonacodi)
        {
            MigracionesModel model = new MigracionesModel();

            var unidad = this.servicio.GetTipoinformacionByTipoinfocodi(ConstantesAppServicio.TipoinfocodiMW);
            if (unidad != null)
            {
                var lista = servicio.ListTrCanalSp7sByZonaAndUnidad(ConstantesTrCanal.TipoPuntoAnalogico, zonacodi, unidad.Canalunidad).OrderBy(x => x.Canalnomb).ToList();

                model.Resultado = servicio.ListarCanalesHtml(lista);
                model.nRegistros = lista.Count;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="canalcodi"></param>
        /// <returns></returns>
        public JsonResult LoadViewScada(string fecha, string canalcodi)
        {
            try
            {
                this.Sfecha = fecha;
                this.Scanalcodi = canalcodi;
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewScada()
        {
            MigracionesModel model = new MigracionesModel();

            DateTime f_ = DateTime.MinValue;

            if (this.Sfecha != null)
            {
                f_ = DateTime.ParseExact(this.Sfecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            }

            var lista = servicio.ListTrScadasp7(f_, this.Scanalcodi);

            if (lista.Count > 0)
            {
                var cabecera = servicio.ListaTrCanalsp7(this.Scanalcodi);

                model.Resultado = servicio.ListarTrScadasp7Html(lista, cabecera);
                model.nRegistros = lista.Count;
            }
            else { model.Resultado = "Sin informacion..."; }

            return View(model);
        }

        #endregion
        public void EscribirTextoLog(string textoConfigurar, string Texto)
        {
            Log.Info("Última actualización: " + DateTime.Now.ToString("dd/MM/yyy HH:mm:ss") + " " + textoConfigurar);
        }
    }
}