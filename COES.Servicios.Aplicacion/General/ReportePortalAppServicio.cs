using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.General
{
    public class ReportePortalAppServicio
    {
        /// <summary>
        /// Permite mostrar el reporte del sorteo
        /// </summary>
        /// <returns></returns>
        public List<PrLogsorteoDTO> ListaLogSorteo() 
        {
            //int nroMeses = 1;
            //int mes = DateTime.Now.Month;
            //int anio = DateTime.Now.Year;
            //if (mes <= nroMeses)
            //{
            //    mes = 12 - nroMeses + mes;
            //    anio = anio - 1;
            //}
            //else 
            //{
            //    mes = mes - nroMeses;
            //}

            //DateTime fechaConsulta = new DateTime(anio, mes, 1);
            return FactorySic.GetPrLogsorteoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener el reporte de la situación de las unidades
        /// </summary>
        /// <returns></returns>
        public List<PrLogsorteoDTO> ObtenerSituacionUnidades() 
        {
            List<PrLogsorteoDTO> result = new List<PrLogsorteoDTO>();
            DateTime fechaInicio = DateTime.Now.AddDays(-30);
            DateTime fechaFin = DateTime.Now;

            List<PrLogsorteoDTO> list = FactorySic.GetPrLogsorteoRepository().ObtenerSituacionUnidades(fechaInicio, fechaFin);
            
            string emprnomb = string.Empty;
            string areanomb = string.Empty;
            string equiabrev = string.Empty;
            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;
            string causadesc = string.Empty;

            foreach (PrLogsorteoDTO item in list)
            {
                if (emprnomb == item.Emprnomb && areanomb == item.Areanomb && equiabrev == item.Equiabrev &&
                    causadesc == item.Subcausadesc && fecFin == item.Fechaini)
                {
                    fecFin = item.Fechafin;
                }
                else 
                {
                    if (!string.IsNullOrEmpty(emprnomb)) 
                    {
                        result.Add(new PrLogsorteoDTO
                        {
                            Emprnomb = emprnomb,
                            Areanomb = areanomb,
                            Equiabrev = equiabrev,
                            Fechaini = fecInicio,
                            Fechafin = fecFin,
                            Subcausadesc = causadesc
                        });
                    }
                    emprnomb = item.Emprnomb;
                    areanomb = item.Areanomb;
                    equiabrev = item.Equiabrev;
                    fecInicio = item.Fechaini;
                    fecFin = item.Fechafin;
                    causadesc = item.Subcausadesc;
                }
            }

            result.Add(new PrLogsorteoDTO
            {
                Emprnomb = emprnomb,
                Areanomb = areanomb,
                Equiabrev = equiabrev,
                Fechaini = fecInicio,
                Fechafin = fecFin,
                Subcausadesc = causadesc
            });

            return result;
        }

        /// <summary>
        /// Permite obtener el reporte de mantenimientos
        /// </summary>
        /// <returns></returns>
        public List<PrLogsorteoDTO> ObtenerMantenimientos()
        {
            List<PrLogsorteoDTO> result = new List<PrLogsorteoDTO>();
            int clase = 2;
            string desclase = string.Empty;

            for (int i = 1; i <= 7; i++)
            {
                DateTime fechaInicio = DateTime.Now.AddDays(i);
                DateTime fechaFin = DateTime.Now.AddDays(i + 1);

                switch (clase)
                {
                    case 1:
                        {
                            desclase = "E";
                            break;
                        }
                    case 2:
                        {
                            desclase = "D";
                            break;
                        }
                    case 3:
                        {
                            desclase = "S";
                            break;
                        }
                    case 4:
                        {
                            desclase = "M";
                            break;
                        }
                    default:
                        {
                            desclase = "X";
                            break;
                        }
                }

                List<PrLogsorteoDTO> list = FactorySic.GetPrLogsorteoRepository().ObtenerMantenimientos(desclase, clase, fechaInicio, fechaFin);
                result.AddRange(list);
                if (i == 1)
                {
                    clase = clase + 1;
                }
                else 
                {
                    if (fechaInicio.DayOfWeek == DayOfWeek.Saturday) 
                    {
                        clase = clase + 1;
                    }
                }

            }

            return result;
        }
    }
}
