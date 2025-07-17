using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using ExcelLibrary.BinaryDrawingFormat;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using static COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper.TablaHTML;

namespace COES.Servicios.Aplicacion.DPODemanda.Helper
{
    public class UtilDpo
    {        
        /// <summary>
        /// Convierte mediciones agrupadas x hora a formato x minuto
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <param name="fecIni">Fecha de inicio del rango de la información</param>
        /// <param name="fecFin">Fecha de termino del rango de la información</param>
        /// <returns></returns>
        public static List<DpoMedicion1440> DarFormato1440(
            List<DpoEstimadorRawDTO> datos, DateTime fecIni,
            DateTime fecFin)
        {
            DpoMedicion1440 entity = new DpoMedicion1440();
            List<DpoMedicion1440> entities = new List<DpoMedicion1440>();

            List<int> puntosMedicion = datos
                .Select(x => x.Ptomedicodi)
                .Distinct()
                .ToList();

            foreach (int punto in puntosMedicion)
            {
                //Obtiene los datos por punto de medición
                List<DpoEstimadorRawDTO> datosxPunto = datos
                    .Where(x => x.Ptomedicodi == punto)
                    .ToList();

                //Obtiene los datos para cada día
                DateTime d = fecIni;
                while (d <= fecFin)
                {
                    DateTime rangoLimite = d.AddHours(23).AddMinutes(59);
                    List<DpoEstimadorRawDTO> datosxDia = datosxPunto
                        .Where(x => x.Dporawfecha >= d && x.Dporawfecha <= rangoLimite)
                        .ToList();

                    if (datosxDia.Count == 0)
                    {
                        d = d.AddDays(1);
                        continue;
                    }

                    List<int> tipoInformacion = datosxDia
                        .Select(x => x.Prnvarcodi)
                        .Distinct()
                        .ToList();

                    //Obtiene los datos por cada tipo de información
                    foreach (int tipoInfo in tipoInformacion)
                    {
                        List<DpoEstimadorRawDTO> datosxTipoInfo = datosxDia
                            .Where(x => x.Prnvarcodi == tipoInfo)
                            .ToList();

                        if (datosxTipoInfo.Count == 0) continue;

                        entity = new DpoMedicion1440()
                        {
                            Ptomedicodi = punto,
                            Medifecha = d,
                            Tipoinfo = tipoInfo,
                            Medicion = new decimal[ConstantesDpo.Itv1min]
                        };

                        //Obtiene los valores por intervalo hora
                        for (int i = 0; i < 24; i++)
                        {
                            DateTime intervaloHora = new DateTime(
                                d.Year, d.Month, d.Day,
                                i, 0, 0);

                            DpoEstimadorRawDTO datoHora = datosxTipoInfo
                                .FirstOrDefault(x => x.Dporawfecha == intervaloHora);

                            if (datoHora == null) continue;

                            int j = 0;
                            while (j < 60)
                            {
                                var r = datoHora.GetType()
                                    .GetProperty($"Dporawvalorh{j + 1}")
                                    .GetValue(datoHora);

                                if (r != null) 
                                    entity.Medicion[(i * 60) + j] = (decimal)r;
                                j++;
                            }                            
                        }
                        entities.Add(entity);
                    }
                    d = d.AddDays(1);
                }
            }

            return entities;
        }

        /// <summary>
        /// Devuelve las mediciones agrupadas por día típico
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <returns></returns>
        public static DpoMedDiaTipicoModel MedicionesxDiaTipico(
            List<DpoMedicion1440> datos)
        {
            DpoMedDiaTipicoModel entity = new DpoMedDiaTipicoModel();
            
            int diaSemana = 0;
            entity.datosLunes = new List<DpoMedicion1440>();
            entity.datosSabado = new List<DpoMedicion1440>();
            entity.datosDomingo = new List<DpoMedicion1440>();
            entity.datosOtros = new List<DpoMedicion1440>();

            foreach (DpoMedicion1440 dato in datos)
            {
                diaSemana = (int)dato.Medifecha.DayOfWeek;

                if (diaSemana == (int)DayOfWeek.Monday)
                    entity.datosLunes.Add(dato);
                else if (diaSemana == (int)DayOfWeek.Saturday)
                    entity.datosSabado.Add(dato);
                else if (diaSemana == (int)DayOfWeek.Sunday)
                    entity.datosDomingo.Add(dato);
                else
                    entity.datosOtros.Add(dato);                               
            }

            return entity;
        }

        /// <summary>
        /// Devuelve el promedio de las mediciones de un día típico [SPL]
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <returns></returns>
        public static decimal[] PromedioSPL(
            List<DpoMedicion1440> datos)
        {
            decimal[] promedio = new decimal[ConstantesDpo.Itv1min];

            if (datos.Count == 0) return promedio;

            int i = 0;
            while (i < promedio.Length)
            {
                int iValidos = 0;
                foreach (DpoMedicion1440 dato in datos)
                {
                    if (dato.Medicion[i] == 0) continue;

                    promedio[i] += dato.Medicion[i];
                    iValidos++;
                }
                if (iValidos != 0) promedio[i] = promedio[i] / iValidos;
                i++;
            }

            return promedio;
        }

        /// <summary>
        /// Devuelve el promedio de las mediciones de un día típico de 96 'H'
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <returns></returns>
        public static decimal[] Promedio96(
            List<decimal[]> datos)
        {
            decimal[] promedio = new decimal[ConstantesDpo.Itv15min];

            if (datos.Count == 0) return promedio;

            int i = 0;
            while (i < promedio.Length)
            {
                int iValidos = 0;
                foreach (decimal[] dato in datos)
                {
                    if (dato[i] == 0) continue;

                    promedio[i] += dato[i];
                    iValidos++;
                }
                if (iValidos != 0) promedio[i] = promedio[i] / iValidos;
                i++;
            }

            return promedio;
        }

