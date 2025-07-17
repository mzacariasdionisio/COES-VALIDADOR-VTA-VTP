using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Models
{
    public class BarraModel : BaseModel
    {
        public List<StBarraDTO> ListaSTBarra { get; set; }
        public StBarraDTO Entidad { get; set; }

        public List<StRecalculoDTO> ListaStRecalculo { get; set; }
        public StRecalculoDTO EntidadStRecalculo { get; set; }

        public List<StPeriodoDTO> ListaPerido { get; set; }
        public StPeriodoDTO EntidadPeriodo { get; set; }

        public List<BarraDTO> ListaBarra { get; set; }
        public BarraDTO EntidadBarra { get; set; }

        public int IdBarra { get; set; }

        public string sError { get; set; }

    }
}