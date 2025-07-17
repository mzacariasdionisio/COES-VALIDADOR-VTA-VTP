using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.GMM.Models
{
    public class GarantiaModel
    {
        public GmmGarantiaDTO mensajeProcesamiento { get; set; }
        
        public int anho { get; set; }
        public int mes { get; set; }
        public GarantiaModel()
        {
            mensajeProcesamiento = new GmmGarantiaDTO();
        }

        public List<GmmGarantiaDTO> listaGarantias { get; set; }

        public string Mensaje2 { get; set; }
    }
}