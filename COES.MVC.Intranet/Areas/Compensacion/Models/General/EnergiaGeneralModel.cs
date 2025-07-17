using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Compensacion.Models
{
    public class EnergiaGeneralModel
    {
        public string Mes { get; set; }
        public List<PeriodoDTO> ListTrnPeriodo { get; set; }
        public List<VceEnergiaDTO> ListEnergia { get; set; }
    }
}