using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class ResultadoDTO<T>
    {
        public int EsCorrecto { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }
    }
}
