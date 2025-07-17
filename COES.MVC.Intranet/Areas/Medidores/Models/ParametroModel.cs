using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Medidores.Models
{
    public class ParametroModel
    {
        public ParametroRangoSolar RangoSolar { get; set; }
        public List<ParametroRangoSolar> ListaRangoSolar { get; set; }
        public List<EstadoParametro> ListaEstado { get; set; }

        public ParametroHPPotenciaActiva HPPotenciaActiva { get; set; }
        public List<ParametroHPPotenciaActiva> ListaHPPotenciaActiva { get; set; }

        public ParametroRangoPotenciaInductiva RangoPotenciaInductiva { get; set; }
        public List<ParametroRangoPotenciaInductiva> ListaRangoPotenciaInductiva { get; set; }

        public ParametroRangoPeriodoHP RangoPeriodoHP { get; set; }
        public List<ParametroRangoPeriodoHP> ListaRangoPeriodoHP { get; set; }
    }
}