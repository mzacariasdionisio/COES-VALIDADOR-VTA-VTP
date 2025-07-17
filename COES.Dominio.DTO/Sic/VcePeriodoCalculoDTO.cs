using System;
namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla VcePeriodoCalculo
    /// </summary>
    public class VcePeriodoCalculoDTO
    {
        public int PecaCodi { get; set; }
        public int PeriCodi { get; set; }
        public int PecaVersionComp { get; set; }
        public String PecaNombre { get; set; }
        public int PecaVersionVtea { get; set; }
        public decimal PecaTipoCambio { get; set; }
        public String PecaEstRegistro { get; set; }
        public String PecaUsuCreacion { get; set; }
        public DateTime PecaFecCreacion { get; set; }
        public String PecaUsuModificacion { get; set; }
        public DateTime PecaFecModificacion { get; set; }
        public int PeriAnioMes { get; set; }
        public String RecaNombre { get; set; }
        public string PecaMotivo { get; set; }
        public string PeriEstado { get; set; }
    }
}
