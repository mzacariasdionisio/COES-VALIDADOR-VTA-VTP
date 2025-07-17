using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Reflection;

using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    // ASSETEC 2019-11
    public class ModeloController : BaseController
    {
        TransferenciasAppServicio servicioModelo = new TransferenciasAppServicio();
        EmpresaAppServicio empresaAppServicio = new EmpresaAppServicio();
        BarraAppServicio barraAppServicio = new BarraAppServicio();

        #region Manejador de Exepciones
        public ModeloController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

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

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        #endregion


        // GET: Transferencias/Modelo
        /// <summary>
        /// Cargar vista de configuración de formato de modelos
        /// </summary>        
        /// <returns>View</returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ModeloModel modelo = new ModeloModel();
            modelo.ListaModelos = servicioModelo.ListarTrnModelo();
            modelo.ListaEmpresas = empresaAppServicio.ListaInterCoReSoGen();            

            return View(modelo);
        }

        #region Modelos
        /// <summary>
        /// Cargar listado de modelos (Vista parcial)
        /// </summary>        
        /// <returns>PartialView</returns>
        [HttpPost]
        public ActionResult ListaModelo()
        {
            base.ValidarSesionUsuario();

            ModeloModel modelo = new ModeloModel();
            modelo.ListaModelos = servicioModelo.ListarTrnModelo();

            return PartialView(modelo);
        }

        /// <summary>
        /// Muestra formulario popup de registro de modelos
        /// </summary>               
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult PopupModelo()
        {
            base.ValidarSesionJsonResult();

            ModeloModel modelo = new ModeloModel();
            return Json(modelo);
        }

        /// <summary>
        /// Grabar modelo
        /// </summary> 
        /// <param name="idModelo">Id de Modelo</param>
        /// <param name="idCoordinador">Id de Coordinador</param>
        /// <param name="nombreModelo">Nombre de Modelo</param>
        /// <param name="fechaRegistro">Fecha de Registro</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GrabarModelo(string idModelo, string nombreModelo, string idCoordinador)
        {
            base.ValidarSesionJsonResult();

            ModeloModel modelo = new ModeloModel();
            int rspta = -1;

            modelo.EntidadModelo = new TrnModeloDTO();

            modelo.EntidadModelo.TrnModCodi = Convert.ToInt32(idModelo);
            modelo.EntidadModelo.TrnModNombre = nombreModelo;
            modelo.EntidadModelo.EmprCodi = Convert.ToInt32(idCoordinador);
            

            if (idModelo == "0") // Registro
            {
                modelo.EntidadModelo.TrnModUseIns = User.Identity.Name;
                modelo.EntidadModelo.TrnModFecIns = DateTime.Now;

                rspta = servicioModelo.SaveTrnModelo(modelo.EntidadModelo);
            }
            else // Modificacion
            {
                modelo.EntidadModelo.TrnModUseAct = User.Identity.Name;
                modelo.EntidadModelo.TrnModFecAct = DateTime.Now;

                rspta = servicioModelo.UpdateTrnModelo(modelo.EntidadModelo);
            }

            return Json(rspta);
        }

        /// <summary>
        /// Eliminar modelo
        /// </summary>      
        /// <param name="id">Id de Modelo</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult EliminarModelo(int idModelo)
        {
            base.ValidarSesionJsonResult();

            ModeloModel modelo = new ModeloModel();
            int rspta = -1;

            rspta = servicioModelo.DeleteTrnModelo(idModelo);

            return Json(modelo);
        }
        #endregion
        
        #region Códigos de Retiro
        /// <summary>
        /// Combo de modelos con jquery
        /// </summary>           
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult ListarComboModelos()
        {
            base.ValidarSesionJsonResult();

            ModeloModel modelo = new ModeloModel();
            modelo.ListaModelos = servicioModelo.ListarComboTrnModelo();

            return Json(modelo);
        }

        /// <summary>
        /// Consultar y carga listado de codigos de retiro por modelo  (Vista parcial)
        /// </summary>     
        /// <param name="idModelo">Id de Modelo</param>
        /// <returns>PartialView</returns>
        [HttpPost]
        public ActionResult ListaCodigoRetiroModelo(string idModelo)
        {
            base.ValidarSesionUsuario();

            ModeloModel modelo = new ModeloModel();
            modelo.ListaModelosRetiro = servicioModelo.ListarTrnModeloRetiro(Convert.ToInt32(idModelo));

            return PartialView(modelo);
        }

        /// <summary>
        /// Muestra formulario popup de registro de codigos de modelos de retiro
        /// </summary>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult PopupCodigoModeloRetiro()
        {
            base.ValidarSesionJsonResult();

            ModeloModel modelo = new ModeloModel();
            return Json(modelo);
        }

        /// <summary>
        /// Combo jquery de Barras
        /// </summary>         
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult ListarComboBarras()
        {
            base.ValidarSesionJsonResult();

            ModeloModel modelo = new ModeloModel();
            modelo.ListaBarras = servicioModelo.ListarComboBarras();

            return Json(modelo);
        }

        /// <summary>
        /// Combo jquery en cascada de Códigos de retiro
        /// </summary>
        /// <param name="idBarra">Id de Barra</param>       
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult ListarComboCodigosRetiro(int idBarra)
        {
            base.ValidarSesionJsonResult();

            ModeloModel modelo = new ModeloModel();
            modelo.ListaCodigosRetiro = servicioModelo.ListComboCodigoSolicitudRetiro(idBarra);

            return Json(modelo);
        }

        /// <summary>
        /// Grabar código de retiro
        /// </summary>        
        /// <param name="id">Id de Modelo</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GrabarCodigoRetiro(string idCodigoRetiro, string idModelo, string idBarra, string coresoCodi, string coresoCodigo)
        {
            base.ValidarSesionJsonResult();

            ModeloModel modelo = new ModeloModel();
            int rspta = -1;

            modelo.EntidadModeloRetiro = new TrnModeloRetiroDTO();

            modelo.EntidadModeloRetiro.TrnMreCodi = Convert.ToInt32(idCodigoRetiro);
            modelo.EntidadModeloRetiro.TrnModCodi = Convert.ToInt32(idModelo);
            modelo.EntidadModeloRetiro.BarrCodi = Convert.ToInt32(idBarra);
            modelo.EntidadModeloRetiro.CoresoCodi = Convert.ToInt32(coresoCodi);
            modelo.EntidadModeloRetiro.CoresoCodigo = coresoCodigo;            

            if (string.IsNullOrEmpty(idCodigoRetiro) || idCodigoRetiro == "0") // Registro
            {
                modelo.EntidadModeloRetiro.TrnModRetUseIns = User.Identity.Name;
                modelo.EntidadModeloRetiro.TrnModRetFecIns = DateTime.Now;

                rspta = servicioModelo.SaveTrnModeloRetiro(modelo.EntidadModeloRetiro);
            }
            else // Modificacion
            {
                modelo.EntidadModeloRetiro.TrnModRetUseAct = User.Identity.Name;
                modelo.EntidadModeloRetiro.TrnModRetFecAct = DateTime.Now;

                rspta = servicioModelo.UpdateTrnModeloRetiro(modelo.EntidadModeloRetiro);
            }

            return Json(rspta);
        }

        /// <summary>
        /// Eliminar codigo retiro
        /// </summary>      
        /// <param name="id">Id de Modelo</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult EliminarCodigoRetiro(int id)
        {
            base.ValidarSesionJsonResult();

            ModeloModel modelo = new ModeloModel();
            int rspta = -1;

            rspta = servicioModelo.DeleteTrnModeloRetiro(id);

            return Json(modelo);
        }

        /// <summary>
        /// Verificar si el modelo seleccionado tiene Códigos de retiro asociados
        /// </summary>      
        /// <param name="idModelo">Id de Modelo</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult TieneCodigosRetiroModelo(int idModelo)
        {
            base.ValidarSesionJsonResult();

            int rspta = -1;

            if(servicioModelo.TieneCodigosRetiroModelo(idModelo))
            {
                rspta = 1;
            }

            return Json(rspta);
        }
        #endregion
    }
}