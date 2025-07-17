using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_SALDOS_SOBRANTES
    /// </summary>
    public class ActualizarTrasEmpFusionDTO
    {
        public Int32 SalsoCodi { get; set; }
        public Int32 EmprCodiOri { get; set; }
        public Int32 PeriCodiOri { get; set; }
        public Int32 RecaCodi { get; set; }
        public Decimal SalsoSaldoOri { get; set; }
        public Int32 SalsoTipOpe { get; set; }
        public Int32 EmprCodiDes { get; set; }
        public Decimal SalsoSaldoDes { get; set; }
        public String SalsoEstado { get; set; }
        public DateTime? SalsoFecProceso { get; set; }
        public String SalsoVTEAVTP { get; set; }
        public String SalsoUsuCreacion { get; set; }
        public DateTime? SalsoFecCreacion { get; set; }
        public String SalsoUsuModificacion { get; set; }
        public DateTime? SalsoFecModificacion { get; set; }
        public DateTime? SalsoFecMigracion { get; set; }

        public Int32 PeriCodiDes { get; set; }
        public string Mensaje { get; set; }

        public string DescEmpresaOrigen { get; set; }
        public string DescPeriodoOrigen { get; set; }
        public string DescVersionOrigen { get; set; }
        public string DescTipoOpe { get; set; }
        public string DescEmpresaDestino { get; set; }
        public string DescEstado { get; set; }
        public Decimal SalsoSaldoFinal { get; set; }

        public string DescEmpresaNI { get; set; }
        public string DescPeriodoNI { get; set; }
        public string DescVersionNI { get; set; }
        public Decimal SaldoNI { get; set; }
        public DateTime? FechaNI { get; set; }
    }
}
