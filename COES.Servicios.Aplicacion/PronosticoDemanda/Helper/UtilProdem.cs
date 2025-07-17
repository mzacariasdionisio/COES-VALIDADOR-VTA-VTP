using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using System.Reflection;
using System.Globalization;
using COES.Base.Core;

namespace COES.Servicios.Aplicacion.PronosticoDemanda.Helper
{
    public class UtilProdem
    {
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
                case ConstantesProdem.Itv15min: minutos = 15; break;
                case ConstantesProdem.Itv30min: minutos = 30; break;
                case ConstantesProdem.Itv60min: minutos = 60; break;
            }

            int i = 0;
            while (i < numIntervalos)
            {
                horaBase = horaBase.AddMinutes(minutos);
                intervalos[i] = horaBase.ToString(ConstantesProdem.FormatoHoraMinuto);
                i++;
            }

            return intervalos;
        }

        /// <summary>
        /// Helper que gernera los intervalos de tiempo concatenados con un rango de fechas
        /// </summary>
        /// <param name="numIntervalos"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static string[] GenerarIntervalosFecha(int numIntervalos, string fechaInicio, string fechaFin)
        {
            DateTime horaBase = new DateTime();
            DateTime inicio = DateTime.ParseExact(fechaInicio, ConstantesProdem.FormatoFecha, CultureInfo.CurrentCulture);
            DateTime fin = DateTime.ParseExact(fechaFin, ConstantesProdem.FormatoFecha, CultureInfo.CurrentCulture);

            int dias = Convert.ToInt32((fin - inicio).TotalDays) + 1;
            string[] intervalos = new string[numIntervalos * dias];

            int minutos = 0;
            switch (numIntervalos)
            {
                case ConstantesProdem.Itv15min: minutos = 15; break;
                case ConstantesProdem.Itv30min: minutos = 30; break;
                case ConstantesProdem.Itv60min: minutos = 60; break;
            }


            int i = 0;
            DateTime fecha = inicio;
            while (i < numIntervalos * dias)
            {
                horaBase = horaBase.AddMinutes(minutos);
                fecha = fecha.AddMinutes(minutos);
                intervalos[i] = fecha.ToString("dd/MM/yyyy") + " " + horaBase.ToString(ConstantesProdem.FormatoHoraMinuto);
                i++;
            }

            return intervalos;
        }

        /// <summary>
        /// Helper que genera la linea patron de un grupo de mediciones (old)
        /// </summary>
        public static decimal[] GenerarLineaPatron(decimal[][] mediciones, int numDias, int numIntervalos, string tipo)
        {
            int x = numDias;
            int y = numIntervalos;
            decimal[] patron = new decimal[y];

            if (x != 0)
            {
                switch (tipo)
                {
                    case ConstantesProdem.PatronByMediana:
                        #region Mediana
                        {
                            List<decimal> row = new List<decimal>();
                            for (int i = 0; i < y; i++)
                            {
                                row = new List<decimal>();
                                for (int j = 0; j < x; j++)
                                {
                                    row.Add(mediciones[j][i]);                                    
                                }
                                //Ordena
                                row = row.OrderBy(a => a).ToList();
                                //Valida si es par o impar
                                if (x % 2 == 0)//Par
                                {
                                    patron[i] = (row[x / 2] + row[x / 2 + 1]) / 2;
                                }
                                else//Impar
                                {
                                    patron[i] = row[x + 1 / 2];
                                }
                            }
                        }
                        #endregion
                        break;
                    case ConstantesProdem.PatronByPromedio:
                        #region Promedio
                        {
                            decimal promedio = 0;
                            for (int i = 0; i < y; i++)
                            {
                                promedio = 0;
                                for (int j = 0; j < x; j++)
                                {
                                    promedio += mediciones[j][i];
                                }

                                patron[i] = promedio / x;
                            }
                        }
                        #endregion
                        break;
                    case ConstantesProdem.PatronByRegresionLineal:
                        #region Regresión Lineal

                        #endregion
                        break;
                }
            }

            return patron;
        }

        /// <summary>
        /// Helper que calcula el perfil patrón segun el parámetro de configuración
        /// </summary>
        /// <param name="mediciones"></param>
        /// <param name="numDias"></param>
        /// <param name="numIntervalos"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static decimal[] CalcularPerfilPatron(List<decimal[]> mediciones, int numDias, int numIntervalos, string tipo)
        {
            int x = numDias;
            int y = numIntervalos;
            decimal[] patron = new decimal[y];
            if (mediciones.Count == 0) return patron;
            if (x != 0)
            {
                switch (tipo)
                {
                    case ConstantesProdem.PatronByMediana:
                        #region Mediana
                        {
                            List<decimal> row = new List<decimal>();
                            for (int i = 0; i < y; i++)
                            {
                                row = new List<decimal>();
                                for (int j = 0; j < x; j++)
                                {
                                    row.Add(mediciones[j][i]);
                                }
                                //Ordena
                                row = row.OrderBy(a => a).ToList();
                                //Valida si es par o impar
                                if (x % 2 == 0)//Par
                                {
                                    patron[i] = (row[x / 2] + row[x / 2 + 1]) / 2;
                                }
                                else//Impar
                                {
                                    patron[i] = row[x + 1 / 2];
                                }
                            }
                        }
                        #endregion
                        break;
                    case ConstantesProdem.PatronByPromedio:
                        #region Promedio
                        {
                            decimal promedio = 0;
                            for (int i = 0; i < y; i++)
                            {
                                promedio = 0;
                                for (int j = 0; j < x; j++)
                                {
                                    promedio += mediciones[j][i];
                                }

                                patron[i] = promedio / x;
                            }
                        }
                        #endregion
                        break;
                    case ConstantesProdem.PatronByRegresionLineal:
                        #region Regresión Lineal

                        #endregion
                        break;
                }
            }

            return patron;
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
        /// Helper que convierte una lista en arreglo
        /// </summary>
        /// <param name="numIntervalos"></param>
        /// /// <param name="dias"></param>
        /// <param name="medicion"></param>
        /// <returns></returns>
        public static decimal[] ConvertirListaMedicionEnArreglo(int numIntervalos, int dias, List<PrnMediciongrpDTO> medicion)
        {
            decimal[] arreglo = new decimal[numIntervalos * dias];

            if (medicion != null)
            {
                int i = 0;
                foreach (var item in medicion)
                {
                    int h = 0;
                    while (h < numIntervalos)
                    {
                        var dValor = item.GetType().GetProperty("H" + (h + 1).ToString()).GetValue(item, null) ?? (decimal)0;
                        arreglo[i] = (decimal)dValor;
                        i++;
                        h++;
                    }
                }
            }
            return arreglo;
        }

        /// <summary>
        /// Helper que permite convertir una lista de entidades en una cadena
        /// </summary>
        /// <param name="eList"></param>
        /// <param name="eAttr"></param>
        /// <returns></returns>
        public static string ConvertirEntityListEnString(dynamic eList, string eAttr)
        {
            string res = String.Empty;
            List<int> intArray = new List<int>();

            foreach (var eItem in eList)
            {
                int x = eItem.GetType().GetProperty(eAttr).GetValue(eItem, null);
                intArray.Add(x);
            }

            res = (intArray.Count != 0) ? string.Join(",", intArray) : "0";
            return res;
        }

        /// <summary>
        /// Helper que permite convertir una lista de entidades en una lista de enteros
        /// </summary>
        /// <param name="eList"></param>
        /// <param name="eAttr"></param>
        /// <returns></returns>
        public static List<int> ConvertirEntityListEnIntList(dynamic eList, string eAttr)
        {
            List<int> res = new List<int>();
            foreach (var eItem in eList)
            {
                int x = eItem.GetType().GetProperty(eAttr).GetValue(eItem, null);
                res.Add(x);
            }

            return res;
        }

        /// <summary>
        /// Valida que la información ingresada sea un numero valido, caso contrario devuelve cero
        /// /// </summary>
        /// <param name="sValor">Cadena de texto</param>
        public static decimal ValidarNumero(string sValor)
        {
            decimal dNumero;
            if (!sValor.Equals("") && (Decimal.TryParse(sValor, out dNumero)))
            {
                return dNumero;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Valida el identificador PRNM48TIPO para cada tipo de día defecto del perfíl patron
        /// </summary>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public static int ValidarPatronDiaDefecto(DateTime regFecha)
        {
            switch (regFecha.DayOfWeek)
            {
                case DayOfWeek.Sunday: return ConstantesProdem.PrntPatronDefDomingo;
                case DayOfWeek.Monday: return ConstantesProdem.PrntPatronDefLunes;
                case DayOfWeek.Saturday: return ConstantesProdem.PrntPatronDefSabado;
                default: return ConstantesProdem.PrntPatronDefMaMiJV;
            }
        }

        /// <summary>
        /// Lista de áreas operativas s/n Sein
        /// </summary>
        /// <param name="sein">Flag que incluye como valor al área Sein (true)</param>
        /// <returns>
        /// Item1: Ptomedicodi
        /// Item2: Areacodi
        /// Item3: Nombre
        /// Item4: Flag "selected"
        /// </returns>
        public static List<Tuple<int, int, string, bool>> ListAreaOperativa(bool sein)
        {
            List<Tuple<int, int, string, bool>> entitys = new List<Tuple<int, int, string, bool>>();

            entitys.Add(new Tuple<int, int, string, bool>(ConstantesProdem.PtomedicodiASur, ConstantesProdem.AreacodiASur, "Sur", true));
            entitys.Add(new Tuple<int, int, string, bool>(ConstantesProdem.PtomedicodiANorte, ConstantesProdem.AreacodiANorte, "Norte", false));
            entitys.Add(new Tuple<int, int, string, bool>(ConstantesProdem.PtomedicodiASierraCentro, ConstantesProdem.AreacodiASierraCentro, "Sierra centro", false));
            entitys.Add(new Tuple<int, int, string, bool>(ConstantesProdem.PtomedicodiACentro, ConstantesProdem.AreacodiACentro, "Centro", false));

            if (sein) entitys.Add(new Tuple<int, int, string, bool>(ConstantesProdem.PtomedicodiASein, -1, "Sein", false));

            return entitys;
        }

        /// <summary>
        /// Lista de tipos de empresa (Usuarios libres y Distribuidores solo)
        /// </summary>
        /// <returns>
        /// Item1: Id
        /// Item2: Nombre
        /// Item3: Flag "selected"
        /// </returns>
        public static List<Tuple<int, string, bool>> ListTipoEmpresa()
        {
            List<Tuple<int, string, bool>> entitys = new List<Tuple<int, string, bool>>();

            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.TipoemprcodiDistribuidores, "Distribuidores", true));
            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.TipoemprcodiUsuLibres, "Usuarios libres", false));

            return entitys;
        }

        /// <summary>
        /// Lista de tipos de demanda diaria(Ejecutada y prevista)
        /// </summary>
        /// <returns>
        /// Item1: Id
        /// Item2: Nombre
        /// Item3: Flag "selected"
        /// </returns>
        public static List<Tuple<int, string, bool>> ListTipoDemandaDiaria()
        {
            List<Tuple<int, string, bool>> entitys = new List<Tuple<int, string, bool>>();

            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.LectcodiDemEjecDiario, "Ejecutada", true));
            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.LectcodiDemPrevDiario, "Prevista", false));
            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.LectcodiDemPrevSemanal, "Semanal", false));

            return entitys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// Item1: Id
        /// Item2: Nombre
        /// Item3: Flag "selected"
        /// </returns>
        public static List<Tuple<int, string, bool>> ListTipoDemanda()
        {
            List<Tuple<int, string, bool>> entitys = new List<Tuple<int, string, bool>>();

            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.LectcodiDemEjecDiario, "Diaria Ejecutada", false));
            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.LectcodiDemPrevDiario, "Diaria Prevista", false));
            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.LectcodiDemPrevSemanal, "Semanal", false));

            return entitys;
        }

        /// <summary>
        /// Lista de tipos de perfiles defecto por día
        /// </summary>
        /// <returns>
        /// Item1: Id
        /// Item2: Nombre
        /// Item3: Flag "selected"
        /// </returns>
        public static List<Tuple<int, string, bool>> ListTipoPatronDiaDefecto()
        {
            List<Tuple<int, string, bool>> entitys = new List<Tuple<int, string, bool>>();

            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.PrntPatronDefLunes, "Lunes", true));
            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.PrntPatronDefMaMiJV, "Martes, Miercoles, Jueves y Viernes", false));
            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.PrntPatronDefSabado, "Sábado", false));
            entitys.Add(new Tuple<int, string, bool>(ConstantesProdem.PrntPatronDefDomingo, "Domingo", false));

            return entitys;
        }

        /// <summary>
        /// Lista simple de elección "Si" o "No"
        /// </summary>
        /// <returns>
        /// Item1 : Id
        /// Item2: Descripción
        /// Item3: Flag "selected"
        /// </returns>
        public static List<Tuple<string, string, bool>> ListSimpleSiNo()
        {
            List<Tuple<string, string, bool>> entitys = new List<Tuple<string, string, bool>>();

            entitys.Add(new Tuple<string, string, bool>(ConstantesProdem.RegSi, "Si", true));
            entitys.Add(new Tuple<string, string, bool>(ConstantesProdem.RegNo, "No", false));

            return entitys;
        }

        /// <summary>
        /// Helper que devuelve los los valores "t" para el pronóstico de la demanda y el valor "tFinal" del día a pronosticar
        /// </summary>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="NDias">Número de dias historicos a utilizar para el pronóstico</param>
        /// <returns></returns>
        public static Tuple<int, List<int>> PronosticoReglaDias(DateTime regFecha, int NDias)
        {
            int valTFinal = 0;
            List<int> valTHistoricos = new List<int>();
            int diaSemana = (int)regFecha.DayOfWeek;

            List<int> ruleA = new List<int> {
                ConstantesProdem.DiaAsIntLunes,
                ConstantesProdem.DiaAsIntSabado,
                ConstantesProdem.DiaAsIntDomingo };

            List<int> ruleB = new List<int> {
                ConstantesProdem.DiaAsIntLunes,
                ConstantesProdem.DiaAsIntMartes,
                ConstantesProdem.DiaAsIntMiercoles,
                ConstantesProdem.DiaAsIntJueves,
                ConstantesProdem.DiaAsIntViernes };

            if (ruleA.Contains(diaSemana))
            {
                //Obtiene los valores "t" para los días historicos
                for (int i = 0; i < NDias; i++)
                {
                    valTHistoricos.Add(7 * i);
                }
                valTHistoricos = valTHistoricos.OrderByDescending(x => x).ToList();

                //Obtiene el valor "t" para el día a pronosticar
                valTFinal = 7 * NDias;
            }
            if (ruleB.Contains(diaSemana))
            {
                //Obtiene los valores "t" para los días historicos
                List<DateTime> tempDates = new List<DateTime>() { regFecha };

                DateTime auxDate = regFecha;
                int i = 0;
                while (i < NDias)
                {
                    auxDate = auxDate.AddDays(-1);
                    if (ruleB.Contains((int)auxDate.DayOfWeek))
                    {
                        tempDates.Add(auxDate);
                        i++;
                    }
                }

                auxDate = tempDates.OrderBy(x => x).First();
                foreach (DateTime d in tempDates)
                {
                    valTHistoricos.Add((d - auxDate).Days);
                }

                //Obtiene el valor "t" para el día a pronosticar
                valTFinal = valTHistoricos.First();
                valTHistoricos.Remove(0);
            }

            return new Tuple<int, List<int>>(valTFinal, valTHistoricos);
        }

        /// <summary>
        /// Tranforma una lista de string en cadena para el IN sql.
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static string ListToString(List<string> tipo)
        {
            string tipos = "";
            int c = 0;
            if (tipo.Count == 0)
            {
                tipos = "0";
            }
            else
            {
                foreach (var item in tipo)
                {
                    if (c == 0)
                    {
                        tipos += "(";
                    }
                    tipos += "'" + item.ToString().Trim() + "'";
                    c++;
                    if (tipo.Count > c)
                    {
                        tipos += ", ";
                    }
                    if (c == tipo.Count)
                    {
                        tipos += ")";
                    }
                }
            }
            return tipos;
        }

        /// <summary>
        /// Devuelve los bloques de información como arreglo según parámetros
        /// </summary>
        /// <param name="indice">Punto de partida para la lectura</param>
        /// <param name="separador">Punto de termino de la lectura</param>
        /// <param name="lista">Lista a recorrer</param>
        /// <returns></returns>
        public static List<string[]> ObtenerBlqInfoRaw(int indice, string separador, List<string> lista)
        {
            List<string[]> l = new List<string[]>();
            while (lista[indice] != separador)
            {
                l.Add(lista[indice].Split(new char[] { ',', '/' }));
                indice++;
            }

            return l;
        }

        /// <summary>
        /// Devuelve el intervalo de tiempo (Hx) correspondiente a la hora parametro
        /// </summary>
        /// <param name="hora">Hora en formato HH:mm</param>
        /// <param name="tipoIntervalo">Tipo de intervalo a generar (15 o 30 min)</param>
        /// <returns></returns>
        public static string ObtenerIntervalo(string hora, int tipoIntervalo)
        {
            DateTime date = DateTime.MinValue;
            DateTime date2 = DateTime.MinValue;
            int m = (tipoIntervalo == ConstantesProdem.Itv15min) ? 15 : 30;
            date = date.AddMinutes(m);
            date2 = date2.AddMinutes((m * 2));

            string strM = date.ToString(ConstantesProdem.FormatoHoraMinuto);
            int i = 1;
            while (!strM.Equals(hora))
            {
                date = date.AddMinutes(1);
                strM = date.ToString(ConstantesProdem.FormatoHoraMinuto);
                if (date == date2)
                {
                    date2 = date2.AddMinutes(m);
                    i++;
                }
            }

            //ultimo intervalo
            if (hora.Equals("23:59"))
            {
                List<int> ruleIntervalo = new List<int>() {
                    ConstantesProdem.Itv15min, ConstantesProdem.Itv30min };
                if (ruleIntervalo.Contains(tipoIntervalo)) i += 1;
            }

            return "H" + i;
        }

        /// <summary>
        /// Devuelve las fechas historicas a partir de una fecha base
        /// </summary>
        /// <param name="fechaRegistro">Fecha de partida para la busqueda</param>
        /// <param name="numDias">Cantidad de días historicos a buscar</param>
        /// <returns></returns>
        public static PrnPatronModel ObtenerFechasHistoricas(DateTime fechaRegistro, int numDias)
        {
            PrnPatronModel model = new PrnPatronModel();
            model.Fechas = new List<DateTime>();
            model.StrFechas = new List<string>();
            model.StrFechasTarde = new List<string>();

            DateTime fechaTemporal = new DateTime();
            int diaSemana = (int)fechaRegistro.DayOfWeek;
            List<int> ruleA = new List<int> { ConstantesProdem.DiaAsIntLunes, ConstantesProdem.DiaAsIntSabado,
                ConstantesProdem.DiaAsIntDomingo };
            List<int> ruleB = new List<int> { ConstantesProdem.DiaAsIntMartes,
                ConstantesProdem.DiaAsIntMiercoles, ConstantesProdem.DiaAsIntJueves, ConstantesProdem.DiaAsIntViernes };


            if (ruleA.Contains(diaSemana))
            {
                fechaTemporal = fechaRegistro;

                int d = 0;
                while (d < numDias)
                {                    
                    fechaTemporal = fechaTemporal.AddDays(-7);                    
                    model.Fechas.Add(fechaTemporal);
                    model.StrFechas.Add(fechaTemporal.ToString(ConstantesProdem.FormatoFecha));
                    d++;
                }
            }

            if (ruleB.Contains(diaSemana))
            {
                fechaTemporal = fechaRegistro;

                int d = 0;
                bool esLunes = false;
                if (diaSemana.Equals(ConstantesProdem.DiaAsIntLunes))
                {
                    esLunes = true;
                    model.EsLunes = true;
                    model.StrFechasTarde = new List<string>();
                }

                while (d < numDias)
                {
                    fechaTemporal = fechaTemporal.AddDays(-1);
                    diaSemana = (int)fechaTemporal.DayOfWeek;
                    while (ruleA.Contains(diaSemana))
                    {
                        fechaTemporal = fechaTemporal.AddDays(-1);
                        diaSemana = (int)fechaTemporal.DayOfWeek;
                    }
                    if (esLunes)
                    {
                        model.StrFechasTarde.Add(fechaTemporal.ToString(ConstantesProdem.FormatoFecha));
                    }
                    else
                    {
                        model.Fechas.Add(fechaTemporal);
                        model.StrFechas.Add(fechaTemporal.ToString(ConstantesProdem.FormatoFecha));
                        model.StrFechasTarde.Add(fechaTemporal.ToString(ConstantesProdem.FormatoFecha));
                    }
                    d++;
                }
            }

            return model;
        }

        /// <summary>
        /// Convierte una lista de intervalos de valores de PRN_ESTIMADORRAW en una Medición de 48 intervalos
        /// </summary>
        /// <param name="dataMediciones">Valores de la tabla PRN_ESTIMADORRAW</param>
        /// <param name="fuente">Identificador de la fuente de información (ieod, sco)</param>
        /// <returns></returns>
        public static List<PrnEstimadorRawDTO> ConvertirItvRawEnMediciones48(List<PrnEstimadorRawDTO> dataMediciones, int fuente)
        {
            PrnEstimadorRawDTO entidad;
            List<PrnEstimadorRawDTO> entidades = new List<PrnEstimadorRawDTO>();

            List<string> fechas = dataMediciones
                .GroupBy(x => x.Etmrawfecha.ToString(ConstantesProdem.FormatoFecha))
                .Select(x => x.First().Etmrawfecha.ToString(ConstantesProdem.FormatoFecha))
                .ToList();
            List<int> puntos = dataMediciones
                .GroupBy(x => x.Ptomedicodi)
                .Select(x => x.First().Ptomedicodi)
                .ToList();

            foreach (int p in puntos)
            {
                List<PrnEstimadorRawDTO> dataRaw = dataMediciones
                    .Where(x => x.Ptomedicodi == p)
                    .ToList();
                foreach (string f in fechas)
                {
                    entidad = new PrnEstimadorRawDTO
                    {
                        Ptomedicodi = p,
                        Etmrawfecha = DateTime.ParseExact(f, ConstantesProdem.FormatoFecha,
                            CultureInfo.InvariantCulture),
                        Etmrawfuente = fuente
                    };
                    List<PrnEstimadorRawDTO> dataRawPorDia = dataRaw
                        .Where(x => x.Etmrawfecha.ToString(ConstantesProdem.FormatoFecha) == f)
                        .OrderBy(x => x.Etmrawfecha)
                        .ToList();
                    foreach (PrnEstimadorRawDTO r in dataRawPorDia)
                    {
                        string intervalo = r.Etmrawfecha.ToString(ConstantesProdem.FormatoHoraMinuto);
                        string attrH = ObtenerIntervalo(intervalo, ConstantesProdem.Itv30min);
                        int i = entidad.GetType()
                            .GetProperties()
                            .Count(x => x.Name.Equals(attrH));
                        if (i > 0) entidad.GetType()
                                .GetProperty(attrH)
                                .SetValue(entidad, r.Etmrawvalor);
                    }
                    entidades.Add(entidad);
                }
            }

            return entidades;
        }

        /// <summary>
        /// Valida si la hora parametro pertenece a los intervalos permitidos
        /// </summary>
        /// <param name="hora">Hora del archivo raw(HH:mm)</param>
        /// <param name="tipoIntervalo">Tipo de intervalo a validar (15[96] o 30[48] min)</param>
        /// <returns></returns>
        public static bool ValidarIntervaloRaw(string hora, int tipoIntervalo)
        {
            if (hora.Equals("23:59")) return true;

            bool valid = true;
            DateTime date = DateTime.MinValue;
            int m = 0;
            if (tipoIntervalo == ConstantesProdem.Itv15min) m = 15;
            if (tipoIntervalo == ConstantesProdem.Itv30min) m = 30;

            date = date.AddMinutes(m);
            string strM = date.ToString(ConstantesProdem.FormatoHoraMinuto);
            int i = 1;
            while (!strM.Equals(hora))
            {
                if (i == tipoIntervalo)
                {
                    valid = false;
                    break;
                }

                date = date.AddMinutes(m);
                strM = date.ToString(ConstantesProdem.FormatoHoraMinuto);
                i++;
            }

            return valid;
        }

        /// <summary>
        /// Método que completa una medición raw
        /// </summary>
        /// <param name="medicionBase">Medición que sera completada</param>
        /// <param name="medicionComplemento">Medición utilizada para completar base</param>
        /// <returns></returns>
        public static decimal[] CompletarMedicionRaw(decimal[] medicionBase, decimal[] medicionComplemento)
        {
            int numIntervalos = medicionBase.Length;
            decimal[] medicion = new decimal[numIntervalos];

            for (int i = 0; i < numIntervalos; i++)
            {
                medicion[i] = (medicionBase[i] != 0)
                    ? medicionBase[i] : medicionComplemento[i];
            }

            return medicion;
        }

        /// <summary>
        /// Método que calcula el factor de aporte de una barra CP
        /// </summary>
        /// <param name="datosBarra">Datos de la medición de la barra CP (obtenida de la fórmula asociada)</param>
        /// <param name="datosAnillo">Datos de las mediciones  de las barras CP que conforman el anillo
        /// (datos obtenidos de la formula asociada *por cada barra)</param>
        /// <returns></returns>
        public static decimal[] ObtenerFactorAporte(decimal[] datosBarra, List<decimal[]> datosAnillo)
        {
            decimal[] factorAporte = datosBarra;
            decimal[] totalSumatoria = new decimal[ConstantesProdem.Itv30min];

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
        /// Calcula el pronóstico aplicando el método de suavizado exponencial
        /// </summary>
        /// <param name="dataMedicion">datos de los días historicos base para el pronóstico</param>
        /// <param name="dAlfa">Valor "Alfa" para el procedimiento</param>
        /// <returns></returns>
        public static decimal[] SuavizadoExponencial(List<decimal[]> dataMedicion, decimal dAlfa)
        {
            decimal[] datos = new decimal[ConstantesProdem.Itv30min];
            if (dataMedicion.Count == 0) return datos;

            decimal a = 0;
            int i = 0, j = 0;
            //Recorrido por intervalo de tiempo
            while (i < ConstantesProdem.Itv30min)
            {
                //Reinicia el valor de alfa para el siguiente intervalo
                a = dAlfa;

                //Recorrido por cada medición
                j = 0;
                while (j < dataMedicion.Count)
                {
                    //Obtiene el valor del pronóstico para el intervalo
                    datos[i] += dataMedicion[j][i] * a;

                    //Cálcula el valor de alfa para la siguiente medición
                    a *= (1 - dAlfa);
                    j++;
                }

                //Para el ultimo valor del cálculo
                datos[i] += dataMedicion[dataMedicion.Count - 1][i] * (decimal)Math.Pow((double)(1 - dAlfa), dataMedicion.Count);
                i++;
            }

            return datos;
        }

        /// <summary>
        /// Obtiene el día histórico valido más reciente segun el día de la semana solicitado
        /// </summary>
        /// <param name="fechaBase">Fecha base para el inicio de la busqueda</param>
        /// <param name="diaSemana">Día de la semana a buscar de referencia</param>
        /// <param name="rangoDias">Rango de días opcional</param>
        /// <param name="esDiaTipico">Flag que indica si se busca un día tipico o no</param>
        /// <returns></returns>
        public static DateTime ObtenerUltimoDiaHistoricoValido(DateTime fechaBase,
            int diaSemana, 
            List<DateTime> rangoDias = null,
            bool esDiaTipico = false)
        {
            int limDiasBusqueda = 365;
            DateTime fechaResultado = new DateTime(fechaBase.Year, 
                fechaBase.Month,
                fechaBase.Day);
            List<int> reglaMaMiJuVi = new List<int>
            {
                (int)DayOfWeek.Tuesday,
                (int)DayOfWeek.Wednesday,
                (int)DayOfWeek.Thursday,
                (int)DayOfWeek.Friday
            };

            int i = 0;
            int resDiaSemana = 0;
            while (i < limDiasBusqueda)
            {
                i++;
                
                fechaResultado = fechaResultado.AddDays(-1);
                if (rangoDias != null)
                {
                    if (!rangoDias.Contains(fechaResultado)) continue;
                    if (!esDiaTipico)
                        if (rangoDias.Contains(fechaResultado)) break;
                }


                resDiaSemana = (int)fechaResultado.DayOfWeek;
                if (reglaMaMiJuVi.Contains(diaSemana))
                {
                    if (reglaMaMiJuVi.Contains(resDiaSemana)) break;
                }
                if (diaSemana.Equals((int)DayOfWeek.Monday))
                {
                    if (resDiaSemana.Equals((int)DayOfWeek.Monday)) break;
                }
                if (diaSemana.Equals((int)DayOfWeek.Saturday))
                {
                    if (resDiaSemana.Equals((int)DayOfWeek.Saturday)) break;
                }
                if (diaSemana.Equals((int)DayOfWeek.Sunday))
                {
                    if (resDiaSemana.Equals((int)DayOfWeek.Sunday)) break;
                }
            }

            return fechaResultado;
        }

        /// <summary>
        /// Devuelve un número de días historicos segun regla de una rango días determinado
        /// </summary>
        /// <param name="fechaBase">Fecha inicial de iteración</param>
        /// <param name="numDias">Número de días a buscar</param>
        /// <param name="rangoDias">Rango de días base</param>
        /// <param name="esDiaTipico">Flag que indica si se busca un día tipico o no</param>
        /// <returns></returns>
        public static List<DateTime> ObtenerFechasHistoricasPorRango(DateTime fechaBase,
            int numDias,
            List<DateTime> rangoDias,
            bool esDiaTipico)
        {
            int limDiasBusqueda = 365;
            List<int> reglaA = new List<int>
            {
                (int)DayOfWeek.Monday,
                (int)DayOfWeek.Saturday,
                (int)DayOfWeek.Sunday,
            };
            List<int> reglaB = new List<int>
            {
                (int)DayOfWeek.Tuesday,
                (int)DayOfWeek.Wednesday,
                (int)DayOfWeek.Thursday,
                (int)DayOfWeek.Friday,
            };

            int i = 0;
            int diaSemana = (int)fechaBase.DayOfWeek;
            List<DateTime> resultado = new List<DateTime>();
            if (!esDiaTipico)
            {
                while (i < limDiasBusqueda)
                {
                    fechaBase = fechaBase.AddDays(-1);
                    if (rangoDias.Contains(fechaBase))
                        resultado.Add(fechaBase);
                    if (resultado.Count == numDias) break;
                    i++;
                }
                return resultado;
            }

            if (reglaA.Contains(diaSemana))
            {
                while (i < limDiasBusqueda)
                {
                    fechaBase = fechaBase.AddDays(-7);
                    if (rangoDias.Contains(fechaBase))
                        resultado.Add(fechaBase);
                    if (resultado.Count == numDias) break;
                    i += 7;
                }
            }
            if (reglaB.Contains(diaSemana))
            {
                while (i < limDiasBusqueda)
                {                    
                    fechaBase = fechaBase.AddDays(-1);
                    diaSemana = (int)fechaBase.DayOfWeek;
                    if (reglaB.Contains(diaSemana))
                        if (rangoDias.Contains(fechaBase))
                            resultado.Add(fechaBase);
                    if (resultado.Count == numDias) break;
                    i++;
                }
            }

            return resultado;
        }
    }
}