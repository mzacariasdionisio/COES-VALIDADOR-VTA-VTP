using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_VARIACION_CODIGO
    /// </summary>
    [Serializable]
    public class VtpVariacionCodigoDTO: EntityBase
    {
        public int VarCodCodi { get; set; }
        public int EmprCodi { get; set; }
        public int BarrCodi { get; set; }
        public string VarCodCodigoVtp { get; set; }
        public decimal VarCodPorcentaje { get; set; }
        public string VarCodEstado { get; set; }
        public string VarCodUsuCreacion { get; set; }
        public DateTime VarCodFecCreacion { get; set; }
        public string VarCodUsuModificacion { get; set; }
        public DateTime VarCodFecModificacion { get; set; }
        public string VarCodTipoComp { get; set; }

        /// Para listas y filtros
        public string EmprNomb { get; set; }
        public int Fila { get; set; }
        public int CliCodi { get; set; }
        public string Cliente { get; set; }
        public string Barra { get; set; }
        public string CodCnCodiVtp { get; set; }
        
    }
}
