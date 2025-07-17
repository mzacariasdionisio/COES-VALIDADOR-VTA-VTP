using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.MVC.Extranet.Helper;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Models
{
    public class InsertarSolCambioTipoModel
    {
        public int Tipocodi { get; set; }
        public string strTipo { get; set; }
        public string DocumentoAdjunto { get; set; }
        public string DocumentoNombre { get; set; }
        //public HttpPostedFileBase DocumentoSustentatorio { get; set; }
    }


    public class SolCambioTipoModel
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
        public int EmpresaCondicionVarianteGenerador { get; set; }
        public int EmpresaCondicionVarianteTransmisor { get; set; }
        public int EmpresaCondicionVarianteDistribuidor { get; set; }
        public int EmpresaCondicionVarianteUsuarioLibre { get; set; }


        public RiSolicitudDTO objSolicitud;
        public List<RiSolicituddetalleDTO> ListDetalleSolicitud;
        public List<TipoPrincipalSecundarioModel> ListaPrincipalSecundario;

        public List<SiTipoempresaDTO> TipoEmpresa { get; set; }
        public List<TipoDocumentoSustentarioModel> TipoDocumentoSustentario { get; set; }

        public SolCambioTipoModel()
        {
            objSolicitud = new RiSolicitudDTO();
            ListDetalleSolicitud = new List<RiSolicituddetalleDTO>();
            ListaPrincipalSecundario = new List<TipoPrincipalSecundarioModel>();
        }
    }
}