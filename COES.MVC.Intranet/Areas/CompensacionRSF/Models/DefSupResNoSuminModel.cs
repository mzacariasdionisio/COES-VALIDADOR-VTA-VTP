using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class DefSupResNoSuminModel : BaseModel
    {
        public VcrVersiondsrnsDTO EntidadVersiondsrn { get; set; }
        public string Vcrdsrfeccreacion { get; set; }
        //Lista de tablas
        public List<VcrVersiondsrnsDTO> ListaVcrSuDeRns { get; set; }
        public PeriodoDTO EntidadPeriodo { get; set; }

        //terminos
        public VcrVerdeficitDTO EntidadDeficit { get; set; }
        public List<VcrVerdeficitDTO> ListaDeficit { get; set; }

        public VcrVerrnsDTO EntidadRNS { get; set; }
        public List<VcrVerrnsDTO> ListaRNS { get; set; }

        public VcrVersuperavitDTO EntidadSuperavit { get; set; }
        public List<VcrVersuperavitDTO> ListaSuperavit { get; set; }

        public int vcrdsrcodi { get; set; }
    }
}