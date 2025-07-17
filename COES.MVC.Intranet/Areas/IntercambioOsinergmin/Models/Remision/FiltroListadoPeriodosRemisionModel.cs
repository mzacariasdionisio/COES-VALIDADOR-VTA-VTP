using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Remision
{
    /// <summary>
    /// Contiene los campos con los que se filtra el listado de remisiones
    /// </summary>
    public class FiltroListadoPeriodosRemisionModel
    {
        [Display(Name = "Año: ")]
        public string Anio { get; set; }

        [Display(Name = "Año: ", Prompt = "Seleccione", Description = "Seleccione el año que desea revisar")]
        public List<SelectListItem> Anios { get; set; }

        public FiltroListadoPeriodosRemisionModel(IEnumerable<string> aniosList, string anioActual)
        {
            ValidarListadoAnios(ref aniosList, anioActual);
            InicializarListadoAnios(aniosList);
            Anio = anioActual;
        }

        /// <summary>
        /// Pasa el listado de años a un listado de Select List Items para la vista
        /// </summary>
        /// <param name="aniosList"> Listado a utilizar </param>
        private void InicializarListadoAnios(IEnumerable<string> aniosList)
        {
            Anios = new List<SelectListItem>();

            foreach (var item in aniosList.Select(anio => new SelectListItem
            {
                Value = anio,
                Text = anio
            }))
            {
                Anios.Add(item);
            }
        }

        /// <summary>
        /// Valida que el listado de años tenga correctamente incluido el elemento solicitado
        /// </summary>
        /// <param name="aniosList">Listado a validar</param>
        /// <param name="anio">Elemento a validar</param>
        private static void ValidarListadoAnios(ref IEnumerable<string> aniosList, string anio)
        {
            var enumerable = aniosList as IList<string> ?? aniosList.ToList();

            // Si el año solicitado no esta en el listado, lo agregamos
            if (enumerable.All(x => x != anio))
                enumerable.Insert(0, anio);

            enumerable.OrderByDescending(x => x);
            aniosList = enumerable;
        }
    }
}