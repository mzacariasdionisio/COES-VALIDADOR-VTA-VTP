using COES.MVC.Intranet.Areas.Intervenciones.Models;
using COES.MVC.Intranet.Areas.TiempoReal.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
using log4net;
using System;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TiempoReal.Controllers
{
    public class CargaArchxmlSp7Controller : BaseController
    {
        TiempoRealAppServicio appTiempoReal = new TiempoRealAppServicio();


        #region Declaracion de variables de Sesión
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            var modelo = new CargaArchxmlSp7Model();
            modelo.FechaInicial = DateTime.Today.AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);
            modelo.FechaFinal = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);

            return View(modelo);
        }

        [HttpPost]
        public PartialViewResult ListadoCargaArchivo(CargaArchxmlSp7Model model)
        {
            DateTime fechaInicial = DateTime.ParseExact(model.FechaInicial,
                ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(model.FechaFinal,
                ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            var lstArchivos = appTiempoReal.GetByFechaTrCargaarchxmlSp7s
                (fechaInicial.Date, fechaFinal.Date);

            model.ListaCargaArchivos = lstArchivos;
            return PartialView(model);
        }


        /// <summary>
        /// Permite  Exportar a Excel los resultados de la consulta cruzada
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarCargaArchivo(CargaArchxmlSp7Model filtro)
        {

            IntervencionResultado model = new IntervencionResultado();
            try
            {
                if (filtro != null)
                {
                    ConstantesFiltro objFiltro = new ConstantesFiltro()
                    {
                        FechaFinal = DateTime.ParseExact(filtro.FechaFinal,
                ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        FechaInicial = DateTime.ParseExact(filtro.FechaInicial,
                ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        TipoReporte = 1
                    };

                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                    string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                    string fileNameSalidaTmp, fileNameReporte;
                    //Reporte con formato (logo, colores)
                    appTiempoReal.ExportarCargaXML(objFiltro, path, pathLogo, out fileNameSalidaTmp, out fileNameReporte);


                    model.Resultado = "1"; //indica si va haber una siguiente descarga
                    model.NombreArchivoTmp = fileNameSalidaTmp;
                    model.NombreArchivo = fileNameReporte;

                }
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
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        /// <param name="file">Nombre del archivo</param>
        /// <returns>Archivo</returns>
        public virtual ActionResult AbrirArchivoReporte(string file1, string file2)
        {
            byte[] buffer = null;

            //leer los bytes del archivo con nombre de archivo estandar
            if (System.IO.File.Exists(file1))
            {
                buffer = System.IO.File.ReadAllBytes(file1);

                System.IO.File.Delete(file1);
            }

            //descargar los bytes pero con nombre de archivo personalizado
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, file2);
        }
    }
}