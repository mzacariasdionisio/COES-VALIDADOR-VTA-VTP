using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Models
{
    public class EmpresaModel : BaseModel
    {
        public List<StRecalculoDTO> ListaStRecalculo { get; set; }
        public StRecalculoDTO EntidadStRecalculo { get; set; }

        public List<StPeriodoDTO> ListaPerido { get; set; }
        public int StPercodi { get; set; }
        public int StRecacodi { get; set; }
        public int IdBarra { get; set; }

    }
}