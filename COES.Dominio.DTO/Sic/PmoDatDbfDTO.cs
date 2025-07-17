using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PmoDatDbfDTO : EntityBase
    {
        public int PmDbf5Codi { get; set; }
        public int? GrupoCodi { get; set; }
        public int? CodBarra { get; set; }
        public string NomBarra { get; set; }
        public string PmDbf5LCod { get; set; }
        public DateTime? PmDbf5FecIni { get; set; }
        public int? PmBloqCodi { get; set; }
        public decimal? PmDbf5Carga { get; set; }
        public int? PmDbf5ICCO { get; set; }

        public int? PeriCodi { get; set; }
        public string BCod { get; set; }
        public string BusName { get; set; }
        public string LCod { get; set; }
        public string Fecha { get; set; }
        public string Llev { get; set; }
        public string Load { get; set; }
        public string Icca { get; set; }

        #region NET 20190305
        public DateTime EnvioFecha { get; set; }
        public string NroSemana { get; set; }
        public int? GrupoCodiSDDP { get; set; }
        public string strPmDbf5Carga
        {
            get
            {
                if (PmDbf5Carga.ToString().IndexOf('.') == -1)
                    return PmDbf5Carga.ToString() + ".";
                else
                    return string.Format("{0:0.#}", PmDbf5Carga);

            }

        }
        #endregion
    }
}
