using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SupervisionPlanificacion.Models
{
    public class RpfEnergiaPotenciaModel
    {
        public List<RpfEnergiaPotenciaDTO> ListaRPF { get; set; }
        public RpfEnergiaPotenciaDTO Entidad { get; set; }
        public int NroRPF { get; set; }
        public DateTime fecha { get; set; }
        public DateTime RpfhidfechaIni { get; set; }
        public DateTime RpfhidfechaFin { get; set; }

    }
}