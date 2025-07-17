using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Siosein.Helper;
using COES.MVC.Intranet.Areas.Siosein.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.SIOSEIN;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace COES.MVC.Intranet.Areas.Siosein.Controllers
{
    public class TablasPrieController : BaseController
    {
        //
        // GET: /Siosein/TablasPrie/
        SIOSEINAppServicio servicio = new SIOSEINAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(TablasPrieController));
        private static string NameController = "TablasPrieController";
        private static List<EstadoModel> ListaEstadoSistemaA = new List<EstadoModel>();

        /// <summary>
        /// Protected de log de errores page
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
        /// listado ventos del controller
        /// </summary>
        public TablasPrieController()
        {
            ListaEstadoSistemaA = new List<EstadoModel>();
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "0", EstadoDescripcion = "NO" });
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "SÍ" });
        }

        /// <summary>
        /// index de mantenimiento de tablas PRIE
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SioseinModel model = new SioseinModel();
            return View();
        }

        /// <summary>
        /// partial html de lista de las tablas PRIE
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Lista()
        {
            SioseinModel model = new SioseinModel();

            model.ListaTablasPrie = servicio.ListSioTablapries().OrderBy(x => x.Tpriecodi).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Devuelve Vista para registrar datos de la tabla Prie, ya sea nueva o para modificar
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ViewTablaPrie(int idTabla)
        {
            SioseinModel model = new SioseinModel();
            model.TablaPrie = new SioTablaprieDTO();
            model.TablaPrie.Tpriecodi = idTabla;
            model.TablaPrie.Tpriefechaplazo = DateTime.Now;
            if (idTabla != -1) //Carga valores iniciales si es modo editar
            {
                var obj = servicio.GetByIdSioTablaprie(idTabla);
                model.TablaPrie.Tpriedscripcion = obj.Tpriedscripcion;
                model.TablaPrie.Tprieabrev = obj.Tprieabrev;
                model.TablaPrie.Areacodi = obj.Areacodi;
                model.TablaPrie.Tpriefechaplazo = obj.Tpriefechaplazo;
                model.TablaPrie.Tpriecodtablaosig = obj.Tpriecodtablaosig;
            }

            model.ListaAreas = servicio.ListFwAreas().Where(x => x.Compcode == 1).ToList();
            model.ListaCampos = servicio.GetByCriteriaSioColumnapries(idTabla);

            var resultHtml= HttpUtility.HtmlDecode(this.RenderPartialViewToString("ViewTablaPrie", model));
            return Json(resultHtml);
        }

        /// <summary>
        /// Agregar Punto de Medicion en BD
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(List<SioColumnaprieDTO> data, string descripcionTabla, int AreaInvolucrada, string plazoentrega, int idTabla, string abreviatura, string tpriecodtablaosig)
        {
            SioTablaprieDTO ObjTablaPrie = new SioTablaprieDTO();
            int resultado = 1;
            int tPriecodi = idTabla;
            try
            {
                DateTime fechaPlazo = DateTime.Now;
                if (plazoentrega != null)
                {
                    fechaPlazo = DateTime.ParseExact(plazoentrega, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                if (idTabla == -1)
                {// nueva tabla
                    ObjTablaPrie.Tpriedscripcion = descripcionTabla;
                    ObjTablaPrie.Areacodi = AreaInvolucrada;
                    ObjTablaPrie.Tpriefechaplazo = fechaPlazo;
                    ObjTablaPrie.Tprieabrev = abreviatura;
                    ObjTablaPrie.Tprieusucreacion = User.Identity.Name;
                    ObjTablaPrie.Tprieusumodificacion = User.Identity.Name;
                    ObjTablaPrie.Tprieffeccreacion = DateTime.Now;
                    ObjTablaPrie.Tpriefecmodificacion = DateTime.Now;
                    ObjTablaPrie.Tpriecodtablaosig = tpriecodtablaosig;

                    //guardamos el objeto TablaPrie
                    tPriecodi = servicio.SaveSioTablaprie(ObjTablaPrie);
                }
                else // modificación de tablaPrie
                {
                    ObjTablaPrie.Tpriecodi = idTabla;
                    ObjTablaPrie.Tpriedscripcion = descripcionTabla;
                    ObjTablaPrie.Areacodi = AreaInvolucrada;
                    ObjTablaPrie.Tpriefechaplazo = fechaPlazo;
                    ObjTablaPrie.Tprieabrev = abreviatura;
                    ObjTablaPrie.Tprieusumodificacion = User.Identity.Name;
                    ObjTablaPrie.Tpriefecmodificacion = DateTime.Now;
                    ObjTablaPrie.Tpriecodtablaosig = tpriecodtablaosig;
                    // modificamos la tabla Prie
                    servicio.UpdateSioTablaprie(ObjTablaPrie);
                }
                SioColumnaprieDTO ObjCampo = new SioColumnaprieDTO();
                if (data != null)
                {
                    foreach (var obj in data)
                    {
                        if (obj.opCrud == 1)
                        {// nuevo campo
                            ObjCampo.Cprienombre = obj.Cprienombre;
                            ObjCampo.Cpriedescripcion = obj.Cpriedescripcion;
                            ObjCampo.Cprietipo = obj.Cprietipo;
                            ObjCampo.Cprielong1 = obj.Cprielong1;
                            ObjCampo.Cprielong2 = obj.Cprielong2;
                            ObjCampo.Tpriecodi = tPriecodi;
                            ObjCampo.Cpriefeccreacion = DateTime.Now;
                            ObjCampo.Cpriefecmodificacion = DateTime.Now;
                            ObjCampo.Cprieusucreacion = User.Identity.Name;
                            ObjCampo.Cprieusumodificacion = User.Identity.Name;

                            servicio.SaveSioColumnaprie(ObjCampo);
                        }

                        if (obj.opCrud == -1) // Eliminar
                        {
                            servicio.DeleteSioColumnaprie(tPriecodi);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Log.Error(NameController, e);
                resultado = 0; 
            }

            return Json(resultado);
        }


    }
}