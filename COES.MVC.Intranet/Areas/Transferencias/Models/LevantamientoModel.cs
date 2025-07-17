using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class LevantamientoModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public List<TramiteDTO> ListaTramites { get; set; }
        public TramiteDTO Entidad { get; set; }
        public int IdTramite { get; set; }
    }
}