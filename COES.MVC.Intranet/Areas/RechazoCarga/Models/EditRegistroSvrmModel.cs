using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Models
{
    public class EditRegistroSvrmModel
    {
        public List<SiEmpresaDTO> ListSiEmpresa { get; set; }

        public List<EqEquipoDTO> ListEqEquipo { get; set; }

        public RcaRegistroSvrmDTO RcaRegistroSvrmDTO { get; set; }
    }
}