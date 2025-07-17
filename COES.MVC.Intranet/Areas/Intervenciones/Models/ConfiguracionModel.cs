using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Intervenciones.Models
{
    public class ConfiguracionModel
    {
    }

    public class NotificacionModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<InConfiguracionNotificacion> ListaConfiguracion { get; set; }
        public string Empresa { get; set; }
        public string Usuario { get; set; }
    }
}