using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Intervenciones.Models
{
    public class ArchivosModel
    {
        public string Fecha { get; set; }
        public List<EqFamiliaDTO> ListaFamilias { get; set; }
        public string Famabrev { get; set; }
        public string Famnomb { get; set; }
        public string Famestado { get; set; }
        public int Famcodi { get; set; }

        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
    }
}