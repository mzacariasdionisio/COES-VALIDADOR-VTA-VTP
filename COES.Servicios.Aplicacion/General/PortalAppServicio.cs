using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Sic.PowerBI;
using COES.Infraestructura.Datos.Repositorio.Sic;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Medidores;
using iTextSharp.xmp.options;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;




public class ReportListResponse
{
    public List<ReportItem> Value { get; set; }
}

public class ReportItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string EmbedUrl { get; set; }
    public string DatasetId { get; set; }
}


public class PowerBIReportDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string EmbedUrl { get; set; }
    public string TokenAccess { get; set; }
    public string TabId { get; set; }
}

public class PowerBIReportIntranetDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string WebUrl { get; set; }
    public string EmbedUrl { get; set; }
    public string DatasetId { get; set; }
    public string TokenAccess { get; set; }
}



public class EmbedIdentity
{
    public string Username { get; set; }
    public string[] Roles { get; set; }
    public string[] Datasets { get; set; }
}



//public class OAuthResultDTO
//{
//    [Newtonsoft.Json.JsonProperty("access_token")]
//    public string AccessToken { get; set; }
//}

public class OAuthResultDTO
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public string ExpiresIn { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }
}



namespace COES.Servicios.Aplicacion.General
{
    public class PortalAppServicio
    {
        ExcelPackage xlPackage = null;

        #region Home Principal

        /// <summary>
        /// Permite listar los eventos para el portal
        /// </summary>
        /// <returns></returns>
        public List<EveEventoDTO> ListarResumenEventosWeb(DateTime fecha)
        {
            return FactorySic.GetEveEventoRepository().ListarResumenEventosWeb(fecha);
        }

        /// <summary>
        /// Permite listar los comunicados del portal
        /// </summary>
        /// <returns></returns>
        public List<WbComunicadosDTO> ListarComunicados()
        {
            return FactorySic.GetWbComunicadosRepository().List();
        }

 

        /// <summary>
        /// Lista las frecuencias del SEIN
        /// </summary>
        /// <returns></returns>
        public List<GraficoFrecuencia> ObtenerFrecuenciaSein(int iGpscodi)
        {
            List<GraficoFrecuencia> result = new List<GraficoFrecuencia>();
            List<FLecturaDTO> list = FactorySic.GetFLecturaRepository().ObtenerFrecuenciaSein(iGpscodi);

            FLecturaDTO anterior = list[1];
            DateTime fechaAnterior = anterior.Fechahora.AddSeconds(-59);

            for (int i = 0; i < 60; i++)
            {
                GraficoFrecuencia item = new GraficoFrecuencia
                {
                    Fecha = fechaAnterior.AddSeconds(i).ToString("yyyy/MM/dd HH:mm:ss"),
                    Valor = (decimal)anterior.GetType().GetProperty("H" + i).GetValue(anterior, null)
                };

                result.Add(item);
            }

            FLecturaDTO presente = list[0];
            int nroSegundo = presente.Fechahora.Second;
            DateTime fechaPresente = presente.Fechahora.AddSeconds(-1 * presente.Fechahora.Second);

            for (int i = 0; i < nroSegundo - 1; i++)
            {
                GraficoFrecuencia item = new GraficoFrecuencia
                {
                    Fecha = fechaPresente.AddSeconds(i).ToString("yyyy/MM/dd HH:mm:ss"),
                    Valor = (decimal)presente.GetType().GetProperty("H" + i).GetValue(presente, null)
                };

                result.Add(item);
            }

            return result;
        }

        #endregion

        #region Generacion

        /// <summary>
        /// Generacion SCADA por empresa
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFina"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerGeneracionPorEmpresa(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerGeneracionPorEmpresa(fechaInicio, fechaFin);

            List<MeMedicion48DTO> resultado = (from row in list
                                               group row by new { row.Emprnomb } into g
                                               select new MeMedicion48DTO()
                                               {
                                                   Emprnomb = g.Key.Emprnomb,
                                                   Meditotal = g.Sum(x => x.Meditotal)
                                               }).ToList();

            return resultado;
        }



        /// <summary>
        /// Generacion SCADA por empresa y tipo de generación
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public ChartGeneracion ObtenerGeneracionPorEmpresaTipoGeneracion(DateTime fechaInicio, DateTime fechaFin)
        {
            ChartGeneracion chart = new ChartGeneracion();
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerGeneracionPorEmpresaTipoGeneracion(fechaInicio, fechaFin);
            List<SeriesXY> listAdicional = new List<SeriesXY>();


            List<MeMedicion48DTO> grupo = (from row in list
                                           group row by new { row.Emprnomb, row.Tgenernomb } into g
                                           select new MeMedicion48DTO()
                                           {
                                               Emprnomb = g.Key.Emprnomb,
                                               Tgenernomb = g.Key.Tgenernomb,
                                               Meditotal = g.Sum(x => x.Meditotal)
                                           }).ToList();

            chart.Categorias = ((from row in grupo
                                 group row by new { row.Emprnomb } into g
                                 select new MeMedicion48DTO()
                                 {
                                     Emprnomb = g.Key.Emprnomb,
                                     Meditotal = g.Sum(x => x.Meditotal)
                                 }).OrderByDescending(x => x.Meditotal)).Select(x => x.Emprnomb).ToList();


            var TipoGeneracion = grupo.OrderBy(x => x.Meditotal).Select(x => x.Tgenernomb).Distinct().ToList();

            foreach (var item in TipoGeneracion)
            {
                var serie = new Series();
                serie.Name = item;

                foreach (var cat in chart.Categorias)
                {
                    decimal? valor = grupo.Where(x => x.Tgenernomb == item && x.Emprnomb == cat).Select(x => x.Meditotal).SingleOrDefault();
                    serie.Data.Add(valor != null ? valor.Value : 0);
                }

                chart.Series.Add(serie);
            }

            foreach (string empresa in chart.Categorias)
            {
                SeriesXY seriexy = new SeriesXY();
                seriexy.Name = empresa;
                seriexy.Data = new List<PuntoSerie>();
                var tgeners = grupo.Where(x => x.Emprnomb == empresa).Select(x => new { x.Tgenernomb, x.Meditotal });

                foreach (var tgener in tgeners)
                {
                    seriexy.Data.Add(new PuntoSerie { Nombre = tgener.Tgenernomb, Valor = (decimal)tgener.Meditotal });
                }

                listAdicional.Add(seriexy);
            }

            chart.SeriesAdicional = listAdicional;
            return chart;
        }

        /// <summary>
        /// Obtener generacion por empresa
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<PuntoSerie> ObtenerGeneracionPorEmpresaReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            List<PuntoSerie> result = new List<PuntoSerie>();
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerGeneracionPorEmpresa(fechaInicio, fechaFin);

            foreach (MeMedicion48DTO entity in list)
            {
                PuntoSerie item = new PuntoSerie
                {
                    Nombre = entity.Emprnomb,
                    Valor = (decimal)entity.Meditotal
                };
                result.Add(item);
            }

