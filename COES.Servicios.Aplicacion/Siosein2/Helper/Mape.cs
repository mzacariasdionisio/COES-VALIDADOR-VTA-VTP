using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
namespace COES.Servicios.Aplicacion.Siosein2.Helper2
{
    public class Mape
    {
        public List<MapMedicion48DTO> ListaHistoricaMape;
        public List<MapDemandaDTO> ListaIndicadores;

        public Mape(List<MapMedicion48DTO> lista, List<MapDemandaDTO> lindicadores)
        {
            this.ListaHistoricaMape = lista;
            this.ListaIndicadores = lindicadores;
        }

        public Tuple<DateTime,decimal> GetMaximaDemandaMensualSein(int anho,int mes)
        {
            
            Tuple<DateTime, decimal> mayor,regDia;
            mayor = new Tuple<DateTime, decimal>(DateTime.MinValue, 0);
            var listaDatos = this.ListaHistoricaMape.Where(x => x.Medicfecha.Year == anho && x.Medicfecha.Month == mes && x.Tipoccodi == (int)ConstantesSiosein2.TipoCalculo.DemandaReal);
            foreach(var reg in listaDatos)
            {
                regDia = MaximoValor(reg);
                if(regDia.Item2 > mayor.Item2)
                {
                    mayor = new Tuple<DateTime, decimal>( regDia.Item1, regDia.Item2);
                }
            }
            return mayor;
        }
        /// <summary>
        /// Maxima Demanda Mensual por Tio, Sein, Coes, Programado
        /// </summary>
        /// <param name="anho"></param>
        /// <param name="mes"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public Tuple<DateTime, decimal> GetMaximaDemandaMensualTipo(int anho, int mes,int tipo)
        {

            Tuple<DateTime, decimal> mayor, regDia;
            mayor = new Tuple<DateTime, decimal>(DateTime.MinValue, 0);
            var listaDatos = this.ListaHistoricaMape.Where(x => x.Medicfecha.Year == anho && x.Medicfecha.Month == mes && x.Tipoccodi == tipo);
            foreach (var reg in listaDatos)
            {
                regDia = MaximoValor(reg);
                if (regDia.Item2 > mayor.Item2)
                {
                    mayor = new Tuple<DateTime, decimal>(regDia.Item1, regDia.Item2);
                }
            }
            return mayor;
        }

        public void GenerarDemandaMensual(DateTime fecha,int vermcodi)
        {

        }
        //public Tuple<DateTime, decimal> GetMaximaDemandaMensualInstantane(int anho, int mes)
        //{

        //    Tuple<DateTime, decimal> mayor, regDia;
        //    mayor = new Tuple<DateTime, decimal>(DateTime.MinValue, 0);
        //    var listaDatos = this.ListaHistoricaMensual.Where(x => x.Medifecha.Year == anho && x.Medifecha.Month == mes && x.Lectcodi == ConstantesSiosein2.LecturaEjecutado);
        //    foreach (var reg in listaDatos)
        //    {
        //        regDia = MaximoValor(reg);
        //        if (regDia.Item2 > mayor.Item2)
        //        {
        //            mayor = new Tuple<DateTime, decimal>(regDia.Item1, regDia.Item2);
        //        }
        //    }
        //    return mayor;
        //}

        public Tuple<DateTime, decimal> GetMaximaDemandaPeriodo(DateTime fini, DateTime ffin)
        {
            Tuple<DateTime, decimal> mayor, regDia;
            mayor = new Tuple<DateTime, decimal>(DateTime.MinValue, 0);
            var listaDatos = this.ListaHistoricaMape.Where(x => x.Medicfecha >= fini && x.Medicfecha <= ffin && x.Tipoccodi == (int)ConstantesSiosein2.TipoCalculo.DemandaReal);
            foreach (var reg in listaDatos)
            {
                regDia = MaximoValor(reg);
                if (regDia.Item2 > mayor.Item2)
                {
                    mayor = new Tuple<DateTime, decimal>(regDia.Item1, regDia.Item2);
                }
            }
            return mayor;
        }

