using COES.Dominio.DTO.Transferencias;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class ActualizarTrasEmpFusionModel 
    {
        public List<ActualizarTrasEmpFusionDTO> ListaSaldosSobrantes { get; set; }
        public List<ActualizarTrasEmpFusionDTO> ListaSaldosNoIdentificados { get; set; }
        public List<PeriodoDTO> ListaPeriodos { get; set; }
        
    }
}