using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.YupanaContinuo.Models
{
    public class CompromisoHidraulicoModel
    {
        public bool TienePermisoAdmin { get; set; }        
        
        public string Resultado { get; set; }
        public string HtmlSC { get; set; }
        public string HtmlCC { get; set; }        
        public string HtmlCambiosSC { get; set; }
        public string HtmlCambiosCC { get; set; }
        public string Mensaje { get; set; }
        
        public int IdEnvioSC { get; set; }
        public int IdEnvioCC { get; set; }
        public int NumEnviosSC { get; set; }
        public int NumEnviosCC { get; set; }

        public string Fecha { get; set; }
        public string VersionFechaSC { get; set; }
        public string VersionFechaCC { get; set; }

        public bool EsUltimaVersionSC { get; set; }
        public bool EsUltimaVersionCC { get; set; }
    }    
}