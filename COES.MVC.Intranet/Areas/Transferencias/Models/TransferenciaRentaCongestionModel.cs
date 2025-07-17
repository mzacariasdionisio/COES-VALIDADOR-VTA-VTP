using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class TransferenciaRentaCongestionModel
    {
        public List<TransferenciaRecalculoDTO> ListaPeriodosRentaCongestion { get; set; }
        public decimal GetTotalRentaCongestion { get; set; }
        public decimal GetTotalRentaNoAsignada { get; set; }
        public List<TransferenciaRentaCongestionDTO> ListRentaCongestion { get; set; }

        public TransferenciaRecalculoDTO PeriodoRentaCongestion { get; set; }

        public List<TransferenciaRentaCongestionDTO> ListRentaCongestionDetalle { get; set; }
        public List<PeriodoDTO> ListaPeriodos { get; set; }
    }
}