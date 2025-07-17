using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.MVC.Extranet.Helper;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Models
{
    public class InsertarSolCambioRepresentanteModel
    {
        public int SoliCodi { get; set; }
        public string strRepresentateLegal { get; set; }
        public string DocumentoAdjunto { get; set; }
        public string DocumentoNombre { get; set; }
        //public HttpPostedFileBase DocumentoSustentatorio { get; set; }
    }


    public class SolCambioRepresentanteModel
    {
        public string EmprRUC { get; set; }
        public string EmprRazSocial { get; set; }
        public string EmprNombreComercial { get; set; }
        public string EmprSigla { get; set; }
        public string EmprDomLegal { get; set; }
        public string EmprTipoAgente { get; set; }

        public string DetAdjunto { get; set; }
        public string NombAdjunto { get; set; }

        public string SolicitudenCurso { get; set; }

        public RiSolicitudDTO objSolicitud;
        public List<RiSolicituddetalleDTO> ListDetalleSolicitud;
        public List<TipoDocumentoModel> ListaTipoDocumento;

        public SolCambioRepresentanteModel()
        {
            objSolicitud = new RiSolicitudDTO();
            ListDetalleSolicitud = new List<RiSolicituddetalleDTO>();
            ListaTipoDocumento = new List<TipoDocumentoModel>();
        }        
    }
}