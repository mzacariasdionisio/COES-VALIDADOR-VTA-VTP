using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Models
{
    public class CostoMarginalVsBarrasModel : BaseModel
    {
        public List<CaiEquisddpbarrDTO> ListaEquisddpbarr { get; set; }
        public CaiEquisddpbarrDTO Entidad { get; set; }

        public string Casddbfecvigencia { get; set; }
    }
}