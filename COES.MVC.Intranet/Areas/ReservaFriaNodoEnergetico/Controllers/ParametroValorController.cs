using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Controllers
{

    public class ParametroValorController : BaseController
    {

        ReservaFriaNodoEnergeticoAppServicio servReservaNodo = new ReservaFriaNodoEnergeticoAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            BusquedaSiParametroValorModel model = new BusquedaSiParametroValorModel();
            model.ListaSiParametro = servReservaNodo.ListSiParametros();
            model.FechaIni = DateTime.Now.AddDays(-365).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);
        }


        /// <summary>
        /// Permite editar el valor de un parámetro
        /// </summary>
        /// <param name="id">Código de parámetro</param>
        /// <param name="accion">Acción</param>
        /// <returns></returns>
        public ActionResult Editar(int id, int accion)
        {

            SiParametroValorModel model = new SiParametroValorModel();
            SiParametroValorDTO siParametroValor = null;

            model.ListaSiParametro = servReservaNodo.ListSiParametros();

            if (id != 0)
                siParametroValor = servReservaNodo.GetByIdSiParametroValor(id);

            if (siParametroValor != null)
            {
                model.SiParametroValor = siParametroValor;
            }
            else
            {
                siParametroValor = new SiParametroValorDTO();
                siParametroValor.Siparcodi = Convert.ToInt32(Constantes.ParametroDefecto);
                siParametroValor.Siparvfechainicial = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                siParametroValor.Siparvfechafinal = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                siParametroValor.Siparveliminado = Constantes.NO;
                model.SiParametroValor = siParametroValor;

            }

            model.Accion = accion;
            return View(model);

        }


        /// <summary>
        /// Permite eliminar el valor de un parámetro
        /// </summary>
        /// <param name="id">Identificador de valor de parámetro</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servReservaNodo.DeleteSiParametroValor(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite deshabilitar un valor de un parámetro configurado
        /// </summary>
        /// <param name="id">Identificador de valor de parámetro</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Desactivar(int id)
        {
            try
            {

                SiParametroValorDTO entity = null;

                if (id != 0)
                {
                    entity = servReservaNodo.GetByIdSiParametroValor(id);
                    entity.Siparvusumodificacion = base.UserName;
                    entity.Siparvfecmodificacion = DateTime.Now;
                    entity.Siparveliminado = "S";

                    servReservaNodo.UpdateSiParametroValor(entity);
                    return Json(1);
                }
                return Json(-1);
            }
            catch
            {
                return Json(-1);
            }

        }


        /// <summary>
        /// Permite grabar el valor de un parámetro
        /// </summary>
        /// <param name="model">modelo del tipo SiParametroValorModel</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(SiParametroValorModel model)
        {
            try
            {

                SiParametroValorDTO entity = new SiParametroValorDTO();

                entity.Siparvcodi = model.SiparvCodi;
                entity.Siparcodi = model.SiparCodi;
                entity.Siparvfechainicial = DateTime.ParseExact(model.SiparvFechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Siparvfechafinal = DateTime.ParseExact(model.SiparvFechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Siparvvalor = model.SiparvValor;
                entity.Siparvnota = model.SiparvNnota;
                entity.Siparveliminado = model.SiparvEliminado;

                if (entity.Siparvcodi == 0)
                {
                    entity.Siparvusucreacion = base.UserName;
                    entity.Siparvfeccreacion = DateTime.Now;
                }

                else
                {

                    if (model.SiparvUsuCreacion != null)
                    {
                        entity.Siparvusucreacion = model.SiparvUsuCreacion;
                    }

                    if (model.SiparvFecCreacion != null)
                    {
                        entity.Siparvfeccreacion = DateTime.ParseExact(model.SiparvFecCreacion, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }

                    entity.Siparvusumodificacion = base.UserName;
                    entity.Siparvfecmodificacion = DateTime.Now;
                }

                int id = this.servReservaNodo.SaveSiParametroValorId(entity);
                return Json(id);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite obtener un listado de los valores de los parámetros
        /// </summary>
        /// <param name="siparCodi">Código de parámetro</param>
        /// <param name="siparvFechaInicial">Fecha inicial</param>
        /// <param name="siparvFechaFinal">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <param name="estado">Estado</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int siparCodi, string siparvFechaInicial, string siparvFechaFinal, int nroPage, string estado)
        {
            BusquedaSiParametroValorModel model = new BusquedaSiParametroValorModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (siparvFechaInicial != null)
            {
                fechaInicio = DateTime.ParseExact(siparvFechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (siparvFechaFinal != null)
            {
                fechaFinal = DateTime.ParseExact(siparvFechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fechaFinal = fechaFinal.AddDays(1);
            model.ListaSiParametroValor = servReservaNodo.BuscarOperaciones(siparCodi, fechaInicio, fechaFinal, nroPage, Constantes.PageSizeEvento, estado).ToList();
            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }


        /// <summary>
        /// Permite realizar el paginado de los valores de los parámetros
        /// </summary>
        /// <param name="siparCodi">Código de parámetro</param>
        /// <param name="siparvFechaInicial">Fecha inicial</param>
        /// <param name="siparvFechaFinal">Fecha final</param>
        /// <param name="estado">Estado</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(int siparCodi, string siparvFechaInicial, string siparvFechaFinal, string estado)
        {
            Paginacion model = new Paginacion();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (siparvFechaInicial != null)
            {
                fechaInicio = DateTime.ParseExact(siparvFechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (siparvFechaFinal != null)
            {
                fechaFinal = DateTime.ParseExact(siparvFechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fechaFinal = fechaFinal.AddDays(1);

            int nroRegistros = servReservaNodo.ObtenerNroFilas(siparCodi, fechaInicio, fechaFinal, estado);

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
