using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.Monitoreo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Monitoreo;
using log4net;
using System;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Monitoreo.Controllers
{
    [ValidarSesion]
    public class RelacionController : BaseController
    {
        IEODAppServicio servIEOD = new IEODAppServicio();
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();
        MonitoreoAppServicio servMonitoreo = new MonitoreoAppServicio();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(RelacionController));
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

        //
        // GET: /Monitoreo/Relacion/
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            RelacionModel model = new RelacionModel();

            model.ListaEmpresas = this.servIEOD.ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamilias);
            model.ListaCategoria = this.servFictec.ListarCategoriaGrupoXCatecodi(ConstantesMonitoreo.CatecodiRelacionGrupoDespachoBarraProg);

            return PartialView(model);
        }

        /// <summary>
        /// Lista de grupos y su relacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoCentral"></param>
        /// <returns></returns>
        public JsonResult ListadoRelacion(string idEmpresa, int idTipoCentral)
        {
            RelacionModel model = new RelacionModel();
            try
            {
                this.ValidarSesionJsonResult();

                string catecodis = idTipoCentral == -2 ? ConstantesMonitoreo.CatecodiRelacionGrupoDespachoBarraProg : idTipoCentral.ToString();
                string url = Url.Content("~/");
                model.Resultado = this.servMonitoreo.ReporteRelacionGrupoBarraProgHtml(idEmpresa, catecodis, url);
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
        /// Listar las barras y el grupo para nueva relacion
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public JsonResult ListarBarraYGrupo(int grupocodi)
        {
            RelacionModel model = new RelacionModel();
            model.LisConfigBarr = this.servMonitoreo.ListarBarraProgramacion(false);
            model.ObjBarr = this.servMonitoreo.ObjGrupoUnidad(grupocodi);
            model.Resultado = "1";
            return Json(model);
        }

        /// <summary>
        /// Guardar la relacion
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idBarra"></param>
        /// <returns></returns>
        public JsonResult SaveRelacionBarraDespacho(int idGrupo, int idBarra)
        {
            RelacionModel model = new RelacionModel();
            try
            {
                this.ValidarSesionJsonResult();

                PrGrupoxcnfbarDTO reg = this.servMonitoreo.GetByGrupocodiPrGrupoxcnfbar(idGrupo);

                if (reg == null)
                {
                    PrGrupoxcnfbarDTO obj = new PrGrupoxcnfbarDTO();
                    obj.Grupocodi = idGrupo;
                    obj.Cnfbarcodi = idBarra;
                    obj.Grcnfbestado = ConstantesMonitoreo.PrGrupoBarraprogEstadoActivo;
                    obj.Grcnfbusucreacion = User.Identity.Name;
                    obj.Grcnfbfeccreacion = DateTime.Now;

                    this.servMonitoreo.SavePrGrupoxcnfbar(obj);

                    model.Resultado = "1";
                }
                else
                {
                    if (reg.Grcnfbestado == ConstantesMonitoreo.PrGrupoBarraprogEstadoActivo) //ya registrado
                    {
                        model.Resultado = "0";
                    }
                    else
                    {
                        reg.Cnfbarcodi = idBarra;
                        reg.Grcnfbfecmodificacion = DateTime.Now;
                        reg.Grcnfbusumodificacion = User.Identity.Name;
                        reg.Grcnfbestado = ConstantesMonitoreo.PrGrupoBarraprogEstadoActivo;

                        this.servMonitoreo.UpdatePrGrupoxcnfbar(reg);
                    }
                }
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
        /// Eliminar Relacion
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        public JsonResult DeleteRelacion(int idGrupo, int idRelacion)
        {
            RelacionModel model = new RelacionModel();
            try
            {
                this.ValidarSesionJsonResult();

                bool existe = this.servMonitoreo.ExisteRelacionGrupoBarraProg(idGrupo);
                if (existe)
                {
                    PrGrupoxcnfbarDTO reg = this.servMonitoreo.GetByIdPrGrupoxcnfbar(idRelacion);
                    reg.Grcnfbfecmodificacion = DateTime.Now;
                    reg.Grcnfbusumodificacion = User.Identity.Name;

                    this.servMonitoreo.DeletePrGrupoxcnfbar(reg);

                    model.Resultado = "1";
                }
                else
                {
                    model.Resultado = "0";
                }
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

    }
}
