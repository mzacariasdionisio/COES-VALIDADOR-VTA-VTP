using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Proteccion.Models
{
    public class CargaMasivaModel
    {        

        public List<EprCargaMasivaDTO> ListCargaMasiva { get; set; }

        public List<EprPropCatalogoDataDTO> ListTipoUso { get; set; }

        public List<EprCargaMasivaDetalleDTO> ListCargaMasivaDetalle { get; set; }

        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public string NombreArchivo { get; set; }
    }

}