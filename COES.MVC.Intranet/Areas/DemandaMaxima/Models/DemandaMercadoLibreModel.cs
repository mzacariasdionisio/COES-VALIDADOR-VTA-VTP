using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Mediciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.DemandaMaxima.Models
{
    public class DemandaMercadoLibreModel
    {
        public List<MeEnvioDTO> ListaPeriodo { get; set; }

        public List<SiEmpresaDTO> ListaEmpresasCumplimiento { get; set; }

        public List<MeDemandaMLibreDTO> ListaDemandaMercadoLibre { get; set; }

        public List<RcaSuministradorDTO> Suministradores { get; set; }

        public string registros { get; set; }

        public List<IioPeriodoSicliDTO> ListaPeriodoSicli { get; set; }
    }
}