using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{
    public class ReportePotenciaModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<EqEquipoDTO> ListaCentrales { get; set; }

    }

    public class ConfiguracionModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<PrGrupoDTO> ListaModosConfigurados { get; set; }
        public List<PrGrupoDTO> ListaModosDisponibles { get; set; }
    }
}
