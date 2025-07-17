using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Transferencias;
using COES.WebService.CostoMarginal.Contratos;
using log4net;

namespace COES.WebService.CostoMarginal.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class CostoMarginalServicio : ICostoMarginalServicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CostoMarginalServicio));
        //private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CostoMarginalServicio));
        public DateTime FechaInicio { get; set; }
        public string[][] Data { get; set; }
        public DateTime FechaFin { get; set; }
        public List<string> ListaHoras { get; set; }
        public string TipoEstimador { get; set; }
        public bool FlagMD { get; set; }
        public string Usuario { get; set; }
        public string[][] DataTIE { get; set; }
        public int Barra { get; set; }
        public DateTime FechaProceso { get; set; }
        public int VersionModelo { get; set; }
        public string Correlativos { get; set; }

        public CostoMarginalServicio()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public List<Dominio.DTO.Transferencias.CostoMarginalDTO> ListarCostosMarginales(int anio, int mes)
        {
            int periodo = Convert.ToInt32(anio.ToString() + "" + mes.ToString().PadLeft(2, '0'));
            int version = 1;
            PeriodoDTO entidadPeriodo = (new PeriodoAppServicio()).GetByAnioMes(periodo);
            List<RecalculoDTO> list = FactoryTransferencia.GetRecalculoRepository().List(entidadPeriodo.PeriCodi);
            if (list.Count > 0) version = list[0].RecaCodi;
            List<CostoMarginalDTO> listaCostoMarginal = (new COES.Servicios.Aplicacion.Transferencias.CostoMarginalAppServicio()).ListCostoMarginalByBarraPeridoVersion(14, entidadPeriodo.PeriCodi, version); //Se usa la barra SANTAROSA220 de TRN_BARRA
            return listaCostoMarginal;
        }

        /// <summary>
        /// Ejecuta el proceso de costos marginales nodales
        /// </summary>
        /// <param name="fecha"></param>
        public void EjecutarCostosMarginales(DateTime fecha, int indicadorPSSE, bool reproceso, bool indicadorNCP, bool flagWeb)
        {
            CalculoCostosMarginales cm = new CalculoCostosMarginales();
            cm.EjecutarProcesoCM(fecha, indicadorPSSE, reproceso, indicadorNCP, flagWeb, string.Empty, true, 0, "automatico", 0);
        }

        /// <summary>
        /// Ejecuta el proceso de costos marginales nodales
        /// </summary>
        /// <param name="fecha"></param>
        public void EjecutarCostosMarginalesAlterno(DateTime fecha, int indicadorPSSE, bool reproceso, bool indicadorNCP, bool flagWeb, string rutaNCP,
            bool flagMD, int idEscenario, string usuario, string tipoEstimador, int tipo, int version)
        {
            CalculoCostosMarginales servicio = new CalculoCostosMarginales();
            servicio.ProcesarCM(fecha, tipoEstimador, indicadorPSSE, reproceso, indicadorNCP, flagWeb,
                rutaNCP, flagMD, idEscenario, usuario, tipo, version);

        }

        /// <summary>
        /// Validación de datos antes del proceso de calculo CM
        /// </summary>
        /// <param name="fecha"></param>
        public void ValidacionProcesoCostosMarginales(DateTime fecha)
        {
            CostoMarginalTnaAppServicio servicio = new CostoMarginalTnaAppServicio();
            servicio.ValidacionProceso(fecha);
        }

        /// <summary>
        /// Permite obtener las validaciones de costos marginales
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public COES.Servicios.Aplicacion.CortoPlazo.Helper.ResultadoValidacion ObtenerAlertasCostosMarginales(DateTime fechaProceso)
        {
            CostoMarginalTnaAppServicio servicio = new CostoMarginalTnaAppServicio();
            return servicio.ObtenerAlertasCostosMarginales(fechaProceso);
        }

        /// <summary>
        /// Reprocesar costos marginales masivamente
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="valores"></param>
        public int EjecutarReprocesoMasivo(DateTime fechaInicio, DateTime fechaFin, List<string> horas, bool flagMD, string usuario, string tipoEstimador,
            int version)
        {
            try
            {
                this.FechaInicio = fechaInicio;
                this.FechaFin = fechaFin;
                this.ListaHoras = horas;
                this.TipoEstimador = tipoEstimador;
                this.FlagMD = flagMD;
                this.Usuario = usuario;
                this.VersionModelo = version;
                Task.Factory.StartNew(this.EjecutarMasivoAsincrono);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Ejecuta asincronamente el reproceso masivo
        /// </summary>
        private void EjecutarMasivoAsincrono()
        {
            CalculoCostosMarginales servicio = new CalculoCostosMarginales();
            servicio.ProcesarMasivoCM(this.FechaInicio, this.FechaFin, this.ListaHoras, this.FlagMD, this.Usuario, this.TipoEstimador, this.VersionModelo);

        }

        /// <summary>
        /// Reprocesar costos marginales masivamente
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="valores"></param>
        public int EjecutarReprocesoMasivoModificado(string[][] datos, string usuario, int version)
        {
            try
            {
                this.Data = datos;
                this.Usuario = usuario;
                this.VersionModelo = version;
                Task.Factory.StartNew(this.EjecutarMasivoAsincronoModificado);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Ejecuta asincronamente el reproceso masivo
        /// </summary>
        private void EjecutarMasivoAsincronoModificado()
        {
            CalculoCostosMarginales servicio = new CalculoCostosMarginales();
            servicio.ProcesarMasivoCMModificado(this.Data, this.Usuario, this.VersionModelo);
        }


        public int EjecutarReprocesoTIE(string[][] datos, string usuario, int barra, DateTime fechaProceso, int version)
        {
            try
            {
                this.DataTIE = datos;
                this.FechaProceso = fechaProceso;
                this.Barra = barra;
                this.Usuario = usuario;
                this.VersionModelo = version;
                Task.Factory.StartNew(this.EjecutarMasivoTIE);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Ejecuta asincronamente el reproceso masivo
        /// </summary>
        private void EjecutarMasivoTIE()
        {
            CalculoCostosMarginales servicio = new CalculoCostosMarginales();
            servicio.ProcesarMasivoTIE(this.DataTIE, this.Usuario, this.FechaProceso, this.Barra, this.VersionModelo);
        }

        public int EjecutarReprocesoVA(string horas, string usuario, DateTime fechaProceso, int version)
        {
            try
            {
                this.Correlativos = horas;
                this.FechaProceso = fechaProceso;
                this.Usuario = usuario;
                this.VersionModelo = version;
                Task.Factory.StartNew(this.EjecutarMasivoVA);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Ejecuta asincronamente el reproceso masivo
        /// </summary>
        private void EjecutarMasivoVA()
        {
            CalculoCostosMarginales servicio = new CalculoCostosMarginales();
            servicio.ProcesarMasivoVA(this.Correlativos, this.Usuario, this.FechaProceso, this.VersionModelo);
        }

    }
}