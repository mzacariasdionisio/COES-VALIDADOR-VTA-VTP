using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_TRANSMISOR
    /// </summary>
    public class StBarraDTO : EntityBase
    {
        public int Stbarrcodi { get; set; }
        public int Strecacodi { get; set; }
        public int Barrcodi { get; set; }
        public string Stbarrusucreacion { get; set; }
        public DateTime Stbarrfeccreacion { get; set; }

        //agregados para consulta
        public string Barrnomb { get; set; }
    }
}
