using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sp7;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Scada;
//using COES.Servicios.Aplicacion.TransfSeniales.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.TransfSeniales
{
    /// <summary>
    /// Clases con métodos del módulo TransfPotencia
    /// </summary>
    public class TransfSenialesAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransfSenialesAppServicio));

        #region Métodos Tabla TrMuestrarisSp7

        /// <summary>
        /// Inserta un registro de la tabla TrMuestrarisSp7
        /// </summary>
        public void SaveMuestrarisSp7(TrMuestrarisSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrMuestrarisSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TrMuestrarisSp7
        /// </summary>
        public void UpdateMuestrarisSp7(TrMuestrarisSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrMuestrarisSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TrMuestrarisSp7
        /// </summary>
        public void DeleteMuestrarisSp7(int emprecodi, DateTime canalfecha)
        {
            try
            {
                FactoryScada.GetTrMuestrarisSp7Repository().Delete(emprecodi, canalfecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener un registro de la tabla TrMuestrarisSp7
        /// </summary>
        public TrMuestrarisSp7DTO GetByIdMuestrarisSp7(int canalcodi, DateTime canalfecha)
        {
            return FactoryScada.GetTrMuestrarisSp7Repository().GetById(canalcodi, canalfecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TrMuestrarisSp7
        /// </summary>
        public List<TrMuestrarisSp7DTO> ListMuestrarisSp7()
        {
            return FactoryScada.GetTrMuestrarisSp7Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrMuestrarisSp7
        /// </summary>
        public List<TrMuestrarisSp7DTO> GetByCriteria()
        {
            return FactoryScada.GetTrMuestrarisSp7Repository().GetByCriteria();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrMuestrarisSp7
        /// </summary>

        public List<TrMuestrarisSp7DTO> GetListMuestraRis(int emprcodi)
        {
            return FactoryScada.GetTrMuestrarisSp7Repository().GetListMuestraRis(emprcodi);
        }


        /// <summary>
        /// Paginado en la tabla TrMuestrarisSp7
        /// </summary>
        /// <param name="empresa"></param>

        /// <returns></returns>
        public int GetPaginadoMuestraRis(int empresa)
        {
            return FactoryScada.GetTrMuestrarisSp7Repository().GetPaginadoMuestraRis(empresa);
        }

        #endregion

        #region Métodos Tabla TrIndempresatSp7

        /// <summary>
        /// Inserta un registro de la tabla TrIndempresatSp7
        /// </summary>
        public void SaveTrIndempresatSp7(TrIndempresatSp7DTO entity)
        {

            try
            {
                FactoryScada.GetTrIndempresatSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Actualiza un registro de la tabla TrIndempresatSp7
        /// </summary>
        public void UpdateTrIndempresatSp7(TrIndempresatSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrIndempresatSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }



        /// <summary>
        /// Elimina un registro de la tabla TrIndempresatSp7
        /// </summary>
        public void TrIndempresatSp7(int emprcodi, DateTime fecha)
        {
            try
            {
                FactoryScada.GetTrIndempresatSp7Repository().Delete(emprcodi, fecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener un registro de la tabla TrIndempresatSp7
        /// </summary>
        public TrIndempresatSp7DTO GetByIdTrIndempresatSp7(int emprcodi, DateTime fecha)
        {
            return FactoryScada.GetTrIndempresatSp7Repository().GetById(emprcodi, fecha);
        }



        /// <summary>
        /// Permite listar todos los registros de la tabla TrIndempresatSp7
        /// </summary>
        public List<TrIndempresatSp7DTO> Lists()
        {
            return FactoryScada.GetTrIndempresatSp7Repository().List();
        }



        /// <summary>
        /// Permite realizar búsquedas en la tabla TrIndempresatSp7
        /// </summary>
        public List<TrIndempresatSp7DTO> GetByCriteriaVtpSaldoEmpresas()
        {
            return FactoryScada.GetTrIndempresatSp7Repository().GetByCriteria();
        }



        /// <summary>
        /// Permite realizar búsquedas en la tabla TrMuestrarisSp7
        /// </summary>

        public List<TrIndempresatSp7DTO> GetListaDispMensual(int emprcodi, DateTime fechaPeriodo)
        {
            return FactoryScada.GetTrIndempresatSp7Repository().GetListDispMensual(emprcodi, fechaPeriodo);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrReporteVersion
        /// </summary>
        public List<TrReporteversionSp7DTO> GetListaDispMensualVersion(int emprcodi, DateTime fechaPeriodo)
        {
            return FactoryScada.GetTrReporteversionSp7Repository().GetListaDispMensualVersion(emprcodi, fechaPeriodo);
        }



        /// <summary>
        /// Paginado en la tabla TrMuestrarisSp7
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public int GetPaginadoDispMensual(int emprcodi, DateTime fecha)
        {
            return FactoryScada.GetTrIndempresatSp7Repository().GetPaginadoDispMensual(emprcodi, fecha);
        }

        #endregion

        #region Métodos Tabla SC_EMPRESA

        /// <summary>
        /// Inserta un registro de la tabla SC_EMPRESA
        /// </summary>
        public void SaveScEmpresa(ScEmpresaDTO entity)
        {
            try
            {
                FactoryScada.GetScEmpresaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SC_EMPRESA
        /// </summary>
        public void UpdateScEmpresa(ScEmpresaDTO entity)
        {
            try
            {
                FactoryScada.GetScEmpresaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SC_EMPRESA
        /// </summary>
        public void DeleteScEmpresa(int emprcodi)
        {
            try
            {
                FactoryScada.GetScEmpresaRepository().Delete(emprcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SC_EMPRESA
        /// </summary>
        public ScEmpresaDTO GetByIdScEmpresa(int emprcodi)
        {
            return FactoryScada.GetScEmpresaRepository().GetById(emprcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SC_EMPRESA
        /// </summary>
        public List<ScEmpresaDTO> ListScEmpresas()
        {
            return FactoryScada.GetScEmpresaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla ScEmpresa
        /// </summary>
        public List<ScEmpresaDTO> GetByCriteriaScEmpresas()
        {
            return FactoryScada.GetScEmpresaRepository().GetByCriteria();
        }




        public ScEmpresaDTO GetInfoScEmpresa(int emprcodi)
        {
            return FactoryScada.GetScEmpresaRepository().GetInfoScEmpresa(emprcodi);
        }


        public List<ScEmpresaDTO> GetListaScEmpresa()
        {

            return FactoryScada.GetScEmpresaRepository().GetListaScEmpresa();

        }



        #endregion

        #region Métodos Tabla TR_ESTADCANAL_SP7

        /// <summary>
        /// Inserta un registro de la tabla TR_ESTADCANAL_SP7
        /// </summary>
        public void SaveTrEstadcanalSp7(TrEstadcanalSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrEstadcanalSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_ESTADCANAL_SP7
        /// </summary>
        public void UpdateTrEstadcanalSp7(TrEstadcanalSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrEstadcanalSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_ESTADCANAL_SP7
        /// </summary>
        public void DeleteTrEstadcanalSp7(int canalcodi, DateTime fecha)
        {
            try
            {
                FactoryScada.GetTrEstadcanalSp7Repository().Delete(canalcodi, fecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_ESTADCANAL_SP7
        /// </summary>
        public TrEstadcanalSp7DTO GetByIdTrEstadcanalSp7(int canalcodi, DateTime fecha)
        {
            return FactoryScada.GetTrEstadcanalSp7Repository().GetById(canalcodi, fecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_ESTADCANAL_SP7
        /// </summary>
        public List<TrEstadcanalSp7DTO> ListTrEstadcanalSp7s()
        {
            return FactoryScada.GetTrEstadcanalSp7Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrEstadcanalSp7
        /// </summary>
        public List<TrEstadcanalSp7DTO> GetByCriteriaTrEstadcanalSp7s()
        {
            return FactoryScada.GetTrEstadcanalSp7Repository().GetByCriteria();
        }


        public List<TrEstadcanalSp7DTO> GetDispDiaSignal(DateTime fecha, int emprcodi)
        {
            return FactoryScada.GetTrEstadcanalSp7Repository().GetDispDiaSignal(fecha, emprcodi);
        }

        public List<TrEstadcanalrSp7DTO> GetDispDiaSignalVersion(DateTime fecha, int emprcodi)
        {
            return FactoryScada.GetTrEstadcanalrSp7Repository().GetDispDiaSignal(fecha, emprcodi);
        }


        public int GetPaginadoDispDiaSignal(DateTime fecha, int emprcodi)
        {
            return FactoryScada.GetTrEstadcanalSp7Repository().GetPaginadoDispDiaSignal(fecha, emprcodi);
        }

        public int GetVersion(DateTime fecha)
        {
            return FactoryScada.GetTrVersionSp7Repository().GetVersion(fecha);
        }

        #endregion





    }
}
