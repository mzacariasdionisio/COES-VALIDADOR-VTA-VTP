using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.Servicios.Aplicacion.PotenciaFirme;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using GAMS;
using log4net;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static COES.Servicios.Aplicacion.PotenciaFirmeRemunerable.Helper.EntidadesAdicionales;

namespace COES.Servicios.Aplicacion.PotenciaFirmeRemunerable
{
    /// <summary>
    /// Clases con métodos del módulo Potencia Firme Remunerable
    /// </summary>
    public class PotenciaFirmeRemunerableAppServicio : AppServicioBase
    {
        private readonly PotenciaFirmeAppServicio pfAppServicio = new PotenciaFirmeAppServicio();
        private readonly ReporteMedidoresAppServicio _medidoresAppServicio;
        private readonly EquipamientoAppServicio _equipamientoAppServicio;
        private readonly INDAppServicio _indAppService;
        private readonly TransfPotenciaAppServicio _transfAppService;
        private readonly PeriodoAppServicio _periodoAppServicio;
        private readonly ConsultaMedidoresAppServicio _consultaMedidoresAppServicio;

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PotenciaFirmeAppServicio));

        public PotenciaFirmeRemunerableAppServicio()
        {
            _medidoresAppServicio = new ReporteMedidoresAppServicio();
            _equipamientoAppServicio = new EquipamientoAppServicio();
            _indAppService = new INDAppServicio();
            _transfAppService = new TransfPotenciaAppServicio();
            _periodoAppServicio = new PeriodoAppServicio();
            _consultaMedidoresAppServicio = new ConsultaMedidoresAppServicio();
        }

        #region CRUD Tablas BD COES

        #region Métodos Tabla PFR_CUADRO

        /// <summary>
        /// Inserta un registro de la tabla PFR_CUADRO
        /// </summary>
        public void SavePfrCuadro(PfrCuadroDTO entity)
        {
            try
            {
                FactorySic.GetPfrCuadroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_CUADRO
        /// </summary>
        public void UpdatePfrCuadro(PfrCuadroDTO entity)
        {
            try
            {
                FactorySic.GetPfrCuadroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_CUADRO
        /// </summary>
        public void DeletePfrCuadro(int pfrcuacodi)
        {
            try
            {
                FactorySic.GetPfrCuadroRepository().Delete(pfrcuacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_CUADRO
        /// </summary>
        public PfrCuadroDTO GetByIdPfrCuadro(int pfrcuacodi)
        {
            return FactorySic.GetPfrCuadroRepository().GetById(pfrcuacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_CUADRO
        /// </summary>
        public List<PfrCuadroDTO> ListPfrCuadros()
        {
            return FactorySic.GetPfrCuadroRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrCuadro
        /// </summary>
        public List<PfrCuadroDTO> GetByCriteriaPfrCuadros()
        {
            return FactorySic.GetPfrCuadroRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PFR_ESCENARIO

        /// <summary>
        /// Inserta un registro de la tabla PFR_ESCENARIO
        /// </summary>        
        public void SavePfrEscenario(PfrEscenarioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfrEscenarioRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_ESCENARIO
        /// </summary>
        public void UpdatePfrEscenario(PfrEscenarioDTO entity)
        {
            try
            {
                FactorySic.GetPfrEscenarioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_ESCENARIO
        /// </summary>
        public void DeletePfrEscenario(int pfresccodi)
        {
            try
            {
                FactorySic.GetPfrEscenarioRepository().Delete(pfresccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_ESCENARIO
        /// </summary>
        public PfrEscenarioDTO GetByIdPfrEscenario(int pfresccodi)
        {
            return FactorySic.GetPfrEscenarioRepository().GetById(pfresccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_ESCENARIO
        /// </summary>
        public List<PfrEscenarioDTO> ListPfrEscenarios()
        {
            return FactorySic.GetPfrEscenarioRepository().List();
        }

        /// <summary>
        /// Permite listar todos los escenario de cierto reporte
        /// </summary>
        public List<PfrEscenarioDTO> ListPfrEscenariosByReportecodi(int pfrrptcodi)
        {
            var lstEsc = FactorySic.GetPfrEscenarioRepository().ListByReportecodi(pfrrptcodi).OrderBy(x => x.Pfrescfecini).ToList();

            int i = 1;
            foreach (var item in lstEsc)
            {
                item.FechaDesc = GetDescripcionEscenario(item.Pfrescfecini, item.Pfrescfecfin);
                item.Numero = i;
                i++;
            }

            return lstEsc;
        }

        private static string GetDescripcionEscenario(DateTime fechaIni, DateTime fechaFin)
        {
            var fechaDesc = (fechaIni.Day != fechaFin.Day) ? fechaIni.Day + "-" + fechaFin.Day : fechaIni.Day + "";
            fechaDesc += " de " + EPDate.f_NombreMes(fechaIni.Month).ToUpper();

            return fechaDesc;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrEscenario
        /// </summary>
        public List<PfrEscenarioDTO> GetByCriteriaPfrEscenarios()
        {
            return FactorySic.GetPfrEscenarioRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PFR_PERIODO

        /// <summary>
        /// Inserta un registro de la tabla PFR_PERIODO
        /// </summary>
        public void SavePfrPeriodo(PfrPeriodoDTO entity)
        {
            try
            {
                FactorySic.GetPfrPeriodoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_PERIODO
        /// </summary>
        public void UpdatePfrPeriodo(PfrPeriodoDTO entity)
        {
            try
            {
                FactorySic.GetPfrPeriodoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_PERIODO
        /// </summary>
        public void DeletePfrPeriodo(int pfrpercodi)
        {
            try
            {
                FactorySic.GetPfrPeriodoRepository().Delete(pfrpercodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_PERIODO
        /// </summary>
        public PfrPeriodoDTO GetByIdPfrPeriodo(int pfrpercodi)
        {
            var periodo = FactorySic.GetPfrPeriodoRepository().GetById(pfrpercodi);
            periodo.FechaIni = new DateTime(periodo.Pfrperanio, periodo.Pfrpermes, 1);
            periodo.FechaFin = periodo.FechaIni.AddMonths(1).AddSeconds(-1);

            return periodo;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_PERIODO
        /// </summary>
        public List<PfrPeriodoDTO> ListPfrPeriodos()
        {
            return FactorySic.GetPfrPeriodoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrPeriodo
        /// </summary>
        public List<PfrPeriodoDTO> GetByCriteriaPfrPeriodos(int anio)
        {
            return FactorySic.GetPfrPeriodoRepository().GetByCriteria(anio).OrderByDescending(x => x.Pfrperaniomes).ToList();
        }

        #endregion

        #region Métodos Tabla PFR_RECALCULO

        /// <summary>
        /// Inserta un registro de la tabla PFR_RECALCULO
        /// </summary>
        public int SavePfrRecalculo(PfrRecalculoDTO entity)
        {
            try
            {
                return FactorySic.GetPfrRecalculoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_RECALCULO
        /// </summary>
        public void UpdatePfrRecalculo(PfrRecalculoDTO entity)
        {
            try
            {
                FactorySic.GetPfrRecalculoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_RECALCULO
        /// </summary>
        public void DeletePfrRecalculo(int pfrreccodi)
        {
            try
            {
                FactorySic.GetPfrRecalculoRepository().Delete(pfrreccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_RECALCULO
        /// </summary>
        public PfrRecalculoDTO GetByIdPfrRecalculo(int pfrreccodi)
        {
            var reg = FactorySic.GetPfrRecalculoRepository().GetById(pfrreccodi);
            this.FormatearPfrRecalculo(reg);
            return reg;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_RECALCULO
        /// </summary>
        public List<PfrRecalculoDTO> ListPfrRecalculos()
        {
            var lista = FactorySic.GetPfrRecalculoRepository().List();

            foreach (var reg in lista)
                this.FormatearPfrRecalculo(reg);

            return lista;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrRecalculo
        /// </summary>
        public List<PfrRecalculoDTO> GetByCriteriaPfrRecalculos(int pfrPericodi)
        {
            var lista = FactorySic.GetPfrRecalculoRepository().GetByCriteria(pfrPericodi).OrderByDescending(x => x.Pfrreccodi).ToList();

            foreach (var reg in lista)
                this.FormatearPfrRecalculo(reg);

            return lista;
        }

        /// <summary>
        /// Da formato a los estados (ejm: "A" -> "Abierto") y fechas
        /// </summary>
        /// <param name="reg"></param>
        private void FormatearPfrRecalculo(PfrRecalculoDTO reg)
        {
            reg.UltimaModificacionFechaDesc = reg.Pfrrecfecmodificacion != null ? reg.Pfrrecfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : reg.Pfrrecfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
            reg.UltimaModificacionUsuarioDesc = reg.Pfrrecfecmodificacion != null ? reg.Pfrrecusumodificacion : reg.Pfrrecusucreacion;
            reg.PfrrecfechalimiteDesc = reg.Pfrrecfechalimite.ToString(ConstantesAppServicio.FormatoFechaFull);

            reg.Estado = DateTime.Now < reg.Pfrrecfechalimite ? ConstantesPotenciaFirmeRemunerable.EstadoPeriodoAbierto : ConstantesPotenciaFirmeRemunerable.EstadoPeriodoCerrado;
            reg.PfrrecestadoDesc = reg.Estado == ConstantesPotenciaFirmeRemunerable.Abierto ? "Abierto" : "Cerrado";
        }

        #endregion

        #region Métodos Tabla PFR_RELACION_POTENCIA_FIRME

        /// <summary>
        /// Inserta un registro de la tabla PFR_RELACION_POTENCIA_FIRME
        /// </summary>
        public void SavePfrRelacionPotenciaFirme(PfrRelacionPotenciaFirmeDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfrRelacionPotenciaFirmeRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_RELACION_POTENCIA_FIRME
        /// </summary>
        public void UpdatePfrRelacionPotenciaFirme(PfrRelacionPotenciaFirmeDTO entity)
        {
            try
            {
                FactorySic.GetPfrRelacionPotenciaFirmeRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_RELACION_POTENCIA_FIRME
        /// </summary>
        public void DeletePfrRelacionPotenciaFirme(int pfrrpfcodi)
        {
            try
            {
                FactorySic.GetPfrRelacionPotenciaFirmeRepository().Delete(pfrrpfcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_RELACION_POTENCIA_FIRME
        /// </summary>
        public PfrRelacionPotenciaFirmeDTO GetByIdPfrRelacionPotenciaFirme(int pfrrpfcodi)
        {
            return FactorySic.GetPfrRelacionPotenciaFirmeRepository().GetById(pfrrpfcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_RELACION_POTENCIA_FIRME
        /// </summary>
        public List<PfrRelacionPotenciaFirmeDTO> ListPfrRelacionPotenciaFirmes()
        {
            return FactorySic.GetPfrRelacionPotenciaFirmeRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrRelacionPotenciaFirme
        /// </summary>
        public List<PfrRelacionPotenciaFirmeDTO> GetByCriteriaPfrRelacionPotenciaFirmes(int pfrrptcodi)
        {
            return FactorySic.GetPfrRelacionPotenciaFirmeRepository().GetByCriteria(pfrrptcodi);
        }

        #endregion

        #region Métodos Tabla PFR_RELACION_INDISPONIBILIDAD

        /// <summary>
        /// Inserta un registro de la tabla PFR_RELACION_INDISPONIBILIDAD
        /// </summary>
        public void SavePfrRelacionIndisponibilidad(PfrRelacionIndisponibilidadDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfrRelacionIndisponibilidadRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_RELACION_INDISPONIBILIDAD
        /// </summary>
        public void UpdatePfrRelacionIndisponibilidad(PfrRelacionIndisponibilidadDTO entity)
        {
            try
            {
                FactorySic.GetPfrRelacionIndisponibilidadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_RELACION_INDISPONIBILIDAD
        /// </summary>
        public void DeletePfrRelacionIndisponibilidad(int pfrrincodi)
        {
            try
            {
                FactorySic.GetPfrRelacionIndisponibilidadRepository().Delete(pfrrincodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_RELACION_INDISPONIBILIDAD
        /// </summary>
        public PfrRelacionIndisponibilidadDTO GetByIdPfrRelacionIndisponibilidad(int pfrrincodi)
        {
            return FactorySic.GetPfrRelacionIndisponibilidadRepository().GetById(pfrrincodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_RELACION_INDISPONIBILIDAD
        /// </summary>
        public List<PfrRelacionIndisponibilidadDTO> ListPfrRelacionIndisponibilidads()
        {
            return FactorySic.GetPfrRelacionIndisponibilidadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrRelacionIndisponibilidad
        /// </summary>
        public List<PfrRelacionIndisponibilidadDTO> GetByCriteriaPfrRelacionIndisponibilidads(int pfrrptcodi)
        {
            return FactorySic.GetPfrRelacionIndisponibilidadRepository().GetByCriteria(pfrrptcodi);
        }

        #endregion

        #region Métodos Tabla PFR_REPORTE

        /// <summary>
        /// Inserta un registro de la tabla PFR_REPORTE
        /// </summary>        
        public void SavePfrReporte(PfrReporteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfrReporteRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_REPORTE
        /// </summary>
        public void UpdatePfrReporte(PfrReporteDTO entity)
        {
            try
            {
                FactorySic.GetPfrReporteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_REPORTE
        /// </summary>
        public void DeletePfrReporte(int pfrrptcodi)
        {
            try
            {
                FactorySic.GetPfrReporteRepository().Delete(pfrrptcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_REPORTE
        /// </summary>
        public PfrReporteDTO GetByIdPfrReporte(int pfrrptcodi)
        {
            return FactorySic.GetPfrReporteRepository().GetById(pfrrptcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_REPORTE
        /// </summary>
        public List<PfrReporteDTO> ListPfrReportes()
        {
            return FactorySic.GetPfrReporteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PFR_REPORTE
        /// </summary>
        public List<PfrReporteDTO> GetByCriteriaPfrReportes(int pfrreccodi, int pfrcuacodi)
        {
            return FactorySic.GetPfrReporteRepository().GetByCriteria(pfrreccodi, pfrcuacodi);
        }

        #endregion

        #region Métodos Tabla PFR_REPORTE_TOTAL

        /// <summary>
        /// Inserta un registro de la tabla PFR_REPORTE_TOTAL
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public void SavePfrReporteTotal(PfrReporteTotalDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfrReporteTotalRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_REPORTE_TOTAL
        /// </summary>
        public void UpdatePfrReporteTotal(PfrReporteTotalDTO entity)
        {
            try
            {
                FactorySic.GetPfrReporteTotalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_REPORTE_TOTAL
        /// </summary>
        public void DeletePfrReporteTotal(int pfrtotcodi)
        {
            try
            {
                FactorySic.GetPfrReporteTotalRepository().Delete(pfrtotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_REPORTE_TOTAL
        /// </summary>
        public PfrReporteTotalDTO GetByIdPfrReporteTotal(int pfrtotcodi)
        {
            return FactorySic.GetPfrReporteTotalRepository().GetById(pfrtotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_REPORTE_TOTAL
        /// </summary>
        public List<PfrReporteTotalDTO> ListPfrReporteTotals()
        {
            return FactorySic.GetPfrReporteTotalRepository().List();
        }

        /// <summary>
        /// Obtener reporte total por reportecodi
        /// </summary>
        /// <param name="pfrrptcodi"></param>
        /// <returns></returns>
        public List<PfrReporteTotalDTO> ListPfrReporteTotalByReportecodi(int pfrrptcodi)
        {
            return FactorySic.GetPfrReporteTotalRepository().ListByReportecodi(pfrrptcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrReporteTotal
        /// </summary>
        public List<PfrReporteTotalDTO> GetByCriteriaPfrReporteTotals()
        {
            return FactorySic.GetPfrReporteTotalRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PFR_RESULTADOS_GAMS

        /// <summary>
        /// Inserta un registro de la tabla PFR_RESULTADOS_GAMS
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public void SavePfrResultadosGams(PfrResultadosGamsDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfrResultadosGamsRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_RESULTADOS_GAMS
        /// </summary>
        public void UpdatePfrResultadosGams(PfrResultadosGamsDTO entity)
        {
            try
            {
                FactorySic.GetPfrResultadosGamsRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_RESULTADOS_GAMS
        /// </summary>
        public void DeletePfrResultadosGams(int pfrrgcodi)
        {
            try
            {
                FactorySic.GetPfrResultadosGamsRepository().Delete(pfrrgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_RESULTADOS_GAMS
        /// </summary>
        public PfrResultadosGamsDTO GetByIdPfrResultadosGams(int pfrrgcodi)
        {
            return FactorySic.GetPfrResultadosGamsRepository().GetById(pfrrgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_RESULTADOS_GAMS
        /// </summary>
        public List<PfrResultadosGamsDTO> ListPfrResultadosGamss()
        {
            return FactorySic.GetPfrResultadosGamsRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros por tipo de salida y escenario especifico
        /// </summary>
        /// <param name="pfresccodi"></param>
        /// <param name="pfrrgtipo"></param>
        /// <returns></returns>
        public List<PfrResultadosGamsDTO> ListPfrResultadosGamsByTipoYEscenario(int pfresccodi, int pfrrgtipo)
        {
            return FactorySic.GetPfrResultadosGamsRepository().ListByTipoYEscenario(pfresccodi, pfrrgtipo);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrResultadosGams
        /// </summary>
        public List<PfrResultadosGamsDTO> GetByCriteriaPfrResultadosGamss()
        {
            return FactorySic.GetPfrResultadosGamsRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PFR_ENTIDAD

        /// <summary>
        /// Inserta un registro de la tabla PFR_ENTIDAD
        /// </summary>
        public int SavePfrEntidad(PfrEntidadDTO entity)
        {
            try
            {
                return FactorySic.GetPfrEntidadRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_ENTIDAD
        /// </summary>
        public void UpdatePfrEntidad(PfrEntidadDTO entity)
        {
            try
            {
                FactorySic.GetPfrEntidadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_ENTIDAD
        /// </summary>
        public void DeletePfrEntidad(int pfrentcodi)
        {
            try
            {
                FactorySic.GetPfrEntidadRepository().Delete(pfrentcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_ENTIDAD
        /// </summary>
        public PfrEntidadDTO GetByIdPfrEntidad(int pfrentcodi)
        {
            return FactorySic.GetPfrEntidadRepository().GetById(pfrentcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_ENTIDAD
        /// </summary>
        public List<PfrEntidadDTO> ListPfrEntidads()
        {
            return FactorySic.GetPfrEntidadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrEntidad
        /// </summary>
        public List<PfrEntidadDTO> GetByCriteriaPfrEntidads(int pfrcatcodi, string pfrentcodi = ConstantesPotenciaFirmeRemunerable.ParametroXDefecto, int pfrentestado = (int)ConstantesPotenciaFirmeRemunerable.Estado.Defecto)
        {
            return FactorySic.GetPfrEntidadRepository().GetByCriteria(pfrcatcodi, pfrentcodi, pfrentestado);
        }

        #endregion

        #region Métodos Tabla PFR_ENTIDAD_DAT

        /// <summary>
        /// Inserta un registro de la tabla PFR_ENTIDAD_DAT
        /// </summary>
        public void SavePfrEntidadDat(PfrEntidadDatDTO entity)
        {
            try
            {
                FactorySic.GetPfrEntidadDatRepository().Save(entity);

                ActualizarFechaModifEntidad(entity.Pfrentcodi, entity.Pfrdatusucreacion, entity.Pfrdatfeccreacion.Value);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_ENTIDAD_DAT
        /// </summary>
        public void UpdatePfrEntidadDat(PfrEntidadDatDTO entity)
        {
            try
            {
                DateTime fechaAct = DateTime.Now;

                if (entity.Pfrdatdeleted2 > 0)//eliminar lógicamente
                {
                    DarDeBajaPfrEntidadDat(entity.Pfrentcodi, entity.Pfrcnpcodi, entity.Prfdatfechavig, entity.Pfrdatusumodificacion);
                }
                else
                {
                    List<PfrEntidadDatDTO> listaDat = GetByCriteriaPfrEntidadDats(entity.Pfrentcodi, entity.Pfrcnpcodi);
                    List<PfrEntidadDatDTO> listaHistXCnp = listaDat.Where(x => x.Prfdatfechavig == entity.Prfdatfechavig).ToList();

                    //obtener activo
                    PfrEntidadDatDTO regActivoHist = listaHistXCnp.Find(x => x.Pfrdatdeleted == 0);

                    if (regActivoHist != null)
                    {
                        bool existeDif = (regActivoHist.Pfrdatvalor ?? "").Trim() != (entity.Pfrdatvalor ?? "").Trim();
                        if (existeDif)
                        {
                            PfrEntidadDatDTO regUpdate = (PfrEntidadDatDTO)regActivoHist.Clone();

                            //el registro que ya está en bd, se le genera una copia y esa se guarda como eliminado para tener la historia de los cambios
                            regActivoHist.Pfrdatdeleted = listaHistXCnp.Max(x => x.Pfrdatdeleted) + 1;
                            SavePfrEntidadDat(regActivoHist);

                            //
                            regUpdate.Pfrdatvalor = (entity.Pfrdatvalor ?? "").Trim();
                            regUpdate.Pfrdatfecmodificacion = entity.Pfrdatfecmodificacion;
                            regUpdate.Pfrdatusumodificacion = entity.Pfrdatusumodificacion;
                            regUpdate.Pfrdatdeleted = 0;
                            regUpdate.Pfrdatdeleted2 = 0;
                            FactorySic.GetPfrEntidadDatRepository().Update(entity);

                            ActualizarFechaModifEntidad(entity.Pfrentcodi, entity.Pfrdatusumodificacion, fechaAct);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Dar de baja a grupodat
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="concepcodi"></param>
        /// <param name="fechaVigencia"></param>
        /// <param name="usuario"></param>
        public void DarDeBajaPfrEntidadDat(int pfrentcodi, int pfrcnpcodi, DateTime fechaVigencia, string usuario)
        {
            DateTime fechaAct = DateTime.Now;

            List<PfrEntidadDatDTO> listaDat = GetByCriteriaPfrEntidadDats(pfrentcodi, pfrcnpcodi);
            List<PfrEntidadDatDTO> listaHistXCnp = listaDat.Where(x => x.Prfdatfechavig == fechaVigencia).ToList();

            //obtener activo y luego cambiarle de estado
            PfrEntidadDatDTO regActivoHist = listaHistXCnp.Find(x => x.Pfrdatdeleted == 0);

            if (regActivoHist != null)
            {
                regActivoHist.Pfrdatfecmodificacion = fechaAct;
                if (!string.IsNullOrEmpty(usuario))
                    regActivoHist.Pfrdatusumodificacion = usuario;
                regActivoHist.Pfrdatdeleted = 0;
                regActivoHist.Pfrdatdeleted2 = listaHistXCnp.Max(x => x.Pfrdatdeleted) + 1;

                FactorySic.GetPfrEntidadDatRepository().Update(regActivoHist);

                ActualizarFechaModifEntidad(pfrentcodi, usuario, fechaAct);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_ENTIDAD_DAT
        /// </summary>
        public void DeletePfrEntidadDat(int pfrentcodi, int pfrcnpcodi, DateTime prfdatfechavig)
        {
            try
            {
                DarDeBajaPfrEntidadDat(pfrentcodi, pfrcnpcodi, prfdatfechavig, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_ENTIDAD_DAT
        /// </summary>
        public PfrEntidadDatDTO GetByIdPfrEntidadDat(int pfrentcodi, int pfrcnpcodi, DateTime prfdatfechavig, int pfrdatdeleted)
        {
            var reg = FactorySic.GetPfrEntidadDatRepository().GetById(pfrentcodi, pfrcnpcodi, prfdatfechavig, pfrdatdeleted);

            FormatearPfrEntidadDat(reg);

            return reg;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrEntidadDat
        /// </summary>
        public List<PfrEntidadDatDTO> GetByCriteriaPfrEntidadDats(int pfrentcodi, int pfrcnpcodi)
        {
            var lista = FactorySic.GetPfrEntidadDatRepository().GetByCriteria(pfrentcodi, pfrcnpcodi)
                                    .OrderByDescending(x => x.Prfdatfechavig).ThenBy(x => (x.Pfrdatdeleted == 0 ? 1 : 2)).ThenByDescending(x => x.Pfrdatfecmodificacion).ToList();

            foreach (var reg in lista)
                FormatearPfrEntidadDat(reg);

            return lista;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrEntidadDat
        /// </summary>
        public List<PfrEntidadDatDTO> ListarPfrentidadDatVigente(DateTime fechaVigencia, string pfrentcodis, int pfrcatcodi)
        {
            var lista = FactorySic.GetPfrEntidadDatRepository().ListarPfrentidadVigente(fechaVigencia, pfrentcodis, pfrcatcodi);

            foreach (var reg in lista)
                FormatearPfrEntidadDat(reg);

            return lista;
        }

        private void FormatearPfrEntidadDat(PfrEntidadDatDTO prop)
        {
            prop.Pfrcnpnomb = String.IsNullOrEmpty(prop.Pfrcnpnomb) ? string.Empty : prop.Pfrcnpnomb.Trim();
            prop.Pfrcatnomb = String.IsNullOrEmpty(prop.Pfrcatnomb) ? string.Empty : prop.Pfrcatnomb.Trim();

            prop.Prfdatfechavigdesc = prop.Prfdatfechavig.ToString(ConstantesAppServicio.FormatoFecha);

            prop.Pfrdatvalor = String.IsNullOrEmpty(prop.Pfrdatvalor) ? string.Empty : prop.Pfrdatvalor.Trim();

            var fechaCambio = prop.Pfrdatfecmodificacion != null ? prop.Pfrdatfecmodificacion.Value : prop.Pfrdatfeccreacion.Value;
            prop.FechaUltimaModif = fechaCambio.ToString(ConstantesAppServicio.FormatoFechaFull2);

            prop.UsuarioUltimaModif = prop.Pfrdatfecmodificacion != null ? prop.Pfrdatusumodificacion : prop.Pfrdatusucreacion;

            prop.EstadoDesc = prop.Pfrdatdeleted == 0 ? "Activo" : "Eliminado";
        }

        #endregion

        #region Métodos Tabla PFR_CONCEPTO

        /// <summary>
        /// Inserta un registro de la tabla PFR_CONCEPTO
        /// </summary>
        public void SavePfrConcepto(PfrConceptoDTO entity)
        {
            try
            {
                FactorySic.GetPfrConceptoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_CONCEPTO
        /// </summary>
        public void UpdatePfrConcepto(PfrConceptoDTO entity)
        {
            try
            {
                FactorySic.GetPfrConceptoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_CONCEPTO
        /// </summary>
        public void DeletePfrConcepto(int pfrcnpcodi)
        {
            try
            {
                FactorySic.GetPfrConceptoRepository().Delete(pfrcnpcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_CONCEPTO
        /// </summary>
        public PfrConceptoDTO GetByIdPfrConcepto(int pfrcnpcodi)
        {
            return FactorySic.GetPfrConceptoRepository().GetById(pfrcnpcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_CONCEPTO
        /// </summary>
        public List<PfrConceptoDTO> ListPfrConceptos()
        {
            return FactorySic.GetPfrConceptoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrConcepto
        /// </summary>
        public List<PfrConceptoDTO> GetByCriteriaPfrConceptos()
        {
            return FactorySic.GetPfrConceptoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PFR_TIPO

        /// <summary>
        /// Inserta un registro de la tabla PFR_TIPO
        /// </summary>
        public void SavePfrTipo(PfrTipoDTO entity)
        {
            try
            {
                FactorySic.GetPfrTipoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PFR_TIPO
        /// </summary>
        public void UpdatePfrTipo(PfrTipoDTO entity)
        {
            try
            {
                FactorySic.GetPfrTipoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PFR_TIPO
        /// </summary>
        public void DeletePfrTipo(int pfrcatcodi)
        {
            try
            {
                FactorySic.GetPfrTipoRepository().Delete(pfrcatcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PFR_TIPO
        /// </summary>
        public PfrTipoDTO GetByIdPfrTipo(int pfrcatcodi)
        {
            return FactorySic.GetPfrTipoRepository().GetById(pfrcatcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PFR_TIPO
        /// </summary>
        public List<PfrTipoDTO> ListPfrTipos()
        {
            return FactorySic.GetPfrTipoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfrTipo
        /// </summary>
        public List<PfrTipoDTO> GetByCriteriaPfrTipos()
        {
            return FactorySic.GetPfrTipoRepository().GetByCriteria();
        }

        #endregion

        #endregion

        #region PERIODOS Y RECALCULOS

        /// <summary>
        /// Devuelve el ultimo periodo vigente (mes anterior)
        /// </summary>
        /// <returns></returns>
        public DateTime GetPeriodoActual()
        {
            DateTime fechaActual = DateTime.Now;
            DateTime fechaPeriodo = new DateTime(fechaActual.Year, fechaActual.Month, 1);
            return fechaPeriodo.AddMonths(-1);
        }

        /// <summary>
        /// Devuelve una lista de años
        /// </summary>
        /// <param name="fechaPeriodoActual"></param>
        /// <returns></returns>
        public List<GenericoDTO> ListaAnio(DateTime fechaPeriodoActual)
        {
            int anioActual = fechaPeriodoActual.Year;

            List<GenericoDTO> listaAnio = new List<GenericoDTO>();
            int iAnioFinal = fechaPeriodoActual.Year;
            for (int i = 2017; i <= iAnioFinal; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = i.ToString();
                reg.String2 = anioActual == i ? "selected" : string.Empty;
                listaAnio.Add(reg);
            }

            return listaAnio.OrderByDescending(x => x.Entero1).ToList();
        }

        /// <summary>
        /// Verifica existencia y crea un periodo mensual, automaticamente
        /// </summary>
        public void CrearIndPeriodoAutomatico()
        {
            List<PfrPeriodoDTO> listaPeriodo = new List<PfrPeriodoDTO>();

            DateTime fechaPeriodoActual = GetPeriodoActual();

            //generar data historica
            DateTime fechaIni = GetPeriodoActual().AddYears(-3);
            do
            {
                int anio = fechaIni.Year;
                int mes = fechaIni.Month;

                PfrPeriodoDTO reg = new PfrPeriodoDTO();
                reg.Pfrperanio = anio;
                reg.Pfrpermes = mes;
                reg.Pfrpernombre = anio + "." + EPDate.f_NombreMes(mes);
                reg.Pfrperaniomes = Convert.ToInt32(anio.ToString() + mes.ToString("D2"));
                reg.Pfrperusucreacion = "SISTEMA";
                reg.Pfrperfeccreacion = DateTime.Now;

                listaPeriodo.Add(reg);

                fechaIni = fechaIni.AddMonths(1);

            } while (fechaIni <= fechaPeriodoActual);

            //verificar la existencia
            List<PfrPeriodoDTO> listaPeriodoBD = this.GetByCriteriaPfrPeriodos(-1);
            List<PfrPeriodoDTO> listaNuevo = new List<PfrPeriodoDTO>();
            foreach (var reg in listaPeriodo)
            {
                var regBD = listaPeriodoBD.Find(x => x.Pfrperaniomes == reg.Pfrperaniomes);
                if (regBD == null)
                {
                    listaNuevo.Add(reg);
                }
            }

            //guardar en bd
            listaNuevo.ForEach(reg => SavePfrPeriodo(reg));
        }

        /// <summary>
        /// Devuelve lista de meses
        /// </summary>
        /// <returns></returns>
        public List<GenericoDTO> ListaMes()
        {
            List<GenericoDTO> listaMes = new List<GenericoDTO>();
            for (int i = 1; i <= 12; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = EPDate.f_NombreMes(i);
                listaMes.Add(reg);
            }

            return listaMes;
        }

        /// <summary>
        /// Genera el listado de los periodos existentes
        /// </summary>
        /// <param name="url"></param>
        /// <param name="horizonte"></param>
        /// <param name="tienePermisoEditar"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoPeriodo(string url)
        {
            List<PfrPeriodoDTO> listaDataPeriodo = this.GetByCriteriaPfrPeriodos(-1).OrderByDescending(x => x.Pfrperaniomes).ToList();
            List<PfrRecalculoDTO> listaDataRecalculo = this.ListPfrRecalculos().OrderByDescending(x => x.Pfrreccodi).ToList();

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_periodo'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 60px;'>Opciones</th>");
            str.Append("<th style='width: 100px'>Periodo</th>");
            str.Append("<th style='width: 100px'>Año</th>");
            str.Append("<th style='width: 100px'>Mes</th>");

            str.Append("<th style='width: 120px; background: #9370DB;'>Estado</th>");
            str.Append("<th style='width: 150px; background: #9370DB;'>Último Recálculo</th>");
            str.Append("<th style='width: 450px; background: #9370DB;'>Comentario</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var reg in listaDataPeriodo)
            {
                str.Append("<tr>");
                str.Append("<td style='width: 60px;'>");
                str.AppendFormat("<a class='' href='JavaScript:verListadoRecalculo(" + reg.Pfrpercodi + ");' style='margin-right: 4px;'><img style='padding-left: 35px; margin-top: 3px; margin-bottom: 3px;' src='" + url + "Content/Images/btn-properties.png' alt='Ver listado de recalculo' title='Ver listado de recalculo' /></a>");
                str.Append("</td>");

                str.AppendFormat("<td class='' style='width: 100px; text-align: center'>{0}</td>", reg.Pfrpernombre);
                str.AppendFormat("<td class='' style='width: 100px; text-align: center'>{0}</td>", reg.Pfrperanio);
                str.AppendFormat("<td class='' style='width: 100px; text-align: center'>{0}</td>", reg.Pfrpermes);

                var regRecalculo = listaDataRecalculo.Find(x => x.Pfrpercodi == reg.Pfrpercodi);
                if (regRecalculo != null)
                {
                    string claseRec = regRecalculo != null && regRecalculo.Estado == ConstantesPotenciaFirmeRemunerable.Abierto ? "clase_recalculo_activo" : "";

                    str.AppendFormat("<td class='{1}' style='width: 120px; text-align: center'>{0}</td>", regRecalculo.PfrrecestadoDesc, claseRec);
                    str.AppendFormat("<td class='' style='width: 150px; text-align: center'>{0}</td>", regRecalculo.Pfrrecnombre);
                    str.AppendFormat("<td class='' style='width: 450px; text-align: left'>{0}</td>", regRecalculo.Pfrrecdescripcion);
                }
                else
                {
                    str.AppendFormat("<td class='' style=''>{0}</td>", string.Empty);
                    str.AppendFormat("<td class='' style=''>{0}</td>", string.Empty);
                    str.AppendFormat("<td class='' style=''>{0}</td>", string.Empty);
                }

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Genera el listado de las revisiones
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tienePermisoEditar"></param>
        /// <param name="pfPericodi"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoRecalculo(string url, bool tienePermisoEditar, int pfrPercodi, out string ultimoTipoRecalculo, out bool tieneReportePfr)
        {
            List<PfrRecalculoDTO> listaDataRecalculo = this.GetByCriteriaPfrRecalculos(pfrPercodi).OrderByDescending(x => x.Pfrreccodi).ToList();
            ultimoTipoRecalculo = listaDataRecalculo.Any() ? listaDataRecalculo.First().Pfrrectipo : string.Empty;

            List<PfrReporteDTO> lstReportes = ListPfrReportes();

            var ultimoRecalculo = listaDataRecalculo.Any() ? listaDataRecalculo.First() : null;
            tieneReportePfr = false;
            if (ultimoRecalculo != null)
                tieneReportePfr = lstReportes.Find(c => c.Pfrreccodi == ultimoRecalculo.Pfrreccodi) != null;

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_recalculo'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 120px'>Opciones</th>");
            str.Append("<th style='width: 150px'>Estado</th>");
            str.Append("<th style='width: 150px'>Nombre</th>");
            str.Append("<th style='width: 460px'>Comentario</th>");
            str.Append("<th style='width: 460px'>Informe</th>");
            str.Append("<th style='width: 220px'>Usuario últ. modif.</th>");
            str.Append("<th style='width: 220px'>Fecha últ. modif.</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var regRecalculo in listaDataRecalculo)
            {
                string claseRec = regRecalculo.Estado == ConstantesPotenciaFirmeRemunerable.Abierto ? "clase_recalculo_activo" : "";

                var recacodi = regRecalculo.Pfrreccodi;
                var reporte = lstReportes.Find(c => c.Pfrreccodi == recacodi);
                str.Append("<tr>");
                str.Append("<td style='width: 120px'>");
                str.AppendFormat("<a class='' href='JavaScript:verRecalculo({0});' style='margin-right: 4px;'><img style='padding-left: 5px; margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-open.png' alt='Ver recálculo' title='Ver recálculo' /></a>", regRecalculo.Pfrreccodi, url);
                if (tienePermisoEditar)
                    str.AppendFormat("<a class='' href='JavaScript:editarRecalculo({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-edit.png' alt='Editar recálculo' title='Editar recálculo' /></a>", regRecalculo.Pfrreccodi, url);

                //Puede generar calculos de PFR hasta que sea Cerrado
                if (regRecalculo.Estado != "C")
                    str.AppendFormat("<a class='' href='JavaScript:ObtenerInsumosParaCalculoPFR({0},{1});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{2}Content/Images/settings.png' alt='Calcular Potencia Firme Remunerable' title='Calcular Potencia Firme Remunerable' /></a>", pfrPercodi, regRecalculo.Pfrreccodi, url);

                //Muestra la lista de excel cuando tenga como minimo un reportecodi
                if (reporte != null)
                    str.AppendFormat("<a class='' href='JavaScript:verListadoReporte({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/excel.png' alt='Ver listado reporte' title='Ver listado reporte' /></a>", regRecalculo.Pfrreccodi, url);


                str.Append("</td>");
                str.AppendFormat("<td class='{1}' style='width: 150px; text-align: center'>{0}</td>", regRecalculo.PfrrecestadoDesc, claseRec);
                str.AppendFormat("<td class='' style='width: 150px; text-align: center'>{0}</td>", regRecalculo.Pfrrecnombre);
                str.AppendFormat("<td class='' style='width: 460px'>{0}</td>", regRecalculo.Pfrrecdescripcion);
                str.AppendFormat("<td class='' style='width: 460px'>{0}</td>", regRecalculo.Pfrrecinforme);
                str.AppendFormat("<td class='' style='width: 220px; text-align: center'>{0}</td>", regRecalculo.UltimaModificacionUsuarioDesc);
                str.AppendFormat("<td class='' style='width: 220px; text-align: center'>{0}</td>", regRecalculo.UltimaModificacionFechaDesc);
                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Verifica la existencia de un recalculo con el mismo nombre
        /// </summary>
        /// <param name="recalculo"></param>
        /// <returns></returns>
        public string ValidarNombreRepetido(PfrRecalculoDTO recalculo)
        {
            string mensaje = "";
            var listaRecalculo = this.ListPfrRecalculos().Where(x => x.Pfrpercodi == recalculo.Pfrpercodi).ToList();
            var regExist = listaRecalculo.Find(x => x.Pfrrecnombre.Trim().ToUpper() == recalculo.Pfrrecnombre.Trim().ToUpper());

            if (regExist != null)
                mensaje = "Ya existe un Recálculo con el mismo nombre";

            return mensaje;
        }

        /// <summary>
        /// Guarda recalculos
        /// </summary>
        /// <param name="recalculo"></param>
        /// <param name="usuario"></param>
        public void GuardarRecalculo(PfrRecalculoDTO recalculo, string usuario)
        {
            //guardar recálculo nuevo 
            recalculo.Pfrrecusucreacion = usuario;
            recalculo.Pfrrecfeccreacion = DateTime.Now;
            int recalculoId = this.SavePfrRecalculo(recalculo);
        }

        /// <summary>
        /// Obtener mes anterior
        /// </summary>
        /// <param name="pfrpercodi"></param>
        /// <returns></returns>
        public DateTime ObtenerMesAnterior(int pfrpercodi)
        {
            PfrPeriodoDTO objPfrPeriodoActual = GetByIdPfrPeriodo(pfrpercodi);
            DateTime mesActual = new DateTime(objPfrPeriodoActual.Pfrperanio, objPfrPeriodoActual.Pfrpermes, 1);
            DateTime mesAnterior = mesActual.AddMonths(-1);

            return mesAnterior;
        }

        /// <summary>
        /// Devuelve la fehca completa del ultimo minuto de cierto periodo mensual
        /// </summary>
        /// <param name="pfrPeriodo"></param>
        /// <returns></returns>
        public DateTime ObtenerUltimoDiaPeriodo(PfrPeriodoDTO pfrPeriodo)
        {
            DateTime fechaF = new DateTime(pfrPeriodo.Pfrperanio, pfrPeriodo.Pfrpermes, 1).AddMonths(1).AddDays(-1);
            DateTime fechaLimite = new DateTime(fechaF.Year, fechaF.Month, fechaF.Day, 23, 59, 59);
            return fechaLimite;
        }

        public DateTime ObtenerPrimerDiaPeriodo(PfrPeriodoDTO pfrPeriodo)
        {
            DateTime fechaF = new DateTime(pfrPeriodo.Pfrperanio, pfrPeriodo.Pfrpermes, 1);
            DateTime fechaIni = new DateTime(fechaF.Year, fechaF.Month, fechaF.Day, 00, 00, 00);
            return fechaIni;
        }

        #endregion

        #region Cálculo Webservice GAMS

        #region Insumo

        #region 01. Costos Variables

        /// <summary>
        /// Devuelve el valor de CV segun el grupocodi
        /// </summary>
        /// <param name="ListaData"></param>
        /// <param name="grupocodi"></param>
        /// <param name="famcodi"></param>
        /// <param name="fecha"></param>
        /// <param name="lstGrupo"></param>
        /// <param name="valorCAActual"></param>
        /// <returns></returns>
        private decimal? ObtenerCVSegunTipo(List<PrCvariablesDTO> ListaData, int equipadre, int? grupocodi, int famcodi, DateTime fechaMD
            , List<PrGrupoDTO> lstGrupo, PrGrupodatDTO valorCAActual, List<PfDispcalorutilDTO> listaDispCU
                                            , List<PrGrupodatDTO> listaCVGrupodat)
        {
            decimal? valor = null;
            string esCoGeneracion = "";
            string esRER = "";
            PrCvariablesDTO regCV = null;

            //si el usuario SME ingreso informacion en el parámetro entonces este valor usará el aplicativo
            var regCVDat = listaCVGrupodat.Find(x => x.Equipadre == equipadre);
            if (regCVDat != null)
            {
                return regCVDat.ValorDecimal;
            }

            //Para el caso de Cogeneradores y Centrales RER se considera como costo variable igual a cero 
            //(en caso de las Cogeneradoras, si esta central se encuentra generando sin calor útil el costo variable considerado será el día de máxima demanda).
            if (grupocodi != null)
            {
                PrGrupoDTO objGrupo = lstGrupo.Find(z => z.Grupocodi == grupocodi);
                esCoGeneracion = objGrupo.Grupotipocogen;
                esRER = objGrupo.TipoGenerRer;
                if (esCoGeneracion == "S") esRER = "N"; //las centrales de cogeneracion no seran consideradas rer
                regCV = GetCostoVariable(ListaData, grupocodi.Value, fechaMD);
            }

            //Para RER (eolicos, solares y algunas hidraulicas)
            if (esRER == "S")
            {
                return 0;
            }

            //Para Cogeneracion (algunos termicos)
            if (esCoGeneracion == "S")
            {
                //el caso especial es CT Oquendo
                PfDispcalorutilDTO regCu = listaDispCU.Find(x => x.Equipadre == equipadre);
                if (regCu != null && regCu.Pfcutienedisp == 0)
                {
                    //por defecto las centrales excepto Oquendo operan con Calor Util, su CV es según bd
                }
                else
                {
                    //RE-19799: el valor ya no va a ser 0, se cambia al valor en bd de costos variables
                    //return 0; //paramonga,san jacinto
                }
            }

            //para las Hidro restantes
            if (famcodi == ConstantesHorasOperacion.IdTipoHidraulica || famcodi == ConstantesHorasOperacion.IdGeneradorHidroelectrico)
            {
                return Convert.ToDecimal(valorCAActual.Formuladat);
            }

            //para las Termo restantes
            if (famcodi == ConstantesHorasOperacion.IdTipoTermica || famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico)
            {
                valor = regCV != null ? regCV.Cvc + regCV.Cvnc : null;
            }

            return valor;
        }

        /// <summary>
        /// Obtener costo variable de un modo de operacion en un día determinado
        /// </summary>
        /// <param name="ListaData"></param>
        /// <param name="grupocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private PrCvariablesDTO GetCostoVariable(List<PrCvariablesDTO> listaData, int grupocodi, DateTime fechaMD)
        {
            PrCvariablesDTO regAntMD = listaData.Where(x => x.Grupocodi == grupocodi && x.Repfecha.Date <= fechaMD.Date)
                .OrderByDescending(x => x.Repfecha).FirstOrDefault();

            PrCvariablesDTO regDespMD = listaData.Where(x => x.Grupocodi == grupocodi && x.Repfecha.Date > fechaMD.Date)
                .OrderBy(x => x.Repfecha).FirstOrDefault();

            //si solo existe un costo variable
            if (regAntMD != null && regDespMD == null)
                return regAntMD;
            if (regAntMD == null && regDespMD != null)
                return regDespMD; //ingreso de nueva central térmica

            //si existe dos costos variables entonces elegir el vigente (anterior o igual a la md)
            if (regAntMD != null && regDespMD != null)
            {
                return regAntMD;
            }

            return null;
        }

        #endregion

        #region 02. Barras


        /// <summary>
        /// Lista los cálculos para pestaña Carga
        /// </summary>
        /// <param name="regPeriodo"></param>
        /// <param name="regReporte"></param>
        /// <param name="lstBarraSSAA"></param>
        /// <param name="listaRelacion"></param>
        /// <param name="listaPeajeEgresoMinfo"></param>
        /// <param name="listaRetiroSinContrato"></param>
        /// <returns></returns>
        public List<BarraSuministro> ListarCalculoBarra(PfrPeriodoDTO regPeriodo, DateTime? pfrrptfecmd, int recpotcodi
                    , out List<BarraSSAA> lstBarraSSAA, out List<PfrEntidadDTO> listaRelacion
                    , out List<VtpPeajeEgresoMinfoDTO> listaPeajeEgresoMinfo, out List<VtpRetiroPotescDTO> listaRetiroSinContrato)
        {
            //obtener relación VTP GAMS
            var fechaVigencia = regPeriodo.FechaFin.ToString(ConstantesBase.FormatoFecha);
            listaRelacion = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.GamsVtp, regPeriodo.FechaIni, regPeriodo.FechaFin);

            //cálculo código de periodo y recálculo en VTP PEAJES
            List<PeriodoDTO> listaPeriodos = _periodoAppServicio.ListPeriodo();
            int codPeriodo = 0, codRecalculo = 0;

            //Obtenemos el periodo de VTP
            PeriodoDTO periodoTransferencia = listaPeriodos.Find(x => x.AnioCodi == regPeriodo.Pfrperanio && x.MesCodi == regPeriodo.Pfrpermes);
            codPeriodo = periodoTransferencia.PeriCodi;
            codRecalculo = recpotcodi;

            //Obtener información de VTP PEAJES
            listaPeajeEgresoMinfo = _transfAppService.GetByCriteriaVtpPeajeEgresoMinfoVista(codPeriodo, codRecalculo, 0, 0, 0, 0, "*", "*", "*", "*");
            listaRetiroSinContrato = _transfAppService.ListVtpRetiroPotenciaSCView(codPeriodo, codRecalculo);

            //agrupar 
            List<Tuple<string, decimal?>> listaBarra = new List<Tuple<string, decimal?>>();
            foreach (var item in listaPeajeEgresoMinfo)
            {
                listaBarra.Add(new Tuple<string, decimal?>(item.Barrnombre, (item.Pegrmipoteegreso / 1000)));
            }
            foreach (var item in listaRetiroSinContrato)
            {
                listaBarra.Add(new Tuple<string, decimal?>(item.Barrnombre, (item.Rpscpoteegreso / 1000)));
            }

            // cálculos
            List<BarraSuministro> lstProgramaciones = new List<BarraSuministro>();
            foreach (var item in listaBarra)
            {
                BarraSuministro barraItem = new BarraSuministro();
                barraItem.NombreBarraGams = item.Item1;

                var nombreGams = listaRelacion.Find(x => x.Pfrentunidadnomb == item.Item1);
                barraItem.NombreGams = nombreGams != null ? nombreGams.Idbarra1desc : String.Empty;
                barraItem.IdGams = nombreGams != null ? nombreGams.Idbarra1 : "0";
                barraItem.CodigoGams = nombreGams != null ? nombreGams.Pfrentcodibarragams.GetValueOrDefault(0) : 0;
                barraItem.Faltante = barraItem.NombreGams == String.Empty ? "BARRA" : "";

                if (barraItem.IdGams == "B0007")
                { }
                if (barraItem.CodigoGams == 644)
                { }

                barraItem.Pload = item.Item2;
                decimal pload3 = (decimal)Math.Pow((double)barraItem.Pload, 3);
                decimal pload2 = (decimal)Math.Pow((double)barraItem.Pload, 2);
                decimal operacion = (decimal)(0.0000000004 * (double)pload3 - 0.0000005 * (double)pload2 + 0.0002 * (double)barraItem.Pload + 0.9403);
                barraItem.Fp = (double)operacion < 0.95 ? (decimal)0.95 : operacion;

                barraItem.Qload = (decimal)((double)barraItem.Pload * (Math.Tan(Math.Acos((double)barraItem.Fp))));

                lstProgramaciones.Add(barraItem);
            }

            //SSAA
            var listaRelacionSSAA = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.GamsSsaa, regPeriodo.FechaIni, regPeriodo.FechaFin);

            //obtener data de Medidores Generación
            //var fechaInicio = regPeriodo.FechaIni.ToString(ConstantesBase.FormatoFecha);
            //var fechaFin = regPeriodo.FechaFin.ToString(ConstantesBase.FormatoFecha);
            // 3 servicios auxiliares
            // nro página
            // 200 

            //regReporte.Pfrrptfecmd?.ToString("dd MMMM yyyy HH:mm");  fecha de maxima demanda
            var listaDatosMd = _consultaMedidoresAppServicio.ObtenerConsultaMedidores(pfrrptfecmd.Value.Date, pfrrptfecmd.Value.Date, ConstantesPotenciaFirmeRemunerable.ParametroDefecto.ToString(), ConstantesPotenciaFirmeRemunerable.ParametroDefecto.ToString(),
              ConstantesPotenciaFirmeRemunerable.ParametroDefecto.ToString(), 1, 3,
                1, 10000, out List<MeMedicion96DTO> sumatoria);

            lstBarraSSAA = new List<BarraSSAA>();
            foreach (var itemAgrupado in listaRelacionSSAA.GroupBy(x => x.Pfrentcodibarragams.Value))
            {
                BarraSSAA barraSSAA = new BarraSSAA();
                var equipoGams = itemAgrupado.First();

                barraSSAA.Codigo = equipoGams.Pfrentcodibarragams.GetValueOrDefault(0);
                barraSSAA.IdBGams = equipoGams.Idbarra1;
                barraSSAA.Ssaa = 0;

                if (barraSSAA.IdBGams == "B0007")
                { }

                foreach (var item in itemAgrupado)
                {
                    var entidad = listaDatosMd.Find(x => x.Equicodi == item.Equicodi);
                    if (entidad != null)
                    {
                        var numero = (pfrrptfecmd.Value.Hour * 60 + pfrrptfecmd.Value.Minute) / 15;
                        var valor = entidad.GetType().GetProperty("H" + numero.ToString()).GetValue(entidad, null);
                        if (valor != null)
                        {
                            barraSSAA.Ssaa += Convert.ToDecimal(valor);
                        }
                    }
                }

                lstBarraSSAA.Add(barraSSAA);
            }

            return lstProgramaciones;
        }

        #endregion

        private List<PfrEntidadDTO> ObtenerListadoEnlaces(List<PfrEntidadDTO> listaLineas, List<PfrEntidadDTO> listaTrafo2, List<PfrEntidadDTO> listaTrafo3)
        {
            List<PfrEntidadDTO> listaTotal = new List<PfrEntidadDTO>();

            foreach (var linea in listaLineas)
            {
                linea.Tap1 = 1;
                linea.Tap2 = 1;
            }

            listaTotal.AddRange(listaLineas);
            listaTotal.AddRange(listaTrafo2);
            listaTotal.AddRange(listaTrafo3);

            return listaTotal;
        }

        /// <summary>
        /// Obtener lista escenarios por periodo (desde PF) 
        /// </summary>
        /// <param name="objPfrPeriodo"></param>
        /// <returns></returns>
        private List<PfrEscenarioDTO> ObtenerEscenariosXPeriodoPFR(PfrPeriodoDTO objPfrPeriodo)
        {
            List<PfrEscenarioDTO> lstEscenariosPFR = new List<PfrEscenarioDTO>();
            int perianio = objPfrPeriodo.Pfrperanio;
            int perimes = objPfrPeriodo.Pfrpermes;
            DateTime fechaIni = new DateTime(perianio, perimes, 1);
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);
            List<PfEscenarioDTO> lstEscenariosPF = pfAppServicio.ObtenerEscenariosPF(perianio, perimes, fechaIni, fechaFin).OrderBy(x => x.Pfescefecini).ToList();
            int i = 1;
            foreach (var objEscenarioPF in lstEscenariosPF)
            {
                //Por elmomento solo ingresamos dos campos
                PfrEscenarioDTO objEscPFR = new PfrEscenarioDTO();
                objEscPFR.Pfrescfecini = objEscenarioPF.Pfescefecini;
                objEscPFR.Pfrescfecfin = objEscenarioPF.Pfescefecfin;
                objEscPFR.Pfrescdescripcion = objEscenarioPF.Pfescedescripcion;
                objEscPFR.Numero = i;
                i++;

                lstEscenariosPFR.Add(objEscPFR);
            }

            return lstEscenariosPFR;
        }


        /// <summary>
        /// Devuelve la lista de reporte_total del reporte AUX2 (para guardarlo en BD)
        /// </summary>
        /// <param name="listaPF"></param>
        /// <param name="escenarioPFR"></param>
        /// <param name="listaReptotFK"></param>
        /// <param name="listaReptotCRT"></param>
        /// <param name="listaReptotCRH"></param>
        /// <returns></returns>
        private List<PfrReporteTotalDTO> ObtenerReporteAux2(List<PfReporteTotalDTO> listaPF, List<IndReporteTotalDTO> listaReptotFK, List<IndReporteTotalDTO> listaReptotCRT,
                                                                List<IndReporteTotalDTO> listaReptotCRH, List<PfrResultadosGamsDTO> lstResultadosPG, List<PfrEntidadDTO> listaRelacionGE,
                                                                decimal? maxDemanda, decimal? factorReservaFirme, out decimal? FRFR)
        {
            List<PfrReporteTotalDTO> lstRT_PFR = new List<PfrReporteTotalDTO>();
            int? valorNulo = null;

            foreach (var reg in listaPF)
            {
                PfrReporteTotalDTO regRepTot = new PfrReporteTotalDTO();

                var grupocodiReg = reg.Grupocodi;
                var equicodiReg = reg.Equicodi;
                var famcodiReg = reg.Famcodi;

                regRepTot.Emprcodi = reg.Emprcodi;
                regRepTot.Equipadre = reg.Equipadre;
                regRepTot.Equicodi = equicodiReg;
                regRepTot.Grupocodi = grupocodiReg;
                regRepTot.Famcodi = famcodiReg;
                regRepTot.Central = reg.Central;
                regRepTot.Pfrtotunidadnomb = reg.Pftotunidadnomb;
                regRepTot.Pfrtotficticio = 0;

                //calculo PF

                regRepTot.Pfrtotpf = ConvertirMWaKw(reg.Pftotpf);

                //calculo de FK                
                decimal? valorFK = ObtenerFactorKPorUnidad(listaReptotFK, regRepTot.Famcodi, grupocodiReg, equicodiReg);
                regRepTot.Pfrtotfkmesant = valorFK;

                //Calculo de CR
                int? valCR = ObtenerValorCRPorUnidad(listaReptotCRT, listaReptotCRH, grupocodiReg, equicodiReg, famcodiReg);
                regRepTot.Pfrtotcrmesant = valCR;

                //Calculo de Potencia Disponible
                decimal? valorPotDisponible = valorNulo;
                var FRF = factorReservaFirme;
                var regFK = regRepTot.Pfrtotfkmesant;
                var regPF = regRepTot.Pfrtotpf;

                if (regFK != null)
                {
                    if (regFK == 1) // PF/FRF                  
                        valorPotDisponible = regPF / FRF;
                    else
                    {
                        valorPotDisponible = (regPF / FRF) * regFK;
                        regRepTot.RegistroDuplicadoAux2 = 1;
                        regRepTot.ValorPDParaDuplicadoAux2 = (regPF / FRF) - valorPotDisponible;
                    }
                }
                regRepTot.Pfrtotpd = valorPotDisponible;


                //Calculo de Potencia Disponible Despachada
                PfrEntidadDTO regRelacion = listaRelacionGE.Find(x => x.Pfrcatcodi == (int)ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos
                                                                    && x.Equicodi == regRepTot.Equicodi && x.Grupocodi == regRepTot.Grupocodi
                                                                    && x.Pfrentficticio.GetValueOrDefault(0) == regRepTot.Pfrtotficticio);
                var relaciongecodi = regRelacion != null ? regRelacion.Pfrentcodi : -1;
                PfrResultadosGamsDTO resultadoGams = lstResultadosPG.Find(c => c.Pfrrgecodi == relaciongecodi);
                regRepTot.Pfrtotpdd = resultadoGams != null ? ConvertirMWaKw(resultadoGams.Pfrrgresultado) : null;

                lstRT_PFR.Add(regRepTot);
            }

            //Calculo de FRFR 
            var sumaPDD = lstRT_PFR.Any() ? lstRT_PFR.Sum(n => n.Pfrtotpdd) : 0;
            FRFR = sumaPDD != null ? ((factorReservaFirme * sumaPDD) / maxDemanda) : null;  //= (FRF * Sum(PDD)) / MD

            //Calculo de Potencia Firme Remunerable
            foreach (var reg in lstRT_PFR)
            {
                //calculo campo PFR
                reg.Pfrtotpfr = reg.Pfrtotpdd != null ? reg.Pfrtotpdd * FRFR : null;
            }

            //Agregamos los Ficticios (los que presentaron FactorK != 1)
            List<PfrReporteTotalDTO> listaAdicional = new List<PfrReporteTotalDTO>();
            listaAdicional = lstRT_PFR.Where(x => x.RegistroDuplicadoAux2 == 1).ToList();

            foreach (var reg in listaAdicional)
            {
                PfrReporteTotalDTO regRepTotAdicional = new PfrReporteTotalDTO();

                var grupocodiReg = reg.Grupocodi;
                var equicodiReg = reg.Equicodi;
                var famcodiReg = reg.Famcodi;

                regRepTotAdicional.Emprcodi = reg.Emprcodi;
                regRepTotAdicional.Equipadre = reg.Equipadre;
                regRepTotAdicional.Equicodi = equicodiReg;
                regRepTotAdicional.Grupocodi = grupocodiReg;
                regRepTotAdicional.Famcodi = famcodiReg;
                regRepTotAdicional.Pfrtotficticio = 1;
                regRepTotAdicional.Pfrtotunidadnomb = reg.Pfrtotunidadnomb + "fk(*)";
                regRepTotAdicional.Pfrtotpd = reg.ValorPDParaDuplicadoAux2;

                lstRT_PFR.Add(regRepTotAdicional);
            }


            return lstRT_PFR;
        }

        /// <summary>
        /// Evalua el valor de CR para cada unidad (equicodi-grupocodi)
        /// </summary>
        /// <param name="listaReptotCRT"></param>
        /// <param name="listaReptotCRH"></param>
        /// <param name="grupocodiReg"></param>
        /// <param name="equicodiReg"></param>
        /// <param name="famcodiReg"></param>
        /// <returns></returns>
        private int? ObtenerValorCRPorUnidad(List<IndReporteTotalDTO> listaReptotCRT, List<IndReporteTotalDTO> listaReptotCRH, int? grupocodiReg, int? equicodiReg, int famcodiReg)
        {
            int? valorNulo = null;

            string valorCRT = "";
            string valorCRH = "";

            var lstCRTermicas = listaReptotCRT.Where(m => m.Grupocodi == grupocodiReg && m.Equicodi == equicodiReg).OrderByDescending(x => x.Itotcr).ToList();
            if (listaReptotCRT.Any() && lstCRTermicas.Count > 0) //si existe CRT (fortuita o programada)para cierto unidad
            {
                valorCRT = lstCRTermicas.First().Itotcr;
            }

            if (listaReptotCRH.Any() && listaReptotCRH.Find(m => m.Grupocodi == grupocodiReg && m.Equicodi == equicodiReg) != null)
            {
                valorCRH = listaReptotCRH.Find(m => m.Grupocodi == grupocodiReg && m.Equicodi == equicodiReg).Itotcr;
            }

            string crString = famcodiReg == ConstantesPotenciaFirmeRemunerable.FamcodiGeneradorTermo || famcodiReg == ConstantesPotenciaFirmeRemunerable.FamcodiCentralTermo ? valorCRT :
                                       (famcodiReg == ConstantesPotenciaFirmeRemunerable.FamcodiGeneradorHidro || famcodiReg == ConstantesPotenciaFirmeRemunerable.FamcodiCentralHidro ? valorCRH : null);
            int? valCR = crString == "" || crString == null ? valorNulo : (crString == "S" ? 1 : (crString == "N" ? 0 : valorNulo));

            return valCR;
        }

        /// <summary>
        /// Convierte MW a KW
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public decimal? ConvertirMWaKw(decimal? valor)
        {
            if (valor.HasValue)
            {
                valor = valor * 1000;
            }
            return valor;
        }

        /// <summary>
        /// Devuelve la lista de reporte_total del reporte AUX1 (para guardarlo en BD)
        /// </summary>
        /// <param name="listaPF"></param>
        /// <param name="escenarioPFR"></param>
        /// <param name="valorCAActual"></param>
        /// <param name="fechaDeMD"></param>
        /// <param name="valorMaximaDemanda"></param>
        /// <param name="valorMargenReserva"></param>
        /// <param name="valorcr"></param>
        /// <param name="listaReptotFK"></param>
        /// <param name="listaReptotCRT"></param>
        /// <param name="listaReptotCRH"></param>
        /// <param name="listDataCV"></param>
        /// <returns></returns>
        private List<PfrReporteTotalDTO> ObtenerReporteTotalAux1(List<PfReporteTotalDTO> listaPF, PrGrupodatDTO valorCAActual,
                                                                DateTime? fechaDeMD, decimal? valorMaximaDemanda, decimal? valorMargenReserva, decimal? valorcr, List<IndReporteTotalDTO> listaReptotCRT,
                                                                List<IndReporteTotalDTO> listaReptotCRH, List<PrCvariablesDTO> listDataCV, List<PfDispcalorutilDTO> listaDispCU, List<PrGrupodatDTO> listaCVGrupodat)
        {
            List<PfrReporteTotalDTO> listaPFRxEscena = new List<PfrReporteTotalDTO>();


            //Costos Variables            
            DateTime? fechaMD = fechaDeMD;
            List<PrGrupoDTO> lstGrupo = FactorySic.GetPrGrupoRepository().List();

            var reserva = (valorMargenReserva / 100) * valorMaximaDemanda ?? 0;
            var maxDemReserv = reserva + valorMaximaDemanda ?? 0;

            foreach (var reg in listaPF)
            {
                PfrReporteTotalDTO regRepTot = new PfrReporteTotalDTO();

                var grupocodiReg = reg.Grupocodi;
                var equipadreReg = reg.Equipadre;
                var equicodiReg = reg.Equicodi;
                var famcodiReg = reg.Famcodi;

                regRepTot.Emprcodi = reg.Emprcodi;
                regRepTot.Equipadre = equipadreReg;
                regRepTot.Equicodi = equicodiReg;
                regRepTot.Grupocodi = grupocodiReg;
                regRepTot.Famcodi = famcodiReg;
                regRepTot.Central = reg.Central;
                regRepTot.Pfrtotunidadnomb = reg.Pftotunidadnomb;

                var valorPe = ConvertirMWaKw(reg.Pftotpe);
                var valorPf = ConvertirMWaKw(reg.Pftotpf);
                regRepTot.Pfrtotpe = valorPe;
                regRepTot.Pfrtotpf = valorPf;

                if (reg.Pftotincremental == 1)
                {
                    grupocodiReg = reg.Grupocodi2;
                }

                //calculo de CV
                var valorCV = ObtenerCVSegunTipo(listDataCV, equipadreReg, grupocodiReg, famcodiReg, fechaMD.Value, lstGrupo, valorCAActual, listaDispCU, listaCVGrupodat);
                regRepTot.Pfrtotcv = valorCV;

                //Calculo de CR
                int? valCR = ObtenerValorCRPorUnidad(listaReptotCRT, listaReptotCRH, reg.Grupocodi, equicodiReg, famcodiReg);
                regRepTot.Pfrtotcrmesant = valCR;

                listaPFRxEscena.Add(regRepTot);
            }

            decimal? sumaAcumulada = 0;
            var listaPFRxEscenaOrd = listaPFRxEscena.OrderBy(x => x.Pfrtotcv).ThenBy(x => x.Central).ThenBy(x => x.Pfrtotunidadnomb).ToList();

            foreach (var reg in listaPFRxEscenaOrd)
            {
                //Acumulados
                var sumAnterior = sumaAcumulada;
                sumaAcumulada = reg.Pfrtotpe.GetValueOrDefault(0) + sumAnterior.GetValueOrDefault(0);
                reg.Pfrtotpea = sumaAcumulada;

                //Factor de Ingreso
                if (sumAnterior == 0)
                {
                    reg.Pfrtotfi = 1;
                }
                else
                {
                    var valor = (maxDemReserv - sumAnterior) / reg.Pfrtotpe;
                    var factorIngreso = sumaAcumulada <= maxDemReserv ? 1 : valor > 0 ? valor : 0;
                    reg.Pfrtotfi = factorIngreso;
                }

                //Potencia Firme Colocada
                var pfirmecolocada = reg.Pfrtotpf * reg.Pfrtotfi;
                reg.Pfrtotpfc = pfirmecolocada;

                //Costo Variable Flujo
                reg.Pfrtotcvf = reg.Pfrtotcrmesant == 1 ? Convert.ToDecimal(valorcr) / 1000 : reg.Pfrtotcv;

            }

            var sumaTotalColocada = listaPFRxEscena.Sum(x => x.Pfrtotpfc);
            var factorRFirme = sumaTotalColocada / valorMaximaDemanda ?? 0;

            foreach (var regRTotal in listaPFRxEscena)
            {
                //Potencia Disponible
                var pDisponible = regRTotal.Pfrtotpf / factorRFirme ?? 0;
                regRTotal.Pfrtotpd = pDisponible;
            }

            return listaPFRxEscena;
        }

        /// <summary>
        /// Devuelve la lista de reporte_total del reporte C8 (para guardarlo en BD)
        /// </summary>
        /// <param name="listaPF"></param>
        /// <returns></returns>
        private List<PfrReporteTotalDTO> ObtenerReporteC8(List<PfReporteTotalDTO> listaPF)
        {
            List<PfrReporteTotalDTO> lstRT_PFR = new List<PfrReporteTotalDTO>();

            foreach (var reg in listaPF)
            {
                PfrReporteTotalDTO regRepTot = new PfrReporteTotalDTO();

                var grupocodiReg = reg.Grupocodi;
                var equicodiReg = reg.Equicodi;
                var famcodiReg = reg.Famcodi;

                regRepTot.Emprcodi = reg.Emprcodi;
                regRepTot.Equipadre = reg.Equipadre;
                regRepTot.Equicodi = equicodiReg;
                regRepTot.Grupocodi = grupocodiReg;
                regRepTot.Famcodi = famcodiReg;
                regRepTot.Pfrtotunidadnomb = reg.Pftotunidadnomb;
                regRepTot.Pfrtotpe = ConvertirMWaKw(reg.Pftotpe);
                regRepTot.Pfrtotpf = ConvertirMWaKw(reg.Pftotpf);

                lstRT_PFR.Add(regRepTot);
            }

            return lstRT_PFR;
        }

        /// <summary>
        /// Devuelve la lista de reporte_total del reporte DATOS (para guardarlo en BD)
        /// </summary>
        /// <param name="listaPF"></param>
        /// <param name="escenarioPFR"></param>
        /// <param name="valorCAActual"></param>
        /// <param name="fechaDeMD"></param>
        /// <param name="listaReptotFK"></param>
        /// <param name="listaReptotCRT"></param>
        /// <param name="listaReptotCRH"></param>
        /// <param name="listDataCV"></param>
        /// <returns></returns>
        private List<PfrReporteTotalDTO> ObtenerReporteTotalDatos(List<PfReporteTotalDTO> listaPF, PrGrupodatDTO valorCAActual,
                                                                DateTime? fechaDeMD
            , List<IndReporteTotalDTO> listaReptotFK, List<IndReporteTotalDTO> listaReptotCRT
            , List<IndReporteTotalDTO> listaReptotCRH, List<PrCvariablesDTO> listDataCV, List<PfDispcalorutilDTO> listaDispCU, List<PrGrupodatDTO> listaCVGrupodat)
        {
            List<PfrReporteTotalDTO> lstRT_PFR = new List<PfrReporteTotalDTO>();


            //Costos Variables            
            DateTime? fechaMD = fechaDeMD;
            List<PrGrupoDTO> lstGrupo = FactorySic.GetPrGrupoRepository().List();

            //var rowEmpIni = rowData;
            foreach (var reg in listaPF)
            {
                PfrReporteTotalDTO regRepTot = new PfrReporteTotalDTO();

                var grupocodiReg = reg.Grupocodi;
                var equipadreReg = reg.Equipadre;
                var equicodiReg = reg.Equicodi;
                var famcodiReg = reg.Famcodi;

                regRepTot.Emprcodi = reg.Emprcodi;
                //regRepTot.Emprnomb = reg.Emprnomb;
                regRepTot.Equipadre = equipadreReg;
                //regRepTot.Central = reg.Central;
                regRepTot.Equicodi = equicodiReg;
                regRepTot.Grupocodi = grupocodiReg;
                regRepTot.Famcodi = famcodiReg;
                regRepTot.Pfrtotunidadnomb = reg.Pftotunidadnomb;
                regRepTot.Grupotipocogen = reg.Grupotipocogen;

                regRepTot.Pfrtotpe = reg.Pftotpe;
                regRepTot.Pfrtotpf = reg.Pftotpf;

                if (reg.Pftotincremental == 1)
                {
                    grupocodiReg = reg.Grupocodi2;
                }

                //calculo de CV
                var valorCV = ObtenerCVSegunTipo(listDataCV, equipadreReg, grupocodiReg, famcodiReg, fechaMD.Value, lstGrupo, valorCAActual, listaDispCU, listaCVGrupodat);
                regRepTot.Pfrtotcv = valorCV;

                //calculo de FK                
                decimal? valorFK = ObtenerFactorKPorUnidad(listaReptotFK, regRepTot.Famcodi, reg.Grupocodi, equicodiReg);
                regRepTot.Pfrtotfkmesant = valorFK;

                //Calculo de CR 
                int? valCR = ObtenerValorCRPorUnidad(listaReptotCRT, listaReptotCRH, reg.Grupocodi, equicodiReg, famcodiReg);
                regRepTot.Pfrtotcrmesant = valCR;

                lstRT_PFR.Add(regRepTot);
            }

            return lstRT_PFR;
        }

        /// <summary>
        /// Permite obtener factor k por unidad
        /// </summary>
        /// <param name="listaReptotFK"></param>
        /// <param name="regRepTot"></param>
        /// <param name="grupocodiReg"></param>
        /// <param name="equicodiReg"></param>
        /// <returns></returns>
        private decimal? ObtenerFactorKPorUnidad(List<IndReporteTotalDTO> listaReptotFK, int? famcodi, int? grupocodiReg, int? equicodiReg)
        {
            decimal? valorFK = null;

            if (famcodi == ConstantesPotenciaFirmeRemunerable.FamcodiGeneradorHidro || famcodi == ConstantesPotenciaFirmeRemunerable.FamcodiCentralHidro
                || famcodi == ConstantesPotenciaFirmeRemunerable.FamcodiGeneradorSolar || famcodi == ConstantesPotenciaFirmeRemunerable.FamcodiCentralSolar
                || famcodi == ConstantesPotenciaFirmeRemunerable.FamcodiGeneradorEolico || famcodi == ConstantesPotenciaFirmeRemunerable.FamcodiCentralEolico)
            {
                valorFK = 1;
            }
            else
            {
                if (listaReptotFK.Any() && listaReptotFK.Find(m => m.Grupocodi == grupocodiReg && m.Equicodi == equicodiReg) != null)
                {
                    valorFK = listaReptotFK.Find(m => m.Grupocodi == grupocodiReg && m.Equicodi == equicodiReg).Itotfactork;
                }
                //valorFK = listaReptotFK?.Find(m => m.Grupocodi == grupocodiReg && m.Equicodi == equicodiReg)?.Itotfactork;
            }

            return valorFK;
        }

        public void SetMaximaDemanda(PfrReporteDTO regReporte, DateTime fechaIni, DateTime fechaFin)
        {
            //Data de Medicion96 (30*220 registros)
            List<MeMedicion96DTO> listAllSinCruceHO = _medidoresAppServicio
                .ListaDataMDGeneracionConsolidado(fechaIni, fechaFin, ConstantesMedicion.IdTipogrupoCOES, ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false);

            //Data Generación (30 registros = 30 dias)
            List<MeMedicion96DTO> listaDemandaGenSinCruceHO = _medidoresAppServicio.ListaDataMDGeneracionFromConsolidado(fechaIni, fechaFin, listAllSinCruceHO);

            //Data Interconexion
            List<MeMedicion96DTO> listaInterconexion = _medidoresAppServicio.ListaDataMDInterconexion96(fechaIni, fechaFin); //para maxima demanda

            //combinación de Medidores e Interconexiones
            List<MeMedicion96DTO> listDemanda = _medidoresAppServicio.ListaDataMDTotalSEIN(listaDemandaGenSinCruceHO, listaInterconexion);

            MedicionReporteDTO umbrales = _medidoresAppServicio.ObtenerParametros(fechaIni, listDemanda, listaInterconexion);
            //Obtener día MD

            regReporte.Pfrrptfecmd = umbrales.HoraBloqueMaxima;
            regReporte.Pfrrptmd = umbrales.MaximaDemanda;
        }

        private List<PfrEntidadDTO> ObtenerDataGeneracionPorRango(int idreportePF, DateTime fechaIniRango, DateTime fechaFinRango, int pfrrptcodiDatos, int pfrrptcodiAux1)
        {
            List<PfrEntidadDTO> listaGeneracion = new List<PfrEntidadDTO>();

            //traer la PF de la BD
            List<PfReporteTotalDTO> listaBD = pfAppServicio.GetByCriteriaPfReporteTotals(idreportePF.ToString()).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Pftotunidadnomb).ToList();

            var listaEscenariopf = pfAppServicio.GetByCriteriaPfEscenarios(idreportePF).OrderBy(x => x.Pfescefecini).ToList();
            int i = 1;
            listaEscenariopf.ForEach(x => x.Numero = i++);

            //escenarios de aux1 para calcular FRF
            List<PfrEscenarioDTO> listaEscenarioAux1 = ListPfrEscenariosByReportecodi(pfrrptcodiAux1).OrderBy(x => x.Pfrescfecini).ToList();
            int j = 1;
            foreach (var reg in listaEscenarioAux1)
            {
                reg.Numero = j;
                j++;
            }
            //obtener número del escenario de pf
            var escenarioPf = listaEscenariopf.Where(x => x.Pfescefecini == fechaIniRango && x.Pfescefecfin == fechaFinRango).First();
            var numeroEscenario = escenarioPf.Numero;

            //obtiene FRF
            var escenarioPfr = listaEscenarioAux1.Find(x => x.Numero == numeroEscenario);
            var factorRFirme = escenarioPfr.Pfrescfrf ?? 0;

            //Obtener reporte datos y Ordenar por costo varible, central y unidad
            List<PestaniaDatos> listaDatos = ObtenerDataDatosPorRangoDeReporte(pfrrptcodiDatos, out List<PrGrupodatDTO> listaParametros);
            listaDatos = listaDatos.OrderBy(x => x.CV).ThenBy(x => x.Central).ThenBy(x => x.UnidadNombre).ToList();

            //Obtener Listado de relacion Gams con Unidades
            var listaRelacion = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos, fechaIniRango, fechaFinRango);
            listaRelacion.ForEach(x => x.Pfrentficticio = x.Pfrentficticio == 1 ? x.Pfrentficticio : 0);

            //Obtener Listado de barras activas para el periodo          
            List<PfrEntidadDTO> equiposBarras = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Barra, fechaIniRango, fechaFinRango);

            PfrReporteDTO regReporteDatos = GetByIdPfrReporte(pfrrptcodiDatos);

            foreach (var item in listaBD.Where(x => x.Pfescecodi == escenarioPf.Pfescecodi).ToList())
            {
                //PfrEntidadDTO objGeneracion = new PfrEntidadDTO();
                var listaGamsEq = item.Famcodi == ConstantesPotenciaFirmeRemunerable.FamcodiGeneradorTermo ? listaRelacion.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).ToList()
                                    : listaRelacion.Where(x => x.Equicodi == item.Equicodi).ToList();
                if (listaGamsEq.Count != 0)
                {
                    foreach (var relGamsEq in listaGamsEq)
                    {
                        //if (relGamsEq.Pfrentid == "T043" || relGamsEq.Pfrentid == "T061" || relGamsEq.Pfrentid == "T105" || relGamsEq.Pfrentid == "T150")
                        //{
                        //    var jjjjj = "fff";
                        //}
                        PfrEntidadDTO objGeneracion = new PfrEntidadDTO();

                        var barraObj = equiposBarras.Find(x => x.Pfrentcodi == relGamsEq.Pfrentcodibarragams);
                        if (barraObj == null)
                        { }
                        if (relGamsEq.Numunidad == null)
                        { }
                        var tension = barraObj != null ? barraObj.Tension : 0;
                        var idBarra = barraObj != null ? barraObj.Pfrentid : "";
                        var nombreBarra = barraObj != null ? barraObj.Pfrentnomb : "";

                        objGeneracion.Pfrentid = relGamsEq.Pfrentid;
                        objGeneracion.Pfrentnomb = nombreBarra;
                        objGeneracion.Numunidad = relGamsEq.Numunidad;
                        objGeneracion.Idbarra1 = idBarra;
                        objGeneracion.Pfrentcodi = relGamsEq.Pfrentcodi;
                        objGeneracion.Tension = tension;

                        //CV
                        //var regdato = listaDatos.Find(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi);
                        var regdato = item.Famcodi == ConstantesPotenciaFirmeRemunerable.FamcodiGeneradorTermo ? listaDatos.Find(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi) : listaDatos.Find(x => x.Equicodi == item.Equicodi);
                        decimal? valorFK = null;
                        if (regdato != null)
                        {
                            objGeneracion.Costov = (relGamsEq.Pfrentficticio == 1 || regdato.CR != null) ? regReporteDatos.Pfrrptcr / 10 : regdato.CV * 100;
                            //objGeneracion.Costov = regdato.CV * 100;
                            valorFK = regdato.FK;
                        }

                        //Calculo de Potencia Disponible
                        decimal? valorPotDisponible = null;
                        var FRF = factorRFirme;
                        var regFK = valorFK;
                        var regPF = item.Pftotpf;

                        if (regFK != null)
                        {
                            if (regFK == 1)
                            {
                                valorPotDisponible = relGamsEq.Pfrentficticio == 1 ? null : regPF / FRF;
                            }
                            else
                            {
                                if (relGamsEq.Pfrentficticio == 1)
                                {
                                    valorPotDisponible = (regPF / FRF) - ((regPF / FRF) * regFK); //pd ficticio
                                }
                                else
                                {
                                    valorPotDisponible = (regPF / FRF) * regFK;
                                }
                            }
                        }

                        //pmax
                        objGeneracion.Potenciamax = (valorPotDisponible.GetValueOrDefault(0) / 1000) * 1000;

                        objGeneracion.Qmax = relGamsEq.Qmax;
                        objGeneracion.Qmin = relGamsEq.Qmin;
                        objGeneracion.Ref = relGamsEq.Ref;

                        listaGeneracion.Add(objGeneracion);
                    }

                }
            }

            return listaGeneracion.OrderBy(x => x.Pfrentid).ToList();
        }

        /// <summary>
        /// Devuelve la informacion del reporte DATOS desde la tabla reporte_total
        /// </summary>
        /// <param name="pfrrptcodi"></param>
        /// <param name="listaParametros"></param>
        /// <returns></returns>
        private List<PestaniaDatos> ObtenerDataDatosPorRangoDeReporte(int pfrrptcodi, out List<PrGrupodatDTO> listaParametros)
        {
            //Parametros (canon de agua y CR) para el mes actual
            PfrReporteDTO reporte = GetByIdPfrReporte(pfrrptcodi);
            PrGrupodatDTO valorCRActual = new PrGrupodatDTO();
            PrGrupodatDTO valorCAActual = new PrGrupodatDTO();
            valorCRActual.Formuladat = reporte.Pfrrptcr != null ? reporte.Pfrrptcr.ToString() : "";
            valorCRActual.Concepcodi = ConstantesPotenciaFirmeRemunerable.ConcepcodiCR;
            valorCAActual.Formuladat = reporte.Pfrrptca != null ? reporte.Pfrrptca.ToString() : "";
            valorCAActual.Concepcodi = ConstantesPotenciaFirmeRemunerable.ConcepcodiCA;
            listaParametros = new List<PrGrupodatDTO>();
            listaParametros.Add(valorCRActual);
            listaParametros.Add(valorCAActual);

            List<PestaniaDatos> listaDatos = new List<PestaniaDatos>();

            #region lista ReporteTotal de Datos
            List<PfrReporteTotalDTO> listadoGeneralDatos = ListPfrReporteTotalByReportecodi(pfrrptcodi);

            List<PfrEscenarioDTO> listaEscenario = ListPfrEscenariosByReportecodi(pfrrptcodi).OrderBy(x => x.Pfrescfecini).ToList();
            int i = 1;
            foreach (var reg in listaEscenario)
            {
                reg.Numero = i;
                i++;
            }

            List<PfrReporteTotalDTO> listaDatosAgrupada = listadoGeneralDatos.GroupBy(x => new { x.Equicodi, x.Grupocodi }).Select(x => new PfrReporteTotalDTO()
            {
                Emprcodi = x.First().Emprcodi,
                Emprnomb = x.First().Emprnomb,
                Equipadre = x.First().Equipadre,
                Central = x.First().Central,
                Equicodi = x.Key.Equicodi,
                Grupocodi = x.Key.Grupocodi,
                Famcodi = x.First().Famcodi,
                Pfrtotunidadnomb = x.First().Pfrtotunidadnomb,

                //Pfrtotpe = x.First().Pfrtotpe,
                //Pfrtotpf = x.First().Pfrtotpf,
                //Pfrtotcv = x.First().Pfrtotcv,
                //Pfrtotcrmesant = x.First().Pfrtotcrmesant,
                //Pfrtotfkmesant = x.First().Pfrtotfkmesant
            }).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Pfrtotunidadnomb).ToList();



            foreach (var regUnidad in listaDatosAgrupada)
            {
                //calculo de pe, pf, cv, cry fk
                List<decimal?> lstPE = new List<decimal?>();
                List<decimal?> lstPF = new List<decimal?>();
                List<decimal?> lstCV = new List<decimal?>();
                List<int?> lstCR = new List<int?>();
                List<decimal?> lstFK = new List<decimal?>();

                foreach (var regEsc in listaEscenario)
                {
                    if (regUnidad.Equicodi == 1195 && regUnidad.Grupocodi == 151)
                    {

                    }
                    var regTotalXUnidadXEsc = listadoGeneralDatos.Find(x => x.Pfresccodi == regEsc.Pfresccodi && x.Equicodi == regUnidad.Equicodi && x.Grupocodi == regUnidad.Grupocodi);
                    if (regTotalXUnidadXEsc != null)
                    {
                        decimal? pe = regTotalXUnidadXEsc.Pfrtotpe;
                        decimal? pf = regTotalXUnidadXEsc.Pfrtotpf;
                        decimal? cv = regTotalXUnidadXEsc.Pfrtotcv;
                        int? cr = regTotalXUnidadXEsc.Pfrtotcrmesant;
                        decimal? fk = regTotalXUnidadXEsc.Pfrtotfkmesant;
                        lstPE.Add(pe);
                        lstPF.Add(pf);
                        lstCV.Add(cv);
                        lstCR.Add(cr);
                        lstFK.Add(fk);
                    }
                }

                decimal? regPE = lstPE.Any() ? lstPE.OrderByDescending(X => X).First() : null;
                decimal? regPF = lstPF.Any() ? lstPF.OrderByDescending(X => X).First() : null;
                decimal? regCV = lstCV.Any() ? lstCV.OrderByDescending(X => X).First() : null;
                int? regCR = lstCR.Any() ? lstCR.OrderByDescending(X => X).First() : null;
                decimal? regFK = lstFK.Any() ? lstFK.OrderByDescending(X => X).First() : null;

                regUnidad.Pfrtotpe = regPE;
                regUnidad.Pfrtotpf = regPF;
                regUnidad.Pfrtotcv = regCV;
                regUnidad.Pfrtotcrmesant = regCR;
                regUnidad.Pfrtotfkmesant = regFK;

            }
            #endregion

            #region Lista para Reporte Datos
            foreach (var reg in listaDatosAgrupada)
            {
                PestaniaDatos objDatos = new PestaniaDatos();

                var grupocodiReg = reg.Grupocodi;
                var equicodiReg = reg.Equicodi;
                var famcodiReg = reg.Famcodi;

                objDatos.Emprcodi = reg.Emprcodi.Value;
                objDatos.Grupocodi = grupocodiReg;
                objDatos.Equicodi = equicodiReg;
                objDatos.Empresa = reg.Emprnomb;
                objDatos.Central = reg.Central;
                objDatos.UnidadNombre = reg.Pfrtotunidadnomb;

                objDatos.PE = reg.Pfrtotpe;
                objDatos.PF = reg.Pfrtotpf;
                objDatos.CV = reg.Pfrtotcv;
                objDatos.FK = reg.Pfrtotfkmesant;
                objDatos.CR = reg.Pfrtotcrmesant != null ? (reg.Pfrtotcrmesant == 1 ? "CR" : "") : null;

                listaDatos.Add(objDatos);
            }
            #endregion

            return listaDatos;
        }

        private List<PestaniaDemanda> ObtenerDataDemandaPorRango(DateTime fechaIniRango, DateTime fechaFinRango, List<BarraSuministro> listadoCalculoBarras, List<BarraSSAA> lstBarraSSAA)
        {
            List<PestaniaDemanda> listaDemanda = new List<PestaniaDemanda>();
            //Obtener Data
            var listaCalculoBarras = listadoCalculoBarras; //

            //Obtener Listado de barras activas para el periodo            
            var listaBarraActivasParaRango = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Barra, fechaIniRango, fechaFinRango);

            foreach (var barra in listaBarraActivasParaRango)
            {
                if (barra.Pfrentid == "B0007")
                { }
                PestaniaDemanda objDemanda = new PestaniaDemanda();
                var codiBarra = barra.Pfrentcodi;

                //Sumo todos los PLoad y QLoadde la primera lista, para una barra
                List<BarraSuministro> barrasLista1 = listaCalculoBarras.Where(x => x.CodigoGams == codiBarra).ToList();
                decimal? valorP1 = barrasLista1.Sum(n => n.Pload);
                decimal? valorQ1 = barrasLista1.Sum(n => n.Qload);

                //Sumo todos los PLoad de la segunda lista, para una barra
                List<BarraSSAA> barrasLista2 = lstBarraSSAA.Where(x => x.Codigo == codiBarra).ToList();
                decimal? valorP2 = barrasLista2.Sum(n => n.Ssaa);

                objDemanda.CodiBarra = codiBarra;
                objDemanda.IdBarra = barra.Pfrentid;
                objDemanda.NombreBarra = barra.Pfrentnomb;
                objDemanda.TensionBarra = barra.Tension;
                objDemanda.P = valorP1 + valorP2;
                objDemanda.Q = valorQ1;
                objDemanda.CompReactiva = barra.Compreactiva;

                listaDemanda.Add(objDemanda);
            }

            return listaDemanda;
        }

        #endregion

        #region Procesar

        /// <summary>
        /// Guarda reportes (ReporteLVTP) a la BD, genera archivos .dat, obtiene resultados del gams y guarda dichos resultados en la BD
        /// </summary>
        /// <param name="pfrreccodi"></param>
        /// <param name="pfrecacodi"></param>
        /// <param name="indrecacodiant"></param>
        /// <param name="usuario"></param>
        /// <param name="pathActual"></param>
        /// <returns></returns>
        public int CalcularReportePFR(int pfrreccodi, int pfrecacodi, int indrecacodiant, int recpotcodi, string usuario)
        {
            #region Insumos

            PfrRecalculoDTO objPfrRecalculo = GetByIdPfrRecalculo(pfrreccodi);
            PfrPeriodoDTO objPfrPeriodo = GetByIdPfrPeriodo(objPfrRecalculo.Pfrpercodi);
            DateTime fechaRegistro = DateTime.Now;

            #region Obtener Reporte PF para utilizarlo en los reportes de PFR

            PfReporteDTO pfReporte = new PfReporteDTO();
            List<PfReporteDTO> lstPfReporte = new List<PfReporteDTO>();

            //El ultimo reporte es el que se usará
            lstPfReporte = pfAppServicio.GetByCriteriaPfReportes(pfrecacodi, ConstantesPotenciaFirme.CuadroPFirme).OrderByDescending(x => x.Pfrptcodi).ToList();

            //Si no existe calculo para PF en el recalculoPF elegido, cancela toda la operación
            if (!lstPfReporte.Any()) throw new ArgumentException("¡No existe Calculo de PF para la revisión elegida!");

            //Si existe Calculos de PF, obtengo el codigo del reporte dePF
            int ultimoReporteCodiPFXRecalculo = lstPfReporte.First().Pfrptcodi;

            _indAppService.ListarUnidadTermicoOpComercial(ConstantesIndisponibilidades.AppPFR, objPfrPeriodo.FechaIni, objPfrPeriodo.FechaFin, out List<EqEquipoDTO> listaUnidadesTermo, out List<EqEquipoDTO> listaEquiposTermicos, out List<ResultadoValidacionAplicativo> listaMsj4);
            var listaPotenciaFirme = pfAppServicio.ListarPotenciaFirme(ultimoReporteCodiPFXRecalculo, listaUnidadesTermo);
            if (!listaPotenciaFirme.Any()) throw new ArgumentException(" No se ha procesado Potencia Firme para el recalculo ingresado");

            #endregion

            #region Obtener MD (valor y fecha) para el periodo

            PfrReporteDTO reportePFRMolde = new PfrReporteDTO();
            this.SetMaximaDemanda(reportePFRMolde, objPfrPeriodo.FechaIni, objPfrPeriodo.FechaFin);
            DateTime? fechaMD = reportePFRMolde.Pfrrptfecmd;
            decimal? MD = reportePFRMolde.Pfrrptmd;
            if (MD == 0 || MD == null) throw new ArgumentException(" No existe información de Máxima Demanda para el periodo");
            #endregion

            #region Parametros (canon de agua y CR) para el mes actual

            List<PrGrupodatDTO> listaParametros = ListarParametrosConfiguracionPorFecha(objPfrPeriodo.FechaFin, ConstantesPotenciaFirmeRemunerable.ConcepcodiIngresos);
            PrGrupodatDTO valorCRActual = listaParametros.Find(r => r.Concepcodi == ConstantesPotenciaFirmeRemunerable.ConcepcodiCR);
            PrGrupodatDTO valorCAActual = listaParametros.Find(r => r.Concepcodi == ConstantesPotenciaFirmeRemunerable.ConcepcodiCA);
            PrGrupodatDTO valorMRActual = listaParametros.Find(r => r.Concepcodi == ConstantesPotenciaFirmeRemunerable.ConcepcodiMR);
            decimal? valorNulo = null;
            decimal? valorcr = valorCRActual.Formuladat != "" ? Convert.ToDecimal(valorCRActual.Formuladat) : valorNulo;
            decimal? valorca = valorCAActual.Formuladat != "" ? Convert.ToDecimal(valorCAActual.Formuladat) : valorNulo;
            decimal? valormr = valorMRActual.Formuladat != "" ? Convert.ToDecimal(valorMRActual.Formuladat) : valorNulo;

            #endregion

            /** Obtenemos los escenarios presentes en el periodo, segun PF. (campos generales)**/
            List<PfrEscenarioDTO> escenariosDePFR = ObtenerEscenariosXPeriodoPFR(objPfrPeriodo);

            #region Reporte Indisponibilidades (Periodo anterior)

            IndReporteDTO objReporteINDFactorK = new IndReporteDTO();
            IndReporteDTO objReporteINDCRTermico = new IndReporteDTO();
            IndReporteDTO objReporteINDCRHidro = new IndReporteDTO();
            int? iReporteCodiFKMesAnterior = null;
            int? iReporteCodiCRTermFortuitoMesAnterior = null;
            int? iReporteCodiCRTermProgramadoMesAnterior = null;
            int? iReporteCodiCRHMesAnterior = null;

            //Obtenemos los reportesCodis de factor k             
            _indAppService.GetCodigoReporteAprobadoXCuadro(indrecacodiant, ConstantesIndisponibilidades.ReportePR25Cuadro3FactorK, out int rptcodi11, out int numVersion11, out string mensaje11);
            iReporteCodiFKMesAnterior = rptcodi11;
            if (mensaje11 != "") throw new ArgumentException(" Indisponibilidades (Mes anterior) " + mensaje11);

            #endregion

            #region Obtenemos la lista de FK, CR Termo y CR Hidro (mes anterior)       
            //agregar reportes de factores de indisponibilidad
            _indAppService.GetCodigoReporteAprobadoXCuadro(indrecacodiant, ConstantesIndisponibilidades.ReportePR25FactorFortTermico, out int rptcodi8, out int numVersion8, out string mensaje8);
            if (mensaje8 != "") throw new ArgumentException(" Indisponibilidades (Mes anterior) " + mensaje8);
            _indAppService.GetCodigoReporteAprobadoXCuadro(indrecacodiant, ConstantesIndisponibilidades.ReportePR25FactorProgTermico, out int rptcodi9, out int numVersion9, out string mensaje9);
            if (mensaje9 != "") throw new ArgumentException(" Indisponibilidades (Mes anterior) " + mensaje9);
            _indAppService.GetCodigoReporteAprobadoXCuadro(indrecacodiant, ConstantesIndisponibilidades.ReportePR25FactorProgHidro, out int rptcodi10, out int numVersion10, out string mensaje10);
            if (mensaje10 != "") throw new ArgumentException(" Indisponibilidades (Mes anterior) " + mensaje10);

            //obtener data por cada reporte
            List<IndReporteTotalDTO> listaReptot8 = _indAppService.GetByCriteriaIndReporteTotals(rptcodi8).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();
            List<IndReporteTotalDTO> listaReptot9 = _indAppService.GetByCriteriaIndReporteTotals(rptcodi9).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();
            List<IndReporteTotalDTO> listaReptot10 = _indAppService.GetByCriteriaIndReporteTotals(rptcodi10).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();

            List<IndReporteTotalDTO> listaReptotCRTotal = listaReptot8;
            listaReptotCRTotal.AddRange(listaReptot9); //junto las termicas (fortutita y programada)
            List<IndReporteTotalDTO> listaReptotFK = iReporteCodiFKMesAnterior != null ? _indAppService.GetByCriteriaIndReporteTotals(iReporteCodiFKMesAnterior.Value).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList() : new List<IndReporteTotalDTO>();

            //duplicar el factork
            List<EqEquipoDTO> listaUnidIncr = listaUnidadesTermo.Where(x => x.Grupoincremental == 1).ToList();
            foreach (var reg in listaUnidIncr)
            {
                //con el grupopadre se determina el modo de operación original
                IndReporteTotalDTO regKRel = listaReptotFK.Find(x => x.Grupocodi == reg.Grupopadre);
                IndReporteTotalDTO regK = listaReptotFK.Find(x => x.Grupocodi == reg.Grupocodi);

                //si la unidad del mes actual no tiene factor k del mes anterior, duplicar el valor del modo original
                if (regK == null && regKRel != null)
                {
                    IndReporteTotalDTO regKNuevo = new IndReporteTotalDTO()
                    {
                        Grupocodi = reg.Grupocodi,
                        Equicodi = regKRel.Equicodi,
                        Itotfactork = regKRel.Itotfactork,
                    };

                    listaReptotFK.Add(regKNuevo);
                }
            }

            #region CT Las Flores CCOMB cuando ingresa en operación comercial

            if (objPfrPeriodo.FechaIni == new DateTime(2022, 6, 1))
            {
                //si no existe FK de las flores ccomb, generar el registro en memoria con el valor de 1
                if (listaReptotFK.Find(x => x.Grupocodi == 3375) == null)
                {
                    var regCS = listaReptotFK.Find(x => x.Grupocodi == 304);
                    if (regCS != null)
                    {
                        IndReporteTotalDTO regKNuevo = new IndReporteTotalDTO()
                        {
                            Grupocodi = 3375, //LFLORES CCOMB TG1 - GAS	
                            Equicodi = 12720, //C.T. LAS FLORES	
                            Itotfactork = 1,
                        };

                        listaReptotFK.Add(regKNuevo);
                    }
                }
            }

            #endregion

            List<IndReporteTotalDTO> listaReptotCRT = listaReptotCRTotal;

            List<IndReporteTotalDTO> listaReptotCRH = listaReptot10;

            //Comprobamos si tienen existen con CR y Fk
            var lstConCRT = listaReptotCRT.Where(x => x.Itotcr != null).ToList();
            var lstConCRH = listaReptotCRH.Where(x => x.Itotcr != null).ToList();
            var lstConFK = listaReptotFK.Where(x => x.Itotfactork != null).ToList();


            #endregion

            #region Costos Variables     

            List<PrCvariablesDTO> listDataCV = FactorySic.GetPrCvariablesRepository().ListCostoVariablesxRangoFecha(objPfrPeriodo.FechaIni.AddDays(-7), objPfrPeriodo.FechaFin);

            #region Obtener costos variables en memoria anteriores a Febrero 2022

            int imes = Int32.Parse(ConstantesPotenciaFirmeRemunerable.MesCVBaseDatos.Substring(0, 2));
            int ianho = Int32.Parse(ConstantesPotenciaFirmeRemunerable.MesCVBaseDatos.Substring(3, 4));
            DateTime fechaCVbd = new DateTime(ianho, imes, 1);

            if (objPfrPeriodo.FechaIni < fechaCVbd)
            {
                DespachoAppServicio appDespacho = new DespachoAppServicio();

                //obtener los costos variables en memoria
                List<int> listaRepcodi = listDataCV.Select(x => x.Repcodi).Distinct().ToList();

                listDataCV = new List<PrCvariablesDTO>();
                foreach (var repcodi in listaRepcodi)
                {
                    var lsResultado = new List<PrCvariablesDTO>();

                    var oRepCv = appDespacho.GetByIdPrRepcv(repcodi);
                    bool flagBd = false;
                    appDespacho.GenerarCostosVariables(oRepCv, ref lsResultado, flagBd);

                    listDataCV.AddRange(lsResultado);
                }
            }

            #endregion

            //duplicar costos variables para los incrementales
            foreach (var reg in listaUnidIncr)
            {
                //con el grupopadre se determina el modo de operación original
                List<PrCvariablesDTO> listaCVXReg = listDataCV.Where(x => x.Grupocodi == reg.Grupopadre).ToList();
                foreach (var regCV in listaCVXReg)
                {
                    PrCvariablesDTO regCVNuevo = new PrCvariablesDTO()
                    {
                        Grupocodi = reg.Grupocodi ?? 0,
                        Repfecha = regCV.Repfecha,
                        Cvc = regCV.Cvc,
                        Cvnc = regCV.Cvnc
                    };

                    listDataCV.Add(regCVNuevo);
                }
            }

            //costos variables del periodo actual (Parametro grupos/mop)
            List<PrGrupodatDTO> listaCVGrupodat = INDAppServicio.ListarPrGrupodatHistoricoDecimalValido(ConstantesPotenciaFirmeRemunerable.ConcepcodiCV_PFR.ToString());
            listaCVGrupodat = listaCVGrupodat.Where(x => x.Fechadat.Value.Date == objPfrPeriodo.FechaIni).ToList();

            foreach (var regDat in listaCVGrupodat)
            {
                EqEquipoDTO regEq = listaEquiposTermicos.Find(x => x.Grupocodi == regDat.Grupocodi);
                if (regEq != null)
                    regDat.Equipadre = regEq.Equipadre ?? 0;
            }

            #endregion

            #region Disponibilidad de Calor Util

            List<PfRelacionIndisponibilidadesDTO> listaIrpt = pfAppServicio.GetByCriteriaPfRelacionIndisponibilidadess(ultimoReporteCodiPFXRecalculo);
            int irptcodiDispCU = listaIrpt.Find(x => x.Icuacodi == ConstantesIndisponibilidades.ReportePR25DisponibilidadCalorUtil).Irptcodi;
            List<PfDispcalorutilDTO> listaDispCU = _indAppService.GetByCriteriaPfDispcalorutils(irptcodiDispCU);

            //quedarse con la informacion del dia, hora y cuarto minuto de máxima demanda
            listaDispCU = listaDispCU.Where(x => x.FechaHora == fechaMD).ToList();

            #endregion

            //Listado usado en reporte CARGA
            var listaCalculoBarras = this.ListarCalculoBarra(objPfrPeriodo, fechaMD, recpotcodi
                                , out List<BarraSSAA> lstBarraSSAA, out List<PfrEntidadDTO> listaRelacion
                                , out List<VtpPeajeEgresoMinfoDTO> listaPeajeEgresoMinfo, out List<VtpRetiroPotescDTO> listaRetiroSinContrato);

            #endregion

            #region Guardar Pestaña C8

            PfrReporteDTO regPfrReporteC8 = new PfrReporteDTO()
            {
                Pfrcuacodi = ConstantesPotenciaFirmeRemunerable.CuadroC8,
                Pfrreccodi = pfrreccodi,
                Pfrrptesfinal = ConstantesPotenciaFirmeRemunerable.EsVersionGenerado,
                Pfrrptcr = valorcr,
                Pfrrptca = valorca,
                Pfrrptmr = valormr,
                Pfrrptmd = reportePFRMolde?.Pfrrptmd,
                Pfrrptfecmd = reportePFRMolde?.Pfrrptfecmd,
                Pfrrptusucreacion = usuario,
                Pfrrptfeccreacion = fechaRegistro

            };

            regPfrReporteC8.ListaPfrEscenario = escenariosDePFR;


            foreach (var escenarioPFR in regPfrReporteC8.ListaPfrEscenario)
            {
                var listaPFxEsc = listaPotenciaFirme.Where(x => x.Pfescefecini == escenarioPFR.Pfrescfecini && x.Pfescefecfin == escenarioPFR.Pfrescfecfin).ToList();

                List<PfrReporteTotalDTO> listaRepTotalC8 = ObtenerReporteC8(listaPFxEsc);

                escenarioPFR.ListaPfrReporteTotal = listaRepTotalC8;
            }

            //Funcion transaccional para guardar en BD

            /** Listado de codis (para guardar Relacion_Ind y Relacion_PF) **/
            int?[] lstCodisForeyKey = new int?[] {
                ultimoReporteCodiPFXRecalculo,
                iReporteCodiFKMesAnterior,
                iReporteCodiCRTermFortuitoMesAnterior,
                iReporteCodiCRTermProgramadoMesAnterior,
                iReporteCodiCRHMesAnterior
            };

            int reporteCodiC8 = this.GuardarReportePFR_BDTransaccional(regPfrReporteC8, lstCodisForeyKey);

            #endregion

            #region Guardar Pestaña Datos
            //PfrReporteDTO regReporte = GetByIdPfrReporte(pfrrptcodi);
            PfrReporteDTO regPfrReporteDatos = new PfrReporteDTO()
            {
                Pfrcuacodi = ConstantesPotenciaFirmeRemunerable.CuadroDatos,
                Pfrreccodi = pfrreccodi,
                Pfrrptesfinal = ConstantesPotenciaFirmeRemunerable.EsVersionGenerado,
                Pfrrptcr = valorcr,
                Pfrrptca = valorca,
                Pfrrptmr = valormr,
                Pfrrptmd = reportePFRMolde.Pfrrptmd,
                Pfrrptfecmd = reportePFRMolde.Pfrrptfecmd,
                Pfrrptusucreacion = usuario,
                Pfrrptfeccreacion = fechaRegistro,
                Pfrrptrevisionvtp = recpotcodi
            };

            regPfrReporteDatos.ListaPfrEscenario = escenariosDePFR;



            foreach (var escenarioPFR in regPfrReporteDatos.ListaPfrEscenario)
            {
                var listaPFxEsc = listaPotenciaFirme.Where(x => x.Pfescefecini == escenarioPFR.Pfrescfecini && x.Pfescefecfin == escenarioPFR.Pfrescfecfin).ToList();

                List<PfrReporteTotalDTO> listaRepTotalDatos = ObtenerReporteTotalDatos(listaPFxEsc, valorCAActual, fechaMD, listaReptotFK
                                , listaReptotCRT, listaReptotCRH, listDataCV, listaDispCU, listaCVGrupodat);

                escenarioPFR.ListaPfrReporteTotal = listaRepTotalDatos;
            }

            //Funcion transaccional para guardar en BD
            int reporteCodiDatos = this.GuardarReportePFR_BDTransaccional(regPfrReporteDatos, lstCodisForeyKey);
            #endregion

            #region Guardar Pestaña AUX1            

            //List<PeriodoDTO> listaPeriodos = this.servicioPeriodo.ListPeriodo();
            //PeriodoDTO periodoTransferencia = listaPeriodos.Find(x => x.AnioCodi == objPfrPeriodo.Pfrperanio && x.MesCodi == objPfrPeriodo.Pfrpermes);

            PfrReporteDTO regPfrReporteAux1 = new PfrReporteDTO()
            {
                Pfrcuacodi = ConstantesPotenciaFirmeRemunerable.CuadroAUX1,
                Pfrreccodi = pfrreccodi,
                Pfrrptesfinal = ConstantesPotenciaFirmeRemunerable.EsVersionGenerado,
                Pfrrptcr = valorcr,
                Pfrrptca = valorca,
                Pfrrptmr = valormr,
                Pfrrptmd = reportePFRMolde.Pfrrptmd,
                Pfrrptfecmd = reportePFRMolde.Pfrrptfecmd,
                Pfrrptusucreacion = usuario,
                Pfrrptfeccreacion = fechaRegistro

            };

            regPfrReporteAux1.ListaPfrEscenario = escenariosDePFR;

            foreach (var escenarioPFR in regPfrReporteAux1.ListaPfrEscenario)
            {

                var listaPFxEsc = listaPotenciaFirme.Where(x => x.Pfescefecini == escenarioPFR.Pfrescfecini && x.Pfescefecfin == escenarioPFR.Pfrescfecfin).ToList();

                //Calculo de FRF Y PFCT
                var maxDemanda = MathHelper.Round(ConvertirMWaKw(regPfrReporteAux1.Pfrrptmd), 0);

                List<PfrReporteTotalDTO> listaRepTotalAux1 = ObtenerReporteTotalAux1(listaPFxEsc, valorCAActual, fechaMD, maxDemanda, regPfrReporteAux1.Pfrrptmr, regPfrReporteAux1.Pfrrptcr
                    , listaReptotCRT, listaReptotCRH, listDataCV, listaDispCU, listaCVGrupodat);

                escenarioPFR.ListaPfrReporteTotal = listaRepTotalAux1;

                var PFCT = listaRepTotalAux1.Sum(n => n.Pfrtotpfc);
                var FRF = PFCT / maxDemanda;

                escenarioPFR.Pfrescpfct = PFCT;
                escenarioPFR.Pfrescfrf = FRF;
            }

            //Funcion transaccional para guardar en BD
            int reporteCodiAux1 = this.GuardarReportePFR_BDTransaccional(regPfrReporteAux1, lstCodisForeyKey);
            #endregion

            #region Archivos .dat y Obtener salidas GAMS      

            List<PfrEntidadDTO> listaGeneracionTotal = new List<PfrEntidadDTO>();
            List<PfrEntidadDTO> listaBarrasTotal = new List<PfrEntidadDTO>();
            List<PfrEntidadDTO> listaCongestionTotal = new List<PfrEntidadDTO>();

            //Uso escenario de Aux1 para guardar los resultados de la Salida Gams
            List<PfrEscenarioDTO> listaEscenarioAux1 = ListPfrEscenariosByReportecodi(reporteCodiAux1);

            foreach (var escenario in listaEscenarioAux1)
            {
                DateTime fecIniEscenario = escenario.Pfrescfecini;
                DateTime fecFinEscenario = escenario.Pfrescfecfin;

                List<PfrEntidadDTO> listaBarra = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Barra, fecIniEscenario, fecFinEscenario);
                List<PfrEntidadDTO> listaLineasActivas = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Linea, fecIniEscenario, fecFinEscenario);
                List<PfrEntidadDTO> listaTrafo2Activas = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2, fecIniEscenario, fecFinEscenario);
                List<PfrEntidadDTO> listaTrafo3Activas = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3, fecIniEscenario, fecFinEscenario);
                List<PfrEntidadDTO> lenlace = ObtenerListadoEnlaces(listaLineasActivas, listaTrafo2Activas, listaTrafo3Activas);

                List<PestaniaDemanda> ldemanda = ObtenerDataDemandaPorRango(fecIniEscenario, fecFinEscenario, listaCalculoBarras, lstBarraSSAA);
                List<PfrEntidadDTO> lgeneracion = ObtenerDataGeneracionPorRango(ultimoReporteCodiPFXRecalculo, fecIniEscenario, fecFinEscenario, reporteCodiDatos, reporteCodiAux1);
                List<PfrEntidadDTO> lcompreactiva = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica, fecIniEscenario, fecFinEscenario);
                List<PfrEntidadDTO> lcongestion = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Congestion, fecIniEscenario, fecFinEscenario);
                List<PfrEntidadDTO> lpenalidad = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Penalidad, fecIniEscenario, fecFinEscenario);

                //Obtengo la lista de generacion, barras y congestion de todos escenarios
                foreach (var regGeneracion in lgeneracion) regGeneracion.EscenariocodiParaGams = escenario.Pfresccodi;
                foreach (var regBarra in listaBarra) regBarra.EscenariocodiParaGams = escenario.Pfresccodi;
                foreach (var regCongestion in lcongestion) regCongestion.EscenariocodiParaGams = escenario.Pfresccodi;

                listaGeneracionTotal.AddRange(lgeneracion);
                listaBarrasTotal.AddRange(listaBarra);
                listaCongestionTotal.AddRange(lcongestion);

                // Genera archivo entrada Gams
                CrearArchivoEntrada(fecIniEscenario, listaBarra, ldemanda, lgeneracion, lcompreactiva, lenlace, lcongestion, lpenalidad, escenario.Numero, objPfrRecalculo.Pfrrecnombre);
            }

            #endregion

            /// Corre Modelo Gmas
            List<SalidaGams> salidasGams = EjecutarPotRemunerableGAMS(listaEscenarioAux1, objPfrRecalculo.Pfrpercodi, objPfrRecalculo.Pfrrecnombre);

            #region Guardamos las salidas del GAMS en la BD

            //List<SalidaGams> salidasGams = new List<SalidaGams>();
            List<SalidaGams> listaSalidaTotalPG = salidasGams.Where(x => x.Tipo == (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Pg).ToList();
            List<SalidaGams> listaSalidaTotalV = salidasGams.Where(x => x.Tipo == (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.V).ToList();
            List<SalidaGams> listaSalidaTotalCongestion = salidasGams.Where(x => x.Tipo == (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Congestion).ToList();

            int resultadoPG = GuardarSalidasGams((int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Pg, listaSalidaTotalPG, listaGeneracionTotal, listaBarrasTotal, listaCongestionTotal, out List<PfrResultadosGamsDTO> lstResultadosPG);
            int resultadoV = GuardarSalidasGams((int)ConstantesPotenciaFirmeRemunerable.SalidasGams.V, listaSalidaTotalV, listaGeneracionTotal, listaBarrasTotal, listaCongestionTotal, out List<PfrResultadosGamsDTO> lstResultadosV);
            int resultadoC = GuardarSalidasGams((int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Congestion, listaSalidaTotalCongestion, listaGeneracionTotal, listaBarrasTotal, listaCongestionTotal, out List<PfrResultadosGamsDTO> lstResultadosCongestion);


            #endregion

            #region Guardar Pestaña Aux2

            PfrReporteDTO regPfrReporteAux2 = new PfrReporteDTO()
            {
                Pfrcuacodi = ConstantesPotenciaFirmeRemunerable.CuadroPFirmeRemunerable,
                Pfrreccodi = pfrreccodi,
                Pfrrptesfinal = ConstantesPotenciaFirmeRemunerable.EsVersionGenerado,
                Pfrrptcr = valorcr,
                Pfrrptca = valorca,
                Pfrrptmr = valormr,
                Pfrrptmd = reportePFRMolde.Pfrrptmd,
                Pfrrptfecmd = reportePFRMolde.Pfrrptfecmd,
                Pfrrptusucreacion = usuario,
                Pfrrptfeccreacion = fechaRegistro

            };

            regPfrReporteAux2.ListaPfrEscenario = escenariosDePFR;

            //List<PfrReporteTotalDTO> listaau = new List<PfrReporteTotalDTO>();
            foreach (var escenarioPFR in regPfrReporteAux2.ListaPfrEscenario)
            {
                var escenarioCodiAux1SalidaGams = listaEscenarioAux1.Find(x => x.Numero == escenarioPFR.Numero).Pfresccodi;

                List<PfrResultadosGamsDTO> lstResultadosPorEscenario = lstResultadosPG.Where(x => x.Pfresccodi == escenarioCodiAux1SalidaGams).ToList();

                var maxDemanda = MathHelper.Round(ConvertirMWaKw(reportePFRMolde.Pfrrptmd), 0);
                var FRF = escenarioPFR.Pfrescfrf;

                var listaPFxEsc = listaPotenciaFirme.Where(x => x.Pfescefecini == escenarioPFR.Pfrescfecini && x.Pfescefecfin == escenarioPFR.Pfrescfecfin).ToList();

                List<PfrEntidadDTO> listaRelacionGE = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos, escenarioPFR.Pfrescfecini, escenarioPFR.Pfrescfecfin);

                List<PfrReporteTotalDTO> listaRepTotalAux2 = ObtenerReporteAux2(listaPFxEsc, listaReptotFK, listaReptotCRT, listaReptotCRH, lstResultadosPorEscenario, listaRelacionGE, maxDemanda, FRF, out decimal? FRFR);
                escenarioPFR.ListaPfrReporteTotal = listaRepTotalAux2;

                //Seteamos FRFR en el escenario                
                escenarioPFR.Pfrescfrfr = FRFR;
            }

            //Funcion transaccional para guardar en BD
            int reporteCodiAux2 = this.GuardarReportePFR_BDTransaccional(regPfrReporteAux2, lstCodisForeyKey);

            #endregion

            return reporteCodiAux2;
        }


        /// <summary>
        /// Genera los archivos .dat
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="lbarra"></param>
        /// <param name="ldemanda"></param>
        /// <param name="lgeneracion"></param>
        /// <param name="ltension"></param>
        /// <param name="lcompreactiva"></param>
        /// <param name="lenlace"></param>
        /// <param name="lcongestion"></param>
        /// <param name="lpenalidad"></param>
        /// <param name="correlativo"></param>
        /// <param name="nombreRevision"></param>
        /// <returns></returns>
        public bool CrearArchivoEntrada(DateTime fechaProceso
            , List<PfrEntidadDTO> lbarra, List<PestaniaDemanda> ldemanda, List<PfrEntidadDTO> lgeneracion
            , List<PfrEntidadDTO> lcompreactiva, List<PfrEntidadDTO> lenlace, List<PfrEntidadDTO> lcongestion, List<PfrEntidadDTO> lpenalidad
            , int correlativo, string nombreRevision)
        {
            string path = ConfigurationManager.AppSettings[ConstantesPotenciaFirmeRemunerable.PathPotRemunerable];

            string stEntrada = UtilPotenciaFirmeRemunerable.GenerarEntradaGams(lbarra, ldemanda, lgeneracion, lcompreactiva, lenlace, lcongestion, lpenalidad);
            string pathTrabajo = FileHelper.EstablecerCarpetaTrabajo(fechaProceso, path, nombreRevision, correlativo);
            return FileHelper.GenerarArchivo(ConstantesPotenciaFirmeRemunerable.ArchivoEntradaGams, path + pathTrabajo, stEntrada);
        }

        /// <summary>
        /// Encuentra la solución del escenario mediante Gams
        /// </summary>
        /// <param name="escenariosPFR"></param>
        /// <param name="pfrpercodi"></param>
        /// <param name="nombreRevision"></param>
        /// <returns></returns>
        public List<SalidaGams> EjecutarPotRemunerableGAMS(List<PfrEscenarioDTO> escenariosPFR, int pfrpercodi, string nombreRevision)
        {
            string path = ConfigurationManager.AppSettings[ConstantesPotenciaFirmeRemunerable.PathPotRemunerable];

            ObtenerArchivosFuenteGams(pfrpercodi, out string fuenteGams1, out string fuenteGams2);

            List<SalidaGams> lResultado = new List<SalidaGams>();
            foreach (var escenario in escenariosPFR)
            {
                DateTime fecIniEscenario = escenario.Pfrescfecini;
                string pathTrabajo = FileHelper.EstablecerCarpetaTrabajo(fecIniEscenario, path, nombreRevision, escenario.Numero);

                bool bFgams2 = FileHelper.GenerarArchivo(ConstantesPotenciaFirmeRemunerable.ArchivoInicializaGms, path + pathTrabajo, fuenteGams2);
                if (!bFgams2) return new List<SalidaGams>();

                EjecucionGams.Ejecutar(out GAMSJob modelo, fecIniEscenario, path + pathTrabajo, fuenteGams1);

                List<SalidaGams> listaResultado = FileHelper.ObtenerResultadoGams(ConstantesPotenciaFirmeRemunerable.ArchivoSalidaGams, escenario.Pfresccodi, path + pathTrabajo);
                List<SalidaGams> lResultadoV = FileHelper.ObtenerResultadoGamsV(modelo, path + pathTrabajo, escenario.Pfresccodi);
                lResultado.AddRange(listaResultado);
                lResultado.AddRange(lResultadoV);
            }

            return lResultado;
        }

        private void ObtenerArchivosFuenteGams(int pfrpercodi, out string fuenteGams1, out string fuenteGams2)
        {
            fuenteGams1 = string.Empty;
            fuenteGams2 = string.Empty;
            PfrPeriodoDTO pfrPeriodo = this.GetByIdPfrPeriodo(pfrpercodi);
            string nombreFuenteGams1 = string.Empty;
            string nombreFuenteGams2 = string.Empty;

            string path = "";
            //Crear path para código fuente GAMS
            path = "//" + ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile + ConstantesPotenciaFirmeRemunerable.SNombreCarpetaCargaGams + "\\";
            var listaDocumentos = FileServer.ListarArhivos(path, null);

            //>>>>>>>>>>>>>>
            List<string> nombres = new List<string>();
            foreach (var item in listaDocumentos)
            {
                nombres.Add(item.FileName);
            }

            var periodofiltro = pfrPeriodo.Pfrperanio.ToString() + pfrPeriodo.Pfrpermes.ToString();
            var filesperiodo = nombres.Where(x => x.Contains(periodofiltro.ToString())).ToList();

            if (filesperiodo.Count != 0)
            {
                filesperiodo = filesperiodo.OrderByDescending(x => x).ToList();
                var parte1 = filesperiodo.Where(x => x.Contains("P1")).OrderByDescending(x => x).ToList();
                var parte2 = filesperiodo.Where(x => x.Contains("P2")).OrderByDescending(x => x).ToList();

                nombreFuenteGams1 = parte1.Count == 0 ? this.ObtenerNombreFuenteGamsActual(nombres, "P1") : parte1.First();
                nombreFuenteGams2 = parte2.Count == 0 ? this.ObtenerNombreFuenteGamsActual(nombres, "P2") : parte2.First();
            }
            else
            {
                nombres = nombres.OrderByDescending(x => x).ToList();
                if (nombres.Count > 0)
                {
                    nombreFuenteGams1 = this.ObtenerNombreFuenteGamsActual(nombres, "P1");
                    nombreFuenteGams2 = this.ObtenerNombreFuenteGamsActual(nombres, "P2");
                }
            }

            string pathi = "//" + ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile + ConstantesPotenciaFirmeRemunerable.SNombreCarpetaCargaGams + "\\";

            var archivos = FileServer.ListarArhivos(pathi, null);
            if (archivos.Find(x => x.FileName == nombreFuenteGams1) != null)
                fuenteGams1 = FileServer.OpenReaderFile(pathi + nombreFuenteGams1, null).ReadToEnd();
            if (archivos.Find(x => x.FileName == nombreFuenteGams2) != null)
                fuenteGams2 = FileServer.OpenReaderFile(pathi + nombreFuenteGams2, null).ReadToEnd();

        }

        public string ObtenerNombreFuenteGamsActual(List<string> nombres, string parte)
        {
            nombres = nombres.OrderByDescending(x => x).ToList();
            string nombrefinal = string.Empty;

            if (nombres != null || nombres.Count > 0)
            {
                var listafiltro = nombres.Where(x => x.Contains(parte)).OrderByDescending(x => x).ToList();
                nombrefinal = listafiltro.Count == 0 ? string.Empty : listafiltro.First();
            }

            return nombrefinal;
        }

        #endregion

        #region Guardar resultado

        /// <summary>
        /// Metodo para guardar las salidas del gams en la tabla Resultados_Gams
        /// </summary>
        /// <param name="tipoResultadoGams"></param>
        /// <param name="listaSalida"></param>
        /// <param name="listaGeneracionTotal"></param>
        /// <param name="listaBarrasTotal"></param>
        /// <param name="listaCongestionTotal"></param>
        /// <param name="lstResultados"></param>
        /// <returns></returns>
        private int GuardarSalidasGams(int tipoResultadoGams, List<SalidaGams> listaSalida, List<PfrEntidadDTO> listaGeneracionTotal
            , List<PfrEntidadDTO> listaBarrasTotal, List<PfrEntidadDTO> listaCongestionTotal, out List<PfrResultadosGamsDTO> lstResultados)
        {

            List<PfrResultadosGamsDTO> lstDeResultados = new List<PfrResultadosGamsDTO>();

            foreach (var reg in listaSalida)
            {
                PfrResultadosGamsDTO regResultadoGams = new PfrResultadosGamsDTO();

                if (tipoResultadoGams == (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Pg)
                {
                    var idGeneracion = reg.Id;
                    var escenarioGeneracion = reg.Escenariocodi;

                    PfrEntidadDTO objRGE = listaGeneracionTotal.Find(m => m.EscenariocodiParaGams == escenarioGeneracion && m.Pfrentid == idGeneracion);

                    if (objRGE != null)
                    {
                        regResultadoGams.Pfresccodi = escenarioGeneracion;
                        regResultadoGams.Pfrrgecodi = objRGE.Pfrentcodi;
                        regResultadoGams.Pfrrgtipo = tipoResultadoGams;
                        regResultadoGams.Pfrrgresultado = reg.Valor;
                    }

                }
                else
                {
                    if (tipoResultadoGams == (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.V)
                    {
                        var idBarra = reg.Id;
                        var escenarioBarra = reg.Escenariocodi;

                        PfrEntidadDTO objBarra = listaBarrasTotal.Find(m => m.EscenariocodiParaGams == escenarioBarra && m.Pfrentid == idBarra);

                        if (objBarra != null)
                        {
                            regResultadoGams.Pfresccodi = escenarioBarra;
                            regResultadoGams.Pfreqpcodi = objBarra.Pfrentcodi;
                            regResultadoGams.Pfrrgtipo = tipoResultadoGams;
                            regResultadoGams.Pfrrgresultado = reg.Valor;
                        }

                    }
                    else
                    {
                        if (tipoResultadoGams == (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Congestion)
                        {
                            var idCongestion = reg.Id;
                            var escenarioCongestion = reg.Escenariocodi;

                            PfrEntidadDTO objCongestion = listaCongestionTotal.Find(m => m.EscenariocodiParaGams == escenarioCongestion && m.Pfrentid == idCongestion);

                            if (objCongestion != null)
                            {
                                regResultadoGams.Pfresccodi = escenarioCongestion;
                                regResultadoGams.Pfrcgtcodi = objCongestion.Pfrentcodi;
                                regResultadoGams.Pfrrgtipo = tipoResultadoGams;
                                regResultadoGams.Pfrrgresultado = reg.Valor;
                            }

                        }
                    }
                }

                if (regResultadoGams.Pfrrgtipo != null)
                    lstDeResultados.Add(regResultadoGams);
            }
            lstResultados = lstDeResultados;

            //Funcion transaccional para guardar Resultados en BD
            int resultado = this.GuardarResultadosGamsBDTransaccional(lstDeResultados);

            return resultado;
        }

        /// <summary>
        /// Guarda registro ResultadoGams en la BD
        /// </summary>
        /// <param name="lstResultados"></param>
        /// <returns></returns>
        private int GuardarResultadosGamsBDTransaccional(List<PfrResultadosGamsDTO> lstResultados)
        {
            int resultado = -1;

            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = (DbTransaction)UoW.StartTransaction(connection))
                {
                    try
                    {
                        foreach (PfrResultadosGamsDTO regResultado in lstResultados)
                        {
                            this.SavePfrResultadosGams(regResultado, connection, transaction);

                        }

                        transaction.Commit();
                        resultado = 1;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Guarda en base de datos los reportes, escenarios y reportes_totales
        /// </summary>
        /// <param name="regReporte"></param>
        /// <param name="lstCodisForeyKey"></param>
        /// <returns></returns>
        private int GuardarReportePFR_BDTransaccional(PfrReporteDTO regReporte, int?[] lstCodisForeyKey)
        {
            int pfrrptcodi = -1;
            int pfresccodi = -1;

            //int pos = 0;

            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = (DbTransaction)UoW.StartTransaction(connection))
                {
                    try
                    {
                        pfrrptcodi = FactorySic.GetPfrReporteRepository().Save(regReporte, connection, transaction);

                        foreach (PfrEscenarioDTO escenario in regReporte.ListaPfrEscenario)
                        {
                            escenario.Pfrrptcodi = pfrrptcodi;

                            pfresccodi = FactorySic.GetPfrEscenarioRepository().Save(escenario, connection, transaction);

                            foreach (PfrReporteTotalDTO reporteTotal in escenario.ListaPfrReporteTotal)
                            {
                                reporteTotal.Pfresccodi = pfresccodi;
                                int pfrtotcodi = FactorySic.GetPfrReporteTotalRepository().Save(reporteTotal, connection, transaction);

                                //pos++;

                            }
                        }

                        #region Save Relaciones con modulos PF e Indisponibilidades (mes anterior)                        
                        var indRptcodiPF = lstCodisForeyKey[0];
                        var indRptcodiFK = lstCodisForeyKey[1];
                        var indRptcodiCRTFortuito = lstCodisForeyKey[2];
                        var indRptcodiCRTProgramado = lstCodisForeyKey[3];
                        var indRptcodiCRH = lstCodisForeyKey[4];

                        //guardar tabla relación IND (FK)                                               
                        PfrRelacionIndisponibilidadDTO relacionIndFK = new PfrRelacionIndisponibilidadDTO()
                        {
                            Pfrrptcodi = pfrrptcodi,
                            Irptcodi = indRptcodiFK,
                            Pfrrintipo = (int)ConstantesPotenciaFirmeRemunerable.TipoRelacionInd.FactorK
                        };
                        this.SavePfrRelacionIndisponibilidad(relacionIndFK, connection, transaction);

                        //guardar tabla relación IND (CRTFortuito)                                               
                        PfrRelacionIndisponibilidadDTO relacionIndCRTF = new PfrRelacionIndisponibilidadDTO()
                        {
                            Pfrrptcodi = pfrrptcodi,
                            Irptcodi = indRptcodiCRTFortuito,
                            Pfrrintipo = (int)ConstantesPotenciaFirmeRemunerable.TipoRelacionInd.CRTermoFortuita
                        };
                        this.SavePfrRelacionIndisponibilidad(relacionIndCRTF, connection, transaction);

                        //guardar tabla relación IND (CRTProgramado)                                               
                        PfrRelacionIndisponibilidadDTO relacionIndCRTP = new PfrRelacionIndisponibilidadDTO()
                        {
                            Pfrrptcodi = pfrrptcodi,
                            Irptcodi = indRptcodiCRTProgramado,
                            Pfrrintipo = (int)ConstantesPotenciaFirmeRemunerable.TipoRelacionInd.CRTermoProgramada
                        };
                        this.SavePfrRelacionIndisponibilidad(relacionIndCRTP, connection, transaction);


                        //guardar tabla relación IND (CRH)                                               
                        PfrRelacionIndisponibilidadDTO relacionIndCRH = new PfrRelacionIndisponibilidadDTO()
                        {
                            Pfrrptcodi = pfrrptcodi,
                            Irptcodi = indRptcodiCRH,
                            Pfrrintipo = (int)ConstantesPotenciaFirmeRemunerable.TipoRelacionInd.CRHidro
                        };
                        this.SavePfrRelacionIndisponibilidad(relacionIndCRH, connection, transaction);

                        //guardar tabla relación PF                                                
                        PfrRelacionPotenciaFirmeDTO relacionPF = new PfrRelacionPotenciaFirmeDTO()
                        {
                            Pfrrptcodi = pfrrptcodi,
                            Pfrptcodi = indRptcodiPF
                        };
                        this.SavePfrRelacionPotenciaFirme(relacionPF, connection, transaction);

                        #endregion

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return pfrrptcodi;
        }

        #endregion

        #endregion

        #region Reportes del Cálculo de PFR

        /// <summary>
        /// Devuelve el codigo del último reporte de potencia firme remunerable
        /// </summary>
        /// <param name="pfrreccodi"></param>
        /// <param name="cuadro"></param>
        /// <returns></returns>
        public int GetUltimoPfrrptcodiXRecalculo(int pfrreccodi, int cuadro)
        {
            if (pfrreccodi > 0)
            {
                List<PfrReporteDTO> lista = GetByCriteriaPfrReportes(pfrreccodi, cuadro).OrderByDescending(x => x.Pfrrptcodi).ToList();
                if (lista.Any())
                    return lista.First().Pfrrptcodi;
            }
            return 0;
        }

        /// <summary>
        /// Generar archivo excel por versión reporte Reporte LVTP
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="irptcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nameFile"></param>
        public void GenerarArchivoExcelPFR(string ruta, int tipo, int pfrreccodi, out string nameFile)
        {
            var pfrrptcodi = GetUltimoPfrrptcodiXRecalculo(pfrreccodi, tipo);
            var pfrrptcodigeneral = GetUltimoPfrrptcodiXRecalculo(pfrreccodi, (int)ConstantesPotenciaFirmeRemunerable.ExcelReporteLVTP.Aux2);

            PfrReporteDTO regReporte = GetByIdPfrReporte(pfrrptcodigeneral);
            PfrRecalculoDTO regRecalculo = GetByIdPfrRecalculo(regReporte.Pfrreccodi.Value);
            PfrPeriodoDTO regPeriodo = GetByIdPfrPeriodo(regRecalculo.Pfrpercodi);

            PfrRelacionPotenciaFirmeDTO regRelacionPF = GetByCriteriaPfrRelacionPotenciaFirmes(regReporte.Pfrrptcodi).FirstOrDefault();

            DateTime fechaIni = new DateTime(regPeriodo.Pfrperanio, regPeriodo.Pfrpermes, 1);

            bool esExportarPF = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelReporteLVTP.C8;
            bool esExportarAUX1 = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelReporteLVTP.Aux1;
            bool esExportarAUX2 = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelReporteLVTP.Aux2;
            bool esExportarDatos = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelReporteLVTP.Datos;
            bool esExportarRelCuadro = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelReporteLVTP.Relacion;

            bool esExportarReporteLvtp = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelReporteLVTP.ReporteLvtp;

            nameFile = string.Format("PFR_{0}_{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000);

            nameFile = esExportarRelCuadro ? string.Format("Cuadros_{0}_{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarPF ? string.Format("C8_{0}_{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarAUX1 ? string.Format("Aux1_{0}_{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarAUX2 ? string.Format("Aux2_{0}_{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarDatos ? string.Format("Datos_{0}_{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarReporteLvtp ? string.Format("Reporte_LVTP_Unificado_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                if (esExportarRelCuadro)
                {
                    GenerarHojaExcelRelacionCuadros(xlPackage, regReporte, regRecalculo, regPeriodo);
                    xlPackage.Save();
                }

                if (esExportarPF)
                {
                    GenerarHojaExcelReportePF(xlPackage, regReporte, pfrrptcodi, regRecalculo, regPeriodo);
                    xlPackage.Save();
                }
                if (esExportarAUX1)
                {
                    GenerarArchivoExcelAUX1(xlPackage, regReporte, pfrrptcodi, regRelacionPF);
                    xlPackage.Save();
                }
                if (esExportarAUX2)
                {
                    GenerarArchivoExcelAUX2(xlPackage, pfrrptcodi);
                    xlPackage.Save();
                }


                if (esExportarDatos)
                {
                    GenerarHojaExcelReporteDatos(xlPackage, pfrrptcodi, regPeriodo);
                    xlPackage.Save();
                }

                if (esExportarReporteLvtp)
                {
                    var pfrrptcodiC8 = GetUltimoPfrrptcodiXRecalculo(pfrreccodi, (int)ConstantesPotenciaFirmeRemunerable.ExcelReporteLVTP.C8);
                    var pfrrptcodiAux1 = GetUltimoPfrrptcodiXRecalculo(pfrreccodi, (int)ConstantesPotenciaFirmeRemunerable.ExcelReporteLVTP.Aux1);
                    var pfrrptcodiDatos = GetUltimoPfrrptcodiXRecalculo(pfrreccodi, (int)ConstantesPotenciaFirmeRemunerable.ExcelReporteLVTP.Datos);

                    GenerarHojaExcelRelacionCuadros(xlPackage, regReporte, regRecalculo, regPeriodo);
                    GenerarHojaExcelReportePF(xlPackage, regReporte, pfrrptcodiC8, regRecalculo, regPeriodo);
                    GenerarArchivoExcelAUX1(xlPackage, regReporte, pfrrptcodiAux1, regRelacionPF);
                    GenerarArchivoExcelAUX2(xlPackage, pfrrptcodigeneral);
                    GenerarHojaExcelReporteDatos(xlPackage, pfrrptcodiDatos, regPeriodo);

                    xlPackage.Save();
                }
            }
        }

        #region Hojas Reporte LVTP

        private void GenerarHojaExcelRelacionCuadros(ExcelPackage xlPackage, PfrReporteDTO regReporte, PfrRecalculoDTO regRecalculo, PfrPeriodoDTO regPeriodo)
        {
            ExcelWorksheet ws = null;
            string nameWS = "Relacion";

            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            //ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Relacion");
            //ws = xlPackage.Workbook.Worksheets["Relacion"];

            string fontFamily = "Arial";
            var fuenteTabla = new Font(fontFamily, 8);
            var fuenteInforme = new Font(fontFamily, 16);
            var fuenteTitulo = new Font(fontFamily, 18);
            var fuentePerido = new Font(fontFamily, 14);
            var fuenteSubTit = new Font(fontFamily, 9);
            var fuentePie = new Font(fontFamily, 7);

            var listaRelacionCuadro = new List<dynamic>
            {
                new { Cuadro = "CUADRO", Procedimiento = "PROCEDIMIENTO", Titulo = "TITULO Y CONCEPTO" },
                new { Cuadro = "C1 - C3", Procedimiento = "30", Titulo = "CUADROS DE PAGO A LOS INTEGRANTES TRANSMISORES Y PAGOS ENTRE PARTICIPANTES DEL MME" },
                new { Cuadro = "C5", Procedimiento = "30", Titulo = "INFORMACIÓN DE CONSUMOS INGRESADA PARA VTP Y PEAJES" },
                new { Cuadro = "C6.1", Procedimiento = "30", Titulo = "RESUMEN DE INFORMACIÓN VTP" },
                new { Cuadro = "C6.2", Procedimiento = "Reg. MME ", Titulo = "ASIGNACIÓN DE CONSUMOS EN EL MCP NO CUBIERTOS" },
                new { Cuadro = "C7", Procedimiento = "30", Titulo = "EGRESO POR COMPRA DE POTENCIA" },
                new { Cuadro = "C8", Procedimiento = "30", Titulo = "DETERMINACIÓN DE LA POTENCIA FIRME" },
                new { Cuadro = "C9", Procedimiento = "30", Titulo = "INGRESOS POR POTENCIA POR GENERADOR INTEGRANTE Y CENTRAL DE GENERACIÓN" },
                new { Cuadro = "C10", Procedimiento = "30", Titulo = "SALDOS VTP" },

            };


            #region Cabecera

            int rowIni = 12;
            int colIni = 1;

            int colCuadro = colIni;
            int colProcedimiento = colCuadro + 1;
            int colTitulo = colProcedimiento + 1;

            ws.Column(colCuadro).SetTrueColumnWidth(18.5);
            ws.Column(colProcedimiento).SetTrueColumnWidth(14.17);
            ws.Column(colTitulo).SetTrueColumnWidth(127.50);

            var rangoInforme = ws.Cells[rowIni, colCuadro];
            rangoInforme.Value = $"{regRecalculo.Pfrrecinforme}-{regRecalculo.Pfrrectipo}";
            rangoInforme.SetFont(fuenteInforme);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni, colCuadro, rowIni, colTitulo);

            var rangoTitulo = ws.Cells[rowIni + 1, colCuadro];
            rangoTitulo.Value = "RELACIÓN DE CUADROS";
            rangoTitulo.SetFont(fuenteTitulo);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 1, colCuadro, rowIni + 1, colTitulo);

            var rangoPeriodo = ws.Cells[rowIni + 4, colCuadro];
            rangoPeriodo.Value = $"{regPeriodo.Pfrpernombre}-{regRecalculo.Pfrrectipo}";
            rangoPeriodo.SetFont(fuentePerido);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 4, colCuadro, rowIni + 4, colTitulo);

            var rangoSubTit = ws.Cells[rowIni + 6, colCuadro];
            rangoSubTit.Value = "RELACIÓN DE CUADROS DE LA LIQUIDACION DE LA VALORIZACIÓN DE LAS TRANSFERENCIAS DE POTENCIA";
            rangoSubTit.SetFont(fuenteSubTit);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 6, colCuadro, rowIni + 6, colTitulo);

            var rangeCab = ws.Cells[rowIni, colCuadro, rowIni + 7, colTitulo];
            rangeCab.SetFontBold();
            rangeCab.SetAlignment();

            #endregion

            #region Cuerpo

            var rowIniCuerpo = 20;
            var rowIniTabla = rowIniCuerpo;

            foreach (var item in listaRelacionCuadro)
            {
                ws.Cells[rowIniTabla, colCuadro].Value = item.Cuadro;
                ws.Cells[rowIniTabla, colProcedimiento].Value = item.Procedimiento;
                ws.Cells[rowIniTabla, colTitulo].Value = item.Titulo;
                rowIniTabla++;
            }

            UtilExcel.AllBorders(ws.Cells[rowIniCuerpo, colCuadro, rowIniCuerpo + 1, colTitulo]);
            UtilExcel.BorderAround(ws.Cells[rowIniCuerpo + 2, colCuadro, rowIniTabla, colTitulo]);
            UtilExcel.BorderAround(ws.Cells[rowIniCuerpo, colProcedimiento, rowIniTabla, colProcedimiento]);

            ExcelRange rangoTabla = ws.Cells[rowIniCuerpo, colCuadro, rowIniTabla, colTitulo];
            rangoTabla.Style.Font.SetFromFont(fuenteTabla);
            UtilExcel.BorderAround(rangoTabla, OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            ExcelRange cabeceraTabla = ws.Cells[rowIniCuerpo, colCuadro, rowIniCuerpo, colTitulo];
            cabeceraTabla.SetFontBold();
            cabeceraTabla.SetBackgroundColor(ColorTranslator.FromHtml("#c0c0c0"));
            cabeceraTabla.SetAlignment();

            ws.Cells[rowIniCuerpo, colCuadro, rowIniTabla, colProcedimiento].SetAlignment();

            #endregion

            #region Pie

            int rowIniPie = 35;

            ws.Cells[rowIniPie, colCuadro].Value = "NUMERO DE INFORME :";
            ws.Cells[rowIniPie, colTitulo].Value = $"{regRecalculo.Pfrrecinforme}-{regRecalculo.Pfrrectipo}";

            ws.Cells[rowIniPie + 1, colCuadro].Value = "MES:";
            ws.Cells[rowIniPie + 1, colTitulo].Value = $"{regPeriodo.Pfrpernombre}-{regRecalculo.Pfrrectipo}";

            var maximaDemanda = MathHelper.Round(ConvertirMWaKw(regReporte.Pfrrptmd), 0);

            ws.Cells[rowIniPie + 2, colCuadro].Value = "MAXIMA DEMANDA :";
            ws.Cells[rowIniPie + 2, colTitulo].Value = maximaDemanda;
            ws.Cells[rowIniPie + 2, colTitulo].Style.Numberformat.Format = "#,##0.000";
            ws.Cells[rowIniPie + 3, colTitulo].Value = regReporte.Pfrrptfecmd?.ToString("dd MMMM yyyy HH:mm");

            ws.Cells[rowIniPie + 4, colCuadro].Value = "Dias del mes :";
            ws.Cells[rowIniPie + 4, colTitulo].Value = regPeriodo.FechaFin.Day;

            var ragoPie = ws.Cells[rowIniPie, colCuadro, rowIniPie + 4, colTitulo];
            ragoPie.SetFont(fuentePie);
            ragoPie.SetAlignment(ExcelHorizontalAlignment.Left);

            #endregion

            ws.View.ShowGridLines = false;
        }

        private void GenerarHojaExcelReportePF(ExcelPackage xlPackage, PfrReporteDTO regReporte, int pfrrptcodi, PfrRecalculoDTO regRecalculo, PfrPeriodoDTO regPeriodo)
        {
            List<PfrReporteTotalDTO> listadoGeneralDatos = ListPfrReporteTotalByReportecodi(pfrrptcodi).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();
            List<PfrEscenarioDTO> listaEscenario = ListPfrEscenariosByReportecodi(pfrrptcodi).OrderBy(x => x.Pfrescfecini).ToList();

            ExcelWorksheet ws = null;
            string nameWS = "C8";

            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            //var nameWS = "C8";

            //ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            //ws = xlPackage.Workbook.Worksheets[nameWS];

            string fontFamTitle = "Calibri";
            string fontFamily = "Arial";


            var fuenteTabla = new Font(fontFamily, 8);

            var fuenteTitulo = new Font(fontFamTitle, 16);
            var fuenteInforme = new Font(fontFamTitle, 11);

            int colIniTable = 5;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 56.33, 29.33, 30, 15, 14, 15, 14 })//columna E-K
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }
            ws.Row(3).Height = 48;

            #region  Filtros y Cabecera

            var colEmpresa = colIniTable;
            var colCentral = colEmpresa + 1;
            var colUnidad = colCentral + 1;
            int colIniEsc = colUnidad + 1;



            ws.Cells[3, colCentral].Value = "DETERMINACIÓN DE LA POTENCIA FIRME";
            ws.Cells[4, colCentral].Value = $"{regPeriodo.Pfrpernombre}-{regRecalculo.Pfrrectipo}";
            ws.Cells[3, colCentral, 4, colCentral].SetFont(fuenteTitulo);


            ws.Cells[6, colCentral].Value = $"{regRecalculo.Pfrrecinforme}-{regRecalculo.Pfrrectipo}";
            ws.Cells[7, colCentral].Value = "CUADRO N° 8";
            ws.Cells[6, colCentral, 7, colCentral].SetFont(fuenteInforme);

            ws.Cells[3, colCentral, 7, colCentral].SetFontBold();


            int rowIniTabla = 11;
            ws.Row(rowIniTabla).Height = 35;
            ws.Cells[rowIniTabla, colEmpresa].Value = "Empresa";
            ws.Cells[rowIniTabla, colCentral].Value = "Central";
            ws.Cells[rowIniTabla, colUnidad].Value = "Unidad";


            var colEscDynamic = colIniEsc;
            foreach (var reg in listaEscenario)
            {
                var rowCab = rowIniTabla - 3;
                ws.Cells[rowCab, colEscDynamic].Value = reg.FechaDesc;
                ws.Cells[rowCab, colEscDynamic, rowCab + 2, colEscDynamic + 1].Merge = true;
                UtilExcel.BorderAround(ws.Cells[rowCab, colEscDynamic, rowCab + 2, colEscDynamic + 1], ExcelBorderStyle.Medium);


                ws.Cells[rowIniTabla, colEscDynamic].Value = "Pot. Efectiva \n (kW)";
                ws.Cells[rowIniTabla, colEscDynamic + 1].Value = "Pot. Firme \n (kW)";

                colEscDynamic += 2;
            }

            var rangoCab = ws.Cells[rowIniTabla - 3, colEmpresa, rowIniTabla, colEscDynamic];
            rangoCab.SetAlignment();
            rangoCab.Style.WrapText = true;

            UtilExcel.AllBorders(ws.Cells[rowIniTabla, colEmpresa, rowIniTabla, colEmpresa]);
            UtilExcel.AllBorders(ws.Cells[rowIniTabla, colIniEsc, rowIniTabla, colEscDynamic - 1]);
            UtilExcel.BorderAround(ws.Cells[rowIniTabla, colEmpresa, rowIniTabla, colEscDynamic - 1], ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;
            int rowIniRangoEmpresa = rowData;

            foreach (var itemGp in listadoGeneralDatos.GroupBy(x => x.Emprcodi))
            {
                var rowEmpIni = rowData;
                foreach (var reg in itemGp.GroupBy(x => new { x.Equipadre, x.Equicodi, x.Grupocodi }))
                {
                    var regis = reg.First();
                    ws.Cells[rowData, colEmpresa].Value = regis.Emprnomb;
                    ws.Cells[rowData, colCentral].Value = regis.Central;
                    ws.Cells[rowData, colUnidad].Value = regis.Pfrtotunidadnomb;

                    colEscDynamic = colIniEsc;
                    foreach (var regEsc in listaEscenario)
                    {

                        var dataEsc = reg.FirstOrDefault(x => x.Pfresccodi == regEsc.Pfresccodi);

                        ws.Cells[rowData, colEscDynamic].Value = dataEsc?.Pfrtotpe;
                        ws.Cells[rowData, colEscDynamic + 1].Value = dataEsc?.Pfrtotpf;

                        ws.Cells[rowData, colEscDynamic, rowData, colEscDynamic + 1].Style.Numberformat.Format = "_ * #,##0_ ;_ * -#,##0_ ;_ * \" - \"_ ;_ @_ ";

                        colEscDynamic += 2;
                    }

                    rowData++;
                }
                UtilExcel.BorderAround(ws.Cells[rowEmpIni, colEmpresa, rowData - 1, colEscDynamic - 1]);
            }

            UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colEmpresa, rowData - 1, colEmpresa]);
            colEscDynamic = colIniEsc;
            foreach (var reg in listaEscenario)
            {
                UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colEscDynamic, rowData, colEscDynamic]);
                colEscDynamic += 2;
            }

            #endregion

            #region Total

            var rangotabla = ws.Cells[rowIniTabla - 3, colEmpresa, rowData, colEscDynamic];
            rangotabla.SetFont(fuenteTabla);

            ws.Cells[rowData, colEmpresa].Value = "TOTAL";
            ws.Cells[rowData, colEmpresa].SetFontBold();
            colEscDynamic = colIniEsc;
            foreach (var regEsc in listaEscenario)
            {
                decimal? valorPf = listadoGeneralDatos.Where(x => x.Pfresccodi == regEsc.Pfresccodi).Sum(x => x.Pfrtotpf ?? 0);
                decimal? valorPe = listadoGeneralDatos.Where(x => x.Pfresccodi == regEsc.Pfresccodi).Sum(x => x.Pfrtotpe ?? 0);

                ws.Cells[rowData, colEscDynamic].Value = valorPe;
                ws.Cells[rowData, colEscDynamic + 1].Value = valorPf;
                ws.Cells[rowData, colEscDynamic, rowData, colEscDynamic + 1].Style.Numberformat.Format = "_ * #,##0_ ;_ * -#,##0_ ;_ * \" - \"_ ;_ @_ "; ;
                ws.Cells[rowData, colEscDynamic, rowData, colEscDynamic + 1].SetFontBold();

                colEscDynamic += 2;
            }

            UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colEmpresa, rowData, colEscDynamic - 1], ExcelBorderStyle.Medium);

            #endregion

            #region Pie


            decimal margenReserva = regReporte.Pfrrptmr.GetValueOrDefault(0) / 100;

            var rowPie = rowData + 2;



            ws.Cells[rowPie, colEmpresa].Value = "Margen de Reserva(**) :";
            ws.Cells[rowPie + 1, colEmpresa].Value = "Máxima Demanda (kW) :";
            ws.Cells[rowPie + 2, colEmpresa].Value = "Reserva(kW) :";
            ws.Cells[rowPie + 3, colEmpresa].Value = "Máxima Demanda + Reserva(kW):";
            ws.Cells[rowPie + 4, colEmpresa].Value = "Potencia Efectiva Total(kW) :";
            ws.Cells[rowPie + 5, colEmpresa].Value = "CONDICION : Máxima Demanda + Reserva < Potencia Efectiva total ";

            colEscDynamic = colIniEsc;
            var reserva = margenReserva * ConvertirMWaKw(regReporte.Pfrrptmd) ?? 0;
            var maxDemReserv = reserva + ConvertirMWaKw(regReporte.Pfrrptmd) ?? 0;
            foreach (var regEsc in listaEscenario)
            {
                ws.Cells[rowPie, colEscDynamic].Value = margenReserva;
                ws.Cells[rowPie, colEscDynamic].Style.Numberformat.Format = "0.0%";

                ws.Cells[rowPie + 1, colEscDynamic].Value = ConvertirMWaKw(regReporte.Pfrrptmd);
                ws.Cells[rowPie + 2, colEscDynamic].Value = reserva;
                ws.Cells[rowPie + 3, colEscDynamic].Value = maxDemReserv;

                decimal? valorPe = listadoGeneralDatos.Where(x => x.Pfresccodi == regEsc.Pfresccodi).Sum(x => x.Pfrtotpe ?? 0);
                ws.Cells[rowPie + 4, colEscDynamic].Value = valorPe;

                var condicion = valorPe <= regReporte.Pfrrptmd ? "No Ejecutar Flujo Óptimo" : "Ejecutar Flujo Óptimo";
                ws.Cells[rowPie + 5, colEscDynamic].Value = condicion;
                ws.Cells[rowPie + 5, colEscDynamic].SetFontColor(Color.Red);

                ws.Cells[rowPie + 1, colEscDynamic, rowPie + 4, colEscDynamic + 1].Style.Numberformat.Format = "_ * #,##0_ ;_ * -#,##0_ ;_ * \" - \"_ ;_ @_ "; ;

                colEscDynamic += 2;
            }

            UtilExcel.BorderAround(ws.Cells[rowPie, colEmpresa, rowPie + 4, colEscDynamic - 1]);
            UtilExcel.BorderAround(ws.Cells[rowPie + 5, colEmpresa, rowPie + 5, colEscDynamic - 1]);
            UtilExcel.BorderAround(ws.Cells[rowPie, colIniEsc, rowPie + 5, colIniEsc + 1]);
            ws.Cells[rowPie, colEmpresa, rowPie + 5, colEscDynamic - 1].SetFont(fuenteTabla);

            #endregion


            ws.View.FreezePanes(rowIniTabla + 1, 1);

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            ws.View.ZoomScale = 100;

            //filter
        }

        public void GenerarArchivoExcelAUX1(ExcelPackage xlPackage, PfrReporteDTO regReporte, int pfrrptcodi, PfrRelacionPotenciaFirmeDTO regRelacionPF)
        {
            if (regRelacionPF == default(PfrRelacionPotenciaFirmeDTO)) throw new ArgumentException("No existe relación con Potencia Firme");

            List<PfrReporteTotalDTO> listadoGeneralDatos = ListPfrReporteTotalByReportecodi(pfrrptcodi).OrderBy(x => x.Pfrtotcv).ThenBy(x => x.Central).ThenBy(x => x.Pfrtotunidadnomb).ToList();
            List<PfrEscenarioDTO> listaEscenario = ListPfrEscenariosByReportecodi(pfrrptcodi).OrderBy(x => x.Pfrescfecini).ToList();

            ExcelWorksheet ws = null;

            char inicio = 'A';
            foreach (var regEsc in listaEscenario)
            {
                var listaPfrxEsc = listadoGeneralDatos.Where(x => x.Pfresccodi == regEsc.Pfresccodi).ToList();
                string nameWS = "AUX1-" + inicio;
                inicio++;
                this.GenerarHojaExcelReporteAUX1(ref ws, xlPackage, nameWS, regReporte, regEsc, listaPfrxEsc);
            }
        }

        private void GenerarHojaExcelReporteAUX1(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS, PfrReporteDTO regReporte, PfrEscenarioDTO pfrEscenario, List<PfrReporteTotalDTO> listaPF)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string fontFamTitle = "Calibri";
            string fontFamily = "Arial";

            var fuenteTabla = new Font(fontFamily, 9);

            var fuenteTitulo = new Font(fontFamTitle, 16);
            var fuenteInforme = new Font(fontFamTitle, 11);

            int colIniTable = 6;
            int colIniDynamic = colIniTable;

            //cálculo del racionamiento actual

            foreach (var columnWidth in new List<double>() { 30, 30, 15, 15, 20, 14, 15, 20, 15, 20 })//columna F-K
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }
            //ws.Row(3).Height = 48;

            #region  Filtros y Cabecera

            var colCentral = colIniTable;
            var colUnidad = colCentral + 1;
            int colCostoV = colUnidad + 1;
            int colPotEfectiva = colCostoV + 1;
            int colPotEfecAcum = colPotEfectiva + 1;
            int colFactorIngreso = colPotEfecAcum + 1;
            int colPotFirme = colFactorIngreso + 1;
            int colPotFirmeColoc = colPotFirme + 1;
            int colPotDisponible = colPotFirmeColoc + 1;
            int colCostoVFlujo = colPotDisponible + 1;

            ws.Cells[6, colCentral].Value = "FACTOR DE RESERVA FIRME Y POTENCIA DISPONIBLE";
            ws.Cells[6, colCentral, 6, colCentral].SetFont(fuenteTitulo);
            //ws.Cells[6, colCentral, 6, colCentral].SetFontBold();
            ws.Cells[6, colCentral].SetFontBold();

            int rowIniTabla = 8;
            ws.Row(rowIniTabla).Height = 45;
            ws.Cells[rowIniTabla, colCentral].Value = "Central";
            ws.Cells[rowIniTabla, colUnidad].Value = "Unidad";
            ws.Cells[rowIniTabla, colCostoV].Value = "Costo Variable \n (S/./KWh)";
            ws.Cells[rowIniTabla, colPotEfectiva].Value = "Potencia Efectiva \n (kW)";
            ws.Cells[rowIniTabla, colPotEfecAcum].Value = "Potencia Efectiva Acumulada \n (kW)";
            ws.Cells[rowIniTabla, colFactorIngreso].Value = "Factor de Ingreso";
            ws.Cells[rowIniTabla, colPotFirme].Value = "Potencia Firme \n (kW)";
            ws.Cells[rowIniTabla, colPotFirmeColoc].Value = "Potencia Firme Colocada \n (kW)";
            ws.Cells[rowIniTabla, colPotDisponible].Value = "Potencia Disponible \n (kW)";
            ws.Cells[rowIniTabla, colCostoVFlujo].Value = "Costo Variable para Flujo \n (kW)";

            var rangoCab = ws.Cells[rowIniTabla - 1, colCentral, rowIniTabla, colCostoVFlujo];
            rangoCab.SetAlignment();
            rangoCab.Style.WrapText = true;

            UtilExcel.AllBorders(ws.Cells[rowIniTabla, colCostoV, rowIniTabla, colCostoV]);
            UtilExcel.AllBorders(ws.Cells[rowIniTabla, colFactorIngreso, rowIniTabla, colFactorIngreso]);
            UtilExcel.AllBorders(ws.Cells[rowIniTabla, colPotDisponible, rowIniTabla, colPotDisponible]);
            UtilExcel.BorderAround(ws.Cells[rowIniTabla, colCentral, rowIniTabla, colCostoVFlujo], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;
            int rowIniRangoEmpresa = rowData;

            decimal margenReserva = regReporte.Pfrrptmr.GetValueOrDefault(0) / 100;
            var maximaDemanda = ConvertirMWaKw(regReporte.Pfrrptmd).Round(0);

            var reserva = margenReserva * maximaDemanda ?? 0;
            var maxDemReserv = reserva + maximaDemanda ?? 0;

            //Calcular suma de potencia firme colocada
            decimal? sumaColocada = listaPF.Sum(x => x.Pfrtotpfc ?? 0);
            decimal? sumaPotDisponiible = 0;


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //Ordenar por costo varible, central y unidad

            decimal? factorRFirme = sumaColocada / maximaDemanda;

            foreach (var item in listaPF)
            {
                ws.Cells[rowData, colCentral].Value = item.Central;
                ws.Cells[rowData, colUnidad].Value = item.Pfrtotunidadnomb;

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                ws.Cells[rowData, colCostoV].Value = item.Pfrtotcv;
                ws.Cells[rowData, colCostoV, rowData, colCostoV].Style.Numberformat.Format = "#,##0.00000";


                decimal? valorPf = item.Pfrtotpf.GetValueOrDefault(0);
                decimal? valorPe = item.Pfrtotpe.GetValueOrDefault(0);

                ws.Cells[rowData, colPotEfectiva].Value = valorPe;
                ws.Cells[rowData, colPotEfecAcum].Value = item.Pfrtotpea;
                ws.Cells[rowData, colPotEfectiva, rowData, colPotEfecAcum].Style.Numberformat.Format = "_ * #,##0_ ;_ * -#,##0_ ;_ * \" - \"_ ;_ @_ ";

                //factor ingreso

                ws.Cells[rowData, colFactorIngreso].Value = item.Pfrtotfi;
                ws.Cells[rowData, colFactorIngreso, rowData, colFactorIngreso].Style.Numberformat.Format = "#,##0.000";

                //potencia firme
                ws.Cells[rowData, colPotFirme].Value = valorPf;

                //potencia firme colocada
                ws.Cells[rowData, colPotFirmeColoc].Value = item.Pfrtotpfc;

                //potencia disponible 
                decimal? pDisponible = factorRFirme.HasValue ? valorPf / factorRFirme.Value : null;
                ws.Cells[rowData, colPotDisponible].Value = pDisponible;

                ws.Cells[rowData, colPotFirme, rowData, colPotDisponible].Style.Numberformat.Format = "_ * #,##0_ ;_ * -#,##0_ ;_ * \" - \"_ ;_ @_ ";

                sumaPotDisponiible += pDisponible;

                //Costo Variable Flujo
                ws.Cells[rowData, colCostoVFlujo].Value = item.Pfrtotcvf;
                ws.Cells[rowData, colCostoVFlujo, rowData, colCostoVFlujo].Style.Numberformat.Format = "#,##0.00000";

                //costo racionamiento
                ws.Cells[rowData, colCostoVFlujo + 1].Value = item.Pfrtotcrmesant == 1 ? "(*)" : string.Empty;
                //>>>>>>>>>>>>>>>>
                rowData++;
            }

            UtilExcel.BorderAround(ws.Cells[rowData, colCentral, rowData, colCostoVFlujo]);

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


            UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colCentral, rowData, colUnidad]);
            UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colPotEfectiva, rowData, colPotEfecAcum]);
            UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colPotFirme, rowData, colPotFirmeColoc]);
            UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colPotDisponible, rowData, colPotDisponible]);
            UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colCostoVFlujo, rowData, colCostoVFlujo]);

            var rangotabla = ws.Cells[rowIniTabla - 1, colCentral, rowData, colCostoVFlujo];
            rangotabla.SetFont(fuenteTabla);

            #region Total
            ws.Cells[rowData, colCentral].Value = "TOTAL";
            ws.Cells[rowData, colCentral].SetFontBold();

            //decimal? valorPfTot = (decimal?)regTotalPF.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpf + escenario.Numero).GetValue(regTotalPF, null);
            //decimal? valorPeTot = (decimal?)regTotalPF.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpe + escenario.Numero).GetValue(regTotalPF, null);

            decimal? valorPfTot = listaPF.Sum(x => x.Pfrtotpf ?? 0);
            decimal? valorPeTot = listaPF.Sum(x => x.Pfrtotpe ?? 0);

            ws.Cells[rowData, colPotEfectiva].Value = valorPeTot;
            ws.Cells[rowData, colPotFirme].Value = valorPfTot;

            //ws.Cells[rowData, colPotFirme, rowData, colPotDisponible].Style.Numberformat.Format = "#,##0.000";
            //ws.Cells[rowData, colPotFirme, rowData, colPotDisponible].SetFontBold();
            ws.Cells[rowData, colPotFirmeColoc].Value = sumaColocada;
            ws.Cells[rowData, colPotDisponible].Value = sumaPotDisponiible;
            //ws.Cells[rowData, colPotFirmeColoc].Value = valorPfTot;
            //ws.Cells[rowData, colPotDisponible].Value = valorPfTot;

            ws.Cells[rowData, colPotEfectiva, rowData, colPotDisponible].Style.Numberformat.Format = "_ * #,##0_ ;_ * -#,##0_ ;_ * \" - \"_ ;_ @_ ";
            ws.Cells[rowData, colPotEfectiva, rowData, colPotDisponible].SetFontBold();

            UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colCentral, rowData, colCostoVFlujo], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region MyRegion

            var rowPie = rowData + 3;

            ws.Cells[rowData + 2, colCentral].Value = "Datos :";
            ws.Cells[rowData + 2, colCentral].SetFontBold();
            ws.Cells[rowData + 3, colCentral].Value = "(*) Se asigna un Costo Variable igual al Costo de Racionamiento por exceder las tolerancias máximas de los factores de indisponibilidad en el mes anterior.";
            ws.Cells[rowData + 4, colCentral].Value = "(**) Por Incentivos a la Disponibilidad, se asignará un Costo Variable igual al Costo de Racionamiento a la fracción Indisponible de la unidad .";

            ws.Cells[rowPie, colCostoV].Value = "Máxima demanda (Nivel generación) :";
            ws.Cells[rowPie + 1, colCostoV].Value = "Máxima demanda (Nivel generación)  + Margen de Reserva :";
            ws.Cells[rowPie + 2, colCostoV].Value = "Potencia Firme Colocada Total (kW)";
            ws.Cells[rowPie + 3, colCostoV].Value = "Factor de Reserva Firme";

            ws.Cells[rowPie, colPotFirmeColoc].Value = maximaDemanda;
            ws.Cells[rowPie + 1, colPotFirmeColoc].Value = maxDemReserv;
            ws.Cells[rowPie + 2, colPotFirmeColoc].Value = pfrEscenario.Pfrescpfct;
            ws.Cells[rowPie + 3, colPotFirmeColoc].Value = pfrEscenario.Pfrescfrf;

            ws.Cells[rowPie, colPotFirmeColoc, rowPie + 2, colPotFirmeColoc].Style.Numberformat.Format = "_ * #,##0_ ;_ * -#,##0_ ;_ * \" - \"_ ;_ @_ "; ;

            //UtilExcel.BorderAround(ws.Cells[rowPie, colEmpresa, rowPie + 4, colEscDynamic - 1]);
            //UtilExcel.BorderAround(ws.Cells[rowPie + 5, colEmpresa, rowPie + 5, colEscDynamic - 1]);
            //UtilExcel.BorderAround(ws.Cells[rowPie, colIniEsc, rowPie + 5, colIniEsc + 1]);
            //ws.Cells[rowPie, colEmpresa, rowPie + 5, colEscDynamic - 1].SetFont(fuenteTabla);

            #endregion

            #endregion
            ws.View.FreezePanes(rowIniTabla + 1, 1);
            //No mostrar lineas
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;

        }

        private void GenerarArchivoExcelAUX2(ExcelPackage xlPackage, int pfrrptcodi)
        {
            List<PfrReporteTotalDTO> listadoGeneralDatos = ListPfrReporteTotalByReportecodi(pfrrptcodi).OrderBy(x => x.Central).ThenBy(x => x.Pfrtotunidadnomb).ToList();
            List<PfrEscenarioDTO> listaEscenario = ListPfrEscenariosByReportecodi(pfrrptcodi).OrderBy(x => x.Pfrescfecini).ToList();

            //var nameWS = "Aux2";

            //ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            //ws = xlPackage.Workbook.Worksheets[nameWS];

            ExcelWorksheet ws = null;
            string nameWS = "Aux2";

            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string fontFamTitle = "Calibri";
            string fontFamily = "Arial";


            var fuenteTabla = new Font(fontFamily, 8);

            var fuenteTitulo = new Font(fontFamTitle, 16);
            var fuenteInforme = new Font(fontFamTitle, 11);

            int colIniTable = 5;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 40, 27 })//columna E-K
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }
            ws.Row(3).Height = 48;

            #region  Filtros y Cabecera

            var colCentral = colIniTable;
            var colUnidad = colCentral + 1;
            int colIniEsc = colUnidad + 1;

            ws.Cells[6, colCentral].Value = "POTENCIA FIRME REMUNERABLE";

            int rowIniTabla = 11;
            ws.Row(rowIniTabla).Height = 35;

            ws.Cells[rowIniTabla - 3, colCentral, rowIniTabla, colCentral].Merge = true;
            ws.Cells[rowIniTabla - 3, colUnidad, rowIniTabla, colUnidad].Merge = true;

            ws.Cells[rowIniTabla - 3, colCentral].Value = "Central";
            ws.Cells[rowIniTabla - 3, colUnidad].Value = "Unidad";

            UtilExcel.BorderAround(ws.Cells[rowIniTabla - 3, colCentral, rowIniTabla, colUnidad], ExcelBorderStyle.Medium);


            var colEscDynamic = colIniEsc;
            foreach (var reg in listaEscenario)
            {
                var rowCab = rowIniTabla - 3;
                ws.Cells[rowCab, colEscDynamic].Value = reg.FechaDesc;
                ws.Cells[rowCab, colEscDynamic, rowCab + 2, colEscDynamic + 3].Merge = true;

                UtilExcel.BorderAround(ws.Cells[rowCab, colEscDynamic, rowCab + 2, colEscDynamic + 3], ExcelBorderStyle.Medium);
                UtilExcel.BorderAround(ws.Cells[rowIniTabla, colEscDynamic, rowIniTabla, colEscDynamic + 3], ExcelBorderStyle.Medium);

                ws.Column(colEscDynamic).SetTrueColumnWidth(17.5);
                ws.Cells[rowIniTabla, colEscDynamic++].Value = "Potencia\nDisponible\n(kW)";

                ws.Column(colEscDynamic).SetTrueColumnWidth(15);
                ws.Cells[rowIniTabla, colEscDynamic++].Value = "Pot. Disponible\nDespachada\n(kW)";

                ws.Column(colEscDynamic).SetTrueColumnWidth(17.5);
                ws.Cells[rowIniTabla, colEscDynamic++].Value = "Pot. Firme\n(kW)";

                ws.Column(colEscDynamic).SetTrueColumnWidth(16);
                ws.Cells[rowIniTabla, colEscDynamic++].Value = "Pot. Firme\nRemunerable\n (kW)";

            }

            var rangoCab = ws.Cells[rowIniTabla - 3, colCentral, rowIniTabla, colEscDynamic];
            rangoCab.SetAlignment();
            rangoCab.Style.WrapText = true;

            //UtilExcel.AllBorders(ws.Cells[rowIniTabla, colCentral, rowIniTabla, colCentral]);
            //UtilExcel.AllBorders(ws.Cells[rowIniTabla, colIniEsc, rowIniTabla, colEscDynamic - 1]);
            //UtilExcel.BorderAround(ws.Cells[rowIniTabla, colCentral, rowIniTabla, colEscDynamic - 1], ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;
            int rowIniRangoEmpresa = rowData;

            foreach (var itemGp in listadoGeneralDatos.GroupBy(x => x.Emprcodi))
            {
                var rowEmpIni = rowData;
                foreach (var reg in itemGp.GroupBy(x => new { x.Equipadre, x.Equicodi, x.Grupocodi, x.Pfrtotunidadnomb }))
                {
                    var regis = reg.First();
                    ws.Cells[rowData, colCentral].Value = regis.Central;
                    ws.Cells[rowData, colUnidad].Value = regis.Pfrtotunidadnomb;

                    colEscDynamic = colIniEsc;
                    foreach (var regEsc in listaEscenario)
                    {

                        var dataEsc = reg.FirstOrDefault(x => x.Pfresccodi == regEsc.Pfresccodi);
                        ws.Cells[rowData, colEscDynamic, rowData, colEscDynamic + 4].Style.Numberformat.Format = "_ * #,##0_ ;_ * -#,##0_ ;_ * \" - \"_ ;_ @_ ";

                        ws.Cells[rowData, colEscDynamic++].Value = dataEsc?.Pfrtotpd;
                        ws.Cells[rowData, colEscDynamic++].Value = dataEsc?.Pfrtotpdd;
                        ws.Cells[rowData, colEscDynamic++].Value = dataEsc?.Pfrtotpf;
                        ws.Cells[rowData, colEscDynamic++].Value = dataEsc?.Pfrtotpfr;

                    }

                    rowData++;
                }
                UtilExcel.BorderAround(ws.Cells[rowEmpIni, colCentral, rowData - 1, colEscDynamic - 1]);
            }

            colEscDynamic = colIniEsc;
            foreach (var reg in listaEscenario)
            {
                UtilExcel.BorderAround(ws.Cells[rowIniTabla, colEscDynamic, rowData, colEscDynamic + 3], ExcelBorderStyle.Medium);

                UtilExcel.BorderAround(ws.Cells[rowIniTabla, colEscDynamic + 1, rowData, colEscDynamic + 1], ExcelBorderStyle.Dotted);
                UtilExcel.BorderAround(ws.Cells[rowIniTabla, colEscDynamic + 2, rowData, colEscDynamic + 2], ExcelBorderStyle.Dotted);

                colEscDynamic += 4;
            }

            #endregion

            #region Total

            var rangotabla = ws.Cells[rowIniTabla - 3, colCentral, rowData, colEscDynamic];
            rangotabla.SetFont(fuenteTabla);

            ws.Cells[rowData, colCentral].Value = "TOTAL";
            ws.Cells[rowData, colCentral].SetFontBold();
            colEscDynamic = colIniEsc;
            foreach (var regEsc in listaEscenario)
            {
                decimal? valorPd = listadoGeneralDatos.Where(x => x.Pfresccodi == regEsc.Pfresccodi).Sum(x => x.Pfrtotpd ?? 0);
                decimal? valorPdd = listadoGeneralDatos.Where(x => x.Pfresccodi == regEsc.Pfresccodi).Sum(x => x.Pfrtotpdd ?? 0);
                decimal? valorPf = listadoGeneralDatos.Where(x => x.Pfresccodi == regEsc.Pfresccodi).Sum(x => x.Pfrtotpf ?? 0);
                decimal? valorPfr = listadoGeneralDatos.Where(x => x.Pfresccodi == regEsc.Pfresccodi).Sum(x => x.Pfrtotpfr ?? 0);

                ws.Cells[rowData, colEscDynamic, rowData, colEscDynamic + 4].Style.Numberformat.Format = "_ * #,##0_ ;_ * -#,##0_ ;_ * \" - \"_ ;_ @_ ";
                ws.Cells[rowData, colEscDynamic, rowData, colEscDynamic + 4].SetFontBold();

                ws.Cells[rowData, colEscDynamic++].Value = valorPd;
                ws.Cells[rowData, colEscDynamic++].Value = valorPdd;
                ws.Cells[rowData, colEscDynamic++].Value = valorPf;
                ws.Cells[rowData, colEscDynamic++].Value = valorPfr;


            }

            UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colCentral, rowData, colEscDynamic - 1], ExcelBorderStyle.Medium);

            #endregion

            #region Pie



            #endregion

            ws.View.FreezePanes(rowIniTabla + 1, 1);

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            ws.View.ZoomScale = 100;

        }

        private void GenerarHojaExcelReporteDatos(ExcelPackage xlPackage, int pfrrptcodiDatos, PfrPeriodoDTO regPeriodo)
        {
            List<PestaniaDatos> listaDatos = ObtenerDataDatosPorRangoDeReporte(pfrrptcodiDatos, out List<PrGrupodatDTO> listaParametros);
            //Obtengo Periodo Anterior
            DateTime fecPeriodoActual = new DateTime(regPeriodo.Pfrperanio, regPeriodo.Pfrpermes, 1);
            DateTime fecPeriodoAnterior = fecPeriodoActual.AddMonths(-1);

            ////Generacion del excel
            //var nameWS = "Datos";
            //ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            //ws = xlPackage.Workbook.Worksheets[nameWS];

            ExcelWorksheet ws = null;
            string nameWS = "Datos";

            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string fontFamTitle = "Calibri";
            string fontFamily = "Arial";

            var fuenteData = new Font(fontFamily, 8);
            var fuenteTitulo = new Font(fontFamTitle, 10);


            #region Bloque Parametros
            PrGrupodatDTO objCRActual = listaParametros.Find(r => r.Concepcodi == ConstantesPotenciaFirmeRemunerable.ConcepcodiCR);
            PrGrupodatDTO objCAActual = listaParametros.Find(r => r.Concepcodi == ConstantesPotenciaFirmeRemunerable.ConcepcodiCA);

            int colIniBloque = 5;
            int rowIniBloque = 3;

            ws.Cells[rowIniBloque, colIniBloque].Value = "Costo de Racionamiento (S/./kwh)";
            ws.Cells[rowIniBloque + 1, colIniBloque].Value = "Canon de Agua (S/./kwh)";

            ws.Cells[rowIniBloque, colIniBloque + 3].Value = Convert.ToDecimal(objCRActual.Formuladat) / 1000;//Mwh a Kwh
            ws.Cells[rowIniBloque, colIniBloque + 3].Style.Numberformat.Format = "#,##0.0000000";
            ws.Cells[rowIniBloque + 1, colIniBloque + 3].Value = Convert.ToDecimal(objCAActual.Formuladat);
            ws.Cells[rowIniBloque + 1, colIniBloque + 3].Style.Numberformat.Format = "#,##0.0000000";

            UtilExcel.BorderAround(ws.Cells[rowIniBloque, colIniBloque, rowIniBloque + 1, colIniBloque + 4], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region   Cabecera Tabla
            int colIniTable = 2;
            int rowIniTable = 8;
            int rowIniData = 0;
            int rowFinData = 0;

            int colEmpresa = colIniTable;
            int colCentral = colIniTable + 1;
            int colUnidad = colIniTable + 2;
            int colPE = colIniTable + 3;
            int colPF = colIniTable + 4;
            int colCV = colIniTable + 5;
            int colCR = colIniTable + 6;
            int colFK = colIniTable + 7;

            ws.Cells[rowIniTable, colCR].Value = "Periodo";
            rowIniTable++;
            ws.Cells[rowIniTable, colCR].Value = EPDate.f_NombreMes(fecPeriodoAnterior.Month) + "-" + fecPeriodoAnterior.Year;
            rowIniTable++;
            ws.Cells[rowIniTable - 2, colCR, rowIniTable - 2, colFK].SetFontBold();
            ws.Cells[rowIniTable - 2, colCR, rowIniTable - 1, colFK].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            UtilExcel.BorderAround(ws.Cells[rowIniTable - 2, colCR, rowIniTable - 1, colFK], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            ws.Cells[rowIniTable - 2, colCR, rowIniTable - 2, colFK].Merge = true;
            ws.Cells[rowIniTable - 1, colCR, rowIniTable - 1, colFK].Merge = true;

            ws.Cells[rowIniTable, colEmpresa].Value = "Empresa";
            ws.Cells[rowIniTable, colCentral].Value = "Central";
            ws.Cells[rowIniTable, colUnidad].Value = "Unidad";
            ws.Cells[rowIniTable, colPE].Value = "Potencia Efectiva";
            ws.Cells[rowIniTable, colPF].Value = "Potencia Firme";
            ws.Cells[rowIniTable, colCV].Value = "Costo Variable";
            ws.Cells[rowIniTable, colCR].Value = "CR";
            ws.Cells[rowIniTable, colFK].Value = "FK";

            ws.Cells[rowIniTable, colEmpresa, rowIniTable, colFK].SetFont(fuenteTitulo);
            ws.Cells[rowIniTable, colEmpresa, rowIniTable, colFK].SetFontBold();

            ws.Cells[rowIniTable, colEmpresa, rowIniTable, colFK].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Column(colEmpresa).Width = 40;
            ws.Column(colCentral).Width = 30;
            ws.Column(colUnidad).Width = 30;
            ws.Column(colPE).Width = 16;
            ws.Column(colPF).Width = 16;
            ws.Column(colCV).Width = 10;
            ws.Column(colCR).Width = 10;
            ws.Column(colFK).Width = 10;

            rowIniData = rowIniTable + 1;
            #endregion

            #region Cuerpo Tabla

            rowIniTable++;
            foreach (var itemGe in listaDatos.GroupBy(x => x.Emprcodi))
            {
                var rowEmpIni = rowIniTable;
                foreach (var regDatos in itemGe)
                {
                    ws.Cells[rowIniTable, colEmpresa].Value = regDatos.Empresa;
                    ws.Cells[rowIniTable, colCentral].Value = regDatos.Central;
                    ws.Cells[rowIniTable, colUnidad].Value = regDatos.UnidadNombre;
                    ws.Cells[rowIniTable, colPE].Value = regDatos.PE;
                    ws.Cells[rowIniTable, colPE].Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[rowIniTable, colPF].Value = regDatos.PF;
                    ws.Cells[rowIniTable, colPF].Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[rowIniTable, colCV].Value = regDatos.CV;
                    ws.Cells[rowIniTable, colCV].Style.Numberformat.Format = "#,##0.0000";
                    ws.Cells[rowIniTable, colCR].Value = regDatos.CR;
                    ws.Cells[rowIniTable, colFK].Value = regDatos.FK;
                    ws.Cells[rowIniTable, colFK].Style.Numberformat.Format = "#,##0.0000";

                    rowIniTable++;
                }
                UtilExcel.BorderAround(ws.Cells[rowEmpIni, colEmpresa, rowIniTable - 1, colFK]);
                ws.Cells[rowEmpIni, colEmpresa, rowIniTable - 1, colFK].Style.WrapText = true;
            }
            rowFinData = rowIniTable;
            ws.Cells[rowIniData, colEmpresa, rowFinData, colFK].SetFont(fuenteData);

            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colUnidad, rowFinData, colUnidad]);
            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colPE, rowFinData, colPE]);
            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colPF, rowFinData, colPF]);
            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colCV, rowFinData, colCV]);
            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colCR, rowFinData, colCR]);
            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colFK, rowFinData, colFK]);

            ws.Cells[rowIniData - 1, colEmpresa, rowFinData - 1, colFK].Style.WrapText = true;

            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colEmpresa, rowFinData - 1, colFK], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            UtilExcel.BorderAround(ws.Cells[rowIniData, colEmpresa, rowFinData, colFK], OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            ws.Cells[rowIniData - 1, colEmpresa, rowFinData, colFK].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            ws.Cells[rowFinData, colUnidad, rowFinData, colUnidad].Value = "TOTAL";
            ws.Cells[rowFinData, colPE, rowFinData, colPE].Value = listaDatos.Sum(x => x.PE);
            ws.Cells[rowFinData, colPF, rowFinData, colPF].Value = listaDatos.Sum(x => x.PF);
            ws.Cells[rowFinData, colUnidad, rowFinData, colPF].Style.Numberformat.Format = "_ * #,##0.000_ ;_ * -#,##0.000_ ;_ * -??_ ;_ @_ ";
            ws.Cells[rowFinData, colUnidad, rowFinData, colPF].SetFontBold();
            #endregion

            ws.View.FreezePanes(rowIniData, 1);

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            ws.View.ZoomScale = 100;

        }

        #endregion

        public void GenerarArchivoExcelPFR2(string ruta, int tipo, int pfrrptcodi, int pfrrptcodiDatos, int numEscenario, out string nameFile)
        {
            PfrReporteDTO regReporte = GetByIdPfrReporte(pfrrptcodi);
            PfrRelacionPotenciaFirmeDTO regRelacionPF = GetByCriteriaPfrRelacionPotenciaFirmes(regReporte.Pfrrptcodi).FirstOrDefault();
            if (regRelacionPF == default(PfrRelacionPotenciaFirmeDTO)) throw new ArgumentException("No existe información de Potencia Firme");

            PfrRecalculoDTO regRecalculo = GetByIdPfrRecalculo(regReporte.Pfrreccodi.Value);
            PfrPeriodoDTO regPeriodo = GetByIdPfrPeriodo(regRecalculo.Pfrpercodi);

            //Obtenemos los escenarios a usar
            List<PfrEscenarioDTO> listaEscenarioDatosPresentes = ListPfrEscenariosByReportecodi(pfrrptcodiDatos).OrderBy(x => x.Pfrescfecini).ToList();
            PfrEscenarioDTO regEscenarioD = new PfrEscenarioDTO();
            int num = 1;
            foreach (var objEscenario in listaEscenarioDatosPresentes)
            {
                if (num == numEscenario)
                {
                    regEscenarioD = objEscenario;
                }
                num++;
            }


            bool esExportarBarras = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.Barras;
            bool esExportarDemanda = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.Demanda;
            bool esExportarGeneracion = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.Generacion;
            bool esExportarCompDinamica = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.CompDinamica;
            bool esExportarLineas = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.Lineas;
            bool esExportarTrafo2 = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.Trafo2;
            bool esExportarTrafo3 = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.Trafo3;
            bool esExportarDiagramaUnifilar = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.DiagramaUnifilar;
            bool esExportarCongestion = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.Congestion;
            bool esExportarAux2 = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.Aux2;
            bool esExportarCarga = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.Carga;

            bool esExportarLvtpOpf = tipo == (int)ConstantesPotenciaFirmeRemunerable.ExcelLVTP_OPF.LvtpOpf;

            string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            string letra = letras[numEscenario - 1];

            nameFile = string.Format("PFR_{0}_{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000);

            nameFile = esExportarBarras ? string.Format("Barras_" + letra + "_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarDemanda ? string.Format("Demanda_" + letra + "_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarGeneracion ? string.Format("Generacion_" + letra + "_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarCompDinamica ? string.Format("CompDinamica_" + letra + "_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarLineas ? string.Format("Lineas_" + letra + "_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarTrafo2 ? string.Format("Trafo2_" + letra + "_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarTrafo3 ? string.Format("Trafo3_" + letra + "_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarCongestion ? string.Format("Congestion_" + letra + "_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarCarga ? string.Format("Carga_" + letra + "_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarAux2 ? string.Format("AUX2" + letra + "_{0}-{1}.xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;
            nameFile = esExportarLvtpOpf ? string.Format("LVTP_OPF_Unificado_{0}-{1}_" + letra + ".xlsx", regPeriodo.Pfrpermes, regPeriodo.Pfrperanio - 2000) : nameFile;


            //Obtenemos los escenarios con los cuales se guardo las salidas GAMS, es decir, escenarios de AUX1 (solo cuando exportemos las salidas)
            int pfrrptcodiAux1 = -1;
            int escenarioCodiAux1Salida = -1;
            List<PfrEscenarioDTO> listaEscenarioAux1 = new List<PfrEscenarioDTO>();
            if (esExportarBarras || esExportarCongestion || esExportarGeneracion || esExportarLvtpOpf)
            {
                pfrrptcodiAux1 = this.GetUltimoPfrrptcodiXRecalculo(regReporte.Pfrreccodi.Value, ConstantesPotenciaFirmeRemunerable.CuadroAUX1);
                listaEscenarioAux1 = ListPfrEscenariosByReportecodi(pfrrptcodiAux1).OrderBy(x => x.Pfrescfecini).ToList();
                int nummero = 1;
                foreach (var objEscenarioA in listaEscenarioAux1)
                {
                    if (nummero == numEscenario)
                    {
                        var regEscenarioA = objEscenarioA;
                        escenarioCodiAux1Salida = regEscenarioA.Pfresccodi;
                    }
                    nummero++;
                }
            }


            //Descargo archivo segun requieran
            string rutaFile = ruta + nameFile;

            DateTime fechaIniRango = ObtenerPrimerDiaPeriodo(regPeriodo);
            DateTime fechaFinRango = ObtenerUltimoDiaPeriodo(regPeriodo);

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }


            //Solo buscamos calculsoBarras si se exportaran ciertos reportes (listado demora 1 minutos)
            List<BarraSuministro> listaCalculoBarras = new List<BarraSuministro>();
            List<BarraSSAA> lstBarraSSAA = new List<BarraSSAA>();
            List<PfrEntidadDTO> listaRelacion = new List<PfrEntidadDTO>();
            List<VtpPeajeEgresoMinfoDTO> listaPeajeEgresoMinfo = new List<VtpPeajeEgresoMinfoDTO>();
            List<VtpRetiroPotescDTO> listaRetiroSinContrato = new List<VtpRetiroPotescDTO>();
            if (esExportarDemanda || esExportarCarga || esExportarLvtpOpf)
            {
                PfrReporteDTO reporteDatos = GetByIdPfrReporte(pfrrptcodiDatos);
                var recpotcodi = reporteDatos.Pfrrptrevisionvtp;
                //Listado usado en reporte CARGA
                listaCalculoBarras = this.ListarCalculoBarra(regPeriodo, regReporte.Pfrrptfecmd, recpotcodi, out lstBarraSSAA, out listaRelacion, out listaPeajeEgresoMinfo, out listaRetiroSinContrato);

            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //individual
                if (esExportarBarras)
                {
                    GenerarHojaExcelLvtpOpfBarras(xlPackage, regEscenarioD, escenarioCodiAux1Salida);
                    xlPackage.Save();
                }
                if (esExportarDemanda)
                {
                    GenerarHojaExcelLvtpOpfDemanda(xlPackage, regEscenarioD, listaCalculoBarras, lstBarraSSAA);
                    xlPackage.Save();
                }
                if (esExportarGeneracion)
                {
                    GenerarHojaExcelGeneracion(xlPackage, regEscenarioD, regReporte, pfrrptcodiDatos, regRelacionPF, escenarioCodiAux1Salida);
                    xlPackage.Save();
                }
                if (esExportarCompDinamica)
                {
                    GenerarHojaExcelLvtpOpfComDinamica(xlPackage, regEscenarioD);
                    xlPackage.Save();
                }
                if (esExportarLineas)
                {
                    GenerarHojaExcelLvtpOpfLineas(xlPackage, regEscenarioD);
                    xlPackage.Save();
                }
                if (esExportarTrafo2)
                {
                    GenerarHojaExcelLvtpOpfTrafo2(xlPackage, regEscenarioD);
                    xlPackage.Save();
                }
                if (esExportarTrafo3)
                {
                    GenerarHojaExcelLvtpOpfTrafo3(xlPackage, regEscenarioD);
                    xlPackage.Save();
                }

                if (esExportarCongestion)
                {
                    GenerarHojaExcelLvtpOpfCongestion(xlPackage, regEscenarioD, escenarioCodiAux1Salida);
                    xlPackage.Save();
                }
                if (esExportarAux2)
                {
                    GenerarHojaExcelAUX2_Lvtp_Opf(xlPackage, regReporte, pfrrptcodiDatos, regPeriodo, regEscenarioD);
                    xlPackage.Save();
                }
                if (esExportarCarga)
                {
                    GenerarHojaExcelReporteCarga(xlPackage, regRecalculo, regPeriodo, listaCalculoBarras, lstBarraSSAA, listaRelacion, listaPeajeEgresoMinfo, listaRetiroSinContrato);
                    xlPackage.Save();
                }

                //agrupado 
                if (esExportarLvtpOpf)
                {
                    GenerarHojaExcelLvtpOpfBarras(xlPackage, regEscenarioD, escenarioCodiAux1Salida);
                    GenerarHojaExcelLvtpOpfDemanda(xlPackage, regEscenarioD, listaCalculoBarras, lstBarraSSAA);
                    GenerarHojaExcelGeneracion(xlPackage, regEscenarioD, regReporte, pfrrptcodiDatos, regRelacionPF, escenarioCodiAux1Salida);
                    GenerarHojaExcelLvtpOpfComDinamica(xlPackage, regEscenarioD);
                    GenerarHojaExcelLvtpOpfLineas(xlPackage, regEscenarioD);
                    GenerarHojaExcelLvtpOpfTrafo2(xlPackage, regEscenarioD);
                    GenerarHojaExcelLvtpOpfTrafo3(xlPackage, regEscenarioD);
                    GenerarHojaExcelLvtpOpfCongestion(xlPackage, regEscenarioD, escenarioCodiAux1Salida);
                    GenerarHojaExcelAUX2_Lvtp_Opf(xlPackage, regReporte, pfrrptcodiDatos, regPeriodo, regEscenarioD);
                    GenerarHojaExcelReporteCarga(xlPackage, regRecalculo, regPeriodo, listaCalculoBarras, lstBarraSSAA, listaRelacion, listaPeajeEgresoMinfo, listaRetiroSinContrato);

                    xlPackage.Save();
                }
            }
        }

        #region Hojas LVTP_OPF-XX-XX

        /// <summary>
        /// Genera el archivo Excel para Barras, por escenario
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="regEscenario"></param>
        private void GenerarHojaExcelLvtpOpfBarras(ExcelPackage xlPackage, PfrEscenarioDTO regEscenario, int escenarioCodiAux1Salida)
        {
            //Obtenemos todo el listado de barras
            DateTime primerDiaEscenario = regEscenario.Pfrescfecini;
            DateTime ultimoDiaEscenario = regEscenario.Pfrescfecfin;

            var listaBarrasActivas = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Barra, primerDiaEscenario, ultimoDiaEscenario);
            List<PfrResultadosGamsDTO> listaSalidas = ListPfrResultadosGamsByTipoYEscenario(escenarioCodiAux1Salida, (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.V);

            ExcelWorksheet ws = null;

            string nameWS = "Barras";

            this.GenerarHojaExcelLvtpOpfBarras(ref ws, xlPackage, nameWS, listaBarrasActivas, listaSalidas);

        }

        /// <summary>
        /// Genera la relacion de Barras en un formato excel 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="listaBarrasActivas"></param>
        private void GenerarHojaExcelLvtpOpfBarras(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS
                    , List<PfrEntidadDTO> listaBarrasActivas, List<PfrResultadosGamsDTO> listaSalidas)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            string fontFamTitle = "Arial Narrow";
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 8);
            var fuenteTitulo = new Font(fontFamTitle, 11);
            var fuenteInforme = new Font(fontFamTitle, 8);

            int colIniTable = 1;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 10, 18, 13, 13, 13, 13 })//columna A-I
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }

            #region  Filtros y Cabecera

            var colIdbarra = colIniTable;
            int colBarranomb = colIniTable + 1;
            int colTension = colIniTable + 2;
            int colVmax = colIniTable + 3;
            int colVmin = colIniTable + 4;
            int colV = colIniTable + 5;

            int rowIniTabla = 1;
            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colIdbarra].Value = "ID";
            ws.Cells[rowIniTabla, colBarranomb].Value = "Nombre";
            ws.Cells[rowIniTabla, colTension].Value = "Tensión";
            ws.Cells[rowIniTabla, colVmax].Value = "VMáx";
            ws.Cells[rowIniTabla, colVmin].Value = "Vmín";
            ws.Cells[rowIniTabla, colV].Value = "V";

            var rangoCab = ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colV];
            rangoCab.SetAlignment();
            //rangoCab.Style.WrapText = true;

            //UtilExcel.AllBorders(ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colIdbarra]);
            UtilExcel.BorderAround(ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colV], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in listaBarrasActivas)
            {
                var regSalida = listaSalidas.Find(v => v.Pfreqpcodi == item.Pfrentcodi);

                ws.Cells[rowData, colIdbarra].Value = item.Pfrentid;
                ws.Cells[rowData, colBarranomb].Value = item.Pfrentnomb;
                ws.Cells[rowData, colTension].Value = item.Tension;
                ws.Cells[rowData, colTension, rowData, colTension].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colVmax].Value = item.Vmax;
                ws.Cells[rowData, colVmax, rowData, colVmax].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colVmin].Value = item.Vmin;
                ws.Cells[rowData, colVmin, rowData, colVmin].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colV].Value = regSalida != null ? regSalida.Pfrrgresultado : null;
                ws.Cells[rowData, colV, rowData, colV].Style.Numberformat.Format = "#,##0.0000";

                rowData++;
            }
            CeldasExcelColorFondo(ws, rowIniTabla, colV, rowData, colV, ConstantesPotenciaFirmeRemunerable.ColorSalidasGams);

            var rangotabla = ws.Cells[rowIniTabla + 1, colIdbarra, rowData, colV];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            //ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(2, 1);
        }

        /// <summary>
        /// Genera el archivo Excel para Demanda, por escenario
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="regEscenario"></param>
        private void GenerarHojaExcelLvtpOpfDemanda(ExcelPackage xlPackage, PfrEscenarioDTO regEscenario
            , List<BarraSuministro> listaCalculoBarras, List<BarraSSAA> lstBarraSSAA)
        {
            //Obtenemos todo el listado de barras
            DateTime primerDiaEscenario = regEscenario.Pfrescfecini;
            DateTime ultimoDiaEscenario = regEscenario.Pfrescfecfin;

            List<PestaniaDemanda> listaDemanda = ObtenerDataDemandaPorRango(primerDiaEscenario, ultimoDiaEscenario, listaCalculoBarras, lstBarraSSAA);

            ExcelWorksheet ws = null;
            string nameWS = "Demanda";

            this.GenerarHojaExcelLvtpOpfDemanda(ref ws, xlPackage, nameWS, listaDemanda);
        }

        /// <summary>
        /// Genera la relacion de Demanda en un formato excel 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="listaDemanda"></param>
        private void GenerarHojaExcelLvtpOpfDemanda(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS, List<PestaniaDemanda> listaDemanda)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            string fontFamTitle = "Arial Narrow";
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 8);
            var fuenteTitulo = new Font(fontFamTitle, 11);
            var fuenteInforme = new Font(fontFamTitle, 8);

            int colIniTable = 1;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 10, 20, 13, 13, 13, 13 })//columna A-I
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }

            #region  Filtros y Cabecera

            int colIdBarra = colIniTable;
            int colNombreBarra = colIdBarra + 1;
            int colTensionBarra = colIdBarra + 2;
            int colPBarra = colIdBarra + 3;
            int colQBarra = colIdBarra + 4;
            int colCRBarra = colIdBarra + 5;

            int rowIniTabla = 1;
            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colIdBarra].Value = "Id Barra";
            ws.Cells[rowIniTabla, colNombreBarra].Value = "Nombre Barra";
            ws.Cells[rowIniTabla, colTensionBarra].Value = "Tensión (kV)";
            ws.Cells[rowIniTabla, colPBarra].Value = "P";
            ws.Cells[rowIniTabla, colQBarra].Value = "Q";
            ws.Cells[rowIniTabla, colCRBarra].Value = "CompReactiva";

            var rangoCab = ws.Cells[rowIniTabla, colIdBarra, rowIniTabla, colCRBarra];
            rangoCab.SetAlignment();

            UtilExcel.BorderAround(ws.Cells[rowIniTabla, colIdBarra, rowIniTabla, colCRBarra], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in listaDemanda)
            {
                ws.Cells[rowData, colIdBarra].Value = item.IdBarra;
                ws.Cells[rowData, colNombreBarra].Value = item.NombreBarra;
                ws.Cells[rowData, colTensionBarra].Value = item.TensionBarra;
                ws.Cells[rowData, colPBarra].Value = item.P;
                ws.Cells[rowData, colPBarra].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colQBarra].Value = item.Q;
                ws.Cells[rowData, colQBarra].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colCRBarra].Value = item.CompReactiva;
                ws.Cells[rowData, colCRBarra].Style.Numberformat.Format = "#,##0.00";

                rowData++;
            }


            var rangotabla = ws.Cells[rowIniTabla + 1, colIdBarra, rowData, colCRBarra];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            //ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(2, 1);
        }

        public void GenerarHojaExcelGeneracion(ExcelPackage xlPackage, PfrEscenarioDTO regEscenario, PfrReporteDTO regReporte, int pfrrptcodiDatos, PfrRelacionPotenciaFirmeDTO regRelacionPF, int escenarioCodiAux1Salida)
        {
            if (regRelacionPF == default(PfrRelacionPotenciaFirmeDTO)) throw new ArgumentException("No existe relación con Potencia Firme");

            List<PfrResultadosGamsDTO> listaSalidas = ListPfrResultadosGamsByTipoYEscenario(escenarioCodiAux1Salida, (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Pg);

            ExcelWorksheet ws = null;

            string nameWS = "GENERACIÓN";

            DateTime primerDiaEscenario = regEscenario.Pfrescfecini;
            DateTime ultimoDiaEscenario = regEscenario.Pfrescfecfin;

            this.GenerarHojaExcelReporteGeneracion(ref ws, xlPackage, nameWS, primerDiaEscenario, ultimoDiaEscenario, regReporte, regRelacionPF, pfrrptcodiDatos, listaSalidas);
        }

        private void GenerarHojaExcelReporteGeneracion(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS
            , DateTime primerDiaEscenario, DateTime ultimoDiaEscenario, PfrReporteDTO regReporte
            , PfrRelacionPotenciaFirmeDTO regRelacionPF, int pfrrptcodiDatos, List<PfrResultadosGamsDTO> listaSalidas)
        {
            var pfrrptcodiAux1 = this.GetUltimoPfrrptcodiXRecalculo(regReporte.Pfrreccodi.Value, ConstantesPotenciaFirmeRemunerable.CuadroAUX1);
            List<PfrEntidadDTO> listaDatosGeneracion = ObtenerDataGeneracionPorRango(regRelacionPF.Pfrptcodi.Value, primerDiaEscenario, ultimoDiaEscenario, pfrrptcodiDatos, pfrrptcodiAux1);
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            string fontFamTitle = "Arial Narrow";
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 8);
            var fuenteTitulo = new Font(fontFamTitle, 11);
            var fuenteInforme = new Font(fontFamTitle, 8);

            int colIniTable = 1;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 13, 13, 25, 15, 16, 16, 14, 14, 13 })//columna A-I
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }

            #region  Filtros y Cabecera

            var colIdbarra = colIniTable;
            var colIdunidad = colIdbarra + 1;
            int colBarranomb = colIdunidad + 1;
            int colTension = colBarranomb + 1;
            int colPmax = colTension + 1;
            int colCV = colPmax + 1;
            int colQmax = colCV + 1;
            int colQmin = colQmax + 1;
            int colRef = colQmin + 1;
            //int colBorrar = colRef + 1;
            int colPg = colRef + 3;

            int rowIniTabla = 1;
            //ws.Row(rowIniTabla).Height = 45;
            ws.Cells[rowIniTabla, colIdbarra].Value = "ID";
            ws.Cells[rowIniTabla, colIdunidad].Value = "Unidad";
            ws.Cells[rowIniTabla, colBarranomb].Value = "Barra";
            ws.Cells[rowIniTabla, colTension].Value = "Tensión";
            ws.Cells[rowIniTabla, colPmax].Value = "Pmax";
            ws.Cells[rowIniTabla, colCV].Value = "CV";
            ws.Cells[rowIniTabla, colQmax].Value = "Qmax";
            ws.Cells[rowIniTabla, colQmin].Value = "Qmin";
            ws.Cells[rowIniTabla, colRef].Value = "Ref";
            //ws.Cells[rowIniTabla, colBorrar].Value = "Codigo Generacion";
            ws.Cells[rowIniTabla, colPg].Value = "Pg";

            var rangoCab = ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colRef];
            rangoCab.SetAlignment();
            rangoCab.Style.WrapText = true;

            UtilExcel.AllBorders(ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colIdbarra]);
            UtilExcel.BorderAround(ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colRef], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;
            int rowIniRangoEmpresa = rowData;

            foreach (var item in listaDatosGeneracion)
            {
                var regSalida = listaSalidas.Find(v => v.Pfrrgecodi == item.Pfrentcodi);

                ws.Cells[rowData, colIdbarra].Value = item.Pfrentid;
                ws.Cells[rowData, colIdunidad].Value = item.Numunidad;
                ws.Cells[rowData, colBarranomb].Value = item.Pfrentnomb;
                //tensión
                ws.Cells[rowData, colTension].Value = item.Tension;
                ws.Cells[rowData, colTension, rowData, colTension].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colPmax].Value = item.Potenciamax;
                ws.Cells[rowData, colPmax, rowData, colPmax].Style.Numberformat.Format = "#,##0.000";
                //costo varibale
                ws.Cells[rowData, colCV].Value = item.Costov;
                ws.Cells[rowData, colCV, rowData, colCV].Style.Numberformat.Format = "#,##0.000";
                ws.Cells[rowData, colQmax].Value = item.Qmax;
                ws.Cells[rowData, colQmax, rowData, colQmax].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colQmin].Value = item.Qmin;
                ws.Cells[rowData, colQmin, rowData, colQmin].Style.Numberformat.Format = "#,##0.00";
                //Costo Variable Flujo
                if (item.Ref.GetValueOrDefault(0) != 1)
                    ws.Cells[rowData, colRef].Value = "-";
                else
                    ws.Cells[rowData, colRef].Value = item.Ref;
                //ws.Cells[rowData, colBorrar].Value = item.Pfrentid;
                ws.Cells[rowData, colPg].Value = regSalida != null ? regSalida.Pfrrgresultado : null;
                rowData++;
                //UtilExcel.BorderAround(ws.Cells[rowCentralIni, colCentral, rowData - 1, colCostoVFlujo]);
            }
            CeldasExcelColorFondo(ws, rowIniTabla, colPg, rowData, colPg, ConstantesPotenciaFirmeRemunerable.ColorSalidasGams);
            //ws.Cells[rowIniTabla + 1, colBarranomb, rowData - 1, colTension].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#e3e4e5"));
            //ws.Cells[rowIniTabla + 1, colPmax, rowData - 1, colCV].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff8c40 "));

            //UtilExcel.BorderAround(ws.Cells[rowData, colIdbarra, rowData, colRef]);
            UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colIdbarra, rowData - 1, colIdbarra]);

            var rangotabla = ws.Cells[rowIniTabla + 1, colIdbarra, rowData, colRef];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            //ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
        }

        /// <summary>
        /// Genera el archivo Excel para Lineas, por escenario
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="regEscenario"></param>
        /// 
        private void GenerarHojaExcelLvtpOpfLineas(ExcelPackage xlPackage, PfrEscenarioDTO regEscenario)
        {
            //Obtenemos todo el listado de barras
            DateTime primerDiaEscenario = regEscenario.Pfrescfecini;
            DateTime ultimoDiaEscenario = regEscenario.Pfrescfecfin;

            var listaLineasActivas = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Linea, primerDiaEscenario, ultimoDiaEscenario);

            ExcelWorksheet ws = null;

            string nameWS = "Lineas";

            this.GenerarHojaExcelLvtpOpfLineas(ref ws, xlPackage, nameWS, listaLineasActivas);

        }

        /// <summary>
        /// Genera la relacion de Lineas en un formato excel 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="listaLineasActivas"></param>
        private void GenerarHojaExcelLvtpOpfLineas(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS, List<PfrEntidadDTO> listaLineasActivas)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            string fontFamTitle = "Arial Narrow";
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 8);
            var fuenteTitulo = new Font(fontFamTitle, 11);
            var fuenteInforme = new Font(fontFamTitle, 8);

            int colIniTable = 1;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 9, 9, 9, 15, 15, 12, 12, 12, 12, 12 })//columna A-I
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }

            #region  Filtros y Cabecera

            var colIdbarra = colIniTable;
            int colIdbarra1 = colIniTable + 1;
            int colIdbarra2 = colIniTable + 2;
            int colNombrebarra1 = colIniTable + 3;
            int colNombrebarra2 = colIniTable + 4;
            int colResistencia_R = colIniTable + 5;
            int colReactancia_X = colIniTable + 6;
            int colConductancia_G = colIniTable + 7;
            int colAdmitancia_B = colIniTable + 8;
            int colPMax = colIniTable + 9;

            int rowIniTabla = 1;
            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colIdbarra].Value = "ID";
            ws.Cells[rowIniTabla, colIdbarra1].Value = "ID Barra 1";
            ws.Cells[rowIniTabla, colIdbarra2].Value = "ID Barra 2";
            ws.Cells[rowIniTabla, colNombrebarra1].Value = "Barra 1";
            ws.Cells[rowIniTabla, colNombrebarra2].Value = "Barra 2";
            ws.Cells[rowIniTabla, colResistencia_R].Value = "Resistencia";
            ws.Cells[rowIniTabla, colReactancia_X].Value = "Reactancia";
            ws.Cells[rowIniTabla, colConductancia_G].Value = "Conductancia";
            ws.Cells[rowIniTabla, colAdmitancia_B].Value = "Admitancia";
            ws.Cells[rowIniTabla, colPMax].Value = "PMáx";

            var rangoCab = ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colPMax];
            rangoCab.SetAlignment();
            //rangoCab.Style.WrapText = true;

            //UtilExcel.AllBorders(ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colIdbarra]);
            UtilExcel.BorderAround(ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colPMax], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in listaLineasActivas)
            {
                ws.Cells[rowData, colIdbarra].Value = item.Pfrentid;
                ws.Cells[rowData, colIdbarra1].Value = item.Idbarra1;
                ws.Cells[rowData, colIdbarra2].Value = item.Idbarra2;
                ws.Cells[rowData, colNombrebarra1].Value = item.Idbarra1desc;
                ws.Cells[rowData, colNombrebarra2].Value = item.Idbarra2desc;
                ws.Cells[rowData, colResistencia_R].Value = item.Resistencia;
                ws.Cells[rowData, colResistencia_R].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colReactancia_X].Value = item.Reactancia;
                ws.Cells[rowData, colReactancia_X].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colConductancia_G].Value = item.Conductancia;
                ws.Cells[rowData, colConductancia_G].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colAdmitancia_B].Value = item.Admitancia;
                ws.Cells[rowData, colAdmitancia_B].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colPMax].Value = item.Potenciamax;

                rowData++;
                //UtilExcel.BorderAround(ws.Cells[rowCentralIni, colCentral, rowData - 1, colCostoVFlujo]);
            }
            ////ws.Cells[rowIniTabla + 1, colBarranomb, rowData - 1, colTension].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#e3e4e5"));
            ////ws.Cells[rowIniTabla + 1, colPmax, rowData - 1, colCV].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff8c40 "));

            ////UtilExcel.BorderAround(ws.Cells[rowData, colIdbarra, rowData, colRef]);
            //UtilExcel.BorderAround(ws.Cells[rowIniTabla + 1, colIdbarra, rowData - 1, colIdbarra]);

            var rangotabla = ws.Cells[rowIniTabla + 1, colIdbarra, rowData, colPMax];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            //ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(2, 1);
        }

        /// <summary>
        /// Genera el archivo Excel para Trafo2, por escenario
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="regEscenario"></param>
        private void GenerarHojaExcelLvtpOpfTrafo2(ExcelPackage xlPackage, PfrEscenarioDTO regEscenario)
        {
            //Obtenemos todo el listado de barras
            DateTime primerDiaEscenario = regEscenario.Pfrescfecini;
            DateTime ultimoDiaEscenario = regEscenario.Pfrescfecfin;

            var listaTrafo2Activas = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2, primerDiaEscenario, ultimoDiaEscenario);

            ExcelWorksheet ws = null;

            string nameWS = "Trafo2";

            this.GenerarHojaExcelLvtpOpfTrafo2(ref ws, xlPackage, nameWS, listaTrafo2Activas);
        }

        /// <summary>
        /// Genera la relacion de Trafo2 en un formato excel 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="listaTrafo2Activas"></param>
        private void GenerarHojaExcelLvtpOpfTrafo2(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS, List<PfrEntidadDTO> listaTrafo2Activas)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            string fontFamTitle = "Arial Narrow";
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 8);
            var fuenteTitulo = new Font(fontFamTitle, 11);
            var fuenteInforme = new Font(fontFamTitle, 8);

            int colIniTable = 1;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 9, 9, 9, 15, 15, 12, 12, 12, 12, 12, 12, 12 })//columna A-I
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }

            #region  Filtros y Cabecera

            var colIdbarra = colIniTable;
            int colIdbarra1 = colIniTable + 1;
            int colIdbarra2 = colIniTable + 2;
            int colNombrebarra1 = colIniTable + 3;
            int colNombrebarra2 = colIniTable + 4;
            int colResistencia_R = colIniTable + 5;
            int colReactancia_X = colIniTable + 6;
            int colConductancia_G = colIniTable + 7;
            int colAdmitancia_B = colIniTable + 8;
            int colTap1 = colIniTable + 9;
            int colTap2 = colIniTable + 10;
            int colPMax = colIniTable + 11;

            int rowIniTabla = 1;
            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colIdbarra].Value = "ID";
            ws.Cells[rowIniTabla, colIdbarra1].Value = "ID Barra 1";
            ws.Cells[rowIniTabla, colIdbarra2].Value = "ID Barra 2";
            ws.Cells[rowIniTabla, colNombrebarra1].Value = "Barra 1";
            ws.Cells[rowIniTabla, colNombrebarra2].Value = "Barra 2";
            ws.Cells[rowIniTabla, colResistencia_R].Value = "Resistencia";
            ws.Cells[rowIniTabla, colReactancia_X].Value = "Reactancia";
            ws.Cells[rowIniTabla, colConductancia_G].Value = "Conductancia";
            ws.Cells[rowIniTabla, colAdmitancia_B].Value = "Admitancia";
            ws.Cells[rowIniTabla, colTap1].Value = "Tap1";
            ws.Cells[rowIniTabla, colTap2].Value = "Tap2";
            ws.Cells[rowIniTabla, colPMax].Value = "PMáx";

            var rangoCab = ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colPMax];
            rangoCab.SetAlignment();

            UtilExcel.BorderAround(ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colPMax], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in listaTrafo2Activas)
            {
                ws.Cells[rowData, colIdbarra].Value = item.Pfrentid;
                ws.Cells[rowData, colIdbarra1].Value = item.Idbarra1;
                ws.Cells[rowData, colIdbarra2].Value = item.Idbarra2;
                ws.Cells[rowData, colNombrebarra1].Value = item.Idbarra1desc;
                ws.Cells[rowData, colNombrebarra2].Value = item.Idbarra2desc;
                ws.Cells[rowData, colResistencia_R].Value = item.Resistencia;
                ws.Cells[rowData, colResistencia_R].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colReactancia_X].Value = item.Reactancia;
                ws.Cells[rowData, colReactancia_X].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colConductancia_G].Value = item.Conductancia;
                ws.Cells[rowData, colConductancia_G].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colAdmitancia_B].Value = item.Admitancia;
                ws.Cells[rowData, colAdmitancia_B].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colTap1].Value = item.Tap1;
                ws.Cells[rowData, colTap2].Value = item.Tap2;
                ws.Cells[rowData, colPMax].Value = item.Potenciamax;

                rowData++;

            }

            var rangotabla = ws.Cells[rowIniTabla + 1, colIdbarra, rowData, colPMax];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            //ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(2, 1);
        }

        /// <summary>
        /// Genera el archivo Excel para Trafo3, por escenario
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="regEscenario"></param>
        private void GenerarHojaExcelLvtpOpfTrafo3(ExcelPackage xlPackage, PfrEscenarioDTO regEscenario)
        {
            //Obtenemos todo el listado de barras
            DateTime primerDiaEscenario = regEscenario.Pfrescfecini;
            DateTime ultimoDiaEscenario = regEscenario.Pfrescfecfin;

            var listaTrafo3Activas = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3, primerDiaEscenario, ultimoDiaEscenario);

            ExcelWorksheet ws = null;

            string nameWS = "Trafo3";

            this.GenerarHojaExcelLvtpOpfTrafo3(ref ws, xlPackage, nameWS, listaTrafo3Activas);
        }

        /// <summary>
        /// Genera la relacion de Trafo3 en un formato excel 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="listaTrafo3Activas"></param>
        private void GenerarHojaExcelLvtpOpfTrafo3(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS, List<PfrEntidadDTO> listaTrafo3Activas)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            string fontFamTitle = "Arial Narrow";
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 8);
            var fuenteTitulo = new Font(fontFamTitle, 11);
            var fuenteInforme = new Font(fontFamTitle, 8);

            int colIniTable = 1;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 9, 9, 9, 15, 15, 12, 12, 12, 12, 12, 12, 12 })//columna A-I
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }

            #region  Filtros y Cabecera

            var colIdbarra = colIniTable;
            int colIdbarra1 = colIniTable + 1;
            int colIdbarra2 = colIniTable + 2;
            int colNombrebarra1 = colIniTable + 3;
            int colNombrebarra2 = colIniTable + 4;
            int colResistencia_R = colIniTable + 5;
            int colReactancia_X = colIniTable + 6;
            int colConductancia_G = colIniTable + 7;
            int colAdmitancia_B = colIniTable + 8;
            int colTap1 = colIniTable + 9;
            int colTap2 = colIniTable + 10;
            int colPMax = colIniTable + 11;

            int rowIniTabla = 1;
            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colIdbarra].Value = "ID";
            ws.Cells[rowIniTabla, colIdbarra1].Value = "ID Barra 1";
            ws.Cells[rowIniTabla, colIdbarra2].Value = "ID Barra 2";
            ws.Cells[rowIniTabla, colNombrebarra1].Value = "Barra 1";
            ws.Cells[rowIniTabla, colNombrebarra2].Value = "Barra 2";
            ws.Cells[rowIniTabla, colResistencia_R].Value = "Resistencia";
            ws.Cells[rowIniTabla, colReactancia_X].Value = "Reactancia";
            ws.Cells[rowIniTabla, colConductancia_G].Value = "Conductancia";
            ws.Cells[rowIniTabla, colAdmitancia_B].Value = "Admitancia";
            ws.Cells[rowIniTabla, colTap1].Value = "Tap1";
            ws.Cells[rowIniTabla, colTap2].Value = "Tap2";
            ws.Cells[rowIniTabla, colPMax].Value = "PMáx";

            var rangoCab = ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colPMax];
            rangoCab.SetAlignment();

            UtilExcel.BorderAround(ws.Cells[rowIniTabla, colIdbarra, rowIniTabla, colPMax], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in listaTrafo3Activas)
            {
                ws.Cells[rowData, colIdbarra].Value = item.Pfrentid;
                ws.Cells[rowData, colIdbarra1].Value = item.Idbarra1;
                ws.Cells[rowData, colIdbarra2].Value = item.Idbarra2;
                ws.Cells[rowData, colNombrebarra1].Value = item.Idbarra1desc;
                ws.Cells[rowData, colNombrebarra2].Value = item.Idbarra2desc;
                ws.Cells[rowData, colResistencia_R].Value = item.Resistencia;
                ws.Cells[rowData, colResistencia_R].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colReactancia_X].Value = item.Reactancia;
                ws.Cells[rowData, colReactancia_X].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colConductancia_G].Value = item.Conductancia;
                ws.Cells[rowData, colConductancia_G].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colAdmitancia_B].Value = item.Admitancia;
                ws.Cells[rowData, colAdmitancia_B].Style.Numberformat.Format = "0.0000E+00";
                ws.Cells[rowData, colTap1].Value = item.Tap1;
                ws.Cells[rowData, colTap2].Value = item.Tap2;
                ws.Cells[rowData, colPMax].Value = item.Potenciamax;

                rowData++;

            }

            var rangotabla = ws.Cells[rowIniTabla + 1, colIdbarra, rowData, colPMax];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            //ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(2, 1);
        }

        /// <summary>
        /// Genera el archivo Excel para CompDinamica, por escenario
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="regEscenario"></param>
        private void GenerarHojaExcelLvtpOpfComDinamica(ExcelPackage xlPackage, PfrEscenarioDTO regEscenario)
        {
            //Obtenemos todo el listado de barras
            DateTime primerDiaEscenario = regEscenario.Pfrescfecini;
            DateTime ultimoDiaEscenario = regEscenario.Pfrescfecfin;

            var listaCompDinamicaActivas = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica, primerDiaEscenario, ultimoDiaEscenario);

            ExcelWorksheet ws = null;

            string nameWS = "ComDinamica";

            this.GenerarHojaExcelLvtpOpfComDinamica(ref ws, xlPackage, nameWS, listaCompDinamicaActivas);
        }

        /// <summary>
        /// Genera la relacion de CompDinamica en un formato excel 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="listaCompDinamicaActivas"></param>
        private void GenerarHojaExcelLvtpOpfComDinamica(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS, List<PfrEntidadDTO> listaCompDinamicaActivas)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            string fontFamTitle = "Arial Narrow";
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 8);
            var fuenteTitulo = new Font(fontFamTitle, 11);
            var fuenteInforme = new Font(fontFamTitle, 8);

            int colIniTable = 1;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 10, 18, 15, 15, 15, 15 })//columna A-I
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }

            #region  Filtros y Cabecera

            var colIdComD = colIniTable;
            int colIdBarra = colIniTable + 1;
            int colNombreBarra = colIniTable + 2;
            int colTension = colIniTable + 3;
            int colQmax = colIniTable + 4;
            int colQmin = colIniTable + 5;

            int rowIniTabla = 1;
            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colIdComD].Value = "ID";
            ws.Cells[rowIniTabla, colIdBarra].Value = "ID Barra";
            ws.Cells[rowIniTabla, colNombreBarra].Value = "Nombre Barra";
            ws.Cells[rowIniTabla, colTension].Value = "Tensión";
            ws.Cells[rowIniTabla, colQmax].Value = "VMáx";
            ws.Cells[rowIniTabla, colQmin].Value = "Vmín";

            var rangoCab = ws.Cells[rowIniTabla, colIdComD, rowIniTabla, colQmin];
            rangoCab.SetAlignment();

            UtilExcel.BorderAround(ws.Cells[rowIniTabla, colIdComD, rowIniTabla, colQmin], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in listaCompDinamicaActivas)
            {
                ws.Cells[rowData, colIdComD].Value = item.Pfrentid;
                ws.Cells[rowData, colIdBarra].Value = item.Idbarra1;
                ws.Cells[rowData, colNombreBarra].Value = item.Idbarra1desc;
                ws.Cells[rowData, colTension].Value = item.TensionBarra1;
                ws.Cells[rowData, colTension, rowData, colTension].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowData, colQmax].Value = item.Qmax;
                ws.Cells[rowData, colQmax, rowData, colQmax].Style.Numberformat.Format = "#,##0.000";
                ws.Cells[rowData, colQmin].Value = item.Qmin;
                ws.Cells[rowData, colQmin, rowData, colQmin].Style.Numberformat.Format = "#,##0.000";

                rowData++;

            }


            var rangotabla = ws.Cells[rowIniTabla + 1, colIdComD, rowData, colQmin];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            //ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(2, 1);
        }

        /// <summary>
        /// Genera el archivo Excel para CompDinamica, por escenario
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="regEscenario"></param>
        private void GenerarHojaExcelLvtpOpfCongestion(ExcelPackage xlPackage, PfrEscenarioDTO regEscenario, int escenarioCodiAux1Salida)
        {
            //Obtenemos todo el listado de barras
            DateTime primerDiaEscenario = regEscenario.Pfrescfecini;
            DateTime ultimoDiaEscenario = regEscenario.Pfrescfecfin;

            List<PfrEntidadDTO> listaCongestionActivas = ListarEntidadVigente((int)ConstantesPotenciaFirmeRemunerable.Tipo.Congestion, primerDiaEscenario, ultimoDiaEscenario);
            List<PfrResultadosGamsDTO> listaSalidas = ListPfrResultadosGamsByTipoYEscenario(escenarioCodiAux1Salida, (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Congestion);

            ExcelWorksheet ws = null;

            string nameWS = "Congestion";

            this.GenerarHojaExcelLvtpOpfCongestion(ref ws, xlPackage, nameWS, listaCongestionActivas, listaSalidas);
        }

        /// <summary>
        /// Genera la relacion de Congestion en un formato excel 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="listaCongestionActivas"></param>
        private void GenerarHojaExcelLvtpOpfCongestion(ref ExcelWorksheet ws, ExcelPackage xlPackage, string nameWS
                    , List<PfrEntidadDTO> listaCongestionActivas, List<PfrResultadosGamsDTO> listaSalidas)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            string fontFamTitle = "Arial Narrow";
            string fontFamily = "Arial Narrow";

            var fuenteTabla = new Font(fontFamily, 8);
            var fuenteTitulo = new Font(fontFamTitle, 11);
            var fuenteInforme = new Font(fontFamTitle, 8);

            int colIniTable = 1;
            int colIniDynamic = colIniTable;


            foreach (var columnWidth in new List<double>() { 7, 20, 10, 10, 8, 60 })//columna A-I
            {
                ws.Column(colIniDynamic++).SetTrueColumnWidth(columnWidth);
            }

            #region  Filtros y Cabecera

            var colIdCongestion = colIniTable;
            int colEnlace = colIniTable + 1;
            int colPMax = colIniTable + 2;
            int colPMin = colIniTable + 3;
            int colLineas = colIniTable + 5;
            int colSalidaGams = colIniTable + 7;

            int rowIniTabla = 1;
            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colIdCongestion].Value = "ID";
            ws.Cells[rowIniTabla, colEnlace].Value = "Enlace";
            ws.Cells[rowIniTabla, colPMax].Value = "PMáx";
            ws.Cells[rowIniTabla, colPMin].Value = "PMín";
            ws.Cells[rowIniTabla, colLineas].Value = "Líneas";
            ws.Cells[rowIniTabla, colSalidaGams].Value = "Enlaces Congestionados";

            var rangoCab = ws.Cells[rowIniTabla, colIdCongestion, rowIniTabla, colLineas];
            rangoCab.SetAlignment();

            UtilExcel.BorderAround(ws.Cells[rowIniTabla, colIdCongestion, rowIniTabla, colLineas], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in listaCongestionActivas)
            {
                var regSalida = listaSalidas.Find(v => v.Pfrcgtcodi == item.Pfrentcodi);
                string lineas = (item.Lineasdesc).Replace("-", "  -  ");
                ws.Cells[rowData, colIdCongestion].Value = item.Pfrentid;
                ws.Cells[rowData, colEnlace].Value = item.Pfrentnomb;
                ws.Cells[rowData, colPMax].Value = item.Potenciamax;
                //ws.Cells[rowData, colPMax, rowData, colPMax].Style.Numberformat.Format = "#,##0.000";
                ws.Cells[rowData, colPMin].Value = item.Potenciamin;
                //ws.Cells[rowData, colPMin, rowData, colPMin].Style.Numberformat.Format = "#,##0.000";
                ws.Cells[rowData, colLineas].Value = lineas;
                ws.Cells[rowData, colSalidaGams].Value = regSalida != null ? regSalida.Pfrrgresultado : null;
                rowData++;

            }
            CeldasExcelColorFondo(ws, rowIniTabla, colSalidaGams, rowData, colSalidaGams, ConstantesPotenciaFirmeRemunerable.ColorSalidasGams);

            var rangotabla = ws.Cells[rowIniTabla + 1, colIdCongestion, rowData, colLineas];
            rangotabla.SetFont(fuenteTabla);

            #endregion

            //ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(2, 1);
        }

        private void GenerarHojaExcelAUX2_Lvtp_Opf(ExcelPackage xlPackage, PfrReporteDTO regReporte, int pfrrptcodiDatos, PfrPeriodoDTO regPeriodo, PfrEscenarioDTO regEscenario)
        {
            //Obtener data de reporte Datos
            List<PestaniaAux2> listaAux2 = ObtenerDataReporteAUX2_Lvtp_Opf(regReporte, pfrrptcodiDatos, regEscenario);

            //Obtengo Periodo Anterior
            DateTime fecPeriodoActual = new DateTime(regPeriodo.Pfrperanio, regPeriodo.Pfrpermes, 1);
            DateTime fecPeriodoAnterior = fecPeriodoActual.AddMonths(-1);

            //Generacion del excel
            var nameWS = "AUX2";
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string fontFamTitle = "Calibri";
            string fontFamily = "Arial";

            var fuenteData = new Font(fontFamily, 8);
            var fuenteTitulo = new Font(fontFamTitle, 10);

            #region   Cabecera Tabla
            int colIniTable = 2;
            int rowIniTable = 5;
            int rowIniData = 0;
            int rowFinData = 0;

            int colCentral = colIniTable;
            int colUnidad = colCentral + 1;
            int colPD = colUnidad + 1;
            int colCV = colPD + 1;
            int colPE = colCV + 1;
            int colPDD = colPE + 1;

            ws.Cells[rowIniTable, colPDD].Value = "Resultado OPF";
            rowIniTable++;

            ws.Cells[rowIniTable, colCentral].Value = "Central";
            ws.Cells[rowIniTable, colUnidad].Value = "Unidad";
            ws.Cells[rowIniTable, colPD].Value = "Potencia Disponible";
            ws.Cells[rowIniTable, colCV].Value = "CV";
            ws.Cells[rowIniTable, colPE].Value = "Potencia Efectiva";
            ws.Cells[rowIniTable, colPDD].Value = "Potencia Disponible Despachada";

            ws.Cells[rowIniTable, colCentral, rowIniTable, colPDD].SetFont(fuenteTitulo);
            ws.Cells[rowIniTable, colCentral, rowIniTable, colPDD].SetFontBold();

            ws.Cells[rowIniTable, colCentral, rowIniTable, colPDD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Column(colCentral).Width = 40;
            ws.Column(colUnidad).Width = 40;
            ws.Column(colPD).Width = 18;
            ws.Column(colCV).Width = 15;
            ws.Column(colPE).Width = 18;
            ws.Column(colPDD).Width = 18;

            rowIniData = rowIniTable + 1;
            #endregion

            #region Cuerpo Tabla
            decimal? totalPD = 0;
            rowIniTable++;
            foreach (var itemGe in listaAux2.GroupBy(x => x.Emprcodi))
            {
                var rowEmpIni = rowIniTable;
                foreach (var regDatos in itemGe)
                {
                    ws.Cells[rowIniTable, colCentral].Value = regDatos.Central;
                    ws.Cells[rowIniTable, colUnidad].Value = regDatos.UnidadNombre;
                    if (regDatos.tieneFicticio)
                    {
                        ExcelRange rg = ws.Cells[rowIniTable, colCentral, rowIniTable, colUnidad];
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFCC"));
                    }

                    ws.Cells[rowIniTable, colPD].Value = regDatos.PD;
                    ws.Cells[rowIniTable, colPD].Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[rowIniTable, colCV].Value = regDatos.CV;
                    ws.Cells[rowIniTable, colCV].Style.Numberformat.Format = "#,##0.0000";
                    ws.Cells[rowIniTable, colPE].Value = regDatos.PE;
                    ws.Cells[rowIniTable, colPE].Style.Numberformat.Format = "#,##0.000";
                    ws.Cells[rowIniTable, colPDD].Value = regDatos.PDD;
                    ws.Cells[rowIniTable, colPDD].Style.Numberformat.Format = "#,##0.000";

                    rowIniTable++;
                    totalPD += regDatos.PD != null ? regDatos.PD : 0;
                }
            }
            //SUMA POTENCIA DISPONIBLE
            ws.Cells[5, colPD].Value = totalPD;

            //color potencia disponible
            ws.Cells[7, colPD, rowIniTable - 1, colPD].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[7, colPD, rowIniTable - 1, colPD].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F2DCDB"));

            //color potencia disponible despachada
            ws.Cells[7, colPDD, rowIniTable - 1, colPDD].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[7, colPDD, rowIniTable - 1, colPDD].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFFCC"));

            rowFinData = rowIniTable;
            ws.Cells[rowIniData, colCentral, rowFinData, colPDD].SetFont(fuenteData);

            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colCentral, rowFinData, colCentral]);
            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colUnidad, rowFinData, colUnidad]);
            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colPD, rowFinData, colPD]);
            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colCV, rowFinData, colCV]);
            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colPE, rowFinData, colPE]);
            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colPDD, rowFinData, colPDD]);

            ws.Cells[rowIniData - 1, colCentral, rowFinData - 1, colPDD].Style.WrapText = true;

            UtilExcel.BorderAround(ws.Cells[rowIniData - 1, colCentral, rowFinData - 1, colPDD], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            UtilExcel.BorderAround(ws.Cells[rowIniData, colCentral, rowFinData, colPDD], OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            ws.Cells[rowIniData - 1, colCentral, rowFinData, colPDD].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            #endregion

        }

        private List<PestaniaAux2> ObtenerDataReporteAUX2_Lvtp_Opf(PfrReporteDTO reporte, int pfrrptcodiDatos, PfrEscenarioDTO regEscenarioDatos)
        {
            DateTime fechaIniRango = regEscenarioDatos.Pfrescfecini;
            DateTime fechaFinRango = regEscenarioDatos.Pfrescfecfin;

            List<PestaniaAux2> listaAUX2 = new List<PestaniaAux2>();

            //Obtener data de reporte Datos
            //List<PestaniaDatos> listaDatos = ObtenerDataDatosPorRangoDeReporte(pfrrptcodiDatos, out List<PrGrupodatDTO> listaParametros);
            List<PfrReporteTotalDTO> listadoGeneralDatos = ListPfrReporteTotalByReportecodi(pfrrptcodiDatos);

            #region lista ReporteTotal de AUX2
            var pfrrptcodiAux2 = this.GetUltimoPfrrptcodiXRecalculo(reporte.Pfrreccodi.Value, ConstantesPotenciaFirmeRemunerable.CuadroPFirmeRemunerable);
            List<PfrReporteTotalDTO> listadoGeneralAUX2 = ListPfrReporteTotalByReportecodi(pfrrptcodiAux2);
            List<PfrEscenarioDTO> listaEscenarioAux2 = ListPfrEscenariosByReportecodi(pfrrptcodiAux2).OrderBy(x => x.Pfrescfecini).ToList();
            int i = 1;
            foreach (var reg in listaEscenarioAux2)
            {
                reg.Numero = i;
                i++;
            }

            //obtener número del escenario de pf
            var escenarioaux2 = listaEscenarioAux2.Where(x => x.Pfrescfecini == fechaIniRango && x.Pfrescfecfin == fechaFinRango).First();
            //var numeroEscenario = escenarioaux2.Numero;
            listadoGeneralAUX2 = listadoGeneralAUX2.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Pfrtotunidadnomb).ToList();
            //hiltrar datos por escenario
            var listaDatosxEscenario = listadoGeneralDatos.Where(x => x.Pfresccodi == regEscenarioDatos.Pfresccodi).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Pfrtotunidadnomb).ToList();
            var prueba = listadoGeneralAUX2.Where(x => x.Pfresccodi == escenarioaux2.Pfresccodi).ToList();

            foreach (var reg in listadoGeneralAUX2.Where(x => x.Pfresccodi == escenarioaux2.Pfresccodi).ToList())
            {
                PestaniaAux2 objAUX2 = new PestaniaAux2();
                var grupocodiReg = reg.Grupocodi;
                var equicodiReg = reg.Equicodi;
                var famcodiReg = reg.Famcodi;

                objAUX2.Emprcodi = reg.Emprcodi.Value;
                objAUX2.Grupocodi = grupocodiReg;
                objAUX2.Equicodi = equicodiReg;
                objAUX2.Empresa = reg.Emprnomb;
                objAUX2.Central = reg.Central;
                objAUX2.UnidadNombre = reg.Pfrtotunidadnomb;
                objAUX2.tieneFicticio = reg.Pfrtotficticio == 1 ? true : false;

                objAUX2.PD = reg.Pfrtotpd;
                objAUX2.PDD = reg.Pfrtotpdd;

                var regDatos = listaDatosxEscenario.Find(x => x.Emprcodi == reg.Emprcodi && x.Equicodi == reg.Equicodi && x.Grupocodi == reg.Grupocodi);

                objAUX2.PE = regDatos.Pfrtotpe;
                objAUX2.CV = regDatos.Pfrtotcv;
                //objAUX2.CR = reg.Pfrtotcrmesant != null ? (reg.Pfrtotcrmesant == 1 ? "CR" : "") : null;

                listaAUX2.Add(objAUX2);

            }
            #endregion

            return listaAUX2;

        }

        private void GenerarHojaExcelReporteCarga(ExcelPackage xlPackage, PfrRecalculoDTO regRecalculo, PfrPeriodoDTO regPeriodo
            , List<BarraSuministro> listaCalculoBarras, List<BarraSSAA> lstBarraSSAA, List<PfrEntidadDTO> listaRelacion
            , List<VtpPeajeEgresoMinfoDTO> listaPeajeEgresoMinfo, List<VtpRetiroPotescDTO> listaRetiroSinContrato)
        {
            var nameWS = "Carga";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            int index = 2;
            int colIniTable = 2;

            int index2 = 7;

            int index3 = 7;

            int index4 = 7;


            #region  Filtros y Cabecera

            var colEmpresa = colIniTable;
            var colCliente = colEmpresa + 1;
            var colBarra = colCliente + 1;
            var colTipoUsu = colBarra + 1;
            var colLicitacion = colTipoUsu + 1;

            var colPrecioPot = colLicitacion + 1;
            var colPotEgreso = colPrecioPot + 1;
            var colPopCalculada = colPotEgreso + 1;
            var colPopDeclarada = colPopCalculada + 1;
            var colPeajeUnit = colPopDeclarada + 1;
            var colCalidad = colPeajeUnit + 1;

            //TITULO
            ws.Cells[index, 3].Value = "INFORMACIÓN INGRESADA PARA VTP Y PEAJES";
            ws.Cells[index + 1, 3].Value = regPeriodo.Pfrpernombre + "/" + regRecalculo.Pfrrecnombre;
            ws.Cells[index + 2, 3].Value = regRecalculo.Pfrrecinforme;
            ws.Cells[index + 3, 3].Value = "CUADRO N° 5";
            ExcelRange rg = ws.Cells[index, 3, index + 3, 3];
            rg.Style.Font.Size = 16;
            rg.Style.Font.Bold = true;

            //CABECERA DE TABLA
            index += 4;
            ws.Cells[index, 7].Value = "PARA EGRESO DE POTENCIA";
            ws.Cells[index, 9].Value = "PARA PEAJE POR CONEXIÓN";
            ws.Cells[index, 12].Value = "PARA FLUJO DE CARGA OPTIMO";

            rg = ws.Cells[index, 7, index, 8];
            rg.Merge = true;
            rg.Style.WrapText = true;
            rg = ObtenerEstiloCelda(rg, 1);

            rg = ws.Cells[index, 9, index, 11];
            rg.Merge = true;
            rg.Style.WrapText = true;
            rg = ObtenerEstiloCelda(rg, 1);

            rg = ws.Cells[index, 12, index, 14];
            rg.Merge = true;
            rg.Style.WrapText = true;
            rg = ObtenerEstiloCelda(rg, 1);
            index++;
            ws.Cells[index, 2].Value = "EMPRESA";
            ws.Cells[index, 3].Value = "CLIENTE";
            ws.Cells[index, 4].Value = "BARRA";
            ws.Cells[index, 5].Value = "TIPO USUARIO";
            ws.Cells[index, 6].Value = "LICITACIÓN";
            ws.Cells[index, 7].Value = "PRECIO POTENCIA S/ /kW-mes";
            ws.Column(7).Style.Numberformat.Format = "#,##0.00";
            ws.Cells[index, 8].Value = "POTENCIA EGRESO kW";
            ws.Column(8).Style.Numberformat.Format = "#,##0.00";
            ws.Cells[index - 2, 8, index - 2, 8].Style.Numberformat.Format = "#,##0.0000";
            ws.Cells[index, 9].Value = "POTENCIA CALCULADA (KW)";
            ws.Column(9).Style.Numberformat.Format = "#,##0.00";
            ws.Cells[index - 2, 9, index - 2, 9].Style.Numberformat.Format = "#,##0.0000";
            ws.Cells[index, 10].Value = "POTENCIA DECLARADA (KW)";
            ws.Column(10).Style.Numberformat.Format = "#,##0.00";
            ws.Cells[index - 2, 10, index - 2, 10].Style.Numberformat.Format = "#,##0.0000";
            ws.Cells[index, 11].Value = "PEAJE UNITARIO S/ /kW-mes";
            ws.Column(11).Style.Numberformat.Format = "#,##0.000";
            ws.Cells[index, 12].Value = "FCO-BARRA";
            ws.Cells[index, 13].Value = "FCO-POTENCIA ACTIVA kW";
            ws.Column(13).Style.Numberformat.Format = "#,##0.00"; ;
            ws.Cells[index, 14].Value = "FCO-POTENCIA REACTIVA kW";
            ws.Column(14).Style.Numberformat.Format = "#,##0.00";
            ws.Cells[index, 15].Value = "CALIDAD";

            rg = ws.Cells[index, 2, index, 15];
            rg.Style.WrapText = true;
            rg = ObtenerEstiloCelda(rg, 1);

            ws.Column(17).Hidden = true;
            ws.Column(18).Hidden = true;
            ws.Column(19).Hidden = true;

            //VTP - GAMS
            ws.Cells[index2, 21].Value = "BARRA (VTP)";
            ws.Cells[index2, 22].Value = "CÓDIGO";
            ws.Cells[index2, 23].Value = "BARRA";
            rg = ws.Cells[index2, 21, index2, 23];
            rg.Style.WrapText = true;
            rg = ObtenerEstiloCelda(rg, 3);

            //BARRAS CÁLCULO
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            ExcelRange valid;
            ws.Cells[2, 27].Value = "Barras";
            rg = ws.Cells[2, 27, 2, 27];
            rg.Style.WrapText = true;
            rg = ObtenerEstiloCelda(rg, 3);
            ws.Cells[2, 28].Value = "BARRAS COMPLETAS";
            valid = ws.Cells[2, 28, 2, 28];
            valid.Style.WrapText = true;
            valid.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            valid.Style.Fill.PatternType = ExcelFillStyle.Solid;
            valid.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#bdecb6 "));
            valid.Style.Font.Size = 11;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

            ws.Cells[index2, 25].Value = "B.Faltante";
            ws.Cells[index2, 26].Value = "CÓDIGO";
            ws.Cells[index2, 27].Value = "NOMBRE GAMS";
            ws.Cells[index2, 28].Value = "BARRAS DE SUMINISTRO";
            ws.Cells[index2, 29].Value = "Pload (MW)";
            ws.Column(29).Style.Numberformat.Format = "#,##0.00";
            ws.Cells[index2 - 1, 29, index2 - 1, 29].Style.Numberformat.Format = "#,##0.0000";
            ws.Cells[index2, 30].Value = "Qload(Mvar)";
            ws.Column(30).Style.Numberformat.Format = "#,##0.00";
            ws.Cells[index2, 31].Value = "f.p";
            rg = ws.Cells[index2, 25, index2, 31];
            rg.Style.WrapText = true;
            rg = ObtenerEstiloCelda(rg, 3);

            //Qload y Pload
            rg = ws.Cells[index2, 29, index2, 29];
            rg = ObtenerEstiloCelda(rg, 4);
            rg = ws.Cells[index2, 30, index2, 30];
            rg = ObtenerEstiloCelda(rg, 4);


            //SSAA
            ws.Cells[index2, 34].Value = "CÓDIGO";
            ws.Cells[index2, 35].Value = "SSAA";
            ws.Column(35).Style.Numberformat.Format = "#,##0.00000";
            ws.Cells[index2 - 1, 35, index2 - 1, 35].Style.Numberformat.Format = "#,##0.00000";
            rg = ws.Cells[index2, 34, index2, 35];
            rg.Style.WrapText = true;
            rg = ObtenerEstiloCelda(rg, 4);


            #endregion

            #region Cuerpo

            index++;
            decimal totalPotenciaEgreso = 0;
            decimal totalPotenciaCalculada = 0;
            decimal totalPotenciaDeclarada = 0;

            if (listaPeajeEgresoMinfo.Count() != 0 || listaRetiroSinContrato.Count() != 0)
            {
                foreach (VtpPeajeEgresoMinfoDTO item in listaPeajeEgresoMinfo)
                {
                    ws.Cells[index, 2].Value = (item.Genemprnombre != null) ? item.Genemprnombre.ToString() : string.Empty;
                    ws.Cells[index, 3].Value = (item.Cliemprnombre != null) ? item.Cliemprnombre.ToString() : string.Empty;
                    ws.Cells[index, 4].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty;
                    ws.Cells[index, 5].Value = (item.Pegrmitipousuario != null) ? item.Pegrmitipousuario.ToString() : string.Empty;
                    ws.Cells[index, 6].Value = (item.Pegrmilicitacion != null) ? item.Pegrmilicitacion.ToString() : string.Empty;
                    ws.Cells[index, 7].Value = (item.Pegrmipreciopote != null) ? item.Pegrmipreciopote : Decimal.Zero;
                    ws.Cells[index, 8].Value = (item.Pegrmipoteegreso != null) ? item.Pegrmipoteegreso : Decimal.Zero;
                    ws.Cells[index, 9].Value = (item.Pegrmipotecalculada != null) ? item.Pegrmipotecalculada : Decimal.Zero;
                    ws.Cells[index, 10].Value = (item.Pegrmipotedeclarada != null) ? item.Pegrmipotedeclarada : Decimal.Zero;
                    ws.Cells[index, 11].Value = (item.Pegrmipeajeunitario != null) ? item.Pegrmipeajeunitario : Decimal.Zero;
                    ws.Cells[index, 12].Value = (item.Barrnombrefco != null) ? item.Barrnombrefco.ToString() : string.Empty;
                    ws.Cells[index, 13].Value = (item.Pegrmipoteactiva != null) ? item.Pegrmipoteactiva : Decimal.Zero;
                    ws.Cells[index, 14].Value = (item.Pegrmipotereactiva != null) ? item.Pegrmipotereactiva : Decimal.Zero;
                    ws.Cells[index, 15].Value = (item.Pegrmicalidad != null) ? item.Pegrmicalidad.ToString() : string.Empty;
                    //Border por celda
                    rg = ws.Cells[index, 2, index, 15];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 0);
                    index++;

                    totalPotenciaEgreso += (item.Pegrmipoteegreso != null) ? (decimal)item.Pegrmipoteegreso : 0;
                    totalPotenciaCalculada += (item.Pegrmipotecalculada != null) ? (decimal)item.Pegrmipotecalculada : 0;
                    totalPotenciaDeclarada += (item.Pegrmipotedeclarada != null) ? (decimal)item.Pegrmipotedeclarada : 0;
                }

                foreach (VtpRetiroPotescDTO item in listaRetiroSinContrato)
                {
                    ws.Cells[index, 2].Value = "RND";
                    ws.Cells[index, 3].Value = (item.Emprnomb != null) ? item.Emprnomb.ToString() : string.Empty;
                    ws.Cells[index, 4].Value = (item.Barrnombre != null) ? item.Barrnombre.ToString() : string.Empty;
                    ws.Cells[index, 5].Value = (item.Rpsctipousuario != null) ? item.Rpsctipousuario.ToString() : string.Empty;
                    ws.Cells[index, 6].Value = "No"; //preguntar
                    ws.Cells[index, 7].Value = (item.Rpscpreciopote != null) ? item.Rpscpreciopote : Decimal.Zero;
                    ws.Cells[index, 8].Value = (item.Rpscpoteegreso != null) ? item.Rpscpoteegreso : Decimal.Zero;
                    ws.Cells[index, 9].Value = (item.Rpscpoteegreso != null) ? item.Rpscpoteegreso : Decimal.Zero;
                    ws.Cells[index, 10].Value = (item.Rpscpoteegreso != null) ? item.Rpscpoteegreso : Decimal.Zero;
                    ws.Cells[index, 11].Value = (item.Rpscpeajeunitario != null) ? item.Rpscpeajeunitario : Decimal.Zero;
                    ws.Cells[index, 12].Value = (item.Barrnombrefco != null) ? item.Barrnombrefco.ToString() : string.Empty;
                    ws.Cells[index, 13].Value = (item.Rpscpoteactiva != null) ? item.Rpscpoteactiva : Decimal.Zero;
                    ws.Cells[index, 14].Value = (item.Rpscpotereactiva != null) ? item.Rpscpotereactiva : Decimal.Zero;
                    ws.Cells[index, 15].Value = (item.Rpsccalidad != null) ? item.Rpsccalidad.ToString() : string.Empty;
                    //Border por celda
                    rg = ws.Cells[index, 2, index, 15];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 0);
                    index++;

                    totalPotenciaEgreso += (item.Rpscpoteegreso != null) ? (decimal)item.Rpscpoteegreso : 0;
                    totalPotenciaCalculada += (item.Rpscpoteegreso != null) ? (decimal)item.Rpscpoteegreso : 0;
                    totalPotenciaDeclarada += (item.Rpscpoteegreso != null) ? (decimal)item.Rpscpoteegreso : 0;
                }
                ////////////////////////////// TOTALES ////////////////////////////////
                var rowCabTot = 5;
                ws.Cells[rowCabTot, colPrecioPot].Value = "Todo (MW)";
                ws.Cells[rowCabTot, colPotEgreso].Value = totalPotenciaEgreso;
                ws.Cells[rowCabTot, colPopCalculada].Value = totalPotenciaCalculada;
                ws.Cells[rowCabTot, colPopDeclarada].Value = totalPotenciaDeclarada;
                /////////////////////////////////////////////////////////////////////
            }

            index2++;
            //LISTADO DE RELACIÓN GAMS CON VTP
            if (listaRelacion.Count() != 0)
            {
                foreach (var item in listaRelacion)
                {
                    ws.Cells[index2, 21].Value = (item.Pfrentunidadnomb != null) ? item.Pfrentunidadnomb.ToString() : string.Empty;
                    ws.Cells[index2, 22].Value = item.Idbarra1;
                    ws.Cells[index2, 23].Value = (item.Idbarra1desc != null) ? item.Idbarra1desc.ToString() : string.Empty;
                    //Border por celda
                    rg = ws.Cells[index2, 21, index2, 23];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 2);
                    index2++;
                }
            }

            index3++;
            decimal totalPload = 0;
            //LISTADO CÁLCULO BARRAS
            if (listaCalculoBarras.Count() != 0)
            {
                foreach (var item in listaCalculoBarras)
                {
                    ws.Cells[index3, 25].Value = (item.Faltante != null) ? item.Faltante.ToString() : string.Empty;
                    ws.Cells[index3, 26].Value = item.IdGams;
                    ws.Cells[index3, 27].Value = (item.NombreGams != null) ? item.NombreGams.ToString() : string.Empty;
                    ws.Cells[index3, 28].Value = (item.NombreBarraGams != null) ? item.NombreBarraGams.ToString() : string.Empty;
                    ws.Cells[index3, 29].Value = (item.Pload != null) ? item.Pload : Decimal.Zero;
                    if (item.Pload != null && item.Pload < 0)
                        ws.Cells[index3, 29, index3, 29].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffb6c1"));

                    ws.Cells[index3, 30].Value = (item.Qload != null) ? item.Qload : Decimal.Zero;
                    if (item.Qload != null && item.Qload < 0)
                        ws.Cells[index3, 30, index3, 30].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffb6c1"));

                    ws.Cells[index3, 31].Value = (item.Fp != null) ? item.Fp : Decimal.Zero;
                    //Border por celda
                    rg = ws.Cells[index3, 25, index3, 31];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 2);

                    if (item.IdGams == "0")
                    {
                        ws.Cells[2, 28].Value = "AGREGAR BARRA";
                        valid.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffb6c1"));

                        ws.Cells[index3, 27, index3, 27].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#dc143c"));
                    }

                    index3++;
                    totalPload += (item.Pload != null) ? (decimal)item.Pload : 0;
                }
                ////////////////////////////// TOTALES ////////////////////////////////
                var rowCabTotPload = 6;
                ws.Cells[rowCabTotPload, 29].Value = totalPload;
                rg = ws.Cells[rowCabTotPload, 29, rowCabTotPload, 29];
                rg.Style.WrapText = true;
                rg = ObtenerEstiloCelda(rg, 3);
                /////////////////////////////////////////////////////////////////////
            }

            index4++;
            decimal totalSSAA = 0;
            //LISTADO DE SSAA
            if (listaRelacion.Count() != 0)
            {
                foreach (var item in lstBarraSSAA)
                {
                    ws.Cells[index4, 34].Value = item.IdBGams;
                    ws.Cells[index4, 35].Value = (item.Ssaa != null) ? item.Ssaa : Decimal.Zero;
                    //Border por celda
                    rg = ws.Cells[index4, 34, index4, 35];
                    rg.Style.WrapText = true;
                    rg = ObtenerEstiloCelda(rg, 2);
                    index4++;

                    totalSSAA += (item.Ssaa != null) ? (decimal)item.Ssaa : 0;
                }
                ////////////////////////////// TOTALES ////////////////////////////////
                var rowCabTotSSAA = 6;
                ws.Cells[rowCabTotSSAA, 35].Value = totalSSAA;
                /////////////////////////////////////////////////////////////////////
            }

            #endregion

            //Fijar panel
            //ws.View.FreezePanes(7, 0);
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 30;
            ws.Column(4).Width = 20;
            ws.Column(5).Width = 10;
            ws.Column(6).Width = 12;
            ws.Column(7).Width = 15;
            ws.Column(8).Width = 18;
            ws.Column(9).Width = 18;
            ws.Column(10).Width = 18;
            ws.Column(11).Width = 15;
            ws.Column(12).Width = 20;
            ws.Column(13).Width = 15;
            ws.Column(14).Width = 15;
            ws.Column(15).Width = 20;

            ws.Column(21).Width = 25;
            ws.Column(22).Width = 15;
            ws.Column(23).Width = 25;

            ws.Column(25).Width = 15;
            ws.Column(26).Width = 15;
            ws.Column(27).Width = 25;
            ws.Column(28).Width = 30;
            ws.Column(29).Width = 14;
            ws.Column(30).Width = 14;
            ws.Column(31).Width = 12;

            ws.Column(34).Width = 20;
            ws.Column(35).Width = 15;

            ws.Column(12).Hidden = true;
            ws.Column(13).Hidden = true;
            ws.Column(14).Hidden = true;

        }

        #endregion

        /// <summary>
        /// Estilo del excel 
        /// 0: Celdas
        /// 1: Titulos        
        /// </summary>
        /// <param name="rango"></param>
        /// <param name="seccion"></param>
        /// <returns></returns>
        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rango.Style.Font.Size = 11;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;

                string colorborder = "#245C86";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 11;
                rango.Style.Font.Bold = true;

                string colorborder = "#DADAD9";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rango.Style.Font.Size = 11;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;

                string colorborder = "#000000";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            if (seccion == 3)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C4D4E9"));
                rango.Style.Font.Color.SetColor(Color.Black);
                rango.Style.Font.Size = 11;
                rango.Style.Font.Bold = true;

                string colorborder = "#000000";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            if (seccion == 4)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF00"));
                rango.Style.Font.Color.SetColor(Color.Black);
                rango.Style.Font.Size = 11;
                rango.Style.Font.Bold = true;

                string colorborder = "#000000";

                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            if (seccion == 5) //Pestaña Demanda
            {
                //rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF00"));
                rango.Style.Font.Color.SetColor(Color.Black);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;

                string colorborder = "#000000";

                //rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                //rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }

        /// <summary>
        /// Da color al fonde en una celda excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaIni"></param>
        /// <param name="coluIni"></param>
        /// <param name="filaFin"></param>
        /// <param name="coluFin"></param>
        /// <param name="color"></param>
        public void CeldasExcelColorFondo(ExcelWorksheet ws, int filaIni, int coluIni, int filaFin, int coluFin, string color)
        {
            var bloque = ws.Cells[filaIni, coluIni, filaFin, coluFin];
            bloque.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            if (color.Contains(","))
            {
                bloque.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                bloque.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(Int32.Parse(color)));
            }
            if (color.Contains("#"))
            {
                bloque.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                bloque.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(color));
            }
        }

        /// <summary>
        /// Generar archivo comprimido de los archivos GAMS
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pfrreccodi"></param>
        /// <param name="nameFile"></param>
        public void GenerarGamsZip(string rutaLocal, int pfrreccodi, out string nameFile)
        {
            try
            {
                PfrRecalculoDTO objPfrRecalculo = GetByIdPfrRecalculo(pfrreccodi);
                PfrPeriodoDTO per = GetByIdPfrPeriodo(objPfrRecalculo.Pfrpercodi);

                string path = ConfigurationManager.AppSettings[ConstantesPotenciaFirmeRemunerable.PathPotRemunerable];
                var directorioRecalculo = $@"{per.FechaIni.Year}\{per.FechaIni.Month:D2}\{objPfrRecalculo.Pfrrecnombre}\";

                var nombreZip = $"{per.FechaIni:yyyy-MM}_{objPfrRecalculo.Pfrrecnombre}.zip";
                var rutaZip = rutaLocal + nombreZip;

                if (File.Exists(rutaZip)) File.Delete(rutaZip);

                FileServer.CreateZipFromDirectory(directorioRecalculo, rutaZip, path);

                nameFile = nombreZip;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Resultado

        /// <summary>
        /// Tab resultado
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pfrrptcodiAux2"></param>
        /// <returns></returns>
        public string GenerarHtmlTabSalida(string url, int pfrrptcodiAux2)
        {
            List<PfrEscenarioDTO> listaEscenarioAux2 = ListPfrEscenariosByReportecodi(pfrrptcodiAux2).OrderBy(x => x.Pfrescfecini).ToList();

            List<dynamic> listaTabla = new List<dynamic>()
            {
                new { Codigo = 0, Descripcion = "Barras" },
                new { Codigo = 1, Descripcion = "Demanda" },
                new { Codigo = 2, Descripcion = "Generación" },
                new { Codigo = 3, Descripcion = "ComDinámica" },
                new { Codigo = 4, Descripcion = "Linea" },
                new { Codigo = 5, Descripcion = "Trafo2" },
                new { Codigo = 6, Descripcion = "Trafo3" },
                //new { Codigo = 7, Descripcion = "Diagrama Unifilar" },
                new { Codigo = 8, Descripcion = "Congestión" },
                new { Codigo = 9, Descripcion = "Aux2" },
                new { Codigo = 10, Descripcion = "Carga" },
                new { Codigo = 11, Descripcion = "LVTP-OPF" },
            };

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional tabla_reporte_excel' style='width: auto; min-width: 500px' role='presentation' >");

            str.Append("<thead>");
            #region cabecera

            str.Append("<tr>");
            str.AppendFormat("<th colspan='{0}'>DESCARGA EXCEL</th>", listaEscenarioAux2.Count);
            str.Append("<th rowspan='2'>REPORTE LVTP-OPF</th>");
            str.Append("</tr>");

            str.Append("<tr>");
            foreach (var item in listaEscenarioAux2)
            {
                str.AppendFormat("<th scope='col' style='width:20px'>{0}</th>", item.Pfrescdescripcion.ToUpper());
            }
            str.Append("</tr>");

            #endregion
            str.Append("</thead>");


            str.Append("<tbody>");

            foreach (var item in listaTabla)
            {
                str.Append("<tr>");
                int numEscenario = 0;
                foreach (var item2 in listaEscenarioAux2)
                {
                    numEscenario++;
                    str.AppendFormat("<td><a href='JavaScript:descargarReporteLVTPOPF({0},{1});' title='Descargar en formato Excel'><img src='{2}Content/Images/exportarExcel.png' alt='logo'></a></td>", item.Codigo, numEscenario, url);
                }
                str.AppendFormat("<td>{0}</td>", item.Descripcion);
                str.Append("</tr>");
            }
            str.Append("</tbody>");

            return str.ToString();
        }

        #region Handson PFR

        public HandsonModel GenerarHandsonPFR(int pfrrptcodi)
        {
            //PfrRelacionPotenciaFirmeDTO regRelacionPF = GetByCriteriaPfrRelacionPotenciaFirmes(pfrrptcodi).FirstOrDefault();
            //if (regRelacionPF == default(PfrRelacionPotenciaFirmeDTO)) throw new ArgumentException("No existe relación con Potencia Firme");

            List<PfrReporteTotalDTO> listadoDatos = ListPfrReporteTotalByReportecodi(pfrrptcodi).OrderBy(x => x.Central).ThenBy(x => x.Pfrtotunidadnomb).ToList();
            List<PfrEscenarioDTO> listaEscenario = ListPfrEscenariosByReportecodi(pfrrptcodi).OrderBy(x => x.Pfrescfecini).ToList();


            var nestedHeader = new NestedHeaders();

            #region Cabecera

            var headerRow1 = new List<CellNestedHeader>()
            {
                new CellNestedHeader(){ Label = "CENTRAL" }, new CellNestedHeader(){ Label = "UNIDAD" },
                new CellNestedHeader(){ Label = "EQUIPADRE" }, new CellNestedHeader(){ Label = "EQUICODI" }, new CellNestedHeader(){ Label = "GRUPOCODI" }
            };

            var headerRow2 = new List<CellNestedHeader>()
            {
                new CellNestedHeader() { Label = "" }, new CellNestedHeader() { Label = "" }, new CellNestedHeader() { Label = "" }, new CellNestedHeader() { Label = "" }, new CellNestedHeader() { Label = "" }
            };

            #endregion

            #region Columna

            var lstColumn = new List<object>()
            {
                new { data = "Central", className = "htLeft" , readOnly = true, editor = false, renderer = "repintadoRenderer"},
                new { data = "Pfrtotunidadnomb", className = "htLeft" , readOnly = true, editor = false, renderer = "repintadoRenderer" },
                new { data = "Equipadre" },
                new { data = "Equicodi" },
                new { data = "Grupocodi" }
            };

            var lstColumnWidth = new List<int> { 300, 250, 0, 0, 0 };

            foreach (var regEsc in listaEscenario)
            {
                headerRow1.Add(new CellNestedHeader() { Label = regEsc.FechaDesc, Colspan = 4 });

                headerRow2.Add(new CellNestedHeader() { Label = "Potencia<br>Disponible<br>(kW)" });
                headerRow2.Add(new CellNestedHeader() { Label = "Pot. Disponible<br>Despachada<br>(kW)" });
                headerRow2.Add(new CellNestedHeader() { Label = "Pot. Firme<br>(kW)" });
                headerRow2.Add(new CellNestedHeader() { Label = "Pot. Firme<br>Remunerable<br>(kW)" });


                lstColumn.Add(new { data = $"{regEsc.Pfresccodi}.Pfrtotpd", className = "htRight", numericFormat = new { pattern = new { thousandSeparated = true, mantissa = 0 } }, type = "numeric", validator = "validatorDecimal" });
                lstColumn.Add(new { data = $"{regEsc.Pfresccodi}.Pfrtotpdd", className = "htRight", numericFormat = new { pattern = new { thousandSeparated = true, mantissa = 0 } }, type = "numeric", validator = "validatorDecimal" });
                lstColumn.Add(new { data = $"{regEsc.Pfresccodi}.Pfrtotpf", className = "htRight", numericFormat = new { pattern = new { thousandSeparated = true, mantissa = 0 } }, type = "numeric", validator = "validatorDecimal" });
                lstColumn.Add(new { data = $"{regEsc.Pfresccodi}.Pfrtotpfr", className = "htRight", numericFormat = new { pattern = new { thousandSeparated = true, mantissa = 0 } }, type = "numeric", validator = "validatorDecimal" });

                lstColumnWidth.AddRange(new List<int> { 100, 100, 100, 100 });
            }

            #endregion

            nestedHeader.ListCellNestedHeaders.Add(headerRow1);
            nestedHeader.ListCellNestedHeaders.Add(headerRow2);

            List<ExpandoObject> listaJson = new List<ExpandoObject>();

            foreach (var reg in listadoDatos.GroupBy(x => new { x.Equipadre, x.Equicodi, x.Grupocodi, x.Pfrtotunidadnomb }))
            {
                var regUnidad = reg.First();

                dynamic data = new ExpandoObject();
                data.Central = regUnidad.Central;
                data.Pfrtotunidadnomb = regUnidad.Pfrtotunidadnomb;
                data.Equipadre = regUnidad.Equipadre;
                data.Equicodi = regUnidad.Equicodi;
                data.Grupocodi = regUnidad.Grupocodi;

                foreach (var regEsc in listaEscenario)
                {

                    var dataEsc = reg.FirstOrDefault(x => x.Pfresccodi == regEsc.Pfresccodi);

                    AddProperty(data, $"{regEsc.Pfresccodi}", new { Pfrtotpd = dataEsc?.Pfrtotpd, Pfrtotpdd = dataEsc?.Pfrtotpdd, Pfrtotpf = dataEsc?.Pfrtotpf, Pfrtotpfr = dataEsc?.Pfrtotpfr });
                }

                listaJson.Add(data);
            }


            HandsonModel handson = new HandsonModel();
            handson.NestedHeader = nestedHeader;
            handson.ListaExcelData2 = JsonConvert.SerializeObject(listaJson);
            handson.ListaColWidth = lstColumnWidth;
            handson.Columnas = lstColumn.ToArray();

            return handson;
        }

        public string GenerarHtmlListadoVersionesPFR(string url, bool tienePermisoEditar, int recacodi)
        {
            List<PfrReporteDTO> listaVersiones = GetByCriteriaPfrReportes(recacodi, ConstantesPotenciaFirmeRemunerable.CuadroPFirmeRemunerable);

            string colorFondo = "";
            var colorEsFinal = "#C6E0B4";
            var colorNoFinal = "##F2F5F7";

            StringBuilder str = new StringBuilder();
            str.Append("<table width='818px' class='pretty tabla-adicional tabla_version_x_recalculo' border='0' cellspacing='0' >");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 80px;'>Acciones</th>");
            str.Append("<th style='width: 80px;'>N° Código Reporte</th>");
            str.Append("<th style='width: 150px;'>Estado</th>");
            str.Append("<th style='width: 200px;'>Usuario creación</th>");
            str.Append("<th style='width: 250px;'>Fecha creación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var reg in listaVersiones.OrderByDescending(x => x.Pfrrptcodi).ToList())
            {
                if (reg.Pfrrptesfinal == 1)  //Es Final
                    colorFondo = colorEsFinal;
                if (reg.Pfrrptesfinal == 0)
                    colorFondo = colorNoFinal;

                str.AppendFormat("<tr style='background-color: {0};'>", colorFondo);
                //Acciones
                str.AppendFormat("<td style='width: 80px; background-color: {0};'>", colorFondo);
                str.AppendFormat("<a class='' href='JavaScript:verPFPorVersion(" + reg.Pfrrptcodi + ");' style='margin-right: 4px;'><img style='margin-left: 30px; margin-top: 4px; margin-bottom: 4px; cursor:pointer;' src='" + url + "Content/Images/btn-open.png' alt='Ver versión' title='Ver versión' /></a>");
                str.Append("</td>");

                //Codigo Reporte
                str.AppendFormat("<td class='' style='width: 80px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfrrptcodi);

                //Estado
                str.AppendFormat("<td class='' style='width: 150px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfrrptesfinal == 0 ? "No Final" : "Es Final");

                //Usuario creacion
                str.AppendFormat("<td class='' style='width: 200px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfrrptusucreacion);

                //Fecha Creacion
                str.AppendFormat("<td class='' style='width: 250px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfrrptfeccreacion);

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Editar la potencia firme Remunerable
        /// </summary>
        /// <param name="pfrptcodi"></param>
        /// <param name="listaPFEdicion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int EditarReportePFRemunerableTransaccional(int pfrrptcodi, int pfrreccodi, List<PfrReporteTotalDTO> listaPFEdicion, string usuario)
        {
            PfrRecalculoDTO objPfrRecalculo = GetByIdPfrRecalculo(pfrreccodi);
            PfrPeriodoDTO objPfrPeriodo = GetByIdPfrPeriodo(objPfrRecalculo.Pfrpercodi);
            List<PfrRelacionIndisponibilidadDTO> lstRelInd = GetByCriteriaPfrRelacionIndisponibilidads(pfrrptcodi);
            List<PfrRelacionPotenciaFirmeDTO> lstRePf = GetByCriteriaPfrRelacionPotenciaFirmes(pfrrptcodi);
            List<PfrReporteTotalDTO> listadoDatos = ListPfrReporteTotalByReportecodi(pfrrptcodi);

            //obtener la información que se guardó anteriormente para el reporte editado
            PfrReporteDTO regReporteBD = GetByIdPfrReporte(pfrrptcodi);

            PfrReporteDTO regPfrReporteAux2 = new PfrReporteDTO()
            {
                Pfrcuacodi = ConstantesPotenciaFirmeRemunerable.CuadroPFirmeRemunerable,
                Pfrreccodi = regReporteBD.Pfrreccodi,
                Pfrrptcr = regReporteBD.Pfrrptcr,
                Pfrrptca = regReporteBD.Pfrrptca,
                Pfrrptmr = regReporteBD.Pfrrptmr,
                Pfrrptmd = regReporteBD.Pfrrptmd,
                Pfrrptfecmd = regReporteBD.Pfrrptfecmd,
                Pfrrptesfinal = ConstantesPotenciaFirme.EsVersionGenerado,
                Pfrrptusucreacion = usuario,
                Pfrrptfeccreacion = DateTime.Now,
            };

            //lista de escenarios
            List<PfrEscenarioDTO> listaEscenario = ListPfrEscenariosByReportecodi(pfrrptcodi).OrderBy(x => x.Pfrescfecini).ToList();

            foreach (var item in listaEscenario)
            {
                var listaEditXEsc = listaPFEdicion.Where(x => x.Pfresccodi == item.Pfresccodi).ToList();
                var listaDatosXEsc = listadoDatos.Where(x => x.Pfresccodi == item.Pfresccodi).ToList();

                var listaPfrReporteTotal = new List<PfrReporteTotalDTO>();

                foreach (var item2 in listaEditXEsc)
                {
                    var pfrDatos = listaDatosXEsc.FirstOrDefault(x => x.Equipadre == item2.Equipadre && x.Equicodi == item2.Equicodi && x.Pfrtotunidadnomb == item2.Pfrtotunidadnomb);

                    if (pfrDatos != null)
                    {
                        if (item2.Pfrtotpd.HasValue || item2.Pfrtotpdd.HasValue || item2.Pfrtotpf.HasValue || item2.Pfrtotpfr.HasValue)
                        {
                            pfrDatos.Pfrtotpd = item2.Pfrtotpd;
                            pfrDatos.Pfrtotpdd = item2.Pfrtotpdd;
                            pfrDatos.Pfrtotpf = item2.Pfrtotpf;
                            pfrDatos.Pfrtotpfr = item2.Pfrtotpfr;

                            listaPfrReporteTotal.Add(pfrDatos);
                        }
                    }
                    else
                    {
                        if (item2.Pfrtotpd.HasValue || item2.Pfrtotpdd.HasValue || item2.Pfrtotpf.HasValue || item2.Pfrtotpfr.HasValue)
                        {
                            var pfrDatos2 = listadoDatos.FirstOrDefault(x => x.Equipadre == item2.Equipadre && x.Equicodi == item2.Equicodi && x.Pfrtotunidadnomb == item2.Pfrtotunidadnomb);
                            if (pfrDatos2 != null)
                            {
                                var pfrDatos_ = (PfrReporteTotalDTO)pfrDatos2.Clone();
                                pfrDatos_.Pfresccodi = item2.Pfresccodi;
                                pfrDatos_.Pfrtotpd = item2.Pfrtotpd;
                                pfrDatos_.Pfrtotpdd = item2.Pfrtotpdd;
                                pfrDatos_.Pfrtotpf = item2.Pfrtotpf;
                                pfrDatos_.Pfrtotpfr = item2.Pfrtotpfr;

                                listaPfrReporteTotal.Add(pfrDatos_);
                            }
                        }
                    }
                }

                item.ListaPfrReporteTotal = listaPfrReporteTotal;
            }

            regPfrReporteAux2.ListaPfrEscenario = listaEscenario;

            #region IDs Relaciones IND y PF

            int? iReporteCodiFKMesAnterior = lstRelInd.Find(x => x.Pfrrintipo == (int)ConstantesPotenciaFirmeRemunerable.TipoRelacionInd.FactorK)?.Irptcodi;
            int? iReporteCodiCRTFMesAnterior = lstRelInd.Find(x => x.Pfrrintipo == (int)ConstantesPotenciaFirmeRemunerable.TipoRelacionInd.CRTermoFortuita)?.Irptcodi;
            int? iReporteCodiCRTPMesAnterior = lstRelInd.Find(x => x.Pfrrintipo == (int)ConstantesPotenciaFirmeRemunerable.TipoRelacionInd.CRTermoProgramada)?.Irptcodi;
            int? iReporteCodiCRHMesAnterior = lstRelInd.Find(x => x.Pfrrintipo == (int)ConstantesPotenciaFirmeRemunerable.TipoRelacionInd.CRHidro)?.Irptcodi;
            int? ultimoReporteCodiPFXRecalculo = lstRePf.FirstOrDefault()?.Pfrptcodi;

            /** Listado de codis (para guardar Relacion_Ind y Relacion_PF) **/
            int?[] lstCodisForeyKey = new int?[] {
                ultimoReporteCodiPFXRecalculo,
                iReporteCodiFKMesAnterior,
                iReporteCodiCRTFMesAnterior,
                iReporteCodiCRTPMesAnterior,
                iReporteCodiCRHMesAnterior

            };

            #endregion

            int reporteCodiAux2 = this.GuardarReportePFR_BDTransaccional(regPfrReporteAux2, lstCodisForeyKey);
            return reporteCodiAux2;
        }

        /// <summary>
        /// agregar propiedad
        /// </summary>
        /// <param name="expando"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        private void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        #endregion

        #endregion

        #region CONFIGURACION      

        #region PARAMETROS

        public List<PrGrupodatDTO> ListarParametrosConfiguracionPorFecha(DateTime fecha, string conceptos, string grupos = "0")
        {
            List<PrGrupodatDTO> lista = FactorySic.GetPrGrupodatRepository().ParametrosConfiguracionPorFecha(fecha, grupos, conceptos);

            foreach (var reg in lista)
            {
                reg.FechadatDesc = reg.Fechadat != null ? reg.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
                reg.Lastuser = reg.Lastuser != null ? reg.Lastuser.Trim() : "SISTEMA";
                reg.FechaactDesc = reg.Fechaact != null ? reg.Fechaact.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
            }

            return lista;
        }

        /// <summary>
        /// Listar historico de prgrupodat
        /// </summary>
        /// <param name="concepcodi"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ListarGrupodatHistoricoValores(int concepcodi, int grupocodi)
        {
            List<PrGrupodatDTO> lista = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(concepcodi.ToString(), grupocodi);

            foreach (var reg in lista)
            {
                reg.FechadatDesc = reg.Fechadat != null ? reg.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.FechaactDesc = reg.Fechaact != null ? reg.Fechaact.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.EstadoDesc = reg.Deleted == ConstantesMigraciones.GrupodatActivo ? ConstantesAppServicio.ActivoDesc : ConstantesAppServicio.BajaDesc;
                reg.Lastuser = reg.Lastuser != null ? reg.Lastuser.Trim() : "SISTEMA";
            }

            return lista;
        }

        #endregion

        /// <summary>
        /// Obtiene una cadena de grupos <pericodis-fechasIni>
        /// </summary>
        /// <returns></returns>
        public string ObtenerListadoPeriodoFechasInicio()
        {
            string salida = "";

            List<PfrPeriodoDTO> listadoPeriodos = ListPfrPeriodos();

            foreach (var regPeriodo in listadoPeriodos)
            {
                int codi = regPeriodo.Pfrpercodi;
                string mes = regPeriodo.Pfrpermes.ToString("#,##00", CultureInfo.InvariantCulture);
                string anio = regPeriodo.Pfrperanio.ToString();

                string grupoFecha = codi + "/01-" + mes + "-" + anio;
                if (salida == "")
                {
                    salida = grupoFecha;
                }
                salida = salida + "," + grupoFecha;
            }

            return salida;
        }

        /// <summary>
        /// LIsta de unidades OP Comercial
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListaUnidades(DateTime fechaInicio, DateTime fechaFin)
        {
            int aplicativo = ConstantesIndisponibilidades.AppPFR;

            //obtener la lista de unidades para ese escenario
            _indAppService.ListarEqCentralSolarOpComercial(fechaInicio, fechaFin, out List<EqEquipoDTO> istaCentrales1, out List<EqEquipoDTO> listaAllEquipos1, out List<ResultadoValidacionAplicativo> listaMsj1);
            _indAppService.ListarEqCentralEolicaOpComercial(fechaInicio, fechaFin, out List<EqEquipoDTO> istaCentrales2, out List<EqEquipoDTO> listaAllEquipos2, out List<ResultadoValidacionAplicativo> listaMsj2);
            _indAppService.ListarEqCentralHidraulicoOpComercial(fechaInicio, fechaFin, out List<EqEquipoDTO> istaCentrales3, out List<EqEquipoDTO> listaEquiposHidro, out List<ResultadoValidacionAplicativo> listaMsj3);
            _indAppService.ListarUnidadTermicoOpComercial(aplicativo, fechaInicio, fechaFin, out List<EqEquipoDTO> listaUnidadesTermo, out List<EqEquipoDTO> listaEquiposTermicos, out List<ResultadoValidacionAplicativo> listaMsj4);

            List<EqEquipoDTO> listaunidades = new List<EqEquipoDTO>();

            istaCentrales1.Select(x => x.Equinomb = string.Empty).ToList();
            istaCentrales2.Select(x => x.Equinomb = string.Empty).ToList();

            listaunidades.AddRange(istaCentrales1);
            listaunidades.AddRange(istaCentrales2);
            listaunidades.AddRange(istaCentrales3);
            listaunidades.AddRange(listaUnidadesTermo);

            listaunidades = listaunidades.OrderBy(x => x.Emprnomb).ToList();
            return listaunidades;
        }

        /// <summary>
        /// Lista de Brras especificando su categoria
        /// </summary>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> GetListBarrasCategoria(int catecodi)
        {
            var listaBarras = FactorySic.GetPrGrupoRepository().ListaBarraCategoria(catecodi);
            listaBarras = listaBarras.Where(x => x.Gruponomb != null).ToList();
            //listaBarras.RemoveAll(x => x.Gruponomb.Contains("_"));
            listaBarras = listaBarras.Where(x => !x.Gruponomb.StartsWith("_")).ToList();

            return listaBarras;
        }

        public List<EqEquipoDTO> ListaUnidadesSsaa()
        {
            var listaUnidadesSsaa = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(ConstantesPotenciaFirmeRemunerable.Equicodissaa).Where(x => x.Equiestado == ConstantesPotenciaFirmeRemunerable.Activo).ToList();
            return listaUnidadesSsaa.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ToList();
        }

        public List<BarraDTO> ListaBarraVtp()
        {
            //return FactorySic.GetTrnBarraRepository().GetListaBarraArea(ConstantesAppServicio.ParametroDefecto);
            List<BarraDTO> listaFiltrada = new List<BarraDTO>();
            var listaTotal = FactorySic.GetTrnBarraRepository().List();
            listaFiltrada = listaTotal.Where(x => x.BarrEstado == "ACT" && x.BarrTension != null).ToList();
            return listaFiltrada;

        }

        public List<PfrEntidadDTO> ListarEntidadVigente(int pfrcatcodi, DateTime fecIniRangoEscogido, DateTime fecFinRangoEscogido)
        {
            return ObtenerListadoEntidadConPropiedad(pfrcatcodi, fecIniRangoEscogido, fecFinRangoEscogido, 1);
        }

        /// <summary>
        /// Obtener las propiedades vigentes de todos las entidades (o uno en particular)
        /// </summary>
        /// <param name="fechaVigencia"></param>
        /// <param name="pfrentcodi"></param>
        /// <param name="pfrcatcodi"></param>
        /// <returns></returns>
        public List<PfrEntidadDatDTO> ListarPropiedadVigenteXEntidad(DateTime fechaVigencia, string pfrentcodi, int pfrcatcodi)
        {
            return ListarPfrentidadDatVigente(fechaVigencia, pfrentcodi, pfrcatcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pfrentcodi"></param>
        /// <param name="fechaVigencia"></param>
        /// <returns></returns>
        public List<PfrEntidadDatDTO> CompletarListaPropiedadVigente(int pfrentcodi, DateTime fechaVigencia)
        {
            var entidad = GetByIdPfrEntidad(pfrentcodi);
            var listaDat = ListarPropiedadVigenteXEntidad(fechaVigencia, pfrentcodi.ToString(), -1);

            List<PfrConceptoDTO> listaCnp = ListarConceptoXTipo(entidad.Pfrcatcodi);

            foreach (var cnp in listaCnp)
            {
                //si la propiedad no está en bd entonces mostrar la propiedad en vacio

                var existeDat = listaDat.Find(x => x.Pfrcnpcodi == cnp.Pfrcnpcodi) != null;
                if (!existeDat)
                {
                    listaDat.Add(new PfrEntidadDatDTO()
                    {
                        Pfrcatcodi = entidad.Pfrcatcodi,
                        Pfrentcodi = entidad.Pfrentcodi,
                        Pfrcnpcodi = cnp.Pfrcnpcodi,
                        Pfrcnpnomb = cnp.Pfrcnpnomb
                    });
                }
            }

            return listaDat.OrderBy(x => x.Pfrcnpcodi).ToList();
        }

        public List<PfrConceptoDTO> ListarConceptoXTipo(int pfrcatcodi)
        {
            List<int> concepcodis = ListarPrfcnpcodiXTipo(pfrcatcodi);

            var listaBD = ListPfrConceptos().Where(x => concepcodis.Contains(x.Pfrcnpcodi)).ToList();

            return listaBD;
        }

        private List<int> ListarPrfcnpcodiXTipo(int pfrcatcodi)
        {
            List<int> l = new List<int>();
            l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Vigencia);

            switch ((ConstantesPotenciaFirmeRemunerable.Tipo)pfrcatcodi)
            {
                case ConstantesPotenciaFirmeRemunerable.Tipo.Barra:
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Tension);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Vmax);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Vmin);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Compreactiva);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Linea:
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Resistencia);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Reactancia);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Conductancia);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Admitancia);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Potenciamaxima);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2:
                case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3:
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Resistencia);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Reactancia);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Conductancia);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Admitancia);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Tap1);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Tap2);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Potenciamaxima);

                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica:
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Tension);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Qmax);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Qmin);

                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.GamsVtp:

                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.GamsSsaa:

                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos:
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Numunidad);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Qmax);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Qmin);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Ref);

                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Congestion:
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.PMax);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.PMin);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea1);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea2);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea3);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea4);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea5);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea6);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea7);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea8);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea9);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea10);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea11);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea12);

                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Penalidad:
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Penalidad);
                    l.Add((int)ConstantesPotenciaFirmeRemunerable.Concepto.Descripcion);

                    break;
            }

            return l;
        }

        /// <summary>
        /// Valida los equipos
        /// </summary>
        /// <param name="pfrEntidad"></param>
        /// <param name="usuario"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        public int ValidarBDEntidad(PfrEntidadDTO pfrEntidad, int accion)
        {
            int salida = -1;
            try
            {
                //Solo si el EQUIPO es NUEVO
                if (accion == (int)ConstantesPotenciaFirmeRemunerable.Accion.Nuevo)
                {
                    var entNombre = pfrEntidad.Pfrentnomb?.ToUpper().Trim();
                    var entId = pfrEntidad.Pfrentid?.ToUpper().Trim();
                    var entBarragams = pfrEntidad.Pfrentcodibarragams;
                    var entBarragams2 = pfrEntidad.Pfrentcodibarragams2;

                    string fechaVigDesc = pfrEntidad.ListEntidadDat.First().Prfdatfechavigdesc;
                    DateTime prfdatfechavig = DateTime.ParseExact(fechaVigDesc, ConstantesPotenciaFirmeRemunerable.FormatoFecha, CultureInfo.InvariantCulture);

                    var listaEntidad = ListarEntidadVigente(pfrEntidad.Pfrcatcodi, prfdatfechavig, prfdatfechavig);

                    switch (pfrEntidad.Pfrcatcodi)
                    {
                        case (int)ConstantesPotenciaFirmeRemunerable.Tipo.Barra:
                            var entBarra = listaEntidad.FirstOrDefault(x => x.Pfrentid.ToUpper().Trim() == entId);
                            if (entBarra != null) throw new ArgumentException("¡Ya existe una Barra vigente con el mismo Id en la DB!");

                            break;

                        case (int)ConstantesPotenciaFirmeRemunerable.Tipo.Linea:

                            var entLinea = listaEntidad.FirstOrDefault(x => x.Pfrentid.ToUpper().Trim() == entId);
                            if (entLinea != null) throw new ArgumentException("¡Ya existe una Línea Activa con el mismo Id en la DB!");

                            entLinea = listaEntidad.FirstOrDefault(x => x.Pfrentid.ToUpper().Trim() == entId
                                        && x.Pfrentcodibarragams == entBarragams
                                        && x.Pfrentcodibarragams2 == entBarragams2);
                            if (entLinea != null) throw new ArgumentException("¡Ya existe una Línea con el mismo Id, Barra 1 y Barra 2 en la DB!");

                            break;

                        case (int)ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2:
                        case (int)ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3:

                            var entTrafo = listaEntidad.FirstOrDefault(x => x.Pfrentid.ToUpper().Trim() == entId);
                            if (entTrafo != null) throw new ArgumentException("¡Ya existe un Trafo Activo con el mistmo Id en la DB!");

                            entTrafo = listaEntidad.FirstOrDefault(x => x.Pfrentid.ToUpper().Trim() == entId
                                            && x.Pfrentcodibarragams == entBarragams
                                            && x.Pfrentcodibarragams2 == entBarragams2);
                            if (entTrafo != null) throw new ArgumentException("¡Ya existe un Trafo con el mistmo Id, Barra 1 y Barra 2 en la DB!");

                            break;

                        case (int)ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica:
                            var entComp = listaEntidad.FirstOrDefault(x => x.Pfrentid.ToUpper().Trim() == entId);
                            if (entComp != null) throw new ArgumentException("¡Ya existe una Compensación Dinámica Activa con el mistmo Id en la DB!");

                            break;
                    }
                }

                salida = 1;

                return salida;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda la informacion de un equipo
        /// </summary>
        /// <param name="pfrEntidad"></param>
        /// <param name="usuario"></param>
        public void GuardarBDEntidad(PfrEntidadDTO pfrEntidad, string usuario)
        {
            try
            {
                pfrEntidad.Pfrentusucreacion = usuario;
                pfrEntidad.Pfrentfeccreacion = DateTime.Now;
                pfrEntidad.Pfrentestado = (int)ConstantesPotenciaFirmeRemunerable.Estado.Activo;


                var pfrentcodi = SavePfrEntidad(pfrEntidad);

                foreach (var entidadDat in pfrEntidad.ListEntidadDat)
                {
                    entidadDat.Pfrentcodi = pfrentcodi;
                    entidadDat.Pfrdatusucreacion = usuario;
                    entidadDat.Pfrdatfeccreacion = DateTime.Now;
                    entidadDat.Prfdatfechavig = DateTime.ParseExact(entidadDat.Prfdatfechavigdesc, ConstantesPotenciaFirmeRemunerable.FormatoFecha, CultureInfo.InvariantCulture);

                    SavePfrEntidadDat(entidadDat);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public List<PfrEntidadDTO> ObtenerListadoEntidadConPropiedad(int pfrcatcodi, DateTime fechaIni, DateTime fechaFin, int estado)
        {
            var listaEntidades = GetByCriteriaPfrEntidads(pfrcatcodi);

            //propiedades de cada entidad
            var listaEntidadesDat = ListarPropiedadVigenteXEntidad(fechaFin, ConstantesPotenciaFirmeRemunerable.ParametroXDefecto, pfrcatcodi);
            //vigencia 
            var listaDatVigencia = GetByCriteriaPfrEntidadDats(-1, (int)ConstantesPotenciaFirmeRemunerable.Concepto.Vigencia).Where(x => x.Pfrdatdeleted == 0).OrderByDescending(x => x.Prfdatfechavig).ToList();

            //
            if (pfrcatcodi == (int)ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica)
            {
                var barrasgams = listaEntidades.Select(x => x.Pfrentcodibarragams).ToList();
                if (barrasgams.Any())
                    listaEntidadesDat.AddRange(ListarPropiedadVigenteXEntidad(fechaFin, string.Join(",", barrasgams), (int)ConstantesPotenciaFirmeRemunerable.Tipo.Barra));
            }

            //if (pfrcatcodi == (int)ConstantesPotenciaFirmeRemunerable.Tipo.Congestion)
            //{
            //    var lineas = new List<int>()
            //        {
            //            (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea1, (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea2, (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea3, (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea4,
            //            (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea5, (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea6, (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea7, (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea8,
            //            (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea9, (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea10, (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea11, (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea12,
            //        };

            //    var lineas_ = listaEntidadesDat.Where(x => lineas.Contains(x.Pfrcnpcodi)).ToList();
            //    if (lineas_.Any())
            //    {
            //        var joinLineas = string.Join(",", lineas_.Select(x => x.Pfrdatvalor).Distinct());
            //        listaLineasEnt = GetByCriteriaPfrEntidads((int)ConstantesPotenciaFirmeRemunerable.Tipo.Linea, joinLineas);
            //    }
            //}

            foreach (var entidad in listaEntidades)
            {
                var lstDat = listaEntidadesDat.Where(x => x.Pfrentcodi == entidad.Pfrentcodi).ToList();

                SetValorVigencia(entidad.Pfrentcodi, fechaIni, fechaFin, listaDatVigencia, out string opVigencia, out DateTime? fechaInicio, out DateTime? fechaRetiro);
                entidad.Pfrentestado = ConstantesAppServicio.SI == opVigencia ? 1 : 0;
                entidad.VigenciaIni = fechaInicio;
                entidad.VigenciaFin = fechaRetiro;

                //si no está vigente
                if (entidad.Pfrentestado == 0)
                {
                    SetValorVigencia(entidad.Pfrentcodi, DateTime.MinValue, DateTime.MaxValue.AddDays(-1), listaDatVigencia, out string opVigencia2, out DateTime? fechaInicio2, out DateTime? fechaRetiro2);
                    entidad.VigenciaIni = fechaInicio2;
                    entidad.VigenciaFin = fechaRetiro2;
                }

                FormatearEntidad(pfrcatcodi, listaEntidadesDat, entidad, lstDat);
            }

            //filtro para web
            if (estado != -1) //baja
                listaEntidades = listaEntidades.Where(x => x.Pfrentestado == estado).ToList();

            return listaEntidades.OrderBy(x => x.Pfrentid).ToList();
        }

        /// <summary>
        /// Establecer el valor de Operacion comercial
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaOperacionComercial"></param>
        /// <returns></returns>
        private static void SetValorVigencia(int pfrentcodi, DateTime fechaIni, DateTime fechaFin, List<PfrEntidadDatDTO> listaDatVigencia
                                                        , out string opVigencia, out DateTime? fechaInicio, out DateTime? fechaRetiro)
        {
            opVigencia = ConstantesAppServicio.NO;
            fechaInicio = null;
            fechaRetiro = null;

            var listaVigXEnt = listaDatVigencia.Where(x => x.Pfrentcodi == pfrentcodi).ToList();

            var regPrimerDia = listaVigXEnt.Find(x => x.Prfdatfechavig <= fechaIni);
            var regExisteOpEnMes = listaVigXEnt.Find(x => x.Pfrdatvalor == ConstantesAppServicio.SI && fechaIni < x.Prfdatfechavig && x.Prfdatfechavig <= fechaFin);
            var regRetiroOpEnMes = listaVigXEnt.Find(x => x.Pfrdatvalor == ConstantesAppServicio.NO && fechaIni <= x.Prfdatfechavig && x.Prfdatfechavig <= fechaFin.AddDays(1));

            //verificar si en el primer dia del mes tiene o no Op comercial
            if (regPrimerDia != null && regPrimerDia.Pfrdatvalor == ConstantesAppServicio.SI)
            {
                opVigencia = ConstantesAppServicio.SI;
                fechaInicio = regPrimerDia.Prfdatfechavig;
            }

            //si en el mes inicia operacion comercial
            if (regExisteOpEnMes != null)
            {
                opVigencia = ConstantesAppServicio.SI;
                fechaInicio = regExisteOpEnMes.Prfdatfechavig;
            }

            if (regRetiroOpEnMes != null)
                fechaRetiro = regRetiroOpEnMes.Prfdatfechavig;
        }

        private void FormatearEntidad(int pfrcatcodi, List<PfrEntidadDatDTO> listaEntidadesDat, PfrEntidadDTO entidades
                , List<PfrEntidadDatDTO> lstDat)
        {
            string tension = null;

            var fechaCambio = entidades.Pfrentfecmodificacion != null ? entidades.Pfrentfecmodificacion.Value : entidades.Pfrentfeccreacion.Value;
            entidades.Fechaultimamodif = fechaCambio.ToString(ConstantesAppServicio.FormatoFechaFull2);
            entidades.Usuarioultimamodif = entidades.Pfrentfecmodificacion != null ? entidades.Pfrentusumodificacion : entidades.Pfrentusucreacion;
            entidades.Pfrentestadodesc = entidades.Pfrentestado == (int)ConstantesPotenciaFirmeRemunerable.Estado.Activo ? "Vigente" : "No Vigente";

            if (entidades.VigenciaIni != null) entidades.VigenciaIniDesc = entidades.VigenciaIni.Value.ToString(ConstantesAppServicio.FormatoFecha);
            if (entidades.VigenciaFin != null) entidades.VigenciaFinDesc = entidades.VigenciaFin.Value.ToString(ConstantesAppServicio.FormatoFecha);

            switch ((ConstantesPotenciaFirmeRemunerable.Tipo)pfrcatcodi)
            {
                case ConstantesPotenciaFirmeRemunerable.Tipo.Barra:

                    tension = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Tension)?.Pfrdatvalor;
                    var vmax = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Vmax)?.Pfrdatvalor;
                    var vmin = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Vmin)?.Pfrdatvalor;
                    var compReactiva = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Compreactiva)?.Pfrdatvalor;

                    entidades.Tension = Util.ConvertirADecimal(tension);
                    entidades.Vmax = Util.ConvertirADecimal(vmax);
                    entidades.Vmin = Util.ConvertirADecimal(vmin);
                    entidades.Compreactiva = Util.ConvertirADecimal(compReactiva);

                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Linea:
                case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2:
                case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3:

                    var resistencia = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Resistencia)?.Pfrdatvalor;
                    var reactancia = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Reactancia)?.Pfrdatvalor;
                    var conductancia = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Conductancia)?.Pfrdatvalor;
                    var admitancia = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Admitancia)?.Pfrdatvalor;
                    var potenciamax = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Potenciamaxima)?.Pfrdatvalor;
                    var tap1 = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Tap1)?.Pfrdatvalor;
                    var tap2 = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Tap2)?.Pfrdatvalor;

                    entidades.Resistencia = Util.ConvertirADecimal(resistencia);
                    entidades.Reactancia = Util.ConvertirADecimal(reactancia);
                    entidades.Conductancia = Util.ConvertirADecimal(conductancia);
                    entidades.Admitancia = Util.ConvertirADecimal(admitancia);
                    entidades.Potenciamax = Util.ConvertirADecimal(potenciamax);
                    entidades.Tap1 = Util.ConvertirADecimal(tap1);
                    entidades.Tap2 = Util.ConvertirADecimal(tap2);

                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica:
                    tension = listaEntidadesDat.FirstOrDefault(x => x.Pfrentcodi == entidades.Pfrentcodibarragams && x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Tension)?.Pfrdatvalor;
                    var qmax = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Qmax)?.Pfrdatvalor;
                    var qmin = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Qmin)?.Pfrdatvalor;

                    entidades.Tension = Util.ConvertirADecimal(tension);
                    entidades.Qmax = Util.ConvertirADecimal(qmax);
                    entidades.Qmin = Util.ConvertirADecimal(qmin);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos:
                    var qmaxg = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Qmax)?.Pfrdatvalor;
                    var qming = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Qmin)?.Pfrdatvalor;
                    var numunidad = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Numunidad)?.Pfrdatvalor;
                    var ref_ = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Ref)?.Pfrdatvalor;

                    entidades.Qmax = Util.ConvertirADecimal(qmaxg);
                    entidades.Qmin = Util.ConvertirADecimal(qming);
                    entidades.Numunidad = Util.ConvertirAEntero(numunidad);
                    entidades.Ref = Util.ConvertirAEntero(ref_);
                    if (entidades.Pfrentficticio == (int)ConstantesPotenciaFirmeRemunerable.Estado.Activo) entidades.Pfrentunidadnomb += "fk(*)";
                    break;

                case ConstantesPotenciaFirmeRemunerable.Tipo.Congestion:
                    var pmax = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.PMax)?.Pfrdatvalor;
                    var pmin = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.PMin)?.Pfrdatvalor;

                    var conceptoLinea = new List<int>()
                    {
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea1,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea2,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea3,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea4,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea5,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea6,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea7,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea8,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea9,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea10,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea11,
                        (int)ConstantesPotenciaFirmeRemunerable.Concepto.Linea12,
                    };

                    var lineas_ = lstDat.Where(x => conceptoLinea.Contains(x.Pfrcnpcodi)).OrderBy(x => x.Pfrcnpcodi).Select(x => x.Pfrdatvalor).ToList();

                    entidades.Pmax = Util.ConvertirADecimal(pmax);
                    entidades.Pmin = Util.ConvertirADecimal(pmin);
                    if (lineas_.Any())
                    {
                        //var linea = listaLineasEnt.Where(x => lineas_.Contains(x.Pfrentcodi.ToString())).Select(x => x.Pfrentid);
                        //entidades.Lineasdesc = string.Join("-", linea);
                        entidades.Lineasdesc = string.Join("-", lineas_);
                    }

                    break;

                case ConstantesPotenciaFirmeRemunerable.Tipo.Penalidad:
                    var penalidad = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Penalidad)?.Pfrdatvalor;
                    var descripcion = lstDat.FirstOrDefault(x => x.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Descripcion)?.Pfrdatvalor;

                    entidades.Penalidad = penalidad;
                    entidades.Descripcion = descripcion;

                    break;

                default:
                    break;
            }
        }

        public string ObtenerHtmlRelacionEntidad(string url, int pfrcatcodi, int pericodi, int estado)
        {

            var pfrPeriodo = GetByIdPfrPeriodo(pericodi);
            var lstEntidad = ObtenerListadoEntidadConPropiedad(pfrcatcodi, pfrPeriodo.FechaIni, pfrPeriodo.FechaFin, estado);

            string htmlListado = "";
            switch ((ConstantesPotenciaFirmeRemunerable.Tipo)pfrcatcodi)
            {
                case ConstantesPotenciaFirmeRemunerable.Tipo.Barra:
                    htmlListado = GenerarRHtmlRelacionBarras(url, lstEntidad);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Linea:
                    htmlListado = GenerarRHtmlRelacionLineas(url, lstEntidad);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2:
                    htmlListado = GenerarRHtmlRelacionTrafos(url, lstEntidad, ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3:
                    htmlListado = GenerarRHtmlRelacionTrafos(url, lstEntidad, ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica:
                    htmlListado = GenerarRHtmlRelacionCompDinamica(url, lstEntidad);
                    break;
            }
            return htmlListado;
        }

        /// <summary>
        /// Genera el html con el listado de Comp Dinamica 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="listadoBarras"></param>
        /// <returns></returns>
        public string GenerarRHtmlRelacionCompDinamica(string url, List<PfrEntidadDTO> listadoCompDinamica)
        {
            int tipoEntidad = (int)ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica;
            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_lstCompDinamica' style='width:100%'>");

            str.Append("<thead>");
            #region cabecera            

            str.Append("<tr>");
            str.Append("<th style=''>Opciones</th>");
            str.Append("<th style=''>Id</th>");
            str.Append("<th style=''>Id Barra</th>");
            str.Append("<th style=''>Nombre</th>");
            str.Append("<th style=''>Tensión (kV)</th>");
            str.Append("<th style=''>QMáx (MVar)</th>");
            str.Append("<th style=''>QMín (MVar)</th>");
            str.Append("<th style=''>Ini. Vigencia</th>");
            str.Append("<th style=''>Fin Vigencia</th>");
            str.Append("<th style=''>Estado</th>");
            str.Append("<th style=''>Usuario Modificación</th>");
            str.Append("<th style=''>Últ. Modificación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");


            str.Append("<tbody>");

            foreach (var item in listadoCompDinamica)
            {
                string claseFila = item.Pfrentestado == 0 ? "clase_eliminado" : "";
                str.AppendFormat("<tr class='{0}'>", claseFila);
                str.AppendFormat("<td class='btnFilaAcciones' ><a href='JavaScript:VerEquipo({0}, {2});' title='Propiedad Compensación Dinamica'><img src='{1}/btn-properties.png' alt='Propiedad Compensación Dinamica' /></a>", item.Pfrentcodi, $"{url}Content/Images", tipoEntidad);

                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Pfrentid);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Idbarra1);
                str.AppendFormat("<td style=''>{0}</td>", item.Idbarra1desc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Tension);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Qmax);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Qmin);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaIniDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaFinDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Pfrentestadodesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Usuarioultimamodif);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Fechaultimamodif);
                str.Append("</tr>");

            }
            str.Append("</tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Genera el html con el listado de Trafos 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="listadoEquipos"></param>
        /// <returns></returns>
        public string GenerarRHtmlRelacionTrafos(string url, List<PfrEntidadDTO> listadoEquipos, ConstantesPotenciaFirmeRemunerable.Tipo tipo)
        {
            int tipoEntidad = (int)tipo;
            var listaRelacion = listadoEquipos;

            StringBuilder str = new StringBuilder();
            if (tipo == ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2)
                str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_lstTrafos2' style='width:100%'>");

            if (tipo == ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3)
                str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_lstTrafos3' style='width:100%'>");


            str.Append("<thead>");
            #region cabecera            

            str.Append("<tr>");
            str.Append("<th >Opciones</th>");
            str.Append("<th style=''>Id Trafo</th>");
            str.Append("<th style=''>Id1</th>");
            str.Append("<th style=''>Barra1</th>");
            str.Append("<th style=''>Id2</th>");
            str.Append("<th style=''>Barra2</th>");
            str.Append("<th style=''>Resistencia (pu)</th>");
            str.Append("<th style=''>Reactancia (pu)</th>");
            str.Append("<th style=''>Conductancia (pu)</th>");
            str.Append("<th style=''>Admitancia (pu)</th>");
            str.Append("<th style=''>Tap1</th>");
            str.Append("<th style=''>Tap2</th>");
            str.Append("<th style=''>Potencia Máxima (MVA)</th>");
            str.Append("<th style=''>Ini. Vigencia</th>");
            str.Append("<th style=''>Fin Vigencia</th>");
            str.Append("<th style=''>Estado</th>");
            str.Append("<th style=''>Usuario <br>Modificación</th>");
            str.Append("<th style=''>Últ. Modificación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");


            str.Append("<tbody>");

            foreach (var item in listaRelacion)
            {
                string claseFila = item.Pfrentestado == 0 ? "clase_eliminado" : "";
                str.AppendFormat("<tr class='{0}'>", claseFila);
                str.AppendFormat("<td class='btnFilaAcciones' ><a href='JavaScript:VerEquipo({0}, {2});' title='Propiedad Trafo 2D'><img src='{1}/btn-properties.png' alt='Propiedad Trafo 3D' /></a>", item.Pfrentcodi, $"{url}Content/Images", tipoEntidad);

                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Pfrentid);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Idbarra1);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Idbarra1desc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Idbarra2);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Idbarra2desc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Resistencia);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Reactancia);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Conductancia);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Admitancia);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Tap1);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Tap2);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Potenciamax);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaIniDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaFinDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Pfrentestadodesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Usuarioultimamodif);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Fechaultimamodif);
                str.Append("</tr>");

            }
            str.Append("</tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Genera el html con el listado de Lineas 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="listadoEquipos"></param>
        /// <returns></returns>
        public string GenerarRHtmlRelacionLineas(string url, List<PfrEntidadDTO> listadoEquipos)
        {
            int tipoEntidad = (int)ConstantesPotenciaFirmeRemunerable.Tipo.Linea;
            var listaRelacion = listadoEquipos;

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_lstLineas' style='width:100%'>");

            str.Append("<thead>");
            #region cabecera            

            str.Append("<tr>");
            str.Append("<th style='width: 180px;'>Opciones</th>");
            str.Append("<th style=''>Id</th>");
            str.Append("<th style=''>Barra1</th>");
            str.Append("<th style=''>Barra2</th>");
            str.Append("<th style=''>Resistencia <br> (pu)</th>");
            str.Append("<th style=''>Reactancia <br> (pu)</th>");
            str.Append("<th style=''>Conductancia <br>(pu)</th>");
            str.Append("<th style=''>Admitancia <br>(pu)</th>");
            str.Append("<th style=''>Potencia Máxima <br>(MVA)</th>");
            str.Append("<th style=''>Ini. Vigencia</th>");
            str.Append("<th style=''>Fin Vigencia</th>");
            str.Append("<th style=''>Estado</th>");
            str.Append("<th style=''>Usuario Modificación</th>");
            str.Append("<th style=''>Últ. Modificación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");


            str.Append("<tbody>");

            foreach (var item in listaRelacion)
            {
                string claseFila = item.Pfrentestado == 0 ? "clase_eliminado" : "";
                str.AppendFormat("<tr class='{0}'>", claseFila);
                str.AppendFormat("<td class='btnFilaAcciones' ><a href='JavaScript:VerEquipo({0}, {2});' title='Propiedad Lineas'><img src='{1}/btn-properties.png' alt='Propiedad Lineas' /></a>", item.Pfrentcodi, $"{url}Content/Images", tipoEntidad);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Pfrentid);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Idbarra1desc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Idbarra2desc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Resistencia);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Reactancia);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Conductancia);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Admitancia);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Potenciamax);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaIniDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaFinDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Pfrentestadodesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Usuarioultimamodif);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Fechaultimamodif);
                str.Append("</tr>");

            }
            str.Append("</tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Genera el html con el listado de Barras 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="listadoBarras"></param>
        /// <returns></returns>
        public string GenerarRHtmlRelacionBarras(string url, List<PfrEntidadDTO> listadoBarras)
        {
            int tipoEntidad = (int)ConstantesPotenciaFirmeRemunerable.Tipo.Barra;
            var listaRelacion = listadoBarras;

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_lstBarras' style='width:100%'>");

            str.Append("<thead>");
            #region cabecera            

            str.Append("<tr>");
            str.Append("<th >Opciones</th>");
            str.Append("<th >Id</th>");
            str.Append("<th >Nombre</th>");
            str.Append("<th >Tensión (kV)</th>");
            str.Append("<th >VMáx (pu)</th>");
            str.Append("<th >VMín (pu)</th>");
            str.Append("<th >Compensación <br/>Reactiva (MVar)</th>");
            str.Append("<th >Ini. Vigencia</th>");
            str.Append("<th >Fin Vigencia</th>");
            str.Append("<th >Estado</th>");
            str.Append("<th >Usuario Modificación</th>");
            str.Append("<th >Últ. Modificación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");

            foreach (var item in listaRelacion)
            {
                string claseFila = item.Pfrentestado == 0 ? "clase_eliminado" : "";
                str.AppendFormat("<tr class='{0}'>", claseFila);
                str.AppendFormat("<td class='btnFilaAcciones' ><a href='JavaScript:VerEquipo({0}, {2});' title='Propiedad Barra'><img src='{1}/btn-properties.png' alt='Propiedad Barra' /></a>", item.Pfrentcodi, $"{url}Content/Images", tipoEntidad);

                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Pfrentid);
                str.AppendFormat("<td style=''>{0}</td>", item.Pfrentnomb);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Tension);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Vmax);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Vmin);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Compreactiva);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaIniDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaFinDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Pfrentestadodesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Usuarioultimamodif);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Fechaultimamodif);
                str.Append("</tr>");
            }

            str.Append("</tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Html del combo listado barras actualizadas
        /// </summary>
        /// <param name="listadoBarras"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="disabled"></param>
        /// <returns></returns>
        public string HtmlListadoBarrasActualizadas(string listadoBarras, string id, string name, bool disabled)
        {
            string strDisabled = "";
            if (disabled)
                strDisabled = "disabled";

            StringBuilder str = new StringBuilder();

            str.Append("<select id='" + id + "' name='" + name + "' style='width: 170px;' " + strDisabled + ">");
            str.Append(listadoBarras);
            str.Append("</select>");

            return str.ToString();
        }

        /// <summary>
        /// Listado de barras actualizadas en formato html
        /// </summary>
        /// <param name="listadoBarras"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoBarrasActualizadas(List<PfrEntidadDTO> listadoBarras)
        {
            StringBuilder str = new StringBuilder();
            foreach (var barra in listadoBarras)
            {
                str.Append("<option value = " + barra.Pfrentcodi + ">" + barra.Pfrentid + "|" + barra.Pfrentnomb + "</option>");
            }
            return str.ToString();
        }

        /// <summary>
        /// Genera reporte Html relación de Gams y equipos
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fechaFin"></param>
        /// <param name="gamsCentrales"></param>
        /// <returns></returns>
        public string GenerarRHtmlRelaciónGeneradoresGamsVtp(string url, int pfrpercodi, int estado)
        {
            PfrPeriodoDTO pfrPeriodo = GetByIdPfrPeriodo(pfrpercodi);

            var listaRelacion = ObtenerListadoEntidadConPropiedad((int)ConstantesPotenciaFirmeRemunerable.Tipo.GamsVtp, pfrPeriodo.FechaIni, pfrPeriodo.FechaFin, estado);

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='lstGamsEquipo' style='width:100%' >");

            str.Append("<thead>");
            #region cabecera

            str.Append("<tr>");
            str.Append("<th rowspan='2'>OPCIÓN</th>");
            str.Append("<th colspan='2'>BARRA GAMS</th>");
            str.Append("<th colspan='2'>BARRA VTP</th>");
            str.Append("<th rowspan='2' >Ini. Vigencia</th>");
            str.Append("<th rowspan='2' >Fin Vigencia</th>");
            str.Append("<th rowspan='2' >ESTADO</th>");
            str.Append("<th rowspan='2' >USU.<br>MODIFICACIÓN</th>");
            str.Append("<th rowspan='2' >ULT.<br>MODIFICACIÓN</th>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<th style=''>Código</th>");
            str.Append("<th>Nombre</th>");

            str.Append("<th style=''>Código</th>");
            str.Append("<th>Nombre</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");


            str.Append("<tbody>");

            foreach (var item in listaRelacion)
            {
                string claseFila = item.Pfrentestado == 0 ? "clase_eliminado" : "";
                str.AppendFormat("<tr class='{0}'>", claseFila);
                str.AppendFormat("<td class='btnFilaAcciones' ><a href='JavaScript:VerEquipo({0});' title='Propiedades Gams - Vtp'><img src='{1}/btn-properties.png' alt='Propiedades Gams - Vtp' /></a>", item.Pfrentcodi, $"{url}Content/Images");
                str.AppendFormat("<td>{0}</td>", item.Idbarra1);
                str.AppendFormat("<td>{0}</td>", item.Idbarra1desc);
                str.AppendFormat("<td>{0}</td>", item.Barrcodi);
                str.AppendFormat("<td>{0}</td>", item.Barrnombre);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaIniDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaFinDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Pfrentestadodesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Usuarioultimamodif);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Fechaultimamodif);
                str.Append("</tr>");

            }
            str.Append("</tbody>");

            return str.ToString();
        }

        /// <summary>
        /// Genera reporte Html relación de Gams y equipos
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fechaFin"></param>
        /// <param name="gamsCentrales"></param>
        /// <returns></returns>
        public string GenerarRHtmlRelaciónGeneradoresGamsSsaa(string url, int pfrpercodi, int estado)
        {
            PfrPeriodoDTO pfrPeriodo = GetByIdPfrPeriodo(pfrpercodi);
            int tipoEntidad = (int)ConstantesPotenciaFirmeRemunerable.Tipo.GamsSsaa;
            var listaRelacion = ObtenerListadoEntidadConPropiedad(tipoEntidad, pfrPeriodo.FechaIni, pfrPeriodo.FechaFin, estado);

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='lstGamsEquipo' >");

            str.Append("<thead>");
            #region cabecera

            str.Append("<tr>");
            str.Append("<th rowspan='2' style='width: 5%;'>Opciones</th>");
            str.Append("<th colspan='2'>BARRA GAMS</th>");
            str.Append("<th colspan='2'>SSAA</th>");
            str.Append("<th rowspan='2' >Ini. Vigencia</th>");
            str.Append("<th rowspan='2' >Fin Vigencia</th>");
            str.Append("<th rowspan='2' style='width: 5%;'>ESTADO</th>");
            str.Append("<th rowspan='2' style='width: 5%;'>USU.<br>MODIFICACIÓN</th>");
            str.Append("<th rowspan='2' style='width: 5%;'>ULT.<br>MODIFICACIÓN</th>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<th style='width: 5%;'>Código</th>");
            str.Append("<th>Nombre</th>");

            str.Append("<th style='width: 5%;'>Código</th>");
            str.Append("<th>Nombre</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");


            str.Append("<tbody>");

            foreach (var item in listaRelacion)
            {
                string claseFila = item.Pfrentestado == 0 ? "clase_eliminado" : "";
                str.AppendFormat("<tr class='{0}'>", claseFila);
                str.AppendFormat("<td class='btnFilaAcciones' ><a href='JavaScript:VerEquipo({0}, {2});' title='Propiedades Gams-Ssaa'><img src='{1}/btn-properties.png' alt='Propiedades Gams-Ssaa' /></a>", item.Pfrentcodi, $"{url}Content/Images", tipoEntidad);
                str.AppendFormat("<td>{0}</td>", item.Idbarra1);
                str.AppendFormat("<td>{0}</td>", item.Idbarra1desc);
                str.AppendFormat("<td>{0}</td>", item.Equicodi);
                str.AppendFormat("<td>{0}</td>", item.Equinomb);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaIniDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.VigenciaFinDesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Pfrentestadodesc);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Usuarioultimamodif);
                str.AppendFormat("<td style='text-align: center;'>{0}</td>", item.Fechaultimamodif);
                str.Append("</tr>");

            }
            str.Append("</tbody>");

            return str.ToString();
        }

        /// <summary>
        /// Genera reporte Html relación de Gams y equipos
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fechaFin"></param>
        /// <param name="gamsCentrales"></param>
        /// <returns></returns>
        public string GenerarRHtmlRelaciónGeneradoresGamsEquipos(string url, int pfrpercodi, int estado)
        {
            PfrPeriodoDTO pfrPeriodo = GetByIdPfrPeriodo(pfrpercodi);
            int tipoEntidad = (int)ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos;

            var listaRelacion = ObtenerListadoEntidadConPropiedad((int)ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos, pfrPeriodo.FechaIni, pfrPeriodo.FechaFin, estado).OrderBy(x => x.Pfrentid).ThenBy(x => x.Pfrentunidadnomb);


            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='lstGamsEquipo' style='width: 100%'>");

            str.Append("<thead>");
            #region cabecera

            str.Append("<tr>");
            str.Append("<th rowspan='2'>Opciones</th>");
            str.Append("<th rowspan='2'>ID</th>");
            str.Append("<th colspan='3'>BARRA GAMS</th>");
            str.Append("<th colspan='6'>EQUIPO</th>");
            str.Append("<th rowspan='2' >Ini. Vigencia</th>");
            str.Append("<th rowspan='2' >Fin Vigencia</th>");
            str.Append("<th rowspan='2'>ESTADO</th>");
            str.Append("<th rowspan='2'>USU.<br>MODIFICACIÓN</th>");
            str.Append("<th rowspan='2'>ULT.<br>MODIFICACIÓN</th>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<th>Código</th>");
            str.Append("<th>Num. Unidad</th>");
            str.Append("<th>Nombre</th>");

            str.Append("<th>Código</th>");
            str.Append("<th>Nombre</th>");
            str.Append("<th>Unidad</th>");

            str.Append("<th>QMáx (MVar)</th>");
            str.Append("<th>QMín (MVar)</th>");
            str.Append("<th>Ref</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");


            str.Append("<tbody>");

            foreach (var item in listaRelacion)
            {
                var estilo = "";
                if (item.Pfrentficticio == (int)ConstantesPotenciaFirmeRemunerable.Estado.Activo)
                    estilo = "background-color:#fcff22;";

                string claseFila = item.Pfrentestado == 0 ? "clase_eliminado" : "";
                str.AppendFormat("<tr class='{0}'>", claseFila);
                str.AppendFormat("<td class='btnFilaAcciones' ><a href='JavaScript:VerEquipo({0}, {2});' title='Propiedad Gams-Equipo'><img src='{1}/btn-properties.png' alt='Propiedad Gams-Equipo' /></a>", item.Pfrentcodi, $"{url}Content/Images", tipoEntidad);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Pfrentid);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Idbarra1);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Numunidad);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Idbarra1desc);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Equicodi);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Equinomb);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Pfrentunidadnomb);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Qmax);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Qmin);

                var referencia = item.Ref == (int)ConstantesPotenciaFirmeRemunerable.Estado.Activo
                                            ? $"<img style='width: 18px; height: 18px;' src='{url}Content/Images/btn-ok.png' alt='Dar de baja' />"
                                            : $"<img style='width: 18px; height: 18px;' src='{url}Content/Images/btn-desmarcado.png' alt='Activar Registro' />";

                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", referencia);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.VigenciaIniDesc);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.VigenciaFinDesc);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Pfrentestadodesc);
                str.AppendFormat("<td style='" + estilo + "'>{0}</td>", item.Usuarioultimamodif);
                str.AppendFormat("<td style='" + estilo + "'>{0:dd/MM/yyy HH:mm}</td>", item.Fechaultimamodif);
                str.Append("</tr>");

            }

            str.Append("</tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// congestiones
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pfrPeriodo"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public string GenerarRHtmlRelacionCongestion(string url, PfrPeriodoDTO pfrPeriodo, int estado)
        {
            int tipoEntidad = (int)ConstantesPotenciaFirmeRemunerable.Tipo.Congestion;
            var listaRelacion = ObtenerListadoEntidadConPropiedad((int)ConstantesPotenciaFirmeRemunerable.Tipo.Congestion, pfrPeriodo.FechaIni, pfrPeriodo.FechaFin, estado);

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_lstCongestiones' style='width:100%'>");

            str.Append("<thead>");
            #region cabecera            

            str.Append("<tr>");
            str.Append("<th style='width: 120px;'>Opciones</th>");
            str.Append("<th style='width: 80px;'>Id</th>");
            str.Append("<th style='width: 240px;'>Enlace</th>");
            str.Append("<th style='width: 100px;'>PMáx</th>");
            str.Append("<th style='width: 100px;'>PMín</th>");
            str.Append("<th style='width: 340px;'>Líneas</th>");
            str.Append("<th style='width: 120px;'>Ini. Vigencia</th>");
            str.Append("<th style='width: 120px;'>Fin Vigencia</th>");
            str.Append("<th style='width: 120px;'>Estado</th>");
            str.Append("<th style='width: 180px;'>Usuario Modificación</th>");
            str.Append("<th style='width: 180px;'>Últ. Modificación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");

            foreach (var item in listaRelacion)
            {
                string claseFila = item.Pfrentestado == 0 ? "clase_eliminado" : "";
                str.AppendFormat("<tr class='{0}'>", claseFila);
                str.AppendFormat("<td class='btnFilaAcciones' ><a href='JavaScript:VerEquipo({0}, {2});' title='Propiedad Congestión'><img src='{1}/btn-properties.png' alt='Propiedad Congestión' /></a>", item.Pfrentcodi, $"{url}Content/Images", tipoEntidad);
                str.AppendFormat("<td style='text-align: center; width: 80px;'>{0}</td>", item.Pfrentid);
                str.AppendFormat("<td style='width: 240px;'>{0}</td>", item.Pfrentnomb);
                str.AppendFormat("<td style='text-align: center; width: 100px;'>{0}</td>", item.Pmax);
                str.AppendFormat("<td style='text-align: center; width: 100px;'>{0}</td>", item.Pmin);
                str.AppendFormat("<td style='text-align: center; width: 340px;'>{0}</td>", item.Lineasdesc);
                str.AppendFormat("<td style='text-align: center; width: 120px;'>{0}</td>", item.VigenciaIniDesc);
                str.AppendFormat("<td style='text-align: center; width: 120px;'>{0}</td>", item.VigenciaFinDesc);
                str.AppendFormat("<td style='text-align: center; width: 120px;'>{0}</td>", item.Pfrentestadodesc);
                str.AppendFormat("<td style='text-align: center; width: 180px;'>{0}</td>", item.Usuarioultimamodif);
                str.AppendFormat("<td style='text-align: center; width: 180px;'>{0}</td>", item.Fechaultimamodif);

                str.Append("</tr>");

            }
            str.Append("</tbody>");

            return str.ToString();
        }

        /// <summary>
        /// congestiones
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pfrPeriodo"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public string GenerarRHtmlRelacionPenalidad(string url, PfrPeriodoDTO pfrPeriodo, int estado)
        {
            int tipoEntidad = (int)ConstantesPotenciaFirmeRemunerable.Tipo.Penalidad;
            var listaRelacion = ObtenerListadoEntidadConPropiedad(tipoEntidad, pfrPeriodo.FechaIni, pfrPeriodo.FechaFin, estado);

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' id='tabla_lstPenalidades' style='width:100%'>");

            str.Append("<thead>");
            #region cabecera            

            str.Append("<tr>");
            str.Append("<th style='width: 120px;'>Opciones</th>");
            str.Append("<th style='width: 80px;'>Id</th>");
            str.Append("<th style='width: 240px;'>Valor Penalidad</th>");
            str.Append("<th style='width: 100px;'>Descripción</th>");
            str.Append("<th style='width: 120px;'>Ini. Vigencia</th>");
            str.Append("<th style='width: 120px;'>Fin Vigencia</th>");
            str.Append("<th style='width: 120px;'>Estado</th>");
            str.Append("<th style='width: 180px;'>Usuario Modificación</th>");
            str.Append("<th style='width: 180px;'>Últ. Modificación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");

            foreach (var item in listaRelacion)
            {
                string claseFila = item.Pfrentestado == 0 ? "clase_eliminado" : "";
                str.AppendFormat("<tr class='{0}'>", claseFila);
                str.AppendFormat("<td class='btnFilaAcciones' ><a href='JavaScript:VerEquipo({0}, {2});' title='Propiedad'><img src='{1}/btn-properties.png' alt='Propiedad' /></a>", item.Pfrentcodi, $"{url}Content/Images", tipoEntidad);
                str.AppendFormat("<td style='text-align: center; width: 80px;'>{0}</td>", item.Pfrentid);
                str.AppendFormat("<td style='width: 240px;'>{0}</td>", item.Penalidad);
                str.AppendFormat("<td style='text-align: center; width: 100px;'>{0}</td>", item.Descripcion);
                str.AppendFormat("<td style='text-align: center; width: 120px;'>{0}</td>", item.VigenciaIniDesc);
                str.AppendFormat("<td style='text-align: center; width: 120px;'>{0}</td>", item.VigenciaFinDesc);
                str.AppendFormat("<td style='text-align: center; width: 120px;'>{0}</td>", item.Pfrentestadodesc);
                str.AppendFormat("<td style='text-align: center; width: 180px;'>{0}</td>", item.Usuarioultimamodif);
                str.AppendFormat("<td style='text-align: center; width: 180px;'>{0}</td>", item.Fechaultimamodif);

                str.Append("</tr>");

            }
            str.Append("</tbody>");

            return str.ToString();
        }

        /// <summary>
        /// Devuelve el siguiente codigo disponible para los equipos (usado al momento de crean nuevo equipo)
        /// </summary>
        /// <param name="ultimoIdEquipo"></param>
        /// <param name="familia"></param>
        /// <returns></returns>
        public string ObtenerSiguienteIdDisponibleEquipo(int familia)
        {
            string id = "";
            switch ((ConstantesPotenciaFirmeRemunerable.Tipo)familia)
            {
                case ConstantesPotenciaFirmeRemunerable.Tipo.Barra:
                    id = ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.Barra);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Linea:
                    id = ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.Linea);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2:
                    id = ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3:
                    id = ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3);
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica:
                    id = ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica);
                    break;
            }
            return id;
        }

        /// <summary>
        /// Devuelve el codigo disponible para cierto equipo
        /// </summary>
        /// <param name="pfrcatcodi"></param>
        /// <returns></returns>
        public string ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo pfrcatcodi)
        {
            int intCodigoDisponible = -1;
            string strCodigoDisponible = "-1";

            switch (pfrcatcodi)
            {
                case ConstantesPotenciaFirmeRemunerable.Tipo.Barra:
                    strCodigoDisponible = "B0001";
                    break;

                case ConstantesPotenciaFirmeRemunerable.Tipo.Linea:
                    strCodigoDisponible = "L0001";
                    break;

                case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2:
                    strCodigoDisponible = "T2001";
                    break;

                case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3:
                    strCodigoDisponible = "T3001";
                    break;
                case ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica:
                    strCodigoDisponible = "S001";
                    break;

                case ConstantesPotenciaFirmeRemunerable.Tipo.Congestion:
                    strCodigoDisponible = "C001";
                    break;

                case ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos:
                    strCodigoDisponible = "T001";
                    break;
            }

            var lstEntidad = GetByCriteriaPfrEntidads((int)pfrcatcodi).OrderByDescending(x => x.Pfrentid).ToList();

            if (lstEntidad.Count > 0)
            {
                int digito;
                string prefijo;
                ObtenerPrefYDigitos(out intCodigoDisponible, lstEntidad, out digito, out prefijo);

                switch (pfrcatcodi)
                {
                    case ConstantesPotenciaFirmeRemunerable.Tipo.Barra:
                        switch (digito)
                        {
                            case 1: prefijo = "B000"; break;
                            case 2: prefijo = "B00"; break;
                            case 3: prefijo = "B0"; break;
                            case 4: prefijo = "B"; break;

                        }
                        strCodigoDisponible = prefijo + intCodigoDisponible.ToString();

                        break;

                    case ConstantesPotenciaFirmeRemunerable.Tipo.Linea:
                        switch (digito)
                        {
                            case 1: prefijo = "L000"; break;
                            case 2: prefijo = "L00"; break;
                            case 3: prefijo = "L0"; break;
                            case 4: prefijo = "L"; break;

                        }
                        strCodigoDisponible = prefijo + intCodigoDisponible.ToString();
                        break;

                    case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2:
                        ObtenerPrefYDigitos(out intCodigoDisponible, lstEntidad, out digito, out prefijo, 2);
                        switch (digito)
                        {
                            case 1: prefijo = "T200"; break;
                            case 2: prefijo = "T20"; break;
                            case 3: prefijo = "T2"; break;
                            case 4: prefijo = "T"; break;
                        }
                        strCodigoDisponible = prefijo + intCodigoDisponible.ToString();
                        break;

                    case ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3:
                        ObtenerPrefYDigitos(out intCodigoDisponible, lstEntidad, out digito, out prefijo, 2);
                        switch (digito)
                        {
                            case 1: prefijo = "T300"; break;
                            case 2: prefijo = "T30"; break;
                            case 3: prefijo = "T3"; break;
                            case 4: prefijo = "T"; break;
                        }
                        strCodigoDisponible = prefijo + intCodigoDisponible.ToString();
                        break;

                    case ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica:
                        switch (digito)
                        {
                            case 1: prefijo = "S00"; break;
                            case 2: prefijo = "S0"; break;
                            case 3: prefijo = "S"; break;

                        }
                        strCodigoDisponible = prefijo + intCodigoDisponible.ToString();
                        break;
                    case ConstantesPotenciaFirmeRemunerable.Tipo.Congestion:
                        switch (digito)
                        {
                            case 1: prefijo = "C00"; break;
                            case 2: prefijo = "C0"; break;
                            case 3: prefijo = "C"; break;

                        }
                        strCodigoDisponible = prefijo + intCodigoDisponible.ToString();
                        break;
                    case ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos:
                        switch (digito)
                        {
                            case 1: prefijo = "T00"; break;
                            case 2: prefijo = "T0"; break;
                            case 3: prefijo = "T"; break;

                        }
                        strCodigoDisponible = prefijo + intCodigoDisponible.ToString();
                        break;
                }
            }

            return strCodigoDisponible;
        }

        private void ObtenerPrefYDigitos(out int intCodigoDisponible, List<PfrEntidadDTO> lstEntidad, out int digitosB, out string prefijoB, int starindex = 1)
        {
            var strNumB = (lstEntidad.First().Pfrentid).Substring(1);
            intCodigoDisponible = int.Parse(strNumB) + 1;
            digitosB = (int)Math.Floor(Math.Log10(intCodigoDisponible) + 1);
            prefijoB = "";
        }

        /// <summary>
        /// Valida las congestiones
        /// </summary>
        /// <param name="pfrCongestionForm"></param>
        /// <param name="usuario"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        public int ValidarBDCongestion(PfrEntidadDTO pfrCongestionForm, int accion)
        {
            try
            {
                string idCongestion = pfrCongestionForm.Pfrentid;
                string enlaceCongestion = pfrCongestionForm.Pfrentnomb;

                //Solo si el EQUIPO es NUEVO
                if (accion == (int)ConstantesPotenciaFirmeRemunerable.Accion.Nuevo)
                {
                    var listadoCongestion = GetByCriteriaPfrEntidads(pfrCongestionForm.Pfrcatcodi);

                    //validacion ID repetido (en congestiones activos)
                    var lstMismoId = listadoCongestion.Where(x => x.Pfrentid.ToUpper().Trim() == idCongestion.ToUpper().Trim());
                    if (lstMismoId.Any()) throw new ArgumentException("¡Ya existe una Congestión Activa con el mismo Id en la Base de Datos!");

                    //validacion Nombre repetido (en congestiones activos)
                    var lstActCongestion = listadoCongestion.Where(x => x.Pfrentnomb.ToUpper().Trim() == enlaceCongestion.ToUpper().Trim());
                    if (lstActCongestion.Any()) throw new ArgumentException("¡Ya existe una congestión con el mismo Enlace en la Base de Datos!");
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public void ActualizarFechaModifEntidad(int pfrentcodi, string usuario, DateTime fechaModif)
        {
            var reg = GetByIdPfrEntidad(pfrentcodi);
            reg.Pfrentusumodificacion = usuario;
            reg.Pfrentfecmodificacion = fechaModif;

            UpdatePfrEntidad(reg);
        }

        #endregion
    }
}
