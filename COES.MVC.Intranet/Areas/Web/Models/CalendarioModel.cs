using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Web.Models
{
    /// <summary>
    /// Model para el calendario de eventos
    /// </summary>
    public class CalendarioModel
    {
        public List<WbCalendarioDTO> ListaCalendario { get; set; }
        public WbCalendarioDTO Entidad { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public int Codigo { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; }
        public string Estado { get; set; }
        public string Color { get; set; }
        public List<string> ListaIconos { get; set; }
        public List<string> ListaColores { get; set; }
        public string Data { get; set; }
        public string Meses { get; set; }
        public string Feriados { get; set; }
        public string FechaActual { get; set; }
        public List<WbCaltipoventoDTO> ListaLeyenda { get; set; }

        public int? TipoEvento { get; set; }
        public List<WbCaltipoventoDTO> ListaTipo { get; set; }
        
    }

    /// <summary>
    /// Model para los meses e infografia
    /// </summary>
    public class MesCalendarioModel
    {
        public List<WbMescalendarioDTO> Listado { get; set; }
        public WbMescalendarioDTO Entidad { get; set; }
        public List<int> ListaAnios { get; set; }
        public List<MesCalendarItemModel> ListMeses { get; set; }
        public int AnioActual { get; set; }
        public List<string> ListaColores { get; set; }
        public List<string> ListaColoresSabado { get; set; }
        public List<string> ListaColoresDomingo { get; set; }
        public List<string> ListaColorTitulo { get; set; }
        public List<string> ListaColorSubtitulo { get; set; }
        public string UrlPortal { get; set; }
        public string Titulo { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string ColorMes { get; set; }
        public string ColorSabado { get; set; }
        public string ColorDomingo { get; set; }
        public string ColorTitulo { get; set; }
        public string ColorSubtitulo { get; set; }
        public string Estado { get; set; }
        public string Descripcion { get; set; }
        public string ColorDia { get; set; }
        public int Codigo { get; set; } 
    }

    /// <summary>
    /// Tipo de evento de calendario COES
    /// </summary>
    public class TipoEventoModel
    {
        public List<WbCaltipoventoDTO> Listado { get; set; }
        public WbCaltipoventoDTO Entidad { get; set; }
        public List<string> ListaIcono { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public string Icono { get; set; }
        public int Codigo { get; set; }
        public string Estado { get; set; }
    }

    /// <summary>
    /// Lista de items del calendario
    /// </summary>
    public class MesCalendarItemModel
    {
        public int Valor { get; set; }
        public string Texto { get; set; }
    
    }   
}