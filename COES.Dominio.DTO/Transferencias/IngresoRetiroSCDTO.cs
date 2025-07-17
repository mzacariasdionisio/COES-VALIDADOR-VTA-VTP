using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_ING_RETIROSC
    /// </summary>
    public class IngresoRetiroSCDTO
    {
        public System.Int32 IngrscCodi { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.Int32 EmprCodi { get; set; }
        public System.Int32 IngrscVersion { get; set; }
        public System.Decimal IngrscImporte { get; set; }
        public System.Decimal IngrscImporteVtp { get; set; }
        public System.String IngrscUserName { get; set; }
        public System.DateTime IngrscFecIns { get; set; }
        public System.DateTime IngrscFecAct { get; set; }
        public System.String EmprNombre { get; set; }
        public System.String PeriNombre { get; set; }
        public System.Decimal Total { get; set; }

    }
}
