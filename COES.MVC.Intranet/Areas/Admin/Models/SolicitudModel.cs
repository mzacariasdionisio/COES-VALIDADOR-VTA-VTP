using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class SolicitudModel
    {
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<SolicitudExtDTO> ListaSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public int IdSolicitud { get; set; }
        public string EmpresaNombre { get; set; }
        public List<UserDTO> ListaUsuarios { get; set; }
    }

    public class ExcelModel
    {
        public string[] Headers { get; set; }
        public int[] Widths { get; set; }
        public object[] Columnas { get; set; }
        public string[][] Data { get; set; }

    }
}