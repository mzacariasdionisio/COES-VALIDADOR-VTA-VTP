using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Areas.Resarcimientos.Models;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.ResarcimientoNTCSE.Helper;


namespace COES.MVC.Intranet.Areas.Resarcimientos.Controllers
{
    public class ConfiguracionController : Controller
    {
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
         public ConfiguracionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        /// <summary>
        /// Instanciamiento de web Servis
        /// </summary>
        SeguridadServicioClient servicio = new SeguridadServicioClient();

        /// <summary>
        /// Instanciamiento de Servicios de Aplicacion
        /// </summary>
        ResarcimientoNTCSEAppServicio ntcse = new ResarcimientoNTCSEAppServicio();

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Valida Session true
        /// </summary>
        public void ValidarSessionUsuarios()
        {
            HttpCookie cookie = Request.Cookies[DatosSesion.InicioSesion];

            if (cookie != null)
            {
                if (cookie[DatosSesion.SesionUsuario] != null)
                {
                    Session[DatosSesion.SesionMapa] = Constantes.NodoPrincipal;
                    Session[DatosSesion.SesionUsuario] = this.servicio.ObtenerUsuarioPorLogin(cookie[DatosSesion.SesionUsuario].ToString());
                    FormsAuthentication.SetAuthCookie(cookie[DatosSesion.SesionUsuario].ToString(), false);

                }
            }
            else
            {
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    Session[DatosSesion.SesionMapa] = Constantes.NodoPrincipal;
                }
                else
                {
                    Response.Redirect("~/");
                }
            }
        }

        #region MÉTODOS CONFIGURACION

        /// <summary>
        /// Muestra Ventana de Configuración de Parametro
        /// </summary>
        public ActionResult ConfParam()
        {
            ValidarSessionUsuarios();
			try
            {
            List<ConfiguraionModel> a = new List<ConfiguraionModel>();
            a = this.ListarConfiguracion(ntcse.ListRntConfiguracions());

            return View(a);
			}
            catch (Exception e) {
                Log.Error("Error" , e);
                return new HttpStatusCodeResult(404 , ConstantesResarcimiento.ErrorDeSistema);
            }
        }

        /// <summary>
        /// Muestra Ventana de Nueva Congifuración de Parametro
        /// </summary>
        public ActionResult ConfParamNuevo()
        {
            ValidarSessionUsuarios();

            return View();
        }

