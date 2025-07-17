using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TiempoReal.Models
{
    public class EstadisticaModel
    {
        public int Anio { get; set; }
        public List<GenericoDTO> ListaTipoArchivo { get; set; }
        public MeEnvioDTO Envio { get; set; }

        public string FileName { get; set; }
        public FileData Documento { get; set; }
        public List<FileData> ListaDocumentos { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
    }
}