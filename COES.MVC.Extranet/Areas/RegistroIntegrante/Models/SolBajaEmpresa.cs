using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Models
{

    public class InsertarSolBajaEmpresaModel
    {
        public int SoliCodi { get; set; }
        public string CondicionBaja { get; set; }
        public string DocumentoAdjunto { get; set; }
        public string DocumentoNombre { get; set; }
        // public HttpPostedFileBase DocumentoSustentatorio { get; set; }

    }

    public class SolBajaEmpresaModel
    {
        public string EmprRUC { get; set; }
        public string EmprRazSocial { get; set; }
        public string EmprNombreComercial { get; set; }
        public string EmprSigla { get; set; }
        public string EmprDomLegal { get; set; }
        public string EmprTipoAgente { get; set; }
        public string CondicionBaja { get; set; }
        public string CondicionBajaDescripcion { get; set; }
        public string DetAdjunto { get; set; }
        public string NombAdjunto { get; set; }

        public string SolicitudenCurso { get; set; }

        public List<CondicionBaja> ListaCondicionBaja;
        public RiSolicitudDTO objSolicitud;
        public List<RiSolicituddetalleDTO> ListDetalleSolicitud;

        public SolBajaEmpresaModel()
        {
            objSolicitud = new RiSolicitudDTO();
            ListDetalleSolicitud = new List<RiSolicituddetalleDTO>();
            ListaCondicionBaja = new List<Models.CondicionBaja>();
        }
    }

    public class CondicionBaja
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}