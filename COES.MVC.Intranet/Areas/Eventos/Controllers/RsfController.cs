using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Eventos.Helper;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class RsfController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        RsfAppServicio servicio = new RsfAppServicio();
        private static readonly ILog Logger = log4net.LogManager.GetLogger(typeof(RsfController));
        private static string NombreControlador = "RsfController";

        /// <summary>
        /// Pagina inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RsfModel model = new RsfModel();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            var item = this.ObtenerRA();
            model.RaUp = item.ValorRaUp;
            model.RaDown = item.ValorRaDown;
            return View(model);
        }

        /// <summary>
        /// Permite obtener el formato de carga
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>   
        public JsonResult ObtenerEstructura(string fecha, int indicador, int operacion)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            int longitud = 0;
            List<int> indices = new List<int>();
            string path = string.Empty;

            int columnas = 0;
            int countAdicional;
            List<RsfLimite> listLimite = new List<RsfLimite>();
            string[][] datos = this.servicio.ObtenerEstructura(fechaConsulta, out longitud, out indices, true, out columnas, operacion,
                out countAdicional, out listLimite);
            RsfModel model = new RsfModel();
            model.Datos = datos;
            model.Longitud = longitud;
            model.Indices = indices.ToArray();
            model.CountAdicional = countAdicional;
            model.Columnas = columnas;
            model.ListaLimite = listLimite;
            if(operacion == 0)
            {
                model.DatosBkp = datos;
            }
            else
            {
                model.DatosBkp = this.servicio.ObtenerEstructura(fechaConsulta, out longitud, out indices, true, out columnas, 0,
                out countAdicional, out listLimite);
            }            
            model.Oper = operacion;
            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos ingresados
        /// </summary>        
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(string fecha, string[][] datos)
        {
            try
            {
                DateTime fechaDatos = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                this.servicio.Grabar(fechaDatos, datos, base.UserName);
                return Json(1);
            }
            catch(Exception ex)
            {
                Logger.Error(NombreControlador, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar los datos ingresados
        /// </summary>        
        /// <param name="datos"></param>
        /// <param name="bkp"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar2(string fecha, string[][] datos, string[][] bkp, int op)
        {
            try
            {
                DateTime fechaDatos = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                int registros = this.servicio.Grabar2(fechaDatos, datos, base.UserName, bkp, op);
                return Json(registros);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }

        #region Administrar horas

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PartialViewResult Horas(string fecha)
        {
            RsfModel model = new RsfModel();
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListaHoras = this.servicio.ObtenerHorasPorFecha(fechaConsulta);
            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos de la hora
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarHora(string fecha, string inicio, string fin, int id)
        {
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaInicio = new DateTime();
                DateTime fechaFin = new DateTime();
                string[] matInicio = inicio.Split(ConstantesAppServicio.CaracterDosPuntos);

                if (matInicio.Length == 2)
                {
                    DateTime operacion = fechaConsulta.AddHours(Convert.ToInt32(matInicio[0])).
                        AddMinutes(Convert.ToInt32(matInicio[1]));
                    fechaInicio = operacion;
                }
                string[] matFin = fin.Split(ConstantesAppServicio.CaracterDosPuntos);

                if (matFin.Length == 2)
                {
                    DateTime operacion = fechaConsulta.AddHours(Convert.ToInt32(matFin[0])).
                        AddMinutes(Convert.ToInt32(matFin[1]));
                    fechaFin = operacion;
                }
                EveRsfhoraDTO entity = new EveRsfhoraDTO();
                entity.Lastuser = base.UserName;
                entity.Lastdate = DateTime.Now;
                entity.Rsfhorcodi = id;
                entity.Rsfhorfecha = fechaConsulta;
                entity.Rsfhorfin = fechaFin;
                entity.Rsfhorinicio = fechaInicio;

                this.servicio.GrabarHora(entity);

                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar los datos de la hora
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarHora(int id)
        {
            try
            {
                this.servicio.EliminarHora(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        #endregion

        /// <summary>
        /// Permite cargar el archivo de potencia
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEventos.RutaReporte;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesEventos.ArchivoAgcXML;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ProcesarArchivo()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEventos.RutaReporte +
                    ConstantesEventos.ArchivoAgcXML;

                //List<RsfEstructura> listado = this.servicio.ProcesarArchivoXML(path);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        #region Exportacion

        /// <summary>
        /// Permite generar archivo de exportacion
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Generar(string fecha)
        {
            int result = 1;
            try
            {
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                int longitud = 0;
                List<int> indices = new List<int>();
                int countAdd = 0;
                List<RsfLimite> limites = new List<RsfLimite>();
                string[][] datos = this.servicio.ObtenerEstructuraExcel(fechaProceso, out longitud, out indices, true, out countAdd, 0, out countAdd, out limites);
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos;
                string file = ConstantesEventos.ExportacionRSF;
                decimal ra = this.ObtenerRA().ValorRaUp;
                this.servicio.ExportarDatos(datos, longitud, fechaProceso, path, file, ra);
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite editar el valor RA
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditarRA()
        {
            return Json(this.ObtenerRA());
        }

        /// <summary>
        /// Permite grabar el valor de RA
        /// </summary>
        /// <param name="ra"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarRA(decimal raUp, decimal raDown)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos;
                string file = ConstantesEventos.ArchivoRA;

                if (System.IO.File.Exists(path + file))
                {
                    System.IO.File.WriteAllText(path + file, raUp.ToString() + "&" + raDown.ToString());
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite exportar el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Exportar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos + ConstantesEventos.ExportacionRSF;
            return File(fullPath, Constantes.AppExcel, ConstantesEventos.ExportacionRSF);
        }

        /// <summary>
        /// Permite exportar el archivo cada media hora
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarMediaHora(string fecha)
        {
            int result = 1;
            try
            {
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos;
                string file = ConstantesEventos.ExportacionRSF30;
                this.servicio.ExportarDatos30(fechaProceso, path, file);
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite descargar el archivo cada media hora
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarMediaHora()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos + ConstantesEventos.ExportacionRSF30;
            return File(fullPath, Constantes.AppExcel, ConstantesEventos.ExportacionRSF30);
        }

        /// <summary>
        /// Permite exportar el archivo cada media hora
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReservaAsignada(string fechaInicio, string fechaFin)
        {
            int result = 1;
            try
            {
                DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos;
                string file = ConstantesEventos.ExportacionRSFReporte;
                this.servicio.ExportarReporte(fecInicio, fecFin, path, file);
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite descargar el archivo cada media hora
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReservaAsignada()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos + ConstantesEventos.ExportacionRSFReporte;
            return File(fullPath, Constantes.AppExcel, ConstantesEventos.ExportacionRSFReporte);
        }

        /// <summary>
        /// Permite obtener el valor de RA
        /// </summary>
        /// <returns></returns>
        private TipoReserva ObtenerRA()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos;
                string file = ConstantesEventos.ArchivoRA;
                decimal valorUp = 0;
                decimal valorDown = 0;
                if (System.IO.File.Exists(path + file))
                {
                    string text = System.IO.File.ReadAllText(path + file);

                    string[] cad = text.Split('&');



                    if (cad.Length == 2)
                    {
                        if (decimal.TryParse(cad[0], out valorUp)) { }
                        if (decimal.TryParse(cad[1], out valorDown)) { }
                    }
                }

                return new TipoReserva { ValorRaUp = valorUp, ValorRaDown = valorDown };
            }
            catch
            {
                return new TipoReserva { ValorRaUp = 0, ValorRaDown = 0 };
            }
        }

        #endregion             

        #region Configuracion RSF

        public ActionResult Configuracion()
        {
            return View();
        }

        

        [HttpPost]
        public JsonResult ObtenerConfiguracion()
        {
            int longitud = 0;
            List<int> indices = new List<int>();
            string[][] datos = this.servicio.ObtenerEstructuraConfiguracion(DateTime.Now, out longitud, out indices);
            RsfModel model = new RsfModel();
            model.Datos = datos;
            model.Longitud = longitud;
            model.Indices = indices.ToArray();

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos ingresados
        /// </summary>        
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarConfiguracion(string[][] datos)
        {
            try
            {
                this.servicio.GrabarConfiguracion(datos, base.UserName);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        #endregion

        #region Generacion de archivos XML

        public ActionResult GenerarXML(string fecha)
        {
            RsfModel model = new RsfModel();
            model.Fecha = fecha;
            return View(model);
        }

        [HttpPost]
        public JsonResult ObtenerEstructuraXML(string fecha)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            int longitud = 0;
            List<int> indices = new List<int>();
            int columnas = 0;
            List<RsfLimite> listLimite = new List<RsfLimite>();
            string[][] datos = this.servicio.ObtenerEstructuraXML(fechaConsulta, out longitud, out indices, out columnas, out listLimite);
            RsfModel model = new RsfModel();
            model.Datos = datos;
            model.Longitud = longitud;
            model.Indices = indices.ToArray();
            model.Columnas = columnas;
            model.ListaLimite = listLimite;
            model.DatosBkp = this.servicio.ObtenerEstructuraXML2(fechaConsulta);
            return Json(model);
        }

        public JsonResult GrabarXML(string fecha, string[][] datos)
        {
            try
            {
                DateTime fechaDatos = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                this.servicio.GrabarXML(fechaDatos, datos, base.UserName);
                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }

        public JsonResult GrabarXML2(string fecha, string[][] datos, string[][] datosBkp)
        {
            try
            {
                DateTime fechaDatos = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                int reg = this.servicio.GrabarXML2(fechaDatos, datos, base.UserName, datosBkp);
                return Json(reg);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult GenerarXml(string fecha)
        {
            try
            {
                Generar(fecha);
                string ursFile = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos + ConstantesEventos.ExportacionRSF;
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos;
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                this.servicio.GeneralXml(fechaProceso, path, ConstantesEventos.ArchivoComprmidoXml, ursFile);
                return Json(1);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception("Error en la generación de los archivos xml");
            }
        }

        /// <summary>
        /// Permite descargar el archivo cada media hora
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXml()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos +  ConstantesEventos.ArchivoComprmidoXml;
            return File(fullPath, Constantes.AppXML, ConstantesEventos.ArchivoComprmidoXml);
        }

        /// <summary>
        /// Permite descargar el archivo cada media hora
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool generarXMLAGC(string fecha)
        {
            DateTime TiempoInicial = DateTime.Now;
            Logger.Info("AGC (generarXMLAGC) - INICIO: " + TiempoInicial.ToString("dd/MM/yyyy HH:mm:ss"));
            string filenameDate = fecha.Replace("/", "");
            string sourcePath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos;
            string targetPath = ConfigurationManager.AppSettings["RutaAGCRSF"];// + "/" + filenameDate;

            Logger.Info($"Metodo generarXMLAGC - Ruta Base Origen : {sourcePath}");
            Logger.Info($"Metodo generarXMLAGC - Ruta Base Destino : {targetPath}");

            try
            {
                string[] filesXML = ConfigurationManager.AppSettings["FilesAGCAllXmls"].Split('|');

                foreach (var fileXML in filesXML)
                {
                    bool isCorrecto = FileServerScada.CopiarFileAgc(sourcePath, targetPath, fileXML);

                    if (!isCorrecto)
                    {
                        Logger.Error("Error al intentar copiar el archivo => " + fileXML);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
            finally
            {
                DateTime TiempoFinal = DateTime.Now;
                Logger.Info("AGC (generarXMLAGC) - FIN: " + TiempoFinal.ToString("dd/MM/yyyy HH:mm:ss") + " con un total de " + TiempoFinal.Subtract(TiempoInicial).TotalSeconds + " segundos.");
            }

            return true;
        }


        /// <summary>
        /// Permite descargar el archivo cada media hora
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool generarXMLUnitMaxGeneration(string fecha)
        {
            DateTime TiempoInicial = DateTime.Now;
            Logger.Info("AGC (generarXMLUnitMaxGeneration) - INICIO: " + TiempoInicial.ToString("dd/MM/yyyy HH:mm:ss"));
            string filenameDate = fecha.Replace("/", "");
            string sourcePath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionEventos;
            string targetPath = ConfigurationManager.AppSettings["RutaAGCRSF"];// + "/" + filenameDate;

            Logger.Info($"Metodo generarXMLUnitMaxGeneration - Ruta Base Origen : {sourcePath}");
            Logger.Info($"Metodo generarXMLUnitMaxGeneration - Ruta Base Destino : {targetPath}");

            try
            {
                string fileXML = ConstantesEventos.ArchivoUnitMaxGeneration;
                bool isCorrecto = FileServerScada.CopiarFileAgc(sourcePath, targetPath, fileXML);

                if (!isCorrecto)
                {
                    Logger.Error("Error al intentar copiar el archivo => " + fileXML);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
            finally
            {
                DateTime TiempoFinal = DateTime.Now;
                Logger.Info("AGC (generarXMLUnitMaxGeneration) - FIN: " + TiempoFinal.ToString("dd/MM/yyyy HH:mm:ss") + " con un total de " + TiempoFinal.Subtract(TiempoInicial).TotalSeconds + " segundos.");
            }

            return true;
        }
        #endregion

        #region Grafico

        public ActionResult Graficar(string fecha)
        {
            RsfModel model = new RsfModel();
            model.Fecha = fecha;
            return View(model);
        }

        /// <summary>
        /// Permite obtener los datos para el gráfico de RSF
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GraficoDetalle(string fecha)
        {
            RsfGraficoModel model = new RsfGraficoModel();
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<RsfGrafico> list = this.servicio.ObtenerGraficoRSF(fechaConsulta);
            model.ListaGrafico = list;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ObtenerGraficoPrecio(string fecha)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<PrecioGrafico> result = this.servicio.ObtenerDatosGraficoPrecio(fechaConsulta);
            return Json(result);
        }

        #endregion



        #region Configuracion avanzada

        public ActionResult ConfAvanzada()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GrabarURS(string URSUrsnomb,string URSEmprnomb, string URSGruponomb,string URSGrupotipo,int URSEquicodi)
        {
            var nuevaURS = new EveRsfdetalleDTO
            {
                Grupocodi = 9999,
                Ursnomb=URSUrsnomb,
                Emprnomb=URSEmprnomb,
                Gruponomb=URSGruponomb,
                Grupotipo=URSGrupotipo,
                Equicodi=URSEquicodi
            };

            return Json(1);
        }


        #endregion
    }
}
