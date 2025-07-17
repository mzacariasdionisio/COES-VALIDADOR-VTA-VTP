using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Models
{
    public class PeriodoModel : BaseModel
    {
        public List<StPeriodoDTO> ListaPeriodos { get; set; }
        public StPeriodoDTO Entidad { get; set; }
    }
}