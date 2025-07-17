using COES.Framework.Base.Tools;
using COES.MVC.Publico.Areas.DirectorioImpugnaciones.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.DirectorioImpugnaciones;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace COES.MVC.Publico.Areas.DirectorioImpugnaciones.Controllers
{
    public class AgendaController : Controller
    {
        //
        // GET: /DirectorioImpugnaciones/Agendas/

        DirectorioImpugnacionesAppServicio servicio = new DirectorioImpugnacionesAppServicio();

        public ActionResult Directorio()
        {
            EventoAgendaModel model = new EventoAgendaModel();
            string mes = DateTime.Now.ToString("MM");
            string anio = DateTime.Now.ToString("yyyy");
            model.Tipo = 1;

            DateTime fecha = DateTime.ParseExact(mes + ' ' + anio, Constantes.FormatoMesAnio,
                CultureInfo.InvariantCulture);

            model.ListaEventosAgenda = servicio.GetByCriteriaWbEventoagendas(model.Tipo, fecha);
            
            List<EventoMostrar> lista = new List<EventoMostrar>();
            foreach (var evento in model.ListaEventosAgenda)
            {
                EventoMostrar eve = new EventoMostrar();
                eve.Codigo = evento.Eveagcodi;
                eve.Titulo = evento.Eveagtitulo;
                eve.FechaInicio = evento.Eveagfechinicio;
                eve.FechaFin = evento.Eveagfechfin;
                eve.Ubicacion = evento.Eveagubicacion;
                eve.Descripcion = evento.Eveagdescripcion;
                eve.Dia = evento.Eveagfechinicio.Value.ToString("dddd", new CultureInfo("es-ES"));
                eve.DiaNro = evento.Eveagfechinicio.Value.Day;
                eve.HoraInicio = evento.Eveagfechinicio.Value.ToString("t") + " " + evento.Eveagfechinicio.Value.ToString("tt");
                eve.HoraFin = evento.Eveagfechfin.Value.ToString("t") + " " + evento.Eveagfechfin.Value.ToString("tt");
                lista.Add(eve);
            }
            model.EventosMes = lista;

            //model.ListaEventosAgenda = servicio.ListWbEventoagendas(model.Tipo, DateTime.Now.Year.ToString());
            ViewBag.Title = "Agenda del Directorio";
            return View("Index", model);
        }

        public ActionResult Calendario()
        {
            EventoAgendaModel model = new EventoAgendaModel();
            model.Tipo = 1;
            model.ListaEventosAgenda = servicio.ListWbEventoagendas(model.Tipo, DateTime.Now.Year.ToString());
            model.ListaEventosAgenda.AddRange(servicio.ListWbEventoagendas(2, DateTime.Now.Year.ToString()));
            return View(model);
        }

        public ActionResult Asamblea()
        {
            EventoAgendaModel model = new EventoAgendaModel();
            model.Tipo = 2;
            model.ListaEventosAgenda = servicio.ListWbEventoagendas(model.Tipo, DateTime.Now.Year.ToString());
            ViewBag.Title = "Agenda de la Asamblea";
            return View("Index", model);
        }

        

        [HttpPost]
        public PartialViewResult ListaEventos(int tipo, string anio)
        {
            EventoAgendaModel model = new EventoAgendaModel();
            model.ListaEventosAgenda = servicio.ListWbEventoagendas(tipo, anio);
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ListaEventosMes(string mes, string anio, int tipo)
        {
            EventoAgendaModel model = new EventoAgendaModel();
            DateTime fecha = DateTime.ParseExact(mes + ' ' + anio, Constantes.FormatoMesAnio,
                CultureInfo.InvariantCulture);

            if (tipo == 1 || tipo == 2)
            {
                model.ListaEventosAgenda = servicio.GetByCriteriaWbEventoagendas(tipo, fecha);
            }
            else 
            {
                model.ListaEventosAgenda = servicio.GetByCriteriaWbEventoagendas(1, fecha);
                model.ListaEventosAgenda.AddRange(servicio.GetByCriteriaWbEventoagendas(2, fecha));
            }


            List<EventoMostrar> lista = new List<EventoMostrar>();
            foreach (var evento in model.ListaEventosAgenda)
            {
                EventoMostrar eve = new EventoMostrar();
                eve.Codigo = evento.Eveagcodi;
                eve.Titulo = evento.Eveagtitulo;
                eve.FechaInicio = evento.Eveagfechinicio;
                eve.FechaFin = evento.Eveagfechfin;
                eve.Ubicacion = evento.Eveagubicacion!= null ? Regex.Replace(evento.Eveagubicacion, "<.*?>", string.Empty) : string.Empty;
                eve.Descripcion = evento.Eveagdescripcion != null ? Regex.Replace(evento.Eveagdescripcion, "<.*?>", string.Empty) : evento.Eveagtitulo != null ? Regex.Replace(evento.Eveagtitulo, "<.*?>", string.Empty) : string.Empty;
                eve.Dia = evento.Eveagfechinicio.Value.ToString("dddd", new CultureInfo("es-ES"));
                eve.DiaNro = evento.Eveagfechinicio.Value.Day;
                eve.HoraInicio = evento.Eveagfechinicio.Value.ToString("t") + " " + evento.Eveagfechinicio.Value.ToString("tt");
                eve.HoraFin = evento.Eveagfechfin.Value.ToString("t") + " " + evento.Eveagfechfin.Value.ToString("tt");
                lista.Add(eve);
            }
            model.EventosMes = lista;
            return PartialView(model);
        }

        /// <summary>
        /// Funcion para visualizar un solo evento de la agenda
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ObtenerEvento(string id)
        {
            EventoAgendaModel model = new EventoAgendaModel();
            model.EventoAgenda = servicio.GetByIdWbEventoagenda(int.Parse(id));
            return PartialView(model);
        }

        /// <summary>
        /// Permite descargar el archivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string id)
        {
            try
            {
                EventoAgendaModel model = new EventoAgendaModel();
                model.EventoAgenda = servicio.GetByIdWbEventoagenda(int.Parse(id));
                string url = RutaDirectorio.DirectorioEventos + "EVEAG" + model.EventoAgenda.Eveagcodi + "." + model.EventoAgenda.Eveagextension;

                if (FileServer.VerificarExistenciaFile(RutaDirectorio.DirectorioEventos, "EVEAG" + model.EventoAgenda.Eveagcodi + "." + model.EventoAgenda.Eveagextension, string.Empty))
                {
                    System.IO.Stream stream = FileServer.DownloadToStream(url, string.Empty);
                    return File(stream, model.EventoAgenda.Eveagextension, model.EventoAgenda.Eveagtitulo + "." + model.EventoAgenda.Eveagextension);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
