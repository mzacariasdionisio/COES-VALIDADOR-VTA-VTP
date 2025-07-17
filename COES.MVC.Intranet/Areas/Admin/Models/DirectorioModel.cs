using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class DirectorioModel
    {
        public List<DirectorioServicio.Area> ListaAreas { get; set; }
        public List<DirectorioServicio.Directorio> ListaDirectorio { get; set; }
        public int Indicador { get; set; }
        public bool IndicadorGrabar { get; set; }
        public int IdDirectorio { get; set; }
        public string Nombre { get; set; }
        public string Anexo { get; set; }
        public string IndAnexo { get; set; }
        public string IndExtranet { get; set; }
    }
}