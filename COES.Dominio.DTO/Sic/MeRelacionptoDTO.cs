using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_RELACIONPTO
    /// </summary>
    public class MeRelacionptoDTO : EntityBase
    {
        public int Relptocodi { get; set; }
        public int Ptomedicodi1 { get; set; }
        public int Ptomedicodi2 { get; set; }
        public int? Trptocodi { get; set; }
        public decimal? Relptofactor { get; set; }
        public int Lectcodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Tptomedicodi { get; set; }
        public string Ptomedinomb { get; set; }
        public int Equicodi { get; set; }
        public int Grupocodi { get; set; }
        public decimal? Relptopotencia { get; set; }
 
        #region Siosein2
        public int Relptotabmed { get; set; }
        #endregion

        #region SIOSEIN
        public int? Funptocodi { get; set; }
        public string Funptofuncion { get; set; }
        #endregion
    }
}
