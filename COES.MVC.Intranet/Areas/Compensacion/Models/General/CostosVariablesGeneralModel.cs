using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Compensacion.Models
{
    public class CostosVariablesGeneralModel
    {
        public List<PeriodoDTO> ListTrnPeriodo { get; set; }

        public List<VceDatcalculoDTO> ListDatCalculo { get; set; }

        public VceDatcalculoDTO VceDatCalculo { get; set; }

        public List<BarraDTO> ListTrnBarra { get; set; }
        public List<SiEmpresaDTO> ListSiEmpresa { get; set; }
    }
}