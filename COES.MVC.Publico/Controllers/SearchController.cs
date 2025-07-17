using COES.MVC.Publico.Helper;
using COES.MVC.Publico.Models;
using COES.Storage.App.Base.Core;
using COES.Storage.App.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Controllers
{
    public class SearchController : Controller
    {
        FileManager fileManager = new FileManager();
        public int IdFuente = 1;

        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public ActionResult Index(string k)
        {
            SearchModel model = new SearchModel();
            model.Texto = k;
            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string texto, string extension)
        {
            SearchModel model = new SearchModel();
            model.IndicadorPagina = false;
            int nroRegistros = this.fileManager.ObtenerNroRegistroBusquedaPortal(texto, extension, this.IdFuente);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeBusqueda;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }


        /// <summary>
        /// Permite mostrar la lista de eventos
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Resultado(string texto, string extension, int nroPagina)
        {
            SearchModel model = new SearchModel();
            List<FileData> list = this.fileManager.BusquedaPortal(texto, extension, nroPagina, Constantes.PageSizeBusqueda, this.IdFuente);
            model.ListaResultado = list;
            return PartialView(model);
        }
    }
}
