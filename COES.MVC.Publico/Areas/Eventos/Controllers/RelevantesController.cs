using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Evento.Helper;
using COES.MVC.Publico.Areas.Eventos.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.Eventos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Eventos.Controllers
{
    public class RelevantesController : Controller
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>
        EventoAppServicio servicio = new EventoAppServicio();
               
        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            BusquedaEventoModel model = new BusquedaEventoModel();
            model.ListaEmpresas = this.servicio.ListarEmpresas().Where(x => x.EMPRCODI != -1 && x.EMPRCODI != 0).OrderBy(x => x.EMPRNOMB).ToList();
            model.ListaFamilias = this.servicio.ListarFamilias().Where(x => x.FAMCODI != -1 && x.FAMCODI != 0).OrderBy(x => x.FAMNOMB).ToList();
            model.ListaTipoEmpresas = this.servicio.ListarTipoEmpresas();            
            model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFechaISO);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFechaISO);
            model.ParametroDefecto = 0;

            return View(model);
        }

        /// <summary>
        /// Permite mostrar la lista de eventos
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(BusquedaEventoModel modelo)
        {
            BusquedaEventoModel model = new BusquedaEventoModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (modelo.FechaInicio != null)            
                fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            
            if (modelo.FechaFin != null)            
                fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            
            fechaFin = fechaFin.AddDays(1);

            model.ListaEventos = servicio.BuscarEventos(modelo.IdTipoEvento, fechaInicio, fechaFin, modelo.Version, modelo.Turno,
                modelo.IdTipoEmpresa, modelo.IdEmpresa, modelo.IdFamilia, modelo.IndInterrupcion, modelo.NroPagina, Constantes.PageSizeEvento,
                "evencodi", "desc", "-1", -1).ToList();            

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(BusquedaEventoModel modelo)
        {
            BusquedaEventoModel model = new BusquedaEventoModel();
            model.IndicadorPagina = false;

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (modelo.FechaInicio != null)            
                fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            
            if (modelo.FechaFin != null)            
                fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            
            fechaFin = fechaFin.AddDays(1);

            int nroRegistros = servicio.ObtenerNroFilasEvento(modelo.IdTipoEvento, fechaInicio, fechaFin, modelo.Version, modelo.Turno,
                modelo.IdTipoEmpresa, modelo.IdEmpresa, modelo.IdFamilia, modelo.IndInterrupcion, "-1", -1);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }
        
        /// <summary>
        /// Permite cargar los puntos de la empresa seleccionada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresas(int idTipoEmpresa)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();

            if (idTipoEmpresa != 0)
                entitys = this.servicio.ListarEmpresasPorTipo(idTipoEmpresa).OrderBy(x => x.EMPRNOMB).ToList();
            else
                entitys = this.servicio.ListarEmpresas().Where(x => x.EMPRCODI != 0 && x.EMPRCODI != 1).OrderBy(x => x.EMPRNOMB).ToList();            

            SelectList list = new SelectList(entitys, EntidadPropiedad.EmprCodi, EntidadPropiedad.EmprNomb);
            return Json(list);
        }

        /// <summary>
        /// Permite mostrar el detalle del Evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Detalle(int idEvento)
        {
            BusquedaEventoModel model = new BusquedaEventoModel();
            model.Entidad = this.servicio.ObtenerEvento(idEvento);

            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el reporte eventos
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarEvento(BusquedaEventoModel modelo)
        {
            string result = "1";
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (modelo.FechaInicio != null)                
                    fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);                
                if (modelo.FechaFin != null)                
                    fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                
                fechaFin = fechaFin.AddDays(1);

                List<EventoDTO> list = servicio.ExportarEventos(modelo.IdTipoEvento, fechaInicio, fechaFin, modelo.Version, modelo.Turno,
                    modelo.IdTipoEmpresa, modelo.IdEmpresa, modelo.IdFamilia, modelo.IndInterrupcion, 0, "-1").ToList();                
                ExcelDocument.GenerarReporteEvento(list, fechaInicio, fechaFin);

                result = "1";
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite exportar los datos del evento
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarEvento()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + NombreArchivo.ReporteEvento;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteEvento);
        }        
    }
}
