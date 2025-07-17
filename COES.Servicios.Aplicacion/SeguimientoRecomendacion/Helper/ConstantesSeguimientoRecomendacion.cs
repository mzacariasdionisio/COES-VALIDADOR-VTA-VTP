using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.SeguimientoRecomendacion.Helper
{
    class ConstantesSeguimientoRecomendacion
    {

        //alarmas
        //al dia de vencimiento
        public const int ReporteDiaVencimiento = 1;
        //dia notificacionn por defecto luego de vencido una recomendación
        public const int DiaNotificacion = 7;
        //repeticion de alarma cada 7 dias
        public const int RepeticionAlarma = 7;


        //plantilla de correo próxima a vencer
        public const int PlantillaAlarmaAvencer = 99;
        //plantilla de correo vencido
        public const int PlantillaAlarmaVencido = 100;
        //plantilla de correo repetitivo (se envía cada n días, n=7 inicialmente)
        public const int PlantillaAlarmaCiclico = 101;

        public const int RolAseguramiento = 217; //aplicativo Seg. Recomendaciones

        public const string FormatoFecha = "dd/MM/yyyy";
        public const string RutaCorreo = "Uploads\\";



    }
}
