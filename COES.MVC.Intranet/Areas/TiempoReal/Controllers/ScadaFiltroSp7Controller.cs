using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.TiempoReal.Models;
using COES.Dominio.DTO.Scada;
using COES.Servicios.Aplicacion.TiempoReal;

namespace COES.MVC.Intranet.Areas.TiempoReal.Controllers
{
    public class ScadaFiltroSp7Controller : BaseController
    {        
        ScadaSp7AppServicio servScadaSp7 = new ScadaSp7AppServicio();

        public ActionResult Index()
        {
            BusquedaMeScadaFiltroSp7Model model = new BusquedaMeScadaFiltroSp7Model();
            return View(model);
        }

        /// <summary>
        /// Permite obtener una lista filtrada por Zona (Ubicación)
        /// </summary>
        /// <param name="zonaCodi">Código de Zona</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaCanalPorZona(int zonaCodi)
        {
            BusquedaMeScadaFiltroSp7Model model = new BusquedaMeScadaFiltroSp7Model();
            model.ListaTrCanalSp7 = servScadaSp7.GetByZonaAnalogicoTrCanalSp7(zonaCodi);

            return Json(model.ListaTrCanalSp7);
        }


        /// <summary>
        /// Permite editar el Filtro y sus elementos
        /// </summary>
        /// <param name="id">Identificador de Filtro</param>
        /// <param name="accion">Acción: 1: edicion</param>
        ///                              0: visualizar
        /// <returns></returns>
        public ActionResult Editar(int id, int accion)
        {

            MeScadaFiltroSp7Model model = new MeScadaFiltroSp7Model();
            MeScadaFiltroSp7DTO meScadaFiltroSp7 = null;
            model.ListaTrZonaSp7 = servScadaSp7.ListTrZonaSp7s();
            model.ListaTrCanalSp7 = new List<Dominio.DTO.Scada.TrCanalSp7DTO>();

            if (id != 0)
            {
                meScadaFiltroSp7 = servScadaSp7.GetByIdMeScadaFiltroSp7(id);
                model.ListaTrCanalSp7 = servScadaSp7.GetByFiltroTrCanalSp7(id);
            }

            if (meScadaFiltroSp7 != null)
            {
                model.MeScadaFiltroSp7 = meScadaFiltroSp7;
            }
            else
            {
                meScadaFiltroSp7 = new MeScadaFiltroSp7DTO();
                model.MeScadaFiltroSp7 = meScadaFiltroSp7;
            }

            model.Accion = accion;
            return View(model);
        }


        /// <summary>
        /// Permite eliminar un filtro de usuario y sus elementos
        /// </summary>
        /// <param name="id">Identificador de Filtro</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                MeScadaFiltroSp7DTO meScadaFiltroSp7;

                if (id != 0)
                {

                    meScadaFiltroSp7 = servScadaSp7.GetByIdMeScadaFiltroSp7(id);

                    if (User.Identity.Name == meScadaFiltroSp7.Filtrouser)
                    {
                        servScadaSp7.DeleteFiltroMeScadaPtofiltroSp7(id);
                        servScadaSp7.DeleteMeScadaFiltroSp7(id);
                    }
                    else
                    {
                        return Json(-2);
                    }

                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite grabar el registro de Filtro y sus elementos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(MeScadaFiltroSp7Model model)
        {
            try
            {
                MeScadaFiltroSp7DTO entity = new MeScadaFiltroSp7DTO();

                entity.Filtrocodi = model.FiltroCodi;
                entity.Filtronomb = model.FiltroNomb;

                if (entity.Filtrocodi == 0)
                {
                    entity.Filtrouser = User.Identity.Name;
                    entity.Scdfifeccreacion = DateTime.Now;
                }
                else
                {
                    entity.Scdfiusumodificacion = User.Identity.Name;
                    entity.Scdfifecmodificacion = DateTime.Now;

                    entity.Filtrouser = model.FiltroUser;
                    entity.Scdfifeccreacion = DateTime.ParseExact(model.ScdFiFecCreacion, Constantes.FormatoFechaFull,
                        CultureInfo.InvariantCulture);
                }

                int id = this.servScadaSp7.SaveMeScadaFiltroSp7Id(entity);

                //grabando los Canales SCADA
                string[] canalCodis = model.FiltroCanal.Split(new char[] {','});

                //eliminando sus códigos relacionados
                servScadaSp7.DeleteFiltroMeScadaPtofiltroSp7(id);

                foreach (string canalCodi1 in canalCodis)
                {

                    int canalCodi;

                    try
                    {
                        canalCodi = Convert.ToInt32(canalCodi1);
                    }
                    catch
                    {
                        canalCodi = 0;
                    }

                    if (canalCodi == 0)
                        continue;

                    MeScadaPtofiltroSp7DTO entityCanal = new MeScadaPtofiltroSp7DTO();
                    entityCanal.Scdpficodi = 0;
                    entityCanal.Filtrocodi = id;
                    entityCanal.Canalcodi = canalCodi;
                    entityCanal.Scdpfiusucreacion = User.Identity.Name;
                    entityCanal.Scdpfifeccreacion = DateTime.Now;
                    
                    servScadaSp7.SaveMeScadaPtofiltroSp7Id(entityCanal);
                }

                return Json(id);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite obtener un listado de filtros por criterio
        /// </summary>
        /// <param name="filtro">Filtro a buscar</param>
        /// <param name="creador">Creador de filtro</param>
        /// <param name="modificador">Persona que modificó el filtro</param>
        /// <param name="nroPage">Nro de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string filtro, string creador, string modificador, int nroPage)
        {
            BusquedaMeScadaFiltroSp7Model model = new BusquedaMeScadaFiltroSp7Model();

            model.ListaMeScadaFiltroSp7 =
                servScadaSp7.BuscarOperaciones(filtro, creador, modificador, nroPage, Constantes.PageSizeEvento)
                    .ToList();

            return PartialView(model);
        }


        /// <summary>
        /// Permite realizar el paginado del listado de filtro
        /// </summary>
        /// <param name="filtro">Filtro a buscar</param>
        /// <param name="creador">Creador de filtro</param>
        /// <param name="modificador">Persona que modificó el filtro</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string filtro, string creador, string modificador)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = servScadaSp7.ObtenerNroFilas(filtro, creador, modificador);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }



    }
}
