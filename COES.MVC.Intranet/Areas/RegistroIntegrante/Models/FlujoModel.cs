using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Models
{
    public class FlujoModel
    {
        public int? Emprcodi { get; set; }
        public int? Solicodi { get; set; }
        public List<FlujoDetalleModel> ListaFlujo;

        public FlujoModel()
        {
            ListaFlujo = new List<FlujoDetalleModel>();
        }

    }

    public class FlujoDetalleModel
    {
        public int Emflcodi { get; set; }
        public int Fljcodi { get; set; }
        public DateTime? FechaOriginal { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public DateTime? FechaProceso { get; set; }
        public string Estado { get; set; }

        public string Observacion { get; set; }

        //public string DocumentoAdjuntoFileName { get; set; }
        //public string DocumentoAdjunto { get; set; }

        public decimal corrnumproc { get; set; }
        public int filecodi { get; set; }

        public string EnlaceArchivo { get; set; }
        
    }
}