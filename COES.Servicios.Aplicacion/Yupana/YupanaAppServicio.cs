using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Dominio.DTO.Sic;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.Servicios.Aplicacion.Yupana.Helper;
using System.IO;
using COES.Framework.Base.Core;
using System.Text.RegularExpressions;
using GAMS;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using System.Configuration;

namespace COES.Servicios.Aplicacion.Yupana
{
    public class YupanaAppServicio : AppServicioBase
    {

        private List<CpPropiedadDTO> _listaPropiedades;
        private List<CpProprecursoDTO> _propiedades;
        private List<CpRecursoDTO> _lrecurso;
        private CpTopologiaDTO _escenario;
        private List<CpDetalleetapaDTO> _lperiodo;
        private List<CpMedicion48DTO> _lsrestriccion48;
        private List<CpSubrestricdatDTO> _lsubrestricdat;

        #region CpRecurso

        /// <summary>
        /// Lista de recurso por topologia -- Movisoft 2022-04-26
        /// </summary>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public List<CpRecursoDTO> ListarRecursoPorTopologia(int topcodi)
        {
            return FactorySic.GetCpRecursoRepository().ListarRecursoPorTopologia(topcodi);
        }

        #endregion

        #region PropRecurso

        public string GetValorPropiedad(int recurcodi, int propcodi, DateTime fecha, int topcodi)
        {
            CpProprecursoDTO registro = new CpProprecursoDTO();
            string valor = string.Empty;
            registro = Factory.FactorySic.GetCpProprecursoRepository().GetById(recurcodi, propcodi, fecha, topcodi);
            if (registro != null)
            {
                if (registro.Valor != null)
                    valor = registro.Valor;
            }
            return valor;
        }

        #endregion

        #region SubrestriccionDat

        public List<CpSubrestricdatDTO> ListarDatosRestriccion(int topcodi, int restriccodi)
        {
            return FactorySic.GetCpSubrestricdatRepository().ListarDatosRestriccion(topcodi, restriccodi);
        }

        #endregion

        #region CPmedicion48

        /// <summary>
        /// Devuelve lista de restricciones cada media hora
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="subrestricodi"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> ListaRestriciones(int topcodi, short subrestricodi)
        {
            List<CpMedicion48DTO> lista = new List<CpMedicion48DTO>();
            lista = FactorySic.GetCpMedicion48Repository().ListaRestricion(topcodi, subrestricodi);

            return lista;
        }

        /// <summary>
        /// Genera Reporte de Resultado en forma horizontal para visualuizacion en Web
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="subrestricodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GenerarReporteResultadoHorizontal(int topcodi, short subrestricodi, DateTime fecha)
        {
            var yupi = (new Aplicacion.CortoPlazo.McpAppServicio()).GetByIdCpFuentesgams(10);
            List<CpMedicion48DTO> lista = new List<CpMedicion48DTO>();
            lista = ListaRestriciones(topcodi, subrestricodi);
            var listaRest48 = lista.Where(x => x.Medifecha == fecha).ToList();
            var listaCabecera = lista.GroupBy(x => new { x.Recurcodi, x.Recurnombre }).
                Select(y => new CpMedicion48DTO()
                {
                    Recurcodi = y.Key.Recurcodi,
                    Recurnombre = y.Key.Recurnombre
                }
                ).ToList();
            //Cabecera
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='0' class='pretty tabla-adicional' cellspacing='0' width='100%' id='tabla'>");
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            GeneraHtmlCabecera(listaCabecera, ref strHtml);
            strHtml.Append("<tbody>");

            if (listaRest48.Count > 0)
            {
                for (int k = 1; k <= 48; k++)
                {
                    var fechaMin = ((fecha.AddMinutes((k - 1) * 30))).ToString(ConstantesBase.FormatoFechaHora);
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td><span>{0}</span></td>", fechaMin));
                    foreach (var p in listaCabecera)
                    {
                        var reg = listaRest48.Find(x => x.Recurcodi == p.Recurcodi && x.Medifecha == fecha);

                        if (reg != null)
                        {
                            decimal? valor;
                            valor = (decimal?)reg.GetType().GetProperty("H" + k).GetValue(reg, null);
                            if (valor != null)
                                strHtml.Append(string.Format("<td style='text-align:right' >{0}</td>", ((decimal)valor).ToString("N", nfi)));
                            else
                                strHtml.Append(string.Format("<td>--</td>"));
                        }
                        else
                            strHtml.Append("<td>--</td>");
                    }

                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        ///  Genera Reporte de Resultado en forma Vertical para visualuizacion en Web
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="subrestricodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GenerarReporteResultadoTransversal(int topcodi, short subrestricodi, DateTime fecha)
        {

            List<CpMedicion48DTO> lista = new List<CpMedicion48DTO>();
            lista = ListaRestriciones(topcodi, subrestricodi);
            var listaRest48 = lista.Where(x => x.Medifecha == fecha).ToList();
            var listaCabecera = lista.GroupBy(x => new { x.Recurcodi, x.Recurnombre }).
                Select(y => new CpMedicion48DTO()
                {
                    Recurcodi = y.Key.Recurcodi,
                    Recurnombre = y.Key.Recurnombre
                }
                ).ToList();
            //Cabecera
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table border='0' class='pretty tabla-adicional' cellspacing='0' width='100%' id='tabla'>");
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            GeneraHtmlCabeceraFecha(ref strHtml, fecha);
            strHtml.Append("<tbody>");

            if (listaRest48.Count > 0)
            {
                foreach (var p in listaCabecera)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", p.Recurnombre));
                    for (int k = 1; k <= 48; k++)
                    {
                        var reg = listaRest48.Find(x => x.Recurcodi == p.Recurcodi && x.Medifecha == fecha);

                        if (reg != null)
                        {
                            decimal? valor;
                            valor = (decimal?)reg.GetType().GetProperty("H" + k).GetValue(reg, null);
                            if (valor != null)
                                strHtml.Append(string.Format("<td style='text-align:right' >{0}</td>", ((decimal)valor).ToString("N", nfi)));
                            else
                                strHtml.Append(string.Format("<td>--</td>"));
                        }
                        else
                            strHtml.Append("<td>--</td>");
                    }

                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// Genera Reporte de Tabla de Costo
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GenerarReporteTablaCosto(int topcodi)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            string stValor = string.Empty;
            var lista = ListarDatosRestriccion(topcodi, ConstantesYupana.RestricCostosOperacion);
            var find = lista.Find(x => x.Srestcodi == ConstantesYupana.CostoArranque);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<H3>Costos Generales $ </H3>");
            strHtml.Append("<table border='1' cellspacing='0' id='tabla' style='width:420px;'");
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;width:300px;'><span>{0}</span></td>", "Costo Aranques Térmico"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            //////// Costo de Operación Térmico
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.CostoTermico);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo de Operación Térmico"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            ////// Costo de Operación Hidros
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.CostoHidro);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo de Operación Hidros"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            //////// Costo Función Costo Futuro
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.CostoFuturo);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo Función Costo Futuro"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            /////// Costo Vertimiento de Centrales
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.CostoVertimientoCentrales);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo Vertimiento de Centrales"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            ///// Costo Vertimiento de Embalses
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.CostoVertimientoEmbalse);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo Vertimiento de Embalses"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");

            ///// Costo Racionamientos (Déficit Demanda)
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.CostoRacionamiento);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo Racionamientos (Déficit Demanda)"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            ///// Costo Exceso de Potencia
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.CostoExcesoPotencia);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo de Excesos de Potencia"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            ///// Costo de Reserva Secundaria(Up)
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.SresCostoReservaUrsUp);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo de Reserva Secundaria(Up)"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            ///// Costo de Reserva Secundaria(Down)
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.SresCostoReservaUrsDown);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo de Reserva Secundaria(Down)"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            ///// Costo de Déficit de Reserva Secundaria (Up)
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.SresCostoDeficitReservaUrsUp);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo de Déficit de Reserva Secundaria (Up)"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            ///// Costo de Déficit de Reserva Secundaria (Down)
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.SresCostoDeficitReservaUrsDown);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo de Déficit de Reserva Secundaria (Down)"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            ///// Costo de Déficit por Región de Seguridad
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.SresCostoDeficitRegSeguridad);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo de Déficit por Región de Seguridad"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            ///// Costo de Exceso por Región de Seguridad
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.SresCostoExcesoReservaUrsDown);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Costo de Exceso por Región de Seguridad"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");

            ///// Costo Total
            find = lista.Find(x => x.Srestcodi == ConstantesYupana.CostoTotal);
            stValor = (find != null) ? ((find.Srestdvalor1 != null) ? ((decimal)find.Srestdvalor1).ToString("N", nfi) : string.Empty) : string.Empty;
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;'><span>{0}</span></td>", "Total:"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            strHtml.Append("</table>");
            strHtml.Append("<br>");
            stValor = GetValorPropiedad(0, ConstantesYupana.TipoCambioDolar, DateTime.MaxValue, topcodi);
            decimal tipoCamb;
            //decimal.TryParse(valor, out tipoCamb);
            strHtml.Append("<H3>Otros Datos </H3>");
            strHtml.Append("<table border='1' cellspacing='0' id='tabla' style='width:350px;'");
            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<td style='background-color:#2980B9;color:white;width:220px;'><span>{0}</span></td>", "Tipo de Cambio:"));
            strHtml.Append(string.Format("<td style='text-align:right;'><span>{0}</span></td>", stValor));
            strHtml.Append("</tr>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera la cabecera de los reportes de resultado horizontal
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="strHtml"></param>
        private void GeneraHtmlCabecera(List<CpMedicion48DTO> listaCabecera, ref StringBuilder strHtml)
        {
            strHtml.Append("<thead>");
            strHtml.Append("<tr><th>Fecha</th>");
            //strHtml.Append("<th>Total</th>");
            foreach (var reg in listaCabecera)
            {
                strHtml.Append("<th >" + reg.Recurnombre + "</th >");
            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
        }

        /// <summary>
        /// Genera la cabecera de los reportes de resulatdo vertical
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabecera"></param>
        /// <param name="strHtml"></param>
        private void GeneraHtmlCabeceraFecha(ref StringBuilder strHtml, DateTime fecha)
        {
            strHtml.Append("<thead>");
            strHtml.Append("<tr><th>Equipo</th>");
            //strHtml.Append("<th>Total</th>");
            for (int k = 1; k <= 48; k++)
            {
                strHtml.Append(string.Format("<th>{0}</th>", fecha.AddMinutes(k * 30).ToString("HH:mm")));
            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
        }


        #endregion

        #region CpTopologia
        /// <summary>
        /// Obtiene registro de topología por id
        /// </summary>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public CpTopologiaDTO GetTopologia(int topcodi)
        {
            return Factory.FactorySic.GetCpTopologiaRepository().GetById(topcodi);
        }

        /// <summary>
        /// Obtiene lista de registros de topología por rango de fecha y tipo de topología
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        public List<CpTopologiaDTO> ListaTopologia(DateTime fechaInicio, DateTime fechaFin, short idTipo)
        {
            return Factory.FactorySic.GetCpTopologiaRepository().GetByCriteria(fechaInicio, fechaFin, idTipo);
        }

        /// <summary>
        /// Obtiene lista de registros de topología por nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<CpTopologiaDTO> ListaTopologiaXNombre(string nombre)
        {
            return Factory.FactorySic.GetCpTopologiaRepository().ListNombre(nombre);
        }

        #endregion

        #region CpPropiedadDTO

        public List<CpPropiedadDTO> ObtenerListaPropiedadesTotal()
        {
            List<CpPropiedadDTO> entitys = new List<CpPropiedadDTO>();
            entitys = FactorySic.GetCpPropiedadRepository().List();
            return entitys;
        }

        #endregion

        #region Yupana Continuo

        #region Archivos Csv
        /// <summary>
        /// Crear archivos csv del escenario
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        public void CrearArchivosCsv(string ruta, int pTopologia, InsumoYupanaContinuo objInsumo)
        {
            var listaPropiedades = ObtenerListaPropiedadesTotal();
            var lista_Recurcodi_OnLine_OffLine = new List<Tuple<int, int>>();
            //Crear Escenario.csv
            var escenario = GetTopologia(pTopologia);

            CrearArchivoEscenarioCsv(ruta, escenario);
            // Crear Periodo.csv
            CrearPeriodoCsv(ruta, pTopologia);
            //Crear Categoria
            CrearCategoriaCsv(ruta);
            // Crear Parametros
            CrearParametroCsv(ruta, pTopologia);
            // Crear csv de propiedades
            CrearPropiedadesCsv(ruta, listaPropiedades);
            // Crear csv de Fuente Gams
            CrearFuenteGamsCsv(ruta);
            // Crear Subresticciones
            CrearSusrestriccionesCsv(ruta);
            var propiedades = FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecurso2(-1, "-1", pTopologia, -1).ToList();
            //Crear CSV NodoTopologico
            List<CpPropiedadDTO> lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.NodoTopologico).OrderBy(x => x.Proporden).ToList();
            CrearNodoTopologicoCsv(ruta, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            // Planta Hidros
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.PlantaH).OrderBy(x => x.Proporden).ToList();
            CrearPlantaHCsv(ruta, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            // crear csv Embalse
            CrearRecursoCsv(ruta, ConstantesBase.Embalse, ConstantesYupana.NombArchivoEmbalse, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crear CSV Planta Rer
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.PlantaNoConvenO).OrderBy(x => x.Proporden).ToList();
            CrearPlantaRerCsv(ruta, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);

            //CREAR CSV PLANTA T
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.PlantaT).OrderBy(x => x.Proporden).ToList();
            CrearRecurso2Csv(ruta, ConstantesBase.PlantaT, ConstantesYupana.NombArchivoPlantaT, ConstantesBase.Tergtermico, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //CREAR CSV UNIDAD T
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.UnidadH).OrderBy(x => x.Proporden).ToList();
            CrearUnidadTCsv(ruta, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crear Combustibles
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.Combustible).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.Combustible, ConstantesYupana.NombArchivoCombustible, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //CREAR CSV MODO T 
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.ModoT).OrderBy(x => x.Proporden).ToList();
            CrearModoTCsv(ruta, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crear CSV Lineas  
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.Linea).OrderBy(x => x.Proporden).ToList();
            CrearTransmisionCsv(ruta, ConstantesBase.Linea, pTopologia, ConstantesYupana.NombArchivoLinea, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            // Crear CSV Trafos 2D
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.Trafo2D).OrderBy(x => x.Proporden).ToList();
            CrearTransmisionCsv(ruta, ConstantesBase.Trafo2D, pTopologia, ConstantesYupana.NombArchivoTrafo2D, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            // Crear CSV Trafos 3D
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.Trafo3D).OrderBy(x => x.Proporden).ToList();
            CrearTransmisionCsv(ruta, ConstantesBase.Trafo3D, pTopologia, ConstantesYupana.NombArchivoTrafo3D, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            // Crear CSV URS 

            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.Urs).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.Urs, ConstantesYupana.NombArchivoURS, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crear Grupo Prioridad
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.GrupoPrioridad).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.GrupoPrioridad, ConstantesYupana.NombArchivoGrupoPrioridad, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crear CicloCombinado
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.CicloCombinado).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.CicloCombinado, ConstantesYupana.NombArchivoCicloCombinado, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crear Disponibilidad Combustible
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.DispComb).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.DispComb, ConstantesYupana.NombArchivoDisponComb, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crea Suma de Flujos
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.SumFlujo).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.SumFlujo, ConstantesYupana.NombArchivoSumaFlujosCsv, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crea RSF
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.Rsf).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.Rsf, ConstantesYupana.NombArchivoRsfCsv, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crea Restricciones de Generacion
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.RestricGener).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.RestricGener, ConstantesYupana.NombArchivoResGenerCsv, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crea Generacion Meta
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.GenerMeta).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.GenerMeta, ConstantesYupana.NombArchivoGenerMetaCsv, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crea Regiones de Seguridad
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.RegionSeguridad).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.RegionSeguridad, ConstantesYupana.NombreArchivoRegionSeguridadCsv, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            //Crear Calderos
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.Caldero).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.Caldero, ConstantesYupana.NombArchivoCalderoCsv, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
            /// Crear Relaciones
            CrearRelacionesCsv(ruta, pTopologia, lista_Recurcodi_OnLine_OffLine);
            //// Crear CSV GrupoRecurso
            CrearGrupoRecursoCsv(ruta, pTopologia, lista_Recurcodi_OnLine_OffLine);
            // Crear Restricciones para subrestrictdat
            CrearRestriccionesDatCsv(ruta, pTopologia, lista_Recurcodi_OnLine_OffLine);
            //Crear Restricciones para Medicion48
            CrearRestricciones48Csv(ruta, pTopologia, escenario.Topfecha, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48Rer, pTopologia, escenario.Topfecha, null, objInsumo.ListaProyRer, null, 1, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48Forzadas, pTopologia, escenario.Topfecha, objInsumo.ListaCondTermicas, null, null, 2, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48Cc, pTopologia, escenario.Topfecha, null, null, objInsumo.ListaAportesCC, 0, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48Sc, pTopologia, escenario.Topfecha, null, null, objInsumo.ListaAportesSC, 0, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48CcSc, pTopologia, escenario.Topfecha, null, null, objInsumo.ListaAportesCCSC, 0, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48RerSc, pTopologia, escenario.Topfecha, null, objInsumo.ListaProyRer, objInsumo.ListaAportesSC, 3, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48RerCc, pTopologia, escenario.Topfecha, null, objInsumo.ListaProyRer, objInsumo.ListaAportesCC, 3, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48RerCcSc, pTopologia, escenario.Topfecha, null, objInsumo.ListaProyRer, objInsumo.ListaAportesCCSC, 3, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48RerForzadas, pTopologia, escenario.Topfecha, objInsumo.ListaCondTermicas, objInsumo.ListaProyRer, null, 5, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48CcForzadas, pTopologia, escenario.Topfecha, objInsumo.ListaCondTermicas, null, objInsumo.ListaAportesCC, 4, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48ScForzadas, pTopologia, escenario.Topfecha, objInsumo.ListaCondTermicas, null, objInsumo.ListaAportesSC, 4, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48RerCcForzadas, pTopologia, escenario.Topfecha, objInsumo.ListaCondTermicas, objInsumo.ListaProyRer, objInsumo.ListaAportesCC, 6, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48RerScForzadas, pTopologia, escenario.Topfecha, objInsumo.ListaCondTermicas, objInsumo.ListaProyRer, objInsumo.ListaAportesSC, 6, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48ForzadasCcSc, pTopologia, escenario.Topfecha, objInsumo.ListaCondTermicas, null, objInsumo.ListaAportesCCSC, 4, lista_Recurcodi_OnLine_OffLine);
            CrearRestricciones48CsvYupanaContinuo(ruta, ConstantesYupana.NombArchivoMedicion48ForzadasRerCcSc, pTopologia, escenario.Topfecha, objInsumo.ListaCondTermicas, objInsumo.ListaProyRer, objInsumo.ListaAportesCCSC, 6, lista_Recurcodi_OnLine_OffLine);

            //Crear Equipos conectados a NodoTopologicos
            CrearEquipoConecNodoTopologico(ruta, pTopologia, lista_Recurcodi_OnLine_OffLine);
            //Crear Costo Futuro 
            CrearCostoFuturo(ruta, pTopologia, lista_Recurcodi_OnLine_OffLine);
            // propiedades Generales
            //Crear CSV Generales
            lpropiedades = listaPropiedades.Where(x => x.Catcodi == ConstantesBase.Generales).OrderBy(x => x.Proporden).ToList();
            CrearRecursoCsv(ruta, ConstantesBase.Generales, ConstantesYupana.NombArchivoGeneralesCsv, pTopologia, lpropiedades, propiedades, ref lista_Recurcodi_OnLine_OffLine);
        }

        /// <summary>
        /// Crea fila cabecera de archivo csv recurso
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="lpropiedades"></param>
        /// <returns></returns>
        public string CrearCabeceraEquipo(string ruta, List<CpPropiedadDTO> lpropiedades)
        {
            string sLine = string.Empty;
            sLine += "Id Equipo" + ConstantesYupana.SeparadorCampo;
            sLine += "Equipo" + ConstantesYupana.SeparadorCampo;
            sLine += "Id COES" + ConstantesYupana.SeparadorCampo;
            sLine += "Considera Equipo" + ConstantesYupana.SeparadorCampo;
            sLine += "Id Cat Vierte" + ConstantesYupana.SeparadorCampo;
            sLine += "Id Vierte" + ConstantesYupana.SeparadorCampo;
            sLine += "Id Cat Turbina" + ConstantesYupana.SeparadorCampo;
            sLine += "Id Turbina" + ConstantesYupana.SeparadorCampo;
            sLine += "Id Padre" + ConstantesYupana.SeparadorCampo;
            sLine += "Id Nod Destino" + ConstantesYupana.SeparadorCampo;
            sLine += "Id Nod Origen" + ConstantesYupana.SeparadorCampo;
            sLine += "Id Nodo" + ConstantesYupana.SeparadorCampo;
            foreach (var reg in lpropiedades)
            {
                sLine += reg.Propnombre + ConstantesYupana.SeparadorCampo;
            }
            sLine += "Estado";
            return sLine;
        }

        /// <summary>
        /// Crea fila contenido para archuivo csv recurso
        /// </summary>
        /// <param name="recurso"></param>
        /// <param name="propiedades"></param>
        /// <param name="totalPropiedades"></param>
        /// <param name="recurcodi"></param>
        /// <param name="recurcodioffline"></param>
        /// <returns></returns>
        public string CreateFilaString(CpRecursoDTO recurso, List<CpProprecursoDTO> propiedades, int totalPropiedades, int recurcodi, int recurcodioffline)
        {
            string sLine = string.Empty;
            sLine += recurcodi.ToString() + ConstantesYupana.SeparadorCampo;
            sLine += recurso.Recurnombre + ConstantesYupana.SeparadorCampo;
            sLine += recurcodioffline.ToString() + ConstantesYupana.SeparadorCampo;
            sLine += recurso.Recurconsideragams.ToString() + ConstantesYupana.SeparadorCampo;
            sLine += recurso.CatcodiVierte.ToString() + ConstantesYupana.SeparadorCampo;
            sLine += recurso.IDVierte.ToString() + ConstantesYupana.SeparadorCampo; // Codigo de Vertimiento
            sLine += recurso.CatcodiTurbina.ToString() + ConstantesYupana.SeparadorCampo;
            sLine += recurso.IDTurbina.ToString() + ConstantesYupana.SeparadorCampo; // Codigo de Turbinamiento
            sLine += recurso.Recurpadre + ConstantesYupana.SeparadorCampo;
            sLine += ((recurso.RecNodoTopDestinoID != null) ? recurso.RecNodoTopDestinoID.ToString() : "0") + ConstantesYupana.SeparadorCampo;
            sLine += ((recurso.RecNodoTopOrigenID != null) ? recurso.RecNodoTopOrigenID.ToString() : "0") + ConstantesYupana.SeparadorCampo;
            sLine += ((recurso.RecNodoID != null) ? recurso.RecNodoID.ToString() : "0");
            if (propiedades.Count > 0)
                for (int i = 0; i < totalPropiedades; i++)
                {
                    var propiedad = propiedades.Find(x => x.Proporden == i);
                    if (propiedad != null)
                    {
                        if (propiedad.Valor != null)
                        {
                            decimal valor;
                            string stvalor = propiedades.Find(x => x.Proporden == i).Valor;
                            if (decimal.TryParse(stvalor, out valor))
                            {
                                stvalor = valor.ToString();
                            }
                            sLine += ConstantesYupana.SeparadorCampo + stvalor;
                        }

                        else
                            sLine += ConstantesYupana.SeparadorCampo + "";
                    }
                    else
                        sLine += ConstantesYupana.SeparadorCampo + "";
                }
            else
                for (int i = 1; i <= totalPropiedades; i++)
                    sLine += ConstantesYupana.SeparadorCampo + "";

            sLine += ConstantesYupana.SeparadorCampo + recurso.Recurestado.ToString();
            return sLine;
        }

        /// <summary>
        /// Reemplaza campos de caracteres especiales
        /// </summary>
        /// <param name="campo"></param>
        /// <returns></returns>
        public string CamposinCaracEsp(string campo)
        {
            string resultado = string.Empty;
            resultado = campo.Replace("\r\n", "¿");
            resultado = campo.Replace("\n", "¿");
            resultado = resultado.Replace(",", "^");

            return resultado;
        }

        /// <summary>
        /// crea archivo csv detcostof
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearCostoFuturo(string ruta, int topcodi, List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var lstFcf = GetByIdCpFcostof(topcodi);
            string embalses = string.Empty;
            if (lstFcf != null)
            {
                if (!string.IsNullOrEmpty(lstFcf.Fcfembalses))
                {
                    var listaEmbalses = lstFcf.Fcfembalses.Split(',');
                    for (int i = 0; i < listaEmbalses.Count(); i++)
                    {
                        var find = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == int.Parse(listaEmbalses[i]));
                        if (find != null)
                            listaEmbalses[i] = find.Item2.ToString();
                        else
                            listaEmbalses[i] = "0";
                    }
                    embalses = lstFcf.Fcfnumcortes + "," + string.Join(",", listaEmbalses);
                }
            }
            var lista = FactorySic.GetCpDetfcostofRepository().GetByCriteria(topcodi);
            UtilYupana.AgregaLinea(ref textoArchivo, embalses);
            foreach (var reg in lista)
            {
                UtilYupana.AgregaLinea(ref textoArchivo, reg.Detfcfvalores);

            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoDetfcostofCsv, ruta, textoArchivo);
        }
        /// <summary>
        /// crea archivo csv equiposnodot
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearEquipoConecNodoTopologico(string ruta, int pTopologia, List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var listaEqNTop = FactorySic.GetCpRecursoRepository().ListarEquiposConecANodoTop(pTopologia);
            foreach (var reg in listaEqNTop)
            {
                if (reg.Recurcodi != reg.RecurcodiConec)
                {
                    var findNodo = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == reg.Recurcodi);
                    if (findNodo != null)
                    {
                        int nodoT = findNodo.Item2;
                        var findEqConec = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == reg.RecurcodiConec);
                        if (findEqConec != null)
                        {
                            int equipoConec = findEqConec.Item2;
                            int tipoEquipoConec = reg.CatcodiConec;
                            reg.Recurcodi = nodoT;
                            reg.RecurcodiConec = equipoConec;
                            sLine += reg.Recurcodi.ToString() + ConstantesYupana.SeparadorCampo;
                            sLine += reg.CatcodiConec.ToString() + ConstantesYupana.SeparadorCampo;
                            sLine += reg.RecurcodiConec.ToString();
                            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                        }
                    }
                }
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoEquipoNotoT, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo csv restricciones
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearRestricciones48Csv(string ruta, int pTopologia, DateTime fecha, List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var listaMedicion48 = FactorySic.GetCpMedicion48Repository().List(pTopologia).Where(x => x.Medifecha == fecha).ToList();
            int i = 0;
            foreach (var entity in listaMedicion48)
            {
                i++;
                var med48 = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.Recurcodi);
                if (med48 != null)
                {
                    entity.Recurcodi = med48.Item2;
                    sLine = entity.Recurcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += CamposinCaracEsp((entity.Recurnombre != null) ? entity.Recurnombre : "") + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Catcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += CamposinCaracEsp((entity.Catnombre != null) ? entity.Catnombre : "") + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Srestcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += CamposinCaracEsp((entity.Srestnombre != null) ? entity.Srestnombre : "") + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Medifecha.ToString(ConstantesBase.FormatoFecha) + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H1 != null) ? entity.H1.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H2 != null) ? entity.H2.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H3 != null) ? entity.H3.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H4 != null) ? entity.H4.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H5 != null) ? entity.H5.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H6 != null) ? entity.H6.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H7 != null) ? entity.H7.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H8 != null) ? entity.H8.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H9 != null) ? entity.H9.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H10 != null) ? entity.H10.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H11 != null) ? entity.H11.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H12 != null) ? entity.H12.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H13 != null) ? entity.H13.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H14 != null) ? entity.H14.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H15 != null) ? entity.H15.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H16 != null) ? entity.H16.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H17 != null) ? entity.H17.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H18 != null) ? entity.H18.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H19 != null) ? entity.H19.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H20 != null) ? entity.H20.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H21 != null) ? entity.H21.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H22 != null) ? entity.H22.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H23 != null) ? entity.H23.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H24 != null) ? entity.H24.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H25 != null) ? entity.H25.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H26 != null) ? entity.H26.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H27 != null) ? entity.H27.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H28 != null) ? entity.H28.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H29 != null) ? entity.H29.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H30 != null) ? entity.H30.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H31 != null) ? entity.H31.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H32 != null) ? entity.H32.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H33 != null) ? entity.H33.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H34 != null) ? entity.H34.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H35 != null) ? entity.H35.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H36 != null) ? entity.H36.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H37 != null) ? entity.H37.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H38 != null) ? entity.H38.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H39 != null) ? entity.H39.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H40 != null) ? entity.H40.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H41 != null) ? entity.H41.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H42 != null) ? entity.H42.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H43 != null) ? entity.H43.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H44 != null) ? entity.H44.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H45 != null) ? entity.H45.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H46 != null) ? entity.H46.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H47 != null) ? entity.H47.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H48 != null) ? entity.H48.ToString() : "");
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoMedicion48, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo csv restricciones
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearRestricciones48CsvYupanaContinuo(string ruta, string archivo, int pTopologia, DateTime fecha, List<CpMedicion48DTO> listaCondTermicas, List<CpMedicion48DTO> listaProyRer, List<CpMedicion48DTO> listaCompromiso, int tipo, List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var listaMedicion48 = FactorySic.GetCpMedicion48Repository().List(pTopologia).Where(x => x.Medifecha == fecha).ToList(); ;

            switch (tipo)
            {
                case 0:
                    listaMedicion48 = listaMedicion48.Where(x => x.Srestcodi != ConstantesBase.SRES_APORTES_PH && x.Srestcodi != ConstantesBase.SRES_APORTES_EMB).ToList();
                    listaMedicion48.AddRange(listaCompromiso);
                    break;
                case 1:
                    listaMedicion48 = listaMedicion48.Where(x => x.Srestcodi != ConstantesBase.SRES_GENER_RER).ToList();
                    listaMedicion48.AddRange(listaProyRer);
                    break;
                case 2:
                    listaMedicion48 = listaMedicion48.Where(x => x.Srestcodi != ConstantesBase.SRES_UNFOR_PT).ToList();
                    listaMedicion48.AddRange(listaCondTermicas);
                    break;
                case 3:
                    listaMedicion48 = listaMedicion48.Where(x => x.Srestcodi != ConstantesBase.SRES_APORTES_PH && x.Srestcodi != ConstantesBase.SRES_APORTES_EMB && x.Srestcodi != ConstantesBase.SRES_GENER_RER).ToList();
                    listaMedicion48.AddRange(listaCompromiso);
                    listaMedicion48.AddRange(listaProyRer);
                    break;
                case 4:
                    listaMedicion48 = listaMedicion48.Where(x => x.Srestcodi != ConstantesBase.SRES_APORTES_PH && x.Srestcodi != ConstantesBase.SRES_APORTES_EMB && x.Srestcodi != ConstantesBase.SRES_UNFOR_PT).ToList();
                    listaMedicion48.AddRange(listaCompromiso);
                    listaMedicion48.AddRange(listaCondTermicas);
                    break;
                case 5:
                    listaMedicion48 = listaMedicion48.Where(x => x.Srestcodi != ConstantesBase.SRES_GENER_RER && x.Srestcodi != ConstantesBase.SRES_UNFOR_PT).ToList();
                    listaMedicion48.AddRange(listaCondTermicas);
                    listaMedicion48.AddRange(listaProyRer);
                    break;
                case 6:
                    listaMedicion48 = listaMedicion48.Where(x => x.Srestcodi != ConstantesBase.SRES_APORTES_PH && x.Srestcodi != ConstantesBase.SRES_APORTES_EMB &&
                    x.Srestcodi != ConstantesBase.SRES_UNFOR_PT && x.Srestcodi != ConstantesBase.SRES_GENER_RER).ToList();
                    listaMedicion48.AddRange(listaCondTermicas);
                    listaMedicion48.AddRange(listaProyRer);
                    listaMedicion48.AddRange(listaCompromiso);
                    break;
            }

            foreach (var entity in listaMedicion48)
            {
                var med48 = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.Recurcodi);
                if (med48 != null)
                {
                    entity.Recurcodi = med48.Item2;
                    sLine = entity.Recurcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += CamposinCaracEsp((entity.Recurnombre != null) ? entity.Recurnombre : "") + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Catcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += CamposinCaracEsp((entity.Catnombre != null) ? entity.Catnombre : "") + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Srestcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += CamposinCaracEsp((entity.Srestnombre != null) ? entity.Srestnombre : "") + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Medifecha.ToString(ConstantesBase.FormatoFecha) + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H1 != null) ? entity.H1.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H2 != null) ? entity.H2.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H3 != null) ? entity.H3.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H4 != null) ? entity.H4.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H5 != null) ? entity.H5.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H6 != null) ? entity.H6.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H7 != null) ? entity.H7.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H8 != null) ? entity.H8.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H9 != null) ? entity.H9.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H10 != null) ? entity.H10.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H11 != null) ? entity.H11.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H12 != null) ? entity.H12.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H13 != null) ? entity.H13.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H14 != null) ? entity.H14.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H15 != null) ? entity.H15.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H16 != null) ? entity.H16.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H17 != null) ? entity.H17.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H18 != null) ? entity.H18.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H19 != null) ? entity.H19.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H20 != null) ? entity.H20.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H21 != null) ? entity.H21.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H22 != null) ? entity.H22.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H23 != null) ? entity.H23.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H24 != null) ? entity.H24.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H25 != null) ? entity.H25.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H26 != null) ? entity.H26.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H27 != null) ? entity.H27.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H28 != null) ? entity.H28.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H29 != null) ? entity.H29.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H30 != null) ? entity.H30.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H31 != null) ? entity.H31.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H32 != null) ? entity.H32.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H33 != null) ? entity.H33.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H34 != null) ? entity.H34.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H35 != null) ? entity.H35.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H36 != null) ? entity.H36.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H37 != null) ? entity.H37.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H38 != null) ? entity.H38.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H39 != null) ? entity.H39.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H40 != null) ? entity.H40.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H41 != null) ? entity.H41.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H42 != null) ? entity.H42.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H43 != null) ? entity.H43.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H44 != null) ? entity.H44.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H45 != null) ? entity.H45.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H46 != null) ? entity.H46.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H47 != null) ? entity.H47.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.H48 != null) ? entity.H48.ToString() : "");
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            FileHelper.GenerarArchivo(archivo, ruta, textoArchivo);
        }


        /// <summary>
        /// Crea archivo csv subrestricciondat
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearRestriccionesDatCsv(string ruta, int pTopologia, List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var listaSubrestriccionesdat = FactorySic.GetCpSubrestricdatRepository().List(pTopologia);
            foreach (var entity in listaSubrestriccionesdat)
            {
                var subdat = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.Recurcodi);
                if (subdat != null)
                {
                    entity.Recurcodi = subdat.Item2;
                    sLine = entity.Recurcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.Srestdvalor1 != null) ? entity.Srestdvalor1.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.Srestdvalor2 != null) ? entity.Srestdvalor2.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.Srestdvalor3 != null) ? entity.Srestdvalor3.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.Srestdvalor4 != null) ? entity.Srestdvalor4.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.Srestdactivo != null) ? entity.Srestdactivo.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.Srestdopcion != null) ? entity.Srestdopcion.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Catcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Srestcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.Srestfecha != null) ? ((DateTime)(entity.Srestfecha)).ToString(ConstantesBase.FormatoFechaHora) : "");
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoSurestricdat, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo csv gruporecurso
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearGrupoRecursoCsv(string ruta, int pTopologia, List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var listaGrupo = FactorySic.GetCpGruporecursoRepository().List(pTopologia);
            foreach (var entity in listaGrupo)
            {
                var find = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.Recurcodi);
                if (find != null)
                    entity.Recurcodi = find.Item2;
                var find2 = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.Recurcodisicoes);
                if (find2 != null)
                    entity.Recurcodisicoes = find2.Item2;
                sLine = entity.Recurcodi + ConstantesYupana.SeparadorCampo;
                sLine += entity.Catcodimain + ConstantesYupana.SeparadorCampo;
                sLine += entity.Recurcodisicoes + ConstantesYupana.SeparadorCampo;
                sLine += entity.Catcodisec + ConstantesYupana.SeparadorCampo;
                sLine += entity.Grurecval1 + ConstantesYupana.SeparadorCampo;
                sLine += entity.Grurecval2 + ConstantesYupana.SeparadorCampo;
                sLine += entity.Grurecval3 + ConstantesYupana.SeparadorCampo;
                sLine += entity.Grurecval4 + ConstantesYupana.SeparadorCampo;
                sLine += entity.Grurecorden + ConstantesYupana.SeparadorCampo;
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoGrupoRec, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo csv relaciones
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name=""></param>
        /// <param name="pTopologia"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearRelacionesCsv(string ruta, int pTopologia, List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var listaRelaciones = FactorySic.GetCpRelacionRepository().List(pTopologia, "-1");
            foreach (var entity in listaRelaciones)
            {
                var find = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.Recurcodi1);
                if (find != null)
                    entity.Recurcodi1 = find.Item2;
                var find2 = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.Recurcodi2);
                if (find2 != null)
                    entity.Recurcodi2 = find2.Item2;

                sLine = entity.Recurcodi1 + ConstantesYupana.SeparadorCampo;
                sLine += entity.Catcodi1 + ConstantesYupana.SeparadorCampo;
                sLine += entity.Recurcodi2 + ConstantesYupana.SeparadorCampo;
                sLine += entity.Catcodi2 + ConstantesYupana.SeparadorCampo;
                sLine += entity.Cptrelcodi + ConstantesYupana.SeparadorCampo;
                sLine += entity.Cpreltiempo + ConstantesYupana.SeparadorCampo;
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoRelacion, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea fila contenido para archuivo csv de transmision
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lpropiedades"></param>
        /// <param name="propiedades"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearTransmisionCsv(string ruta, int catcodi, int pTopologia, string nombreArchivo, List<CpPropiedadDTO> lpropiedades, List<CpProprecursoDTO> propiedades, ref List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            int recurcodiCSV = 0;
            int total;
            string sLine = CrearCabeceraEquipo(ruta, lpropiedades);
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaT = ListarRecurso(catcodi, pTopologia).OrderBy(x => x.Recurcodi).ToList();
            total = lpropiedades.Count();
            foreach (var reg in listaT)
            {
                var aux = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == reg.RecNodoTopDestinoID);
                if (aux != null)
                    reg.RecNodoTopDestinoID = aux.Item2;
                var aux2 = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == reg.RecNodoTopOrigenID);
                if (aux2 != null)
                    reg.RecNodoTopOrigenID = aux2.Item2;

                var lista = propiedades.Where(x => x.Recurcodi == reg.Recurcodi);
                recurcodiCSV++;
                lista_Recurcodi_OnLine_OffLine.Add(Tuple.Create(reg.Recurcodi, recurcodiCSV));
                sLine = CreateFilaString(reg, propiedades, total, recurcodiCSV, reg.Recurcodi);
                reg.RecurcodiOffline = reg.Recurcodi;
                reg.Recurcodi = recurcodiCSV;
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(nombreArchivo, ruta, textoArchivo);
        }


        /// <summary>
        /// Crea fila contenido para archuivo csv unidad termica
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lpropiedades"></param>
        /// <param name="propiedades"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearModoTCsv(string ruta, int pTopologia, List<CpPropiedadDTO> lpropiedades, List<CpProprecursoDTO> propiedades, ref List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            int recurcodiCSV = 0;
            int total;
            string sLine = CrearCabeceraEquipo(ruta, lpropiedades);
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaModoT = ListarRecurso(ConstantesBase.ModoT, pTopologia).OrderBy(x => x.Recurcodi).ToList();
            foreach (var reg in listaModoT)
            {
                //Encontrar nodo topologico conectado
                reg.RecNodoID = ObtenerNodoTopologico((int)reg.Recurpadre, ConstantesBase.Tergtermico, pTopologia);
                if (reg.RecNodoID != 0)
                {
                    var aux = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == reg.RecNodoID);
                    if (aux != null)
                        reg.RecNodoID = aux.Item2;
                }
                var aux2 = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == reg.Recurpadre);
                if (aux2 != null)
                {
                    reg.Recurpadre = aux2.Item2;
                }
                total = lpropiedades.Count();
                var lista = propiedades.Where(x => x.Recurcodi == reg.Recurcodi);
                recurcodiCSV++;
                lista_Recurcodi_OnLine_OffLine.Add(Tuple.Create(reg.Recurcodi, recurcodiCSV));
                sLine = CreateFilaString(reg, propiedades, total, recurcodiCSV, reg.Recurcodi);
                reg.RecurcodiOffline = reg.Recurcodi;
                reg.Recurcodi = recurcodiCSV;
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoNodoT, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea fila contenido para archuivo csv unidad termica
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lpropiedades"></param>
        /// <param name="propiedades"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearUnidadTCsv(string ruta, int pTopologia, List<CpPropiedadDTO> lpropiedades, List<CpProprecursoDTO> propiedades, ref List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            int recurcodiCSV = 0;
            int total;
            string sLine = CrearCabeceraEquipo(ruta, lpropiedades);
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaUnidadT = ListarRecurso(ConstantesBase.UnidadT, pTopologia).OrderBy(x => x.Recurcodi).ToList();
            foreach (var reg in listaUnidadT)
            {
                var aux = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == reg.Recurpadre);
                if (aux != null)
                {
                    reg.Recurpadre = aux.Item2;
                }
                total = lpropiedades.Count();
                var lista = propiedades.Where(x => x.Recurcodi == reg.Recurcodi);  //ListarPropiedadxRecurso(reg.Recurcodi, pTopologia, string.Empty);
                recurcodiCSV++;
                lista_Recurcodi_OnLine_OffLine.Add(Tuple.Create(reg.Recurcodi, recurcodiCSV));
                sLine = CreateFilaString(reg, propiedades, total, recurcodiCSV, reg.Recurcodi);
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                reg.RecurcodiOffline = reg.Recurcodi;
                reg.Recurcodi = recurcodiCSV;
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoUnidadT, ruta, textoArchivo);
        }

        /// <summary>
        /// Crear archivo csv de recurso
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lpropiedades"></param>
        /// <param name="propiedades"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearRecursoCsv(string ruta, short catcodi, string nombreArchivo, int pTopologia, List<CpPropiedadDTO> lpropiedades, List<CpProprecursoDTO> propiedades, ref List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            int recurcodiCSV = 0;
            int total;
            string sLine = CrearCabeceraEquipo(ruta, lpropiedades);
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaRecurso = ListarRecurso(catcodi, pTopologia).OrderBy(x => x.Recurcodi).ToList();
            foreach (var entity in listaRecurso)
            {
                var lista = propiedades.Where(x => x.Recurcodi == entity.Recurcodi);
                var find = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.Recurpadre);
                if (find != null)
                {
                    entity.Recurpadre = find.Item2;
                }
                recurcodiCSV++;
                total = lpropiedades.Count();
                // El nuevo codigo pasa como recurcodi y el codigo de BD se traslada a recurcodioffline
                sLine = CreateFilaString(entity, propiedades, total, recurcodiCSV, entity.Recurcodi);
                lista_Recurcodi_OnLine_OffLine.Add(Tuple.Create(entity.Recurcodi, recurcodiCSV));
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                entity.RecurcodiOffline = entity.Recurcodi;
                entity.Recurcodi = recurcodiCSV;

            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoNodoT, ruta, textoArchivo);
        }

        /// <summary>
        /// Crear archivo csv de recurso 2
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lpropiedades"></param>
        /// <param name="propiedades"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearRecurso2Csv(string ruta, short catcodi, string nombreArchivo, short tipoTerminal, int pTopologia, List<CpPropiedadDTO> lpropiedades, List<CpProprecursoDTO> propiedades, ref List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            int recurcodiCSV = 0;
            int total;
            string sLine = CrearCabeceraEquipo(ruta, lpropiedades);
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaRecurso = ListarRecurso(catcodi, pTopologia).OrderBy(x => x.Recurcodi).ToList();
            foreach (var entity in listaRecurso)
            {
                var find = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.Recurpadre);
                if (find != null)
                {
                    entity.Recurpadre = find.Item2;
                }
                entity.RecNodoID = ObtenerNodoTopologico(entity.Recurcodi, tipoTerminal, pTopologia);
                if (entity.RecNodoID != 0)
                {
                    var regnodo = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.RecNodoID);
                    if (regnodo != null)
                        entity.RecNodoID = regnodo.Item2;
                }
                var lista = propiedades.Where(x => x.Recurcodi == entity.Recurcodi);
                recurcodiCSV++;
                total = lpropiedades.Count();
                // El nuevo codigo pasa como recurcodi y el codigo de BD se traslada a recurcodioffline
                sLine = CreateFilaString(entity, propiedades, total, recurcodiCSV, entity.Recurcodi);
                lista_Recurcodi_OnLine_OffLine.Add(Tuple.Create(entity.Recurcodi, recurcodiCSV));
                entity.RecurcodiOffline = entity.Recurcodi;
                entity.Recurcodi = recurcodiCSV;

                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(nombreArchivo, ruta, textoArchivo);
        }


        /// <summary>
        /// Crear archivo csv de plantas hidros
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lpropiedades"></param>
        /// <param name="propiedades"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearPlantaHCsv(string ruta, int pTopologia, List<CpPropiedadDTO> lpropiedades, List<CpProprecursoDTO> propiedades, ref List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            int recurcodiCSV = 0;
            int total;
            string sLine = CrearCabeceraEquipo(ruta, lpropiedades);
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaPLantaH = ListarRecurso(ConstantesBase.PlantaH, pTopologia).OrderBy(x => x.Recurcodi).ToList();
            foreach (var entity in listaPLantaH)
            {
                entity.RecNodoID = ObtenerNodoTopologico(entity.Recurcodi, ConstantesBase.Terghidro, pTopologia);
                if (entity.RecNodoID != 0)
                {
                    var findN = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.RecNodoID);
                    if (findN != null)
                        entity.RecNodoID = findN.Item2;
                    else
                        entity.RecNodoID = 0;
                }

                var lista = propiedades.Where(x => x.Recurcodi == entity.Recurcodi);
                recurcodiCSV++;
                total = lpropiedades.Count();
                // El nuevo codigo pasa como recurcodi y el codigo de BD se traslada a recurcodioffline
                sLine = CreateFilaString(entity, propiedades, total, recurcodiCSV, entity.Recurcodi);
                lista_Recurcodi_OnLine_OffLine.Add(Tuple.Create(entity.Recurcodi, recurcodiCSV));
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                entity.RecurcodiOffline = entity.Recurcodi;
                entity.Recurcodi = recurcodiCSV;

            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoNodoT, ruta, textoArchivo);
        }

        /// <summary>
        /// Crear archivo csv de plantas termicas
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lpropiedades"></param>
        /// <param name="propiedades"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearPlantaRerCsv(string ruta, int pTopologia, List<CpPropiedadDTO> lpropiedades, List<CpProprecursoDTO> propiedades, ref List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            int recurcodiCSV = 0;
            int total;
            string sLine = CrearCabeceraEquipo(ruta, lpropiedades);
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaPLantaR = ListarRecurso(ConstantesBase.PlantaNoConvenO, pTopologia).OrderBy(x => x.Recurcodi).ToList();
            foreach (var entity in listaPLantaR)
            {
                entity.RecNodoID = ObtenerNodoTopologico(entity.Recurcodi, ConstantesBase.Terplantarer, pTopologia);
                if (entity.RecNodoID != 0)
                    entity.RecNodoID = lista_Recurcodi_OnLine_OffLine.Find(x => x.Item1 == entity.RecNodoID).Item2;
                var lista = propiedades.Where(x => x.Recurcodi == entity.Recurcodi);
                recurcodiCSV++;
                total = lpropiedades.Count();
                // El nuevo codigo pasa como recurcodi y el codigo de BD se traslada a recurcodioffline
                sLine = CreateFilaString(entity, propiedades, total, recurcodiCSV, entity.Recurcodi);
                lista_Recurcodi_OnLine_OffLine.Add(Tuple.Create(entity.Recurcodi, recurcodiCSV));
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                entity.RecurcodiOffline = entity.Recurcodi;
                entity.Recurcodi = recurcodiCSV;

            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoPlantaNoConvO, ruta, textoArchivo);
        }


        /// <summary>
        /// Crear archivo csv nodo_topologico
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        /// <param name="lpropiedades"></param>
        /// <param name="propiedades"></param>
        /// <param name="lista_Recurcodi_OnLine_OffLine"></param>
        public void CrearNodoTopologicoCsv(string ruta, int pTopologia, List<CpPropiedadDTO> lpropiedades, List<CpProprecursoDTO> propiedades, ref List<Tuple<int, int>> lista_Recurcodi_OnLine_OffLine)
        {
            string textoArchivo = string.Empty;
            int recurcodiCSV = 0;
            int total;
            string sLine = CrearCabeceraEquipo(ruta, lpropiedades);
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaNodoT = ListarRecurso(ConstantesBase.NodoTopologico, pTopologia).OrderBy(x => x.Recurcodi).ToList();
            foreach (var entity in listaNodoT)
            {
                var lista = propiedades.Where(x => x.Recurcodi == entity.Recurcodi);
                recurcodiCSV++;
                total = lpropiedades.Count();
                // El nuevo codigo pasa como recurcodi y el codigo de BD se traslada a recurcodioffline
                sLine = CreateFilaString(entity, propiedades, total, recurcodiCSV, entity.Recurcodi);
                lista_Recurcodi_OnLine_OffLine.Add(Tuple.Create(entity.Recurcodi, recurcodiCSV));
                entity.RecurcodiOffline = entity.Recurcodi;
                entity.Recurcodi = recurcodiCSV;
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoNodoT, ruta, textoArchivo);
        }

        /// <summary>
        /// Crear archivos csv de subrestricciones
        /// </summary>
        /// <param name="ruta"></param>
        public void CrearSusrestriccionesCsv(string ruta)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var listaSubrestricciones = FactorySic.GetCpSubrestriccionRepository().List().Where(x => x.Restriccodi != null);
            foreach (var entity in listaSubrestricciones)
            {
                sLine = entity.Catcodi.ToString() + ConstantesYupana.SeparadorCampo;
                sLine += entity.Restriccodi.ToString() + ConstantesYupana.SeparadorCampo;
                sLine += entity.Srestcodi.ToString() + ConstantesYupana.SeparadorCampo;

                if (entity.Srestnombre != null)
                {
                    string valorsinnenter = entity.Srestnombre.Replace("\r\n", "¿");
                    valorsinnenter = valorsinnenter.Replace("\n", "¿");
                    sLine += valorsinnenter + ConstantesYupana.SeparadorCampo;
                }
                else
                    sLine += "" + ConstantesYupana.SeparadorCampo;
                if (entity.Srestnombregams != null)
                    sLine += entity.Srestnombregams.ToString() + ConstantesYupana.SeparadorCampo;
                else
                    sLine += "" + ConstantesYupana.SeparadorCampo;

                if (entity.Srestunidad != null)
                    sLine += entity.Srestunidad.ToString() + ConstantesYupana.SeparadorCampo;
                else
                    sLine += "" + ConstantesYupana.SeparadorCampo;
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoSurestric, ruta, textoArchivo);
        }
        /// <summary>
        /// Crear archivos csv fuente gams
        /// </summary>
        /// <param name="ruta"></param>
        public void CrearFuenteGamsCsv(string ruta)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var lista = FactorySic.GetCpFuentegamsRepository().List();
            foreach (var entity in lista)
            {
                sLine = entity.Ftegcodi + ConstantesYupana.SeparadorCampo;
                sLine += entity.Ftegnombre + ConstantesYupana.SeparadorCampo;
                sLine += entity.Ftegestado + ConstantesYupana.SeparadorCampo;
                sLine += entity.Ftemetodo + ConstantesYupana.SeparadorCampo;

                if (entity.Fversruncase != null)
                {
                    string valorsinnenter = entity.Fversruncase.Replace("\r\n", "¿");
                    valorsinnenter = valorsinnenter.Replace("\n", "¿");
                    sLine += valorsinnenter + ConstantesYupana.SeparadorCampo;
                }
                else
                    sLine += "" + ConstantesYupana.SeparadorCampo;
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivofuenteCsv, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo csv Propiedades
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="listaPropiedades"></param>
        public void CrearPropiedadesCsv(string ruta, List<CpPropiedadDTO> listaPropiedades)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            int i = 0;
            try
            {
                foreach (var entity in listaPropiedades)
                {
                    sLine = entity.Catcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Propabrev.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Propcodi.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += "0" + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Propnombre.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += entity.Proporden.ToString() + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.Proptipo != null) ? entity.Proptipo.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.Propunidad != null) ? entity.Propunidad.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    sLine += ((entity.Propabrev != null) ? entity.Propabrev.ToString() : "") + ConstantesYupana.SeparadorCampo;
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                    i++;
                }
            }
            catch (Exception ex)
            {
                int h = i;
            }

            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoPropiedad, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo csv escenario
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="entity"></param>
        private void CrearParametroCsv(string ruta, int pTopologia)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var lista = GetByCriteriaCpParametros(pTopologia);
            foreach (var entity in lista)
            {
                sLine = entity.Paramcodi.ToString() + ConstantesYupana.SeparadorCampo;
                sLine += ((entity.Paramnombre != null) ? entity.Paramnombre : "") + ConstantesYupana.SeparadorCampo;
                sLine += ((entity.Paramunidad != null) ? entity.Paramunidad : "") + ConstantesYupana.SeparadorCampo;
                string valorsinnenter = entity.Paramvalor.Replace("\r\n", "¿");
                valorsinnenter = valorsinnenter.Replace("\n", "¿");
                sLine += valorsinnenter;
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoParametroCsv, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo csv categoria
        /// </summary>
        /// <param name="ruta"></param>
        public void CrearCategoriaCsv(string ruta)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var listaCategoria = FactorySic.GetCpRecursoRepository().ListaCategoria();
            foreach (var entity in listaCategoria)
            {
                sLine = entity.Catcodi + ConstantesYupana.SeparadorCampo;
                sLine += entity.Catnombre + ConstantesYupana.SeparadorCampo;
                sLine += entity.Catdescripcion + ConstantesYupana.SeparadorCampo;
                sLine += entity.Catmatrizgams + ConstantesYupana.SeparadorCampo;
                sLine += entity.Catprefijo + ConstantesYupana.SeparadorCampo;
                sLine += entity.Catabrev + ConstantesYupana.SeparadorCampo;
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoCategoriaCsv, ruta, textoArchivo);
        }
        /// <summary>
        /// Crea archivo csv escenario
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="entity"></param>
        private void CrearArchivoEscenarioCsv(string ruta, CpTopologiaDTO entity)
        {
            string sLine = string.Empty;
            sLine += entity.Topnombre + ConstantesYupana.SeparadorCampo;
            sLine += entity.Topfecha.ToString(ConstantesBase.FormatoFecha) + ConstantesYupana.SeparadorCampo;
            sLine += entity.Tophorizonte + ConstantesYupana.SeparadorCampo;
            sLine += entity.Tophora + ConstantesYupana.SeparadorCampo;
            sLine += entity.Toptipo + ConstantesYupana.SeparadorCampo;
            sLine += entity.Topinicio + ConstantesYupana.SeparadorCampo;
            sLine += entity.Topdiasproc + ConstantesYupana.SeparadorCampo;
            sLine += entity.Topiniciohora + ConstantesYupana.SeparadorCampo;
            sLine += entity.Topsinrsf + ConstantesYupana.SeparadorCampo;
            sLine += entity.Fverscodi + ConstantesYupana.SeparadorCampo;
            sLine += entity.Avercodi;

            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoEscenario, ruta, sLine);
        }

        /// <summary>
        /// Crea archivo csv Periodo
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pTopologia"></param>
        private void CrearPeriodoCsv(string ruta, int pTopologia)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            var listadetesc = ListCpDetalleetapas(pTopologia);
            foreach (var entity in listadetesc)
            {
                sLine = entity.Etpbloque.ToString() + ConstantesYupana.SeparadorCampo;
                sLine += entity.Etpini.ToString() + ConstantesYupana.SeparadorCampo;
                sLine += entity.Etpfin.ToString() + ConstantesYupana.SeparadorCampo;
                sLine += entity.Etpdelta.ToString();
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            FileHelper.GenerarArchivo(ConstantesYupana.NombArchivoEscenario, ruta, textoArchivo);
        }
        #endregion

        #region Proceso Yupana Continuo

        /// <summary>
        /// Crea directorios base Yupana Continuo
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="arbol"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string CrearDirectorioTrabajoYupanaContinuo(string rutaBase, CpArbolContinuoDTO arbol, DateTime fecha)
        {
            string directorio = string.Empty;
            directorio = rutaBase + "/" + fecha.ToString(ConstantesBase.FormatoFecha);
            FileHelper.CreateFolder2(rutaBase, fecha.ToString(ConstantesBase.FormatoFecha));

            FileHelper.CreateFolder2(directorio, arbol.Cparbidentificador);
            directorio += "/" + arbol.Cparbidentificador;

            FileHelper.CreateFolder2(directorio, arbol.Cparbtag);
            directorio += "/" + arbol.Cparbtag;

            //string rutaNodo = rutaBase + arbol.Cparbidentificador + "\\" + arbol.Cparbtag + "\\01\\"; //nodo.Cpnodocarpeta.subsrt(nodo.Cpnodocarpeta.len() - 2,2)
            return directorio;
        }

        /// <summary>
        /// Crea los directorios de todos los nodos en Yupana Continuo
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="arbol"></param>
        /// <param name="fecha"></param>
        /// <param name="listaCondTermicas"></param>
        /// <param name="listaProyRer"></param>
        /// <returns></returns>
        public string CrearArchivosDatCsvEnArbol(string ruta, CpArbolContinuoDTO arbol, DateTime fecha, InsumoYupanaContinuo objInsumo, int numProcesadores)
        {
            string directorio = CrearDirectorioTrabajoYupanaContinuo(ruta, arbol, fecha);
            var topologia = GetTopologia(arbol.Topcodi);
            FileHelper.CreateFolder2(directorio, "01");
            CrearArchivoGams(directorio + "//01//", topologia.Topcodi, topologia.Topinicio, topologia.Topiniciohora, topologia.Topdiasproc
                                                , objInsumo, numProcesadores);
            CrearArchivosCsv(directorio + "//01//", topologia.Topcodi, objInsumo);
            for (int i = 2; i <= 16; i++)
            {
                CopiarArchivosDataNodo(i, directorio);
                CopiarArchivosCsvNodo(i, directorio);
            }
            return directorio;
        }

        /// <summary>
        /// Copia Archivos de entrada Gams al directorio del nodo
        /// </summary>
        /// <param name="nroNodo"></param>
        /// <param name="rutaOrigen"></param>
        /// <param name="rutaDestino"></param>
        public void CopiarArchivosDataNodo(int nroNodo, string directorio)
        {
            string stNodo = "0" + nroNodo.ToString();
            string stNodoi = stNodo.Substring(stNodo.Length - 2, 2);
            string rutaOrigen = directorio + "\\01\\";
            string rutaDestino = directorio + "\\" + stNodoi + "\\";
            FileHelper.CreateFolder2(directorio, stNodoi);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPeriodo, ConstantesBase.NombArchivoPeriodo);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaTGams, ConstantesBase.NombArchivoPlantaTGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCondIniciales, ConstantesBase.NombArchivoCondIniciales);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoNodoTGams, ConstantesBase.NombArchivoNodoTGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoDemanda, ConstantesBase.NombArchivoDemanda);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoTemper, ConstantesBase.NombArchivoTemper);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoMantenimiento, ConstantesBase.NombArchivoMantenimiento);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoReserva, ConstantesBase.NombArchivoReserva);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaHGams, ConstantesBase.NombArchivoPlantaHGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoEmbalseGams, ConstantesBase.NombArchivoEmbalseGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoLineaGams, ConstantesBase.NombArchivoLineaGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoArranquesGams, ConstantesBase.NombArchivoArranquesGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoResTerUpGams, ConstantesBase.NombArchivoResTerUpGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoResTerDnGams, ConstantesBase.NombArchivoResTerDnGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoResHidUpGams, ConstantesBase.NombArchivoResHidUpGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoResHidDnGams, ConstantesBase.NombArchivoResHidDnGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoRSFGams, ConstantesBase.NombArchivoRSFGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoresURSGams, ConstantesBase.NombArchivoresURSGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoresResrangoGams, ConstantesBase.NombArchivoresResrangoGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoresCOSTURSGams, ConstantesBase.NombArchivoresCOSTURSGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoresProvisionBaseGams, ConstantesBase.NombArchivoresProvisionBaseGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivomantoLineasGams, ConstantesBase.NombArchivomantoLineasGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoVolMetaGams, ConstantesBase.NombArchivoVolMetaGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoVolumenGams, ConstantesBase.NombArchivoVolumenGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoResGener, ConstantesBase.NombArchivoResGener);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoSumaFlujos, ConstantesBase.NombArchivoSumaFlujos);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoGenerMeta, ConstantesBase.NombArchivoGenerMeta);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoDispComb, ConstantesBase.NombArchivoDispComb);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoFCostoF, ConstantesBase.NombArchivoFCostoF);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoConstantes, ConstantesBase.NombArchivoConstantes);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoTolerancia, ConstantesBase.NombArchivoTolerancia);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoOptSolver, ConstantesBase.NombArchivoOptSolver);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoEcuacion, ConstantesBase.NombArchivoEcuacion);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoParadasGams, ConstantesBase.NombArchivoParadasGams);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombreArchivoRegionSeguridad, ConstantesBase.NombreArchivoRegionSeguridad);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoNodoTopDescrip, ConstantesBase.NombArchivoNodoTopDescrip);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoEmbalseDescrip, ConstantesBase.NombArchivoEmbalseDescrip);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaRerDescrip, ConstantesBase.NombArchivoPlantaRerDescrip);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaOtrosDescrip, ConstantesBase.NombArchivoPlantaOtrosDescrip);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaTermicaDescrip, ConstantesBase.NombArchivoPlantaTermicaDescrip);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaHidroDescrip, ConstantesBase.NombArchivoPlantaHidroDescrip);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoLineaDescrip, ConstantesBase.NombArchivoLineaDescrip);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaUrsDescrip, ConstantesBase.NombArchivoPlantaUrsDescrip);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoSets, ConstantesBase.NombArchivoSets);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoEncriptado, ConstantesBase.NombArchivoEncriptado);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoInputData, ConstantesBase.NombArchivoInputData);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoLogGamsMetodo, ConstantesBase.NombArchivoLogGamsMetodo);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoConfiguraciones, ConstantesBase.NombArchivoConfiguraciones);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombreArchivoTiempoViaje, ConstantesBase.NombreArchivoTiempoViaje);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombreArchivoRsfTipo, ConstantesBase.NombreArchivoRsfTipo);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPerdidasModeloUninodal, ConstantesBase.NombArchivoPerdidasModeloUninodal);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombreArchivoUnidadesIndisponibles, ConstantesBase.NombreArchivoUnidadesIndisponibles);
            //Yupana 2022
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoTiempoTransicion, ConstantesBase.NombArchivoTiempoTransicion);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoTiempoCaldero, ConstantesBase.NombArchivoTiempoCaldero);
            //Fin Yupana 2022

            switch (nroNodo)
            {
                case 2:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudales, ConstantesBase.NombArchivoCaudales);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt, ConstantesBase.NombArchivoForzadaUt);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams2, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    break;
                case 3:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt, ConstantesBase.NombArchivoForzadaUt);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesCc, ConstantesBase.NombArchivoCaudales);
                    break;
                case 4:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt, ConstantesBase.NombArchivoForzadaUt);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams2, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesCc, ConstantesBase.NombArchivoCaudales);
                    break;
                case 5:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt, ConstantesBase.NombArchivoForzadaUt);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesSc, ConstantesBase.NombArchivoCaudales);
                    break;
                case 6:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt, ConstantesBase.NombArchivoForzadaUt);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams2, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesSc, ConstantesBase.NombArchivoCaudales);
                    break;
                case 7:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt, ConstantesBase.NombArchivoForzadaUt);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesCcSc, ConstantesBase.NombArchivoCaudales);
                    break;
                case 8:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt, ConstantesBase.NombArchivoForzadaUt);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams2, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesCcSc, ConstantesBase.NombArchivoCaudales);
                    break;
                case 9:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudales, ConstantesBase.NombArchivoCaudales);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt2, ConstantesBase.NombArchivoForzadaUt);
                    break;
                case 10:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudales, ConstantesBase.NombArchivoCaudales);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams2, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt2, ConstantesBase.NombArchivoForzadaUt);
                    break;
                case 11:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesCc, ConstantesBase.NombArchivoCaudales);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt2, ConstantesBase.NombArchivoForzadaUt);
                    break;
                case 12:
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams2, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesCc, ConstantesBase.NombArchivoCaudales);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt2, ConstantesBase.NombArchivoForzadaUt);
                    break;

                case 13:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt2, ConstantesBase.NombArchivoForzadaUt);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesSc, ConstantesBase.NombArchivoCaudales);
                    break;
                case 14:
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams2, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt2, ConstantesBase.NombArchivoForzadaUt);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesSc, ConstantesBase.NombArchivoCaudales);
                    break;
                case 15:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt2, ConstantesBase.NombArchivoForzadaUt);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesCcSc, ConstantesBase.NombArchivoCaudales);
                    break;
                case 16:
                    //Cambio
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoPlantaNoConvOGams2, ConstantesBase.NombArchivoPlantaNoConvOGams);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoForzadaUt2, ConstantesBase.NombArchivoForzadaUt);
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesBase.NombArchivoCaudalesCcSc, ConstantesBase.NombArchivoCaudales);
                    break;
            }
        }

        /// <summary>
        /// Copia Archivos csv al directorio del nodo
        /// </summary>
        /// <param name="nroNodo"></param>
        /// <param name="directorio"></param>
        /// <param name="topcodi"></param>
        public void CopiarArchivosCsvNodo(int nroNodo, string directorio)
        {
            string stNodo = "0" + nroNodo.ToString();
            string stNodoi = stNodo.Substring(stNodo.Length - 2, 2);
            string rutaOrigen = directorio + "\\01\\";
            string rutaDestino = directorio + "\\" + stNodoi + "\\";

            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoEscenario, ConstantesYupana.NombArchivoEscenario);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoDetEtapa, ConstantesYupana.NombArchivoDetEtapa);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoCategoriaCsv, ConstantesYupana.NombArchivoCategoriaCsv);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoParametroCsv, ConstantesYupana.NombArchivoParametroCsv);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoPropiedad, ConstantesYupana.NombArchivoPropiedad);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivofuenteCsv, ConstantesYupana.NombArchivofuenteCsv);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoSurestric, ConstantesYupana.NombArchivoSurestric);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoNodoT, ConstantesYupana.NombArchivoNodoT);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoPlantaH, ConstantesYupana.NombArchivoPlantaH);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoEmbalse, ConstantesYupana.NombArchivoEmbalse);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoPlantaNoConvO, ConstantesYupana.NombArchivoPlantaNoConvO);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoPlantaT, ConstantesYupana.NombArchivoPlantaT);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoUnidadT, ConstantesYupana.NombArchivoUnidadT);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoCombustible, ConstantesYupana.NombArchivoCombustible);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoModoT, ConstantesYupana.NombArchivoModoT);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoLinea, ConstantesYupana.NombArchivoLinea);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoTrafo2D, ConstantesYupana.NombArchivoTrafo2D);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoTrafo3D, ConstantesYupana.NombArchivoTrafo3D);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoURS, ConstantesYupana.NombArchivoURS);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoGrupoPrioridad, ConstantesYupana.NombArchivoGrupoPrioridad);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoCicloCombinado, ConstantesYupana.NombArchivoCicloCombinado);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoDisponComb, ConstantesYupana.NombArchivoDisponComb);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoSumaFlujosCsv, ConstantesYupana.NombArchivoSumaFlujosCsv);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoRsfCsv, ConstantesYupana.NombArchivoRsfCsv);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoResGenerCsv, ConstantesYupana.NombArchivoResGenerCsv);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoGenerMetaCsv, ConstantesYupana.NombArchivoGenerMetaCsv);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombreArchivoRegionSeguridadCsv, ConstantesYupana.NombreArchivoRegionSeguridadCsv);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoRelacion, ConstantesYupana.NombArchivoRelacion);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoGrupoRec, ConstantesYupana.NombArchivoGrupoRec);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoSurestricdat, ConstantesYupana.NombArchivoSurestricdat);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoEquipoNotoT, ConstantesYupana.NombArchivoEquipoNotoT);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoDetfcostofCsv, ConstantesYupana.NombArchivoDetfcostofCsv);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoGeneralesCsv, ConstantesYupana.NombArchivoGeneralesCsv);
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoCalderoCsv, ConstantesYupana.NombArchivoCalderoCsv);
            //
            FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48, ConstantesYupana.NombArchivoMedicion48);

            switch (nroNodo)
            {
                case 2:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48Rer, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 3:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48Cc, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 4:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48RerCc, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 5:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48Sc, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 6:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48RerSc, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 7:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48CcSc, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 8:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48RerCcSc, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 9:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48Forzadas, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 10:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48RerForzadas, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 11:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48CcForzadas, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 12:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48RerCcForzadas, ConstantesYupana.NombArchivoMedicion48);
                    break;

                case 13:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48ScForzadas, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 14:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48RerScForzadas, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 15:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48ForzadasCcSc, ConstantesYupana.NombArchivoMedicion48);
                    break;
                case 16:
                    FileHelper.CopiarFile(rutaOrigen, rutaDestino, ConstantesYupana.NombArchivoMedicion48ForzadasRerCcSc, ConstantesYupana.NombArchivoMedicion48);
                    break;
            }

        }

        #endregion

        #region Varios

        /// <summary>
        /// Crea archivo csv restricciones
        /// </summary>
        /// <param name="pTopologia"></param>
        public void CrearRestricciones48YupanaContinuo(int pTopologia, List<CpMedicion48DTO> listaCondTermicas, List<CpMedicion48DTO> listaProyRer, List<CpMedicion48DTO> listaAportesCC,
            List<CpMedicion48DTO> listaAportesSC, List<CpMedicion48DTO> listaAportesCCSC, int nodo)
        {
            string lSubrest = string.Empty;
            List<CpMedicion48DTO> listaMedicion48 = new List<CpMedicion48DTO>();

            switch (nodo)
            {
                case 3:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaAportesCC);
                    break;
                case 5:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaAportesSC);
                    break;
                case 7:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaAportesCCSC);
                    break;
                case 2:
                    lSubrest = ConstantesBase.SRES_GENER_RER.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaProyRer);
                    break;
                case 9:
                    lSubrest = ConstantesBase.SRES_UNFOR_PT.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaCondTermicas);
                    break;
                case 4:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString() + "," + ConstantesBase.SRES_GENER_RER.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaAportesCC);
                    listaMedicion48.AddRange(listaProyRer);
                    break;
                case 6:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString() + "," + ConstantesBase.SRES_GENER_RER.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaAportesSC);
                    listaMedicion48.AddRange(listaProyRer);
                    break;
                case 8:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString() + "," + ConstantesBase.SRES_GENER_RER.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaAportesCCSC);
                    listaMedicion48.AddRange(listaProyRer);
                    break;
                case 11:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString() + "," + ConstantesBase.SRES_UNFOR_PT.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaAportesCC);
                    listaMedicion48.AddRange(listaCondTermicas);
                    break;
                case 13:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString() + "," + ConstantesBase.SRES_UNFOR_PT.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaAportesSC);
                    listaMedicion48.AddRange(listaCondTermicas);
                    break;
                case 15:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString() + "," + ConstantesBase.SRES_UNFOR_PT.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaAportesCCSC);
                    listaMedicion48.AddRange(listaCondTermicas);
                    break;
                case 10:
                    lSubrest = ConstantesBase.SRES_GENER_RER.ToString() + "," + ConstantesBase.SRES_UNFOR_PT.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaCondTermicas);
                    listaMedicion48.AddRange(listaProyRer);
                    break;
                case 12:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString() + "," + ConstantesBase.SRES_UNFOR_PT.ToString()
                        + "," + ConstantesBase.SRES_GENER_RER.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaCondTermicas);
                    listaMedicion48.AddRange(listaProyRer);
                    listaMedicion48.AddRange(listaAportesCC);
                    break;
                case 14:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString() + "," + ConstantesBase.SRES_UNFOR_PT.ToString()
                        + "," + ConstantesBase.SRES_GENER_RER.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaCondTermicas);
                    listaMedicion48.AddRange(listaProyRer);
                    listaMedicion48.AddRange(listaAportesSC);
                    break;
                case 16:
                    lSubrest = ConstantesBase.SRES_APORTES_PH.ToString() + "," + ConstantesBase.SRES_APORTES_EMB.ToString() + "," + ConstantesBase.SRES_UNFOR_PT.ToString()
                        + "," + ConstantesBase.SRES_GENER_RER.ToString();
                    FactorySic.GetCpMedicion48Repository().DeleteTopSubrestric(pTopologia, lSubrest);
                    listaMedicion48.AddRange(listaCondTermicas);
                    listaMedicion48.AddRange(listaProyRer);
                    listaMedicion48.AddRange(listaAportesCCSC);
                    break;
            }

            //Grabar listaMedicion48
            foreach (var reg in listaMedicion48)
            {
                reg.Topcodi = pTopologia;
                FactorySic.GetCpMedicion48Repository().Save(reg);
            }


        }

        /// <summary>
        /// Crea un escenario
        /// </summary>
        /// <param name="topcodi1"></param>
        /// <param name="fecha1"></param>
        /// <param name="topologia"></param>
        /// <param name="signo"></param>
        public int CrearCopiaEscenario(int topcodi1, string nombre, InsumoYupanaContinuo objInsumo, int nodo)
        {

            var topologia = FactorySic.GetCpTopologiaRepository().GetById(topcodi1);
            DateTime fecha = topologia.Topfecha;
            topologia.Topnombre = nombre;
            topologia.Toptipo = 3;
            int topcodi2 = FactorySic.GetCpTopologiaRepository().Save(topologia);
            topologia.Topcodi = topcodi2;
            FactorySic.GetCpRecursoRepository().CrearCopia(topcodi1, topcodi2);
            FactorySic.GetCpTerminalRepository().CrearCopiaNodoConectividad(topcodi1, topcodi2);
            FactorySic.GetCpTerminalRepository().CrearCopia(topcodi1, topcodi2);
            FactorySic.GetCpRecurptomedRepository().CrearCopia(topcodi1, topcodi2);

            FactorySic.GetCpDetalleetapaRepository().CrearCopia(topcodi1, topcodi2);
            FactorySic.GetCpParametroRepository().CopiarParametroAEscenario(topcodi1, topcodi2);
            FactorySic.GetCpGruporecursoRepository().CrearCopia(topcodi1, topcodi2);
            FactorySic.GetCpProprecursoRepository().CrearCopia(topcodi1, topcodi2);
            FactorySic.GetCpFcostofRepository().CrearCopia(topcodi1, topcodi2);
            FactorySic.GetCpDetfcostofRepository().CrearCopia(topcodi1, topcodi2);
            FactorySic.GetCpMedicion48Repository().CrearCopia(topcodi1, topcodi2, fecha, fecha, -1);
            FactorySic.GetCpSubrestricdatRepository().CrearCopia(topcodi1, topcodi2);
            FactorySic.GetCpRelacionRepository().CrearCopia(topcodi1, topcodi2);

            // Actualizar Data
            CrearRestricciones48YupanaContinuo(topcodi2, objInsumo.ListaCondTermicas, objInsumo.ListaProyRer, objInsumo.ListaAportesCC, objInsumo.ListaAportesSC, objInsumo.ListaAportesCCSC, nodo);
            return topcodi2;
        }

        /// <summary>
        /// Devuelve lista de Costos Marginales
        /// </summary>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaCostoMarginales(int topcodi)
        {
            decimal? valor;
            List<MeMedicion48DTO> listaMed48 = new List<MeMedicion48DTO>();
            MeMedicion48DTO registro;
            var lista = ListaRestriciones(topcodi, ConstantesBase.SresCostoMarginalBarra);
            foreach (var reg in lista)
            {
                registro = new MeMedicion48DTO();
                registro.Medifecha = reg.Medifecha;
                registro.Equicodi = reg.Recurcodi;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    registro.GetType().GetProperty("H" + i.ToString()).SetValue(registro, valor);
                }
                listaMed48.Add(registro);
            }

            return listaMed48;
        }

        public void GetCostoRacionamiento(int topcodi, out decimal? costoRacionamiento, out decimal? costoTotal)
        {
            costoRacionamiento = 0;
            costoTotal = 0;
            List<CpSubrestricdatDTO> listaCosto = ListarDatosRestriccion(topcodi, ConstantesBase.RestricCostosOperacion);
            var lcostoRacionamiento = listaCosto.Where(x => x.Srestcodi == ConstantesBase.SresCostoRacionamiento).ToList();
            if (lcostoRacionamiento.Count > 0)
                costoRacionamiento = lcostoRacionamiento[0].Srestdvalor1;
            var lcostoTotal = listaCosto.Where(x => x.Srestcodi == ConstantesBase.SresCostoOperacion).ToList();
            if (lcostoTotal.Count > 0)
                costoTotal = lcostoTotal[0].Srestdvalor1 != null ? lcostoTotal[0].Srestdvalor1 / 1000 : null;
        }

        /// <summary>
        /// Lista de Relacion
        /// </summary>
        /// <param name="cptrelcodi"></param>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public List<CpRelacionDTO> ListCpRelacion(string cptrelcodi, int topcodi)
        {
            List<CpRelacionDTO> lista = new List<CpRelacionDTO>();
            return FactorySic.GetCpRelacionRepository().List(topcodi, cptrelcodi);
        }
        /// <summary>
        /// Permite realizar búsquedas en la tabla CpDetfcostof
        /// </summary>
        public List<CpDetfcostofDTO> GetByCriteriaCpDetfcostofs(int topcodi)
        {
            List<CpDetfcostofDTO> lista = new List<CpDetfcostofDTO>();
            lista = FactorySic.GetCpDetfcostofRepository().GetByCriteria(topcodi);

            return lista;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CP_FCOSTOF
        /// </summary>
        public CpFcostofDTO GetByIdCpFcostof(int topcodi)
        {
            CpFcostofDTO registro;
            registro = FactorySic.GetCpFcostofRepository().GetById(topcodi);
            return (registro != null) ? registro : new CpFcostofDTO();

        }

        /// <summary>
        /// Graba un registro de Función Costo Futuro
        /// </summary>
        /// <param name="entity"></param>
        public void SaveCpFcostof(CpFcostofDTO entity)
        {
            try
            {
                FactorySic.GetCpFcostofRepository().Save(entity);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Devuelve la lista de Función Costo Futuro
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="nCortes"></param>
        /// <returns></returns>
        public List<CpRecursoDTO> GetListaEmbalseFCostoF(int topcodi, ref int nCortes)
        {
            List<CpRecursoDTO> listaOrden = new List<CpRecursoDTO>();
            var lstFcf = GetByIdCpFcostof(topcodi);
            if (lstFcf == null)
            {
                lstFcf = new CpFcostofDTO();
                lstFcf.Topcodi = topcodi;
                lstFcf.Fcfembalses = string.Empty;
                lstFcf.Fcfnumcortes = 0;
                SaveCpFcostof(lstFcf);
            }
            if (lstFcf != null)
            {

                nCortes = (lstFcf.Fcfnumcortes == null) ? 0 : (int)lstFcf.Fcfnumcortes;
                string cabecera = string.Empty;
                if (!string.IsNullOrEmpty(lstFcf.Fcfembalses))
                {
                    cabecera = lstFcf.Fcfembalses;
                    var listaCabecera = cabecera.Split(',');
                    var lista = FactorySic.GetCpRecursoRepository().List(topcodi, cabecera);
                    for (int i = 0; i < listaCabecera.Count(); i++)
                    {
                        var find = lista.Find(x => x.Recurcodi == int.Parse(listaCabecera[i]));
                        listaOrden.Add(find);
                    }
                }
            }


            return listaOrden;
        }

        /// <summary>
        /// Obtiene datos del tipo de subrestriccion
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="srestcodi"></param>
        /// <returns></returns>
        public List<CpSubrestricdatDTO> ListarDatosSubRestriccion(int topcodi, int srestcodi)
        {
            List<CpSubrestricdatDTO> lista = new List<CpSubrestricdatDTO>();
            lista = FactorySic.GetCpSubrestricdatRepository().ListarDatosSubRestriccion(topcodi, srestcodi);
            return lista;
        }
        /// <summary>
        /// Obtiene Lista de Subrestricciones de registro de datos 48
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="srestcodi"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> Lista48SubRestriciones(int topcodi, short srestcodi)
        {
            List<CpMedicion48DTO> lista = new List<CpMedicion48DTO>();
            lista = FactorySic.GetCpMedicion48Repository().ListaRestricion(topcodi, srestcodi);
            return lista;
        }

        /// <summary>
        /// Encuentra el indice en la lista de etapas
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="hora"></param>
        /// <param name="horaInicio"></param>
        /// <returns></returns>
        public static int GetIndiceEtapa(List<EtapaDTO> lista, decimal hora, int horaInicio)
        {
            int nTotal = lista.Count;
            decimal acumulado = (decimal)horaInicio;

            int i = 0;
            while (i < nTotal)
            {
                acumulado += lista[i].Etpdelta;
                if (acumulado >= hora)
                {
                    i++;
                    break;
                }
                i++;
            }

            return i;
        }
        /// <summary>
        /// Obtiene Subrestriccion
        /// </summary>
        /// <param name="pRestriccion"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        public List<CpSubrestriccionDTO> ObtenerSubRestriccion(short pRestriccion)
        {
            List<CpSubrestriccionDTO> lista = new List<CpSubrestriccionDTO>();
            lista = FactorySic.GetCpSubrestriccionRepository().GetByCriteria(pRestriccion);
            return lista;
        }
        /// <summary>
        /// Obtiene el valor de una propiedad
        /// </summary>
        /// <param name="catcodi"></param>
        /// <param name="recurcodi"></param>
        /// <param name="propcodi"></param>
        /// <param name="fecha"></param>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        public string GetValorPropiedad(int catcodi, int recurcodi, int propcodi, DateTime fecha, int topcodi)
        {
            CpProprecursoDTO registro = new CpProprecursoDTO();
            string valor = string.Empty;
            registro = Factory.FactorySic.GetCpProprecursoRepository().GetById(recurcodi, propcodi, fecha, topcodi);
            if (registro != null)
            {
                if (registro.Valor != null)
                    valor = registro.Valor;
            }
            return valor;
        }
        /// <summary>
        /// Permite listar todos los registros de la tabla CP_DETALLEETAPA
        /// </summary>
        public List<CpDetalleetapaDTO> ListCpDetalleetapas(int topcodi)
        {
            return FactorySic.GetCpDetalleetapaRepository().List((int)topcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpParametro
        /// </summary>
        public List<CpParametroDTO> GetByCriteriaCpParametros(int topcodi)
        {
            List<CpParametroDTO> lista = new List<CpParametroDTO>();
            lista = FactorySic.GetCpParametroRepository().GetByCriteria(topcodi);
            return lista;
        }

        /// <summary>
        /// Lista de Subrestricciones para Gams
        /// </summary>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public List<CpSubrestricdatDTO> ListaCpSubRestriccionDat(int topcodi)
        {
            List<CpSubrestricdatDTO> lista;
            lista = FactorySic.GetCpSubrestricdatRepository().List(topcodi);
            return lista;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTopologia"></param>
        /// <param name="modo"></param>
        /// <param name="catcodi"></param>
        /// <returns></returns>
        public List<CpGruporecursoDTO> ListarGrupoRecursoFamiliaGams(int pTopologia, short catcodi, bool noconsideragams)
        {
            var lista = FactorySic.GetCpGruporecursoRepository().ListaGrupoPorCategoria(catcodi, pTopologia);
            if (noconsideragams)
            {
                lista = lista.Where(x => x.Recurconsideragams == 1).ToList();
            }
            return lista;
        }

        /// <summary>
        /// Lista recorsos por categoria para Gams
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="catcodi"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        public List<CpRecursoDTO> ListarRecursosPorCategoriaGams(List<CpRecursoDTO> lista, int catcodi)
        {

            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();

            entitys = lista.Where(x => x.Catcodi == catcodi && x.Recurconsideragams == 1).ToList();
            var lurs = lista.Where(x => x.Catcodi == ConstantesBase.Urs);
            foreach (var reg in lurs)
            {
                reg.ConsideraEquipo = reg.Recurconsideragams.ToString();//
                int resultado;
                var find = _propiedades.Find(x => x.Proporden == ConstantesBase.OrdenProvisionBaseUp && x.Recurcodi == reg.Recurcodi);
                if (find != null)
                {
                    if (int.TryParse(find.Valor, out resultado))
                        reg.ProvisionBaseUp = resultado;
                }
                find = _propiedades.Find(x => x.Proporden == ConstantesBase.OrdenProvisionBaseDn && x.Recurcodi == reg.Recurcodi);
                if (find != null)
                {
                    if (int.TryParse(find.Valor, out resultado))
                        reg.ProvisionBaseDn = resultado;
                }
            }

            switch (catcodi)
            {
                case ConstantesBase.Caldero:
                    decimal resultado;
                    foreach (var reg in lista)
                    {
                        var find = _propiedades.Find(x => x.Proporden == 1 && x.Recurcodi == reg.Recurcodi);
                        if (find != null)
                        {
                            if (decimal.TryParse(find.Valor, out resultado))
                                reg.TiempoES = resultado;
                        }
                        find = _propiedades.Find(x => x.Proporden == 2 && x.Recurcodi == reg.Recurcodi);
                        if (find != null)
                        {
                            if (decimal.TryParse(find.Valor, out resultado))
                                reg.TiempoFS = resultado;
                        }
                    }
                    break;
            }

            return entitys.OrderBy(x => x.Recurcodi).ToList();
        }

        /// <summary>
        /// Lista Subrestriccion por Categoria
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="catcodi"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        public List<CpSubrestricdatDTO> ListaSubRestriccionCategoria(int topcodi, int catcodi)
        {
            List<CpSubrestricdatDTO> lista;
            lista = FactorySic.GetCpSubrestricdatRepository().ListadeSubRestriccionCategoria(topcodi, catcodi);
            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"> Lista de propiedades totales para modo online</param>
        /// <param name="pCategoria"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        public List<CpPropiedadDTO> ObtenerListaPropiedadesCategoria(List<CpPropiedadDTO> lista, short pCategoria)
        {
            List<CpPropiedadDTO> entitys = new List<CpPropiedadDTO>();
            entitys = lista.Where(x => x.Catcodi == pCategoria).OrderBy(x => x.Proporden).ToList();
            return entitys;
        }

        /// <summary>
        /// Listar propiedad por recurso para Gams
        /// </summary>
        /// <param name="orden"></param>
        /// <param name="topcodi"></param>
        /// <param name="catcodi"></param>
        /// <returns></returns>
        public List<CpProprecursoDTO> ListarPropRecursoGams(int orden, int topcodi, int catcodi)
        {
            return FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecursoToGams(orden, catcodi.ToString(), topcodi, 1).ToList();
        }

        /// <summary>
        /// Graba id gams
        /// </summary>
        /// <param name="Recurcodi"></param>
        /// <param name="idGams"></param>
        /// <param name="catcodi"></param>
        public void SaveRecursoGams(int recurcodi, int idGams, int catcodi, int catcodi2, bool? ficticio = null)
        {
            CpRecursoGamsDTO recGams = new CpRecursoGamsDTO();
            recGams.RecursoID = recurcodi;
            recGams.GamsID = idGams;
            recGams.CategoriaID = catcodi;
            MapeoGamsEscenario.AddItem(idGams, recurcodi, catcodi, catcodi2, ficticio);
        }

        /// <summary>
        /// Lista restricciones para Gams
        /// </summary>
        /// <param name="lrest"></param>
        /// <param name="subrestricodi"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> ListaRestricionesGams(List<CpMedicion48DTO> lrest, short subrestricodi)
        {
            List<CpMedicion48DTO> lista = new List<CpMedicion48DTO>();
            lista = lrest.Where(x => x.Srestcodi == subrestricodi).ToList();

            return lista;
        }

        /// <summary>
        /// Lista propiedades de recurso para Gams
        /// </summary>
        /// <param name="recurcodi"></param>
        /// <param name="catcodi"></param>
        /// <returns></returns>
        public List<CpProprecursoDTO> ListarPropiedadRecursoGams(int recurcodi, int catcodi)
        {
            List<CpProprecursoDTO> lista = new List<CpProprecursoDTO>();
            lista = _propiedades.Where(x => x.Recurcodi == recurcodi).ToList();
            return lista;
        }

        /// <summary>
        /// Obtiene lista de propiedades por categoria
        /// </summary>
        /// <param name="pCategoria"></param>
        /// <returns></returns>
        public List<CpPropiedadDTO> ObtenerListaPropiedades(short pCategoria)
        {
            List<CpPropiedadDTO> entitys = new List<CpPropiedadDTO>();
            entitys = FactorySic.GetCpPropiedadRepository().GetByCriteria(pCategoria);
            return entitys;
        }
        /// <summary>
        /// Lista Recursos en Subrestriccion
        /// </summary>
        /// <param name="pTopologia"></param>
        /// <param name="sRestriccion"></param>
        /// <param name="catcodi"></param>
        /// <returns></returns>
        public List<CpSubrestricdatDTO> ListarRecursosEnSubRestriccion(int pTopologia, int sRestriccion, short catcodi)
        {
            List<CpSubrestricdatDTO> entitys = new List<CpSubrestricdatDTO>();
            entitys = FactorySic.GetCpSubrestricdatRepository().ListarRecursosEnSubRestriccion(pTopologia, sRestriccion);
            return entitys;
        }

        /// <summary>
        /// Lista recurso por tipo de recurso
        /// </summary>
        /// <param name="pTipoRecurso"></param>
        /// <param name="pTopologia"></param>
        /// <returns></returns>
        public List<CpRecursoDTO> ListarRecurso(int pTipoRecurso, int pTopologia) // pRutaRepositorio
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            entitys = FactorySic.GetCpRecursoRepository().ListaRecursoXCategoria(pTipoRecurso, (int)pTopologia);
            var propiedades = FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecurso2(-1, pTipoRecurso.ToString(), (int)pTopologia, -1).ToList();
            CpProprecursoDTO find;
            foreach (var reg in entitys)
            {
                reg.ConsideraEquipo = reg.Recurconsideragams.ToString();
                switch (reg.Catcodi)
                {
                    case ConstantesBase.Urs:
                        int resultado;
                        find = propiedades.Find(x => x.Proporden == ConstantesBase.OrdenProvisionBaseUp && x.Recurcodi == reg.Recurcodi);
                        if (find != null)
                        {
                            if (int.TryParse(find.Valor, out resultado))
                                reg.ProvisionBaseUp = resultado;
                        }
                        find = propiedades.Find(x => x.Proporden == ConstantesBase.OrdenProvisionBaseDn && x.Recurcodi == reg.Recurcodi);
                        if (find != null)
                        {
                            if (int.TryParse(find.Valor, out resultado))
                                reg.ProvisionBaseDn = resultado;
                        }
                        break;
                    case ConstantesBase.ModoT:
                        find = propiedades.Find(x => x.Proporden == ConstantesBase.CombustibleOrden && x.Recurcodi == reg.Recurcodi);
                        if (find != null)
                        {
                            if (int.TryParse(find.Valor, out resultado))
                                reg.Combcodi = resultado;
                        }
                        find = propiedades.Find(x => x.Proporden == ConstantesBase.FuenteOrden && x.Recurcodi == reg.Recurcodi);
                        if (find != null)
                        {
                            if (int.TryParse(find.Valor, out resultado))
                                reg.Fuentecodi = resultado;
                        }
                        break;
                    case ConstantesBase.Caldero:
                        find = propiedades.Find(x => x.Proporden == 1 && x.Recurcodi == reg.Recurcodi);
                        if (find != null)
                        {
                            if (int.TryParse(find.Valor, out resultado))
                                reg.TiempoES = resultado;
                        }
                        find = propiedades.Find(x => x.Proporden == 2 && x.Recurcodi == reg.Recurcodi);
                        if (find != null)
                        {
                            if (int.TryParse(find.Valor, out resultado))
                                reg.TiempoFS = resultado;
                        }
                        break;
                }
            }
            return entitys;
        }

        /// <summary>
        /// Lista de Urs por RSF
        /// </summary>
        /// <param name="idRsF"></param>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public List<CpRecursoDTO> ListarURSporRSF(int idRsF, int topcodi)
        {
            List<CpRecursoDTO> lista = ListarRecurso(ConstantesBase.Urs, topcodi).OrderBy(x => x.Recurcodi).ToList();
            var listaRsfUrsf = ListarGrupoRecursoFamiliaGams(topcodi, ConstantesBase.Rsf, false);
            var resultado = lista.Where(x => listaRsfUrsf.Any(y => y.Recurcodisicoes == x.Recurcodi)).ToList();
            return resultado;
        }

        /// <summary>
        /// Encuentra el codigo Gams del recurso
        /// </summary>
        /// <param name="recurcodi"></param>
        /// <param name="catcodi"></param>
        /// <returns></returns>
        public int FindIdGams(int recurcodi, int catcodi)
        {
            int result = 0;
            result = MapeoGamsEscenario.FindIdGams(recurcodi, catcodi);
            return result;
        }

        /// <summary>
        /// Busca un nodo Ficticio
        /// </summary>
        /// <param name="nodoT"></param>
        /// <returns></returns>
        public int BuscarEquipoFicticio(int nodoT)
        {
            return ReduccionBarra.FindEquipo(nodoT);

        }
        /// <summary>
        /// Lista Lineas para Gams
        /// </summary>
        /// <param name="tipoRecurso"></param>
        /// <param name="pTopcodi"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        public List<CpRecursoDTO> ListarLineaGams(int tipoRecurso, int pTopcodi)
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            entitys = FactorySic.GetCpRecursoRepository().ListarLinea(tipoRecurso, pTopcodi).Where(x => x.Recurconsideragams == 1).ToList();
            var lista = entitys.Where(x => x.Recurconsideragams.ToString() == ConstantesBase.ConsideraEscenario).ToList();
            return lista;
        }
        /// <summary>
        /// / Crae archivo de descripcion de nodos topologicos
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        /// <param name="categoria"></param>
        private void CrearDescEquipo(string ruta, int topcodi, int tipoEquipo, string prefijo, string nombreArchivo)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            int idGams = 0;
            int idEquipo = 0;
            List<CruceIdGamsCoes> listaGams = new List<CruceIdGamsCoes>();
            List<CruceIdGamsCoes> listaCopia = new List<CruceIdGamsCoes>();

            List<CpRecursoDTO> lista = new List<CpRecursoDTO>();
            List<int> listOrdenCol = new List<int>();
            sLine += UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("Equipo");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = string.Empty;

            if (tipoEquipo == ConstantesBase.Linea)
            {
                lista = ListarLineaGams(ConstantesBase.Linea, topcodi).OrderBy(x => x.Recurcodi).ToList();
                var listat2 = ListarLineaGams(ConstantesBase.Trafo2D, topcodi).ToList();
                int totTrafos2D = listat2.Count;
                var listat3 = ListarLineaGams(ConstantesBase.Trafo3D, topcodi).ToList();
                lista = lista.Union(listat2).Concat(listat3).ToList();
                listaGams = MapeoGamsEscenario.ListaMapeo.Where(x => x.Tipo == ConstantesBase.Linea).ToList();
                listaCopia = new List<CruceIdGamsCoes>();
                CruceIdGamsCoes reg;
                foreach (var r in listaGams)
                {
                    reg = new CruceIdGamsCoes();
                    reg.IdEquipo = r.IdEquipo;
                    reg.IdGams = r.IdGams;
                    reg.Tipo = r.Tipo2;
                    reg.Tipo2 = r.Tipo2;
                    reg.Ficticio = r.Ficticio;
                    listaCopia.Add(reg);
                }
            }
            else
            {
                lista = ListarRecursosPorCategoriaGams(_lrecurso, tipoEquipo);
                if (MapeoGamsEscenario.ListaMapeo != null)
                {
                    listaGams = MapeoGamsEscenario.ListaMapeo.Where(x => x.Tipo == tipoEquipo).ToList();
                    listaCopia = new List<CruceIdGamsCoes>();
                    listaCopia = listaGams;
                }
            }
            foreach (var reg in listaCopia)
            {
                if (reg.Ficticio != true)
                {
                    var find = lista.Find(x => x.Recurcodi == reg.IdEquipo && x.Catcodi == reg.Tipo);
                    if (find != null)
                    {
                        idEquipo = find.Recurcodi;
                        idGams = reg.IdGams;
                        sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                        sLine += find.Recurnombre;
                        UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                        sLine = string.Empty;
                    }
                }
                else
                {
                    var find = ReduccionBarra.ListaRecursoReduccion.Find(x => x.RecurReduccion == reg.IdEquipo);
                    if (find != null)
                    {
                        idEquipo = reg.IdEquipo;
                        idGams = reg.IdGams;
                        sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                        sLine += find.Recurnombre;
                        UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                        sLine = string.Empty;
                    }
                }
            }

            FileHelper.GenerarArchivo(nombreArchivo, ruta, textoArchivo);
        }

        /// <summary>
        /// Genera Nodo Ficcticio
        /// </summary>
        /// <param name="topcodi"></param>
        private void GeneraNodoTFicticio(int topcodi)
        {
            string scategoria = ConstantesBase.Linea.ToString() + "," + ConstantesBase.Trafo2D + "," + ConstantesBase.Trafo3D;

            List<CpProprecursoDTO> propLineas = FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecursoToGams(ConstantesBase.ReduccionOrden, ConstantesBase.Linea.ToString(), topcodi, 1);
            List<CpProprecursoDTO> propTrafos2d = FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecursoToGams(ConstantesBase.ReduccionOrden, ConstantesBase.Trafo2D.ToString(), topcodi, 1);
            List<CpProprecursoDTO> propTrafos3d = FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecursoToGams(ConstantesBase.ReduccionOrden, ConstantesBase.Trafo3D.ToString(), topcodi, 1);
            List<CpProprecursoDTO> propNodot = FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecursoToGams(ConstantesBase.TensionOrden, ConstantesBase.NodoTopologico.ToString(), topcodi, 1);
            var listap = propLineas.Union(propTrafos2d).Concat(propTrafos3d).Where(x => x.Valor == "1");
            var tension = "";

            foreach (var reg in listap)
            {
                //encontrar nodos topologicos
                int barra1 = ObtenerNodoTopologico(reg.Recurcodi, ConstantesBase.TerLinDestino, topcodi);
                var regbarra1 = FactorySic.GetCpRecursoRepository().GetById(barra1, topcodi);
                var prop = ListarPropiedadRecurso(topcodi, barra1, ConstantesBase.NodoTopologico, string.Empty);
                var find2 = prop.Find(x => x.Proporden == ConstantesBase.TensionOrden);
                if (find2 != null)
                {
                    tension = find2.Valor;
                }
                var nodoT = FactorySic.GetCpRecursoRepository().GetById(barra1, topcodi);
                int barra2 = ObtenerNodoTopologico(reg.Recurcodi, ConstantesBase.TerLinOrigen, topcodi);
                var regbarra2 = FactorySic.GetCpRecursoRepository().GetById(barra2, topcodi);
                SaveRecursoReduccion(barra1, barra2, tension, regbarra1.Recurnombre + " / " + regbarra2.Recurnombre);
            }
        }

        /// <summary>
        /// Permite grabar una barra de reduccion
        /// </summary>
        /// <param name="recurdcodi1"></param>
        /// <param name="recurcodi2"></param>
        /// <param name="tension"></param>
        /// <param name="recurnombre"></param>
        public void SaveRecursoReduccion(int recurdcodi1, int recurcodi2, string tension, string recurnombre)
        {
            ReduccionBarra.AddItem(recurdcodi1, recurcodi2, recurnombre, tension);
        }

        /// <summary>
        /// Obtiene Propiedades de Recurso
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="recurcodi"></param>
        /// <param name="catcodi"></param>
        /// <param name="modo"></param>
        /// <param name="sqlSoloManual"></param>
        /// <returns></returns>
        public List<CpProprecursoDTO> ListarPropiedadRecurso(int topcodi, int recurcodi, int catcodi, string sqlSoloManual)
        {
            List<CpProprecursoDTO> lista = new List<CpProprecursoDTO>();
            lista = FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecurso(recurcodi, topcodi, sqlSoloManual).ToList();
            return lista;
        }

        /// <summary>
        /// Obtiene Terminal del Nodo Topologico
        /// </summary>
        /// <param name="recurcodi"></param>
        /// <param name="ttermcodi"></param>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public int ObtenerNodoTopologico(int recurcodi, int ttermcodi, int topcodi)
        {

            return FactorySic.GetCpTerminalRepository().ObtenerNodoTopologico(recurcodi, ttermcodi, topcodi);

        }

        /// <summary>
        /// Obtiene etapa inicial y etapa final
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="diaIni"></param>
        /// <param name="totDias"></param>
        /// <param name="etpFinal"></param>
        /// <returns></returns>
        private int ObtenerEtapaInicialFinal(List<CpDetalleetapaDTO> lista, int diaIni, int horaInicio, int totDias, ref int etpFinal)
        {
            int etapaIni = 1;
            decimal? acumulado = 0;
            if (horaInicio == -1)
                horaInicio = 1;
            int acumHora = (horaInicio - 1);
            for (int i = 0; i < lista.Count; i++)
            {
                for (int j = (int)lista[i].Etpini; j <= lista[i].Etpfin; j++)
                {
                    acumulado += lista[i].Etpdelta * 2;
                    if (acumulado == (diaIni * 48 + acumHora))
                    {
                        etapaIni = j + 1;
                    }
                    if ((acumulado - diaIni * 48) == totDias * 48)
                    {
                        etpFinal = j;
                        break;
                    }
                }
            }
            return etapaIni;
        }

        /// <summary>
        /// Consigue lista de etapas a partir de lista de periodos
        /// </summary>
        /// <param name="listaPeriodo"></param>
        /// <returns></returns>
        public List<EtapaDTO> GetListaEtapa(List<CpDetalleetapaDTO> listaPeriodo, int iniEtapa, int finEtapa)
        {
            List<EtapaDTO> lista = new List<EtapaDTO>();
            EtapaDTO etapa;
            int? nBloques = listaPeriodo.Max(x => x.Etpbloque);
            decimal? acumulado = 0;
            int item = 1;
            for (var nB = 1; nB <= nBloques; nB++)
            {
                var p = listaPeriodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                for (var i = p.Etpini; i <= p.Etpfin; i++)
                {
                    if (iniEtapa <= i && finEtapa >= i)
                    //if (finEtapa >= i)
                    {
                        acumulado += p.Etpdelta;
                        etapa = new EtapaDTO();
                        etapa.Etpcodi = i;// item;
                        item++;
                        etapa.Etpdelta = (decimal)p.Etpdelta;
                        etapa.Etpacumulado = (decimal)acumulado;
                        lista.Add(etapa);
                    }
                }
            }
            return lista;
        }

        #endregion

        #region Archivos Gams

        /// <summary>
        /// Crea los archivos de entreda para Gams
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="diaInicio"></param>
        /// <param name="horaInicio"></param>
        /// <param name="totDias"></param>
        public void CrearArchivoGams(string ruta, int topcodi, int diaInicio, int horaInicio, int totDias, InsumoYupanaContinuo objInsumo, int numProcesadores)
        {
            int topsinrsf = 0;
            int iniEtapa = 1;
            int finEtapa = 1;
            _escenario = GetTopologia(topcodi);
            if (_escenario != null)
                topsinrsf = _escenario.Topsinrsf;
            _listaPropiedades = ObtenerListaPropiedadesTotal();
            _propiedades = FactorySic.GetCpProprecursoRepository().ListarPropiedadxRecurso2(-1, "-1", topcodi, -1).ToList();
            _lrecurso = FactorySic.GetCpRecursoRepository().ListarRecursoPorTopologia(topcodi);
            _lsrestriccion48 = FactorySic.GetCpMedicion48Repository().ListaSubRestriccionGams(topcodi);
            _lsubrestricdat = FactorySic.GetCpSubrestricdatRepository().List(topcodi);

            _lperiodo = FactorySic.GetCpDetalleetapaRepository().List(topcodi);
            iniEtapa = ObtenerEtapaInicialFinal(_lperiodo, diaInicio, horaInicio, totDias, ref finEtapa);
            var listaPeriodoDetalle = GetListaEtapa(_lperiodo, iniEtapa, finEtapa);

            MapeoGamsEscenario.Inicio();
            ReduccionBarra.Inicio();
            GeneraNodoTFicticio(topcodi);
            List<CpCategoriaDTO> listaCategoria;
            listaCategoria = FactorySic.GetCpRecursoRepository().ListaCategoria();
            CpCategoriaDTO regCategoria;
            CrearGamsPeriodo(ruta, topcodi, iniEtapa, finEtapa);
            //CrearGamsParametros(ruta, topcodi);
            CrearGamsConfiguraciones(ruta, topcodi, numProcesadores);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.ModoT);
            CrearGamsPTermica(ruta, topcodi, regCategoria, iniEtapa, finEtapa);
            CrearDescEquipo(ruta, topcodi, ConstantesBase.ModoT, ConstantesYupana.PrefijoPlantaT, ConstantesBase.NombArchivoPlantaTermicaDescrip);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.NodoTopologico);
            CrearGamsNodoTopologico(ruta, topcodi, regCategoria);
            CrearDescEquipo(ruta, topcodi, ConstantesBase.NodoTopologico, ConstantesYupana.PrefijoNodoTopologico, ConstantesBase.NombArchivoNodoTopDescrip);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.PlantaH);
            CrearGamsPlantasHidro(ruta, topcodi, regCategoria);
            CrearDescEquipo(ruta, topcodi, ConstantesBase.PlantaH, ConstantesYupana.PrefijoPlantaH, ConstantesBase.NombArchivoPlantaHidroDescrip);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.Embalse);
            CrearGamsEmbalse(ruta, topcodi, regCategoria);
            CrearDescEquipo(ruta, topcodi, ConstantesBase.Embalse, ConstantesYupana.PrefijoEmbalse, ConstantesBase.NombArchivoEmbalseDescrip);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.PlantaNoConvenO);
            CrearGamsPRer(ruta, topcodi, regCategoria, iniEtapa, finEtapa);
            CrearGamsPRerYupanaContinuo(ruta, objInsumo.ListaProyRer, topcodi, regCategoria, iniEtapa, finEtapa); //Crear rer para Yupana continuo
            CrearDescEquipo(ruta, topcodi, ConstantesBase.PlantaNoConvenO, ConstantesYupana.PrefijoPlantaRer, ConstantesBase.NombArchivoPlantaRerDescrip);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.Linea);
            CrearGamsLineas(ruta, topcodi, iniEtapa, finEtapa, regCategoria);
            CrearDescEquipo(ruta, topcodi, ConstantesBase.Linea, ConstantesYupana.PrefijoLinea, ConstantesBase.NombArchivoLineaDescrip);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.Urs);
            CrearGamsURS(ruta, topcodi, regCategoria);
            CrearDescEquipo(ruta, topcodi, ConstantesBase.Urs, ConstantesYupana.PrefijoURS, ConstantesBase.NombArchivoPlantaUrsDescrip);
            CrearProvisionUrs(ruta, topcodi, topsinrsf, iniEtapa, finEtapa);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.Rsf);
            CrearGamsRSF(ruta, topcodi, regCategoria, iniEtapa, finEtapa, topsinrsf);
            CrearGamsArranques(ruta, topcodi);
            CrearGamsParadas(ruta, topcodi);
            CrearGamsCondIniciales(ruta, topcodi);
            CrearGamsDemanda(ruta, topcodi, iniEtapa, finEtapa);
            CrearGamsCaudales(ruta, topcodi, iniEtapa, finEtapa);

            CrearGamsCaudalesYupanaContinuo(ruta, ConstantesBase.NombArchivoCaudalesCc, objInsumo.ListaAportesCC, topcodi, iniEtapa, finEtapa);// Con Compromiso
            CrearGamsCaudalesYupanaContinuo(ruta, ConstantesBase.NombArchivoCaudalesSc, objInsumo.ListaAportesSC, topcodi, iniEtapa, finEtapa);// Sin Compromiso
            CrearGamsCaudalesYupanaContinuo(ruta, ConstantesBase.NombArchivoCaudalesCcSc, objInsumo.ListaAportesCCSC, topcodi, iniEtapa, finEtapa);// Sin Compromiso

            CrearGamsMantenimiento(ruta, topcodi, iniEtapa, finEtapa);
            CrearGamsReserva(ruta, topcodi, iniEtapa, finEtapa);
            CrearGamsVolMeta(ruta, topcodi);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.GenerMeta);
            CrearGamsGenerMeta(ruta, topcodi, regCategoria, iniEtapa, finEtapa, listaPeriodoDetalle);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.RestricGener);
            CrearGamsRestricGener(ruta, topcodi, regCategoria, iniEtapa, finEtapa);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.SumFlujo);
            CrearGamsSumFlujos(ruta, topcodi, regCategoria, iniEtapa, finEtapa);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.DispComb);
            CrearGamsDispComb(ruta, topcodi, regCategoria, iniEtapa, finEtapa, listaPeriodoDetalle);
            CrearGamsFCostoF(ruta, topcodi);
            CrearGamsVolumen(ruta, topcodi, iniEtapa, finEtapa);
            ////////////////////////////// Res Ter Dn //////////////////////////////////
            CrearGamsReservaUrs(ruta, topcodi, iniEtapa, finEtapa, topsinrsf);
            CrearGamsCostURS(ruta, topcodi, iniEtapa, finEtapa, topsinrsf);
            CrearGamsForazada(ruta, topcodi, iniEtapa, finEtapa);
            CrearGamsForzadaYupanaContinuo(ruta, topcodi, objInsumo.ListaCondTermicas, iniEtapa, finEtapa);
            CrearGamsTemper(ruta, topcodi, iniEtapa, finEtapa);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.GrupoPrioridad);
            CrearGamsGrupoprioridad(topcodi, regCategoria);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.CicloCombinado);
            CrearGamsCicloCombinado(topcodi, regCategoria);
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.RegionSeguridad);
            CrearGamsRegionSeguridad(ruta, topcodi, regCategoria, iniEtapa, finEtapa, listaPeriodoDetalle);
            CrearMetodoGams(ruta, topcodi);
            CrearGamsTiempoViaje(ruta, topcodi);
            CrearGamsTipoReserva(ruta, topcodi, topsinrsf, iniEtapa, finEtapa);
            //Iteracion 3 Yupana
            CrearGamsUnidIndisponibles(ruta, topcodi, iniEtapa, finEtapa);
            CrearPerdidasUninodal(ruta, topcodi, iniEtapa, finEtapa);
            //Fin Iteracion 3 Yupana

            //Yupana 2022
            CrearTiempoTransicion(ruta, topcodi);
            ////////////////////////////// Calderos //////////////////////////////////////////
            regCategoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.Caldero);
            CrearGamsCaldero(ruta, topcodi, regCategoria);
            //Fin Yupana 2022
            CrearGamsSets(ruta, topcodi, listaCategoria, iniEtapa, listaPeriodoDetalle);

            return;
        }

        #region Iteracion 3 Yupana
        /// <summary>
        /// Lista las plantas de todas las URS
        /// </summary>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public List<CpGruporecursoDTO> ListarPlantasUrsAll(int topcodi)
        {
            decimal resultado;
            int idMinimo, idMaximo;
            var listaPlantas = FactorySic.GetCpGruporecursoRepository().GetByCriteriaFamilia(ConstantesBase.Urs, topcodi);

            foreach (var reg in listaPlantas)
            {
                var propiedades = ListarPropiedadRecurso(topcodi, reg.Recurcodisicoes, reg.Catcodisec, ConstantesBase.SqlSoloPropManual).ToList();
                if (reg.Catcodisec == ConstantesBase.PlantaH)
                {
                    idMinimo = ConstantesBase.MinimoPh;
                    idMaximo = ConstantesBase.MaximoPh;
                }
                else
                {
                    idMinimo = ConstantesBase.MinimoPt;
                    idMaximo = ConstantesBase.MaximoPt;
                }

                var find = propiedades.Find(x => x.Propcodi == idMaximo);
                if (find != null)
                {
                    if (decimal.TryParse(find.Valor, out resultado))
                        reg.Plantamax = resultado;
                }
                find = propiedades.Find(x => x.Propcodi == idMinimo);
                if (find != null)
                {
                    if (decimal.TryParse(find.Valor, out resultado))
                        reg.Plantamin = resultado;
                }

            }
            return listaPlantas;
        }

        /// <summary>
        /// Crea Archivo Gams Perdidas Uninodal
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        private void CrearPerdidasUninodal(string ruta, int topcodi, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            int indice;
            string sLine = string.Empty;
            decimal? acumulado = 0;
            var lista = ListaRestricionesGams(_lsrestriccion48, ConstantesBase.SresPerdModUninodal);
            int? nBloques = _lperiodo.Max(x => x.Etpbloque);
            UtilYupana.AgregaLinea(ref textoArchivo, "PARAMETER Perdidas_uninodal(t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            int c = 0;
            if (nBloques >= 1)
            {
                for (var nB = 1; nB <= nBloques; nB++)
                {
                    var p = _lperiodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                    for (var i = p.Etpini; i <= p.Etpfin; i++)
                    {
                        acumulado += p.Etpdelta * 2;
                        if (i >= iniEtapa && i <= finEtapa)
                        {
                            c++;
                            indice = (int)acumulado % 48;
                            if (indice == 0) indice = 48;
                            sLine = UtilYupana.WriteArchivoGams((i - iniEtapa + 1).ToString());
                            int dia = (int)((acumulado - 1) / 48);
                            DateTime fecha = _escenario.Topfecha.AddDays(dia);
                            var registro = lista.Find(x => x.Recurcodi == 0 && x.Medifecha == fecha);
                            if (registro != null)
                            {
                                decimal total = 0;
                                decimal nDatos = 0;
                                for (int z = 0; z < p.Etpdelta * 2; z++)
                                {
                                    if ((indice - z) > 0)
                                    {
                                        decimal? valor = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro, null);
                                        if (valor != null)
                                        {
                                            total += (decimal)valor;
                                            nDatos = nDatos + 1;
                                        }
                                    }
                                }
                                if (nDatos > 0)
                                    sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)(total / nDatos), ConstantesBase.NroDecimalGams).ToString());
                                else
                                    sLine += UtilYupana.WriteArchivoGams("");
                            }

                            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                            sLine = string.Empty;
                        }
                    }
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoPerdidasModeloUninodal, ruta, textoArchivo);
        }
        /// Crea el archivo Reserva.dat
        /// </summary>
        /// <param name="nperiodo"></param>
        private void CrearGamsUnidIndisponibles(string ruta, int topcodi, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;

            //Indisponibildad Up RSF Plantas Termicas
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE IndispRSFUpt(Ut,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = UtilYupana.WriteArchivoGams("");
            var listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.ModoT);
            var listaUrs = ListarPlantasUrsAll(topcodi);
            listaEquipo = listaEquipo.Where(x => listaUrs.Any(y => y.Recurcodisicoes == x.Recurcodi && y.Catcodisec == x.Catcodi)).ToList();
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresIndRsfPtUp, (int)topcodi, ConstantesYupana.PrefijoPlantaT, listaEquipo, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            //Indisponibildad Down RSF Plantas Termicas
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE IndispRSFDnt(Ut,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresIndRsfPtDown, (int)topcodi, ConstantesYupana.PrefijoPlantaT, listaEquipo, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "");

            //Indisponibildad Up RSF Planta Hidro
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE IndispRSFUph(Uh,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);

            sLine = UtilYupana.WriteArchivoGams("");
            listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.PlantaH);
            listaEquipo = listaEquipo.Where(x => listaUrs.Any(y => y.Recurcodisicoes == x.Recurcodi && y.Catcodisec == x.Catcodi)).ToList();
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresIndRsfPhUp, (int)topcodi, ConstantesYupana.PrefijoPlantaH, listaEquipo, iniEtapa, finEtapa);

            //Indisponibildad Down RSF Planta Hidro
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE IndispRSFDnh(Uh,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);

            sLine = UtilYupana.WriteArchivoGams("");
            listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.PlantaH);
            listaEquipo = listaEquipo.Where(x => listaUrs.Any(y => y.Recurcodisicoes == x.Recurcodi && y.Catcodisec == x.Catcodi)).ToList();
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresIndRsfPhDown, (int)topcodi, ConstantesYupana.PrefijoPlantaH, listaEquipo, iniEtapa, finEtapa);

            FileHelper.GenerarArchivo(ConstantesBase.NombreArchivoUnidadesIndisponibles, ruta, textoArchivo);
        }

        #endregion

        #region Adicional Yupana 2022
        /// <summary>
        /// Crea archivo Tiempo de Transicion
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        private void CrearTiempoTransicion(string ruta, int topcodi)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            int idmodoSup, idmodoInf;
            var listaM = ListCpRelacion(ConstantesBase.Transicion.ToString(), topcodi);
            UtilYupana.AgregaLinea(ref textoArchivo, "Table TiemTransicion(Ut,Ut, *)");
            sLine = UtilYupana.WriteArchivoGams("");
            sLine += UtilYupana.WriteArchivoGams("TminO");
            sLine += UtilYupana.WriteArchivoGams("TminFS");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var p in listaM)
            {
                idmodoInf = FindIdGams(p.Recurcodi1, ConstantesBase.ModoT);
                idmodoSup = FindIdGams(p.Recurcodi2, ConstantesBase.ModoT);
                if (idmodoInf != 0 && idmodoSup != 0)
                {
                    sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaT + idmodoInf.ToString() + "." + ConstantesYupana.PrefijoPlantaT + idmodoSup.ToString());
                    sLine += UtilYupana.WriteArchivoGams(p.Cprelval1.ToString());
                    sLine += UtilYupana.WriteArchivoGams(p.Cprelval2.ToString());
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoTiempoTransicion, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo teimpotransicion
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        private void CrearGamsCaldero(string ruta, int topcodi, CpCategoriaDTO categoria)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            string sLine = string.Empty;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            if (categoria != null)
                categoria.Total = lista.Count;

            var listaM = ListCpRelacion(ConstantesBase.Transicion.ToString(), topcodi);

            UtilYupana.AgregaLinea(ref textoArchivo, "Table TiemCald(Cal, *)");
            sLine = UtilYupana.WriteArchivoGams("");
            sLine += UtilYupana.WriteArchivoGams("TminO");
            sLine += UtilYupana.WriteArchivoGams("TminFS");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var p in lista)
            {
                idGams++;
                sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoCaldero + idGams.ToString());
                SaveRecursoGams(p.Recurcodi, idGams, ConstantesBase.Caldero, 0);
                sLine += UtilYupana.WriteArchivoGams(p.TiempoES.ToString());
                sLine += UtilYupana.WriteArchivoGams(p.TiempoFS.ToString());
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoTiempoCaldero, ruta, textoArchivo);
        }
        #endregion

        /// <summary>
        /// Crear archivo dat Tipo de Reserva
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="topsinrsf"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        private void CrearGamsTipoReserva(string ruta, int topcodi, int topsinrsf, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            int indice;
            decimal? acumulado = 0;
            string sLine = string.Empty;
            string valor = string.Empty;
            List<CpMedicion48DTO> lista = new List<CpMedicion48DTO>();
            if (topsinrsf == 0)
                lista = ListaRestricionesGams(_lsrestriccion48, ConstantesBase.SresTipoReserva);
            var listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.Rsf);
            int? nBloques = _lperiodo.Max(x => x.Etpbloque);

            UtilYupana.AgregaLinea(ref textoArchivo, "PARAMETER RSFSimetrica_Activar(t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "/");

            int c = 0;
            if (nBloques >= 1)
            {
                for (var nB = 1; nB <= nBloques; nB++)
                {
                    var p = _lperiodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                    for (var i = p.Etpini; i <= p.Etpfin; i++)
                    {
                        acumulado += p.Etpdelta * 2;
                        if (i >= iniEtapa && i <= finEtapa)
                        {
                            c++;
                            indice = (int)acumulado % 48;
                            if (indice == 0) indice = 48;
                            sLine = UtilYupana.WriteArchivoGams((i - iniEtapa + 1).ToString());
                            int dia = (int)((acumulado - 1) / 48);
                            DateTime fecha = _escenario.Topfecha.AddDays(dia);
                            foreach (var reg in listaEquipo)
                            {
                                var registro = lista.Find(x => x.Recurcodi == reg.Recurcodi && x.Medifecha == fecha);
                                if (registro != null)
                                {
                                    decimal? valor2 = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice)).ToString()).GetValue(registro, null);
                                    if (valor2 != null)
                                        sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)valor2, ConstantesBase.NroDecimalGams).ToString());
                                    else
                                        sLine += UtilYupana.WriteArchivoGams("");
                                }
                            }
                            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                            sLine = string.Empty;
                        }

                    }
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombreArchivoRsfTipo, ruta, textoArchivo);
        }


        /// <summary>
        /// Crear archivo dat Tienpo de viaje
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        private void CrearGamsTiempoViaje(string ruta, int topcodi)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            string valor = string.Empty;
            CpRelacionDTO findReg;
            List<CpRelacionDTO> listaTopEmb;
            int idGams = 0;
            int idHidGams = 0;
            List<string> datosTurbinaPhEmb = new List<string>();
            List<string> datosTurbinaPhPh = new List<string>();
            List<string> datosTurbinaEmbPh = new List<string>();
            List<string> datosTurbinaEmbEmb = new List<string>();
            List<string> datosVierteEmbPh = new List<string>();
            List<string> datosVierteEmbEmb = new List<string>();
            List<string> datosViertePhEmb = new List<string>();
            List<string> datosViertePhPh = new List<string>();

            List<CpRecursoDTO> lista;
            //Embalse
            lista = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.Embalse);
            var listaTpoHid = ListCpRelacion(ConstantesBase.Turbinamiento + "," + ConstantesBase.Vertimiento, topcodi).Where(x => x.Recurconsideragams == 1).ToList();
            foreach (var reg in lista)
            {
                idGams = FindIdGams(reg.Recurcodi, reg.Catcodi);
                listaTopEmb = listaTpoHid.Where(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.PlantaH).ToList();
                foreach (var p in listaTopEmb)
                {
                    idHidGams = FindIdGams(p.Recurcodi2, ConstantesBase.PlantaH);
                    if (idHidGams > 0)
                    {
                        sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoEmbalse + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoPlantaH + idHidGams.ToString());
                        sLine += UtilYupana.WriteArchivoGams(p.Cprelval1.ToString());
                        datosTurbinaEmbPh.Add(sLine);
                    }
                }

                listaTopEmb = listaTpoHid.Where(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.Embalse).ToList();
                foreach (var p in listaTopEmb)
                {
                    idHidGams = FindIdGams(p.Recurcodi2, ConstantesBase.Embalse);
                    if (idHidGams > 0)
                    {
                        sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoEmbalse + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoEmbalse + idHidGams.ToString());
                        sLine += UtilYupana.WriteArchivoGams(p.Cprelval1.ToString());
                        datosTurbinaEmbEmb.Add(sLine);
                    }
                }

                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.PlantaH);
                if (findReg != null)
                {
                    idHidGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.PlantaH);
                    if (idHidGams > 0)
                    {
                        sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoEmbalse + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoPlantaH + idHidGams.ToString());
                        sLine += UtilYupana.WriteArchivoGams(findReg.Cprelval1.ToString());
                        datosVierteEmbPh.Add(sLine);
                    }
                }

                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.Embalse);
                if (findReg != null)
                {
                    idHidGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.Embalse);
                    if (idHidGams > 0)
                    {
                        sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoEmbalse + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoEmbalse + idHidGams.ToString());
                        sLine += UtilYupana.WriteArchivoGams(findReg.Cprelval1.ToString());
                        datosVierteEmbEmb.Add(sLine);
                    }
                }

            }

            ///Planta
            var listphnt = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.PlantaH);
            listaTpoHid = ListCpRelacion(ConstantesBase.Turbinamiento + "," + ConstantesBase.Vertimiento, topcodi);
            foreach (var reg in listphnt)
            {
                idGams = FindIdGams(reg.Recurcodi, reg.Catcodi);
                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.PlantaH);
                if (findReg != null)
                {
                    int idTurbPHGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.PlantaH);
                    if (idTurbPHGams > 0)
                    {
                        sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaH + idGams.ToString() + "." + ConstantesYupana.PrefijoPlantaH + idTurbPHGams.ToString());
                        sLine += UtilYupana.WriteArchivoGams(findReg.Cprelval1.ToString());
                        datosTurbinaPhPh.Add(sLine);
                    }
                }

                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.PlantaH);
                if (findReg != null)
                {
                    int idViertePHGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.PlantaH);
                    if (idViertePHGams > 0)
                    {
                        sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaH + idGams.ToString() + "." + ConstantesYupana.PrefijoPlantaH + idViertePHGams.ToString());
                        sLine += UtilYupana.WriteArchivoGams(findReg.Cprelval1.ToString());
                        datosViertePhPh.Add(sLine);
                    }
                }

                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.Embalse);
                if (findReg != null)
                {
                    int idTurbEmbGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.Embalse);
                    if (idTurbEmbGams > 0)
                    {
                        sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaH + idGams.ToString() + "." + ConstantesYupana.PrefijoEmbalse + idTurbEmbGams.ToString());
                        sLine += UtilYupana.WriteArchivoGams(findReg.Cprelval1.ToString());
                        datosTurbinaPhEmb.Add(sLine);
                    }
                }

                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.Embalse);
                if (findReg != null)
                {
                    int idVierteEmbGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.Embalse);
                    if (idVierteEmbGams > 0)
                    {
                        sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaH + idGams.ToString() + "." + ConstantesYupana.PrefijoEmbalse + idVierteEmbGams.ToString());
                        sLine += UtilYupana.WriteArchivoGams(findReg.Cprelval1.ToString());
                        datosViertePhEmb.Add(sLine);
                    }
                }
            }

            UtilYupana.AgregaLinea(ref textoArchivo, "Table   TiemViajTurbinamPlantEmb(Uh,Emb, *)");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("DeltaT");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);

            foreach (var linea in datosTurbinaPhEmb)
                UtilYupana.AgregaLinea(ref textoArchivo, linea);
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            UtilYupana.AgregaLinea(ref textoArchivo, "Table   TiemViajTurbinamPlantPlant(Uh,Uh, *)");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("DeltaT");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var linea in datosTurbinaPhPh)
                UtilYupana.AgregaLinea(ref textoArchivo, linea);
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            UtilYupana.AgregaLinea(ref textoArchivo, "Table   TiemViajVertimientPlantEmb(Uh,Emb, *)");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("DeltaT");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var linea in datosViertePhEmb)
                UtilYupana.AgregaLinea(ref textoArchivo, linea);
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            UtilYupana.AgregaLinea(ref textoArchivo, "Table   TiemViajVertimientPlantPlant(Uh,Uh, *)");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("DeltaT");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var linea in datosViertePhPh)
                UtilYupana.AgregaLinea(ref textoArchivo, linea);
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            /////////////////////////////////////////
            UtilYupana.AgregaLinea(ref textoArchivo, "Table   TiemViajTurbinamEmbEmb(Emb,Emb, *)");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("DeltaT");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var linea in datosTurbinaEmbEmb)
                UtilYupana.AgregaLinea(ref textoArchivo, linea);
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            UtilYupana.AgregaLinea(ref textoArchivo, "Table   TiemViajTurbinamEmbPlant(Emb,Uh, *)");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("DeltaT");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var linea in datosTurbinaEmbPh)
                UtilYupana.AgregaLinea(ref textoArchivo, linea);
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            UtilYupana.AgregaLinea(ref textoArchivo, "Table   TiemViajVertimientEmbEmb(Emb,Emb, *)");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("DeltaT");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var linea in datosVierteEmbEmb)
                UtilYupana.AgregaLinea(ref textoArchivo, linea);
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            UtilYupana.AgregaLinea(ref textoArchivo, "Table   TiemViajVertimientEmbPlant(Emb,Uh, *)");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("DeltaT");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var linea in datosVierteEmbPh)
                UtilYupana.AgregaLinea(ref textoArchivo, linea);
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            FileHelper.GenerarArchivo(ConstantesBase.NombreArchivoTiempoViaje, ruta, textoArchivo);
        }

        /// <summary>
        /// Crear archivo dat Configuracion
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        private void CrearGamsConfiguraciones(string ruta, int topcodi, int numProcesador)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            string valor = string.Empty;
            var parametros = GetByCriteriaCpParametros((int)topcodi);

            UtilYupana.AgregaLinea(ref textoArchivo, "SCALAR");
            UtilYupana.AgregaLinea(ref textoArchivo, "Pi                                        /3.141592/");
            //
            valor = GetValorPropiedad(ConstantesBase.Generales, 0, ConstantesBase.TipoCambioDolar, DateTime.MaxValue, (int)topcodi);
            UtilYupana.AgregaLinea(ref textoArchivo, "TCom         Tipo de cambio                        /" + valor + "/");
            //
            valor = string.Empty;
            var find = parametros.Find(x => x.Paramcodi == ConstantesBase.CostoRacionamiento);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "Rac          Costo de potencia de racionamiento   /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.PenalidadesExceso);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "Rex          Costo de exceso de racionamiento     /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.CostoDeficitResrSecUp);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "RacRsfUp     Costo de deficit de RsfUp            /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.CostoDeficitResrSecDn);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "RacRsfDn     Costo de deficit de RsfDn            /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.CostoExcesoResrSecUp);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "RexRsfUp     Costo de exceso de RsfUp            /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.CostoExcesoResrSecDn);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "RexRsfDn     Costo de exceso de RsfDn            /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.CostoDeficitRegSeg);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "RacReg       Costo de deficit de Regiones de Seguridad  /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.CostoExcesoRegSeg);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "RexReg       Costo de exceso de Regiones de Seguridad  /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.PotenciaBase);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "Pbase        Potencia Base                             /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.ConstCaudalVol);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "Kh           constante caudal-volumen                 /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.ConsideraTViaje);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "TiempViaj    Considerar tiempo de viaje               /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.ConsideraEficiencia);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "Eficiencia    Considerar Eficiencia               /" + valor + "/");
            //
            valor = string.Empty;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.NumProcesadores);
            if (find != null)
                valor = find.Paramvalor;
            //obtener cantidad de procesadores a usar para toda la ejecución gams
            if (numProcesador > 0)
                valor = numProcesador.ToString();
            UtilYupana.AgregaLinea(ref textoArchivo, "Nucleos      Numero de procesadores                   /" + valor + "/");
            //
            valor = string.Empty;
            decimal resultado = 0;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.TolBenders);
            if (find != null)
            {
                valor = find.Paramvalor;
                decimal.TryParse(find.Paramvalor, out resultado);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "TolBenders   Tolerancia Benders en porcentaje         /" + (resultado / 100).ToString() + "/");
            //
            valor = string.Empty;
            resultado = 0;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.TolCplex);
            if (find != null)
            {
                valor = find.Paramvalor;
                decimal.TryParse(find.Paramvalor, out resultado);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "TolCplex     Tolerancia CPLEX en porcentaje           /" + (resultado / 100).ToString() + "/");
            //
            valor = string.Empty;
            resultado = 0;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.TolIPOPTsp);
            if (find != null)
            {
                valor = find.Paramvalor;
                decimal.TryParse(find.Paramvalor, out resultado);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "TolIPOPTsp   Tolerancia IPOPT de subproblemas en porcentaje /" + (resultado / 100).ToString() + "/");
            //
            valor = string.Empty;
            resultado = 0;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.TolSNOPTsp);
            if (find != null)
            {
                valor = find.Paramvalor;
                decimal.TryParse(find.Paramvalor, out resultado);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "TolSNOPTsp   Tolerancia SNOPT de subproblemas en porcentaje /" + (resultado / 100).ToString() + "/");
            //
            valor = string.Empty;
            resultado = 0;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.TolIPOPTfo);
            if (find != null)
            {
                valor = find.Paramvalor;
                decimal.TryParse(find.Paramvalor, out resultado);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "TolIPOPTfo   Tolerancia IPOPT de flujo optimo en porcentaje /" + (resultado / 100).ToString() + "/");
            //
            valor = string.Empty;
            resultado = 0;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.TolSNOPTfo);
            if (find != null)
            {
                valor = find.Paramvalor;
                decimal.TryParse(find.Paramvalor, out resultado);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "TolSNOPTfo   Tolerancia SNOPT de flujo optimo en porcentaje /" + (resultado / 100).ToString() + "/");
            //
            valor = string.Empty;
            resultado = 0;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.MaxItPrRel);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "MaxItPrRel   Maximo de iteraciones de prob. relajado        /" + valor + "/");
            //
            valor = string.Empty;
            resultado = 0;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.MaxItBend);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "MaxItBend   Maximo de iteraciones de Benders algoritmo      /" + valor + "/");
            //
            valor = string.Empty;
            resultado = 0;
            find = parametros.Find(x => x.Paramcodi == ConstantesBase.NivelCorte);
            if (find != null)
                valor = find.Paramvalor;
            UtilYupana.AgregaLinea(ref textoArchivo, "NivelCorte   Eliminar los cortes lejanos                    /" + valor + "/");
            //
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoConfiguraciones, ruta, textoArchivo);

            var find2 = parametros.Find(x => x.Paramcodi == ConstantesBase.ToleranciaRelativa);
            if (find2 != null)
            {
                decimal resultado2 = 0;
                if (decimal.TryParse(find2.Paramvalor, out resultado2))
                {
                    UtilYupana.AgregaLinea(ref textoArchivo, "SCALAR");
                    UtilYupana.AgregaLinea(ref textoArchivo, "Tolerancia /" + (resultado2 / 100).ToString() + "/");
                    UtilYupana.AgregaLinea(ref textoArchivo, ";");
                }

            }


            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoTolerancia, ruta, textoArchivo);

        }
        /// <summary>
        /// Crea el archivo periodo.dat
        /// </summary>
        private void CrearGamsPeriodo(string ruta, int topcodi, int etpIni, int etpFin)
        {
            string sLine = string.Empty;
            string textoArchivo = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "SETS");
            List<CpDetalleetapaDTO> lista = ListCpDetalleetapas(topcodi);
            int? indice = lista.Max(x => x.Etpbloque);
            int? netapas = 0;
            netapas = etpFin - etpIni + 1;
            UtilYupana.AgregaLinea(ref textoArchivo, "t horizonte de tiempo /1* " + netapas.ToString() + "/;");
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE DIVISION(t,*)");
            sLine += UtilYupana.WriteArchivoGams("");
            sLine += UtilYupana.WriteArchivoGams("DeltaT");
            sLine += UtilYupana.WriteArchivoGams("Acumul");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = string.Empty;
            decimal? acumulado = 0;
            foreach (var reg in lista)
            {
                for (var i = reg.Etpini; i <= reg.Etpfin; i++)
                {
                    if (i >= etpIni && i <= etpFin)
                    {
                        acumulado += reg.Etpdelta;
                        sLine += UtilYupana.WriteArchivoGams((i - etpIni + 1).ToString());
                        sLine += UtilYupana.WriteArchivoGams(reg.Etpdelta.ToString());
                        sLine += UtilYupana.WriteArchivoGams(acumulado.ToString());
                        UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                        sLine = string.Empty;
                    }
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoPeriodo, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo Plantas.dat
        /// </summary>
        /// <param name="topcodi"></param>
        private void CrearGamsPTermica(string ruta, int topcodi, CpCategoriaDTO categoria, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            string gMin = string.Empty;
            int idGams = 0;
            List<int> listOrdenCol = new List<int>();
            List<int> listOrdenRestric = new List<int>();
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE   TERMICAS (Ut,*)  Datos de las unidades termicas");
            sLine += UtilYupana.WriteArchivoGams(string.Empty);
            var listSubRestr = ObtenerSubRestriccion(ConstantesBase.ResResopPt);
            var listaCombustible = ListarGrupoRecursoFamiliaGams((int)topcodi, ConstantesBase.PlantaT, true);
            foreach (var reg in listSubRestr)
            {
                if (reg.Srestnombregams != null && reg.Srestnombregams != "")
                {
                    sLine += UtilYupana.WriteArchivoGams(reg.Srestnombregams);
                    listOrdenRestric.Add(reg.Srestcodi);
                }
            }
            var lprop = ObtenerListaPropiedadesCategoria(_listaPropiedades, (short)((categoria != null) ? categoria.Catcodi : 0));
            foreach (var reg in lprop)
            {
                if (reg.Propabrevgams != null && reg.Propabrevgams != "")
                {
                    sLine += UtilYupana.WriteArchivoGams(reg.Propabrevgams);
                    listOrdenCol.Add(reg.Proporden);
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = string.Empty;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            if (categoria != null)
                categoria.Total = lista.Count;
            int fila = 1;
            int maxpuntos = 0;
            string nptosUt = "MaxTramos (Ut)   /";
            List<string> tablaNpuntos = new List<string>();
            List<string> tablaNpuntosPot = new List<string>();
            var listaRestricciones = ListaSubRestriccionCategoria((int)topcodi, categoria.Catcodi);
            var propiedades = ListarPropRecursoGams(-1, (int)topcodi, ConstantesBase.ModoT);
            foreach (var reg in lista)
            {
                idGams++;
                sLine += UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaT + idGams.ToString());
                SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.ModoT, 0);
                var restricciones = listaRestricciones.Where(x => x.Recurcodi == reg.Recurcodi).ToList();
                var findGMin = propiedades.Find(x => x.Recurcodi == reg.Recurcodi && x.Propcodi == ConstantesBase.IdGenMIn);
                var findFuncComb = propiedades.Find(x => x.Recurcodi == reg.Recurcodi && x.Propcodi == ConstantesBase.IdFuncComb);
                //Obtener funcion combustible
                if (findGMin != null)
                    if (!string.IsNullOrEmpty(findGMin.Valor)) gMin = findGMin.Valor;
                    else gMin = "0";
                if (findFuncComb != null)
                    if (!string.IsNullOrEmpty(findFuncComb.Valor))
                    {
                        var funcion = UtilYupana.ConvertirStringToFuncComb(findFuncComb.Valor);
                        if (funcion.Count > maxpuntos) maxpuntos = funcion.Count;
                        nptosUt += " Ut" + idGams.ToString() + "   " + funcion.Count.ToString() + ",";
                        string filaNpuntos = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaT + idGams.ToString());
                        string filNptosPot = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaT + idGams.ToString());

                        //decimal? potant = decimal.Parse(gMin);
                        decimal? potant = 0;
                        decimal? potencia = 0M;

                        //foreach (var g in funcion)
                        for (int i = 0; i < funcion.Count; i++)
                        {
                            filaNpuntos += UtilYupana.WriteArchivoGams(funcion[i].Consumo.ToString());
                            switch (i)
                            {
                                case 0:
                                    potencia = funcion[i].Potencia;
                                    break;
                                case 1:
                                    decimal vgmin = 0;
                                    decimal.TryParse(gMin, out vgmin);
                                    potencia = funcion[i].Potencia - vgmin;
                                    potant = funcion[i].Potencia;
                                    break;
                                default:
                                    potencia = funcion[i].Potencia - potant;
                                    break;
                            }

                            filNptosPot += UtilYupana.WriteArchivoGams(potencia.ToString());
                            potant = funcion[i].Potencia;
                        }
                        tablaNpuntos.Add(filaNpuntos);
                        tablaNpuntosPot.Add(filNptosPot);
                    }
                // colocar los valores de las restricciones en la tabla
                foreach (var p in listOrdenRestric)
                {
                    var restriccion = restricciones.Find(x => x.Srestcodi == p);
                    if (restriccion != null) sLine += UtilYupana.WriteArchivoGams(restriccion.Srestdvalor1.ToString());
                    else sLine += UtilYupana.WriteArchivoGams(string.Empty);
                }
                // colocar los valores de las propiedades en la tabla
                foreach (var p in listOrdenCol)
                {
                    var propiedad = propiedades.Find(x => x.Proporden == p && x.Recurcodi == reg.Recurcodi);
                    if (propiedad != null)
                    {
                        switch (p)
                        {
                            case ConstantesBase.CostoCombustibleOrden:
                            case ConstantesBase.CostoVarOrden:
                                break;
                            case ConstantesBase.TMinOperOrden:
                                var find1 = restricciones.Find(x => x.Srestcodi == ConstantesBase.SRES_TMINCE_PT);
                                if (find1 != null)
                                {
                                    sLine += UtilYupana.WriteArchivoGams(find1.Srestdvalor1.ToString());
                                }
                                else
                                {
                                    sLine += UtilYupana.WriteArchivoGams(string.Empty);
                                }
                                break;
                            case ConstantesBase.TMinTSOrden:
                                var find2 = restricciones.Find(x => x.Srestcodi == ConstantesBase.SRES_TMINCE_PT);
                                if (find2 != null)
                                {
                                    sLine += UtilYupana.WriteArchivoGams(find2.Srestdvalor2.ToString());
                                }
                                else
                                {
                                    sLine += UtilYupana.WriteArchivoGams(string.Empty);
                                }
                                break;
                            case 0: /// Orden 0 
                                sLine += UtilYupana.WriteArchivoGams(reg.Recurconsideragams.ToString());
                                break;
                            default:
                                sLine += UtilYupana.WriteArchivoGams(propiedad.Valor);
                                break;
                        }

                    }
                    else
                    {
                        switch (p)
                        {
                            case ConstantesBase.TipoPlantaOrden:
                                sLine += UtilYupana.WriteArchivoGams("0");
                                break;
                            case ConstantesBase.TMinOperOrden:
                                var find1 = restricciones.Find(x => x.Srestcodi == ConstantesBase.SRES_TMINCE_PT);
                                if (find1 != null)
                                {
                                    sLine += UtilYupana.WriteArchivoGams(find1.Srestdvalor1.ToString());
                                }
                                else
                                {
                                    sLine += UtilYupana.WriteArchivoGams(string.Empty);
                                }
                                break;
                            case ConstantesBase.TMinTSOrden:
                                var find2 = restricciones.Find(x => x.Srestcodi == ConstantesBase.SRES_TMINCE_PT);
                                if (find2 != null)
                                {
                                    sLine += UtilYupana.WriteArchivoGams(find2.Srestdvalor2.ToString());
                                }
                                else
                                {
                                    sLine += UtilYupana.WriteArchivoGams(string.Empty);
                                }
                                break;
                            default:
                                sLine += UtilYupana.WriteArchivoGams("");
                                break;
                        }

                    }
                }
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                sLine = string.Empty;
                fila++;
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            //Nro de Puntos
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            UtilYupana.AgregaLinea(ref textoArchivo, "PARAMETER");
            nptosUt = nptosUt.Substring(0, nptosUt.Length - 1) + " /;";
            UtilYupana.AgregaLinea(ref textoArchivo, nptosUt);
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            //Tabla Combustible
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE Coef(Ut,tramo)");
            string tituloNpto = UtilYupana.WriteArchivoGams(string.Empty);
            for (var i = 1; i <= maxpuntos; i++)
                tituloNpto += UtilYupana.WriteArchivoGams(i.ToString());
            UtilYupana.AgregaLinea(ref textoArchivo, tituloNpto);

            foreach (var g in tablaNpuntos)
                UtilYupana.AgregaLinea(ref textoArchivo, g);
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            //Tabla Potencia
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE LimitDp (Ut,tramo)");
            tituloNpto = UtilYupana.WriteArchivoGams(string.Empty);
            for (var i = 1; i <= maxpuntos; i++)
                tituloNpto += UtilYupana.WriteArchivoGams(i.ToString());
            UtilYupana.AgregaLinea(ref textoArchivo, tituloNpto);

            foreach (var g in tablaNpuntosPot)
                UtilYupana.AgregaLinea(ref textoArchivo, g);
            UtilYupana.AgregaLinea(ref textoArchivo, "");

            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            UtilYupana.AgregaLinea(ref textoArchivo, "");
            //Costo de Combustible
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE CComb(Ut,t) Datos de las unidades termicas");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresPTCCOMB, topcodi, ConstantesYupana.PrefijoPlantaT, lista, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "");

            //Costo No Variable
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE CVNC(Ut,t) Datos de las unidades termicas");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresPTCVNC, topcodi, ConstantesYupana.PrefijoPlantaT, lista, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "");

            //Costo de Combustible
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE CCombEf(Ut,t) Datos de las unidades termicas");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresPTCCOMBEFIC, (int)topcodi, ConstantesYupana.PrefijoPlantaT, lista, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "");

            //Costo No Variable
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE CVNCEf(Ut,t) Datos de las unidades termicas");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresPTCVNCEFIC, (int)topcodi, ConstantesYupana.PrefijoPlantaT, lista, iniEtapa, finEtapa);

            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoPlantaTGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo tension.dat
        /// </summary>
        /// <param name="topcodi"></param>
        private void CrearGamsNodoTopologico(string ruta, int topcodi, CpCategoriaDTO categoria)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            int idGams = 0;
            List<int> listOrdenCol = new List<int>();
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE Barra(N,*)");
            sLine += UtilYupana.WriteArchivoGams(string.Empty);
            var lprop = ObtenerListaPropiedadesCategoria(_listaPropiedades, (short)((categoria != null) ? categoria.Catcodi : 0));
            foreach (var reg in lprop)
            {
                if (reg.Propabrevgams != null)
                {
                    if (reg.Propabrevgams != "")
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Propabrevgams);
                        listOrdenCol.Add(reg.Proporden);
                    }
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = string.Empty;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            if (categoria != null)
                categoria.Total = lista.Count;
            var propiedades = ListarPropRecursoGams(-1, topcodi, categoria.Catcodi);
            CpProprecursoDTO propiedad;
            int idgamsAnt = -1;
            foreach (var reg in lista)
            {

                int idFicticio = BuscarEquipoFicticio(reg.Recurcodi);
                if (idFicticio > 0)
                {
                    int idTempGams = MapeoGamsEscenario.FindIdGams(idFicticio, ConstantesBase.NodoTopologico);
                    if (idTempGams == 0)
                    {
                        idGams++;
                        SaveRecursoGams(idFicticio, idGams, ConstantesBase.NodoTopologico, 0, true);
                    }
                }
                else
                {
                    idGams++;
                    SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.NodoTopologico, 0);
                }
                if (idgamsAnt != idGams)
                {
                    sLine += UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoNodoTopologico + idGams.ToString());
                    foreach (var p in listOrdenCol)
                    {
                        propiedad = propiedades.Find(x => x.Proporden == p && x.Recurcodi == reg.Recurcodi);
                        if (propiedad != null)
                        {
                            if (propiedad.Valor != null)
                                sLine += UtilYupana.WriteArchivoGams(propiedad.Valor);
                            else
                                sLine += UtilYupana.WriteArchivoGams(string.Empty);
                        }
                        else
                            sLine += UtilYupana.WriteArchivoGams(string.Empty);
                    }
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);

                }
                sLine = string.Empty;
                idgamsAnt = idGams;
            }

            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoNodoTGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo Hidro.dat
        /// </summary>
        /// <param name="topcodi"></param>
        private void CrearGamsPlantasHidro(string ruta, int topcodi, CpCategoriaDTO categoria)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            int idGams = 0;
            List<int> listOrdenCol = new List<int>();
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE  ParamUh(Uh,*)");
            sLine += UtilYupana.WriteArchivoGams(string.Empty);
            var lprop = ObtenerListaPropiedadesCategoria(_listaPropiedades, (short)((categoria != null) ? categoria.Catcodi : 0));
            foreach (var reg in lprop)
            {
                if (reg.Propabrevgams != null)
                {
                    if (reg.Propabrevgams != "")
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Propabrevgams);
                        listOrdenCol.Add(reg.Proporden);
                    }
                }
            }
            /// Listar Restricciones
            sLine += UtilYupana.WriteArchivoGams("HTMinES");
            sLine += UtilYupana.WriteArchivoGams("HTMinFS");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = string.Empty;

            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            if (categoria != null)
                categoria.Total = lista.Count;
            var listaRestricciones = ListaSubRestriccionCategoria((int)topcodi, categoria.Catcodi).Where(x => x.Srestcodi == ConstantesBase.SresPHRestricOp).ToList();
            foreach (var reg in lista)
            {
                idGams++;
                sLine += UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaH + idGams.ToString());
                SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.PlantaH, 0);
                var propiedades = ListarPropiedadRecursoGams(reg.Recurcodi, reg.Catcodi);
                foreach (var p in listOrdenCol)
                {
                    var propiedad = propiedades.Find(x => x.Proporden == p);
                    if (propiedad != null)
                    {
                        if (propiedad.Valor != null)
                            sLine += UtilYupana.WriteArchivoGams(propiedad.Valor);
                        else
                            sLine += UtilYupana.WriteArchivoGams(string.Empty);
                    }
                    else
                        sLine += UtilYupana.WriteArchivoGams(string.Empty);
                }
                var restriccion = listaRestricciones.Find(x => x.Recurcodi == reg.Recurcodi);
                if (restriccion != null)
                {
                    if (restriccion.Srestdvalor1 != null)
                        sLine += UtilYupana.WriteArchivoGams(restriccion.Srestdvalor1.ToString());
                    else
                        sLine += UtilYupana.WriteArchivoGams(string.Empty);
                    if (restriccion.Srestdvalor2 != null)
                        sLine += UtilYupana.WriteArchivoGams(restriccion.Srestdvalor2.ToString());
                    else
                        sLine += UtilYupana.WriteArchivoGams(string.Empty);
                }
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                sLine = string.Empty;
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoPlantaHGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo embalse.dat
        /// </summary>
        /// <param name="topcodi"></param>
        private void CrearGamsEmbalse(string ruta, int topcodi, CpCategoriaDTO categoria)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            string sLine = string.Empty;
            List<int> listOrdenCol = new List<int>();
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE  ParamEmb(Emb,*)");
            sLine += UtilYupana.WriteArchivoGams(string.Empty);
            var lprop = ObtenerListaPropiedadesCategoria(_listaPropiedades, (short)((categoria != null) ? categoria.Catcodi : 0));
            foreach (var reg in lprop)
            {
                if (reg.Propabrevgams != null)
                {
                    if (reg.Propabrevgams != "")
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Propabrevgams);
                        listOrdenCol.Add(reg.Proporden);
                    }
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = string.Empty;
            string prefijo = ConstantesYupana.PrefijoEmbalse;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            var listaRestric = ListarRecursosEnSubRestriccion((int)topcodi, (int)ConstantesBase.SRES_VOLMETA_EMB, ConstantesBase.Embalse);
            string ValorIni = string.Empty;
            if (categoria != null)
                categoria.Total = lista.Count;
            foreach (var reg in lista)
            {
                idGams++;
                ValorIni = string.Empty;
                sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.Embalse, 0);
                var propiedades = ListarPropiedadRecursoGams(reg.Recurcodi, reg.Catcodi);
                foreach (var p in listOrdenCol)
                {
                    var propiedad = propiedades.Find(x => x.Proporden == p);
                    if (propiedad != null)
                    {
                        if (propiedad.Valor != null)
                            sLine += UtilYupana.WriteArchivoGams(propiedad.Valor);
                        else
                            sLine += UtilYupana.WriteArchivoGams(string.Empty);
                        if (propiedad.Proporden == ConstantesBase.OrdenVolMin)
                            ValorIni = propiedad.Valor;
                    }

                    else
                        sLine += UtilYupana.WriteArchivoGams(string.Empty);
                }
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                sLine = string.Empty;
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoEmbalseGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo Renovables.dat de Yupana Continuo
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        private void CrearGamsPRerYupanaContinuo(string ruta, List<CpMedicion48DTO> lista, int topcodi, CpCategoriaDTO categoria, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            //int idGams = 0;
            //if (categoria != null)
            //    categoria.Total = lista.Count;
            //foreach (var reg in lista)
            //{
            //    idGams++;
            //    SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.PlantaNoConvenO, 0);
            //}
            string sLine = string.Empty;
            //Caudal de Embalse
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE          Pnc(t,Unc)");
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48VerticalYupanaContinuo(ref textoArchivo, lista, ConstantesBase.SRES_GENER_RER, topcodi, ConstantesYupana.PrefijoPlantaRer, ConstantesBase.PlantaNoConvenO, iniEtapa, finEtapa);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoPlantaNoConvOGams2, ruta, textoArchivo);
        }


        /// <summary>
        /// Crea el archivo Renovables.dat
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        private void CrearGamsPRer(string ruta, int topcodi, CpCategoriaDTO categoria, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            if (categoria != null)
                categoria.Total = lista.Count;
            foreach (var reg in lista)
            {
                idGams++;
                SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.PlantaNoConvenO, 0);
            }
            string sLine = string.Empty;
            //Caudal de Embalse
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE          Pnc(t,Unc)");
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48Vertical(ref textoArchivo, ConstantesBase.SRES_GENER_RER, topcodi, ConstantesYupana.PrefijoPlantaRer, ConstantesBase.PlantaNoConvenO, iniEtapa, finEtapa);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoPlantaNoConvOGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo linea.dat
        /// </summary>
        /// <param name="topcodi"></param>
        private void CrearGamsLineas(string ruta, int topologia, int iniEtapa, int finEtapa, CpCategoriaDTO categoria)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            int total = 0;
            string sLine = string.Empty;

            List<int> listOrdenCol = new List<int>();
            List<string> listaLineaNN = new List<string>();
            List<string> listaLinea = new List<string>();
            UtilYupana.AgregaLinea(ref textoArchivo, "SET MAPLIN(L,N,N)");
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            string prefijo = ConstantesYupana.PrefijoLinea;
            ///
            sLine += UtilYupana.WriteArchivoGams(string.Empty);
            var listProp = ObtenerListaPropiedades(ConstantesBase.Linea);
            foreach (var reg in listProp)
            {
                if (reg.Propabrevgams != null)
                {
                    if (reg.Propabrevgams != "")
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Propabrevgams);
                        listOrdenCol.Add(reg.Proporden);
                    }
                }
            }
            var lista = ListarLineaGams(ConstantesBase.Linea, topologia).OrderBy(x => x.Recurcodi).ToList();
            var listat2 = ListarLineaGams(ConstantesBase.Trafo2D, topologia).OrderBy(x => x.Recurcodi).ToList();
            var listat3 = ListarLineaGams(ConstantesBase.Trafo3D, topologia).OrderBy(x => x.Recurcodi).ToList();
            lista = lista.Union(listat2).Concat(listat3).ToList();
            total = lista.Count;
            string scategoria = ConstantesBase.Linea.ToString() + "," + ConstantesBase.Trafo2D + "," + ConstantesBase.Trafo3D;
            List<CpProprecursoDTO> propLineas = ListarPropRecursoGams(-1, topologia, ConstantesBase.Linea);
            List<CpProprecursoDTO> propTrafos2d = ListarPropRecursoGams(-1, topologia, ConstantesBase.Trafo2D);
            List<CpProprecursoDTO> propTrafos3d = ListarPropRecursoGams(-1, topologia, ConstantesBase.Trafo3D);
            var propiedades = propLineas.Union(propTrafos2d).Concat(propTrafos3d).ToList();
            foreach (var reg in lista)
            {
                bool lineaReduccion = false;
                var find = propiedades.Find(x => x.Proporden == ConstantesBase.ReduccionOrden && x.Recurcodi == reg.Recurcodi && x.Catcodi == reg.Catcodi);
                if (find != null)
                {
                    if (find.Valor != null)
                    {
                        var reduccion = find.Valor;

                        if (reduccion == "1")
                        {
                            lineaReduccion = true;
                            total--;
                        }
                    }
                }

                if (!lineaReduccion)
                {
                    idGams++;
                    SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.Linea, reg.Catcodi);
                    int idNodo = ReduccionBarra.FindEquipo((int)reg.RecNodoTopOrigenID);
                    int nodo1 = 0;
                    if (idNodo == 0)
                        nodo1 = FindIdGams((int)reg.RecNodoTopOrigenID, ConstantesBase.NodoTopologico);
                    else
                        nodo1 = FindIdGams(idNodo, ConstantesBase.NodoTopologico);
                    idNodo = ReduccionBarra.FindEquipo((int)reg.RecNodoTopDestinoID);
                    int nodo2 = 0;
                    if (idNodo == 0)
                        nodo2 = FindIdGams((int)reg.RecNodoTopDestinoID, ConstantesBase.NodoTopologico);
                    else
                        nodo2 = FindIdGams(idNodo, ConstantesBase.NodoTopologico);
                    string fila = prefijo + idGams + " .   " + ConstantesYupana.PrefijoNodoTopologico + nodo1.ToString() +
                        "  .    " + ConstantesYupana.PrefijoNodoTopologico + nodo2.ToString();
                    listaLineaNN.Add(fila);
                    fila = string.Empty;
                    fila += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                    foreach (var p in listOrdenCol)
                    {
                        var propiedad = propiedades.Find(x => x.Proporden == p && x.Recurcodi == reg.Recurcodi && x.Catcodi == reg.Catcodi);
                        if (propiedad != null)
                        {
                            if (p == ConstantesBase.PerdidasTransOrd)//Perdidas Transversales
                            {
                                var findPerMax = propiedades.Find(x => x.Proporden == ConstantesBase.PerdidasMaxOrd); // Perdidas Maximas
                                if (findPerMax != null)
                                {
                                    if (findPerMax.Valor == "0")
                                        fila += UtilYupana.WriteArchivoGams(string.Empty);
                                    else
                                        fila += UtilYupana.WriteArchivoGams(propiedad.Valor);
                                }
                                else
                                    fila += UtilYupana.WriteArchivoGams(string.Empty);
                            }
                            else
                            {
                                fila += UtilYupana.WriteArchivoGams(propiedad.Valor);
                            }
                        }
                        else
                            fila += UtilYupana.WriteArchivoGams(string.Empty);
                    }
                    listaLinea.Add(fila);
                }
            }
            categoria.Total = total;
            foreach (var reg in listaLineaNN)
            {
                UtilYupana.AgregaLinea(ref textoArchivo, reg);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "/;");
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE Linea(L,*)");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = string.Empty;
            foreach (var reg in listaLinea)
            {
                UtilYupana.AgregaLinea(ref textoArchivo, reg);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoLineaGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo resURS.dat
        /// </summary>
        private void CrearGamsURS(string ruta, int topcodi, CpCategoriaDTO categoria)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            CpRecursoDTO find;
            int idGams = 0, idPhGams = 0, idPTGams = 0;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            if (categoria != null)
                categoria.Total = lista.Count;
            foreach (var p in lista)
            {
                idGams++;
                SaveRecursoGams(p.Recurcodi, idGams, ConstantesBase.Urs, 0);
            }
            var listaaux = ListarGrupoRecursoFamiliaGams(topcodi, ConstantesBase.Urs, true);
            var listah = listaaux.Where(x => x.Catcodisec == ConstantesBase.PlantaH).OrderBy(x => x.Recurcodisicoes).ToList();
            var listap = listaaux.Where(x => x.Catcodisec == ConstantesBase.ModoT).OrderBy(x => x.Recurcodisicoes).ToList();
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE RangoRSFh(Uh,*)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   rango de operacion de la RSF en centrales hidro");
            sLine = UtilYupana.WriteArchivoGams("");
            sLine += UtilYupana.WriteArchivoGams("Min");
            sLine += UtilYupana.WriteArchivoGams("Max");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listapropiedades = ListarPropRecursoGams(-1, (int)topcodi, ConstantesBase.Urs);
            var listapropPh = ListarPropRecursoGams(-1, (int)topcodi, ConstantesBase.PlantaH);
            var listapropMt = ListarPropRecursoGams(-1, (int)topcodi, ConstantesBase.ModoT);
            bool listavacia = true;
            foreach (var p in listah)
            {
                find = lista.Find(x => x.Recurcodi == p.Recurcodi);
                if (find != null)
                {
                    sLine = string.Empty;
                    idPhGams = FindIdGams(p.Recurcodisicoes, ConstantesBase.PlantaH);
                    sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaH + idPhGams.ToString());
                    var propiedades = listapropPh.Where(x => x.Recurcodi == p.Recurcodisicoes).ToList();
                    var find2 = propiedades.Find(x => x.Propcodi == ConstantesBase.MinimoPh);
                    sLine += (find2 != null) ? UtilYupana.WriteArchivoGams(find2.Valor) : UtilYupana.WriteArchivoGams("");
                    find2 = propiedades.Find(x => x.Propcodi == ConstantesBase.MaximoPh);
                    sLine += (find2 != null) ? UtilYupana.WriteArchivoGams(find2.Valor) : UtilYupana.WriteArchivoGams("");
                    listavacia = false;
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            if (listavacia)
            {
                UtilYupana.AgregaLinea(ref textoArchivo, ConstantesYupana.PrefijoPlantaH + "1");
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE RangoRSFt(Ut,*)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   rango de operacion de la RSF en centrales térmicas");
            sLine = UtilYupana.WriteArchivoGams("");
            sLine += UtilYupana.WriteArchivoGams("Min");
            sLine += UtilYupana.WriteArchivoGams("Max");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            listavacia = true;
            foreach (var p in listap)
            {
                find = lista.Find(x => x.Recurcodi == p.Recurcodi);
                if (find != null)
                {
                    sLine = string.Empty;
                    idPTGams = FindIdGams(p.Recurcodisicoes, ConstantesBase.ModoT);
                    sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoPlantaT + idPTGams.ToString());
                    var propiedades = listapropMt.Where(x => x.Recurcodi == p.Recurcodisicoes).ToList();


                    var find2 = propiedades.Find(x => x.Propcodi == ConstantesBase.MinimoPt);
                    if (find2 != null)
                    {
                        if (find2.Valor != null)
                            sLine += UtilYupana.WriteArchivoGams(find2.Valor);
                        else
                            sLine += UtilYupana.WriteArchivoGams("");
                    }
                    else
                    {
                        sLine += UtilYupana.WriteArchivoGams("");
                    }

                    find2 = propiedades.Find(x => x.Propcodi == ConstantesBase.MaximoPt);
                    if (find2 != null)
                    {
                        if (find2.Valor != null)
                            sLine += UtilYupana.WriteArchivoGams(find2.Valor);
                        else
                            sLine += UtilYupana.WriteArchivoGams("");
                    }
                    else
                    {
                        sLine += UtilYupana.WriteArchivoGams("");
                    }
                    listavacia = false;
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            if (listavacia)
            {
                UtilYupana.AgregaLinea(ref textoArchivo, ConstantesYupana.PrefijoPlantaT + "1");
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoresResrangoGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Genera Archivo ProvisionBase
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        /// <param name="topsinres"></param>
        private void CrearProvisionUrs(string ruta, int topcodi, int topsinres, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            List<CpRecursoDTO> lista = new List<CpRecursoDTO>();
            List<CpRecursoDTO> listaPlantaT = new List<CpRecursoDTO>();
            List<CpRecursoDTO> listaPlantaH = new List<CpRecursoDTO>();
            CpRecursoDTO regP;
            CpRecursoDTO find;
            int idGams;
            string sLine = string.Empty;
            if (topsinres == 0)
                lista = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.Urs).Where(x => x.ProvisionBaseUp == 1 || x.ProvisionBaseDn == 1).ToList();
            var listaaux = ListarGrupoRecursoFamiliaGams(topcodi, ConstantesBase.Urs, true);
            var listah = listaaux.Where(x => x.Catcodisec == ConstantesBase.PlantaH).OrderBy(x => x.Recurcodisicoes).ToList();
            foreach (var reg in listah)
            {
                regP = new CpRecursoDTO();
                regP.Recurcodi = reg.Recurcodisicoes;
                regP.Catcodi = reg.Catcodisec;
                find = lista.Find(x => x.Recurcodi == reg.Recurcodi);
                if (find != null)
                    listaPlantaH.Add(regP);
            }

            var listap = listaaux.Where(x => x.Catcodisec == ConstantesBase.ModoT).OrderBy(x => x.Recurcodisicoes).ToList();
            foreach (var reg in listap)
            {
                regP = new CpRecursoDTO();
                regP.Recurcodi = reg.Recurcodisicoes;
                regP.Catcodi = reg.Catcodisec;
                find = lista.Find(x => x.Recurcodi == reg.Recurcodi);
                if (find != null)
                    listaPlantaT.Add(regP);
            }

            UtilYupana.AgregaLinea(ref textoArchivo, "Parameter ProvBaseUp(URS)");
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            var listaUp = lista.Where(x => x.ProvisionBaseUp == 1).ToList();
            foreach (var p in listaUp)
            {
                idGams = FindIdGams(p.Recurcodi, ConstantesBase.Urs);
                sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoURS + idGams.ToString());
                sLine += UtilYupana.WriteArchivoGams("");
                sLine += UtilYupana.WriteArchivoGams("1");
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                sLine = string.Empty;
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            UtilYupana.AgregaLinea(ref textoArchivo, "Parameter ProvBaseDn(URS)");
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            var listaDn = lista.Where(x => x.ProvisionBaseDn == 1).ToList();
            foreach (var p in listaDn)
            {
                idGams = FindIdGams(p.Recurcodi, ConstantesBase.Urs);
                sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoURS + idGams.ToString());
                sLine += UtilYupana.WriteArchivoGams("");
                sLine += UtilYupana.WriteArchivoGams("1");
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                sLine = string.Empty;
            }
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            UtilYupana.AgregaLinea(ref textoArchivo, "Table BandProvBaseTerUp(Ut,t)");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresProvisionBaseUp, topcodi, ConstantesYupana.PrefijoPlantaT, listaPlantaT, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "Table BandProvBaseTerDn(Ut,t)");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresProvisionBaseDn, topcodi, ConstantesYupana.PrefijoPlantaT, listaPlantaT, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "Table BandProvBaseHidUp(Uh,t)");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresProvisionBaseUp, topcodi, ConstantesYupana.PrefijoPlantaH, listaPlantaH, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "Table BandProvBaseHidDn(Uh,t)");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresProvisionBaseDn, topcodi, ConstantesYupana.PrefijoPlantaH, listaPlantaH, iniEtapa, finEtapa);
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoresProvisionBaseGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Genera archivo Gams RSF
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="categoria"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        /// <param name="topsinres"></param>
        private void CrearGamsRSF(string ruta, int topcodi, CpCategoriaDTO categoria, int iniEtapa, int finEtapa, int topsinres)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            int idGams = 0;
            var listaRsf = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            foreach (var reg in listaRsf)
            {
                idGams++;
                SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.Rsf, 0);
            }
            var lista = ListarURSporRSF(ConstantesBase.Urs, topcodi).ToList();
            if (categoria != null)
                categoria.Total = listaRsf.Count;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE ReservaUp(t,RSF) reserva secundaria total hacia arriba");
            sLine = UtilYupana.WriteArchivoGams("*");
            sLine += UtilYupana.WriteArchivoGams("");
            sLine += UtilYupana.WriteArchivoGams("Autom");
            sLine += UtilYupana.WriteArchivoGams("Manual");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            ImprimirGamsRestric48Vertical(ref textoArchivo, ConstantesBase.SresSisRSFUp, topcodi, ConstantesYupana.PrefijoRSF, ConstantesBase.Rsf, iniEtapa, finEtapa, topsinres);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE ReservaDn(t,RSF)  reserva secundaria total hacia abajo");
            sLine = UtilYupana.WriteArchivoGams("*");
            sLine += UtilYupana.WriteArchivoGams("");
            sLine += UtilYupana.WriteArchivoGams("Autom");
            sLine += UtilYupana.WriteArchivoGams("Manual");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            ImprimirGamsRestric48Vertical(ref textoArchivo, ConstantesBase.SresSisRSFDn, topcodi, ConstantesYupana.PrefijoRSF, ConstantesBase.Rsf, iniEtapa, finEtapa, topsinres);
            sLine = UtilYupana.WriteArchivoGams("");
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            UtilYupana.AgregaLinea(ref textoArchivo, "Parameter URStimeUp(RSF)");
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            UtilYupana.AgregaLinea(ref textoArchivo, "RSF1  0");
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoRSFGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo arranques.dat para entrada a GAMS
        /// </summary>
        /// <param name="topologia"></param>
        /// <param name="modo"></param>
        private void CrearGamsArranques(string ruta, int topologia)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            string sLine = string.Empty;
            string prefijo = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE ArranqueH(Uh,*)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Numero maximo de arranques en todo el horizonte o dia");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("Horizonte");
            sLine += UtilYupana.WriteArchivoGams("Dia");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            prefijo = ConstantesYupana.PrefijoPlantaH;
            sLine = string.Empty;
            var lista = ListarRecursosEnSubRestriccion((int)topologia, (int)ConstantesBase.SRES_NMAX_PH, ConstantesBase.PlantaH).Where(x => x.Srestdvalor1 != null || x.Srestdvalor2 != null).OrderBy(x => x.Recurcodi).ToList();

            foreach (var reg in lista)
            {
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.PlantaH);
                if (idGams > 0)
                {
                    sLine = UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                    if (reg.Srestdvalor1 != null)
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Srestdvalor1.ToString());
                    }
                    else
                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                    if (reg.Srestdvalor2 != null)
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Srestdvalor2.ToString());
                    }
                    else
                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }

            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE ArranqueT(Ut,*)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Numero maximo de arranques en todo el horizonte o dia");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("Horizonte");
            sLine += UtilYupana.WriteArchivoGams("Dia");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            prefijo = ConstantesYupana.PrefijoPlantaT;
            lista = ListarRecursosEnSubRestriccion((int)topologia, (int)ConstantesBase.SRES_NMAX_PT, ConstantesBase.PlantaT).OrderBy(x => x.Recurcodi).ToList().Where(x => x.Srestdvalor1 != null || x.Srestdvalor2 != null).OrderBy(x => x.Recurcodi).ToList();

            foreach (var reg in lista)
            {
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.ModoT);
                if (idGams > 0)
                {
                    sLine = UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                    if (reg.Srestdvalor1 != null)
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Srestdvalor1.ToString());
                    }
                    else
                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                    if (reg.Srestdvalor2 != null)
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Srestdvalor2.ToString());
                    }
                    else
                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }

            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoArranquesGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo paradas.dat para entrada a GAMS
        /// </summary>
        /// <param name="topologia"></param>
        /// <param name="modo"></param>
        private void CrearGamsParadas(string ruta, int topologia)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            string sLine = string.Empty;
            string prefijo = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE ParadaH(Uh,*)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*  Numero maximo de paradas en todo el horizonte o dia");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("Horizonte");
            sLine += UtilYupana.WriteArchivoGams("Dia");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            prefijo = ConstantesYupana.PrefijoPlantaH;
            sLine = string.Empty;
            var lista = ListarRecursosEnSubRestriccion((int)topologia, (int)ConstantesBase.Sres_nmaxp_ph, ConstantesBase.PlantaH).Where(x => x.Srestdvalor1 != null || x.Srestdvalor2 != null).OrderBy(x => x.Recurcodi).OrderBy(x => x.Recurcodi).ToList();

            foreach (var reg in lista)
            {
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.PlantaH);
                if (idGams > 0)
                {
                    sLine = UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                    if (reg.Srestdvalor1 != null)
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Srestdvalor1.ToString());
                    }
                    else
                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                    if (reg.Srestdvalor2 != null)
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Srestdvalor2.ToString());
                    }
                    else
                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }

            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE ParadaT(Ut,*)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Numero maximo de paradas en todo el horizonte o dia");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("Horizonte");
            sLine += UtilYupana.WriteArchivoGams("Dia");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            prefijo = ConstantesYupana.PrefijoPlantaT;
            lista = ListarRecursosEnSubRestriccion((int)topologia, (int)ConstantesBase.Sres_nmaxp_pt, ConstantesBase.PlantaT).OrderBy(x => x.Recurcodi).ToList().Where(x => x.Srestdvalor1 != null || x.Srestdvalor2 != null).OrderBy(x => x.Recurcodi).OrderBy(x => x.Recurcodi).ToList();

            foreach (var reg in lista)
            {
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.ModoT);
                if (idGams > 0)
                {
                    sLine = UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                    if (reg.Srestdvalor1 != null)
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Srestdvalor1.ToString());
                    }
                    else
                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                    if (reg.Srestdvalor2 != null)
                    {
                        sLine += UtilYupana.WriteArchivoGams(reg.Srestdvalor2.ToString());
                    }
                    else
                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }

            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoParadasGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea Gams de Condiciones Iniciales
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        private void CrearGamsCondIniciales(string ruta, int topcodi)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            string sLine = string.Empty;
            string prefijo = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE   Condinic (Ut,*)  Condiciones iniciales de las centrales termicas");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("Est");
            sLine += UtilYupana.WriteArchivoGams("Numhor");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = UtilYupana.WriteArchivoGams("*");
            sLine += UtilYupana.WriteArchivoGams("(1:ES 0:FS)");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            prefijo = ConstantesYupana.PrefijoPlantaT;
            var listaPT = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.ModoT);
            var lista = ListarRecursosEnSubRestriccion((int)topcodi, (int)ConstantesBase.SRES_CONDINI_PT, ConstantesBase.PlantaT);
            var listaMod = ListarGrupoRecursoFamiliaGams(topcodi, ConstantesBase.Caldero, true);
            foreach (var reg in listaPT)
            {
                var find3 = listaMod.Find(x => x.Recurcodi == reg.Recurcodi);
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.ModoT);
                if (idGams != 0)
                {
                    sLine = UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                    var find = lista.Find(x => x.Recurcodi == reg.Recurcodi);
                    if (find == null)
                    {
                        sLine += UtilYupana.WriteArchivoGams("0");
                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaracterInf);
                    }
                    else
                    {
                        if (find.Srestdvalor1 != null)
                        {
                            sLine += UtilYupana.WriteArchivoGams(find.Srestdvalor1.ToString());
                        }
                        else
                            sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                        if (find3 != null)
                            sLine += UtilYupana.WriteArchivoGams("168");
                        if (find.Srestdvalor2 != null)
                        {
                            sLine += UtilYupana.WriteArchivoGams(find.Srestdvalor2.ToString());
                        }
                        else
                            sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);

                    }

                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            if (lista.Count == 0)
            {
                idGams = 0;
                if (listaPT.Count() > 0)
                {
                    idGams = 1;
                }
                sLine = UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE   CondinicCal (Cal,*)  Condiciones iniciales de las calderos");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("Est");
            sLine += UtilYupana.WriteArchivoGams("Numhor");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = UtilYupana.WriteArchivoGams("*");
            sLine += UtilYupana.WriteArchivoGams("(1:ES 0:FS)");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            prefijo = ConstantesYupana.PrefijoCaldero;
            var listaCaldero = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.Caldero);
            var listaC = ListarRecursosEnSubRestriccion(topcodi, (int)ConstantesBase.SresCondiniCaldero, ConstantesBase.Caldero);
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            foreach (var reg in listaCaldero)
            {
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.Caldero);
                if (idGams != 0)
                {
                    sLine = UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                    var find = listaC.Find(x => x.Recurcodi == reg.Recurcodi);
                    if (find == null)
                    {
                        sLine += UtilYupana.WriteArchivoGams("0");
                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaracterInf);
                    }
                    else
                    {
                        if (find.Srestdvalor1 != null)
                        {
                            sLine += UtilYupana.WriteArchivoGams(find.Srestdvalor1.ToString());
                        }
                        else
                            sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                        if (find.Srestdvalor2 != null)
                        {
                            sLine += UtilYupana.WriteArchivoGams(find.Srestdvalor2.ToString());
                        }
                        else
                            sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);

                    }

                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            if (listaC.Count == 0)
            {
                idGams = 0;
                if (listaCaldero.Count() > 0)
                {
                    idGams = 1;
                }
                sLine = UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                //file.WriteLine(sLine);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoCondIniciales, ruta, textoArchivo);
        }

        /// <summary>
        ///  Crea Archivo Gams de demanda
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        private void CrearGamsDemanda(string ruta, int topcodi, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE activa(t,N)");
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48Vertical(ref textoArchivo, ConstantesBase.SRES_DEMBARRA_NT, topcodi, ConstantesYupana.PrefijoNodoTopologico, ConstantesBase.NodoTopologico, iniEtapa, finEtapa);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoDemanda, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo caudales.dat
        /// </summary>
        /// <param name="nperiodo"></param>
        private void CrearGamsCaudales(string ruta, int topcodi, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            //Caudal de Embalse
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE          Q(t,Emb)");
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48Vertical(ref textoArchivo, ConstantesBase.SRES_APORTES_EMB, topcodi, ConstantesYupana.PrefijoEmbalse, ConstantesBase.Embalse, iniEtapa, finEtapa);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            //Caudal de Planta Hidro
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE          Qn(t,Uh)");
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48Vertical(ref textoArchivo, ConstantesBase.SRES_APORTES_PH, topcodi, ConstantesYupana.PrefijoPlantaH, ConstantesBase.PlantaH, iniEtapa, finEtapa);
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoCaudales, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo caudales.dat
        /// </summary>
        /// <param name="nperiodo"></param>
        private void CrearGamsCaudalesYupanaContinuo(string ruta, string archivo, List<CpMedicion48DTO> lista, int topcodi, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            //Caudal de Embalse
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE          Q(t,Emb)");
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48VerticalYupanaContinuo(ref textoArchivo, lista, ConstantesBase.SRES_APORTES_EMB, topcodi, ConstantesYupana.PrefijoEmbalse, ConstantesBase.Embalse, iniEtapa, finEtapa);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            //Caudal de Planta Hidro
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE          Qn(t,Uh)");
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48Vertical(ref textoArchivo, ConstantesBase.SRES_APORTES_PH, topcodi, ConstantesYupana.PrefijoPlantaH, ConstantesBase.PlantaH, iniEtapa, finEtapa);
            FileHelper.GenerarArchivo(archivo, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo mantenimiento.dat
        /// </summary>
        private void CrearGamsMantenimiento(string ruta, int topcodi, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            //Mantenimiento Planta Termica
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE ManttogerT(Ut,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Porcentaje de indisponibilidad en unidades térmicas: (100: fuera de servicio)");
            sLine = UtilYupana.WriteArchivoGams("");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.ModoT);
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SRES_MANTO_PT, topcodi, ConstantesYupana.PrefijoPlantaT, listaEquipo, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            //Mantenimiento Planta Hidrologica
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE ManttogerH(Uh,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   (Porcentaje de indisponibilidad en unidades hidroeléctricas: (100: fuera de servicio)");
            sLine = UtilYupana.WriteArchivoGams("");
            listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.PlantaH);
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SRES_MANTENIMIENTO_PH, topcodi, ConstantesYupana.PrefijoPlantaH, listaEquipo, iniEtapa, finEtapa);
            //Mantenimiento Lineas
            var lista = ListarLineaGams(ConstantesBase.Linea, topcodi).OrderBy(x => x.Recurcodi).ToList();
            var listat2 = ListarLineaGams(ConstantesBase.Trafo2D, topcodi).OrderBy(x => x.Recurcodi).ToList();
            var listat3 = ListarLineaGams(ConstantesBase.Trafo3D, topcodi).OrderBy(x => x.Recurcodi).ToList();
            lista = lista.Union(listat2).Concat(listat3).ToList();
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE DLT(L,t)");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SRES_MANTENIMIENTO_LT, topcodi, ConstantesYupana.PrefijoLinea, lista.ToList(), iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoMantenimiento, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo Reserva.dat
        /// </summary>
        /// <param name="nperiodo"></param>
        private void CrearGamsReserva(string ruta, int topcodi, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            //RSF Plantas Termicas
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE RPFHid(Uh,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Porcentaje de la RPF (de la potencia generada):");
            sLine = UtilYupana.WriteArchivoGams("");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.PlantaH);
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SRES_RESERVPRIM_PH, topcodi, ConstantesYupana.PrefijoPlantaH, listaEquipo, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            //RSF Planta Hidro
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE RPFTer(Ut,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Porcentaje de la RPF (de la potencia generada):");
            sLine = UtilYupana.WriteArchivoGams("");
            listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.ModoT);
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SRES_RPRIM_PT, topcodi, ConstantesYupana.PrefijoPlantaT, listaEquipo, iniEtapa, finEtapa);
            //RSF Planta Rer
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE RPFUnc(Unc,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Porcentaje de la RPF (de la potencia generada):");
            sLine = UtilYupana.WriteArchivoGams("");
            listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.PlantaNoConvenO);
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresRprimPrer, topcodi, ConstantesYupana.PrefijoPlantaRer, listaEquipo, iniEtapa, finEtapa);
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoReserva, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo Volmeta dat
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topologia"></param>
        private void CrearGamsVolMeta(string ruta, int topologia)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            string sLine = string.Empty;
            string prefijo = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "Table    VolEmb(Emb,*)");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("Considera");
            sLine += UtilYupana.WriteArchivoGams("Min");
            sLine += UtilYupana.WriteArchivoGams("Max");
            sLine += UtilYupana.WriteArchivoGams("Vini");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            prefijo = ConstantesYupana.PrefijoEmbalse;
            sLine = string.Empty;
            List<CpSubrestricdatDTO> lista = new List<CpSubrestricdatDTO>();
            lista = ListarRecursosEnSubRestriccion((int)topologia, (int)ConstantesBase.SRES_VOLMETA_EMB, ConstantesBase.Embalse);
            foreach (var reg in lista)
            {
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.Embalse);
                if (idGams > 0)
                {
                    sLine = UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                    sLine += (reg.Srestdactivo != null) ? UtilYupana.WriteArchivoGams(reg.Srestdactivo.ToString()) : UtilYupana.WriteArchivoGams("0");
                    sLine += (reg.Srestdvalor1 != null) ? UtilYupana.WriteArchivoGams(reg.Srestdvalor1.ToString()) : UtilYupana.WriteArchivoGams("");
                    sLine += (reg.Srestdvalor2 != null) ? UtilYupana.WriteArchivoGams(reg.Srestdvalor2.ToString()) : UtilYupana.WriteArchivoGams("");
                    sLine += (reg.Srestdvalor3 != null) ? UtilYupana.WriteArchivoGams(reg.Srestdvalor3.ToString()) : UtilYupana.WriteArchivoGams("");
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoVolMetaGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea Parametros para archivo Set y archivo Generacion Meta
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        /// <param name="categoria"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        private void CrearGamsGenerMeta(string ruta, int topcodi, CpCategoriaDTO categoria, int iniEtapa, int finEtapa, List<EtapaDTO> listaPeriodoDetalle)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            int totalGen = 0;
            int inigenMeta = 0;
            int fingenMeta = 0;
            CpSubrestricdatDTO find = null;
            string sLine = string.Empty;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            var listaSrestric = ListaCpSubRestriccionDat(topcodi);
            var listaSubRestric = listaSrestric.Where(x => x.Srestcodi == ConstantesBase.SresGenMeta).ToList();
            foreach (var reg in lista)
            {
                // verificar si equipo esta en el rango de periodos
                find = listaSubRestric.Find(x => x.Recurcodi == reg.Recurcodi);
                if (find != null)
                {
                    decimal delta = (listaPeriodoDetalle.Count > 0) ? listaPeriodoDetalle[0].Etpdelta : 1;
                    inigenMeta = GetIndiceEtapa(listaPeriodoDetalle, (decimal)find.Srestdvalor3, (int)(_escenario.Topiniciohora / 2));
                    fingenMeta = GetIndiceEtapa(listaPeriodoDetalle, (decimal)find.Srestdvalor4, (int)(_escenario.Topiniciohora / 2));
                    if (fingenMeta > inigenMeta)
                    {
                        idGams++;
                        SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.GenerMeta, ConstantesBase.GenerMeta);
                    }
                }
            }
            totalGen = idGams;
            if (categoria != null)
                categoria.Total = totalGen;
            totalGen = 0;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE EngMeta(GenMeta,*)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Energia meta");
            sLine = UtilYupana.WriteArchivoGams("");
            sLine += UtilYupana.WriteArchivoGams("Min");
            sLine += UtilYupana.WriteArchivoGams("Max");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var reg in listaSubRestric)
            {
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.GenerMeta);
                if (idGams > 0)
                {
                    totalGen++;
                    sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoGenMeta + idGams.ToString());
                    sLine += UtilYupana.WriteArchivoGams(reg.Srestdvalor1.ToString());
                    sLine += UtilYupana.WriteArchivoGams(reg.Srestdvalor2.ToString());
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            if (totalGen == 0)
            {
                idGams = 0;
                if (lista.Count > 0)
                {
                    idGams = FindIdGams(lista[0].Recurcodi, ConstantesBase.GenerMeta);
                }
                sLine = UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoGenMeta + idGams.ToString());
                sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoGenerMeta, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea Parametros para archivo Set y archivo Restricciones de Generacion
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        /// <param name="categoria"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        private void CrearGamsRestricGener(string ruta, int topcodi, CpCategoriaDTO categoria, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            int idGams = 0;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            foreach (var reg in lista)
            {
                idGams++;
                SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.RestricGener, ConstantesBase.RestricGener);
            }
            if (categoria != null)
                categoria.Total = lista.Count;

            var lista48 = Lista48SubRestriciones(topcodi, ConstantesBase.SresSisRestricGener);
            var listaSrestric = ListarDatosSubRestriccion(topcodi, ConstantesBase.SresSisRestricGener);
            var listaSubRestric = listaSrestric.Where(x => x.Recurconsideragams == 1).OrderBy(x => x.Recurcodi).ToList();
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE RestricGerMay(Resgen,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Generacion mayor que");
            var listaMay = listaSubRestric.Where(x => x.Srestdopcion == ConstantesBase.MayorRestric).ToList();
            ImprimirRestricGener(ref textoArchivo, lista48, listaMay, ConstantesBase.RestricGener, ConstantesYupana.PrefijoResGen, ConstantesBase.SresSisRestricGener, topcodi, iniEtapa, finEtapa);
            var listaMen = listaSubRestric.Where(x => x.Srestdopcion == ConstantesBase.MenorRestric).ToList();
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE RestricGerMen(Resgen,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Generacion menor que");
            ImprimirRestricGener(ref textoArchivo, lista48, listaMen, ConstantesBase.RestricGener, ConstantesYupana.PrefijoResGen, ConstantesBase.SresSisRestricGener, topcodi, iniEtapa, finEtapa);
            var listaIgu = listaSubRestric.Where(x => x.Srestdopcion == ConstantesBase.IgualRestric).ToList();
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE RestricGerIgu(Resgen,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   Generacion igual que");
            ImprimirRestricGener(ref textoArchivo, lista48, listaIgu, ConstantesBase.RestricGener, ConstantesYupana.PrefijoResGen, ConstantesBase.SresSisRestricGener, topcodi, iniEtapa, finEtapa);
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoResGener, ruta, textoArchivo);
        }

        /// <summary>
        /// Crear archivo Suma de Flujos dat
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="categoria"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        private void CrearGamsSumFlujos(string ruta, int topcodi, CpCategoriaDTO categoria, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            foreach (var reg in lista)
            {
                idGams++;
                SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.SumFlujo, 0);
            }
            if (categoria != null)
                categoria.Total = lista.Count;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE SumFmax(Sumf,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   suma de flujos máxima");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresSumFlujoLimSup, topcodi, ConstantesYupana.PrefijoSumFlujo, lista, iniEtapa, finEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE SumFmin(Sumf,t)");
            UtilYupana.AgregaLinea(ref textoArchivo, "*   suma de flujos mínima");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresSumFlujoLimInf, topcodi, ConstantesYupana.PrefijoSumFlujo, lista, iniEtapa, finEtapa);
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoSumaFlujos, ruta, textoArchivo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        /// <param name="categoria"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        /// <param name="listaPeriodoDetalle"></param>
        private void CrearGamsDispComb(string ruta, int topcodi, CpCategoriaDTO categoria, int iniEtapa, int finEtapa, List<EtapaDTO> listaPeriodoDetalle)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            string sLine = "";
            int inigenMeta = 0;
            int fingenMeta = 0;
            CpSubrestricdatDTO find = null;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            var listaSrestric = ListaCpSubRestriccionDat(topcodi);
            var listaSubRestric = listaSrestric.Where(x => x.Srestcodi == ConstantesBase.SresDispComb && x.Srestdopcion != 1).ToList();
            foreach (var reg in lista)
            {
                find = listaSubRestric.Find(x => x.Recurcodi == reg.Recurcodi);
                if (find != null)
                {
                    decimal delta = (listaPeriodoDetalle.Count > 0) ? listaPeriodoDetalle[0].Etpdelta : 1;
                    inigenMeta = GetIndiceEtapa(listaPeriodoDetalle, (decimal)find.Srestdvalor2, (int)(_escenario.Topiniciohora / 2));
                    fingenMeta = GetIndiceEtapa(listaPeriodoDetalle, (decimal)find.Srestdvalor3.GetValueOrDefault(0), (int)(_escenario.Topiniciohora / 2)); //Mejoras RER. validación de NULL
                    if (fingenMeta > inigenMeta)
                    {
                        idGams++;
                        SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.DispComb, ConstantesBase.DispComb);
                    }
                }
            }
            int totalComb = 0;
            UtilYupana.AgregaLinea(ref textoArchivo, "PARAMETER DispCmb(DComb)");
            UtilYupana.AgregaLinea(ref textoArchivo, "/");
            foreach (var reg in listaSubRestric)
            {
                sLine = string.Empty;
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.DispComb);
                if (idGams > 0)
                {
                    totalComb++;
                    sLine += ConstantesYupana.PrefijoDispComb + idGams + UtilYupana.WriteArchivoGams("") + reg.Srestdvalor1.ToString();
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                }
            }
            if (categoria != null)
                categoria.Total = totalComb;
            if (totalComb > 0)
                UtilYupana.AgregaLinea(ref textoArchivo, "/");
            else
            {
                UtilYupana.AgregaLinea(ref textoArchivo, ConstantesYupana.PrefijoDispComb + "1  0");
                UtilYupana.AgregaLinea(ref textoArchivo, "/");
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");

            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoDispComb, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea Gams de FCostoF
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        private void CrearGamsFCostoF(string ruta, int topcodi)
        {
            string textoArchivo = string.Empty;
            int nCortes = 0;
            int idGams = 0;
            string sLine = string.Empty;
            var embalses = GetListaEmbalseFCostoF(topcodi, ref nCortes);
            var lista = GetByCriteriaCpDetfcostofs(topcodi);
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE DatoVA(Cortes,Emb)");
            sLine = UtilYupana.WriteArchivoGams("");
            foreach (var reg in embalses)
            {
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.Embalse);
                sLine += UtilYupana.WriteArchivoGams(ConstantesYupana.PrefijoEmbalse + idGams.ToString());
            }

            UtilYupana.AgregaLinea(ref textoArchivo, sLine);

            foreach (var r in lista)
            {
                var fila = r.Detfcfvalores.Split(',');
                sLine = UtilYupana.WriteArchivoGams(fila[0]);
                for (var i = 2; i < fila.Count(); i++)
                {
                    sLine += UtilYupana.WriteArchivoGams(fila[i]);
                }
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }


            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE DatoRHS(Cortes,*)");
            sLine = UtilYupana.WriteArchivoGams("") + UtilYupana.WriteArchivoGams("RHS");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            foreach (var r in lista)
            {
                var fila = r.Detfcfvalores.Split(',');
                sLine = UtilYupana.WriteArchivoGams(fila[0]) + UtilYupana.WriteArchivoGams(fila[1]);
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            }

            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoFCostoF, ruta, textoArchivo);
        }

        /// <summary>
        /// Genera archivo VolumenGas dat
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topologia"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        private void CrearGamsVolumen(string ruta, int topologia, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            string prefijo = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "Table    Volmin(Emb,t)");
            ImprimirGamsRestric48Horizontal2(ref textoArchivo, ConstantesBase.SresVolMinEmb, (int)topologia, ConstantesYupana.PrefijoEmbalse, ConstantesBase.Embalse, iniEtapa, finEtapa);
            sLine = UtilYupana.WriteArchivoGams("");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            UtilYupana.AgregaLinea(ref textoArchivo, "Table    Volmax(Emb,t)");
            ImprimirGamsRestric48Horizontal2(ref textoArchivo, ConstantesBase.SresVolMaxEmb, (int)topologia, ConstantesYupana.PrefijoEmbalse, ConstantesBase.Embalse, iniEtapa, finEtapa);
            sLine = UtilYupana.WriteArchivoGams("");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            UtilYupana.AgregaLinea(ref textoArchivo, "Table    DefminEmb(Emb,t)");
            ImprimirGamsRestric48Horizontal2(ref textoArchivo, ConstantesBase.SresDefMinEmb, (int)topologia, ConstantesYupana.PrefijoEmbalse, ConstantesBase.Embalse, iniEtapa, finEtapa);
            sLine = UtilYupana.WriteArchivoGams("");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            UtilYupana.AgregaLinea(ref textoArchivo, "Table    DefmaxEmb(Emb,t)");
            ImprimirGamsRestric48Horizontal2(ref textoArchivo, ConstantesBase.SresDefMaxEmb, (int)topologia, ConstantesYupana.PrefijoEmbalse, ConstantesBase.Embalse, iniEtapa, finEtapa);
            sLine = UtilYupana.WriteArchivoGams("");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            UtilYupana.AgregaLinea(ref textoArchivo, "Table    RiegoEmb1(Emb,t) Riego en m3 por seg");
            ImprimirGamsRestric48Horizontal2(ref textoArchivo, ConstantesBase.SresCauRiegoEmb, (int)topologia, ConstantesYupana.PrefijoEmbalse, ConstantesBase.Embalse, iniEtapa, finEtapa);
            sLine = UtilYupana.WriteArchivoGams("");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoVolumenGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Genera archivo Reserva Urs dat
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topologia"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        /// <param name="topsinrsf"></param>
        private void CrearGamsReservaUrs(string ruta, int topologia, int iniEtapa, int finEtapa, int topsinrsf)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            string sLineHead1 = string.Empty;
            string sLineHead2 = string.Empty;
            var listaUrs = ListarURSporRSF(ConstantesBase.Urs, topologia);
            sLineHead1 = UtilYupana.WriteArchivoGams("");
            sLineHead2 = UtilYupana.WriteArchivoGams("");
            int idGamsUrs = 0;
            foreach (var reg in listaUrs)
            {
                idGamsUrs = FindIdGams(reg.Recurcodi, ConstantesBase.Urs);
                sLineHead2 += UtilYupana.WriteArchivoGams("URS" + idGamsUrs.ToString() + ".Tr1");
                sLineHead2 += UtilYupana.WriteArchivoGams("URS" + idGamsUrs.ToString() + ".Tr2");
                sLineHead1 += UtilYupana.WriteArchivoGams("URS" + idGamsUrs.ToString());
            }
            if (listaUrs.Count == 0)
            {
                sLineHead2 += UtilYupana.WriteArchivoGams("URS1.Tr1");
                sLineHead2 += UtilYupana.WriteArchivoGams("URS1.Tr2");
                sLineHead1 += UtilYupana.WriteArchivoGams("URS1");
            }
            string strTablaMinUp = "TABLE URSMinUp(t,URS)    potencia ofertada por URS minima hacia arriba";
            string strTablaMinDn = "TABLE URSMinDn(t,URS)    potencia ofertada por URS minima hacia abajo";
            string strTablaMaxUp = "TABLE URSMaxUp(t,URS,TramoURS)    potencia de cada tramo";
            string strTablaMaxDn = "TABLE URSMaxDn(t,URS,TramoURS)    potencia de cada tramo";
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, strTablaMinUp);
            UtilYupana.AgregaLinea(ref textoArchivo, sLineHead1);
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48VerticalURS(ref textoArchivo, ConstantesBase.SresSistResvSecUrsMinUp, topologia, listaUrs, iniEtapa, finEtapa, topsinrsf);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");

            UtilYupana.AgregaLinea(ref textoArchivo, strTablaMinDn);
            UtilYupana.AgregaLinea(ref textoArchivo, sLineHead1);
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48VerticalURS(ref textoArchivo, ConstantesBase.SresSistResvSecUrsMinDn, topologia, listaUrs, iniEtapa, finEtapa, topsinrsf);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");

            UtilYupana.AgregaLinea(ref textoArchivo, strTablaMaxUp);
            UtilYupana.AgregaLinea(ref textoArchivo, sLineHead2);
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48VerticalURSTr(ref textoArchivo, ConstantesBase.SresSistResvSecUrsTramo1Up, ConstantesBase.SresSistResvSecUrsTramo2Up, topologia, listaUrs, iniEtapa, finEtapa, topsinrsf);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");

            UtilYupana.AgregaLinea(ref textoArchivo, strTablaMaxDn);
            UtilYupana.AgregaLinea(ref textoArchivo, sLineHead2);
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48VerticalURSTr(ref textoArchivo, ConstantesBase.SresSistResvSecUrsTramo1Dn, ConstantesBase.SresSistResvSecUrsTramo2Dn, topologia, listaUrs, iniEtapa, finEtapa, topsinrsf);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoresURSGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo costURS.dat
        /// </summary>
        private void CrearGamsCostURS(string ruta, int topologia, int iniEtapa, int finEtapa, int topsinrsf)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            string sLineHead = string.Empty;
            var listaUrs = ListarURSporRSF(ConstantesBase.Urs, topologia).ToList();
            sLineHead = UtilYupana.WriteArchivoGams("");
            int idGamsUrs = 0;
            foreach (var reg in listaUrs)
            {
                idGamsUrs = FindIdGams(reg.Recurcodi, ConstantesBase.Urs);
                sLineHead += UtilYupana.WriteArchivoGams("URS" + idGamsUrs.ToString() + ".Tr1");
                sLineHead += UtilYupana.WriteArchivoGams("URS" + idGamsUrs.ToString() + ".Tr2");
            }
            if (listaUrs.Count == 0)
            {
                sLineHead += UtilYupana.WriteArchivoGams("URS1.Tr1");
                sLineHead += UtilYupana.WriteArchivoGams("URS1.Tr2");
            }
            string strTablaUp = "TABLE CostURSup(t,URS,TramoURS)   Asignacion del precio para la reserva hacia arriba";
            string strTablaDn = "TABLE CostURSDn(t,URS,TramoURS)   Asignacion del precio para la reserva hacia abajo";
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, strTablaUp);
            UtilYupana.AgregaLinea(ref textoArchivo, sLineHead);
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48VerticalURSTr(ref textoArchivo, ConstantesBase.SresSistResvSecUrsPrecioTramo1Up, ConstantesBase.SresSistResvSecUrsPrecioTramo2Up, topologia, listaUrs, iniEtapa, finEtapa, topsinrsf);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            UtilYupana.AgregaLinea(ref textoArchivo, strTablaDn);
            UtilYupana.AgregaLinea(ref textoArchivo, sLineHead);
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48VerticalURSTr(ref textoArchivo, ConstantesBase.SresSistResvSecUrsPrecioTramo1Dn, ConstantesBase.SresSistResvSecUrsPrecioTramo2Dn, topologia, listaUrs, iniEtapa, finEtapa, topsinrsf);
            sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoresCOSTURSGams, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo Forzada dat
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        private void CrearGamsForazada(string ruta, int topcodi, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE ForzadaUt(t,Ut)");
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48Vertical(ref textoArchivo, ConstantesBase.SRES_UNFOR_PT, topcodi, ConstantesYupana.PrefijoPlantaT, ConstantesBase.ModoT, iniEtapa, finEtapa);
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoForzadaUt, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo Forzada dat
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        public void CrearGamsForzadaYupanaContinuo(string ruta, int topcodi, List<CpMedicion48DTO> lista, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE ForzadaUt(t,Ut)");
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48VerticalYupanaContinuo(ref textoArchivo, lista, ConstantesBase.SRES_UNFOR_PT, topcodi, ConstantesYupana.PrefijoPlantaT, ConstantesBase.ModoT, iniEtapa, finEtapa);
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoForzadaUt2, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea el archivo Temperatura dat
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        private void CrearGamsTemper(string ruta, int topcodi, int iniEtapa, int finEtapa)
        {
            string textoArchivo = string.Empty;
            List<CpRecursoDTO> listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.ModoT);
            string sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE TemperT(Ut,t)");
            sLine = UtilYupana.WriteArchivoGams("");
            ImprimirGamsRestric48Horizontal(ref textoArchivo, ConstantesBase.SresEfecTempPt, topcodi, ConstantesYupana.PrefijoPlantaT, listaEquipo, iniEtapa, finEtapa);
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoTemper, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea elementos Gams de Grupo Prioridad
        /// </summary>
        /// <param name="topologia"></param>
        /// <param name="regCategoria"></param>
        private void CrearGamsGrupoprioridad(int topologia, CpCategoriaDTO regCategoria)
        {
            int idGams = 0;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.GrupoPrioridad);
            foreach (var reg in lista)
            {
                idGams++;
                SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.GrupoPrioridad, 0);
            }
            regCategoria.Total = lista.Count();
        }

        /// <summary>
        /// Crea elementos Gams de Ciclo Combinados
        /// </summary>
        /// <param name="topologia"></param>
        /// <param name="regCategoria"></param>
        private void CrearGamsCicloCombinado(int topologia, CpCategoriaDTO regCategoria)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.CicloCombinado);
            foreach (var reg in lista)
            {
                idGams++;
                SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.CicloCombinado, 0);
            }
            regCategoria.Total = lista.Count();
        }

        /// <summary>
        /// Crea archivo Región de Seguridad dat
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        /// <param name="categoria"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        /// <param name="listaPeriodoDetalle"></param>
        private void CrearGamsRegionSeguridad(string ruta, int? topcodi, CpCategoriaDTO categoria, int iniEtapa, int finEtapa, List<EtapaDTO> listaPeriodoDetalle)
        {
            string textoArchivo = string.Empty;
            int idGams = 0;
            string sLine = "";
            int iniregseg = 0;
            int finregseg = 0;
            DateTime fini = DateTime.MinValue;
            List<CpSubrestricdatDTO> listaSubRestric, listaSubRestric2;
            CpSubrestricdatDTO find = null;
            int dif = 0;
            var lista = ListarRecursosPorCategoriaGams(_lrecurso, categoria.Catcodi);
            listaSubRestric = (_lsubrestricdat != null) ? _lsubrestricdat.Where(x => x.Srestcodi == ConstantesBase.SresSistRegSeguridad && x.Indiceorden == 0).ToList() : null;
            listaSubRestric2 = (_lsubrestricdat != null) ? _lsubrestricdat.Where(x => x.Srestcodi == ConstantesBase.SresSistRegSeguridad && x.Indiceorden != 0).OrderBy(x => x.Recurcodi).ThenBy(x => x.Indiceorden).ToList() : null;
            fini = _escenario.Topfecha.AddDays(_escenario.Topinicio);
            if (listaSubRestric != null)
                foreach (var reg in lista)
                {
                    find = listaSubRestric.Find(x => x.Recurcodi == reg.Recurcodi);
                    if (find != null)
                    {
                        dif = (find.Srestfecha != null) ? (int)((DateTime)find.Srestfecha - fini).TotalDays * 24 : 0;
                        decimal delta = (listaPeriodoDetalle.Count > 0) ? listaPeriodoDetalle[0].Etpdelta : 1;
                        iniregseg = GetIndiceEtapa(listaPeriodoDetalle, (decimal)find.Srestdvalor1 + dif, (int)(_escenario.Topiniciohora / 2));
                        finregseg = GetIndiceEtapa(listaPeriodoDetalle, (decimal)find.Srestdvalor2 + dif, (int)(_escenario.Topiniciohora / 2));
                        if (finregseg > iniregseg)
                        {
                            idGams++;
                            SaveRecursoGams(reg.Recurcodi, idGams, ConstantesBase.RegionSeguridad, ConstantesBase.RegionSeguridad);
                        }
                    }
                }
            categoria.Total = idGams;
            string prefijo = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "TABLE  RegPoints(Region,Rest,*)");
            sLine = UtilYupana.WriteArchivoGams(string.Empty);
            sLine += UtilYupana.WriteArchivoGams("xo");
            sLine += UtilYupana.WriteArchivoGams("yo");
            sLine += UtilYupana.WriteArchivoGams("xf");
            sLine += UtilYupana.WriteArchivoGams("yf");
            sLine += UtilYupana.WriteArchivoGams("UpDn");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            prefijo = ConstantesYupana.PrefijoRegionSeg;
            int orden = 1;
            if (listaSubRestric2 != null)
                foreach (var reg in listaSubRestric2)
                {
                    idGams = FindIdGams(reg.Recurcodi, ConstantesBase.RegionSeguridad);
                    if (idGams != 0)
                    {
                        sLine = UtilYupana.WriteArchivoGams(prefijo + idGams.ToString() + ".Rest" + orden);
                        sLine += (reg.Srestdvalor1 != null) ? UtilYupana.WriteArchivoGams(reg.Srestdvalor1.ToString()) : UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                        sLine += (reg.Srestdvalor2 != null) ? UtilYupana.WriteArchivoGams(reg.Srestdvalor2.ToString()) : UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                        sLine += (reg.Srestdvalor3 != null) ? UtilYupana.WriteArchivoGams(reg.Srestdvalor3.ToString()) : UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                        sLine += (reg.Srestdvalor4 != null) ? UtilYupana.WriteArchivoGams(reg.Srestdvalor4.ToString()) : UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                        sLine += (reg.Srestdopcion != null) ? UtilYupana.WriteArchivoGams(reg.Srestdopcion.ToString()) : UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                        UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                        orden++;
                    }
                }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
            FileHelper.GenerarArchivo(ConstantesBase.NombreArchivoRegionSeguridad, ruta, textoArchivo);
        }

        /// <summary>
        /// Crea archivo Método Gams dat
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <param name="modo"></param>
        private void CrearMetodoGams(string ruta, int topcodi)
        {
            string textoArchivo = string.Empty;
            string opcion = string.Empty;
            var lista = GetByCriteriaCpParametros(topcodi);
            if (lista.Count > (ConstantesBase.IdTipoProceso - 1))
            {
                if (lista[ConstantesBase.IdTipoProceso - 1].Paramvalor != null)
                    opcion = lista[ConstantesBase.IdTipoProceso - 1].Paramvalor;
            }
            string sLine = string.Empty;
            UtilYupana.AgregaLinea(ref textoArchivo, "SCALAR");
            UtilYupana.AgregaLinea(ref textoArchivo, "*Paralelo = 0, Flujo Secuencial");
            UtilYupana.AgregaLinea(ref textoArchivo, "*Paralelo = 1, Flujo Paralelo");
            UtilYupana.AgregaLinea(ref textoArchivo, "Paralelo /" + opcion + "/ ;");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoLogGamsMetodo, ruta, textoArchivo);
        }

        /// <summary>
        /// Escribe definicion de variables arreglos de recurso en Set.inc
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="tipoEquipo"></param>
        /// <param name="categoria"></param>
        /// <param name="textoArchivo"></param>
        public void CrearArregloEquipo(int topcodi, int tipoEquipo, CpCategoriaDTO categoria, ref string textoArchivo)
        {
            string sLine = string.Empty;
            int idGams = 0;
            int idEquipo = 0;
            List<CruceIdGamsCoes> listaGams = new List<CruceIdGamsCoes>();
            List<CpRecursoDTO> lista = new List<CpRecursoDTO>();
            List<CruceIdGamsCoes> listaCopia = new List<CruceIdGamsCoes>();
            List<int> listOrdenCol = new List<int>();
            sLine = categoria.Catmatrizgams + " " + categoria.Catdescripcion;
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            switch (tipoEquipo)
            {
                case ConstantesBase.Embalse:
                    sLine = categoria.Catprefijo + "0";
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                    break;
            }
            sLine = string.Empty;
            if (tipoEquipo == ConstantesBase.Linea)
            {
                lista = ListarLineaGams(ConstantesBase.Linea, topcodi).OrderBy(x => x.Recurcodi).ToList();

                var listat2 = ListarLineaGams(ConstantesBase.Trafo2D, topcodi).ToList();
                int totTrafos2D = listat2.Count;
                var listat3 = ListarLineaGams(ConstantesBase.Trafo3D, topcodi).ToList();
                lista = lista.Union(listat2).Concat(listat3).ToList();
                listaGams = MapeoGamsEscenario.ListaMapeo.Where(x => x.Tipo == ConstantesBase.Linea).ToList();
                listaCopia = new List<CruceIdGamsCoes>();
                CruceIdGamsCoes reg;
                foreach (var r in listaGams)
                {
                    reg = new CruceIdGamsCoes();
                    reg.IdEquipo = r.IdEquipo;
                    reg.IdGams = r.IdGams;
                    reg.Tipo = r.Tipo2;
                    reg.Tipo2 = r.Tipo2;
                    reg.Ficticio = r.Ficticio;
                    listaCopia.Add(reg);
                }
            }
            else
            {
                lista = ListarRecursosPorCategoriaGams(_lrecurso, tipoEquipo);
                if (MapeoGamsEscenario.ListaMapeo != null)
                {
                    listaGams = MapeoGamsEscenario.ListaMapeo.Where(x => x.Tipo == tipoEquipo).ToList();
                    listaCopia = new List<CruceIdGamsCoes>();
                    listaCopia = listaGams;
                }

            }

            string strSinCarcEspecial;
            foreach (var reg in listaCopia)
            {
                if (reg.Ficticio != true)
                {
                    var find = lista.Find(x => x.Recurcodi == reg.IdEquipo && x.Catcodi == reg.Tipo);
                    if (find != null)
                    {
                        idEquipo = find.Recurcodi;
                        idGams = reg.IdGams;
                        sLine += UtilYupana.WriteArchivoGams(categoria.Catprefijo + idGams.ToString());
                        strSinCarcEspecial = Regex.Replace(find.Recurnombre.Normalize(NormalizationForm.FormD), @"[^a-zA-Z0-9 ]+", "");
                        sLine += strSinCarcEspecial;
                        UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                        sLine = string.Empty;
                    }
                }
                else
                {
                    var find = ReduccionBarra.ListaRecursoReduccion.Find(x => x.RecurReduccion == reg.IdEquipo);
                    if (find != null)
                    {
                        idEquipo = reg.IdEquipo;
                        idGams = reg.IdGams;
                        sLine += UtilYupana.WriteArchivoGams(categoria.Catprefijo + idGams.ToString());
                        sLine += Regex.Replace(find.Recurnombre.Normalize(NormalizationForm.FormD), @"[^a-zA-Z0-9 ]+", "");
                        UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                        sLine = string.Empty;
                    }
                }
            }
            sLine = "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
        }

        /// <summary>
        /// Crea cadena indice de control de número de días
        /// </summary>
        /// <param name="file"></param>
        private void CrearIndiceControlNroDias(ref string textoArchivo)
        {

            string sLine = "td  ";
            sLine += "/td1*td1000/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
        }

        /// <summary>
        ///  Genera definicion de variable en archivoo Set segun tipo de recurso
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        private string CrearDefVarCatGams(CpCategoriaDTO categoria)
        {
            string fila = string.Empty;
            if (categoria != null)
            {

                switch (categoria.Catcodi)
                {

                    case ConstantesBase.DispComb:
                    case ConstantesBase.Rsf:
                    case ConstantesBase.Urs:
                    case ConstantesBase.GrupoPrioridad:
                        fila = categoria.Catmatrizgams + " " + categoria.Catdescripcion +
    " " + ((categoria.Total == 0) ? "/" + categoria.Catprefijo + "1*" + categoria.Catprefijo + "1" + "/" : "/" + categoria.Catprefijo + "1*" + categoria.Catprefijo +
    categoria.Total.ToString() + "/");

                        break;
                    case ConstantesBase.Embalse:
                        fila = categoria.Catmatrizgams + " " + ((categoria.Total == 0) ? ConstantesBase.SufijoNulo : "") + " " + categoria.Catdescripcion +
    " " + ("/" + categoria.Catprefijo + "0*" + categoria.Catprefijo +
    categoria.Total.ToString() + "/");
                        break;
                    case ConstantesBase.RestricGener:
                    case ConstantesBase.GenerMeta:
                        fila = categoria.Catmatrizgams + " " + categoria.Catdescripcion +
    " " + ((categoria.Total == 0) ? "//" : "/" + categoria.Catprefijo + "1*" + categoria.Catprefijo +
    categoria.Total.ToString() + "/");
                        break;
                    default:
                        fila = categoria.Catmatrizgams + " " + ((categoria.Total == 0) ? ConstantesBase.SufijoNulo : "") + " " + categoria.Catdescripcion +
    " " + ((categoria.Total == 0) ? "/" + categoria.Catprefijo + "0*" + categoria.Catprefijo + "0" + "/" : "/" + categoria.Catprefijo + "1*" + categoria.Catprefijo +
    categoria.Total.ToString() + "/");
                        break;
                }
            }
            return fila;
        }

        /// <summary>
        /// Crea matriz set Urs Planta
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="topologia"></param>
        /// <param name="modo"></param>
        private void CrearSetsRsfUrsPlantas(ref string textoArchivo, int topologia)
        {
            string sLinersf_urs_up = "MapRSF_URSUp(RSF,URS) Asocia RSF con URS para subir /";
            string sLinersf_urs_dn = "MapRSF_URSDn(RSF,URS) Asocia RSF con URS para bajar /";
            string sLinersf = "MAPURS(RSF,URS) asocia RSF a URS /";
            string sLineursh = "MAPURSh(URS,Uh) asocia URS a Uh /";
            string sLineurst = "MAPURSt(URS,Ut) asocia URS a Ut / ";
            string sLine = "RSFh(RSF,URS,Uh) configuracion RSF hidro / ";
            List<CpGruporecursoDTO> listursplanta = new List<CpGruporecursoDTO>();
            List<CpGruporecursoDTO> listrsfurs = new List<CpGruporecursoDTO>();
            listursplanta = ListarGrupoRecursoFamiliaGams((int)topologia, ConstantesBase.Urs, true);
            listrsfurs = ListarGrupoRecursoFamiliaGams((int)topologia, ConstantesBase.Rsf, true);
            //RSF URS plantas Hidros
            foreach (var reg in listrsfurs)
            {
                int idRsfGams = FindIdGams(reg.Recurcodi, ConstantesBase.Rsf);
                int idUrsGams = FindIdGams(reg.Recurcodisicoes, ConstantesBase.Urs);
                sLinersf += ConstantesYupana.PrefijoRSF + idRsfGams.ToString() + "." + ConstantesYupana.PrefijoURS + idUrsGams.ToString() + ",";
                sLinersf_urs_up += ConstantesYupana.PrefijoRSF + idRsfGams.ToString() + "." + ConstantesYupana.PrefijoURS + idUrsGams.ToString() + ",";
                sLinersf_urs_dn += ConstantesYupana.PrefijoRSF + idRsfGams.ToString() + "." + ConstantesYupana.PrefijoURS + idUrsGams.ToString() + ",";
                var listurs = listursplanta.Where(x => x.Catcodisec == ConstantesBase.PlantaH && x.Recurcodi == reg.Recurcodisicoes).ToList();
                foreach (var g in listurs)
                {
                    int idPlantaHGams = FindIdGams(g.Recurcodisicoes, ConstantesBase.PlantaH);
                    sLine += ConstantesYupana.PrefijoRSF + idRsfGams.ToString() + "." + ConstantesYupana.PrefijoURS + idUrsGams.ToString() + "." + ConstantesYupana.PrefijoPlantaH + idPlantaHGams + ",";
                }
            }

            var listursh = listursplanta.Where(x => x.Catcodisec == ConstantesBase.PlantaH).ToList();
            foreach (var g in listursh)
            {
                int idPlantaHGams = FindIdGams(g.Recurcodisicoes, ConstantesBase.PlantaH);
                int idUrsGams = FindIdGams(g.Recurcodi, ConstantesBase.Urs);
                sLineursh += ConstantesYupana.PrefijoURS + idUrsGams.ToString() + "." + ConstantesYupana.PrefijoPlantaH + idPlantaHGams + ",";
            }

            if (listrsfurs.Count > 0)
            {
                sLinersf = sLinersf.Substring(0, sLinersf.Length - 1) + "/";
                sLinersf_urs_up = sLinersf_urs_up.Substring(0, sLinersf_urs_up.Length - 1) + "/";
                sLinersf_urs_dn = sLinersf_urs_dn.Substring(0, sLinersf_urs_dn.Length - 1) + "/";
            }
            else
            {
                sLinersf += ConstantesBase.Phantom + "/";
                sLinersf_urs_up += ConstantesBase.Phantom + "/";
                sLinersf_urs_dn += ConstantesBase.Phantom + "/";

            }
            UtilYupana.AgregaLinea(ref textoArchivo, sLinersf);
            UtilYupana.AgregaLinea(ref textoArchivo, sLinersf_urs_up);
            UtilYupana.AgregaLinea(ref textoArchivo, sLinersf_urs_dn);
            UtilYupana.AgregaLinea(ref textoArchivo, "");

            if (listursh.Count > 0)
            {
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
                sLineursh = sLineursh.Substring(0, sLineursh.Length - 1) + "/";
            }
            else
            {
                sLine += ConstantesBase.Phantom + "/";
                sLineursh += ConstantesBase.Phantom + "/";
            }
            UtilYupana.AgregaLinea(ref textoArchivo, sLineursh);
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            //RSF URS plantas Termicas

            sLine = "RSFt(RSF,URS,Ut) configuracion RSF termica / ";
            foreach (var reg in listrsfurs)
            {
                int idRsfGams = FindIdGams(reg.Recurcodi, ConstantesBase.Rsf);
                int idUrsGams = FindIdGams(reg.Recurcodisicoes, ConstantesBase.Urs);
                var listurs = listursplanta.Where(x => x.Catcodisec == ConstantesBase.ModoT && x.Recurcodi == reg.Recurcodisicoes).ToList();
                foreach (var g in listurs)
                {
                    int idPlantaTGams = FindIdGams(g.Recurcodisicoes, ConstantesBase.ModoT);
                    sLine += ConstantesYupana.PrefijoRSF + idRsfGams.ToString() + "." + ConstantesYupana.PrefijoURS + idUrsGams.ToString() + "." + ConstantesYupana.PrefijoPlantaT + idPlantaTGams + ",";
                }
            }

            var listurst = listursplanta.Where(x => x.Catcodisec == ConstantesBase.ModoT).ToList();
            foreach (var g in listurst)
            {
                int idPlantaTGams = FindIdGams(g.Recurcodisicoes, ConstantesBase.ModoT);
                int idUrsGams = FindIdGams(g.Recurcodi, ConstantesBase.Urs);
                sLineurst += ConstantesYupana.PrefijoURS + idUrsGams.ToString() + "." + ConstantesYupana.PrefijoPlantaT + idPlantaTGams + ",";

            }

            if (listurst.Count > 0)
            {
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
                sLineurst = sLineurst.Substring(0, sLineurst.Length - 1) + "/";
            }
            else
            {
                sLine += ConstantesBase.Phantom + "/";
                sLineurst += ConstantesBase.Phantom + "/";
            }
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            UtilYupana.AgregaLinea(ref textoArchivo, sLineurst);
            UtilYupana.AgregaLinea(ref textoArchivo, "");

        }

        /// <summary>
        /// Crea la variable MAPH,MAPT,MAPRER,MAPUNC en el Sets.ini
        /// </summary>
        /// <param name="file"></param>
        private void CrearSetsTopologiaNodoT(ref string textoArchivo, int topologia)
        {
            int idGams = 0;
            int idNodo = 0;
            int idNodoGams = 0;
            string sLine = string.Empty;
            sLine = "MAPH(Uh,N) asocia generadores hidro a nodos /";
            List<CpRecursoDTO> lista;
            lista = FactorySic.GetCpRecursoRepository().ListaRecursoXCategoriaInNodoT(ConstantesBase.PlantaH, (int)topologia, ConstantesBase.Terghidro).Where(x => x.Recurconsideragams == 1).OrderBy(x => x.Recurcodi).ToList();
            foreach (var reg in lista)
            {
                if (reg.RecNodoID != null)
                {
                    idGams = FindIdGams(reg.Recurcodi, ConstantesBase.PlantaH);
                    if (reg.RecNodoID != null)
                    {
                        idNodo = BuscarEquipoFicticio((int)reg.RecNodoID);
                        if (idNodo > 0)
                            idNodoGams = FindIdGams(idNodo, ConstantesBase.NodoTopologico);
                        else
                            idNodoGams = FindIdGams((int)reg.RecNodoID, ConstantesBase.NodoTopologico);
                    }
                    sLine += ConstantesYupana.PrefijoPlantaH + idGams.ToString() + "." + ConstantesYupana.PrefijoNodoTopologico + idNodoGams.ToString() + ",";
                }
            }
            if (lista.Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            // Configuracion  Planta T con Nodo T
            sLine = string.Empty;
            sLine = "MAPT(Ut,N) asocia generadores termicos a nodos  /";
            lista = FactorySic.GetCpRecursoRepository().ListaModoInNodoT(ConstantesBase.ModoT, (int)topologia, ConstantesBase.Tergtermico).Where(x => x.Recurconsideragams == 1).ToList();
            foreach (var reg in lista)
            {
                if (reg.RecNodoID != null)
                {
                    idGams = FindIdGams(reg.Recurcodi, ConstantesBase.ModoT);
                    idNodo = BuscarEquipoFicticio((int)reg.RecNodoID);
                    if (idNodo > 0)
                        idNodoGams = FindIdGams(idNodo, ConstantesBase.NodoTopologico);
                    else
                        idNodoGams = FindIdGams((int)reg.RecNodoID, ConstantesBase.NodoTopologico);
                    sLine += ConstantesYupana.PrefijoPlantaT + idGams.ToString() + "." + ConstantesYupana.PrefijoNodoTopologico + idNodoGams.ToString() + ",";
                }
            }
            if (lista.Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = string.Empty;
            sLine = "MAPUNC(Unc,N) asocia generadores NO Convencionales a nodos /";
            lista = FactorySic.GetCpRecursoRepository().ListaRecursoXCategoriaInNodoT(ConstantesBase.PlantaNoConvenO, (int)topologia, ConstantesBase.Terplantarer)
                    .Where(x => x.Recurconsideragams == 1).OrderBy(x => x.Recurcodi).ToList();
            foreach (var reg in lista)
            {
                if (reg.RecNodoID != null)
                {
                    idGams = FindIdGams(reg.Recurcodi, ConstantesBase.PlantaNoConvenO);
                    idNodo = BuscarEquipoFicticio((int)reg.RecNodoID);
                    if (idNodo > 0)
                        idNodoGams = FindIdGams(idNodo, ConstantesBase.NodoTopologico);
                    else
                        idNodoGams = FindIdGams((int)reg.RecNodoID, ConstantesBase.NodoTopologico);
                    sLine += ConstantesYupana.PrefijoPlantaOtros + idGams.ToString() + "." + ConstantesYupana.PrefijoNodoTopologico + idNodoGams.ToString() + ",";
                }
            }
            if (lista.Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
        }

        /// <summary>
        /// Crea la matriz de relacion Topologia Embalse en Set.inc
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="topologia"></param>
        private void CrearSetsTopologiaEmbalse(ref string textoArchivo, int topologia)
        {
            CpRelacionDTO findReg;
            List<CpRelacionDTO> listaTopEmb;
            int idGams = 0;
            int idHidGams = 0;
            string sLine = string.Empty;
            string datosViertePH = " ";
            string datosTurbinaPH = " ";
            string datosVierteEMB = " ";
            string datosTurbinaEMB = " ";
            sLine = string.Empty;
            sLine = "MAPHQ(Emb,Uh) asocia descarga embalse a hidro /";
            List<CpRecursoDTO> lista;
            lista = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.Embalse);
            var listaTpoHid = ListCpRelacion(ConstantesBase.Turbinamiento + "," + ConstantesBase.Vertimiento, topologia).Where(x => x.Recurconsideragams == 1).ToList();
            foreach (var reg in lista)
            {
                idGams = FindIdGams(reg.Recurcodi, reg.Catcodi);
                listaTopEmb = listaTpoHid.Where(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.PlantaH).ToList();

                foreach (var p in listaTopEmb)
                {
                    idHidGams = FindIdGams(p.Recurcodi2, ConstantesBase.PlantaH);
                    if (idHidGams > 0)
                    {
                        datosTurbinaPH += ConstantesYupana.PrefijoEmbalse + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoPlantaH + idHidGams.ToString() + ",";
                    }
                }

                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.PlantaH);
                if (findReg != null)
                {
                    idHidGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.PlantaH);
                    if (idHidGams > 0)
                    {
                        datosViertePH += ConstantesYupana.PrefijoEmbalse + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoPlantaH + idHidGams.ToString() + ",";
                    }
                }
                listaTopEmb = listaTpoHid.Where(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.Embalse).ToList();
                foreach (var p in listaTopEmb)
                {
                    idHidGams = FindIdGams(p.Recurcodi2, ConstantesBase.Embalse);
                    if (idHidGams > 0)
                    {
                        datosTurbinaEMB += ConstantesYupana.PrefijoEmbalse + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoEmbalse + idHidGams.ToString() + ",";
                    }
                }

                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.Embalse);
                if (findReg != null)
                {
                    idHidGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.Embalse);
                    if (idHidGams > 0)
                    {
                        datosVierteEMB += ConstantesYupana.PrefijoEmbalse + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoEmbalse + idHidGams.ToString() + ",";
                    }
                }
            }
            sLine += datosTurbinaPH;
            if (listaTpoHid.Where(x => x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.PlantaH).ToList().Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = "MAPHQV(Emb,Uh) asocia vertimiento embalse a hidro /" + datosViertePH;
            if (listaTpoHid.Where(x => x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.PlantaH).ToList().Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = "MAPturbem(Emb,Emb) asocia descarga embalse a embalse /" + datosTurbinaEMB;
            if (listaTpoHid.Where(x => x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.Embalse).ToList().Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = "MAPvertem(Emb,Emb) asocia vertimiento embalse a embalse /" + datosVierteEMB;
            if (listaTpoHid.Where(x => x.Catcodi1 == ConstantesBase.Embalse && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.Embalse).ToList().Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);

        }

        /// <summary>
        /// Crea la matriz de relacion Topologia Planta Hidro en Set.inc
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="topologia"></param>
        private void CrearSetsTopologiaPH(ref string textoArchivo, int topologia)
        {
            CpRelacionDTO findReg;
            int idGams = 0;
            string sLine = string.Empty;
            string datosViertePH = " ";
            string datosTurbinaPH = " ";
            string datosVierteEMB = " ";
            string datosTurbinaEMB = " ";
            sLine = "MAPturb(Uh,Uh) asocia turbinamiento hidro a hidro /";
            var listphnt = ListarRecursosPorCategoriaGams(_lrecurso, ConstantesBase.PlantaH);
            var listaTpoHid = ListCpRelacion(ConstantesBase.Turbinamiento + "," + ConstantesBase.Vertimiento, topologia);
            foreach (var reg in listphnt)
            {
                idGams = FindIdGams(reg.Recurcodi, reg.Catcodi);
                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.PlantaH);

                if (findReg != null)
                {
                    int idTurbPHGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.PlantaH);
                    if (idTurbPHGams > 0)
                    {
                        datosTurbinaPH += ConstantesYupana.PrefijoPlantaH + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoPlantaH + idTurbPHGams.ToString() + ",";
                    }
                }
                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.PlantaH);

                if (findReg != null)
                {
                    int idViertePHGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.PlantaH);
                    if (idViertePHGams > 0)
                    {
                        datosViertePH += ConstantesYupana.PrefijoPlantaH + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoPlantaH + idViertePHGams.ToString() + ",";
                    }
                }
                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.Embalse);

                if (findReg != null)
                {
                    int idTurbEmbGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.Embalse);
                    if (idTurbEmbGams > 0)
                    {
                        datosTurbinaEMB += ConstantesYupana.PrefijoPlantaH + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoEmbalse + idTurbEmbGams.ToString() + ",";
                    }
                }
                findReg = listaTpoHid.Find(x => x.Recurcodi1 == reg.Recurcodi && x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.Embalse);

                if (findReg != null)
                {
                    int idVierteEmbGams = FindIdGams(findReg.Recurcodi2, ConstantesBase.Embalse);
                    if (idVierteEmbGams > 0)
                    {
                        datosVierteEMB += ConstantesYupana.PrefijoPlantaH + idGams.ToString() + "." +
                            ConstantesYupana.PrefijoEmbalse + idVierteEmbGams.ToString() + ",";
                    }
                }
            }
            sLine += datosTurbinaPH;
            if (listaTpoHid.Where(x => x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.PlantaH).ToList().Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            //
            sLine = "MAPvert(Uh,Uh) asocia vertimiento hidro a hidro /" + datosViertePH;
            if (listaTpoHid.Where(x => x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.PlantaH).ToList().Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            //
            sLine = "MAPturbh(Uh,Emb) asocia turbinamiento hidro a embalse /" + datosTurbinaEMB;

            if (listaTpoHid.Where(x => x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Turbinamiento && x.Catcodi2 == ConstantesBase.Embalse).ToList().Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            //
            sLine = "MAPverth(Uh,Emb) asocia vertimiento hidro a embalse /" + datosVierteEMB;

            if (listaTpoHid.Where(x => x.Catcodi1 == ConstantesBase.PlantaH && x.Cptrelcodi == ConstantesBase.Vertimiento && x.Catcodi2 == ConstantesBase.Embalse).ToList().Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            //            
        }

        /// <summary>
        /// Crea la matriz de relacion Restricciones de Generación en Set.inc
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="topologia"></param>
        private void CrearSetsRestricGener(ref string textoArchivo, int topologia)
        {
            int idGamsRestrGen, idGamsPH, idGamsPT;
            int contMayH, contMinH, contIguH;
            int contMayT, contMinT, contIguT;
            string sLineMayH, sLineMinH, sLineIguH;
            string sLineMayT, sLineMinT, sLineIguT;
            CpSubrestricdatDTO regRecid;
            var lista = ListarGrupoRecursoFamiliaGams(topologia, ConstantesBase.RestricGener, true);
            var listah = lista.Where(x => x.Catcodisec == ConstantesBase.PlantaH).OrderBy(x => x.Recurcodisicoes).ToList();
            var listaSrestric = ListaCpSubRestriccionDat(topologia);
            var listaSubRestric = listaSrestric.Where(x => x.Srestcodi == ConstantesBase.SresSisRestricGener).ToList();
            sLineMayH = "ResmayH(Resgen,Uh) asocia generadores hidro a la restriccion de generacion /";
            sLineMinH = "ResmenH(Resgen,Uh) asocia generadores hidro a la restriccion de generacion /";
            sLineIguH = "ResiguH(Resgen,Uh) asocia generadores hidro a la restriccion de generacion /";
            contMayH = contMinH = contIguH = 0;
            foreach (var reg in listah)
            {
                idGamsRestrGen = FindIdGams(reg.Recurcodi, ConstantesBase.RestricGener);
                idGamsPH = FindIdGams(reg.Recurcodisicoes, ConstantesBase.PlantaH);
                regRecid = listaSubRestric.Find(x => x.Recurcodi == reg.Recurcodi && x.Srestdopcion == ConstantesBase.MayorRestric);
                if (regRecid != null)
                {
                    sLineMayH += ConstantesYupana.PrefijoResGen + idGamsRestrGen + "." + ConstantesYupana.PrefijoPlantaH + idGamsPH + ",";
                    contMayH++;
                }
                regRecid = listaSubRestric.Find(x => x.Recurcodi == reg.Recurcodi && x.Srestdopcion == ConstantesBase.MenorRestric);
                if (regRecid != null)
                {
                    sLineMinH += ConstantesYupana.PrefijoResGen + idGamsRestrGen + "." + ConstantesYupana.PrefijoPlantaH + idGamsPH + ",";
                    contMinH++;
                }
                regRecid = listaSubRestric.Find(x => x.Recurcodi == reg.Recurcodi && x.Srestdopcion == ConstantesBase.IgualRestric);
                if (regRecid != null)
                {
                    sLineIguH += ConstantesYupana.PrefijoResGen + idGamsRestrGen + "." + ConstantesYupana.PrefijoPlantaH + idGamsPH + ",";
                    contIguH++;
                }
            }
            if (contMayH > 0)
                sLineMayH = sLineMayH.Substring(0, sLineMayH.Length - 1) + "/";
            else
                sLineMayH += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLineMayH);
            if (contMinH > 0)
                sLineMinH = sLineMinH.Substring(0, sLineMinH.Length - 1) + "/";
            else
                sLineMinH += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLineMinH);
            if (contIguH > 0)
                sLineIguH = sLineIguH.Substring(0, sLineIguH.Length - 1) + "/";
            else
                sLineIguH += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLineIguH);

            ///// Termicas
            var listat = lista.Where(x => x.Catcodisec == ConstantesBase.ModoT).OrderBy(x => x.Recurcodisicoes).ToList();
            sLineMayT = "ResmayT(Resgen,Ut) asocia generadores termicos a la restriccion de generacion /";
            sLineMinT = "ResmenT(Resgen,Ut) asocia generadores termicos a la restriccion de generacion /";
            sLineIguT = "ResiguT(Resgen,Ut) asocia generadores termicos a la restriccion de generacion /";
            contMayT = contMinT = contIguT = 0;
            foreach (var reg in listat)
            {
                idGamsRestrGen = FindIdGams(reg.Recurcodi, ConstantesBase.RestricGener);
                idGamsPT = FindIdGams(reg.Recurcodisicoes, ConstantesBase.ModoT);
                regRecid = listaSubRestric.Find(x => x.Recurcodi == reg.Recurcodi && x.Srestdopcion == ConstantesBase.MayorRestric);
                if (regRecid != null)
                {
                    sLineMayT += ConstantesYupana.PrefijoResGen + idGamsRestrGen + "." + ConstantesYupana.PrefijoPlantaT + idGamsPT + ",";
                    contMayT++;
                }
                regRecid = listaSubRestric.Find(x => x.Recurcodi == reg.Recurcodi && x.Srestdopcion == ConstantesBase.MenorRestric);
                if (regRecid != null)
                {
                    sLineMinT += ConstantesYupana.PrefijoResGen + idGamsRestrGen + "." + ConstantesYupana.PrefijoPlantaT + idGamsPT + ",";
                    contMinT++;
                }
                regRecid = listaSubRestric.Find(x => x.Recurcodi == reg.Recurcodi && x.Srestdopcion == ConstantesBase.IgualRestric);
                if (regRecid != null)
                {
                    sLineIguT += ConstantesYupana.PrefijoResGen + idGamsRestrGen + "." + ConstantesYupana.PrefijoPlantaT + idGamsPT + ",";
                    contIguT++;
                }
            }
            if (contMayT > 0)
                sLineMayT = sLineMayT.Substring(0, sLineMayT.Length - 1) + "/";
            else
                sLineMayT += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLineMayT);
            if (contMinT > 0)
                sLineMinT = sLineMinT.Substring(0, sLineMinT.Length - 1) + "/";
            else
                sLineMinT += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLineMinT);
            if (contIguT > 0)
                sLineIguT = sLineIguT.Substring(0, sLineIguT.Length - 1) + "/";
            else
                sLineIguT += "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLineIguT);

        }

        /// <summary>
        /// Crea la matriz de relacion Suma de Flujos en Set.inc
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="topologia"></param>
        private void CrearSetsSumaFlujo(ref string textoArchivo, int topologia)
        {
            string sLine = "SetSumF(sumf,L) asocia lineas con restriccion de suma de flujos /";
            int idGamsSumF, idGamsL;
            var lista = ListarGrupoRecursoFamiliaGams(topologia, ConstantesBase.SumFlujo, true);
            foreach (var reg in lista)
            {
                idGamsSumF = FindIdGams(reg.Recurcodi, ConstantesBase.SumFlujo);
                idGamsL = FindIdGams(reg.Recurcodisicoes, reg.Catcodisec);
                sLine += ConstantesYupana.PrefijoSumFlujo + idGamsSumF + "." + ConstantesYupana.PrefijoLinea + idGamsL + ",";
            }
            if (lista.Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
        }

        /// <summary>
        /// Crea la matriz de relacion Disponibilidad de Combustible en Set.inc
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="topologia"></param>
        /// <param name="listaPeriodoDetalle"></param>
        /// <param name="iniEtapa"></param>
        private void CrearSetsDispComb(ref string textoArchivo, int topologia, List<EtapaDTO> listaPeriodoDetalle, int iniEtapa)
        {
            int idGams, idGamsPT;
            decimal delta = (listaPeriodoDetalle.Count > 0) ? listaPeriodoDetalle[0].Etpdelta : 1;
            string sLine = "SetDcomb(DComb,Ut) asocia Disponibilidad de Combustibles con centrales termicas /";
            var lista = ListarGrupoRecursoFamiliaGams(topologia, ConstantesBase.DispComb, true);
            int totalReg = 0;
            foreach (var reg in lista)
            {
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.DispComb);
                if (idGams != 0)
                {
                    idGamsPT = FindIdGams(reg.Recurcodisicoes, ConstantesBase.ModoT);
                    sLine += ConstantesYupana.PrefijoDispComb + idGams + "." + ConstantesYupana.PrefijoPlantaT + idGamsPT + ",";
                    totalReg++;
                }
            }
            if (totalReg > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            var listaSrestric = ListaCpSubRestriccionDat(topologia);
            var listaSubRestric = listaSrestric.Where(x => x.Srestcodi == ConstantesBase.SresDispComb).ToList();
            sLine = "SetComb(DComb,t) conjunto de disponibilidad de combustible /";
            int totalComb = 0;
            int iniComb = 0;
            int finComb = 0;
            int iniDComb = 0;
            int finDComb = 0;
            foreach (var reg in listaSubRestric)
            {
                idGams = FindIdGams(reg.Recurcodi, ConstantesBase.DispComb);
                if (idGams > 0)
                {
                    if (reg.Srestdvalor2 != null && reg.Srestdvalor3 != null)
                    {
                        totalComb++;
                        iniComb = GetIndiceEtapa(listaPeriodoDetalle, (decimal)reg.Srestdvalor2, (int)(_escenario.Topiniciohora / 2));
                        finComb = GetIndiceEtapa(listaPeriodoDetalle, (decimal)reg.Srestdvalor3, (int)(_escenario.Topiniciohora / 2));
                        iniDComb = (iniComb <= 0) ? 1 : iniComb;
                        finDComb = (finComb <= 0) ? 1 : finComb;
                        sLine += ConstantesYupana.PrefijoDispComb + idGams + ".(" + iniDComb.ToString() + "*" + finDComb.ToString() + "),";
                    }

                }
            }
            if (totalComb > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
        }

        /// <summary>
        /// Crea la matriz de relacion Generación Meta en Set.inc
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="topologia"></param>
        /// <param name="listaPeriodoDetalle"></param>
        /// <param name="iniEtapa"></param>
        private void CrearSetsGenerMeta(ref string textoArchivo, int topologia, List<EtapaDTO> listaPeriodoDetalle, int iniEtapa)
        {
            int idGamsGenMeta, idGamsPH, idGamsPT;
            decimal delta = (listaPeriodoDetalle.Count > 0) ? listaPeriodoDetalle[0].Etpdelta : 1;
            string sLine = "GenMetaH(GenMeta,Uh) asocia generadores hidro a generacion meta /";
            var lista = ListarGrupoRecursoFamiliaGams((int)topologia, ConstantesBase.GenerMeta, true);
            var listah = lista.Where(x => x.Catcodisec == ConstantesBase.PlantaH).OrderBy(x => x.Recurcodisicoes).ToList();
            var listaSrestric = ListaCpSubRestriccionDat(topologia);
            var listaSubRestric = listaSrestric.Where(x => x.Srestcodi == ConstantesBase.SresGenMeta).ToList();
            int totalGen = 0;
            foreach (var reg in listah)
            {
                idGamsGenMeta = FindIdGams(reg.Recurcodi, ConstantesBase.GenerMeta);
                if (idGamsGenMeta > 0)
                {
                    totalGen++;
                    idGamsPH = FindIdGams(reg.Recurcodisicoes, ConstantesBase.PlantaH);
                    sLine += ConstantesYupana.PrefijoGenMeta + idGamsGenMeta + "." + ConstantesYupana.PrefijoPlantaH + idGamsPH + ",";
                }
            }
            if (totalGen > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            ///// Termicas
            sLine = "GenMetaT(GenMeta,Ut) asocia generadores termicos a generacion meta /";
            var listat = lista.Where(x => x.Catcodisec == ConstantesBase.ModoT).OrderBy(x => x.Recurcodisicoes).ToList();
            totalGen = 0;
            foreach (var reg in listat)
            {
                idGamsGenMeta = FindIdGams(reg.Recurcodi, ConstantesBase.GenerMeta);
                if (idGamsGenMeta > 0)
                {
                    totalGen++;
                    idGamsPT = FindIdGams(reg.Recurcodisicoes, ConstantesBase.ModoT);
                    sLine += ConstantesYupana.PrefijoGenMeta + idGamsGenMeta + "." + ConstantesYupana.PrefijoPlantaT + idGamsPT + ",";
                }
            }
            if (totalGen > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = "SetMeta(GenMeta,t) conjunto de generacion meta /";
            totalGen = 0;
            int inigenMeta = 0;
            int fingenMeta = 0;
            foreach (var reg in listaSubRestric)
            {
                idGamsGenMeta = FindIdGams(reg.Recurcodi, ConstantesBase.GenerMeta);
                if (idGamsGenMeta > 0)
                {
                    totalGen++;
                    int iniMeta = GetIndiceEtapa(listaPeriodoDetalle, (decimal)reg.Srestdvalor3, (int)(_escenario.Topiniciohora / 2));
                    int finMeta = GetIndiceEtapa(listaPeriodoDetalle, (decimal)reg.Srestdvalor4, (int)(_escenario.Topiniciohora / 2));
                    inigenMeta = (iniMeta <= 0) ? 1 : iniMeta;
                    fingenMeta = (finMeta <= 0) ? 1 : finMeta;
                    sLine += ConstantesYupana.PrefijoGenMeta + idGamsGenMeta + ".(" + inigenMeta.ToString() + "*" + fingenMeta.ToString() + "),";
                }
            }
            if (totalGen > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
        }

        /// <summary>
        /// Crea Set de Grupo Prioridad
        /// </summary>
        /// <param name="file"></param>
        /// <param name="topologia"></param>
        /// <param name="modo"></param>
        private void CrearSetGrupoprioridad(ref string textoArchivo, int topologia)
        {
            string sLine = "SetPrior(GrupPrior,Prioridad,Ut) /";
            int idGrupo, idTermica, prioridad;
            var lista = ListarGrupoRecursoFamiliaGams(topologia, ConstantesBase.GrupoPrioridad, true);
            int total = 0;
            foreach (var reg in lista)
            {
                idTermica = FindIdGams(reg.Recurcodisicoes, ConstantesBase.ModoT);
                prioridad = (int)reg.Grurecorden;
                idGrupo = FindIdGams(reg.Recurcodi, ConstantesBase.GrupoPrioridad);

                if (idGrupo > 0)
                {
                    sLine += "Grup" + idGrupo.ToString() + ".Prior" + prioridad.ToString() + "." + ConstantesYupana.PrefijoPlantaT + idTermica.ToString() + ",";
                    total++;
                }
            }

            if (total > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
        }

        /// <summary>
        /// Crea Set de Ciclo Combinado
        /// </summary>
        /// <param name="file"></param>
        /// <param name="topologia"></param>
        /// <param name="modo"></param>
        private void CrearSetCicloCombinado(ref string textoArchivo, int topologia, CpCategoriaDTO categoria)
        {

            int total = categoria.Total;
            string sLine = "";
            if (total == 0)
                sLine = "NumCC Numero de Ciclos Combinados modelados /cc0*cc0/";
            else
                sLine = "NumCC Numero de Ciclos Combinados modelados /cc1*cc" + total.ToString() + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = "MapCC_Ut(NumCC,Ut) asocia el CC a las unidades termicas modeladas /";

            int idCC, idTermica;
            var lista = ListarGrupoRecursoFamiliaGams(topologia, ConstantesBase.CicloCombinado, true);
            foreach (var reg in lista)
            {
                idTermica = FindIdGams(reg.Recurcodisicoes, ConstantesBase.ModoT);
                if (idTermica != 0)
                {
                    idCC = FindIdGams(reg.Recurcodi, ConstantesBase.CicloCombinado);
                    sLine += ConstantesYupana.PrefijoCComb + idCC.ToString() + "." + ConstantesYupana.PrefijoPlantaT + idTermica.ToString() + ",";
                }
            }
            if (lista.Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = "MAPEDe(Ut,Ut) Modos Destinos /";
            var listaM = ListCpRelacion(ConstantesBase.Transicion.ToString(), topologia);
            int idmodoSup, idmodoInf;
            foreach (var p in listaM)
            {
                idmodoInf = FindIdGams(p.Recurcodi1, ConstantesBase.ModoT);
                idmodoSup = FindIdGams(p.Recurcodi2, ConstantesBase.ModoT);
                if (idmodoInf != 0 && idmodoSup != 0)
                    sLine += ConstantesYupana.PrefijoPlantaT + idmodoInf.ToString() + "." + ConstantesYupana.PrefijoPlantaT + idmodoSup.ToString() + ",";
            }
            if (listaM.Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
        }

        /// <summary>
        /// Crea Set de Regiones de Seguridad
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="topologia"></param>
        /// <param name="categoria"></param>
        /// <param name="listaPeriodoDetalle"></param>
        /// <param name="iniEtapa"></param>
        private void CrearSetRegionSeguridad(ref string textoArchivo, int topologia, CpCategoriaDTO categoria, List<EtapaDTO> listaPeriodoDetalle, int iniEtapa)
        {
            int total = categoria.Total;
            int idRegSeg;
            string sLine = "";
            List<CpSubrestricdatDTO> listaSubrest, lequipo;
            lequipo = (_lsubrestricdat != null) ? _lsubrestricdat.Where(x => x.Srestcodi == ConstantesBase.SresSistRegSeguridad && x.Indiceorden == 0).ToList() : null;
            listaSubrest = (_lsubrestricdat != null) ? _lsubrestricdat.Where(x => x.Srestcodi == ConstantesBase.SresSistRegSeguridad && x.Indiceorden != 0).OrderBy(x => x.Recurcodi).ThenBy(x => x.Indiceorden).ToList() : null;
            sLine = "RegRest(Region,Rest) /";
            int totalReg = 0;
            int indice = 1;
            foreach (var reg in listaSubrest)
            {
                idRegSeg = FindIdGams(reg.Recurcodi, ConstantesBase.RegionSeguridad);
                if (idRegSeg > 0)
                {
                    sLine += categoria.Catprefijo + idRegSeg + "." + "Rest" + indice + ",";
                    totalReg++;
                    indice++;
                }

            }
            var sLine2 = (totalReg == 0) ? "Rest //" : "Rest /Rest1*Rest" + totalReg + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine2);
            if (totalReg > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = "SetRegEta(Region,t) /";
            totalReg = 0;
            DateTime fini = DateTime.MinValue;
            int iniRegSeg = 0;
            int finRegSeg = 0;
            fini = _escenario.Topfecha.AddDays(_escenario.Topinicio);
            int dif = 0;
            foreach (var reg in lequipo)
            {
                dif = (reg.Srestfecha != null) ? (int)((DateTime)reg.Srestfecha - fini).TotalDays * 24 : 0;
                idRegSeg = FindIdGams(reg.Recurcodi, ConstantesBase.RegionSeguridad);
                if (idRegSeg > 0)
                {
                    totalReg++;
                    int iniRegS = GetIndiceEtapa(listaPeriodoDetalle, (decimal)reg.Srestdvalor1 + dif, (int)(_escenario.Topiniciohora / 2));
                    int finRegS = GetIndiceEtapa(listaPeriodoDetalle, (decimal)reg.Srestdvalor2 + dif, (int)(_escenario.Topiniciohora / 2));
                    iniRegSeg = (iniRegS <= 0) ? 1 : iniRegS;
                    finRegSeg = (finRegS <= 0) ? 1 : finRegS;
                    sLine += ConstantesYupana.PrefijoRegionSeg + idRegSeg + ".(" + iniRegSeg.ToString() + "*" + finRegSeg.ToString() + "),";
                }
            }
            if (totalReg > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);

            sLine = "GenHidReg(Region,Uh) /";
            // Plantas Hidros
            var lista = ListarGrupoRecursoFamiliaGams(topologia, ConstantesBase.RegionSeguridad, true);
            var listah = lista.Where(x => x.Catcodisec == ConstantesBase.PlantaH).OrderBy(x => x.Recurcodisicoes).ToList();
            int totalGen = 0;
            int idGamsPH = 0;
            foreach (var reg in listah)
            {
                idRegSeg = FindIdGams(reg.Recurcodi, ConstantesBase.RegionSeguridad);
                if (idRegSeg > 0)
                {
                    totalGen++;
                    idGamsPH = FindIdGams(reg.Recurcodisicoes, ConstantesBase.PlantaH);
                    sLine += ConstantesYupana.PrefijoRegionSeg + idRegSeg + "." + ConstantesYupana.PrefijoPlantaH + idGamsPH + ",";
                }
            }
            if (totalGen > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);

            sLine = "GenTerReg(Region,Ut) /";
            // Plantas Termicas
            var listat = lista.Where(x => x.Catcodisec == ConstantesBase.ModoT).OrderBy(x => x.Recurcodisicoes).ToList();
            totalGen = 0;
            int idGamsPt = 0;
            foreach (var reg in listat)
            {
                idRegSeg = FindIdGams(reg.Recurcodi, ConstantesBase.RegionSeguridad);
                if (idRegSeg > 0)
                {
                    totalGen++;
                    idGamsPt = FindIdGams(reg.Recurcodisicoes, ConstantesBase.ModoT);
                    sLine += ConstantesYupana.PrefijoRegionSeg + idRegSeg + "." + ConstantesYupana.PrefijoPlantaT + idGamsPt + ",";
                }
            }
            if (totalGen > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = "EquipoReg(Region,L) /";
            // Lineas Trafos2D,Trafos3D
            var listal = lista.Where(x => x.Catcodisec == ConstantesBase.Linea).OrderBy(x => x.Recurcodisicoes).ToList();
            totalGen = 0;
            int idGamsL = 0;
            foreach (var reg in listal)
            {
                idRegSeg = FindIdGams(reg.Recurcodi, ConstantesBase.RegionSeguridad);
                if (idRegSeg > 0)
                {
                    totalGen++;
                    idGamsL = FindIdGams(reg.Recurcodisicoes, ConstantesBase.Linea);
                    sLine += ConstantesYupana.PrefijoRegionSeg + idRegSeg + "." + ConstantesYupana.PrefijoLinea + idGamsL + ",";
                }
            }
            if (totalGen > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
        }

        /// <summary>
        /// Crea el archivo Sets.ini
        /// </summary>
        /// <param name="topcodi"></param>
        private void CrearGamsSets(string ruta, int topcodi, List<CpCategoriaDTO> listaCategoria, int iniEtapa, List<EtapaDTO> listaPeriodoDetalle)
        {
            string textoArchivo = string.Empty;
            string sLine = string.Empty;
            CpCategoriaDTO categoria;
            UtilYupana.AgregaLinea(ref textoArchivo, "$onempty");
            UtilYupana.AgregaLinea(ref textoArchivo, "SETS");
            CrearIndiceControlNroDias(ref textoArchivo);
            //Declaracion de Embalses
            string sufijoNulo = string.Empty;
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.Embalse);
            CrearArregloEquipo(topcodi, ConstantesBase.Embalse, categoria, ref textoArchivo);
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.PlantaH);
            CrearArregloEquipo(topcodi, ConstantesBase.PlantaH, categoria, ref textoArchivo);
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.ModoT);
            CrearArregloEquipo(topcodi, ConstantesBase.ModoT, categoria, ref textoArchivo);
            UtilYupana.AgregaLinea(ref textoArchivo, "tramo  Maximo tramos de la curva de costos termica /1*5/"); /// /1*5/ Se debe encontrar el maximo nro de parejas(Potencia,Consumo)
            UtilYupana.AgregaLinea(ref textoArchivo, "Segmento Maximo tramos de la curva de perdidas de las lineas /Sg1*Sg3/");
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.PlantaNoConvenO);
            CrearArregloEquipo(topcodi, ConstantesBase.PlantaNoConvenO, categoria, ref textoArchivo);
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.NodoTopologico);
            CrearArregloEquipo(topcodi, ConstantesBase.NodoTopologico, categoria, ref textoArchivo);
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.Linea);
            int nlineas = categoria.Total;
            CrearArregloEquipo(topcodi, ConstantesBase.Linea, categoria, ref textoArchivo);

            /// definicion de Estricciondes de Generacion
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.RestricGener);
            UtilYupana.AgregaLinea(ref textoArchivo, CrearDefVarCatGams(categoria));
            /// definicion de Generacion Meta
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.GenerMeta);
            UtilYupana.AgregaLinea(ref textoArchivo, CrearDefVarCatGams(categoria));
            // definicion Suma de flujos
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.SumFlujo);
            UtilYupana.AgregaLinea(ref textoArchivo, CrearDefVarCatGams(categoria));
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.DispComb);
            UtilYupana.AgregaLinea(ref textoArchivo, CrearDefVarCatGams(categoria));
            /// definicion de Generacion Meta
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.Urs);
            CrearArregloEquipo(topcodi, ConstantesBase.Urs, categoria, ref textoArchivo);
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.Rsf);
            UtilYupana.AgregaLinea(ref textoArchivo, CrearDefVarCatGams(categoria));
            UtilYupana.AgregaLinea(ref textoArchivo, "TramoURS Numero de tramos de la oferta URS /Tr1*Tr2/");
            CrearSetsRsfUrsPlantas(ref textoArchivo, topcodi);
            // Configuracion  Topologia Nodo T
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            CrearSetsTopologiaNodoT(ref textoArchivo, topcodi);
            // Configuracion  Topologia Hidro - Origen Embalse
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            CrearSetsTopologiaEmbalse(ref textoArchivo, topcodi);
            UtilYupana.AgregaLinea(ref textoArchivo, "");
            CrearSetsTopologiaPH(ref textoArchivo, topcodi);
            var parametros = GetByCriteriaCpParametros(topcodi);
            if (parametros != null)
            {
                var find = parametros.Find(x => x.Paramcodi == ConstantesBase.NumIteraciones);
                if (find != null)
                    UtilYupana.AgregaLinea(ref textoArchivo, "iter  maximo numero de iteraciones de Benders /iter1*iter" + find.Paramvalor + "/"); // Dato de Parametro de Ejecucion
                else
                    UtilYupana.AgregaLinea(ref textoArchivo, "iter  maximo numero de iteraciones de Benders /" + ConstantesBase.Phantom + "/"); // Dato de Parametro de Ejecucion
            }
            var fCostoF = GetByIdCpFcostof(topcodi);
            if (fCostoF != null)
            {
                if (fCostoF.Fcfnumcortes != null)
                {
                    UtilYupana.AgregaLinea(ref textoArchivo, "Cortes Numero de cortes de Benders generados por el modelo SDDP /1*" + ((fCostoF.Fcfnumcortes > 0) ? fCostoF.Fcfnumcortes.ToString() : "1") + "/"); // CArga del Del archivo Dat en restricciones si no hay restricciones null, el numero de filas del archivo define el total del arreglo
                }
                else
                    UtilYupana.AgregaLinea(ref textoArchivo, "Cortes Numero de cortes de Benders generados por el modelo SDDP /1*1/"); // CArga del Del archivo Dat en restricciones si no hay restricciones null, el numero de filas del archivo define el total del arreglo
            }
            else
                UtilYupana.AgregaLinea(ref textoArchivo, "Cortes Numero de cortes de Benders generados por el modelo SDDP /1*1/"); // CArga del Del archivo Dat en restricciones si no hay restricciones null, el numero de filas del archivo define el total del arreglo
            UtilYupana.AgregaLinea(ref textoArchivo, "dyniter(iter) subconjunto dinamico"); // Cadena fija
            CrearSetsRestricGener(ref textoArchivo, topcodi);
            CrearSetsGenerMeta(ref textoArchivo, topcodi, listaPeriodoDetalle, iniEtapa);
            UtilYupana.AgregaLinea(ref textoArchivo, "SetArrdH(Uh,t) conjunto de arranques diarios hidro //");// Uh10.1*24/");// dividir grupos por dia para cada CH ( uh1.1*24,uh1*25.48 ), donde 1*24 es la etapa inicial y etapa final del dia.
            UtilYupana.AgregaLinea(ref textoArchivo, "SetArrdT(Ut,t) conjunto de arranques diarios termicas //");// dividir grupos por dia para cada CT ( ut1.1*24,ut1*25.48 ), donde 1*24 es la etapa inicial y etapa final del dia.
            CrearSetsSumaFlujo(ref textoArchivo, topcodi);
            CrearSetsDispComb(ref textoArchivo, topcodi, listaPeriodoDetalle, iniEtapa);
            //Estas son los nuevos agregados Benders
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.GrupoPrioridad);
            UtilYupana.AgregaLinea(ref textoArchivo, CrearDefVarCatGams(categoria));
            UtilYupana.AgregaLinea(ref textoArchivo, "Prioridad Niveles de prioridad máximos /Prior1*Prior10/");
            CrearSetGrupoprioridad(ref textoArchivo, topcodi);
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.CicloCombinado);
            CrearSetCicloCombinado(ref textoArchivo, topcodi, categoria);
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.RegionSeguridad);
            CrearArregloEquipo(topcodi, ConstantesBase.RegionSeguridad, categoria, ref textoArchivo);
            CrearSetRegionSeguridad(ref textoArchivo, topcodi, categoria, listaPeriodoDetalle, iniEtapa);
            //Yupana 2022
            categoria = listaCategoria.Find(x => x.Catcodi == ConstantesBase.Caldero);
            CrearArregloEquipo(topcodi, ConstantesBase.Caldero, categoria, ref textoArchivo);
            CrearSetCaldero(ref textoArchivo, topcodi, categoria);
            //Fin Yupana 2022
            UtilYupana.AgregaLinea(ref textoArchivo, "ALIAS(Ut,Utt)");
            //***Fin de los nuevos agregados
            UtilYupana.AgregaLinea(ref textoArchivo, "ALIAS(N,NP);");
            UtilYupana.AgregaLinea(ref textoArchivo, "ALIAS(Uh,Uhh);");
            UtilYupana.AgregaLinea(ref textoArchivo, "ALIAS(Emb,Embb);");
            UtilYupana.AgregaLinea(ref textoArchivo, "$offempty");
            FileHelper.GenerarArchivo(ConstantesBase.NombArchivoSets, ruta, textoArchivo);
        }

        //Yupanan 2022
        /// <summary>
        /// Crea set de caldero
        /// </summary>
        /// <param name="file"></param>
        /// <param name="topologia"></param>
        /// <param name="categoria"></param>
        /// <param name="modo"></param>
        private void CrearSetCaldero(ref string textoArchivo, int topologia, CpCategoriaDTO categoria)
        {
            string sLine = "";
            sLine = "MapCal_Ut(NumCal,Ut) asocia la caldera a las unidades termicas modeladas /";
            int idCaldero, idTermica;
            var lista = ListarGrupoRecursoFamiliaGams(topologia, ConstantesBase.Caldero, true);
            foreach (var reg in lista)
            {
                idTermica = FindIdGams(reg.Recurcodisicoes, ConstantesBase.ModoT);
                if (idTermica != 0)
                {
                    idCaldero = FindIdGams(reg.Recurcodi, ConstantesBase.Caldero);
                    sLine += ConstantesYupana.PrefijoCaldero + idCaldero.ToString() + "." + ConstantesYupana.PrefijoPlantaT + idTermica.ToString() + ",";
                }
            }

            if (lista.Count > 0)
                sLine = sLine.Substring(0, sLine.Length - 1) + "/";
            else
                sLine += ConstantesBase.Phantom + "/";
            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
        }
        //Fin Yupana 2022

        /// <summary>
        /// escribe en un archivo dat la restriccion en forma horizontal
        /// </summary>
        /// <param name="file"></param>
        /// <param name="srestriccion"></param>
        /// <param name="prefijo"></param>
        private void ImprimirGamsRestric48Horizontal(ref string textoArchivo, int srestriccion, int topcodi, string prefijo, List<CpRecursoDTO> listaEquipo, int iniEtapa, int finEtapa)
        {
            int idGams = 0;
            string sLine = string.Empty;
            decimal? acumulado;
            int indice;
            var lista = ListaRestricionesGams(_lsrestriccion48, (short)srestriccion);
            string sPeriodoHor = UtilYupana.WriteArchivoGams(string.Empty);
            int? nBloques = _lperiodo.Max(x => x.Etpbloque);
            foreach (var reg in _lperiodo)
                for (var i = reg.Etpini; i <= reg.Etpfin; i++)
                {
                    if (i >= iniEtapa && i <= finEtapa)
                    {
                        sPeriodoHor += UtilYupana.WriteArchivoGams((i - iniEtapa + 1).ToString());
                    }
                }
            UtilYupana.AgregaLinea(ref textoArchivo, sPeriodoHor);
            if (nBloques >= 1)
            {
                foreach (var reg in listaEquipo)
                {
                    var listadia = lista.Where(x => x.Recurcodi == reg.Recurcodi && x.Catcodi == reg.Catcodi);
                    if (listadia.Count() > 0)
                    {
                        acumulado = 0;
                        idGams = FindIdGams(reg.Recurcodi, reg.Catcodi);
                        sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                        decimal sumaFila = 0;
                        for (var nB = 1; nB <= nBloques; nB++)
                        {
                            var p = _lperiodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                            for (var i = p.Etpini; i <= p.Etpfin; i++)
                            {
                                acumulado += p.Etpdelta * 2;
                                if (i >= iniEtapa && i <= finEtapa)
                                {
                                    try
                                    {

                                        indice = (int)acumulado % 48;
                                        if (indice == 0) indice = 48;


                                        int dia = (int)((acumulado - 1) / 48);
                                        DateTime fecha = _escenario.Topfecha.AddDays(dia);
                                        var registro = lista.Find(x => x.Recurcodi == reg.Recurcodi && x.Catcodi == reg.Catcodi && x.Medifecha == fecha);

                                        if (registro != null)
                                        {
                                            switch (srestriccion)
                                            {
                                                case ConstantesBase.SRES_MANTENIMIENTO_LT:
                                                    int totalMantos = 0;
                                                    for (int z = 0; z < p.Etpdelta * 2; z++)
                                                    {
                                                        if ((indice - z) > 0)
                                                        {
                                                            decimal? valor = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro, null);
                                                            if (valor != null)
                                                            {
                                                                if (valor == 1)
                                                                    totalMantos++;
                                                            }
                                                        }
                                                    }
                                                    if (totalMantos < p.Etpdelta)
                                                    {
                                                        sLine += UtilYupana.WriteArchivoGams("0");
                                                    }
                                                    else
                                                    {
                                                        sLine += UtilYupana.WriteArchivoGams("1");
                                                    }
                                                    break;
                                                case ConstantesBase.SRES_GENER_OTROS:
                                                case ConstantesBase.SRES_GENER_RER:
                                                case ConstantesBase.SRES_APORTES_PH:
                                                case ConstantesBase.SRES_RESERVPRIM_PH:
                                                case ConstantesBase.SRES_APORTES_EMB:
                                                case ConstantesBase.SresCauRiegoEmb:
                                                case ConstantesBase.SresDefMaxEmb:
                                                case ConstantesBase.SresDefMinEmb:
                                                case ConstantesBase.SresVolMinEmb:
                                                case ConstantesBase.SresVolMaxEmb:
                                                case ConstantesBase.SRES_RPRIM_PT:
                                                case ConstantesBase.SresEfecTempPt:
                                                case ConstantesBase.SRES_DEMBARRA_NT:
                                                case ConstantesBase.SresSumFlujoLimInf:
                                                case ConstantesBase.SresSumFlujoLimSup:
                                                case ConstantesBase.SresSisRestricGener:
                                                    decimal total = 0;
                                                    decimal nDatos = 0;
                                                    for (int z = 0; z < p.Etpdelta * 2; z++)
                                                    {
                                                        if ((indice - z) > 0)
                                                        {
                                                            decimal? valor = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro, null);
                                                            if (valor != null)
                                                            {
                                                                total += (decimal)valor;
                                                                nDatos = nDatos + 1;
                                                            }
                                                        }
                                                    }
                                                    sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)(total / nDatos), ConstantesBase.NroDecimalGams).ToString());
                                                    break;
                                                default:
                                                    decimal? valor2 = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice)).ToString()).GetValue(registro, null);
                                                    if (valor2 != null)
                                                    {
                                                        sumaFila += (decimal)valor2;
                                                        sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)valor2, ConstantesBase.NroDecimalGams).ToString());
                                                    }
                                                    else
                                                        sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            sLine += UtilYupana.WriteArchivoGams(ConstantesBase.CaractNulo);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        var mensaje = ex.Message;
                                    }
                                }
                            }
                        }
                        switch (srestriccion)
                        {
                            case ConstantesBase.SresIndRsfPhUp:
                            case ConstantesBase.SresIndRsfPhDown:
                            case ConstantesBase.SresIndRsfPtUp:
                            case ConstantesBase.SresIndRsfPtDown:
                            case ConstantesBase.SresPTCCOMBEFIC:
                            case ConstantesBase.SresPTCVNCEFIC:
                                if (sumaFila > 0)
                                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                                break;
                            default:
                                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                                break;
                        }
                        sLine = string.Empty;
                    }
                }

                if (lista.Count == 0)
                {
                    switch (srestriccion)
                    {
                        case ConstantesBase.SresIndRsfPhUp:
                        case ConstantesBase.SresIndRsfPhDown:
                        case ConstantesBase.SresIndRsfPtUp:
                        case ConstantesBase.SresIndRsfPtDown:
                        case ConstantesBase.SresPTCCOMBEFIC:
                        case ConstantesBase.SresPTCVNCEFIC:
                            break;
                        default:
                            acumulado = 0;
                            idGams = (listaEquipo.Count > 0) ? 1 : 0;
                            sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                            for (var nB = 1; nB <= nBloques; nB++)
                            {
                                var p = _lperiodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                                acumulado += p.Etpdelta * 2;
                                for (var i = p.Etpini; i <= p.Etpfin; i++)
                                {

                                    indice = (int)acumulado % 48;
                                    if (indice == 0) indice = 48;
                                    int dia = (int)((acumulado - 1) / 48);
                                }

                                break;
                            }
                            break;
                    }
                    UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                    sLine = string.Empty;
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
        }

        /// <summary>
        /// escribe en un archivo dat la restriccion en forma vertical
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name=""></param>
        /// <param name="srestriccion"></param>
        /// <param name="topcodi"></param>
        /// <param name="prefijo"></param>
        /// <param name="tipoEquipo"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        /// <param name="topsinrsf"></param>
        private void ImprimirGamsRestric48VerticalYupanaContinuo(ref string textoArchivo, List<CpMedicion48DTO> lista, short srestriccion, int topcodi, string prefijo, int tipoEquipo, int iniEtapa, int finEtapa, int topsinrsf = 0)
        {
            int idFicticio, idGams = 0;
            string sLine = string.Empty;
            bool imprimirEquipo;
            decimal? acumulado = 0;
            List<int> listaFicticio = new List<int>();
            int indice;
            var lequipo = ListarRecursosPorCategoriaGams(_lrecurso, tipoEquipo);
            var listaEquipo = lequipo.Where(x => lista.Any(y => x.Recurcodi == y.Recurcodi)).ToList();

            sLine = UtilYupana.WriteArchivoGams("");
            foreach (var reg in listaEquipo)
            {
                if (tipoEquipo == ConstantesBase.NodoTopologico)
                {
                    idFicticio = BuscarEquipoFicticio(reg.Recurcodi);
                    if (idFicticio > 0)
                    {
                        var findFic = listaFicticio.Find(x => x == idFicticio);
                        if (findFic == 0)
                        {
                            listaFicticio.Add(idFicticio);
                        }
                    }
                    else
                    {
                        idGams = FindIdGams(reg.Recurcodi, reg.Catcodi);
                        sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                    }
                }
                else
                {
                    idGams = FindIdGams(reg.Recurcodi, reg.Catcodi);
                    sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                }
            }
            if (listaEquipo.Count == 0)
            {
                switch (srestriccion)
                {
                    case ConstantesBase.SRES_UNFOR_PT:
                    case ConstantesBase.SRES_APORTES_EMB:
                    case ConstantesBase.SRES_APORTES_PH:
                    case ConstantesBase.SRES_DEMBARRA_NT:
                    case ConstantesBase.SRES_GENER_RER:
                        break;
                    default:
                        switch (tipoEquipo)
                        {
                            case ConstantesBase.Urs:
                            case ConstantesBase.Rsf:
                                sLine += prefijo + "1";
                                break;
                            default:
                                sLine += prefijo + "0";
                                break;

                        }
                        break;
                }
            }
            foreach (var reg in listaFicticio)
            {
                idGams = FindIdGams(reg, tipoEquipo);
                sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
            }

            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = string.Empty;
            int? nBloques = _lperiodo.Max(x => x.Etpbloque);
            if (nBloques >= 1)
            {
                for (var nB = 1; nB <= nBloques; nB++)
                {
                    var p = _lperiodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                    for (var i = p.Etpini; i <= p.Etpfin; i++)
                    {
                        acumulado += p.Etpdelta * 2;
                        if (i >= iniEtapa && i <= finEtapa)
                        {
                            indice = (int)acumulado % 48;
                            if (indice == 0) indice = 48;
                            sLine = UtilYupana.WriteArchivoGams((i - iniEtapa + 1).ToString());
                            int dia = (int)((acumulado - 1) / 48);
                            DateTime fecha = _escenario.Topfecha.AddDays(dia);
                            foreach (var reg in listaEquipo)
                            {
                                var registro = lista.Find(x => x.Recurcodi == reg.Recurcodi && x.Medifecha == fecha);
                                if (registro != null)
                                {
                                    switch (srestriccion)
                                    {
                                        case ConstantesBase.SRES_GENER_OTROS:
                                        case ConstantesBase.SRES_GENER_RER:
                                        case ConstantesBase.SRES_APORTES_PH:
                                        case ConstantesBase.SRES_RESERVPRIM_PH:
                                        case ConstantesBase.SRES_APORTES_EMB:
                                        case ConstantesBase.SresCauRiegoEmb:
                                        case ConstantesBase.SresDefMaxEmb:
                                        case ConstantesBase.SresDefMinEmb:
                                        case ConstantesBase.SresVolMinEmb:
                                        case ConstantesBase.SresVolMaxEmb:
                                        case ConstantesBase.SRES_RPRIM_PT:
                                        case ConstantesBase.SresEfecTempPt:
                                        case ConstantesBase.SresSumFlujoLimInf:
                                        case ConstantesBase.SresSumFlujoLimSup:
                                        case ConstantesBase.SresSisRestricGener:
                                        case ConstantesBase.SRES_DEMBARRA_NT:
                                            imprimirEquipo = true;

                                            if (tipoEquipo == ConstantesBase.NodoTopologico)
                                            {
                                                idFicticio = BuscarEquipoFicticio(reg.Recurcodi);
                                                if (idFicticio > 0)
                                                {
                                                    imprimirEquipo = false;
                                                }
                                            }
                                            if (imprimirEquipo)
                                            {
                                                decimal total = 0;
                                                decimal nDatos = 0;
                                                for (int z = 0; z < p.Etpdelta * 2; z++)
                                                {
                                                    if ((indice - z) > 0)
                                                    {
                                                        decimal? valor = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro, null);
                                                        if (valor != null)
                                                        {
                                                            total += (decimal)valor;
                                                            nDatos = nDatos + 1;
                                                        }
                                                    }
                                                }
                                                if (nDatos > 0)
                                                    sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)(total / nDatos), ConstantesBase.NroDecimalGams).ToString());
                                                else
                                                    sLine += UtilYupana.WriteArchivoGams("");
                                            }

                                            break;

                                        default:
                                            decimal? valor2 = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice)).ToString()).GetValue(registro, null);
                                            if (valor2 != null)
                                                sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)valor2, ConstantesBase.NroDecimalGams).ToString());
                                            else
                                                sLine += UtilYupana.WriteArchivoGams("");
                                            break;
                                    }
                                }
                                else
                                {
                                    idFicticio = BuscarEquipoFicticio(reg.Recurcodi);
                                    if (idFicticio == 0)
                                    {
                                        sLine += UtilYupana.WriteArchivoGams("");
                                    }

                                }
                            }
                            foreach (var reg in listaFicticio)
                            {
                                //Encontrar los dos nodos
                                var findRed = ReduccionBarra.ListaRecursoReduccion.Find(x => x.RecurReduccion == reg);
                                if (findRed != null)
                                {
                                    var registro1 = lista.Find(x => x.Recurcodi == findRed.ListaReduccion[0].RecurReduccion && x.Medifecha == fecha);
                                    var registro2 = lista.Find(x => x.Recurcodi == findRed.ListaReduccion[1].RecurReduccion && x.Medifecha == fecha);
                                    if (registro1 != null && registro2 != null)
                                    {
                                        decimal total = 0;
                                        decimal nDatos = 0;
                                        for (int z = 0; z < p.Etpdelta * 2; z++)
                                        {
                                            if ((indice - z) > 0)
                                            {
                                                decimal? valor1 = (decimal?)registro1.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro1, null);
                                                decimal? valor2 = (decimal?)registro2.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro2, null);
                                                decimal? valor3 = valor1 + valor2;
                                                if (valor3 != null)
                                                {
                                                    total += (decimal)valor3;
                                                    nDatos = nDatos + 1;
                                                }
                                            }
                                        }
                                        sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)(total / nDatos), ConstantesBase.NroDecimalGams).ToString());
                                    }
                                }
                            }
                            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                            sLine = string.Empty;
                        }
                    }
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
        }

        /// <summary>
        /// escribe en un archivo dat la restriccion en forma vertical
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name=""></param>
        /// <param name="srestriccion"></param>
        /// <param name="topcodi"></param>
        /// <param name="prefijo"></param>
        /// <param name="tipoEquipo"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        /// <param name="topsinrsf"></param>
        private void ImprimirGamsRestric48Vertical(ref string textoArchivo, short srestriccion, int topcodi, string prefijo, int tipoEquipo, int iniEtapa, int finEtapa, int topsinrsf = 0)
        {
            int idFicticio, idGams = 0;
            string sLine = string.Empty;
            bool imprimirEquipo;
            decimal? acumulado = 0;
            List<int> listaFicticio = new List<int>();
            int indice;
            List<CpMedicion48DTO> lista = new List<CpMedicion48DTO>();
            DateTime fechaini = _escenario.Topfecha.AddDays(_escenario.Topinicio);
            DateTime fechafin = fechaini.AddDays(_escenario.Topdiasproc);
            if (topsinrsf == 0)
            {
                lista = ListaRestriciones(topcodi, (short)srestriccion).Where(x => x.Medifecha >= fechaini && x.Medifecha < fechafin).ToList();
                lista = lista.Where(x => x.Meditotal != null).ToList();
            }

            //var listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, tipoEquipo);
            var lequipo = ListarRecursosPorCategoriaGams(_lrecurso, tipoEquipo);
            var listaEquipo = lequipo.Where(x => lista.Any(y => x.Recurcodi == y.Recurcodi)).ToList();

            sLine = UtilYupana.WriteArchivoGams("");
            foreach (var reg in listaEquipo)
            {
                if (tipoEquipo == ConstantesBase.NodoTopologico)
                {
                    idFicticio = BuscarEquipoFicticio(reg.Recurcodi);
                    if (idFicticio > 0)
                    {
                        var findFic = listaFicticio.Find(x => x == idFicticio);
                        if (findFic == 0)
                        {
                            listaFicticio.Add(idFicticio);
                        }
                    }
                    else
                    {
                        idGams = FindIdGams(reg.Recurcodi, reg.Catcodi);
                        sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                    }
                }
                else
                {
                    idGams = FindIdGams(reg.Recurcodi, reg.Catcodi);
                    sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                }
            }

            if (listaEquipo.Count == 0)
            {
                switch (srestriccion)
                {
                    case ConstantesBase.SRES_UNFOR_PT:
                    case ConstantesBase.SRES_APORTES_EMB:
                    case ConstantesBase.SRES_APORTES_PH:
                    case ConstantesBase.SRES_DEMBARRA_NT:
                    case ConstantesBase.SRES_GENER_RER:
                        break;
                    default:
                        switch (tipoEquipo)
                        {
                            case ConstantesBase.Urs:
                            case ConstantesBase.Rsf:
                                sLine += prefijo + "1";
                                break;
                            default:
                                sLine += prefijo + "0";
                                break;

                        }
                        break;
                }

            }

            foreach (var reg in listaFicticio)
            {
                idGams = FindIdGams(reg, tipoEquipo);
                sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
            }

            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
            sLine = string.Empty;
            int? nBloques = _lperiodo.Max(x => x.Etpbloque);

            if (listaEquipo.Count > 0)
            {
                if (nBloques >= 1)
                {
                    for (var nB = 1; nB <= nBloques; nB++)
                    {
                        var p = _lperiodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                        for (var i = p.Etpini; i <= p.Etpfin; i++)
                        {
                            acumulado += p.Etpdelta * 2;
                            if (i >= iniEtapa && i <= finEtapa)
                            {
                                indice = (int)acumulado % 48;
                                if (indice == 0) indice = 48;
                                sLine = UtilYupana.WriteArchivoGams((i - iniEtapa + 1).ToString());
                                int dia = (int)((acumulado - 1) / 48);
                                DateTime fecha = _escenario.Topfecha.AddDays(dia);
                                foreach (var reg in listaEquipo)
                                {
                                    var registro = lista.Find(x => x.Recurcodi == reg.Recurcodi && x.Medifecha == fecha);
                                    if (registro != null)
                                    {
                                        switch (srestriccion)
                                        {
                                            case ConstantesBase.SRES_GENER_OTROS:
                                            case ConstantesBase.SRES_GENER_RER:
                                            case ConstantesBase.SRES_APORTES_PH:
                                            case ConstantesBase.SRES_RESERVPRIM_PH:
                                            case ConstantesBase.SRES_APORTES_EMB:
                                            case ConstantesBase.SresCauRiegoEmb:
                                            case ConstantesBase.SresDefMaxEmb:
                                            case ConstantesBase.SresDefMinEmb:
                                            case ConstantesBase.SresVolMinEmb:
                                            case ConstantesBase.SresVolMaxEmb:
                                            case ConstantesBase.SRES_RPRIM_PT:
                                            case ConstantesBase.SresEfecTempPt:
                                            case ConstantesBase.SresSumFlujoLimInf:
                                            case ConstantesBase.SresSumFlujoLimSup:
                                            case ConstantesBase.SresSisRestricGener:
                                            case ConstantesBase.SRES_DEMBARRA_NT:
                                                imprimirEquipo = true;

                                                if (tipoEquipo == ConstantesBase.NodoTopologico)
                                                {
                                                    idFicticio = BuscarEquipoFicticio(reg.Recurcodi);
                                                    if (idFicticio > 0)
                                                    {
                                                        imprimirEquipo = false;
                                                    }
                                                }
                                                if (imprimirEquipo)
                                                {
                                                    decimal total = 0;
                                                    decimal nDatos = 0;
                                                    for (int z = 0; z < p.Etpdelta * 2; z++)
                                                    {
                                                        if ((indice - z) > 0)
                                                        {
                                                            decimal? valor = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro, null);
                                                            if (valor != null)
                                                            {
                                                                total += (decimal)valor;
                                                                nDatos = nDatos + 1;
                                                            }
                                                        }
                                                    }
                                                    if (nDatos > 0)
                                                        sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)(total / nDatos), ConstantesBase.NroDecimalGams).ToString());
                                                    else
                                                        sLine += UtilYupana.WriteArchivoGams("");
                                                }

                                                break;

                                            default:
                                                decimal? valor2 = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice)).ToString()).GetValue(registro, null);
                                                if (valor2 != null)
                                                    sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)valor2, ConstantesBase.NroDecimalGams).ToString());
                                                else
                                                    sLine += UtilYupana.WriteArchivoGams("");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        idFicticio = BuscarEquipoFicticio(reg.Recurcodi);
                                        if (idFicticio == 0)
                                        {
                                            sLine += UtilYupana.WriteArchivoGams("");
                                        }

                                    }
                                }
                                foreach (var reg in listaFicticio)
                                {
                                    //Encontrar los dos nodos
                                    var findRed = ReduccionBarra.ListaRecursoReduccion.Find(x => x.RecurReduccion == reg);
                                    if (findRed != null)
                                    {
                                        var registro1 = lista.Find(x => x.Recurcodi == findRed.ListaReduccion[0].RecurReduccion && x.Medifecha == fecha);
                                        var registro2 = lista.Find(x => x.Recurcodi == findRed.ListaReduccion[1].RecurReduccion && x.Medifecha == fecha);
                                        if (registro1 != null && registro2 != null)
                                        {
                                            decimal total = 0;
                                            decimal nDatos = 0;
                                            for (int z = 0; z < p.Etpdelta * 2; z++)
                                            {
                                                if ((indice - z) > 0)
                                                {
                                                    decimal? valor1 = (decimal?)registro1.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro1, null);
                                                    decimal? valor2 = (decimal?)registro2.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro2, null);
                                                    decimal? valor3 = valor1 + valor2;
                                                    if (valor3 != null)
                                                    {
                                                        total += (decimal)valor3;
                                                        nDatos = nDatos + 1;
                                                    }
                                                }
                                            }
                                            sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)(total / nDatos), ConstantesBase.NroDecimalGams).ToString());
                                        }
                                    }
                                }
                                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                                sLine = string.Empty;
                            }
                        }
                    }
                }
            }

            UtilYupana.AgregaLinea(ref textoArchivo, ";");
        }

        /// <summary>
        /// escribe en un archivo dat la restriccion en forma horizontal
        /// </summary>
        /// <param name="file"></param>
        /// <param name="srestriccion"></param>
        /// <param name="prefijo"></param>
        private void ImprimirGamsRestric48Horizontal2(ref string textoArchivo, short srestriccion, int topcodi, string prefijo, int tipoEquipo, int iniEtapa, int finEtapa)
        {
            int idGams = 0;
            string sLine = string.Empty;
            decimal? acumulado;
            int indice;
            var lista = ListaRestricionesGams(_lsrestriccion48, srestriccion);
            var listaEquipo = ListarRecursosPorCategoriaGams(_lrecurso, tipoEquipo);
            string sPeriodoHor = UtilYupana.WriteArchivoGams(string.Empty);
            int? nBloques = _lperiodo.Max(x => x.Etpbloque);

            foreach (var reg in _lperiodo)
                for (var i = reg.Etpini; i <= reg.Etpfin; i++)
                {
                    if (i >= iniEtapa && i <= finEtapa)
                    {
                        sPeriodoHor += UtilYupana.WriteArchivoGams((i - iniEtapa + 1).ToString());
                    }
                }
            UtilYupana.AgregaLinea(ref textoArchivo, sPeriodoHor);


            if (nBloques >= 1)
            {
                foreach (var reg in listaEquipo)
                {
                    var find = lista.Find(x => x.Recurcodi == reg.Recurcodi);
                    if (find != null)
                    {
                        acumulado = 0;
                        idGams = FindIdGams(reg.Recurcodi, reg.Catcodi);
                        sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                        for (var nB = 1; nB <= nBloques; nB++)
                        {
                            var p = _lperiodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                            for (var i = p.Etpini; i <= p.Etpfin; i++)
                            {
                                acumulado += p.Etpdelta * 2;
                                if (i >= iniEtapa && i <= finEtapa)
                                {
                                    indice = (int)acumulado % 48;
                                    if (indice == 0) indice = 48;
                                    int dia = (int)((acumulado - 1) / 48);
                                    DateTime fecha = _escenario.Topfecha.AddDays(dia);
                                    var registro = lista.Find(x => x.Recurcodi == reg.Recurcodi && x.Medifecha == fecha);
                                    if (registro != null)
                                    {
                                        decimal valor = (decimal)registro.GetType().GetProperty("H" + ((int)(indice)).ToString()).GetValue(registro, null);
                                        sLine += UtilYupana.WriteArchivoGams(valor.ToString());
                                    }
                                    else
                                    {
                                        sLine += UtilYupana.WriteArchivoGams("");
                                    }
                                }
                            }
                        }
                        UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                        sLine = string.Empty;
                    }
                }
            }

            if (lista.Count == 0)
            {
                idGams = 0;
                if (listaEquipo.Count > 0)
                {
                    idGams = FindIdGams(listaEquipo[0].Recurcodi, tipoEquipo);
                }
                sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                sLine = string.Empty;
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
        }

        /// <summary>
        /// Genera Texto para archivo Dat de restricciomes de generación pot subrestricción
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="lista48"></param>
        /// <param name="listSubr"></param>
        /// <param name="tipoEquipo"></param>
        /// <param name="prefijo"></param>
        /// <param name="subrestriccion"></param>
        /// <param name="topcodi"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        private void ImprimirRestricGener(ref string textoArchivo, List<CpMedicion48DTO> lista48, List<CpSubrestricdatDTO> listSubr, int tipoEquipo, string prefijo, int subrestriccion, int topcodi, int iniEtapa, int finEtapa)
        {
            int idGams = 0;
            string sLine = string.Empty;
            decimal? acumulado;
            int indice;
            string sPeriodoHor = UtilYupana.WriteArchivoGams(string.Empty);
            int? nBloques = _lperiodo.Max(x => x.Etpbloque);
            foreach (var reg in _lperiodo)
                for (var i = reg.Etpini; i <= reg.Etpfin; i++)
                {
                    if (i >= iniEtapa && i <= finEtapa)
                    {
                        sPeriodoHor += UtilYupana.WriteArchivoGams((i - iniEtapa + 1).ToString());
                    }
                }
            UtilYupana.AgregaLinea(ref textoArchivo, sPeriodoHor);
            if (nBloques >= 1)
            {
                foreach (var reg in listSubr)
                {
                    var find = lista48.Find(x => x.Recurcodi == reg.Recurcodi);
                    if (find != null)
                    {
                        acumulado = 0;
                        idGams = FindIdGams(reg.Recurcodi, tipoEquipo);
                        sLine += UtilYupana.WriteArchivoGams(prefijo + idGams.ToString());
                        for (var nB = 1; nB <= nBloques; nB++)
                        {
                            var p = _lperiodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                            for (var i = p.Etpini; i <= p.Etpfin; i++)
                            {
                                acumulado += p.Etpdelta * 2;
                                if (i >= iniEtapa && i <= finEtapa)
                                {
                                    indice = (int)acumulado % 48;
                                    if (indice == 0) indice = 48;
                                    int dia = (int)((acumulado - 1) / 48);
                                    DateTime fecha = _escenario.Topfecha.AddDays(dia);
                                    var registro = lista48.Find(x => x.Recurcodi == reg.Recurcodi && x.Medifecha == fecha);
                                    if (registro != null)
                                    {
                                        decimal valor = (decimal)registro.GetType().GetProperty("H" + ((int)(indice)).ToString()).GetValue(registro, null);
                                        sLine += UtilYupana.WriteArchivoGams(Math.Round(valor, ConstantesBase.NroDecimalGams).ToString());
                                    }
                                    else
                                    {
                                        sLine += UtilYupana.WriteArchivoGams("");
                                    }
                                }
                            }
                        }
                        UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                        sLine = string.Empty;
                    }
                }
            }

            if (listSubr.Count == 0)
            {
                idGams = 0;
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
        }

        /// <summary>
        /// Genera Texto Vertical para archivo Dat de restricciomes de Urs por subrestricción
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="srestriccion"></param>
        /// <param name="topcodi"></param>
        /// <param name="listaEquipo"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        /// <param name="topsinrsf"></param>
        private void ImprimirGamsRestric48VerticalURS(ref string textoArchivo, short srestriccion, int topcodi, List<CpRecursoDTO> listaEquipo, int iniEtapa, int finEtapa, int topsinrsf)
        {
            string sLine = string.Empty;
            decimal? acumulado = 0;
            int indice;
            List<CpMedicion48DTO> lista = new List<CpMedicion48DTO>();
            if (topsinrsf == 0)
                lista = ListaRestriciones(topcodi, srestriccion);
            sLine = UtilYupana.WriteArchivoGams("");
            sLine = string.Empty;
            int? nBloques = _lperiodo.Max(x => x.Etpbloque);
            if (nBloques >= 1)
            {
                for (var nB = 1; nB <= nBloques; nB++)
                {
                    var p = _lperiodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                    for (var i = p.Etpini; i <= p.Etpfin; i++)
                    {
                        acumulado += p.Etpdelta * 2;
                        if (i >= iniEtapa && i <= finEtapa)
                        {
                            indice = (int)acumulado % 48;
                            if (indice == 0) indice = 48;
                            sLine = UtilYupana.WriteArchivoGams((i - iniEtapa + 1).ToString());
                            int dia = (int)((acumulado - 1) / 48);
                            DateTime fecha = _escenario.Topfecha.AddDays(dia);
                            foreach (var reg in listaEquipo)
                            {
                                var registro = lista.Find(x => x.Recurcodi == reg.Recurcodi && x.Medifecha == fecha);
                                if (registro != null)
                                {
                                    decimal total = 0;
                                    decimal nDatos = 0;
                                    for (int z = 0; z < p.Etpdelta * 2; z++)
                                    {
                                        if ((indice - z) > 0)
                                        {
                                            decimal? valor = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro, null);
                                            if (valor != null)
                                            {
                                                total += (decimal)valor;
                                                nDatos = nDatos + 1;
                                            }
                                        }
                                    }
                                    sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)(total / nDatos), ConstantesBase.NroDecimalGams).ToString());
                                }
                                else
                                {
                                    sLine += UtilYupana.WriteArchivoGams("");
                                }
                            }
                            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                            sLine = string.Empty;
                        }
                    }
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
        }

        /// <summary>
        /// Genera Texto Vertical para archivo Dat de restricciomes de Urs por subrestricción
        /// </summary>
        /// <param name="textoArchivo"></param>
        /// <param name="srestriccion1"></param>
        /// <param name="srestriccion2"></param>
        /// <param name="topcodi"></param>
        /// <param name="listaEquipo"></param>
        /// <param name="iniEtapa"></param>
        /// <param name="finEtapa"></param>
        /// <param name="topsinrsf"></param>
        private void ImprimirGamsRestric48VerticalURSTr(ref string textoArchivo, short srestriccion1, short srestriccion2, int topcodi, List<CpRecursoDTO> listaEquipo, int iniEtapa, int finEtapa, int topsinrsf)
        {
            string sLine = string.Empty;
            decimal? acumulado = 0;
            int indice;
            List<CpMedicion48DTO> listatr1 = new List<CpMedicion48DTO>();
            List<CpMedicion48DTO> listatr2 = new List<CpMedicion48DTO>();
            if (topsinrsf == 0)
            {
                listatr1 = ListaRestricionesGams(_lsrestriccion48, srestriccion1);
                listatr2 = ListaRestricionesGams(_lsrestriccion48, srestriccion2);
            }
            sLine = UtilYupana.WriteArchivoGams("");
            sLine = string.Empty;
            int? nBloques = _lperiodo.Max(x => x.Etpbloque);
            if (nBloques >= 1)
            {
                for (var nB = 1; nB <= nBloques; nB++)
                {
                    var p = _lperiodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                    for (var i = p.Etpini; i <= p.Etpfin; i++)
                    {
                        acumulado += p.Etpdelta * 2;
                        if (i >= iniEtapa && i <= finEtapa)
                        {
                            indice = (int)acumulado % 48;
                            if (indice == 0) indice = 48;
                            sLine = UtilYupana.WriteArchivoGams((i - iniEtapa + 1).ToString());
                            int dia = (int)((acumulado - 1) / 48);
                            DateTime fecha = _escenario.Topfecha.AddDays(dia);
                            foreach (var reg in listaEquipo)
                            {
                                var registro = listatr1.Find(x => x.Recurcodi == reg.Recurcodi && x.Medifecha == fecha);
                                if (registro != null)
                                {
                                    decimal total = 0;
                                    decimal nDatos = 0;
                                    for (int z = 0; z < p.Etpdelta * 2; z++)
                                    {
                                        if ((indice - z) > 0)
                                        {
                                            decimal? valor = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro, null);
                                            if (valor != null)
                                            {
                                                total += (decimal)valor;
                                                nDatos = nDatos + 1;
                                            }
                                        }
                                    }
                                    sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)(total / nDatos), ConstantesBase.NroDecimalGams).ToString());
                                }
                                else
                                {
                                    sLine += UtilYupana.WriteArchivoGams("");
                                }
                                registro = listatr2.Find(x => x.Recurcodi == reg.Recurcodi && x.Medifecha == fecha);
                                if (registro != null)
                                {
                                    decimal total = 0;
                                    decimal nDatos = 0;
                                    for (int z = 0; z < p.Etpdelta * 2; z++)
                                    {
                                        if ((indice - z) > 0)
                                        {
                                            decimal? valor = (decimal?)registro.GetType().GetProperty("H" + ((int)(indice - z)).ToString()).GetValue(registro, null);
                                            if (valor != null)
                                            {
                                                total += (decimal)valor;
                                                nDatos = nDatos + 1;
                                            }
                                        }
                                    }
                                    sLine += UtilYupana.WriteArchivoGams(Math.Round((decimal)(total / nDatos), ConstantesBase.NroDecimalGams).ToString());
                                }
                                else
                                {
                                    sLine += UtilYupana.WriteArchivoGams("");
                                }
                            }
                            UtilYupana.AgregaLinea(ref textoArchivo, sLine);
                            sLine = string.Empty;
                        }
                    }
                }
            }
            UtilYupana.AgregaLinea(ref textoArchivo, ";");
        }

        #endregion

        #region Fuentes Gams

        /// <summary>
        /// Ejecuta el caso en Gams
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public GAMSJob EjecutarEscenario(string ruta, int topcodi)
        {
            GAMSJob resultado = null;
            var fuente = FactorySic.GetCpFuentegamsversionRepository().GetByCriteria(topcodi);
            if (fuente.Count > 0)
            {
                //- Declaraando los objetos para integracion con GAMS
                string XsystemDirectory = ConfigurationManager.AppSettings[ConstantesYupanaContinuo.DirectorioGAMS] ?? @"C:\GAMS\34";
                GAMSWorkspace ws;
                ws = new GAMSWorkspace(workingDirectory: ruta, XsystemDirectory, DebugLevel.Off);
                FileHelper.GenerarArchivo(ConstantesBase.NombArchivoInputData, ruta, fuente[0].Fversinputdata);
                FileHelper.GenerarArchivoByte(ConstantesBase.NombArchivoEncriptado, ruta, fuente[0].Fverscodigoencrip);
                GAMSCheckpoint cp = ws.AddCheckpoint(); ///Nueva linea
                GAMSDatabase resultDB = ws.AddDatabase();

                using (GAMSOptions opt = ws.AddOptions())
                {
                    //- Ejecutando el modelo

                    resultado = ws.AddJobFromString(fuente[0].Fversruncase);
                    //resultado.Run(opt, resultDB);
                    FileHelper.RunGams(ref resultado, resultDB, ruta, ConstantesBase.NombArchivoLogGams);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Genera lista de horas a partir de lista de etapas del escenario
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static List<HoraEtapa> ListaHoras(List<CpDetalleetapaDTO> lista, int diaInicio)
        {
            decimal? acumulado = 0;
            List<HoraEtapa> retorno = new List<HoraEtapa>();
            int iniEtapa = 1;
            if (diaInicio > 0)
                iniEtapa = ObtenerBloqueInicio(lista, diaInicio);
            foreach (var reg in lista)
                for (var i = reg.Etpini; i <= reg.Etpfin; i++)
                {
                    if (i >= iniEtapa)
                    {
                        acumulado += reg.Etpdelta * 2;
                        retorno.Add(new HoraEtapa
                        {
                            Hora = (int)acumulado,
                            Delta = (decimal)reg.Etpdelta
                        });
                        //retorno.Add((int)acumulado);
                    }
                }
            return retorno;
        }

        /// <summary>
        /// Obtine el primer registro del perido
        /// </summary>
        /// <param name="listaPeriodo"></param>
        /// <param name="diaInicio"></param>
        /// <returns></returns>
        public static int ObtenerBloqueInicio(List<CpDetalleetapaDTO> listaPeriodo, int diaInicio)
        {
            decimal? acumulado = 0;
            int etapaIni = 1;
            int? nBloques = listaPeriodo.Max(x => x.Etpbloque);
            for (var nB = 1; nB <= nBloques; nB++)
            {
                var p = listaPeriodo.Where(x => x.Etpbloque == nB).FirstOrDefault();
                for (int i = (int)p.Etpini; i <= p.Etpfin; i++)
                {
                    acumulado += p.Etpdelta * 2;
                    if (acumulado > diaInicio * 48)
                    {
                        etapaIni = i;
                        break;
                    }
                }
            }

            return etapaIni;
        }

        /// <summary>
        /// Devuelve prefijo del idGams
        /// </summary>
        /// <param name="idGams"></param>
        /// <returns></returns>
        public static string ExtraePrefijoCodigoGams(string idGams)
        {
            string cadenaTipoEquipo = string.Empty;

            int resultado = 0;
            for (int i = 0; i < idGams.Length; i++)
            {
                if (int.TryParse(idGams[i].ToString(), out resultado))
                {
                    break;
                }
                else
                {
                    cadenaTipoEquipo += idGams[i];
                }
            }
            return cadenaTipoEquipo;
        }

        /// <summary>
        /// Devuelve codigo numerico del idGams
        /// </summary>
        /// <param name="idGams"></param>
        /// <param name="prefijo"></param>
        /// <returns></returns>
        public static int ExtraeIdGamsNum(string idGams, string prefijo)
        {
            int id = 0;
            string idText = idGams.Substring(prefijo.Length, idGams.Length - prefijo.Length);
            if (int.TryParse(idText, out id))
            {

            }
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="resultado"></param>
        /// <param name="topcodi"></param>
        /// <param name="subrestcodi"></param>
        /// <param name="diaSeleccionado"></param>
        /// <param name="variable"></param>
        /// <param name="tipoGdx"></param>
        /// <param name="tipoEquipo"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetLista48EcuacionGams(string ruta, GAMSJob resultado, int topcodi, int diaSeleccionado, string variable, int catecodi)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            int delta1;
            int idCoes = 0;
            int idgams = 0;
            int indice = 0;
            int idcat2 = 0;
            CpRecursoDTO findReg = new CpRecursoDTO();
            string prefijoGams = string.Empty;
            MeMedicion48DTO registro = null;
            List<CpRecursoDTO> listaRecurso = new List<CpRecursoDTO>();
            List<CpRecursoDTO> trafos2d = new List<CpRecursoDTO>();
            List<CpRecursoDTO> trafos3d = new List<CpRecursoDTO>();
            GAMSEquation listaVariable = ResultadoEcuacionGams(ruta, resultado, variable);
            decimal valor = 0M;
            bool buscaRecurso = true;
            string equipo = string.Empty;
            if (listaVariable != null)
            {
                int diaAnterior = 0;
                List<CpDetalleetapaDTO> listaPeriodo = ListCpDetalleetapas(topcodi);
                var topologia = GetTopologia(topcodi);
                var listaP = ListaHoras(listaPeriodo, topologia.Topinicio);
                int horaInicio = topologia.Topiniciohora;
                int periodo, periodoAnt = 0;
                int etapa = 0;
                int etapadia = 0;
                int dia;
                foreach (GAMSEquationRecord rec in listaVariable)
                {
                    periodo = int.Parse(rec.Key(1));
                    delta1 = (int)(listaP[periodo - 1].Delta * 2);
                    if (horaInicio <= 1)
                        etapa = listaP[periodo - 1].Hora;
                    else
                    {
                        periodo += (horaInicio - 1) / delta1;//2;
                        etapa = listaP[periodo - 1].Hora;//.Hora + horaInicio - 1 - 2;
                    }

                    dia = (etapa - 1) / 48;
                    etapadia = (etapa - 1) % 48 + 1;
                    if (dia <= diaSeleccionado)
                    {
                        if ((rec.Key(0) != equipo || diaAnterior != dia) && !string.IsNullOrEmpty(equipo))
                        {
                            if (registro.Recurcodi != 0)
                                lista.Add(registro);
                            registro = new MeMedicion48DTO();
                            //periodoAnt = 0;
                        }
                        if (registro == null)
                        {
                            registro = new MeMedicion48DTO();
                        }
                        equipo = rec.Key(0);
                        diaAnterior = dia;

                        if (indice == 0)
                        {
                            prefijoGams = ExtraePrefijoCodigoGams(rec.Key(0));
                            if (buscaRecurso)
                            {
                                buscaRecurso = false;
                                switch (catecodi)
                                {
                                    case ConstantesBase.Linea:
                                        //listaRecurso = ListarLinea(catecodi, Parametros.getTopologia(), Parametros.getModo()).ToList();
                                        trafos2d = ListarRecurso(ConstantesBase.Trafo2D, topcodi);
                                        trafos3d = ListarRecurso(ConstantesBase.Trafo3D, topcodi);
                                        break;
                                    case ConstantesBase.ModoT:
                                        listaRecurso = ListarRecurso(catecodi, topcodi);
                                        listaRecurso = listaRecurso.Where(x => x.Recurconsideragams == 1).ToList();
                                        break;
                                    default:
                                        listaRecurso = ListarRecurso(catecodi, topcodi);
                                        break;
                                }

                            }

                        }

                        idgams = ExtraeIdGamsNum(rec.Key(0), prefijoGams);
                        idCoes = MapeoGamsEscenario.FindIdEquipo(idgams, catecodi);
                        idcat2 = MapeoGamsEscenario.FindCategoria2(idgams, catecodi);
                        switch (catecodi)
                        {
                            case ConstantesBase.Linea:
                                switch (idcat2)
                                {
                                    case ConstantesBase.Linea:
                                        findReg = listaRecurso.Find(x => x.Recurcodi == idCoes);
                                        break;
                                    case ConstantesBase.Trafo2D:
                                        findReg = trafos2d.Find(x => x.Recurcodi == idCoes);
                                        break;
                                    case ConstantesBase.Trafo3D:
                                        findReg = trafos3d.Find(x => x.Recurcodi == idCoes);
                                        break;
                                }
                                break;
                            default:
                                findReg = listaRecurso.Find(x => x.Recurcodi == idCoes);
                                break;
                        }

                        if (findReg != null)
                        {
                            //registro.RECURNOMBRE = findReg.Recurnombre;
                            //registro.Idgams = rec.Key(0);
                            registro.Medifecha = topologia.Topfecha.AddDays(dia + topologia.Topinicio);
                            //registro.TOPCODI = Parametros.getTopologia();
                            registro.Recurcodi = idCoes;
                            //registro.SRESTRICCODI = subrestcodi;

                            valor = (decimal)rec.Marginal;
                            registro.GetType().GetProperty("H" + etapadia.ToString()).SetValue(registro, valor);
                            int delta = (int)(listaP[periodo - 1].Delta * 2);
                            for (int k = 1; k < delta; k++)
                            {

                                registro.GetType().GetProperty("H" + (etapadia - k).ToString()).SetValue(registro, valor);
                            }
                        }
                        indice++;
                    }
                    else
                    {
                        equipo = rec.Key(0);
                        diaAnterior = dia;
                        periodoAnt = periodo;
                    }

                }
                if (registro != null)
                    if (registro.Recurcodi != 0)
                        lista.Add(registro);
            }

            return lista;
        }

        /// <summary>
        /// Devuelve el reultado Gams de una ecuacion
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="resultado"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public static GAMSEquation ResultadoEcuacionGams(string ruta, GAMSJob resultado, string variable)
        {
            GAMSEquation listaVariable = null;
            if (resultado != null)
            {
                GAMSDatabase db2 = resultado.Workspace.AddDatabaseFromGDX(ruta + ConstantesBase.ArchivoGdxFinal);
                try
                {
                    listaVariable = db2.GetEquation(variable);
                }
                catch (Exception ex)
                {
                    //return null;
                }
                //}
            }

            return listaVariable;

        }

        /// <summary>
        /// Devuelve el valor de un parametro Gams
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="resultado"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public decimal? GetValorParameterGams(string ruta, GAMSJob resultado, string variable)
        {
            decimal? valor = null;
            try
            {
                GAMSDatabase db2 = resultado.Workspace.AddDatabaseFromGDX(ruta + ConstantesBase.ArchivoGdxFinal);
                GAMSParameter listaVariable = db2.GetParameter(variable);

                if (listaVariable != null)
                {
                    foreach (GAMSParameterRecord rec in listaVariable)
                    {
                        valor = Convert.ToDecimal(rec.Value);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return valor;
        }

        /// <summary>
        /// Devuelve el valor de una variable Gams
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="resultado"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public decimal? GetValorVariableGams(string ruta, GAMSJob resultado, string variable)
        {
            decimal? valor = null;
            try
            {
                GAMSDatabase db2 = resultado.Workspace.AddDatabaseFromGDX(ruta + ConstantesBase.ArchivoGdxFinal);
                GAMSVariable listaVariable = db2.GetVariable(variable);

                if (listaVariable != null)
                {
                    foreach (GAMSVariableRecord rec in listaVariable)
                    {
                        valor = Convert.ToDecimal(rec.Level) / 1000;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return valor;
        }


        public string GetConvergeGams(string ruta)
        {
            return FileHelper.LeerLogGamsDivergeConverge(ruta, ConstantesBase.NombArchivoLogGams);
        }
        #endregion

        #endregion

        /// <summary>
        /// Configura Formato de numeros al sistema internacional
        /// </summary>
        /// <returns></returns>
        public NumberFormatInfo GenerarNumberFormatInfo()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// Genera reporte excel de listado de resultado
        /// </summary>
        /// <param name="rutaArchivo"></param>
        /// <param name="topcodi"></param>
        /// <param name="subrestricodi"></param>
        /// <param name="fecha"></param>
        public void GenerarExcelListadoSalida(string rutaArchivo, int topcodi, short subrestricodi, DateTime fecha)
        {
            int row = 3;
            int col = 3;
            FileInfo newFile = new FileInfo(rutaArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("Formato");
                ws = xlPackage.Workbook.Worksheets["Formato"];
                int x = 0;
                List<CpMedicion48DTO> lista = new List<CpMedicion48DTO>();
                lista = ListaRestriciones(topcodi, subrestricodi);
                var listaRest48 = lista.Where(z => z.Medifecha == fecha).ToList();
                var listaCabecera = lista.GroupBy(z => new { z.Recurcodi, z.Recurnombre }).
                    Select(w => new CpMedicion48DTO()
                    {
                        Recurcodi = w.Key.Recurcodi,
                        Recurnombre = w.Key.Recurnombre
                    }
                    ).ToList();

                ws.Cells[row + 1, col + x++].Value = "Fecha/Hora";
                ws.Cells[row + 1, col + x++].Value = "Total";

                foreach (var reg in listaCabecera)
                {
                    ws.Cells[row + 1, col + x].Value = reg.Recurnombre;
                    x++;
                }

                int y = 1;
                decimal totvalor = 0; //Total

                ///
                if (listaRest48.Count > 0)
                {
                    for (int k = 1; k <= 48; k++)
                    {
                        x = 2;
                        totvalor = 0;
                        ws.Cells[row + 1 + y, col + 0].Value = ((fecha.AddMinutes((k - 1) * 30))).ToString(ConstantesBase.FormatoFechaHora);
                        ws.Cells[row + 1 + y, col + 1].Value = totvalor;
                        foreach (var p in listaCabecera)
                        {
                            var reg = listaRest48.Find(z => z.Recurcodi == p.Recurcodi && z.Medifecha == fecha);

                            if (reg != null)
                            {
                                ws.Cells[row + 1 + y, col + x, row + 1 + y, col + x].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[row + 1 + y, col + x, row + 1 + y, col + x].Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);
                                decimal? valor;
                                valor = (decimal?)reg.GetType().GetProperty("H" + k).GetValue(reg, null);
                                if (valor != null)
                                {
                                    ws.Cells[row + 1 + y, col + x].Value = valor;
                                    totvalor += (decimal)valor;
                                }

                                x++;
                            }
                        }
                        ws.Cells[row + 1 + y, col + 1].Value = totvalor;
                        y++;
                    }
                }
                ws.Cells[row + 1, 1, row + y, col + x - 1].AutoFitColumns();
                var stiloBorde = ws.Cells[row + 1, col, row + y, col + x - 1].Style.Border;
                stiloBorde.Bottom.Style = stiloBorde.Top.Style = stiloBorde.Left.Style = stiloBorde.Right.Style = ExcelBorderStyle.Thin;

                ws.Cells[row + 1, col].AutoFitColumns();
                Color colCabecera = System.Drawing.ColorTranslator.FromHtml("#21618C");
                Color colFecha = System.Drawing.ColorTranslator.FromHtml("#85C1E9");
                Color colTotal = System.Drawing.ColorTranslator.FromHtml("#F5B041");

                ws.Cells[row + 1, col, row + 1, col + x - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[row + 1, col, row + 1, col + x - 1].Style.Fill.BackgroundColor.SetColor(colCabecera);
                ws.Cells[row + 1, col, row + 1, col + x - 1].Style.Font.Color.SetColor(Color.White);
                ws.Cells[row + 1, col, row + 1, col + x - 1].Style.Font.Size = 12;

                //Colores Fecha
                ws.Cells[row + 1, col, row + y, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[row + 1, col, row + y, col].Style.Fill.BackgroundColor.SetColor(colFecha);
                ws.Cells[row + 1, col, row + y, col].Style.Font.Color.SetColor(Color.Blue);
                ws.Cells[row + 1, col, row + y, col].Style.Font.Size = 12;

                //Colores Fecha
                ws.Cells[row + 1, col + 1, row + y, col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[row + 1, col + 1, row + y, col + 1].Style.Fill.BackgroundColor.SetColor(colTotal);
                ws.Cells[row + 1, col + 1, row + y, col + 1].Style.Font.Color.SetColor(Color.Blue);
                ws.Cells[row + 1, col + 1, row + y, col + 1].Style.Font.Size = 12;


                xlPackage.Save();
            }


        }

        /// <summary>
        /// Genera reporte excel de reporte RSF
        /// </summary>
        /// <param name="reporteExcel"></param>
        /// <param name="topcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        public void GeneraReporteRSF(string reporteExcel, int topcodi, DateTime fechaini, DateTime fechafin)
        {
            var listaReservaUp = FactorySic.GetCpMedicion48Repository().ListaRestricion(topcodi, ConstantesYupana.SresReservaUrsUp);
            if (listaReservaUp.Count > 0)
                listaReservaUp = listaReservaUp.Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            var listaReservaDown = FactorySic.GetCpMedicion48Repository().ListaRestricion(topcodi, ConstantesYupana.SresReservaUrsDown);
            if (listaReservaDown.Count > 0)
                listaReservaDown = listaReservaDown.Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();


            var listaUrs = FactorySic.GetCpRecursoRepository().ListaUrsEmpresaAnexo5(ConstantesYupana.Urs, ConstantesYupana.UrsSicoes, topcodi).OrderBy(x => x.Gequinomb).ToList();
            var escenario = GetTopologia(topcodi);
            int iInicio = 1;
            if (escenario.Topiniciohora > 0)
                iInicio = escenario.Topiniciohora;
            GenerarArchivoExcelReporteRSF(reporteExcel, listaReservaUp, listaReservaDown, listaUrs, iInicio);
        }

        /// <summary>
        /// Genera el contenido del reporte excel de RSF
        /// </summary>
        /// <param name="rutaFile"></param>
        /// <param name="listaData"></param>
        /// <param name="lurs"></param>
        /// <param name="iInicio"></param>
        public void GenerarArchivoExcelReporteRSF(string rutaFile, List<CpMedicion48DTO> listaDataUp, List<CpMedicion48DTO> listaDataDown, List<CpRecursoDTO> lurs, int iInicio)
        {
            FileInfo newFile = new FileInfo(rutaFile);
            int fil = 3; int col = 1; int iniF = 0;
            int nFechas = 0;
            string ursNombre = string.Empty;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }


            List<DateTime> listaFechas = listaDataUp.Select(x => x.Medifecha).Distinct().ToList().OrderBy(x => x).ToList();

            ExcelWorksheet ws = null;


            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                foreach (var fecha in listaFechas)
                {
                    nFechas++;
                    iniF = (nFechas == 1) ? iInicio : 1;
                    string nombreHoja = fecha.ToString(ConstantesBase.FormatoFecha) + " - Up";
                    ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);
                    ws = xlPackage.Workbook.Worksheets[nombreHoja];
                    List<CpMedicion48DTO> ldata = listaDataUp.Where(x => x.Medifecha == fecha).ToList();
                    this.GeneraDetalleExcelReporteRSF(ws, ldata, lurs, iniF);
                    //// Down ///
                    nombreHoja = fecha.ToString(ConstantesBase.FormatoFecha) + " - Down";
                    ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);
                    ws = xlPackage.Workbook.Worksheets[nombreHoja];
                    ldata = listaDataDown.Where(x => x.Medifecha == fecha).ToList();
                    this.GeneraDetalleExcelReporteRSF(ws, ldata, lurs, iniF);

                }

                if (listaFechas.Count == 0)
                {
                    ws = xlPackage.Workbook.Worksheets.Add("sheet");
                    ws = xlPackage.Workbook.Worksheets["sheet"];
                }


                ws.View.ShowGridLines = false;
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Genera el detalle para el contenido del reporte excel de RSF
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaData"></param>
        /// <param name="lurs"></param>
        /// <param name="iInicio"></param>
        public void GeneraDetalleExcelReporteRSF(ExcelWorksheet ws, List<CpMedicion48DTO> listaData, List<CpRecursoDTO> lurs, int iInicio)
        {
            int fil = 3; int col = 2; int i = 0;
            string ursNombre = string.Empty;
            int nCol = lurs.Count;
            ws.Cells[1, 1, 200, 200].Style.Font.Name = "Arial";
            ws.Cells[1, 1, 200, 200].Style.Font.Size = 8;
            for (int z = 1; z < 200; z++)
            {
                ws.Row(z).Height = 10.8;
            }
            var listaCabecera = listaData.Select(x => x.Recurcodi).Distinct().ToList().OrderBy(x => x).ToList();
            ws.View.ShowGridLines = false;

            //primera columna
            ws.Cells[fil - 1, col - 1, fil + 49 - iInicio, col - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[fil - 1, col - 1, fil + 49 - iInicio, col - 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            CeldasExcelEnNegrita(ws, fil - 1, col - 1, fil + 49 - iInicio, col - 1);

            //última columna
            ws.Cells[fil - 1, col + nCol, fil + 49 - iInicio, col + nCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[fil - 1, col + nCol, fil + 49 - iInicio, col + nCol].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            CeldasExcelEnNegrita(ws, fil - 1, col + nCol, fil + 49 - iInicio, col + nCol);
            CeldasExcelAlinearHorizontalmente(ws, fil - 1, col + nCol, fil + 49 - iInicio, col + nCol, "Centro");

            //cabecera primera tabla
            ws.Cells[fil - 1, col, fil, col + nCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[fil - 1, col, fil, col + nCol].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            CeldasExcelEnNegrita(ws, fil - 1, col, fil, col + nCol);

            ws.Cells[fil - 1, col - 1].Value = "Hora";
            ws.Cells[fil - 1, col - 1, fil, col - 1].Merge = true;
            ws.Cells[fil - 1, col - 1, fil, col - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[fil - 1, col - 1, fil, col - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[fil - 1, col].Value = "RESERVA SECUNDARIA AUTOMÁTICA";
            ws.Cells[fil - 1, col, fil - 1, col + nCol].Merge = true;
            ws.Cells[fil - 1, col, fil - 1, col + nCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[fil, col + nCol].Value = "TOTAL";


            for (int j = iInicio; j <= 48; j++)
            {
                ws.Cells[fil + 1 + j - iInicio, col + nCol].Formula = "=SUM(" + ws.Cells[fil + 1 + j - iInicio, col].Address + ":" + ws.Cells[fil + 1 + j - iInicio, col + nCol - 1].Address + ")";
            }
            ws.Cells[fil, col + lurs.Count()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            DateTime fecha = DateTime.MinValue;
            decimal? valor;
            for (int j = iInicio; j <= 48; j++)
            {
                ws.Cells[fil + j - iInicio + 1, col - 1].Value = fecha.AddMinutes(j * 30).ToString("HH:mm");

            }

            foreach (var reg in lurs)
            {
                ws.Cells[fil, col + i].Value = reg.Gequinomb.Trim();
                var findUrs = listaData.Find(x => x.Recurcodi == reg.Recurcodi);
                if (findUrs != null)
                {
                    for (int j = iInicio; j <= 48; j++)
                    {
                        valor = (decimal?)findUrs.GetType().GetProperty("H" + j.ToString()).GetValue(findUrs, null);
                        if (valor != null)
                            ws.Cells[fil + j - iInicio + 1, col + i].Value = valor;
                    }
                }
                i++;
                ws.Column(i + 1).Width = 21;
            }
            ws.Column(i + 2).Width = 23.57;

            ws.Cells[fil + 49 - iInicio + 2, 2].Value = "Nota";
            ws.Cells[fil + 49 - iInicio + 3, 2].Value = "- Los valores mencionados en el presente cuadro implican la subida o bajada de generación de las URS en la magnitud mencionada.";
            CeldasExcelEnNegrita(ws, fil + 49 - iInicio + 2, 2, fil + 49 - iInicio + 2, 2);
            CeldasExcelEnNegrita(ws, fil + 49 - iInicio + 3, 2, fil + 49 - iInicio + 3, 2);

            ws.Cells[fil + 49 - iInicio + 6, 2].Value = "Leyenda";
            ws.Cells[fil + 49 - iInicio + 7, 2].Value = "NOMBRE";
            ws.Cells[fil + 49 - iInicio + 7, 3].Value = "EMPRESA";
            ws.Cells[fil + 49 - iInicio + 7, 4].Value = "UNIDAD/CENTRAL";
            ws.Cells[fil + 49 - iInicio + 7, 5].Value = "MARGEN DE RSF";
            ws.Cells[fil + 49 - iInicio + 7, 5, fil + 49 - iInicio + 7, 6].Merge = true;
            ws.Cells[fil + 49 - iInicio + 8, 5].Value = "MÍNIMO (MW)";
            ws.Cells[fil + 49 - iInicio + 8, 6].Value = "MÁXIMO (MW)";
            ws.Cells[fil + 49 - iInicio + 6, 2, fil + 49 - iInicio + 8, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[fil + 49 - iInicio + 6, 2, fil + 49 - iInicio + 8, 6].Style.Fill.BackgroundColor.SetColor(Color.GhostWhite);
            for (int k = 0; k < nCol; k++)
            {
                ws.Cells[fil + 49 - iInicio + 8 + k + 1, 2].Value = lurs[k].Gequinomb.Trim();
                ws.Cells[fil + 49 - iInicio + 8 + k + 1, 3].Value = lurs[k].Emprnomb.Trim();
                ws.Cells[fil + 49 - iInicio + 8 + k + 1, 4].Value = lurs[k].Centralnomb.Trim();
                ws.Cells[fil + 49 - iInicio + 8 + k + 1, 5].Value = lurs[k].Ursmin;
                ws.Cells[fil + 49 - iInicio + 8 + k + 1, 6].Value = lurs[k].Ursmax;
            }


            var rangoc2 = ws.Cells[fil + 49 - iInicio + 7, 2, fil + 48 + 6 + 2 + nCol - iInicio + 1, 6];
            rangoc2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rangoc2.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rangoc2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rangoc2.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rangoc2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rangoc2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            ws.Calculate();
            var rango = ws.Cells[fil - 1, col - 1, fil + 49 - iInicio, col + i];
            rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //rango.AutoFitColumns();

            //CeldasExcelAgrupar(ws, fil + 49 - iInicio + 6, 2, fil + 49 - iInicio + 6, 6);
            CeldasExcelAgrupar(ws, fil + 49 - iInicio + 7, 2, fil + 49 - iInicio + 7 + 1, 2);
            CeldasExcelAgrupar(ws, fil + 49 - iInicio + 7, 3, fil + 49 - iInicio + 7 + 1, 3);
            CeldasExcelAgrupar(ws, fil + 49 - iInicio + 7, 4, fil + 49 - iInicio + 7 + 1, 4);
            CeldasExcelEnNegrita(ws, fil + 49 - iInicio + 6, 2, fil + 49 - iInicio + 7 + 1, 6);

            CeldasExcelAlinearHorizontalmente(ws, fil + 49 - iInicio + 6, 2, fil + 49 - iInicio + 6, 6, "Centro");

            //bordes
            BorderCeldasPerimetroGrueso(ws, fil + 49 - iInicio + 6, 2, fil + 49 - iInicio + 6, 6);
            BorderCeldasContinuaGruesa(ws, fil + 49 - iInicio + 7, 2, fil + 49 - iInicio + 8, 6);
            BorderCeldasPerimetroGrueso(ws, fil + 49 - iInicio + 9, 2, fil + 48 + 6 + 2 + nCol - iInicio + 1, 4);
            BorderCeldasPerimetroGrueso(ws, fil + 49 - iInicio + 9, 5, fil + 48 + 6 + 2 + nCol - iInicio + 1, 6);
        }

        /// <summary>
        /// Alinear  horizontalmente a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="alineacion"></param>
        /// <returns></returns>
        public void CeldasExcelAlinearHorizontalmente(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string alineacion)
        {
            var rg = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            switch (alineacion)
            {
                case "General": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.General; break;
                case "Izquierda": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; break;
                case "Centro": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; break;
                case "CentroContinuo": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous; break;
                case "Derecha": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; break;
                case "Lleno": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Fill; break;
                case "Distribuido": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Distributed; break;
                case "Justificado": rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify; break;
            }
        }
        /// <summary>
        ///  Colocar en negrita a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <returns></returns>
        public void CeldasExcelEnNegrita(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            if (filaIni <= filaFin)
            {
                var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
                bloque.Style.Font.Bold = true;
            }
        }

        /// <summary>
        ///  Agrupar varias celdas en un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <returns></returns>
        public void CeldasExcelAgrupar(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            ws.Cells[filaIni, coluIni, filaFin, coluFin].Merge = true;
        }

        /// <summary>
        ///  Dar Wrap a una celda de la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <returns></returns>
        public void CeldasExcelWrapText(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.WrapText = true;
        }

        /// <summary>
        /// Bordea el perimetro con linea gruesa
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public void BorderCeldasPerimetroGrueso(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin) //BORDES
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Style = ExcelBorderStyle.Thick;
            ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            ws.Cells[rowIni, colIni, rowFin, colIni].Style.Border.Left.Style = ExcelBorderStyle.Thick;
            ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Style = ExcelBorderStyle.Thick;
        }

        /// <summary>
        /// Bordea cada celda con linea continua y delgada
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public void BorderCeldasContinuaDelgada(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Bordea cada celda con linea continua y gruesa
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public void BorderCeldasContinuaGruesa(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thick;
        }

    }
}
