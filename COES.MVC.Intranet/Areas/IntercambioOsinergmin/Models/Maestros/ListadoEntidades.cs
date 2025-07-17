using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Maestros
{
    public class ListadoEntidades
    {
        public IEnumerable<DetalleEntidadModel> DetalleEntidadModelList { get; set; }
        public bool IsAsignado { get; set; }

        public string Entidad { get; set; }

        public string Radio { get; set; }

        public ListadoEntidades(IEnumerable<DetalleEntidadModel> listadoEntidadModels, bool isAsignado, string entidad, string radio)
        {
            DetalleEntidadModelList = listadoEntidadModels;
            IsAsignado = isAsignado;
            Entidad = entidad;
            Radio = radio;
        }
    }
}