using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.ServicioRPF;
using OfficeOpenXml;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo.Helper
{
    /// <summary>
    /// Clase de utilidades para el analisis de RPF
    /// </summary>
    public class RpfHelper
    {
        /// <summary>
        /// Permite obtener la configuracion del RPF
        /// </summary>
        ConfiguracionRPF configuracion = new ConfiguracionRPF();

        /// <summary>
        /// Lee los artículos desde el formato excel cargado
        /// </summary>
        /// <param name="codCliente"></param>
        /// <returns></returns>
        public List<ServicioRpfDTO> LeerDesdeFormato(string file)
        {
            try
            {
                List<ServicioRpfDTO> list = new List<ServicioRpfDTO>();

                int cantidad = 200;

                FileInfo fileInfo = new FileInfo(file);
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                    for (int i = 2; i <= cantidad; i++)
                    {
                        if (ws.Cells[i, 1].Value != null && ws.Cells[i, 1].Value != string.Empty)
                        {
                            ServicioRpfDTO item = new ServicioRpfDTO();

                            item.EMPRNOMB = ws.Cells[i, 1].Value.ToString();
                            item.EQUINOMB = (ws.Cells[i, 2].Value != null) ? ws.Cells[i, 2].Value.ToString() : string.Empty;
                            item.EQUIABREV = (ws.Cells[i, 3].Value != null) ? ws.Cells[i, 3].Value.ToString() : string.Empty;
                            item.PTOMEDICODI = (ws.Cells[i, 4].Value != null) ? int.Parse(ws.Cells[i, 4].Value.ToString()) : 0;
                            item.EQUICODI = (ws.Cells[i, 5].Value != null) ? int.Parse(ws.Cells[i, 5].Value.ToString()) : 0;
                            item.POTENCIAMAX = (ws.Cells[i, 6].Value != null) ? decimal.Parse(ws.Cells[i, 6].Value.ToString()) : 0;

                            list.Add(item);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los puntos para el gráfico
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ajuste"></param>
        /// <param name="list"></param>
        public SerieGraficoRpf ObtenerGrafico(decimal ajuste, List<RegistrorpfDTO> list, ServicioRpfDTO itemPotencia)
        {
            SerieGraficoRpf grafico = new SerieGraficoRpf();
            StringBuilder regresion = new StringBuilder();
            StringBuilder superior = new StringBuilder();
            StringBuilder inferior = new StringBuilder();
            StringBuilder puntos = new StringBuilder();

            grafico.Indicador = 2;

            int count = list.Count;
            decimal frecuencia = list.Sum(x => x.FRECUENCIA) / count;
            decimal potencia = list.Sum(x => x.POTENCIA) / count;
            decimal estatismo = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.Estatismo);
            decimal frecNominal = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.FrecuenciaNominal);

            if (itemPotencia != null)
            {
                if (itemPotencia.POTENCIAMAX > 0)
                {
                    decimal potenciaMax = itemPotencia.POTENCIAMAX;

                    decimal a = frecuencia + estatismo * frecNominal * potencia / (100 * potenciaMax);
                    decimal b = -1 * estatismo * frecNominal / (100 * potenciaMax);

                    decimal potenciaMinima = list.Min(x => x.POTENCIA);
                    decimal potenciaMaxima = list.Max(x => x.POTENCIA);

                    int xCount = list.Count;
                    decimal intervalo = (potenciaMaxima - potenciaMinima) / xCount;

                    regresion.Append(Constantes.AperturaSerie);
                    superior.Append(Constantes.AperturaSerie);
                    inferior.Append(Constantes.AperturaSerie);
                    puntos.Append(Constantes.AperturaSerie);

                    decimal potenciaCalculada = 0;
                    decimal frecuenciaCalculada = 0;
                    int cumplimientoCount = 0;

                    for (int i = 0; i < xCount; i++)
                    {
                        potenciaCalculada = potenciaMinima + i * intervalo;
                        frecuenciaCalculada = a + b * potenciaCalculada;

                        regresion.Append(Constantes.AperturaSerie + potenciaCalculada.ToString() + Constantes.Coma +
                            frecuenciaCalculada.ToString() + Constantes.CierreSerie);
                        superior.Append(Constantes.AperturaSerie + potenciaCalculada.ToString() + Constantes.Coma +
                            (frecuenciaCalculada + ajuste).ToString() + Constantes.CierreSerie);
                        inferior.Append(Constantes.AperturaSerie + potenciaCalculada.ToString() + Constantes.Coma +
                            (frecuenciaCalculada - ajuste).ToString() + Constantes.CierreSerie);
                        puntos.Append(Constantes.AperturaSerie + list[i].POTENCIA.ToString() + Constantes.Coma +
                            list[i].FRECUENCIA.ToString() + Constantes.CierreSerie);

                        decimal frecuenciaComparacion = a + b * list[i].POTENCIA;

                        if (list[i].FRECUENCIA >= frecuenciaComparacion - ajuste && list[i].FRECUENCIA <= frecuenciaComparacion + ajuste)
                        {
                            cumplimientoCount++;
                        }

                        if (i < xCount - 1)
                        {
                            regresion.Append(Constantes.Coma);
                            superior.Append(Constantes.Coma);
                            inferior.Append(Constantes.Coma);
                            puntos.Append(Constantes.Coma);
                        }
                    }

                    regresion.Append(Constantes.CierreSerie);
                    superior.Append(Constantes.CierreSerie);
                    inferior.Append(Constantes.CierreSerie);
                    puntos.Append(Constantes.CierreSerie);

                    grafico.SerieRegresion = Constantes.AperturaSerie + regresion + Constantes.Coma + superior +
                        Constantes.Coma + inferior + Constantes.Coma + puntos + Constantes.CierreSerie;

                    decimal cumplimiento = (decimal)cumplimientoCount * 100 / (decimal)xCount;
                    grafico.PorcentajeCumplimiento = cumplimiento.ToString(Constantes.FormatoNumero);
                    grafico.IndCumplimiento = (cumplimiento >= (decimal)this.configuracion.ObtenerParametro(ValoresRPF.PorcentajeEvaluacion))
                        ? Constantes.SI : Constantes.NO;

                    grafico.Indicador = 1;
                }
            }

            return grafico;
        }

        /// <summary>
        /// Permite obtener el reporte de cumplimiento
        /// </summary>
        /// <param name="listConfiguracion"></param>
        /// <param name="listDatos"></param>
        /// <param name="ajuste"></param>
        /// <returns></returns>
        public List<ServicioRpfDTO> ObtenerReporte(List<ServicioRpfDTO> listConfiguracion, List<RegistrorpfDTO> listDatos,
            List<ServicioRpfDTO> listPotencia, decimal ajuste)
        {
            List<ServicioRpfDTO> listReporte = listConfiguracion;

            decimal porcentaje = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.PorcentajeEvaluacion);
            decimal estatismo = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.Estatismo);
            decimal frecNominal = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.FrecuenciaNominal);
            decimal porCumplimiento = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.PorcentajeEvaluacion);


            foreach (ServicioRpfDTO item in listReporte)
            {
                List<RegistrorpfDTO> list = listDatos.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).ToList();
                ServicioRpfDTO itemPotencia = listPotencia.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).FirstOrDefault();

                if (itemPotencia != null)
                {
                    if (itemPotencia.POTENCIAMAX > 0)
                    {
                        int count = list.Count;
                        decimal frecuencia = list.Sum(x => x.FRECUENCIA) / count;
                        decimal potencia = list.Sum(x => x.POTENCIA) / count;
                        decimal potenciaMax = itemPotencia.POTENCIAMAX;

                        decimal a = frecuencia + estatismo * frecNominal * potencia / (100 * potenciaMax);
                        decimal b = -1 * estatismo * frecNominal / (100 * potenciaMax);

                        int cumplimientoCount = 0;

                        for (int i = 0; i < count; i++)
                        {
                            decimal frecuenciaComparacion = a + b * list[i].POTENCIA;

                            if (list[i].FRECUENCIA >= frecuenciaComparacion - ajuste && list[i].FRECUENCIA <= frecuenciaComparacion + ajuste)
                            {
                                cumplimientoCount++;
                            }
                        }

                        decimal cumplimiento = (decimal)cumplimientoCount * 100 / (decimal)count;

                        item.PORCENTAJE = cumplimiento;
                        item.INDCUMPLIMIENTO = (cumplimiento >= porCumplimiento)
                            ? Constantes.TextoSI : Constantes.TextoNO;
                    }
                }
            }

            return listReporte;
        }


        /// <summary>
        /// Permite generar las gráficas para el reporte del cumplimiento
        /// </summary>
        /// <param name="listConfiguracion"></param>
        /// <param name="listDatos"></param>
        /// <param name="ajuste"></param>
        /// <returns></returns>
        public List<ServicioRpfDTO> ObtenerReporteWord(List<ServicioRpfDTO> listConfiguracion, List<RegistrorpfDTO> listDatos,
            List<ServicioRpfDTO> listPotencia, decimal ajuste)
        {
            List<ServicioRpfDTO> listReporte = listConfiguracion;

            decimal porcentaje = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.PorcentajeEvaluacion);
            decimal estatismo = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.Estatismo);
            decimal frecNominal = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.FrecuenciaNominal);
            decimal porCumplimiento = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.PorcentajeEvaluacion);


            foreach (ServicioRpfDTO item in listReporte)
            {
                List<RegistrorpfDTO> list = listDatos.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).ToList();
                ServicioRpfDTO itemPotencia = listPotencia.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).FirstOrDefault();

                if (itemPotencia != null)
                {
                    if (itemPotencia.POTENCIAMAX > 0)
                    {
                        int count = list.Count;
                        decimal frecuencia = list.Sum(x => x.FRECUENCIA) / count;
                        decimal potencia = list.Sum(x => x.POTENCIA) / count;
                        decimal potenciaMax = itemPotencia.POTENCIAMAX;

                        decimal a = frecuencia + estatismo * frecNominal * potencia / (100 * potenciaMax);
                        decimal b = -1 * estatismo * frecNominal / (100 * potenciaMax);

                        decimal potenciaMinima = list.Min(x => x.POTENCIA);
                        decimal potenciaMaxima = list.Max(x => x.POTENCIA);

                        int xCount = list.Count;
                        decimal intervalo = (potenciaMaxima - potenciaMinima) / xCount;

                        decimal potenciaCalculada = 0;
                        decimal frecuenciaCalculada = 0;
                        int cumplimientoCount = 0;

                        List<ServicioRpfSerie> listaSerie = new List<ServicioRpfSerie>();
                        List<ServicioRpfSerie> listaSuperior = new List<ServicioRpfSerie>();
                        List<ServicioRpfSerie> listaInferior = new List<ServicioRpfSerie>();
                        List<ServicioRpfSerie> listaPuntos = new List<ServicioRpfSerie>();

                        for (int i = 0; i < xCount; i++)
                        {
                            potenciaCalculada = potenciaMinima + i * intervalo;
                            frecuenciaCalculada = a + b * potenciaCalculada;

                            listaSerie.Add(new ServicioRpfSerie { Frecuencia = frecuenciaCalculada, Potencia = potenciaCalculada });
                            listaSuperior.Add(new ServicioRpfSerie { Frecuencia = frecuenciaCalculada + ajuste, Potencia = potenciaCalculada });
                            listaInferior.Add(new ServicioRpfSerie { Frecuencia = frecuenciaCalculada - ajuste, Potencia = potenciaCalculada });
                            listaPuntos.Add(new ServicioRpfSerie { Frecuencia = list[i].FRECUENCIA, Potencia = list[i].POTENCIA });

                            decimal frecuenciaComparacion = a + b * list[i].POTENCIA;

                            if (list[i].FRECUENCIA >= frecuenciaComparacion - ajuste && list[i].FRECUENCIA <= frecuenciaComparacion + ajuste)
                            {
                                cumplimientoCount++;
                            }
                        }

                        item.ListaSerie = listaSerie;
                        item.ListaSuperior = listaSuperior;
                        item.ListaInferior = listaInferior;
                        item.ListaPuntos = listaPuntos;

                        decimal cumplimiento = (decimal)cumplimientoCount * 100 / (decimal)count;

                        item.PORCENTAJE = cumplimiento;
                        item.INDCUMPLIMIENTO = (cumplimiento >= porCumplimiento)
                            ? Constantes.TextoSI : Constantes.TextoNO;
                    }
                }
            }

            return listReporte;
        }

        /// <summary>
        /// Permite obtener los puntos del gráfico de análisis ante fallas
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ajuste"></param>
        /// <param name="list"></param>
        public SerieGraficoRpf ObtenerGraficoFalla(decimal ra, List<RegistrorpfDTO> listDatos, int indice, List<decimal> sanjuan)
        {
            SerieGraficoRpf grafico = new SerieGraficoRpf();
            StringBuilder regresion = new StringBuilder();
            StringBuilder puntos = new StringBuilder();
            StringBuilder frecuencias = new StringBuilder();
            StringBuilder frecSanjuan = new StringBuilder();

            List<RegistrorpfDTO> list = listDatos.Skip(10).ToList();
            List<RegistrorpfDTO> listAnterior = listDatos.Take(10).ToList();

            int count = list.Count;
            int cumplimientoCount = 0;

            regresion.Append(Constantes.AperturaSerie);
            puntos.Append(Constantes.AperturaSerie);
            frecuencias.Append(Constantes.AperturaSerie);
            frecSanjuan.Append(Constantes.AperturaSerie);

            for (int i = 0; i < 10; i++)
            {
                puntos.Append(Constantes.AperturaSerie + (i - 10).ToString() + Constantes.Coma +
                    listAnterior[i].POTENCIA.ToString() + Constantes.CierreSerie);
                frecuencias.Append(Constantes.AperturaSerie + (i - 10).ToString() + Constantes.Coma +
                    listAnterior[i].FRECUENCIA.ToString() + Constantes.CierreSerie);
                frecSanjuan.Append(Constantes.AperturaSerie + (i - 10).ToString() + Constantes.Coma +
                    sanjuan[i].ToString() + Constantes.CierreSerie);
                puntos.Append(Constantes.Coma);
                frecuencias.Append(Constantes.Coma);
                frecSanjuan.Append(Constantes.Coma);
            }

            for (int i = 0; i <= indice; i++)
            {
                decimal valor = 0;

                if (i < 5)
                {
                    //valor = list[0].POTENCIA;     
                    valor = 0;
                }
                if (i >= 5 && i < 30)
                {
                    valor = list[0].POTENCIA + ra * (i - 5.0M) / 25.0M;
                }
                if (i >= 30 && i <= 60)
                {
                    valor = list[0].POTENCIA + ra;
                }
                if (i > 60)
                {
                    valor = list[0].POTENCIA + ra * 0.85M;
                }


                regresion.Append(Constantes.AperturaSerie + i.ToString() + Constantes.Coma +
                            valor.ToString() + Constantes.CierreSerie);
                puntos.Append(Constantes.AperturaSerie + i.ToString() + Constantes.Coma +
                    list[i].POTENCIA.ToString() + Constantes.CierreSerie);
                frecuencias.Append(Constantes.AperturaSerie + i.ToString() + Constantes.Coma +
                    list[i].FRECUENCIA.ToString() + Constantes.CierreSerie);
                frecSanjuan.Append(Constantes.AperturaSerie + i.ToString() + Constantes.Coma +
                    sanjuan[i + 10].ToString() + Constantes.CierreSerie);

                if (list[i].POTENCIA >= valor)
                {
                    cumplimientoCount++;
                }

                if (i < indice)
                {
                    regresion.Append(Constantes.Coma);
                    puntos.Append(Constantes.Coma);
                    frecuencias.Append(Constantes.Coma);
                    frecSanjuan.Append(Constantes.Coma);
                }
            }

            regresion.Append(Constantes.CierreSerie);
            puntos.Append(Constantes.CierreSerie);
            frecuencias.Append(Constantes.CierreSerie);
            frecSanjuan.Append(Constantes.CierreSerie);

            grafico.SerieRegresion = Constantes.AperturaSerie + regresion + Constantes.Coma + puntos + Constantes.Coma + frecuencias +
                Constantes.Coma + frecSanjuan + Constantes.CierreSerie;

            decimal cumplimiento = (decimal)cumplimientoCount * 100 / (decimal)(indice + 1);
            grafico.PorcentajeCumplimiento = cumplimiento.ToString(Constantes.FormatoNumero);
            grafico.IndCumplimiento = (cumplimiento >= (decimal)this.configuracion.ObtenerParametro(ValoresRPF.PorcentajeCumplimientoFalla))
                ? Constantes.SI : Constantes.NO;

            grafico.Indicador = 1;

            return grafico;
        }

        /// <summary>
        /// Permite obtener el reporte de cumplimiento en análisis ante ocurrencia de fallas
        /// </summary>
        /// <param name="listConfiguracion"></param>
        /// <param name="listDatos"></param>
        /// <param name="listPotencias"></param>
        /// <param name="indiceFrecuencia"></param>
        /// <returns></returns>
        public List<ServicioRpfDTO> ObtenerReporteFalla(List<ServicioRpfDTO> listConfiguracion, List<RegistrorpfDTO> listDatos,
            List<ServicioRpfDTO> listPotencias, int indiceFrecuencia)
        {
            List<ServicioRpfDTO> listReporte = listConfiguracion;
            decimal porcentaje = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.PorcentajeCumplimientoFalla);

            foreach (ServicioRpfDTO item in listConfiguracion)
            {
                List<RegistrorpfDTO> list = listDatos.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).OrderBy(x => x.FECHAHORA).Skip(10).ToList();
                ServicioRpfDTO itemPotencia = listPotencias.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).FirstOrDefault();

                if (list.Count >= 60)
                {
                    if (list[0].FRECUENCIA > 0)
                    {

                        if (itemPotencia != null)
                        {

                            int indFrecuencia = indiceFrecuencia;
                            bool isTV = (new RpfHelper()).IsTurboVapor(item.PTOMEDICODI);
                            if (isTV && indiceFrecuencia > 60) indFrecuencia = 60;

                            decimal potenciaMax = itemPotencia.POTENCIAMAX;
                            decimal frec30Seg = list[30].FRECUENCIA;
                            //decimal potenciaGenerada = list[0].POTENCIA;                           


                            List<RegistrorpfDTO> listFrec = listDatos.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).OrderBy(x => x.FECHAHORA).Take(10).ToList();
                            decimal frecFalla = 0;
                            int nroSegundos = (int)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.SegundosFrecuenciaFalla);
                            for (int i = 0; i < nroSegundos; i++)
                            {
                                frecFalla = frecFalla + listFrec[9 - i].FRECUENCIA;
                            }
                            frecFalla = frecFalla / nroSegundos;


                            decimal potenciaGenerada = (list[0].POTENCIA + listFrec[9].POTENCIA + listFrec[8].POTENCIA +
                                listFrec[7].POTENCIA + listFrec[6].POTENCIA) / 5M;

                            if (frecFalla != 0)
                            {

                                decimal ra = (new ConfiguracionRPF()).ObtenerValorRA(potenciaMax, frec30Seg, frecFalla, potenciaGenerada);

                                decimal valor = 0;
                                int cumplimientoCount = 0;

                                for (int i = 0; i <= indFrecuencia; i++)
                                {
                                    if (i < 5)
                                    {
                                        //valor = list[0].POTENCIA;
                                        valor = 0;
                                    }
                                    if (i >= 5 && i < 30)
                                    {
                                        valor = list[0].POTENCIA + ra * (i - 5.0M) / 25.0M;
                                    }
                                    if (i >= 30 && i <= 60)
                                    {
                                        valor = list[0].POTENCIA + ra;
                                    }
                                    if (i > 60)
                                    {
                                        valor = list[0].POTENCIA + ra * 0.85M;
                                    }

                                    if (list[i].POTENCIA >= valor)
                                    {
                                        cumplimientoCount++;
                                    }
                                }

                                decimal cumplimiento = (decimal)cumplimientoCount * 100 / (decimal)(indFrecuencia + 1);

                                item.PORCENTAJE = cumplimiento;
                                item.INDCUMPLIMIENTO = (cumplimiento >= porcentaje)
                                    ? Constantes.TextoSI : Constantes.TextoNO;
                                item.INDICADORCARGA = Constantes.SI;
                            }
                            else
                            {
                                item.PORCENTAJE = 0;
                                item.INDCUMPLIMIENTO = Constantes.TextoNO;
                                item.INDICADORCARGA = Constantes.NO;
                            }
                        }
                    }
                    else
                    {
                        item.PORCENTAJE = 0;
                        item.INDCUMPLIMIENTO = Constantes.TextoNO;
                        item.INDICADORCARGA = Constantes.NO;
                    }
                }
                else
                {
                    item.PORCENTAJE = 0;
                    item.INDCUMPLIMIENTO = Constantes.TextoNO;
                    item.INDICADORCARGA = Constantes.NO;
                }
            }

            return listReporte;
        }


        /// <summary>
        /// Permite generar las gráficas para el reporte del cumplimiento ante fallas
        /// </summary>
        /// <param name="listConfiguracion"></param>
        /// <param name="listDatos"></param>
        /// <param name="listPotencia"></param>
        /// <returns></returns>
        public List<ServicioRpfDTO> ObtenerReporteWordRPF(List<ServicioRpfDTO> listConfiguracion, List<RegistrorpfDTO> listDatos,
            List<ServicioRpfDTO> listPotencias, int indiceFrecuencia, List<decimal> frecuencias)
        {
            List<ServicioRpfDTO> listReporte = listConfiguracion;

            decimal porcentaje = (decimal)this.configuracion.ObtenerParametro(ValoresRPF.PorcentajeCumplimientoFalla);

            foreach (ServicioRpfDTO item in listReporte)
            {
                List<RegistrorpfDTO> list = listDatos.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).OrderBy(x => x.FECHAHORA).Skip(10).ToList();
                List<RegistrorpfDTO> listInicial = listDatos.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).OrderBy(x => x.FECHAHORA).Take(10).ToList();
                ServicioRpfDTO itemPotencia = listPotencias.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).FirstOrDefault();

                if (list.Count >= 60)
                {
                    if (list[0].FRECUENCIA > 0)
                    {
                        if (itemPotencia != null)
                        {
                            int indFrecuencia = indiceFrecuencia;
                            bool isTV = (new RpfHelper()).IsTurboVapor(item.PTOMEDICODI);
                            if (isTV && indiceFrecuencia > 60) indFrecuencia = 60;

                            decimal potenciaMax = itemPotencia.POTENCIAMAX;
                            decimal frec30Seg = list[30].FRECUENCIA;
                            //decimal potenciaGenerada = list[0].POTENCIA;

                            List<RegistrorpfDTO> listFrec = listDatos.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).OrderBy(x => x.FECHAHORA).Take(10).ToList();
                            decimal frecFalla = 0;
                            int nroSegundos = (int)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.SegundosFrecuenciaFalla);
                            for (int i = 0; i < nroSegundos; i++)
                            {
                                frecFalla = frecFalla + listFrec[9 - i].FRECUENCIA;
                            }
                            frecFalla = frecFalla / nroSegundos;

                            decimal potenciaGenerada = (list[0].POTENCIA + listFrec[9].POTENCIA + listFrec[8].POTENCIA +
                                listFrec[7].POTENCIA + listFrec[6].POTENCIA) / 5M;

                            if (frecFalla != 0)
                            {
                                decimal ra = (new ConfiguracionRPF()).ObtenerValorRA(potenciaMax, frec30Seg, frecFalla, potenciaGenerada);

                                decimal valor = 0;
                                int cumplimientoCount = 0;

                                List<ServicioRpfSerie> listaSerie = new List<ServicioRpfSerie>();
                                List<ServicioRpfSerie> listaPotencias = new List<ServicioRpfSerie>();
                                List<ServicioRpfSerie> listaFrecuencias = new List<ServicioRpfSerie>();
                                List<ServicioRpfSerie> listSanJuan = new List<ServicioRpfSerie>();

                                for (int i = 0; i < 10; i++)
                                {
                                    listaFrecuencias.Add(new ServicioRpfSerie { Valor = listInicial[i].FRECUENCIA, Segundo = i - 10 });
                                    listaPotencias.Add(new ServicioRpfSerie { Valor = listInicial[i].POTENCIA, Segundo = i - 10 });
                                    listSanJuan.Add(new ServicioRpfSerie { Valor = frecuencias[i], Segundo = i - 10 });
                                }

                                for (int i = 0; i <= indFrecuencia; i++)
                                {
                                    if (i < 5)
                                    {
                                        //valor = list[0].POTENCIA;
                                        valor = 0;
                                    }
                                    if (i >= 5 && i < 30)
                                    {
                                        valor = list[0].POTENCIA + ra * (i - 5.0M) / 25.0M;
                                    }
                                    if (i >= 30 && i <= 60)
                                    {
                                        valor = list[0].POTENCIA + ra;
                                    }
                                    if (i > 60)
                                    {
                                        valor = list[0].POTENCIA + ra * 0.85M;
                                    }

                                    if (list[i].POTENCIA >= valor)
                                    {
                                        cumplimientoCount++;
                                    }

                                    if (i >= 5)
                                    {
                                        listaSerie.Add(new ServicioRpfSerie { Valor = valor, Segundo = i });
                                    }

                                    listaPotencias.Add(new ServicioRpfSerie { Valor = list[i].POTENCIA, Segundo = i });
                                    listaFrecuencias.Add(new ServicioRpfSerie { Valor = list[i].FRECUENCIA, Segundo = i });
                                    listSanJuan.Add(new ServicioRpfSerie { Valor = frecuencias[i + 10], Segundo = i });

                                }

                                item.ListaArea = listaSerie;
                                item.ListaPotencia = listaPotencias;
                                item.ListaFrecuencia = listaFrecuencias;
                                item.ListaSanJuan = listSanJuan;
                                item.ValorRA = ra;
                                decimal cumplimiento = (decimal)cumplimientoCount * 100 / (decimal)(indFrecuencia + 1);

                                item.PORCENTAJE = cumplimiento;
                                item.INDCUMPLIMIENTO = (cumplimiento >= porcentaje)
                                    ? Constantes.TextoSI : Constantes.TextoNO;

                                item.INDICADORCARGA = Constantes.SI;
                            }
                            else
                            {
                                item.PORCENTAJE = 0;
                                item.INDCUMPLIMIENTO = Constantes.TextoNO;

                                item.INDICADORCARGA = Constantes.SI;
                            }
                        }
                    }
                    else
                    {
                        item.INDICADORCARGA = Constantes.NO;
                    }
                }
                else
                {
                    item.INDICADORCARGA = Constantes.NO;
                }
            }

            return listReporte;
        }


        /// <summary>
        /// Permite verificar si un punto es de un equipo turbovapor
        /// </summary>
        /// <param name="ptoMediCodi"></param>
        /// <returns></returns>
        public bool IsTurboVapor(int ptoMediCodi)
        {
            int[] ids = { 420, 471, 466, 467, 437, 47, 488, 367, 48, 351, 368, 346, 49, 347 };
            return ids.Contains(ptoMediCodi);
        }
    }


    /// <summary>
    /// Clase para menejar los datos de la serie
    /// </summary>
    public class SerieGraficoRpf
    {
        public string SerieRegresion { get; set; }
        public string IndCumplimiento { get; set; }
        public string PorcentajeCumplimiento { get; set; }
        public int Indicador { get; set; }
    }

}