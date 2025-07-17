using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Globalization;
using log4net.Repository.Hierarchy;
using log4net;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.ServicioRPF;
using System.Data;
using COES.Servicios.Aplicacion.ReportesMedicion.Helper;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using Newtonsoft.Json;
using COES.Framework.Base.Tools;
using OfficeOpenXml.Drawing;
using COES.Servicios.Aplicacion.Scada;
using System.Reflection;
using iTextSharp.text.pdf.qrcode;
using COES.Servicios.Aplicacion.IEOD;
using MathNet.Numerics.Interpolation;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using COES.Servicios.Aplicacion.DemandaPO.Helper;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;

namespace COES.Servicios.Aplicacion.DPODemanda
{
    /// <summary>
    /// Modulo de Demandas
    /// </summary>
    public class DemandaPOAppServicio : AppServicioBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DemandaPOAppServicio));

        #region Módulo - [DEF.Anexo B] Generación de info i96(15min) desde info i1440(1min) 
        /// <summary>
        /// Rutina que permite Probar el filtrado de información TNA en formato de 15 min
        /// </summary>        
        /// <param name="regPunto">Punto de medición a ejecutar</param>
        /// <param name="regFecha">Fecha de ejecución del proceso (a partir de)</param>
        /// <param name="path">Dirección temporal para el reporte</param>
        /// <param name="length">longitud de la ventana para el alisado_gaussiano</param>
        /// <param name="std">desvió estándar  para el alisado_gaussiano</param>
        public string PruebaFiltradoInfSco(int regPunto,
            string regFecha, string path,
            int length = 30, double std = 2.5)
        {
            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);

            string nomTabla = ConstantesDpo.tablaEstimadorRaw
                + parseFecha.ToString(ConstantesDpo.FormatoAnioMes);

            //Setea los parámetros iniciales
            DateTime regIni = new DateTime(parseFecha.Year,
                parseFecha.Month, 1, 0, 0, 0);
            DateTime regFin = regIni.AddMonths(1);

            //Obtiene los datos filtrados
            List<DpoMedicion1440> datosSco = this.DatosDiaTipicoSCO(
                nomTabla, regPunto,
                regIni.ToString(ConstantesProdem.FormatoFecha),
                regFin.ToString(ConstantesProdem.FormatoFecha));

            //Convierte los datos en formato de 96 intervalos (15min)
            List<DpoDemandaScoDTO> medicion96 = UtilDpo.DarFormato96(datosSco);

            //Proceso alisado gaussiano
            List<DpoDemandaScoDTO> medicionCorregida = this.ProcesoAlisadoGaussiano(
                medicion96, length, std);

            //Creación de reportes
            medicionCorregida = medicionCorregida
                .Where(x => x.Medifecha == parseFecha)
                .OrderBy(x => x.Prnvarcodi)
                .ToList();

            List<PrnFormatoExcel> libroReporte = new List<PrnFormatoExcel>();
            //Hoja Resultado
            string[] intervalos = UtilDpo.GenerarIntervalos(ConstantesDpo.Itv15min);
            List<string[]> listaContenido = new List<string[]>();
            listaContenido.Add(intervalos);
            foreach (DpoDemandaScoDTO e in medicionCorregida)
            {
                string[] arryMedicion = new string[ConstantesDpo.Itv15min];
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    var val = (decimal)e.GetType()
                        .GetProperty($"H{i + 1}")
                        .GetValue(e);
                    arryMedicion[i] = val.ToString();
                }

                listaContenido.Add(arryMedicion);
            }

            List<string> cabecera = new List<string>() { "Hora" };
            foreach (DpoDemandaScoDTO entidad in medicionCorregida)
            {
                string str = "ESTADO";
                if (entidad.Prnvarcodi == ConstantesProdem.GeneradorPotActivaMW)
                    str = "GEN-POT.ACTIVA MW";
                if (entidad.Prnvarcodi == ConstantesProdem.GeneradorPotReactivaMVAR)
                    str = "GEN-POT.REACTIVA MVAR";
                if (entidad.Prnvarcodi == ConstantesProdem.LineaPotActivaMW)
                    str = "LIN-POT.ACTIVA MW";
                if (entidad.Prnvarcodi == ConstantesProdem.LineaPotReactivaMVAR)
                    str = "LIN-POT.REACTIVA MVAR";
                if (entidad.Prnvarcodi == ConstantesProdem.LineaPotAparenteMVA)
                    str = "LIN-POT.APARENTE MVA";
                if (entidad.Prnvarcodi == ConstantesProdem.LineaPerdidasMW)
                    str = "LIN-PERDIDAS MW";
                if (entidad.Prnvarcodi == ConstantesProdem.LineaCargaMaxima)
                    str = "LIN-CARGAMAX";
                if (entidad.Prnvarcodi == ConstantesProdem.TransPotActivaMW)
                    str = "TRANS-POT.ACTIVA MW";
                if (entidad.Prnvarcodi == ConstantesProdem.TransPotReactivaMVAR)
                    str = "TRANS-POT.REACTIVA MVAR";
                if (entidad.Prnvarcodi == ConstantesProdem.TransPotAparenteMVA)
                    str = "TRANS-POT.APARENTE MVA";
                if (entidad.Prnvarcodi == ConstantesProdem.TransCargaMaxima)
                    str = "TRANS-CARGAMAX";
                if (entidad.Prnvarcodi == ConstantesProdem.TransPerdidasMW)
                    str = "TRANS-PERDIDAS MW";
                if (entidad.Prnvarcodi == ConstantesProdem.BarraTensionKv)
                    str = "BAR-TENSION KV";
                if (entidad.Prnvarcodi == ConstantesProdem.BarraAngulo)
                    str = "BAR-ANGULO";
                if (entidad.Prnvarcodi == ConstantesProdem.BarraDemActivaMW)
                    str = "BAR-DEM.ACTIVA MW";
                if (entidad.Prnvarcodi == ConstantesProdem.BarraDemReactivaMVAR)
                    str = "BAR-DEM.REACTIVA MVAR";
                if (entidad.Prnvarcodi == ConstantesProdem.CargaPotActivaMW)
                    str = "CARG-POT.ACTIVA MW";
                if (entidad.Prnvarcodi == ConstantesProdem.CargaPotReactivaMVAR)
                    str = "CARG-POT.REACTIVA MVAR";
                if (entidad.Prnvarcodi == ConstantesProdem.ShuntPotReactivaMVAR)
                    str = "SHUN-POT.REACTIVA MVAR";
                cabecera.Add(str);
            }

            PrnFormatoExcel formato = new PrnFormatoExcel();
            formato.NombreLibro = "Resultado";
            formato.Titulo = "Reporte Filtrado de Información TNA";
            formato.Cabecera = cabecera.ToArray();
            formato.Contenido = listaContenido.ToArray();
            formato.AnchoColumnas = Enumerable
                .Repeat(20, cabecera.Count)
                .ToArray();

            libroReporte.Add(formato);

            //Hoja Datos            
            listaContenido = new List<string[]>();
            intervalos = UtilDpo.GenerarIntervalos(ConstantesDpo.Itv1min);
            listaContenido.Add(intervalos);
            foreach (DpoMedicion1440 e in datosSco)
            {
                string[] med = Array
                    .ConvertAll(e.Medicion, x => x.ToString());
                listaContenido.Add(med);
            }

            cabecera = new List<string>() { "Hora" };
            foreach (DpoMedicion1440 entidad in datosSco)
            {
                string str = $"{entidad.Medifecha.ToString(ConstantesDpo.FormatoFecha)}\n";
                if (entidad.Tipoinfo == ConstantesProdem.GeneradorPotActivaMW)
                    str += "GEN-POT.ACTIVA MW";
                else if (entidad.Tipoinfo == ConstantesProdem.GeneradorPotReactivaMVAR)
                    str += "GEN-POT.REACTIVA MVAR";
                else if (entidad.Tipoinfo == ConstantesProdem.LineaPotActivaMW)
                    str += "LIN-POT.ACTIVA MW";
                else if (entidad.Tipoinfo == ConstantesProdem.LineaPotReactivaMVAR)
                    str += "LIN-POT.REACTIVA MVAR";
                else if (entidad.Tipoinfo == ConstantesProdem.LineaPotAparenteMVA)
                    str += "LIN-POT.APARENTE MVA";
                else if (entidad.Tipoinfo == ConstantesProdem.LineaPerdidasMW)
                    str += "LIN-PERDIDAS MW";
                else if (entidad.Tipoinfo == ConstantesProdem.LineaCargaMaxima)
                    str += "LIN-CARGAMAX";
                else if (entidad.Tipoinfo == ConstantesProdem.TransPotActivaMW)
                    str += "TRANS-POT.ACTIVA MW";
                else if (entidad.Tipoinfo == ConstantesProdem.TransPotReactivaMVAR)
                    str += "TRANS-POT.REACTIVA MVAR";
                else if (entidad.Tipoinfo == ConstantesProdem.TransPotAparenteMVA)
                    str += "TRANS-POT.APARENTE MVA";
                else if (entidad.Tipoinfo == ConstantesProdem.TransCargaMaxima)
                    str += "TRANS-CARGAMAX";
                else if (entidad.Tipoinfo == ConstantesProdem.TransPerdidasMW)
                    str += "TRANS-PERDIDAS MW";
                else if (entidad.Tipoinfo == ConstantesProdem.BarraTensionKv)
                    str += "BAR-TENSION KV";
                else if (entidad.Tipoinfo == ConstantesProdem.BarraAngulo)
                    str += "BAR-ANGULO";
                else if (entidad.Tipoinfo == ConstantesProdem.BarraDemActivaMW)
                    str += "BAR-DEM.ACTIVA MW";
                else if (entidad.Tipoinfo == ConstantesProdem.BarraDemReactivaMVAR)
                    str += "BAR-DEM.REACTIVA MVAR";
                else if (entidad.Tipoinfo == ConstantesProdem.CargaPotActivaMW)
                    str += "CARG-POT.ACTIVA MW";
                else if (entidad.Tipoinfo == ConstantesProdem.CargaPotReactivaMVAR)
                    str += "CARG-POT.REACTIVA MVAR";
                else if (entidad.Tipoinfo == ConstantesProdem.ShuntPotReactivaMVAR)
                    str += "SHUN-POT.REACTIVA MVAR";
                else
                    str += "ESTADO";
                cabecera.Add(str);
            }
            formato = new PrnFormatoExcel();
            formato.NombreLibro = "Datos entrada";
            formato.Titulo = "Datos de entrada";
            formato.Cabecera = cabecera.ToArray();
            formato.Contenido = listaContenido.ToArray();
            formato.AnchoColumnas = Enumerable
                .Repeat(20, cabecera.Count)
                .ToArray();
            libroReporte.Add(formato);

            return this.ExportarReporteConLibros(libroReporte, path, "Reporte_Filtrado");
        }

        /// <summary>
        /// Rutina que permite aplicar el Alisado Gaussiano a la información TNA en formato de 15 min
        /// </summary>        
        /// <param name="arryMedicion">Señal a suavisar por el Alisado Gaussiano</param>
        /// <param name="length">longitud de la ventana para el alisado_gaussiano</param>
        /// <param name="std">desvió estándar  para el alisado_gaussiano</param>
        public void Alissado_Gaussiano(decimal[] arryMedicion, int length, double std)
        {
            // Aplicar convolución con el kernel gaussiano
            double[] kernel = GaussianWindow(length, std);

            double[] signal = new double[arryMedicion.Length];
            for (int i = 0; i < arryMedicion.Length; i++)
            {
                signal[i] = Convert.ToDouble(arryMedicion[i]);
            }

            // Aplicar convolución con el kernel gaussiano
            int halfLength = length / 2;
            for (int i = halfLength; i < signal.Length - halfLength; i++)
            {
                double convolution = 0;
                for (int j = 0; j < length; j++)
                {
                    convolution += signal[i - halfLength + j] * kernel[j];
                }
                arryMedicion[i] = (decimal)convolution;
            }

            // Copiar los resultados a un arreglo de salida
            for (int i = 0; i < halfLength; i++)
            {
                arryMedicion[i] = Math.Round((decimal)signal[i], 5);
            }
            for (int i = signal.Length - halfLength; i < signal.Length; i++)
            {
                arryMedicion[i] = Math.Round((decimal)signal[i], 5);
            }
        }

        /// <summary>
        /// Devuelve una función de ventana de Gauss -> Kernel
        /// </summary>        
        /// <param name="length">longitud de la ventana para el alisado_gaussiano</param>
        /// <param name="std">desvió estándar  para el alisado_gaussiano</param>
        public double[] GaussianWindow(int length, double std)
        {
            double[] window = new double[length];
            double sum = 0;

            for (int i = 0; i < length; i++)
            {
                double x = (i - (length - 1) / 2.0) / (std * (length - 1) / 2.0);
                window[i] = Math.Exp(-0.5 * x * x);
                sum += window[i];
            }

            for (int i = 0; i < length; i++)
            {
                window[i] /= sum;
            }

            return window;
        }

        /// <summary>
        /// Rutina que convierte la información TNA en formato de 15 min
        /// Frecuencia: mensual
        /// </summary>        
        /// <param name="regFecha">Fecha de ejecución del proceso (a partir de)</param>
        /// <param name="tipoFecha">Tipo de fecha a ejecutar (día o mes)</param>
        /// <param name="selPunto">Punto de medición a ejecutar (-1 = todos)</param>
        /// <param name="length">Longitud de la ventana para el alisado_gaussiano</param>
        /// <param name="std">Desvió estándar  para el alisado_gaussiano</param>
        /// <exception cref="Exception"></exception>
        public void FiltradoInformacionTNA(DateTime regFecha,
            string tipoFecha, int selPunto, int length = 30,
            double std = 2.5)
        {
            //Parametros
            string parseFecIni = string.Empty;
            string parseFecFin = string.Empty;

            //Tabla correspondiente para el registro
            string nomTabla = ConstantesDpo.tablaEstimadorRaw
                + regFecha.ToString(ConstantesDpo.FormatoAnioMes);

            if (tipoFecha == "dia")
            {
                parseFecIni = regFecha.ToString(ConstantesDpo.FormatoFecha);
                parseFecFin = regFecha.ToString(ConstantesDpo.FormatoFecha);
            }
            else if (tipoFecha == "mes")
            {
                DateTime fecIni = new DateTime(regFecha.Year,
                    regFecha.Month, 1, 0, 0, 0);
                DateTime fecFin = fecIni.AddMonths(1);

                parseFecIni = fecIni.ToString(ConstantesDpo.FormatoFecha);
                parseFecFin = fecFin.ToString(ConstantesDpo.FormatoFecha);
            }

            //Obtención de puntos de medición
            List<MePtomedicionDTO> entPuntos = this.ListPtomedicionSco();

            List<int> puntosMedicion = entPuntos
                .Select(x => x.Ptomedicodi)
                .Distinct()
                .ToList();

            //Selecciona solo el punto solicitado
            if (selPunto != -1)
            {
                puntosMedicion = puntosMedicion.Where(x => x == selPunto).ToList();
            }
 
            foreach (int punto in puntosMedicion)
            {
                //Obtención de datos filtrados
                List<DpoMedicion1440> datosSco = this.DatosDiaTipicoSCO(
                    nomTabla, punto,
                    parseFecIni, parseFecFin);

                //Conversión a formato de 15 min
                List<DpoDemandaScoDTO> medicion96 = UtilDpo.DarFormato96(datosSco);

                //Proceso alisado gaussiano
                List<DpoDemandaScoDTO> medicionCorregida = this.ProcesoAlisadoGaussiano(
                    medicion96, length, std);

                //Registra los datos 
                try
                {
                    //Elimina registros previos
                    this.DeleteRangoFechaDpoDemandaSco(
                        punto, parseFecIni,
                        parseFecFin);

                    //Registro de información    
                    this.BulkInsertDpoDemandaSco(medicionCorregida,
                    ConstantesDpo.NomTablaDpoDemandaSco);
                    Logger.Info("[Srvc:376] FiltradoInformacionTNA: BulkInsertDpoDemandaSco: [" + punto + "] " + parseFecIni + " al " + parseFecFin);
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Devuelve un grupo de registros de la tabla DPO_ESTIMADOERAW por rango de fechas
        /// </summary>
        /// <param name="fuente">Identificador de la fuente de información a buscar</param>
        /// <param name="fecIni">Fecha inicio del rango de busqueda</param>
        /// <param name="fecFin">Fecha fin del rango de busqueda</param>
        /// <returns></returns>
        public List<DpoEstimadorRawDTO> ObtenerPorRangoFuente(
            int fuente, string fecIni,
            string fecFin)
        {
            return FactorySic.GetDpoEstimadorRawRepository()
                .ObtenerPorRangoFuente(fuente, fecIni, fecFin);
        }

        /// <summary>
        /// Devuelve los datos SPL segun días típicos
        /// </summary>
        /// <param name="nombreTabla">Nombre de la tabla a consultar</param>
        /// <param name="idPunto">Identificador del</param>
        /// <param name="fecIni">Fecha inicio del rango de busqueda</param>
        /// <param name="fecFin">Fecha fin del rango de busqueda</param>
        /// <returns></returns>
        public List<DpoMedicion1440> DatosDiaTipicoSPL(
            string nombreTabla, int idPunto,
            string fecIni, string fecFin)
        {
            List<DpoMedicion1440> entities = new List<DpoMedicion1440>();

            List<DpoEstimadorRawDTO> datosBase = this.ObtenerDatosTnaPorPtoMedicion(
                nombreTabla, ConstantesDpo.DporawfuenteIeod,
                idPunto);

            List<int> puntosMedicion = datosBase
                .Select(x => x.Ptomedicodi)
                .Distinct()
                .ToList();

            DateTime parseFecIni = DateTime.ParseExact(fecIni,
                ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);
            DateTime parseFecFin = DateTime.ParseExact(fecFin,
                ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);

            //Se remueven los días feriados            
            List<DateTime> feriados = this.ObtenerFeriadosSpl();
            datosBase = datosBase
                .Where(x => !(feriados.Contains(x.Dporawfecha)))
                .ToList();

            //Conversión a formato i1440
            List<DpoMedicion1440> datosMedicion = UtilDpo
                .DarFormato1440(datosBase,
                parseFecIni,
                parseFecFin);

            //Corrección de intervalos faltantes
            foreach (int pto in puntosMedicion)
            {
                List<DpoMedicion1440> datosPunto = datosMedicion
                    .Where(x => x.Ptomedicodi == pto)
                    .ToList();

                List<int> tipoInformacion = datosPunto
                    .Select(x => x.Tipoinfo)
                    .Distinct()
                    .ToList();

                foreach (int tipoInfo in tipoInformacion)
                {
                    List<DpoMedicion1440> medPunto = datosPunto
                        .Where(x => x.Tipoinfo == tipoInfo)
                        .ToList();

                    DpoMedDiaTipicoModel modelo = UtilDpo
                        .MedicionesxDiaTipico(medPunto);

                    //Obtención de promedios
                    decimal[] promLunes = UtilDpo
                        .PromedioSPL(modelo.datosLunes);
                    decimal[] promSabado = UtilDpo
                        .PromedioSPL(modelo.datosSabado);
                    decimal[] promDomingo = UtilDpo
                        .PromedioSPL(modelo.datosDomingo);
                    decimal[] promOtros = UtilDpo
                        .PromedioSPL(modelo.datosOtros);

                    //Corrección de intervalos faltantes
                    foreach (DpoMedicion1440 med in medPunto)
                    {
                        int diaSemana = (int)med.Medifecha.DayOfWeek;
                        decimal[] promedio = new decimal[ConstantesDpo.Itv1min];

                        if (diaSemana == (int)DayOfWeek.Monday)
                            promedio = promLunes;
                        else if (diaSemana == (int)DayOfWeek.Saturday)
                            promedio = promSabado;
                        else if (diaSemana == (int)DayOfWeek.Sunday)
                            promedio = promDomingo;
                        else
                            promedio = promOtros;

                        int i = 0;
                        while (i < ConstantesDpo.Itv1min)
                        {
                            decimal valor = med.Medicion[i];
                            if (valor == 0) med.Medicion[i] = promedio[i];
                            i++;
                        }
                    }

                    entities.AddRange(medPunto);
                }
            }

            return entities;
        }

        /// <summary>
        /// Devuelve los datos SCO segun días típicos corregidos
        /// </summary>
        /// <param name="nombreTabla">Nombre de la tabla a consultar</param>
        /// <param name="idPunto">Identificador de un punto de medición</param>
        /// <param name="fecIni">Fecha inicio del rango de busqueda</param>
        /// <param name="fecFin">Fecha fin del rango de busqueda</param>
        /// <returns></returns>
        public List<DpoMedicion1440> DatosDiaTipicoSCO(
            string nombreTabla, int idPunto,
            string fecIni, string fecFin)
        {
            List<DpoMedicion1440> entities = new List<DpoMedicion1440>();

            DateTime parseFecIni = DateTime.ParseExact(fecIni,
                ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);
            DateTime parseFecFin = DateTime.ParseExact(fecFin,
                ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);

            //Obtiene datos TNA por cada punto de medición
            List<DpoEstimadorRawDTO> datosBase = this.ObtenerDatosTnaPorPtoMedicion(
                nombreTabla, ConstantesDpo.DporawfuenteIeod,
                idPunto);

            //Elimina datos que corresponden a un día feriado
            List<DateTime> feriados = this.ObtenerFeriadosSco();
            datosBase = datosBase
                .Where(x => !(feriados.Contains(x.Dporawfecha)))
                .ToList();

            //Conversión a formato de 1440 intervalos (1min)
            List<DpoMedicion1440> medicion1440 = UtilDpo.DarFormato1440(
                datosBase, parseFecIni,
                parseFecFin);

            //Obtención de tipos de información
            List<int> tipoInformacion = this.ListaVariablesUnidadTna();

            foreach (int tipo in tipoInformacion)
            {
                //Obtención de mediciones por tipo de información
                //(variable unidad TNA)
                List<DpoMedicion1440> medicionTipo = medicion1440
                        .Where(x => x.Tipoinfo == tipo)
                        .ToList();

                if (medicionTipo.Count == 0) continue;

                //Obtención de medición promedio con los datos de las mediciones
                decimal[] medicionPromedio = UtilDpo.PromedioSCO(medicionTipo);

                //Corrección de intervalos faltantes
                this.CorreccionIntervalosFaltantesSCO(
                    medicionTipo, medicionPromedio);

                entities.AddRange(medicionTipo);
            }

            return entities;
        }

        /// <summary>
        /// Corrige los vacios en un grupo de mediciones SCO agrupadas por tipo
        /// </summary>
        /// <param name="datosMediciones">Datos de las mediciones a corregir</param>
        /// <param name="promedioMedicion">Datos de la medición promedio</param>
        public void CorreccionIntervalosFaltantesSCO(
            List<DpoMedicion1440> datosMediciones,
            decimal[] promedioMedicion)
        {
            foreach (DpoMedicion1440 dato in datosMediciones)
            {
                if (dato.Medicion.Sum() == 0)
                {
                    dato.Medicion = promedioMedicion;
                    continue;
                }

                //Evalua intervalo x intervalo
                for (int i = 0; i < ConstantesDpo.Itv1min; i++)
                {
                    if (dato.Medicion[i] != 0) continue;

                    string rangoFaltante = string.Empty;
                    List<decimal> ev = dato.Medicion.ToList();

                    //Evaluación rango faltante
                    if (i == 0)
                        rangoFaltante = "ini";
                    else if (i == ConstantesDpo.Itv1min - 1)
                        rangoFaltante = "fin";
                    else
                    {        
                        if (ev.GetRange(0, (i + 1))
                            .Select(x => Math.Abs(x))
                            .Sum() == 0)
                            rangoFaltante = "ini";
                        else if (ev.GetRange(i, ConstantesDpo.Itv1min - i)
                            .Select(x => Math.Abs(x))
                            .Sum() == 0)
                            rangoFaltante = "fin";
                        else
                            rangoFaltante = "med";
                    }

                    if (rangoFaltante == "ini")
                    {
                        #region Rango faltante al inicio
                        int idxPrevio = 0;
                        List<decimal> input = new List<decimal>();
                        int j = 1;
                        while (j < ConstantesDpo.Itv1min)
                        {
                            if (dato.Medicion[j] != 0)
                            {
                                input.Add(dato.Medicion[j]);
                                if (idxPrevio == 0) idxPrevio = (j - 1);
                            }

                            if (input.Count == 2) break;
                            j++;
                        }
                        decimal ex = input.Sum() / 2;
                        decimal delta = ex - promedioMedicion[idxPrevio];
                        int k = idxPrevio;
                        while (k >= 0)
                        {
                            dato.Medicion[k] = promedioMedicion[k] + delta;
                            k--;
                        }

                        i = idxPrevio;
                        #endregion
                    }
                    if (rangoFaltante == "fin")
                    {
                        #region Rango faltante al final
                        int idxPrevio = 0;
                        List<decimal> input = new List<decimal>();
                        int j = i - 1;
                        while (j >= 0)
                        {
                            if (dato.Medicion[j] != 0)
                            {
                                input.Add(dato.Medicion[j]);
                                if (idxPrevio == 0) idxPrevio = (j + 1);
                            }
                            if (input.Count == 2) break;
                            j--;
                        }
                        decimal ex = input.Sum() / 2;
                        decimal delta = ex - promedioMedicion[idxPrevio];
                        int k = idxPrevio;
                        while (k < ConstantesDpo.Itv1min)
                        {
                            dato.Medicion[k] = promedioMedicion[k] + delta;
                            k++;
                        }
                        i = k;
                        #endregion
                    }
                    if (rangoFaltante == "med")
                    {
                        #region Rango faltante en medio
                        //Obtención del intervalo inmediato anterior
                        int idxPrevioIni = 0;
                        List<decimal> inputRangoIni = new List<decimal>();
                        int j = i - 1;
                        while (j >= 0)
                        {
                            if (dato.Medicion[j] != 0)
                            {
                                inputRangoIni.Add(dato.Medicion[j]);
                                if (idxPrevioIni == 0) idxPrevioIni = (j + 1);
                            }
                            if (inputRangoIni.Count == 2) break;
                            j--;
                        }
                        decimal exIni = inputRangoIni.Sum() / 2;
                        decimal deltaRangoIni = exIni - promedioMedicion[idxPrevioIni];

                        //Obtención del intervalo inmediato posterior
                        int idxPrevioFin = 0;
                        List<decimal> inputRangoFin = new List<decimal>();
                        j = i + 1;
                        while (j < ConstantesDpo.Itv1min)
                        {
                            if (dato.Medicion[j] != 0)
                            {
                                inputRangoFin.Add(dato.Medicion[j]);
                                if (idxPrevioFin == 0) idxPrevioFin = (j - 1);
                            }

                            if (inputRangoFin.Count == 2) break;
                            j++;
                        }
                        decimal exFin = inputRangoFin.Sum() / 2;
                        decimal deltaRangoFin = exFin - promedioMedicion[idxPrevioFin];

                        decimal funcLinealRango = ((deltaRangoIni + deltaRangoFin) * -1) / idxPrevioFin;
                        j = idxPrevioIni;
                        while (j <= idxPrevioFin)
                        {
                            dato.Medicion[j] = promedioMedicion[j]
                                + deltaRangoIni
                                + funcLinealRango;
                            j++;
                        }
                        i = idxPrevioFin;
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// Método que da formato a las mediciones para el proceso de alisado_gaussiano y lo aplica
        /// </summary>
        /// <param name="datosMedicion">Datos de las mediciones de entrada</param>
        /// <param name="length">Variable para el alisado gaussiano</param>
        /// <param name="std">Variable para el alisado gaussiano</param>
        /// <returns></returns>
        public List<DpoDemandaScoDTO> ProcesoAlisadoGaussiano(
            List<DpoDemandaScoDTO> datosMedicion,
            int length, double std)
        {
            List<DpoDemandaScoDTO> entidades = new List<DpoDemandaScoDTO>();

            List<int> tipoInformacion = this.ListaVariablesUnidadTna();

            foreach (int t in tipoInformacion)
            {
                //Obtiene las mediciones de cada tipo de información
                List<DpoDemandaScoDTO> mPuntoInfo = datosMedicion
                    .Where(x => x.Prnvarcodi == t)
                    .OrderBy(x => x.Medifecha)
                    .ToList();

                if (mPuntoInfo.Count == 0) continue;

                //Agrupa las mediciones por día en formato mensual
                List<decimal> medicionMes = new List<decimal>();
                foreach (DpoDemandaScoDTO m in mPuntoInfo)
                {
                    decimal[] arrMes = UtilDpo.ConvertirMedicionEnArreglo(
                        ConstantesDpo.Itv15min, m);
                    medicionMes.AddRange(arrMes);
                }

                //Aplica el alissado gaussiano
                decimal[] arrMedicionMes = medicionMes.ToArray();
                this.Alissado_Gaussiano(arrMedicionMes, length, std);

                //Devuelve la información filtrada a su medición correspondiente
                int i = 0;
                foreach (DpoDemandaScoDTO m in mPuntoInfo)
                {
                    int j = 0;
                    while (j < ConstantesDpo.Itv15min)
                    {
                        decimal val = arrMedicionMes[j + (i * ConstantesDpo.Itv15min)];
                        m.GetType().GetProperty($"H{(j + 1)}").SetValue(m, val);
                        j++;
                    }
                    i++;
                }
                entidades.AddRange(mPuntoInfo);
            }

            return entidades;
        }

        /// <summary>
        /// Realiza el filtrado por minuto a una medición
        /// </summary>
        /// <param name="dataMedicion">Datos de la medición</param>
        /// <returns></returns>
        public List<decimal> FiltradoPorMinuto(
            List<decimal> dataMedicion)
        {
            decimal toleranciaxMinuto = 100;
            List<decimal> dataFiltrada = new List<decimal>();
            dataFiltrada.Add(dataMedicion[0]);

            int i = 1;//indice de inicio
            while (i < dataMedicion.Count)
            {
                decimal valor = dataMedicion[i] - dataMedicion[i - 1];
                valor = Math.Abs(valor);
                if (valor > toleranciaxMinuto)
                    dataFiltrada.Add(valor);
                i++;
            }

            return dataFiltrada;
        }
        #endregion

        #region Módulo - [DEF.Anexo C] Generación de info i60(1min) y info i60(30min) desde info i1440(1min)

        #region Proceso Automatico
        /// <summary>
        /// Job que se ejecuta cada 5 minutos y busca los archivos de 1 minuto y los procesa
        /// </summary>
        public void ObtenerDatosRawCada5Mninutos()
        {
            try
            {
                DateTime dAhora = DateTime.Now.AddMinutes(-5); 
                //00:05:02,202 - Lo estoy retrasando 5 minutos para darle un margen en que aparezcan los archivos
                //Redondeamos la fecha/hora a intervalos de 5 minutos exactos
                //restamos los milisegundos
                int iMiliSegundos = dAhora.Millisecond; //202
                dAhora = dAhora.AddMilliseconds(-iMiliSegundos); //13:05:02,000
                                                           //restamos los segundos
                int iSegundos = dAhora.Second; //02
                dAhora = dAhora.AddSeconds(-iSegundos); //13:05:00,000
                                                  //Validamos que los minutos sean multiplo de 5
                int iMinutos = dAhora.Minute; //05
                int iMinutosRedondeados = iMinutos - (iMinutos % 5);
                dAhora = dAhora.AddMinutes(iMinutosRedondeados - iMinutos);  //2023/05/28 13:05:00,000
                Logger.Info("[Srvc:828] JOB.INICIO: " + dAhora.ToString("dd/MM/yyyy HH:mm") + "]: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                //Paso 1 - Validamos que no exista un procedimiento que esta poniendo al dia la carga de los TNA:
                //select * from dpo_estimadorraw_files where flag = 2
                DpoEstimadorRawFilesDTO dtoEstimadorRawFilesFlag = FactorySic.GetDpoEstimadorRawRepository().ObtenerEstimadorRawFilesFlag();
                if (dtoEstimadorRawFilesFlag == null)
                {
                    string sProcedimiento = "Auto";
                    //Paso 2: Validamos cual fue la ultima lectura disponible en DPO_ESTIMADORRAW_FILES
                    /*
                     select * from dpo_estimadorraw_files
                     where fechaarchivoraw = (select max(fechaarchivoraw) from dpo_estimadorraw_files where tipo = 1) and tipo = 1
                     order by fechaarchivoraw desc
                     */
                    DpoEstimadorRawFilesDTO dtoEstimadorRawFiles = FactorySic.GetDpoEstimadorRawRepository().ObtenerUltimoEstimadorRawFiles();
                    if (dtoEstimadorRawFiles != null && dtoEstimadorRawFiles.FechaArchivoRaw < dAhora) 
                    {
                        //Si ingresa es porque la lectura de los archivos tiene que ponerse al corriente : 2023/05/27 23:45
                        DateTime dIntervaloInicio = dtoEstimadorRawFiles.FechaArchivoRaw.AddMinutes(5);
                        //Paso 3: Dejamos una bandera para que nadie ejecute nada hasta que se ponga al día -> SETEAMOS EL VALOR 2 EN FLAG
                        FactorySic.GetDpoEstimadorRawRepository().UpdateFileRaw(dtoEstimadorRawFiles.NomArchivoRaw, dtoEstimadorRawFiles.FechaArchivoRaw, "1", "2");

                        //Paso4: completamos hacia adelante hasta dIntervaloInicio.AddMinutes(30)
                        for (DateTime dFecha = dIntervaloInicio; dFecha <= dAhora; dFecha = dFecha.AddMinutes(5))
                        {
                            if (dFecha > dIntervaloInicio.AddMinutes(30))
                                break; //Bloques de 30 minutos sale
                            EjecutaProcesoLecturaRawPorMinuto(dFecha, dFecha.Day, sProcedimiento);
                        }
                        //Paso5: SETEAMOS A 1 EL FLAG
                        FactorySic.GetDpoEstimadorRawRepository().UpdateFileRaw(dtoEstimadorRawFiles.NomArchivoRaw, dtoEstimadorRawFiles.FechaArchivoRaw, "1", "1");
                    }
                }
                else
                {   //dtoEstimadorRawFilesFlag != null --> Existe un Flag
                    //Por lo tanto, se esta poniendo al día.
                    //Paso 1: Validamos cual fue la ultima lectura disponible en DPO_ESTIMADORRAW_FILES
                    /*
                     select * from dpo_estimadorraw_files
                     where fechaarchivoraw = (select max(fechaarchivoraw) from dpo_estimadorraw_files where tipo = 1) and tipo = 1
                     order by fechaarchivoraw desc
                     */
                    DpoEstimadorRawFilesDTO dtoEstimadorRawFiles = FactorySic.GetDpoEstimadorRawRepository().ObtenerUltimoEstimadorRawFiles();
                    //Paso 2: Seteamos una variable "dUltimaLecturaMas60" que permite trabajar con un margen de minutos donde la ultima lectura quedo congelado
                    //Seria la fecha/hora de la ultima lectura mas de 60 minutos
                    DateTime dUltimaLecturaMas60 = dtoEstimadorRawFiles.FechaArchivoRaw.AddMinutes(60);
                    //comparado con la fecha/hora del sistema -> hoy
                    if (dtoEstimadorRawFiles != null && dUltimaLecturaMas60 < dAhora)
                    {
                        //Pao 3: SETEAMOS A 1 EL FLAG, para que continue con la carga desde la Ultima lectura
                        FactorySic.GetDpoEstimadorRawRepository().UpdateFileRaw(dtoEstimadorRawFilesFlag.NomArchivoRaw, dtoEstimadorRawFilesFlag.FechaArchivoRaw, "1", "1");
                    }
                    //Else, pasa de largo, pues se esta ejecutando un procedimiento que se esta poniendo al dia.
                }
                Logger.Info("[Srvc:882] JOB.FIN: " + dAhora.ToString("dd/MM/yyyy HH:mm") + "]: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Procedimiento que busca un archivo y procesa un archivo raw para un grupo de 5 minutos
        /// </summary>
        public string EjecutaProcesoLecturaRawPorMinuto(DateTime fechaHoraProceso, int diaProceso, string sProcedimiento)
        {
            string sResultado = "1";

            try
            {
                int intervalo = 1; //Archivos de un minuto

                int tolerancia = 5; //Lectura de Archivos en grupos de 5
                //referencia toma el año, mes y dia a las 00:00
                DateTime referencia = new DateTime(fechaHoraProceso.Year, fechaHoraProceso.Month, fechaHoraProceso.Day, 0, 0, 0);

                // Setea el primer intervalo (H1)
                DateTime temporal = referencia.AddMinutes(intervalo);

                // Intervalo limite, para controlar el cammbio de día
                DateTime limite = (fechaHoraProceso < temporal) ? referencia.AddDays(-1) : referencia;

                //strHoy = HH:MM
                string strHoy = fechaHoraProceso.ToString(ConstantesDpo.FormatoHoraMinuto);

                /* ------------------------------------------------------------------------------------------------ */
                // Intervalo valido
                if (limite < fechaHoraProceso)
                {
                    // Obtiene posibles intervalos en caso el original no exista
                    // rango permitido +/-5 min o +/-30 min
                    DateTime minRangoInf = fechaHoraProceso.AddMinutes(1);
                    List<string> rangoTolerancia = new List<string>();
                    int i = 0;
                    while (i < tolerancia)
                    {
                        minRangoInf = minRangoInf.AddMinutes(-1);
                        string strMinRangoInf = minRangoInf.ToString(ConstantesDpo.FormatoHoraMinuto);
                        // Se llena la lista con los nombres de los archivos a procesar ....
                        rangoTolerancia.Add(minRangoInf.ToString(ConstantesDpo.FormatoFechaArchivoRaw) + "_" +
                                            strMinRangoInf.Replace(":", string.Empty) +
                                            "_PSSEOutput.raw");
                        i++;
                    }
                    // Eliminar los elementos duplicados de la lista
                    rangoTolerancia = rangoTolerancia.Distinct().ToList();
                    
                    string ruta = String.Format(ConfigurationManager.AppSettings[ConstantesDpo.RutaDemandaRaw] + "{0:0000}\\{1:00}\\{2:00}\\", fechaHoraProceso.Year, fechaHoraProceso.Month, fechaHoraProceso.Day);
                    string rutaRespaldo = String.Format(ConfigurationManager.AppSettings[ConstantesDpo.PathProcesoDemandaRespaldoRaw] + "{0:0000}\\{1:00}\\{2:00}\\", fechaHoraProceso.Year, fechaHoraProceso.Month, fechaHoraProceso.Day);
                    string nombreArchivo = fechaHoraProceso.ToString(ConstantesDpo.FormatoFechaArchivoRaw) + "_" +
                                           strHoy.Replace(":", string.Empty) +
                                           "_PSSEOutput.raw";
                    this.ObtenerDatosArchivoRawXMinuto(ruta,
                                                       ConstantesDpo.EtmrawfntSco,
                                                       DateTime.Now,
                                                       nombreArchivo,
                                                       rangoTolerancia,
                                                       fechaHoraProceso,
                                                       diaProceso,
                                                       ConstantesDpo.CargaAutoRawXMinuto,
                                                       sProcedimiento,
                                                       rutaRespaldo);
                }

                /* ------------------------------------------------------------------------------------------------ */
                // Se ejecuta solo una vez al dia a las 0:30
                // Eliminamos en el log todos los registros de todos los procesos que tengan antiguedad de 60 dia de antiguedad
                // a partir de la fechaHoraProceso
                DateTime fecDepuracion = new DateTime(fechaHoraProceso.Year, fechaHoraProceso.Month, fechaHoraProceso.Day, 0, 30, 0);
                if (sProcedimiento.Equals("Auto") && fecDepuracion >= fechaHoraProceso && fecDepuracion < fechaHoraProceso.AddMinutes(5))
                {
                    this.DeleteRawsHistoricos(fecDepuracion);
                    Logger.Info("[Srvc:960] DeleteRawsHistoricos: " + fecDepuracion.ToString("dd/MM/yyyy HH:mm"));

                    // Se ejecuta solo una vez al dia para optimizar la tabla dpo_estimadorraw_tmp
                    this.DeleteREstimadorRawTemporalByDiaProceso(fecDepuracion.AddYears(-3), fecDepuracion.AddDays(-1));
                    Logger.Info("[Srvc:964] DeleteRawsTMP menor a: " + fecDepuracion.AddDays(-3).ToString("dd/MM/yyyy HH:mm"));
                }

                /* ------------------------------------------------------------------------------------------------ 
                // para intervalos de 15, 30 y 45 minutos, se va intentar migrar los RAW de TMP a dpo_estimadorraw_YYYYMM
                if (sProcedimiento.Equals("Auto") &&
                    (   (fechaHoraProceso.Minute >= 15 && fechaHoraProceso.Minute < 20) ||
                        (fechaHoraProceso.Minute >= 30 && fechaHoraProceso.Minute < 35) ||
                        (fechaHoraProceso.Minute >= 45 && fechaHoraProceso.Minute < 50)
                    )
                   )
                {
                    DateTime dHoraMinuto1 = fechaHoraProceso.AddMinutes(-59); // bajamos 59 minutos para trabajar sobre la ultima hora
                    //Traemos la lista horas de RAW que no estan en dpo_estimadorraw_YYYYMM
                    List<DpoEstimadorRawDTO> listaHoras = FactorySic.GetDpoEstimadorRawRepository().verificarFaltantesDia(dHoraMinuto1).OrderByDescending(x => x.Dporawfecha).ToList();
                    foreach (DpoEstimadorRawDTO dto in listaHoras)
                    {
                        DateTime hora = dto.Dporawfecha; //20/06/2023 17:00 => Migrar 17:01 a 18:00
                        DateTime horaLimite = fechaHoraProceso.AddHours(-1);
                        int result = hora.CompareTo(fechaHoraProceso);
                        result = hora.CompareTo(horaLimite);
                        if (result > 0) continue;
                        //Para cada fecha/hora que no se ha migrado
                        string sTabla = "tmp";
                        DateTime dFecha60 = hora.AddMinutes(60); //termina en el minuto 18:00 la hora siguiente
                        string sufAnioMes = hora.ToString("yyyyMM"); // Se le asignara a la tabla correspondiente del año mes: DPO_ESTIMADORRAW_YYYYMM
                        Logger.Info("[Srvc:1178] MigrarRawsProcesadosHora: DPO_ESTIMADORRAW_" + sufAnioMes + " -> [INICIO] " + hora.ToString("dd/MM/yyyy HH:mm"));
                        FactorySic.GetDpoEstimadorRawRepository().MigrarRawsProcesadosHora(sufAnioMes,
                                                                                           hora.ToString(ConstantesDpo.FormatoFechaMedicionRaw),
                                                                                           hora.ToString(ConstantesDpo.FormatoHoraMinuto).Substring(0, 2),
                                                                                           dFecha60, sTabla);
                        Logger.Info("[Srvc:1183] MigrarRawsProcesadosHora: DPO_ESTIMADORRAW_" + sufAnioMes + " -> [OK] " + hora.ToString("dd/MM/yyyy HH:mm"));
                        break; //Solo migra 1 bloque, para no saturar el procedimiento
                    }
                }
                */
            }
            catch (Exception ex)
            {
                sResultado = ex.StackTrace;
                sResultado = ex.Message;
                Logger.Error("[Srvc:1005] StackTrace: " + ex.StackTrace);
                Logger.Error("[Srvc:1006] Message: " + ex.Message);
                return sResultado;
            }

            return sResultado;
        }

        /// <summary>
        /// Job que se ejecuta cada día a la 13 horas donde busca los archivos IEOD en intervalo de 30 minutos
        /// del día anterior [00:30 a 23:59] y los procesa
        /// </summary>
        public void ObtenerDatosRawIEOD1Dia()
        {
            string sResultado = "1";
            try
            {
                DateTime hoy = DateTime.Now; //Fecha de hoy, p.e. 04/10/2023
                //Se ejecutara para el día de ayer
                DateTime ayer = hoy.AddDays(-1); // 03/10/2023

                //lo llevamos a 03/10/2023 00:30:00
                //restamos los milisegundos
                int iMiliSegundos = ayer.Millisecond;
                ayer = ayer.AddMilliseconds(-iMiliSegundos);
                //restamos los segundos
                int iSegundos = ayer.Second;
                ayer = ayer.AddSeconds(-iSegundos);
                //restamos los minutos
                int iMinutos = ayer.Minute;
                ayer = ayer.AddMinutes(-iMinutos);
                //restamos las horas
                int iHoras = ayer.Hour;
                ayer = ayer.AddHours(-iHoras);

                string strFecProcess = ayer.ToString("dd/MM/yyyy HH:mm");
                sResultado = EjecutaJobGeneracionRawsIEOD(strFecProcess);
                Logger.Info("ObtenerDatosRawIEOD1Dia:Se va a ejecutar EjecutaJobProcesoAutomaticoRawPor30Minutos " + ayer.ToString("dd/MM/yyyy HH:mm"));

                for (int i = 30; i <= 1440; i += 30)
                {
                    DateTime parseFecha = ayer.AddMinutes(i);
                    sResultado = EjecutaJobProcesoAutomaticoRawPor30Minutos(parseFecha);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca un archivo y procesa un archivo raw IEOD
        /// </summary>
        public string EjecutaJobProcesoAutomaticoRawPor30Minutos(DateTime fechaHoraProceso)
        {
            string sResultado = "1";

            try
            {
                int intervalo = 48;

                int tolerancia = 1;

                DateTime referencia = new DateTime(fechaHoraProceso.Year, fechaHoraProceso.Month, fechaHoraProceso.Day, 0, 0, 0);

                // Setea el primer intervalo (H1)
                DateTime temporal = referencia.AddMinutes(intervalo);

                // Intervalo limite
                DateTime limite = (fechaHoraProceso < temporal) ? referencia.AddDays(-1) : referencia;

                // Intervalo valido
                if (limite < fechaHoraProceso)
                {
                    // Obtiene posibles intervalos en caso el original no exista
                    // rango permitido +/-30 min
                    DateTime minRangoInf = fechaHoraProceso.AddMinutes(30);

                    List<string> rangoTolerancia = new List<string>();

                    int i = 0;
                    while (i < tolerancia)
                    {
                        minRangoInf = minRangoInf.AddMinutes(-30);
                        // Se llena la lista con los nombres de los archivos a procesar ....
                        rangoTolerancia.Add("PSSE_" + minRangoInf.ToString("yyyyMMddHHmm") + ".raw");

                        i++;
                    }

                    string ruta = ConfigurationManager.AppSettings[ConstantesDpo.RutaDemandaRawCostoMarginal].ToString();

                    string nombreArchivo = "PSSE_" + fechaHoraProceso.ToString("yyyyMMddHHmm") + ".raw";

                    Logger.Info("ObtenerDatosRawSco:Buscando archivo " + nombreArchivo);

                    this.ObtenerDatosArchivoRawX30Minutos(ruta,
                                                          ConstantesDpo.EtmrawfntSco,
                                                          new DateTime(),
                                                          nombreArchivo,
                                                          rangoTolerancia,
                                                          fechaHoraProceso,
                                                          ConstantesDpo.CargaAutoRawX30Minutos);
                }
                /* ------------------------------------------------------------------------------------------------ */
                // Se ejecuta solo al final de haber cargado los 24 archivos a las 00:00 del día siguiente
                DateTime fecMigracion = new DateTime(fechaHoraProceso.Year, fechaHoraProceso.Month, fechaHoraProceso.Day, 0, 0, 0);
                if (fecMigracion == fechaHoraProceso)
                {
                    //se le resta un minuto para que no salte a la tabla siguiente
                    string sufAnioMes = fechaHoraProceso.AddMinutes(-1).ToString("yyyyMM"); // Se le asignara a la tabla correspondiente del año mes
                    string sFechaProceso = fechaHoraProceso.AddMinutes(-1).ToString("dd/MM/yyyy"); // Intervalo de inicio de carga
                    string sFechaSiguiente = fechaHoraProceso.ToString("dd/MM/yyyy"); // Intervalo final de carga
                    Logger.Info("[Srvc:1120] Proc.AutomaticoIEOD: IniciaMigración: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    //Migramos intervalo de 30 minutos
                    FactorySic.GetDpoEstimadorRawRepository().UpdateRawsProcesados30Minutos(sufAnioMes, sFechaProceso);
                    Logger.Info("[Srvc:1123] Proc.AutomaticoIEOD: 30MinutosMigrado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    //Migramos intervalos de 60 Minutos
                    FactorySic.GetDpoEstimadorRawRepository().UpdateRawsProcesados60Minutos(sufAnioMes, sFechaProceso, sFechaSiguiente);
                    Logger.Info("[Srvc:1126] Proc.AutomaticoIEOD: 1HoraMigrado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    //Truncamos la tabla
                    this.TruncateTemporalDpoEstimadorRaw(ConstantesDpo.CargaAutoRawX30Minutos);
                }
            }
            catch (Exception ex)
            {
                sResultado = ex.StackTrace;
                return ex.Message;
            }

            return sResultado;
        }

        /// <summary>
        /// Método que lee y procesa los archivos raw de un directorio
        /// </summary>
        /// <param name="ruta">Ubicación de los archivos</param>
        /// <param name="fuente">Fuente de la lectura (manual o auto)</param>
        /// <param name="fechaImportacion">Fecha para la importación(solo manual)</param>
        /// <param name="archivoEspecifico">Archivo específico para la carga de datos</param>
        /// <param name="rangoTolerancia">Lista de archivos raw a procesar</param>
        /// <param name="fechaHoraProceso">Fecha - hora del proceso para la importación</param>
        /// <param name="diaProceso">Fecha - hora del proceso para la importación</param>
        /// <param name="proceso">Tipo de proceso</param>
        /// <param name="sProcedimiento">Manual / Automatico</param>
        /// <param name="rutaRespaldo">Ubicación de los archivos (respaldo)</param>
        public void ObtenerDatosArchivoRawXMinuto(string ruta,
                                                  int fuente,
                                                  DateTime fechaImportacion,
                                                  string archivoEspecifico,
                                                  List<string> rangoTolerancia,
                                                  DateTime fechaHoraProceso,
                                                  int diaProceso,
                                                  int proceso,
                                                  string sProcedimiento,
                                                  string rutaRespaldo)
        {
            /* Valida el directorio de los archivos generados */
            bool existe = FileServerScada.VerificarLaExistenciaDirectorio(ruta);
            if (!existe)
            {
                Logger.Error("[Srvc:1166] VerificarLaExistenciaDirectorio: La ruta [" + ruta + "] no es valida o el usuario no tiene acceso");
                existe = FileServerScada.VerificarLaExistenciaDirectorio(rutaRespaldo);

                if (!existe)
                {
                    Logger.Error("[Srvc:1166] VerificarLaExistenciaDirectorio (respaldo): La ruta [" + ruta + "] no es valida o el usuario no tiene acceso");
                    return;
                }
            }

            try
            {
                /* Llena el listado de archivos FileData -> DPO_ESTIMADORRAW_TMP */
                List<FileData> archivos = new List<FileData>();

                if (!string.IsNullOrEmpty(archivoEspecifico))
                {
                    /* Validar que los archivos del paquete de N archivos raw existan en el directorio fisico y conviertelos a formato FileData 
                     Si existen, los inserta en dpo_estimadorraw_files con tipo = 1 y flag = 0 */
                    archivos = ConvertirStringArchivosToFileData(rangoTolerancia, ruta, proceso.ToString(), rutaRespaldo);

                    /* Procesa los archivos raw FileData validos */
                    if (archivos.Count > 0)
                    {
                        //Insertando el TNA en DPO_ESTIMADORRAW_TMP
                        ProcesarDatosArchivoRaw(ruta, fuente, fechaImportacion, archivos, proceso, sProcedimiento, rutaRespaldo);
                    }
                }

                /* ----------------------------------------------------------------------------------------------------- */
                /* --PROCEDIMIENTO QUE SE EJECUTA AL FINAL DE UNA HORA: 01:00 / 02:00 / 03:00 / 04:00 / 05:00 / etc ---- */
                /* ----------------------------------------------------------------------------------------------------- */
                /* PASO 1 - Si los minutos son CERO, HORA DE CAMBIO.
                 * Valida que la hora esta completa para migrar de DPO_ESTIMADORRAW_TMP -> DPO_ESTIMADORRAW */
                if (fechaHoraProceso.Minute >= 0 && fechaHoraProceso.Minute < 5)
                {
                    while (fechaHoraProceso.Minute > 0)
                    {
                        fechaHoraProceso = fechaHoraProceso.AddMinutes(-1);
                    }

                    DateTime dHoraMinInicio = fechaHoraProceso.AddMinutes(-59); // para que inicie en el minuto 01
                    /* ------------------------------------------------------------------------------------------------------- */
                    /* PASO 2: PROCEDIMIENTO QUE RETOMA DESDE EL PRIMER ARCHIVO NO ENCONTRADO EN LAPRESETE HORA XX:01 HASTA X1:00 
                     * CON LA FINALIDAD DE INCORPORARLO ANTES DE COMPLETAR LA HORA Y SE MIGRE A DPO_ESTIMADORRAW_YYYYMM (final)---- */
                    //Consultamos la tabla DPO_ESTIMADORRAW_LOG
                    List<DpoEstimadorRawLogDTO> listLogRawUltimaHora = FactorySic.GetDpoEstimadorRawRepository().ListFilesLogRawHora(proceso.ToString(), dHoraMinInicio, fechaHoraProceso);
                    Logger.Info("[Srvc - INICIO REINTENTO] Se encontraron " + listLogRawUltimaHora.Count + " archivos raws por recuperar!");
                    string sArchivoTemp = "";
                    List<string> archivosRawLogNoProcesados = new List<string>();
                    foreach (DpoEstimadorRawLogDTO fileLogRaw in listLogRawUltimaHora)
                    {
                        //a) Para cada archivo que no se encontro en su momento
                        if (sArchivoTemp == fileLogRaw.NomArchivoRaw)
                        {
                            continue; //Por si existen duplicados en la tabla DPO_ESTIMADORRAW_LOG
                        }
                        sArchivoTemp = fileLogRaw.NomArchivoRaw;

                        /* b) Verifica si existe el archivo fisico, que no fue procesado en su momento, si ya se encuentra en el directorio, ok*/
                        bool existeArchivo = FileServerScada.VerificarExistenciaFile(string.Empty, fileLogRaw.NomArchivoRaw, ruta);
                        Logger.Info("[Srvc Reintento] Raw " + fileLogRaw.NomArchivoRaw + (existeArchivo ? " encontrado!" : " no encontrado!"));
                        if (existeArchivo)
                        {
                            /* c) Eliminamos el archivo no procesado DPO_ESTIMADORRAW_LOG y DPO_ESTIMADORRAW_FILES
                             pues el procedimiento ConvertirStringArchivosToFileData los volvera a considerar*/
                            FactorySic.GetDpoEstimadorRawRepository().DeleteLogRaw(fileLogRaw.NomArchivoRaw, fileLogRaw.FechaArchivoRaw);
                            FactorySic.GetDpoEstimadorRawRepository().DeleteREstimadorRawFileByNomArchivo(fileLogRaw.NomArchivoRaw, proceso);
                            /* Final: Lee los datos del archivo no procesado y lo guarda en la tabla temporal DPO_ESTIMADORRAW_TMP */
                            archivosRawLogNoProcesados.Add(fileLogRaw.NomArchivoRaw);
                            
                        }
                    }
                    if (archivosRawLogNoProcesados.Count > 0)
                    {
                        List<FileData> archivosFileDataLog = ConvertirStringArchivosToFileData(archivosRawLogNoProcesados, ruta, proceso.ToString(), rutaRespaldo);
                        if(archivosFileDataLog.Count > 0)
                            ProcesarDatosArchivoRaw(ruta, fuente, fechaImportacion, archivosFileDataLog, proceso, sProcedimiento, rutaRespaldo);
                    }
                    /* ------------------------------------------------------------------------------------------------------- */
                    /* PASO 3 - Migrar los registros de la tabla temporal a la tabla principal [DPO_ESTIMADORRAW_TMP -> DPO_ESTIMADORRAW] 
                    * el formato fecha dd/mm/yyyy hh:00 */
                    DateTime dFecha60 = fechaHoraProceso; //indica la hora final, para el ultimo intervalo de la hora (ver sql, minuto 60)
                    string sufAnioMes = dHoraMinInicio.ToString("yyyyMM"); // Se le asignara a la tabla correspondiente del año mes: DPO_ESTIMADORRAW_YYYYMM
                    string sTabla = "tmp";
                    if (sProcedimiento.Equals("Manual")) sTabla = "manual";
                    Logger.Info("[Srvc:1246] MigrarRawsProcesadosHora: DPO_ESTIMADORRAW_TMP -> DPO_ESTIMADORRAW_" + sufAnioMes + " -> [INICIO] " + fechaHoraProceso.AddHours(-1).ToString("dd/MM/yyyy HH:mm"));
                    FactorySic.GetDpoEstimadorRawRepository().MigrarRawsProcesadosHora(sufAnioMes,
                                                                                       fechaHoraProceso.AddHours(-1).ToString(ConstantesDpo.FormatoFechaMedicionRaw),
                                                                                       fechaHoraProceso.AddHours(-1).ToString(ConstantesDpo.FormatoHoraMinuto).Substring(0, 2),
                                                                                       dFecha60, sTabla);
                    Logger.Info("[Srvc:1251] MigrarRawsProcesadosHora: DPO_ESTIMADORRAW_TMP -> DPO_ESTIMADORRAW_" + sufAnioMes + " -> [OK] " + fechaHoraProceso.AddHours(-1).ToString("dd/MM/yyyy HH:mm"));
                    /* PASO 4 - Elimina los registros de la tabla DPO_ESTIMADORRAW_TMP */
                    if (sTabla.Equals("tmp"))
                    {
                        //Del minuto 0X:01 al 0[X+1]:00
                        this.DeleteREstimadorRawTemporalByDiaProceso(dFecha60.AddMinutes(-59), dFecha60);
                        Logger.Info("[Srvc - Reintento] Se eliminó los registros de la tabla temporal (TMP) de " + dFecha60.AddMinutes(-59) + " hasta las " + dFecha60);
                    }
                    else
                    {
                        //TRuncando la tabla Manual, la carga es mas rapido
                        this.TruncateTemporalDpoEstimadorRaw(ConstantesDpo.CargaManualRaw);
                        Logger.Info("[Srvc - Reintento] Se eliminó los registros de la tabla manual de " + dFecha60.AddMinutes(-59) + " hasta las " + dFecha60);
                    }
                    Logger.Info("[Srvc - FIN REINTENTO] Fin de Reintentos!");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("[Srvc:1270] " + fechaHoraProceso.AddHours(-1).ToString("dd/MM/yyyy HH:mm") + " [StackTrace]: " + ex.StackTrace);
                Logger.Error("[Srvc:1271] " + fechaHoraProceso.AddHours(-1).ToString("dd/MM/yyyy HH:mm") + " [Message]: " + ex.Message);

            }
        }

        /// <summary>
        /// Método que lee y procesa los archivos raw de un directorio
        /// </summary>
        /// <param name="ruta">Ubicación de los archivos</param>
        /// <param name="fuente">Fuente de la lectura (manual o auto)</param>
        /// <param name="fechaImportacion">Fecha para la importación(solo manual)</param>
        /// <param name="archivoEspecifico">Archivo específico para la carga de datos</param>
        /// <param name="rangoTolerancia">Lista de archivos raw a procesar</param>
        /// <param name="fechaHoraProceso">Fecha - hora del proceso para la importación</param>
        /// <param name="proceso">Tipo de proceso</param>
        public void ObtenerDatosArchivoRawX30Minutos(string ruta,
                                                     int fuente,
                                                     DateTime fechaImportacion,
                                                     string archivoEspecifico,
                                                     List<string> rangoTolerancia,
                                                     DateTime fechaHoraProceso,
                                                     int proceso)
        {
            try
            {
                /* Valida el directorio de los archivos generados */
                bool existe = FileServerScada.VerificarLaExistenciaDirectorio(ruta);
                if (!existe)
                {
                    Logger.Error("ObtenerDatosArchivoRaw: La ruta ingresada no es valida o el usuario no tiene acceso");
                    return;
                }

                /* Llena el listado de archivos FileData */
                List<FileData> archivos = new List<FileData>();

                if (!string.IsNullOrEmpty(archivoEspecifico))
                {
                    /* Validar que los archivos del paquete de N archivos raw existan en 
                       el directorio fisico y conviertelos a formato FileData */
                    archivos = ConvertirStringArchivosToFileData(rangoTolerancia, ruta, proceso.ToString(), string.Empty);

                    /* Procesa los archivos raw FileData validos */
                    if (archivos.Count > 0)
                    {
                        ProcesarDatosArchivoRaw(ruta, fuente, fechaImportacion, archivos, proceso, "Auto", string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("[Srvc:1322] ObtenerDatosArchivoRawX30Minutos/StackTrace: " + ex.StackTrace);
                Logger.Error("[Srvc:1323] ObtenerDatosArchivoRawX30Minutos/Message: " + ex.Message);

            }
        }

        #region Proceso Manual
        /// <summary>
        /// Metodo que ejecuta la lectura de archivos Raw de una carpeta especifica
        /// </summary>
        /// <param name="fechaHoraProceso">Fecha para la importación</param>
        /// <param name="direccion">Dirección de los archivos</param>
        /// <returns></returns>
        public object EjecutaProcesoManualRawDemandaCPSco(string fechaHoraProceso, string direccion)
        {
            // Inicializa la devolucion del procedimiento
            string msg = string.Empty;
            string type = string.Empty;

            // Verifica el contenido de la carpeta
            bool existe = FileServerScada.VerificarLaExistenciaDirectorio(direccion);
            if (!existe)
            {
                msg = "Ingrese una dirección valida";
                type = ConstantesDpo.MsgWarning;

                return new { msg, type };
            }

            // Convierte a Datetime y formatea la fecha de proceso
            DateTime parseFecha = DateTime.ParseExact(fechaHoraProceso, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);

            // Se obtiene el año mes para el nombre de la tabla ESTIMADORRAW_{0}
            string sufAnioMes = parseFecha.ToString("yyyyMM");

            // Obtener los archivos tipo FileData del directorio leido
            List<FileData> archivos = FileServerScada.ListarArhivos(string.Empty, direccion);

            // Valida que la fecha de proceso no sea mayor a la fecha actual del servidor
            if (parseFecha > DateTime.Now)
            {
                msg = "La fecha de proceso no puede ser mayor a la fecha actual";
                type = ConstantesDpo.MsgWarning;

                return new { msg, type };
            }

            // Valida la cantidad minima y maxima de archivos que debe tener el directorio
            int countArchivos = archivos.Count();
            if (countArchivos == 0)
            {
                msg = "No existen archivos con la extención raw dentro de la carpeta";
                type = ConstantesDpo.MsgWarning;

                return new { msg, type };

            }
            else if (countArchivos > 60)
            {
                msg = "No se pueden procesar un numero de archivos mayor a 60";
                type = ConstantesDpo.MsgWarning;

                return new { msg, type };
            }

            // Validar si existen archivos repetidos
            List<string> archivosManuales = new List<string>();
            foreach (FileData archivo in archivos)
            {
                archivosManuales.Add(archivo.FileName);
            }
            var duplicates = archivosManuales.GroupBy(x => x)
                            .SelectMany(g => g.Skip(1))
                            .Distinct()
                            .ToList();
            if (duplicates.Count > 0)
            {
                msg = "Existen archivos repetidos dentro de la carpeta";
                type = ConstantesDpo.MsgWarning;

                return new { msg, type };
            }

            // Validar si existen archivos con fecha distinta a la fecha de proceso
            string strCadFecha = string.Empty;
            List<string> archivosManualesFecha = new List<string>();
            foreach (FileData archivo in archivos)
            {   //20230221_0805_PSSEOutput.raw
                string sFechaArchivo = archivo.FileName.Substring(0, 8); //20230221
                if (sFechaArchivo != parseFecha.ToString("yyyyMMdd"))
                {
                    archivosManualesFecha.Add(archivo.FileName);
                    strCadFecha += "<p>" + archivo.FileName + " </p> ";
                }
            }
            if (archivosManualesFecha.Count > 0)
            {
                msg = strCadFecha;
                type = ConstantesDpo.MsgWarning;

                return new { msg, type };
            }

            try
            {
                /* Procesa los archivos raw FileData validos en DPO_ESTIMADORRAW_MANUAL */
                ProcesarDatosArchivoRaw(direccion, ConstantesProdem.EtmrawfntIeod, parseFecha, archivos, ConstantesDpo.CargaManualRaw, "Auto", string.Empty);

                // Inserta el archivo procesado
                foreach (FileData archivo in archivos)
                {
                    string[] nomFile = archivo.FileName.Split(new char[] { '_' });
                    string hora = nomFile[1].Substring(0, 2); //08
                    string minuto = nomFile[1].Substring(2, 2); //05

                    int k = Convert.ToInt32(hora) * 60 + Convert.ToInt32(minuto);
                    DateTime dFechaHoraArchivo = parseFecha.AddMinutes(k);
                    FactorySic.GetDpoEstimadorRawRepository().DeleteREstimadorRawFileByNomArchivo(archivo.FileName, ConstantesDpo.CargaManualRaw); //20230201_0805_PSSEOutput.raw
                    FactorySic.GetDpoEstimadorRawRepository().InsertFileRaw(archivo.FileName, dFechaHoraArchivo, ConstantesDpo.CargaManualRaw.ToString(), "1");
                }

                /* Actualizar con UPDATE la DPO_ESTIMADORRAW */
                // Se recorre el Cursor por Hora
                List<DpoEstimadorRawManualDTO> listEstimadorRawManualHora = FactorySic.GetDpoEstimadorRawRepository().ListEstimadorRawManualHora();
                foreach (DpoEstimadorRawManualDTO estimadorRawManualHora in listEstimadorRawManualHora)
                {
                    // Se recorre el cursor por minuto
                    List<DpoEstimadorRawManualDTO> listEstimadorRawManualMinuto = FactorySic.GetDpoEstimadorRawRepository().ListEstimadorRawManualMinuto();
                    foreach (DpoEstimadorRawManualDTO estimadorRawManualMinuto in listEstimadorRawManualMinuto)
                    {
                        string campoHNTablaRaw = string.Empty;
                        if (Convert.ToInt32(estimadorRawManualMinuto.Minuto) == 0)
                        {
                            campoHNTablaRaw = "dporawvalorh60";
                        }
                        else
                        {
                            campoHNTablaRaw = "dporawvalorh" + Convert.ToInt32(estimadorRawManualMinuto.Minuto);
                        }

                        // Formatea las fechas
                        DateTime fechaHraProc = DateTime.ParseExact(fechaHoraProceso + " " + estimadorRawManualHora.Hora + ":" + estimadorRawManualMinuto.Minuto,
                                                                    ConstantesDpo.FormatoFechaMedicionRaw,
                                                                    CultureInfo.InvariantCulture);

                        DateTime fechaHra = DateTime.ParseExact(fechaHoraProceso + " " + estimadorRawManualHora.Hora + ":" + "00",
                                                                ConstantesDpo.FormatoFechaMedicionRaw,
                                                                CultureInfo.InvariantCulture);

                        FactorySic.GetDpoEstimadorRawRepository().UpdateRawsProcesadosHoraManual(sufAnioMes, campoHNTablaRaw, fechaHraProc, fechaHra);
                    }
                }

                /* Elimina los registros de la tabla temporal */
                this.TruncateTemporalDpoEstimadorRaw(ConstantesDpo.CargaManualRaw);

                msg = "El proceso se ejecuto correctamente para los archivos localizados para el dia " + fechaHoraProceso;
                type = ConstantesDpo.MsgSuccess;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                type = ConstantesDpo.MsgError;

                return new { msg, type };
            }


            return new { msg, type };
        }

        #endregion

        #region Metodos Auxiliares
        /// <summary>
        /// Método que lee convierte cadena de nombres de los archivos del directoria a tipo FileData para su procesamiento
        /// </summary>
        /// <param name="archivosAProcesar">Lista de nombres de los archivos a procesar</param>
        /// <param name="ruta">Ruta de los nombres de los archivos a procesar</param>
        /// <param name="tipoProceso">Tipo de porceso</param>
        public List<FileData> ConvertirStringArchivosToFileData(List<string> archivosAProcesar, string ruta, string tipoProceso, string rutaRespaldo)
        {
            // Llena el listado de archivos
            List<FileData> archivos = new List<FileData>();
            bool isRespaldoActive = false;
            bool existeArchivo = false;
            string sArchivoAnterior = "";

            foreach (string strArchivo in archivosAProcesar)
            {
                if (sArchivoAnterior == strArchivo)
                {
                    continue;
                }
                sArchivoAnterior = strArchivo;

                DateTime fechaRegistro = DateTime.Now;

                if (tipoProceso == "1")
                {
                    // Extrae y formatea la fecha y hora del archivo
                    fechaRegistro = DateTime.ParseExact((strArchivo.Substring(6, 2) + "/" + // 15
                                                         strArchivo.Substring(4, 2) + "/" + // 12
                                                         strArchivo.Substring(0, 4) + " " + // 2022
                                                         strArchivo.Substring(9, 2) + ":" + // 13
                                                         strArchivo.Substring(11, 2)),      // 06
                                                         ConstantesProdem.FormatoFechaMedicionRaw,
                                                         CultureInfo.InvariantCulture);
                }
                else if (tipoProceso == "2")
                {
                    // Extrae y formatea la fecha y hora del archivo PSSE_202302011800
                    fechaRegistro = DateTime.ParseExact((strArchivo.Substring(11, 2) + "/" + // 01
                                                         strArchivo.Substring(9, 2) + "/" + // 02
                                                         strArchivo.Substring(5, 4) + " " + // 2022
                                                         strArchivo.Substring(13, 2) + ":" + // 18
                                                         strArchivo.Substring(15, 2)),      // 00
                                                         ConstantesProdem.FormatoFechaMedicionRaw,
                                                         CultureInfo.InvariantCulture);
                }
                
                existeArchivo = FileServerScada.VerificarExistenciaFile(string.Empty, strArchivo, ruta);

                if (!existeArchivo && rutaRespaldo != string.Empty)
                {
                    existeArchivo = FileServerScada.VerificarExistenciaFile(string.Empty, strArchivo, rutaRespaldo);
                    isRespaldoActive = true;
                }

                if (existeArchivo)
                {
                    FileData entityFile = FileServerScada.ObtenerArchivoEspecifico(isRespaldoActive ? rutaRespaldo : ruta, strArchivo);
                    //Validamos el tamaño del archivo
                    string[] aFile = entityFile.FileSize.Split(' '); //[0] tamaño , [1] unidad
                    decimal dTamanioFile = aFile[0] == "" ? 0 : Convert.ToDecimal(aFile[0]);
                    string sUnidad = aFile[1].ToUpper();
                    if (sUnidad.Equals("KB") && dTamanioFile <= 10)
                    {   // El archivo esta corrupto: se inserta en DPO_ESTIMADORRAW_LOG y DPO_ESTIMADORRAW_FILE de no procesados
                        FactorySic.GetDpoEstimadorRawRepository().InsertLogRaw(strArchivo, fechaRegistro, tipoProceso);
                        FactorySic.GetDpoEstimadorRawRepository().InsertFileRaw(strArchivo, fechaRegistro, tipoProceso, "0");
                        Logger.Info("[Serv:1537] El Archivo:" + strArchivo + " está corrupto, su tamaño es de " + entityFile.FileSize);
                    }
                    else
                    {
                        archivos.Add(entityFile);
                        Logger.Info("[Serv:1542] El archivo :" + strArchivo + " [" + entityFile.FileSize + "] esta listo para su lectura");
                    }
                }
                else
                {   // Si el archivo no existe fisicamente inserta en DPO_ESTIMADORRAW_LOG y DPO_ESTIMADORRAW_FILE de no procesado
                    FactorySic.GetDpoEstimadorRawRepository().InsertLogRaw(strArchivo, fechaRegistro, tipoProceso);
                    FactorySic.GetDpoEstimadorRawRepository().InsertFileRaw(strArchivo, fechaRegistro, tipoProceso, "0");
                    Logger.Info("[Serv:1549] El Archivo:" + strArchivo + " no existe, se almacena en tabla de Log para cargarlo mas adelante");
                }
            }

            // Guarda en log el error
            if (archivos.Count == 0)
            {
                Logger.Error("[Serv:1556] ObtenerDatosArchivoRaw: No se encontraron archivos");
            }

            return archivos;
        }

        /// <summary>
        /// Método que lee la tabla DPO_ESTIMADORRAW_FILES para generar una matriz que alimentara la barra de estado del proceso
        /// </summary>
        public object ObtenerMatrizBarraEstadoO(DateTime fechaProceso)
        {
            /* Se buscan todos los archivos raw generados y no generados por el COES, en la tabla HISTORICA */
            List<DpoEstimadorRawFilesDTO> listFilesRawAuto = FactorySic.GetDpoEstimadorRawRepository().ListFilesRaw(1, fechaProceso);
            List<DpoEstimadorRawFilesDTO> listFilesRawIeod = FactorySic.GetDpoEstimadorRawRepository().ListFilesRaw(2, fechaProceso);
            List<DpoEstimadorRawFilesDTO> listFilesRawManual = FactorySic.GetDpoEstimadorRawRepository().ListFilesRaw(3, fechaProceso);

            /* Arma la matriz de estados de proceso Automatico x Minuto de los archivos Raw en base a los datos obtenidos de la tabla
             * HISTORICA*/
            int i = 0;
            string[] arregloAuto = new string[1441];
            string[] arregloAutoNombre = new string[1441];
            string[] arregloIeod = new string[1441];
            string[] arregloIeodNombre = new string[1441];
            string[] arregloManual = new string[1441];
            string[] arregloManualNombre = new string[1441];

            foreach (DpoEstimadorRawFilesDTO filesRawAuto in listFilesRawAuto)
            {
                if (i > 1440) break;
                arregloAuto[i] = filesRawAuto.Flag;
                arregloAutoNombre[i] = filesRawAuto.NomArchivoRaw;
                arregloIeod[i] = "2";
                arregloManual[i] = "2";
                i++;
            }

            /* Arma la matriz de estados de proceso Automatico Ieod x 30 minutos de los archivos Raw en base a los datos obtenidos 
             * de la tabla HISTORICA*/
            int j = 0;
            foreach (DpoEstimadorRawFilesDTO filesRawIeod in listFilesRawIeod)
            {
                //Formato del nombre del archivo: PSSE_202302011335.raw
                string[] nomFile = filesRawIeod.NomArchivoRaw.Split(new char[] { '_' });
                string hora = nomFile[1].Substring(8, 2); //13 horas
                string minuto = nomFile[1].Substring(10, 2); //35 minutos

                j = Convert.ToInt32(hora) * 60 + Convert.ToInt32(minuto);
                if (j > 0)
                    j--;
                else
                    j = 24 * 60 - 1;
                //j = 60; // 01:00 == 01 * 60 + 00 = 60     02 * 60 + 30 = 150
                arregloIeodNombre[j] = (filesRawIeod.NomArchivoRaw == "null" ? "" : filesRawIeod.NomArchivoRaw);
                arregloIeod[j] = filesRawIeod.Flag;
                //j++;
            }

            /* Arma la matriz de estados de proceso Automatico MANUAL de los archivos Raw en base a los datos obtenidos de la tabla
             * HISTORICA*/
            int k = 0;
            foreach (DpoEstimadorRawFilesDTO filesRawManual in listFilesRawManual)
            {   //20230207_1415_PSSEOutput.raw
                string[] nomFile = filesRawManual.NomArchivoRaw.Split(new char[] { '_' });
                string hora = nomFile[1].Substring(0, 2); //08
                string minuto = nomFile[1].Substring(2, 2); //05

                k = Convert.ToInt32(hora) * 60 + Convert.ToInt32(minuto);
                if (k > 0)
                    k--;
                else
                    k = 24 * 60 - 1;
                arregloManual[k] = filesRawManual.Flag;
                arregloManualNombre[k] = (filesRawManual.NomArchivoRaw == "null" ? "" : filesRawManual.NomArchivoRaw);
                //k++;
            }

            object[] res = new object[3];
            res[0] = new
            {
                tipo = 1,
                id = "statusAuto",
                array = arregloAuto,
                arrayNombre = arregloAutoNombre
            };

            res[1] = new
            {
                tipo = 2,
                id = "statusIeod",
                array = arregloIeod,
                arrayNombre = arregloIeodNombre
            };

            res[2] = new
            {
                tipo = 3,
                id = "statusManual",
                array = arregloManual,
                arrayNombre = arregloManualNombre
            };

            return res;
        }

        /// <summary>
        /// Listar registros de DpoEstimadorRaw
        /// </summary>
        /// <returns></returns>
        public List<DpoEstimadorRawFilesDTO> ListFilesRawPorMinuto()
        {
            return FactorySic.GetDpoEstimadorRawRepository().ListFilesRawPorMinuto();
        }

        /// <summary>
        /// Listar registros de DpoEstimadorRaw
        /// </summary>
        /// <returns></returns>
        public List<DpoEstimadorRawFilesDTO> ListFilesRawIeod()
        {
            return FactorySic.GetDpoEstimadorRawRepository().ListFilesRawIeod();
        }

        /// <summary>
        /// Permite generar el reporte en formato Excel de archivos Raw no procesados
        /// </summary>
        /// <param name="listaRawsNoProcesadosXMinuto">Lista</param>
        /// <param name="listaRawsNoProcesadosIeod">Lista</param>
        /// <param name="path">Ruta del archivo Excel</param>
        /// <param name="fileName">Nombre del archivo Excel</param>
        public void ExportarToExcelRawsNoProcesados(List<DpoEstimadorRawFilesDTO> listaRawsNoProcesadosXMinuto,
                                                    List<DpoEstimadorRawFilesDTO> listaRawsNoProcesadosIeod,
                                                    string path,
                                                    string fileName)
        {
            string file = path + fileName;
            FileInfo newFile = new FileInfo(file);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("RAWS NO PROCESADOS");

                if (ws != null)
                {
                    // --------------------------------------------------------------------------------------------------------------
                    ws.Cells[2, 2].Value = "REPORTE DE RAWS NO PROCESADOS X MINUTO";

                    ExcelRange rg = ws.Cells[2, 2, 2, 2];
                    rg.Style.Font.Size = 13;
                    rg.Style.Font.Bold = true;

                    int index = 5;

                    ws.Cells[index, 2].Value = "Nombre Archivo Raw";
                    ws.Cells[index, 3].Value = "Fecha Archivo";

                    rg = ws.Cells[index, 2, index, 3];
                    rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rg.Style.Font.Color.SetColor(Color.White);
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Bold = true;

                    index = 6;
                    foreach (DpoEstimadorRawFilesDTO item in listaRawsNoProcesadosXMinuto)
                    {
                        ws.Cells[index, 2].Value = item.NomArchivoRaw;
                        ws.Cells[index, 3].Value = ((DateTime)item.FechaArchivoRaw).ToString(ConstantesAppServicio.FormatoFechaHora);

                        rg = ws.Cells[index, 2, index, 3];
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                        index++;
                    }

                    rg = ws.Cells[5, 2, index - 1, 3];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    ws.Column(2).Width = 80;
                    ws.Column(3).Width = 80;

                    rg = ws.Cells[5, 3, index, 3];
                    rg.AutoFitColumns();

                    // --------------------------------------------------------------------------------------------------------------
                    ws.Cells[2, 5].Value = "REPORTE DE RAWS NO PROCESADOS EIOD";

                    ExcelRange rgE = ws.Cells[2, 5, 5, 5];
                    rgE.Style.Font.Size = 13;
                    rgE.Style.Font.Bold = true;

                    int indexE = 5;

                    ws.Cells[indexE, 5].Value = "Nombre Archivo Raw";
                    ws.Cells[indexE, 6].Value = "Fecha Archivo";

                    rgE = ws.Cells[indexE, 5, indexE, 6];
                    rgE.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rgE.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rgE.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rgE.Style.Font.Color.SetColor(Color.White);
                    rgE.Style.Font.Size = 10;
                    rgE.Style.Font.Bold = true;

                    indexE = 6;
                    foreach (DpoEstimadorRawFilesDTO item in listaRawsNoProcesadosIeod)
                    {
                        ws.Cells[indexE, 5].Value = item.NomArchivoRaw;
                        ws.Cells[indexE, 6].Value = ((DateTime)item.FechaArchivoRaw).ToString(ConstantesAppServicio.FormatoFechaHora);

                        rgE = ws.Cells[indexE, 5, indexE, 6];
                        rgE.Style.Font.Size = 10;
                        rgE.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                        indexE++;
                    }

                    rgE = ws.Cells[5, 5, indexE - 1, 6];
                    rgE.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rgE.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgE.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rgE.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgE.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rgE.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgE.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rgE.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    ws.Column(5).Width = 120;
                    ws.Column(6).Width = 80;

                    rgE = ws.Cells[5, 5, indexE, 6];
                    rgE.AutoFitColumns();
                    // --------------------------------------------------------------------------------------------------------------

                    // Extrae el logo del COES
                    //HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    //System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    //ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                    //picture.From.Column = 1;
                    //picture.From.Row = 1;
                    //picture.To.Column = 2;
                    //picture.To.Row = 2;
                    //picture.SetSize(120, 35);
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Método que verifica si existen un numero de horas que no se han subido a la tabla DPO_ESTIMADORRAW_
        /// </summary>
        /// <param name="fechaProceso">Fecha de consulta</param>
        /// <returns>Numero de horas faltantes de cargar</returns>
        public int verificarFaltantesDia(DateTime fechaProceso)
        {
            int iNumregistros = 0;
            List<DpoEstimadorRawDTO> list = FactorySic.GetDpoEstimadorRawRepository().verificarFaltantesDia(fechaProceso);
            if (list.Count > 0)
                iNumregistros = list.Count;
            return iNumregistros;
        }

        /// <summary>
        /// Método que completa las horas que no se han subido a la tabla DPO_ESTIMADORRAW_
        /// </summary>
        /// <param name="fechaProceso">Fecha de consulta</param>
        /// <returns></returns>
        public int completarDia(DateTime fechaProceso)
        {
            int iNumregistros = 0;
            List<DpoEstimadorRawDTO> lista = FactorySic.GetDpoEstimadorRawRepository().verificarFaltantesDia(fechaProceso);
            foreach (DpoEstimadorRawDTO dto in lista)
            {
                //Para cada fecha
                DateTime dFecha = dto.Dporawfecha; //20/06/2023 17:00 => Migrar 17:01 a 18:00
                /* PASO 2 - Migrar los registros de la tabla temporal a la tabla principal [DPO_ESTIMADORRAW_TMP -> DPO_ESTIMADORRAW] 
                     * el formato fecha dd/mm/yyyy hh:00 */
                string sufAnioMes = dFecha.ToString("yyyyMM"); // Se le asignara a la tabla correspondiente del año mes: DPO_ESTIMADORRAW_YYYYMM
                DateTime hoyMinutoInicio = dFecha.AddMinutes(1); // para que inicie en el minuto 01 -> 17:01
                DateTime dFecha60 = dFecha.AddHours(1); //indica la hora final -> 18:00
                Logger.Info("[Srvc:1870] completarDia: MigrarRawsProcesadosHora / DPO_ESTIMADORRAW_TMP -> DPO_ESTIMADORRAW_YYYYMM [INICIO] " + dFecha.ToString("dd/MM/yyyy HH:mm"));
                FactorySic.GetDpoEstimadorRawRepository().MigrarRawsProcesadosHora(sufAnioMes,
                                                                                   dFecha.ToString(ConstantesDpo.FormatoFechaMedicionRaw),
                                                                                   dFecha.ToString(ConstantesDpo.FormatoHoraMinuto).Substring(0, 2),
                                                                                   dFecha60, "tmp");
                Logger.Info("[Srvc:1875] completarDia: MigrarRawsProcesadosHora / DPO_ESTIMADORRAW_TMP -> DPO_ESTIMADORRAW_YYYYMM [OK] " + dFecha.ToString("dd/MM/yyyy HH:mm"));
                /* PASO 3 - Elimina los registros de la tabla DPO_ESTIMADORRAW_TMP */
                this.DeleteREstimadorRawTemporalByDiaProceso(hoyMinutoInicio, dFecha60);
                iNumregistros++;
            }
            return iNumregistros;
        }

        /// <summary>
        /// Método que completa las horas faltantes migrando hora por hora
        /// </summary>
        /// <param name="fechaProceso">Fecha de consulta</param>
        /// <returns></returns>
        public int completarDiaMigrando(DateTime fechaProceso)
        {
            int iNumregistros = 0;
            List<DpoEstimadorRawDTO> lista = FactorySic.GetDpoEstimadorRawRepository().verificarFaltantesDia(fechaProceso);
            foreach (DpoEstimadorRawDTO dto in lista)
            {
                //Para cada fecha
                DateTime hora = dto.Dporawfecha; //20/06/2023 17:00 => Migrar 17:01 a 18:00
                DateTime horaInicio = hora.AddMinutes(1); //Inicia en el minuto 17:01
                DateTime horaFin = hora.AddMinutes(60); //termina en el minuto 18:00 la hora siguiente
                int diaProceso = horaInicio.Day;
                int iTipo = 1;
                // PASO 1: Eliminamos los archivos dpo_estimadorraw_files TIPO = 1 and fecha entre horaInicio, horaFin
                DeleteREstimadorRawFileByDiaProceso(horaInicio, horaFin, iTipo);

                // PASO 2: Delete DPO_ESTIMADOR_RAW_LOG WHERE TIPO = 1 and fecha entre horaInicio, horaFin
                DeleteREstimadorRawLogByDiaProceso(horaInicio, horaFin, iTipo);

                //PASO 3: Delete DPO_ESTIMADORRAW_MANUAL WHERE fecha entre hoyInicio, hoyFin
                TruncateTemporalDpoEstimadorRaw(ConstantesDpo.CargaManualRaw);

                string sProcedimiento = "Manual";
                hora = hora.AddMinutes(5);
                //PASO 4: Insertamos todos los TNA de la hora
                for (int i = 0; i < 60; i += 5)
                {
                    DateTime parseFecha = hora.AddMinutes(i);
                    Logger.Info("[Srvc:1915] Completando completarDiaMigrando:  " + parseFecha.ToString("dd/MM/yyyy HH:mm"));
                    EjecutaProcesoLecturaRawPorMinuto(parseFecha, diaProceso, sProcedimiento);
                }
                iNumregistros++;
            }
            return iNumregistros;
        }

        /// <summary>
        /// Reconstruye los indices de la tabla dpo_estimadorraw
        /// </summary>
        /// <param name="fechaProceso">Fecha de consulta</param>
        /// <returns>OK / Error</returns>
        public string UpdateRAW(DateTime fechaProceso)
        {
            string sMensaje = FactorySic.GetDpoEstimadorRawRepository().UpdateRAW(fechaProceso);
            return sMensaje;
        }

        /// <summary>
        /// Reconstruye los indices de la tabla dpo_estimadorraw_tmp
        /// </summary>
        /// <returns>OK / Error</returns>
        public string UpdateTMP()
        {
            string sMensaje = FactorySic.GetDpoEstimadorRawRepository().UpdateTMP();
            return sMensaje;
        }

        /// <summary>
        /// Trunca la tabla dpo_estimadorraw_manual y cmtmp y elimina la data TMP
        /// </summary>
        /// <returns>OK / Error</returns>
        public string DeleteEstimadorRawTemporal()
        {
            string sMensaje = FactorySic.GetDpoEstimadorRawRepository().DeleteEstimadorRawTemporal();
            return sMensaje;
        }
        #endregion

        #endregion

        #region Métodos del procesamientos de datos de los archivos RAW
        /// <summary>
        /// Método que lee y procesa los archivos raw de un directorio
        /// </summary>
        /// <param name="ruta">Ubicación de los archivos</param>
        /// <param name="fuente">Fuente de la lectura (manual o auto)</param>
        /// <param name="fechaImportacion">Fecha para la importación(solo manual)</param>
        /// <param name="archivos">Lista de archivos a procesar</param>
        /// <param name="proceso">Tipo de proceso</param> 
        /// <param name="sProcedimiento">Auto/Manual</param>
        public void ProcesarDatosArchivoRaw(string ruta,
                                            int fuente,
                                            DateTime fechaImportacion,
                                            List<FileData> archivos,
                                            int proceso,
                                            string sProcedimiento,
                                            string rutaRespaldo)
        {
            List<string> arrayData;
            List<string[]> dataBarras, dataCargas, dataShunts,
                           dataGeneradores, dataLineas, dataTransformadores;

            List<MePtomedicionDTO> puntos = new List<MePtomedicionDTO>();
            List<DpoEstimadorRawTmpDTO> entidades = new List<DpoEstimadorRawTmpDTO>();

            //Iniciamos el proceso de lectura de los archivos raw
            foreach (FileData archivo in archivos)
            {
                Logger.Info("[Srvc:1984] Se va a procesar el archivo " + archivo.FileName);
                if (archivo.Extension != ".raw") continue;

                //ASSETEC: 20240610
                DpoEstimadorRawFilesDTO dtoEstimadoFile = FactorySic.GetDpoEstimadorRawRepository().GetFileRaw(archivo.FileName);
                if (dtoEstimadoFile != null) continue;

                string[] nombreArchivo;
                string strFechaRegistro = string.Empty;
                string strIntervaloMedicion = string.Empty;
                string strHoraMinutoRegistro = string.Empty;
                DateTime fechaRegistro = new DateTime();
                entidades = new List<DpoEstimadorRawTmpDTO>();

                // -----------------------------------------------------------------------------------------
                // Demanda automatica
                // -----------------------------------------------------------------------------------------
                if (fuente == ConstantesDpo.EtmrawfntSco)
                {
                    puntos = this.ListPtomedicionByOriglectcodi(ConstantesDpo.OriglectcodiTnaSco);

                    nombreArchivo = archivo.FileName.Split(new char[] { '_', '.' });

                    if (proceso == ConstantesDpo.CargaAutoRawXMinuto)
                    {
                        strFechaRegistro = nombreArchivo[0].Substring(0, 8);
                        strIntervaloMedicion = nombreArchivo[1].Insert(2, ":");
                    }


                    if (proceso == ConstantesDpo.CargaAutoRawX30Minutos)
                    {
                        strFechaRegistro = nombreArchivo[1].Substring(0, 8);
                        strHoraMinutoRegistro = nombreArchivo[1].Substring(8, 4);
                        strIntervaloMedicion = strHoraMinutoRegistro.Insert(2, ":");
                    }

                    fechaRegistro = DateTime.ParseExact(strFechaRegistro,
                                                        ConstantesDpo.FormatoFechaArchivoRaw,
                                                        CultureInfo.InvariantCulture);
                }
                // -----------------------------------------------------------------------------------------

                // -----------------------------------------------------------------------------------------
                // Demanda manual
                // -----------------------------------------------------------------------------------------
                if (fuente == ConstantesProdem.EtmrawfntIeod)
                {
                    puntos = this.ListPtomedicionByOriglectcodi(ConstantesDpo.OriglectcodiTnaSco); //OriglectcodiTnaIeod

                    nombreArchivo = archivo.FileName.Split(new char[] { '_', '.' });

                    strFechaRegistro = nombreArchivo[0].Substring(0, 8);
                    strIntervaloMedicion = nombreArchivo[1].Insert(2, ":");

                    fechaRegistro = fechaImportacion;
                }
                // -----------------------------------------------------------------------------------------

                // Convierte texto fecha del archivo RAW a DateTime
                string strFechaMedicion = $"{fechaRegistro.ToString(ConstantesDpo.FormatoFecha)} {strIntervaloMedicion}";

                DateTime fechaMedicion = DateTime.ParseExact(strFechaMedicion,
                                                             ConstantesDpo.FormatoFechaMedicionRaw,
                                                             CultureInfo.InvariantCulture);

                // Inserta el archivo que se va a poder procesar en DPO_ESTIMADORRAW_FILES por que el archivo existe pero aun no se migra a TMP
                FactorySic.GetDpoEstimadorRawRepository().InsertFileRaw(archivo.FileName, fechaMedicion, proceso.ToString(), "0");

                //SE VA A PROCEDER A LEER EL ARCHIVO PARA SU POSTERIOR INSERCIÓN EN BD
                try
                {
                    // Carga todas las filas del archivo
                    arrayData = new List<string>();

                    using (StreamReader sr = FileServerScada.OpenReaderFile(archivo.FileName, ruta))
                    {
                        if (sr != null)
                        {
                            string s = "";
                            while ((s = sr.ReadLine()) != null) arrayData.Add(s);
                        }
                    }

                    if(arrayData.Count == 0)
                    {
                        using (StreamReader sr = FileServerScada.OpenReaderFile(archivo.FileName, rutaRespaldo))
                        {
                            if (sr != null)
                            {
                                string s = "";
                                while ((s = sr.ReadLine()) != null) arrayData.Add(s);
                            }
                        }
                    }

                    arrayData.RemoveRange(0, 3);

                    // Obtención de los datos de barras
                    int i = 0;
                    int iNroRegistros = 0;
                    dataBarras = new List<string[]>();
                    dataBarras = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueBarras, arrayData);
                    int totalBarras = dataBarras.Count;

                    // Validación de equipo desconectado
                    dataBarras = dataBarras
                                .Where(x => x[ConstantesProdem.indexEqDesconectado] != ConstantesProdem.valorEqDesconectado)
                                .ToList();

                    // Obtención de los datos de Cargas
                    i += totalBarras + 1;
                    dataCargas = new List<string[]>();
                    dataCargas = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueCargas, arrayData);

                    // Obtención de los datos de Shunts
                    i += dataCargas.Count + 1;
                    dataShunts = new List<string[]>();
                    dataShunts = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueShunts, arrayData);

                    // Obtención de los datos de Generadores
                    i += dataShunts.Count + 1;
                    dataGeneradores = new List<string[]>();
                    dataGeneradores = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueGeneradores, arrayData);

                    // Obtención de los datos de Lineas
                    i += dataGeneradores.Count + 1;
                    dataLineas = new List<string[]>();
                    dataLineas = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueLineas, arrayData);

                    // Obtención de los datos de Transformadores
                    i += dataLineas.Count + 1;
                    dataTransformadores = new List<string[]>();
                    dataTransformadores = UtilProdem.ObtenerBlqInfoRaw(i, ConstantesProdem.corteBloqueTransformadores, arrayData);

                    // Procesar datos de los archivos Raw
                    try
                    { entidades.AddRange(ProcesarDatosGeneradores(fuente, fechaMedicion, dataGeneradores, puntos)); }
                    catch
                    {
                        Logger.Error("[Srvc:2108] ERROR de formato para Generadores en " + archivo.FileName);
                    }
                    try { entidades.AddRange(ProcesarDatosCargas(fuente, fechaMedicion, dataCargas, puntos)); }
                    catch
                    {
                        Logger.Error("[Srvc:2113] ERROR de formato para Cargas en " + archivo.FileName);
                    }
                    try
                    { entidades.AddRange(ProcesarDatosShunts(fuente, fechaMedicion, dataShunts, puntos)); }
                    catch
                    {
                        Logger.Error("[Srvc:2119] ERROR de formato para Shunts en " + archivo.FileName);
                    }
                    try
                    { entidades.AddRange(ProcesarDatosBarras(fuente, fechaMedicion, dataCargas, dataBarras, puntos)); }
                    catch
                    {
                        Logger.Error("[Srvc:2125] ERROR de formato para Barra en " + archivo.FileName);
                    }
                    try
                    { entidades.AddRange(ProcesarDatosLineas(fuente, fechaMedicion, dataLineas, dataBarras, puntos)); }
                    catch
                    {
                        Logger.Error("[Srvc:2131] ERROR de formato para Lineas en " + archivo.FileName);
                    }
                    try
                    { entidades.AddRange(ProcesarDatosTransformadores(fuente, fechaMedicion, dataTransformadores, dataBarras, puntos)); }
                    catch
                    {
                        Logger.Error("[Srvc:2137] ERROR de formato para Transformadores en " + archivo.FileName);
                    }
                    try
                    {
                        if (entidades.Count > 0)
                        {
                            /* Registra las mediciones en las tablas temporales -> proceso: 
                             * [1] TNA x Minuto -> DPO_ESTIMADORRAW_TMP 
                             * [2] IEOD x 30 Minutos -> DPO_ESTIMADORRAW_CMTMP 
                             * [3] Manual -> DPO_ESTIMADORRAW_MANUAL */
                            Logger.Info("[Srvc:2147] BulkInsertDpoEstimadorRaw - INICIO [ " + entidades.Count + " registros] - para " + archivo.FileName);
                            this.BulkInsertDpoEstimadorRaw(entidades, proceso, sProcedimiento);
                            Logger.Info("[Srvc:2149] BulkInsertDpoEstimadorRaw - OK - para " + archivo.FileName);

                            //Actualizamos el registro en la Tabla DPO_ESTIMADORRAW_FILES, en donde se valida la correcta carga del archivo
                            FactorySic.GetDpoEstimadorRawRepository().UpdateFileRaw(archivo.FileName, fechaMedicion, proceso.ToString(), "1");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Como hubo un problema con importar el archivo, lo reportamos al log para que lo intente cargar mas tarde, antes de migrar la hora.
                        FactorySic.GetDpoEstimadorRawRepository().InsertLogRaw(archivo.FileName, fechaRegistro, proceso.ToString());
                        Logger.Error("[Srvc:2159] No se ejecuto el BulkInsert en DPO_ESTIMADORRAW_TMP: " + ex.Message);
                    }
                    i++;
                }
                catch (Exception ex)
                {
                    Logger.Error("[Srvc:2165] " + archivos[0].FileName + " [StackTrace]: " + ex.StackTrace);
                    Logger.Error("[Srvc:2166] " + archivos[0].FileName + " [Message]: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Procesa los datos de GENERADORES leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataRaw">Datos de generadores</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<DpoEstimadorRawTmpDTO> ProcesarDatosGeneradores(int fuente,
                                                                    DateTime fechaMedicion,
                                                                    List<string[]> dataRaw,
                                                                    List<MePtomedicionDTO> listaPuntos)
        {
            List<DpoEstimadorRawTmpDTO> entities = new List<DpoEstimadorRawTmpDTO>();

            int indexPotenciaActiva = 2;
            int indexPotenciaReactiva = 3;
            int indexNombre = 28;

            // Nuevo campo Estado
            int indexGeneradorEstado = 14;

            int idUnidad;
            DpoEstimadorRawTmpDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicion;
            decimal valorPotenciaActiva, valorPotenciaReactiva;

            // Nuevo campo Estado
            decimal valorEstado;

            foreach (string[] r in dataRaw)
            {
                valorPotenciaActiva = PasarDecimalCero(r[indexPotenciaActiva]);
                valorPotenciaReactiva = PasarDecimalCero(r[indexPotenciaReactiva]);
                
                // Nuevo campo Estado
                valorEstado = PasarDecimalCero(r[indexGeneradorEstado]);

                entidadPtoMedicion = new MePtomedicionDTO();
                entidadPtoMedicion.Ptomedidesc = r[indexNombre].Trim();
                entidadPtoMedicion.Codref = ConstantesProdem.EtmrawtpGenerador;

                if (fuente == ConstantesProdem.EtmrawfntIeod)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                if (fuente == ConstantesProdem.EtmrawfntSco)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;

                idUnidad = RegistrarPtoMedicionRaw(entidadPtoMedicion, listaPuntos);

                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.GeneradorPotActivaMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpGenerador,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotenciaActiva
                };
                entities.Add(entidadMedicion);

                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.GeneradorPotReactivaMVAR,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpGenerador,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotenciaReactiva
                };
                entities.Add(entidadMedicion);

                // Nuevo campo Estado
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.GeneradorEstado, // Constante estado
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpGenerador,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorEstado // Estado
                };
                entities.Add(entidadMedicion);
            }
            return entities;
        }

        /// <summary>
        /// Procesa los datos de CARGAS leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataRaw">Datos de cargas</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<DpoEstimadorRawTmpDTO> ProcesarDatosCargas(int fuente,
                                                                DateTime fechaMedicion,
                                                                List<string[]> dataRaw,
                                                                List<MePtomedicionDTO> listaPuntos)
        {
            List<DpoEstimadorRawTmpDTO> entities = new List<DpoEstimadorRawTmpDTO>();

            int indexPotenciaActiva = 5;
            int indexPotenciaReactiva = 6;
            int indexNombre = 13;

            // Nuevo campo Estado
            int indexPotenciaEstado = 2;

            int idUnidad;
            DpoEstimadorRawTmpDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicion;
            decimal valorPotenciaActiva, valorPotenciaReactiva;

            // Nuevo campo Estado
            decimal valorEstado;

            foreach (string[] r in dataRaw)
            {
                valorPotenciaActiva = PasarDecimalCero(r[indexPotenciaActiva]);
                valorPotenciaReactiva = PasarDecimalCero(r[indexPotenciaReactiva]);

                // Nuevo campo Estado
                valorEstado = PasarDecimalCero(r[indexPotenciaEstado]);

                entidadPtoMedicion = new MePtomedicionDTO();
                entidadPtoMedicion.Ptomedidesc = r[indexNombre].Trim();
                entidadPtoMedicion.Codref = ConstantesProdem.EtmrawtpCarga;
                if (fuente == ConstantesProdem.EtmrawfntIeod)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                if (fuente == ConstantesProdem.EtmrawfntSco)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;

                idUnidad = RegistrarPtoMedicionRaw(entidadPtoMedicion, listaPuntos);

                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.CargaPotActivaMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpCarga,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotenciaActiva
                };
                entities.Add(entidadMedicion);

                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.CargaPotReactivaMVAR,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpCarga,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotenciaReactiva
                };
                entities.Add(entidadMedicion);

                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.LineaEstado,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpCarga,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorEstado
                };
                entities.Add(entidadMedicion);
            }
            return entities;
        }

        /// <summary>
        /// Procesa los datos de SHUNTS leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataRaw">Datos de shunts</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<DpoEstimadorRawTmpDTO> ProcesarDatosShunts(int fuente,
                                                                DateTime fechaMedicion,
                                                                List<string[]> dataRaw,
                                                                List<MePtomedicionDTO> listaPuntos)
        {
            List<DpoEstimadorRawTmpDTO> entities = new List<DpoEstimadorRawTmpDTO>();

            int indexC3 = 2;
            int indexPotenciaReactiva = 4;
            int indexNombre = 5;

            // Nuevo campo Estado
            int indexShuntEstado = 2;

            int idUnidad;
            DpoEstimadorRawTmpDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicion;
            decimal valorPotenciaReactiva, valorC3;

            // Nuevo campo Estado
            decimal valorEstado;

            foreach (string[] r in dataRaw)
            {
                valorC3 = PasarDecimalCero(r[indexC3]);
                valorPotenciaReactiva = PasarDecimalCero(r[indexPotenciaReactiva]);
                valorPotenciaReactiva = valorPotenciaReactiva * valorC3;
                valorPotenciaReactiva = Math.Round(valorPotenciaReactiva, 4);

                // Nuevo campo Estado
                valorEstado = PasarDecimalCero(r[indexShuntEstado]);

                entidadPtoMedicion = new MePtomedicionDTO();
                entidadPtoMedicion.Ptomedidesc = r[indexNombre].Trim();
                entidadPtoMedicion.Codref = ConstantesProdem.EtmrawtpShunt;
                if (fuente == ConstantesProdem.EtmrawfntIeod)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                if (fuente == ConstantesProdem.EtmrawfntSco)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;

                idUnidad = RegistrarPtoMedicionRaw(entidadPtoMedicion, listaPuntos);

                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.ShuntPotReactivaMVAR,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpShunt,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotenciaReactiva
                };
                entities.Add(entidadMedicion);

                // Nuevo campo estado
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.ShuntEstado,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpShunt,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorEstado
                };
                entities.Add(entidadMedicion);
            }

            return entities;
        }

        /// <summary>
        /// Procesa los datos de BARRAS leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataCargas">Datos de cargas</param>
        /// <param name="dataBarras">Datos de barras</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<DpoEstimadorRawTmpDTO> ProcesarDatosBarras(int fuente,
                                                                DateTime fechaMedicion,
                                                                List<string[]> dataCargas,
                                                                List<string[]> dataBarras,
                                                                List<MePtomedicionDTO> listaPuntos)
        {
            List<DpoEstimadorRawTmpDTO> entities = new List<DpoEstimadorRawTmpDTO>();

            int indexDemandaActiva = 5;
            int indexDemandaReactiva = 6;
            int indexTensionPU = 7;
            int indexTensionNominal = 2;
            int indexAngulo = 8;
            int indexNombre = 1;
            int indexId = 0;

            //// Nuevo campo Estado
            //int indexBarraEstado = 7;

            int idUnidad;
            DpoEstimadorRawTmpDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicion;
            decimal valorDemandaActiva, valorDemandaReactiva, valorTensionKV;

            //// Nuevo campo Estado
            //decimal valorEstado;

            List<string> listaBarras = dataBarras
                .Select(x => x[indexNombre].Trim(new char[] { ' ', '\'' }))
                .Distinct()
                .ToList();

            foreach (string nomBarra in listaBarras)
            {
                entidadPtoMedicion = new MePtomedicionDTO();
                entidadPtoMedicion.Ptomedidesc = nomBarra;
                entidadPtoMedicion.Codref = ConstantesProdem.EtmrawtpBarra;

                if (fuente == ConstantesProdem.EtmrawfntIeod)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                if (fuente == ConstantesProdem.EtmrawfntSco)
                    entidadPtoMedicion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;

                idUnidad = RegistrarPtoMedicionRaw(entidadPtoMedicion, listaPuntos);

                //// Nuevo campo Estado
                //valorEstado = 0;

                // Obtiene las barras con el mismo nombre si existieran
                List<string[]> grupoBarras = dataBarras
                    .Where(x => x[indexNombre].Trim(new char[] { ' ', '\'' }) == nomBarra)
                    .ToList();
                int totalRegistros = grupoBarras.Count;

                // Cálculo de Tensión en KV
                decimal tensionPU = 0;
                decimal tensionNominal = 0;
                foreach (string[] x in grupoBarras)
                {
                    tensionPU += PasarDecimalCero(x[indexTensionPU]);
                    tensionNominal += PasarDecimalCero(x[indexTensionNominal]);
                }
                tensionPU = tensionPU / totalRegistros;
                tensionNominal = tensionNominal / totalRegistros;

                // Ángulo
                decimal anguloBarra = 0;
                foreach (string[] x in grupoBarras)
                {
                    anguloBarra += PasarDecimalCero(x[indexAngulo]);
                }
                anguloBarra = anguloBarra / totalRegistros;

                // Cálculo de Demandas
                valorDemandaActiva = 0;
                valorDemandaReactiva = 0;
                List<string> idBarras = grupoBarras
                    .Select(x => x[indexId].Trim())
                    .ToList();
                foreach (string[] rCarga in dataCargas)
                {
                    if (idBarras.Contains(rCarga[indexId].Trim()))
                    {
                        valorDemandaActiva = PasarDecimalCero(rCarga[indexDemandaActiva]);
                        valorDemandaReactiva = PasarDecimalCero(rCarga[indexDemandaReactiva]);
                    }

                    //// Nuevo campo Estado
                    //valorEstado = decimal.Parse(rCarga[indexBarraEstado]);
                }

                #region Agrega los valores a la lista de entidades para el registro
                // Demanda Activa
                valorDemandaActiva = Math.Round(valorDemandaActiva, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.BarraDemActivaMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpBarra,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorDemandaActiva
                };
                entities.Add(entidadMedicion);

                // Demanda Reactiva
                valorDemandaReactiva = Math.Round(valorDemandaReactiva, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.BarraDemReactivaMVAR,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpBarra,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorDemandaReactiva
                };
                entities.Add(entidadMedicion);

                // Cálculo de Tensión en KV
                valorTensionKV = tensionPU * tensionNominal;
                valorTensionKV = Math.Round(valorTensionKV, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.BarraTensionKv,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpBarra,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorTensionKV
                };
                entities.Add(entidadMedicion);

                // Ángulo
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidad,
                    Prnvarcodi = ConstantesProdem.BarraAngulo,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpBarra,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = anguloBarra
                };
                entities.Add(entidadMedicion);

                //// Nuevo campo estado
                //entidadMedicion = new DpoEstimadorRawTmpDTO
                //{
                //    Ptomedicodi = idUnidad,
                //    Dpovarcodi = ConstantesProdem.BarraEstado,
                //    Dporawfuente = fuente,
                //    Dporawtipomedi = ConstantesProdem.EtmrawtpBarra,
                //    Dporawfecha = fechaMedicion,
                //    Dporawvalor = valorEstado
                //};
                //entities.Add(entidadMedicion);
                #endregion
            }

            return entities;
        }

        /// <summary>
        /// Procesa los datos de LINEAS leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataLineas">Datos de lineas</param>
        /// <param name="dataBarras">Datos de barras</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<DpoEstimadorRawTmpDTO> ProcesarDatosLineas(int fuente,
                                                                DateTime fechaMedicion,
                                                                List<string[]> dataLineas,
                                                                List<string[]> dataBarras,
                                                                List<MePtomedicionDTO> listaPuntos)
        {
            List<DpoEstimadorRawTmpDTO> entities = new List<DpoEstimadorRawTmpDTO>();

            // Índices de los datos de lineas
            int indexNombre = 24;
            int indexLineaBarraI = 0;//Una linea esta conectada a dos barras
            int indexLineaBarraJ = 1;
            int indexVarR = 3;
            int indexVarX = 4;
            int indexVarB = 5;
            int indexVarCap = 6;

            //Índices de los datos de barras
            int indexBarra = 0;
            int indexVarVkv = 2;
            int indexVarVpu = 7;
            int indexAngulo = 8;

            // Nuevo campo Estado
            int indexLineaEstado = 13;

            int idUnidadEnvio;
            int idUnidadRecepcion;

            DpoEstimadorRawTmpDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicionEnvio;
            MePtomedicionDTO entidadPtoMedicionRecepcion;

            decimal valorK1, valorK2, valorBase;
            decimal valorPotEnvioActiva, valorPotEnvioReactiva, valorPotEnvioAparente;
            decimal valorPotRecepcionActiva, valorPotRecepcionReactiva, valorPotRecepcionAparente;
            decimal valorPerdidasLineas;
            decimal valorCargabilidadMaxima;

            // Nuevo campo Estado
            decimal valorEstado;

            foreach (string[] rLinea in dataLineas)
            {
                entidadPtoMedicionEnvio = new MePtomedicionDTO();
                entidadPtoMedicionEnvio.Ptomedidesc = rLinea[indexNombre].Trim(new char[] { ' ', '\'' });
                entidadPtoMedicionEnvio.Ptomedidesc = entidadPtoMedicionEnvio.Ptomedidesc + ConstantesProdem.PrefijoEnvio;
                entidadPtoMedicionEnvio.Codref = ConstantesProdem.EtmrawtpLinea;

                // Nuevo campo Estado
                valorEstado = PasarDecimalCero(rLinea[indexLineaEstado]);

                entidadPtoMedicionRecepcion = new MePtomedicionDTO();
                entidadPtoMedicionRecepcion.Ptomedidesc = rLinea[indexNombre].Trim(new char[] { ' ', '\'' });
                entidadPtoMedicionRecepcion.Ptomedidesc = entidadPtoMedicionRecepcion.Ptomedidesc + ConstantesProdem.PrefijoRecepcion;
                entidadPtoMedicionRecepcion.Codref = ConstantesProdem.EtmrawtpLinea;

                if (fuente == ConstantesProdem.EtmrawfntIeod)
                {
                    entidadPtoMedicionEnvio.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                    entidadPtoMedicionRecepcion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                }
                if (fuente == ConstantesProdem.EtmrawfntSco)
                {
                    entidadPtoMedicionEnvio.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;
                    entidadPtoMedicionRecepcion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;
                }

                idUnidadEnvio = RegistrarPtoMedicionRaw(entidadPtoMedicionEnvio, listaPuntos);
                idUnidadRecepcion = RegistrarPtoMedicionRaw(entidadPtoMedicionRecepcion, listaPuntos);

                #region Procesa los datos
                decimal varR = PasarDecimalCero(rLinea[indexVarR]);
                decimal varX = PasarDecimalCero(rLinea[indexVarX]);
                decimal varB = PasarDecimalCero(rLinea[indexVarB]);
                decimal varCap = PasarDecimalCero(rLinea[indexVarCap]);

                // Calculo de la variable K1 y K2
                decimal dDenominador = ((varR * varR) + (varX * varX));
                if (dDenominador == 0) dDenominador = 1;
                valorK1 = varR / dDenominador;
                valorK2 = varX / dDenominador;

                // Obtención de varibles por barra
                decimal varIkv = 0;
                decimal varJkv = 0;
                decimal varIpu = 0;
                decimal varJpu = 0;
                double anguloI = 0;
                double anguloJ = 0;

                foreach (string[] rBarra in dataBarras)
                {
                    if (rLinea[indexLineaBarraI] == rBarra[indexBarra])
                    {
                        varIkv = PasarDecimalCero(rBarra[indexVarVkv]);
                        varIpu = PasarDecimalCero(rBarra[indexVarVpu]);
                        anguloI = double.Parse(rBarra[indexAngulo]);
                    }

                    if (rLinea[indexLineaBarraJ] == rBarra[indexBarra])
                    {
                        varJkv = PasarDecimalCero(rBarra[indexVarVkv]);
                        varJpu = PasarDecimalCero(rBarra[indexVarVpu]);
                        anguloJ = double.Parse(rBarra[indexAngulo]);
                    }
                }
                // Obtiene diferencia angular
                double difAnguloIJ = anguloI - anguloJ;
                difAnguloIJ = (difAnguloIJ * Math.PI) / 180;

                // Cálculo de la variable Base
                valorBase = (varIkv + varJkv) / 2;

                // Calculo de la Potencia de envío
                valorPotEnvioActiva = (-1 * valorK1) * (decimal)(Math.Cos(difAnguloIJ));
                valorPotEnvioActiva = valorPotEnvioActiva + (valorK2 * (decimal)(Math.Sin(difAnguloIJ)));
                valorPotEnvioActiva = valorPotEnvioActiva * varIpu * varJpu;
                valorPotEnvioActiva = valorPotEnvioActiva + (varIpu * varIpu * valorK1);
                valorPotEnvioActiva = valorPotEnvioActiva * 100;

                valorPotEnvioReactiva = (-1 * valorK1) * (decimal)(Math.Sin(difAnguloIJ));
                valorPotEnvioReactiva = valorPotEnvioReactiva - (valorK2 * (decimal)(Math.Cos(difAnguloIJ)));
                valorPotEnvioReactiva = valorPotEnvioReactiva * varIpu * varJpu;
                valorPotEnvioReactiva = valorPotEnvioReactiva + (varIpu * varIpu * (valorK2 - (varB / 2)));
                valorPotEnvioReactiva = valorPotEnvioReactiva * 100;

                valorPotEnvioAparente = valorPotEnvioActiva * valorPotEnvioActiva;
                valorPotEnvioAparente = valorPotEnvioAparente + (valorPotEnvioReactiva * valorPotEnvioReactiva);
                valorPotEnvioAparente = (decimal)Math.Sqrt((double)valorPotEnvioAparente);

                // Calculo de la Potencia de recepción
                valorPotRecepcionActiva = (-1 * valorK1) * (decimal)(Math.Cos(difAnguloIJ));
                valorPotRecepcionActiva = valorPotRecepcionActiva - (valorK2 * (decimal)(Math.Sin(difAnguloIJ)));
                valorPotRecepcionActiva = valorPotRecepcionActiva * varIpu * varJpu;
                valorPotRecepcionActiva = valorPotRecepcionActiva + (varJpu * varJpu * valorK1);
                valorPotRecepcionActiva = valorPotRecepcionActiva * 100;

                valorPotRecepcionReactiva = valorK1 * (decimal)(Math.Sin(difAnguloIJ));
                valorPotRecepcionReactiva = valorPotRecepcionReactiva - (valorK2 * (decimal)(Math.Cos(difAnguloIJ)));
                valorPotRecepcionReactiva = valorPotRecepcionReactiva * varIpu * varJpu;
                valorPotRecepcionReactiva = valorPotRecepcionReactiva + (varJpu * varJpu * (valorK2 - (varB / 2)));
                valorPotRecepcionReactiva = valorPotRecepcionReactiva * 100;

                valorPotRecepcionAparente = valorPotRecepcionActiva * valorPotRecepcionActiva;
                valorPotRecepcionAparente = valorPotEnvioAparente + (valorPotRecepcionReactiva * valorPotRecepcionReactiva);
                valorPotRecepcionAparente = (decimal)Math.Sqrt((double)valorPotRecepcionAparente);

                // Cálculo de las pérdidas en línea
                valorPerdidasLineas = valorPotEnvioActiva + valorPotRecepcionActiva;

                // Cálculo de cargabilidad máxima
                decimal porEnvio = (valorPotEnvioAparente / varCap) * 100;
                decimal porRecepcion = (valorPotRecepcionAparente / varCap) * 100;
                valorCargabilidadMaxima = (porEnvio > porRecepcion) ? porEnvio : porRecepcion;
                #endregion

                #region Carga la lista de entidades
                // a) Potencia de envio
                // a.1) activa
                valorPotEnvioActiva = Math.Round(valorPotEnvioActiva, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.LineaPotActivaMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotEnvioActiva
                };
                entities.Add(entidadMedicion);

                // a.2) reactiva
                valorPotEnvioReactiva = Math.Round(valorPotEnvioReactiva, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.LineaPotReactivaMVAR,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotEnvioReactiva
                };
                entities.Add(entidadMedicion);

                // a.3) aparente
                valorPotEnvioAparente = Math.Round(valorPotEnvioAparente, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.LineaPotAparenteMVA,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotEnvioAparente
                };
                entities.Add(entidadMedicion);

                // b) Potencia de recepción
                // b.1) activa
                valorPotRecepcionActiva = Math.Round(valorPotRecepcionActiva, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaPotActivaMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotRecepcionActiva
                };
                entities.Add(entidadMedicion);

                // b.2) reactiva
                valorPotRecepcionReactiva = Math.Round(valorPotRecepcionReactiva, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaPotReactivaMVAR,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotRecepcionReactiva
                };
                entities.Add(entidadMedicion);

                // b.3) aparente
                valorPotRecepcionAparente = Math.Round(valorPotRecepcionAparente, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaPotAparenteMVA,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotRecepcionAparente
                };
                entities.Add(entidadMedicion);

                // c.1) Pérdidas en línea envio
                valorPerdidasLineas = Math.Round(valorPerdidasLineas, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.LineaPerdidasMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPerdidasLineas
                };
                entities.Add(entidadMedicion);

                // c.2) Pérdidas en línea recepción
                valorPerdidasLineas = Math.Round(valorPerdidasLineas, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaPerdidasMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPerdidasLineas
                };
                entities.Add(entidadMedicion);

                // d.1) Cargabilidad máxima envio
                valorCargabilidadMaxima = Math.Round(valorCargabilidadMaxima, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.LineaCargaMaxima,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorCargabilidadMaxima
                };
                entities.Add(entidadMedicion);

                // d.2) Cargabilidad máxima recepción
                valorCargabilidadMaxima = Math.Round(valorCargabilidadMaxima, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaCargaMaxima,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorCargabilidadMaxima
                };
                entities.Add(entidadMedicion);

                // Nuevo campo estado
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.LineaEstado,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpLinea,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorEstado
                };
                entities.Add(entidadMedicion);
                #endregion
            }

            return entities;
        }

        /// <summary>
        /// Procesa los datos de TRANSFORMADORES leidos de los archivos RAW
        /// </summary>
        /// <param name="fuente">Fuente de la obtención de datos (auto o manual)</param>
        /// <param name="fechaMedicion">Fecha del registro</param>
        /// <param name="dataTransformadores">Datos de transformadores</param>
        /// <param name="dataBarras">Datos de barras</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        public List<DpoEstimadorRawTmpDTO> ProcesarDatosTransformadores(int fuente,
                                                                        DateTime fechaMedicion,
                                                                        List<string[]> dataTransformadores,
                                                                        List<string[]> dataBarras,
                                                                        List<MePtomedicionDTO> listaPuntos)
        {
            List<DpoEstimadorRawTmpDTO> entities = new List<DpoEstimadorRawTmpDTO>();

            // Índices de los datos de lineas
            // F:fila, un registro de transformador se compone de 4 filas
            int indexNombre = 2;//F4
            int indexWIND1 = 0;//F3
            int indexRMA1 = 8;//F3
            int indexRMI1 = 9;//F3

            int indexNTP = 12;//F3

            int indexVarR = 0;//F2
            int indexVarX = 1;//F2
            int indexVarCap = 3;//F3
            int indexTrafoBarraI = 0;//Barra 1
            int indexTrafoBarraJ = 1;//Barra 2

            // Índice de los datos de barras
            int indexBarra = 0;
            int indexVarVkv = 2;
            int indexVarVpu = 7;
            int indexAngulo = 8;

            // Nuevo campo Estado
            int indexTransformadorEstado = 11;

            int idUnidadEnvio;
            int idUnidadRecepcion;

            DpoEstimadorRawTmpDTO entidadMedicion;
            MePtomedicionDTO entidadPtoMedicionEnvio;
            MePtomedicionDTO entidadPtoMedicionRecepcion;

            decimal valorNtpx, valorNOMV1p;
            decimal valorUtrnH, valorUtrnL;
            decimal valorK1, valorK2, valortVipu, valortVjpu;
            decimal valorPotEnvioActiva, valorPotEnvioReactiva, valorPotEnvioAparente;
            decimal valorPotRecepcionActiva, valorPotRecepcionReactiva, valorPotRecepcionAparente;
            decimal valorPerdidasTrafos;
            decimal valorCargabilidadMaxima;

            // Nuevo campo Estado
            decimal valorEstado = 0;

            int i = 0;
            while (i < dataTransformadores.Count)
            {
                string[] rTransformador1 = dataTransformadores[i];

                // Nuevo campo Estado
                valorEstado = PasarDecimalCero(rTransformador1[indexTransformadorEstado]);

                string[] rTransformador2 = dataTransformadores[i + 1];
                string[] rTransformador3 = dataTransformadores[i + 2];
                string[] rTransformador4 = dataTransformadores[i + 3];

                entidadPtoMedicionEnvio = new MePtomedicionDTO();
                entidadPtoMedicionEnvio.Ptomedidesc = rTransformador4[indexNombre].Trim(new char[] { ' ', '\'' });
                entidadPtoMedicionEnvio.Ptomedidesc = entidadPtoMedicionEnvio.Ptomedidesc + ConstantesProdem.PrefijoEnvio;
                entidadPtoMedicionEnvio.Codref = ConstantesProdem.EtmrawtpTrafo;

                entidadPtoMedicionRecepcion = new MePtomedicionDTO();
                entidadPtoMedicionRecepcion.Ptomedidesc = rTransformador4[indexNombre].Trim(new char[] { ' ', '\'' });
                entidadPtoMedicionRecepcion.Ptomedidesc = entidadPtoMedicionRecepcion.Ptomedidesc + ConstantesProdem.PrefijoRecepcion;
                entidadPtoMedicionEnvio.Codref = ConstantesProdem.EtmrawtpTrafo;

                if (fuente == ConstantesProdem.EtmrawfntIeod)
                {
                    entidadPtoMedicionEnvio.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                    entidadPtoMedicionRecepcion.Origlectcodi = ConstantesProdem.OriglectcodiTnaIeod;
                }

                if (fuente == ConstantesProdem.EtmrawfntSco)
                {
                    entidadPtoMedicionEnvio.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;
                    entidadPtoMedicionRecepcion.Origlectcodi = ConstantesProdem.OriglectcodiTnaSco;
                }

                idUnidadEnvio = RegistrarPtoMedicionRaw(entidadPtoMedicionEnvio, listaPuntos);
                idUnidadRecepcion = RegistrarPtoMedicionRaw(entidadPtoMedicionRecepcion, listaPuntos);

                #region Procesa los datos
                decimal varWIND1 = PasarDecimalCero(rTransformador3[indexWIND1]);
                decimal varRMA1 = PasarDecimalCero(rTransformador3[indexRMA1]);
                decimal varRMI1 = PasarDecimalCero(rTransformador3[indexRMI1]);
                decimal varNTP = PasarDecimalCero(rTransformador3[indexNTP]);
                decimal varR = PasarDecimalCero(rTransformador2[indexVarR]);
                decimal varX = PasarDecimalCero(rTransformador2[indexVarX]);
                decimal varCap = PasarDecimalCero(rTransformador3[indexVarCap]);

                // Calculo de la variable K1 y K2
                decimal dDenominador = ((varR * varR) + (varX * varX));
                if (dDenominador == 0) dDenominador = 1;
                valorK1 = varR / dDenominador;
                valorK2 = varX / dDenominador;

                // Obtención de varibles por barra
                decimal varIkv = 0;
                decimal varJkv = 0;
                decimal varIpu = 0;
                decimal varJpu = 0;
                double anguloI = 0;
                double anguloJ = 0;
                foreach (string[] rBarra in dataBarras)
                {
                    if (rTransformador1[indexTrafoBarraI] == rBarra[indexBarra])
                    {
                        varIkv = PasarDecimalCero(rBarra[indexVarVkv]);
                        varIpu = PasarDecimalCero(rBarra[indexVarVpu]);
                        anguloI = double.Parse(rBarra[indexAngulo]);
                    }

                    if (rTransformador1[indexTrafoBarraJ] == rBarra[indexBarra])
                    {
                        varJkv = PasarDecimalCero(rBarra[indexVarVkv]);
                        varJpu = PasarDecimalCero(rBarra[indexVarVpu]);
                        anguloJ = double.Parse(rBarra[indexAngulo]);
                    }
                }

                // Obtiene diferencia angular
                double difAnguloIJ = anguloI - anguloJ;
                difAnguloIJ = (difAnguloIJ * Math.PI) / 180;

                // Obtiene Ntpmx
                valorNtpx = (varRMA1 - varWIND1) * (varNTP - 1);

                //Validación por denominador "0"
                decimal v1 = varRMA1 - varRMI1;
                v1 = (v1 != 0) ? v1 : 1;
                //---
                valorNtpx = valorNtpx / v1;
                valorNtpx = Math.Round(valorNtpx, 0);

                // Obtiene NOMV1p
                decimal v2 = varNTP - 1;
                v2 = (v2 != 0) ? v2 : 1;
                valorNOMV1p = (valorNtpx * (varRMA1 - varRMI1)) / v2;
                valorNOMV1p = varRMA1 - valorNOMV1p;
                valorNOMV1p = varIkv * valorNOMV1p;

                // Obtiene "utrnH" y "utrnL"
                if (varIkv > varJkv)
                {
                    valorUtrnH = (valorNOMV1p != 0) ? valorNOMV1p : 1;
                    valorUtrnL = (varJkv != 0) ? varJkv : 1;
                }
                else
                {
                    valorUtrnH = (varJkv != 0) ? varJkv : 1;
                    valorUtrnL = (valorNOMV1p != 0) ? valorNOMV1p : 1;
                }

                // Obtiene "tVipu" y "tVjpu"                
                valortVipu = (varIpu * varIkv) / valorUtrnH;
                valortVjpu = (varJpu * varJkv) / valorUtrnL;

                // Calculo de la Potencia de envío
                valorPotEnvioActiva = (-1 * valorK1) * (decimal)(Math.Cos(difAnguloIJ));
                valorPotEnvioActiva = valorPotEnvioActiva + (valorK2 * (decimal)(Math.Sin(difAnguloIJ)));
                valorPotEnvioActiva = valorPotEnvioActiva * valortVipu * valortVjpu;
                valorPotEnvioActiva = valorPotEnvioActiva + (valortVipu * valortVipu * valorK1);
                valorPotEnvioActiva = valorPotEnvioActiva * 100;

                valorPotEnvioReactiva = (-1 * valorK1) * (decimal)(Math.Sin(difAnguloIJ));
                valorPotEnvioReactiva = valorPotEnvioReactiva - (valorK2 * (decimal)(Math.Cos(difAnguloIJ)));
                valorPotEnvioReactiva = valorPotEnvioReactiva * valortVipu * valortVjpu;
                valorPotEnvioReactiva = valorPotEnvioReactiva + (valortVipu * valortVipu * valorK2);
                valorPotEnvioReactiva = valorPotEnvioReactiva * 100;

                valorPotEnvioAparente = valorPotEnvioActiva * valorPotEnvioActiva;
                valorPotEnvioAparente = valorPotEnvioAparente + (valorPotEnvioReactiva * valorPotEnvioReactiva);
                valorPotEnvioAparente = (decimal)Math.Sqrt((double)valorPotEnvioAparente);

                // Calculo de la Potencia de recepción
                valorPotRecepcionActiva = (-1 * valorK1) * (decimal)(Math.Cos(difAnguloIJ));
                valorPotRecepcionActiva = valorPotRecepcionActiva - (valorK2 * (decimal)(Math.Sin(difAnguloIJ)));
                valorPotRecepcionActiva = valorPotRecepcionActiva * valortVipu * valortVjpu;
                valorPotRecepcionActiva = valorPotRecepcionActiva + (valortVjpu * valortVjpu * valorK1);
                valorPotRecepcionActiva = valorPotRecepcionActiva * 100;

                valorPotRecepcionReactiva = valorK1 * (decimal)(Math.Sin(difAnguloIJ));
                valorPotRecepcionReactiva = valorPotRecepcionReactiva - (valorK2 * (decimal)(Math.Cos(difAnguloIJ)));
                valorPotRecepcionReactiva = valorPotRecepcionReactiva * valortVipu * valortVjpu;
                valorPotRecepcionReactiva = valorPotRecepcionReactiva + (valortVjpu * valortVjpu * valorK2);
                valorPotRecepcionReactiva = valorPotRecepcionReactiva * 100;

                valorPotRecepcionAparente = valorPotRecepcionActiva * valorPotRecepcionActiva;
                valorPotRecepcionAparente = valorPotEnvioAparente + (valorPotRecepcionReactiva * valorPotRecepcionReactiva);
                valorPotRecepcionAparente = (decimal)Math.Sqrt((double)valorPotRecepcionAparente);

                // Cálculo de las pérdidas en línea
                valorPerdidasTrafos = valorPotEnvioActiva + valorPotRecepcionActiva;

                // Cálculo de cargabilidad máxima
                decimal porEnvio = (valorPotEnvioAparente / varCap) * 100;
                decimal porRecepcion = (valorPotRecepcionAparente / varCap) * 100;
                valorCargabilidadMaxima = (porEnvio > porRecepcion) ? porEnvio : porRecepcion;
                #endregion

                #region Carga la lista de entidades
                // a) Potencia de envio
                // a.1) activa
                valorPotEnvioActiva = Math.Round(valorPotEnvioActiva, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.TransPotActivaMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotEnvioActiva
                };
                entities.Add(entidadMedicion);

                // a.2) reactiva
                valorPotEnvioReactiva = Math.Round(valorPotEnvioReactiva, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.TransPotReactivaMVAR,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotEnvioReactiva
                };
                entities.Add(entidadMedicion);

                // a.3) aparente
                valorPotEnvioAparente = Math.Round(valorPotEnvioAparente, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.TransPotAparenteMVA,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotEnvioAparente
                };
                entities.Add(entidadMedicion);

                // b) Potencia de recepción
                // b.1) activa
                valorPotRecepcionActiva = Math.Round(valorPotRecepcionActiva, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransPotActivaMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotRecepcionActiva
                };
                entities.Add(entidadMedicion);

                // b.2) reactiva
                valorPotRecepcionReactiva = Math.Round(valorPotRecepcionReactiva, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransPotReactivaMVAR,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotRecepcionReactiva
                };
                entities.Add(entidadMedicion);

                // b.3) aparente
                valorPotRecepcionAparente = Math.Round(valorPotRecepcionAparente, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransPotAparenteMVA,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPotRecepcionAparente
                };
                entities.Add(entidadMedicion);

                // c.1) Cargabilidad máxima envio
                valorCargabilidadMaxima = Math.Round(valorCargabilidadMaxima, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.TransCargaMaxima,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorCargabilidadMaxima
                };
                entities.Add(entidadMedicion);

                // c.2) Cargabilidad máxima recepción
                valorCargabilidadMaxima = Math.Round(valorCargabilidadMaxima, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransCargaMaxima,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorCargabilidadMaxima
                };
                entities.Add(entidadMedicion);

                // c.3) Pérdidas en trafo envio
                valorPerdidasTrafos = Math.Round(valorPerdidasTrafos, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadEnvio,
                    Prnvarcodi = ConstantesProdem.TransPerdidasMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPerdidasTrafos
                };
                entities.Add(entidadMedicion);

                // c.3) Pérdidas en trafo recepción
                valorPerdidasTrafos = Math.Round(valorPerdidasTrafos, 4);
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransPerdidasMW,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorPerdidasTrafos
                };
                entities.Add(entidadMedicion);

                // Nuevo campo estado
                entidadMedicion = new DpoEstimadorRawTmpDTO
                {
                    Ptomedicodi = idUnidadRecepcion,
                    Prnvarcodi = ConstantesProdem.TransformadorEstado,
                    Dporawfuente = fuente,
                    Dporawtipomedi = ConstantesProdem.EtmrawtpTrafo,
                    Dporawfecha = fechaMedicion,
                    Dporawvalor = valorEstado
                };
                entities.Add(entidadMedicion);
                #endregion

                // Cada registro de transformadores se compone de 4 lineas
                i += 4;
            }
            return entities;
        }

        /// <summary>
        /// Método que devuelve los registros de la tabla ME_PTOMEDICION segun el origen de lectura
        /// </summary>
        /// <param name="origlectcodi">Identificador de la tabla ME_ORIGENLECTURA</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListPtomedicionByOriglectcodi(int origlectcodi)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListPtomedicionByOriglectcodi(origlectcodi);
        }
        #endregion

        #region Métodos de lectura de los archivos IEOD (Modulo de Costos Marginales)
        /// <summary>
        /// Metodo que ejecuta la lectura de datos de costos marginales y generacion de archivos Raw en una carpeta especifica
        /// </summary>
        /// <param name="fechaHoraProceso">Fecha para la importación</param>
        /// <returns></returns>
        public string EjecutaJobGeneracionRawsIEOD(string fechaHoraProceso)
        {
            string file = "";
            string sReturn = "1";
            string estimador = "T";
            string fuentepd = "Y";
            int version = 2;

            try
            {
                DateTime fecInicio = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaHoraProceso))
                {
                    fecInicio = DateTime.ParseExact(fechaHoraProceso, ConstantesDpo.FormatoFechaMedicionRaw, CultureInfo.InvariantCulture);
                }

                List<CmCostomarginalDTO> listado = FactorySic.GetCmCostomarginalRepository().ObtenerReporteCostosMarginales(fecInicio, fecInicio, estimador, fuentepd, version);

                var entitys = listado.Select(x => new { x.Cmgncorrelativo, x.Cmgnfecha }).Distinct().ToList();

                List<CmCostomarginalDTO> result = new List<CmCostomarginalDTO>();
                foreach (var item in entitys)
                {
                    CmCostomarginalDTO entity = new CmCostomarginalDTO();
                    entity.Cmgncorrelativo = item.Cmgncorrelativo;
                    entity.Cmgnfecha = item.Cmgnfecha;
                    result.Add(entity);
                }

                file = this.ExportarArchivosDAT(result, fecInicio);
            }
            catch (Exception ex)
            {
                sReturn = ex.StackTrace;
                sReturn = ex.Message;
                return sReturn;
            }

            return sReturn;
        }

        /// <summary>
        /// Permite exportar los archivos .RAW
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="fechaProceso"></param>
        /// <returns>string</returns>
        public string ExportarArchivosDAT(List<CmCostomarginalDTO> entitys, DateTime fechaProceso)
        {
            string sResultado = "1";

            try
            {
                //- Estableciendo carpeta de archivos y nombres
                string pathTrabajo = ConfigurationManager.AppSettings[ConstantesDpo.PathCostosMarginales];

                List<string> blobs = new List<string>();
                List<string> names = new List<string>();

                foreach (CmCostomarginalDTO entity in entitys)
                {
                    DateTime fecha = (entity.Cmgnfecha.Hour == 23 && entity.Cmgnfecha.Minute == 59) ? entity.Cmgnfecha.AddMinutes(1) :
                                                                                                      entity.Cmgnfecha;

                    string path = fecha.Year + @"\" +
                                  fecha.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesDpo.CaracterCero)) +
                                  fecha.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesDpo.CaracterCero)) +
                                  @"\Corrida_" + entity.Cmgncorrelativo + @"\";

                    List<FileData> files = FileServerScada.ListarArhivos(path, pathTrabajo);

                    FileData dat = files.Where(x => (x.Extension.Contains("RAW") || x.Extension.Contains("raw"))).FirstOrDefault();

                    if (dat != null)
                    {
                        string pathDestino = ConfigurationManager.AppSettings[ConstantesDpo.RutaDemandaRawCostoMarginal];
                        File.Copy(pathTrabajo + dat.FileUrl, pathDestino + dat.FileName);
                        //var resultado = FileServerScada.CopiarFile((pathTrabajo + dat.FileUrl), (pathDestino + dat.FileName), null, null);
                        //if (resultado != 1)
                        //{
                        //    throw new ArgumentException(string.Format("Ocurrió un error cuando se copia el archivo {0} de {1} a {2}.", dat.FileName, pathTrabajo, pathDestino));
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                sResultado = e.StackTrace;
                sResultado = e.Message;
                return sResultado;
            }
            return sResultado;
        }
        #endregion

        #endregion

        #region Métodos Tabla DPO_ESTIMADORRAW
        /// <summary>
        /// Inserta un registro de la tabla DPO_ESTIMADORRAW
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoEstimadorRaw(DpoEstimadorRawTmpDTO entity)
        {
            try
            {
                FactorySic.GetDpoEstimadorRawRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DPO_ESTIMADORRAW
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void UpdateDpoEstimadorRaw(DpoEstimadorRawTmpDTO entity)
        {
            FactorySic.GetDpoEstimadorRawRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla DPO_ESTIMADORRAW
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="dpovarcodi"></param>
        /// <param name="dporawfuente"></param>
        /// <param name="dporawtipomedi"></param>
        /// <param name="dporawfecha"></param>
        /// <returns></returns>
        public void DeleteDpoEstimadorRaw(int ptomedicodi, int dpovarcodi, int dporawfuente, int dporawtipomedi, DateTime dporawfecha)
        {
            FactorySic.GetDpoEstimadorRawRepository().Delete(ptomedicodi, dpovarcodi, dporawfuente, dporawtipomedi, dporawfecha);
        }

        /// <summary>
        /// Listar registros de DpoEstimadorRaw
        /// </summary>
        /// <returns></returns>
        public List<DpoEstimadorRawTmpDTO> ListDpoEstimadorRaw()
        {
            return FactorySic.GetDpoEstimadorRawRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DPO_ESTIMADORRAW
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="dpovarcodi"></param>
        /// <param name="dporawfuente"></param>
        /// <param name="dporawtipomedi"></param>
        /// <param name="dporawfecha"></param>
        /// <returns></returns>
        public DpoEstimadorRawTmpDTO GetByIdDpoEstimadorRaw(int ptomedicodi, int dpovarcodi, int dporawfuente, int dporawtipomedi, DateTime dporawfecha)
        {
            return FactorySic.GetDpoEstimadorRawRepository().GetById(ptomedicodi, dpovarcodi, dporawfuente, dporawtipomedi, dporawfecha);
        }

        /// <summary>
        /// Método que elimina los registros de la tabla PRN_ESTIMADORRAW
        /// que correspondan a cierta fecha, intervalo y fuente
        /// </summary>
        /// <param name="fuente">origen de la carga de información (ieod, sco)</param>
        /// <param name="fecha">fecha de los registros(dd/MM/yyyy)</param>
        public void DeletePrnEstimadorRawPorFechaIntervalo(int fuente, string fecha)
        {
            FactorySic.GetPrnEstimadorRawRepository().DeletePorFechaIntervalo(fuente, fecha);
        }

        /// <summary>
        /// BulkInsert de la tabla DPO_ESTIMADORRAW
        /// </summary>
        /// <param name="listaMediciones"></param>
        /// <param name="proceso">[1] TNA x Minuto -> DPO_ESTIMADORRAW_TMP / IEOD x 30 Minutos -> DPO_ESTIMADORRAW_CMTMP / [3] Manual -> DPO_ESTIMADORRAW_MANUAL</param>
        /// <param name="sProcedimiento">Auto/Manual</param>
        public void BulkInsertDpoEstimadorRaw(List<DpoEstimadorRawTmpDTO> listaMediciones, int proceso, string sProcedimiento)
        {
            string nombreTabla = string.Empty;

            // -----------------------------------------------------------------------------------------
            // Proceso automatica
            // -----------------------------------------------------------------------------------------
            if (proceso == ConstantesDpo.CargaAutoRawXMinuto)
                nombreTabla = ConstantesDpo.tablaCargaDemandaDpoTmp;

            if (proceso == ConstantesDpo.CargaAutoRawX30Minutos)
                nombreTabla = ConstantesDpo.tablaCargaDemandaDpoCmTmp;

            // -----------------------------------------------------------------------------------------
            // Proceso manual
            // -----------------------------------------------------------------------------------------
            if (proceso == ConstantesDpo.CargaManualRaw || sProcedimiento.Equals("Manual"))
                nombreTabla = ConstantesDpo.tablaCargaDemandaDpoMan;
            // -----------------------------------------------------------------------------------------


            int x = listaMediciones.Count;
            List<DpoEstimadorRawTmpDTO> entityBulkInsert = new List<DpoEstimadorRawTmpDTO>();

            int i = 0, j = 0;
            if (x != 0)
            {
                while (i < x)
                {
                    j = 0;
                    entityBulkInsert = new List<DpoEstimadorRawTmpDTO>();
                    while (j < ConstantesProdem.LimiteBulkInsert)
                    {
                        entityBulkInsert.Add(listaMediciones[i]);
                        i++; j++;
                        if (i == x) break;
                    }
                    try
                    {
                        FactorySic.GetDpoEstimadorRawRepository().BulkInsert(entityBulkInsert, nombreTabla);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }


        /// <summary>
        /// Método que limpia la tabla temporal utilizada para la carga de archivos raw
        /// </summary>
        /// <param name="proceso">Identificador del tipo de proceso</param>
        public void TruncateTemporalDpoEstimadorRaw(int proceso)
        {
            string nombreTabla = string.Empty;

            // -----------------------------------------------------------------------------------------
            // Proceso automatico
            // -----------------------------------------------------------------------------------------
            if (proceso == ConstantesDpo.CargaAutoRawXMinuto)
                nombreTabla = ConstantesDpo.tablaCargaDemandaDpoTmp;

            if (proceso == ConstantesDpo.CargaAutoRawX30Minutos)
                nombreTabla = ConstantesDpo.tablaCargaDemandaDpoCmTmp;

            // -----------------------------------------------------------------------------------------
            // Proceso manual
            // -----------------------------------------------------------------------------------------
            if (proceso == ConstantesDpo.CargaManualRaw)
                nombreTabla = ConstantesDpo.tablaCargaDemandaDpoMan;
            // -----------------------------------------------------------------------------------------

            if (!string.IsNullOrEmpty(nombreTabla))
                FactorySic.GetDpoEstimadorRawRepository().TruncateTablaTemporal(nombreTabla);
        }

        /// <summary>
        /// Permite registra un punto de medición (unidad de archivo raw) en la bd validando su existencia
        /// </summary>
        /// <param name="entidad">Entidad que representa un registro de la tabla MePtomedicion</param>
        /// <param name="listaPuntos">Lista de puntos de medición registrados en la bd</param>
        /// <returns></returns>
        public int RegistrarPtoMedicionRaw(MePtomedicionDTO entidad, List<MePtomedicionDTO> listaPuntos)
        {
            MePtomedicionDTO existe = listaPuntos
                .Where(x => x.Ptomedidesc.Equals(entidad.Ptomedidesc))
                .FirstOrDefault() ?? new MePtomedicionDTO();

            return (existe.Ptomedicodi != 0)
                ? existe.Ptomedicodi
                : SaveMePtomedicion(entidad);
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_PTOMEDICION
        /// </summary>
        /// <param name="entity"></param>
        public int SaveMePtomedicion(MePtomedicionDTO entity)
        {
            return FactorySic.GetMePtomedicionRepository().Save(entity);
        }

        /// <summary>
        /// Devuelve la información procesada TNA por Punto de medición
        /// </summary>
        /// <param name="nombreTabla">Nombre de la tabla a consultar</param>
        /// <param name="fuente">Fuente de la información</param>
        /// <param name="ptomedicion">Identificador del punto de medición a buscara</param>
        /// <returns></returns>
        public List<DpoEstimadorRawDTO> ObtenerDatosTnaPorPtoMedicion(
            string nombreTabla, int fuente,
            int ptomedicion)
        {
            return FactorySic.GetDpoEstimadorRawRepository()
                .DatosPorPtoMedicion(nombreTabla, fuente,
                ptomedicion);
        }

        /// <summary>
        /// Método que Eliminamos en el log todos los registros de todos los procesos que tengan antiguedad de 60 dia atras 
        /// o de antiguedad apartir de la fecDepuracion
        /// </summary>
        /// <param name="fechaHoraProceso">fecha-hora-minuto de proceso</param>
        public void DeleteRawsHistoricos(DateTime fechaHoraProceso)
        {
            DateTime fecDepuracion = fechaHoraProceso.AddDays(-60);
            FactorySic.GetDpoEstimadorRawRepository().DeleteRawsHistoricos(fecDepuracion);
        }

        /// <summary>
        /// Método que Elimina Delete registro de DPO_ESTIMADORRAW_FILES por diaProceso
        /// </summary>
        /// <param name="fechaHoraInicio">fecha-hora-minuto de inicio</param>
        /// <param name="fechaHoraFin">fecha-hora-minuto fin</param>
        /// <param name="tipo">1:RAW / 2: IEOD</param>
        public void DeleteREstimadorRawFileByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin, int tipo)
        {
            FactorySic.GetDpoEstimadorRawRepository().DeleteREstimadorRawFileByDiaProceso(fechaHoraInicio, fechaHoraFin, tipo);
        }

        /// <summary>
        /// Método que Elimina Delete registro de DPO_ESTIMADORRAW_LOG por diaProceso
        /// </summary>
        /// <param name="fechaHoraInicio">fecha-hora-minuto de inicio</param>
        /// <param name="fechaHoraFin">fecha-hora-minuto fin</param>
        /// <param name="tipo">1:RAW / 2: IEOD</param>
        public void DeleteREstimadorRawLogByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin, int tipo)
        {
            FactorySic.GetDpoEstimadorRawRepository().DeleteREstimadorRawLogByDiaProceso(fechaHoraInicio, fechaHoraFin, tipo);
        }

        /// <summary>
        /// Método que Elimina Delete registro de DPO_ESTIMADORRAW_TMP por diaProceso
        /// </summary>
        /// <param name="fechaHoraInicio">fecha-hora-minuto de inicio</param>
        /// <param name="fechaHoraFin">fecha-hora-minuto fin</param>
        public void DeleteREstimadorRawTemporalByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin)
        {
            FactorySic.GetDpoEstimadorRawRepository().DeleteREstimadorRawTemporalByDiaProceso(fechaHoraInicio, fechaHoraFin);
        }

        /// <summary>
        /// Método que Elimina Delete registro de DPO_ESTIMADORRAW_TMP por diaProceso
        /// </summary>
        /// <param name="fechaHoraInicio">fecha-hora-minuto de inicio</param>
        /// <param name="fechaHoraFin">fecha-hora-minuto fin</param>
        public void DeleteREstimadorRawCmTemporalByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin)
        {
            FactorySic.GetDpoEstimadorRawRepository().DeleteREstimadorRawCmTemporalByDiaProceso(fechaHoraInicio, fechaHoraFin);
        }

        /// <summary>
        /// Método que Elimina Delete registro de DPO_ESTIMADOR_RAW por diaProceso
        /// </summary>
        /// <param name="sufAnioMes">año mes de proceso</param>
        /// <param name="fechaHoraInicio">fecha-hora-minuto de inicio</param>
        /// <param name="fechaHoraFin">fecha-hora-minuto fin</param>
        public void DeleteREstimadorRawByDiaProceso(string sufAnioMes, DateTime fechaHoraInicio, DateTime fechaHoraFin)
        {
            FactorySic.GetDpoEstimadorRawRepository().DeleteREstimadorRawByDiaProceso(sufAnioMes, fechaHoraInicio, fechaHoraFin);
        }

        #endregion

        #region Métodos Tabla DPO_DEMANDASCO
        /// <summary>
        /// Inserta un registro de la tabla DPO_DEMANDASCO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoDemandaSco(DpoDemandaScoDTO entity)
        {
            try
            {
                FactorySic.GetDpoDemandaScoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DPO_DEMANDASCO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void UpdateDpoDemandaSco(DpoDemandaScoDTO entity)
        {
            FactorySic.GetDpoDemandaScoRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla DPO_DEMANDASCO
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="medifecha"></param>
        /// <param name="prnvarcodi"></param>
        /// <returns></returns>
        public void DeleteDpoDemandaSco(int ptomedicodi, DateTime medifecha, int prnvarcodi)
        {
            FactorySic.GetDpoDemandaScoRepository().Delete(ptomedicodi, medifecha, prnvarcodi);
        }

        /// <summary>
        /// Listar registros de DPO_DEMANDASCO
        /// </summary>
        /// <returns></returns>
        public List<DpoDemandaScoDTO> ListDpoDemandaSco()
        {
            return FactorySic.GetDpoDemandaScoRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DPO_DEMANDASCO
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="medifecha"></param>
        /// <param name="prnvarcodi"></param>
        /// <returns></returns>
        public DpoDemandaScoDTO GetByIdDpoDemandaSco(int ptomedicodi, DateTime medifecha, int prnvarcodi)
        {
            return FactorySic.GetDpoDemandaScoRepository().GetById(ptomedicodi, medifecha, prnvarcodi);
        }

        /// <summary>
        /// BulkInsert de la tabla DPO_DEMANDASCO
        /// </summary>
        /// <param name="listaMediciones"></param>
        /// <param name="nombreTabla"></param>
        public void BulkInsertDpoDemandaSco(List<DpoDemandaScoDTO> listaMediciones, string nombreTabla)
        {
            int x = listaMediciones.Count();
            List<DpoDemandaScoDTO> entityBulkInsert = new List<DpoDemandaScoDTO>();

            int i = 0, j = 0;
            if (x != 0)
            {
                while (i < x)
                {
                    j = 0;
                    entityBulkInsert = new List<DpoDemandaScoDTO>();
                    while (j < ConstantesProdem.LimiteBulkInsert)
                    {
                        entityBulkInsert.Add(listaMediciones[i]);
                        i++; j++;
                        if (i == x) break;
                    }

                    try
                    {
                        FactorySic.GetDpoDemandaScoRepository().BulkInsert(entityBulkInsert, nombreTabla);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Método que limpia la tabla temporal utilizada para la carga de archivos raw
        /// </summary>
        /// <param name="idFuente">Identificador de la fuente de información</param>
        public void TruncateTemporalDpoDemandaSco(int idFuente)
        {
            string nombreTabla = string.Empty;
            if (idFuente == ConstantesProdem.EtmrawfntIeod)
                nombreTabla = ConstantesProdem.tablaCargaIeod;
            if (idFuente == ConstantesProdem.EtmrawfntSco)
                nombreTabla = ConstantesProdem.tablaCargaSco;

            if (!string.IsNullOrEmpty(nombreTabla))
                FactorySic.GetDpoDemandaScoRepository().TruncateTablaTemporal(nombreTabla);
        }

        /// <summary>
        /// Método que devuelve el reporte del estado del proceso de filtrado de info
        /// </summary>
        /// <param name="idFuente">Fuente de la información a buscar</param>
        /// <param name="regFecha">Fecha de consulta</param>        
        /// <returns></returns>
        public List<int> RepEstProcesoDpoDemandaSco(int idFuente, string regFecha)
        {
            List<int> res = new List<int>();
            DateTime parseFecIni = DateTime.ParseExact(regFecha,
                ConstantesDpo.FormatoMesAnio,
                CultureInfo.InvariantCulture);
            DateTime parseFecFin = parseFecIni.AddMonths(1);

            List<DateTime> listaFechas = FactorySic
                .GetDpoDemandaScoRepository().ReporteEstadoProceso(idFuente,
                parseFecIni.ToString(ConstantesDpo.FormatoFecha),
                parseFecFin.ToString(ConstantesDpo.FormatoFecha));

            DateTime f = parseFecIni;
            while (f < parseFecFin)
            {
                if (listaFechas.Contains(f))
                    res.Add(1);
                else
                    res.Add(0);

                f = f.AddDays(1);
            }

            return res;
        }

        /// <summary>
        /// Elimina los registros de la tabla DPO_DEMANDASCO por rango de fechas
        /// </summary>
        /// <param name="punto">Identificador de un punto de medición</param>
        /// <param name="fecIni">Fecha de inicio del rango</param>
        /// <param name="fecFin">Fecha de termino del rango</param>
        public void DeleteRangoFechaDpoDemandaSco(
            int punto, string fecIni,
            string fecFin)
        {
            FactorySic.GetDpoDemandaScoRepository().DeleteRangoFecha(
                punto, fecIni, fecFin);
        }
        #endregion

        #region Métodos Tabla DPO_FERIADO

        /// <summary>
        /// Inserta un registro de la tabla DPO_FERIADOS
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoFeriados(DpoFeriadosDTO entity)
        {
            try
            {
                FactorySic.GetDpoFeriadosRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DPO_FERIADOS
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void UpdateDpoFeriados(DpoFeriadosDTO entity)
        {
            FactorySic.GetDpoFeriadosRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla DPO_FERIADOS
        /// </summary>
        /// <param name="dpofercodi"></param>
        /// <returns></returns>
        public void DeleteDpoFeriados(int dpofercodi)
        {
            FactorySic.GetDpoFeriadosRepository().Delete(dpofercodi);
        }

        /// <summary>
        /// Listar registros de DpoFeriados
        /// </summary>
        /// <returns></returns>
        public List<DpoFeriadosDTO> ListDpoFeriados()
        {
            return FactorySic.GetDpoFeriadosRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DPO_FERIADOS
        /// </summary>
        /// <param name="dpofercodi"></param>
        /// <returns></returns>
        public DpoFeriadosDTO GetByIdDpoFeriados(int dpofercodi)
        {
            return FactorySic.GetDpoFeriadosRepository().GetById(dpofercodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DPO_FERIADOS
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<DpoFeriadosDTO> GetByAnioDpoFeriados(int anio)
        {
            return FactorySic.GetDpoFeriadosRepository().GetByAnio(anio);
        }
        public List<DpoFeriadosDTO> GetByFechaDpoFeriados(string fecha)
        {
            return FactorySic.GetDpoFeriadosRepository().GetByFecha(fecha);
        }

        public void UpdateByIdDpoFeriados(int id, string descripcion, string spl, string sco, string fecha)
        {
            FactorySic.GetDpoFeriadosRepository().UpdateById(id, descripcion, spl, sco, fecha);
        }

        /// <summary>
        /// Método que devuelve los feriados registrados para SPL
        /// </summary>
        /// <returns></returns>
        public List<DateTime> ObtenerFeriadosSpl()
        {
            return FactorySic.GetDpoFeriadosRepository().ObtenerFeriadosSpl();
        }

        /// <summary>
        /// Método que devuelve los feriados registrados para SCO
        /// </summary>
        /// <returns></returns>
        public List<DateTime> ObtenerFeriadosSco()
        {
            return FactorySic.GetDpoFeriadosRepository().ObtenerFeriadosSco();
        }

        /// <summary>
        /// Método que devuelve los feriados registrados por año
        /// </summary>
        /// <returns></returns>
        public List<DateTime> ObtenerFeriadosPorAnio(int dpoferanio)
        {
            return FactorySic.GetDpoFeriadosRepository().ObtenerFeriadosPorAnio(dpoferanio);
        }

        /// <summary>
        /// Método que devuelve los feriados en un rango de tiempo (anios)
        /// </summary>
        /// <param name="anioIni">Anio inicial del rango</param>
        /// <param name="anioFin">Anio final del rango</param>
        /// <returns></returns>
        public List<DpoFeriadosDTO> GetByAnioRangoDpoFeriados(
            int anioIni, int anioFin)
        {
            return FactorySic.GetDpoFeriadosRepository()
                .GetByAnioRango(anioIni, anioFin);
        }
        #endregion

        #region Métodos Tabla ME_PTOMEDICION
        /// <summary>
        /// Devuelve los puntos de medición SCO
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListPtomedicionSco()
        {
            return FactorySic
                .GetPrnPronosticoDemandaRepository()
                .ListPtomedicionByOriglectcodi(ConstantesDpo.OriglectcodiTnaSco);
        }

        /// <summary>
        /// Devuelve los puntos de medición IEOD
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListPtomedicionIeod()
        {
            return FactorySic
                .GetPrnPronosticoDemandaRepository()
                .ListPtomedicionByOriglectcodi(ConstantesDpo.OriglectcodiTnaIeod);
        }
        #endregion

        /// <summary>
        /// Método que genera un reporte excel simple
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="pathFile"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string ExportarReporteSimple(
            PrnFormatoExcel formato, string pathFile, 
            string filename)
        {
            string Reporte = filename + ".xlsx";
            ReportesDpo.GenerarArchivoExcel(
                formato, pathFile + Reporte);
            return Reporte;
        }

        /// <summary>
        /// Método que genera un reporte excel por libro
        /// </summary>
        /// <param name="formatos"></param>
        /// <param name="pathFile"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string ExportarReporteConLibros
            (List<PrnFormatoExcel> formatos, string pathFile,
            string filename)
        {
            //string Reporte = filename + "_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";
            string Reporte = filename + ".xlsx";
            ReportesDpo.GenerarArchivoExcelConLibros(
                formatos, pathFile + Reporte);

            return Reporte;
        }

        /// <summary>
        /// Devuelve la lista de variables por unidad TNA de los archivos .raw
        /// </summary>
        /// <returns></returns>
        public List<int> ListaVariablesUnidadTna()
        {
            return new List<int>()
            {
                ConstantesProdem.GeneradorPotActivaMW,
                ConstantesProdem.GeneradorPotReactivaMVAR,
                ConstantesProdem.LineaPotActivaMW,
                ConstantesProdem.LineaPotReactivaMVAR,
                ConstantesProdem.LineaPotAparenteMVA,
                ConstantesProdem.LineaPerdidasMW,
                ConstantesProdem.LineaCargaMaxima,
                ConstantesProdem.TransPotActivaMW,
                ConstantesProdem.TransPotReactivaMVAR,
                ConstantesProdem.TransPotAparenteMVA,
                ConstantesProdem.TransCargaMaxima,
                ConstantesProdem.TransPerdidasMW,
                ConstantesProdem.BarraTensionKv,
                ConstantesProdem.BarraAngulo,
                ConstantesProdem.BarraDemActivaMW,
                ConstantesProdem.BarraDemReactivaMVAR,
                ConstantesProdem.CargaPotActivaMW,
                ConstantesProdem.CargaPotReactivaMVAR,
                ConstantesProdem.ShuntPotReactivaMVAR,
                ConstantesProdem.GeneradorEstado,
                ConstantesProdem.LineaEstado,
                ConstantesProdem.TransformadorEstado,
                ConstantesProdem.BarraEstado,
                ConstantesProdem.CargaEstado,
                ConstantesProdem.ShuntEstado,
            };
        }


        #region DemandaPO Iteracion 2
        /// <summary>
        /// Lista el resultado de calcular la formula.
        /// </summary>
        /// <param name="ListaItemFormula">fecha de inicio</param>
        /// <param name="anio">anio de la consulta</param>
        /// <param name="mes">mes de la consulta</param>
        /// <param name="variable">tipo de datoa activa o reactiva</param>
        /// <param name="version">version a corregir</param>
        /// <returns></returns>
        public List<DpoConsultaPrefiltrado> CalculoFormula(List<FormulaItem> ListaItemFormula, int anio, string[] mes, int variable, int version)
        {
            List<DpoConsultaPrefiltrado> result = new List<DpoConsultaPrefiltrado>();
            string meses = string.Join(", ", mes.Select(x => $"'{x}'"));

            List<DpoMedicion96DTO> lista = new List<DpoMedicion96DTO>(); 

            foreach (var item in ListaItemFormula)
            {
                switch (item.Tipo)
                {
                    case ConstantesDpo.OrigenSirpit:
                        int tipoSirpit = (variable == 1) ? ConstantesDpo.DpotmeSirpitActiva : ConstantesDpo.DpotmeSirpitReactiva;
                        if (version == -1)
                        {
                            List<DpoDatos96DTO> dpoDatos = this.ListaDatosSIRPIT(anio, meses, item.Codigo.ToString(), variable.ToString());
                            foreach (DpoDatos96DTO d in dpoDatos)
                            {
                                DpoMedicion96DTO sirpit = new DpoMedicion96DTO();
                                sirpit.Dpomedcodi = d.Tnfbarcodi;
                                sirpit.Dpotmecodi = tipoSirpit;
                                sirpit.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                                sirpit.Dpomedfecha = d.Dpodatfecha;
                                for (int i = 1; i <= 96; i++)
                                {
                                    var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                                    sirpit.GetType().GetProperty("H" + i.ToString()).SetValue(sirpit, (decimal?)dValor * item.Constante);
                                }
                                lista.Add(sirpit);
                            }
                        }
                        else
                        {
                            List<DpoMedicion96DTO> listaSirpit = this.ListByFiltros(item.Codigo.ToString(), tipoSirpit,
                                                                                    ConstantesDpo.DpotdtPunto, version, anio, meses);
                            foreach (var medicion in listaSirpit)
                            {
                                for (int i = 1; i <= 96; i++)
                                {
                                    var propInfo = typeof(DpoMedicion96DTO).GetProperty($"H{i}");

                                    if (propInfo != null)
                                    {
                                        decimal? valorOriginal = (decimal?)propInfo.GetValue(medicion);

                                        if (valorOriginal != null)
                                        {
                                            decimal nuevoValor = valorOriginal.Value * item.Constante;
                                            propInfo.SetValue(medicion, nuevoValor);
                                        }
                                    }
                                }
                            }
                            lista.AddRange(listaSirpit);
                        }
                        break;
                    case ConstantesDpo.OrigenSicli:
                        int tipoSicli = (variable == 1) ? ConstantesDpo.DpotmeSicliActiva : ConstantesDpo.DpotmeSicliReactiva;
                        if (version == -1)
                        {
                            List<IioTabla04DTO> iioTabla04 = this.ListaDatosSICLI(anio, meses, item.Codigo.ToString(), variable.ToString());
                            foreach (IioTabla04DTO d in iioTabla04)
                            {
                                DpoMedicion96DTO sicli = new DpoMedicion96DTO();
                                sicli.Dpomedcodi = d.Ptomedicodi;
                                sicli.Dpotmecodi = tipoSicli;
                                sicli.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                                sicli.Dpomedfecha = d.FechaMedicion;
                                for (int i = 1; i <= 96; i++)
                                {
                                    var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                                    sicli.GetType().GetProperty("H" + i.ToString()).SetValue(sicli, (decimal)dValor * item.Constante);
                                }
                                lista.Add(sicli);
                            }
                        }
                        else
                        {
                            List<DpoMedicion96DTO> listaSicli = this.ListByFiltros(item.Codigo.ToString(), tipoSicli,
                                                                                   ConstantesDpo.DpotdtPunto, version, anio, meses);
                            foreach (var medicion in listaSicli)
                            {
                                for (int i = 1; i <= 96; i++)
                                {
                                    var propInfo = typeof(DpoMedicion96DTO).GetProperty($"H{i}");

                                    if (propInfo != null)
                                    {
                                        decimal? valorOriginal = (decimal?)propInfo.GetValue(medicion);

                                        if (valorOriginal != null)
                                        {
                                            decimal nuevoValor = valorOriginal.Value * item.Constante;
                                            propInfo.SetValue(medicion, nuevoValor);
                                        }
                                    }
                                }
                            }
                            lista.AddRange(listaSicli);
                        }
                        break;
                    case ConstantesProdem.OrigenTnaDpo:
                        int tipoTna = (variable == 1) ? ConstantesDpo.DpotmeTnaActiva : ConstantesDpo.DpotmeTnaReactiva;
                        if (version == -1)
                        {
                            string tipos = (variable == 1) ? ConstantesDpo.OrigenActiva : ConstantesDpo.OrigenReactiva;
                            List<DpoDemandaScoDTO> demandaSCO = this.ListaDatosTNA(anio, meses, item.Codigo.ToString(), tipos);
                            foreach (DpoDemandaScoDTO d in demandaSCO)
                            {
                                DpoMedicion96DTO tna = new DpoMedicion96DTO();
                                tna.Dpomedcodi = d.Ptomedicodi;
                                tna.Dpotmecodi = tipoTna;
                                tna.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                                tna.Dpomedfecha = d.Medifecha;
                                for (int i = 1; i <= 96; i++)
                                {
                                    var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                                    tna.GetType().GetProperty("H" + i.ToString()).SetValue(tna, (decimal)dValor * item.Constante);
                                }
                                lista.Add(tna);
                            }
                        }
                        else
                        {
                            List<DpoMedicion96DTO> listaTna = this.ListByFiltros(item.Codigo.ToString(), tipoTna,
                                                                                 ConstantesDpo.DpotdtPunto, version, anio, meses);
                            foreach (var medicion in listaTna)
                            {
                                for (int i = 1; i <= 96; i++)
                                {
                                    var propInfo = typeof(DpoMedicion96DTO).GetProperty($"H{i}");

                                    if (propInfo != null)
                                    {
                                        decimal? valorOriginal = (decimal?)propInfo.GetValue(medicion);

                                        if (valorOriginal != null)
                                        {
                                            decimal nuevoValor = valorOriginal.Value * item.Constante;
                                            propInfo.SetValue(medicion, nuevoValor);
                                        }
                                    }
                                }
                            }
                            lista.AddRange(listaTna);
                        }
                        break;
                }
            }

            //Agrupando la data por dia
            var dataAgrupada = lista
                                 .GroupBy(x => x.Dpomedfecha)
                                 .Select(y =>
                                 {
                                     var dto = new DpoMedicion96DTO { Dpomedfecha = y.Key };
                                     for (int i = 1; i <= 96; i++)
                                     {
                                         string propertyName = "H" + i;
                                         decimal? sum = y.Sum(x => (decimal?)x.GetType().GetProperty(propertyName)?.GetValue(x));
                                         dto.GetType().GetProperty(propertyName)?.SetValue(dto, sum);
                                     }
                                     return dto;
                                 })
                                 .ToList();


            //Separar la data en paquetes por mes
            foreach (var item in mes)
            {
                int parseMes = Int32.Parse(item);
                DateTime lastDay = new DateTime(anio, parseMes,
                                                DateTime.DaysInMonth(anio, parseMes));
                DateTime firstDay = new DateTime(anio, parseMes, 1);
                List<decimal> dataMes = new List<decimal>();
                List<string> fechas = new List<string>();
                while (firstDay <= lastDay)
                {
                    fechas.AddRange(this.ListaFechaHora96(firstDay));
                    var temporal = dataAgrupada.Where(x => x.Dpomedfecha == firstDay).FirstOrDefault();
                    if (temporal != null)
                    {
                        dataMes.AddRange(UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv15min, temporal).ToList());

                    }
                    else
                    {
                        dataMes.AddRange(Enumerable.Repeat(0m, ConstantesProdem.Itv15min).ToArray().ToList());
                    }
                    firstDay = firstDay.AddDays(1);
                }
                DpoConsultaPrefiltrado entity = new DpoConsultaPrefiltrado()
                {
                    id = parseMes * -1,
                    dias = DateTime.DaysInMonth(anio, parseMes),
                    diasHora = fechas,
                    name = "Fórmula",//DateTimeFormatInfo.CurrentInfo.GetMonthName(parseMes),
                    data = dataMes.ToArray(),
                };
                result.Add(entity);
            }

            return result;
        }

        /// <summary>
        /// Lista el resultado de calcular la formula.
        /// </summary>
        /// <param name="ListaItemFormula">fecha de inicio</param>
        /// <param name="anio">anio de la consulta</param>
        /// <param name="mes">mes de la consulta</param>
        /// <param name="variable">tipo de datoa activa o reactiva</param>
        /// <param name="version">version a corregir</param>
        /// <param name="dataCorregida">version a corregir</param>
        /// <returns></returns>
        public List<DpoConsultaPrefiltrado> CalculoFormulaCorregida(List<FormulaItem> ListaItemFormula, int anio, string[] mes, 
                                                                    int variable, int version, List<DpoMedicion96DTO> dataCorregida)
        {
            List<DpoConsultaPrefiltrado> result = new List<DpoConsultaPrefiltrado>();
            string meses = string.Join(", ", mes.Select(x => $"'{x}'"));

            List<DpoMedicion96DTO> lista = new List<DpoMedicion96DTO>();

            foreach (var item in ListaItemFormula)
            {
                switch (item.Tipo)
                {
                    case ConstantesDpo.OrigenSirpit:
                        int tipoSirpit = (variable == 1) ? ConstantesDpo.DpotmeSirpitActiva : ConstantesDpo.DpotmeSirpitReactiva;
                        List<DpoMedicion96DTO> dpoDatosSirpit = dataCorregida.Where(x => x.Dpotmecodi == tipoSirpit && 
                                                                                    x.Dpomedcodi == item.Codigo)
                                                                             .ToList();
                        foreach (DpoMedicion96DTO d in dpoDatosSirpit)
                        {
                            DpoMedicion96DTO sirpit = new DpoMedicion96DTO();
                            sirpit.Dpomedcodi = d.Dpomedcodi;
                            sirpit.Dpotmecodi = tipoSirpit;
                            sirpit.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                            sirpit.Dpomedfecha = d.Dpomedfecha;
                            for (int i = 1; i <= 96; i++)
                            {
                                var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                                sirpit.GetType().GetProperty("H" + i.ToString()).SetValue(sirpit, (decimal?)dValor * item.Constante);
                            }
                            lista.Add(sirpit);
                        }

                        break;
                    case ConstantesDpo.OrigenSicli:
                        int tipoSicli = (variable == 1) ? ConstantesDpo.DpotmeSicliActiva : ConstantesDpo.DpotmeSicliReactiva;
                        List<DpoMedicion96DTO> dpoDatosSicli = dataCorregida.Where(x => x.Dpotmecodi == tipoSicli && 
                                                                                   x.Dpomedcodi == item.Codigo)
                                                                            .ToList();
                        foreach (DpoMedicion96DTO d in dpoDatosSicli)
                        {
                            DpoMedicion96DTO sicli = new DpoMedicion96DTO();
                            sicli.Dpomedcodi = d.Dpomedcodi;
                            sicli.Dpotmecodi = tipoSicli;
                            sicli.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                            sicli.Dpomedfecha = d.Dpomedfecha;
                            for (int i = 1; i <= 96; i++)
                            {
                                var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                                sicli.GetType().GetProperty("H" + i.ToString()).SetValue(sicli, (decimal)dValor * item.Constante);
                            }
                            lista.Add(sicli);
                        }

                        break;
                    case ConstantesProdem.OrigenTnaDpo:
                        int tipoTna = (variable == 1) ? ConstantesDpo.DpotmeTnaActiva : ConstantesDpo.DpotmeTnaReactiva;
                        List<DpoMedicion96DTO> dpoDatosTna = dataCorregida.Where(x => x.Dpotmecodi == tipoTna &&
                                                                                 x.Dpomedcodi == item.Codigo)
                                                                          .ToList();
                        foreach (DpoMedicion96DTO d in dpoDatosTna)
                        {
                            DpoMedicion96DTO tna = new DpoMedicion96DTO();
                            tna.Dpomedcodi = d.Dpomedcodi;
                            tna.Dpotmecodi = tipoTna;
                            tna.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                            tna.Dpomedfecha = d.Dpomedfecha;
                            for (int i = 1; i <= 96; i++)
                            {
                                var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                                tna.GetType().GetProperty("H" + i.ToString()).SetValue(tna, (decimal)dValor * item.Constante);
                            }
                            lista.Add(tna);
                        }

                        break;
                }
            }

            //Agrupando la data por dia
            var dataAgrupada = lista
                                 .GroupBy(x => x.Dpomedfecha)
                                 .Select(y =>
                                 {
                                     var dto = new DpoMedicion96DTO { Dpomedfecha = y.Key };
                                     for (int i = 1; i <= 96; i++)
                                     {
                                         string propertyName = "H" + i;
                                         decimal? sum = y.Sum(x => (decimal?)x.GetType().GetProperty(propertyName)?.GetValue(x));
                                         dto.GetType().GetProperty(propertyName)?.SetValue(dto, sum);
                                     }
                                     return dto;
                                 })
                                 .ToList();


            //Separar la data en paquetes por mes
            foreach (var item in mes)
            {
                int parseMes = Int32.Parse(item);
                DateTime lastDay = new DateTime(anio, parseMes,
                                                DateTime.DaysInMonth(anio, parseMes));
                DateTime firstDay = new DateTime(anio, parseMes, 1);
                List<decimal> dataMes = new List<decimal>();
                List<string> fechas = new List<string>();
                while (firstDay <= lastDay)
                {
                    fechas.AddRange(this.ListaFechaHora96(firstDay));
                    var temporal = dataAgrupada.Where(x => x.Dpomedfecha == firstDay).FirstOrDefault();
                    if (temporal != null)
                    {
                        dataMes.AddRange(UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv15min, temporal).ToList());

                    }
                    else
                    {
                        dataMes.AddRange(Enumerable.Repeat(0m, ConstantesProdem.Itv15min).ToArray().ToList());
                    }
                    firstDay = firstDay.AddDays(1);
                }
                DpoConsultaPrefiltrado entity = new DpoConsultaPrefiltrado()
                {
                    id = parseMes * -1,
                    dias = DateTime.DaysInMonth(anio, parseMes),
                    diasHora = fechas,
                    name = "Fórmula",//DateTimeFormatInfo.CurrentInfo.GetMonthName(parseMes),
                    data = dataMes.ToArray(),
                };
                result.Add(entity);
            }

            return result;
        }

        /// <summary>
        /// Lista los datos necesarios de DPO_DATOS96 para insertar en la tabla DPO_TRAFOBARRA.
        /// </summary>
        /// <param name="inicio">fecha de inicio</param>
        /// <param name="fin">fecha de fin</param>
        /// <returns></returns>
        public List<DpoDatos96DTO> ListaTrafoBarraInfo(string inicio, string fin)
        {
            return FactorySic.GetDpoDatos96Repository().ListTrafoBarraInfo(inicio, fin);
        }

        /// <summary>
        /// Lista los datos registrado en la tabla DPO_TRAFOBARRA.
        /// <returns></returns>
        public List<DpoTrafoBarraDTO> ListaTrafoBarra()
        {
            return FactorySic.GetDpoTrafoBarraRepository().List();
        }

        //
        /// <summary>
        /// Lista los datos registrado en la tabla DPO_TRAFOBARRA filtrado por Ids.
        /// <returns></returns>
        public List<DpoTrafoBarraDTO> ListTrafoBarraById(string codigo)
        {
            return FactorySic.GetDpoTrafoBarraRepository().ListTrafoBarraById(codigo);
        }

        /// <summary>
        /// Permnite calcular el maxcimo y minimo para un juego de datos.
        /// </summary>
        /// <param name="data">informacion por dia(96 valores) de cada mes</param>
        /// <param name="maximo">datos maximos</param>
        /// <param name="minimo">datos minimos</param>
        /// <param name="posiciones">datos minimos</param>
        /// <returns></returns>
        public void CalculoMaximoMinimo(List<DpoConsultaPrefiltrado> data, out DpoConsultaPrefiltrado maximo, 
                                        out DpoConsultaPrefiltrado minimo, out List<int> posiciones)
        {
            maximo = new DpoConsultaPrefiltrado();
            minimo = new DpoConsultaPrefiltrado();
            posiciones = new List<int>();

            List<decimal> listaMaximos = new List<decimal>();
            List<decimal> listaMinimos = new List<decimal>();
            List<string> fechas = new List<string>();
            foreach (var item in data)
            {
                List<decimal> dataMes = new List<decimal>(item.data);
                List<string> dias = new List<string>();
                foreach (var f in item.diasHora)
                {
                    DateTime regFecha = DateTime.ParseExact(f.Substring(0, 10), ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                    dias.Add(regFecha.ToString("MMMM dd").ToUpper());
                }

                dias = dias.Distinct().ToList();
                
                while (dataMes.Count >= ConstantesProdem.Itv15min)
                {
                    decimal vMaximo = dataMes.GetRange(0, 96).Max();
                    decimal vMinimo = dataMes.GetRange(0, 96).Min();
                    listaMaximos.Add(vMaximo);
                    listaMinimos.Add(vMinimo);
                    dataMes.RemoveRange(0, 96);
                }
                fechas.AddRange(dias);
            }

            maximo.data = listaMaximos.ToArray();
            maximo.diasHora = fechas;//.Select(str => Regex.Replace(str, @"\d+", "").Trim()).ToList();
            minimo.data = listaMinimos.ToArray();
            minimo.diasHora = fechas;//.Select(str => Regex.Replace(str, @"\d+", "").Trim()).ToList();

            //Encontrar las posiciones para las quinenas
            posiciones = fechas
                .Select((str, index) => new { String = str, Index = index })
                .Where(item => item.String.EndsWith("15"))
                .Select(item => item.Index)
                .ToList();
        }

        /// <summary>
        /// Lista las barras con su punto tna relacionadas a una version.
        /// </summary>
        /// <param name="dpomedcodi">codigo del punto o barra</param>
        /// <param name="dpotmecodi">identificador para saber si la data es SIRIPIT, SICLI, TNA activa o reactiva</param>
        /// <param name="dpotdtcodi">identificador para saber si la data es un punto, barra , etc</param>
        /// <param name="vergrpcodi">identificador de la vesion</param>
        /// <param name="anio">anio que se consulta</param>
        /// <param name="mes">mes o meses</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> ListByFiltros(string dpomedcodi, int dpotmecodi, int dpotdtcodi, int vergrpcodi, int anio, string mes)
        {
            return FactorySic.GetDpoMedicion96Repository().ListByFiltros(dpomedcodi, dpotmecodi, dpotdtcodi, vergrpcodi, anio, mes);
        }

        /// <summary>
        /// Elimina un registro de la tabla DPO_MEDICION96 por medio de la version
        /// </summary>
        /// <param name="codigo">Identificador de la version</param>
        /// <returns></returns>
        public void DeleteDpoMedicion96ByVersion(int codigo)
        {
            try
            {
                FactorySic.GetDpoMedicion96Repository().DeleteByVersion(codigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite consultar la data para SIRPIT, SICLI y TNA
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="meses"></param>
        /// <param name="fuente"></param>
        /// <param name="variable"></param>
        /// <param name="carga"></param>
        /// <param name="versionOrigen"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> ConsultaCargas96(int anio, string meses, int fuente, int variable,
                                                       List<string> carga,
                                                       int versionOrigen, string usuario)
        {
            List<DpoMedicion96DTO> lista = new List<DpoMedicion96DTO>();
            #region Consultar la data
            //Se trata si la data es de origenm, por eso el -1
            if (versionOrigen == -1)
            {
                //Se consulta la data si se selecciona SIRPIT, SICLI o TNA
                if (fuente != 4)
                {
                    lista = this.ListaConsultaDataBasePrefiltra(anio, meses, fuente, variable,
                                                                carga, usuario);
                }
                //Se cosnulta la data si es Formula  
                else
                {
                    List<string> tna = carga.Where(x => x.StartsWith(ConstantesDpo.PrefijoTNA))
                                            .Select(y => y.Replace(ConstantesDpo.PrefijoTNA, "")).ToList();
                    List<string> sirpit = carga.Where(x => x.StartsWith(ConstantesDpo.PrefijoSIRPIT))
                                               .Select(y => y.Replace(ConstantesDpo.PrefijoSIRPIT, "")).ToList();
                    List<string> sicli = carga.Where(x => x.StartsWith(ConstantesDpo.PrefijoSICLI))
                                              .Select(y => y.Replace(ConstantesDpo.PrefijoSICLI, "")).ToList();
                    lista.AddRange(this.ListaConsultaDataBasePrefiltra(anio, meses, ConstantesDpo.fuenteSIRPIT, variable,
                                                                      sirpit, usuario));
                    lista.AddRange(this.ListaConsultaDataBasePrefiltra(anio, meses, ConstantesDpo.fuenteSICLI, variable,
                                                                      sicli, usuario));
                    lista.AddRange(this.ListaConsultaDataBasePrefiltra(anio, meses, ConstantesDpo.fuenteTNA, variable,
                                                                      tna, usuario));
                }
            }
            //Se consulta la data en la nueva tabla DPO_MEDICION96
            else
            {
                //Se consulta la data si se selecciona SIRPIT, SICLI o TNA
                if (fuente != 4)
                {
                    lista = this.ListaConsultaDataVersionPrefiltra(anio, meses, fuente, variable,
                                                                   carga, versionOrigen);
                }
                ////Se cosnulta la data si es Formula  
                else
                {
                    List<string> tna = carga.Where(x => x.StartsWith(ConstantesDpo.PrefijoTNA))
                                            .Select(y => y.Replace(ConstantesDpo.PrefijoTNA, "")).ToList();
                    List<string> sirpit = carga.Where(x => x.StartsWith(ConstantesDpo.PrefijoSIRPIT))
                                               .Select(y => y.Replace(ConstantesDpo.PrefijoSIRPIT, "")).ToList();
                    List<string> sicli = carga.Where(x => x.StartsWith(ConstantesDpo.PrefijoSICLI))
                                              .Select(y => y.Replace(ConstantesDpo.PrefijoSICLI, "")).ToList();
                    lista.AddRange(this.ListaConsultaDataVersionPrefiltra(anio, meses, ConstantesDpo.fuenteSIRPIT, variable,
                                                                      sirpit, versionOrigen));
                    lista.AddRange(this.ListaConsultaDataVersionPrefiltra(anio, meses, ConstantesDpo.fuenteSICLI, variable,
                                                                      sicli, versionOrigen));
                    lista.AddRange(this.ListaConsultaDataVersionPrefiltra(anio, meses, ConstantesDpo.fuenteTNA, variable,
                                                                      tna, versionOrigen));
                }
            }
            #endregion

            return lista;
        }


        /// <summary>
        /// Permite grabar data corregida como una  nueva versión o actualizar una existente
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="fuente"></param>
        /// <param name="variable"></param>
        /// <param name="formula"></param>
        /// <param name="carga"></param>
        /// <param name="metodo"></param>
        /// <param name="version"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> ConsultaDatosPorPunto(int anio, string[] mes, int fuente, int variable,
                                                            int formula, List<string> carga, int metodo,
                                                            int version, string usuario)
        {
            string meses = string.Join(", ", mes.Select(x => $"'{x}'"));
            List<DpoMedicion96DTO> lista = new List<DpoMedicion96DTO>();
            //Consultar la data          
            lista = this.ConsultaCargas96(anio, meses, fuente, variable, carga, version, usuario);

            return lista;
        }

        /// <summary>
        /// Permite grabar data corregida como una  nueva versión o actualizar una existente
        /// </summary>
        /// <param name="metodo">metodo para corregir seleccionado</param>
        /// <param name="data">Conjunto de datos(universo)</param>
        /// <param name="tna">codigos tna que pertenecen la formula</param>
        /// <param name="sicli">codigos sicli que pertenecen la formula</param>
        /// <param name="sirpit">codigos sirpit que pertenecen la formula</param>
        /// <param name="fuente">tipo de fuente</param>
        /// <param name="codigo">codigo de la relacion donde se encuentra el punto TNA para el reparto</param>
        /// <param name="anio">anio de la consulta</param>
        /// <param name="mes">meses de la cosnulta</param>
        /// <param name="variable">indica si se trabaja con activa o reactiva</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> CorrecionDatosFormulas(int metodo, List<DpoMedicion96DTO> data, List<int> tna,
                                                             List<int> sicli, List<int> sirpit, int fuente, int codigo,
                                                             int anio, string[] mes, int variable)
        {
            List<DpoMedicion96DTO> lista = new List<DpoMedicion96DTO>();
            List<int> tipo = new List<int>();
            string meses = string.Join(", ", mes.Select(x => $"'{x}'"));
            //Inicio - Armando las fechas de consulta para el hsitorico
            DateTime tempFechaInicio = new DateTime(anio, Int32.Parse(mes.First()), 1);
            DateTime fechaInicio = tempFechaInicio.AddMonths(-12);//AJUSTAR A LOS MESES QUE SE DESEA TENER DE HISTORICO
            DateTime fechaFin = new DateTime(anio, Int32.Parse(mes.Last()), DateTime.DaysInMonth(anio, Int32.Parse(mes.Last())));
            //Fin - Armando las fechas de consulta para el hsitorico

            List<DpoMedicion96DTO> result = new List<DpoMedicion96DTO>();

            switch (fuente)
            {
                //Para data SIRPIT
                case ConstantesDpo.fuenteSIRPIT:
                    lista = data.Where(x => sirpit.Contains(x.Dpomedcodi)).ToList();
                    if (metodo == 3 && lista.Count > 0)
                    {
                        result = this.CorrecionMetodoDatosSirpitTNA(metodo, lista, fuente, sirpit, codigo, anio,
                                                                        mes, variable, fechaInicio, fechaFin);
                    }
                    else if (metodo == 2)
                    {
                        foreach (var m in mes)
                        {
                            List<DpoMedicion96DTO> dataMes = lista.Where(x => x.Dpomedfecha.Month == Int32.Parse(m)).ToList();
                            foreach (var s in sirpit)
                            {
                                List<DpoMedicion96DTO> dataPunto = dataMes.Where(x => x.Dpomedcodi == s).ToList();
                                result.AddRange(this.CorreccionCambiosDiasTipicos(dataPunto, anio));
                            }
                        }
                    }
                    else if (metodo == 1)
                    {
                        foreach (var m in mes)
                        {
                            List<DpoMedicion96DTO> dataMes = lista.Where(x => x.Dpomedfecha.Month == Int32.Parse(m)).ToList();
                            foreach (var s in sirpit)
                            {
                                List<DpoMedicion96DTO> dataPunto = dataMes.Where(x => x.Dpomedcodi == s).ToList();
                                result.AddRange(this.CorreccionCambiosBruscos(dataPunto, anio));
                            }
                        }
                    }
                    result.AddRange(data.Where(x => tna.Contains(x.Dpomedcodi)).ToList());
                    result.AddRange(data.Where(x => sicli.Contains(x.Dpomedcodi)).ToList());
                    break;
                //Para data SICLI
                case ConstantesDpo.fuenteSICLI:
                    lista = data.Where(x => sicli.Contains(x.Dpomedcodi)).ToList();
                    if (metodo == 3 && lista.Count > 0)
                    {
                        result = this.CorrecionMetodoDatosSicliTNA(metodo, lista, fuente, sicli, codigo, anio,
                                                                        mes, variable, fechaInicio, fechaFin);
                    }
                    else if (metodo == 2)
                    {
                        //result = this.CorrecionDiaTipico96(lista, codigo, fechaInicio, fechaFin);
                        foreach (var m in mes)
                        {
                            List<DpoMedicion96DTO> dataMes = lista.Where(x => x.Dpomedfecha.Month == Int32.Parse(m)).ToList();
                            foreach (var s in sicli)
                            {
                                List<DpoMedicion96DTO> dataPunto = dataMes.Where(x => x.Dpomedcodi == s).ToList();
                                result.AddRange(this.CorreccionCambiosDiasTipicos(dataPunto, anio));
                            }
                        }
                    }
                    else if (metodo == 1)
                    {
                        foreach (var m in mes)
                        {
                            List<DpoMedicion96DTO> dataMes = lista.Where(x => x.Dpomedfecha.Month == Int32.Parse(m)).ToList();
                            foreach (var s in sicli)
                            {
                                List<DpoMedicion96DTO> dataPunto = dataMes.Where(x => x.Dpomedcodi == s).ToList();
                                result.AddRange(this.CorreccionCambiosBruscos(dataPunto, anio));
                            }
                        }
                    }
                    result.AddRange(data.Where(x => sirpit.Contains(x.Dpomedcodi)).ToList());
                    result.AddRange(data.Where(x => tna.Contains(x.Dpomedcodi)).ToList());
                    break;
                case ConstantesDpo.fuenteFormulaSPL:
                    List<DpoMedicion96DTO> listaSirpit = data.Where(x => sirpit.Contains(x.Dpomedcodi)).ToList();
                    if (metodo == 3 && listaSirpit.Count > 0)
                    {
                        result.AddRange(this.CorrecionMetodoDatosSirpitTNA(metodo, listaSirpit, fuente, sirpit, codigo, anio,
                                                                        mes, variable, fechaInicio, fechaFin));
                    }
                    else if (metodo == 2)
                    {
                        foreach (var m in mes)
                        {
                            List<DpoMedicion96DTO> dataMes = listaSirpit.Where(x => x.Dpomedfecha.Month == Int32.Parse(m)).ToList();
                            foreach (var s in sirpit)
                            {
                                List<DpoMedicion96DTO> dataPunto = dataMes.Where(x => x.Dpomedcodi == s).ToList();
                                result.AddRange(this.CorreccionCambiosDiasTipicos(dataPunto, anio));
                            }
                        }
                    }
                    else if (metodo == 1)
                    {
                        foreach (var m in mes)
                        {
                            List<DpoMedicion96DTO> dataMes = listaSirpit.Where(x => x.Dpomedfecha.Month == Int32.Parse(m)).ToList();
                            foreach (var s in sirpit)
                            {
                                List<DpoMedicion96DTO> dataPunto = dataMes.Where(x => x.Dpomedcodi == s).ToList();
                                result.AddRange(this.CorreccionCambiosBruscos(dataPunto, anio));
                            }
                        }
                    }
                    //Parte SICLI
                    List<DpoMedicion96DTO> listaSicli = data.Where(x => sicli.Contains(x.Dpomedcodi)).ToList();
                    if (metodo == 3 && listaSicli.Count > 0)
                    {
                        result.AddRange(this.CorrecionMetodoDatosSicliTNA(metodo, listaSicli, fuente, sicli, codigo, anio,
                                                                        mes, variable, fechaInicio, fechaFin));
                    }
                    else if (metodo == 2)
                    {
                        foreach (var m in mes)
                        {
                            List<DpoMedicion96DTO> dataMes = listaSicli.Where(x => x.Dpomedfecha.Month == Int32.Parse(m)).ToList();
                            foreach (var s in sicli)
                            {
                                List<DpoMedicion96DTO> dataPunto = dataMes.Where(x => x.Dpomedcodi == s).ToList();
                                result.AddRange(this.CorreccionCambiosDiasTipicos(dataPunto, anio));
                            }
                        }
                    }
                    else if (metodo == 1)
                    {
                        foreach (var m in mes)
                        {
                            List<DpoMedicion96DTO> dataMes = listaSicli.Where(x => x.Dpomedfecha.Month == Int32.Parse(m)).ToList();
                            foreach (var s in sicli)
                            {
                                List<DpoMedicion96DTO> dataPunto = dataMes.Where(x => x.Dpomedcodi == s).ToList();
                                result.AddRange(this.CorreccionCambiosBruscos(dataPunto, anio));
                            }
                        }
                    }
                    //Parte TNA
                    result.AddRange(data.Where(x => tna.Contains(x.Dpomedcodi)).ToList());

                    break;
            }

            return result;
        }


        /// <summary>
        /// Devuelve los datos SPL segun días típicos
        /// </summary>
        /// <param name="lista">Data a corregir</param>
        /// <param name="anio">anio de la data consultada</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> CorreccionCambiosDiasTipicos(List<DpoMedicion96DTO> lista, int anio)
        {

            List<DpoMedicion96DTO> lunes = lista.Where(x => x.Dpomedfecha.DayOfWeek == DayOfWeek.Monday).ToList();
            List<DpoMedicion96DTO> viernes = lista.Where(x => x.Dpomedfecha.DayOfWeek == DayOfWeek.Friday).ToList();
            List<DpoMedicion96DTO> sabado = lista.Where(x => x.Dpomedfecha.DayOfWeek == DayOfWeek.Saturday).ToList();
            List<DpoMedicion96DTO> domingo = lista.Where(x => x.Dpomedfecha.DayOfWeek == DayOfWeek.Sunday).ToList();
            List<DpoMedicion96DTO> otros = lista.Where(x => x.Dpomedfecha.DayOfWeek != DayOfWeek.Monday &&
                                                                      x.Dpomedfecha.DayOfWeek != DayOfWeek.Saturday &&
                                                                      x.Dpomedfecha.DayOfWeek != DayOfWeek.Friday &&
                                                                      x.Dpomedfecha.DayOfWeek != DayOfWeek.Sunday).ToList();
            try
            {
                //Correcion por dias tipicos, recordar que en este metodo se entra a un bucle,
                //se saldra cuando no se detecte datos fuera de los limites.
                decimal[] lunesInf2 = new decimal[ConstantesDpo.Itv15min];
                decimal[] lunesSup2 = new decimal[ConstantesDpo.Itv15min];
                List<DpoMedicion96DTO> lunesResult = (lunes.Count() > 0) ? this.CorreccionDiasTipicos
                                                                                      (lunes.OrderBy(x => x.Dpomedfecha)
                                                                                            .ToList(), anio, out lunesInf2, out lunesSup2)
                                                                               : new List<DpoMedicion96DTO>();

                decimal[] viernesInf2 = new decimal[ConstantesDpo.Itv15min];
                decimal[] viernesSup2 = new decimal[ConstantesDpo.Itv15min];
                List<DpoMedicion96DTO> viernesResult = (viernes.Count() > 0) ? this.CorreccionDiasTipicos
                                                                                      (viernes.OrderBy(x => x.Dpomedfecha)
                                                                                              .ToList(), anio, out viernesInf2, out viernesSup2)
                                                                                   : new List<DpoMedicion96DTO>();

                decimal[] sabadoInf2 = new decimal[ConstantesDpo.Itv15min];
                decimal[] sabadoSup2 = new decimal[ConstantesDpo.Itv15min];
                List<DpoMedicion96DTO> sabadoResult = (sabado.Count() > 0) ? this.CorreccionDiasTipicos
                                                                                      (sabado.OrderBy(x => x.Dpomedfecha)
                                                                                             .ToList(), anio, out sabadoInf2, out sabadoSup2)
                                                                                 : new List<DpoMedicion96DTO>();

                decimal[] domingoInf2 = new decimal[ConstantesDpo.Itv15min];
                decimal[] domingoSup2 = new decimal[ConstantesDpo.Itv15min];
                List<DpoMedicion96DTO> domingoResult = (domingo.Count() > 0) ? this.CorreccionDiasTipicos
                                                                                      (domingo.OrderBy(x => x.Dpomedfecha)
                                                                                              .ToList(), anio, out domingoInf2, out domingoSup2)
                                                                                   : new List<DpoMedicion96DTO>();

                decimal[] otrosInf2 = new decimal[ConstantesDpo.Itv15min];
                decimal[] otrosSup2 = new decimal[ConstantesDpo.Itv15min];
                List<DpoMedicion96DTO> otrosResult = (otros.Count() > 0) ? this.CorreccionDiasTipicos
                                                                                      (otros.OrderBy(x => x.Dpomedfecha)
                                                                                            .ToList(), anio, out otrosInf2, out otrosSup2)
                                                                               : new List<DpoMedicion96DTO>();

                //Corregiendo datos con los delta
                List<DpoMedicion96DTO> lunesCorregido = this.CorreccionIntervalosFaltantesDelta(lunesResult, lunesInf2, lunesSup2);
                List<DpoMedicion96DTO> viernesCorregido = this.CorreccionIntervalosFaltantesDelta(viernesResult, viernesInf2, viernesSup2);
                List<DpoMedicion96DTO> sabadoCorregido = this.CorreccionIntervalosFaltantesDelta(sabadoResult, sabadoInf2, sabadoSup2);
                List<DpoMedicion96DTO> domingoCorregido = this.CorreccionIntervalosFaltantesDelta(domingoResult, domingoInf2, domingoSup2);
                List<DpoMedicion96DTO> otrosCorregido = this.CorreccionIntervalosFaltantesDelta(otrosResult, otrosInf2, otrosSup2);

                List<DpoMedicion96DTO> result = new List<DpoMedicion96DTO>();
                if (lunes.Count() > 0) { result.AddRange(lunesCorregido); }
                if (viernes.Count() > 0) { result.AddRange(viernesCorregido); }
                if (sabado.Count() > 0) { result.AddRange(sabadoCorregido); }
                if (domingo.Count() > 0) { result.AddRange(domingoCorregido); }
                if (otros.Count() > 0) { result.AddRange(otrosCorregido); }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Devuelve los datos SPL segun días típicos
        /// </summary>
        /// <param name="lista">Data a corregir</param>
        /// <param name="anio">anio de la data consultada</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> CorreccionCambiosBruscos(List<DpoMedicion96DTO> lista, int anio)
        {

            List<DpoMedicion96DTO> lunes = lista.Where(x => x.Dpomedfecha.DayOfWeek == DayOfWeek.Monday).ToList();
            List<DpoMedicion96DTO> viernes = lista.Where(x => x.Dpomedfecha.DayOfWeek == DayOfWeek.Friday).ToList();
            List<DpoMedicion96DTO> sabado = lista.Where(x => x.Dpomedfecha.DayOfWeek == DayOfWeek.Saturday).ToList();
            List<DpoMedicion96DTO> domingo = lista.Where(x => x.Dpomedfecha.DayOfWeek == DayOfWeek.Sunday).ToList();
            List<DpoMedicion96DTO> otros = lista.Where(x => x.Dpomedfecha.DayOfWeek != DayOfWeek.Monday &&
                                                                      x.Dpomedfecha.DayOfWeek != DayOfWeek.Saturday &&
                                                                      x.Dpomedfecha.DayOfWeek != DayOfWeek.Friday &&
                                                                      x.Dpomedfecha.DayOfWeek != DayOfWeek.Sunday).ToList();
            try
            {
                //Corrigiendo Maximos y Miniimos
                List<DpoMedicion96DTO> lunesMaxMin = (lunes.Count() > 0) ? this.CorreccionMaximosMinimos(lunes) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> viernesMaxMin = (viernes.Count() > 0) ? this.CorreccionMaximosMinimos(viernes) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> sabadoMaxMin = (sabado.Count() > 0) ? this.CorreccionMaximosMinimos(sabado) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> domingoMaxMin = (domingo.Count() > 0) ? this.CorreccionMaximosMinimos(domingo) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> otrosMaxMin = (otros.Count() > 0) ? this.CorreccionMaximosMinimos(otros) : new List<DpoMedicion96DTO>();

                //Corrigiendo Rampa
                List<DpoMedicion96DTO> lunesRampa = (lunes.Count() > 0) ? this.CorreccionRampa(lunes) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> viernesRampa = (viernes.Count() > 0) ? this.CorreccionRampa(viernes) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> sabadoRampa = (sabado.Count() > 0) ? this.CorreccionRampa(sabado) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> domingoRampa = (domingo.Count() > 0) ? this.CorreccionRampa(domingo) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> otrosRampa = (otros.Count() > 0) ? this.CorreccionRampa(otros) : new List<DpoMedicion96DTO>();

                //Comparando resultados, buscando nulos en lunesRampa
                //para grabarlos en lunesMaxMin y asi con todos los dias
                List<DpoMedicion96DTO> lunesBruscos = (lunes.Count() > 0) ? this.ComparacionRampaMaxMin(lunesMaxMin, lunesRampa) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> viernesBruscos = (viernes.Count() > 0) ? this.ComparacionRampaMaxMin(viernesMaxMin, viernesRampa) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> sabadoBruscos = (sabado.Count() > 0) ? this.ComparacionRampaMaxMin(sabadoMaxMin, sabadoRampa) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> domingoBruscos = (domingo.Count() > 0) ? this.ComparacionRampaMaxMin(domingoMaxMin, domingoRampa) : new List<DpoMedicion96DTO>();
                List<DpoMedicion96DTO> otrosBruscos = (otros.Count() > 0) ? this.ComparacionRampaMaxMin(otrosMaxMin, otrosRampa) : new List<DpoMedicion96DTO>();

                //Correcion por dias tipicos, recordar que en este metodo se entra a un bucle,
                //se saldra cuando no se detecte datos fuera de los limites.
                decimal[] lunesInf2 = new decimal[ConstantesDpo.Itv15min];
                decimal[] lunesSup2 = new decimal[ConstantesDpo.Itv15min];
                List<DpoMedicion96DTO> lunesResult = (lunesBruscos.Count() > 0) ? this.CorreccionDiasTipicos
                                                                                      (lunesBruscos.OrderBy(x => x.Dpomedfecha)
                                                                                                   .ToList(), anio, out lunesInf2, out lunesSup2) 
                                                                                : new List<DpoMedicion96DTO>();

                decimal[] viernesInf2 = new decimal[ConstantesDpo.Itv15min];
                decimal[] viernesSup2 = new decimal[ConstantesDpo.Itv15min];
                List<DpoMedicion96DTO> viernesResult = (viernesBruscos.Count() > 0) ? this.CorreccionDiasTipicos
                                                                                          (viernesBruscos.OrderBy(x => x.Dpomedfecha)
                                                                                                         .ToList(), anio, out viernesInf2, out viernesSup2) 
                                                                                    : new List<DpoMedicion96DTO>();

                decimal[] sabadoInf2 = new decimal[ConstantesDpo.Itv15min];
                decimal[] sabadoSup2 = new decimal[ConstantesDpo.Itv15min];
                List<DpoMedicion96DTO> sabadoResult = (sabadoBruscos.Count() > 0) ? this.CorreccionDiasTipicos
                                                                                        (sabadoBruscos.OrderBy(x => x.Dpomedfecha)
                                                                                                      .ToList(), anio, out sabadoInf2, out sabadoSup2) 
                                                                                  : new List<DpoMedicion96DTO>();

                decimal[] domingoInf2 = new decimal[ConstantesDpo.Itv15min];
                decimal[] domingoSup2 = new decimal[ConstantesDpo.Itv15min];
                List<DpoMedicion96DTO> domingoResult = (domingoBruscos.Count() > 0) ? this.CorreccionDiasTipicos
                                                                                          (domingoBruscos.OrderBy(x => x.Dpomedfecha)
                                                                                                         .ToList(), anio, out domingoInf2, out domingoSup2) 
                                                                                    : new List<DpoMedicion96DTO>();

                decimal[] otrosInf2 = new decimal[ConstantesDpo.Itv15min];
                decimal[] otrosSup2 = new decimal[ConstantesDpo.Itv15min];
                List<DpoMedicion96DTO> otrosResult = (otrosBruscos.Count() > 0) ? this.CorreccionDiasTipicos
                                                                                      (otrosBruscos.OrderBy(x => x.Dpomedfecha)
                                                                                                   .ToList(), anio, out otrosInf2, out otrosSup2) 
                                                                                : new List<DpoMedicion96DTO>();

                //Corregiendo datos con los delta
                List<DpoMedicion96DTO> lunesCorregido = this.CorreccionIntervalosFaltantesDelta(lunesResult, lunesInf2, lunesSup2);
                List<DpoMedicion96DTO> viernesCorregido = this.CorreccionIntervalosFaltantesDelta(viernesResult, viernesInf2, viernesSup2);
                List<DpoMedicion96DTO> sabadoCorregido = this.CorreccionIntervalosFaltantesDelta(sabadoResult, sabadoInf2, sabadoSup2);
                List<DpoMedicion96DTO> domingoCorregido = this.CorreccionIntervalosFaltantesDelta(domingoResult, domingoInf2, domingoSup2);
                List<DpoMedicion96DTO> otrosCorregido = this.CorreccionIntervalosFaltantesDelta(otrosResult, otrosInf2, otrosSup2);

                List<DpoMedicion96DTO> result = new List<DpoMedicion96DTO>();
                if (lunes.Count() > 0) { result.AddRange(lunesCorregido); }
                if (viernes.Count() > 0) { result.AddRange(viernesCorregido); }
                if (sabado.Count() > 0) { result.AddRange(sabadoCorregido); }
                if (domingo.Count() > 0) { result.AddRange(domingoCorregido); }
                if (otros.Count() > 0) { result.AddRange(otrosCorregido); }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Corrige los vacios en un grupo de mediciones SCO agrupadas por tipo
        /// </summary>
        /// <param name="datosMediciones">Datos de las mediciones a corregir</param>
        /// <param name="inferior">Datos de las mediciones a corregir</param>
        /// <param name="superior">Datos de las mediciones a corregir</param>
        public List<DpoMedicion96DTO> CorreccionIntervalosFaltantesDelta(List<DpoMedicion96DTO> datosMediciones, decimal[] inferior, decimal[] superior)
        {
            decimal?[] promedioMedicion = Enumerable.Range(1, ConstantesDpo.Itv15min)
                                        .Select(index => datosMediciones
                                        .Select(dto => dto.GetType()
                                        .GetProperty($"H{index}").GetValue(dto) as decimal?)
                                            .Average())
                                        .ToArray();

            for (int i = 0; i < promedioMedicion.Length; i++)
            {
                if (promedioMedicion[i] == null)
                {
                    decimal a = 0m;
                    if (i == 0)
                    {
                        for (int k = 1; k < promedioMedicion.Length; k++)
                        {
                            if (promedioMedicion[k] != null)
                            {
                                a = (decimal)promedioMedicion[k];
                            }
                        }
                    }
                    else {
                        a = (decimal)promedioMedicion[i - 1];
                    }                    
                    decimal b = 0m;
                    for (int j = i + 1; j < promedioMedicion.Length; j++)
                    {
                        if (promedioMedicion[j] != null) 
                        {
                            b = (decimal)promedioMedicion[j];
                        }
                    }
                    promedioMedicion[i] = (a + b) / 2;
                }
            }

            int[] valPosicion = {15, 30, 45, 100, 115, 130, 145, 200,
                                 215, 230, 245, 300, 315, 330, 345, 400,
                                 415, 430, 445, 500, 515, 530, 545, 600,
                                 615, 630, 645, 700, 715, 730, 745, 800,
                                 815, 830, 845, 900, 915, 930, 945, 1000,
                                 1015, 1030, 1045, 1100, 1115, 1130, 1145, 1200,
                                 1215, 1230, 1245, 1300, 1315, 1330, 1345, 1400,
                                 1415, 1430, 1445, 1500, 1515, 1530, 1545, 1600,
                                 1615, 1630, 1645, 1700, 1715, 1730, 1745, 1800,
                                 1815, 1830, 1845, 1900, 1915, 1930, 1945, 2000,
                                 2015, 2030, 2045, 2100, 2115, 2130, 2145, 2200,
                                 2215, 2230, 2245, 2300, 2315, 2330, 2345, 2400};

            foreach (DpoMedicion96DTO item in datosMediciones)
            {
                //Evaluando intervalo por intervalo
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    decimal?[] value = UtilDpo.ConvertirMedicionNuloEnArreglo(ConstantesDpo.Itv15min, item);
                    string rangoFaltante = string.Empty;

                    //Si todos los valores son nulos
                    if (value.All(x => x == null))
                        rangoFaltante = "todo";
                    //Si es nulo y esta en primera posicion 0(H1)
                    else if (i == 0 && value[0] == null)
                        rangoFaltante = "ini";
                    //Si es nulo y esta en ultima posicion 95(H96)
                    else if (i == 95 && value[95] == null)
                        rangoFaltante = "fin";
                    else
                    {
                        if (value[i] == null) 
                        {
                            if (value[i - 1] != null && value[i + 1] != null)
                                rangoFaltante = "medUno";
                            else
                                rangoFaltante = "medRng";
                        }
                    }

                    //Corrigiendo si toda la data para una fecha es nulo
                    if (rangoFaltante == "todo") {
                        decimal deltaInferior = this.CalculoDelta(0, (decimal)promedioMedicion[95], -15, (decimal)promedioMedicion[94], 15);
                        decimal deltaSuperior = this.CalculoDelta(2415, (decimal)promedioMedicion[0], 2430, (decimal)promedioMedicion[1], 2400);
                        decimal extremoInferior = deltaInferior - (decimal)promedioMedicion[0];
                        decimal extremoSuperior = deltaSuperior - (decimal)promedioMedicion[95];
                        item.GetType().GetProperty("H" + 1).SetValue(item, (decimal)promedioMedicion[0] + extremoInferior);
                        item.GetType().GetProperty("H" + 96).SetValue(item, (decimal)promedioMedicion[95] + extremoSuperior);

                        for (int j = 2; j < ConstantesDpo.Itv15min; j++)
                        {
                            decimal reemplazo = ((j - 1) * (-1 * extremoInferior + extremoSuperior) / 95) + extremoInferior;
                            item.GetType().GetProperty("H" + j.ToString()).SetValue(item, (decimal)promedioMedicion[j - 1] + reemplazo);
                        }

                        continue;
                    }
                    if (rangoFaltante == "ini") 
                    {
                        decimal deltaInferior = this.CalculoDelta(0, (decimal)promedioMedicion[95], -15, (decimal)promedioMedicion[94], 15);
                        decimal extremoInferior = deltaInferior - (decimal)promedioMedicion[0];
                        item.GetType().GetProperty("H" + 1).SetValue(item, (decimal)promedioMedicion[0] + extremoInferior);
                    }
                    if (rangoFaltante == "fin") 
                    {
                        decimal deltaSuperior = this.CalculoDelta(2415, (decimal)promedioMedicion[0], 2430, (decimal)promedioMedicion[1], 2400);
                        decimal extremoSuperior = deltaSuperior - (decimal)promedioMedicion[95];
                        item.GetType().GetProperty("H" + 96).SetValue(item, (decimal)promedioMedicion[95] + extremoSuperior);
                    }
                    if (rangoFaltante == "medUno") 
                    {
                        decimal anterior = (decimal)value[i - 1];//(decimal)item.GetType().GetProperty("H" + (i - 1).ToString()).GetValue(item, null);
                        decimal posterior = (decimal)value[i + 1];//item.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(item, null);
                        item.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(item, (anterior + posterior)/2);
                    }
                    if (rangoFaltante == "medRng") 
                    {
                        decimal deltaInferior;
                        decimal extremoInferior;
                        if (i == 1)
                        {
                            deltaInferior = this.CalculoDelta(15, (decimal)value[0], 0, (decimal)promedioMedicion[95], 30);
                            extremoInferior = deltaInferior - (decimal)promedioMedicion[1];
                        }
                        else {
                            deltaInferior = this.CalculoDelta(valPosicion[i - 2], (decimal)value[i - 2], valPosicion[i - 1],
                                              (decimal)value[i - 1], valPosicion[i]);
                            extremoInferior = deltaInferior - (decimal)promedioMedicion[i];
                        }
                        item.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(item, (decimal)promedioMedicion[i] + extremoInferior);

                        //Calcular hasta donde llega los nulos
                        int ultimoNulo = 0;
                        for (int j = (i + 1); j < ConstantesDpo.Itv15min; j++)
                        {
                            decimal? n = value[j];
                            if (n != null)
                            {
                                ultimoNulo = j - 1;
                                break;
                            }
                        }
                        //Calculando los 2 sgtes valores para el delta
                        decimal posterior1 = (decimal)value[ultimoNulo + 1];
                        decimal posterior2 = 0m;
                        int posicion2 = 0;
                        //Validando que ultimoNulo sea 94
                        if (ultimoNulo == 94)
                        {
                            posterior2 = (decimal)promedioMedicion[0];
                            posicion2 = 2415;
                        }
                        else
                        {
                            posterior2 = (value[ultimoNulo + 2] == null) ? (decimal)promedioMedicion[ultimoNulo + 2] : (decimal)value[ultimoNulo + 2];
                            posicion2 = valPosicion[ultimoNulo + 2];
                        }

                        decimal deltaSuperior = this.CalculoDelta(valPosicion[ultimoNulo + 1], posterior1, posicion2, posterior2, valPosicion[ultimoNulo]);
                        decimal extremoSuperior = deltaSuperior - (decimal)promedioMedicion[ultimoNulo];

                        item.GetType().GetProperty("H" + 2).SetValue(item, (decimal)promedioMedicion[1] + extremoInferior);
                        item.GetType().GetProperty("H" + (ultimoNulo + 1).ToString()).SetValue(item, (decimal)promedioMedicion[ultimoNulo] + extremoSuperior);
                        //recorremos la clase para completar los valores
                        for (int j = 3; j < ultimoNulo + 1; j++)
                        {
                            decimal reemplazo = ((j - 1) * (-1 * extremoInferior + extremoSuperior) / (ultimoNulo - 1)) + extremoInferior;
                            item.GetType().GetProperty("H" + j.ToString()).SetValue(item, (decimal)promedioMedicion[j - 1] + reemplazo);
                        }
                        i = ultimoNulo;
                    }
                }
            }

            foreach (DpoMedicion96DTO item in datosMediciones)
            {
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    decimal? dato = (decimal?)item.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(item, null);
                    if (dato != null)
                    {
                        if (inferior[i] != 0 && dato < inferior[i]) { item.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(item, inferior[i]); }
                        if (superior[i] != 0 && dato > superior[i]) { item.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(item, superior[i]); }
                    }
                }
            }

            return datosMediciones;
        }

        /// <summary>
        /// Devuelve los datos SPL segun días típicos
        /// </summary>
        /// <param name="x1">primer intervalo</param>
        /// <param name="y1">valor del primer intervalo</param>
        /// <param name="x2">segundo intervalo</param>
        /// <param name="y2">valor del segundo intervalo</param>
        /// <param name="intervalo">intervalo para el que se desea calcular</param>
        /// <returns></returns>
        public decimal CalculoDelta(int x1, decimal y1, int x2, decimal y2, int intervalo)
        {
            decimal result;

            decimal m = (y2 - y1) / (x2 - x1);//pendiente de la recta
            decimal b = y1 - m * x1;//intercepto de la recta

            result = m * intervalo + b;

            return result;
        }

        /// <summary>
        /// Devuelve los datos SPL segun días típicos
        /// </summary>
        /// <param name="dataTemporal">Data a corregir</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> CorreccionMaximosMinimos(List<DpoMedicion96DTO> dataTemporal)
        {
            decimal promedioMax;
            decimal promedioMin;
            decimal desviacionMax;
            decimal desviacionMin;
            decimal limitSup1Max;
            decimal limitInf1Max;
            decimal limitSup1Min;
            decimal limitInf1Min;
            decimal cuartilMax;
            decimal cuartilMin;
            decimal limitSup2Max;
            decimal limitInf2Max;
            decimal limitSup2Min;
            decimal limitInf2Min;

            List<DpoMedicion96DTO> temporal = new List<DpoMedicion96DTO>();
            foreach (var dt in dataTemporal)
            {
                DpoMedicion96DTO dm = new DpoMedicion96DTO();
                dm.Dpomedcodi = dt.Dpomedcodi;
                dm.Dpomedfeccreacion = dt.Dpomedfeccreacion;
                dm.Dpomedfecha = dt.Dpomedfecha;
                dm.Dpomedfecmodificacion = dt.Dpomedfecmodificacion;
                dm.Dpomedusucreacion = dt.Dpomedusucreacion;
                dm.Dpomedusumodificacion = dt.Dpomedusumodificacion;
                dm.Dpotdtcodi = dt.Dpotdtcodi;
                dm.Dpotmecodi = dt.Dpotmecodi;
                dm.Vergrpcodi = dt.Vergrpcodi;
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    decimal? n = (decimal?)dt.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(dt, null);
                    dm.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(dm, n);
                }
                temporal.Add(dm);
            }


            List<decimal> maximos = new List<decimal>();
            List<decimal> minimos = new List<decimal>();
            foreach (var t in temporal)
            {
                decimal[] valores = UtilDpo.ConvertirMedicionEnArreglo(ConstantesDpo.Itv15min, t);
                maximos.Add(valores.Max());
                minimos.Add(valores.Min());
            }

            promedioMax = maximos.Average();
            promedioMin = minimos.Average();

            desviacionMax = maximos.Zip(Enumerable.Repeat(promedioMax, maximos.Count), (a, b) => a - b).ToArray().Average();
            desviacionMin = minimos.Zip(Enumerable.Repeat(promedioMin, minimos.Count), (a, b) => a - b).ToArray().Average();

            limitSup1Max = promedioMax + 3 * desviacionMax;
            limitSup1Min = promedioMin + 3 * desviacionMin;

            limitInf1Max = promedioMax - 3 * desviacionMax;
            limitInf1Min = promedioMin - 3 * desviacionMin;

            cuartilMax = UtilDpo.CuartilIncluido(maximos.ToArray(), 3) - UtilDpo.CuartilIncluido(maximos.ToArray(), 1);
            cuartilMin = UtilDpo.CuartilIncluido(minimos.ToArray(), 3) - UtilDpo.CuartilIncluido(minimos.ToArray(), 1);

            limitSup2Max = UtilDpo.CuartilIncluido(maximos.ToArray(), 3) + 3 * 1.5m * cuartilMax;
            limitSup2Min = UtilDpo.CuartilIncluido(minimos.ToArray(), 3) + 1.5m * cuartilMin;

            limitInf2Max = UtilDpo.CuartilIncluido(maximos.ToArray(), 3) - 1.5m * cuartilMax;
            limitInf2Min = UtilDpo.CuartilIncluido(minimos.ToArray(), 3) - 1.5m * cuartilMin;

            //Validades el limite inferior por si usar 0
            decimal menor = (limitInf1Min < limitSup2Min) ? limitInf1Min : limitSup2Min; 
            limitSup2Min = (limitInf1Min < 0 && limitSup2Min < 0) ? menor : limitSup2Min;


            foreach (var entity in temporal)
            {
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    decimal? val = (decimal?)entity.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(entity, null);
                    if (val < menor || val > limitSup2Max) {
                        entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, null);
                    }
                }
                
            }

            return temporal;
        }

        /// <summary>
        /// Devuelve los datos SPL segun días típicos
        /// </summary>
        /// <param name="dataTemporal">Data a corregir</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> CorreccionRampa(List<DpoMedicion96DTO> dataTemporal)
        {
            //variables para la lectura por intervalos
            decimal[] promedio = new decimal[ConstantesDpo.Itv15min];
            decimal[] desviacion = new decimal[ConstantesDpo.Itv15min];
            decimal[] limSup1 = new decimal[ConstantesDpo.Itv15min];
            decimal[] limInf1 = new decimal[ConstantesDpo.Itv15min];
            decimal[] cuartil = new decimal[ConstantesDpo.Itv15min];
            decimal[] limitInf2 = new decimal[ConstantesDpo.Itv15min];
            decimal[] limitSup2 = new decimal[ConstantesDpo.Itv15min];

            List<DpoMedicion96DTO> temporal = new List<DpoMedicion96DTO>();
            foreach (var dt in dataTemporal)
            {
                DpoMedicion96DTO dm = new DpoMedicion96DTO();
                dm.Dpomedcodi = dt.Dpomedcodi;
                dm.Dpomedfeccreacion = dt.Dpomedfeccreacion;
                dm.Dpomedfecha = dt.Dpomedfecha;
                dm.Dpomedfecmodificacion = dt.Dpomedfecmodificacion;
                dm.Dpomedusucreacion = dt.Dpomedusucreacion;
                dm.Dpomedusumodificacion = dt.Dpomedusumodificacion;
                dm.Dpotdtcodi = dt.Dpotdtcodi;
                dm.Dpotmecodi = dt.Dpotmecodi;
                dm.Vergrpcodi = dt.Vergrpcodi;
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    decimal? n = (decimal?)dt.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(dt, null);
                    dm.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(dm, n);
                }
                temporal.Add(dm);
            }


            //Calculando los datos para la rampa
            foreach (DpoMedicion96DTO t in temporal)
            {
                for (int i = ConstantesDpo.Itv15min; i >= 1; i--)
                {
                    if (i > 1)
                    {
                        decimal? post = (decimal?)t.GetType().GetProperty("H" + i.ToString()).GetValue(t, null);
                        decimal? ant = (decimal?)t.GetType().GetProperty("H" + (i - 1).ToString()).GetValue(t, null);
                        if (post == null && ant != null)
                        {
                            t.GetType().GetProperty("H" + i.ToString()).SetValue(t, Math.Abs((decimal)ant)); ;
                        }
                        if (ant == null && post != null)
                        {
                            t.GetType().GetProperty("H" + i.ToString()).SetValue(t, Math.Abs((decimal)post));
                        }
                        if (post == null && ant == null)
                        {
                            t.GetType().GetProperty("H" + i.ToString()).SetValue(t, null);
                        }
                        if (post != null && ant != null)
                        {
                            t.GetType().GetProperty("H" + i.ToString()).SetValue(t, Math.Abs((decimal)ant - (decimal)post));
                        }
                    }
                    else
                    {
                        t.GetType().GetProperty("H" + i.ToString()).SetValue(t, (decimal)0);
                    }
                }
            }


            //Promedio
            for (int i = 0; i < ConstantesDpo.Itv15min; i++)
            {
                List<decimal> intervalo = new List<decimal>();
                foreach (var temp in temporal)
                {
                    decimal? valor = (decimal?)temp.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(temp, null);
                    if (valor != null) 
                    {
                        intervalo.Add((decimal)valor);
                    }                    
                }
                if (intervalo.Count == 0)
                {
                    promedio[i] = 0;
                }
                else {
                    promedio[i] = intervalo.Average();
                }                
                //Cuartiles y sus limites
                cuartil[i] = UtilDpo.CuartilIncluido(intervalo.ToArray(), 3) - UtilDpo.CuartilIncluido(intervalo.ToArray(), 1);
                limitInf2[i] = UtilDpo.CuartilIncluido(intervalo.ToArray(), 1) - 1.5m * cuartil[i];
                limitSup2[i] = (UtilDpo.CuartilIncluido(intervalo.ToArray(), 3) + 1.5m * cuartil[i]) * 2;
            }

            //Desviacion
            for (int i = 0; i < ConstantesDpo.Itv15min; i++)
            {
                List<decimal> intervalo = new List<decimal>();
                foreach (var temp in temporal)
                {
                    decimal? valor = (decimal?)temp.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(temp, null);
                    if (valor != null)
                    {
                        intervalo.Add(Math.Abs((decimal)valor - promedio[i]));
                    }
                    else {
                        intervalo.Add(Math.Abs(promedio[i]));
                    }
                    
                }
                desviacion[i] = intervalo.Average();
            }

            //Calculando los limites Sup1 e Inf1
            limInf1 = promedio.Zip(desviacion, (a, b) => a - 3 * b).ToArray();
            limSup1 = promedio.Zip(desviacion, (a, b) => a + 3 * b).ToArray();

            foreach (var item in temporal)
            {
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    decimal? dia = (decimal?)item.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(item, null);
                    if (dia != null) {
                        if (dia <= limitInf2[i] || dia >= limitSup2[i]) {
                            dia = null;
                            item.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(item, dia);
                        }
                    }
                }
            }
            return temporal;
        }


        /// <summary>
        /// Devuelve los datos SPL segun días típicos
        /// </summary>
        /// <param name="dataTemporal">Data a corregir</param>
        /// <param name="anio">anio de la data a corregir</param>
        /// <param name="limitInf2">limite inferior</param>
        /// <param name="limitSup2">limite superior</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> CorreccionDiasTipicos(List<DpoMedicion96DTO> dataTemporal, int anio, out decimal[] limitInf2, out decimal[] limitSup2)
        {
            //variables para la lectura por intervalos
            decimal[] promedio = new decimal[ConstantesDpo.Itv15min];
            decimal[] desviacion = new decimal[ConstantesDpo.Itv15min];
            decimal[] limSup1 = new decimal[ConstantesDpo.Itv15min];
            decimal[] limInf1 = new decimal[ConstantesDpo.Itv15min];
            decimal[] cuartil = new decimal[ConstantesDpo.Itv15min];
            decimal[] limitInf2Temporal = new decimal[ConstantesDpo.Itv15min];
            decimal[] limitSup2Temporal = new decimal[ConstantesDpo.Itv15min];

            // Obtengo las fechas de los feriados filtradas por el año actual
            List<DateTime> feriados = this.ObtenerFeriadosPorAnio(anio);

            List<DpoMedicion96DTO> temporal = new List<DpoMedicion96DTO>();
            foreach (var dt in dataTemporal)
            {
                DpoMedicion96DTO dm = new DpoMedicion96DTO();
                dm.Dpomedcodi = dt.Dpomedcodi;
                dm.Dpomedfeccreacion = dt.Dpomedfeccreacion;
                dm.Dpomedfecha = dt.Dpomedfecha;
                dm.Dpomedfecmodificacion = dt.Dpomedfecmodificacion;
                dm.Dpomedusucreacion = dt.Dpomedusucreacion;
                dm.Dpomedusumodificacion = dt.Dpomedusumodificacion;
                dm.Dpotdtcodi = dt.Dpotdtcodi;
                dm.Dpotmecodi = dt.Dpotmecodi;
                dm.Vergrpcodi = dt.Vergrpcodi;
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    bool validar = feriados.Contains(dt.Dpomedfecha);
                    if (validar)
                    {
                        dm.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(dm, 0m);
                    }
                    else {
                        decimal? n = (decimal?)dt.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(dt, null);
                        dm.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(dm, n);
                    }
                }
                temporal.Add(dm);
            }

            // Se remueven los días feriados del año para el mes
            //temporal = temporal.Where(x => !(feriados.Contains(x.Dpomedfecha))).ToList();

            //Promedio
            for (int i = 0; i < ConstantesDpo.Itv15min; i++)
            {
                List<decimal> intervalo = new List<decimal>();
                foreach (var temp in temporal)
                {
                    decimal? valor = (decimal?)temp.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(temp, null);
                    if (valor != null)
                    {
                        intervalo.Add((decimal)valor);
                    }
                }
                if (intervalo.Count == 0)
                {
                    promedio[i] = 0;
                }
                else
                {
                    promedio[i] = intervalo.Average();
                }
                //Cuartiles y sus limites
                cuartil[i] = UtilDpo.CuartilIncluido(intervalo.ToArray(), 3) - UtilDpo.CuartilIncluido(intervalo.ToArray(), 1);
                limitInf2Temporal[i] = UtilDpo.CuartilIncluido(intervalo.ToArray(), 1) - 1.5m * cuartil[i];
                limitSup2Temporal[i] = (UtilDpo.CuartilIncluido(intervalo.ToArray(), 3) + 1.5m * cuartil[i]);
            }

            //Desviacion
            for (int i = 0; i < ConstantesDpo.Itv15min; i++)
            {
                List<decimal> intervalo = new List<decimal>();
                foreach (var temp in temporal)
                {
                    decimal? valor = (decimal?)temp.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(temp, null);
                    if (valor != null)
                    {
                        intervalo.Add(Math.Abs((decimal)valor - promedio[i]));
                    }
                    else
                    {
                        intervalo.Add(Math.Abs(promedio[i]));
                    }

                }
                desviacion[i] = intervalo.Average();
            }

            //Calculando los limites Sup1 e Inf1
            limInf1 = promedio.Zip(desviacion, (a, b) => a - 3 * b).ToArray();
            limSup1 = promedio.Zip(desviacion, (a, b) => a + 3 * b).ToArray();

            //Evaluando para setear nulos
            bool flag = false;

            foreach (var item in temporal)
            {
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    decimal? dia = (decimal?)item.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(item, null);
                    if (dia != null)
                    {
                        if (dia < limitInf2Temporal[i] || dia > limitSup2Temporal[i])
                        {
                            dia = null;
                            flag = true;
                            item.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(item, dia);
                        }
                    }
                }
            }

            List<DpoMedicion96DTO> feriadosNulos = temporal.Where(x => feriados.Contains(x.Dpomedfecha)).ToList();
            foreach (var nulos in feriadosNulos)
            {
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    nulos.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(nulos, null);
                }
            }

            if (flag) {
                this.CorreccionDiasTipicos(temporal, anio, out limitInf2Temporal, out limitSup2Temporal);
            }

            limitInf2 = limitInf2Temporal;
            limitSup2 = limitSup2Temporal;

            return temporal;
        }

        /// <summary>
        /// Devuelve los datos SPL segun días típicos
        /// </summary>
        /// <param name="diaMaxMin">Data corregida con maximos y minimos</param>
        /// <param name="diaRampa">Data corregida por rampa</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> ComparacionRampaMaxMin(List<DpoMedicion96DTO> diaMaxMin, List<DpoMedicion96DTO> diaRampa) 
        {

            //Comparando resultados, buscando nulos en lunesRampa
            //para grabarlos en lunesMaxMin y asi con todos los dias
            foreach (DpoMedicion96DTO item in diaRampa)
            {
                for (int i = 0; i < ConstantesDpo.Itv15min; i++)
                {
                    decimal? post = (decimal?)item.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(item, null);
                    if (post == null)
                    {
                        DateTime fecha = item.Dpomedfecha;
                        foreach (DpoMedicion96DTO m in diaMaxMin)
                        {
                            if (m.Dpomedfecha == fecha) {
                                m.GetType().GetProperty("H" + (i + 1).ToString())
                                           .SetValue(m, null);
                            }
                        }
                    }
                }
            }

            return diaMaxMin;
        }  

        /// <summary>
        /// Devuelve los datos SPL segun días típicos
        /// </summary>
        /// <param name="lista">Data a coregir</param>
        /// <param name="idPunto">Identificador del</param>
        /// <param name="fechaInicio">Fecha inicio del rango de busqueda</param>
        /// <param name="fechaFin">Fecha fin del rango de busqueda</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> CorrecionDiaTipico96(List<DpoMedicion96DTO> lista, int idPunto,
                                                        DateTime fechaInicio, DateTime fechaFin)
        {
            //Consulta de data historica para el TNA
            List<DpoRelacionPtoBarraDTO> dataPunto = this.ListaPuntoRelacionBarra(idPunto,
                                                                                  fechaInicio.ToString(ConstantesDpo.FormatoFecha),
                                                                                  fechaFin.ToString(ConstantesDpo.FormatoFecha));


            //Agrupando Lunes
            List<DpoRelacionPtoBarraDTO> tempLunes = dataPunto.Where(x => x.Medifecha.DayOfWeek == DayOfWeek.Monday).ToList();
            List<decimal[]> lunes = new List<decimal[]>();
            List<decimal[]> sabado = new List<decimal[]>();
            List<decimal[]> domingo = new List<decimal[]>();
            List<decimal[]> otros = new List<decimal[]>();
            foreach (var l in tempLunes)
            {
                lunes.Add(UtilProdem.ConvertirMedicionEnArreglo(ConstantesDpo.Itv15min, l));
            }
            //Agrupando Sabado
            List<DpoRelacionPtoBarraDTO> tempSabado = dataPunto.Where(x => x.Medifecha.DayOfWeek == DayOfWeek.Saturday).ToList();
            foreach (var s in tempSabado)
            {
                sabado.Add(UtilProdem.ConvertirMedicionEnArreglo(ConstantesDpo.Itv15min, s));
            }
            //Agrupando Domingo
            List<DpoRelacionPtoBarraDTO> tempDomingo = dataPunto.Where(x => x.Medifecha.DayOfWeek == DayOfWeek.Sunday).ToList();
            foreach (var d in tempDomingo)
            {
                domingo.Add(UtilProdem.ConvertirMedicionEnArreglo(ConstantesDpo.Itv15min, d));
            }
            //Agrupando Otros
            List<DpoRelacionPtoBarraDTO> tempOtros = dataPunto.Where(x => x.Medifecha.DayOfWeek != DayOfWeek.Monday &&
                                                                      x.Medifecha.DayOfWeek != DayOfWeek.Saturday &&
                                                                      x.Medifecha.DayOfWeek != DayOfWeek.Sunday).ToList();
            foreach (var o in tempOtros)
            {
                lunes.Add(UtilProdem.ConvertirMedicionEnArreglo(ConstantesDpo.Itv15min, o));
            }

            //Obtencion de promedios
            decimal[] proLunes = UtilDpo.Promedio96(lunes);
            decimal[] proSabado = UtilDpo.Promedio96(sabado);
            decimal[] proDomingo = UtilDpo.Promedio96(domingo);
            decimal[] proOtros = UtilDpo.Promedio96(otros);

            foreach (DpoMedicion96DTO item in lista)
            {
                int diaSemana = (int)item.Dpomedfecha.DayOfWeek;
                decimal[] promedio = new decimal[ConstantesDpo.Itv15min];

                if (diaSemana == (int)DayOfWeek.Monday)
                    promedio = proLunes;
                else if (diaSemana == (int)DayOfWeek.Saturday)
                    promedio = proSabado;
                else if (diaSemana == (int)DayOfWeek.Sunday)
                    promedio = proDomingo;
                else
                    promedio = proOtros;

                int i = 0;
                while (i < ConstantesDpo.Itv15min)
                {
                    decimal valor = (decimal)item.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(item, null);
                    if (valor == 0) item.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(item, promedio[i]);// = promedio[i];
                    i++;
                }
            }

            return lista;
        }

        /// <summary>
        /// Devuelve los datos SPL segun días típicos
        /// </summary>
        /// <param name="lista">Data a coregir</param>
        /// <param name="idPunto">Identificador del</param>
        /// <param name="fechaInicio">Fecha inicio del rango de busqueda</param>
        /// <param name="fechaFin">Fecha fin del rango de busqueda</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> CorrecionCambiosBruscos(List<DpoMedicion96DTO> lista, int idPunto,
                                                                DateTime fechaInicio, DateTime fechaFin)
        {
            //Consulta de data historica para el TNA
            List<DpoRelacionPtoBarraDTO> dataPunto = this.ListaPuntoRelacionBarra(idPunto,
                                                                                  fechaInicio.ToString(ConstantesDpo.FormatoFecha),
                                                                                  fechaFin.ToString(ConstantesDpo.FormatoFecha));


            List<decimal[]> lunes = new List<decimal[]>();
            List<decimal[]> sabado = new List<decimal[]>();
            List<decimal[]> domingo = new List<decimal[]>();
            List<decimal[]> otros = new List<decimal[]>();
            //Agrupando Lunes
            List<DpoRelacionPtoBarraDTO> tempLunes = dataPunto.Where(x => x.Medifecha.DayOfWeek == DayOfWeek.Monday).ToList();
            foreach (var l in tempLunes)
            {
                lunes.Add(UtilProdem.ConvertirMedicionEnArreglo(ConstantesDpo.Itv15min, l));
            }
            //Agrupando Sabado
            List<DpoRelacionPtoBarraDTO> tempSabado = dataPunto.Where(x => x.Medifecha.DayOfWeek == DayOfWeek.Saturday).ToList();
            foreach (var s in tempSabado)
            {
                sabado.Add(UtilProdem.ConvertirMedicionEnArreglo(ConstantesDpo.Itv15min, s));
            }
            //Agrupando Domingo
            List<DpoRelacionPtoBarraDTO> tempDomingo = dataPunto.Where(x => x.Medifecha.DayOfWeek == DayOfWeek.Sunday).ToList();
            foreach (var d in tempDomingo)
            {
                domingo.Add(UtilProdem.ConvertirMedicionEnArreglo(ConstantesDpo.Itv15min, d));
            }
            //Agrupando Otros
            List<DpoRelacionPtoBarraDTO> tempOtros = dataPunto.Where(x => x.Medifecha.DayOfWeek != DayOfWeek.Monday &&
                                                                      x.Medifecha.DayOfWeek != DayOfWeek.Saturday &&
                                                                      x.Medifecha.DayOfWeek != DayOfWeek.Sunday).ToList();
            foreach (var o in tempOtros)
            {
                lunes.Add(UtilProdem.ConvertirMedicionEnArreglo(ConstantesDpo.Itv15min, o));
            }

            //Obtencion de promedios
            decimal[] proLunes = UtilDpo.Promedio96(lunes);
            decimal[] proSabado = UtilDpo.Promedio96(sabado);
            decimal[] proDomingo = UtilDpo.Promedio96(domingo);
            decimal[] proOtros = UtilDpo.Promedio96(otros);

            foreach (DpoMedicion96DTO item in lista)
            {
                int diaSemana = (int)item.Dpomedfecha.DayOfWeek;
                decimal[] promedio = new decimal[ConstantesDpo.Itv15min];

                if (diaSemana == (int)DayOfWeek.Monday)
                    promedio = proLunes;
                else if (diaSemana == (int)DayOfWeek.Saturday)
                    promedio = proSabado;
                else if (diaSemana == (int)DayOfWeek.Sunday)
                    promedio = proDomingo;
                else
                    promedio = proOtros;

                int i = 0;
                while (i < ConstantesDpo.Itv15min)
                {
                    decimal valor = (decimal)item.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(item, null);
                    //Correcion carga cero o negativo 2.1
                    if (valor <= 0) item.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(item, promedio[i]);
                    //Correcion cuya maxima carga es extremandamene elevada respecto al promedio general de maximas 2.2
                    else if (valor > promedio.Average() * ConstantesDpo.LSExtremadamenteElevada) item.GetType().GetProperty("H" + (i + 1).ToString())
                                                                                                     .SetValue(item, promedio[i]);
                    i++;
                }
            }

            return lista;
        }

        /// <summary>
        /// Permite grabar data corregida como una  nueva versión o actualizar una existente
        /// </summary>
        /// <param name="metodo">metodo para corregir seleccionado</param>
        /// <param name="data">Conjunto de datos(universo)</param>
        /// <param name="fuente">tipo de fuente</param>
        /// <param name="tipo">indica las cargas(ids) de los tipos de info: tna, sirpit, sicli</param>
        /// <param name="codigo">codigo de la relacion donde se encuentra el punto TNA para el reparto</param>
        /// <param name="anio">anio de la consulta</param>
        /// <param name="mes">meses de la cosnulta</param>
        /// <param name="variable">indica si se trabaja con activa o reactiva</param>
        /// <param name="fechaInicio">fecha de inicio para el historico</param>
        /// <param name="fechaFin">fecha de fin para el historico</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> CorrecionMetodoDatosSirpitTNA(int metodo, List<DpoMedicion96DTO> data, int fuente,
                                                              List<int> tipo, int codigo, int anio, string[] mes, int variable,
                                                              DateTime fechaInicio, DateTime fechaFin)
        {
            List<DpoMedicion96DTO> result = new List<DpoMedicion96DTO>();
            //Consulta de data historica para el TNA
            List<DpoRelacionPtoBarraDTO> dataPunto = this.ListaPuntoRelacionBarra(codigo,
                                                                                  fechaInicio.ToString(ConstantesDpo.FormatoFecha),
                                                                                  fechaFin.ToString(ConstantesDpo.FormatoFecha));
            //Consulta de data historica para el SIRPIT
            string cargas = string.Join(", ", tipo.Select(x => $"'{x}'"));
            List<DpoDatos96DTO> dataSIRPIT = FactorySic.GetDpoDatos96Repository()
                                                            .ListSirpitByDateRange(cargas,
                                                                                   fechaInicio.ToString(ConstantesDpo.FormatoFecha),
                                                                                   fechaFin.ToString(ConstantesDpo.FormatoFecha),
                                                                                   variable.ToString());//new List<DpoDatos96DTO>();
            foreach (var m in mes)
            {
                //Fechas para iterar
                DateTime regInicio = new DateTime(anio, Int32.Parse(m), 1);//PONER 1 PARA EL PRIMER DIA
                DateTime regFin = new DateTime(anio, Int32.Parse(m), DateTime.DaysInMonth(anio, Int32.Parse(m)));

                while (regInicio <= regFin)
                {
                    foreach (var s in tipo)
                    {
                        DpoMedicion96DTO entity = data.Where(x => x.Dpomedcodi == s && x.Dpomedfecha == regInicio).FirstOrDefault();
                        if (entity == null)
                        {
                            entity = new DpoMedicion96DTO();

                            for (int i = 1; i <= 96; i++)
                            {
                                DateTime itvFecha = regInicio.AddDays(-7);
                                List<decimal> factores = new List<decimal>();
                                //var dValor = entity.GetType().GetProperty("H" + i.ToString()).GetValue(entity, null);
                                while (itvFecha >= fechaInicio)
                                {
                                    //dato anterior del TNA
                                    decimal historicoTNA = dataPunto.Where(x => x.Medifecha == itvFecha &&
                                                                                      x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                          .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                          .FirstOrDefault();
                                    //dato anterior del SIRPIT
                                    decimal historicoSIRPIT = dataSIRPIT.Where(x => x.Dpodatfecha == itvFecha &&
                                                                                         x.Tnfbarcodi == s &&
                                                                                         //x.Dpotnfcodi == s &&
                                                                                         x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                             .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                             .FirstOrDefault();

                                    if (historicoTNA != 0 && historicoSIRPIT != 0 && factores.Count < 4)
                                    {
                                        decimal f = historicoSIRPIT / historicoTNA;
                                        factores.Add(f);
                                    }
                                    itvFecha = itvFecha.AddDays(-7);
                                }
                                if (factores.Count == 0)
                                {
                                    entity.GetType().GetProperty("H" + i.ToString()).SetValue(entity, 0m);
                                }
                                else
                                {
                                    decimal promedio = factores.Average();
                                    decimal actualTNA = dataPunto.Where(x => x.Medifecha == regInicio &&
                                                                             x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                 .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                 .FirstOrDefault();
                                    entity.GetType().GetProperty("H" + i.ToString()).SetValue(entity, actualTNA * promedio);
                                }
                            }
                            entity.Dpomedcodi = s;
                            entity.Dpomedfecha = regInicio;
                            entity.Dpotmecodi = (variable == 1) ? ConstantesDpo.DpotmeSirpitActiva : ConstantesDpo.DpotmeSirpitReactiva;
                            entity.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                            result.Add(entity);
                        }
                        else
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                DateTime itvFecha = regInicio.AddDays(-7);
                                List<decimal> factores = new List<decimal>();
                                decimal dValor = (decimal)entity.GetType().GetProperty("H" + i.ToString()).GetValue(entity, null);
                                if (dValor == 0)
                                {
                                    while (itvFecha >= fechaInicio)
                                    {
                                        //dato anterior del TNA
                                        decimal historicoTNA = dataPunto.Where(x => x.Medifecha == itvFecha &&
                                                                                          x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                              .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                              .FirstOrDefault();
                                        //dato anterior del SIRPIT
                                        decimal historicoSIRPIT = dataSIRPIT.Where(x => x.Dpodatfecha == itvFecha &&
                                                                                             x.Tnfbarcodi == s &&
                                                                                             //x.Dpotnfcodi == s &&
                                                                                             x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                                 .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                                 .FirstOrDefault();

                                        if (historicoTNA != 0 && historicoSIRPIT != 0 && factores.Count < 4)
                                        {
                                            decimal f = historicoSIRPIT / historicoTNA;
                                            factores.Add(f);
                                        }
                                        itvFecha = itvFecha.AddDays(-7);
                                    }
                                    if (factores.Count == 0)
                                    {
                                        entity.GetType().GetProperty("H" + i.ToString()).SetValue(entity, 0m);
                                    }
                                    else
                                    {
                                        decimal promedio = factores.Average();
                                        decimal actualTNA = dataPunto.Where(x => x.Medifecha == regInicio &&
                                                                                 x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                     .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                     .FirstOrDefault();
                                        entity.GetType().GetProperty("H" + i.ToString()).SetValue(entity, actualTNA * promedio);
                                    }
                                }
                            }
                            result.Add(entity);
                        }
                    }
                    regInicio = regInicio.AddDays(1);
                }
            }

            return result;
        }


        /// <summary>
        /// Permite grabar data corregida como una  nueva versión o actualizar una existente
        /// </summary>
        /// <param name="metodo">metodo para corregir seleccionado</param>
        /// <param name="data">Conjunto de datos(universo)</param>
        /// <param name="fuente">tipo de fuente</param>
        /// <param name="tipo">indica las cargas(ids) de los tipos de info: tna, sirpit, sicli</param>
        /// <param name="codigo">codigo de la relacion donde se encuentra el punto TNA para el reparto</param>
        /// <param name="anio">anio de la consulta</param>
        /// <param name="mes">meses de la cosnulta</param>
        /// <param name="variable">indica si se trabaja con activa o reactiva</param>
        /// <param name="fechaInicio">fecha de inicio para el historico</param>
        /// <param name="fechaFin">fecha de fin para el historico</param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> CorrecionMetodoDatosSicliTNA(int metodo, List<DpoMedicion96DTO> data, int fuente,
                                                              List<int> tipo, int codigo, int anio, string[] mes, int variable,
                                                              DateTime fechaInicio, DateTime fechaFin)
        {
            List<DpoMedicion96DTO> result = new List<DpoMedicion96DTO>();
            //Consulta de data historica para el TNA
            List<DpoRelacionPtoBarraDTO> dataPunto = this.ListaPuntoRelacionBarra(codigo,
                                                                                  fechaInicio.ToString(ConstantesDpo.FormatoFecha),
                                                                                  fechaFin.ToString(ConstantesDpo.FormatoFecha));
            //Consulta de data historica para el SIRPIT
            string cargas = string.Join(", ", tipo.Select(x => $"'{x}'"));
            List<IioTabla04DTO> dataSICLI = FactorySic.GetIioLogImportacionRepository()
                                                            .ListSicliByDateRange(cargas,
                                                                                   fechaInicio.ToString(ConstantesDpo.FormatoFecha),
                                                                                   fechaFin.ToString(ConstantesDpo.FormatoFecha),
                                                                                   variable.ToString());//new List<DpoDatos96DTO>();
            foreach (var m in mes)
            {
                //Fechas para iterar
                DateTime regInicio = new DateTime(anio, Int32.Parse(m), 1);//PONER 1 PARA EL PRIMER DIA
                DateTime regFin = new DateTime(anio, Int32.Parse(m), DateTime.DaysInMonth(anio, Int32.Parse(m)));

                while (regInicio <= regFin)
                {
                    foreach (var s in tipo)
                    {
                        DpoMedicion96DTO entity = data.Where(x => x.Dpomedcodi == s && x.Dpomedfecha == regInicio).FirstOrDefault();
                        if (entity == null)
                        {
                            entity = new DpoMedicion96DTO();

                            for (int i = 1; i <= 96; i++)
                            {
                                DateTime itvFecha = regInicio.AddDays(-7);
                                List<decimal> factores = new List<decimal>();
                                //var dValor = entity.GetType().GetProperty("H" + i.ToString()).GetValue(entity, null);
                                while (itvFecha >= fechaInicio)
                                {
                                    //dato anterior del TNA
                                    decimal historicoTNA = dataPunto.Where(x => x.Medifecha == itvFecha &&
                                                                                      x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                          .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                          .FirstOrDefault();
                                    //dato anterior del SIRPIT
                                    decimal historicoSIRPIT = dataSICLI.Where(x => x.FechaMedicion == itvFecha &&
                                                                                         x.Ptomedicodi == s &&
                                                                                         x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                             .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                             .FirstOrDefault();

                                    if (historicoTNA != 0 && historicoSIRPIT != 0 && factores.Count < 4)
                                    {
                                        decimal f = historicoSIRPIT / historicoTNA;
                                        factores.Add(f);
                                    }
                                    itvFecha = itvFecha.AddDays(-7);
                                }
                                if (factores.Count == 0)
                                {
                                    entity.GetType().GetProperty("H" + i.ToString()).SetValue(entity, 0m);
                                }
                                else
                                {
                                    decimal promedio = factores.Average();
                                    decimal actualTNA = dataPunto.Where(x => x.Medifecha == regInicio &&
                                                                             x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                 .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                 .FirstOrDefault();
                                    entity.GetType().GetProperty("H" + i.ToString()).SetValue(entity, actualTNA * promedio);
                                }
                            }
                            entity.Dpomedcodi = s;
                            entity.Dpomedfecha = regInicio;
                            entity.Dpotmecodi = (variable == 1) ? ConstantesDpo.DpotmeSirpitActiva : ConstantesDpo.DpotmeSirpitReactiva;
                            entity.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                            result.Add(entity);
                        }
                        else
                        {
                            for (int i = 1; i <= 96; i++)
                            {
                                DateTime itvFecha = regInicio.AddDays(-7);
                                List<decimal> factores = new List<decimal>();
                                decimal dValor = (decimal)entity.GetType().GetProperty("H" + i.ToString()).GetValue(entity, null);
                                if (dValor == 0)
                                {
                                    while (itvFecha >= fechaInicio)
                                    {
                                        //dato anterior del TNA
                                        decimal historicoTNA = dataPunto.Where(x => x.Medifecha == itvFecha &&
                                                                                          x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                              .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                              .FirstOrDefault();
                                        //dato anterior del SIRPIT
                                        decimal historicoSIRPIT = dataSICLI.Where(x => x.FechaMedicion == itvFecha &&
                                                                                             x.Ptomedicodi == s &&
                                                                                             x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                                 .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                                 .FirstOrDefault();

                                        if (historicoTNA != 0 && historicoSIRPIT != 0 && factores.Count < 4)
                                        {
                                            decimal f = historicoSIRPIT / historicoTNA;
                                            factores.Add(f);
                                        }
                                        itvFecha = itvFecha.AddDays(-7);
                                    }
                                    if (factores.Count == 0)
                                    {
                                        entity.GetType().GetProperty("H" + i.ToString()).SetValue(entity, 0m);
                                    }
                                    else
                                    {
                                        decimal promedio = factores.Average();
                                        decimal actualTNA = dataPunto.Where(x => x.Medifecha == regInicio &&
                                                                                 x.GetType().GetProperty("H" + i.ToString()).GetValue(x, null) != null)
                                                                     .Select(y => (decimal)y.GetType().GetProperty("H" + i.ToString()).GetValue(y, null))
                                                                     .FirstOrDefault();
                                        entity.GetType().GetProperty("H" + i.ToString()).SetValue(entity, actualTNA * promedio);
                                    }
                                }
                            }
                            result.Add(entity);
                        }
                    }
                    regInicio = regInicio.AddDays(1);
                }
            }

            return result;
        }

        /// <summary>
        /// Permite grabar data corregida como una  nueva versión o actualizar una existente
        /// </summary>
        /// <param name="lista"> data </param>
        /// <param name="versionDestino">version donde se registra los nuevos datos </param>
        /// <param name="opcion">flag para saber si se selcciona nueva o version existente </param>\
        /// <param name="usuario">usuario para la creacion del registro </param>
        /// <returns></returns>
        public void SaveDatosCorregidos96(List<DpoMedicion96DTO> lista, string versionDestino, int opcion, string usuario)
        {
            int verCodigo = 0;
            //Si opcion es 2 se debe registrar
            if (opcion == 2)
            {
                //Primero se debe registrar la nueva version y recuperar el Id 
                PrnVersiongrpDTO entVersion = new PrnVersiongrpDTO()
                {
                    Vergrpnomb = versionDestino,
                    Vergrpareausuaria = ConstantesDpo.datosCorregidos
                };

                verCodigo = FactorySic.GetPrnVersiongrpReporsitory()
                                      .SaveGetId(entVersion);
            }
            //Si es 1 se debe eliminar los registros existentes en DPO_Medicion96
            //que ptertenezcan a las vesion pasada en versionDestino
            else
            {
                verCodigo = Int32.Parse(versionDestino);
                this.DeleteDpoMedicion96ByVersion(verCodigo);
            }
            //Segundo pasamos en Id de la version a la lista y grabamos.
            foreach (DpoMedicion96DTO item in lista)
            {
                item.Vergrpcodi = verCodigo;
                item.Dpomedusucreacion = usuario;
                item.Dpomedfeccreacion = DateTime.Now;
                this.SaveDpoMedicion96(item);
            }
        }

        /// <summary>
        /// Permite grabar data corregida como una  nueva versión o actualizar una existente
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="meses"></param>
        /// <param name="fuente"></param>
        /// <param name="variable"></param>
        /// <param name="carga"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> ListaConsultaDataBasePrefiltra(int anio, string meses, int fuente, int variable,
                                                                     List<string> carga, string usuario)
        {
            string tempData;
            List<DpoMedicion96DTO> lista = new List<DpoMedicion96DTO>();

            switch (fuente)
            {
                case ConstantesDpo.fuenteSIRPIT:
                    List<DpoDatos96DTO> dpoDatos = new List<DpoDatos96DTO>();
                    while (carga.Count > 0)
                    {
                        if (carga.Count > 1000)
                        {
                            tempData = string.Join(", ", carga.Take(1000).Select(x => $"'{x}'"));
                            dpoDatos.AddRange(this.ListaDatosSIRPIT(anio, meses, tempData, variable.ToString()));
                            carga.RemoveRange(0, 1000);
                        }
                        else
                        {
                            tempData = string.Join(", ", carga.Select(x => $"'{x}'"));
                            dpoDatos.AddRange(this.ListaDatosSIRPIT(anio, meses, tempData, variable.ToString()));
                            carga.RemoveRange(0, carga.Count);
                        }
                    }

                    foreach (DpoDatos96DTO d in dpoDatos)
                    {
                        DpoMedicion96DTO sirpit = new DpoMedicion96DTO();
                        sirpit.Dpomedcodi = d.Tnfbarcodi;
                        sirpit.Dpotmecodi = (variable == 1) ? ConstantesDpo.DpotmeSirpitActiva : ConstantesDpo.DpotmeSirpitReactiva;
                        sirpit.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                        sirpit.Dpomedfecha = d.Dpodatfecha;
                        for (int i = 1; i <= 96; i++)
                        {
                            var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                            sirpit.GetType().GetProperty("H" + i.ToString()).SetValue(sirpit, (decimal?)dValor);
                        }
                        sirpit.Dpomedusucreacion = usuario;
                        sirpit.Dpomedfeccreacion = DateTime.Now;
                        lista.Add(sirpit);
                    }
                    break;
                case ConstantesDpo.fuenteSICLI:
                    List<IioTabla04DTO> iioTabla04 = new List<IioTabla04DTO>();
                    while (carga.Count > 0)
                    {
                        if (carga.Count > 1000)
                        {
                            tempData = string.Join(", ", carga.Take(1000).Select(x => $"'{x}'"));
                            iioTabla04.AddRange(this.ListaDatosSICLI(anio, meses, tempData, variable.ToString()));
                            carga.RemoveRange(0, 1000);
                        }
                        else
                        {
                            tempData = string.Join(", ", carga.Select(x => $"'{x}'"));
                            iioTabla04.AddRange(this.ListaDatosSICLI(anio, meses, tempData, variable.ToString()));
                            carga.RemoveRange(0, carga.Count);
                        }
                    }
                    foreach (IioTabla04DTO d in iioTabla04)
                    {
                        DpoMedicion96DTO sicli = new DpoMedicion96DTO();
                        sicli.Dpomedcodi = d.Ptomedicodi;
                        sicli.Dpotmecodi = (variable == 1) ? ConstantesDpo.DpotmeSicliActiva : ConstantesDpo.DpotmeSicliReactiva;
                        sicli.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                        sicli.Dpomedfecha = d.FechaMedicion;
                        for (int i = 1; i <= 96; i++)
                        {
                            var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                            sicli.GetType().GetProperty("H" + i.ToString()).SetValue(sicli, (decimal)dValor);
                        }
                        sicli.Dpomedusucreacion = usuario;
                        sicli.Dpomedfeccreacion = DateTime.Now;
                        lista.Add(sicli);
                    }
                    break;
                case ConstantesDpo.fuenteTNA:
                    List<DpoDemandaScoDTO> demandaSCO = new List<DpoDemandaScoDTO>();
                    string tipos = (variable == 1) ? ConstantesDpo.OrigenActiva : ConstantesDpo.OrigenReactiva;
                    while (carga.Count > 0)
                    {
                        if (carga.Count > 1000)
                        {
                            tempData = string.Join(", ", carga.Take(1000).Select(x => $"'{x}'"));
                            demandaSCO.AddRange(this.ListaDatosTNA(anio, meses, tempData, tipos));
                            carga.RemoveRange(0, 1000);
                        }
                        else
                        {
                            tempData = string.Join(", ", carga.Select(x => $"'{x}'"));
                            demandaSCO.AddRange(this.ListaDatosTNA(anio, meses, tempData, tipos));
                            carga.RemoveRange(0, carga.Count);
                        }
                    }
                    foreach (DpoDemandaScoDTO d in demandaSCO)
                    {
                        DpoMedicion96DTO tna = new DpoMedicion96DTO();
                        tna.Dpomedcodi = d.Ptomedicodi;
                        tna.Dpotmecodi = (variable == 1) ? ConstantesDpo.DpotmeTnaActiva : ConstantesDpo.DpotmeTnaReactiva;
                        tna.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                        tna.Dpomedfecha = d.Medifecha;
                        for (int i = 1; i <= 96; i++)
                        {
                            var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                            tna.GetType().GetProperty("H" + i.ToString()).SetValue(tna, (decimal)dValor);
                        }
                        tna.Dpomedusucreacion = usuario;
                        tna.Dpomedfeccreacion = DateTime.Now;
                        lista.Add(tna);
                    }
                    break;
            }

            return lista;
        }

        /// <summary>
        /// Permite grabar data corregida como una  nueva versión o actualizar una existente
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="meses"></param>
        /// <param name="fuente"></param>
        /// <param name="variable"></param>
        /// <param name="carga"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> ListaConsultaDataVersionPrefiltra(int anio, string meses, int fuente, int variable,
                                                                     List<string> carga, int version)
        {
            string tempData;
            List<DpoMedicion96DTO> lista = new List<DpoMedicion96DTO>();

            switch (fuente)
            {
                case ConstantesDpo.fuenteSIRPIT:
                    int tipoSirpit = (variable == 1) ? ConstantesDpo.DpotmeSirpitActiva : ConstantesDpo.DpotmeSirpitReactiva;
                    //string cargaString = string.Join(", ", carga.Select(x => $"'{x}'"));
                    //List<DpoTransformadorDTO> trafos = this.ListTransformadorByListExcel(cargaString);
                    //List<string> nCarga = trafos.Select(x => x.Dpotnfcodi.ToString()).ToList();
                    while (carga.Count > 0)
                    {
                        if (carga.Count > 1000)
                        {
                            tempData = string.Join(", ", carga.Take(1000).Select(x => $"'{x}'"));
                            lista.AddRange(this.ListByFiltros(tempData, tipoSirpit, ConstantesDpo.DpotdtPunto, version, anio, meses));
                            carga.RemoveRange(0, 1000);
                        }
                        else
                        {
                            tempData = string.Join(", ", carga.Select(x => $"'{x}'"));
                            lista.AddRange(this.ListByFiltros(tempData, tipoSirpit, ConstantesDpo.DpotdtPunto, version, anio, meses));
                            carga.RemoveRange(0, carga.Count);
                        }
                    }

                    break;
                case ConstantesDpo.fuenteSICLI:
                    int tipoSicli = (variable == 1) ? ConstantesDpo.DpotmeSicliActiva : ConstantesDpo.DpotmeSicliReactiva;
                    while (carga.Count > 0)
                    {
                        if (carga.Count > 1000)
                        {
                            tempData = string.Join(", ", carga.Take(1000).Select(x => $"'{x}'"));
                            lista.AddRange(this.ListByFiltros(tempData, tipoSicli, ConstantesDpo.DpotdtPunto, version, anio, meses));
                            carga.RemoveRange(0, 1000);
                        }
                        else
                        {
                            tempData = string.Join(", ", carga.Select(x => $"'{x}'"));
                            lista.AddRange(this.ListByFiltros(tempData, tipoSicli, ConstantesDpo.DpotdtPunto, version, anio, meses));
                            carga.RemoveRange(0, carga.Count);
                        }
                    }

                    break;
                case ConstantesDpo.fuenteTNA:
                    int tipoTna = (variable == 1) ? ConstantesDpo.DpotmeTnaActiva : ConstantesDpo.DpotmeTnaReactiva;
                    while (carga.Count > 0)
                    {
                        if (carga.Count > 1000)
                        {
                            tempData = string.Join(", ", carga.Take(1000).Select(x => $"'{x}'"));
                            lista.AddRange(this.ListByFiltros(tempData, tipoTna, ConstantesDpo.DpotdtPunto, version, anio, meses));
                            carga.RemoveRange(0, 1000);
                        }
                        else
                        {
                            tempData = string.Join(", ", carga.Select(x => $"'{x}'"));
                            lista.AddRange(this.ListByFiltros(tempData, tipoTna, ConstantesDpo.DpotdtPunto, version, anio, meses));
                            carga.RemoveRange(0, carga.Count);
                        }
                    }

                    break;
            }

            return lista;
        }

        /// <summary>
        /// Elimina un registro de la tabla DPO_RELACION_PTOBARRA
        /// </summary>
        /// <param name="codigo">Identificador del registro a eliminar</param>
        /// <returns></returns>
        public void DeleteDpoRelacionPtoBarra(int codigo)
        {
            try
            {
                FactorySic.GetDpoRelacionPtoBarraRepository().Delete(codigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista las barras con su punto tna relacionadas a una version.
        /// </summary>
        /// <param name="version">version qe se consulta</param>
        /// <returns></returns>
        public List<DpoRelacionPtoBarraDTO> ListaBarraPuntoVersion(int version)
        {
            return FactorySic.GetDpoRelacionPtoBarraRepository().ListBarraPuntoVersion(version);
        }

        /// <summary>
        /// Lista los H del punto TNA relacionado a una barra SPL.
        /// </summary>
        /// <param name="splfrmcodi">identificador de la relacio entre la barra y punto</param>
        /// <param name="inicio">fecha de inicio</param>
        /// <param name="fin">fecha de fin</param>
        /// <returns></returns>
        public List<DpoRelacionPtoBarraDTO> ListaPuntoRelacionBarra(int splfrmcodi, string inicio, string fin)
        {
            return FactorySic.GetDpoRelacionPtoBarraRepository().ListPuntoRelacionBarra(splfrmcodi, inicio, fin);
        }

        /// <summary>
        /// Inserta un registro de la tabla Dpo_Relacion_PtoBarra
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoRelacionPtoBarra(DpoRelacionPtoBarraDTO entity)
        {
            try
            {
                FactorySic.GetDpoRelacionPtoBarraRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla Dpo_Relacion_PtoBarra
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void UpdateDpoRelacionPtoBarra(DpoRelacionPtoBarraDTO entity)
        {
            try
            {
                FactorySic.GetDpoRelacionPtoBarraRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo para actualizar o registrar una relacion en tabla Dpo_Relacion_PtoBarra
        /// </summary>
        /// <param name="version">identificador de la version creada en la tabla DPO_REL_SPL_FORMULA</param>
        /// <param name="punto">identificador del punto de la tabla Me_PtoMedicion</param>
        /// <param name="usuario">usuario que realiza la insercion o actualizacion</param>
        /// <param name="codigo">identificador d la tabla DPO_RELACIONPTOBARRA</param>
        /// <returns></returns>
        public void RegistrarRelacionPtoBarra(int version, int punto, int codigo, string usuario)
        {
            DpoRelacionPtoBarraDTO entity = new DpoRelacionPtoBarraDTO();

            if (codigo == -1)
            {
                entity = new DpoRelacionPtoBarraDTO()
                {
                    Splfrmcodi = version,
                    Ptomedicodi = punto,
                    Ptobarusucreacion = usuario,
                    Ptobarfeccreacion = DateTime.Now
                };
                this.SaveDpoRelacionPtoBarra(entity);
            }
            else {
                entity = new DpoRelacionPtoBarraDTO()
                {
                    Ptobarcodi = codigo,
                    Ptomedicodi = punto,
                    Ptobarusumodificacion = usuario,
                    Ptobarfecmodificacion = DateTime.Now
                };
                this.UpdateDpoRelacionPtoBarra(entity);
            }
        }

        /// Permite consultar la data a mostrar en las graficas
        /// </summary>
        /// <param name="anio">Anio del periodo</param>
        /// <param name="mes">Mes del periodo</param>
        /// <param name="version">version de la consulta</param>
        /// <param name="fuente">fuente que se debe consultar, puede ser TNA, SICLI, SIRPIT o Formulas</param>
        /// <param name="variable">Tipo de datos, energia activa o reactiva</param>
        /// <param name="formula">Identificador de la forrmulas SPL</param>
        /// <param name="carga">Identificador de la carga</param>
        /// <returns></returns>
        public List<DpoConsultaPrefiltrado> ConsultarDataFiltrada(string anio, string[] mes, int version, int fuente, int variable,
                                                                  string formula, List<string> carga)
        {
            string tempData;
            string meses = string.Join(", ", mes.Select(x => $"'{x}'"));
            int parseAnio = Int32.Parse(anio);
            List<DpoConsultaPrefiltrado> data = new List<DpoConsultaPrefiltrado>();
            switch (fuente)
            {
                case ConstantesDpo.fuenteSIRPIT:
                    List<DpoDatos96DTO> dpoDatos = new List<DpoDatos96DTO>();
                    while (carga.Count > 0)
                    {
                        if (carga.Count > 1000)
                        {
                            tempData = string.Join(", ", carga.Take(1000).Select(x => $"'{x}'"));
                            dpoDatos.AddRange(this.ListaDatosAgrupadaPorFecha(anio, meses, tempData, variable.ToString()));
                            carga.RemoveRange(0, 1000);
                        }
                        else
                        {
                            tempData = string.Join(", ", carga.Select(x => $"'{x}'"));
                            dpoDatos.AddRange(this.ListaDatosAgrupadaPorFecha(anio, meses, tempData, variable.ToString()));
                            carga.RemoveRange(0, carga.Count);
                        }
                    }
                    var resultSIRPIT = dpoDatos
                                        .GroupBy(x => x.Dpodatfecha)
                                        .Select(y =>
                                        {
                                            var dto = new DpoDatos96DTO { Dpodatfecha = y.Key };
                                            for (int i = 1; i <= 96; i++)
                                            {
                                                string propertyName = "H" + i;
                                                decimal? sum = y.Sum(x => (decimal?)x.GetType().GetProperty(propertyName)?.GetValue(x));
                                                dto.GetType().GetProperty(propertyName)?.SetValue(dto, sum);
                                            }
                                            return dto;
                                        })
                                        .ToList();
                    foreach (var item in mes)
                    {
                        int parseMes = Int32.Parse(item);
                        DateTime lastDay = new DateTime(parseAnio, parseMes,
                                                        DateTime.DaysInMonth(parseAnio, parseMes));
                        DateTime firstDay = new DateTime(parseAnio, parseMes, 1);
                        List<decimal> dataMes = new List<decimal>();
                        List<string> fechas = new List<string>();
                        while (firstDay <= lastDay)
                        {
                            fechas.AddRange(this.ListaFechaHora96(firstDay));
                            var temporal = resultSIRPIT.Where(x => x.Dpodatfecha == firstDay).FirstOrDefault();
                            if (temporal != null)
                            {
                                dataMes.AddRange(UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv15min, temporal).ToList());

                            }
                            else
                            {
                                dataMes.AddRange(Enumerable.Repeat(0m, ConstantesProdem.Itv15min).ToArray().ToList());
                            }
                            firstDay = firstDay.AddDays(1);
                        }
                        DpoConsultaPrefiltrado entitySIRPIT = new DpoConsultaPrefiltrado()
                        {
                            id = parseMes,
                            dias = DateTime.DaysInMonth(parseAnio, parseMes),
                            diasHora = fechas,
                            name = DateTimeFormatInfo.CurrentInfo.GetMonthName(parseMes),
                            data = dataMes.ToArray(),
                        };
                        data.Add(entitySIRPIT);
                    }
                    break;
                case ConstantesDpo.fuenteSICLI:
                    List<IioTabla04DTO> iioTabla04 = new List<IioTabla04DTO>();
                    while (carga.Count > 0)
                    {
                        if (carga.Count > 1000)
                        {
                            tempData = string.Join(", ", carga.Take(1000).Select(x => $"'{x}'"));
                            iioTabla04.AddRange(this.ListaTabla04AgrupadaPorFecha(anio, meses, tempData, variable.ToString()));
                            carga.RemoveRange(0, 1000);
                        }
                        else
                        {
                            tempData = string.Join(", ", carga.Select(x => $"'{x}'"));
                            iioTabla04.AddRange(this.ListaTabla04AgrupadaPorFecha(anio, meses, tempData, variable.ToString()));
                            carga.RemoveRange(0, carga.Count);
                        }
                    }
                    var resultSICLI = iioTabla04
                                    .GroupBy(x => x.FechaMedicion)
                                    .Select(y =>
                                    {
                                        var dto = new IioTabla04DTO { FechaMedicion = y.Key };
                                        for (int i = 1; i <= 96; i++)
                                        {
                                            string propertyName = "H" + i;
                                            decimal? sum = y.Sum(x => (decimal?)x.GetType().GetProperty(propertyName)?.GetValue(x));
                                            dto.GetType().GetProperty(propertyName)?.SetValue(dto, sum);
                                        }
                                        return dto;
                                    })
                                    .ToList();
                    foreach (var item in mes)
                    {
                        int parseMes = Int32.Parse(item);
                        DateTime lastDay = new DateTime(parseAnio, parseMes,
                                                        DateTime.DaysInMonth(parseAnio, parseMes));
                        DateTime firstDay = new DateTime(parseAnio, parseMes, 1);
                        List<decimal> dataMes = new List<decimal>();
                        List<string> fechas = new List<string>();
                        while (firstDay <= lastDay)
                        {
                            fechas.AddRange(this.ListaFechaHora96(firstDay));
                            var temporal = resultSICLI.Where(x => x.FechaMedicion == firstDay).FirstOrDefault();
                            if (temporal != null)
                            {
                                dataMes.AddRange(UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv15min, temporal).ToList());

                            }
                            else
                            {
                                dataMes.AddRange(Enumerable.Repeat(0m, ConstantesProdem.Itv15min).ToArray().ToList());
                            }
                            firstDay = firstDay.AddDays(1);
                        }
                        DpoConsultaPrefiltrado entitySICLI = new DpoConsultaPrefiltrado()
                        {
                            id = parseMes,
                            dias = DateTime.DaysInMonth(parseAnio, parseMes),
                            diasHora = fechas,
                            name = DateTimeFormatInfo.CurrentInfo.GetMonthName(parseMes),
                            data = dataMes.ToArray(),
                        };
                        data.Add(entitySICLI);
                    }
                    break;
                case ConstantesDpo.fuenteTNA:
                    List<DpoDemandaScoDTO> demandaSCO = new List<DpoDemandaScoDTO>();
                    string tipos = (variable == 1) ? ConstantesDpo.OrigenActiva : ConstantesDpo.OrigenReactiva;
                    while (carga.Count > 0)
                    {
                        if (carga.Count > 1000)
                        {
                            tempData = string.Join(", ", carga.Take(1000).Select(x => $"'{x}'"));
                            demandaSCO.AddRange(this.ListaDemandaAgrupadaPorFecha(anio, meses, tempData, tipos));
                            carga.RemoveRange(0, 1000);
                        }
                        else
                        {
                            tempData = string.Join(", ", carga.Select(x => $"'{x}'"));
                            demandaSCO.AddRange(this.ListaDemandaAgrupadaPorFecha(anio, meses, tempData, tipos));
                            carga.RemoveRange(0, carga.Count);
                        }
                    }
                    var resultTNA = demandaSCO
                                    .GroupBy(x => x.Medifecha)
                                    .Select(y =>
                                    {
                                        var dto = new DpoDemandaScoDTO { Medifecha = y.Key };
                                        for (int i = 1; i <= 96; i++)
                                        {
                                            string propertyName = "H" + i;
                                            decimal? sum = y.Sum(x => (decimal?)x.GetType().GetProperty(propertyName)?.GetValue(x));
                                            dto.GetType().GetProperty(propertyName)?.SetValue(dto, sum);
                                        }
                                        return dto;
                                    })
                                    .ToList();

                    foreach (var item in mes)
                    {
                        int parseMes = Int32.Parse(item);
                        DateTime lastDay = new DateTime(parseAnio, parseMes,
                                                        DateTime.DaysInMonth(parseAnio, parseMes));
                        DateTime firstDay = new DateTime(parseAnio, parseMes, 1);
                        List<decimal> dataMes = new List<decimal>();
                        List<string> fechas = new List<string>();
                        while (firstDay <= lastDay)
                        {
                            fechas.AddRange(this.ListaFechaHora96(firstDay));
                            var temporal = resultTNA.Where(x => x.Medifecha == firstDay).FirstOrDefault();
                            if (temporal != null)
                            {
                                dataMes.AddRange(UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv15min, temporal).ToList());

                            }
                            else
                            {
                                dataMes.AddRange(Enumerable.Repeat(0m, ConstantesProdem.Itv15min).ToArray().ToList());
                            }
                            firstDay = firstDay.AddDays(1);
                        }
                        DpoConsultaPrefiltrado entityTNA = new DpoConsultaPrefiltrado()
                        {
                            id = parseMes,
                            dias = DateTime.DaysInMonth(parseAnio, parseMes),
                            diasHora = fechas,
                            name = DateTimeFormatInfo.CurrentInfo.GetMonthName(parseMes),
                            data = dataMes.ToArray(),
                        };
                        data.Add(entityTNA);
                    }
                    break;
                case ConstantesDpo.fuenteFormulaSPL:
                    break;
            }
            return data;
        }


        /// Permite consultar la data a mostrar en las graficas de versiones con data registrada en DPO_MEDICION96
        /// </summary>
        /// <param name="anio">Anio del periodo</param>
        /// <param name="mes">Mes del periodo</param>
        /// <param name="version">version de la consulta</param>
        /// <param name="fuente">fuente que se debe consultar, puede ser TNA, SICLI, SIRPIT o Formulas</param>
        /// <param name="variable">Tipo de datos, energia activa o reactiva</param>
        /// <param name="formula">Identificador de la forrmulas SPL</param>
        /// <param name="carga">Identificador de la carga</param>
        /// <returns></returns>
        public List<DpoConsultaPrefiltrado> ConsultarDataVersionFiltrada(string anio, string[] mes, int version, int fuente, int variable,
                                                                  string formula, List<string> carga)
        {
            string meses = string.Join(", ", mes.Select(x => $"'{x}'"));
            int parseAnio = Int32.Parse(anio);
            List<DpoConsultaPrefiltrado> data = new List<DpoConsultaPrefiltrado>();

            List<DpoMedicion96DTO> lista = this.ListaConsultaDataVersionPrefiltra(parseAnio, meses, fuente, variable,
                                                                                    carga, version);
            var result = lista
                                .GroupBy(x => x.Dpomedfecha)
                                .Select(y =>
                                {
                                    var dto = new DpoMedicion96DTO { Dpomedfecha = y.Key };
                                    for (int i = 1; i <= 96; i++)
                                    {
                                        string propertyName = "H" + i;
                                        decimal? sum = y.Sum(x => (decimal?)x.GetType().GetProperty(propertyName)?.GetValue(x));
                                        dto.GetType().GetProperty(propertyName)?.SetValue(dto, sum);
                                    }
                                    return dto;
                                })
                                .ToList();
            foreach (var item in mes)
            {
                int parseMes = Int32.Parse(item);
                DateTime lastDay = new DateTime(parseAnio, parseMes,
                                                DateTime.DaysInMonth(parseAnio, parseMes));
                DateTime firstDay = new DateTime(parseAnio, parseMes, 1);
                List<decimal> dataMes = new List<decimal>();
                List<string> fechas = new List<string>();
                while (firstDay <= lastDay)
                {
                    fechas.AddRange(this.ListaFechaHora96(firstDay));
                    var temporal = result.Where(x => x.Dpomedfecha == firstDay).FirstOrDefault();
                    if (temporal != null)
                    {
                        dataMes.AddRange(UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv15min, temporal).ToList());

                    }
                    else
                    {
                        dataMes.AddRange(Enumerable.Repeat(0m, ConstantesProdem.Itv15min).ToArray().ToList());
                    }
                    firstDay = firstDay.AddDays(1);
                }
                DpoConsultaPrefiltrado entity = new DpoConsultaPrefiltrado()
                {
                    id = parseMes,
                    dias = DateTime.DaysInMonth(parseAnio, parseMes),
                    diasHora = fechas,
                    name = DateTimeFormatInfo.CurrentInfo.GetMonthName(parseMes),
                    data = dataMes.ToArray(),
                };
                data.Add(entity);
            }

            return data;
        }

        /// <summary>
        /// Lista un dia concatenado con las horas
        /// </summary>
        /// <param name="dia">dia al que se le concatena las hora</param>
        /// <returns></returns>
        public List<string> ListaFechaHora96(DateTime dia)
        {
            List<string> fechas = Enumerable.Repeat(dia.ToString(ConstantesDpo.FormatoFecha), ConstantesProdem.Itv15min).ToList();
            string[] array = UtilProdem.GenerarIntervalos(ConstantesProdem.Itv15min);

            for (int i = 0; i < fechas.Count; i++)
            {
                fechas[i] = fechas[i] + " " + array[i];
            }

            return fechas;
        }

        /// <summary>
        /// Lista las empresas activas por tipo
        /// </summary>
        /// <param name="id">identificador de la tabla dpo_ transformador</param>
        /// <returns></returns>
        public DpoTransformadorDTO GetByIdTransformador(int id)
        {
            return FactorySic.GetDpoTransformadorRepository().GetById(id);
        }

        /// <summary>
        /// Lista los datos de una barra relacionada a la tabla DPO_REL_SPL_FORMULA
        /// </summary>
        /// <param name="codi">identificador de la tabla DPO_REL_SPL_FORMULA</param>
        /// <returns></returns>
        public DpoRelacionPtoBarraDTO GetPuntoById(int codi)
        {
            return FactorySic.GetDpoRelacionPtoBarraRepository().GetPuntoById(codi);
        }

        /// <summary>
        /// Arma el archivo .CSV para medidor de demanda
        /// </summary>
        /// <param name="cabecera">Tipo de la empresa</param>
        /// <param name="datos">Tipo de la empresa</param>
        /// <param name="pathFile">Tipo de la empresa</param>
        /// <param name="filename">Tipo de la empresa</param>
        /// <returns></returns>
        public string ExportarMedidorDemandaCSV(string[] cabecera, List<DpoGrillaDatos96> datos, string pathFile, string filename)
        {
            string Reporte = filename + ".csv";
            try
            {
                // Crear el archivo CSV
                using (StreamWriter writer = new StreamWriter(pathFile + Reporte))
                {
                    // Escribir los encabezados de las columnas
                    foreach (var c in cabecera)
                    {
                        writer.Write(c);
                        writer.Write(",");
                    }
                    writer.WriteLine();

                    // Escribir los datos de las filas
                    foreach (DpoGrillaDatos96 reg in datos)
                    {
                        writer.Write(reg.Fecha.ToString());
                        writer.Write(",");
                        writer.Write(reg.NombrePunto.ToString());
                        writer.Write(",");
                        writer.Write(reg.NombreTransformador.ToString());
                        writer.Write(",");
                        writer.Write(reg.NombreBarra.ToString());
                        writer.Write(",");
                        writer.Write(reg.Total.ToString());
                        writer.Write(",");
                        for (int i = 1; i <= 96; i++)
                        {
                            var dValor = reg.GetType().GetProperty("H" + (i).ToString()).GetValue(reg, null);
                            //grilla.GetType().GetProperty("H" + i.ToString()).SetValue(grilla, (decimal)dValor);
                            writer.Write(dValor.ToString());
                            writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return Reporte;
        }

        /// <summary>
        /// Arma el archivo .CSV para medidor de demanda
        /// </summary>
        /// <param name="datos">Tipo de la empresa</param>
        /// <param name="pathFile">Tipo de la empresa</param>
        /// <param name="filename">Tipo de la empresa</param>
        /// <param name="version">Tipo de la empresa</param>
        /// <param name="variable">Tipo de la empresa</param>
        /// <param name="formula">Tipo de la empresa</param>
        /// <returns></returns>
        public string ExportarFormulasCSV(List<DpoFormulaCorregidaFormmato> datos, string pathFile, string filename, 
                                            string version, string variable, string formula)
        {
            string Reporte = filename + ".csv";
            List<string> cabecera = new List<string> { "Fechas DD/MM/YYYY", "Formula", "Suma Cargas"}; 
            try
            {
                List<DpoFormulaCorregidaFormmato> dataFormulas = datos.Where(x => x.Serie == "Formula").ToList();
                List<DpoFormulaCorregidaFormmato> dataMeses = datos.Where(x => x.Serie != "Formula").ToList();
                List<decimal> meses = new List<decimal>();
                List<string> fechas = new List<string>();
                foreach (var dm in dataMeses)
                {
                    meses.AddRange(dm.Valores);
                    fechas.AddRange(dm.Fechas);
                }

                List<decimal> formulas = new List<decimal>();
                foreach (var df in dataFormulas)
                {
                    formulas.AddRange(df.Valores);
                }

                // Crear el archivo CSV
                using (StreamWriter writer = new StreamWriter(pathFile + Reporte))
                {
                    //Data
                    writer.Write("Version: " + version);
                    writer.WriteLine();

                    writer.Write("Formula: " + formula);
                    writer.WriteLine();

                    writer.Write("Variable: " + variable);
                    writer.WriteLine();

                    // Escribir los encabezados de las columnas
                    foreach (var c in cabecera)
                    {
                        writer.Write(c);
                        writer.Write(",");
                    }
                    writer.WriteLine();

                    //Filas
                    for (int i = 0; i < meses.Count; i++)
                    {
                        writer.Write(fechas[i]);
                        writer.Write(",");
                        writer.Write(formulas[i].ToString());
                        writer.Write(",");
                        writer.Write(meses[i].ToString());
                      
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return Reporte;
        }

        /// <summary>
        /// Lista las empresas activas por tipo
        /// </summary>
        /// <param name="tipo">Tipo de la empresa</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmpresasByTipo(string tipo)
        {
            return FactorySic.GetSiEmpresaRepository().ListaEmpresasByTipo(tipo);
        }

        /// <summary>
        /// Lista los puntos activos por equipo
        /// </summary>
        /// <param name="equipo">id del equipo</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListaPuntoByEquipo(int equipo)
        {
            return FactorySic.GetMePtomedicionRepository().ListaPuntoByEquipo(equipo);
        }

        /// <summary>
        /// Lista los puntos activos por equipo de lectura y empresa
        /// </summary>
        /// <param name="origen">id del origen de lectura</param>
        /// <param name="empresa">id de la empresa</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListaPuntoByOrigenEmpresa(int origen, int empresa)
        {
            return FactorySic.GetMePtomedicionRepository().ListaPuntoByOrigenEmpresa(origen, empresa);
        }

        /// <summary>
        /// Lista los puntos siclis por equipo de lectura y empresa
        /// </summary>
        /// <param name="origen">id del origen de lectura</param>
        /// <param name="empresa">id de la empresa</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListaPuntoSicliByEmpresa(int origen, int empresa)
        {
            return FactorySic.GetMePtomedicionRepository().ListaPuntoSicliByEmpresa(origen, empresa);
        }

        //
        /// <summary>
        /// Lista los puntos de medicion en base a una lista de puntos
        /// </summary>
        /// <param name="puntos">id del origen de lectura</param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListaPuntoMedicionByLista(string puntos)
        {
            return FactorySic.GetMePtomedicionRepository().ListaPuntoMedicionByLista(puntos);
        }

        /// <summary>
        /// Lista los equipos activos por empresa
        /// </summary>
        /// <param name="empresa">id de la empresa</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListaEquipoByEmpresa(int empresa)
        {
            return FactorySic.GetEqEquipoRepository().ListaEquipoByEmpresa(empresa);
        }

        /// <summary>
        /// Permite obtener los registro de la tabla PRN_VERSIONGRP por area
        /// </summary>
        public List<PrnVersiongrpDTO> ListVersionByArea(string area)
        {
            return FactorySic.GetPrnVersiongrpReporsitory().ListVersionByArea(area);
        }

        /// <summary>
        /// Lista los datos de la tabal DemandaSCO sumando los H agrupadas por fecha
        /// </summary>
        /// <returns></returns>
        public List<DpoDemandaScoDTO> ListaDemandaAgrupadaPorFecha(string anio, string mes, string cargas, string tipo)
        {
            return FactorySic.GetDpoDemandaScoRepository().ListGroupByMonthYear(anio, mes, cargas, tipo);
        }

        /// <summary>
        /// Lista los datos para el TNA para una lista de fechas y cargas
        /// </summary>
        /// <returns></returns>
        public List<DpoDemandaScoDTO> ListaDatosTNA(int anio, string mes, string cargas, string tipo)
        {
            return FactorySic.GetDpoDemandaScoRepository().ListDatosTNA(anio, mes, cargas, tipo);
        }

        /// <summary>
        /// Lista los datos de la tabla Datos96 sumando los H agrupadas por fecha
        /// </summary>
        /// <returns></returns>
        public List<DpoDatos96DTO> ListaDatosAgrupadaPorFecha(string anio, string mes, string cargas, string tipo)
        {
            return FactorySic.GetDpoDatos96Repository().ListGroupByMonthYear(anio, mes, cargas, tipo);
        }

        /// <summary>
        /// Lista los datos de la tabla Datos96 para una lista de meses y cargas
        /// </summary>
        /// <returns></returns>
        public List<DpoDatos96DTO> ListaDatosSIRPIT(int anio, string mes, string cargas, string tipo)
        {
            return FactorySic.GetDpoDatos96Repository().ListDatosSIRPIT(anio, mes, cargas, tipo);
        }

        /// <summary>
        /// Lista los datos de la tabla Datos96 en un intervalo de tiempo
        /// </summary>
        /// <returns></returns>
        public List<DpoDatos96DTO> ListAllBetweenDates(string fechainicio, string fechafin, int subestacion, int transformador, int barra)
        {
            return FactorySic.GetDpoDatos96Repository().ListAllBetweenDates(fechainicio, fechafin, subestacion, transformador, barra);
        }

        /// <summary>
        /// Lista los datos para el SICLI sumando los H agrupadas por fecha
        /// </summary>
        /// <returns></returns>
        public List<IioTabla04DTO> ListaTabla04AgrupadaPorFecha(string anio, string mes, string cargas, string tipo)
        {
            return FactorySic.GetIioLogImportacionRepository().ListGroupByMonthYear(anio, mes, cargas, tipo);
        }

        /// <summary>
        /// Lista los datos para el SICLI para una lista de meses y cargas
        /// </summary>
        /// <returns></returns>
        public List<IioTabla04DTO> ListaDatosSICLI(int anio, string mes, string cargas, string tipo)
        {
            return FactorySic.GetIioLogImportacionRepository().ListDatosSICLI(anio, mes, cargas, tipo);
        }

        /// <summary>
        /// Lista los datos de la tabal DemandaSCO sumando los H agrupadas por fecha, punto y tipo
        /// </summary>
        /// <returns></returns>
        public List<DpoDemandaScoDTO> ListMedidorDemandaTna(string punto, string inicio, string fin, string tipo)
        {
            return FactorySic.GetDpoDemandaScoRepository().ListMedidorDemandaTna(punto, inicio, fin, tipo);
        }

        /// <summary>
        /// Lista los datos de la tabal Me_Medicion96
        /// </summary>
        /// <returns></returns>
        public List<IioTabla04DTO> ListMedidorDemandaSicli(string carga, string inicio, string fin, int tipo)
        {
            return FactorySic.GetIioLogImportacionRepository().ListMedidorDemandaSicli(carga, inicio, fin, tipo);
        }

        //List<DpoDemandaScoDTO> ListMedidorDemandaSirpit(string carga, string inicio, string fin, int tipo);
        /// <summary>
        /// Lista los datos de la tabal DemandaSCO sumando los H agrupadas por fecha, punto y tipo
        /// </summary>
        /// <returns></returns>
        public List<DpoDatos96DTO> ListMedidorDemandaSirpit(string carga, string inicio, string fin, int tipo)
        {
            return FactorySic.GetDpoDatos96Repository().ListMedidorDemandaSirpit(carga, inicio, fin, tipo);
        }

        /// <summary>
        /// Lista las subestaciones dpo
        /// </summary>
        /// <returns></returns>
        public List<DpoSubestacionDTO> ListaSubestacionesDPO()
        {
            return FactorySic.GetDpoSubestacionRepository().List();
        }

        /// <summary>
        /// Inserta un registro de la tabla DPO_SUBESTACION
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoSubestacion(DpoSubestacionDTO entity)
        {
            try
            {
                FactorySic.GetDpoSubestacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de la tabla DPO_SUBESTACION
        /// </summary>
        /// <param name="codigo">Identificador del registro a eliminar, se puede usar '0' para todos</param>
        /// <returns></returns>
        public void DeleteDpoSubestacion(string codigo)
        {
            try
            {
                FactorySic.GetDpoSubestacionRepository().Delete(codigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista las barras dpo
        /// </summary>
        /// <returns></returns>
        public List<DpoBarraDTO> ListaBarrasDPO()
        {
            return FactorySic.GetDpoBarraRepository().List();
        }

        /// <summary>
        /// Inserta un registro de la tabla DPO_BARRA
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoBarra(DpoBarraDTO entity)
        {
            try
            {
                FactorySic.GetDpoBarraRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de la tabla DPO_BARRA
        /// </summary>
        /// <param name="codigo">Identificador del registro a eliminar, se puede usar '0' para todos</param>
        /// <returns></returns>
        public void DeleteDpoBarra(string codigo)
        {
            try
            {
                FactorySic.GetDpoBarraRepository().Delete(codigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista los transformadores dpo
        /// </summary>
        /// <returns></returns>
        public List<DpoTransformadorDTO> ListaTransformadoresDPO()
        {
            return FactorySic.GetDpoTransformadorRepository().List();
        }

        /// <summary>
        /// Lista los transformadores dpo que pertenecen a una subestacion 
        /// </summary>
        /// <param name="codigo">Identificador de la subestacion</param>
        /// <returns></returns>
        public List<DpoTransformadorDTO> ListaTransformadorBySubestacion(int codigo)
        {
            return FactorySic.GetDpoTransformadorRepository().ListTransformadorBySubestacion(codigo);
        }


        /// <summary>
        /// Lista los transformadores dpo en base a una lista de id
        /// </summary>
        /// <param name="codigo">codigos del transformador</param>
        /// <returns></returns>
        public List<DpoTransformadorDTO> ListTransformadorByList(string codigo)
        {
            return FactorySic.GetDpoTransformadorRepository().ListTransformadorByList(codigo);
        }

        /// <summary>
        /// Lista los transformadores dpo en base a una lista de id excel
        /// </summary>
        /// <param name="codigo">codigos del transformador</param>
        /// <returns></returns>
        public List<DpoTransformadorDTO> ListTransformadorByListExcel(string codigo)
        {
            return FactorySic.GetDpoTransformadorRepository().ListTransformadorByListExcel(codigo);
        }

        /// <summary>
        /// Inserta un registro de la tabla DPO_TRANSFORMADOR
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoTransformador(DpoTransformadorDTO entity)
        {
            try
            {
                FactorySic.GetDpoTransformadorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de la tabla DPO_TRANSFORMADOR
        /// </summary>
        /// <param name="codigo">Identificador del registro a eliminar, se puede usar '0' para todos</param>
        /// <returns></returns>
        public void DeleteDpoTransformador(string codigo)
        {
            try
            {
                FactorySic.GetDpoTransformadorRepository().Delete(codigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Procesa el archivo excel importado
        /// </summary>
        /// <param name="rutaArchivo">Dirección del archivo importado</param>
        /// <param name="regFecha">Fecha de referencia para la ejecución</param>
        /// <param name="user">Usuario que realiza el proceso</param>
        /// <returns></returns>
        public object MaestrasDPOProcesar(string rutaArchivo, string regFecha, string user)
        {
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;
            List<string> detailMsg = new List<string>();//String.Empty;

            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);

            FileInfo fileInfo = new FileInfo(rutaArchivo);
            ExcelPackage package = new ExcelPackage(fileInfo);

            //Primera hoja - Subestacion
            ExcelWorksheet hojaSubestacion = package.Workbook.Worksheets[1];
            List<DpoSubestacionDTO> listaSubestacion = new List<DpoSubestacionDTO>();
            DpoSubestacionDTO entitySubestacion;
            var start = hojaSubestacion.Dimension.Start;
            var end = hojaSubestacion.Dimension.End;
            //int sub = 1;
            for (int row = start.Row + 1; row <= end.Row; row++)
            {
                entitySubestacion = new DpoSubestacionDTO();
                //entitySubestacion.Dposubcodi = sub;
                entitySubestacion.Dposubcodiexcel = hojaSubestacion.Cells[row, 1].Text.Trim();
                entitySubestacion.Dposubnombre = hojaSubestacion.Cells[row, 2].Text.Trim();
                entitySubestacion.Dposubusucreacion = user;
                entitySubestacion.Dposubfeccreacion = parseFecha;
                listaSubestacion.Add(entitySubestacion);
                //sub++;
            }
            //Segunda hoja - Transformador
            ExcelWorksheet hojaTransformador = package.Workbook.Worksheets[2];
            List<DpoTransformadorDTO> listaTransformador = new List<DpoTransformadorDTO>();
            DpoTransformadorDTO entityTransformador;
            start = hojaTransformador.Dimension.Start;
            end = hojaTransformador.Dimension.End;
            //int tnf = 1;
            for (int row = start.Row + 1; row <= end.Row; row++)
            {
                entityTransformador = new DpoTransformadorDTO();
                //entityTransformador.Dpotnfcodi = tnf;
                entityTransformador.Dpotnfcodiexcel = hojaTransformador.Cells[row, 1].Text.Trim();
                entityTransformador.Dposubnombre = hojaTransformador.Cells[row, 2].Text.Trim();
                entityTransformador.Emprnomb = hojaTransformador.Cells[row, 3].Text.Trim();
                entityTransformador.Dpotnfusucreacion = user;
                entityTransformador.Dpotnffeccreacion = parseFecha;
                listaTransformador.Add(entityTransformador);
                //tnf++;
            }

            //Tercera hija - Barra
            ExcelWorksheet hojaBarra = package.Workbook.Worksheets[3];
            List<DpoBarraDTO> listaBarra = new List<DpoBarraDTO>();
            DpoBarraDTO entityBarra;
            start = hojaBarra.Dimension.Start;
            end = hojaBarra.Dimension.End;
            //int bar = 1;
            for (int row = start.Row + 1; row <= end.Row; row++)
            {
                entityBarra = new DpoBarraDTO();
                //entityBarra.Dpobarcodi = bar;
                entityBarra.Dpobarcodiexcel = hojaBarra.Cells[row, 1].Text.Trim();
                entityBarra.Dpobarnombre = hojaBarra.Cells[row, 2].Text.Trim();
                entityBarra.Dpobartension = PasarDecimalCero(hojaBarra.Cells[row, 3].Text);
                entityBarra.Dpobarusucreacion = user;
                entityBarra.Dpobarfeccreacion = parseFecha;
                listaBarra.Add(entityBarra);
                //bar++;
            }

            //Antiguas subestaciones, barras y transformadores registrados
            List<DpoSubestacionDTO> oldSubestaciones = this.ListaSubestacionesDPO();
            List<DpoBarraDTO> oldBarras = this.ListaBarrasDPO();
            List<DpoTransformadorDTO> oldTransformadores = this.ListaTransformadoresDPO();

            //Elimino posibles registros repetidos de la lista de transformadores
            listaTransformador = listaTransformador
                                .GroupBy(p => new { p.Dpotnfcodiexcel, p.Dposubnombre })
                                .Select(g => g.First())
                                .ToList();

            //Listas de nuevos equipos registrados 
            List<string> newSubestaciones = new List<string>();
            List<string> newBarras = new List<string>();
            List<string> newTransformadores = new List<string>();

            //Registrando Subestaciones
            //if (oldSubestaciones.Count() > 0) { this.DeleteDpoSubestacion("0"); }
            foreach (DpoSubestacionDTO s in listaSubestacion)
            {
                DpoSubestacionDTO subTemp = oldSubestaciones.Where(x => x.Dposubcodiexcel == s.Dposubcodiexcel).FirstOrDefault();
                if (subTemp == null)
                {
                    this.SaveDpoSubestacion(s);
                    newSubestaciones.Add(s.Dposubcodiexcel);
                }
            }
            //Registrando Barras
            //if (oldBarras.Count() > 0) { this.DeleteDpoBarra("0"); }
            foreach (DpoBarraDTO b in listaBarra)
            {
                DpoBarraDTO barTemp = oldBarras.Where(x => x.Dpobarcodiexcel == b.Dpobarcodiexcel).FirstOrDefault();
                if (barTemp == null)
                {
                    this.SaveDpoBarra(b);
                    newBarras.Add(b.Dpobarcodiexcel);
                }
                //this.SaveDpoBarra(b);
            }
            //Registrando Transformadores
            //if (oldTransformadores.Count() > 0) { this.DeleteDpoTransformador("0"); }
            foreach (DpoTransformadorDTO t in listaTransformador)
            {
                DpoTransformadorDTO tnfTemp = oldTransformadores.Where(x => x.Dpotnfcodiexcel == t.Dpotnfcodiexcel).FirstOrDefault();
                if (tnfTemp == null)
                {
                    this.SaveDpoTransformador(t);
                    newTransformadores.Add(t.Dpotnfcodiexcel);
                }
                //this.SaveDpoTransformador(t);
            }

            string strSubestaciones = (newSubestaciones.Count > 0) ? "Subestaciones :" + string.Join(" ", newSubestaciones) : "";
            string strBarras = (newBarras.Count > 0) ? "Barras :" + string.Join(" ", newBarras) : "";
            string strTransformadores = (newTransformadores.Count > 0) ? "Transformadores :" + string.Join(" ", newTransformadores) : "";

            typeMsg = ConstantesProdem.MsgSuccess;
            dataMsg = "El archivo de data maestra fue cargado de manera exitosa.";
            ////detailMsg = "Los nuevos registros son: " + Environment.NewLine +
            ////            strSubestaciones + Environment.NewLine + 
            ////            strBarras + Environment.NewLine + 
            ////            strTransformadores;
            if (!string.IsNullOrEmpty(strSubestaciones)) detailMsg.Add(strSubestaciones);
            if (!string.IsNullOrEmpty(strBarras)) detailMsg.Add(strBarras);
            if (!string.IsNullOrEmpty(strTransformadores)) detailMsg.Add(strTransformadores);

            return new { typeMsg, dataMsg, detailMsg };
        }

        /// <summary>
        /// Lista las formulas por tipo(usuario)
        /// </summary>
        /// <returns></returns>
        public void UpdateTransformadoresSirpit(string inicio, string fin)
        {
            FactorySic.GetDpoTransformadorRepository().UpdateTransformadoresSirpit(inicio, fin);
        }

        /// <summary>
        /// Procesa el archivo .txt
        /// </summary>
        /// <param name="rutaArchivo">Dirección del archivo importado</param>
        /// <param name="regFecha">Fecha de referencia para la ejecución</param>
        /// <param name="user">Usuario que realiza el proceso</param>
        /// <returns></returns>
        public object DatosDPO96(string rutaArchivo, string regFecha, string user)
        {
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;
            List<string> detailMsg = new List<string>();//String.Empty;

            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            List<string> arrayData = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(rutaArchivo))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null) arrayData.Add(line);
                }
                arrayData.RemoveRange(0, 1);

                List<string[]> cleanArrayData = new List<string[]>();

                for (int i = 0; i < arrayData.Count; i++)
                {
                    string[] arry = arrayData[i].Split(new char[] { '\t' });
                    cleanArrayData.Add(arry);
                }

                List<string[]> cleanArrayDataCompleta = new List<string[]>();
                #region Revisando si la data esta completa

                //Se toma la primera fecha y se la formatea a 00:00 y despues le smo 15min
                //La fecha fin se calcula sumando 4 meses al inicio en 00:00
                DateTime fechaInicio = DateTime.ParseExact(cleanArrayData[0][4], "yyyyMMddHHmm",
                                       CultureInfo.InvariantCulture);//cleanArrayData[0][4];
                fechaInicio = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day, 0, 0, 0);
                DateTime fechaFin = fechaInicio.AddMonths(4);
                fechaInicio = fechaInicio.AddMinutes(15);

                //DateTime fechaFin = DateTime.ParseExact(cleanArrayData[cleanArrayData.Count - 1][4], "yyyyMMddHHmm",
                //                                        CultureInfo.InvariantCulture);//cleanArrayData[cleanArrayData.Count - 1][4];
                 
                DateTime fechaSiguiente = fechaInicio;
                string subestacionSiguiente = string.Empty;
                string transformadorSiguiente = string.Empty;

                for (int i = 0; i < cleanArrayData.Count; i++)
                {
                    //Datos Actuales
                    string subestacion = cleanArrayData[i][0];
                    string transformador = cleanArrayData[i][1];
                    string serie = cleanArrayData[i][2];
                    string barra = cleanArrayData[i][3];
                    DateTime fechaHoy = DateTime.ParseExact(cleanArrayData[i][4], "yyyyMMddHHmm",
                                            CultureInfo.InvariantCulture);

                    if (fechaHoy == fechaInicio && fechaSiguiente == fechaInicio)
                    {
                        cleanArrayDataCompleta.Add(cleanArrayData[i]);
                    }
                    else 
                    {
                        while (fechaSiguiente < fechaHoy)
                        {
                            string[] faltante = new string[9];
                            faltante[0] = subestacion;
                            faltante[1] = transformador;
                            faltante[2] = serie;
                            faltante[3] = barra;
                            faltante[4] = fechaSiguiente.ToString("yyyyMMddHHmm");
                            faltante[5] = "0"; faltante[6] = "0"; faltante[7] = "0"; faltante[8] = "0";
                            cleanArrayDataCompleta.Add(faltante);
                            fechaSiguiente = fechaSiguiente.AddMinutes(15);
                            if (fechaSiguiente >= fechaFin)
                            {
                                break;
                            }
                        }
                        cleanArrayDataCompleta.Add(cleanArrayData[i]);
                    }

                    if (fechaSiguiente == fechaFin)
                    {
                        fechaSiguiente = fechaInicio;
                    }
                    else 
                    {
                        fechaSiguiente = fechaSiguiente.AddMinutes(15);
                    }                    
                }
                #endregion

                List<DpoDatos96DTO> datos = new List<DpoDatos96DTO>();
                DpoDatos96DTO dato;

                for (int i = 0; i < cleanArrayDataCompleta.Count; i += 96)
                {
                    List<string[]> temporal = cleanArrayDataCompleta.GetRange(i, 96);
                    string fecha = temporal[0][4].Substring(6, 2) + "/" + temporal[0][4].Substring(4, 2) + "/" + temporal[0][4].Substring(0, 4);
                    DateTime rFecha = DateTime.ParseExact(fecha,
                                                               ConstantesProdem.FormatoFecha,
                                                               CultureInfo.InvariantCulture);
                    List<DpoDatos96DTO> datosTemporal = new List<DpoDatos96DTO>();

                    for (int j = 0; j < 4; j++)
                    {
                        dato = new DpoDatos96DTO();
                        dato.Dpodatsubcodi = temporal[0][0];
                        dato.Dpodattnfcodi = temporal[0][1];
                        dato.Dpodattnfserie = temporal[0][2];
                        dato.Dpodatbarcodi = temporal[0][3];
                        dato.Dpodatfecha = rFecha;
                        dato.Dpodattipocodi = j + 1;
                        datosTemporal.Add(dato);
                    }

                    int index = 0;
                    foreach (var t in temporal)
                    {
                        decimal? activaNull = null;
                        decimal? reactivaNull = null;
                        decimal? tensionNull = null;
                        decimal? corrienteNull = null;

                        //Inserto el en el Hn el valor de energia activa
                        activaNull = (decimal.TryParse(t[5], out decimal activa)) ? (activa * 4 / 1000) : activaNull;
                        datosTemporal[0].GetType().GetProperty("H" + (index + 1).ToString()).
                                                    SetValue(datosTemporal[0], activaNull);
                        //Grabo el en el Hn el valor de energia reactiva
                        reactivaNull = (decimal.TryParse(t[6], out decimal reactiva)) ? (reactiva * 4 / 1000) : reactivaNull;
                        datosTemporal[1].GetType().GetProperty("H" + (index + 1).ToString()).
                                                    SetValue(datosTemporal[1], reactivaNull);
                        //Grabo el en el Hn el valor de registro de tension
                        tensionNull = (decimal.TryParse(t[7], out decimal tension)) ? tension : tensionNull;
                        datosTemporal[2].GetType().GetProperty("H" + (index + 1).ToString()).
                                                    SetValue(datosTemporal[2], tensionNull);
                        //Grabo el en el Hn el valor de registro de corriente
                        corrienteNull = (decimal.TryParse(t[8], out decimal corriente)) ? corriente : corrienteNull;
                        datosTemporal[3].GetType().GetProperty("H" + (index + 1).ToString()).
                                                    SetValue(datosTemporal[3], corrienteNull);
                        index++;
                    }
                    datos.AddRange(datosTemporal);
                }

                string nombreTabla = ConstantesDpo.tablaDpoDatos;

                //Validando si existe data registrada entre ese periodo
                string inicio = datos.First().Dpodatfecha.ToString(ConstantesDpo.FormatoFecha);
                string fin = (datos.First().Dpodatfecha.AddMonths(4)).ToString(ConstantesDpo.FormatoFecha);
                string subEstaciones = string.Join(",", datos.Select(x => "'" + x.Dpodatsubcodi + "'").Distinct().ToList());

                List<DpoDatos96DTO> reg = FactorySic.GetDpoDatos96Repository().ListBetweenDates(inicio, fin, subEstaciones);
                if (reg.Count > 0)
                {
                    FactorySic.GetDpoDatos96Repository().DeleteBetweenDates(inicio, fin, subEstaciones);
                }

                var filtrandoDatos = datos.GroupBy(x => new { x.Dpodatsubcodi, x.Dpodattnfcodi, x.Dpodatbarcodi, x.Dpodattipocodi, x.Dpodatfecha })
                                          .Select(x => x.First())
                                          .ToList();

                FactorySic.GetDpoDatos96Repository().BulkInsert((List<DpoDatos96DTO>)filtrandoDatos, nombreTabla);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El archivo de datos fue cargado de manera exitosa";

                //Se inicia el proceso para registrar las nuevas relaciones trafo - barras
                List<DpoDatos96DTO> info = this.ListaTrafoBarraInfo(inicio, fin);
                List<DpoTrafoBarraDTO> trafoBarra = this.ListaTrafoBarra();

                foreach (var item in info)
                {
                    DpoTrafoBarraDTO existeTrafoBarra = trafoBarra.Where(x => x.Tnfbartnfcodi == item.Dpodattnfcodi && 
                                                                              x.Tnfbarbarcodi == item.Dpobarcodiexcel)
                                                                  .FirstOrDefault();

                    if (existeTrafoBarra == null) 
                    {
                        DpoTrafoBarraDTO entidadTrafoBarra = new DpoTrafoBarraDTO();
                        entidadTrafoBarra.Tnfbartnfcodi = item.Dpodattnfcodi;
                        entidadTrafoBarra.Tnfbarbarcodi = item.Dpobarcodiexcel;
                        entidadTrafoBarra.Tnfbarbarnombre = item.Dpobarnombre;
                        entidadTrafoBarra.Tnfbarbartension = item.Dpobartension;
                        this.SaveDpoTrafoBarra(entidadTrafoBarra);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                //throw new Exception(ex.Message, ex);
                typeMsg = "Error en lectura de archivo";
                //detailMsg = "";
                return new { typeMsg, ex.Message, detailMsg };
            }

            return new { typeMsg, dataMsg, detailMsg};
        }

        /// <summary>
        /// Procesa el archivo .txt
        /// </summary>
        /// <param name="ruta">Dirección del archivo importado</param>
        /// <param name="nombre">Nombre del archivo importado</param>
        /// <param name="regFecha">Fecha de referencia para la ejecución</param>
        /// <param name="user">Usuario que realiza el proceso</param>
        /// <returns></returns>
        public object DatosDPO96FileServer(string ruta, string nombre, string regFecha, string user)
        {
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;
            List<string> detailMsg = new List<string>();//String.Empty;

            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            List<string> arrayData = new List<string>();
            try
            {
                using (StreamReader reader = FileServerScada.OpenReaderFile(nombre, ruta))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null) arrayData.Add(line);
                }
                arrayData.RemoveRange(0, 1);

                List<string[]> cleanArrayData = new List<string[]>();

                for (int i = 0; i < arrayData.Count; i++)
                {
                    string[] arry = arrayData[i].Split(new char[] { '\t' });
                    cleanArrayData.Add(arry);
                }

                List<string[]> cleanArrayDataCompleta = new List<string[]>();
                #region Revisando si la data esta completa

                //Se toma la primera fecha y se la formatea a 00:00 y despues le smo 15min
                //La fecha fin se calcula sumando 4 meses al inicio en 00:00
                DateTime fechaInicio = DateTime.ParseExact(cleanArrayData[0][4], "yyyyMMddHHmm",
                                       CultureInfo.InvariantCulture);//cleanArrayData[0][4];
                fechaInicio = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day, 0, 0, 0);
                DateTime fechaFin = fechaInicio.AddMonths(4);
                fechaInicio = fechaInicio.AddMinutes(15);

                //DateTime fechaFin = DateTime.ParseExact(cleanArrayData[cleanArrayData.Count - 1][4], "yyyyMMddHHmm",
                //                                        CultureInfo.InvariantCulture);//cleanArrayData[cleanArrayData.Count - 1][4];

                DateTime fechaSiguiente = fechaInicio;
                string subestacionSiguiente = string.Empty;
                string transformadorSiguiente = string.Empty;

                for (int i = 0; i < cleanArrayData.Count; i++)
                {
                    //Datos Actuales
                    string subestacion = cleanArrayData[i][0];
                    string transformador = cleanArrayData[i][1];
                    string serie = cleanArrayData[i][2];
                    string barra = cleanArrayData[i][3];
                    DateTime fechaHoy = DateTime.ParseExact(cleanArrayData[i][4], "yyyyMMddHHmm",
                                            CultureInfo.InvariantCulture);

                    if (fechaHoy == fechaInicio && fechaSiguiente == fechaInicio)
                    {
                        cleanArrayDataCompleta.Add(cleanArrayData[i]);
                    }
                    else
                    {
                        while (fechaSiguiente < fechaHoy)
                        {
                            string[] faltante = new string[9];
                            faltante[0] = subestacion;
                            faltante[1] = transformador;
                            faltante[2] = serie;
                            faltante[3] = barra;
                            faltante[4] = fechaSiguiente.ToString("yyyyMMddHHmm");
                            faltante[5] = "0"; faltante[6] = "0"; faltante[7] = "0"; faltante[8] = "0";
                            cleanArrayDataCompleta.Add(faltante);
                            fechaSiguiente = fechaSiguiente.AddMinutes(15);
                            if (fechaSiguiente >= fechaFin)
                            {
                                break;
                            }
                        }
                        cleanArrayDataCompleta.Add(cleanArrayData[i]);
                    }

                    if (fechaSiguiente == fechaFin)
                    {
                        fechaSiguiente = fechaInicio;
                    }
                    else
                    {
                        fechaSiguiente = fechaSiguiente.AddMinutes(15);
                    }
                }
                #endregion

                List<DpoDatos96DTO> datos = new List<DpoDatos96DTO>();
                DpoDatos96DTO dato;

                for (int i = 0; i < cleanArrayDataCompleta.Count; i += 96)
                {
                    List<string[]> temporal = cleanArrayDataCompleta.GetRange(i, 96);
                    string fecha = temporal[0][4].Substring(6, 2) + "/" + temporal[0][4].Substring(4, 2) + "/" + temporal[0][4].Substring(0, 4);
                    DateTime rFecha = DateTime.ParseExact(fecha,
                                                               ConstantesProdem.FormatoFecha,
                                                               CultureInfo.InvariantCulture);
                    List<DpoDatos96DTO> datosTemporal = new List<DpoDatos96DTO>();

                    for (int j = 0; j < 4; j++)
                    {
                        dato = new DpoDatos96DTO();
                        dato.Dpodatsubcodi = temporal[0][0];
                        dato.Dpodattnfcodi = temporal[0][1];
                        dato.Dpodattnfserie = temporal[0][2];
                        dato.Dpodatbarcodi = temporal[0][3];
                        dato.Dpodatfecha = rFecha;
                        dato.Dpodattipocodi = j + 1;
                        datosTemporal.Add(dato);
                    }

                    int index = 0;
                    foreach (var t in temporal)
                    {
                        decimal? activaNull = null;
                        decimal? reactivaNull = null;
                        decimal? tensionNull = null;
                        decimal? corrienteNull = null;

                        //Inserto el en el Hn el valor de energia activa
                        activaNull = (decimal.TryParse(t[5], out decimal activa)) ? (activa * 4 / 1000) : activaNull;
                        datosTemporal[0].GetType().GetProperty("H" + (index + 1).ToString()).
                                                    SetValue(datosTemporal[0], activaNull);
                        //Grabo el en el Hn el valor de energia reactiva
                        reactivaNull = (decimal.TryParse(t[6], out decimal reactiva)) ? (reactiva * 4 / 1000) : reactivaNull;
                        datosTemporal[1].GetType().GetProperty("H" + (index + 1).ToString()).
                                                    SetValue(datosTemporal[1], reactivaNull);
                        //Grabo el en el Hn el valor de registro de tension
                        tensionNull = (decimal.TryParse(t[7], out decimal tension)) ? tension : tensionNull;
                        datosTemporal[2].GetType().GetProperty("H" + (index + 1).ToString()).
                                                    SetValue(datosTemporal[2], tensionNull);
                        //Grabo el en el Hn el valor de registro de corriente
                        corrienteNull = (decimal.TryParse(t[8], out decimal corriente)) ? corriente : corrienteNull;
                        datosTemporal[3].GetType().GetProperty("H" + (index + 1).ToString()).
                                                    SetValue(datosTemporal[3], corrienteNull);
                        index++;
                    }
                    datos.AddRange(datosTemporal);
                }

                string nombreTabla = ConstantesDpo.tablaDpoDatos;

                //Validando si existe data registrada entre ese periodo
                string inicio = datos.First().Dpodatfecha.ToString(ConstantesDpo.FormatoFecha);
                string fin = (datos.First().Dpodatfecha.AddMonths(4)).ToString(ConstantesDpo.FormatoFecha);                
                string subEstaciones = string.Join(",", datos.Select(x => "'" + x.Dpodatsubcodi + "'").Distinct().ToList());

                List<DpoDatos96DTO> reg = FactorySic.GetDpoDatos96Repository().ListBetweenDates(inicio, fin, subEstaciones);
                if (reg.Count > 0)
                {
                    FactorySic.GetDpoDatos96Repository().DeleteBetweenDates(inicio, fin, subEstaciones);
                }

                var filtrandoDatos = datos.GroupBy(x => new { x.Dpodatsubcodi, x.Dpodattnfcodi, x.Dpodatbarcodi, x.Dpodattipocodi, x.Dpodatfecha })
                                          .Select(x => x.First())
                                          .ToList();

                FactorySic.GetDpoDatos96Repository().BulkInsert((List<DpoDatos96DTO>)filtrandoDatos, nombreTabla);
                typeMsg = ConstantesProdem.MsgSuccess;
                dataMsg = "El archivo de datos fue cargado de manera exitosa";

                //Se inicia el proceso para registrar las nuevas relaciones trafo - barras
                List<DpoDatos96DTO> info = this.ListaTrafoBarraInfo(inicio, fin);
                List<DpoTrafoBarraDTO> trafoBarra = this.ListaTrafoBarra();

                foreach (var item in info)
                {
                    DpoTrafoBarraDTO existeTrafoBarra = trafoBarra.Where(x => x.Tnfbartnfcodi == item.Dpodattnfcodi &&
                                                                              x.Tnfbarbarcodi == item.Dpobarcodiexcel)
                                                                  .FirstOrDefault();

                    if (existeTrafoBarra == null)
                    {
                        DpoTrafoBarraDTO entidadTrafoBarra = new DpoTrafoBarraDTO();
                        entidadTrafoBarra.Tnfbartnfcodi = item.Dpodattnfcodi;
                        entidadTrafoBarra.Tnfbarbarcodi = item.Dpobarcodiexcel;
                        entidadTrafoBarra.Tnfbarbarnombre = item.Dpobarnombre;
                        entidadTrafoBarra.Tnfbarbartension = item.Dpobartension;
                        this.SaveDpoTrafoBarra(entidadTrafoBarra);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                //throw new Exception(ex.Message, ex);
                typeMsg = "Error en lectura de archivo";
                //detailMsg = "";
                return new { typeMsg, ex.Message, detailMsg };
            }

            return new { typeMsg, dataMsg, detailMsg };
        }

        /// <summary>
        /// Inserta un registro de la tabla DPO_TRAFOBARRA
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoTrafoBarra(DpoTrafoBarraDTO entity)
        {
            try
            {
                FactorySic.GetDpoTrafoBarraRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista las formulas por tipo(usuario)
        /// </summary>
        /// <returns></returns>
        public List<MePerfilRuleDTO> ListaPerfilRuleForEstimadorByPrefijo(string prefijo)
        {
            return FactorySic.GetPrnPronosticoDemandaRepository().ListPerfilRuleByEstimador(prefijo + "%");
        }

        /// <summary>
        /// Método que Lista las versiones registradas
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListaBarrasByCodigos(string codigos)
        {
            return FactorySic.GetPrGrupoRepository().ListaBarrasByCodigos(codigos);
        }

        /// <summary>
        /// Inserta un registro de la tabla DPO_BARRASPL
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoBarraSPL(DpoBarraSplDTO entity)
        {
            try
            {
                FactorySic.GetDpoBarraSplRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de la tabla DPO_REL_SPL_VERSION
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteDpoRelSplFormula(int id)
        {
            try
            {
                FactorySic.GetDpoRelSplFormulaRepository().Delete(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla DPO_VERSIONRELACION
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteDpoVersionRelacion(int id)
        {
            try
            {
                FactorySic.GetDpoVersionRelacionRepository().Delete(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros que pertenecen a una version en la tabla DPO_REL_SPL_VERSION
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteDpoRelSplFormulaByVersion(int id)
        {
            try
            {
                FactorySic.GetDpoRelSplFormulaRepository().DeleteByVersion(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros que pertenecen a una version en la tabla DPO_REL_SPL_VERSION
        /// especificando la version y barra
        /// </summary>
        /// <param name="version">Id de la version</param>
        /// <param name="barra">Id de la barra</param>
        /// <returns></returns>
        public void DeleteDpoRelSplFormulaByVersionxBarra(int version, int barra)
        {
            try
            {
                FactorySic.GetDpoRelSplFormulaRepository().DeleteByVersionxBarra(version, barra);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla DPO_VERSIONRELACION
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int SaveDpoVersionRelacion(DpoVersionRelacionDTO entity)
        {
            try
            {
                return FactorySic.GetDpoVersionRelacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla DPO_REL_SPL_FORMULA
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoRelSplFormula(DpoRelSplFormulaDTO entity)
        {
            try
            {
                FactorySic.GetDpoRelSplFormulaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza las formulas de la tabla DPO_REL_SPL_FORMULA
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void UpdateRegistroDpoRelSplFormula(DpoRelSplFormulaDTO entity)
        {
            try
            {
                FactorySic.GetDpoRelSplFormulaRepository().UpdateFormulas(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza el estado de varios registros de la tabla DPO_BARRASPL
        /// </summary>
        /// <param name="ids">Ids con formato para el IN en el query</param>
        /// <param name="estado">estado A o I</param>
        /// <returns></returns>
        public void UpdateDpoBarraSPLEstado(string ids, string estado)
        {
            try
            {
                FactorySic.GetDpoBarraSplRepository().UpdateDpoBarraSPlEstado(ids, estado);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Método que Lista las versiones registradas
        /// </summary>
        /// <returns></returns>
        public List<DpoVersionRelacionDTO> ListaVersiones()
        {
            return FactorySic.GetDpoVersionRelacionRepository().List();
        }

        /// <summary>
        /// Método que Lista las barras que estan relacionadas a una version
        /// </summary>
        /// <returns></returns>
        public List<DpoRelSplFormulaDTO> ListaBarrasxVersion(int id)
        {
            return FactorySic.GetDpoRelSplFormulaRepository().ListBarrasxVersion(id);
        }

        /// <summary>
        /// Método que Lista las barras SPL
        /// </summary>
        /// <returns></returns>
        public List<DpoBarraSplDTO> ListaBarraSpl()
        {
            return FactorySic.GetDpoBarraSplRepository().List();
        }

        /// <summary>
        /// Método que Lista las barras que estan registradas en PR_GRUPO
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListaBarrasGrupo()
        {
            return FactorySic.GetPrGrupoRepository().List();
        }

        /// <summary>
        /// Método que Lista las barras SPL por grupocodi(usando IN)
        /// </summary>
        /// <returns></returns>
        public List<DpoBarraSplDTO> ListaBarrasSPLByGrupo(string barras)
        {
            return FactorySic.GetDpoBarraSplRepository().ListBarrasSPLByGrupo(barras);
        }

        /// <summary>
        /// Método que Lista las formulas vegetativas
        /// </summary>
        /// <returns></returns>
        public List<DpoRelSplFormulaDTO> ListFormulasVegetativa()
        {
            return FactorySic.GetDpoRelSplFormulaRepository().ListFormulasVegetativa();
        }

        /// <summary>
        /// Método que Lista lasformulas industriales
        /// </summary>
        /// <returns></returns>
        public List<DpoRelSplFormulaDTO> ListFormulasIndustrial()
        {
            return FactorySic.GetDpoRelSplFormulaRepository().ListFormulasIndustrial();
        }

        /// <summary>
        /// Método que registra las nuevas barras como spl
        /// </summary>
        /// <returns></returns>
        public void RegistraBarraSPL(int[] barras, string usuario)
        {
            //Todas las barras registradas en DPO_BARRASSPL
            List<DpoBarraSplDTO> barrasRegistradas = this.ListaBarraSpl();
            List<DpoBarraSplDTO> nuevasBarrasSPL = new List<DpoBarraSplDTO>();
            DpoBarraSplDTO entity = new DpoBarraSplDTO();
            string ids = "";
            List<PrGrupoDTO> br = new List<PrGrupoDTO>();
            if (barrasRegistradas.Count == 0)
            {
                ids = string.Join(",", barras);
                br = this.ListaBarrasByCodigos(ids);
                foreach (var item in br)
                {
                    entity = new DpoBarraSplDTO();
                    entity.Grupocodi = item.Grupocodi;
                    entity.Gruponomb = item.Gruponomb.Trim();
                    entity.Grupoabrev = item.Grupoabrev.Trim();
                    entity.Barsplestado = "A";
                    entity.Barsplusucreacion = usuario;
                    entity.Barsplfeccreacion = DateTime.Now;
                    SaveDpoBarraSPL(entity);
                }
            }
            else
            {
                //Solo las barras activas
                List<DpoBarraSplDTO> barrasActivas = barrasRegistradas
                                                         .Where(x => x.Barsplestado == "A").ToList();
                List<DpoBarraSplDTO> barrasInactivas = barrasRegistradas
                                                         .Where(x => x.Barsplestado == "I").ToList();
                //Los ids(Grupocodi) de las barras activas
                int[] idActivas = barrasActivas.Select(x => x.Grupocodi).ToArray();
                //Los ids(Grupocodi) de las barras inactivas
                int[] idInactivas = barrasInactivas.Select(x => x.Grupocodi).ToArray();
                //Barras que se podran en estado : I
                int[] barrasToInactiva = idActivas.Except(barras).ToArray();
                //Barras nuevas que se agregan
                int[] barrasToSave = barras.Except(barrasRegistradas.Select(x => x.Grupocodi).ToArray()).ToArray();
                //Barras que  se pondran en estado: A
                int[] barrasToActiva = idInactivas.Intersect(barras).ToArray();

                //Se trabaja con las barras que pasaran su estado de A a I
                if (barrasToInactiva.Count() > 0)
                {
                    ids = string.Join(",", barrasToInactiva);
                    UpdateDpoBarraSPLEstado(ids, "I");
                }
                //Se trabaja con las barras nuevas
                if (barrasToSave.Count() > 0)
                {
                    ids = string.Join(",", barrasToSave);
                    br = this.ListaBarrasByCodigos(ids);
                    foreach (var item in br)
                    {
                        entity = new DpoBarraSplDTO();
                        entity.Grupocodi = item.Grupocodi;
                        entity.Gruponomb = item.Gruponomb.Trim();
                        entity.Grupoabrev = item.Grupoabrev.Trim();
                        entity.Barsplestado = "A";
                        entity.Barsplusucreacion = usuario;
                        entity.Barsplfeccreacion = DateTime.Now;
                        SaveDpoBarraSPL(entity);
                    }
                }
                //Se trabajara con las barras que cambian de I a A
                if (barrasToActiva.Count() > 0)
                {
                    ids = string.Join(",", barrasToActiva);
                    UpdateDpoBarraSPLEstado(ids, "A");
                }
            }
        }

        /// <summary>
        /// Método que registra las versiones con sus barras SPL
        /// </summary>
        /// <param name="version">nombre de la version</param>
        /// <param name="barras">ids de las barras</param>
        /// <param name="usuario">nombre del usuario</param>
        /// <returns></returns>
        public void RegistraNuevaVersion(string version, int[] barras, string usuario)
        {
            //Registro en la tabla DPO_VersioNRelacion
            DpoVersionRelacionDTO entityVersion = new DpoVersionRelacionDTO();
            entityVersion.Dposplnombre = version.Trim();
            entityVersion.Dposplusucreacion = usuario;
            entityVersion.Dposplfecmodificacion = DateTime.Now;
            int id = this.SaveDpoVersionRelacion(entityVersion);

            //Registro para la tabla DPO_REL_SPL_FORMULA
            DpoRelSplFormulaDTO entityDetalle;
            foreach (var item in barras)
            {
                entityDetalle = new DpoRelSplFormulaDTO();
                entityDetalle.Dposplcodi = id;
                entityDetalle.Barsplcodi = item;
                entityDetalle.Ptomedicodifveg = null;
                entityDetalle.Ptomedicodiful = null;
                this.SaveDpoRelSplFormula(entityDetalle);
            }
        }

        /// <summary>
        /// Método que actualiza las versiones no sus barras SPL
        /// </summary>
        /// <param name="version">nombre de la version</param>
        /// <param name="barras">ids de las barras</param>
        /// <param name="usuario">nombre del usuario</param>
        /// <returns></returns>
        public void ActualizaVersion(int version, int[] barras, string usuario)
        {
            List<DpoRelSplFormulaDTO> listaRegistrada = this.ListaBarrasxVersion(version);
            //Ids de las barras SPL registrados
            int[] idRegistrados = listaRegistrada.Select(x => x.Barsplcodi).ToArray();
            string idBarras = string.Join(",", barras);
            //Ids de las barras SPL consultados usando los grupcodi
            List<DpoBarraSplDTO> listaBarras = this.ListaBarrasSPLByGrupo(idBarras);
            int[] barrasSPL = listaBarras.Select(x => x.Barsplcodi).ToArray();
            int[] idEliminar = idRegistrados.Except(barrasSPL).ToArray();

            if (idEliminar.Count() > 0)
            {
                foreach (var item in idEliminar)
                {
                    this.DeleteDpoRelSplFormulaByVersionxBarra(version, item);
                }
            }

            //Registro las nuevas barras para la tabla DPO_REL_SPL_FORMULA
            DpoRelSplFormulaDTO entity;
            int[] idRegistrar = barrasSPL.Except(idRegistrados).ToArray();
            if (idRegistrar.Count() > 0)
            {
                foreach (var item in idRegistrar)
                {
                    entity = new DpoRelSplFormulaDTO();
                    entity.Dposplcodi = version;
                    entity.Barsplcodi = item;
                    entity.Ptomedicodifveg = null;
                    entity.Ptomedicodiful = null;
                    this.SaveDpoRelSplFormula(entity);
                }
            }
        }

        /// <summary>
        /// Método que actualiza las formulas de la tabla DPO_REL_SPL_FORMULA
        /// </summary>
        /// <param name="dato">Entidad de DpoRelSplFormultaDTO</param>
        /// <param name="flag">1 para editar, 2 para limpiar la relacion</param>
        /// <returns></returns>
        public void ActualizarRegistro(DpoRelSplFormulaDTO dato, int flag)
        {
            if (flag == 1)
            {
                //Actualizo las formulas para la tabla DPO_REL_SPL_FORMULA
                DpoRelSplFormulaDTO entity = new DpoRelSplFormulaDTO();
                entity.Splfrmcodi = dato.Splfrmcodi;
                entity.Ptomedicodifveg = dato.Ptomedicodifveg;
                entity.Ptomedicodiful = dato.Ptomedicodiful;
                entity.Splfrmarea = dato.Splfrmarea;
                this.UpdateRegistroDpoRelSplFormula(entity);
            }
            else {
                //Actualizo las formulas para la tabla DPO_REL_SPL_FORMULA
                DpoRelSplFormulaDTO entity = new DpoRelSplFormulaDTO();
                entity.Splfrmcodi = dato.Splfrmcodi;
                entity.Ptomedicodifveg = null;
                entity.Ptomedicodiful = null;
                entity.Splfrmarea = null;
                this.UpdateRegistroDpoRelSplFormula(entity);
            }
        }

        /// <summary>
        /// Método que elimina la version y su detalle
        /// </summary>
        /// <param name="id">id de la version a eliminar</param>
        /// <returns></returns>
        public void EliminarVersion(int id)
        {
            //Registro en la tabla DPO_REL_SPL_FORMULA
            this.DeleteDpoRelSplFormulaByVersion(id);

            //Registro para la tabla DPO_VERSION_RELACION
            this.DeleteDpoVersionRelacion(id);
        }

        /// <summary>
        /// Método que Lista las barras que estan relacionadas a una version
        /// </summary>
        /// <returns></returns>
        public List<DpoRelSplFormulaDTO> ListaBarrasPorVersion(int id)
        {
            List<DpoRelSplFormulaDTO> lista = new List<DpoRelSplFormulaDTO>();
            lista = FactorySic.GetDpoRelSplFormulaRepository().ListBarrasxVersion(id);
            foreach (var a in lista)
            {
                List<DpoRelSplFormulaDTO> subListaVeg = lista.Where(x => (x.Splfrmcodi != a.Splfrmcodi) && (x.Ptomedicodifveg != null)).ToList();
                List<DpoRelSplFormulaDTO> subListaUli = lista.Where(x => (x.Splfrmcodi != a.Splfrmcodi) && (x.Ptomedicodiful != null)).ToList();
                if (subListaVeg.Count > 0)
                {
                    List<int?> tempVeg = new List<int?>();
                    foreach (var v in subListaVeg)
                    {
                        tempVeg.Add(v.Ptomedicodifveg);
                    }
                    a.VegNoDisponible = tempVeg;
                }
                if (subListaUli.Count > 0)
                {
                    List<int?> tempUli = new List<int?>();
                    foreach (var u in subListaUli)
                    {
                        tempUli.Add(u.Ptomedicodiful);
                    }
                    a.UliNoDisponible = tempUli;
                }
            }

            return lista;
        }
        #endregion

        #region Iteración 2 - REQ03

        public void SaveDpoMedicion96(DpoMedicion96DTO entity)
        {
            try
            {
                FactorySic.GetDpoMedicion96Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        public void UpdateDpoMedicion96(DpoMedicion96DTO entity)
        {
            try
            {
                FactorySic.GetDpoMedicion96Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        public List<DpoMedicion96DTO> ListDpoMedicion96PorVersion(
            int tipoMedicion, int tipoDato, int idVersion,
            string fechaIni, string fechaFin)
        {
            return FactorySic.GetDpoMedicion96Repository()
                .ListByVersion(tipoMedicion, tipoDato,
                idVersion, fechaIni, fechaFin);
        }

        public List<DpoMedicion96DTO> ListDatosMediciongrp(
            List<int> idPuntos, string idTipoInfo, 
            string regFecha, string idVersion)
        {
            string strPuntos = string.Join(",", idPuntos);
            return FactorySic.GetDpoMedicion96Repository()
                .ObtenerDatosMediciongrp(strPuntos, idTipoInfo,
                regFecha, idVersion);
        }
        public List<DpoMedicion96DTO> ListDatosDemandaTotalPorBarra(
            int idTipoReg, int idVersion, string selFecha)
        {
            return FactorySic.GetDpoMedicion96Repository()
                .ObtenerDatosPorVersion(idTipoReg,
                ConstantesDpo.DpotmeDemandaTotalPorBarra,
                idVersion, selFecha);
        }
        public List<DpoMedicion96DTO> ListDatosDemTotalPorBarraNulleable(
            int idTipoReg, int idVersion, string selFecha)
        {
            return FactorySic.GetDpoMedicion96Repository()
                .ObtenerDatosPorVersionNulleable(idTipoReg,
                ConstantesDpo.DpotmeDemandaTotalPorBarra,
                idVersion, selFecha);
        }
        public DpoMedicion96DTO GetByIdDpoMedicion96(int idRegistro, 
            int idTipoReg, string idTipoInfo, int idVersion,
            string selFecha)
        {
            return FactorySic.GetDpoMedicion96Repository()
                .ObtenerDatosPorId(idRegistro, idTipoReg,
                idTipoInfo, idVersion, selFecha);
        }
        public List<DpoMedicion96DTO> GetByVersionDpoMedicion96(
            int idTipoReg, string idTipoInfo, int idVersion, 
            string selFecha)
        {
            return FactorySic.GetDpoMedicion96Repository()
                .ObtenerDatosPorVersion(idTipoReg, idTipoInfo,
                idVersion, selFecha);
        }
        public DpoMedicion96DTO GetGroupDpoMedicion96(
            string idRegistro, int idTipoReg, string idTipoInfo,
            int idVersion, string selFecha)
        {
            return FactorySic.GetDpoMedicion96Repository()
                .ObtenerDatosAgrupados(idRegistro, idTipoReg, 
                idTipoInfo, idVersion, selFecha);
        }

        public void SaveDpoConfiguracion(DpoConfiguracionDTO entity)
        {
            try
            {
                FactorySic.GetDpoConfiguracionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateDpoConfiguracion(DpoConfiguracionDTO entity)
        {
            try
            {
                FactorySic.GetDpoConfiguracionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public DpoConfiguracionDTO GetByIdDpoConfiguracion(int id)
        {
            return FactorySic.GetDpoConfiguracionRepository().GetById(id);
        }

        public DpoConfiguracionDTO GetByVersionDpoConfiguracion(int idVersion)
        {
            return FactorySic.GetDpoConfiguracionRepository()
                .GetByVersion(idVersion);
        }

        public object ReprogramaDatosVeg(int idVersion,
            string nomVersion, string fecha)
        {
            List<MePtomedicionDTO> puntosMedicion = this.ListaBarrasFormato();

            List<int> idPuntos = puntosMedicion
                .Select(x => x.Ptomedicodi)
                .ToList();

            string fecFin = DateTime.ParseExact(fecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture)
                .AddDays(1).ToString(ConstantesProdem.FormatoFecha);

            List<string[]> table = new List<string[]>();
            List<string> rowHeader = new List<string>() { "FECHA HORA / BARRA" };
            List<string> rowFooter = new List<string>() { "ID" };
            rowHeader.AddRange(puntosMedicion.Select(x => x.Ptomedibarranomb).ToList());
            rowFooter.AddRange(idPuntos.Select(x => x.ToString()).ToList());
            table.Add(rowHeader.ToArray());

            int i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                table.Add(new string[rowHeader.Count]);
                i++;
            }
            table.Add(rowFooter.ToArray());

            //.Llenado de la tabla
            //.Llenado de la columna intervalos
            string[] itv = UtilProdem.GenerarIntervalosFecha(
                ConstantesDpo.Itv30min, fecha, fecFin);
            i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                table[i + 1][0] = itv[i];
                i++;
            }
            //.Llenado de las mediciones
            List<DpoMedicion96DTO> datosMedicion = this.ListDpoMedicion96PorVersion(
                ConstantesDpo.DpotmeDemVegetativa, ConstantesDpo.DpotdtPunto,
                idVersion, fecha, fecha);

            i = 1;
            foreach (int idPunto in idPuntos)
            {
                DpoMedicion96DTO entMedicion = datosMedicion
                    .FirstOrDefault(x => x.Dpomedcodi == idPunto)
                    ?? new DpoMedicion96DTO();
                decimal?[] medicion = UtilDpo.ConvertirMedEnAryNullable(
                    ConstantesDpo.Itv15min, entMedicion);
                for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                {
                    table[j + 1][i] = medicion[(j * 2) + 1].ToString();
                }
                i++;
            }

            //.Creación de mensajes 
            string msgVegtativa = $"• Vegetativa: Tabla mostrada con datos de la versión {nomVersion}";
            string msgUsuLibres = $"• Usuarios libres: Tabla mostrada con datos de la versión {nomVersion}";
            
            return new
            {
                msgVegtativa,
                msgUsuLibres,
                datos = table,
            };
        }

        public object ReprogramaEjecutarVeg(int idVersion,
            string nomVersion, string fecha, int idVersionAnterior,
            int idVersionComparacion)
        {
            List<MePtomedicionDTO> puntosMedicion = this.ListaBarrasFormato();

            List<int> idPuntos = puntosMedicion
                .Select(x => x.Ptomedicodi)
                .ToList();
            List<int> idBarras = puntosMedicion
                .Select(x => (int)x.Grupocodi)
                .ToList();
            
            //Calcular pronóstico vegetativo
            //.Obtiene la configuración de la versión
            DpoConfiguracionDTO configuracion = this.GetByVersionDpoConfiguracion(
                idVersion);
            if (configuracion.Dpocngcodi == 0)
            {
                configuracion = this.GetByIdDpoConfiguracion(
                    ConstantesDpo.DpocngcodiGeneral);
            }

            DateTime parseFecha = DateTime.ParseExact(
                fecha, ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);

            //.Obtiene la lista de feriados
            int anio = parseFecha.Year;
            int anioPrev = anio - 1;
            List<DpoFeriadosDTO> datosFeriados = this
                .GetByAnioRangoDpoFeriados(anioPrev, anio);

            List<DateTime> feriados = datosFeriados
                .Select(x => x.Dpoferfecha)
                .ToList();

            //Version de la informacion para el proceso
            string version = $"{idVersionComparacion},{idVersionAnterior}";

            List<DemVegCol> modelo = new List<DemVegCol>();
            if (feriados.Contains(parseFecha))
            {

                modelo = ProcVegTiempoRealFeriado(parseFecha, version, configuracion);
            }
            else
            {                
                modelo = ProcVegTiempoReal(parseFecha, version, configuracion);
            }

            //Generación de tabla de presentación (Handsontable)
            List<string[]> tabla = GenerarTablaVegetativa(puntosMedicion, 
                idPuntos, idBarras, modelo, fecha);

            string horaEjecucion = $"{DateTime.Now.ToString("hh:mm")}h";
            string msgVegtativa = $"• Vegetativa: Tabla mostrada con datos del pronóstico realizado con datos TNA hasta las {horaEjecucion}";
            string msgUsuLibres = $"• Usuarios libres: Tabla mostrada con datos de la versión {nomVersion}";

            return new
            {
                msgVegtativa,
                msgUsuLibres,
                datos = tabla,
            };
        }

        /// <summary>
        /// Proceso de cálculo de pronóstico vegetativo en tiempo real
        /// </summary>
        /// <param name="fecha">Fecha a pronosticar</param>
        /// <param name="version">Version complementaria para la informacion</param>
        /// <param name="configuracion">Configuracion del pronostico</param>
        /// <returns></returns>
        public List<DemVegCol> ProcVegTiempoReal(
            DateTime fecha, string version, DpoConfiguracionDTO configuracion)
        {            
            ProcesoPronostico proc = new ProcesoPronostico();
            return proc.EjecutarDemVegTipico(fecha, version, configuracion);
        }

        /// <summary>
        /// Proceso de cálculo de pronóstico vegetativo en tiempo real
        /// </summary>
        /// <param name="fecha">Fecha a pronosticar</param>
        /// <param name="version">Version complementaria para la informacion</param>
        /// <param name="configuracion">Configuracion del pronostico</param>
        /// <returns></returns>
        public List<DemVegCol> ProcVegTiempoRealFeriado(
            DateTime fecha, string version, DpoConfiguracionDTO configuracion)
        {            
            ProcesoPronostico proc = new ProcesoPronostico();
            return proc.EjecutarDemVegFeriado(fecha, version, configuracion);
        }

        /// <summary>
        /// Genera la tabla de presentación (Handsontable)
        /// del modulo Reprograma/Vegetativa
        /// </summary>
        /// <param name="puntosMedicion"></param>
        /// <param name="idPuntos"></param>
        /// <param name="idBarras"></param>
        /// <param name="datosMedicion"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<string[]> GenerarTablaVegetativa(
            List<MePtomedicionDTO> puntosMedicion, List<int> idPuntos,
            List<int> idBarras, List<DemVegCol> datosMedicion,
            string fecha)
        {
            List<string[]> table = new List<string[]>();
            List<string> rowHeader = new List<string>() { "FECHA HORA / BARRA" };
            List<string> rowFooter = new List<string>() { "ID" };
            rowHeader.AddRange(puntosMedicion.Select(x => x.Ptomedibarranomb).ToList());
            rowFooter.AddRange(idPuntos.Select(x => x.ToString()).ToList());
            table.Add(rowHeader.ToArray());

            int i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                table.Add(new string[rowHeader.Count]);
                i++;
            }
            table.Add(rowFooter.ToArray());

            //.Llenado de la tabla
            //.Llenado de la columna intervalos
            string fecFin = DateTime.ParseExact(fecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture)
                .AddDays(1).ToString(ConstantesProdem.FormatoFecha);

            string[] itv = UtilProdem.GenerarIntervalosFecha(
                ConstantesDpo.Itv30min, fecha, fecFin);
            i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                table[i + 1][0] = itv[i];
                i++;
            }

            i = 1;
            foreach (int idBarra in idBarras)
            {
                DemVegCol entMedicion = datosMedicion
                    .FirstOrDefault(x => x.Id == idBarra) ?? null;

                List<decimal?> medicion = new List<decimal?>();

                if (entMedicion == null)
                    medicion = new decimal?[ConstantesProdem.Itv30min].ToList();
                else
                    medicion = entMedicion.Valores;

                for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                {
                    table[j + 1][i] = medicion[j].ToString();
                }
                i++;
            }

            return table;
        }


        /// <summary>
        /// Genera el archivo excel de seguimiento del proceso
        /// de cálculo del pronóstico vegetativo
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public string ReporteControlVegetativa(DpoModProcesoVeg modelo)
        {            
            List<PrnFormatoExcel> excel = new List<PrnFormatoExcel>();

            #region Tipos de datos
            List<string> tipo1 = new List<string>()
            {
                "PromHistoricoFiltroF2",
                "PromHistoricoFiltroF3",
                "PromHistoricoPerfilesMin",
                "PromHistoricoFiltroF4",
                "PromHistoricoFiltroF5",
                "HoyPerfilesMin",
                "HoyFiltroF2",
                "HoyFiltroF6",
                "HoyFiltroF5",
                "HoyPronosticoMinVegSCO",
                "HoyPronosticoMinVegSPR",
            };
            List<string> tipo2 = new List<string>();
            
            List<string> tipo3 = new List<string>()
            {
                "HoyPronostico30MinVegSCO",
                "HoyPronostico30MinVegSPR",
            };

            List<string> tipoIntervalo1 = new List<string>()
            {
                "PromHistoricoFiltroF2",               
            };

            #endregion

            PropertyInfo[] propModelo = typeof(DpoModProcesoVeg).GetProperties();
            foreach (PropertyInfo prop in propModelo)
            {
                string propName = prop.Name;
                if (tipo1.Contains(propName))
                {                    
                    List<DpoMedicionTotal> med = (List<DpoMedicionTotal>)modelo
                        .GetType()
                        .GetProperty(propName)
                        .GetValue(modelo);

                    if (med == null) continue;
                    if (med.Count == 0) continue;

                    List<int> idPuntos = med.Select(x => x.Ptomedicodi)
                        .Distinct()
                        .ToList();

                    List<string[]> contenido = new List<string[]>();
                    if (tipoIntervalo1.Contains(propName))
                    {
                        contenido.Add(modelo.ItvPromF2F3PerfMin.ToArray());
                    }

                    foreach (int idPunto in idPuntos)
                    {
                        List<DpoMedicionTotal> datosPuntos = med
                            .Where(x => x.Ptomedicodi == idPunto)
                            .OrderBy(x => x.Medifecha)
                            .ToList();

                        List<decimal> valores = new List<decimal>();
                        foreach (DpoMedicionTotal dato in datosPuntos)
                        {
                            valores.AddRange(dato.Medicion);
                        }

                        contenido.Add(valores
                            .Select(x => x.ToString())
                            .ToArray());
                    }

                    List<string> cabecera = idPuntos
                        .Select(x => x.ToString())
                        .ToList();
                    if (tipoIntervalo1.Contains(propName))
                    {
                        cabecera.Insert(0, "Fecha Hora");
                    }

                    PrnFormatoExcel libro = new PrnFormatoExcel()
                    {
                        NombreLibro = propName,
                        Titulo = propName,
                        Contenido = contenido.ToArray(),
                        Cabecera = cabecera.ToArray(),
                        AnchoColumnas = Enumerable.Repeat(20, cabecera.Count).ToArray(),
                    };
                    
                    excel.Add(libro);
                }
                else if (tipo2.Contains(propName))
                {                    
                    List<DpoMedicion1440> med = (List<DpoMedicion1440>)modelo
                        .GetType()
                        .GetProperty(propName)
                        .GetValue(modelo);

                    if (med == null) continue;
                    if (med.Count == 0) continue;

                    List<int> idPuntos = med.Select(x => x.Ptomedicodi)
                        .Distinct()
                        .ToList();

                    List<string[]> contenido = new List<string[]>();
                    foreach (int idPunto in idPuntos)
                    {
                        List<DpoMedicion1440> datosPuntos = med
                            .Where(x => x.Ptomedicodi == idPunto)
                            .OrderBy(x => x.Medifecha)
                            .ToList();

                        List<decimal> valores = new List<decimal>();
                        foreach (DpoMedicion1440 dato in datosPuntos)
                        {
                            valores.AddRange(dato.Medicion);
                        }

                        contenido.Add(valores
                            .Select(x => x.ToString())
                            .ToArray());
                    }

                    PrnFormatoExcel libro = new PrnFormatoExcel()
                    {
                        NombreLibro = propName,
                        Titulo = propName,
                        Contenido = contenido.ToArray(),
                        Cabecera = idPuntos.Select(x => x.ToString()).ToArray(),
                        AnchoColumnas = Enumerable.Repeat(20, idPuntos.Count).ToArray(),
                    };

                    excel.Add(libro);
                }
                else if (tipo3.Contains(propName))
                {
                    List<DpoMedicion96DTO> med = (List<DpoMedicion96DTO>)modelo
                        .GetType()
                        .GetProperty(propName)
                        .GetValue(modelo);

                    if (med == null) continue;
                    if (med.Count == 0) continue;

                    List<int> idPuntos = med.Select(x => x.Dpomedcodi)
                        .Distinct()
                        .ToList();

                    List<string[]> contenido = new List<string[]>();
                    foreach (int idPunto in idPuntos)
                    {
                        List<DpoMedicion96DTO> datosPunto = med
                            .Where(x => x.Dpomedcodi == idPunto)
                            .OrderBy(x => x.Dpomedfecha)
                            .ToList();

                        List<decimal> valores = new List<decimal>();
                        foreach (DpoMedicion96DTO dato in datosPunto)
                        {
                            decimal[] medicion = UtilDpo.ConvertirMedicionEnArreglo(
                                ConstantesDpo.Itv15min, dato)
                                .Where((x, index) => index % 2 != 0)
                                .ToArray();

                            valores.AddRange(medicion);
                        }

                        contenido.Add(valores
                            .Select(x => x.ToString())
                            .ToArray());
                    }

                    PrnFormatoExcel libro = new PrnFormatoExcel()
                    {
                        NombreLibro = propName,
                        Titulo = propName,
                        Contenido = contenido.ToArray(),
                        Cabecera = idPuntos.Select(x => x.ToString()).ToArray(),
                        AnchoColumnas = Enumerable.Repeat(20, idPuntos.Count).ToArray(),
                    };

                    excel.Add(libro);
                }
            }

            string pathFile = AppDomain.CurrentDomain.BaseDirectory + ConstantesDpo.RutaReportes;
            string filename = "REPVEG";

            return this.ExportarReporteConLibros(excel, pathFile, filename);
        }

        /// <summary>
        /// Devuelve los componentes individuales de las relaciones tna
        /// </summary>
        /// <returns></returns>
        public DpoModRelacionTna ObtenerComponentesRelTNA()
        {
            //Obtiene relaciones
            List<PrnRelacionTnaDTO> listaRel = this.ListaRelacionTna();

            List<DpoMedCompFormula> modFormulasVeg = new List<DpoMedCompFormula>();
            List<DpoMedCompFormula> modFormulasInput = new List<DpoMedCompFormula>();

            #region Obtiene las "cabeceras" de cada formula
            foreach (PrnRelacionTnaDTO rel in listaRel)
            {
                //Descompone la formula del componente vegetativo de la relación
                //Identifica los componentes de la formula por tipo
                List<Scada.FormulaItem> listaComponentes = this.DescomponerFormula(
                    rel.Reltnaformula);

                foreach (Scada.FormulaItem comp in listaComponentes)
                {
                    DpoMedCompFormula entFormInput = new DpoMedCompFormula();

                    entFormInput.idRelacion = rel.Reltnacodi;
                    entFormInput.idRelBarra = -1;
                    entFormInput.idRelFormula = -1;
                    entFormInput.idCompVegetativo = rel.Reltnaformula;
                    entFormInput.idCompFormula = comp.Codigo;
                    entFormInput.idFuente = comp.Tipo;
                    entFormInput.constante = comp.Constante;

                    modFormulasInput.Add(entFormInput);
                }

                //Descompone la/las formula(s) de cada barra de la relación
                //Identifica los componentes de la formula por tipo
                foreach (dynamic[] det in rel.Detalle)
                {
                    int iBarra = det[0];
                    int iFormula = det[2];

                    listaComponentes = this.DescomponerFormula(iFormula);
                    foreach (Scada.FormulaItem comp in listaComponentes)
                    {
                        DpoMedCompFormula entFormVeg = new DpoMedCompFormula();

                        entFormVeg.idRelacion = rel.Reltnacodi;
                        entFormVeg.idRelBarra = iBarra;
                        entFormVeg.idRelFormula = iFormula;
                        entFormVeg.idCompVegetativo = rel.Reltnaformula;
                        entFormVeg.idCompFormula = comp.Codigo;
                        entFormVeg.idFuente = comp.Tipo;
                        entFormVeg.constante = comp.Constante;

                        modFormulasVeg.Add(entFormVeg);
                    }
                }
            }
            #endregion

            //Agrupa los datos por el tipo de fuente
            List<string> fuenteTNA = new List<string> { 
                ConstantesDpo.FormOrigenTnaIEOD, 
                ConstantesDpo.FormOrigenTnaSCO 
            };

            List<DpoMedCompFormula> ForInput_F_CDispatch = modFormulasInput
                .Where(x => x.idFuente == ConstantesDpo.FormOrigenDespacho)
                .ToList();
            List<DpoMedCompFormula> ForInput_F_Tna = modFormulasInput
                .Where(x => fuenteTNA.Contains(x.idFuente))
                .ToList();
            List<DpoMedCompFormula> ForVeg_F_CDispatch = modFormulasVeg
                .Where(x => x.idFuente == ConstantesDpo.FormOrigenDespacho)
                .ToList();
            List<DpoMedCompFormula> ForVeg_F_Tna = modFormulasVeg
                .Where(x => fuenteTNA.Contains(x.idFuente))
                .ToList();

            return new DpoModRelacionTna()
            {
                ForInputCDispatch = ForInput_F_CDispatch,
                ForInputEstimadorTna = ForInput_F_Tna,
                ForVegCDispatch = ForVeg_F_CDispatch,
                ForVegEstimadorTna = ForVeg_F_Tna,
                Relaciones = listaRel,
            };            
        }

        /// <summary>
        /// Devuleve la demanda vegetativa para el pronóstico de días feriados
        /// </summary>
        /// <param name="idBarras"></param>
        /// <param name="regFecha"></param>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        public List<DpoMedicion96DTO> ObtenerDemandaVegSCO(
            List<int> idBarras, DateTime regFecha,
            string idVersion)
        {
            string strFecha = regFecha.ToString(ConstantesDpo.FormatoFecha);
            string idTipoInfo = ConstantesDpo.MedgrpDemVegetativa.ToString();
            return this.ListDatosMediciongrp(idBarras, idTipoInfo, strFecha, idVersion);
        }

        public object ReprogramaRegistrarVeg(
            List<DpoMedicion96DTO> datosMedicion, int idVersion,
            string regFecha, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);

            List<MePtomedicionDTO> listaPuntos = this.ListaBarrasFormato();

            //Obtiene la lista de datos de la versión si existen
            List<DpoMedicion96DTO> datosRegistrados = this.ListDpoMedicion96PorVersion(
                ConstantesDpo.DpotmeDemVegetativa, ConstantesDpo.DpotdtPunto,
                idVersion, regFecha, regFecha);
            List<int> ids = datosRegistrados
                .Select(x => x.Dpomedcodi)
                .Distinct()
                .ToList();

            //Registra o actualiza los datos
            try
            {
                foreach (DpoMedicion96DTO entity in datosMedicion)
                {
                    entity.Vergrpcodi = idVersion;
                    entity.Dpomedfecha = parseFecha;
                    entity.Dpotmecodi = ConstantesDpo.DpotmeDemVegetativa;
                    entity.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                    entity.Dpomedusumodificacion = nomUsuario;
                    entity.Dpomedfecmodificacion = DateTime.Now;

                    if (ids.Contains(entity.Dpomedcodi))
                        UpdateDpoMedicion96(entity);
                    else
                        SaveDpoMedicion96(entity);
                }

                typeMsg = ConstantesDpo.MsgSuccess;
                dataMsg = "Versión actualizada";
            }
            catch (Exception ex)
            {
                typeMsg = ConstantesDpo.MsgError;
                dataMsg = ex.Message;
            }

            //Recalcula los ajustes por área
            List<MePtomedicionDTO> listaAreas = this.ListaAreaOperativa();
            foreach (MePtomedicionDTO area in listaAreas)
            {
                DpoMedicion96DTO ajusteArea = this.GetByIdDpoMedicion96(
                    area.Areacodi, ConstantesDpo.DpotdtArea,
                    ConstantesDpo.DpotmeAjusteArea.ToString(),
                    idVersion, regFecha);

                decimal[] medArea = UtilDpo.ConvertirMedicionEnArreglo(
                    ConstantesDpo.Itv15min, ajusteArea)
                    .Where((x, index) => index % 2 != 0)
                    .ToArray();

                ReprogramaRegAjusteAreaxBarra(listaPuntos, idVersion,
                    regFecha, area.Areacodi, medArea, 
                    nomUsuario);
            }

            //Recalcula los ajustes por sein
            DpoMedicion96DTO ajusteSein = this.GetByIdDpoMedicion96(
                ConstantesDpo.AreacodiSein, ConstantesDpo.DpotdtArea,
                ConstantesDpo.DpotmeAjusteArea.ToString(),
                idVersion, regFecha);
            decimal[] medSein = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, ajusteSein)
                .Where((x, index) => index % 2 != 0)
                .ToArray();

            ReprogramaRegAjusteSeinxBarra(listaPuntos, idVersion,
                regFecha, medSein, nomUsuario);

            return new { typeMsg, dataMsg };
        }

        public object ReprogramaRegistrarConfigVeg(DpoConfiguracionDTO entidad)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            if (entidad.Dpocngcodi != 0)
            {
                this.UpdateDpoConfiguracion(entidad);
                typeMsg = ConstantesDpo.MsgSuccess;
                dataMsg = "Configuración actualizada";            
            }
            else
            {
                try
                {
                    this.SaveDpoConfiguracion(entidad);
                    typeMsg = ConstantesDpo.MsgSuccess;
                    dataMsg = "Configuración actualizada";
                }
                catch (Exception ex)
                {
                    typeMsg = ConstantesDpo.MsgError;
                    dataMsg = ex.Message;
                }
            }
            
            return new { typeMsg, dataMsg };
        }

        public object ReprogramaDatosUL(int idVersion, 
            string nomVersion, string fecha)
        {
            List<MePtomedicionDTO> puntosMedicion = this.ListaBarrasFormato();

            List<int> idPuntos = puntosMedicion
                .Select(x => x.Ptomedicodi)
                .ToList();

            string fecFin = DateTime.ParseExact(fecha, 
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture)
                .AddDays(1).ToString(ConstantesProdem.FormatoFecha);

            List<string[]> table = new List<string[]>();
            List<string> rowHeader = new List<string>(){ "FECHA HORA / BARRA" };
            List<string> rowFooter = new List<string>() { "ID" };
            rowHeader.AddRange(puntosMedicion.Select(x => x.Ptomedibarranomb).ToList());
            rowFooter.AddRange(idPuntos.Select(x => x.ToString()).ToList());
            table.Add(rowHeader.ToArray());

            int i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                table.Add(new string[rowHeader.Count]);
                i++;
            }
            table.Add(rowFooter.ToArray());

            //.Llenado de la tabla
            //.Llenado de la columna intervalos
            string[] itv = UtilProdem.GenerarIntervalosFecha(
                ConstantesDpo.Itv30min, fecha, fecFin);
            i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                table[i + 1][0] = itv[i];
                i++;
            }
            //.Llenado de las mediciones
            List<DpoMedicion96DTO> datosMedicion = this.ListDpoMedicion96PorVersion(
                ConstantesDpo.DpotmeDemUlibre, ConstantesDpo.DpotdtPunto, 
                idVersion, fecha, fecha);

            i = 1;
            foreach (int idPunto in idPuntos)
            {
                DpoMedicion96DTO entMedicion = datosMedicion
                    .FirstOrDefault(x => x.Dpomedcodi == idPunto) 
                    ?? new DpoMedicion96DTO();
                decimal?[] medicion = UtilDpo.ConvertirMedEnAryNullable(
                    ConstantesDpo.Itv15min, entMedicion);
                for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                {
                    table[j + 1][i] = medicion[(j * 2) + 1].ToString();
                }
                i++;
            }

            //.Creación de mensajes 
            string msgVegtativa = $"• Vegetativa: Tabla mostrada con datos de la versión {nomVersion}";
            string msgUsuLibres = $"• Usuarios libres: Tabla mostrada con datos de la versión {nomVersion}";
            List<DpoActualizacionUL> datosModificados = new List<DpoActualizacionUL>();

            return new {
                msgVegtativa,
                msgUsuLibres,
                datos = table,
                datosModificados,                
            };
        }

        public object ReprogramaActualizarUL(
            int idVersion, string nomVersion, string fecha)
        {
            List<MePtomedicionDTO> puntosMedicion = this.ListaBarrasFormato();

            List<int> idPuntos = puntosMedicion
                .Select(x => x.Ptomedicodi)
                .ToList();
            List<int> idBarras = puntosMedicion
                .Select(x => (int)x.Grupocodi)
                .ToList();

            string fecFin = DateTime.ParseExact(fecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture)
                .AddDays(1).ToString(ConstantesProdem.FormatoFecha);

            
            List<string[]> table = new List<string[]>();
            List<string> rowHeader = new List<string>() { "FECHA HORA / BARRA" };
            List<string> rowFooter = new List<string>() { "ID" };
            rowHeader.AddRange(puntosMedicion.Select(x => x.Ptomedibarranomb).ToList());
            rowFooter.AddRange(idPuntos.Select(x => x.ToString()).ToList());
            table.Add(rowHeader.ToArray());

            int i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                table.Add(new string[rowHeader.Count]);
                i++;
            }
            table.Add(rowFooter.ToArray());

            //.Llenado de la tabla
            //.Llenado de la columna intervalos
            string[] itv = UtilProdem.GenerarIntervalosFecha(
                ConstantesProdem.Itv30min, fecha, fecFin);
            i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                table[i + 1][0] = itv[i];
                i++;
            }
            //.Llenado de las mediciones
            //.Obtiene los datos de la versión
            List<DpoMedicion96DTO> datosMedicion = this.ListDpoMedicion96PorVersion(
                ConstantesDpo.DpotmeDemUlibre, ConstantesDpo.DpotdtPunto,
                idVersion, fecha, fecha);
            //.Obtiene los datos de extranet
            List<DpoMedicion96DTO> datosExtranet = FactorySic
                .GetDpoMedicion96Repository().ObtenerDemRDOPrevExtranet(
                ConstantesDpo.FormatcodiDemPrevRDO, ConstantesDpo.LectcodiDemPrevRDO,
                fecha);

            i = 1;
            List<DpoActualizacionUL> datosModificados = new List<DpoActualizacionUL>();
            for (int e = 0; e < idBarras.Count; e++)
            {
                int idPunto = idPuntos[e];
                int idBarra = idBarras[e];
                
                DpoMedicion96DTO entMedicion = datosMedicion
                    .FirstOrDefault(x => x.Dpomedcodi == idPunto)
                    ?? new DpoMedicion96DTO();
                DpoMedicion96DTO entExtranet = datosExtranet
                    .FirstOrDefault(x => x.Dpomedcodi == idBarra)
                    ?? new DpoMedicion96DTO();

                decimal?[] medicion = UtilDpo.ConvertirMedicionNuloEnArreglo(
                    ConstantesDpo.Itv15min, entMedicion)
                    .Where((x, index) => index % 2 != 0)
                    .ToArray(); ;
                decimal?[] medExtranet = UtilDpo.ConvertirMedicionNuloEnArreglo(
                    ConstantesDpo.Itv15min, entExtranet)
                    .Where((x, index) => index % 2 != 0)
                    .ToArray(); ;

                //Comparación de datos
                if (medExtranet.Sum() != 0)
                {
                    //List<int> intervalos = UtilDpo.CompararArreglosNullPorIntervalos(
                    //    medicion, medExtranet);
                    List<int> intervalos = medExtranet
                        .Select((item, index) => new { item, index })
                        .Where(x => x.item != null)
                        .Select(x => x.index)
                        .ToList();

                    if (intervalos.Count > 0)
                    {
                        foreach (int index in intervalos)
                        {
                            medicion[index] = medExtranet[index];
                        }

                        datosModificados.Add(new DpoActualizacionUL()
                        {
                            id = idPunto,
                            intervalos = intervalos,
                        });
                    }
                     
                }
                
                //Llenado de los datos
                for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                {
                    table[j + 1][i] = medicion[j].ToString();
                }
                i++;
            }

            //.Creación de mensajes 
            string msgVegtativa = $"• Vegetativa: Tabla mostrada con datos de la versión {nomVersion}";
            string msgUsuLibres = "• Usuarios libres: Tabla mostrada con datos de Extranet. ";
            msgUsuLibres += $"Ultima actualización realizada a las {DateTime.Now.ToString("HH:mm")}";
            
            return new
            {
                msgVegtativa,
                msgUsuLibres,
                datos = table,
                datosModificados,
            };
        }
        public object ReprogramaRegistrarUL(
            List<DpoMedicion96DTO> datosMedicion,
            int idVersion, string regFecha, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            DateTime parseFecha = DateTime.ParseExact(regFecha,
                ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);
            
            //Obtiene la lista de datos de la versión si existen
            List<DpoMedicion96DTO> datosRegistrados = this.ListDpoMedicion96PorVersion(
                ConstantesDpo.DpotmeDemUlibre, ConstantesDpo.DpotdtPunto,
                idVersion, regFecha, regFecha);
            List<int> ids = datosRegistrados
                .Select(x => x.Dpomedcodi)
                .Distinct()
                .ToList();

            //Registra o actualiza los datos
            try
            {
                foreach (DpoMedicion96DTO entity in datosMedicion)
                {
                    entity.Vergrpcodi = idVersion;
                    entity.Dpomedfecha = parseFecha;
                    entity.Dpotmecodi = ConstantesDpo.DpotmeDemUlibre;
                    entity.Dpotdtcodi = ConstantesDpo.DpotdtPunto;                    
                    entity.Dpomedusumodificacion = nomUsuario;
                    entity.Dpomedfecmodificacion = DateTime.Now;

                    if (ids.Contains(entity.Dpomedcodi))
                        UpdateDpoMedicion96(entity);
                    else
                        SaveDpoMedicion96(entity);
                }

                typeMsg = ConstantesDpo.MsgSuccess;
                dataMsg = "Versión actualizada";
            }
            catch(Exception ex)
            {
                typeMsg = ConstantesDpo.MsgError;
                dataMsg = ex.Message;
            }
            
            return new { typeMsg, dataMsg };
        }

        public object ReprogramaDatosTotalBarra(
            int idVersion, string nomVersion, string selFecha)
        {
            List<MePtomedicionDTO> puntosMedicion = this.ListaBarrasFormato();

            List<int> idPuntos = puntosMedicion
                .Select(x => x.Ptomedicodi)
                .ToList();

            string fecFin = DateTime.ParseExact(selFecha,
                ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture)
                .AddDays(1).ToString(ConstantesProdem.FormatoFecha);

            List<string[]> table = new List<string[]>();
            List<string> rowHeader = new List<string>() { "FECHA HORA / BARRA" };
            List<string> rowFooter = new List<string>() { "ID" };
            rowHeader.AddRange(puntosMedicion.Select(x => x.Ptomedibarranomb).ToList());
            rowFooter.AddRange(idPuntos.Select(x => x.ToString()).ToList());
            table.Add(rowHeader.ToArray());

            int i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                table.Add(new string[rowHeader.Count]);
                i++;
            }
            table.Add(rowFooter.ToArray());

            //.Llenado de la tabla
            //.Llenado de la columna intervalos
            string[] itv = UtilProdem.GenerarIntervalosFecha(
                ConstantesDpo.Itv30min, selFecha, fecFin);
            i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                table[i + 1][0] = itv[i];
                i++;
            }
            //.Llenado de las mediciones
            List<DpoMedicion96DTO> datosMedicion = this
                .ListDatosDemTotalPorBarraNulleable(ConstantesDpo.DpotdtPunto, 
                idVersion, selFecha);

            i = 1;
            foreach (int idPunto in idPuntos)
            {
                DpoMedicion96DTO entMedicion = datosMedicion
                    .FirstOrDefault(x => x.Dpomedcodi == idPunto)
                    ?? new DpoMedicion96DTO();
                decimal?[] medicion = UtilDpo.ConvertirMedEnAryNullable(
                    ConstantesDpo.Itv15min, entMedicion);
                for (int j = 0; j < ConstantesProdem.Itv30min; j++)
                {
                    table[j + 1][i] = medicion[(j * 2) + 1].ToString();
                }
                i++;
            }

            //.Creación de mensajes 
            string msgVegtativa = $"• Vegetativa: Tabla mostrada con datos de la versión: {nomVersion}";
            string msgUsuLibres = $"• Usuarios libres: Tabla mostrada con datos de la versión: {nomVersion}";
            
            return new
            {
                msgVegtativa,
                msgUsuLibres,
                datos = table,
            };
        }

        public object ReprogramaDatosABarra(
            int idVersion, string nomVersion, int selBarra,
            string selFecha, int idVersionComp,
            string selFechaComp)
        {
            List<object> data = new List<object>();

            string[] aryIntervalos = UtilDpo
                .GenerarIntervalos(ConstantesProdem.Itv30min);

            //Obtiene los datos de comparación
            List<DpoMedicion96DTO> datosVersion = FactorySic
                .GetDpoMedicion96Repository().ObtenerVersionComparacion(
                selFechaComp, idVersionComp);
            DpoMedicion96DTO entVersion = datosVersion
                .FirstOrDefault(x => x.Dpomedcodi == selBarra)
                ?? new DpoMedicion96DTO();
            decimal[] aryVersion = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entVersion)
                .Where((x, index) => index % 2 != 0).ToArray();

            //Obtiene los datos pronosticados de la versión
            DpoMedicion96DTO entPronostico = this.GetByIdDpoMedicion96(
                selBarra, ConstantesDpo.DpotdtPunto, 
                ConstantesDpo.DpotmeDemandaTotalPorBarraNoAjuste,
                idVersion, selFecha);
            decimal[] medPronostico = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entPronostico)
                .Where((x, index) => index % 2 != 0).ToArray();

            //Obtiene los datos de la columna ajuste
            DpoMedicion96DTO entAjuste = this.GetByIdDpoMedicion96(
                selBarra, ConstantesDpo.DpotdtPunto,
                ConstantesDpo.DpotmeAjusteBarra.ToString(),
                idVersion, selFecha);
            decimal[] medAjuste = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entAjuste)
                .Where((x, index) => index % 2 != 0).ToArray();

            //Obtiene los datos de la columna final
            decimal[] medFinal = medPronostico
                .Zip(medAjuste, (a, b) => a + b)
                .ToArray();

            object entity;
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = aryIntervalos,
                htrender = "hora",
                hcrender = "categoria",
            };
            data.Add(entity);

            entity = new
            {
                id = "version",
                label = "Versión",
                data = aryVersion,
                htrender = "normal",
                hcrender = "normal",
            };
            data.Add(entity);

            entity = new
            {
                id = "base",
                label = "Pronóstico(P)",
                data = medPronostico,
                htrender = "normal",
                hcrender = "normal",
            };
            data.Add(entity);

            entity = new
            {
                id = "ajuste",
                label = "Ajuste(A)",
                data = medAjuste,
                htrender = "edit",
                hcrender = "no",
            };
            data.Add(entity);

            entity = new
            {
                id = "final",
                label = "Final(P + A)",
                data = medFinal,
                htrender = "final",
                hcrender = "final",
            };
            data.Add(entity);

            return new { data };
        }

        public object ReprogramaDifABarra(
            int idVersion,  string nomVersion, string selFecha,
            int selIntervalo, int idVersionComp, string selFechaComp)
        {
            List<MePtomedicionDTO> listaBarras = this.ListaBarrasFormato();

            List<DpoBarraDiferencia> dataDifPositiva = new List<DpoBarraDiferencia>();
            List<DpoBarraDiferencia> dataDifNegativa = new List<DpoBarraDiferencia>();

            List<DpoMedicion96DTO> datosVersion = this.GetByVersionDpoMedicion96(
                ConstantesDpo.DpotdtPunto, 
                ConstantesDpo.DpotmeDemandaTotalPorBarra,
                idVersion, selFecha);

            List<DpoMedicion96DTO> datosComparacion = FactorySic
                .GetDpoMedicion96Repository().ObtenerVersionComparacion(
                selFechaComp, idVersionComp);

            foreach (MePtomedicionDTO barra in listaBarras)
            {                
                DpoMedicion96DTO entidadVersion = datosVersion
                    .FirstOrDefault(x => x.Dpomedcodi == barra.Ptomedicodi)
                    ?? new DpoMedicion96DTO();
                DpoMedicion96DTO entidadComparacion = datosComparacion
                    .FirstOrDefault(x => x.Dpomedcodi == barra.Ptomedicodi)
                    ?? new DpoMedicion96DTO();
                var datoVersion = entidadVersion
                    .GetType().GetProperty($"H{(selIntervalo * 2)}")
                    .GetValue(entidadVersion);
                var datoComparacion = entidadComparacion
                    .GetType().GetProperty($"H{(selIntervalo * 2)}")
                    .GetValue(entidadComparacion);

                decimal valorDiferencia = (datoVersion != null) ? (decimal)datoVersion : 0;
                valorDiferencia -= (datoComparacion != null) ? (decimal)datoComparacion : 0;
                valorDiferencia = Math.Round(valorDiferencia, 4);

                if (valorDiferencia == 0) continue;
                if (valorDiferencia > 0)
                {
                    dataDifPositiva.Add(new DpoBarraDiferencia()
                    {
                        id = (int)barra.Grupocodi,
                        nombre = barra.Ptomedibarranomb,
                        diferencia = valorDiferencia,
                    });
                }
                if (valorDiferencia < 0)
                {
                    dataDifNegativa.Add(new DpoBarraDiferencia()
                    {
                        id = (int)barra.Grupocodi,
                        nombre = barra.Ptomedibarranomb,
                        diferencia = valorDiferencia,
                    });
                }
            }

            dataDifPositiva = dataDifPositiva
                .OrderByDescending(x => x.diferencia)
                .ToList();
            dataDifNegativa = dataDifNegativa
                .OrderBy(x => x.diferencia)
                .ToList();

            return new { 
                dataDifPositiva,
                dataDifNegativa,
            };
        }
        public object ReprogramaRegistrarABarra(int idVersion,
            int idBarra, string nomBarra, string selFecha,
            string nomUsuario, decimal[] datos)
        {
            string msg = string.Empty;
            DateTime parseFecha = DateTime.ParseExact(selFecha, 
                ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);
            DpoMedicion96DTO res = this.GetByIdDpoMedicion96(
                idBarra, ConstantesDpo.DpotdtPunto,
                ConstantesDpo.DpotmeAjusteBarra.ToString(),
                idVersion, selFecha);

            try
            {
                msg = $"Se actualizo la barra {nomBarra}";
                if (res.Dpomedcodi == 0)
                {
                    res.Dpomedcodi = idBarra;
                    res.Dpotmecodi = ConstantesDpo.DpotmeAjusteBarra;
                    res.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                    res.Dpomedfecha = parseFecha;
                    res.Vergrpcodi = idVersion;
                    res.Dpomedusucreacion = nomUsuario;
                    res.Dpomedfeccreacion = DateTime.Now;
                    res.Dpomedusumodificacion = nomUsuario;
                    res.Dpomedfecmodificacion = DateTime.Now;

                    int i = 0;
                    while (i < datos.Length)
                    {
                        res.GetType()
                            .GetProperty($"H{(i + 1) * 2}")
                            .SetValue(res, datos[i]);
                        i++;
                    }
                    this.SaveDpoMedicion96(res);
                }
                else
                {                    
                    res.Dpotmecodi = ConstantesDpo.DpotmeAjusteBarra;
                    res.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                    res.Vergrpcodi = idVersion;
                    res.Dpomedusumodificacion = nomUsuario;
                    res.Dpomedfecmodificacion = DateTime.Now;

                    int i = 0;
                    while (i < datos.Length)
                    {                        
                        res.GetType()
                            .GetProperty($"H{(i + 1) * 2}")
                            .SetValue(res, datos[i]);
                        i++;
                    }
                    this.UpdateDpoMedicion96(res);
                }
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }

            return new { msg };
        }

        public object ReprogramaDatosAreaSein(int idVersion, 
            string nomVersion, int idArea, string selFecha,
            int idVersionComp, string selFechaComp)
        {
            List<object> data = new List<object>();

            List<MePtomedicionDTO> listaBarras = this.ListaBarrasFormato();

            List<int> idPuntos = listaBarras
                .Where(x => x.Areacodi == idArea)
                .Select(x => x.Ptomedicodi)
                .Distinct()
                .ToList();
            List<int> idBarras = listaBarras
                .Where(x => x.Areacodi == idArea)
                .Select(x => (int)x.Grupocodi)
                .Distinct()
                .ToList();
            string strPuntos = string.Join(",", idPuntos);
            string strBarras = string.Join(",", idBarras);

            string[] aryIntervalos = UtilDpo
                .GenerarIntervalos(ConstantesProdem.Itv30min);

            //Obtiene los datos de comparación
            DpoMedicion96DTO entVersion = FactorySic
                .GetDpoMedicion96Repository().ObtenerDatosCompAgrupados(
                strBarras, idVersionComp, selFechaComp);
            decimal[] medVersion = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entVersion)
                .Where((x, index) => index % 2 != 0).ToArray();
            
            //Obtiene los datos del pronóstico agrupado por área operativa
            DpoMedicion96DTO entPronostico = this.GetGroupDpoMedicion96(
                strPuntos, ConstantesDpo.DpotdtPunto, 
                ConstantesDpo.DpotmeDemandaTotalArea, 
                idVersion, selFecha);
            decimal[] medPronostico = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entPronostico)
                .Where((x, index) => index % 2 != 0).ToArray();

            //Obtiene los datos del ajuste
            DpoMedicion96DTO entAjuste = this.GetByIdDpoMedicion96(
                idArea, ConstantesDpo.DpotdtArea, 
                ConstantesDpo.DpotmeAjusteArea.ToString(),
                idVersion, selFecha);
            decimal[] medAjuste = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entAjuste)
                .Where((x, index) => index % 2 != 0).ToArray();

            //Obtiene los datos de la columna final
            decimal[] medFinal = medPronostico
                .Zip(medAjuste, (a, b) => a + b)
                .ToArray();

            //Creación de modelo para la presentación
            object entity;
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = aryIntervalos,
                htrender = "hora",
                hcrender = "categoria",
            };
            data.Add(entity);

            entity = new
            {
                id = "version",
                label = $"Versión",
                data = medVersion,
                htrender = "normal",
                hcrender = "normal",
            };
            data.Add(entity);

            entity = new
            {
                id = "base",
                label = "Pronóstico(P)",
                data = medPronostico,
                htrender = "normal",
                hcrender = "normal",
            };
            data.Add(entity);

            entity = new
            {
                id = "ajuste",
                label = "Ajuste(A)",
                data = medAjuste,
                htrender = "edit",
                hcrender = "no",
            };
            data.Add(entity);

            entity = new
            {
                id = "final",
                label = "Final(P + A)",
                data = medFinal,
                htrender = "final",
                hcrender = "final",
            };
            data.Add(entity);

            return new { data };
        }
        public object ReprogramaRegistrarAreaSein(
            int idVersion, string nomVersion, int idArea, string selFecha,
            decimal[] datosMedicion, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            DateTime parseFecha = DateTime.ParseExact(selFecha,
                ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);

            List<MePtomedicionDTO> puntosMedicion = this.ListaBarrasFormato();

            DpoMedicion96DTO res = this.GetByIdDpoMedicion96(
                idArea, ConstantesDpo.DpotdtArea, 
                ConstantesDpo.DpotmeAjusteArea.ToString(),
                idVersion, selFecha);

            this.ReprogramaRegAjusteAreaxBarra(puntosMedicion,
                idVersion, selFecha, idArea, datosMedicion,
                nomUsuario);

            try
            {
                typeMsg = ConstantesDpo.MsgSuccess;
                dataMsg = "Versión actualizada";

                if (res.Dpomedcodi == 0)
                {
                    res.Dpomedcodi = idArea;
                    res.Dpotmecodi = ConstantesDpo.DpotmeAjusteArea;
                    res.Dpotdtcodi = ConstantesDpo.DpotdtArea;
                    res.Dpomedfecha = parseFecha;
                    res.Vergrpcodi = idVersion;
                    res.Dpomedusucreacion = nomUsuario;
                    res.Dpomedfeccreacion = DateTime.Now;
                    res.Dpomedusumodificacion = nomUsuario;
                    res.Dpomedfecmodificacion = DateTime.Now;

                    int i = 0;
                    while (i < datosMedicion.Length)
                    {
                        res.GetType()
                            .GetProperty($"H{(i + 1) * 2}")
                            .SetValue(res, datosMedicion[i]);
                        i++;
                    }
                    this.SaveDpoMedicion96(res);
                }
                else
                {
                    res.Dpotmecodi = ConstantesDpo.DpotmeAjusteArea;
                    res.Dpotdtcodi = ConstantesDpo.DpotdtArea;
                    res.Vergrpcodi = idVersion;
                    res.Dpomedusumodificacion = nomUsuario;
                    res.Dpomedfecmodificacion = DateTime.Now;

                    int i = 0;
                    while (i < datosMedicion.Length)
                    {
                        res.GetType()
                            .GetProperty($"H{(i + 1) * 2}")
                            .SetValue(res, datosMedicion[i]);
                        i++;
                    }
                    this.UpdateDpoMedicion96(res);
                }
            }
            catch (Exception ex)
            {
                typeMsg = ConstantesDpo.MsgError;
                dataMsg = ex.Message;
            }

            return new { typeMsg, dataMsg };
        }

        public object ReprogramaDatosAjusteSein(int idVersion,
            int selYupana, string selFecha, string selFechaComp)
        {
            List<object> data = new List<object>();

            List<MePtomedicionDTO> listaBarras = this.ListaBarrasFormato();

            List<int> ids = listaBarras                
                .Select(x => x.Ptomedicodi)
                .Distinct()
                .ToList();
            string strIds = string.Join(",", ids);

            string[] aryIntervalos = UtilDpo
                .GenerarIntervalos(ConstantesProdem.Itv30min);

            //Demanda Hidro + Termica + RER y No Coes
            //(Demanda total a nivel de generación)
            DpoMedicion96DTO entDemanda = FactorySic
                .GetDpoMedicion96Repository().ObtenerDatosMedicion48(
                    ConstantesDpo.LectcodiDespacho, ConstantesDpo.TipoinfocodiSein,
                    ConstantesDpo.PtomedicodiSein.ToString(),
                    selFechaComp, selFechaComp);
            decimal[] medDemanda = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entDemanda)
                .Where((x, index) => index % 2 != 0).ToArray();

            //Obtenido desde Escenario Yupana
            DpoMedicion96DTO entGeneracion = FactorySic
                .GetDpoMedicion96Repository().ObtenerDatosEscenarioYupana(
                    selYupana, ConstantesDpo.SrestcodiGeneracionTotal,
                    selFecha);
            decimal[] medGeneracion = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entGeneracion)
                .Where((x, index) => index % 2 != 0).ToArray();

            //Perdidas de la red de transmisión
            DpoMedicion96DTO entPerdida = FactorySic
                .GetDpoMedicion96Repository().ObtenerDatosSumEscenarioYupana(
                    selYupana, ConstantesDpo.SrestcodiPeridasTransmision,
                    selFecha);
            decimal[] medPerdida = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entPerdida)
                .Where((x, index) => index % 2 != 0).ToArray();

            //Calculo del factor "F"
            decimal[] medFactor = new decimal[ConstantesDpo.Itv30min];
            for (int i = 0; i < ConstantesDpo.Itv30min; i++)
            {
                decimal diff = medGeneracion[i] - medPerdida[i];
                if (diff <= 0) continue;

                medFactor[i] = medPerdida[i] / diff;
            }

            //Obtiene los datos pronosticados
            //(Demanda a nivel de Demanda [Vegetativa + Industrial])
            DpoMedicion96DTO entPronostico = this.GetGroupDpoMedicion96(
                strIds, ConstantesDpo.DpotdtPunto,
                ConstantesDpo.DpotmeDemandaTotalSein,
                idVersion, selFecha);
            decimal[] medPronostico = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entPronostico)
                .Where((x, index) => index % 2 != 0).ToArray();

            //Columna Perdidas calculadas
            //Aplicacion del factor "F" a la demanda pronosticada
            //(Demanda pronosticada * Factor F)
            decimal[] medPerdidaCalc = medPronostico
                .Zip(medFactor, (a, b) => Math.Round((a * b), 4))
                .ToArray();

            //Pronostico calculado
            decimal[] medPronosticoCalc = medPronostico
                .Zip(medPerdidaCalc, (a, b) => a + b)
                .ToArray();

            //Obtiene los datos del ajuste
            DpoMedicion96DTO entAjuste = this.GetByIdDpoMedicion96(
                ConstantesDpo.AreacodiSein, ConstantesDpo.DpotdtArea,
                ConstantesDpo.DpotmeAjusteArea.ToString(),
                idVersion, selFecha);
            decimal[] medAjuste = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, entAjuste)
                .Where((x, index) => index % 2 != 0).ToArray();

            //Obtiene los datos de la columna final
            decimal[] medFinal = medPronosticoCalc
                .Zip(medAjuste, (a, b) => a + b)
                .ToArray();

            object entity;
            entity = new
            {
                id = "intervalos",
                label = "Hora",
                data = aryIntervalos,
                htrender = "hora",
                hcrender = "categoria",
            };
            data.Add(entity);

            entity = new
            {
                id = "despacho",
                label = "Despacho Ejecutado",
                data = medDemanda,
                htrender = "normal",
                hcrender = "normal",
            };
            data.Add(entity);

            entity = new
            {
                id = "perdida",
                label = "Pérdidas",
                data = medPerdidaCalc,
                htrender = "normal",
                hcrender = "no",
            };
            data.Add(entity);

            entity = new
            {
                id = "totalBarra",
                label = "TotalxBarras*",
                data = medPronostico,
                htrender = "normal",
                hcrender = "normal",
            };
            data.Add(entity);

            entity = new
            {
                id = "base",
                label = "Pronóstico(P)",
                data = medPronosticoCalc,
                htrender = "normal",
                hcrender = "normal",
            };
            data.Add(entity);

            entity = new
            {
                id = "ajuste",
                label = "Ajuste(A)",
                data = medAjuste,
                htrender = "edit",
                hcrender = "no",
            };
            data.Add(entity);

            entity = new
            {
                id = "final",
                label = "Final(P + A)",
                data = medFinal,
                htrender = "final",
                hcrender = "final",
            };
            data.Add(entity);

            return new { data };
        }

        public object ReprogramaRegistrarAjusteSein(
            int idVersion, string nomVersion, string selFecha,
            decimal[] datosMedicion, string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;
            DateTime parseFecha = DateTime.ParseExact(selFecha,
                ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);

            List<MePtomedicionDTO> puntosMedicion = this.ListaBarrasFormato();

            DpoMedicion96DTO res = this.GetByIdDpoMedicion96(
                ConstantesDpo.AreacodiSein, ConstantesDpo.DpotdtArea,
                ConstantesDpo.DpotmeAjusteArea.ToString(),
                idVersion, selFecha);

            this.ReprogramaRegAjusteSeinxBarra(puntosMedicion,
                idVersion, selFecha, datosMedicion,
                nomUsuario);

            try
            {
                typeMsg = ConstantesDpo.MsgSuccess;
                dataMsg = "Versión actualizada";

                if (res.Dpomedcodi == 0)
                {
                    res.Dpomedcodi = ConstantesDpo.AreacodiSein;
                    res.Dpotmecodi = ConstantesDpo.DpotmeAjusteArea;
                    res.Dpotdtcodi = ConstantesDpo.DpotdtArea;
                    res.Dpomedfecha = parseFecha;
                    res.Vergrpcodi = idVersion;
                    res.Dpomedusucreacion = nomUsuario;
                    res.Dpomedfeccreacion = DateTime.Now;
                    res.Dpomedusumodificacion = nomUsuario;
                    res.Dpomedfecmodificacion = DateTime.Now;

                    int i = 0;
                    while (i < datosMedicion.Length)
                    {
                        res.GetType()
                            .GetProperty($"H{(i + 1) * 2}")
                            .SetValue(res, datosMedicion[i]);
                        i++;
                    }
                    this.SaveDpoMedicion96(res);
                }
                else
                {
                    res.Dpotmecodi = ConstantesDpo.DpotmeAjusteArea;
                    res.Dpotdtcodi = ConstantesDpo.DpotdtArea;
                    res.Vergrpcodi = idVersion;
                    res.Dpomedusumodificacion = nomUsuario;
                    res.Dpomedfecmodificacion = DateTime.Now;

                    int i = 0;
                    while (i < datosMedicion.Length)
                    {
                        res.GetType()
                            .GetProperty($"H{(i + 1) * 2}")
                            .SetValue(res, datosMedicion[i]);
                        i++;
                    }
                    this.UpdateDpoMedicion96(res);
                }
            }
            catch (Exception ex)
            {
                typeMsg = ConstantesDpo.MsgError;
                dataMsg = ex.Message;
            }

            return new { typeMsg, dataMsg };
        }
        public string ReprogramaCrearVersion(
            int idModulo, string nomVersion, string fecha,
            string nomUsuario)
        {
            List<MePtomedicionDTO> puntosMedicion = this.ListaBarrasFormato();

            string res = $"Se ha creado la versión: {nomVersion} con fecha: {fecha}";

            if (String.IsNullOrEmpty(nomVersion))
                return "El nombre de la versión es obligatorio...";

            PrnVersiongrpDTO valid = FactorySic.GetPrnVersiongrpReporsitory()
                .GetByNameArea(nomVersion, ConstantesDpo.areaUsuariaDPO);
            if (valid != null)
                return "El nombre de la versión ya existe...";
            
            //Creación de la nueva versión
            PrnVersiongrpDTO entVersion = new PrnVersiongrpDTO()
            {
                Vergrpnomb = nomVersion,
                Vergrpareausuaria = ConstantesDpo.areaUsuariaDPO,
            };

            try
            {
                int nuevaVersion = FactorySic.GetPrnVersiongrpReporsitory()
                    .SaveGetId(entVersion);

                //Registra los primeros datos correspondientes al módulo
                DateTime parseFecha = DateTime.ParseExact(
                    fecha, ConstantesDpo.FormatoFecha,
                    CultureInfo.InvariantCulture);

                List<int> idPuntos = puntosMedicion
                    .Select(x => x.Ptomedicodi)
                    .ToList();
                foreach (int idPunto in idPuntos)
                {
                    DpoMedicion96DTO entVegetativa = new DpoMedicion96DTO()
                    {
                        Dpomedcodi = idPunto,
                        Dpotmecodi = ConstantesDpo.DpotmeDemVegetativa,
                        Dpotdtcodi = ConstantesDpo.DpotdtPunto,
                        Dpomedfecha = parseFecha,
                        Vergrpcodi = nuevaVersion,
                        Dpomedusucreacion = nomUsuario,
                        Dpomedfeccreacion = DateTime.Now,
                        Dpomedusumodificacion = nomUsuario,
                        Dpomedfecmodificacion = DateTime.Now,
                    };
                    DpoMedicion96DTO entUsuLibre = new DpoMedicion96DTO()
                    {
                        Dpomedcodi = idPunto,
                        Dpotmecodi = ConstantesDpo.DpotmeDemUlibre,
                        Dpotdtcodi = ConstantesDpo.DpotdtPunto,
                        Dpomedfecha = parseFecha,
                        Vergrpcodi = nuevaVersion,
                        Dpomedusucreacion = nomUsuario,
                        Dpomedfeccreacion = DateTime.Now,
                        Dpomedusumodificacion = nomUsuario,
                        Dpomedfecmodificacion = DateTime.Now,
                    };
                    FactorySic.GetDpoMedicion96Repository().Save(entVegetativa);
                    FactorySic.GetDpoMedicion96Repository().Save(entUsuLibre);
                }
            }
            catch(Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

        public object ReprogramaImportarVersion(
            int idModulo, int idVersionImportar, string selFechaImportar,
            int idVersion, string selFecha,
            string nomUsuario)
        {
            string typeMsg = string.Empty;
            string dataMsg = string.Empty;

            List<MePtomedicionDTO> puntosMedicion = this.ListaBarrasFormato();

            List<int> idBarras = puntosMedicion
                .Select(x => (int)x.Grupocodi)
                .ToList();
            
            //.Obtención de datos vegetativos
            List<DpoMedicion96DTO> datosImpVegetativa = this.ListDatosMediciongrp(
                idBarras, ConstantesDpo.MedgrpDemVegTotal,
                selFechaImportar, idVersionImportar.ToString());
            //.Obtención de datos usuarios libres
            List<DpoMedicion96DTO> datosImpUsulibre = this.ListDatosMediciongrp(
                idBarras, ConstantesDpo.MedgrpDemIndustrial.ToString(),
                selFechaImportar, idVersionImportar.ToString());

            //.Actualización de datos de la versión
            DateTime parseFecha = DateTime.ParseExact(
                selFecha, ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);

            #region Actualización de datos vegetativos
            List <DpoMedicion96DTO> datosVersionVeg = this.ListDpoMedicion96PorVersion(
                ConstantesDpo.DpotmeDemVegetativa, ConstantesDpo.DpotdtPunto,
                idVersion, selFecha, selFecha);
            try
            {
                foreach (DpoMedicion96DTO entity in datosVersionVeg)
                {
                    DpoMedicion96DTO entImportar = datosImpVegetativa
                        .FirstOrDefault(x => x.Dpomedcodi == entity.Dpomedcodi)
                        ?? new DpoMedicion96DTO();

                    if (entImportar.Dpomedcodi == 0) continue;

                    entity.Vergrpcodi = idVersion;
                    entity.Dpomedfecha = parseFecha;
                    entity.Dpotmecodi = ConstantesDpo.DpotmeDemVegetativa;
                    entity.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                    entity.Dpomedusumodificacion = nomUsuario;
                    entity.Dpomedfecmodificacion = DateTime.Now;

                    int i = 1;
                    while (i <= ConstantesDpo.Itv15min)
                    {
                        var valid = entImportar.GetType()
                            .GetProperty($"H{i}")
                            .GetValue(entImportar);
                        decimal valor = (valid != null)
                            ? (decimal)valid : 0;

                        entity.GetType().GetProperty($"H{i}")
                            .SetValue(entity, valor);
                        i++;
                    }

                    this.UpdateDpoMedicion96(entity);
                }
            }
            catch (Exception ex)
            {
                typeMsg = ConstantesDpo.MsgError;
                dataMsg = ex.Message;
            }
            #endregion

            #region Actualización de datos usuarios libres
            List<DpoMedicion96DTO> datosVersionUL = this.ListDpoMedicion96PorVersion(
                ConstantesDpo.DpotmeDemUlibre, ConstantesDpo.DpotdtPunto,
                idVersion, selFecha, selFecha);
            try
            {
                foreach (DpoMedicion96DTO entity in datosVersionUL)
                {
                    DpoMedicion96DTO entImportar = datosImpUsulibre
                        .FirstOrDefault(x => x.Dpomedcodi == entity.Dpomedcodi)
                        ?? new DpoMedicion96DTO();

                    if (entImportar.Dpomedcodi == 0) continue;

                    entity.Vergrpcodi = idVersion;
                    entity.Dpomedfecha = parseFecha;
                    entity.Dpotmecodi = ConstantesDpo.DpotmeDemUlibre;
                    entity.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                    entity.Dpomedusumodificacion = nomUsuario;
                    entity.Dpomedfecmodificacion = DateTime.Now;

                    int i = 1;
                    while (i <= ConstantesDpo.Itv15min)
                    {
                        var valid = entImportar.GetType()
                            .GetProperty($"H{i}")
                            .GetValue(entImportar);
                        decimal valor = (valid != null)
                            ? (decimal)valid : 0;

                        entity.GetType().GetProperty($"H{i}")
                            .SetValue(entity, valor);

                        i++;
                    }

                    this.UpdateDpoMedicion96(entity);
                }
            }
            catch (Exception ex)
            {
                typeMsg = ConstantesDpo.MsgError;
                dataMsg = ex.Message;
            }
            #endregion

            //.Creación de mensajes 
            string msgVegtativa = "• Vegetativa: No hay datos";
            string msgUsuLibres = $"• Usuarios libres: Tabla mostrada con datos de la versión";

            return new
            {
                msgVegtativa,
                msgUsuLibres,
            };
        }

        public void ReprogramaRegAjusteAreaxBarra(
            List<MePtomedicionDTO> puntosMedicion, int idVersion,
            string selFecha, int idArea, decimal[] medAjuste,
            string nomUsuario)
        {
            //if (medAjuste.Sum() == 0) return;

            DateTime parseFecha = DateTime.ParseExact(
                selFecha, ConstantesDpo.FormatoFecha, 
                CultureInfo.InvariantCulture);

            List<MePtomedicionDTO> puntosArea = puntosMedicion
                .Where(x => x.Areacodi == idArea)
                .ToList();

            List<int> idPuntos = puntosArea
                .Select(x => x.Ptomedicodi)
                .ToList();

            string strPuntos = (idPuntos.Count != 0)
                ? string.Join(", ", idPuntos.Distinct().ToList())
                : "-1";

            //Obtiene la demanda vegetativa total del área
            DpoMedicion96DTO datoArea = this.GetGroupDpoMedicion96(
                strPuntos, ConstantesDpo.DpotdtPunto,
                ConstantesDpo.DpotmeDemVegetativa.ToString(),
                idVersion, selFecha);
            decimal[] medArea = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, datoArea)
                .Where((x, index) => index % 2 != 0)
                .ToArray();

            if (medArea.Sum() == 0) return;
            
            List<DpoMedicion96DTO> datosMedicion = this.ListDpoMedicion96PorVersion(
                ConstantesDpo.DpotmeDemVegetativa, ConstantesDpo.DpotdtPunto,
                idVersion, selFecha, selFecha);

            foreach (int idPunto in idPuntos)
            {
                DpoMedicion96DTO datoPunto = datosMedicion
                    .FirstOrDefault(x => x.Dpomedcodi == idPunto)
                    ?? new DpoMedicion96DTO();
                decimal[] medPunto = UtilDpo.ConvertirMedicionEnArreglo(
                    ConstantesDpo.Itv15min, datoPunto)
                    .Where((x, index) => index % 2 != 0)
                    .ToArray();

                if (medPunto.Sum() == 0) continue;

                //Obtiene el factor proporcional al componente vegetativo
                decimal[] medFactor = new decimal[ConstantesDpo.Itv30min];
                for (int i = 0; i < ConstantesDpo.Itv30min; i++)
                {
                    if (medArea[i] == 0) continue;
                    if (medPunto[i] == 0) continue;

                    medFactor[i] = medPunto[i] / medArea[i];
                    medFactor[i] = Math.Round(medFactor[i], 4);
                }

                //Registro del ajuste proporcional de la barra
                DpoMedicion96DTO res = this.GetByIdDpoMedicion96(
                    idPunto, ConstantesDpo.DpotdtPunto,
                    ConstantesDpo.DpotmeAjusteArea.ToString(),
                    idVersion, selFecha);

                try
                {                    
                    if (res.Dpomedcodi == 0)
                    {
                        res.Dpomedcodi = idPunto;
                        res.Dpotmecodi = ConstantesDpo.DpotmeAjusteArea;
                        res.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                        res.Dpomedfecha = parseFecha;
                        res.Vergrpcodi = idVersion;
                        res.Dpomedusucreacion = nomUsuario;
                        res.Dpomedfeccreacion = DateTime.Now;
                        res.Dpomedusumodificacion = nomUsuario;
                        res.Dpomedfecmodificacion = DateTime.Now;

                        int i = 0;
                        while (i < medAjuste.Length)
                        {
                            decimal valor = medAjuste[i] * medFactor[i];
                            valor = Math.Round(valor, 4);

                            res.GetType()
                                .GetProperty($"H{(i + 1) * 2}")
                                .SetValue(res, valor);
                            i++;
                        }
                        this.SaveDpoMedicion96(res);
                    }
                    else
                    {                        
                        res.Dpotmecodi = ConstantesDpo.DpotmeAjusteArea;
                        res.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                        res.Vergrpcodi = idVersion;
                        res.Dpomedusumodificacion = nomUsuario;
                        res.Dpomedfecmodificacion = DateTime.Now;

                        int i = 0;
                        while (i < medAjuste.Length)
                        {
                            decimal valor = medAjuste[i] * medFactor[i];
                            res.GetType()
                                .GetProperty($"H{(i + 1) * 2}")
                                .SetValue(res, valor);
                            i++;
                        }
                        this.UpdateDpoMedicion96(res);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public void ReprogramaRegAjusteSeinxBarra(
            List<MePtomedicionDTO> puntosMedicion, int idVersion,
            string selFecha, decimal[] medAjuste, string nomUsuario)
        {
            //if (medAjuste.Sum() == 0) return;

            DateTime parseFecha = DateTime.ParseExact(
                selFecha, ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);

            List<int> idPuntos = puntosMedicion
                .Select(x => x.Ptomedicodi)
                .ToList();

            string strPuntos = (idPuntos.Count != 0)
                ? string.Join(", ", idPuntos.Distinct().ToList())
                : "-1";

            //Obtiene la demanda vegetativa total
            DpoMedicion96DTO datoSein = this.GetGroupDpoMedicion96(
                strPuntos, ConstantesDpo.DpotdtPunto,
                ConstantesDpo.DpotmeDemVegetativa.ToString(),
                idVersion, selFecha);
            decimal[] medSein = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, datoSein)
                .Where((x, index) => index % 2 != 0)
                .ToArray();

            if (medSein.Sum() == 0) return;

            List<DpoMedicion96DTO> datosMedicion = this.ListDpoMedicion96PorVersion(
                ConstantesDpo.DpotmeDemVegetativa, ConstantesDpo.DpotdtPunto,
                idVersion, selFecha, selFecha);

            foreach (int idPunto in idPuntos)
            {
                DpoMedicion96DTO datoPunto = datosMedicion
                    .FirstOrDefault(x => x.Dpomedcodi == idPunto)
                    ?? new DpoMedicion96DTO();
                decimal[] medPunto = UtilDpo.ConvertirMedicionEnArreglo(
                    ConstantesDpo.Itv15min, datoPunto)
                    .Where((x, index) => index % 2 != 0)
                    .ToArray();

                if (medPunto.Sum() == 0) continue;

                //Obtiene el factor proporcional al componente vegetativo
                decimal[] medFactor = new decimal[ConstantesDpo.Itv30min];
                for (int i = 0; i < ConstantesDpo.Itv30min; i++)
                {
                    if (medSein[i] == 0) continue;
                    if (medPunto[i] == 0) continue;

                    medFactor[i] = medPunto[i] / medSein[i];
                    medFactor[i] = Math.Round(medFactor[i], 4);
                }

                //Registro del ajuste proporcional de la barra
                DpoMedicion96DTO res = this.GetByIdDpoMedicion96(
                    idPunto, ConstantesDpo.DpotdtPunto,
                    ConstantesDpo.DpotmeAjusteSein.ToString(),
                    idVersion, selFecha);

                try
                {
                    if (res.Dpomedcodi == 0)
                    {
                        res.Dpomedcodi = idPunto;
                        res.Dpotmecodi = ConstantesDpo.DpotmeAjusteSein;
                        res.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                        res.Dpomedfecha = parseFecha;
                        res.Vergrpcodi = idVersion;
                        res.Dpomedusucreacion = nomUsuario;
                        res.Dpomedfeccreacion = DateTime.Now;
                        res.Dpomedusumodificacion = nomUsuario;
                        res.Dpomedfecmodificacion = DateTime.Now;

                        int i = 0;
                        while (i < medAjuste.Length)
                        {
                            decimal valor = medAjuste[i] * medFactor[i];
                            valor = Math.Round(valor, 4);

                            res.GetType()
                                .GetProperty($"H{(i + 1) * 2}")
                                .SetValue(res, valor);
                            i++;
                        }
                        this.SaveDpoMedicion96(res);
                    }
                    else
                    {
                        res.Dpotmecodi = ConstantesDpo.DpotmeAjusteSein;
                        res.Dpotdtcodi = ConstantesDpo.DpotdtPunto;
                        res.Vergrpcodi = idVersion;
                        res.Dpomedusumodificacion = nomUsuario;
                        res.Dpomedfecmodificacion = DateTime.Now;

                        int i = 0;
                        while (i < medAjuste.Length)
                        {
                            decimal valor = medAjuste[i] * medFactor[i];
                            res.GetType()
                                .GetProperty($"H{(i + 1) * 2}")
                                .SetValue(res, valor);
                            i++;
                        }
                        this.UpdateDpoMedicion96(res);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public List<PrnVersiongrpDTO> VersiongrpListByAreaFecha(
            string idArea, string fecIni,
            string fecFin)
        {
            return FactorySic.GetPrnVersiongrpReporsitory()
                .ListVersionByAreaFecha(idArea,
                fecIni, fecFin);
        }
        public List<MePtomedicionDTO> ListaAreaOperativa()
        {
            return new List<MePtomedicionDTO>()
            {
                new MePtomedicionDTO() {
                  Ptomedicodi = ConstantesProdem.PtomedicodiASur,
                  Ptomedidesc = "Sur",
                  Areacodi = ConstantesProdem.AreacodiASur,
                },
                new MePtomedicionDTO() {
                  Ptomedicodi = ConstantesProdem.PtomedicodiANorte,
                  Ptomedidesc = "Norte",
                  Areacodi = ConstantesProdem.AreacodiANorte,
                },
                new MePtomedicionDTO() {
                  Ptomedicodi = ConstantesProdem.PtomedicodiASierraCentro,
                  Ptomedidesc = "Sierra Centro",
                  Areacodi = ConstantesProdem.AreacodiASierraCentro,
                },
                new MePtomedicionDTO() {
                  Ptomedicodi = ConstantesProdem.PtomedicodiACentro,
                  Ptomedidesc = "Centro",
                  Areacodi = ConstantesProdem.AreacodiACentro,
                },
            };
        }
        public List<DpoMedicion96DTO> ObtenerDemVegModeloTNA(
            DateTime regFecha)
        {
            List<DpoMedicion96DTO> entidades = new List<DpoMedicion96DTO>();

            //Obtiene las relaciones registradas en el módulo de Barras CP-Tna
            //Diferencia los registros "anillo" y "radial"
            List<PrnRelacionTnaDTO> listaRelaciones = this.ListaRelacionTna();
            //Obtiene la demanda Vegetativa de las barras CP desde el modelo TNA
            List<DpoMedicion96DTO> dataVegetativa = new List<DpoMedicion96DTO>();
            foreach (PrnRelacionTnaDTO rRelacion in listaRelaciones)
            {
                //Componente vegetativo de toda la relación
                bool esAnillo = (rRelacion.Detalle.Count > 1) ? true : false;
                decimal[] componenteVegetativo = this.ObtenerDemandaTnaPorFormula(
                    rRelacion.Reltnaformula, regFecha);

                //Obtiene el factor de aporte de cada barra CP perteneciente a la relación
                List<dynamic[]> dataBarrasCPRelacion = new List<dynamic[]>();
                List<decimal[]> vegetativaPorBarraCP = new List<decimal[]>();
                foreach (dynamic[] d in rRelacion.Detalle)
                {
                    int iBarra = d[0];
                    int iFormula = d[2];
                    decimal[] componenteVegetativoBarraCP = this.ObtenerDemandaTnaPorFormula(
                        iFormula, regFecha);
                    vegetativaPorBarraCP.Add(componenteVegetativoBarraCP);
                    dataBarrasCPRelacion.Add(new dynamic[] { iBarra, componenteVegetativoBarraCP });
                }

                foreach (dynamic[] d in dataBarrasCPRelacion)
                {
                    DpoMedicion96DTO entidadVegetativa = new DpoMedicion96DTO
                    {
                        Dpomedcodi = d[0],
                        Dpomedfecha = regFecha
                    };
                    decimal[] factorAporte = (esAnillo)
                        ? UtilProdem.ObtenerFactorAporte(d[1], vegetativaPorBarraCP)
                        : Enumerable.Repeat((decimal)1, ConstantesProdem.Itv30min).ToArray();

                    int j = 0;
                    while (j < ConstantesProdem.Itv30min)
                    {
                        //Dem.Veg.Pronosticada.BarraCP = Dem.Veg.Pronosticada.Anillo * FactorAporte
                        entidadVegetativa.GetType()
                            .GetProperty($"H{(j + 1) * 2}")
                            .SetValue(entidadVegetativa,
                            Math.Round((componenteVegetativo[j] * factorAporte[j]), 4));
                        j++;
                    }
                    dataVegetativa.Add(entidadVegetativa);
                }
            }

            //Agrupa los diferentes componentes de cada barra CP
            List<int> barrasCP = dataVegetativa
                .Select(x => x.Dpomedcodi)
                .Distinct()
                .ToList();
            foreach (int barraCP in barrasCP)
            {
                DpoMedicion96DTO entidad = new DpoMedicion96DTO()
                {
                    Dpomedcodi = barraCP,
                    Dpomedfecha = regFecha
                };
                List<DpoMedicion96DTO> demandaPorBarra = dataVegetativa
                    .Where(x => x.Dpomedcodi == barraCP)
                    .ToList();
                decimal[] finalDemanda = new decimal[ConstantesProdem.Itv30min];
                foreach (DpoMedicion96DTO d in demandaPorBarra)
                {
                    decimal[] arrayDemanda = UtilProdem
                        .ConvertirMedicionEnArreglo(ConstantesProdem.Itv15min, d)
                        .Where((x, index) => index % 2 != 0)
                        .ToArray();
                    finalDemanda = finalDemanda
                        .Zip(arrayDemanda, (a, b) => a + b)
                        .ToArray();
                }
                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    entidad.GetType()
                        .GetProperty($"H{(i + 1) * 2}")
                        .SetValue(entidad, finalDemanda[i]);
                    i++;
                }
                entidades.Add(entidad);
            }

            return entidades;
        }
        public List<PrnRelacionTnaDTO> ListaRelacionTna()
        {
            List<PrnRelacionTnaDTO> listaEntidades = this.ListPrnRelacionTna();
            List<PrnRelacionTnaDTO> listaDetalle = this.ListPrnRelacionTnaDetalle();

            List<PrnRelacionTnaDTO> listaRelacionados;
            foreach (PrnRelacionTnaDTO entidad in listaEntidades)
            {
                entidad.Detalle = new List<dynamic[]>();
                listaRelacionados = new List<PrnRelacionTnaDTO>();
                listaRelacionados = listaDetalle
                    .Where(e => e.Reltnacodi == entidad.Reltnacodi)
                    .ToList();
                foreach (PrnRelacionTnaDTO e in listaRelacionados)
                {
                    entidad.Detalle.Add(new dynamic[] {
                        e.Barracodi,
                        e.Barranom,
                        e.Reltnadetformula,
                        e.Formulanomb
                    });
                }
            }

            return listaEntidades;
        }
        public List<PrnRelacionTnaDTO> ListPrnRelacionTna()
        {
            return FactorySic.GetPrnRelacionTnaRepository().List();
        }
        public List<PrnRelacionTnaDTO> ListPrnRelacionTnaDetalle()
        {
            return FactorySic.GetPrnRelacionTnaRepository().ListRelacionTnaDetalle();
        }        
        public List<ScadaDTO> ObtenerDemandaTnaxMin(string idPunto,
            DateTime fecIni, DateTime fecFin, string fuente)
        {
            List<ScadaDTO> entities = new List<ScadaDTO>();
            string nomTabla = ConstantesDpo.tablaEstimadorRaw
                + fecIni.ToString(ConstantesDpo.FormatoAnioMes);

            List<DpoEstimadorRawDTO> datosMedicion = FactorySic
                .GetDpoEstimadorRawRepository().ObtenerDemActivaPorDia(
                nomTabla, ConstantesDpo.DporawfuenteIeod,
                idPunto, ConstantesProdem.ConsPotActivaTna, 
                fecIni.ToString(ConstantesDpo.FormatoFecha));
            List<DpoMedicion1440> datos1440 = UtilDpo.DarFormato1440(
                datosMedicion, fecIni, fecFin);

            List<int> idPuntos = datos1440
                .Select(x => x.Ptomedicodi)
                .Distinct()
                .ToList();

            foreach (int ptoMed in idPuntos)
            {
                DpoMedicion1440 dato1440 = datos1440
                    .FirstOrDefault(x => x.Ptomedicodi == ptoMed)
                    ?? new DpoMedicion1440();

                if (dato1440.Ptomedicodi == 0) continue;

                ScadaDTO entidad = new ScadaDTO
                {
                    CANALCODI = -1,
                    PTOMEDICODI = ptoMed,
                    MEDIFECHA = dato1440.Medifecha,
                    FUENTE = fuente
                };

                int i = 1;
                while (i <= ConstantesDpo.Itv30min)
                {
                    int index = i * 30 - 1;
                    decimal valor = dato1440.Medicion[index];

                    entidad.GetType().GetProperty($"H{i * 2}")
                        .SetValue(entidad, valor);
                    i++;
                }

                entities.Add(entidad);
            }

            return entities;
        }

        public List<ScadaDTO> ObtenerDemandaSicli(string idPunto, 
            DateTime fecIni, DateTime fecFin, string fuente)
        {
            string strFecIni = fecIni.ToString(
                ConstantesDpo.FormatoFecha);
            string strFecFin = fecFin.ToString(
                ConstantesDpo.FormatoFecha);

            List<IioTabla04DTO> datosMedicion = this.ListMedidorDemandaSicli(
                idPunto, strFecIni, strFecFin, -1);

            List<ScadaDTO> entities = new List<ScadaDTO>();
            foreach (IioTabla04DTO datoMedicion in datosMedicion)
            {
                ScadaDTO entity = new ScadaDTO()
                {
                    CANALCODI = -1,
                    PTOMEDICODI = datoMedicion.Ptomedicodi,
                    MEDIFECHA = datoMedicion.FechaMedicion,
                    FUENTE = fuente
                };

                int i = 1;
                while (i <= ConstantesDpo.Itv30min)
                {
                    decimal valor = (decimal)datoMedicion
                        .GetType().GetProperty($"H{i * 2}")
                        .GetValue(datoMedicion);

                    entity.GetType().GetProperty($"H{i * 2}")
                        .SetValue(entity, valor);
                    i++;
                }

                entities.Add(entity);
            }

            return entities;
        }
        public List<ScadaDTO> ObtenerDemandaSirpit(string idPunto, 
            DateTime fecIni, DateTime fecFin, string fuente)
        {
            string strFecIni = fecIni.ToString(
                ConstantesDpo.FormatoFecha);
            string strFecFin = fecFin.ToString(
                ConstantesDpo.FormatoFecha);

            List<DpoDatos96DTO> datosMedicion = FactorySic
                .GetDpoDatos96Repository()
                .ObtenerDemandaSirpit(idPunto, strFecIni);

            List<ScadaDTO> entities = new List<ScadaDTO>();
            foreach (DpoDatos96DTO datoMedicion in datosMedicion)
            {
                ScadaDTO entity = new ScadaDTO()
                {
                    CANALCODI = -1,
                    PTOMEDICODI = datoMedicion.Dpotnfcodi,
                    MEDIFECHA = datoMedicion.Dpodatfecha,
                    FUENTE = fuente
                };

                int i = 1;
                while (i <= ConstantesDpo.Itv30min)
                {
                    decimal valor = (decimal)datoMedicion
                        .GetType().GetProperty($"H{i * 2}")
                        .GetValue(datoMedicion);

                    entity.GetType().GetProperty($"H{i * 2}")
                        .SetValue(entity, valor);
                    i++;
                }

                entities.Add(entity);
            }

            return entities;
        }
        public List<ScadaDTO> ObtenerDemandaSco(string idPunto,
            DateTime fecIni, DateTime fecFin, string fuente)
        {
            string strFecIni = fecIni.ToString(
                ConstantesDpo.FormatoFecha);
            string strFecFin = fecFin.ToString(
                ConstantesDpo.FormatoFecha);

            List<DpoDemandaScoDTO> datosMedicion = FactorySic
                .GetDpoDemandaScoRepository()
                .ObtenerDemandaSco(idPunto, 
                ConstantesProdem.ConsPotActivaTna, strFecIni);

            List<ScadaDTO> entities = new List<ScadaDTO>();
            foreach (DpoDemandaScoDTO datoMedicion in datosMedicion)
            {
                ScadaDTO entity = new ScadaDTO()
                {
                    CANALCODI = -1,
                    PTOMEDICODI = datoMedicion.Ptomedicodi,
                    MEDIFECHA = datoMedicion.Medifecha,
                    FUENTE = fuente
                };

                int i = 1;
                while (i <= ConstantesDpo.Itv30min)
                {
                    decimal valor = (decimal)datoMedicion
                        .GetType().GetProperty($"H{i * 2}")
                        .GetValue(datoMedicion);

                    entity.GetType().GetProperty($"H{i * 2}")
                        .SetValue(entity, valor);
                    i++;
                }

                entities.Add(entity);
            }

            return entities;
        }

        public decimal[] ObtenerDemandaTnaPorFormula(
            int idCalculado, DateTime medifecha)
        {
            string nomTabla = ConstantesDpo.tablaEstimadorRaw
                + medifecha.ToString(ConstantesDpo.FormatoAnioMes);

            List<string> origenTna = new List<string> {
                ConstantesProdem.OrigenTnaIeod,
                ConstantesProdem.OrigenTnaSco
            };

            Scada.PerfilScadaServicio servicio = new Scada.PerfilScadaServicio();
            List<Scada.FormulaItem> listaFormulas = new List<Scada.FormulaItem>();
            MePerfilRuleDTO entFormula = servicio.GetByIdMePerfilRule(idCalculado);

            servicio.DescomponerFormulaRecursivo(entFormula.Prruformula, listaFormulas);

            List<int> idPuntos = listaFormulas
                .Where(x => origenTna.Contains(x.Tipo))
                .Select(x => x.Codigo)
                .ToList();
            List<int> idDespacho = listaFormulas
                .Where(x => x.Tipo == ConstantesDpo.FormOrigenDespacho)
                .Select(x => x.Codigo)
                .ToList();

            string strPuntos = (idPuntos.Count != 0)
                ? string.Join(", ", idPuntos.Distinct().ToList())
                : "-1";
            string strDespacho = (idDespacho.Count != 0)
                ? string.Join(", ", idDespacho.Distinct().ToList())
                : "-1";

            string strFecha = medifecha.ToString(ConstantesDpo.FormatoFecha);
            List<DpoEstimadorRawDTO> datosMedicion = FactorySic
                .GetDpoEstimadorRawRepository().ObtenerDemActivaPorDia(
                nomTabla, ConstantesDpo.DporawfuenteIeod,
                strPuntos, ConstantesProdem.ConsPotActivaTna,
                strFecha);

            List<DpoMedicion1440> datos1440 = UtilDpo.DarFormato1440(
                datosMedicion, medifecha, medifecha);

            decimal[] total1440 = new decimal[ConstantesDpo.Itv1min];
            foreach (DpoMedicion1440 dato1440 in datos1440)
            {
                total1440 = total1440
                    .Zip(dato1440.Medicion, (a, b) => a + b)
                    .ToArray();
            }

            decimal[] res = new decimal[ConstantesDpo.Itv30min];
            int i = 0;
            while (i < ConstantesDpo.Itv30min)
            {
                res[i] = total1440[((i + 1) * 30) - 1];
                i++;
            }

            //Obtiene y Suma la información del despacho
            DpoMedicion96DTO medDespacho = FactorySic
                .GetDpoMedicion96Repository().ObtenerDatosFormulaM48(
                ConstantesDpo.LectcodiDespacho, ConstantesDpo.TipoinfocodiDespacho,
                strDespacho, strFecha, strFecha);
            decimal[] totalDespacho = UtilDpo.ConvertirMedicionEnArreglo(
                ConstantesDpo.Itv15min, medDespacho)
                .Where((x, index) => index % 2 != 0)
                .ToArray();

            res = res.Zip(totalDespacho, (a, b) => a + b).ToArray();

            return res;
        }

        public List<CpTopologiaDTO> ObtenerEscenariosYupana(
            string selFecha)
        {
            DateTime parseFecha = DateTime.ParseExact(
                selFecha, ConstantesDpo.FormatoFecha,
                CultureInfo.InvariantCulture);
            return FactorySic.GetCpTopologiaRepository()
                .ObtenerEscenariosPorDiaConsulta(parseFecha, 0);
        }

        /// <summary>
        /// Devuelve los componenetes de la fórmula parámetro
        /// </summary>
        /// <param name="idFormula">Identificador de la formula</param>
        /// <returns></returns>
        public List<Scada.FormulaItem> DescomponerFormula(int idFormula)
        {
            Scada.PerfilScadaServicio servicio = new Scada.PerfilScadaServicio();
            List<Scada.FormulaItem> listaFormulas = new List<Scada.FormulaItem>();
            MePerfilRuleDTO entFormula = servicio.GetByIdMePerfilRule(idFormula);
            servicio.DescomponerFormulaRecursivo(entFormula.Prruformula, listaFormulas);

            return listaFormulas;
        }
        #endregion

        #region 2da Iteracción RQ009 - Métodos TablaS DPO_CASO, DPO_CASO_DETALLE y DPO_FUNCION

        /// <summary>
        /// Inserta un registro del caso de configuracion de la tablas DPO_CASO, DPO_CASO_DETALLE
        /// </summary>
        /// <param name="entityCaso"></param>
        /// <param name="entitysCasoDetalle"></param>
        /// <returns></returns>
        public int SaveDpoCasoConfiguracion(DpoCasoDTO entityCaso, List<DpoCasoDetalleDTO> entitysCasoDetalle)
        {
            try
            {
                int idCaso = SaveDpoCaso(entityCaso);

                foreach (DpoCasoDetalleDTO entityCasoDetalle in entitysCasoDetalle)
                {
                    entityCasoDetalle.Dpocsocodi = idCaso;
                    SaveDpoCasoDetalle(entityCasoDetalle);
                }

                return idCaso;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro del caso de configuracion de la tablas DPO_CASO, DPO_CASO_DETALLE
        /// </summary>
        /// <param name="entityCaso"></param>
        /// <param name="entitysCasoDetalle"></param>
        /// <param name="idCaso"></param>
        /// <returns></returns>
        public void UpdateDpoCasoConfiguracion(int idCaso, DpoCasoDTO entityCaso, List<DpoCasoDetalleDTO> entitysCasoDetalle)
        {
            try
            {
                DeleteDpoDetalleCasoByIdCaso(idCaso);

                UpdateDpoCaso(entityCaso);

                foreach (DpoCasoDetalleDTO entityCasoDetalle in entitysCasoDetalle)
                {
                    entityCasoDetalle.Dpocsocodi = idCaso;
                    SaveDpoCasoDetalle(entityCasoDetalle);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Elimina el registro del caso de configuracion de la tablas DPO_CASO, DPO_CASO_DETALLE por id de caso
        /// </summary>
        /// <param name="idCaso"></param>
        /// <returns></returns>
        public void DeleteCasoConfiguracion(int idCaso)
        {
            try
            {
                DeleteDpoDetalleCasoByIdCaso(idCaso);

                DeleteDpoCaso(idCaso);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Copiar un caso de configuracion de la tablas DPO_CASO, DPO_CASO_DETALLE por id de caso
        /// </summary>
        /// <param name="entityCaso"></param>
        /// <param name="entitysCasoDetalle"></param>
        /// <returns></returns>
        public void CopyCasoConfiguracion(DpoCasoDTO entityCaso, List<DpoCasoDetalleDTO> entitysCasoDetalle)
        {
            try
            {
                int idCaso = CopiarDpoCaso(entityCaso);

                foreach (DpoCasoDetalleDTO entityCasoDetalle in entitysCasoDetalle)
                {
                    entityCasoDetalle.Dpocsocodi = idCaso;
                    SaveDpoCasoDetalle(entityCasoDetalle);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        #region Métodos Tabla DPO_CASO
        /// <summary>
        /// Inserta un registro de la tabla DPO_CASO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int SaveDpoCaso(DpoCasoDTO entity)
        {
            try
            {
                return FactorySic.GetDpoCasoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DPO_CASO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void UpdateDpoCaso(DpoCasoDTO entity)
        {
            FactorySic.GetDpoCasoRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla DPO_CASO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteDpoCaso(int id)
        {
            FactorySic.GetDpoCasoRepository().Delete(id);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DPO_CASO
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DpoCasoDTO</returns>
        public DpoCasoDTO GetByIdDpoCaso(int id)
        {
            return FactorySic.GetDpoCasoRepository().GetById(id);
        }

        /// <summary>
        /// Listar registros de DPO_CASO
        /// </summary>
        /// <returns>List DpoCasoDTO </returns>
        public List<DpoCasoDTO> ListDpoCaso()
        {
            return FactorySic.GetDpoCasoRepository().List();
        }

        /// <summary>
        /// Copiar un registro de la tabla DPO_CASO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int CopiarDpoCaso(DpoCasoDTO entity)
        {
            return FactorySic.GetDpoCasoRepository().Save(entity);
        }

        /// <summary>
        /// Combo Filtro Nombre casos
        /// </summary>
        /// <returns>List Nombre casos </returns>
        public List<DpoNombreCasoDTO> ListDpoNombreCasos()
        {
            return FactorySic.GetDpoCasoRepository().ListNombreCasos();
        }

        /// <summary>
        /// Combo Filtro Usuario
        /// </summary>
        /// <returns>List Usuarios </returns>
        public List<DpoUsuarioDTO> ListDpoUsuarios()
        {
            return FactorySic.GetDpoCasoRepository().ListUsuarios();
        }

        /// <summary>
        /// Filtrar registros de DPO_CASO
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="areaOperativa"></param>
        /// <param name="usuario"></param>
        /// <returns>List DpoCasoDTO</returns>
        public List<DpoCasoDTO> FilterDpoCaso(string nombre, string areaOperativa, string usuario)
        {
            return FactorySic.GetDpoCasoRepository().Filter(nombre, areaOperativa, usuario);
        }
        #endregion

        #region Métodos Tabla DPO_CASO_DETALLE
        /// <summary>
        /// Inserta un registro de la tabla DPO_CASO_DETALLE
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoCasoDetalle(DpoCasoDetalleDTO entity)
        {
            try
            {
                FactorySic.GetDpoCasoDetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DPO_CASO_DETALLE
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void UpdateDpoCasoDetalle(DpoCasoDetalleDTO entity)
        {
            FactorySic.GetDpoCasoDetalleRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla DPO_CASO_DETALLE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteDpoCasoDetalle(int id)
        {
            FactorySic.GetDpoCasoDetalleRepository().Delete(id);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DPO_CASO_DETALLE
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DpoCasoDTO</returns>
        public DpoCasoDetalleDTO GetByIdDpoCasoDetalle(int id)
        {
            return FactorySic.GetDpoCasoDetalleRepository().GetById(id);
        }

        /// <summary>
        /// Listar registros de DPO_CASO_DETALLE
        /// </summary>
        /// <returns>List DpoCasoDetalleDTO </returns>
        public List<DpoCasoDetalleDTO> ListDpoCasoDetalle()
        {
            return FactorySic.GetDpoCasoDetalleRepository().List();
        }

        /// <summary>
        /// Listar registros de DPO_CASO_DETALLE
        /// </summary>
        /// <param name="idCaso"></param>
        /// <returns>List DpoCasoDetalleDTO </returns>
        public List<DpoCasoDetalleDTO> ListByIdCaso(int idCaso)
        {
            return FactorySic.GetDpoCasoDetalleRepository().ListByIdCaso(idCaso);
        }

        /// <summary>
        /// Listar funciones de data maestra
        /// </summary>
        /// <param name="idCaso"></param>
        /// <returns>List DpoFuncionDataMaestraDTO </returns>
        public List<DpoFuncionDataMaestraDTO> ListFuncionesDataMaestra(int idCaso)
        {
            return FactorySic.GetDpoCasoDetalleRepository().ListFuncionesDataMaestra(idCaso);
        }

        /// <summary>
        /// Listar funciones de data a procesar
        /// </summary>
        /// <param name="idCaso"></param>
        /// <returns>List DpoFuncionDataProcesarDTO </returns>
        public List<DpoFuncionDataProcesarDTO> ListFuncionesDataProcesar(int idCaso)
        {
            return FactorySic.GetDpoCasoDetalleRepository().ListFuncionesDataProcesar(idCaso);
        }

        /// <summary>
        /// Lista funciones
        /// </summary>
        /// <param name="idCaso"></param>
        /// <returns>object { listFuncionesDataMaestra, listFuncionesDataProcesar }</returns>
        public object ListFunciones(int idCaso)
        {
            // Lista de funciones de data maestra
            List<DpoFuncionDataMaestraDTO> listFuncionesDataMaestra = new List<DpoFuncionDataMaestraDTO>();
            listFuncionesDataMaestra = ListFuncionesDataMaestra(idCaso);

            // Lista de funciones de data a procesar
            List<DpoFuncionDataProcesarDTO> listFuncionesDataProcesar = new List<DpoFuncionDataProcesarDTO>();
            listFuncionesDataProcesar = this.ListFuncionesDataProcesar(idCaso);

            // Instancia y llena el objeto
            object res = new { listFuncionesDataMaestra, listFuncionesDataProcesar };

            return res;
        }

        /// <summary>
        /// Elimina registros de la tabla DPO_CASO_DETALLE por id de caso
        /// </summary>
        /// <param name="idCaso"></param>
        /// <returns></returns>
        public void DeleteDpoDetalleCasoByIdCaso(int idCaso)
        {
            FactorySic.GetDpoCasoDetalleRepository().DeleteByIdCaso(idCaso);
        }

        /// <summary>
        /// Elimina registros de la tabla DPO_CASO_DETALLE_FUNCPAR por id de caso
        /// </summary>
        /// <param name="idCaso"></param>
        /// <returns></returns>
        public void DeleteDpoDetalleCasoParametroByIdCaso(int idCaso)
        {
            FactorySic.GetDpoCasoDetalleRepository().DeleteByIdCaso(idCaso);
        }

        /// <summary>
        /// Listar los parametros de la funcion r1
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns> object </returns>
        public object ListParametrosR1(int idCaso, int idDetalleCaso)
        {
            // Lista de parametros de la funcion r1
            List<DpoParametrosR1DTO> listParametrosR1 = new List<DpoParametrosR1DTO>();
            listParametrosR1 = FactorySic.GetDpoCasoDetalleRepository().ListParametrosR1(idCaso, idDetalleCaso);

            // Instancia y llena el objeto
            object res = new { listParametrosR1 };

            return res;
        }

        /// <summary>
        /// Listar los parametros de la funcion r2
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns> object </returns>
        public object ListParametrosR2(int idCaso, int idDetalleCaso)
        {
            // Lista de parametros de la funcion r2
            List<DpoParametrosR2DTO> listParametrosR2 = new List<DpoParametrosR2DTO>();
            listParametrosR2 = FactorySic.GetDpoCasoDetalleRepository().ListParametrosR2(idCaso, idDetalleCaso);

            // Instancia y llena el objeto
            object res = new { listParametrosR2 };

            return res;
        }

        /// <summary>
        /// Listar los parametros de la funcion f1
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns> object </returns>
        public object ListParametrosF1(int idCaso, int idDetalleCaso)
        {
            // Lista de parametros de la funcion f1
            List<DpoParametrosF1DTO> listParametrosF1 = new List<DpoParametrosF1DTO>();
            listParametrosF1 = FactorySic.GetDpoCasoDetalleRepository().ListParametrosF1(idCaso, idDetalleCaso);

            // Instancia y llena el objeto
            object res = new { listParametrosF1 };

            return res;
        }

        /// <summary>
        /// Listar los parametros de la funcion f2
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns> object </returns>
        public object ListParametrosF2(int idCaso, int idDetalleCaso)
        {
            // Lista de parametros de la funcion f2
            List<DpoParametrosF2DTO> listParametrosF2 = new List<DpoParametrosF2DTO>();
            listParametrosF2 = FactorySic.GetDpoCasoDetalleRepository().ListParametrosF2(idCaso, idDetalleCaso);

            // Instancia y llena el objeto
            object res = new { listParametrosF2 };

            return res;
        }

        /// <summary>
        /// Listar los parametros de la funcion a1
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns> object </returns>
        public object ListParametrosA1(int idCaso, int idDetalleCaso)
        {
            // Lista de parametros de la funcion a1
            List<DpoParametrosA1DTO> listParametrosA1 = new List<DpoParametrosA1DTO>();
            listParametrosA1 = FactorySic.GetDpoCasoDetalleRepository().ListParametrosA1(idCaso, idDetalleCaso);

            // Instancia y llena el objeto
            object res = new { listParametrosA1 };

            return res;
        }

        /// <summary>
        /// Listar los parametros de la funcion a2
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns> object </returns>
        public object ListParametrosA2(int idCaso, int idDetalleCaso)
        {
            // Lista de parametros de la funcion a2
            List<DpoParametrosA2DTO> listParametrosA2 = new List<DpoParametrosA2DTO>();
            listParametrosA2 = FactorySic.GetDpoCasoDetalleRepository().ListParametrosA2(idCaso, idDetalleCaso);

            // Instancia y llena el objeto
            object res = new { listParametrosA2 };

            return res;
        }
        #endregion

        #region Métodos Tabla DPO_FUNCION
        /// <summary>
        /// Inserta un registro de la tabla DPO_FUNCION
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveDpoFuncion(DpoFuncionDTO entity)
        {
            try
            {
                FactorySic.GetDpoFuncionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DPO_FUNCION
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void UpdateDpoFuncion(DpoFuncionDTO entity)
        {
            FactorySic.GetDpoFuncionRepository().Update(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla DPO_FUNCION
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteDpoFuncion(int id)
        {
            FactorySic.GetDpoFuncionRepository().Delete(id);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DPO_FUNCION
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DpoFuncionDTO</returns>
        public DpoFuncionDTO GetByIdDpoFuncion(int id)
        {
            return FactorySic.GetDpoFuncionRepository().GetById(id);
        }

        /// <summary>
        /// Listar registros de DPO_FUNCION
        /// </summary>
        /// <returns>List DpoFuncionDTO </returns>
        public List<DpoFuncionDTO> ListDpoFuncion()
        {
            return FactorySic.GetDpoFuncionRepository().List();
        }
        #endregion

        /// <summary>
        /// Lista de Áreas operativas
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> ObtenerListaAreaOperativa()
        {
            List<EqAreaDTO> entitys = new List<EqAreaDTO>();

            EqAreaDTO area = new EqAreaDTO();
            area.Areacodi = 2;
            area.Areaabrev = ConstantesDpo.AreaOperativaAbrevNorte;
            area.Areanomb = ConstantesDpo.AreaOperativaNombreNorte;
            area.Orden = 1;
            area.Tareacodi = ConstantesDpo.TipoAreaOperativa;
            area.Subestacion = ConstantesDpo.FormulaAreaOperativaNorte;
            entitys.Add(area);

            area = new EqAreaDTO();
            area.Areacodi = 3;
            area.Areaabrev = ConstantesDpo.AreaOperativaAbrevCentro;
            area.Areanomb = ConstantesDpo.AreaOperativaNombreCentro;
            area.Orden = 2;
            area.Tareacodi = ConstantesDpo.TipoAreaOperativa;
            area.Subestacion = ConstantesDpo.FormulaAreaOperativaCentro;
            entitys.Add(area);

            area = new EqAreaDTO();
            area.Areacodi = 5;
            area.Areaabrev = ConstantesDpo.AreaOperativaAbrevSur;
            area.Areanomb = ConstantesDpo.AreaOperativaNombreSur;
            area.Orden = 3;
            area.Tareacodi = ConstantesDpo.TipoAreaOperativa;
            area.Subestacion = ConstantesDpo.FormulaAreaOperativaSur;
            entitys.Add(area);

            //area = new EqAreaDTO();
            //area.Areaabrev = ConstantesDpo.AreaOperativaAbrevNorteMedio;
            //area.Areanomb = ConstantesDpo.AreaOperativaNombreNorteMedio;
            //area.Orden = 4;
            //area.Tareacodi = ConstantesDpo.TipoSubAreaOperativa;
            //area.Subestacion = ConstantesDpo.FormulaAreaOperativaNorteMedio;
            //entitys.Add(area);

            //area = new EqAreaDTO();
            //area.Areaabrev = ConstantesDpo.AreaOperativaAbrevLima;
            //area.Areanomb = ConstantesDpo.AreaOperativaNombreLima;
            //area.Orden = 5;
            //area.Tareacodi = ConstantesDpo.TipoSubAreaOperativa;
            //area.Subestacion = ConstantesDpo.FormulaAreaOperativaLima;
            //entitys.Add(area);

            //area = new EqAreaDTO();
            //area.Areaabrev = ConstantesDpo.AreaOperativaAbrevElectroandes;
            //area.Areanomb = ConstantesDpo.AreaOperativaNombreElectroandes;
            //area.Orden = 6;
            //area.Tareacodi = ConstantesDpo.TipoSubAreaOperativa;
            //area.Subestacion = ConstantesDpo.FormulaAreaOperativaElectroandes;
            //entitys.Add(area);

            //area = new EqAreaDTO();
            //area.Areaabrev = ConstantesDpo.AreaOperativaAbrevSurMedio;
            //area.Areanomb = ConstantesDpo.AreaOperativaNombreSurMedio;
            //area.Orden = 7;
            //area.Tareacodi = ConstantesDpo.TipoSubAreaOperativa;
            //area.Subestacion = ConstantesDpo.FormulaAreaOperativaSurMedio;
            //entitys.Add(area);

            //area = new EqAreaDTO();
            //area.Areaabrev = ConstantesDpo.AreaOperativaAbrevSurEste;
            //area.Areanomb = ConstantesDpo.AreaOperativaNombreSurEste;
            //area.Orden = 8;
            //area.Tareacodi = ConstantesDpo.TipoSubAreaOperativa;
            //area.Subestacion = ConstantesDpo.FormulaAreaOperativaSurEste;
            //entitys.Add(area);

            //area = new EqAreaDTO();
            //area.Areaabrev = ConstantesDpo.AreaOperativaAbrevSurOeste;
            //area.Areanomb = ConstantesDpo.AreaOperativaNombreSurOeste;
            //area.Orden = 9;
            //area.Tareacodi = ConstantesDpo.TipoSubAreaOperativa;
            //area.Subestacion = ConstantesDpo.FormulaAreaOperativaSurOeste;

            //area = new EqAreaDTO();
            //area.Areaabrev = ConstantesDpo.AreaOperativaAbrevArequipa;
            //area.Areanomb = ConstantesDpo.AreaOperativaNombreArequipa;
            //area.Orden = 10;
            //area.Tareacodi = ConstantesDpo.TipoSubAreaOperativa;
            //area.Subestacion = ConstantesDpo.FormulaAreaOperativaArequipa;
            //entitys.Add(area);

            //area = new EqAreaDTO();
            //area.Areaabrev = ConstantesDpo.AreaOperativaAbrevCentroNorte;
            //area.Areanomb = ConstantesDpo.AreaOperativaNombreCentroNorte;
            //area.Orden = 11;
            //area.Tareacodi = ConstantesDpo.TipoNodefinido;
            //entitys.Add(area);

            //area = new EqAreaDTO();
            //area.Areaabrev = ConstantesDpo.AreaOperativaAbrevCentroSur;
            //area.Areanomb = ConstantesDpo.AreaOperativaNombreCentroSur;
            //area.Orden = 12;
            //area.Tareacodi = ConstantesDpo.TipoNodefinido;
            //entitys.Add(area);

            return entitys;
        }

        #region Métodos para el desarrollo del ANEXO D
        /// <summary>
        /// Filtrar por rango de fechas data historica de cada 30 minutos (48 mediciones)
        /// </summary>
        /// <param name="fechaini">Fecha inicio</param>
        /// <param name="fechafin">Fecha fin</param>
        /// <returns></returns>
        public List<DpoHistorico48DTO> FiltrarHistorico48PorRangoFechas(DateTime fechaini, DateTime fechafin)
        {
            return FactorySic.GetDpoCasoDetalleRepository().FiltrarHistorico48PorRangoFechas(fechaini, fechafin);
        }

        /// <summary>
        /// Filtrar por rango de fechas data historica completa (96 mediciones)
        /// </summary>
        /// <param name="fechaini">Fecha inicio</param>
        /// <param name="fechafin">Fecha fin</param>
        /// <returns></returns>
        public List<DpoHistorico96DTO> FiltrarHistorico96PorRangoFechas(DateTime fechaini, DateTime fechafin)
        {
            return FactorySic.GetDpoCasoDetalleRepository().FiltrarHistorico96PorRangoFechas(fechaini, fechafin);
        }

        /// <summary>
        /// Obtener una columna de datos (1 registro) cuyo muestreo es cada 30 minutos (48 Hs)
        /// </summary>
        /// <returns></returns>
        public List<DpoHistorico48DTO> ObtenerColumnaDatos48()
        {
            return FactorySic.GetDpoCasoDetalleRepository().ObtenerColumnaDatos48();
        }

        /// <summary>
        /// Obtener una columna de datos (1 registro) cuyo muestreo completas (96 Hs)
        /// </summary>
        /// <returns></returns>
        public List<DpoHistorico96DTO> ObtenerColumnaDatos96()
        {
            return FactorySic.GetDpoCasoDetalleRepository().ObtenerColumnaDatos96();
        }

        /// <summary>
        /// Obtener una serie de datos (mas de 1 registro) cuyo muestreo es cada 30 minutos (48 Hs)
        /// </summary>
        /// <returns></returns>
        public List<DpoHistorico48DTO> ObtenerSerieDatos48()
        {
            return FactorySic.GetDpoCasoDetalleRepository().ObtenerColumnaDatos48();
        }

        /// <summary>
        /// Obtener una serie de datos (mas de 1 registro) cuyo muestreo completas (96 Hs)
        /// </summary>
        /// <returns></returns>
        public List<DpoHistorico96DTO> ObtenerSerieDatos96()
        {
            return FactorySic.GetDpoCasoDetalleRepository().ObtenerColumnaDatos96();
        }

        /// <summary>
        /// Obtener una serie de datos historicos filtrados por grupo de dia tipico
        /// <param name="fechaGrupo">Dia tipico</param>
        /// <param name="lstDatos48">Lista de datos histricos</param>
        /// </summary>
        /// <returns>lstDatos48Parcial</returns>
        public List<DpoHistorico48DTO> ObtenerDataHistoricaGrupoDiaTipico(string fechaGrupo, List<DpoHistorico48DTO> lstDatos48)
        {
            List<DpoHistorico48DTO> lstDatos48Parcial = new List<DpoHistorico48DTO>();
            List<DpoHistorico48DTO> lstDatos48Filtrado = new List<DpoHistorico48DTO>();

            // Convierto el grupos que contiene la cadena de dias tipicos (dd/MM/yyyy) separada x comas y lo convierto en un array
            string[] fechasDiasTipicos = fechaGrupo.Split(new char[] { ',', '.' });

            // Recorro el grupo array string[] fechasDiasTipico (dia tipico)
            foreach (string fechaDiaTipico in fechasDiasTipicos)
            {
                // Parseo la fecha del array
                DateTime parseFecha = DateTime.ParseExact(fechaDiaTipico.Trim(), ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);

                // Añade los registro filtrados por dia tipico a la lista parcial apartir de la lista historica
                lstDatos48Filtrado = lstDatos48.Where(x => x.Medifecha == parseFecha).ToList();

                // Añade la lista filtrada a la lista parcial
                lstDatos48Parcial.AddRange(lstDatos48Filtrado);
            }

            return lstDatos48Parcial;
        }

        /// <summary>
        /// Obtener una serie de datos en blanco de una lista histrorica previamente filtrada
        /// <param name="lstDatos48Parcial">Dia tipico</param>
        /// <param name="blancos">Lista de datos histricos</param>
        /// </summary>
        /// <returns>lstDatos48Parcial, ref int blancos</returns>
        public List<DpoHistorico48DTO> ObtenerDataBlancos(List<DpoHistorico48DTO> lstDatos48Parcial, ref int blancos)
        {
            List<DpoHistorico48DTO> listBlancos = new List<DpoHistorico48DTO>();

            decimal[] maximos = new decimal[lstDatos48Parcial.Count];
            decimal[] minimos = new decimal[lstDatos48Parcial.Count];

            decimal[] promedios = new decimal[ConstantesDpo.Itv30min];
            decimal promedioMax = 0;
            decimal promedioMin = 0;

            decimal[] desviacionesEstandar = new decimal[ConstantesDpo.Itv30min];
            decimal desviacionEstandarMax = 0;
            decimal desviacionEstandarMin = 0;

            decimal[] limsSuperiores = new decimal[ConstantesDpo.Itv30min];
            decimal limSuperiorMax = 0;
            decimal limSuperiorMin = 0;

            decimal[] limsInferiores = new decimal[ConstantesDpo.Itv30min];
            decimal limInferiorMax = 0;
            decimal limInferiorMin = 0;

            // Declaro la variable auxiliar para obtener los maximos y minimos
            decimal[] arrayDatos48ParcialUx = new decimal[lstDatos48Parcial.Count];

            // Obtengo los max, min de la lista filtrada por grupo de dias tipicos
            int j = 0;
            foreach (DpoHistorico48DTO dato in lstDatos48Parcial)
            {
                for (int i = 0; i < ConstantesDpo.Itv30min; i++)
                {
                    arrayDatos48ParcialUx[i] = (decimal)dato.GetType().GetProperty("H" + (i + 1)).GetValue(dato);
                }

                maximos[j] = arrayDatos48ParcialUx.Max();
                minimos[j] = arrayDatos48ParcialUx.Min();

                j++;
            }

            // Calculo los promedios desviaciones y limites inferiores y superiores
            // apartir de la lista filtrada por grupo de dias tipicos
            for (int k = 0; k < ConstantesDpo.Itv30min; k++)
            {
                // Calcula el promedio recorriendo los campos H de la clase y lo asigna al campo de indice i del array
                promedios[k] = lstDatos48Parcial.Average(m => (decimal)m.GetType().GetProperty("H" + (k + 1)).GetValue(m));

                // Calcular la desviación estándar
                desviacionesEstandar[k] = (decimal)Math.Sqrt((double)(((decimal)lstDatos48Parcial.Sum(m => Math.Pow((double)m.GetType().GetProperty("H" + (k + 1)).GetValue(m) - (double)promedios[k], 2))) / lstDatos48Parcial.Count));

                // Calcular los limites superior e inferior
                limsSuperiores[k] = promedios[k] + 3 * desviacionesEstandar[k];
                limsInferiores[k] = promedios[k] - 3 * desviacionesEstandar[k];

                k++;
            }

            // Calcula el promedioMax y promedioMin
            promedioMax = maximos.Average();
            promedioMin = minimos.Average();

            // Calcula la desviacionEstandarMax y desviacionEstandarMin
            desviacionEstandarMax = (decimal)Math.Sqrt((double)((decimal)maximos.Sum(m => Math.Pow((double)maximos.Average() - (double)promedioMax, 2))) / maximos.Length);
            desviacionEstandarMin = (decimal)Math.Sqrt((double)((decimal)minimos.Sum(m => Math.Pow((double)minimos.Average() - (double)promedioMin, 2))) / minimos.Length);

            // Calcula el limSuperiorMax y limInferiorMax
            limSuperiorMax = promedioMax + 3 * desviacionEstandarMax;
            limInferiorMax = promedioMax - 3 * desviacionEstandarMax;

            // Calcula el limSuperiorMax y limInferiorMax
            limSuperiorMin = promedioMin + 3 * desviacionEstandarMin;
            limInferiorMin = promedioMin - 3 * desviacionEstandarMin;

            // Obtenidido los valores maximo, minimo, limites superiores o inferiores y los promedio y desviaciones
            // Filtro la lista para obtener las posciones nulas
            blancos = 0;
            foreach (DpoHistorico48DTO dato in lstDatos48Parcial)
            {
                for (int i = 0; i < ConstantesDpo.Itv30min; i++)
                {
                    // Se lee el valor de la celda de la matriz filtrada
                    decimal val = (decimal)dato.GetType().GetProperty("H" + (i + 1)).GetValue(dato);

                    // Valido el valor y asigno el mismo valor o nulo
                    if (val > limSuperiorMax || val < 0)
                    {
                        dato.GetType().GetProperty($"H{(i + 1)}").SetValue(dato, null);
                        blancos++;
                    }
                }
            }

            return listBlancos;
        }

        /// <summary>
        /// Obtener una serie de datos en blanco de una lista histrorica previamente filtrada
        /// <param name="lstDatos48Parcial">Dia tipico</param>
        /// <param name="blancos">Lista de datos histricos</param>
        /// </summary>
        /// <returns>lstDatos48Parcial, ref int blancos</returns>
        public List<DpoHistorico48DTO> CorregirVacios(List<DpoHistorico48DTO> lstDatos48Parcial, ref int blancos)
        {
            List<DpoHistorico48DTO> listBlancos = new List<DpoHistorico48DTO>();

            decimal[] promedios = new decimal[ConstantesDpo.Itv30min];

            // Calculo los promedios desviaciones y limites inferiores y superiores
            // apartir de la lista filtrada por grupo de dias tipicos
            for (int k = 0; k < ConstantesDpo.Itv30min; k++)
            {
                // Calcula el promedio recorriendo los campos H de la clase y lo asigna al campo de indice i del array
                promedios[k] = lstDatos48Parcial.Average(m => (decimal)m.GetType().GetProperty("H" + (k + 1)).GetValue(m));
            }

            foreach (DpoHistorico48DTO dato in lstDatos48Parcial)
            {
                // Se lee el valor de la celda de la matriz filtrada
                decimal val1 = (decimal)dato.GetType().GetProperty("H1").GetValue(dato);
                decimal val48 = (decimal)dato.GetType().GetProperty("H48").GetValue(dato);

                // con el promedio corregiria los blancos finalmente
                if (val1 == 0)
                {
                    // reemplazo nulo con promedios[k]
                    dato.GetType().GetProperty($"H1").SetValue(dato, promedios[0]);
                }
                
                if (val48 == 0)
                {
                    // reemplazo nulo con promedios[k]
                    dato.GetType().GetProperty($"H48").SetValue(dato, promedios[47]);
                }

               
            }


            return listBlancos;
        }

        /// <summary>
        /// A1: De una serie de tiempo completa, obtener los días típicos definidos en el caso con su perfil promedio
        /// </summary>
        /// <param name="idCaso">Id del Caso</param>
        /// <param name="idFuncion">Id de la función de configuración</param>
        /// <param name="tipFuncion">Tipo función DM: DataMaestra; DP: DataProcesar</param>
        /// <param name="fecInicioFormula">Fecha inicio de la formula SCADA</param>
        /// <param name="fecFinFormula">Fecha fin de la formula SCADAr</param>
        /// <param name="lstDatos48">Data historica</param>
        /// <returns></returns>
        public List<DpoMedicion48DTO> ObtenerUnaSemanaTipicaConPromedioYDesviacionEstandarA1(int idCaso,
                                                                                             int idFuncion,
                                                                                             string tipFuncion,
                                                                                             DateTime fecInicioFormula,
                                                                                             DateTime fecFinFormula,
                                                                                             List<DpoHistorico48DTO> lstDatos48)
        {
            List<DpoMedicion48DTO> lstDatos48Promedios = new List<DpoMedicion48DTO>();
            DpoMedicion48DTO entityDatos48Promedios = null;

            List<DpoHistorico48DTO> lstDatos48Parcial = new List<DpoHistorico48DTO>();


            #region Carga de parametros
            // Obtengo los parametros de la configuracion de dias tipicos correspondiente a esta funcion A1
            List<DpoParametrosA1DTO> listParametrosA1 = FactorySic.GetDpoCasoDetalleRepository().GetParametrosA1(idCaso, idFuncion, tipFuncion);

            // Seteo la entidad para dias tipicos
            DpoDiasTipicosDTO dpoDiasTipicosDTO = null;

            foreach (DpoParametrosA1DTO entityParametros in listParametrosA1)
            {
                dpoDiasTipicosDTO = new DpoDiasTipicosDTO()
                {
                    Lunes = entityParametros.Pafuna1dtg1,
                    Martes = entityParametros.Pafuna1dtg2,
                    Miercoles = entityParametros.Pafuna1dtg3,
                    Jueves = entityParametros.Pafuna1dtg4,
                    Viernes = entityParametros.Pafuna1dtg5,
                    Sabado = entityParametros.Pafuna1dtg6,
                    Domingo = entityParametros.Pafuna1dtg7,
                };
            }
            #endregion

            #region Obtener los dias tipicos
            // Se establece el rango para dos meses hacia atras, apartir del mes actual
            DateTime fecInicio = new DateTime(fecInicioFormula.Year, fecInicioFormula.Month, fecInicioFormula.Day, 0, 0, 0);
            DateTime fecFin = new DateTime(fecFinFormula.Year, fecFinFormula.Month, fecFinFormula.Day, 0, 0, 0);

            // Defino la Semana tipica y obtengo los grupos de dias tipicos
            List<string> lstGruposDiasTipicos = ObtenerGruposDiasTipicos(dpoDiasTipicosDTO);

            // Defino los dias tipicos  expresados en fechas de formato dd/MM/yyyy a apartir de los grupos de dias tipicos o Semana tipica
            List<string> lstFechasDiasTipicos = ObtenerFechasGruposDiasTipicos(lstGruposDiasTipicos,
                                                                               fecInicio.Year,
                                                                               fecInicio.Month,
                                                                               fecFin.Month);
            #endregion

            #region 1er Filtro de Información - Filtra feriados a la información historica
            // Obtengo las fechas de los feriados filtradas por el año actual
            List<DateTime> feriados = this.ObtenerFeriadosPorAnio(DateTime.Now.Year);

            // Se remueven los días feriados del año que se encuentren en el rango de los dos meses seleccionados
            // los dias tipicos (Historicos - feriados = tipicos)
            lstDatos48 = lstDatos48.Where(x => !(feriados.Contains(x.Medifecha))).ToList();
            #endregion

            #region 2do Filtro de Información - Filtra por grupo de los dias tipicos a la información historica
            int iContadorGrupos = 0;

            // Recorro los grupos de fechas de dias tipicos y armo la lista filtrada por dias tipicos
            foreach (string fechaGrupo in lstFechasDiasTipicos)
            {
                // Obtenego la lista filtrada por grupo de dias tipicos
                lstDatos48Parcial = ObtenerDataHistoricaGrupoDiaTipico(fechaGrupo, lstDatos48);

                // Calculo los promedios apartir de la lista filtrada por grupo de dias tipicos
                decimal[] promedios = new decimal[ConstantesDpo.Itv30min];

                if (lstDatos48Parcial.Count > 0)
                {
                    promedios = ObtenerPerfilPromedio(lstDatos48Parcial, ConstantesDpo.Itv30min);
                }
                else
                {
                    for (int k = 0; k < ConstantesDpo.Itv30min; k++)
                    {
                        promedios[k] = 0;
                    }
                }

                iContadorGrupos++;

                entityDatos48Promedios = new DpoMedicion48DTO();

                entityDatos48Promedios.FuncionOrigen = "A1";

                if (iContadorGrupos == 1)
                    entityDatos48Promedios.Grupo = "G1";
                if (iContadorGrupos == 2)
                    entityDatos48Promedios.Grupo = "G2";
                if (iContadorGrupos == 3)
                    entityDatos48Promedios.Grupo = "G3";
                if (iContadorGrupos == 4)
                    entityDatos48Promedios.Grupo = "G4";
                if (iContadorGrupos == 5)
                    entityDatos48Promedios.Grupo = "G5";
                if (iContadorGrupos == 6)
                    entityDatos48Promedios.Grupo = "G6";
                if (iContadorGrupos == 7)
                    entityDatos48Promedios.Grupo = "G7";

                for (int j = 0; j < ConstantesDpo.Itv30min; j++)
                {
                    entityDatos48Promedios.GetType().GetProperty($"H{(j + 1)}").SetValue(entityDatos48Promedios, promedios[j]);
                }

                lstDatos48Promedios.Add(entityDatos48Promedios);

            }
            #endregion


            return lstDatos48Promedios;
        }

        /// <summary>
        /// A2: De una serie de tiempo completa, obtener los días típicos definidos en el caso  con su perfil promedio 
        /// y desviación estándar
        /// </summary>
        /// <param name="idCaso">Id del Caso</param>
        /// <param name="idFuncion">Id de la función de configuración</param>
        /// <param name="tipFuncion">Tipo función DM: DataMaestra; DP: DataProcesar</param>
        /// <param name="fecInicioFormula">Fecha inicio de la formula SCADA</param>
        /// <param name="fecFinFormula">Fecha fin de la formula SCADAr</param>
        /// <param name="lstDatos48">Data historica</param>
        /// <returns></returns>
        public List<List<DpoMedicion48DTO>> ObtenerDiasTipicosConPromedioYDesviacionEstandarA2(int idCaso,
                                                                                               int idFuncion,
                                                                                               string tipFuncion,
                                                                                               DateTime fecInicioFormula,
                                                                                               DateTime fecFinFormula,
                                                                                               List<DpoHistorico48DTO> lstDatos48)
        {
            List<List<DpoMedicion48DTO>> lstDatos48Final = new List<List<DpoMedicion48DTO>>();

            List<DpoMedicion48DTO> lstDatos48Promedios = new List<DpoMedicion48DTO>();
            DpoMedicion48DTO entityDatos48Promedios = null;

            List<DpoMedicion48DTO> lstDatos48Desviaciones = new List<DpoMedicion48DTO>();
            DpoMedicion48DTO entityDatos48PDesviaciones = null;

            List<DpoHistorico48DTO> lstDatos48Parcial = new List<DpoHistorico48DTO>();


            #region Carga de parametros
            // Obtengo los parametros de la configuracion de dias tipicos correspondiente a esta funcion A2
            List<DpoParametrosA2DTO> listParametrosA2 = FactorySic.GetDpoCasoDetalleRepository().GetParametrosA2(idCaso, idFuncion, tipFuncion);

            // Seteo la entidad para dias tipicos
            DpoDiasTipicosDTO dpoDiasTipicosDTO = null;

            foreach (DpoParametrosA2DTO entityParametros in listParametrosA2)
            {
                dpoDiasTipicosDTO = new DpoDiasTipicosDTO()
                {
                    Lunes = entityParametros.Pafuna2dtg1,
                    Martes = entityParametros.Pafuna2dtg2,
                    Miercoles = entityParametros.Pafuna2dtg3,
                    Jueves = entityParametros.Pafuna2dtg4,
                    Viernes = entityParametros.Pafuna2dtg5,
                    Sabado = entityParametros.Pafuna2dtg6,
                    Domingo = entityParametros.Pafuna2dtg7,
                };
            }
            #endregion

            #region Obtener los dias tipicos
            // Se establece el rango para dos meses hacia atras, apartir del mes actual
            DateTime fecInicio = new DateTime(fecInicioFormula.Year, fecInicioFormula.Month, fecInicioFormula.Day, 0, 0, 0);
            DateTime fecFin = new DateTime(fecFinFormula.Year, fecFinFormula.Month, fecFinFormula.Day, 0, 0, 0);

            // Defino la Semana tipica y obtengo los grupos de dias tipicos
            List<string> lstGruposDiasTipicos = ObtenerGruposDiasTipicos(dpoDiasTipicosDTO);

            // Defino los dias tipicos  expresados en fechas de formato dd/MM/yyyy a apartir de los grupos de dias tipicos o Semana tipica
            List<string> lstFechasDiasTipicos = ObtenerFechasGruposDiasTipicos(lstGruposDiasTipicos,
                                                                               fecInicio.Year,
                                                                               fecInicio.Month,
                                                                               fecFin.Month);
            #endregion

            #region Filtra feriados a la información historica
            // Obtengo las fechas de los feriados filtradas por el año actual
            List<DateTime> feriados = this.ObtenerFeriadosPorAnio(DateTime.Now.Year);

            // Se remueven los días feriados del año que se encuentren en el rango de los dos meses seleccionados
            // los dias tipicos (Historicos - feriados = tipicos)
            lstDatos48 = lstDatos48.Where(x => !(feriados.Contains(x.Medifecha))).ToList();
            #endregion

            #region 2do Filtro de Información - Filtra por grupo de los dias tipicos a la información historica
            int iContadorGrupos = 0;

            // Recorro los grupos de fechas de dias tipicos y armo la lista filtrada por dias tipicos
            foreach (string fechaGrupo in lstFechasDiasTipicos)
            {
                // Obtenego la lista filtrada por grupo de dias tipicos
                lstDatos48Parcial = ObtenerDataHistoricaGrupoDiaTipico(fechaGrupo, lstDatos48);

                // Calculo los promedios apartir de la lista filtrada por grupo de dias tipicos
                decimal[] promedios = new decimal[ConstantesDpo.Itv30min];
                if (lstDatos48Parcial.Count > 0)
                {
                    promedios = ObtenerPerfilPromedio(lstDatos48Parcial, ConstantesDpo.Itv30min);
                }
                else
                {
                    for (int k = 0; k < ConstantesDpo.Itv30min; k++)
                    {
                        promedios[k] = 0;
                    }
                }

                // Calculo las desviaciones apartir de la lista filtrada por grupo de dias tipicos
                decimal[] desviacionesEstandar = new decimal[ConstantesDpo.Itv30min];
                if (lstDatos48Parcial.Count > 0)
                {
                    desviacionesEstandar = ObtenerPerfilDesviacionEstandar(lstDatos48Parcial, ConstantesDpo.Itv30min);
                }
                else
                {
                    for (int k = 0; k < ConstantesDpo.Itv30min; k++)
                    {
                        desviacionesEstandar[k] = 0;
                    }
                }

                iContadorGrupos++;

                // Lleno la lista de promedios con los promedios por grupo recorrido para finalmente llenar
                // lista de listas final
                entityDatos48Promedios = new DpoMedicion48DTO();

                entityDatos48Promedios.FuncionOrigen = "A2";

                if (iContadorGrupos == 1)
                    entityDatos48Promedios.Grupo = "G1";
                if (iContadorGrupos == 2)
                    entityDatos48Promedios.Grupo = "G2";
                if (iContadorGrupos == 3)
                    entityDatos48Promedios.Grupo = "G3";
                if (iContadorGrupos == 4)
                    entityDatos48Promedios.Grupo = "G4";
                if (iContadorGrupos == 5)
                    entityDatos48Promedios.Grupo = "G5";
                if (iContadorGrupos == 6)
                    entityDatos48Promedios.Grupo = "G6";
                if (iContadorGrupos == 7)
                    entityDatos48Promedios.Grupo = "G7";

                for (int k = 0; k < ConstantesDpo.Itv30min; k++)
                {
                    entityDatos48Promedios.GetType().GetProperty($"H{(k + 1)}").SetValue(entityDatos48Promedios, promedios[k]);
                }

                lstDatos48Promedios.Add(entityDatos48Promedios);


                // Lleno la lista de desviaciones con las desviaciones por grupo recorrido para finalmente llenar
                // lista de listas final
                entityDatos48PDesviaciones = new DpoMedicion48DTO();

                entityDatos48PDesviaciones.FuncionOrigen = "A2";

                if (iContadorGrupos == 1)
                    entityDatos48PDesviaciones.Grupo = "G1";
                if (iContadorGrupos == 2)
                    entityDatos48PDesviaciones.Grupo = "G2";
                if (iContadorGrupos == 3)
                    entityDatos48PDesviaciones.Grupo = "G3";
                if (iContadorGrupos == 4)
                    entityDatos48PDesviaciones.Grupo = "G4";
                if (iContadorGrupos == 5)
                    entityDatos48PDesviaciones.Grupo = "G5";
                if (iContadorGrupos == 6)
                    entityDatos48PDesviaciones.Grupo = "G6";
                if (iContadorGrupos == 7)
                    entityDatos48PDesviaciones.Grupo = "G7";

                for (int j = 0; j < ConstantesDpo.Itv30min; j++)
                {
                    entityDatos48PDesviaciones.GetType().GetProperty($"H{(j + 1)}").SetValue(entityDatos48PDesviaciones, desviacionesEstandar[j]);
                }

                lstDatos48Desviaciones.Add(entityDatos48PDesviaciones);
            }

            lstDatos48Final.Add(lstDatos48Promedios);
            lstDatos48Final.Add(lstDatos48Desviaciones);
            #endregion


            return lstDatos48Final;
        }

        /// <summary>
        /// F1: Filtrado por máxima variación por rampa
        /// </summary>
        /// <param name="idCaso">Id del Caso</param>
        /// <param name="idFuncion">Id de la función de configuración</param>
        /// <param name="tipFuncion">Tipo función DM: DataMaestra; DP: DataProcesar</param>
        /// <param name="lstDatos48">Data historica</param>
        /// <returns></returns>
        public List<DpoMedicion48DTO> FiltrarPorPMaximaVariacionRampaF1(int idCaso,
                                                                        int idFuncion,
                                                                        string tipFuncion,
                                                                        List<DpoHistorico48DTO> lstDatos48)
        {
            List<DpoMedicion48DTO> lstDatos48Corregido = new List<DpoMedicion48DTO>();
            DpoMedicion48DTO entityDpoMedicion48 = null;

            #region Carga de parametros
            // Obtengo los parametros de la configuracion de la funcion A1
            List<DpoParametrosF1DTO> listParametrosF1 = FactorySic.GetDpoCasoDetalleRepository().GetParametrosF1(idCaso, idFuncion, tipFuncion);
            double valTolRampa = Convert.ToDouble(listParametrosF1[0].Pafunf1toram);
            #endregion

            #region Se corrige la serie de tiempo
            // Recorre la lista
            foreach (DpoHistorico48DTO dato in lstDatos48)
            {
                entityDpoMedicion48 = new DpoMedicion48DTO();

                // Recorro las columnas de la lista apartir del campo H2
                for (int i = 1; i < ConstantesDpo.Itv30min; i++)
                {
                    decimal valHnMas1 = (decimal)dato.GetType().GetProperty("H" + (i + 1)).GetValue(dato);

                    // En la 2da columna = H2 se obtiene el dato H menos el dato anterior (H -1) en valor absoluto
                    decimal diferenciaAbs = Math.Abs((decimal)dato.GetType().GetProperty("H" + (i + 1)).GetValue(dato) - (decimal)dato.GetType().GetProperty("H" + i).GetValue(dato));

                    // Si el valor de la diferencia en valor adsoluto de H(n) - H(n-1) es mayor al told rampa
                    if (diferenciaAbs > (decimal)valTolRampa)
                    {
                        // Se blanquea el valor del campo H(n). Los valores de la 1ra columna permaneceran iguales
                        entityDpoMedicion48.GetType().GetProperty($"H{(i + 1)}").SetValue(entityDpoMedicion48, null);
                    }
                    else
                    {
                        // En caso contrario se asigna el mismo valor H(n). Los valores de la 1ra columna permaneceran iguales
                        entityDpoMedicion48.GetType().GetProperty($"H{(i + 1)}").SetValue(entityDpoMedicion48, valHnMas1);
                    }
                }

                entityDpoMedicion48.Medifecha = dato.Medifecha;

                lstDatos48Corregido.Add(entityDpoMedicion48);
            }
            #endregion


            return lstDatos48Corregido;
        }

        /// <summary>
        /// F2: Exclusión de datos fuera del promedio +/- K* desviación estándar
        /// </summary>
        /// <param name="idCaso">Id del Caso</param>
        /// <param name="idFuncion">Id de la función de configuración</param>
        /// <param name="tipFuncion">Tipo función DM: DataMaestra; DP: DataProcesar</param>
        /// <param name="fecInicioFormula">Fecha inicio de la formula SCADA</param>
        /// <param name="fecFinFormula">Fecha fin de la formula SCADAr</param>
        /// <param name="lstDatos48">Data historica</param>
        /// <param name="lstDatos48A2">Data historica</param>
        /// <returns></returns>
        public List<DpoMedicion48DTO> ExcluirDatosFueraPromedioYDesviacionEstandarF2(int idCaso,
                                                                                     int idFuncion,
                                                                                     string tipFuncion,
                                                                                     DateTime fecInicioFormula,
                                                                                     DateTime fecFinFormula,
                                                                                     List<DpoHistorico48DTO> lstDatos48,
                                                                                     List<List<DpoMedicion48DTO>> lstDatos48A2)
        {
            List<DpoMedicion48DTO> lstDatos48Corregido = new List<DpoMedicion48DTO>();
            DpoMedicion48DTO entityDpoMedicion48 = null;

            List<DpoHistorico48DTO> lstDatos48Parcial = new List<DpoHistorico48DTO>();

            #region Carga de parametros A2
            // Obtengo los parametros de la configuracion de dias tipicos correspondiente a esta funcion A2
            List<DpoParametrosA2DTO> listParametrosA2 = FactorySic.GetDpoCasoDetalleRepository().GetParametrosA2(idCaso, idFuncion, tipFuncion);

            // Seteo la entidad para dias tipicos
            DpoDiasTipicosDTO dpoDiasTipicosDTO = null;

            foreach (DpoParametrosA2DTO entityParametros in listParametrosA2)
            {
                dpoDiasTipicosDTO = new DpoDiasTipicosDTO()
                {
                    Lunes = entityParametros.Pafuna2dtg1,
                    Martes = entityParametros.Pafuna2dtg2,
                    Miercoles = entityParametros.Pafuna2dtg3,
                    Jueves = entityParametros.Pafuna2dtg4,
                    Viernes = entityParametros.Pafuna2dtg5,
                    Sabado = entityParametros.Pafuna2dtg6,
                    Domingo = entityParametros.Pafuna2dtg7,
                };
            }
            #endregion

            #region Obtener los dias tipicos
            // Se establece el rango para dos meses hacia atras, apartir del mes actual
            DateTime fecInicio = new DateTime(fecInicioFormula.Year, fecInicioFormula.Month, fecInicioFormula.Day, 0, 0, 0);
            DateTime fecFin = new DateTime(fecFinFormula.Year, fecFinFormula.Month, fecFinFormula.Day, 0, 0, 0);

            // Defino la Semana tipica y obtengo los grupos de dias tipicos
            List<string> lstGruposDiasTipicos = ObtenerGruposDiasTipicos(dpoDiasTipicosDTO);

            // Defino los dias tipicos  expresados en fechas de formato dd/MM/yyyy a apartir de los grupos de dias tipicos o Semana tipica
            List<string> lstFechasDiasTipicos = ObtenerFechasGruposDiasTipicos(lstGruposDiasTipicos,
                                                                               fecInicio.Year,
                                                                               fecInicio.Month,
                                                                               fecFin.Month);
            #endregion

            #region Carga de parametros F2
            // Obtengo los parametros de la configuracion de la funcion A1
            List<DpoParametrosF2DTO> listParametrosF2 = FactorySic.GetDpoCasoDetalleRepository().GetParametrosF2(idCaso, idFuncion, tipFuncion);
            decimal factorK = Convert.ToDecimal(listParametrosF2[0].Pafunf2factk);
            #endregion

            #region Filtra feriados a la información historica
            // Obtengo las fechas de los feriados filtradas por el año actual
            List<DateTime> feriados = this.ObtenerFeriadosPorAnio(DateTime.Now.Year);

            // Se remueven los días feriados del año que se encuentren en la serie de datos de entrada (Historicos - feriados = tipicos)
            lstDatos48 = lstDatos48.Where(x => !(feriados.Contains(x.Medifecha))).ToList();
            #endregion

            #region Se corrige la serie de tiempo acorde a los dias tipicos
            int iContadorGrupos = 0;

            List<DpoMedicion48DTO> listPromedios = lstDatos48A2[0];
            List<DpoMedicion48DTO> listDesviaciones = lstDatos48A2[1];

            foreach (string fechaGrupo in lstFechasDiasTipicos)
            {
                // Obtenego la lista filtrada por grupo de dias tipicos a corregir
                lstDatos48Parcial = ObtenerDataHistoricaGrupoDiaTipico(fechaGrupo, lstDatos48);

                if (iContadorGrupos == 0)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    listPromedios = listPromedios.Where(x => x.Grupo == "G1").ToList();

                    // de la lista listDesviaciones TRAE solo un registro POR GRUPO
                    listDesviaciones = listDesviaciones.Where(x => x.Grupo == "G1").ToList();
                }

                if (iContadorGrupos == 1)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    listPromedios = listPromedios.Where(x => x.Grupo == "G2").ToList();

                    // de la lista listDesviaciones TRAE solo un registro POR GRUPO
                    listDesviaciones = listDesviaciones.Where(x => x.Grupo == "G2").ToList();
                }

                if (iContadorGrupos == 2)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    listPromedios = listPromedios.Where(x => x.Grupo == "G3").ToList();

                    // de la lista listDesviaciones TRAE solo un registro POR GRUPO
                    listDesviaciones = listDesviaciones.Where(x => x.Grupo == "G3").ToList();
                }

                if (iContadorGrupos == 3)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    listPromedios = listPromedios.Where(x => x.Grupo == "G4").ToList();

                    // de la lista listDesviaciones TRAE solo un registro POR GRUPO
                    listDesviaciones = listDesviaciones.Where(x => x.Grupo == "G4").ToList();
                }

                if (iContadorGrupos == 4)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    listPromedios = listPromedios.Where(x => x.Grupo == "G5").ToList();

                    // de la lista listDesviaciones TRAE solo un registro POR GRUPO
                    listDesviaciones = listDesviaciones.Where(x => x.Grupo == "G5").ToList();
                }

                if (iContadorGrupos == 5)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    listPromedios = listPromedios.Where(x => x.Grupo == "G6").ToList();

                    // de la lista listDesviaciones TRAE solo un registro POR GRUPO
                    listDesviaciones = listDesviaciones.Where(x => x.Grupo == "G6").ToList();
                }

                if (iContadorGrupos == 6)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    listPromedios = listPromedios.Where(x => x.Grupo == "G7").ToList();

                    // de la lista listDesviaciones TRAE solo un registro POR GRUPO
                    listDesviaciones = listDesviaciones.Where(x => x.Grupo == "G7").ToList();
                }

                // Calculo los rangos minimo y maximo apartir de las listas de promedios y desviaciones A2
                // Recorro las columnas de la lista de promedios y desviaciones
                decimal[] promedios = new decimal[ConstantesDpo.Itv30min];
                decimal[] desviaciones = new decimal[ConstantesDpo.Itv30min];
                decimal[] rangosMinimos = new decimal[ConstantesDpo.Itv30min];
                decimal[] rangosMaximos = new decimal[ConstantesDpo.Itv30min];

                int contadorListas = 0;
                foreach (List<DpoMedicion48DTO> subListas in lstDatos48A2)
                {
                    foreach (DpoMedicion48DTO item in subListas)
                    {
                        if (contadorListas == 0)
                        {
                            // Llena el array con los promedios
                            for (int k = 0; k < ConstantesDpo.Itv30min; k++)
                            {
                                promedios[k] = (decimal)item.GetType().GetProperty("H" + (k + 1)).GetValue(item);
                            }

                        }
                        else if (contadorListas == 1)
                        {
                            // Llena el array con las desviaciones
                            for (int k = 0; k < ConstantesDpo.Itv30min; k++)
                            {
                                desviaciones[k] = (decimal)item.GetType().GetProperty("H" + (k + 1)).GetValue(item);
                            }
                        }
                    }

                    contadorListas++;
                }

                //Calculamos y llenamos los array de rangos maximos y minimos
                for (int i = 0; i < ConstantesDpo.Itv30min; i++)
                {
                    rangosMinimos[i] = (-factorK) * desviaciones[i] + promedios[i];
                    rangosMaximos[i] = factorK * desviaciones[i] + promedios[i];
                }

                // Corrijo la data historica
                foreach (DpoHistorico48DTO dato in lstDatos48Parcial)
                {
                    entityDpoMedicion48 = new DpoMedicion48DTO();

                    // Recorro las columnas de la lista
                    for (int i = 0; i < ConstantesDpo.Itv30min; i++)
                    {
                        decimal val = (decimal)dato.GetType().GetProperty("H" + (i + 1)).GetValue(dato);

                        if (val > rangosMaximos[i] || val < rangosMinimos[i])
                        {
                            entityDpoMedicion48.GetType().GetProperty($"H{(i + 1)}").SetValue(entityDpoMedicion48, promedios[i]);
                        }
                    }

                    entityDpoMedicion48.Medifecha = dato.Medifecha;

                    lstDatos48Corregido.Add(entityDpoMedicion48);
                }

                iContadorGrupos++;
            }
            #endregion


            return lstDatos48Corregido;
        }

        /// <summary>
        /// R1: Autocompletar datos del rango de fechas, tomando como referencia meses pasados
        /// </summary>
        /// <param name="idCaso">Id del Caso</param>
        /// <param name="idFuncion">Id de la función de configuración</param>
        /// <param name="tipFuncion">Tipo función DM: DataMaestra; DP: DataProcesar</param>
        /// <param name="fecInicioFormula">Fecha inicio de la formula SCADA</param>
        /// <param name="fecFinFormula">Fecha fin de la formula SCADAr</param>
        /// <param name="lstDatos48">Data historica</param>
        /// <param name="lstDatos48A1">Data de A1 de perfiles promedios</param>
        /// <returns></returns>
        public List<DpoMedicion48DTO> AutoCompletarDelRangoFechasR1(int idCaso,
                                                                    int idFuncion,
                                                                    string tipFuncion,
                                                                    DateTime fecInicioFormula,
                                                                    DateTime fecFinFormula,
                                                                    List<DpoHistorico48DTO> lstDatos48,
                                                                    List<DpoMedicion48DTO> lstDatos48A1)
        {
            List<DpoMedicion48DTO> lstDatos48Corregido = new List<DpoMedicion48DTO>();
            DpoMedicion48DTO entityDpoMedicion48 = null;
            List<DpoHistorico48DTO> lstDatos48Parcial = new List<DpoHistorico48DTO>();


            #region Carga de parametros
            // Obtengo los parametros de la configuracion de dias tipicos correspondiente a esta funcion A1
            List<DpoParametrosR1DTO> listParametrosR1 = FactorySic.GetDpoCasoDetalleRepository().GetParametrosR1(idCaso, idFuncion, tipFuncion);

            // Apartir de las fechas de inicio fin de los parametros de R1, obtengo los anioMesI y anioMesF
            string[] anioMesI = listParametrosR1[0].Pafunr1deg7.Split(new char[] { ' ', '.' });
            string[] anioMesF = listParametrosR1[0].Pafunr1hag7.Split(new char[] { ' ', '.' });

            // Apartir de los anioMesI y anioMesF obtengo
            int anio = Convert.ToInt16(anioMesI[1]);
            int mesI = Convert.ToInt16(anioMesI[0]);
            int mesF = Convert.ToInt16(anioMesF[0]);

            // Seteo la entidad para dias tipicos
            DpoDiasTipicosDTO dpoDiasTipicosDTO = null;

            foreach (DpoParametrosR1DTO entityParametros in listParametrosR1)
            {
                dpoDiasTipicosDTO = new DpoDiasTipicosDTO()
                {
                    Lunes = entityParametros.Pafunr1dtg1,
                    Martes = entityParametros.Pafunr1dtg2,
                    Miercoles = entityParametros.Pafunr1dtg3,
                    Jueves = entityParametros.Pafunr1dtg4,
                    Viernes = entityParametros.Pafunr1dtg5,
                    Sabado = entityParametros.Pafunr1dtg6,
                    Domingo = entityParametros.Pafunr1dtg7,
                };
            }
            #endregion

            #region Obtener los dias tipicos
            // Defino de la semana tipica y obtengo los grupos dias tipicos
            List<string> lstGruposDiasTipicos = ObtenerGruposDiasTipicos(dpoDiasTipicosDTO);

            // Defino los rangos de fecha apartir de los grupos de dias tipicos
            List<string> lstFechasDiasTipicos = ObtenerFechasGruposDiasTipicos(lstGruposDiasTipicos,
                                                                               anio,
                                                                               mesI,
                                                                               mesF);
            #endregion

            #region Filtra feriados a la información historica
            // Obtengo las fechas de los feriados filtradas por el año actual
            List<DateTime> feriados = this.ObtenerFeriadosPorAnio(DateTime.Now.Year);

            // Se remueven los días feriados del año que se encuentren en el rango de los dos meses seleccionados
            // los dias tipicos (Historicos - feriados = tipicos)
            lstDatos48 = lstDatos48.Where(x => !(feriados.Contains(x.Medifecha))).ToList();
            #endregion

            #region 2do Filtro de Información - Filtra por grupo de los dias tipicos a la información historica
            int iContadorGrupos = 0;

            // Recorro los grupos de fechas de dias tipicos y armo la lista filtrada por dias tipicos
            foreach (string fechaGrupo in lstFechasDiasTipicos)
            {
                decimal[] promedios = new decimal[ConstantesDpo.Itv30min];

                // Obtenego la lista filtrada por grupo de dias tipicos
                lstDatos48Parcial = ObtenerDataHistoricaGrupoDiaTipico(fechaGrupo, lstDatos48);

                if (iContadorGrupos == 0)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    lstDatos48A1 = lstDatos48A1.Where(x => x.Grupo == "G1").ToList();
                }

                if (iContadorGrupos == 1)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    lstDatos48A1 = lstDatos48A1.Where(x => x.Grupo == "G2").ToList();
                }

                if (iContadorGrupos == 2)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    lstDatos48A1 = lstDatos48A1.Where(x => x.Grupo == "G3").ToList();
                }

                if (iContadorGrupos == 3)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    lstDatos48A1 = lstDatos48A1.Where(x => x.Grupo == "G4").ToList();
                }

                if (iContadorGrupos == 4)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    lstDatos48A1 = lstDatos48A1.Where(x => x.Grupo == "G5").ToList();
                }

                if (iContadorGrupos == 5)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    lstDatos48A1 = lstDatos48A1.Where(x => x.Grupo == "G6").ToList();
                }

                if (iContadorGrupos == 6)
                {
                    // de la lista listPromedios TRAE solo un registro POR GRUPO
                    lstDatos48A1 = lstDatos48A1.Where(x => x.Grupo == "G7").ToList();
                }

                // Convierte la lista de promedios filtrada en array
                foreach (DpoMedicion48DTO item in lstDatos48A1)
                {
                    // Llena el array con los promedios
                    for (int k = 0; k < ConstantesDpo.Itv30min; k++)
                    {
                        promedios[k] = (decimal)item.GetType().GetProperty("H" + (k + 1)).GetValue(item);
                    }
                }

                // Corrijo la data historica
                foreach (DpoHistorico48DTO dato in lstDatos48Parcial)
                {
                    entityDpoMedicion48 = new DpoMedicion48DTO();

                    // Recorro las columnas de la lista
                    for (int i = 0; i < ConstantesDpo.Itv30min; i++)
                    {
                        decimal? periodoPrevio = 0;
                        if (i > 0)
                        {
                            periodoPrevio = (decimal)dato.GetType().GetProperty("H" + i).GetValue(dato);
                        }

                        decimal? periodoActual = (decimal)dato.GetType().GetProperty("H" + (i + 1)).GetValue(dato);


                        decimal? periodoSiguiente = 0;
                        if (i < 47)
                        {
                            periodoSiguiente = (decimal)dato.GetType().GetProperty("H" + (i + 2)).GetValue(dato);
                        }

                        if (i == 0) // rangoIncio
                        {
                            decimal? periodoH1 = (decimal)dato.GetType().GetProperty("H1").GetValue(dato);
                            decimal? periodoH47 = (decimal)dato.GetType().GetProperty("H47").GetValue(dato);
                            decimal? periodoH48 = (decimal)dato.GetType().GetProperty("H48").GetValue(dato);

                            if (periodoH1 == null || periodoH1 == 0)
                            {
                                decimal deltaRangoInicio = (decimal)((periodoH1 + 0) / (promedios[0] == 0 ? 1 : promedios[0]));
                                decimal deltaRangoFin = (decimal)((periodoH47 + periodoH48) / (promedios[47] == 0 ? 1 : promedios[47]));

                                decimal rangoFaltanteInicio = (decimal)(deltaRangoInicio + deltaRangoFin + promedios[0]);

                                entityDpoMedicion48.GetType().GetProperty($"H{(i + 1)}").SetValue(entityDpoMedicion48, rangoFaltanteInicio);
                            }
                            else
                            {
                                entityDpoMedicion48.GetType().GetProperty($"H{(i + 1)}").SetValue(entityDpoMedicion48, periodoH1);
                            }
                        }
                        else if (i == ConstantesDpo.Itv30min - 1) //  rangoFinal
                        {
                            if (periodoActual == null || periodoActual == 0)
                            {
                                if ((periodoPrevio == null || periodoPrevio == 0) && (periodoSiguiente == null || periodoSiguiente == 0))
                                {
                                    entityDpoMedicion48.GetType().GetProperty($"H{(i + 1)}").SetValue(entityDpoMedicion48, promedios[47]);
                                }
                                else
                                {
                                    decimal deltaRangoInicio = (decimal)((periodoPrevio + 0) / (promedios[0] == 0 ? 1 : promedios[0]));
                                    decimal deltaRangoFin = (decimal)((periodoActual + periodoPrevio) / (promedios[47] == 0 ? 1 : promedios[47]));

                                    decimal rangoFaltanteInicio = (decimal)(deltaRangoInicio + deltaRangoFin + promedios[0]);

                                    entityDpoMedicion48.GetType().GetProperty($"H{(i + 1)}").SetValue(entityDpoMedicion48, rangoFaltanteInicio);
                                }
                            }
                        }
                        else // rangoIntermedio
                        {
                            decimal? periodoFinalH47 = (decimal)dato.GetType().GetProperty("H47").GetValue(dato);
                            decimal? periodoFinalH48 = (decimal)dato.GetType().GetProperty("H48").GetValue(dato);

                            if (periodoFinalH48 == null || periodoFinalH48 == 0)
                            {
                                decimal deltaRangoInicio = (decimal)((periodoFinalH48 + periodoFinalH47) / (promedios[46] == 0 ? 1 : promedios[46]));
                                decimal deltaRangoFin = (decimal)((periodoFinalH48 + periodoFinalH47) / (promedios[i] == 0 ? 1 : promedios[i]));

                                decimal rangoFaltanteFinal = (decimal)(deltaRangoInicio + deltaRangoFin + promedios[i]);

                                entityDpoMedicion48.GetType().GetProperty($"H{(i + 1)}").SetValue(entityDpoMedicion48, rangoFaltanteFinal);
                            }
                            else
                            {
                                entityDpoMedicion48.GetType().GetProperty($"H{(i + 1)}").SetValue(entityDpoMedicion48, periodoFinalH48);
                            }
                        }
                    }

                    iContadorGrupos++;
                    
                    entityDpoMedicion48.Medifecha = dato.Medifecha;

                    lstDatos48Corregido.Add(entityDpoMedicion48);
                }
            }
            #endregion


            return lstDatos48Corregido;
        }

        /// <summary>
        /// R2: Reconstruir una serie de tiempo en base a otra serie de tiempo completa
        /// </summary>
        /// <param name="idCaso">Id del Caso</param>
        /// <param name="idFuncion">Id de la función de configuración</param>
        /// <param name="tipFuncion">Tipo función DM: DataMaestra; DP: DataProcesar</param>
        /// <param name="fecInicioFormula">Fecha inicio de la formula SCADA</param>
        /// <param name="fecFinFormula">Fecha fin de la formula SCADAr</param>
        /// <param name="lstDatos48">Data historica</param>
        /// <param name="lstDatos48A2">Data A1</param>
        /// <param name="lstDatos48F2">Data F2</param>
        /// <returns></returns>
        public List<DpoMedicion48DTO> ReconstruirSeriesTiempoR2(int idCaso,
                                                                int idFuncion,
                                                                string tipFuncion,
                                                                DateTime fecInicioFormula,
                                                                DateTime fecFinFormula,
                                                                List<DpoHistorico48DTO> lstDatos48,
                                                                List<List<DpoMedicion48DTO>> lstDatos48A2,
                                                                List<DpoMedicion48DTO> lstDatos48F2)
        {
            List<DpoMedicion48DTO> lstDatos48Corregido = new List<DpoMedicion48DTO>();
            List<DpoHistorico48DTO> lstDatos48Parcial = new List<DpoHistorico48DTO>();

            #region Carga de parametros
            // Obtengo los parametros de la configuracion de dias tipicos correspondiente a esta funcion A1
            List<DpoParametrosR2DTO> listParametrosR2 = FactorySic.GetDpoCasoDetalleRepository().GetParametrosR2(idCaso, idFuncion, tipFuncion);

            // Seteo la entidad para dias tipicos
            DpoDiasTipicosDTO dpoDiasTipicosDTO = null;

            foreach (DpoParametrosR2DTO entityParametros in listParametrosR2)
            {
                dpoDiasTipicosDTO = new DpoDiasTipicosDTO()
                {
                    Lunes = entityParametros.Pafunr2dtg1,
                    Martes = entityParametros.Pafunr2dtg2,
                    Miercoles = entityParametros.Pafunr2dtg3,
                    Jueves = entityParametros.Pafunr2dtg4,
                    Viernes = entityParametros.Pafunr2dtg5,
                    Sabado = entityParametros.Pafunr2dtg6,
                    Domingo = entityParametros.Pafunr2dtg7,
                };
            }
            #endregion

            #region Obtener los dias tipicos
            // Se establece el rango para dos meses hacia atras, apartir del mes actual para establecer los dias tipicos
            DateTime fecInicio = new DateTime(fecInicioFormula.Year, fecInicioFormula.Month, fecInicioFormula.Day, 0, 0, 0);
            DateTime fecFin = new DateTime(fecFinFormula.Year, fecFinFormula.Month, fecFinFormula.Day, 0, 0, 0);

            // Defino de la semana tipica y obtengo los grupos dias tipicos
            List<string> lstGruposDiasTipicos = ObtenerGruposDiasTipicos(dpoDiasTipicosDTO);

            // Defino los rangos de fecha apartir de los grupos de dias tipicos
            List<string> lstFechasDiasTipicos = ObtenerFechasGruposDiasTipicos(lstGruposDiasTipicos,
                                                                               fecInicio.Year,
                                                                               fecInicio.Month,
                                                                               fecFin.Month);
            #endregion

            #region Filtra feriados a la información historica
            // Obtengo las fechas de los feriados filtradas por el año actual
            List<DateTime> feriados = this.ObtenerFeriadosPorAnio(DateTime.Now.Year);

            // Se remueven los días feriados del año que se encuentren en el rango de los dos meses seleccionados
            // los dias tipicos (Historicos - feriados = tipicos)
            lstDatos48 = lstDatos48.Where(x => !(feriados.Contains(x.Medifecha))).ToList();
            #endregion


            return lstDatos48Corregido = lstDatos48F2;
        }

        /// <summary>
        ///  Devuelve el promedio total de una la serie o matriz de datos
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <param name="mediciones o numero de campos">Numero de Hs de la tabla</param>
        /// <returns>decimal[]</returns>
        public static decimal ObtenerPerfilPromedioMatriz(List<DpoHistorico48DTO> datos, int mediciones)
        {
            // Se declara variables acumuladoras
            decimal promedio = 0;
            decimal sumaTotal = 0;

            // Recorre la lista de n campos H y datos.Count filas para calcular la suma total de valores de las H
            int i = 0;
            foreach (DpoHistorico48DTO dato in datos)
            {
                // Calcula el promedio recorriendo los campos H de la clase y lo asigna al campo de indice i del array
                sumaTotal = sumaTotal + datos.Sum(m => (decimal)m.GetType().GetProperty("H" + (i + 1)).GetValue(m));
                i++;
            }

            if (datos.Count > 0)
                promedio = sumaTotal / (datos.Count * mediciones);
            else
                promedio = sumaTotal / (mediciones);

            // Devuelve el promedio total de la matriz
            return promedio;
        }

        /// <summary>
        ///  Devuelve el promedio total de una la serie o matriz de datos
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <param name="mediciones o numero de campos">Numero de Hs de la tabla</param>
        /// <returns>decimal[]</returns>
        public static decimal ObtenerPerfilPromedioMatriz(List<DpoHistorico96DTO> datos, int mediciones)
        {
            // Se declara variables acumuladoras
            decimal promedio = 0;
            decimal sumaTotal = 0;

            // Recorre la lista de n campos H y datos.Count filas para calcular la suma total de valores de las H
            int i = 0;
            foreach (DpoHistorico96DTO dato in datos)
            {
                // Calcula el promedio recorriendo los campos H de la clase y lo asigna al campo de indice i del array
                sumaTotal = sumaTotal + datos.Sum(m => (decimal)m.GetType().GetProperty("H" + (i + 1)).GetValue(m));
                i++;
            }

            promedio = sumaTotal / (datos.Count * mediciones);

            // Devuelve el promedio total de la matriz
            return promedio;
        }

        /// <summary>
        /// Devuelve el promedio de una la serie de tiempo completa o serie maestra a reconstruir
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <param name="mediciones o numero de campos">Numero de Hs de la tabla</param>
        /// <returns>decimal[]</returns>
        public static decimal[] ObtenerPerfilPromedio(List<DpoHistorico48DTO> datos, int mediciones)
        {
            // Se declara variable de tipo array de decimales n campos (mediciones) para los promedios
            decimal[] promedio = new decimal[mediciones];

            for (int i = 0; i < mediciones; i++)
            {
                // Calcula el promedio recorriendo los campos H de la clase y lo asigna al campo de indice i del array
                promedio[i] = datos.Average(m => (decimal)m.GetType().GetProperty("H" + (i + 1)).GetValue(m));
            }

            // Devuelve el array con los promedios calculados en la iteracion anterior
            return promedio;
        }

        /// <summary>
        /// Devuelve el promedio de una la serie de tiempo completa o serie maestra a reconstruir
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <param name="mediciones o numero de campos">Numero de Hs de la tabla</param>
        /// <returns>decimal[]</returns>
        public static decimal[] ObtenerPerfilPromedio(List<DpoHistorico96DTO> datos, int mediciones)
        {
            // Se declara variable de tipo array de decimales n campos (mediciones) para los promedios
            decimal[] promedio = new decimal[mediciones];

            for (int i = 0; i < mediciones; i++)
            {
                // Calcula el promedio recorriendo los campos H de la clase y lo asigna al campo de indice i del array
                promedio[i] = datos.Average(m => (decimal)m.GetType().GetProperty("H" + (i + 1)).GetValue(m));
            }

            // Devuelve el array con los promedios calculados en la iteracion anterior
            return promedio;
        }

        /// <summary>
        /// Devuelve la desviación estandar de las mediciones de un día típico
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <param name="mediciones o numero de campos">Numero de Hs de la tabla</param>
        /// <returns>decimal[]</returns>
        public static decimal[] ObtenerPerfilDesviacionEstandar(List<DpoHistorico48DTO> datos, int mediciones)
        {
            // Se declara variable de tipo array de decimales n campos (mediciones) para las desviaciones estandar
            decimal[] desviacionEstandar = new decimal[mediciones];


            // Recorre las columnas H de la lista y calcula el promedio de dichas columnas H de n filas que tiene la lista
            for (int i = 0; i < mediciones; i++)
            {
                // Calcula la media o el promedio del campo H recorriendo los todos campos H de la clase y lo
                // asigna al campo de indice i del array
                decimal media = datos.Average(m => (decimal)m.GetType().GetProperty("H" + (i + 1)).GetValue(m));

                // Calcular la suma de los cuadrados de las diferencias con la media
                decimal sumaCuadradosDiferencias = (decimal)datos.Sum(m => Math.Pow((double)((decimal)m.GetType().GetProperty("H" + (i + 1)).GetValue(m) - media), 2));

                // Calcular la varianza
                decimal varianza = sumaCuadradosDiferencias / datos.Count;

                // Calcular la desviación estándar
                desviacionEstandar[i] = (decimal)Math.Sqrt((double)varianza);
            }

            // Devuelve el array con los promedios calculados en la iteracion anterior
            return desviacionEstandar;
        }

        /// <summary>
        /// Devuelve la desviación estandar de las mediciones de un día típico
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <param name="mediciones o numero de campos">Numero de Hs de la tabla</param>
        /// <returns>decimal[]</returns>
        public static decimal[] ObtenerPerfilDesviacionEstandar(List<DpoHistorico96DTO> datos, int mediciones)
        {
            // Se declara variable de tipo array de decimales n campos (mediciones) para las desviaciones estandar
            decimal[] desviacionEstandar = new decimal[mediciones];


            // Recorre las columnas H de la lista y calcula el promedio de dichas columnas H de n filas que tiene la lista
            for (int i = 0; i < mediciones; i++)
            {
                // Calcula la media o el promedio del campo H recorriendo los todos campos H de la clase y lo
                // asigna al campo de indice i del array
                decimal media = datos.Average(m => (decimal)m.GetType().GetProperty("H" + (i + 1)).GetValue(m));

                // Calcular la suma de los cuadrados de las diferencias con la media
                decimal sumaCuadradosDiferencias = (decimal)datos.Sum(m => Math.Pow((double)((decimal)m.GetType().GetProperty("H" + (i + 1)).GetValue(m) - media), 2));

                // Calcular la varianza
                decimal varianza = sumaCuadradosDiferencias / datos.Count;

                // Calcular la desviación estándar
                desviacionEstandar[i] = (decimal)Math.Sqrt((double)varianza);
            }

            // Devuelve el array con los promedios calculados en la iteracion anterior
            return desviacionEstandar;
        }

        /// <summary>
        /// Devuelve los grupos de los dias tipicos de manera dinamica
        /// </summary>
        /// <param name="dpoDiasTipicosDTO">Entidad de dias tipicos</param>
        /// <returns>List string</returns>
        public static List<string> ObtenerGruposDiasTipicos(DpoDiasTipicosDTO dpoDiasTipicosDTO)
        {
            List<string> gruposDiasTipicos = new List<string>();

            // Crear un diccionario para almacenar los valores y los atributos correspondientes
            Dictionary<string, List<string>> atributosPorValor = new Dictionary<string, List<string>>();

            // Obtener las propiedades de la clase
            PropertyInfo[] properties = typeof(DpoDiasTipicosDTO).GetProperties();

            // Recorrer las propiedades y almacenar los atributos para cada valor
            foreach (PropertyInfo property in properties)
            {
                string valor = property.GetValue(dpoDiasTipicosDTO)?.ToString();

                if (valor != null)
                {
                    if (atributosPorValor.ContainsKey(valor))
                    {
                        atributosPorValor[valor].Add(property.Name);
                    }
                    else
                    {
                        atributosPorValor[valor] = new List<string> { property.Name };
                    }
                }
            }

            // Imprimir los resultados
            foreach (var kvp in atributosPorValor)
            {
                // Agrupa los los valores por atributo
                string atributos = string.Join(", ", kvp.Value);

                gruposDiasTipicos.Add(atributos);
            }

            // El rango: Lunes, Martes
            // El rango: Miercoles, Jueves, Viernes
            // El rango: Sabado, Domingo

            return gruposDiasTipicos;
        }

        /// <summary>
        /// Devuelve los rangos apartir de los dias tipicos
        /// </summary>
        /// <param name="gruposDiasTipicos">Lista de grupos de dias tipicos</param>
        /// <param name="anio">Año actual</param>
        /// <param name="mesInicio">Mes de inicio del año actual</param>
        /// <param name="mesFin">Mes de fin del año actual</param>
        /// <returns>List string de las fechas del grupo del dia tipico</returns>
        public static List<string> ObtenerFechasGruposDiasTipicos(List<string> gruposDiasTipicos, int anio, int mesInicio, int mesFin)
        {
            List<string> rangosTmp = null;
            List<string> fechasGrupoDiasTipicos = new List<string>();

            // gruposDiasTipicos[0] = Lunes, Martes  
            // gruposDiasTipicos[1] = Miercoles, Jueves, Viernes
            // gruposDiasTipicos[2] = Sabado, Domingo

            // Recorre la lista de dias tipicos
            foreach (string rango in gruposDiasTipicos)
            {
                // Separa los dias de los grupos de los dias tipicos
                string[] diasTipicosRango = rango.Split(new char[] { ',', '.' });

                // gruposDiasTipicos[0]
                // diasTipicosRango[0]: Lunes
                // diasTipicosRango[1]: Martes

                // gruposDiasTipicos[1]
                // diasTipicosRango[0]: Miercoles
                // diasTipicosRango[1]: Jueves
                // diasTipicosRango[2]: Viernes

                // gruposDiasTipicos[2]
                // diasTipicosRango[0]: Sabado
                // diasTipicosRango[1]: Domingo

                //DateTime ultDiaMesFin;
                //DateTime primDiaMesInicio;

                int amplitudRango = mesFin - mesInicio; // mesInicio = 3, mesFin = 5 ==> amplitudRango = 2

                rangosTmp = new List<string>();

                // Recorre los dias de los  grupos de dias tipicos
                for (int i = 0; i <= diasTipicosRango.Length - 1; i++)
                {
                    for (int j = mesInicio; j <= mesInicio + amplitudRango; j++)
                    {
                        if (diasTipicosRango[i].Trim() == "Lunes")
                        {
                            rangosTmp.Add(ObtenerFechasDelDiaDelMesAño(anio, j, DayOfWeek.Monday));
                        }
                        else if (diasTipicosRango[i].Trim() == "Martes")
                        {
                            rangosTmp.Add(ObtenerFechasDelDiaDelMesAño(anio, j, DayOfWeek.Tuesday));
                        }
                        else if (diasTipicosRango[i].Trim() == "Miercoles")
                        {
                            rangosTmp.Add(ObtenerFechasDelDiaDelMesAño(anio, j, DayOfWeek.Wednesday));
                        }
                        else if (diasTipicosRango[i].Trim() == "Jueves")
                        {
                            rangosTmp.Add(ObtenerFechasDelDiaDelMesAño(anio, j, DayOfWeek.Thursday));
                        }
                        else if (diasTipicosRango[i].Trim() == "Viernes")
                        {
                            rangosTmp.Add(ObtenerFechasDelDiaDelMesAño(anio, j, DayOfWeek.Friday));
                        }
                        else if (diasTipicosRango[i].Trim() == "Sabado")
                        {
                            rangosTmp.Add(ObtenerFechasDelDiaDelMesAño(anio, j, DayOfWeek.Saturday));
                        }
                        else if (diasTipicosRango[i].Trim() == "Domingo")
                        {
                            rangosTmp.Add(ObtenerFechasDelDiaDelMesAño(anio, j, DayOfWeek.Sunday));
                        }
                    }
                }

                // Conmvierte la lista rangosTmp en una cadena separada por comas
                string cadena = string.Join(", ", rangosTmp);

                fechasGrupoDiasTipicos.Add(cadena);
            }

            // fechasDias[0] = <<fechas separadas de Lunes y Martes separadas x comas>>
            // fechasDias[1] = <<fechas separadas de Miercoles, jueves y Viernes separadas x comas>>
            // fechasDias[2] = <<fechas separadas de Sabado y Domingo separadas x comas>>

            return fechasGrupoDiasTipicos;
        }

        /// <summary>
        /// Devuelve todas las fechas del dia D del YYYY y MM
        /// </summary>
        /// <param name="year">Año actual</param>
        /// <param name="month">Mes del año actual</param>
        /// <param name="diaSemana">dia de la semana</param>
        /// <returns>las fechas de un dia espcifico del mes seaparado por comas</returns>
        public static string ObtenerFechasDelDiaDelMesAño(int year, int month, DayOfWeek diaSemana)
        {
            List<string> listFechasDiaOfMonthYear = new List<string>();

            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(year, month, day);
                if (date.DayOfWeek == diaSemana)
                {
                    listFechasDiaOfMonthYear.Add(date.ToString("dd/MM/yyyy"));
                }
            }

            string cadena = string.Join(", ", listFechasDiaOfMonthYear);

            return cadena;
        }

        /// <summary>
        /// Devuelve el primer dia del mes
        /// </summary>
        /// <param name="anio">Año actual</param>
        /// <param name="mes">Mes del año actual</param>
        /// <param name="diaSemana">dia de la semana</param>
        /// <returns>DateTime</returns>
        public static DateTime ObtenerPrimerDiaAñoMesDia(int anio, int mes, DayOfWeek diaSemana)
        {
            DayOfWeek targetDayOfWeek = diaSemana;

            // Obtener el primer día del mes y año especificados
            DateTime firstDayOfMonth = new DateTime(anio, mes, 1);

            // Obtener el día de la semana del primer día del mes
            DayOfWeek firstDayOfWeek = firstDayOfMonth.DayOfWeek;

            // Calcular la cantidad de días para llegar al día objetivo
            int daysToAdd = (targetDayOfWeek - firstDayOfWeek + 7) % 7;

            // Obtener el primer día del mes que corresponde al día objetivo
            DateTime firstTargetDayOfMonth = firstDayOfMonth.AddDays(daysToAdd);


            return firstTargetDayOfMonth;
        }

        /// <summary>
        /// Devuelve el ultimo dia del mes
        /// </summary>
        /// <param name="anio">Año actual</param>
        /// <param name="mes">Mes del año actual</param>
        /// <param name="diaSemana">dia de la semana</param>
        /// <returns>DateTime</returns>
        public static DateTime ObtenerUltimoDiaAñoMesDia(int anio, int mes, DayOfWeek diaSemana)
        {
            DayOfWeek dayOfWeek = diaSemana;

            // Obtener el primer día del mes y año especificados
            DateTime firstDayOfMonth = new DateTime(anio, mes, 1);

            // Obtener el último día del mes especificado
            int lastDayOfMonth = DateTime.DaysInMonth(anio, mes);

            // Buscar el último día que coincide con el día de la semana especificado
            DateTime lastDayOfWeek = firstDayOfMonth.AddDays(lastDayOfMonth - 1);
            while (lastDayOfWeek.DayOfWeek != dayOfWeek)
            {
                lastDayOfWeek = lastDayOfWeek.AddDays(-1);
            }


            return lastDayOfWeek;
        }

        /// <summary>
        /// Método que permite obtener la medición calculada
        /// </summary>
        /// <param name="idCalculado">Identificador Ptomedicalc de la tabla ME_PTOMEDICION</param>
        /// <param name="medifecha">Fecha del registro</param>
        /// <returns></returns>
        public decimal[] ObtenerMedicionesCalculadas(int idCalculado, DateTime medifecha)
        {
            COES.Servicios.Aplicacion.Scada.PerfilScadaServicio servicio = new Scada.PerfilScadaServicio();
            decimal[] arrayMedicion = new decimal[ConstantesProdem.Itv30min];
            List<MeMedicion48DTO> listaMeMedicion = new List<MeMedicion48DTO>();

            try
            {
                listaMeMedicion = servicio.ProcesarFormula(idCalculado, medifecha);
                int i = 0;
                while (i < ConstantesProdem.Itv30min)
                {
                    foreach (var a in listaMeMedicion)
                    {
                        var dValor = a.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(a, null);
                        arrayMedicion[i] += (decimal)dValor;
                    }
                    i++;
                }
            }
            catch
            {
                return arrayMedicion;
            }

            return arrayMedicion;
        }

        /// <summary>
        /// Permite generar el reporte en formato Excel de archivos Raw no procesados
        /// </summary>
        /// <param name="lstDatosCorregidosDm48">Lista</param>
        /// <param name="lstDatosCorregidosDp48">Lista</param>
        /// <param name="path">Ruta del archivo Excel</param>
        /// <param name="fileName">Nombre del archivo Excel</param>
        public string ExportToExcelDataCorregida(List<DpoMedicion48DTO> lstDatosCorregidosDm48,
                                                 List<DpoMedicion48DTO> lstDatosCorregidosDp48,
                                                 string path,
                                                 string fileName)
        {
            // Se declara la variable de ruta del archivo a generar
            string file = path + fileName;

            // Se declara la variable de objeto archivo Excel
            FileInfo newFile = new FileInfo(file);

            // Se valida si ya existe el archivo generado
            if (newFile.Exists)
            {
                // Si existe se elimina el archivo
                newFile.Delete();

                // Se refresca la instancia la varible de objeto archivo Excel
                newFile = new FileInfo(file);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                // --------------------------------------------------------------------------------------------------------------
                // Se crea el libro de data maestra corregida
                // --------------------------------------------------------------------------------------------------------------
                #region Se procesa el contenido de la lista de data maestra corregida
                ExcelWorksheet wsDm = xlPackage.Workbook.Worksheets.Add("DATA MAESTRA");

                if (wsDm != null)
                {
                    // Se coloca el titulo de la hoja creada
                    wsDm.Cells[2, 2].Value = "REPORTE DE DATA MAESTRA CORREGIDA";

                    ExcelRange rgDm = wsDm.Cells[2, 2, 2, 2];
                    rgDm.Style.Font.Size = 13;
                    rgDm.Style.Font.Bold = true;

                    // Establece el indice o celda inicial donde se colocara el contenido de las cabeceras en la hoja
                    int indexDm = 5;

                    // Se crean las cabeceras
                    wsDm.Cells[indexDm, 2].Value = "Fecha";
                    for (int i = 3; i <= 50; i++)
                    {
                        wsDm.Cells[indexDm, i].Value = "H" + (i - 2);
                    }

                    // Se formatea las cabeceras
                    rgDm = wsDm.Cells[indexDm, 2, indexDm, 50];
                    rgDm.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rgDm.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rgDm.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rgDm.Style.Font.Color.SetColor(Color.White);
                    rgDm.Style.Font.Size = 10;
                    rgDm.Style.Font.Bold = true;

                    // Establece el indice o celda inicial donde se colocara el contenido de la data maestra en la hoja
                    indexDm = 6;

                    // Se inserta el contenido de la lista de data maestra
                    foreach (DpoMedicion48DTO item in lstDatosCorregidosDm48)
                    {
                        wsDm.Cells[indexDm, 2].Value = ((DateTime)item.Medifecha).ToString(ConstantesAppServicio.FormatoFechaHora);
                        
                        // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                        for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                        {
                            // corrige el dato de la columna Hi
                            wsDm.Cells[indexDm, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                        }

                        // Se formatea el contenido de las celdas que contiene la data maestra
                        rgDm = wsDm.Cells[indexDm, 2, indexDm, 50];
                        rgDm.Style.Font.Size = 10;
                        rgDm.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                        indexDm++;
                    }

                    // Se establece el formato general del libro
                    rgDm = wsDm.Cells[5, 2, indexDm - 1, 50];
                    rgDm.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rgDm.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDm.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rgDm.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDm.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rgDm.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDm.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rgDm.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    for (int i = 2; i <= 50; i++)
                    {
                        wsDm.Column(i).Width = 80;
                    }

                    rgDm = wsDm.Cells[5, 3, indexDm, 50];
                    rgDm.AutoFitColumns();
                }
                #endregion

                // --------------------------------------------------------------------------------------------------------------
                // Se crea el libro de data a procesar corregida
                // --------------------------------------------------------------------------------------------------------------
                #region Se procesa el contenido de la lista de data a procesar
                ExcelWorksheet wsDp = xlPackage.Workbook.Worksheets.Add("DATA A PROCESAR");

                if (wsDp != null)
                {
                    // Se coloca el titulo de la hoja creada
                    wsDp.Cells[2, 2].Value = "REPORTE DE DATA A PROCESAR CORREGIDA";

                    ExcelRange rgDp = wsDp.Cells[2, 2, 2, 2];
                    rgDp.Style.Font.Size = 13;
                    rgDp.Style.Font.Bold = true;

                    // Establece el indice o celda inicial donde se colocara el contenido de las cabeceras en la hoja
                    int indexDp = 5;

                    // Se crean las cabeceras
                    wsDp.Cells[indexDp, 2].Value = "Fecha";
                    for (int i = 3; i <= 50; i++)
                    {
                        wsDp.Cells[indexDp, i].Value = "H" + (i - 2);
                    }

                    // Se formatea las cabeceras
                    rgDp = wsDp.Cells[indexDp, 2, indexDp, 50];
                    rgDp.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rgDp.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rgDp.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rgDp.Style.Font.Color.SetColor(Color.White);
                    rgDp.Style.Font.Size = 10;
                    rgDp.Style.Font.Bold = true;

                    // Establece el indice o celda inicial donde se colocara el contenido de la data maestra en la hoja
                    indexDp = 6;

                    // Se inserta el contenido de la lista de data maestra
                    foreach (DpoMedicion48DTO item in lstDatosCorregidosDp48)
                    {
                        wsDp.Cells[indexDp, 2].Value = ((DateTime)item.Medifecha).ToString(ConstantesAppServicio.FormatoFechaHora);

                        // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                        for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                        {
                            // corrige el dato de la columna Hi
                            wsDp.Cells[indexDp, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                        }

                        // Se formatea el contenido de las celdas que contiene la data maestra
                        rgDp = wsDp.Cells[indexDp, 2, indexDp, 50];
                        rgDp.Style.Font.Size = 10;
                        rgDp.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                        indexDp++;
                    }

                    // Se establece el formato general del libro
                    rgDp = wsDp.Cells[5, 2, indexDp - 1, 50];
                    rgDp.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rgDp.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDp.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rgDp.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDp.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rgDp.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDp.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rgDp.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    for (int i = 2; i <= 50; i++)
                    {
                        wsDp.Column(i).Width = 80;
                    }

                    rgDp = wsDp.Cells[5, 3, indexDp, 50];
                    rgDp.AutoFitColumns();


                }
                #endregion

                // Extrae el logo del COES
                //HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                //ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                //picture.From.Column = 1;
                //picture.From.Row = 1;
                //picture.To.Column = 2;
                //picture.To.Row = 2;
                //picture.SetSize(120, 35);

                xlPackage.Save();
            }

            string Reporte = fileName;

            return Reporte;
        }


        /// <summary>
        /// Permite generar el reporte en formato Excel de archivos Raw no procesados
        /// </summary>
        /// <param name="lstDatosHistoricosDm48">Lista</param>
        /// <param name="lstDatosCorregidosDmR148">Lista</param>
        /// <param name="lstDatosCorregidosDmR248">Lista</param>
        /// <param name="lstDatosCorregidosDmF148">Lista</param>
        /// <param name="lstDatosCorregidosDmF248">Lista</param>
        /// <param name="lstDatosCorregidosDmA148">Lista</param>
        /// <param name="lstDatosCorregidosDmA248">Lista</param>
        /// <param name="lstDatosHistoricosDp48">Lista</param>
        /// <param name="lstDatosCorregidosDpR148">Lista</param>
        /// <param name="lstDatosCorregidosDpR248">Lista</param>
        /// <param name="lstDatosCorregidosDpF148">Lista</param>
        /// <param name="lstDatosCorregidosDpF248">Lista</param>
        /// <param name="lstDatosCorregidosDpA148">Lista</param>
        /// <param name="lstDatosCorregidosDpA248">Lista</param>
        /// <param name="path">Ruta del archivo Excel</param>
        /// <param name="fileName">Nombre del archivo Excel</param>
        public string ExportToExcelDataCorregida2(List<DpoHistorico48DTO> lstDatosHistoricosDm48,
                                                  List<DpoMedicion48DTO> lstDatosCorregidosDmR148,
                                                  List<DpoMedicion48DTO> lstDatosCorregidosDmR248,
                                                  List<DpoMedicion48DTO> lstDatosCorregidosDmF148,
                                                  List<DpoMedicion48DTO> lstDatosCorregidosDmF248,
                                                  List<DpoMedicion48DTO> lstDatosCorregidosDmA148,
                                                  List<List<DpoMedicion48DTO>> lstDatosCorregidosDmA248,
                                                  List<DpoHistorico48DTO> lstDatosHistoricosDp48,
                                                  List<DpoMedicion48DTO> lstDatosCorregidosDpR148,
                                                  List<DpoMedicion48DTO> lstDatosCorregidosDpR248,
                                                  List<DpoMedicion48DTO> lstDatosCorregidosDpF148,
                                                  List<DpoMedicion48DTO> lstDatosCorregidosDpF248,
                                                  List<DpoMedicion48DTO> lstDatosCorregidosDpA148,
                                                  List<List<DpoMedicion48DTO>> lstDatosCorregidosDpA248,
                                                  string path,
                                                  string fileName)
        {
            // Se declara la variable de ruta del archivo a generar
            string file = path + fileName;

            // Se declara la variable de objeto archivo Excel
            FileInfo newFile = new FileInfo(file);

            // Se valida si ya existe el archivo generado
            if (newFile.Exists)
            {
                // Si existe se elimina el archivo
                newFile.Delete();

                // Se refresca la instancia la varible de objeto archivo Excel
                newFile = new FileInfo(file);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                // --------------------------------------------------------------------------------------------------------------
                // Se crea el libro de data maestra corregida
                // --------------------------------------------------------------------------------------------------------------
                #region Se procesa el contenido de la lista de data maestra corregida
                ExcelWorksheet wsDm = xlPackage.Workbook.Worksheets.Add("DATA MAESTRA");

                if (wsDm != null)
                {
                    ExcelRange rgDm;

                    #region Titulos
                    // Se coloca los titulos de la hoja creada
                    wsDm.Cells[2, 2].Value = "REPORTE DE DATA MAESTRA CORREGIDA";
                    rgDm = wsDm.Cells[2, 2, 2, 2];
                    rgDm.Style.Font.Size = 13;
                    rgDm.Style.Font.Bold = true;

                    wsDm.Cells[4, 2].Value = "DATA HISTORICA";
                    rgDm = wsDm.Cells[4, 2, 4, 2];
                    rgDm.Style.Font.Size = 13;
                    rgDm.Style.Font.Bold = true;
                    #endregion

                    #region Data historica-maestra
                    // Establece el indice o celda inicial donde se colocara el contenido de la data-historica-maestra
                    // de las cabeceras en la hoja
                    int indexDm = 5;

                    // Se crean las cabeceras
                    wsDm.Cells[indexDm, 2].Value = "FECHA";
                    for (int i = 3; i <= 50; i++)
                    {
                        wsDm.Cells[indexDm, i].Value = "H" + (i - 2);
                    }

                    // Se formatea las cabeceras
                    rgDm = wsDm.Cells[indexDm, 2, indexDm, 50];
                    rgDm.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rgDm.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rgDm.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rgDm.Style.Font.Color.SetColor(Color.White);
                    rgDm.Style.Font.Size = 10;
                    rgDm.Style.Font.Bold = true;

                    // Establece el indice o celda inicial donde se colocara el contenido de la data-historica-maestra en la hoja
                    indexDm = 6;

                    // Se inserta el contenido de la lista de data historica - maestra
                    foreach (DpoHistorico48DTO item in lstDatosHistoricosDm48)
                    {
                        wsDm.Cells[indexDm, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");

                        // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                        for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                        {
                            // corrige el dato de la columna Hi
                            wsDm.Cells[indexDm, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                        }

                        // Se formatea el contenido de las celdas que contiene la data maestra
                        rgDm = wsDm.Cells[indexDm, 2, indexDm, 50];
                        rgDm.Style.Font.Size = 10;
                        rgDm.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                        indexDm++;
                    }

                    // Se establece el formato general del libro
                    rgDm = wsDm.Cells[5, 2, indexDm - 1, 50];
                    rgDm.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rgDm.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDm.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rgDm.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDm.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rgDm.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDm.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rgDm.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    for (int i = 2; i <= 50; i++)
                    {
                        wsDm.Column(i).Width = 80;
                    }

                    rgDm = wsDm.Cells[5, 3, indexDm, 50];
                    rgDm.AutoFitColumns();
                    #endregion

                    if (lstDatosHistoricosDm48.Count > 0)
                    {
                        #region Data maestra - A1
                        int indexDmA1 = 0;

                        if (lstDatosCorregidosDmA148.Count > 0)
                        {
                            // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-maestra-A1 en la hoja
                            indexDmA1 = indexDm + 2;

                            // Se coloca el titulo de la función en hoja creada
                            wsDm.Cells[indexDmA1, 2].Value = "DATA - FUNCIÓN A1";
                            rgDm = wsDm.Cells[indexDmA1, 2, indexDmA1, 2];
                            rgDm.Style.Font.Size = 13;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDmA1 = indexDmA1 + 1;

                            // Se crean las cabeceras
                            wsDm.Cells[indexDmA1, 2].Value = "GRUPO";
                            for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                            {
                                wsDm.Cells[indexDmA1, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDm = wsDm.Cells[indexDmA1, 2, indexDmA1, 50];
                            rgDm.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDm.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDm.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDm.Style.Font.Color.SetColor(Color.White);
                            rgDm.Style.Font.Size = 10;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDmA1 = indexDmA1 + 1;

                            // Se inserta el contenido de la lista de data-procesar-A1
                            foreach (DpoMedicion48DTO item in lstDatosCorregidosDmA148)
                            {
                                wsDm.Cells[indexDmA1, 2].Value = item.Grupo;

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDm.Cells[indexDmA1, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDm = wsDm.Cells[indexDmA1, 2, indexDmA1, 50];
                                rgDm.Style.Font.Size = 10;
                                rgDm.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDmA1++;
                            }

                            // Se establece el formato general del libro
                            rgDm = wsDm.Cells[5, 2, indexDmA1 - 1, 50];
                            rgDm.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDm.Column(i).Width = 80;
                            }

                            rgDm = wsDm.Cells[indexDmA1, 3, indexDmA1, 50];
                            rgDm.AutoFitColumns();
                        }
                        #endregion

                        #region Data maestra - A2
                        int indexDmA2Prom = 0;
                        int indexDmA2Desv = 0;

                        if (lstDatosCorregidosDmA248.Count > 0)
                        {
                            // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-maestra - A2 en la hoja
                            if (lstDatosCorregidosDmA148.Count > 0)
                            {
                                indexDmA2Prom = indexDmA1 + 2;
                            }
                            else
                            {
                                indexDmA2Prom = indexDm + 2;
                            }

                            // Se coloca el titulo de la función en hoja creada
                            wsDm.Cells[indexDmA2Prom, 2].Value = "DATA - FUNCIÓN A2 - PROMEDIOS";
                            rgDm = wsDm.Cells[indexDmA2Prom, 2, indexDmA2Prom, 2];
                            rgDm.Style.Font.Size = 13;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDmA2Prom = indexDmA2Prom + 1;

                            // Se crean las cabeceras
                            wsDm.Cells[indexDmA2Prom, 2].Value = "GRUPO";
                            for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                            {
                                wsDm.Cells[indexDmA2Prom, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDm = wsDm.Cells[indexDmA2Prom, 2, indexDmA2Prom, 50];
                            rgDm.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDm.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDm.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDm.Style.Font.Color.SetColor(Color.White);
                            rgDm.Style.Font.Size = 10;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-A2 en la hoja
                            indexDmA2Prom = indexDmA2Prom + 1;

                            // Se inserta el contenido de la lista de data-procesar-A1
                            List<DpoMedicion48DTO> sublistaPromediosDmA248 = lstDatosCorregidosDmA248[0];
                            foreach (DpoMedicion48DTO item in sublistaPromediosDmA248)
                            {
                                wsDm.Cells[indexDmA2Prom, 2].Value = item.Grupo;

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDm.Cells[indexDmA2Prom, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDm = wsDm.Cells[indexDmA2Prom, 2, indexDmA2Prom, 50];
                                rgDm.Style.Font.Size = 10;
                                rgDm.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDmA2Prom++;
                            }

                            // Se establece el formato general del libro
                            rgDm = wsDm.Cells[5, 2, indexDmA2Prom - 1, 50];
                            rgDm.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDm.Column(i).Width = 80;
                            }

                            rgDm = wsDm.Cells[indexDmA2Prom, 3, indexDmA2Prom, 50];
                            rgDm.AutoFitColumns();
                            // ---------------------------------------------------------------------------------------------------

                            // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-maestra-A2 en la hoja
                            if (sublistaPromediosDmA248.Count > 0)
                            {
                                indexDmA2Desv = indexDmA2Prom + 2;
                            }
                            else if (lstDatosCorregidosDmA148.Count > 0)
                            {
                                indexDmA2Desv = indexDmA1 + 2;
                            }
                            else
                            {
                                indexDmA2Desv = indexDm + 2;
                            }

                            // Se coloca el titulo de la función en hoja creada
                            wsDm.Cells[indexDmA2Desv, 2].Value = "DATA - FUNCIÓN A2 - DESVIACIONES";
                            rgDm = wsDm.Cells[indexDmA2Desv, 2, indexDmA2Desv, 2];
                            rgDm.Style.Font.Size = 13;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-A2 en la hoja
                            indexDmA2Desv = indexDmA2Desv + 1;

                            // Se crean las cabeceras
                            wsDm.Cells[indexDmA2Desv, 2].Value = "GRUPO";
                            for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                            {
                                wsDm.Cells[indexDmA2Desv, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDm = wsDm.Cells[indexDmA2Desv, 2, indexDmA2Desv, 50];
                            rgDm.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDm.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDm.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDm.Style.Font.Color.SetColor(Color.White);
                            rgDm.Style.Font.Size = 10;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-A2 en la hoja
                            indexDmA2Desv = indexDmA2Desv + 1;

                            // Se inserta el contenido de la lista de data-procesar-A1
                            List<DpoMedicion48DTO> sublistaDesviacionesDmA248 = lstDatosCorregidosDmA248[1];
                            foreach (DpoMedicion48DTO item in sublistaDesviacionesDmA248)
                            {
                                wsDm.Cells[indexDmA2Desv, 2].Value = item.Grupo;

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDm.Cells[indexDmA2Desv, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDm = wsDm.Cells[indexDmA2Desv, 2, indexDmA2Desv, 50];
                                rgDm.Style.Font.Size = 10;
                                rgDm.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDmA2Desv++;
                            }

                            // Se establece el formato general del libro
                            rgDm = wsDm.Cells[5, 2, indexDmA2Desv - 1, 50];
                            rgDm.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDm.Column(i).Width = 80;
                            }

                            rgDm = wsDm.Cells[indexDmA2Desv, 3, indexDmA2Desv, 50];
                            rgDm.AutoFitColumns();
                        }
                        #endregion

                        #region Data maestra - F1
                        // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-maestra-F1 en la hoja
                        int indexDmF1 = 0;

                        if (lstDatosCorregidosDmF148.Count > 0)
                        {
                            if (lstDatosCorregidosDmA248.Count > 0)
                            {
                                indexDmF1 = indexDmA2Desv + 2;
                            }
                            else if (lstDatosCorregidosDmA148.Count > 0)
                            {
                                indexDmF1 = indexDmA1 + 2;
                            }
                            else
                            {
                                indexDmF1 = indexDm + 2;
                            }

                            // Se coloca el titulo de la función en hoja creada
                            wsDm.Cells[indexDmF1, 2].Value = "DATA - FUNCIÓN F1";
                            rgDm = wsDm.Cells[indexDmF1, 2, indexDmF1, 2];
                            rgDm.Style.Font.Size = 13;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDmF1 = indexDmF1 + 1;

                            // Se crean las cabeceras
                            wsDm.Cells[indexDmF1, 2].Value = "FECHA";
                            for (int i = 3; i <= 50; i++)
                            {
                                wsDm.Cells[indexDmF1, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDm = wsDm.Cells[indexDmF1, 2, indexDmF1, 50];
                            rgDm.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDm.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDm.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDm.Style.Font.Color.SetColor(Color.White);
                            rgDm.Style.Font.Size = 10;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDmF1 = indexDmF1 + 1;

                            // Se inserta el contenido de la lista de data-procesar-F1
                            foreach (DpoMedicion48DTO item in lstDatosCorregidosDmF148)
                            {
                                wsDm.Cells[indexDmF1, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDm.Cells[indexDmF1, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDm = wsDm.Cells[indexDmF1, 2, indexDmF1, 50];
                                rgDm.Style.Font.Size = 10;
                                rgDm.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDmF1++;
                            }

                            // Se establece el formato general del libro
                            rgDm = wsDm.Cells[5, 2, indexDmF1 - 1, 50];
                            rgDm.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDm.Column(i).Width = 80;
                            }

                            rgDm = wsDm.Cells[indexDmF1, 3, indexDmF1, 50];
                            rgDm.AutoFitColumns();
                        }
                        #endregion

                        #region Data maestra - F2
                        // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-maestra-F2 en la hoja
                        int indexDmF2 = 0;

                        if (lstDatosCorregidosDmF248.Count > 0)
                        {
                            if (lstDatosCorregidosDmA248.Count > 0)
                            {
                                indexDmF2 = indexDmA2Desv + 2;
                            }
                            else if (lstDatosCorregidosDmA148.Count > 0)
                            {
                                indexDmF2 = indexDmA1 + 2;
                            }
                            else if (lstDatosCorregidosDmF148.Count > 0)
                            {
                                indexDmF2 = indexDmF1 + 2;
                            }
                            else
                            {
                                indexDmF2 = indexDm + 2;
                            }

                            // Se coloca el titulo de la función en hoja creada
                            wsDm.Cells[indexDmF2, 2].Value = "DATA - FUNCIÓN F2";
                            rgDm = wsDm.Cells[indexDmF2, 2, indexDmF2, 2];
                            rgDm.Style.Font.Size = 13;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDmF2 = indexDmF2 + 1;

                            // Se crean las cabeceras
                            wsDm.Cells[indexDmF2, 2].Value = "FECHA";
                            for (int i = 3; i <= 50; i++)
                            {
                                wsDm.Cells[indexDmF2, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDm = wsDm.Cells[indexDmF2, 2, indexDmF2, 50];
                            rgDm.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDm.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDm.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDm.Style.Font.Color.SetColor(Color.White);
                            rgDm.Style.Font.Size = 10;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F2 en la hoja
                            indexDmF2 = indexDmF2 + 1;

                            // Se inserta el contenido de la lista de data-procesar-F1
                            foreach (DpoMedicion48DTO item in lstDatosCorregidosDmF248)
                            {
                                wsDm.Cells[indexDmF2, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDm.Cells[indexDmF2, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDm = wsDm.Cells[indexDmF2, 2, indexDmF2, 50];
                                rgDm.Style.Font.Size = 10;
                                rgDm.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDmF2++;
                            }

                            // Se establece el formato general del libro
                            rgDm = wsDm.Cells[5, 2, indexDmF2 - 1, 50];
                            rgDm.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDm.Column(i).Width = 80;
                            }

                            rgDm = wsDm.Cells[indexDmF2, 3, indexDmF2, 50];
                            rgDm.AutoFitColumns();
                        }
                        #endregion

                        #region Data maestra - R1
                        // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-maestra-R1 en la hoja
                        int indexDmR1 = 0;

                        if (lstDatosCorregidosDmR148.Count > 0)
                        {
                            if (lstDatosCorregidosDmA248.Count > 0)
                            {
                                indexDmR1 = indexDmA2Desv + 2;
                            }
                            else if (lstDatosCorregidosDmA148.Count > 0)
                            {
                                indexDmR1 = indexDmA1 + 2;
                            }
                            else if (lstDatosCorregidosDmF148.Count > 0)
                            {
                                indexDmR1 = indexDmF1 + 2;
                            }
                            else if (lstDatosCorregidosDmF248.Count > 0)
                            {
                                indexDmR1 = indexDmF2 + 2;
                            }
                            else
                            {
                                indexDmR1 = indexDm + 2;
                            }

                            // Se coloca el titulo de la función en hoja creada
                            wsDm.Cells[indexDmR1, 2].Value = "DATA - FUNCIÓN R1";
                            rgDm = wsDm.Cells[indexDmR1, 2, indexDmR1, 2];
                            rgDm.Style.Font.Size = 13;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDmR1 = indexDmR1 + 1;

                            // Se crean las cabeceras
                            wsDm.Cells[indexDmR1, 2].Value = "FECHA";
                            for (int i = 3; i <= 50; i++)
                            {
                                wsDm.Cells[indexDmR1, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDm = wsDm.Cells[indexDmR1, 2, indexDmR1, 50];
                            rgDm.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDm.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDm.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDm.Style.Font.Color.SetColor(Color.White);
                            rgDm.Style.Font.Size = 10;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDmR1 = indexDmR1 + 1;

                            // Se inserta el contenido de la lista de data-procesar-F2
                            foreach (DpoMedicion48DTO item in lstDatosCorregidosDmR148)
                            {
                                wsDm.Cells[indexDmR1, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDm.Cells[indexDmR1, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDm = wsDm.Cells[indexDmR1, 2, indexDmR1, 50];
                                rgDm.Style.Font.Size = 10;
                                rgDm.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDmR1++;
                            }

                            // Se establece el formato general del libro
                            rgDm = wsDm.Cells[5, 2, indexDmR1 - 1, 50];
                            rgDm.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDm.Column(i).Width = 80;
                            }

                            rgDm = wsDm.Cells[indexDmR1, 3, indexDmR1, 50];
                            rgDm.AutoFitColumns();
                        }
                        #endregion

                        #region Data maestra - R2
                        // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-maestra-R2 en la hoja
                        int indexDmR2 = 0;

                        if (lstDatosCorregidosDmR248.Count > 0)
                        {
                            if (lstDatosCorregidosDmA248.Count > 0)
                            {
                                indexDmR2 = indexDmA2Desv + 2;
                            }
                            else if (lstDatosCorregidosDmA148.Count > 0)
                            {
                                indexDmR2 = indexDmA1 + 2;
                            }
                            else if (lstDatosCorregidosDmF148.Count > 0)
                            {
                                indexDmR2 = indexDmF1 + 2;
                            }
                            else if (lstDatosCorregidosDmF248.Count > 0)
                            {
                                indexDmR2 = indexDmF2 + 2;
                            }
                            else if (lstDatosCorregidosDmR148.Count > 0)
                            {
                                indexDmR2 = indexDmR1 + 2;
                            }
                            else
                            {
                                indexDmR2 = indexDm + 2;
                            }

                            // Se coloca el titulo de la función en hoja creada
                            wsDm.Cells[indexDmR2, 2].Value = "DATA - FUNCIÓN R2";
                            rgDm = wsDm.Cells[indexDmR2, 2, indexDmR2, 2];
                            rgDm.Style.Font.Size = 13;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDmR2 = indexDmR2 + 1;

                            // Se crean las cabeceras
                            wsDm.Cells[indexDmR2, 2].Value = "FECHA";
                            for (int i = 3; i <= 50; i++)
                            {
                                wsDm.Cells[indexDmR2, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDm = wsDm.Cells[indexDmR2, 2, indexDmR2, 50];
                            rgDm.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDm.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDm.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDm.Style.Font.Color.SetColor(Color.White);
                            rgDm.Style.Font.Size = 10;
                            rgDm.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-R2 en la hoja
                            indexDmR2 = indexDmR2 + 1;

                            // Se inserta el contenido de la lista de data-procesar-F2
                            foreach (DpoMedicion48DTO item in lstDatosCorregidosDmR248)
                            {
                                wsDm.Cells[indexDmR2, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDm.Cells[indexDmR2, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDm = wsDm.Cells[indexDmR2, 2, indexDmR2, 50];
                                rgDm.Style.Font.Size = 10;
                                rgDm.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDmR2++;
                            }

                            // Se establece el formato general del libro
                            rgDm = wsDm.Cells[5, 2, indexDmR2 - 1, 50];
                            rgDm.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDm.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDm.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDm.Column(i).Width = 80;
                            }

                            rgDm = wsDm.Cells[indexDmR2, 3, indexDmR2, 50];
                            rgDm.AutoFitColumns();
                        }
                        #endregion
                    }
                }
                #endregion

                // --------------------------------------------------------------------------------------------------------------
                // Se crea el libro de data a procesar corregida
                // --------------------------------------------------------------------------------------------------------------
                #region Se procesa el contenido de la lista de data a procesar
                ExcelWorksheet wsDp = xlPackage.Workbook.Worksheets.Add("DATA A PROCESAR");

                if (wsDp != null)
                {
                    ExcelRange rgDp;

                    #region Titulos
                    // Se coloca el titulo de la hoja creada
                    wsDp.Cells[2, 2].Value = "REPORTE DE DATA A PROCESAR CORREGIDA";
                    rgDp = wsDp.Cells[2, 2, 2, 2];
                    rgDp.Style.Font.Size = 13;
                    rgDp.Style.Font.Bold = true;

                    wsDp.Cells[4, 2].Value = "DATA HISTORICA";
                    rgDp = wsDp.Cells[4, 2, 4, 2];
                    rgDp.Style.Font.Size = 13;
                    rgDp.Style.Font.Bold = true;
                    #endregion

                    #region Data historica-procesar
                    // Establece el indice o celda inicial donde se colocara el contenido de la data-historica-procesar
                    // de las cabeceras en la hoja
                    int indexDp = 5;

                    // Se crean las cabeceras
                    wsDp.Cells[indexDp, 2].Value = "Fecha";
                    for (int i = 3; i <= 50; i++)
                    {
                        wsDp.Cells[indexDp, i].Value = "H" + (i - 2);
                    }

                    // Se formatea las cabeceras
                    rgDp = wsDp.Cells[5, 2, indexDp, 50];
                    rgDp.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rgDp.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rgDp.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    rgDp.Style.Font.Color.SetColor(Color.White);
                    rgDp.Style.Font.Size = 10;
                    rgDp.Style.Font.Bold = true;

                    // Se inserta el contenido de la lista de data-historica-procesar
                    indexDp = 6;

                    // Se inserta el contenido de la lista de data maestra
                    foreach (DpoHistorico48DTO item in lstDatosHistoricosDp48)
                    {
                        wsDp.Cells[indexDp, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");

                        // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                        for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                        {
                            // corrige el dato de la columna Hi
                            wsDp.Cells[indexDp, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                        }

                        // Se formatea el contenido de las celdas que contiene la data maestra
                        rgDp = wsDp.Cells[indexDp, 2, indexDp, 50];
                        rgDp.Style.Font.Size = 10;
                        rgDp.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                        indexDp++;
                    }

                    // Se establece el formato general del libro
                    rgDp = wsDp.Cells[5, 2, indexDp - 1, 50];
                    rgDp.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rgDp.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDp.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rgDp.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDp.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rgDp.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rgDp.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rgDp.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    for (int i = 2; i <= 50; i++)
                    {
                        wsDp.Column(i).Width = 80;
                    }

                    rgDp = wsDp.Cells[5, 3, indexDp, 50];
                    rgDp.AutoFitColumns();
                    #endregion

                    if (lstDatosHistoricosDp48.Count > 0)
                    {
                        #region Data a procesar - A1
                        int indexDpA1 = 0;

                        if (lstDatosCorregidosDpA148.Count > 0)
                        {
                            // Establece el indice o celda inicial donde se colocara el titulo de la funcion data-procesar-A1 en la hoja
                            indexDpA1 = indexDp + 2;

                            wsDp.Cells[indexDpA1, 2].Value = "DATA - FUNCIÓN A1";
                            rgDp = wsDp.Cells[indexDpA1, 2, indexDpA1, 2];
                            rgDp.Style.Font.Size = 13;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-procesar-A1 en la hoja
                            indexDpA1 = indexDpA1 + 1;

                            // Se crean las cabeceras
                            wsDp.Cells[indexDpA1, 2].Value = "GRUPO";
                            for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                            {
                                wsDp.Cells[indexDpA1, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDp = wsDp.Cells[indexDpA1, 2, indexDpA1, 50];
                            rgDp.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDp.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDp.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDp.Style.Font.Color.SetColor(Color.White);
                            rgDp.Style.Font.Size = 10;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDpA1 = indexDpA1 + 1;

                            // Se inserta el contenido de la lista de data maestra
                            foreach (DpoMedicion48DTO item in lstDatosCorregidosDpA148)
                            {
                                wsDp.Cells[indexDpA1, 2].Value = item.Grupo;

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDp.Cells[indexDpA1, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDp = wsDp.Cells[indexDpA1, 2, indexDpA1, 50];
                                rgDp.Style.Font.Size = 10;
                                rgDp.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDpA1++;
                            }

                            // Se establece el formato general del libro
                            rgDp = wsDp.Cells[5, 2, indexDpA1 - 1, 50];
                            rgDp.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDp.Column(i).Width = 80;
                            }

                            rgDp = wsDp.Cells[indexDpA1, 3, indexDpA1, 50];
                            rgDp.AutoFitColumns();
                        }
                        #endregion

                        #region Data a procesar - A2
                        int indexDpA2Prom = 0;
                        int indexDpA2Desv = 0;

                        if (lstDatosCorregidosDpA248.Count > 0)
                        {
                            // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-procesar-A2 en la hoja
                            if (lstDatosCorregidosDpA148.Count > 0)
                            {
                                indexDpA2Prom = indexDpA1 + 2;
                            }
                            else
                            {
                                indexDpA2Prom = indexDp + 2;
                            }

                            wsDp.Cells[indexDpA2Prom, 2].Value = "DATA - FUNCIÓN A2 - PROMEDIOS";
                            rgDp = wsDp.Cells[indexDpA2Prom, 2, indexDpA2Prom, 2];
                            rgDp.Style.Font.Size = 13;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-procesar-A2 en la hoja
                            indexDpA2Prom = indexDpA2Prom + 1;

                            // Se crean las cabeceras
                            wsDp.Cells[indexDpA2Prom, 2].Value = "GRUPO";
                            for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                            {
                                wsDp.Cells[indexDpA2Prom, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDp = wsDp.Cells[indexDpA2Prom, 2, indexDpA2Prom, 50];
                            rgDp.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDp.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDp.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDp.Style.Font.Color.SetColor(Color.White);
                            rgDp.Style.Font.Size = 10;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDpA2Prom = indexDpA2Prom + 1;

                            // Se inserta el contenido de la lista de data-procesar-A1
                            List<DpoMedicion48DTO> sublistaPromediosDpA248 = lstDatosCorregidosDpA248[0];
                            foreach (DpoMedicion48DTO item in sublistaPromediosDpA248)
                            {
                                wsDp.Cells[indexDpA2Prom, 2].Value = item.Grupo;

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDp.Cells[indexDpA2Prom, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDp = wsDp.Cells[indexDpA2Prom, 2, indexDpA2Prom, 50];
                                rgDp.Style.Font.Size = 10;
                                rgDp.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDpA2Prom++;
                            }

                            // Se establece el formato general del libro
                            rgDp = wsDp.Cells[5, 2, indexDpA2Prom - 1, 50];
                            rgDp.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDp.Column(i).Width = 80;
                            }

                            rgDp = wsDp.Cells[indexDpA2Prom, 3, indexDpA2Prom, 50];
                            rgDp.AutoFitColumns();

                            // ----------------------------------------------------------------------------------------------------------

                            // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-procesar-A2 en la hoja
                            if (sublistaPromediosDpA248.Count > 0)
                            {
                                indexDpA2Desv = indexDpA2Prom + 2;
                            }
                            else if (lstDatosCorregidosDpA148.Count > 0)
                            {
                                indexDpA2Desv = indexDpA1 + 2;
                            }
                            else
                            {
                                indexDpA2Desv = indexDp + 2;
                            }


                            wsDp.Cells[indexDpA2Desv, 2].Value = "FUNCIÓN A2 - DESVIACIONES";
                            rgDp = wsDp.Cells[indexDpA2Desv, 2, indexDpA2Desv, 2];
                            rgDp.Style.Font.Size = 13;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-procesar-A2 en la hoja
                            indexDpA2Desv = indexDpA2Desv + 1;

                            // Se crean las cabeceras
                            wsDp.Cells[indexDpA2Desv, 2].Value = "GRUPO";
                            for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                            {
                                wsDp.Cells[indexDpA2Desv, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDp = wsDp.Cells[indexDpA2Desv, 2, indexDpA2Desv, 50];
                            rgDp.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDp.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDp.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDp.Style.Font.Color.SetColor(Color.White);
                            rgDp.Style.Font.Size = 10;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDpA2Desv = indexDpA2Desv + 1;

                            // Se inserta el contenido de la lista de data-procesar-A1
                            List<DpoMedicion48DTO> sublistaDesviacionesDpA248 = lstDatosCorregidosDpA248[1];
                            foreach (DpoMedicion48DTO item in sublistaPromediosDpA248)
                            {
                                wsDp.Cells[indexDpA2Desv, 2].Value = item.Grupo;

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDp.Cells[indexDpA2Desv, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDp = wsDp.Cells[indexDpA2Desv, 2, indexDpA2Desv, 50];
                                rgDp.Style.Font.Size = 10;
                                rgDp.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDpA2Desv++;
                            }

                            // Se establece el formato general del libro
                            rgDp = wsDp.Cells[5, 2, indexDpA2Desv - 1, 50];
                            rgDp.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDp.Column(i).Width = 80;
                            }

                            rgDp = wsDp.Cells[indexDpA2Desv, 3, indexDpA2Desv, 50];
                            rgDp.AutoFitColumns();
                        }
                        #endregion

                        #region Data a procesar - F1
                        // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-procesar-F1 en la hoja
                        int indexDpF1 = 0;

                        if (lstDatosCorregidosDpF148.Count > 0)
                        {
                            if (lstDatosCorregidosDpA248.Count > 0)
                            {
                                indexDpF1 = indexDpA2Desv + 2;
                            }
                            else if (lstDatosCorregidosDpA148.Count > 0)
                            {
                                indexDpF1 = indexDpA1 + 2;
                            }
                            else
                            {
                                indexDpF1 = indexDp + 2;
                            }

                            wsDp.Cells[indexDpF1, 2].Value = "DATA - FUNCIÓN F1";
                            rgDp = wsDp.Cells[indexDpF1, 2, indexDpF1, 2];
                            rgDp.Style.Font.Size = 13;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-procesar-F1 en la hoja
                            indexDpF1 = indexDpF1 + 1;

                            // Se crean las cabeceras
                            wsDp.Cells[indexDpF1, 2].Value = "FECHA";
                            for (int i = 3; i <= 50; i++)
                            {
                                wsDp.Cells[indexDpF1, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDp = wsDp.Cells[indexDpF1, 2, indexDpF1, 50];
                            rgDp.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDp.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDp.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDp.Style.Font.Color.SetColor(Color.White);
                            rgDp.Style.Font.Size = 10;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDpF1 = indexDpF1 + 1;

                            // Se inserta el contenido de la lista de data maestra
                            foreach (DpoMedicion48DTO item in lstDatosCorregidosDpF148)
                            {
                                wsDp.Cells[indexDpF1, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDp.Cells[indexDpF1, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDp = wsDp.Cells[indexDpF1, 2, indexDpF1, 50];
                                rgDp.Style.Font.Size = 10;
                                rgDp.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDpF1++;
                            }

                            // Se establece el formato general del libro
                            rgDp = wsDp.Cells[5, 2, indexDpF1 - 1, 50];
                            rgDp.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDp.Column(i).Width = 80;
                            }

                            rgDp = wsDp.Cells[indexDpF1, 3, indexDpF1, 50];
                            rgDp.AutoFitColumns();
                        }
                        #endregion

                        #region Data a procesar - F2
                        // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-procesar-F2 en la hoja
                        int indexDpF2 = 0;

                        if (lstDatosCorregidosDpF248.Count > 0)
                        {
                            if (lstDatosCorregidosDpA248.Count > 0)
                            {
                                indexDpF2 = indexDpA2Desv + 2;
                            }
                            else if (lstDatosCorregidosDpA148.Count > 0)
                            {
                                indexDpF2 = indexDpA1 + 2;
                            }
                            else if (lstDatosCorregidosDpF148.Count > 0)
                            {
                                indexDpF2 = indexDpF1 + 2;
                            }
                            else
                            {
                                indexDpF2 = indexDp + 2;
                            }

                            wsDp.Cells[indexDpF2, 2].Value = "DATA - FUNCIÓN F2";
                            rgDp = wsDp.Cells[indexDpF2, 2, indexDpF2, 2];
                            rgDp.Style.Font.Size = 13;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-procesar-F2 en la hoja
                            indexDpF2 = indexDpF2 + 1;

                            // Se crean las cabeceras
                            wsDp.Cells[indexDpF2, 2].Value = "FECHA";
                            for (int i = 3; i <= 50; i++)
                            {
                                wsDp.Cells[indexDpF2, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDp = wsDp.Cells[indexDpF2, 2, indexDpF2, 50];
                            rgDp.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDp.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDp.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDp.Style.Font.Color.SetColor(Color.White);
                            rgDp.Style.Font.Size = 10;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDpF2 = indexDpF2 + 1;

                            // Se inserta el contenido de la lista de data maestra
                            foreach (DpoMedicion48DTO item in lstDatosCorregidosDpA148)
                            {
                                wsDp.Cells[indexDpF2, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDp.Cells[indexDpF2, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDp = wsDp.Cells[indexDpF2, 2, indexDpF2, 50];
                                rgDp.Style.Font.Size = 10;
                                rgDp.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDpF2++;
                            }

                            // Se establece el formato general del libro
                            rgDp = wsDp.Cells[5, 2, indexDpF2 - 1, 50];
                            rgDp.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDp.Column(i).Width = 80;
                            }

                            rgDp = wsDp.Cells[indexDpF2, 3, indexDpF2, 50];
                            rgDp.AutoFitColumns();
                        }
                        #endregion

                        #region Data a procesar - R1
                        // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-procesar-R1 en la hoja
                        int indexDpR1 = 0;

                        if (lstDatosCorregidosDpR148.Count > 0)
                        {
                            if (lstDatosCorregidosDpA248.Count > 0)
                            {
                                indexDpR1 = indexDpA2Desv + 2;
                            }
                            else if (lstDatosCorregidosDpA148.Count > 0)
                            {
                                indexDpR1 = indexDpA1 + 2;
                            }
                            else if (lstDatosCorregidosDpF148.Count > 0)
                            {
                                indexDpR1 = indexDpF1 + 2;
                            }
                            else if (lstDatosCorregidosDpF248.Count > 0)
                            {
                                indexDpR1 = indexDpF2 + 2;
                            }
                            else
                            {
                                indexDpR1 = indexDp + 2;
                            }

                            wsDp.Cells[indexDpR1, 2].Value = "DATA - FUNCIÓN R1";
                            rgDp = wsDp.Cells[indexDpR1, 2, indexDpR1, 2];
                            rgDp.Style.Font.Size = 13;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-procesar-A1 en la hoja
                            indexDpR1 = indexDpR1 + 1;

                            // Se crean las cabeceras
                            wsDp.Cells[indexDpR1, 2].Value = "FECHA";
                            for (int i = 3; i <= 50; i++)
                            {
                                wsDp.Cells[indexDpR1, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDp = wsDp.Cells[indexDpR1, 2, indexDpR1, 50];
                            rgDp.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDp.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDp.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDp.Style.Font.Color.SetColor(Color.White);
                            rgDp.Style.Font.Size = 10;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDpR1 = indexDpR1 + 1;

                            // Se inserta el contenido de la lista de data maestra
                            foreach (DpoMedicion48DTO item in lstDatosCorregidosDpR148)
                            {
                                wsDp.Cells[indexDpR1, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDp.Cells[indexDpR1, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDp = wsDp.Cells[indexDpR1, 2, indexDpR1, 50];
                                rgDp.Style.Font.Size = 10;
                                rgDp.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDpR1++;
                            }

                            // Se establece el formato general del libro
                            rgDp = wsDp.Cells[5, 2, indexDpR1 - 1, 50];
                            rgDp.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDp.Column(i).Width = 80;
                            }

                            rgDp = wsDp.Cells[indexDpR1, 3, indexDpR1, 50];
                            rgDp.AutoFitColumns();
                        }
                        #endregion

                        #region Data a procesar - R2
                        // Establece el indice o celda inicial donde se colocara el contenido el titulo de la funcion data-procesar-R2 en la hoja
                        int indexDpR2 = 0;

                        if (lstDatosCorregidosDpR248.Count > 0)
                        {
                            if (lstDatosCorregidosDpA248.Count > 0)
                            {
                                indexDpR2 = indexDpA2Desv + 2;
                            }
                            else if (lstDatosCorregidosDpA148.Count > 0)
                            {
                                indexDpR2 = indexDpA1 + 2;
                            }
                            else if (lstDatosCorregidosDpF148.Count > 0)
                            {
                                indexDpR2 = indexDpF1 + 2;
                            }
                            else if (lstDatosCorregidosDpF248.Count > 0)
                            {
                                indexDpR2 = indexDpF2 + 2;
                            }
                            else if (lstDatosCorregidosDpR148.Count > 0)
                            {
                                indexDpR2 = indexDpR1 + 2;
                            }
                            else
                            {
                                indexDpR2 = indexDp + 2;
                            }

                            wsDp.Cells[indexDpR2, 2].Value = "DATA - FUNCIÓN R2";
                            rgDp = wsDp.Cells[indexDpR2, 2, indexDpR2, 2];
                            rgDp.Style.Font.Size = 13;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-procesar-R1 en la hoja
                            indexDpR2 = indexDpR2 + 1;

                            // Se crean las cabeceras
                            wsDp.Cells[indexDpR2, 2].Value = "FECHA";
                            for (int i = 3; i <= 50; i++)
                            {
                                wsDp.Cells[indexDpR2, i].Value = "H" + (i - 2);
                            }

                            // Se formatea las cabeceras
                            rgDp = wsDp.Cells[indexDpR2, 2, indexDpR2, 50];
                            rgDp.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rgDp.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rgDp.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rgDp.Style.Font.Color.SetColor(Color.White);
                            rgDp.Style.Font.Size = 10;
                            rgDp.Style.Font.Bold = true;

                            // Establece el indice o celda inicial donde se colocara el contenido de la data-maestra-F1 en la hoja
                            indexDpR2 = indexDpR2 + 1;

                            // Se inserta el contenido de la lista de data maestra
                            foreach (DpoMedicion48DTO item in lstDatosCorregidosDpR248)
                            {
                                wsDp.Cells[indexDpR2, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");

                                // Se recorre las columnas y corrige los datos de la fila de la lista parcial
                                for (int i = 3; i <= (ConstantesDpo.Itv30min + 2); i++)
                                {
                                    // corrige el dato de la columna Hi
                                    wsDp.Cells[indexDpR2, i].Value = item.GetType().GetProperty($"H{(i - 2)}").GetValue(item);
                                }

                                // Se formatea el contenido de las celdas que contiene la data maestra
                                rgDp = wsDp.Cells[indexDpR2, 2, indexDpR2, 50];
                                rgDp.Style.Font.Size = 10;
                                rgDp.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                                indexDpR2++;
                            }

                            // Se establece el formato general del libro
                            rgDp = wsDp.Cells[5, 2, indexDpR2 - 1, 50];
                            rgDp.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                            rgDp.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rgDp.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                            for (int i = 2; i <= 50; i++)
                            {
                                wsDp.Column(i).Width = 80;
                            }

                            rgDp = wsDp.Cells[indexDpR2, 3, indexDpR2, 50];
                            rgDp.AutoFitColumns();
                        }
                        #endregion
                    }
                }
                #endregion

                xlPackage.Save();
            }

            string Reporte = fileName;

            return Reporte;
        }

        /// <summary>
        /// Permite generar el reporte en formato Excel de archivos Raw no procesados
        /// </summary>
        /// <param name="arreglo">arreglo</param>
        /// <param name="cuartil">n cuartil</param>
        public decimal CalcularCuartilN(decimal[] arreglo, int cuartil)
        {
            if (arreglo.Length == 0)
            {
                throw new ArgumentException("El arreglo debe contener al menos un elemento.");
            }

            decimal[] arregloOrdenado = arreglo.OrderBy(x => x).ToArray();

            decimal posicion = ((decimal)(arregloOrdenado.Length + 1) * cuartil) / 4;

            int entero = (int)posicion;
            decimal decimalFraccionario = posicion - entero;

            decimal cuartilInferior = arregloOrdenado[entero - 1];
            decimal cuartilSuperior = arregloOrdenado[entero];

            decimal cuartilInterpolado = cuartilInferior + (cuartilSuperior - cuartilInferior) * decimalFraccionario;

            return cuartilInterpolado;
        }
        #endregion

        #endregion

        public string CargaDatosReporteDatosxHora(
            string regFecha, int regHora,
            int selTipo)
        {
            DateTime parseFecha = DateTime.ParseExact(regFecha,
               ConstantesDpo.FormatoFecha,
               CultureInfo.InvariantCulture);

            string nomTabla = ConstantesDpo.tablaEstimadorRaw
                + parseFecha.ToString(ConstantesDpo.FormatoAnioMes);

            string strFecha = parseFecha.AddHours(regHora)
                .ToString(ConstantesDpo.FormatoFechaMedicionRaw);

            List<DpoEstimadorRawDTO> datosReporte = FactorySic
                .GetDpoEstimadorRawRepository().ReporteDatosEstimadorxHora(
                nomTabla, strFecha, strFecha, selTipo);

            List<string> rowCabecera = new List<string>()
            { 
                "Punto de medición",
                "Tipo información",
                "Fecha",
            };

            DateTime refHora = parseFecha.AddHours(regHora);
            for (int i = 0; i < 60; i++)
            {
                string strHora = refHora
                    .AddMinutes(i + 1)
                    .ToString(ConstantesDpo.FormatoFechaMedicionRaw);
                rowCabecera.Add(strHora);
            }
                        
            List<string[]> tbContenido = new List<string[]>();
            foreach (DpoEstimadorRawDTO dato in datosReporte)
            {
                string[] row = new string[63];
                row[0] = dato.Ptomedidesc;
                row[1] = dato.Prnvarnom;
                row[2] = dato.Dporawfecha.ToString(ConstantesDpo.FormatoFecha);

                for (int i = 0; i < 60; i++)
                {
                    var valor = dato.GetType()
                        .GetProperty($"Dporawvalorh{(i + 1)}")
                        .GetValue(dato);
                    string strValor = (valor == null) 
                        ? string.Empty 
                        : valor.ToString();

                    row[3 + i] = strValor;
                }
                tbContenido.Add(row);
            }

            string strSubTitulo = string.Empty;
            if (selTipo == ConstantesProdem.EtmrawtpLinea)
                strSubTitulo = "Lineas";
            if (selTipo == ConstantesProdem.EtmrawtpBarra)
                strSubTitulo = "Barras";
            if (selTipo == ConstantesProdem.EtmrawtpShunt)
                strSubTitulo = "Shunts";
            if (selTipo == ConstantesProdem.EtmrawtpGenerador)
                strSubTitulo = "Generadores";
            if (selTipo == ConstantesProdem.EtmrawtpCarga)
                strSubTitulo = "Cargas";
            if (selTipo == ConstantesProdem.EtmrawtpTrafo)
                strSubTitulo = "Transformadores";

            PrnFormatoExcel excel = new PrnFormatoExcel()
            {
                Titulo = "Reporte de datos por hora",
                Subtitulo1 = strSubTitulo,
                Cabecera = rowCabecera.ToArray(),
                Contenido = tbContenido.ToArray(),
                AnchoColumnas = Enumerable
                .Repeat(40, rowCabecera.Count)
                .ToArray(),
            };

            string pathFile = AppDomain.CurrentDomain.BaseDirectory + ConstantesDpo.RutaReportes;
            string filename = "REP_DATOS_HORA";
            string res = this.ExportarReporteSimple(excel, pathFile, filename);

            return res;
        }

        public decimal PasarDecimalCero(string sValor)
        {
            decimal valor;
            if (decimal.TryParse(sValor, out valor))
            {
                if (valor >= decimal.MinValue && valor <= decimal.MaxValue)
                {
                    // El valor se encuentra dentro del rango válido para un decimal
                    return valor;
                }
                else
                {
                   //"El valor está fuera del rango válido para un decimal.
                    return 0.0M;
                }
                
            }
            else
            {
                return 0.0M;
            }   
        }


        #region PronDem. Vegetativo por Barras - Ultima hora 20250214
        /// <summary>
        /// Devuelve la lista de barras registradas en el formato hoja 47
        /// </summary>
        /// <returns></returns>
        public List<MePtomedicionDTO> ListaBarrasFormato()
        {
            return FactorySic.GetDpoProcesoPronosticoRepository()
                .ListaBarras();
        }
        #endregion
    }
}
