using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COES.Dominio.DTO.Sic;
using System.Configuration;
using COES.Servicios.Aplicacion.Hidrologia.Helper;
using log4net;


namespace COES.Servicios.Aplicacion.Hidrologia
{
    public class NotificacionCambioPtoMedicion
    {
        public NotificacionCambioPtoMedicion()
        {

        }

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(NotificacionCambioPtoMedicion));

        public void NotificarCambio(int nroMinutos)
        {
            try
            {
                DateTime dtFechaInicio = DateTime.Now.AddMinutes(-1 * nroMinutos);
                DateTime dtFechaFin = DateTime.Now;

                List<MePtomedicionDTO> listaPuntos = Factory.FactorySic.GetMePtomedicionRepository().ListadoPtoMedicionModificados(dtFechaInicio, dtFechaFin);
                List<PrGrupoDTO> listaGrupos = Factory.FactorySic.GetPrGrupoRepository().ListadoGruposModificados(dtFechaInicio, dtFechaFin);

                if (listaPuntos.Count > 0 || listaGrupos.Count > 0)
                {
                    string mensaje = this.ObtenerCuerpoCorreoPuntos(listaPuntos, listaGrupos);

                    string sTo = ConfigurationManager.AppSettings["MedicionesNotificacionMailTo"];
                    string sBcc = ConfigurationManager.AppSettings["MedicionesNotificacionMailBcc"];
                    List<string> listTo = new List<string>();
                    listTo = sTo.Split(';').ToList();
                    List<string> listBCc = new List<string>();
                    listBCc = sBcc.Split(';').ToList();
                    List<string> listCC = new List<string>();

                    Base.Tools.Util.SendEmail(listTo, listCC, listBCc, "Notificación de Cambios en Mediciones", mensaje, "mediciones-noreply@coes.org.pe");
                    log.Info("El proceso Alerta Mediciones ejecutado a las  {0} ha sido existoso. " + dtFechaFin.ToString());
                }
                else
                {
                    log.Info("El proceso Alerta Mediciones ejecutado a las {0} no tuvo datos que enviar. "+ dtFechaFin.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("El proceso Alerta Mediciones ejecutado a las  {0} ha sido fallido. " + DateTime.Now,ex);
                throw ex;
            }
        }


        protected string ObtenerCuerpoCorreoPuntos(List<MePtomedicionDTO> listaPuntos, List<PrGrupoDTO> listaGrupos)
        {
            StringBuilder htmlPtoMedicion = new StringBuilder();
            StringBuilder htmlGrupo = new StringBuilder();

            foreach (MePtomedicionDTO punto in listaPuntos)
            {
                string sUsuario;
                DateTime? dFecha;


                sUsuario = punto.Lastuser;
                dFecha = punto.Lastdate;


                htmlPtoMedicion.Append(String.Format(HidrologiaHelper.Html.HtmlEquipo, punto.Ptomedibarranomb, punto.Ptomedielenomb, punto.Ptomedidesc, punto.Equinomb, punto.Gruponomb, punto.Emprnomb, punto.Tptomedinomb, punto.Origlectnombre,punto.Ptomediestado, sUsuario, dFecha));
            }

            foreach (PrGrupoDTO grupo in listaGrupos)
            {
                htmlGrupo.Append(String.Format(HidrologiaHelper.Html.HtmlGrupo, grupo.Grupocodi, grupo.Gruponomb, grupo.Grupoabrev, grupo.GruponombPadre, grupo.EmprNomb, grupo.Catenomb, grupo.Fenergnomb, grupo.DesTipoGrupo, grupo.TipoGenerRer, grupo.Osinergcodi, grupo.Grupointegrante, grupo.Grupotipocogen, grupo.Gruponodoenergetico, grupo.Gruporeservafria, grupo.Grupoactivo, grupo.GrupoEstado, grupo.Lastuser, grupo.Lastdate));
            }

            String mensaje = String.Format(HidrologiaHelper.Html.HtmlCuerpo, htmlPtoMedicion.ToString(), htmlGrupo.ToString());
            mensaje = mensaje.Replace("[", "{");
            mensaje = mensaje.Replace("]", "}");


            return mensaje;
        }

    }
}
