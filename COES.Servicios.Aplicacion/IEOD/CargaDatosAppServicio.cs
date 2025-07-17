using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD.Helper;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;

namespace COES.Servicios.Aplicacion.IEOD
{
    public class CargaDatosAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CargaDatosAppServicio));

        /// <summary>
        /// Permite obtener los datos
        /// </summary>
        /// <param name="idReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public EstructuraRespuesta ConsultarDatos(int idReporte, int tipoinfocodi, DateTime fecha, int tipo)
        {
            EstructuraRespuesta response = new EstructuraRespuesta();
            try
            {
                List<MeMedicion48DTO> result = this.ObtenerDataMedicion(idReporte, fecha, fecha, tipoinfocodi);
                List<MePtomedicionDTO> puntosMedicion = this.ObtenerPuntosMedicionReporte(idReporte, tipoinfocodi);

                response = this.ObtenerEstructura(puntosMedicion, result, fecha, fecha, -1, false, tipo);
                response.Result = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                response.Result = -1;
            }

            return response;
        }

        /// <summary>
        /// Permite obtener los datos scada
        /// </summary>
        /// <param name="idReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public EstructuraRespuesta ConsultarDatosScada(int idReporte, int tipoinfocodi, DateTime fecha, int tipo)
        {
            EstructuraRespuesta response = new EstructuraRespuesta();
            try
            {
                List<MePtomedicionDTO> puntosMedicion = this.ObtenerPuntosMedicionReporte(idReporte, tipoinfocodi);
                List<MeMedicion48DTO> result = this.ObtenerDataScada(puntosMedicion, idReporte, fecha, tipoinfocodi);
                response = this.ObtenerEstructura(puntosMedicion, result, fecha, fecha, -1, false, tipo);
                response.Result = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                response.Result = -1;
            }

            return response;
        }

        /// <summary>
        /// Permite obtener la consulta de datos
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="reporte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public EstructuraRespuesta ObtenerReporte(int reporte, int dia, int tipoinfocodi, DateTime fechaInicio, DateTime fechaFin, int tipo)
        {
            EstructuraRespuesta response = new EstructuraRespuesta();
            try
            {
                List<MeMedicion48DTO> result = this.ObtenerDataMedicion(reporte, fechaInicio, fechaFin, tipoinfocodi);
                List<MePtomedicionDTO> puntosMedicion = this.ObtenerPuntosMedicionReporte(reporte, tipoinfocodi);
                response = this.ObtenerEstructura(puntosMedicion, result, fechaInicio, fechaFin, dia, true, tipo);
                response.Result = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                response.Result = -1;
            }

            return response;
        }

        /// <summary>
        /// Permite obtener la estructura de datos
        /// </summary>
        /// <param name="listaPuntos"></param>
        /// <param name="datos"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="diaSemana"></param>
        /// <param name="incluirFeriado"></param>
        /// <returns></returns>
        public EstructuraRespuesta ObtenerEstructura(List<MePtomedicionDTO> listaPuntos, List<MeMedicion48DTO> datos, DateTime fechaInicio,
            DateTime fechaFin, int diaSemana, bool incluirFeriado, int tipo)
        {
            EstructuraRespuesta respuesta = new EstructuraRespuesta();

            List<List<string>> result = new List<List<string>>();
            List<InformacionCelda> merge = new List<InformacionCelda>();
            List<InformacionCelda> informacionCelda = new List<InformacionCelda>();
            List<InformacionCelda> celdasFeriados = new List<InformacionCelda>();
            List<DocDiaEspDTO> listaFeriados = new List<DocDiaEspDTO>();
            if (incluirFeriado) listaFeriados = FactorySic.GetDocDiaEspRepository().List();

            int dias = (int)fechaFin.Subtract(fechaInicio).TotalDays;
            List<DateTime> fechas = new List<DateTime>();
            if (diaSemana > -1)
            {
                fechas = CargaDatosUtil.ObtenerFechasPorDiaSemana(fechaInicio, fechaFin, diaSemana);
                dias = fechas.Count - 1;
            }

            int longitud = (dias + 1) * 48 + ((tipo == 1) ? 9 : 10);

            for (int i = 0; i < longitud; i++)
            {
                List<string> itemDato = new List<string>();
                for (int k = 0; k <= listaPuntos.Count; k++) itemDato.Add("");

                result.Add(itemDato);
                if (i == 0) result[i][0] = "PTO MEDICIÓN";
                if (i == 1) result[i][0] = "CANAL SCADA";
                if (i == 2) result[i][0] = "SS.EE.";
                if (i == 3) result[i][0] = (tipo == 1) ? "FECHA HORA" : "NIVEL TENSION (kV)";
                if (i == 4 && tipo == 2) result[i][0] = "FECHA HORA";
                if (i == longitud - 5) result[i][0] = "MÁXIMO";
                if (i == longitud - 4) result[i][0] = "MÍNIMO";
                if (i == longitud - 3) result[i][0] = "PROMEDIO";
                if (i == longitud - 2) result[i][0] = "ÁREA OPERATIVA";
                if (i == longitud - 1) result[i][0] = "EMPRESA";
            }

            int rowData = (tipo == 1) ? 4 : 5;
            int colData = 1;
            for (int i = 0; i <= dias; i++)
            {
                DateTime fecha = (diaSemana > -1) ? fechas[i] : fechaInicio.AddDays(i);
                for (int j = 1; j <= 48; j++)
                {
                    result[rowData][0] = fecha.AddMinutes(30 * j).ToString(ConstantesAppServicio.FormatoFechaFull);
                    rowData++;
                }
            }
            rowData = (tipo == 1) ? 4 : 5;
            var listaColorColumna = new List<string>();
            foreach (MePtomedicionDTO pto in listaPuntos)
            {
                result[0][colData] = pto.Ptomedicodi.ToString();
                result[1][colData] = pto.Canales;
                result[2][colData] = pto.Subestacion;
                result[3][colData] = (tipo == 1) ? pto.Repptonomb.Trim() : pto.NivelTension.ToString();
                if (tipo == 2) result[4][colData] = pto.Repptonomb;

                //color cabecera
                string colorPto = pto.Colorcelda ?? "#4C97C3";
                listaColorColumna.Add(colorPto);

                List<decimal> valores = new List<decimal>();
                rowData = (tipo == 1) ? 4 : 5;

                for (int i = 0; i <= dias; i++)
                {
                    DateTime fecha = (diaSemana > -1) ? fechas[i] : fechaInicio.AddDays(i);
                    MeMedicion48DTO itemData = datos.Where(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == fecha).FirstOrDefault();
                    itemData = (itemData != null) ? itemData : new MeMedicion48DTO();
                    bool esFeriado = false;
                    if (incluirFeriado)
                    {
                        if ((new GeneralAppServicio()).EsFeriadoByFecha(fecha, listaFeriados))
                        {
                            esFeriado = true;
                        }
                    }

                    for (int j = 1; j <= 48; j++)
                    {
                        var itemDato = itemData.GetType().GetProperty("H" + j).GetValue(itemData);
                        var itemTipo = itemData.GetType().GetProperty("T" + j).GetValue(itemData);
                        decimal valor = (itemDato != null) ? (decimal)itemDato : 0;
                        string txtValor = (itemDato != null) ? itemDato.ToString() : string.Empty;
                        result[rowData][colData] = txtValor;
                        valores.Add(valor);

                        if (itemDato == null)
                        {
                            informacionCelda.Add(new InformacionCelda { Row = rowData, Col = colData, Tipo = -1 }); //- Sin datos
                        }
                        else
                        {
                            int tipoDato = (itemTipo != null) ? (int)itemTipo : -1;
                            informacionCelda.Add(new InformacionCelda { Row = rowData, Col = colData, Tipo = tipoDato });
                        }

                        if (esFeriado)
                        {
                            celdasFeriados.Add(new InformacionCelda { Row = rowData, Col = colData });
                        }

                        rowData++;
                    }
                }

                result[rowData][colData] = valores.Any() ? valores.Max().ToString() : "";
                result[rowData + 1][colData] = valores.Any() ? valores.Min().ToString() : "";
                result[rowData + 2][colData] = valores.Any() ? Math.Round(((double)valores.Average()), 4).ToString() : "";
                result[rowData + 3][colData] = pto.AreaOperativa;
                result[rowData + 4][colData] = pto.Emprnomb;

                colData++;
            }

            int totalPto = listaPuntos.Count;

            //merge subestación
            colData = 1;
            var listaSig = new List<MePtomedicionDTO>();
            listaSig.AddRange(listaPuntos);
            for (int i = 0; i < totalPto; i++)
            {
                MePtomedicionDTO regPto = listaPuntos[i];
                int nSigSSEE = listaSig.FindIndex(x => x.Ptomedidesc != regPto.Ptomedidesc);

                int numElemSig = 1;

                if (nSigSSEE > -1)
                {
                    listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                    i += (nSigSSEE - 1);

                    numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                }
                else
                {
                    //si no existe un elemento distinto terminar la iteracion
                    i = totalPto;
                    numElemSig = listaSig.Count();
                }


                if (numElemSig > 1)
                    merge.Add(new InformacionCelda { Row = 2, Col = colData, Colspan = numElemSig });
                colData += numElemSig;
            }

            //merge empresa
            colData = 1;
            listaSig = new List<MePtomedicionDTO>();
            listaSig.AddRange(listaPuntos);
            for (int i = 0; i < totalPto; i++)
            {
                MePtomedicionDTO regPto = listaPuntos[i];
                int nSigSSEE = listaSig.FindIndex(x => x.Emprcodi != regPto.Emprcodi);

                int numElemSig = 1;

                if (nSigSSEE > -1)
                {
                    listaSig = listaSig.GetRange(nSigSSEE, listaSig.Count() - nSigSSEE);
                    i += (nSigSSEE - 1);

                    numElemSig = nSigSSEE > 0 ? nSigSSEE : 1;
                }
                else
                {
                    //si no existe un elemento distinto terminar la iteracion
                    i = totalPto;
                    numElemSig = listaSig.Count();
                }


                if (numElemSig > 1)
                    merge.Add(new InformacionCelda { Row = longitud - 1, Col = colData, Colspan = numElemSig });
                colData += numElemSig;
            }

            respuesta.Data = result;
            respuesta.ListaInformacionCelda = informacionCelda;
            respuesta.ListaMerge = merge;
            respuesta.ListaFeriados = celdasFeriados;
            respuesta.ExisteDatos = datos.Any() ? 1 : 0;
            respuesta.ListaColorColumna = listaColorColumna;

            return respuesta;
        }

        /// <summary>
        /// Permite obtener los puntos de medición por reporte
        /// </summary>
        /// <param name="idReporte"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ObtenerPuntosMedicionReporte(int idReporte, int tipoinfocodi)
        {
            return (new PR5ReportesAppServicio()).ObtenerPuntosMedicionReporte(idReporte, tipoinfocodi);
        }

        /// <summary>
        /// Permite obtener los datos mediciones
        /// </summary>
        /// <param name="idReporte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerDataMedicion(int idReporte, DateTime fechaInicio, DateTime fechaFin, int tipoinfocodi)
        {
            return FactorySic.GetMeMedicion48Repository().ObtenerDatosPorReporte(idReporte, fechaInicio, fechaFin, tipoinfocodi);
        }

        /// <summary>
        /// Permite obtener los datos desde SCADA
        /// </summary>
        /// <param name="puntos"></param>
        /// <param name="reporte"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerDataScada(List<MePtomedicionDTO> puntos, int reporte, DateTime fecha, int tipoinfocodi)
        {
            List<MeMedicion48DTO> result = new List<MeMedicion48DTO>();

            List<MeMedicion48DTO> datosScada = FactoryScada.GetMeScadaSp7Repository().ObtenerDatosPorReporte(reporte, fecha, tipoinfocodi);

            foreach (MePtomedicionDTO itemPto in puntos)
            {
                MeMedicion48DTO itemData = datosScada.Where(x => x.Ptomedicodi == itemPto.Ptomedicodi).FirstOrDefault();
                if (itemData == null)
                {
                    //solo consultar circulares para fechas anteriores a hoy
                    if (fecha < DateTime.Today && !string.IsNullOrEmpty(itemPto.Canales))
                    {
                        List<TrCircularSp7DTO> listTr = new List<TrCircularSp7DTO>();
                        try
                        {
                            listTr = FactoryScada.GetTrCircularSp7Repository().ObtenerConsultaFlujos(
                                  fecha.ToString(ConstantesAppServicio.FormatoAnioMesDia), itemPto.Canales);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ConstantesAppServicio.LogError, ex);
                        }

                        if (listTr.Count > 0)
                        {
                            MeMedicion48DTO newItem = new MeMedicion48DTO();
                            newItem.Ptomedicodi = itemPto.Ptomedicodi;
                            newItem.Lectcodi = itemPto.Lectcodi;
                            newItem.Tipoinfocodi = (int)itemPto.Tipoinfocodi;
                            newItem.Medifecha = fecha;

                            for (int i = 1; i <= 48; i++)
                            {
                                DateTime fechaItem = fecha.AddMinutes((i - 1) * 30);
                                List<TrCircularSp7DTO> listTrHora = listTr.Where(x =>
                                x.Canalfechahora.Hour == fechaItem.Hour && x.Canalfechahora.Minute == fechaItem.Minute && x.Canalfechahora.Second == fechaItem.Second
                                && x.Canalfechahora.Millisecond == 0).ToList();
                                decimal? valor = null;
                                if (listTrHora.Count > 0)
                                {
                                    valor = listTrHora.Sum(x => x.Canalvalor);
                                }

                                newItem.GetType().GetProperty("H" + i).SetValue(newItem, valor);
                                int tipo = -1;
                                if (valor != null) tipo = 3;
                                newItem.GetType().GetProperty("T" + i).SetValue(newItem, tipo);
                            }

                            result.Add(newItem);
                        }
                    }
                }
                else
                {
                    itemData.Ptomedicodi = itemPto.Ptomedicodi;
                    itemData.Lectcodi = itemPto.Lectcodi;
                    itemData.Tipoinfocodi = (int)itemPto.Tipoinfocodi;
                    itemData.Medifecha = fecha;
                    result.Add(itemData);
                }
            }

            return result;
        }

        /// <summary>
        /// Permite generar la plantilla Excel
        /// </summary>
        /// <param name="idReporte"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <param name="tipoDato"></param>
        /// <returns></returns>
        public int GenerarFormatoCarga(int idReporte, string path, string filename, int tipoinfocodi, DateTime fecha, int tipo, int tipoDato)
        {
            try
            {
                EstructuraRespuesta result = new EstructuraRespuesta();
                switch (tipoDato)
                {
                    case CargaDatosUtil.TipoInfoMed48:
                        result = this.ConsultarDatos(idReporte, tipoinfocodi, fecha, tipo);
                        break;
                    case CargaDatosUtil.TipoInfoScada:
                        result = this.ConsultarDatosScada(idReporte, tipoinfocodi, fecha, tipo);
                        break;
                }

                GenerarHojaExcelFormato(idReporte, path, filename, result, tipo);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite generar la plantilla Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="reporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public int GenererReporteConsulta(int idReporte, string path, string filename, int dia, int tipoinfocodi, DateTime fechaInicio, DateTime fechaFin, int tipo)
        {
            try
            {
                List<MeMedicion48DTO> data = this.ObtenerDataMedicion(idReporte, fechaInicio, fechaFin, tipoinfocodi);
                List<MePtomedicionDTO> puntosMedicion = this.ObtenerPuntosMedicionReporte(idReporte, tipoinfocodi);
                EstructuraRespuesta result = this.ObtenerEstructura(puntosMedicion, data, fechaInicio, fechaFin, dia, true, tipo);

                GenerarHojaExcelFormato(idReporte, path, filename, result, tipo);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        private void GenerarHojaExcelFormato(int idReporte, string path, string filename, EstructuraRespuesta result, int tipo)
        {
            string file = path + filename;
            FileInfo newFile = new FileInfo(file);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }

            int rowData = (tipo == 1) ? 4 : 5;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("MEDICIONES");

                if (ws != null)
                {
                    //cabecera
                    ExcelRange rg = ws.Cells[1, 1, rowData, result.Data[0].Count];
                    CargaDatosUtil.FormatearCeldaTitulo(rg);

                    ws.Column(1).Width = 18;
                    for (int j = 1; j < result.Data[0].Count; j++)
                    {
                        //colores de celda
                        string colorColPto = result.ListaColorColumna[j - 1];
                        if (tipo == 1)
                        {
                            UtilExcel.CeldasExcelColorFondo(ws, 4, j + 1, 4, j + 1, colorColPto);
                        }
                        else
                        {
                            UtilExcel.CeldasExcelColorFondo(ws, 1, j + 1, rowData, j + 1, colorColPto);
                        }

                        ws.Column(j + 1).Width = 14;
                    }

                    //cuerpo
                    rg = ws.Cells[rowData, 1, result.Data.Count, 1];
                    CargaDatosUtil.FormatearCeldaTitulo(rg);
                    rg = ws.Cells[result.Data.Count - 4, 2, result.Data.Count, result.Data[0].Count];
                    CargaDatosUtil.FormatearCeldaTitulo(rg);

                    for (int i = 0; i < result.Data.Count; i++)
                    {
                        for (int j = 0; j < result.Data[0].Count; j++)
                        {
                            ws.Cells[i + 1, j + 1].Value = result.Data[i][j];
                            UtilExcel.CeldasExcelWrapText(ws, i + 1, j + 1, i + 1, j + 1);

                            InformacionCelda info = result.ListaInformacionCelda.Where(x => x.Row == i && x.Col == j).FirstOrDefault();
                            if (info != null)
                            {
                                rg = ws.Cells[i + 1, j + 1, i + 1, j + 1];
                                CargaDatosUtil.FormatearCeldaItem(rg, info.Tipo);
                            }
                        }
                    }

                    foreach (InformacionCelda item in result.ListaMerge)
                    {
                        ws.Cells[item.Row + 1, item.Col + 1, item.Row + 1, item.Col + item.Colspan].Merge = true;
                    }

                    rg = ws.Cells[1, 1, result.Data.Count, result.Data[0].Count];
                    CargaDatosUtil.FormatearCeldaGeneral(rg);

                    ws.View.FreezePanes(rowData + 1, 1 + 1);

                    CargaDatosUtil.AgregarSeccionNota(idReporte, ws, result.Data.Count + 2, 1);
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite obtener los datos desde Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="reporte"></param>
        /// <param name="fecha"></param>
        /// <param name="validaciones"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public EstructuraRespuesta CargarDataExcel(int idReporte, string path, string file, int tipoinfocodi, DateTime fecha, out List<string> validaciones, int tipo)
        {
            EstructuraRespuesta result = new EstructuraRespuesta();
            validaciones = new List<string>();
            try
            {

                FileInfo fileInfo = new FileInfo(path + file);
                List<string> errores = new List<string>();
                List<MePtomedicionDTO> puntosMedicion = this.ObtenerPuntosMedicionReporte(idReporte, tipoinfocodi);
                List<MeMedicion48DTO> listaData = new List<MeMedicion48DTO>();
                int filaFinCabecera = tipo == 1 ? 4 : 5;
                int filaIniFormula = (tipo == 1 ? 53 : 54);

                //leer archivo
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets["MEDICIONES"];

                    if (ws != null)
                    {
                        bool flagEstrucura = true;
                        bool flagData = true;
                        //- Validamos que los puntos de medición estén completos
                        for (int i = 0; i < puntosMedicion.Count; i++)
                        {
                            bool flag = false;
                            for (int k = 0; k < puntosMedicion.Count; k++)
                            {
                                if ((ws.Cells[1, 2 + k].Value != null) && ws.Cells[1, 2 + k].Value.ToString() == puntosMedicion[i].Ptomedicodi.ToString())
                                {
                                    flag = true;
                                    break;
                                }
                            }

                            if (!flag)
                            {
                                errores.Add("El punto de medición " + puntosMedicion[i].Ptomedicodi + " no existe en el formato.");
                                flagEstrucura = false;
                            }
                        }

                        //- Validamos que las fechas estén completas
                        for (int i = 1; i <= 48; i++)
                        {
                            DateTime fechaDato = fecha.AddMinutes(i * 30);
                            if (ws.Cells[filaFinCabecera + i, 1].Value != null && ws.Cells[filaFinCabecera + i, 1].Value.ToString() != fechaDato.ToString(ConstantesAppServicio.FormatoFechaFull))
                            {
                                errores.Add("La fecha y hora de la fila " + (filaFinCabecera + i).ToString() + " debe ser " + fechaDato.ToString(ConstantesAppServicio.FormatoFechaFull));
                                flagEstrucura = false;
                            }
                        }

                        if (flagEstrucura)
                        {
                            for (int i = 1; i <= puntosMedicion.Count; i++)
                            {
                                MeMedicion48DTO itemDato = new MeMedicion48DTO();
                                itemDato.Ptomedicodi = int.Parse(ws.Cells[1, i + 1].Value.ToString());
                                itemDato.Medifecha = fecha;

                                for (int j = filaFinCabecera + 1; j < filaIniFormula; j++)
                                {
                                    var obj = ws.Cells[j, 1 + i].Value;
                                    string color = ws.Cells[j, 1 + i].Style.Fill.BackgroundColor.Rgb;
                                    itemDato.GetType().GetProperty("H" + (j - filaFinCabecera).ToString()).SetValue(itemDato, null);
                                    int idColor = CargaDatosUtil.ObtenerCodigoColor(color);

                                    if (obj != null && obj.ToString() != string.Empty)
                                    {
                                        decimal valor = 0;
                                        if (decimal.TryParse(obj.ToString(), out valor))
                                        {
                                            itemDato.GetType().GetProperty("H" + (j - filaFinCabecera).ToString()).SetValue(itemDato, valor);
                                        }
                                        else
                                        {
                                            errores.Add("La celda [" + j + ", " + (i + 1).ToString() + "] debe tener formato numérico.");
                                            flagData = false;
                                        }
                                        itemDato.GetType().GetProperty("T" + (j - filaFinCabecera).ToString()).SetValue(itemDato, idColor);
                                    }
                                    else
                                    {
                                        itemDato.GetType().GetProperty("T" + (j - filaFinCabecera).ToString()).SetValue(itemDato, -1);
                                    }

                                }

                                listaData.Add(itemDato);
                            }
                        }

                        if (flagEstrucura && flagData)
                        {
                            result = this.ObtenerEstructura(puntosMedicion, listaData, fecha, fecha, -1, false, tipo);
                            result.Result = 1;
                        }
                        else
                        {
                            result = new EstructuraRespuesta();
                            if (!flagEstrucura) result.Result = 2;
                            if (!flagData) result.Result = 3;
                            validaciones = errores;
                        }

                    }
                }

                //eliminar archivo temporal
                if (System.IO.File.Exists(path + file))
                {
                    System.IO.File.Delete(path + file);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                result = new EstructuraRespuesta();
                result.Result = -1;
            }

            return result;
        }

        /// <summary>
        /// Permite almacenar la data de mediciones
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listInformacion"></param>
        /// <param name="idReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int GrabarMediciones(int idReporte, string[][] data, List<InformacionCelda> listInformacion, int tipoinfocodi, DateTime fecha,
                                        int tipo, string userName, DateTime fechaRegistro)
        {
            int result = 1;

            try
            {
                MeReporteDTO reporte = FactorySic.GetMeReporteRepository().GetById(idReporte);
                List<MePtomedicionDTO> puntosMedicion = this.ObtenerPuntosMedicionReporte(idReporte, tipoinfocodi);
                List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

                int filaFinCabecera = tipo == 1 ? 4 : 5;
                int filaIniData = filaFinCabecera - 1;

                for (int i = 0; i < puntosMedicion.Count; i++)
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    entity.Medifecha = fecha;
                    entity.Ptomedicodi = int.Parse(data[0][i + 1]);

                    MePtomedicionDTO pto = puntosMedicion.Where(x => x.Ptomedicodi == entity.Ptomedicodi).First();

                    entity.Tipoinfocodi = (int)pto.Tipoinfocodi;
                    entity.Lectcodi = (int)reporte.Lectcodi;
                    entity.Emprcodi = (int)pto.Emprcodi;
                    entity.Lastuser = userName;
                    entity.Lastdate = fechaRegistro;
                    decimal suma = 0;

                    for (int j = 1; j <= 48; j++)
                    {
                        if (!string.IsNullOrEmpty(data[filaIniData + j][i + 1]))
                        {
                            decimal valor = 0;
                            if (decimal.TryParse(data[filaIniData + j][i + 1], out valor))
                            {
                                entity.GetType().GetProperty("H" + j).SetValue(entity, valor);
                                suma = suma + valor;
                                InformacionCelda itemInfo = listInformacion.Where(x => x.Col == i + 1 && x.Row == filaIniData + j).FirstOrDefault();

                                if (itemInfo != null)
                                {
                                    entity.GetType().GetProperty("T" + j).SetValue(entity, itemInfo.Tipo);
                                }
                            }
                        }
                    }
                    entity.Meditotal = suma;
                    entitys.Add(entity);
                }

                foreach (MeMedicion48DTO entity in entitys)
                {
                    FactorySic.GetMeMedicion48Repository().Delete(entity.Lectcodi, entity.Medifecha, entity.Tipoinfocodi, entity.Ptomedicodi);
                    FactorySic.GetMeMedicion48Repository().SaveInfoAdicional(entity);
                }

                result = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                result = -1;
            }
            return result;
        }

        #region Reporte Interconexiones

        /// <summary>
        /// Permite obtener el reporte de interconexion entre sistemas operativos del sein
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public EstructuraRespuesta ObtenerReporteInterconexion(DateTime fecha)
        {
            EstructuraRespuesta response = new EstructuraRespuesta();
            try
            {
                List<MeMedicion48DTO> resultCN = this.ObtenerDataMedicion(ConstantesPR5ReportesServicio.IdReporteInterconexionCentroNorte, fecha, fecha, ConstantesAppServicio.TipoinfocodiMW);
                List<MePtomedicionDTO> puntosMedicionCN = this.ObtenerPuntosMedicionReporte(ConstantesPR5ReportesServicio.IdReporteInterconexionCentroNorte, ConstantesAppServicio.TipoinfocodiMW);
                puntosMedicionCN.Add(new MePtomedicionDTO { Ptomedicodi = 0, Equinomb = "TOTAL", Emprnomb = "TOTAL" });
                resultCN.Add(this.CalcularSuma(resultCN, fecha));


                List<MeMedicion48DTO> resultCS = this.ObtenerDataMedicion(ConstantesPR5ReportesServicio.IdReporteInterconexionCentroSur, fecha, fecha, ConstantesAppServicio.TipoinfocodiMW);
                List<MePtomedicionDTO> puntosMedicionCS = this.ObtenerPuntosMedicionReporte(ConstantesPR5ReportesServicio.IdReporteInterconexionCentroSur, ConstantesAppServicio.TipoinfocodiMW);
                puntosMedicionCS.Add(new MePtomedicionDTO { Ptomedicodi = 0, Equinomb = "TOTAL", Emprnomb = "TOTAL" });
                resultCS.Add(this.CalcularSuma(resultCS, fecha));

                response = this.ObtenerEstructuraInterconexion(puntosMedicionCN, resultCN, puntosMedicionCS, resultCS, fecha, fecha);

                if ((resultCN.Count == 0 && resultCS.Count == 0) || (resultCN.Count == 1 && resultCS.Count == 1))
                {
                    List<List<string>> subData = new List<List<string>>();
                    for (int i = 0; i <= 3; i++)
                        subData.Add(response.Data[i]);

                    response.Data = subData;
                    response.ExisteDatos = -1;

                }

                response.Result = 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                response.Result = -1;
            }

            return response;
        }

        /// <summary>
        /// Permite obtener la columna total
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public MeMedicion48DTO CalcularSuma(List<MeMedicion48DTO> entitys, DateTime fecha)
        {
            MeMedicion48DTO entity = new MeMedicion48DTO();
            entity.Ptomedicodi = 0;
            entity.Medifecha = fecha;

            for (int i = 1; i <= 48; i++)
            {
                decimal suma = 0;
                foreach (MeMedicion48DTO item in entitys)
                {
                    decimal valor = 0;
                    object val = item.GetType().GetProperty("H" + i).GetValue(item);
                    if (val != null)
                    {
                        valor = Math.Abs((decimal)val);
                    }
                    suma = suma + valor;
                }
                entity.GetType().GetProperty("H" + i).SetValue(entity, suma);
            }

            return entity;
        }

        /// <summary>
        /// Permite obtener la estructura de datos para la consulta de interconexiones
        /// </summary>
        /// <param name="listaPuntosCN"></param>
        /// <param name="datosCN"></param>     
        /// <param name="listaPuntosCS"></param>
        /// <param name="datosCS"></param>     
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public EstructuraRespuesta ObtenerEstructuraInterconexion(
            List<MePtomedicionDTO> listaPuntosCN, List<MeMedicion48DTO> datosCN,
            List<MePtomedicionDTO> listaPuntosCS, List<MeMedicion48DTO> datosCS,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            EstructuraRespuesta respuesta = new EstructuraRespuesta();
            List<List<string>> result = new List<List<string>>();
            List<InformacionCelda> merge = new List<InformacionCelda>();

            int dias = (int)fechaFin.Subtract(fechaInicio).TotalDays;
            int longitud = (dias + 1) * 48 + 8;

            for (int i = 0; i < longitud; i++)
            {
                List<string> itemDato = new List<string>();
                for (int k = 0; k <= listaPuntosCN.Count + listaPuntosCS.Count; k++) itemDato.Add("");

                result.Add(itemDato);
                if (i == 0) result[i][0] = "INTERCONEXIÓN ENTRE SISTEMAS OPERATIVOS DEL SEIN";
                if (i == 1) result[i][0] = "SS.EE.";
                if (i == 1) result[i][1] = "CENTRO - NORTE";
                if (i == 1) result[i][1 + listaPuntosCN.Count] = "CENTRO - SUR";
                if (i == 2) result[i][0] = "PTO MEDICIÓN";
                if (i == 3) result[i][0] = "HORA";
                if (i == longitud - 4) result[i][0] = "MÁXIMO";
                if (i == longitud - 3) result[i][0] = "MÍNIMO";
                if (i == longitud - 2) result[i][0] = "PROMEDIO";
                if (i == longitud - 1) result[i][0] = "EMPRESA";
            }

            merge.Add(new InformacionCelda { Row = 0, Col = 0, Colspan = 1 + listaPuntosCN.Count + listaPuntosCS.Count });
            merge.Add(new InformacionCelda { Row = 1, Col = 1, Colspan = listaPuntosCN.Count });
            merge.Add(new InformacionCelda { Row = 1, Col = 1 + listaPuntosCN.Count, Colspan = listaPuntosCS.Count });

            int rowData = 4;
            int colData = 1;
            for (int i = 0; i <= dias; i++)
            {
                DateTime fecha = fechaInicio.AddDays(i);
                for (int j = 1; j <= 48; j++)
                {
                    result[rowData][0] = fecha.AddMinutes(30 * j).ToString(ConstantesAppServicio.FormatoFechaFull);
                    rowData++;
                }
            }
            rowData = 4;
            foreach (MePtomedicionDTO pto in listaPuntosCN)
            {
                List<decimal> valores = new List<decimal>();
                for (int i = 0; i <= dias; i++)
                {
                    DateTime fecha = fechaInicio.AddDays(i);
                    MeMedicion48DTO itemData = datosCN.Where(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == fecha).FirstOrDefault();
                    itemData = (itemData != null) ? itemData : new MeMedicion48DTO();

                    for (int j = 1; j <= 48; j++)
                    {
                        var itemDato = itemData.GetType().GetProperty("H" + j).GetValue(itemData);
                        decimal valor = (itemDato != null) ? Math.Abs((decimal)itemDato) : 0;
                        string txtValor = (itemDato != null) ? valor.ToString() : string.Empty;
                        result[rowData][colData] = txtValor;
                        valores.Add(valor);
                        rowData++;
                    }
                }
                result[2][colData] = pto.Ptomedicodi.ToString();
                result[3][colData] = (pto.Subestacion != null) ? pto.Subestacion.Replace("SE ", "") + " \n " + pto.Equinomb : pto.Equinomb;
                result[rowData][colData] = valores.Max().ToString();
                result[rowData + 1][colData] = valores.Min().ToString();
                result[rowData + 2][colData] = Math.Round(((double)valores.Average()), 4).ToString();
                result[rowData + 3][colData] = pto.Emprnomb;
                colData++;
                rowData = 4;
            }

            rowData = 4;
            foreach (MePtomedicionDTO pto in listaPuntosCS)
            {
                List<decimal> valores = new List<decimal>();
                for (int i = 0; i <= dias; i++)
                {
                    DateTime fecha = fechaInicio.AddDays(i);
                    MeMedicion48DTO itemData = datosCS.Where(x => x.Ptomedicodi == pto.Ptomedicodi && x.Medifecha == fecha).FirstOrDefault();
                    itemData = (itemData != null) ? itemData : new MeMedicion48DTO();

                    for (int j = 1; j <= 48; j++)
                    {
                        var itemDato = itemData.GetType().GetProperty("H" + j).GetValue(itemData);
                        decimal valor = (itemDato != null) ? Math.Abs((decimal)itemDato) : 0;
                        string txtValor = (itemDato != null) ? valor.ToString() : string.Empty;
                        result[rowData][colData] = txtValor;
                        valores.Add(valor);
                        rowData++;
                    }
                }
                result[2][colData] = pto.Ptomedicodi.ToString();
                result[3][colData] = (pto.Subestacion != null) ? pto.Subestacion.Replace("SE ", "") + " \n " + pto.Equinomb : pto.Equinomb;
                result[rowData][colData] = valores.Max().ToString();
                result[rowData + 1][colData] = valores.Min().ToString();
                result[rowData + 2][colData] = Math.Round(((double)valores.Average()), 4).ToString();
                result[rowData + 3][colData] = pto.Emprnomb;
                colData++;
                rowData = 4;
            }

            respuesta.Data = result;
            respuesta.ListaMerge = merge;

            return respuesta;
        }

        /// <summary>
        /// Permite generar la plantilla Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="reporte"></param>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public int ExportarInterconexiones(string path, string filename, DateTime fecha)
        {
            try
            {
                EstructuraRespuesta result = this.ObtenerReporteInterconexion(fecha);
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Reporte");
                    ExcelWorksheet wsGrafico = xlPackage.Workbook.Worksheets.Add("Gráficos");

                    int row = 4;

                    if (ws != null)
                    {
                        ExcelRange rg = ws.Cells[row, 1, row + 3, result.Data[0].Count];
                        CargaDatosUtil.FormatearCeldaTitulo(rg);
                        rg = ws.Cells[row + 4, 1, row + result.Data.Count - 1, 1];
                        CargaDatosUtil.FormatearCeldaTitulo(rg);
                        rg = ws.Cells[row + result.Data.Count - 4, 2, row + result.Data.Count - 1, result.Data[0].Count];
                        CargaDatosUtil.FormatearCeldaTitulo(rg);

                        for (int i = 0; i < result.Data.Count; i++)
                        {
                            for (int j = 0; j < result.Data[0].Count; j++)
                            {
                                decimal? valor = null;
                                decimal val = 0;
                                if (decimal.TryParse(result.Data[i][j], out val))
                                {
                                    valor = val;
                                    ws.Cells[row + i, j + 1].Value = valor;
                                }
                                else
                                {
                                    ws.Cells[row + i, j + 1].Value = result.Data[i][j];
                                }

                            }
                        }

                        foreach (InformacionCelda item in result.ListaMerge)
                        {
                            ws.Cells[item.Row + row, item.Col + 1, item.Row + row, item.Col + item.Colspan].Merge = true;
                        }

                        rg = ws.Cells[row, 1, row + result.Data.Count, result.Data[0].Count];
                        CargaDatosUtil.FormatearCeldaGeneral(rg);

                        var colIniSN = result.ListaMerge[1].Col + 1;
                        var colFinSN = result.ListaMerge[1].Col + result.ListaMerge[1].Colspan;
                        var colIniSC = result.ListaMerge[2].Col + 1;
                        var colFinSC = result.ListaMerge[2].Col + result.ListaMerge[1].Colspan;

                        this.AgregarGrafico(colIniSN, colFinSN, 1, "graficoSN", "INTERCONEXIÓN ENTRE SISTEMAS OPERATIVOS CENTRO -  NORTE", ws, wsGrafico);
                        this.AgregarGrafico(colIniSC, colFinSC, 14, "graficoSC", "INTERCONEXIÓN ENTRE SISTEMAS OPERATIVOS CENTRO -  SUR", ws, wsGrafico);

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 0;
                        picture.From.Row = 0;
                        picture.To.Column = 1;
                        picture.To.Row = 1;
                        picture.SetSize(90, 45);

                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite agregar grafico al reporte
        /// </summary>
        /// <param name="colIni"></param>
        /// <param name="colFin"></param>
        /// <param name="posx"></param>
        /// <param name="name"></param>
        /// <param name="titulo"></param>
        /// <param name="ws"></param>
        public void AgregarGrafico(int colIni, int colFin, int posx, string name, string titulo, ExcelWorksheet ws, ExcelWorksheet wsGrafico)
        {
            var AreaChart = wsGrafico.Drawings.AddChart(name, eChartType.ColumnStacked);
            AreaChart.SetPosition(1, 1, posx, 1);
            AreaChart.SetSize(720, 420);

            for (int sn = colIni; sn < colFin; sn++)
            {
                var ran1 = ws.Cells[3 + 5, sn, 3 + 5 + 47, sn];
                var ran2 = ws.Cells[3 + 5, 1, 3 + 5 + 47, 1];

                var serie = (ExcelChartSerie)AreaChart.Series.Add(ran1, ran2);
                serie.Header = ws.Cells[3 + 4, sn].Value.ToString();
            }

            AreaChart.Title.Text = titulo;
            AreaChart.Title.Font.Bold = true;
            AreaChart.YAxis.Title.Text = "";
            AreaChart.Legend.Position = eLegendPosition.Bottom;
        }

        #endregion

        #region Mejora Migraciones

        /// <summary>
        /// Copiar datos al antiguo aplicativo de Desktop
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="usuario"></param>
        /// <param name="fechaRegistro"></param>
        public void CopiarDataALecturaCargaIEOD(DateTime fecha, string usuario, DateTime fechaRegistro)
        {
            var servPR = new PR5ReportesAppServicio();

            //lista de puntos de medición del "Reporte principal"
            List<MeReporptomedDTO> listaPto = FactorySic.GetMeReporptomedRepository().ListarPuntoReporte(ConstantesMigraciones.IdReporteDemandaAreaPrincipal, fecha);
            List<int> listaPtomedicodiNuevo = listaPto.Where(x => x.Repptoequivpto > 0).Select(x => x.Repptoequivpto.Value).ToList();
            List<int> listaPtomedicodiOld = listaPto.Select(x => x.Ptomedicodi).ToList();

            //insumo areas y subareas calculadas en memoria
            int lectcodiDespacho = ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto;
            List<MeMedicion48DTO> lista30minAreaYSubarea = servPR.ReporteDemandaPorAreaYSubareaDataReporte(lectcodiDespacho, fecha, fecha);
            lista30minAreaYSubarea = lista30minAreaYSubarea.Where(x => listaPtomedicodiNuevo.Contains(x.Ptomedicodi)).ToList();

            //insumo flujos de potencia activa ya guardadas en bd
            List<MeMedicion48DTO> lista30minFlujo = new List<MeMedicion48DTO>();
            int lectcodiFlujo = ConstantesPR5ReportesServicio.LectcodiFlujoPotencia;
            int tipoinfocodi = ConstantesPR5ReportesServicio.TipoinfoMW;

            string ptomedicodisNuevo = string.Join(",", listaPtomedicodiNuevo);
            string ptomedicodisOld = string.Join(",", listaPtomedicodiOld);
            if (listaPtomedicodiNuevo.Any())
            {
                lista30minFlujo = FactorySic.GetMeMedicion48Repository().ListarMeMedicion48ByFiltro(lectcodiFlujo.ToString(),
                             fecha, fecha, ptomedicodisNuevo, tipoinfocodi.ToString());
            }

            //filtrar data de insumos para guardar en BD otro lectcodi
            List<MeMedicion48DTO> lista30minIEOD = new List<MeMedicion48DTO>();
            lista30minIEOD.AddRange(lista30minAreaYSubarea);
            lista30minIEOD.AddRange(lista30minFlujo);

            if (lista30minIEOD.Any())
            {
                int lectcodiCargaIEOD = ConstantesPR5ReportesServicio.LectcodiEjecutadoPorAreaClienteDesktop;

                try
                {
                    //eliminar data previa
                    FactorySic.GetMeMedicion48Repository().DeleteMasivo(lectcodiCargaIEOD, fecha, tipoinfocodi.ToString(), ptomedicodisOld);

                    //guardar nueva data (23 registros)
                    foreach (MeMedicion48DTO entity in lista30minIEOD)
                    {
                        var regPtoEquiv = listaPto.Find(x => x.Repptoequivpto == entity.Ptomedicodi);
                        if (regPtoEquiv != null)
                        {
                            int ptomedicodiOld = regPtoEquiv.Ptomedicodi;

                            entity.Ptomedicodi = ptomedicodiOld;
                            entity.Lectcodi = lectcodiCargaIEOD;
                            entity.Tipoinfocodi = tipoinfocodi;
                            entity.Medifecha = fecha;
                            entity.Lastuser = usuario;
                            entity.Lastdate = fechaRegistro;
                            FactorySic.GetMeMedicion48Repository().Save(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw ex;
                }
            }
        }

        #endregion

    }

}
