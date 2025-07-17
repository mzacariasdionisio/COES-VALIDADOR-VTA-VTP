using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Monitoreo.Models
{
    public class CmgProgramadosModel
    {
        public List<CmCostomarginalprogDTO> ListaCmg { get; set; }
        public string Resultado { get; set; }
        public int DiaMes { get; set; }
        public string FechaInicio { get; set; }
    }
}