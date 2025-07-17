using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Controllers
{
    public class ParametroController : BaseController
    {

        ReservaFriaNodoEnergeticoAppServicio servReservaNodo = new ReservaFriaNodoEnergeticoAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            BusquedaSiParametroModel model = new BusquedaSiParametroModel();

            return View(model);
        }

        /// <summary>
        /// Pemite editar el parámetro
        /// </summary>
        /// <param name="id">Código de parámetro</param>
        /// <param name="accion">Acción</param>
        /// <returns></returns>
        public ActionResult Editar(int id, int accion)
        {

            SiParametroModel model = new SiParametroModel();
            SiParametroDTO siParametro =null;

            if (id != 0)
                siParametro = servReservaNodo.GetByIdSiParametro(id);

            if (siParametro != null)
            {
                model.SiParametro = siParametro;
            }
            else
            {
                siParametro = new SiParametroDTO();
                model.SiParametro = siParametro;

            }

            model.Accion = accion;
            return View(model);            
        }


        /// <summary>
        /// Permite eliminar el parámetro a nivel lógico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servReservaNodo.DeleteSiParametro(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite grabar el parámetro
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(SiParametroModel model)
        {
            try
            {

                SiParametroDTO entity = new SiParametroDTO();

                entity.Siparcodi = model.SiparCodi;
                entity.Siparabrev = model.SiparAbrev;
                entity.Sipardescripcion = model.SiparDescripcion;

                if (entity.Siparcodi == 0)
                {
                    entity.Siparusucreacion = base.UserName;
                    entity.Siparfeccreacion = DateTime.Now;
                }
                else
                {

                    if (model.SiparUsuCreacion != null)
                    {
                        entity.Siparusucreacion = model.SiparUsuCreacion;
                    }

                    if (model.SiparFecCreacion != null)
                    {
                        entity.Siparfeccreacion = DateTime.ParseExact(model.SiparFecCreacion, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }

                    entity.Siparusumodificacion = base.UserName;
                    entity.Siparfecmodificacion = DateTime.Now;
                }

                int id = this.servReservaNodo.SaveSiParametroId(entity);
                return Json(id);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite listar los parámetros
        /// </summary>
        /// <param name="abreviatura">Abreviatura</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="nroPage">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string abreviatura,string descripcion, int nroPage)
        {
            BusquedaSiParametroModel model = new BusquedaSiParametroModel();
            model.ListaSiParametro = servReservaNodo.BuscarOperaciones(abreviatura, descripcion, nroPage, Constantes.PageSizeEvento).ToList();
            return PartialView(model);
        }


        /// <summary>
        /// Permite realizar el paginado
        /// </summary>
        /// <param name="abreviatura">Abreviatura</param>
        /// <param name="descripcion">Descripción</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string abreviatura, string descripcion)
        {
            Paginacion model = new Paginacion();
            int nroRegistros = servReservaNodo.ObtenerNroFilas( abreviatura, descripcion);

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
