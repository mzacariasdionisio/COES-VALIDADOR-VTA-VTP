using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ProvinciaDTO
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string DepartamentoId { get; set; }

        public List <DistritoDTO> ListaDepartamentos { get; set; }
    }
}
