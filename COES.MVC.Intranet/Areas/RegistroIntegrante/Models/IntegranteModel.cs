using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Models
{
    public class IntegranteModel
    {
        public int NroIteracion { get; set; }
        public int TipoIntegrante { get; set; }
        public int RazonSocial { get; set; }
        public int NombreComercial { get; set; }
        public int Sigla { get; set; }
        public int FechaSolicitud { get; set; }
        public int HorasTranscurridas { get; set; }
        public int FlgSGI { get; set; }
        public int FlgDJR { get; set; }
    }
}