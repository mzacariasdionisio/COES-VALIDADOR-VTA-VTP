using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class PeriodoModel
    {
        public List<PeriodoDTO> ListaPeriodo { get; set; }
        public List<PeriodoDeclaracionDTO> ListaPeriodoDeclaracion { get; set; }
        public PeriodoDTO Entidad { get; set; }
        public int IdPeriodo { get; set; }
    }
}