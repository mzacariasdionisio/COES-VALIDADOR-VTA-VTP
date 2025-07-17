using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico.Helper;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Helper;


namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Controllers
{
    public class ProcesoController : BaseController
    {
        /// <summary>
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicio
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaInicio] != null) ?
                    (DateTime?)(Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaInicio]) : null;
            }
            set
            {
                Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaInicio] = value;
            }
        }


        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFinal
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaFin] != null) ?
                  (DateTime?)(Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaFin]) : null;
            }
            set
            {
                Session[ConstanteReservaFriaNodoEnergetico.FechaConsultaFin] = value;
            }
        }


        /// <summary>
        /// Concepto
        /// </summary>
        public int? Concepto
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FiltroConcepto] != null)
                    ? (int?)(Session[ConstanteReservaFriaNodoEnergetico.FiltroConcepto])
                    : 0;
            }

            set { Session[ConstanteReservaFriaNodoEnergetico.FiltroConcepto] = value; }
        }


        /// <summary>
        /// Grupo
        /// </summary>
        public int? Grupo
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FiltroGrupo] != null)
                    ? (int?)(Session[ConstanteReservaFriaNodoEnergetico.FiltroGrupo])
                    : 0;

            }

            set { Session[ConstanteReservaFriaNodoEnergetico.FiltroGrupo] = value; }
        }


        /// <summary>
        /// Estado
        /// </summary>
        public string Estado
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FiltroEstado] != null)
                    ? (string)(Session[ConstanteReservaFriaNodoEnergetico.FiltroEstado])
                    : "N";
            }

            set { Session[ConstanteReservaFriaNodoEnergetico.FiltroEstado] = value; }
        }


        ReservaFriaNodoEnergeticoAppServicio servReservaNodo = new ReservaFriaNodoEnergeticoAppServicio();


        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            BusquedaNrProcesoModel model = new BusquedaNrProcesoModel();
            model.ListaNrPeriodo = servReservaNodo.ListNrPeriodos();
            //model.ListaPrGrupo = servDespacho.ListarModoOperacionSubModulo(ConstantesReservaFriaNodoEnergetico.ModuloReservaFria);
            model.ListaPrGrupo = servReservaNodo.ListarModoOperacionSubModulo(ConstantesReservaFriaNodoEnergetico.Todos);// ConstantesReservaFriaNodoEnergetico.ModuloReservaFria);

            model.ListaNrConcepto = servReservaNodo.ListNrSubModuloConcepto();

            model.FechaIni = (this.FechaInicio != null)
                ? ((DateTime)this.FechaInicio).ToString(Constantes.FormatoFecha)
                : DateTime.Now.AddDays(-120).ToString(Constantes.FormatoFecha);

            model.FechaFin = (this.FechaFinal != null)
                ? ((DateTime)this.FechaFinal).ToString(Constantes.FormatoFecha)
                : DateTime.Now.ToString(Constantes.FormatoFecha);

            model.Concepto = this.Concepto;
            model.Grupo = this.Grupo;
            model.Estado = this.Estado;

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);
        }


        /// <summary>
        /// Permite editar el proceso de reserva fría o nodo energético
        /// </summary>
        /// <param name="id">Código de proceso</param>
        /// <param name="accion">Acción</param>
        /// <returns></returns>
        public ActionResult Editar(int id, int accion)
        {
            //parametro a recibir: submodulo y luego filtrar conceptos.

            NrProcesoModel model = new NrProcesoModel();
            model.ListaNrPeriodo = servReservaNodo.ListNrPeriodos().Where(x => x.Nrpereliminado == Constantes.NO).ToList();
            model.ListaPrGrupo = servReservaNodo.ListarModoOperacionSubModulo(0);
            model.ListaNrConcepto = model.ListaNrConcepto = servReservaNodo.ListNrSubModuloConcepto();
            //servReservaNodo.ListNrConceptos();
            NrProcesoDTO nrProceso = null;

            if (id != 0)
                nrProceso = servReservaNodo.GetByIdNrProceso(id);

            if (nrProceso != null)
            {
                model.NrProceso = nrProceso;
            }
            else
            {
                nrProceso = new NrProcesoDTO();

                nrProceso.Nrpercodi = Convert.ToInt32(Constantes.ParametroDefecto);
                nrProceso.Grupocodi = Convert.ToInt32(Constantes.ParametroDefecto);
                nrProceso.Nrcptcodi = Convert.ToInt32(Constantes.ParametroDefecto);
                nrProceso.Nrprcfechainicio = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                nrProceso.Nrprcfechafin = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                nrProceso.Nrprchoraunidad = 0;
                nrProceso.Nrprchoracentral = 0;
                nrProceso.Nrprcpotencialimite = 0;
                nrProceso.Nrprcpotenciarestringida = 0;
                nrProceso.Nrprcpotenciaadjudicada = 0;
                nrProceso.Nrprcpotenciaefectiva = 0;
                nrProceso.Nrprcpotenciaprommedidor = 0;
                nrProceso.Nrprcprctjrestringefect = 0;
                nrProceso.Nrprcvolumencombustible = 0;
                nrProceso.Nrprcrendimientounidad = 1;
                nrProceso.Nrprcede = 0;
                nrProceso.Nrprcpadre = -1;
                nrProceso.Nrprcsobrecosto = 0;
                nrProceso.Nrprcrpf = 0;
                nrProceso.Nrprctolerancia = 0;

                model.NrProceso = nrProceso;
            }

            model.Accion = accion;
            return View(model);
        }


        /// <summary>
        /// Permitel eliminar un proceso de reserva fría o nodo energético
        /// </summary>
        /// <param name="id">Código de proceso</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                //servReservaNodo.DeleteNrProceso(id);
                NrProcesoDTO nrProceso = servReservaNodo.GetByIdNrProceso(id);
                nrProceso.Nrprcfiltrado = "S";
                nrProceso.Nrprcusumodificacion = base.UserName;
                nrProceso.Nrprcfecmodificacion = DateTime.Now;

                this.servReservaNodo.SaveNrProcesoId(nrProceso);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// PErmite grabar el proceso de reserva fría o nodo energético
        /// </summary>
        /// <param name="model">modelo del tipo proceso</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(NrProcesoModel model)
        {
            try
            {

                NrProcesoDTO entity = new NrProcesoDTO();

                entity.Nrprccodi = model.NrprcCodi;
                entity.Nrpercodi = model.NrperCodi;
                entity.Grupocodi = model.GrupoCodi;
                entity.Nrcptcodi = model.NrcptCodi;
                entity.Nrprcfechainicio = DateTime.ParseExact(model.NrprcFechaInicio, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                entity.Nrprcfechafin = DateTime.ParseExact(model.NrprcFechaFin, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                entity.Nrprchoraunidad = model.NrprcHoraUnidad;
                entity.Nrprchoracentral = model.NrprcHoraCentral;
                entity.Nrprcpotencialimite = model.NrprcPotenciaLimite;
                entity.Nrprcpotenciarestringida = model.NrprcPotenciaRestringida;
                entity.Nrprcpotenciaadjudicada = model.NrprcPotenciaAdjudicada;
                entity.Nrprcpotenciaefectiva = model.NrprcPotenciaEfectiva;
                entity.Nrprcpotenciaprommedidor = model.NrprcPotenciaPromMedidor;
                entity.Nrprcprctjrestringefect = model.NrprcPrctjRestringEfect;
                entity.Nrprcvolumencombustible = model.NrprcVolumenCombustible;
                entity.Nrprcrendimientounidad = model.NrprcRendimientoUnidad;
                entity.Nrprcede = model.NrprcEde;
                entity.Nrprcpadre = model.NrprcPadre;
                entity.Nrprcexceptuacoes = model.NrprcExceptuaCoes;
                entity.Nrprcexceptuaosinergmin = model.NrprcExceptuaOsinergmin;
                entity.Nrprctipoingreso = ConstantesReservaFriaNodoEnergetico.ProcesoManual;//model.NrprcTipoIngreso;
                entity.Nrprchorafalla = model.NrprcHoraFalla;
                entity.Nrprcsobrecosto = model.NrprcSobrecosto;
                entity.Nrprcobservacion = model.NrprcObservacion;
                entity.Nrprcnota = model.NrprcNota;
                entity.Nrprcnotaautomatica = model.NrprcNotaAutomatica;
                entity.Nrprcfiltrado = model.NrprcFiltrado;
                entity.Nrprcrpf = model.NrprcRpf;
                entity.Nrprctolerancia = model.NrprcTolerancia;

                if (entity.Nrprccodi == 0)
                {
                    entity.Nrprcusucreacion = base.UserName;
                    entity.Nrprcfeccreacion = DateTime.Now;
                }

                else
                {
                    if (model.NrprcUsuCreacion != null)
                    {
                        entity.Nrprcusucreacion = model.NrprcUsuCreacion;
                    }

                    if (model.NrprcFecCreacion != null)
                    {
                        entity.Nrprcfeccreacion = DateTime.ParseExact(model.NrprcFecCreacion, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }

                    entity.Nrprcusumodificacion = base.UserName;
                    entity.Nrprcfecmodificacion = DateTime.Now;
                }

                int id = this.servReservaNodo.SaveNrProcesoId(entity);
                return Json(id);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite listar el proceso de reserva fría o nodo energético
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="grupoCodi">Código de grupo</param>
        /// <param name="nrcptCodi">Código de concepto</param>
        /// <param name="nrprcFechaInicio">Fecha inicial</param>
        /// <param name="nrprcFechaFin">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string estado, int nrperCodi, int grupoCodi, int nrcptCodi, string nrprcFechaInicio, string nrprcFechaFin, int nroPage)
        {
            BusquedaNrProcesoModel model = new BusquedaNrProcesoModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (nrprcFechaInicio != null)
            {
                fechaInicio = DateTime.ParseExact(nrprcFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (nrprcFechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(nrprcFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fechaFinal = fechaFinal.AddDays(1);
            this.FechaInicio = fechaInicio;
            this.FechaFinal = fechaFinal;
            this.Concepto = nrcptCodi;
            this.Grupo = grupoCodi;
            this.Estado = estado;

            model.ListaNrProceso = servReservaNodo.BuscarOperaciones(estado, nrperCodi, grupoCodi, nrcptCodi, fechaInicio, fechaFinal,
                nroPage, Constantes.PageSizeEvento).ToList();

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return PartialView(model);
        }


        /// <summary>
        /// Permite realizar el paginado del proceso de reserva fría o nodo energético
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="grupoCodi">Código de grupo</param>
        /// <param name="nrcptCodi">Código de concepto</param>
        /// <param name="nrprcFechaInicio">Fecha inicial</param>
        /// <param name="nrprcFechaFin">Fecha final</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string estado, int nrperCodi, int grupoCodi, int nrcptCodi, string nrprcFechaInicio, string nrprcFechaFin)
        {
            Paginacion model = new Paginacion();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (nrprcFechaInicio != null)
            {
                fechaInicio = DateTime.ParseExact(nrprcFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (nrprcFechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(nrprcFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fechaFinal = fechaFinal.AddDays(1);

            int nroRegistros = servReservaNodo.ObtenerNroFilas(estado, nrperCodi, grupoCodi, nrcptCodi, fechaInicio, fechaFinal);

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
