using COES.MVC.Publico.Areas.DirectorioImpugnaciones.Models;
using COES.Servicios.Aplicacion.DirectorioImpugnaciones;
using COES.MVC.Publico.Helper;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Framework.Base.Tools;
using System.IO;


namespace COES.MVC.Publico.Areas.DirectorioImpugnaciones.Controllers
{
    public class ImpugnacionesController : Controller
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        DirectorioImpugnacionesAppServicio servicio = new DirectorioImpugnacionesAppServicio();

        /// <summary>
        /// Muestra la página de dirección ejecutiva
        /// </summary>
        /// <returns></returns>
        public ActionResult DireccionEjecutiva()
        {
            ImpugnacionesModel model = new ImpugnacionesModel();
            model.TipoImpugnacion = new Dominio.DTO.Sic.WbTipoimpugnacionDTO();
            ViewBag.subtitulo = "Reconsideraciones";
            model.TipoImpugnacion = servicio.GetByIdWbTipoimpugnacion(1);
            return View("Impugnaciones", model);
        }

        /// <summary>
        /// Muesrta la página de directorio
        /// </summary>
        /// <returns></returns>
        public ActionResult Directorio()
        {
            ImpugnacionesModel model = new ImpugnacionesModel();
            model.TipoImpugnacion = new Dominio.DTO.Sic.WbTipoimpugnacionDTO();
            model.TipoImpugnacion = servicio.GetByIdWbTipoimpugnacion(2);
            ViewBag.subtitulo = "Reconsideraciones";
            return View("Impugnaciones", model);
        }

        /// <summary>
        /// Muestra la página de recursos de apelación
        /// </summary>
        /// <returns></returns>
        public ActionResult RecursosApelacion()
        {
            ImpugnacionesModel model = new ImpugnacionesModel();
            model.TipoImpugnacion = new Dominio.DTO.Sic.WbTipoimpugnacionDTO();
            model.TipoImpugnacion = servicio.GetByIdWbTipoimpugnacion(3);
            ViewBag.subtitulo = "Directorio e Impugnaciones";
            return View("Impugnaciones", model);
        }

        /// <summary>
        /// Devuelve la lista de impugnaciones
        /// </summary>
        /// <param name="tipoImpugnacion"></param>
        /// <param name="fecAnio"></param>
        /// <param name="fecMes"></param>
        /// <returns></returns>
        public PartialViewResult ListaImpugnaciones(int tipoImpugnacion, string fecAnio, string fecMes)
        {
            ImpugnacionesModel model = new ImpugnacionesModel();
            model.TipoImpugnacion = servicio.GetByIdWbTipoimpugnacion(tipoImpugnacion);
            DateTime fecha = DateTime.ParseExact(fecMes + ' ' + fecAnio, Constantes.FormatoMesAnio,
                CultureInfo.InvariantCulture);
            model.ListaImpugnaciones = servicio.GetByCriteriaWbImpugnacions(tipoImpugnacion, fecha);
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el cuadro de estadística
        /// </summary>
        /// <param name="maximo"></param>
        /// <param name="minimo"></param>
        /// <param name="promedio"></param>
        /// <returns></returns>
        public PartialViewResult CuadroEstadistico(string maximo, string minimo, string promedio)
        {
            ViewBag.maximo = maximo;
            ViewBag.minimo = minimo;
            ViewBag.promedio = promedio;
            return PartialView();
        }
              
        /// <summary>
        /// Permite descargar el archivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(int id)
        {
            try
            {
                ImpugnacionesModel model = new ImpugnacionesModel();
                model.Impugnacion = servicio.GetByIdWbImpugnacion(id);
                string url = "IMPG" + model.Impugnacion.Impgcodi + "." + model.Impugnacion.Impgextension;

                if (FileServer.VerificarExistenciaFile(RutaDirectorio.DirectorioImpugnaciones, url, string.Empty))
                {
                    System.IO.Stream stream = FileServer.DownloadToStream(RutaDirectorio.DirectorioImpugnaciones + url, string.Empty);
                    return File(stream, model.Impugnacion.Impgextension, model.Impugnacion.Impgtitulo + "." + model.Impugnacion.Impgextension);
                }
                else 
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
