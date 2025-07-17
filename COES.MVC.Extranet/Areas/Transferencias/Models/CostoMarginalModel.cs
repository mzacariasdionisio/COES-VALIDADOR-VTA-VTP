using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class CostoMarginalModel
    {
        public List<CostoMarginalDTO> ListaCostoMarginal { get; set; }
        public CostoMarginalDTO Entidad { get; set; }
        public int IdCostoMarginal { get; set; }

    }
}
