using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Models
{
    public class ProgramaModel
    {
        public List<RcaProgramaDTO> ListProgramas { get; set; }
        public List<RcaHorizonteProgDTO> Horizontes { get; set; }

        public bool VerReprograma { get; set; }

        public int SemanaActual { get; set; }      
    }
}