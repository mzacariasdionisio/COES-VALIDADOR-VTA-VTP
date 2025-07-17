using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Models
{
    public class DirectorioModel
    {
        public List<DirectorioServicio.IntAreaDTO> ListaAreas { get; set; }
        public List<DirectorioServicio.IntDirectorioDTO> ListaDirectorio { get; set; }
        public int Indicador { get; set; }        
    }
}