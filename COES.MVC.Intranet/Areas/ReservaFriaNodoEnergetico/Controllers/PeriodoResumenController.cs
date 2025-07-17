using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Medidores;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Helper;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Controllers
{
    public class PeriodoResumenController : BaseController
    {
        /// <summary>
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicio
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.PeriodoConsultaInicio] != null) ?
                    (DateTime?)(Session[ConstanteReservaFriaNodoEnergetico.PeriodoConsultaInicio]) : null;
            }
            set
            {
                Session[ConstanteReservaFriaNodoEnergetico.PeriodoConsultaInicio] = value;
            }
        }


        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFinal
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.PeriodoConsultaFin] != null) ?
                  (DateTime?)(Session[ConstanteReservaFriaNodoEnergetico.PeriodoConsultaFin]) : null;
            }
            set
            {
                Session[ConstanteReservaFriaNodoEnergetico.PeriodoConsultaFin] = value;
            }
        }


        /// <summary>
        /// Código de Submodulo
        /// </summary>
        public int? SubModulo
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FiltroSubModulo] != null)
                    ? (int?)(Session[ConstanteReservaFriaNodoEnergetico.FiltroSubModulo])
                    : 0;
            }

            set { Session[ConstanteReservaFriaNodoEnergetico.FiltroSubModulo] = value; }
        }


        ReservaFriaNodoEnergeticoAppServicio servReservaNodo = new ReservaFriaNodoEnergeticoAppServicio();
        

        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            
            BusquedaNrPeriodoResumenModel model = new BusquedaNrPeriodoResumenModel();
            model.ListaNrPeriodo = servReservaNodo.ListNrPeriodos();
            model.ListaNrSubmodulo = servReservaNodo.ListNrSubmodulos();

            model.FechaIni = (this.FechaInicio != null)
                ? ((DateTime)this.FechaInicio).ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes)
                : DateTime.Now.AddDays(-6 * 30).ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes);

            model.FechaFin = (this.FechaFinal != null)
                ? ((DateTime)this.FechaFinal).ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes)
                : DateTime.Now.ToString(ConstanteReservaFriaNodoEnergetico.FormatoAnioMes);

            model.SubModulo = this.SubModulo;

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);
        }


        /// <summary>
        /// Permite editar el periodo resumen
        /// </summary>
        /// <param name="idNrsmodCodi">Código de submódulo</param>
        /// <param name="idNrperCodi">Código de periodo</param>
        /// <returns></returns>
        public ActionResult Editar(int idNrsmodCodi, int idNrperCodi)
        {

            NrPeriodoResumenModel model = new NrPeriodoResumenModel();
            model.ListaNrPeriodo = servReservaNodo.ListNrPeriodos();
            model.ListaNrConcepto = servReservaNodo.ListNrConceptos();
            model.ListaNrSubmodulo = servReservaNodo.ListNrSubmodulos();           

            NrPeriodoResumenDTO nrPeriodoResumen =null;

            model.ListaNrPeriodoResumen = new List<NrPeriodoResumenDTO>();

            model.NrsmodCodi = idNrsmodCodi;
            model.NrperCodi = idNrperCodi;
            
            if (idNrsmodCodi != 0 && idNrperCodi != 0)
            {
                model.ListaNrPeriodoResumen = servReservaNodo.ListNrPeriodoResumens(idNrsmodCodi, idNrperCodi);
            }

            if (nrPeriodoResumen != null)
            {
                model.NrPeriodoResumen = nrPeriodoResumen;
                model.NrperCodi = (int)nrPeriodoResumen.Nrpercodi;

                model.NrsmodCodi = (int)servReservaNodo.GetByIdNrConcepto((int)nrPeriodoResumen.Nrcptcodi).Nrsmodcodi;
            }
            else
            {
                nrPeriodoResumen = new NrPeriodoResumenDTO();

                nrPeriodoResumen.Nrpercodi = Convert.ToInt32(Constantes.ParametroDefecto);
                nrPeriodoResumen.Nrcptcodi = Convert.ToInt32(Constantes.ParametroDefecto);
                nrPeriodoResumen.Nrperrfeccreacion = DateTime.Now;
                nrPeriodoResumen.Nrperrfecmodificacion = DateTime.Now;

                model.NrPeriodoResumen = nrPeriodoResumen;
            }
            
            return View(model);            
        }





        /// <summary>
        /// Permite procesar los conceptos por periodo
        /// </summary>
        /// <param name="nrcptcodi">Código de concepto</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarConceptoPeriodo(int nrcptcodi, int nrperCodi)
        {
            string retornoFinal = servReservaNodo.ProcesarConceptoPeriodo(nrcptcodi, nrperCodi, base.UserName);

            return Json(retornoFinal);
        }


        /// <summary>
        /// Permite actualizar el proceso resultante
        /// </summary>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="resultado">resultado de la forma: cod1:obs1,cod2:obs2,...,codn:obsn</param>
        [HttpPost]
        public JsonResult ActualizarProceso(int nrperCodi, string resultado)
        {
            try
            {
                //ejemplo: string cad = "1:0,2:0";
                //ejemplo: string cad = "1:0";

                string[] valores = resultado.Split(',');

                foreach (string valorParcial in valores)
                {
                    string[] arrIdObs = valorParcial.Split(':');
                    
                    int id = Convert.ToInt32(arrIdObs[0]);
                    int numObservacion = Convert.ToInt32(arrIdObs[1]);

                    NrPeriodoResumenDTO periodoResumen = servReservaNodo.GetByIdNrPeriodoResumen(nrperCodi, id);

                    periodoResumen.Nrperrnumobservacion = numObservacion;
                    periodoResumen.Nrperrusumodificacion = base.UserName;
                    periodoResumen.Nrperrfecmodificacion = DateTime.Now;

                    servReservaNodo.SaveNrPeriodoResumenId(periodoResumen);

                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite eliminar un periodo
        /// </summary>
        /// <param name="id">Código de periodo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servReservaNodo.DeleteNrPeriodoResumen(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite obtener las observaciones de un periodo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerObservaciones(int idNrperCodi)
        {
            try
            {
                string observaciones = servReservaNodo.ListNrProcesosObservaciones(idNrperCodi);
                return Json(observaciones);
            }
            catch
            {
                return Json(-1);
            }
        }

        
        /// <summary>
        /// Permite crear un nuevo periodo del módulo
        /// </summary>
        /// <param name="idNrsmodCodi">Código de módulo</param>
        /// <param name="idNrperCodi">Código de periodo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(int idNrsmodCodi, int idNrperCodi)
        {
            try
            {
                List<NrConceptoDTO> listaConcepto= servReservaNodo.ListNrConceptos().Where(x => x.Nrsmodcodi == idNrsmodCodi).ToList();
                
                List<NrPeriodoResumenDTO> listaPeriodoResumen = servReservaNodo.ListNrPeriodoResumens().Where(x => x.Nrpercodi == idNrperCodi).ToList();

                foreach(NrPeriodoResumenDTO itemPRes in listaPeriodoResumen)
                {
                    foreach (NrConceptoDTO itemConcepto in listaConcepto)
                    {

                        if (itemConcepto.Nrcptcodi == itemPRes.Nrcptcodi)
                        {
                            //periodo ya registrado
                            return Json(1);
                        }
                    }

                }
                
                
                //crea registros
                
                foreach (NrConceptoDTO item in listaConcepto)
                {

                    NrPeriodoResumenDTO entity = new NrPeriodoResumenDTO();

                    entity.Nrperrcodi = 0;
                    entity.Nrpercodi = idNrperCodi;
                    entity.Nrcptcodi = item.Nrcptcodi;
                    entity.Nrperrnumobservacion = -1;                    
                    entity.Nrperreliminado = "N";
                    entity.Nrperrusucreacion = base.UserName;
                    entity.Nrperrfeccreacion = DateTime.Now;
                    
                    int id = this.servReservaNodo.SaveNrPeriodoResumenId(entity);

                }

                //operación correcta
                return Json(0);                
            }
            catch
            {
                return Json(-1);
            }
        }
        

        /// <summary>
        /// Permite obtener un listado de información
        /// </summary>
        /// <param name="nrsmodCodi">Código de módulo</param>
        /// <param name="estado">Estado</param>
        /// <param name="nrperFecIni">Fecha inicial</param>
        /// <param name="nrperFecFin">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int nrsmodCodi, string estado,string nrperFecIni, string nrperFecFin, int nroPage)
        {
            BusquedaNrPeriodoResumenModel model = new BusquedaNrPeriodoResumenModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (nrperFecIni != null)
            {
                fechaInicio = DateTime.ParseExact(nrperFecIni, ConstanteReservaFriaNodoEnergetico.FormatoAnioMes, CultureInfo.InvariantCulture);
            }
            if (nrperFecFin != null)
            {
                fechaFinal = DateTime.ParseExact(nrperFecFin, ConstanteReservaFriaNodoEnergetico.FormatoAnioMes, CultureInfo.InvariantCulture);
            }
            fechaFinal = fechaFinal.AddDays(1);

            this.FechaInicio = fechaInicio;
            this.FechaFinal = fechaFinal;
            this.SubModulo = nrsmodCodi;

            model.ListaNrPeriodoResumen = servReservaNodo.BuscarOperaciones(nrsmodCodi,  estado, fechaInicio, fechaFinal, nroPage, Constantes.PageSizeEvento).ToList();

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return PartialView(model);
        }


        /// <summary>
        /// Permite realizar el paginado
        /// </summary>
        /// <param name="nrsmodCodi">Códig de módulo</param>
        /// <param name="estado">Estado</param>
        /// <param name="nrperFecIni">Fecha inicial</param>
        /// <param name="nrperFecFin">Fecha final</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(int nrsmodCodi, string estado, string nrperFecIni, string nrperFecFin)
        {
            Paginacion model = new Paginacion();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (nrperFecIni != null)
            {
                fechaInicio = DateTime.ParseExact(nrperFecIni, ConstanteReservaFriaNodoEnergetico.FormatoAnioMes, CultureInfo.InvariantCulture);
            }
            if (nrperFecFin != null)
            {
                fechaFinal = DateTime.ParseExact(nrperFecFin, ConstanteReservaFriaNodoEnergetico.FormatoAnioMes, CultureInfo.InvariantCulture);
            }
            fechaFinal = fechaFinal.AddDays(1);

            int nroRegistros = servReservaNodo.ObtenerNroFilas(nrsmodCodi, estado, fechaInicio, fechaFinal);

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


        /// <summary>
        /// Permite eliminar lógicamente un periodo-resumen configurado
        /// </summary>
        /// <param name="id">Código de periodo resumen</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Desactivar(int id)
        {
            try
            {
                NrPeriodoResumenDTO entity = null;

                if (id != 0)
                {
                    entity = servReservaNodo.GetByIdNrPeriodoResumen(id);

                    entity.Nrperrusumodificacion = base.UserName;
                    entity.Nrperrfecmodificacion = DateTime.Now;

                    entity.Nrperreliminado = "S";

                    servReservaNodo.UpdateNrPeriodoResumen(entity);
                    return Json(1);
                }
                return Json(-1);
            }
            catch
            {
                return Json(-1);
            }
        }

        
        
    }
}



