using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class ReporteUsuarioController : Controller
    {
        /// <summary>
        /// Instancia del servicio de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Pantalla inicial del formulario
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ReporteUsuarioModel model = new ReporteUsuarioModel();
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.EMPRCODI > 1).OrderBy(x => x.EMPRNOMB).ToList();
            return View(model);
        }

        /// <summary>
        /// Permite mostrar el reporte de usuarios de la extranet
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultaUsuario(int? empresa, string estado)
        {
            if (empresa == null) empresa = -2;
            List<ReporteUsuarioDTO> list = this.seguridad.ObtenerReporteUsuario(empresa, estado).ToList();
            string reporte = ReporteHelper.GenerarReporteUsuario(list);

            return Json(reporte);
        }

        /// <summary>
        /// Permite obtener el listado de usuarios
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(int idEmpresa, string estado)
        {
            ReporteUsuarioModel model = new ReporteUsuarioModel();
            model.ReporteUsuario = this.seguridad.ObtenerDetalleReporteUsuario(idEmpresa, estado).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin;
                string file = ConstantesAdmin.ReporteUsuarios;
                List<ReporteUsuarioDTO> list = this.seguridad.ObtenerDetalleReporteUsuario(-1, (-1).ToString()).ToList();
                ReporteHelper.GenerarReporteUsuarioExcel(list, path, file);                

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin + ConstantesAdmin.ReporteUsuarios;
            return File(fullPath, Constantes.AppExcel, ConstantesAdmin.ReporteUsuarios);
        }
    }
}
