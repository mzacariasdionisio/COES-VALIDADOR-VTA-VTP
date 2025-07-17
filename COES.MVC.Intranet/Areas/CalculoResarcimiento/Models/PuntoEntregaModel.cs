using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Models
{
    public class PuntoEntregaModel
    {
        public bool TienePermisoAdmin { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }        

        public List<RePuntoEntregaDTO> ListadoPuntoEntrega { get; set; }
        public List<ReNivelTensionDTO> ListaNivelTension { get; set; }
        public RePuntoEntregaDTO PuntoEntrega { get; set; }
        public bool Grabar { get; set; }

    }
}