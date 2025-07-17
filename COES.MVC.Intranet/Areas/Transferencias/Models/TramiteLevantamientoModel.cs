using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class TramiteLevantamientoModel
    {
        public List<TipoTramiteDTO> ListaTipoTramites { get; set; }
        public TipoTramiteDTO EntidadTipoTramite { get; set; }
        public TramiteDTO Entidad { get; set; }
        public string PeriodoNombre { get; set; }
        public string NombreVersion { get; set; }
        public int IdTipoTramite { get; set; }
        public bool bGrabar { get; set; }
    }
}