using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.SGDoc
{
    public class DocumentoDTO
    {
        public int Filecodi { get; set; }
        public string Fileruta { get; set; }
        public DateTime Lastdate { get; set; }
        public string Lastuser { get; set; }
        public string Filecomentario { get; set; }
        public int Fileanio { get; set; }
    }
}
