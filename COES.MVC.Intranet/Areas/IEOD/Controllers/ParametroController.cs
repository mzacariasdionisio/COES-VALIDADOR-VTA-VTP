using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class ParametroController : BaseController
    {
        ParametroAppServicio servParametro = new ParametroAppServicio();
        ConfiguracionCentralAppServicio appCentral = new ConfiguracionCentralAppServicio();
        EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        private int IdOpcionIndexCoordinador = 1022;

        private static readonly ILog Logger = log4net.LogManager.GetLogger(typeof(ParametroController));
        private static string NombreControlador = "ParametroController";
        private readonly List<EstadoParametro> ListaEstado = new List<EstadoParametro>();

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Logger.Error(NombreControlador, objErr);
            }
            catch (Exception ex)
            {
                Logger.Fatal(NombreControlador, ex);
                throw;
            }
        }

        public ParametroController()
        {
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = "N", EstadoDescripcion = "Activo" });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = "S", EstadoDescripcion = "Baja" });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = "P", EstadoDescripcion = "Pendiente" });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = "X", EstadoDescripcion = "Anulado" });
        }

        #region Magnitud de la Reserva Rotante para la RPF
        /// <summary>
        /// Index IndexMagnitudReservaFlotanteRPF 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMagnitudReservaFlotanteRPF()
        {
            if (!IsValidSesion) return base.RedirectToLogin();

            ParametroModel model = new ParametroModel();
            model.IdPeriodoAvenida = ConstantesParametro.ValorPeriodoAvenida;
            model.IdPeriodoEstiaje = ConstantesParametro.ValorPeriodoEstiaje;

            return View(model);
        }

        /// <summary>
        /// Listado Magnitud de la Reserva Rotante para la RPF
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoMagnitudRPF()
        {
            ParametroModel model = new ParametroModel();

            GenerarMagnitudRPF(model);

            return PartialView(model);
        }

        /// <summary>
        /// Generar Magnitud de la Reserva Rotante para la RPF
        /// </summary>
        /// <param name="model"></param>
        private void GenerarMagnitudRPF(ParametroModel model)
        {
            List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroMagnitudRPF);
            model.ListaMagnitudRPF = this.servParametro.GetListaParametroMagnitudRPF(listaParam, this.ListaEstado);
            model.ListaMagnitudRPF = model.ListaMagnitudRPF.OrderBy(x => x.Estado).ThenByDescending(x => x.FechaInicio).ToList();
        }

        /// <summary>
        /// Registrar Magnitud de la Reserva Rotante para la RPF
        /// </summary>
        [HttpPost]
        public PartialViewResult NuevoMagnitudRPF(string periodo)
        {
            ParametroModel model = new ParametroModel();
            model.MagnitudRPF = new ParametroMagnitudRPF();
            model.MagnitudRPF.FechaFormatoInicio = DateTime.Now.ToString(ConstantesBase.FormatoFechaBase);
            model.MagnitudRPF.FechaFormatoFin = DateTime.Now.ToString(ConstantesBase.FormatoFechaBase);
            model.MagnitudRPF.Periodo = periodo;

            model.Anho = DateTime.Now.Year;

            return PartialView(model);
        }

        /// <summary>
        /// Registrar Magnitud de la Reserva Rotante para la RPF
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="normativa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarMagnitudRPF(string fechaInicio, string fechaFin, string periodo, decimal magnitud)
        {
            try
            {
                DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
                DateTime fechaFinal = DateTime.ParseExact(fechaFin, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);

                SiParametroValorDTO param = new SiParametroValorDTO();
                param.Siparcodi = ConstantesParametro.IdParametroMagnitudRPF;
                param.Siparvfechainicial = fechaInicial;
                param.Siparvfechafinal = fechaFinal;
                param.Siparvnota = periodo;
                param.Siparvvalor = magnitud;
                param.Siparveliminado = ConstantesParametro.EstadoActivo;
                param.Siparvusucreacion = User.Identity.Name;
                param.Siparvfeccreacion = DateTime.Now;

                //validacion
                if (param.Siparvfechainicial.Value.Date >= param.Siparvfechafinal.Value.Date)
                {
                    return Json("La Fecha Inicio debe ser menor que la Fecha Fin");
                }

                if (magnitud < 0)
                {
                    return Json("La magnitud debe ser mayor o igual a 0");
                }

                //validacion de existencia
                var listaFecha = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroMagnitudRPF);
                listaFecha = listaFecha.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo).ToList();

                foreach (var fecha in listaFecha)
                {
                    if ((fecha.Siparvfechainicial.Value.Date <= fechaInicial && fechaInicial <= fecha.Siparvfechafinal.Value.Date)
                        || (fecha.Siparvfechainicial.Value.Date <= fechaFinal && fechaFinal <= fecha.Siparvfechafinal.Value.Date))
                    {
                        return Json("Existe cruce de rangos");
                    }
                }

                this.servParametro.SaveSiParametroValor(param);

                return Json(1);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Editar Magnitud de la Reserva Rotante para la RPF
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarMagnitudRPF(int idRango)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO param = this.servParametro.GetByIdSiParametroValor(idRango);

            ParametroMagnitudRPF rango = this.servParametro.GetParametroMagnitudRPF(param, this.ListaEstado);
            rango.Magnitud = rango.Magnitud;
            model.MagnitudRPF = rango;
            model.ListaEstado = this.ListaEstado;

            return PartialView(model);
        }

        /// <summary>
        /// Actualizar Magnitud de la Reserva Rotante para la RPF
        /// </summary>
        /// <param name="idRango"></param>
        /// <param name="estado"></param>
        /// <param name="normativa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarMagnitudRPF(int idRango, string estado, decimal magnitud)
        {
            try
            {
                SiParametroValorDTO param = this.servParametro.GetByIdSiParametroValor(idRango);
                param.Siparveliminado = estado;
                param.Siparvusumodificacion = User.Identity.Name;
                param.Siparvfecmodificacion = DateTime.Now;
                param.Siparvvalor = magnitud;

                //validacion
                if (magnitud < 0)
                {
                    return Json("La magnitud debe ser mayor o igual a 0");
                }

                this.servParametro.UpdateSiParametroValor(param);

                return Json(1);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Ver Magnitud de la Reserva Rotante para la RPF
        /// </summary>
        [HttpPost]
        public PartialViewResult VerMagnitudRPF(int idRango)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO param = this.servParametro.GetByIdSiParametroValor(idRango);

            ParametroMagnitudRPF rango = this.servParametro.GetParametroMagnitudRPF(param, this.ListaEstado);
            model.MagnitudRPF = rango;

            return PartialView(model);
        }
        #endregion

        #region Configuración aplicativo

        public ActionResult IndexParametroHo()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            ParametroModel model = new ParametroModel();
            model.ListaEmpresas = appCentral.ListarEmpresasTermicas();

            return View(model);
        }

        /// <summary>
        /// Listado color central
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoCentralesColor(int idEmpresa)
        {
            int iEmpresa = idEmpresa;
            int iEquipo = -2;
            string sNombre = "";

            var model = new ColorModel();

            //defecto electrico
            int iFamilia = ConstantesIEOD.IDFamiliaTermica;
            int iTipoEmpresa = -2;
            string sEstado = "A";

            var lsResultado = appCentral.ListarCentralColorProp(iEmpresa, iFamilia,
                iTipoEmpresa, iEquipo, sEstado, sNombre, 1, 1000);
            foreach (var oEquipo in lsResultado)
            {
                oEquipo.EstadoDesc = Servicios.Aplicacion.Equipamiento.Helper.EquipamientoHelper.EstadoDescripcion(oEquipo.Equiestado);
                oEquipo.Osigrupocodi = EquipamientoHelper.EstiloEstado(oEquipo.Equiestado);

            }
            model.ListadoEquipamiento = lsResultado;
            return PartialView(model);
        }

        /// <summary>
        /// grabar color central
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarPropiedadColor(int equicodi, string color)
        {
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionIndexCoordinador)) throw new Exception(Constantes.MensajePermisoNoValido);

                EqPropequiDTO entity = new EqPropequiDTO
                {
                    Propcodi = ConstantesIEOD.IDPropiedadColor,
                    Equicodi = equicodi,
                    Fechapropequi = DateTime.Now.Date,
                    Propequicheckcero = 0,
                    Valor = color,
                    Propequiusucreacion = base.UserName
                };

                appEquipamiento.SaveEqPropequi(entity);

                return Json(1);

            }
            catch
            {
                return Json(-1);
            }
        }

        #region Umbral

        /// <summary>
        /// Obtener valor umbral
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerUmbral()
        {
            ParametroModel model = new ParametroModel();

            try
            {
                base.ValidarSesionJsonResult();

                var listaParametroValor = servParametro.ListSiParametroValorByIdParametro(ConstantesHorasOperacion.IdParametroUmbral).OrderByDescending(x => x.Siparvfeccreacion).ToList();

                if (listaParametroValor.Any())
                {
                    var regParam1 = listaParametroValor.First(); // toma el último elemento guardado

                    model.ValorUmbral = regParam1.Siparvvalor;
                    model.UsuarioModificacion = regParam1.Siparvusucreacion;
                    model.FechaModificacion = regParam1.Siparvfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                }

                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador, ex);
                model.Resultado = -1;
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// guardar valor Umbral
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarUmbral(decimal valor)
        {
            ParametroModel modelResultado = new ParametroModel();

            try
            {
                base.ValidarSesionJsonResult();

                SiParametroValorDTO param = new SiParametroValorDTO
                {
                    Siparcodi = ConstantesHorasOperacion.IdParametroUmbral,

                    Siparvvalor = valor,
                    Siparveliminado = "N",
                    Siparvusucreacion = base.UserName,
                    Siparvfeccreacion = DateTime.Now
                };

                //Guardar en parámetro valor
                servParametro.SaveSiParametroValor(param);

                modelResultado.Resultado = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador, ex);
                modelResultado.Resultado = -1;
                modelResultado.StrMensaje = ex.Message;
                modelResultado.Detalle = ex.StackTrace;
            }

            return Json(modelResultado);
        }

        #endregion

        #endregion

    }
}
