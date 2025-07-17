using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Models
{
    public class EnvioArchivosMagnitudModel
    {
        public List<RcaProgramaDTO> Programas { get; set; }
        public List<RcaSuministradorDTO> Suministradores { get; set; }
    }
}