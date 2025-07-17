using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Models
{
    public class PlantillaCorreoModel
    {

        public bool TienePermisoAdmin { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public List<SiPlantillacorreoDTO> ListadoPlantillasCorreo { get; set; }
        public SiPlantillacorreoDTO PlantillaCorreo { get; set; }
        public List<VariableCorreo> ListaVariables { get; set; }
        public bool Grabar { get; set; }

    }
}