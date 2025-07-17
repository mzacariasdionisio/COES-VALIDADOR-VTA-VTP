using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Importacion
{
    public class DetallePeriodoImportacionModel
    {
        public PeriodoImportacionModel PeriodoImportacionModel { get; set; }
        public FiltroPeriodoImportacion FiltroPeriodoImportacion { get; set; }

        public DetallePeriodoImportacionModel(PeriodoImportacionModel modelo, IEnumerable<string> periodosList)
        {
            PeriodoImportacionModel = modelo;
            FiltroPeriodoImportacion = new FiltroPeriodoImportacion(periodosList, modelo.Periodo);
        }

        public DetallePeriodoImportacionModel()
        {
            PeriodoImportacionModel = new PeriodoImportacionModel();
            FiltroPeriodoImportacion = new FiltroPeriodoImportacion();
        }

    }
}