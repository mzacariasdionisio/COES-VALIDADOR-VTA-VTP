using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Models
{
    public class ConfiguracionModel
    {
        public List<ReTipoInterrupcionDTO> ListaTiposInterrupcion { get; set; }
        public ReTipoInterrupcionDTO EntidadTipoInterrupcion { get; set; }
        public List<ReCausaInterrupcionDTO> ListaCausasInterrupcion { get; set; }
        public ReCausaInterrupcionDTO EntidadCausaInterrupcion { get; set; }
        public string NombreTipoInterrupcion { get; set; }
        public int IdTipoInterrupcion { get; set; }
    }
}