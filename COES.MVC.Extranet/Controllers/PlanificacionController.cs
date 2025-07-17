using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Planificacion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Controllers
{
    public class PlanificacionController : BaseController
    {
        string pathModelo = @"\\coes.org.pe\archivosapp\webapp\";
        string folder = @"Modelos\ModPlan\";
        int IdModplan = 1;

        /// <summary>
        /// Instancia de la clase de servicio1
        /// </summary>
        ModplanAppServicio servicio = new ModplanAppServicio();

        // GET: Planificacion
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Inicio de la página
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado()
        {
            PlanificacionModel model = new PlanificacionModel();
            model.Listado = this.servicio.ListWbVersionModplans(this.IdModplan);
            return PartialView(model);
        }

        /// <summary>
        /// Permite visualizar el detalle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Detalle(int id)
        {
            PlanificacionModel model = new PlanificacionModel();
            model.Codigo = id;
            string url = "VERSION_" + id + @"\";
            WbVersionModplanDTO entity = this.servicio.ObtenerDetalleVersion(id, pathModelo, folder + url);
            WbVersionModplanDTO entityPadre = this.servicio.GetByIdWbVersionModplan((int)entity.Vermplpadre);

            model.NombreVersion = entity.Vermpldesc;
            model.NombrePlan = entityPadre.Vermpldesc;
            model.NombreModelo = entity.RutaModelo;
            model.NombreManual = entity.RutaManual;

            return PartialView(model);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivo(int idVersion, string indicador)
        {
            string url = "VERSION_" + idVersion + @"\";
            string extension = string.Empty;
            string nombre = string.Empty;
            Stream stream = this.servicio.ObtenerArchivo(idVersion, indicador, pathModelo, folder + url, out extension, out nombre);

            WbRegistroModplanDTO entity = new WbRegistroModplanDTO()
            {
                Vermplcodi = idVersion,
                Regmodusuario = base.UserName,
                Regmodfecha = DateTime.Now,
                Emprcodi = base.EmpresaId,
                Regmodtipo = this.IdModplan,
                Arcmplcodi = int.Parse(indicador)

            };

            this.servicio.SaveRegistroModPlan(entity);

            return File(stream, extension, nombre);
        }



    }
}