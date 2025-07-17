using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class ReduccionRedModel
    {

        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public string FecIni { get; set; }
        public string FecFin { get; set; }
        public List<PrnVersionDTO> ListVersion { get; set; }
        public List<PrGrupoDTO> ListBarra { get; set; }
        public List<PrGrupoDTO> ListBarraPopCP { get; set; }
        public List<PrGrupoDTO> ListBarraPopPM { get; set; }
        public List<PrGrupoDTO> ListBarraDefecto { get; set; }
        public List<PrnReduccionRedDTO> ListRed { get; set; }
        public PrnVersionDTO Version { get; set; }
        //adicionales
        public List<int> SelBarra { get; set; }

        //19032020
        public int flagPop { get; set; }
    }
}