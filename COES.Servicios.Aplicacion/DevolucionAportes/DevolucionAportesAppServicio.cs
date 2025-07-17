using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.DevolucionAportes
{
    /// <summary>
    /// Clases con métodos del módulo DevolucionAportes
    /// </summary>
    public class DevolucionAportesAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DevolucionAportesAppServicio));

        #region Métodos Tabla DAI_APORTANTE

        /// <summary>
        /// Inserta un registro de la tabla DAI_APORTANTE
        /// </summary>
        public void SaveDaiAportante(DaiAportanteDTO entity)
        {
            try
            {
                FactorySic.GetDaiAportanteRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DAI_APORTANTE
        /// </summary>
        public void UpdateDaiAportante(DaiAportanteDTO entity)
        {
            try
            {
                FactorySic.GetDaiAportanteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla DAI_APORTANTE
        /// </summary>
        public void DeleteDaiAportante(int aporcodi)
        {
            try
            {
                FactorySic.GetDaiAportanteRepository().Delete(aporcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla DAI_APORTANTE
        /// </summary>
        public void DeleteByPresupuesto(DaiAportanteDTO aportante)
        {
            try
            {
                FactorySic.GetDaiAportanteRepository().DeleteByPresupuesto(aportante);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        

        /// <summary>
        /// Permite obtener un registro de la tabla DAI_APORTANTE
        /// </summary>
        public DaiAportanteDTO GetByIdDaiAportante(int aporcodi)
        {
            return FactorySic.GetDaiAportanteRepository().GetById(aporcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla DAI_APORTANTE
        /// </summary>
        public List<DaiAportanteDTO> ListDaiAportantes()
        {
            return FactorySic.GetDaiAportanteRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla DAI_APORTANTE
        /// </summary>
        public List<DaiAportanteDTO> ListCuadroDevolucion(decimal igv, int anio, int estado)
        {
            return FactorySic.GetDaiAportanteRepository().ListCuadroDevolucion(igv, anio, estado);
        }
        
        /// <summary>
        /// Permite realizar búsquedas en la tabla DaiAportante
        /// </summary>
        public List<DaiAportanteDTO> GetByCriteriaDaiAportantes(int prescodi, int tabcdcodiestado)
        {
            return FactorySic.GetDaiAportanteRepository().GetByCriteria(prescodi, tabcdcodiestado);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla DaiAportante
        /// </summary>
        public List<DaiAportanteDTO> GetByCriteriaDaiAportantesCronograma(int anio, string tabcdcodiestado)
        {
            return FactorySic.GetDaiAportanteRepository().GetByCriteriaAportanteCronograma(anio, tabcdcodiestado);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla DaiAportante
        /// </summary>
        public List<DaiAportanteDTO> GetByCriteriaDaiAportantesLiquidacion(int prescodi, int tabcdcodiestado)
        {
            return FactorySic.GetDaiAportanteRepository().GetByCriteriaAportanteLiquidacion(prescodi, tabcdcodiestado);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla DaiAportante
        /// </summary>
        /// <param name="aportante"></param>
        /// <returns></returns>
        public List<DaiAportanteDTO> GetByCriteriaDaiAportantesFinalizar(DaiAportanteDTO aportante)
        {
            return FactorySic.GetDaiAportanteRepository().GetByCriteriaFinalizar(aportante);
        }
        

        /// <summary>
        /// Permite obtener la lista de años de calendario por el presupuesto
        /// </summary>
        public List<DaiAportanteDTO> GetByCriteriaAniosCronograma(int prescodi, int estado)
        {
            return FactorySic.GetDaiAportanteRepository().GetByCriteriaAniosCronograma(prescodi, estado);
        }
        
        /// <summary>
        /// Devuelve el registro de la empresas por su emprcodi
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public SiEmpresaDTO GetByIdEmpresa(int emprcodi)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(emprcodi);
        }

        /// <summary>
        /// Devuelve todas las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresaDevolucion()
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresaDevolucion();
        }
        #endregion

        #region Métodos Tabla DAI_CALENDARIOPAGO

        /// <summary>
        /// Inserta un registro de la tabla DAI_CALENDARIOPAGO
        /// </summary>
        public void SaveDaiCalendariopago(DaiCalendariopagoDTO entity)
        {
            try
            {
                FactorySic.GetDaiCalendariopagoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DAI_CALENDARIOPAGO
        /// </summary>
        public void UpdateDaiCalendariopago(DaiCalendariopagoDTO entity)
        {
            try
            {
                FactorySic.GetDaiCalendariopagoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla DAI_CALENDARIOPAGO
        /// </summary>
        public void DeleteDaiCalendariopago(int calecodi)
        {
            try
            {
                FactorySic.GetDaiCalendariopagoRepository().Delete(calecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla DAI_CALENDARIOPAGO
        /// </summary>
        public void ReprocesarDaiCalendariopago(DaiCalendariopagoDTO calendario)
        {
            try
            {
                FactorySic.GetDaiCalendariopagoRepository().Reprocesar(calendario);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla DAI_CALENDARIOPAGO
        /// </summary>
        public void LiquidarDaiCalendariopago(DaiCalendariopagoDTO calendario)
        {
            try
            {
                FactorySic.GetDaiCalendariopagoRepository().Liquidar(calendario);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DAI_CALENDARIOPAGO
        /// </summary>
        public DaiCalendariopagoDTO GetByIdDaiCalendariopago(int calecodi)
        {
            return FactorySic.GetDaiCalendariopagoRepository().GetById(calecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla DAI_CALENDARIOPAGO
        /// </summary>
        public List<DaiCalendariopagoDTO> ListDaiCalendariopagos(int aporcodi, int estado)
        {
            return FactorySic.GetDaiCalendariopagoRepository().List(aporcodi, estado);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla DaiCalendariopago
        /// </summary>
        public List<DaiCalendariopagoDTO> GetByCriteriaDaiCalendariopagos(int aporcodi)
        {
            return FactorySic.GetDaiCalendariopagoRepository().GetByCriteria(aporcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla DaiCalendariopago por el año
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<DaiCalendariopagoDTO> GetByCriteriaAnioDaiCalendariopagos(int emprcodi)
        {
            return FactorySic.GetDaiCalendariopagoRepository().GetByCriteriaAnio(emprcodi);
        }

        
        #endregion

        #region Métodos Tabla DAI_PRESUPUESTO

        /// <summary>
        /// Inserta un registro de la tabla DAI_PRESUPUESTO
        /// </summary>
        public void SaveDaiPresupuesto(DaiPresupuestoDTO entity)
        {
            try
            {
                FactorySic.GetDaiPresupuestoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DAI_PRESUPUESTO
        /// </summary>
        public void UpdateDaiPresupuesto(DaiPresupuestoDTO entity)
        {
            try
            {
                FactorySic.GetDaiPresupuestoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla DAI_PRESUPUESTO
        /// </summary>
        public void DeleteDaiPresupuesto(DaiPresupuestoDTO presupuesto)
        {
            try
            {
                FactorySic.GetDaiPresupuestoRepository().Delete(presupuesto);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DAI_PRESUPUESTO
        /// </summary>
        public DaiPresupuestoDTO GetByIdDaiPresupuesto(int prescodi)
        {
            return FactorySic.GetDaiPresupuestoRepository().GetById(prescodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla DAI_PRESUPUESTO
        /// </summary>
        public List<DaiPresupuestoDTO> ListDaiPresupuestos()
        {
            return FactorySic.GetDaiPresupuestoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla DaiPresupuesto
        /// </summary>
        public List<DaiPresupuestoDTO> GetByCriteriaDaiPresupuestos()
        {
            return FactorySic.GetDaiPresupuestoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla DAI_TABLACODIGO

        /// <summary>
        /// Inserta un registro de la tabla DAI_TABLACODIGO
        /// </summary>
        public void SaveDaiTablacodigo(DaiTablacodigoDTO entity)
        {
            try
            {
                FactorySic.GetDaiTablacodigoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DAI_TABLACODIGO
        /// </summary>
        public void UpdateDaiTablacodigo(DaiTablacodigoDTO entity)
        {
            try
            {
                FactorySic.GetDaiTablacodigoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla DAI_TABLACODIGO
        /// </summary>
        public void DeleteDaiTablacodigo(int tabcodi)
        {
            try
            {
                FactorySic.GetDaiTablacodigoRepository().Delete(tabcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DAI_TABLACODIGO
        /// </summary>
        public DaiTablacodigoDTO GetByIdDaiTablacodigo(int tabcodi)
        {
            return FactorySic.GetDaiTablacodigoRepository().GetById(tabcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla DAI_TABLACODIGO
        /// </summary>
        public List<DaiTablacodigoDTO> ListDaiTablacodigos()
        {
            return FactorySic.GetDaiTablacodigoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla DaiTablacodigo
        /// </summary>
        public List<DaiTablacodigoDTO> GetByCriteriaDaiTablacodigos()
        {
            return FactorySic.GetDaiTablacodigoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla DAI_TABLACODIGO_DETALLE

        /// <summary>
        /// Inserta un registro de la tabla DAI_TABLACODIGO_DETALLE
        /// </summary>
        public void SaveDaiTablacodigoDetalle(DaiTablacodigoDetalleDTO entity)
        {
            try
            {
                FactorySic.GetDaiTablacodigoDetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla DAI_TABLACODIGO_DETALLE
        /// </summary>
        public void UpdateDaiTablacodigoDetalle(DaiTablacodigoDetalleDTO entity)
        {
            try
            {
                FactorySic.GetDaiTablacodigoDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla DAI_TABLACODIGO_DETALLE
        /// </summary>
        public void DeleteDaiTablacodigoDetalle(int tabdcodi)
        {
            try
            {
                FactorySic.GetDaiTablacodigoDetalleRepository().Delete(tabdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla DAI_TABLACODIGO_DETALLE
        /// </summary>
        public DaiTablacodigoDetalleDTO GetByIdDaiTablacodigoDetalle(int tabdcodi)
        {
            return FactorySic.GetDaiTablacodigoDetalleRepository().GetById(tabdcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla DAI_TABLACODIGO_DETALLE
        /// </summary>
        public List<DaiTablacodigoDetalleDTO> ListDaiTablacodigoDetalles()
        {
            return FactorySic.GetDaiTablacodigoDetalleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla DaiTablacodigoDetalle
        /// </summary>
        public List<DaiTablacodigoDetalleDTO> GetByCriteriaDaiTablacodigoDetalles()
        {
            return FactorySic.GetDaiTablacodigoDetalleRepository().GetByCriteria();
        }

        #endregion

    }
}
