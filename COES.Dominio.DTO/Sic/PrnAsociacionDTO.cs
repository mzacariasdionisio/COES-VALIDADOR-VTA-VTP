using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnAsociacionDTO
    {
        public int Asociacodi { get; set; }
        public string Asocianom { get; set; }
        public string Asociatipomedi { get; set; }

        //adicionales
        public int Asodetcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public List<int> Detalle { get; set; }

        public string Asodettipomedi { get; set; }
    }
}
