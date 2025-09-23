using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Yupana.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Yupana;
using COES.Servicios.Aplicacion.Yupana.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Yupana.Controllers
{
    public class ConfiguracionController : BaseController
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
        public JsonResult ObtenerRecurso(int rescfgcodi, int tipo)
        {
            CondicionInicialModel model = new CondicionInicialModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.Recurso = new PrRestricCfgDTO();
                if (tipo == ConstantesYupana.TipoModoOp)
                    model.ListaModosOpCOES = servicio.ListarModosOperacionCOES(ConstantesYupana.catecodiModo);
                else
                {
                    if (tipo == ConstantesYupana.TipoHidro)
                        model.ListaUnidadesCOES = servicio.ListarUnidadesHidroCOES(ConstantesYupana.strCategoriaHidro);
                    else
                        model.ListaRerCOES = servicio.ListarUnidadesRerCOES(ConstantesYupana.strCategoriaRer);
                }

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
        public JsonResult GuardarRecurso(int rescfgcodi, int restipcodi, int codigo, string nombre)
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
                    model.Recurso.Restipcodi = restipcodi;
                    model.Recurso.Rescfgestado = ConstantesAppServicio.Activo;
                    model.Recurso.Rescfgusucreacion = base.UserName;
                    model.Recurso.Rescfgfeccreacion = DateTime.Now;

                    // Graba los cambio en la BD
                    idRecurso = servicio.SavePrRestricCfg(model.Recurso);

                    //GUARDAR RELACIÓN
                    List<PrRestricCfgdetDTO> listaRelaciones = new List<PrRestricCfgdetDTO>();
                    PrRestricCfgdetDTO relacion = new PrRestricCfgdetDTO();
                    relacion.Rescfgcodi = codigo;
                    listaRelaciones.Add(relacion);
                    servicio.GuardarRelaciones(idRecurso, listaRelaciones, base.UserName);
                }
                else
                {
                    model.Recurso = listadoRecursos.Find(x => x.Rescfgcodi == rescfgcodi);
                    //capturar valores
                    model.Recurso.Rescfgnombre = nombre ?? "";
                    model.Recurso.Rescfgusumodificacion = base.UserName;
                    model.Recurso.Rescfgfecmodificacion = DateTime.Now;

                    // Graba los cambio en la BD
                    servicio.UpdatePrRestricCfg(model.Recurso);
                    idRecurso = model.Recurso.Rescfgcodi;

                    //ACTUALIZAR EL CÓDIGO DE RELACIÓN
                    var entidad = servicio.GetByIdPrRestricCfgdet(idRecurso);
                    entidad.Resdetcodigo = codigo;
                    servicio.UpdatePrRestricCfgdet(entidad);
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

    }
}
