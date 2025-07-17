using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Models
{
    public class ProcesoModel
    {
        public List<CoPeriodoDTO> ListaPeriodo { get; set; }
        public List<CoVersionDTO> ListaVersion { get; set; }  
        public string[][] DatosPrograma { get; set; }
        public string[][] DatosProgramaSinReserva { get; set; }
        public string[][] DatosRAProgramadaUp { get; set; }
        public string[][] DatosRAProgramadaDown { get; set; }
        public string[][] DatosRAEjecutadaUp { get; set; }
        public string[][] DatosRAEjecutadaDown { get; set; }
        public string[][] DatosDespacho { get; set; }
        public string[][] DatosDespachoSinR { get; set; }
        public int[][] ColoresDespacho { get; set; }
        public int[][] ColoresDespachoSinR { get; set; }
        public int Indicador { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<Dominio.DTO.Transferencias.PeriodoDTO> ListaPeriodoTrn { get; set; }      
        public List<CoEnvioliquidacionDTO> ListaEnvios { get; set; }
        public string VersionNombre { get; set; }
        public string PeriodoNombre { get; set; }
        public int IdPeriodoTrn { get; set; }
        public List<CoRaejecutadadetDTO> ListaRADetalle { get; set; }

    }
}