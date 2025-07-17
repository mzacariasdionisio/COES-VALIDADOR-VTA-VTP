using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class PantallaResponsables
    {
        public List<Responsable> ListaResponsables { get; set; }

        public string RutaArchivosFirma { get; set; }

        public List<SiDirectorioDTO> ListaDirectorio { get; set; }
    }


}