using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PmoDatGndseDTO : EntityBase
    {
        public int PmGnd5Codi { get; set; }
        public int GrupoCodi { get; set; }
        public int GrupoCodiSDDP { get; set; }
        public string GrupoNomb { get; set; }
        public string PmGnd5STG { get; set; }
        public string PmGnd5SCN { get; set; }
        public int PmBloqCodi { get; set; }
        public decimal PmGnd5PU { get; set; }

        public int PmPeriCodi { get; set; }
        public string Stg { get; set; }
        public string Scn { get; set; }
        public string Lblk { get; set; }
        public string Pu { get; set; }

        #region NET 20190306
        public DateTime EnvioFecha { get; set; }

        public string strPmGnd5PU
        {
            get
            {
                if (PmGnd5PU.ToString().IndexOf('.') == -1)
                    return PmGnd5PU.ToString() + ".";
                else
                    return string.Format("{0:0.###}", PmGnd5PU);// 20190314 - NET: Levantamiento de observaciones de usuario
                                                                //return PmGnd5PU.ToString();

            }

        }
        #endregion
    }
}
