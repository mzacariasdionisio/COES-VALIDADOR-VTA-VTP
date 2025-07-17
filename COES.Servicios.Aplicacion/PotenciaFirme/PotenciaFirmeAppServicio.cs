using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Titularidad;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace COES.Servicios.Aplicacion.PotenciaFirme
{
    /// <summary>
    /// Clases con métodos del módulo Potencia Firme
    /// </summary>
    public class PotenciaFirmeAppServicio : AppServicioBase
    {
        readonly INDAppServicio servIndisp = new INDAppServicio();
        private readonly ReporteMedidoresAppServicio _medidoresAppServicio;
        private readonly EquipamientoAppServicio _equipamientoAppServicio;

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PotenciaFirmeAppServicio));

        public PotenciaFirmeAppServicio()
        {
            _medidoresAppServicio = new ReporteMedidoresAppServicio();
            _equipamientoAppServicio = new EquipamientoAppServicio();
        }

        #region CRUD Tablas BD COES

        #region Métodos Tabla PF_CONTRATOS

        /// <summary>
        /// Inserta un registro de la tabla PF_CONTRATOS
        /// </summary>
        public void SavePfContratos(PfContratosDTO entity)
        {
            try
            {
                FactorySic.GetPfContratosRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_CONTRATOS
        /// </summary>
        public void UpdatePfContratos(PfContratosDTO entity)
        {
            try
            {
                FactorySic.GetPfContratosRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_CONTRATOS
        /// </summary>
        public void DeletePfContratos(int pfcontcodi)
        {
            try
            {
                FactorySic.GetPfContratosRepository().Delete(pfcontcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_CONTRATOS
        /// </summary>
        public PfContratosDTO GetByIdPfContratos(int pfcontcodi)
        {
            return FactorySic.GetPfContratosRepository().GetById(pfcontcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_CONTRATOS
        /// </summary>
        public List<PfContratosDTO> ListPfContratoss()
        {
            return FactorySic.GetPfContratosRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfContratos
        /// </summary>
        public List<PfContratosDTO> GetByCriteriaPfContratoss()
        {
            return FactorySic.GetPfContratosRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PF_CUADRO

        /// <summary>
        /// Inserta un registro de la tabla PF_CUADRO
        /// </summary>
        public void SavePfCuadro(PfCuadroDTO entity)
        {
            try
            {
                FactorySic.GetPfCuadroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }


        /// <summary>
        /// Actualiza un registro de la tabla PF_CUADRO
        /// </summary>
        public void UpdatePfCuadro(PfCuadroDTO entity)
        {
            try
            {
                FactorySic.GetPfCuadroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_CUADRO
        /// </summary>
        public void DeletePfCuadro(int pfcuacodi)
        {
            try
            {
                FactorySic.GetPfCuadroRepository().Delete(pfcuacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_CUADRO
        /// </summary>
        public PfCuadroDTO GetByIdPfCuadro(int pfcuacodi)
        {
            return FactorySic.GetPfCuadroRepository().GetById(pfcuacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_CUADRO
        /// </summary>
        public List<PfCuadroDTO> ListPfCuadros()
        {
            return FactorySic.GetPfCuadroRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfCuadro
        /// </summary>
        public List<PfCuadroDTO> GetByCriteriaPfCuadros()
        {
            return FactorySic.GetPfCuadroRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PF_ESCENARIO

        /// <summary>
        /// Inserta un registro de la tabla PF_ESCENARIO
        /// </summary>
        public void SavePfEscenario(PfEscenarioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfEscenarioRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_ESCENARIO
        /// </summary>
        public void UpdatePfEscenario(PfEscenarioDTO entity)
        {
            try
            {
                FactorySic.GetPfEscenarioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_ESCENARIO
        /// </summary>
        public void DeletePfEscenario(int pfescecodi)
        {
            try
            {
                FactorySic.GetPfEscenarioRepository().Delete(pfescecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_ESCENARIO
        /// </summary>
        public PfEscenarioDTO GetByIdPfEscenario(int pfescecodi)
        {
            return FactorySic.GetPfEscenarioRepository().GetById(pfescecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_ESCENARIO
        /// </summary>
        public List<PfEscenarioDTO> ListPfEscenarios()
        {
            return FactorySic.GetPfEscenarioRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfEscenario
        /// </summary>
        public List<PfEscenarioDTO> GetByCriteriaPfEscenarios(int pfrptcodi)
        {
            var lista = FactorySic.GetPfEscenarioRepository().GetByCriteria(pfrptcodi);

            foreach (var reg in lista)
            {
                reg.FechaDesc = GetDescripcionEscenario(reg.Pfescefecini, reg.Pfescefecfin);
            }

            return lista;
        }

        public static string GetDescripcionEscenario(DateTime fechaIni, DateTime fechaFin)
        {
            var fechaDesc = (fechaIni.Day != fechaFin.Day) ? fechaIni.Day + "-" + fechaFin.Day : fechaIni.Day + "";
            fechaDesc += " de " + EPDate.f_NombreMes(fechaIni.Month).ToUpper();

            return fechaDesc;
        }

        #endregion

        #region Métodos Tabla PF_PERIODO

        /// <summary>
        /// Inserta un registro de la tabla PF_PERIODO
        /// </summary>
        public int SavePfPeriodo(PfPeriodoDTO entity)
        {
            try
            {
                return FactorySic.GetPfPeriodoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_PERIODO
        /// </summary>
        public void UpdatePfPeriodo(PfPeriodoDTO entity)
        {
            try
            {
                FactorySic.GetPfPeriodoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_PERIODO
        /// </summary>
        public void DeletePfPeriodo(int pfpericodi)
        {
            try
            {
                FactorySic.GetPfPeriodoRepository().Delete(pfpericodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_PERIODO
        /// </summary>
        public PfPeriodoDTO GetByIdPfPeriodo(int pfpericodi)
        {
            var reg = FactorySic.GetPfPeriodoRepository().GetById(pfpericodi);

            reg.FechaIni = new DateTime(reg.Pfperianio, reg.Pfperimes, 1);
            reg.FechaFin = reg.FechaIni.AddMonths(1).AddDays(-1);

            return reg;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_PERIODO
        /// </summary>
        public List<PfPeriodoDTO> ListPfPeriodos()
        {
            return FactorySic.GetPfPeriodoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfPeriodo
        /// </summary>
        public List<PfPeriodoDTO> GetByCriteriaPfPeriodos(int anio)
        {
            return FactorySic.GetPfPeriodoRepository().GetByCriteria(anio).OrderByDescending(x => x.Pfperianiomes).ToList();
        }

        #endregion

        #region Métodos Tabla PF_POTENCIA_ADICIONAL

        /// <summary>
        /// Inserta un registro de la tabla PF_POTENCIA_ADICIONAL
        /// </summary>
        public void SavePfPotenciaAdicional(PfPotenciaAdicionalDTO entity)
        {
            try
            {
                FactorySic.GetPfPotenciaAdicionalRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_POTENCIA_ADICIONAL
        /// </summary>
        public void UpdatePfPotenciaAdicional(PfPotenciaAdicionalDTO entity)
        {
            try
            {
                FactorySic.GetPfPotenciaAdicionalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_POTENCIA_ADICIONAL
        /// </summary>
        public void DeletePfPotenciaAdicional(int pfpadicodi)
        {
            try
            {
                FactorySic.GetPfPotenciaAdicionalRepository().Delete(pfpadicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_POTENCIA_ADICIONAL
        /// </summary>
        public PfPotenciaAdicionalDTO GetByIdPfPotenciaAdicional(int pfpadicodi)
        {
            return FactorySic.GetPfPotenciaAdicionalRepository().GetById(pfpadicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_POTENCIA_ADICIONAL
        /// </summary>
        public List<PfPotenciaAdicionalDTO> ListPfPotenciaAdicionals()
        {
            return FactorySic.GetPfPotenciaAdicionalRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfPotenciaAdicional
        /// </summary>
        public List<PfPotenciaAdicionalDTO> GetByCriteriaPfPotenciaAdicionals()
        {
            return FactorySic.GetPfPotenciaAdicionalRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PF_POTENCIA_GARANTIZADA

        /// <summary>
        /// Inserta un registro de la tabla PF_POTENCIA_GARANTIZADA
        /// </summary>
        public void SavePfPotenciaGarantizada(PfPotenciaGarantizadaDTO entity)
        {
            try
            {
                FactorySic.GetPfPotenciaGarantizadaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_POTENCIA_GARANTIZADA
        /// </summary>
        public void UpdatePfPotenciaGarantizada(PfPotenciaGarantizadaDTO entity)
        {
            try
            {
                FactorySic.GetPfPotenciaGarantizadaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }



        /// <summary>
        /// Elimina un registro de la tabla PF_POTENCIA_GARANTIZADA
        /// </summary>
        public void DeletePfPotenciaGarantizada(int pfpgarcodi)
        {
            try
            {
                FactorySic.GetPfPotenciaGarantizadaRepository().Delete(pfpgarcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_POTENCIA_GARANTIZADA
        /// </summary>
        public PfPotenciaGarantizadaDTO GetByIdPfPotenciaGarantizada(int pfpgarcodi)
        {
            return FactorySic.GetPfPotenciaGarantizadaRepository().GetById(pfpgarcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_POTENCIA_GARANTIZADA
        /// </summary>
        public List<PfPotenciaGarantizadaDTO> ListPfPotenciaGarantizadas()
        {
            return FactorySic.GetPfPotenciaGarantizadaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfPotenciaGarantizada
        /// </summary>
        public List<PfPotenciaGarantizadaDTO> GetByCriteriaPfPotenciaGarantizadas()
        {
            return FactorySic.GetPfPotenciaGarantizadaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PF_RECALCULO

        /// <summary>
        /// Inserta un registro de la tabla PF_RECALCULO
        /// </summary>
        public int SavePfRecalculo(PfRecalculoDTO entity)
        {
            try
            {
                return FactorySic.GetPfRecalculoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_RECALCULO
        /// </summary>
        public void UpdatePfRecalculo(PfRecalculoDTO entity)
        {
            try
            {
                FactorySic.GetPfRecalculoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_RECALCULO
        /// </summary>
        public void DeletePfRecalculo(int pfrecacodi)
        {
            try
            {
                FactorySic.GetPfRecalculoRepository().Delete(pfrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_RECALCULO
        /// </summary>
        public PfRecalculoDTO GetByIdPfRecalculo(int pfrecacodi)
        {
            var reg = FactorySic.GetPfRecalculoRepository().GetById(pfrecacodi);
            FormatearPfRecalculo(reg);
            return reg;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_RECALCULO
        /// </summary>
        public List<PfRecalculoDTO> ListPfRecalculos()
        {
            var lista = FactorySic.GetPfRecalculoRepository().List();

            foreach (var reg in lista)
                FormatearPfRecalculo(reg);

            return lista;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfRecalculo
        /// </summary>
        public List<PfRecalculoDTO> GetByCriteriaPfRecalculos(int pfPericodi)
        {
            var lista = FactorySic.GetPfRecalculoRepository().GetByCriteria(pfPericodi, -1, -1).OrderByDescending(x => x.Pfrecacodi).ToList();

            foreach (var reg in lista)
                FormatearPfRecalculo(reg);

            return lista;
        }

        #endregion

        #region Métodos Tabla PF_RECURSO

        /// <summary>
        /// Inserta un registro de la tabla PF_RECURSO
        /// </summary>
        public void SavePfRecurso(PfRecursoDTO entity)
        {
            try
            {
                FactorySic.GetPfRecursoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_RECURSO
        /// </summary>
        public void UpdatePfRecurso(PfRecursoDTO entity)
        {
            try
            {
                FactorySic.GetPfRecursoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_RECURSO
        /// </summary>
        public void DeletePfRecurso(int pfrecucodi)
        {
            try
            {
                FactorySic.GetPfRecursoRepository().Delete(pfrecucodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_RECURSO
        /// </summary>
        public PfRecursoDTO GetByIdPfRecurso(int pfrecucodi)
        {
            return FactorySic.GetPfRecursoRepository().GetById(pfrecucodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_RECURSO
        /// </summary>
        public List<PfRecursoDTO> ListPfRecursos()
        {
            return FactorySic.GetPfRecursoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfRecurso
        /// </summary>
        public List<PfRecursoDTO> GetByCriteriaPfRecursos()
        {
            return FactorySic.GetPfRecursoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PF_REPORTE

        /// <summary>
        /// Inserta un registro de la tabla PF_REPORTE
        /// </summary>
        public void SavePfReporte(PfReporteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfReporteRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_REPORTE
        /// </summary>
        public void UpdatePfReporte(PfReporteDTO entity)
        {
            try
            {
                FactorySic.GetPfReporteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_REPORTE
        /// </summary>
        public void DeletePfReporte(int pfrptcodi)
        {
            try
            {
                FactorySic.GetPfReporteRepository().Delete(pfrptcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_REPORTE
        /// </summary>
        public PfReporteDTO GetByIdPfReporte(int pfrptcodi)
        {
            return FactorySic.GetPfReporteRepository().GetById(pfrptcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_REPORTE
        /// </summary>
        public List<PfReporteDTO> ListPfReportes()
        {
            return FactorySic.GetPfReporteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfReporte
        /// </summary>
        public List<PfReporteDTO> GetByCriteriaPfReportes(int pfrecacodi, int pfcuacodi)
        {
            return FactorySic.GetPfReporteRepository().GetByCriteria(pfrecacodi, pfcuacodi);
        }

        #endregion

        #region Métodos Tabla PF_REPORTE_TOTAL

        /// <summary>
        /// Inserta un registro de la tabla PF_REPORTE_TOTAL
        /// </summary>
        public void SavePfReporteTotal(PfReporteTotalDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfReporteTotalRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_REPORTE_TOTAL
        /// </summary>
        public void UpdatePfReporteTotal(PfReporteTotalDTO entity)
        {
            try
            {
                FactorySic.GetPfReporteTotalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_REPORTE_TOTAL
        /// </summary>
        public void DeletePfReporteTotal(int pftotcodi)
        {
            try
            {
                FactorySic.GetPfReporteTotalRepository().Delete(pftotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_REPORTE_TOTAL
        /// </summary>
        public PfReporteTotalDTO GetByIdPfReporteTotal(int pftotcodi)
        {
            return FactorySic.GetPfReporteTotalRepository().GetById(pftotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_REPORTE_TOTAL
        /// </summary>
        public List<PfReporteTotalDTO> ListPfReporteTotals()
        {
            return FactorySic.GetPfReporteTotalRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfReporteTotal
        /// </summary>
        public List<PfReporteTotalDTO> GetByCriteriaPfReporteTotals(string pfrptcodi)
        {
            if (string.IsNullOrEmpty(pfrptcodi))
                return new List<PfReporteTotalDTO>();

            var lista = FactorySic.GetPfReporteTotalRepository().GetByCriteria(pfrptcodi);

            foreach (var reg in lista)
                FormatearPfReporteTotal(reg);

            return lista;
        }

        private void FormatearPfReporteTotal(PfReporteTotalDTO reg)
        {
            reg.FechaIni = new DateTime(reg.Pfperianio, reg.Pfperimes, 1);
            reg.FechaFin = reg.FechaIni.AddMonths(1).AddDays(-1);
        }

        #endregion

        #region Métodos Tabla PF_REPORTE_DET

        /// <summary>
        /// Actualiza un registro de la tabla PF_REPORTE_DET
        /// </summary>
        public void UpdatePfReporteDet(PfReporteDetDTO entity)
        {
            try
            {
                FactorySic.GetPfReporteDetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_REPORTE_DET
        /// </summary>
        public void DeletePfReporteDet(int pfdetcodi)
        {
            try
            {
                FactorySic.GetPfReporteDetRepository().Delete(pfdetcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_REPORTE_DET
        /// </summary>
        public PfReporteDetDTO GetByIdPfReporteDet(int pfdetcodi)
        {
            return FactorySic.GetPfReporteDetRepository().GetById(pfdetcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_REPORTE_DET
        /// </summary>
        public List<PfReporteDetDTO> ListPfReporteDets()
        {
            return FactorySic.GetPfReporteDetRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfReporteDet
        /// </summary>
        public List<PfReporteDetDTO> GetByCriteriaPfReporteDets(int pfrptcodi)
        {
            return FactorySic.GetPfReporteDetRepository().GetByCriteria(pfrptcodi);
        }

        #endregion

        #region Métodos Tabla PF_VERSION

        /// <summary>
        /// Inserta un registro de la tabla PF_VERSION
        /// </summary>
        public int SavePfVersion(PfVersionDTO entity)
        {
            try
            {
                return FactorySic.GetPfVersionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_VERSION
        /// </summary>
        public void UpdatePfVersion(PfVersionDTO entity)
        {
            try
            {
                FactorySic.GetPfVersionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_VERSION
        /// </summary>
        public void DeletePfVersion(int pfverscodi)
        {
            try
            {
                FactorySic.GetPfVersionRepository().Delete(pfverscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_VERSION
        /// </summary>
        public PfVersionDTO GetByIdPfVersion(int pfverscodi)
        {
            return FactorySic.GetPfVersionRepository().GetById(pfverscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_VERSION
        /// </summary>
        public List<PfVersionDTO> ListPfVersions()
        {
            return FactorySic.GetPfVersionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfVersion
        /// </summary>
        public List<PfVersionDTO> GetByCriteriaPfVersions(int recacodi, int recucodi)
        {
            return FactorySic.GetPfVersionRepository().GetByCriteria(recacodi, recucodi).OrderBy(x => x.Pfverscodi).ToList();
        }

        #endregion

        #region Métodos Tabla PF_RELACION_INSUMOS

        /// <summary>
        /// Inserta un registro de la tabla PF_RELACION_INSUMOS
        /// </summary>
        public void SavePfRelacionInsumos(PfRelacionInsumosDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfRelacionInsumosRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_RELACION_INSUMOS
        /// </summary>
        public void UpdatePfRelacionInsumos(PfRelacionInsumosDTO entity)
        {
            try
            {
                FactorySic.GetPfRelacionInsumosRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_RELACION_INSUMOS
        /// </summary>
        public void DeletePfRelacionInsumos(int pfrinscodi)
        {
            try
            {
                FactorySic.GetPfRelacionInsumosRepository().Delete(pfrinscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_RELACION_INSUMOS
        /// </summary>
        public PfRelacionInsumosDTO GetByIdPfRelacionInsumos(int pfrinscodi)
        {
            return FactorySic.GetPfRelacionInsumosRepository().GetById(pfrinscodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfRelacionInsumos
        /// </summary>
        public List<PfRelacionInsumosDTO> GetByCriteriaPfRelacionInsumoss(int pfrptcodi)
        {
            return FactorySic.GetPfRelacionInsumosRepository().GetByCriteria(pfrptcodi);
        }

        #endregion

        #region Métodos Tabla PF_RELACION_INDISPONIBILIDADES

        /// <summary>
        /// Inserta un registro de la tabla PF_RELACION_INDISPONIBILIDADES
        /// </summary>
        public void SavePfRelacionIndisponibilidades(PfRelacionIndisponibilidadesDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetPfRelacionIndisponibilidadesRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_RELACION_INDISPONIBILIDADES
        /// </summary>
        public void UpdatePfRelacionIndisponibilidades(PfRelacionIndisponibilidadesDTO entity)
        {
            try
            {
                FactorySic.GetPfRelacionIndisponibilidadesRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_RELACION_INDISPONIBILIDADES
        /// </summary>
        public void DeletePfRelacionIndisponibilidades(int pfrindcodi)
        {
            try
            {
                FactorySic.GetPfRelacionIndisponibilidadesRepository().Delete(pfrindcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_RELACION_INDISPONIBILIDADES
        /// </summary>
        public PfRelacionIndisponibilidadesDTO GetByIdPfRelacionIndisponibilidades(int pfrindcodi)
        {
            return FactorySic.GetPfRelacionIndisponibilidadesRepository().GetById(pfrindcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfRelacionIndisponibilidades
        /// </summary>
        public List<PfRelacionIndisponibilidadesDTO> GetByCriteriaPfRelacionIndisponibilidadess(int pfrptcodi)
        {
            return FactorySic.GetPfRelacionIndisponibilidadesRepository().GetByCriteria(pfrptcodi);
        }

        #endregion

        #region Métodos Tabla PF_FACTORES

        /// <summary>
        /// Inserta un registro de la tabla PF_FACTORES
        /// </summary>
        public void SavePfFactores(PfFactoresDTO entity)
        {
            try
            {
                FactorySic.GetPfFactoresRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PF_FACTORES
        /// </summary>
        public void UpdatePfFactores(PfFactoresDTO entity)
        {
            try
            {
                FactorySic.GetPfFactoresRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PF_FACTORES
        /// </summary>
        public void DeletePfFactores(int pffactcodi)
        {
            try
            {
                FactorySic.GetPfFactoresRepository().Delete(pffactcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PF_FACTORES
        /// </summary>
        public PfFactoresDTO GetByIdPfFactores(int pffactcodi)
        {
            return FactorySic.GetPfFactoresRepository().GetById(pffactcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PF_FACTORES
        /// </summary>
        public List<PfFactoresDTO> ListPfFactoress()
        {
            return FactorySic.GetPfFactoresRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PfFactores
        /// </summary>
        public List<PfFactoresDTO> GetByCriteriaPfFactoress()
        {
            return FactorySic.GetPfFactoresRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla IND_RECALCULO

        /// <summary>
        /// Permite listar todos los registros de la tabla IND_RECALCULO
        /// </summary>
        public List<IndRecalculoDTO> ListIndRecalculosByMes(DateTime fechaPeriodo)
        {
            var lista = FactorySic.GetIndRecalculoRepository().ListXMes(fechaPeriodo.Year, fechaPeriodo.Month);
            foreach (var reg in lista)
                INDAppServicio.FormatearIndRecalculo(reg);

            return lista.OrderByDescending(x => x.Orden).ToList();
        }

        #endregion

        #endregion

        #region UTILIDADES

        /// <summary>
        /// Permite obtener las empresa por tipo para cierto mes
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasConCentralHidro(DateTime fechaIni)
        {
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

            servIndisp.ListarEqCentralHidraulicoOpComercial(fechaIni, fechaFin, out List<EqEquipoDTO> listaUnidadData, out List<EqEquipoDTO> listaEquiposHidro, out List<ResultadoValidacionAplicativo> listaMsj);
            List<SiEmpresaDTO> listaEmprsa = listaUnidadData
                .GroupBy(x => new { x.Emprcodi, x.Emprnomb })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi ?? 0, Emprnomb = x.Key.Emprnomb })
                .OrderBy(x => x.Emprnomb)
                .ToList();

            return listaEmprsa;
        }

        /// <summary>
        /// Lista Centrales por Empresa para cierto periodo mensual
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListCentralesXEmpresa(int emprcodi, DateTime fechaIni)
        {
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

            servIndisp.ListarEqCentralHidraulicoOpComercial(fechaIni, fechaFin, out List<EqEquipoDTO> listaUnidadData, out List<EqEquipoDTO> listaEquiposHidro, out List<ResultadoValidacionAplicativo> listaMsj);

            List<EqEquipoDTO> listaCentral = listaUnidadData
                .Where(x => x.Emprcodi == emprcodi)
                .GroupBy(x => new { x.Equipadre, x.Central })
                .Select(x => new EqEquipoDTO() { Equipadre = x.Key.Equipadre, Central = x.Key.Central })
                .ToList();

            return listaCentral;
        }

        /// <summary>
        /// Genera el listado de las versiones de los insumos (potencia garantizada, adicional y contratos de compra y venta)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tienePermisoEditar"></param>
        /// <param name="recursocodi"></param>
        /// <param name="recacodi"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoVersion(string url, bool tienePermisoEditar, int recursocodi, int recacodi)
        {
            List<PfVersionDTO> listaVersiones = GetPfVersionByRevisionRecurso(recacodi, recursocodi);

            string colorFondo = "";
            var colorAprobado = "#C6E0B4";
            var colorGenerado = "##F2F5F7";

            StringBuilder str = new StringBuilder();
            str.Append("<table width='818px' class='pretty tabla-adicional tabla_version_x_recalculo' border='0' cellspacing='0' >");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 80px;'>Acciones</th>");
            str.Append("<th style='width: 80px;'>N° Versión</th>");
            str.Append("<th style='width: 150px;'>Estado</th>");
            str.Append("<th style='width: 200px;'>Usuario creación</th>");
            str.Append("<th style='width: 250px;'>Fecha creación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var reg in listaVersiones.OrderBy(x => x.Pfversestado).ThenByDescending(x => x.Pfversnumero).ToList())
            {
                if (reg.Pfversestado == ConstantesPotenciaFirme.Aprobado)
                    colorFondo = colorAprobado;
                if (reg.Pfversestado == ConstantesPotenciaFirme.Generado)
                    colorFondo = colorGenerado;

                str.AppendFormat("<tr style='background-color: {0};'>", colorFondo);
                str.AppendFormat("<td style='width: 80px; background-color: {0};'>", colorFondo);
                str.AppendFormat("<a class='' href='JavaScript:verPorVersion(" + reg.Pfverscodi + ");' style='margin-right: 4px;'><img style='margin-left: 20px; margin-top: 4px; margin-bottom: 4px; cursor:pointer;' src='" + url + "Content/Images/btn-open.png' alt='Ver versión' title='Ver versión' /></a>");
                if (tienePermisoEditar && ConstantesPotenciaFirme.Aprobado == reg.Pfversestado) str.AppendFormat("<a class='' href='JavaScript:aprobarVersion(" + reg.Pfverscodi + "," + recursocodi + "," + recacodi + ");' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px; cursor:pointer;' src='" + url + "Content/Images/btn-ok.png' alt='Validar reporte' title='Aprobar Versión' /></a>");
                if (tienePermisoEditar && ConstantesPotenciaFirme.Generado == reg.Pfversestado) str.AppendFormat("<a class='' href='JavaScript:aprobarVersion(" + reg.Pfverscodi + "," + recursocodi + "," + recacodi + ");' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px; cursor:pointer;' src='" + url + "Content/Images/btn-desmarcado.png' alt='Validar reporte' title='Aprobar Versión' /></a>");
                str.Append("</td>");
                str.AppendFormat("<td class='' style='width: 80px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfversnumero);
                str.AppendFormat("<td class='' style='width: 150px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfversestado == "A" ? "Aprobado" : "Generado");
                str.AppendFormat("<td class='' style='width: 200px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfversusucreacion);
                str.AppendFormat("<td class='' style='width: 250px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfversfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull));

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();

        }

        /// <summary>
        /// Genera el listado de las versiones de las potencias firmes
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tienePermisoEditar"></param>
        /// <param name="recursocodi"></param>
        /// <param name="recacodi"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoVersionesPF(string url, bool tienePermisoEditar, int recacodi)
        {
            List<PfReporteDTO> listaVersiones = GetByCriteriaPfReportes(recacodi, ConstantesPotenciaFirme.CuadroPFirme);

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

            foreach (var reg in listaVersiones.OrderByDescending(x => x.Pfrptcodi).ToList())
            {
                if (reg.Pfrptesfinal == 1)  //Es Final
                    colorFondo = colorEsFinal;
                if (reg.Pfrptesfinal == 0)
                    colorFondo = colorNoFinal;

                str.AppendFormat("<tr style='background-color: {0};'>", colorFondo);
                //Acciones
                str.AppendFormat("<td style='width: 80px; background-color: {0};'>", colorFondo);
                str.AppendFormat("<a class='' href='JavaScript:verPFPorVersion(" + reg.Pfrptcodi + ");' style='margin-right: 4px;'><img style='margin-left: 30px; margin-top: 4px; margin-bottom: 4px; cursor:pointer;' src='" + url + "Content/Images/btn-open.png' alt='Ver versión' title='Ver versión' /></a>");
                str.Append("</td>");

                //Codigo Reporte
                str.AppendFormat("<td class='' style='width: 80px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfrptcodi);

                //Estado
                str.AppendFormat("<td class='' style='width: 150px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfrptesfinal == 0 ? "No Final" : "Es Final");

                //Usuario creacion
                str.AppendFormat("<td class='' style='width: 200px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfrptusucreacion);

                //Fecha Creacion
                str.AppendFormat("<td class='' style='width: 250px; text-align: center; background-color: {0};'>{1}</td>", colorFondo, reg.Pfrptfeccreacion);

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();

        }

        /// <summary>
        /// Lista empresas para casos especiales de nodo energético o reeserva fría para cierto periodo mensual
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListEmpresasConCentralNodoEnergORsrvFria(DateTime fechaIni)
        {
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

            var listaUnidadData = servIndisp.ListaUnidadNodoEnergORsrvFria(fechaIni, fechaFin);
            List<SiEmpresaDTO> listaEmprsa = listaUnidadData
                .GroupBy(x => new { x.Emprcodi, x.Emprnomb })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi ?? 0, Emprnomb = x.Key.Emprnomb })
                .OrderBy(x => x.Emprnomb)
                .ToList();

            return listaEmprsa;
        }

        /// <summary>
        /// Lista centrales por empresa para casos especiales de nodo energético o reeserva fría para cierto periodo mensual
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaIni"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListCentralesXEmpresaNodoEnergORsrvFria(int emprcodi, DateTime fechaIni)
        {
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

            var listaUnidadData = servIndisp.ListaUnidadNodoEnergORsrvFria(fechaIni, fechaFin);

            List<EqEquipoDTO> listaCentral = listaUnidadData
                .Where(x => x.Emprcodi == emprcodi)
                .GroupBy(x => new { x.Equipadre, x.Central })
                .Select(x => new EqEquipoDTO() { Equipadre = x.Key.Equipadre, Central = x.Key.Central })
                .ToList();

            return listaCentral;
        }

        /// <summary>
        /// Permite obtener las empresas termo para cierto periodo
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasTermo(DateTime fechaIni)
        {
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

            int aplicativo = ConstantesIndisponibilidades.AppPF;
            servIndisp.ListarUnidadTermicoOpComercial(aplicativo, fechaIni, fechaFin, out List<EqEquipoDTO> listaUnidadesTermo, out List<EqEquipoDTO> listaEquiposTermicos, out List<ResultadoValidacionAplicativo> listaMsj);
            List<SiEmpresaDTO> listaEmprsa = listaUnidadesTermo
                .GroupBy(x => new { x.Emprcodi, x.Emprnomb })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi ?? 0, Emprnomb = x.Key.Emprnomb })
                .OrderBy(x => x.Emprnomb)
                .ToList();

            return listaEmprsa;
        }

        /// <summary>
        /// Lista Centrales Termo por Empresa para cierto periodo mensual
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListCentralesTermoXEmpresa(int emprcodi, DateTime fechaIni)
        {
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

            int aplicativo = ConstantesIndisponibilidades.AppPF;
            servIndisp.ListarUnidadTermicoOpComercial(aplicativo, fechaIni, fechaFin, out List<EqEquipoDTO> listaUnidadesTermo, out List<EqEquipoDTO> listaEquiposTermicos, out List<ResultadoValidacionAplicativo> listaMsj);

            List<EqEquipoDTO> listaCentral = listaUnidadesTermo
                .Where(x => x.Emprcodi == emprcodi)
                .GroupBy(x => new { x.Equipadre, x.Central })
                .Select(x => new EqEquipoDTO() { Equipadre = x.Key.Equipadre, Central = x.Key.Central })
                .ToList();

            return listaCentral;
        }

        /// <summary>
        /// Permite obtener las empresas del SEIN
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasSEIN()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }

        /// <summary>
        /// Cambia el estado de la version elegida a "APROBADO : 'A'" y todas las demas a "GENERADO: 'G'"
        /// </summary>
        /// <param name="verscodi"></param>
        /// <param name="recursocodi"></param>
        /// <param name="recacodi"></param>
        /// <returns></returns>
        public void AprobarVersionElegida(int verscodi, int recursocodi, int recacodi)
        {
            List<PfVersionDTO> listaVersiones = GetPfVersionByRevisionRecurso(recacodi, recursocodi);

            foreach (var item in listaVersiones)
            {
                item.Pfversestado = item.Pfverscodi == verscodi ? ConstantesPotenciaFirme.Aprobado : ConstantesPotenciaFirme.Generado;
                this.UpdatePfVersion(item);
            }
        }

        public int GuardarPfVersion(int pfrecacodi, int pfrecucodi, int? irptcodi, string usuario)
        {
            var listVersiones = GetPfVersionByRevisionRecurso(pfrecacodi, pfrecucodi);
            var ultVersion = listVersiones.Any() ? listVersiones.Max(x => x.Pfversnumero) : 0;

            //Guardar un registro de versión
            PfVersionDTO entityVersion = new PfVersionDTO();
            entityVersion.Pfrecacodi = pfrecacodi;
            entityVersion.Irptcodi = irptcodi;
            entityVersion.Pfrecucodi = pfrecucodi;
            entityVersion.Pfversnumero = ultVersion + 1;
            entityVersion.Pfversestado = ConstantesPotenciaFirme.Aprobado;
            entityVersion.Pfversusucreacion = usuario;
            entityVersion.Pfversfeccreacion = DateTime.Now;
            int versionId = this.SavePfVersion(entityVersion);

            //actualizar las versiones
            AprobarVersionElegida(versionId, pfrecucodi, pfrecacodi);

            return versionId;
        }

        public PfVersionDTO ObtenerRevisionesVersionInsumo(int recacodi, int recursocodi)
        {
            List<PfVersionDTO> lstVersionesPorRecurso = GetPfVersionByRevisionRecurso(recacodi, recursocodi).ToList();

            PfVersionDTO version = lstVersionesPorRecurso.Find(x => x.Pfversestado == ConstantesAppServicio.Activo);

            if (version == null)
            {
                version = new PfVersionDTO()
                {
                    Pfverscodi = -1,
                    Descripcion = "El insumo no ha sido procesado.",
                };
            }
            else
            {
                version.Descripcion = string.Format("Se utilizará la versión {0} del insumo.", version.Pfversnumero);
            }

            return version;
        }

        public int GetUltimaPfVersionRecurso(int pfrecacodi, int recursocodi, int versionId)
        {
            var versionActiva = GetPfVersionByRevisionRecurso(pfrecacodi, recursocodi).Find(x => x.Pfversestado == ConstantesAppServicio.Activo);
            versionId = versionActiva != null ? versionActiva.Pfverscodi : versionId;

            return versionId;
        }

        public PfVersionDTO GetPfVersionDefecto()
        {
            return new PfVersionDTO() { Pfverscodi = ConstantesPotenciaFirme.ParametroDefecto, Pfversnumero = -1 };
        }

        #endregion

        #region INSUMO POTENCIA GARANTIZADA

        /// <summary>
        /// Obtiene el listado de las potencias garantizadas de un periodo y revision especifico y para cierto periodo mensual
        /// </summary>
        /// <param name="fechaPeriodoIni"></param>
        /// <param name="pericodi"></param>
        /// <param name="revision"></param>
        /// <param name="emprcodi"></param>
        /// <param name="centralId"></param>
        /// <param name="versionId"></param>
        /// <param name="conRegistro"></param>
        /// <returns></returns>
        public void ListarPotenciaGarantizada(int pfrecacodi, int versionId, int emprcodi, int centralId
                                            , out List<PfPotenciaGarantizadaDTO> lstPGarantizadas, out PfVersionDTO pfVersionRecurso)
        {
            lstPGarantizadas = new List<PfPotenciaGarantizadaDTO>();
            pfVersionRecurso = GetPfVersionDefecto();
            if (versionId == ConstantesPotenciaFirme.ParametroDefecto) versionId = GetUltimaPfVersionRecurso(pfrecacodi, ConstantesPotenciaFirme.RecursoPGarantizada, versionId);

            PfRecalculoDTO regRecalculo = GetByIdPfRecalculo(pfrecacodi);
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            if (versionId > 0)
            {
                pfVersionRecurso = GetByIdPfVersion(versionId);

                //consulta p.garantizada filtros
                lstPGarantizadas = FactorySic.GetPfPotenciaGarantizadaRepository().ListarPotGarantizadaFiltro(regPeriodo.Pfpericodi, pfrecacodi, emprcodi, centralId, versionId);
                lstPGarantizadas.ForEach(x => x.Equinomb = x.Pfpgarunidadnomb);
            }

            if (versionId == ConstantesPotenciaFirme.ParametroTraerBD)
            {
                servIndisp.ListarEqCentralHidraulicoOpComercial(regPeriodo.FechaIni, regPeriodo.FechaFin, out List<EqEquipoDTO> listaEqhidro, out List<EqEquipoDTO> listaEquiposHidro, out List<ResultadoValidacionAplicativo> listaMsj);

                foreach (var item in listaEqhidro)
                {
                    PfPotenciaGarantizadaDTO objPGarantizada = new PfPotenciaGarantizadaDTO();
                    var nombreUnidades = item.Equinomb.Trim().Replace("+", "/");

                    objPGarantizada.Emprcodi = item.Emprcodi.Value;
                    objPGarantizada.Emprnomb = item.Emprnomb;
                    objPGarantizada.Equipadre = item.Equipadre.Value;
                    objPGarantizada.Central = item.Central;
                    objPGarantizada.Equicodi = item.Equicodi;
                    objPGarantizada.Grupocodi = item.Grupocodi;
                    objPGarantizada.Equinomb = nombreUnidades;
                    objPGarantizada.Pfpgarunidadnomb = nombreUnidades;
                    objPGarantizada.Pfpgarpe = item.Pe;
                    objPGarantizada.Pfpgarpg = item.Potenciagarantizada;

                    lstPGarantizadas.Add(objPGarantizada);
                }
            }

            lstPGarantizadas = lstPGarantizadas.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ToList();
        }

        /// <summary>
        /// Guarda registros de potencia garantizada, previamente genera registros de versiones
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="lstPGarantizada"></param>
        /// <param name="pericodi"></param>
        /// <param name="recacodi"></param>
        /// <param name="versionAnterior"></param>
        /// <param name="fechaPeriodoIni"></param>
        public void GuardarPotenciaGarantizada(List<PfPotenciaGarantizadaDTO> lstPGarantizada, string usuario, int recacodi)
        {
            PfRecalculoDTO regRecalculo = GetByIdPfRecalculo(recacodi);
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            int versionId = GuardarPfVersion(recacodi, ConstantesPotenciaFirme.RecursoPGarantizada, null, usuario);

            // Guardar P. Garantizada
            foreach (var item in lstPGarantizada)
            {
                item.Pfpericodi = regPeriodo.Pfpericodi;
                item.Pfverscodi = versionId;

                this.SavePfPotenciaGarantizada(item);
            }
        }

        public void EditarListaPotenciaGarantizada(int pfrecacodi, int versionId, List<PfPotenciaGarantizadaDTO> lstPgInput, string usuario)
        {
            this.ListarPotenciaGarantizada(pfrecacodi, versionId, ConstantesPotenciaFirme.ParametroDefecto, ConstantesPotenciaFirme.ParametroDefecto
                            , out List<PfPotenciaGarantizadaDTO> pGarantizadaList, out PfVersionDTO pfVersionRecurso);

            //Hacer match de las potencias efectiva y garantizada
            foreach (var item in lstPgInput)
            {
                var val = pGarantizadaList.Find(x => x.Equipadre == item.Equipadre && x.Central == item.Central);
                if (val != null)
                {
                    val.Pfpgarpe = item.Pfpgarpe;
                    val.Pfpgarpg = item.Pfpgarpg;
                }
            }

            this.GuardarPotenciaGarantizada(pGarantizadaList, usuario, pfrecacodi);
        }

        /// <summary>
        /// Obtiene las versiones existentes para cierto insumo y para cierta revisión
        /// </summary>
        /// <param name="recacodi"></param>
        /// <param name="recursocodi"></param>
        /// <returns></returns>
        public List<PfVersionDTO> GetPfVersionByRevisionRecurso(int recacodi, int recursocodi)
        {
            return this.GetByCriteriaPfVersions(recacodi, recursocodi);
        }

        /// <summary>
        /// Obtener Nombre del archivo excel exportado
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="lstPGarantizada"></param>
        /// <returns></returns>
        public string ObtenerNombreArchivoFormatoPotenciaGarantizada(string rutaBase, List<PfPotenciaGarantizadaDTO> lstPGarantizada)
        {
            string nombPlantilla = string.Empty;
            string nombArchivo = string.Empty;
            nombPlantilla = ConstantesPotenciaFirme.FormatoPotenciaGarantizada;
            nombArchivo = "FORMATO POTENCIA GARANTIZADA";
            return ObtenerExcelPotenciaGarantizada(rutaBase, nombPlantilla, nombArchivo, lstPGarantizada);
        }

        /// <summary>
        /// Obtener Excel de Potencia garantizada
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="nombPlantilla"></param>
        /// <param name="nombArchivoFormato"></param>
        /// <param name="lstPGarantizada"></param>
        /// <returns></returns>
        private string ObtenerExcelPotenciaGarantizada(string rutaBase, string nombPlantilla, string nombArchivoFormato, List<PfPotenciaGarantizadaDTO> lstPGarantizada)
        {
            var nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            var nombFormato = $"{nombArchivoFormato}{ConstantesAppServicio.ExtensionExcel}";
            var nombCompletFormato = $"{rutaBase}{nombFormato}";
            var archivoPlantilla = new FileInfo(nombCompletPlantilla);

            var nuevoArchivo = new FileInfo(nombCompletFormato);
            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                using (var ws = xlPackage.Workbook.Worksheets[1])
                {
                    int rowIniFiltro = 4;
                    int colIniFiltro = 6;
                    ws.Cells[rowIniFiltro, colIniFiltro].Value = "REPORTE DE POTENCIA GARANTIZADA";

                    //Lista de datos de P GARANTIZADA
                    int rowIni = 9, colIni = 2;
                    int colIniDynamic = colIni, rowIniDynamic = rowIni;

                    lstPGarantizada = lstPGarantizada != null ? lstPGarantizada : new List<PfPotenciaGarantizadaDTO>();

                    ws.Protection.IsProtected = true;

                    foreach (var item in lstPGarantizada)
                    {
                        colIniDynamic = colIni;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Grupocodi;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Emprcodi;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Emprnomb;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Equipadre;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Central;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Equicodi;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Equinomb;

                        var cellPE = ws.Cells[rowIniDynamic, colIniDynamic++];
                        cellPE.Style.Locked = false; cellPE.Value = item.Pfpgarpe;

                        var cellPG = ws.Cells[rowIniDynamic, colIniDynamic++];
                        cellPG.Style.Locked = false; cellPG.Value = item.Pfpgarpg;

                        rowIniDynamic++;
                    }

                    if (lstPGarantizada.Any())
                    {
                        var modelTable = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIniDynamic - 1];
                        UtilExcel.AllBorders(modelTable);

                        var columnSoloLectura = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIni + 6];
                        UtilExcel.BackgroundColor(columnSoloLectura, ColorTranslator.FromHtml("#D2EFF7"));
                    }

                    xlPackage.SaveAs(nuevoArchivo);
                }
            }
            return nombFormato;
        }

        /// <summary>
        /// Obtener Potencia Garantizada Data Excel
        /// </summary>
        /// <param name="stremExcel"></param>
        /// <param name="fdatcodi"></param>
        /// <returns></returns>
        public List<PfPotenciaGarantizadaDTO> ObtenerPotenciaGarantizadaDataExcel(Stream stremExcel, int fdatcodi)
        {
            List<PfPotenciaGarantizadaDTO> lstPGarantizada = new List<PfPotenciaGarantizadaDTO>();
            using (var xlPackage = new ExcelPackage(stremExcel))
            {
                var ws = xlPackage.Workbook.Worksheets[1];

                int rowIni = 8, colIni = 2;

                var dim = ws.Dimension;
                int numColumnas = 10;
                ExcelRange excelRange = ws.Cells[rowIni, colIni, dim.End.Row, numColumnas];
                var dataExcel = (object[,])excelRange.Value;

                var rowLast = dim.End.Row - rowIni;
                if (fdatcodi == ConstantesPotenciaFirme.RecursoPGarantizada)
                {
                    for (int i = 1; i <= rowLast; i++)
                    {
                        int col = 0;
                        var grupocodi = Convert.ToInt32(dataExcel[i, col++]);
                        var exEmprcodi = dataExcel[i, col++]?.ToString();
                        var exEmprnomb = dataExcel[i, col++]?.ToString();
                        var exEquipadre = dataExcel[i, col++]?.ToString();
                        var excentral = dataExcel[i, col++]?.ToString();
                        var exEquicodi = dataExcel[i, col++]?.ToString();
                        var exunidad = dataExcel[i, col++]?.ToString();
                        var expe = dataExcel[i, col++];
                        var expg = dataExcel[i, col];

                        decimal? potEfectiva = null, potGarantizada = null;
                        int emprcodi = 0, equipadre = 0, equicodi = 0;

                        int.TryParse(exEmprcodi, out emprcodi);
                        int.TryParse(exEquipadre, out equipadre);
                        int.TryParse(exEquicodi, out equicodi);

                        if (emprcodi <= 0 || equipadre <= 0 || equicodi <= 0) continue;

                        if (expe != null && expe.IsNumericType()) potEfectiva = Convert.ToDecimal(expe);
                        if (expg != null && expg.IsNumericType()) potGarantizada = Convert.ToDecimal(expg);

                        lstPGarantizada.Add(new PfPotenciaGarantizadaDTO()
                        {
                            Grupocodi = grupocodi,
                            Emprcodi = emprcodi,
                            Emprnomb = exEmprnomb,
                            Equipadre = equipadre,
                            Central = excentral,
                            Equicodi = equicodi,
                            Equinomb = exunidad?.Trim(),
                            Pfpgarpe = potEfectiva,
                            Pfpgarpg = potGarantizada
                        });
                    }
                }
            }

            return lstPGarantizada;
        }

        #endregion

        #region INSUMO POTENCIA ADICIONAL

        /// <summary>
        /// Devuelve las potencias adicionales de un periodo y revision especifico y para cierto periodo mensual
        /// </summary>
        /// <param name="fechaPeriodoIni"></param>
        /// <param name="pericodi"></param>
        /// <param name="revision"></param>
        /// <param name="emprcodi"></param>
        /// <param name="centralId"></param>
        /// <param name="versionId"></param>
        /// <param name="conRegistro"></param>
        /// <returns></returns>
        public void ListarPotenciaAdicional(int pfrecacodi, int versionId, int irptcodi, int emprcodi, int centralId
                                            , out List<PfPotenciaAdicionalDTO> lstPAdicional, out PfVersionDTO pfVersionRecurso)
        {
            lstPAdicional = new List<PfPotenciaAdicionalDTO>();
            pfVersionRecurso = GetPfVersionDefecto();
            if (versionId == ConstantesPotenciaFirme.ParametroDefecto) versionId = GetUltimaPfVersionRecurso(pfrecacodi, ConstantesPotenciaFirme.RecursoPAdicional, versionId);

            PfRecalculoDTO regRecalculo = GetByIdPfRecalculo(pfrecacodi);
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            if (versionId > 0)
            {
                pfVersionRecurso = GetByIdPfVersion(versionId);
                irptcodi = pfVersionRecurso.Irptcodi ?? 0;

                //consulta p.adicional filtros
                lstPAdicional = FactorySic.GetPfPotenciaAdicionalRepository().ListarPotAdicionalFiltro(regPeriodo.Pfpericodi, pfrecacodi, emprcodi, centralId, versionId).OrderBy(x => x.Emprcodi).ThenBy(x => x.Equipadre).ToList();
            }

            if (versionId == ConstantesPotenciaFirme.ParametroTraerBD)
            {
                var listaEqEspecial = servIndisp.ListaUnidadNodoEnergORsrvFria(regPeriodo.FechaIni, regPeriodo.FechaFin);

                foreach (var item in listaEqEspecial)
                {
                    PfPotenciaAdicionalDTO objPAdicional = new PfPotenciaAdicionalDTO();

                    objPAdicional.Emprcodi = item.Emprcodi.Value;
                    objPAdicional.Emprnomb = item.Emprnomb;
                    objPAdicional.Equipadre = item.Equipadre.Value;
                    objPAdicional.Central = item.Central;
                    objPAdicional.Grupocodi = item.Grupocodi;
                    objPAdicional.Famcodi = item.Famcodi ?? 0;
                    objPAdicional.Equicodi = item.Equicodi;
                    objPAdicional.Equinomb = item.Equinomb;
                    objPAdicional.Pfpadiunidadnomb = item.Equiabrev;
                    objPAdicional.Pfpadipe = item.Pe;
                    objPAdicional.Pfpadipf = item.Pf > 0 ? item.Pf : null;
                    objPAdicional.Pfpadiincremental = item.Grupoincremental;

                    lstPAdicional.Add(objPAdicional);
                }
            }

            //factores Fortuito
            var listVersiones = GetPfVersionByRevisionRecurso(pfrecacodi, ConstantesPotenciaFirme.RecursoFactorIndispFortuita).OrderByDescending(x => x.Pfversnumero).ToList();
            int vercodiFI = listVersiones.First().Pfverscodi;
            irptcodi = listVersiones.First().Irptcodi.Value;
            pfVersionRecurso.Irptcodi = irptcodi;

            ListarFactores(ConstantesPotenciaFirme.FactorIndispFortuita, pfrecacodi, vercodiFI, irptcodi, emprcodi, centralId, out List<PfFactoresDTO> listadoFI, out PfVersionDTO pfVersionRecurso3);

            //formatear
            lstPAdicional = lstPAdicional.OrderBy(x => x.Emprnomb).ThenBy(y => y.Central).ThenBy(z => z.Pfpadiunidadnomb).ToList();

            int fila = 0;
            string formula = "";
            foreach (var item in lstPAdicional)
            {
                var regFI = listadoFI.Find(x => x.Equicodi == item.Equicodi && x.Grupocodi == item.Grupocodi);
                if (regFI != null)
                    item.Pfpadifi = regFI.Pffactfi;
                item.Pfpadiunidadnomb = item.Pfpadiunidadnomb + " " + (item.Pfpadiincremental == 1 ? "(*)" : "");
                item.Formula = item.Pfpadiincremental == 1 ? "=+(1- E" + (fila + 1).ToString() + ")*D" + (fila + 1).ToString() : formula;
                fila++;
            }

            lstPAdicional = lstPAdicional.OrderBy(x => x.Emprnomb).ThenBy(y => y.Central).ThenBy(x => x.Pfpadiincremental).ThenBy(z => z.Pfpadiunidadnomb).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="lstPAdicional"></param>
        /// <param name="pericodi"></param>
        /// <param name="recacodi"></param>
        /// <param name="versionAnterior"></param>
        /// <param name="fechaPeriodoIni"></param>
        public void GuardarPotenciaAdicional(List<PfPotenciaAdicionalDTO> lstPAdicional, string usuario, int recacodi, int irptcodi)
        {
            PfRecalculoDTO regRecalculo = GetByIdPfRecalculo(recacodi);
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            int versionId = GuardarPfVersion(recacodi, ConstantesPotenciaFirme.RecursoPAdicional, irptcodi, usuario);

            // Guardar P. Adicional
            foreach (var item in lstPAdicional)
            {
                //setear equicodi fictisio 900000
                item.Pfpadiunidadnomb = item.Pfpadiunidadnomb != null ? item.Pfpadiunidadnomb.Replace(@"(*)", @"") : "";

                item.Pfpericodi = regPeriodo.Pfpericodi;
                item.Pfverscodi = versionId;

                this.SavePfPotenciaAdicional(item);
            }
        }

        public void EditarListaPotenciaAdicional(int pfrecacodi, int versionId, List<PfPotenciaAdicionalDTO> lstPAdInput, string usuario)
        {
            this.ListarPotenciaAdicional(pfrecacodi, versionId, -1, ConstantesPotenciaFirme.ParametroDefecto, ConstantesPotenciaFirme.ParametroDefecto
                            , out List<PfPotenciaAdicionalDTO> lstPAdicional, out PfVersionDTO pfVersionRecurso);

            //Hacer match de los factores
            foreach (var item in lstPAdInput)
            {
                var val = lstPAdicional.Find(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi);

                if (val != null)
                {
                    val.Pfpadipe = item.Pfpadipe;
                    val.Pfpadifi = item.Pfpadifi;
                    val.Pfpadipf = item.Pfpadipf;
                }
            }

            this.GuardarPotenciaAdicional(lstPAdicional, usuario, pfrecacodi, pfVersionRecurso.Irptcodi.Value);
        }

        /// <summary>
        /// Obtener Nombre del archivo excel exportado
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="lstPGarantizada"></param>
        /// <returns></returns>
        public string ObtenerNombreArchivoFormatoPotenciaAdicional(string rutaBase, List<PfPotenciaAdicionalDTO> lstPAdicional)
        {
            string nombPlantilla = string.Empty;
            string nombArchivo = string.Empty;
            nombPlantilla = ConstantesPotenciaFirme.FormatoPotenciaAdicional;
            nombArchivo = "FORMATO POTENCIA ADICIONAL";
            return ObtenerExcelPotenciaAdicional(rutaBase, nombPlantilla, nombArchivo, lstPAdicional);
        }

        /// <summary>
        /// Obtener Excel de Potencia adicional
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="nombPlantilla"></param>
        /// <param name="nombArchivoFormato"></param>
        /// <param name="lstPGarantizada"></param>
        /// <returns></returns>
        private string ObtenerExcelPotenciaAdicional(string rutaBase, string nombPlantilla, string nombArchivoFormato, List<PfPotenciaAdicionalDTO> lstPAdicional)
        {
            var nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            var nombFormato = $"{nombArchivoFormato}{ConstantesAppServicio.ExtensionExcel}";
            var nombCompletFormato = $"{rutaBase}{nombFormato}";
            var archivoPlantilla = new FileInfo(nombCompletPlantilla);

            var nuevoArchivo = new FileInfo(nombCompletFormato);
            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                using (var ws = xlPackage.Workbook.Worksheets[1])
                {
                    int rowIniFiltro = 4;
                    int colIniFiltro = 6;
                    ws.Cells[rowIniFiltro, colIniFiltro].Value = "REPORTE DE POTENCIA ADICIONAL";

                    //Lista de datos de P Adicional
                    int rowIni = 9, colIni = 1;
                    int colIniDynamic = colIni, rowIniDynamic = rowIni;

                    lstPAdicional = lstPAdicional != null ? lstPAdicional : new List<PfPotenciaAdicionalDTO>();

                    ws.Protection.IsProtected = true;

                    foreach (var item in lstPAdicional)
                    {
                        colIniDynamic = colIni;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Grupocodi;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Pfpadiincremental;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Emprcodi;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Emprnomb;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Equipadre;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Central;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Equicodi;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Pfpadiunidadnomb;

                        var cellPE = ws.Cells[rowIniDynamic, colIniDynamic++];
                        cellPE.Style.Locked = false; cellPE.Value = item.Pfpadipe;

                        var cellFI = ws.Cells[rowIniDynamic, colIniDynamic++];
                        cellFI.Style.Locked = true; cellFI.Value = item.Pfpadifi;
                        cellFI.Style.Numberformat.Format = "0.00%";

                        var cellPF = ws.Cells[rowIniDynamic, colIniDynamic++];
                        cellPF.Style.Locked = false; cellPF.Value = item.Pfpadipf;

                        rowIniDynamic++;
                    }

                    if (lstPAdicional.Any())
                    {
                        var modelTable = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIniDynamic - 1];
                        UtilExcel.AllBorders(modelTable);

                        var columnSoloLectura = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIni + 7];
                        UtilExcel.BackgroundColor(columnSoloLectura, ColorTranslator.FromHtml("#D2EFF7"));

                        UtilExcel.BackgroundColor(ws.Cells[rowIni, 10, rowIniDynamic - 1, 10], ColorTranslator.FromHtml("#D2EFF7"));
                    }

                    xlPackage.SaveAs(nuevoArchivo);
                }
            }
            return nombFormato;
        }

        /// <summary>
        /// Obtener Potencia Garantizada Data Excel
        /// </summary>
        /// <param name="stremExcel"></param>
        /// <param name="fdatcodi"></param>
        /// <returns></returns>
        public List<PfPotenciaAdicionalDTO> ObtenerPotenciaAdicionalDataExcel(Stream stremExcel, int fdatcodi)
        {
            List<PfPotenciaAdicionalDTO> lstPAdicional = new List<PfPotenciaAdicionalDTO>();
            using (var xlPackage = new ExcelPackage(stremExcel))
            {
                var ws = xlPackage.Workbook.Worksheets[1];
                int rowIni = 8, colIni = 1;

                var dim = ws.Dimension;
                int numColumnas = 11;
                ExcelRange excelRange = ws.Cells[rowIni, colIni, dim.End.Row, numColumnas];
                var dataExcel = (object[,])excelRange.Value;

                var rowLast = dim.End.Row - rowIni;
                if (fdatcodi == ConstantesPotenciaFirme.RecursoPAdicional)
                {
                    for (int i = 1; i <= rowLast; i++)
                    {
                        int col = 0;
                        var grupocodi = Convert.ToInt32(dataExcel[i, col++]);
                        var incremental = Convert.ToInt32(dataExcel[i, col++]);
                        var exEmprcodi = dataExcel[i, col++]?.ToString();
                        var exEmprnomb = dataExcel[i, col++]?.ToString();
                        var exEquipadre = dataExcel[i, col++]?.ToString();
                        var excentral = dataExcel[i, col++]?.ToString();
                        var exEquicodi = dataExcel[i, col++]?.ToString();
                        var exunidad = dataExcel[i, col++]?.ToString();
                        var expe = dataExcel[i, col++];
                        var exfi = dataExcel[i, col++];
                        var expf = dataExcel[i, col];

                        decimal? potEfectiva = null, factorIndisp = null, potFirme = null;
                        int emprcodi = 0, equipadre = 0, equicodi = 0;

                        int.TryParse(exEmprcodi, out emprcodi);
                        int.TryParse(exEquipadre, out equipadre);
                        int.TryParse(exEquicodi, out equicodi);

                        if (expe != null && expe.IsNumericType()) potEfectiva = Convert.ToDecimal(expe);
                        if (exfi != null && exfi.IsNumericType()) factorIndisp = Convert.ToDecimal(exfi);
                        if (expf != null && expf.IsNumericType()) potFirme = Convert.ToDecimal(expf);

                        if (emprcodi <= 0 || equipadre <= 0 || equicodi <= 0) continue;


                        lstPAdicional.Add(new PfPotenciaAdicionalDTO()
                        {
                            Grupocodi = grupocodi,
                            Pfpadiincremental = incremental,
                            Emprcodi = emprcodi,
                            Emprnomb = exEmprnomb,
                            Equipadre = equipadre,
                            Central = excentral,
                            Equicodi = equicodi,
                            Pfpadiunidadnomb = exunidad?.Trim(),
                            Pfpadipe = potEfectiva,
                            Pfpadifi = factorIndisp,
                            Pfpadipf = potFirme
                        });
                    }
                }
            }

            return lstPAdicional;
        }

        #endregion

        #region INSUMO Disponibilidad de Calor Útil

        #endregion

        #region INSUMO FACTOR DE INDISPONIBILIDAD Y PRESENCIA

        /// <summary>
        /// Devuelve factores de indisponibilidad o presencia de un periodo y revision especifico y para cierto periodo mensual
        /// </summary>
        /// <param name="fechaPeriodoIni"></param>
        /// <param name="pericodi"></param>
        /// <param name="revision"></param>
        /// <param name="emprcodi"></param>
        /// <param name="centralId"></param>
        /// <param name="versionId"></param>
        /// <param name="conRegistro"></param>
        /// <returns></returns>
        public void ListarFactores(int tipoFactor, int pfrecacodi, int versionId, int irptcodi, int emprcodi, int centralId
                                                    , out List<PfFactoresDTO> lstFactores, out PfVersionDTO pfVersionRecurso)
        {
            int recursocodi = tipoFactor == ConstantesPotenciaFirme.FactorIndispFortuita ? ConstantesPotenciaFirme.RecursoFactorIndispFortuita : ConstantesPotenciaFirme.RecursoFactorPresencia;
            int cuadro = tipoFactor == ConstantesPotenciaFirme.FactorIndispFortuita ? ConstantesIndisponibilidades.ReportePR25FactorFortTermico : ConstantesIndisponibilidades.ReportePR25FactorPresencia;

            lstFactores = new List<PfFactoresDTO>();
            pfVersionRecurso = GetPfVersionDefecto();
            if (versionId == ConstantesPotenciaFirme.ParametroDefecto) versionId = GetUltimaPfVersionRecurso(pfrecacodi, recursocodi, versionId);

            PfRecalculoDTO regRecalculo = GetByIdPfRecalculo(pfrecacodi);
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            if (versionId > 0)
            {
                pfVersionRecurso = GetByIdPfVersion(versionId);

                //consulta factores filtros
                lstFactores = FactorySic.GetPfFactoresRepository().ListarFactoresFiltro(regPeriodo.Pfpericodi, pfrecacodi, emprcodi, centralId, versionId).OrderBy(x => x.Emprcodi).ThenBy(x => x.Equipadre).ToList();
                lstFactores = lstFactores.Where(x => x.Pffacttipo == tipoFactor).ToList();
            }

            if (versionId == ConstantesPotenciaFirme.ParametroTraerBD)
            {
                IndCuadroDTO regCuadro = servIndisp.GetByIdIndCuadro(cuadro);

                if (irptcodi <= 0)
                {
                    servIndisp.GetCodigoReporteAprobadoXCuadro(regRecalculo.Irecacodi, cuadro, out irptcodi, out int numVersion8, out string mensaje8);
                    pfVersionRecurso.Irptcodi = irptcodi;
                }

                servIndisp.ListaDataXVersionReporte(irptcodi, regCuadro.ListaFamcodi, ConstantesAppServicio.SI, emprcodi.ToString(), centralId.ToString()
                                           , out List<IndReporteTotalDTO> listaReptot, out List<IndReporteDetDTO> listaRepdetOut
                                            , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral);

                foreach (var item in listaReptot)
                {
                    PfFactoresDTO objPAdicional = new PfFactoresDTO();
                    var nombreUnidades = item.Equinomb.Trim().Replace("+", "/");

                    objPAdicional.Emprcodi = item.Emprcodi;
                    objPAdicional.Emprnomb = item.Emprnomb;
                    objPAdicional.Equipadre = item.Equipadre;
                    objPAdicional.Central = item.Central;
                    objPAdicional.Grupocodi = item.Grupocodi;
                    objPAdicional.Famcodi = item.Famcodi;
                    objPAdicional.Equicodi = item.Equicodi;
                    objPAdicional.Equinomb = nombreUnidades;
                    if (tipoFactor == ConstantesPotenciaFirme.FactorIndispFortuita)
                    {
                        objPAdicional.Pffactunidadnomb = item.Itotunidadnomb;
                        objPAdicional.Pffactfi = item.Itotfactorif;
                    }
                    if (tipoFactor == ConstantesPotenciaFirme.FactorPresencia)
                    {
                        objPAdicional.Pffactunidadnomb = nombreUnidades;
                        objPAdicional.Pffactfp = item.Itotfactorpresm;
                    }
                    objPAdicional.Pffacttipo = tipoFactor;

                    objPAdicional.Pffactincremental = item.Itotincremental;

                    lstFactores.Add(objPAdicional);
                }
            }

            lstFactores = lstFactores.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Pffactincremental).ThenBy(x => x.Pffactunidadnomb).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="lstPAdicional"></param>
        /// <param name="pericodi"></param>
        /// <param name="recacodi"></param>
        /// <param name="versionAnterior"></param>
        /// <param name="fechaPeriodoIni"></param>
        public void GuardarFactor(int tipoFactor, List<PfFactoresDTO> lstFactores, string usuario, int recacodi, int irptcodi)
        {
            PfRecalculoDTO regRecalculo = GetByIdPfRecalculo(recacodi);
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            int tipoRecursoFactor = tipoFactor == ConstantesPotenciaFirme.FactorIndispFortuita ? ConstantesPotenciaFirme.RecursoFactorIndispFortuita : ConstantesPotenciaFirme.RecursoFactorPresencia;
            int versionId = GuardarPfVersion(recacodi, tipoRecursoFactor, irptcodi, usuario);

            // Guardar Factor
            foreach (var item in lstFactores)
            {
                item.Pfpericodi = regPeriodo.Pfpericodi;
                item.Pfverscodi = versionId;

                this.SavePfFactores(item);
            }
        }

        public void EditarListaFactor(int pfrecacodi, int versionId, int tipoFactor, List<PfFactoresDTO> listaFact, string usuario)
        {
            this.ListarFactores(tipoFactor, pfrecacodi, versionId, -1, ConstantesPotenciaFirme.ParametroDefecto, ConstantesPotenciaFirme.ParametroDefecto
                            , out List<PfFactoresDTO> lstFactores, out PfVersionDTO pfVersionRecurso);

            //Hacer match de los factores
            foreach (var item in listaFact)
            {
                var val = tipoFactor == ConstantesPotenciaFirme.FactorIndispFortuita
                            ? lstFactores.Find(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi)
                            : lstFactores.Find(x => x.Equipadre == item.Equipadre);
                if (val != null)
                {
                    val.Pffactfi = item.Pffactfi;
                    val.Pffactfp = item.Pffactfp;
                }
            }

            this.GuardarFactor(tipoFactor, lstFactores, usuario, pfrecacodi, pfVersionRecurso.Irptcodi.Value);
        }

        /// <summary>
        /// Obtiene el nombre del archivo Formato Factores
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="listaFP"></param>
        /// <param name="factorPresencia"></param>
        /// <returns></returns>
        public string ObtenerNombreArchivoFormatoFactores(string rutaBase, List<PfFactoresDTO> listaFP, int factorPresencia)
        {
            string nombreArchivo = "";
            switch (factorPresencia)
            {
                case ConstantesPotenciaFirme.FactorPresencia:
                    var nomArchivoFP = "FORMATO FACTOR PRESENCIA";
                    nombreArchivo = GenerarExcelFactorPresencia(rutaBase, ConstantesPotenciaFirme.FormatoFactorPresencia, nomArchivoFP, listaFP);
                    break;

                case ConstantesPotenciaFirme.FactorIndispFortuita:
                    var nomArchivoIF = "FACTOR  DE INDISPONIBILIDAD FORTUÍTA";
                    nombreArchivo = GenerarExcelFactorIndispFortuita(rutaBase, ConstantesPotenciaFirme.FormatoIndisponibilidadFortuita, nomArchivoIF, listaFP);
                    break;
            }

            return nombreArchivo;
        }

        /// <summary>
        /// Genera archivo excel con datos de Factor de Indisponibilidad Fortuita
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="formatoIndisponibilidadFortuita"></param>
        /// <param name="nomArchivoIF"></param>
        /// <param name="listaFP"></param>
        /// <returns></returns>
        private string GenerarExcelFactorIndispFortuita(string rutaBase, string nombPlantilla, string nombArchivoFormato, List<PfFactoresDTO> listaFP)
        {
            var nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            var archivoPlantilla = new FileInfo(nombCompletPlantilla);

            var nombFormato = $"{nombArchivoFormato}{ConstantesAppServicio.ExtensionExcel}";
            var nombCompletFormato = $"{rutaBase}{nombFormato}";

            var nuevoArchivo = new FileInfo(nombCompletFormato);
            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                using (var ws = xlPackage.Workbook.Worksheets[1])
                {

                    int rowIni = 9, colIni = 2;
                    int colIniDynamic = colIni, rowIniDynamic = rowIni;

                    if (listaFP.Any())
                    {
                        ws.Protection.IsProtected = true;

                        foreach (var item in listaFP)
                        {
                            colIniDynamic = colIni;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Grupocodi;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Emprcodi;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Emprnomb;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Equipadre;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Central;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Equicodi;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Pffactunidadnomb;

                            var cellPE = ws.Cells[rowIniDynamic, colIniDynamic++];
                            cellPE.Style.Locked = false; cellPE.Value = item.Pffactfi;
                            cellPE.Style.Numberformat.Format = "0.00%";

                            rowIniDynamic++;
                        }

                        var modelTable = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIniDynamic - 1];

                        ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIniDynamic - 2].Style.Font.Color.SetColor(Color.Black);
                        modelTable.Style.Font.Size = 10;
                        UtilExcel.AllBorders(modelTable, ExcelBorderStyle.Thin, ColorTranslator.FromHtml("#cccccc"));

                        var columnSoloLectura = ws.Cells[rowIni, colIni, --rowIniDynamic, colIni + 6];
                        UtilExcel.BackgroundColor(columnSoloLectura, ColorTranslator.FromHtml("#D2EFF7"));
                        ws.View.ZoomScale = 100;
                    }

                    xlPackage.SaveAs(nuevoArchivo);
                }
            }
            return nombFormato;
        }

        /// <summary>
        /// Genera archivo excel con datos de Factor Presencia
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="nombPlantilla"></param>
        /// <param name="nombArchivoFormato"></param>
        /// <param name="listaFP"></param>
        /// <returns></returns>
        private string GenerarExcelFactorPresencia(string rutaBase, string nombPlantilla, string nombArchivoFormato, List<PfFactoresDTO> listaFP)
        {
            var nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            var archivoPlantilla = new FileInfo(nombCompletPlantilla);

            var nombFormato = $"{nombArchivoFormato}{ConstantesAppServicio.ExtensionExcel}";
            var nombCompletFormato = $"{rutaBase}{nombFormato}";

            var nuevoArchivo = new FileInfo(nombCompletFormato);
            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                using (var ws = xlPackage.Workbook.Worksheets[1])
                {

                    int rowIni = 9, colIni = 2;
                    int colIniDynamic = colIni, rowIniDynamic = rowIni;

                    if (listaFP.Any())
                    {
                        ws.Protection.IsProtected = true;

                        foreach (var item in listaFP)
                        {
                            colIniDynamic = colIni;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Grupocodi;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Emprcodi;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Emprnomb;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Equipadre;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Central;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Equicodi;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = item.Pffactunidadnomb;

                            ws.Cells[rowIniDynamic, colIniDynamic].Style.Locked = true;

                            var cellPE = ws.Cells[rowIniDynamic, colIniDynamic++];
                            cellPE.Style.Locked = false; cellPE.Value = item.Pffactfp;

                            rowIniDynamic++;
                        }

                        var modelTable = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIniDynamic - 1];

                        ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIniDynamic - 2].Style.Font.Color.SetColor(Color.Black);
                        modelTable.Style.Font.Size = 10;
                        UtilExcel.AllBorders(modelTable, ExcelBorderStyle.Thin, ColorTranslator.FromHtml("#cccccc"));

                        var columnSoloLectura = ws.Cells[rowIni, colIni, --rowIniDynamic, colIni + 6];
                        UtilExcel.BackgroundColor(columnSoloLectura, ColorTranslator.FromHtml("#D2EFF7"));
                        ws.View.ZoomScale = 100;
                    }

                    xlPackage.SaveAs(nuevoArchivo);
                }
            }
            return nombFormato;
        }

        /// <summary>
        /// Retorna lista a partir de data excel Factor Presencia
        /// </summary>
        /// <param name="stremExcel"></param>
        /// <returns></returns>
        public List<PfFactoresDTO> ObtenerFactorePresenciaDataExcel(Stream stremExcel)
        {
            List<PfFactoresDTO> lstFactor = new List<PfFactoresDTO>();
            using (var xlPackage = new ExcelPackage(stremExcel))
            {
                var ws = xlPackage.Workbook.Worksheets[1];

                int rowIni = 8, colIni = 2;

                var dim = ws.Dimension;
                int numColumnas = 10;
                ExcelRange excelRange = ws.Cells[rowIni, colIni, dim.End.Row, numColumnas];
                var dataExcel = (object[,])excelRange.Value;

                var rowLast = dim.End.Row - rowIni;

                for (int i = 1; i <= rowLast; i++)
                {
                    int col = 0;
                    var grupocodi = Convert.ToInt32(dataExcel[i, col++]);
                    var exEmprcodi = dataExcel[i, col++]?.ToString();
                    var exEmprnomb = dataExcel[i, col++]?.ToString();
                    var exEquipadre = dataExcel[i, col++]?.ToString();
                    var excentral = dataExcel[i, col++]?.ToString();
                    var exEquicodi = dataExcel[i, col++]?.ToString();
                    var exunidad = dataExcel[i, col++]?.ToString();
                    var facfp = dataExcel[i, col];

                    decimal? valFacPres = null;
                    int emprcodi = 0, equipadre = 0, equicodi = 0;

                    int.TryParse(exEmprcodi, out emprcodi);
                    int.TryParse(exEquipadre, out equipadre);
                    int.TryParse(exEquicodi, out equicodi);

                    if (facfp != null && facfp.IsNumericType()) valFacPres = Convert.ToDecimal(facfp);

                    if (emprcodi <= 0 || equipadre <= 0) continue;

                    lstFactor.Add(new PfFactoresDTO()
                    {
                        Grupocodi = grupocodi,
                        Emprcodi = emprcodi,
                        Emprnomb = exEmprnomb,
                        Equipadre = equipadre,
                        Central = excentral,
                        Equicodi = equicodi,
                        Pffactunidadnomb = exunidad?.Trim(),
                        Pffactfp = valFacPres
                    });
                }

            }

            return lstFactor;
        }

        /// <summary>
        /// Retorna lista a partir de data excel Factor de Indis. Fortuita
        /// </summary>
        /// <param name="stremExcel"></param>
        /// <returns></returns>
        public List<PfFactoresDTO> ObtenerFactoreIndisFortuitaDataExcel(Stream stremExcel)
        {
            List<PfFactoresDTO> lstFactor = new List<PfFactoresDTO>();
            using (var xlPackage = new ExcelPackage(stremExcel))
            {
                var ws = xlPackage.Workbook.Worksheets[1];

                int rowIni = 8, colIni = 2;

                var dim = ws.Dimension;
                int numColumnas = 10;
                ExcelRange excelRange = ws.Cells[rowIni, colIni, dim.End.Row, numColumnas];
                var dataExcel = (object[,])excelRange.Value;

                var rowLast = dim.End.Row - rowIni;

                for (int i = 1; i <= rowLast; i++)
                {
                    int col = 0;
                    var grupocodi = Convert.ToInt32(dataExcel[i, col++]);
                    var exEmprcodi = dataExcel[i, col++]?.ToString();
                    var exEmprnomb = dataExcel[i, col++]?.ToString();
                    var exEquipadre = dataExcel[i, col++]?.ToString();
                    var excentral = dataExcel[i, col++]?.ToString();
                    var exEquicodi = dataExcel[i, col++]?.ToString();
                    var exunidad = dataExcel[i, col++]?.ToString();
                    var facfi = dataExcel[i, col];

                    decimal? valFacIndis = null;
                    int emprcodi = 0, equipadre = 0, equicodi = 0;

                    int.TryParse(exEmprcodi, out emprcodi);
                    int.TryParse(exEquipadre, out equipadre);
                    int.TryParse(exEquicodi, out equicodi);

                    if (facfi != null && facfi.IsNumericType()) valFacIndis = Convert.ToDecimal(facfi);

                    if (emprcodi <= 0 || equipadre <= 0) continue;

                    lstFactor.Add(new PfFactoresDTO()
                    {
                        Grupocodi = grupocodi,
                        Emprcodi = emprcodi,
                        Emprnomb = exEmprnomb,
                        Equipadre = equipadre,
                        Central = excentral,
                        Equicodi = equicodi,
                        Pffactunidadnomb = exunidad?.Trim(),
                        Pffactfi = valFacIndis
                    });
                }

            }

            return lstFactor;
        }

        #endregion

        #region INSUMO CONTRATOS DE COMPRA Y VENTA

        /// <summary>
        /// Obtiene el listado de los contratos de compra y venta para un periodo específico
        /// </summary>
        /// <param name="fechaPeriodoIni"></param>
        /// <param name="pericodi"></param>
        /// <param name="revision"></param>
        /// <param name="emprcodi"></param>
        /// <param name="centralId"></param>
        /// <param name="versionId"></param>
        /// <param name="conRegistro"></param>
        /// <returns></returns>
        public void ListarContratosCV(int pfrecacodi, int versionId, out List<PfContratosDTO> lstContratosCV, out PfVersionDTO pfVersionRecurso)
        {
            lstContratosCV = new List<PfContratosDTO>();
            pfVersionRecurso = GetPfVersionDefecto();
            if (versionId == ConstantesPotenciaFirme.ParametroDefecto) versionId = GetUltimaPfVersionRecurso(pfrecacodi, ConstantesPotenciaFirme.RecursoContratosCV, versionId);

            PfRecalculoDTO regRecalculo = GetByIdPfRecalculo(pfrecacodi);
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            if (versionId > 0)
            {
                pfVersionRecurso = GetByIdPfVersion(versionId);

                //consulta Contratos de Compra y Venta filtros
                lstContratosCV = FactorySic.GetPfContratosRepository().ListarContratosCVFiltro(regPeriodo.Pfpericodi, pfrecacodi, versionId).OrderBy(x => x.Pfcontnombcedente).ThenBy(x => x.Pfcontnombcesionario).ToList();

                foreach (var reg in lstContratosCV)
                {
                    reg.PfcontvigenciainiDesc = reg.Pfcontvigenciaini.Value.ToString(ConstantesAppServicio.FormatoFecha);
                    reg.PfcontvigenciafinDesc = reg.Pfcontvigenciafin.Value.ToString(ConstantesAppServicio.FormatoFecha);
                }
            }
        }

        /// <summary>
        /// Guarda registros de potencia garantizada, previamente genera registros de versiones
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="lstPGarantizada"></param>
        /// <param name="pericodi"></param>
        /// <param name="recacodi"></param>
        /// <param name="versionAnterior"></param>
        /// <param name="fechaPeriodoIni"></param>
        public void GuardarContratosCV(List<PfContratosDTO> lstContratosCV, string usuario, int recacodi)
        {
            PfVersionDTO entityVersion;

            PfRecalculoDTO regRecalculo = GetByIdPfRecalculo(recacodi);
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            int versionId = -1;

            var listVersiones = GetPfVersionByRevisionRecurso(recacodi, ConstantesPotenciaFirme.RecursoContratosCV);
            var ultVersion = listVersiones.Any() ? listVersiones.Max(x => x.Pfversnumero) : 0;

            //Guardar una version, antes de guardar la potencia garantizada
            entityVersion = new PfVersionDTO();
            entityVersion.Pfrecacodi = recacodi;
            entityVersion.Pfrecucodi = ConstantesPotenciaFirme.RecursoContratosCV;
            //entityVersion.Irptcodi = 1;  //deshabilitado temporalmente hasta unirlo con modulo indisponibilidad
            entityVersion.Pfversnumero = ultVersion + 1;
            entityVersion.Pfversestado = ConstantesPotenciaFirme.Aprobado;
            entityVersion.Pfversusucreacion = usuario;
            entityVersion.Pfversfeccreacion = DateTime.Now;

            versionId = this.SavePfVersion(entityVersion);

            //Actualizar las versiones
            AprobarVersionElegida(versionId, ConstantesPotenciaFirme.RecursoContratosCV, recacodi);

            // Guardar Contratos de CV
            foreach (var item in lstContratosCV)
            {
                item.Pfcontvigenciaini = DateTime.ParseExact(item.PfcontvigenciainiDesc, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                item.Pfcontvigenciafin = DateTime.ParseExact(item.PfcontvigenciafinDesc, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                item.Pfpericodi = regPeriodo.Pfpericodi;
                item.Pfverscodi = versionId;

                this.SavePfContratos(item);
            }
        }

        /// <summary>
        /// Obtiene el nombre del archivo Formato Contrato Compra y Venta
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="listaCCV"></param>
        /// <returns></returns>
        public string ObtenerNombreArchivoFormatoContratoCV(string rutaBase, List<PfContratosDTO> listaCCV)
        {
            var nombCompletPlantilla = $"{rutaBase}{ConstantesPotenciaFirme.FormatoContratoCV}";
            var archivoPlantilla = new FileInfo(nombCompletPlantilla);

            var nombFormato = $"CONTRATO DE COMPRA Y VENTA{ConstantesAppServicio.ExtensionExcel}";
            var nombCompletFormato = $"{rutaBase}{nombFormato}";

            var nuevoArchivo = new FileInfo(nombCompletFormato);
            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                using (var ws = xlPackage.Workbook.Worksheets[1])
                {

                    int rowIni = 10, colIni = 2;
                    int colCedenteCesi = colIni, colCantidad = 7, colFechaIni = 8, colFechaFin = 9, colObservaciones = 10;

                    int rowIniDynamic = rowIni;

                    if (listaCCV.Any())
                    {
                        foreach (var item in listaCCV)
                        {
                            ws.Cells[rowIniDynamic, colCedenteCesi].Value = $"{item.Pfcontnombcedente} a {item.Pfcontnombcesionario}";
                            ws.Cells[rowIniDynamic, colCantidad].Value = item.Pfcontcantidad;
                            ws.Cells[rowIniDynamic, colCantidad].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniDynamic, colFechaIni].Value = item.PfcontvigenciainiDesc;
                            ws.Cells[rowIniDynamic, colFechaIni].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniDynamic, colFechaFin].Value = item.PfcontvigenciafinDesc;
                            ws.Cells[rowIniDynamic, colFechaFin].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[rowIniDynamic++, colObservaciones].Value = item.Pfcontobservacion;
                        }

                        var modelTable = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colObservaciones];
                        modelTable.Style.Font.Size = 10;

                        UtilExcel.BorderAround(ws.Cells[rowIni, colCedenteCesi, rowIniDynamic - 1, colCantidad - 1]);
                        UtilExcel.BorderAround(ws.Cells[rowIni, colCantidad, rowIniDynamic - 1, colCantidad]);
                        UtilExcel.BorderAround(ws.Cells[rowIni, colFechaIni, rowIniDynamic - 1, colFechaIni]);
                        UtilExcel.BorderAround(ws.Cells[rowIni, colFechaFin, rowIniDynamic - 1, colFechaFin]);
                        UtilExcel.BorderAround(ws.Cells[rowIni, colObservaciones, rowIniDynamic - 1, colObservaciones + 2]);

                        ws.View.ZoomScale = 100;
                    }

                    xlPackage.SaveAs(nuevoArchivo);
                }
            }
            return nombFormato;
        }

        #endregion

        #region PERIODO Y RECÁLCULOS

        /// <summary>
        /// Genera el listado de los periodos existentes
        /// </summary>
        /// <param name="url"></param>
        /// <param name="horizonte"></param>
        /// <param name="tienePermisoEditar"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoPeriodo(string url)
        {
            List<PfPeriodoDTO> listaDataPeriodo = this.GetByCriteriaPfPeriodos(-1).OrderByDescending(x => x.Pfperianiomes).ToList();
            List<PfRecalculoDTO> listaDataRecalculo = this.ListPfRecalculos().OrderByDescending(x => x.Pfrecacodi).ToList();

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_periodo'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 60px;'>Listado de <br/> Recálculo</th>");
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
                var regRecalculo = listaDataRecalculo.Find(x => x.Pfpericodi == reg.Pfpericodi);
                string claseRec = regRecalculo != null && regRecalculo.Estado == ConstantesIndisponibilidades.EstadoPeriodoAbierto ? "clase_recalculo_activo" : "";

                str.Append("<tr>");
                str.Append("<td style='width: 60px;'>");
                str.AppendFormat("<a class='' href='JavaScript:verListadoRecalculo(" + reg.Pfpericodi + ");' style='margin-right: 4px;'><img style='padding-left: 35px; margin-top: 3px; margin-bottom: 3px;' src='" + url + "Content/Images/btn-properties.png' alt='Ver listado de recalculo' title='Ver listado de recalculo' /></a>");
                str.Append("</td>");

                str.AppendFormat("<td class='' style='width: 100px; text-align: center'>{0}</td>", reg.Pfperinombre);
                str.AppendFormat("<td class='' style='width: 100px; text-align: center'>{0}</td>", reg.Pfperianio);
                str.AppendFormat("<td class='' style='width: 100px; text-align: center'>{0}</td>", reg.Pfperimes);

                if (regRecalculo != null)
                {
                    str.AppendFormat("<td class='{1}' style='width: 120px; text-align: center'>{0}</td>", regRecalculo.PfrecaestadoDesc, claseRec);
                    str.AppendFormat("<td class='' style='width: 150px; text-align: center'>{0}</td>", regRecalculo.Pfrecanombre);
                    str.AppendFormat("<td class='' style='width: 450px; text-align: left'>{0}</td>", regRecalculo.Pfrecadescripcion);
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
        public string GenerarHtmlListadoRecalculo(string url, bool tienePermisoEditar, int pfPericodi, out string ultimoTipoRecalculo, out bool tieneReportePf)
        {
            List<PfRecalculoDTO> listaDataRecalculo = this.GetByCriteriaPfRecalculos(pfPericodi).OrderByDescending(x => x.Pfrecacodi).ToList();
            ultimoTipoRecalculo = listaDataRecalculo.Any() ? listaDataRecalculo.First().Pfrecatipo : string.Empty;

            List<PfReporteDTO> lstReportes = ListPfReportes();

            var ultimoRecalculo = listaDataRecalculo.Any() ? listaDataRecalculo.First() : null;
            tieneReportePf = false;
            if (ultimoRecalculo != null)
                tieneReportePf = lstReportes.Find(c => c.Pfrecacodi == ultimoRecalculo.Pfrecacodi) != null;

            StringBuilder str = new StringBuilder();
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_recalculo'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='background: #9370DB; width: 120px'>Opciones</th>");
            str.Append("<th style='background: #9370DB; width: 150px'>Estado</th>");
            str.Append("<th style='background: #9370DB; width: 50px'>Tipo</th>");
            str.Append("<th style='background: #9370DB; width: 150px'>Nombre</th>");
            str.Append("<th style='background: #9370DB; width: 460px'>Comentario</th>");
            str.Append("<th style='background: #9370DB; width: 460px'>Informe</th>");
            str.Append("<th style='background: #9370DB; width: 220px'>Usuario últ. modif.</th>");
            str.Append("<th style='background: #9370DB; width: 220px'>Fecha últ. modif.</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var regRecalculo in listaDataRecalculo)
            {
                var recacodi = regRecalculo.Pfrecacodi;
                var reporte = lstReportes.Find(c => c.Pfrecacodi == recacodi);
                string claseRec = regRecalculo != null && regRecalculo.Estado == ConstantesIndisponibilidades.EstadoPeriodoAbierto ? "clase_recalculo_activo" : "";

                str.Append("<tr>");
                str.Append("<td style='width: 120px'>");
                str.AppendFormat("<a class='' href='JavaScript:verRecalculo({0});' style='margin-right: 4px;'><img style='padding-left: 40px; margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-open.png' alt='Ver recálculo' title='Ver recálculo' /></a>", regRecalculo.Pfrecacodi, url);
                if (tienePermisoEditar)
                    str.AppendFormat("<a class='' href='JavaScript:editarRecalculo({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-edit.png' alt='Editar recálculo' title='Editar recálculo' /></a>", regRecalculo.Pfrecacodi, url);

                //Puede generar calculos de PF hasta que sea Cerrado
                if (regRecalculo.Estado != "C")
                    str.AppendFormat("<a class='' href='JavaScript:ObtenerInsumosParaCalculoPF({0},{1});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{2}Content/Images/settings.png' alt='Calcular Potencia Firme' title='Calcular Potencia Firme' /></a>", pfPericodi, regRecalculo.Pfrecacodi, url);

                //Muestra la lista de excel cuando tenga como minimo un reportecodi
                if (reporte != null)
                    str.AppendFormat("<a class='' href='JavaScript:verListadoReporte({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/excel.png' alt='Ver listado reporte' title='Ver listado reporte' /></a>", regRecalculo.Pfrecacodi, url);


                str.Append("</td>");
                str.AppendFormat("<td class='{1}' style='width: 150px; text-align: center'>{0}</td>", regRecalculo.PfrecaestadoDesc, claseRec);
                str.AppendFormat("<td class='' style='width: 50px; text-align: center'>{0}</td>", regRecalculo.Pfrecatipo);
                str.AppendFormat("<td class='' style='width: 150px; text-align: center'>{0}</td>", regRecalculo.Pfrecanombre);
                str.AppendFormat("<td class='' style='width: 460px'>{0}</td>", regRecalculo.Pfrecadescripcion);
                str.AppendFormat("<td class='' style='width: 460px'>{0}</td>", regRecalculo.Pfrecainforme);
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
        /// Da formato a los estados (ejm: "A" -> "Abierto") y fechas
        /// </summary>
        /// <param name="reg"></param>
        public static void FormatearPfRecalculo(PfRecalculoDTO reg)
        {
            if (reg != null)
            {
                reg.UltimaModificacionFechaDesc = reg.Pfrecafecmodificacion != null ? reg.Pfrecafecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : reg.Pfrecafeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                reg.UltimaModificacionUsuarioDesc = reg.Pfrecafecmodificacion != null ? reg.Pfrecausumodificacion : reg.Pfrecausucreacion;
                reg.PfrecafechalimiteDesc = reg.Pfrecafechalimite.ToString(ConstantesAppServicio.FormatoFechaFull);

                reg.Estado = DateTime.Now < reg.Pfrecafechalimite ? ConstantesIndisponibilidades.EstadoPeriodoAbierto : ConstantesIndisponibilidades.EstadoPeriodoCerrado;
                reg.PfrecaestadoDesc = reg.Estado == ConstantesPotenciaFirme.Abierto ? "Abierto" : "Cerrado";

                reg.FechaIni = new DateTime(reg.Pfperianio, reg.Pfperimes, 1);
                reg.FechaFin = reg.FechaIni.AddMonths(1).AddDays(-1);
            }
        }

        /// <summary>
        /// Verifica existencia y crea un periodo mensual, automaticamente
        /// </summary>
        public void CrearIndPeriodoAutomatico()
        {
            List<PfPeriodoDTO> listaPeriodo = new List<PfPeriodoDTO>();

            DateTime fechaPeriodoActual = GetPeriodoActual();

            //generar data historica
            DateTime fechaIni = GetPeriodoActual().AddYears(-3);
            do
            {
                int anio = fechaIni.Year;
                int mes = fechaIni.Month;

                PfPeriodoDTO reg = new PfPeriodoDTO();
                reg.Pfperianio = anio;
                reg.Pfperimes = mes;
                reg.Pfperinombre = anio + "." + EPDate.f_NombreMes(mes);
                reg.Pfperianiomes = Convert.ToInt32(anio.ToString() + mes.ToString("D2"));
                reg.Pfperiusucreacion = "SISTEMA";
                reg.Pfperifeccreacion = DateTime.Now;

                listaPeriodo.Add(reg);

                fechaIni = fechaIni.AddMonths(1);

            } while (fechaIni <= fechaPeriodoActual);

            //verificar la existencia
            List<PfPeriodoDTO> listaPeriodoBD = this.GetByCriteriaPfPeriodos(-1);
            List<PfPeriodoDTO> listaNuevo = new List<PfPeriodoDTO>();
            foreach (var reg in listaPeriodo)
            {
                var regBD = listaPeriodoBD.Find(x => x.Pfperianiomes == reg.Pfperianiomes);
                if (regBD == null)
                {
                    listaNuevo.Add(reg);
                }
            }

            //guardar en bd
            listaNuevo.ForEach(reg => SavePfPeriodo(reg));
        }

        public void GuardarRecalculo(PfRecalculoDTO regRecalculo, string usuario)
        {
            if (regRecalculo.Pfrecatipo == "M")
            {
                //guardar recálculo nuevo 
                regRecalculo.Pfrecausucreacion = usuario;
                regRecalculo.Pfrecafeccreacion = DateTime.Now;
                regRecalculo.Pfrecacodi = this.SavePfRecalculo(regRecalculo);

                var lstContratosCV = new List<PfContratosDTO>();
                this.GuardarContratosCV(lstContratosCV, usuario, regRecalculo.Pfrecacodi);
            }
            else
                GuardarRecalculoN(regRecalculo, usuario);
        }

        public void GuardarRecalculoN(PfRecalculoDTO regRecalculo, string usuario)
        {
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            //Obtener el último recálculo
            List<PfRecalculoDTO> listRecalculoAnterior = this.ListPfRecalculos().Where(x => x.Pfpericodi == regPeriodo.Pfpericodi).OrderByDescending(x => x.Pfrecacodi).ToList();
            int idRecalculoAnterior = listRecalculoAnterior.Count == 0 ? ConstantesPotenciaFirme.ParametroDefecto : listRecalculoAnterior.First().Pfrecacodi;

            if (idRecalculoAnterior > 0)
            {
                int idRptAnterior = GetUltimoPfrptcodiXRecalculo(idRecalculoAnterior, ConstantesPotenciaFirme.CuadroPFirme);
                List<PfRelacionInsumosDTO> listaPfversOld = this.GetByCriteriaPfRelacionInsumoss(idRptAnterior);
                List<PfRelacionIndisponibilidadesDTO> listaIrptOld = this.GetByCriteriaPfRelacionIndisponibilidadess(idRptAnterior);

                if (listaPfversOld.Any() || listaIrptOld.Any())
                {

                    //guardar recálculo nuevo 
                    regRecalculo.Pfrecausucreacion = usuario;
                    regRecalculo.Pfrecafeccreacion = DateTime.Now;
                    regRecalculo.Pfrecacodi = this.SavePfRecalculo(regRecalculo);
                    int recalculoId = regRecalculo.Pfrecacodi;

                    int emprcodi = ConstantesPotenciaFirme.ParametroDefecto;
                    int centralId = ConstantesPotenciaFirme.ParametroDefecto;

                    int versionOldPg = listaPfversOld.Find(x => x.Pfrecucodi == ConstantesPotenciaFirme.RecursoPGarantizada).Pfverscodi;
                    int versionOldFi = listaPfversOld.Find(x => x.Pfrecucodi == ConstantesPotenciaFirme.RecursoFactorIndispFortuita).Pfverscodi;
                    int versionOldFp = listaPfversOld.Find(x => x.Pfrecucodi == ConstantesPotenciaFirme.RecursoFactorPresencia).Pfverscodi;
                    int versionOldPa = listaPfversOld.Find(x => x.Pfrecucodi == ConstantesPotenciaFirme.RecursoPAdicional).Pfverscodi;
                    int versionOldCv = listaPfversOld.Find(x => x.Pfrecucodi == ConstantesPotenciaFirme.RecursoContratosCV).Pfverscodi;

                    int rptcodiFi = listaIrptOld.Find(x => x.Icuacodi == ConstantesIndisponibilidades.ReportePR25FactorFortTermico).Irptcodi;
                    int rptcodiFp = listaIrptOld.Find(x => x.Icuacodi == ConstantesIndisponibilidades.ReportePR25FactorPresencia).Irptcodi;

                    this.ListarPotenciaGarantizada(idRecalculoAnterior, versionOldPg, emprcodi, centralId, out List<PfPotenciaGarantizadaDTO> lstPGarantizadas, out PfVersionDTO pfVersionRecurso1);
                    this.GuardarPotenciaGarantizada(lstPGarantizadas, usuario, recalculoId);

                    this.ListarFactores(ConstantesPotenciaFirme.FactorIndispFortuita, idRecalculoAnterior, versionOldFi, rptcodiFi, emprcodi, centralId, out List<PfFactoresDTO> lstFactorIndisp, out PfVersionDTO pfVersionRecurso2);
                    this.GuardarFactor(ConstantesPotenciaFirme.FactorIndispFortuita, lstFactorIndisp, usuario, recalculoId, rptcodiFi);

                    this.ListarFactores(ConstantesPotenciaFirme.FactorPresencia, idRecalculoAnterior, versionOldFp, rptcodiFp, emprcodi, centralId, out List<PfFactoresDTO> lstFactorPresencia, out PfVersionDTO pfVersionRecurso3);
                    this.GuardarFactor(ConstantesPotenciaFirme.FactorPresencia, lstFactorPresencia, usuario, recalculoId, rptcodiFp);

                    //para generar adicional es necesario que primero de guarda Fortuita
                    this.ListarPotenciaAdicional(idRecalculoAnterior, versionOldPa, rptcodiFi, emprcodi, centralId, out List<PfPotenciaAdicionalDTO> lstPAdicional, out PfVersionDTO pfVersionRecurso4);
                    this.GuardarPotenciaAdicional(lstPAdicional, usuario, recalculoId, rptcodiFi);

                    this.ListarContratosCV(idRecalculoAnterior, versionOldCv, out List<PfContratosDTO> lstContratosCV, out PfVersionDTO pfVersionRecurso6);
                    this.GuardarContratosCV(lstContratosCV, usuario, recalculoId);
                }
                else
                {
                    GuardarRecalculoMensual(regRecalculo, usuario);
                }
            }
            else
            {
                GuardarRecalculoMensual(regRecalculo, usuario);
            }
        }

        public void GuardarRecalculoMensual(PfRecalculoDTO regRecalculo, string usuario)
        {
            //guardar recálculo nuevo 
            regRecalculo.Pfrecausucreacion = usuario;
            regRecalculo.Pfrecafeccreacion = DateTime.Now;
            regRecalculo.Pfrecacodi = this.SavePfRecalculo(regRecalculo);
            int recalculoId = regRecalculo.Pfrecacodi;

            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            //obtener listado de insumos
            servIndisp.GetCodigoReporteAprobadoXCuadro(regRecalculo.Irecacodi, ConstantesIndisponibilidades.ReportePR25FactorFortTermico, out int rptcodi8, out int numVersion8, out string mensaje8);
            servIndisp.GetCodigoReporteAprobadoXCuadro(regRecalculo.Irecacodi, ConstantesIndisponibilidades.ReportePR25FactorPresencia, out int rptcodi11, out int numVersion11, out string mensaje11);

            int emprcodi = ConstantesPotenciaFirme.ParametroDefecto;
            int centralId = ConstantesPotenciaFirme.ParametroDefecto;

            int versionId = ConstantesPotenciaFirme.ParametroTraerBD;

            this.ListarPotenciaGarantizada(recalculoId, versionId, emprcodi, centralId, out List<PfPotenciaGarantizadaDTO> lstPGarantizadas, out PfVersionDTO pfVersionRecurso1);
            this.GuardarPotenciaGarantizada(lstPGarantizadas, usuario, recalculoId);

            this.ListarFactores(ConstantesPotenciaFirme.FactorIndispFortuita, recalculoId, versionId, rptcodi8, emprcodi, centralId, out List<PfFactoresDTO> lstFactorIndisp, out PfVersionDTO pfVersionRecurso2);
            this.GuardarFactor(ConstantesPotenciaFirme.FactorIndispFortuita, lstFactorIndisp, usuario, recalculoId, rptcodi8);

            this.ListarFactores(ConstantesPotenciaFirme.FactorPresencia, recalculoId, versionId, rptcodi11, emprcodi, centralId, out List<PfFactoresDTO> lstFactorPresencia, out PfVersionDTO pfVersionRecurso3);
            this.GuardarFactor(ConstantesPotenciaFirme.FactorPresencia, lstFactorPresencia, usuario, recalculoId, rptcodi11);

            //para generar adicional es necesario que primero de guarda Fortuita
            this.ListarPotenciaAdicional(recalculoId, versionId, rptcodi8, emprcodi, centralId, out List<PfPotenciaAdicionalDTO> lstPAdicional, out PfVersionDTO pfVersionRecurso4);
            this.GuardarPotenciaAdicional(lstPAdicional, usuario, recalculoId, rptcodi8);

            var lstContratosCV = new List<PfContratosDTO>();
            this.GuardarContratosCV(lstContratosCV, usuario, recalculoId);
        }

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

        public string ValidarNombreRepetido(PfRecalculoDTO recalculo)
        {
            string mensaje = "";
            var listaRecalculo = this.ListPfRecalculos().Where(x => x.Pfpericodi == recalculo.Pfpericodi).ToList();
            var regExist = listaRecalculo.Find(x => (x.Pfrecatipo ?? "").Trim().ToUpper() == (recalculo.Pfrecatipo ?? "").Trim().ToUpper());
            var regExist2 = listaRecalculo.Find(x => x.Pfrecanombre.Trim().ToUpper() == recalculo.Pfrecanombre.Trim().ToUpper());

            if (regExist != null || regExist2 != null)
                mensaje = "Ya existe el Recálculo";

            return mensaje;
        }

        /// <summary>
        /// Validar que el recalculo de Indisponibilidades tenga los factores validados y aprobados
        /// </summary>
        /// <param name="irecacodi"></param>
        /// <returns></returns>
        public string ValidarRecalculoIndisponibilidades(int irecacodi)
        {
            string msj = "";
            if (irecacodi <= 0)
                msj = "Debe seleccionar un recálculo de Indisponibilidades";
            else
            {
                servIndisp.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25FactorFortTermico, out int rptcodi8, out int numVersion8, out string mensaje1);
                servIndisp.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25FactorProgTermico, out int rptcodi9, out int numVersion9, out string mensaje2);
                servIndisp.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25FactorProgHidro, out int rptcodi10, out int numVersion10, out string mensaje3);
                servIndisp.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25FactorPresencia, out int rptcodi11p, out int numVersion11p, out string mensaje4);
                servIndisp.GetCodigoReporteAprobadoXCuadro(irecacodi, ConstantesIndisponibilidades.ReportePR25DisponibilidadCalorUtil, out int rptcodi14p, out int numVersion14p, out string mensaje14);

                if (!string.IsNullOrEmpty(mensaje1)) msj += mensaje1 + "\n";
                if (!string.IsNullOrEmpty(mensaje2)) msj += mensaje2 + "\n";
                if (!string.IsNullOrEmpty(mensaje3)) msj += mensaje3 + "\n";
                if (!string.IsNullOrEmpty(mensaje4)) msj += mensaje4 + "\n";
                if (!string.IsNullOrEmpty(mensaje14)) msj += mensaje14 + "\n";

                if (!string.IsNullOrEmpty(msj))
                    msj = "Recálculo de Indisponibilidades: " + "\n" + msj;
            }

            return msj;
        }

        #endregion

        #region POTENCIA FIRME DE CENTRALES RER EÓLICAS Y SOLARES

        /// <summary>
        /// Obtener Reporte Total PF Rer
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public void ObtenerReporteTotalPFRer(DateTime fechaIni, DateTime fechaFin, out List<PfReporteTotalDTO> listaPFRepTot, out List<PfReporteDetDTO> listaPFRepDet)
        {
            listaPFRepTot = new List<PfReporteTotalDTO>();
            listaPFRepDet = new List<PfReporteDetDTO>();

            servIndisp.ListarEqCentralSolarOpComercial(fechaIni, fechaFin, out List<EqEquipoDTO> listaCentrales1, out List<EqEquipoDTO> istaAllEquipos1, out List<ResultadoValidacionAplicativo> istaMsj1);
            servIndisp.ListarEqCentralEolicaOpComercial(fechaIni, fechaFin, out List<EqEquipoDTO> listaCentrales2, out List<EqEquipoDTO> istaAllEquipos2, out List<ResultadoValidacionAplicativo> istaMsj2);
            List<EqEquipoDTO> listaCentralRER = new List<EqEquipoDTO>();
            listaCentralRER.AddRange(listaCentrales1);
            listaCentralRER.AddRange(listaCentrales2);

            List<PfReporteTotalDTO> listaEnerHpOC = ListarDataRerHistorico(fechaFin);

            //Hora punta segun fecha de vigencia
            List<SiParametroValorDTO> listaBloqueHorario = (new ParametroAppServicio()).ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            ListarTiempoRer(fechaIni, fechaFin, listaBloqueHorario, out List<PfReporteDetDTO> listaAnio, out List<PfReporteDetDTO> listaMes, out List<PfReporteDetDTO> listAllMes, out PfReporteDetDTO regEstadistico36Meses);

            int idTotal = 1;
            foreach (var regOpCom in listaCentralRER)
            {
                List<PfReporteTotalDTO> listaEnerHpOCxCentral = listaEnerHpOC.Where(x => x.Equipadre == regOpCom.Equipadre).ToList();

                PfReporteTotalDTO regTotal = new PfReporteTotalDTO()
                {
                    Pftotcodi = idTotal,
                    Emprcodi = regOpCom.Emprcodi ?? 0,
                    Emprnomb = regOpCom.Emprnomb,
                    Equipadre = regOpCom.Equipadre ?? 0,
                    Central = regOpCom.Central,
                    Equicodi = regOpCom.Equipadre ?? 0,
                    Famcodi = regOpCom.Famcodi ?? 0,
                    Pftotunidadnomb = regOpCom.Central,
                    ListaDetalle = new List<PfReporteDetDTO>()
                };

                regTotal.Equifechiniopcom = regOpCom.Equifechiniopcom;
                regTotal.EquifechiniopcomDesc = regOpCom.Equifechiniopcom != null ? regOpCom.Equifechiniopcom.Value.ToString(ConstantesAppServicio.FormatoFecha) : "";
                regTotal.Tiene36Meses = fechaIni.AddMonths(-36) >= regOpCom.Equifechiniopcom;
                if (!regTotal.Tiene36Meses) regTotal.Central += " (*)";

                //
                CalcularPotenciaFirmerRer(listaEnerHpOCxCentral.Where(x => x.FechaIni >= fechaIni.AddMonths(-35) && x.FechaIni <= fechaFin).ToList(), out decimal valorMovil36m, out decimal valorPF);
                regTotal.Pftotpf = valorPF;
                regTotal.Pftotenergia = valorMovil36m;

                foreach (var anio in listaAnio)
                {
                    decimal? valorEnergiaXCentralxAnio = listaEnerHpOCxCentral.Where(x => x.FechaIni >= anio.Pfdetfechaini && x.FechaIni <= anio.Pfdetfechafin).Sum(x => x.Pftotenergia);

                    //Enero - Diciembre 2018
                    PfReporteDetDTO regDet = new PfReporteDetDTO()
                    {
                        Equipadre = regOpCom.Equipadre ?? 0,
                        Pfdettipo = ConstantesPotenciaFirme.TipoDetAnual,
                        Pfdetenergia = valorEnergiaXCentralxAnio,
                        Pfdetfechaini = anio.Pfdetfechaini,
                        Pfdetfechafin = anio.Pfdetfechafin,
                        Pftotcodi = idTotal
                    };
                    regTotal.ListaDetalle.Add(regDet);
                }

                foreach (var fechaMes in listAllMes)
                {
                    var lista = listaEnerHpOCxCentral.Where(x => x.FechaIni.Month == fechaMes.Pfdetfechaini.Value.Month && x.FechaIni.Year == fechaMes.Pfdetfechaini.Value.Year).ToList();

                    if (lista.Any())
                    {
                        decimal? valorEnergiaXCentralxMes = lista?.Sum(x => x.Pftotenergia ?? 0);
                        int numdiapoc = lista.Sum(x => x.Pftotnumdiapoc);

                        PfReporteDetDTO regDetMes = new PfReporteDetDTO()
                        {
                            Equipadre = regOpCom.Equipadre ?? 0,
                            Pfdettipo = ConstantesPotenciaFirme.TipoDetMensual,
                            Pfdetenergia = valorEnergiaXCentralxMes,
                            Pfdetfechaini = fechaMes.Pfdetfechaini,
                            Pfdetfechafin = fechaMes.Pfdetfechafin,
                            Pfdetnumdiapoc = numdiapoc,
                            Pftotcodi = idTotal
                        };
                        regTotal.ListaDetalle.Add(regDetMes);
                    }
                }

                listaPFRepDet.AddRange(regTotal.ListaDetalle);
                listaPFRepTot.Add(regTotal);

                idTotal++;
            }

            listaPFRepTot = listaPFRepTot.OrderBy(x => x.Equifechiniopcom).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ToList();
        }

        private List<PfReporteTotalDTO> ListarDataRerHistorico(DateTime fechaFin)
        {
            DateTime fechaIni = DateTime.ParseExact(ConstantesPotenciaFirme.MesIniHistoricoRER, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

            //Obtener la lista de rptcodi
            List<PfRecalculoDTO> listaRecalculo = this.ListPfRecalculos().Where(x => x.Pfrecatipo == "M"
                                                    && x.FechaIni >= fechaIni && x.FechaIni <= fechaFin).ToList();
            //Obtener la lista de rptcodi
            List<PfReporteDTO> listaRptValidado = ListarPfReporteMensualValidado(ConstantesPotenciaFirme.CuadroRerHistorico, listaRecalculo);

            List<PfReporteTotalDTO> listaDetDiarioHist = GetByCriteriaPfReporteTotals(string.Join(",", listaRptValidado.Select(x => x.Pfrptcodi)));

            return listaDetDiarioHist;
        }

        public List<PfReporteDTO> ListarPfReporteMensualValidado(int cuadro, List<PfRecalculoDTO> listaRecalculo)
        {
            List<PfReporteDTO> listaAllRpt = ListPfReportes().Where(x => x.Pfcuacodi == cuadro).ToList();

            List<PfReporteDTO> listaRptValidado = new List<PfReporteDTO>();
            foreach (var reg in listaRecalculo)
            {
                var regRpt = listaAllRpt.Where(x => x.Pfrecacodi == reg.Pfrecacodi).OrderByDescending(x => x.Pfrptcodi).FirstOrDefault();
                if (regRpt != null)
                {
                    listaRptValidado.Add(regRpt);
                }
            }

            return listaRptValidado;
        }

        private void ListarTiempoRer(DateTime fechaIni, DateTime fechaFin, List<SiParametroValorDTO> listaBloqueHorario, out List<PfReporteDetDTO> listaAnio, out List<PfReporteDetDTO> listaMes, out List<PfReporteDetDTO> listaAllMes, out PfReporteDetDTO regEstadistico36Meses)
        {
            //36 meses, separados por 12 meses
            listaAnio = new List<PfReporteDetDTO>();

            var fechaInicio36 = fechaIni.AddMonths(-35);
            int totalMeses = 0;
            DateTime fecha1 = fechaInicio36;
            do
            {
                DateTime fecha2 = new DateTime(fecha1.Year, 12, 1);
                int numMeses = fecha2.Month - fecha1.Month + 1;
                totalMeses += numMeses;
                if (totalMeses > 36)
                {
                    fecha2 = new DateTime(fechaFin.Year, fechaFin.Month, 1);//primero del mes
                    numMeses = fecha2.Month - fecha1.Month + 1;
                }

                DateTime fechaMax = fecha2.AddMonths(1).AddDays(-1);
                string cabeceraPeriodo = $"{EPDate.f_NombreMes(fechaMax.Month)} {fechaMax.Year}";
                if ((fechaMax - fecha1).TotalDays > fechaMax.Day)
                    cabeceraPeriodo = $"{EPDate.f_NombreMes(fecha1.Month)} - {cabeceraPeriodo}";

                var reg = new PfReporteDetDTO()
                {
                    Pfdetfechaini = fecha1,
                    Pfdetfechafin = fechaMax,
                    FechaDesc = cabeceraPeriodo,
                };
                reg.TotalDias = (reg.Pfdetfechafin.Value - reg.Pfdetfechaini.Value).Days + 1;
                reg.TotalHP = MedidoresHelper.TotalHorasXRango(reg.Pfdetfechaini.Value, reg.Pfdetfechafin.Value, listaBloqueHorario);
                reg.TotalMes = numMeses;

                listaAnio.Add(reg);

                fecha1 = fecha2.AddMonths(1);
            } while (fecha1 <= fechaFin);

            //
            listaMes = new List<PfReporteDetDTO>();
            foreach (var anio in listaAnio)
            {
                for (DateTime fechaMes = anio.Pfdetfechaini.Value; fechaMes <= anio.Pfdetfechafin; fechaMes = fechaMes.AddMonths(1))
                {
                    listaMes.Add(new PfReporteDetDTO()
                    {
                        Pfdetfechaini = fechaMes,
                        Pfdetfechafin = fechaMes.AddMonths(1).AddDays(-1),
                    });
                }
            }

            //
            listaAllMes = new List<PfReporteDetDTO>();

            for (DateTime fechaMes = new DateTime(2016, 11, 1); fechaMes <= fechaFin; fechaMes = fechaMes.AddMonths(1))
            {
                listaAllMes.Add(new PfReporteDetDTO()
                {
                    Pfdetfechaini = fechaMes,
                    Pfdetfechafin = fechaMes.AddMonths(1).AddDays(-1),
                });
            }

            regEstadistico36Meses = new PfReporteDetDTO()
            {
                TotalMes = 36,
                TotalDias = listaAnio.Sum(x => x.TotalDias),
                TotalHP = listaAnio.Sum(x => x.TotalHP),
            };
        }

        /// <summary>
        /// Calcula la potencia firme centrales rer eólicas y solares
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private void CalcularPotenciaFirmerRer(List<PfReporteTotalDTO> lista, out decimal valorMovil36m, out decimal valorPF)
        {
            valorMovil36m = 0;
            valorPF = 0;

            if (lista.Any())
            {
                var diasMovil36m = lista.Sum(x => x.Pftotnumdiapoc);
                var horasPuntaMovil = Convert.ToDecimal(diasMovil36m * 6.0);

                valorMovil36m = lista.Sum(x => x.Pftotenergia ?? 0);

                valorPF = valorMovil36m / horasPuntaMovil;
            }
        }

        #region Procesar RER

        /// <summary>
        /// Generar data histórica
        /// </summary>
        public void EjecutarHistoricoRER()
        {
            DateTime fechaIni = DateTime.ParseExact(ConstantesPotenciaFirme.MesIniHistoricoRER, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(ConstantesPotenciaFirme.MesFinHistoricoRER, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

            for (var mes = fechaIni; mes <= fechaFin; mes = mes.AddMonths(1))
            {
                ProcesarRERMensual(mes);
            }
        }

        private void ProcesarRERMensual(DateTime fechaPeriodo)
        {
            DateTime fechaRegistro = DateTime.Now;

            #region Periodo

            int anio = fechaPeriodo.Year;
            int mes = fechaPeriodo.Month;

            PfPeriodoDTO regPeriodo = new PfPeriodoDTO();
            regPeriodo.Pfperianio = anio;
            regPeriodo.Pfperimes = mes;
            regPeriodo.Pfperinombre = anio + "." + EPDate.f_NombreMes(mes);
            regPeriodo.Pfperianiomes = Convert.ToInt32(anio.ToString() + mes.ToString("D2"));
            regPeriodo.Pfperiusucreacion = "SISTEMA";
            regPeriodo.Pfperifeccreacion = fechaRegistro;

            //verificar la existencia
            List<PfPeriodoDTO> listaPeriodoBD = this.GetByCriteriaPfPeriodos(-1);
            var regBD = listaPeriodoBD.Find(x => x.Pfperianiomes == regPeriodo.Pfperianiomes);
            if (regBD == null)
            {
                regPeriodo.Pfpericodi = SavePfPeriodo(regPeriodo);
            }
            else
            {
                regPeriodo = regBD;
            }

            regPeriodo.FechaIni = new DateTime(regPeriodo.Pfperianio, regPeriodo.Pfperimes, 1);
            regPeriodo.FechaFin = regPeriodo.FechaIni.AddMonths(1).AddDays(-1);

            #endregion

            #region Recálculo

            var listaRecalculo = GetByCriteriaPfRecalculos(regPeriodo.Pfpericodi);
            PfRecalculoDTO regRecalculo = listaRecalculo.Find(x => x.Pfrecatipo == "M");

            if (regRecalculo == null)
            {
                regRecalculo = new PfRecalculoDTO();
                regRecalculo.Pfpericodi = regPeriodo.Pfpericodi;
                regRecalculo.Pfrecadescripcion = string.Empty;
                regRecalculo.Pfrecafechalimite = regPeriodo.FechaFin.AddDays(3).AddHours(23).AddMinutes(59);
                regRecalculo.Pfrecainforme = "INFORME COES/D/DO/SME-INF-0XX-" + anio;
                regRecalculo.Pfrecatipo = "M";
                regRecalculo.Pfrecanombre = "Mensual";
                regRecalculo.Pfrecausucreacion = "SISTEMA";
                regRecalculo.Pfrecafeccreacion = fechaRegistro;

                regRecalculo.Pfrecacodi = SavePfRecalculo(regRecalculo);
            }

            #endregion

            #region Registrar información de RER NC

            DateTime fechaIni = regPeriodo.FechaIni;
            DateTime fechaFin = regPeriodo.FechaFin;

            //Obtener energia diaria de cada central
            ObtenerDataPFCentralesRERXDiaXCentral(fechaIni, fechaFin, out List<MeMedicion96DTO> lista96CentralDetalle, out List<EqEquipoDTO> listaEq);

            List<PfReporteTotalDTO> listaPFRepTot = new List<PfReporteTotalDTO>();
            int idTotal = 1;
            foreach (var listaEnerHpOCxCentral in lista96CentralDetalle.GroupBy(x => x.Equipadre))
            {
                var enerHpOC = listaEnerHpOCxCentral.First();

                var lista = listaEnerHpOCxCentral.Where(x => x.Medifecha.Value.Month == fechaIni.Month && x.Medifecha.Value.Year == fechaIni.Year).ToList();

                if (lista.Any())
                {
                    DateTime fechaImes = lista.Min(x => x.Medifecha).Value;
                    DateTime fechaFmes = lista.Max(x => x.Medifecha).Value;

                    decimal? valorEnergiaXCentralxMes = lista?.Sum(x => x.Total);
                    int numdiapoc = (fechaFmes - fechaImes).Days + 1;

                    PfReporteTotalDTO regTotal = new PfReporteTotalDTO()
                    {
                        Emprcodi = enerHpOC.Emprcodi,
                        Emprnomb = enerHpOC.Emprnomb,
                        Famcodi = enerHpOC.Famcodi,
                        Equipadre = enerHpOC.Equipadre,
                        Central = enerHpOC.Central,
                        Equicodi = enerHpOC.Equipadre,
                        Pftotenergia = valorEnergiaXCentralxMes,
                        Pftotnumdiapoc = numdiapoc,
                        Pftotunidadnomb = enerHpOC.Central,
                        ListaDetalle = new List<PfReporteDetDTO>(),
                        Pftotcodi = idTotal
                    };
                    listaPFRepTot.Add(regTotal);

                    idTotal++;
                }
            }

            listaPFRepTot = listaPFRepTot.OrderBy(x => x.Equifechiniopcom).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ToList();

            PfEscenarioDTO objEscenarioRer = new PfEscenarioDTO()
            {
                Pfescefecini = fechaIni,
                Pfescefecfin = fechaFin,
                Pfescedescripcion = GetDescripcionEscenario(fechaIni, fechaFin),
            };
            objEscenarioRer.ListaPfReporteTotal = listaPFRepTot;

            PfReporteDTO regReporteNC = new PfReporteDTO()
            {
                Pfcuacodi = ConstantesPotenciaFirme.CuadroRerHistorico,
                Pfrecacodi = regRecalculo.Pfrecacodi,
                Pfrptesfinal = ConstantesPotenciaFirme.EsVersionValidado,
                Pfrptusucreacion = "SISTEMA",
                Pfrptfeccreacion = fechaRegistro,
            };

            regReporteNC.ListaPfEscenario = new List<PfEscenarioDTO>() { objEscenarioRer };

            if (EsVersionCreableRerHist(regRecalculo.Pfrecacodi, listaPFRepTot))
            {
                //Funcion transaccional para guardar en BD
                GuardarReportePotFirmeBDTransaccional(regReporteNC, new List<int>(), new List<int>());
            }
            #endregion
        }

        public bool EsVersionCreableRerHist(int pfrecacodi, List<PfReporteTotalDTO> listaRptTotMemoria)
        {
            int id = GetUltimoPfrptcodiXRecalculo(pfrecacodi, ConstantesPotenciaFirme.CuadroRerHistorico);

            if (id > 0)
            {
                List<PfReporteTotalDTO> listaRptTotBD = GetByCriteriaPfReporteTotals(id.ToString());

                //comparar bd con memory
                var regCambio1 = ExisteDiferenciaListaDataTot(1, listaRptTotBD, listaRptTotMemoria);
                if (regCambio1.TieneCambio)
                    return true;

                //comparar memory con bd
                var regCambio2 = ExisteDiferenciaListaDataTot(2, listaRptTotMemoria, listaRptTotBD);
                if (regCambio2.TieneCambio)
                    return true;

                return false;
            }
            else
            {
                return true;//no existe bd
            }
        }

        private RegistroCambioPR25 ExisteDiferenciaListaDataTot(int tipoComparacion, List<PfReporteTotalDTO> listaRptTotBD, List<PfReporteTotalDTO> listaRptTotMemoria)
        {
            foreach (var reg1 in listaRptTotBD)
            {
                #region

                var reg2 = listaRptTotMemoria.Find(x => x.Equicodi == reg1.Equicodi);
                if (reg2 != null)
                {
                    decimal valor1 = Math.Round(reg2.Pftotenergia.GetValueOrDefault(0), 10);
                    decimal valor2 = Math.Round(reg1.Pftotenergia.GetValueOrDefault(0), 10);
                    if (valor1 != valor2)
                        return new RegistroCambioPR25() { TieneCambio = true, TipoComparacion = tipoComparacion, RegPfTot1 = reg1, RegPfTot2 = reg2, Campo = "Pftotenergia" };

                    int valor3 = reg2.Pftotnumdiapoc;
                    int valor4 = reg1.Pftotnumdiapoc;
                    if (valor3 != valor4)
                        return new RegistroCambioPR25() { TieneCambio = true, TipoComparacion = tipoComparacion, RegPfTot1 = reg1, RegPfTot2 = reg2, Campo = "Pftotnumdiapoc" };

                    string sValor1 = (reg2.Pftotunidadnomb ?? "").Trim().ToUpper();
                    string sValor2 = (reg1.Pftotunidadnomb ?? "").Trim().ToUpper();
                    if (sValor1 != sValor2)
                        return new RegistroCambioPR25() { TieneCambio = true, TipoComparacion = tipoComparacion, RegPfTot1 = reg1, RegPfTot2 = reg2, Campo = "Pftotunidadnomb" };
                }
                else
                {
                    return new RegistroCambioPR25() { TieneCambio = true, TipoComparacion = tipoComparacion, RegPfTot1 = reg1, RegPfTot2 = reg2 };
                }

                #endregion

            }

            return new RegistroCambioPR25() { TieneCambio = false, };
        }


        /// <summary>
        /// Obtiene data de Potencia firme de centrales rer eólicas y solares
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="meses"></param>
        /// <returns></returns>
        private void ObtenerDataPFCentralesRERXDiaXCentral(DateTime fechaIni, DateTime fechaFin, out List<MeMedicion96DTO> lista96Detalle, out List<EqEquipoDTO> listaEq)
        {
            var fuentesEnergia = $"{ConstantesPR5ReportesServicio.FenergcodiSolar},{ConstantesPR5ReportesServicio.FenergcodiEolica}";

            List<MeMedicion96DTO> listaProdEner = _medidoresAppServicio.
                ListaDataMDGeneracionConsolidado(fechaIni, fechaFin, ConstantesMedicion.IdTipogrupoCOES, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstanteValidacion.EstadoTodos, fuentesEnergia, false);

            //Hora punta segun fecha de vigencia
            List<SiParametroValorDTO> listaBloqueHorario = (new ParametroAppServicio()).ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            //se aplica TTIE por el caso C.E. MARCONA que tiene dos equipadres distintos 
            //set TTIE
            TitularidadAppServicio servTitEmp = new TitularidadAppServicio();
            //Consulta el histórico de relación entre los equipos y las empresas
            List<SiHisempeqDataDTO> listaHist = servTitEmp.ListSiHisempeqDatas("-1").Where(x => x.Heqdatfecha <= DateTime.Today).ToList();

            //Titularidad de Instalaciones de Empresas, para evitar duplicados
            servTitEmp.SetTTIEequipoCentralToM96(listaProdEner, listaHist);

            //Centrales solares y eolicas que tiene operación comercial durantes el mes
            var familia = $"{ConstantesHorasOperacion.IdGeneradorSolar},{ConstantesHorasOperacion.IdGeneradorEolica},{ConstantesHorasOperacion.IdTipoSolar},{ConstantesHorasOperacion.IdTipoEolica}";
            var listaEquiposOC = _equipamientoAppServicio.ListarEquiposTienenOpComercial(fechaIni, fechaFin, familia, out List<ResultadoValidacionAplicativo> listaMsjEq);

            //Operación comercial
            List<EqPropequiDTO> listaOperacionComercial = FactorySic.GetEqPropequiRepository().ListarValoresHistoricosPropiedadPorEquipo(-1, ConstantesAppServicio.PropiedadOperacionComercial.ToString())
                                                            .Where(x => x.Propequideleted == 0).OrderByDescending(x => x.Fechapropequi).ToList();

            listaOperacionComercial = listaOperacionComercial.Where(x => listaEquiposOC.Select(y => y.Equicodi).Contains(x.Equicodi)).ToList();

            //Convertir a Energia
            lista96Detalle = ObtenerProduccionEnerHoraPuntaOCxDia(fechaIni, fechaFin, listaProdEner, listaEquiposOC, listaOperacionComercial, listaBloqueHorario);

            //
            listaEq = listaEquiposOC.Where(x => ConstantesHorasOperacion.IdTipoSolar == x.Famcodi.GetValueOrDefault(0)
                                                                         || ConstantesHorasOperacion.IdTipoEolica == x.Famcodi.GetValueOrDefault(0)).ToList();
        }

        /// <summary>
        /// Obtener Energia total en Hora Punta y Operación Comercial
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaEnergiaHP"></param>
        /// <param name="listaEquiposRER"></param>
        /// <param name="listaOperacionComercial"></param>
        /// <param name="listaBloqueHorario"></param>
        /// <returns></returns>
        private List<MeMedicion96DTO> ObtenerProduccionEnerHoraPuntaOCxDia(DateTime fechaIni, DateTime fechaFin, List<MeMedicion96DTO> listaEnergiaHP
                                                            , List<EqEquipoDTO> listaEquiposRER, List<EqPropequiDTO> listaOperacionComercial, List<SiParametroValorDTO> listaBloqueHorario)
        {
            var listaRerNC = new List<MeMedicion96DTO>();

            List<EqEquipoDTO> listaCentral = listaEquiposRER.Where(x => ConstantesHorasOperacion.IdTipoSolar == x.Famcodi.GetValueOrDefault(0)
                                                                        || ConstantesHorasOperacion.IdTipoEolica == x.Famcodi.GetValueOrDefault(0)).ToList();

            foreach (var reg in listaCentral)
            {
                if (reg.Equipadre == 14407)
                { }

                List<int> listaEquicodiXCentral = listaEquiposRER.Where(x => x.Equipadre == reg.Equipadre && x.Equipadre > 0).Select(x => x.Equicodi).Distinct().ToList();
                List<MeMedicion96DTO> listaMWXCentral = listaEnergiaHP.Where(x => x.Equipadre == reg.Equipadre).ToList();

                for (DateTime day = fechaIni; day <= fechaFin; day = day.AddDays(1))
                {
                    //parámetros de hora punta vigente para la fecha de consulta
                    SiParametroValorDTO paramHPyHFP = ParametroAppServicio.GetParametroVigenteHPyHFPXResolucion(listaBloqueHorario, day, ParametrosFormato.ResolucionCuartoHora);

                    //obtener equipos con operación comercial
                    List<int> listaEquicodiOC = new List<int>();
                    foreach (var equicodi in listaEquicodiXCentral)
                    {
                        EquipamientoAppServicio.SetValorOperacionComercial(equicodi, day, day, listaOperacionComercial, out string opComercial, out DateTime? fechaInicio, out DateTime? fechaRetiro);
                        if (ConstantesAppServicio.SI == opComercial) listaEquicodiOC.Add(equicodi);
                    }

                    if (listaEquicodiOC.Any())
                    {
                        var reg96 = new MeMedicion96DTO()
                        {
                            Emprcodi = reg.Emprcodi.Value,
                            Emprnomb = reg.Emprnomb,
                            Equipadre = reg.Equipadre.Value,
                            Central = reg.Central,
                            Famcodi = INDAppServicio.GetFamcodiPadre(reg.Famcodi.Value),
                            FechaFila = reg.Equifechiniopcom.Value,
                            Medifecha = day,
                        };

                        var lista96XDiaXCentral = listaMWXCentral.Where(x => x.Medifecha == day && listaEquicodiOC.Contains(x.Equicodi)).ToList();

                        decimal total = 0;
                        for (int hx = paramHPyHFP.HIniHP; hx <= paramHPyHFP.HFinHP; hx++)
                        {
                            decimal valorHPunta = 0;
                            foreach (var reg96Eq in lista96XDiaXCentral)
                            {
                                var valorH = (decimal?)reg96Eq.GetType().GetProperty(ConstantesAppServicio.CaracterH + hx).GetValue(reg96Eq, null);
                                valorHPunta += valorH ?? 0;
                            }
                            total += valorHPunta;

                            reg96.GetType().GetProperty(ConstantesAppServicio.CaracterH + hx).SetValue(reg96, valorHPunta);
                        }
                        reg96.Total = total / 4;

                        #region Casos Especiales

                        #region Puesta en Marcha aplicativo Potencia Firme

                        //C.E. MARCONA
                        if (reg.Equipadre == 14160 && day == new DateTime(2017, 10, 1))
                        {
                            reg96.Total -= (10376.152595m - 5188.0762975m);
                        }
                        if (reg.Equipadre == 14160 && day == new DateTime(2017, 11, 1))
                        {
                            reg96.Total -= (7098.755265m - 3549.3776325m);
                        }
                        if (reg.Equipadre == 14160 && day == new DateTime(2017, 12, 1))
                        {
                            reg96.Total -= (6262.421765m - 3131.2108825m);
                        }

                        #endregion

                        #region RE-19007 Cálculo de Potencia Firme de las Centrales RER Huambos y Dunas

                        bool esOmitirHistorico = false;
                        //C.E. DUNA para diciembre 2020 debe ser 0
                        if (reg.Equipadre == 21298 && (new DateTime(2020, 12, 1) <= day && day <= new DateTime(2021, 1, 31)))
                        {
                            reg96.Total = 0;
                            esOmitirHistorico = true;
                        }
                        //C.E. HUAMBOS para diciembre 2020 debe ser 0
                        if (reg.Equipadre == 21117 && (new DateTime(2020, 12, 1) <= day && day <= new DateTime(2021, 1, 31)))
                        {
                            reg96.Total = 0;
                            esOmitirHistorico = true;
                        }

                        //C.E. DUNA para 06/05/2021 debe ser 1491.312875
                        if (reg.Equipadre == 21298 && day == new DateTime(2021, 5, 6)) //inicio de operacion comercial
                        {
                            reg96.Total += (1491.312875m - 1474.21277m); //se agrega el valor esperado - el valor del aplicativo
                        }
                        //C.E. HUAMBOS para 06/05/2021 debe ser 1066.116635m
                        if (reg.Equipadre == 21117 && day == new DateTime(2021, 5, 6)) //inicio de operacion comercial
                        {
                            reg96.Total += (1066.116635m - 1039.8677425m); //se agrega el valor esperado - el valor del aplicativo
                        }
                        #endregion

                        #region REQ 2023-001712 reconsideración que no actualizó el resultado de la Potencia Firme de las centrales renovables
                        //C.E. MARCONA
                        if (reg.Equipadre == 14160 && day == new DateTime(2023, 2, 1))
                        {
                            reg96.Total -= (2950.93266m - 2937.0039775m); //Valor actual BD medidores - Valor final manual aplicativo
                        }
                        //C.E. CUPISNIQUE
                        if (reg.Equipadre == 14407 && day == new DateTime(2023, 2, 1))
                        {
                            reg96.Total -= (5235.32036500001m - 5166.80125m); //Valor actual BD medidores - Valor final manual aplicativo
                        }
                        //C.E. TALARA
                        if (reg.Equipadre == 14426 && day == new DateTime(2023, 2, 1))
                        {
                            reg96.Total -= (684.3918325m - 673.1447m); //Valor actual BD medidores - Valor final manual aplicativo
                        }
                        //C.E. TRES HERMANAS
                        if (reg.Equipadre == 15160 && day == new DateTime(2023, 2, 1))
                        {
                            reg96.Total -= (8779.44522m - 8737.208755m); //Valor actual BD medidores - Valor final manual aplicativo
                        }
                        #endregion

                        #endregion

                        if (!esOmitirHistorico)
                            listaRerNC.Add(reg96);
                    }
                }
            }

            return listaRerNC;
        }

        #endregion

        #region Reporte Excel


        /// <summary>
        /// Generar archivo excel por versión reporte plantilla térmico
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="irptcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nameFile"></param>
        public void GenerarArchivoExcelRER(string ruta, int tipo, int pfpericodi, out string nameFile)
        {
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(pfpericodi);
            PfCuadroDTO regCuadroRerNC = GetByIdPfCuadro(ConstantesPotenciaFirme.CuadroRerNC);
            nameFile = string.Empty;

            //Hora punta segun fecha de vigencia
            List<SiParametroValorDTO> listaBloqueHorario = (new ParametroAppServicio()).ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            if (tipo == 1) //Medidores
            {
                nameFile = string.Format("Medidores_RER_{0}_{1}.xlsx", regPeriodo.Pfperimes, regPeriodo.Pfperianio - 2000);
                string rutaFile = ruta + nameFile;

                this.ObtenerReporteTotalPFRer(regPeriodo.FechaIni, regPeriodo.FechaFin, out List<PfReporteTotalDTO> listaPFRerNC, out List<PfReporteDetDTO> listaRerDet);

                ObtenerDataPFCentralesRERXDiaXCentral(regPeriodo.FechaIni, regPeriodo.FechaFin, out List<MeMedicion96DTO> lista96Detalle, out List<EqEquipoDTO> listaEq);

                FileInfo newFile = new FileInfo(rutaFile);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(rutaFile);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    GenerarHojaExcelRerNC(xlPackage, "RER_NC", true, 1, 3
                                        , string.Empty, regCuadroRerNC.Pfcuanombre, regCuadroRerNC.Pfcuatitulo, regPeriodo.Pfperianio, regPeriodo.Pfperimes, regPeriodo.FechaIni, regPeriodo.FechaFin
                                        , listaPFRerNC, listaRerDet, listaBloqueHorario);
                    xlPackage.Save();

                    GeneraRptRegistroMedidores96(xlPackage, "Medidores", 3, 2, regPeriodo.FechaIni.AddMonths(-50), regPeriodo.FechaFin, listaPFRerNC, lista96Detalle, listaBloqueHorario);
                    xlPackage.Save();
                }
            }

            if (tipo == 2)
            {
                nameFile = string.Format("PF_RER_{0}_{1}.xlsx", regPeriodo.Pfperimes.ToString("D2"), regPeriodo.Pfperianio - 2000);
                string rutaFile = ruta + nameFile;

                ListarDetalleRer(0, regPeriodo.FechaIni, regPeriodo.FechaFin, out List<PfReporteTotalDTO> listaPFRerNC, out List<PfReporteDetDTO> listaRerDet);

                FileInfo newFile = new FileInfo(rutaFile);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(rutaFile);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //RER_NC
                    GenerarHojaExcelRerNC(xlPackage, "RER_NC", true, 1, 3
                                        , "", regCuadroRerNC.Pfcuanombre, regCuadroRerNC.Pfcuatitulo, regPeriodo.Pfperianio, regPeriodo.Pfperimes, regPeriodo.FechaIni, regPeriodo.FechaFin
                                        , listaPFRerNC, listaRerDet, listaBloqueHorario);
                    xlPackage.Save();
                }
            }
        }

        private void GeneraRptRegistroMedidores96(ExcelPackage xlPackage, string nameWS, int rowIni, int colIni, DateTime fecha1, DateTime fecha2
                                                , List<PfReporteTotalDTO> listaCentrales, List<MeMedicion96DTO> data, List<SiParametroValorDTO> listaBloqueHorario)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            int row = rowIni + 1;
            int col = colIni;
            //
            if (listaCentrales.Any())
            {
                #region cabecera

                int rowIniNombreReporte = rowIni;
                /// Fila Hora - Empresa - Total

                int colIniFecha = col;
                int rowIniFecha = row;
                int rowFinFecha = rowIniFecha + 2 - 1;
                ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Merge = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.WrapText = true;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                int colIniHora = colIniFecha + 1;
                int rowIniHora = rowIniFecha;
                int rowFinHora = rowFinFecha;
                ws.Cells[rowIniHora, colIniHora].Value = "HORA";
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Merge = true;
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.WrapText = true;
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniHora, colIniHora, rowFinHora, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                int rowIniEmp = rowIniFecha;

                int rowIniEquipo = rowIniEmp + 1;
                int colIniEquipo = colIniHora + 1;
                int colFinEquipo;

                for (int j = 0; j < listaCentrales.Count; j++)
                {
                    var thCentral = listaCentrales[j];

                    colFinEquipo = colIniEquipo;
                    ws.Cells[rowIniEmp, colIniEquipo].Value = thCentral.Emprnomb;
                    ws.Cells[rowIniEmp, colIniEquipo].Style.Font.Size = 8;
                    ws.Cells[rowIniEmp, colIniEquipo].Style.WrapText = true;
                    ws.Cells[rowIniEmp, colIniEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmp, colIniEquipo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEmp, colIniEquipo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[rowIniEquipo, colIniEquipo].Value = thCentral.Central;
                    ws.Cells[rowIniEquipo, colIniEquipo].Style.Font.Size = 8;
                    ws.Cells[rowIniEquipo, colIniEquipo].Style.WrapText = true;
                    ws.Cells[rowIniEquipo, colIniEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEquipo, colIniEquipo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[rowIniEquipo, colIniEquipo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    colIniEquipo = colFinEquipo + 1;
                }

                #endregion

                int rowIniData = rowIniEquipo + 1;
                row = rowIniData;

                #region cuerpo

                decimal? valor;
                int numDia = 0;

                int colData;
                for (var day = fecha1.Date; day.Date <= fecha2.Date; day = day.AddDays(1))
                {
                    var dataXDia = data.Where(x => x.Medifecha == day).ToList();
                    numDia++;

                    //parámetros de hora punta vigente para la fecha de consulta
                    SiParametroValorDTO paramHPyHFP = ParametroAppServicio.GetParametroVigenteHPyHFPXResolucion(listaBloqueHorario, day, ParametrosFormato.ResolucionCuartoHora);

                    //HORA
                    DateTime horas = day.AddMinutes(15);

                    for (int h = 1; h <= 96; h++)
                    {
                        colData = colIniHora + 1;

                        ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[row, colIniHora].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);

                        if (h >= paramHPyHFP.HIniHP && h <= paramHPyHFP.HFinHP)
                        {
                            foreach (var pto in listaCentrales)
                            {
                                MeMedicion96DTO regpotActiva = dataXDia.Find(x => x.Equipadre == pto.Equipadre);
                                valor = regpotActiva != null ? (decimal?)regpotActiva.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regpotActiva, null) : null;
                                ws.Cells[row, colData].Value = valor;
                                colData++;
                            }
                        }

                        horas = horas.AddMinutes(15);
                        row++;
                    }
                }

                //total

                colData = colIniHora + 1;
                for (int j = 0; j < listaCentrales.Count; j++)
                {
                    var thCentral = listaCentrales[j];
                    ws.Cells[row, colData].Value = thCentral.Pftotenergia;
                    ws.Cells[row, colData].Style.Font.Bold = true;
                    ws.Cells[row, colData].Style.Font.Size = 10;
                    colData++;
                }

                ///Fecha
                ws.Cells[rowIniData, colIniFecha, rowIniData + numDia * 96, colIniHora].Style.Font.Bold = true;
                ws.Cells[rowIniData, colIniFecha, rowIniData + numDia * 96, colIniHora].Style.Font.Size = 10;
                ws.Cells[rowIniData, colIniFecha, rowIniData + numDia * 96, colIniHora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniData, colIniFecha, rowIniData + numDia * 96, colIniHora].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                using (var range = ws.Cells[rowIniData, colIniHora + 1, rowIniData + numDia * 96 + 1, colData - 1])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 10;
                    range.Style.Numberformat.Format = "#,##0.000";
                }

                //mostrar lineas horas
                for (int c = colIniHora - 1; c <= colData - 1; c++)
                {
                    for (int f = rowIniData; f < rowIniData + numDia * 96; f += 8)
                    {
                        ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c].Style.Border.Top.Color.SetColor(Color.Blue);

                        ws.Cells[f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Color.SetColor(Color.Blue);

                        ws.Cells[f, c, f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Color.SetColor(Color.Blue);
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Color.SetColor(Color.Blue);
                    }
                }

                //Formato de Filas y columnas
                for (int columna = colIniHora + 1; columna < colData; columna++)
                    ws.Column(columna).Width = 14;

                ws.Column(colIniFecha).Width = 11;
                ws.Column(colIniHora).Width = 9;
                ws.Row(rowIniNombreReporte).Height = 30;
                ws.Row(rowIniEmp).Height = 40;
                ws.Row(rowIniEquipo).Height = 57;

                ws.View.FreezePanes(rowIniEquipo + 1, colIniHora + 1);
                ws.View.ZoomScale = 100;

                #endregion
            }

            ws.View.ZoomScale = 85;
        }

        #endregion

        #endregion

        #region Exportación excel

        /// <summary>
        /// Obtener la información para el Handson y excel
        /// </summary>
        /// <param name="pfrptcodi"></param>
        /// <param name="listaPF"></param>
        /// <param name="listaEscenario"></param>
        public void ListarReportePotenciaFirme(int pfrptcodiCuadroPf, out List<PfReporteTotalDTO> listaPF, out PfReporteTotalDTO regTotalPF
                                            , out List<PfReporteTotalDTO> listaPFxEmp, out List<PfEscenarioDTO> listaEscenario)
        {
            PfReporteDTO regReporte = GetByIdPfReporte(pfrptcodiCuadroPf);
            if (regReporte != null)
            {

                listaPF = new List<PfReporteTotalDTO>();

                //
                listaEscenario = GetByCriteriaPfEscenarios(pfrptcodiCuadroPf).OrderBy(x => x.Pfescefecini).ToList();
                int i = 1;
                foreach (var reg in listaEscenario)
                {
                    reg.Numero = i;
                    i++;
                }

                //
                List<PfReporteTotalDTO> listaBD = GetByCriteriaPfReporteTotals(pfrptcodiCuadroPf.ToString());

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                /// PFirme
                /// generar la lista de unidades de todo el periodo
                listaPF = listaBD.GroupBy(x => new { x.Equicodi, x.Grupocodi }).Select(x => new PfReporteTotalDTO()
                {
                    Emprcodi = x.First().Emprcodi,
                    Emprnomb = x.First().Emprnomb,
                    Equipadre = x.First().Equipadre,
                    Central = x.First().Central,
                    Equicodi = x.Key.Equicodi,
                    Grupocodi = x.Key.Grupocodi,
                    Famcodi = x.First().Famcodi,
                    Pftotunidadnomb = x.First().Pftotunidadnomb,
                    Pftotpe = x.First().Pftotpe,
                    Pftotpprom = x.First().Pftotpprom,
                    Pftotfi = x.First().Pftotfi,
                    Pftotfp = x.First().Pftotfp,
                    Pftotpg = x.First().Pftotpg,
                    Grupotipocogen = x.First().Grupotipocogen,
                    Pftotminsincu = x.First().Pftotminsincu,
                    Pftotincremental = x.First().Pftotincremental
                }).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Pftotincremental).ThenBy(x => x.Pftotunidadnomb).ToList();

                foreach (var regUnidad in listaPF)
                {
                    //total por escenario
                    foreach (var regEsc in listaEscenario)
                    {
                        var regTotalXUnidadXEsc = listaBD.Find(x => x.Pfescecodi == regEsc.Pfescecodi && x.Equicodi == regUnidad.Equicodi && x.Grupocodi == regUnidad.Grupocodi);
                        if (regTotalXUnidadXEsc != null)
                        {
                            regUnidad.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpf + regEsc.Numero.ToString()).SetValue(regUnidad, regTotalXUnidadXEsc.Pftotpf);
                            regUnidad.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpe + regEsc.Numero.ToString()).SetValue(regUnidad, regTotalXUnidadXEsc.Pftotpe);

                            regEsc.Total += regTotalXUnidadXEsc.Pftotpf.GetValueOrDefault(0);
                            regEsc.TotalPe += regTotalXUnidadXEsc.Pftotpe.GetValueOrDefault(0);
                        }
                    }


                }

                regTotalPF = new PfReporteTotalDTO()
                {
                    Emprnomb = "TOTAL",
                    Pftotpe = listaPF.Sum(x => x.Pftotpe ?? 0),
                };
                foreach (var regEsc in listaEscenario)
                {
                    regTotalPF.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpf + regEsc.Numero.ToString()).SetValue(regTotalPF, regEsc.Total);
                    regTotalPF.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpe + regEsc.Numero.ToString()).SetValue(regTotalPF, regEsc.TotalPe);

                    decimal? porcentaje = regTotalPF.Pftotpe > 0 ? regEsc.Total / regTotalPF.Pftotpe : null;
                    regTotalPF.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpfpor + regEsc.Numero.ToString()).SetValue(regTotalPF, porcentaje);
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                /// PFirmeEmp
                ///generar la lista de empresas de todo el periodo
                listaPFxEmp = listaBD.GroupBy(x => new { x.Emprcodi }).Select(x => new PfReporteTotalDTO()
                {
                    Emprcodi = x.Key.Emprcodi,
                    Emprnomb = x.First().Emprnomb,
                }).OrderBy(x => x.Emprnomb).ToList();

                foreach (var regEmp in listaPFxEmp)
                {
                    var listaUnidadxEmp = listaPF.Where(x => x.Emprcodi == regEmp.Emprcodi).ToList();

                    foreach (var regEsc in listaEscenario)
                    {
                        decimal valorEmpXEsc = 0, valEmpEscPe = 0;
                        foreach (var regUnidad in listaUnidadxEmp)
                        {
                            decimal? valorPf = (decimal?)regUnidad.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpf + regEsc.Numero).GetValue(regUnidad, null);
                            decimal? valorPe = (decimal?)regUnidad.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpe + regEsc.Numero).GetValue(regUnidad, null);

                            valorEmpXEsc += valorPf.GetValueOrDefault(0);
                            valEmpEscPe += valorPe ?? 0;
                        }

                        if (valorEmpXEsc > 0)
                            regEmp.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpf + regEsc.Numero.ToString()).SetValue(regEmp, valorEmpXEsc);

                        if (valEmpEscPe > 0)
                            regEmp.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpe + regEsc.Numero.ToString()).SetValue(regEmp, valEmpEscPe);
                    }
                }
            }
            else
            {
                listaPF = new List<PfReporteTotalDTO>();
                regTotalPF = new PfReporteTotalDTO();
                listaPFxEmp = new List<PfReporteTotalDTO>();
                listaEscenario = new List<PfEscenarioDTO>();

            }

        }

        /// <summary>
        /// Obtiene lista PfReporteTotal por codigo de reporte
        /// </summary>
        /// <param name="pfrptcodi"></param>
        /// <returns></returns>
        public List<PfReporteTotalDTO> ListarPotenciaFirme(int pfrptcodi, List<EqEquipoDTO> listaUnidadesTermo = null)
        {
            List<PfEscenarioDTO> listaEscenario = GetByCriteriaPfEscenarios(pfrptcodi);
            List<PfReporteTotalDTO> listaBD = GetByCriteriaPfReporteTotals(pfrptcodi.ToString());

            List<PfReporteTotalDTO> lista = new List<PfReporteTotalDTO>();

            if (listaUnidadesTermo != null)
            {
                foreach (var item in listaBD.Where(x => x.Pftotincremental == 1))
                {
                    item.Grupocodi2 = listaUnidadesTermo.Find(x => x.Equicodi == item.Equicodi).Grupocodi;
                }
            }


            foreach (var esc in listaEscenario)
            {
                foreach (var item in listaBD.Where(x => x.Pfescecodi == esc.Pfescecodi))
                {
                    item.Pfescefecini = esc.Pfescefecini;
                    item.Pfescefecfin = esc.Pfescefecfin;
                    lista.Add(item);
                }

            }
            return lista;
        }

        public List<MeMedicion96DTO> ListarDetalleCogeneracion(DateTime fechaIni, DateTime fechaFin, ref List<PfReporteTotalDTO> listaPFCog)
        {
            //Detalle Cogeneración
            string empresasCog = string.Join(", ", listaPFCog.Where(x => x.Grupotipocogen == ConstantesAppServicio.SI).Select(x => x.Emprcodi));
            List<MeMedicion96DTO> lista96Cog = (new ReporteMedidoresAppServicio()).ListaDataMDGeneracionConsolidado(fechaIni, fechaFin, ConstantesMedicion.IdTipogrupoCOES, ConstantesMedicion.IdTipoGeneracionTodos.ToString(), empresasCog
                , ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), false);
            lista96Cog = lista96Cog.Where(x => x.Grupotipocogen == ConstantesAppServicio.SI).ToList();

            TimeSpan ts = fechaFin.Date - fechaIni.Date;
            int diasPeriodo = ts.Days + 1;
            int totalMinutosPeriodo = diasPeriodo * 96 * 15;

            foreach (var reg in listaPFCog)
            {
                int totalMinutosCalorUtil = totalMinutosPeriodo - reg.Pftotminsincu;

                if (reg.Pftotminsincu > 0)
                    reg.ComentarioCalorUtil = $"Tiempo que estuvo asociado a su calor útil = {totalMinutosCalorUtil} minutos \n Tiempo que no estuvo asociado a calor útil = {reg.Pftotminsincu} minutos";
            }

            return lista96Cog;
        }

        public void ListarDetalleRer(int pfrecacodi, DateTime fechaIni, DateTime fechaFin, out List<PfReporteTotalDTO> listaPFRerNC, out List<PfReporteDetDTO> listaBDDet)
        {
            listaPFRerNC = new List<PfReporteTotalDTO>();
            listaBDDet = new List<PfReporteDetDTO>();

            var regRer = GetByCriteriaPfReportes(pfrecacodi, ConstantesPotenciaFirme.CuadroRerNC).OrderByDescending(x => x.Pfrptcodi).FirstOrDefault();
            if (regRer != null)
            {
                int pfrptcodiCuadroRerNC = regRer.Pfrptcodi;

                listaPFRerNC = GetByCriteriaPfReporteTotals(pfrptcodiCuadroRerNC.ToString());
                listaBDDet = GetByCriteriaPfReporteDets(pfrptcodiCuadroRerNC);

                //RER NC
                var familia = $"{ConstantesHorasOperacion.IdGeneradorSolar},{ConstantesHorasOperacion.IdGeneradorEolica},{ConstantesHorasOperacion.IdTipoSolar},{ConstantesHorasOperacion.IdTipoEolica}";
                var listaEquiposOCEolicoSolar = _equipamientoAppServicio.ListarEquiposTienenOpComercial(fechaIni, fechaFin, familia, out List<ResultadoValidacionAplicativo> listaMsjEq);

                foreach (var reg in listaPFRerNC)
                {
                    var regOpCom = listaEquiposOCEolicoSolar.Find(x => x.Equicodi == reg.Equicodi);
                    if (regOpCom != null)
                    {
                        reg.Equifechiniopcom = regOpCom.Equifechiniopcom;
                        reg.Equifechfinopcom = regOpCom.Equifechfinopcom;
                        reg.EquifechiniopcomDesc = reg.Equifechiniopcom != null ? reg.Equifechiniopcom.Value.ToString(ConstantesAppServicio.FormatoFecha) : "";

                        reg.Tiene36Meses = fechaIni.AddMonths(-36) >= reg.Equifechiniopcom;
                        if (!reg.Tiene36Meses) reg.Central = $"{reg.Central} (*)";
                    }
                }

                listaPFRerNC = listaPFRerNC.OrderBy(x => x.Equifechiniopcom).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ToList();
            }
        }

        /// <summary>
        /// Generar archivo excel por versión reporte plantilla térmico
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="irptcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nameFile"></param>
        public void GenerarArchivoExcelPF(string ruta, int tipo, int pfrptcodi, out string nameFile)
        {
            PfReporteDTO regReporte = GetByIdPfReporte(pfrptcodi);
            PfRecalculoDTO regRecalculo = GetByIdPfRecalculo(regReporte.Pfrecacodi);
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            DateTime fechaIni = new DateTime(regPeriodo.Pfperianio, regPeriodo.Pfperimes, 1);
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

            bool esExportarIndFort = ConstantesPotenciaFirme.CuadroTodo == tipo || ConstantesPotenciaFirme.CuadroIndFort == tipo;
            bool esExportarIndProgHidro = ConstantesPotenciaFirme.CuadroTodo == tipo || ConstantesPotenciaFirme.CuadroIndProgHidro == tipo;
            bool esExportarIndProg = ConstantesPotenciaFirme.CuadroTodo == tipo || ConstantesPotenciaFirme.CuadroIndProg == tipo;
            bool esExportarFp = ConstantesPotenciaFirme.CuadroTodo == tipo || ConstantesPotenciaFirme.CuadroFp == tipo;
            bool esExportarCog = ConstantesPotenciaFirme.CuadroTodo == tipo || ConstantesPotenciaFirme.CuadroCog == tipo;
            bool esExportarRerNC = ConstantesPotenciaFirme.CuadroTodo == tipo || ConstantesPotenciaFirme.CuadroRerNC == tipo;
            bool esExportarPFirme = ConstantesPotenciaFirme.CuadroTodo == tipo || ConstantesPotenciaFirme.CuadroPFirme == tipo;
            bool esExportarPFirmeEmp = ConstantesPotenciaFirme.CuadroTodo == tipo || ConstantesPotenciaFirme.CuadroPFirmeEmp == tipo;
            bool incluirDetalle = ConstantesPotenciaFirme.CuadroTodo != tipo;

            PfCuadroDTO regCuadroCog = GetByIdPfCuadro(ConstantesPotenciaFirme.CuadroCog);
            PfCuadroDTO regCuadroRerNC = GetByIdPfCuadro(ConstantesPotenciaFirme.CuadroRerNC);
            PfCuadroDTO regCuadroPFirme = GetByIdPfCuadro(ConstantesPotenciaFirme.CuadroPFirme);
            PfCuadroDTO regCuadroPFirmeEmp = GetByIdPfCuadro(ConstantesPotenciaFirme.CuadroPFirmeEmp);

            nameFile = string.Format("PF_{0}_{1}.xlsx", regPeriodo.Pfperimes, regPeriodo.Pfperianio - 2000);
            string rutaFile = ruta + nameFile;

            //Potencia Firme
            this.ListarReportePotenciaFirme(pfrptcodi, out List<PfReporteTotalDTO> listaPF, out PfReporteTotalDTO regTotalPF
                                            , out List<PfReporteTotalDTO> listaPFxEmp, out List<PfEscenarioDTO> listaEscenario);
            List<PfRelacionIndisponibilidadesDTO> listaIrpt = this.GetByCriteriaPfRelacionIndisponibilidadess(pfrptcodi);

            //
            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //IndFortuita
                if (esExportarIndFort)
                {
                    int rptcodi8 = listaIrpt.Find(x => x.Icuacodi == ConstantesIndisponibilidades.ReportePR25FactorFortTermico).Irptcodi;

                    servIndisp.GenerarReporteFactorFortTermicoXVersionReporte(rptcodi8, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                            , out HandsonModel handsonFF
                                                            , out IndReporteDTO regVersion, out IndPeriodoDTO regPeriodo8, out IndCuadroDTO regCuadro8
                                                            , out List<IndReporteTotalDTO> listaReptot, out List<IndReporteDetDTO> listaRepdet, out List<IndReporteTotalDTO> listaReptotMes
                                                            , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral, out List<FactorINDTeorica> listaTeorica, out List<SiParametroValorDTO> listaBloqueHorario);

                    servIndisp.GenerarHojaExcelFactorFortuitoTermico(xlPackage, "IndFortuita", 5, 4, 11, 1
                                                                , regRecalculo.Pfrecainforme, "CUADRO N° 1", regCuadro8.Icuanombre, regPeriodo.Pfperianio, regPeriodo.Pfperimes, regPeriodo.FechaIni
                                                                , listaReptot, listaRepdet, listaReptotMes
                                                                , listaEmpresa, listaCentral, listaTeorica, listaBloqueHorario);
                    xlPackage.Save();
                }

                //IndProgHidro
                if (esExportarIndProgHidro)
                {
                    int rptcodi10 = listaIrpt.Find(x => x.Icuacodi == ConstantesIndisponibilidades.ReportePR25FactorProgHidro).Irptcodi;

                    servIndisp.GenerarReporteFactorProgramadoHidroXVersionReporte(rptcodi10, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                            , out HandsonModel handsonFF
                                                            , out IndReporteDTO regVersion, out IndPeriodoDTO regPeriodo10, out IndCuadroDTO regCuadro
                                                            , out List<IndReporteTotalDTO> listaReptot, out List<IndReporteDetDTO> listaRepdet, out List<IndReporteTotalDTO> listaReptotMes
                                                            , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral, out List<SiParametroValorDTO> listaBloqueHorario);

                    servIndisp.GenerarHojaExcelFactorProgramadoHidro(xlPackage, "IndProgHidro", 1, 7, 7, 2
                                                                , regRecalculo.Pfrecainforme, "CUADRO N°  3", regCuadro.Icuatitulo, regPeriodo10.Iperianio, regPeriodo10.Iperimes, regPeriodo.FechaIni
                                                                , listaReptot, listaRepdet, listaReptotMes
                                                                , listaEmpresa, listaCentral, listaBloqueHorario);
                    xlPackage.Save();
                }

                //IndProgramada
                if (esExportarIndProg)
                {
                    int rptcodi9 = listaIrpt.Find(x => x.Icuacodi == ConstantesIndisponibilidades.ReportePR25FactorProgTermico).Irptcodi;

                    servIndisp.GenerarReporteFactorProgramadoTermoXVersionReporte(rptcodi9, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                            , out HandsonModel handsonFP
                                                            , out IndReporteDTO regVersion, out IndPeriodoDTO regPeriodo9, out IndCuadroDTO regCuadro
                                                            , out List<IndReporteTotalDTO> listaReptot, out List<IndReporteDetDTO> listaRepdet
                                                            , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral, out List<SiParametroValorDTO> listaBloqueHorario);

                    servIndisp.GenerarHojaExcelFactorProgramadoTermo(xlPackage, "IndProgramada", 1, 7, 7, 2
                                                                , regRecalculo.Pfrecainforme, "CUADRO N°  2", regCuadro.Icuatitulo, regPeriodo9.Iperianio, regPeriodo9.Iperimes, regPeriodo.FechaIni
                                                                , listaReptot, listaRepdet
                                                                , listaEmpresa, listaCentral, listaBloqueHorario);
                    xlPackage.Save();
                }

                //Factorpresencia
                if (esExportarFp)
                {
                    int rptcodi11 = listaIrpt.Find(x => x.Icuacodi == ConstantesIndisponibilidades.ReportePR25FactorPresencia).Irptcodi;

                    servIndisp.GenerarReporteFactorPresenciaCuadroXVersionReporte(rptcodi11, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                            , out HandsonModel FP
                                                            , out IndReporteDTO regVersion, out IndPeriodoDTO regPeriodo10, out IndCuadroDTO regCuadro
                                                            , out List<IndReporteTotalDTO> listaReptot, out List<SiEmpresaDTO> listaEmpresa);

                    servIndisp.GenerarHojaExcelFactorPresencia(xlPackage, "Factorpresencia", 1, 4, 8, 1
                                                                , regRecalculo.Pfrecainforme, "CUADRO N° 4", regCuadro.Icuatitulo, regPeriodo10.Iperianio, regPeriodo10.Iperimes, regPeriodo10.TotalDias, regPeriodo.FechaIni
                                                                , listaReptot, listaEmpresa);
                    xlPackage.Save();
                }

                //COG
                if (esExportarCog)
                {
                    List<PfReporteTotalDTO> listaPFCog = listaPF.Where(x => x.Grupotipocogen == ConstantesAppServicio.SI).ToList();
                    List<MeMedicion96DTO> lista96Cog = new List<MeMedicion96DTO>();
                    if (esExportarCog)
                    {
                        lista96Cog = ListarDetalleCogeneracion(fechaIni, fechaFin, ref listaPFCog);
                    }

                    GenerarHojaExcelCOG(xlPackage, "COG", esExportarCog, 1, 2
                                        , fechaIni, fechaFin, regRecalculo.Pfrecainforme, regCuadroCog.Pfcuanombre, regCuadroCog.Pfcuatitulo, regPeriodo.Pfperimes
                                        , "Potencia Firme de una Unidad de Central de Cogeneración Calificada", "PR-N° 26 numeral 8.2"
                                        , listaPFCog, lista96Cog);
                    xlPackage.Save();
                }

                //RER_NC
                if (esExportarRerNC)
                {
                    List<PfReporteTotalDTO> listaPFRerNC = new List<PfReporteTotalDTO>();
                    List<PfReporteDetDTO> listaRerDet = new List<PfReporteDetDTO>();
                    if (esExportarRerNC)
                    {
                        ListarDetalleRer(regRecalculo.Pfrecacodi, fechaIni, fechaFin, out listaPFRerNC, out listaRerDet);
                    }

                    //Hora punta segun fecha de vigencia
                    List<SiParametroValorDTO> listaBloqueHorario = (new ParametroAppServicio()).ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

                    GenerarHojaExcelRerNC(xlPackage, "RER_NC", true, 1, 3
                                        , regRecalculo.Pfrecainforme, regCuadroRerNC.Pfcuanombre, regCuadroRerNC.Pfcuatitulo, regPeriodo.Pfperianio, regPeriodo.Pfperimes, fechaIni, fechaFin
                                        , listaPFRerNC, listaRerDet, listaBloqueHorario);
                    xlPackage.Save();
                }

                //PFirme
                if (esExportarPFirme)
                {
                    GenerarHojaExcelPFirme(xlPackage, "PFirme", incluirDetalle, 1, 3, regRecalculo.Pfrecainforme, regCuadroPFirme.Pfcuanombre, regCuadroPFirme.Pfcuatitulo, "PROCEDIMIENTO N° 26", regPeriodo.Pfperianio, regPeriodo.Pfperimes
                                        , listaPF, regTotalPF, listaEscenario);
                    xlPackage.Save();
                }

                //PFirmeEmp
                if (esExportarPFirmeEmp)
                {
                    //PFirmeEmp
                    int versionCVId = ConstantesPotenciaFirme.ParametroDefecto;
                    this.ListarContratosCV(regRecalculo.Pfrecacodi, versionCVId, out List<PfContratosDTO> lstContratosCV, out PfVersionDTO pfVersionRecurso);

                    GenerarHojaExcelPFirmeEmp(xlPackage, "PFirmeEmp", incluirDetalle, 1, 2, regRecalculo.Pfrecainforme, regCuadroPFirmeEmp.Pfcuanombre, regCuadroPFirmeEmp.Pfcuatitulo, "PROCEDIMIENTO N° 26", regPeriodo.Pfperianio, regPeriodo.Pfperimes
                                        , listaPFxEmp, regTotalPF, listaEscenario, lstContratosCV);
                    xlPackage.Save();
                }
            }
        }

        /// <summary>
        /// Generar hoja excel cuadro detalle Fotuito y Programado
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="tipoIndisp"></param>
        /// <param name="incluirDescripcion"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaReptHoja"></param>
        /// <param name="listaRepdetHojaInput"></param>
        public void GenerarHojaExcelPFirme(ExcelPackage xlPackage, string nameWS, bool incluirDetalle, int rowIni, int colIni
                                                   , string informe, string nombreCuadro, string titulo, string subtitulo, int anio, int mes
                                                    , List<PfReporteTotalDTO> listaReptHoja, PfReporteTotalDTO regTotalPF, List<PfEscenarioDTO> listaEscenario)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            ws.TabColor = ColorTranslator.FromHtml("#FFC000");

            string font = "Arial";
            string colorCeldaTextoDefecto = "#000000";
            string colorCeldaFondoDefecto = "#FFFFFF";
            string colorLineaDefecto = "#000000";

            #region  Filtros y Cabecera

            int colEmpresa = colIni;
            int colCentral = colEmpresa + 1;
            int colUnidad = colCentral + 1;
            int colPe = colUnidad + 1;
            int colFI = colPe + 1;
            int colPG = colFI + 1;
            int colFP = colPG + 1;
            int colIniEsc = colFP + 1;
            int colUltEsc = colIniEsc + listaEscenario.Count - 1;

            ws.Cells[rowIni, colFP].Value = informe; //COES/D/DO/STR-INF-063-2020
            UtilExcel.SetFormatoCelda(ws, rowIni, colFP, rowIni, colFP, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, true);

            ws.Cells[rowIni + 3, colEmpresa].Value = nombreCuadro; // CUADRO N°  5
            UtilExcel.SetFormatoCelda(ws, rowIni + 3, colEmpresa, rowIni + 3, colEmpresa, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 12, true);

            ws.Cells[rowIni + 4, colEmpresa].Value = subtitulo; //PROCEDIMIENTO N° 26
            UtilExcel.SetFormatoCelda(ws, rowIni + 4, colEmpresa, rowIni + 4, colEmpresa, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 12, true);

            ws.Cells[rowIni + 5, colEmpresa].Value = titulo; //CÁLCULO DE LA POTENCIA FIRME
            UtilExcel.SetFormatoCelda(ws, rowIni + 5, colEmpresa, rowIni + 5, colEmpresa, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 12, true);

            ws.Cells[rowIni + 6, colEmpresa].Value = EPDate.f_NombreMes(mes).ToUpper() + " " + anio; //NOVIEMBRE
            UtilExcel.SetFormatoCelda(ws, rowIni + 6, colEmpresa, rowIni + 6, colEmpresa, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 12, true);

            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 3, colEmpresa, rowIni + 3, colUltEsc);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 4, colEmpresa, rowIni + 4, colUltEsc);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 5, colEmpresa, rowIni + 5, colUltEsc);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 6, colEmpresa, rowIni + 6, colUltEsc);

            int rowEmpresa = rowIni + 9;
            ws.Cells[rowEmpresa, colEmpresa].Value = "Empresa";
            ws.Cells[rowEmpresa, colCentral].Value = "Central";
            ws.Cells[rowEmpresa, colUnidad].Value = "Unidad";
            ws.Cells[rowEmpresa, colPe].Value = "Potencia Efectiva \n (MW)";
            ws.Cells[rowEmpresa, colFI].Value = "Factor de Indisponibilidad";
            ws.Cells[rowEmpresa, colPG].Value = "Potencia Garantizada \n (MW)";
            ws.Cells[rowEmpresa, colFP].Value = "Factor de Presencia";

            UtilExcel.SetFormatoCelda(ws, rowEmpresa, colEmpresa, rowEmpresa, colFP, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false, true);

            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colEmpresa, rowEmpresa, colEmpresa, colorLineaDefecto);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colCentral, rowEmpresa, colCentral, colorLineaDefecto);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colUnidad, rowEmpresa, colUnidad, colorLineaDefecto);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colPe, rowEmpresa, colPe, colorLineaDefecto);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colFI, rowEmpresa, colFI, colorLineaDefecto);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colPG, rowEmpresa, colPG, colorLineaDefecto);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colFP, rowEmpresa, colFP, colorLineaDefecto);

            double factorAncho = 1;

            ws.Row(rowEmpresa).Height = 53;
            ws.Column(1).Width = 4 * factorAncho;
            ws.Column(2).Width = 1 * factorAncho;
            ws.Column(colEmpresa).Width = 73 * factorAncho;
            ws.Column(colCentral).Width = 36 * factorAncho;
            ws.Column(colUnidad).Width = 27 * factorAncho;
            ws.Column(colPe).Width = 14 * factorAncho;
            ws.Column(colFI).Width = 14 * factorAncho;
            ws.Column(colPG).Width = 14 * factorAncho;
            ws.Column(colFP).Width = 14 * factorAncho;

            foreach (var reg in listaEscenario)
            {
                ws.Cells[rowEmpresa, colIniEsc].Value = "Potencia Firme \n" + reg.FechaDesc + " \n(MW)";
                UtilExcel.SetFormatoCelda(ws, rowEmpresa, colIniEsc, rowEmpresa, colIniEsc, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false, true);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colIniEsc, rowEmpresa, colIniEsc, colorLineaDefecto);
                ws.Column(colIniEsc).Width = 26 * factorAncho;
                colIniEsc++;
            }

            #endregion

            #region Cuerpo Principal

            int rowData = rowEmpresa + 1;
            int rowIniRangoEmpresa = rowData;
            string empresaActual, empresaSiguiente;

            for (int i = 0; i < listaReptHoja.Count; i++)
            {
                var reg = listaReptHoja[i];
                empresaActual = reg.Emprnomb;
                empresaSiguiente = i + 1 < listaReptHoja.Count ? listaReptHoja[i + 1].Emprnomb : string.Empty;

                ws.Cells[rowData, colEmpresa].Value = reg.Emprnomb;
                ws.Cells[rowData, colCentral].Value = reg.Central;
                ws.Cells[rowData, colUnidad].Value = reg.Pftotunidadnomb;
                ws.Cells[rowData, colPe].Value = reg.Pftotpe;
                ws.Cells[rowData, colFI].Value = reg.Pftotfi;
                ws.Cells[rowData, colPG].Value = reg.Pftotpg;
                ws.Cells[rowData, colFP].Value = reg.Pftotfp;

                UtilExcel.CeldasExcelIndentar(ws, rowData, colEmpresa, rowData, colUnidad, 1);

                UtilExcel.SetFormatoCelda(ws, rowData, colEmpresa, rowData, colUnidad, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
                UtilExcel.SetFormatoCelda(ws, rowData, colPe, rowData, colFP, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);

                UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colPe, rowData, colPe, 3);

                UtilExcel.CeldasExcelFormatoPorcentaje(ws, rowData, colFI, rowData, colFI, 3);

                UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colPG, rowData, colPG, 3);

                UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colFP, rowData, colFP, 2);

                colIniEsc = colFP + 1;
                foreach (var regEsc in listaEscenario)
                {
                    decimal? valor = (decimal?)reg.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpf + regEsc.Numero).GetValue(reg, null);

                    ws.Cells[rowData, colIniEsc].Value = valor;
                    UtilExcel.SetFormatoCelda(ws, rowData, colIniEsc, rowData, colIniEsc, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
                    UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colIniEsc, rowData, colIniEsc, 3);
                    colIniEsc++;
                }


                //agrupar cada columna
                if (empresaActual != empresaSiguiente)
                {
                    for (int c = colEmpresa; c <= colUltEsc; c++)
                        UtilExcel.BorderCeldasLineaDelgada(ws, rowIniRangoEmpresa, c, rowData, c, colorLineaDefecto);
                    rowIniRangoEmpresa = rowData + 1;
                }

                rowData++;
            }

            #region Total

            ws.Cells[rowData, colEmpresa].Value = "TOTAL";
            UtilExcel.SetFormatoCelda(ws, rowData, colEmpresa, rowData, colEmpresa, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
            UtilExcel.CeldasExcelAgrupar(ws, rowData, colEmpresa, rowData, colUnidad);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colEmpresa, rowData, colUnidad, colorLineaDefecto);

            ws.Cells[rowData, colPe].Value = regTotalPF.Pftotpe;
            UtilExcel.SetFormatoCelda(ws, rowData, colPe, rowData, colFP, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
            UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colPe, rowData, colPe, 3);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colPe, rowData, colPe, colorLineaDefecto);

            UtilExcel.CeldasExcelAgrupar(ws, rowData, colFI, rowData, colFP);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colFI, rowData, colFP, colorLineaDefecto);

            colIniEsc = colFP + 1;
            foreach (var regEsc in listaEscenario)
            {
                decimal? valor = (decimal?)regTotalPF.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpf + regEsc.Numero).GetValue(regTotalPF, null);
                decimal? valorPorcentaje = (decimal?)regTotalPF.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpfpor + regEsc.Numero).GetValue(regTotalPF, null);

                ws.Cells[rowData, colIniEsc].Value = valor;
                UtilExcel.SetFormatoCelda(ws, rowData, colIniEsc, rowData, colIniEsc, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
                UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colIniEsc, rowData, colIniEsc, 3);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colIniEsc, rowData, colIniEsc, colorLineaDefecto);

                ws.Cells[rowData + 1, colIniEsc].Value = valorPorcentaje;
                UtilExcel.CeldasExcelFormatoPorcentaje(ws, rowData + 1, colIniEsc, rowData + 1, colIniEsc, 3);
                UtilExcel.SetFormatoCelda(ws, rowData + 1, colIniEsc, rowData + 1, colIniEsc, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);

                colIniEsc++;
            }

            #endregion

            #endregion

            ws.View.FreezePanes(rowEmpresa + 1, 1);

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            ws.View.ZoomScale = 80;

            //filter

            //Todo el excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        /// <summary>
        /// Generar hoja excel cuadro detalle Fotuito y Programado
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="tipoIndisp"></param>
        /// <param name="incluirDescripcion"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaReptHoja"></param>
        /// <param name="listaRepdetHojaInput"></param>
        public void GenerarHojaExcelPFirmeEmp(ExcelPackage xlPackage, string nameWS, bool incluirDetalle, int rowIni, int colIni
                                                   , string informe, string nombreCuadro, string titulo, string subtitulo, int anio, int mes
                                                    , List<PfReporteTotalDTO> listaReptHoja, PfReporteTotalDTO regTotalPF, List<PfEscenarioDTO> listaEscenario, List<PfContratosDTO> listaContrato)
        {
            //inicia en B1

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            ws.TabColor = ColorTranslator.FromHtml("#FFC000");

            string font = "Arial";
            string colorCeldaTextoDefecto = "#000000";
            string colorCeldaFondoDefecto = "#FFFFFF";
            string colorLineaDefecto = "#000000";

            #region  Filtros y Cabecera

            int colEmpresa = colIni + 3;
            int colIniEsc = colEmpresa + 1;
            int colUltEsc = colIniEsc + listaEscenario.Count - 1;
            int colInforme = colIni + 10;

            ws.Cells[rowIni, colInforme].Value = informe; //COES/D/DO/SME-INF-008-2021
            UtilExcel.SetFormatoCelda(ws, rowIni, colInforme, rowIni, colInforme, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, true);

            ws.Cells[rowIni + 2, colEmpresa].Value = nombreCuadro; //      CUADRO N°  6
            UtilExcel.SetFormatoCelda(ws, rowIni + 2, colEmpresa, rowIni + 2, colEmpresa, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 11, true);

            ws.Cells[rowIni + 3, colEmpresa].Value = subtitulo; //POTENCIA FIRME POR EMPRESA
            UtilExcel.SetFormatoCelda(ws, rowIni + 3, colEmpresa, rowIni + 3, colEmpresa, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 11, true);

            ws.Cells[rowIni + 4, colEmpresa].Value = EPDate.f_NombreMes(mes).ToUpper() + " " + anio; // DICIEMBRE 2020			
            UtilExcel.SetFormatoCelda(ws, rowIni + 4, colEmpresa, rowIni + 4, colEmpresa, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 11, true);

            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 2, colEmpresa, rowIni + 2, colUltEsc);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 3, colEmpresa, rowIni + 3, colUltEsc);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 4, colEmpresa, rowIni + 4, colUltEsc);

            int rowEmpresa = rowIni + 6;
            ws.Cells[rowEmpresa, colEmpresa].Value = "Empresa";

            UtilExcel.SetFormatoCelda(ws, rowEmpresa, colEmpresa, rowEmpresa, colEmpresa, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, false, true);

            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colEmpresa, rowEmpresa, colEmpresa, colorLineaDefecto);

            double factorAncho = 1;

            ws.Row(rowEmpresa).Height = 33;
            ws.Column(1).Width = 1 * factorAncho;
            ws.Column(colEmpresa).Width = 48 * factorAncho;
            ws.Column(colInforme).Width = 4;

            foreach (var reg in listaEscenario)
            {
                ws.Cells[rowEmpresa, colIniEsc].Value = "Potencia Firme \n" + reg.FechaDesc + " \n(MW)";
                UtilExcel.SetFormatoCelda(ws, rowEmpresa, colIniEsc, rowEmpresa, colIniEsc, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, false, true);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colIniEsc, rowEmpresa, colIniEsc, colorLineaDefecto);
                ws.Column(colIniEsc).Width = 14 * factorAncho;
                colIniEsc++;
            }

            #endregion

            #region Cuerpo Principal

            int rowData = rowEmpresa + 1;

            for (int i = 0; i < listaReptHoja.Count; i++)
            {
                var reg = listaReptHoja[i];

                ws.Cells[rowData, colEmpresa].Value = reg.Emprnomb;

                UtilExcel.CeldasExcelIndentar(ws, rowData, colEmpresa, rowData, colEmpresa, 1);
                UtilExcel.SetFormatoCelda(ws, rowData, colEmpresa, rowData, colEmpresa, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, false);

                colIniEsc = colEmpresa + 1;
                foreach (var regEsc in listaEscenario)
                {
                    decimal? valor = (decimal?)reg.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpf + regEsc.Numero).GetValue(reg, null);

                    ws.Cells[rowData, colIniEsc].Value = valor;
                    UtilExcel.SetFormatoCelda(ws, rowData, colIniEsc, rowData, colIniEsc, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, false);
                    UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colIniEsc, rowData, colIniEsc, 3);
                    colIniEsc++;
                }

                rowData++;
            }

            ws.Row(rowData).Height = 24;

            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa + 1, colEmpresa, rowData - 1, colEmpresa, colorLineaDefecto);
            colIniEsc = colEmpresa + 1;
            foreach (var regEsc in listaEscenario)
            {
                UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa + 1, colIniEsc, rowData - 1, colIniEsc, colorLineaDefecto);
                colIniEsc++;
            }

            #region Total

            UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colEmpresa, rowData, colEmpresa, colorLineaDefecto);

            colIniEsc = colEmpresa + 1;
            foreach (var regEsc in listaEscenario)
            {
                decimal? valor = (decimal?)regTotalPF.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpf + regEsc.Numero).GetValue(regTotalPF, null);

                ws.Cells[rowData, colIniEsc].Value = valor;
                UtilExcel.SetFormatoCelda(ws, rowData, colIniEsc, rowData, colIniEsc, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, false);
                UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colIniEsc, rowData, colIniEsc, 3);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colIniEsc, rowData, colIniEsc, colorLineaDefecto);

                colIniEsc++;
            }

            #endregion

            #endregion

            #region Contratos Compra Venta

            if (listaContrato.Any())
            {
                int rowNota = rowData + 2;
                int rowCedente = rowNota + 2;
                int colCedente = colIni;
                int colCantidad = colCedente + 5;
                int colPeriodoIni = colCantidad + 1;
                int colPeriodoFin = colPeriodoIni + 1;
                int colObservacion = colPeriodoFin + 1;

                ws.Column(colPeriodoIni).Width = 15;
                ws.Column(colPeriodoFin).Width = 15;
                ws.Column(colObservacion).Width = 35;
                ws.Column(colObservacion + 1).Width = 1;

                ws.Cells[rowNota, colCedente].Value = "Nota: Las siguientes empresas comunicaron contratos de respaldo de potencia firme:";
                UtilExcel.SetFormatoCelda(ws, rowNota, colCedente, rowNota, colCedente, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 7, false);

                ws.Cells[rowCedente, colCedente].Value = "Cedente / Cesionario";
                ws.Cells[rowCedente, colCantidad].Value = "Cantidad (MW)";
                ws.Cells[rowCedente, colPeriodoIni].Value = "Inicio";
                ws.Cells[rowCedente, colPeriodoFin].Value = "Fin";
                ws.Cells[rowCedente, colObservacion].Value = "Observaciones";

                UtilExcel.SetFormatoCelda(ws, rowCedente, colCedente, rowCedente, colObservacion, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 7, true);
                UtilExcel.SetFormatoCelda(ws, rowCedente, colPeriodoIni, rowCedente, colPeriodoIni, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 7, true);
                UtilExcel.CeldasExcelAgrupar(ws, rowCedente, colCedente, rowCedente, colCantidad - 1);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCedente, colCedente, rowCedente, colCantidad - 1, colorLineaDefecto);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCedente, colCantidad, rowCedente, colCantidad, colorLineaDefecto);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCedente, colPeriodoIni, rowCedente, colPeriodoIni, colorLineaDefecto);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCedente, colPeriodoFin, rowCedente, colPeriodoFin, colorLineaDefecto);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCedente, colObservacion, rowCedente, colObservacion, colorLineaDefecto);

                rowData = rowCedente + 1;
                foreach (var reg in listaContrato)
                {
                    ws.Cells[rowData, colCedente].Value = $"{reg.Pfcontnombcedente} a {reg.Pfcontnombcesionario}";
                    ws.Cells[rowData, colCantidad].Value = reg.Pfcontcantidad;
                    ws.Cells[rowData, colPeriodoIni].Value = reg.PfcontvigenciainiDesc;
                    ws.Cells[rowData, colPeriodoFin].Value = reg.PfcontvigenciafinDesc;
                    ws.Cells[rowData, colObservacion].Value = reg.Pfcontobservacion;

                    UtilExcel.CeldasExcelAgrupar(ws, rowData, colCedente, rowData, colCantidad - 1);
                    UtilExcel.SetFormatoCelda(ws, rowData, colCedente, rowData, colCantidad - 1, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 7, false);
                    UtilExcel.SetFormatoCelda(ws, rowData, colCantidad, rowData, colCantidad, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 7, false);
                    UtilExcel.SetFormatoCelda(ws, rowData, colPeriodoIni, rowData, colPeriodoIni, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 7, false);
                    UtilExcel.SetFormatoCelda(ws, rowData, colPeriodoFin, rowData, colPeriodoFin, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 7, false);
                    UtilExcel.SetFormatoCelda(ws, rowData, colObservacion, rowData, colObservacion, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 7, false);

                    UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colCantidad, rowData, colCantidad, 2);

                    rowData = rowData + 1;
                }

                UtilExcel.BorderCeldasLineaDelgada(ws, rowCedente + 1, colCedente, rowData - 1, colCantidad - 1, colorLineaDefecto);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCedente + 1, colCantidad, rowData - 1, colCantidad, colorLineaDefecto);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCedente + 1, colPeriodoIni, rowData - 1, colPeriodoIni, colorLineaDefecto);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCedente + 1, colPeriodoFin, rowData - 1, colPeriodoFin, colorLineaDefecto);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowCedente + 1, colObservacion, rowData - 1, colObservacion, colorLineaDefecto);
            }

            #endregion

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            //filter

            //Todo el excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        /// <summary>
        /// Generar hoja excel cuadro detalle Fotuito y Programado
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIni"></param>
        /// <param name="colIni"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="tipoIndisp"></param>
        /// <param name="incluirDescripcion"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaReptHoja"></param>
        /// <param name="listaRepdetHojaInput"></param>
        private void GenerarHojaExcelCOG(ExcelPackage xlPackage, string nameWS, bool incluirDetalle, int rowIni, int colIni
                                                   , DateTime fechaIni, DateTime fechaFin, string informe, string nombreCuadro, string titulo, int mes
                                                    , string tituloDetalle, string subtituloDetalle
                                                    , List<PfReporteTotalDTO> listaReptHoja, List<MeMedicion96DTO> listaM96)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            ws.TabColor = ColorTranslator.FromHtml("#FFC000");

            string font = "Arial";
            string colorCeldaTextoDefecto = "#000000";
            string colorCeldaFondoDefecto = "#FFFFFF";
            string colorLineaDefecto = "#000000";

            #region  Filtros y Cabecera

            int colEmpresa = colIni + 1;
            int colCentral = colEmpresa + 1;
            int colPotProm = colCentral + 1;

            ws.Cells[rowIni, colPotProm].Value = informe; //COES/D/DO/STR-INF-063-2020
            UtilExcel.SetFormatoCelda(ws, rowIni, colPotProm, rowIni, colPotProm, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);

            ws.Cells[rowIni + 1, colIni].Value = nombreCuadro; //CUADRO N°  5A
            UtilExcel.SetFormatoCelda(ws, rowIni + 1, colIni, rowIni + 1, colIni, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, true);

            ws.Cells[rowIni + 2, colIni].Value = titulo; //POTENCIA FIRME DE CENTRALES DE COGENERACIÓN CALIFICADAS (PR-N° 26 numeral 8.2)
            UtilExcel.SetFormatoCelda(ws, rowIni + 2, colIni, rowIni + 2, colIni, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, true);

            ws.Cells[rowIni + 3, colIni].Value = EPDate.f_NombreMes(mes).ToUpper(); //NOVIEMBRE
            UtilExcel.SetFormatoCelda(ws, rowIni + 3, colIni, rowIni + 3, colIni, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, true);

            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 1, colIni, rowIni + 1, colPotProm + 1);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 2, colIni, rowIni + 2, colPotProm + 1);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 3, colIni, rowIni + 3, colPotProm + 1);


            int rowEmpresa = rowIni + 5;
            ws.Cells[rowEmpresa, colEmpresa].Value = "Empresa";
            ws.Cells[rowEmpresa, colCentral].Value = "Central";
            ws.Cells[rowEmpresa, colPotProm].Value = "Potencia Promedio \n (MW)";

            UtilExcel.SetFormatoCelda(ws, rowEmpresa, colEmpresa, rowEmpresa, colEmpresa, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
            UtilExcel.SetFormatoCelda(ws, rowEmpresa, colCentral, rowEmpresa, colCentral, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
            UtilExcel.SetFormatoCelda(ws, rowEmpresa, colPotProm, rowEmpresa, colPotProm, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false, true);

            UtilExcel.CeldasExcelAgrupar(ws, rowEmpresa, colEmpresa, rowEmpresa + 2, colEmpresa);
            UtilExcel.CeldasExcelAgrupar(ws, rowEmpresa, colCentral, rowEmpresa + 2, colCentral);
            UtilExcel.CeldasExcelAgrupar(ws, rowEmpresa, colPotProm, rowEmpresa + 2, colPotProm);

            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colEmpresa, rowEmpresa + 2, colEmpresa, colorLineaDefecto);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colCentral, rowEmpresa + 2, colCentral, colorLineaDefecto);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmpresa, colPotProm, rowEmpresa + 2, colPotProm, colorLineaDefecto);

            double factorAncho = 1;
            ws.Column(1).Width = 6 * factorAncho;
            ws.Column(colIni).Width = 8 * factorAncho;
            ws.Column(colEmpresa).Width = 35 * factorAncho;
            ws.Column(colCentral).Width = 27 * factorAncho;
            ws.Column(colPotProm).Width = 17 * factorAncho;

            #endregion

            #region Cuerpo Principal

            int rowData = rowEmpresa + 3;

            for (int i = 0; i < listaReptHoja.Count; i++)
            {
                var reg = listaReptHoja[i];

                ws.Cells[rowData, colEmpresa].Value = reg.Emprnomb;
                ws.Cells[rowData, colCentral].Value = reg.Central;

                UtilExcel.SetFormatoCelda(ws, rowData, colEmpresa, rowData, colCentral, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);

                if (reg.Pftotpprom > 0)
                {
                    ws.Cells[rowData, colPotProm].Value = reg.Pftotpprom;
                    UtilExcel.SetFormatoCelda(ws, rowData, colPotProm, rowData, colPotProm, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
                    UtilExcel.CeldasExcelIndentar(ws, rowData, colPotProm, rowData, colPotProm, 1);
                    UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colPotProm, rowData, colPotProm, 2);
                }
                else
                {
                    ws.Cells[rowData, colPotProm].Value = "-";
                    UtilExcel.SetFormatoCelda(ws, rowData, colPotProm, rowData, colPotProm, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
                    UtilExcel.CeldasExcelIndentar(ws, rowData, colPotProm, rowData, colPotProm, 2);
                }

                if (!string.IsNullOrEmpty(reg.ComentarioCalorUtil))
                {
                    UtilExcel.AgregarComentarioExcel(ws, rowData, colPotProm, reg.ComentarioCalorUtil);
                }

                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colEmpresa, rowData, colEmpresa, colorLineaDefecto);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colCentral, rowData, colCentral, colorLineaDefecto);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colPotProm, rowData, colPotProm, colorLineaDefecto);

                rowData++;
            }

            UtilExcel.BorderCeldasLineaGruesa(ws, rowEmpresa, colEmpresa, rowData - 1, colPotProm, colorLineaDefecto);

            #endregion

            if (incluirDetalle)
            {
                #region  Filtros y Cabecera

                int colDetalle = colIni + 7;
                int rowIniCabecera = rowIni + 5;

                ws.Cells[rowIni + 1, colDetalle].Value = tituloDetalle; //Potencia Firme de una Unidad de Central de Cogeneración Calificada
                UtilExcel.SetFormatoCelda(ws, rowIni + 1, colDetalle, rowIni + 1, colDetalle, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, true);

                ws.Cells[rowIni + 2, colDetalle].Value = subtituloDetalle; //PR-N° 26 numeral 8.2
                UtilExcel.SetFormatoCelda(ws, rowIni + 2, colDetalle, rowIni + 2, colDetalle, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, true);

                ws.Cells[rowIni + 7, colDetalle].Value = "Potencia Promedio";
                UtilExcel.SetFormatoCelda(ws, rowIni + 7, colDetalle, rowIni + 7, colDetalle, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, true);

                ws.Column(colDetalle).Width = 17 * factorAncho;

                int colCabecera = colDetalle + 1;
                foreach (var regCentral in listaReptHoja)
                {
                    ws.Cells[rowIniCabecera, colCabecera].Value = regCentral.Central;
                    ws.Cells[rowIniCabecera + 1, colCabecera].Value = regCentral.Pftotunidadnomb;
                    ws.Cells[rowIniCabecera + 2, colCabecera].Value = regCentral.Pftotpprom;

                    UtilExcel.SetFormatoCelda(ws, rowIniCabecera, colCabecera, rowIniCabecera, colCabecera, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, true);
                    UtilExcel.SetFormatoCelda(ws, rowIniCabecera + 1, colCabecera, rowIniCabecera + 1, colCabecera, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, true);
                    UtilExcel.SetFormatoCelda(ws, rowIniCabecera + 2, colCabecera, rowIniCabecera + 2, colCabecera, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, true);
                    UtilExcel.CeldasExcelFormatoNumero(ws, rowIniCabecera + 1, colCabecera, rowIniCabecera + 2, colCabecera, 2);

                    ws.Column(colCabecera).Width = 15 * factorAncho;

                    colCabecera++;
                }

                #endregion

                #region Cuerpo Detalle

                int row = rowIni + 9;

                for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
                {
                    //HORA
                    DateTime horas = day.AddMinutes(15);

                    for (int h = 1; h <= 96; h++)
                    {
                        ws.Cells[row, colDetalle].Value = horas.ToString(ConstantesAppServicio.FormatoFechaHora);
                        UtilExcel.SetFormatoCelda(ws, row, colDetalle, row, colDetalle, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 9, false);

                        int colData = colDetalle + 1;
                        foreach (var regCentral in listaReptHoja)
                        {
                            MeMedicion96DTO m96 = listaM96.Find(x => x.Equipadre == regCentral.Equipadre && x.Medifecha == day);
                            decimal? valor = m96 != null ? (decimal?)m96.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m96, null) : null;
                            ws.Cells[row, colData].Value = valor;
                            UtilExcel.CeldasExcelFormatoNumero(ws, row, colData, row, colData, 2);
                            UtilExcel.SetFormatoCelda(ws, row, colData, row, colData, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 9, false);

                            colData++;
                        }

                        horas = horas.AddMinutes(15);
                        row++;
                    }

                }

                #endregion
            }

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            //filter

            //Todo el excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        private void GenerarHojaExcelRerNC(ExcelPackage xlPackage, string nameWS, bool incluirDetalle, int rowIni, int colIni
                                                    , string informe, string nombreCuadro, string titulo, int anio, int mes, DateTime fechaIni, DateTime fechaFin
                                                    , List<PfReporteTotalDTO> listaReptHoja, List<PfReporteDetDTO> listaDetRer, List<SiParametroValorDTO> listaBloqueHorario)
        {
            //inicia C1

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            ws.TabColor = ColorTranslator.FromHtml("#FFC000");

            string font = "Arial";
            string colorCeldaTextoDefecto = "#000000";
            string colorCeldaTextoTotalMes = "#D9D9D9";
            string colorCeldaFondoDefecto = "#FFFFFF";

            ListarTiempoRer(fechaIni, fechaFin, listaBloqueHorario, out List<PfReporteDetDTO> listaAnio, out List<PfReporteDetDTO> listaMes, out List<PfReporteDetDTO> listaAllMes, out PfReporteDetDTO regEstadistico36Meses);

            List<PfReporteDetDTO> listaDetxAnio = listaDetRer.Where(x => x.Pfdettipo == ConstantesPotenciaFirme.TipoDetAnual).ToList();
            List<PfReporteDetDTO> listaDetxMes = listaDetRer.Where(x => x.Pfdettipo == ConstantesPotenciaFirme.TipoDetMensual).ToList();

            #region Titulo

            int colPoc = colIni - 2;
            int colEmpresa = colIni;
            int colCentral = colEmpresa + 1;
            int colEstadistico = colCentral + listaAnio.Count + 1;
            int colPF = colEstadistico + 1;

            ws.Cells[rowIni, colEstadistico].Value = informe;
            ws.Cells[rowIni + 1, colIni].Value = nombreCuadro;
            ws.Cells[rowIni + 2, colIni].Value = titulo;
            ws.Cells[rowIni + 3, colIni].Value = $"{EPDate.f_NombreMes(mes)} {anio}";

            UtilExcel.SetFormatoCelda(ws, rowIni + 1, colIni, rowIni + 3, colIni, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, true);
            UtilExcel.SetFormatoCelda(ws, rowIni, colEstadistico, rowIni, colEstadistico, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, true);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 1, colIni, rowIni + 1, colEstadistico);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 2, colIni, rowIni + 2, colEstadistico);
            UtilExcel.CeldasExcelAgrupar(ws, rowIni + 3, colIni, rowIni + 3, colEstadistico);

            int rowIniCabecera = rowIni + 5;
            if (incluirDetalle) ws.Cells[rowIniCabecera + 2, colPoc].Value = "POC";
            ws.Cells[rowIniCabecera, colEmpresa].Value = "Empresa";
            ws.Cells[rowIniCabecera, colCentral].Value = "Central";

            UtilExcel.CeldasExcelAgrupar(ws, rowIniCabecera, colEmpresa, rowIniCabecera + 2, colEmpresa);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniCabecera, colCentral, rowIniCabecera + 2, colCentral);

            UtilExcel.SetFormatoCelda(ws, rowIniCabecera, colEmpresa, rowIniCabecera, colEmpresa, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
            UtilExcel.SetFormatoCelda(ws, rowIniCabecera, colCentral, rowIniCabecera, colCentral, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);

            UtilExcel.BorderAround(ws.Cells[rowIniCabecera, colCentral, rowIniCabecera + 2, colCentral]);

            ws.Column(colPoc).Width = 10;
            ws.Column(colPoc + 1).Width = 1;
            ws.Column(colEmpresa).Width = 40;
            ws.Column(colCentral).Width = 30;
            ws.Column(colEstadistico).Width = 19;
            ws.Column(colPF).Width = 15;


            int colDinamica = colCentral;
            //36 meses, separados por 12 meses
            foreach (var item in listaAnio)
            {
                colDinamica++;
                ws.Cells[rowIniCabecera, colDinamica].Value = item.FechaDesc + " \n" + "(MWh)";
                ws.Cells[rowIniCabecera + 2, colDinamica].Value = item.TotalMes;

                //style
                UtilExcel.CeldasExcelAgrupar(ws, rowIniCabecera, colDinamica, rowIniCabecera + 1, colDinamica);
                UtilExcel.SetFormatoCelda(ws, rowIniCabecera, colDinamica, rowIniCabecera + 1, colDinamica, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, false, true);
                UtilExcel.SetFormatoCelda(ws, rowIniCabecera + 2, colDinamica, rowIniCabecera + 2, colDinamica, "Centro", "Centro", colorCeldaTextoTotalMes, colorCeldaFondoDefecto, font, 10, false, true);
                UtilExcel.BorderAround(ws.Cells[rowIniCabecera, colDinamica, rowIniCabecera + 1, colDinamica]);
                UtilExcel.BorderAround(ws.Cells[rowIniCabecera + 2, colDinamica, rowIniCabecera + 2, colDinamica]);

                ws.Column(colDinamica).Width = 25;
            }

            ws.Cells[rowIniCabecera, colEstadistico].Value = "Información estadistica \n movil 36 meses";
            ws.Cells[rowIniCabecera + 2, colEstadistico].Value = regEstadistico36Meses.TotalMes;
            ws.Cells[rowIniCabecera, colPF].Value = "Potencia Firme\n(MW)";

            UtilExcel.CeldasExcelAgrupar(ws, rowIniCabecera, colEstadistico, rowIniCabecera + 1, colEstadistico);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniCabecera, colPF, rowIniCabecera + 2, colPF);

            UtilExcel.SetFormatoCelda(ws, rowIniCabecera, colEstadistico, rowIniCabecera + 1, colEstadistico, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, false, true);
            UtilExcel.SetFormatoCelda(ws, rowIniCabecera + 2, colEstadistico, rowIniCabecera + 2, colEstadistico, "Centro", "Centro", colorCeldaTextoTotalMes, colorCeldaFondoDefecto, font, 10, false, true);
            UtilExcel.SetFormatoCelda(ws, rowIniCabecera, colPF, rowIniCabecera + 2, colPF, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false, true);

            UtilExcel.BorderAround(ws.Cells[rowIniCabecera, colEstadistico, rowIniCabecera + 1, colEstadistico]);
            UtilExcel.BorderAround(ws.Cells[rowIniCabecera + 2, colEstadistico, rowIniCabecera + 2, colEstadistico]);
            UtilExcel.BorderAround(ws.Cells[rowIniCabecera, colEmpresa, rowIniCabecera + 2, colPF], OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            UtilExcel.Alignment(ws.Cells[rowIniCabecera, colEmpresa, rowIniCabecera + 2, colPF]);


            int colIniDetalle = colPF + 3;
            int colDetDinamico = colIniDetalle;

            if (incluirDetalle)
            {
                ws.Cells[rowIni + 1, colIniDetalle].Value = titulo;
                ws.Cells[rowIniCabecera + 1, colIniDetalle].Value = "MWH";
                ws.Column(colDetDinamico).Width = 23;

                colDetDinamico++;
                foreach (var item in listaAllMes)
                {
                    ws.Cells[rowIniCabecera + 2, colDetDinamico].Value = new DateTime(item.Pfdetfechaini.Value.Year, item.Pfdetfechafin.Value.Month, 1);
                    ws.Cells[rowIniCabecera + 2, colDetDinamico].Style.Numberformat.Format = "mmm-yy";
                    UtilExcel.SetFormatoCelda(ws, rowIniCabecera + 2, colDetDinamico, rowIniCabecera + 2, colDetDinamico, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false, true);
                    ws.Column(colDetDinamico++).Width = 10;
                }
            }

            #endregion

            #region Cuerpo

            int rowIniCuerpo = rowIniCabecera + 3;
            colDetDinamico = colIniDetalle;
            colDinamica = colCentral;

            decimal totalPf = 0;

            foreach (var regCentral in listaReptHoja)
            {

                if (incluirDetalle)
                {
                    ws.Cells[rowIniCuerpo, colPoc].Value = regCentral.EquifechiniopcomDesc;
                    UtilExcel.SetFormatoCelda(ws, rowIniCuerpo, colPoc, rowIniCuerpo, colPoc, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, false);
                }

                ws.Cells[rowIniCuerpo, colEmpresa].Value = regCentral.Emprnomb;
                ws.Cells[rowIniCuerpo, colCentral].Value = regCentral.Central;
                UtilExcel.SetFormatoCelda(ws, rowIniCuerpo, colEmpresa, rowIniCuerpo, colCentral, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);

                foreach (var year in listaAnio)
                {
                    var regCentralxAnio = listaDetxAnio.Find(x => x.Equipadre == regCentral.Equipadre && x.Pfdetfechaini == year.Pfdetfechaini);

                    ws.Cells[rowIniCuerpo, ++colDinamica].Value = regCentralxAnio != null && regCentralxAnio.Pfdetenergia > 0 ? regCentralxAnio.Pfdetenergia : null;
                    UtilExcel.SetFormatoCelda(ws, rowIniCuerpo, colDinamica, rowIniCuerpo, colDinamica, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
                    UtilExcel.CeldasExcelFormatoNumero(ws, rowIniCuerpo, colDinamica, rowIniCuerpo, colDinamica, 2);
                }

                //Información estadistica 
                ws.Cells[rowIniCuerpo, colEstadistico].Value = regCentral.Pftotenergia;
                ws.Cells[rowIniCuerpo, colPF].Value = regCentral.Pftotpf;

                UtilExcel.SetFormatoCelda(ws, rowIniCuerpo, colEstadistico, rowIniCuerpo, colPF, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
                UtilExcel.CeldasExcelFormatoNumero(ws, rowIniCuerpo, colEstadistico, rowIniCuerpo, colPF, 2);

                totalPf += regCentral.Pftotpf.GetValueOrDefault(0);

                if (incluirDetalle)
                {
                    ws.Cells[rowIniCuerpo, colDetDinamico].Value = regCentral.Central;
                    UtilExcel.SetFormatoCelda(ws, rowIniCuerpo, colDetDinamico, rowIniCuerpo, colDetDinamico, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);

                    //ws.Cells[rowIniCuerpo + 25, colDetDinamico].Value = regCentral.Central;

                    foreach (var itemDet in listaAllMes)
                    {
                        var regCentralxMes = listaDetxMes.Find(x => x.Equipadre == regCentral.Equipadre && x.Pfdetfechaini.Value.Year == itemDet.Pfdetfechaini.Value.Year && x.Pfdetfechaini.Value.Month == itemDet.Pfdetfechaini.Value.Month);

                        ws.Cells[rowIniCuerpo, ++colDetDinamico].Value = regCentralxMes != null && regCentralxMes.Pfdetenergia.GetValueOrDefault(0) != 0 ? regCentralxMes.Pfdetenergia : null;
                        UtilExcel.SetFormatoCelda(ws, rowIniCuerpo, colDetDinamico, rowIniCuerpo, colDetDinamico, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
                        UtilExcel.CeldasExcelFormatoNumero(ws, rowIniCuerpo, colDetDinamico, rowIniCuerpo, colDetDinamico, 2);

                        //if (regCentralxMes != null)  ws.Cells[rowIniCuerpo + 25, colDetDinamico].Value = regCentralxMes.Pfdetcodi;
                    }
                }

                colDetDinamico = colIniDetalle;
                colDinamica = colCentral;
                rowIniCuerpo++;
            }

            UtilExcel.AllBorders(ws.Cells[rowIniCabecera + 1, colEmpresa, rowIniCuerpo - 1, colPF]);
            UtilExcel.BorderAround(ws.Cells[rowIniCabecera + 1, colEmpresa, rowIniCuerpo - 1, colPF], OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //Total
            ws.Cells[rowIniCuerpo, colEstadistico].Value = "TOTAL";
            ws.Cells[rowIniCuerpo, colPF].Value = totalPf;

            UtilExcel.SetFormatoCelda(ws, rowIniCuerpo, colEstadistico, rowIniCuerpo, colEstadistico, "Centro", "Centro", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
            UtilExcel.SetFormatoCelda(ws, rowIniCuerpo, colPF, rowIniCuerpo, colPF, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
            UtilExcel.CeldasExcelFormatoNumero(ws, rowIniCuerpo, colPF, rowIniCuerpo, colPF, 2);

            UtilExcel.AllBorders(ws.Cells[rowIniCuerpo, colEstadistico, rowIniCuerpo, colPF]);
            UtilExcel.BorderAround(ws.Cells[rowIniCuerpo, colEstadistico, rowIniCuerpo, colPF], OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            #endregion

            #region Cuerpo Numero días y horas

            var rowIniCuerpo2 = rowIniCuerpo + 4;
            int colDinamica2 = colCentral;
            foreach (var item in listaAnio)
            {
                ws.Cells[rowIniCuerpo2, colCentral].Value = "Dias";
                ws.Cells[rowIniCuerpo2 + 1, colCentral].Value = "Horas Punta";

                ws.Cells[rowIniCuerpo2, ++colDinamica2].Value = item.TotalDias;
                ws.Cells[rowIniCuerpo2 + 1, colDinamica2].Value = item.TotalHP;
            }
            ws.Cells[rowIniCuerpo2, ++colDinamica2].Value = regEstadistico36Meses.TotalDias;
            ws.Cells[rowIniCuerpo2 + 1, colDinamica2].Value = regEstadistico36Meses.TotalHP;

            UtilExcel.SetFormatoCelda(ws, rowIniCuerpo2, colCentral, rowIniCuerpo2 + 1, colCentral, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
            UtilExcel.SetFormatoCelda(ws, rowIniCuerpo2, colCentral + 1, rowIniCuerpo2 + 1, colEstadistico, "Centro", "Derecha", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 10, false);
            UtilExcel.AllBorders(ws.Cells[rowIniCuerpo2, colCentral, rowIniCuerpo2 + 1, colEstadistico]);
            UtilExcel.BorderAround(ws.Cells[rowIniCuerpo2, colCentral, rowIniCuerpo2 + 1, colEstadistico], OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            #endregion

            #region Centrales sin 36 meses

            rowIniCuerpo2 += 3;
            ws.Cells[rowIniCuerpo2, colEmpresa].Value = "(*) Se considera la información a partir de su operación comercial:";
            UtilExcel.SetFormatoCelda(ws, rowIniCuerpo2, colEmpresa, rowIniCuerpo2, colEmpresa, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, false);

            var listaSin36Meses = listaReptHoja.Where(x => !x.Tiene36Meses).ToList();
            rowIniCuerpo2++;
            foreach (var reg in listaSin36Meses)
            {
                ws.Cells[rowIniCuerpo2, colEmpresa].Value = reg.Central + ": " + reg.EquifechiniopcomDesc;
                UtilExcel.SetFormatoCelda(ws, rowIniCuerpo2, colEmpresa, rowIniCuerpo2, colEmpresa, "Centro", "Izquierda", colorCeldaTextoDefecto, colorCeldaFondoDefecto, font, 8, false);
                UtilExcel.CeldasExcelIndentar(ws, rowIniCuerpo2, colEmpresa, rowIniCuerpo2, colEmpresa, 1);
                rowIniCuerpo2++;
            }

            #endregion


            ws.View.ShowGridLines = false;
        }

        public int GetUltimoPfrptcodiXRecalculo(int pfrecacodi, int cuadro)
        {
            if (pfrecacodi > 0)
            {
                List<PfReporteDTO> lista = GetByCriteriaPfReportes(pfrecacodi, cuadro).OrderByDescending(x => x.Pfrptcodi).ToList();
                if (lista.Any())
                    return lista.First().Pfrptcodi;
            }
            return 0;
        }

        #endregion

        #region Calcular Potencia firme

        /// <summary>
        /// Método que hace el cálulo de la potencia Firme
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="recacodi"></param>
        /// <param name="versionPG"></param>
        /// <param name="versionPA"></param>
        /// <param name="versionCU"></param>
        /// <param name="versionccv"></param>
        /// <param name="versionFI"></param>
        /// <param name="versionFP"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int CalcularReportePFirmeTransaccional(int recacodi, int versionPG, int versionPA, int versionFI, int versionFP, int versionCCV, string usuario)
        {
            PfRecalculoDTO regRecalculo = GetByIdPfRecalculo(recacodi);
            PfPeriodoDTO regPeriodo = GetByIdPfPeriodo(regRecalculo.Pfpericodi);

            DateTime fechaIni = new DateTime(regPeriodo.Pfperianio, regPeriodo.Pfperimes, 1);
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);
            DateTime fechaRegistro = DateTime.Now;

            #region Registrar información de RER NC

            //
            DateTime fechaFinHistRER = DateTime.ParseExact(ConstantesPotenciaFirme.MesFinHistoricoRER, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
            if (fechaFinHistRER < fechaIni)
                ProcesarRERMensual(fechaIni);

            PfReporteDTO regReporteNC = new PfReporteDTO()
            {
                Pfcuacodi = ConstantesPotenciaFirme.CuadroRerNC,
                Pfrecacodi = recacodi,
                Pfrptesfinal = ConstantesPotenciaFirme.EsVersionValidado,
                Pfrptusucreacion = usuario,
                Pfrptfeccreacion = fechaRegistro,
            };

            this.ObtenerReporteTotalPFRer(fechaIni, fechaFin, out List<PfReporteTotalDTO> listaPFRer, out List<PfReporteDetDTO> listaRerDet);

            PfEscenarioDTO objEscenarioRer = new PfEscenarioDTO()
            {
                Pfescefecini = fechaIni,
                Pfescefecfin = fechaFin,
                Pfescedescripcion = GetDescripcionEscenario(fechaIni, fechaFin),
            };

            objEscenarioRer.ListaPfReporteTotal = listaPFRer;

            regReporteNC.ListaPfEscenario = new List<PfEscenarioDTO>();
            regReporteNC.ListaPfEscenario.Add(objEscenarioRer);

            //Funcion transaccional para guardar en BD
            GuardarReportePotFirmeBDTransaccional(regReporteNC, new List<int>(), new List<int>());

            #endregion

            //Agregar las versiones de los insumos para poder guardarlas posteriormente
            List<int> lstVersiones = new List<int>();
            lstVersiones.Add(versionPG);
            lstVersiones.Add(versionPA);
            lstVersiones.Add(versionFI);
            lstVersiones.Add(versionFP);
            lstVersiones.Add(versionCCV);

            //agregar reportes de factores de indisponibilidad
            servIndisp.GetCodigoReporteAprobadoXCuadro(regRecalculo.Irecacodi, ConstantesIndisponibilidades.ReportePR25FactorProgHidro, out int rptcodi10, out int numVersion10, out string mensaje10);
            servIndisp.GetCodigoReporteAprobadoXCuadro(regRecalculo.Irecacodi, ConstantesIndisponibilidades.ReportePR25FactorProgTermico, out int rptcodi9, out int numVersion9, out string mensaje9);
            servIndisp.GetCodigoReporteAprobadoXCuadro(regRecalculo.Irecacodi, ConstantesIndisponibilidades.ReportePR25DisponibilidadCalorUtil, out int rptcodiCU, out int numVersion14, out string mensaje14);

            if (rptcodi10 <= 0)
            {
                throw new ArgumentException(mensaje10);
            }
            if (rptcodi9 <= 0)
            {
                throw new ArgumentException(mensaje9);
            }
            if (rptcodiCU <= 0)
            {
                throw new ArgumentException(mensaje14);
            }

            PfVersionDTO regPfVersionFI = GetByIdPfVersion(versionFI);
            PfVersionDTO regPfVersionFP = GetByIdPfVersion(versionFP);

            List<int> listaIrptcodis = new List<int>();
            listaIrptcodis.Add(rptcodi10);
            listaIrptcodis.Add(rptcodi9);
            listaIrptcodis.Add(rptcodiCU);
            listaIrptcodis.Add(regPfVersionFI.Irptcodi.Value);
            listaIrptcodis.Add(regPfVersionFP.Irptcodi.Value);

            //
            PfReporteDTO regReporte = new PfReporteDTO()
            {
                Pfcuacodi = ConstantesPotenciaFirme.CuadroPFirme,
                Pfrecacodi = recacodi,
                Pfrptesfinal = ConstantesPotenciaFirme.EsVersionGenerado,
                Pfrptusucreacion = usuario,
                Pfrptfeccreacion = fechaRegistro
            };
            int perianio = regPeriodo.Pfperianio;
            int perimes = regPeriodo.Pfperimes;
            List<PfEscenarioDTO> lista = ObtenerEscenariosPF(perianio, perimes, fechaIni, fechaFin);

            regReporte.ListaPfEscenario = lista;

            // llamar métodos de insumos 
            servIndisp.CalculoPotenciaFirmeCogeneracion(rptcodiCU, fechaIni, fechaFin, out List<PfReporteTotalDTO> listaPFcog);

            var InsumoPG = this.GetListaPFHidroTermicasFactores(versionPG, ConstantesPotenciaFirme.RecursoPGarantizada);
            var InsumoPA = this.GetListaPFHidroTermicasFactores(versionPA, ConstantesPotenciaFirme.RecursoPAdicional);
            var InsumoFI = this.GetListaPFHidroTermicasFactores(versionFI, ConstantesPotenciaFirme.RecursoFactorIndispFortuita);
            var InsumoFP = this.GetListaPFHidroTermicasFactores(versionFP, ConstantesPotenciaFirme.RecursoFactorPresencia);

            //Obtener potencia efectiva
            int aplicativo = ConstantesIndisponibilidades.AppPF;
            servIndisp.ListarUnidadTermicoOpComercial(aplicativo, fechaIni, fechaFin, out List<EqEquipoDTO> listaUnidadesTermoPe, out List<EqEquipoDTO> listaEquiposTermicos, out List<ResultadoValidacionAplicativo> listaMsj4);

            foreach (var escenario in regReporte.ListaPfEscenario)
            {
                //obtener la lista de unidades para ese escenario
                servIndisp.ListarEqCentralSolarOpComercial(escenario.Pfescefecini, escenario.Pfescefecfin, out List<EqEquipoDTO> listaCentrales1, out List<EqEquipoDTO> istaAllEquipos1, out List<ResultadoValidacionAplicativo> istaMsj1);
                servIndisp.ListarEqCentralEolicaOpComercial(escenario.Pfescefecini, escenario.Pfescefecfin, out List<EqEquipoDTO> listaCentrales2, out List<EqEquipoDTO> istaAllEquipos2, out List<ResultadoValidacionAplicativo> istaMsj2);
                servIndisp.ListarEqCentralHidraulicoOpComercial(escenario.Pfescefecini, escenario.Pfescefecfin, out List<EqEquipoDTO> listaCentrales3, out List<EqEquipoDTO> istaEquiposHidro, out List<ResultadoValidacionAplicativo> istaMsj3);
                servIndisp.ListarUnidadTermicoOpComercial(aplicativo, escenario.Pfescefecini, escenario.Pfescefecfin, out List<EqEquipoDTO> listaUnidadesTermo, out List<EqEquipoDTO> istaEquiposTermicos, out List<ResultadoValidacionAplicativo> istaMsj4);

                List<EqEquipoDTO> listaunidades = new List<EqEquipoDTO>();
                listaunidades.AddRange(listaCentrales1);
                listaunidades.AddRange(listaCentrales2);
                listaunidades.AddRange(listaCentrales3);
                listaunidades.AddRange(listaUnidadesTermo);

                var listaReporteTotal = new List<PfReporteTotalDTO>();
                foreach (var unidad in listaunidades)
                {
                    decimal valPFirme;
                    decimal? pe;

                    PfReporteTotalDTO reporteTotal = new PfReporteTotalDTO();
                    reporteTotal.Famcodi = unidad.Famcodi ?? 0;
                    reporteTotal.Emprcodi = unidad.Emprcodi.Value;
                    reporteTotal.Equipadre = unidad.Equipadre.Value;
                    reporteTotal.Equicodi = unidad.Equicodi;
                    reporteTotal.Grupocodi = unidad.Grupocodi;
                    reporteTotal.Pftotpe = unidad.Pe; //eolica, solar y del nodo energetico/reserva fria tienen su propia potencia efectiva
                    reporteTotal.Pftotunidadnomb = unidad.Equinomb;
                    reporteTotal.Pftotincremental = unidad.Grupoincremental;

                    switch (unidad.Famcodi)
                    {
                        case ConstantesHorasOperacion.IdTipoHidraulica://Hidro
                            reporteTotal.Pftotunidadnomb = reporteTotal.Pftotunidadnomb.Trim().Replace("+", "/");

                            var pgHidro = InsumoPG.Find(x => x.Equipadre == unidad.Equipadre);
                            var pFFactorPresencia = InsumoFP.Find(x => x.Equipadre == unidad.Equipadre);

                            if (pgHidro != null && pFFactorPresencia != null)
                            {
                                pe = pgHidro.Pftotpe;
                                reporteTotal.Pftotpe = pe;

                                valPFirme = pgHidro.Pftotpg.GetValueOrDefault(0) * pFFactorPresencia.Pftotfp.GetValueOrDefault(0);
                                reporteTotal.Pftotpf = valPFirme;
                                reporteTotal.Pftotpg = pgHidro.Pftotpg.GetValueOrDefault(0);
                                reporteTotal.Pftotfp = pFFactorPresencia.Pftotfp.GetValueOrDefault(0);
                            }

                            listaReporteTotal.Add(reporteTotal);

                            break;
                        case ConstantesHorasOperacion.IdTipoTermica://Termo
                        case ConstantesHorasOperacion.IdGeneradorTemoelectrico://Termo
                            pe = listaUnidadesTermoPe.Find(x => x.Grupocodi == unidad.Grupocodi && x.Equicodi == unidad.Equicodi)?.Pe;
                            reporteTotal.Pftotpe = pe;

                            reporteTotal.Pftotunidadnomb = unidad.UnidadnombPR25;

                            if (unidad.Grupotipocogen == ConstantesAppServicio.SI)
                            {
                                var pFTermoCoGen = listaPFcog.Find(x => x.Equipadre == unidad.Equipadre);
                                var pFFactorIndisp = InsumoFI.Find(x => x.Grupocodi == unidad.Grupocodi && x.Equicodi == unidad.Equicodi);

                                if (pFTermoCoGen != null && pFFactorIndisp != null)
                                {
                                    //valPFirme = (pFTermoCoGen.PotenciaPromedio.GetValueOrDefault(0) * pFTermoCoGen.TotalMinutosCalorUtil) + (reporteTotal.Pftotpe.GetValueOrDefault(0) * pFTermoCoGen.TotalMinutosSinCalorUtil);
                                    valPFirme = (pFTermoCoGen.PotenciaPromedio.GetValueOrDefault(0) * pFTermoCoGen.TotalMinutosCalorUtil) + (reporteTotal.Pftotpe.GetValueOrDefault(0) * (1 - pFFactorIndisp.Pftotfi.GetValueOrDefault(0)) * pFTermoCoGen.TotalMinutosSinCalorUtil);
                                    valPFirme = valPFirme / pFTermoCoGen.TotalMinutosPeriodo;

                                    reporteTotal.Pftotpf = valPFirme;
                                    reporteTotal.Pftotfi = pFFactorIndisp.Pftotfi;
                                    reporteTotal.Pftotpprom = pFTermoCoGen.PotenciaPromedio;
                                    reporteTotal.Pftotminsincu = pFTermoCoGen.TotalMinutosSinCalorUtil;

                                    listaReporteTotal.Add(reporteTotal);
                                }
                            }
                            else
                            {
                                if (unidad.Gruporeservafria == 1 || unidad.Gruponodoenergetico == 1)
                                {
                                    var pFAdicional = InsumoPA.Find(x => x.Grupocodi == unidad.Grupocodi && x.Equicodi == unidad.Equicodi);

                                    if (pFAdicional != null)
                                    {
                                        reporteTotal.Pftotpe = pFAdicional.Pftotpe;

                                        if (reporteTotal.Pftotincremental == 1)
                                        {
                                            var pFFactorIndisp = InsumoFI.Find(x => x.Grupocodi == unidad.Grupocodi && x.Equicodi == unidad.Equicodi);
                                            if (pFFactorIndisp != null)
                                            {
                                                valPFirme = reporteTotal.Pftotpe.GetValueOrDefault(0) * (1 - pFFactorIndisp.Pftotfi.GetValueOrDefault(0));

                                                reporteTotal.Pftotfi = pFFactorIndisp.Pftotfi;
                                                reporteTotal.Pftotpf = valPFirme;
                                            }
                                        }
                                        else
                                        {
                                            reporteTotal.Pftotfi = null;
                                            reporteTotal.Pftotpf = pFAdicional.Pftotpf;
                                        }
                                    }

                                    if (reporteTotal.Pftotpf.GetValueOrDefault(0) > 0)
                                        listaReporteTotal.Add(reporteTotal);
                                }
                                else
                                {
                                    var pFFactorIndisp = InsumoFI.Find(x => x.Grupocodi == unidad.Grupocodi && x.Equicodi == unidad.Equicodi);
                                    if (pFFactorIndisp != null)
                                    {
                                        valPFirme = pe.GetValueOrDefault(0) * (1 - pFFactorIndisp.Pftotfi.GetValueOrDefault(0));
                                        reporteTotal.Pftotpf = valPFirme;
                                        reporteTotal.Pftotfi = pFFactorIndisp.Pftotfi;
                                        reporteTotal.Pftotincremental = pFFactorIndisp.Pftotincremental;

                                        listaReporteTotal.Add(reporteTotal);
                                    }
                                }
                            }
                            break;
                        case ConstantesHorasOperacion.IdTipoSolar://Solar
                            reporteTotal.Pftotunidadnomb = string.Empty;

                            var pFSolar = listaPFRer.Find(x => x.Equipadre == unidad.Equipadre);

                            if (pFSolar != null)
                            {
                                valPFirme = pFSolar.Pftotpf.GetValueOrDefault(0);
                                reporteTotal.Pftotpe = valPFirme;
                                reporteTotal.Pftotpf = valPFirme;
                            }

                            listaReporteTotal.Add(reporteTotal);

                            break;
                        case ConstantesHorasOperacion.IdTipoEolica://Eólico
                            reporteTotal.Pftotunidadnomb = string.Empty;

                            var pFEolico = listaPFRer.Find(x => x.Equipadre == unidad.Equipadre);

                            if (pFEolico != null)
                            {
                                valPFirme = pFEolico.Pftotpf.GetValueOrDefault(0);
                                reporteTotal.Pftotpe = valPFirme;
                                reporteTotal.Pftotpf = valPFirme;
                            }

                            listaReporteTotal.Add(reporteTotal);

                            break;
                    }
                }

                escenario.ListaPfReporteTotal = listaReporteTotal;
            }

            //Funcion transaccional para guardar en BD
            return this.GuardarReportePotFirmeBDTransaccional(regReporte, lstVersiones, listaIrptcodis);
        }

        public List<PfEscenarioDTO> ObtenerEscenariosPF(int perianio, int perimes, DateTime fechaIni, DateTime fechaFin)
        {
            List<int> listaFamcodi = new List<int>() { ConstantesHorasOperacion.IdGeneradorTemoelectrico, ConstantesHorasOperacion.IdTipoHidraulica
                                                        , ConstantesHorasOperacion.IdTipoSolar, ConstantesHorasOperacion.IdTipoEolica };

            List<EqEquipoDTO> listaAllEquipos = _equipamientoAppServicio.ListarEquiposTienenOpComercial(fechaIni, fechaFin
                                                                , string.Join(",", listaFamcodi), out List<ResultadoValidacionAplicativo> listaMsjEq);
            listaAllEquipos = listaAllEquipos.Where(x => (x.Equifechiniopcom > fechaIni && x.Equifechiniopcom <= fechaFin)
                                                    || (x.Equifechfinopcom > fechaIni && x.Equifechfinopcom <= fechaFin)).ToList();

            var escenariosEntrada = listaAllEquipos.Select(x => x.Equifechiniopcom).Distinct().OrderBy(x => x).ToList();
            var escenariosSalida = listaAllEquipos.Select(x => x.Equifechfinopcom).Distinct().OrderBy(x => x).ToList();
            escenariosEntrada.AddRange(escenariosSalida);

            var listaFinalEscenarios = escenariosEntrada.Where(x => x != null).Distinct().OrderBy(x => x).ToList();
            listaFinalEscenarios = listaFinalEscenarios.Where(x => fechaIni < x && x <= fechaFin).ToList();

            int numEscenarios = listaFinalEscenarios.Count + 1;
            var inicioDia = new DateTime(perianio, perimes, 1);
            var lista = new List<PfEscenarioDTO>();
            for (var item = 0; item < numEscenarios; item++)
            {
                var fechEscenario = listaFinalEscenarios.Count != 0 && item < numEscenarios - 1 ? listaFinalEscenarios[item] : null;
                var objEscenario = new PfEscenarioDTO()
                {
                    Pfescefecini = inicioDia,
                    Pfescefecfin = fechEscenario != null ? fechEscenario.Value.AddDays(-1) : fechaFin,
                    Pfescedescripcion = "escenario" + (item + 1),
                };
                if (fechEscenario != null)
                    inicioDia = fechEscenario.Value;

                lista.Add(objEscenario);
            }

            return lista;
        }

        /// <summary>
        /// Método que guarda el cálulo de POTENCIA fIRME
        /// </summary>
        /// <param name="regReporte"></param>
        /// <returns></returns>
        private int GuardarReportePotFirmeBDTransaccional(PfReporteDTO regReporte, List<int> lstVersiones, List<int> listaIrptcodis)
        {
            int pfrptcodi = -1;
            int pfescecodi = -1;

            int pos = 0;

            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = (DbTransaction)UoW.StartTransaction(connection))
                {
                    try
                    {
                        pfrptcodi = FactorySic.GetPfReporteRepository().Save(regReporte, connection, transaction);

                        foreach (PfEscenarioDTO escenario in regReporte.ListaPfEscenario)
                        {
                            escenario.Pfrptcodi = pfrptcodi;

                            pfescecodi = FactorySic.GetPfEscenarioRepository().Save(escenario, connection, transaction);

                            foreach (PfReporteTotalDTO reporteTotal in escenario.ListaPfReporteTotal)
                            {
                                reporteTotal.Pfescecodi = pfescecodi;
                                int pftotcodi = FactorySic.GetPfReporteTotalRepository().Save(reporteTotal, connection, transaction);

                                pos++;

                                if (reporteTotal.ListaDetalle != null)
                                {
                                    foreach (PfReporteDetDTO regDet in reporteTotal.ListaDetalle)
                                    {
                                        regDet.Pftotcodi = pftotcodi;

                                        FactorySic.GetPfReporteDetRepository().Save(regDet, connection, transaction);
                                    }
                                }
                            }
                        }

                        //guardar tabla relación insumos
                        foreach (var item in lstVersiones)
                        {
                            PfRelacionInsumosDTO relacionInsumo = new PfRelacionInsumosDTO()
                            {
                                Pfrptcodi = pfrptcodi,
                                Pfverscodi = item
                            };
                            this.SavePfRelacionInsumos(relacionInsumo, connection, transaction);
                        }

                        //guardar tabla relación factores de indisponibilidad
                        foreach (var item in listaIrptcodis)
                        {
                            PfRelacionIndisponibilidadesDTO relacionInd = new PfRelacionIndisponibilidadesDTO()
                            {
                                Pfrptcodi = pfrptcodi,
                                Irptcodi = item
                            };
                            this.SavePfRelacionIndisponibilidades(relacionInd, connection, transaction);
                        }

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

            return pfrptcodi;
        }

        /// <summary>
        /// Editar la potencia firme
        /// </summary>
        /// <param name="pfrptcodi"></param>
        /// <param name="listaPFEdicion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int EditarReportePFirmeTransaccional(int pfrptcodi, List<PfReporteTotalDTO> listaPFEdicion, string usuario)
        {
            //obtener la información que se guardó anteriormente para el reporte editado
            PfReporteDTO regReporteBD = GetByIdPfReporte(pfrptcodi);

            PfReporteDTO regReporte = new PfReporteDTO()
            {
                Pfcuacodi = ConstantesPotenciaFirme.CuadroPFirme,
                Pfrecacodi = regReporteBD.Pfrecacodi,
                Pfrptesfinal = ConstantesPotenciaFirme.EsVersionGenerado,
                Pfrptusucreacion = usuario,
                Pfrptfeccreacion = DateTime.Now
            };

            //lista de escenarios
            List<PfEscenarioDTO> listaEscenario = GetByCriteriaPfEscenarios(pfrptcodi).OrderBy(x => x.Pfescefecini).ToList();
            int i = 1;
            foreach (var reg in listaEscenario)
            {
                reg.Numero = i;
                i++;
            }
            regReporte.ListaPfEscenario = listaEscenario;

            //lista de data REPORTE_TOTAL
            List<PfReporteTotalDTO> listaBD = GetByCriteriaPfReporteTotals(pfrptcodi.ToString());

            //generar la lista de unidades de todo el periodo
            List<PfReporteTotalDTO> listaPF = listaBD.GroupBy(x => new { x.Equicodi, x.Grupocodi }).Select(x => new PfReporteTotalDTO()
            {
                Emprcodi = x.First().Emprcodi,
                Emprnomb = x.First().Emprnomb,
                Equipadre = x.First().Equipadre,
                Central = x.First().Central,
                Equicodi = x.Key.Equicodi,
                Grupocodi = x.Key.Grupocodi,
                Famcodi = x.First().Famcodi,
                Pftotunidadnomb = x.First().Pftotunidadnomb,
                Pftotpe = x.First().Pftotpe,
                Pftotpprom = x.First().Pftotpprom,
                Pftotfi = x.First().Pftotfi,
                Pftotfp = x.First().Pftotfp,
                Pftotpg = x.First().Pftotpg,
                Grupotipocogen = x.First().Grupotipocogen,
                Pftotincremental = x.First().Pftotincremental
            }).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Pftotunidadnomb).ToList();

            foreach (var regEsc in listaEscenario)
            {
                regEsc.ListaPfReporteTotal = new List<PfReporteTotalDTO>();
                foreach (var regUnidad in listaPF)
                {
                    var regEdicion = listaPFEdicion.Find(x => x.Equicodi == regUnidad.Equicodi && x.Grupocodi == regUnidad.Grupocodi);
                    if (regEdicion != null)
                    {
                        regUnidad.Pftotpf = ((decimal?)regEdicion.GetType().GetProperty(ConstantesPotenciaFirme.PrefijoPftotpf + regEsc.Numero.ToString()).GetValue(regEdicion, null)).GetValueOrDefault(0);

                        PfReporteTotalDTO regReporteTotal = new PfReporteTotalDTO()
                        {
                            Emprcodi = regUnidad.Emprcodi,
                            Emprnomb = regUnidad.Emprnomb,
                            Equipadre = regUnidad.Equipadre,
                            Central = regUnidad.Central,
                            Equicodi = regUnidad.Equicodi,
                            Grupocodi = regUnidad.Grupocodi,
                            Famcodi = regUnidad.Famcodi,
                            Pftotunidadnomb = regUnidad.Pftotunidadnomb,
                            Pftotpe = regUnidad.Pftotpe,
                            Pftotpprom = regUnidad.Pftotpprom,
                            Pftotfi = regUnidad.Pftotfi,
                            Pftotfp = regUnidad.Pftotfp,
                            Pftotpg = regUnidad.Pftotpg,
                            Pftotpf = regUnidad.Pftotpf
                        };
                        regEsc.ListaPfReporteTotal.Add(regReporteTotal);
                    }
                }
            }

            //obtener lista de relación insumos del reporrte editado
            var lstVersiones = this.GetByCriteriaPfRelacionInsumoss(pfrptcodi).Select(x => x.Pfverscodi).ToList();
            var listaIrptcodis = this.GetByCriteriaPfRelacionIndisponibilidadess(pfrptcodi).Select(x => x.Irptcodi).ToList();

            //Funcion transaccional para guardar en BD
            return this.GuardarReportePotFirmeBDTransaccional(regReporte, lstVersiones, listaIrptcodis);
        }

        /// <summary>
        /// Obtiene los insumos formateados en REPORTE TOTAL
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="versionId"></param>
        /// <param name="tipoInsumo"></param>
        /// <returns></returns>
        private List<PfReporteTotalDTO> GetListaPFHidroTermicasFactores(int versionId, int tipoInsumo)
        {
            var lista = new List<PfReporteTotalDTO>();

            var version = this.GetByIdPfVersion(versionId);
            var recalculo = this.GetByIdPfRecalculo(version.Pfrecacodi);
            var periodo = this.GetByIdPfPeriodo(recalculo.Pfpericodi);
            var emprcodi = ConstantesPotenciaFirme.ParametroDefecto;
            var centralId = ConstantesPotenciaFirme.ParametroDefecto;

            if (tipoInsumo == ConstantesPotenciaFirme.RecursoPGarantizada)
            {
                this.ListarPotenciaGarantizada(version.Pfrecacodi, versionId, emprcodi, centralId, out List<PfPotenciaGarantizadaDTO> lstPGarantizadas, out PfVersionDTO pfVersionRecurso1);

                foreach (var item in lstPGarantizadas)
                {
                    var objUnidad = new PfReporteTotalDTO()
                    {
                        Emprcodi = item.Emprcodi.Value,
                        Equipadre = item.Equipadre.Value,
                        Grupocodi = item.Grupocodi,
                        Equicodi = item.Equicodi,
                        Pftotpe = item.Pfpgarpe,
                        Pftotpg = item.Pfpgarpg,
                        Pftotunidadnomb = item.Pfpgarunidadnomb
                    };

                    lista.Add(objUnidad);
                }
            }

            if (tipoInsumo == ConstantesPotenciaFirme.RecursoPAdicional)
            {
                this.ListarPotenciaAdicional(version.Pfrecacodi, versionId, version.Irptcodi.Value, emprcodi, centralId, out List<PfPotenciaAdicionalDTO> listadoPA, out PfVersionDTO pfVersionRecurso);

                foreach (var item in listadoPA)
                {
                    var objUnidad = new PfReporteTotalDTO()
                    {
                        Emprcodi = item.Emprcodi,
                        Equipadre = item.Equipadre,
                        Grupocodi = item.Grupocodi,
                        Famcodi = item.Famcodi,
                        Equicodi = item.Equicodi,
                        Pftotpe = item.Pfpadipe,
                        Pftotfi = item.Pfpadifi,
                        Pftotpf = item.Pfpadipf,
                        Pftotunidadnomb = item.Pfpadiunidadnomb,
                        Pftotincremental = item.Pfpadiincremental
                    };
                    lista.Add(objUnidad);
                }
            }

            if (tipoInsumo == ConstantesPotenciaFirme.RecursoFactorIndispFortuita)
            {
                this.ListarFactores(ConstantesPotenciaFirme.FactorIndispFortuita, version.Pfrecacodi, versionId, version.Irptcodi.Value, emprcodi, centralId, out List<PfFactoresDTO> listadoFI, out PfVersionDTO pfVersionRecurso3);

                foreach (var item in listadoFI)
                {
                    var objUnidad = new PfReporteTotalDTO()
                    {
                        Emprcodi = item.Emprcodi,
                        Equipadre = item.Equipadre,
                        Grupocodi = item.Grupocodi,
                        Famcodi = item.Famcodi,
                        Equicodi = item.Equicodi,
                        Pftotfi = item.Pffactfi,
                        Pftotunidadnomb = item.Pffactunidadnomb,
                        Pftotincremental = item.Pffactincremental
                    };
                    lista.Add(objUnidad);
                }
            }

            if (tipoInsumo == ConstantesPotenciaFirme.RecursoFactorPresencia)
            {
                this.ListarFactores(ConstantesPotenciaFirme.FactorPresencia, version.Pfrecacodi, versionId, version.Irptcodi.Value, emprcodi, centralId, out List<PfFactoresDTO> listadoFP, out PfVersionDTO pfVersionRecurso3);

                foreach (var item in listadoFP)
                {
                    var objUnidad = new PfReporteTotalDTO()
                    {
                        Emprcodi = item.Emprcodi,
                        Equipadre = item.Equipadre,
                        Grupocodi = item.Grupocodi,
                        Famcodi = item.Famcodi,
                        Equicodi = item.Equicodi.Value,
                        Pftotfp = item.Pffactfp,
                        Pftotunidadnomb = item.Pffactunidadnomb,
                        Pftotincremental = item.Pffactincremental
                    };
                    lista.Add(objUnidad);
                }
            }

            return lista;
        }

        #endregion

    }
}
