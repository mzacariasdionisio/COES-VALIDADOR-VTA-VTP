using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Models
{
    public class DemandaUsuarioModel
    {
        public int IdEmpresa { get; set; }
        public int IdFormato { get; set; }
        public int IdModulo { get; set; }
        public string Anho { get; set; }
        public string Mes { get; set; }        
        public List<MeEnvioDTO> ListaPeriodo { get; set; }        
        public List<RcaDemandaUsuarioDTO> ListaReporteInformacion15min { get; set; }      

        public string registros { get; set; }

        public List<RcaSuministradorDTO> Suministradores { get; set; }
        public List<AreaDTO> Subestaciones { get; set; }
        public List<AreaDTO> Zonas { get; set; }
    }
}