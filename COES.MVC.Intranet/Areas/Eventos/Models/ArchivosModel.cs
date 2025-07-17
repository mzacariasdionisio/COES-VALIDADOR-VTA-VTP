using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class ArchivosModel
    {
        public string Fecha { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
        public List<FileData> ListaDocumentosFiltrado { get; set; }
    }
}