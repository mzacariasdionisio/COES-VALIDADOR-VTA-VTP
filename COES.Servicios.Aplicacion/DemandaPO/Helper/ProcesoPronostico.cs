using System;
using System.Collections.Generic;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using COES.Infraestructura.Datos.Repositorio.Sic;
using System.Security.Cryptography;
using System.Globalization;
using System.Configuration;

namespace COES.Servicios.Aplicacion.DemandaPO.Helper
{
    public class ProcesoPronostico
    {
        private static string rutaReporte = ConfigurationManager.AppSettings[ConstantesDpo.RutaReporteProDemVeg];        

        //CU01 - Proceso días tipicos
        public List<DemVegCol> EjecutarDemVegTipico(
            DateTime dia, string idVersion, DpoConfiguracionDTO configuracion)
        {
            //Constantes tipo de informacion
            string idTipo = "4";

            //Variables
            int ndias = configuracion.Dpocngdias;//4
            decimal tolMinuto = configuracion.Dpocngumbral;//10
            int etapas = configuracion.Dpocngpromedio;//1
            decimal porcentaje = configuracion.Dpocngtendencia;//0
            int vgm = configuracion.Dpocngvmg;//15
            double std = (double)configuracion.Dpocngstd;//1.645
            DateTime hora = DateTime.ParseExact(configuracion.Dpocngfechora,
                "HH:mm", CultureInfo.InvariantCulture);

            int rango = etapas * 60;            
            List<DateTime> fechas = FechasDiasTipicos(dia, ndias);

            //---------------------------------------------------------------------------
            //• Procesamiento de datos Historicos
            //---------------------------------------------------------------------------
            //CU002 - Subflujo 3
            List<DemVegCol> datosGeneracion = GeneracionFechasPorMinuto(fechas);
            //Exportación del caso de prueba [CP001]
            List<string> CP001_itv = ObtenerColumnaIntervalos(fechas, 1, false);
            ReportesDpo.DemVegReporteSimple(datosGeneracion, CP001_itv,
               "CP001_Generacion",
               $"{rutaReporte}\\CP001_Generacion.xlsx",
               "Id");

            //CU002 - Subflujo 4
            List<DpoFormulaDTO> listaFormulas = FactorySic
                .GetDpoProcesoPronosticoRepository()
                .ObtenerFormulas();
            List<DemVegCol> datosDemanda = DemandaFechasPorMinuto(
                fechas, listaFormulas, false);
            //Exportación del caso de prueba [CP002]
            List<string> CP002_itv = ObtenerColumnaIntervalos(fechas, 1, false);
            ReportesDpo.DemVegReporteSimple(datosDemanda, CP002_itv,
               "CP002_Demanda",
               $"{rutaReporte}\\CP002_Demanda.xlsx",
               "Id");

            //CU002 - Subflujo 5
            foreach (DemVegCol dem in datosDemanda)
            {
                dem.Valores = FiltradoPorUmbral(dem.Valores, tolMinuto);
            }
            //Exportación del caso de prueba [CP003]
            List<string> CP003_itv = ObtenerColumnaIntervalos(fechas, 1, false);
            ReportesDpo.DemVegReporteSimple(datosDemanda, CP003_itv,
               "CP003_Demanda",
               $"{rutaReporte}\\CP003_Demanda.xlsx",
               "Id");

            //CU002 - Subflujo 6
            List<DemVegCol> averageSimple = CalcPromedioSimplePorMinuto(
                datosDemanda);
            //Exportación del caso de prueba [CP004]
            List<string> CP004_itv = Enumerable.Range(0, 1440)
                .Select(x => x.ToString())
                .ToList();
            ReportesDpo.DemVegReporteSimple(averageSimple, CP004_itv,
               "CP004_Promedio",
               $"{rutaReporte}\\CP004_Promedio.xlsx",
               "Id");

            //CU002 - Subflujo 7
            List<DemVegCol> demandaCompleta = CompletadoDatosDemanda(
                datosDemanda, averageSimple, "Id");
            //Exportación del caso de prueba [CP005]
            ReportesDpo.DemVegReporteSimple(demandaCompleta, CP003_itv,
               "CP005_Demanda",
               $"{rutaReporte}\\CP005_Demanda.xlsx",
               "Id");

            //CU002 - Subflujo 8
            List<DemVegCol> demandaBarras = DemBarrasFormatoPronDema(
                fechas, datosGeneracion, demandaCompleta,
                listaFormulas, true);
            //Exportación del caso de prueba [CP006]
            ReportesDpo.DemVegReporteSimple(demandaBarras, CP003_itv,
               "CP006_Demanda",
               $"{rutaReporte}\\CP006_Demanda.xlsx",
               "Nombre");

            //CU002 - Subflujo 9
            foreach (DemVegCol barra in demandaBarras)
            {
                barra.Valores = FiltradoPorUmbral(barra.Valores, tolMinuto);
            }
            //Exportación del caso de prueba [CP007]
            ReportesDpo.DemVegReporteSimple(demandaBarras, CP003_itv,
               "CP007_Demanda",
               $"{rutaReporte}\\CP007_Demanda.xlsx",
               "Nombre");

            //CU002 - Subflujo 10
            List<DemVegCol> averageSimpleBarras = CalcPromedioSimplePorMinuto(
                demandaBarras);
            //Exportación del caso de prueba [CP008]
            ReportesDpo.DemVegReporteSimple(averageSimpleBarras, CP004_itv,
                "CP008_Promedio",
                $"{rutaReporte}\\CP008_Promedio.xlsx",
                "Nombre");

            //CU002 - Subflujo 11
            List<DemVegCol> demBarrasCompleta = CompletadoDatosDemanda(
                demandaBarras, averageSimpleBarras, "Nombre");
            //Exportación del caso de prueba [CP009]
            ReportesDpo.DemVegReporteSimple(demBarrasCompleta, CP003_itv,
               "CP009_Demanda",
               $"{rutaReporte}\\CP009_Demanda.xlsx",
               "Nombre");

            //CU002 - Subflujo 12
            List<DemVegCol> demPromDepuradoxMin = CalcPromDepuradoPorMinuto(
                fechas, demBarrasCompleta);
            //Exportación del caso de prueba [CP010]
            ReportesDpo.DemVegReporteSimple(demPromDepuradoxMin, CP004_itv,
               "CP010_Promedio",
               $"{rutaReporte}\\CP010_Promedio.xlsx",
               "Nombre");

            //CU002 - Subflujo 13            
            foreach (DemVegCol barra in demPromDepuradoxMin)
            {
                AlisadoGaussiano(barra.Valores, vgm, std);
            }
            //Exportación del caso de prueba [CP007]
            ReportesDpo.DemVegReporteSimple(demPromDepuradoxMin, CP004_itv,
               "CP011_Promedio",
               $"{rutaReporte}\\CP011_Promedio.xlsx",
               "Nombre");

            //---------------------------------------------------------------------------
            //• Procesamiento de datos Hoy
            //---------------------------------------------------------------------------
            List<DateTime> fechaHoy = new List<DateTime>() { dia };

            //CU01 - Proceso 2 (CU02 - Subflujo 3)
            List<DemVegCol> datosGeneracionHoy = GeneracionFechasPorMinuto(fechaHoy);
            //Exportación del caso de prueba [CP012]
            List<string> CP001_itv_hoy = ObtenerColumnaIntervalos(fechaHoy, 1, false);
            ReportesDpo.DemVegReporteSimple(datosGeneracionHoy, CP001_itv_hoy,
               "CP012_Generacion",
               $"{rutaReporte}\\CP012_Generacion.xlsx",
               "Id");

            //CU01 - Proceso 2 (CU02 - Subflujo 4)
            List<DemVegCol> datosDemandaHoy = DemandaFechasPorMinuto(
                fechaHoy, listaFormulas, true);
            //Exportación del caso de prueba [CP013]
            List<string> CP002_itv_hoy = ObtenerColumnaIntervalos(fechaHoy, 1, false);
            ReportesDpo.DemVegReporteSimple(datosDemandaHoy, CP002_itv_hoy,
               "CP013_Demanda",
               $"{rutaReporte}\\CP013_Demanda.xlsx",
               "Id");

            //CU01 - Proceso 3            
            int corte = ObtenerIntervaloDesdeHora(hora);
            List<int> parte1 = Enumerable.Range(0, corte).ToList();
            List<int> parte2 = Enumerable.Range(corte, 1440 - corte).ToList();

            //CU01 - Proceso 4
            foreach (DemVegCol d in datosDemandaHoy)
            {
                foreach (int i in parte2)
                {
                    d.Valores[i] = null;
                }
            }

            //CU01 - Proceso 5 (CU002 - Subflujo 8)
            List<DemVegCol> demandaBarrasHoy = DemBarrasFormatoPronDema(
                fechaHoy, datosGeneracionHoy, datosDemandaHoy,
                listaFormulas, false);
            //Se reemplazan los valores "0" por "null"
            foreach (DemVegCol d in demandaBarrasHoy)
            {
                for (int i = 0; i < d.Valores.Count; i++)
                {
                    decimal? valor = d.Valores[i];
                    if (valor == 0) d.Valores[i] = null;
                }
            }
            //Solo se mantiene la "Parte 1" de los datos
            foreach (DemVegCol d in demandaBarrasHoy)
            {
                foreach (int i in parte2)
                {
                    d.Valores[i] = null;
                }
            }

            List<string> CP003_itv_hoy = ObtenerColumnaIntervalos(fechaHoy, 1, false);
            //Exportación del caso de prueba [CP014]
            ReportesDpo.DemVegReporteSimple(demandaBarrasHoy, CP003_itv_hoy,
               "CP014_Demanda",
               $"{rutaReporte}\\CP014_Demanda.xlsx",
               "Nombre");

            //CU01 - Proceso 6 (CU04 - Filtrado por Umbral)
            foreach (DemVegCol barra in demandaBarrasHoy)
            {
                barra.Valores = FiltradoPorUmbral(barra.Valores, tolMinuto);
            }
            
            //Exportación del caso de prueba [CP015]
            ReportesDpo.DemVegReporteSimple(demandaBarrasHoy, CP003_itv_hoy,
               "CP015_Demanda",
               $"{rutaReporte}\\CP015_Demanda.xlsx",
               "Nombre");

            //CU01 - Proceso 7 (CU06 - Completado datos demanda)
            List<DemVegCol> demBarrasCompletaHoy = CompletadoDatosDemanda(
                demandaBarrasHoy, demPromDepuradoxMin, "Nombre");
            //Exportación del caso de prueba [CP016]
            ReportesDpo.DemVegReporteSimple(demBarrasCompletaHoy, CP003_itv_hoy,
               "CP016_Demanda",
               $"{rutaReporte}\\CP016_Demanda.xlsx",
               "Nombre");

            //CU01 - Proceso 8 (CU08 - Alisado Gaussiano)
            foreach (DemVegCol barra in demBarrasCompletaHoy)
            {
                AlisadoGaussiano(barra.Valores, vgm, std);
            }

            foreach (DemVegCol d in demBarrasCompletaHoy)
            {
                foreach (int i in parte2)
                {
                    d.Valores[i] = null;
                }
            }
            //Exportación del caso de prueba [CP017]
            ReportesDpo.DemVegReporteSimple(demBarrasCompletaHoy, CP003_itv_hoy,
               "CP017_Demanda",
               $"{rutaReporte}\\CP017_Demanda.xlsx",
               "Nombre");

            //CU01 - Proceso 9
            foreach (DemVegCol d in demBarrasCompletaHoy)
            {
                foreach (int i in parte2)
                {
                    d.Valores[i] = null;
                }
            }
            //Exportación del caso de prueba [CP018]
            ReportesDpo.DemVegReporteSimple(demPromDepuradoxMin, CP004_itv,
                "CP018_Promedio",
                $"{rutaReporte}\\CP018_Promedio.xlsx",
                "Nombre");

            //CU01 - Proceso 11
            List<DemVegCol> diferencia = new List<DemVegCol>();
            foreach (DemVegCol d in demBarrasCompletaHoy)
            {
                diferencia.Add(new DemVegCol()
                {
                    Id = d.Id,
                    SubId = d.SubId,
                    Nombre = d.Nombre,
                    Valores = new decimal?[d.Valores.Count].ToList(),
                });
            }

            //CU01 - Proceso 12
            foreach (DemVegCol eDemanda in demBarrasCompletaHoy)
            {
                DemVegCol ePromedio = demPromDepuradoxMin
                    .FirstOrDefault(x => x.Nombre == eDemanda.Nombre) ?? null;
                DemVegCol eDiferencia = diferencia
                    .FirstOrDefault(x => x.Nombre == eDemanda.Nombre) ?? null;

                if (ePromedio == null || eDiferencia == null) continue;

                foreach (int i in parte1)
                {
                    decimal? valor = ePromedio.Valores[i] - eDemanda.Valores[i];
                    eDiferencia.Valores[i] = valor;
                }              
            }
            //Exportación del caso de prueba [CP019]
            ReportesDpo.DemVegReporteSimple(diferencia, CP003_itv_hoy,
               "CP019_Diferencia",
               $"{rutaReporte}\\CP019_Diferencia.xlsx",
               "Nombre");

            //CU01 - Proceso 13            
            List<DemVegCol> diferenciaPromedio = new List<DemVegCol>();
            foreach (DemVegCol d in diferencia)
            {
                //Valida los rangos a tomar
                int i = (corte < rango) ? 0 : (corte - rango);
                int n = (corte < rango) ? corte : rango;

                decimal? prom = d.Valores.GetRange(i, n).Average();

                diferenciaPromedio.Add(new DemVegCol()
                {
                    Id = d.Id,
                    SubId = d.SubId,
                    Nombre = d.Nombre,
                    PromRango = prom,
                    Valores = new List<decimal?> { prom },
                });
            }
            //Exportación del caso de prueba [CP020]
            ReportesDpo.DemVegReporteSimple(diferenciaPromedio, new List<string>() { "1" },
               "CP020_Diferencia_Promedio",
               $"{rutaReporte}\\CP020_Diferencia_Promedio.xlsx",
               "Nombre");

            //CU01 - Proceso 14
            foreach (DemVegCol d in diferencia)
            {
                DemVegCol dPromedio = diferenciaPromedio
                    .FirstOrDefault(x => x.Nombre == d.Nombre) ?? null;

                if (dPromedio == null) continue;

                decimal? ultimoValor = dPromedio.PromRango * (1 - porcentaje);
                d.Valores[d.Valores.Count - 1] = ultimoValor;
            }
            //Exportación del caso de prueba [CP021]
            ReportesDpo.DemVegReporteSimple(diferencia, CP003_itv_hoy,
               "CP021_Diferencia",
               $"{rutaReporte}\\CP021_Diferencia.xlsx",
               "Nombre");

            //CU01 - Proceso 15
            foreach (DemVegCol d in diferencia)
            {
                Interpolacion(d.Valores);
            }
            //Exportación del caso de prueba [CP022]
            ReportesDpo.DemVegReporteSimple(diferencia, CP003_itv_hoy,
               "CP022_Diferencia",
               $"{rutaReporte}\\CP022_Diferencia.xlsx",
               "Nombre");

            //CU01 - Proceso 16
            List<DemVegCol> scoVegetativa = new List<DemVegCol>();
            foreach (DemVegCol eDiferencia in diferencia)
            {                
                DemVegCol ePromedio = demPromDepuradoxMin
                    .FirstOrDefault(x => x.Nombre == eDiferencia.Nombre) ?? null;

                if (ePromedio == null) continue;

                List<decimal?> valores = ePromedio.Valores
                    .Zip(eDiferencia.Valores, (a, b) => a - b)
                    .ToList();

                for (int i = 0; i < valores.Count; i++)
                {
                    decimal? valor = valores[i];
                    valores[i] = (valor == null || valor < 0) 
                        ? 0 : valor;                   
                }

                scoVegetativa.Add(new DemVegCol()
                {
                    Id = eDiferencia.Id,
                    SubId = eDiferencia.SubId,
                    Nombre = eDiferencia.Nombre,
                    Valores = valores
                });
            }
            //Exportación del caso de prueba [CP023]
            ReportesDpo.DemVegReporteSimple(scoVegetativa, CP003_itv_hoy,
               "CP023_SCO_Vegetativa",
               $"{rutaReporte}\\CP023_SCO_Vegetativa.xlsx",
               "Nombre");

            //CU01 - Proceso 17            
            List<DemVegCol> sprVegetativo = ExtraccionDemandaSPRMin(
                dia, idTipo, idVersion);
            //Exportación del caso de prueba [CP024]
            List<string> CP024_itv = ObtenerColumnaIntervalos(fechaHoy, 1, true);
            ReportesDpo.DemVegReporteSimple(sprVegetativo, CP024_itv,
               "CP024_SPR_Vegetativa",
               $"{rutaReporte}\\CP024_SPR_Vegetativa.xlsx",
               "Nombre");

            //Conversión de datos a formato de 30min
            List<DemVegCol> res = new List<DemVegCol>();
            foreach (DemVegCol e in scoVegetativa)
            {
                res.Add(new DemVegCol()
                {
                    Id =e.Id,
                    SubId = e.SubId,
                    Nombre = e.Nombre,
                    Valores = DarFormatoPorIntervalos(e.Valores, ConstantesDpo.Itv30min)
                });
            }

            return res;
        }

