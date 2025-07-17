using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class IntegranteModel
    {
        public List<SiEmpresaDTO> ListadoEmpresa { get; set; }
        public List<SiEmpresaCorreoDTO> ListaEmpresaCorreo { get; set; }
        public int IdEmpresa { get; set; }
        public string Integrante { get;set; }
        public List<SiRepresentanteDTO> ListaRepresentantes { get; set; }
        public SiEmpresaCorreoDTO EmpresaCorreo { get; set; }
        public SiRepresentanteDTO Representante { get; set; }
        public string NombreCuenta { get; set; }

        public string CorreoCuenta { get; set; }

        public string CargoCuenta { get; set; }
        public string TelefonoCuenta { get; set; }

        public string MovilCuenta { get; set; }
        public string EstadoCuenta { get; set; }
        public int CodigoCuenta { get; set; }
        public int EmpresaCuenta { get; set; }
        public string IncluirNotificacion { get; set; }
    }
}