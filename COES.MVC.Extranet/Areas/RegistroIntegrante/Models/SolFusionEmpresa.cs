using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Models
{

    public class InsertarSolFusionEmpresaModel
    {
        public int SoliCodi { get; set; }
        public string Vista { get; set; }

        public string DocumentoAdjunto { get; set; }
        public string DocumentoNombre { get; set; }

        //public HttpPostedFileBase DocumentoSustentatorio { get; set; }
    }

    public class SolFusionEmpresaModel
    {
        public string EmprRUC { get; set; }
        public string EmprRUCCambio { get; set; }
        public string EmprRazSocial { get; set; }
        public string EmprRazSocialCambio { get; set; }
        public string EmprNombreComercial { get; set; }
        public string EmprNombreComercialCambio { get; set; }
        public string EmprSigla { get; set; }
        public string EmprSiglaCambio { get; set; }
        public string EmprDomLegal { get; set; }
        public string EmprTipoAgente { get; set; }
        public string DetAdjunto { get; set; }
        public string NombAdjunto { get; set; }

        public string SolicitudenCurso { get; set; }

        public RiSolicitudDTO objSolicitud;
        public List<RiSolicituddetalleDTO> ListDetalleSolicitud;

        public SolFusionEmpresaModel()
        {
            objSolicitud = new RiSolicitudDTO();
            ListDetalleSolicitud = new List<RiSolicituddetalleDTO>();
        }


    }
}