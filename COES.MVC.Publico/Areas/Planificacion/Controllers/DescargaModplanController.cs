using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Planificacion.Models;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Planificacion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Planificacion.Controllers
{
    public class DescargaModplanController : Controller
    {
        // GET: Planificacion/DescargaModplan
        //public ActionResult Index()
        //{
        //    string s = System.Web.HttpContext.Current.Request.RawUrl;
        //    string varib = s.Replace("/planificacion/descargamodplan/?id=", "");
        //    string url = Encriptacion.DecryptString(varib);
        //    ViewBag.Url = url;
        //    return View();
        //}


        string pathModelo = @"\\coes.org.pe\archivosapp\webapp\";
        string folder = @"Modelos\ModPlan\";
        int IdModplan = 1;

        /// <summary>
        /// Instancia de la clase de servicio1
        /// </summary>
        ModplanAppServicio servicio = new ModplanAppServicio();

        // GET: Planificacion
        public ActionResult Index()
        {
            DescargaModPlanModel model = new DescargaModPlanModel();
            model.Listado = new List<WbVersionModplanDTO>();
            int result = 0;
            string rawUrl = System.Web.HttpContext.Current.Request.RawUrl;
            string url = rawUrl.Replace("/portal/planificacion/descargamodplan/?id=", "");

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    string codigos = Encriptacion.DecryptString(url);
                    string[] ids = codigos.Split('@');
                    int idEmpresa = int.Parse(ids[0]);
                    int idModulo = int.Parse(ids[1]);
                    int idCorreo = int.Parse(ids[2]);

                    FwAccesoModeloDTO acceso = (new GeneralAppServicio()).ObtenerAccesoModPlan(idEmpresa, idModulo, idCorreo);

                    if (acceso != null)
                    {
                        DateTime fechaInicio = (DateTime)acceso.Acmodfecinicio;
                        DateTime fechaFin = (DateTime)acceso.Acmodfin;

                        if (DateTime.Now.Subtract(fechaInicio).TotalMinutes >= 0 && fechaFin.Subtract(DateTime.Now).TotalMinutes >= 0)
                        {
                            //model.Listado = this.servicio.ListWbVersionModplans();
                            //result = 1;
                            (new GeneralAppServicio()).ConfirmarAccesoModPlan(acceso);
                            model.Identificador = url;
                            result = 1;
                        }
                        else
                        {
                            model.FechaConclusion = fechaFin.ToString("dd/MM/yyyy");   
                            result = 2;
                        }
                        model.EntidadAcceso = acceso;
                    }
                }
                catch (Exception ex)
                {
                    result = 0;
                }
            }

            model.Resultado = result;
            ViewBag.Url = url;
            return View(model);
        }

        [HttpPost]
        public PartialViewResult Listado(string identificador, string codigo)
        {
            DescargaModPlanModel model = new DescargaModPlanModel();
            try
            {

                string codigos = Encriptacion.DecryptString(identificador);
                string[] ids = codigos.Split('@');
                int idEmpresa = int.Parse(ids[0]);
                int idModulo = int.Parse(ids[1]);
                int idCorreo = int.Parse(ids[2]);

                FwAccesoModeloDTO acceso = (new GeneralAppServicio()).ObtenerAccesoModPlan(idEmpresa, idModulo, idCorreo);
                model.Resultado = 2;
                if (codigo == acceso.Acmodkey)
                {
                    model.Listado = this.servicio.ListWbVersionModplans(this.IdModplan);
                    model.Resultado = 1;
                    model.Identificador = identificador;
                }
            }
            catch (Exception)
            {
                model.Resultado = -1;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite visualizar el detalle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Detalle(int id)
        {
            DescargaModPlanModel model = new DescargaModPlanModel();
            model.Codigo = id;
            string url = "VERSION_" + id + @"\";
            WbVersionModplanDTO entity = this.servicio.ObtenerDetalleVersion(id, pathModelo, folder + url);
            WbVersionModplanDTO entityPadre = this.servicio.GetByIdWbVersionModplan((int)entity.Vermplpadre);

            model.NombreVersion = entity.Vermpldesc;
            model.NombrePlan = entityPadre.Vermpldesc;
            model.NombreModelo = entity.RutaModelo;
            model.NombreManual = entity.RutaManual;

            return PartialView(model);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivo(int idVersion, string indicador, string identificador)
        {

            string codigos = Encriptacion.DecryptString(identificador);
            string[] ids = codigos.Split('@');
            int idEmpresa = int.Parse(ids[0]);
            int idModulo = int.Parse(ids[1]);
            int idCorreo = int.Parse(ids[2]);

            FwAccesoModeloDTO acceso = (new GeneralAppServicio()).ObtenerAccesoModPlan(idEmpresa, idModulo, idCorreo);

            if (acceso != null)
            {
                DateTime fechaInicio = (DateTime)acceso.Acmodfecinicio;
                DateTime fechaFin = (DateTime)acceso.Acmodfin;

                if (DateTime.Now.Subtract(fechaInicio).TotalMinutes >= 0 && fechaFin.Subtract(DateTime.Now).TotalMinutes >= 0)
                {
                    string url = "VERSION_" + idVersion + @"\";
                    string extension = string.Empty;
                    string nombre = string.Empty;
                    Stream stream = this.servicio.ObtenerArchivo(idVersion, indicador, pathModelo, folder + url, out extension, out nombre);

                    SiEmpresaCorreoDTO cuenta = (new GeneralAppServicio()).ObtenerEmpresaCorreo((int)acceso.Empcorcodi);

                    WbRegistroModplanDTO entity = new WbRegistroModplanDTO()
                    {
                        Vermplcodi = idVersion,
                        Regmodusuario = cuenta.Empcoremail,
                        Emprcodi = cuenta.Emprcodi,
                        Regmodfecha = DateTime.Now,
                        Regmodtipo = this.IdModplan,
                        Arcmplcodi = int.Parse(indicador)
                    };

                    this.servicio.SaveRegistroModPlan(entity);

                    return File(stream, extension, nombre);
                }

            }

            return null;
        }

    }
}
