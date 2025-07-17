using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Mediciones.Models
{
    public class DesviacionModel
    {
        public List<DesviacionDTO> ListaDesviaciones { get; set; }
        public DesviacionDTO Entidad { get; set; }
        public int NroDesviaciones { get; set; }        
        public DateTime fecha { get; set; }
        public static String version { get; set; }

    }
}