using System;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.Proteccion.Helper
{
    public class EvaluacionHelper
    {
        /// <summary>
        /// Descarga el archivo adjuntado desde el fileserver
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="rutaBase"></param>
        /// <param name="rutaArchivo"></param>
        /// <returns></returns>
        public byte[] GetBufferArchivoAdjunto(string fileName, string rutaBase, string rutaArchivo)
        {
            string pathAlternativo = GetPathPrincipal();

            string rutaDestino = string.Format("{0}\\{1}\\", rutaBase, rutaArchivo);

            if (FileServer.VerificarExistenciaFile(rutaDestino, fileName, pathAlternativo))
            {
                return FileServer.DownloadToArrayByte(rutaDestino + "\\" + fileName, pathAlternativo);
            }

            return null;
        }
        /// <summary>
        /// Permite obtener la carpeta principal de Intervenciones
        /// </summary>
        /// <returns></returns>
        public string GetPathPrincipal()
        {
            //- Definimos la carpeta raiz (termina con /)
            string pathRaiz = FileServer.GetDirectory();
            return pathRaiz;
        }

        /// <summary>
        /// Permite redondear un valor a un número de decimales y regresa un numero en formato string
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string RedondearValor(object valor, int numeroDecimales = 0)
        {
            double valorDouble = Convert.ToDouble(valor);

            string resultado = numeroDecimales > 0 ? Math.Round(valorDouble, numeroDecimales).ToString() : Math.Round(valorDouble).ToString("0");

            return resultado;
        }

    }
}