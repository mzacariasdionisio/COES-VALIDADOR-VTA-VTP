using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Despacho.Models
{
    public class CortoPlazoModel
    {
        public string FechaConsulta { get; set; }
        public List<CpTopologiaDTO> ListaTopologia { get; set; }
        public List<CpMedicion48DTO> ListaMedicion { get; set; }
        public List<CpTopologiaDTO> ListaRestricciones { get; set; }
    }
}