using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Sorteo.Models
{
    public class HistoricoSorteoModel
    {
        public List<PrLogsorteoDTO> ListarHistorico { get; set; }
        public string Logusuario { get; set; }
        public DateTime Logfecha { get; set; }
        public string Logdescrip { get; set; }
        public string Logtipo { get; set; }
        public string Logcoordinador { get; set; }
        public string Logdocoes { get; set; }
    }
}