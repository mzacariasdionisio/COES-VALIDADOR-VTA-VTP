using COES.MVC.Extranet.Areas.IEOD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Extranet.Areas.IEOD.Helper
{
    public class HelperIEOD
    {

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