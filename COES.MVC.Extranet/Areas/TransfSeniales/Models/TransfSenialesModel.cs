using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Scada;
using COES.Servicios.Aplicacion.TransfSeniales;
using COES.MVC.Extranet.Models;
using COES.Dominio.DTO.Sp7;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Extranet.Areas.TransfSeniales.Models
{
    public class TransfSenialesModel : FormatoModel
    {

        public int Emprcodi { get; set; }
        public string FechaReporte { get; set; }
        public string FechaPeriodo { get; set; }
        public string fecha { get; set; }

        public DateTime fechahoraactual { get; set; }
        public int CRC { get { return fechahoraactual.Month*500 + fechahoraactual.Day*60 + fechahoraactual.Hour*50 
                    + fechahoraactual.Minute*(fechahoraactual.Minute % 10+1)*20 + fechahoraactual.Second * (fechahoraactual.Second % 3 + 1) * 10 + (new Random(fechahoraactual.Second)).Next(10); } }       
        
    
        public List<ScEmpresaDTO> GetListaEmpresaRis7 { get; set; }  
        public List<TrMuestrarisSp7DTO> List { get; set; }
        public List<TrMuestrarisSp7DTO> GetMuestraRis7 { get; set; }
        public List<TrIndempresatSp7DTO> GetDispMensual7 { get; set; }
        public List<TrReporteversionSp7DTO> GetDispMensual7Version { get; set; }
        public ScEmpresaDTO GetInfoEmpresa7 { get; set; }
        public List<TrEstadcanalSp7DTO> GetDispDiaSignal7 { get; set; }
        public List<TrEstadcanalrSp7DTO> GetDispDiaSignal7Version { get; set; }
        public SiEmpresaDTO empresa { get; set; }


        //GetDispMensual7

        public int Media { get; set; }
        public int Factor { get; set; }
        public int Media2 { get; set; }
        public int Factor2 { get; set; }
        public int Findispon { get; set; }
        public int Ciccpe { get; set; }
        public int? Ciccpea { get; set; }
        public int? Factorg { get; set; }
        public DateTime? Lastdate { get; set; }
        public double disponibilidad { get; set; }
       
        
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }


        public int Version { get; set; }

        public List<SiEmpresaDTO> GetListaEmpresaRi7 { get; set; }
    }

   
}