using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class ActualizarTrasEmpFusionModel: BaseModel
    {
        public List<ActualizarTrasEmpFusionDTO> ListaSaldosSobrantes { get; set; }
        public List<ActualizarTrasEmpFusionDTO> ListaSaldosNoIdentificados { get; set; }
    }
}