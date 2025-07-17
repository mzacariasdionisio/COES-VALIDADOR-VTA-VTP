using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Compensacion.Models
{
    public class ArranquesParadasGeneralModel
    {
        public List<PeriodoDTO> ListTrnPeriodo { get; set; }
        public List<StructData> ListCabArranquesParadas { get; set; }
        public List<string> ListBodyArranquesParadas { get; set; }
        public List<ComboCompensaciones> ListCabAgrupada { get; set; }
        public List<EveSubcausaeventoDTO> ListEveSubcausaevento { get; set; }
        public List<SiEmpresaDTO> ListSiEmpresa { get; set; }
        public string mensaje { get; set; }
    }
}