        public Tuple<DateTime,decimal> MaximoValor(MapMedicion48DTO registro)
        {
            int imayor = 1;
            decimal valorMayor = 0;
            decimal val1;
            for(int i = 1; i <= 48; i++)
            {
                val1 = (decimal?)registro.GetType().GetProperty("H" + i).GetValue(registro, null) ?? 0;
                if (val1 > valorMayor)
                {
                    imayor = i;
                    valorMayor = val1;
                }
            }
            return new Tuple<DateTime, decimal>(registro.Medicfecha.AddMinutes(imayor * 30), valorMayor);
        }

        public decimal? GetDesvioDiario(DateTime fecha)
        {
            decimal? valor = null;
            var registro = this.ListaHistoricaMape.Find(x => x.Medicfecha == fecha && x.Tipoccodi == (int)ConstantesSiosein2.TipoCalculo.Desvio);
            if(registro != null)
                valor = (registro.Medicpromedio);
            return valor;
        }

        public decimal? GetTipoIndicadorDiario(DateTime fecha, int tipoccodi)
        {
            decimal? valor = null;
            var registro = this.ListaHistoricaMape.Find(x => x.Medicfecha == fecha && x.Tipoccodi == tipoccodi);
            if (registro != null)
                valor = (registro.Medicpromedio);
            return valor;
        }

        public decimal GetDesvioPromedioMensual(int anho,int mes)
        {
            decimal valor = 0;
            var lista = this.ListaHistoricaMape.Where(x => x.Medicfecha.Year == anho && x.Medicfecha.Month == mes && x.Tipoccodi == (int)ConstantesSiosein2.TipoCalculo.Desvio);
            valor = lista.Average(x => x.Medicpromedio) ?? 0;
            return valor;
        }

        public decimal GetTipoPromedioMensual(int anho, int mes,int tipo)
        {
            decimal valor = 0;
            var lista = this.ListaHistoricaMape.Where(x => x.Medicfecha.Year == anho && x.Medicfecha.Month == mes && x.Tipoccodi == tipo);
            valor = lista.Average(x => x.Medicpromedio) ?? 0;
            return valor;
        }

        public List<MapMedicion48DTO> GetListaDesvioMensual(int anho, int mes)
        {
            return this.ListaHistoricaMape.Where(x => x.Medicfecha.Year == anho && x.Medicfecha.Month == mes && x.Tipoccodi == (int)ConstantesSiosein2.TipoCalculo.Desvio).ToList();
        }

        public MapDemandaDTO GetMaximaDemandaAnual(int anho, int tipo)
        {
            MapDemandaDTO resultado = new MapDemandaDTO();
            decimal maxDem = 0;
            var findAnual = ListaIndicadores.Find(x => x.Mapdemtipo == tipo && x.Mapdemperiodo == ConstantesSiosein2.Anual
                        && x.Mapdemfechaperiodo.Year == anho);

            if (findAnual != null)
            {
                resultado = findAnual;
            }
            else
            {
                var lista = ListaIndicadores.Where(x => x.Mapdemtipo == tipo && x.Mapdemperiodo == ConstantesSiosein2.Mensual
            && x.Mapdemfechaperiodo.Year == anho);
                foreach (var reg in lista)
                {
                    if (reg.Mapdemvalor != null)
                    {
                        if (reg.Mapdemvalor > maxDem)
                        {
                            resultado = reg;
                        }
                    }
                }
            }

            return resultado;
        }

        public decimal GetDesvEstandarDiario(DateTime fecha)
        {
            double valor = .1;
            decimal? val1;
            List<decimal> lista = new List<decimal>();
            var registro = this.ListaHistoricaMape.Find(x => x.Medicfecha == fecha && x.Tipoccodi == (int)ConstantesSiosein2.TipoCalculo.Desvio);
            if(registro != null)
            for (int i = 1;i <=48; i++)
            {
                val1 = (decimal?)registro.GetType().GetProperty("H" + i).GetValue(registro, null);
                lista.Add(val1 ?? 0);
            }
            if(lista.Count > 0)
                valor = DesviacionEstandar(lista);
            return (decimal)valor;
        }

