using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.DirectorioImpugnaciones.Models;
using COES.MVC.Publico.Areas.PostOperacion.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.DirectorioImpugnaciones;
using COES.Servicios.Aplicacion.Eventos;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.PostOperacion.Controllers
{
    public class EvaluacionFallasController : Controller
    {
        //
        // GET: /PostOperacion/EvaluacionFallas/

        DirectorioImpugnacionesAppServicio servicio = new DirectorioImpugnacionesAppServicio();
        AnalisisFallasAppServicio service = new AnalisisFallasAppServicio();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CalendarioCtaf()
        {
            EventoModel analisis = new EventoModel();
            analisis.LISTA = service.ObtenerAnalisisFallaCompleto();
            ViewBag.Title = "Calendario CTAF";
            return View("Ctaf", analisis);
        }

        [HttpPost]
        public JsonResult ListaEventosMes(string mes, string anio)
        {
            List<AnalisisFallaDTO> listas = new List<AnalisisFallaDTO>();
            DateTime fecha = DateTime.ParseExact(mes + ' ' + anio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            listas = service.ObtenerAnalisisFallaCompleto(fecha);
            return Json(listas);
        }

        [HttpPost]
        public PartialViewResult ListaEventosMesNew(string mes, string anio)
        {
            EventoModel analisis = new EventoModel();
            List<AnalisisFallaDTO> listas = new List<AnalisisFallaDTO>();
            DateTime fecha = DateTime.ParseExact(mes + ' ' + anio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            listas = service.ObtenerAnalisisFallaCompleto(fecha);

            List<COES.MVC.Publico.Areas.PostOperacion.Models.EventoMostrar> listaEvento = new List<COES.MVC.Publico.Areas.PostOperacion.Models.EventoMostrar>();
            foreach (var evento in listas)
            {
                COES.MVC.Publico.Areas.PostOperacion.Models.EventoMostrar eve = new COES.MVC.Publico.Areas.PostOperacion.Models.EventoMostrar();
                eve.Codigo = evento.AFECODI;
                eve.CodigoEvento = evento.CODIGO;
                eve.Fecha = evento.FECHA_EVENTO;
                eve.Descripcion = evento.EVENASUNTO;
                eve.Presencial = evento.AFECONVTIPOREUNION == "P" ? "Sí" : "No";
                eve.FechaEvento = evento.AFEREUFECHAPROGstr + " " + evento.AFEREUHORAPROG;
                eve.Dia = evento.AFEREUFECHAPROG.Value.ToString("dddd", new CultureInfo("es-ES"));
                eve.DiaNro = evento.AFEREUFECHAPROG.Value.Day;

                //eve.FechaInicio = evento.FECHA_EVENTO;
                //eve.FechaFin = evento.Eveagfechfin;
                //eve.Ubicacion = evento.Eveagubicacion != null ? Regex.Replace(evento.Eveagubicacion, "<.*?>", string.Empty) : string.Empty;
                // != null ? Regex.Replace(evento.Eveagdescripcion, "<.*?>", string.Empty) : evento.Eveagtitulo != null ? Regex.Replace(evento.Eveagtitulo, "<.*?>", string.Empty) : string.Empty;
                //eve.Dia = evento.Eveagfechinicio.Value.ToString("dddd", new CultureInfo("es-ES"));
                //eve.DiaNro = evento.Eveagfechinicio.Value.Day;
                //eve.HoraInicio = evento.Eveagfechinicio.Value.ToString("t") + " " + evento.Eveagfechinicio.Value.ToString("tt");
                //eve.HoraFin = evento.Eveagfechfin.Value.ToString("t") + " " + evento.Eveagfechfin.Value.ToString("tt");
                listaEvento.Add(eve);
            }
            analisis.EventosMes = listaEvento.OrderBy(m => m.DiaNro).ToList();
            return PartialView(analisis);
        }

        [HttpPost]
        public PartialViewResult ObtenerEvento(string id)
        {
            EventoModel model = new EventoModel();
            AnalisisFallaDTO eventoAnalisis = new AnalisisFallaDTO();
            eventoAnalisis = service.ObtenerAnalisisFalla2(int.Parse(id));

            model.AFEREUFECHAPROGSTR = eventoAnalisis.AFEREUFECHAPROGstr;
            model.AFEREUHORAPROG = eventoAnalisis.AFEREUHORAPROG;
            model.EVENASUNTO = eventoAnalisis.EVENASUNTO;
            model.AFECONVTIPOREUNION = eventoAnalisis.AFECONVTIPOREUNION;
            model.CODIGO = eventoAnalisis.CODIGO;
            model.FECHA_EVENTO = eventoAnalisis.FECHA_EVENTO;
            return PartialView(model);
        }

    }
}
