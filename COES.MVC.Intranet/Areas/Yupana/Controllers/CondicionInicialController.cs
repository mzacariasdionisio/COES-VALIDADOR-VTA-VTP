using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Yupana.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Yupana;
using COES.Servicios.Aplicacion.Yupana.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using static COES.Servicios.Aplicacion.Yupana.Helper.ConstantesYupana;

namespace COES.MVC.Intranet.Areas.Yupana.Controllers
{
    public class CondicionInicialController : BaseController
    {
        readonly YupanaAppServicio servicio = new YupanaAppServicio();

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }


        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index()
        {
            CondicionInicialModel model = new CondicionInicialModel();

            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, '0'));
            }
            //model.ListaSemanas = semanas;
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);

            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }
        public PartialViewResult CargarSemanas(string idAnho)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            List<TipoInformacion> entitys = new List<TipoInformacion>();
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                TipoInformacion reg = new TipoInformacion();
                reg.IdTipoInfo = i;
                reg.NombreTipoInfo = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemana = entitys;
            return PartialView(model);
        }

        /// <summary>
        /// Carga la información de condiciones iniciales
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarCondicionesIniciales(int formato, string fecha, string semana)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            DateTime fechaProceso = new DateTime();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                int formatoVentana = servicio.ObtenerFormatoSegunVentana(formato, ConstantesYupana.TipoCondicionesIniciales);

                if (formatoVentana != ConstantesYupana.FormatoCondInicialSemanal) // diario reprograma
                    fechaProceso = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                else
                    fechaProceso = EPDate.GetFechaIniPeriodo(2, string.Empty, semana, fecha, Constantes.FormatoFecha);

                DatoCondicionesIniciales data = servicio.ObtenerDatosCondicionesIniciales(formatoVentana, fechaProceso);

                model.DataCondicionInicial = data;
                model.IdEnvio = data.IdEnvio;
                //OBTENER ENVIO
                var envio = servicio.GetByIdPrRestricEnvio(data.IdEnvio);
                model.FechaProceso = envio != null ? envio.Resenvfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";

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
        /// Guarda informacion de condiciones iniciales
        /// </summary>
        /// <param name="formatoReal"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="datosAGuardar"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarCondicionesIniciales(int formato, string fecha, string semana, DatoCondicionesIniciales datosAGuardar)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            DateTime fechaProceso = new DateTime();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                int formatoVentana = servicio.ObtenerFormatoSegunVentana(formato, ConstantesYupana.TipoCondicionesIniciales);

                if (formatoVentana != ConstantesYupana.FormatoCondInicialSemanal) // diario reprograma
                    fechaProceso = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                else
                    fechaProceso = EPDate.GetFechaIniPeriodo(2, string.Empty, semana, fecha, Constantes.FormatoFecha);

                if (datosAGuardar != null)
                {
                    var codeEnvio = servicio.EnviarDatosCI(formatoVentana, fechaProceso, datosAGuardar, base.UserName);
                    model.IdEnvio = codeEnvio;
                    model.FechaProceso = fechaProceso.ToString(ConstantesAppServicio.FormatoFecha);
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        #region Configuración

        public ActionResult ConfiguracionIndex()
        {
            CondicionInicialModel model = new CondicionInicialModel();
            return View(model);
        }

        /// <summary>
        /// Obtener Lista de recursos por tipo
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEntidadXTipo(int tipo)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.Recursos = servicio.ObtenerRecursos(tipo);
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
        /// Obtener datos de un recurso
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerRecurso(int rescfgcodi)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.Recurso = new PrRestricCfgDTO();
                model.ListaModosOpCOES = servicio.ListarModosOperacionCOES(ConstantesYupana.catecodiModo);

                if (rescfgcodi > 0)
                    model.Recurso = servicio.GetByIdPrRestricCfg(rescfgcodi);

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
        /// Guardar recurso
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <param name="restipcodi"></param>
        /// <param name="codmodo"></param>
        /// <param name="nombre"></param>
        /// <param name="rescfges"></param>
        /// <param name="rescfgfs"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarRecurso(int rescfgcodi, int restipcodi, string nombre, string rescfges, string rescfgfs)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<PrRestricCfgDTO> listadoRecursos = servicio.GetByCriteriaPrRestricCfgs(restipcodi.ToString());
                listadoRecursos = listadoRecursos.Where(x => x.Rescfgestado == ConstantesAppServicio.Activo).ToList();
                var listadoValid = listadoRecursos.Where(x => x.Restipcodi == restipcodi && x.Rescfgcodi != rescfgcodi).ToList();

                //Valido que no exista duplicados
                PrRestricCfgDTO duplicado = listadoValid.Find(x => x.Rescfgnombre.Trim() == nombre.Trim());
                if (duplicado != null)
                    throw new ArgumentException("Existe recurso con el mismo nombre.");

                int idRecurso = 0;
                if (rescfgcodi == 0)
                {
                    model.Recurso.Rescfgnombre = nombre ?? "";
                    model.Recurso.Rescfges = decimal.Parse(rescfges);
                    model.Recurso.Rescfgfs = decimal.Parse(rescfgfs);
                    model.Recurso.Restipcodi = restipcodi;
                    model.Recurso.Rescfgestado = ConstantesAppServicio.Activo;
                    model.Recurso.Rescfgusucreacion = base.UserName;
                    model.Recurso.Rescfgfeccreacion = DateTime.Now;

                    // Graba los cambio en la BD
                    idRecurso = servicio.SavePrRestricCfg(model.Recurso);
                }
                else
                {
                    model.Recurso = listadoRecursos.Find(x => x.Rescfgcodi == rescfgcodi);
                    //capturar valores
                    model.Recurso.Rescfgnombre = nombre ?? "";
                    model.Recurso.Rescfges = decimal.Parse(rescfges);
                    model.Recurso.Rescfgfs = decimal.Parse(rescfgfs);
                    model.Recurso.Rescfgusumodificacion = base.UserName;
                    model.Recurso.Rescfgfecmodificacion = DateTime.Now;

                    // Graba los cambio en la BD
                    servicio.UpdatePrRestricCfg(model.Recurso);
                    idRecurso = model.Recurso.Rescfgcodi;
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
        /// Eliminar recurso
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarRecurso(int rescfgcodi)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            try
            {
                base.ValidarSesionJsonResult();
                string usuario = base.UserName;
                servicio.ActualizarEstadoBaja(rescfgcodi, usuario);

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

        [HttpPost]
        public PartialViewResult CargarRelaciones(int rescfgcodi)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);
                //model.Recursos = servicio.ObtenerRelaciones(rescfgcodi);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Cragar y listar relaciones
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarModosRelacionados(int rescfgcodi)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                // modos de operación
                List<PrRestricCfgDTO> listaModosOperacion = servicio.GetByCriteriaPrRestricCfgs(ConstantesYupana.TipoModoOp.ToString());
                listaModosOperacion = listaModosOperacion.Where(x => x.Rescfgestado == ConstantesAppServicio.Activo).ToList();
                model.ModosGenerales = servicio.ListarModosGeneral(rescfgcodi, listaModosOperacion);
                model.Recursos = servicio.ObtenerRelaciones(rescfgcodi, listaModosOperacion);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Guardar relaciones
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <param name="listaRelaciones"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarRelaciones(int rescfgcodi, List<PrRestricCfgdetDTO> listaRelaciones)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                servicio.GuardarRelaciones(rescfgcodi, listaRelaciones, base.UserName);

                model.Resultado = "1";

            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        #endregion

    }
}
