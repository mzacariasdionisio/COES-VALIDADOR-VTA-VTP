using Microsoft.Graph;
using File = System.IO.File;

namespace COES.WebAPI.HTrabajo.Helper
{
    public class UtilArchivo
    {
        public static void CopiarArchivoTemporal(string carpetaTemporal, string nombreArchivo)
        {
            string pathOrigen = carpetaTemporal + "Htrabajo_generación_YYYYMMDD.xlsm";
            string pathDestino = carpetaTemporal + nombreArchivo;
            File.Copy(pathOrigen, pathDestino);
        }

        public static void EliminarArchivoTemporal(string carpetaTemporal, string prefijoArchivo)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(carpetaTemporal);
            if (directoryInfo.Exists)
            {
                foreach (FileInfo file in directoryInfo.GetFiles().OrderBy(x => x.Name))
                {
                    if (file.Name.ToLower().StartsWith(prefijoArchivo.ToLower()))
                        File.Delete(file.FullName);
                }
            }
        }
    }
}
