using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Models
{
    public class RegistroSvrmModel
    {
        public List<RcaRegistroSvrmDTO> ListRegistroSvrm { get; set; }

        public RcaRegistroSvrmDTO RcaRegistroSvrmDTO { get; set; }

        public List<SiEmpresaDTO> ListSiEmpresa { get; set; }
    }
}