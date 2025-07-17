using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;


namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class MotivosModel
    {
        public int IdModulo { get; set; }
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<EveSubcausaeventoDTO> ListaMotivos { get; set; }
        public object Datos { get; set; }
    }
}