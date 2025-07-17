using COES.Dominio.DTO.Sic;
using COES.Storage.App.Metadata.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Models
{
    public class PronosticoTempoRealModel
    {
        public string fecha { get; set; }
        public Double Rdo { get; set; }
        public Double PronosticoTiempoReal { get; set; }
        public Double PronosticoMinimo { get; set; }
        public Double PronosticoMaximo { get; set; }
        public Double Ejecutado { get; set; }
    }

}