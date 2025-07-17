using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class ExportExcelDTO
    {
        public System.Int32 CodiEntreRetiCodi { get; set; }
        public System.String EmprNomb { get; set; }        
        public System.String BarrNombBarrTran{ get; set; }
        public System.String CodiEntreRetiCodigo { get; set; }
        public System.String Tipo { get; set; }
        public System.String CentGeneCliNombre { get; set; }
        public System.Int32 EmprCodi { get; set; }
        public System.Int32 BarrCodi { get; set; }
        //CUADRO
        public System.Decimal ValorizacionTransferencia { get; set; }
        public System.Decimal SaldoTransmision { get; set; }
        public System.Decimal Compensacion { get; set; }
        public System.Decimal TotalEmp { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.Int32 VtranVersion { get; set; }
        public System.Decimal SaldoCodigoRetiroSC { get; set; }
        //Cuadro-compensacion
        public System.Int32 IngComVersion { get; set; }
        public System.Decimal IngComImporte { get; set; }
        public System.String CabComNombre { get; set; }
        //InformacionBase
        //Para Informacion Base
        public System.Int32 CoInfbCodi { get; set; }
        public System.String CoInfbCodigo { get; set; }
    }
}
