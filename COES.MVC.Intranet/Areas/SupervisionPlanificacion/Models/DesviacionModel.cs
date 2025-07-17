using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace COES.MVC.Intranet.Areas.SupervisionPlanificacion.Models
{
    public class DesviacionModel
    {
        public List<DesviacionDTO> ListaDesviaciones { get; set; }
        public DesviacionDTO Entidad { get; set; }
        public int NroDesviaciones { get; set; }
        public string FechaConsulta { get; set; }
        public static String version { get; set; }
        public string Mensaje { get;set; }
        public string Indicador { get; set; }

    }
}