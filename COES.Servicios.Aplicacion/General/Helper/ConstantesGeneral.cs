using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.General.Helper
{
    public class ConstantesGeneral
    {
        
    }

    public class NotificacionAplicativo
    {
        public const string KeyNotificacionFlagEnviarACOES = "NotificacionFlagEnviarACOES";
        public const string KeyNotificacionFlagEnviarAAgente = "NotificacionFlagEnviarAAgente";
        public const string KeyNotificacionFlagEnviarCCAdicional = "NotificacionFlagEnviarCCAdicional";
        public const string KeyNotificacionListaEmailCCAdicional = "NotificacionListaEmailCCAdicional";
        public const string KeyNotificacionPrefijoAsunto = "NotificacionPrefijoAsunto";
        public const string KeyEmailServer = "EmailServer";
        public const string KeyMailFrom = "MailFrom";

        public const string TipoMensajeLeido = "L";
        public const string TipoMensajeNoLeido = "N";

        public const int TipoComunicacionNinguna = 1;
        public const int TipoComunicacionTodoLeido = 2;
        public const int TipoComunicacionPendienteLeer = 3;
        public const int TipoComunicacionExisteMensaje = 4;
    }

    /// <summary>
    /// Notificacion de Nueva Empresa
    /// </summary>
    public class NotificacionEmpresa
    {
        public const string ListUsuarioTo = "";
        public const string ListUsuarioCC = "";
    }

    /// <summary>
    /// Mensajes utilizados en el registro de empresas
    /// </summary>
    public class MensajesEmpresa
    {
        public const string NombreEmpresaExiste = "El nombre de la empresa ya se encuentra registrado.";
        public const string RucEmpresaExiste = "El RUC ingresado pertenece a otra empresa.";
        public const string RucRequerido = "Debe ingresar el RUC de la empresa.";
        public const string RucInvalido = "Debe ingresar un RUC válido";
        public const string DocimiliadaNacional = "N";
        public const string DomiciliadaExtranjera = "E";
    }

    /// <summary>
    /// Mensajes para la parte de suscripcion
    /// </summary>
    public class MensajeSuscripcion
    {
        public const string AsuntoUsuario = "Confirmación de Suscripción a Publicaciones del COES.";
        public const string AsuntoAdministrador = "Se ha registrado una nueva Suscripciones a publicaciones del COES.";
        public const string EmailSubscripciones = "EmailSubscripciones";
        public const string AsuntoResponsable = "Se ha registrado una subscripción a la publicación {0}";
    }

}
