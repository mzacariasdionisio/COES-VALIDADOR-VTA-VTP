using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_TRANS_RETIRO
    /// </summary>
    public class TransferenciaRetiroDTO
    {
        public System.Int32 TranRetiCodi { get; set; }
        public System.Int32 EmprCodi { get; set; }
        public System.Int32 BarrCodi { get; set; }
        public System.Int32 TRetCoresoCorescCodi { get; set; }
        public System.String SoliCodiRetiCodigo { get; set; }
        public System.Int32 CliCodi { get; set; }
        public System.String TretTabla { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.Int32 TranRetiVersion { get; set; }
        public System.String TranRetiTipoInformacion { get; set; }
        public System.String TranRetiEstado { get; set; }
        public System.DateTime TranRetiFecIns { get; set; }
        public System.String TretUserName { get; set; }
        public System.DateTime TranRetiFecAct { get; set; }
        public System.String EmprNombre { get; set; }
        public System.String CliNombre { get; set; }
        public System.String BarrNombre { get; set; }
        public System.Decimal Total { get; set; }
        /* ASSETEC 202001 */
        public System.Int32 TrnEnvCodi { get; set; }
    }

    public class TrnTransRetiroBullk
    {
        public System.Int32 Tretcodi { get; set; }
        public System.Int32 Pericodi { get; set; }
        public System.Int32 Barrcodi { get; set; }
        public System.Int32 Genemprcodi { get; set; }
        public System.Int32 Cliemprcodi { get; set; }
        public System.String Trettabla { get; set; }
        public System.Int32 Tretcoresocoresccodi { get; set; }
        public System.String Tretcodigo { get; set; }
        public System.Int32 Tretversion { get; set; }
        public System.String Trettipoinformacion { get; set; }
        public System.String Tretestado { get; set; }
        public System.String Tretusername { get; set; }
        public System.DateTime Tretfecins { get; set; }
        public System.DateTime Tretfecact { get; set; }

        public TrnTransRetiroBullk(TransferenciaRetiroDTO entity)
        {
            this.Tretcodi = entity.TranRetiCodi;
            this.Pericodi = entity.PeriCodi;
            this.Barrcodi = entity.BarrCodi;
            this.Genemprcodi = entity.EmprCodi;
            this.Cliemprcodi = entity.CliCodi;
            this.Trettabla = entity.TretTabla;
            this.Tretcoresocoresccodi = entity.TRetCoresoCorescCodi;
            this.Tretcodigo = entity.SoliCodiRetiCodigo;
            this.Tretversion = entity.TranRetiVersion;
            this.Trettipoinformacion = entity.TranRetiTipoInformacion;
            this.Tretestado = entity.TranRetiEstado;
            this.Tretusername = entity.TretUserName;
            this.Tretfecins = entity.TranRetiFecIns;
            this.Tretfecact = entity.TranRetiFecAct;

        }
    }
}
