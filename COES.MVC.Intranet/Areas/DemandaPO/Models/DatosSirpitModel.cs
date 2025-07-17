using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.DemandaPO.Models
{
    public class DatosSirpitModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<DpoSubestacionDTO> cboSubestaciones { get; set; }
        public List<DpoSubestacionDTO> ListaSubestaciones { get; set; }
        public List<DpoBarraDTO> cboBarras { get; set; }
        public List<DpoBarraDTO> ListaBarras { get; set; }
        public List<DpoTransformadorDTO> cboTransformadores { get; set; }
        public List<DpoTransformadorDTO> ListaTransformadores { get; set; }
        public List<DpoDatos96DTO> ListaDatos96 { get; set; }
    }
}