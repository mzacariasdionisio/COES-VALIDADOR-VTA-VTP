using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.General.Helper
{
    public class ConstantesParametro
    {
        public const int IdParametroRangoSolar = 10;
        public const int IdParametroHPPotenciaActiva = 11;
        public const int IdParametroRangoPotenciaInductiva = 12;
        public const int IdParametroRangoPeriodoHP = 13;
        public const string EstadoActivo = "N";
        public const string EstadoBaja = "S";
        public const string EstadoAnulado = "X";
        public const string EstadoPendiente = "P";
        public const string ValorHoraInicioSolar = "HORA_INI";
        public const string ValorHoraFinSolar = "HORA_FIN";

        public const string ValorH1Ini = "H1_INI";
        public const string ValorH1Fin = "H1_FIN";
        public const string ValorH2Ini = "H2_INI";
        public const string ValorH2Fin = "H2_FIN";

        public const string ValorHoraMinimaHP = "HORA_MINIMA";
        public const string ValorHoraMediaHP = "HORA_MEDIA";
        public const string ValorHoraMaximaHP = "HORA_MAXIMA";

        //PR5

        public const int IdParametroMagnitudRPF = 14;
        public const string ValorPeriodoAvenida = "PRD_AV";
        public const string ValorPeriodoEstiaje = "PRD_ETJ";
        public const string DescPeriodoAvenida = "Período de Avenida";
        public const string DescPeriodoEstiaje = "Período de Estiaje";

        //Monitoreo
        public const int IdParametroTendenciaHHI = 17;
        public const string ValorMonitoreoTendenciaCero = "HHI_A_CERO";
        public const string ValorMonitoreoTendenciaUno = "HHI_A_UNO";

        public const int IdIndicePivotal = 18;
        public const string ValorMonitoreoIndicePivotal = "Valor Indice Pivotal";
    }
}
