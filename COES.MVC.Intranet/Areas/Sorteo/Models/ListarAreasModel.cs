using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Sorteo.Models
{
    public class ListarAreasModel
    {
        public List<SiListarAreasDTO> ListarAreas { get; set; }
        public int equicodi { get; set; }
        public string emprnomb { get; set; }
        public string areanomb { get; set; }
        public string equiabrev { get; set; }
        public int? equipadre { get; set; }
    }
}