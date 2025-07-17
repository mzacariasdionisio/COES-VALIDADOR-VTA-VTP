using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Eventos.Helper;
using System.IO;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class FileAnalisisEventosModel
    {
        public int Eveanaevecodi { get; set; }
        public HttpPostedFileBase[] Archivo { get; set; }
    }
}