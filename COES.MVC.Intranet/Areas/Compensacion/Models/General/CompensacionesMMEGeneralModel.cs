using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Compensacion.Models
{
    public class CompensacionesMMEGeneralModel
    {
        public string Mes { get; set; }
        public List<PeriodoDTO> ListTrnPeriodo { get; set; }
        public List<EveSubcausaeventoDTO> ListEveSubcausaevento { get; set; }
        public List<SiEmpresaDTO> ListSiEmpresa { get; set; }
        public List<VceCompRegularDetDTO> ListCompensacionesMME { get; set; }
    }
}