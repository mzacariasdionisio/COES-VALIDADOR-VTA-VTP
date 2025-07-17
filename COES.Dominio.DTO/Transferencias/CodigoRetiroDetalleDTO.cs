using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{   /// <summary>
    /// Clase que mapea la tabla VTP_CODIGO_RETIRO_SOL_DET
    /// </summary>
    public class CodigoRetiroDetalleDTO
    {
        public System.Int32 CoresdcCodi { get; set; }
        public System.Int32 BarrCodiSum { get; set; }
        public System.String BarrNombreSuministro { get; set; }
        public System.Int32 coresdRegistros { get; set; }
        
    }
}
