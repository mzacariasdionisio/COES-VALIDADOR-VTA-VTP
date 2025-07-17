using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.DemandaCP.Models
{
    public class ReprogramaModel
    {
        public int IdModulo { get; set; }
        public string Fecha { get; set; }        
        public List<PrnVersiongrpDTO> ListaVersion { get; set; }
        public List<MePtomedicionDTO> ListaAreaOperativa { get; set; }
        public List<string[]> ListaIntervalos { get; set; }
        public List<MePtomedicionDTO> ListaBarras { get; set; }
        public List<PrnVersiongrpDTO> ListaVersionComp { get; set; }
        public List<MePerfilRuleDTO> ListaFormulas { get; set; }
        public List<CpTopologiaDTO> ListaEscenario { get; set; }
        public List<PrnVersiongrpDTO> ListaVersionAnte { get; set; }
        public string FechaAnterior { get; set; }
    }
}