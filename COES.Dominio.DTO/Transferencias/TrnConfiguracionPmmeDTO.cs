using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class TrnConfiguracionPmmeDTO
    {
        public int Confconcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Emprcodi { get; set; }
        public DateTime? Fechavigencia { get; set; }
        public string Vigencia { get; set; }       
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public string EmprNomb { get; set; }
        public string PtoMediDesc { get; set; }       
    }
}
