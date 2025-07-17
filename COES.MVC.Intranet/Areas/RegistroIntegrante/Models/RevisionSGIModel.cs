using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Models
{
    public class RevisionSGIModel
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
        public string Correlativo { get; set; }
        public string Iteracion { get; set; }
        public string UsuarioRevision { get; set; }

        public int IdIntegrante { get; set; }
        public string NumeroRuc { get; set; }
        public string NombreComercial { get; set; }
        public string RazonSocial { get; set; }
        public string DomicilioLegal { get; set; }

        public string TipoAgente { get; set; }
        public string DocumentoSustentatorio { get; set; }
        public string ArchivoDigitalizado { get; set; }
        public string MaximaDemanda { get; set; }

        public string DocumentoSustentatorioComentario { get; set; }
        public string ArchivoDigitalizadoComentario { get; set; }
        public string MaximaDemandaComentario { get; set; }

        public bool TieneRevisionDetalle { get; set; }
        public SiTipoComportamientoDTO TipoComportamiento { get; set; }

        public string Data { get; set; }
        public string Revifinalizado { get; set; }
        public string Revienviado { get; set; }
        public string Revinotificado { get; set; }
        public string Reviterminado { get; set; }

    }
}