        public decimal GetDesviacionEstandarMes(int anho,int mes)
        {
            double valor = -1;
            decimal? val1;
            List<decimal> lista = new List<decimal>();
            var listaDatos = this.ListaHistoricaMape.Where(x => x.Medicfecha.Year == anho && x.Medicfecha.Month == mes && x.Tipoccodi == (int)ConstantesSiosein2.TipoCalculo.Desvio);
            foreach (var reg in listaDatos)
            {
                for (int i = 1; i <= 48; i++)
                {
                    val1 = (decimal?)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                    lista.Add(val1 ?? 0);
                }
            }
            if(lista.Count > 0)
                valor = DesviacionEstandar(lista);
            return (decimal)valor;
        }

        public decimal GetDesviacionEstandarAnual(int anho)
        {
            double valor = -1;
            decimal? val1;
            List<decimal> lista = new List<decimal>();
            var listaDatos = this.ListaHistoricaMape.Where(x => x.Medicfecha.Year == anho && x.Tipoccodi == (int)ConstantesSiosein2.TipoCalculo.Desvio);
            foreach (var reg in listaDatos)
            {
                for (int i = 1; i <= 48; i++)
                {
                    val1 = (decimal?)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                    lista.Add(val1 ?? 0);
                }
            }
            if (lista.Count > 0)
                valor = DesviacionEstandar(lista);
            return (decimal)valor;
        }

        public decimal GetDesvioPromedioAnual(int anho)
        {
            decimal valor = 0;
            var lista = this.ListaHistoricaMape.Where(x => x.Medicfecha.Year == anho && x.Tipoccodi == (int)ConstantesSiosein2.TipoCalculo.Desvio).ToList();
            if (lista.Count > 0)
                valor = lista.Average(x => x.Medicpromedio) ?? 0;
            return valor;
        }

        /// <summary>
        /// Calcula Mape Anual Hasta una fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public decimal GetMapeAnualInstantaneo(DateTime fecha, int tipo)
        {
            decimal result1 = 0, totDias1 = 0;
            decimal valor = 0;
           if(fecha.Month > 1)
            {

                var lmeses = this.ListaIndicadores.Where(x => x.Mapdemtipo == tipo && x.Mapdemperiodo == ConstantesSiosein2.Mensual
                                        && x.Mapdemfechaperiodo.Year == fecha.Year && x.Mapdemfechaperiodo.Month < fecha.Month);
                foreach(var reg in lmeses)
                {
                    result1 += (reg.Mapdemvalor ?? 0) * reg.Mapdemfechafin.Day;
                    totDias1 += reg.Mapdemfechafin.Day;
                }
                if(totDias1 > 0)
                 valor = result1 / totDias1;

            }
            var lista = this.ListaHistoricaMape.Where(x => x.Medicfecha.Year == fecha.Year && x.Medicfecha.Month == fecha.Month &&
             x.Medicfecha.Day <= fecha.Day && x.Tipoccodi == tipo);
            int totDias2 = lista.Count();
            decimal valorDia = lista.Average(x => x.Medicpromedio) ?? 0;
            if(totDias2 + totDias1 > 0)
                valor = (valor * totDias1 + valorDia * totDias2) / (totDias1 + totDias2);
            return valor;
        }

