using COES.Base.Core;
using COES.MVC.Intranet.Areas.Migraciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Migraciones.Controllers
{
    public class DigSilentController : BaseController
    {
        readonly MigracionesAppServicio servicio = new MigracionesAppServicio();

        #region Declaracion de variables de Sesión

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

        #endregion

        /// <summary>
        /// Index interface DigSilent
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcesoDigsilent()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            MigracionesModel model = new MigracionesModel();

            string lectcodi = ConstantesAppServicio.LectcodiProgSemanal + "," + ConstantesAppServicio.LectcodiProgDiario + "," + ConstantesAppServicio.LectcodiReprogDiario + "," + ConstantesAppServicio.LectcodiEjecutadoHisto + "," + ConstantesAppServicio.LectcodiAjusteDiario;

            List<string> ListaHorario = new List<string>();
            for (int x = 1; x <= 24; x++)
            {
                string val = "0" + x;
                ListaHorario.Add(val.Substring(val.Length - 2));
            }

            var ListaTipoProg = servicio.GetByCriteriaMeLectura(lectcodi);

            model.ListaString = ListaHorario;
            model.TipoProgramacion = ListaTipoProg;
            model.TipoProgramacion4 = new List<Dominio.DTO.Sic.MeLecturaDTO>();
            model.Fecha = DateTime.Now.ToString(ConstantesBase.FormatoFechaBase);

            return View(model);
        }

        /// <summary>
        /// Proceso carga de informacion generacion, manttos, demanda, transformadores, lineas
        /// </summary>
        /// <param name="program"></param>
        /// <param name="fecha"></param>
        /// <param name="rdchk"></param>
        /// <param name="bloq"></param>
        /// <param name="fuente"></param>
        /// <param name="topcodiYupana"></param>
        /// <returns></returns>
        public JsonResult ProcesarDigsilent(string program, string fecha, int rdchk, string bloq, int fuente, int topcodiYupana)
        {
            MigracionesModel model = new MigracionesModel();
            DateTime fechaPeriodo = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            try
            {
                this.servicio.ProcesarDIgSILENT(fechaPeriodo, program, rdchk, bloq, fuente, topcodiYupana, out string resultado, out string comentario, out string configuracionOpera, out string validacionDuplicadoForeignKey);
                model.nRegistros = 1;
                model.Resultado = resultado;
                model.Comentario = comentario;
                model.Resultado2 = configuracionOpera;
                model.Resultado3 = validacionDuplicadoForeignKey;
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// CargarInformacionYupana
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult CargarInformacionYupana(string fecha)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.Resultado = servicio.GenerarTablaHtmlReprograma(fechaPeriodo);
                model.nRegistros = 1;
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Proceso de crear el archivo digsilent .dle
        /// </summary>
        /// <param name="texto_"></param>
        /// <returns></returns>
        public JsonResult SaveDigSilent(string texto_, string fecha)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                DateTime dtfecha = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
                string nameFile = ConstantesMigraciones.FileDigsilente + "_" + dtfecha.ToString("yyyyMMdd") + ConstantesAppServicio.ExtensionDle;

                this.GenerarArchivoDigSilent(dtfecha, ruta + nameFile, texto_);

                model.Resultado = nameFile;
                model.nRegistros = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Generador de archivo .dle
        /// </summary>
        /// <param name="f_"></param>
        /// <param name="rutaNombreArchivo"></param>
        public void GenerarArchivoDigSilent(DateTime f_, string file, string tags)
        {
            try
            {
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                try
                {
                    string[] separador = new string[] { "~" };
                    if (!newFile.Exists)
                    {
                        using (StreamWriter sw = newFile.CreateText())
                        {
                            var texto_ = tags.Split(separador, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var d in texto_)
                            {
                                sw.WriteLine(d);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(NameController, ex);
                    throw new ArgumentException(ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

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
            return File(bytes, Constantes.AppXML, nombreArchivo);
        }

    }
}