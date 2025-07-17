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
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico.Helper;
using COES.Framework.Base.Core;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Controllers
{
    public class SobrecostoController : BaseController
    {

        ReservaFriaNodoEnergeticoAppServicio servReservaNodo = new ReservaFriaNodoEnergeticoAppServicio();
        //DespachoAppServicio servDespacho = new DespachoAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            BusquedaNrSobrecostoModel model = new BusquedaNrSobrecostoModel();
            model.ListaPrGrupo = servReservaNodo.ListarModoOperacionSubModulo(ConstantesReservaFriaNodoEnergetico.ModuloNodoEnergetico); //2
            model.FechaIni = DateTime.Now.AddDays(-120).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            
            return View(model);
        }


        /// <summary>
        /// Permite realizar la edición de sobrecosto
        /// </summary>
        /// <param name="id">Código de sobrecosto</param>
        /// <param name="accion">Acción</param>
        /// <returns></returns>
        public ActionResult Editar(int id, int accion)
        {

            NrSobrecostoModel model = new NrSobrecostoModel();
            NrSobrecostoDTO nrSobrecosto =null;

            model.ListaPrGrupo = servReservaNodo.ListarModoOperacionSubModulo(ConstantesReservaFriaNodoEnergetico.ModuloNodoEnergetico);

            if (id != 0)
                nrSobrecosto = servReservaNodo.GetByIdNrSobrecosto(id);

            if (nrSobrecosto != null)
            {
                model.NrSobrecosto = nrSobrecosto;
            }
            else
            {
                nrSobrecosto = new NrSobrecostoDTO();
                nrSobrecosto.Nrscfecha = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                model.NrSobrecosto = nrSobrecosto;
            }

            model.Accion = accion;
            return View(model);            
        }


        /// <summary>
        /// Permite eliminar un sobrecosto
        /// </summary>
        /// <param name="id">Código de sobrecosto</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servReservaNodo.DeleteNrSobrecosto(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite deshabilitar un periodo configurado
        /// </summary>
        /// <param name="id">Código de sobrecosto</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Desactivar(int id)
        {
            try
            {
                NrSobrecostoDTO entity = null;

                if (id != 0)
                {
                    entity = servReservaNodo.GetByIdNrSobrecosto(id);

                    entity.Nrscusumodificacion = base.UserName;
                    entity.Nrscfecmodificacion = DateTime.Now;
                    entity.Nrsceliminado = "S";

                    servReservaNodo.UpdateNrSobrecosto(entity);
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
        /// Permite grabar sobrecosto
        /// </summary>
        /// <param name="model">Modelo del tipo sobrecosto</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(NrSobrecostoModel model)
        {
            try
            {
                NrSobrecostoDTO entity = new NrSobrecostoDTO();

                entity.Nrsccodi = model.NrscCodi;                
                entity.Nrscfecha = DateTime.ParseExact(model.NrscFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Nrsccodespacho0 = model.NrscCodespacho0;
                entity.Nrsccodespacho1 = model.NrscCodespacho1;

                //decimal diferencia = (decimal)(model.NrscCodespacho0) - (decimal)(model.NrscCodespacho1);
                decimal diferencia = (decimal)(model.NrscCodespacho1) - (decimal)(model.NrscCodespacho0);
                                
                entity.Nrscsobrecosto = (diferencia>=0?diferencia:0);
                
                entity.Nrscnota = model.NrscNota;                
                entity.Nrsceliminado = model.NrscEliminado;
                entity.Nrscpadre = model.NrscPadre;

                if (entity.Nrsccodi == 0)
                {
                    entity.Nrscusucreacion = base.UserName;
                    entity.Nrscfeccreacion = DateTime.Now;
                }
                else
                {
                    if (model.NrscUsucreacion != null)
                    {
                        entity.Nrscusucreacion = model.NrscUsucreacion;
                    }

                    if (model.NrscFecCreacion != null)
                    {
                        entity.Nrscfeccreacion = DateTime.ParseExact(model.NrscFecCreacion, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }

                    entity.Nrscusumodificacion = base.UserName;
                    entity.Nrscfecmodificacion = DateTime.Now;
                }

                int id = this.servReservaNodo.SaveNrSobrecostoId(entity);                
                return Json(id);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite obtener un listado de los sobrecostos
        /// </summary>
        /// <param name="nrscFechaIni">Fecha inicial</param>
        /// <param name="nrscFechaFin">Fecha final</param>
        /// <param name="estado">Estado</param>
        /// <param name="nroPage">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string nrscFechaIni, string nrscFechaFin, string estado, int nroPage)
        {
            BusquedaNrSobrecostoModel model = new BusquedaNrSobrecostoModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (nrscFechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(nrscFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (nrscFechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(nrscFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
                        
            model.ListaNrSobrecosto = servReservaNodo.BuscarOperaciones(fechaInicio, fechaFinal, estado, nroPage, Constantes.PageSizeEvento).ToList();

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            
            return PartialView(model);
        }


        /// <summary>
        /// Permite realizar el paginado de los sobrecostos
        /// </summary>
        /// <param name="nrscFechaIni">Fecha inicial</param>
        /// <param name="nrscFechaFin">Fecha final</param>
        /// <param name="estado">Estado</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string nrscFechaIni, string nrscFechaFin,string estado)
        {
            Paginacion model = new Paginacion();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (nrscFechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(nrscFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (nrscFechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(nrscFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            int nroRegistros = servReservaNodo.ObtenerNroFilas(fechaInicio, fechaFinal, estado);

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
