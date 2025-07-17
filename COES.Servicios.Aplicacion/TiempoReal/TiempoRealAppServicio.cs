using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Sp7;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using OfficeOpenXml;
using System.IO;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2013.ExcelAc;

namespace COES.Servicios.Aplicacion.TiempoReal
{
    /// <summary>
    /// Clases con métodos del módulo TiempoReal
    /// </summary>
    public class TiempoRealAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TiempoRealAppServicio));

        #region Métodos Tabla TR_CANALCAMBIO_SP7

        /// <summary>
        /// Inserta un registro de la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public void SaveTrCanalcambioSp7(TrCanalcambioSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrCanalcambioSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public void UpdateTrCanalcambioSp7(TrCanalcambioSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrCanalcambioSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public void DeleteTrCanalcambioSp7(int canalcodi, DateTime canalcmfeccreacion)
        {
            try
            {
                FactoryScada.GetTrCanalcambioSp7Repository().Delete(canalcodi, canalcmfeccreacion);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public TrCanalcambioSp7DTO GetByIdTrCanalcambioSp7(int canalcodi, DateTime canalcmfeccreacion)
        {
            return FactoryScada.GetTrCanalcambioSp7Repository().GetById(canalcodi, canalcmfeccreacion);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public List<TrCanalcambioSp7DTO> ListTrCanalcambioSp7s()
        {
            return FactoryScada.GetTrCanalcambioSp7Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public List<TrCanalcambioSp7DTO> GetByCriteriaTrCanalcambioSp7s()
        {
            return FactoryScada.GetTrCanalcambioSp7Repository().GetByCriteria();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public List<TrCanalcambioSp7DTO> GetByFechaTrCanalcambioSp7s(DateTime fechaInicial, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactoryScada.GetTrCanalcambioSp7Repository().GetByFecha(fechaInicial, fechaFinal, nroPage, pageSize);
        }
        /// <summary>
        /// Permite eliminar filas en blanco del reporte
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public List<TrCanalcambioSp7DTO> FiltrarFilasEnBlanco(List<TrCanalcambioSp7DTO> lista)
        {
            List<TrCanalcambioSp7DTO> listaFinal = new List<TrCanalcambioSp7DTO>();
            foreach(var reg in lista)
            {
                if (reg.Emprcodi == null && reg.Zonacodi == null && reg.Canalpathb == null && reg.Canaliccp == null && reg.Canalunidad == null) { }
                else 
                    listaFinal.Add(reg);
            }
            return listaFinal;
        }
        /// <summary>
        /// Graba los datos de la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public int SaveTrCanalcambioSp7Id(TrCanalcambioSp7DTO entity)
        {
            return FactoryScada.GetTrCanalcambioSp7Repository().SaveTrCanalcambioSp7Id(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public List<TrCanalcambioSp7DTO> BuscarOperacionesCanalCambioSp7(DateTime canalCmfeccreacion, int nroPage, int pageSize)
        {
            return FactoryScada.GetTrCanalcambioSp7Repository().BuscarOperaciones(canalCmfeccreacion, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public int ObtenerNroFilasCanalCambioSp7(DateTime canalCmfeccreacion)
        {
            return FactoryScada.GetTrCanalcambioSp7Repository().ObtenerNroFilas(canalCmfeccreacion);
        }

        #endregion

        #region Métodos Tabla TR_CARGAARCHIVO_XML
        /// <summary>
        /// Permite realizar búsquedas en la tabla TR_CANALCAMBIO_SP7
        /// </summary>
        public List<TrCargaarchxmlSp7DTO> GetByFechaTrCargaarchxmlSp7s(DateTime fechaInicial, DateTime fechaFinal)
        {
            var listaCargaXML = FactoryScada.GetTrCargaarchxmlSp7Repository().GetByFecha(fechaInicial, fechaFinal);

            foreach (var cargaXML in listaCargaXML)
            {
                ConstantesTiempoReal Constantes = new ConstantesTiempoReal();
                if (cargaXML.CarTipo != null)
                {
                    var cargaTipo = "";

                    if (Constantes.TipoCargaArchivo.ContainsKey((int)cargaXML.CarTipo))
                    {
                        cargaTipo = Constantes.TipoCargaArchivo[(int)cargaXML.CarTipo];
                    }
                    else
                    {
                        cargaTipo = "Valor no encontrado";
                    }

                    cargaXML.CarTipoNombre = cargaTipo;
                }
            }

            return listaCargaXML;
        }
        #endregion

        #region Métodos Tabla TR_ESTADCANALR_SP7

        /// <summary>
        /// Inserta un registro de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public void SaveTrEstadcanalrSp7(TrEstadcanalrSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrEstadcanalrSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public void UpdateTrEstadcanalrSp7(TrEstadcanalrSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrEstadcanalrSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public void DeleteTrEstadcanalrSp7(int estcnlcodi)
        {
            try
            {
                FactoryScada.GetTrEstadcanalrSp7Repository().Delete(estcnlcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros por version de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public void DeleteTrEstadcanalrSp7Version(int vercodi)
        {
            try
            {
                FactoryScada.GetTrEstadcanalrSp7Repository().DeleteVersion(vercodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public TrEstadcanalrSp7DTO GetByIdTrEstadcanalrSp7(int estcnlcodi)
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().GetById(estcnlcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public List<TrEstadcanalrSp7DTO> ListTrEstadcanalrSp7s()
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().List();
        }

        /// <summary>
        /// Permite listar todos los registros por version y fecha de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public List<TrEstadcanalrSp7DTO> ListTrEstadcanalrSp7s(int vercodi, DateTime fecha)
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().List(vercodi, fecha);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public List<TrEstadcanalrSp7DTO> GetByCriteriaTrEstadcanalrSp7s()
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public int SaveTrEstadcanalrSp7Id(TrEstadcanalrSp7DTO entity)
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().SaveTrEstadcanalrSp7Id(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public List<TrEstadcanalrSp7DTO> BuscarOperacionesEstadCanalrSp7(int verCodi, DateTime estcnlFechaIni, DateTime estcnlFechaIniFin, int nroPage, int pageSize)
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().BuscarOperaciones(verCodi, estcnlFechaIni, estcnlFechaIniFin, nroPage, pageSize);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public List<TrEstadcanalrSp7DTO> BuscarOperacionesEstadCanalrSp7(int verCodi, int emprcodi, int zonacodi, int canalcodi, int segundosDia, DateTime estcnlFechaIni, DateTime estcnlFechaFin, int nroPage, int pageSize)
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().BuscarOperaciones(verCodi, emprcodi, zonacodi, canalcodi, segundosDia, estcnlFechaIni, estcnlFechaFin, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public int ObtenerNroFilasEstadCanalrSp7(int verCodi, DateTime estcnlFechaIni, DateTime estcnlFechaFin)
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().ObtenerNroFilas(verCodi, estcnlFechaIni, estcnlFechaFin);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public int ObtenerNroFilasEstadCanalrSp7(int verCodi, int emprcodi, int zonacodi, int canalcodi, DateTime estcnlFechaIni, DateTime estcnlFechaFin)
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().ObtenerNroFilas(verCodi, emprcodi, zonacodi, canalcodi, estcnlFechaIni, estcnlFechaFin);
        }


        /// <summary>
        /// Obtiene un listado de disponibilidad por fecha y empresa de la tabla TR_ESTADCANALR_SP7
        /// </summary>
        public List<TrEstadcanalrSp7DTO> GetDispDiaSignal(DateTime fecha, int emprcodi)
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().GetDispDiaSignal(fecha,emprcodi);
        }

        #endregion
        

        #region Métodos Tabla TR_LOGDMP_SP7

        /// <summary>
        /// Inserta un registro de la tabla TR_LOGDMP_SP7
        /// </summary>
        public void SaveTrLogdmpSp7(TrLogdmpSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrLogdmpSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_LOGDMP_SP7
        /// </summary>
        public void UpdateTrLogdmpSp7(TrLogdmpSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrLogdmpSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_LOGDMP_SP7
        /// </summary>
        public void DeleteTrLogdmpSp7(int ldmcodi)
        {
            try
            {
                FactoryScada.GetTrLogdmpSp7Repository().Delete(ldmcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_LOGDMP_SP7
        /// </summary>
        public TrLogdmpSp7DTO GetByIdTrLogdmpSp7(int ldmcodi)
        {
            return FactoryScada.GetTrLogdmpSp7Repository().GetById(ldmcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_LOGDMP_SP7
        /// </summary>
        public List<TrLogdmpSp7DTO> ListTrLogdmpSp7s()
        {
            return FactoryScada.GetTrLogdmpSp7Repository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de exportacion con estado de la tabla TR_LOGDMP_SP7
        /// </summary>
        public List<TrLogdmpSp7DTO> ListExpTrLogdmpSp7s(string estado)
        {            
            return FactoryScada.GetTrLogdmpSp7Repository().ListExportacion(estado);
        }

        /// <summary>
        /// Permite listar todos los registros de importacion con estado de la tabla TR_LOGDMP_SP7
        /// </summary>
        public List<TrLogdmpSp7DTO> ListImpTrLogdmpSp7s(string estado)
        {
            return FactoryScada.GetTrLogdmpSp7Repository().ListImportacion(estado);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TR_LOGDMP_SP7
        /// </summary>
        public List<TrLogdmpSp7DTO> GetByCriteriaTrLogdmpSp7s()
        {
            return FactoryScada.GetTrLogdmpSp7Repository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla TR_LOGDMP_SP7
        /// </summary>
        public int SaveTrLogdmpSp7Id(TrLogdmpSp7DTO entity)
        {
            return FactoryScada.GetTrLogdmpSp7Repository().SaveTrLogdmpSp7Id(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla TR_LOGDMP_SP7
        /// </summary>
        public List<TrLogdmpSp7DTO> BuscarOperacionesLogDmpSp7(DateTime fechaIni, DateTime fechaFin,string tipo, int nroPage, int pageSize)
        {
            return FactoryScada.GetTrLogdmpSp7Repository().BuscarOperaciones(fechaIni, fechaFin,tipo, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla TR_LOGDMP_SP7
        /// </summary>
        public int ObtenerNroFilasLogDmpSp7(DateTime fechaIni, DateTime fechaFin)
        {
            return FactoryScada.GetTrLogdmpSp7Repository().ObtenerNroFilas(fechaIni, fechaFin);
        }

        #endregion


        #region Métodos Tabla TR_REPORTEVERSION_SP7

        /// <summary>
        /// Inserta un registro de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public void SaveTrReporteversionSp7(TrReporteversionSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrReporteversionSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public void UpdateTrReporteversionSp7(TrReporteversionSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrReporteversionSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public void DeleteTrReporteversionSp7(int revcodi)
        {
            try
            {
                FactoryScada.GetTrReporteversionSp7Repository().Delete(revcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros por version de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public void DeleteTrReporteversionSp7Version(int vercodi)
        {
            try
            {
                FactoryScada.GetTrReporteversionSp7Repository().DeleteVersion(vercodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public TrReporteversionSp7DTO GetByIdTrReporteversionSp7(int revcodi)
        {
            return FactoryScada.GetTrReporteversionSp7Repository().GetById(revcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public List<TrReporteversionSp7DTO> ListTrReporteversionSp7s()
        {
            return FactoryScada.GetTrReporteversionSp7Repository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la version de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public List<TrReporteversionSp7DTO> ListTrReporteversionSp7s(int verCodi)
        {
            return FactoryScada.GetTrReporteversionSp7Repository().List(verCodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la version agrupada de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public List<TrReporteversionSp7DTO> ListTrReporteversionSp7s(int verCodi, DateTime fechaIni, DateTime fechaFin)
        {
            return FactoryScada.GetTrReporteversionSp7Repository().List(verCodi, fechaIni, fechaFin);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public List<TrReporteversionSp7DTO> GetByCriteriaTrReporteversionSp7s()
        {
            return FactoryScada.GetTrReporteversionSp7Repository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public int SaveTrReporteversionSp7Id(TrReporteversionSp7DTO entity)
        {
            return FactoryScada.GetTrReporteversionSp7Repository().SaveTrReporteversionSp7Id(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public List<TrReporteversionSp7DTO> BuscarOperacionesReporteVersionSp7(int verCodi, DateTime fechaIni, DateTime fechaFin, int nroPage, int pageSize)
        {
            return FactoryScada.GetTrReporteversionSp7Repository().BuscarOperaciones(verCodi, fechaIni, fechaFin, nroPage, pageSize);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla TR_REPORTEVERSION_SP7 (resumen)
        /// </summary>
        public List<TrReporteversionSp7DTO> BuscarOperacionesReporteVersionSp7Resumen(int verCodi, DateTime fechaIni, DateTime fechaFin, int nroPage, int pageSize)
        {
            return FactoryScada.GetTrReporteversionSp7Repository().BuscarOperacionesResumen(verCodi, fechaIni, fechaFin, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla TR_REPORTEVERSION_SP7
        /// </summary>
        public int ObtenerNroFilasReporteVersionSp7(int verCodi, DateTime revFeccreacion, DateTime revFecmodificacion)
        {
            return FactoryScada.GetTrReporteversionSp7Repository().ObtenerNroFilas(verCodi, revFeccreacion, revFecmodificacion);
        }

        #endregion


        #region Métodos Tabla TR_VERSION_SP7

        /// <summary>
        /// Inserta un registro de la tabla TR_VERSION_SP7
        /// </summary>
        public void SaveTrVersionSp7(TrVersionSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrVersionSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_VERSION_SP7
        /// </summary>
        public void UpdateTrVersionSp7(TrVersionSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrVersionSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_VERSION_SP7
        /// </summary>
        public void DeleteTrVersionSp7(int vercodi)
        {
            try
            {
                FactoryScada.GetTrVersionSp7Repository().Delete(vercodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_VERSION_SP7
        /// </summary>
        public TrVersionSp7DTO GetByIdTrVersionSp7(int vercodi)
        {
            return FactoryScada.GetTrVersionSp7Repository().GetById(vercodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_VERSION_SP7
        /// </summary>
        public List<TrVersionSp7DTO> ListTrVersionSp7s()
        {
            return FactoryScada.GetTrVersionSp7Repository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de acuerdo a una fecha de la tabla TR_VERSION_SP7
        /// </summary>
        public List<TrVersionSp7DTO> ListTrVersionSp7s(DateTime verFecha)
        {
            return FactoryScada.GetTrVersionSp7Repository().List(verFecha);
        }

        /// <summary>
        /// Permite listar todos los registros pendientes de la tabla TR_VERSION_SP7
        /// </summary>
        public List<TrVersionSp7DTO> ListTrVersionPendienteSp7s()
        {
            return FactoryScada.GetTrVersionSp7Repository().ListPendiente();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TR_VERSION_SP7
        /// </summary>
        public List<TrVersionSp7DTO> GetByCriteriaTrVersionSp7s()
        {
            return FactoryScada.GetTrVersionSp7Repository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla TR_VERSION_SP7
        /// </summary>
        public int SaveTrVersionSp7Id(TrVersionSp7DTO entity)
        {
            return FactoryScada.GetTrVersionSp7Repository().SaveTrVersionSp7Id(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla TR_VERSION_SP7
        /// </summary>
        public List<TrVersionSp7DTO> BuscarOperacionesVersionSp7(DateTime verFechaini, DateTime verFechafin, int nroPage, int pageSize)
        {
            return FactoryScada.GetTrVersionSp7Repository().BuscarOperaciones(verFechaini, verFechafin, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla TR_VERSION_SP7
        /// </summary>
        public int ObtenerNroFilasVersionSp7(DateTime verFechaini, DateTime verFechafin)
        {
            return FactoryScada.GetTrVersionSp7Repository().ObtenerNroFilas(verFechaini, verFechafin);
        }

        /// <summary>
        /// Obtiene la version del reporte de la tabla TR_VERSION_SP7
        /// </summary>
        public int GetVersion(DateTime verFecha)
        {
            return FactoryScada.GetTrVersionSp7Repository().GetVersion(verFecha);
        }
                

        #endregion



        #region Métodos Tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)

        /// <summary>
        /// Inserta un registro de la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        public void SaveTrCircularSp7(TrCircularSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrCircularSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        public void UpdateTrCircularSp7(TrCircularSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrCircularSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        public void DeleteTrCircularSp7(int canalcodi, DateTime canalfhsist)
        {
            try
            {
                FactoryScada.GetTrCircularSp7Repository().Delete(canalcodi, canalfhsist);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        public TrCircularSp7DTO GetByIdTrCircularSp7(int canalcodi, DateTime canalfhsist)
        {
            return FactoryScada.GetTrCircularSp7Repository().GetById(canalcodi, canalfhsist);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_CALIDADICCP_SP7
        /// </summary>
        public List<TrCalidadSp7DTO> ListTrCalidadSp7s()
        {
            return FactoryScada.GetTrCircularSp7Repository().GetCalidades();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        public List<TrCircularSp7DTO> ListTrCircularSp7s()
        {
            return FactoryScada.GetTrCircularSp7Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        public List<TrCircularSp7DTO> GetByCriteriaTrCircularSp7s()
        {
            return FactoryScada.GetTrCircularSp7Repository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        public int SaveTrCircularSp7Id(TrCircularSp7DTO entity)
        {
            return FactoryScada.GetTrCircularSp7Repository().SaveTrCircularSp7Id(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        public List<TrCircularSp7DTO> BuscarOperacionesCircularSp7(string canalcodis, DateTime canalFhsistInicio, DateTime canalFhsistFin, int nroPage, int pageSize)
        {
            return FactoryScada.GetTrCircularSp7Repository().BuscarOperaciones(canalcodis, canalFhsistInicio, canalFhsistFin, nroPage, pageSize);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        public List<TrCircularSp7DTO> BuscarOperacionesRangoCircularSp7(string canalcodis, DateTime canalFhsistInicio, DateTime canalFhsistFin, int nroPage, int pageSize)
        {
            return FactoryScada.GetTrCircularSp7Repository().BuscarOperacionesRango(canalcodis, canalFhsistInicio, canalFhsistFin, nroPage, pageSize);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        public List<TrCircularSp7DTO> BuscarOperacionesCircularSp7(int canalcodi, DateTime canalFhsistInicio, DateTime canalFhsistFin, int nroPage, int pageSize, string analisis, int calidadNotRenew, int calidadHisNotCollected)
        {
            return FactoryScada.GetTrCircularSp7Repository().BuscarOperaciones(canalcodi, canalFhsistInicio, canalFhsistFin, nroPage, pageSize, analisis, calidadNotRenew, calidadHisNotCollected);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla TR_CIRCULAR_SP7 (TR_YYYYMMDD_SP7)
        /// </summary>
        /// <param name="canalcodis"></param>
        /// <param name="canalFhsistInicio"></param>
        /// <param name="canalFhsistFin"></param>
        /// <returns></returns>
        public int ObtenerNroFilasCircularSp7(string canalcodis, DateTime canalFhsistInicio, DateTime canalFhsistFin, string analisis, int calidadNotRenew, int calidadHisNotCollected)
        {
            return FactoryScada.GetTrCircularSp7Repository().ObtenerNroFilas(canalcodis, canalFhsistInicio, canalFhsistFin, analisis, calidadNotRenew, calidadHisNotCollected);
        }

        /// <summary>
        /// Script asociado a la creacion de tabla circular (TR_YYYYMMDD_SP7)
        /// </summary>
        /// <param name="i">opcion i</param>
        /// <param name="fecha">fecha</param>
        public void CrearTablaCircularSp7(int i, string fecha)
        {
            FactoryScada.GetTrCircularSp7Repository().CrearTabla(i, fecha);
        }

        #endregion

        public void ExportarCambiosAlSincronizarCanales(DateTime fecha, string path, string filename, string absolutePath)
        {
            List<TrCanalcambioSp7DTO> listaActualizacionSp7 = GetByFechaTrCanalcambioSp7s(fecha, fecha.AddDays(1), 1, 1); 
            FileInfo fileExcel = new FileInfo(absolutePath);

            if (fileExcel.Exists)
            {
                fileExcel.Delete();
                fileExcel = new FileInfo(absolutePath);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(fileExcel))
            {
                ExcelWorksheet ws = null;

                //
                ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];

                #region Cabecera
                //Titulo
                int rowIniTitulo = 2;
                int rowIniLeyenda = rowIniTitulo + 2;

                ws.Cells[rowIniLeyenda++, 1].Value = "LEYENDA";
                ws.Cells[rowIniLeyenda++, 1].Value = "AZUL: ACTUAL";
                ws.Cells[rowIniLeyenda++, 1].Value = "VERDE: ANTERIOR";

                int colIniTitulo = 3;
                ws.Cells[rowIniTitulo, colIniTitulo].Value = "Actualización de canales - Tiempo Real SP7";
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo, "Arial", 16);
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo);

                //Fila cabecera
                int rowIni = rowIniLeyenda + 2;
                int colIni = 2;

                int colIniEmpresa = colIni;
                int colIniEmpresaAnt = colIniEmpresa + 1;
                int colIniZona = colIniEmpresaAnt + 1;
                int colIniZonaAnt = colIniZona + 1;
                int colIniPatch = colIniZonaAnt + 1;
                int colIniPatchAnt = colIniPatch + 1;
                int colIniNomb = colIniPatchAnt + 1;
                int colIniNombAnt = colIniNomb + 1;
                int colIniICCP = colIniNombAnt + 1;
                int colIniICCPAnt = colIniICCP + 1;
                int colIniUnidad = colIniICCPAnt + 1;
                int colIniUnidadAnt = colIniUnidad + 1;


                //anteriores
                int colIniFecha = colIniUnidadAnt + 1;
                int colIniUsuario = colIniFecha + 1;

                int rowIniCab = rowIni;

                ws.Cells[rowIniCab, colIniEmpresa].Value = "EMPRESA";
                ws.Cells[rowIniCab, colIniEmpresaAnt].Value = "EMPRESA (ANTERIOR)";
                ws.Cells[rowIniCab, colIniZona].Value = "ZONA";
                ws.Cells[rowIniCab, colIniZonaAnt].Value = "ZONA (ANTERIOR)";
                ws.Cells[rowIniCab, colIniPatch].Value = "PATCH";
                ws.Cells[rowIniCab, colIniPatchAnt].Value = "PATCH (ANTERIOR)";
                ws.Cells[rowIniCab, colIniNomb].Value = "NOMBRE";
                ws.Cells[rowIniCab, colIniNombAnt].Value = "NOMBRE (ANTERIOR)";
                ws.Cells[rowIniCab, colIniICCP].Value = "ICCP";
                ws.Cells[rowIniCab, colIniICCPAnt].Value = "ICCP (ANTERIOR)";
                ws.Cells[rowIniCab, colIniUnidad].Value = "UNIDAD (ANTERIOR)";
                ws.Cells[rowIniCab, colIniUnidadAnt].Value = "UNIDAD (ANTERIOR)";

                ws.Cells[rowIniCab, colIniFecha].Value = "FECHA";
                ws.Cells[rowIniCab, colIniUsuario].Value = "USUARIO";

                int colIniDia = colIniUsuario + 1;
                int colFinDia = colIniDia;

                #endregion

                int row = rowIni + 1;
                int col = colIni;

                int rowIniData = row;

                #region Cuerpo

                if (listaActualizacionSp7.Any())
                {
                    foreach (var regEq in listaActualizacionSp7)
                    {
                        ws.Cells[row, colIniEmpresa].Value = regEq.EmpresaNombre;
                        ws.Cells[row, colIniEmpresaAnt].Value = regEq.EmpresaNombreant;
                        ws.Cells[row, colIniZona].Value = regEq.ZonaNombre;
                        ws.Cells[row, colIniZonaAnt].Value = regEq.ZonaNombreant;
                        ws.Cells[row, colIniPatch].Value = regEq.Canalpathb;
                        ws.Cells[row, colIniPatchAnt].Value = regEq.Canalpathbant;
                        ws.Cells[row, colIniNomb].Value = regEq.Canalnomb;
                        ws.Cells[row, colIniNombAnt].Value = regEq.Canalnombant;
                        ws.Cells[row, colIniICCP].Value = regEq.Canaliccp;
                        ws.Cells[row, colIniICCPAnt].Value = regEq.Canaliccpant;
                        ws.Cells[row, colIniUnidad].Value = regEq.Canalunidad;
                        ws.Cells[row, colIniUnidadAnt].Value = regEq.Canalunidadant;

                        ws.Cells[row, colIniFecha].Value = regEq.Canalcmfeccreacion.ToString(ConstantesBase.FormatoFechaHora);
                        ws.Cells[row, colIniUsuario].Value = regEq.Lastuser;

                        row++;
                    }
                }

                int rowFinData = row - 1;

                #region Formato Excel

                //fila cabecera
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniEmpresa, rowIniCab, colIniEmpresa, "#03218c");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniEmpresaAnt, rowIniCab, colIniEmpresaAnt, "#038c03");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniZona, rowIniCab, colIniZona, "#03218c");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniZonaAnt, rowIniCab, colIniZonaAnt, "#038c03");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniPatch, rowIniCab, colIniPatch, "#03218c");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniPatchAnt, rowIniCab, colIniPatchAnt, "#038c03");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniNomb, rowIniCab, colIniNomb, "#03218c");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniNombAnt, rowIniCab, colIniNombAnt, "#038c03");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniICCP, rowIniCab, colIniICCP, "#03218c");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniICCPAnt, rowIniCab, colIniICCPAnt, "#038c03");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniUnidad, rowIniCab, colIniUnidad, "#03218c");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniUnidadAnt, rowIniCab, colIniUnidadAnt, "#038c03");
                //UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniEmpresa, rowIniCab, colIniUnidad, "#03218c");
                //UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniEmpresaAnt, rowIniCab, colIniUnidadAnt, "#038c03");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniFecha, rowIniCab, colIniUsuario, "#000000");
                UtilExcel.CeldasExcelColorTexto(ws, rowIniCab, colIniEmpresa, rowIniCab, colIniUsuario, "#FFFFFF");

                //Cuerpo
                if (rowIniData <= rowFinData)
                {
                    UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniUsuario, "Arriba");
                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniICCP, "Izquierda");
                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniUnidad, rowFinData, colIniUnidad, "Centro");
                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresaAnt, rowFinData, colIniICCPAnt, "Izquierda");
                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniUnidadAnt, rowFinData, colIniUnidadAnt, "Centro");
                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniFecha, rowFinData, colIniUsuario, "Centro");
                    UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniEmpresa, rowFinData, colIniUsuario, "Arial", 10);
                }
                #endregion

                #endregion

                ws.Column(1).Width = 1;
                ws.Column(colIniEmpresa).Width = 36;
                ws.Column(colIniZona).Width = 30;
                ws.Column(colIniPatch).Width = 40;
                ws.Column(colIniNomb).Width = 40;
                ws.Column(colIniICCP).Width = 40;
                ws.Column(colIniUnidad).Width = 20;

                ws.Column(colIniEmpresaAnt).Width = 36;
                ws.Column(colIniZonaAnt).Width = 30;
                ws.Column(colIniPatchAnt).Width = 40;
                ws.Column(colIniNombAnt).Width = 40;
                ws.Column(colIniICCPAnt).Width = 40;
                ws.Column(colIniUnidadAnt).Width = 20;

                ws.Column(colIniFecha).Width = 25;
                ws.Column(colIniUsuario).Width = 20;

                ws.View.ShowGridLines = false;
                ws.View.ZoomScale = 100;

                if (ws == null) xlPackage.Workbook.Worksheets.Add("REPORTE");
                xlPackage.Workbook.View.ActiveTab = 0;
                xlPackage.Save();
            }
        }


        #region EXPORTAR A EXCEL

        /// <summary>
        /// Permite generar el reporte en formato Excel de cambios de señales
        /// </summary>
        /// <param name="objFiltro"></param>
        /// <param name="path"></param>
        /// <param name="pathLogo"></param>
        /// <param name="fileNameSalidaTmp"></param>
        /// <param name="fileNameReporte"></param>
        public void ExportarCambios(ConstantesFiltro objFiltro, string path, string pathLogo, out string fileNameSalidaTmp, out string fileNameReporte)
        {
            List<TrCanalcambioSp7DTO> listaActualizacionSp7 = GetByFechaTrCanalcambioSp7s
                (objFiltro.FechaInicial.Date, objFiltro.FechaFinal.Date,
                1, 1);
            listaActualizacionSp7 = FiltrarFilasEnBlanco(listaActualizacionSp7);
            fileNameReporte = string.Format("{1}_{2}_{0}",
                                    ConstantesTiempoReal.NombreReporteExcelActualizaciones
                                    , objFiltro.FechaInicial.ToString(ConstantesAppServicio.FormatoFechaYMD2),
                                    objFiltro.FechaFinal.ToString(ConstantesAppServicio.FormatoFechaYMD2));
            fileNameSalidaTmp = path + ConstantesIntervencionesAppServicio.NombreReporteExcelIntervencionesCruzadas;
            FileInfo newFile = new FileInfo(fileNameSalidaTmp);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileNameSalidaTmp);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;

                this.GenerarHojaRpt(xlPackage, ref ws, "REPORTE", "Actualizacion de canales - Tiempo Real SP7", pathLogo, listaActualizacionSp7);

                if (ws == null) xlPackage.Workbook.Worksheets.Add("REPORTE");
                xlPackage.Workbook.View.ActiveTab = 0;
                xlPackage.Save();
            }
        }


        private void GenerarHojaRpt(ExcelPackage xlPackage, ref ExcelWorksheet ws, string nameWS, string titulo, string rutaLogo,
                                        List<TrCanalcambioSp7DTO> listaActualizacionSp7)
        {
            //
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            #region Cabecera

            //Logo
            UtilExcel.AddImageLocal(ws, 1, 0, rutaLogo);

            //Titulo
            int rowIniTitulo = 2;
            int colIniTitulo = 3;
            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo, "Arial", 16);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo);

            //Fila cabecera
            int rowIni = 4;
            int colIni = 2;

            int colIniEmpresa = colIni;
            int colIniZona = colIniEmpresa + 1;
            int colIniPatch = colIniZona + 1;
            int colIniNomb = colIniPatch + 1;
            int colIniICCP = colIniNomb + 1;
            int colIniUnidad = colIniICCP + 1;

            //anteriores
            int colIniEmpresaAnt = colIniUnidad + 1;
            int colIniZonaAnt = colIniEmpresaAnt + 1;
            int colIniPatchAnt = colIniZonaAnt + 1;
            int colIniNombAnt = colIniPatchAnt + 1;
            int colIniICCPAnt = colIniNombAnt + 1;
            int colIniUnidadAnt = colIniICCPAnt + 1;

            int colIniFecha = colIniUnidadAnt + 1;
            int colIniUsuario = colIniFecha + 1;

            int rowIniCab = rowIni;

            ws.Cells[rowIniCab, colIniEmpresa].Value = "EMPRESA";
            ws.Cells[rowIniCab, colIniZona].Value = "ZONA";
            ws.Cells[rowIniCab, colIniPatch].Value = "PATCH";
            ws.Cells[rowIniCab, colIniNomb].Value = "NOMBRE";
            ws.Cells[rowIniCab, colIniICCP].Value = "ICCP";
            ws.Cells[rowIniCab, colIniUnidad].Value = "UNIDAD";

            ws.Cells[rowIniCab, colIniEmpresaAnt].Value = "EMPRESA";
            ws.Cells[rowIniCab, colIniZonaAnt].Value = "ZONA";
            ws.Cells[rowIniCab, colIniPatchAnt].Value = "PATCH";
            ws.Cells[rowIniCab, colIniNombAnt].Value = "NOMBRE";
            ws.Cells[rowIniCab, colIniICCPAnt].Value = "ICCP";
            ws.Cells[rowIniCab, colIniUnidadAnt].Value = "UNIDAD";

            ws.Cells[rowIniCab, colIniFecha].Value = "FECHA";
            ws.Cells[rowIniCab, colIniUsuario].Value = "USUARIO";

            int colIniDia = colIniUsuario + 1;
            int colFinDia = colIniDia;

            #endregion

            int row = rowIni + 1;
            int col = colIni;

            int rowIniData = row;

            #region Cuerpo

            if (listaActualizacionSp7.Any())
            {
                foreach (var regEq in listaActualizacionSp7)
                {
                    ws.Cells[row, colIniEmpresa].Value = regEq.EmpresaNombre;
                    ws.Cells[row, colIniZona].Value = regEq.ZonaNombre;
                    ws.Cells[row, colIniPatch].Value = regEq.Canalpathb;
                    ws.Cells[row, colIniNomb].Value = regEq.Canalnomb;
                    ws.Cells[row, colIniICCP].Value = regEq.Canaliccp;
                    ws.Cells[row, colIniUnidad].Value = regEq.Canalunidad;

                    ws.Cells[row, colIniEmpresaAnt].Value = regEq.EmpresaNombreant;
                    ws.Cells[row, colIniZonaAnt].Value = regEq.ZonaNombreant;
                    ws.Cells[row, colIniPatchAnt].Value = regEq.Canalpathbant;
                    ws.Cells[row, colIniNombAnt].Value = regEq.Canalnombant;
                    ws.Cells[row, colIniICCPAnt].Value = regEq.Canaliccpant;
                    ws.Cells[row, colIniUnidadAnt].Value = regEq.Canalunidadant;

                    ws.Cells[row, colIniFecha].Value = regEq.Canalcmfeccreacion.ToString(ConstantesBase.FormatoFechaHora);
                    ws.Cells[row, colIniUsuario].Value = regEq.Lastuser;

                    row++;
                }
            }

            int rowFinData = row - 1;

            #region Formato Excel

            //fila cabecera
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniEmpresa, rowIniCab, colIniUnidad, "#366092");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniEmpresaAnt, rowIniCab, colIniUnidadAnt, "#36c95a");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colIniFecha, rowIniCab, colIniUsuario, "#366092");

            UtilExcel.CeldasExcelColorTexto(ws, rowIniCab, colIniEmpresa, rowIniCab, colIniUsuario, "#FFFFFF");

            //Cuerpo
            if (rowIniData <= rowFinData)
            {
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniUsuario, "Arriba");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresa, rowFinData, colIniICCP, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniUnidad, rowFinData, colIniUnidad, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniEmpresaAnt, rowFinData, colIniICCPAnt, "Izquierda");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniUnidadAnt, rowFinData, colIniUnidadAnt, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colIniFecha, rowFinData, colIniUsuario, "Izquierda");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colIniEmpresa, rowFinData, colIniUsuario, "Arial", 10);
            }
            #endregion

            #endregion

            ws.Column(1).Width = 1;
            ws.Column(colIniEmpresa).Width = 36;
            ws.Column(colIniZona).Width = 30;
            ws.Column(colIniPatch).Width = 40;
            ws.Column(colIniNomb).Width = 40;
            ws.Column(colIniICCP).Width = 40;
            ws.Column(colIniUnidad).Width = 20;

            ws.Column(colIniEmpresaAnt).Width = 36;
            ws.Column(colIniZonaAnt).Width = 30;
            ws.Column(colIniPatchAnt).Width = 40;
            ws.Column(colIniNombAnt).Width = 40;
            ws.Column(colIniICCPAnt).Width = 40;
            ws.Column(colIniUnidadAnt).Width = 20;

            ws.Column(colIniFecha).Width = 25;
            ws.Column(colIniUsuario).Width = 20;
            //ws.Row(rowIniCab).Height = 30;

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            //ws.View.FreezePanes(rowIniData, colIniData);
            //ws.Select(ws.Cells[rowIniTitulo, colIniTitulo], true);
        }


        /// <summary>
        /// Permite generar el reporte en formato Excel de Intervenciones Cruzadas
        /// </summary>
        /// <param name="objFiltro"></param>
        /// <param name="path"></param>
        /// <param name="pathLogo"></param>
        /// <param name="fileNameSalidaTmp"></param>
        /// <param name="fileNameReporte"></param>
        public void ExportarCargaXML(ConstantesFiltro objFiltro, string path, string pathLogo, out string fileNameSalidaTmp, out string fileNameReporte)
        {
            List<TrCargaarchxmlSp7DTO> listaCargaArchivoXMLSp7 = GetByFechaTrCargaarchxmlSp7s
                (objFiltro.FechaInicial.Date, objFiltro.FechaFinal.Date);

            fileNameReporte = string.Format("{1}_{2}_{0}",
                                    ConstantesTiempoReal.NombreReporteExcelCargaArchivoXML
                                    , objFiltro.FechaInicial.ToString(ConstantesAppServicio.FormatoFechaYMD2),
                                    objFiltro.FechaFinal.ToString(ConstantesAppServicio.FormatoFechaYMD2));
            fileNameSalidaTmp = path + ConstantesIntervencionesAppServicio.NombreReporteExcelIntervencionesCruzadas;
            FileInfo newFile = new FileInfo(fileNameSalidaTmp);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(fileNameSalidaTmp);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;

                this.GenerarHojaArchivoXMLRpt(xlPackage, ref ws, "REPORTE", "Cargas de Archivos XML", pathLogo, listaCargaArchivoXMLSp7);

                if (ws == null) xlPackage.Workbook.Worksheets.Add("REPORTE");
                xlPackage.Workbook.View.ActiveTab = 0;
                xlPackage.Save();
            }
        }

        private void GenerarHojaArchivoXMLRpt(ExcelPackage xlPackage, ref ExcelWorksheet ws, string nameWS, string titulo, string rutaLogo,
                                        List<TrCargaarchxmlSp7DTO> listaCargaArchivoXMLSp7)
        {
            //
            ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            #region Cabecera

            //Logo
            UtilExcel.AddImageLocal(ws, 1, 0, rutaLogo);

            //Titulo
            int rowIniTitulo = 2;
            int colIniTitulo = 3;
            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo, "Arial", 16);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo);

            //Fila cabecera
            int rowIni = 4;
            int colIni = 2;

            int colCarFecha = colIni;
            int colCarTipo = colCarFecha + 1;
            int colCarCantidad = colCarTipo + 1;
            int colCarUsuario = colCarCantidad + 1;

            int rowIniCab = rowIni;

            ws.Cells[rowIniCab, colCarFecha].Value = "FECHA";
            ws.Cells[rowIniCab, colCarTipo].Value = "TIPO DE CARGA";
            ws.Cells[rowIniCab, colCarCantidad].Value = "CANALES ACTUALIZADOS";
            ws.Cells[rowIniCab, colCarUsuario].Value = "USUARIO";

            int colIniDia = colCarUsuario + 1;
            int colFinDia = colIniDia;

            #endregion

            int row = rowIni + 1;
            int col = colIni;

            int rowIniData = row;

            #region Cuerpo

            if (listaCargaArchivoXMLSp7.Any())
            {
                foreach (var regEq in listaCargaArchivoXMLSp7)
                {
                    ws.Cells[row, colCarFecha].Value = regEq.CarFecha.ToString(ConstantesBase.FormatoFecha);
                    ws.Cells[row, colCarTipo].Value = regEq.CarTipoNombre;
                    ws.Cells[row, colCarCantidad].Value = regEq.CarCantidad;
                    ws.Cells[row, colCarUsuario].Value = regEq.CarUsuario;

                    row++;
                }
            }

            int rowFinData = row - 1;

            #region Formato Excel

            //fila cabecera
            UtilExcel.CeldasExcelColorFondo(ws, rowIniCab, colCarFecha, rowIniCab, colCarUsuario, "#366092");
            UtilExcel.CeldasExcelColorTexto(ws, rowIniCab, colCarFecha, rowIniCab, colCarUsuario, "#FFFFFF");

            //Cuerpo
            if (rowIniData <= rowFinData)
            {
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniData, colCarFecha, rowFinData, colCarUsuario, "Arriba");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniData, colCarFecha, rowFinData, colCarUsuario, "Izquierda");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData, colCarFecha, rowFinData, colCarUsuario, "Arial", 10);
            }
            #endregion

            #endregion

            ws.Column(1).Width = 1;
            ws.Column(colCarFecha).Width = 20;
            ws.Column(colCarTipo).Width = 35;
            ws.Column(colCarCantidad).Width = 30;
            ws.Column(colCarUsuario).Width = 35;
            //ws.Row(rowIniCab).Height = 30;

            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            //ws.View.FreezePanes(rowIniData, colIniData);
            //ws.Select(ws.Cells[rowIniTitulo, colIniTitulo], true);
        }

        #endregion

        /// <summary>
        /// Retorna la data de todos los circulares existentes en una estampa de tiempo
        /// </summary>
        public List<TrCircularSp7DTO> ObtenerCircularesPorFecha(DateTime fecha)
        {
            return FactoryScada.GetTrCircularSp7Repository().ObtenerCircularesPorFecha(fecha);
        }

        /// <summary>
        /// Retorna canales para muestra instantánea
        /// </summary>
        public List<int> ObtenerCodigosDeCanalesParaMuestraInstantanea()
        {
            return FactoryScada.GetTrCircularSp7Repository().ObtenerCodigosDeCanalesParaMuestraInstantanea();
        }

        /// <summary>
        /// Retorna canales para muestra instantánea
        /// </summary>
        public List<TrCircularSp7DTO> ObtenerCircularesPorIntervalosDeFechaMuestraInstantanea(int canalcodigo, DateTime fechaDesde, DateTime fechaHasta)
        {
            return FactoryScada.GetTrCircularSp7Repository().ObtenerCircularesPorIntervalosDeFechaMuestraInstantanea(canalcodigo, fechaDesde, fechaHasta);
        }

        /// <summary>
        /// Retorna canales para muestra instantánea
        /// </summary>
        public TrCanalSp7DTO GetCanalById(int canalcodigo)
        {
            return FactoryScada.GetTrCircularSp7Repository().GetCanalById(canalcodigo);
        }

        /// <summary>
        /// Retorna canales para muestra instantánea
        /// </summary>
        public void GenerarEliminarMuestraInstantanea(int canalcodigo)
        {
            FactoryScada.GetTrCircularSp7Repository().EliminarRegistroMuestraInstantanea(canalcodigo);
        }

        /// <summary>
        /// Retorna canales para muestra instantánea
        /// </summary>
        public void GenerarRegistroMuestraInstantanea(TrCanalSp7DTO canal, TrCircularSp7DTO circular, string usuario)
        {
            FactoryScada.GetTrCircularSp7Repository().GenerarRegistroMuestraInstantanea(canal, circular, usuario);
        }
    }


}
