using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Models
{
    public class CompensacionModel : BaseModel
    {
        public List<StCompensacionDTO> ListaCompensacion { get; set; }
        public StCompensacionDTO EntidadCompensacion { get; set; }

        public List<StSistematransDTO> ListaSistema { get; set; }
        public StSistematransDTO EntidadSistema { get; set; }

        public List<BarraDTO> ListaBarra { get; set; }
        public BarraDTO EntidadBarra { get; set; }
    }
}