using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class UnidadExoneradaModel : BaseModel
    {
        public VcrUnidadexoneradaDTO EntidadUnidadExonerada { get; set; }
        public List<VcrUnidadexoneradaDTO> ListaUnidadExonerada { get; set; }
    }
}