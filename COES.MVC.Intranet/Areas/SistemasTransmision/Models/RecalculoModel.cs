using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Models
{
    public class RecalculoModel : BaseModel
    {
        public List<StRecalculoDTO> ListaRecalculo { get; set; }
        public StRecalculoDTO Entidad { get; set; }
    }
}