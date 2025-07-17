using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Models
{
    /// <summary>
    /// Periodo model
    /// </summary>
    public class PeriodoModel
    {
        public int Anio { get; set; }
        public List<RePeriodoDTO> Listado { get; set; }
        public RePeriodoDTO Entidad { get; set; }
        public List<RePeriodoDTO> ListaPadre { get; set; }
        public string TipoPeriodo { get; set; }
        public string PeriodoRevision { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int PeriodoPadre { get; set; }
        public int OrdenAnual { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal FactorCompensacion { get; set; }
        public string Estado { get; set; }
        public int Codigo { get; set; }
        public string DataEtapa { get; set; }
        public string Nombre { get; set; }
        public bool Grabar { get; set; }

        public List<ReEmpresaDTO> ListaSuministradores { get; set; }
    }
}