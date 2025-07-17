using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.RechazoCarga.Models
{
    public class EditEsquemaUnifilarModel
    {
        public List<RcaEsquemaUnifilarDTO> ListEsquemaUnifilar { get; set; }

        public RcaEsquemaUnifilarDTO RcaEsquemaUnifilarDTO { get; set; }

        public List<SiEmpresaDTO> ListSiEmpresa { get; set; }
    }
}