using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class PeriodoDeclaracionModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public List<PeriodoDeclaracionDTO> ListaPeriodos { get; set; }
        public PeriodoDeclaracionDTO Entidad { get; set; }
        public int IdPeriodo { get; set; }

        public string Perifechavalorizacion { get; set; }
        public string Perifechalimite { get; set; }
        public string Perifechaobservacion { get; set; }
        public string sError { get; set; }
    }
}