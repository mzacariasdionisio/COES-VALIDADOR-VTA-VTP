using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Compensacion.Models
{
    public class TextoReporteGeneralModel
    {
        public List<PeriodoDTO> ListTrnPeriodo { get; set; }
        public VcePeriodoCalculoDTO VcePeriodoCalculoDTO { get; set; }
        public List<VceTextoReporteDTO> ListTextoReporte { get; set; }
    }
}