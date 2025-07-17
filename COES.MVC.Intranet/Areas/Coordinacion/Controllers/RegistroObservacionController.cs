using COES.Dominio.DTO.Scada;
using COES.MVC.Intranet.Areas.Coordinacion.Helper;
using COES.MVC.Intranet.Areas.Coordinacion.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.RegistroObservacion;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Coordinacion.Controllers
{
    public class RegistroObservacionController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        RegistroObservacionAppServicio servicio = new RegistroObservacionAppServicio();

        /// <summary>
        /// Action para mostrar la página inicialfv
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RegistroObservacionModel model = new RegistroObservacionModel();

            model.ListaEmpresas = this.servicio.ObtenerEmpresasScada();
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
                model.Entidad.Obscantipo = ConstantesRegistroObservacion.TipoObservacion;
                model.Entidad.Obscanestado = ConstantesRegistroObservacion.EstadoPendiente;
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
            string[][] grilla = this.servicio.ObtenerConfiguracionGrillaCanal(idObservacion, 0);
            string[] estados = this.servicio.ObtenerEstados(0);

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
                int id = this.servicio.GrabarObservacion(canales, idEmpresa, observacion, idObservacion, base.UserName, 0, out descEstado, tipo, estado, observacionAgente);
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


        #region "FIT - Señales no Disponibles"

        /// <summary>
        /// Permite mostrar el paginado 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoCanalSenalesObservadasBusqueda(int idEmpresa)
        {
            RegistroObservacionModel model = new RegistroObservacionModel();
            model.IndicadorPagina = false;
            int nroRegistros = servicio.ObtenerNroFilasBusquedaCanalSenalesObservadasBusqueda(idEmpresa);

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
        /// Muestra el resultado de la busqueda
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaCanalSenalesObservadasBusqueda(int idEmpresa, int nroPagina)
        {
            RegistroObservacionModel model = new RegistroObservacionModel();
            model.ListadoCanal = servicio.ObtenerCanalesSenalesObservadasBusqueda(idEmpresa, nroPagina, Constantes.PageSize).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Trae lista de señales observadas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerSenalesObservadas(int idEmpresa)
        {

            RegistroObservacionModel model = new RegistroObservacionModel();
            model.ListadoCanal = servicio.ObtenerSenalesObservadas(idEmpresa).ToList();

            if (model.ListadoCanal != null)
            {
                if (model.ListadoCanal.Count > 0)
                {
                    string[][] canales = new string[model.ListadoCanal.Count][];

                    for (int i = 0; i < model.ListadoCanal.Count; i++)
                    {

                        canales[i] = new string[7];
                        canales[i][0] = model.ListadoCanal[i].Canalcodi.ToString();
                        canales[i][1] = model.ListadoCanal[i].Zonanomb;
                        canales[i][2] = model.ListadoCanal[i].Canalnomb;
                        canales[i][3] = model.ListadoCanal[i].Canaliccp;
                        canales[i][4] = string.Empty;
                        canales[i][5] = model.ListadoCanal[i].Motivo; 
                        canales[i][6] = string.Empty;
                    }

                    return Json(canales);
                }
            }
            return Json("");
        }


        /// <summary>
        /// Procesar señales observadas
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarSenalesObservadas()
        {            
            return Json(this.servicio.ProcesarSenalesObservadas(-100, string.Empty).ToString());
        }
        #endregion


        /// <summary>
        /// Action para mostrar la página inicialfv
        /// </summary>
        /// <returns></returns>
        public ActionResult RegistroAutomatico()
        {
            RegistroObservacionModel model = new RegistroObservacionModel();

            model.ListaEmpresas = this.servicio.ObtenerEmpresasScada();            

            return View(model);
        }

        [HttpPost]
        public JsonResult ProcesarAutomatico(int idEmpresa, string correo, int tipo)
        {
            try
            {
                if (tipo == 1)
                {
                    this.servicio.ProcesarSenalesObservadas(idEmpresa, correo);
                }
                if (tipo == 2)
                {
                    this.servicio.ProcesarSenalesCaidaEnlace(idEmpresa, correo);
                }
                if (tipo == 3)
                {
                    this.servicio.NotificarIndicesDisponibilidad(idEmpresa, correo);
                }
                if (tipo == 4)
                {
                    this.servicio.NotificarCongelamientoSeñales(idEmpresa, correo);
                }
                

                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }

}
