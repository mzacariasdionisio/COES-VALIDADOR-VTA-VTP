using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class EprCalculosDTO
    {
        public int Equicodi { get; set; }
        public int Propcodi { get; set; }
        public string Identificador {  get; set; }  
        public string Parametro {  get; set; }
        public object Valor { get; set; }
        public string Formula { get; set; }
        public int Estado { get; set; }
        public string MensajeError { get; set; }

        public string TipoDato {  get; set; }
    }

    
}