        //CU03 - Proceso días feriados
        public List<DemVegCol> EjecutarDemVegFeriado(
            DateTime dia, string idVersion, DpoConfiguracionDTO configuracion)
        {
            //Constantes de tipo de informacion
            string idTipo = "4";

            //Variables
            decimal tolMinuto = configuracion.Dpocngumbral;//10
            int etapas = configuracion.Dpocngpromedio;//1
            decimal porcentaje = configuracion.Dpocngtendencia;//0
            int vgm = configuracion.Dpocngvmg;//15
            double std = (double)configuracion.Dpocngstd;//1.645
            DateTime hora = DateTime.ParseExact(configuracion.Dpocngfechora,
                "HH:mm", CultureInfo.InvariantCulture);

            int rango = etapas * 60;
            List<DateTime> fechas = new List<DateTime>() { dia };
            

            //CU01 - Subflujo 3
            List<DemVegCol> promedioSPR = ExtraccionDemandaSPRMin(
                dia, idTipo, idVersion);
            //Exportación del caso de prueba [CP011]
            List<string> CP011_itv = Enumerable.Range(0, 1440)
                .Select(x => x.ToString())
                .ToList();            
            ReportesDpo.DemVegReporteSimple(promedioSPR, CP011_itv,
               "CP011_Promedio",
               $"{rutaReporte}\\CP011_Promedio.xlsx",
               "Nombre");

            //---------------------------------------------------------------------------
            //• Procesamiento de datos Hoy
            //---------------------------------------------------------------------------
            List<DateTime> fechaHoy = new List<DateTime>() { dia };

            //CU01 - Proceso 2 (CU02 - Subflujo 3)
            List<DemVegCol> datosGeneracionHoy = GeneracionFechasPorMinuto(fechaHoy);
            //Exportación del caso de prueba [CP012]
            List<string> CP001_itv_hoy = ObtenerColumnaIntervalos(fechaHoy, 1, false);
            ReportesDpo.DemVegReporteSimple(datosGeneracionHoy, CP001_itv_hoy,
               "CP012_Generacion",
               $"{rutaReporte}\\CP012_Generacion.xlsx",
               "Id");

            //CU01 - Proceso 2 (CU02 - Subflujo 4)
            List<DpoFormulaDTO> listaFormulas = FactorySic
                .GetDpoProcesoPronosticoRepository()
                .ObtenerFormulas();
            List<DemVegCol> datosDemandaHoy = DemandaFechasPorMinuto(
                fechaHoy, listaFormulas, true);
            //Exportación del caso de prueba [CP013]
            List<string> CP002_itv_hoy = ObtenerColumnaIntervalos(fechaHoy, 1, false);
            ReportesDpo.DemVegReporteSimple(datosDemandaHoy, CP002_itv_hoy,
               "CP013_Demanda",
               $"{rutaReporte}\\CP013_Demanda.xlsx",
               "Id");

            //CU01 - Proceso 3
            int corte = ObtenerIntervaloDesdeHora(hora);
            List<int> parte1 = Enumerable.Range(0, corte).ToList();
            List<int> parte2 = Enumerable.Range(corte, 1440 - corte).ToList();

            //CU01 - Proceso 4
            foreach (DemVegCol d in datosDemandaHoy)
            {
                foreach (int i in parte2)
                {
                    d.Valores[i] = null;
                }
            }

            //CU01 - Proceso 5 (CU002 - Subflujo 8)
            List<DemVegCol> demandaBarrasHoy = DemBarrasFormatoPronDema(
                fechaHoy, datosGeneracionHoy, datosDemandaHoy,
                listaFormulas, false);

            //Se reemplazan los valores "0" por "null"
            foreach (DemVegCol d in demandaBarrasHoy)
            {
                for (int i = 0; i < d.Valores.Count; i++)
                {
                    decimal? valor = d.Valores[i];
                    if (valor == 0) d.Valores[i] = null;
                }
            }
            //Solo se mantiene la "Parte 1" de los datos
            foreach (DemVegCol d in demandaBarrasHoy)
            {
                foreach (int i in parte2)
                {
                    d.Valores[i] = null;
                }
            }

            List<string> CP014_itv = ObtenerColumnaIntervalos(fechaHoy, 1, false);
            //Exportación del caso de prueba [CP014]
            ReportesDpo.DemVegReporteSimple(demandaBarrasHoy, CP014_itv,
               "CP014_Demanda",
               $"{rutaReporte}\\CP014_Demanda.xlsx",
               "Nombre");

            //CU01 - Proceso 6 (CU04 - Filtrado por Umbral)
            foreach (DemVegCol barra in demandaBarrasHoy)
            {
                barra.Valores = FiltradoPorUmbral(barra.Valores, tolMinuto);
            }

            //Exportación del caso de prueba [CP015]
            ReportesDpo.DemVegReporteSimple(demandaBarrasHoy, CP014_itv,
               "CP015_Demanda",
               $"{rutaReporte}\\CP015_Demanda.xlsx",
               "Nombre");

            //CU01 - Proceso 7 (CU06 - Completado datos demanda)
            List<DemVegCol> demBarrasCompletaHoy = CompletadoDatosDemanda(
                demandaBarrasHoy, promedioSPR, "Nombre");
            //Exportación del caso de prueba [CP016]
            ReportesDpo.DemVegReporteSimple(demBarrasCompletaHoy, CP014_itv,
               "CP016_Demanda",
               $"{rutaReporte}\\CP016_Demanda.xlsx",
               "Nombre");

            //CU01 - Proceso 8 (CU08 - Alisado Gaussiano)
            foreach (DemVegCol barra in demBarrasCompletaHoy)
            {
                AlisadoGaussiano(barra.Valores, vgm, std);
            }

            foreach (DemVegCol d in demBarrasCompletaHoy)
            {
                foreach (int i in parte2)
                {
                    d.Valores[i] = null;
                }
            }

            //Exportación del caso de prueba [CP017]
            ReportesDpo.DemVegReporteSimple(demBarrasCompletaHoy, CP014_itv,
               "CP017_Demanda",
               $"{rutaReporte}\\CP017_Demanda.xlsx",
               "Nombre");

            //CU01 - Proceso 9
            foreach (DemVegCol d in demBarrasCompletaHoy)
            {
                foreach (int i in parte2)
                {
                    d.Valores[i] = null;
                }
            }
            //Exportación del caso de prueba [CP018]
            List<string> CP018_itv = Enumerable.Range(0, 1440)
                .Select(x => x.ToString())
                .ToList();
            ReportesDpo.DemVegReporteSimple(promedioSPR, CP018_itv,
                "CP018_Promedio",
                $"{rutaReporte}\\CP018_Promedio.xlsx",
                "Nombre");

            //CU01 - Proceso 11
            List<DemVegCol> diferencia = new List<DemVegCol>();
            foreach (DemVegCol d in demBarrasCompletaHoy)
            {
                diferencia.Add(new DemVegCol()
                {
                    Id = d.Id,
                    SubId = d.SubId,
                    Nombre = d.Nombre,
                    Valores = new decimal?[d.Valores.Count].ToList(),
                });
            }

            //CU01 - Proceso 12
            foreach (DemVegCol eDemanda in demBarrasCompletaHoy)
            {
                DemVegCol ePromedio = promedioSPR
                    .FirstOrDefault(x => x.Nombre == eDemanda.Nombre) ?? null;
                DemVegCol eDiferencia = diferencia
                    .FirstOrDefault(x => x.Nombre == eDemanda.Nombre) ?? null;

                if (ePromedio == null || eDiferencia == null) continue;

                foreach (int i in parte1)
                {
                    decimal? valor = ePromedio.Valores[i] - eDemanda.Valores[i];
                    eDiferencia.Valores[i] = valor;
                }
            }
            //Exportación del caso de prueba [CP019]
            ReportesDpo.DemVegReporteSimple(diferencia, CP014_itv,
               "CP019_Diferencia",
               $"{rutaReporte}\\CP019_Diferencia.xlsx",
               "Nombre");

            //CU01 - Proceso 13            
            List<DemVegCol> diferenciaPromedio = new List<DemVegCol>();
            foreach (DemVegCol d in diferencia)
            {
                //Valida los rangos a tomar
                int i = (corte < rango) ? 0 : (corte - rango);
                int n = (corte < rango) ? corte : rango;

                decimal? prom = d.Valores.GetRange(i, n).Average();

                diferenciaPromedio.Add(new DemVegCol()
                {
                    Id = d.Id,
                    SubId = d.SubId,
                    Nombre = d.Nombre,
                    PromRango = prom,
                    Valores = new List<decimal?> { prom },
                });
            }
            //Exportación del caso de prueba [CP020]
            ReportesDpo.DemVegReporteSimple(diferenciaPromedio, new List<string>() { "1" },
               "CP020_Diferencia_Promedio",
               $"{rutaReporte}\\CP020_Diferencia_Promedio.xlsx",
               "Nombre");

            //CU01 - Proceso 14
            foreach (DemVegCol d in diferencia)
            {
                DemVegCol dPromedio = diferenciaPromedio
                    .FirstOrDefault(x => x.Nombre == d.Nombre) ?? null;

                if (dPromedio == null) continue;

                decimal? ultimoValor = dPromedio.PromRango * (1 - porcentaje);
                d.Valores[d.Valores.Count - 1] = ultimoValor;
            }
            //Exportación del caso de prueba [CP021]
            ReportesDpo.DemVegReporteSimple(diferencia, CP014_itv,
               "CP021_Diferencia",
               $"{rutaReporte}\\CP021_Diferencia.xlsx",
               "Nombre");

            //CU01 - Proceso 15
            foreach (DemVegCol d in diferencia)
            {
                Interpolacion(d.Valores);
            }
            //Exportación del caso de prueba [CP022]
            ReportesDpo.DemVegReporteSimple(diferencia, CP014_itv,
               "CP022_Diferencia",
               $"{rutaReporte}\\CP022_Diferencia.xlsx",
               "Nombre");

            //CU01 - Proceso 16
            List<DemVegCol> scoVegetativa = new List<DemVegCol>();
            foreach (DemVegCol eDiferencia in diferencia)
            {
                DemVegCol ePromedio = promedioSPR
                    .FirstOrDefault(x => x.Nombre == eDiferencia.Nombre) ?? null;

                if (ePromedio == null) continue;

                List<decimal?> valores = ePromedio.Valores
                    .Zip(eDiferencia.Valores, (a, b) => a - b)
                    .ToList();

                for (int i = 0; i < valores.Count; i++)
                {
                    decimal? valor = valores[i];
                    valores[i] = (valor == null || valor < 0)
                        ? 0 : valor;
                }

                scoVegetativa.Add(new DemVegCol()
                {
                    Id = eDiferencia.Id,
                    SubId = eDiferencia.SubId,
                    Nombre = eDiferencia.Nombre,
                    Valores = valores
                });
            }
            //Exportación del caso de prueba [CP023]
            ReportesDpo.DemVegReporteSimple(scoVegetativa, CP014_itv,
               "CP023_SCO_Vegetativa",
               $"{rutaReporte}\\CP023_SCO_Vegetativa.xlsx",
               "Nombre");

            //CU01 - Proceso 17
            List<DemVegCol> sprVegetativo = ExtraccionDemandaSPRMin(
                dia, idTipo, idVersion);
            //Exportación del caso de prueba [CP024]
            List<string> CP024_itv = ObtenerColumnaIntervalos(fechaHoy, 1, true);
            ReportesDpo.DemVegReporteSimple(sprVegetativo, CP024_itv,
               "CP024_SPR_Vegetativa",
               $"{rutaReporte}\\CP024_SPR_Vegetativa.xlsx",
               "Nombre");

            //Conversión de datos a formato de 30min
            List<DemVegCol> res = new List<DemVegCol>();
            foreach (DemVegCol e in scoVegetativa)
            {
                res.Add(new DemVegCol()
                {
                    Id = e.Id,
                    SubId = e.SubId,
                    Nombre = e.Nombre,
                    Valores = DarFormatoPorIntervalos(e.Valores, ConstantesDpo.Itv30min)
                });
            }

            return res;
        }

