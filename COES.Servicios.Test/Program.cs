using COES.Dominio.DTO.SGDoc;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Coordinacion;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Aplicacion.DemandaBarras;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.Servicios.Aplicacion.Recomendacion;
using COES.Servicios.Aplicacion.RegistroObservacion;
using COES.Servicios.Aplicacion.SGDoc;
using COES.Servicios.Aplicacion.Subastas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Test
{
    class Program
    {

        static void Main(string[] args)
        {
            (new GenerarWordResarcimiento()).GenerarWordDemo();

            Console.Read();
            //GenerarExcelDespacho();

            //(new DemandaPOAppServicio()).ObtenerDatosRawCada5Mninutos();
            //(new DemandaPOAppServicio()).ObtenerDatosRawIEOD1Dia();

            /*DateTime fecha = new DateTime(2022, 7, 1);
            (new CostoOportunidadAppServicio()).EjecutarProcesoDiario(fecha, ConstantesCostoOportunidad.TipoManual, "");


            
            int bloques = 1;
            int segundos = 24*60*60;
            DateTime fecha1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            for (int i = 0; i < bloques; i++)
            {
                DateTime fecInicio = fecha1.AddSeconds(i * segundos);
                DateTime fecFin = fecha1.AddSeconds((i + 1) * segundos - 1);
                int add = 0;
                if (fecInicio.Minute == 0 || fecInicio.Minute == 30) add = 1;

                int periodoInicio = (new CostoOportunidadAppServicio()).CalcularPeriodo(fecInicio) + add;
                int periodoFin = (new CostoOportunidadAppServicio()).CalcularPeriodo(fecFin);
                Console.WriteLine("Inicio: " + fecInicio.ToString("HH:mm") + "-" + periodoInicio + ", Fin: " + fecFin.ToString("HH:mm") + "-" + periodoFin );
            }


                Console.WriteLine("Proceso terminado");
            Console.ReadLine();*/



            DateTime fecha = DateTime.ParseExact("04/12/2024 11:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            COES.Servicios.Aplicacion.CortoPlazo.CostoMarginalTnaV2AppServicio servicio = new Aplicacion.CortoPlazo.CostoMarginalTnaV2AppServicio();
            servicio.Procesar(fecha, 1, true, false, false, string.Empty, true, 0, "raul.castro", 1);

            //DateTime fecha = DateTime.ParseExact("25/02/2022 11:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //DateTime fecha = DateTime.ParseExact("13/08/2022 04:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //COES.Servicios.Aplicacion.CortoPlazo.CostoMarginalTnaAppServicio servicio = new Aplicacion.CortoPlazo.CostoMarginalTnaAppServicio();
            //servicio.Procesar(fecha, 1, true, false, false, string.Empty, true, 0, "raul.castro", 1);

            //(new TramiteVirtualAppServicio()).EnviarNotificacionAgente();

            //DateTime fechaProceso = DateTime.ParseExact("17/08/2020 15:20", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //CostoMarginalTnaAppServicio servicio = new CostoMarginalTnaAppServicio();
            //servicio.ObtenerAlertasCostosMarginales(DateTime.Now);

            //DatosResumenMovil e = (new PortalAppServicio()).ObtenerDatosResumen();

            //(new TramiteVirtualAppServicio()).EnviarNotificacionAgente();

            //TramiteVirtualAppServicio se = new TramiteVirtualAppServicio();
            //se.EnviarNotificacionAgente();
            //se.CrearCredencialesFicticias();
            //CostoMarginalAppServicio servicio = new CostoMarginalAppServicio();
            //ResultadoValidacion validacion = servicio.ObtenerAlertasCostosMarginales(DateTime.ParseExact("2019/07/14 08:30", "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture));

            //(new PortalAppServicio()).ObtenerDatosResumen();
            //SeguimientoRecomendacionAppServicio seguimiento = new SeguimientoRecomendacionAppServicio();
            //seguimiento.EnvioAlarma();                    

            //DateTime fecha = DateTime.ParseExact("30/03/2021 16:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            // (new PronosticoDemandaAppServicio()).ObtenerDatosRawSco();

            //COES.Servicios.Aplicacion.CortoPlazo.CostoMarginalTnaAppServicio servicio = new Aplicacion.CortoPlazo.CostoMarginalTnaAppServicio();
            //servicio.Procesar(fecha, 0, true, false, false, string.Empty, true, 0, "raul.castro");

            //CostoMarginalAppServicio cm = new CostoMarginalAppServicio();
            //cm.Procesar(fecha, 1, true, true, false, "ofi30", false, 0, "automatico");

            //DateTime fecha = DateTime.ParseExact("18/03/2020 10:30", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //COES.Servicios.Aplicacion.CortoPlazo.CostoMarginalAppServicio servicio = new Aplicacion.CortoPlazo.CostoMarginalAppServicio();
            //servicio.Procesar(fecha, 0, false, false, false, string.Empty, false, 0, "raul.castro");

            //DateTime fechaProceso = DateTime.ParseExact("05/03/2020 07:30", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //CostoMarginalAppServicio servicio = new CostoMarginalAppServicio();
            //ResultadoValidacion resultado = servicio.ObtenerAlertasCostosMarginales(fechaProceso);

            //Console.Write(resultado);

            //COES.Servicios.Aplicacion.CortoPlazo.CostoMarginalAppServicio servicio = new Aplicacion.CortoPlazo.CostoMarginalAppServicio();
            //servicio.ProcesarMasivo(fechaInicio, fechaFin, horas.ToList());

            //-Procesar calcula maxima demanda mes anterior
            //DateTime fecha = new DateTime(2020, 8, 1);
            //(new PortalAppServicio()).CalcularMaximaDemandaMensual(fecha);
            //string result = "";

            ////- Costos Marginales Programados
            //DateTime fechaInicio = DateTime.ParseExact("01/06/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime fechaFin = DateTime.ParseExact("30/06/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //(new CortoPlazoAppServicio()).CargarCostosMarginalesProgramados(fechaInicio, fechaFin, "App-MMME");
            //GenerarArchivosNCP();
        }

        /// <summary>
        /// Permite obtener el número de semana del año
        /// </summary>
        /// <param name="Fecha">Fecha a consultar</param>
        /// <returns>Semana del año</returns>
        public static int ObtenerNroSemanaAnio(DateTime Fecha)
        {
            GregorianCalendar Calendario = new GregorianCalendar();
            return Calendario.GetWeekOfYear(Fecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Saturday);
        }

        /// <summary>
        /// Calcula el periodo en el que se está ejecutando el proceso
        /// </summary>
        /// <returns></returns>
        public static int CalcularPeriodo(DateTime fecha)
        {
            int totalMinutes = fecha.Hour * 60 + fecha.Minute;
            return Convert.ToInt32(Math.Ceiling(((decimal)totalMinutes / 30.0M)));
        }

        /// <summary>
        /// Obtiene los datos de generacion
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static GraficoGeneracion ObtenerReporteGeneracion()
        {
            GraficoGeneracion resultado = new GraficoGeneracion();
            DateTime fecha = DateTime.Now;

            DateTime fechaEjecutado;
            decimal valorEjecutado;
            List<PuntoSerie> valorPorEmpresa = (new PortalAppServicio()).ObtenerGeneracionPorEmpresaReporte(fecha, fecha);

            var list = valorPorEmpresa.ToList().OrderByDescending(c => c.Valor);
            var top10 = list.Take(10);

            var response = new List<PuntoSerie>();
            response.AddRange(top10);
            response.Add(new PuntoSerie { Nombre = "OTROS", Valor = list.Skip(10).Sum(c => c.Valor) });

            List<PuntoSerie> valoresPorTipoCombustible = (new PortalAppServicio()).ObtenerChartProduccionHome(out fechaEjecutado, out valorEjecutado);

            resultado.GeneracionPorEmpresa = response.OrderBy(x => x.Valor).ToList();//valorPorEmpresa;
            resultado.GeneracionPorTipoCombustible = valoresPorTipoCombustible;
            resultado.LastValue = valorEjecutado;

            return resultado;
        }

        public static void GenerarArchivosNCP()
        {

            SubastasAppServicio subastaAppServicio = new SubastasAppServicio();
            var valor = subastaAppServicio.DecryptData("V0ZZGY2kK8wRvEjqSHEjgQ==");
            //subastaAppServicio.GenerarArchivosAutomatico(4);
        }

        public static void GenerarExcelDespacho()
        {
            DateTime fechaInicial = new DateTime(2018, 01, 01);
            DateTime fechaFinal = new DateTime(2018, 01, 03);
            string rutaArchivo = "C:\\Users\\usuariow10\\source\\repos\\FramworkCoesProd\\COES.MVC.Intranet\\Temporales\\Rpt_Despacho.xlsx";

            EjecutadoAppServicio servEjec = new EjecutadoAppServicio();
            servEjec.GenerarReporteDespachoYReservaFria(fechaInicial, fechaFinal, rutaArchivo);
        }
    }
}