        /// <summary>
        /// Devuelve el promedio de las mediciones de un día típico [SCO]
        /// </summary>
        /// <param name="datos">Datos de las mediciones</param>
        /// <returns></returns>        
        public static decimal[] PromedioSCO(
            List<DpoMedicion1440> datos)
        {
            decimal[] promedio = new decimal[ConstantesDpo.Itv1min];

            if (datos.Count == 0) return promedio;

            List<decimal> medicion = datos[0].Medicion.ToList();
            int i = 0;
            while (i < ConstantesDpo.Itv1min)
            {
                int iValidos = 0;
                decimal totalArreglo = 0;
                foreach (DpoMedicion1440 d in datos)
                {
                    if (d.Medicion[i] != 0)
                    {
                        totalArreglo += d.Medicion[i];
                        iValidos++;
                    }
                }
                if (iValidos != 0)
                    promedio[i] = totalArreglo / iValidos;
                else
                {
                    string rangoFaltante = string.Empty;
                    
                    //Evaluación rango faltante
                    if (i == 0)
                        rangoFaltante = "ini";
                    else if (i == ConstantesDpo.Itv1min - 1)
                        rangoFaltante = "fin";
                    else
                    {
                        if (medicion.GetRange(0, (i + 1)).Sum() == 0)
                            rangoFaltante = "ini";
                        else if (medicion.GetRange(i, ConstantesDpo.Itv1min - i).Sum() == 0)
                            rangoFaltante = "fin";
                        else
                            rangoFaltante = "med";                        
                    }

                    //Corrección del intervalo
                    if (rangoFaltante == "ini")
                    {
                        List<decimal> input = new List<decimal>();
                        int k = 1;
                        while (k < ConstantesDpo.Itv1min)
                        {
                            if (medicion[k] != 0)
                                input.Add(medicion[k]);
                            if (input.Count == 2) break;
                            k++;
                        }
                        medicion[i] = input.Sum() / 2;
                        promedio[i] = medicion[i];
                    }
                    if (rangoFaltante == "med")
                    {
                        List<decimal> input = new List<decimal>();
                        int k = i;
                        while (k < ConstantesDpo.Itv1min)
                        {
                            if (medicion[k] != 0)
                            {
                                input.Add(medicion[k]);
                                break;
                            }
                            k++;
                        }
                        k = i;
                        while (k >= 0)
                        {
                            if (medicion[k] != 0)
                            {
                                input.Add(medicion[k]);
                                break;
                            }
                            k--;
                        }
                        medicion[i] = input.Sum() / 2;
                        promedio[i] = medicion[i];
                    }
                    if (rangoFaltante == "fin")
                    {
                        List<decimal> input = new List<decimal>();
                        int k = i - 1;
                        while (k >= 0)
                        {
                            if (promedio[k] != 0)
                                input.Add(promedio[k]);
                            if (input.Count == 2) break;
                            k--;
                        }
                        medicion[i] = input.Sum() / 2;
                        promedio[i] = medicion[i];
                    }
                }
                i++;
            }

            return promedio;
        }

        /// <summary>
        /// Convierte mediciones en formato de 96 intervalos (15 min)
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public static List<DpoDemandaScoDTO> DarFormato96(
            List<DpoMedicion1440> datos)
        {
            List<DpoDemandaScoDTO> entities = new List<DpoDemandaScoDTO>();

            foreach (DpoMedicion1440 dato in datos)
            {
                DpoDemandaScoDTO entity = new DpoDemandaScoDTO
                {
                    Ptomedicodi = dato.Ptomedicodi,
                    Medifecha = dato.Medifecha,
                    Prnvarcodi = dato.Tipoinfo,                    
                    Demscofeccreacion = DateTime.Now,
                };

                List<decimal> total = new List<decimal>();
                List<decimal> medicion = dato.Medicion.ToList();                
                int i = 0;
                while (i < ConstantesDpo.Itv15min)
                {
                    decimal valor = medicion.GetRange(i * 15, 15).Sum();
                    valor = valor / 15;
                    valor = Math.Round(valor, 4);
                    total.Add(valor);
                    entity.GetType()
                        .GetProperty($"H{(i + 1)}")
                        .SetValue(entity, valor);
                    i++;
                }
                entity.Meditotal = Math.Round(total.Sum(), 4);
                entities.Add(entity);
            }

            return entities;
        }

        /// <summary>
        /// Helper que genera los intervalos de tiempo correspondiente para cada medición
        /// </summary>
        public static string[] GenerarIntervalos(int numIntervalos)
        {
            string[] intervalos = new string[numIntervalos];
            DateTime horaBase = new DateTime();

            int minutos = 0;
            switch (numIntervalos)
            {
                case ConstantesDpo.Itv1min: minutos = 1; break;                
                case ConstantesDpo.Itv15min: minutos = 15; break;
                case ConstantesDpo.Itv30min: minutos = 30; break;
            }

            int i = 0;
            while (i < numIntervalos)
            {
                horaBase = horaBase.AddMinutes(minutos);
                intervalos[i] = horaBase.ToString(ConstantesDpo.FormatoHoraMinuto);
                i++;
            }

            return intervalos;
        }

        /// <summary>
        /// Helper que convierte una entidad en arreglo
        /// </summary>
        public static decimal[] ConvertirMedicionEnArreglo(int numIntervalos, object medicion)
        {
            decimal[] arreglo = new decimal[numIntervalos];

            if (medicion != null)
            {
                int i = 0;
                while (i < numIntervalos)
                {
                    var dValor = medicion.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(medicion, null) ?? (decimal)0;
                    arreglo[i] = (decimal)dValor;
                    i++;
                }
            }
            return arreglo;
        }

        /// <summary>
        /// Helper que convierte una entidad en arreglo
        /// </summary>
        public static decimal?[] ConvertirMedicionNuloEnArreglo(int numIntervalos, object medicion)
        {
            decimal?[] arreglo = new decimal?[numIntervalos];

            if (medicion != null)
            {
                int i = 0;
                while (i < numIntervalos)
                {
                    decimal? dValor = (decimal?)medicion.GetType().GetProperty("H" + (i + 1).ToString()).GetValue(medicion, null);
                    arreglo[i] = dValor;
                    i++;
                }
            }

            return arreglo;
        }


        /// <summary>
        /// Permite comparar dos arreglos
        /// </summary>
        /// <param name="aryFirst">Primer arreglo a comparar</param>
        /// <param name="arySecond">Segundo arreglo a comparar</param>
        /// <returns></returns>
        public static bool CompararArreglos(decimal[] aryFirst, decimal[] arySecond)
        {
            if (aryFirst.Length != arySecond.Length)
                return false;

            for (int i = 0; i < aryFirst.Length; i++)
            {
                if (aryFirst[i] != arySecond[i])
                    return false;
            }

            return true;
        }

        public static List<int> CompararArreglosPorIntervalos(
            decimal[] aryFirst, decimal[] arySecond)
        {
            List<int> res = new List<int>();
            
            if (aryFirst.Length != arySecond.Length)
                return res;

            int i = 0;
            while (i < aryFirst.Length)
            {
                //if (aryFirst[i] == null) continue;
                //if (arySecond[i] == null) continue;
                if (aryFirst[i] != arySecond[i]) res.Add(i);
                i++;
            }

            return res;
        }

        public static List<int> CompararArreglosNullPorIntervalos(
            decimal?[] aryFirst, decimal?[] arySecond)
        {
            List<int> res = new List<int>();

            if (aryFirst.Length != arySecond.Length)
                return res;

            for (int i = 0; i < aryFirst.Length; i++)
            {
                if (arySecond[i] == null) continue;
                if (aryFirst[i] != arySecond[i]) res.Add(i);
            }

            return res;
        }

