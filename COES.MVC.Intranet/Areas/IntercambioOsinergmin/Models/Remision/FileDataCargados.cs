using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Remision
{
    public class FileDataCargados
    {
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string Icono { get; set; }
        public string Extension { get; set; }
        public string LastWriteTime { get; set; }
        public int idEnvio { get; set; }
        public string tabla { get; set; }
    }
}