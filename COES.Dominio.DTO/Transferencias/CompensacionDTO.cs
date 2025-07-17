using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_CABE_COMPENSACION
    /// </summary>
    public class CompensacionDTO
    {
        public System.Int32 CabeCompCodi { get; set; }
        public System.String CabeCompNombre { get; set; }
        public System.String CabeCompVer { get; set; }
        public System.String CabeCompEstado { get; set; }
        public System.String CabeCompUserName { get; set; }
        public System.DateTime CabeCompFecIns { get; set; }
        public System.DateTime CabeCompFecAct { get; set; }
        public System.Int32 CabeCompPeriCodi { get; set; }
        public System.String CabeCompRentConge { get; set; }

    }
}
