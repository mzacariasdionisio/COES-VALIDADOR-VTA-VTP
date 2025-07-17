using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.DirectorioImpugnaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.PostOperacion.Models
{
    /// <summary>
    /// Model para el reporte de pruebas aleatorias
    /// </summary>
    public class EventoModel
    {
        public List<AnalisisFallaDTO> LISTA { get; set; }

        public DateTime? AFEREUFECHAPROG { get; set; }

        public string AFEREUFECHAPROGSTR { get; set; }
        public string AFEREUHORAPROG { get; set; }
        public string EVENASUNTO { get; set; }
        public string AFECONVTIPOREUNION { get; set; }
        public string CODIGO { get; set; }
        public string FECHA_EVENTO { get; set; }
        public int Tipo { get; set; }

        public List<EventoMostrar> EventosMes { get; set; }
    }

    public class EventoMostrar
    {
        public int Codigo { get; set; }
        public string CodigoEvento { get; set; }
        public DateTime? FechaInicio { get; set; }

        public string Fecha { get; set; }
        public string FechaEvento { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Ubicacion { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Dia { get; set; }
        public string Presencial { get; set; }
        public int DiaNro { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
    }

}