        /// <summary>
        /// Devuelve el Mape Minimo mensual del añho
        /// </summary>
        /// <param name="anho"></param> Año del calculo
        /// <param name="tipo"></param> Indica si es corregido o sin corregido
        /// <returns></returns> retorna el valor y el mes
        public Tuple<DateTime, decimal> GetAnualMapeMinMensual(int anho,int tipo)
        {
            Tuple<DateTime, decimal>  resultado = new Tuple<DateTime, decimal>(DateTime.MinValue, 0);
           
            var lista = ListaIndicadores.Where(x => x.Mapdemtipo == tipo && x.Mapdemperiodo == ConstantesSiosein2.Mensual
                    && x.Mapdemfechaperiodo.Year == anho).ToList();
            decimal? vmin = lista.Min(x => x.Mapdemvalor);
            if(vmin != null)
            {
                var find = lista.Find(x => x.Mapdemvalor == vmin);
                if (find != null)
                {
                    resultado = new Tuple<DateTime, decimal>(find.Mapdemfechaperiodo, (decimal)vmin);
                }
            }
            return resultado;
        }

        /// <summary>
        /// Devuelve el Mape Minimo diario del añho
        /// </summary>
        /// <param name="anho"></param> Año del calculo
        /// <param name="tipo"></param> Indica si es corregido o sin corregido
        /// <returns></returns> retorna el valor y el mes
        public Tuple<DateTime, decimal> GetAnualMapeMinDiario(int anho, int tipo)
        {
            Tuple<DateTime, decimal> resultado = new Tuple<DateTime, decimal>(DateTime.MinValue, 0);

            var lista = ListaIndicadores.Where(x => x.Mapdemtipo == tipo && x.Mapdemperiodo == ConstantesSiosein2.Mensual
                    && x.Mapdemfechaperiodo.Year == anho).ToList();
            decimal? vmin = lista.Min(x => x.Mapdemvalor);
            if (vmin != null)
            {
                var find = lista.Find(x => x.Mapdemvalor == vmin);
                if (find != null)
                {
                    resultado = new Tuple<DateTime, decimal>(find.Mapdemfecha, (decimal)vmin);
                }
            }
            return resultado;
        }
        /// <summary>
        /// Devuelve el Mape Maximo mensual del añho
        /// </summary>
        /// <param name="anho"></param> Año del calculo
        /// <param name="tipo"></param> Indica si es corregido o sin corregido
        /// <returns></returns> retorna el valor y el mes
        public Tuple<DateTime, decimal> GetAnualMapeMaxMensual(int anho, int tipo)
        {
            Tuple<DateTime, decimal> resultado = new Tuple<DateTime, decimal>(DateTime.MinValue, 0);

            var lista = ListaIndicadores.Where(x => x.Mapdemtipo == tipo && x.Mapdemperiodo == ConstantesSiosein2.Mensual
                    && x.Mapdemfechaperiodo.Year == anho).ToList();
            decimal? vmin = lista.Max(x => x.Mapdemvalor);
            if (vmin != null)
            {
                var find = lista.Find(x => x.Mapdemvalor == vmin);
                if (find != null)
                {
                    resultado = new Tuple<DateTime, decimal>(find.Mapdemfechaperiodo, (decimal)vmin);
                }
            }
            return resultado;
        }

