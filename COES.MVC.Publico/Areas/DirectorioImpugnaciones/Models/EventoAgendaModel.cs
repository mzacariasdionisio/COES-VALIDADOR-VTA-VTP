using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.DirectorioImpugnaciones.Models
{
    public class EventoAgendaModel
    {
        public List<WbEventoagendaDTO> ListaEventosAgenda { get; set; }
        public WbEventoagendaDTO EventoAgenda { get; set; }

        public int Codigo { get; set; }
        public int Tipo { get; set; }
        [AllowHtml]
        public string Ubicacion { get; set; }
        public string Titulo { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string ExtensionAntiguo { get; set; }
        [AllowHtml]
        public string Descripcion { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }

        public string Nuevo { get; set; }
        public string CambiarArchivo { get; set; }
        public List<EventoMostrar> EventosMes { get; set; }
    }

    public class EventoMostrar
    {
        public int Codigo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Ubicacion { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; } 
        public string Dia { get; set; }
        public int DiaNro { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
    }
}