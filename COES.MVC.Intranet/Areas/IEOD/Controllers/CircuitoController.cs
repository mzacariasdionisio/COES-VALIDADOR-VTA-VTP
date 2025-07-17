using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class CircuitoController : BaseController
    {
        McpAppServicio servMCP = new McpAppServicio();
        IEODAppServicio servIeod = new IEODAppServicio();
        EventoAppServicio servEvento = new EventoAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CircuitoModel model = new CircuitoModel();
            model.ListaEmpresas = this.servIeod.ListarEmpresasxTipoEquipos(ConstantesTopologiaElect.TipodeEquipos);

            return View(model);
        }

        /// <summary>
        /// Lista de circuitos filtrados por empresa
        /// </summary>
        /// <param name="listaEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string listaEmpresa)
        {
            bool accionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            CircuitoModel model = new CircuitoModel();
            model.ListaCircuito = this.servMCP.GetByCriteriaEqCircuitos(listaEmpresa, ConstantesAppServicio.ParametroDefecto, ConstantesMcp.CircuitoEstadoTodos);
            model.OpcionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            return PartialView(model);
        }

        #region Formulario

        /// <summary>
        /// Registrar/editar ficha tecnica
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CircuitoGuardar(string strJsonData)
        {
            CircuitoModel model = new CircuitoModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                model = serialize.Deserialize<CircuitoModel>(strJsonData);

                int codigo = model.Circodi;
                int codigoEq = model.Equicodi;
                string nombre = model.Circnomb;
                
                //Validaciones
                if (nombre == null || nombre.Trim() == string.Empty)
                {
                    throw new Exception("Debe ingresar nombre de circuito.");
                }
                if (codigoEq <= 0)
                {
                    throw new Exception("Debe seleccionar equipo.");
                }

                if (!model.ListaDetalleCircuito.Any())
                {
                    throw new Exception("No existe dependencias para el equipo seleccionado.");
                }
                else
                {
                    foreach (var regDet in model.ListaDetalleCircuito)
                    {
                        if (regDet.Equicodihijo == null && regDet.Circodihijo == null)
                        {
                            throw new Exception("No ha registrado dependencia.");
                        }
                    }

                    List<int> listaAgrup = model.ListaDetalleCircuito.Select(x => x.Circdtagrup).Distinct().OrderBy(x => x).ToList();
                    for (int pos = 1; pos <= listaAgrup.Max() && pos <= 15; pos++)
                    {
                        bool existe = model.ListaDetalleCircuito.Find(x => x.Circdtagrup == pos) != null;
                        if (!existe)
                        {
                            throw new Exception("El nivel " + pos + " no tiene elementos");
                        }
                    }
                }

                //Guardar
                EqCircuitoDTO reg = new EqCircuitoDTO();
                reg.Circodi = codigo;
                reg.Circnomb = nombre.Trim();
                reg.Equicodi = codigoEq;

                
                this.servMCP.RegistrarCircuito(reg, model.ListaDetalleCircuito, User.Identity.Name, out bool HayBucle);

                if (HayBucle)
                {
                    throw new Exception(" Existe Bucle en el circuíto.");
                }
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Editar circuito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CircuitoEditar(int id)
        {
            CircuitoModel model = new CircuitoModel();
            try
            {
                base.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Editar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Circuito = this.servMCP.GetByIdEqCircuito(id);
                if (model.Circuito == null)
                    throw new Exception("No existe el circuito seleccionado.");
                else
                    model.ListaDetalleCircuito = this.servMCP.GetByCriteriaEqCircuitoDets(id, ConstantesAppServicio.ParametroDefecto, ConstantesMcp.CircuitoEstadoTodos);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Dar de baja circuito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CircuitoDarBaja(int id)
        {
            CircuitoModel model = new CircuitoModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var circuito = this.servMCP.GetByIdEqCircuito(id);
                if (circuito == null)
                    throw new Exception("No existe el circuito seleccionado.");

                circuito.Circestado = ConstantesMcp.CircuitoEstadoInactivo;
                circuito.Circfecmodificacion = DateTime.Now;
                circuito.Circusumodificacion = User.Identity.Name;
                this.servMCP.UpdateEqCircuito(circuito);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #region Búsqueda de Equipos

        /// <summary>
        /// View Busqueda equipos
        /// </summary>
        /// <returns></returns>
        public PartialViewResult BusquedaEquipo(int? filtroFamilia = 0)
        {
            CircuitoModel model = new CircuitoModel();
            model.ListaEmpresas = this.servIeod.ListarEmpresasxTipoEquipos(ConstantesTopologiaElect.TipodeEquipos);
            model.ListaFamilia = this.servIeod.ListarFamilia();
            model.FiltroFamilia = filtroFamilia.Value;

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
        public PartialViewResult BusquedaEquipoResultado(int idEmpresa, int idFamilia, string filtro, int nroPagina, int? idArea = 0, int? filtroFamilia = 0, int? tipoFormulario = 0)
        {
            CircuitoModel model = new CircuitoModel();
            model.TipoFormulario = tipoFormulario.GetValueOrDefault(0);

            List<EqEquipoDTO> listaEquipo = new List<EqEquipoDTO>();
            var listaEq = this.servEvento.BuscarEquipoEvento(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro, nroPagina, Constantes.NroPageShow);

            foreach (var reg in listaEq)
            {
                EqEquipoDTO eq = new EqEquipoDTO();
                eq.Emprnomb = reg.EMPRENOMB;
                eq.Areanomb = reg.AREANOMB;
                eq.Equicodi = reg.EQUICODI;
                eq.Equinomb = reg.EQUIABREV != null ? reg.EQUIABREV.Trim() : string.Empty;
                eq.Equiabrev = reg.EQUIABREV;
                eq.Famabrev = reg.FAMABREV;
                eq.Emprcodi = reg.EMPRCODI;
                eq.Areacodi = reg.AREACODI;

                listaEquipo.Add(eq);
            }

            this.servMCP.AsignarCircuitoToEquipos(listaEquipo);

            model.ListaEquipo = listaEquipo;
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
        public PartialViewResult BusquedaEquipoPaginado(int idEmpresa, int idFamilia, string filtro, int? idArea = 0, int? filtroFamilia = 0)
        {
            CircuitoModel model = new CircuitoModel();
            model.IndicadorPagina = false;
            int nroRegistros = this.servEvento.ObtenerNroFilasBusquedaEquipo(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.NroPageShow;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Muestra las areas de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoArea(int idEmpresa, int idFamilia, int? filtroFamilia = 0)
        {
            CircuitoModel model = new CircuitoModel();
            model.ListaArea = this.servEvento.ObtenerAreaPorEmpresa(idEmpresa, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia)).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Funcion para obtener las familias permitidas. 
        /// </summary>
        /// <param name="idFamilia"></param>
        /// <param name="filtroFamilia"> -1: filtrar todas las familias, 0: filtrar solo para lineas de tranmision </param>
        /// <returns></returns>
        private string ListarFamiliaByFiltro(int idFamilia, int? filtroFamilia = 0)
        {
            if (filtroFamilia == 0)
            {
                return "0";
            }

            return idFamilia.ToString();
        }

        

        /// <summary>
        /// Eliminar grupodat
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEnCircuito(int circdtcodi)
        {
            CircuitoModel model = new CircuitoModel();
            try
            {
                DateTime fechaDat = DateTime.Now; 

                if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);

                EqCircuitoDetDTO reg = this.servMCP.GetByIdEqCircuitoDet(circdtcodi);

                if (reg == null)
                {
                    throw new Exception("El registro no existe o ha sido eliminada.");
                }

                //reg.Lastuser = User.Identity.Name;
                //reg.Fechaact = DateTime.Now;
                //reg.Deleted2 = ConstantesMigraciones.GrupodatInactivo;
                var fechaAhora = DateTime.Now;
                reg.Circdtusumodificacion = User.Identity.Name;
                reg.Circdtfecmodificacion= fechaAhora;
                reg.Circdtestado = ConstantesMcp.CircuitoEstadoInactivo;
                reg.UltimaModificacionFechaDesc = fechaAhora.ToString(ConstantesAppServicio.FormatoFechaFull2);
                model.CircuitoDet = reg;

                this.servMCP.UpdateEqCircuitoDet(reg);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        #endregion

        #endregion
    }
}