using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class DepartamentoDTO
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public List<ProvinciaDTO> ListaProvincias { get; set; }


    }
}
