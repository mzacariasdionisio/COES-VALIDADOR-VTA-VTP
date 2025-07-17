using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Models
{
    public class OfertaMPAModel : BaseModel
    {
        public string sFechaEnvioIni { get; set; }
        public string sFechaEnvioFin { get; set; }
        public List<TrnBarraursDTO> ListaBarraURS { get; internal set; }
    }
}