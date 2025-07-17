using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Models
{
    public class CumplimientoEnvioMagnitudModel
    {
        public List<RcaProgramaDTO> Programas { get; set; }
        public List<RcaSuministradorDTO> Suministradores { get; set; }
        public List<RcaCuadroProgUsuarioDTO> Reporte { get; set; }
    }
}