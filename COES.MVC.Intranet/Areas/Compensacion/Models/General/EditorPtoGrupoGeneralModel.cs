using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Compensacion.Models
{
    public class EditorPtoGrupoGeneralModel
    {
        // DSH 02-05-2017 : Se agrego por requerimiento
        public List<PeriodoDTO> ListTrnPeriodo { get; set; }
        public PeriodoDTO TrnPeriodoDTO { get; set; }
        public VcePeriodoCalculoDTO VcePeriodoCalculoDTO { get; set; }
        
        public List<string> ListGrillaHead { get; set; }
        public List<ComboCompensaciones> ListGrillaBody { get; set; }

        public MePtomedicionDTO mePtomedicion { get; set; }
        public List<MePtomedicionDTO> ListMePtomedicion { get; set; }
        public List<VcePtomedModopeDTO> ListVcePtomedModope { get; set; }

        public List<PrGrupodatDTO> ListPrGrupodat { get; set; }
        public string mensaje { get; set; }

    }
}