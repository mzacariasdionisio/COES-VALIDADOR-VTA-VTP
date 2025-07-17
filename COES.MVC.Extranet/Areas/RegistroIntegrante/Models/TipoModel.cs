using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Models
{
    public class TipoModel
    {
        public List<SiTipoComportamientoDTO> ListaTipos { get; set; }

        public int Tipocodi { get; set; }
        public string Tipoprincipal { get; set; }
        public string Tipotipagente { get; set; }
        public string Tipodocsustentatorio { get; set; }
        public string Tipoarcdigitalizado { get; set; }
        public string Tipoarcdigitalizadofilename { get; set; }
        public string Tipopotenciainstalada { get; set; }
        public string Tiponrocentrales { get; set; }
        public string Tipolineatrans500 { get; set; }
        public string Tipolineatrans220 { get; set; }
        public string Tipolineatrans138 { get; set; }
        public string Tipolineatrans500km { get; set; }
        public string Tipolineatrans220km { get; set; }
        public string Tipolineatrans138km { get; set; }
        public string Tipototallineastransmision { get; set; }
        public string Tipomaxdemandacoincidente { get; set; }
        public string Tipomaxdemandacontratada { get; set; }
        public string Tiponumsuministrador { get; set; }
        public string Tipousucreacion { get; set; }
        public DateTime? Tipofeccreacion { get; set; }
        public string Tipousumodificacion { get; set; }
        public DateTime? Tipofecmodificacion { get; set; }
        public int? Tipoemprcodi { get; set; }
        public int? Emprcodi { get; set; }

        public HttpPostedFileBase ArchivoDigitalizado { get; set; }
                
    }

    public class TipoPrincipalSecundarioModel
    {
        public string TipoPrincipalSecundarioCodigo { get; set; }
        public string TipoPrincipalSecundarioDescripcion { get; set; }
        public TipoPrincipalSecundarioModel() { }
        public TipoPrincipalSecundarioModel(string pTipoPrincipalSecundarioCodigo, string pTipoPrincipalSecundarioDescripcion)
        {
            TipoPrincipalSecundarioCodigo = TipoPrincipalSecundarioCodigo;
            TipoPrincipalSecundarioDescripcion = pTipoPrincipalSecundarioDescripcion;
        }
    }
}