using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la vista VW_TRN_RATIO_CUMPLIMIENTO
    /// </summary>
    public class RatioCumplimientoDTO
    {
        public System.Int32 EmprCodi { get; set; }
        public System.String EmprNomb { get; set; }
        public System.Int32 TipoEmprCodi { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.Decimal Requerido { get; set; }
        public System.Decimal Informado { get; set; }
        public System.Decimal Infofinal { get; set; }
        //public System.Decimal Cumplimiento { get; set; }

    }
}
