using System;
using System.Configuration;

namespace COES.Servicios.Distribuidos.Config
{
    public static class CorreoConstantes
    {
        public static int CodigoCorreoAnalisisFallaAlertaCitacion = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAnalisisFallaAlertaCitacion"]);
        public static int CodigoCorreoAlertaElaboracionInformeCtaf = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeCtaf"]);
        public static int CodigoCorreoAlertaElaboracionInformeCtafMasDosDiasHabiles = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeCtafMasDosDiasHabiles"]);
        public static int CodigoCorreoAlertaElaboracionInformeTecnico = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeTecnico"]);
        public static int CodigoCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles"]);
        public static int CodigoCorreoAlertaElaboracionInformeTecnicoSemanal = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeTecnicoSemanal"]);
        public static int CodigoCorreoAlertaDatosFrecuencia = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaDatosFrecuencias"]);
        public static int CodigoCorreoAlertaEventosFrecuencia = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaEventosFrecuencia"]);
        public static int CodigoCorreoAlertaReporteSegundosFaltantes = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaReporteSegundosFaltantes"]);
    }
}