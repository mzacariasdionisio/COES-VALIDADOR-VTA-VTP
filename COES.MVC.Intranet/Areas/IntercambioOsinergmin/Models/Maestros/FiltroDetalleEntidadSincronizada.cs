using System.ComponentModel.DataAnnotations;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Servicios.Aplicacion.IntercambioOsinergmin;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Maestros
{


    public class FiltroDetalleEntidadSincronizada
    {
        [Display(Name = "Nombre: ", Prompt = "Escribir texto")]
        public string NombreEntidad { get; set; }

        public string EntidadLabel { get; set; }

        public List<string> ListEntidades { get; set; }

        public string FechaUltSincronizacion { get; set; }

        public FiltroDetalleEntidadSincronizada(EntidadSincroniza entidad, string nombreEntidad = null)
        {
            EntidadLabel = "Entidad: " + EntidadesHelper.GetEntidadDisplayName(entidad);
            NombreEntidad = nombreEntidad;

            List<string> lista = new List<string>();
            foreach (var item in Enum.GetValues(typeof(EntidadSincroniza)).Cast<EntidadSincroniza>().Select(v => v.ToString()).ToList())
            {
                if (!item.ToString().Equals("Ninguno"))
                {
                    lista.Add(item.ToString());
                }
            }
            ListEntidades = lista;

            SincronizaMaestroAppServicio sincronizaMaestroAppServicio = new SincronizaMaestroAppServicio();

            FechaUltSincronizacion = sincronizaMaestroAppServicio.ObtenerFechaUltSincronizacion();
        }
    }
}