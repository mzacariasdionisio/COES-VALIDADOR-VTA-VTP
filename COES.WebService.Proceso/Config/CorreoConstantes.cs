using System;
using System.Configuration;

namespace COES.WebService.Proceso.Config
{
    public static class CorreoConstantes
    {
        public static int CodigoCorreoAnalisisFallaAlertaCitacion = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAnalisisFallaAlertaCitacion"]);
        public static int CodigoCorreoAlertaElaboracionInformeCtaf = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeCtaf"]);
        public static int CodigoCorreoAlertaElaboracionInformeCtafMasDosDiasHabiles = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeCtafMasDosDiasHabiles"]);
        public static int CodigoCorreoAlertaElaboracionInformeTecnico = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeTecnico"]);
        public static int CodigoCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles"]);
        public static int CodigoCorreoAlertaElaboracionInformeTecnicoSemanal = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoCorreoAlertaElaboracionInformeTecnicoSemanal"]);
    }
}