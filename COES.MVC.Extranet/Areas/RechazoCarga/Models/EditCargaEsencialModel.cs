using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.RechazoCarga.Models
{
    public class EditCargaEsencialModel
    {
        public List<SiEmpresaDTO> ListSiEmpresa { get; set; }

        public List<EqEquipoDTO> ListEqEquipo { get; set; }

        public RcaCargaEsencialDTO RcaCargaEsencialDTO { get; set; }
        public string nombArc { get; set; }
    }
}