using COES.Dominio.DTO.Scada;
using COES.MVC.Intranet.Areas.Coordinacion.Helper;
using COES.MVC.Intranet.Areas.Coordinacion.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.RegistroObservacion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Coordinacion.Controllers
{
    public class AsociacionEmpresaController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        RegistroObservacionAppServicio servicio = new RegistroObservacionAppServicio();        

        /// <summary>
        /// Action para mostrar la página inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            AsociacionEmpresanModel model = new AsociacionEmpresanModel();
            model.ListaEmpresasSp7 = this.servicio.ListarEmpresasSp7();
            model.ListaEmpresas = this.servicio.ListarEmpresas();

            return View(model);
        }

        /// <summary>
        /// permite mostrar el listado de asociacion empresas
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(string nombre)
        {
            if (nombre == "")
                nombre = "-1";
            
            AsociacionEmpresanModel model = new AsociacionEmpresanModel();
            model.Listado = this.servicio.ObtenerBusquedaAsociocionesEmpresa(nombre);

            return PartialView(model);
        }



        /// <summary>
        /// Permite eliminar el registro de una observación
        /// </summary>
        /// <param name="emprcodi"></param>
        /// /// <param name="emprcodisic"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int emprcodi, int emprcodisic)
        {
            try
            {
                this.servicio.EliminarAsociacionesEmpresa(emprcodi, emprcodisic);
                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite generar el formato excel
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string nombre)
        {
            try
            {
                if (nombre == "")
                    nombre = "-1";

                string path = HttpContext.Server.MapPath("~/") + ConstantesCoordinacion.RutaReporte;
                string file = ConstantesCoordinacion.FileExportacionObservacion;

                AsociacionEmpresanModel model = new AsociacionEmpresanModel();
                model.Listado = this.servicio.ObtenerBusquedaAsociocionesEmpresa(nombre);
           
                this.servicio.GenerarReporteAsociacionEmpresaExcel(model.Listado, path, file);

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
            string path = HttpContext.Server.MapPath("~/") + ConstantesCoordinacion.RutaReporte;
            string fullPath = path + ConstantesCoordinacion.FileExportacionObservacion;
            return File(fullPath, Constantes.AppExcel, ConstantesCoordinacion.FileExportacionObservacion);
        }

        /// <summary>
        /// Permite grabar los datos de la observación
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="emprcodisp7"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(int emprcodi, int emprcodisp7)
        {
            try
            {
                this.servicio.GrabarAsociacionEmpresa(emprcodi, emprcodisp7, base.UserName);

                return Json(1);                
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite actualizar los datos del nombre de empresa del SP7
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="emprnomb"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarSp7(int emprcodi, string emprnomb)
        {
            try
            {
                this.servicio.ActualizarNombreEmpresaScadaSP7(emprcodi, emprnomb);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

    }

}
