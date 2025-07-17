using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Models
{
    public class InsumosModel
    {
        public List<RePeriodoDTO> ListaPeriodo { get; set; }
        public int Anio { get; set; }
        public List<RePtoentregaPeriodoDTO> ListaPtoEntrega { get; set; }
        public RePtoentregaPeriodoDTO EntidadPtoEntrega { get; set; }
        public List<RePuntoEntregaDTO> ListaMaestroPtoEntrega { get; set; }
        public List<ReIngresoTransmisionDTO> ListaIngresos { get; set; }
        public List<ReEmpresaDTO> ListaEmpresa { get; set; }
        public ReIngresoTransmisionDTO EntidadIngreso { get; set; }
        public List<ReEventoPeriodoDTO> ListaEventos { get; set; }
        public ReEventoPeriodoDTO EntidadEvento { get; set; }

        public List<ReEmpresaDTO> ListSuministradores { get; set; }
        public List<RePtoentregaSuministradorDTO> ListadoSuministradorPeriodo { get; set; }
        public string Evento { get; set; }
        public string FechaEvento { get; set; }
        public int Empresa1 { get; set; }
        public decimal Porcentaje1 { get; set; }
        public int? Empresa2 { get; set; }
        public decimal? Porcentaje2 { get; set; }
        public int? Empresa3 { get; set; }
        public decimal? Porcentaje3 { get; set; }
        public int? Empresa4 { get; set; }
        public decimal? Porcentaje4 { get; set; }
        public int? Empresa5 { get; set; }
        public decimal? Porcentaje5 { get; set; }
        public string Comentario { get; set; }
        public int CodigoEvento { get; set; }
        public int CodigoPeriodo { get; set; }
        public bool Grabar { get; set; }
    }
}