using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Helper;
using COES.MVC.Publico.Models;
using COES.MVC.Publico.SeguridadServicio;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace COES.MVC.Publico.Controllers
{
    public class HomeController : Controller
    {
        int iGpsCodi = 1;
        public HomeController() {
           iGpsCodi =Convert.ToInt32(ConfigurationManager.AppSettings["GpsFrecuencia"].ToString());
        }
        /// <summary>
        /// Lista de frecuencias
        /// </summary>
        public List<GraficoFrecuencia> ListaFrecuencia
        {
            get
            {
                return (Session[DatosSesion.ListaFrecuencia] != null) ?
                    (List<GraficoFrecuencia>)Session[DatosSesion.ListaFrecuencia] : new List<GraficoFrecuencia>();
            }
            set
            {
                Session[DatosSesion.ListaFrecuencia] = value;
            }
        }

        /// <summary>
        /// Instancia de la clase servicio correspondiente
        /// </summary>
        PortalAppServicio servicio = new PortalAppServicio();

        SeguridadServicioClient servicioSeguridad = new SeguridadServicioClient();

        /// <summary>
        /// Lista la pagina inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Para Pruebas
            DateTime fechaHoy = DateTime.Now;
            HomeModel model = new HomeModel();
            model.ListaEventos = this.servicio.ListarResumenEventosWeb(fechaHoy).OrderByDescending(x => (DateTime)x.Evenini).ToList();
            var ListComuni = new List<WbComunicadosDTO>();
            var ListComuni1 = this.servicio.ListarComunicados().Where(x => x.Composition == 1 && x.Comestado == ConstantesAppServicio.Activo).OrderBy(x => x.Comorden).ToList();
            var ListComuni2 = this.servicio.ListarComunicados().Where(x => x.Composition != 1 && x.Comestado == ConstantesAppServicio.Activo && x.Comtipo != "S").OrderByDescending(x => x.Comfecha).ToList();
            ListComuni.AddRange(ListComuni1);
            ListComuni.AddRange(ListComuni2);
            model.ListaComunicado = ListComuni.Take(30).ToList();
            model.ListaBanner = (new COES.Storage.App.Servicio.Portal()).ObtenerBannerPortal();
            model.ListaSalaPrensa = this.servicio.ListarComunicados().Where(x => x.Composition != 1 && x.Comestado == ConstantesAppServicio.Activo && x.Comtipo == "S" && x.Comfechaini <= DateTime.Now && DateTime.Now <= x.Comfechafin).OrderByDescending(x => x.Comfecha).ToList();
            string path = ConfigurationManager.AppSettings["RutaComunicados"].ToString();
            foreach (var item in model.ListaSalaPrensa)
            {
                string ruta = path + item.Comcodi + ".jpg";
                byte[] imagen = null;
                if (System.IO.File.Exists(ruta))
                {
                    imagen = System.IO.File.ReadAllBytes(path + item.Comcodi + ".jpg");
                }

                if (imagen != null)
                {
                    string mimeType = "image/" + "jpg";
                    string base64 = Convert.ToBase64String(imagen);
                    item.ComImagen = string.Format("data:{0};base64,{1}", mimeType, base64);
                }
                else item.ComImagen = null;
            }
            ViewBag.IndicadorAviso = Constantes.SI;
            return View(model);
        }

        /// <summary>
        /// Permite obtener los datos del grafico de frecuencias
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Frecuencia()
        {
            List<GraficoFrecuencia> resultado = this.servicio.ObtenerFrecuenciaSein(iGpsCodi);
            this.ListaFrecuencia = resultado;

            return Json(resultado);
        }

        /// <summary>
        /// Obtiene el nuevo dato de la frecuencia
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateFrecuencia()
        {
            List<GraficoFrecuencia> resultado = this.servicio.ObtenerFrecuenciaSein(iGpsCodi);
            List<GraficoFrecuencia> result = resultado.Where(x => !this.ListaFrecuencia.Any(y => x.Fecha == y.Fecha)).ToList();
            this.ListaFrecuencia.AddRange(result);

            return Json(result);
        }

        /// <summary>
        /// Permite obtener los datos del grafico de demanda
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Demanda()
        {
            //Para Pruebas
            //DateTime fechaEjecutado = Convert.ToDateTime("12/10/2022");
            DateTime fechaEjecutado = DateTime.Now;
            decimal valorEjecutado = 0;
            ChartDemanda demanda = this.servicio.ObtenerReporteDemanda(fechaEjecutado, fechaEjecutado, out fechaEjecutado, out valorEjecutado);
            demanda.LastDate = fechaEjecutado.ToString(Constantes.FormatoFechaFull);
            demanda.LastValue = valorEjecutado;

            return Json(demanda);
        }

        /// <summary>
        /// Permite obtener los datos del gráfico de producción
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Produccion()
        {
            //Para Pruebas
            //DateTime lastDate = Convert.ToDateTime("12/10/2022");
            ChartProduccion produccion = new ChartProduccion();
            DateTime lastDate = DateTime.Now;
            decimal lastValue = 0;
            List<PuntoSerie> list = this.servicio.ObtenerChartProduccionHome(out lastDate, out lastValue);
            produccion.Data = list;
            produccion.LastDate = lastDate.ToString(Constantes.FormatoFechaFull);
            produccion.LastValue = lastValue;

            return Json(produccion);
        }

        /// <summary>
        /// Permite mostrar el menu de la aplicacion
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public PartialViewResult Menu()
        {
            var siteRoot = Url.Content("~/");
            ViewBag.Menu = Helper.Helper.ObtenerMenuPrincipal((new COES.Storage.App.Servicio.Portal()).ObtenerMenuPortal(), siteRoot);
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult MenuPrincipal()
        {
            var siteRoot = Url.Content("~/");
            ViewBag.MenuPrincipal = Helper.Helper.ObtenerMenuPrincipalDesktop((new COES.Storage.App.Servicio.Portal()).ObtenerMenuPortal(), siteRoot);
           return PartialView();
        }
        /// <summary>
        /// Muestra el listado de anexos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Anexos()
        {
            return PartialView();
        }

        /// <summary>
        /// Muestra el calendario de eventos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual PartialViewResult Calendario()
        {
            CalendarioModel model = new CalendarioModel();
            model.ListaLeyenda = this.servicio.ListWbCaltipoventos();

            #region Eventos

            List<WbCalendarioDTO> list = this.servicio.ListWbCalendarios();
            List<DocDiaEspDTO> feriados = this.servicio.ObtenerDiasFeriados();

            int index = 0;
            StringBuilder strFeriado = new StringBuilder();
            strFeriado.Append("[");

            foreach (DocDiaEspDTO item in feriados)
            {
                string final = @",";
                if (index == feriados.Count - 1) final = @"";

                strFeriado.Append("'" + ((DateTime)item.Diafecha).ToString("yyyy-MM-dd") + "'" + final);
                index++;
            }

            strFeriado.Append("]");
            model.Feriados = strFeriado.ToString();

            StringBuilder str = new StringBuilder();
            str.Append("[");

           
           

            index = 0;
            foreach (WbCalendarioDTO item in list)
            {
                string final = @",";
                if (index == list.Count - 1) final = @"";
               
                string contenido = string.Format(@"               
                [
                    id : {0},
                    titulo : '{1}',
                    start : '{2}',
                    end : '{3}',
                    color : '{4}',
                    imageurl: '{6}',
                    description : '{7}'
                ]{5}", item.Calendcodi, item.Calendtitulo, ((DateTime)item.Calendfecinicio).ToString("yyyy-MM-ddTHH:mm:00"),
                  ((DateTime)item.Calendfecinicio).ToString("yyyy-MM-ddTHH:mm:00"), item.Calendcolor, final,
                  Url.Content("~/") + "content/images/" + item.Calendicon, item.Calenddescripcion);
                index++;

                contenido = contenido.Replace("[", "{");
                contenido = contenido.Replace("]", "}");
                str.Append(contenido);
            }

            str.Append("]");
            model.Data = str.ToString();

            #endregion

            #region Meses

            List<WbMescalendarioDTO> listMeses = this.servicio.ListWbMescalendarios();
            StringBuilder strMeses = new StringBuilder();
            strMeses.Append("[");

            index = 0;
            foreach (WbMescalendarioDTO item in listMeses)
            {
                string image = (!string.IsNullOrEmpty(item.Mescalinfo)) ?
                    string.Format(Constantes.InfografiaPortal, item.Mescalcodi) + item.Mescalinfo : string.Empty;

                string final = @",";
                if (index == listMeses.Count - 1) final = @"";

                string contenido = string.Format(@"               
                [
                    anio : {0},
                    mes : '{1}',
                    imagen : '{2}',
                    color : '{3}',
                    colorsat : '{4}',
                    colorsun: '{5}',
                    titulo: '{6}',
                    subtitulo: '{7}',
                    colortitulo: '{8}',
                    colorsubtitulo: '{9}',
                    colordia: '{10}',
                    aniomes: '{11}'

                ]{12}",
                     item.Mescalanio, COES.Base.Tools.Util.ObtenerNombreMesAbrev((int)item.Mescalmes),
                     image, item.Mescalcolor, item.Mescalcolorsat, item.Mescalcolorsun, item.Mescaltitulo, item.Mescaldescripcion,
                     item.Mescalcolortit, item.Mescalcolorsubtit, item.Mesdiacolor, item.Mescalanio + "-" + item.Mescalmes.ToString().PadLeft(2, '0'), final);
                index++;

                contenido = contenido.Replace("[", "{");
                contenido = contenido.Replace("]", "}");
                strMeses.Append(contenido);
            }

            strMeses.Append("]");
            model.Meses = strMeses.ToString();
            model.FechaActual = "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            #endregion


            return PartialView(model);
        }

        /// <summary>
        /// Cierra el calendario
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CloseAvisoCalendario()
        {
            Session["IndicadorCalendario"] = Constantes.SI;
            return Json(1);
        }
    }
}
