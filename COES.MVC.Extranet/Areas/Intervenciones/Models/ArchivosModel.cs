using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using System.Collections.Generic;

namespace COES.MVC.Extranet.Areas.Intervenciones.Models
{
    public class ArchivosModel
    {
        public string Fecha { get; set; }
        public List<EqFamiliaDTO> ListaFamilias { get; set; }
        public string Famabrev { get; set; }
        public string Famnomb { get; set; }
        public string Famestado { get; set; }
        public int Famcodi { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
        public List<FileData> ListaDocumentosFiltrado { get; set; }

        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
    }
}