            return result;
        }

        public List<MeMedicion48DTO> ObtenerReporteGeneracion(DateTime fecha)
        {
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerGeneracionPorEmpresaTipoGeneracionMovil(fecha, fecha);
            return list;
        }

        /// <summary>
        /// Permite procesar comparativo generación acumulada
        /// </summary>
        public void ProcesarGeneracionAcumulada()
        {
            try
            {
                DateTime fecha = DateTime.Now.AddDays(-1);

                decimal demandaActual = FactorySic.GetMeMedicion48Repository().ObtenerGeneracionAcumuladaAnual(fecha);
                decimal demandaAnterior = FactorySic.GetMeMedicion48Repository().ObtenerGeneracionAcumuladaAnual(fecha.AddYears(-1));
                decimal variacion = 0;

                if (demandaAnterior != 0)
                {
                    variacion = (demandaActual - demandaAnterior) / demandaAnterior;
                }

                WbResumengenDTO entity = new WbResumengenDTO
                {
                    Resgenactual = demandaActual,
                    Resgenanterior = demandaAnterior,
                    Resgenvariacion = variacion,
                    Resgenfecha = new DateTime(fecha.Year, fecha.Month, fecha.Day),
                    Lastdate = DateTime.Now
                };

                FactorySic.GetWbResumengenRepository().Save(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el porcentaje de crecimiento
        /// </summary>
        /// <returns></returns>
        public decimal ObtenerPorcentajeCrecimiento() 
        {
            DateTime fecha = DateTime.Now.AddDays(-1);

            WbResumengenDTO entity = FactorySic.GetWbResumengenRepository().GetByCriteria(fecha);

            if (entity != null)
            {
                return Math.Round((decimal)entity.Resgenvariacion, 4) * 100;
            }

            return 0;
        }

        /// <summary>
        /// Permite obtener los datos por tipo de generación
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public List<PuntoSerie> ObtenerGeneracionPorTipoGeneracion(DateTime fechaInicio, DateTime fechaFin)
        {
            List<PuntoSerie> result = new List<PuntoSerie>();
            List<MeMedicion96DTO> list = FactorySic.GetMeMedicion96Repository().ObtenerConsultaWeb(fechaInicio, fechaFin);

            var tipoGeneracion = list.OrderBy(x => x.Tgenernomb).Select(x => x.Tgenernomb).Distinct().ToList();

            foreach (var item in tipoGeneracion)
            {
                decimal? valor = list.Where(x => x.Tgenernomb == item).Sum(x => x.Meditotal);
                PuntoSerie itemPunto = new PuntoSerie { Nombre = item, Valor = (valor != null ? valor.Value : 0) };

                if (item == "HIDROELÉCTRICA")
                {
                    itemPunto.CodColor = "#4572A7";
                }
                else if (item == "SOLAR")
                {
                    itemPunto.CodColor = "#FFD700";
                }
                else if (item == "TERMOELÉCTRICA")
                {
                    itemPunto.CodColor = "#FF0000";
                }
                else if (item == "EÓLICA")
                {
                    itemPunto.CodColor = "#69C9E0";
                }

                result.Add(itemPunto);

            }

            return result;
        }


        public List<MeMedicion48DTO> TestPort(DateTime fecInicio, DateTime fecFin)
        {
            return FactorySic.GetPortalInformacionRepository().ProduccionxTipoCombustible(fecInicio, fecFin);
        }

        /// <summary>
        /// Permite obtener los datos del gráfico de producción
        /// </summary>
        /// <param name="lastDate"></param>
        /// <param name="lastValue"></param>
        /// <returns></returns>
        public List<PuntoSerie> ObtenerChartProduccionHome(out DateTime lastDate, out decimal lastValue)
        {
            List<MeMedicion48DTO> list = FactorySic.GetPortalInformacionRepository().ProduccionxTipoCombustible(DateTime.Now, DateTime.Now);
            List<PuntoSerie> pie = new List<PuntoSerie>();
            int hora = DateTime.Now.Hour;
            int min = DateTime.Now.Minute;

            foreach (var item in list)
            {
                PuntoSerie fuente = new PuntoSerie();
                fuente.Nombre = item.Fenergnomb;
                fuente.CodColor = this.GetColorCombustible(fuente.Nombre);
                int hFin = hora * 2;
                if (min >= 30)
                {
                    hFin = hFin + 1;
                    min = 30;
                }
                else
                {
                    min = 0;
                }

                for (int i = 1; i <= hFin; i++)
                {
                    var propertyName = string.Concat("H", i);
                    if (item != null)
                    {
                        var property = item.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            fuente.Valor += decimal.Parse(property.GetValue(item).ToString());
                        }
                    }
                }


                pie.Add(fuente);
            }

            lastDate = DateTime.Now.Date.AddHours(hora).AddMinutes(min);
            lastValue = pie.Sum(x => x.Valor);
            return pie.Where(x => x.Valor > 0).ToList();
        }

        public string GetColorCombustible(string name)
        {

            var color = "#fff";
            switch (name.Trim())
            {
                case "HÍDRICO":
                    color = "#4572A7";//(AZUL)
                    break;
                case "HIDROELÉCTRICA":
                    color = "#4572A7";//(AZUL)
                    break;
                case "AGUA":
                    color = "#4572A7";//(AZUL)
                    break;
                case "GAS":
                    color = "#F79646";//(ROJO)
                    break;
                case "DIESEL":
                    color = "#880000"; //(VERDE OSCURO)
                    break;
                case "DIESEL B5":
                    color = "#880000"; //(VERDE OSCURO)
                    break;
                case "RESIDUAL":
                    color = "#477519"; //(MARRON)
                    break;
                case "RESIDUAL R500":
                    color = "#477519"; //(MARRON)
                    break;
                case "RESIDUAL R6":
                    color = "#477519"; //(MARRON)
                    break;
                case "BAGAZO":
                    color = "#C3C3C3"; //(NARANJA)
                    break;
                case "EÓLICA":
                    color = "#69C9E0"; //(CELESTE)
                    break;
                case "EOLICA":
                    color = "#69C9E0"; //(CELESTE)
                    break;
                case "BIOGÁS":
                    color = "#2CDD17"; //(VERDE CLARO)
                    break;
                case "BIOGAS":
                    color = "#2CDD17"; //(VERDE CLARO)
                    break;
                case "CARBÓN":
                    color = "#515151"; //(GRIS OSCURO)
                    break;
                case "CARBON":
                    color = "#515151"; //(GRIS OSCURO)
                    break;
                case "SOLAR":
                    color = "#FFFF70"; //(AMARILLO)
                    break;
                case "Hídrico":
                    color = "#6699FF";//(AZUL)
                    break;
                case "Gas":
                    color = "#FF3300";//(ROJO)
                    break;
                case "Diesel":
                    color = "#477519"; //(VERDE OSCURO)
                    break;
                case "Residual":
                    color = "#AC5930"; //(MARRON)
                    break;
                case "Carbón":
                    color = "#515151"; //(GRIS OSCURO)
                    break;
                case "Solar":
                    color = "#FFFF70"; //(AMARILLO)
                    break;
                case "Eólica":
                    color = "#69C9E0"; //(AMARILLO)
                    break;
                case "Otros":
                    color = "#F5B67F";
                    break;
                case "TERMOELÉCTRICA":
                    color = "#FF7A33";
                    break;
            }

            return color;
        }


        /// <summary>
        /// Generación SCADA por tipo de combustible
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public ChartStock ObtenerGeneracionPorTipoCombustible(DateTime fechaInicio, DateTime fechaFin)
        {
            ChartStock resultado = new ChartStock();
            List<MeMedicion48DTO> resultTemp = FactorySic.GetPortalInformacionRepository().ProduccionxTipoCombustible(fechaInicio, fechaFin);

            List<MeMedicion48DTO> result = (from row in resultTemp
                                            group row by new { row.Fenergnomb, row.Medifecha } into g
                                            select new MeMedicion48DTO()
                                            {
                                                Fenergnomb = g.Key.Fenergnomb,
                                                Medifecha = g.Key.Medifecha,
                                                H1 = g.Sum(x => x.H1),
                                                H2 = g.Sum(x => x.H2),
                                                H3 = g.Sum(x => x.H3),
                                                H4 = g.Sum(x => x.H4),
                                                H5 = g.Sum(x => x.H5),
                                                H6 = g.Sum(x => x.H6),
                                                H7 = g.Sum(x => x.H7),
                                                H8 = g.Sum(x => x.H8),
                                                H9 = g.Sum(x => x.H9),
                                                H10 = g.Sum(x => x.H10),
                                                H11 = g.Sum(x => x.H11),
                                                H12 = g.Sum(x => x.H12),
                                                H13 = g.Sum(x => x.H13),
                                                H14 = g.Sum(x => x.H14),
                                                H15 = g.Sum(x => x.H15),
                                                H16 = g.Sum(x => x.H16),
                                                H17 = g.Sum(x => x.H17),
                                                H18 = g.Sum(x => x.H18),
                                                H19 = g.Sum(x => x.H19),
                                                H20 = g.Sum(x => x.H20),
                                                H21 = g.Sum(x => x.H21),
                                                H22 = g.Sum(x => x.H22),
                                                H23 = g.Sum(x => x.H23),
                                                H24 = g.Sum(x => x.H24),
                                                H25 = g.Sum(x => x.H25),
                                                H26 = g.Sum(x => x.H26),
                                                H27 = g.Sum(x => x.H27),
                                                H28 = g.Sum(x => x.H28),
                                                H29 = g.Sum(x => x.H29),
                                                H30 = g.Sum(x => x.H30),
                                                H31 = g.Sum(x => x.H31),
                                                H32 = g.Sum(x => x.H32),
                                                H33 = g.Sum(x => x.H33),
                                                H34 = g.Sum(x => x.H34),
                                                H35 = g.Sum(x => x.H35),
                                                H36 = g.Sum(x => x.H36),
                                                H37 = g.Sum(x => x.H37),
                                                H38 = g.Sum(x => x.H38),
                                                H39 = g.Sum(x => x.H39),
                                                H40 = g.Sum(x => x.H40),
                                                H41 = g.Sum(x => x.H41),
                                                H42 = g.Sum(x => x.H42),
                                                H43 = g.Sum(x => x.H43),
                                                H44 = g.Sum(x => x.H44),
                                                H45 = g.Sum(x => x.H45),
                                                H46 = g.Sum(x => x.H46),
                                                H47 = g.Sum(x => x.H47),
                                                H48 = g.Sum(x => x.H48),
                                            }).ToList();

            var FuenteEnergia = result.OrderBy(x => x.Fenergnomb).Select(x => x.Fenergnomb).Distinct().ToList();
            var FechasMedicion = result.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();

            SeriesXY serie = new SeriesXY();

            foreach (var itemPM in FuenteEnergia)
            {
                serie = new SeriesXY();
                serie.Name = itemPM;

                foreach (var itemFM in FechasMedicion)
                {
                    int hora = DateTime.Now.Hour;
                    int min = DateTime.Now.Minute;
                    int hFin = 48;

                    if (itemFM == DateTime.Now.Date)
                    {
                        hFin = hora * 2;
                        if (min >= 30)
                        {
                            hFin = hFin + 1;
                        }
                    }

                    int addMin = 0;
                    for (int i = 1; i <= hFin; i++)
                    {
                        addMin = addMin + 30;
                        DateTime FECHA = itemFM.AddMinutes(addMin);
                        var propertyName = string.Concat("H", i);
                        var point = result.Where(x => x.Fenergnomb == itemPM && x.Medifecha == itemFM).FirstOrDefault();

                        if (point != null)
                        {
                            var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                            if (property != null)
                            {
                                if (property.GetValue(point) != null)
                                {
                                    serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = decimal.Parse(property.GetValue(point).ToString()) });
                                }
                                else
                                {
                                    serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = 0 });
                                }
                            }
                        }
                        else
                        {
                            serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = 0 });
                        }
                    }
                }
                resultado.Series.Add(serie);
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener los datos de generaciones de medidores
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listMedicion"></param>
        /// <param name="graficoGeneracion"></param>
        /// <param name="graficoCombustible"></param>
        public void ObtenerGeneracionMedidores(DateTime fechaInicio, DateTime fechaFin, out List<MeMedicion48DTO> listMedicion,
            out ChartGeneracion graficoGeneracion, out ChartStock graficoCombustible)
        {
            List<MeMedicion96DTO> list = FactorySic.GetMeMedicion96Repository().ObtenerConsultaWeb(fechaInicio, fechaFin);

            #region Por Empresa
            listMedicion = ((from row in list
                             group row by new { row.Emprnomb } into g
                             select new MeMedicion48DTO()
                             {
                                 Emprnomb = g.Key.Emprnomb,
                                 Meditotal = g.Sum(x => x.Meditotal) / 4M
                             })).OrderByDescending(x => x.Meditotal).ToList();


            #endregion

            #region Por empresa y tipo de generacion

            graficoGeneracion = new ChartGeneracion();

            List<MeMedicion96DTO> result = (from row in list
                                            group row by new { row.Emprnomb, row.Tgenernomb } into g
                                            select new MeMedicion96DTO()
                                            {
                                                Emprnomb = g.Key.Emprnomb,
                                                Tgenernomb = g.Key.Tgenernomb,
                                                Meditotal = g.Sum(x => x.Meditotal) / 4M
                                            }).ToList();

            graficoGeneracion.Categorias = ((from row in result
                                             group row by new { row.Emprnomb } into g
                                             select new MeMedicion48DTO()
                                             {
                                                 Emprnomb = g.Key.Emprnomb,
                                                 Meditotal = g.Sum(x => x.Meditotal)
                                             }).OrderByDescending(x => x.Meditotal)).Select(x => x.Emprnomb).ToList();

            var TipoGeneracion = result.OrderBy(x => x.Tgenernomb).Select(x => x.Tgenernomb).Distinct().ToList();

            foreach (var item in TipoGeneracion)
            {
                var serie = new Series();
                serie.Name = item;

                foreach (var cat in graficoGeneracion.Categorias)
                {
                    decimal? valor = result.Where(x => x.Tgenernomb == item && x.Emprnomb == cat).Select(x => x.Meditotal).SingleOrDefault();
                    serie.Data.Add(valor != null ? valor.Value : 0);
                }
                graficoGeneracion.Series.Add(serie);
            }

            List<SeriesXY> listAdicional = new List<SeriesXY>();

            foreach (string empresa in graficoGeneracion.Categorias)
            {
                SeriesXY seriexy1 = new SeriesXY();
                seriexy1.Name = empresa;
                seriexy1.Data = new List<PuntoSerie>();
                var tgeners = result.Where(x => x.Emprnomb == empresa).Select(x => new { x.Tgenernomb, x.Meditotal });

                foreach (var tgener in tgeners)
                {
                    seriexy1.Data.Add(new PuntoSerie { Nombre = tgener.Tgenernomb, Valor = (decimal)tgener.Meditotal });
                }

                listAdicional.Add(seriexy1);
            }

            graficoGeneracion.SeriesAdicional = listAdicional;

            #endregion

            #region Por tipo de combustible

            graficoCombustible = new ChartStock();

            List<MeMedicion96DTO> groupList = (from row in list
                                               group row by new { row.Fenergnomb, row.Medifecha } into g
                                               select new MeMedicion96DTO()
                                               {
                                                   Fenergnomb = g.Key.Fenergnomb,
                                                   Medifecha = g.Key.Medifecha,
                                                   Meditotal = g.Sum(x => x.Meditotal) / 4M,
                                                   H1 = g.Sum(x => x.H1),
                                                   H2 = g.Sum(x => x.H2),
                                                   H3 = g.Sum(x => x.H3),
                                                   H4 = g.Sum(x => x.H4),
                                                   H5 = g.Sum(x => x.H5),
                                                   H6 = g.Sum(x => x.H6),
                                                   H7 = g.Sum(x => x.H7),
                                                   H8 = g.Sum(x => x.H8),
                                                   H9 = g.Sum(x => x.H9),
                                                   H10 = g.Sum(x => x.H10),
                                                   H11 = g.Sum(x => x.H11),
                                                   H12 = g.Sum(x => x.H12),
                                                   H13 = g.Sum(x => x.H13),
                                                   H14 = g.Sum(x => x.H14),
                                                   H15 = g.Sum(x => x.H15),
                                                   H16 = g.Sum(x => x.H16),
                                                   H17 = g.Sum(x => x.H17),
                                                   H18 = g.Sum(x => x.H18),
                                                   H19 = g.Sum(x => x.H19),
                                                   H20 = g.Sum(x => x.H20),
                                                   H21 = g.Sum(x => x.H21),
                                                   H22 = g.Sum(x => x.H22),
                                                   H23 = g.Sum(x => x.H23),
                                                   H24 = g.Sum(x => x.H24),
                                                   H25 = g.Sum(x => x.H25),
                                                   H26 = g.Sum(x => x.H26),
                                                   H27 = g.Sum(x => x.H27),
                                                   H28 = g.Sum(x => x.H28),
                                                   H29 = g.Sum(x => x.H29),
                                                   H30 = g.Sum(x => x.H30),
                                                   H31 = g.Sum(x => x.H31),
                                                   H32 = g.Sum(x => x.H32),
                                                   H33 = g.Sum(x => x.H33),
                                                   H34 = g.Sum(x => x.H34),
                                                   H35 = g.Sum(x => x.H35),
                                                   H36 = g.Sum(x => x.H36),
                                                   H37 = g.Sum(x => x.H37),
                                                   H38 = g.Sum(x => x.H38),
                                                   H39 = g.Sum(x => x.H39),
                                                   H40 = g.Sum(x => x.H40),
                                                   H41 = g.Sum(x => x.H41),
                                                   H42 = g.Sum(x => x.H42),
                                                   H43 = g.Sum(x => x.H43),
                                                   H44 = g.Sum(x => x.H44),
                                                   H45 = g.Sum(x => x.H45),
                                                   H46 = g.Sum(x => x.H46),
                                                   H47 = g.Sum(x => x.H47),
                                                   H48 = g.Sum(x => x.H48),
                                                   H49 = g.Sum(x => x.H49),
                                                   H50 = g.Sum(x => x.H50),
                                                   H51 = g.Sum(x => x.H51),
                                                   H52 = g.Sum(x => x.H52),
                                                   H53 = g.Sum(x => x.H53),
                                                   H54 = g.Sum(x => x.H54),
                                                   H55 = g.Sum(x => x.H55),
                                                   H56 = g.Sum(x => x.H56),
                                                   H57 = g.Sum(x => x.H57),
                                                   H58 = g.Sum(x => x.H58),
                                                   H59 = g.Sum(x => x.H59),
                                                   H60 = g.Sum(x => x.H60),
                                                   H61 = g.Sum(x => x.H61),
                                                   H62 = g.Sum(x => x.H62),
                                                   H63 = g.Sum(x => x.H63),
                                                   H64 = g.Sum(x => x.H64),
                                                   H65 = g.Sum(x => x.H65),
                                                   H66 = g.Sum(x => x.H66),
                                                   H67 = g.Sum(x => x.H67),
                                                   H68 = g.Sum(x => x.H68),
                                                   H69 = g.Sum(x => x.H69),
                                                   H70 = g.Sum(x => x.H70),
                                                   H71 = g.Sum(x => x.H71),
                                                   H72 = g.Sum(x => x.H72),
                                                   H73 = g.Sum(x => x.H73),
                                                   H74 = g.Sum(x => x.H74),
                                                   H75 = g.Sum(x => x.H75),
                                                   H76 = g.Sum(x => x.H76),
                                                   H77 = g.Sum(x => x.H77),
                                                   H78 = g.Sum(x => x.H78),
                                                   H79 = g.Sum(x => x.H79),
                                                   H80 = g.Sum(x => x.H80),
                                                   H81 = g.Sum(x => x.H81),
                                                   H82 = g.Sum(x => x.H82),
                                                   H83 = g.Sum(x => x.H83),
                                                   H84 = g.Sum(x => x.H84),
                                                   H85 = g.Sum(x => x.H85),
                                                   H86 = g.Sum(x => x.H86),
                                                   H87 = g.Sum(x => x.H87),
                                                   H88 = g.Sum(x => x.H88),
                                                   H89 = g.Sum(x => x.H89),
                                                   H90 = g.Sum(x => x.H90),
                                                   H91 = g.Sum(x => x.H91),
                                                   H92 = g.Sum(x => x.H92),
                                                   H93 = g.Sum(x => x.H93),
                                                   H94 = g.Sum(x => x.H94),
                                                   H95 = g.Sum(x => x.H95),
                                                   H96 = g.Sum(x => x.H96)
                                               }).ToList();


            var FuenteEnergia = groupList.OrderBy(x => x.Fenergnomb).Select(x => x.Fenergnomb).Distinct().ToList();
            var FechasMedicion = groupList.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();

            SeriesXY seriexy = new SeriesXY();

            foreach (var itemPM in FuenteEnergia)
            {
                seriexy = new SeriesXY();
                seriexy.Name = itemPM;

                foreach (var itemFM in FechasMedicion)
                {
                    int hora = DateTime.Now.Hour;
                    int min = DateTime.Now.Minute;
                    int hFin = 96;

                    if (itemFM == DateTime.Now.Date)
                    {
                        hFin = hora * 4;
                        if (min >= 15 && min < 30)
                        {
                            hFin = hFin + 1;
                        }
                        if (min >= 30 && min < 45)
                        {
                            hFin = hFin + 2;
                        }
                        if (min >= 45 && min < 60)
                        {
                            hFin = hFin + 3;
                        }
                    }

                    int addMin = 0;
                    for (int i = 1; i <= hFin; i++)
                    {
                        addMin = addMin + 15;
                        DateTime FECHA = itemFM.Value.AddMinutes(addMin);
                        var propertyName = string.Concat("H", i);
                        var point = groupList.Where(x => x.Fenergnomb == itemPM && x.Medifecha == itemFM).FirstOrDefault();

                        if (point != null)
                        {
                            var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                            if (property != null)
                            {
                                if (property.GetValue(point) != null)
                                {
                                    seriexy.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = decimal.Parse(property.GetValue(point).ToString()) });
                                }
                                else
                                {
                                    seriexy.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = 0 });
                                }
                            }
                        }
                        else
                        {
                            seriexy.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = 0 });
                        }
                    }
                }
                graficoCombustible.Series.Add(seriexy);
            }

            #endregion
        }

        /// <summary>
        /// Permite obtener los datos para la exportación del reporte
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        public void ObtenerExportacionGeneracion(DateTime fechaInicio, DateTime fechaFin, int indicador, string path, string file)
        {
            List<MeMedicion48DTO> list = null;
            if (indicador == 1)
                list = FactorySic.GetMeMedicion96Repository().ObtenerConsultaWebReporte(fechaInicio, fechaFin);
            else
                list = FactorySic.GetMeMedicion48Repository().ObtenerConsultaWebReporte(fechaInicio, fechaFin);

            file = path + file;
            FileInfo newFile = new FileInfo(file);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }

            using (xlPackage = new ExcelPackage(newFile))
            {
                this.ExportarDesagregadoGeneracion(list);
                this.ExportarPorEmpresaGeneracion(list);
                this.ExportarPorTipoGeneracion(list);
                //this.ExportarPorFuenteEnergia(list);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera la hoja del reporte de generación desagregado
        /// </summary>
        /// <param name="list"></param>
        private void ExportarDesagregadoGeneracion(List<MeMedicion48DTO> list)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("PRODUCCIÓN DIARIA");

            if (ws != null)
            {
                int index = 4;

                ws.Cells[index, 1].Value = "FECHA";
                ws.Cells[index, 2].Value = "EMPRESA";
                ws.Cells[index, 3].Value = "GRUPO/CENTRAL";
                ws.Cells[index, 4].Value = "TIPO GENERACIÓN";
                ws.Cells[index, 5].Value = "TOTAL";

                ExcelRange rg = ws.Cells[index, 1, index, 5];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                index++;

                var listFecha = list.Select(x => x.Medifecha).Distinct().ToList();

                foreach (var itemFecha in listFecha)
                {
                    var listEmpresa = list.Where(x => x.Medifecha == itemFecha).Select(x =>
                        new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();

                    foreach (var itemEmpresa in listEmpresa)
                    {
                        var listCentral = list.Where(x => x.Medifecha == itemFecha && x.Emprcodi == itemEmpresa.Emprcodi).
                            Select(x => new { x.Grupocodi, x.Gruponomb }).Distinct().ToList();

                        foreach (var itemCentral in listCentral)
                        {
                            var listTipoGen = list.Where(x => x.Medifecha == itemFecha && x.Emprcodi == itemEmpresa.Emprcodi &&
                                x.Grupocodi == itemCentral.Grupocodi).Select(x => new { x.Tgenercodi, x.Tgenernomb }).Distinct().ToList();

                            foreach (var itemTipoGen in listTipoGen)
                            {
                                ws.Cells[index, 1].Value = itemFecha.ToString("dd/MM/yyyy");
                                ws.Cells[index, 2].Value = itemEmpresa.Emprnomb.Trim();
                                ws.Cells[index, 3].Value = itemCentral.Gruponomb.Trim();
                                ws.Cells[index, 4].Value = itemTipoGen.Tgenernomb;

                                var itemValor = list.Where(x => x.Medifecha == itemFecha && x.Emprcodi == itemEmpresa.Emprcodi &&
                                x.Grupocodi == itemCentral.Grupocodi && x.Tgenercodi == itemTipoGen.Tgenercodi).Select(x => x.Meditotal).Sum();

                                if (itemValor != null)
                                    ws.Cells[index, 5].Value = itemValor;

                                index++;
                            }
                        }
                    }
                }

                rg = ws.Cells[5, 1, index - 1, 5];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;

                rg = ws.Cells[1, 1, index + 2, 5];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("logo", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
            }
        }

        /// <summary>
        /// Genera la hoja de generación por empresa
        /// </summary>
        /// <param name="list"></param>
        private void ExportarPorEmpresaGeneracion(List<MeMedicion48DTO> list)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("POR EMPRESA");

            if (ws != null)
            {
                int index = 4;

                ws.Cells[index, 1].Value = "EMPRESA";
                ws.Cells[index, 2].Value = "TOTAL";

                ExcelRange rg = ws.Cells[index, 1, index, 2];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                index++;

                var listEmpresa = list.Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();

                foreach (var itemEmpresa in listEmpresa)
                {
                    var sumEmpresa = list.Where(x => x.Emprcodi == itemEmpresa.Emprcodi).Sum(x => x.Meditotal);
                    ws.Cells[index, 1].Value = itemEmpresa.Emprnomb.Trim();
                    ws.Cells[index, 2].Value = sumEmpresa;
                    index++;
                }

                rg = ws.Cells[5, 1, index - 1, 2];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;

                rg = ws.Cells[1, 1, index + 2, 5];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("logo", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
            }
        }

        /// <summary>
        /// Genera la hoja de generación por tipo de fuente energética
        /// </summary>
        /// <param name="list"></param>
        private void ExportarPorTipoGeneracion(List<MeMedicion48DTO> list)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("POR TIPO GENERACIÓN");

            if (ws != null)
            {
                int index = 4;

                ws.Cells[index, 1].Value = "EMPRESA";
                ws.Cells[index, 2].Value = "TIPO GENERACIÓN";
                ws.Cells[index, 3].Value = "TOTAL";

                ExcelRange rg = ws.Cells[index, 1, index, 3];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                index++;

                var listEmpresa = list.Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();

                foreach (var itemEmpresa in listEmpresa)
                {
                    var listTipoGeneracion = list.Where(x => x.Emprcodi == itemEmpresa.Emprcodi).Select(x =>
                        new { x.Tgenercodi, x.Tgenernomb }).Distinct().ToList();

                    foreach (var itemGeneracion in listTipoGeneracion)
                    {
                        var total = list.Where(x => x.Emprcodi == itemEmpresa.Emprcodi && x.Tgenercodi == itemGeneracion.Tgenercodi).Sum(x => x.Meditotal);
                        ws.Cells[index, 1].Value = itemEmpresa.Emprnomb.Trim();
                        ws.Cells[index, 2].Value = itemGeneracion.Tgenernomb;
                        ws.Cells[index, 3].Value = total;
                        index++;
                    }
                }

                rg = ws.Cells[5, 1, index - 1, 3];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;

                rg = ws.Cells[1, 1, index + 2, 5];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("logo", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 1;
                picture.SetSize(120, 60);
            }
        }

        /// <summary>
        /// Permite generar la hoja de generación por fuente de energia
        /// </summary>
        /// <param name="list"></param>
        private void ExportarPorFuenteEnergia(List<MeMedicion48DTO> list)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("POR TIPO DE COMBUSTIBLE");

            if (ws != null)
            {
                int index = 4;
                var listFuenteEnergia = list.Select(x => new { x.Fenergcodi, x.Fenergnomb }).Distinct().ToList();
                int column = 0;
                ws.Cells[index, 1].Value = "FECHA";
                foreach (var item in listFuenteEnergia)
                {
                    ws.Cells[index, 2 + column].Value = item.Fenergnomb.Trim();
                    column++;
                }

                ExcelRange rg = ws.Cells[index, 1, index, column + 1];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                index++;

                var listFecha = list.Select(x => x.Medifecha).Distinct().ToList();

                foreach (var itemFecha in listFecha)
                {
                    ws.Cells[index, 1].Value = itemFecha.ToString("dd/MM/yyyy");
                    column = 0;
                    foreach (var itemCombustible in listFuenteEnergia)
                    {
                        var itemDato = list.Where(x => x.Medifecha == itemFecha && x.Fenergcodi == itemCombustible.Fenergcodi).Select(x => x.Meditotal).Sum();

                        if (itemDato != null)
                        {
                            ws.Cells[index, 2 + column].Value = itemDato;
                        }

                        column++;
                    }
                    index++;
                }

                rg = ws.Cells[5, 1, index - 1, column + 1];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;

                rg = ws.Cells[1, 1, index + 2, column + 1];
                rg.AutoFitColumns();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("logo", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
            }
        }


        #endregion

        #region Demanda

        /// <summary>
        /// Permite obtener los datos del reporte de Demanda
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public ChartDemanda ObtenerReporteDemanda(DateTime fechaInicio, DateTime fechaFin, out DateTime fechaEjecutado, out decimal valorEjecutado)
        {
            ChartDemanda resultado = new ChartDemanda();
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerDemandaPortalWeb(fechaInicio, fechaFin);

            List<MeMedicion48DTO> ejecutado = list.Where(x => x.Lectcodi == 6).ToList();
            List<MeMedicion48DTO> ProgramacionDiaria = list.Where(x => x.Lectcodi == 4).ToList();
            List<MeMedicion48DTO> ProgramacionSemanal = list.Where(x => x.Lectcodi == 3).ToList();

            ChartStock chart = new ChartStock();
            SeriesXY serie = new SeriesXY();

            #region Ejecutado

            var FechasMedicion = ejecutado.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();
            serie = new SeriesXY();
            serie.Name = "Ejecutado";
            fechaEjecutado = DateTime.Now;
            valorEjecutado = 0;


            foreach (var itemFM in FechasMedicion)
            {
                int hora = DateTime.Now.Hour;
                int min = DateTime.Now.Minute;
                int hFin = 48;
                if (itemFM == DateTime.Now.Date)
                {
                    hFin = hora * 2;
                    if (min >= 30)
                    {
                        hFin = hFin + 1;
                    }
                }

                int addMin = 0;
                var point = ejecutado.Where(x => x.Medifecha == itemFM).FirstOrDefault();

                for (int i = 1; i <= hFin; i++)
                {
                    addMin = addMin + 30;
                    DateTime FECHA = itemFM.AddMinutes(addMin);
                    var propertyName = string.Concat("H", i);
                    if (point != null)
                    {
                        var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            decimal valor = decimal.Parse(property.GetValue(point).ToString());
                            serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = valor, CodColor = "#33B8FF" });
                            fechaEjecutado = FECHA;
                            valorEjecutado = valor;
                        }
                    }
                    else
                    {
                        serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = 0, CodColor = "#33B8FF" });
                    }
                }
            }
            chart.Series.Add(serie);

            #endregion

            #region Programacion Diaria

            FechasMedicion = ProgramacionDiaria.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();
            serie = new SeriesXY();
            serie.Name = "Programación Diaria";
            foreach (var itemFM in FechasMedicion)
            {
                int addMin = 0;
                var point = ProgramacionDiaria.Where(x => x.Medifecha == itemFM).FirstOrDefault();

                for (int i = 1; i <= 48; i++)
                {
                    addMin = addMin + 30;
                    DateTime FECHA = itemFM.AddMinutes(addMin);
                    var propertyName = string.Concat("H", i);
                    if (point != null)
                    {
                        var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = decimal.Parse(property.GetValue(point).ToString()), CodColor = "#3DD4B5" });
                        }
                    }
                    else
                    {
                        serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = 0, CodColor = "#3DD4B5" });
                    }
                }
            }
            chart.Series.Add(serie);

            #endregion

            #region Programacion semanal

            FechasMedicion = ProgramacionSemanal.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();
            serie = new SeriesXY();
            serie.Name = "Programación Semanal";

            foreach (var itemFM in FechasMedicion)
            {
                int addMin = 0;
                var point = ProgramacionSemanal.Where(x => x.Medifecha == itemFM).FirstOrDefault();

                for (int i = 1; i <= 48; i++)
                {
                    addMin = addMin + 30;
                    DateTime FECHA = itemFM.AddMinutes(addMin);
                    var propertyName = string.Concat("H", i);
                    if (point != null)
                    {
                        var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = decimal.Parse(property.GetValue(point).ToString()), CodColor = "#FF7A33" });
                        }
                    }
                    else
                    {
                        serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString("yyyy/MM/dd HH:mm:ss"), Valor = 0, CodColor = "#FF7A33" });
                    }
                }
            }
            chart.Series.Add(serie);

            resultado.Chart = chart;

            #endregion

            #region Datos

            List<ChartDemandaData> data = new List<ChartDemandaData>();
            foreach (var itemFM in FechasMedicion)
            {
                int addMin = 0;
                var point = ejecutado.Where(x => x.Medifecha == itemFM).FirstOrDefault();
                var pointPD = ProgramacionDiaria.Where(x => x.Medifecha == itemFM).FirstOrDefault();
                var pointPS = ProgramacionSemanal.Where(x => x.Medifecha == itemFM).FirstOrDefault();

                int hora = DateTime.Now.Hour;
                int min = DateTime.Now.Minute;
                int hFin = 48;

                if (itemFM == DateTime.Now.Date)
                {
                    hFin = hora * 2;
                    if (min >= 30)
                    {
                        hFin = hFin + 1;
                    }
                }

                for (int i = 1; i <= 48; i++)
                {
                    addMin = addMin + 30;
                    DateTime FECHA = itemFM.AddMinutes(addMin);
                    var propertyName = string.Concat("H", i);

                    ChartDemandaData pointData = new ChartDemandaData();
                    pointData.Fecha = FECHA.ToString("yyyy/MM/dd HH:mm");

                    if (itemFM == DateTime.Now.Date && i > hFin)
                    {
                        pointData.ValorEjecutado = null;
                    }
                    else
                    {
                        if (point != null)
                        {
                            var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                            if (property != null)
                            {
                                pointData.ValorEjecutado = decimal.Parse(property.GetValue(point).ToString());
                            }
                        }
                        else
                        {
                            pointData.ValorEjecutado = 0;
                        }
                    }

                    if (pointPD != null)
                    {
                        var property = pointPD.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            pointData.ValorProgramacionDiaria = decimal.Parse(property.GetValue(pointPD).ToString());
                        }
                    }
                    else
                    {
                        pointData.ValorEjecutado = 0;
                    }

                    if (pointPS != null)
                    {
                        var property = pointPS.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            pointData.ValorProgramacionSemanal = decimal.Parse(property.GetValue(pointPS).ToString());
                        }
                    }
                    else
                    {
                        pointData.ValorEjecutado = 0;
                    }

                    data.Add(pointData);
                }
            }
            resultado.Data = data;

            #endregion

            return resultado;
        }

        /// <summary>
        /// Permite obtener los datos del reporte de Demanda
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public ChartDemanda ObtenerReporteDemandaMovil(DateTime fechaInicio, DateTime fechaFin, out DateTime fechaEjecutado, out decimal valorEjecutado)
        {
            CultureInfo culture = new CultureInfo("en-US"); // Definir la cultura (puede ser diferente según la región)
            string formatoFecha = "MM/dd/yyyy hh:mm:ss tt";
            
            ChartDemanda resultado = new ChartDemanda();
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerDemandaPortalWeb(fechaInicio, fechaFin);

            List<MeMedicion48DTO> ejecutado = list.Where(x => x.Lectcodi == 6).ToList();
            List<MeMedicion48DTO> ProgramacionDiaria = list.Where(x => x.Lectcodi == 4).ToList();
            //List<MeMedicion48DTO> ReprogramacionDiaria = list.Where(x => x.Lectcodi == 5).ToList();
            List<MeMedicion48DTO> ProgramacionSemanal = list.Where(x => x.Lectcodi == 3).ToList();

            //- Líneas agregadas para utilizar el reprograma en lugar del programa
            /*if (ReprogramacionDiaria != null)
            {
                if (ReprogramacionDiaria.Count > 0)
                {
                    ProgramacionDiaria = ReprogramacionDiaria;
                }
            }*/

            ChartStock chart = new ChartStock();
            SeriesXY serie = new SeriesXY();

            #region Ejecutado

            var FechasMedicion = ejecutado.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();
            serie = new SeriesXY();
            serie.Name = "Ejecutado (MW)";
            fechaEjecutado = DateTime.Now;
            valorEjecutado = 0;


            foreach (var itemFM in FechasMedicion)
            {
                int hora = DateTime.Now.Hour;
                int min = DateTime.Now.Minute;
                int hFin = 48;
                if (itemFM == DateTime.Now.Date)
                {
                    hFin = hora * 2;
                    if (min >= 30)
                    {
                        hFin = hFin + 1;
                    }
                }

                int addMin = 0;
                var point = ejecutado.Where(x => x.Medifecha == itemFM).FirstOrDefault();

                for (int i = 1; i <= hFin; i++)
                {
                    addMin = addMin + 30;
                    DateTime FECHA = itemFM.AddMinutes(addMin);
                    var propertyName = string.Concat("H", i);
                    if (point != null)
                    {
                        var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            decimal valor = decimal.Parse(property.GetValue(point).ToString());
                            serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString(formatoFecha, culture), Valor = valor, CodColor = "#FF6600" });
                            fechaEjecutado = FECHA;
                            valorEjecutado = valor;
                        }
                    }
                    else
                    {
                        serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString(formatoFecha, culture), Valor = 0, CodColor = "#FF6600" });
                    }
                }
            }
            chart.Series.Add(serie);

            if (!(fechaEjecutado.Year == fechaInicio.Year && fechaEjecutado.Month == fechaInicio.Month && fechaEjecutado.Day == fechaInicio.Day))
            {
                fechaEjecutado = fechaEjecutado.AddMinutes(-1);
            }


            #endregion

            #region Programacion Diaria

            FechasMedicion = ProgramacionDiaria.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();
            serie = new SeriesXY();
            serie.Name = "Programación Diaria (MW)";
            foreach (var itemFM in FechasMedicion)
            {
                int addMin = 0;
                var point = ProgramacionDiaria.Where(x => x.Medifecha == itemFM).FirstOrDefault();

                for (int i = 1; i <= 48; i++)
                {
                    addMin = addMin + 30;
                    DateTime FECHA = itemFM.AddMinutes(addMin);
                    var propertyName = string.Concat("H", i);
                    if (point != null)
                    {
                        var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString(formatoFecha, culture), Valor = decimal.Parse(property.GetValue(point).ToString()), CodColor = "#0099CC" });
                        }
                    }
                    else
                    {
                        serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString(formatoFecha, culture), Valor = 0, CodColor = "#0099CC" });
                    }
                }
            }
            chart.Series.Add(serie);

            #endregion

            #region Programacion semanal

            //FechasMedicion = ProgramacionSemanal.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();
            //serie = new SeriesXY();
            //serie.Name = "Programación Semanal";

            //foreach (var itemFM in FechasMedicion)
            //{
            //    int addMin = 0;
            //    var point = ProgramacionSemanal.Where(x => x.Medifecha == itemFM).FirstOrDefault();

            //    for (int i = 1; i <= 48; i++)
            //    {
            //        addMin = addMin + 30;
            //        DateTime FECHA = itemFM.AddMinutes(addMin);
            //        var propertyName = string.Concat("H", i);
            //        if (point != null)
            //        {
            //            var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
            //            if (property != null)
            //            {
            //                serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString(), Valor = decimal.Parse(property.GetValue(point).ToString()), CodColor = "#FF7A33" });
            //            }
            //        }
            //        else
            //        {
            //            serie.Data.Add(new PuntoSerie { Nombre = FECHA.ToString(), Valor = 0, CodColor = "#FF7A33" });
            //        }
            //    }
            //}
            //chart.Series.Add(serie);

            resultado.Chart = chart;

            #endregion

            #region Datos

            List<ChartDemandaData> data = new List<ChartDemandaData>();
            foreach (var itemFM in FechasMedicion)
            {
                int addMin = 0;
                var point = ejecutado.Where(x => x.Medifecha == itemFM).FirstOrDefault();
                var pointPD = ProgramacionDiaria.Where(x => x.Medifecha == itemFM).FirstOrDefault();
                var pointPS = ProgramacionSemanal.Where(x => x.Medifecha == itemFM).FirstOrDefault();

                int hora = DateTime.Now.Hour;
                int min = DateTime.Now.Minute;
                int hFin = 48;

                if (itemFM == DateTime.Now.Date)
                {
                    hFin = hora * 2;
                    if (min >= 30)
                    {
                        hFin = hFin + 1;
                    }
                }

                for (int i = 1; i <= 48; i++)
                {
                    addMin = addMin + 30;
                    DateTime FECHA = itemFM.AddMinutes(addMin);
                    var propertyName = string.Concat("H", i);

                    ChartDemandaData pointData = new ChartDemandaData();
                    pointData.Fecha = FECHA.ToString(formatoFecha, culture);

                    if (itemFM == DateTime.Now.Date && i > hFin)
                    {
                        pointData.ValorEjecutado = null;
                    }
                    else
                    {
                        if (point != null)
                        {
                            var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                            if (property != null)
                            {
                                pointData.ValorEjecutado = decimal.Parse(property.GetValue(point).ToString());
                            }
                        }
                        else
                        {
                            pointData.ValorEjecutado = 0;
                        }
                    }

                    if (pointPD != null)
                    {
                        var property = pointPD.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            pointData.ValorProgramacionDiaria = decimal.Parse(property.GetValue(pointPD).ToString());
                        }
                    }
                    else
                    {
                        pointData.ValorEjecutado = 0;
                    }

                    if (pointPS != null)
                    {
                        var property = pointPS.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            pointData.ValorProgramacionSemanal = decimal.Parse(property.GetValue(pointPS).ToString());
                        }
                    }
                    else
                    {
                        pointData.ValorEjecutado = 0;
                    }

                    data.Add(pointData);
                }
            }
            resultado.Data = data;

            #endregion

            return resultado;
        }

        /// <summary>
        /// Permite exportar el reporte de demanda
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void ObtenerExportacionDemanda(DateTime fechaInicio, DateTime fechaFin, string path, string file)
        {
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerDemandaPortalWeb(fechaInicio, fechaFin);

            List<MeMedicion48DTO> ejecutado = list.Where(x => x.Lectcodi == 6).ToList();
            List<MeMedicion48DTO> ProgramacionDiaria = list.Where(x => x.Lectcodi == 4).ToList();
            List<MeMedicion48DTO> ProgramacionSemanal = list.Where(x => x.Lectcodi == 3).ToList();

            var FechasMedicion = ProgramacionSemanal.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();

            List<ChartDemandaData> data = new List<ChartDemandaData>();
            foreach (var itemFM in FechasMedicion)
            {
                int addMin = 0;
                var point = ejecutado.Where(x => x.Medifecha == itemFM).FirstOrDefault();
                var pointPD = ProgramacionDiaria.Where(x => x.Medifecha == itemFM).FirstOrDefault();
                var pointPS = ProgramacionSemanal.Where(x => x.Medifecha == itemFM).FirstOrDefault();

                int hora = DateTime.Now.Hour;
                int min = DateTime.Now.Minute;
                int hFin = 48;

                if (itemFM == DateTime.Now.Date)
                {
                    hFin = hora * 2;
                    if (min >= 30)
                    {
                        hFin = hFin + 1;
                    }
                }

                for (int i = 1; i <= 48; i++)
                {
                    addMin = addMin + 30;
                    DateTime FECHA = itemFM.AddMinutes(addMin);
                    var propertyName = string.Concat("H", i);

                    ChartDemandaData pointData = new ChartDemandaData();
                    pointData.Fecha = FECHA.ToString("dd/MM/yyyy HH:mm");

                    if (itemFM == DateTime.Now.Date && i > hFin)
                    {
                        pointData.ValorEjecutado = null;
                    }
                    else
                    {
                        if (point != null)
                        {
                            var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                            if (property != null)
                            {
                                pointData.ValorEjecutado = decimal.Parse(property.GetValue(point).ToString());
                            }
                        }
                        else
                        {
                            pointData.ValorEjecutado = 0;
                        }
                    }

                    if (pointPD != null)
                    {
                        var property = pointPD.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            pointData.ValorProgramacionDiaria = decimal.Parse(property.GetValue(pointPD).ToString());
                        }
                    }
                    else
                    {
                        pointData.ValorEjecutado = 0;
                    }

                    if (pointPS != null)
                    {
                        var property = pointPS.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
                        if (property != null)
                        {
                            pointData.ValorProgramacionSemanal = decimal.Parse(property.GetValue(pointPS).ToString());
                        }
                    }
                    else
                    {
                        pointData.ValorEjecutado = 0;
                    }

                    data.Add(pointData);
                }
            }

            file = path + file;
            FileInfo newFile = new FileInfo(file);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }

            using (xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("DEMANDA");

                if (ws != null)
                {
                    int index = 4;
                    ws.Cells[index, 1].Value = "FECHA";
                    ws.Cells[index, 2].Value = "EJECUTADO";
                    ws.Cells[index, 3].Value = "PROG. DIARIA";
                    ws.Cells[index, 4].Value = "PROG. SEMANAL";

                    ExcelRange rg = ws.Cells[index, 1, index, 4];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    index++;

                    foreach (ChartDemandaData item in data)
                    {
                        ws.Cells[index, 1].Value = item.Fecha;
                        ws.Cells[index, 2].Value = item.ValorEjecutado;
                        ws.Cells[index, 3].Value = item.ValorProgramacionDiaria;
                        ws.Cells[index, 4].Value = item.ValorProgramacionSemanal;
                        index++;
                    }

                    rg = ws.Cells[5, 1, index - 1, 4];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    rg = ws.Cells[1, 1, index + 2, 5];
                    rg.AutoFitColumns();

                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture("logo", img);
                    picture.From.Column = 0;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Devuelve el listado de mediciones cada 30 min. por cada día
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio de consulta</param>
        /// <param name="fechaFin">Fecha de fin de consulta</param>
        /// <param name="lectoCodi">Código de lectura de medición (6 Ejecutado,4 Prog. Diaria,3 Prog. Semanal)</param>
        /// <returns>Listado por día de mediciones</returns>
        public List<MeMedicion48DTO> ObtenerDatosDemandaPortal(DateTime fechaInicio, DateTime fechaFin, int lectoCodi)
        {
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerDemandaPortalWeb(fechaInicio, fechaFin);
            return list.Where(x => x.Lectcodi == lectoCodi).ToList();
        }

        #endregion

        #region EventoFallas

        /// <summary>
        /// Listado de empresas
        /// </summary>
        /// <returns></returns>
        public List<EmpresaDTO> ListarEmpresas()
        {
            return FactorySic.ObtenerEventoDao().ListarEmpresas();
        }

        /// <summary>
        /// Listado de tipos de empresa
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListarTipoEmpresas()
        {
            return FactorySic.GetSiTipoempresaRepository().List();
        }

        /// <summary>
        /// Listado de familias de equipos
        /// </summary>
        /// <returns></returns>
        public List<FamiliaDTO> ListarFamilias()
        {
            return FactorySic.ObtenerEventoDao().ListarFamilias();
        }

        //public async Task<List<PowerBIReportDTO>> ObtenerReportesPowerBIAsync()
        //{
        //    var clientId = ConfigurationManager.AppSettings["PowerBIClientId"];
        //    var clientSecret = ConfigurationManager.AppSettings["PowerBIClientSecret"];
        //    var tenantId = ConfigurationManager.AppSettings["PowerBITenantId"];
        //    var workspaceId = ConfigurationManager.AppSettings["PowerBIWorkspaceId"];

        //    var formUrlEncoded = new FormUrlEncodedContent(new[]
        //    {
        //        new KeyValuePair<string, string>("grant_type", "client_credentials"),
        //        new KeyValuePair<string, string>("client_id", clientId),
        //        new KeyValuePair<string, string>("client_secret", clientSecret),
        //        new KeyValuePair<string, string>("resource", "https://analysis.windows.net/powerbi/api")
        //    });

        //    using (var client = new HttpClient())
        //    {
        //        var tokenResponse = await client.PostAsync($"https://login.microsoftonline.com/{tenantId}/oauth2/token", formUrlEncoded);
        //        var json = await tokenResponse.Content.ReadAsStringAsync();
        //        var oauth = JsonConvert.DeserializeObject<OAuthResultDTO>(json);

        //        using (var pbiClient = new PowerBIClient(new Uri("https://api.powerbi.com/"), new TokenCredentials(oauth.AccessToken, "Bearer")))
        //        {
        //            var reports = await pbiClient.Reports.GetReportsInGroupAsync(workspaceId);
        //            return reports.Value.Select(r => new PowerBIReportDTO
        //            {
        //                Id = r.Id.ToString(),
        //                Name = r.Name,
        //                EmbedUrl = r.EmbedUrl,
        //                TokenAccess = oauth.AccessToken
        //            }).ToList();
        //        }
        //    }
        //}



        /// <summary>
        /// Lista de causas de eventos
        /// </summary>
        /// <returns></returns>
        public List<EveCausaeventoDTO> ListarCausasEventos()
        {
            return FactorySic.GetEveCausaeventoRepository().List();
        }



        /// <summary>
        /// Permite buscar eventos segun los parametros especificados
        /// </summary>
        /// <param name="idFallaCier"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="version"></param>
        /// <param name="turno"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoEquipo"></param>
        /// <param name="indInterrupcion"></param>
        /// <param name="nroPage"></param>
        /// <param name="nroFilas"></param>
        /// <returns></returns>
        public List<EventoDTO> BuscarEventos(int? idFallaCier, DateTime fechaInicio, DateTime fechaFin,
            string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion, int nroPage, int nroFilas)
        {
            string filtro = string.Empty;
            return FactorySic.ObtenerEventoDao().BuscarEventosPortal(idFallaCier, fechaInicio, fechaFin, version, filtro, idTipoEmpresa,
                idEmpresa, idTipoEquipo, indInterrupcion, nroPage, nroFilas);
        }

        /// <summary>
        /// Permite obtener el nro de registros de la consulta
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="version"></param>
        /// <param name="turno"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoEquipo"></param>
        /// <param name="indInterrupcion"></param>
        /// <returns></returns>
        public int ObtenerNroFilasEvento(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
            string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion)
        {
            string filtro = string.Empty;
            return FactorySic.ObtenerEventoDao().ObtenerNroRegistrosPortal(idTipoEvento, fechaInicio,
                fechaFin, version, filtro, idTipoEmpresa, idEmpresa, idTipoEquipo, indInterrupcion);
        }

        /// <summary>
        /// Permite listar empresas por tipo
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public List<EmpresaDTO> ListarEmpresasPorTipo(int idTipoEmpresa)
        {
            return FactorySic.ObtenerEventoDao().ListarEmpresasPorTipo(idTipoEmpresa);
        }

        /// <summary>
        /// Obtiene los datos para exportar los eventos
        /// </summary>
        /// <param name="idFallaCier"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="version"></param>
        /// <param name="turno"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoEquipo"></param>
        /// <param name="indInterrupcion"></param>
        /// <returns></returns>
        public List<EventoDTO> ExportarEventos(int? idFallaCier, DateTime fechaInicio, DateTime fechaFin,
             string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion)
        {
            string filtro = string.Empty;
            return FactorySic.ObtenerEventoDao().ExportarEventosPortal(idFallaCier, fechaInicio, fechaFin, version, filtro, idTipoEmpresa,
                idEmpresa, idTipoEquipo, indInterrupcion);
        }

        #endregion

        #region Hidrologia

        /// <summary>
        /// Permite obtener el reporte de hidrologia
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public List<DataHidrologia> ObtenerReporteHidrologia(DateTime fechaInicio, DateTime fechaFin, out List<string> columnas)
        {
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerDatosHidrologiaPortal(fechaInicio,
                fechaFin);

            #region Datos para la tabla

            var puntos = list.Select(x => new { x.Ptomedidesc, x.Ptomedicodi }).OrderBy(x => x.Ptomedidesc).Distinct().ToList();
            int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;
            List<string> nombresColumna = puntos.Select(x => x.Ptomedidesc).ToList();
            columnas = new List<string>();

            foreach (string columna in nombresColumna)
            {
                string nombre = columna.Substring(columna.IndexOf(' '), columna.Length - columna.IndexOf(' '));
                columnas.Add(nombre);
            }

            List<DataHidrologia> data = new List<DataHidrologia>();

            for (int i = 0; i <= nroDias; i++)
            {
                DateTime fecha = fechaInicio.AddDays(i);
                List<MeMedicion48DTO> listFecha = list.Where(x => x.Medifecha == fecha).ToList();

                List<DataHidrologia> valoresDia = new List<DataHidrologia>();
                foreach (var item in puntos)
                {
                    MeMedicion48DTO itemHidrologia = listFecha.Where(x => x.Ptomedicodi ==
                        item.Ptomedicodi).FirstOrDefault();
                    List<decimal> valores = new List<decimal>();

                    if (itemHidrologia != null)
                    {
                        for (int j = 1; j <= 48; j++)
                        {
                            valores.Add((decimal)itemHidrologia.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).
                                GetValue(itemHidrologia, null));
                        }
                    }

                    valoresDia.Add(new DataHidrologia { Punto = item.Ptomedicodi, Valores = valores });
                }

                for (int j = 1; j <= 48; j++)
                {
                    DataHidrologia itemData = new DataHidrologia();
                    itemData.Fecha = fecha.AddMinutes(j * 30).ToString(ConstantesAppServicio.FormatoFechaHora);
                    itemData.Valores = new List<decimal>();

                    for (int k = 0; k < puntos.Count; k++)
                    {
                        if (valoresDia.Count == puntos.Count)
                        {
                            if (valoresDia[k].Valores.Count == 48)
                            {
                                itemData.Valores.Add(valoresDia[k].Valores[j - 1]);
                            }
                            else
                            {
                                itemData.Valores.Add(0);
                            }
                        }
                        else
                        {
                            itemData.Valores.Add(0);
                        }
                    }

                    data.Add(itemData);
                }
            }

            #endregion

            return data;
        }

        /// <summary>
        /// Permite obtener los datos para el grafico de hidrologia
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoInfoDesc"></param>
        public List<SeriesXY> ObtenerGraficoReporteHidrologia(DateTime fechaInicio, DateTime fechaFin, string tipoInfoDesc)
        {
            List<SeriesXY> result = new List<SeriesXY>();
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerDatosHidrologiaPortal(fechaInicio,
                fechaFin).Where(x => x.Tipoinfodesc == tipoInfoDesc).ToList();

            var puntos = list.Select(x => new { x.Ptomedidesc, x.Ptomedicodi }).OrderBy(x => x.Ptomedidesc).Distinct().ToList();
            var fechas = list.OrderBy(x => x.Medifecha).Select(x => x.Medifecha).Distinct().ToList();

            foreach (var punto in puntos)
            {
                SeriesXY serie = new SeriesXY();
                serie.Name = punto.Ptomedidesc.Substring(punto.Ptomedidesc.IndexOf(' '), punto.Ptomedidesc.Length -
                    punto.Ptomedidesc.IndexOf(' '));

                foreach (var fecha in fechas)
                {
                    int addMin = 0;
                    for (int i = 1; i <= 48; i++)
                    {
                        addMin = addMin + 30;
                        var point = list.Where(x => x.Ptomedicodi == punto.Ptomedicodi && x.Medifecha == fecha).FirstOrDefault();

                        if (point != null)
                        {
                            var property = point.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(string.Concat("H", i)));
                            if (property != null)
                            {
                                serie.Data.Add(new PuntoSerie
                                {
                                    Nombre = fecha.AddMinutes(addMin).ToString(),
                                    Valor = decimal.Parse(property.GetValue(point).ToString())
                                });
                            }
                        }
                        else
                        {
                            serie.Data.Add(new PuntoSerie { Nombre = fecha.AddMinutes(addMin).ToString(), Valor = 0 });
                        }
                    }
                }

                result.Add(serie);
            }

            return result;
        }

        /// <summary>
        /// Permite exportar los datos de hidrologia
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void ObtenerExportacionHidrologia(DateTime fechaInicio, DateTime fechaFin, string path, string file)
        {
            List<string> puntos = new List<string>();
            List<DataHidrologia> list = this.ObtenerReporteHidrologia(fechaInicio, fechaFin, out puntos);

            file = path + file;
            FileInfo newFile = new FileInfo(file);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }

            using (xlPackage = new ExcelPackage(newFile))
            {

                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("HIDROLOGIA");

                if (ws != null)
                {
                    int index = 4;
                    int j = 1;
                    ws.Cells[index, 1].Value = "FECHA";

                    foreach (var item in puntos)
                    {
                        ws.Cells[index, j + 1].Value = item.Trim();
                        j++;
                    }

                    ExcelRange rg = ws.Cells[index, 1, index, j];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    index++;

                    foreach (DataHidrologia item in list)
                    {
                        j = 1;
                        ws.Cells[index, 1].Value = item.Fecha;

                        foreach (var valor in item.Valores)
                        {
                            ws.Cells[index, j + 1].Value = valor;
                            j++;
                        }

                        index++;
                    }

                    rg = ws.Cells[5, 1, index - 1, j];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    rg = ws.Cells[1, 1, index + 2, j];
                    rg.AutoFitColumns();

                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = ws.Drawings.AddPicture("logo", img);
                    picture.From.Column = 0;
                    picture.From.Row = 1;
                    picture.To.Column = 2;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);
                }

                xlPackage.Save();
            }
        }

        #endregion

        #region Métodos Tabla WB_DECISIONEJECUTADO_DET

        /// <summary>
        /// Inserta un registro de la tabla WB_DECISIONEJECUTADO_DET
        /// </summary>
        public int SaveWbDecisionejecutadoDet(WbDecisionejecutadoDetDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Dejdetcodi == 0)
                {
                    id = FactorySic.GetWbDecisionejecutadoDetRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetWbDecisionejecutadoDetRepository().Update(entity);
                    id = entity.Dejdetcodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar la extension de un archivo
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="extension"></param>
        public void ActualizarDecisionEjecutiva(int codigo, string extension, string file)
        {
            try
            {
                WbDecisionejecutivaDTO entity = FactorySic.GetWbDecisionejecutivaRepository().GetById(codigo);
                entity.Desejeextension = extension;
                entity.Desejefile = file;
                FactorySic.GetWbDecisionejecutivaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_DECISIONEJECUTADO_DET
        /// </summary>
        public void UpdateWbDecisionejecutadoDet(WbDecisionejecutadoDetDTO entity)
        {
            try
            {
                FactorySic.GetWbDecisionejecutadoDetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_DECISIONEJECUTADO_DET
        /// </summary>
        public void DeleteWbDecisionejecutadoDet(int dejdetcodi)
        {
            try
            {
                FactorySic.GetWbDecisionejecutadoDetRepository().DeleteItem(dejdetcodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_DECISIONEJECUTADO_DET
        /// </summary>
        public WbDecisionejecutadoDetDTO GetByIdWbDecisionejecutadoDet(int dejdetcodi)
        {
            return FactorySic.GetWbDecisionejecutadoDetRepository().GetById(dejdetcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_DECISIONEJECUTADO_DET
        /// </summary>
        public List<WbDecisionejecutadoDetDTO> ListWbDecisionejecutadoDets()
        {
            return FactorySic.GetWbDecisionejecutadoDetRepository().List();
        }

        #endregion

        #region Métodos Tabla WB_DECISIONEJECUTIVA

        /// <summary>
        /// Inserta un registro de la tabla WB_DECISIONEJECUTIVA
        /// </summary>
        public int SaveWbDecisionejecutiva(WbDecisionejecutivaDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Desejecodi == 0)
                {
                    id = FactorySic.GetWbDecisionejecutivaRepository().Save(entity);
                }
                else
                {
                    WbDecisionejecutivaDTO item = FactorySic.GetWbDecisionejecutivaRepository().GetById(entity.Desejecodi);
                    entity.Desejefile = item.Desejefile;
                    entity.Desejeextension = item.Desejeextension;

                    FactorySic.GetWbDecisionejecutivaRepository().Update(entity);
                    id = entity.Desejecodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar la descripcion de los items
        /// </summary>
        /// <param name="datos"></param>
        public void ActualizarDescripcionItemDecision(string[][] datos)
        {
            try
            {
                if (datos != null)
                {
                    foreach (string[] item in datos)
                    {
                        if (item.Count() == 2)
                        {
                            int id = int.Parse(item[0]);
                            string txt = item[1];

                            FactorySic.GetWbDecisionejecutadoDetRepository().ActualizarDescripcion(id, txt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_DECISIONEJECUTIVA
        /// </summary>
        public void UpdateWbDecisionejecutiva(WbDecisionejecutivaDTO entity)
        {
            try
            {
                FactorySic.GetWbDecisionejecutivaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_DECISIONEJECUTIVA
        /// </summary>
        public void DeleteWbDecisionejecutiva(int desejecodi)
        {
            try
            {
                FactorySic.GetWbDecisionejecutadoDetRepository().Delete(desejecodi);
                FactorySic.GetWbDecisionejecutivaRepository().Delete(desejecodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_DECISIONEJECUTIVA
        /// </summary>
        public WbDecisionejecutivaDTO GetByIdWbDecisionejecutiva(int desejecodi)
        {
            WbDecisionejecutivaDTO entity = FactorySic.GetWbDecisionejecutivaRepository().GetById(desejecodi);
            List<WbDecisionejecutadoDetDTO> itemList = FactorySic.GetWbDecisionejecutadoDetRepository().GetByCriteria(entity.Desejecodi);
            entity.ListaCarta = itemList.Where(x => x.Dejdettipo == ConstantesPortal.ArchivoCarta.ToString()).OrderBy(x => x.Dejdetcodi).ToList();
            entity.ListaAntecedentes = itemList.Where(x => x.Dejdettipo == ConstantesPortal.ArchivoAntecendente.ToString()).OrderBy(x => x.Dejdetcodi).ToList();
            entity.ListaItems = itemList;
            return entity;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_DECISIONEJECUTIVA
        /// </summary>
        public List<WbDecisionejecutivaDTO> ListWbDecisionejecutivas()
        {
            return FactorySic.GetWbDecisionejecutivaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbDecisionejecutiva
        /// </summary>
        public List<WbDecisionejecutivaDTO> GetByCriteriaWbDecisionejecutivas(string tipo)
        {
            List<WbDecisionejecutivaDTO> entitys = FactorySic.GetWbDecisionejecutivaRepository().GetByCriteria(tipo);

            foreach (WbDecisionejecutivaDTO entity in entitys)
            {
                List<WbDecisionejecutadoDetDTO> itemList = FactorySic.GetWbDecisionejecutadoDetRepository().GetByCriteria(entity.Desejecodi);
                entity.ListaCarta = itemList.Where(x => x.Dejdettipo == ConstantesPortal.ArchivoCarta.ToString()).OrderBy(x => x.Dejdetcodi).ToList();
                entity.ListaAntecedentes = itemList.Where(x => x.Dejdettipo == ConstantesPortal.ArchivoAntecendente.ToString()).OrderBy(x => x.Dejdetcodi).ToList();
                entity.ListaItems = itemList;
            }

            return entitys;
        }

        #endregion

        #region Calendario COES

        /// <summary>
        /// Inserta un registro de la tabla WB_CALENDARIO
        /// </summary>
        public int SaveWbCalendario(WbCalendarioDTO entity)
        {
            try
            {
                int idEvento = 0;

                if (entity.Calendcodi == 0)
                {
                    idEvento = FactorySic.GetWbCalendarioRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetWbCalendarioRepository().Update(entity);
                    idEvento = entity.Calendcodi;
                }

                return idEvento;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Elimina un registro de la tabla WB_CALENDARIO
        /// </summary>
        public void DeleteWbCalendario(int calendcodi)
        {
            try
            {
                FactorySic.GetWbCalendarioRepository().Delete(calendcodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_CALENDARIO
        /// </summary>
        public WbCalendarioDTO GetByIdWbCalendario(int calendcodi)
        {
            return FactorySic.GetWbCalendarioRepository().GetById(calendcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_CALENDARIO
        /// </summary>
        public List<WbCalendarioDTO> ListWbCalendarios()
        {
            List<WbCalendarioDTO> list = FactorySic.GetWbCalendarioRepository().List();
            foreach (WbCalendarioDTO item in list)
            {
                if (!string.IsNullOrEmpty(item.Calenddescripcion))
                {
                    item.Calenddescripcion = item.Calenddescripcion.Replace(System.Environment.NewLine, " ");
                }
            }
            return list;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbCalendario
        /// </summary>
        public List<WbCalendarioDTO> GetByCriteriaWbCalendarios(string nombre, DateTime fechaInicio, DateTime fechaFin)
        {
            List<WbCalendarioDTO> list = FactorySic.GetWbCalendarioRepository().GetByCriteria(nombre, fechaInicio, fechaFin);
            //this.ColocarAtributosAdicionales(ref list);
            return list;
        }

        /// <summary>
        /// Permite agregar las lineas adicionales
        /// </summary>
        /// <returns></returns>
        public void ColocarAtributosAdicionales(ref List<WbCalendarioDTO> list)
        {
            foreach (WbCalendarioDTO item in list)
            {
                if (item.Calendtipo == ConstantesPortal.TipoEventoPublicacion)
                {
                    item.Calendicon = ConstantesPortal.IconoEventoPublicacion;
                    item.Calendcolor = ConstantesPortal.ColorEventoPublicacion;
                }
                else if (item.Calendtipo == ConstantesPortal.TipoEventoReunion)
                {
                    item.Calendicon = ConstantesPortal.IconoEventoReunion;
                    item.Calendcolor = ConstantesPortal.ColorEventoReunion;
                }
                else if (item.Calendtipo == ConstantesPortal.TipoEventoVencimiento)
                {
                    item.Calendicon = ConstantesPortal.IconoEventoVencimiento;
                    item.Calendcolor = ConstantesPortal.ColorEventoVencimiento;
                }
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla WB_MESCALENDARIO
        /// </summary>
        public int SaveWbMescalendario(WbMescalendarioDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Mescalcodi == 0)
                {
                    id = FactorySic.GetWbMescalendarioRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetWbMescalendarioRepository().Update(entity);

                    if (!string.IsNullOrEmpty(entity.Mescalinfo))
                    {
                        FactorySic.GetWbMescalendarioRepository().ActualizarInfografia(entity.Mescalcodi, entity.Mescalinfo);
                    }

                    id = entity.Mescalcodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_MESCALENDARIO
        /// </summary>
        public void DeleteWbMescalendario(int mescalcodi)
        {
            try
            {
                FactorySic.GetWbMescalendarioRepository().Delete(mescalcodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Elimina un registro de la tabla WB_MESCALENDARIO
        /// </summary>
        public void QuitarImagen(int mescalcodi)
        {
            try
            {
                FactorySic.GetWbMescalendarioRepository().QuitarImagen(mescalcodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener un registro de la tabla WB_MESCALENDARIO
        /// </summary>
        public WbMescalendarioDTO GetByIdWbMescalendario(int mescalcodi)
        {
            return FactorySic.GetWbMescalendarioRepository().GetById(mescalcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_MESCALENDARIO
        /// </summary>
        public List<WbMescalendarioDTO> ListWbMescalendarios()
        {
            return FactorySic.GetWbMescalendarioRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbMescalendario
        /// </summary>
        public List<WbMescalendarioDTO> GetByCriteriaWbMescalendarios(int? anio, int? mes)
        {
            List<WbMescalendarioDTO> list = FactorySic.GetWbMescalendarioRepository().GetByCriteria(anio, mes);

            foreach (WbMescalendarioDTO item in list)
            {
                item.Mescalnombmes = COES.Base.Tools.Util.ObtenerNombreMes((int)item.Mescalmes);
            }

            return list;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla DOC_DIA_ESP
        /// </summary>
        public List<DocDiaEspDTO> ObtenerDiasFeriados()
        {
            return FactorySic.GetDocDiaEspRepository().List();
        }

        #endregion

        #region Métodos Tabla WB_CALTIPOVENTO

        /// <summary>
        /// Inserta un registro de la tabla WB_CALTIPOVENTO
        /// </summary>
        public int SaveWbCaltipovento(WbCaltipoventoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Tipcalcodi == 0)
                {
                    id = FactorySic.GetWbCaltipoventoRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetWbCaltipoventoRepository().Update(entity);
                    id = entity.Tipcalcodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_CALTIPOVENTO
        /// </summary>
        public void UpdateWbCaltipovento(WbCaltipoventoDTO entity)
        {
            try
            {
                FactorySic.GetWbCaltipoventoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_CALTIPOVENTO
        /// </summary>
        public void DeleteWbCaltipovento(int tipcalcodi)
        {
            try
            {
                FactorySic.GetWbCaltipoventoRepository().Delete(tipcalcodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_CALTIPOVENTO
        /// </summary>
        public WbCaltipoventoDTO GetByIdWbCaltipovento(int tipcalcodi)
        {
            return FactorySic.GetWbCaltipoventoRepository().GetById(tipcalcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_CALTIPOVENTO
        /// </summary>
        public List<WbCaltipoventoDTO> ListWbCaltipoventos()
        {
            return FactorySic.GetWbCaltipoventoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbCaltipovento
        /// </summary>
        public List<WbCaltipoventoDTO> GetByCriteriaWbCaltipoventos()
        {
            return FactorySic.GetWbCaltipoventoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla WB_BLOB

        /// <summary>
        /// Permite listar en la tabla WB_BLOB
        /// </summary>
        public List<WbBlobDTO> GetWbBlobByUrlParcial(string url, DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetWbBlobRepository().ObtenerByUrlParcial(url, fechaInicio,fechaFin);
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public WbResumenmdDTO ObtenerResumenMD(string fechaConsulta)
        {
            DateTime fecha = DateTime.ParseExact(fechaConsulta, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
            //DateTime fechaCompara = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            //if (fecha.Subtract(fechaCompara).TotalMinutes >= 0) fecha = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);

            WbResumenmdDTO resumen = null;
            
            resumen = FactorySic.GetWbResumenmdRepository().VerificarExistencia(fecha);

            if (resumen == null)
            {
                resumen = FactorySic.GetWbResumenmdRepository().VerificarExistencia(fecha.AddMonths(-1));
            }

            if (resumen == null)
            {
                resumen = FactorySic.GetWbResumenmdRepository().VerificarExistencia(fecha.AddMonths(-2));
            }

            return resumen;
        }

        /// <summary>
        /// Permite obtener los datos de máxima demanda
        /// </summary>
        public GraficoMaximaDemanda ObtenerReporteMaximaDemanda(WbResumenmdDTO resumen)
        {
            List<SiFuenteenergiaDTO> fuenteEnergia = FactorySic.GetSiFuenteenergiaRepository().GetByCriteria();
            int estadoValidacion = 1;
            int tipoCentral = ConstantesMedicion.IdTipogrupoCOES; //COES 
            int tipoGeneracion = ConstantesMedicion.IdTipoGeneracionTodos; //TODOS

            List<ConsolidadoEnvioDTO> listRecursoEnergetico = (new ReporteMedidoresAppServicio()).GetRecursoEnergeticoMaximaDemanda96((DateTime)resumen.Resmdfecha,
                tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);

            //List<MaximaDemandaDTO> diagramaCarga = (new ReporteMedidoresAppServicio()).GetResumenDetalleDiaMaximaDemanda96((DateTime)resumen.Resmdfecha, tipoCentral, tipoGeneracion,
            //    ConstantesMedicion.IdEmpresaTodos, estadoValidacion, DateTime.Now);

            List<PuntoSerie> puntosFE = new List<PuntoSerie>();

            foreach (ConsolidadoEnvioDTO item in listRecursoEnergetico)
            {
                PuntoSerie itemFE = new PuntoSerie();
                itemFE.Nombre = item.Fenergnomb;
                itemFE.Valor = item.Total;
                itemFE.CodColor = item.Fenercolor;
                //if (itemFE.Valor > 0)
                puntosFE.Add(itemFE);
            }

            List<PuntoSerie> puntosTG = new List<PuntoSerie>();
            var tiposGeneracion = fuenteEnergia.Select(x => new { x.Tgenercodi, x.Tgenernomb }).Distinct().ToList();

            foreach (var item in tiposGeneracion)
            {
                List<int> idsFuente = fuenteEnergia.Where(x => x.Tgenercodi == item.Tgenercodi).Select(x => x.Fenergcodi).ToList();
                List<ConsolidadoEnvioDTO> subList = listRecursoEnergetico.Where(x => idsFuente.Any(y => x.Fenergcodi == y)).ToList();

                PuntoSerie itemTG = new PuntoSerie();
                itemTG.Nombre = item.Tgenernomb;
                itemTG.Valor = subList.Sum(x => x.Total);
                itemTG.CodColor = this.GetColorCombustible(itemTG.Nombre);
                puntosTG.Add(itemTG);
            }

            /*===DIAGRAMA DE CARGA===*/

            List<MeMedicion96DTO> list = (new RankingConsolidadoAppServicio()).ObtenerDatosEvolucionRER((DateTime)resumen.Resmdfecha, (DateTime)resumen.Resmdfecha,
                ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, tipoCentral);

            List<PuntoSerie> puntosDiagramaCarga = new List<PuntoSerie>();

            foreach (MeMedicion96DTO item in list)
            {
                PuntoSerie serie = new PuntoSerie();
                List<decimal> valores = new List<decimal>();
                for (int i = 1; i <= 96; i++)
                {
                    var valor = item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i.ToString()).GetValue(item, null);
                    valores.Add((decimal)valor);
                }

                serie.Nombre = item.Fenergnomb;
                serie.Valores = valores;
                serie.CodColor = item.Fenercolor;
                serie.Valor = valores.Sum();
                serie.Codigo = item.Fenergcodi;
                serie.RER = item.Tipogenerrer;
                puntosDiagramaCarga.Add(serie);
            }

            //List<PuntoSerie> listPunto = puntosDiagramaCarga.OrderByDescending(x => x.Valor).ToList();
            //int len = listPunto.Count();

            //for (int i = 1; i < len; i++)
            //{
            //    listPunto[i].Valores = this.ObtenerSuma(listPunto[i].Valores, listPunto[i - 1].Valores);
            //    listPunto[i].Valor = listPunto[i].Valores.Sum();
            //}

            List<PuntoSerie> seriesOrdenado = new List<PuntoSerie>();

            PuntoSerie serieSolar = puntosDiagramaCarga.Where(x => x.Codigo == 8).FirstOrDefault();
            if (serieSolar != null)
            {
                //serieSolar.Valores = this.ObtenerSuma(serieSolar.Valores, serieAgua.Valores);
                serieSolar.Valor = serieSolar.Valores.Sum();
                seriesOrdenado.Add(serieSolar);
            }

            PuntoSerie serieEolica = puntosDiagramaCarga.Where(x => x.Codigo == 9).FirstOrDefault();
            if (serieEolica != null)
            {
                //serieEolica.Valores = serieEolica.Valores;
                serieEolica.Valor = serieEolica.Valores.Sum();
                seriesOrdenado.Add(serieEolica);
            }
            
            PuntoSerie serieAguaS = puntosDiagramaCarga.Where(x => x.Codigo == 1 & x.RER == "S").FirstOrDefault();
            if (serieAguaS != null)
            {
                //serieAguaS.Valores = serieAguaS.Valores;
                serieAguaS.Valor = serieAguaS.Valores.Sum();
                seriesOrdenado.Add(serieAguaS);
            }

            PuntoSerie serieAguaN = puntosDiagramaCarga.Where(x => x.Codigo == 1 & x.RER == "N").FirstOrDefault();
            if (serieAguaN != null)
            {
                //serieAguaN.Valores = serieAguaN.Valores;
                serieAguaN.Valor = serieAguaN.Valores.Sum();
                seriesOrdenado.Add(serieAguaN);
            }

            PuntoSerie serieCarbon = puntosDiagramaCarga.Where(x => x.Codigo == 5).FirstOrDefault();
            if (serieCarbon != null)
            {               
                //serieCarbon.Valores = serieCarbon.Valores;                
                serieCarbon.Valor = serieCarbon.Valores.Sum();
                seriesOrdenado.Add(serieCarbon);
            }

            PuntoSerie serieGas = puntosDiagramaCarga.Where(x => x.Codigo == 2).FirstOrDefault();
            if (serieGas != null)
            {
                //serieGas.Valores = serieGas.Valores;
                serieGas.Valor = serieGas.Valores.Sum();
                seriesOrdenado.Add(serieGas);
            }

            PuntoSerie serieBagazo = puntosDiagramaCarga.Where(x => x.Codigo == 6).FirstOrDefault();
            if (serieBagazo != null)
            {
                /*if (serieBagazo.Valores.Sum() > 0)
                {
                    serieBagazo.Valores = serieBagazo.Valores;
                }*/ 
                serieBagazo.Valor = serieBagazo.Valores.Sum();
                seriesOrdenado.Add(serieBagazo);
            }

            PuntoSerie serieBiogas = puntosDiagramaCarga.Where(x => x.Codigo == 7).FirstOrDefault();
            if (serieBiogas != null)
            {
                //serieBiogas.Valores = serieBiogas.Valores;
                serieBiogas.Valor = serieBiogas.Valores.Sum();
                seriesOrdenado.Add(serieBiogas);
            }

            PuntoSerie serieResidual = puntosDiagramaCarga.Where(x => x.Codigo == 4).FirstOrDefault();
            if (serieResidual != null)
            {
                //List<decimal> sumValores = this.ObtenerValoresLista(serieBiogas);
                //serieResidual.Valores = serieResidual.Valores;
                serieResidual.Valor = serieResidual.Valores.Sum();
                seriesOrdenado.Add(serieResidual);
            }

            PuntoSerie serieDiesel = puntosDiagramaCarga.Where(x => x.Codigo == 3).FirstOrDefault();
            if (serieDiesel != null)
            {
                //serieDiesel.Valores = serieDiesel.Valores;
                serieDiesel.Valor = serieDiesel.Valores.Sum();
                seriesOrdenado.Add(serieDiesel);
            }

            PuntoSerie serieNafta = puntosDiagramaCarga.Where(x => x.Codigo == 15).FirstOrDefault();
            if (serieNafta != null)
            {
                //serieNafta.Valores = serieNafta.Valores;
                serieNafta.Valor = serieNafta.Valores.Sum();
                seriesOrdenado.Add(serieNafta);
            }

            List<string> ejexDiagramaCarga = new List<string>();
            DateTime fechaDatos = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            for (int i = 1; i <= 96; i++)
            {
                DateTime fechaLeyenda = fechaDatos.AddMinutes(15 * i);
                ejexDiagramaCarga.Add(fechaLeyenda.ToString("HH:mm"));
            }

            GraficoMaximaDemanda result = new GraficoMaximaDemanda();
            result.GraficoFuenteEnergia = puntosFE;
            result.GraficoTipoGeneracion = puntosTG;
            result.GraficoDiagramaCarga = seriesOrdenado.OrderByDescending(x => x.Valor).ThenByDescending(x => x.Nombre).ToList();
            result.ValorMD = (decimal)resumen.Resmdvalor;
            result.FechaMD = ((DateTime)resumen.Resmdfecha).ToString("dd/MM/yyyy");
            result.HoraMD = ((DateTime)resumen.Resmdfecha).ToString("HH:mm");
            result.EjexDiagramaCarga = ejexDiagramaCarga;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaconsulta"></param>
        /// <returns></returns>
        public GraficoMaximaDemanda ObtenerDiagramaCarga(string fechaconsulta)
        {
            int tipoCentral = ConstantesMedicion.IdTipogrupoCOES; //COES 

            DateTime fecha = DateTime.ParseExact(fechaconsulta, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime hoy = DateTime.Today;

            if (fecha.Date == hoy)
            {
                DateTime fechaMes = DateTime.Now.AddMonths(-1);
                DateTime ultimoDiaMesAnterior = new DateTime(fechaMes.Year, fechaMes.Month, DateTime.DaysInMonth(fechaMes.Year, fechaMes.Month));
                string ultimoDiaMesAnt = ultimoDiaMesAnterior.ToString("yyyy-MM-dd");
                fecha = ultimoDiaMesAnterior;
            }

            /*===DIAGRAMA DE CARGA===*/

            List<MeMedicion96DTO> list = (new RankingConsolidadoAppServicio()).ObtenerDatosEvolucionRER((DateTime)fecha, (DateTime)fecha,
                ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, tipoCentral);

            List<PuntoSerie> puntosDiagramaCarga = new List<PuntoSerie>();

            foreach (MeMedicion96DTO item in list)
            {
                PuntoSerie serie = new PuntoSerie();
                List<decimal> valores = new List<decimal>();
                for (int i = 1; i <= 96; i++)
                {
                    var valor = item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i.ToString()).GetValue(item, null);
                    valores.Add((decimal)valor);
                }

                serie.Nombre = item.Fenergnomb;
                serie.Valores = valores;
                serie.CodColor = item.Fenercolor;
                serie.Valor = valores.Sum();
                serie.Codigo = item.Fenergcodi;
                serie.RER = item.Tipogenerrer;
                puntosDiagramaCarga.Add(serie);
            }

            List<PuntoSerie> seriesOrdenado = new List<PuntoSerie>();

            PuntoSerie serieSolar = puntosDiagramaCarga.Where(x => x.Codigo == 8).FirstOrDefault();
            if (serieSolar != null)
            {
                //serieSolar.Valores = this.ObtenerSuma(serieSolar.Valores, serieAgua.Valores);
                serieSolar.Valor = serieSolar.Valores.Sum();
                seriesOrdenado.Add(serieSolar);
            }

            PuntoSerie serieEolica = puntosDiagramaCarga.Where(x => x.Codigo == 9).FirstOrDefault();
            if (serieEolica != null)
            {
                //serieEolica.Valores = serieEolica.Valores;
                serieEolica.Valor = serieEolica.Valores.Sum();
                seriesOrdenado.Add(serieEolica);
            }

            PuntoSerie serieAguaS = puntosDiagramaCarga.Where(x => x.Codigo == 1 & x.RER == "S").FirstOrDefault();
            if (serieAguaS != null)
            {
                //serieAguaS.Valores = serieAguaS.Valores;
                serieAguaS.Valor = serieAguaS.Valores.Sum();
                seriesOrdenado.Add(serieAguaS);
            }

            PuntoSerie serieAguaN = puntosDiagramaCarga.Where(x => x.Codigo == 1 & x.RER == "N").FirstOrDefault();
            if (serieAguaN != null)
            {
                //serieAguaN.Valores = serieAguaN.Valores;
                serieAguaN.Valor = serieAguaN.Valores.Sum();
                seriesOrdenado.Add(serieAguaN);
            }

            PuntoSerie serieCarbon = puntosDiagramaCarga.Where(x => x.Codigo == 5).FirstOrDefault();
            if (serieCarbon != null)
            {
                //serieCarbon.Valores = serieCarbon.Valores;                
                serieCarbon.Valor = serieCarbon.Valores.Sum();
                seriesOrdenado.Add(serieCarbon);
            }

            PuntoSerie serieGas = puntosDiagramaCarga.Where(x => x.Codigo == 2).FirstOrDefault();
            if (serieGas != null)
            {
                //serieGas.Valores = serieGas.Valores;
                serieGas.Valor = serieGas.Valores.Sum();
                seriesOrdenado.Add(serieGas);
            }

            PuntoSerie serieBagazo = puntosDiagramaCarga.Where(x => x.Codigo == 6).FirstOrDefault();
            if (serieBagazo != null)
            {
                /*if (serieBagazo.Valores.Sum() > 0)
                {
                    serieBagazo.Valores = serieBagazo.Valores;
                }*/
                serieBagazo.Valor = serieBagazo.Valores.Sum();
                seriesOrdenado.Add(serieBagazo);
            }

            PuntoSerie serieBiogas = puntosDiagramaCarga.Where(x => x.Codigo == 7).FirstOrDefault();
            if (serieBiogas != null)
            {
                //serieBiogas.Valores = serieBiogas.Valores;
                serieBiogas.Valor = serieBiogas.Valores.Sum();
                seriesOrdenado.Add(serieBiogas);
            }

            PuntoSerie serieResidual = puntosDiagramaCarga.Where(x => x.Codigo == 4).FirstOrDefault();
            if (serieResidual != null)
            {
                //List<decimal> sumValores = this.ObtenerValoresLista(serieBiogas);
                //serieResidual.Valores = serieResidual.Valores;
                serieResidual.Valor = serieResidual.Valores.Sum();
                seriesOrdenado.Add(serieResidual);
            }

            PuntoSerie serieDiesel = puntosDiagramaCarga.Where(x => x.Codigo == 3).FirstOrDefault();
            if (serieDiesel != null)
            {
                //serieDiesel.Valores = serieDiesel.Valores;
                serieDiesel.Valor = serieDiesel.Valores.Sum();
                seriesOrdenado.Add(serieDiesel);
            }

            PuntoSerie serieNafta = puntosDiagramaCarga.Where(x => x.Codigo == 15).FirstOrDefault();
            if (serieNafta != null)
            {
                //serieNafta.Valores = serieNafta.Valores;
                serieNafta.Valor = serieNafta.Valores.Sum();
                seriesOrdenado.Add(serieNafta);
            }

            List<string> ejexDiagramaCarga = new List<string>();
            DateTime fechaDatos = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            for (int i = 1; i <= 96; i++)
            {
                DateTime fechaLeyenda = fechaDatos.AddMinutes(15 * i);
                ejexDiagramaCarga.Add(fechaLeyenda.ToString("HH:mm"));
            }

            GraficoMaximaDemanda result = new GraficoMaximaDemanda();
            result.GraficoDiagramaCarga = seriesOrdenado.OrderByDescending(x => x.Valor).ThenByDescending(x => x.Nombre).ToList();
            result.FechaMD = ((DateTime)fecha).ToString("dd/MM/yyyy");
            result.EjexDiagramaCarga = ejexDiagramaCarga;
            return result;
        }

        /// <summary>
        /// Permite obtener los datos de máxima demanda
        /// </summary>
        public GraficoMaximaDemanda ObtenerMaximaDemanda(WbResumenmdDTO resumen)
        {
            List<SiTipogeneracionDTO> tiposGeneracion = FactorySic.GetSiTipogeneracionRepository().GetByCriteria().Where(x => x.Tgenercodi > 0).OrderBy(x => x.Tgenercodi).ToList();
            
            var listaMedicion = (new ReporteMedidoresAppServicio()).GetResumenDetalleDiaMaximaDemanda(resumen.Resmdcodi);

            List <PuntoSerie> puntosTG = new List<PuntoSerie>();

            foreach (var item in tiposGeneracion)
            {
                List<WbResumenmddetalleDTO> subList = listaMedicion.Where(x => x.Tgenerercodi == item.Tgenercodi).ToList();

                PuntoSerie itemTG = new PuntoSerie();
                itemTG.Nombre = item.Tgenernomb;
                itemTG.Valor = (decimal) subList.Sum(x => x.Resmddvalor);
                itemTG.CodColor = item.Tgenercolor;
                puntosTG.Add(itemTG);
            }

            /*==FILTRO POR EMPRESA===*/

            List<PuntoSerie> puntosEmpresa = new List<PuntoSerie>();
            

            var empresas = listaMedicion.Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();

            foreach (var emp in empresas)
            {
                List<PuntoSerie> puntosEmpresaTg = new List<PuntoSerie>();
                List<WbResumenmddetalleDTO> subEmpresa = listaMedicion.Where(x => x.Emprcodi == emp.Emprcodi).ToList();

                PuntoSerie itemEmprsa = new PuntoSerie();
                itemEmprsa.Nombre = emp.Emprnomb;
                itemEmprsa.Valor = (decimal) subEmpresa.Sum(x => x.Resmddvalor);

                foreach (var item in tiposGeneracion)
                {
                    List<WbResumenmddetalleDTO> subEmpTg = subEmpresa.Where(x => x.Tgenerercodi == item.Tgenercodi).ToList();

                    PuntoSerie itemEmpTG = new PuntoSerie();
                    itemEmpTG.Nombre = item.Tgenernomb;
                    itemEmpTG.Valor = (decimal)subEmpTg.Sum(x => x.Resmddvalor);
                    itemEmpTG.CodColor = item.Tgenercolor;
                    puntosEmpresaTg.Add(itemEmpTG);
                }

                itemEmprsa.Valores_ = puntosEmpresaTg;

                puntosEmpresa.Add(itemEmprsa);
            }
            
            GraficoMaximaDemanda result = new GraficoMaximaDemanda();
            result.FiltroTipoGeneracion = puntosTG;
            result.FiltroEmpresa = puntosEmpresa;
            result.ValorMD = (decimal)resumen.Resmdvalor;
            result.FechaMD = ((DateTime)resumen.Resmdfecha).ToString("dd/MM/yyyy");
            result.HoraMD = ((DateTime)resumen.Resmdfecha).ToString("HH:mm");
            return result;
        }

        private List<decimal> ObtenerSuma(List<decimal> a, List<decimal> b)
        {
            int len = a.Count();
            decimal[] result = new decimal[len];

            for (int i = 0; i < len; i++)
            {
                result[i] = a[i] + b[i];
            }

            return result.ToList();
        }

        private List<decimal> ObtenerValoresLista(PuntoSerie lista)
        {
            List<decimal> sumValores = new List<decimal>();

            if (lista != null)
            {
                sumValores = lista.Valores;
            }
            else
            {
                for (int i = 0; i < 96; i++)
                {
                    sumValores.Add(0);
                }
            }

            return sumValores;
        }

        /// <summary>
        /// Permite obtener los datos resumen del app movil
        /// </summary>
        /// <returns></returns>
        public DatosResumenMovil ObtenerDatosResumen()
        {
            DatosResumenMovil entity = new DatosResumenMovil();

            //- Obtención de la ultima frecuencia
            int gps = Convert.ToInt32(ConfigurationManager.AppSettings["GpsFrecuencia"].ToString());
            List<GraficoFrecuencia> listFrecuencia = this.ObtenerFrecuenciaSein(gps);
            entity.Frecuencia = listFrecuencia[listFrecuencia.Count - 1].Valor;

            //- Obtención de la maxima demanda     
            //DateTime fechaDM = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
            WbResumenmdDTO datosMD = this.ObtenerResumenMD(DateTime.Now.AddDays(-1).ToString(ConstantesAppServicio.FormatoMesAnio));
            entity.MaximaDemanda = (decimal)datosMD.Resmdvalor;

            //- Obtención de la demanda pico ejecutado del dia anterior
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerDemandaPicoDiaAnterior(DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(-1));
            decimal demandaPico = 0;

            if (list.Count > 0)
            {
                MeMedicion48DTO resumenDemandaPico = list[0];
                demandaPico = (decimal)resumenDemandaPico.H1;
            }

            entity.DemandaPico = demandaPico;

            //- Obtención de los datos de costos marginales
            CmCostomarginalDTO entityCostoMarginal = (new CortoPlazoAppServicio()).ObtenerResumenCostoMarginal();
            entity.CostoMarginal = 0;

            if (entityCostoMarginal != null)
            {
                entity.CostoMarginal = (decimal)entityCostoMarginal.Cmgntotal;
                entity.TextoCostoMarginalHome = string.Format(@"CMg Santa Rosa 220
{0} - (S/. /MWh)", entityCostoMarginal.Cmgnfecha.ToString("HH:mm"));
                entity.TextoCostoMarginal = "CMg en TR";
            }

            return entity;
        }

        #region Notificacion
        /// <summary>
        /// Permite realizar búsquedas en la tabla APM_NOTIFICACION
        /// </summary>
        public List<WbNotificacionDTO> ListNotificaciones(string titulo, DateTime? fechaInicio, DateTime? fechaFin)
        {
            return FactorySic.GetWbNotificacionRepository().GetByCriteria(titulo, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Elimina un registro de la tabla APM_NOTIFICACION
        /// </summary>
        public void DeleteNotificacion(int notiCodi)
        {
            try
            {
                FactorySic.GetWbNotificacionRepository().Delete(notiCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// Guarda un registro de la tabla APM_NOTIFICACION
        public int SaveWbNotificacion(WbNotificacionDTO entity)
        {
            try
            {
                int notiCodi = 0;

                if (entity.NotiCodi == 0)
                {
                    notiCodi = FactorySic.GetWbNotificacionRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetWbNotificacionRepository().Update(entity);
                    notiCodi = entity.NotiCodi;
                }

                return notiCodi;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla APM_NOTIFICACION
        /// </summary>
        public WbNotificacionDTO GetByIdNotificacion(int notiCodi)
        {
            WbNotificacionDTO entity = new WbNotificacionDTO();
            if (notiCodi != 0)
                entity = FactorySic.GetWbNotificacionRepository().GetById(notiCodi);

            return entity;
        }

        public Response EnviarNotificacion()
        {
            var response = new Response();

            try
            {
                var notificaciones = FactorySic.GetWbNotificacionRepository().List();
                foreach (var item in notificaciones)
                {
                    SendNotificationFCM(item.NotiTitulo, item.NotiDescripcion);
                    FactorySic.GetWbNotificacionRepository().CambiarEstadoNotificacion(item.NotiCodi);
                }

            }
            catch (Exception e)
            {
                response.Mensaje = e.Message;
                response.Estado = false;
                response.Trace = e.StackTrace;
            }
            return response;
        }

        /*private void SendNotificationFCM(string title, string body)
        {
            var serverKey = ConfigurationManager.AppSettings["ApiKeyFireBase"].ToString();
            var result = "-1";
            var webAddr = "https://fcm.googleapis.com/fcm/send";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
            httpWebRequest.Method = "POST";
            dynamic request = new System.Dynamic.ExpandoObject();
            dynamic data = new System.Dynamic.ExpandoObject();
            dynamic notification = new System.Dynamic.ExpandoObject();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                request.to = "/topics/ALL";
                data.title = title;
                data.body = body;
                data.small_icon = "ic_notifications_v2";
                data.large_icon = "events_active";
                notification.title = title;
                notification.body = body;
                request.data = data;
                request.notification = notification;
                //string json = "{\"to\": \"/topics/ALL\",\"notification\": {\"title\": \"" + title + "\",\"body\": \"" + body + "\"}}";
                var str = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                streamWriter.Write(Newtonsoft.Json.JsonConvert.SerializeObject(request));
                streamWriter.Flush();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
        }*/

        private async void SendNotificationFCM(string title, string body)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(ConfigurationManager.AppSettings["FCMGoogleCredential"].ToString())
            });

            var message = new FirebaseAdmin.Messaging.Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                // Enviar a todos los dispositivos, usa el topic "all" o cualquier otro topic común.
                Topic = "ALL",
            };

            try
            {
                // Enviar el mensaje
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Notificación enviada con éxito: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar notificación: {ex.Message}");
            }

        }

        /// <summary>
        /// Permite obtener la version actual APP
        /// </summary>
        /// <returns></returns>
        public WbVersionappDTO ObtenerVersionAPP()
        {
            return FactorySic.GetWbVersionappRepository().ObtenerVersionActual();
        }

        #endregion


        #region Calculo de la MD mensual

        /// <summary>
        /// Permite calcular el valor de la maxima demanda mensual para el aplicativo movil
        /// </summary>
        /// <param name="fecha"></param>
        public void CalcularMaximaDemandaMensual(DateTime fecha)
        {
            DateTime fechaProceso = new DateTime(fecha.AddMonths(-1).Year, fecha.AddMonths(-1).Month, 1);
            MaximaDemandaDTO resumen = (new ReporteMedidoresAppServicio()).GetDiaMaximaDemanda96XMes(fechaProceso, true);

            if (resumen != null)
            {
                WbResumenmdDTO entity = new WbResumenmdDTO
                {
                    Lastdate = DateTime.Now,
                    Resmdfecha = resumen.FechaHora,
                    Resmdvalor = resumen.Valor,
                    Resmdmes = fechaProceso
                };

                //- Verificamos la existencia de algun registro anterior
                WbResumenmdDTO anterior = FactorySic.GetWbResumenmdRepository().VerificarExistencia(fechaProceso);

                if (anterior != null)
                {
                    entity.Resmdcodi = anterior.Resmdcodi;
                    FactorySic.GetWbResumenmdRepository().Update(entity);
                }
                else
                {
                    FactorySic.GetWbResumenmdRepository().Save(entity);
                }
            }
        }

        #endregion

        #region Costo marginal VS Tarifa en barra

        /// <summary>
        /// Permite listar los CM vs Tarifa en barra
        /// </summary>
        /// <returns></returns>
        public List<WbCmvstarifaDTO> ListarCmVsTarifa()
        {
            return FactorySic.GetWbCmvstarifaRepository().List();
        }

        /// <summary>
        /// Permite realizar la carga de datos de CM VS Tarifa en Barra
        /// </summary>
        /// <param name="list"></param>
        public void CargarCmVsTarifa(List<WbCmvstarifaDTO> list, string usuario)
        {
            List<WbCmvstarifaDTO> result = this.ListarCmVsTarifa();
            List<WbCmvstarifaDTO> entitys = list.Where(x => !result.Any(y => x.Cmtarfecha == y.Cmtarfecha)).ToList();

            foreach (WbCmvstarifaDTO entity in entitys)
            {
                entity.Cmtarusucreacion = usuario;
                entity.Cmtarusumodificacion = usuario;
                entity.Cmtarfeccreacion = DateTime.Now;
                entity.Cmtarfecmodificacion = DateTime.Now;

                FactorySic.GetWbCmvstarifaRepository().Save(entity);
            }
        }

        /// <summary>
        /// Muestra los datos del compartivo de CM vs Tarifa en Barra
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<ChartCmVsTarifaBarra> ObtenerCmVsTarifaBarra(string fechaConsulta)
        {
            List<ChartCmVsTarifaBarra> entitys = new List<ChartCmVsTarifaBarra>();
            DateTime fecha = DateTime.ParseExact(fechaConsulta, Aplicacion.Helper.ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
            List<WbCmvstarifaDTO> list = (new PortalAppServicio()).ListarCmVsTarifa().Where(x => (DateTime)x.Cmtarfecha <= fecha).
                OrderBy(x => (DateTime)x.Cmtarfecha).ToList();
            int len = list.Count - 36;
            List<WbCmvstarifaDTO> filtrado = list.Skip(len).ToList();

            foreach (WbCmvstarifaDTO item in filtrado)
            {
                entitys.Add(new ChartCmVsTarifaBarra {
                    FechaMes = ((DateTime)item.Cmtarfecha).ToString("MMM yyyy"),
                    CostoMarginalProm = (decimal)item.Cmtarcmprom,
                    TarifaEnergia = (decimal)item.Cmtartarifabarra,
                    PromedioMovilAnual = (decimal)item.Cmtarprommovil
                });
            }

            return entitys;
        }

        /// <summary>
        /// Retorna el listado de ayudas del app movil
        /// </summary>
        /// <returns></returns>
        public List<WbAyudaappDTO> ObtenerListaAyudaApp()
        {
            return FactorySic.GetWbAyudaappRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener los datos de la ventana
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WbAyudaappDTO ObtenerAyudaVentanaApp(int id)
        {
            return FactorySic.GetWbAyudaappRepository().GetById(id);
        }

        /// <summary>
        /// Permite grabar los datos de la ayuda
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarAyudaVentana(WbAyudaappDTO entity)
        {
            try
            {
                FactorySic.GetWbAyudaappRepository().Update(entity);
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        #endregion

        #region Reporte Power BI
        /// <summary>
        /// Datos para la pantalla de Reportes de Power BI
        /// </summary>
        /// <returns></returns>        
        public async Task<List<PowerBIReportDTO>> ObtenerReportes()
        {
            var clientId = ConfigurationManager.AppSettings["PowerBI:ClientId"];
            var clientSecret = ConfigurationManager.AppSettings["PowerBI:ClientSecret"];
            var tenantId = ConfigurationManager.AppSettings["PowerBI:TenantId"];
            var workspaceId = ConfigurationManager.AppSettings["PowerBI:WorkspaceId"];

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
            var body = $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}&scope=https://analysis.windows.net/powerbi/api/.default";
            var content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            using (var httpClient = new HttpClient())
            {
                var tokenResponse = await httpClient.PostAsync(tokenEndpoint, content);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

                if (!tokenResponse.IsSuccessStatusCode)
                    throw new Exception("Error al obtener el token: " + tokenContent);

                dynamic tokenData = JsonConvert.DeserializeObject(tokenContent);
                string accessToken = tokenData.access_token;

                var reportsUrl = $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId}/reports";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var reportsResponse = await httpClient.GetAsync(reportsUrl);
                var reportsContent = await reportsResponse.Content.ReadAsStringAsync();

                if (!reportsResponse.IsSuccessStatusCode)
                    throw new Exception("Error al obtener los reportes: " + reportsContent);

                dynamic reportsData = JsonConvert.DeserializeObject(reportsContent);
                var reportDtos = new List<PowerBIReportDTO>();

                // Nueva asignación de TabId según orden específico
                var tabMap = new Dictionary<string, string>
        {
            { "93bacba8-215b-456d-af98-20f45049e8cc", "tab-5" },   // Energía
            { "c6822b2d-dfcf-4da2-9e99-a941fb4fa4da", "tab-6" },   // Demanda
            { "c22a9525-f4a0-49c4-9cda-9ddc5e41a74d", "tab-7" },   // Potencia Efectiva            
            //{ "14bd5f18-df1a-42dc-9002-0a8482d732b1", "tab-7" },   // Potencia Efectiva
            { "679ece76-b27b-4cf6-ace0-652aa489544b", "tab-8" },   // Combustibles
            { "fa34e303-a979-4b6a-b79e-3b63f6c4aa45", "tab-9" },   // Hidrología
            { "94a6cb1c-fea0-4817-98f2-47136492ccf6", "tab-10" },  // Costos Marginales
            { "cde662a3-ab46-48a5-87ae-1c316efb1109", "tab-11" },  // Usuarios Libres
            { "534d1970-2df1-4157-be90-80d1672dcf61", "tab-12" },  // Estadísticas de Fallas
            { "2766d7c8-a6fc-4759-8574-4c7fe90ce821", "tab-1" },   // Generación
            { "69838d89-986a-43cd-bcbd-3d4124272900", "tab-2" },   // Producción Energía
            { "e4b71257-ba05-443b-a391-24fd091ffa37", "tab-3" },   // Ubicación Centrales
            { "bac47641-44a4-46b4-b968-5be36899145d", "tab-4" }    // Parque Generación SEIN
        };

                foreach (var report in reportsData.value)
                {
                    string reportId = report.id;
                    string embedUrl = report.embedUrl;
                    string name = report.name;
                    string datasetId = report.datasetId;

                    if (!tabMap.ContainsKey((string)reportId))
                        continue;

                    try
                    {
                        string embedToken;

                        if (name.Contains("DEMANDA") || name.Contains("ENERGIA"))
                        {
                            var identities = new[]
                            {
                        new EmbedIdentity
                        {
                            Username = "biapp01@coes.org.pe",
                            Roles = new string[] { },
                            Datasets = new[] { datasetId }
                        }
                    };

                            embedToken = await ObtenerEmbedToken(accessToken, workspaceId, reportId, identities);
                        }
                        else
                        {
                            embedToken = await ObtenerEmbedToken(accessToken, workspaceId, reportId, null);
                        }

                        reportDtos.Add(new PowerBIReportDTO
                        {
                            Id = reportId,
                            Name = name,
                            EmbedUrl = embedUrl,
                            TokenAccess = embedToken,
                            TabId = tabMap[reportId]
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error en el reporte: {name} ({reportId}) - {ex.Message}");
                    }
                }

                return reportDtos;
            }
        }


        /// <summary>
        /// Datos para la pantalla de Reportes de Power BI
        /// </summary>
        /// <returns></returns>        
        public async Task<List<PowerBIReportIntranetDTO>> ObtenerReportesParaIntranet()
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            List<PowerBIReportIntranetDTO> result = new List<PowerBIReportIntranetDTO>();
            string client_id = ConfigurationManager.AppSettings["powerbi:client_id"].ToString();
            string client_secret = ConfigurationManager.AppSettings["powerbi:client_secret"].ToString();
            string pbi_user = ConfigurationManager.AppSettings["powerbi:pbi_user"].ToString();
            string pbi_password = ConfigurationManager.AppSettings["powerbi:pbi_password"].ToString();

            var formUrlEncoded = new FormUrlEncodedContent(new[]
             {
                new KeyValuePair<string, string>("resource", "https://analysis.windows.net/powerbi/api"),
                new KeyValuePair<string, string>("client_id", client_id),
                new KeyValuePair<string, string>("client_secret", client_secret),
                new KeyValuePair<string, string>("grant_type",  "password"),
                new KeyValuePair<string, string>("username", pbi_user),
                new KeyValuePair<string, string>("password", pbi_password),
                new KeyValuePair<string, string>("scope", "openid")
            });

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync("https://login.windows.net/common/oauth2/token/", formUrlEncoded);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    OAuthResultDTO oauth = JsonConvert.DeserializeObject<OAuthResultDTO>(json);
                    using (var powerBIClient = new PowerBIClient(new Uri("https://api.powerbi.com/"), new TokenCredentials(oauth.AccessToken, "Bearer")))
                    {
                        List<Report> reports = powerBIClient.Reports.GetReports().Value.ToList();
                        PowerBIReportIntranetDTO reportResult;
                        foreach (Report report in reports)
                        {
                            reportResult = new PowerBIReportIntranetDTO();
                            reportResult.Id = report.Id;
                            reportResult.Name = report.Name;
                            reportResult.WebUrl = report.WebUrl;
                            reportResult.EmbedUrl = report.EmbedUrl;
                            reportResult.DatasetId = report.DatasetId;
                            reportResult.TokenAccess = oauth.AccessToken;
                            result.Add(reportResult);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<PowerBIReportDTO> ObtenerReportePorTabId(string tabId)
        {
            var clientId = ConfigurationManager.AppSettings["PowerBI:ClientId"];
            var clientSecret = ConfigurationManager.AppSettings["PowerBI:ClientSecret"];
            var tenantId = ConfigurationManager.AppSettings["PowerBI:TenantId"];
            var workspaceId = ConfigurationManager.AppSettings["PowerBI:WorkspaceId"];

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
            var body = $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}&scope=https://analysis.windows.net/powerbi/api/.default";
            var content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            using (var httpClient = new HttpClient())
            {
                var tokenResponse = await httpClient.PostAsync(tokenEndpoint, content);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

                if (!tokenResponse.IsSuccessStatusCode)
                    throw new Exception("Error al obtener el token: " + tokenContent);

                dynamic tokenData = JsonConvert.DeserializeObject(tokenContent);
                string accessToken = tokenData.access_token;

                var reportsUrl = $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId}/reports";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var reportsResponse = await httpClient.GetAsync(reportsUrl);
                var reportsContent = await reportsResponse.Content.ReadAsStringAsync();

                if (!reportsResponse.IsSuccessStatusCode)
                    throw new Exception("Error al obtener los reportes: " + reportsContent);

                dynamic reportsData = JsonConvert.DeserializeObject(reportsContent);

                var tabMap = new Dictionary<string, string>
                {
                    { "93bacba8-215b-456d-af98-20f45049e8cc", "tab-5" },
                    { "c6822b2d-dfcf-4da2-9e99-a941fb4fa4da", "tab-6" },
                    { "c22a9525-f4a0-49c4-9cda-9ddc5e41a74d", "tab-7" },
                    { "679ece76-b27b-4cf6-ace0-652aa489544b", "tab-8" },
                    { "fa34e303-a979-4b6a-b79e-3b63f6c4aa45", "tab-9" },
                    { "94a6cb1c-fea0-4817-98f2-47136492ccf6", "tab-10" },
                    { "cde662a3-ab46-48a5-87ae-1c316efb1109", "tab-11" },
                    { "534d1970-2df1-4157-be90-80d1672dcf61", "tab-12" },
                    { "2766d7c8-a6fc-4759-8574-4c7fe90ce821", "tab-1" },
                    { "69838d89-986a-43cd-bcbd-3d4124272900", "tab-2" },
                    { "e4b71257-ba05-443b-a391-24fd091ffa37", "tab-3" },
                    { "bac47641-44a4-46b4-b968-5be36899145d", "tab-4" }
                };

                var reportEntry = tabMap.FirstOrDefault(x => x.Value == tabId);
                if (string.IsNullOrEmpty(reportEntry.Key))
                {
                    throw new Exception($"No se encontró un ReportId asociado al tab '{tabId}'.");
                }

                string targetReportId = reportEntry.Key;

                foreach (var report in reportsData.value)
                {
                    if ((string)report.id != targetReportId) continue;

                    string reportId = report.id;
                    string embedUrl = report.embedUrl;
                    string name = report.name;
                    string datasetId = report.datasetId;

                    string embedToken;

                    if (name.Contains("01_") || name.Contains("02_"))
                    {
                        var identities = new[]
                        {
                            new EmbedIdentity
                            {
                                Username = "biapp01@coes.org.pe",
                                Roles = new string[] { },
                                Datasets = new[] { datasetId }
                            }
                        };

                        embedToken = await ObtenerEmbedToken(accessToken, workspaceId, reportId, identities);
                    }
                    else
                    {
                        embedToken = await ObtenerEmbedToken(accessToken, workspaceId, reportId, null);
                    }

                    return new PowerBIReportDTO
                    {
                        Id = reportId,
                        Name = name,
                        EmbedUrl = embedUrl,
                        TokenAccess = embedToken,
                        TabId = tabId
                    };
                }

                throw new Exception($"El reporte con ID '{targetReportId}' no fue encontrado en Power BI.");
            }
        }

        public async Task<string> ObtenerEmbedToken(string accessToken, string workspaceId, string reportId, EmbedIdentity[] identities)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var url = $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId}/reports/{reportId}/GenerateToken";

                var requestBody = new
                {
                    accessLevel = "view",
                    identities = identities
                };

                var json = JsonConvert.SerializeObject(requestBody, Formatting.None,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al generar token de embed: {result}");
                }

                var tokenObj = JsonConvert.DeserializeObject<JObject>(result);
                return tokenObj["token"]?.ToString();
            }
        }

        #endregion
    }




    public class DataHidrologia
    {
        public string Fecha { get; set; }
        public int Punto { get; set; }
        public List<decimal> Valores { get; set; }
    }



    /// <summary>
    /// Clase para manejo de graficos
    /// </summary>
    public class ChartGeneracion
    {
        private List<string> categorias = new List<string>();
        private List<Series> series = new List<Series>();
        private List<SeriesXY> seriesAdicional = new List<SeriesXY>();

        public List<SeriesXY> SeriesAdicional
        {
            get { return seriesAdicional; }
            set { seriesAdicional = value; }
        }

        public List<string> Categorias
        {
            get { return categorias; }
            set { categorias = value; }
        }

        public List<Series> Series
        {
            get { return series; }
            set { series = value; }
        }
    }

    /// <summary>
    /// Estructura para el grafico de generacion
    /// </summary>
    public class GraficoGeneracion
    {
        public List<PuntoSerie> GeneracionPorEmpresa { get; set; }
        public string LastDate { get; set; }
        public decimal LastValue { get; set; }
        public List<PuntoSerie> GeneracionPorTipoCombustible { get; set; }
        public List<PuntoSerie> GeneracionPorTipoCombustiblePie { get; set; }
        public List<PuntoSerie> GeneracionPorTipoGeneracion { get; set; }
        public decimal PorcentajeCrecimiento { get; set; }
    }



    /// <summary>
    /// Estructura para maxima demanda
    /// </summary>
    public class GraficoMaximaDemanda
    {
        public List<PuntoSerie> GraficoFuenteEnergia { get; set; }
        public List<PuntoSerie> GraficoTipoGeneracion { get; set; }
        public List<PuntoSerie> GraficoDiagramaCarga { get; set; }
        public List<PuntoSerie> FiltroTipoGeneracion { get; set; }
        public List<PuntoSerie> FiltroEmpresa { get; set; }
        public decimal ValorExpMD { get; set; }
        public decimal ValorImpMD { get; set; }
        public decimal ValorMD { get; set; }
        public string FechaMD { get; set; }
        public string HoraMD { get; set; }
        public List<string> EjexDiagramaCarga { get; set; }
    }

    /// <summary>
    /// Clase para manejo de series
    /// </summary>
    public class Series
    {
        public string Name { get; set; }
        private List<decimal> data = new List<decimal>();

        public List<decimal> Data
        {
            get { return data; }
            set { data = value; }
        }
    }

    /// <summary>
    /// Clase para pintado de chart
    /// </summary>
    public class ChartStock
    {
        private List<SeriesXY> series = new List<SeriesXY>();

        public List<SeriesXY> Series
        {
            get { return series; }
            set { series = value; }
        }
    }

    /// <summary>
    /// Series XY
    /// </summary>
    public class SeriesXY
    {
        public string Name { get; set; }
        private List<PuntoSerie> data = new List<PuntoSerie>();

        public List<PuntoSerie> Data
        {
            get { return data; }
            set { data = value; }
        }
    }

    /// <summary>
    /// Punto de una serie
    /// </summary>
    public class PuntoSerie
    {
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public string CodColor { get; set; }
        public List<decimal> Valores { get; set; }
        public List<PuntoSerie> Valores_ { get; set; }
        public int Codigo { get; set; }
        public string RER { get; set; }
    }

    /// <summary>
    /// Datos para la demanda
    /// </summary>
    public class ChartDemanda
    {
        public ChartStock Chart { get; set; }
        public List<ChartDemandaData> Data { get; set; }
        public string LastDate { get; set; }
        public string LastTime { get; set; }
        public decimal LastValue { get; set; }
        public List<string> EjexDemanda { get; set; }
    }

    /// <summary>
    /// Estructura para mostrar el gráfico de CM VS Tarifa barra
    /// </summary>
    public class ChartCmVsTarifaBarra
    {
        public string FechaMes { get; set; }
        public decimal CostoMarginalProm { get; set; }
        public decimal TarifaEnergia { get; set; }
        public decimal PromedioMovilAnual { get; set; }
    }

    /// <summary>
    /// Datos para el gráfico de producción
    /// </summary>
    public class ChartProduccion
    {
        public List<PuntoSerie> Data { get; set; }
        public string LastDate { get; set; }
        public decimal LastValue { get; set; }
    }

    /// <summary>
    /// Objeto para manejo de grafico de demanda
    /// </summary>
    public class ChartDemandaData
    {
        public string Fecha { get; set; }
        public decimal? ValorEjecutado { get; set; }
        public decimal? ValorProgramacionDiaria { get; set; }
        public decimal? ValorProgramacionSemanal { get; set; }
    }

    /// <summary>
    /// Permite grabar el gráfico de frecuencia
    /// </summary>
    public class GraficoFrecuencia
    {
        public string Fecha { get; set; }
        public decimal Valor { get; set; }
    }

    /// <summary>
    /// Clase para manejo de datos del app pantalla inicial
    /// </summary>
    public class DatosResumenMovil
    {
        public decimal CostoMarginal { get; set; }
        public decimal Frecuencia { get; set; }
        public decimal DemandaPico { get; set; }
        public decimal MaximaDemanda { get; set; }
        public string TextoCostoMarginal { get; set; }
        public string TextoCostoMarginalHome { get; set; }
    }

}
