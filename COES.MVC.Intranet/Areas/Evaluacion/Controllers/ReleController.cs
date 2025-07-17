using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using log4net;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.Evaluacion.Models;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Evaluacion;
using COES.MVC.Intranet.Areas.Evaluacion.Helper;
using COES.MVC.Intranet.Areas.Proteccion.Helper;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.Evaluacion.Controllers
{
    public class ReleController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ReleController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        EquipoProteccionAppServicio equipoProteccion = new EquipoProteccionAppServicio();
        ProyectoActualizacionAppServicio proyectoActualzacion = new ProyectoActualizacionAppServicio();
        ConsultaMedidoresAppServicio consultaMedidores = new ConsultaMedidoresAppServicio();
        ReleAppServicio servicioRele = new ReleAppServicio();

        public ReleController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                log.Fatal(NameController, ex);
                throw;
            }
        }

        /*INICIO RELE MANDO SINCRONIZADO*/
        [AllowAnonymous]
        public ActionResult IndexReleMandoSincronizado()
        {
            ReleMandoSincronizadoModel modelo = new ReleMandoSincronizadoModel();

            modelo.ListaSubestacion = equipoProteccion.ListSubEstacion();
            modelo.ListaCelda = new List<EprEquipoDTO>();
            modelo.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.ListaEstado = equipoProteccion.ListPropCatalogoData(ConstantesEvaluacion.EstadoLinea);

            return View(modelo);
        }

        [ActionName("IndexReleMandoSincronizado"), HttpPost]
        public ActionResult IndexReleMandoSincronizadoPost(ReleMandoSincronizadoModel datos)
        {
            return View(datos);
        }
            
        [HttpPost]
        public PartialViewResult ListaReleMandoSincronizado(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            ListadoReleMandoSincronizadoModel model = new ListadoReleMandoSincronizadoModel();
            model.ListaReleMandoSincronizado = servicioRele.ListaReleMandoSincronizado(codigoId, codigo, subestacion, celda,
            empresa, area, estado).ToList();
            return PartialView("~/Areas/Evaluacion/Views/Rele/ListaReleMandoSincronizado.cshtml", model);
        }

        public JsonResult ExportarReleMandoSincronizado(string codigoId, string codigo, int subestacion, int celda,
         int empresa, int area, string estado)
        {
            ReleMandoSincronizadoModel model = new ReleMandoSincronizadoModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelRelesMandoSincronizado;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                servicioRele.GenerarExcelExportarReleMandoSincronizado(pathDestino, fileName, codigoId, codigo, subestacion, celda, empresa, area, estado);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("ReleController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);

        }

        /*FIN RELE MANDO SINCRONIZADO*/

        /*INICIO RELE PMU*/
        [AllowAnonymous]
        public ActionResult IndexRelePmu()
        {
            RelePmuModel modelo = new RelePmuModel();

            modelo.ListaSubestacion = equipoProteccion.ListSubEstacion();
            modelo.ListaCelda = new List<EprEquipoDTO>();
            modelo.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.ListaEstado = equipoProteccion.ListPropCatalogoData(ConstantesEvaluacion.EstadoLinea);

            return View(modelo);
        }

        [ActionName("IndexRelePmu"), HttpPost]
        public ActionResult IndexRelePmuPost(RelePmuModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaRelePmu(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            ListadoRelePmuModel model = new ListadoRelePmuModel();
            model.ListaRelePMU = servicioRele.ListaRelePMU(codigoId, codigo, subestacion, celda, 
                empresa, area, estado).ToList();
            return PartialView("~/Areas/Evaluacion/Views/Rele/ListaRelePmu.cshtml", model);
        }

        public JsonResult ExportarRelePmu(string codigoId, string codigo, int subestacion, int celda,
        int empresa, int area, string estado)
        {
            RelePmuModel model = new RelePmuModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelRelesPmu;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                servicioRele.GenerarExcelExportarRelePmu(pathDestino, fileName, codigoId, codigo, subestacion, celda, empresa, area, estado);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("ReleController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);

        }

        /*FIN RELE PMU*/

        /*INICIO RELE SINCRONISMO*/
        [AllowAnonymous]
        public ActionResult IndexReleSincronismo()
        {
            ReleSincronismoModel modelo = new ReleSincronismoModel();

            modelo.ListaSubestacion = equipoProteccion.ListSubEstacion();
            modelo.ListaCelda = new List<EprEquipoDTO>();
            modelo.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.ListaEstado = equipoProteccion.ListPropCatalogoData(ConstantesEvaluacion.EstadoLinea);

            return View(modelo);
        }

        [ActionName("IndexReleSincronismo"), HttpPost]
        public ActionResult IndexReleSincronismoPost(ReleSincronismoModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaReleSincronismo(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            ListadoReleSincronismoModel model = new ListadoReleSincronismoModel();
            model.ListaReleSincronismo = servicioRele.ListaReleSincronismo(codigoId, codigo, subestacion, celda,
            empresa, area, estado).ToList();
            return PartialView("~/Areas/Evaluacion/Views/Rele/ListaReleSincronismo.cshtml", model);
        }

        public JsonResult ExportarReleSincronismo(string codigoId, string codigo, int subestacion, int celda,
          int empresa, int area, string estado)
        {
            ReleSincronismoModel model = new ReleSincronismoModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelReleSincronismo;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                servicioRele.GenerarExcelExportarReleSincronismo(pathDestino, fileName, codigoId, codigo, subestacion, celda, empresa, area, estado);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("ReleController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);

        }

        /*FIN RELE SINCRONISMO*/

        /*INICIO RELE SOBRETENSION*/
        [AllowAnonymous]
        public ActionResult IndexReleSobretension()
        {
            ReleSobretensionModel modelo = new ReleSobretensionModel();

            modelo.ListaSubestacion = equipoProteccion.ListSubEstacion();
            modelo.ListaCelda = new List<EprEquipoDTO>();
            modelo.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.ListaEstado = equipoProteccion.ListPropCatalogoData(ConstantesEvaluacion.EstadoLinea);

            return View(modelo);
        }

        [ActionName("IndexReleSobretension"), HttpPost]
        public ActionResult IndexReleSobretensionPost(ReleSobretensionModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaReleSobretension(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            ListadoReleSobretensionModel model = new ListadoReleSobretensionModel();
            model.ListaReleSobretension = servicioRele.ListaReleSobretension(codigoId, codigo, subestacion, celda,
            empresa, area, estado).ToList();
            return PartialView("~/Areas/Evaluacion/Views/Rele/ListaReleSobretension.cshtml", model);
        }

        public JsonResult ExportarReleSobreTension(string codigoId, string codigo, int subestacion, int celda,
        int empresa, int area, string estado)
        {
            ReleSobretensionModel model = new ReleSobretensionModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelReleSobreTension;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                servicioRele.GenerarExcelExportarReleSobreTension(pathDestino, fileName, codigoId, codigo, subestacion, celda, empresa, area, estado);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("ReleController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);

        }

        /*FIN RELE SOBRETENSION*/

        /*INICIO RELE TORSIONAL*/
        [AllowAnonymous]
        public ActionResult IndexReleTorsional()
        {
            ReleTorsionalModel modelo = new ReleTorsionalModel();

            modelo.ListaSubestacion = equipoProteccion.ListSubEstacion();
            modelo.ListaCelda = new List<EprEquipoDTO>();
            modelo.ListaEmpresa = consultaMedidores.ListObtenerEmpresaSEINProtecciones();
            modelo.ListaArea = servicioEquipamiento.ListarZonasxNivel(5);
            modelo.ListaEstado = equipoProteccion.ListPropCatalogoData(ConstantesEvaluacion.EstadoLinea);

            return View(modelo);
        }

        [ActionName("IndexReleTorsional"), HttpPost]
        public ActionResult IndexReleTorsionalPost(ReleTorsionalModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public PartialViewResult ListaReleTorsional(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado)
        {
            ListadoReleTorsionalModel model = new ListadoReleTorsionalModel();
            model.ListaReleTorsional = servicioRele.ListaReleTorcional(codigoId, codigo, subestacion, celda,
            empresa, area, estado).ToList();
            return PartialView("~/Areas/Evaluacion/Views/Rele/ListaReleTorsional.cshtml", model);
        }

        public JsonResult ExportarReleTorsional(string codigoId, string codigo, int subestacion, int celda,
      int empresa, int area, string estado)
        {
            ReleTorsionalModel model = new ReleTorsionalModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesEvaluacion.NombrePlantillaExcelRelesTorsionales;
                string pathOrigen = ConstantesEvaluacion.FolderGestProtec + "/" + ConstantesEvaluacion.Plantilla;
                string pathDestino = FileServer.GetDirectory() + ConstantesEvaluacion.FolderTemporal + "/";

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);

                servicioRele.GenerarExcelExportarReleTorsional(pathDestino, fileName, codigoId, codigo, subestacion, celda, empresa, area, estado);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
                model.StrMensaje = "";


            }
            catch (Exception ex)
            {
                log.Error("ReleController", ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);

        }

        /*FIN RELE TORSIONAL*/

        [HttpPost]
        public JsonResult ListaCelda(int idSubEstacion)
        {
            return Json(equipoProteccion.ListCelda(idSubEstacion));
        }


        public virtual FileResult AbrirArchivo(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");

            byte[] buffer = new EvaluacionHelper().GetBufferArchivoAdjunto(file, base.PathFiles, ConstantesEvaluacion.FolderTemporal);
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, sFecha + "_" + file);
        }

    }
}
