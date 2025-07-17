using System.ServiceProcess;

namespace COES.Service.Tawa
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServicioTawa()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
