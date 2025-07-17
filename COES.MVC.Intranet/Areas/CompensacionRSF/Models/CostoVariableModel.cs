using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class CostoVariableModel : BaseModel
    {
        public VcrRecalculoDTO EntidadVcrVersion { get; set; }
        public VcrCostvariableDTO EntidadCostVar { get; set; }
        public List<VcrRecalculoDTO> ListaVcrVersion { get; set; }
        //Atributos Adicionales
        public int idgrupocodi { get; set; }
    }
}