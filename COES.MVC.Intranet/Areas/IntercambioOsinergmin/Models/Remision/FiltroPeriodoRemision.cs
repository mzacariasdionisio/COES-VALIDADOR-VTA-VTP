using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Remision
{
    public class FiltroPeriodoRemision
    {
        [Display(Name = "Periodo")]
        public string Periodo { get; set; }

        [Display(Name = "Periodo", Prompt = "Seleccione", Description = "Seleccione el periodo que desea revisar")]
        public List<SelectListItem> Periodos { get; set; }

        public FiltroPeriodoRemision(IEnumerable<string> periodosList, string periodo)
        {
            InicializarPeriodos(periodosList);
            Periodo = periodo;
        }

        public FiltroPeriodoRemision()
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