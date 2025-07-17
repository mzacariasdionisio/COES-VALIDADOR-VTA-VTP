using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class ParametroController : BaseController
    {
        readonly ParametroAppServicio servParametro = new ParametroAppServicio();

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private readonly List<EstadoParametro> ListaEstado = new List<EstadoParametro>();

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

        public ParametroController()
        {
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = "N", EstadoDescripcion = "Activo" });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = "S", EstadoDescripcion = "Eliminado" });
            ListaEstado.Add(new EstadoParametro { EstadoCodigo = "X", EstadoDescripcion = "Anulado" });
        }

        #endregion

        #region Rango de Operación de Centrales Solares
        /// <summary>
        /// Index RangoOperacionCentralSolar 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexRangoOperacionCentralSolar()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ParametroModel model = new ParametroModel();

            return View(model);
        }

        /// <summary>
        /// Listado RangoOperacionCentralSolar
        /// </summary>
        [HttpPost]
        public PartialViewResult ListadoRangoOperacionCentralSolar()
        {
            ParametroModel model = new ParametroModel();

            GenerarParametroRangoSolar(model);

            return PartialView(model);
        }

        /// <summary>
        /// Generar ParametroRangoSolar
        /// </summary>
        /// <param name="model"></param>
        private void GenerarParametroRangoSolar(ParametroModel model)
        {
            model.RangoSolar = new ParametroRangoSolar();
            List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoSolar);
            model.ListaRangoSolar = this.servParametro.GetListaParametroSolar(listaParam, this.ListaEstado, ParametrosFormato.ResolucionCuartoHora);
            model.ListaRangoSolar = model.ListaRangoSolar.OrderBy(x => x.Estado).ThenByDescending(x => x.Fecha).ToList();

            if (model.ListaRangoSolar.Count > 0)
            {
                var solar = model.ListaRangoSolar.Where(x => x.Estado == ConstantesParametro.EstadoActivo).FirstOrDefault();
                if (solar != null)
                {
                    model.RangoSolar = solar;
                    model.RangoSolar.FechaFormatoLetra = EPDate.f_FechaenLetras(model.RangoSolar.Fecha);
                }
            }
        }

        /// <summary>
        /// Registrar nuevo Rango Operacion Central Solar
        /// </summary>
        [HttpPost]
        public PartialViewResult NuevoRangoOperacionCentralSolar()
        {
            ParametroModel model = new ParametroModel();
            model.RangoSolar = new ParametroRangoSolar();
            model.RangoSolar.FechaFormato = DateTime.Now.ToString(Constantes.FormatoFecha);

            return PartialView(model);
        }

        /// <summary>
        /// Registrar RangoOperacionCentralSolar
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="horaInicio"></param>
        /// <param name="horaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarRangoOperacionCentralSolar(string fechaInicio, string horaInicio, string horaFin)
        {
            try
            {
                DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                SiParametroValorDTO paramHoraInicio = new SiParametroValorDTO();
                paramHoraInicio.Siparcodi = ConstantesParametro.IdParametroRangoSolar;
                paramHoraInicio.Siparvfechainicial = fechaInicial;
                paramHoraInicio.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(horaInicio);
                paramHoraInicio.Siparvnota = ConstantesParametro.ValorHoraInicioSolar;
                paramHoraInicio.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramHoraInicio.Siparvusucreacion = base.UserName;
                paramHoraInicio.Siparvfeccreacion = DateTime.Now;

                SiParametroValorDTO paramHoraFin = new SiParametroValorDTO();
                paramHoraFin.Siparcodi = ConstantesParametro.IdParametroRangoSolar;
                paramHoraFin.Siparvfechainicial = fechaInicial;
                paramHoraFin.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(horaFin);
                paramHoraFin.Siparvnota = ConstantesParametro.ValorHoraFinSolar;
                paramHoraFin.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramHoraFin.Siparvusucreacion = base.UserName;
                paramHoraFin.Siparvfeccreacion = DateTime.Now;

                //validacion de datos
                if (paramHoraInicio.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una hora de inicio válida");
                }
                if (paramHoraFin.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una hora de fin válida");
                }

                if (paramHoraInicio.Siparvvalor >= paramHoraFin.Siparvvalor)
                {
                    return Json("La hora de fin debe ser mayor a la hora de inicio");
                }

                //validacion de existencia
                var listaFecha = this.servParametro.ListSiParametroValorByIdParametroAndFechaInicial(ConstantesParametro.IdParametroRangoSolar, fechaInicial);
                if (listaFecha.Count > 0)
                {
                    return Json("Ya existe Rango de operación para la fecha indicada");
                }

                //actualizacion del estado de los demás parametros
                List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoSolar);
                listaParam = listaParam.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo).ToList();

                foreach (var reg in listaParam)
                {
                    reg.Siparveliminado = ConstantesParametro.EstadoBaja;
                    reg.Siparvusumodificacion = base.UserName;
                    reg.Siparvfecmodificacion = DateTime.Now;
                    this.servParametro.UpdateSiParametroValor(reg);
                }

                //registro del parametro
                this.servParametro.SaveSiParametroValor(paramHoraInicio);
                this.servParametro.SaveSiParametroValor(paramHoraFin);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Editar Rango Operacion Central Solar
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarRangoOperacionCentralSolar(int idHoraIni, int idHoraFin)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO paramHoraIni = this.servParametro.GetByIdSiParametroValor(idHoraIni);
            SiParametroValorDTO paramHoraFin = this.servParametro.GetByIdSiParametroValor(idHoraFin);

            ParametroRangoSolar solar = this.servParametro.GetParametroSolar(paramHoraIni, paramHoraFin, this.ListaEstado, ParametrosFormato.ResolucionCuartoHora);
            model.RangoSolar = solar;
            model.ListaEstado = this.ListaEstado;

            return PartialView(model);
        }

        /// <summary>
        /// Actualizar OperacionCentralSolar
        /// </summary>
        /// <param name="idHoraIni"></param>
        /// <param name="idHoraFin"></param>
        /// <param name="horaInicio"></param>
        /// <param name="horaFin"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarOperacionCentralSolar(int idHoraIni, int idHoraFin, string horaInicio, string horaFin, string estado)
        {
            try
            {
                SiParametroValorDTO paramHoraInicio = this.servParametro.GetByIdSiParametroValor(idHoraIni);
                paramHoraInicio.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(horaInicio);
                paramHoraInicio.Siparveliminado = estado;
                paramHoraInicio.Siparvusumodificacion = base.UserName;
                paramHoraInicio.Siparvfecmodificacion = DateTime.Now;

                SiParametroValorDTO paramHoraFin = this.servParametro.GetByIdSiParametroValor(idHoraFin);
                paramHoraFin.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(horaFin);
                paramHoraFin.Siparveliminado = estado;
                paramHoraFin.Siparvusumodificacion = base.UserName;
                paramHoraFin.Siparvfecmodificacion = DateTime.Now;

                //validacion de datos
                if (paramHoraInicio.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una hora de inicio válida");
                }
                if (paramHoraFin.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una hora de fin válida");
                }

                if (paramHoraInicio.Siparvvalor >= paramHoraFin.Siparvvalor)
                {
                    return Json("La hora de fin debe ser mayor a la hora de inicio");
                }

                //actualización del parametro
                this.servParametro.UpdateSiParametroValor(paramHoraInicio);
                this.servParametro.UpdateSiParametroValor(paramHoraFin);

                if (ConstantesParametro.EstadoActivo == estado)
                {
                    //actualizacion del estado de los demás parametros
                    List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoSolar);
                    listaParam = listaParam.Where(x => x.Siparvcodi != idHoraIni).ToList();
                    listaParam = listaParam.Where(x => x.Siparvcodi != idHoraFin).ToList();
                    listaParam = listaParam.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo).ToList();

                    foreach (var reg in listaParam)
                    {
                        reg.Siparveliminado = ConstantesParametro.EstadoBaja;
                        reg.Siparvusumodificacion = base.UserName;
                        reg.Siparvfecmodificacion = DateTime.Now;
                        this.servParametro.UpdateSiParametroValor(reg);
                    }
                }

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Ver Rango Operacion Central Solar
        /// </summary>
        [HttpPost]
        public PartialViewResult VerRangoOperacionCentralSolar(int idHoraIni, int idHoraFin)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO paramHoraIni = this.servParametro.GetByIdSiParametroValor(idHoraIni);
            SiParametroValorDTO paramHoraFin = this.servParametro.GetByIdSiParametroValor(idHoraFin);

            ParametroRangoSolar solar = this.servParametro.GetParametroSolar(paramHoraIni, paramHoraFin, this.ListaEstado, ParametrosFormato.ResolucionCuartoHora);
            model.RangoSolar = solar;

            return PartialView(model);
        }
        #endregion

        #region Hora Punta para Potencia Activa

        /// <summary>
        /// Index HPPotenciaActiva
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexHPPotenciaActiva()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ParametroModel model = new ParametroModel();

            return View(model);
        }

        /// <summary>
        /// Listado HPPotenciaActiva
        /// </summary>
        [HttpPost]
        public PartialViewResult ListadoHPPotenciaActiva()
        {
            ParametroModel model = new ParametroModel();

            GenerarParametroHPPotenciaActiva(model);

            return PartialView(model);
        }

        /// <summary>
        /// Generar ParametroHPPotenciaActiva
        /// </summary>
        /// <param name="model"></param>
        private void GenerarParametroHPPotenciaActiva(ParametroModel model)
        {
            model.HPPotenciaActiva = new ParametroHPPotenciaActiva();
            List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);
            model.ListaHPPotenciaActiva = ParametroAppServicio.GetListaParametroHPPotenciaActiva(listaParam, DateTime.Today, ParametrosFormato.ResolucionCuartoHora).OrderByDescending(x => x.Fecha).ToList();

            var hp = model.ListaHPPotenciaActiva.Find(x => x.EsVigente);
            if (hp != null)
            {
                model.HPPotenciaActiva = hp;
                model.HPPotenciaActiva.FechaFormatoLetra = EPDate.f_FechaenLetras(model.HPPotenciaActiva.Fecha);
            }
        }

        /// <summary>
        /// Registrar HPPotenciaActiva
        /// </summary>
        [HttpPost]
        public PartialViewResult NuevoHPPotenciaActiva()
        {
            ParametroModel model = new ParametroModel();
            model.HPPotenciaActiva = new ParametroHPPotenciaActiva();
            model.HPPotenciaActiva.FechaFormato = DateTime.Now.ToString(Constantes.FormatoFecha);

            return PartialView(model);
        }

        /// <summary>
        /// Registrar HPPotenciaActiva
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="horaMinima"></param>
        /// <param name="horaMedia"></param>
        /// <param name="horaMaxima"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarHPPotenciaActiva(string fechaInicio, string horaMinima, string horaMedia, string horaMaxima)
        {
            try
            {
                DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                SiParametroValorDTO paramHoraMinima = new SiParametroValorDTO();
                paramHoraMinima.Siparcodi = ConstantesParametro.IdParametroHPPotenciaActiva;
                paramHoraMinima.Siparvfechainicial = fechaInicial;
                paramHoraMinima.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(horaMinima);
                paramHoraMinima.Siparvnota = ConstantesParametro.ValorHoraMinimaHP;
                paramHoraMinima.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramHoraMinima.Siparvusucreacion = base.UserName;
                paramHoraMinima.Siparvfeccreacion = DateTime.Now;

                SiParametroValorDTO paramHoraMedia = new SiParametroValorDTO();
                paramHoraMedia.Siparcodi = ConstantesParametro.IdParametroHPPotenciaActiva;
                paramHoraMedia.Siparvfechainicial = fechaInicial;
                paramHoraMedia.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(horaMedia);
                paramHoraMedia.Siparvnota = ConstantesParametro.ValorHoraMediaHP;
                paramHoraMedia.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramHoraMedia.Siparvusucreacion = base.UserName;
                paramHoraMedia.Siparvfeccreacion = DateTime.Now;

                SiParametroValorDTO paramHoraMaxima = new SiParametroValorDTO();
                paramHoraMaxima.Siparcodi = ConstantesParametro.IdParametroHPPotenciaActiva;
                paramHoraMaxima.Siparvfechainicial = fechaInicial;
                paramHoraMaxima.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(horaMaxima);
                paramHoraMaxima.Siparvnota = ConstantesParametro.ValorHoraMaximaHP;
                paramHoraMaxima.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramHoraMaxima.Siparvusucreacion = base.UserName;
                paramHoraMaxima.Siparvfeccreacion = DateTime.Now;

                //validacion de datos
                if (paramHoraMinima.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una hora mínima válida");
                }
                if (paramHoraMedia.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una hora media válida");
                }
                if (paramHoraMaxima.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una hora máxima válida");
                }

                if (paramHoraMinima.Siparvvalor >= paramHoraMedia.Siparvvalor)
                {
                    return Json("La hora media debe ser mayor a la hora mínima");
                }

                if (paramHoraMinima.Siparvvalor >= paramHoraMaxima.Siparvvalor)
                {
                    return Json("La hora máxima debe ser mayor a la hora media");
                }

                //validacion de existencia
                var listaFecha = this.servParametro.ListSiParametroValorByIdParametroAndFechaInicial(ConstantesParametro.IdParametroHPPotenciaActiva, fechaInicial);
                if (listaFecha.Count > 0)
                {
                    return Json("Ya existe Horas punta para Potencia Activa para la fecha indicada");
                }

                //registro del parametro
                this.servParametro.SaveSiParametroValor(paramHoraMinima);
                this.servParametro.SaveSiParametroValor(paramHoraMedia);
                this.servParametro.SaveSiParametroValor(paramHoraMaxima);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Editar HPPotenciaActiva
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarHPPotenciaActiva(int idHoraMinima, int idHoraMedia, int idHoraMaxima)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO paramHoraMinima = this.servParametro.GetByIdSiParametroValor(idHoraMinima);
            SiParametroValorDTO paramHoraMedia = this.servParametro.GetByIdSiParametroValor(idHoraMedia);
            SiParametroValorDTO paramHoraMaxima = this.servParametro.GetByIdSiParametroValor(idHoraMaxima);

            ParametroHPPotenciaActiva hp = ParametroAppServicio.GetParametroHPPotenciaActiva(paramHoraMinima, paramHoraMedia, paramHoraMaxima, ParametrosFormato.ResolucionCuartoHora);
            model.HPPotenciaActiva = hp;
            model.ListaEstado = this.ListaEstado;

            return PartialView(model);
        }

        /// <summary>
        /// Actualizar HPPotenciaActiva
        /// </summary>
        /// <param name="idHoraMinima"></param>
        /// <param name="idHoraMedia"></param>
        /// <param name="idHoraMaxima"></param>
        /// <param name="horaMinima"></param>
        /// <param name="horaMedia"></param>
        /// <param name="horaMaxima"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarHPPotenciaActiva(int idHoraMinima, int idHoraMedia, int idHoraMaxima, string horaMinima, string horaMedia, string horaMaxima, string estado)
        {
            try
            {
                SiParametroValorDTO paramHoraMinima = this.servParametro.GetByIdSiParametroValor(idHoraMinima);
                paramHoraMinima.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(horaMinima);
                paramHoraMinima.Siparveliminado = estado;
                paramHoraMinima.Siparvusumodificacion = base.UserName;
                paramHoraMinima.Siparvfecmodificacion = DateTime.Now;

                SiParametroValorDTO paramHoraMedia = this.servParametro.GetByIdSiParametroValor(idHoraMedia);
                paramHoraMedia.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(horaMedia);
                paramHoraMedia.Siparveliminado = estado;
                paramHoraMedia.Siparvusumodificacion = base.UserName;
                paramHoraMedia.Siparvfecmodificacion = DateTime.Now;

                SiParametroValorDTO paramHoraMaxima = this.servParametro.GetByIdSiParametroValor(idHoraMaxima);
                paramHoraMaxima.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(horaMaxima);
                paramHoraMaxima.Siparveliminado = estado;
                paramHoraMaxima.Siparvusumodificacion = base.UserName;
                paramHoraMaxima.Siparvfecmodificacion = DateTime.Now;

                //validacion de datos
                if (paramHoraMinima.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una hora mínima válida");
                }
                if (paramHoraMedia.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una hora media válida");
                }
                if (paramHoraMaxima.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una hora máxima válida");
                }

                if (paramHoraMinima.Siparvvalor >= paramHoraMedia.Siparvvalor)
                {
                    return Json("La hora media debe ser mayor a la hora mínima");
                }

                if (paramHoraMinima.Siparvvalor >= paramHoraMaxima.Siparvvalor)
                {
                    return Json("La hora máxima debe ser mayor a la hora media");
                }

                //actualización del parametro
                this.servParametro.UpdateSiParametroValor(paramHoraMinima);
                this.servParametro.UpdateSiParametroValor(paramHoraMedia);
                this.servParametro.UpdateSiParametroValor(paramHoraMaxima);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Ver HPPotenciaActiva
        /// </summary>
        [HttpPost]
        public PartialViewResult VerHPPotenciaActiva(int idHoraMinima, int idHoraMedia, int idHoraMaxima)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO paramHoraMinima = this.servParametro.GetByIdSiParametroValor(idHoraMinima);
            SiParametroValorDTO paramHoraMedia = this.servParametro.GetByIdSiParametroValor(idHoraMedia);
            SiParametroValorDTO paramHoraMaxima = this.servParametro.GetByIdSiParametroValor(idHoraMaxima);

            ParametroHPPotenciaActiva hp = ParametroAppServicio.GetParametroHPPotenciaActiva(paramHoraMinima, paramHoraMedia, paramHoraMaxima, ParametrosFormato.ResolucionCuartoHora);
            model.HPPotenciaActiva = hp;

            return PartialView(model);
        }

        /// <summary>
        /// Eliminar registro
        /// </summary>
        /// <param name="idHoraMinima"></param>
        /// <param name="idHoraMedia"></param>
        /// <param name="idHoraMaxima"></param>
        /// <returns></returns>
        public JsonResult EliminarHPPotenciaActiva(int idHoraMinima, int idHoraMedia, int idHoraMaxima)
        {
            try
            {
                servParametro.EliminarParametrosHPPotenciaActiva(idHoraMinima, idHoraMedia, idHoraMaxima, base.UserName);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        #endregion

        #region Rango de Análisis de Potencia Inductiva
        /// <summary>
        /// Index RangoPotenciaInductiva
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexRangoPotenciaInductiva()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ParametroModel model = new ParametroModel();

            return View(model);
        }

        /// <summary>
        /// Listado RangoPotenciaInductiva
        /// </summary>
        [HttpPost]
        public PartialViewResult ListadoRangoPotenciaInductiva()
        {
            ParametroModel model = new ParametroModel();

            GenerarRangoPotenciaInductiva(model);

            return PartialView(model);
        }

        /// <summary>
        /// Generar RangoPotenciaInductiva
        /// </summary>
        /// <param name="model"></param>
        private void GenerarRangoPotenciaInductiva(ParametroModel model)
        {
            model.RangoPotenciaInductiva = new ParametroRangoPotenciaInductiva();
            List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPotenciaInductiva);
            model.ListaRangoPotenciaInductiva = this.servParametro.GetListaParametroRangoPotenciaInductiva(listaParam, this.ListaEstado, ParametrosFormato.ResolucionCuartoHora);
            model.ListaRangoPotenciaInductiva = model.ListaRangoPotenciaInductiva.OrderBy(x => x.Estado).ThenByDescending(x => x.Fecha).ToList();

            if (model.ListaRangoPotenciaInductiva.Count > 0)
            {
                var hp = model.ListaRangoPotenciaInductiva.Where(x => x.Estado == ConstantesParametro.EstadoActivo).FirstOrDefault();
                if (hp != null)
                {
                    model.RangoPotenciaInductiva = hp;
                    model.RangoPotenciaInductiva.FechaFormatoLetra = EPDate.f_FechaenLetras(model.RangoPotenciaInductiva.Fecha);
                }
            }
        }

        /// <summary>
        /// Registrar RangoPotenciaInductiva
        /// </summary>
        [HttpPost]
        public PartialViewResult NuevoRangoPotenciaInductiva()
        {
            ParametroModel model = new ParametroModel();
            model.RangoPotenciaInductiva = new ParametroRangoPotenciaInductiva();
            model.RangoPotenciaInductiva.FechaFormato = DateTime.Now.ToString(Constantes.FormatoFecha);

            return PartialView(model);
        }

        /// <summary>
        /// Registrar RangoPotenciaInductiva
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="h1Ini"></param>
        /// <param name="h1Fin"></param>
        /// <param name="h2Ini"></param>
        /// <param name="h2Fin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarRangoPotenciaInductiva(string fechaInicio, string h1Ini, string h1Fin, string h2Ini, string h2Fin)
        {
            try
            {
                DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                SiParametroValorDTO paramH1Ini = new SiParametroValorDTO();
                paramH1Ini.Siparcodi = ConstantesParametro.IdParametroRangoPotenciaInductiva;
                paramH1Ini.Siparvfechainicial = fechaInicial;
                paramH1Ini.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(h1Ini);
                paramH1Ini.Siparvnota = ConstantesParametro.ValorH1Ini;
                paramH1Ini.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramH1Ini.Siparvusucreacion = base.UserName;
                paramH1Ini.Siparvfeccreacion = DateTime.Now;

                SiParametroValorDTO paramH1Fin = new SiParametroValorDTO();
                paramH1Fin.Siparcodi = ConstantesParametro.IdParametroRangoPotenciaInductiva;
                paramH1Fin.Siparvfechainicial = fechaInicial;
                paramH1Fin.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(h1Fin);
                paramH1Fin.Siparvnota = ConstantesParametro.ValorH1Fin;
                paramH1Fin.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramH1Fin.Siparvusucreacion = base.UserName;
                paramH1Fin.Siparvfeccreacion = DateTime.Now;

                SiParametroValorDTO paramH2Ini = new SiParametroValorDTO();
                paramH2Ini.Siparcodi = ConstantesParametro.IdParametroRangoPotenciaInductiva;
                paramH2Ini.Siparvfechainicial = fechaInicial;
                paramH2Ini.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(h2Ini);
                paramH2Ini.Siparvnota = ConstantesParametro.ValorH2Ini;
                paramH2Ini.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramH2Ini.Siparvusucreacion = base.UserName;
                paramH2Ini.Siparvfeccreacion = DateTime.Now;

                SiParametroValorDTO paramH2Fin = new SiParametroValorDTO();
                paramH2Fin.Siparcodi = ConstantesParametro.IdParametroRangoPotenciaInductiva;
                paramH2Fin.Siparvfechainicial = fechaInicial;
                paramH2Fin.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(h2Fin);
                paramH2Fin.Siparvnota = ConstantesParametro.ValorH2Fin;
                paramH2Fin.Siparveliminado = ConstantesParametro.EstadoActivo;
                paramH2Fin.Siparvusucreacion = base.UserName;
                paramH2Fin.Siparvfeccreacion = DateTime.Now;

                //validacion de datos
                if (paramH1Ini.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una Hora Inicio 1 válida");
                }
                if (paramH1Fin.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una Hora Fin 1 válida");
                }

                if (paramH2Ini.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una Hora Inicio 2 válida");
                }
                if (paramH2Fin.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una Hora Fin 2 válida");
                }

                if (paramH1Ini.Siparvvalor >= paramH1Fin.Siparvvalor)
                {
                    return Json("La hora de fin 1 debe ser mayor a la hora de inicio 1");
                }
                if (paramH2Ini.Siparvvalor >= paramH2Fin.Siparvvalor)
                {
                    return Json("La hora de fin 2 debe ser mayor a la hora de inicio 2");
                }

                if (paramH1Fin.Siparvvalor >= paramH2Ini.Siparvvalor)
                {
                    return Json("La hora de inicio 2 debe ser mayor a la hora de fin 1");
                }

                //validacion de existencia
                var listaFecha = this.servParametro.ListSiParametroValorByIdParametroAndFechaInicial(ConstantesParametro.IdParametroRangoPotenciaInductiva, fechaInicial);
                if (listaFecha.Count > 0)
                {
                    return Json("Ya existe Rango de Análisis de Potencia Inductiva para la fecha indicada");
                }

                //actualizacion del estado de los demás parametros
                List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPotenciaInductiva);
                listaParam = listaParam.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo).ToList();

                foreach (var reg in listaParam)
                {
                    reg.Siparveliminado = ConstantesParametro.EstadoBaja;
                    reg.Siparvusumodificacion = base.UserName;
                    reg.Siparvfecmodificacion = DateTime.Now;
                    this.servParametro.UpdateSiParametroValor(reg);
                }

                //registro del parametro
                this.servParametro.SaveSiParametroValor(paramH1Ini);
                this.servParametro.SaveSiParametroValor(paramH1Fin);
                this.servParametro.SaveSiParametroValor(paramH2Ini);
                this.servParametro.SaveSiParametroValor(paramH2Fin);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Editar HPPotenciaActiva
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarRangoPotenciaInductiva(int idH1Ini, int idH1Fin, int idH2Ini, int idH2Fin)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO paramH1Ini = this.servParametro.GetByIdSiParametroValor(idH1Ini);
            SiParametroValorDTO paramH1Fin = this.servParametro.GetByIdSiParametroValor(idH1Fin);
            SiParametroValorDTO paramH2Ini = this.servParametro.GetByIdSiParametroValor(idH2Ini);
            SiParametroValorDTO paramH2Fin = this.servParametro.GetByIdSiParametroValor(idH2Fin);

            ParametroRangoPotenciaInductiva hp = this.servParametro.GetParametroRangoPotenciaInductiva(paramH1Ini, paramH1Fin, paramH2Ini, paramH2Fin, this.ListaEstado, ParametrosFormato.ResolucionCuartoHora);
            model.RangoPotenciaInductiva = hp;
            model.ListaEstado = this.ListaEstado;

            return PartialView(model);
        }

        /// <summary>
        /// Actualizar RangoPotenciaInductiva
        /// </summary>
        /// <param name="idH1Ini"></param>
        /// <param name="idH1Fin"></param>
        /// <param name="idH2Ini"></param>
        /// <param name="idH2Fin"></param>
        /// <param name="h1Ini"></param>
        /// <param name="h1Fin"></param>
        /// <param name="h2Ini"></param>
        /// <param name="h2Fin"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarRangoPotenciaInductiva(int idH1Ini, int idH1Fin, int idH2Ini, int idH2Fin, string h1Ini, string h1Fin, string h2Ini, string h2Fin, string estado)
        {
            try
            {
                SiParametroValorDTO paramH1Ini = this.servParametro.GetByIdSiParametroValor(idH1Ini);
                paramH1Ini.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(h1Ini);
                paramH1Ini.Siparveliminado = estado;
                paramH1Ini.Siparvusumodificacion = base.UserName;
                paramH1Ini.Siparvfecmodificacion = DateTime.Now;

                SiParametroValorDTO paramH1Fin = this.servParametro.GetByIdSiParametroValor(idH1Fin);
                paramH1Fin.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(h1Fin);
                paramH1Fin.Siparveliminado = estado;
                paramH1Fin.Siparvusumodificacion = base.UserName;
                paramH1Fin.Siparvfecmodificacion = DateTime.Now;

                SiParametroValorDTO paramH2Ini = this.servParametro.GetByIdSiParametroValor(idH2Ini);
                paramH2Ini.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(h2Ini);
                paramH2Ini.Siparveliminado = estado;
                paramH2Ini.Siparvusumodificacion = base.UserName;
                paramH2Ini.Siparvfecmodificacion = DateTime.Now;

                SiParametroValorDTO paramH2Fin = this.servParametro.GetByIdSiParametroValor(idH2Fin);
                paramH2Fin.Siparvvalor = ParametroAppServicio.ConvertirMinutosFormatoNumero(h2Fin);
                paramH2Fin.Siparveliminado = estado;
                paramH2Fin.Siparvusumodificacion = base.UserName;
                paramH2Fin.Siparvfecmodificacion = DateTime.Now;

                //validacion de datos
                if (paramH1Ini.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una Hora Inicio 1 válida");
                }
                if (paramH1Fin.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una Hora Fin 1 válida");
                }

                if (paramH2Ini.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una Hora Inicio 2 válida");
                }
                if (paramH2Fin.Siparvvalor == -1)
                {
                    return Json("Debe ingresar una Hora Fin 2 válida");
                }

                if (paramH1Ini.Siparvvalor >= paramH1Fin.Siparvvalor)
                {
                    return Json("La hora de fin 1 debe ser mayor a la hora de inicio 1");
                }
                if (paramH2Ini.Siparvvalor >= paramH2Fin.Siparvvalor)
                {
                    return Json("La hora de fin 2 debe ser mayor a la hora de inicio 2");
                }

                if (paramH1Fin.Siparvvalor >= paramH2Ini.Siparvvalor)
                {
                    return Json("La hora de inicio 2 debe ser mayor a la hora de fin 1");
                }

                //actualización del parametro
                this.servParametro.UpdateSiParametroValor(paramH1Ini);
                this.servParametro.UpdateSiParametroValor(paramH1Fin);
                this.servParametro.UpdateSiParametroValor(paramH2Ini);
                this.servParametro.UpdateSiParametroValor(paramH2Fin);

                if (ConstantesParametro.EstadoActivo == estado)
                {
                    //actualizacion del estado de los demás parametros
                    List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPotenciaInductiva);
                    listaParam = listaParam.Where(x => x.Siparvcodi != idH1Ini).ToList();
                    listaParam = listaParam.Where(x => x.Siparvcodi != idH1Fin).ToList();
                    listaParam = listaParam.Where(x => x.Siparvcodi != idH2Ini).ToList();
                    listaParam = listaParam.Where(x => x.Siparvcodi != idH2Fin).ToList();
                    listaParam = listaParam.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo).ToList();

                    foreach (var reg in listaParam)
                    {
                        reg.Siparveliminado = ConstantesParametro.EstadoBaja;
                        reg.Siparvusumodificacion = base.UserName;
                        reg.Siparvfecmodificacion = DateTime.Now;
                        this.servParametro.UpdateSiParametroValor(reg);
                    }
                }

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Ver HPPotenciaActiva
        /// </summary>
        [HttpPost]
        public PartialViewResult VerRangoPotenciaInductiva(int idH1Ini, int idH1Fin, int idH2Ini, int idH2Fin)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO paramH1Ini = this.servParametro.GetByIdSiParametroValor(idH1Ini);
            SiParametroValorDTO paramH1Fin = this.servParametro.GetByIdSiParametroValor(idH1Fin);
            SiParametroValorDTO paramH2Ini = this.servParametro.GetByIdSiParametroValor(idH2Ini);
            SiParametroValorDTO paramH2Fin = this.servParametro.GetByIdSiParametroValor(idH2Fin);

            ParametroRangoPotenciaInductiva hp = this.servParametro.GetParametroRangoPotenciaInductiva(paramH1Ini, paramH1Fin, paramH2Ini, paramH2Fin, this.ListaEstado, ParametrosFormato.ResolucionCuartoHora);
            model.RangoPotenciaInductiva = hp;

            return PartialView(model);
        }
        #endregion

        #region Parámetros de Rango de Periodos para emisión de Reporte de Máxima Demanda
        /// <summary>
        /// Index RangoPeriodoHP
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexRangoPeriodoHP()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ParametroModel model = new ParametroModel();

            return View(model);
        }

        /// <summary>
        /// Listado RangoPeriodoHP
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoRangoPeriodoHP()
        {
            ParametroModel model = new ParametroModel();

            GenerarRangoPeriodoHP(model);

            return PartialView(model);
        }

        /// <summary>
        /// Generar RangoPeriodoHP
        /// </summary>
        /// <param name="model"></param>
        private void GenerarRangoPeriodoHP(ParametroModel model)
        {
            List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            model.ListaRangoPeriodoHP = this.servParametro.GetListaParametroRangoPeriodoHP(listaParam, this.ListaEstado);
            model.ListaRangoPeriodoHP = model.ListaRangoPeriodoHP.OrderBy(x => x.Estado).ThenByDescending(x => x.FechaInicio).ToList();
        }

        /// <summary>
        /// Registrar Rango de Periodos de Hora Punta
        /// </summary>
        [HttpPost]
        public PartialViewResult NuevoRangoPeriodoHP()
        {
            ParametroModel model = new ParametroModel();
            model.RangoPeriodoHP = new ParametroRangoPeriodoHP();
            model.RangoPeriodoHP.FechaFormatoInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.RangoPeriodoHP.FechaFormatoFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            return PartialView(model);
        }

        /// <summary>
        /// Registrar RangoPeriodoHP
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="normativa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarRangoPeriodoHP(string fechaInicio, string fechaFin, string normativa)
        {
            try
            {
                DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                SiParametroValorDTO param = new SiParametroValorDTO();
                param.Siparcodi = ConstantesParametro.IdParametroRangoPeriodoHP;
                param.Siparvfechainicial = fechaInicial;
                param.Siparvfechafinal = fechaFinal;
                param.Siparvnota = normativa != null ? normativa.Trim() : string.Empty;
                param.Siparveliminado = ConstantesParametro.EstadoActivo;
                param.Siparvusucreacion = base.UserName;
                param.Siparvfeccreacion = DateTime.Now;

                //validacion
                if (param.Siparvfechainicial.Value.Date >= param.Siparvfechafinal.Value.Date)
                {
                    return Json("La Fecha Inicio debe ser menor que la Fecha Fin");
                }

                //validacion de existencia
                var listaFecha = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
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
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Editar Rango de Periodos de Hora Punta
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarRangoPeriodoHP(int idRango)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO param = this.servParametro.GetByIdSiParametroValor(idRango);

            ParametroRangoPeriodoHP rango = this.servParametro.GetParametroRangoPeriodoHP(param, this.ListaEstado);
            model.RangoPeriodoHP = rango;
            model.ListaEstado = this.ListaEstado;

            return PartialView(model);
        }

        /// <summary>
        /// Actualizar RangoPeriodoHP
        /// </summary>
        /// <param name="idRango"></param>
        /// <param name="estado"></param>
        /// <param name="normativa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarRangoPeriodoHP(int idRango, string estado, string normativa)
        {
            try
            {
                SiParametroValorDTO param = this.servParametro.GetByIdSiParametroValor(idRango);
                param.Siparveliminado = estado;
                param.Siparvusumodificacion = base.UserName;
                param.Siparvfecmodificacion = DateTime.Now;
                param.Siparvnota = normativa != null ? normativa.Trim() : string.Empty;

                this.servParametro.UpdateSiParametroValor(param);

                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Ver Rango de Periodos de Hora Punta
        /// </summary>
        [HttpPost]
        public PartialViewResult VerRangoPeriodoHP(int idRango)
        {
            ParametroModel model = new ParametroModel();

            SiParametroValorDTO param = this.servParametro.GetByIdSiParametroValor(idRango);

            ParametroRangoPeriodoHP rango = this.servParametro.GetParametroRangoPeriodoHP(param, this.ListaEstado);
            model.RangoPeriodoHP = rango;

            return PartialView(model);
        }
        #endregion
    }
}
