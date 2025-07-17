using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.RechazoCarga.Models
{
    public class EsquemaUnifilarModel
    {
        public List<SiEmpresaDTO> ListSiEmpresa { get; set; }
        public List<RcaEsquemaUnifilarDTO> ListEsquemaUnifilar { get; set; }

        public RcaEsquemaUnifilarDTO RcaEsquemaUnifilarDTO { get; set; }
    }
}