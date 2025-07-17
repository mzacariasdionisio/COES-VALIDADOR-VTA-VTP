using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Remision
{
    public class DetallePeriodoRemisionModel
    {
        public PeriodoRemisionModel PeriodoRemisionModel { get; set; }
        public FiltroPeriodoRemision FiltroPeriodoRemision { get; set; }
        
        public DetallePeriodoRemisionModel(PeriodoRemisionModel modelo, IEnumerable<string> periodosList)
        {
            PeriodoRemisionModel = modelo;
            FiltroPeriodoRemision = new FiltroPeriodoRemision(periodosList, modelo.Periodo);
        }

        public DetallePeriodoRemisionModel()
        {
            PeriodoRemisionModel = new PeriodoRemisionModel();
            FiltroPeriodoRemision = new FiltroPeriodoRemision();
        }

    }
}