using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Enum
{
    public enum ETipoProcesoAuditoriaVTEA : int
    {
        ImportarCostosMarginales = 2,
        ProcesarValorizacionProcesar = 5,
        ProcesarValorizacionBorrar = 6,
        CreacionMesValorizacion = 7,
        ProcesarVentasCongestion = 8,
        CargaInformacionVTEAExtranet = 10,
        CargaInformacionVTEAIntranet = 12,
        EjecutarSaldosSobrantes = 15
    }
}
