using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mantto;
using COES.Servicios.Aplicacion.Migraciones;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace COES.Servicios.Aplicacion.Subastas
{
    /// <summary>
    /// Clases con métodos del módulo Subastas
    /// </summary>
    public class SubastasAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SubastasAppServicio));

        readonly DespachoAppServicio wsUrsCliente = new DespachoAppServicio();
        readonly MigracionesAppServicio servMigra = new MigracionesAppServicio();
        readonly CorreoAppServicio servCorreo = new CorreoAppServicio();

        #region Métodos Tablas SMA_*

        #region Métodos Tabla SMA_MAESTRO_MOTIVO

        /// <summary>
        /// Inserta un registro de la tabla SMA_MAESTRO_MOTIVO
        /// </summary>
        public void SaveSmaMaestroMotivo(SmaMaestroMotivoDTO entity)
        {
            try
            {
                FactorySic.GetSmaMaestroMotivoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_MAESTRO_MOTIVO
        /// </summary>
        public void UpdateSmaMaestroMotivo(SmaMaestroMotivoDTO entity)
        {
            try
            {
                FactorySic.GetSmaMaestroMotivoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_MAESTRO_MOTIVO
        /// </summary>
        public void DeleteSmaMaestroMotivo(int smammcodi)
        {
            try
            {
                FactorySic.GetSmaMaestroMotivoRepository().Delete(smammcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_MAESTRO_MOTIVO
        /// </summary>
        public SmaMaestroMotivoDTO GetByIdSmaMaestroMotivo(int smammcodi)
        {
            return FactorySic.GetSmaMaestroMotivoRepository().GetById(smammcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_MAESTRO_MOTIVO
        /// </summary>
        public List<SmaMaestroMotivoDTO> ListSmaMaestroMotivos()
        {
            return FactorySic.GetSmaMaestroMotivoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaMaestroMotivo
        /// </summary>
        public List<SmaMaestroMotivoDTO> GetByCriteriaSmaMaestroMotivos(string smammcodis)
        {
            return FactorySic.GetSmaMaestroMotivoRepository().GetByCriteria(smammcodis);
        }

        #endregion

        #region Métodos Tabla SMA_AMPLIACION_PLAZO

        /// <summary>
        /// Inserta un registro de la tabla SMA_AMPLIACION_PLAZO
        /// </summary>
        public void SaveSmaAmpliacionPlazo(SmaAmpliacionPlazoDTO entity)
        {
            try
            {
                FactorySic.GetSmaAmpliacionPlazoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_AMPLIACION_PLAZO
        /// </summary>
        public void UpdateSmaAmpliacionPlazo(SmaAmpliacionPlazoDTO entity)
        {
            try
            {
                FactorySic.GetSmaAmpliacionPlazoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_AMPLIACION_PLAZO
        /// </summary>
        public void DeleteSmaAmpliacionPlazo(int smaapcodi)
        {
            try
            {
                FactorySic.GetSmaAmpliacionPlazoRepository().Delete(smaapcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_AMPLIACION_PLAZO
        /// </summary>
        public SmaAmpliacionPlazoDTO GetByIdSmaAmpliacionPlazo(int smaapcodi)
        {
            return FactorySic.GetSmaAmpliacionPlazoRepository().GetById(smaapcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_AMPLIACION_PLAZO
        /// </summary>
        public List<SmaAmpliacionPlazoDTO> ListSmaAmpliacionPlazos()
        {
            return FactorySic.GetSmaAmpliacionPlazoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaAmpliacionPlazo
        /// </summary>
        public List<SmaAmpliacionPlazoDTO> GetByCriteriaSmaAmpliacionPlazos()
        {
            return FactorySic.GetSmaAmpliacionPlazoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SMA_ACTIVACION_OFERTA

        /// <summary>
        /// Inserta un registro de la tabla SMA_ACTIVACION_OFERTA
        /// </summary>
        public void SaveSmaActivacionOferta(SmaActivacionOfertaDTO entity)
        {
            try
            {
                FactorySic.GetSmaActivacionOfertaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla SMA_ACTIVACION_OFERTA
        /// </summary>       
        public int SaveSmaActivacionOfertaTransaccional(SmaActivacionOfertaDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetSmaActivacionOfertaRepository().SaveTransaccional(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_ACTIVACION_OFERTA
        /// </summary>
        public void UpdateSmaActivacionOferta(SmaActivacionOfertaDTO entity)
        {
            try
            {
                FactorySic.GetSmaActivacionOfertaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_ACTIVACION_OFERTA
        /// </summary>
        public void DeleteSmaActivacionOferta(int smapaccodi)
        {
            try
            {
                FactorySic.GetSmaActivacionOfertaRepository().Delete(smapaccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_ACTIVACION_OFERTA
        /// </summary>
        public SmaActivacionOfertaDTO GetByIdSmaActivacionOferta(int smapaccodi)
        {
            return FactorySic.GetSmaActivacionOfertaRepository().GetById(smapaccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_ACTIVACION_OFERTA
        /// </summary>
        public List<SmaActivacionOfertaDTO> ListSmaActivacionOfertas()
        {
            return FactorySic.GetSmaActivacionOfertaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaActivacionOferta
        /// </summary>
        public List<SmaActivacionOfertaDTO> GetByCriteriaSmaActivacionOfertas()
        {
            return FactorySic.GetSmaActivacionOfertaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SMA_ACTIVACION_DATA

        /// <summary>
        /// Inserta un registro de la tabla SMA_ACTIVACION_DATA
        /// </summary>
        public void SaveSmaActivacionData(SmaActivacionDataDTO entity)
        {
            try
            {
                FactorySic.GetSmaActivacionDataRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla SMA_ACTIVACION_DATA
        /// </summary>
        public int SaveSmaActivacionDataTransaccional(SmaActivacionDataDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetSmaActivacionDataRepository().SaveTransaccional(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_ACTIVACION_DATA
        /// </summary>
        public void UpdateSmaActivacionData(SmaActivacionDataDTO entity)
        {
            try
            {
                FactorySic.GetSmaActivacionDataRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_ACTIVACION_DATA
        /// </summary>
        public void DeleteSmaActivacionData(int smaacdcodi)
        {
            try
            {
                FactorySic.GetSmaActivacionDataRepository().Delete(smaacdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_ACTIVACION_DATA
        /// </summary>
        public SmaActivacionDataDTO GetByIdSmaActivacionData(int smaacdcodi)
        {
            return FactorySic.GetSmaActivacionDataRepository().GetById(smaacdcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_ACTIVACION_DATA
        /// </summary>
        public List<SmaActivacionDataDTO> ListSmaActivacionDatas()
        {
            return FactorySic.GetSmaActivacionDataRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaActivacionData
        /// </summary>
        public List<SmaActivacionDataDTO> GetByCriteriaSmaActivacionDatas()
        {
            return FactorySic.GetSmaActivacionDataRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SMA_ACTIVACION_MOTIVO

        /// <summary>
        /// Inserta un registro de la tabla SMA_ACTIVACION_MOTIVO
        /// </summary>
        public void SaveSmaActivacionMotivo(SmaActivacionMotivoDTO entity)
        {
            try
            {
                FactorySic.GetSmaActivacionMotivoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla SMA_ACTIVACION_MOTIVO
        /// </summary>
        public int SaveSmaActivacionMotivoTransaccional(SmaActivacionMotivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetSmaActivacionMotivoRepository().SaveTransaccional(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_ACTIVACION_MOTIVO
        /// </summary>
        public void UpdateSmaActivacionMotivo(SmaActivacionMotivoDTO entity)
        {
            try
            {
                FactorySic.GetSmaActivacionMotivoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_ACTIVACION_MOTIVO
        /// </summary>
        public void DeleteSmaActivacionMotivo(int smaacmcodi)
        {
            try
            {
                FactorySic.GetSmaActivacionMotivoRepository().Delete(smaacmcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_ACTIVACION_MOTIVO
        /// </summary>
        public SmaActivacionMotivoDTO GetByIdSmaActivacionMotivo(int smaacmcodi)
        {
            return FactorySic.GetSmaActivacionMotivoRepository().GetById(smaacmcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_ACTIVACION_MOTIVO
        /// </summary>
        public List<SmaActivacionMotivoDTO> ListSmaActivacionMotivos()
        {
            return FactorySic.GetSmaActivacionMotivoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaActivacionMotivo
        /// </summary>
        public List<SmaActivacionMotivoDTO> GetByCriteriaSmaActivacionMotivos()
        {
            return FactorySic.GetSmaActivacionMotivoRepository().GetByCriteria();
        }

        #endregion


        #region Métodos Tabla SMA_INDISPONIBILIDAD_TEMP_CAB

        /// <summary>
        /// Inserta un registro de la tabla SMA_INDISPONIBILIDAD_TEMP_CAB
        /// </summary>
        public void SaveSmaIndisponibilidadTempCab(SmaIndisponibilidadTempCabDTO entity)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTempCabRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la SMA_INDISPONIBILIDAD_TEMP_CAB
        /// </summary>
        public int SaveSmaIndisponibilidadTempCabTransaccional(SmaIndisponibilidadTempCabDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetSmaIndisponibilidadTempCabRepository().SaveTransaccional(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_INDISPONIBILIDAD_TEMP_CAB
        /// </summary>
        public void UpdateSmaIndisponibilidadTempCab(SmaIndisponibilidadTempCabDTO entity)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTempCabRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la SMA_INDISPONIBILIDAD_TEMP_CAB
        /// </summary>
        public void UpdateSmaIndisponibilidadTempCabTransaccional(SmaIndisponibilidadTempCabDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTempCabRepository().UpdateTransaccional(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_INDISPONIBILIDAD_TEMP_CAB
        /// </summary>
        public void DeleteSmaIndisponibilidadTempCab(int intcabcodi)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTempCabRepository().Delete(intcabcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_INDISPONIBILIDAD_TEMP_CAB
        /// </summary>
        public SmaIndisponibilidadTempCabDTO GetByIdSmaIndisponibilidadTempCab(int intcabcodi)
        {
            return FactorySic.GetSmaIndisponibilidadTempCabRepository().GetById(intcabcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_INDISPONIBILIDAD_TEMP_CAB
        /// </summary>
        public List<SmaIndisponibilidadTempCabDTO> ListSmaIndisponibilidadTempCabs()
        {
            return FactorySic.GetSmaIndisponibilidadTempCabRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaIndisponibilidadTempCab
        /// </summary>
        public List<SmaIndisponibilidadTempCabDTO> GetByCriteriaSmaIndisponibilidadTempCabs()
        {
            return FactorySic.GetSmaIndisponibilidadTempCabRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SMA_INDISPONIBILIDAD_TEMP_DET

        /// <summary>
        /// Inserta un registro de la tabla SMA_INDISPONIBILIDAD_TEMP_DET
        /// </summary>
        public void SaveSmaIndisponibilidadTempDet(SmaIndisponibilidadTempDetDTO entity)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTempDetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la SMA_INDISPONIBILIDAD_TEMP_DET
        /// </summary>
        public int SaveSmaIndisponibilidadTempDetTransaccional(SmaIndisponibilidadTempDetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetSmaIndisponibilidadTempDetRepository().SaveTransaccional(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_INDISPONIBILIDAD_TEMP_DET
        /// </summary>
        public void UpdateSmaIndisponibilidadTempDet(SmaIndisponibilidadTempDetDTO entity)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTempDetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_INDISPONIBILIDAD_TEMP_DET
        /// </summary>
        public void DeleteSmaIndisponibilidadTempDet(int intdetcodi)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTempDetRepository().Delete(intdetcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_INDISPONIBILIDAD_TEMP_DET
        /// </summary>
        public void DeleteSmaIndisponibilidadTempDetPorCabTransaccional(string intcabcodis, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTempDetRepository().DeletePorCabTransaccional(intcabcodis, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_INDISPONIBILIDAD_TEMP_DET
        /// </summary>
        public SmaIndisponibilidadTempDetDTO GetByIdSmaIndisponibilidadTempDet(int intdetcodi)
        {
            return FactorySic.GetSmaIndisponibilidadTempDetRepository().GetById(intdetcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_INDISPONIBILIDAD_TEMP_DET
        /// </summary>
        public List<SmaIndisponibilidadTempDetDTO> ListSmaIndisponibilidadTempDets()
        {
            return FactorySic.GetSmaIndisponibilidadTempDetRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaIndisponibilidadTempDet
        /// </summary>
        public List<SmaIndisponibilidadTempDetDTO> GetByCriteriaSmaIndisponibilidadTempDets()
        {
            return FactorySic.GetSmaIndisponibilidadTempDetRepository().GetByCriteria();
        }

        #endregion
        #region Métodos Tabla SMA_INDISPONIBILIDAD_TEMPORAL

        /// <summary>
        /// Inserta un registro de la tabla SMA_INDISPONIBILIDAD_TEMPORAL
        /// </summary>
        public void SaveSmaIndisponibilidadTemporal(SmaIndisponibilidadTemporalDTO entity)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTemporalRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_INDISPONIBILIDAD_TEMPORAL
        /// </summary>
        public void UpdateSmaIndisponibilidadTemporal(SmaIndisponibilidadTemporalDTO entity)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTemporalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_INDISPONIBILIDAD_TEMPORAL
        /// </summary>
        public void DeleteSmaIndisponibilidadTemporal(int smaintcodi)
        {
            try
            {
                FactorySic.GetSmaIndisponibilidadTemporalRepository().Delete(smaintcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_INDISPONIBILIDAD_TEMPORAL
        /// </summary>
        public SmaIndisponibilidadTemporalDTO GetByIdSmaIndisponibilidadTemporal(int smaintcodi)
        {
            return FactorySic.GetSmaIndisponibilidadTemporalRepository().GetById(smaintcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_INDISPONIBILIDAD_TEMPORAL
        /// </summary>
        public List<SmaIndisponibilidadTemporalDTO> ListSmaIndisponibilidadTemporals()
        {
            return FactorySic.GetSmaIndisponibilidadTemporalRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaIndisponibilidadTemporal
        /// </summary>
        public List<SmaIndisponibilidadTemporalDTO> GetByCriteriaSmaIndisponibilidadTemporals()
        {
            return FactorySic.GetSmaIndisponibilidadTemporalRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SMA_MODO_OPER_VAL

        /// <summary>
        /// Inserta un registro de la tabla SMA_MODO_OPER_VAL
        /// </summary>
        public string SaveSmaModoOperVal(SmaModoOperValDTO entity)
        {
            string resultDes = "";

            try
            {
                FactorySic.GetSmaModoOperValRepository().Save(entity);
                resultDes = "00|Registro satisfactorio...";
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                resultDes = "01|Error al regsitrar";

            }

            return resultDes;
        }

        /// <summary>
        /// Permite obtener el Número de Modo Valido
        /// </summary>
        /// <returns></returns>
        public int GetNumModoOperVal()
        {
            try
            {
                return FactorySic.GetSmaModoOperValRepository().GetNumVal();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_MODO_OPER_VAL
        /// </summary>
        public void UpdateSmaModoOperVal(SmaModoOperValDTO entity)
        {
            try
            {
                FactorySic.GetSmaModoOperValRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_MODO_OPER_VAL
        /// </summary>
        public void DeleteSmaModoOperVal(string user, int mopvcodi)
        {
            try
            {
                FactorySic.GetSmaModoOperValRepository().Delete(user, mopvcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_MODO_OPER_VAL
        /// </summary>
        public SmaModoOperValDTO GetByIdSmaModoOperVal(int mopvcodi)
        {
            return FactorySic.GetSmaModoOperValRepository().GetById(mopvcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_MODO_OPER_VAL
        /// </summary>
        public List<SmaModoOperValDTO> ListSmaModoOperVals(string grupocodi)
        {
            return FactorySic.GetSmaModoOperValRepository().List(grupocodi);
        }

        /// <summary>
        /// Permite listar los Modos de Operación Disponibles
        /// </summary>
        /// <param name="mopvgrupoval"></param>
        /// <param name="urscodi"></param>
        /// <returns></returns>
        public List<SmaModoOperValDTO> ListSmaModoOperDisponibles(int? mopvgrupoval, int urscodi)
        {
            return FactorySic.GetSmaModoOperValRepository().ListMOVal(mopvgrupoval, urscodi);
        }

        /// <summary>
        /// Permite listar todos los modos de operación válidos
        /// </summary>
        /// <returns></returns>
        public List<SmaModoOperValDTO> ListAllSmaModoOperVals()
        {
            return FactorySic.GetSmaModoOperValRepository().ListAll();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaModoOperVal
        /// </summary>
        public List<SmaModoOperValDTO> GetByCriteriaSmaModoOperVals()
        {
            return FactorySic.GetSmaModoOperValRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener modos de operación válidos por URS
        /// </summary>
        /// <param name="urscodi"></param>
        /// <returns></returns>
        public List<SmaModoOperValDTO> GetListMOValxUrs(int urscodi)
        {
            return FactorySic.GetSmaModoOperValRepository().GetListMOValxUrs(urscodi);
        }
        #endregion

        #region Métodos Tabla SMA_OFERTA

        /// <summary>
        /// Inserta un registro de la tabla SMA_OFERTA
        /// </summary>
        public string SaveSmaOferta(SmaOfertaDTO entity, List<SmaOfertaDetalleDTO> entityDet)
        {
            IDbConnection conn = null;
            DbTransaction tran = null;

            string resultDes = "";
            int resultOfer = -1;
            int resultOfeDet = -1;

            try
            {
                conn = FactorySic.GetSmaOfertaRepository().BeginConnection();
                tran = FactorySic.GetSmaOfertaRepository().StartTransaction(conn);

                //Guardamos SMA_Oferta (incluye dar de baja a otro envio con las mismas urs)
                string urscodisEnvio = string.Join(",", entityDet.Select(x => x.Urscodi).Distinct());
                resultOfer = FactorySic.GetSmaOfertaRepository().Save(entity, urscodisEnvio, conn, tran);

                //Si SMA_Oferta guardo exitosamente
                //Obtenemos los ids de SMA_OfertaDetalle y SMA_RelacionOdMo
                int corrOferdet = FactorySic.GetSmaOfertaDetalleRepository().GetMaxId();
                int corrMO = FactorySic.GetSmaRelacionOdMoRepository().GetMaxId();

                //obtenemos los tiposCarga del listado: (tipoCargaSubir= 1, tipoCargaBajar= 2)
                List<int> lstTiposCarga = entityDet.Select(x => x.Ofdetipo).Distinct().ToList();

                //Para cada tipoCarga (handsonSubir y handsonBajar)
                foreach (int tipoCarga in lstTiposCarga)
                {
                    //Agrupamos las ofertas por tipoCarga (ofertas handsontableSubir y ofertas handsontableBajar)
                    List<SmaOfertaDetalleDTO> ofertasSegunTipoCarga = entityDet.Where(x => x.Ofdetipo == tipoCarga).ToList();

                    List<int> lstUrsCodis = ofertasSegunTipoCarga.Select(x => x.Urscodi).Distinct().ToList();


                    //para cada URS del listado del handsontable (subir o bajar)
                    foreach (int urscodiX in lstUrsCodis)
                    {
                        //Obtengo las ofertas de cierta URS
                        var ofertasAgrupadaPorURS = ofertasSegunTipoCarga.Where(x => x.Urscodi == urscodiX).ToList();

                        List<string> lstHoraInis = ofertasAgrupadaPorURS.Select(x => x.Ofdehorainicio).Distinct().ToList();

                        //Ahora agrupamos segun su periodo
                        foreach (var horaIniX in lstHoraInis)
                        {
                            //Obtengo las ofertas de una URS especifica y de un horaIni especifico
                            var ofertasPorURSyHoraIni = ofertasAgrupadaPorURS.Where(x => x.Ofdehorainicio.Trim() == horaIniX.Trim()).ToList();

                            ///yu
                            if (ofertasPorURSyHoraIni.Count > 0)
                            {
                                //Obtengo cualquiera, solo para obtener datos generales de la URS y periodo diferente
                                //SmaOfertaDetalleDTO ofertaDetalleXUrs = ofertasAgrupadaPorURS.First();
                                SmaOfertaDetalleDTO ofertaDetalleXUrs = ofertasPorURSyHoraIni.First();

                                ofertaDetalleXUrs.Ofdedusucreacion = entity.Oferusucreacion;
                                ofertaDetalleXUrs.Ofdeprecio = this.EncryptData(ofertaDetalleXUrs.Ofdeprecio);

                                //Guardo SMA_OfertaDetalle
                                resultOfeDet = FactorySic.GetSmaOfertaDetalleRepository().Save(resultOfer, ofertaDetalleXUrs, conn, tran, corrOferdet);

                                if (resultOfeDet == -1)
                                {
                                    tran.Rollback();
                                    Logger.Error("Error insertando registro en Tabla OfertaDetalle!...");
                                    resultDes = "02|Error insertando registro ......";
                                    throw new Exception("Error insertando registro en Tabla OfertaDetalle...");
                                }
                                else
                                {
                                    //Ahora guardo los datos por cada ModoOperacion de la oferta de cierto URS y periodo
                                    //foreach (var ofertaConUrsX in ofertasAgrupadaPorURS)
                                    foreach (var ofertaConUrsX in ofertasPorURSyHoraIni)
                                    {
                                        SmaRelacionOdMoDTO entyMO = new SmaRelacionOdMoDTO();
                                        entyMO.Grupocodi = ofertaConUrsX.Grupocodi;
                                        entyMO.Odmobndcalificada = Convert.ToDecimal(ofertaConUrsX.BandaCalificada);
                                        entyMO.Odmobnddisponible = Convert.ToDecimal(ofertaConUrsX.BandaDisponible);
                                        entyMO.Odmousucreacion = entity.Oferusucreacion;
                                        if (FactorySic.GetSmaRelacionOdMoRepository().Save(resultOfeDet, entyMO, conn, tran, corrMO) == -1)
                                        {
                                            tran.Rollback();
                                            Logger.Error("Error insertando registro en Tabla RelacionOdMo!...");
                                            resultDes = "02|Error insertando registro ......";
                                            throw new Exception("Error insertando registro en Tabla RelacionOdMo...");
                                        }
                                        corrMO++;
                                    }
                                }
                                corrOferdet++;
                            }
                            ///yu
                        }
                    }
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                resultDes = "Error insertando registro ......";
                if (tran != null) tran.Rollback();

            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }

            return resultDes;
        }

        public void ResetearOfertaDefecto(DateTime fechaIniMes, DateTime fechaFinMes)
        {
            try
            {
                FactorySic.GetSmaOfertaRepository().ResetearOfertaDefecto(fechaIniMes, fechaFinMes);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public bool RevertirEstadoSmaOfertaSimetricaHorario(string id, int estado)
        {
            bool estadoEjecucionQuery = true;

            try
            {
                FactorySic.GetSmaOfertaRepository().RevertirEstadoSmaOfertaSimetricaHorario(id, estado);
            }
            catch (Exception ex)
            {
                estadoEjecucionQuery = false;
            }

            return estadoEjecucionQuery;
        }

        public bool CrearSmaOfertaSimetricaHorario(string horarioInicio, string horarioFin)
        {
            bool estadoEjecucionQuery = true;

            try
            {
                FactorySic.GetSmaOfertaRepository().CrearSmaOfertaSimetricaHorario(horarioInicio, horarioFin);
            }
            catch (Exception ex)
            {
                estadoEjecucionQuery = false;
                throw new ArgumentException(ex.Message);
            }

            return estadoEjecucionQuery;
        }

        public List<SmaOfertaSimetricaHorarioDTO> ListSmaOfertaSimetricaHorario()
        {
            return FactorySic.GetSmaOfertaRepository().ListSmaOfertaSimetricaHorario();
        }

        public bool ExisteVigenteSmaOfertaSimetricaHorario()
        {
            return FactorySic.GetSmaOfertaRepository().ExisteVigenteSmaOfertaSimetricaHorario();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_OFERTA
        /// </summary>
        public SmaOfertaDTO GetByIdSmaOferta(int ofercodi)
        {
            return FactorySic.GetSmaOfertaRepository().GetById(ofercodi);
        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_OFERTA
        /// </summary>
        public void UpdateSmaOferta(SmaOfertaDTO entity)
        {
            try
            {
                FactorySic.GetSmaOfertaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todas las ofertas en función a pámetros ingresados INTRANET (agrupados los modos de operacion)
        /// </summary>
        /// <param name="ofertipo"></param>
        /// <param name="oferfechaenvio"></param>
        /// <param name="oferfechaenviofin"></param>
        /// <param name="usercode"></param>
        /// <param name="ofercodi"></param>
        /// <param name="oferestado"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listurscodi"></param>
        /// <param name="oferfuente"> Campo que indica que la informacion fue cargada desde extranet o fue creado por activacion de oferta desde intranet por el sistema</param>
        /// <returns></returns>
        public List<SmaOfertaDTO> ListSmaOfertasInterna(int ofertipo, DateTime oferfechaenvio, DateTime oferfechaenviofin, int usercode, string ofercodi, string oferestado, int emprcodi, string listurscodi, string oferfuente)
        {
            return FactorySic.GetSmaOfertaRepository().ListInterna(ofertipo, oferfechaenvio, oferfechaenviofin, usercode, ofercodi, oferestado, emprcodi, listurscodi, oferfuente);
        }

        /// <summary>
        /// Permite listar las ENVIOS ofertas por Dia
        /// </summary>
        /// <param name="ofertipo"></param>
        /// <param name="oferfechaenvio"></param>
        /// <param name="usercode"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listUrs"></param>
        /// <returns></returns>
        public List<SmaOfertaDTO> ListSmaOfertasxDia(int ofertipo, DateTime oferfechaenvio, DateTime oferfechaenviofin, int usercode, int emprcodi, string listUrs, string oferfuente)
        {
            List<SmaOfertaDTO> list = FactorySic.GetSmaOfertaRepository().ListOfertasxDia(ofertipo, oferfechaenvio, oferfechaenviofin, usercode, emprcodi, listUrs, oferfuente);

            return list.OrderBy(x => x.Oferfechainicio).ThenByDescending(x => x.Oferfechaenvio).ToList();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_OFERTA (envios) para EXTRANET (sin agrupar los envios)
        /// </summary>
        public List<SmaOfertaDTO> ListSmaOfertas(int ofertipo, DateTime oferfechaenvio, int usercode, int ofercodi, string oferestado, bool mostrarSinEncriptacion)
        {
            List<SmaOfertaDTO> list = FactorySic.GetSmaOfertaRepository().List(ofertipo, oferfechaenvio, usercode, ofercodi, oferestado, ConstantesSubasta.FuenteExtranet);

            if (mostrarSinEncriptacion)
            {
                for (int i = 0; i < list.Count; i++)
                    if (list[i].Usercode == usercode)
                        if (list[i].Repoprecio != null)
                            if (this.AnalizarNumerico(list[i].Repoprecio) == false)
                                list[i].Repoprecio = this.DecryptData(list[i].Repoprecio);
            }

            list = this.DividirDataOferta(list);

            return list;
        }

        /// <summary>
        /// Permite analizar si un dato es numérico
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public Boolean AnalizarNumerico(string valor)
        {
            Boolean bresult = false;
            decimal number3;
            bresult = decimal.TryParse(valor, out number3);

            return bresult;
        }

        #endregion

        #region Métodos Tabla SMA_OFERTA_DETALLE

        /// <summary>
        /// Actualiza un registro de la tabla SMA_OFERTA_DETALLE
        /// </summary>
        public void UpdateSmaOfertaDetalle(SmaOfertaDetalleDTO entity)
        {
            try
            {
                FactorySic.GetSmaOfertaDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_OFERTA_DETALLE
        /// </summary>
        public void DeleteSmaOfertaDetalle(int ofdecodi)
        {
            try
            {
                FactorySic.GetSmaOfertaDetalleRepository().Delete(ofdecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaOfertaDetalle
        /// </summary>
        public int GetByCriteriaSmaOfertaDetalles(int Urscodi)
        {
            return FactorySic.GetSmaOfertaDetalleRepository().GetByCriteria(Urscodi);
        }

        #endregion

        #region Métodos Tabla SMA_PARAM_PROCESO

        /// <summary>
        /// Inserta un registro de la tabla SMA_PARAM_PROCESO
        /// </summary>
        public int SaveSmaParamProceso(SmaParamProcesoDTO entity)
        {
            try
            {
                int idx = FactorySic.GetSmaParamProcesoRepository().Save(entity);

                FactorySic.GetSmaParamProcesoRepository().UpdateInactive(idx);

                return idx;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_PARAM_PROCESO
        /// </summary>
        public List<SmaParamProcesoDTO> ListSmaParamProcesos()
        {
            var lista = FactorySic.GetSmaParamProcesoRepository().List();

            foreach (var reg in lista)
                reg.PapofeccreacionDesc = reg.Papofeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);

            return lista;
        }

        /// <summary>
        /// Obtener el registro de proceso que tiene la hora de ejecucion automatica
        /// </summary>
        /// <returns></returns>
        public SmaParamProcesoDTO GetParamValidoEnvioyProcesoAutomatico()
        {
            return FactorySic.GetSmaParamProcesoRepository().GetValidRangeNCP();
        }

        #endregion

        #region Métodos Tabla SMA_RELACION_OD_MO

        /// <summary>
        /// Actualiza un registro de la tabla SMA_RELACION_OD_MO
        /// </summary>
        public void UpdateSmaRelacionOdMo(SmaRelacionOdMoDTO entity)
        {
            try
            {
                FactorySic.GetSmaRelacionOdMoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_RELACION_OD_MO
        /// </summary>
        public void DeleteSmaRelacionOdMo(int odmocodi)
        {
            try
            {
                FactorySic.GetSmaRelacionOdMoRepository().Delete(odmocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_RELACION_OD_MO
        /// </summary>
        public SmaRelacionOdMoDTO GetByIdSmaRelacionOdMo(int odmocodi)
        {
            return FactorySic.GetSmaRelacionOdMoRepository().GetById(odmocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_RELACION_OD_MO
        /// </summary>
        public List<SmaRelacionOdMoDTO> ListSmaRelacionOdMos()
        {
            return FactorySic.GetSmaRelacionOdMoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaRelacionOdMo
        /// </summary>
        public List<SmaRelacionOdMoDTO> GetByCriteriaSmaRelacionOdMos()
        {
            return FactorySic.GetSmaRelacionOdMoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SMA_URS_MODO_OPERACION

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_URS_MODO_OPERACION
        /// </summary>
        public List<SmaUrsModoOperacionDTO> ListSmaUrsModoOperacions()
        {
            return FactorySic.GetSmaUrsModoOperacionRepository().List();
        }

        /// <summary>
        /// Permite listar todos los URS de la tabla SMA_URS_MODO_OPERACION
        /// </summary>
        public List<SmaUrsModoOperacionDTO> ListSmaUrsModoOperacions_Urs(int usercode)
        {
            return FactorySic.GetSmaUrsModoOperacionRepository().ListUrs(usercode);
        }

        public List<SmaUrsModoOperacionDTO> ListSmaUrsModoOperacions_InUrs(int usercode)
        {
            return FactorySic.GetSmaUrsModoOperacionRepository().ListInUrs(usercode);
        }

        public List<SmaUrsModoOperacionDTO> ListSmaUrsModoOperacions_MO(int urscodi)
        {
            return FactorySic.GetSmaUrsModoOperacionRepository().ListMO(urscodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaUrsModoOperacion
        /// </summary>
        public List<SmaUrsModoOperacionDTO> GetByCriteriaSmaUrsModoOperacions()
        {
            return FactorySic.GetSmaUrsModoOperacionRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SMA_USUARIO_URS

        /// <summary>
        /// Inserta un registro de la tabla SMA_USUARIO_URS
        /// </summary>
        public string SaveSmaUsuarioUrs(SmaUsuarioUrsDTO entity)
        {
            string resultDes = "";
            try
            {
                int[] idExistente;
                //Primero validar si Existe el registro

                List<SmaUsuarioUrsDTO> usuUrs = FactorySic.GetSmaUsuarioUrsRepository().GetUsuUrsAct(entity, "A");//Buscar activos
                idExistente = new Int32[usuUrs.Count];
                if (usuUrs.Count > 0)
                    for (int i = 0; i < usuUrs.Count; i++)
                        idExistente[i] = usuUrs[i].Uurscodi;

                FactorySic.GetSmaUsuarioUrsRepository().Save(entity);
                resultDes = "00|Registro satisfactorio";

                for (int i = 0; i < usuUrs.Count; i++)
                    FactorySic.GetSmaUsuarioUrsRepository().UpdateUsuAct(idExistente[i]);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                resultDes = "01|Error al regsitrar";
            }
            return resultDes;

        }

        /// <summary>
        /// Actualiza un registro de la tabla SMA_USUARIO_URS
        /// </summary>
        public void UpdateSmaUsuarioUrs(SmaUsuarioUrsDTO entity)
        {
            try
            {
                FactorySic.GetSmaUsuarioUrsRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SMA_USUARIO_URS
        /// </summary>
        public void DeleteSmaUsuarioUrs(int uurscodi, String user)
        {
            try
            {
                FactorySic.GetSmaUsuarioUrsRepository().Delete(uurscodi, user);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                int rc1 = ex.Message.IndexOf("ORA-00001");
                int rc2 = ex.Message.IndexOf("ORA-000033");

                if (ex.Message.IndexOf("ORA-00001") >= 0)
                    Logger.Error("Error porque se elimino previamente este registro, se conserva el primer eliminado");
                else
                    throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SMA_USUARIO_URS
        /// </summary>
        public SmaUsuarioUrsDTO GetByIdSmaUsuarioUrs(int uurscodi)
        {
            return FactorySic.GetSmaUsuarioUrsRepository().GetById(uurscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_USUARIO_URS
        /// </summary>
        public List<SmaUsuarioUrsDTO> ListSmaUsuarioUrss()
        {
            return FactorySic.GetSmaUsuarioUrsRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaUsuarioUrs
        /// </summary>
        public List<SmaUsuarioUrsDTO> GetByCriteriaSmaUsuarioUrss(int usercode)
        {
            return FactorySic.GetSmaUsuarioUrsRepository().GetByCriteria(usercode);
        }

        /// <summary>
        /// Permite obtener los datos de URS y Modos de Operación en función al usuario
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public List<SmaUsuarioUrsDTO> GetByCriteriaSmaUsuarioUrssMO(int usercode)
        {
            return FactorySic.GetSmaUsuarioUrsRepository().GetByCriteriaMO(usercode);
        }

        #endregion

        #region Métodos Tabla SMA_USER_EMPRESA

        /// <summary>
        /// Permite listar todos los registros de la tabla SMA_USER_EMPRESA
        /// </summary>
        public List<SmaUserEmpresaDTO> ListSmaUserEmpresa(int codigoEmpresa)
        {
            return FactorySic.GetSmaUserEmpresaRepository().List(codigoEmpresa);
        }

        /// <summary>
        /// Permite obtener el listado de empresas de Usuarios
        /// </summary>
        /// <param name="codigoUser"></param>
        /// <returns></returns>
        public List<SmaUserEmpresaDTO> ListEmpresaSmaUserEmpresa(int codigoUser)
        {
            return FactorySic.GetSmaUserEmpresaRepository().ListEmpresa(codigoUser);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SmaUserEmpresa
        /// </summary>
        public List<SmaUserEmpresaDTO> GetByCriteriaSmaUserEmpresa(int codigoEmpresa)
        {
            return FactorySic.GetSmaUserEmpresaRepository().GetByCriteria(codigoEmpresa);
        }

        #endregion

        #endregion

        #region Métodos Tabla PR_GRUPO
        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPO 
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public PrGrupoDTO GetByIdPrGrupo(int grupocodi)
        {
            return FactorySic.GetPrGrupoRepository().GetById(grupocodi);
        }
        #endregion

        #region Generar y Grabar Archivo NCP (Deprecated)

        ///// <summary>
        ///// Permite listar las ofertas en función a parámetros ingresados
        ///// </summary>
        ///// <param name="fecha"></param>
        ///// <param name="tipo"></param>
        ///// <param name="grupocodi"></param>
        ///// <param name="rangoHorario"></param>
        ///// <param name="mensajeresultado"></param>
        ///// <returns></returns>
        //public List<SmaOfertaDTO> ListReporteSmaOfertas(DateTime fecha, string tipo, int grupocodi, int rangoHorario, bool urs_rsv_firme, out string mensajeresultado)
        //{
        //    List<SmaOfertaDTO> mallaReporte = new List<SmaOfertaDTO>();
        //    List<SmaOfertaDTO> mallaFinal = new List<SmaOfertaDTO>();
        //    List<SmaOfertaDTO> listReporte;

        //    //Logger.Info("ListReporteSmaOfertas - Generar data reporte SmaOfertas - Termicas e Hidraulicas - fecha [" + fecha + "] tipo [" + tipo + "] grupocodi[" + grupocodi + "] rangoHorario [" + rangoHorario + "]");

        //    //if (this.EsValidoRangoNCP() == false)
        //    //{
        //    //    mensajeresultado = "02|Error en Generar Archivos NCP por que no esta en Horario permitido";
        //    //    return mallaFinal;
        //    //}

        //    if (tipo.Equals("CH") == true || tipo.Equals("CT") == true)
        //    {
        //        PrGrupodatDTO datoConf;
        //        for (int h = 0; h < 24; h++)
        //        {
        //            //Logger.Info("ListReporteSmaOfertas - ListReporteMin grupocodi[" + grupocodi + "]");
        //            listReporte = FactorySic.GetSmaOfertaRepository().ListReporteMin(grupocodi);
        //            if (listReporte.Count > 0)
        //            {
        //                if (rangoHorario == 1)
        //                {
        //                    listReporte[0].Repointvhini = h;
        //                    listReporte[0].Repointvmini = 0;
        //                    listReporte[0].Repointvhfin = h;
        //                    listReporte[0].Repointvmfin = 30;
        //                    datoConf = this.GetValorConfiguracion(DateTime.Now, ConstantesSubasta.PotenciaNoMinimo);
        //                    if (datoConf != null) listReporte[0].Repopotmaxofer = Convert.ToDecimal(datoConf.Formuladat);
        //                    mallaFinal.AddRange(listReporte);
        //                }
        //                else
        //                {
        //                    listReporte[0].Repointvhini = h;
        //                    listReporte[0].Repointvmini = 0;
        //                    listReporte[0].Repointvhfin = h;
        //                    listReporte[0].Repointvmfin = 30;
        //                    datoConf = this.GetValorConfiguracion(DateTime.Now, ConstantesSubasta.PotenciaNoMinimo);
        //                    if (datoConf != null) listReporte[0].Repopotmaxofer = Convert.ToDecimal(datoConf.Formuladat);
        //                    mallaFinal.AddRange(listReporte);
        //                    if (h == 23)
        //                    {
        //                        listReporte[0].Repointvhini = h;
        //                        listReporte[0].Repointvmini = 30;
        //                        listReporte[0].Repointvhfin = h;
        //                        listReporte[0].Repointvmfin = 59;
        //                    }
        //                    else
        //                    {
        //                        listReporte[0].Repointvhini = h;
        //                        listReporte[0].Repointvmini = 30;
        //                        listReporte[0].Repointvhfin = h + 1;
        //                        listReporte[0].Repointvmfin = 0;
        //                    }
        //                    datoConf = this.GetValorConfiguracion(DateTime.Now, ConstantesSubasta.PotenciaNoMinimo);
        //                    if (datoConf != null) listReporte[0].Repopotmaxofer = Convert.ToDecimal(datoConf.Formuladat);
        //                    mallaFinal.AddRange(listReporte);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //Logger.Info("ListReporteSmaOfertas - ListReporte x 24 horas ");
        //        bool val_zero = false;
        //        for (int h = 0; h < 24; h++)
        //        {
        //            val_zero = false;
        //            listReporte = FactorySic.GetSmaOfertaRepository().ListReporte(1, fecha, tipo, h, 0, h, 30, grupocodi);
        //            listReporte = (val_zero = (listReporte.Count == 0)) ? FactorySic.GetSmaOfertaRepository().ListReporte(0, (DateTime?)null, tipo, h, 0, h, 30, grupocodi) : listReporte;
        //            if (listReporte.Count > 0)
        //            {
        //                if (this.AnalizarNumerico(listReporte[0].Repoprecio) == false)
        //                {
        //                    //Logger.Info("ListReporteSmaOfertas - Decrypt data 1er intervalo 30 min [" + listReporte[0].Repoprecio + "]");
        //                    listReporte[0].Repoprecio = this.DecryptData(listReporte[0].Repoprecio);
        //                    //Logger.Info("ListReporteSmaOfertas - Actualizar Precio " + listReporte[0].Repoprecio);
        //                    FactorySic.GetSmaOfertaDetalleRepository().UpdatePrecio(listReporte[0].Ofdecodi, listReporte[0].Repoprecio);
        //                }
        //                if (val_zero && !urs_rsv_firme) //Si no es urs_reserva firme se completa con ceros
        //                {
        //                    listReporte[0].Repoprecio = "0.00";
        //                    listReporte[0].Repopotmaxofer = 0;
        //                }
        //                mallaReporte.AddRange(listReporte);
        //            }
        //            val_zero = false;
        //            listReporte = FactorySic.GetSmaOfertaRepository().ListReporte(1, fecha, tipo, h, 30, h + 1, 0, grupocodi);
        //            listReporte = (val_zero = (listReporte.Count == 0)) ? FactorySic.GetSmaOfertaRepository().ListReporte(0, (DateTime?)null, tipo, h, 30, h + 1, 0, grupocodi) : listReporte;
        //            if (listReporte.Count > 0)
        //            {
        //                if (this.AnalizarNumerico(listReporte[0].Repoprecio) == false)
        //                {
        //                    //Logger.Info("ListReporteSmaOfertas - Decrypt data 2do intervalo 30 min [" + listReporte[0].Repoprecio + "]");
        //                    listReporte[0].Repoprecio = this.DecryptData(listReporte[0].Repoprecio);
        //                    //Logger.Info("ListReporteSmaOfertas - Actualizar Precio " + listReporte[0].Repoprecio);
        //                    FactorySic.GetSmaOfertaDetalleRepository().UpdatePrecio(listReporte[0].Ofdecodi, listReporte[0].Repoprecio);
        //                }
        //                if (val_zero && !urs_rsv_firme) //Si no es urs_reserva firme se completa con ceros
        //                {
        //                    listReporte[0].Repoprecio = "0.00";
        //                    listReporte[0].Repopotmaxofer = 0;
        //                }
        //                mallaReporte.AddRange(listReporte);
        //            }
        //        }


        //        List<SmaOfertaDTO> mallaTemp;
        //        int grupoCodi = 0;
        //        int i = 1, j = 0, sgte = 0;

        //        if (mallaReporte.Count > 0)
        //        {
        //            mallaReporte = mallaReporte.OrderBy(x => x.Grupocodi).ToList();
        //            //Logger.Info("ListReporteSmaOfertas - Procesando cada intervalo ");

        //            while (true) // inicio while
        //            {
        //                if (i > 48)
        //                    break; // Nuevo termino por GRUIPOCODI

        //                mallaTemp = new List<SmaOfertaDTO>();
        //                mallaTemp.AddRange(mallaReporte.Select(x => x.Copy()));
        //                if (i == mallaReporte[j].Repointvnum)
        //                {
        //                    if (rangoHorario == 1)
        //                    {
        //                        if (i % 2 != 0)
        //                        {
        //                            if (i != 47)
        //                            {
        //                                mallaTemp[j].Repointvnum = mallaTemp[j].Repointvnum / 2 + 1;
        //                                mallaTemp[j].Repointvmfin = 0;
        //                                mallaTemp[j].Repointvhfin = mallaTemp[j].Repointvhfin + 1;

        //                            }
        //                            else
        //                            {
        //                                mallaTemp[j].Repointvnum = mallaTemp[j].Repointvnum / 2 + 1;
        //                                mallaTemp[j].Repointvmfin = 59;
        //                                mallaTemp[j].Repointvhfin = mallaTemp[j].Repointvhfin;
        //                            }
        //                            mallaFinal.Add(mallaTemp[j]);
        //                        }
        //                    }
        //                    else
        //                        mallaFinal.Add(mallaReporte[j]);

        //                    grupoCodi = mallaReporte[j].Grupocodi;
        //                    j++;
        //                    if (mallaReporte.Count == j)
        //                    {
        //                        j--;
        //                    }
        //                }
        //                else
        //                {
        //                    if (grupoCodi == mallaReporte[j].Grupocodi || grupoCodi == 0 || sgte == 1)
        //                    {
        //                        sgte = 0;
        //                        mallaTemp[j].Repointvnum = i;
        //                        mallaTemp[j].Repopotmaxofer = 0;
        //                        mallaTemp[j].Repoprecio = "0.00";

        //                        if (i % 2 == 0) //SI es PAR
        //                        {
        //                            if (i == 48)
        //                            {
        //                                mallaTemp[j].Repointvhini = 23;
        //                                mallaTemp[j].Repointvmini = 30;
        //                                mallaTemp[j].Repointvhfin = 23;
        //                                mallaTemp[j].Repointvmfin = 59;
        //                            }
        //                            else
        //                            {
        //                                mallaTemp[j].Repointvhini = i / 2 - 1;
        //                                mallaTemp[j].Repointvmini = 30;
        //                                mallaTemp[j].Repointvhfin = i / 2;
        //                                mallaTemp[j].Repointvmfin = 0;
        //                            }
        //                        }
        //                        else //Si es IMPAR
        //                        {
        //                            mallaTemp[j].Repointvhini = i / 2;
        //                            mallaTemp[j].Repointvmini = 0;
        //                            mallaTemp[j].Repointvhfin = i / 2;
        //                            mallaTemp[j].Repointvmfin = 30;
        //                        }
        //                        if (rangoHorario == 1)
        //                        {
        //                            if (i % 2 != 0)
        //                            {
        //                                if (i != 47)
        //                                {
        //                                    mallaTemp[j].Repointvnum = mallaTemp[j].Repointvnum / 2 + 1;
        //                                    mallaTemp[j].Repointvmfin = 0;
        //                                    mallaTemp[j].Repointvhfin = mallaTemp[j].Repointvhfin + 1;

        //                                }
        //                                else
        //                                {
        //                                    mallaTemp[j].Repointvnum = mallaTemp[j].Repointvnum / 2 + 1;
        //                                    mallaTemp[j].Repointvmfin = 59;
        //                                    mallaTemp[j].Repointvhfin = mallaTemp[j].Repointvhfin;
        //                                }
        //                                mallaFinal.Add(mallaTemp[j]);
        //                            }
        //                        }
        //                        else
        //                            mallaFinal.Add(mallaTemp[j]);
        //                    }
        //                    else
        //                    {
        //                        mallaTemp[j - 1].Repointvnum = i;
        //                        mallaTemp[j - 1].Repopotmaxofer = 0;
        //                        mallaTemp[j - 1].Repoprecio = "0.00";

        //                        if (i % 2 == 0) //SI es PAR
        //                        {
        //                            if (i == 48)
        //                            {
        //                                mallaTemp[j - 1].Repointvhini = 23;
        //                                mallaTemp[j - 1].Repointvmini = 30;
        //                                mallaTemp[j - 1].Repointvhfin = 23;
        //                                mallaTemp[j - 1].Repointvmfin = 59;
        //                            }
        //                            else
        //                            {
        //                                mallaTemp[j - 1].Repointvhini = i / 2 - 1;
        //                                mallaTemp[j - 1].Repointvmini = 30;
        //                                mallaTemp[j - 1].Repointvhfin = i / 2;
        //                                mallaTemp[j - 1].Repointvmfin = 0;
        //                            }
        //                        }
        //                        else //Si es IMPAR
        //                        {
        //                            mallaTemp[j - 1].Repointvhini = i / 2;
        //                            mallaTemp[j - 1].Repointvmini = 0;
        //                            mallaTemp[j - 1].Repointvhfin = i / 2;
        //                            mallaTemp[j - 1].Repointvmfin = 30;
        //                        }
        //                        if (rangoHorario == 1)
        //                        {
        //                            if (i % 2 != 0)
        //                            {
        //                                if (i != 47)
        //                                {
        //                                    mallaTemp[j - 1].Repointvnum = mallaTemp[j - 1].Repointvnum / 2 + 1;
        //                                    mallaTemp[j - 1].Repointvmfin = 0;
        //                                    mallaTemp[j - 1].Repointvhfin = mallaTemp[j - 1].Repointvhfin + 1;

        //                                }
        //                                else
        //                                {
        //                                    mallaTemp[j - 1].Repointvnum = mallaTemp[j - 1].Repointvnum / 2 + 1;
        //                                    mallaTemp[j - 1].Repointvmfin = 59;
        //                                    mallaTemp[j - 1].Repointvhfin = mallaTemp[j - 1].Repointvhfin;
        //                                }
        //                                mallaFinal.Add(mallaTemp[j - 1]);
        //                            }
        //                        }
        //                        else
        //                            mallaFinal.Add(mallaTemp[j - 1]);
        //                    }

        //                }
        //                i++;
        //            } //fin de while...

        //        }



        //    }
        //    mensajeresultado = "00|Archivos NCP generados correctamente";
        //    return mallaFinal;
        //}

        ///// <summary>
        ///// Permite listar las Ofertas agrupadas por URS
        ///// </summary>
        ///// <param name="fecha"></param>
        ///// <param name="tipo"></param>
        ///// <param name="urscodi"></param>
        ///// <param name="rangoHorario"></param>
        ///// <param name="mensajeresultado"></param>
        ///// <returns></returns>
        //public List<SmaOfertaDTO> ListUrsReporteSmaOfertas(DateTime fecha, string tipo, int urscodi, int rangoHorario, bool urs_rsv_firme, out string mensajeresultado)
        //{

        //    List<SmaOfertaDTO> mallaReporte = new List<SmaOfertaDTO>();
        //    List<SmaOfertaDTO> listReporte;
        //    List<SmaOfertaDTO> mallaFinal = new List<SmaOfertaDTO>();
        //    List<SmaOfertaDTO> mallaTemp;
        //    int pos = 0;
        //    bool val_zero = false;

        //    if (tipo == "U")
        //        //Logger.Info("ListUrsReporteSmaOfertas - Generar data URS SmaOfertas -  fecha [" + fecha + "] tipo [" + tipo + "] urscodi[" + urscodi + "] rangoHorario [" + rangoHorario + "]");

        //        //if (this.EsValidoRangoNCP() == false)
        //        //{
        //        //    mensajeresultado = "02|Error en Generar Archivos NCP por que no esta en Horario permitido";
        //        //    return mallaFinal;
        //        //}
        //        if (tipo.Equals("CU") == true)
        //        {
        //            //Logger.Info("ListUrsReporteSmaOfertas - Empezando a procesar con URSReporteMin urscodi = [" + urscodi + "]");

        //            PrGrupodatDTO datoConf;
        //            for (int h = 0; h < 24; h++)
        //            {
        //                listReporte = FactorySic.GetSmaOfertaRepository().ListUrsReporteMin(urscodi);
        //                if (listReporte.Count > 0)
        //                {
        //                    if (rangoHorario == 1)
        //                    {
        //                        listReporte[0].Repointvhini = h;
        //                        listReporte[0].Repointvmini = 0;
        //                        listReporte[0].Repointvhfin = h;
        //                        listReporte[0].Repointvmfin = 30;
        //                        if (listReporte[0].Urstipo == "A")
        //                            datoConf = this.GetValorConfiguracion(DateTime.Now, ConstantesSubasta.PotenciaURSMinimoAuto);
        //                        else
        //                            datoConf = this.GetValorConfiguracion(DateTime.Now, ConstantesSubasta.PotenciaURSMinimoMan);

        //                        if (datoConf != null) listReporte[0].Repopotmaxofer = Convert.ToDecimal(datoConf.Formuladat);
        //                        mallaFinal.AddRange(listReporte);
        //                    }
        //                    else
        //                    {
        //                        listReporte[0].Repointvhini = h;
        //                        listReporte[0].Repointvmini = 0;
        //                        listReporte[0].Repointvhfin = h;
        //                        listReporte[0].Repointvmfin = 30;
        //                        if (listReporte[0].Urstipo == "A")
        //                            datoConf = this.GetValorConfiguracion(DateTime.Now, ConstantesSubasta.PotenciaURSMinimoAuto);
        //                        else
        //                            datoConf = this.GetValorConfiguracion(DateTime.Now, ConstantesSubasta.PotenciaURSMinimoMan);
        //                        if (datoConf != null) listReporte[0].Repopotmaxofer = Convert.ToDecimal(datoConf.Formuladat);
        //                        mallaFinal.AddRange(listReporte);
        //                        if (h == 23)
        //                        {
        //                            listReporte[0].Repointvhini = h;
        //                            listReporte[0].Repointvmini = 30;
        //                            listReporte[0].Repointvhfin = h;
        //                            listReporte[0].Repointvmfin = 59;
        //                        }
        //                        else
        //                        {
        //                            listReporte[0].Repointvhini = h;
        //                            listReporte[0].Repointvmini = 30;
        //                            listReporte[0].Repointvhfin = h + 1;
        //                            listReporte[0].Repointvmfin = 0;
        //                        }
        //                        if (listReporte[0].Urstipo == "A")
        //                            datoConf = this.GetValorConfiguracion(DateTime.Now, ConstantesSubasta.PotenciaURSMinimoAuto);
        //                        else
        //                            datoConf = this.GetValorConfiguracion(DateTime.Now, ConstantesSubasta.PotenciaURSMinimoMan);
        //                        if (datoConf != null) listReporte[0].Repopotmaxofer = Convert.ToDecimal(datoConf.Formuladat);
        //                        mallaFinal.AddRange(listReporte);
        //                    }

        //                }

        //            }
        //        }
        //        else
        //        {
        //            for (int h = 0; h < 24; h++)
        //            {
        //                val_zero = false;
        //                listReporte = FactorySic.GetSmaOfertaRepository().ListUrsReporte(1, fecha, tipo, h, 0, h, 30, urscodi);
        //                listReporte = ((val_zero = listReporte.Count == 0)) ? FactorySic.GetSmaOfertaRepository().ListUrsReporte(0, (DateTime?)null, tipo, h, 0, h, 30, urscodi) : listReporte;
        //                if (listReporte.Count > 0)
        //                {
        //                    if (this.AnalizarNumerico(listReporte[0].Repoprecio) == false)
        //                    {
        //                        //Logger.Info("ListUrsReporteSmaOfertas - Decrypt data 1er intervalo 30 min [" + listReporte[0].Repoprecio + "]");
        //                        listReporte[0].Repoprecio = this.DecryptData(listReporte[0].Repoprecio);
        //                        FactorySic.GetSmaOfertaDetalleRepository().UpdatePrecio(listReporte[0].Ofdecodi, listReporte[0].Repoprecio);
        //                    }
        //                    if (tipo == "U") listReporte[0].Repopotmaxofer = listReporte[0].Repopotofer; // Actualizar por Potencia Ofertada
        //                    if (val_zero && !urs_rsv_firme) //Si no es urs_reserva firme se completa con ceros
        //                    {
        //                        listReporte[0].Repoprecio = "0.00";
        //                        listReporte[0].Repopotmaxofer = 0;
        //                    }
        //                    mallaReporte.AddRange(listReporte);

        //                }

        //                val_zero = false;
        //                listReporte = FactorySic.GetSmaOfertaRepository().ListUrsReporte(1, fecha, tipo, h, 30, h + 1, 0, urscodi);
        //                listReporte = ((val_zero = listReporte.Count == 0)) ? FactorySic.GetSmaOfertaRepository().ListUrsReporte(0, (DateTime?)null, tipo, h, 30, h + 1, 0, urscodi) : listReporte;
        //                if (listReporte.Count > 0)
        //                {
        //                    if (this.AnalizarNumerico(listReporte[0].Repoprecio) == false)
        //                    {
        //                        //Logger.Info("ListUrsReporteSmaOfertas - Decrypt data 2do intervalo 30 min [" + listReporte[0].Repoprecio + "]");
        //                        listReporte[0].Repoprecio = this.DecryptData(listReporte[0].Repoprecio);
        //                        FactorySic.GetSmaOfertaDetalleRepository().UpdatePrecio(listReporte[0].Ofdecodi, listReporte[0].Repoprecio);
        //                    }
        //                    if (tipo == "U") listReporte[0].Repopotmaxofer = listReporte[0].Repopotofer; // Actualizar por Potencia Ofertada
        //                    if (val_zero && !urs_rsv_firme) //Si no es urs_reserva firme se completa con ceros
        //                    {
        //                        listReporte[0].Repoprecio = "0.00";
        //                        listReporte[0].Repopotmaxofer = 0;
        //                    }
        //                    mallaReporte.AddRange(listReporte);
        //                }

        //            }

        //            int maxrows = mallaReporte.Count;
        //            if (mallaReporte.Count > 0)
        //            {
        //                mallaReporte = mallaReporte.OrderBy(x => x.Urscodi).ToList();
        //                //Logger.Info("ListUrsReporteSmaOfertas - Empezando a procesar cada intervalo...");
        //                int ursCodi = 0;
        //                int i = 1, j = 0, sgte = 0;

        //                while (true) // inicio de while..
        //                {

        //                    if (i > maxrows)
        //                    {
        //                        break; // Nuevo termino por GRUPOCODI
        //                    }
        //                    mallaTemp = new List<SmaOfertaDTO>();
        //                    mallaTemp.AddRange(mallaReporte.Select(x => x.Copy()));

        //                    if (i == mallaReporte[j].Repointvnum)
        //                    {
        //                        if (rangoHorario == 1)
        //                        {
        //                            if (i % 2 != 0)
        //                            {
        //                                if (i != 47)
        //                                {
        //                                    mallaTemp[j].Repointvnum = mallaTemp[j].Repointvnum / 2 + 1;
        //                                    mallaTemp[j].Repointvmfin = 0;
        //                                    mallaTemp[j].Repointvhfin = mallaTemp[j].Repointvhfin + 1;

        //                                }
        //                                else
        //                                {
        //                                    mallaTemp[j].Repointvnum = mallaTemp[j].Repointvnum / 2 + 1;
        //                                    mallaTemp[j].Repointvmfin = 59;
        //                                    mallaTemp[j].Repointvhfin = mallaTemp[j].Repointvhfin;
        //                                }
        //                                mallaFinal.Add(mallaTemp[j]);
        //                            }
        //                        }
        //                        else
        //                            mallaFinal.Add(mallaReporte[j]);

        //                        ursCodi = mallaReporte[j].Urscodi;
        //                        j++;
        //                        if (mallaReporte.Count == j)
        //                        {
        //                            j--;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (ursCodi == mallaReporte[j].Urscodi || ursCodi == 0 || sgte == 1)
        //                        {
        //                            sgte = 0;
        //                            mallaTemp[j].Repointvnum = i;
        //                            //
        //                            for (int k = 0; k < 48; k++)
        //                                if (mallaReporte[k].Repointvnum == i) { pos = k; break; }
        //                            //
        //                            mallaTemp[j].Repopotmaxofer = mallaReporte[pos].Repopotmaxofer; // valor por defecto0;
        //                            mallaTemp[j].Repoprecio = mallaReporte[pos].Repoprecio;// valor por defecto "0.00";

        //                            if (i % 2 == 0) //SI es PAR
        //                            {
        //                                if (i == 48)
        //                                {
        //                                    mallaTemp[j].Repointvhini = 23;
        //                                    mallaTemp[j].Repointvmini = 30;
        //                                    mallaTemp[j].Repointvhfin = 23;
        //                                    mallaTemp[j].Repointvmfin = 59;
        //                                }
        //                                else
        //                                {
        //                                    mallaTemp[j].Repointvhini = i / 2 - 1;
        //                                    mallaTemp[j].Repointvmini = 30;
        //                                    mallaTemp[j].Repointvhfin = i / 2;
        //                                    mallaTemp[j].Repointvmfin = 0;
        //                                }
        //                            }
        //                            else //Si es IMPAR
        //                            {
        //                                mallaTemp[j].Repointvhini = i / 2;
        //                                mallaTemp[j].Repointvmini = 0;
        //                                mallaTemp[j].Repointvhfin = i / 2;
        //                                mallaTemp[j].Repointvmfin = 30;
        //                            }

        //                            if (rangoHorario == 1)
        //                            {
        //                                if (i % 2 != 0)
        //                                {
        //                                    if (i != 47)
        //                                    {
        //                                        mallaTemp[j].Repointvnum = mallaTemp[j].Repointvnum / 2 + 1;
        //                                        mallaTemp[j].Repointvmfin = 0;
        //                                        mallaTemp[j].Repointvhfin = mallaTemp[j].Repointvhfin + 1;

        //                                    }
        //                                    else
        //                                    {
        //                                        mallaTemp[j].Repointvnum = mallaTemp[j].Repointvnum / 2 + 1;
        //                                        mallaTemp[j].Repointvmfin = 59;
        //                                        mallaTemp[j].Repointvhfin = mallaTemp[j].Repointvhfin;
        //                                    }
        //                                    mallaFinal.Add(mallaTemp[j]);
        //                                }
        //                            }
        //                            else
        //                                mallaFinal.Add(mallaTemp[j]);
        //                        }
        //                        else
        //                        {
        //                            mallaTemp[j - 1].Repointvnum = i;
        //                            //
        //                            for (int k = 0; k < 48; k++)
        //                                if (mallaReporte[k].Repointvnum == i) { pos = k; break; }
        //                            //
        //                            mallaTemp[j - 1].Repopotmaxofer = mallaReporte[pos].Repopotmaxofer; // valor por defecto0;
        //                            mallaTemp[j - 1].Repoprecio = mallaReporte[pos].Repoprecio;//"0.00";

        //                            if (i % 2 == 0) //SI es PAR
        //                            {
        //                                if (i == 48)
        //                                {
        //                                    mallaTemp[j - 1].Repointvhini = 23;
        //                                    mallaTemp[j - 1].Repointvmini = 30;
        //                                    mallaTemp[j - 1].Repointvhfin = 23;
        //                                    mallaTemp[j - 1].Repointvmfin = 59;
        //                                }
        //                                else
        //                                {
        //                                    mallaTemp[j - 1].Repointvhini = i / 2 - 1;
        //                                    mallaTemp[j - 1].Repointvmini = 30;
        //                                    mallaTemp[j - 1].Repointvhfin = i / 2;
        //                                    mallaTemp[j - 1].Repointvmfin = 0;
        //                                }
        //                            }
        //                            else //Si es IMPAR
        //                            {
        //                                mallaTemp[j - 1].Repointvhini = i / 2;
        //                                mallaTemp[j - 1].Repointvmini = 0;
        //                                mallaTemp[j - 1].Repointvhfin = i / 2;
        //                                mallaTemp[j - 1].Repointvmfin = 30;
        //                            }

        //                            if (rangoHorario == 1)
        //                            {
        //                                if (i % 2 != 0)
        //                                {
        //                                    if (i != 47)
        //                                    {
        //                                        mallaTemp[j - 1].Repointvnum = mallaTemp[j - 1].Repointvnum / 2 + 1;
        //                                        mallaTemp[j - 1].Repointvmfin = 0;
        //                                        mallaTemp[j - 1].Repointvhfin = mallaTemp[j - 1].Repointvhfin + 1;

        //                                    }
        //                                    else
        //                                    {
        //                                        mallaTemp[j - 1].Repointvnum = mallaTemp[j - 1].Repointvnum / 2 + 1;
        //                                        mallaTemp[j - 1].Repointvmfin = 59;
        //                                        mallaTemp[j - 1].Repointvhfin = mallaTemp[j - 1].Repointvhfin;
        //                                    }
        //                                    mallaFinal.Add(mallaTemp[j - 1]);
        //                                }
        //                            }
        //                            else
        //                                mallaFinal.Add(mallaTemp[j - 1]);

        //                        }

        //                    }
        //                    i++;
        //                } // fin de while...
        //            }

        //        }

        //    mensajeresultado = "00|Archivos NCP generados correctamente";
        //    return mallaFinal;

        //}

        ///// <summary>
        ///// Permite ejecutar el proceso de generación de archivos NCP
        ///// </summary>
        ///// <param name="idProceso"></param>
        //public void GenerarArchivosAutomatico(int idProceso)
        //{
        //    SiProcesoDTO proceso = FactorySic.GetSiProcesoRepository().ObtenerParametros(idProceso);

        //    if (proceso != null)
        //    {
        //        if (proceso.PathFile != null)
        //        {
        //            string result = this.GenerarArchivos(proceso.PathFile);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Permite genenar los archivos NCP
        ///// </summary>
        ///// <param name="ruta"></param>
        ///// <param name="arrNombreArchivos"></param>
        ///// <param name="arrHoras"></param>
        ///// <param name="pathFile"></param>
        ///// <returns></returns>
        //public string GenerarArchivos(string pathFile)
        //{

        //    System.Collections.Generic.Dictionary<string, string> arrNombreArchivos = new Dictionary<string, string>();

        //    arrNombreArchivos.Add("T", "cprmxt30pe.dat");
        //    arrNombreArchivos.Add("H", "cprmxh30pe.dat");
        //    arrNombreArchivos.Add("U", "cprmxg30pe.dat");
        //    arrNombreArchivos.Add("CU", "cprmng30pe.dat");
        //    arrNombreArchivos.Add("CH", "cprmnh30pe.dat");
        //    arrNombreArchivos.Add("CT", "cprmnt30pe.dat");
        //    arrNombreArchivos.Add("PU", "cprprg30pe.dat");
        //    arrNombreArchivos.Add("PH", "cprprh30pe.dat");
        //    arrNombreArchivos.Add("PT", "cprprt30pe.dat");

        //    string resultado = "Archivos Generados Satisfactoriamente";

        //    if (this.EsValidoRangoNCP() == false)
        //    {
        //        return "No se puede generar archivos NCP en este horario!";
        //    }

        //    try
        //    {

        //        string strFecha = DateTime.Now.AddDays(1).ToString("yyyyMMdd"); //Sumar +1 Dia por Fecha de Participacion
        //        bool flag = FileServer.CreateFolder(pathFile, strFecha, string.Empty);

        //        pathFile = string.Format("{0}\\{1}\\", pathFile, strFecha);

        //        string[] arrTipo = new string[] { "T", "H", "U", "CU", "CH", "CT" };

        //        for (int i = 0, j = 2; i < j; i++)
        //        {
        //            foreach (string strTipo in arrTipo)
        //            {
        //                this.CrearArchivo(strFecha, strTipo, i, arrNombreArchivos, arrHoras, pathFile);
        //            }
        //        }

        //        Stream stream1 = new MemoryStream();

        //        using (StreamWriter swCondirg = new StreamWriter(stream1))
        //        {

        //            swCondirg.AutoFlush = true;
        //            //Logger.Info("Listando datos de ListCondir - ListCondir");
        //            string[,] arrCondirg = this.ListCondir();

        //            swCondirg.Write(arrCondirg[0, 0].PadLeft(8, ' ') + " " + arrCondirg[0, 1].PadLeft(8, ' '));//arrCondirg[0, 0].PadLeft(8, ' ') + " " + arrCondirg[0, 1].PadLeft(8, ' '));
        //            swCondirg.Write(Environment.NewLine);
        //            swCondirg.Write(arrCondirg[1, 0].PadLeft(8, ' ') + " " + arrCondirg[1, 1].PadLeft(8, ' '));
        //            swCondirg.BaseStream.Position = 0;

        //            FileServer.UploadFromStream(stream1, pathFile, "condirg.dat", string.Empty);
        //        }

        //        Stream stream = new MemoryStream();

        //        using (StreamWriter swAgentagr = new StreamWriter(stream)) // new StreamWriter(string.Format("{0}agentagr.dat", _strRuta)))
        //        {
        //            swAgentagr.AutoFlush = true;
        //            //Logger.Info("Listando Datos de AgentaGr - ListAgentagr");
        //            int _intInicioSemana = 1;

        //            DateTime dteFecha = new DateTime(int.Parse(strFecha.Substring(0, 4)), int.Parse(strFecha.Substring(4, 2)), int.Parse(strFecha.Substring(6, 2)));
        //            int intDiaSemana = dteFecha.DayOfWeek.GetHashCode();

        //            if (intDiaSemana == 7 - _intInicioSemana)
        //            {
        //                intDiaSemana = -1;
        //            }

        //            DateTime dteFechainicio = dteFecha.AddDays((-1 * _intInicioSemana) - intDiaSemana);
        //            string[,] arrAgentagr = this.ListAgentagr(dteFechainicio);

        //            for (int i = 0, j = arrAgentagr.GetLength(0); i < j; i++)
        //            {
        //                if (i > 0)
        //                {
        //                    swAgentagr.Write(Environment.NewLine);
        //                }

        //                for (int m = 0, n = arrAgentagr.GetLength(1); m < n; m++)
        //                {
        //                    swAgentagr.Write((m == 0 ? "" : " ") + (arrAgentagr[i, m] == null ? " ".PadLeft(4, ' ') : arrAgentagr[i, m].PadLeft(4, ' ')));
        //                }
        //            }

        //            swAgentagr.BaseStream.Position = 0;
        //            FileServer.UploadFromStream(stream, pathFile, "agentagr.dat", string.Empty);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = "Error Generando Archivos NCP";
        //        Logger.Error("Error Generando Archivos NCP " + ex);
        //    }





        //    return resultado;
        //}


        ///// <summary>
        ///// Permite crear los archivos NCP en disco
        ///// </summary>
        ///// <param name="fecha"></param>
        ///// <param name="tipo"></param>
        ///// <param name="incremento"></param>
        ///// <param name="arrNombreArchivos"></param>
        ///// <param name="arrHoras"></param>
        ///// <param name="pathFile"></param>
        //public void CrearArchivo(string fecha, string tipo, int incremento, Dictionary<string, string> arrNombreArchivos, string[] arrHoras, string pathFile)
        //{
        //    int _intInicioSemana = 1;
        //    string respuesta = "";
        //    bool urs_rsv_firme = false;

        //    if (fecha == null || fecha.Trim().Length == 0)
        //    {
        //        throw new Exception("No ha especificado la fecha.");
        //    }

        //    if (fecha.Trim().Length != 8)
        //    {
        //        throw new Exception("La fecha no tiene la longitud correcta.");
        //    }

        //    DateTime dteFecha = new DateTime(int.Parse(fecha.Substring(0, 4)), int.Parse(fecha.Substring(4, 2)), int.Parse(fecha.Substring(6, 2)));

        //    int intDiaSemana = dteFecha.DayOfWeek.GetHashCode();

        //    if (intDiaSemana == 7 - _intInicioSemana)
        //    {
        //        intDiaSemana = -1;
        //    }

        //    DateTime dteFechainicio = dteFecha.AddDays((-1 * _intInicioSemana) - intDiaSemana);

        //    string strFechaInicio = dteFechainicio.ToString("yyyyMMdd");

        //    bool blnEsUrs = (tipo == "U" || tipo == "PU" || tipo == "CU");

        //    List<SmaOfertaDTO> arrCabecera;

        //    if (tipo == "CH" || tipo == "CT" )
        //    {
        //        //Logger.Info("Listado de Rango de URS Minimo - ListRangeURSMin || Listado de Rango Modo Operación Minimo - ListRangeMOMin");
        //        arrCabecera = (blnEsUrs ? this.ListRangeURSMin() : this.ListRangeMOMin(tipo.Substring(1, 1)));
        //    }
        //    else
        //    {
        //        //Logger.Info("Listando Lista de Rango URS - ListRangeURS || Listando Rango Modo Operación - ListRangeMO");
        //        arrCabecera = (blnEsUrs ? this.ListRangeURS(dteFechainicio) : this.ListRangeMO(dteFechainicio, (tipo.Length == 1) ? tipo : tipo.Substring(1, 1)));
        //    }

        //    if (arrCabecera != null && arrCabecera.Count > 0)
        //    {
        //        string strArchivoPotencia = arrNombreArchivos[tipo],
        //            strArchivoPrecio = ((tipo == "U" || tipo == "H" || tipo == "T") ? arrNombreArchivos[string.Format("P{0}", tipo)] : null);

        //        if (incremento > 0)
        //        {
        //            strArchivoPotencia = strArchivoPotencia.Replace("30", "");

        //            if (strArchivoPrecio != null)
        //            {
        //                strArchivoPrecio = strArchivoPrecio.Replace("30", "");
        //            }
        //        }

        //        Stream streamPotencia = new MemoryStream();
        //        Stream streamPrecio = new MemoryStream();

        //        using (StreamWriter swPotencia = new StreamWriter(streamPotencia),//, string.Format("{0}{1}", _strRuta, strArchivoPotencia), false),
        //            swPrecio = (strArchivoPrecio != null ? new StreamWriter(streamPrecio) : null))
        //        {
        //            StreamWriter[] streams = new StreamWriter[] { swPotencia, swPrecio };

        //            #region [ Lógica ]
        //            bool sinUnidades = true;
        //            if (incremento == 0 || ((incremento == 1) && (tipo == "H" || tipo == "T" || tipo == "CH" || tipo == "CT")))
        //            {
        //                sinUnidades = false;
        //                this.EscribirUnidades(streams);
        //            }


        //            int nfila = 0;
        //            foreach (SmaOfertaDTO objMO in arrCabecera)
        //            {
        //                nfila++;
        //                int intCodiNcp = (blnEsUrs ? objMO.Urscodi : objMO.Grupocodincp);
        //                int intGrupoCodi = (blnEsUrs ? objMO.Urscodi : objMO.Grupocodi);
        //                string strNomb = (blnEsUrs ? objMO.Ursnomb : objMO.Grupoabrev);

        //                //Averiguar si es Urs_Reserva_Firme
        //                List<SmaUrsModoOperacionDTO> datosGrupo = null;
        //                if (blnEsUrs)
        //                    datosGrupo = this.ListSmaUrsModoOperacions_MO(objMO.Urscodi);

        //                int intGrupoCodiReserva = (blnEsUrs ? (int)datosGrupo[0].Grupocodi : objMO.Grupocodi);
        //                string strGrupoTipo = (blnEsUrs ? datosGrupo[0].Grupotipo : objMO.Grupotipo);

        //                urs_rsv_firme = this.EsReservaFime(intGrupoCodiReserva, strGrupoTipo, dteFechainicio);
        //                // Termina Urs_Reserva_Firme

        //                List<SmaOfertaDTO> arrDetalle = null;
        //                bool pintaCabecera = false;
        //                for (int i = 0, j = 6; i <= j; i++)
        //                {
        //                    DateTime dteFechaConsulta = dteFechainicio.AddDays(i);
        //                    if (dteFechaConsulta <= dteFecha)
        //                    {
        //                        //Logger.Info("Lista de Ofertas por URS - ListUrsReporteSmaOfertas || Lista de Reporte de Ofertas - ListReporteSmaOfertas");
        //                        arrDetalle = (blnEsUrs ? this.ListUrsReporteSmaOfertas(dteFechaConsulta, tipo, intGrupoCodi, incremento, urs_rsv_firme, out respuesta).OrderBy(o => o.Urscodi).ToList<SmaOfertaDTO>() :
        //                            this.ListReporteSmaOfertas(dteFechaConsulta, tipo, intGrupoCodi, incremento, urs_rsv_firme, out respuesta).OrderBy(o => o.Grupocodi).ToList<SmaOfertaDTO>());
        //                    }
        //                    if (arrDetalle.Count > 0)
        //                    {
        //                        if (!pintaCabecera) { this.EscribirCabecera(strArchivoPotencia, streams, intCodiNcp, strNomb.Trim(), incremento, sinUnidades, blnEsUrs, nfila, arrHoras);  pintaCabecera = true; }
        //                        this.EscribirFila(tipo, streams, dteFechaConsulta, arrDetalle, incremento, arrHoras);
        //                    }
        //                }
        //            }

        //            #endregion


        //            streams[0].BaseStream.Position = 0;
        //            bool flag = FileServer.UploadFromStream(streams[0].BaseStream, pathFile, strArchivoPotencia, string.Empty);


        //            if (strArchivoPrecio != null) { streams[1].BaseStream.Position = 0; FileServer.UploadFromStream(streams[1].BaseStream, pathFile, strArchivoPrecio, string.Empty); }

        //        }


        //    }
        //}

        //private string GetGrupoTipo(int grupoCodi)
        //{
        //    string grupoTipo = "";

        //    return grupoTipo;
        //}
        /////
        //private bool EsReservaFime(int intGrupoCodi, string strGrupoTipo, DateTime dteFechainicio){

        //                decimal ProvisionPaseFirme=0;
        //                bool urs_rsv_firme = false;
        //                var obtdatmo = wsUrsCliente.ObtenerDatosMO_URS(intGrupoCodi, dteFechainicio);
        //                if (obtdatmo.Count > 0)
        //                {
        //                    for (int j = 0; j < obtdatmo.Count; j++)
        //                    {
        //                        if (strGrupoTipo == "T")
        //                        {
        //                            switch (obtdatmo[j].Concepcodi)
        //                            {
        //                                case 253:
        //                                    ProvisionPaseFirme = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat)  : 0;
        //                                    break;
        //                            }
        //                        }
        //                        else
        //                        { //Es Hidraulica
        //                            switch (obtdatmo[j].Concepcodi)
        //                            {
        //                                case 254:
        //                                    ProvisionPaseFirme = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat)  : 0;
        //                                    break;

        //                            }
        //                        }
        //                    }
        //                }
        //                if(ProvisionPaseFirme>0) urs_rsv_firme = true;

        //                return urs_rsv_firme;
        //}

        ///// <summary>
        ///// Genera el encabezado
        ///// </summary>
        //private void EscribirCabecera(string archivo, StreamWriter[] streamWriters, int codi, string nomb, int incremento, bool sinUnidades, bool esUrs, int nfila, string[] arrHoras)
        //{
        //    int i30 = archivo.IndexOf("30");

        //    foreach (StreamWriter streamWriter in streamWriters)
        //    {
        //        if (streamWriter != null)
        //        {
        //            if (!sinUnidades)
        //            {
        //                streamWriter.Write(System.Environment.NewLine);
        //                if (esUrs)
        //                    streamWriter.Write("****" + Convert.ToString(nfila).PadLeft(13, ' ') + " " + nomb);
        //                else
        //                    streamWriter.Write("****" + Convert.ToString(codi).PadLeft(13, ' ') + " " + nomb);
        //            }
        //            else
        //            {
        //                if (nfila > 1) streamWriter.Write(System.Environment.NewLine);
        //                if (esUrs)
        //                    streamWriter.Write("****" + Convert.ToString(nfila).PadLeft(13, ' ') + " " + nomb);
        //                else
        //                    streamWriter.Write("****" + Convert.ToString(codi).PadLeft(13, ' ') + " " + nomb);
        //            }
        //            streamWriter.Flush();


        //            streamWriter.Write(System.Environment.NewLine);
        //            streamWriter.Write("dd/mm/aaaa");

        //            for (int m = 0, n = arrHoras.Length; m < n; m += (incremento + 1))
        //            {
        //                if (i30 > 0)
        //                    streamWriter.Write(" ." + arrHoras[m] + "h");
        //                else
        //                    streamWriter.Write(" ...." + arrHoras[m].Substring(0, 2) + "h");

        //            }

        //            streamWriter.Flush();
        //        }
        //    }



        //}



        ///// <summary>
        ///// Genera el encabezado de Unidades
        ///// </summary>
        //private void EscribirUnidades(StreamWriter[] streamWriters)
        //{
        //    foreach (StreamWriter streamWriter in streamWriters)
        //    {
        //        if (streamWriter != null)
        //        {
        //            streamWriter.Write("Unidades   :    1  (1=MW,2=%G)");
        //            streamWriter.Flush();
        //        }
        //    }
        //}

        ///// <summary>
        ///// Permite escribir los registros de los archivos generados
        ///// </summary>
        ///// <param name="tipo"></param>
        ///// <param name="streamWriters"></param>
        ///// <param name="fecha"></param>
        ///// <param name="arrSmaOfertaDTO"></param>
        ///// <param name="incremento"></param>
        ///// <param name="arrHoras"></param>
        //private void EscribirFila(string tipo, StreamWriter[] streamWriters, DateTime fecha, List<SmaOfertaDTO> arrSmaOfertaDTO, int incremento, string[] arrHoras)
        //{
        //    for (int i = 0, j = streamWriters.Length; i < j; i++)
        //    {
        //        StreamWriter streamWriter = streamWriters[i];

        //        if (streamWriter != null)
        //        {
        //            streamWriter.Write(System.Environment.NewLine);
        //            streamWriter.Write(fecha.ToString("dd/MM/yyyy"));

        //            if (arrSmaOfertaDTO != null && arrSmaOfertaDTO.Count > 0)
        //            {
        //                foreach (SmaOfertaDTO objSmaOfertaDTOAntes in arrSmaOfertaDTO)
        //                {
        //                    if (incremento == 0)
        //                        streamWriter.Write(" " + (i == 0 && tipo.Substring(0, 1) != "P" ? Convert.ToString(objSmaOfertaDTOAntes.Repopotmaxofer).PadLeft(7, ' ') : Convert.ToString(Math.Round(Convert.ToDecimal(objSmaOfertaDTOAntes.Repoprecio) / 2, 2)).PadLeft(7, ' ')));
        //                    else
        //                        streamWriter.Write(" " + (i == 0 && tipo.Substring(0, 1) != "P" ? Convert.ToString(objSmaOfertaDTOAntes.Repopotmaxofer).PadLeft(7, ' ') : Convert.ToString(objSmaOfertaDTOAntes.Repoprecio).PadLeft(7, ' ')));
        //                }
        //                streamWriter.Flush();
        //            }
        //            else
        //            {
        //                for (int m = 0, n = arrHoras.Length; m < n; m += (incremento + 1))
        //                {
        //                    streamWriter.Write(" " + "0".PadLeft(7, ' '));
        //                }
        //                streamWriter.Flush();
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Permite listar los datos para el archivo NCP Agentagr
        ///// </summary>
        ///// <param name="fecha"></param>
        ///// <returns></returns>
        //public string[,] ListAgentagr(DateTime? fecha)
        //{
        //    string[,] listagen;

        //    List<SmaOfertaDTO> lista = FactorySic.GetSmaOfertaRepository().ListAgentagr(fecha);
        //    listagen = new string[lista.GroupBy(x => x.Ursnomb).Count() + 1, lista.Count * 2 + 3];

        //    int j = 1, urscodi = 0, contador = 0, numncp = 0, maxnumncp = 0;

        //    if (lista.Count > 0)
        //    {
        //        for (int i = 0; i < lista.Count; i++)
        //        {
        //            if (lista[i].Urscodi != urscodi) // el primer registro o cambio de registro
        //            {
        //                // completar el URS del registro anterior
        //                if (urscodi != 0)
        //                {
        //                    listagen[j, contador++] = "!" + lista[i - 1].Ursnomb;  //(NombreURS);
        //                    listagen[j, 1] = Convert.ToString(numncp);  //(CantiMO);
        //                    if (numncp > maxnumncp) { maxnumncp = numncp; };
        //                    j++;
        //                }
        //                else
        //                    maxnumncp = numncp;
        //                contador = 0;
        //                numncp = 0;
        //                listagen[j, contador++] = Convert.ToString(j);
        //                contador++;//separar espacio del numero de MO/va al final
        //            }

        //            listagen[j, contador++] = lista[i].Grupocodincp.ToString();
        //            listagen[j, contador++] = lista[i].Grupotipo;
        //            numncp++;

        //            urscodi = lista[i].Urscodi;
        //        }

        //        //completar ultimo registro
        //        listagen[j, contador++] = "!" + lista[lista.Count - 1].Ursnomb;  //(NombreURS);
        //        listagen[j, 1] = Convert.ToString(numncp);

        //        //Cabecera
        //        listagen[0, 0] = "CODU";
        //        listagen[0, 1] = "NPLT";
        //        for (j = 1; j <= maxnumncp; j++)
        //        {
        //            listagen[0, 2 * j] = (j > 9) ? "C#0" + Convert.ToString(j - 9) : "C#0" + Convert.ToString(j);
        //            listagen[0, 2 * j + 1] = "TYPE";
        //        }

        //    }


        //    return listagen;
        //}

        ///// <summary>
        ///// Permite listar los datos para el archivo NCP CONDIRG
        ///// </summary>
        ///// <returns></returns>
        //public string[,] ListCondir()
        //{
        //    string[,] listacondir;

        //    listacondir = new string[2, 2];

        //    listacondir[0, 0] = ".CODURG.";
        //    listacondir[0, 1] = ".CONINI.";
        //    listacondir[1, 0] = "2";
        //    listacondir[1, 1] = "3";

        //    return listacondir;
        //}


        #endregion

        #region Parámetros Subastas

        /// <summary>
        /// Permite obtener la lista de parámetros de configuración para subastas
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="conceptos"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ListarParametrosConfiguracionPorFecha(DateTime fecha, string conceptos, string grupos = "0")
        {
            List<PrGrupodatDTO> lista = FactorySic.GetPrGrupodatRepository().ParametrosConfiguracionPorFecha(fecha, grupos, conceptos);

            ////Agregar parámetro eliminado para mostrar en listado
            //var eliminados = FactorySic.GetPrGrupodatRepository().ParametrosConfiguracionEliminados(fecha, grupos, conceptos);

            //var lista2 = lista.Select(x=>x.Concepcodi).ToList();
            //eliminados = eliminados.Where((x) => !(lista2.Contains(x.Concepcodi))).ToList();

            //foreach (var item in eliminados)
            //{
            //    lista.Add(item);
            //}
            //
            foreach (var reg in lista)
            {
                reg.FechadatDesc = reg.Fechadat != null ? reg.Fechadat.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
                reg.Lastuser = reg.Lastuser != null ? reg.Lastuser.Trim() : "SISTEMA";
                reg.FechaactDesc = reg.Fechaact != null ? reg.Fechaact.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
            }

            return lista;
        }

        /// <summary>
        /// Permite obtener los datos de Configuración en función de concepcodi
        /// 
        /// </summary>
        /// <param name="atributo"></param>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public PrGrupodatDTO GetValorConfiguracion(DateTime fecha, int concepcodi)
        {
            List<PrGrupodatDTO> lista = FactorySic.GetPrGrupodatRepository().ParametrosConfiguracionPorFecha(fecha, "0", concepcodi.ToString());
            var paramConfig = lista.First();
            return paramConfig;
        }

        #endregion

        #region Maestro de Motivos de activación de oferta por defecto

        public List<SmaMaestroMotivoDTO> ListarMaestroMotivoOfDefecto()
        {
            var lista = ListSmaMaestroMotivos().OrderBy(x => x.Smammdescripcion).ToList();

            foreach (var obj in lista)
            {
                FormatearSmaMaestroMotivo(obj);
            }

            return lista;
        }

        private void FormatearSmaMaestroMotivo(SmaMaestroMotivoDTO obj)
        {
            obj.SmammestadoDesc = "";
            if (ConstantesSubasta.EstadoActivo == obj.Smammestado) obj.SmammestadoDesc = "Activo";
            if (ConstantesSubasta.EstadoInactivo == obj.Smammestado) obj.SmammestadoDesc = "Inactivo";

            obj.SmammfeccreacionDesc = obj.Smammfeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull2);

            obj.Smammusumodificacion = obj.Smammusumodificacion ?? "";

            obj.SmammfecmodificacionDesc = "";
            if (obj.Smammfecmodificacion != null) obj.SmammfecmodificacionDesc = obj.Smammfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
        }

        public void GuardarMotivo(SmaMaestroMotivoDTO obj, string usuario)
        {
            var listaBD = ListarMaestroMotivoOfDefecto();

            //validar existencia
            var objConMotivoExistente = listaBD.Find(x => x.Smammcodi != obj.Smammcodi && x.Smammdescripcion.ToUpper() == obj.Smammdescripcion.ToUpper());
            if (objConMotivoExistente != null) throw new ArgumentException("El Motivo registrado ya existe en el listado. ");

            //validar cambio
            if (obj.Smammcodi > 0)
            {
                var objExistente = listaBD.Find(x => x.Smammcodi == obj.Smammcodi);
                if (objExistente.Smammdescripcion.ToUpper() == obj.Smammdescripcion.ToUpper()
                    && objExistente.Smammestado == obj.Smammestado)
                {
                    throw new ArgumentException("No se detectó cambios en el registro. ");
                }
            }

            //transaccional
            if (obj.Smammcodi == 0)
            {
                obj.Smammfeccreacion = DateTime.Now;
                obj.Smammusucreacion = usuario;
                SaveSmaMaestroMotivo(obj);
            }
            else
            {
                var objExistente = listaBD.Find(x => x.Smammcodi == obj.Smammcodi);
                objExistente.Smammdescripcion = obj.Smammdescripcion;
                objExistente.Smammestado = obj.Smammestado;
                objExistente.Smammfecmodificacion = DateTime.Now;
                objExistente.Smammusumodificacion = usuario;

                UpdateSmaMaestroMotivo(objExistente);
            }
        }

        #endregion

        #region URS Calificadas

        /// <summary>
        /// permite juntar la data de la URS con la de GrupoDat 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<SmaUrsModoOperacionDTO> ReporteListadoURS(DateTime fecha, string estado)
        {
            List<SmaUrsModoOperacionDTO> listaURSCompleto = new List<SmaUrsModoOperacionDTO>();
            List<SmaUrsModoOperacionDTO> listaTodasURS = new List<SmaUrsModoOperacionDTO>();

            listaTodasURS = GetByCriteriaSmaUrsModoOperacions();
            var listaURSDistinc = ListSmaUrsModoOperacions_Urs(-1);

            //captura de la data
            var lstGrupoCodiURS = listaTodasURS.Select(x => x.Urscodi).Distinct().ToList();
            var lstGrupoCodiModo = listaTodasURS.Select(x => x.Grupocodi.Value).Distinct().ToList();
            lstGrupoCodiURS.AddRange(lstGrupoCodiModo);

            List<PrGrupodatDTO> listaGrupoDatURS = this.ListarParametrosConfiguracionPorFecha(fecha, ConstantesSubasta.ConcepcodiURSyModosCalificadas, string.Join(",", lstGrupoCodiURS));
            //List<PrGrupodatDTO> listaGrupoDatURS = FactorySic.GetPrGrupodatRepository().ParametrosConfiguracionPorFecha(fecha, string.Join(",", lstGrupoCodiURS), ConstantesSubasta.ConcepcodiURSyModosCalificadas);

            foreach (var urs in listaURSDistinc)
            {
                int cantModos = listaURSDistinc.Count();

                //obtener valores de la URS mediante PrGrupoDat
                var ursFecIni = listaGrupoDatURS.Find(x => x.Grupocodi == urs.Urscodi & x.Concepcodi == ConstantesSubasta.ConceptoURSFecInicio);
                var ursFecFin = listaGrupoDatURS.Find(x => x.Grupocodi == urs.Urscodi & x.Concepcodi == ConstantesSubasta.ConceptoURSFecfIn);
                var ursActa = listaGrupoDatURS.Find(x => x.Grupocodi == urs.Urscodi & x.Concepcodi == ConstantesSubasta.ConceptoURSActa);
                var ursBanda = listaGrupoDatURS.Find(x => x.Grupocodi == urs.Urscodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBanda);

                // empresas
                var ursEmpresas = listaTodasURS.Where(x => x.Urscodi == urs.Urscodi).OrderBy(y => y.Emprnomb).ToList();

                foreach (var item in ursEmpresas.GroupBy(x => x.Emprcodi))
                {

                    var listaModos = item.OrderBy(y => y.Gruponomb).ToList();
                    var codempresa = item.Key;
                    //obtiene la urs para una empresa de la agrupación
                    var ursEmpresa = listaTodasURS.Find(x => x.Emprcodi == codempresa);

                    foreach (var modo in listaModos)
                    {
                        //Obtener valores de los modos de la URS
                        PrGrupodatDTO modPmin = new PrGrupodatDTO();
                        PrGrupodatDTO modPmax = new PrGrupodatDTO();
                        PrGrupodatDTO modBanda = new PrGrupodatDTO();
                        PrGrupodatDTO modComentario = new PrGrupodatDTO();
                        switch (modo.Catecodi)
                        {
                            case 5:
                                modPmin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoGrupHidraPotenciaMin);
                                modPmax = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoGrupHidraPotenciaMax);
                                modBanda = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoGrupHidraBandaCalificada);
                                modComentario = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoGrupHidraComentario);
                                break;
                            case 9:
                                modPmin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoMOHidroPotenciaMin);
                                modPmax = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoMOHidroPotenciaMax);
                                modBanda = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoMOHidroBandaCalificada);
                                modComentario = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoMOHidroComentario);
                                break;
                            case 2:
                                modPmin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoMOTermoPotenciaMin);
                                modPmax = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoMOTermoPotenciaMax);
                                modBanda = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoMOTermoBandaCalificada);
                                modComentario = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoMOTermoComentario);
                                break;
                        }

                        //NIVEL DE MODO
                        decimal? ValorNull = null;
                        modo.PMin = modPmin != null ? Convert.ToDecimal(modPmin.Formuladat) : ValorNull;
                        modo.PMax = modPmax != null ? Convert.ToDecimal(modPmax.Formuladat) : ValorNull;
                        modo.BandaCalificada = modBanda != null ? Convert.ToDecimal(modBanda.Formuladat) : ValorNull;
                        modo.Comentario = modComentario != null ? modComentario.Formuladat : string.Empty;


                        //nivel de URS
                        modo.FechaInico = ursFecIni != null ? ursFecIni.Formuladat.Trim() : string.Empty;
                        modo.FechaFin = ursFecFin != null ? ursFecFin.Formuladat.Trim() : string.Empty;
                        modo.Acta = ursActa != null ? ursActa.Formuladat : string.Empty;
                        modo.BandaURS = ursBanda != null ? Convert.ToDecimal(ursBanda.Formuladat) : ValorNull;

                        //validar estad vigencia
                        if (ursFecIni != null && ursFecFin != null)
                        {
                            var fecIniURS = DateTime.ParseExact(modo.FechaInico, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                            var fecFinURS = DateTime.ParseExact(modo.FechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                            modo.Estado = (fecha.Date >= fecIniURS && fecha.Date <= fecFinURS) ? "Vigente" : "No Vigente";

                            //Obtener datos de potencia efectiva, minima de los modos de operación
                            this.ObtenerFechaYMensajeBandaCalificada(fecIniURS, fecFinURS, out string mensaje, out DateTime fechaConsulta);
                            this.ObtenerDatosPotEfecMinRpfByModo(modo.Grupocodi.Value, modo.Grupotipo, fechaConsulta, out decimal? bandaDisponible, out decimal capacidadMax);
                            modo.BandaDisponible = bandaDisponible;
                            modo.MensajeBandaCalificada = mensaje;
                        }
                        else
                        {
                            modo.Estado = string.Empty;
                            modo.MensajeBandaCalificada = string.Empty;
                        }

                        modo.FechaModif = ursFecIni != null ? ursFecIni.FechaactDesc : string.Empty;
                        modo.UsuarioModif = ursFecIni != null ? ursFecIni.Lastuser : string.Empty;

                        listaURSCompleto.Add(modo);
                    }
                }
            }


            if (ConstantesSubasta.EstadoURSVigente == estado)
            {
                //listar las urs que tengan modos de operación vigentes según la fecha de consulta
                listaURSCompleto = listaURSCompleto.Where(x => x.Estado.ToUpper() == "VIGENTE").ToList();
            }

            return listaURSCompleto;
        }

        /// <summary>
        /// Reporte de URS Calificadas HTML 
        /// </summary>
        /// <returns></returns>
        public string ReporteListadoURSCalificadasHtml(string url, DateTime fecha, bool tienePermisoEditar)
        {
            StringBuilder str = new StringBuilder();
            List<SmaUrsModoOperacionDTO> listaTodasURS = ReporteListadoURS(fecha, ConstantesAppServicio.ParametroDefecto);
            List<SmaUrsModoOperacionDTO> listaURSDistinc = ListSmaUrsModoOperacions_Urs(-1); //cabecera

            str.Append("<div id='resultado' style='height: auto;'>");
            //str.Append("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: 2200px;' >");
            str.Append("<table id='tabla_agrupacion' border='0' class='pretty tabla-icono' cellspacing='0'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 75px'>Acción</th>");
            str.Append("<th style='width: 150px'>URS</th>");
            str.Append("<th style='width: 240px'>Empresa</th>");
            str.Append("<th style='width: 240px'>Central</th>");
            str.Append("<th style='width: 240px'>UNIDAD /<br /> MODO DE OPERACIÓN</th>");
            str.Append("<th style='width: 120px'>Pminrsf<br/>(MW)</th>");
            str.Append("<th style='width: 120px'>Pmxrsf<br/>(MW)</th>");
            str.Append("<th style='width: 120px'>Banda<br/>(MW)</th>");
            str.Append("<th style='width: 120px'>Banda URS <br/>Calificada</th>");
            str.Append("<th style='width: 120px'>Banda <br/>Disponible</th>");
            str.Append("<th style='width: 120px'>Fecha Inicio</th>");
            str.Append("<th style='width: 120px'>Fecha Fin</th>");
            str.Append("<th style='width: 70px'>Estado</th>");
            str.Append("<th style='width: 250px'>Comentario</th>");
            str.Append("<th style='width: 120px'>Usuario<br />  modificación</th>");
            str.Append("<th style='width: 120px'>Fecha<br />  modificación</th>");
            str.Append("</tr>");
            #endregion

            str.Append("</thead>");
            str.Append("<tbody>");
            #region cuerpo
            foreach (var urs in listaURSDistinc)
            {
                int cantModos = listaURSDistinc.Count();

                // empresas
                var ursEmpresas = listaTodasURS.Where(x => x.Urscodi == urs.Urscodi).OrderBy(y => y.Emprnomb).ToList();

                int numeroModosTotales = 0;
                foreach (var item in ursEmpresas.GroupBy(x => x.Emprcodi))
                {
                    numeroModosTotales = numeroModosTotales + item.ToList().Count;
                }

                string clase = "";
                clase = ursEmpresas.First().Estado == "No Vigente" ? "clase_eliminado" : "";
                var acta = ursEmpresas.First().Acta;

                str.Append("<tr class='" + clase + "'>");

                str.AppendFormat("<td rowspan='" + numeroModosTotales + "'>");
                str.AppendFormat("<a style='padding: 2px' href='JavaScript:verHistoricoURS({0},\"{1}\")'><img src='" + url + "Content/Images/btn-open.png' alt='Ver URS' /></a>", urs.Urscodi, urs.Ursnomb.Trim());
                if (tienePermisoEditar)
                    str.AppendFormat("<a style='padding: 2px' href='JavaScript:editarURS({0},\"{1}\")'><img src='" + url + "Content/Images/btn-edit.png' alt='Editar uRS' /></a>", urs.Urscodi, urs.Ursnomb.Trim());
                if (!string.IsNullOrEmpty(acta))
                    str.AppendFormat("<a style='padding: 2px'href='JavaScript:descargarActa(\"{0}\",{1}, 1)'><img src='" + url + "Content/Images/pdf.png' alt='Descargar Acta' /></a>", acta, urs.Urscodi);
                str.AppendFormat("</td>");

                str.Append(string.Format("<td rowspan='" + numeroModosTotales + "'>{0}</td>", urs.Ursnomb.Trim()));

                int contador = 0;
                foreach (var item in ursEmpresas.GroupBy(x => x.Emprcodi))
                {
                    if (contador != 0)
                    {
                        str.Append("</tr>");
                        str.Append("<tr class='" + clase + "'>");
                    }

                    var listaModos = item.OrderBy(y => y.Gruponomb).ToList();
                    var codempresa = item.Key;

                    //obtiene la urs para una empresa de la agrupación
                    var ursEmpresa = listaTodasURS.Find(x => x.Emprcodi == codempresa);
                    var ursCentral = listaModos.First();

                    str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", ursEmpresa.Emprnomb));
                    str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", ursCentral.Central));

                    int contador2 = 0;
                    foreach (var modo in listaModos)
                    {
                        if (contador2 != 0)
                        {
                            str.Append("</tr>");
                            str.Append("<tr class='" + clase + "'>");
                        }

                        str.AppendFormat("<td>{0}</td>", (modo.Gruponomb == "" ? "" : modo.Gruponomb));

                        //Obtener valores de los modos de la URS
                        str.AppendFormat("<td style='text-align: right;background-color: yellowgreen;padding-right: 10px;'>{0}</td>", modo.PMin != null ? Math.Round(modo.PMin.Value, 2).ToString() : string.Empty);
                        str.AppendFormat("<td style='text-align: right;background-color: yellowgreen;padding-right: 10px;'>{0}</td>", modo.PMax != null ? Math.Round(modo.PMax.Value, 2).ToString() : string.Empty);
                        str.AppendFormat("<td>{0}</td>", modo.BandaCalificada);

                        if (contador2 == 0)
                        {
                            str.AppendFormat("<td style='text-align: right;padding-right: 10px;' rowspan='" + listaModos.Count + "'>{0}</td>", modo.BandaURS != null ? Math.Round(modo.BandaURS.Value, 2).ToString() : string.Empty);
                        }

                        str.AppendFormat("<td title='{0}' style='padding-top: 0px; text-align: right;padding-right: 10px;'>", modo.MensajeBandaCalificada);
                        str.AppendFormat("{0}", modo.BandaDisponible != null ? Math.Round(modo.BandaDisponible.Value, 2).ToString() : string.Empty);
                        if (modo.BandaDisponible != null)
                            str.AppendFormat("<a style='padding: 2px' ><img src='" + url + "Content/Images/ico-info.gif' alt='Fecha de consulta Banda Disponible' title='{0}' style='width: 12px; height: 12px;' /></a>", modo.MensajeBandaCalificada);
                        str.AppendFormat("</td>");

                        if (contador2 == 0)
                        {
                            str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", modo.FechaInico));
                            str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", modo.FechaFin));
                            str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", modo.Estado));
                        }

                        str.AppendFormat("<td>{0}</td>", modo.Comentario);

                        if (contador2 == 0)
                        {
                            str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", modo.UsuarioModif));
                            str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", modo.FechaModif));
                        }

                        contador2++;
                    }

                    contador++;
                }

                str.Append("</tr>");
            }
            #endregion
            str.Append("</tbody>");

            str.Append("</table>");
            str.Append("</div>");

            return str.ToString();
        }

        /// <summary>
        /// Dinuja la sección de edición para la URS
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="urscodi"></param>
        /// <param name="opcionActual"></param>
        /// <returns></returns>
        public string ListadoEdicionURS(DateTime fecha, int urscodi, int opcionActual)
        {
            StringBuilder str = new StringBuilder();
            List<SmaUrsModoOperacionDTO> listaTodasURS = new List<SmaUrsModoOperacionDTO>();

            listaTodasURS = ReporteListadoURS(fecha, ConstantesAppServicio.ParametroDefecto);
            var listaURSDistinc = ListSmaUrsModoOperacions_Urs(-1);

            //captura de la data
            var urs = listaURSDistinc.Find(x => x.Urscodi == urscodi);

            str.Append("<div id='resultado2' style='height: auto;'>");
            //str.Append("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: 2200px;' >");
            str.Append("<table id='tabla_agrupacion2' border='0' class='pretty tabla-icono' cellspacing='0'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 240px'>Empresa</th>");
            str.Append("<th style='width: 240px'>Central</th>");
            str.Append("<th style='width: 240px'>UNIDAD /<br /> MODO DE OPERACIÓN</th>");
            str.Append("<th style='width: 120px'>Pminrsf</th>");
            str.Append("<th style='width: 120px'>Pmxrsf</th>");
            str.Append("<th style='width: 120px'>Banda</th>");
            str.Append("<th style='width: 120px'>Banda <br/>Disponible</th>");
            str.Append("<th style='width: 250px'>Comentario</th>");
            str.Append("</tr>");
            #endregion

            str.Append("</thead>");
            str.Append("<tbody>");
            #region cuerpo

            // empresas
            var ursEmpresas = listaTodasURS.Where(x => x.Urscodi == urs.Urscodi).OrderBy(y => y.Emprnomb).ToList();

            int contador = 0;
            foreach (var item in ursEmpresas.GroupBy(x => x.Emprcodi))
            {
                if (contador != 0)
                {
                    str.Append("</tr>");
                    str.Append("<tr>");
                }

                var listaModos = item.OrderBy(y => y.Gruponomb).ToList();
                var codempresa = item.Key;

                //obtiene la urs para una empresa de la agrupación
                var ursEmpresa = listaTodasURS.Find(x => x.Emprcodi == codempresa);
                var ursCentral = listaModos.First();

                str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", ursEmpresa.Emprnomb));
                str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", ursCentral.Central));

                int contador2 = 0;
                foreach (var modo in listaModos)
                {
                    if (contador2 != 0)
                    {
                        str.Append("</tr>");
                        str.Append("<tr>");
                    }

                    str.AppendFormat("<td>{0}</td>", (modo.Gruponomb == "" ? "" : modo.Gruponomb));

                    if (opcionActual == 3)
                    {
                        decimal? valorNull = null;
                        modo.PMin = valorNull;
                        modo.PMax = valorNull;
                        modo.BandaCalificada = valorNull;
                        modo.Comentario = "";
                    }
                    str.AppendFormat("<td style='background-color: yellowgreen;'>");
                    str.AppendFormat("<input id='grupocodiModo" + contador2 + "' type='hidden' name='grupocodiModo' value='{0}' >", modo.Grupocodi);
                    str.AppendFormat("<input id='catecodiModo" + contador2 + "' type='hidden' name='catecodiModo' value='{0}' >", modo.Catecodi);
                    str.AppendFormat("<input id='valorPmin" + contador2 + "' type='text' name='valorPmin' value='{0}' style='width: 40px; text-indent: 3px;'>", modo.PMin != null ? Math.Round(modo.PMin.Value, 2).ToString() : string.Empty);
                    str.AppendFormat("</td>");
                    str.AppendFormat("<td style='background-color: yellowgreen;'><input id='valorPmax" + contador2 + "' type='text' name='valorPmax' value='{0}' style='width: 40px; text-indent: 3px;'></td>", modo.PMax != null ? Math.Round(modo.PMax.Value, 2).ToString() : string.Empty);
                    str.AppendFormat("<td><input id='valorBanda" + contador2 + "' type='text' name='valorBanda' value='{0}' style='width: 40px; text-indent: 3px;'></td>", modo.BandaCalificada != null ? Math.Round(modo.BandaCalificada.Value, 2).ToString() : string.Empty);
                    str.AppendFormat("<td>");
                    str.AppendFormat("{0}", modo.BandaDisponible != null ? Math.Round(modo.BandaDisponible.Value, 2).ToString() : string.Empty);
                    str.AppendFormat("</td>");
                    str.AppendFormat("<td><input id='valorComent" + contador2 + "' type='text' name='valorComent' value='{0}' style='WIDTH: 250px;'></td>", modo.Comentario);

                    contador2++;
                }

                contador++;
            }

            str.Append("</tr>");
            #endregion
            str.Append("</tbody>");

            str.Append("</table>");
            str.Append("</div>");

            return str.ToString();
        }

        /// <summary>
        /// Permite completar los formuladat para la URS
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="concepcodi"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> CompletarDataHistoricoURS(int grupocodi)
        {
            var listaURSFecIni = servMigra.ListarGrupodatHistoricoValores(ConstantesSubasta.ConceptoURSFecInicio, grupocodi);
            var listaURSFecFin = servMigra.ListarGrupodatHistoricoValores(ConstantesSubasta.ConceptoURSFecfIn, grupocodi);
            var listaURSActa = servMigra.ListarGrupodatHistoricoValores(ConstantesSubasta.ConceptoURSActa, grupocodi);
            var listaURSBanda = servMigra.ListarGrupodatHistoricoValores(ConstantesSubasta.ConceptoURSBanda, grupocodi);

            int contador = 0;
            foreach (var reg in listaURSFecIni)
            {
                reg.FechaInicio = DateTime.ParseExact(reg.Formuladat, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                reg.FechaIniciodesc = reg.FechaInicio.Date.ToString(ConstantesAppServicio.FormatoFecha);

                var fecFin = listaURSFecFin[contador];
                reg.FechaFin = DateTime.ParseExact(fecFin.Formuladat, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                reg.FechaFindesc = reg.FechaFin.Date.ToString(ConstantesAppServicio.FormatoFecha);

                var acta = listaURSActa[contador];
                reg.Acta = acta.Formuladat;

                var bandaURS = listaURSBanda[contador];
                reg.BandaURS = bandaURS.Formuladat;

                DateTime fechaDatDesc = DateTime.ParseExact(reg.FechadatDesc, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                reg.FechadatDesc = fechaDatDesc.Date.ToString(ConstantesAppServicio.FormatoFecha);

                contador++;
            }

            return listaURSFecIni;
        }

        /// <summary>
        /// Resgitrar o actualizar URS Y MODOS
        /// </summary>
        /// <param name="regURS"></param>
        /// <param name="listaDetalleModos"></param>
        /// <param name="opcion"></param>
        public string RegistrarGrupoDat(PrGrupodatDTO regURS, List<SmaUrsModoOperacionDTO> listaDetalleModos, int tipoAccion)
        {
            string resultDes = "";
            try
            {
                if (tipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    PrGrupodatDTO nuevo = new PrGrupodatDTO();
                    nuevo.Grupocodi = regURS.Grupocodi;
                    nuevo.Fechadat = regURS.Fechadat;
                    nuevo.Lastuser = regURS.Lastuser;
                    nuevo.Fechaact = regURS.Fechaact;
                    nuevo.Deleted = regURS.Deleted;

                    string[] valores = ConstantesSubasta.ConceptoURSFull.Split(',');
                    List<int> conceptosURS = valores.Select(x => int.Parse(x)).ToList();
                    foreach (var concep in conceptosURS)
                    {
                        switch (concep)
                        {
                            case ConstantesSubasta.ConceptoURSFecInicio:
                                nuevo.Formuladat = regURS.FechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
                                nuevo.Concepcodi = ConstantesSubasta.ConceptoURSFecInicio;
                                break;
                            case ConstantesSubasta.ConceptoURSFecfIn:
                                nuevo.Formuladat = regURS.FechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                                nuevo.Concepcodi = ConstantesSubasta.ConceptoURSFecfIn;
                                break;
                            case ConstantesSubasta.ConceptoURSActa:
                                nuevo.Formuladat = regURS.Acta;
                                nuevo.Concepcodi = ConstantesSubasta.ConceptoURSActa;
                                break;
                            case ConstantesSubasta.ConceptoURSBanda:
                                nuevo.Formuladat = regURS.BandaURS;
                                nuevo.Concepcodi = ConstantesSubasta.ConceptoURSBanda;
                                break;
                        }
                        this.wsUrsCliente.SavePrGrupodat(nuevo);
                    }

                    //modos de la URS
                    foreach (var modo in listaDetalleModos)
                    {
                        PrGrupodatDTO modoOP = new PrGrupodatDTO();
                        modoOP.Grupocodi = modo.Grupocodi.Value;
                        modoOP.Fechadat = regURS.Fechadat;
                        modoOP.Lastuser = regURS.Lastuser;
                        modoOP.Fechaact = regURS.Fechaact;
                        modoOP.Deleted = regURS.Deleted;
                        modoOP.Catecodi = modo.Catecodi;

                        if (modoOP.Catecodi == 5)
                        {
                            resultDes = this.SaveUpdateModos(modoOP, ConstantesSubasta.AccionNuevo, ConstantesSubasta.ConceptoGrupHidraPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoGrupHidraPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoGrupHidraBandaCalificada, modo.BandaCalifDesc, ConstantesSubasta.ConceptoGrupHidraComentario, modo.Comentario);
                        }

                        if (modoOP.Catecodi == 9)
                        {
                            resultDes = this.SaveUpdateModos(modoOP, ConstantesSubasta.AccionNuevo, ConstantesSubasta.ConceptoMOHidroPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoMOHidroPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoMOHidroBandaCalificada, modo.BandaCalifDesc, ConstantesSubasta.ConceptoMOHidroComentario, modo.Comentario);
                        }

                        if (modoOP.Catecodi == 2)
                        {
                            resultDes = this.SaveUpdateModos(modoOP, ConstantesSubasta.AccionNuevo, ConstantesSubasta.ConceptoMOTermoPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoMOTermoPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoMOTermoBandaCalificada, modo.BandaCalifDesc, ConstantesSubasta.ConceptoMOTermoComentario, modo.Comentario);
                        }
                    }
                }
                if (tipoAccion == ConstantesSubasta.AccionEditar)
                {
                    PrGrupodatDTO edicion = new PrGrupodatDTO();

                    string[] valores = ConstantesSubasta.ConceptoURSFull.Split(',');
                    List<int> conceptosURS = valores.Select(x => int.Parse(x)).ToList();
                    foreach (var concep in conceptosURS)
                    {
                        switch (concep)
                        {
                            case ConstantesSubasta.ConceptoURSFecInicio:
                                edicion = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSFecInicio, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                                edicion.Formuladat = regURS.FechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
                                edicion.Lastuser = regURS.Lastuser;
                                edicion.Fechaact = DateTime.Now;
                                break;
                            case ConstantesSubasta.ConceptoURSFecfIn:
                                edicion = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSFecfIn, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                                edicion.Formuladat = regURS.FechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                                edicion.Lastuser = regURS.Lastuser;
                                edicion.Fechaact = DateTime.Now;
                                break;
                            case ConstantesSubasta.ConceptoURSActa:
                                edicion = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSActa, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                                edicion.Formuladat = regURS.Acta;
                                edicion.Lastuser = regURS.Lastuser;
                                edicion.Fechaact = DateTime.Now;
                                break;
                            case ConstantesSubasta.ConceptoURSBanda:
                                edicion = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSBanda, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                                edicion.Formuladat = regURS.BandaURS;
                                edicion.Lastuser = regURS.Lastuser;
                                edicion.Fechaact = DateTime.Now;
                                break;
                        }
                        if (edicion == null)
                        {
                            throw new Exception("El registro no existe, no puede modificarse.");
                        }
                        wsUrsCliente.UpdatePrGrupodat(edicion);
                    }

                    //modos de la URS
                    foreach (var modo in listaDetalleModos)
                    {
                        PrGrupodatDTO modoOPEdicion = new PrGrupodatDTO();
                        modoOPEdicion.Fechadat = regURS.Fechadat;
                        modoOPEdicion.Grupocodi = modo.Grupocodi.Value;
                        modoOPEdicion.Lastuser = regURS.Lastuser;

                        if (modo.Catecodi == 5)
                        {

                            resultDes = this.SaveUpdateModos(modoOPEdicion, ConstantesSubasta.AccionEditar, ConstantesSubasta.ConceptoGrupHidraPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoGrupHidraPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoGrupHidraBandaCalificada, modo.BandaCalifDesc, ConstantesSubasta.ConceptoGrupHidraComentario, modo.Comentario);
                        }

                        if (modo.Catecodi == 9)
                        {
                            resultDes = this.SaveUpdateModos(modoOPEdicion, ConstantesSubasta.AccionEditar, ConstantesSubasta.ConceptoMOHidroPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoMOHidroPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoMOHidroBandaCalificada, modo.BandaCalifDesc, ConstantesSubasta.ConceptoMOHidroComentario, modo.Comentario);
                        }

                        if (modo.Catecodi == 2)
                        {

                            resultDes = this.SaveUpdateModos(modoOPEdicion, ConstantesSubasta.AccionEditar, ConstantesSubasta.ConceptoMOTermoPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoMOTermoPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoMOTermoBandaCalificada, modo.BandaCalifDesc, ConstantesSubasta.ConceptoMOTermoComentario, modo.Comentario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                resultDes = ex.Message;
            }
            return resultDes;
        }

        /// <summary>
        /// mantenimiento de pr_GrupoDat para los modos de la URS
        /// </summary>
        /// <param name="modoURS"></param>
        /// <param name="tipoAccion"></param>
        /// <param name="ConcepPMin"></param>
        /// <param name="formulaDatPmin"></param>
        /// <param name="ConcepPMax"></param>
        /// <param name="formulaDatPmax"></param>
        /// <param name="ConcepBanda"></param>
        /// <param name="formulaDatBanda"></param>
        /// <param name="ConcepComentario"></param>
        /// <param name="formulaDatComent"></param>
        /// <returns></returns>
        public string SaveUpdateModos(PrGrupodatDTO modoURS, int tipoAccion, int ConcepPMin, string formulaDatPmin, int ConcepPMax, string formulaDatPmax, int ConcepBanda, string formulaDatBanda, int ConcepComentario, string formulaDatComent)
        {
            string resultDes = "";
            try
            {
                if (tipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    // guardar potencia minima
                    var potenciaMin = modoURS;
                    potenciaMin.Concepcodi = ConcepPMin;
                    potenciaMin.Formuladat = formulaDatPmin;
                    this.wsUrsCliente.SavePrGrupodat(potenciaMin);

                    // guardar potencia máxima
                    var potenciaMax = modoURS;
                    potenciaMax.Concepcodi = ConcepPMax;
                    potenciaMax.Formuladat = formulaDatPmax;
                    this.wsUrsCliente.SavePrGrupodat(potenciaMax);

                    // guardar Banda
                    var banda = modoURS;
                    banda.Concepcodi = ConcepBanda;
                    banda.Formuladat = formulaDatBanda;
                    this.wsUrsCliente.SavePrGrupodat(banda);

                    // guardar comentario
                    var comentario = modoURS;
                    comentario.Concepcodi = ConcepComentario;
                    comentario.Formuladat = formulaDatComent;
                    this.wsUrsCliente.SavePrGrupodat(comentario);
                }
                if (tipoAccion == ConstantesSubasta.AccionEditar)
                {
                    // actualizar potencia minima
                    var potenciaMinEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPMin, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    potenciaMinEdicion.Formuladat = formulaDatPmin;
                    potenciaMinEdicion.Lastuser = modoURS.Lastuser;
                    potenciaMinEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(potenciaMinEdicion);

                    // actualizar potencia máxima
                    var potenciaMaxEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPMax, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    potenciaMaxEdicion.Formuladat = formulaDatPmax;
                    potenciaMaxEdicion.Lastuser = modoURS.Lastuser;
                    potenciaMaxEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(potenciaMaxEdicion);

                    // actualizar Banda
                    var bandaEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepBanda, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    bandaEdicion.Formuladat = formulaDatBanda;
                    bandaEdicion.Lastuser = modoURS.Lastuser;
                    bandaEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(bandaEdicion);

                    // actualizar comentario
                    var comentarioEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepComentario, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    comentarioEdicion.Formuladat = formulaDatComent;
                    comentarioEdicion.Lastuser = modoURS.Lastuser;
                    comentarioEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(comentarioEdicion);
                }
                if (tipoAccion == ConstantesSubasta.AccionEliminar)
                {
                    // actualizar potencia minima
                    var potenciaMinDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPMin, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    potenciaMinDeleted.Lastuser = modoURS.Lastuser;
                    potenciaMinDeleted.Fechaact = DateTime.Now;
                    potenciaMinDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(potenciaMinDeleted);

                    // actualizar potencia máxima
                    var potenciaMaxDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPMax, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    potenciaMaxDeleted.Lastuser = modoURS.Lastuser;
                    potenciaMaxDeleted.Fechaact = DateTime.Now;
                    potenciaMaxDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(potenciaMaxDeleted);

                    // actualizar Banda
                    var bandaDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepBanda, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    bandaDeleted.Lastuser = modoURS.Lastuser;
                    bandaDeleted.Fechaact = DateTime.Now;
                    bandaDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(bandaDeleted);

                    // actualizar comentario
                    var comentarioDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepComentario, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    comentarioDeleted.Lastuser = modoURS.Lastuser;
                    comentarioDeleted.Fechaact = DateTime.Now;
                    comentarioDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(comentarioDeleted);
                }
            }
            catch (Exception ex)
            {
                resultDes = ex.Message;
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }

            return resultDes;
        }

        /// <summary>
        /// Eliminado lógico de la urs y sus smodos de grupodat
        /// </summary>
        /// <param name="regURS"></param>
        /// <param name="listaDetalleModos"></param>
        /// <returns></returns>
        public string EliminarURSGrupoDat(PrGrupodatDTO regURS, List<SmaUrsModoOperacionDTO> listaDetalleModos)
        {
            string resultDes = "";
            try
            {
                PrGrupodatDTO deleted = new PrGrupodatDTO();
                var correlativoDeleted = ValidarEliminacion(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSFecInicio, regURS.Grupocodi);

                string[] valores = ConstantesSubasta.ConceptoURSFull.Split(',');
                List<int> conceptosURS = valores.Select(x => int.Parse(x)).ToList();
                foreach (var concep in conceptosURS)
                {
                    switch (concep)
                    {
                        case ConstantesSubasta.ConceptoURSFecInicio:
                            deleted = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSFecInicio, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                            deleted.Lastuser = regURS.Lastuser;
                            deleted.Fechaact = DateTime.Now;
                            deleted.Deleted2 = correlativoDeleted;
                            break;
                        case ConstantesSubasta.ConceptoURSFecfIn:
                            deleted = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSFecfIn, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                            deleted.Lastuser = regURS.Lastuser;
                            deleted.Fechaact = DateTime.Now;
                            deleted.Deleted2 = correlativoDeleted;
                            break;
                        case ConstantesSubasta.ConceptoURSActa:
                            deleted = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSActa, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                            deleted.Lastuser = regURS.Lastuser;
                            deleted.Fechaact = DateTime.Now;
                            deleted.Deleted2 = correlativoDeleted;
                            break;
                        case ConstantesSubasta.ConceptoURSBanda:
                            deleted = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSBanda, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                            deleted.Lastuser = regURS.Lastuser;
                            deleted.Fechaact = DateTime.Now;
                            deleted.Deleted2 = correlativoDeleted;
                            break;
                    }
                    if (deleted == null)
                    {
                        throw new Exception("El registro no existe o ha sido eliminada.");
                    }
                    wsUrsCliente.UpdatePrGrupodat(deleted);
                }

                //modos de la URS
                foreach (var modo in listaDetalleModos)
                {
                    PrGrupodatDTO modoOPDeleted = new PrGrupodatDTO();
                    modoOPDeleted.Fechadat = regURS.Fechadat;
                    modoOPDeleted.Grupocodi = modo.Grupocodi.Value;
                    modoOPDeleted.Lastuser = regURS.Lastuser;
                    modoOPDeleted.Deleted2 = correlativoDeleted;

                    if (modo.Catecodi == 5)
                    {
                        resultDes = this.SaveUpdateModos(modoOPDeleted, ConstantesSubasta.AccionEliminar, ConstantesSubasta.ConceptoGrupHidraPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoGrupHidraPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoGrupHidraBandaCalificada, modo.BandaCalifDesc, ConstantesSubasta.ConceptoGrupHidraComentario, modo.Comentario);
                    }

                    if (modo.Catecodi == 9)
                    {

                        resultDes = this.SaveUpdateModos(modoOPDeleted, ConstantesSubasta.AccionEliminar, ConstantesSubasta.ConceptoMOHidroPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoMOHidroPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoMOHidroBandaCalificada, modo.BandaCalifDesc, ConstantesSubasta.ConceptoMOHidroComentario, modo.Comentario);
                    }

                    if (modo.Catecodi == 2)
                    {
                        resultDes = this.SaveUpdateModos(modoOPDeleted, ConstantesSubasta.AccionEliminar, ConstantesSubasta.ConceptoMOTermoPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoMOTermoPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoMOTermoBandaCalificada, modo.BandaCalifDesc, ConstantesSubasta.ConceptoMOTermoComentario, modo.Comentario);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                resultDes = ex.Message;
            }
            return resultDes;
        }

        /// <summary>
        /// verifico traslape para dos periodos
        /// </summary>
        /// <param name="periodo1"></param>
        /// <param name="periodo2"></param>
        /// <returns></returns>
        public string ValidarInterseccion(int codigoURS, string fechaIni, string fechaFin, int tipoURS)
        {
            string msgesultado = "";

            DateTime fechaIniURS = DateTime.ParseExact(fechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinURS = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            //traer listado historico de las URS
            var listHistoricoURS = tipoURS == ConstantesSubasta.AccionURSCalificada ? CompletarDataHistoricoURS(codigoURS) : CompletarDataHistoricoURSBase(codigoURS);

            foreach (var item in listHistoricoURS.Where(x => x.Deleted == 0).ToList())
            {
                bool hayInterseccion = false;

                var valor = fechaIniURS.CompareTo(item.FechaInicio);

                //Urs actual inicia primero
                if (valor < 0)
                {
                    var valor2 = fechaFinURS.CompareTo(item.FechaInicio);

                    //Urs actual termina antes o justo cuando incia el rango de su histórico
                    hayInterseccion = valor2 <= 0 ? false : true;
                }
                else
                {
                    //Urs actual inicia igual a rango de su histórico
                    if (valor == 0)
                    {
                        hayInterseccion = true;
                    }
                    //rango de su histórico inicia primero
                    else
                    {
                        var valor3 = item.FechaFin.CompareTo(fechaIniURS);

                        //rango de su histórico termina antes que inice Urs actual 
                        hayInterseccion = valor3 <= 0 ? false : true;
                    }
                }
                if (hayInterseccion)
                {
                    msgesultado = " Existe cruce de fechas con los registros de la URS";
                    break;
                }
            }

            return msgesultado;
        }

        /// <summary>
        /// Validar que todos los rangos de los modos esten dentro de la urs
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaDetalleModos"></param>
        /// <returns></returns>
        public string ValidarModosIncluidosEnUrs(string fechaIni, string fechaFin, List<SmaUrsModoOperacionDTO> listaDetalleModos)
        {
            DateTime fechaIniURS = DateTime.ParseExact(fechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinURS = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            bool existeNoInclusion = false;
            foreach (var nuevo in listaDetalleModos)
            {
                DateTime fechaIniModNew = DateTime.ParseExact(nuevo.ModFechIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinModNew = DateTime.ParseExact(nuevo.ModFechFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                if (fechaIniURS <= fechaIniModNew && fechaIniModNew <= fechaFinURS) { }
                else
                {
                    existeNoInclusion = true;
                }
                if (fechaIniURS <= fechaFinModNew && fechaFinModNew <= fechaFinURS) { }
                else
                {
                    existeNoInclusion = true;
                }
            }

            return existeNoInclusion ? "Existe modo(s) cuyo periodo no está incluido dentro de la Fecha inicio y fin del Proceso URS." : string.Empty;
        }

        /// <summary>
        /// LISTADO DE NOMBRE DE LA CABECERA Y SU ANCHO
        /// </summary>
        /// <param name="listaOferta"></param>
        /// <param name="listaOfertaOld"></param>
        /// <param name="listaCabecera"></param>
        private void ListarCabeceraURSCalificadas(out List<CabeceraRow> listaCabecera)
        {
            listaCabecera = new List<CabeceraRow>();
            listaCabecera.Add(new CabeceraRow() { TituloRow = "URS", Ancho = 25 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "EMPRESA", Ancho = 25 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "CENTRAL", Ancho = 25 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "UNIDAD <br/> MODOS OPERACIÓN", Ancho = 40 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "PMINRSF <br/> (MW)", Ancho = 10 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "PMXRSF <br/> (MW)", Ancho = 10 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "BANDA <br/> (MW)", Ancho = 10 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "BANDA URS <br/> CALIFICADA <br/> (MW)", Ancho = 15 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "BANDA DISPONIBLE <br/> (MW)", Ancho = 15 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "FECHA INICIO", Ancho = 15 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "FECHA FIN", Ancho = 15 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "ESTADO", Ancho = 15 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "COMENTARIO", IsMerge = 1, Ancho = 40 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "USUARIO MODIFICACIÓN", Ancho = 15 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "FECHA MODIFICACIÓN", Ancho = 20 });
        }

        /// <summary>
        /// Genera el archivo excel a exportar
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="fecha"></param>
        public void GenerarArchivoExcelURSCalificadas(string ruta, string nombreArchivo, DateTime fecha)
        {
            List<SmaUrsModoOperacionDTO> listaTodasURS = ReporteListadoURS(fecha, ConstantesAppServicio.ParametroDefecto);
            List<SmaUrsModoOperacionDTO> listaURSDistinc = ListSmaUrsModoOperacions_Urs(-1); //cabecera

            this.ListarCabeceraURSCalificadas(out List<CabeceraRow> listaCabecera);

            FileInfo newFile = new FileInfo(ruta + nombreArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + nombreArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                //System.Drawing.Image img = null;

                string nombre = ConstantesSubasta.NombreTabURSCalificadas;
                string titulo = ConstantesSubasta.TituloReporteURSCalificada;
                this.GenerarArchivoExcelURSCalifHoja(ref ws, xlPackage, img, titulo, nombre, 1, 2, listaTodasURS, listaURSDistinc, listaCabecera);

                if (ws == null)
                {
                    throw new Exception("No se generó el archivo Excel");
                }
            }
        }

        /// <summary>
        /// Dibuja hoja Excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="img"></param>
        /// <param name="titulo"></param>
        /// <param name="nombre"></param>
        /// <param name="rowIniHeader"></param>
        /// <param name="colIniHeader"></param>
        /// <param name="listaTodasURS"></param>
        /// <param name="listaURSDistinc"></param>
        /// <param name="listaCabecera"></param>
        private void GenerarArchivoExcelURSCalifHoja(ref ExcelWorksheet ws, ExcelPackage xlPackage, System.Drawing.Image img
            , string titulo, string nombre, int rowIniHeader, int colIniHeader, List<SmaUrsModoOperacionDTO> listaTodasURS, List<SmaUrsModoOperacionDTO> listaURSDistinc, List<CabeceraRow> listaCabecera)
        {
            try
            {
                ws = xlPackage.Workbook.Worksheets.Add(nombre);

                int filTitulo = rowIniHeader + 1;
                int colTitulo = colIniHeader + 4;
                ws.Cells[filTitulo, colTitulo].Value = titulo;
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filTitulo, colTitulo, filTitulo, colTitulo, "Calibri", 14);
                UtilExcel.CeldasExcelEnNegrita(ws, filTitulo, colTitulo, filTitulo, colTitulo);

                int row = rowIniHeader + 5;
                int col = colIniHeader;

                #region Cabecera

                int filaIniCab = row;
                int coluIniCab = col;
                int numeroColums = 15;
                int coluFinCab = coluIniCab + numeroColums - 1;
                int posCol = 0;
                foreach (var cab in listaCabecera)
                {
                    ws.Cells[filaIniCab, coluIniCab + posCol].Value = cab.TituloRow.ToUpper().Replace("<BR/>", "\n");
                    ws.Column(coluIniCab + posCol).Width = cab.Ancho;
                    posCol++;
                }

                UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "Centro");
                UtilExcel.CeldasExcelColorTexto(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "#2B579A");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "Calibri", 11);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab);
                UtilExcel.CeldasExcelWrapText(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab);
                ws.Row(filaIniCab).Height = 50;

                #endregion

                #region cuerpo

                int filaIniData = row + 1;
                int coluIniData = col;
                int filActual = filaIniData;
                for (var fila = 0; fila < listaURSDistinc.Count(); fila++)
                {
                    var urs = listaURSDistinc[fila];
                    if (urs != null)
                    {
                        int numeroModosTotales = 0;
                        // empresas
                        var ursEmpresas = listaTodasURS.Where(x => x.Urscodi == urs.Urscodi).OrderBy(y => y.Emprnomb).ToList();
                        foreach (var val in ursEmpresas.GroupBy(x => x.Emprcodi))
                        {
                            numeroModosTotales = numeroModosTotales + val.ToList().Count;
                        }

                        int colActual = coluIniData;
                        ws.Cells[filActual, colActual].Value = urs.Ursnomb.Trim();
                        UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual, filActual + numeroModosTotales - 1, colActual);
                        UtilExcel.BorderCeldas3(ws, filActual, colActual, filActual + numeroModosTotales - 1, colActual);

                        int contador = 0;
                        foreach (var item in ursEmpresas.GroupBy(x => x.Emprcodi))
                        {
                            var listaModos = item.OrderBy(y => y.Gruponomb).ToList();
                            var codempresa = item.Key;

                            //obtiene la urs para una empresa de la agrupación
                            var ursEmpresa = listaTodasURS.Find(x => x.Emprcodi == codempresa);
                            var ursCentral = listaModos.First();

                            ws.Cells[filActual, colActual + 1].Value = ursEmpresa.Emprnomb.Trim();
                            UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual + 1, filActual + listaModos.Count - 1, colActual + 1);
                            UtilExcel.BorderCeldas3(ws, filActual, colActual + 1, filActual + listaModos.Count - 1, colActual + 1);
                            ws.Cells[filActual, colActual + 2].Value = ursCentral.Central.Trim();
                            UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual + 2, filActual + listaModos.Count - 1, colActual + 2);
                            UtilExcel.BorderCeldas3(ws, filActual, colActual + 2, filActual + listaModos.Count - 1, colActual + 2);

                            int contador2 = 0;
                            foreach (var modo in listaModos)
                            {
                                ws.Cells[filActual, colActual + 3].Value = modo.Gruponomb.Trim();
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 3, filActual, colActual + 3);
                                ws.Cells[filActual, colActual + 4].Value = modo.PMin;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 4, filActual, colActual + 4);
                                ws.Cells[filActual, colActual + 5].Value = modo.PMax;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 5, filActual, colActual + 5);
                                ws.Cells[filActual, colActual + 6].Value = modo.BandaCalificada;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 6, filActual, colActual + 6);

                                if (contador2 == 0)
                                {
                                    ws.Cells[filActual, colActual + 7].Value = modo.BandaURS;
                                    UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual + 7, filActual + listaModos.Count - 1, colActual + 7);
                                    UtilExcel.BorderCeldas3(ws, filActual, colActual + 7, filActual + listaModos.Count - 1, colActual + 7);
                                }

                                if (modo.BandaDisponible != null)
                                    ws.Cells[filActual, colActual + 8].Value = Math.Round(modo.BandaDisponible.Value, 2);
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 8, filActual, colActual + 8);

                                if (contador2 == 0)
                                {
                                    ws.Cells[filActual, colActual + 9].Value = modo.FechaInico;
                                    UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual + 9, filActual + listaModos.Count - 1, colActual + 9);
                                    UtilExcel.BorderCeldas3(ws, filActual, colActual + 9, filActual + listaModos.Count - 1, colActual + 9);

                                    ws.Cells[filActual, colActual + 10].Value = modo.FechaFin;
                                    UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual + 10, filActual + listaModos.Count - 1, colActual + 10);
                                    UtilExcel.BorderCeldas3(ws, filActual, colActual + 10, filActual + listaModos.Count - 1, colActual + 10);

                                    ws.Cells[filActual, colActual + 11].Value = modo.Estado;
                                    UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual + 11, filActual + listaModos.Count - 1, colActual + 11);
                                    UtilExcel.BorderCeldas3(ws, filActual, colActual + 11, filActual + listaModos.Count - 1, colActual + 11);
                                }

                                ws.Cells[filActual, colActual + 12].Value = modo.Comentario;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 12, filActual, colActual + 12);

                                if (contador2 == 0)
                                {
                                    ws.Cells[filActual, colActual + 13].Value = modo.UsuarioModif;
                                    UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual + 13, filActual + listaModos.Count - 1, colActual + 13);
                                    UtilExcel.BorderCeldas3(ws, filActual, colActual + 13, filActual + listaModos.Count - 1, colActual + 13);

                                    ws.Cells[filActual, colActual + 14].Value = modo.FechaModif;
                                    UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual + 14, filActual + listaModos.Count - 1, colActual + 14);
                                    UtilExcel.BorderCeldas3(ws, filActual, colActual + 14, filActual + listaModos.Count - 1, colActual + 14);
                                }

                                if (modo.Estado.ToUpper() == "NO VIGENTE")
                                    UtilExcel.CeldasExcelColorFondo(ws, filActual, colActual, filActual, colActual + 14, "#D9D9D9");

                                contador2++;
                                filActual = filActual + 1;
                            }
                            contador++;
                        }
                    }
                }
                // DAR FORMATO DE ALINEAMIENTO GENERAL
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData, filActual, numeroColums + 1, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData, filActual, numeroColums + 1, "Centro");
                UtilExcel.CeldasExcelWrapText(ws, filaIniData, coluIniData, filActual, numeroColums + 1);
                #endregion

                ws.Column(1).Width = 2;
                UtilExcel.AddImage(ws, img, rowIniHeader, colIniHeader);
                ws.View.FreezePanes(7, 6);
                //No mostrar lineas
                ws.View.ShowGridLines = false;
                ws.View.ZoomScale = 80;
                xlPackage.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Obtener fecha de consulta y mensaje
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="mensaje"></param>
        /// <param name="fechaConsulta"></param>
        private void ObtenerFechaYMensajeBandaCalificada(DateTime fechaIni, DateTime fechaFin, out string mensaje, out DateTime fechaConsulta)
        {
            fechaConsulta = DateTime.Now.Date;

            if (DateTime.Now.Date < fechaIni)
            {
                fechaConsulta = fechaIni;
            }
            if (fechaFin < DateTime.Now.Date)
            {
                fechaConsulta = fechaFin;
            }

            mensaje = "Banda disponible para la fecha " + fechaConsulta.ToString(ConstantesAppServicio.FormatoFecha);
        }

        /// <summary>
        /// OBtener el correlativo de la última eliminación para evitar calves duplicadas en la BD 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="concepto"></param>
        /// <param name="grupo"></param>
        /// <returns></returns>
        public int ValidarEliminacion(DateTime fecha, int concepto, int grupo)
        {
            int resultado = 0;

            var eliminadoList = FactorySic.GetPrGrupodatRepository().BuscarEliminados(fecha, concepto, grupo);
            var eliminado = eliminadoList.OrderByDescending(x => x.Deleted).First(); //obtiene el ultimo item correlativo

            resultado = eliminado.Deleted + 1;
            return resultado;
        }

        #endregion

        #region URS Provisión Base

        /// <summary>
        /// permite juntar la data de la URS BASE con la de GrupoDat
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<SmaUrsModoOperacionDTO> ReporteListadoURSBase(DateTime fecha, string estado)
        {
            List<SmaUrsModoOperacionDTO> listaURSCompleto = new List<SmaUrsModoOperacionDTO>();
            List<SmaUrsModoOperacionDTO> listaTodasURS = new List<SmaUrsModoOperacionDTO>();

            listaTodasURS = GetByCriteriaSmaUrsModoOperacions();
            var listaURSDistinc = ListSmaUrsModoOperacions_Urs(-1);

            //captura de la data
            var lstGrupoCodiURS = listaTodasURS.Select(x => x.Urscodi).Distinct().ToList();
            var lstGrupoCodiModo = listaTodasURS.Select(x => x.Grupocodi.Value).Distinct().ToList();
            lstGrupoCodiURS.AddRange(lstGrupoCodiModo);

            List<PrGrupodatDTO> listaGrupoDatURS = this.ListarParametrosConfiguracionPorFecha(fecha, ConstantesSubasta.ConcepcodiURSyModosBase, string.Join(",", lstGrupoCodiURS));

            foreach (var urs in listaURSDistinc)
            {
                int cantModos = listaURSDistinc.Count();

                //obtener valores de la URS mediante PrGrupoDat
                var ursFecIni = listaGrupoDatURS.Find(x => x.Grupocodi == urs.Urscodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseFecInicio);
                var ursFecFin = listaGrupoDatURS.Find(x => x.Grupocodi == urs.Urscodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseFecfIn);
                var ursActa = listaGrupoDatURS.Find(x => x.Grupocodi == urs.Urscodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseActa);

                // empresas
                var ursEmpresas = listaTodasURS.Where(x => x.Urscodi == urs.Urscodi).OrderBy(y => y.Emprnomb).ToList();

                foreach (var item in ursEmpresas.GroupBy(x => x.Emprcodi))
                {
                    var listaModos = item.OrderBy(y => y.Gruponomb).ToList();
                    var codempresa = item.Key;
                    //obtiene la urs para una empresa de la agrupación
                    var ursEmpresa = listaTodasURS.Find(x => x.Emprcodi == codempresa);

                    foreach (var modo in listaModos)
                    {
                        var modoBase = SetDatosProvisionBasexModo(modo, listaGrupoDatURS);

                        //NIVEL de URS
                        modoBase.FechaInico = ursFecIni != null ? ursFecIni.Formuladat.Trim() : string.Empty; // fecha de la URS
                        modoBase.FechaFin = ursFecFin != null ? ursFecFin.Formuladat.Trim() : string.Empty; //Fecha de la URS
                        modoBase.Acta = ursActa != null ? ursActa.Formuladat : string.Empty;

                        //validar estad vigencia
                        if (!string.IsNullOrEmpty(modoBase.ModFechIni) && !string.IsNullOrEmpty(modoBase.ModFechFin))
                        {
                            var FecIniURSModo = DateTime.ParseExact(modoBase.ModFechIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                            var FecFinURSModo = DateTime.ParseExact(modoBase.ModFechFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                            modoBase.Estado = (fecha.Date >= FecIniURSModo && fecha.Date <= FecFinURSModo) ? "Vigente" : "No Vigente";
                        }
                        else
                        {
                            modoBase.Estado = string.Empty;
                        }

                        //modoBase.FechaModif = ursFecIni != null ? ursFecIni.FechaactDesc : string.Empty; //el mismo para urs y modos
                        //modoBase.UsuarioModif = ursFecIni != null ? ursFecIni.Lastuser : string.Empty; //el mismo para urs y modos

                        listaURSCompleto.Add(modoBase);
                    }
                }
            }

            if (ConstantesSubasta.EstadoURSVigente == estado)
            {
                //listar las urs que tengan modos de operación vigentes según la fecha de consulta
                listaURSCompleto = listaURSCompleto.Where(x => x.Estado.ToUpper() == "VIGENTE").ToList();
            }

            if (ConstantesSubasta.EstadoURSConProvBase == estado)
            {
                //listar a las urs que estan vigentes para la fecha de consulta y tambien a las urs que estan en los parámetros
                //var strUrscodi = ConfigurationManager.AppSettings[ConstantesSubasta.KeyUrscodiProvisionBase].ToString();
                //if (!string.IsNullOrEmpty(strUrscodi))
                //{
                //List<int> listaUrscodi = strUrscodi.Split(',').Select(x => int.Parse(x)).ToList();
                List<int> listaUrscodiVigente = listaURSCompleto.Where(x => x.Estado.ToUpper() == "VIGENTE").Select(x => x.Urscodi).Distinct().ToList();

                //listaUrscodi.AddRange(listaUrscodiVigente);

                listaURSCompleto = listaURSCompleto.Where(x => listaUrscodiVigente.Contains(x.Urscodi)).ToList();
                //}
            }

            return listaURSCompleto;
        }

        /// <summary>
        /// Generar lista para editar los modos de urs con provision base
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private List<SmaUrsModoOperacionDTO> ReporteEdicionURSBase(int grupocodi, DateTime fecha)
        {
            List<SmaUrsModoOperacionDTO> listaFinal = new List<SmaUrsModoOperacionDTO>();

            var listaModos = GetByCriteriaSmaUrsModoOperacions().Where(x => x.Urscodi == grupocodi).ToList();

            List<PrGrupodatDTO> listaGrupoDatURS = this.ListarParametrosConfiguracionPorFecha(fecha, ConstantesSubasta.ConceptoURSBaseFecInicio + "," + ConstantesSubasta.ConceptoURSBaseFecfIn, grupocodi.ToString());
            if (listaGrupoDatURS.Any())
            {
                var ursFecIni = listaGrupoDatURS.Find(x => x.Grupocodi == grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseFecInicio);
                var ursFecFin = listaGrupoDatURS.Find(x => x.Grupocodi == grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseFecfIn);
                var strfechaInico = ursFecIni != null ? ursFecIni.Formuladat.Trim() : string.Empty; // fecha de la URS
                var strfechaFin = ursFecFin != null ? ursFecFin.Formuladat.Trim() : string.Empty; //Fecha de la URS
                var fecIniURS = DateTime.ParseExact(strfechaInico, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                var fecFinURS = DateTime.ParseExact(strfechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                foreach (var modo in listaModos)
                {
                    var conceptoFecIni = 0;
                    conceptoFecIni = modo.Catecodi == 5 ? ConstantesSubasta.ConceptoURSBaseGrupHidraFecInicio : conceptoFecIni;
                    conceptoFecIni = modo.Catecodi == 9 ? ConstantesSubasta.ConceptoURSBaseMOHidroFecInicio : conceptoFecIni;
                    conceptoFecIni = modo.Catecodi == 2 ? ConstantesSubasta.ConceptoURSBaseMOTermoFecInicio : conceptoFecIni;
                    var listaModoFecIni = servMigra.ListarGrupodatHistoricoValores(conceptoFecIni, modo.Grupocodi.Value);
                    listaModoFecIni = listaModoFecIni.Where(x => x.Fechadat >= fecIniURS && fecFinURS >= x.Fechadat && x.Deleted == 0).ToList();

                    if (listaModoFecIni.Any())
                    {
                        foreach (var regModoFec in listaModoFecIni)
                        {
                            var regModo = modo.Copy();
                            List<PrGrupodatDTO> listaGrupoDatURSxModo = this.ListarParametrosConfiguracionPorFecha(regModoFec.Fechadat.Value, ConstantesSubasta.ConcepcodiURSyModosBase, string.Join(",", modo.Grupocodi));

                            regModo = SetDatosProvisionBasexModo(regModo, listaGrupoDatURSxModo);

                            listaFinal.Add(regModo);
                        }
                    }
                }
            }

            return listaFinal;
        }

        /// <summary>
        /// metodo para setear los valores por cada categoria de modo de operacion
        /// </summary>
        /// <param name="modo"></param>
        /// <param name="listaGrupoDatURS"></param>
        /// <returns></returns>
        private SmaUrsModoOperacionDTO SetDatosProvisionBasexModo(SmaUrsModoOperacionDTO modo, List<PrGrupodatDTO> listaGrupoDatURS)
        {

            //Obtener valores de los modos de la URS
            PrGrupodatDTO modPmin = new PrGrupodatDTO();
            PrGrupodatDTO modPmax = new PrGrupodatDTO();
            PrGrupodatDTO modBanda = new PrGrupodatDTO();
            PrGrupodatDTO modPrecMin = new PrGrupodatDTO();
            PrGrupodatDTO modPrecMax = new PrGrupodatDTO();
            PrGrupodatDTO modFecIni = new PrGrupodatDTO();
            PrGrupodatDTO modFecFin = new PrGrupodatDTO();
            PrGrupodatDTO modComentario = new PrGrupodatDTO();
            switch (modo.Catecodi)
            {
                case 5:
                    modPmin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMin);
                    modPmax = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMax);
                    modBanda = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseGrupHidraBanda);
                    modPrecMin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseGrupHidraPrecioMin);
                    modPrecMax = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseGrupHidraPrecioMax);
                    modFecIni = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseGrupHidraFecInicio);
                    modFecFin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseGrupHidraFecfIn);
                    modComentario = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseGrupHidraComentario);
                    break;
                case 9:
                    modPmin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMin);
                    modPmax = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMax);
                    modBanda = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOHidroBanda);
                    modPrecMin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOHidroPrecioMin);
                    modPrecMax = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOHidroPrecioMax);
                    modFecIni = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOHidroFecInicio);
                    modFecFin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOHidroFecfIn);
                    modComentario = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOHidroComentario);
                    break;
                case 2:
                    modPmin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMin);
                    modPmax = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMax);
                    modBanda = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOTermoBanda);
                    modPrecMin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOTermoPrecioMin);
                    modPrecMax = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOTermoPrecioMax);
                    modFecIni = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOTermoFecInicio);
                    modFecFin = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOTermoFecfIn);
                    modComentario = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi & x.Concepcodi == ConstantesSubasta.ConceptoURSBaseMOTermoComentario);
                    break;
            }

            //NIVEL DE MODO
            decimal? ValorNull = null;
            modo.PMin = modPmin != null ? Convert.ToDecimal(modPmin.Formuladat) : ValorNull;
            modo.PMax = modPmax != null ? Convert.ToDecimal(modPmax.Formuladat) : ValorNull;
            modo.BandaAdjudicada = modBanda != null ? Convert.ToDecimal(modBanda.Formuladat) : ValorNull;
            modo.PrecMin = modPrecMin != null ? Convert.ToDecimal(modPrecMin.Formuladat) : ValorNull;
            modo.PrecMax = modPrecMax != null ? Convert.ToDecimal(modPrecMax.Formuladat) : ValorNull;
            modo.ModFechIni = modFecIni != null ? modFecIni.Formuladat.Trim() : string.Empty; // fecha del modo
            modo.ModFechFin = modFecFin != null ? modFecFin.Formuladat.Trim() : string.Empty; //Fecha del modo
            modo.Comentario = modComentario != null ? modComentario.Formuladat : string.Empty;

            modo.FechaModif = modBanda != null ? modBanda.FechaactDesc : string.Empty;
            modo.UsuarioModif = modBanda != null ? modBanda.Lastuser : string.Empty;

            return modo;
        }

        /// <summary>
        /// Reporte de URS Calificadas HTML
        /// </summary>
        /// <returns></returns>
        public string ReporteListadoProvisionBaseHtml(string url, DateTime fecha, string estadoFiltro, bool tienePermisoEditar)
        {
            StringBuilder str = new StringBuilder();
            List<SmaUrsModoOperacionDTO> listaTodasURS = ReporteListadoURSBase(fecha, estadoFiltro);
            List<int> listaUrscodiConProvBase = listaTodasURS.Select(x => x.Urscodi).Distinct().ToList();

            List<SmaUrsModoOperacionDTO> listaURSDistinc = ListSmaUrsModoOperacions_Urs(-1);
            listaURSDistinc = listaURSDistinc.Where(x => listaUrscodiConProvBase.Contains(x.Urscodi)).ToList();

            str.Append("<div id='resultado' style='height: auto;'>");
            str.Append("<table id='tabla_agrupacion' border='0' class='pretty tabla-icono' cellspacing='0'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 200px'>Acción</th>");
            str.Append("<th style='width: 150px'>URS</th>");
            str.Append("<th style='width: 240px'>Empresa</th>");
            str.Append("<th style='width: 240px'>Central</th>");
            str.Append("<th style='width: 240px'>UNIDAD /<br /> MODO DE OPERACIÓN</th>");
            str.Append("<th style='width: 120px'>Potencia para bajar <br/>(MW)</th>");
            str.Append("<th style='width: 120px'>Potencia para subir <br/>(MW)</th>");
            str.Append("<th style='width: 120px'>Banda</th>");
            str.Append("<th style='width: 120px'>Precio para bajar <br/> (S/. /MW-Mes) </th>");
            str.Append("<th style='width: 120px'>Precio para subir <br/> (S/. /MW-Mes) </th>");
            str.Append("<th style='width: 120px'>Fecha Inicio</th>");
            str.Append("<th style='width: 120px'>Fecha Fin</th>");
            str.Append("<th style='width: 70px'>Estado</th>");
            str.Append("<th style='width: 250px'>Comentario</th>");
            str.Append("<th style='width: 120px'>Usuario<br />  modificación</th>");
            str.Append("<th style='width: 120px'>Fecha<br />  modificación</th>");
            str.Append("</tr>");
            #endregion

            str.Append("</thead>");
            str.Append("<tbody>");
            #region cuerpo
            foreach (var urs in listaURSDistinc)
            {
                int cantModos = listaURSDistinc.Count();

                // empresas
                var ursEmpresas = listaTodasURS.Where(x => x.Urscodi == urs.Urscodi).OrderBy(y => y.Emprnomb).ToList();

                int numeroModosTotales = 0;
                foreach (var item in ursEmpresas.GroupBy(x => x.Emprcodi))
                {
                    numeroModosTotales = numeroModosTotales + item.ToList().Count;
                }

                string clase = "";
                clase = ursEmpresas.First().Estado == "No Vigente" ? "clase_eliminado" : "";
                var acta = ursEmpresas.First().Acta;

                str.Append("<tr class='" + clase + "'>");

                str.AppendFormat("<td rowspan='" + numeroModosTotales + "'>");
                str.AppendFormat("<a style='padding: 2px;' href='JavaScript:verHistoricoURS({0},\"{1}\")'><img src='" + url + "Content/Images/btn-open.png' alt='Ver URS' /></a>", urs.Urscodi, urs.Ursnomb.Trim());
                if (tienePermisoEditar)
                    str.AppendFormat("<a style='padding: 2px;' href='JavaScript:editarURS({0},\"{1}\")'><img src='" + url + "Content/Images/btn-edit.png' alt='Editar uRS' /></a>", urs.Urscodi, urs.Ursnomb.Trim());
                if (!string.IsNullOrEmpty(acta))
                    str.AppendFormat("<a style='padding: 2px;' href='JavaScript:descargarActa(\"{0}\",{1}, 2)'><img src='" + url + "Content/Images/pdf.png' alt='Descargar Acta' /></a>", acta, urs.Urscodi);
                str.AppendFormat("</td>");

                str.Append(string.Format("<td rowspan='" + numeroModosTotales + "'>{0}</td>", urs.Ursnomb.Trim()));

                int contador = 0;
                foreach (var item in ursEmpresas.GroupBy(x => x.Emprcodi))
                {
                    if (contador != 0)
                    {
                        str.Append("</tr>");
                        str.Append("<tr class='" + clase + "'>");
                    }

                    var listaModos = item.OrderBy(y => y.Gruponomb).ToList();
                    var codempresa = item.Key;

                    //obtiene la urs para una empresa de la agrupación
                    var ursEmpresa = listaTodasURS.Find(x => x.Emprcodi == codempresa);
                    var ursCentral = listaModos.First();

                    str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", ursEmpresa.Emprnomb));
                    str.Append(string.Format("<td rowspan='" + listaModos.Count + "'>{0}</td>", ursCentral.Central));

                    int contador2 = 0;
                    foreach (var modo in listaModos)
                    {
                        clase = modo.Estado == "No Vigente" ? "clase_eliminado" : "";

                        if (contador2 != 0)
                        {
                            str.Append("</tr>");
                            str.Append("<tr class='" + clase + "'>");
                        }

                        str.AppendFormat("<td>{0}</td>", (modo.Gruponomb == "" ? "" : modo.Gruponomb));

                        //Obtener valores de los modos de la URS
                        str.AppendFormat("<td style='background-color: yellowgreen;'>{0}</td>", modo.PMin);
                        str.AppendFormat("<td style='background-color: yellowgreen;'>{0}</td>", modo.PMax);
                        str.AppendFormat("<td>{0}</td>", modo.BandaAdjudicada);
                        str.AppendFormat("<td>{0}</td>", modo.PrecMin);
                        str.AppendFormat("<td>{0}</td>", modo.PrecMax);
                        str.AppendFormat("<td>{0}</td>", modo.ModFechIni);
                        str.AppendFormat("<td>{0}</td>", modo.ModFechFin);
                        str.AppendFormat("<td>{0}</td>", modo.Estado);
                        str.AppendFormat("<td>{0}</td>", modo.Comentario);
                        str.AppendFormat("<td>{0}</td>", modo.UsuarioModif);
                        str.AppendFormat("<td>{0}</td>", modo.FechaModif);

                        contador2++;
                    }

                    contador++;
                }

                str.Append("</tr>");
            }
            #endregion
            str.Append("</tbody>");

            str.Append("</table>");
            str.Append("</div>");

            return str.ToString();
        }

        /// <summary>
        /// Dinuja la sección de edición para la URS
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="urscodi"></param>
        /// <param name="opcionActual"></param>
        /// <returns></returns>
        public string ListadoEdicionProvisionBase(DateTime fecha, int urscodi, int opcionActual, out List<PrGrupoDTO> listaModoXUrs)
        {
            List<SmaUrsModoOperacionDTO> listaTodasURSModo = GetByCriteriaSmaUrsModoOperacions().Where(x => x.Urscodi == urscodi).ToList();
            listaModoXUrs = listaTodasURSModo.GroupBy(x => x.Grupocodi)
                .Select(x => new PrGrupoDTO() { Grupocodi = x.Key.Value, Gruponomb = x.First().Gruponomb, Central = x.First().Central, Emprnomb = x.First().Emprnomb, Catecodi = x.First().Catecodi }).ToList();

            List<SmaUrsModoOperacionDTO> listaModoXFecIni = opcionActual != 3 ? ReporteEdicionURSBase(urscodi, fecha) : new List<SmaUrsModoOperacionDTO>();
            listaModoXFecIni.ForEach(x => x.FlagValidateFecha = 1);

            StringBuilder str = new StringBuilder();
            //captura de la data

            str.Append("<div id='resultado2' style='height: auto;'>");
            str.Append("<table id='tabla_agrupacion2' border='0' class='pretty tabla-icono' cellspacing='0'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 70px'>Acción</th>");
            str.Append("<th style='width: 240px'>Empresa</th>");
            str.Append("<th style='width: 240px'>Central</th>");
            str.Append("<th style='width: 240px'>UNIDAD /<br /> MODO DE OPERACIÓN</th>");
            str.Append("<th style='width: 120px'>Potencia para bajar <br/>(MW)</th>");
            str.Append("<th style='width: 120px'>Potencia para subir <br/>(MW)</th>");
            str.Append("<th style='width: 120px'>Banda</th>");
            str.Append("<th style='width: 120px'>Precio para bajar <br/> (S/. /MW-Mes) </th>");
            str.Append("<th style='width: 120px'>Precio para subir <br/> (S/. /MW-Mes) </th>");
            str.Append("<th style='width: 120px'>Fecha Inicio <br/>PERÍODO</th>");
            str.Append("<th style='width: 120px'>Fecha Fin <br/>PERÍODO</th>");
            str.Append("<th style='width: 250px'>Comentario</th>");
            str.Append("</tr>");
            #endregion

            str.Append("</thead>");
            str.Append("<tbody>");
            #region cuerpo

            int contador2 = 0;
            foreach (var modo in listaModoXFecIni)
            {
                str.AppendFormat("<tr id='tr_pos_{0}'>", contador2);

                if (opcionActual == 3)
                {
                    decimal? valorNull = null;
                    modo.PMin = valorNull;
                    modo.PMax = valorNull;
                    modo.BandaAdjudicada = valorNull;
                    //modo.
                    modo.Comentario = "";
                }

                str.AppendFormat("<td><input type='button' id='quitar_fila_{0}' onclick='quitarFilaTabla({0})' value='-' /></td>", contador2);
                str.AppendFormat("<td>{0}</td>", modo.Emprnomb);
                str.AppendFormat("<td>{0}</td>", modo.Central);
                str.AppendFormat("<td>{0}</td>", modo.Gruponomb);

                str.AppendFormat("<td style='background-color: yellowgreen;'>");
                str.AppendFormat("<input type='hidden' name='posicionFila' value='{0}' >", contador2);
                str.AppendFormat("<input id='grupocodiModo" + contador2 + "' type='hidden' name='grupocodiModo' value='{0}' >", modo.Grupocodi);
                str.AppendFormat("<input id='catecodiModo" + contador2 + "' type='hidden' name='catecodiModo' value='{0}' >", modo.Catecodi);
                str.AppendFormat("<input id='flagValidateFecha" + contador2 + "' type='hidden' name='flagValidateFecha' value='{0}' >", modo.FlagValidateFecha);
                str.AppendFormat("<input id='valorPotBajar" + contador2 + "' type='text' name='valorPotBajar' value='{0}' style='width: 40px; text-indent: 3px;'>", modo.PMin);
                str.AppendFormat("</td>");

                str.AppendFormat("<td style='background-color: yellowgreen;'><input id='valorPotSubir" + contador2 + "' type='text' name='valorPotSubir' value='{0}' style='width: 40px; text-indent: 3px;'></td>", modo.PMax);
                str.AppendFormat("<td><input id='valorBanda" + contador2 + "' type='text' name='valorBanda' value='{0}' style='width: 40px; text-indent: 3px;'></td>", modo.BandaAdjudicada);
                str.AppendFormat("<td><input id='valorPrecioBajar" + contador2 + "' type='text' name='valorPrecioBajar' value='{0}' style='width: 40px; text-indent: 3px;'></td>", modo.PrecMin);
                str.AppendFormat("<td><input id='valorPrecioSubir" + contador2 + "' type='text' name='valorPrecioSubir' value='{0}' style='width: 40px; text-indent: 3px;'></td>", modo.PrecMax);
                str.AppendFormat("<td><input id='fechaModoInicio" + contador2 + "' type='text' name='fechaModoInicio' value='{0}' style='WIDTH: 90px;' disabled></td>", modo.ModFechIni);
                str.AppendFormat("<td><input id='fechaModoFin" + contador2 + "' type='text' name='fechaModoFin' value='{0}' style='WIDTH: 90px;' disabled></td>", modo.ModFechFin);
                str.AppendFormat("<td><input id='comentario" + contador2 + "' type='text' name='comentario' value='{0}' style='WIDTH: 250px;'></td>", modo.Comentario);

                str.Append("</tr>");
                contador2++;
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");
            str.Append("</div>");

            return str.ToString();
        }

        /// <summary>
        /// Permite completar los formuladat para la URS BASE
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public List<PrGrupodatDTO> CompletarDataHistoricoURSBase(int grupocodi)
        {
            var listaURSFecIni = servMigra.ListarGrupodatHistoricoValores(ConstantesSubasta.ConceptoURSBaseFecInicio, grupocodi);
            var listaURSFecFin = servMigra.ListarGrupodatHistoricoValores(ConstantesSubasta.ConceptoURSBaseFecfIn, grupocodi);
            var listaURSActa = servMigra.ListarGrupodatHistoricoValores(ConstantesSubasta.ConceptoURSBaseActa, grupocodi);

            int contador = 0;
            foreach (var reg in listaURSFecIni)
            {
                reg.FechaInicio = DateTime.ParseExact(reg.Formuladat, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                reg.FechaIniciodesc = reg.FechaInicio.Date.ToString(ConstantesAppServicio.FormatoFecha);

                var fecFin = listaURSFecFin[contador];
                reg.FechaFin = DateTime.ParseExact(fecFin.Formuladat, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                reg.FechaFindesc = reg.FechaFin.Date.ToString(ConstantesAppServicio.FormatoFecha);

                var acta = listaURSActa[contador];
                reg.Acta = acta.Formuladat;

                DateTime fechaDatDesc = DateTime.ParseExact(reg.FechadatDesc, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                reg.FechadatDesc = fechaDatDesc.Date.ToString(ConstantesAppServicio.FormatoFecha);

                contador++;
            }

            return listaURSFecIni;
        }

        /// <summary>
        /// Resgitrar o actualizar URS Y MODOS
        /// </summary>
        /// <param name="regURS"></param>
        /// <param name="listaDetalleModos"></param>
        /// <param name="opcion"></param>
        public string RegistrarURSBaseGrupoDat(PrGrupodatDTO regURS, List<SmaUrsModoOperacionDTO> listaDetalleModos, int tipoAccion)
        {
            string resultDes = "";
            try
            {
                if (tipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    PrGrupodatDTO nuevo = new PrGrupodatDTO();
                    nuevo.Grupocodi = regURS.Grupocodi;
                    nuevo.Fechadat = regURS.Fechadat;
                    nuevo.Lastuser = regURS.Lastuser;
                    nuevo.Fechaact = regURS.Fechaact;
                    nuevo.Deleted = regURS.Deleted;

                    string[] valores = ConstantesSubasta.ConceptoURSBaseFull.Split(',');
                    List<int> conceptosURS = valores.Select(x => int.Parse(x)).ToList();
                    foreach (var concep in conceptosURS)
                    {
                        switch (concep)
                        {
                            case ConstantesSubasta.ConceptoURSBaseFecInicio:
                                nuevo.Formuladat = regURS.FechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
                                nuevo.Concepcodi = ConstantesSubasta.ConceptoURSBaseFecInicio;
                                break;
                            case ConstantesSubasta.ConceptoURSBaseFecfIn:
                                nuevo.Formuladat = regURS.FechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                                nuevo.Concepcodi = ConstantesSubasta.ConceptoURSBaseFecfIn;
                                break;
                            case ConstantesSubasta.ConceptoURSBaseActa:
                                nuevo.Formuladat = regURS.Acta;
                                nuevo.Concepcodi = ConstantesSubasta.ConceptoURSBaseActa;
                                break;
                        }
                        this.wsUrsCliente.SavePrGrupodat(nuevo);
                    }

                    //modos de la URS
                    foreach (var modo in listaDetalleModos)
                    {
                        DateTime fechaDatUrsRango = DateTime.ParseExact(modo.ModFechIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        PrGrupodatDTO modoOP = new PrGrupodatDTO();
                        modoOP.Grupocodi = modo.Grupocodi.Value;
                        modoOP.Fechadat = fechaDatUrsRango;
                        modoOP.Lastuser = regURS.Lastuser;
                        modoOP.Fechaact = regURS.Fechaact;
                        modoOP.Deleted = regURS.Deleted;
                        modoOP.Catecodi = modo.Catecodi;

                        if (modoOP.Catecodi == 5)
                        {
                            resultDes = this.SaveUpdateURSBaseModos(modoOP, ConstantesSubasta.AccionNuevo, ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseGrupHidraFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseGrupHidraComentario, modo.Comentario);
                        }

                        if (modoOP.Catecodi == 9)
                        {
                            resultDes = this.SaveUpdateURSBaseModos(modoOP, ConstantesSubasta.AccionNuevo, ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseMOHidroBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseMOHidroFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseMOHidroFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseMOHidroComentario, modo.Comentario);
                        }

                        if (modoOP.Catecodi == 2)
                        {
                            resultDes = this.SaveUpdateURSBaseModos(modoOP, ConstantesSubasta.AccionNuevo, ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseMOTermoBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseMOTermoFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseMOTermoFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseMOTermoComentario, modo.Comentario);
                        }
                    }
                }
                if (tipoAccion == ConstantesSubasta.AccionEditar)
                {
                    PrGrupodatDTO edicion = new PrGrupodatDTO();

                    string[] valores = ConstantesSubasta.ConceptoURSBaseFull.Split(',');
                    List<int> conceptosURS = valores.Select(x => int.Parse(x)).ToList();
                    foreach (var concep in conceptosURS)
                    {
                        switch (concep)
                        {
                            case ConstantesSubasta.ConceptoURSBaseFecInicio:
                                edicion = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseFecInicio, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                                edicion.Formuladat = regURS.FechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
                                edicion.Lastuser = regURS.Lastuser;
                                edicion.Fechaact = DateTime.Now;
                                break;
                            case ConstantesSubasta.ConceptoURSBaseFecfIn:
                                edicion = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseFecfIn, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                                edicion.Formuladat = regURS.FechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                                edicion.Lastuser = regURS.Lastuser;
                                edicion.Fechaact = DateTime.Now;
                                break;
                            case ConstantesSubasta.ConceptoURSBaseActa:
                                edicion = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseActa, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                                edicion.Formuladat = regURS.Acta;
                                edicion.Lastuser = regURS.Lastuser;
                                edicion.Fechaact = DateTime.Now;
                                break;
                        }
                        if (edicion == null)
                        {
                            throw new Exception("El registro no existe, no puede modificarse.");
                        }
                        wsUrsCliente.UpdatePrGrupodat(edicion);
                    }

                    #region obtener eliminados

                    List<SmaUrsModoOperacionDTO> listaEliminado = new List<SmaUrsModoOperacionDTO>();
                    List<SmaUrsModoOperacionDTO> listaBD = ReporteEdicionURSBase(regURS.Grupocodi, regURS.Fechadat.Value);
                    foreach (var regBd in listaBD)
                    {
                        var regModo = listaDetalleModos.Find(x => x.Grupocodi == regBd.Grupocodi && x.ModFechIni == regBd.ModFechIni);
                        if (regModo == null) //si el elemento de bd no se encuentra en la tabla html se considera eliminado
                            listaEliminado.Add(regBd);
                    }

                    //modos de la URS
                    foreach (var modo in listaEliminado)
                    {
                        DateTime fechaDatUrsRango = DateTime.ParseExact(modo.ModFechIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        PrGrupodatDTO modoOPDeleted = new PrGrupodatDTO();
                        modoOPDeleted.Fechadat = fechaDatUrsRango;
                        modoOPDeleted.Grupocodi = modo.Grupocodi.Value;
                        modoOPDeleted.Lastuser = regURS.Lastuser;

                        if (modo.Catecodi == 5)
                        {
                            var correlativoDeletedGrupHidra = ValidarEliminacion(modoOPDeleted.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMin, modoOPDeleted.Grupocodi);
                            modoOPDeleted.Deleted2 = correlativoDeletedGrupHidra;

                            resultDes = this.SaveUpdateURSBaseModos(modoOPDeleted, ConstantesSubasta.AccionEliminar, ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseGrupHidraFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseGrupHidraComentario, modo.Comentario);
                        }

                        if (modo.Catecodi == 9)
                        {
                            var correlativoDeletedMOHidro = ValidarEliminacion(modoOPDeleted.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMin, modoOPDeleted.Grupocodi);
                            modoOPDeleted.Deleted2 = correlativoDeletedMOHidro;

                            resultDes = this.SaveUpdateURSBaseModos(modoOPDeleted, ConstantesSubasta.AccionEliminar, ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseMOHidroBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseMOHidroFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseMOHidroFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseMOHidroComentario, modo.Comentario);
                        }

                        if (modo.Catecodi == 2)
                        {
                            var correlativoDeletedMOTermo = ValidarEliminacion(modoOPDeleted.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMin, modoOPDeleted.Grupocodi);
                            modoOPDeleted.Deleted2 = correlativoDeletedMOTermo;

                            resultDes = this.SaveUpdateURSBaseModos(modoOPDeleted, ConstantesSubasta.AccionEliminar, ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseMOTermoBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseMOTermoFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseMOTermoFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseMOTermoComentario, modo.Comentario);
                        }
                    }

                    #endregion


                    //modos de la URS
                    foreach (var modo in listaDetalleModos)
                    {
                        DateTime fechaDatUrsRango = DateTime.ParseExact(modo.ModFechIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        PrGrupodatDTO modoOPEdicion = new PrGrupodatDTO();
                        modoOPEdicion.Fechadat = fechaDatUrsRango;
                        modoOPEdicion.Grupocodi = modo.Grupocodi.Value;
                        modoOPEdicion.Lastuser = regURS.Lastuser;

                        if (modo.Catecodi == 5)
                        {
                            resultDes = this.SaveUpdateURSBaseModos(modoOPEdicion, ConstantesSubasta.AccionEditar, ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseGrupHidraFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseGrupHidraComentario, modo.Comentario);
                        }

                        if (modo.Catecodi == 9)
                        {
                            resultDes = this.SaveUpdateURSBaseModos(modoOPEdicion, ConstantesSubasta.AccionEditar, ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseMOHidroBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseMOHidroFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseMOHidroFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseMOHidroComentario, modo.Comentario);
                        }

                        if (modo.Catecodi == 2)
                        {
                            resultDes = this.SaveUpdateURSBaseModos(modoOPEdicion, ConstantesSubasta.AccionEditar, ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseMOTermoBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseMOTermoFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseMOTermoFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseMOTermoComentario, modo.Comentario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                resultDes = ex.Message;
            }
            return resultDes;
        }

        /// <summary>
        /// mantenimiento de pr_GrupoDat para los modos de la URS
        /// </summary>
        /// <param name="modoURS"></param>
        /// <param name="tipoAccion"></param>
        /// <param name="ConcepPMin"></param>
        /// <param name="formulaDatPmin"></param>
        /// <param name="ConcepPMax"></param>
        /// <param name="formulaDatPmax"></param>
        /// <param name="ConcepBanda"></param>
        /// <param name="formulaDatBanda"></param>
        /// <param name="ConcepComentario"></param>
        /// <param name="formulaDatComent"></param>
        /// <returns></returns>
        public string SaveUpdateURSBaseModos(PrGrupodatDTO modoURS, int tipoAccion, int ConcepPMin, string formulaDatPmin, int ConcepPMax, string formulaDatPmax, int ConcepBanda, string formulaDatBanda, int ConcepPrecMin, string formulaDatPrecMin, int ConcepPrecMax, string formulaDatPrecMax, int ConcepModFechIni, string formulaDatModFechIni, int ConcepModFechFin, string formulaDatModFechFin, int ConcepComentario, string formulaDatComent)
        {
            string resultDes = "";
            try
            {
                var regPeriodoExistente = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepModFechIni, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);

                if (tipoAccion == ConstantesSubasta.AccionNuevo || regPeriodoExistente == null)
                {
                    // guardar potencia minima
                    var potenciaMin = modoURS;
                    potenciaMin.Concepcodi = ConcepPMin;
                    potenciaMin.Formuladat = formulaDatPmin;
                    this.wsUrsCliente.SavePrGrupodat(potenciaMin);

                    // guardar potencia máxima
                    var potenciaMax = modoURS;
                    potenciaMax.Concepcodi = ConcepPMax;
                    potenciaMax.Formuladat = formulaDatPmax;
                    this.wsUrsCliente.SavePrGrupodat(potenciaMax);

                    // guardar Banda
                    var banda = modoURS;
                    banda.Concepcodi = ConcepBanda;
                    banda.Formuladat = formulaDatBanda;
                    this.wsUrsCliente.SavePrGrupodat(banda);

                    // guardar precio a bajar
                    var precioMin = modoURS;
                    precioMin.Concepcodi = ConcepPrecMin;
                    precioMin.Formuladat = formulaDatPrecMin;
                    this.wsUrsCliente.SavePrGrupodat(precioMin);

                    // guardar precio a subir
                    var precioMax = modoURS;
                    precioMax.Concepcodi = ConcepPrecMax;
                    precioMax.Formuladat = formulaDatPrecMax;
                    this.wsUrsCliente.SavePrGrupodat(precioMax);

                    // guardar fecha inicio del modo
                    var modoFechaIni = modoURS;
                    modoFechaIni.Concepcodi = ConcepModFechIni;
                    modoFechaIni.Formuladat = formulaDatModFechIni;
                    this.wsUrsCliente.SavePrGrupodat(modoFechaIni);

                    // guardar fecha fin del modo
                    var modoFechaFin = modoURS;
                    modoFechaFin.Concepcodi = ConcepModFechFin;
                    modoFechaFin.Formuladat = formulaDatModFechFin;
                    this.wsUrsCliente.SavePrGrupodat(modoFechaFin);

                    // guardar comentario
                    var comentario = modoURS;
                    comentario.Concepcodi = ConcepComentario;
                    comentario.Formuladat = formulaDatComent;
                    this.wsUrsCliente.SavePrGrupodat(comentario);
                }
                if (tipoAccion == ConstantesSubasta.AccionEditar)
                {
                    // actualizar potencia minima
                    var potenciaMinEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPMin, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    potenciaMinEdicion.Formuladat = formulaDatPmin;
                    potenciaMinEdicion.Lastuser = modoURS.Lastuser;
                    potenciaMinEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(potenciaMinEdicion);

                    // actualizar potencia máxima
                    var potenciaMaxEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPMax, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    potenciaMaxEdicion.Formuladat = formulaDatPmax;
                    potenciaMaxEdicion.Lastuser = modoURS.Lastuser;
                    potenciaMaxEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(potenciaMaxEdicion);

                    // actualizar Banda
                    var bandaEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepBanda, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    bandaEdicion.Formuladat = formulaDatBanda;
                    bandaEdicion.Lastuser = modoURS.Lastuser;
                    bandaEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(bandaEdicion);

                    // actualizar precio a bajar 
                    var precioMinEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPrecMin, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    precioMinEdicion.Formuladat = formulaDatPrecMin;
                    precioMinEdicion.Lastuser = modoURS.Lastuser;
                    precioMinEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(precioMinEdicion);

                    // actualizar precio a subir
                    var precioMaxEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPrecMax, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    precioMaxEdicion.Formuladat = formulaDatPrecMax;
                    precioMaxEdicion.Lastuser = modoURS.Lastuser;
                    precioMaxEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(precioMaxEdicion);

                    // actualizar fecha inicio del modo
                    var modoFechaIniEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepModFechIni, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    modoFechaIniEdicion.Formuladat = formulaDatModFechIni;
                    modoFechaIniEdicion.Lastuser = modoURS.Lastuser;
                    modoFechaIniEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(modoFechaIniEdicion);

                    // actualizar fecha fin del modo
                    var modoFechaFinEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepModFechFin, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    modoFechaFinEdicion.Formuladat = formulaDatModFechFin;
                    modoFechaFinEdicion.Lastuser = modoURS.Lastuser;
                    modoFechaFinEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(modoFechaFinEdicion);

                    // actualizar comentario
                    var comentarioEdicion = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepComentario, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    comentarioEdicion.Formuladat = formulaDatComent;
                    comentarioEdicion.Lastuser = modoURS.Lastuser;
                    comentarioEdicion.Fechaact = DateTime.Now;
                    this.wsUrsCliente.UpdatePrGrupodat(comentarioEdicion);
                }
                if (tipoAccion == ConstantesSubasta.AccionEliminar)
                {
                    // eliminar potencia minima
                    var potenciaMinDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPMin, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    potenciaMinDeleted.Lastuser = modoURS.Lastuser;
                    potenciaMinDeleted.Fechaact = DateTime.Now;
                    potenciaMinDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(potenciaMinDeleted);

                    // eliminar potencia máxima
                    var potenciaMaxDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPMax, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    potenciaMaxDeleted.Lastuser = modoURS.Lastuser;
                    potenciaMaxDeleted.Fechaact = DateTime.Now;
                    potenciaMaxDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(potenciaMaxDeleted);

                    // eliminar Banda
                    var bandaDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepBanda, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    bandaDeleted.Lastuser = modoURS.Lastuser;
                    bandaDeleted.Fechaact = DateTime.Now;
                    bandaDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(bandaDeleted);

                    // eliminar precio a bajar 
                    var precioMinDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPrecMin, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    precioMinDeleted.Lastuser = modoURS.Lastuser;
                    precioMinDeleted.Fechaact = DateTime.Now;
                    precioMinDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(precioMinDeleted);

                    // eliminar precio a subir
                    var precioMaxDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepPrecMax, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    precioMaxDeleted.Lastuser = modoURS.Lastuser;
                    precioMaxDeleted.Fechaact = DateTime.Now;
                    precioMaxDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(precioMaxDeleted);

                    // eliminar fecha inicio del modo
                    var modoFechaIniDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepModFechIni, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    modoFechaIniDeleted.Lastuser = modoURS.Lastuser;
                    modoFechaIniDeleted.Fechaact = DateTime.Now;
                    modoFechaIniDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(modoFechaIniDeleted);

                    // eliminar fecha fin del modo
                    var modoFechaFinDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepModFechFin, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    modoFechaFinDeleted.Lastuser = modoURS.Lastuser;
                    modoFechaFinDeleted.Fechaact = DateTime.Now;
                    modoFechaFinDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(modoFechaFinDeleted);

                    // eliminar comentario
                    var comentarioDeleted = wsUrsCliente.GetByIdPrGrupodat(modoURS.Fechadat.Value, ConcepComentario, modoURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                    comentarioDeleted.Lastuser = modoURS.Lastuser;
                    comentarioDeleted.Fechaact = DateTime.Now;
                    comentarioDeleted.Deleted2 = modoURS.Deleted2;
                    this.wsUrsCliente.UpdatePrGrupodat(comentarioDeleted);
                }
            }
            catch (Exception ex)
            {
                resultDes = ex.Message;
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }

            return resultDes;
        }

        /// <summary>
        /// Eliminado lógico de la urs y sus smodos de grupodat
        /// </summary>
        /// <param name="regURS"></param>
        /// <param name="listaDetalleModos"></param>
        /// <returns></returns>
        public string EliminarURSBaseGrupoDat(PrGrupodatDTO regURS, List<SmaUrsModoOperacionDTO> listaDetalleModos)
        {
            string resultDes = "";
            try
            {
                PrGrupodatDTO deleted = new PrGrupodatDTO();
                var correlativoDeleted = ValidarEliminacion(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseFecInicio, regURS.Grupocodi);

                string[] valores = ConstantesSubasta.ConceptoURSBaseFull.Split(',');
                List<int> conceptosURS = valores.Select(x => int.Parse(x)).ToList();
                foreach (var concep in conceptosURS)
                {
                    switch (concep)
                    {
                        case ConstantesSubasta.ConceptoURSBaseFecInicio:
                            deleted = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseFecInicio, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                            deleted.Lastuser = regURS.Lastuser;
                            deleted.Fechaact = DateTime.Now;
                            deleted.Deleted2 = correlativoDeleted;
                            break;
                        case ConstantesSubasta.ConceptoURSBaseFecfIn:
                            deleted = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseFecfIn, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                            deleted.Lastuser = regURS.Lastuser;
                            deleted.Fechaact = DateTime.Now;
                            deleted.Deleted2 = correlativoDeleted;
                            break;
                        case ConstantesSubasta.ConceptoURSBaseActa:
                            deleted = wsUrsCliente.GetByIdPrGrupodat(regURS.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseActa, regURS.Grupocodi, ConstantesSubasta.GrupodatActivo);
                            deleted.Lastuser = regURS.Lastuser;
                            deleted.Fechaact = DateTime.Now;
                            deleted.Deleted2 = correlativoDeleted;
                            break;
                    }
                    if (deleted == null)
                    {
                        throw new Exception("El registro no existe o ha sido eliminada.");
                    }
                    wsUrsCliente.UpdatePrGrupodat(deleted);
                }

                //modos de la URS
                foreach (var modo in listaDetalleModos)
                {
                    DateTime fechaDatUrsRango = DateTime.ParseExact(modo.ModFechIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    PrGrupodatDTO modoOPDeleted = new PrGrupodatDTO();
                    modoOPDeleted.Fechadat = fechaDatUrsRango;
                    modoOPDeleted.Grupocodi = modo.Grupocodi.Value;
                    modoOPDeleted.Lastuser = regURS.Lastuser;
                    //modoOPDeleted.Deleted2 = correlativoDeleted;

                    if (modo.Catecodi == 5)
                    {
                        var correlativoDeletedGrupHidra = ValidarEliminacion(modoOPDeleted.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMin, modoOPDeleted.Grupocodi);
                        modoOPDeleted.Deleted2 = correlativoDeletedGrupHidra;

                        resultDes = this.SaveUpdateURSBaseModos(modoOPDeleted, ConstantesSubasta.AccionEliminar, ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseGrupHidraFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseGrupHidraFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseGrupHidraComentario, modo.Comentario);
                    }

                    if (modo.Catecodi == 9)
                    {
                        var correlativoDeletedMOHidro = ValidarEliminacion(modoOPDeleted.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMin, modoOPDeleted.Grupocodi);
                        modoOPDeleted.Deleted2 = correlativoDeletedMOHidro;

                        resultDes = this.SaveUpdateURSBaseModos(modoOPDeleted, ConstantesSubasta.AccionEliminar, ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseMOHidroBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseMOHidroPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseMOHidroFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseMOHidroFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseMOHidroComentario, modo.Comentario);
                    }

                    if (modo.Catecodi == 2)
                    {
                        var correlativoDeletedMOTermo = ValidarEliminacion(modoOPDeleted.Fechadat.Value, ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMin, modoOPDeleted.Grupocodi);
                        modoOPDeleted.Deleted2 = correlativoDeletedMOTermo;

                        resultDes = this.SaveUpdateURSBaseModos(modoOPDeleted, ConstantesSubasta.AccionEliminar, ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMin, modo.PMinDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPotenciaMax, modo.PMaxDesc, ConstantesSubasta.ConceptoURSBaseMOTermoBanda, modo.BandaAdjudDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPrecioMin, modo.PrecMinDesc, ConstantesSubasta.ConceptoURSBaseMOTermoPrecioMax, modo.PrecMaxDesc, ConstantesSubasta.ConceptoURSBaseMOTermoFecInicio, modo.ModFechIni, ConstantesSubasta.ConceptoURSBaseMOTermoFecfIn, modo.ModFechFin, ConstantesSubasta.ConceptoURSBaseMOTermoComentario, modo.Comentario);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                resultDes = ex.Message;
            }
            return resultDes;
        }

        /// <summary>
        /// valida rango de fechas
        /// </summary>
        /// <param name="codigoURS"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ValidarInterseccionModosURSBase(int codigoURS, List<SmaUrsModoOperacionDTO> listaDetalleModos, DateTime fechaConsulta)
        {
            string msgesultado = "";

            //traer listado modos 
            List<SmaUrsModoOperacionDTO> listaModoXFecIni = ReporteEdicionURSBase(codigoURS, fechaConsulta);
            listaModoXFecIni.ForEach(x => x.FlagValidateFecha = 1);

            // quitar elementos que ya existen
            listaDetalleModos = listaDetalleModos.Where(x => x.FlagValidateFecha == 0).ToList(); // nuevos

            //Valida cruce solo entre los modos que son nuevos (agregados recientemente)
            foreach (var itemNuevo in listaDetalleModos)
            {
                DateTime fechaIniNuevo = DateTime.ParseExact(itemNuevo.ModFechIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinNuevo = DateTime.ParseExact(itemNuevo.ModFechFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                if (msgesultado != "")
                    break;

                var listaRestante = listaDetalleModos.Where(x => x != itemNuevo).ToList();
                foreach (var val in listaRestante)
                {
                    bool hayCruce = false;
                    DateTime fechaIniRestante = DateTime.ParseExact(val.ModFechIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fechaFinRestante = DateTime.ParseExact(val.ModFechFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                    var valor = fechaIniNuevo.CompareTo(fechaIniRestante);
                    //item nuevo inicia primero
                    if (valor < 0)
                    {
                        var valor2 = fechaFinNuevo.CompareTo(fechaIniRestante);

                        //item nuevo termina antes o justo cuando incia el rango del otro elemnto nuevo
                        hayCruce = valor2 <= 0 ? false : true;
                    }
                    else
                    {
                        //item nuevo inicia igual a rango del otro elemnto nuevo
                        if (valor == 0)
                        {
                            hayCruce = true;
                        }
                        //rango del otro elemnto nuevo inicia primero
                        else
                        {
                            var valor3 = fechaFinRestante.CompareTo(fechaIniNuevo);

                            //rango del otro elemnto nuevo termina antes que inice item nuevo 
                            hayCruce = valor3 <= 0 ? false : true;
                        }
                    }
                    if (hayCruce)
                    {
                        msgesultado = " Existe cruce de fechas con los modos y/o unidades";
                        break;
                    }
                }
            }

            //Valida esos modos nuevos libre de cruces con los modos existentes en la BD
            foreach (var nuevo in listaDetalleModos)
            {
                DateTime fechaIniModNew = DateTime.ParseExact(nuevo.ModFechIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinModNew = DateTime.ParseExact(nuevo.ModFechFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                if (msgesultado != "")
                    break;

                foreach (var item in listaModoXFecIni)
                {
                    bool hayInterseccion = false;
                    DateTime fechaIniModExist = DateTime.ParseExact(item.ModFechIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fechaFinModExist = DateTime.ParseExact(item.ModFechFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                    var valor = fechaIniModNew.CompareTo(fechaIniModExist);

                    //modo nuevo inicia primero
                    if (valor < 0)
                    {
                        var valor2 = fechaFinModNew.CompareTo(fechaIniModExist);

                        //modo nuevo termina antes o justo cuando incia el rango del histórico
                        hayInterseccion = valor2 <= 0 ? false : true;
                    }
                    else
                    {
                        //modo nuevo inicia igual a rango del histórico
                        if (valor == 0)
                        {
                            hayInterseccion = true;
                        }
                        //rango del histórico inicia primero
                        else
                        {
                            var valor3 = fechaFinModExist.CompareTo(fechaIniModNew);

                            //rango del histórico termina antes que inice modo nuevo
                            hayInterseccion = valor3 <= 0 ? false : true;
                        }
                    }
                    if (hayInterseccion)
                    {
                        msgesultado = " Existe cruce de fechas con los modos y/o unidades";
                        break;
                    }
                }
            }

            return msgesultado;
        }

        /// <summary>
        /// LISTADO DE NOMBRE DE LA CABECERA Y SU ANCHO
        /// </summary>
        /// <param name="listaOferta"></param>
        /// <param name="listaOfertaOld"></param>
        /// <param name="listaCabecera"></param>
        private void ListarCabeceraURSBase(out List<CabeceraRow> listaCabecera)
        {
            listaCabecera = new List<CabeceraRow>();
            listaCabecera.Add(new CabeceraRow() { TituloRow = "URS", Ancho = 25 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "EMPRESA", Ancho = 25 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "CENTRAL", Ancho = 25 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "UNIDAD <br/> MODOS OPERACIÓN", Ancho = 40 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "POTENCIA PARA BAJAR <br/> (MW)", Ancho = 10 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "POTENCIA PARA SUBIR <br/> (MW)", Ancho = 10 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "BANDA <br/> (MW)", Ancho = 10 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "PRECIO PARA BAJAR <br/> (S/. /MW-MES)", Ancho = 10 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "PRECIO PARA SUBIR <br/> (S/. /MW-MES))", Ancho = 10 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "FECHA INICIO", Ancho = 15 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "FECHA FIN", Ancho = 15 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "ESTADO", Ancho = 15 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "COMENTARIO", IsMerge = 1, Ancho = 40 });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "USUARIO MODIFICACIÓN", Ancho = 15 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "FECHA MODIFICACIÓN", Ancho = 20 });
        }

        /// <summary>
        /// Genera el archivo excel a exportar
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="fecha"></param>
        public void GenerarArchivoExcelURSBase(string ruta, string nombreArchivo, DateTime fecha, string estadoFiltro)
        {
            List<SmaUrsModoOperacionDTO> listaTodasURS = ReporteListadoURSBase(fecha, estadoFiltro);
            List<int> listaUrscodiConProvBase = listaTodasURS.Select(x => x.Urscodi).Distinct().ToList();

            List<SmaUrsModoOperacionDTO> listaURSDistinc = ListSmaUrsModoOperacions_Urs(-1); //cabecera
            listaURSDistinc = listaURSDistinc.Where(x => listaUrscodiConProvBase.Contains(x.Urscodi)).ToList();

            this.ListarCabeceraURSBase(out List<CabeceraRow> listaCabecera);

            FileInfo newFile = new FileInfo(ruta + nombreArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + nombreArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                //System.Drawing.Image img = null;

                string nombre = ConstantesSubasta.NombreTabURSBase;
                string titulo = ConstantesSubasta.TituloReporteURSBase;
                this.GenerarArchivoExcelURSBaseHoja(ref ws, xlPackage, img, titulo, nombre, 1, 2, listaTodasURS, listaURSDistinc, listaCabecera);

                if (ws == null)
                {
                    throw new Exception("No se generó el archivo Excel");
                }
            }
        }

        /// <summary>
        /// Dibuja hoja Excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="img"></param>
        /// <param name="titulo"></param>
        /// <param name="nombre"></param>
        /// <param name="rowIniHeader"></param>
        /// <param name="colIniHeader"></param>
        /// <param name="listaTodasURS"></param>
        /// <param name="listaURSDistinc"></param>
        /// <param name="listaCabecera"></param>
        private void GenerarArchivoExcelURSBaseHoja(ref ExcelWorksheet ws, ExcelPackage xlPackage, System.Drawing.Image img
            , string titulo, string nombre, int rowIniHeader, int colIniHeader, List<SmaUrsModoOperacionDTO> listaTodasURS, List<SmaUrsModoOperacionDTO> listaURSDistinc, List<CabeceraRow> listaCabecera)
        {
            try
            {
                ws = xlPackage.Workbook.Worksheets.Add(nombre);

                int filTitulo = rowIniHeader + 1;
                int colTitulo = colIniHeader + 4;
                ws.Cells[filTitulo, colTitulo].Value = titulo;
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filTitulo, colTitulo, filTitulo, colTitulo, "Calibri", 14);
                UtilExcel.CeldasExcelEnNegrita(ws, filTitulo, colTitulo, filTitulo, colTitulo);

                int row = rowIniHeader + 5;
                int col = colIniHeader;

                #region Cabecera

                int filaIniCab = row;
                int coluIniCab = col;
                int numeroColums = 15;
                int coluFinCab = coluIniCab + numeroColums - 1;
                int posCol = 0;
                foreach (var cab in listaCabecera)
                {
                    ws.Cells[filaIniCab, coluIniCab + posCol].Value = cab.TituloRow.ToUpper().Replace("<BR/>", "\n");
                    ws.Column(coluIniCab + posCol).Width = cab.Ancho;
                    posCol++;
                }

                UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "Centro");
                UtilExcel.CeldasExcelColorTexto(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "#2B579A");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "Calibri", 11);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab);
                UtilExcel.CeldasExcelWrapText(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab);
                ws.Row(filaIniCab).Height = 84;

                #endregion

                #region cuerpo

                int filaIniData = row + 1;
                int coluIniData = col;
                int filActual = filaIniData;
                for (var fila = 0; fila < listaURSDistinc.Count(); fila++)
                {
                    var urs = listaURSDistinc[fila];
                    if (urs != null)
                    {
                        int numeroModosTotales = 0;
                        // empresas
                        var ursEmpresas = listaTodasURS.Where(x => x.Urscodi == urs.Urscodi).OrderBy(y => y.Emprnomb).ToList();
                        foreach (var val in ursEmpresas.GroupBy(x => x.Emprcodi))
                        {
                            numeroModosTotales = numeroModosTotales + val.ToList().Count;
                        }

                        int colActual = coluIniData;
                        ws.Cells[filActual, colActual].Value = urs.Ursnomb.Trim();
                        UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual, filActual + numeroModosTotales - 1, colActual);
                        UtilExcel.BorderCeldas3(ws, filActual, colActual, filActual + numeroModosTotales - 1, colActual);

                        int contador = 0;
                        foreach (var item in ursEmpresas.GroupBy(x => x.Emprcodi))
                        {
                            var listaModos = item.OrderBy(y => y.Gruponomb).ToList();
                            var codempresa = item.Key;

                            //obtiene la urs para una empresa de la agrupación
                            var ursEmpresa = listaTodasURS.Find(x => x.Emprcodi == codempresa);
                            var ursCentral = listaModos.First();

                            ws.Cells[filActual, colActual + 1].Value = ursEmpresa.Emprnomb.Trim();
                            UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual + 1, filActual + listaModos.Count - 1, colActual + 1);
                            UtilExcel.BorderCeldas3(ws, filActual, colActual + 1, filActual + listaModos.Count - 1, colActual + 1);
                            ws.Cells[filActual, colActual + 2].Value = ursCentral.Central.Trim();
                            UtilExcel.CeldasExcelAgrupar(ws, filActual, colActual + 2, filActual + listaModos.Count - 1, colActual + 2);
                            UtilExcel.BorderCeldas3(ws, filActual, colActual + 2, filActual + listaModos.Count - 1, colActual + 2);

                            int contador2 = 0;
                            foreach (var modo in listaModos)
                            {
                                ws.Cells[filActual, colActual + 3].Value = modo.Gruponomb.Trim();
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 3, filActual, colActual + 3);
                                ws.Cells[filActual, colActual + 4].Value = modo.PMin;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 4, filActual, colActual + 4);
                                ws.Cells[filActual, colActual + 5].Value = modo.PMax;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 5, filActual, colActual + 5);
                                ws.Cells[filActual, colActual + 6].Value = modo.BandaAdjudicada;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 6, filActual, colActual + 6);

                                ws.Cells[filActual, colActual + 7].Value = modo.PrecMin;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 7, filActual, colActual + 7);
                                ws.Cells[filActual, colActual + 8].Value = modo.PrecMax;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 8, filActual, colActual + 8);
                                ws.Cells[filActual, colActual + 9].Value = modo.ModFechIni;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 9, filActual, colActual + 9);
                                ws.Cells[filActual, colActual + 10].Value = modo.ModFechFin;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 10, filActual, colActual + 10);

                                ws.Cells[filActual, colActual + 11].Value = modo.Estado;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 11, filActual, colActual + 11);
                                ws.Cells[filActual, colActual + 12].Value = modo.Comentario;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 12, filActual, colActual + 12);
                                ws.Cells[filActual, colActual + 13].Value = modo.UsuarioModif;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 13, filActual, colActual + 13);
                                ws.Cells[filActual, colActual + 14].Value = modo.FechaModif;
                                UtilExcel.BorderCeldas3(ws, filActual, colActual + 14, filActual, colActual + 14);

                                if (modo.Estado.ToUpper() == "NO VIGENTE")
                                    UtilExcel.CeldasExcelColorFondo(ws, filActual, colActual, filActual, colActual + 14, "#D9D9D9");

                                contador2++;
                                filActual = filActual + 1;
                            }
                            contador++;
                        }
                    }
                }
                // DAR FORMATO DE ALINEAMIENTO GENERAL
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData, filActual, numeroColums + 1, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData, filActual, numeroColums + 1, "Centro");
                UtilExcel.CeldasExcelWrapText(ws, filaIniData, coluIniData, filActual, numeroColums + 1);
                #endregion

                ws.Column(1).Width = 2;
                UtilExcel.AddImage(ws, img, rowIniHeader, colIniHeader);
                ws.View.FreezePanes(7, 6);
                //No mostrar lineas
                ws.View.ShowGridLines = false;
                ws.View.ZoomScale = 80;
                xlPackage.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw ex;
            }
        }

        #endregion

        #region Desencriptar Ofertas

        /// <summary>
        /// Desencriptar ofertas
        /// </summary>
        /// <param name="fechaOferta"></param>
        /// <param name="tipoOferta"></param>
        public void ProcesarDesencriptacionXFechaYTipoOferta(DateTime fechaOferta, int tipoOferta, string usuario)
        {
            DateTime fechaActual = DateTime.Now;

            List<SmaOfertaDetalleDTO> ofertaDetalleDTOs = FactorySic.GetSmaOfertaDetalleRepository().ListByDate(fechaOferta, fechaOferta, tipoOferta, ConstantesSubasta.EstadoDefecto);

            foreach (var ofertaDetalle in ofertaDetalleDTOs)
            {
                if (ofertaDetalle.Ofdeprecio != null)
                    if (AnalizarNumerico(ofertaDetalle.Ofdeprecio) == false)
                    {
                        ofertaDetalle.Ofdeprecio = DecryptData(ofertaDetalle.Ofdeprecio);
                        FactorySic.GetSmaOfertaDetalleRepository().UpdatePrecio(ofertaDetalle.Ofdecodi, ofertaDetalle.Ofdeprecio, fechaActual, usuario);
                    }
            }
        }

        /// <summary>
        /// Desencriptar envíos históricos anteriores
        /// Esta opción solo se habilitará el día del despliegue de las mejoras de Subastas
        /// </summary>
        /// <param name="fechaOferta"></param>
        /// <param name="tipoOferta"></param>
        public void ProcesarDesencriptacionEspecialInicial(DateTime fechaMaxOferta)
        {
            DateTime fechaActual = DateTime.Now;

            List<SmaOfertaDetalleDTO> ofertaDetalleDTOs = FactorySic.GetSmaOfertaDetalleRepository().ListByDate(DateTime.MinValue, fechaMaxOferta, -1, ConstantesSubasta.EstadoDefecto);

            foreach (var ofertaDetalle in ofertaDetalleDTOs)
            {
                if (ofertaDetalle.Ofdeprecio != null)
                    if (AnalizarNumerico(ofertaDetalle.Ofdeprecio) == false)
                    {
                        ofertaDetalle.Ofdeprecio = DecryptData(ofertaDetalle.Ofdeprecio);
                        FactorySic.GetSmaOfertaDetalleRepository().UpdatePrecio(ofertaDetalle.Ofdecodi, ofertaDetalle.Ofdeprecio, fechaActual, "SISTEMA");
                    }
            }
        }

        /// <summary>
        /// Proceso automatico para desencriptar las ofertas diarias 
        /// </summary>
        public void EjecutarProcesoAutomatico()
        {
            this.ProcesarDesencriptacionXFechaYTipoOferta(DateTime.Now.Date, ConstantesSubasta.OfertipoDiaria, "SISTEMA");
        }

        #region Encrypt Decrypt Section

        /// <summary>
        /// Permite encriptar la data ingresada
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        private string EncryptData(string plainText)
        {
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                byte[] keyBytes = new Rfc2898DeriveBytes(ConstantesSubasta.PasswordHash, Encoding.ASCII.GetBytes(ConstantesSubasta.SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
                var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(ConstantesSubasta.VIKey));

                byte[] cipherTextBytes;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                        cryptoStream.Close();
                    }
                    memoryStream.Close();
                }
                return Convert.ToBase64String(cipherTextBytes);
            }
        }

        /// <summary>
        /// Permite desencriptar la data ingresada
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public string DecryptData(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(ConstantesSubasta.PasswordHash, Encoding.ASCII.GetBytes(ConstantesSubasta.SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(ConstantesSubasta.VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        #endregion

        #endregion

        #region Horarios Subastas

        /// <summary>
        /// Actualizar nueva configuracion de la hora maxima del envio de agentes y ejecucion del proceso automatico
        /// </summary>
        /// <param name="entity"></param>
        public void RegistrarHoraMaxCargaDatosyProcesoAutomatico(SmaParamProcesoDTO entity)
        {
            //registrar sma_param
            this.SaveSmaParamProceso(entity);

            string[] separadas = entity.Papohoraenvioncp.Split(':');
            int hora = Convert.ToInt32(separadas[0]);
            int minutos = Convert.ToInt32(separadas[1]);

            //Actualizar SI_PROCESO
            SiProcesoDTO proceso = FactorySic.GetSiProcesoRepository().GetById(ConstantesSubasta.PrcscodiDesencriptarOfertaDiariaSubasta);
            proceso.Prschorainicio = hora;
            proceso.Prscminutoinicio = minutos;
            FactorySic.GetSiProcesoRepository().Update(proceso);
        }

        #endregion

        #region Ampliación de plazo Oferta por Defecto

        public List<SmaAmpliacionPlazoDTO> ListarAmpliacionPlazoOfDefecto()
        {
            var lista = GetByCriteriaSmaAmpliacionPlazos().OrderByDescending(x => x.Smaapaniomes).ThenBy(x => x.Smaapcodi).ToList();

            foreach (var item in lista)
            {
                FormatearSmaAmpliacionPlazo(item);
            }

            return lista;
        }

        private void FormatearSmaAmpliacionPlazo(SmaAmpliacionPlazoDTO obj)
        {
            DateTime mesActual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime mesRegistro = new DateTime(obj.Smaapaniomes.Year, obj.Smaapaniomes.Month, 1);

            obj.EsEditable = mesRegistro >= mesActual;
            obj.SmaapaniomesDesc = obj.Smaapaniomes.ToString(ConstantesAppServicio.FormatoMes);
            obj.SmaapaniomesTexto = string.Format("{0}-{1}", EPDate.f_NombreMesCorto(obj.Smaapaniomes.Month), obj.Smaapaniomes.Year - 2000);

            obj.SmaapplazodefectoDesc = obj.Smaapplazodefecto.ToString(ConstantesAppServicio.FormatoFechaFull);
            obj.SmaapnuevoplazoDesc = obj.Smaapnuevoplazo.ToString(ConstantesAppServicio.FormatoFechaFull);

            obj.SmaapestadoDesc = "";
            if (ConstantesSubasta.EstadoActivo == obj.Smaapestado) obj.SmaapestadoDesc = "Activo";
            if (ConstantesSubasta.EstadoInactivo == obj.Smaapestado) obj.SmaapestadoDesc = "Inactivo";

            obj.SmaapfeccreacionDesc = obj.Smaapfeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull2);

            obj.Smaapusumodificacion = obj.Smaapusumodificacion ?? "";

            obj.SmaapfecmodificacionDesc = "";
            if (obj.Smaapfecmodificacion != null) obj.SmaapfecmodificacionDesc = obj.Smaapfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
        }

        public void GuardarAmpliacionPlazo(SmaAmpliacionPlazoDTO obj, string usuario)
        {
            var listaBD = ListarAmpliacionPlazoOfDefecto();

            //plazo defecto
            ObtenerPlazoValidoCargaExtranet(ConstantesSubasta.OfertipoDefecto, obj.Smaapaniomes, out DateTime fechaHoraIniPlazo, out DateTime fechaHoraFinPlazo, out int maxDiaDiaria);
            obj.Smaapplazodefecto = fechaHoraFinPlazo;
            obj.Smaapnuevoplazo = obj.Smaapnuevoplazo.AddHours(fechaHoraFinPlazo.Hour).AddMinutes(fechaHoraFinPlazo.Minute); //agregar las HH:mm

            //validar 1
            if (obj.Smaapnuevoplazo < obj.Smaapplazodefecto)
                throw new ArgumentException("El campo 'Nuevo plazo' no deberá ser menor que el campo 'Plazo por defecto'.");

            //validar cambio
            if (obj.Smaapcodi > 0)
            {
                var objExistente = listaBD.Find(x => x.Smaapcodi == obj.Smaapcodi);
                if (objExistente.Smaapnuevoplazo == obj.Smaapnuevoplazo
                    && objExistente.Smaapestado == obj.Smaapestado)
                {
                    throw new ArgumentException("No se permite grabar la información dado que no se detectó cambios en el registro.");
                }
            }
            else
            {
                var objExistente = listaBD.Find(x => x.Smaapaniomes == obj.Smaapaniomes);
                if (objExistente != null)
                    throw new ArgumentException(string.Format("Ya existe el registro de ampliación de la Oferta para el mes {0}.", obj.Smaapaniomes.ToString(ConstantesAppServicio.FormatoMes)));
            }

            //transaccional
            if (obj.Smaapcodi == 0)
            {
                obj.Smaapfeccreacion = DateTime.Now;
                obj.Smaapusucreacion = usuario;
                SaveSmaAmpliacionPlazo(obj);
            }
            else
            {
                var objExistente = listaBD.Find(x => x.Smaapcodi == obj.Smaapcodi);
                objExistente.Smaapnuevoplazo = obj.Smaapnuevoplazo;
                objExistente.Smaapestado = obj.Smaapestado;
                objExistente.Smaapfecmodificacion = DateTime.Now;
                objExistente.Smaapusumodificacion = usuario;

                UpdateSmaAmpliacionPlazo(objExistente);
            }
        }

        #endregion

        #region Extranet Carga de Datos

        /// <summary>
        /// Se usa para habilitar o deshabilitar el seleccionado de fecha
        /// </summary>
        /// <param name="tipoOferta"></param>
        /// <param name="fechaOferta"></param>
        /// <returns></returns>
        public bool EsFechaHabilitado(int tipoOferta, DateTime fechaOferta, out DateTime fechaHoraIniPlazo, out DateTime fechaHoraFinPlazo)
        {
            //1. Verificar que la extranet está habilitado en los plazos default
            ObtenerPlazoValidoCargaExtranet(tipoOferta, fechaOferta, out fechaHoraIniPlazo, out fechaHoraFinPlazo, out int maxDiaDiaria);

            if (ConstantesSubasta.OfertipoDiaria == tipoOferta)
            {
                //Para la oferta diaria, se considera habilitado la carga de datos para una fecha x: fecha x-1 00:00 hasta fecha x-1 10:00 
                if (fechaHoraIniPlazo <= DateTime.Now && DateTime.Now <= fechaHoraFinPlazo)
                    return true;

                //caso especial de N días 
                DateTime fechaHoy = DateTime.Now.Date;

                DateTime fechaIniOfertaValida = fechaHoy.AddDays(2).Date;
                DateTime fechaFinOfertaValida = fechaIniOfertaValida.AddDays(maxDiaDiaria - 1).Date;

                return (fechaIniOfertaValida <= fechaOferta) && (fechaOferta <= fechaFinOfertaValida);
            }
            else
            {
                //en Oferta por defecto, verificar la ampliación
                DateTime? ampliacionPlazoMes = ObtenerUltimaAmpliacionXMes(fechaOferta);

                //si no hay ampliación entonces validar valores por default
                if (ampliacionPlazoMes == null)
                {
                    if (fechaHoraIniPlazo <= DateTime.Now && DateTime.Now <= fechaHoraFinPlazo)
                        return true;
                }
                else
                {
                    //validar con ampliación activa
                    fechaHoraFinPlazo = ampliacionPlazoMes.Value;

                    return (fechaHoraIniPlazo <= DateTime.Now) && (DateTime.Now <= ampliacionPlazoMes.Value);
                }
            }

            return false;
        }

        private DateTime? ObtenerUltimaAmpliacionXMes(DateTime fecha1Mes)
        {
            var listaBD = ListarAmpliacionPlazoOfDefecto();

            //ampliación activa
            var objMes = listaBD.Find(x => x.Smaapaniomes == fecha1Mes && x.Smaapestado == ConstantesSubasta.EstadoActivo);
            if (objMes != null)
                return objMes.Smaapnuevoplazo;

            return null;
        }

        /// <summary>
        /// Obtener los parametros generales
        /// </summary>
        /// <param name="listUrs"></param>
        /// <param name="fechaOferta"></param>
        /// <returns></returns>
        public SmaTraerParametrosDTO GetTraerParametros(DateTime fechaOferta)
        {

            SmaTraerParametrosDTO resultado = new SmaTraerParametrosDTO();

            //Verifica si tiene Oferta por defecto
            resultado.TParamOfertaDefecto = true;

            PrGrupodatDTO datoConf = this.GetValorConfiguracion(fechaOferta, ConstantesSubasta.PrecioMaximo);
            if (datoConf != null) resultado.TParamPrecioMaximo = Convert.ToDecimal(datoConf.Formuladat);

            datoConf = this.GetValorConfiguracion(fechaOferta, ConstantesSubasta.PrecioMinimo);
            if (datoConf != null) resultado.TParamPrecioMinimo = Convert.ToDecimal(datoConf.Formuladat);

            datoConf = this.GetValorConfiguracion(fechaOferta, ConstantesSubasta.PotenciaURSMinimoMan);
            if (datoConf != null) resultado.TParamPotenciaMinimaMan = Convert.ToDecimal(datoConf.Formuladat);

            resultado.TParamPrecioDefecto = resultado.TParamPrecioMaximo; // Traer precio x defecto del URS

            return resultado;
        }

        public void ObtenerPlazoValidoCargaExtranet(int tipoOferta, DateTime fechaOferta, out DateTime fechaHoraIniPlazo, out DateTime fechaHoraFinPlazo, out int maxDiaDiaria)
        {
            //configuración de BD
            SmaParamProcesoDTO rangoNCP = GetParamValidoEnvioyProcesoAutomatico();
            maxDiaDiaria = rangoNCP.Papomaxdiasoferta;

            var minhIni = DateTime.ParseExact(rangoNCP.Papohorainicio, "HH:mm", CultureInfo.InvariantCulture);
            var minhFin = DateTime.ParseExact(rangoNCP.Papohorafin, "HH:mm", CultureInfo.InvariantCulture);

            DateTime fechaIni;
            DateTime fechaFin;

            if (ConstantesSubasta.OfertipoDiaria == tipoOferta)
            {
                //en oferta diaria, solo se puede registrar el día anterior a la "fechaOferta"
                fechaIni = fechaOferta.AddDays(-1).Date;
                fechaFin = fechaOferta.AddDays(-1).Date;
            }
            else
            {
                //Antes: en oferta por defecto, solo se puede registrar en todo el mes anterior a la "fechaOferta"
                //fechaIni = new DateTime(fechaOferta.Year, fechaOferta.Month, 1).AddMonths(-1);
                //fechaFin = fechaIni.AddMonths(1).AddDays(-1);

                //la oferta por defecto siempre se puede cargar
                //Ahora: //registrar hasta 4 meses hacia adelante
                fechaIni = DateTime.Today;
                fechaFin = new DateTime(fechaOferta.Year, fechaOferta.Month, 1).AddDays(-1);
            }

            //Para la oferta diaria, se considera habilitado la carga de datos para una fecha x: fecha x-1 00:00 hasta fecha x-1 10:00 
            //para la oferta por defecto, se considera habilitado la carga de datos para el mes x: 01/x-1 00:00 hasta fin_mes/x-1 10:00
            fechaHoraIniPlazo = fechaIni.AddHours(minhIni.Hour).AddMinutes(minhIni.Minute);
            fechaHoraFinPlazo = fechaFin.AddHours(minhFin.Hour).AddMinutes(minhFin.Minute);
        }

        /// <summary>
        /// Permite obtener información de cada Modo de Operación en función a sus mantenimientos y parámetros configurados en SICOES
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<SmaUrsModoOperacionDTO> GetURSMOMantto(int usercode, DateTime fecha)
        {
            List<SmaUrsModoOperacionDTO> listUrsMOMantto = new List<SmaUrsModoOperacionDTO>();
            List<SmaUsuarioUrsDTO> datoUrsMo = this.GetByCriteriaSmaUsuarioUrssMO(usercode);
            string[] listGrupocodiMant = new string[48]; //lista de grupocodi x 48 horas

            ManttoAppServicio appMantto = new ManttoAppServicio();

            List<int> listaUrsCodis = datoUrsMo.Select(x => x.Urscodi.Value).Distinct().ToList();

            List<SmaUrsModoOperacionDTO> listaTodasURS = new List<SmaUrsModoOperacionDTO>();
            List<SmaUrsModoOperacionDTO> listaTodasURSBase = new List<SmaUrsModoOperacionDTO>();
            listaTodasURS = ReporteListadoURS(fecha, ConstantesSubasta.EstadoURSVigente);
            listaTodasURSBase = ReporteListadoURSBase(fecha, ConstantesSubasta.EstadoURSVigente);

            List<SmaUrsModoOperacionDTO> listaUrsGeneral = listaTodasURS.Where(x => listaUrsCodis.Contains(x.Urscodi)).ToList();
            List<SmaUrsModoOperacionDTO> listaUrsGeneralBase = listaTodasURSBase.Where(x => listaUrsCodis.Contains(x.Urscodi)).ToList();

            for (int i = 0; i < datoUrsMo.Count; i++)
            {
                var objUrsTemp = listaUrsGeneral.Find(x => x.Urscodi == (int)datoUrsMo[i].Urscodi) ?? new SmaUrsModoOperacionDTO();
                var objUrsTempBase = listaUrsGeneralBase.Find(x => x.Urscodi == (int)datoUrsMo[i].Urscodi) ?? new SmaUrsModoOperacionDTO();

                this.ObtenerDatosPotEfecMinRpfByModo(datoUrsMo[i].Grupocodi, datoUrsMo[i].Grupotipo, fecha, out decimal? bandaDisponible, out decimal capacidadMax);

                SmaUrsModoOperacionDTO dataUrsMOMantto = new SmaUrsModoOperacionDTO();
                dataUrsMOMantto.Urscodi = (int)datoUrsMo[i].Urscodi;
                dataUrsMOMantto.Ursnomb = datoUrsMo[i].Ursnomb;
                dataUrsMOMantto.Gruponomb = datoUrsMo[i].Gruponom;
                dataUrsMOMantto.Grupocodi = datoUrsMo[i].Grupocodi;

                dataUrsMOMantto.BandaAdjudicada = objUrsTempBase.BandaAdjudicada;
                dataUrsMOMantto.BandaURS = objUrsTemp.BandaURS;
                dataUrsMOMantto.BandaDisponible = bandaDisponible;

                var conmantto = appMantto.ConsultarManttoURS(datoUrsMo[i].Grupocodi, fecha);

                if (conmantto.Count > 0)
                {
                    dataUrsMOMantto.Intervalo = new string[conmantto.Count, 2];
                    dataUrsMOMantto.ListIntervalos = "";
                    for (int j = 0; j < conmantto.Count; j++)
                    {
                        dataUrsMOMantto.Intervalo[j, 0] = (string)conmantto[j].Evenini.Value.ToString("HH:mm");
                        dataUrsMOMantto.Intervalo[j, 1] = (string)conmantto[j].Evenfin.Value.ToString("HH:mm");
                        //marcar los mantenimientos
                        int hini = Convert.ToInt32(dataUrsMOMantto.Intervalo[j, 0].Substring(0, 2)) * 2 + ((Convert.ToInt32(dataUrsMOMantto.Intervalo[j, 0].Substring(3, 2)) > 0) ? 1 : 0);
                        int hfin = Convert.ToInt32(dataUrsMOMantto.Intervalo[j, 1].Substring(0, 2)) * 2 + ((Convert.ToInt32(dataUrsMOMantto.Intervalo[j, 1].Substring(3, 2)) > 0) ? 1 : 0);
                        for (int intv = hini; intv < hfin; intv++)
                        {
                            if (dataUrsMOMantto.ListIntervalos == "") dataUrsMOMantto.ListIntervalos = intv.ToString();
                            else
                                dataUrsMOMantto.ListIntervalos = dataUrsMOMantto.ListIntervalos + "," + intv.ToString();
                        }
                    }
                    dataUrsMOMantto.ManttoProgramado = "SI";
                }
                else dataUrsMOMantto.ManttoProgramado = "NO";
                listUrsMOMantto.Add(dataUrsMOMantto);

            }

            return listUrsMOMantto;
        }

        /// <summary>
        /// Obtener los datos de potencia efectiva, potencia minima, porcentaje rpf, variacion termica y provisionpasefirme por modo de operación
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="tipoModo"></param>
        /// <param name="fecha"></param>
        /// <param name="bandaDisponible"></param>
        /// <param name="capacidadMax"></param>
        private void ObtenerDatosPotEfecMinRpfByModo(int grupocodi, string tipoModo, DateTime fecha, out decimal? bandaDisponible, out decimal capacidadMax)
        {
            decimal porcentajeRpf = 0;
            decimal potEfectiva = 0;
            decimal potMinima = 0;
            decimal variacionTermica = 0;
            decimal provisionPaseFirme = 0;
            bandaDisponible = null;
            //int[] listGruposPotenciaMinimaNoAplicable = {289, 290, 291};
            string identificadorGruposPotenciaMinimaNoAplicable = "No Aplica";

            var obtdatmo = wsUrsCliente.ObtenerDatosMO_URS(grupocodi, fecha);

            if (obtdatmo.Count > 0)
            {
                List<PrGrupodatDTO> list = FactorySic.GetPrGrupodatRepository().ObtenerParametroPorConcepto("282");

                PrGrupodatDTO obj = list.OrderByDescending(x => x.Fechadat).FirstOrDefault();

                porcentajeRpf = this.AnalizarNumerico(obj.Formuladat) ? Convert.ToDecimal(obj.Formuladat) / 100 : ConstantesAppServicio.ErrorPorcRPF;

                for (int j = 0; j < obtdatmo.Count; j++)
                {
                    //Logger.Info("GetURSMOMantto - GrupoCodi = " + datoUrsMo[i].Grupocodi + " Grupotipo = " + datoUrsMo[i].Grupotipo);

                    if (tipoModo == "T")
                    {
                        switch (obtdatmo[j].Concepcodi)
                        {
                            case 14:
                                //Logger.Info("GetURSMOMantto - Termica - potEfectiva = " + obtdatmo[j].Formuladat);
                                potEfectiva = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) : ConstantesAppServicio.ErrorPotMax;
                                //Logger.Info("GetURSMOMantto - Convert Numerico - potEfectiva = " + potEfectiva);
                                break;
                            case 16:
                                //Logger.Info("GetURSMOMantto - Termica - potMinima = " + obtdatmo[j].Formuladat);
                                potMinima = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) : ConstantesAppServicio.ErrorPotMin;
                                //if (listGruposPotenciaMinimaNoAplicable.Contains(obtdatmo[j].Grupocodi))//(obtdatmo[j].Grupocodi)
                                if (identificadorGruposPotenciaMinimaNoAplicable == obtdatmo[j].Formuladat.Trim())
                                    potMinima = 0;
                                //Logger.Info("GetURSMOMantto - Convert Numerico - potMinima = " + potMinima);
                                break;
                            //case 236:
                            //Logger.Info("GetURSMOMantto - Termica - porcentajeRpf = " + obtdatmo[j].Formuladat);
                            //porcentajeRpf = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) / 100 : ConstantesAppServicio.ErrorPorcRPF;
                            //Logger.Info("GetURSMOMantto - Convert Numerico - porcentajeRpf = " + porcentajeRpf);
                            //break;
                            case 237:
                                //Logger.Info("GetURSMOMantto - Termica - variacionTermica = " + obtdatmo[j].Formuladat);
                                variacionTermica = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) / 100 : ConstantesAppServicio.ErrorVarTer;
                                //Logger.Info("GetURSMOMantto - Convert Numerico - variacionTermica = " + variacionTermica);
                                break;
                            case 253:
                                //Logger.Info("GetURSMOMantto - Termica - ProvisionPaseFirme = " + obtdatmo[j].Formuladat);
                                provisionPaseFirme = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) : 0;
                                //Logger.Info("GetURSMOMantto - Convert Numerico - ProvisionPaseFirme = " + ProvisionPaseFirme);
                                break;
                        }
                    }
                    else
                    { //Es Hidraulica
                        switch (obtdatmo[j].Concepcodi)
                        {
                            case 251:
                                //Logger.Info("GetURSMOMantto - Hidraulica - potEfectiva = " + obtdatmo[j].Formuladat);
                                potEfectiva = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) : ConstantesAppServicio.ErrorPotMax;
                                //Logger.Info("GetURSMOMantto - Convert Numerico - potEfectiva = " + potEfectiva);
                                break;
                            case 252:
                                //Logger.Info("GetURSMOMantto - Hidraulica - potMinima = " + obtdatmo[j].Formuladat);
                                potMinima = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) : ConstantesAppServicio.ErrorPotMin;
                                //Logger.Info("GetURSMOMantto - Convert Numerico - potMinima = " + potMinima);
                                break;
                            //case 246:
                            //Logger.Info("GetURSMOMantto - Hidraulica - porcentajeRpf = " + obtdatmo[j].Formuladat);
                            //porcentajeRpf = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) / 100 : ConstantesAppServicio.ErrorPorcRPF;
                            //Logger.Info("GetURSMOMantto - Convert Numerico - porcentajeRpf = " + porcentajeRpf);
                            //break;
                            case 247:
                                variacionTermica = 0;
                                break;
                            case 254:
                                //Logger.Info("GetURSMOMantto - Hidraulica - ProvisionPaseFirme = " + obtdatmo[j].Formuladat);
                                provisionPaseFirme = this.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) : 0;
                                //Logger.Info("GetURSMOMantto - Convert Numerico - ProvisionPaseFirme = " + ProvisionPaseFirme);
                                break;

                        }
                    }
                }


                if (potEfectiva == ConstantesAppServicio.ErrorPotMax)
                {
                    throw new IndexOutOfRangeException("Error: Potencia Efectiva con valor negativo");
                }
                else if (potMinima == ConstantesAppServicio.ErrorPotMin)
                {
                    throw new IndexOutOfRangeException("Error: Potencia Mínima con valor negativo");
                }
                else if (porcentajeRpf == ConstantesAppServicio.ErrorPorcRPF)
                {
                    throw new IndexOutOfRangeException("Error: Porcentaje RPF con valor negativo");
                }
                else if (variacionTermica == ConstantesAppServicio.ErrorVarTer)
                {
                    throw new IndexOutOfRangeException("Error: Variación Térmica con valor negativo");
                }

                //obtener banda disponible
                if (potEfectiva > 0)
                    bandaDisponible = (potEfectiva - variacionTermica * potEfectiva - potMinima);

                //Obtener capacidad maxima para mantenimientos
                decimal PG1 = (potEfectiva - variacionTermica * potEfectiva);
                PG1 = PG1 / (decimal)(1 + porcentajeRpf);

                decimal PG2 = potMinima;
                PG2 = PG2 / (decimal)(1 - porcentajeRpf);

                decimal PotenciaMaxima = PG1 - PG2;

                capacidadMax = PotenciaMaxima - provisionPaseFirme;

            }
            else // URSCodi no configurado con parametros
            {
                throw new Exception("No existen datos de potencia efectiva, mínima para el modo de operación.");
            }


        }

        /// <summary>
        /// obtener el date correcto
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="strfechaoferta"></param>
        /// <returns></returns>
        public DateTime GetFechaInput(int tipo, string strfechaoferta)
        {
            DateTime fechaOferta = DateTime.MinValue;
            if (tipo == ConstantesSubasta.OfertipoDiaria)
                fechaOferta = !string.IsNullOrEmpty(strfechaoferta) ? DateTime.ParseExact(strfechaoferta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now.Date;
            else
                fechaOferta = !string.IsNullOrEmpty(strfechaoferta) ? EPDate.GetFechaIniPeriodo(5, strfechaoferta, string.Empty, string.Empty, string.Empty) : DateTime.Now.Date;

            return fechaOferta;
        }

        /// <summary>
        /// obtener el formato de fecha correcto
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fechaOferta"></param>
        /// <returns></returns>
        public string GetFechaOfertaDesc(int tipo, DateTime fechaOferta)
        {
            if (tipo == ConstantesSubasta.OfertipoDiaria)
                return fechaOferta.ToString(ConstantesAppServicio.FormatoFecha);
            else
                return fechaOferta.ToString(ConstantesAppServicio.FormatoMes);
        }

        /// <summary>
        /// Validación obligatorio para Oferta diaria: Debe existir oferta defecto de enero de ese año
        /// </summary>
        /// <param name="fechaIniOferta"></param>
        /// <param name="numDia"></param>
        /// <param name="listaURSxUsuario"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public bool ExisteOfertaDefectoXRangoFecha(DateTime fechaIniOferta, int numDia, List<int> listaURSxUsuario, out string mensaje)
        {
            //URS calificadas vigentes en el periodo de oferta diaria
            DateTime fechaFinOferta = fechaIniOferta.AddDays(numDia - 1);
            List<SmaUrsModoOperacionDTO> listaUrsIniOferta = ReporteListadoURS(fechaIniOferta, ConstantesSubasta.EstadoURSVigente);
            List<SmaUrsModoOperacionDTO> listaUrsFinOferta = new List<SmaUrsModoOperacionDTO>();
            if (fechaIniOferta != fechaFinOferta) listaUrsFinOferta = ReporteListadoURS(fechaIniOferta, ConstantesSubasta.EstadoURSVigente);

            List<SmaUrsModoOperacionDTO> listaUrsDentroOferta = new List<SmaUrsModoOperacionDTO>();
            listaUrsDentroOferta.AddRange(listaUrsIniOferta);
            listaUrsDentroOferta.AddRange(listaUrsFinOferta);

            //filtros data historica de oferta defecto
            int opcionReporte = 1;
            int tipoOferta = ConstantesSubasta.OfertipoDefecto;
            int userCode = ConstantesSubasta.Todos;
            int oferCodi = ConstantesSubasta.Todos;
            int empresaCodi = ConstantesSubasta.Todos;

            //verificar por cada URS del usuario
            List<SmaUrsModoOperacionDTO> lUrsSinOfertaDef = new List<SmaUrsModoOperacionDTO>();
            foreach (var urscodi in listaURSxUsuario)
            {
                SmaUrsModoOperacionDTO objURSVigente = listaUrsDentroOferta.Find(x => x.Urscodi == urscodi);
                if (objURSVigente != null)
                {
                    DateTime fIniVigDia = DateTime.ParseExact(objURSVigente.FechaInico, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture); //mes de inicio de vigencia de URS
                    DateTime fFinVigDia = fechaIniOferta; //mes de la fecha inicial de la oferta diaria
                    if (fFinVigDia < fIniVigDia) fIniVigDia = fFinVigDia; //normalmente no debería ocurrir

                    //verificar que existe alguna oferta por defecto
                    DateTime fIniVigMes = new DateTime(fIniVigDia.Year, fIniVigDia.Month, 1);
                    DateTime fFinVigMes = new DateTime(fFinVigDia.Year, fFinVigDia.Month, 1);
                    List<SmaOfertaDTO> listaOferta = ListaConsultaOferta(opcionReporte, tipoOferta, fIniVigMes, fFinVigMes, userCode, oferCodi.ToString(), empresaCodi, urscodi.ToString(), ConstantesSubasta.FuenteExtranet);

                    if (!listaOferta.Any()) lUrsSinOfertaDef.Add(objURSVigente);
                }
            }

            //existe al menos una URS sin oferta por defecto en su periodo de vigencia
            mensaje = "";
            if (lUrsSinOfertaDef.Any())
            {
                string ursTexto = string.Join(", ", lUrsSinOfertaDef.Select(x => x.Ursnomb));
                string txtMes = EPDate.f_NombreMes(fechaIniOferta.Month).ToLower() + " " + fechaIniOferta.Year;
                mensaje = string.Format("Requiere registrar la oferta por defecto debido a que no la tiene para el periodo inicial de vigencia de {0} hasta {1}.", ursTexto, txtMes);

                return false;
            }

            return true;
        }

        #endregion

        #region Consultas Ofertas (Defecto y Diaria) - Intranet

        /// <summary>
        /// Obtener la data para reporte Html y excel
        /// </summary>
        /// <param name="opcionReporte"></param>
        /// <param name="tipoOferta"></param>
        /// <param name="fecha"></param>
        /// <param name="fechafin"></param>
        /// <param name="userCode"></param>
        /// <param name="oferCodi"></param>
        /// <param name="empresaCodi"></param>
        /// <param name="listUrs"></param>
        /// <param name="oferfuente"> Campo que indica que la informacion fue cargada desde extranet o fue creado por activacion de oferta desde intranet por el sistema</param>
        /// <returns></returns>
        public List<SmaOfertaDTO> ListaConsultaOferta(int opcionReporte, int tipoOferta, DateTime fecha, DateTime fechafin, int userCode, string oferCodi, int empresaCodi, string listUrs, string oferfuente)
        {
            //opcion 1: Reporte con envios activos, opcion 2: Reporte con data histórica
            string estadoEnvio = opcionReporte == 1 ? ConstantesSubasta.EstadoActivo : ConstantesSubasta.EstadoDefecto;
            if (estadoEnvio == ConstantesSubasta.EstadoDefecto && oferCodi == ConstantesSubasta.EstadoDefecto) throw new Exception("Código de envío inválido.");

            List<SmaOfertaDTO> listFinal = this.ListSmaOfertasInterna(tipoOferta, fecha, fechafin, userCode, oferCodi, estadoEnvio, empresaCodi, listUrs, oferfuente);

            //si es ofertas por defecto, no considerar los activados
            if (tipoOferta == ConstantesSubasta.OfertipoDefecto)
                listFinal = listFinal.Where(x => x.Oferfuente != ConstantesSubasta.EstadoActivo).ToList();

            listFinal = this.DividirDataOferta(listFinal);

            return listFinal;
        }

        /// <summary>
        /// Dividir la información existente (banda)
        /// </summary>
        /// <param name="listFinal"></param>
        /// <returns></returns>
        private List<SmaOfertaDTO> DividirDataOferta(List<SmaOfertaDTO> listFinal)
        {
            //Tipo de oferta (Subi, Bajar)
            List<SmaOfertaDTO> listaXTipo = new List<SmaOfertaDTO>();

            foreach (var reg in listFinal)
            {
                reg.Gruponomb = reg.OferlistMODes;

                if (ConstantesSubasta.TipoCargaBanda == reg.Ofdetipo)
                {
                    decimal potencia = reg.Repopotofer;

                    var regSubir = reg.Copy();
                    regSubir.Ofdetipo = ConstantesSubasta.TipoCargaSubir;
                    regSubir.Repopotofer = potencia / 2;

                    var regBajar = reg.Copy();
                    regBajar.Ofdetipo = ConstantesSubasta.TipoCargaBajar;
                    regBajar.Repopotofer = potencia / 2;

                    listaXTipo.Add(regSubir);
                    listaXTipo.Add(regBajar);
                }
                else
                    listaXTipo.Add(reg);
            }

            listaXTipo = listaXTipo.OrderBy(x => x.Oferfechainicio).ThenBy(x => x.Ursnomb).ThenBy(x => x.Ofdehorainicio).ThenBy(x => x.Ofdehorafin).ThenBy(x => x.Emprnomb).ThenBy(x => x.BandaDisponible).ThenBy(x => x.OferlistMODes).ToList();

            return listaXTipo;
        }

        /// <summary>
        /// Generar handson model
        /// </summary>
        /// <param name="opcionReporte"></param>
        /// <param name="tipoOferta"></param>
        /// <param name="fecha"></param>
        /// <param name="fechafin"></param>
        /// <param name="userCode"></param>
        /// <param name="oferCodi"></param>
        /// <param name="empresaCodi"></param>
        /// <param name="listUrs"></param>
        /// <returns></returns> 
        public List<HandsonModel> GenerarGrillaConsultaOferta(int opcionReporte, int tipoOferta, DateTime fecha, DateTime fechafin, int userCode, int oferCodi, int empresaCodi, string listUrs)
        {
            List<SmaOfertaDTO> listaOferta = this.ListaConsultaOferta(opcionReporte, tipoOferta, fecha, fechafin, userCode, oferCodi.ToString(), empresaCodi, listUrs, ConstantesSubasta.FuenteExtranet);
            List<SmaOfertaDTO> listaOfertaOld = new List<SmaOfertaDTO>();

            List<SmaOfertaDTO> listaEnvio = this.ListSmaOfertasxDia(tipoOferta, fecha, fechafin, userCode, -1, "-1", ConstantesSubasta.FuenteExtranet).OrderByDescending(x => x.Oferfechaenvio).ToList();

            List<int> listaOfercodi = this.ListarOfercodiOld(listaEnvio, oferCodi);

            if (listaOfercodi.Any())
            {
                listaOfertaOld = this.ListaConsultaOferta(2, tipoOferta, fecha, fechafin, userCode, string.Join(",", listaOfercodi), -1, "-1", ConstantesSubasta.FuenteExtranet);
                foreach (var reg in listaOfertaOld)
                {
                    ////desencriptar data historica
                    //if (reg.Oferfechainicio.Value < DateTime.Now.Date)
                    //{
                    //    if (reg.Repoprecio != null)
                    //        if (this.AnalizarNumerico(reg.Repoprecio) == false)
                    //            reg.Repoprecio = this.DecryptData(reg.Repoprecio);
                    //}
                }
            }

            return this.ListarGrillaConsultaOferta(tipoOferta, listaOferta, listaOfertaOld, out List<CabeceraRow> listaCabecera);
        }

        /// <summary>
        /// lista de envios historicos
        /// </summary>
        /// <param name="listaEnvio"></param>
        /// <param name="oferCodi"></param>
        /// <returns></returns>
        public List<int> ListarOfercodiOld(List<SmaOfertaDTO> listaEnvio, int oferCodi)
        {
            List<int> l = new List<int>();

            if (oferCodi > 0)
            {
                var objEnvio = this.GetByIdSmaOferta(oferCodi);

                var objEnvioOld = listaEnvio.FirstOrDefault(x => x.Oferfechainicio == objEnvio.Oferfechainicio && x.Usercode == objEnvio.Usercode && x.Oferfechaenvio < objEnvio.Oferfechaenvio);
                if (objEnvioOld != null)
                {
                    l.Add(objEnvioOld.Ofercodi);
                }
            }
            else
            {
                List<SmaOfertaDTO> listaEnvioActivo = listaEnvio.Where(x => x.Oferestado == "A").ToList();
                foreach (var objEnvio in listaEnvioActivo)
                {
                    var objEnvioOld = listaEnvio.FirstOrDefault(x => x.Oferfechainicio == objEnvio.Oferfechainicio && x.Usercode == objEnvio.Usercode && x.Oferfechaenvio < objEnvio.Oferfechaenvio);
                    if (objEnvioOld != null)
                        l.Add(objEnvioOld.Ofercodi);
                }
            }

            return l;
        }

        /// <summary>
        /// Obtener reporte con la información de Consulta Ofertas x Día
        /// </summary>
        /// <param name="listaOferta"></param>
        /// <returns></returns>
        private List<HandsonModel> ListarGrillaConsultaOferta(int tipoOferta, List<SmaOfertaDTO> listaOferta, List<SmaOfertaDTO> listaOfertaOld, out List<CabeceraRow> listaCabecera)
        {
            listaCabecera = new List<CabeceraRow>();
            listaCabecera.Add(new CabeceraRow() { TituloRow = "FECHA DE <br/> OFERTA", IsMerge = 1, Ancho = 70, AlineacionHorizontal = "Centro", TipoLimite = 1 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "URS", IsMerge = 1, Ancho = 80, AlineacionHorizontal = "Izquierda", TipoLimite = 1 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "HORA DE <br/> INICIO", IsMerge = 1, Ancho = 50, AlineacionHorizontal = "Centro", TipoLimite = 2 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "HORA DE <br/> FIN", IsMerge = 1, Ancho = 50, AlineacionHorizontal = "Centro", TipoLimite = 2 });
            if (tipoOferta == ConstantesSubasta.OfertipoDiaria)
                listaCabecera.Add(new CabeceraRow() { TituloRow = "POTENCIA <br/> OFERTADA", IsMerge = 1, Ancho = 70, AlineacionHorizontal = "Derecha", TipoLimite = 2, TipoDato = "Numerico" });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "PRECIO <br/> (S/. / MW-Mes)", IsMerge = 1, Ancho = 70, AlineacionHorizontal = "Derecha", TipoLimite = 2, TipoDato = "Numerico" });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "EMPRESA", IsMerge = 1, Ancho = 130, AlineacionHorizontal = "Izquierda", TipoLimite = 1 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "MODO DE <br/> OPERACIÓN", IsMerge = 0, Ancho = 200, AlineacionHorizontal = "Izquierda", TipoLimite = 1 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "BANDA <br/> CALIFICADA", IsMerge = 1, Ancho = 70, AlineacionHorizontal = "Derecha", TipoLimite = 1, TipoDato = "Numerico" });
            //listaCabecera.Add(new CabeceraRow() { TituloRow = "BANDA <br/> DISPONIBLE", IsMerge = 1, Ancho = 70, AlineacionHorizontal = "Derecha", TipoLimite = 2, TipoDato = "Numerico" });

            listaCabecera.Add(new CabeceraRow() { TituloRow = "CODIGO DE <br/> ENVÍO", IsMerge = 1, Ancho = 70, AlineacionHorizontal = "Centro", TipoLimite = 1 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "USUARIO", IsMerge = 1, Ancho = 70, AlineacionHorizontal = "Centro", TipoLimite = 1 });
            listaCabecera.Add(new CabeceraRow() { TituloRow = "FECHA DE <br/> ENVÍO", IsMerge = 1, Ancho = 70, AlineacionHorizontal = "Centro", TipoLimite = 1 });

            List<HandsonModel> listaTab = new List<HandsonModel>();

            List<int> listaTipo = new List<int>() { ConstantesSubasta.TipoCargaSubir, ConstantesSubasta.TipoCargaBajar };
            foreach (var tipoCarga in listaTipo)
            {
                HandsonModel model = new HandsonModel();

                //
                string[][] data = new string[1][];
                string[] headers = listaCabecera.Select(x => x.TituloRow).ToArray();
                List<int> widths = listaCabecera.Select(x => x.Ancho).ToList();
                object[] columnas = new object[headers.Length];
                List<CeldaMerge> arrMergeCells = new List<CeldaMerge>();
                List<CeldaCambios> arrCambioCells = new List<CeldaCambios>();
                List<int> filasActDefecto = new List<int>();

                int index = 0;

                List<SmaOfertaDTO> listaOfertaxTipo = listaOferta.Where(x => x.Ofdetipo == tipoCarga).ToList();
                List<SmaOfertaDTO> listaOfertaOldxTipo = listaOfertaOld.Where(x => x.Ofdetipo == tipoCarga).ToList();

                if (!listaOfertaxTipo.Any())
                {
                    data = new string[1][];

                    data[index] = new string[headers.Length];
                    model.Resultado = "No hay registro para la consulta realizada...";
                }
                else
                {
                    data = new string[listaOfertaxTipo.Count][];

                    foreach (SmaOfertaDTO item in listaOfertaxTipo)
                    {
                        string precioReporte = !string.IsNullOrEmpty(item.Repoprecio) ? (this.AnalizarNumerico(item.Repoprecio) ? item.Repoprecio : "NO DISPONIBLE") : "NO INGRESADO";
                        string[] itemDato;
                        if (tipoOferta == ConstantesSubasta.OfertipoDiaria)
                            itemDato = new string[] { this.GetFechaOfertaDesc( tipoOferta, item.Oferfechainicio.Value),item.Ursnomb,  item.Ofdehorainicio, item.Ofdehorafin, Convert.ToString(item.Repopotofer), precioReporte
                                    ,item.Emprnomb, item.Gruponomb, Convert.ToString(item.BandaCalificada) //Convert.ToString(item.BandaDisponible)
                                    ,item.Ofercodenvio.ToString(), item.Username, item.Oferfechaenvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2)};
                        else
                            itemDato = new string[] { this.GetFechaOfertaDesc( tipoOferta, item.Oferfechainicio.Value),item.Ursnomb,  item.Ofdehorainicio, item.Ofdehorafin, precioReporte
                                    ,item.Emprnomb, item.Gruponomb, Convert.ToString(item.BandaCalificada) //Convert.ToString(item.BandaDisponible)
                                    ,item.Ofercodenvio.ToString(), item.Username, item.Oferfechaenvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2)};

                        if (tipoOferta == ConstantesSubasta.OfertipoDiaria)
                        {
                            if (item.Oferfuente == "A")
                                filasActDefecto.Add(index);
                        }

                        #region Cambio entre versiones
                        int posAdd = tipoOferta == ConstantesSubasta.OfertipoDiaria ? 0 : -1;

                        //existe envio anterior
                        var regEnvioOld = listaOfertaOldxTipo.Find(x => x.Oferfechainicio == item.Oferfechainicio && x.Usercode == item.Usercode);

                        //existe data anterior
                        if (regEnvioOld != null && listaOfertaOldxTipo.Any())
                        {
                            SmaOfertaDTO itemOld = null;
                            itemOld = listaOfertaOldxTipo.Find(x => x.Urscodi == item.Urscodi);
                            if (itemOld == null)
                                arrCambioCells.Add(new CeldaCambios() { Row = index, Col = 1 });

                            itemOld = listaOfertaOldxTipo.Find(x => x.Urscodi == item.Urscodi && x.Emprnomb == item.Emprnomb);
                            if (itemOld == null)
                                arrCambioCells.Add(new CeldaCambios() { Row = index, Col = 6 + posAdd });

                            itemOld = listaOfertaOldxTipo.Find(x => x.Urscodi == item.Urscodi && x.Ofdehorainicio == item.Ofdehorainicio && x.Ofdehorafin == item.Ofdehorafin && x.Gruponomb == item.Gruponomb);
                            if (itemOld == null)
                            {
                                for (int i = 2; i <= 5 + posAdd; i++)
                                    arrCambioCells.Add(new CeldaCambios() { Row = index, Col = i });
                                arrCambioCells.Add(new CeldaCambios() { Row = index, Col = 7 + posAdd });
                                arrCambioCells.Add(new CeldaCambios() { Row = index, Col = 8 + posAdd });
                            }
                            else
                            {
                                string precioReporteActual = !string.IsNullOrEmpty(item.Repoprecio) ? (this.AnalizarNumerico(item.Repoprecio) ? item.Repoprecio : "NO DISPONIBLE") : "NO INGRESADO";
                                string precioReporteOld = !string.IsNullOrEmpty(itemOld.Repoprecio) ? (this.AnalizarNumerico(itemOld.Repoprecio) ? itemOld.Repoprecio : "NO DISPONIBLE") : "NO INGRESADO";

                                if (tipoOferta == ConstantesSubasta.OfertipoDiaria && item.Repopotofer != itemOld.Repopotofer)
                                    arrCambioCells.Add(new CeldaCambios() { Row = index, Col = 4 });
                                if (precioReporteActual != precioReporteOld)
                                    arrCambioCells.Add(new CeldaCambios() { Row = index, Col = 5 + posAdd });
                                if (item.Gruponomb != itemOld.Gruponomb)
                                    arrCambioCells.Add(new CeldaCambios() { Row = index, Col = 7 + posAdd });
                                if (item.BandaCalificada != itemOld.BandaCalificada)
                                    arrCambioCells.Add(new CeldaCambios() { Row = index, Col = 8 + posAdd });
                                //if (item.BandaDisponible != itemOld.BandaDisponible)
                                //    arrCambioCells.Add(new CeldaCambios() { Row = index, Col = 9 });
                            }
                        }

                        #endregion

                        data[index] = itemDato;
                        index++;
                    }
                }

                List<CeldaMerge> arrayLimUrs = this.ListarCeldaMezcladaXColumna(data, 1, new List<CeldaMerge>());
                List<CeldaMerge> arrayLimHoraInicio = this.ListarCeldaMezcladaXColumna(data, 2, arrayLimUrs);
                for (int m = 0; m < headers.Length; m++)
                {
                    var cabecera = listaCabecera[m];

                    columnas[m] = new
                    {
                        type = GridExcel.TipoTexto,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        dateFormat = string.Empty,
                        correctFormat = true,
                        defaultDate = string.Empty,
                        format = string.Empty,
                        className = cabecera.AlineacionHorizontal == "Derecha" ? "htRight" : (cabecera.AlineacionHorizontal == "Izquierda" ? "htLeft" : "htCenter"),
                    };

                    if (cabecera.IsMerge == 1) //columna seleccionada tiene merge
                    {
                        List<CeldaMerge> arrayLim = cabecera.TipoLimite == 1 ? arrayLimUrs : arrayLimHoraInicio;
                        arrMergeCells.AddRange(this.ListarCeldaMezcladaXColumna(data, m, arrayLim).Where(x => x.rowspan > 1).ToList());
                    }
                }

                model.ListaExcelData = data;
                model.Headers = headers;
                model.ListaColWidth = widths;
                model.Columnas = columnas;
                model.ListaMerge = arrMergeCells;
                model.ListaCambios = arrCambioCells;
                model.FilasActDefecto = filasActDefecto;

                listaTab.Add(model);
            }

            return listaTab;
        }

        private List<CeldaMerge> ListarCeldaMezcladaXColumna(string[][] data, int m, List<CeldaMerge> arrayLim)
        {
            List<CeldaMerge> arrMergeCells = new List<CeldaMerge>();

            int filasConsecutivas = 1;

            for (var i = 0; i < data.Count(); i++) //recorrer filas
            {
                if (m == 2 && i == 1)
                { }

                int intRowIndex = i - filasConsecutivas + 1;
                var objLim = arrayLim.Find(x => x.row <= i && i <= x.rowAbsoluto);
                int rowLimFin = objLim != null ? objLim.rowAbsoluto : 1000000;

                if (i < data.Count() - 1) //todas las filas menos la primera
                {
                    var itemCurrent = data[intRowIndex][m];
                    var itemNext = data[i + 1][m];

                    if ((itemCurrent != itemNext) || rowLimFin < i + 1)
                    {
                        arrMergeCells.Add(new CeldaMerge { rowAbsoluto = i, row = intRowIndex, col = m, rowspan = filasConsecutivas, colspan = 1 });

                        filasConsecutivas = 1;
                    }
                    else
                    {
                        filasConsecutivas++;
                    }
                }
                else
                {
                    //ultima fila
                    arrMergeCells.Add(new CeldaMerge { rowAbsoluto = i, row = intRowIndex, col = m, rowspan = filasConsecutivas, colspan = 1 });
                }
            }

            return arrMergeCells;
        }

        /// <summary>
        /// Generar reporte excel
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="opcionReporte"></param>
        /// <param name="tipoOferta"></param>
        /// <param name="fecha"></param>
        /// <param name="fechafin"></param>
        /// <param name="userCode"></param>
        /// <param name="oferCodi"></param>
        /// <param name="empresaCodi"></param>
        /// <param name="listUrs"></param>
        public void GenerarArchivoExcelConsultaOferta(string ruta, string nombreArchivo,
            int opcionReporte, int tipoOferta, DateTime fecha, DateTime fechafin, int userCode, int oferCodi, int empresaCodi, string listUrs)
        {
            List<SmaOfertaDTO> listaOferta = ListaConsultaOferta(opcionReporte, tipoOferta, fecha, fechafin, userCode, oferCodi.ToString(), empresaCodi, listUrs, ConstantesSubasta.FuenteExtranet);
            List<HandsonModel> listaTab = this.ListarGrillaConsultaOferta(tipoOferta, listaOferta, new List<SmaOfertaDTO>(), out List<CabeceraRow> listaCabecera);

            FileInfo newFile = new FileInfo(ruta + nombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + nombreArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                //System.Drawing.Image img = null;
                List<int> listaTipo = new List<int>() { ConstantesSubasta.TipoCargaSubir, ConstantesSubasta.TipoCargaBajar };
                foreach (var tipoCarga in listaTipo)
                {
                    string nombreTab = ConstantesSubasta.TipoCargaSubir == tipoCarga ? ConstantesSubasta.TabCargaSubir : ConstantesSubasta.TabCargaBajar;
                    string titulo = ConstantesSubasta.OfertipoDefecto == tipoOferta ? ConstantesSubasta.TituloReporteOfertaDefecto : ConstantesSubasta.TituloReporteOfertaDiaria;
                    this.GenerarArchivoExcelConsultaOfertaHoja(ref ws, xlPackage, img, titulo, nombreTab, 1, 2, listaTab[tipoCarga - 1], listaCabecera);
                }

                if (ws == null)
                {
                    throw new Exception("No se generó el archivo Excel");
                }
            }
        }

        /// <summary>
        /// Generar hoja
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="xlPackage"></param>
        /// <param name="img"></param>
        /// <param name="nombreTab"></param>
        /// <param name="rowIniHeader"></param>
        /// <param name="colIniHeader"></param>
        /// <param name="handson"></param>
        private void GenerarArchivoExcelConsultaOfertaHoja(ref ExcelWorksheet ws, ExcelPackage xlPackage, System.Drawing.Image img
            , string titulo, string nombreTab, int rowIniHeader, int colIniHeader, HandsonModel handson, List<CabeceraRow> listaCabecera)
        {
            ws = xlPackage.Workbook.Worksheets.Add(nombreTab);

            int filTitulo = rowIniHeader + 1;
            int colTitulo = colIniHeader + 4;
            ws.Cells[filTitulo, colTitulo].Value = titulo;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filTitulo, colTitulo, filTitulo, colTitulo, "Calibri", 14);
            UtilExcel.CeldasExcelEnNegrita(ws, filTitulo, colTitulo, filTitulo, colTitulo);

            int row = rowIniHeader + 5;
            int col = colIniHeader;

            #region Cabecera

            int filaIniCab = row;
            int coluIniCab = col;
            int numColumnas = handson.Columnas.Length;
            int coluFinCab = coluIniCab + numColumnas - 1;
            int posCol = 0;
            foreach (var cab in listaCabecera)
            {
                ws.Cells[filaIniCab, coluIniCab + posCol].Value = cab.TituloRow.ToUpper().Replace("<BR/>", "\n");
                posCol++;
            }

            UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab);
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "Centro");
            UtilExcel.CeldasExcelColorTexto(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "#2B579A");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab, "Calibri", 11);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab);
            UtilExcel.CeldasExcelWrapText(ws, filaIniCab, coluIniCab, filaIniCab, coluFinCab);
            ws.Row(filaIniCab).Height = 40;

            #endregion

            #region cuerpo

            int filaIniData = row + 1;
            int filaX = 0;
            int coluIniData = coluIniCab;
            for (var fila = 0; fila < handson.ListaExcelData.Count(); fila++)
            {
                var arrayFila = handson.ListaExcelData[fila];
                if (arrayFila != null)
                {
                    int colX = 0;
                    int filActual = filaIniData + filaX;

                    UtilExcel.BorderCeldas3(ws, filActual, coluIniCab, filActual, coluFinCab);
                    UtilExcel.CeldasExcelAlinearVerticalmente(ws, filActual, coluIniCab, filActual, coluFinCab, "Centro");

                    for (var columna = 0; columna < arrayFila.Length; columna++)
                    {
                        var cab = listaCabecera[columna];
                        int colActual = coluIniData + colX;

                        if (cab.TipoDato == "Numerico")
                        {
                            string numeroStr = !string.IsNullOrEmpty(arrayFila[columna]) ? ((arrayFila[columna].Replace(",", ".")).Replace(" ", "")).Trim() : string.Empty;
                            if (this.AnalizarNumerico(numeroStr))
                            {
                                //
                                decimal.TryParse(numeroStr, out decimal valorDecimal);
                                ws.Cells[filActual, colActual].Value = valorDecimal;
                            }
                            else
                                ws.Cells[filActual, colActual].Value = arrayFila[columna];
                        }
                        else
                            ws.Cells[filActual, colActual].Value = arrayFila[columna];
                        UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filActual, colActual, filActual, colActual, cab.AlineacionHorizontal);
                        colX++;
                    }
                    filaX++;
                }
            }

            foreach (var reg in handson.ListaMerge)
            {
                UtilExcel.CeldasExcelAgrupar(ws, filaIniData + reg.row, coluIniData + reg.col, filaIniData + reg.rowAbsoluto, coluIniData + reg.col);
            }

            UtilExcel.CeldasExcelWrapText(ws, filaIniData, coluIniCab, filaIniData + handson.ListaExcelData.Count(), coluFinCab);

            int colX1 = 0;
            foreach (var ancho in handson.ListaColWidth)
            {
                ws.Column(coluIniData + colX1).Width = (ancho / 4.5);
                colX1++;
            }


            #endregion

            ws.Column(1).Width = 2;
            UtilExcel.AddImage(ws, img, rowIniHeader, colIniHeader);

            //No mostrar lineas
            ws.View.ShowGridLines = false;
            ws.View.FreezePanes(filaIniCab + 1, coluIniCab + 2);
            ws.View.ZoomScale = 80;

            //Todo el excel con Font Arial
            //var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            //var cellFont = allCells.Style.Font;
            //cellFont.name="Arial";

            xlPackage.Save();
        }

        /// <summary>
        /// Generar tabla html de los envios
        /// </summary>
        /// <param name="tipoOferta"></param>
        /// <param name="fecha"></param>
        /// <param name="fechafin"></param>
        /// <param name="userCode"></param>
        /// <param name="empresaCodi"></param>
        /// <param name="listUrs"></param>
        /// <returns></returns>
        public string GenerarHtmlListaEnvios(int tipoOferta, DateTime fecha, DateTime fechafin, int userCode, int empresaCodi, string listUrs)
        {
            List<SmaOfertaDTO> listaOferta = this.ListSmaOfertasxDia(tipoOferta, fecha, fechafin, userCode, empresaCodi, listUrs, ConstantesSubasta.FuenteExtranet).OrderByDescending(x => x.Oferfechaenvio).ToList();

            StringBuilder str = new StringBuilder();

            #region cuerpo

            if (listaOferta.Count > 0)
            {
                string htmlTr = @"
                        <table id='tabla_envios' class='pretty tabla-adicional' style='width:500px'>
                <thead>
                    <tr>
                        <th>Fecha Oferta</th>
                        <th>Código de Envio</th>
                        <th>Fecha de Env&iacute;o</th>
                        <th>Usuario</th>
                        <th>Estado</th>
                    </tr>
                </thead>
                <tbody>                                                        
                    ";

                str.Append(htmlTr);
                foreach (var mp in listaOferta)
                {
                    string htmlFila = @"
                        <tr>
                            <td style='padding: 5px; text-align: center;'>{0}</td>
                            <td style='padding: 5px; text-align: center;'><a data-ofercodi='{1}' class='codenvio'> {2}</a></td>
                            <td style='padding: 5px; text-align: center;'>{3}</td>
                            <td>{4}</td>
                            <td style='padding: 5px; text-align: center;'>{5}</td>
                        </tr>
                    ";

                    htmlFila = string.Format(htmlFila, this.GetFechaOfertaDesc(tipoOferta, mp.Oferfechainicio.Value), mp.Ofercodi, mp.Ofercodenvio
                        , mp.Oferfechaenvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2), mp.Username, mp.Oferestado == "H" ? "Histórico" : "Activo");
                    str.Append(htmlFila);

                }
                str.Append(@"</tbody>
                        </table> ");
            }

            #endregion

            return str.ToString();

        }

        #endregion

        #region Reserva Secundaria
        /// <summary>
        /// Obtiene la tabla de subir y de bajar
        /// </summary>
        /// <param name="dataTablaSubir"></param>
        /// <param name="dataTablaBajar"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        public void CargarListadoReservaSecundaria(out TablaReporte dataTablaSubir, out TablaReporte dataTablaBajar, DateTime fechaIni, DateTime fechaFin)
        {
            List<SmaOfertaDTO> listaOfertaGeneral = this.ListaConsultaOferta(1, 1, fechaIni, fechaFin, -1, "-1", -1, "-1", ConstantesSubasta.FuenteExtranet);
            List<SmaOfertaDTO> listaOfertaSubir = listaOfertaGeneral.Where(x => x.Ofdetipo == 1).ToList();
            List<SmaOfertaDTO> listaOfertaBajar = listaOfertaGeneral.Where(x => x.Ofdetipo == 2).ToList();

            List<ReporteRSModel> listaSubir = ObtenerListadoReporteReservaSecundaria(listaOfertaSubir);
            List<ReporteRSModel> listaBajar = ObtenerListadoReporteReservaSecundaria(listaOfertaBajar);

            List<SmaUrsModoOperacionDTO> urs = this.ListSmaUrsModoOperacions_Urs(-1);

            dataTablaSubir = ObtenerListadoReservaSecundaria(urs, listaSubir, fechaIni, fechaFin);
            dataTablaBajar = ObtenerListadoReservaSecundaria(urs, listaBajar, fechaIni, fechaFin);
        }

        /// <summary>
        /// Obtiene listado de ofertas por cada 30min en un formato sencillo que se usará para formar los TablaReportes de ambas hojas (Subir y Bajar)
        /// </summary>
        /// <param name="listaOfertas"></param>
        /// <returns></returns>
        public List<ReporteRSModel> ObtenerListadoReporteReservaSecundaria(List<SmaOfertaDTO> listaOfertas)
        {
            List<ReporteRSModel> listaGeneral = new List<ReporteRSModel>();
            string[] horasI = { "00:00", "00:30", "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00", "21:30", "22:00", "22:30", "23:00", "23:30" };
            string[] horasF = { "00:30", "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00", "21:30", "22:00", "22:30", "23:00", "23:30", "23:59" };
            string[] horasR = { "00:30", "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00", "21:30", "22:00", "22:30", "23:00", "23:30", "23:59" };

            foreach (var oferta in listaOfertas)
            {


                DateTime fechaOfertada = oferta.Oferfechainicio.Value;
                string horaInicio = oferta.Ofdehorainicio.Trim();
                string horaFin = oferta.Ofdehorafin.Trim();
                decimal potenciaOfertada = oferta.Repopotofer;
                string urs = oferta.Ursnomb.Trim();

                string dia = fechaOfertada.ToString("dd/MM/yyyy");
                int indexIni = Array.IndexOf(horasI, horaInicio);
                int indexFin = Array.IndexOf(horasF, horaFin);

                for (int i = indexIni; i <= indexFin; i++)
                {
                    ReporteRSModel objOferta = new ReporteRSModel();

                    objOferta.Urs = urs;
                    objOferta.Dia = dia;
                    objOferta.Hora = horasR[i];
                    objOferta.PotenciaOfertada = potenciaOfertada;


                    listaGeneral.Add(objOferta);
                }

            }

            return listaGeneral;
        }

        /// <summary>
        /// Obtiene toda la data del reporte y lo guarda en un TablaReporte
        /// </summary>
        /// <param name="listadoUrs"></param>
        /// <param name="listaOferta"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public TablaReporte ObtenerListadoReservaSecundaria(List<SmaUrsModoOperacionDTO> listadoUrs, List<ReporteRSModel> listaOferta, DateTime fechaIni, DateTime fechaFin)
        {

            TablaReporte tabla = new TablaReporte();

            var timeSpan = fechaFin - fechaIni;
            int numDiasConsultados = timeSpan.Days + 1;
            string[] horas = { "00:30", "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00", "21:30", "22:00", "22:30", "23:00", "23:30", "23:59" };

            #region CABECERA

            CabeceraReporte cabRepo = new CabeceraReporte();
            int numUrs = listadoUrs.Count;

            string[,] matrizCabecera = new string[2, numUrs + 3];

            matrizCabecera[0, 0] = "Fecha";
            matrizCabecera[0, 1] = "Hora";
            matrizCabecera[0, 2] = "Mercado de Ajustes de Reserva Secundaria Automática";

            for (int col = 0; col < numUrs; col++)
            {
                matrizCabecera[1, 2 + col] = listadoUrs[col].Ursnomb.Trim();
            }

            matrizCabecera[1, 2 + numUrs] = "TOTAL";

            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;
            #endregion


            #region CUERPO

            string colorAzul = "#2980B9";
            string colorBlanco = "#FFFFFF";
            string colorNegro = "#000000";
            string colorRojo = "#E12222";
            string colorMarino = "#0AA3A1";

            List<RegistroReporte> registros = new List<RegistroReporte>();

            for (int dia = 0; dia < numDiasConsultados; dia++)
            {


                string fechaHoy = fechaIni.AddDays(dia).ToString("dd/MM/yyyy");

                for (int med = 0; med < 48; med++)
                {
                    RegistroReporte registro = new RegistroReporte();
                    List<string> valores = new List<string>();
                    List<CeldaReporte> celdas = new List<CeldaReporte>();
                    CeldaReporte cel = new CeldaReporte();

                    string hora = horas[med];



                    celdas.Add(new CeldaReporte(fechaHoy, colorBlanco, colorAzul)); //Texto blanco, Fondo azul
                    valores.Add(fechaHoy);

                    celdas.Add(new CeldaReporte(hora, colorBlanco, colorAzul));
                    valores.Add(hora);

                    decimal sumaFila = 0;

                    foreach (var urs in listadoUrs)
                    {
                        var ursnomb = urs.Ursnomb.Trim();
                        var encontrado = listaOferta.Where(x => x.Dia == fechaHoy && x.Hora == hora && x.Urs == ursnomb).ToList();

                        if (encontrado.Count() > 0)
                        {
                            ReporteRSModel reg = encontrado.First();

                            if (reg.PotenciaOfertada.ToString() == "0")
                            {
                                celdas.Add(new CeldaReporte(reg.PotenciaOfertada.ToString(), colorNegro, colorRojo));
                            }
                            else
                            {
                                celdas.Add(new CeldaReporte(reg.PotenciaOfertada.ToString(), colorNegro, colorBlanco));
                            }

                            valores.Add(reg.PotenciaOfertada.ToString());

                            sumaFila = sumaFila + reg.PotenciaOfertada.Value;
                        }
                        else
                        {
                            celdas.Add(new CeldaReporte("No Ofertó", colorBlanco, colorAzul));
                            valores.Add("No Ofertó");
                        }

                    }
                    celdas.Add(new CeldaReporte(sumaFila.ToString(), colorBlanco, colorAzul));
                    valores.Add(sumaFila.ToString());

                    registro.ListaCelda = celdas;
                    registro.ListaPropiedades = valores;
                    registros.Add(registro);

                }


            }
            tabla.ListaRegistros = registros;
            #endregion

            return tabla;
        }

        /// <summary>
        /// Genera el listado en html del reporte reserva secundaria
        /// </summary>
        /// <param name="tablaData"></param>
        /// <returns></returns>
        public string ListadoReservaSecundariaHTML(TablaReporte tablaData, string tipo)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            //int numUrs = dataCab[1].Length;
            int numUrs = registros[0].ListaPropiedades.Count - 3;


            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi2 = GenerarNumberFormatInfo2();

            int padding = 20;
            int anchoTotal = (100 + 80 + padding * 2) + numUrs * (120 + padding);

            strHtml.Append("<div id='listado_reporte' style='height: auto;'>");
            strHtml.AppendFormat("<table id='reporte-{0}' class='pretty tabla-icono' style='table-layout: fixed; width: {1}px;'>", tipo, anchoTotal);
            //strHtml.Append("<table id='reporte' class='pretty tabla-icono' style='width: 1100px;'>");

            #region cabecera
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th class='sticky-col first-col' rowspan='2' style='width: 100px;'>{0}</th>", dataCab[0, 0]);
            strHtml.AppendFormat("<th class='sticky-col second-col' rowspan='2' style='width: 80px;'>{0}</th>", dataCab[0, 1]);
            strHtml.AppendFormat("<th colspan='{0}' style='width: {2}px;'>{1}</th>", numUrs, dataCab[0, 2], numUrs * (120 + padding));
            strHtml.AppendFormat("<th class='sticky-col ultima-col' rowspan='2' style='width:120px;'>{0}</th>", dataCab[1, numUrs + 2]);

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            for (int col = 0; col < numUrs; col++)
            {
                strHtml.AppendFormat("<th style='width:120px;'>{0}</th>", dataCab[1, 2 + col]);

            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");

            foreach (var reg in registros)
            {
                strHtml.Append("<tr>");

                //foreach (string col in reg.ListaPropiedades)
                //{
                //    strHtml.AppendFormat("<td class='alignValorRight'>{0}</td>", col);
                //}
                int columna = 0;
                foreach (CeldaReporte col in reg.ListaCelda)
                {
                    string colorFondo = (columna == 0 || columna == 1 || columna == numUrs + 2) ? "#3e7ba2" : col.ColorFondo;
                    if (columna < 2)
                    {
                        if (columna == 0)
                            strHtml.AppendFormat("<td class='alignValorRight sticky-col first-col' style='color: {0}; background: {1};'>{2}</td>", col.ColorTexto, colorFondo, col.Texto);
                        if (columna == 1)
                            strHtml.AppendFormat("<td class='alignValorRight sticky-col second-col' style='color: {0}; background: {1};'>{2}</td>", col.ColorTexto, colorFondo, col.Texto);

                    }
                    else
                    {
                        if (col.Texto != "No Ofertó")
                        {
                            if (columna == numUrs + 2)
                                strHtml.AppendFormat("<td class='alignValorRight sticky-col ultima-col' style='color: {0}; background: {1};'>{2}</td>", col.ColorTexto, colorFondo, UtilAnexoAPR5.ImprimirValorTotalHtml(decimal.Parse(col.Texto), nfi2));
                            else
                                strHtml.AppendFormat("<td class='alignValorRight' style='color: {0}; background: {1};'>{2}</td>", col.ColorTexto, colorFondo, UtilAnexoAPR5.ImprimirValorTotalHtml(decimal.Parse(col.Texto), nfi2));
                        }

                        else
                            strHtml.AppendFormat("<td class='alignValorRight' style='color: {0}; background: {1};'>{2}</td>", col.ColorTexto, colorFondo, col.Texto);
                    }

                    columna++;
                }
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");

            strHtml.Append("</table>");
            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Genera el excel para las dos hojas (subir y bajar) del reporte se reserva secundaria
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="fecInicial"></param>
        /// <param name="fecFinal"></param>
        public void GenerarArchivoExcelReservaSecundaria(string ruta, string nombreArchivo, DateTime fecInicial, DateTime fecFinal)
        {
            CargarListadoReservaSecundaria(out TablaReporte dataTablaSubir, out TablaReporte dataTablaBajar, fecInicial, fecFinal);

            FileInfo newFile = new FileInfo(ruta + nombreArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + nombreArchivo);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                try
                {
                    ExcelWorksheet ws = null;

                    xlPackage.Workbook.Worksheets.Select(x => x.View.ShowGridLines = false).ToList();

                    #region "Hoja Subir"    
                    //ws = xlPackage.Workbook.Worksheets[1];
                    ws = xlPackage.Workbook.Worksheets.Add("Subir");

                    GenerarExcelReservaSecundaria(ws, dataTablaSubir);
                    #endregion

                    #region "Hoja Bajar"    
                    //ws = xlPackage.Workbook.Worksheets[2];
                    ws = xlPackage.Workbook.Worksheets.Add("Bajar");
                    GenerarExcelReservaSecundaria(ws, dataTablaBajar);
                    #endregion


                    if (ws != null)
                    {
                        xlPackage.Workbook.Worksheets.Select(x => x.View.ShowGridLines = false).ToList();
                        xlPackage.Save();
                    }
                    else
                    {
                        throw new Exception("No se generó el archivo Excel");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Genera y da formato a la tabla de l reporte reserva secundaria
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="tablaData"></param>
        private void GenerarExcelReservaSecundaria(ExcelWorksheet ws, TablaReporte tablaData)
        {
            string colorCabecera = "#CAC5B5";
            string colorAzul = "#2980B9";
            string colorBlanco = "#FFFFFF";
            string colorNegro = "#000000";
            string colorRojo = "#E12222";
            string colorMarino = "#0AA3A1";

            int filaIniFecha = 3;
            int coluIniFecha = 2;

            int filaIniData = filaIniFecha + 2;
            int coluIniData = coluIniFecha;

            int ultimaFila = 0;
            int ultimaColu = 0;

            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            int numUrs = registros[0].ListaPropiedades.Count - 3;

            #region cabecera

            ws.Cells[filaIniFecha, coluIniFecha + 0].Value = dataCab[0, 0];
            ws.Cells[filaIniFecha, coluIniFecha + 1].Value = dataCab[0, 1];
            ws.Cells[filaIniFecha, coluIniFecha + 2].Value = dataCab[0, 2];

            for (int col = 0; col < numUrs; col++)
            {
                ws.Cells[filaIniFecha + 1, coluIniFecha + 2 + col].Value = dataCab[1, 2 + col];

            }

            ultimaColu = coluIniFecha + numUrs + 2;

            ws.Cells[filaIniFecha + 1, ultimaColu].Value = dataCab[1, numUrs + 2];

            #region FormatoCabecera

            ws.Column(coluIniFecha).Width = 14;
            ws.Column(coluIniFecha + 1).Width = 10;
            for (int c = coluIniFecha + 2; c <= ultimaColu; c++)
            {
                ws.Column(c).Width = 15;
            }

            ws.Row(filaIniFecha).Height = 26;

            UtilExcel.CeldasExcelAgrupar(ws, filaIniFecha, coluIniFecha, filaIniFecha + 1, coluIniFecha);
            UtilExcel.CeldasExcelAgrupar(ws, filaIniFecha, coluIniFecha + 1, filaIniFecha + 1, coluIniFecha + 1);
            UtilExcel.CeldasExcelAgrupar(ws, filaIniFecha, coluIniFecha + 2, filaIniFecha, ultimaColu);

            UtilExcel.CeldasExcelColorTexto(ws, filaIniFecha, coluIniFecha, filaIniFecha + 1, ultimaColu, colorNegro);
            UtilExcel.CeldasExcelColorFondo(ws, filaIniFecha, coluIniFecha, filaIniFecha + 1, ultimaColu, colorCabecera);

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniFecha, coluIniFecha, filaIniFecha, ultimaColu, "Arial", 14);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniFecha + 1, coluIniFecha + 2, filaIniFecha + 1, ultimaColu, "Arial", 9);

            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniFecha, coluIniFecha, filaIniFecha + 1, ultimaColu, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniFecha, coluIniFecha, filaIniFecha + 1, ultimaColu, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniFecha, coluIniFecha, filaIniFecha + 1, ultimaColu);
            #endregion

            #endregion

            #region cuerpo

            int fila = 0;
            foreach (var reg in registros)
            {
                int columna = 0;
                foreach (CeldaReporte col in reg.ListaCelda)
                {
                    //celdas fecha y hora
                    if (columna < 2)
                    {
                        ws.Cells[filaIniData + fila, coluIniData + columna].Value = col.Texto;
                        UtilExcel.CeldasExcelColorTexto(ws, filaIniData + fila, coluIniData + columna, filaIniData + fila, coluIniData + columna, colorNegro);
                        UtilExcel.CeldasExcelColorFondo(ws, filaIniData + fila, coluIniData + columna, filaIniData + fila, coluIniData + columna, colorCabecera);
                    }
                    else
                    {
                        //celdas no ofertadas
                        if (col.Texto != "No Ofertó")
                        {
                            //ofertados con 0
                            if (col.Texto == "0")
                            {
                                UtilExcel.CeldasExcelColorTexto(ws, filaIniData + fila, coluIniData + columna, filaIniData + fila, coluIniData + columna, colorBlanco);
                                UtilExcel.CeldasExcelColorFondo(ws, filaIniData + fila, coluIniData + columna, filaIniData + fila, coluIniData + columna, colorRojo);
                            }
                            else //celdas con valor mayor a 0
                            {
                                UtilExcel.CeldasExcelColorTexto(ws, filaIniData + fila, coluIniData + columna, filaIniData + fila, coluIniData + columna, colorNegro);
                                UtilExcel.CeldasExcelColorFondo(ws, filaIniData + fila, coluIniData + columna, filaIniData + fila, coluIniData + columna, colorBlanco);
                            }
                            ws.Cells[filaIniData + fila, coluIniData + columna].Value = decimal.Parse(col.Texto);
                            ws.Cells[filaIniData + fila, coluIniData + columna].Style.Numberformat.Format = "#,##0.00";
                        }
                        else  //No ofertó
                        {
                            ws.Cells[filaIniData + fila, coluIniData + columna].Value = col.Texto;
                            UtilExcel.CeldasExcelColorTexto(ws, filaIniData + fila, coluIniData + columna, filaIniData + fila, coluIniData + columna, colorBlanco);
                            UtilExcel.CeldasExcelColorFondo(ws, filaIniData + fila, coluIniData + columna, filaIniData + fila, coluIniData + columna, colorAzul);
                        }
                    }

                    if (ultimaColu == coluIniData + columna)
                    {
                        UtilExcel.CeldasExcelColorTexto(ws, filaIniData + fila, coluIniData + columna, filaIniData + fila, coluIniData + columna, colorNegro);
                        UtilExcel.CeldasExcelColorFondo(ws, filaIniData + fila, coluIniData + columna, filaIniData + fila, coluIniData + columna, colorCabecera);
                    }
                    columna++;
                }

                fila++;

            }
            ultimaFila = filaIniData + fila - 1;

            #region FormatoCuerpo
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "Arial", 11);

            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData, ultimaFila, ultimaColu, "Centro");
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniData, coluIniData, ultimaFila, coluIniData + 1);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniData, ultimaColu, ultimaFila, ultimaColu);

            UtilExcel.BorderCeldas2(ws, filaIniFecha, coluIniFecha, ultimaFila, ultimaColu);
            UtilExcel.BorderCeldas5(ws, filaIniFecha, coluIniFecha, ultimaFila, ultimaColu);
            #endregion


            #endregion

            ws.View.ShowGridLines = false;
            ws.View.FreezePanes(5, 4);
            ws.View.ZoomScale = 80;
        }

        /// <summary>
        /// formatea némuero decimal a 2 decimales
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GenerarNumberFormatInfo2()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 2;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        #endregion

        #region RSF_2024

        #region Activacion de Ofertas por defecto
        /// <summary>
        /// Devuelve el listado de motivos de activacion de ofertas por defecto ordenados alfabeticamente
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public List<SmaMaestroMotivoDTO> ListarMotivosActivacion(string estado)
        {
            List<SmaMaestroMotivoDTO> lstSalida = new List<SmaMaestroMotivoDTO>();

            List<SmaMaestroMotivoDTO> lstMotivosTotales = ListSmaMaestroMotivos();

            if (estado == ConstantesSubasta.Todos.ToString())
            {
                lstSalida = lstMotivosTotales.OrderBy(x => x.Smammdescripcion).ToList();
            }
            else
            {
                lstSalida = lstMotivosTotales.Where(x => x.Smammestado == estado).OrderBy(x => x.Smammdescripcion).ToList();
            }


            return lstSalida;

        }

        /// <summary>
        /// Obtener  el historial de activaciones para cierta fecha oferta
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <returns></returns>
        public List<SmaActivacionOfertaDTO> ObtenerHistorialActivaciones(DateTime fechaDeOferta)
        {
            List<SmaActivacionOfertaDTO> salida = new List<SmaActivacionOfertaDTO>();

            salida = FactorySic.GetSmaActivacionOfertaRepository().ListarActivacionesPorRangoFechas(fechaDeOferta, fechaDeOferta);
            FormatearActivacionOferta(salida);

            return salida.OrderByDescending(x => x.Smapacfeccreacion).ToList();
        }

        /// <summary>
        /// Da formato a los registros de Activacion Oferta
        /// </summary>
        /// <param name="lista"></param>
        public void FormatearActivacionOferta(List<SmaActivacionOfertaDTO> lista)
        {
            foreach (var reg in lista)
            {
                reg.SmapacfeccreacionDesc = reg.Smapacfeccreacion != null ? reg.Smapacfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                reg.SmapacfechaDesc = reg.Smapacfecha != null ? reg.Smapacfecha.Value.ToString(ConstantesAppServicio.FormatoFecha) : "";
            }
        }

        /// <summary>
        /// Obtener datos de la activacion para cierta fecha oferta
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <param name="smapaccodi"></param>
        /// <returns></returns>
        public DatoActivacionOferta ObtenerDatosActivacionOferta(DateTime fechaDeOferta, int? smapaccodi)
        {
            DatoActivacionOferta salida = new DatoActivacionOferta();


            if (smapaccodi != null) //abre uno del historial de activaciones
            {
                SmaActivacionOfertaDTO regActivacionOferta = FactorySic.GetSmaActivacionOfertaRepository().GetById(smapaccodi.Value);
                ArmarDatosActivacionOfertaDefecto(salida, regActivacionOferta);
            }
            else //hace consulta desde boton CONSULTAR (muestra el ultimo)
            {
                List<SmaActivacionOfertaDTO> listaActivacionesActivasExistentes = FactorySic.GetSmaActivacionOfertaRepository().ListarActivacionesPorRangoFechas(fechaDeOferta, fechaDeOferta);
                SmaActivacionOfertaDTO regActivacionOferta = listaActivacionesActivasExistentes.Any() ? listaActivacionesActivasExistentes.First() : null;

                if (regActivacionOferta != null)
                {
                    ArmarDatosActivacionOfertaDefecto(salida, regActivacionOferta);
                }
                else
                {
                    salida.ExisteActivacionOferta = false;
                }


            }

            return salida;
        }

        /// <summary>
        /// Arma los datos de la activacion
        /// </summary>
        /// <param name="salida"></param>
        /// <param name="regActivacionOferta"></param>
        public void ArmarDatosActivacionOfertaDefecto(DatoActivacionOferta salida, SmaActivacionOfertaDTO regActivacionOferta)
        {
            salida.ExisteActivacionOferta = true;

            //datos handson
            List<SmaActivacionDataDTO> lstDatos = FactorySic.GetSmaActivacionDataRepository().ObtenerPorActivacionesOferta(regActivacionOferta.Smapaccodi.ToString());
            salida.DatosDeficitSubir = lstDatos.Find(x => x.Smaacdtipodato == ConstantesSubasta.DatoDeficitRSF && x.Smaacdtiporeserva == ConstantesSubasta.ReservaSubir);
            salida.DatosDeficitBajar = lstDatos.Find(x => x.Smaacdtipodato == ConstantesSubasta.DatoDeficitRSF && x.Smaacdtiporeserva == ConstantesSubasta.ReservaBajar);
            salida.DatosReduccionBandaSubir = lstDatos.Find(x => x.Smaacdtipodato == ConstantesSubasta.DatoReduccionBanda && x.Smaacdtiporeserva == ConstantesSubasta.ReservaSubir);
            salida.DatosReduccionBandaBajar = lstDatos.Find(x => x.Smaacdtipodato == ConstantesSubasta.DatoReduccionBanda && x.Smaacdtiporeserva == ConstantesSubasta.ReservaBajar);

            //motivos
            List<SmaActivacionMotivoDTO> lstMotivos = FactorySic.GetSmaActivacionMotivoRepository().ObtenerPorActivacionesOferta(regActivacionOferta.Smapaccodi.ToString());
            salida.IdsMotivosSubir = lstMotivos.Where(x => x.Smaacmtiporeserva == ConstantesSubasta.ReservaSubir).Select(x => x.Smammcodi.Value).ToList();
            salida.IdsMotivosBajar = lstMotivos.Where(x => x.Smaacmtiporeserva == ConstantesSubasta.ReservaBajar).Select(x => x.Smammcodi.Value).ToList();
        }

        /// <summary>
        ///  Guarda los datos y realiza el proceso de activacion
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <param name="datosAGuardar"></param>
        /// <param name="usuario"></param>
        /// <param name="usercode"></param>
        public int GuardarActivacionOferta(DateTime fechaDeOferta, DatoActivacionOferta datosAGuardar, string usuario, int usercode, out string StrLstURSSinActivacion)
        {
            int salida = 2; //solo guarda
            StrLstURSSinActivacion = "";
            List<SmaActivacionDataDTO> listaDatos = new List<SmaActivacionDataDTO>();
            SmaOfertaDTO regOfertaActivacion = null;

            DatoActivacionOferta datosDeficitYReduccionAGuardar = FormatearInformacionDataActivacion(datosAGuardar, usuario);
            List<SmaActivacionMotivoDTO> lstMotivosXActivacion = GenerarRelacionesActivacionMotivo(datosAGuardar, usuario);

            //nuevo Año Operativo
            SmaActivacionOfertaDTO activacionOferta = new SmaActivacionOfertaDTO();
            activacionOferta.Smapacfecha = fechaDeOferta;
            activacionOferta.Smapacestado = ConstantesSubasta.EstadoActivo;
            activacionOferta.Smapacusucreacion = usuario;
            activacionOferta.Smapacfeccreacion = DateTime.Now;

            listaDatos.Add(datosDeficitYReduccionAGuardar.DatosDeficitSubir);
            listaDatos.Add(datosDeficitYReduccionAGuardar.DatosDeficitBajar);
            listaDatos.Add(datosDeficitYReduccionAGuardar.DatosReduccionBandaSubir);
            listaDatos.Add(datosDeficitYReduccionAGuardar.DatosReduccionBandaBajar);

            activacionOferta.ListaDatosXActivacion = listaDatos;
            activacionOferta.ListaMotivosXActivacion = lstMotivosXActivacion;

            //Si no existen valores de Deficit de RSF, no realiza Activacion de ofertas
            bool HayValoresDeficitSubir = ExistenValoresPositivosEnInsumoActivacion(datosDeficitYReduccionAGuardar.DatosDeficitSubir);
            bool HayValoresDeficitBajar = ExistenValoresPositivosEnInsumoActivacion(datosDeficitYReduccionAGuardar.DatosDeficitBajar);

            if (HayValoresDeficitSubir || HayValoresDeficitBajar)
            {
                //Si existe algun dato de Deficit de RSF entones realiza tambien la activacion
                salida = 1; //activa y guarda
                #region Proceso de Activacion
                List<SmaUrsModoOperacionDTO> listaURSCalificadas = ObtenerDatosURSCalificadas(fechaDeOferta);
                List<SmaUrsModoOperacionDTO> listaURSCalificadasVigentes = listaURSCalificadas.Any() ? listaURSCalificadas.Where(x => x.Estado == ConstantesSubasta.EstadoVigenteDesc).ToList() : new List<SmaUrsModoOperacionDTO>();

                //Obtengo los rangos candidatos para activar ofertas
                List<RangoEscenario> lstRangosEscenario = ObtenerDatosEscenario();
                List<RangoActivacionOfertaDefecto> listaRangosActivarOfertaSubirGeneral = ObtenerRangoActivacionDeOfertaPorDefecto(lstRangosEscenario, datosDeficitYReduccionAGuardar.DatosDeficitSubir);
                List<RangoActivacionOfertaDefecto> listaRangosActivarOfertaBajarGeneral = ObtenerRangoActivacionDeOfertaPorDefecto(lstRangosEscenario, datosDeficitYReduccionAGuardar.DatosDeficitBajar);

                List<RangoActivacionPorUrs> rangosRealActivacionSubir = new List<RangoActivacionPorUrs>();
                List<RangoActivacionPorUrs> rangosRealActivacionBajar = new List<RangoActivacionPorUrs>();
                List<int> lstUrsDefecto = ObtenerListadoDeURSPorDefecto(fechaDeOferta, listaURSCalificadasVigentes, listaRangosActivarOfertaSubirGeneral, listaRangosActivarOfertaBajarGeneral, out rangosRealActivacionSubir, out rangosRealActivacionBajar, out List<SmaUrsModoOperacionDTO> lstURSSinActivacion);
                StrLstURSSinActivacion = lstURSSinActivacion.Any() ? string.Join(", ", lstURSSinActivacion.Where(x => x.Ursnomb != null).Select(x => x.Ursnomb.Trim()).ToList()) : "";
                if (!lstUrsDefecto.Any()) //Hasta ahora tenemos URS por defecto a nivel general, falta hacer un analisis por escenario
                    throw new ArgumentException("No se encuentran URS Disponibles para activar sus ofertas por defecto.");

                #region Insumos para la activacion de las "urs por defecto"

                //Obtengo parametros para la fecha de oferta
                PrGrupodatDTO paramPotMinAuto = ObtenerParametroPotenciaUrsMinAuto(fechaDeOferta); //item.Formuladat

                //Obtengo datos de oferta por defecto (5 anios atras)
                List<SmaUrsModoOperacionDTO> lstUrsEnVentanas = this.ListSmaUrsModoOperacions_Urs(-1);
                DateTime fechaAnterior = fechaDeOferta.AddYears(-5);
                List<SmaOfertaDTO> listaOfertasDefecto = ObtenerOfertasDefecto(fechaAnterior, fechaDeOferta, lstUrsEnVentanas);

                //Obtengo datos de oferta Diaria
                List<SmaOfertaDTO> listaOfertasDiaria = ObtenerOfertasDiaria(fechaDeOferta, fechaDeOferta, lstUrsEnVentanas);

                //Obtengo datos banda disponible
                List<SmaIndisponibilidadTempDetDTO> lstIndisponibilidad = ObtenerDatosIndisponibilidadTemporal(fechaDeOferta, out bool muestraTablaEnWeb); //SOS Copletar cuando exista indisponibilidad

                ////Obtengo insumos por urs            
                List<InsumosActivacionPorEscenario> lstDatosInsumosSubirPorEscenario = rangosRealActivacionSubir.Any() ?
                    ObtenerInsumosPorEscenario(ConstantesSubasta.TipoCargaSubir, fechaDeOferta, rangosRealActivacionSubir, paramPotMinAuto, listaOfertasDefecto, listaOfertasDiaria, lstIndisponibilidad) : new List<InsumosActivacionPorEscenario>();
                List<InsumosActivacionPorEscenario> lstDatosInsumosBajarPorEscenario = rangosRealActivacionBajar.Any() ?
                    ObtenerInsumosPorEscenario(ConstantesSubasta.TipoCargaBajar, fechaDeOferta, rangosRealActivacionBajar, paramPotMinAuto, listaOfertasDefecto, listaOfertasDiaria, lstIndisponibilidad) : new List<InsumosActivacionPorEscenario>();

                //Hallo las ofertas detalles final a guardar          
                List<SmaOfertaDetalleDTO> lstFinalActivacionSubir = lstDatosInsumosSubirPorEscenario.Any() ? ObtenerRegistrosActivacionFinal(ConstantesSubasta.TipoCargaSubir, lstDatosInsumosSubirPorEscenario, listaURSCalificadasVigentes, usuario) : new List<SmaOfertaDetalleDTO>();
                List<SmaOfertaDetalleDTO> lstFinalActivacionBajar = lstDatosInsumosBajarPorEscenario.Any() ? ObtenerRegistrosActivacionFinal(ConstantesSubasta.TipoCargaBajar, lstDatosInsumosBajarPorEscenario, listaURSCalificadasVigentes, usuario) : new List<SmaOfertaDetalleDTO>();

                List<SmaOfertaDetalleDTO> listaDetalleActivacion = new List<SmaOfertaDetalleDTO>();
                listaDetalleActivacion.AddRange(lstFinalActivacionSubir);
                listaDetalleActivacion.AddRange(lstFinalActivacionBajar);

                if (!listaDetalleActivacion.Any()) //Ahora ya tenemos listado de URS con un analisis por cada escenario
                    throw new ArgumentException("No se encuentran URS Disponibles para activar sus ofertas por defecto.");

                regOfertaActivacion = new SmaOfertaDTO();
                if (listaDetalleActivacion.Any())
                {
                    regOfertaActivacion.Ofertipo = ConstantesSubasta.OfertipoDiaria;
                    regOfertaActivacion.Oferfechainicio = fechaDeOferta.Date;
                    regOfertaActivacion.Oferfechafin = fechaDeOferta.Date;
                    regOfertaActivacion.Ofercodenvio = "";
                    regOfertaActivacion.Oferestado = ConstantesSubasta.EstadoActivo;
                    regOfertaActivacion.Oferusucreacion = usuario;
                    regOfertaActivacion.Oferfeccreacion = DateTime.Now;
                    ////regOfertaActivacion.Oferusumodificacion { get; set; } 
                    ////regOfertaActivacion.Oferfecmodificacion { get; set; } 
                    regOfertaActivacion.Oferfechaenvio = DateTime.Now;
                    //regOfertaActivacion.Ofercodi { get; set; } 
                    regOfertaActivacion.Usercode = usercode;
                    regOfertaActivacion.Oferfuente = ConstantesSubasta.FuenteActivacion;
                    //regOfertaActivacion.Smapaccodi { get; set; }

                    regOfertaActivacion.ListaDetalles = listaDetalleActivacion;
                }
                else
                {
                    throw new ArgumentException("No se encontró escenarios horario a activar.");
                }

                #endregion

                #endregion

            }

            //Guardar Nuevo
            this.GuardarActivacionOfertaTransaccional(activacionOferta, regOfertaActivacion, usuario);

            return salida;
        }

        /// <summary>
        /// Verifica si existen datos mayores a cero
        /// </summary>
        /// <param name="regDatoActivar"></param>
        /// <returns></returns>
        public bool ExistenValoresPositivosEnInsumoActivacion(SmaActivacionDataDTO regDatoActivar)
        {
            bool salida = false;

            decimal suma = 0;

            for (int i = 1; i <= 48; i++)
            {
                decimal deficitEscenario = ((decimal?)regDatoActivar.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regDatoActivar, null)).GetValueOrDefault(0);
                suma = suma + deficitEscenario;
            }

            if (suma > 0)
            {
                salida = true;
            }

            return salida;
        }

        /// <summary>
        /// Devuelve los registros de oferta detalle finales de las activaciones
        /// </summary>
        /// <param name="tipoCarga"></param>
        /// <param name="lstDatosInsumosPorEscenario"></param>
        /// <param name="listaURSCalificadasVig"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<SmaOfertaDetalleDTO> ObtenerRegistrosActivacionFinal(int tipoCarga, List<InsumosActivacionPorEscenario> lstDatosInsumosPorEscenario, List<SmaUrsModoOperacionDTO> listaURSCalificadasVig, string usuario)
        {
            List<SmaOfertaDetalleDTO> lstSalida = new List<SmaOfertaDetalleDTO>();
            decimal? valNulo = null;
            List<RangoEscenario> lstRangosEscenario = ObtenerDatosEscenario();
            List<int> listaUrsPresentes = lstDatosInsumosPorEscenario.Select(x => x.Urscodi).Distinct().ToList();

            foreach (int urscodi in listaUrsPresentes)
            {
                //Obtengo la relacion Oferta Diaria y Modo de Operacion (OD MO) para cada URS
                SmaUrsModoOperacionDTO urs_ = listaURSCalificadasVig.Find(x => x.Urscodi == urscodi);
                List<int> lstGrupodisPorUrs = listaURSCalificadasVig.Where(x => x.Urscodi == urscodi && x.Grupocodi != null).Select(x => x.Grupocodi.Value).Distinct().ToList();
                List<SmaRelacionOdMoDTO> lstRelacionPorUrs = new List<SmaRelacionOdMoDTO>();
                foreach (int grupocodi in lstGrupodisPorUrs)
                {
                    SmaRelacionOdMoDTO regRelacion = new SmaRelacionOdMoDTO();
                    regRelacion.Grupocodi = grupocodi;
                    regRelacion.Odmoestado = ConstantesSubasta.EstadoActivo;
                    regRelacion.Odmousucreacion = usuario;
                    regRelacion.Odmofeccreacion = DateTime.Now;
                    regRelacion.Odmobndcalificada = urs_ != null ? urs_.BandaURS != null ? urs_.BandaURS.Value : 0 : 0;
                    //regRelacion.Ofdecodi { get; set; }
                    lstRelacionPorUrs.Add(regRelacion);
                }

                //Ahora obtengo los detalles por URS
                List<InsumosActivacionPorEscenario> lstInsumosXUrs = lstDatosInsumosPorEscenario.Where(x => x.Urscodi == urscodi).OrderBy(x => x.Escenario).ToList();
                if (lstInsumosXUrs.Any())
                {
                    //Obtengo los rangos donde se debera activar
                    List<RangoActivacionOfertaDefecto> lstRangosPorUrs = ObtenerRangosFinalesActivacion(lstInsumosXUrs, lstRangosEscenario);

                    foreach (RangoActivacionOfertaDefecto rangoActivar in lstRangosPorUrs)
                    {
                        int escIni = lstRangosEscenario.Find(x => x.HoraIni == rangoActivar.HoraIni.Trim()).Escenario;
                        InsumosActivacionPorEscenario insumoEscIni = lstInsumosXUrs.Find(x => x.Escenario == escIni);

                        SmaOfertaDetalleDTO regG = new SmaOfertaDetalleDTO();
                        regG.Urscodi = urscodi;
                        regG.Ursnomb = insumoEscIni.Ursnomb;
                        regG.Ofdehorainicio = rangoActivar.HoraIni;
                        regG.Ofdehorafin = rangoActivar.HoraFin;
                        regG.Ofdeprecio = insumoEscIni.Precio;
                        regG.Ofdedusucreacion = usuario;
                        regG.Ofdefeccreacion = DateTime.Now;
                        regG.Ofdemoneda = "604"; //SOS
                        regG.Ofdeusumodificacion = usuario;
                        regG.Ofdefecmodificacion = DateTime.Now;
                        //regG.Ofdecodi { get; set; } 
                        //regG.Ofercodi { get; set; }
                        regG.Ofdepotofertada = insumoEscIni.PotenciaOfertada != null ? insumoEscIni.PotenciaOfertada.Value : -1;
                        regG.Ofdetipo = tipoCarga;
                        regG.RelacionesODMO = lstRelacionPorUrs;

                        lstSalida.Add(regG);
                    }
                }
            }


            return lstSalida;
        }

        /// <summary>
        /// Devuelve el grupo de rangos finales a activar
        /// </summary>
        /// <param name="lstInsumosXUrs"></param>
        /// <param name="lstRangosEscenario"></param>
        /// <returns></returns>
        public List<RangoActivacionOfertaDefecto> ObtenerRangosFinalesActivacion(List<InsumosActivacionPorEscenario> lstInsumosXUrs, List<RangoEscenario> lstRangosEscenario)
        {
            List<RangoActivacionOfertaDefecto> lstRangosPorUrs = new List<RangoActivacionOfertaDefecto>();
            RangoActivacionOfertaDefecto rango = new RangoActivacionOfertaDefecto();
            decimal? POfAnterior = -1;
            int escAnterior = -1;
            bool rangoAbierto = false;
            int numRegTotal = lstInsumosXUrs.Count();
            int numReg = 0;
            //para cada escenario a activar
            foreach (InsumosActivacionPorEscenario insumoActual in lstInsumosXUrs)
            {
                numReg++;
                if (insumoActual.DebeActivarseOferta)
                {
                    bool esUltimoRegistro = numReg == numRegTotal ? true : false;

                    int escActual = insumoActual.Escenario;
                    decimal? POfActual = insumoActual.PotenciaOfertada;

                    bool hayHuecoEntreEscenarios = escActual - escAnterior > 1 ? true : false;
                    bool POfertadaCambio = POfAnterior == POfActual ? false : true;

                    if (hayHuecoEntreEscenarios || POfertadaCambio)
                    {
                        if (rangoAbierto)
                        {
                            lstRangosPorUrs.Add(rango);
                        }

                        rangoAbierto = true;
                        rango = new RangoActivacionOfertaDefecto();
                        rango.EscenarioIni = escActual;
                        rango.EscenarioFin = escActual;

                        if (esUltimoRegistro)
                        {
                            lstRangosPorUrs.Add(rango);
                        }

                    }
                    else
                    {
                        rango.EscenarioFin = escActual;

                        if (esUltimoRegistro)
                        {
                            lstRangosPorUrs.Add(rango);
                        }
                    }

                    escAnterior = escActual;
                    POfAnterior = POfActual;
                }
            }

            //Completo datos de los rangos (horaIni y horaFin)
            foreach (RangoActivacionOfertaDefecto rangee in lstRangosPorUrs)
            {
                int escIni = rangee.EscenarioIni;
                RangoEscenario rangoI = lstRangosEscenario.Find(x => x.Escenario == escIni);
                rangee.HoraIni = rangoI.HoraIni;

                int escFin = rangee.EscenarioFin;
                RangoEscenario rangoF = lstRangosEscenario.Find(x => x.Escenario == escFin);
                rangee.HoraFin = rangoF.HoraFin;
            }

            return lstRangosPorUrs;
        }


        /// <summary>
        /// Devuelve los insumos por cada escenario horario para los rangos candidatos a activarse
        /// </summary>
        /// <param name="tipoCarga"></param>
        /// <param name="fechaDeOferta"></param>
        /// <param name="lstRangosCandidatasAActivar"></param>
        /// <param name="paramPotMinAuto"></param>
        /// <param name="listaOfertasDefecto"></param>
        /// <param name="listaOfertasDiaria"></param>
        /// <param name="lstIndisponibilidad"></param>
        /// <returns></returns>
        private List<InsumosActivacionPorEscenario> ObtenerInsumosPorEscenario(int tipoCarga, DateTime fechaDeOferta, List<RangoActivacionPorUrs> lstRangosCandidatasAActivar, PrGrupodatDTO paramPotMinAuto, List<SmaOfertaDTO> listaOfertasDefectoCincoAnios, List<SmaOfertaDTO> listaOfertasDiaria, List<SmaIndisponibilidadTempDetDTO> lstIndisponibilidad)
        {
            //obtengo las ofertas por tipo carga (subir y bajar)
            List<SmaOfertaDTO> listaOfertasDefectoPorTipo = listaOfertasDefectoCincoAnios.Where(x => x.Ofdetipo == tipoCarga).ToList();
            List<SmaOfertaDTO> listaOfertasDiariaPorTipo = listaOfertasDiaria.Where(x => x.Ofdetipo == tipoCarga).ToList();

            //Obtengo Oferta diaria de la otra pestaña segun el valor de "tipoCarga" (servira para hallar TieneOfertaDiariaSubir y TieneOfertaDiariaBajar)
            int tipoCargaOtraPestania = -1;
            if (tipoCarga == ConstantesSubasta.TipoCargaSubir) tipoCargaOtraPestania = ConstantesSubasta.TipoCargaBajar;
            if (tipoCarga == ConstantesSubasta.TipoCargaBajar) tipoCargaOtraPestania = ConstantesSubasta.TipoCargaSubir;
            List<SmaOfertaDTO> listaOfertasDiariaOtroTipo = listaOfertasDiaria.Where(x => x.Ofdetipo == tipoCargaOtraPestania).ToList();

            List<int> listaUrsPresentes = lstRangosCandidatasAActivar.Select(x => x.Urscodi).Distinct().ToList();
            decimal? valNulo = null;
            List<InsumosActivacionPorEscenario> lstSalida = new List<InsumosActivacionPorEscenario>();

            //Precio máximo Osinergmin
            decimal paramPrecioMaximo = -1;
            PrGrupodatDTO datoConf = this.GetValorConfiguracion(fechaDeOferta, ConstantesSubasta.PrecioMaximo);
            if (datoConf != null) paramPrecioMaximo = Convert.ToDecimal(datoConf.Formuladat);

            foreach (int urscodi in listaUrsPresentes)
            {
                RangoActivacionPorUrs datosRangoPorUrs = lstRangosCandidatasAActivar.Find(x => x.Urscodi == urscodi);

                //Obtengo Of.Defecto por urs, deben tener BandaCalificada y debe estar ordenado desde lo mas actual al mas antiguo
                List<SmaOfertaDTO> lstOfDefectoUrs = listaOfertasDefectoPorTipo.Where(x => x.Urscodi == urscodi && x.BandaCalificada != null).OrderByDescending(x => x.Oferfechainicio).ToList();

                //Obtengo Of.Defecto por urs, deben tener PRECIO y debe estar ordenado desde lo mas actual al mas antiguo
                List<SmaOfertaDTO> lstOfDefectoUrsPrecio = listaOfertasDefectoPorTipo.Where(x => x.Urscodi == urscodi && x.Repoprecio != null).OrderByDescending(x => x.Oferfechainicio).ToList();

                //obtengo la of. Diaria por urs para el "tipoCarga"
                List<SmaOfertaDTO> lstOfDiariaUrs = listaOfertasDiariaPorTipo.Where(x => x.Urscodi == urscodi).ToList();

                //obtengo la of. Diaria por urs para el otro "tipoCarga"
                List<SmaOfertaDTO> lstOfDiariaOtroTipoUrs = listaOfertasDiariaOtroTipo.Where(x => x.Urscodi == urscodi).ToList();

                //Obtengo la Indis Temporal para la URS
                List<SmaIndisponibilidadTempDetDTO> lstindisponibilidadUrs = lstIndisponibilidad.Where(x => x.Urscodi == urscodi).ToList();
                SmaIndisponibilidadTempDetDTO indisponibilidadUrs = lstIndisponibilidad.Find(x => x.Urscodi == urscodi);

                //Obtengo los rangos donde posiblemente se deba activar
                List<RangoActivacionOfertaDefecto> ListaRangosPosibleActivacionPorUrs = datosRangoPorUrs.ListaRangosActivacion;

                foreach (RangoActivacionOfertaDefecto rangoCandidato in ListaRangosPosibleActivacionPorUrs)
                {
                    int escIni = rangoCandidato.EscenarioIni;
                    int escFin = rangoCandidato.EscenarioFin;

                    for (int i = escIni; i <= escFin; i++)
                    {
                        string mprecio = lstOfDefectoUrsPrecio.Any() ? lstOfDefectoUrsPrecio.First().Repoprecio : "";

                        //Primero valido la desencriptacion de precio (para activar los precios deben estar desencriptados)
                        if (!string.IsNullOrEmpty(mprecio))
                        {
                            if (!this.AnalizarNumerico(mprecio))
                            {
                                throw new ArgumentException("No es posible activar, debe desencriptar la información antes.");
                            }
                        }

                        //Busco el menor precio entre la ultima de la oferta por defecto (5 anios) y lo reportado en parametros
                        string precioF = DevolverMenorValor(mprecio, paramPrecioMaximo.ToString(), out int flag);

                        InsumosActivacionPorEscenario insumoPorEscenario = new InsumosActivacionPorEscenario();
                        insumoPorEscenario.Urscodi = urscodi;
                        insumoPorEscenario.Ursnomb = datosRangoPorUrs.Ursnombre;
                        insumoPorEscenario.Escenario = i;
                        //string precioReporte = !string.IsNullOrEmpty(item.Repoprecio) ? (this.AnalizarNumerico(item.Repoprecio) ? item.Repoprecio : "NO DISPONIBLE") : "NO INGRESADO";
                        //insumoPorEscenario.Precio = !string.IsNullOrEmpty(mprecio) ? (this.AnalizarNumerico(mprecio) ? mprecio : "NO DISPONIBLE") : "NO INGRESADO";
                        insumoPorEscenario.Precio = !string.IsNullOrEmpty(precioF) ? (precioF) : "NO INGRESADO";
                        //insumoPorEscenario.Precio = lstOfDefectoUrsPrecio.Any() ? lstOfDefectoUrsPrecio.First().Repoprecio != "" ? Convert.ToDecimal(lstOfDefectoUrsPrecio.First().Repoprecio) : valNulo : valNulo; 
                        insumoPorEscenario.ParamPotMinAuto = paramPotMinAuto.Formuladat != "" ? Convert.ToDecimal(paramPotMinAuto.Formuladat) : valNulo;
                        insumoPorEscenario.PotOfertaDefecto = lstOfDefectoUrs.Any() ? lstOfDefectoUrs.First().BandaCalificada : valNulo;
                        insumoPorEscenario.PotOfertaDiaria = valNulo; //como es un rango candidato, se entiende que este escenario  no tiene oferta diaria
                        insumoPorEscenario.PotOfertaDiariaOtroTipoCarga = ObtenerPotenciaOfertadaDiariaPorEscenario(lstOfDiariaOtroTipoUrs, urscodi, i);
                        insumoPorEscenario.BandaDisponible = indisponibilidadUrs != null ? (indisponibilidadUrs.Intdetbanda != null ? indisponibilidadUrs.Intdetbanda : indisponibilidadUrs.BandaUrsCalificada) : valNulo;
                        insumoPorEscenario.PotActivada = insumoPorEscenario.PotOfertaDefecto != null && insumoPorEscenario.BandaDisponible != null ? (insumoPorEscenario.PotOfertaDefecto < insumoPorEscenario.BandaDisponible ? insumoPorEscenario.PotOfertaDefecto : insumoPorEscenario.BandaDisponible) : valNulo;
                        insumoPorEscenario.TieneOfertaDiariaParaTipoCarga = false;//como es un rango candidato, se entiende que este escenario  no tiene oferta diaria
                        insumoPorEscenario.TieneOfertaDiariaParaOtroTipoCarga = insumoPorEscenario.PotOfertaDiariaOtroTipoCarga != null ? true : false;

                        //Ahora calculo la potencia ofertda por cada escenario
                        decimal? potenciaOfertadaXEscenario;
                        //Si no tiene oferta diaria para el otro tipo
                        if (insumoPorEscenario.TieneOfertaDiariaParaOtroTipoCarga == false)
                        {
                            potenciaOfertadaXEscenario = insumoPorEscenario.PotActivada;
                            insumoPorEscenario.DebeActivarseOferta = true;
                        }
                        else
                        {
                            potenciaOfertadaXEscenario = (insumoPorEscenario.PotActivada != null && insumoPorEscenario.PotOfertaDiariaOtroTipoCarga != null) ? (insumoPorEscenario.PotActivada - insumoPorEscenario.PotOfertaDiariaOtroTipoCarga) : (valNulo);

                            if (potenciaOfertadaXEscenario != null)
                            {
                                if (potenciaOfertadaXEscenario < insumoPorEscenario.ParamPotMinAuto)
                                {
                                    insumoPorEscenario.DebeActivarseOferta = false;
                                }
                                else
                                {
                                    insumoPorEscenario.DebeActivarseOferta = true;
                                }

                            }
                            else
                            {
                                insumoPorEscenario.DebeActivarseOferta = false;
                            }
                        }

                        insumoPorEscenario.PotenciaOfertada = potenciaOfertadaXEscenario;

                        lstSalida.Add(insumoPorEscenario);

                    }

                }
            }


            return lstSalida;
        }

        /// <summary>
        /// Pbtiene la potencia ofertada para cierto escenario de las ofertas diarias (se asume que si hay ofertas diarias para una misma urs (para X Modos de Operacion), todos tienen la misma potencia ofertada)
        /// </summary>
        /// <param name="listaOfertasDiariaMismoTipoCarga"></param>
        /// <param name="numEscenario"></param>
        /// <returns></returns>
        public decimal? ObtenerPotenciaOfertadaDiariaPorEscenario(List<SmaOfertaDTO> listaOfertasDiariaMismoTipoCarga, int urscodi, int numEscenario)
        {
            decimal? salida;
            decimal? valNulo = null;

            List<SmaOfertaDTO> listaOfertasDiariaPorUrs = listaOfertasDiariaMismoTipoCarga.Where(x => x.Urscodi == urscodi).ToList();
            List<RangoEscenario> lstRangosEscenario = ObtenerDatosEscenario();
            SmaOfertaDTO regBuscado = new SmaOfertaDTO();

            foreach (SmaOfertaDTO regPD in listaOfertasDiariaPorUrs)
            {
                string horaIni = regPD.Ofdehorainicio;
                string horaFin = regPD.Ofdehorafin;
                int escIni = lstRangosEscenario.Find(x => x.HoraIni == horaIni.Trim()).Escenario;
                int escFin = lstRangosEscenario.Find(x => x.HoraFin == horaFin.Trim()).Escenario;

                //Si el escenario buscado se encuentra en el rango, saco el objeto, y dejo de recorrer la lista
                if (escIni <= numEscenario && numEscenario <= escFin)
                {
                    regBuscado = regPD;
                    break;
                }
                else
                {
                    regBuscado = null;
                }
            }

            if (regBuscado != null)
            {
                salida = regBuscado.Repopotofer;
            }
            else
            {
                salida = valNulo;
            }

            return salida;
        }



        /// <summary>
        /// Devuelve listado de escenarios
        /// </summary>
        /// <returns></returns>
        public List<RangoEscenario> ObtenerDatosEscenario()
        {
            List<RangoEscenario> lstSalida = new List<RangoEscenario>();

            for (int i = 1; i <= 48; i++)
            {
                bool esPar = (i % 2) == 0 ? true : false;
                RangoEscenario escenario = new RangoEscenario();
                escenario.Escenario = i;
                escenario.HoraIni = esPar ? (string.Format("{0:D2}", (i / 2 - 1)) + ":30") : (string.Format("{0:D2}", (i / 2)) + ":00");
                escenario.HoraFin = i == 48 ? ("23:59") : (esPar ? (string.Format("{0:D2}", (i / 2)) + ":00") : (string.Format("{0:D2}", (i / 2)) + ":30"));

                lstSalida.Add(escenario);
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve los rangos candidatos para la activacion de oferta por defecto de forma general
        /// </summary>
        /// <param name="lstRangosEscenario"></param>
        /// <param name="DatosDeficit"></param>
        /// <returns></returns>
        public List<RangoActivacionOfertaDefecto> ObtenerRangoActivacionDeOfertaPorDefecto(List<RangoEscenario> lstRangosEscenario, SmaActivacionDataDTO DatosDeficit)
        {
            List<RangoActivacionOfertaDefecto> lstRangos = new List<RangoActivacionOfertaDefecto>();

            //Obtengo rangos (escenarios)
            RangoActivacionOfertaDefecto rango = new RangoActivacionOfertaDefecto();
            decimal anterior = 0;
            bool rangoAbierto = false;
            for (int i = 1; i <= 48; i++)
            {
                decimal deficitEscenario = ((decimal?)DatosDeficit.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(DatosDeficit, null)).GetValueOrDefault(0);
                if (deficitEscenario != 0)  //Solo si hay valor (mayor que cero) en la celda
                {
                    decimal sumaActual = deficitEscenario + anterior;

                    if (anterior == 0)
                    {
                        rangoAbierto = true;
                        rango = new RangoActivacionOfertaDefecto();
                        rango.EscenarioIni = i;
                    }

                    if (sumaActual > anterior)
                    {
                        rango.EscenarioFin = i;
                    }

                    anterior = sumaActual;

                    //si es el ultimo y tiene rango abierto, guardo en listado de rangos
                    if (i == 48)
                    {
                        if (rangoAbierto && rango.EscenarioIni != null && rango.EscenarioFin != null)
                        {
                            lstRangos.Add(rango);
                        }
                    }
                }
                else
                {
                    if (rangoAbierto && rango.EscenarioIni != null && rango.EscenarioFin != null)
                    {
                        lstRangos.Add(rango);
                        rangoAbierto = false;
                    }
                    anterior = 0;
                }


            }

            //Completo datos de los rangos (horaIni y horaFin)

            foreach (RangoActivacionOfertaDefecto rangee in lstRangos)
            {
                int escIni = rangee.EscenarioIni;
                RangoEscenario rangoI = lstRangosEscenario.Find(x => x.Escenario == escIni);
                rangee.HoraIni = rangoI.HoraIni;

                int escFin = rangee.EscenarioFin;
                RangoEscenario rangoF = lstRangosEscenario.Find(x => x.Escenario == escFin);
                rangee.HoraFin = rangoF.HoraFin;
            }

            return lstRangos.OrderBy(x => x.HoraIni).ToList();
        }


        /// <summary>
        /// Lista las "URS po Defecto" para la fecha de oferta. Debe cumplir 4 condiciones: 
        /// Condicion 1. Son URS que no ofertaron en el mercado de ajuste (analisis por pestaña y escenario)
        /// Condicion 2. Son URS vigentes
        /// Condicion 3. Son URS disponibles y
        /// Condicion 4. Son URS que cuenten con oferta por defecto
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <param name="listaRangosActivarOfertaSubirGeneral"></param>
        /// <param name="listaRangosActivarOfertaBajarGeneral"></param>
        /// <param name="rangosRealActivacionSubir"></param>
        /// <param name="rangosRealActivacionBajar"></param>
        /// <returns></returns>
        public List<int> ObtenerListadoDeURSPorDefecto(DateTime fechaDeOferta, List<SmaUrsModoOperacionDTO> listaURSCalificadasVigentes, List<RangoActivacionOfertaDefecto> listaRangosActivarOfertaSubirGeneral, List<RangoActivacionOfertaDefecto> listaRangosActivarOfertaBajarGeneral, out List<RangoActivacionPorUrs> rangosRealActivacionSubir, out List<RangoActivacionPorUrs> rangosRealActivacionBajar, out List<SmaUrsModoOperacionDTO> lstURSSinActivacion)
        {
            List<SmaUrsModoOperacionDTO> lstUrsEnVentanas = this.ListSmaUrsModoOperacions_Urs(-1);
            List<int> listaUrsPorDefecto = new List<int>();
            List<int> listaTemporal = new List<int>();


            List<SmaOfertaDTO> listaOfertasDiaria = ObtenerOfertasDiaria(fechaDeOferta, fechaDeOferta, lstUrsEnVentanas);
            string ofercodisPorOfertaDiaria = listaOfertasDiaria.Any() ? string.Join(",", listaOfertasDiaria.Select(x => x.Ofercodi).Distinct().ToList()) : "";
            List<SmaOfertaDetalleDTO> listaOfertaDetallePorOfertaDiaria = ofercodisPorOfertaDiaria != "" ? FactorySic.GetSmaOfertaDetalleRepository().ListarPorOfertas(ofercodisPorOfertaDiaria) : new List<SmaOfertaDetalleDTO>();

            //Condicion_1: URS con oferta diaria (Mercado de ajuste)
            List<int> listaUrsConOfertaDiariaPorFechaOferta = listaOfertasDiaria.Select(x => x.Urscodi).Distinct().ToList();
            List<SmaUrsModoOperacionDTO> listaUrsConOfertaDiariaPorFechaOferta_ = lstUrsEnVentanas.Where(x => listaUrsConOfertaDiariaPorFechaOferta.Contains(x.Urscodi)).Distinct().ToList(); //urs con nombres

            //Condicion_2: URS Vigentes
            List<SmaUrsModoOperacionDTO> listaURSCalificadasVigentesFinal = listaURSCalificadasVigentes.Where(x => x.Estado == ConstantesSubasta.EstadoVigenteDesc).ToList();
            List<int> listaUrsVigentePorFechaOferta = listaURSCalificadasVigentesFinal.Select(x => x.Urscodi).Distinct().ToList();
            List<SmaUrsModoOperacionDTO> listaUrsVigentePorFechaOferta_ = lstUrsEnVentanas.Where(x => listaUrsVigentePorFechaOferta.Contains(x.Urscodi)).Distinct().ToList(); //urs con nombres

            //Condicion_3: URS Disponibles
            //List<int> listaUrsDisponibles = listaUrsVigentePorFechaOferta; //Falta editar (SOS)
            List<SmaIndisponibilidadTempDetDTO> listaUrsDisponiilidad = ObtenerUrsDisponibles(fechaDeOferta);
            List<int> listaUrsDisponibles = listaUrsDisponiilidad.Any() ? listaUrsDisponiilidad.Select(x => x.Urscodi.Value).Distinct().ToList() : new List<int>();
            //List<SmaUrsModoOperacionDTO> listaUrsDisponibles_ = lstUrsEnVentanas.Where(x => listaUrsDisponibles.Contains(x.Urscodi)).Distinct().ToList(); //urs con nombres

            //Condicion_4: URS con Oferta por Defecto
            List<SmaOfertaDTO> listaUrsConOfertaDefectoPorPeriodoDeCincoAnios = ObtenerOfertasPorDefectoPorPeriodoCincoAnios(listaURSCalificadasVigentesFinal, fechaDeOferta, ConstantesSubasta.CasoActivacionOfertaDefecto);
            List<int> listaUrscodisConOfertaDefectoPorPeriodoDeCincoAnios = listaUrsConOfertaDefectoPorPeriodoDeCincoAnios.Any() ? listaUrsConOfertaDefectoPorPeriodoDeCincoAnios.Select(x => x.Urscodi).Distinct().ToList() : new List<int>();

            //Obtengo las URS con 3 condiciones, que serian candidatas para la activacion, para luego contrastar con la condicion 1
            List<int> listaTemporalURSCandidatas = listaUrsVigentePorFechaOferta.Intersect(listaUrsDisponibles).Intersect(listaUrscodisConOfertaDefectoPorPeriodoDeCincoAnios).Distinct().ToList();
            List<SmaUrsModoOperacionDTO> listaTemporalURSCandidatas_ = lstUrsEnVentanas.Where(x => listaTemporalURSCandidatas.Contains(x.Urscodi)).Distinct().ToList(); //urs con nombres

            //Para las URS Candidatas, obtengo los rangos donde no tienen oferta diaria (tanto para SUBIR como para BAJAR)
            List<RangoActivacionPorUrs> listaRangosRealActivacionSubir = ObtenerRangoRealDeActivacionPorUrsCandidata(ConstantesSubasta.TipoCargaSubir, listaOfertasDiaria, listaOfertaDetallePorOfertaDiaria, listaTemporalURSCandidatas_, listaRangosActivarOfertaSubirGeneral);
            List<RangoActivacionPorUrs> listaRangosRealActivacionBajar = ObtenerRangoRealDeActivacionPorUrsCandidata(ConstantesSubasta.TipoCargaBajar, listaOfertasDiaria, listaOfertaDetallePorOfertaDiaria, listaTemporalURSCandidatas_, listaRangosActivarOfertaBajarGeneral);

            rangosRealActivacionSubir = listaRangosRealActivacionSubir.Any() ? listaRangosRealActivacionSubir : new List<RangoActivacionPorUrs>();
            rangosRealActivacionBajar = listaRangosRealActivacionBajar.Any() ? listaRangosRealActivacionBajar : new List<RangoActivacionPorUrs>();

            //por ultimo, calculo urs por defecto
            if (listaRangosRealActivacionSubir.Any())
                listaUrsPorDefecto.AddRange(listaRangosRealActivacionSubir.Select(x => x.Urscodi).Distinct().ToList());

            if (listaRangosRealActivacionBajar.Any())
                listaUrsPorDefecto.AddRange(listaRangosRealActivacionBajar.Select(x => x.Urscodi).Distinct().ToList());

            listaUrsPorDefecto = listaUrsPorDefecto.Distinct().ToList();

            //Por ultimo obtengo las URS que cumplen las 3 primeas condiciones (no tiene oferta diaria, disponible y calificada) pero no cumple la 4ta condicion es decir que no tiene oferta defecto en los ultimos 5 años
            List<SmaUrsModoOperacionDTO> lstUrsTresMenosCuatro = ObtenerListaUrsCumplenTresPrimerasCondicionesPeroNoCuartaCondicion(listaUrsVigentePorFechaOferta, listaUrsDisponibles, listaUrscodisConOfertaDefectoPorPeriodoDeCincoAnios, listaOfertasDiaria, listaOfertaDetallePorOfertaDiaria, listaUrsVigentePorFechaOferta_, listaRangosActivarOfertaSubirGeneral, listaRangosActivarOfertaBajarGeneral);
            lstURSSinActivacion = lstUrsTresMenosCuatro;

            return listaUrsPorDefecto;

        }

        /// <summary>
        /// Devuelve el grupo de URS que cumplen las 3 primeras condiciones pero que no cumple la 4ta condicion
        /// </summary>
        /// <param name="listaUrsVigentePorFechaOferta"></param>
        /// <param name="listaUrsDisponibles"></param>
        /// <param name="listaUrscodisConOfertaDefectoPorPeriodoDeCincoAnios"></param>
        /// <param name="listaOfertasDiaria"></param>
        /// <param name="listaOfertaDetallePorOfertaDiaria"></param>
        /// <param name="listaUrsVigentePorFechaOferta_"></param>
        /// <param name="listaRangosActivarOfertaSubirGeneral"></param>
        /// <param name="listaRangosActivarOfertaBajarGeneral"></param>
        /// <returns></returns>
        public List<SmaUrsModoOperacionDTO> ObtenerListaUrsCumplenTresPrimerasCondicionesPeroNoCuartaCondicion(List<int> listaUrsVigentePorFechaOferta, List<int> listaUrsDisponibles, List<int> listaUrscodisConOfertaDefectoPorPeriodoDeCincoAnios ,List<SmaOfertaDTO> listaOfertasDiaria, List<SmaOfertaDetalleDTO> listaOfertaDetallePorOfertaDiaria, List<SmaUrsModoOperacionDTO> listaUrsVigentePorFechaOferta_, List<RangoActivacionOfertaDefecto> listaRangosActivarOfertaSubirGeneral, List<RangoActivacionOfertaDefecto> listaRangosActivarOfertaBajarGeneral)
        {
            //Para las URS Candidatas, obtengo los rangos donde no tienen oferta diaria (tanto para SUBIR como para BAJAR)
            List<RangoActivacionPorUrs> listaRangosRealActivacionSubir = ObtenerRangoRealDeActivacionPorUrsCandidata(ConstantesSubasta.TipoCargaSubir, listaOfertasDiaria, listaOfertaDetallePorOfertaDiaria, listaUrsVigentePorFechaOferta_, listaRangosActivarOfertaSubirGeneral);
            List<RangoActivacionPorUrs> listaRangosRealActivacionBajar = ObtenerRangoRealDeActivacionPorUrsCandidata(ConstantesSubasta.TipoCargaBajar, listaOfertasDiaria, listaOfertaDetallePorOfertaDiaria, listaUrsVigentePorFechaOferta_, listaRangosActivarOfertaBajarGeneral);

            List<int> lstUrsActivarGeneralSubir = listaRangosRealActivacionSubir.Select(x => x.Urscodi).Distinct().ToList();
            List<int> lstUrsActivarGeneralBajar = listaRangosRealActivacionBajar.Select(x => x.Urscodi).Distinct().ToList();

            List<int> lstUrsPrimeraCondicion = new List<int>();
            lstUrsPrimeraCondicion.AddRange(lstUrsActivarGeneralSubir);
            lstUrsPrimeraCondicion.AddRange(lstUrsActivarGeneralBajar);

            lstUrsPrimeraCondicion = lstUrsPrimeraCondicion.Distinct().ToList();

            List<int> listaUrsTresPrimerasCondiciones = listaUrsVigentePorFechaOferta.Intersect(listaUrsDisponibles).Intersect(lstUrsPrimeraCondicion).Distinct().ToList();
            List<int> lstUrsSinOfertaDefectoCincoAnios = listaUrsVigentePorFechaOferta.Except(listaUrscodisConOfertaDefectoPorPeriodoDeCincoAnios).ToList();
            List<int> lstUrsTresCondMenosCuartaCondicion = lstUrsSinOfertaDefectoCincoAnios.Intersect(listaUrsTresPrimerasCondiciones).ToList();

            List<SmaUrsModoOperacionDTO> lstUrsSalida = listaUrsVigentePorFechaOferta_.Where(x => lstUrsTresCondMenosCuartaCondicion.Contains(x.Urscodi)).ToList();

            return lstUrsSalida;
        }
        

        /// <summary>
        /// Devuelve el listado de ofertas por defecto (cargada por agentes desde extranet) de hace 5 años para un grupo de urs
        /// </summary>
        /// <param name="listaURSCalificadasVigentesFinal"></param>
        /// <param name="fechaActual"></param>
        /// <param name="casoBusquedaOfertaDefecto"></param>
        /// <returns></returns>
        private List<SmaOfertaDTO> ObtenerOfertasPorDefectoPorPeriodoCincoAnios(List<SmaUrsModoOperacionDTO> listaURSCalificadasVigentesFinal, DateTime fechaActual, int casoBusquedaOfertaDefecto)
        {
            List<SmaOfertaDTO> lstSalida = new List<SmaOfertaDTO>();

            List<int> listaUrsVigentePorFechaOferta = listaURSCalificadasVigentesFinal.Select(x => x.Urscodi).Distinct().ToList();

            /*************/
            DateTime mesFechaDeOfertaFin = new DateTime();
            DateTime mesFechaDeOfertaIni = new DateTime();
            switch (casoBusquedaOfertaDefecto)
            {
                case ConstantesSubasta.CasoAutogeneracionOfertasDefectoMesEnero:
                    mesFechaDeOfertaFin = new DateTime(fechaActual.Year, 12, 1); //diciembre este año
                    mesFechaDeOfertaIni = mesFechaDeOfertaFin.AddYears(-5).AddMonths(1); //5 años atras
                    break;
                case ConstantesSubasta.CasoActivacionOfertaDefecto:
                    mesFechaDeOfertaFin = new DateTime(fechaActual.Year, fechaActual.Month, 1); //diciembre este año
                    mesFechaDeOfertaIni = mesFechaDeOfertaFin.AddYears(-5); //5 años atras
                    break;
            }
            

            //Busco las ofertas por defecto para esas URS y en ese periodo
            int opcionReporte = 1;
            int tipoOferta = 0;
            int userCode = -1;
            int oferCodi = -1;
            int empresaCodi = -1;
            //string listUrs = urscodi.ToString();
            string listUrs = String.Join(",", listaUrsVigentePorFechaOferta.ToList());
            string estadoEnvio = ConstantesSubasta.EstadoActivo;

            List<SmaOfertaDTO> listOfertasDefectoVigencia = this.ListSmaOfertasInterna(tipoOferta, mesFechaDeOfertaIni, mesFechaDeOfertaFin, userCode, oferCodi.ToString(), estadoEnvio, empresaCodi, listUrs, ConstantesSubasta.FuenteExtranet);

            lstSalida = listOfertasDefectoVigencia;
            /****************/            

            return lstSalida;
        }

        /// <summary>
        /// Devuelve detalles de las urs disponibles
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <returns></returns>
        public List<SmaIndisponibilidadTempDetDTO> ObtenerUrsDisponibles(DateTime fechaDeOferta)
        {
            List<SmaIndisponibilidadTempDetDTO> lstSalida = new List<SmaIndisponibilidadTempDetDTO>();
            List<SmaIndisponibilidadTempDetDTO> lstIndisponibilidad = ObtenerDatosIndisponibilidadTemporal(fechaDeOferta, out bool muestraTablaEnWeb); //SOS Copletar cuando exista indisponibilidad
            List<SmaIndisponibilidadTempDetDTO> lstDisponibies = lstIndisponibilidad.Where(x => x.Intdetindexiste == ConstantesSubasta.No).ToList();
            List<SmaIndisponibilidadTempDetDTO> lstDisponibiesParcial = lstIndisponibilidad.Where(x => x.Intdetindexiste == ConstantesSubasta.Si && x.Intdettipo == ConstantesSubasta.TipoParcial).ToList();

            lstSalida.AddRange(lstDisponibies);
            lstSalida.AddRange(lstDisponibiesParcial);

            return lstSalida;
        }

        /// <summary>
        /// Devuelve los rangos a activar por cada URS por tipo de reserva, para ello usa las URS Candidatas y busca los rangos de activacion donde no tienen oferta diaria (condicion 1 de las 4 condiciones)
        /// </summary>
        /// <param name="tipoReserva"></param>
        /// <param name="listaOfertasDiaria"></param>
        /// <param name="listaURSCandidatas"></param>
        /// <param name="listaRangosActivarGeneral"></param>
        /// <returns></returns>
        public List<RangoActivacionPorUrs> ObtenerRangoRealDeActivacionPorUrsCandidata(int tipoReserva, List<SmaOfertaDTO> listaOfertasDiaria, List<SmaOfertaDetalleDTO> listaOfertaDetallePorOfertaDiaria, List<SmaUrsModoOperacionDTO> listaURSCandidatas, List<RangoActivacionOfertaDefecto> listaRangosActivarGeneral)
        {
            List<RangoActivacionPorUrs> lstSalida = new List<RangoActivacionPorUrs>();

            List<SmaOfertaDetalleDTO> listaOfertasDetalleDiariaPorTipoReserva = listaOfertaDetallePorOfertaDiaria.Where(X => X.Ofdetipo == tipoReserva).ToList();

            foreach (SmaUrsModoOperacionDTO urs in listaURSCandidatas)
            {
                List<SmaOfertaDetalleDTO> listaOfertasDetalleDiariaPorTipoReservaAndURS = listaOfertasDetalleDiariaPorTipoReserva.Where(x => x.Urscodi == urs.Urscodi).ToList();
                List<RangoActivacionOfertaDefecto> lstRangosConOfertaDiariaPorUrs = FormatearRangoMismaURSyTipoOferta(listaOfertasDetalleDiariaPorTipoReservaAndURS);

                //ahora, por cada URS, se debe encontrar rangos sin oferta diaria pero que deben activarse 
                List<RangoActivacionOfertaDefecto> lstRangosSinOfertaDiariaParaActivacion = ContrastarRangosParaActivacion(listaRangosActivarGeneral, lstRangosConOfertaDiariaPorUrs);

                //solo agrego si existen rangos para activar
                if (lstRangosSinOfertaDiariaParaActivacion.Any())
                {
                    RangoActivacionPorUrs reg = new RangoActivacionPorUrs();
                    reg.Urscodi = urs.Urscodi;
                    reg.Ursnombre = urs.Ursnomb;
                    reg.TipoReserva = tipoReserva;
                    reg.ListaRangosActivacion = lstRangosSinOfertaDiariaParaActivacion;

                    lstSalida.Add(reg);
                }

            }

            return lstSalida;
        }

        /// <summary>
        /// Busca los rangos que no tienen oferta diaria pero que deben activarse, por URS
        /// </summary>
        /// <param name="listaRangosActivarGeneral"></param>
        /// <param name="lstRangosConOfertaDiariaPorUrs"></param>
        /// <returns></returns>
        public List<RangoActivacionOfertaDefecto> ContrastarRangosParaActivacion(List<RangoActivacionOfertaDefecto> listaRangosActivarGeneral, List<RangoActivacionOfertaDefecto> lstRangosConOfertaDiariaPorUrs)
        {
            List<RangoActivacionOfertaDefecto> lstSalida = new List<RangoActivacionOfertaDefecto>();
            SmaActivacionDataDTO general = new SmaActivacionDataDTO();
            SmaActivacionDataDTO conDiario = new SmaActivacionDataDTO();
            SmaActivacionDataDTO objTemp = new SmaActivacionDataDTO();
            decimal cero = 0;
            decimal uno = 1;
            decimal dos = 2;
            foreach (var item in listaRangosActivarGeneral)
            {
                int escIni = item.EscenarioIni;
                int escFin = item.EscenarioFin;

                for (int i = escIni; i <= escFin; i++)
                {
                    general.GetType().GetProperty("H" + i.ToString()).SetValue(general, uno);
                }
            }

            foreach (var item in lstRangosConOfertaDiariaPorUrs)
            {
                int escIni = item.EscenarioIni;
                int escFin = item.EscenarioFin;

                for (int i = escIni; i <= escFin; i++)
                {
                    conDiario.GetType().GetProperty("H" + i.ToString()).SetValue(conDiario, dos);
                }
            }

            for (int i = 1; i <= 48; i++)
            {
                decimal? valorGeneral = ((decimal?)general.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(general, null));
                decimal? valorConDiario = ((decimal?)conDiario.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(conDiario, null));

                bool estaEnGeneral = valorGeneral != null ? true : false;
                bool tieneOfertaDiaria = valorConDiario != null ? true : false;

                if (estaEnGeneral && !tieneOfertaDiaria)
                    objTemp.GetType().GetProperty("H" + i.ToString()).SetValue(objTemp, uno);
                else
                    objTemp.GetType().GetProperty("H" + i.ToString()).SetValue(objTemp, cero);
            }

            List<RangoEscenario> lstRangosEscenario = ObtenerDatosEscenario();
            lstSalida = ObtenerRangoActivacionDeOfertaPorDefecto(lstRangosEscenario, objTemp);

            return lstSalida;
        }

        /// <summary>
        /// Cambia de formato a los rangos que son de una misma URS y de un mismo tipo de oferta (subir o bajar)
        /// </summary>
        /// <param name="listaAFormatear"></param>
        /// <returns></returns>
        public List<RangoActivacionOfertaDefecto> FormatearRangoMismaURSyTipoOferta(List<SmaOfertaDetalleDTO> listaAFormatear)
        {
            List<RangoActivacionOfertaDefecto> lstSalida = new List<RangoActivacionOfertaDefecto>();
            List<RangoEscenario> lstRangosEscenario = ObtenerDatosEscenario();

            //primero busco rangos sin huecos en el listado a formatear
            List<SmaOfertaDetalleDTO> lstAFormatearSinHuecos = new List<SmaOfertaDetalleDTO>();
            SmaOfertaDetalleDTO regRango = new SmaOfertaDetalleDTO();
            int numEnListado = listaAFormatear.Count();
            int nreg = 0;
            List<SmaOfertaDetalleDTO> lstOrdenada = listaAFormatear.OrderBy(x => x.Ofdehorainicio).ToList();
            //foreach (SmaOfertaDetalleDTO ofertaDetActual in listaAFormatear.OrderBy(x => x.Ofdehorainicio).ToList())
            foreach (SmaOfertaDetalleDTO ofertaDetActual in lstOrdenada)
            {
                nreg++;

                if (nreg == 1)
                {
                    regRango = new SmaOfertaDetalleDTO();
                    regRango.Ofdehorainicio = ofertaDetActual.Ofdehorainicio;
                    regRango.Ofdehorafin = ofertaDetActual.Ofdehorafin;
                }

                if (nreg < numEnListado) //si hay mas registro por analizar
                {
                    SmaOfertaDetalleDTO ofertaDetSiguiente = lstOrdenada.ElementAt(nreg);
                    //SmaOfertaDetalleDTO ofertaDetSiguiente = listaAFormatear.ElementAt(nreg);

                    if (ofertaDetActual.Ofdehorafin != ofertaDetSiguiente.Ofdehorainicio)
                    {
                        lstAFormatearSinHuecos.Add(regRango);

                        regRango = new SmaOfertaDetalleDTO();
                        regRango.Ofdehorainicio = ofertaDetSiguiente.Ofdehorainicio;
                        regRango.Ofdehorafin = ofertaDetSiguiente.Ofdehorafin;
                    }
                    else
                    {
                        regRango.Ofdehorafin = ofertaDetSiguiente.Ofdehorafin;
                    }
                }
                else
                {
                    lstAFormatearSinHuecos.Add(regRango);
                }
            }

            //Ahora cambio de formato
            foreach (SmaOfertaDetalleDTO item in lstAFormatearSinHuecos)
            {
                string horaIni = item.Ofdehorainicio;
                string horaFin = item.Ofdehorafin;

                RangoActivacionOfertaDefecto reg = new RangoActivacionOfertaDefecto();
                reg.EscenarioIni = lstRangosEscenario.Find(x => x.HoraIni == horaIni).Escenario;
                reg.EscenarioFin = lstRangosEscenario.Find(x => x.HoraFin == horaFin).Escenario;
                reg.HoraIni = horaIni;
                reg.HoraFin = horaFin;

                lstSalida.Add(reg);
            }


            return lstSalida;
        }
        /// <summary>
        /// Devuelve el listado de las ofertas Defecto para cierta fecha
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <param name="lstUrsEnVentanaDefecto"></param>
        /// <returns></returns>
        public List<SmaOfertaDTO> ObtenerOfertasDefecto(DateTime fechaIni, DateTime fechaFin, List<SmaUrsModoOperacionDTO> lstUrsEnVentanaDefecto)
        {
            List<SmaOfertaDTO> lstSalida = new List<SmaOfertaDTO>();

            DateTime mesFechaDeOfertaIni = new DateTime(fechaIni.Year, fechaIni.Month, 1);
            DateTime mesFechaDeOfertaFin = new DateTime(fechaFin.Year, fechaFin.Month, 1);

            int opcionReporte = 1;
            int tipoOferta = 0;
            int userCode = -1;
            int oferCodi = -1;
            int empresaCodi = -1;
            string listUrs = lstUrsEnVentanaDefecto.Any() ? String.Join(",", lstUrsEnVentanaDefecto.Select(x => x.Urscodi).Distinct().ToList()) : "-1";
            lstSalida = this.ListaConsultaOferta(opcionReporte, tipoOferta, mesFechaDeOfertaIni, mesFechaDeOfertaFin, userCode, oferCodi.ToString(), empresaCodi, listUrs, ConstantesSubasta.FuenteExtranet);

            return lstSalida;
        }

        /// <summary>
        /// Devuelve el listado de las ofertas diarias para cierta fecha
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <param name="lstUrsEnVentanaDiaria"></param>
        /// <returns></returns>
        public List<SmaOfertaDTO> ObtenerOfertasDiaria(DateTime fechaIni, DateTime fechaFin, List<SmaUrsModoOperacionDTO> lstUrsEnVentanaDiaria)
        {
            List<SmaOfertaDTO> lstSalida = new List<SmaOfertaDTO>();

            List<SmaUrsModoOperacionDTO> lstUrsEnDiaria = this.ListSmaUrsModoOperacions_Urs(-1);
            int opcionReporte = 1;
            int tipoOferta = 1;
            int userCode = -1;
            int oferCodi = -1;
            int empresaCodi = -1;
            string listUrs = lstUrsEnVentanaDiaria.Any() ? String.Join(",", lstUrsEnVentanaDiaria.Select(x => x.Urscodi).Distinct().ToList()) : "-1";
            lstSalida = this.ListaConsultaOferta(opcionReporte, tipoOferta, fechaIni, fechaFin, userCode, oferCodi.ToString(), empresaCodi, listUrs, ConstantesSubasta.FuenteExtranet);

            return lstSalida;
        }
        /// <summary>
        /// Devuelve el listado con informacion necesaria de las URS Calificadas
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <returns></returns>
        public List<SmaUrsModoOperacionDTO> ObtenerDatosURSCalificadas(DateTime fechaDeOferta)
        {
            List<SmaUrsModoOperacionDTO> lstSalida = new List<SmaUrsModoOperacionDTO>();

            //Obtenemos todas las URS
            List<SmaUrsModoOperacionDTO> listaTodasURS = GetByCriteriaSmaUrsModoOperacions();
            List<SmaUrsModoOperacionDTO> listaURSDistinc = ListSmaUrsModoOperacions_Urs(-1);

            var lstGrupoCodiURS = listaTodasURS.Select(x => x.Urscodi).Distinct().ToList();
            var lstGrupoCodiModo = listaTodasURS.Select(x => x.Grupocodi.Value).Distinct().ToList();
            lstGrupoCodiURS.AddRange(lstGrupoCodiModo);

            List<PrGrupodatDTO> listaGrupoDatURS = this.ListarParametrosConfiguracionPorFecha(fechaDeOferta, ConstantesSubasta.ConcepcodiURSyModosCalificadas, string.Join(",", lstGrupoCodiURS));

            foreach (var urs in listaURSDistinc)
            {
                int cantModos = listaURSDistinc.Count();

                //obtener valores de la URS mediante PrGrupoDat
                var ursFecIni = listaGrupoDatURS.Find(x => x.Grupocodi == urs.Urscodi && x.Concepcodi == ConstantesSubasta.ConceptoURSFecInicio);
                var ursFecFin = listaGrupoDatURS.Find(x => x.Grupocodi == urs.Urscodi && x.Concepcodi == ConstantesSubasta.ConceptoURSFecfIn);
                var ursBanda = listaGrupoDatURS.Find(x => x.Grupocodi == urs.Urscodi && x.Concepcodi == ConstantesSubasta.ConceptoURSBanda);

                // empresas
                var ursEmpresas = listaTodasURS.Where(x => x.Urscodi == urs.Urscodi).OrderBy(y => y.Emprnomb).ToList();

                foreach (var item in ursEmpresas.GroupBy(x => x.Emprcodi))
                {
                    var listaModos = item.OrderBy(y => y.Gruponomb).ToList();

                    foreach (var modo in listaModos)
                    {
                        PrGrupodatDTO modBanda = new PrGrupodatDTO();
                        switch (modo.Catecodi)
                        {
                            case 5:
                                modBanda = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi && x.Concepcodi == ConstantesSubasta.ConceptoGrupHidraBandaCalificada);
                                break;
                            case 9:
                                modBanda = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi && x.Concepcodi == ConstantesSubasta.ConceptoMOHidroBandaCalificada);
                                break;
                            case 2:
                                modBanda = listaGrupoDatURS.Find(x => x.Grupocodi == modo.Grupocodi && x.Concepcodi == ConstantesSubasta.ConceptoMOTermoBandaCalificada);
                                break;
                        }
                        //nivel de URS
                        decimal? ValorNull = null;
                        modo.BandaCalificada = modBanda != null ? Convert.ToDecimal(modBanda.Formuladat) : ValorNull;

                        modo.FechaInico = ursFecIni != null ? ursFecIni.Formuladat.Trim() : string.Empty;
                        modo.FechaFin = ursFecFin != null ? ursFecFin.Formuladat.Trim() : string.Empty;
                        modo.BandaURS = ursBanda != null ? Convert.ToDecimal(ursBanda.Formuladat) : ValorNull;

                        //validar estad vigencia
                        if (ursFecIni != null && ursFecFin != null)
                        {
                            var fecIniURS = DateTime.ParseExact(modo.FechaInico, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                            var fecFinURS = DateTime.ParseExact(modo.FechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                            modo.Estado = (fechaDeOferta.Date >= fecIniURS && fechaDeOferta.Date <= fecFinURS) ? ConstantesSubasta.EstadoVigenteDesc : ConstantesSubasta.EstadoNoVigenteDesc;
                        }
                        else
                        {
                            modo.Estado = string.Empty;
                        }

                        lstSalida.Add(modo);
                    }
                }
            }


            return lstSalida;
        }

        /// <summary>
        /// Genera los registro de la relacion Activacion con Motivo
        /// </summary>
        /// <param name="datosAGuardar"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<SmaActivacionMotivoDTO> GenerarRelacionesActivacionMotivo(DatoActivacionOferta datosAGuardar, string usuario)
        {
            List<SmaActivacionMotivoDTO> salida = new List<SmaActivacionMotivoDTO>();

            if (datosAGuardar.IdsMotivosSubir != null)
            {
                foreach (var idMotivo in datosAGuardar.IdsMotivosSubir)
                {
                    SmaActivacionMotivoDTO objRelacion = new SmaActivacionMotivoDTO();
                    objRelacion.Smammcodi = idMotivo;
                    objRelacion.Smaacmtiporeserva = ConstantesSubasta.ReservaSubir;
                    objRelacion.Smaacmusucreacion = usuario;
                    objRelacion.Smaacmfeccreacion = DateTime.Now;

                    salida.Add(objRelacion);
                }
            }

            if (datosAGuardar.IdsMotivosBajar != null)
            {
                foreach (var idMotivo in datosAGuardar.IdsMotivosBajar)
                {
                    SmaActivacionMotivoDTO objRelacion = new SmaActivacionMotivoDTO();
                    objRelacion.Smammcodi = idMotivo;
                    objRelacion.Smaacmtiporeserva = ConstantesSubasta.ReservaBajar;
                    objRelacion.Smaacmusucreacion = usuario;
                    objRelacion.Smaacmfeccreacion = DateTime.Now;

                    salida.Add(objRelacion);
                }
            }

            return salida;
        }

        /// <summary>
        /// Da formato a los registros de activacion de oferta a guardar
        /// </summary>
        /// <param name="datosAGuardar"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public DatoActivacionOferta FormatearInformacionDataActivacion(DatoActivacionOferta datosAGuardar, string usuario)
        {
            DatoActivacionOferta salida = new DatoActivacionOferta();

            SmaActivacionDataDTO datosDeficitSubir = datosAGuardar.DatosDeficitSubir;
            datosDeficitSubir.Smaacdtipodato = ConstantesSubasta.DatoDeficitRSF;
            datosDeficitSubir.Smaacdtiporeserva = ConstantesSubasta.ReservaSubir;
            datosDeficitSubir.Smaacdusucreacion = usuario;
            datosDeficitSubir.Smaacdfeccreacion = DateTime.Now;
            CompletarConCerosAVacios(datosDeficitSubir);

            SmaActivacionDataDTO datosDeficitBajar = datosAGuardar.DatosDeficitBajar;
            datosDeficitBajar.Smaacdtipodato = ConstantesSubasta.DatoDeficitRSF;
            datosDeficitBajar.Smaacdtiporeserva = ConstantesSubasta.ReservaBajar;
            datosDeficitBajar.Smaacdusucreacion = usuario;
            datosDeficitBajar.Smaacdfeccreacion = DateTime.Now;
            CompletarConCerosAVacios(datosDeficitBajar);

            SmaActivacionDataDTO datosReduccionSubir = datosAGuardar.DatosReduccionBandaSubir;
            datosReduccionSubir.Smaacdtipodato = ConstantesSubasta.DatoReduccionBanda;
            datosReduccionSubir.Smaacdtiporeserva = ConstantesSubasta.ReservaSubir;
            datosReduccionSubir.Smaacdusucreacion = usuario;
            datosReduccionSubir.Smaacdfeccreacion = DateTime.Now;
            CompletarConCerosAVacios(datosReduccionSubir);

            SmaActivacionDataDTO datosReduccionBajar = datosAGuardar.DatosReduccionBandaBajar;
            datosReduccionBajar.Smaacdtipodato = ConstantesSubasta.DatoReduccionBanda;
            datosReduccionBajar.Smaacdtiporeserva = ConstantesSubasta.ReservaBajar;
            datosReduccionBajar.Smaacdusucreacion = usuario;
            datosReduccionBajar.Smaacdfeccreacion = DateTime.Now;
            CompletarConCerosAVacios(datosReduccionBajar);

            salida.DatosDeficitSubir = datosDeficitSubir;
            salida.DatosDeficitBajar = datosDeficitBajar;
            salida.DatosReduccionBandaSubir = datosReduccionSubir;
            salida.DatosReduccionBandaBajar = datosReduccionBajar;

            return salida;
        }

        /// <summary>
        /// Completa con cero aquellas celdas que estan vacias
        /// </summary>
        /// <param name="datosAnalizar"></param>
        public void CompletarConCerosAVacios(SmaActivacionDataDTO datosAnalizar)
        {
            for (int i = 1; i <= 48; i++)
            {
                //decimal valor = (decimal?)datosAnalizar.GetType().GetProperty("H" + i.ToString()).GetValue(datosAnalizar, null);
                decimal valor = ((decimal?)datosAnalizar.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(datosAnalizar, null)).GetValueOrDefault(0);
                //m24.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(m24, valorH);
                decimal cero = 0;
                if (valor == null || valor == 0)
                    datosAnalizar.GetType().GetProperty("H" + i.ToString()).SetValue(datosAnalizar, cero);
            }
        }

        /// <summary>
        /// Guarda los datos de la ventana y de las activaciones
        /// </summary>
        /// <param name="objActivacionOferta"></param>
        /// <param name="regOfertaActivacion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int GuardarActivacionOfertaTransaccional(SmaActivacionOfertaDTO objActivacionOferta, SmaOfertaDTO regOfertaActivacion, string usuario)
        {

            int smapaccodi = 0;
            int smaacmcodi = 0;
            int smaacdcodi = 0;
            int ofercodi = 0;

            IDbConnection conn = null;
            DbTransaction tran = null;

            try
            {
                conn = FactorySic.GetFtExtEnvioRepository().BeginConnection();
                tran = FactorySic.GetFtExtEnvioRepository().StartTransaction(conn);


                //Guardar activacion oferta
                smapaccodi = SaveSmaActivacionOfertaTransaccional(objActivacionOferta, conn, tran);

                //Guardar activacion motivos
                foreach (SmaActivacionMotivoDTO regRelacionMotivoActivacion in objActivacionOferta.ListaMotivosXActivacion)
                {
                    regRelacionMotivoActivacion.Smapaccodi = smapaccodi;
                    smaacmcodi = SaveSmaActivacionMotivoTransaccional(regRelacionMotivoActivacion, conn, tran);
                }

                //Guardar activacion data (deficit de RSF y Reduccion de Banda)
                foreach (SmaActivacionDataDTO regActivacionData in objActivacionOferta.ListaDatosXActivacion)
                {
                    regActivacionData.Smapaccodi = smapaccodi;
                    smaacdcodi = SaveSmaActivacionDataTransaccional(regActivacionData, conn, tran);
                }

                //Guardo data de activaciones
                if (regOfertaActivacion != null)
                {
                    regOfertaActivacion.Smapaccodi = smapaccodi;
                    string urscodisEnvio = string.Join(",", regOfertaActivacion.ListaDetalles.Select(x => x.Urscodi).Distinct());
                    ofercodi = FactorySic.GetSmaOfertaRepository().Save(regOfertaActivacion, urscodisEnvio, conn, tran);
                    //resultOfer = FactorySic.GetSmaOfertaRepository().Save(entity, conn, tran); //

                    int corrOferdet = FactorySic.GetSmaOfertaDetalleRepository().GetMaxId();
                    int corrMO = FactorySic.GetSmaRelacionOdMoRepository().GetMaxId();
                    foreach (SmaOfertaDetalleDTO detalle in regOfertaActivacion.ListaDetalles)
                    {
                        int ofdecodi = FactorySic.GetSmaOfertaDetalleRepository().Save(ofercodi, detalle, conn, tran, corrOferdet);

                        foreach (SmaRelacionOdMoDTO regRelacion in detalle.RelacionesODMO)
                        {
                            int odmocodi = FactorySic.GetSmaRelacionOdMoRepository().Save(ofdecodi, regRelacion, conn, tran, corrMO);
                            corrMO++;
                        }
                        corrOferdet++;
                    }
                }
                //guardar definitivamente
                tran.Commit();
            }
            catch (Exception ex)
            {
                smapaccodi = 0;
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException("Ocurrió un error al momento de guarda los datos de la activacion de oferta por defecto.");
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }

            return smapaccodi;


        }

        #region Indisponibilidad temporal urs
        /// <summary>
        /// Devuelve el parametro Potencia Urs min auto para cierta fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PrGrupodatDTO ObtenerParametroPotenciaUrsMinAuto(DateTime fecha)
        {
            List<PrGrupodatDTO> lstParam = FactorySic.GetPrGrupodatRepository().ParametrosConfiguracionPorFecha(fecha, "0", ConstantesSubasta.ParametroPotenciaUrsMinAuto.ToString());
            PrGrupodatDTO paramPotMinAuto = lstParam.First(); //item.Formuladat

            return paramPotMinAuto;
        }

        /// <summary>
        /// Guarda la ifnormacion de indisponibilidad temporal
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <param name="datosAGuardar"></param>
        /// <param name="usuario"></param>
        public void GuardarIndisponibilidadTemporaUrs(DateTime fechaDeOferta, List<SmaIndisponibilidadTempDetDTO> datosAGuardar, string usuario)
        {
            SmaIndisponibilidadTempCabDTO indispCab = FactorySic.GetSmaIndisponibilidadTempCabRepository().ObtenerPorFecha(fechaDeOferta);

            List<SmaIndisponibilidadTempDetDTO> lstDetalle = new List<SmaIndisponibilidadTempDetDTO>();

            //Obtengo los detalles a guardar
            foreach (SmaIndisponibilidadTempDetDTO regDet in datosAGuardar)
            {
                SmaIndisponibilidadTempDetDTO detalle = new SmaIndisponibilidadTempDetDTO();
                //detalle.Intdetcodi { get; set; } 
                //detalle.Intcabcodi { get; set; } 
                detalle.Urscodi = regDet.Urscodi;
                detalle.Intdetindexiste = regDet.IntdetindexisteDesc == "Si" ? ConstantesSubasta.Si : regDet.IntdetindexisteDesc == "No" ? ConstantesSubasta.No : "";
                detalle.Intdettipo = regDet.IntdettipoDesc == "Total" ? ConstantesSubasta.TipoTotal : regDet.IntdettipoDesc == "Parcial" ? ConstantesSubasta.TipoParcial : "";
                detalle.Intdetbanda = regDet.Intdetbanda;
                detalle.Intdetmotivo = regDet.Intdetmotivo;

                lstDetalle.Add(detalle);
            }



            SmaIndisponibilidadTempCabDTO cabGuardar = new SmaIndisponibilidadTempCabDTO();
            //Si existe el registro para la fecha se actualiza
            if (indispCab != null)
            {
                cabGuardar = indispCab;
                cabGuardar.Intcabfecmodificacion = DateTime.Now;
                cabGuardar.Intcabusumodificacion = usuario;
            }
            else
            {
                //cabGuardar.Intcabcodi { get; set; } 
                cabGuardar.Intcabfecha = fechaDeOferta;
                cabGuardar.Intcabusucreacion = usuario;
                cabGuardar.Intcabfeccreacion = DateTime.Now;
                //cabGuardar.Intcabusumodificacion { get; set; } 
                //cabGuardar.Intcabfecmodificacion { get; set; } 
            }

            cabGuardar.ListaDetalles = lstDetalle;

            //Guardar 
            this.GuardarIndisponibilidadTemporalTransaccional(fechaDeOferta, cabGuardar, usuario);
        }

        /// <summary>
        /// Guarda transaccionalmente los datos de indisponibilidad temporal
        /// </summary>
        /// <param name="fechaOferta"></param>
        /// <param name="indispCab"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int GuardarIndisponibilidadTemporalTransaccional(DateTime fechaOferta, SmaIndisponibilidadTempCabDTO indispCab, string usuario)
        {

            int intcabcodi = 0;
            int intdetcodi = 0;

            IDbConnection conn = null;
            DbTransaction tran = null;

            try
            {
                conn = FactorySic.GetFtExtEnvioRepository().BeginConnection();
                tran = FactorySic.GetFtExtEnvioRepository().StartTransaction(conn);

                SmaIndisponibilidadTempCabDTO regCabBD = FactorySic.GetSmaIndisponibilidadTempCabRepository().ObtenerPorFecha(fechaOferta);

                if (regCabBD == null)
                {
                    //Guardar cabecera
                    intcabcodi = SaveSmaIndisponibilidadTempCabTransaccional(indispCab, conn, tran);
                }
                else
                {
                    intcabcodi = regCabBD.Intcabcodi;

                    //actualizo cabecera
                    UpdateSmaIndisponibilidadTempCabTransaccional(indispCab, conn, tran);

                    //elimino detalles
                    DeleteSmaIndisponibilidadTempDetPorCabTransaccional(intcabcodi.ToString(), conn, tran);
                }

                //Guardar detalles
                foreach (SmaIndisponibilidadTempDetDTO regDetalle in indispCab.ListaDetalles)
                {
                    regDetalle.Intcabcodi = intcabcodi;
                    intdetcodi = SaveSmaIndisponibilidadTempDetTransaccional(regDetalle, conn, tran);
                }


                //guardar definitivamente
                tran.Commit();
            }
            catch (Exception ex)
            {
                intcabcodi = 0;
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException("Ocurrió un error al momento de guarda los datos de la indisponibilidad temporal URS.");
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }

            return intcabcodi;


        }

        /// <summary>
        /// Devuelve el registro de la indisponibilidad para cierta fecha
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <returns></returns>
        public SmaIndisponibilidadTempCabDTO ObtenerIndisponibilidadCabPorFecha(DateTime fechaDeOferta)
        {
            SmaIndisponibilidadTempCabDTO salida = FactorySic.GetSmaIndisponibilidadTempCabRepository().ObtenerPorFecha(fechaDeOferta);

            if (salida != null)
            {
                salida.IntcabfechaDesc = salida.Intcabfecha != null ? salida.Intcabfecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                salida.IntcabfeccreacionDesc = salida.Intcabfeccreacion != null ? salida.Intcabfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                salida.IntcabfecmodificacionDesc = salida.Intcabfecmodificacion != null ? salida.Intcabfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                salida.Intcabusucreacion = salida.Intcabusucreacion != null ? salida.Intcabusucreacion : "";
                salida.Intcabusumodificacion = salida.Intcabusumodificacion != null ? salida.Intcabusumodificacion : "";
            }

            return salida;
        }

        /// <summary>
        /// Devuelve los registros de indisponibilidad temporal para cierto dia
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <returns></returns>
        public List<SmaIndisponibilidadTempDetDTO> ObtenerDatosIndisponibilidadTemporal(DateTime fechaDeOferta, out bool mostrarTablaEnWeb)
        {
            List<SmaIndisponibilidadTempDetDTO> lstSalida = new List<SmaIndisponibilidadTempDetDTO>();

            // List<SmaIndisponibilidadTemporalDTO> lstIndisponibilidadBD = FactorySic.GetSmaIndisponibilidadTemporalRepository().ListarPorFecha(fechaDeOferta);
            List<SmaIndisponibilidadTempDetDTO> lstIndisponibilidadBD = FactorySic.GetSmaIndisponibilidadTempDetRepository().ListarPorFecha(fechaDeOferta);

            //busco el lsitado de urs
            List<SmaUrsModoOperacionDTO> listaURSCalificadas = ObtenerDatosURSCalificadas(fechaDeOferta);

            if (lstIndisponibilidadBD.Any())
            {
                FormatearDatosIndisponibilidadTemporal(lstIndisponibilidadBD, listaURSCalificadas);
                lstSalida = lstIndisponibilidadBD;
                mostrarTablaEnWeb = true;

            }
            else //si no hay indisponibilidd en BD uso datos de URS CALIFICADA (esto ultimo para obtener el listado de URS del dia con su "Banda URS calificada" que seran usado tanto para insumos al
                 //activar o para pintar la tabla para MAÑANA cuando sea primera carga)... Tener en cuenta que para fechas hoy o anterior no se pinta tabla en web
            {
                //Solo muestra en web si la fecha oferta es manana, sino no muestra en web
                mostrarTablaEnWeb = false;
                DateTime fechaManania = DateTime.Now.AddDays(1).Date;
                var valor = (fechaDeOferta.Date).CompareTo(fechaManania);
                if (valor == 0)
                {
                    mostrarTablaEnWeb = true;
                }

                listaURSCalificadas = listaURSCalificadas.Where(x => x.Estado == ConstantesSubasta.EstadoVigenteDesc).ToList();
                var lstUrs = listaURSCalificadas.GroupBy(x => new { x.Urscodi, x.Ursnomb, x.Emprcodi, x.Emprnomb, x.Central, x.BandaURS }).ToList();

                foreach (var regUrs in lstUrs)
                {
                    SmaIndisponibilidadTempDetDTO objIndisponibilidad = new SmaIndisponibilidadTempDetDTO();
                    //objIndisponibilidad.Intdetcodi { get; set; } 
                    //objIndisponibilidad.Intcabcodi { get; set; } 
                    objIndisponibilidad.Urscodi = regUrs.Key.Urscodi;
                    objIndisponibilidad.Intdetindexiste = ConstantesSubasta.No;
                    objIndisponibilidad.IntdetindexisteDesc = "No";
                    objIndisponibilidad.IntdettipoDesc = "";
                    //objIndisponibilidad.Intdettipo { get; set; } 
                    objIndisponibilidad.Intdetbanda = regUrs.Key.BandaURS;
                    //objIndisponibilidad.Intdetmotivo { get; set; } 
                    objIndisponibilidad.Ursnomb = regUrs.Key.Ursnomb;
                    objIndisponibilidad.Emprcodi = regUrs.Key.Emprcodi.Value;
                    objIndisponibilidad.Emprnomb = regUrs.Key.Emprnomb;
                    objIndisponibilidad.Centralnomb = regUrs.Key.Central;
                    objIndisponibilidad.BandaUrsCalificada = regUrs.Key.BandaURS;

                    lstSalida.Add(objIndisponibilidad);
                }
            }


            return lstSalida;
        }

        /// <summary>
        /// Da formato a los registros de indisponibilidad temporal
        /// </summary>
        /// <param name="lista"></param>
        public void FormatearDatosIndisponibilidadTemporal(List<SmaIndisponibilidadTempDetDTO> lista, List<SmaUrsModoOperacionDTO> listaURSCalificadas)
        {

            var lstUrs = listaURSCalificadas.GroupBy(x => new { x.Urscodi, x.Ursnomb, x.Emprcodi, x.Emprnomb, x.Central, x.BandaURS }).ToList();


            foreach (var item in lista)
            {
                var datoUrs = lstUrs.Find(x => x.Key.Urscodi == item.Urscodi);

                item.IntdetindexisteDesc = item.Intdetindexiste == ConstantesSubasta.Si ? "Si" : (item.Intdetindexiste == ConstantesSubasta.No ? "No" : "");
                item.IntdettipoDesc = item.Intdettipo == ConstantesSubasta.TipoTotal ? "Total" : (item.Intdettipo == ConstantesSubasta.TipoParcial ? "Parcial" : "");

                item.Ursnomb = datoUrs.Key.Ursnomb;
                item.Emprcodi = datoUrs.Key.Emprcodi.Value;
                item.Emprnomb = datoUrs.Key.Emprnomb;
                item.Centralnomb = datoUrs.Key.Central;
                item.BandaUrsCalificada = datoUrs.Key.BandaURS;
            }
        }

        #endregion

        #endregion

        #region Notificación plantilla de correo

        #region Métodos Tabla SI_PLANTILLACORREO

        /// <summary>
        /// Permite obtener un registro de la tabla SI_PLANTILLACORREO
        /// </summary>
        public SiPlantillacorreoDTO GetByIdSiPlantillacorreo(int plantcodi)
        {
            var plantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(plantcodi);
            FormatearSiPlantillacorreo(plantilla);

            return plantilla;
        }

        private void FormatearSiPlantillacorreo(SiPlantillacorreoDTO plantilla)
        {
            plantilla.Plantnomb = plantilla.Plantnomb ?? "";
            plantilla.Plantusucreacion = plantilla.Plantusucreacion != null ? plantilla.Plantusucreacion : "";
            plantilla.Plantusumodificacion = plantilla.Plantusumodificacion != null ? plantilla.Plantusumodificacion : "";
            plantilla.PlantfeccreacionDesc = plantilla.Plantfeccreacion != null ? plantilla.Plantfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : "";
            plantilla.PlantfecmodificacionDesc = plantilla.Plantfecmodificacion != null ? plantilla.Plantfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : "";

            plantilla.UltimaModificacionFechaDesc = plantilla.Plantfecmodificacion != null ? plantilla.PlantfecmodificacionDesc : plantilla.PlantfeccreacionDesc;
            plantilla.UltimaModificacionUsuarioDesc = plantilla.Plantfecmodificacion != null ? plantilla.Plantusumodificacion : plantilla.Plantusucreacion;
        }

        #endregion

        public List<SiPlantillacorreoDTO> ListarPlantillaNotificacion()
        {
            List<SiPlantillacorreoDTO> lista = new List<SiPlantillacorreoDTO>();
            List<int> plantcodis = new List<int> { ConstantesSubasta.PlantcodiNotifAgenteSinOfertaXDefecto, ConstantesSubasta.PlantcodiNotifAutogenerarOfertaXDefecto };

            lista = FactorySic.GetSiPlantillacorreoRepository().ListarPlantillas(string.Join(",", plantcodis)).OrderBy(x => x.Plantnomb).ToList();
            foreach (var reg in lista)
                FormatearSiPlantillacorreo(reg);

            return lista;
        }

        public void ActualizarDatosPlantillaCorreo(SiPlantillacorreoDTO correo, string usuario)
        {

            /**** Actualizar la plantilla ****/
            SiPlantillacorreoDTO c = servCorreo.GetByIdSiPlantillacorreo(correo.Plantcodi);
            c.Plantcontenido = correo.Plantcontenido;
            c.Plantasunto = correo.Plantasunto;
            c.Planticorreos = correo.Planticorreos;
            c.PlanticorreosCc = correo.PlanticorreosCc;
            c.PlanticorreosBcc = correo.PlanticorreosBcc;
            c.PlanticorreoFrom = correo.PlanticorreoFrom;
            c.Plantfecmodificacion = DateTime.Now;
            c.Plantusumodificacion = usuario;

            servCorreo.UpdateSiPlantillacorreo(c);

        }

        #endregion

        #region Procesos Automáticos

        /// <summary>
        /// Método de Oferta RSF ejecutado en la Intranet a petición del usuario
        /// </summary>
        /// <param name="plantcodi"></param>
        /// <param name="resultadoEjec"></param>
        /// <param name="mensajeEjec"></param>
        public void EjecutarProcesoAutomaticoOfertaRSFXPlantilla(int plantcodi, out int resultadoEjec, out string mensajeEjec)
        {
            resultadoEjec = 0;
            mensajeEjec = "";

            if (ConstantesSubasta.PlantcodiNotifAgenteSinOfertaXDefecto == plantcodi)
                resultadoEjec = EjecutarProcesoRecordatorioSinOfDefectoAnioSig(plantcodi, out mensajeEjec);

            if (ConstantesSubasta.PlantcodiNotifAutogenerarOfertaXDefecto == plantcodi)
                resultadoEjec = EjecutarProcesoAutogeneracionOfDefectoAnioSig(plantcodi, out mensajeEjec);
        }

        /// <summary>
        /// Método de Oferta RSF ejecutado en el Servicio Distribuido de forma automática
        /// </summary>
        /// <param name="prcscodi"></param>
        public void EjecutarProcesoAutomaticoOfertaRSF(int prcscodi)
        {
            if (ConstantesSubasta.PrcscodiOfertaRSF_RecordatorioSinOfDefectoAnioSig == prcscodi)
                EjecutarProcesoAutomaticoOfertaRSFXPlantilla(ConstantesSubasta.PlantcodiNotifAgenteSinOfertaXDefecto, out int resultadoEjec, out string mensajeEjec);

            if (ConstantesSubasta.PrcscodiOfertaRSF_AutogeneracionOfDefectoAnioSig == prcscodi)
                EjecutarProcesoAutomaticoOfertaRSFXPlantilla(ConstantesSubasta.PlantcodiNotifAutogenerarOfertaXDefecto, out int resultadoEjec, out string mensajeEjec);
        }

        private int EjecutarProcesoRecordatorioSinOfDefectoAnioSig(int plantcodi, out string mensaje)
        {
            //0. Obtener año siguiente
            DateTime fechaActual = DateTime.Now;
            int anioSig = fechaActual.Year + 1;
            DateTime fecha1EneroSig = new DateTime(anioSig, 1, 1);

            try
            {
                SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(plantcodi) ?? throw new ArgumentException("No existe plantilla de correo " + plantcodi);

                //1. Obtener listado de empresas que tienen al menos una URS sin oferta por defecto
                List<SmaUserEmpresaDTO> listaEmpresaSinOfDef = ListarEmpresaSinOfDefecto(fecha1EneroSig, fechaActual);

                if (listaEmpresaSinOfDef.Any())
                {
                    //2. Enviar notificación a cada empresa
                    foreach (var objEmpr in listaEmpresaSinOfDef)
                    {
                        //Generar Tupla de Variable y valor
                        var mapaVariable = new Dictionary<string, string>();
                        mapaVariable[ConstantesSubasta.VariableNombreEmpresa] = objEmpr.Emprnomb;
                        mapaVariable[ConstantesSubasta.VariableAnioSiguiente] = anioSig.ToString();
                        mapaVariable[ConstantesSubasta.VariableCorreosAgente] = string.Join("; ", objEmpr.ListaCorreo);

                        try
                        {
                            //Notificación
                            EnviarNotificacionAutomatico(plantilla, mapaVariable, objEmpr.Emprcodi, fecha1EneroSig, fechaActual);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ConstantesAppServicio.LogError + ". Oferta RSF - No se envió notificación de empresa " + objEmpr.Emprnomb, ex);
                        }
                    }

                    mensaje = "Se enviaron las notificaciones de forma correcta.";
                    return 1;
                }
                else
                {
                    mensaje = "No hay empresas a quien notificar dado que todas tienen ofertas registradas por defecto para el mes de enero del siguiente año.";
                    return 0;
                }
            }
            catch (Exception ex)
            {
                mensaje = "Ha ocurrido un error al procesar.";
                Logger.Error(ConstantesAppServicio.LogError, ex);

                return -1;
            }
        }

        public List<SmaUserEmpresaDTO> ListarEmpresaSinOfDefecto(DateTime fechaMes, DateTime fechaActual)
        {
            List<SmaUserEmpresaDTO> listaEmpresa = new List<SmaUserEmpresaDTO>();

            //Lista de URS calificadas vigentes para enero del proximo año
            List<SmaUrsModoOperacionDTO> listaUrsIniOferta = ReporteListadoURS(fechaMes, ConstantesSubasta.EstadoURSVigente);
            List<int> listaURSxVigente = listaUrsIniOferta.Select(x => x.Urscodi).Distinct().ToList();

            //filtros data historica de oferta defecto
            int opcionReporte = 1;
            int tipoOferta = ConstantesSubasta.OfertipoDefecto;
            int userCode = ConstantesSubasta.Todos;
            int oferCodi = ConstantesSubasta.Todos;
            int empresaCodi = ConstantesSubasta.Todos;
            var listUrs = string.Join(",", listaURSxVigente);

            if (listaURSxVigente.Any())
            {
                //Obtener oferta por defecto registrado por el agente en la Extranet
                List<SmaOfertaDTO> listaOferta = ListaConsultaOferta(opcionReporte, tipoOferta, fechaMes, fechaMes, userCode, oferCodi.ToString(), empresaCodi, listUrs, ConstantesSubasta.FuenteExtranet);

                //Verificar que exista
                List<int> listaURSxSinOfDefecto = new List<int>();
                foreach (var urscodi in listaURSxVigente)
                {
                    var regExisteOfDef = listaOferta.Find(x => x.Urscodi == urscodi);
                    
                    if (regExisteOfDef == null)
                    {
                        listaURSxSinOfDefecto.Add(urscodi);
                    }
                }

                //al menos una URS no fue registrada en enero del siguiente año
                if (listaURSxSinOfDefecto.Any())
                {                    
                    //Obtengo las ofertas por defecto totales para el periodo de vigencia
                    List<SmaUrsModoOperacionDTO> listaURSCalificadas = ObtenerDatosURSCalificadas(fechaMes);
                    List<SmaUrsModoOperacionDTO> listaURSCalificadasVigentesFinal = listaURSCalificadas.Any() ? listaURSCalificadas.Where(x => x.Estado == ConstantesSubasta.EstadoVigenteDesc).ToList() : new List<SmaUrsModoOperacionDTO>();
                    List<SmaOfertaDTO> listaUrsConOfertaDefectoPorPeriodoDeCincoAnios = ObtenerOfertasPorDefectoPorPeriodoCincoAnios(listaURSCalificadasVigentesFinal, fechaActual, ConstantesSubasta.CasoAutogeneracionOfertasDefectoMesEnero);                     

                    //obtener los agentes activos (código y email) de cada empresa. No considerar a los usuarios COES
                    List<SmaUsuarioUrsDTO> listaUsuarioYURS = ListSmaUsuarioUrss().Where(x => (x.Userlogin ?? "").Trim().ToLower().Contains("@")).ToList();

                    //obtner las urs sin of defecto con datos adicionales (empresa, correo agente)
                    List<SmaUrsModoOperacionDTO> listaUrsANotif = listaUrsIniOferta.Where(x => listaURSxSinOfDefecto.Contains(x.Urscodi)).ToList();

                    //Precio máximo Osinergmin
                    decimal paramPrecioMaximo = 0;
                    PrGrupodatDTO datoConf = this.GetValorConfiguracion(fechaMes, ConstantesSubasta.PrecioMaximo);
                    if (datoConf != null) paramPrecioMaximo = Convert.ToDecimal(datoConf.Formuladat); 

                    //por cada empresa obtener la lista de correos
                    var lstEmpresasNotificar = listaUrsANotif.GroupBy(x => x.Emprcodi);
                    foreach (var itemXEmpr in lstEmpresasNotificar)
                    {
                        //ADD
                        //Obtengo las ofertas por defecto declaradas en su periodo de 5 anios para la empresa
                        List<SmaOfertaDTO> listaOfDefectoPeriodoVigenciaPorEmpresa = listaUrsConOfertaDefectoPorPeriodoDeCincoAnios.Where(x => x.Emprcodi == itemXEmpr.Key && x.Repoprecio != null && x.Repoprecio != "").OrderByDescending(x=>x.Oferfechainicio).ToList();
                        List<DateTime?> lstFechasOfDefecto = listaOfDefectoPeriodoVigenciaPorEmpresa.Select(x => x.Oferfechainicio).ToList();
                        //ADD

                        //Genero las ofertas para el prox anio
                        List<int> listaUrsXEmp = itemXEmpr.Select(x => x.Urscodi).ToList();
                        List<string> listaEmailAgenteXEmp = listaUsuarioYURS.Where(x => listaUrsXEmp.Contains(x.Urscodi ?? 0)).
                            Select(x => x.Userlogin).Distinct().OrderBy(x => x).ToList();

                        SmaUserEmpresaDTO objEmp = new SmaUserEmpresaDTO()
                        {
                            Emprcodi = itemXEmpr.Key ?? 0,
                            Emprnomb = itemXEmpr.First().Emprnomb,
                            ListaCorreo = listaEmailAgenteXEmp,
                            OfertaAutogenerada = GetOfertaAutogenerada(fechaMes, fechaActual, paramPrecioMaximo, itemXEmpr.ToList(), listaOfDefectoPeriodoVigenciaPorEmpresa),
                        };

                        listaEmpresa.Add(objEmp);
                    }
                }
            }
            else
            {
                //se asume que siempre existen urs vigentes
                throw new ArgumentException(string.Format("No existe URS calificadas vigentes para enero de {0}.", fechaMes.Year));
            }

            //salida
            return listaEmpresa.OrderBy(x => x.Emprnomb).ToList();
        }

        private void EnviarNotificacionAutomatico(SiPlantillacorreoDTO plantilla, Dictionary<string, string> mapaVariable,
                                            int emprcodi, DateTime fechaProceso, DateTime fechaActual)
        {
            string from = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreoFrom, mapaVariable);
            string to = CorreoAppServicio.GetTextoSinVariable(plantilla.Planticorreos, mapaVariable);
            string cc = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreosCc, mapaVariable);
            string bcc = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreosBcc, mapaVariable);
            string asunto = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantasunto, mapaVariable);
            string contenido = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantcontenido, mapaVariable);

            List<string> listaTo = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(to);
            List<string> listaCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(cc);
            List<string> listaBCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(bcc, true, true);
            //listaBCC.Add("desamovisoft05@gmail.com");
            //listaBCC.Add("soporte1@movisoft.pe");
            string asuntoSendEmail = CorreoAppServicio.GetTextoAsuntoSegunAmbiente(asunto);

            //Enviar correo
            COES.Base.Tools.Util.SendEmail(listaTo, listaCC, listaBCC, asuntoSendEmail, contenido, null);

            //guardar log en BD
            SiCorreoDTO correo = new SiCorreoDTO();
            correo.Corrasunto = asunto;
            correo.Corrcontenido = contenido;
            correo.Corrfechaenvio = fechaActual;
            correo.Corrfechaperiodo = fechaProceso;
            correo.Corrfrom = from;
            correo.Corrto = to;
            correo.Corrcc = cc;
            correo.Corrbcc = bcc;
            correo.Emprcodi = emprcodi;
            correo.Plantcodi = plantilla.Plantcodi;

            servCorreo.SaveSiCorreo(correo);
        }

        /// <summary>
        /// Genera Ofertas por defecto para el año siguiente
        /// </summary>
        /// <param name="plantcodi"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        private int EjecutarProcesoAutogeneracionOfDefectoAnioSig(int plantcodi, out string mensaje)
        {
            //0. Obtener año siguiente
            DateTime fechaActual = DateTime.Now;
            int anioSig = fechaActual.Year + 1;
            DateTime fecha1EneroSig = new DateTime(anioSig, 1, 1);

            try
            {
                SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(plantcodi) ?? throw new ArgumentException("No existe plantilla de correo " + plantcodi);

                //1. Obtener listado de empresas que tienen al menos una URS sin oferta por defecto
                List<SmaUserEmpresaDTO> listaEmpresaSinOfDef = ListarEmpresaSinOfDefecto(fecha1EneroSig, fechaActual);

                if (listaEmpresaSinOfDef.Any())
                {
                    //2. Procesar a cada empresa
                    foreach (var objEmpr in listaEmpresaSinOfDef)
                    {
                        //Generar Tupla de Variable y valor
                        var mapaVariable = new Dictionary<string, string>();
                        mapaVariable[ConstantesSubasta.VariableNombreEmpresa] = objEmpr.Emprnomb;
                        mapaVariable[ConstantesSubasta.VariableAnioSiguiente] = anioSig.ToString();
                        mapaVariable[ConstantesSubasta.VariableCorreosAgente] = string.Join("; ", objEmpr.ListaCorreo);
                        mapaVariable[ConstantesSubasta.VariableTablaOfertaDefectoAutogenerado] = GetHtmlTablaOfertaDefectoAutogenerado(objEmpr.OfertaAutogenerada, objEmpr.OfertaAutogenerada.ListaDetalle);

                        try
                        {
                            //2.1 Generar oferta defecto por empresa. Como el SMA_OFERTA está asociado al usuario SISTEMA no se podrá consultar en Extranet pero sí en Intranet
                            SaveSmaOferta(objEmpr.OfertaAutogenerada, objEmpr.OfertaAutogenerada.ListaDetalle);

                            //2.2 Notificación
                            EnviarNotificacionAutomatico(plantilla, mapaVariable, objEmpr.Emprcodi, fecha1EneroSig, fechaActual);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ConstantesAppServicio.LogError + ". Oferta RSF - No se envió notificación de empresa " + objEmpr.Emprnomb, ex);
                        }
                    }

                    mensaje = "Se enviaron las notificaciones de forma correcta.";
                    return 1;
                }
                else
                {
                    mensaje = "No hay empresas a quien notificar dado que todas tienen ofertas registradas por defecto para el mes de enero del siguiente año.";
                    return 0;
                }
            }
            catch (Exception ex)
            {
                mensaje = "Ha ocurrido un error al procesar.";
                Logger.Error(ConstantesAppServicio.LogError, ex);

                return -1;
            }
        }

        private SmaOfertaDTO GetOfertaAutogenerada(DateTime fecha1Enero, DateTime fechaActual, decimal paramPrecioMaximo, List<SmaUrsModoOperacionDTO> listaUrsANotif, List<SmaOfertaDTO> listaOfDefectoPeriodoVigenciaPorEmpresa) 
        {
            //cabecera (SMA_OFERTA)
            SmaOfertaDTO objEnvio = new SmaOfertaDTO();
            objEnvio.Oferfechainicio = fecha1Enero;
            objEnvio.Oferfechafin = fecha1Enero;
            objEnvio.Oferfechaenvio = fechaActual;
            objEnvio.Ofertipo = ConstantesSubasta.OfertipoDefecto;
            objEnvio.Oferusucreacion = "SISTEMA";
            objEnvio.Usercode = ConstantesSubasta.UsercodeSistema;
            objEnvio.Oferfuente = ConstantesSubasta.FuenteExtranet; //por consultar

            //detalle (SMA_OFERTA_DETALLE)
            List<SmaOfertaDetalleDTO> listaDetalle = new List<SmaOfertaDetalleDTO>();            

            //la variable "listaUrsANotif" tiene 1 a más grupos (SMA_RELACION_OD_MO)
            foreach (var itemGrupo in listaUrsANotif)
            {
                List<SmaOfertaDTO> lstOfertaDefectoUrsSubir = listaOfDefectoPeriodoVigenciaPorEmpresa.Where(x => x.Urscodi == itemGrupo.Urscodi && x.Ofdetipo == ConstantesSubasta.TipoCargaSubir).OrderByDescending(x => x.Oferfechainicio).ToList();
                List<SmaOfertaDTO> lstOfertaDefectoUrsBajar = listaOfDefectoPeriodoVigenciaPorEmpresa.Where(x => x.Urscodi == itemGrupo.Urscodi && x.Ofdetipo == ConstantesSubasta.TipoCargaBajar).OrderByDescending(x => x.Oferfechainicio).ToList();

                SmaOfertaDTO ultimaOfertaDefectoURSSubir = lstOfertaDefectoUrsSubir.Any() ? lstOfertaDefectoUrsSubir.First() : null;
                SmaOfertaDTO ultimaOfertaDefectoURSBajar = lstOfertaDefectoUrsBajar.Any() ? lstOfertaDefectoUrsBajar.First() : null;

                listaDetalle.Add(GetOfertaDetalleXUrsAutogenerada(fecha1Enero, itemGrupo, "SISTEMA", paramPrecioMaximo, ultimaOfertaDefectoURSSubir, ConstantesSubasta.TipoCargaSubir));
                listaDetalle.Add(GetOfertaDetalleXUrsAutogenerada(fecha1Enero, itemGrupo, "SISTEMA", paramPrecioMaximo, ultimaOfertaDefectoURSBajar, ConstantesSubasta.TipoCargaBajar));
            }

            objEnvio.ListaDetalle = listaDetalle;

            return objEnvio;
        }

        private SmaOfertaDetalleDTO GetOfertaDetalleXUrsAutogenerada(DateTime fecha1Enero, SmaUrsModoOperacionDTO itemGrupo, string usuarioCreacion, decimal paramPrecioMaximo, SmaOfertaDTO ultimaOfertaDefecto, int tipo)
        {
            if (itemGrupo.Urscodi == 924)
            {
            }
            SmaOfertaDetalleDTO obj = new SmaOfertaDetalleDTO();
            obj.Ofdedusucreacion = usuarioCreacion;
            obj.Urscodi = itemGrupo.Urscodi;
            obj.BandaCalificada = itemGrupo.BandaURS ?? 0;
            obj.BandaDisponible = 0; //siempre es 0
            obj.Ofdepotofertada = (itemGrupo.BandaURS ?? 0) / 2.0m; //la mitad para subir, la mitad para bajar
            obj.Ofdemoneda = "604";
            obj.Ofdehorainicio = "00:00";
            obj.Ofdehorafin = "23:59";
            obj.Grupocodi = itemGrupo.Grupocodi ?? 0;
            obj.Ofdetipo = tipo;

            obj.Ursnomb = itemGrupo.Ursnomb;
            obj.Emprnomb = itemGrupo.Emprnomb;
            obj.Gruponomb = itemGrupo.Gruponomb;

            //se toma el menor valor entre el precio de la ultima oferta por defecto o el precio de parametros
            //Primero desencripto si esta encriptado
            if(ultimaOfertaDefecto != null)
            {
                if(ultimaOfertaDefecto.Repoprecio != "")
                {
                    string valOF = ultimaOfertaDefecto.Repoprecio.Trim();
                    if (AnalizarNumerico(valOF) == false)
                    {
                        valOF = DecryptData(valOF);
                    }
                    obj.Ofdeprecio = DevolverMenorValor(valOF, paramPrecioMaximo.ToString(), out int flag);

                    if(flag == -1) //valOF es menor que precioOsinergmin
                    {
                        obj.Observacion = "Se actualizó su precio de oferta por defecto con la información de su última oferta por defecto registrada";
                    }
                    else
                    {
                        obj.Observacion = "Se actualizó el precio de oferta por defecto con lo aprobado por OSINERGMIN debido a que el precio de última oferta por defecto es mayor o igual al precio aprobado por OSINERGMIN para el año " + fecha1Enero.Year;
                    }
                    
                }
                else
                {
                    obj.Ofdeprecio = paramPrecioMaximo.ToString();
                    obj.Observacion = "Dado que no tuvo oferta por defecto en los últimos 5 años, se propone el precio aprobado por Osinergmin para el año " + fecha1Enero.Year;
                }

            }
            else
            {
                obj.Ofdeprecio = paramPrecioMaximo.ToString();
                obj.Observacion = "Dado que no tuvo oferta por defecto en los últimos 5 años, se propone el precio aprobado por Osinergmin para el año " + fecha1Enero.Year;
            }
            
            return obj;
        }

        /// <summary>
        /// Devuelve el menor valor entro dos numeros string
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public string DevolverMenorValor(string val1, string val2, out int flag)
        {
            string salida = "";

            decimal valor1 = Convert.ToDecimal(val1);
            decimal valor2 = Convert.ToDecimal(val2);

            if (valor1 >= valor2)
            {
                flag = 1;
                salida = valor2.ToString();
            }

            else
            {
                flag = -1;
                salida = valor1.ToString();
            }

            return salida;
        }

        private string GetHtmlTablaOfertaDefectoAutogenerado(SmaOfertaDTO objEnvio, List<SmaOfertaDetalleDTO> listaDetalle)
        {
            StringBuilder str = new StringBuilder();

            str.Append("<div style='height: auto;'>");
            str.Append("<table cellspacing='0' style='display: table; border-collapse: separate;font-size: 11px;text-align: center;'>");

            str.Append("<thead style='background-color: #0b3fdb;height: 50px;color: white;'>");

            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='width: 70px; border-right: 1px solid white;'>FECHA DE OFERTA</th>");
            str.Append("<th style='width: 100px; border-right: 1px solid white;'>URS</th>");
            str.Append("<th style='width: 70px; border-right: 1px solid white;'>HORA DE INICIO</th>");
            str.Append("<th style='width: 70px; border-right: 1px solid white;'>HORA DE FIN</th>");
            str.Append("<th style='width: 67px; border-right: 1px solid white;'>PRECIO (S/. / MW-Mes)</th>");
            str.Append("<th style='width: 200px; border-right: 1px solid white;'>EMPRESA</th>");
            str.Append("<th style='width: 200px; border-right: 1px solid white;'>MODO DE OPERACIÓN</th>");
            //str.Append("<th style='width: 85px; border-right: 1px solid white;'>BANDA CALIFICADA</th>");
            str.Append("<th style='width: 80px; border-right: 1px solid white;'>CÓDIGO DE ENVÍO</th>");
            str.Append("<th style='width: 70px; border-right: 1px solid white;'>USUARIO</th>");            
            str.Append("<th style='width: 100px'>FECHA DE ENVÍO</th>");
            str.Append("<th style='width: 250px; border-right: 1px solid white;'>OBSERVACIÓN</th>");
            str.Append("</tr>");
            #endregion

            str.Append("</thead>");
            str.Append("<tbody>");
            #region cuerpo
            foreach (var sublistaxURS in listaDetalle.Where(x => x.Ofdetipo == ConstantesSubasta.TipoCargaSubir).GroupBy(x => x.Ursnomb).OrderBy(x => x.Key))
            {
                str.Append("<tr class='' style='height: 50px;'>");

                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", objEnvio.Oferfechainicio.Value.ToString(ConstantesAppServicio.FormatoMes)));
                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", sublistaxURS.Key));
                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", sublistaxURS.First().Ofdehorainicio));
                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", sublistaxURS.First().Ofdehorafin));
                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", sublistaxURS.First().Ofdeprecio));
                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", sublistaxURS.First().Emprnomb));
                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", string.Join("<br> ", sublistaxURS.Select(x => x.Gruponomb).OrderBy(x => x))));
                //str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", sublistaxURS.First().Ofdepotofertada));
                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", ""));
                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", objEnvio.Oferusucreacion));
                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", objEnvio.Oferfechaenvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull)));
                str.Append(string.Format("<td style='border: 1px solid #dbdcdd;'>{0}</td>", sublistaxURS.First().Observacion));

                str.Append("</tr>");
            }
            #endregion
            str.Append("</tbody>");

            str.Append("</table>");
            str.Append("</div>");

            return str.ToString();
        }

        /// <summary>
        /// Ejecutar opción manual de resetear enero Of Defecto
        /// </summary>
        public int ResetearRecordatoriosManualmente(out string mensaje)
        {
            //0. Obtener año siguiente
            DateTime fechaActual = DateTime.Now;
            int anioSig = fechaActual.Year + 1;

            DateTime fechaIniEneroSig = new DateTime(anioSig, 1, 1);
            DateTime fechaFinEneroSig = fechaIniEneroSig.AddMonths(1).AddDays(-1);

            //filtros data historica de oferta defecto
            int opcionReporte = 1;
            int tipoOferta = ConstantesSubasta.OfertipoDefecto;
            int userCode = ConstantesSubasta.Todos;
            int oferCodi = ConstantesSubasta.Todos;
            int empresaCodi = ConstantesSubasta.Todos;
            var listUrs = "-1";

            //1. Obtener oferta por defecto registrado por el agente en la Extranet
            List<SmaOfertaDTO> listaOferta = ListaConsultaOferta(opcionReporte, tipoOferta, fechaIniEneroSig, fechaIniEneroSig, userCode, oferCodi.ToString(), empresaCodi, listUrs, ConstantesSubasta.FuenteExtranet);

            if (listaOferta.Any())
            {
                //2. Dar de baja lógica a todos los envios de tipo "Oferta defecto" en el mes de enero
                ResetearOfertaDefecto(fechaIniEneroSig, fechaFinEneroSig);

                mensaje = "El proceso se ejecutó correctamente.";
                return 1;
            }
            else
            {
                mensaje = "No se ha autogenerado oferta por defecto para el mes de enero del siguiente año.";
                return 0;
            }
        }

        #endregion

        #region ofertas por defecto activadas

        public void CargarListarOfertasDefectoActivadas(DateTime fechaIni, DateTime fechaFin, string emprcodi, string listUrs, string fuente, out List<SmaOfertaDTO> listaOfertaSubir, out List<SmaOfertaDTO> listaOfertaBajar)
        {
            var emprecodis = emprcodi.Split(',').Select(x => int.Parse(x)).ToList();
            var urscodis = listUrs.Split(',').Select(x => int.Parse(x)).ToList();
            var lstfuente = fuente.Split(',').Select(x => x).ToList();

            int opcionReporte = 1;
            int tipoOferta = 1; // diaria
            int userCode = -1;
            int empresaCodi = -1;
            int oferCodi = -1;

            var fuenteConsulta = lstfuente.Count > 1 ? "-1" : lstfuente.First();

            List<SmaOfertaDTO> listaOfertaGeneral = this.ListaConsultaOferta(1, tipoOferta, fechaIni, fechaFin, userCode, "-1", empresaCodi, listUrs, fuenteConsulta);
            listaOfertaGeneral = listaOfertaGeneral.Where(x => x.Oferfuente != null).ToList();

            //filtrar por fuente
            //if (fuenteConsulta != null)
            //    listaOfertaGeneral = listaOfertaGeneral.Where(x => lstfuente.Contains(x.Oferfuente)).ToList();

            //filtrar las empresas
            listaOfertaGeneral = listaOfertaGeneral.Where(x => emprecodis.Contains(x.Emprcodi)).ToList();

            listaOfertaSubir = listaOfertaGeneral.Where(x => x.Ofdetipo == 1).ToList();
            this.FormatearListaOfertas(listaOfertaSubir);
            this.RealizarAgrupacion(listaOfertaSubir);

            listaOfertaBajar = listaOfertaGeneral.Where(x => x.Ofdetipo == 2).ToList();
            this.FormatearListaOfertas(listaOfertaBajar);
            this.RealizarAgrupacion(listaOfertaBajar);
        }

        public void RealizarAgrupacion(List<SmaOfertaDTO> listaOfertas)
        {
            foreach (var agrupacion in listaOfertas.GroupBy(x => new { x.Oferfechainicio, x.Urscodi }))
            {
                var cantidad = agrupacion.Count();
                var countFuentes = agrupacion.ToList().DistinctBy(x => x.Oferfuente).Count();

                foreach (var item in agrupacion.ToList())
                {
                    item.TineAgrupFuentes = countFuentes > 1 ? true : false;
                    item.Rowspan = cantidad;
                    break;
                }

                foreach (var agrupacion2 in agrupacion.GroupBy(x => new { x.Emprcodi, x.Ofdehorainicio, x.Ofdehorafin }))
                {
                    var cantidad2 = agrupacion2.Count();
                    var countFuentes2 = agrupacion2.ToList().DistinctBy(x => x.Oferfuente).Count();

                    foreach (var item2 in agrupacion2.ToList())
                    {
                        item2.TineAgrupFuentes2 = countFuentes2 > 1 ? true : false;
                        item2.Rowspan2 = cantidad2;
                        break;
                    }

                }
            }
        }

        public void FormatearListaOfertas(List<SmaOfertaDTO> listaOfertas)
        {
            foreach (var reg in listaOfertas)
            {
                reg.OferfechainicioDesc = reg.Oferfechainicio != null ? reg.Oferfechainicio.Value.ToString(ConstantesAppServicio.FormatoFecha) : "";
                reg.OferfechafinDesc = reg.Oferfechafin != null ? reg.Oferfechafin.Value.ToString(ConstantesAppServicio.FormatoFecha) : "";
                reg.RepopotoferDesc = Convert.ToString(reg.Repopotofer);
                reg.Repoprecio = !string.IsNullOrEmpty(reg.Repoprecio) ? (this.AnalizarNumerico(reg.Repoprecio) ? reg.Repoprecio : "NO DISPONIBLE") : "NO INGRESADO";
                reg.BandaCalificadaDesc = Convert.ToString(reg.BandaCalificada);
                reg.OferfechaenvioDesc = reg.Oferfechaenvio != null ? reg.Oferfechaenvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
            }
        }

        public string GenerarArchivoExcelOfertas(string ruta, string pathLogo, DateTime fechaIni, DateTime fechaFin, string emprcodi, string listUrs, string fuente, out bool existeDatos)
        {
            string fechIni = fechaIni.ToString(ConstantesAppServicio.FormatoFechaDMY);
            string fechFin = fechaFin.ToString(ConstantesAppServicio.FormatoFechaDMY);
            var lstfuente = fuente.Split(',').Select(x => x).ToList();
            existeDatos = false;

            //obtener data
            this.CargarListarOfertasDefectoActivadas(fechaIni, fechaFin, emprcodi, listUrs, fuente, out List<SmaOfertaDTO> listaOfertaSubir, out List<SmaOfertaDTO> listaOfertaBajar);
            //List<HandsonModel> listaTab = this.CargarListarOfertasDefectoActivadas2(fechaIni, fechaFin, emprcodi, listUrs, fuente);

            if (listaOfertaSubir.Any() || listaOfertaBajar.Any())
                existeDatos = true; // hay datos

            string nameFile = "ReporteDeOfertasRSF_";
            nameFile = fechaIni == fechaFin ? nameFile + fechIni + ".xlsx" : nameFile + fechIni + "_" + fechFin + ".xlsx";
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                string titulo = "Ofertas por Día Mercado de Ajuste y Activación de Ofertas por Defecto";

                //SUBIR
                this.GenerarExcelHojaOfertas(xlPackage, listaOfertaSubir, pathLogo, titulo, lstfuente, ConstantesSubasta.TabCargaSubir);
                //this.GenerarExcelHojaOfertas2(xlPackage, pathLogo, titulo, listaTab[0], 1, 2, ConstantesSubasta.TabCargaSubir);
                xlPackage.Save();

                //BAJAR
                this.GenerarExcelHojaOfertas(xlPackage, listaOfertaBajar, pathLogo, titulo, lstfuente, ConstantesSubasta.TabCargaBajar);
                //this.GenerarExcelHojaOfertas2(xlPackage, pathLogo, titulo, listaTab[1], 1, 2, ConstantesSubasta.TabCargaBajar);
                xlPackage.Save();
            }

            return nameFile;
        }

        private void GenerarExcelHojaOfertas(ExcelPackage xlPackage, List<SmaOfertaDTO> listaOfertas, string pathLogo, string titulo, List<string> lstfuente, string nameWS)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);

            //Logo
            UtilExcel.AddImageLocalAlto4Filas(ws, 1, 0, pathLogo);

            int rowIniHeader = 1;
            int colIniHeader = 2;
            int filTitulo = rowIniHeader + 1;
            int colTitulo = colIniHeader + 4;
            int filLeyenda = rowIniHeader + 3;
            int colLeyenda = colIniHeader + 2;

            ws.Cells[filTitulo, colTitulo].Value = titulo;
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filTitulo, colTitulo, filTitulo, colTitulo, "Calibri", 14);
            UtilExcel.CeldasExcelEnNegrita(ws, filTitulo, colTitulo, filTitulo, colTitulo);


            ws.Cells[filLeyenda, colLeyenda].Value = "LEYENDA";
            UtilExcel.CeldasExcelEnNegrita(ws, filLeyenda, colLeyenda, filLeyenda, colLeyenda);

            int filainileyen = 1;
            foreach (var item in lstfuente)
            {
                var color = item == "E" ? "#ffffff" : "#75A2FD";
                var nombre = item == "E" ? "Mercado de ajuste" : "Activación de Oferta por defecto";

                UtilExcel.BorderCeldas2(ws, filLeyenda + filainileyen, colLeyenda + 1, filLeyenda + filainileyen, colLeyenda + 1);
                UtilExcel.CeldasExcelColorFondo(ws, filLeyenda + filainileyen, colLeyenda + 1, filLeyenda + filainileyen, colLeyenda + 1, color);

                ws.Cells[filLeyenda + filainileyen, colLeyenda + 2].Value = nombre;

                filainileyen++;
            }

            int row = rowIniHeader + 7;
            int col = colIniHeader;

            #region Cabecera

            int filaIniCab = row;
            int coluIniCab = col;

            int colFecha = coluIniCab;
            int colUrs = colFecha + 1;
            int colHoraini = colUrs + 1;
            int colHorafin = colHoraini + 1;
            int colPotencia = colHorafin + 1;
            int colPrecio = colPotencia + 1;
            int colEmpresa = colPrecio + 1;
            int colModo = colEmpresa + 1;
            int colBanda = colModo + 1;
            int colCodenvio = colBanda + 1;
            int colUsuario = colCodenvio + 1;
            int colFechenvio = colUsuario + 1;

            ws.Cells[filaIniCab, colFecha].Value = "FECHA DE \n OFERTA";
            ws.Cells[filaIniCab, colUrs].Value = "URS";
            ws.Cells[filaIniCab, colHoraini].Value = "HORA DE \n INICIO";
            ws.Cells[filaIniCab, colHorafin].Value = "HORA DE \n FIN";
            ws.Cells[filaIniCab, colPotencia].Value = "POTENCIA \n OFERTADA";
            ws.Cells[filaIniCab, colPrecio].Value = "PRECIO \n (S/. / MW-MES)";
            ws.Cells[filaIniCab, colEmpresa].Value = "EMPRESA";
            ws.Cells[filaIniCab, colModo].Value = "MODO DE \n OPERACIÓN";
            ws.Cells[filaIniCab, colBanda].Value = "BANDA \n CALIFICADA";
            ws.Cells[filaIniCab, colCodenvio].Value = "CODIGO DE \n ENVÍO";
            ws.Cells[filaIniCab, colUsuario].Value = "USUARIO";
            ws.Cells[filaIniCab, colFechenvio].Value = "FECHA DE \n ENVÍO";

            UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab, filaIniCab, colFechenvio);
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCab, coluIniCab, filaIniCab, colFechenvio, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCab, coluIniCab, filaIniCab, colFechenvio, "Centro");
            UtilExcel.CeldasExcelColorTexto(ws, filaIniCab, coluIniCab, filaIniCab, colFechenvio, "#FFFFFF");
            UtilExcel.CeldasExcelColorFondo(ws, filaIniCab, coluIniCab, filaIniCab, colFechenvio, "#2B579A");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCab, coluIniCab, filaIniCab, colFechenvio, "Calibri", 11);
            UtilExcel.CeldasExcelEnNegrita(ws, filaIniCab, coluIniCab, filaIniCab, colFechenvio);
            UtilExcel.CeldasExcelWrapText(ws, filaIniCab, coluIniCab, filaIniCab, colFechenvio);
            ws.Row(filaIniCab).Height = 40;

            #endregion

            #region cuerpo

            int filaIniData = row + 1;
            int filaX = filaIniData;
            int coluIniData = coluIniCab;
            foreach (var item in listaOfertas)
            {
                var sColor = "";
                if (item.Oferfuente == "A") sColor = "#75A2FD"; // azul
                if (item.Oferfuente == "E") sColor = "#FFFFFF"; // blanco

                int filaAgrup = 1;
                if (item.Rowspan > 1)
                    filaAgrup = filaX + item.Rowspan - 1;

                if (item.Rowspan > 0)
                {
                    var sColor1 = item.TineAgrupFuentes ? "#FFFFFF" : sColor;

                    ws.Cells[filaIniData, colFecha].Value = item.OferfechainicioDesc;
                    ws.Cells[filaIniData, colUrs].Value = item.Ursnomb;

                    UtilExcel.CeldasExcelColorFondo(ws, filaIniData, colFecha, filaIniData, colUrs, sColor1);

                    if (item.Rowspan > 1)
                    {
                        UtilExcel.CeldasExcelAgrupar(ws, filaIniData, colFecha, filaAgrup, colFecha); // agrupar fecha
                        UtilExcel.CeldasExcelAgrupar(ws, filaIniData, colUrs, filaAgrup, colUrs); // agrupar urs
                    }
                }

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                int filaAgrup2 = 1;
                if (item.Rowspan2 > 1)
                    filaAgrup2 = filaX + item.Rowspan2 - 1;

                if (item.Rowspan2 > 0)
                {
                    var sColor1 = item.TineAgrupFuentes2 ? "#FFFFFF" : sColor;

                    ws.Cells[filaIniData, colHoraini].Value = item.Ofdehorainicio;
                    ws.Cells[filaIniData, colHorafin].Value = item.Ofdehorafin;

                    UtilExcel.CeldasExcelColorFondo(ws, filaIniData, colHoraini, filaIniData, colHorafin, sColor1);

                    if (item.Rowspan2 > 1)
                    {
                        UtilExcel.CeldasExcelAgrupar(ws, filaIniData, colHoraini, filaAgrup2, colHoraini); // agrupar hora inicio
                        UtilExcel.CeldasExcelAgrupar(ws, filaIniData, colHorafin, filaAgrup2, colHorafin); // agrupar hora fin
                    }
                }

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


                //ws.Cells[filaIniData, colFecha].Value = item.OferfechainicioDesc;
                //ws.Cells[filaIniData, colUrs].Value = item.Ursnomb;
                ws.Cells[filaIniData, colHoraini].Value = item.Ofdehorainicio;
                ws.Cells[filaIniData, colHorafin].Value = item.Ofdehorafin;
                ws.Cells[filaIniData, colPotencia].Value = item.RepopotoferDesc;
                ws.Cells[filaIniData, colPrecio].Value = item.Repoprecio;
                ws.Cells[filaIniData, colEmpresa].Value = item.Emprnomb;
                ws.Cells[filaIniData, colModo].Value = item.Gruponomb;

                UtilExcel.CeldasExcelColorFondo(ws, filaIniData, colHoraini, filaIniData, colModo, sColor);

                if (item.Rowspan > 0)
                {
                    var sColor1 = item.TineAgrupFuentes ? "#FFFFFF" : sColor;
                    ws.Cells[filaIniData, colBanda].Value = item.BandaCalificadaDesc;
                    UtilExcel.CeldasExcelColorFondo(ws, filaIniData, colBanda, filaIniData, colBanda, sColor1);

                    if (item.Rowspan > 1)
                        UtilExcel.CeldasExcelAgrupar(ws, filaIniData, colBanda, filaAgrup, colBanda); // agrupar urs
                }

                ws.Cells[filaIniData, colCodenvio].Value = item.Ofercodenvio;
                ws.Cells[filaIniData, colUsuario].Value = item.Username;
                ws.Cells[filaIniData, colFechenvio].Value = item.OferfechaenvioDesc;

                UtilExcel.CeldasExcelColorFondo(ws, filaIniData, colCodenvio, filaIniData, colFechenvio, sColor);

                filaIniData++;
                filaX++;
            }

            if (filaIniData - 1 >= row + 1)
            {
                UtilExcel.BorderCeldas2(ws, row + 1, coluIniCab, filaIniData - 1, colFechenvio);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, row + 1, coluIniCab, filaIniData - 1, colFechenvio, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, row + 1, coluIniCab, filaIniData - 1, colFechenvio, "Centro");
            }

            #endregion

            UtilExcel.CeldasExcelWrapText(ws, row + 1, coluIniCab, filaX, colFechenvio);


            ws.Column(1).Width = 2;

            ws.Column(colFecha).Width = 70 / 4.5;
            ws.Column(colUrs).Width = 80 / 4.5;
            ws.Column(colHoraini).Width = 50 / 4.5;
            ws.Column(colHorafin).Width = 50 / 4.5;
            ws.Column(colPotencia).Width = 70 / 4.5;
            ws.Column(colPrecio).Width = 70 / 4.5;
            ws.Column(colEmpresa).Width = 130 / 4.5;
            ws.Column(colModo).Width = 200 / 4.5;
            ws.Column(colBanda).Width = 70 / 4.5;
            ws.Column(colCodenvio).Width = 70 / 4.5;
            ws.Column(colUsuario).Width = 70 / 4.5;
            ws.Column(colFechenvio).Width = 70 / 4.5;

            //No mostrar lineas
            ws.View.ShowGridLines = false;
            ws.View.FreezePanes(filaIniCab + 1, coluIniCab + 2);
            ws.View.ZoomScale = 80;

            xlPackage.Save();
        }

        #endregion

        #region Grafico y Bitacora

        #region Grafico

        /// <summary>
        /// Lista las URS con informacion de potencia ofertada para graficos
        /// </summary>
        /// <param name="fechaOferta"></param>
        /// <param name="tipoOferta"></param>
        /// <param name="tipoGrafico"></param>
        /// <returns></returns>
        public List<SmaUrsModoOperacionDTO> ObtenerUrsConPotenciaOfertada(DateTime fechaOferta, string tipoOferta, string tipoGrafico)
        {
            List<SmaUrsModoOperacionDTO> lstSalida = new List<SmaUrsModoOperacionDTO>();
            List<SmaUrsModoOperacionDTO> ursTotales = this.ListSmaUrsModoOperacions_Urs(-1);

            if (tipoGrafico == ConstantesSubasta.GraficoUrsBajar || tipoGrafico == ConstantesSubasta.GraficoUrsSubir)
            {
                List<Dato48> listaPO = ObtenerDataGraficoPotenciaOfertada(fechaOferta, tipoOferta, tipoGrafico, ConstantesSubasta.EstadoDefecto, out bool HayInfoPotencia);
                if (HayInfoPotencia)
                {
                    List<int> lstUrscodis = listaPO.Select(x => x.Urscodi).Distinct().ToList();
                    lstSalida = ursTotales.Where(x => lstUrscodis.Contains(x.Urscodi)).ToList();
                }
            }

            return lstSalida;
        }

        /// <summary>
        /// Devuelve los datos de potencias ofertadas segun filtro ingresado
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <param name="tipoOferta"></param>
        /// <param name="tipoGrafico"></param>
        /// <param name="idsUrs"></param>
        /// <param name="hayInformacion"></param>
        /// <returns></returns>
        public List<Dato48> ObtenerDataGraficoPotenciaOfertada(DateTime fechaDeOferta, string tipoOferta, string tipoGrafico, string idsUrs, out bool hayInformacion)
        {
            List<Dato48> lstSalida = new List<Dato48>();
            hayInformacion = false;

            //Obtengo las ofertas segun el filtro ingresado
            List<SmaOfertaDetalleDTO> ofertasSegunFiltro = new List<SmaOfertaDetalleDTO>();
            int ofdetipo = (tipoGrafico == ConstantesSubasta.GraficoUrsSubir || tipoGrafico == ConstantesSubasta.GraficoTotalSubir) ? ConstantesSubasta.OfdetipoSubir : ConstantesSubasta.OfdetipoBajar;
            List<SmaOfertaDetalleDTO> ofertasTotalPorFecha = FactorySic.GetSmaOfertaDetalleRepository().ListByDateTipo(fechaDeOferta, fechaDeOferta, ConstantesSubasta.OfertipoDiaria, ConstantesSubasta.EstadoActivo);
            List<SmaOfertaDetalleDTO> ofertasTotalPorFechaYTipoG = ofertasTotalPorFecha.Where(x => x.Ofdetipo == ofdetipo).ToList();
            List<SmaOfertaDetalleDTO> ofertasTotalPorFechaYTipoGYTipoO = tipoOferta == ConstantesSubasta.FuenteExtranet ? ofertasTotalPorFechaYTipoG.Where(x => x.Oferfuente == tipoOferta).ToList() : ofertasTotalPorFechaYTipoG;
            ofertasSegunFiltro = ofertasTotalPorFechaYTipoGYTipoO;
            if (tipoGrafico == ConstantesSubasta.GraficoUrsSubir || tipoGrafico == ConstantesSubasta.GraficoUrsBajar)
            {
                if (idsUrs != ConstantesSubasta.EstadoDefecto)
                {
                    List<int> listaUrscodi = idsUrs.Split(',').Select(x => int.Parse(x)).ToList();
                    ofertasSegunFiltro = ofertasTotalPorFechaYTipoGYTipoO.Where(x => listaUrscodi.Contains(x.Urscodi)).ToList();
                }
            }

            //Doy formato a las ofertas
            List<Dato48> lstPotenciasOfertadasXUrs = new List<Dato48>();
            List<RangoEscenario> lstRangosEscenario = ObtenerDatosEscenario();

            var listaXUrs = ofertasSegunFiltro.GroupBy(x => new { x.Urscodi }).ToList();
            foreach (var item in listaXUrs)
            {
                int urscodi = item.Key.Urscodi;
                Dato48 objM48 = new Dato48();
                objM48.Urscodi = urscodi;
                objM48.Etiqueta = ofertasSegunFiltro.Find(x => x.Urscodi == urscodi).Gruponomb;

                foreach (SmaOfertaDetalleDTO regDet in item.OrderBy(x => x.Ofdehorainicio).ToList()) //aca se debe usar el item y recorrer, evitando crear un where por cada grupo de la lista inicial
                {
                    decimal potOfertada = regDet.Ofdepotofertada;

                    string horaIni = regDet.Ofdehorainicio;
                    string horaFin = regDet.Ofdehorafin;
                    int escIni = lstRangosEscenario.Find(x => x.HoraIni == horaIni).Escenario;
                    int escFin = lstRangosEscenario.Find(x => x.HoraFin == horaFin).Escenario;

                    for (int escenario = escIni; escenario <= escFin; escenario++)
                    {
                        objM48.GetType().GetProperty(ConstantesSubasta.CaracterV + escenario).SetValue(objM48, potOfertada);
                    }
                }

                lstPotenciasOfertadasXUrs.Add(objM48);
            }
            hayInformacion = lstPotenciasOfertadasXUrs.Any();


            //Si el tipo de grafico es TOTAL se debe sumar los valores
            if (tipoGrafico == ConstantesSubasta.GraficoTotalSubir || tipoGrafico == ConstantesSubasta.GraficoTotalBajar)
            {
                Dato48 objM48_ = new Dato48();
                objM48_.Etiqueta = "Oferta Diaria Total";

                for (int escenario = 1; escenario <= 48; escenario++)
                {
                    decimal? suma = 0;
                    foreach (var regDet in lstPotenciasOfertadasXUrs)
                    {
                        decimal? val = ((decimal?)regDet.GetType().GetProperty(ConstantesSubasta.CaracterV + escenario).GetValue(regDet, null)).GetValueOrDefault(0);
                        suma = suma + val;
                    }
                    suma = suma != 0 ? suma : null;
                    objM48_.GetType().GetProperty(ConstantesSubasta.CaracterV + escenario).SetValue(objM48_, suma);
                }

                lstSalida.Add(objM48_);
            }
            else
            {
                lstSalida = lstPotenciasOfertadasXUrs;

                //Si no existe informacion para el filtro, mando registros nullos para mostrar grafico
                if (!lstPotenciasOfertadasXUrs.Any())
                {

                }
            }


            return lstSalida;
        }

        /// <summary>
        /// Devuelve la informacion de reserva de RSF para el filtro ingresado
        /// </summary>
        /// <param name="fechaDeOferta"></param>
        /// <param name="idsUrs"></param>
        /// <param name="tipoGrafico"></param>
        /// <returns></returns>
        public Dato48 ObtenerDataGraficoReservaRSF(DateTime fechaDeOferta, string idsUrs, string tipoGrafico)
        {
            Dato48 regSalida = new Dato48();
            regSalida.ConInformacion = false;

            DateTime fechaIniSemana = EPDate.f_fechainiciosemana(fechaDeOferta);
            //fechaIniSemana = new DateTime(2024, 6, 17);
            CpTopologiaDTO topSemFinal = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(fechaIniSemana.Date, ConstantesCortoPlazo.TopologiaSemanal);

            if (topSemFinal != null)
            {
                int tipoInformacion = 0;
                switch (tipoGrafico)
                {
                    case ConstantesSubasta.GraficoTotalSubir:
                    case ConstantesSubasta.GraficoUrsSubir:
                        tipoInformacion = ConstantesSubasta.SrestcodiSubir; break;

                    case ConstantesSubasta.GraficoTotalBajar:
                    case ConstantesSubasta.GraficoUrsBajar:
                        tipoInformacion = ConstantesSubasta.SrestcodiBajar; break;
                }

                List<CpMedicion48DTO> list = (new McpAppServicio()).ObtenerDatosEscenario(topSemFinal.Topcodi, fechaDeOferta, tipoInformacion);
                //List<CpMedicion48DTO> list = new List<CpMedicion48DTO>(); //CP110
                if (list.Any())
                {
                    CpMedicion48DTO data = list.First();

                    string etiq = "";
                    if (tipoInformacion == ConstantesSubasta.SrestcodiSubir) etiq = "Reserva Total a Subir";
                    if (tipoInformacion == ConstantesSubasta.SrestcodiBajar) etiq = "Reserva Total a Bajar";

                    Dato48 dato = new Dato48();
                    dato.Etiqueta = etiq;

                    for (int escenario = 1; escenario <= 48; escenario++)
                    {
                        decimal? valEsc = ((decimal?)data.GetType().GetProperty(ConstantesAppServicio.CaracterH + escenario).GetValue(data, null));

                        dato.GetType().GetProperty(ConstantesSubasta.CaracterV + escenario.ToString()).SetValue(dato, valEsc);
                    }
                    dato.ConInformacion = true;

                    regSalida = dato;
                }
            }

            return regSalida;
        }

        #endregion

        #region Bitacora

        /// <summary>
        /// Genera el listado de bitacora
        /// </summary>
        /// <param name="fechaIniOferta"></param>
        /// <param name="fechaFinOferta"></param>
        /// <returns></returns>
        public List<Bitacora> CargarListarBitacora(DateTime fechaIniOferta, DateTime fechaFinOferta)
        {
            List<Bitacora> listaBitacora = new List<Bitacora>();

            List<SmaActivacionOfertaDTO> listaTotalActivacionesActivasExistentesXRango = FactorySic.GetSmaActivacionOfertaRepository().ListarActivacionesPorRangoFechas(fechaIniOferta, fechaFinOferta);

            //Por dia solo debe haber un registro
            List<SmaActivacionOfertaDTO> listaActivacionUsar = new List<SmaActivacionOfertaDTO>();
            var gruposXdia = listaTotalActivacionesActivasExistentesXRango.GroupBy(x => x.Smapacfecha).ToList();
            foreach (var grupo in gruposXdia)
            {
                SmaActivacionOfertaDTO regDiaUltimo = grupo.OrderByDescending(x => x.Smapaccodi).First();
                if (regDiaUltimo != null)
                {
                    listaActivacionUsar.Add(regDiaUltimo);
                }
            }

            listaBitacora = ObtenerDatosBitacora(listaActivacionUsar);

            listaBitacora = listaBitacora.OrderBy(x => x.FechaOferta).ThenBy(x => x.Deficit).ThenBy(x => x.Horario).ToList();
            return listaBitacora;
        }

        /// <summary>
        /// Obtener los registos del reporte de bitacora
        /// </summary>
        /// <param name="listaActivaciones"></param>
        /// <returns></returns>
        public List<Bitacora> ObtenerDatosBitacora(List<SmaActivacionOfertaDTO> listaActivaciones)
        {
            List<Bitacora> lstSalida = new List<Bitacora>();

            foreach (SmaActivacionOfertaDTO activacionOfertaXDia in listaActivaciones)
            {
                string fechaDeOferta = activacionOfertaXDia.Smapacfecha.Value.ToString(ConstantesAppServicio.FormatoFecha);

                //obtener data
                List<SmaActivacionDataDTO> listaDatosXActivacion = FactorySic.GetSmaActivacionDataRepository().ObtenerPorActivacionesOferta(activacionOfertaXDia.Smapaccodi.ToString());
                List<SmaActivacionMotivoDTO> listaMotivosXActivacion = FactorySic.GetSmaActivacionMotivoRepository().ObtenerPorActivacionesOferta(activacionOfertaXDia.Smapaccodi.ToString());


                //Agrupo por tipo pestaña (Subi r y Bajar)
                var datosActivacionPorFechaYPestania = listaDatosXActivacion.GroupBy(x => x.Smaacdtiporeserva).ToList();

                foreach (var grupoDataXPestania in datosActivacionPorFechaYPestania)
                {
                    string tipoReserva = grupoDataXPestania.Key;

                    //obtengo valor de campo Deficit
                    string campoDeficit = "";
                    switch (tipoReserva)
                    {
                        case ConstantesSubasta.ReservaSubir: campoDeficit = "RSF para subir"; break;
                        case ConstantesSubasta.ReservaBajar: campoDeficit = "RSF para bajar"; break;
                    }

                    //obtengo los motivos para el tipo pestania
                    List<SmaActivacionMotivoDTO> lstMotivosActivacionXPeYFec = listaMotivosXActivacion.Where(x => x.Smaacmtiporeserva == tipoReserva).ToList();
                    List<int> codigos = lstMotivosActivacionXPeYFec.Where(x => x.Smammcodi != null).Select(x => x.Smammcodi.Value).ToList();
                    List<SmaMaestroMotivoDTO> lstMotivos = codigos.Any() ? GetByCriteriaSmaMaestroMotivos(string.Join(",", codigos)) : new List<SmaMaestroMotivoDTO>();

                    SmaActivacionDataDTO regDeficit = grupoDataXPestania.ToList().Where(x => x.Smaacdtipodato == ConstantesSubasta.DatoDeficitRSF).First();
                    SmaActivacionDataDTO regReducbanda = grupoDataXPestania.ToList().Where(x => x.Smaacdtipodato == ConstantesSubasta.DatoReduccionBanda).First();

                    //Obtengo los rangos (registros del reporte)
                    bool hayCambioRespectoEscAnterior;

                    decimal valAnteriorDeficit = -1;
                    decimal valAnteriorReduccion = -1;

                    bool rangoAbierto = false;
                    Bitacora regBitacora = new Bitacora();

                    for (int escenarioX = 1; escenarioX <= 48; escenarioX++)
                    {
                        decimal valDeficit = ((decimal?)regDeficit.GetType().GetProperty(ConstantesAppServicio.CaracterH + escenarioX).GetValue(regDeficit, null)).GetValueOrDefault(0);
                        decimal valReduccion = ((decimal?)regReducbanda.GetType().GetProperty(ConstantesAppServicio.CaracterH + escenarioX).GetValue(regReducbanda, null)).GetValueOrDefault(0);

                        bool escConDeficit = valDeficit != 0;
                        bool escConReduccion = valReduccion != 0;
                        bool valReduccionIgualAnterior = valReduccion == valAnteriorReduccion;
                        bool escAnteriorConDeficit = valAnteriorDeficit > 0;

                        hayCambioRespectoEscAnterior = (valDeficit != valAnteriorDeficit || valReduccion != valAnteriorReduccion);

                        decimal sumaVal = valDeficit + valReduccion;

                        if (hayCambioRespectoEscAnterior)
                        {
                            if (sumaVal > 0)
                            {
                                if (escConDeficit)
                                {
                                    if (valReduccionIgualAnterior)
                                    {
                                        if (escAnteriorConDeficit)
                                        {
                                            ////siguiente escenarioX. 
                                            if (escenarioX == 48 && rangoAbierto)
                                            {
                                                regBitacora.EscenarioFin = escenarioX;
                                                lstSalida.Add(regBitacora);
                                                //rangoAbierto = false;//no se cierra para que agregue deficit
                                            }
                                        }
                                        else
                                        {
                                            //si el rango esta abierto lo cierro
                                            if (rangoAbierto)
                                            {
                                                regBitacora.EscenarioFin = escenarioX - 1;
                                                lstSalida.Add(regBitacora);
                                                rangoAbierto = false;
                                            }

                                            regBitacora = new Bitacora();
                                            rangoAbierto = true;
                                            AgregarDatosARegistroBitacora(regBitacora, fechaDeOferta, lstMotivos, campoDeficit, valReduccion);

                                            regBitacora.EscenarioIni = escenarioX;
                                        }
                                    }
                                    else
                                    {
                                        //si el rango esta abierto lo cierro
                                        if (rangoAbierto)
                                        {
                                            regBitacora.EscenarioFin = escenarioX - 1;
                                            lstSalida.Add(regBitacora);
                                            rangoAbierto = false;
                                        }

                                        regBitacora = new Bitacora();
                                        rangoAbierto = true;
                                        AgregarDatosARegistroBitacora(regBitacora, fechaDeOferta, lstMotivos, campoDeficit, valReduccion);

                                        regBitacora.EscenarioIni = escenarioX;
                                    }

                                }
                                else
                                {
                                    //si el rango esta abierto lo cierro
                                    if (rangoAbierto)
                                    {
                                        regBitacora.EscenarioFin = escenarioX - 1;
                                        lstSalida.Add(regBitacora);
                                        rangoAbierto = false;
                                    }

                                    regBitacora = new Bitacora();
                                    rangoAbierto = true;
                                    AgregarDatosARegistroBitacora(regBitacora, fechaDeOferta, lstMotivos, campoDeficit, valReduccion);

                                    regBitacora.EscenarioIni = escenarioX;
                                }

                            }
                            else
                            {
                                //si el rango esta abierto lo cierro
                                if (rangoAbierto)
                                {
                                    regBitacora.EscenarioFin = escenarioX - 1;
                                    lstSalida.Add(regBitacora);
                                    rangoAbierto = false;
                                }
                            }
                        }
                        else
                        {
                            if (escenarioX == 48 && rangoAbierto)
                            {
                                regBitacora.EscenarioFin = escenarioX;
                                lstSalida.Add(regBitacora);
                                //rangoAbierto = false;//no se cierra para que agregue deficit
                            }
                        }

                        //siguiente escenarioX. Agrego valores de deficit para luego sacarle (min, max, prom)
                        if (rangoAbierto)
                        {
                            if (regBitacora.ValoresDeficit == null)
                                regBitacora.ValoresDeficit = new List<decimal>();

                            regBitacora.ValoresDeficit.Add(valDeficit);
                        }

                        valAnteriorDeficit = valDeficit;
                        valAnteriorReduccion = valReduccion;
                    }
                }
            }

            decimal? valNulo = null;
            //Formateo el listado
            List<RangoEscenario> lstRangosEscenario = ObtenerDatosEscenario();
            foreach (Bitacora regBitacora in lstSalida)
            {
                List<decimal> lstDeficit = regBitacora.ValoresDeficit;
                decimal sumDeficit = lstDeficit.Sum(x => x);
                decimal max = lstDeficit != null ? (lstDeficit.Any() ? lstDeficit.Max(x => x) : 0) : 0;
                decimal min = lstDeficit != null ? (lstDeficit.Any() ? lstDeficit.Min(x => x) : 0) : 0;
                decimal prom = lstDeficit != null ? (lstDeficit.Any() ? lstDeficit.Average(x => x) : 0) : 0;

                string motivos = "";
                if (regBitacora.ValoresMotivos != null)
                {
                    foreach (var motivo in regBitacora.ValoresMotivos)
                    {
                        motivos = motivos + "<p>" + motivo.Smammdescripcion.Trim() + "</p>";
                    }
                }

                regBitacora.HoraIni = lstRangosEscenario.Find(x => x.Escenario == regBitacora.EscenarioIni).HoraIni;
                regBitacora.HoraFin = lstRangosEscenario.Find(x => x.Escenario == regBitacora.EscenarioFin).HoraFin;
                regBitacora.Horario = regBitacora.HoraIni + " - " + regBitacora.HoraFin;
                regBitacora.Minimo = min == 0 ? valNulo : min;
                regBitacora.Maximo = max == 0 ? valNulo : max;
                regBitacora.Promedio = prom == 0 ? valNulo : prom;
                regBitacora.Motivos = sumDeficit != 0 ? motivos : "";
            }

            //Ordenamiento del listado
            lstSalida = lstSalida.OrderBy(x => x.FechaOferta).ThenBy(x => x.Deficit).ThenBy(x => x.Horario).ToList();

            return lstSalida;
        }

        /// <summary>
        /// Rellena campos a cierto registro de bitacora
        /// </summary>
        /// <param name="regBitacora"></param>
        /// <param name="fechaDeOferta"></param>
        /// <param name="lstMotivos"></param>
        /// <param name="campoDeficit"></param>
        /// <param name="valReduccion"></param>
        private void AgregarDatosARegistroBitacora(Bitacora regBitacora, string fechaDeOferta, List<SmaMaestroMotivoDTO> lstMotivos, string campoDeficit, decimal valReduccion)
        {
            decimal? valNulo = null;

            regBitacora.FechaOferta = fechaDeOferta;
            regBitacora.ValoresMotivos = lstMotivos;
            regBitacora.Deficit = campoDeficit;
            regBitacora.Banda = valReduccion == 0 ? valNulo : valReduccion;
        }

        /// <summary>
        /// Genera el archivo excel
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="existeDatos"></param>
        /// <returns></returns>
        public string GenerarArchivoExcelBitacora(string ruta, string pathLogo, DateTime fechaIni, DateTime fechaFin, out bool existeDatos)
        {
            existeDatos = false;

            List<Bitacora> lstRegistros = CargarListarBitacora(fechaIni, fechaFin);

            int result = DateTime.Compare(fechaIni, fechaFin);
            string nameFile = result == 0 ? ("BitacoraRSF_" + fechaIni.ToString(ConstantesAppServicio.FormatoFechaEjecutivo2)) : ("BitacoraRSF_" + fechaIni.ToString(ConstantesAppServicio.FormatoFechaEjecutivo2) + "-" + fechaFin.ToString(ConstantesAppServicio.FormatoFechaEjecutivo2));
            nameFile = nameFile + ".xlsx";
            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarExcelHojaBitacora(xlPackage, pathLogo, lstRegistros, fechaIni, fechaFin);
                xlPackage.Save();
            }

            return nameFile;
        }

        /// <summary>
        ///     Genera la hoja del archivo excel
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="pathLogo"></param>
        /// <param name="lstRegistrosTotales"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        private void GenerarExcelHojaBitacora(ExcelPackage xlPackage, string pathLogo, List<Bitacora> lstRegistrosTotales, DateTime fechaInicio, DateTime fechaFin)
        {
            //string EstadoEnvio = ObtenerDescripcionEstadoEnvioFT(estado, 1);
            string nameWS = "REPORTE";
            string titulo = "BITÁCORA DE ACTIVACIÓN DEL MECANISMO DE OFERTAS POR DEFECTO";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;

            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo, 120, 70);

            #region  Filtros y Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 4;

            int colIniFecha = colIniTitulo + 1;
            int rowIniFecha = rowIniTitulo + 2;

            int colIniTable = colIniTitulo + 1;
            int rowIniTabla = rowIniTitulo + 6;

            int colFechaOferta = colIniTable;
            int colDeficit = colIniTable + 1;
            int colHorario = colIniTable + 2;
            int colPromedio = colIniTable + 3;
            int colMinimo = colIniTable + 4;
            int colMaximo = colIniTable + 5;
            int colMotivos = colIniTable + 6;
            int colBanda = colIniTable + 7;
            int colUltima = colIniTable + 8;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            ws.Cells[rowIniFecha, colIniFecha].Value = "Fecha de consulta:";
            ws.Cells[rowIniFecha, colIniFecha + 1].Value = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaHora);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniFecha, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha, colIniFecha + 2, rowIniFecha, colIniFecha + 2, "Calibri", 10);

            ws.Cells[rowIniFecha + 1, colIniFecha].Value = "Desde:";
            ws.Cells[rowIniFecha + 1, colIniFecha + 1].Value = fechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha + 1, colIniFecha, rowIniFecha + 1, colIniFecha, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha + 1, colIniFecha, rowIniFecha + 1, colIniFecha + 1, "Calibri", 10);

            ws.Cells[rowIniFecha + 2, colIniFecha].Value = "Hasta:";
            ws.Cells[rowIniFecha + 2, colIniFecha + 1].Value = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniFecha + 2, colIniFecha, rowIniFecha + 2, colIniFecha, "Derecha");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha + 2, colIniFecha, rowIniFecha + 2, colIniFecha + 1, "Calibri", 10);

            ws.Row(rowIniTabla).Height = 20;
            ws.Cells[rowIniTabla, colFechaOferta].Value = "Fecha de Oferta";
            ws.Cells[rowIniTabla, colDeficit].Value = "Déficit";
            ws.Cells[rowIniTabla, colHorario].Value = "Horario";
            ws.Cells[rowIniTabla, colPromedio].Value = "Promedio";
            ws.Cells[rowIniTabla, colMinimo].Value = "Mínimo";
            ws.Cells[rowIniTabla, colMaximo].Value = "Máximo";
            ws.Cells[rowIniTabla, colMotivos].Value = "Motivo Activación Oferta por Defecto";
            ws.Cells[rowIniTabla, colBanda].Value = "Reducción de Banda a (MW)";

            //Estilos titulo
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo, "Calibri", 16);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colUltima);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colUltima, "Centro");

            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniFecha, colIniFecha, rowIniFecha, colIniFecha + 1, "Calibri", 10);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIniFecha, colIniFecha, rowIniFecha + 2, colIniFecha);

            //Estilos cabecera
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colFechaOferta, rowIniTabla, colUltima - 1, "Calibri", 11);
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colFechaOferta, rowIniTabla, colUltima - 1, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colFechaOferta, rowIniTabla, colUltima - 1, "Centro");
            UtilExcel.CeldasExcelColorFondo(ws, rowIniTabla, colFechaOferta, rowIniTabla, colUltima - 1, "#2980B9");
            UtilExcel.CeldasExcelColorTexto(ws, rowIniTabla, colFechaOferta, rowIniTabla, colUltima - 1, "#FFFFFF");
            UtilExcel.BorderCeldasHair(ws, rowIniTabla, colFechaOferta, rowIniTabla, colUltima - 1, "#000000");

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;
            int numDecimales = 2;

            foreach (var item in lstRegistrosTotales)
            {                
                var prom = item.Promedio;
                var min = item.Minimo;
                var max = item.Maximo;
                var banda = item.Banda;

                ws.Cells[rowData, colFechaOferta].Value = item.FechaOferta;
                ws.Cells[rowData, colDeficit].Value = item.Deficit.Trim();
                ws.Cells[rowData, colHorario].Value = item.Horario.Trim();
                ws.Cells[rowData, colPromedio].Value = prom;
                ws.Cells[rowData, colPromedio].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);
                ws.Cells[rowData, colMinimo].Value = min;
                ws.Cells[rowData, colMinimo].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);
                ws.Cells[rowData, colMaximo].Value = max;
                ws.Cells[rowData, colMaximo].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);
                ws.Cells[rowData, colMotivos].Value = item.Motivos != null ? (item.Motivos != "" ? (((item.Motivos.Substring(3)).Replace("</p>", "")).Replace("<p>", "\r\n")) : "") : "";
                ws.Cells[rowData, colBanda].Value = banda;
                ws.Cells[rowData, colBanda].Style.Numberformat.Format = FormatoNumDecimales(numDecimales);


                rowData++;
            }
            if (!lstRegistrosTotales.Any())
            {
                rowData++;
                ws.Cells[rowIniTabla + 1, colFechaOferta].Value = "No existen registros";
                UtilExcel.CeldasExcelAgrupar(ws, rowIniTabla + 1, colFechaOferta, rowData - 1, colBanda);
            }

            //Estilos registros
            UtilExcel.CeldasExcelWrapText(ws, rowIniTabla + 1, colFechaOferta, rowData - 1, colUltima - 1);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colFechaOferta, rowData - 1, colUltima - 1, "Calibri", 8);
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colFechaOferta, rowData - 1, colUltima - 1, "Centro");
            UtilExcel.BorderCeldasThin(ws, rowIniTabla + 1, colFechaOferta, rowData - 1, colUltima - 1, "#000000");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colFechaOferta, rowData - 1, colUltima - 1, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colDeficit, rowData - 1, colUltima - 1, "Centro");

            #endregion

            //filter
            //ws.Cells[rowIniTabla, colFechaOferta, rowIniTabla, colUltima - 1].AutoFilter = true;
            ws.Cells[rowIniTabla, colIniTable, rowData, colUltima - 1].AutoFitColumns();
            ws.Column(colDeficit).Width = 15;
            ws.Column(colHorario).Width = 15;
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        private string FormatoNumDecimales(int numDecimales)
        {
            string salida = "";
            switch (numDecimales)
            {
                case 1: salida = "#,#0.0"; break;
                case 2: salida = "#,##0.00"; break;
                case 3: salida = "#,###0.000"; break;
                case 4: salida = "#,####0.0000"; break;
                case 5: salida = "#,#####0.00000"; break;
                case 6: salida = "#,######0.000000"; break;
                case 7: salida = "#,#######0.0000000"; break;
                case 8: salida = "#,########0.00000000"; break;
                default:
                    break;
            }
            return salida;
        }

        #endregion

        #endregion

        #endregion
    }
}
