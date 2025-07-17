using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.ValorizacionDiaria.Models
{
    public class ArchivoModel
    {
        public string Nombre { get; set; }
        public List<MeHojaptomedDTO> ListaHojaPto { get; set; }
        public int Empresa { get; set; }
        public DateTime FechaProceso { get; set; }
        public MeFormatoDTO Formato { get; set; }
    }

}