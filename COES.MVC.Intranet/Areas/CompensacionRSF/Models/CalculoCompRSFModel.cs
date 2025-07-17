using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class CalculoCompRSFModel : BaseModel
    {
        public VcrCargoincumplDTO EntidadIncumpl = new VcrCargoincumplDTO();
        public List<VcrCargoincumplDTO> ListaIncumpl = new List<VcrCargoincumplDTO>();

    }
}