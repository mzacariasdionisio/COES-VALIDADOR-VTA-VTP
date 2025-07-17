using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.GMM.Models
{
    public class AgenteModel
    {
        /*
            <!-- HABILITADO | RAZON SOCIAL | TIPO | FECHA VECIMIENTO | MODALIDAD | MOTO S/. | INCUMPLIMIENTO | EDITAR -->
         */
        public string Habilitado { get; set; }
        public string razonSocial { get; set; }
        public string tipo { get; set; }
        public string fechaVencimiento { get; set; }
        public string modalidad { get; set; }
        public string monto { get; set; }
        public string incumplimiento { get; set; }

        public List<GmmEmpresaDTO> listadoAgentes { get; set; }
        public List<GmmEmpresaDTO> listadoModalidades { get; set; }
        public List<GmmEmpresaDTO> listadoEstados { get; set; }
        public List<GmmEmpresaDTO> listadoIncumplimientos { get; set; }

        public List<SiEmpresaDTO> ListSiEmpresa { get; set; }
        public List<GmmEmpresaDTO> ListMaestroEmpresa { get; set; }
        public List<GmmEmpresaDTO> ListMaestroEmpresaCliente { get; set; }

        public AgenteModel()
        {
            listadoAgentes = new List<GmmEmpresaDTO>();
            ListSiEmpresa = new List<SiEmpresaDTO>();

            listadoModalidades = new List<GmmEmpresaDTO>();
            listadoEstados = new List<GmmEmpresaDTO>();
            listadoIncumplimientos = new List<GmmEmpresaDTO>();
        }
    }
}