        //CU02 Subflujo 2 - Obtención de Fechas a analizar
        /// <summary>
        /// [CU02]Subflujo_2 - Obtencion de Fechas a analizar para un dia tipico
        /// </summary> 
        /// <param name="fechaBase">Fecha de partida</param>
        /// <param name="numeroDias">Cantidad de dias a obtener (Nd, Horizonte)</param>
        /// <returns></returns>
        public List<DateTime> FechasDiasTipicos(
            DateTime fechaBase, int numeroDias)
        {
            //Reglas de negocio
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

            //Obtención de lista de días feriados para su exclusión
            int anio = fechaBase.Year;
            int anioPrev = anio - 1;

            List<DpoFeriadosDTO> datosFeriados = FactorySic
                .GetDpoFeriadosRepository().GetByAnioRango(anioPrev, anio);

            List<DateTime> listaFeriados = datosFeriados
                .Select(x => x.Dpoferfecha)
                .ToList();

            //Obtención de fechas para el análisis
            int diaSemana = (int)fechaBase.DayOfWeek;
            List<DateTime> res = new List<DateTime>();

            if (reglaA.Contains(diaSemana))
            {
                DateTime d = fechaBase;

                while (res.Count < numeroDias)
                {
                    d = d.AddDays(-7);
                    if (listaFeriados.Contains(d)) continue;

                    res.Add(d);
                }
            }
            if (reglaB.Contains(diaSemana))
            {
                DateTime d = fechaBase;

                while (res.Count < numeroDias)
                {
                    d = d.AddDays(-1);
                    if (listaFeriados.Contains(d)) continue;
                    if (!reglaB.Contains((int)d.DayOfWeek)) continue;

                    res.Add(d);
                }
            }

            res = res.OrderBy(x => x).ToList();
            return res;
        }

