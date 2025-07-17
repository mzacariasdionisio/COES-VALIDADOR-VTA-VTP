using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.ServicioRPF.Models
{
    public class EnvioAGCModel
    {
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public string Fecha { get; set; }
        public List<CoConfiguracionGenDTO> ListaReporte { get; set; }
    }
}