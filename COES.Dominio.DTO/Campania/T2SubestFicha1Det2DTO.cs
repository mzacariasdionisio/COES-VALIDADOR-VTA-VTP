using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class T2SubestFicha1Det2DTO
    {
        public int SubestFicha1Det2Codi { get; set; }
        public int SubestFicha1Codi { get; set; }
        public int DataCatCodi { get; set; }
        public string Referencia { get; set; }
        public string NumTrafo { get; set; }
        public string ValorTrafo { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
    }
}
