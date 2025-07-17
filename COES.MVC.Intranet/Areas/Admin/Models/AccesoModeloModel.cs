using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class AccesoModeloModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<COES.MVC.Intranet.SeguridadServicio.ModuloDTO> ListaModulos { get; set; }

        public List<SiEmpresaCorreoDTO> ListaEmpresaCorreo { get; set; }

        public List<FwAccesoModeloDTO> ListaAccesos { get; set; }
        public int IdEmpresa { get; set; }       
        public SiEmpresaCorreoDTO EmpresaCorreo { get; set; }


        public string NombreCuenta { get; set; }

        public string CorreoCuenta { get; set; }

        public string CargoCuenta { get; set; }
        public string TelefonoCuenta { get; set; }

        public string MovilCuenta { get; set; }
        public string EstadoCuenta { get; set; }
        public int CodigoCuenta { get; set; }
        public int EmpresaCuenta { get; set; }
        public string IncluirNotificacion { get; set; }
        public int IdModulo { get; set; }

        public int IdEmpresaCorreo { get; set; }
        public int NroVeces { get; set; }
        public string ActivoDesde { get; set; }
        public string ActivoHasta { get; set; }
    }
}