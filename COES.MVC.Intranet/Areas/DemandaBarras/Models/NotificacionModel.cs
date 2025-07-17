using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DemandaBarras.Models
{
    public class NotificacionModel
    {
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<SiEmpresaCorreoDTO> ListaCuentas { get; set; }
        public SiEmpresaCorreoDTO EntidadCuenta { get; set; }
        public List<SiCorreoDTO> ListaCorreos { get; set; }
        public string FechaLog { get; set; }
        public int IdPlantilla { get; set; }
        public string EstadoProceso { get; set; }
        public string AsuntoCorreo { get; set; }
        [AllowHtml]
        public string ContenidoCorreo { get; set; }
        public List<SiEmpresaCorreoDTO> ListaEmpresaConfiguracion { get; set; }
    }
}