        /// <summary>
        /// [CU02]Subflujo_3 - Extraccion de Generacion de las fechas por minuto
        /// </summary>
        /// <param name="listaFechas">Lista de dias a procesar</param>
        /// <returns></returns>
        public List<DemVegCol> GeneracionFechasPorMinuto(
            List<DateTime> listaFechas)
        {
            if (listaFechas.Count == 0)
                return new List<DemVegCol>();

            string str = CondicionConsultaGeneracion(listaFechas);
            //[Tabla]
            List<MeMedicion48DTO> datosGeneracion = FactorySic
                .GetDpoProcesoPronosticoRepository()
                .ObtenerGeneracionPorFechas(str);

            //[Tabla1]
            List<int> ids = datosGeneracion
                .Select(x => x.Ptomedicodi)
                .Distinct().ToList();
            List<DemVegCol> tabla1 = new List<DemVegCol>();
            foreach (int id in ids)
            {                
                List<MeMedicion48DTO> datosPorPunto = datosGeneracion
                    .Where(x => x.Ptomedicodi == id)
                    .OrderBy(x => x.Medifecha)
                    .ToList();
                tabla1.AddRange(DarFormatoArreglo(id, datosPorPunto,
                    listaFechas, 48));                
            }

            //[Tabla2]
            List<DemVegCol> tabla2 = new List<DemVegCol>();
            foreach (DemVegCol col in tabla1)
            {                             
                DemVegCol ent = new DemVegCol()
                {
                    Id = col.Id,
                    FechaHora = col.FechaHora,
                    Valores = DarFormatoPorMinuto(col.Valores, 1, 48)
                };
                tabla2.Add(ent);
            }

            //[Tabla2] Interpolacion
            foreach (DemVegCol col in tabla2)
            {               
                Interpolacion(col.Valores);
            }

            //[Matrix] Se ordena en columnas agrupadas
            List<DemVegCol> matrix = new List<DemVegCol>();
            foreach (int id in ids)
            {
                List<DemVegCol> datosPorPunto = tabla2
                    .Where(x => x.Id == id)
                    .OrderBy(x => x.FechaHora)
                    .ToList();

                DemVegCol e = new DemVegCol()
                {
                    Id = id,
                    Valores = DarFormatoAgrupado(datosPorPunto, listaFechas, 1440)
                };
                matrix.Add(e);
            }

            return matrix;
        }

        /// <summary>
        /// [CU02]Subflujo_4 - Extraccion de la Demanda de las fechas por minuto
        /// </summary>
        /// <param name="listaFechas">Lista de dias a procesar</param>
        /// <param name="listaFormulas">Lista de formulas</param>
        /// <param name="ultimaHora">Flag que indica si se debe buscar los datos
        /// de la ultima hora procesada (tabla temporal)</param>
        /// <returns></returns>
        public List<DemVegCol> DemandaFechasPorMinuto(
            List<DateTime> listaFechas, List<DpoFormulaDTO> listaFormulas,
            bool ultimaHora)
        {    
            //Información codificada tipo TNA(Ieod)
            List<int> listaCodIeod = ObtenerListaCodigos(listaFormulas);

            List<DpoRelacionScoIeod> listaRelScoIeod = FactorySic
                .GetDpoProcesoPronosticoRepository()
                .RelacionScoIeod();

            listaRelScoIeod = listaRelScoIeod
                .Where(x => listaCodIeod.Contains(x.Ptomedicodi_Ieod))
                .ToList();

            //Se obtiene la lista de codigos correspondientes en SCO
            List<int> listaCodSco = listaRelScoIeod
                .Select(x => x.Ptomedicodi_Sco)
                .ToList();
            string strIds = string.Join(",", listaCodSco);

            //[Tabla]
            List<DemVegCol> tabla = new List<DemVegCol>();
            List<DpoEstimadorRawDTO> datosDemanda = new List<DpoEstimadorRawDTO>();
            foreach (DateTime dia in listaFechas)
            {
                string nomTabla = ConstantesDpo.tablaEstimadorRaw
                    + dia.ToString(ConstantesDpo.FormatoAnioMes);
                string strIni = dia.ToString("yyyy-MM-dd HH:mm:ss");
                string strFin = dia.AddHours(23).ToString("yyyy-MM-dd HH:mm:ss");
                List<DpoEstimadorRawDTO> datosDia = FactorySic
                    .GetDpoProcesoPronosticoRepository()
                    .ObtenerDemandaPorFechas(nomTabla, strIni, strFin, strIds);

                //Obtiene los datos de la ultima hora desde la tabla temporal
                if (ultimaHora)
                {
                    DateTime hoy = DateTime.Now;
                    DateTime diaSiguiente = hoy.AddDays(1);
                    DateTime diaHora = new DateTime(hoy.Year,
                        hoy.Month, hoy.Day, hoy.Hour, 0, 0);
                    DateTime diaHoraSig = new DateTime(diaSiguiente.Year,
                        diaSiguiente.Month, diaSiguiente.Day, 0, 0, 0);

                    string strDiaHora = diaHora.ToString(ConstantesDpo.FormatoFechaMedicionRaw);
                    string strHora24 = diaHora.ToString("HH");
                    string strDia = diaHora.ToString(ConstantesDpo.FormatoFecha);
                    string strDiaHoraSig = diaHoraSig.ToString(ConstantesDpo.FormatoFechaMedicionRaw);

                    if (dia.ToString(ConstantesDpo.FormatoFecha) == strDia)
                    {
                        datosDia.AddRange(FactorySic.GetDpoProcesoPronosticoRepository()
                        .ObtenerDemandaUltimaHora(strDiaHora, strHora24,
                        strDia, strDiaHoraSig, strIds));
                    }
                }

                tabla.AddRange(DarFormatoPorMinuto(datosDia, listaFechas));
            }
            //[Tabla1]
            //Se esctructura los datos por punto en una sola columna
            List<DemVegCol> tabla1 = new List<DemVegCol>();
            foreach (DpoRelacionScoIeod cod in listaRelScoIeod)
            {
                DemVegCol entidad = new DemVegCol()
                {
                    Id = cod.Ptomedicodi_Ieod,
                    SubId = cod.Ptomedicodi_Sco,
                    FechaHora = listaFechas.First(),
                    Valores = new List<decimal?>(),
                };

                List<DemVegCol> datosPunto = tabla
                    .Where(x => x.Id == cod.Ptomedicodi_Sco)
                    .ToList();

                if (datosPunto.Count == 0) continue;

                datosPunto = datosPunto
                    .OrderBy(x => x.FechaHora)
                    .ToList();

                foreach (DemVegCol datoPunto in datosPunto)
                {
                    entidad.Valores
                        .AddRange(datoPunto.Valores
                        .ToList());
                }
                tabla1.Add(entidad);
            }

            //[Matrix]
            return tabla1;
        }

