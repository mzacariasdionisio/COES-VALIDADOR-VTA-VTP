using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Models
{
    public class RevisionModel
    {
        public int Id { get; set; }
        public List<EstadoModel> Estado { get; set; }
        public List<SiEmpresaDTO> Lista { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresa;
        public bool esUsuarioSGI = false;
        public bool esUsuarioDJR = false;
        public bool esUsuarioDE= false;

        public RevisionModel()
        {
            Lista = new List<SiEmpresaDTO>();
            Estado = new List<EstadoModel>();
            ListaTipoEmpresa = new List<SiTipoempresaDTO>();
        }
    }

    public class CartaModel
    {
        public string NroDocumento { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public string Direccion { get; set; }
        public string Distrito { get; set; }
        public string Empresa { get; set; }
        public string FechaSolicitud { get; set; }
        public string Condicion { get; set; }
        public string NroRegistro { get; set; }
        public string FechaRegistro { get; set; }

        public CartaModel()
        {
        }
    }

}