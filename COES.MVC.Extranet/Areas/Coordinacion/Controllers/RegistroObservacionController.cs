using COES.Dominio.DTO.Scada;
using COES.MVC.Extranet.Areas.Coordinacion.Helper;
using COES.MVC.Extranet.Areas.Coordinacion.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.RegistroObservacion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Coordinacion.Controllers
{
    public class RegistroObservacionController : BaseController
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
            RegistroObservacionModel model = new RegistroObservacionModel();
            List<EmpresaDTO> listaEmpresa = base.ListaEmpresas;
            //-Filtramos solo las empresa que tiene acceso el usuario
            model.ListaEmpresas = this.servicio.ObtenerEmpresasScada().Where(x => listaEmpresa.Any(y => x.Emprcodisic == y.EMPRCODI)).ToList();
            model.IndicadorEmpresa = (model.ListaEmpresas.Count == 0) ? false : true;
            model.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// permite mostrar el listado de observaciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(int idEmpresa, string fechaInicio, string fechaFin)
        {
            RegistroObservacionModel model = new RegistroObservacionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicio)) fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(fechaFin)) fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            model.ListadoObservacion = this.servicio.ObtenerBusquedaObservaciones(idEmpresa, fecInicio, fecFin);

            return PartialView(model);
        }

        /// <summary>
        /// Muestra la pantalla de creación o de edicion
        /// </summary>
        /// <param name="obscodi"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Editar(int id)
        {
            RegistroObservacionModel model = new RegistroObservacionModel();
            model.ListaEmpresas = this.servicio.ObtenerEmpresasScada();
            model.IdObservacion = id;

            if (id == 0)
            {
                model.Entidad = new TrObservacionDTO();
                model.Entidad.IndEdicion = Constantes.SI;
            }
            else
            {
                model.Entidad = this.servicio.ObtenerDatosObservacion(id);               
            }

            return View(model);
        }

        /// <summary>
        /// Permite obtener la grilla de señales
        /// </summary>
        /// <param name="idObservacin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarGrilla(int idObservacion)
        {
            string[][] grilla = this.servicio.ObtenerConfiguracionGrillaCanal(idObservacion, 1);
            string[] estados = this.servicio.ObtenerEstados(1);

            RegistroObservacionModel model = new RegistroObservacionModel
            {
                Datos = grilla,
                Estados = estados
            };

            return Json(model);
        }

        /// <summary>
        /// Permite eliminar el registro de una observación
        /// </summary>
        /// <param name="obscodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int obscodi)
        {
            try
            {
                this.servicio.EliminarObservacion(obscodi);
                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
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
        public JsonResult Exportar(int idEmpresa, string fechaInicio, string fechaFin)
        {
            try
            {
                string path = HttpContext.Server.MapPath("~/") + ConstantesCoordinacion.RutaReporte;
                string file = ConstantesCoordinacion.FileExportacionObservacion;

                RegistroObservacionModel model = new RegistroObservacionModel();

                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicio)) fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(fechaFin)) fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListadoObservacion = this.servicio.ObtenerBusquedaObservaciones(idEmpresa, fecInicio, fecFin);

                this.servicio.GenerarReporteExcel(model.ListadoObservacion, path, file);

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
        /// Permite mostrar la pantalla de busqueda de canales
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaCanal(int emprcodi)
        {
            RegistroObservacionModel model = new RegistroObservacionModel();
            model.ListaZonas = this.servicio.ObtenerZonasPorEmpresa(emprcodi);
            model.IdEmpresa = emprcodi;
            return PartialView(model);
        }


        /// <summary>
        /// Muestra el resultado de la busqueda
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaCanal(int idEmpresa, int idZona, string idTipo, string filtro, int nroPagina)
        {
            RegistroObservacionModel model = new RegistroObservacionModel();
            model.ListadoCanal = servicio.ObtenerCanales(idEmpresa, idZona, idTipo, filtro, nroPagina, Constantes.PageSize).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoCanal(int idEmpresa, int idZona, string idTipo, string filtro)
        {
            RegistroObservacionModel model = new RegistroObservacionModel();
            model.IndicadorPagina = false;
            int nroRegistros = servicio.ObtenerNroFilasBusquedaCanal(idEmpresa, idZona, idTipo, filtro);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener los equipos a agregar
        /// </summary>
        /// <param name="canales"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosCanal(string canales)
        {
            string[][] result = this.servicio.ObtenerCanalesSeleccion(canales);            

            return Json(result);
        }

        /// <summary>
        /// Permite grabar los datos de la observación
        /// </summary>
        /// <param name="canales"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="observacion"></param>
        /// <param name="idObservacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(string[][] canales, int idEmpresa, string observacion, int idObservacion, string tipo, string estado, string observacionAgente)
        {
            try
            {
                string descEstado = string.Empty;
                int id = this.servicio.GrabarObservacion(canales, idEmpresa, observacion, idObservacion, base.UserName, 1, out descEstado, tipo,
                    estado, observacionAgente);
                return Json(new { Id = id, Estado = descEstado });
            }
            catch (Exception)
            {               
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite ver los cambios que hubieron sobre un registro
        /// </summary>
        /// <param name="canal"></param>
        /// <param name="idObservacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult HistoriaCanal(int canal, int idObservacion)
        {
            RegistroObservacionModel model = new RegistroObservacionModel();
            model.ListaHistoriaItem = this.servicio.ObtenerHistoriaItemObservacion(canal, idObservacion);

            return PartialView(model);
        }

        /// <summary>
        /// Permite ver la vista de señales pendientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Consulta() 
        {
            RegistroObservacionModel model = new RegistroObservacionModel();
            model.ListaEmpresas = this.servicio.ObtenerEmpresasScada();

            return View(model);
        }

        /// <summary>
        /// Muestra los datos del reporte
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargarGrillaReporte(int idEmpresa)
        {
            if (idEmpresa == -2) idEmpresa = base.EmpresaId;
            RegistroObservacionModel model = new RegistroObservacionModel();
            model.ListadoSeniales = this.servicio.ObtenerReporteSeniales(idEmpresa);
            return PartialView(model);
        }
    }

}
