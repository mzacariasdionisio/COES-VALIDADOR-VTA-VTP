using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PmoDatMgndDTO : EntityBase
    {
        public int PmMgndCodi { get; set; }
        public DateTime? PmMgndFecha { get; set; }
        public string PmMgndFechaTexto { get { return PmMgndFecha.Value.ToString("dd/MM/yyyy"); } }
        public int? GrupoCodi { get; set; }
        public int? CodCentral { get; set; }
        public string NombCentral { get; set; }
        public int? CodBarra { get; set; }
        public string NombBarra { get; set; }
        public string PmMgndTipoPlanta { get; set; }
        public int? PmMgndNroUnidades { get; set; }
        public decimal? PmMgndPotInstalada { get; set; }
        public decimal? PmMgndFactorOpe { get; set; }
        public decimal? PmMgndProbFalla { get; set; }
        public decimal? PmMgndCorteOFalla { get; set; }

        public string Fecha { get; set; }
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
        public string strPmMgndPotInstalada
        {
            get
            {
                if (PmMgndPotInstalada.ToString().IndexOf('.') == -1)
                    return PmMgndPotInstalada.ToString() + ".";
                else
                    return PmMgndPotInstalada.ToString();

            }

        }
        public string strPmMgndFactorOpe
        {
            get
            {
                if (PmMgndFactorOpe.ToString().IndexOf('.') == -1)
                    return PmMgndFactorOpe.ToString() + ".";
                else
                    return PmMgndFactorOpe.ToString();

            }

        }
        public string strPmMgndProbFalla
        {
            get
            {
                if (PmMgndProbFalla.ToString().IndexOf('.') == -1)
                    return PmMgndProbFalla.ToString() + ".";
                else
                    return PmMgndProbFalla.ToString();

            }

        }

        #endregion

    }
}
