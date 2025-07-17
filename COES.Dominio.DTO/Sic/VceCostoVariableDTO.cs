using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class VceCostoVariableDTO
    {
        public decimal? Crcvcvtbajaefic { get; set; }
        public decimal? Crcvcvcbajaefic { get; set; }
        public decimal? Crcvconsumobajaefic { get; set; }
        public decimal? Crcvpotenciabajaefic { get; set; }
        public string Crcvaplicefectiva { get; set; }
        public int? Barrcodi { get; set; }
        public decimal? Crcvcvt { get; set; }
        public decimal? Crcvcvnc { get; set; }
        public decimal? Crcvcvc { get; set; }
        public decimal? Crcvprecioaplic { get; set; }
        public decimal? Crcvconsumo { get; set; }
        public decimal? Crcvpotencia { get; set; }
        public DateTime Crcvfecmod { get; set; }
        public int Grupocodi { get; set; }
        public int PecaCodi { get; set; } 

        //Adicional
        public string Gruponomb { get; set; }
        public string Dia { get; set; }
        public string Crdcgprecioaplicunid { get; set; }
        public string Barrbarratransferencia { get; set; }

    }
}
