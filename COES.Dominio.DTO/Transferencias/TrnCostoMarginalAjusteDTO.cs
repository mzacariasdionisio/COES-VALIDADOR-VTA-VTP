using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class TrnCostoMarginalAjusteDTO
    {
        public int Trncmacodi { get; set; }
        public int Pericodi { get; set; }
        public int Recacodi { get; set; }
        public DateTime Trncmafecha { get; set; }
        public string Trncmausucreacion { get; set; }
        public DateTime Trncmafeccreacion { get; set; }
        public string Trncmausumodificacion { get; set; }
        public DateTime Trncmafecmodificacion { get; set; }

        //Adicionales
        public string TrncmafechaStr { get; set; }
        public string TrncmafeccreacionStr { get; set; }
        public String TrncmafecmodificacionStr { get; set; }
    }
}
