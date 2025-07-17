using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CompensacionRSF.Helper
{
    public class ConstantesCompensacionRSF
    {
        //RUTAS
        public const string ReporteDirectorio = "ReporteTransferencia";
        //MENSAJES
        public const string MensajeOkInsertarReistro = "La información ha sido correctamente registrada";
        public const string MensajeOkEditarReistro = "La información ha sido correctamente actualizada";
        public const string MensajeErrorGrabarReistro = "Se ha producido un error al grabar la información";
        public const string MensajeSoles = "Importes Expresados en Soles (S/)";

        public const int TipoCargaSubir = 1; //Subida
        public const int TipoCargaBajar = 2; //Bajada
        public const int Periodo2021 = 202101;
        public const int PeriodoFebrero2021 = 202102;
    }

    public class ConstantesEventos
    {
        public const string ExportacionRSFReporte = "ReservaAsignada.xlsx";
        
    }
}
