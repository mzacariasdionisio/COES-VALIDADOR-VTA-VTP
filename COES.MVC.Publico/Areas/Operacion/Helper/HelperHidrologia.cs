using COES.MVC.Publico.Areas.Operacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.FormatoMedicion;


namespace COES.MVC.Publico.Areas.Operacion.Helper
{
    public class HelperHidrologia
    {
        /// <summary>
        /// Inicializa Lista de medidas para filtro
        /// </summary>
        /// <returns></returns>
        public static List<PtoMedida> ObtenerListaMedida()
        {
            List<PtoMedida> lista = new List<PtoMedida>();
            var elemento = new PtoMedida() { IdMedida = 1, NombreMedida = "Central" };
            lista.Add(elemento);
            elemento = new PtoMedida() { IdMedida = 2, NombreMedida = "Embalse" };
            lista.Add(elemento);
            elemento = new PtoMedida() { IdMedida = 3, NombreMedida = "Estación Hidrológica" };
            lista.Add(elemento);
            elemento = new PtoMedida() { IdMedida = 4, NombreMedida = "Cuenca" };
            lista.Add(elemento);

            return lista;
        }

        /// <summary>
        /// Inicializa Lista de tipo de informacion para filtro
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerListaTipoInfo()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "HISTORICO" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "PROYECTADO" };
            lista.Add(elemento);
            return lista;
        }

        /// <summary>
        /// Inicializa Lista de frecuencia de carga para filtro
        /// </summary>
        /// <returns></returns>

        public static List<TipoInformacion> ObtenerListaFrecuencia()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "DIARIA" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "CADA (3) HORAS" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 3, NombreTipoInfo = "SEMANAL" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 4, NombreTipoInfo = "MENSUAL" };
            lista.Add(elemento);
            return lista;
        }

        /// <summary>
        /// Inicializa Lista de tipo de formato de carga para filtro
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerListaTipoFormato()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "EJECUTADO" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "PROGRAMADO DIARIO" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 3, NombreTipoInfo = "PROGRAMADO SEMANAL" };
            lista.Add(elemento);
            return lista;

        }

        /// <summary>
        /// Inicializa Lista de tipo de reporte de carga para filtro
        /// </summary>
        /// <returns></returns>
        public static List<TipoInformacion> ObtenerListaTipoReporte()
        {
            List<TipoInformacion> lista = new List<TipoInformacion>();
            var elemento = new TipoInformacion() { IdTipoInfo = 1, NombreTipoInfo = "DIARIO" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 2, NombreTipoInfo = "SEMANAL" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 3, NombreTipoInfo = "MENSUAL" };
            lista.Add(elemento);
            elemento = new TipoInformacion() { IdTipoInfo = 3, NombreTipoInfo = "ANUAL" };
            lista.Add(elemento);
            return lista;

        }
        /// <summary>
        /// Devuelve Lista de semanas por anho
        /// </summary>
        /// <param name="anho"></param>
        /// <returns></returns>
        public static List<TipoInformacion> GetListaSemana(int anho)
        {
            List<TipoInformacion> entitys = new List<TipoInformacion>();
            DateTime dfecha = new DateTime(anho, 12, 31);
            int nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                TipoInformacion reg = new TipoInformacion();
                reg.IdTipoInfo = i;
                reg.NombreTipoInfo = "Sem" + i + "-" + anho;
                entitys.Add(reg);

            }
            return entitys;
        }
    }
}