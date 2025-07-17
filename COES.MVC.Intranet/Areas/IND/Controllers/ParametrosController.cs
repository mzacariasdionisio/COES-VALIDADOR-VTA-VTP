using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IND.Controllers
{
    public class ParametrosController : BaseController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ParametrosController));
        private static readonly string NameController = "ConfiguracionController";

        private readonly INDAppServicio _indAppServicio;
        private readonly DespachoAppServicio _desapachoAppService;

        public ParametrosController()
        {
            _indAppServicio = new INDAppServicio();
            _desapachoAppService = new DespachoAppServicio();
        }

        #region Parametros

        /// <summary>
        /// Muestra la vista principal de Parametros
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public ActionResult IndexParametros(string anexo, int? pericodi)
        {
            var model = new IndisponibilidadesModel();
            model.UsarLayoutModulo = true;
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = _indAppServicio.GetPeriodoActual();
                model.ListaAnio = _indAppServicio.ListaAnio(fechaPeriodo).ToList();
                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);


                model.Anexo = anexo;


                if (pericodi.HasValue)
                {
                    model.IdPeriodo = pericodi.Value;
                    var regPeriodo = _indAppServicio.GetByIdIndPeriodo(pericodi.Value);
                    model.AnioActual = regPeriodo.FechaIni.Year;
                    model.ListaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);

        }

        /// <summary>
        /// Listado de parametros
        /// </summary>
        /// <param name="indpercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoParametros(int indpercodi, string anexo)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            var pfrPeriodo = _indAppServicio.GetByIdIndPeriodo(indpercodi);

            var concepcodi = string.Empty;
            if (anexo == ConstantesIndisponibilidades.AnexoB)
                concepcodi = ConstantesIndisponibilidades.ConcepcodiAnexoB;

            if (anexo == ConstantesIndisponibilidades.AnexoC)
                concepcodi = ConstantesIndisponibilidades.ConcepcodiAnexoC;

            model.ListaParametros = _indAppServicio
                .ListarParametrosConfiguracionPorFecha(pfrPeriodo.FechaFin, concepcodi);
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            PartialViewResult partialView = new PartialViewResult();

            if (anexo == ConstantesIndisponibilidades.AnexoB) partialView = PartialView("_ListadoParametros", model);
            if (anexo == ConstantesIndisponibilidades.AnexoC)
            {
                foreach (var item in model.ListaParametros)
                {
                    var descripcion = item.ConcepDesc.Split('-');
                    item.ConcepDesc = descripcion.First()?.Trim();
                    item.ConcepDesc2 = descripcion.Last()?.Trim();
                }
                partialView = PartialView("_ListadoParametrosAnexoC", model);
            }

            return partialView;
        }

        /// <summary>
        /// Listar historico de grupocodi y concepto
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="concepcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarHistoricoParametros(int grupocodi, int concepcodi, int opedicion)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.AccesoEditar = opedicion == Acciones.Editar ? base.VerificarAccesoAccion(Acciones.Editar, base.UserName) : false;
            model.ListaParametros = _indAppServicio.ListarGrupodatHistoricoValores(concepcodi, grupocodi);

            foreach (var reg in model.ListaParametros)
            {
                DateTime fechaDatDesc = DateTime.ParseExact(reg.FechadatDesc, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                reg.FechadatDesc = fechaDatDesc.Date.ToString(Constantes.FormatoFecha);
            }

            return PartialView("_ListarHistoricoParametros", model);
        }

        /// <summary>
        /// Registrar/editar parámetros configuración
        /// </summary>
        /// <param name="deleted"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupodatGuardar(int tipoAccion, int grupocodi, int concepcodi, string strfechaDat, string formuladat, int deleted)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaDat = DateTime.ParseExact(strfechaDat, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                //Validaciones

                if (concepcodi <= 0)
                {
                    throw new ArgumentException("Debe seleccionar un parámetro.");
                }

                if (deleted != 0)
                {
                    throw new ArgumentException("El registro ya ha sido eliminado, no puede modificarse.");
                }

                //Guardar
                if (tipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    PrGrupodatDTO reg = new PrGrupodatDTO();
                    reg.Grupocodi = grupocodi;
                    reg.Formuladat = formuladat;
                    reg.Concepcodi = concepcodi;
                    reg.Fechadat = fechaDat;
                    reg.Lastuser = User.Identity.Name;
                    reg.Fechaact = DateTime.Now;
                    reg.Deleted = ConstantesSubasta.GrupodatActivo;

                    this._desapachoAppService.SavePrGrupodat(reg);
                }
                if (tipoAccion == ConstantesSubasta.AccionEditar)
                {
                    PrGrupodatDTO reg = this._desapachoAppService.GetByIdPrGrupodat(fechaDat, concepcodi, grupocodi, ConstantesSubasta.GrupodatActivo);
                    if (reg == null)
                    {
                        throw new ArgumentException("El registro no existe, no puede modificarse.");
                    }
                    reg.Formuladat = formuladat;
                    reg.Lastuser = User.Identity.Name;
                    reg.Fechaact = DateTime.Now;

                    this._desapachoAppService.UpdatePrGrupodat(reg);
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
        /// Eliminar grupodat
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupodatEliminar(int grupocodi, int concepcodi, string strfechaDat, int deleted)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaDat = DateTime.ParseExact(strfechaDat, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                PrGrupodatDTO reg = this._desapachoAppService.GetByIdPrGrupodat(fechaDat, concepcodi, grupocodi, ConstantesSubasta.GrupodatActivo);

                if (reg == null)
                {
                    throw new ArgumentException("El registro no existe o ha sido eliminada.");
                }

                reg.Lastuser = User.Identity.Name;
                reg.Fechaact = DateTime.Now;
                reg.Deleted2 = ConstantesSubasta.GrupodatInactivo;

                this._desapachoAppService.UpdatePrGrupodat(reg);

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
        /// Listar periodo por año en formato JSON
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(anio);
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

        #endregion
    }
}