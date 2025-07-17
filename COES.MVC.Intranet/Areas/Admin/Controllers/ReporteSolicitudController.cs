using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class ReporteSolicitudController : Controller
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
            model.ListaModulos = this.seguridad.ListarModulos().ToList();
            return View(model);
        }

        /// <summary>
        /// Permite mostrar el reporte de usuarios de la extranet
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultaSolicitud(int? empresa, int? modulo, string estado)
        {
            List<ReporteUsuarioDTO> list = this.seguridad.ObtenerReporteSolicitud(empresa, modulo, estado).ToList();
            string reporte = ReporteHelper.GenerarReporteSolicitud(list);

            return Json(reporte);
        }

        /// <summary>
        /// Permite obtener el listado de usuarios
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(int idEmpresa, int? idModulo, string estado)
        {
            ReporteUsuarioModel model = new ReporteUsuarioModel();
            model.ReporteUsuario = this.seguridad.ObtenerDetalleReporteSolicitud(idEmpresa, idModulo, estado).ToList();

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
        public JsonResult Exportar(int empresa, int? modulo, string estado)
        {
            try
            {
                if (empresa == -2) empresa = -1;
                if (estado.Equals("0")) estado = "-1";
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin;
                string file = ConstantesAdmin.ReporteSolicitudes;
                //List<ReporteUsuarioDTO> list = this.seguridad.ObtenerDetalleReporteSolicitud((int)empresa, modulo, estado).ToList();
                List<ReporteUsuarioDTO> list = this.seguridad.ObtenerDetalleReporteSolicitud(empresa, modulo, estado).ToList();
                ReporteHelper.GenerarReporteSolicitudExcel(list, path, file);

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
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin + ConstantesAdmin.ReporteSolicitudes;
            return File(fullPath, Constantes.AppExcel, ConstantesAdmin.ReporteSolicitudes);
        }
    }
}
