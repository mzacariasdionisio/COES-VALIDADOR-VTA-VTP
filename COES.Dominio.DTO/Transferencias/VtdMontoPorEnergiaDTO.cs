using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class VtdMontoPorEnergiaDTO
    {
        public int Emprcodi { get; set; }
        //commentary-agregado para el join de tablas
        public string Emprnomb { get; set; }
        //endcommentary
        public decimal Valdretiro { get; set; }
        public decimal Valdentrega { get; set; }
        public DateTime Valofecha { get; set; }
    }
}
