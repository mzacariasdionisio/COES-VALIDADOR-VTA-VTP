using COES.MVC.Intranet.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace COES.MVC.Intranet.Areas.Eventos.Helper
{
    public class EventoHelper
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(Tools));
        /// <summary>
        /// Permite verificar el acceso
        /// </summary>
        /// <param name="idOpcion"></param>
        /// <param name="userName"></param>
        /// <param name="idAccion"></param>
        /// <returns></returns>
        public bool VerificarAcceso(object idOpcion, string userName, int idAccion)
        {
            string appIISName = ConfigurationManager.AppSettings["AppIISName"];
            SeguridadServicio.SeguridadServicioClient cliente = new SeguridadServicio.SeguridadServicioClient();
            Boolean isCodigoOpcion = false;
            try
            {
                Uri uriRequest = HttpContext.Current.Request.UrlReferrer;
                String[] pathValidador = uriRequest.LocalPath.Replace(appIISName, "").Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                isCodigoOpcion = cliente.ValidarAccesoOpcion(pathValidador[1], pathValidador[2], Constantes.IdAplicacion, userName);

                log.Debug("PERMISO - debug: uriRequest.LocalPath => " + uriRequest.LocalPath);

                if (isCodigoOpcion == false)
                {
                    isCodigoOpcion = cliente.ValidarAccesoOpcion(pathValidador[0] + "/" + pathValidador[1], pathValidador[2], Constantes.IdAplicacion, userName);
                }
            }
            catch { }

            if (isCodigoOpcion == true)
            {
                return true;
            }
            else if (idOpcion != null)
            {
                return cliente.ValidarPermisoOpcion(Constantes.IdAplicacion, Convert.ToInt32(idOpcion), idAccion, userName);
            }

            return false;
        }
        /// Permite verificar el acceso
        /// </summary>
        /// <param name="idOpcion"></param>
        /// <param name="userName"></param>
        /// <param name="idAccion"></param>
        /// <returns></returns>
        public bool VerificarAccesoSinServicioSeguridad(object idOpcion, string userName, int idAccion)
        {
            SeguridadServicio.SeguridadServicioClient cliente = new SeguridadServicio.SeguridadServicioClient();
            
            if (idOpcion != null)
            {
                return cliente.ValidarPermisoOpcion(Constantes.IdAplicacion, Convert.ToInt32(idOpcion), idAccion, userName);
            }

            return false;
        }

        /// <summary>
        /// Inicializa las dimensiones de la matriz de valores del objeto excel web
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static string[][] InicializaMatrizExcel(int rowsHead, int nFil, int colsHead, int nCol)
        {
            string[][] matriz = new string[rowsHead + nFil][];
            for (int i = 0; i <= nFil; i++)
            {
                matriz[i] = new string[nCol + colsHead];
                for (int j = 0; j < nCol; j++)
                {
                    matriz[i][j] = string.Empty;
                }
            }
            return matriz;
        }
    }
}