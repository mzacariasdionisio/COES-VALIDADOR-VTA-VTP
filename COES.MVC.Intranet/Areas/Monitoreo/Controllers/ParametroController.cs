using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.Monitoreo.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Monitoreo;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Monitoreo.Controllers
{
    [ValidarSesion]
    public class ParametroController : BaseController
    {
        ParametroAppServicio servParametro = new ParametroAppServicio();
        MonitoreoAppServicio servMonitoreo = new MonitoreoAppServicio();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ParametroController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #region HHI

        /// <summary>
        /// Index inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult ParametroHHI()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            ParametroModel model = new ParametroModel();
            model.FechaInicio = DateTime.Now.ToString(ConstantesAppServicio.FormatoMes);
            return View(model);
        }

        /// <summary>
        /// Listado ParametroHHI
        /// </summary>
        [HttpPost]
        public PartialViewResult ListarParametroHHI()
        {
            ParametroModel model = new ParametroModel();
            model.ListaParametroHHI = this.servMonitoreo.ListarParametroTendenciaHHI();

            return PartialView(model);
        }

        /// <summary>
        /// Save Parametros
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="aCero"></param>
        /// <param name="aUno"></param>
        public JsonResult SaveParametroHHI(string strfecha, decimal aCero, decimal aUno)
        {
            ParametroModel model = new ParametroModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime fecha = new DateTime(Int32.Parse(strfecha.Substring(3, 4)), Int32.Parse(strfecha.Substring(0, 2)), 1);
                var listaParam = this.servParametro.ListSiParametroValorByIdParametroAndFechaInicial(ConstantesParametro.IdParametroTendenciaHHI, fecha);

                //validacion de datos
                if (!(0 <= aCero && aCero <= 1))
                {
                    throw new Exception("Debe ingresar un valor de Tendencia a Cero válido");
                }
                if (!(0 <= aUno && aUno <= 1))
                {
                    throw new Exception("Debe ingresar un valor de Tendencia a Uno válido");
                }
                if (!(aCero < aUno))
                {
                    throw new Exception("El valor de la Tendencia a cero debe ser menos a la Tendencia a Uno");
                }


                if (listaParam.Count > 0)
                {
                    throw new Exception("Ya existe configuración de la tendencia del HHI para el período " + strfecha);
                }

                SiParametroValorDTO paramTendenCero = new SiParametroValorDTO();
                paramTendenCero.Siparcodi = ConstantesParametro.IdParametroTendenciaHHI;
                paramTendenCero.Siparvfechainicial = fecha;
                paramTendenCero.Siparvvalor = aCero;
                paramTendenCero.Siparvnota = ConstantesParametro.ValorMonitoreoTendenciaCero;
                paramTendenCero.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramTendenCero.Siparvusucreacion = User.Identity.Name;
                paramTendenCero.Siparvfeccreacion = DateTime.Now;
                this.servParametro.SaveSiParametroValor(paramTendenCero);


                SiParametroValorDTO paramTendenUno = new SiParametroValorDTO();
                paramTendenUno.Siparcodi = ConstantesParametro.IdParametroTendenciaHHI;
                paramTendenUno.Siparvfechainicial = fecha;
                paramTendenUno.Siparvvalor = aUno;
                paramTendenUno.Siparvnota = ConstantesParametro.ValorMonitoreoTendenciaUno;
                paramTendenUno.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramTendenUno.Siparvusucreacion = User.Identity.Name;
                paramTendenUno.Siparvfeccreacion = DateTime.Now;

                this.servParametro.SaveSiParametroValor(paramTendenUno);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Editar ParametroHHI
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarParametroHHI(int idCero, int idUno)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO paramHoraCero = this.servParametro.GetByIdSiParametroValor(idCero);
            SiParametroValorDTO paramHoraUno = this.servParametro.GetByIdSiParametroValor(idUno);

            ParametroTendenciaHHI hp = this.servParametro.GetParametroTendenciaHHIFromLista(paramHoraCero, paramHoraUno, null);
            model.ParametroHHI = hp;
            model.ListaEstado = this.servMonitoreo.ListarEstadoParametro();

            return PartialView(model);
        }

        /// <summary>
        /// Actualizar ParametroHHI
        /// </summary>
        /// <param name="idCero"></param>
        /// <param name="idUno"></param>
        /// <param name="aCero"></param>
        /// <param name="aUno"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarParametroHHI(int idCero, int idUno, decimal aCero, decimal aUno, string estado)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                this.ValidarSesionJsonResult();

                SiParametroValorDTO paramCero = this.servParametro.GetByIdSiParametroValor(idCero);
                paramCero.Siparvvalor = aCero;
                paramCero.Siparveliminado = estado;
                paramCero.Siparvusumodificacion = User.Identity.Name;
                paramCero.Siparvfecmodificacion = DateTime.Now;

                SiParametroValorDTO paramUno = this.servParametro.GetByIdSiParametroValor(idUno);
                paramUno.Siparvvalor = aUno;
                paramUno.Siparveliminado = estado;
                paramUno.Siparvusumodificacion = User.Identity.Name;
                paramUno.Siparvfecmodificacion = DateTime.Now;

                //validacion de datos
                if (!(0 <= paramCero.Siparvvalor && paramCero.Siparvvalor <= 1))
                {
                    throw new Exception("Debe ingresar un valor de Tendencia a Cero válido");
                }
                if (!(0 <= paramUno.Siparvvalor && paramUno.Siparvvalor <= 1))
                {
                    throw new Exception("Debe ingresar un valor de Tendencia a Uno válido");
                }
                if (!(paramCero.Siparvvalor < paramUno.Siparvvalor))
                {
                    throw new Exception("El valor de la Tendencia a cero debe ser menor a la Tendencia a Uno");
                }

                //actualización del parametro
                this.servParametro.UpdateSiParametroValor(paramCero);
                this.servParametro.UpdateSiParametroValor(paramUno);

                if (ConstantesParametro.EstadoActivo == estado)
                {
                    //actualizacion del estado de los demás parametros
                    List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroTendenciaHHI);
                    listaParam = listaParam.Where(x => x.Siparvcodi != idCero).ToList();
                    listaParam = listaParam.Where(x => x.Siparvcodi != idUno).ToList();
                    listaParam = listaParam.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo).ToList();

                    foreach (var reg in listaParam)
                    {
                        reg.Siparveliminado = ConstantesParametro.EstadoBaja;
                        reg.Siparvusumodificacion = User.Identity.Name;
                        reg.Siparvfecmodificacion = DateTime.Now;
                        this.servParametro.UpdateSiParametroValor(reg);
                    }
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return Json(model);
        }

        /// <summary>
        /// Ver ParametroHHI
        /// </summary>
        [HttpPost]
        public PartialViewResult VerParametroHHI(int idCero, int idUno)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO paramHoraCero = this.servParametro.GetByIdSiParametroValor(idCero);
            SiParametroValorDTO paramHoraUno = this.servParametro.GetByIdSiParametroValor(idUno);

            ParametroTendenciaHHI hp = this.servParametro.GetParametroTendenciaHHIFromLista(paramHoraCero, paramHoraUno, this.servMonitoreo.ListarEstadoParametro());
            model.ParametroHHI = hp;

            return PartialView(model);
        }

        #endregion

    }
}
