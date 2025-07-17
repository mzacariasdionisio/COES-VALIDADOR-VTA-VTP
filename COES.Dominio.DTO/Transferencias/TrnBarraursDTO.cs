using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_BARRA_URS
    /// </summary>
    public class TrnBarraursDTO
    {
        public System.Int32 BarrCodi { get; set; }
        public System.Int32 GrupoCodi { get; set; }
        public System.Int32 EquiCodi { get; set; }
        public System.Int32 EmprCodi { get; set; }
        public System.String BarUrsUsuCreacion { get; set; }
        public System.DateTime BarUrsFecCreacion { get; set; }

        //atributos adicionales
        public System.String GrupoNomb { get; set; }
        public System.String EquiNomb { get; set; }
        public System.String EmprNomb { get; set; }
        
    }
}
