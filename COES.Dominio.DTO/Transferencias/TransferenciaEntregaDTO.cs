using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_TRANS_ENTREGA
    /// </summary>
    public class TransferenciaEntregaDTO
    {
        public System.Int32 TranEntrCodi { get; set; }
        public System.Int32 CodEntCodi { get; set; }
        public System.Int32 EmprCodi { get; set; }
        public System.Int32 BarrCodi { get; set; }
        public System.String CodiEntrCodigo { get; set; }
        public System.Int32 CentGeneCodi { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.Int32 TranEntrVersion { get; set; }
        public System.String TranEntrTipoInformacion { get; set; }
        public System.String TranEntrEstado { get; set; }
        public System.DateTime TranEntrFecIns { get; set; }
        public System.DateTime TranEntrFecAct { get; set; }
        public System.String TentUserName { get; set; }
        public System.String EmprNombre { get; set; }
        public System.String CentGeneNombre { get; set; }
        public System.String BarrNombre { get; set; }
        public System.Decimal Total { get; set; }
        /* ASSETEC 202001 */
        public System.Int32 TrnEnvCodi { get; set; }
    }

    public class TrnTransEntregaBullk
    {
        public System.Int32 Tentcodi { get; set; }
        public System.Int32 Codentcodi { get; set; }
        public System.Int32 Barrcodi { get; set; }
        public System.Int32 Pericodi { get; set; }
        public System.Int32 Emprcodi { get; set; }
        public System.Int32 Equicodi { get; set; }
        public System.String Tentcodigo { get; set; }
        public System.Int32 Tentversion { get; set; }
        public System.String Tenttipoinformacion { get; set; }
        public System.String Tentestado { get; set; }
        public System.String Tentusername { get; set; }
        public System.DateTime Tentfecins { get; set; }
        public System.DateTime Tentfecact { get; set; }

        public TrnTransEntregaBullk(TransferenciaEntregaDTO entity)
        {
            this.Tentcodi = entity.TranEntrCodi;
            this.Codentcodi = entity.CodEntCodi;
            this.Barrcodi = entity.BarrCodi;
            this.Pericodi = entity.PeriCodi;
            this.Emprcodi = entity.EmprCodi;
            this.Equicodi = entity.CentGeneCodi;
            this.Tentcodigo = entity.CodiEntrCodigo;
            this.Tentversion = entity.TranEntrVersion;
            this.Tenttipoinformacion = entity.TranEntrTipoInformacion;
            this.Tentestado = entity.TranEntrEstado;
            this.Tentusername = entity.TentUserName;
            this.Tentfecins = entity.TranEntrFecIns;
            this.Tentfecact = entity.TranEntrFecAct;

        }
    }
}
