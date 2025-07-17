using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class DespachoURSModel : BaseModel
    {
        public VcrRecalculoDTO EntidadVcrVersion { get; set; }
        public VcrDespachoursDTO EntidadDespacho { get; set; }
        public List<VcrDespachoursDTO> ListaDespacho { get; set; }
    }
}