        /// <summary>
        /// Devuelve el Mape Maximo diario del añho
        /// </summary>
        /// <param name="anho"></param> Año del calculo
        /// <param name="tipo"></param> Indica si es corregido o sin corregido
        /// <returns></returns> retorna el valor y el mes
        public Tuple<DateTime, decimal> GetAnualMapeMaxDiario(int anho, int tipo)
        {
            Tuple<DateTime, decimal> resultado = new Tuple<DateTime, decimal>(DateTime.MinValue, 0);

            var lista = ListaIndicadores.Where(x => x.Mapdemtipo == tipo && x.Mapdemperiodo == ConstantesSiosein2.Mensual
                    && x.Mapdemfechaperiodo.Year == anho).ToList();
            decimal? vmin = lista.Max(x => x.Mapdemvalor);
            if (vmin != null)
            {
                var find = lista.Find(x => x.Mapdemvalor == vmin);
                if (find != null)
                {
                    resultado = new Tuple<DateTime, decimal>(find.Mapdemfecha, (decimal)vmin);
                }
            }
            return resultado;
        }
        /// <summary>
        /// Obtiene el valor de la media desde una lista de valores dobles
        /// </summary>
        /// <param name="Values">The list of values</param>
        /// <returns>The mean/average of the list</returns>
        public static double Media(List<double> Values)
        {
            if (Values.Count == 0)
                return 0.0;
            double ReturnValue = 0.0;
            for (int x = 0; x < Values.Count; ++x)
            {
                ReturnValue += Values[x];
            }
            return ReturnValue / (double)Values.Count;
        }
        /// <summary>
        /// Calcula la varianza de una lista de valores
        /// </summary>
        /// <param name="Values">List of values</param>
        /// <returns>The variance</returns>
        public static double Varianza(List<double> Values)
        {
            if (Values == null || Values.Count == 0)
                return 0;
            double MeanValue = Media(Values);
            double Sum = 0;
            for (int x = 0; x < Values.Count; ++x)
            {
                Sum += Math.Pow(Values[x] - MeanValue, 2);
            }
            return Sum / (double)Values.Count;
        }

        public static double DesviacionEstandar(List<double> Values)
        {
            return Math.Sqrt(Varianza(Values));
        }

        /// <summary>
        /// Obtiene la variacion estandar
        /// </summary>
        /// <param name="Values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double DesviacionEstandar(List<decimal> Values)
        {
            List<double> values1 = Values.Select(i => (double)i).ToList();
            return DesviacionEstandar(values1);
        }

        private List<decimal> GetListaRangoMaxMape()
        {
            List<decimal> lista = new List<decimal> { -725, -675, -625, -575, -525, -475, -425, -375, -325, -275, -225, -175, -125, -75, -25, 25, 75, 125, 175, 225, 275, 325, 375, 425, 475 };
            return lista;
        }

        private List<decimal> GetListaRangoMinMape()
        {
            List<decimal> lista = new List<decimal> { -675, -625, -575, -525, -475, -425, -375, -325, -275, -225, -175, -125, -75, -25, 25, 75, 125, 175, 225, 275, 325, 375, 425, 475, 525 };
            return lista;
        }
        /// <summary>
        /// Calcula la desviacion anual para un rango de años
        /// </summary>
        /// <param name="anhoIni"></param>
        /// <param name="anhofin"></param>
        /// <returns></returns>
        public List<MapMedicion48DTO> DistribucionAnual(int anhoIni,int anhofin)
        {
            List<MapMedicion48DTO> lista = new List<MapMedicion48DTO>();
            MapMedicion48DTO registro;
            var listaMax = GetListaRangoMaxMape();
            var listaMin = GetListaRangoMinMape();
            decimal? val1;
            List<decimal> listaTotal = new List<decimal> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int anho = anhoIni; anho <= anhofin; anho++)
            {
                registro = new MapMedicion48DTO();
                registro.Medicfecha = new DateTime(anho, 1, 1);
                var listaDatos = this.ListaHistoricaMape.Where(x => x.Medicfecha.Year == anho && x.Tipoccodi == (int)ConstantesSiosein2.TipoCalculo.Desvio);
                listaTotal = new List<decimal> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                foreach (var reg in listaDatos)
                {

                    for (int h = 1; h <= 48; h++)
                    {
                        val1 = (decimal?)reg.GetType().GetProperty("H" + h).GetValue(reg, null);
                        if (val1 != null)
                        {
                            for (int z = 0; z < 25; z++)
                            {
                                if (listaMax[z] < val1 && listaMin[z] > val1)
                                {
                                    listaTotal[z] += 1;
                                }
                            }
                        }

                    }

                }
                for (int z = 1; z <= 25; z++)
                {
                    registro.GetType().GetProperty("H" + z).SetValue(registro, listaTotal[z - 1]);
                }
                lista.Add(registro);
            }
            return lista;
        }

    }
}
