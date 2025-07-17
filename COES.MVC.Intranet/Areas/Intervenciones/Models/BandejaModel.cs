using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Intervenciones.Models
{
    public class BandejaModel
    {
        public List<EveEvenclaseDTO> ListaTiposProgramacion { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<SiMensajeDTO> ListaMensajes { get; set; }
        public int Progcodi { get; set; }
    }
}