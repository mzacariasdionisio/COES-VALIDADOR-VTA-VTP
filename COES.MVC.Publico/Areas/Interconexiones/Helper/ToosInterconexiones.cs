using COES.MVC.Publico.Areas.Interconexiones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace COES.MVC.Publico.Areas.Interconexiones.Helper
{

    public class ToolsInterconexiones
    {
        
        /// <summary>
        /// Devuelve lista para combo Version en los reportes
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerListaVersion()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "FINAL" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "PRELIMINAR" };
            lista.Add(elemento);           
            return lista;
        }
      
        /// <summary>
        /// Devuelve lista para combo Horizonte en los reportes
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerListaHorizonte()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 71, NombreTipoInfo = "DIARIO" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 72, NombreTipoInfo = "SEMANAL" };
            lista.Add(elemento);                      
            return lista;

        }

        /// <summary>
        /// Devuelve lista para cobo tipo de Operacion en Contratos
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerListaTipoOpContrato()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "IMPORTACIÓN" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "EXPORTACIÓN" };
            lista.Add(elemento);
            return lista;

        }

        /// <summary>
        /// Devuelve lista para combo de tipo de Copia
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerListaTipoCopia()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "Contrato" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "Anexo 1" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 3, NombreTipoInfo = "Anexo 2" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 4, NombreTipoInfo = "Acuerdo de intercambio" };
            lista.Add(elemento);
            return lista;
        }

        /// <summary>
        /// Devuelve lista para combo  de horas
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerListaHoras()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 0, NombreTipoInfo = "00:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "01:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "02:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 3, NombreTipoInfo = "03:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 4, NombreTipoInfo = "04:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 5, NombreTipoInfo = "05:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 6, NombreTipoInfo = "06:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 7, NombreTipoInfo = "07:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 8, NombreTipoInfo = "08:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 9, NombreTipoInfo = "09:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 10, NombreTipoInfo = "10:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 11, NombreTipoInfo = "11:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 12, NombreTipoInfo = "12:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 13, NombreTipoInfo = "13:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 14, NombreTipoInfo = "14:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 15, NombreTipoInfo = "15:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 16, NombreTipoInfo = "16:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 17, NombreTipoInfo = "17:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 18, NombreTipoInfo = "18:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 19, NombreTipoInfo = "19:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 20, NombreTipoInfo = "20:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 21, NombreTipoInfo = "21:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 22, NombreTipoInfo = "22:00" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 22, NombreTipoInfo = "23:00" };
            lista.Add(elemento);
            return lista;

        }
    }
   
}