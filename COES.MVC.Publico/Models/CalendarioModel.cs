using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Models
{
    /// <summary>
    /// Model para el calendario de eventos
    /// </summary>
    public class CalendarioModel
    {        
        public string Data { get; set; }
        public string Meses { get; set; }
        public string Feriados { get; set; }
        public string FechaActual { get; set; }
        public List<WbCaltipoventoDTO> ListaLeyenda { get; set; } 
    }

    public class LeyendaIcono
    {
        public string Definicion { get; set; }
        public string Icono { get; set; }
        public string Color { get; set; }
    }

    public class WebHelper
    {
        /// <summary>
        /// Permite obtener los íconos para los eventos
        /// </summary>
        /// <returns></returns>
        public static List<LeyendaIcono> ObtenerIconosDef()
        {
            List<LeyendaIcono> list = new List<LeyendaIcono>();
            list.Add(new LeyendaIcono
            {
                Definicion = ConstantesPortal.TextoEventoPublicacion,
                Icono = ConstantesPortal.IconoEventoPublicacion,
                Color = ConstantesPortal.ColorEventoPublicacion
            });

            list.Add(new LeyendaIcono
            {
                Definicion = ConstantesPortal.TextoEventoVencimiento,
                Icono = ConstantesPortal.IconoEventoVencimiento,
                Color = ConstantesPortal.ColorEventoVencimiento
            });

            list.Add(new LeyendaIcono
            {
                Definicion = ConstantesPortal.TextoEventoReunion,
                Icono = ConstantesPortal.IconoEventoReunion,
                Color = ConstantesPortal.ColorEventoReunion

            });

            return list;
        }
    }
}