        /// <summary>
        /// Muestra Ventana de Nueva Congifuración de Parametro
        /// </summary>
        public JsonResult ObtenerParametro(FormCollection colection)
        {
            ValidarSessionUsuarios();

            string atributo = colection["atributo"];
            List<RntConfiguracionDTO> List = new List<RntConfiguracionDTO>();
            List = ntcse.GetComboParametro(atributo);

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validar Tension si existe
        /// </summary>
        [HttpPost]
        public JsonResult ValidarConfiguracion(FormCollection collection)
        {
            try
            {
                int registro = Convert.ToInt32(collection["registro"]);
                string valor = Convert.ToString(collection["valor"]);

                int tension = -1;
                List<ConfiguraionModel> listPer = this.ListarConfiguracion(ntcse.ListRntConfiguracions());
                foreach (ConfiguraionModel item in listPer)
                {
                    if (registro != 0)
                    {
                        if (item.Configcodi == registro && item.Confvalor == valor)
                        {
                            return Json(1);
                        }
                        else if (item.Confvalor == valor)
                        {
                            return Json(0);
                        }
                    }
                    else
                    {
                        if (item.Confvalor == valor)
                        {
                            return Json(0);
                        }
                    }
                }
                if (registro != 0)
                {
                    tension = registro;
                }
                //getByPeriodo
                int catper = 0;
                List<RntPeriodoDTO> lper = new List<RntPeriodoDTO>();
                lper = ntcse.GetByCriteriaRntPeriodos();

                foreach (RntPeriodoDTO item in lper)
                {

                    if (item.PerdSemestre == valor)
                    {
                        catper = item.PeriodoCodi;
                        break;
                    }

                }
                List<RntRegRechazoCargaDTO> opcionesRrc = ntcse.GetByCriteriaRntRegRechazoCargas("0", catper, 0, 0, 0, 0, DateTime.MinValue);
                List<RntRegPuntoEntregaDTO> opcionesRpe = ntcse.GetByCriteriaRntRegPuntoEntregas("0", catper, 0, 0, 0, 0, DateTime.MinValue);
                if (opcionesRrc.Count > 0 || opcionesRpe.Count > 0)
                {
                    return Json(0);
                }
                else
                {
                    return Json(1);
                }
            }
            catch (Exception e)
            {
                Log.Error("Error", e);
                return Json(0);
            }
        }


        /// <summary>
        /// Permite realizar el Registro y Modificacion de Configuración de Parametro
        /// </summary>
        [HttpPost]
        public string MantenimientoConfiguracion(FormCollection collection)
        {
            ValidarSessionUsuarios();
			try
            {
            RntConfiguracionDTO bdto = new RntConfiguracionDTO();
            bdto.ConfAtributo = Convert.ToString(collection["atributo"]);
            bdto.ConfParametro = Convert.ToString(collection["parametro"]);
            bdto.ConfValor = Convert.ToString(collection["valor"]);

            string accion = collection["accion"];
            int key = Convert.ToInt32(collection["registro"]);
            if (accion == "Editar")
            {
                bdto.ConfigCodi = key;
                bdto.ConfUsuarioUpdate = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                bdto.ConfFechaUpdate = DateTime.Now;
                ntcse.UpdateRntConfiguracion(bdto);
                //Auditoria
                return ConstantesResarcimiento.Modificar;
            }
            else
            {
                bdto.ConfigCodi = 0;
                bdto.ConfUsuarioCreacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                bdto.ConfFechaCreacion = DateTime.Now;
                ntcse.SaveRntConfiguracion(bdto);
                return ConstantesResarcimiento.Registrar;
            }
			}catch(Exception e){
                Log.Error("Error", e);
                return ConstantesResarcimiento.ErrorDeSistema;
            }
        }

        /// <summary>
        /// Permite Eliminar el Registro de Configuración de Parametro
        /// </summary>
        [HttpPost]
        public String EliminarConfigParametro(FormCollection collection)
        {
            ValidarSessionUsuarios();
            try
            {
			RntConfiguracionDTO conf = new RntConfiguracionDTO();

            conf.ConfigCodi =Convert.ToInt32(collection["registro"]);
            conf.ConfUsuarioUpdate = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
            ntcse.DeleteRntConfiguracion(conf);
            string result = ConstantesResarcimiento.Eliminar;
            return result;
			}catch(Exception e){
                Log.Error("Error" , e);
                return ConstantesResarcimiento.ErrorDeSistema;
            }
        }

        /// <summary>
        /// Muestra Ventana de Editar Configuración de Parametro
        /// </summary>
        [HttpPost]
        public ActionResult EditarConfigParam(FormCollection collection)
        {
            ValidarSessionUsuarios();
            try
            {
			int registro = Convert.ToInt32(collection["registro"]);
            ConfiguraionModel b = new ConfiguraionModel();

                b = this.ListarConfiguracionREg(registro);
                return View(b);
            }
            catch (Exception e){
                Log.Error("Error" , e);
                return new HttpStatusCodeResult(404, ConstantesResarcimiento.ErrorDeSistema);
            }
        }

        #endregion

        #region LISTAR

        /// <summary>
        /// Metodo de Listar por Configuración de Parametro
        /// </summary>
        private List<ConfiguraionModel> ListarConfiguracion(List<RntConfiguracionDTO> data)
        {
            List<ConfiguraionModel> list = new List<ConfiguraionModel>();
            try
            {
			if (User != null)
            {
                Log.Info("iniciando Tensión");
                List<RntConfiguracionDTO> opciones = data;
                foreach (RntConfiguracionDTO item in opciones)
                {
                    if (item.ConfAtributo != ConstantesResarcimiento.Atributo)
                    {
                    list.Add(new ConfiguraionModel()
                    {
                        Confatributo = item.ConfAtributo,
                        Confparametro = item.ConfParametro,
                        Confvalor = item.ConfValor,
                        Configcodi = item.ConfigCodi,
                    });
                    }
                }
            }

			}
            catch (Exception e){
                Log.Error("Error", e);
            }
            return list;
        }

        /// <summary>
        /// Metodo de Listar por Registros de Configuración de Parametro
        /// </summary>
        public ConfiguraionModel ListarConfiguracionREg(int registro)
        {

            ConfiguraionModel b = new ConfiguraionModel();
            try
            {
			RntConfiguracionDTO c = new RntConfiguracionDTO();
            c = ntcse.GetByIdRntConfiguracion(registro);
            b.Configcodi = c.ConfigCodi;
            b.Confatributo = c.ConfAtributo;
            b.Confparametro = c.ConfParametro;
            b.Confvalor = c.ConfValor;

			}
            catch(Exception e) {
                Log.Error("Error" ,e);
            }
            return b;
        }
        
        #endregion
    }
}
