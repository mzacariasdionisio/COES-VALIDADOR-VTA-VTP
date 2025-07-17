using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Models
{
    public class RevisionDJRModel
    {
        public int Revicodi { get; set; }
        public string EmprRUC { get; set; }
        public string EmprRazSocial { get; set; }
        public string EmprNombreComercial { get; set; }
        public string EmprSigla { get; set; }
        public string EmprDomLegal { get; set; }
        public string EmprTipoAgente { get; set; }
        public int EmprTipoAgenteCodigo { get; set; }
        public int EmprCodi { get; set; }
        public string Reviestado { get; set; }
        public string Revifinalizado { get; set; }
        public string Correlativo { get; set; }
        public string Iteracion { get; set; }
        public RiRevisionDTO RiRevision { get; set; }
        public List<RiDetalleRevisionDTO> ListaDetalles { get; set; }
        public List<SiRepresentanteDTO> ListaRepresentantes { get; set; }
        public List<TitulosAdicionalesModel> TitulosAdicionales { get; set; }

        public string Data { get; set; }
        public string DataTA { get; set; }
        public string UsuarioRevision { get; set; }
        public string ComentarioTitulosAdicionales { get; set; }

        public RevisionDJRModel()
        {
            ListaRepresentantes = new List<SiRepresentanteDTO>();
            ListaDetalles = new List<RiDetalleRevisionDTO>();
        }
    }
}