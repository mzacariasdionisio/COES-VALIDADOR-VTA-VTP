using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COES.Job.CostoOportunidad.Helper
{
    /// <summary>
    /// Interface de manejo de errores
    /// </summary>
    public interface ILogger
    {
        void Info(string text);
        void Debug(string text);
        void Warn(string text);
        void Error(string text);
        void Error(string text, Exception ex);
    }
}
