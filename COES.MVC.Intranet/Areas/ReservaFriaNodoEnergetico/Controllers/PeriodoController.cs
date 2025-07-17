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
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Helper;


namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Controllers
{
    public class PeriodoController : BaseController
    {

        /// <summary>
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicioNrPer
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaInicioNrPer] != null) ?
                    (DateTime?)(Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaInicioNrPer]) : null;
            }
            set
            {
                Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaInicioNrPer] = value;
            }
        }

        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFinalNrPer
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaFinNrPer] != null) ?
                  (DateTime?)(Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaFinNrPer]) : null;
            }
            set
            {
                Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaFin] = value;
            }
        }

        ReservaFriaNodoEnergeticoAppServicio servReservaNodo = new ReservaFriaNodoEnergeticoAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            BusquedaNrPeriodoModel model = new BusquedaNrPeriodoModel();
            //model.FechaIni = DateTime.Now.AddDays(-6 * 30).ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes);
            //model.FechaFin = DateTime.Now.ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes);

            model.FechaIni = (this.FechaInicioNrPer != null) ? ((DateTime)this.FechaInicioNrPer).ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes) :
               DateTime.Now.AddDays(-6 * 30).ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes);
            model.FechaFin = (this.FechaFinalNrPer != null) ? ((DateTime)this.FechaFinalNrPer).ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes) :
               DateTime.Now.ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes);

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);
        }


        /// <summary>
        /// Permite editar el periodo
        /// </summary>
        /// <param name="id">Código de periodo</param>
        /// <param name="accion">Acción</param>
        /// <returns></returns>
        public ActionResult Editar(int id, int accion)
        {

            NrPeriodoModel model = new NrPeriodoModel();
            NrPeriodoDTO nrPeriodo =null;

            if (id != 0)
                nrPeriodo = servReservaNodo.GetByIdNrPeriodo(id);

            if (nrPeriodo != null)
            {
                model.NrPeriodo = nrPeriodo;
            }
            else
            {
                nrPeriodo = new NrPeriodoDTO();
                
                //mes anterior
                DateTime periodoAnterior = DateTime.Now; 

                if(periodoAnterior.Month==1)
                    periodoAnterior = Convert.ToDateTime((periodoAnterior.Year-1)+"-12-01");
                else
                    periodoAnterior = Convert.ToDateTime((periodoAnterior.Year) + "-" + (periodoAnterior.Month-1) + "-01");
                
                nrPeriodo.Nrpermes = periodoAnterior;
                model.NrPeriodo = nrPeriodo;

            }

            model.Accion = accion;
            return View(model);            
        }


        /// <summary>
        /// Permite deshabilitar un periodo configurado
        /// </summary>
        /// <param name="id">Código de periodo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Desactivar(int id)
        {
            try
            {
                NrPeriodoDTO entity = null;

                if (id != 0)
                {
                    entity = servReservaNodo.GetByIdNrPeriodo(id);

                    entity.Nrperusumodificacion = base.UserName;
                    entity.Nrperfecmodificacion = DateTime.Now;
                    entity.Nrpereliminado = "S";

                    servReservaNodo.UpdateNrPeriodo(entity);
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
        /// Permite grabar el periodo
        /// </summary>
        /// <param name="model">modelo de periodo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(NrPeriodoModel model)
        {
            try
            {
                NrPeriodoDTO entity = new NrPeriodoDTO();
                entity.Nrpercodi = model.NrperCodi;
                entity.Nrpermes = DateTime.ParseExact(model.NrperMes + "-01", ConstanteReservaFriaNodoEnergetico.FormatoAnioMesInicio, CultureInfo.InvariantCulture);
                entity.Nrpereliminado = model.NrperEliminado;
                
                if (entity.Nrpercodi == 0)
                {
                    entity.Nrperusucreacion = base.UserName;
                    entity.Nrperfeccreacion = DateTime.Now;
                }
                else
                {

                    if (model.NrperUsuCreacion != null)
                    {

                        entity.Nrperusucreacion = model.NrperUsuCreacion;
                    }

                    if (model.NrperFecCreacion != null)
                    {
                        entity.Nrperfeccreacion = DateTime.ParseExact(model.NrperFecCreacion, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }

                    entity.Nrperusumodificacion = base.UserName;
                    entity.Nrperfecmodificacion = DateTime.Now;
                }
                

                if (entity.Nrpercodi == 0)
                {

                    if (!this.servReservaNodo.ExistePeriodoId(entity.Nrpermes))
                    {
                        int id = this.servReservaNodo.SaveNrPeriodoId(entity);

                        return Json(id);
                    }
                    else
                    {
                        return Json(-2);
                    }
                }

                else
                {
                    int id = this.servReservaNodo.SaveNrPeriodoId(entity);
                    return Json(id);
                }
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite listar los periodos
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string estado, string fechaIni,string fechaFin, int nroPage)
        {
            BusquedaNrPeriodoModel model = new BusquedaNrPeriodoModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, ConstanteReservaFriaNodoEnergetico.FormatoFechaYMD, CultureInfo.InvariantCulture);
            }

            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, ConstanteReservaFriaNodoEnergetico.FormatoFechaYMD, CultureInfo.InvariantCulture);
            }

            this.FechaInicioNrPer = fechaInicio;
            this.FechaFinalNrPer = fechaFinal;

            model.ListaNrPeriodo = servReservaNodo.BuscarOperaciones(estado,fechaInicio, fechaFinal, nroPage, Constantes.PageSizeEvento).ToList();
            
            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return PartialView(model);
        }


        /// <summary>
        /// Permite realizar el paginado
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string estado, string fechaIni, string fechaFin)
        {
            BusquedaNrPeriodoModel model = new BusquedaNrPeriodoModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, ConstanteReservaFriaNodoEnergetico.FormatoFechaYMD, CultureInfo.InvariantCulture);
            }


            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, ConstanteReservaFriaNodoEnergetico.FormatoFechaYMD, CultureInfo.InvariantCulture);
            }


            int nroRegistros = servReservaNodo.ObtenerNroFilas(estado, fechaInicio, fechaFinal);

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
    }
}
