using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Eventos;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class BusquedaController : Controller
    {
        /// <summary>
        /// Instancia de clase de acceso a datos
        /// </summary>
        EventoAppServicio servicio = new EventoAppServicio();

        /// <summary>
        /// Muestra la pagina inical
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Index(string famcodi)
        {
            BusquedaEquipoModel model = new BusquedaEquipoModel();
            model.ListaEmpresa = this.servicio.ListarEmpresas().Where(x => x.EMPRCODI != 0 && x.EMPRCODI != -1).ToList();

            if (string.IsNullOrEmpty(famcodi))
            {
                model.ListaFamilia = this.servicio.ListarFamilias().ToList();
            }
            else
            {
                string famcodicoma = Constantes.CaracterComa + famcodi + Constantes.CaracterComa;
                model.ListaFamilia = this.servicio.ListarFamilias().Where(x => famcodicoma.IndexOf(Constantes.CaracterComa.ToString()
                    + x.FAMCODI + Constantes.CaracterComa.ToString()) >= 0).ToList();
            }

            return PartialView(model);
        }

        /// <summary>
        /// Muestra las areas de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Area(int idEmpresa, string idFamilia)
        {
            BusquedaEquipoModel model = new BusquedaEquipoModel();
            model.ListaArea = servicio.ObtenerAreaPorEmpresa(idEmpresa, idFamilia).ToList();
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
        public PartialViewResult Resultado(int idEmpresa, string idFamilia, int idArea, string filtro, int nroPagina)
        {
            BusquedaEquipoModel model = new BusquedaEquipoModel();
            model.ListaEquipo = servicio.BuscarEquipoEvento(idEmpresa, idArea, idFamilia, filtro, nroPagina, Constantes.PageSize).ToList();
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
        public PartialViewResult Paginado(int idEmpresa, string idFamilia, int idArea, string filtro)
        {
            BusquedaEquipoModel model = new BusquedaEquipoModel();
            model.IndicadorPagina = false;
            int nroRegistros = servicio.ObtenerNroFilasBusquedaEquipo(idEmpresa, idArea, idFamilia, filtro);

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

    }
}
