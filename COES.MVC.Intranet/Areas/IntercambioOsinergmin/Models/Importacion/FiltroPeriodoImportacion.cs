using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Importacion
{
    public class FiltroPeriodoImportacion
    {
        [Display(Name = "Periodo")]
        public string Periodo { get; set; }

        [Display(Name = "Periodo", Prompt = "Seleccione", Description = "Seleccione el periodo que desea revisar")]
        public List<SelectListItem> Periodos { get; set; }

        public FiltroPeriodoImportacion(IEnumerable<string> periodosList, string periodo)
        {
            InicializarPeriodos(periodosList);
            Periodo = periodo;
        }

        public FiltroPeriodoImportacion()
        {
            Periodos = null;
        }

        private void InicializarPeriodos(IEnumerable<string> periodosList)
        {
            Periodos = new List<SelectListItem>();

            foreach (var item in periodosList.Select(periodo => new SelectListItem
            {
                Text = periodo,
                Value = periodo
            }))
            {
                Periodos.Add(item);
            }
        }
    }
}