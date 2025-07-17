using COES.MVC.Intranet.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TiempoReal.Helper
{
    public class XMLDocument
    {        
        /// <summary>
        /// Permite generar el archivo excel con las interrupciones de un evento
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarArchivoXML(string nombreArchivo, string tags)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal + nombreArchivo;
                
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }
                
                try
                {
                    if (!newFile.Exists)
                    {
                        using (StreamWriter sw = newFile.CreateText())
                        {
                            sw.WriteLine(tags);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
