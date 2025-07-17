using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class DirectorioController : BaseController
    {
        DirectorioServicio.ServicioSoapClient cliente = new DirectorioServicio.ServicioSoapClient();

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
                model.ListaAreas = cliente.ListarAreas().ToList();
                model.IndicadorGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
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
            List<DirectorioServicio.Directorio> lista = cliente.ObtenerDirectorioPorArea(idArea).ToList();
            model.ListaDirectorio = lista;

            return PartialView(model);
        }

        /// <summary>
        /// Permite editar la vista edicion
        /// </summary>
        /// <param name="idDirectorio"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Edicion(int idDirectorio, string nombre, string apellido, string anexo, string indAnexo, string indExtranet)
        {
            DirectorioModel model = new DirectorioModel();
            model.IdDirectorio = idDirectorio;
            model.Nombre = nombre + " " + apellido;
            model.Anexo = anexo;
            model.IndAnexo = (!string.IsNullOrEmpty(indAnexo)) ? indAnexo : Constantes.NO;
            model.IndExtranet = (indExtranet == Constantes.NO) ? Constantes.NO : Constantes.SI;
            return PartialView(model);
        }

        /// <summary>
        /// Permite actualizar el anexo alternativo
        /// </summary>
        /// <param name="idDirectorio"></param>
        /// <param name="idAnexoo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarAnexo(int idDirectorio, string anexo, string indAnexo, string indExtranet)
        {
            try
            {
                cliente.ActualizarAnexoDirectorio(idDirectorio, anexo, indAnexo, indExtranet);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

    }
}
