using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Service.Proceso.Helper
{
    public interface ILogger
    {
        void Info(string text);
        void Debug(string text);
        void Warn(string text);
        void Error(string text);
        void Error(string text, Exception ex);
    }
}
