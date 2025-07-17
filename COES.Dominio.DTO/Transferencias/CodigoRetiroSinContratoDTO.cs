using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_CODIGO_RETIRO_SINCONTRATO
    /// </summary>
    public class CodigoRetiroSinContratoDTO
    {
        public System.Int32 CodRetiSinConCodi { get; set; }
        public System.Int32 CliCodi { get; set; }
        public System.String CliRuc { get; set; }
        public System.Int32 BarrCodi { get; set; }
        public System.String CodRetiSinConCodigo { get; set; }          
        public System.DateTime CodRetiSinConFechaInicio { get; set; }
        public System.DateTime? CodRetiSinConFechaFin { get; set; }   
        public System.String CodRetiSinConEstado { get; set; }
        public System.String CodRetiSinConUserName { get; set; }
        public System.DateTime CodRetiSinConFecIns { get; set; }
        public System.DateTime CodRetiSinConFecAct { get; set; }
        public System.Int32 GenEmprCodi { get; set; }        
        public System.String CliNombre { get; set; }
        public System.String BarrNombBarrTran { get; set; }
        public System.Int32 TipUsuCodi { get; set; }
        public System.String TipUsuNomb { get; set; }
    }
}