        /// <summary>
        /// [CU04] Filtrado por umbral
        /// </summary>
        /// <param name="datos">Columna a evaluar</param>
        /// <param name="tolMinuto">Toleracia(Umbral)</param>
        public List<decimal?> FiltradoPorUmbral(
            List<decimal?> datos, decimal tolMinuto)
        {
            List<int> indices = new List<int>();
            decimal?[] res = new decimal?[datos.Count];

            for (int i = 1; i < datos.Count; i++)
            {
                var v1 = datos[i];
                var v2 = datos[i - 1];
                if (v1 == null || v2 == null) continue;

                decimal diff = (decimal)datos[i] - (decimal)datos[i - 1];
                diff = Math.Abs(diff);
                if (diff >= tolMinuto) 
                    indices.Add(i);
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
        /// [CU05] Calculo del promedio simple por minuto
        /// </summary>
        /// <param name="datosBase">Datos base a procesar</param>
        /// <returns></returns>
        public List<DemVegCol> CalcPromedioSimplePorMinuto(
            List<DemVegCol> datosBase)
        {
            if (datosBase.Count == 0) return new List<DemVegCol>();

            int n = 1440;
            int dias = datosBase.First().Valores.Count / n;
            if (dias == 1) return datosBase;

            List<DemVegCol> res = new List<DemVegCol>();
            foreach (DemVegCol d in datosBase)
            {                
                List<decimal?> valores = new List<decimal?>();
                List<List<decimal?>> tabla = new List<List<decimal?>>();

                for (int i = 0; i < dias; i++)
                {
                    List<decimal?> bloque = d.Valores.GetRange(i * n, n);
                    Interpolacion(bloque);
                    tabla.Add(bloque);
                }

                //Calcula la media
                for (int i = 0; i < n; i++)
                {                    
                    List<decimal?> fila = new List<decimal?>();
                    for (int j = 0; j < dias; j++)
                    {
                        decimal? valor = tabla[j][i];
                        if (valor != null) fila.Add(valor);
                    }

                    if (fila.Count == 0)
                    {
                        valores.Add(null);
                        continue;
                    }

                    decimal? media = fila.Sum() / fila.Count;
                    valores.Add(media);
                }

                res.Add(new DemVegCol()
                {
                    Id = d.Id,
                    SubId = d.SubId,
                    Nombre = d.Nombre,
                    FechaHora = d.FechaHora,
                    Valores = valores,
                });
            }

            return res;
        }

        /// <summary>
        /// [CU06] Completado de datos de la demanda
        /// </summary>
        /// <param name="datosDemanda">Datos demanda para cada punto</param>
        /// <param name="datosPromedio">Datos promedio para cada punto</param>
        /// <param name="criterio">Criterio para la identificacion de los datos (Id o Nombre)</param>
        /// <returns></returns>
        public List<DemVegCol> CompletadoDatosDemanda(
            List<DemVegCol> datosDemanda, List<DemVegCol> datosPromedio,
            string criterio)
        {            
            if (datosDemanda.Count == 0) return new List<DemVegCol>();

            int n = 1440;
            int dias = datosDemanda.First().Valores.Count / n;
            
            List<DemVegCol> res = new List<DemVegCol>();
            foreach (DemVegCol d in datosDemanda)
            {                
                List<decimal?> valores = new List<decimal?>();
                List<decimal?> average = new List<decimal?>();

                if (criterio == "Id")
                    average = datosPromedio
                        .FirstOrDefault(x => x.Id == d.Id).Valores;
                if (criterio == "Nombre")
                    average = datosPromedio
                        .FirstOrDefault(x => x.Nombre == d.Nombre).Valores;

                for (int i = 0; i < dias; i++)
                {
                    List<decimal?> bloque = d.Valores.GetRange(i * n, n);

                    //Si el bloque de informacion esta vacio
                    //se reemplaza con la columna average
                    int valid = bloque.Where(x => x != null).Count();
                    if (valid == 0)
                    {
                        valores.AddRange(average);
                        continue;
                    }

                    //Si contiene valores se realiza el procedimiento
                    CompletadoDatos(bloque, average);
                    valores.AddRange(bloque);
                }

                res.Add(new DemVegCol()
                {
                    Id = d.Id,
                    SubId = d.SubId,
                    Nombre = d.Nombre,
                    FechaHora = d.FechaHora,
                    Valores = valores,
                });
            }

            return res;
        }

        /// <summary>
        /// [CU02 SUbflujo 8] Demanda por barras y por minuto historica
        /// formato PronDema
        /// </summary>
        /// <param name="listaFechas">Lista de fechas a analisar</param>
        /// <param name="datosGeneracion">Datos de la generacion por punto</param>
        /// <param name="datosDemanda">Datos de la demanda por punto</param>
        /// <param name="listaFormulas">Lista de formulas para la agrupacion</param>
        /// <param name="valoresNan">Flag true: no considera NAN, false: si considera</param>
        /// <returns></returns>
        public List<DemVegCol> DemBarrasFormatoPronDema(
            List<DateTime> listaFechas, List<DemVegCol> datosGeneracion,
            List<DemVegCol> datosDemanda, List<DpoFormulaDTO> listaFormulas,
            bool valoresNan)
        {
            int numIntervalos = listaFechas.Count * 1440;
            List<string> nombresP = listaFormulas
                .Select(x => x.Nombre_P)
                .Distinct().ToList();

            List<DemVegCol> datosFormulas = new List<DemVegCol>();
            datosFormulas.AddRange(datosGeneracion);
            datosFormulas .AddRange(datosDemanda);


            List<DpoFormulaDTO> unicos = new List<DpoFormulaDTO>();
            List<DpoFormulaDTO> grupos = new List<DpoFormulaDTO>();
            foreach (string nombreP in nombresP)
            {
                List<DpoFormulaDTO> e = listaFormulas
                    .Where(x => x.Nombre_P == nombreP)
                    .ToList();
                if (e.Count == 0) continue;
                if (e.Count == 1) unicos.AddRange(e);
                else grupos.AddRange(e);
            }

            List<DemVegCol> res = new List<DemVegCol>();

            //Obtiene los datos unicos
            foreach (DpoFormulaDTO e in unicos)
            {                                
                List<decimal?> valores = ObtenerValoresPorFormula(
                    e.Formula_P, datosFormulas,
                    numIntervalos, valoresNan);

                res.Add(new DemVegCol()
                {
                    Nombre = e.Ptomedibarranomb,
                    Valores = valores
                });
            }

            //Obtiene los datos agrupados
            List<string> nomGrupos = grupos
                .Select(x => x.Nombre_P)
                .Distinct()
                .ToList();
            foreach (string nom in nomGrupos)
            {                
                List<DemVegCol> resGrupos = new List<DemVegCol>();

                List<DpoFormulaDTO> frmGrupo = grupos
                    .Where(x => x.Nombre_P == nom)
                    .ToList();

                if (frmGrupo.Count == 0) continue;

                //Obtiene el valor de Formula_P del Grupo
                DpoFormulaDTO eFrmP = frmGrupo.First();
                List<decimal?> valoresFrmP = ObtenerValoresPorFormula(
                    eFrmP.Formula_P, datosFormulas,
                    numIntervalos, valoresNan);

                //Obtiene los valores de Formula_S de cada miembro del grupo                
                List<decimal?> valoresGrupo = new decimal?[numIntervalos].ToList();
                for (int i = 0; i < frmGrupo.Count; i++)
                {
                    DpoFormulaDTO e = frmGrupo[i];

                    List<decimal?> valores = ObtenerValoresPorFormula(
                        e.Formula_S, datosFormulas,
                        numIntervalos, valoresNan);

                    if (i == 0) 
                        valoresGrupo = valores;
                    else
                        valoresGrupo = SumarListas(valoresGrupo, valores, valoresNan);

                    resGrupos.Add(new DemVegCol()
                    {
                        Nombre = e.Ptomedibarranomb,
                        Valores = valores
                    });
                }

                //Obtiene el factor por grupo
                List<decimal?> factor = new List<decimal?>();
                for (int i = 0; i < valoresFrmP.Count; i++)
                {
                    decimal? valid = valoresGrupo[i];
                    if (valid == 0 || valid == null)
                        factor.Add(null);
                    else
                        factor.Add(valoresFrmP[i] / valoresGrupo[i]);
                }

                //Si algun valor del factor es nulo o 0 se reemplaza por 1
                for (int i = 0; i < factor.Count; i++)
                {
                    decimal? valor = factor[i];
                    if (valor == 0 || valor == null) factor[i] = 1;
                }

                //Aplica el factor a cada integrante del grupo
                foreach (DemVegCol e in resGrupos)
                {
                    e.Valores = e.Valores
                        .Zip(factor, (a, b) => a * b)
                        .ToList();
                }

                res.AddRange(resGrupos);
            }

            //Obtiene el total para cada barra
            List<DemVegCol> matrix = new List<DemVegCol>();
            List<MePtomedicionDTO> listaBarras = FactorySic
                .GetDpoProcesoPronosticoRepository()
                .ListaBarras();

            foreach (MePtomedicionDTO barra in listaBarras)
            {
                string nom = barra.Ptomedibarranomb;
                List<DemVegCol> rBarra = res
                    .Where(x => x.Nombre == nom)
                    .ToList();
                if (rBarra.Count == 1)
                {
                    DemVegCol e = rBarra.First();
                    e.Id = (int)barra.Grupocodi;
                    matrix.Add(e);
                }
                else if (rBarra.Count > 1)
                {
                    List<decimal?> valores = rBarra[0].Valores;
                    for (int i = 1; i < rBarra.Count; i++)
                    {
                        valores = SumarListas(valores, rBarra[i].Valores, valoresNan);
                    }

                    matrix.Add(new DemVegCol()
                    {
                        Id = (int)barra.Grupocodi,
                        Nombre = nom,
                        Valores = valores
                    });
                }
                else
                {
                    matrix.Add(new DemVegCol()
                    {
                        Id = (int)barra.Grupocodi,
                        Nombre = nom,
                        Valores = new decimal?[numIntervalos].ToList()
                    });
                }
            }

            return matrix;
        }

        /// <summary>
        /// [CU07] Calculo de la demanda promedio de Depurado x min de la Demanda x barras
        /// </summary>
        /// <param name="listaFechas">Lista de fechas</param>
        /// <param name="datos">Datos base para el proceso</param>
        /// <returns></returns>
        public static List<DemVegCol> CalcPromDepuradoPorMinuto(
            List<DateTime> listaFechas, List<DemVegCol> datos)
        {
            decimal? tolDisEuc = 1.1M;
            decimal? tolCorrPearson = 0.2M;
            int n = 1440;
            int dias = listaFechas.Count;


            List<DemVegCol> res = new List<DemVegCol>();
            foreach (DemVegCol dato in datos)
            {                
                List<List<decimal?>> temp1 = new List<List<decimal?>>();
                for (int i = 0; i < dias; i++)
                {
                    List<decimal?> bloque = dato.Valores.GetRange(i * n, n);
                    Interpolacion(bloque);
                    temp1.Add(bloque);
                }

                List<decimal?> sumTemp1 = temp1.Select(x => x.Sum()).ToList();
                if (sumTemp1.Sum() == 0)
                {
                    res.Add(new DemVegCol()
                    {
                        Id = dato.Id,
                        SubId = dato.SubId,
                        Nombre = dato.Nombre,
                        Valores = new decimal?[n].ToList(),
                    });
                    continue;
                }

                //Se obtiene la tabla distancia euclidiana
                List<List<decimal?>> distancia = new List<List<decimal?>>();
                foreach (List<decimal?> colFija in temp1)
                {
                    List<decimal?> filaDistancia = new List<decimal?>();
                    foreach (List<decimal?> colVariable in temp1)
                    {
                        List<decimal?> col = new List<decimal?>();
                        for (int i = 0; i < colFija.Count; i++)
                        {
                            decimal? valor = colFija[i] - colVariable[i];
                            if (valor == null) valor = 0;

                            col.Add((decimal?)Math.Pow((double)valor, 2));
                        }

                        decimal? valorFila = (decimal?)Math.Sqrt((double)col.Sum());
                        filaDistancia.Add(valorFila);
                    }
                    distancia.Add(filaDistancia);
                }

                //Se obtiene la media de los promedios de la tb distancia
                List<decimal?> average_distancia = new List<decimal?>();
                foreach (List<decimal?> row in distancia)
                {
                    List<decimal?> average = row
                        .Where(x => x != null && x != 0)
                        .ToList();

                    decimal? averageValor = average.Sum() / average.Count;
                    average_distancia.Add(averageValor);
                }

                decimal? mediaP = average_distancia.Sum() / average_distancia.Count;

                //Identifica que valores esta debajo de [1.1 * mediaP]
                //Se genera el array(vector) boolean_distancia
                decimal? valorComparacion = tolDisEuc * mediaP;
                List<bool> boolean_distancia = new List<bool>();
                foreach (decimal? row in average_distancia)
                {
                    if (row < valorComparacion)
                        boolean_distancia.Add(true);
                    else
                        boolean_distancia.Add(false);
                }

                //Calcula la correlación de Pearson (Cpe) para Temp1
                List<List<decimal?>> correlacion = new List<List<decimal?>>();
                foreach (List<decimal?> colFija in temp1)
                {
                    List<decimal?> filaCorrelacion= new List<decimal?>();
                    foreach (List<decimal?> colVariable in temp1)
                    {
                        decimal? valorFila = CorrelacionPearson(colFija, colVariable);
                        filaCorrelacion.Add(valorFila);
                    }
                    correlacion.Add(filaCorrelacion);
                }

                //Se crea la tabla de valores bool
                List<List<bool>> temp3 = new List<List<bool>>();
                foreach (List<decimal?> row in correlacion)
                {
                    List<bool> filaBool = new List<bool>();
                    foreach (decimal r in row)
                    {
                        if (r > 0 && r > tolCorrPearson)
                            filaBool.Add(true);
                        else
                            filaBool.Add(false);
                    }
                    temp3.Add(filaBool);
                }
                
                List<int> colsValidas = new List<int>();
                List<int> colsRemover = new List<int>();
                for (int i = 0; i < temp3[0].Count; i++)
                {
                    List<bool> columna = new List<bool>();
                    for (int j = 0; j < temp3.Count; j++)
                    {
                        columna.Add(temp3[j][i]);
                    }
                    columna.RemoveAt(i);

                    int t = columna.Where(x => x == true).Count();
                    if (t == columna.Count)
                        colsRemover.Add(i);
                    else
                        colsValidas.Add(i);
                }
                //Asigna las filas validas (Eliminacion de filas no validas)
                //Elimina las columnas no validas
                List<List<decimal?>> matrizProcesada = new List<List<decimal?>>();
                
                foreach (int i in colsValidas)
                {
                    matrizProcesada.Add(correlacion[i]);
                }
                for (int i = 0; i < matrizProcesada.Count; i++)
                {
                    foreach (int iCol in colsRemover)
                    {
                        matrizProcesada[i][iCol] = null;
                    }
                    matrizProcesada[i] = matrizProcesada[i]
                        .Where(x => x != null)
                        .ToList();
                }

                //Se obtiene el promedio de correlacion
                //teniendo en cuenta los indices de filasValidas
                List<decimal?> average_correlacion = new List<decimal?>();
                foreach (List<decimal?> row in matrizProcesada)
                {
                    List<decimal?> average = row
                        .Where(x => x != null && x != 0)
                        .ToList();
                    if (average.Count == 0) 
                        average_correlacion.Add(0);
                    else
                    {
                        decimal? averageValor = average.Sum() / average.Count;
                        average_correlacion.Add(averageValor);
                    }                    
                }

                //Identifica que valores estan debajo de [0.2]
                //Se genera el array(vector) boolean_correlacion
                List<bool?> boolean_correlacion = new List<bool?>();
                foreach (decimal? row in average_correlacion)
                {
                    if (row <= tolCorrPearson)
                        boolean_correlacion.Add(true);
                    else
                        boolean_correlacion.Add(false);
                }

                //Proceso 13 - Validacion 
                //total <= 3 "true"
                int totalTrue = boolean_correlacion.Where(x => x == true).Count();                
                if (totalTrue <= 3)
                {
                    for (int i = 0; i < boolean_correlacion.Count; i++)
                    {
                        boolean_correlacion[i] = true;
                    }
                }

                //Proceso 14 - comparacion boolean distancia vs correlacion
                //.se cuadra los registros de boolean_correlacion                
                foreach (int i in colsRemover)
                {
                    boolean_correlacion.Insert(i, null);
                }

                List<bool> boolean_comparacion = new List<bool>();
                for (int i = 0; i < boolean_distancia.Count; i++)
                {                    
                    if (boolean_correlacion[i] == null)
                        boolean_comparacion.Add(boolean_distancia[i]);
                    else
                    {
                        bool d = boolean_distancia[i];
                        bool c = (bool)boolean_correlacion[i];
                        boolean_comparacion.Add(d && c);
                    }
                }

                //Proceso 15
                List<int> indicesValidos = new List<int>();
                for (int i = 0; i < boolean_comparacion.Count;i++)
                {
                    if (boolean_comparacion[i] == true)
                        indicesValidos.Add(i);
                }
                List<List<decimal?>> temp4 = new List<List<decimal?>>();
                foreach (int i in indicesValidos)
                {
                    temp4.Add(temp1[i]);
                }

                //Proceso 16 - asignacion de pesos a cada dia restante
                List<int> pesos = Enumerable
                    .Range(1, indicesValidos.Count)
                    .ToList();
                int sumaPesos = pesos.Sum();

                //Aplica los pesos
                for (int i = 0; i < temp4.Count; i++)
                {
                    temp4[i] = temp4[i].Select(x => x * pesos[i]).ToList();
                }

                //Obtiene el promedio
                List<decimal?> average_filter_barra = new decimal?[n].ToList();
                if (temp4.Count > 0)
                {
                    for (int i = 0; i < temp4.First().Count; i++)
                    {
                        List<decimal?> average = new List<decimal?>();
                        for (int j = 0; j < temp4.Count; j++)
                        {
                            average.Add(temp4[j][i]);
                        }
                        decimal? valorAverage = average.Sum() / sumaPesos;
                        average_filter_barra[i] = valorAverage;
                    }
                }
                
                //Se carga los datos de la barra
                res.Add(new DemVegCol()
                {
                    Id = dato.Id,
                    SubId = dato.SubId,
                    Nombre = dato.Nombre,
                    Valores = average_filter_barra,
                });
            }

            return res;
        }

        /// <summary>
        /// [CU08] Alisado Gaussiano de la demanda
        /// </summary>
        /// <param name="datosBase">Datos base para el proceso</param>
        /// <param name="length">Longitud de la ventana para el alisado gaussiano</param>
        /// <param name="std">Desvio estándar  para el alisado gaussiano</param>
        public static void AlisadoGaussiano(
            List<decimal?> datosBase, int length,
            double std)
        {
            //Proceso 2
            Interpolacion(datosBase);

            double[] kernel = Gaussian(length, std);
            double kernelSum = kernel.Sum();
            for (int i = 0; i < kernel.Length; i++)
            {
                kernel[i] = kernel[i] / kernelSum;
            }

            double[] signal = new double[datosBase.Count];
            for (int i = 0; i < datosBase.Count; i++)
            {
                signal[i] = Convert.ToDouble(datosBase[i]);
            }

            // Aplicar convolución con el kernel gaussiano
            double[] convolucion = Convolve(signal, kernel);

            // Copiar los resultados a un arreglo de salida
            int halfLength = (length + 1) / 2;  
            for (int i = 0; i < convolucion.Length; i++)
            {
                datosBase[halfLength + i] = (decimal)convolucion[i];
            }
        }

        public List<DemVegCol> ExtraccionDemandaSPRMin(
            DateTime fecha, string idTipo, string idVersion)
        {            
            List<DateTime> listaFechas = new List<DateTime>() { fecha };
            string strIni = fecha.AddDays(-1).ToString("yyyy-MM-dd");
            string strFin = fecha.ToString("yyyy-MM-dd");

            List<PrnMedicion48DTO> datosDemandaSPR = FactorySic.
                GetDpoProcesoPronosticoRepository().
                ObtenerDemandaSRP(strIni, strFin, idTipo, idVersion);

            //[Tabla1]
            List<MePtomedicionDTO> barras = FactorySic
                .GetDpoProcesoPronosticoRepository()
                .ListaBarras();            
            List<DemVegCol> tabla1 = new List<DemVegCol>();
            foreach (MePtomedicionDTO barra in barras)
            {
                int id = (int)barra.Grupocodi;
                string nom = barra.Ptomedibarranomb;
                List<PrnMedicion48DTO> datosPorBarra = datosDemandaSPR
                    .Where(x => x.Gruponomb == nom)
                    .OrderBy(x => x.Medifecha)
                    .ToList();

                if (datosPorBarra.Count == 0)
                {
                    datosPorBarra.Add(new PrnMedicion48DTO()
                    {
                        Grupocodi = id,
                        Gruponomb = nom,
                        Medifecha = fecha,
                    });
                }

                tabla1.AddRange(DarFormatoArreglo(
                    id, nom, datosPorBarra, listaFechas, 48));
            }

            //[Tabla2]
            List<DemVegCol> tabla2 = new List<DemVegCol>();
            foreach (DemVegCol col in tabla1)
            {
                DemVegCol ent = new DemVegCol()
                {
                    Id = col.Id,
                    Nombre =col.Nombre,
                    FechaHora = col.FechaHora,
                    Valores = DarFormatoPorMinuto(col.Valores, 1, 48)
                };
                tabla2.Add(ent);
            }

            //[Tabla2] Interpolacion
            foreach (DemVegCol col in tabla2)
            {
                Interpolacion(col.Valores);
                col.Valores.RemoveAt(0);
            }

            //Reemplaza todos los valores null por 0
            foreach (DemVegCol col in tabla2)
            {
                for (int i = 0; i < col.Valores.Count; i++)
                {
                    decimal? valor = col.Valores[i];
                    if (valor == null) col.Valores[i] = 0;
                }
            }

            return tabla2;
        }

        /// <summary>
        /// Completa los datos haciendo uso de la columna promedio
        /// </summary>
        /// <param name="datos">Datos base para el procedimiento</param>
        /// <param name="average">Datos promedio</param>
        public static void CompletadoDatos(
            List<decimal?> datos, List<decimal?> average)
        {
            decimal?[] columna = datos.ToArray();

            //Rango inicio
            if (datos.First() == null)
            {
                int indexIni = 0;
                for (int i = 1; i < datos.Count; i++)
                {
                    if (datos[i] == null) continue;
                    indexIni = i;
                    break;
                }

                if (indexIni != 0)
                {
                    decimal? ajuste = datos[indexIni] - average[indexIni];
                    for (int i = indexIni - 1; i >= 0; i--)
                    {
                        if (columna[i] != null) continue;
                        decimal? valor = average[i] + ajuste;
                        columna[i] = (valor < 0) ? 0 : valor;
                    }
                }
            }

            //Rango final
            if (datos.Last() == null)
            {
                int indexFin = 0;
                for (int i = datos.Count - 2; i >= 0; i--)
                {
                    if (datos[i] == null) continue;
                    indexFin = i;
                    break;
                }

                if (indexFin != 0)
                {
                    decimal? ajuste = datos[indexFin] - average[indexFin];
                    for (int i = indexFin + 1; i < datos.Count; i++)
                    {
                        if (columna[i] != null) continue;
                        decimal? valor = average[i] + ajuste;
                        columna[i] = (valor < 0) ? 0 : valor;
                    }
                }
            }
            
            List<decimal?> diferencia = new decimal?[datos.Count].ToList();
            for (int i = 0; i < columna.Length; i++)
            {
                if (columna[i] == null) continue;
                diferencia[i] = columna[i] - average[i];
            }

            //Completa los datos de la columna diferencia
            Interpolacion(diferencia);

            //Actualización de valores
            for (int i = 0; i < datos.Count; i++)
            {
                if (datos[i] != null) continue;
                datos[i] = average[i] + diferencia[i];
            }
        }

        /// <summary>
        /// Genera la condicion para la consulta de la "Generacion"
        /// de las fechas por minuto
        /// </summary>
        /// <param name="listaFechas">Lista de fechas a incluir en el query</param>
        /// <returns></returns>
        public string CondicionConsultaGeneracion(
            List<DateTime> listaFechas)
        {
            string fmtFecha = "yyyy-MM-dd";
            string fmtOracle = "yyyy-mm-dd";
            string query = string.Empty;
            string diaBase = string.Empty;
            string diaAnterior = string.Empty;

            query += "(";
            for (int i = 0; i < listaFechas.Count; i++)
            {
                diaBase = listaFechas[i].ToString(fmtFecha);
                diaAnterior = listaFechas[i].AddDays(-1).ToString(fmtFecha);

                if (i > 0) query += " OR ";
                query += $"(medifecha between to_date('{diaAnterior}','{fmtOracle}')";
                query += $" and to_date('{diaBase}','{fmtOracle}'))";
            }
            query += ")";

            return query;
        }

        /// <summary>
        /// Permite dar formato de array a una lista de entidades tipo ME_MEDICION48
        /// </summary>
        /// <param name="idPunto">Identificador del punto</param>
        /// <param name="datosBase">Lista de valores de un punto de medicion</param>
        /// <param name="listaFechas">Lista de fechas base (en orden)</param>
        /// <param name="numIntervalos">Numero de intervalos por "Fecha"</param>
        /// <returns></returns>
        public List<DemVegCol> DarFormatoArreglo(
            int idPunto, List<MeMedicion48DTO> datosBase,
            List<DateTime> listaFechas, int numIntervalos)
        {            
            List<DemVegCol> res = new List<DemVegCol>();
            
            foreach (DateTime dia in listaFechas)
            {
                DemVegCol e = new DemVegCol()
                {
                    Id = idPunto,
                    FechaHora = dia,
                    Valores = new decimal?[numIntervalos].ToList()
                };

                MeMedicion48DTO datoPorDia = datosBase
                    .FirstOrDefault(x => x.Medifecha == dia) ?? null;

                for (int i = 0; i < numIntervalos; i++)
                {
                    if (datoPorDia == null)
                        e.Valores[i] = 0;
                    else
                        e.Valores[i] = (decimal?)datoPorDia
                        .GetType().GetProperty($"H{(i + 1)}")
                        .GetValue(datoPorDia) ?? null;
                }

                //Se inserta el dato del dia previo como primer valor de la lista
                DateTime diaAnterior = dia.AddDays(-1);
                MeMedicion48DTO datoDiaAnterior = datosBase
                    .FirstOrDefault(x => x.Medifecha == diaAnterior) ?? null;
                if (datoDiaAnterior == null)
                    e.Valores.Insert(0, 0);
                else
                    e.Valores.Insert(0, datoDiaAnterior.H48);

                res.Add(e);
            }

            return res;
        }

        /// <summary>
        /// Permite dar formato de array a una lista de entidades tipo PRN_MEDICION48
        /// </summary>
        /// <param name="idBarra">Codigo de la barra</param>
        /// <param name="nomBarra">Identificador de la barra</param>
        /// <param name="datosBase">Lista de valores de un punto de medicion</param>
        /// <param name="listaFechas">Lista de fechas base (en orden)</param>
        /// <param name="numIntervalos">Numero de intervalos por "Fecha"</param>
        /// <returns></returns>
        public List<DemVegCol> DarFormatoArreglo(
            int idBarra, string nomBarra, List<PrnMedicion48DTO> datosBase,
            List<DateTime> listaFechas, int numIntervalos)
        {
            List<DemVegCol> res = new List<DemVegCol>();

            foreach (DateTime dia in listaFechas)
            {
                DemVegCol e = new DemVegCol()
                {
                    Id = idBarra,
                    Nombre = nomBarra,
                    FechaHora = dia,
                    Valores = new decimal?[numIntervalos].ToList()
                };

                PrnMedicion48DTO datoPorDia = datosBase
                    .FirstOrDefault(x => x.Medifecha == dia) ?? null;

                for (int i = 0; i < numIntervalos; i++)
                {
                    if (datoPorDia == null)
                        e.Valores[i] = 0;
                    else
                        e.Valores[i] = (decimal?)datoPorDia
                        .GetType().GetProperty($"H{(i + 1)}")
                        .GetValue(datoPorDia) ?? null;
                }

                //Se inserta el dato del dia previo como primer valor de la lista
                DateTime diaAnterior = dia.AddDays(-1);
                PrnMedicion48DTO datoDiaAnterior = datosBase
                    .FirstOrDefault(x => x.Medifecha == diaAnterior) ?? null;
                if (datoDiaAnterior == null)
                    e.Valores.Insert(0, 0);
                else
                    e.Valores.Insert(0, datoDiaAnterior.H48);

                res.Add(e);
            }

            return res;
        }

        /// <summary>
        /// Permite dar formato agrupado (columna) a una lista de datos
        /// </summary>
        /// <param name="datosBase">Lista de valores de un punto de medicion</param>
        /// <param name="listaFechas">Lista de fechas base (en orden)</param>
        /// <param name="numIntervalos">Numero de intervalos por "Fecha"</param>
        /// <returns></returns>
        public List<decimal?> DarFormatoAgrupado(
            List<DemVegCol> datosBase, List<DateTime> listaFechas,
            int numIntervalos)
        {            
            int intervalos = listaFechas.Count * numIntervalos;
            List<decimal?> res = new decimal?[intervalos].ToList();

            int indice = 0;
            foreach (DateTime dia in listaFechas)
            {
                DemVegCol datoPorDia = datosBase
                    .FirstOrDefault(x => x.FechaHora == dia) ?? null;

                if (datoPorDia == null)
                {
                    indice += numIntervalos;
                    continue;
                }
                    
                for (int i = 1; i <= numIntervalos; i++)
                {
                    res[indice] = datoPorDia.Valores[i];
                    indice++;
                }
            }

            return res;
        }

        /// <summary>
        /// Permite convertir un arreglo de datos a formato por minuto (1440 intervalos)
        /// </summary>
        /// <param name="datosBase">Lista de valores base</param>
        /// <param name="numDias">Numero de dias totales en los valores</param>
        /// <param name="numIntervalos">Numero de intervalos de referencia (cant x día)</param>
        /// <returns></returns>
        public List<decimal?> DarFormatoPorMinuto(
            List<decimal?> datosBase, int numDias, int numIntervalos)
        {
            int filas = numDias * 1440;
            List<decimal?> res = new decimal?[filas].ToList();

            int indice = 1440 / numIntervalos;
            for (int i = 1; i <= datosBase.Count - 1; i++)
            {
                res[i * indice - 1] = datosBase[i];
            }

            //Se inserta el dato del dia previo como primer valor de la lista
            res.Insert(0, datosBase[0]);

            return res;
        }

        /// <summary>
        /// Convierte mediciones agrupadas x hora a formato x minuto
        /// </summary>
        /// <param name="datos">Datos de las mediciones base</param>
        /// <param name="listaFechas">Lista de fechas base</param>        
        /// <returns></returns>
        public List<DemVegCol> DarFormatoPorMinuto(
            List<DpoEstimadorRawDTO> datos, List<DateTime> listaFechas)
        {
            DemVegCol entity = new DemVegCol();
            List<DemVegCol> entities = new List<DemVegCol>();

            List<int> puntosMedicion = datos
                .Select(x => x.Ptomedicodi)
                .Distinct()
                .ToList();

            DateTime fecIni = listaFechas.First();
            DateTime fecFin = listaFechas.Last();

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

                        entity = new DemVegCol()
                        {
                            Id = punto,
                            FechaHora = d,
                            TipoInfo = tipoInfo,
                            Valores = new decimal?[ConstantesDpo.Itv1min].ToList()
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
                                {
                                    if ((decimal)r == 0)
                                    {
                                        entity.Valores[(i * 60) + j] = null;
                                    }
                                    else
                                    {
                                        entity.Valores[(i * 60) + j] = (decimal)r;
                                    }                                    
                                }                                    
                                else
                                {
                                    entity.Valores[(i * 60) + j] = null;
                                }                                    
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
        /// Devuelve la lista de codigos validos desde una lista formulas
        /// </summary>
        /// <param name="listaFormulas">Lista de formulas base</param>
        /// <returns></returns>
        public List<int> ObtenerListaCodigos(
            List<DpoFormulaDTO> listaFormulas)
        {
            List<int> res = new List<int>();
            foreach (DpoFormulaDTO frml in listaFormulas)
            {
                res.AddRange(ExtraerDetallePorFormula(frml.Formula_P)
                    .Select(x => x.Id));
                res.AddRange(ExtraerDetallePorFormula(frml.Formula_S)
                    .Select(x => x.Id));
            }
            res = res.Distinct().ToList();
            return res;
        }

        /// <summary>
        /// Extrae los codigos y factores de una formula
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public List<DemVegFrm> ExtraerDetallePorFormula(string formula)
        {
            string[] arry = formula.Split('+');

            List<DemVegFrm> res = new List<DemVegFrm>();
            foreach (string a in arry)
            {
                //Y: Estimador TNA(IEOD) [Demanda]
                //B: Despacho ejecutado diario [Generacion]
                List<char> caracteres = new List<char>
                {
                   'D', 'C', 'A', 'B', 'M', 'E', 'S',
                    'U', 'X', 'Y', 'Z', 'F', 'I', 'G'
                };
                foreach (char c in caracteres)
                {
                    int index = a.LastIndexOf(c);
                    if (index != -1)
                    {
                        int id = int.Parse(a.Substring(index + 1));
                        decimal factor = decimal.Parse(a.Substring(0, index));
                        res.Add(new DemVegFrm()
                        {
                            Id = id,
                            Tipo = c,
                            Factor = factor
                        });
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Devuelve el valor de medicion de una formula
        /// </summary>
        /// <param name="formula">Formula a calcular</param>
        /// <param name="datosFormulas">Fuente de datos de las Formulas</param>
        /// <param name="numIntervalos">Numero de intervalos esperados</param>
        /// <param name="flag">Flag para el uso de los valores NaN</param>
        /// <returns></returns>
        public List<decimal?> ObtenerValoresPorFormula(
            string formula, List<DemVegCol> datosFormulas,
            int numIntervalos, bool flag)
        {
            List<decimal?> valores = new decimal?[numIntervalos].ToList();
            List<DemVegFrm> detalle = ExtraerDetallePorFormula(formula);
            foreach (DemVegFrm d in detalle)
            {
                int valid = valores.Where(x => x == null).Count();

                decimal factor = d.Factor;
                DemVegCol dato = new DemVegCol();
                dato = datosFormulas.FirstOrDefault(x => x.Id == d.Id)
                        ?? null;

                if (dato == null) continue;

                int totalNan = dato.Valores
                    .Where(x => x == null)
                    .Count();
                if (totalNan == dato.Valores.Count)
                    dato.Valores = dato.Valores
                        .Select(x => x = 0)
                        .ToList();
                
                List<decimal?> calc = dato.Valores
                    .Select(x => x * factor)
                    .ToList();

                if (valid == numIntervalos)
                    valores = calc;
                else
                    valores = SumarListas(valores, calc, flag);
            }

            return valores;
        }

        /// <summary>
        /// Crea la columna de HoraFecha para los reportes y calculos
        /// </summary>
        /// <param name="listaFechas">Lista de fechas base</param>
        /// <param name="intervalo">Intervalo de tiempo a utilizar (1, 15, 30)</param>
        /// <param name="primerIntervalo">Flag para crear primer intervalo (00:00:00)</param>
        /// <returns></returns>
        public List<string> ObtenerColumnaIntervalos(
            List<DateTime> listaFechas, int intervalo,
            bool primerIntervalo)
        {
            int totalIntervalos = 1440 / intervalo;
            List<string> res = new List<string>();
            foreach (DateTime dia in listaFechas)
            {
                for (int i = 1; i <= totalIntervalos; i++)
                {
                    string str = dia.AddMinutes(i * intervalo)
                        .ToString("yyy-MM-dd HH:mm:ss");
                    res.Add(str);
                }
            }

            if (primerIntervalo)
            {
                DateTime dia = listaFechas.First();
                res.Insert(0, dia.ToString("yyy-MM-dd HH:mm:ss"));
            }
            
            return res;
        }

        public List<decimal?> SumarListas(
            List<decimal?> listaA, List<decimal?> listaB, bool flag)
        {
            int intervalos = listaA.Count;
            List<decimal?> res = new decimal?[intervalos].ToList();
            for (int i = 0; i < intervalos; i++)
            {
                if (flag)
                {
                    if (listaA[i] == null && listaB[i] == null)
                        continue;
                    else if (listaA[i] == null)
                        res[i] = listaB[i];
                    else if (listaB[i] == null)
                        res[i] = listaA[i];
                    else
                        res[i] = listaA[i] + listaB[i];
                }
                else
                {
                    if (listaA[i] == null && listaB[i] == null)
                        continue;
                    else if (listaA[i] == null || listaB[i] == null)
                        continue;
                    else
                        res[i] = listaA[i] + listaB[i];
                }
            }

            return res;
        }

        public int ObtenerIntervaloDesdeHora(DateTime tiempo)
        {
            int horas = tiempo.Hour;
            int minutos = tiempo.Minute;
            int intervalo = (horas * 60) + minutos;

            return intervalo;
        }

        public List<decimal?> DarFormatoPorIntervalos(
            List<decimal?> datos, int intervalos)
        {
            List<decimal?> res = new decimal?[intervalos].ToList();
            int n = 1440 / intervalos;

            for (int i = 1; i <= intervalos; i++)
            {
                res[i - 1] = datos[(i * n) - 1];
            }

            return res;
        }

        #region Operaciones
        public static void Interpolacion(
            List<decimal?> datos)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < datos.Count; i++)
            {
                if (datos[i] != null)
                    indices.Add(i);
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
                decimal vIni = (decimal)datos[rIni];
                decimal vFin = (decimal)datos[rFin];

                decimal pendiente = Pendiente(
                    rIni, vIni, rFin, vFin);

                //Completa el rango
                for (int j = rIni + 1; j < rFin; j++)
                {
                    decimal valor = InterpolacionLineal(
                        j, rIni, vIni, pendiente);
                    
                    datos[j] = valor;
                    //datos[j] = Math.Round(valor, numDecimales);
                }
            }
        }

        public static decimal InterpolacionLineal(
            int x, int xRef, decimal yRef, decimal pendiente)
        {
            decimal diff = (x - xRef);
            return pendiente * diff + yRef;
        }

        public static decimal Pendiente(
            int x1, decimal y1, int x2, decimal y2)
        {
            decimal res = 0;
            try
            {
                decimal diff = x2 - x1;
                res = (y2 - y1) / diff;
            }
            catch
            {
                res = 0;
            }
            return res;
        }

        static decimal? CorrelacionPearson(
            List<decimal?> x, List<decimal?> y)
        {
            int n = x.Count;
            if (n != y.Count)
            {
                throw new ArgumentException("Los arreglos deben tener la misma longitud");
            }

            decimal? sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0, sumY2 = 0;

            for (int i = 0; i < n; i++)
            {
                decimal? valorX = (x[i] != null) ? x[i] : 0;
                decimal? valorY = (y[i] != null) ? y[i] : 0;

                sumX += valorX;
                sumY += valorY;
                sumXY += valorX * valorY;
                sumX2 += valorX * valorX;
                sumY2 += valorY * valorY;
            }

            decimal? numerator = n * sumXY - sumX * sumY;
            double calc = (double)(n * sumX2 - sumX * sumX) * (double)(n * sumY2 - sumY * sumY);
            decimal? denominator = (decimal?)Math.Sqrt(calc);

            if (denominator == 0)
            {
                // Manejar el caso especial de división por cero
                return 0;
            }

            return numerator / denominator;
        }

        public static double[] Gaussian(int M, double std)
        {
            if (M < 1) return new double[0];

            if (M == 1) return new double[] { 1.0 };

            int odd = M % 2;
            if (!true && odd == 0) M = M + 1;

            double[] res = new double[M];
            double[] n = new double[M];
            double sig2 = 2 * std * std;

            for (int i = 0; i < M; i++)
            {
                n[i] = i - (M - 1.0) / 2.0;
                res[i] = Math.Exp(-n[i] * n[i] / sig2);
            }

            if (!true && odd == 0)
                Array.Resize(ref res, M - 1);

            return res;
        }

        public static double[] Convolve(double[] input, double[] kernel)
        {
            int inputLength = input.Length;
            int kernelLength = kernel.Length;
            int resultLength = inputLength - kernelLength + 1;
            double[] result = new double[resultLength];

            for (int i = 0; i < resultLength; i++)
            {
                double sum = 0;
                for (int j = 0; j < kernelLength; j++)
                {
                    sum += input[i + j] * kernel[kernelLength - j - 1];
                }
                result[i] = sum;
            }

            return result;

        }
        #endregion
    }
}
