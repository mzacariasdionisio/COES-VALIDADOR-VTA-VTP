using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WScoes;

namespace ExtranetConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LogService logService = new LogService();
            logService.nf_add_log("EXTRANET", "login", "LOGIN", "inválida", "FW_USER", null, null, "127.0.0.1", "Oracle", -1);
        }
    }
}
