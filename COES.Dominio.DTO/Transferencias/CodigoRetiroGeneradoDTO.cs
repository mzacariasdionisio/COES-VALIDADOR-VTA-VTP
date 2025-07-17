using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_CODIGO_RETIRO_GENERADO
    /// </summary>

    public class CodigoRetiroGeneradoDTO
    {
        public string TipCasaAbrev { get; set; }

        public System.Int32 CoresoCodi { get; set; }
        public System.Int32 indexBarra { get; set; }
        public System.String CoresoCodigoVTA { get; set; }
        public System.Int32 GenemprCodi { get; set; }
        public System.Int32 CliemprCodi { get; set; }
        public System.Int32 CoregeCodi { get; set; }
        public System.Int32 CoresdcCodi { get; set; }
        public System.Int32 PeridcCodi { get; set; }
        public System.Int32 PeridcAnio { get; set; }
        public System.Int32 PeridcMes { get; set; }
        public System.Int32 BarrCodiTra { get; set; }
        public System.Int32 BarrCodiSum { get; set; }
        public System.String BarrNombre { get; set; }
        public System.String CoregeCodVTP { get; set; }
        public System.String CoregeEstado { get; set; }
        public System.String EstdAbrev { get; set; }
        public System.String EstdDescripcion { get; set; }

        public DateTime? SoliCodiRetiFechaInicio { get; set; }
        public DateTime? SoliCodiRetiFechaFin { get; set; }
        /// Para las vistas
        public string Emprnomb { get; set; }
        public string Tipconnombre { get; set; }
        public string Tipusunombre { get; set; }
        public decimal Pegrdpotecalculada { get; set; }
        public decimal Pegrdpotecoincidente { get; set; }
        public decimal Pegrdpotedeclarada { get; set; }
        public decimal Pegrdpeajeunitario { get; set; }
        public decimal Pegrdfacperdida { get; set; }
        public string Pegrdcalidad { get; set; }
        public Int32 TrnPcTipoPotencia { get; set; }
        public string Username { get; set; }

        // Potencia Contrato
        public int OmitirFila { get; set; }

        public SolicitudCodigoPotenciaContratadaDTO PotenciaContratadaDTO { get; set; }

    }
}
