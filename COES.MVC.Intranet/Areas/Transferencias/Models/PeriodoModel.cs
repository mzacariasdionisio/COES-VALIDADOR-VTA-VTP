using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Transferencias;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class PeriodoModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public List<PeriodoDTO> ListaPeriodos { get; set; }
        public PeriodoDTO Entidad { get; set; }
        public int IdPeriodo { get; set; }

        public string Perifechavalorizacion { get; set; }
        public string Perifechalimite { get; set; }
        public string Perifechaobservacion { get; set; }
        public string sError { get; set; }
        public List<PeriodoDeclaracionDTO> ListaPeriodoDeclaracion { get; set; }
        //ASSETEC 202108 TIEE
        public List<TrnMigracionDTO> ListaTrnMigracion { get; set; }
    }
}