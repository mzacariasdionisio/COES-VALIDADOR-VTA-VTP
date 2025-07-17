using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Models
{
    public class ConsultasModel
    {
        public List<CoPeriodoDTO> ListaPeriodo { get; set; }
        public List<CoMedicion48DTO> ListadoURS { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string[][] Data { get; set; }
        public int Indicador { get; set; }

    }
}