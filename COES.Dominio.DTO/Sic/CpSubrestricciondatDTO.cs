using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CpSubrestricdatDTO
    {
        public Nullable<short> Srestcodi { get; set; }
        public decimal? Srestdvalor1 { get; set; }
        public decimal? Srestdvalor2 { get; set; }
        public decimal? Srestdvalor3 { get; set; }
        public decimal? Srestdvalor4 { get; set; }
        public Nullable<short> Srestdactivo { get; set; }
        public Nullable<short> Srestdopcion { get; set; }
        public int Topcodi { get; set; }
        public int Recurcodisicoes { get; set; }
        public string Recurnombre { get; set; }
        public int Recurcodi { get; set; }
        public short Catcodi { get; set; }
        public string Srestnombregams { get; set; }
        public DateTime? Srestfecha { get; set; }
        public string Catnombre { get; set; }
        public int Recurconsideragams { get; set; }

        #region Yupana Continuo
        public int Indiceorden { get; set; }
        #endregion
    }
}
