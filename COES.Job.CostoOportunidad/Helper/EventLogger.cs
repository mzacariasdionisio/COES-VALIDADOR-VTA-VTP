using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace COES.Job.CostoOportunidad.Helper
{
    public class EventLogger: ILogger
    {
        /// <summary>
        /// Identificador del aplicativo
        /// </summary>
        string Aplicacion = "Tareas.HTrabajo";

        /// <summary>
        /// Graba un mensaje informativo
        /// </summary>
        /// <param name="text"></param>
        public void Info(string text)
        {
            EventLog.WriteEntry(this.Aplicacion, text, EventLogEntryType.Information);
        }

        /// <summary>
        /// Graba un mensaje informativo
        /// </summary>
        /// <param name="text"></param>
        public void Debug(string text)
        {
            EventLog.WriteEntry(this.Aplicacion, text, EventLogEntryType.Information);
        }

        /// <summary>
        /// Graba un mensaje de alerta
        /// </summary>
        /// <param name="text"></param>
        public void Warn(string text)
        {
            EventLog.WriteEntry(this.Aplicacion, text, EventLogEntryType.Warning);
        }

        /// <summary>
        /// Graba un mensaje de error
        /// </summary>
        /// <param name="text"></param>
        public void Error(string text)
        {
            EventLog.WriteEntry(this.Aplicacion, text, EventLogEntryType.Error);
        }

        /// <summary>
        /// Graba un mensaje de error
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ex"></param>
        public void Error(string text, Exception ex)
        {
            Error(text);
            Error(ex.StackTrace);
        }
    }
}
