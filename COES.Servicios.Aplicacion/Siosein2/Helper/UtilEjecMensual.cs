using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.SIOSEIN;
using OfficeOpenXml;
using OfficeOpenXml.ConditionalFormatting;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace COES.Servicios.Aplicacion.Siosein2.Helper
{
    public static class UtilEjecMensual
    {
        /// <summary>
        /// Metodo que retorna fechas actuales y anteriores apartir de un rango de fechas para el Informe Mensual
        /// </summary>
        /// <param name="fechaActualInicial"></param>
        /// <returns></returns>
        public static FechasPR5 ObtenerFechasEjecutivoMensual(DateTime fechaActualInicial)
        {
            //las fechas son iguales para el informe mensual y ejecutivo mensual
            var obj = UtilInfMensual.ObtenerFechasInformesMensual(fechaActualInicial);
            obj.TipoReporte = ConstantesPR5ReportesServicio.ReptipcodiEjecutivoMensual;

            return obj;
        }

        #region 1. PRODUCCION Y POTENCIA COINCIDENTE EN BORNES DE GENERACIÓN DEL SEIN

        #region 1.1. Produccion por empresa generadora

        public static List<GenericoDTO> ListarFilaCuadro1_1TIE()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroImportacion, String1 = "IMPORTACIÓN DESDE ECUADOR" },
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroExportacion, String1 = "EXPORTACION HACIA ECUADOR" },
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalTIE, String1 = "TOTALES INTERCAMBIOS INTERNACIONALES" },
                                }.ToList();

            return listaCuadro;
        }

        public static List<GenericoDTO> ListarFilaCuadro1_1Gen()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalGeneracion, String1 = "TOTAL" },
                                }.ToList();

            return listaCuadro;
        }

        public static List<GenericoDTO> ListarFilaCuadro1_1Sein()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalSein, String1 = "TOTAL DEMANDA DEL SEIN" },
                                }.ToList();

            return listaCuadro;
        }

        public static TablaReporte ObtenerDataTablaResumenProduccionMensual(FechasPR5 objFecha, List<SiEmpresaDTO> listaEmpresa,
                List<MaximaDemandaDTO> listaMDCoincidenteDataDesc,
                List<ResultadoTotalGeneracion> listaTgen, List<ResultadoTotalGeneracion> listaEnergEjec, List<ResultadoTotalGeneracion> listaMDEnerg,
                List<ResultadoTotalGeneracion> listaTotalTgen, List<ResultadoTotalGeneracion> listaTotalEnergEjec, List<ResultadoTotalGeneracion> listaTotalMDEnerg,
                List<ResultadoTotalGeneracion> listaTotalyTIEEnergEjec, List<ResultadoTotalGeneracion> listaTotalyTIEMDEnerg,
                List<ResultadoTotalGeneracion> listaTIEC3Total, List<ResultadoTotalGeneracion> listaTIEC3MD)
        {
            var regMDAct = listaMDCoincidenteDataDesc.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
            var regMDAnt = listaMDCoincidenteDataDesc.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
            var regMDActAcum = listaMDCoincidenteDataDesc.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);

            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = objFecha.TipoReporte;
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[4, 11];

            matrizCabecera[0, 0] = "EMPRESAS";
            matrizCabecera[0, 1] = "PRODUCCIÓN DE ENERGÍA ELÉCTRICA";
            matrizCabecera[0, 7] = "MÁXIMA POTENCIA COINCIDENTE   (MW)";

            matrizCabecera[1, 1] = objFecha.AnioAct.RangoAct_NumYAnio;
            matrizCabecera[1, 5] = objFecha.Anio1Ant.RangoAct_NumYAnio;
            matrizCabecera[1, 6] = "%";
            matrizCabecera[1, 7] = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(objFecha.AnioAct.NumMes), objFecha.AnioAct.NumAnio);
            matrizCabecera[1, 8] = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(objFecha.Anio1Ant.NumMes), objFecha.Anio1Ant.NumAnio);
            matrizCabecera[1, 9] = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(regMDActAcum.FechaHora.Month), regMDActAcum.FechaHora.Year);
            matrizCabecera[1, 10] = string.Format("Variación {0}{1} / {0}{2}", EPDate.f_NombreMesCorto(objFecha.AnioAct.NumMes), objFecha.AnioAct.NumAnio, objFecha.Anio1Ant.NumAnio);

            matrizCabecera[2, 1] = "HIDROELÉCTRICA";
            matrizCabecera[2, 2] = "TERMOELÉCTRICA";
            matrizCabecera[2, 3] = "RER(***)";
            matrizCabecera[2, 4] = "TOTAL";
            matrizCabecera[2, 7] = regMDAct.FechaOnlyDia;
            matrizCabecera[2, 8] = regMDAnt.FechaOnlyDia;
            matrizCabecera[2, 9] = regMDActAcum.FechaOnlyDia;

            matrizCabecera[3, 1] = "GWh";
            matrizCabecera[3, 2] = "GWh";
            matrizCabecera[3, 3] = "GWh";
            matrizCabecera[3, 4] = "GWh";
            matrizCabecera[3, 5] = "GWh";
            matrizCabecera[3, 7] = regMDAct.FechaOnlyHora;
            matrizCabecera[3, 8] = regMDAnt.FechaOnlyHora;
            matrizCabecera[3, 9] = regMDActAcum.FechaOnlyHora;

            cabRepo.CabeceraData = matrizCabecera;
            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            //Por empresa
            var contador = 0;
            foreach (var regFila in listaEmpresa)
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaXTgen = listaTgen.Where(x => x.Emprcodi == regFila.Emprcodi).ToList();
                var listaEnergEjecXemp = listaEnergEjec.Where(x => x.Emprcodi == regFila.Emprcodi).ToList();
                var listaMDEnergXemp = listaMDEnerg.Where(x => x.Emprcodi == regFila.Emprcodi).ToList();

                ResultadoTotalGeneracion regTgenHidro = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiHidro);
                ResultadoTotalGeneracion regTgenTermo = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiTermo);
                ResultadoTotalGeneracion regTgenRER = listaXTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiRER);

                ResultadoTotalGeneracion regEnergEjecAnio0G = listaEnergEjecXemp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regEnergEjecAnio1G = listaEnergEjecXemp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regEnergEjecVarAnio0G = listaEnergEjecXemp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regMDAnio0G = listaMDEnergXemp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaMDEnergXemp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDAnio0AcumG = listaMDEnergXemp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                ResultadoTotalGeneracion regMDVarAnio0G = listaMDEnergXemp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                datos.Add(regTgenHidro.Meditotal != 0 ? regTgenHidro.Meditotal : null);
                datos.Add(regTgenTermo.Meditotal != 0 ? regTgenTermo.Meditotal : null);
                datos.Add(regTgenRER.Meditotal != 0 ? regTgenRER.Meditotal : null);

                datos.Add(regEnergEjecAnio0G.Meditotal);
                datos.Add(regEnergEjecAnio1G.Meditotal);
                datos.Add(regEnergEjecVarAnio0G.Meditotal);

                datos.Add(regMDAnio0G.Meditotal != 0 ? regMDAnio0G.Meditotal : null);
                datos.Add(regMDAnio1G.Meditotal != 0 ? regMDAnio1G.Meditotal : null);
                datos.Add(regMDAnio0AcumG.Meditotal != 0 ? regMDAnio0AcumG.Meditotal : null);
                datos.Add(regMDVarAnio0G.Meditotal);

                registro.Nombre = regFila.Emprnomb;
                registro.ListaData = datos;

                registro.ColorFila = contador % 2 != 0 ? "#D9E1F2" : "#FFFFFF";
                contador++;

                registros.Add(registro);
            }

            //fila total generacion
            foreach (var regFila in ListarFilaCuadro1_1Gen())
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                ResultadoTotalGeneracion regTgenHidro = listaTotalTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiHidro);
                ResultadoTotalGeneracion regTgenTermo = listaTotalTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiTermo);
                ResultadoTotalGeneracion regTgenRER = listaTotalTgen.Find(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiRER);

                ResultadoTotalGeneracion regEnergEjecAnio0G = listaTotalEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regEnergEjecAnio1G = listaTotalEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regEnergEjecVarAnio0G = listaTotalEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regMDAnio0G = listaTotalMDEnerg.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaTotalMDEnerg.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDAnio0AcumG = listaTotalMDEnerg.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                ResultadoTotalGeneracion regMDVarAnio0G = listaTotalMDEnerg.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                datos.Add(regTgenHidro.Meditotal);
                datos.Add(regTgenTermo.Meditotal);
                datos.Add(regTgenRER.Meditotal);

                datos.Add(regEnergEjecAnio0G.Meditotal);
                datos.Add(regEnergEjecAnio1G.Meditotal);
                datos.Add(regEnergEjecVarAnio0G.Meditotal);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDAnio0AcumG.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                registro.Nombre = regFila.String1;
                registro.ListaData = datos;
                registro.EsFilaResumen = true;
                registro.ColorFila = "#8EA9DB";

                registros.Add(registro);
            }

            //Agregar 3 filas de Interconexion
            contador = 0;
            foreach (var regFila in ListarFilaCuadro1_1TIE())
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaDataC3 = listaTIEC3Total.Where(x => x.TipoSemanaRelProd == regFila.Entero1).ToList();
                var listaDataMD = listaTIEC3MD.Where(x => x.TipoSemanaRelProd == regFila.Entero1).ToList();

                ResultadoTotalGeneracion regEnergEjecAnio0G = listaDataC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regEnergEjecAnio1G = listaDataC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regEnergEjecVarAnio0G = listaDataC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regMDAnio0G = listaDataMD.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaDataMD.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDAnio0AcumG = listaDataMD.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                ResultadoTotalGeneracion regMDVarAnio0G = listaDataMD.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                datos.Add(null);
                datos.Add(null);
                datos.Add(null);

                datos.Add(regEnergEjecAnio0G.Meditotal);
                datos.Add(regEnergEjecAnio1G.Meditotal);
                datos.Add(regEnergEjecVarAnio0G.Meditotal);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDAnio0AcumG.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                registro.Nombre = regFila.String1;
                registro.ListaData = datos;

                registro.ColorFila = contador == 2 ? "#D9E1F2" : "#FFFFFF";
                contador++;

                registros.Add(registro);
            }

            //fila total sein
            foreach (var regFila in ListarFilaCuadro1_1Sein())
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                ResultadoTotalGeneracion regEnergEjecAnio0G = listaTotalyTIEEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regEnergEjecAnio1G = listaTotalyTIEEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regEnergEjecVarAnio0G = listaTotalyTIEEnergEjec.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regMDAnio0G = listaTotalyTIEMDEnerg.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaTotalyTIEMDEnerg.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDAnio0AcumG = listaTotalyTIEMDEnerg.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                ResultadoTotalGeneracion regMDVarAnio0G = listaTotalyTIEMDEnerg.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                datos.Add(null);
                datos.Add(null);
                datos.Add(null);

                datos.Add(regEnergEjecAnio0G.Meditotal);
                datos.Add(regEnergEjecAnio1G.Meditotal);
                datos.Add(regEnergEjecVarAnio0G.Meditotal);

                datos.Add(regMDAnio0G.Meditotal != 0 ? regMDAnio0G.Meditotal : null);
                datos.Add(regMDAnio1G.Meditotal != 0 ? regMDAnio1G.Meditotal : null);
                datos.Add(regMDAnio0AcumG.Meditotal != 0 ? regMDAnio0AcumG.Meditotal : null);
                datos.Add(regMDVarAnio0G.Meditotal);

                registro.Nombre = regFila.String1;
                registro.ListaData = datos;
                registro.EsFilaResumen = true;
                registro.ColorFila = "#8EA9DB";

                registros.Add(registro);
            }

            #endregion

            tabla.ListaRegistros = registros;

            return tabla;
        }

        public static string GenerarRHtmlProduccionEmpresaGeneradora(FechasPR5 objFecha, string textoResumen, TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo2();
            var tamTabla = 1300;

            strHtml.Append("<div id='listado_reporte' style='height: auto; width: " + tamTabla + "px;'>");

            strHtml.AppendFormat(textoResumen);

            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: " + (tamTabla - 30) + "px;'>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th rowspan='4' style='width: 285px;'>{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th colspan='6'>{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th colspan='4'>{0}</th>", dataCab[0, 7]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th colspan='4'>{0}</th>", dataCab[1, 1]);
            strHtml.AppendFormat("<th rowspan='2' style='width: 120px;'>{0}</th>", dataCab[1, 5]);
            strHtml.AppendFormat("<th rowspan='3' style=''>{0}</th>", dataCab[1, 6]);
            strHtml.AppendFormat("<th style='width: 120px;'>{0}</th>", dataCab[1, 7]);
            strHtml.AppendFormat("<th style='width: 120px;'>{0}</th>", dataCab[1, 8]);
            strHtml.AppendFormat("<th style='width: 120px;'>{0}</th>", dataCab[1, 9]);
            strHtml.AppendFormat("<th rowspan='3' style='width: 120px;'>{0}</th>", dataCab[1, 10]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 120px;'>{0}</th>", dataCab[2, 1]);
            strHtml.AppendFormat("<th style='width: 120px;'>{0}</th>", dataCab[2, 2]);
            strHtml.AppendFormat("<th style='width: 120px;'>{0}</th>", dataCab[2, 3]);
            strHtml.AppendFormat("<th style='width: 120px;'>{0}</th>", dataCab[2, 4]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 7]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 8]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 9]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 1]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 2]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 3]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 4]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 5]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 7]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 8]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 9]);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            int f = 0;
            foreach (var reg in registros)
            {
                string styleTotal = "background-color: " + reg.ColorFila + ";";
                styleTotal += reg.EsFilaResumen ? "font-weight: bold;padding-top: 5px; padding-bottom: 5px;" : "";

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;{1}'>{0}</td>", reg.Nombre, styleTotal);

                int c = 1;
                foreach (decimal? col in reg.ListaData)
                {

                    if (c == 6 || c == 10) //con signo  de %
                        strHtml.AppendFormat("<td class='alignValorRight' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirVariacionHtml(col.HasValue ? col : null, nfi2), styleTotal);
                    else
                        strHtml.AppendFormat("<td class='alignValorRight' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi2), styleTotal);

                    c++;
                }
                strHtml.Append("</tr>");
                f++;
            }
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Generar reporte excel de Produccion por empresa generadora
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static void GenerarHojaTblExcelProduccionEnergXEmpresa(ExcelWorksheet ws, SiVersionDTO objVersion, FechasPR5 objFecha, string resumen,
                                                    TablaReporte tablaData, List<SiNotaDTO> listaNotas)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            ObtenerDatosCabeceraExcel(ws, objVersion, objFecha);
            //INICIO DE LA TABLA PRODUCCIÓN POR EMPRESA GENERADORA

            int filaIniEmpresa = 7;
            int coluIniEmpresa = 1;
            int ultimaFilaTabla = filaIniEmpresa;

            int filaIniData = filaIniEmpresa + 4;
            int coluIniData = coluIniEmpresa;

            int ultimaFila = 0;
            int ultimaColu = 0;

            ws.Cells[4, 1].Value = resumen;

            #region cabecera
            ws.Cells[filaIniEmpresa, coluIniEmpresa + 0].Value = dataCab[0, 0];
            ws.Cells[filaIniEmpresa, coluIniEmpresa + 1].Value = dataCab[0, 1];
            ws.Cells[filaIniEmpresa, coluIniEmpresa + 7].Value = dataCab[0, 7];

            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 1].Value = dataCab[1, 1];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 5].Value = dataCab[1, 5];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 6].Value = dataCab[1, 6];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 7].Value = dataCab[1, 7];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 8].Value = dataCab[1, 8];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 9].Value = dataCab[1, 9];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 10].Value = dataCab[1, 10];

            ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 1].Value = dataCab[2, 1];
            ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 2].Value = dataCab[2, 2];
            ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 3].Value = dataCab[2, 3];
            ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 4].Value = dataCab[2, 4];
            ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 7].Value = dataCab[2, 7];
            ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 8].Value = dataCab[2, 8];
            ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 9].Value = dataCab[2, 9];

            ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 1].Value = dataCab[3, 1];
            ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 2].Value = dataCab[3, 2];
            ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 3].Value = dataCab[3, 3];
            ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 4].Value = dataCab[3, 4];
            ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 5].Value = dataCab[3, 5];
            ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 7].Value = dataCab[3, 7];
            ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 8].Value = dataCab[3, 8];
            ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 9].Value = dataCab[3, 9];

            #endregion

            ultimaColu = coluIniEmpresa + 10;

            #region cuerpo

            ultimaFila = filaIniData + registros.Count() - 1;
            ultimaFilaTabla = ultimaFila;

            #region Formato Cuerpo
            //UtilExcel.CeldasExcelWrapText(ws, filaIniData, coluIniData, ultimaFila, coluIniData);
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData + 1, ultimaFila, ultimaColu, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData, ultimaFila, coluIniData, "Izquierda");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 1, ultimaFila, ultimaColu, "Derecha");
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniData, coluIniData + 0, ultimaFila, coluIniData + 0);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo3);

            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, 1, ultimaFila, 1, "#5B9BD5");
            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, 2, ultimaFila, 5, "#5B9BD5");
            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, 6, ultimaFila, 6, "#5B9BD5");
            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, 7, ultimaFila, 7, "#5B9BD5");
            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, 8, ultimaFila, 10, "#5B9BD5");
            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, 11, ultimaFila, 11, "#5B9BD5");

            #endregion

            int filaX = 0;
            foreach (var reg in registros)
            {
                int colX = 0;

                ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre;
                colX++;

                if (reg.EsFilaResumen)
                {
                    UtilExcel.CeldasExcelEnNegrita(ws, filaIniData + filaX, coluIniData, filaIniData + filaX, ultimaColu);
                }

                foreach (decimal? numValor in reg.ListaData)
                {
                    string strFormat = ConstantesPR5ReportesServicio.FormatoNumero2Digito;

                    if (numValor != null)
                    {
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Value = numValor;
                    }
                    ws.Cells[filaIniData + filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                    colX++;
                }

                if (!string.IsNullOrEmpty(reg.ColorFila))
                    UtilExcel.CeldasExcelColorFondo(ws, filaIniData + filaX, coluIniData, filaIniData + filaX, ultimaColu, reg.ColorFila);

                filaX++;
            }
            #endregion

            #region NotaBD
            int filaIniNotasBD = ultimaFilaTabla + 2 + 2;
            int coluIniNotasBD = coluIniEmpresa;

            int numNotas;
            UtilEjecMensual.ColocarNotasEnReporte(ws, filaIniNotasBD, coluIniNotasBD, listaNotas, out numNotas);

            #endregion

        }

        /// <summary>
        /// Generar el titulo, fecha, version del reporte exportado
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        public static void ObtenerDatosCabeceraExcel(ExcelWorksheet ws, SiVersionDTO objVersion, FechasPR5 objFecha)
        {
            int filaAGuardar = 2;
            int coluAGuardar = 26;

            int numVersion = objVersion.Verscorrelativo;
            DateTime fechaInicio = objFecha.AnioAct.Fecha_Inicial;

            ws.Cells[filaAGuardar, coluAGuardar].Value = "INFORME EJECUTIVO MENSUAL DE LA OPERACIÓN DEL SEIN \n" + ExtensionMethod.NombreMesAnho(fechaInicio).ToUpper();
            ws.Cells[filaAGuardar + 1, coluAGuardar].Value = "Código: EJECSGI-" + string.Format("MES{0:D2}", fechaInicio.Month) + "-" + fechaInicio.Year + " \n Fecha: " + DateTime.Now.ToString("dd/MM/yyyy") + " \n Versión: " + numVersion;
        }

        #endregion

        #region 1.2. Producción total de centrales de generación eléctrica

        public static ResultadoTotalGeneracion ResumenProduccionObtenerDataGWhTotal(List<MeMedicion96DTO> listaData96, int tipoResultadoFecha, DateTime fechaProceso, DateTime fechaIniConsulta, DateTime fechaFinConsulta,
                            int tgenercodi, int emprcodi, int tipoSemanaRelProd = 0)
        {
            ResultadoTotalGeneracion m = new ResultadoTotalGeneracion();
            m.Medifecha = fechaProceso;
            m.TipoResultadoFecha = tipoResultadoFecha;
            m.Meditotal = 0;

            m.Tgenercodi = tgenercodi;
            m.Emprcodi = emprcodi;
            /*m.Equipadre = equipadre;*/
            m.TipoSemanaRelProd = tipoSemanaRelProd;

            if (listaData96.Count > 0)
            {
                decimal total = 0;
                foreach (var aux in listaData96)
                {
                    total += aux.Meditotal.GetValueOrDefault(0);
                }

                m.Meditotal = total / 4000.0m; //cuarto de hora a GWh
            }

            m.FiltroCeldaDato = new FiltroCeldaDato()
            {
                FechaIni = fechaIniConsulta,
                FechaFin = fechaFinConsulta,
            };

            return m;
        }

        public static List<GenericoDTO> ListarFilaCuadro1_2()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroHidroelectrica, String1 = "HIDROELÉCTRICAS" },
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTermoelectrica, String1 = "TERMOELÉCTRICAS" },
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroRenovable, String1 = "RENOVABLES (*)" },
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalGeneracion, String1 = "TOTAL" }
                                }.ToList();

            return listaCuadro;
        }

        public static TablaReporte ObtenerDataTablaProduccionXCentral(FechasPR5 objFecha, List<ResultadoTotalGeneracion> listaCeldaData)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = objFecha.TipoReporte;
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[2, 5];

            matrizCabecera[0, 0] = "CENTRALES";
            matrizCabecera[0, 1] = string.Format("{0}", objFecha.AnioAct.NumAnio);
            matrizCabecera[0, 2] = string.Format("{0}", objFecha.Anio1Ant.NumAnio);
            matrizCabecera[0, 3] = "VARIACIÓN";

            matrizCabecera[1, 1] = "GWh";
            matrizCabecera[1, 2] = "GWh";
            matrizCabecera[1, 3] = "GWh";
            matrizCabecera[1, 4] = "%";

            cabRepo.CabeceraData = matrizCabecera;
            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            //Por tipo de Generación
            foreach (var regFila in ListarFilaCuadro1_2())
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaXTgen = listaCeldaData.Where(x => x.TipoSemanaRelProd == regFila.Entero1).ToList();

                ResultadoTotalGeneracion regTotalAnio0 = listaXTgen.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regTotalAnio1 = listaXTgen.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regDifEnerg = listaXTgen.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Resta);
                ResultadoTotalGeneracion regTotalVarAnio0 = listaXTgen.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                datos.Add(regTotalAnio0.Meditotal);
                datos.Add(regTotalAnio1.Meditotal);
                datos.Add(regDifEnerg.Meditotal);
                datos.Add(regTotalVarAnio0.Meditotal);

                registro.Nombre = regFila.String1;
                registro.ListaData = datos;
                registro.EsFilaResumen = regFila.Entero1 == ConstantesSiosein2.FilaCuadroTotalGeneracion;

                registros.Add(registro);
            }

            #endregion

            tabla.ListaRegistros = registros;

            return tabla;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static string GenerarRHtmlProduccionTotalCentralesGeneracion(TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi3 = UtilAnexoAPR5.GenerarNumberFormatInfo3();
            var tamTabla = 800;

            strHtml.Append("<div id='listado_reporte' style='height: auto; width: " + tamTabla + "px;'>");

            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: " + (tamTabla - 30) + "px;'>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th rowspan='2' style='width: 285px;'>{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th>{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th>{0}</th>", dataCab[0, 2]);
            strHtml.AppendFormat("<th colspan='2' >{0}</th>", dataCab[0, 3]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 100px;'>{0}</th>", dataCab[1, 1]);
            strHtml.AppendFormat("<th style='width: 100px;'>{0}</th>", dataCab[1, 2]);
            strHtml.AppendFormat("<th style='width: 100px;'>{0}</th>", dataCab[1, 3]);
            strHtml.AppendFormat("<th style='width: 100px;'>{0}</th>", dataCab[1, 4]);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            int f = 0;
            foreach (var reg in registros)
            {
                if (!reg.EsFilaResumen)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;font-weight:bold;'>{0}</td>", reg.Nombre);

                    int c = 0;
                    foreach (decimal? col in reg.ListaData)
                    {
                        if (c == 4) //con signo  de %
                            strHtml.AppendFormat("<td class='alignValorRight' >{0}</td>", UtilAnexoAPR5.ImprimirVariacionHtml(col.HasValue ? col : null, nfi3));
                        else
                            strHtml.AppendFormat("<td class='alignValorRight' >{0}</td>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi3));

                        c++;
                    }
                }
                strHtml.Append("</tr>");
                f++;
            }
            strHtml.Append("</tbody>");

            strHtml.Append("<thead>");
            foreach (var reg in registros)
            {
                if (reg.EsFilaResumen)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<th style='padding-left: 5px;text-align: left;font-weight:bold;'>{0}</th>", reg.Nombre);

                    int c = 0;
                    foreach (decimal? col in reg.ListaData)
                    {
                        if (c == 4) //con signo  de %
                            strHtml.AppendFormat("<th class='alignValorRight' >{0}</th>", UtilAnexoAPR5.ImprimirVariacionHtml(col.HasValue ? col : null, nfi3));
                        else
                            strHtml.AppendFormat("<th class='alignValorRight' >{0}</th>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi3));

                        c++;
                    }

                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</thead>");
            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        public static void GenerarHojaTblExcelProduccionTotalCentralesDeGeneracion(ExcelWorksheet ws, SiVersionDTO objVersion, FechasPR5 objFecha, TablaReporte tablaData)
        {
            var ultimoDiaDelMes = objFecha.AnioAct.Fecha_Final;

            int filaIniTabla = 5;

            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            int filaIniCab = filaIniTabla;
            int filaIniData = filaIniCab + 2;
            int coluIniData = 2;

            ws.Cells[3, 1].Value = string.Format("1.2. PRODUCCIÓN TOTAL DE CENTRALES DE GENERACIÓN ELÉCTRICA (ACUMULADO A {0})", EPDate.f_NombreMes(objFecha.AnioAct.NumMes).ToUpper());

            #region cabecera

            ws.Cells[filaIniCab, coluIniData + 0].Value = dataCab[0, 0];
            ws.Cells[filaIniCab, coluIniData + 1].Value = dataCab[0, 1];
            ws.Cells[filaIniCab, coluIniData + 2].Value = dataCab[0, 2];
            ws.Cells[filaIniCab, coluIniData + 3].Value = dataCab[0, 3];

            ws.Cells[filaIniCab + 1, coluIniData + 1].Value = dataCab[1, 1];
            ws.Cells[filaIniCab + 1, coluIniData + 2].Value = dataCab[1, 2];
            ws.Cells[filaIniCab + 1, coluIniData + 3].Value = dataCab[1, 3];
            ws.Cells[filaIniCab + 1, coluIniData + 4].Value = dataCab[1, 4];

            #endregion

            #region cuerpo

            int filaX = 0;
            foreach (var reg in registros)
            {
                int colX = 0;

                ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre;
                colX++;
                foreach (decimal? numValor in reg.ListaData)
                {
                    string strFormat = string.Empty;

                    bool tieneTextoPorcentaje = colX == 4;
                    if (numValor != null)
                    {
                        var numValor2 = tieneTextoPorcentaje ? numValor / 100 : numValor;
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Value = numValor2;
                    }
                    strFormat = tieneTextoPorcentaje ? ConstantesPR5ReportesServicio.FormatoNumero3DigitoPorcentaje : ConstantesPR5ReportesServicio.FormatoNumero3Digito;
                    ws.Cells[filaIniData + filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                    colX++;
                }

                filaX++;
            }

            #endregion

            #region Nota
            ws.Cells[filaIniTabla + 6, coluIniData].Value = "Cuadro N°2: Producción de energía eléctrica acumulada al " + ultimoDiaDelMes.Day + " de " + ExtensionMethod.NombreMes(ultimoDiaDelMes).ToLower();
            UtilEjecMensual.FormatoNota(ws, filaIniTabla + 6, coluIniData);
            #endregion
        }

        #endregion

        #region 1.3. Participación por empresas en la producción total de energía del mes

        /// <summary>
        /// Genera grafico web de la participacion por empresa en la prod total
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaEmpresas"></param>
        /// <returns></returns>
        public static GraficoWeb GenerarGWebParticipacionXEmpresaProducTotal(DateTime fechaInicio, List<SiEmpresaDTO> listaEmpresas)
        {
            var primerDiaDelMes = fechaInicio;

            //****************
            var graficoWeb = new GraficoWeb
            {
                TitleText = "Participación por empresas en la producción total de energía del mes de " + primerDiaDelMes.NombreMesAnho(),
                Type = "pie",
                TooltipPointFormat = "{series.name}'",
                PlotOptionsFormat = "{point.name}"
            };
            var serieData = new DatosSerie[listaEmpresas.Count];


            for (int i = 0; i < listaEmpresas.Count; i++)
            {
                var empresa = listaEmpresas[i];
                serieData[i] = new DatosSerie()
                {
                    Name = empresa.Emprnomb,
                    Y = empresa.Total
                };
            }

            graficoWeb.SerieData = serieData.ToArray();

            return graficoWeb;
        }

        /// <summary>
        /// Retorna objeto GraficoWeb para la varia
        /// </summary>
        /// <param name="listaData"></param>
        /// <param name="titulo"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>        
        public static GraficoWeb GenerarGWebParticipacionXEmpresaProducTotalMenores(string titulo, List<SiEmpresaDTO> listaEmpresas)
        {
            var graficoWeb = new GraficoWeb
            {
                Type = "bar",
                TitleText = titulo,
                XAxisCategories = listaEmpresas.Select(x => x.Emprnomb).ToList(),
                YaxixTitle = "Participación (%)",
                TooltipValueSuffix = " %",
                YaxixLabelsFormat = "%",
                XAxisLabelsRotation = 0,
            };

            graficoWeb.SerieData = new DatosSerie[]
            {
                new DatosSerie
                {
                    Name = "Participación",
                    Data = listaEmpresas.Select(x=> (decimal?)x.Total).ToArray()
                }
            };

            return graficoWeb;
        }

        /// <summary>
        ///  Escoge el chartPie idoneo (de 4) para el tamaño de la lista
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="tamanioLista"></param>
        /// <param name="posFilaIni"></param>
        /// <param name="ultimo"></param>
        /// <returns>escogido</returns>
        /// <returns>coluNombre</returns>
        /// <returns>filaInicio</returns>
        public static void EscogerMejorGraficaPie(ExcelWorksheet ws, int tamanioLista, int posFilaIni, int posColuIni, out ExcelPieChart escogido, out int coluNombre, out int filaInicio, bool ultimo)
        {
            var pieChart20 = ws.Drawings["Chart20"] as ExcelPieChart;
            var pieChart30 = ws.Drawings["Chart30"] as ExcelPieChart;
            var pieChart40 = ws.Drawings["Chart40"] as ExcelPieChart;
            var pieChart50 = ws.Drawings["Chart50"] as ExcelPieChart;

            int coluNombre1;   // columna inicial de donde se pintara los datos            
            int filaInicio1;   // fila inicial de donde se pintaran los datos           

            if (tamanioLista <= 20)
            {
                escogido = pieChart20;
                pieChart20.Name = "GraficoEmpresas";
                coluNombre1 = 53;
                filaInicio1 = 10;

            }
            else
            {
                if (tamanioLista <= 30)
                {
                    escogido = pieChart30;
                    pieChart30.Name = "GraficoEmpresas";
                    coluNombre1 = 58;
                    filaInicio1 = 10;
                }
                else
                {
                    if (tamanioLista <= 40)
                    {
                        escogido = pieChart40;
                        pieChart40.Name = "GraficoEmpresas";
                        coluNombre1 = 63;
                        filaInicio1 = 10;
                    }
                    else
                    {
                        escogido = pieChart50;
                        pieChart50.Name = "GraficoEmpresas";
                        coluNombre1 = 68;
                        filaInicio1 = 10;
                    }
                }
            }

            escogido.SetPosition(posFilaIni, 0, posColuIni, 0);
            coluNombre = coluNombre1;
            filaInicio = filaInicio1;

            if (ultimo)
            {
                if (pieChart20.Name != "GraficoEmpresas") pieChart20.SetSize(0, 0);
                if (pieChart30.Name != "GraficoEmpresas") pieChart30.SetSize(0, 0);
                if (pieChart40.Name != "GraficoEmpresas") pieChart40.SetSize(0, 0);
                if (pieChart50.Name != "GraficoEmpresas") pieChart50.SetSize(0, 0);

            }

        }

        /// <summary>
        /// Genera el gráfico en el excel del reporte Participación por empresas en la producción de energía del mes menores al 1%
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="tituloZona"></param>
        /// <param name="filaInicioTabla"></param>
        /// <param name="coluInicioTabla"></param>
        /// <param name="numDatos"></param>
        /// <returns></returns>
        private static void GenerarChartExcelGraficoProdempresasMenor1porc(ExcelWorksheet ws, int filaInicioTabla, int coluInicioTabla, int numDatos, string titulo)
        {
            var miChart = ws.Drawings["GraficoEmpresasA"] as ExcelChart;
            //miChart.SetPosition(filaInicioTabla, 0, 7, 0);
            //miChart.SetSize(320, numDatos * 21);
            miChart.Title.Text = titulo;

            miChart.Series[0].Series = ExcelRange.GetAddress(filaInicioTabla, coluInicioTabla + 1, filaInicioTabla + numDatos + 1, coluInicioTabla + 1);
            miChart.Series[0].XSeries = ExcelRange.GetAddress(filaInicioTabla, coluInicioTabla, filaInicioTabla + numDatos + 1, coluInicioTabla);
            //miChart.Series[0].Header = (string)ws.Cells[filaInicioTabla + 1, coluInicioTabla + 5].Value;

        }

        /// <summary>
        /// Genera excel de la participacion por empresa en la prod total
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="versionAnexo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public static void GenerarHojaChartExcelParticipacionXEmpresasProduccionMen(ExcelWorksheet ws, SiVersionDTO objVersion, FechasPR5 objFecha,
                            List<SiEmpresaDTO> listaEmpresas, List<SiEmpresaDTO> listaEmpresasOtros)
        {
            DateTime fechaInicio = objFecha.AnioAct.Fecha_Inicial;

            #region Titulo del Item
            ws.Cells[15, 1].Value = "1.3. PARTICIPACIÓN POR EMPRESAS EN LA PRODUCCIÓN TOTAL DE ENERGÍA DEL MES DE " + EPDate.f_NombreMes(fechaInicio.Month).ToUpper() + " DEL " + fechaInicio.Year;
            #endregion

            var titulo = "PARTICIPACIÓN POR EMPRESA EN LA PRODUCCIÓN TOTAL DE ENERGÍA DEL MES";
            var titulo2 = "PARTICIPACIÓN POR EMPRESA EN LA PRODUCCIÓN TOTAL DE ENERGÍA DEL MES MENORES AL 1%";

            int tamLista1 = listaEmpresas.Count;
            int tamLista2 = listaEmpresasOtros.Count;

            int coluNombre1;
            int filaInicio1;
            ExcelPieChart pieChart1;
            EscogerMejorGraficaPie(ws, tamLista1, 18, 0, out pieChart1, out coluNombre1, out filaInicio1, true);
            int coluPorcentaje1 = coluNombre1 + 1;
            int filaInicioOcultos1 = filaInicio1;

            int coluNombre2 = 68;
            int filaInicio2 = 10;
            int coluPorcentaje2 = coluNombre2 + 1;
            int filaInicioOcultos2 = filaInicio2;

            GenerarChartExcelGraficoProdempresasMenor1porc(ws, filaInicio2, coluNombre2, listaEmpresasOtros.Count, titulo2);

            foreach (var empresa in listaEmpresas)
            {
                var meditotalEmp = empresa.Total;
                ws.Cells[filaInicio1, coluNombre1].Value = empresa.Descripcion;
                ws.Cells[filaInicio1, coluNombre1 + 1].Value = empresa.Total;
                filaInicio1++;
            }
            foreach (var empresa in listaEmpresasOtros)
            {
                ws.Cells[filaInicio2, coluNombre2].Value = empresa.Emprnomb;
                ws.Cells[filaInicio2, coluNombre2 + 1].Value = empresa.Total;
                filaInicio2++;
            }

            var celdas1 = ws.Cells[filaInicioOcultos1, coluNombre1, filaInicio1 - 1, coluNombre1 + 1];
            var fontC1 = celdas1.Style.Font;
            fontC1.Color.SetColor(Color.White);

            pieChart1.Series[0].Series = ExcelRange.GetAddress(filaInicioOcultos1, coluNombre1 + 1, filaInicioOcultos1 + listaEmpresas.Count - 1, coluNombre1 + 1);
            pieChart1.Series[0].XSeries = ExcelRange.GetAddress(filaInicioOcultos1, coluNombre1, filaInicioOcultos1 + listaEmpresas.Count - 1, coluNombre1);
            pieChart1.Series[0].Header = titulo;

            #region Nota
            ws.Cells[42, 1].Value = "Gráfico N° 1: Participación de cada empresa generadora en la producción de energía eléctrica en el mes de " + ExtensionMethod.NombreMes(fechaInicio).ToLower() + " del " + fechaInicio.Year + " mayor al 1%";
            FormatoNota(ws, 42, 1);
            ws.Cells[115, 1].Value = "Gráfico N° 2: Participación de cada empresa generadora en la producción de energía eléctrica en el mes de " + ExtensionMethod.NombreMes(fechaInicio).ToLower() + " del " + fechaInicio.Year + " menor al 1%";
            FormatoNota(ws, 115, 1);

            #endregion

        }

        #endregion

        #region 1.4. Evolución del crecimiento mensual de la máxima potencia coincidente sin exportación a ecuador

        /// <summary>
        /// GenerarGwebEvolucionMaxPotCoincidenteSinEcuador
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static GraficoWeb GenerarGWebEvolucionMaxPotCoincidenteSinEcuador(FechasPR5 objFecha, List<ResultadoTotalGeneracion> listaVarSemanalData)
        {
            List<string> listaAnio = new List<string>();
            listaAnio.Add(objFecha.Anio2Ant.NumAnio.ToString());
            listaAnio.Add(objFecha.Anio1Ant.NumAnio.ToString());
            listaAnio.Add(objFecha.AnioAct.NumAnio.ToString());

            List<string> listaMesDesc = new List<string> { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

            GraficoWeb grafico = new GraficoWeb();
            grafico.TitleText = "Evolución del crecimiento mensual de la máxima potencia coincidente";
            grafico.Subtitle = "Gráfico N° 3: Evolución de la máxima potencia coincidente mensual en bornes de generación periodo " + objFecha.Anio2Ant.NumAnio + " - " + objFecha.AnioAct.NumAnio;

            grafico.XAxisCategories = listaMesDesc;
            grafico.YaxixTitle = string.Empty;
            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesData = new decimal?[3][];

            grafico.YAxisLabelsFormat = new List<string>() { "", "" };
            grafico.YAxixTitle = new List<string>() { "MW", "Variación" };

            //inicializar serie
            var serieData = new DatosSerie[listaAnio.Count + 2];
            for (int numSerie = 0; numSerie < 5; numSerie++)
            {
                string type = "";
                string name = "";
                string tooltip = "";
                int yaxis = 0;
                switch (numSerie)
                {
                    case 0:
                    case 1:
                    case 2:
                        type = "column";
                        name = listaAnio[numSerie];
                        tooltip = " MW";
                        break;
                    case 3:
                    case 4:
                        type = "spline";
                        name = string.Format("Inc. Anual {0}/{1}", (numSerie == 3 ? listaAnio[2] : listaAnio[1]), (numSerie == 3 ? listaAnio[1] : listaAnio[0]));
                        tooltip = " %";
                        yaxis = 1;
                        break;
                }

                serieData[numSerie] = new DatosSerie { Name = name, Data = new decimal?[12], TooltipValueSuffix = tooltip, Type = type, YAxis = yaxis };
            }

            //recorrer por mes
            DateTime fechaIniData = objFecha.Anio2Ant.Fecha_01Enero;
            DateTime fechaFinData = objFecha.AnioAct.RangoAct_FechaFin;
            int contadorSerie = 0;
            for (DateTime mes = fechaIniData; mes <= fechaFinData; mes = mes.AddMonths(1))
            {
                var obj = listaVarSemanalData.Find(x => x.Medifecha == mes && x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                serieData[contadorSerie].Data[mes.Month - 1] = obj != null ? obj.Meditotal : null;

                if (mes.Month == 12)
                    contadorSerie++;
            }
            contadorSerie = 3;
            for (DateTime mes = fechaIniData.AddYears(1); mes <= fechaFinData; mes = mes.AddMonths(1))
            {
                var obj = listaVarSemanalData.Find(x => x.Medifecha == mes && x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);
                serieData[contadorSerie].Data[mes.Month - 1] = obj != null ? obj.Meditotal : null;

                if (mes.Month == 12)
                    contadorSerie++;
            }

            grafico.SerieData = serieData;

            var valorMinimo = serieData.ToList().Where(x => x.Type == "column").Min(x => x.Data.Min());
            grafico.YaxixMin = (decimal)((valorMinimo.HasValue) ? (valorMinimo.Value < 5000 ? valorMinimo.Value - (valorMinimo.Value / 2) : 5000) : 0);

            return grafico;
        }

        public static List<string> ListaResumen(List<ResultadoTotalGeneracion> listaEvolucionMensual, FechasPR5 objFecha)
        {


            NumberFormatInfo nfi = UtilAnexoAPR5.GenerarNumberFormatInfo3();

            decimal maxAnio0 = listaEvolucionMensual.Where(x => x.Medifecha == objFecha.AnioAct.RangoAct_FechaIni && x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct).First().Meditotal.GetValueOrDefault(0);
            decimal maxAnio1 = listaEvolucionMensual.Where(x => x.Medifecha == objFecha.Anio1Ant.RangoAct_FechaIni && x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct).First().Meditotal.GetValueOrDefault(0);
            decimal varAnio0 = listaEvolucionMensual.Where(x => x.Medifecha == objFecha.AnioAct.RangoAct_FechaIni && x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var).First().Meditotal.GetValueOrDefault(0); ;

            List<string> listaResumen = new List<string>();
            listaResumen.Add(string.Format(" - Total Máxima potencia coincidente {0} {1}: {2} MW", EPDate.f_NombreMes(objFecha.AnioAct.NumMes).ToLower(), objFecha.AnioAct.NumAnio, maxAnio0.ToString("N", nfi)));
            listaResumen.Add(string.Format(" - Total Máxima potencia coincidente {0} {1}: {2} MW", EPDate.f_NombreMes(objFecha.Anio1Ant.NumMes).ToLower(), objFecha.Anio1Ant.NumAnio, maxAnio1.ToString("N", nfi)));
            listaResumen.Add(string.Format(" - Variación {0} {1} / {0} {2}: {3}%", EPDate.f_NombreMes(objFecha.AnioAct.NumMes).ToLower(), objFecha.AnioAct.NumAnio, objFecha.Anio1Ant.NumAnio, varAnio0.ToString("N", nfi)));

            return listaResumen;

        }

        /// <summary>
        /// Genera el excel de la evolucion del crecimiento mensual de maxima potencia
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="version"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static void GenerarHojaChartExcelEvolucionCrecimientoMensualMaxPotCoincidente(ExcelWorksheet ws, SiVersionDTO objVersion, FechasPR5 objFecha,
                                    List<string> listaResumen, GraficoWeb grafico, List<SiNotaDTO> listaNotas)
        {
            int filaInicio = 55;  //fila inicio de data
            int coluInicio = 28;  // columna inicio de data

            int filaAnio1 = filaInicio - 1;
            int coluAnio1 = coluInicio;

            var col = 0;
            var fil = 0;

            foreach (var serie in grafico.SerieData)
            {
                fil = 0;

                ws.Cells[filaAnio1, coluAnio1 + col].Value = serie.Name;
                fil++;
                foreach (var valor in serie.Data)
                {
                    if (valor != null)
                        ws.Cells[filaAnio1 + fil, coluAnio1 + col].Value = col > 2 ? valor / 100.0m : valor;
                    fil++;
                }

                col++;
            }

            #region Notas
            ws.Cells[138, 1].Value = grafico.Subtitle;
            FormatoNota(ws, 138, 1);

            var filaResumen = 120;
            foreach (var item in listaResumen)
            {
                ws.Cells[filaResumen, 1].Value = item;
                FormatoNota2(ws, filaResumen, 1);
                filaResumen++;
            }

            #endregion

            #region NotaBD
            int filaIniNotasBD = 140;
            int coluIniNotasBD = 1;

            int numNotas;
            ColocarNotasEnReporte(ws, filaIniNotasBD, coluIniNotasBD, listaNotas, out numNotas);

            #endregion

        }

        #endregion

        #region 1.5. Comparación de la cobertura de la máxima demanda por tipo de generación

        public static List<GenericoDTO> ListarFilaCuadro1_5Tgen()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroHidroelectrica, String1 = "HIDROELÉCTRICAS" },
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTermoelectrica, String1 = "TERMOELÉCTRICAS" },
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroRenovable, String1 = "GENERACIÓN RER" },
                                }.ToList();

            return listaCuadro;
        }

        /// <summary>
        /// Genera grafico web de la compracion de la cobertura de la maxima demanda por tipo de generacion
        /// </summary>
        /// <param name="diaMaximaDemanda"></param>
        /// <param name="diaMaximaDemandaAnhoAnt"></param>
        /// <param name="listaMaximaDemandaAll"></param>
        /// <param name="listDiasConsultadas"></param>
        /// <param name="listaTipogeneracion"></param>
        /// <returns></returns>
        public static GraficoWeb GenerarGWebComparacionMaxDemandaXTipoGeneracion(List<MaximaDemandaDTO> listaMDCoincidenteDataDesc,
                                List<ResultadoTotalGeneracion> listaC1)
        {
            //datos
            DateTime diaMaximaDemanda = listaMDCoincidenteDataDesc[0].FechaHora;
            DateTime diaMaximaDemandaAnhoAnt = listaMDCoincidenteDataDesc[1].FechaHora;
            var listaTipogeneracion = ListarFilaCuadro1_5Tgen();
            List<int> listaTipo = new List<int>() { PR5ConstanteFecha.ValorAnioAct_Sem1Ant, PR5ConstanteFecha.ValorAnioAct_SemAct };

            //grafico
            GraficoWeb graficoWeb = new GraficoWeb
            {
                TitleText = "Comparación de la cobertura de la demanda máxima por tipo de generación",
                XAxisCategories = new List<string>
                {
                    string.Format("{0} DIA: {1} HORA: {2}", diaMaximaDemandaAnhoAnt.Year, diaMaximaDemandaAnhoAnt.Date.ToString(ConstantesBase.FormatoFechaPE),diaMaximaDemandaAnhoAnt.ToString(ConstantesBase.FormatoHoraMinuto)),
                    string.Format("{0} DIA: {1} HORA: {2}", diaMaximaDemanda.Year, diaMaximaDemanda.Date.ToString(ConstantesBase.FormatoFechaPE),diaMaximaDemanda.ToString(ConstantesBase.FormatoHoraMinuto)),
                },
                YAxixTitle = new List<string> { "Megavatio (MW)" },
                YaxixLabelsFormat = "{value} MW",
                TooltipValueSuffix = " MW",
                SerieData = new DatosSerie[listaTipogeneracion.Count]
            };

            var indexS = 0;
            foreach (var tipoGeneracion in listaTipogeneracion)
            {
                graficoWeb.SerieData[indexS] = new DatosSerie { Name = tipoGeneracion.String1, Data = new decimal?[listaTipo.Count], TooltipValueSuffix = " MW" };
                var indexD = 0;
                foreach (var tipo in listaTipo)
                {
                    var objTgen = listaC1.Find(x => x.TipoSemanaRelProd == tipoGeneracion.Entero1 && x.TipoResultadoFecha == tipo);

                    graficoWeb.SerieData[indexS].Data[indexD] = objTgen.Meditotal;
                    indexD++;
                }

                indexS++;
            }
            return graficoWeb;
        }

        /// <summary>
        /// Genera el excel de la compracion de la cobertura de la maxima demanda por tipo de generacion
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="versionAnexo"></param>
        /// <returns></returns>
        public static void GenerarHojaChartExcelComparacionCoberturaMaxDemanda(ExcelWorksheet ws, SiVersionDTO objVersion, FechasPR5 objFecha,
                            List<MaximaDemandaDTO> listaMDCoincidenteDataDesc, GraficoWeb grafico, List<SiNotaDTO> listaNotas)
        {
            //datos
            DateTime diaMaximaDemanda = listaMDCoincidenteDataDesc[0].FechaHora;
            DateTime diaMaximaDemandaAnhoAnt = listaMDCoincidenteDataDesc[1].FechaHora;

            //grafico
            DateTime fechaInicio = objFecha.AnioAct.Fecha_Inicial;

            int filaInicio1 = 8; //fila inicial a donde se empezará colocar datos
            int coluInicio1 = 14; //columna inicial a donde se empezará colocar datos

            int filaHora = filaInicio1 - 3;
            int filaDia = filaInicio1 - 2;
            int filaAnio = filaInicio1 - 1;

            int coluHoraAnioAnt = coluInicio1;
            int coluHoraAnioAct = coluInicio1 + 1;

            ws.Cells[filaHora, coluHoraAnioAct].Value = diaMaximaDemanda.ToString(ConstantesBase.FormatoHoraMinuto);
            ws.Cells[filaDia, coluHoraAnioAct].Value = "DÍA: " + diaMaximaDemanda.Date.ToString(ConstantesBase.FormatoFechaPE);
            ws.Cells[filaAnio, coluHoraAnioAct].Value = "HORA: " + diaMaximaDemanda.Year;

            ws.Cells[filaHora, coluHoraAnioAnt].Value = diaMaximaDemandaAnhoAnt.ToString(ConstantesBase.FormatoHoraMinuto);
            ws.Cells[filaDia, coluHoraAnioAnt].Value = "DÍA: " + diaMaximaDemandaAnhoAnt.Date.ToString(ConstantesBase.FormatoFechaPE);
            ws.Cells[filaAnio, coluHoraAnioAnt].Value = "HORA: " + diaMaximaDemandaAnhoAnt.Year;

            var indexS = 0;
            foreach (var serie in grafico.SerieData)
            {
                ws.Cells[filaInicio1 + indexS, coluInicio1 - 1].Value = serie.Name; //Tipo de Generacion
                var indexD = 0;
                foreach (var valor in serie.Data)
                {
                    ws.Cells[filaInicio1 + indexS, coluInicio1 + indexD].Value = valor; //coloco datos

                    indexD++;
                }
                indexS++;
            }

            #region Nota
            //ws.Cells[4, 1].Value = "- Total Máxima potencia coincidente " + ExtensionMethod.NombreMes(fechaInicio).ToLower() + fechaInicio.Year + " :  YYY";
            //ws.Cells[5, 1].Value = "- Total Máxima potencia coincidente " + ExtensionMethod.NombreMes(fechaInicio.AddYears(-1)).ToLower() +" "+ fechaInicio.AddYears(-1).Year + " :  YYY";
            //ws.Cells[6, 1].Value = "- Variación " + ExtensionMethod.NombreMes(fechaInicio).ToLower() + fechaInicio.Year + " / " + ExtensionMethod.NombreMes(fechaInicio.AddYears(-1)).ToLower() +" "+ fechaInicio.AddYears(-1).Year + " : WWW";
            FormatoNota2(ws, 4, 1);
            FormatoNota2(ws, 5, 1);
            FormatoNota2(ws, 6, 1);

            ws.Cells[23, 1].Value = "Gráfico N°4: Comparación de la cobertura del tipo de generación en la maxima demanda coincidente en " + ExtensionMethod.NombreMes(fechaInicio).ToLower();
            FormatoNota(ws, 23, 1);

            //ws.Cells[27, 1].Value = "- Día de Máxima potencia coincidente: "  + " YYY1";
            //ws.Cells[28, 1].Value = "- Valor de Máxima potencia coincidente:" + " YYY2";
            FormatoNota2(ws, 27, 1);
            FormatoNota2(ws, 28, 1);

            ws.Cells[44, 1].Value = "Gráfico N°5: Cobertura del tipo de recurso energético en el diagrama de carga del día de maxima potencia coincidente " + ExtensionMethod.NombreMes(fechaInicio).ToLower() + " " + fechaInicio.Year;
            FormatoNota(ws, 44, 1);

            ws.Cells[66, 1].Value = "Gráfico N°6: Cobertura del tipo de tecnología en la maxima potencia coincidente " + ExtensionMethod.NombreMes(fechaInicio).ToLower() + " " + fechaInicio.Year;
            FormatoNota(ws, 66, 1);

            #endregion

            #region NotaBD
            int filaIniNotasBD = 68;
            int coluIniNotasBD = 1;

            int numNotas;
            ColocarNotasEnReporte(ws, filaIniNotasBD, coluIniNotasBD, listaNotas, out numNotas);
            #endregion
        }

        #endregion

        #region 1.6. Despacho en el día de máxima potencia coincidente

        /// <summary>
        /// Genera grafico web del despacho de maxima potencia coicidente mensual
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="lstDataYMaxPotSinOrden"></param>
        /// <returns></returns>
        public static GraficoWeb GenerarGWebDespachoMaxPotCoincidenteMensual(DateTime fechaInicio, List<MeMedicion96DTO> lstDataYMaxPotSinOrden)
        {
            var primerDiaDelMes = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);

            var lstData = lstDataYMaxPotSinOrden.Where(x => x.Fenergnomb != ConstantesSiosein2.NombreMaximaDemanda).ToList();
            var regMD = lstDataYMaxPotSinOrden.Find(x => x.Fenergnomb == ConstantesSiosein2.NombreMaximaDemanda) ?? new MeMedicion96DTO();

            //primero debe ser maxima demanda, el primer registro es columna en la plantilla excel
            var lstDataYMaxPot = new List<MeMedicion96DTO>() { regMD };
            lstDataYMaxPot.AddRange(lstData);

            var graficoWeb = new GraficoWeb
            {
                TitleText = "Despacho en el día de máxima potencia coincidente " + primerDiaDelMes.NombreMesAnho(),
                XAxisCategories = new List<string>(),
                YAxixTitle = new List<string> { "Megavatio (MW)" },
                YaxixLabelsFormat = "{value} MW",
                SerieData = new DatosSerie[lstDataYMaxPot.Count],
                TooltipValueSuffix = " MW"
            };

            var min = 0;
            var fecha = DateTime.MinValue;
            for (var hx = 1; hx <= 96; hx++)
            {
                min += 15;
                graficoWeb.XAxisCategories.Add(fecha.AddMinutes(min).ToString(ConstantesBase.FormatoHoraMinuto));
            }

            var indexSerie = 0;
            foreach (var medicion96 in lstDataYMaxPot)
            {
                if (medicion96.Fenergnomb == ConstantesSiosein2.NombreMaximaDemanda)
                {
                    graficoWeb.SerieData[indexSerie] = new DatosSerie() { Name = medicion96.Fenergnomb, Data = new decimal?[96], Type = "column", PointWidth = 2, BorderWidth = 0 };
                }
                else
                {
                    graficoWeb.SerieData[indexSerie] = new DatosSerie() { Name = medicion96.Fenergnomb, Data = new decimal?[96] };
                }

                for (var hx = 1; hx <= 96; hx++)
                {
                    var valHx = medicion96.GetType().GetProperty("H" + hx).GetValue(medicion96, null);
                    graficoWeb.SerieData[indexSerie].Data[hx - 1] = (decimal?)valHx;
                }

                indexSerie++;
            }

            return graficoWeb;
        }

        /// <summary>
        /// Genero excel del despacho de maxima potencia coicidente mensual
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        public static void GenerarHojaChartExcelDespachoDiaMaxDemandaCoincidente(ExcelWorksheet ws, GraficoWeb graficoWeb)
        {
            var serieMD = graficoWeb.SerieData.ToList().Find(x => x.Name == ConstantesSiosein2.NombreMaximaDemanda);
            var listaSerieNoMD = graficoWeb.SerieData.ToList().Where(x => x.Name != ConstantesSiosein2.NombreMaximaDemanda);
            List<DatosSerie> listaSerie = new List<DatosSerie>() { serieMD };
            listaSerie.AddRange(listaSerieNoMD);

            //
            int filaInicio = 27;
            int coluInicio = 16;

            int filaIniHora = filaInicio;
            int coluIniHora = coluInicio;

            int filaIniData = filaIniHora;
            int coluIniData = coluIniHora + 1;

            var indexSerie = 0;
            foreach (var medicion96 in listaSerie)
            {
                ws.Cells[filaIniData - 1, coluIniData + indexSerie].Value = medicion96.Name;
                for (var hx = 1; hx <= 96; hx++)
                {
                    var valHx = medicion96.Data[hx - 1];
                    if (valHx.GetValueOrDefault(0) != 0)
                        ws.Cells[filaIniData + hx - 1, coluIniData + indexSerie].Value = valHx;
                    else
                    {
                        //mostrar 0 para las series que tengan fuente de energia
                        if (indexSerie > 2)
                            ws.Cells[filaIniData + hx - 1, coluIniData + indexSerie].Value = 0;
                    }
                }

                indexSerie++;
            }
        }

        #endregion

        #region 1.7. Cobertura de la máxima potencia coincidente por tipo de tecnología

        /// <summary>
        /// Genera grafico web de Cobertura de la máxima potencia coincidente por tipo de tecnología
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="diaMaximaDemanda"></param>
        /// <returns></returns>
        public static GraficoWeb GenerarGWebMaxPotCoincidenteMensualxTecnologia(DateTime fechaInicio, List<EqCategoriaDetDTO> listaCategoria)
        {
            var primerDiaDelMes = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);

            //********************  Grafico**************************************
            var graficoWeb = new GraficoWeb
            {
                TitleText = "Cobertura de la máxima potencia coincidente por tipo de tecnología en " + primerDiaDelMes.NombreMesAnho(),
                Type = "pie",
                SeriesInnerSize = "50%"
            };
            var serieData = new DatosSerie[listaCategoria.Count];

            var row = 0;
            foreach (var ctg in listaCategoria)
            {
                serieData[row] = new DatosSerie()
                {
                    Name = ctg.Ctgdetnomb,
                    Y = ctg.Total,
                    Z = ctg.Porcentaje
                };
                row++;

            }
            graficoWeb.SerieData = serieData.OrderByDescending(x => x.Y).ToArray();

            return graficoWeb;

        }

        /// <summary>
        /// Genera excel de Cobertura de la máxima potencia coincidente por tipo de tecnología
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="versionAnexo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static void GenerarHojaChartExcelMaxPotCoincidenteMensualxTecnologia(ExcelWorksheet ws, SiVersionDTO objVersion, FechasPR5 objFecha,
                    GraficoWeb grafico)
        {
            int filaInicio = 132;
            int coluInicio = 16;

            int filaTecnol = filaInicio;
            int coluTecnol = coluInicio;

            var rango = ws.Cells[filaInicio + 1, coluInicio, filaInicio + 16, coluInicio + 2];

            var row = 0;
            foreach (var serie in grafico.SerieData)
            {

                ws.Cells[filaTecnol + 1 + row, coluTecnol].Value = serie.Name;
                ws.Cells[filaTecnol + 1 + row, coluTecnol + 1].Value = serie.Y;
                ws.Cells[filaTecnol + 1 + row, coluTecnol + 2].Value = serie.Z;

                row++;

            }

        }

        #endregion

        #region 1.8. Utilización de los recursos energéticos

        public static List<SiFuenteenergiaDTO> ListarFilaCuadro1_8CuadroNoRer()
        {
            List<SiFuenteenergiaDTO> listaFenerg = new List<SiFuenteenergiaDTO>();
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiAgua, Fenergnomb = "Hidráulica", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesSiosein2.FenergcodiGasCamisea, Fenergnomb = "Gas Natural de Camisea", Ctgdetcodi = ConstantesPR5ReportesServicio.SubCategoriaRecursoGasNatural });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesSiosein2.FenergcodiGasMalacas, Fenergnomb = "Gas Natural de Malacas", Ctgdetcodi = ConstantesPR5ReportesServicio.SubCategoriaRecursoGasMalacas });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesSiosein2.FenergcodiGasAguaytia, Fenergnomb = "Gas Natural de Aguaytía", Ctgdetcodi = ConstantesPR5ReportesServicio.SubCategoriaRecursoGasAguaytia });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesSiosein2.FenergcodiGasLaIsla, Fenergnomb = "Gas Natural de La Isla", Ctgdetcodi = ConstantesPR5ReportesServicio.SubCategoriaRecursoGasLaIsla, ValidarDatoObligatorio = true });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiDiesel, Fenergnomb = "Diesel 2", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiCarbon, Fenergnomb = "Carbón", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiResidual, Fenergnomb = "Residual", ValidarDatoObligatorio = true });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiR500, Fenergnomb = "Residual 500", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiR6, Fenergnomb = "Residual 6", ValidarDatoObligatorio = true });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiNoAplica, Fenergnomb = "No aplica", ValidarDatoObligatorio = true });

            return listaFenerg;
        }

        public static List<GenericoDTO> ListarFilaCuadro1_8NoRer()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalGeneracionNoRER, String1 = "Total Generación sin RER" },
                                }.ToList();

            return listaCuadro;
        }

        public static List<SiFuenteenergiaDTO> ListarFilaCuadro1_8CuadroSiRer()
        {
            List<SiFuenteenergiaDTO> listaFenerg = new List<SiFuenteenergiaDTO>();

            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesSiosein2.FenergcodiAguaRER, Fenergnomb = "Hidráulica", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiEolica, Fenergnomb = "Aerogenerador", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiSolar, Fenergnomb = "Solar", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiBiogas, Fenergnomb = "Biogás", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiBagazo, Fenergnomb = "Biomasa - Bagazo", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesSiosein2.FenergcodiNoAplicaRER, Fenergnomb = "No aplica", ValidarDatoObligatorio = true });

            return listaFenerg;
        }

        public static List<GenericoDTO> ListarFilaCuadro1_8SiRer()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalGeneracionSiRER, String1 = "Total Generación RER" },
                                }.ToList();

            return listaCuadro;
        }

        public static List<GenericoDTO> ListarFilaCuadro1_8TIE()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroImportacion, String1 = "Importación desde Ecuador" },
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroExportacion, String1 = "Exportación hacia Ecuador " },
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalTIE, String1 = "Intercambios Internacionales" },
                                }.ToList();

            return listaCuadro;
        }

        public static List<GenericoDTO> ListarFilaCuadro1_8Sein()
        {
            var listaCuadro = new List<GenericoDTO> {
                                    new GenericoDTO(){ Entero1 = ConstantesSiosein2.FilaCuadroTotalSein, String1 = "TOTALES CONSIDERANDO INTERCAMBIOS INTERNACIONALES" },
                                }.ToList();

            return listaCuadro;
        }

        public static TablaReporte ObtenerDataTablaUtilizacionRREE(FechasPR5 objFecha,
                List<MaximaDemandaDTO> listaMDCoincidenteDataDesc, List<SiFuenteenergiaDTO> listaFenergCuadro,
                List<ResultadoTotalGeneracion> listaFenergGen, List<ResultadoTotalGeneracion> listaFenergMD,
                List<ResultadoTotalGeneracion> listaFenergGenTotal, List<ResultadoTotalGeneracion> listaFenergMDTotal,
                List<ResultadoTotalGeneracion> listaTotalyTIEGen, List<ResultadoTotalGeneracion> listaTotalyTIEMD,
                List<ResultadoTotalGeneracion> listaTIEC3Total, List<ResultadoTotalGeneracion> listaTIEC3MD)
        {
            var regMDAct = listaMDCoincidenteDataDesc.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
            var regMDAnt = listaMDCoincidenteDataDesc.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);

            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = objFecha.TipoReporte;
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[4, 10];

            matrizCabecera[0, 0] = "Tipo de Generación";
            matrizCabecera[0, 1] = "Máxima potencia coincidente (MW)";
            matrizCabecera[0, 4] = "Energía producida mensual (GWh)";
            matrizCabecera[0, 7] = "Acumulado anual (GWh)";

            matrizCabecera[1, 1] = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(objFecha.AnioAct.NumMes), objFecha.AnioAct.NumAnio);
            matrizCabecera[1, 2] = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(objFecha.Anio1Ant.NumMes), objFecha.Anio1Ant.NumAnio);
            matrizCabecera[1, 3] = string.Format("Variación {0}{1} / {0}{2}", EPDate.f_NombreMesCorto(objFecha.AnioAct.NumMes), objFecha.AnioAct.NumAnio, objFecha.Anio1Ant.NumAnio);
            matrizCabecera[1, 4] = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(objFecha.AnioAct.NumMes), objFecha.AnioAct.NumAnio);
            matrizCabecera[1, 5] = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(objFecha.Anio1Ant.NumMes), objFecha.Anio1Ant.NumAnio);
            matrizCabecera[1, 6] = string.Format("Variación {0}{1} / {0}{2}", EPDate.f_NombreMesCorto(objFecha.AnioAct.NumMes), objFecha.AnioAct.NumAnio, objFecha.Anio1Ant.NumAnio);
            matrizCabecera[1, 7] = objFecha.AnioAct.NumAnio.ToString();
            matrizCabecera[1, 8] = objFecha.Anio1Ant.NumAnio.ToString();
            matrizCabecera[1, 9] = string.Format("Variación {0} / {1}", objFecha.AnioAct.NumAnio, objFecha.Anio1Ant.NumAnio);

            matrizCabecera[2, 1] = regMDAct.FechaOnlyDia;
            matrizCabecera[2, 2] = regMDAnt.FechaOnlyDia;

            matrizCabecera[3, 1] = regMDAct.FechaOnlyHora;
            matrizCabecera[3, 2] = regMDAnt.FechaOnlyHora;

            cabRepo.CabeceraData = matrizCabecera;
            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            //No rer
            var contador = 0;
            foreach (var regFila in ListarFilaCuadro1_8CuadroNoRer())
            {
                var listaMDTmp = listaFenergMD.Where(x => x.Fenergcodi == regFila.Fenergcodi).ToList();
                var listaGenTmp = listaFenergGen.Where(x => x.Fenergcodi == regFila.Fenergcodi).ToList();

                if (listaFenergCuadro.Any(x => x.Fenergcodi == regFila.Fenergcodi))
                {
                    RegistroReporte registro = new RegistroReporte();
                    List<decimal?> datos = new List<decimal?>();

                    ResultadoTotalGeneracion regMDAnio0G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                    ResultadoTotalGeneracion regMDAnio1G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                    ResultadoTotalGeneracion regMDVarAnio0G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                    ResultadoTotalGeneracion regValor0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                    ResultadoTotalGeneracion regValor1G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                    ResultadoTotalGeneracion regValorVarAnio0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                    ResultadoTotalGeneracion regValorAcum0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                    ResultadoTotalGeneracion regValorAcum1G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_Acum);
                    ResultadoTotalGeneracion regValorVarAcumAnio0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum_Var);

                    datos.Add(regMDAnio0G.Meditotal);
                    datos.Add(regMDAnio1G.Meditotal);
                    datos.Add(regMDVarAnio0G.Meditotal);

                    datos.Add(regValor0G.Meditotal);
                    datos.Add(regValor1G.Meditotal);
                    datos.Add(regValorVarAnio0G.Meditotal);

                    datos.Add(regValorAcum0G.Meditotal);
                    datos.Add(regValorAcum1G.Meditotal);
                    datos.Add(regValorVarAcumAnio0G.Meditotal);

                    registro.Nombre = regFila.Fenergnomb;
                    registro.ListaData = datos;
                    registro.ColorFila = "#FFFFFF";

                    registros.Add(registro);
                }
            }

            //fila total No rer
            foreach (var regFila in ListarFilaCuadro1_8NoRer())
            {
                var listaMDTmp = listaFenergMDTotal.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();
                var listaGenTmp = listaFenergGenTotal.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();

                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                ResultadoTotalGeneracion regMDAnio0G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDVarAnio0G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regValor0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regValor1G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regValorVarAnio0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regValorAcum0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                ResultadoTotalGeneracion regValorAcum1G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_Acum);
                ResultadoTotalGeneracion regValorVarAcumAnio0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum_Var);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                datos.Add(regValor0G.Meditotal);
                datos.Add(regValor1G.Meditotal);
                datos.Add(regValorVarAnio0G.Meditotal);

                datos.Add(regValorAcum0G.Meditotal);
                datos.Add(regValorAcum1G.Meditotal);
                datos.Add(regValorVarAcumAnio0G.Meditotal);

                registro.Nombre = regFila.String1;
                registro.ListaData = datos;
                registro.EsFilaResumen = true;
                registro.ColorFila = "#B4C6E7";

                registros.Add(registro);
            }

            //Si rer
            foreach (var regFila in ListarFilaCuadro1_8CuadroSiRer())
            {
                var listaMDTmp = listaFenergMD.Where(x => x.Fenergcodi == regFila.Fenergcodi).ToList();
                var listaGenTmp = listaFenergGen.Where(x => x.Fenergcodi == regFila.Fenergcodi).ToList();

                if (listaFenergCuadro.Any(x => x.Fenergcodi == regFila.Fenergcodi))
                {
                    RegistroReporte registro = new RegistroReporte();
                    List<decimal?> datos = new List<decimal?>();

                    ResultadoTotalGeneracion regMDAnio0G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                    ResultadoTotalGeneracion regMDAnio1G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                    ResultadoTotalGeneracion regMDVarAnio0G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                    ResultadoTotalGeneracion regValor0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                    ResultadoTotalGeneracion regValor1G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                    ResultadoTotalGeneracion regValorVarAnio0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                    ResultadoTotalGeneracion regValorAcum0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                    ResultadoTotalGeneracion regValorAcum1G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_Acum);
                    ResultadoTotalGeneracion regValorVarAcumAnio0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum_Var);

                    datos.Add(regMDAnio0G.Meditotal);
                    datos.Add(regMDAnio1G.Meditotal);
                    datos.Add(regMDVarAnio0G.Meditotal);

                    datos.Add(regValor0G.Meditotal);
                    datos.Add(regValor1G.Meditotal);
                    datos.Add(regValorVarAnio0G.Meditotal);

                    datos.Add(regValorAcum0G.Meditotal);
                    datos.Add(regValorAcum1G.Meditotal);
                    datos.Add(regValorVarAcumAnio0G.Meditotal);

                    registro.Nombre = regFila.Fenergnomb;
                    registro.ListaData = datos;
                    registro.ColorFila = "#FFFFFF";

                    registros.Add(registro);
                }
            }

            //fila total Si rer
            foreach (var regFila in ListarFilaCuadro1_8SiRer())
            {
                var listaMDTmp = listaFenergMDTotal.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();
                var listaGenTmp = listaFenergGenTotal.Where(x => x.TipoSemanaRelProd == regFila.Entero1.Value).ToList();

                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                ResultadoTotalGeneracion regMDAnio0G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDVarAnio0G = listaMDTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regValor0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regValor1G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regValorVarAnio0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regValorAcum0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                ResultadoTotalGeneracion regValorAcum1G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_Acum);
                ResultadoTotalGeneracion regValorVarAcumAnio0G = listaGenTmp.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum_Var);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                datos.Add(regValor0G.Meditotal);
                datos.Add(regValor1G.Meditotal);
                datos.Add(regValorVarAnio0G.Meditotal);

                datos.Add(regValorAcum0G.Meditotal);
                datos.Add(regValorAcum1G.Meditotal);
                datos.Add(regValorVarAcumAnio0G.Meditotal);

                registro.Nombre = regFila.String1;
                registro.ListaData = datos;
                registro.EsFilaResumen = true;
                registro.ColorFila = "#B4C6E7";

                registros.Add(registro);
            }

            //Agregar 3 filas de Interconexion
            contador = 0;
            foreach (var regFila in ListarFilaCuadro1_8TIE())
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaDataC3 = listaTIEC3Total.Where(x => x.TipoSemanaRelProd == regFila.Entero1).ToList();
                var listaDataMD = listaTIEC3MD.Where(x => x.TipoSemanaRelProd == regFila.Entero1).ToList();

                ResultadoTotalGeneracion regMDAnio0G = listaDataMD.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaDataMD.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDVarAnio0G = listaDataMD.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regValor0G = listaDataC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regValor1G = listaDataC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regValorVarAnio0G = listaDataC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regValorAcum0G = listaDataC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                ResultadoTotalGeneracion regValorAcum1G = listaDataC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_Acum);
                ResultadoTotalGeneracion regValorVarAcumAnio0G = listaDataC3.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum_Var);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                datos.Add(regValor0G.Meditotal);
                datos.Add(regValor1G.Meditotal);
                datos.Add(regValorVarAnio0G.Meditotal);

                datos.Add(regValorAcum0G.Meditotal);
                datos.Add(regValorAcum1G.Meditotal);
                datos.Add(regValorVarAcumAnio0G.Meditotal);

                registro.Nombre = regFila.String1;
                registro.ListaData = datos;

                registro.ColorFila = contador == 2 ? "#DDEBF7" : "#FFFFFF";
                contador++;

                registros.Add(registro);
            }

            //fila total sein
            foreach (var regFila in ListarFilaCuadro1_8Sein())
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                ResultadoTotalGeneracion regMDAnio0G = listaTotalyTIEMD.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regMDAnio1G = listaTotalyTIEMD.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regMDVarAnio0G = listaTotalyTIEMD.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regValor0G = listaTotalyTIEGen.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct);
                ResultadoTotalGeneracion regValor1G = listaTotalyTIEGen.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_SemAct);
                ResultadoTotalGeneracion regValorVarAnio0G = listaTotalyTIEGen.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_SemAct_Var);

                ResultadoTotalGeneracion regValorAcum0G = listaTotalyTIEGen.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum);
                ResultadoTotalGeneracion regValorAcum1G = listaTotalyTIEGen.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnio1Ant_Acum);
                ResultadoTotalGeneracion regValorVarAcumAnio0G = listaTotalyTIEGen.Find(x => x.TipoResultadoFecha == PR5ConstanteFecha.ValorAnioAct_Acum_Var);

                datos.Add(regMDAnio0G.Meditotal);
                datos.Add(regMDAnio1G.Meditotal);
                datos.Add(regMDVarAnio0G.Meditotal);

                datos.Add(regValor0G.Meditotal);
                datos.Add(regValor1G.Meditotal);
                datos.Add(regValorVarAnio0G.Meditotal);

                datos.Add(regValorAcum0G.Meditotal);
                datos.Add(regValorAcum1G.Meditotal);
                datos.Add(regValorVarAcumAnio0G.Meditotal);

                registro.Nombre = regFila.String1;
                registro.ListaData = datos;
                registro.EsFilaResumen = true;
                registro.ColorFila = "#B4C6E7";

                registros.Add(registro);
            }

            #endregion

            tabla.ListaRegistros = registros;

            return tabla;
        }

        /// <summary>
        /// Genera grafico web con el listado de la Utilizacion de los Recursos Energeticos
        /// </summary>
        /// <returns></returns>
        public static string GenerarHtmlUtilizacionRecursosEnergeticoHtml(TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo3();
            var tamTabla = 1300;

            strHtml.Append("<div id='listado_reporte' style='height: auto; width: " + tamTabla + "px;'>");

            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: " + (tamTabla - 30) + "px;'>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th rowspan='4' style='width: 285px;'>{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th colspan='3'>{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th colspan='3'>{0}</th>", dataCab[0, 4]);
            strHtml.AppendFormat("<th colspan='3'>{0}</th>", dataCab[0, 7]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 120px;'>{0}</th>", dataCab[1, 1]);
            strHtml.AppendFormat("<th style='width: 120px;'>{0}</th>", dataCab[1, 2]);
            strHtml.AppendFormat("<th rowspan='3' style='width: 120px;'>{0}</th>", dataCab[1, 3]);
            strHtml.AppendFormat("<th rowspan='3' style='width: 120px;'>{0}</th>", dataCab[1, 4]);
            strHtml.AppendFormat("<th rowspan='3' style='width: 120px;'>{0}</th>", dataCab[1, 5]);
            strHtml.AppendFormat("<th rowspan='3' style='width: 120px;'>{0}</th>", dataCab[1, 6]);
            strHtml.AppendFormat("<th rowspan='3' style='width: 120px;'>{0}</th>", dataCab[1, 7]);
            strHtml.AppendFormat("<th rowspan='3' style='width: 120px;'>{0}</th>", dataCab[1, 8]);
            strHtml.AppendFormat("<th rowspan='3' style='width: 120px;'>{0}</th>", dataCab[1, 9]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 1]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[2, 2]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 1]);
            strHtml.AppendFormat("<th style=''>{0}</th>", dataCab[3, 2]);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            int f = 0;
            foreach (var reg in registros)
            {
                string styleTotal = "background-color: " + reg.ColorFila + ";";
                styleTotal += reg.EsFilaResumen ? "font-weight: bold;padding-top: 5px; padding-bottom: 5px;" : "";

                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;{1}'>{0}</td>", reg.Nombre, styleTotal);

                int c = 1;
                foreach (decimal? col in reg.ListaData)
                {

                    if (c == 3 || c == 6 || c == 9) //con signo  de %
                        strHtml.AppendFormat("<td class='alignValorRight' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirVariacionHtml(col.HasValue ? col : null, nfi2), styleTotal);
                    else
                        strHtml.AppendFormat("<td class='alignValorRight' style='{1}'>{0}</td>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi2), styleTotal);

                    c++;
                }
                strHtml.Append("</tr>");
                f++;
            }
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el excel  con el listado para el reporte de la Utilizacion de los Recursos Energeticos
        /// </summary>
        /// <param name="ws"></param>  
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="ultimaFila"></param>
        /// <returns></returns>
        public static void GenerarChartExcelListadoGeneracionRecursoEnergeticos(ExcelWorksheet ws, TablaReporte tablaData, out int ultimaFila)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            //INICIO DE LA TABLA 
            int filaIniEmpresa = 4;
            int coluIniEmpresa = 1;
            int filaIniData = filaIniEmpresa + 4;
            int coluIniData = coluIniEmpresa;

            #region cabecera
            ws.Cells[filaIniEmpresa, coluIniEmpresa + 0].Value = dataCab[0, 0];
            ws.Cells[filaIniEmpresa, coluIniEmpresa + 1].Value = dataCab[0, 1];
            ws.Cells[filaIniEmpresa, coluIniEmpresa + 4].Value = dataCab[0, 4];
            ws.Cells[filaIniEmpresa, coluIniEmpresa + 7].Value = dataCab[0, 7];

            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 1].Value = dataCab[1, 1];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 2].Value = dataCab[1, 2];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 3].Value = dataCab[1, 3];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 4].Value = dataCab[1, 4];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 5].Value = dataCab[1, 5];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 6].Value = dataCab[1, 6];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 7].Value = dataCab[1, 7];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 8].Value = dataCab[1, 8];
            ws.Cells[filaIniEmpresa + 1, coluIniEmpresa + 9].Value = dataCab[1, 9];

            ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 1].Value = dataCab[2, 1];
            ws.Cells[filaIniEmpresa + 2, coluIniEmpresa + 2].Value = dataCab[2, 2];

            ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 1].Value = dataCab[3, 1];
            ws.Cells[filaIniEmpresa + 3, coluIniEmpresa + 2].Value = dataCab[3, 2];

            #endregion

            int ultimaColu = coluIniEmpresa + 9;

            #region cuerpo

            ultimaFila = filaIniData + registros.Count() - 1;

            #region Formato Cuerpo
            //UtilExcel.CeldasExcelWrapText(ws, filaIniData, coluIniData, ultimaFila, coluIniData);
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData + 1, ultimaFila, ultimaColu, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData, ultimaFila, coluIniData, "Izquierda");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData + 1, ultimaFila, ultimaColu, "Derecha");
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniData, coluIniData + 0, ultimaFila, coluIniData + 0);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, ConstantesPR5ReportesServicio.TipoLetraCuerpo, ConstantesPR5ReportesServicio.TamLetraCuerpo3);

            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, 1, ultimaFila, 1, "#5B9BD5");
            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, 2, ultimaFila, 4, "#5B9BD5");
            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, 5, ultimaFila, 7, "#5B9BD5");
            UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData, 8, ultimaFila, 10, "#5B9BD5");

            #endregion

            int filaX = 0;
            foreach (var reg in registros)
            {
                int colX = 0;

                ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre;
                colX++;

                if (reg.EsFilaResumen)
                {
                    UtilExcel.CeldasExcelEnNegrita(ws, filaIniData + filaX, coluIniData, filaIniData + filaX, ultimaColu);
                    UtilExcel.BorderCeldasLineaDelgada(ws, filaIniData + filaX, coluIniData, filaIniData + filaX, ultimaColu, "#5B9BD5");
                }

                foreach (decimal? numValor in reg.ListaData)
                {
                    string strFormat = ConstantesPR5ReportesServicio.FormatoNumero3Digito;

                    if (numValor != null)
                    {
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Value = numValor;
                    }
                    ws.Cells[filaIniData + filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                    colX++;
                }

                if (!string.IsNullOrEmpty(reg.ColorFila))
                    UtilExcel.CeldasExcelColorFondo(ws, filaIniData + filaX, coluIniData, filaIniData + filaX, ultimaColu, reg.ColorFila);

                filaX++;
            }
            #endregion

            #region Nota

            ws.Cells[ultimaFila + 1, 1].Value = "Cuadro N°3: Detalle de recurso energético ";
            FormatoNota(ws, ultimaFila + 1, 1);
            DarTamanioFilas(ws, ultimaFila + 3, ultimaFila + 11, 8);

            #endregion

            ultimaFila = ultimaFila + 11;
        }

        /// <summary>
        /// Genera el primer grafico en excel  (Máxima Potencia Coincidente por Tipo de Recurso Energético) para el reporte de la Utilizacion de los Recursos Energeticos
        /// </summary>
        /// <param name="ws"></param>  
        /// <param name="fechaInicio"></param>
        /// <param name="horaMaxDelDiaAct"></param>
        /// <param name="listaMaximaDemanda"></param>
        /// <param name="ultimaFila"></param>
        /// <returns></returns>
        public static void GenerarChartExcelMaxPotXTiporecursoEnergeticoMesActual(ExcelWorksheet ws, FechasPR5 objFecha, GraficoWeb graficoPie, GraficoWeb graficoBarra, int ultimaFila)
        {
            var graficoArea = ws.Drawings["grafMaxPotXTipo"] as ExcelPieChart;
            graficoArea.SetPosition(ultimaFila + 2, 0, 0, 0);
            graficoArea.Title.Text = graficoPie.TitleText;

            int filaIniDatos = 36;
            int coluIniDatos = 18;
            int fila = 0;

            List<DatosSerie> listaSerie = new List<DatosSerie>();
            listaSerie.AddRange(graficoPie.SerieData.ToList().GetRange(0, 2));
            listaSerie.AddRange(graficoBarra.SerieData.ToList());

            foreach (var fenerg in listaSerie)
            {
                ws.Cells[filaIniDatos + fila, coluIniDatos].Value = fenerg.Name;
                //ws.Cells[filaIniDatos + fila, coluIniDatos + 1].Value = (decimal)meditotalEmp;
                ws.Cells[filaIniDatos + fila, coluIniDatos + 2].Value = fenerg.Y;
                fila++;
            }

            #region Nota
            ws.Cells[ultimaFila + 24, 1].Value = "Gráfico N°7: Participación de los recursos energéticos en la máxima potencia coincidente de " + objFecha.AnioAct.RangoAct_NumYAnio;
            FormatoNota(ws, ultimaFila + 24, 1);
            #endregion

        }

        /// <summary>
        /// Genera el primer grafico en excel  (Generación de Energía Eléctrica por Tipo de Recurso Energético) para el reporte de la Utilizacion de los Recursos Energeticos
        /// </summary>
        /// <param name="ws"></param>  
        /// <param name="fechaInicio"></param>
        /// <param name="listaProduccionEnergiaMensual"></param>
        /// <param name="ultimaFila"></param>
        /// <returns></returns>
        public static void GenerarChartExcelGeneracionenergiaXTiporecursoEnergeticoMesActual(ExcelWorksheet ws, FechasPR5 objFecha, GraficoWeb graficoPie, GraficoWeb graficoBarra,
                    List<SiNotaDTO> listaNotas, int ultimaFila)
        {
            var graficoArea = ws.Drawings["grafGenEner"] as ExcelPieChart;
            graficoArea.SetPosition(ultimaFila + 3 + 24, 0, 0, 0);
            graficoArea.Title.Text = graficoPie.TitleText;

            int filaIniDatos = 36;
            int coluIniDatos = 22;
            int fila = 0;

            List<DatosSerie> listaSerie = new List<DatosSerie>();
            listaSerie.AddRange(graficoPie.SerieData.ToList().GetRange(0, 2));
            listaSerie.AddRange(graficoBarra.SerieData.ToList());

            foreach (var fenerg in listaSerie)
            {
                ws.Cells[filaIniDatos + fila, coluIniDatos].Value = fenerg.Name;
                //ws.Cells[filaIniDatos + fila, coluIniDatos + 1].Value = (decimal)meditotalEmp;
                ws.Cells[filaIniDatos + fila, coluIniDatos + 2].Value = fenerg.Y;
                fila++;
            }

            #region Nota
            ws.Cells[ultimaFila + 3 + 24 + 26, 1].Value = "Gráfico N°8: Participación de los recursos energéticos en la producción de energía eléctrica de " + objFecha.AnioAct.RangoAct_NumYAnio;
            FormatoNota(ws, ultimaFila + 3 + 24 + 26, 1);
            #endregion

            #region NotaBD
            int filaIniNotasBD = ultimaFila + 3 + 24 + 26 + 2;
            int coluIniNotasBD = 1;
            int numNotas;
            ColocarNotasEnReporte(ws, filaIniNotasBD, coluIniNotasBD, listaNotas, out numNotas);
            #endregion

        }

        public static GraficoWeb GetGraficoPieTiporecursoEnergetico(string titulo, List<ResultadoTotalGeneracion> listaFenerg)
        {
            var graficoWeb = new GraficoWeb
            {
                TitleText = titulo,
                Type = "pie",
                SerieData = new DatosSerie[listaFenerg.Count]
            };

            var serieData = new DatosSerie[listaFenerg.Count];

            var row = 0;
            foreach (var fenerg in listaFenerg)
            {
                serieData[row] = new DatosSerie()
                {
                    Name = fenerg.Fenergnomb,
                    Y = fenerg.Meditotal
                };
                row++;
            }

            var seriesDataG = serieData.OrderByDescending(x => x.Y).ToList();

            graficoWeb.SerieData = seriesDataG.ToArray();

            return graficoWeb;
        }

        public static GraficoWeb GetGraficoBarraTiporecursoEnergetico(string titulo, List<ResultadoTotalGeneracion> listaFenerg)
        {
            var graficoWebC = new GraficoWeb
            {
                TitleText = titulo,
                XAxisCategories = new List<string>() { "OTROS" },
                YAxixTitle = new List<string> { "Porcentaje otros (%)" },
                YaxixLabelsFormat = "{value} %",
                SerieData = new DatosSerie[listaFenerg.Count],
                TooltipValueSuffix = " %"
            };

            var indexSerie = 0;
            foreach (var fenerg in listaFenerg)
            {
                decimal? valorGrafico = fenerg.Meditotal.GetValueOrDefault(0) * 100.0m;
                graficoWebC.SerieData[indexSerie] = new DatosSerie() { Name = fenerg.Fenergnomb, Y = fenerg.Meditotal, Data = new[] { valorGrafico } };
                indexSerie++;
            }

            return graficoWebC;
        }

        #endregion

        #region 1.9. Participación de la utilización de los recursos energéticos en la producción de energía eléctrica

        public static List<SiFuenteenergiaDTO> ListarFilaCuadro1_9Fenerg()
        {
            List<SiFuenteenergiaDTO> listaFenerg = new List<SiFuenteenergiaDTO>();
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiAgua, Fenergnomb = "Hidro", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiGas, Fenergnomb = "Gas" });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiCarbon, Fenergnomb = "Carbón", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiDiesel, Fenergnomb = "Diesel", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiResidual, Fenergnomb = "Residual", ValidarDatoObligatorio = true });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiR500, Fenergnomb = "Residual 500", });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiR6, Fenergnomb = "Residual 6", ValidarDatoObligatorio = true });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiNoAplica, Fenergnomb = "No aplica", ValidarDatoObligatorio = true });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesSiosein2.FenergcodiRERConvencional, Fenergnomb = "Convencional", EsRer = true });
            listaFenerg.Add(new SiFuenteenergiaDTO() { Fenergcodi = ConstantesSiosein2.FenergcodiRERNoConvencional, Fenergnomb = "No Convencional", EsRer = true });

            return listaFenerg;
        }

        public static TablaReporte ObtenerDataTablaParticipacionFenergia(FechasPR5 objFecha, List<DateTime> listaMes,
                    List<ResultadoTotalGeneracion> listaPorcMensual, List<SiFuenteenergiaDTO> listaFenerg)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = objFecha.TipoReporte;
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[listaMes.Count, listaFenerg.Count + 1];

            matrizCabecera[0, 0] = "MES";
            int contador = 1;
            foreach (var item in listaFenerg.Where(x => !x.EsRer))
            {
                matrizCabecera[0, contador] = item.Fenergnomb;
                contador++;
            }
            matrizCabecera[0, contador] = "Renovable";

            matrizCabecera[1, contador] = "Convencional";
            matrizCabecera[1, contador + 1] = "No Convencional";

            cabRepo.CabeceraData = matrizCabecera;
            tabla.Cabecera = cabRepo;

            #region cuerpo
            List<RegistroReporte> registros = new List<RegistroReporte>();

            //Por tipo de Generación
            contador = 0;
            foreach (var regFila in listaMes)
            {
                RegistroReporte registro = new RegistroReporte();
                List<decimal?> datos = new List<decimal?>();

                var listaXmes = listaPorcMensual.Where(x => x.Medifecha == regFila).ToList();

                foreach (var item in listaFenerg)
                {
                    var objPor = listaXmes.Find(x => x.Fenergcodi == item.Fenergcodi);
                    decimal por = objPor != null ? objPor.Meditotal.GetValueOrDefault(0) : 0;
                    datos.Add(por);
                }

                registro.Nombre = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(regFila.Month), regFila.Year);
                registro.ListaData = datos;
                registro.ColorFila = contador % 2 == 0 ? "#DDEBF7" : "#FFFFFF";
                registro.EsFilaVisible = regFila <= objFecha.AnioAct.RangoAct_FechaIni;

                contador++;

                registros.Add(registro);
            }

            #endregion

            tabla.ListaRegistros = registros;

            return tabla;
        }

        /// <summary>
        /// reto participacion de la utilizacion de los recursos energeticos en la produccion de energia electrica
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static string GenerarRHtmlUtilizacionRecursosEnergeticoEnProduccion(TablaReporte tablaData)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros.Where(x => x.EsFilaVisible).ToList();

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi3 = UtilAnexoAPR5.GenerarNumberFormatInfo3();
            var tamTabla = 800;

            strHtml.Append("<div id='listado_reporte' style='height: auto; width: " + tamTabla + "px;'>");

            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: " + (tamTabla - 30) + "px;'>");

            #region cabecera

            int numCol = dataCab.GetLength(1);

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            for (var i = 0; i < numCol - 2; i++)
                strHtml.AppendFormat("<th rowspan='2' style='width: 100px;'>{0}</th>", dataCab[0, i]);
            strHtml.AppendFormat("<th colspan='2' >{0}</th>", dataCab[0, numCol - 2]);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 100px;'>{0}</th>", dataCab[1, numCol - 2]);
            strHtml.AppendFormat("<th style='width: 100px;'>{0}</th>", dataCab[1, numCol - 1]);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            int f = 0;
            foreach (var reg in registros)
            {
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;background: {1};'>{0}</td>", reg.Nombre, reg.ColorFila);

                int c = 0;
                foreach (decimal? col in reg.ListaData)
                {
                    strHtml.AppendFormat("<td class='alignValorRight' style='background: {1};' >{0}</td>", UtilAnexoAPR5.ImprimirVariacionHtml(col.HasValue ? col * 100.0m : null, nfi3), reg.ColorFila);

                    c++;
                }
                strHtml.Append("</tr>");
                f++;
            }
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        public static GraficoWeb GenerarGWebUtilizacionRecursosEnergeticoEnProduccion(FechasPR5 objFecha, List<SiFuenteenergiaDTO> listaFenerg, List<DateTime> listaMes,
                                                List<ResultadoTotalGeneracion> listaPorcMensual)
        {
            DateTime fechaIniData = objFecha.Anio2Ant.Fecha_01Enero;
            DateTime fechaFinData = objFecha.AnioAct.RangoAct_FechaFin;

            var graficoWeb = new GraficoWeb
            {
                TitleText = string.Format("Evolución de la utilización de los recursos energéticos en la producción de energía eléctrica periodo {0}-{1}", fechaIniData.Year, fechaFinData.Year),
                XAxisCategories = new List<string>(),
                YAxixTitle = new List<string> { "Valor Porcentual" },
                YaxixLabelsFormat = "{value} %",
                TooltipValueSuffix = " GWh",
                SerieData = new DatosSerie[listaFenerg.Count]
            };

            var indexS = 0;
            foreach (var recurso in listaFenerg)
            {
                graficoWeb.SerieData[indexS] = new DatosSerie { Name = recurso.Fenergnomb, Data = new decimal?[listaMes.Count], TooltipValueSuffix = " %" };

                var listaXFenerg = listaPorcMensual.Where(x => x.Fenergcodi == recurso.Fenergcodi).ToList();

                var indexD = 0;
                foreach (var fecha in listaMes.OrderBy(x => x.Month).ThenBy(x => x.Year).ToList())
                {
                    var objPor = listaXFenerg.Find(x => x.Medifecha == fecha);
                    decimal por = objPor != null ? objPor.Meditotal.GetValueOrDefault(0) : 0;

                    graficoWeb.SerieData[indexS].Data[indexD] = por;
                    indexD++;
                }

                indexS++;
            }

            var agrupacionFecha = listaMes.GroupBy(x => x.Month);
            graficoWeb.Categorias = new Categorias[agrupacionFecha.Count()];

            var indexC = 0;
            foreach (var fechaG in agrupacionFecha)
            {
                graficoWeb.Categorias[indexC] = new Categorias() { Name = EPDate.f_NombreMes(fechaG.Key), Categories = new string[fechaG.Count()] };
                var indexX = 0;
                foreach (var fecha in fechaG)
                {
                    graficoWeb.Categorias[indexC].Categories[indexX] = " " + fecha.Year.ToString();
                    indexX++;
                }
                indexC++;
            }

            return graficoWeb;
        }

        /// <summary>
        /// Genera el listado (tabla) excel del reporte Utilización de Recursos Energéticos en Producción
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fechaInicioConsulta"></param>
        /// <param name="fechaFinConsulta"></param>
        /// <param name="listaProduccionEnergiaConsulta"></param>
        /// <returns></returns>
        public static void GenerarChartExcelListadoUtilizacionRecursosEnergeticoEnProduccion(ExcelWorksheet ws, TablaReporte tablaData, out int ultimaFila)
        {
            int filaInicioAll = 6;
            int coluIniAll = 2;

            int rowIniCabecera = filaInicioAll;
            int colIniCabecera = coluIniAll;

            int filaIniData = filaInicioAll + 2;
            int coluIniData = coluIniAll;

            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros.Where(x => x.EsFilaVisible).ToList();

            #region Cabecera tabla

            // cabeceras
            int numCol = dataCab.GetLength(1);
            for (var i = 0; i < numCol - 2; i++)
            {
                ws.Cells[rowIniCabecera, colIniCabecera + i].Value = dataCab[0, i];
                UtilExcel.CeldasExcelAgrupar(ws, rowIniCabecera, colIniCabecera + i, rowIniCabecera + 1, colIniCabecera + i);
            }

            //formato de las celdas de la ultima columna de la cabecera
            UtilExcel.CeldasExcelColorTexto(ws, rowIniCabecera, colIniCabecera + numCol - 1, rowIniCabecera + 1, colIniCabecera + numCol - 1, "#000000");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCabecera, colIniCabecera + numCol - 1, rowIniCabecera + 1, colIniCabecera + numCol - 1, "#D9E1F2");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniCabecera, colIniCabecera + numCol - 1, rowIniCabecera + 1, colIniCabecera + numCol - 1, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniCabecera, colIniCabecera + numCol - 1, rowIniCabecera + 1, colIniCabecera + numCol - 1, "Centro");
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniCabecera, colIniCabecera + numCol - 1, rowIniCabecera + 1, colIniCabecera + numCol - 1, "#2F75B5", true, true);

            ws.Cells[rowIniCabecera, colIniCabecera + numCol - 2].Value = dataCab[0, numCol - 2];
            UtilExcel.CeldasExcelAgrupar(ws, rowIniCabecera, colIniCabecera + numCol - 2, rowIniCabecera, colIniCabecera + numCol - 1);
            ws.Cells[rowIniCabecera + 1, colIniCabecera + numCol - 2].Value = dataCab[1, numCol - 2];
            ws.Cells[rowIniCabecera + 1, colIniCabecera + numCol - 1].Value = dataCab[1, numCol - 1];

            #endregion

            #region cuerpo

            int ultFila = filaIniData + registros.Count - 1;
            int ultimaColu = coluIniAll + numCol - 1;

            int filaX = 0;
            foreach (var reg in registros)
            {
                int colX = 0;

                ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Nombre;
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData + filaX, coluIniData + colX, filaIniData + filaX, coluIniData + colX, "Centro");
                colX++;

                foreach (decimal? numValor in reg.ListaData)
                {
                    string strFormat = ConstantesPR5ReportesServicio.FormatoNumero3DigitoPorcentaje;

                    if (numValor != null)
                    {
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Value = numValor;
                    }
                    ws.Cells[filaIniData + filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                    colX++;
                }

                if (!string.IsNullOrEmpty(reg.ColorFila))
                    UtilExcel.CeldasExcelColorFondo(ws, filaIniData + filaX, coluIniData, filaIniData + filaX, ultimaColu, reg.ColorFila);

                filaX++;
            }

            ultimaFila = ultFila;
            //tamanio de letra para toda la tabla
            var cuerpoTabla = ws.Cells[rowIniCabecera, colIniCabecera, ultFila, ultimaColu];
            cuerpoTabla.Style.Font.SetFromFont(new Font("Arial", 6));
            cuerpoTabla.Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorTranslator.FromHtml(ConstantesSiosein2.ColorBordeTablaRepEje));

            #endregion

            #region Nota
            ws.Cells[ultimaFila + 1, coluIniAll].Value = "Cuadro N°4: Participación de cada tipo de recurso energético en la producción mensual de energía eléctrica.";
            FormatoNota2(ws, ultimaFila + 1, coluIniAll);
            #endregion

        }

        /// <summary>
        /// Genera la comparación en el excel del reporte Utilización de Recursos Energéticos en Producción
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="fechaInicioConsulta"></param>
        /// <param name="fechaFinConsulta"></param>
        /// <param name="listaProduccionEnergiaConsulta"></param>
        /// <returns></returns>
        public static void GenerarChartExcelComparativaUtilizacionRecursosEnergeticoEnProduccion(ExcelWorksheet ws, SiVersionDTO objVersion, FechasPR5 objFecha,
                                                    GraficoWeb grafico, List<SiNotaDTO> listaNotas, int ultimaFila)
        {
            var miChart = ws.Drawings["grafUtilizacionRer"] as ExcelChart;
            miChart.SetPosition(ultimaFila + 3, 0, 0, 0);

            int filaInicioAll = 5;
            int coluIniAll = 32;

            int rowIniCabecera = filaInicioAll;
            int colIniCabecera = coluIniAll + 1;

            int filaIniData = filaInicioAll + 1;
            int coluIniData = coluIniAll;

            #region Cabecera tabla

            // cabeceras
            for (var i = 0; i < grafico.SerieData.Length; i++)
                ws.Cells[rowIniCabecera, colIniCabecera + i].Value = grafico.SerieData[i].Name;

            #endregion

            #region cuerpo

            int filaX = 0;
            int contador = 1;
            foreach (var reg in grafico.Categorias)
            {
                for (int k = 0; k < reg.Categories.Length; k++)
                {
                    int colX = 0;

                    ws.Cells[filaIniData + filaX, coluIniData + colX].Value = reg.Name + reg.Categories[k];
                    colX++;

                    for (var i = 0; i < grafico.SerieData.Length; i++)
                    {
                        string strFormat = ConstantesPR5ReportesServicio.FormatoNumero2DigitoPorcentaje;
                        decimal? numValor = grafico.SerieData[i].Data[contador - 1];
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Value = numValor;
                        ws.Cells[filaIniData + filaX, coluIniData + colX].Style.Numberformat.Format = strFormat;

                        colX++;
                    }

                    if (contador % 3 == 0) filaX++;
                    contador++;

                    filaX++;
                }
            }

            #endregion

            #region Nota
            ws.Cells[ultimaFila + 26, 1].Value = "Gráfico N°9:  Evolución de la utilización de los recursos energéticos en la producción de energía eléctrica periodo " + objFecha.Anio2Ant.NumAnio + "-" + objFecha.AnioAct.NumAnio + ".";
            FormatoNota(ws, ultimaFila + 26, 1);
            #endregion

            #region NotaBD
            int filaIniNotasBD = ultimaFila + 26 + 2;
            int coluIniNotasBD = 1;
            int numNotas;
            ColocarNotasEnReporte(ws, filaIniNotasBD, coluIniNotasBD, listaNotas, out numNotas);
            #endregion

        }
        #endregion

        #endregion

        #region 5. HORAS CONGESTION EN LOS PRINCIPALES EQUIPOS DE TRANSMISIÓN

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TablaReporte ObtenerDataTablaHCxArea(List<DateTime> listaMeses, FechasPR5 objFecha, List<EqGrupoLineaDTO> listaGrupolinea,
                            List<ResultadoTotalGeneracion> listaDataXArea, List<ResultadoTotalGeneracion> listaCongestionTotalXFecha)
        {
            #region Cabecera

            var anioActual = objFecha.AnioAct.NumAnio;

            TablaReporte tabla = new TablaReporte();
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[16, listaGrupolinea.Count + 1];

            matrizCabecera[0, 0] = "MES - AÑO";
            for (int i = 0; i < listaGrupolinea.Count; i++)
            {
                matrizCabecera[0, 1 + i] = listaGrupolinea[i].Areanomb;
                matrizCabecera[1, 1 + i] = listaGrupolinea[i].Equipo;
            }

            cabRepo.CabeceraData = matrizCabecera;
            tabla.Cabecera = cabRepo;

            #endregion

            #region cuerpo

            List<RegistroReporte> registros = new List<RegistroReporte>();


            foreach (var mes in listaMeses)
            {
                var listaXAreageoXMes = listaDataXArea.Where(x => x.Medifecha == mes).ToList();

                RegistroReporte registro = new RegistroReporte();

                List<string> propiedades = new List<string>();
                List<decimal?> datos = new List<decimal?>();
                propiedades.Add(EPDate.f_NombreMes(mes.Month) + " " + mes.Year);

                foreach (var item in listaGrupolinea)
                {
                    ResultadoTotalGeneracion regAnio0 = listaXAreageoXMes.Find(x => x.Codigo == item.Grulincodi);

                    datos.Add(regAnio0?.Meditotal);
                }

                registro.ListaData = datos;
                registro.ListaPropiedades = propiedades;

                registros.Add(registro);
            }

            for (var i = 0; i < 1; i++)
            {
                RegistroReporte registro = new RegistroReporte();

                List<string> propiedades = new List<string>();
                List<decimal?> datos = new List<decimal?>();
                propiedades.Add("TOTAL (HORAS)");

                foreach (var item in listaGrupolinea)
                {
                    ResultadoTotalGeneracion regAnio0 = listaCongestionTotalXFecha.Find(x => x.Codigo == item.Grulincodi);

                    datos.Add(regAnio0?.Meditotal);
                }

                registro.ListaData = datos;
                registro.ListaPropiedades = propiedades;
                registro.EsFilaResumen = true;

                registros.Add(registro);
            }

            #endregion

            tabla.ListaRegistros = registros;

            return tabla;
        }

        public static GraficoWeb GenerarGWebHorasCongestionEquiposTransmision(FechasPR5 objFecha, List<DateTime> listaMeses,
                            List<EqGrupoLineaDTO> listaGrupolinea, List<ResultadoTotalGeneracion> listaCongestionXFecha)
        {
            var listaMesesString = new List<string>();
            foreach (var item in listaMeses)
            {
                listaMesesString.Add(EPDate.f_NombreMes(item.Month) + " " + item.Year);
            }

            var graficoWeb = new GraficoWeb
            {
                TitleText = "HORAS DE CONGESTIÓN EQUIPOS DE TRANSMISIÓN " + objFecha.Anio1Ant.RangoAct_NumYAnio + "-" + objFecha.AnioAct.RangoAct_NumYAnio,
                XAxisCategories = listaMesesString,
                YAxixTitle = new List<string> { "HORAS" },
                YaxixLabelsFormat = "{value} h",
                TooltipValueSuffix = " h",
                PlotOptionsDataLabels = false,
            };

            graficoWeb.SerieData = new DatosSerie[listaGrupolinea.Count()];

            var row = 0;
            foreach (var agrupConsgestion in listaGrupolinea)
            {
                var linea = agrupConsgestion.Areanomb + " " + agrupConsgestion.Equipo;
                graficoWeb.SerieData[row] = new DatosSerie { Name = linea, Data = new decimal?[listaMeses.Count] };
                var rowd = 0;
                foreach (var fecha in listaMeses)
                {
                    var objData = listaCongestionXFecha.Find(x => x.Codigo == agrupConsgestion.Grulincodi && x.Medifecha == fecha);

                    graficoWeb.SerieData[row].Data[rowd] = objData?.Meditotal;
                    rowd++;
                }

                row++;
            }

            return graficoWeb;
        }

        public static string ListaHorasCongestionEquiposTransmisioHTML(TablaReporte tablaData)

        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;
            int numCol = dataCab.GetLength(1);

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi3 = UtilAnexoAPR5.GenerarNumberFormatInfo2();
            var tamTabla = 800;

            strHtml.Append("<div id='listado_reporte' style='height: auto; width: " + tamTabla + "px;'>");

            strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: " + (tamTabla - 30) + "px;'>");

            #region cabecera

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th rowspan='2' style='width: 100px;'>{0}</th>", dataCab[0, 0]);
            for (var i = 1; i < numCol; i++)
                strHtml.AppendFormat("<th style='width: 100px;'>{0}</th>", dataCab[0, i]);
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            for (var i = 1; i < numCol; i++)
                strHtml.AppendFormat("<th style='width: 100px;'>{0}</th>", dataCab[1, i]);
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            int f = 0;
            foreach (var reg in registros)
            {
                if (!reg.EsFilaResumen)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<td style='padding-left: 5px;text-align: left;font-weight:bold;'>{0}</td>", reg.ListaPropiedades[0]);

                    int c = 0;
                    foreach (decimal? valor in reg.ListaData)
                    {
                        strHtml.AppendFormat("<td class='alignValorRight' >{0}</td>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(valor, nfi3));

                        c++;
                    }
                }
                strHtml.Append("</tr>");
                f++;
            }
            strHtml.Append("</tbody>");

            strHtml.Append("<thead>");
            foreach (var reg in registros)
            {
                if (reg.EsFilaResumen)
                {
                    strHtml.Append("<tr>");
                    strHtml.AppendFormat("<th style='padding-left: 5px;text-align: left;font-weight:bold;'>{0}</th>", reg.ListaPropiedades[0]);

                    int c = 0;
                    foreach (decimal? col in reg.ListaData)
                    {
                        strHtml.AppendFormat("<th class='alignValorRight' >{0}</th>", UtilAnexoAPR5.ImprimirValorTotalOcultar0Html(col.HasValue ? col : null, nfi3));

                        c++;
                    }

                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</thead>");
            strHtml.Append("</table>");

            #endregion

            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el listado (tabla) en el excel del reporte Congestion  Equipo Transmision
        /// <param name="ws"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="lista48"></param>
        /// <param name="cabecera"></param>
        /// <param name="xNumSeries"></param>
        /// <param name="xFilaIniMes"></param>
        /// <param name="xColuIniMes"></param>
        /// <param name="xUltimaFila"></param>
        /// <returns></returns>
        public static void GenerarChartExcelListadoCongestionEqTransmision(ExcelWorksheet ws, TablaReporte tablaData, FechasPR5 objFecha, out int xNumSeries, out int xFilaIniMes, out int xColuIniMes, out int xUltimaFila)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;
            int numCol = dataCab.GetLength(1);

            ws.Cells[5, 1].Value = "5.  HORAS DE CONGESTIÓN EN LOS PRINCIPALES EQUIPOS DE TRANSMISIÓN " + objFecha.Anio1Ant.RangoAct_NumYAnio + " - " + objFecha.AnioAct.RangoAct_NumYAnio;

            int filaIniMes = 7;
            int coluIniMes = 1;

            int ultimaFila = 0;
            int ultimaColu = 0;

            #region cabecera

            ws.Cells[filaIniMes, coluIniMes].Value = dataCab[0, 0];
            int coluX1 = 0;
            for (var i = 1; i < numCol; i++)
            {
                ws.Cells[filaIniMes, coluIniMes + 1 + coluX1].Value = dataCab[0, i];
                coluX1++;
            }
            int col = 0;
            for (var i = 1; i < numCol; i++)
            {
                ws.Cells[filaIniMes + 1, coluIniMes + 1 + col].Value = dataCab[1, i];
                col++;
            }
            ultimaColu = coluIniMes + 1 + coluX1 - 1;

            #region Formato Cabecera
            ws.Column(coluIniMes).Width = 18;
            for (int i = coluIniMes + 1; i <= ultimaColu; i++)
            {
                ws.Column(i).Width = 11;
            }
            ws.Row(filaIniMes).Height = 40;
            ws.Row(filaIniMes + 1).Height = 30;
            UtilEjecMensual.CeldasExcelAgrupar(ws, filaIniMes, coluIniMes, filaIniMes + 1, coluIniMes);
            UtilEjecMensual.CeldasExcelTipoYTamanioLetra(ws, filaIniMes, coluIniMes, filaIniMes + 1, coluIniMes, "Arial", 7);
            UtilEjecMensual.CeldasExcelTipoYTamanioLetra(ws, filaIniMes, coluIniMes + 1, filaIniMes + 1, ultimaColu, "Arial", 5);

            UtilEjecMensual.CeldasExcelAlinearHorizontalmente(ws, filaIniMes, coluIniMes, filaIniMes + 1, ultimaColu, "Centro");
            UtilEjecMensual.CeldasExcelAlinearVerticalmente(ws, filaIniMes, coluIniMes, filaIniMes + 1, ultimaColu, "Centro");
            UtilEjecMensual.CeldasExcelWrapText(ws, filaIniMes, coluIniMes, filaIniMes + 1, ultimaColu);
            UtilEjecMensual.CeldasExcelEnNegrita(ws, filaIniMes, coluIniMes, filaIniMes + 1, ultimaColu);
            UtilEjecMensual.borderCeldas(ws, filaIniMes, coluIniMes, filaIniMes + 1, ultimaColu);
            #endregion

            #endregion

            #region Cuerpo         

            int filaX1 = 0;
            foreach (var reg in registros)
            {
                ws.Cells[filaIniMes + 2 + filaX1, coluIniMes].Value = reg.ListaPropiedades[0];
                int coluX2 = 0;
                foreach (decimal? valor in reg.ListaData)
                {
                    ws.Cells[filaIniMes + 2 + filaX1, coluIniMes + 1 + coluX2].Value = valor;
                    ws.Cells[filaIniMes + 2 + filaX1, coluIniMes + 1 + coluX2].Style.Numberformat.Format = "#,##0.00";
                    coluX2++;
                }
                filaX1++;
            }
            ultimaFila = filaIniMes + 15;

            #region Formato Cuerpo
            UtilEjecMensual.CeldasExcelTipoYTamanioLetra(ws, filaIniMes + 2, coluIniMes, ultimaFila - 1, ultimaColu, "Arial", 6);
            UtilEjecMensual.CeldasExcelAlinearHorizontalmente(ws, filaIniMes + 2, coluIniMes + 1, ultimaFila - 1, ultimaColu, "Centro");
            UtilEjecMensual.CeldasExcelEnNegrita(ws, filaIniMes + 2, coluIniMes, ultimaFila - 1, coluIniMes);
            UtilEjecMensual.borderCeldas3(ws, filaIniMes + 2, coluIniMes, ultimaFila - 1, ultimaColu);
            #endregion

            #endregion

            #region Pie

            #region Formato Pie
            UtilEjecMensual.CeldasExcelTipoYTamanioLetra(ws, ultimaFila, coluIniMes, ultimaFila, ultimaColu, "Arial", 6);
            UtilEjecMensual.CeldasExcelAlinearHorizontalmente(ws, ultimaFila, coluIniMes + 1, ultimaFila, ultimaColu, "Centro");
            UtilEjecMensual.CeldasExcelEnNegrita(ws, ultimaFila, coluIniMes, ultimaFila, ultimaColu);
            UtilEjecMensual.borderCeldas(ws, ultimaFila, coluIniMes, ultimaFila, ultimaColu);
            #endregion

            #endregion

            xNumSeries = numCol - 1;
            xFilaIniMes = filaIniMes;
            xColuIniMes = coluIniMes;
            xUltimaFila = ultimaFila;
        }

        /// <summary>
        /// Genera el grafico en el excel del reporte Congestion  Equipo Transmision
        /// <param name="ws"></param>
        /// <param name="numSeries"></param>
        /// <param name="filaIniMes"></param>
        /// <param name="coluIniMes"></param>
        /// <param name="ultimaFila"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static void GenerarChartExcelGraficoCongestionEqTransmision(ExcelWorksheet ws, SiVersionDTO objVersion, FechasPR5 objFecha, List<SiNotaDTO> listaNotas,
                            int numSeries, int filaIniMes, int coluIniMes, int ultimaFila)
        {
            #region Grafico Barras
            var miChart = ws.Drawings["grafHorasCongestion"] as ExcelChart;
            miChart.Title.Text = "HORAS DE CONGESTIÓN EQUIPOS DE TRANSMISIÓN " + objFecha.Anio1Ant.RangoAct_NumYAnio + " - " + objFecha.AnioAct.RangoAct_NumYAnio;

            miChart.SetPosition(ultimaFila + 5, 0, 1, 0);
            if (numSeries == 0)
            {
                miChart.SetSize(0, 0);
            }

            UtilEjecMensual.EliminarAllSeriesDelGrafico(miChart);

            var ran1 = ws.Cells[5, 5, 6, 6]; //aleatorio
            var ran2 = ws.Cells[5, 5, 6, 6]; //aleatorio
            for (int i = 0; i < numSeries; i++) //creamos series
            {
                miChart.Series.Add(ran1, ran2);
            }

            for (int pto = 0; pto < numSeries; pto++)
            {
                miChart.Series[pto].Series = ExcelRange.GetAddress(filaIniMes + 2, coluIniMes + 1 + pto, ultimaFila - 1, coluIniMes + 1 + pto);
                miChart.Series[pto].XSeries = ExcelRange.GetAddress(filaIniMes + 2, coluIniMes, ultimaFila - 1, coluIniMes);
                miChart.Series[pto].Header = (string)ws.Cells[filaIniMes, coluIniMes + 1 + pto].Value;
            }

            #endregion

            #region NotaBD
            int filaIniNotasBD = 60;
            int coluIniNotasBD = 1;
            int numNotas;
            UtilEjecMensual.ColocarNotasEnReporte(ws, filaIniNotasBD, coluIniNotasBD, listaNotas, out numNotas);
            #endregion

        }

        #endregion

        #region 6. EVOLUCIÓN DE LOS COSTOS MARGINALES

        #region 6.1. EVOLUCIÓN DEL COSTO MARGINAL EN BARRA DE REFERENCIA

        public static string ListaHorasCongestionEquiposTransmisioHTML(List<PrCongestionDTO> listaCongestion, DateTime fechaInicio, DateTime fechaFin)

        {
            StringBuilder strHtml = new StringBuilder();

            var listaAgrupConsgestion = listaCongestion.GroupBy(x => new { x.Areacodi, x.Grulincodi });

            var digit = 2;
            strHtml.Append("<table class='pretty tabla-icono' id='cmcpdet'>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th rowspan='2'>MES - AÑO</th>");
            foreach (var item in listaAgrupConsgestion)
            {
                strHtml.AppendFormat("<th>{0}</th>", item.First().Grulinnombre);
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            foreach (var item in listaAgrupConsgestion)
            {
                strHtml.AppendFormat("<th>{0}</th>", string.Join(" ", item.Select(x => x.Equinomb).Distinct()));
            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            #endregion

            #region Cuerpo

            var listaMeses = new List<DateTime>();
            UtilEjecMensual.BuclePorMeses(fechaInicio, fechaFin, date => { listaMeses.Add(date); });
            List<MeMedicion48DTO> listaMedicion = new List<MeMedicion48DTO>();
            strHtml.Append("<tbody>");
            foreach (var fecha in listaMeses)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td class='text'>" + fecha.NombreMesAnho().ToUpper() + "</td>");
                foreach (var AgrupConsgestion in listaAgrupConsgestion)
                {
                    var lstCongestionMenlXGrupolin = AgrupConsgestion.Where(x => x.Congesfecinicio >= fecha && x.Congesfecfin < fecha.AddMonths(1));

                    var listaHorasTraslape = EPDate.GetPeriodosCombinadosXInterceccion(lstCongestionMenlXGrupolin.Select(x => new Periodo() { FechaInicio = x.Congesfecinicio.Value, FechaFin = x.Congesfecfin.Value }));

                    double horasCongestion = listaHorasTraslape.Sum(x => x.Duracion.TotalHours);
                    strHtml.AppendFormat("<td class='number'>{0}</td>", ((decimal)horasCongestion).FormatoDecimal(digit));
                }
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");

            #endregion

            #region Pie

            strHtml.Append("<tfoot>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='width: 100px;'>TOTAL (HORAS)</td>");
            foreach (var AgrupConsgestion in listaAgrupConsgestion)
            {
                double horasCongestion = AgrupConsgestion.Sum(x => x.Congesfecfin.Value.Subtract(x.Congesfecinicio.Value).TotalHours);
                strHtml.AppendFormat("<td class='number'>{0}</td>", ((decimal)horasCongestion).FormatoDecimal(digit));
            }
            strHtml.Append("</tr>");
            strHtml.Append("</tfoot>");
            #endregion

            return strHtml.ToString();
        }

        #endregion

        #endregion

        #region Metodos

        public static decimal? ConvertirMwaGw(decimal? valor, ConstantesSiosein2.TipoMedicion medicion)
        {
            if (valor == null) return null;

            decimal resultado;
            switch (medicion)
            {
                case ConstantesSiosein2.TipoMedicion.Medicion24:
                    resultado = (decimal)valor / 1000;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion48:
                    resultado = (decimal)valor / 2000;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion96:
                    resultado = (decimal)valor / 4000;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Tipo de medicion no considerado", medicion, null);
            }

            return resultado;
        }

        public static decimal? ConvertirMwhaMw(decimal? valor, ConstantesSiosein2.TipoMedicion medicion)
        {
            if (valor == null) return null;

            decimal resultado;
            switch (medicion)
            {
                case ConstantesSiosein2.TipoMedicion.Medicion24:
                    resultado = (decimal)valor;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion48:
                    resultado = (decimal)valor * 2;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion96:
                    resultado = (decimal)valor * 4;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("medicion", medicion, null);
            }

            return resultado;
        }

        public static decimal? ConvertirMwaMwh(decimal? valor, ConstantesSiosein2.TipoMedicion medicion)
        {
            if (valor == null) return null;
            if (valor == 0M) return 0M;

            decimal resultado;
            switch (medicion)
            {
                case ConstantesSiosein2.TipoMedicion.Medicion24:
                    resultado = (decimal)valor;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion48:
                    resultado = (decimal)valor / 2;
                    break;
                case ConstantesSiosein2.TipoMedicion.Medicion96:
                    resultado = (decimal)valor / 4;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("medicion", medicion, null);
            }

            return resultado;
        }

        /// <summary>
        /// Coloca el Height a ciertas filas del excel
        /// </summary>
        /// <param name="ws"></param>  
        /// <param name="filaInicio"></param>
        /// <param name="filaFin"></param>
        /// <param name="tamanio"></param>
        /// <returns></returns>
        public static void DarTamanioFilas(ExcelWorksheet ws, int filaInicio, int filaFin, int tamanio)
        {
            for (int fila = filaInicio; fila <= filaFin; fila++)
            {
                ws.Row(fila).Height = tamanio;
            }
        }

        /// <summary>
        /// Calcula la variación porcentual de dos valores
        /// </summary>
        /// <param name="primerValor">Primer valor</param>
        /// <param name="segundoValor">Segundo valor</param>
        /// <returns></returns>
        public static decimal? VariacionPorcentual(decimal? primerValor, decimal? segundoValor)
        {
            if (!primerValor.HasValue || !segundoValor.HasValue) return null;
            return VariacionPorcentual(primerValor.Value, segundoValor.Value);
        }

        /// <summary>
        /// Calcula la variación porcentual de dos valores
        /// </summary>
        /// <param name="primerValor">Primer valor</param>
        /// <param name="segundoValor">Segundo valor</param>
        /// <returns></returns>
        public static decimal VariacionPorcentual(decimal primerValor, decimal segundoValor)
        {
            if (segundoValor == 0) return 0;
            return ((primerValor - segundoValor) / segundoValor) * 100M;
        }

        /// <summary>
        /// Calcula el desvio porcentual de dos valores
        /// </summary>
        /// <param name="primerValor"></param>
        /// <param name="segundoValor"></param>
        /// <returns></returns>
        public static decimal? DesvioPorcentualCOES(decimal primerValor, decimal segundoValor)
        {
            if (primerValor == 0) return null;

            return ((primerValor - segundoValor) / primerValor) * 100;
        }

        /// <summary>
        /// Calcula el desvio porcentual de dos valores
        /// </summary>
        /// <param name="primerValor"></param>
        /// <param name="segundoValor"></param>
        /// <returns></returns>
        public static decimal? DesvioPorcentualCOES(decimal? primerValor, decimal? segundoValor)
        {
            if (!primerValor.HasValue || !segundoValor.HasValue) return null;

            return DesvioPorcentualCOES(primerValor.Value, segundoValor.Value);

        }

        public static decimal? ConvertirMWhaGWh(decimal? valor)
        {
            return (valor.HasValue) ? valor / 1000 : valor;
        }

        public static decimal? ConvertirKWaMW(decimal? valor)
        {
            return (valor.HasValue) ? valor / 1000 : valor;
        }

        public static decimal? ConvertirGWhaMWh(decimal? valor)
        {
            return (valor.HasValue) ? valor * 1000 : valor;
        }

        /// <summary>
        /// Verifica la interscción de fechas
        /// </summary>
        /// <param name="primeFechaInicio">Primera Fecha de inicio</param>
        /// <param name="primeraFechaFin">Primera Fecha fin</param>
        /// <param name="segundaFechaInicio">Segunda Fecha de inicio</param>
        /// <param name="segundaFechaFin">Segunda Fecha fin</param>
        /// <returns></returns>
        public static bool VericarInterseccionDeFechas(DateTime primeFechaInicio, DateTime primeraFechaFin, DateTime segundaFechaInicio, DateTime segundaFechaFin)
        {
            bool overlap = primeFechaInicio < segundaFechaFin && segundaFechaInicio < primeraFechaFin;
            return overlap;
        }

        public static List<TransferenciaEntregaDetalleDTO> ObtenerTranferenciaDetPorEmpresa(List<TransferenciaEntregaDetalleDTO> listaEntregaDet)
        {

            var listaTransferenciaEntregaDetalles = new List<TransferenciaEntregaDetalleDTO>();
            var listaAgrupEntregaDet = listaEntregaDet.GroupBy(x => x.EmprCodi);
            foreach (var itemAgrup in listaAgrupEntregaDet)
            {
                var tranfEntregaDet = new TransferenciaEntregaDetalleDTO();
                tranfEntregaDet.EmprCodi = itemAgrup.Key;
                tranfEntregaDet.NombEmpresa = itemAgrup.First().NombEmpresa;
                tranfEntregaDet.TranEntrDetaSumaDia = itemAgrup.Sum(x => x.TranEntrDetaSumaDia);

                for (int i = 1; i <= 96; i++)
                {
                    var valorTENTDEx = itemAgrup.Sum(x => (decimal?)x.GetType().GetProperty(ConstantesAppServicio.CaracterTENTDE + i).GetValue(x, null));
                    tranfEntregaDet.GetType().GetProperty(ConstantesAppServicio.CaracterTENTDE + i).SetValue(tranfEntregaDet, valorTENTDEx);
                }
                listaTransferenciaEntregaDetalles.Add(tranfEntregaDet);
            }

            return listaTransferenciaEntregaDetalles;
        }

        #endregion

        #region Exportación versión numeral

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void borderCeldas(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void borderCeldas2(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// Bordes Laterales, arriba y abajo
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void borderCeldas3(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin) //TODOS LOS LATERALES, EL TOP Y EL BOTTOM
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        }

        /// <summary>
        /// Bordea el perimetro
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        public static void borderCeldas4(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin) //BORDES
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            ws.Cells[rowIni, colIni, rowIni, colFin].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowFin, colIni, rowFin, colFin].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIni, colIni, rowFin, colIni].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIni, colFin, rowFin, colFin].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// formato excel Cabecera
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="sizeFont"></param>
        public static void formatoCabecera(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, int sizeFont)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni, rowFin, colFin])
            {
                r1.Style.Font.Color.SetColor(Color.White);
                r1.Style.Font.Size = sizeFont;
                r1.Style.Font.Bold = true;
                r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                r1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }
        }

        /// <summary>
        /// formato excel Cabecera para el reporte de ejecutivo mensual
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="sizeFont"></param>
        public static void FormatoCabeceraEjecutivoMensual(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, int sizeFont)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni, rowFin, colFin])
            {
                r1.Style.Font.Color.SetColor(Color.Black);
                r1.Style.Font.Size = sizeFont;
                //r1.Style.Font.Bold = true;
                r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                r1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r1.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(ConstantesSiosein2.ColorFilaTablaRepEje));
                r1.Style.WrapText = true;
            }
        }

        /// <summary>
        /// formato para las notas  en el reporte de ejecutivo mensual
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        public static void FormatoNota(ExcelWorksheet ws, int rowIni, int colIni)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni])
            {
                r1.Style.Font.Color.SetColor(Color.Black);
                r1.Style.Font.SetFromFont(new Font("Arial", 7));
                r1.Style.Font.Italic = true;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
        }

        /// <summary>
        /// formato para las notas  en el reporte de ejecutivo mensual
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        public static void FormatoNota2(ExcelWorksheet ws, int rowIni, int colIni)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni])
            {
                r1.Style.Font.Color.SetColor(Color.Black);
                r1.Style.Font.SetFromFont(new Font("Arial", 7));
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
        }

        /// <summary>
        /// formato excel Datos para el reporte de ejecutivo mensual
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="sizeFont"></param>
        public static void FormatoCuerpoDatosEjecutivoMensual(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, int sizeFont)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni, rowFin, colFin])
            {
                r1.Style.Font.Color.SetColor(Color.Black);
                r1.Style.Font.Size = sizeFont;
                r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                r1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r1.Style.Fill.BackgroundColor.SetColor(Color.White);
                r1.Style.WrapText = true;
            }
        }

        /// <summary>
        /// formato excel solo letra para el reporte de ejecutivo mensual
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="sizeFont"></param>
        public static void FormatoTextoCuerpoDatosEjecutivoMensual(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, int sizeFont)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni, rowFin, colFin])
            {
                r1.Style.Font.Color.SetColor(Color.Black);
                r1.Style.Font.Size = sizeFont;
                //r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                r1.Style.WrapText = true;
            }
        }

        /// <summary>
        /// formato excel solo letra para el reporte de ejecutivo mensual
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="sizeFont"></param>
        public static void FormatoPiePaginaEjecutivoMensual(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, int sizeFont)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni, rowFin, colFin])
            {
                r1.Style.Font.SetFromFont(new Font("Arial", sizeFont));
                r1.Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorTranslator.FromHtml(ConstantesSiosein2.ColorBordeTablaRepEje));
                r1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                r1.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(ConstantesSiosein2.ColorTotTablaRepEje));
            }
        }

        /// <summary>
        /// formato excel solo letra para el reporte de ejecutivo mensual
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="rowFin"></param>
        /// <param name="colFin"></param>
        /// <param name="sizeFont"></param>
        public static void OcultarCeldas(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni, rowFin, colFin])
            {

                r1.Style.Font.Color.SetColor(Color.White);
                r1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                r1.Style.Fill.BackgroundColor.SetColor(Color.White);
            }
        }

        /// <summary>
        /// Configura la imagen que ira en el excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="tipo"></param>
        public static void AddImage(ExcelWorksheet ws, int tipo, string url)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(url + "Content/Images/coes_.png");

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic", image);
                picture.From.Column = 0;
                picture.From.Row = 0;
                picture.From.ColumnOff = Pixel2MTU(12);
                picture.From.RowOff = Pixel2MTU(10);
                picture.SetSize(200, 100);
            }
        }

        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }

        /// <summary>
        /// formato autowidth columnas excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="Ini"></param>
        /// <param name="Fin"></param>
        public static void AddAutoWidthColumn(ExcelWorksheet ws, int Ini, int Fin)
        {
            for (int z = Ini; z <= Fin; z++) { ws.Column(z).AutoFit(); }
        }

        #endregion

        #region Metodos utiles MAPE

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="fila"></param>
        /// <param name="columna"></param>
        public static void ExcelCabecera(ref ExcelWorksheet ws, string titulo, int fila, int columna)
        {
            ws.Cells[fila, columna].Value = titulo;
            var font = ws.Cells[fila, columna].Style.Font;
            font.Size = 14;
            font.Bold = true;
            font.Name = "Calibri";
        }

        /// <summary>
        /// Crear hoja de cálculo de Excel: aquí está la función que crea una hoja de cálculo
        /// </summary>
        /// <param name="p"></param>
        /// <param name="sheetName"></param>
        /// <param name="sheetId"></param>
        /// <returns></returns> 
        public static ExcelWorksheet CrearHoja(ExcelPackage p, string sheetName, int sheetId)
        {
            p.Workbook.Worksheets.Add(sheetName);
            ExcelWorksheet ws = p.Workbook.Worksheets[sheetId];
            ws.Name = sheetName; //Configuración del nombre de la hoja
            ws.Cells.Style.Font.Size = 11; //Tamaño de fuente predeterminado para toda la hoja
            ws.Cells.Style.Font.Name = "Calibri"; //Nombre de fuente predeterminado para toda la hoja
            return ws;
        }

        /// <summary>
        /// Estilo personalizado del grafico Linea epplus
        /// </summary>
        /// <param name="chart"></param>
        public static void ExcelLineEstiloPersonalizado(ExcelLineChart chart)
        {
            chart.DisplayBlanksAs = eDisplayBlanksAs.Gap;
            chart.RoundedCorners = false;
            chart.PlotArea.CreateDataTable();
            chart.Legend.Remove();
            chart.Border.Fill.Color = Color.LightGray;
            chart.YAxis.MajorGridlines.Fill.Color = Color.LightGray;
            chart.YAxis.Border.Fill.Color = Color.White;
            chart.PlotArea.DataTable.Border.Fill.Color = Color.LightGray;
        }

        /// <summary>
        /// Asignar series al grafico de Linea
        /// </summary>
        /// <param name="rango"></param>
        /// <param name="chart"></param>
        /// <param name="ws"></param>
        public static void LineaChartAsignarSeries(ExcelRange rango, ExcelLineChart chart, ExcelWorksheet ws)
        {
            var direccionSerieX = new ExcelAddress(rango.Start.Row, rango.Start.Column + 1, rango.Start.Row, rango.End.Column);

            for (int i = rango.Start.Row + 1; i <= rango.End.Row; i++)
            {
                var direccionSerie = new ExcelAddress(i, rango.Start.Column + 1, i, rango.End.Column);
                var ser = chart.Series.Add(direccionSerie.Address, direccionSerieX.Address);
                ser.Header = ws.Cells[i, rango.Start.Column].Value.ToString();
            }
        }

        /// <summary>
        /// Obtiene colores degradados en un rango de 2 colores
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public static IEnumerable<Color> ObtenerColoresDegradados(Color start, Color end, int steps)
        {
            int rMax = end.R;
            int rMin = start.R;

            int gMax = end.G;
            int gMin = start.G;

            int bMax = end.B;
            int bMin = start.B;
            // ... and for B, G
            var colorList = new List<Color>();
            for (int i = 0; i < steps; i++)
            {
                var rAverage = rMin + (int)((rMax - rMin) * i / steps);
                var gAverage = gMin + (int)((gMax - gMin) * i / steps);
                var bAverage = bMin + (int)((bMax - bMin) * i / steps);
                colorList.Add(Color.FromArgb(rAverage, gAverage, bAverage));
            }

            return colorList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rango"></param>
        public static void FormatoCondicionalTresEscalas(ExcelWorksheet ws, ExcelAddress rango)
        {
            var cfRule3 = ws.ConditionalFormatting.AddThreeColorScale(rango);
            cfRule3.MiddleValue.Type = eExcelConditionalFormattingValueObjectType.Percentile;
            cfRule3.MiddleValue.Value = 50;
            cfRule3.LowValue.Color = Color.FromArgb(72, 172, 100);
            cfRule3.HighValue.Color = Color.FromArgb(254, 80, 84);
            cfRule3.MiddleValue.Color = Color.FromArgb(250, 223, 73);
        }

        /// <summary>
        /// Valor total de fecha en milisegundos desde 1970
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static double TotalMillisegundosDesde1970(DateTime fecha)
        {
            var fechaUtc = DateTime.SpecifyKind(fecha, DateTimeKind.Utc);
            var totalMilliseconds = (fechaUtc - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            return totalMilliseconds;
        }

        /// <summary>
        /// Valor total de fecha en milisegundos desde 1970
        /// </summary>
        /// <param name="fecha"></param> 
        /// <returns></returns> 
        public static double TotalSegundos(DateTime fechafin, DateTime fechaInicio)
        {
            //var span = fechafin.Subtract(fechaInicio);


            var fechaFUtc = DateTime.SpecifyKind(fechafin, DateTimeKind.Utc);
            var fechaIUtc = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Utc);
            var span = TotalMillisegundosDesde1970(fechaFUtc) - TotalMillisegundosDesde1970(fechaIUtc);

            double hours = span / 3600000;
            return hours;
        }

        /// <summary>
        /// Recorre un rando de fecha por meses
        /// </summary>
        /// <param name="startYearMonth"></param>
        /// <param name="endYearMonth"></param>
        /// <param name="action"></param>
        public static void BuclePorMeses(DateTime startYearMonth, DateTime endYearMonth, Action<DateTime> action)
        {
            while (startYearMonth <= endYearMonth)
            {
                action(startYearMonth);
                startYearMonth = startYearMonth.AddMonths(1);
            }
        }

        /// <summary>
        /// Recorre un rango de fechas por año
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <param name="action"></param>
        public static void BuclePorAnhos(DateTime startYear, DateTime endYear, Action<DateTime> action)
        {
            while (startYear <= endYear)
            {
                action(startYear);
                startYear = startYear.AddYears(1);
            }
        }

        #endregion

        #region Metodos Excel

        /// <summary>
        /// Quita todas las series del un ExcelChart
        /// </summary>
        /// <param name="chart"></param>
        /// <returns></returns>
        public static void EliminarAllSeriesDelGrafico(ExcelChart chart)
        {
            int numSeries = chart.Series.Count;
            for (int i = 0; i < numSeries; i++)
            {
                chart.Series.Delete(0);
            }
        }

        /// <summary>
        /// Dar borde a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <returns></returns>
        public static void CeldasExcelBordear(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            var bloqueABordear = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloqueABordear.Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorTranslator.FromHtml(ConstantesSiosein2.ColorBordeTablaRepEje));

        }

        public static void CeldasExcelBordear1(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            var bloqueABordear = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloqueABordear.Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorTranslator.FromHtml(ConstantesSiosein2.ColorBordeTablaRepEje));
            bloqueABordear.Style.Fill.PatternType = ExcelFillStyle.Solid;
            bloqueABordear.Style.Fill.BackgroundColor.SetColor(Color.White);

        }

        /// <summary>
        /// Colocar las notas en los reportes
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIniNotasBD"></param>
        /// <param name="coluIniNotasBD"></param>
        /// <param name="version"></param>
        /// <param name="codReporte"></param>
        /// <param name="periodo"></param>
        public static void ColocarNotasEnReporte(ExcelWorksheet ws, int filaIniNotasBD, int coluIniNotasBD, List<SiNotaDTO> listaNotas, out int numNotas)
        {
            numNotas = 0;

            if (listaNotas.Any())
            {
                numNotas = listaNotas.Count;
                ws.Cells[filaIniNotasBD, coluIniNotasBD].Value = "NOTAS";

                int filaNota = 0;
                foreach (var nota in listaNotas)
                {
                    ws.Cells[filaIniNotasBD + 1 + filaNota, coluIniNotasBD].Value = nota.Sinotadesc;
                    ws.Row(filaIniNotasBD + 1 + filaNota).Height = 12;
                    filaNota++;
                }

                #region formatoNotaBD
                CeldasExcelTipoYTamanioLetra(ws, filaIniNotasBD, coluIniNotasBD, filaIniNotasBD, coluIniNotasBD, "Arial", 7);
                CeldasExcelEnNegrita(ws, filaIniNotasBD, coluIniNotasBD, filaIniNotasBD, coluIniNotasBD);

                CeldasExcelTipoYTamanioLetra(ws, filaIniNotasBD + 1, coluIniNotasBD, filaIniNotasBD + 1 + filaNota, coluIniNotasBD, "Arial", 6);
                #endregion
            }
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
        public static void CeldasExcelAlinearHorizontalmente(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string alineacion)
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
        ///  Agrupar varias celdas en un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <returns></returns>
        public static void CeldasExcelAgrupar(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            ws.Cells[filaIni, coluIni, filaFin, coluFin].Merge = true;
        }

        /// <summary>
        /// Alinear  verticalmente a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="alineacion"></param>
        /// <returns></returns>
        public static void CeldasExcelAlinearVerticalmente(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string alineacion)
        {
            var rg = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            switch (alineacion)
            {
                case "Arriba": rg.Style.VerticalAlignment = ExcelVerticalAlignment.Top; break;
                case "Centro": rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center; break;
                case "Abajo": rg.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom; break;
                case "Distribuido": rg.Style.VerticalAlignment = ExcelVerticalAlignment.Distributed; break;
                case "Justificado": rg.Style.VerticalAlignment = ExcelVerticalAlignment.Justify; break;
            }
        }

        /// <summary>
        ///  Dar tipo y tamanio de letra a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="tipoLetra"></param>
        /// <param name="tamLetra"></param>
        /// <returns></returns>
        public static void CeldasExcelTipoYTamanioLetra(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string tipoLetra, int tamLetra)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Font.SetFromFont(new Font(tipoLetra, tamLetra));
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
        public static void CeldasExcelEnNegrita(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Font.Bold = true;
        }

        /// <summary>
        ///  Dar Wrap a una celda de la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <returns></returns>
        public static void CeldasExcelWrapText(ExcelWorksheet ws, int filaIni, int coluIni)
        {
            var bloque = ws.Cells[filaIni, coluIni];
            bloque.Style.WrapText = true;
        }

        /// <summary>
        ///  Dar Wrap a una celda de la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <returns></returns>
        public static void CeldasExcelWrapText(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.WrapText = true;
        }

        /// <summary>
        ///  Dar tipo y tamanio de letra a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static void CeldasExcelColorTexto(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string color)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            if (color.Contains(","))
            {
                bloque.Style.Font.Color.SetColor(Color.FromArgb(Int32.Parse(color)));
            }
            if (color.Contains("#"))
            {
                bloque.Style.Font.Color.SetColor(ColorTranslator.FromHtml(color));
            }

        }

        /// <summary>
        ///  Dar tipo y tamanio de letra a un bloque en la tabla excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static void CeldasExcelColorFondo(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string color)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            if (color.Contains(","))
            {
                bloque.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(Int32.Parse(color)));
            }
            if (color.Contains("#"))
            {
                bloque.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(color));
            }

        }

        #endregion

    }
}