        /// <summary>
        /// Genera una lista filtro de intervalos
        /// </summary>
        /// <param name="numIntervalos">Número de intervalos a generar</param>
        /// <returns></returns>
        public static List<string[]> ListaIntervalos(int numIntervalos)
        {
            List<string[]> res = new List<string[]>();
            DateTime horaBase = new DateTime();

            int minutos = 0;
            switch (numIntervalos)
            {
                case ConstantesDpo.Itv1min: minutos = 1; break;
                case ConstantesDpo.Itv15min: minutos = 15; break;
                case ConstantesDpo.Itv30min: minutos = 30; break;
            }

            int i = 0;
            while (i < numIntervalos)
            {
                horaBase = horaBase.AddMinutes(minutos);
                res.Add(new string[]
                {
                    (i + 1).ToString(),
                    horaBase.ToString(ConstantesDpo.FormatoHoraMinuto),
                });
                i++;
            }

            return res;
        }

        /// <summary>
        /// Permite calcular el cuartil para juego de datos
        /// </summary>
        /// <param name="arreglo">conjunto de datos</param>
        /// <param name="cuartil">indica el cuartil que se desea calcular</param>
        /// <returns></returns>
        public static decimal CuartilIncluido(decimal[] arreglo, int cuartil)
        {
            if (arreglo.Length == 0)
            {
                return 0;//throw new ArgumentException("El arreglo debe contener al menos un elemento.");
            }

            if (arreglo.Length == 1)
            {
                return arreglo[0];
            }
            else if (arreglo.Length == 2 || arreglo.Length == 3) {
                if (cuartil == 1)
                {
                    return arreglo[0];
                }
                else {
                    return arreglo[arreglo.Length - 1];
                }
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

        /// <summary>
        /// Devuelve los días históricos correspondientes a una fecha consultada
        /// </summary>
        /// <param name="fecBase">Fecha base para el inicio de la busqueda</param>
        /// <param name="numDias">Número de días a buscar</param>
        /// <param name="fecNoValidas">Fechas no validas</param>
        /// <returns></returns>
        public static List<DateTime> ObtenerDiasHistoricos(
            DateTime fecBase, int numDias,
            List<DateTime> fecNoValidas)
        {
            List<int> reglaA = new List<int>()
            {
                (int)DayOfWeek.Monday,                
                (int)DayOfWeek.Saturday,
                (int)DayOfWeek.Sunday,
            };

            List<int> reglaB = new List<int>()
            {
                (int)DayOfWeek.Tuesday,
                (int)DayOfWeek.Wednesday,
                (int)DayOfWeek.Thursday,
                (int)DayOfWeek.Friday,
            };

            int diaSemana = (int)fecBase.DayOfWeek;
            List<DateTime> res = new List<DateTime>();
            if (reglaA.Contains(diaSemana))
            {
                DateTime d = fecBase;

                while (res.Count < numDias)
                {                  
                    d = d.AddDays(-7);
                    if (fecNoValidas.Contains(d)) continue;

                    res.Add(d);
                }
            }
            if (reglaB.Contains(diaSemana))
            {
                DateTime d = fecBase;

                while (res.Count < numDias)
                {
                    d = d.AddDays(-1);
                    if (fecNoValidas.Contains(d)) continue;
                    if (!reglaB.Contains((int)d.DayOfWeek)) continue;

                    res.Add(d);
                }
            }

            return res;
        }

        public static decimal?[] ConvertirMedEnAryNullable(
            int numIntervalos, object medicion)
        {
            decimal?[] arreglo = new decimal?[numIntervalos];

            if (medicion != null)
            {
                int i = 0;
                while (i < numIntervalos)
                {
                    var dValor = medicion.GetType()
                        .GetProperty("H" + (i + 1).ToString())
                        .GetValue(medicion, null);
                    arreglo[i] = (decimal?)dValor;
                    i++;
                }
            }
            return arreglo;
        }

        public static List<int> CompararAryNulleablePorItv(
            decimal?[] aryFirst, decimal?[] arySecond)
        {
            List<int> res = new List<int>();

            if (aryFirst.Length != arySecond.Length)
                return res;

            int i = 0;
            while (i < aryFirst.Length)
            {
                if (aryFirst[i] != arySecond[i]) res.Add(i);
                i++;
            }

            return res;
        }

        public static List<string> ObtenerColumnaIntervalos(
            List<DateTime> listaFechas, int intervalo)
        {
            List<string> res = new List<string>();
            foreach (DateTime dia in listaFechas)
            {
                for (int i = 1; i <= intervalo; i++)
                {
                    string str = dia.AddMinutes(i)
                        .ToString("yyy-MM-dd HH:mm:ss");
                    res.Add(str);
                }
            }

            return res;
        }
    }

    public class FiltrosDpo
    {
        /// <summary>
        /// Filtro F2
        /// </summary>
        /// <param name="datos">Columna a evaluar</param>
        /// <param name="tolMinuto">Toleracia(Umbral)</param>
        public static void FiltradoPorUmbral(
            List<decimal> datos, decimal tolMinuto)
        {
            List<int> indices = new List<int>();
            for (int i = 1; i < datos.Count; i++)
            {
                decimal diff = datos[i] - datos[i - 1];
                diff = Math.Abs(diff);
                if (diff > tolMinuto)
                    indices.Add(i);                    
            }
            
            for (int i = 0; i < datos.Count; i++)
            {
                if (indices.Contains(i))
                    datos[i] = 0;
            }
        }

        /// <summary>
        /// Filtro F2 considerando los valores null
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="tolMinuto"></param>
        /// <returns></returns>
        public static List<decimal> FiltradoPorUmbralNull(
            List<decimal?> datos, decimal tolMinuto)
        {
            List<int> indices = new List<int>();
            decimal[] res = new decimal[datos.Count];

            for (int i = 1; i < datos.Count; i++)
            {
                var v1 = datos[i];
                var v2 = datos[i - 1];
                if (v1 == null || v2 == null) continue;

                decimal diff = (decimal)datos[i] - (decimal)datos[i - 1];
                diff = Math.Abs(diff);
                if (diff > tolMinuto) indices.Add(i);
            }

            for (int i = 0; i < datos.Count; i++)
            {
                if (indices.Contains(i)) continue;
                if (datos[i] == null) continue;
                res[i] = (decimal)datos[i];
            }

            return res.ToList();
        }

        /// <summary>
        /// Filtro F3: Crea un promedio y completa faltantes
        /// si fuera necesario
        /// </summary>
        /// <param name="datos">Columna a evaluar</param>
        /// <returns></returns>
        public static List<decimal> PromedioSimplePorMinuto(
            List<decimal> datos)
        {
            int n = 1440;
            int dias = datos.Count / n;

            if (dias < 1) return datos;

            List<decimal> res = new List<decimal>();
            List<List<decimal>> tabla = new List<List<decimal>>();

            //Se crean las columnas x día
            for (int i = 0; i < dias; i++)
            {
                List<decimal> datosDia = datos
                    .GetRange((i * n), n);

                Interpolacion(datosDia);

                tabla.Add(datosDia);
            }

            for (int i = 0; i < n; i++)
            {
                List<decimal> fila = new List<decimal>();
                for (int j = 0; j < dias; j++)
                {
                    decimal valor = tabla[j][i];
                    //if (valor != 0) fila[j] = valor;
                    if (valor != 0) fila.Add(valor);
                }

                if (fila.Count == 0)
                {
                    res.Add(0);
                    continue;
                }
                decimal prom = fila.Sum() / fila.Count;                
                res.Add(prom);
            }

            //Formateo
            for (int i = 0; i < res.Count; i++)
            {
                res[i] = Math.Round(res[i], 2);
            }

            return res;
        }

        /// <summary>
        /// Filtro F4: Exclusión de perfíl no coincidente y
        /// obtención de promedio ponderado
        /// </summary>
        /// <param name="datos">Columna a evaluar</param>
        public static List<decimal> PromedioDepuradoPorMinuto(
            List<decimal> datos)
        {
            decimal tolDisEuc = 1.10M;
            decimal tolCorrPearson = 0.2M;
            int n = ConstantesDpo.Itv1min;
            int ndias = datos.Count / n;
            List<List<decimal>> bloques = new List<List<decimal>>();
            
            for (int i = 0; i < ndias; i++)
            {
                List<decimal> bloque = datos.GetRange((i * n), n);
                bloques.Add(bloque);
            }

            if (datos.Sum() == 0) return datos;

            #region Obtención de lista bool distancia euclidiana
            //Obtención de la matriz euclidiada
            List<List<decimal>> matrizEuclidiada = new List<List<decimal>>();
            foreach (List<decimal> bloque in bloques)
            {
                List<decimal> colFija = bloque.ToList();
                List<decimal> filaMatriz = new List<decimal>();

                foreach (List<decimal> colDinamica in bloques)
                {
                    List<decimal> calc = colFija.Zip(colDinamica,
                        (a, b) => (decimal)Math.Pow((double)(b - a), 2))
                        .ToList();

                    decimal valFila = (decimal)Math.Sqrt((double)calc.Sum());
                    filaMatriz.Add(valFila);
                }

                matrizEuclidiada.Add(filaMatriz);
            }

            List<decimal> matrizEuclidiadaProm = new List<decimal>();
            foreach (List<decimal> row in matrizEuclidiada)
            {
                if (row.Sum() == 0) continue;
                decimal valor = row.Where(x => x != 0).Average();
                matrizEuclidiadaProm.Add(valor);
            }

            List<bool> listaBoolDistEuc = new List<bool>();

            decimal totEucProm = (matrizEuclidiadaProm.Count == 0)
                ? 0 : matrizEuclidiadaProm.Average() * tolDisEuc;
            
            foreach (decimal prom in matrizEuclidiadaProm)
            {
                bool valor = (prom <= totEucProm) ? true : false;
                listaBoolDistEuc.Add(valor);
            }
            #endregion

            #region Obtención de lista bool correlación pearson
            //Obtención de la matriz Corr. Pearson
            List<List<decimal>> matrizPearson = new List<List<decimal>>();
            foreach (List<decimal> bloque in bloques)
            {
                List<decimal> colFija = bloque.ToList();
                List<decimal> filaMatriz = new List<decimal>();

                foreach (List<decimal> colDinamica in bloques)
                {                    
                    decimal valFila = CorrelacionPearson(colFija, colDinamica);
                    filaMatriz.Add(valFila);
                }

                matrizPearson.Add(filaMatriz);
            }

            //Se crea la tabla de valores bool
            List<List<bool>> listaBoolFiltro = new List<List<bool>>();
            foreach (List<decimal> fila in matrizPearson)
            {
                List<bool> filaBool = new List<bool>();
                foreach (decimal f in fila)
                {
                    if (f > 0 && f > totEucProm) filaBool.Add(true);
                    else filaBool.Add(false);
                }
                listaBoolFiltro.Add(filaBool);
            }

            //Se obtiene los indices a remover segun el filtro
            List<int> remover = new List<int>();
            for (int i = 0; i < listaBoolFiltro.Count; i++)
            {
                List<bool> fila = listaBoolFiltro[i];
                int t = fila.Where(x => x == true).Count();

                if (t >= fila.Count - 1)
                    remover.Add(i);                
            }

            //Se obtiene la nueva matriz de Corr. Pearson
            //Elimina los valores de las filas correspondintes
            List<List<decimal>> matrizPearsonFiltrada = matrizPearson.ToList();
            foreach (int i in remover)
            {
                matrizPearsonFiltrada.RemoveAt(i);
            }
            //Elimina los valores de las columnas correspondientes
            foreach (List<decimal> fila in matrizPearsonFiltrada)
            {
                foreach (int i in remover)
                {
                    fila.RemoveAt(i);
                }
            }

            //Obtiene el promedio de la nueva matriz
            List<decimal> matrizPearsonProm = new List<decimal>();
            foreach (List<decimal> fila in matrizPearsonFiltrada)
            {
                List<decimal> f = fila.Where(x => x > 0).ToList();
                decimal valor = (f.Count == 0) ? 0 : f.Average();

                matrizPearsonProm.Add(valor);
            }

            List<bool> listaBoolCorrPearson = new List<bool>();            
            foreach (decimal prom in matrizPearsonProm)
            {
                bool valor = (prom <= tolCorrPearson) ? true : false;
                listaBoolCorrPearson.Add(valor);
            }

            int valid = listaBoolCorrPearson
                .Where(x => x == true)
                .Count();
            if (valid <= 3)
            {
                for (int i = 0; i < listaBoolCorrPearson.Count; i++)
                {
                    listaBoolCorrPearson[i] = true;
                }
            }

            #endregion

            #region Obtención de promedio ponderado
            //Comparación de listas bool
            /*
             * index[0] = día 1(fecha más antigua)
             * index['last'] = día 'last'(fecha más reciente) 
             */
            List<bool> fechasCandidatas = new List<bool>();
            int indexEuc = 0;
            try
            {
                for (int i = 0; i < ndias; i++)
                {
                    if (remover.Contains(i)) continue;

                    bool val1 = (i > listaBoolDistEuc.Count - 1)
                        ? false : listaBoolDistEuc[i];
                    bool val2 = (i > listaBoolCorrPearson.Count - 1)
                        ? false : listaBoolCorrPearson[indexEuc];

                    if (val1 && val2)
                        fechasCandidatas.Add(true);
                    else
                        fechasCandidatas.Add(false);

                    indexEuc++;
                }
            }
            catch
            {
                Console.WriteLine("1");
            }
            

            //Calcula el promedio ponderado
            List<List<decimal>> medicionesCandidatas = new List<List<decimal>>();
            for (int i = 0; i < fechasCandidatas.Count; i++)
            {
                if (fechasCandidatas[i])
                    medicionesCandidatas.Add(bloques[i]);
            }

            int denPonderado = 0;
            for (int i = 0; i < medicionesCandidatas.Count; i++)
            {
                denPonderado += i + 1;
                medicionesCandidatas[i] = medicionesCandidatas[i]
                    .Select(x => x * (i + 1))
                    .ToList();
            }

            List<decimal> res = Enumerable
                .Repeat(0M, ConstantesDpo.Itv1min)
                .ToList();

            foreach (List<decimal> med in medicionesCandidatas)
            {
                res = res.Zip(med, (a, b) => a + b).ToList();
            }

            if (denPonderado != 0)
            {
                res = res.Select(x => x / denPonderado).ToList();
            }
            
            #endregion

            for (int i = 0; i < res.Count; i++)
            {
                res[i] = Math.Round(res[i], 2);
            }

            return res;
        }

        /// <summary>
        /// Filtro F5
        /// </summary>        
        /// <param name="arryMedicion">Señal a suavisar por el Alisado Gaussiano</param>
        /// <param name="length">longitud de la ventana para el alisado gaussiano</param>
        /// <param name="std">desvió estándar  para el alisado_gaussiano</param>
        public static void AlisadoGaussiano(
            List<decimal> arryMedicion, int length,
            double std)
        {
            // Aplicar convolución con el kernel gaussiano
            double[] kernel = GaussianWindow(length, std);

            double[] signal = new double[arryMedicion.Count];
            for (int i = 0; i < arryMedicion.Count; i++)
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
        
        public static void Interpolacion(
            List<decimal> datos)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < datos.Count; i++)
            {
                if (datos[i] != 0) indices.Add(i);
            }

            //Validaciones
            if (indices.Count == datos.Count) return;

            if (indices.Count < 2) return;
            
            if (indices.Count == 2)
                if (indices[1] - indices[0] == 1) return;

           //Evalua los rangos
            for (int i = 0; i < indices.Count - 1; i++)
            {
                int rIni = indices[i];
                int rFin = indices[i + 1];

                if (rFin - rIni == 1) continue;

                //Pendiente del rango
                decimal vIni = datos[rIni];
                decimal vFin = datos[rFin];

                decimal pendiente = Pendiente(
                    rIni, vIni, rFin, vFin);

                //Completa el rango
                for (int j = rIni + 1; j < rFin; j++)
                {
                    decimal valor = InterpolacionLineal2(
                        j, rIni, vIni, pendiente);

                    datos[j] = Math.Round(valor, 2);
                }
            }
        }

        /// <summary>
        /// Completa los datos haciendo uso de deltas
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="promedio"></param>
        public static void CompletandoDatos(
            List<decimal> datos, List<decimal> promedio)
        {
            CorreccionRangoDelta(datos, promedio);
            CorreccionExtremoDelta(datos, promedio);

            for (int i = 0; i < datos.Count; i++)
            {
                datos[i] = Math.Round(datos[i], 2);
            }
        }

        public static void CompletadoDatosTest(
            List<decimal> datos, List<decimal> promedio)
        {
            decimal[] col = datos.ToArray();

            //Rango inicio
            int indexIni = 0;
            for (int i = 1; i < datos.Count; i++)
            {
                if (datos[i] == 0) continue;
                indexIni = i;
                break;
            }

            if (indexIni != 0)
            {
                decimal diffIni = datos[indexIni] - promedio[indexIni];
                for (int i = indexIni - 1; i >= 0; i--)
                {
                    if (col[i] != 0) continue;
                    col[i] = promedio[i] + diffIni;
                }
            }

            //Rango final
            int indexFin = 0;

            for (int i = datos.Count - 2; i >= 0; i--)
            {
                if (datos[i] == 0) continue;
                indexFin = i;
                break;
            }

            if (indexFin != 0)
            {
                decimal diffFin = datos[indexFin] - promedio[indexFin];
                for (int i = indexFin + 1; i < datos.Count; i++)
                {
                    if (col[i] != 0) continue;
                    col[i] = promedio[i] + diffFin;
                }
            }
            
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i] == 0) continue;               
                col[i] = col[i] - promedio[i];
            }

