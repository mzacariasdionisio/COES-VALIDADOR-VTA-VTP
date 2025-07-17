using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CpCategoriaDTO
    {
        public int Catcodi { get; set; }
        public string Catnombre { get; set; }
        public string Catprefijo { get; set; }
        public string Catmatrizgams { get; set; }
        public string Catdescripcion { get; set; }
        public string Catabrev { get; set; }
        public int Total { get; set; }
    }
}
