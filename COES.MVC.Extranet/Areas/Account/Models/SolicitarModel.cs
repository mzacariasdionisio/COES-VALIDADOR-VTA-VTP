using COES.MVC.Extranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Account.Models
{
    public class SolicitudUsuarioModel
    {
        public string EmpresaNombre { get; set; }
        public int EmpresaId { get; set; }
        public List<SolicitudExtDTO> ListaSolicitud { get; set; }
        public SolicitudExtDTO Entidad { get; set; }
        public int IdSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
    }

    public class ExcelModel
    {
        public string[] Headers { get; set; }
        public int[] Widths { get; set; }
        public object[] Columnas { get; set; }
        public string[][] Data { get; set; }

    }
}