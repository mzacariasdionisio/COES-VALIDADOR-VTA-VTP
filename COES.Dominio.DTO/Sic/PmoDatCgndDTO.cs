using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PmoDatCgndDTO : EntityBase
    {
        public int? PmCgndCodi { get; set; }
        public int? GrupoCodi { get; set; }
        public int? PmCgndGrupoCodiBarra { get; set; }
        public string PmCgndTipoPlanta { get; set; }
        public int? PmCgndNroUnidades { get; set; }
        public decimal? PmCgndPotInstalada { get; set; }
        public decimal? PmCgndFactorOpe { get; set; }
        public decimal? PmCgndProbFalla { get; set; }
        public decimal? PmCgndCorteOFalla { get; set; }

        //adicionales list
        public int? CodCentral { get; set; }
        public string NombCentral { get; set; }
        public int? CodBarra { get; set; }
        public string NombBarra { get; set; }

        public string Num { get; set; }
        public string Name { get; set; }
        public string Bus { get; set; }
        public string Tipo { get; set; }
        public string Uni { get; set; }
        public string PotIns { get; set; }
        public string FatOpe { get; set; }
        public string ProbFal { get; set; }
        public string SFal { get; set; }

        #region 20190308 - NET: Adecuaciones a los archivos .DAT
        public string strPmCgndProbFalla
        {
            get
            {
                if (PmCgndProbFalla.ToString().IndexOf('.') == -1)
                    return PmCgndProbFalla.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmCgndProbFalla);

            }

        }

        public string strPmCgndFactorOpe
        {
            get
            {
                if (PmCgndFactorOpe.ToString().IndexOf('.') == -1)
                    return PmCgndFactorOpe.ToString() + ".";
                else
                    return PmCgndFactorOpe.ToString();

            }

        }

        public string strPmCgndPotInstalada
        {
            get
            {
                if (PmCgndPotInstalada.ToString().IndexOf('.') == -1)
                    return PmCgndPotInstalada.ToString() + ".";
                else
                    return PmCgndPotInstalada.ToString();

            }

        }
        
        #endregion
    }
}
