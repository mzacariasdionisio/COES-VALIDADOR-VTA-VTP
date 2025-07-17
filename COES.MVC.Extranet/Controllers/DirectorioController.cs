using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Controllers
{
    public class DirectorioController : BaseController
    {
        DirectorioServicio.ServicioClient cliente = new DirectorioServicio.ServicioClient();

       /// <summary>
       /// Muestra la pantalla inicial del módulo
       /// </summary>
       /// <returns></returns>
        public ActionResult Index()
        {
            DirectorioModel model = new DirectorioModel();
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                model.Indicador = 1;
                model.ListaAreas = cliente.ObtenerAreasExtranet().ToList();                
            }
            else
            {
                model.Indicador = 0;
            }

            return PartialView(model);            
        }

        /// <summary>
        /// Muestra el listado del directorio
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(int idArea) 
        {
            DirectorioModel model = new DirectorioModel();
            List<DirectorioServicio.IntDirectorioDTO> lista = cliente.ObtenerDirectorioExtranet(idArea).ToList();
            //model.ListaDirectorio = lista.Where(x => x.IndExtranet != Constantes.NO).ToList();
            model.ListaDirectorio = lista;
            return PartialView(model);
        }
    }
}
