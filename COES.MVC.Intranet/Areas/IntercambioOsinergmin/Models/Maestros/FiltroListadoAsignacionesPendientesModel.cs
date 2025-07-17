using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Maestros
{
    public class FiltroListadoAsignacionesPendientesModel
    {
        [Display(Name = "Entidad: ", Prompt = "Seleccione", Description = "Seleccione la entidad que desea revisar")]
        public List<SelectListItem> ListadoOpcionesFiltroItems { get; set; }
        [Display(Name = "Entidad: ")]
        public EntidadSincroniza EntidadEnum { get; set; }

        public int EntidadValue
        {
            get { return (int) EntidadEnum; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("value");
                EntidadValue = value;
            }
        }

        public string EntidadLabel { get; set; }

        public FiltroListadoAsignacionesPendientesModel(EntidadSincroniza entidadArbol, IEnumerable<EntidadSincroniza> entidades, EntidadSincroniza entidadFiltro = EntidadSincroniza.Empresa)
        {
            EntidadLabel = "Entidad: " + EntidadesHelper.GetEntidadDisplayName(entidadArbol);
            EntidadEnum = entidadFiltro;
            ListadoOpcionesFiltroItems = new List<SelectListItem>();

            foreach (var entidadEnum in entidades.Where(entidadEnum => !EntidadesHelper.IsEntidadConAsignacionesPendiente(entidadEnum)))
            {
                ListadoOpcionesFiltroItems.Add(
                    new SelectListItem
                    {
                        Value = entidadEnum.ToString("d"),
                        Text = EntidadesHelper.GetEntidadDisplayName(entidadEnum)
                    });
            }
        }
    }
}