            //completa los datos de la columna diff
            List<decimal> res = col.ToList();
            Interpolacion(res);

            for (int i = 0;i < res.Count; i++)
            {
                if (datos[i] != 0) continue;
                datos[i] = promedio[i] + res[i];
            }
        }

        /// <summary>
        /// Completa los datos haciendo uso de deltas para varios días
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="promedio"></param>
        /// <param name="ndias"></param>
        /// <returns></returns>
        public static List<decimal> CompletandoDatosPorDias(
            List<decimal> datos, List<decimal> promedio,
            int ndias)
        {
            List<decimal> res = new List<decimal>();

            int n = ConstantesDpo.Itv1min;
            for (int i = 0; i < ndias; i++)
            {
                List<decimal> bloque = datos.GetRange((i * n), n);
                CompletadoDatosTest(bloque, promedio);

                res.AddRange(bloque);
            }

            for (int i = 0; i < datos.Count; i++)
            {
                res[i] = Math.Round(res[i], 2);
            }

            return res;
        }

        /// <summary>
        /// Devuelve una función de ventana de Gauss -> Kernel
        /// </summary>        
        /// <param name="length">longitud de la ventana para el alisado_gaussiano</param>
        /// <param name="std">desvió estándar  para el alisado_gaussiano</param>
        public static double[] GaussianWindow(
            int length, double std)
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

        public static void CorreccionRangoDelta(
            List<decimal> datos, List<decimal> promedio)
        {
            List<int> indiceEntrada = new List<int>();
            List<decimal> valorEntrada = new List<decimal>();
            for (int i = 1; i < datos.Count - 1; i++)
            {
                if (datos[i] == 0) continue;

                //Obtiene los extremos del rango vacio
                indiceEntrada.Add(i);
                valorEntrada.Add(datos[i]);

                for (int j = i + 1; j < datos.Count - 1; j++)
                {
                    if (datos[j] == 0) continue;

                    if (j - indiceEntrada[0] == 1)
                    {
                        indiceEntrada[0] = j;
                        valorEntrada[0] = datos[j];
                        continue;
                    }

                    indiceEntrada.Add(j);
                    valorEntrada.Add(datos[j]);
                    if (indiceEntrada.Count >= 2) break;
                }

                if (indiceEntrada[0] == datos.Count - 2) break;
                if (indiceEntrada.Count < 2) continue;

                //Se obtiene los valores previos de cada extremo
                int indexPrevIni = indiceEntrada[0] - 1;
                decimal valorPrevIni = (datos[indexPrevIni] != 0) 
                    ? datos[indexPrevIni]
                    : promedio[indexPrevIni];

                int indexPrevFin = indiceEntrada[1] + 1;
                decimal valorPrevFin = (datos[indexPrevFin] != 0)
                    ? datos[indexPrevFin] 
                    : promedio[indexPrevFin];

                //Se calcula el valor extrapolado de cada extremo
                int indexExIni = indiceEntrada[0] + 1;
                decimal valorExIni = ExtrapolacionLineal(
                    indexPrevIni, valorPrevIni, indiceEntrada[0],
                    valorEntrada[0], indexExIni);

                int indexExFin = indiceEntrada[1] - 1;
                decimal valorExFin = ExtrapolacionLineal(
                    indiceEntrada[1], valorEntrada[1], indexPrevFin, 
                    valorPrevFin, indexExFin);

                decimal deltaIni = valorExIni - promedio[indexExIni];
                decimal deltaFin = valorExFin - promedio[indexExFin];

                //Obtención de la función lineal
                //. Se completan los deltas de los valores
                //  intermedios del rango
                int dimRango = (indexExFin - indexExIni) - 1;
                List<decimal> lineal = new List<decimal>();
                if (dimRango > 1)
                {                    
                    for (int j = 1; j <= dimRango; j++)
                    {
                        decimal valor = ((deltaIni + deltaFin) * -1);
                        valor = j * valor / dimRango;
                        lineal.Add(valor);
                    }
                }
                //. Se agregan los deltas extremos
                lineal.Insert(0, deltaIni);
                lineal.Add(deltaFin);

                try
                {
                    //Corrige el rango vacio
                    for (int j = 0; j < lineal.Count; j++)
                    {
                        datos[indexExIni + j] = promedio[indexExIni + j]
                            + deltaIni
                            + lineal[j];
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(indexExIni);
                }

                //Reinicia las listas e
                //inicia el iterador en el ultimo indice del rango
                i = indiceEntrada[1] - 1;
                indiceEntrada.Clear();
                valorEntrada.Clear();                
            }
        }

        public static void CorreccionExtremoSimple(
            List<decimal> datos)
        {
            //Corrección del rango inicial
            if (datos[0] == 0)
            {
                List<int> indiceEntrada = new List<int>();
                List<decimal> valorEntrada = new List<decimal>();

                //Se obtiene el rango inicial
                for (int i = 1; i < datos.Count; i++)
                {
                    if (indiceEntrada.Count == 2) break;

                    if (datos[i] != 0)
                    {
                        indiceEntrada.Add(i);
                        valorEntrada.Add(datos[i]);
                    }
                }

                //Corrección del rango inicial
                if (indiceEntrada.Count == 2)
                {                    
                    for (int i = 0; i < indiceEntrada[0]; i++)
                    {
                        decimal valor = ExtrapolacionLineal(
                            indiceEntrada[0], valorEntrada[0], indiceEntrada[1],
                            valorEntrada[1], i);
                        datos[i] = Math.Round(valor, 2);
                    }
                }
            }

            //Corrección del rango final
            if (datos[datos.Count - 1] == 0)
            {
                List<int> indiceEntrada = new List<int>();
                List<decimal> valorEntrada = new List<decimal>();

                //Se obtiene el rango final
                for (int i = datos.Count - 1; i >= 0; i--)
                {
                    if (indiceEntrada.Count == 2) break;

                    if (datos[i] != 0)
                    {
                        indiceEntrada.Add(i);
                        valorEntrada.Add(datos[i]);
                    }
                }

                //Corrección del rango final
                if (indiceEntrada.Count == 2)
                {
                    for (int i = datos.Count - 1; i > indiceEntrada[0]; i--)
                    {
                        datos[i] = ExtrapolacionLineal(
                            indiceEntrada[1], valorEntrada[1], indiceEntrada[0],
                            valorEntrada[0], i);
                    }
                }
            }
        }

        public static void CorreccionExtremoDelta(
            List<decimal> datos, List<decimal> promedio)
        {
            //Rango inicial faltante
            if (datos[0] == 0)
            {
                List<int> indiceEntrada = new List<int>();
                List<decimal> valorEntrada = new List<decimal>();

                //Se obtiene el rango inicial
                for (int i = 1; i < datos.Count; i++)
                {
                    if (indiceEntrada.Count == 2) break;

                    if (datos[i] != 0)
                    {
                        indiceEntrada.Add(i);
                        valorEntrada.Add(datos[i]);
                    }
                }

                if (indiceEntrada.Count == 2)
                {
                    //Se obtiene el valor extrapolado y el delta
                    int indiceEx = indiceEntrada[0] - 1;
                    decimal valorEx = ExtrapolacionLineal(
                        indiceEntrada[0], valorEntrada[0],
                        indiceEntrada[1], valorEntrada[1],
                        indiceEx);
                    decimal valorDelta = valorEx - promedio[indiceEx];

                    //Corrección del rango inicial
                    for (int i = 0; i < indiceEntrada[0]; i++)
                    {
                        datos[i] = promedio[i] + valorDelta;
                    }
                }                
            }

            //Rango final faltante
            if (datos[datos.Count - 1] == 0)
            {
                List<int> indiceEntrada = new List<int>();
                List<decimal> valorEntrada = new List<decimal>();

                //Se obtiene el rango final
                for (int i = datos.Count - 1; i >= 0; i--)
                {
                    if (indiceEntrada.Count == 2) break;

                    if (datos[i] != 0)
                    {
                        indiceEntrada.Add(i);
                        valorEntrada.Add(datos[i]);
                    }
                }

                if (indiceEntrada.Count == 2)
                {
                    //Se obtiene el valor extrapolado y el delta
                    int indiceEx = indiceEntrada[1] + 1;
                    decimal valorEx = ExtrapolacionLineal(
                            indiceEntrada[1], valorEntrada[1],
                            indiceEntrada[0], valorEntrada[0],
                            indiceEx);
                    decimal valorDelta = valorEx - promedio[indiceEx];

                    //Corrección del rango inicial
                    for (int i = datos.Count - 1; i > indiceEntrada[0]; i--)
                    {
                        datos[i] = promedio[i] + valorDelta;
                    }
                }
            }
        }

        public static decimal InterpolacionLineal(
            int x1, decimal y1, int x2, decimal y2,
            int x)
        {
            return y1 + (x - x1) * (y2 - y1) / (x2 - x1);
        }

        public static decimal InterpolacionLineal2(
            int x, int xRef, decimal yRef, decimal pendiente)
        {
            decimal diff = (x - xRef);
            diff = diff / 1440;
            return pendiente * diff + yRef;
        }

        public static decimal Pendiente(
            int x1, decimal y1, int x2, decimal y2)
        {
            decimal res = 0;
            try
            {
                decimal diff = x2 - x1;
                diff = diff / 1440;
                res = (y2 - y1) / diff;
            }
            catch
            {
                res = 0;
            }
            return res;
        }

        public static decimal ExtrapolacionLineal(
            int x1, decimal y1, int x2, decimal y2,
            int x) 
        {
            decimal m = (y2 - y1) / (x2 - x1);
            decimal res = y1 + m * (x - x1);
            return res;
        }

        public static decimal DistanciaEuclidiana(
            int x1, double y1, int x2, double y2)
        {
            double res = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            return (decimal)res;
        }

        public static decimal CorrelacionPearson(
            List<decimal> x, List<decimal> y)
        {            
            int n = x.Count;

            // Calcular las medias de x e y
            decimal promX = x.Average();
            decimal promY = x.Average();

            // Calcular los términos necesarios para el coeficiente de correlación
            decimal sumXY = 0;
            decimal sumX2 = 0;
            decimal sumY2 = 0;

            for (int i = 0; i < n; i++)
            {
                decimal diffX = x[i] - promX;
                decimal diffY = y[i] - promY;

                sumXY += diffX * diffY;
                sumX2 += diffX * diffX;
                sumY2 += diffY * diffY;
            }

            // Calcular el coeficiente de correlación
            decimal den = (decimal)Math.Sqrt((double)(sumX2 * sumY2));
            
            decimal res = (den == 0) ? 0 : sumXY / den;

            return res;
        }

        public static List<DpoMedicionTotal> ReduccionABarrasCP(
            List<DpoMedicionTotal> datos, DpoModRelacionTna modelo,
            int ndias)
        {            
            List<DpoMedicionTotal> res = new List<DpoMedicionTotal>();
            List<DpoMedicionTotal> dataVegetativa = new List<DpoMedicionTotal>();
            foreach (PrnRelacionTnaDTO rel in modelo.Relaciones)
            {
                if (rel.Reltnacodi == 33)
                    Console.WriteLine(rel.Reltnadetcodi);

                //Identifica si la relación es una "Anillo"
                bool esAnillo = (rel.Detalle.Count > 1) 
                    ? true : false;

                //Obtiene el componente vegetativo de la relación                
                List<DpoMedCompFormula> compVeg = new List<DpoMedCompFormula>();
                compVeg.AddRange(
                    modelo.ForInputCDispatch
                    .Where(x => x.idRelacion == rel.Reltnacodi)                    
                    .ToList());
                compVeg.AddRange(
                    modelo.ForInputEstimadorTna
                    .Where(x => x.idRelacion == rel.Reltnacodi)
                    .ToList());

                decimal[] compVegMed = ObtenerTotalComponentes(
                    compVeg, datos, ndias);

                //Obtiene el factor de aporte de cada barra CP perteneciente a la relación
                List<dynamic[]> dataBarrasCPRelacion = new List<dynamic[]>();
                List<decimal[]> vegetativaPorBarraCP = new List<decimal[]>();
                foreach (dynamic[] r in rel.Detalle)
                {
                    int iBarra = r[0];
                    int iFormula = r[2];

                    List<DpoMedCompFormula> compVegBarraCP = new List<DpoMedCompFormula>();
                    compVegBarraCP.AddRange(
                        modelo.ForVegCDispatch
                        .Where(x => x.idRelFormula == iFormula)
                        .ToList());
                    compVegBarraCP.AddRange(
                        modelo.ForVegEstimadorTna
                        .Where(x => x.idRelFormula == iFormula)
                        .ToList());

                
                    decimal[] compVegBarraCPMed = ObtenerTotalComponentes(
                        compVegBarraCP, datos, ndias);

                    vegetativaPorBarraCP.Add(compVegBarraCPMed);
                    dataBarrasCPRelacion.Add(new dynamic[] { iBarra, compVegBarraCPMed });
                }

                foreach (dynamic[] d in dataBarrasCPRelacion)
                {
                    DpoMedicionTotal entidadVegetativa = new DpoMedicionTotal
                    {
                        Ptomedicodi = d[0],
                        Medicion = new List<decimal>(),
                    };

                    decimal[] factorAporte = (esAnillo)
                        ? ObtenerFactorAporte(d[1], vegetativaPorBarraCP)
                        : Enumerable.Repeat((decimal)1, (ConstantesDpo.Itv1min * ndias)).ToArray();

                    for (int i = 0; i < (ConstantesDpo.Itv1min * ndias); i++)
                    {
                        entidadVegetativa.Medicion.Add(compVegMed[i] * factorAporte[i]);
                    }

                    dataVegetativa.Add(entidadVegetativa);
                }
            }

            //Agrupa los diferentes componentes de cada barra CP
            List<int> barrasCP = dataVegetativa
                .Select(x => x.Ptomedicodi)
                .Distinct()
                .ToList();
            foreach (int barraCP in barrasCP)
            {
                DpoMedicionTotal entidad = new DpoMedicionTotal()
                {
                    Ptomedicodi = barraCP,
                    Medicion = new List<decimal>(),
                };
                List<DpoMedicionTotal> demandaPorBarra = dataVegetativa
                    .Where(x => x.Ptomedicodi == barraCP)
                    .ToList();
                List<decimal> finalDemanda = Enumerable
                    .Repeat(0M, (ConstantesDpo.Itv1min * ndias))
                    .ToList();
                foreach (DpoMedicionTotal d in demandaPorBarra)
                {
                    finalDemanda = finalDemanda
                        .Zip(d.Medicion, (a, b) => a + b)
                        .ToList();
                }

                entidad.Medicion = finalDemanda;
                res.Add(entidad);
            }

            return res;
        }

        public static List<DpoMedicion1440> DarFormato1440(
            List<DpoEstimadorRawDTO> datos, List<int> puntosMedicion, 
            DateTime fecha)
        {
            DpoMedicion1440 entidad = new DpoMedicion1440();
            List<DpoMedicion1440> res = new List<DpoMedicion1440>();

            DateTime rangoLimite = fecha.AddHours(23).AddMinutes(59);

            foreach (int idPunto in puntosMedicion)
            {
                entidad = new DpoMedicion1440();                
                entidad.Ptomedicodi = idPunto;
                entidad.Medifecha = fecha;
                entidad.Medicion = new decimal[ConstantesDpo.Itv1min];

                //Obtiene los datos por punto
                List<DpoEstimadorRawDTO> datosPunto = datos
                    .Where(x => x.Ieod == idPunto)
                    .ToList();

                if (datosPunto.Count == 0)
                {
                    res.Add(entidad);
                    continue;
                }

                //Asigna las mediciones en orden
                //Obtiene los valores por intervalo hora
                for (int i = 0; i < 24; i++)
                {
                    DateTime intervaloHora = new DateTime(
                        fecha.Year, fecha.Month, fecha.Day,
                        i, 0, 0);

                    DpoEstimadorRawDTO datoHora = datosPunto
                        .FirstOrDefault(x => x.Dporawfecha == intervaloHora);

                    if (datoHora == null) continue;

                    int j = 0;
                    while (j < 60)
                    {
                        var r = datoHora.GetType()
                            .GetProperty($"Dporawvalorh{j + 1}")
                            .GetValue(datoHora);

                        if (r != null)
                            entidad.Medicion[(i * 60) + j] = (decimal)r;
                        j++;
                    }
                }

                res.Add(entidad);
            }

            return res;
        }

        public static List<DpoMedicion1440> DarFormato1440Null(
            List<DpoEstimadorRawDTO> datos, List<int> puntosMedicion,
            DateTime fecha)
        {
            DpoMedicion1440 entidad = new DpoMedicion1440();
            List<DpoMedicion1440> res = new List<DpoMedicion1440>();

            DateTime rangoLimite = fecha.AddHours(23).AddMinutes(59);

            foreach (int idPunto in puntosMedicion)
            {
                entidad = new DpoMedicion1440();
                entidad.Ptomedicodi = idPunto;
                entidad.Medifecha = fecha;
                entidad.MedicionNull = new decimal?[ConstantesDpo.Itv1min];

                //Obtiene los datos por punto
                List<DpoEstimadorRawDTO> datosPunto = datos
                    .Where(x => x.Ieod == idPunto)
                    .ToList();

                if (datosPunto.Count == 0)
                {
                    res.Add(entidad);
                    continue;
                }

                //Asigna las mediciones en orden
                //Obtiene los valores por intervalo hora
                for (int i = 0; i < 24; i++)
                {
                    DateTime intervaloHora = new DateTime(
                        fecha.Year, fecha.Month, fecha.Day,
                        i, 0, 0);

                    DpoEstimadorRawDTO datoHora = datosPunto
                        .FirstOrDefault(x => x.Dporawfecha == intervaloHora);

                    if (datoHora == null) continue;

                    int j = 0;
                    while (j < 60)
                    {
                        var r = datoHora.GetType()
                            .GetProperty($"Dporawvalorh{j + 1}")
                            .GetValue(datoHora);

                        if (r != null)
                            entidad.MedicionNull[(i * 60) + j] = (decimal)r;
                        j++;
                    }
                }

                res.Add(entidad);
            }

            return res;
        }

        public static List<DpoMedicion1440> DarFormato1440Generacion(
            List<MeMedicion48DTO> datos, List<int> puntosMedicion,
            DateTime fecha)
        {
            DpoMedicion1440 entidad = new DpoMedicion1440();
            List<DpoMedicion1440> res = new List<DpoMedicion1440>();

            foreach (int idPunto in puntosMedicion)
            {
                entidad = new DpoMedicion1440();
                entidad.Ptomedicodi = idPunto;
                entidad.Medifecha = fecha;
                entidad.Medicion = new decimal[ConstantesDpo.Itv1min];

                MeMedicion48DTO datosPunto = datos
                    .FirstOrDefault(x => x.Ptomedicodi == idPunto);

                if (datosPunto == null)
                {
                    res.Add(entidad);
                    continue;
                }

                decimal[] medicion = new decimal[1440];
                for (int i = 1; i <= 48; i++)
                {
                    var valid = datosPunto.GetType()
                        .GetProperty($"H{(i)}")
                        .GetValue(datosPunto);
                    decimal valor = (valid != null) ? (decimal)valid : 0;
                    medicion[i * 30 - 1] = valor;
                }

                entidad.Medicion = medicion;

                res.Add(entidad);
            }

            return res;
        }

        public static List<DpoMedicion1440> DarFormato1440DemandaSCO(
            List<DpoMedicion96DTO> datos, List<int> puntosMedicion,
            DateTime fecha)
        {
            DpoMedicion1440 entidad = new DpoMedicion1440();
            List<DpoMedicion1440> res = new List<DpoMedicion1440>();

            foreach (int idPunto in puntosMedicion)
            {
                entidad = new DpoMedicion1440();
                entidad.Ptomedicodi = idPunto;
                entidad.Medifecha = fecha;
                entidad.Medicion = new decimal[ConstantesDpo.Itv1min];

                DpoMedicion96DTO datosPunto = datos
                    .FirstOrDefault(x => x.Dpomedcodi == idPunto);

                if (datosPunto == null)
                {
                    res.Add(entidad);
                    continue;
                }

                decimal[] medicion = new decimal[1440];
                for (int i = 1; i <= 48; i++)
                {
                    var valid = datosPunto.GetType()
                        .GetProperty($"H{(i * 2)}")
                        .GetValue(datosPunto);
                    decimal valor = (valid != null) ? (decimal)valid : 0;
                    medicion[i * 30 - 1] = valor;
                }

                entidad.Medicion = medicion;

                res.Add(entidad);
            }

            return res;
        }

        /// <summary>
        /// Método que calcula el factor de aporte de una barra CP
        /// </summary>
        /// <param name="datosBarra">Datos de la medición de la barra CP (obtenida de la fórmula asociada)</param>
        /// <param name="datosAnillo">Datos de las mediciones  de las barras CP que conforman el anillo
        /// (datos obtenidos de la formula asociada *por cada barra)</param>
        /// <returns></returns>
        public static decimal[] ObtenerFactorAporte(
            decimal[] datosBarra, List<decimal[]> datosAnillo)
        {
            decimal[] factorAporte = datosBarra;
            decimal[] totalSumatoria = new decimal[datosBarra.Length];

            foreach (decimal[] dato in datosAnillo)
                totalSumatoria = totalSumatoria
                    .Zip(dato, (a, b) => (a + b != 0) ? a + b : 1)
                    .ToArray();

            factorAporte = factorAporte
                .Zip(totalSumatoria, (a, b) => a / b)
                .ToArray();

            return factorAporte;
        }

        /// <summary>
        /// Método que calcula el total valor de un conjunto de componentes de formula
        /// </summary>
        /// <param name="componentes">Componentes de la formula</param>
        /// <param name="datos">Valores de las mediciones por componente</param>
        /// <param name="ndias">dias de referencia</param>
        /// <returns></returns>
        public static decimal[] ObtenerTotalComponentes(
            List<DpoMedCompFormula> componentes,
            List<DpoMedicionTotal> datos, int ndias)
        {
            decimal[] res = new decimal[ConstantesDpo.Itv1min * ndias];

            foreach (DpoMedCompFormula componente in componentes)
            {
                DpoMedicionTotal dato = datos
                    .FirstOrDefault(x => x.Ptomedicodi == componente.idCompFormula) 
                    ?? new DpoMedicionTotal();

                if (dato.Ptomedicodi == 0) continue;

                res = res
                    .Zip(dato.Medicion, (a, b) => a + (b * componente.constante))
                    .ToArray();
            }

            return res;
        }
    }
}
