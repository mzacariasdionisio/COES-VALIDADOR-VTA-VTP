using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Servicios.Aplicacion.SistemasTransmision.Helper;
using System.Data;
using COES.Servicios.Aplicacion.Transferencias;

namespace COES.Servicios.Aplicacion.SistemasTransmision
{
    /// <summary>
    /// Clases con métodos del módulo SistemasTransmision
    /// </summary>
    public class SistemasTransmisionAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SistemasTransmisionAppServicio));

        #region Métodos Tabla ST_CENTRALGEN

        /// <summary>
        /// Inserta un registro de la tabla ST_CENTRALGEN
        /// </summary>
        public int SaveStCentralgen(StCentralgenDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetStCentralgenRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_CENTRALGEN
        /// </summary>
        public void UpdateStCentralgen(StCentralgenDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStCentralgenRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_CENTRALGEN
        /// </summary>
        public void DeleteStCentralgen(int stcntgcodi)
        {
            try
            {
                FactoryTransferencia.GetStCentralgenRepository().Delete(stcntgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_CENTRALGEN que tienen el mismo strecacodi
        /// </summary>
        public void DeleteStCentralgenVersion(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStCentralgenRepository().DeleteVersion(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_CENTRALGEN
        /// </summary>
        public StCentralgenDTO GetByIdStCentralgen(int stcntgcodi)
        {
            return FactoryTransferencia.GetStCentralgenRepository().GetById(stcntgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_CENTRALGEN
        /// </summary>
        public List<StCentralgenDTO> ListStCentralgens(int id)
        {
            return FactoryTransferencia.GetStCentralgenRepository().List(id);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StCentralgen
        /// </summary>
        public List<StCentralgenDTO> GetByCriteriaStCentralgens(int strecacodi)
        {
            return FactoryTransferencia.GetStCentralgenRepository().GetByCriteria(strecacodi);
        }

        /// <summary>
        /// Permite extraer todos los registros de la tabla StCentralgen para el reporte
        /// </summary>
        public List<StCentralgenDTO> GetByCriteriaStCentralgensReporte(int strecacodi)
        {
            return FactoryTransferencia.GetStCentralgenRepository().GetByCriteriaReporte(strecacodi);
        }

        /// <summary>
        /// Permite extraer un registros de la tabla StCentralgen filtrado por el nombre
        /// </summary>
        public StCentralgenDTO GetByCentNomb(string EquiNomb, int strecacodi)
        {
            return FactoryTransferencia.GetStCentralgenRepository().GetByCentNombre(EquiNomb, strecacodi);
        }


        #endregion

        #region Métodos Tabla ST_COMPENSACION

        /// <summary>
        /// Inserta un registro de la tabla ST_COMPENSACION
        /// </summary>
        public int SaveStCompensacion(StCompensacionDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetStCompensacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_COMPENSACION
        /// </summary>
        public void UpdateStCompensacion(StCompensacionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStCompensacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_COMPENSACION
        /// </summary>
        public void DeleteStCompensacion(int stcompcodi)
        {
            try
            {
                FactoryTransferencia.GetStCompensacionRepository().Delete(stcompcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_COMPENSACION que tienen el mismo strecacodi
        /// </summary>
        public void DeleteStCompensacionVersion(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStCompensacionRepository().DeleteVersion(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_COMPENSACION
        /// </summary>
        public StCompensacionDTO GetByIdStCompensacion(int stcompcodi)
        {
            return FactoryTransferencia.GetStCompensacionRepository().GetById(stcompcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_COMPENSACION
        /// </summary>
        public List<StCompensacionDTO> ListStCompensacions()
        {
            return FactoryTransferencia.GetStCompensacionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StCompensacion
        /// </summary>
        public List<StCompensacionDTO> GetByCriteriaStCompensacions(int strecacodi)
        {
            return FactoryTransferencia.GetStCompensacionRepository().GetByCriteria(strecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StCompensacion por sistrncodi
        /// </summary>
        public List<StCompensacionDTO> GetBySisTransStCompensacions(int sistrncodi)
        {
            return FactoryTransferencia.GetStCompensacionRepository().GetBySisTrans(sistrncodi);
        }

        /// <summary>
        /// Permite listar todos los registros en la tabla StCompensacion por sistrncodi
        /// </summary>
        public List<StCompensacionDTO> ListStCompensacionsPorID(int sistrncodi)
        {
            return FactoryTransferencia.GetStCompensacionRepository().ListStCompensacionsPorID(sistrncodi);
        }

        #endregion

        #region Métodos Tabla ST_COMPMENSUAL

        /// <summary>
        /// Inserta un registro de la tabla ST_COMPMENSUAL
        /// </summary>
        public int SaveStCompmensual(StCompmensualDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetStCompmensualRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_COMPMENSUAL
        /// </summary>
        public void UpdateStCompmensual(StCompmensualDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStCompmensualRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_COMPMENSUAL
        /// </summary>
        public void DeleteStCompmensual(int recacodi)
        {
            try
            {
                FactoryTransferencia.GetStCompmensualRepository().Delete(recacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_COMPMENSUAL
        /// </summary>
        public StCompmensualDTO GetByIdStCompmensual(int cmpmencodi)
        {
            return FactoryTransferencia.GetStCompmensualRepository().GetById(cmpmencodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_COMPMENSUAL
        /// </summary>
        public List<StCompmensualDTO> ListStCompmensuals()
        {
            return FactoryTransferencia.GetStCompmensualRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StCompmensual
        /// </summary>
        public List<StCompmensualDTO> GetByCriteriaStCompmensuals(int strecacodi)
        {
            return FactoryTransferencia.GetStCompmensualRepository().GetByCriteria(strecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StCompmensual que tienen el mismo strecacodi
        /// </summary>
        public List<StCompmensualDTO> ListByStCompMensualVersion(int strecacodi)
        {
            return FactoryTransferencia.GetStCompmensualRepository().ListByStCompMensualVersion(strecacodi);
        }

        #endregion

        #region Métodos Tabla ST_COMPMENSUALELE

        /// <summary>
        /// Inserta un registro de la tabla StCompmensualele
        /// </summary>
        public void SaveStCompmensualele(StCompmensualeleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStCompmensualeleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla StCompmensualele
        /// </summary>
        public void UpdateStCompmensualele(StCompmensualeleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStCompmensualeleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla StCompmensualele
        /// </summary>
        public void DeleteStCompmensualele(int cmpmelcodi)
        {
            try
            {
                FactoryTransferencia.GetStCompmensualeleRepository().Delete(cmpmelcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla StCompmensualele que tienen el mismo strecacodi
        /// </summary>
        public void DeleteStCompmensualEleVersion(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStCompmensualeleRepository().DeleteStCompmensualEleVersion(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla StCompmensualele
        /// </summary>
        public StCompmensualeleDTO GetByIdStCompmensualele(int cmpmencodi, int stcompcodi)
        {
            return FactoryTransferencia.GetStCompmensualeleRepository().GetById(cmpmencodi, stcompcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla StCompmensualele
        /// </summary>
        public List<StCompmensualeleDTO> ListStCompmensualeles()
        {
            return FactoryTransferencia.GetStCompmensualeleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StCompmensualele
        /// </summary>
        public List<StCompmensualeleDTO> GetByCriteriaStCompmensualeles(int strecacodi)
        {
            return FactoryTransferencia.GetStCompmensualeleRepository().GetByCriteria(strecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StCompmensualele filtrado por strecacodi, stcompcodi, stcntgcodi
        /// </summary>
        public StCompmensualeleDTO GetByIdCriteriaStCompMensualEle(int strecacodi, int stcompcodi, int stcntgcodi)
        {
            return FactoryTransferencia.GetStCompmensualeleRepository().GetByIdStCompMensualEle(strecacodi, stcompcodi, stcntgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla StCompmensualele filtrado por strecacodi
        /// </summary>
        public List<StCompmensualeleDTO> ListStCompMenElePorID(int Cmpmencodi)
        {
            return FactoryTransferencia.GetStCompmensualeleRepository().ListStCompMenElePorID(Cmpmencodi);
        }

        ///// <summary>
        ///// Permite listar todos los registros de la tabla StCompmensualele filtrado por stcompcodi
        ///// </summary>
        //public List<StCompmensualeleDTO> ListStCompMenElePorID(int stcompcodi)
        //{
        //    return FactoryTransferencia.GetStCompmensualeleRepository().ListStCompMenElePorID(stcompcodi);
        //}

        #endregion

        #region Métodos Tabla ST_DISTELECTRICA

        /// <summary>
        /// Inserta un registro de la tabla ST_DISTELECTRICA
        /// </summary>
        public int SaveStDistelectrica(StDistelectricaDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetStDistelectricaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_DISTELECTRICA
        /// </summary>
        public void UpdateStDistelectrica(StDistelectricaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStDistelectricaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_DISTELECTRICA
        /// </summary>
        public void DeleteStDistelectrica(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStDistelectricaRepository().Delete(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_DISTELECTRICA que tienen el mismo strecacodi
        /// </summary>
        public List<StDistelectricaDTO> ListByStDistElectricaVersion(int strecacodi)
        {
            return FactoryTransferencia.GetStDistelectricaRepository().GetByCriteriaVersion(strecacodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_DISTELECTRICA
        /// </summary>
        public StDistelectricaDTO GetByIdStDistelectrica(int dstelecodi)
        {
            return FactoryTransferencia.GetStDistelectricaRepository().GetById(dstelecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_DISTELECTRICA
        /// </summary>
        public List<StDistelectricaDTO> ListStDistelectricas()
        {
            return FactoryTransferencia.GetStDistelectricaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla ST_DISTELECTRICA
        /// </summary>
        public List<StDistelectricaDTO> GetByCriteriaStDistelectricas(int strecacodi)
        {
            return FactoryTransferencia.GetStDistelectricaRepository().GetByCriteria(strecacodi);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla ST_DISTANCIAELECTRICA
        /// </summary>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoStDistelectricas(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StDistelectricaDTO> ListaDistelectrica = this.GetByCriteriaStDistelectricas(strecacodi);
            List<StBarraDTO> ListaBarra = this.GetByCriteriaStBarra(strecacodi);
            if (formato == 1)
            {
                fileName = "ReporteDistanciasElectricas.xlsx";
                ExcelDocument.GenerarFormatoStDistelectrica(pathFile + fileName, EntidadRecalculo, ListaDistelectrica, ListaBarra);
            }

            return fileName;
        }

        #endregion

        #region Métodos Tabla ST_DISTELECTRICA_GENELE

        /// <summary>
        /// Inserta un registro de la tabla ST_DISTELECTRICA_GENELE
        /// </summary>
        public void SaveStDistelectricaGenele(StDistelectricaGeneleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStDistelectricaGeneleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_DISTELECTRICA_GENELE
        /// </summary>
        public void UpdateStDistelectricaGenele(StDistelectricaGeneleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStDistelectricaGeneleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_DISTELECTRICA_GENELE
        /// </summary>
        public void DeleteStDistelectricaGenele(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStDistelectricaGeneleRepository().Delete(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_DISTELECTRICA_GENELE
        /// </summary>
        public StDistelectricaGeneleDTO GetByIdStDistelectricaGenele(int degelecodi)
        {
            return FactoryTransferencia.GetStDistelectricaGeneleRepository().GetById(degelecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_DISTELECTRICA_GENELE
        /// </summary>
        public List<StDistelectricaGeneleDTO> ListStDistelectricaGeneles()
        {
            return FactoryTransferencia.GetStDistelectricaGeneleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StDistelectricaGenele
        /// </summary>
        public List<StDistelectricaGeneleDTO> GetByCriteriaStDistelectricaGeneles(int strecacodi, int Stcompcodi)
        {
            return FactoryTransferencia.GetStDistelectricaGeneleRepository().GetByCriteria(strecacodi, Stcompcodi);
        }//

        /// <summary>
        /// Permite realizar búsquedas en la tabla StDistelectricaGenele filtrado por stcntgcodi y stcompcodi
        /// </summary>
        public StDistelectricaGeneleDTO GetByIdCriteriaStDistGene(int stcntgcodi, int stcompcodi)
        {
            return FactoryTransferencia.GetStDistelectricaGeneleRepository().GetByIdCriteriaStDistGene(stcntgcodi, stcompcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StDistelectricaGenele filtrado por strecacodi
        /// </summary>
        public List<StDistelectricaGeneleDTO> GetByCriteriaStDistelectricaGenelesReporte(int strecacodi)
        {
            return FactoryTransferencia.GetStDistelectricaGeneleRepository().GetByIdCriteriaStDistGeneReporte(strecacodi);
        }

        #endregion

        #region Métodos Tabla ST_DSTELE_BARRA

        /// <summary>
        /// Inserta un registro de la tabla ST_DSTELE_BARRA
        /// </summary>
        public void SaveStDsteleBarra(StDsteleBarraDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStDsteleBarraRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_DSTELE_BARRA
        /// </summary>
        public void UpdateStDsteleBarra(StDsteleBarraDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStDsteleBarraRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_DSTELE_BARRA
        /// </summary>
        public void DeleteStDsteleBarra(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStDsteleBarraRepository().Delete(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_DSTELE_BARRA
        /// </summary>
        public StDsteleBarraDTO GetByIdStDsteleBarra(int dstelecodi, int barrcodi)
        {
            return FactoryTransferencia.GetStDsteleBarraRepository().GetById(dstelecodi, barrcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_DSTELE_BARRA filtrado por strecacodi, barr1, barr2
        /// </summary>
        public StDsteleBarraDTO GetByCriteriosStDsteleBarra(int strecacodi, int barr1, int barr2)
        {
            return FactoryTransferencia.GetStDsteleBarraRepository().GetByCriterios(strecacodi, barr1, barr2);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_DSTELE_BARRA
        /// </summary>
        public List<StDsteleBarraDTO> ListStDsteleBarras()
        {
            return FactoryTransferencia.GetStDsteleBarraRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StDsteleBarra
        /// </summary>
        public List<StDsteleBarraDTO> GetByCriteriaStDsteleBarras()
        {
            return FactoryTransferencia.GetStDsteleBarraRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StDsteleBarra filtrado por strecacodi
        /// </summary>
        public List<StDsteleBarraDTO> ListStDistEleBarrPorID(int strecacodi)
        {
            return FactoryTransferencia.GetStDsteleBarraRepository().ListStDistEleBarrPorID(strecacodi);
        }

        #endregion

        #region Métodos Tabla ST_ELEMENTO_COMPENSADO

        /// <summary>
        /// Inserta un registro de la tabla ST_ELEMENTO_COMPENSADO
        /// </summary>
        public void SaveStElementoCompensado(StElementoCompensadoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStElementoCompensadoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_ELEMENTO_COMPENSADO
        /// </summary>
        public void UpdateStElementoCompensado(StElementoCompensadoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStElementoCompensadoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_ELEMENTO_COMPENSADO 
        /// </summary>
        public void DeleteStElementoCompensado(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStElementoCompensadoRepository().Delete(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_ELEMENTO_COMPENSADO
        /// </summary>
        public StElementoCompensadoDTO GetByIdStElementoCompensado(int elecmpcodi)
        {
            return FactoryTransferencia.GetStElementoCompensadoRepository().GetById(elecmpcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_ELEMENTO_COMPENSADO
        /// </summary>
        public List<StElementoCompensadoDTO> ListStElementoCompensados()
        {
            return FactoryTransferencia.GetStElementoCompensadoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StElementoCompensado
        /// </summary>
        public List<StElementoCompensadoDTO> GetByCriteriaStElementoCompensados()
        {
            return FactoryTransferencia.GetStElementoCompensadoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla ST_ENERGIA

        /// <summary>
        /// Inserta un registro de la tabla ST_ENERGIA
        /// </summary>
        public void SaveStEnergia(StEnergiaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStEnergiaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_ENERGIA
        /// </summary>
        public void UpdateStEnergia(StEnergiaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStEnergiaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_ENERGIA
        /// </summary>
        public void DeleteStEnergia(int stenrgcodi)
        {
            try
            {
                FactoryTransferencia.GetStEnergiaRepository().Delete(stenrgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_ENERGIA que tienen el mismo strecacodi
        /// </summary>
        public List<StEnergiaDTO> ListByStEnergiaVersion(int strecacodi)
        {
            return FactoryTransferencia.GetStEnergiaRepository().ListByStEnergiaVersion(strecacodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_ENERGIA
        /// </summary>
        public StEnergiaDTO GetByIdStEnergia(int stenrgcodi)
        {
            return FactoryTransferencia.GetStEnergiaRepository().GetById(stenrgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_ENERGIA
        /// </summary>
        public List<StEnergiaDTO> ListStEnergias()
        {
            return FactoryTransferencia.GetStEnergiaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StEnergia
        /// </summary>
        public List<StEnergiaDTO> GetByCriteriaStEnergias(int strecacodi)
        {
            return FactoryTransferencia.GetStEnergiaRepository().GetByCriteria(strecacodi);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla ST_ENERGIA
        /// </summary>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoStEnergias(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StEnergiaDTO> ListaEnergiaNetas = this.GetByCriteriaStEnergias(strecacodi);

            if (formato == 1)
            {
                fileName = "ReporteEnergiasNetas.xlsx";
                ExcelDocument.GenerarFormatoStEnergiasNetas(pathFile + fileName, EntidadRecalculo, ListaEnergiaNetas);
            }

            return fileName;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_ENERGIA filtrado por stcntgcodi
        /// </summary>
        public StEnergiaDTO GetByCentralCodiStEnergia(int strecacodi, int stcntgcodi)
        {
            return FactoryTransferencia.GetStEnergiaRepository().GetByCentralCodi(strecacodi, stcntgcodi);
        }


        #endregion

        #region Métodos Tabla ST_FACTOR

        /// <summary>
        /// Inserta un registro de la tabla ST_FACTOR
        /// </summary>
        public void SaveStFactor(StFactorDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStFactorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_FACTOR
        /// </summary>
        public void UpdateStFactor(StFactorDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStFactorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_FACTOR
        /// </summary>
        public void DeleteStFactor(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStFactorRepository().Delete(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_FACTOR que tienen el mismo strecacodi
        /// </summary>
        public void DeleteStFactorVersion(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStFactorRepository().DeleteVersion(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_FACTOR
        /// </summary>
        public StFactorDTO GetByIdStFactor(int stfactcodi)
        {
            return FactoryTransferencia.GetStFactorRepository().GetById(stfactcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_FACTOR
        /// </summary>
        public List<StFactorDTO> ListStFactors()
        {
            return FactoryTransferencia.GetStFactorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StFactor
        /// </summary>
        public List<StFactorDTO> GetByCriteriaStFactors(int strecacodi)
        {
            return FactoryTransferencia.GetStFactorRepository().GetByCriteria(strecacodi);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla ST_FACTOR
        /// </summary>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoStFactor(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StFactorDTO> ListaFactorActualizacion = this.GetByCriteriaStFactors(strecacodi);

            if (formato == 1)
            {
                fileName = "ReporteFactorActualizacion.xlsx";
                ExcelDocument.GenerarFormatoStFactor(pathFile + fileName, EntidadRecalculo, ListaFactorActualizacion);
            }

            return fileName;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_FACTOR filtrado por sistrncodi
        /// </summary>
        public StFactorDTO GetByCriteriaStFactorsSistema(int sistrncodi)
        {
            return FactoryTransferencia.GetStFactorRepository().GetBySisTrans(sistrncodi);
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_CENTRALGEN que tienen el mismo strecacodi
        /// </summary>
        public List<StFactorDTO> ListByStFactorVersion(int strecacodi)
        {
            return FactoryTransferencia.GetStFactorRepository().ListByStFactorVersion(strecacodi);
        }

        #endregion

        #region Métodos Tabla ST_FACTORPAGO

        /// <summary>
        /// Inserta un registro de la tabla ST_FACTORPAGO
        /// </summary>
        public void SaveStFactorpago(StFactorpagoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStFactorpagoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_FACTORPAGO
        /// </summary>
        public void UpdateStFactorpago(StFactorpagoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStFactorpagoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_FACTORPAGO 
        /// </summary>
        public void DeleteStFactorpago(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStFactorpagoRepository().Delete(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_FACTORPAGO
        /// </summary>
        public StFactorpagoDTO GetByIdStFactorpago(int facpagcodi)
        {
            return FactoryTransferencia.GetStFactorpagoRepository().GetById(facpagcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_FACTORPAGO a través de sus FK
        /// </summary>
        private StFactorpagoDTO GetByIdStFactorpagoFK(int strecacodi, int stcntgcodi, int stcompcodi)
        {
            return FactoryTransferencia.GetStFactorpagoRepository().GetByIdFK(strecacodi, stcntgcodi, stcompcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_FACTORPAGO
        /// </summary>
        public List<StFactorpagoDTO> ListStFactorpagos(int strecacodi, int stcompcodi)
        {
            return FactoryTransferencia.GetStFactorpagoRepository().List(strecacodi, stcompcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StFactorpago filtrado por strecacodi, stcompcodi y facpagreajuste = 1
        /// </summary>
        public List<StFactorpagoDTO> GetByCriteriaStFactorpagos(int strecacodi, int stcompcodi)
        {
            return FactoryTransferencia.GetStFactorpagoRepository().GetByCriteria(strecacodi, stcompcodi);
        }

        /// <summary>
        /// Permite listar los Factores de Pago Inicial en la tabla StFactorpago filtrado por strecacodi
        /// </summary>
        private List<StFactorpagoDTO> GetByCriteriaStFactorpagosInicialReporte(int strecacodi)
        {
            return FactoryTransferencia.GetStFactorpagoRepository().GetByCriteriaInicialReporte(strecacodi);
        }

        /// <summary>
        /// Permite listar los Factores de Pago Final en la tabla StFactorpago filtrado por strecacodi y facpagreajuste = 1
        /// </summary>
        public List<StFactorpagoDTO> GetByCriteriaStFactorpagosReporte(int strecacodi)
        {
            return FactoryTransferencia.GetStFactorpagoRepository().GetByCriteriaReporte(strecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StFactorpago filtrado por strecacodi
        /// </summary>
        public List<StFactorpagoDTO> GetByCriteriaStFactorpagosReporteFactorParticipacion(int strecacodi)
        {
            return FactoryTransferencia.GetStFactorpagoRepository().GetByCriteriaReporteFactorPago(strecacodi);
        }

        #endregion

        #region Métodos Tabla ST_PAGOASIGNADO

        /// <summary>
        /// Inserta un registro de la tabla ST_PAGOASIGNADO
        /// </summary>
        public void SaveStPagoasignado(StPagoasignadoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStPagoasignadoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_PAGOASIGNADO
        /// </summary>
        public void UpdateStPagoasignado(StPagoasignadoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStPagoasignadoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_PAGOASIGNADO
        /// </summary>
        public void DeleteStPagoasignado(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStPagoasignadoRepository().Delete(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Elimina todos los registros de la tabla ST_PAGOASIGNADO que son iguales a stcompcodi
        /// </summary>
        public void DeleteStPagoasignadoByCompensacion(int stcompcodi)
        {
            try
            {
                FactoryTransferencia.GetStPagoasignadoRepository().DeleteByCompensacion(stcompcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_PAGOASIGNADO
        /// </summary>
        public StPagoasignadoDTO GetByIdStPagoasignado(int facpagcodi)
        {
            return FactoryTransferencia.GetStPagoasignadoRepository().GetById(facpagcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_PAGOASIGNADO
        /// </summary>
        public List<StPagoasignadoDTO> ListStPagoasignados(int strecacodi)
        {
            return FactoryTransferencia.GetStPagoasignadoRepository().List(strecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla ST_PAGOASIGNADO
        /// </summary>
        public StPagoasignadoDTO GetByCriteriaStPagoasignados(int Stcntgcodi, int Stcompcodi)
        {
            return FactoryTransferencia.GetStPagoasignadoRepository().GetByCriteria(Stcntgcodi, Stcompcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla ST_PAGOASIGNADO para mostrar en el reporte
        /// </summary>
        public List<StPagoasignadoDTO> GetByCriteriaStPagoasignadoReporte(int strecacodi)
        {
            return FactoryTransferencia.GetStPagoasignadoRepository().GetByCriteriaReporte(strecacodi);
        }

        /// <summary>
        /// Permite listar las empresas cuyos generadores van a recibir un pago
        /// </summary>
        public List<StPagoasignadoDTO> ListStPagoasignadosEmpresaGeneradores(int strecacodi)
        {
            return FactoryTransferencia.GetStPagoasignadoRepository().ListEmpresaGeneradores(strecacodi);
        }

        /// <summary>
        /// Permite listar las empresas cuyos sistemas efectuaran un pago
        /// </summary>
        public List<StPagoasignadoDTO> ListStPagoasignadosEmpresaSistemas(int strecacodi)
        {
            return FactoryTransferencia.GetStPagoasignadoRepository().ListEmpresaSistemas(strecacodi);
        }

        /// <summary>
        /// Permite calcular el pago de una empresa/sistema a un generador
        /// </summary>
        public decimal GetPagoStPagoasignadosGeneradorXSistema(int strecacodi, int genemprcodi, int sisemprecodi, int sistrncodi)
        {
            return FactoryTransferencia.GetStPagoasignadoRepository().GetPagoGeneradorXSistema(strecacodi, genemprcodi, sisemprecodi, sistrncodi);
        }

        #endregion

        #region Métodos Tabla ST_GENERADOR

        /// <summary>
        /// Inserta un registro de la tabla ST_GENERADOR
        /// </summary>
        public int SaveStGenerador(StGeneradorDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetStGeneradorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_GENERADOR
        /// </summary>
        public void UpdateStGenerador(StGeneradorDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStGeneradorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_GENERADOR
        /// </summary>
        public void DeleteStGenerador(int stgenrcodi)
        {
            try
            {
                FactoryTransferencia.GetStGeneradorRepository().Delete(stgenrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_GENERADOR que tienen el mismo strecacodi
        /// </summary>
        public void DeleteStGeneradorVersion(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStGeneradorRepository().DeleteVersion(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_GENERADOR
        /// </summary>
        public StGeneradorDTO GetByIdStGenerador(int stgenrcodi)
        {
            return FactoryTransferencia.GetStGeneradorRepository().GetById(stgenrcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_GENERADOR
        /// </summary>
        public List<StGeneradorDTO> ListStGeneradors(int strecacodi)
        {
            return FactoryTransferencia.GetStGeneradorRepository().List(strecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StGenerador
        /// </summary>
        public List<StGeneradorDTO> GetByCriteriaStGeneradors()
        {
            return FactoryTransferencia.GetStGeneradorRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener la lista de todos los registro de la tabla ST_CENTRALGEN que tienen el mismo strecacodi
        /// </summary>
        public List<StGeneradorDTO> ListByStGeneradorVersion(int strecacodi)
        {
            return FactoryTransferencia.GetStGeneradorRepository().ListByStGeneradorVersion(strecacodi);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de los generadores
        /// </summary>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarListaEmpresasGeneradoras(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StGeneradorDTO> ListaGeneradores = this.ListByStGeneradorReporte(strecacodi);

            if (formato == 1)
            {
                fileName = "ListaGeneradorCentralBarra.xlsx";
                ExcelDocument.ListaEmpresasGeneradoras(pathFile + fileName, EntidadRecalculo, ListaGeneradores);
            }

            return fileName;
        }

        /// <summary>
        /// Permite obtener la lista de todos los registro de la tabla ST_GENERADOR, EMPRESA, CENTRAL Y EQUIPO  que tienen el mismo strecacodi
        /// </summary>
        private List<StGeneradorDTO> ListByStGeneradorReporte(int strecacodi)
        {
            return FactoryTransferencia.GetStGeneradorRepository().ListByStGeneradorReporte(strecacodi);
        }
        #endregion

        #region Métodos Tabla ST_PERIODO

        /// <summary>
        /// Inserta un registro de la tabla ST_PERIODO
        /// </summary>
        public void SaveStPeriodo(StPeriodoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStPeriodoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_PERIODO
        /// </summary>
        public void UpdateStPeriodo(StPeriodoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStPeriodoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_PERIODO
        /// </summary>
        public void DeleteStPeriodo(int stpercodi)
        {
            try
            {
                FactoryTransferencia.GetStPeriodoRepository().Delete(stpercodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_PERIODO
        /// </summary>
        public StPeriodoDTO GetByIdStPeriodo(int stpercodi)
        {
            return FactoryTransferencia.GetStPeriodoRepository().GetById(stpercodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_PERIODO
        /// </summary>
        public List<StPeriodoDTO> ListStPeriodos()
        {
            return FactoryTransferencia.GetStPeriodoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StPeriodo
        /// </summary>
        public List<StPeriodoDTO> GetByCriteriaStPeriodos(string nombre)
        {
            return FactoryTransferencia.GetStPeriodoRepository().GetByCriteria(nombre);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_PERIODO del periodo anterior
        /// </summary>
        public StPeriodoDTO BuscarPeriodoAnterior(int stpercodi)
        {
            return FactoryTransferencia.GetStPeriodoRepository().GetByIdPeriodoAnterior(stpercodi);
        }

        #endregion

        #region Métodos Tabla ST_RECALCULO

        /// <summary>
        /// Inserta un registro de la tabla ST_RECALCULO
        /// </summary>
        public int SaveStRecalculo(StRecalculoDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetStRecalculoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_RECALCULO
        /// </summary>
        public void UpdateStRecalculo(StRecalculoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStRecalculoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_RECALCULO
        /// </summary>
        public void DeleteStRecalculo(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStRecalculoRepository().Delete(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_RECALCULO
        /// </summary>
        public StRecalculoDTO GetByIdStRecalculo(int strecacodi)
        {
            return FactoryTransferencia.GetStRecalculoRepository().GetById(strecacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_RECALCULO
        /// </summary>
        public List<StRecalculoDTO> ListStRecalculos(int id)
        {
            return FactoryTransferencia.GetStRecalculoRepository().List(id);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StRecalculo
        /// </summary>
        public List<StRecalculoDTO> GetByCriteriaStRecalculos()
        {
            return FactoryTransferencia.GetStRecalculoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StRecalculo filtrado por stpercodi
        /// </summary>
        public List<StRecalculoDTO> ListByPericodiRecalculo(int stpercodi)
        {
            return FactoryTransferencia.GetStRecalculoRepository().ListByStPericodi(stpercodi);
        }

        /// <summary>
        /// Permite obtener un registro de tabla StRecalculo filtrado por stpercodi y strecacodi
        /// </summary>
        public StRecalculoDTO GetByIdStRecalculoView(int stpericodi, int strecacodi)
        {
            return FactoryTransferencia.GetStRecalculoRepository().GetByIdView(stpericodi, strecacodi);
        }

        #endregion

        #region Métodos Tabla ST_RESPAGO

        /// <summary>
        /// Inserta un registro de la tabla ST_RESPAGO
        /// </summary>
        public int SaveStRespago(StRespagoDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetStRespagoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_RESPAGO
        /// </summary>
        public void UpdateStRespago(StRespagoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStRespagoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_RESPAGO
        /// </summary>
        public void DeleteStRespago(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStRespagoRepository().Delete(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_RESPAGO
        /// </summary>
        public StRespagoDTO GetByIdStRespago(int respagcodi)
        {
            return FactoryTransferencia.GetStRespagoRepository().GetById(respagcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_RESPAGO
        /// </summary>
        public List<StRespagoDTO> ListStRespagos()
        {
            return FactoryTransferencia.GetStRespagoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StRespago
        /// </summary>
        public List<StRespagoDTO> GetByCriteriaStRespagos(int recacodi)
        {
            return FactoryTransferencia.GetStRespagoRepository().GetByCriteria(recacodi);
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_CENTRALGEN que tienen el mismo strecacodi
        /// </summary>
        public List<StRespagoDTO> ListByStRespagoVersion(int strecacodi)
        {
            return FactoryTransferencia.GetStRespagoRepository().ListByStRespagoVersion(strecacodi);
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_CENTRALGEN que tienen el mismo strecacodi
        /// </summary>
        public List<StRespagoDTO> GetByCriteriaStRespagosElemento(int strecacodi, int stcompcodi)
        {
            return FactoryTransferencia.GetStRespagoRepository().GetByCodElem(strecacodi, stcompcodi);
        }

        #endregion

        #region Métodos Tabla ST_RESPAGOELE

        /// <summary>
        /// Inserta un registro de la tabla ST_RESPAGOELE
        /// </summary>
        public void SaveStRespagoele(StRespagoeleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStRespagoeleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_RESPAGOELE
        /// </summary>
        public void UpdateStRespagoele(StRespagoeleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStRespagoeleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_RESPAGOELE
        /// </summary>
        public void DeleteStRespagoele(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStRespagoeleRepository().Delete(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_RESPAGOELE que tienen el mismo strecacodi
        /// </summary>
        public void DeleteStRespagoEleVersion(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStRespagoeleRepository().DeleteStRespagoEleVersion(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_RESPAGOELE
        /// </summary>
        public StRespagoeleDTO GetByIdStRespagoele(int respagcodi, int stcompcodi)
        {
            return FactoryTransferencia.GetStRespagoeleRepository().GetById(respagcodi, stcompcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_RESPAGOELE
        /// </summary>
        public List<StRespagoeleDTO> ListStRespagoeles()
        {
            return FactoryTransferencia.GetStRespagoeleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StRespagoele
        /// </summary>
        public List<StRespagoeleDTO> GetByCriteriaStRespagoeles()
        {
            return FactoryTransferencia.GetStRespagoeleRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StRespagoele filtrado por strecacodi
        /// </summary>
        public List<StRespagoeleDTO> ListStRespagElePorID(int strecacodi)
        {
            return FactoryTransferencia.GetStRespagoeleRepository().ListStRespagElePorID(strecacodi);
        }


        #endregion

        #region Métodos Tabla ST_SISTEMATRANS

        /// <summary>
        /// Inserta un registro de la tabla ST_SISTEMATRANS
        /// </summary>
        public int SaveStSistematrans(StSistematransDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetStSistematransRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_SISTEMATRANS
        /// </summary>
        public void UpdateStSistematrans(StSistematransDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStSistematransRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_SISTEMATRANS
        /// </summary>
        public void DeleteStSistematrans(int sistrncodi)
        {
            try
            {
                FactoryTransferencia.GetStSistematransRepository().Delete(sistrncodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_SISTEMATRANS que tienen el mismo strecacodi
        /// </summary>
        public void DeleteStSistematransVersion(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStSistematransRepository().DeleteVersion(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_SISTEMATRANS
        /// </summary>
        public StSistematransDTO GetByIdStSistematrans(int sistrncodi)
        {
            return FactoryTransferencia.GetStSistematransRepository().GetById(sistrncodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_SISTEMATRANS
        /// </summary>
        public List<StSistematransDTO> ListStSistematranss(int strecacodi)
        {
            return FactoryTransferencia.GetStSistematransRepository().List(strecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StSistematrans
        /// </summary>
        public List<StSistematransDTO> GetByCriteriaStSistematranss(int recacodi)
        {
            return FactoryTransferencia.GetStSistematransRepository().GetByCriteria(recacodi);
        }

        /// <summary>
        /// Lista todos los registros de StSistematrans filtrado por strecacodi
        /// </summary>
        public List<StSistematransDTO> ListByStSistemaTransVersion(int recacodi)
        {
            return FactoryTransferencia.GetStSistematransRepository().ListByStSistemaTransVersion(recacodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_SISTEMATRANS filtrado por SisTransNomb
        /// </summary>
        public StSistematransDTO GetBySisTransNomb(int strecacodi, string SisTransNomb)
        {
            return FactoryTransferencia.GetStSistematransRepository().GetBySisTransNomb(strecacodi, SisTransNomb);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de los sistemas de transmisión
        /// </summary>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarListaSistemasTransmision(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StSistematransDTO> ListaSistemas = this.ListByStSistemaTransReporte(strecacodi);

            if (formato == 1)
            {
                fileName = "ListaSistemasTransmision.xlsx";
                ExcelDocument.ListaSistemasTransmision(pathFile + fileName, EntidadRecalculo, ListaSistemas);
            }

            return fileName;
        }

        /// <summary>
        /// Permite obtener la lista de todos los registro de la tabla ST_GENERADOR, EMPRESA, CENTRAL Y EQUIPO  que tienen el mismo strecacodi
        /// </summary>
        private List<StSistematransDTO> ListByStSistemaTransReporte(int strecacodi)
        {
            return FactoryTransferencia.GetStSistematransRepository().ListByStSistemaTransReporte(strecacodi);
        }

        #endregion

        #region Métodos Tabla ST_TRANSMISOR

        /// <summary>
        /// Inserta un registro de la tabla ST_TRANSMISOR
        /// </summary>
        public void SaveStTransmisor(StTransmisorDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStTransmisorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_TRANSMISOR
        /// </summary>
        public void UpdateStTransmisor(StTransmisorDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStTransmisorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_TRANSMISOR
        /// </summary>
        public void DeleteStTransmisor(int stranscodi)
        {
            try
            {
                FactoryTransferencia.GetStTransmisorRepository().Delete(stranscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_TRANSMISOR que tienen el mismo strecacodi
        /// </summary>
        public void DeleteStTransmisorVersion(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStTransmisorRepository().DeleteVersion(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_TRANSMISOR
        /// </summary>
        public StTransmisorDTO GetByIdStTransmisor(int stranscodi)
        {
            return FactoryTransferencia.GetStTransmisorRepository().GetById(stranscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_TRANSMISOR
        /// </summary>
        public List<StTransmisorDTO> ListStTransmisors(int strecacodi)
        {
            return FactoryTransferencia.GetStTransmisorRepository().List(strecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla StTransmisor
        /// </summary>
        public List<StTransmisorDTO> GetByCriteriaStTransmisors()
        {
            return FactoryTransferencia.GetStTransmisorRepository().GetByCriteria();
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_CENTRALGEN que tienen el mismo strecacodi
        /// </summary>
        public List<StTransmisorDTO> ListByStTransmisorVersion(int strecacodi)
        {
            return FactoryTransferencia.GetStTransmisorRepository().ListByStTransmisorVersion(strecacodi);
        }

        #endregion

        #region Métodos Tabla ST_BARRA

        /// <summary>
        /// Inserta un registro de la tabla ST_BARRA
        /// </summary>
        public void SaveStBarra(StBarraDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStBarraRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ST_BARRA
        /// </summary>
        public void UpdateStBarra(StBarraDTO entity)
        {
            try
            {
                FactoryTransferencia.GetStBarraRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Elimina un registro de la tabla ST_BARRA
        /// </summary>
        public void DeleteStBarra(int stranscodi)
        {
            try
            {
                FactoryTransferencia.GetStBarraRepository().Delete(stranscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_BARRA
        /// </summary>
        public void DeleteDstEleDet(int barrcodi, int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStBarraRepository().DeleteDstEleDet(barrcodi, strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_BARRA que tienen el mismo strecacodi
        /// </summary>
        public void DeleteStBarraVersion(int strecacodi)
        {
            try
            {
                FactoryTransferencia.GetStBarraRepository().DeleteVersion(strecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ST_BARRA
        /// </summary>
        public List<StBarraDTO> ListStBarra(int strecacodi)
        {
            return FactoryTransferencia.GetStBarraRepository().List(strecacodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ST_BARRA
        /// </summary>
        public StBarraDTO GetByIdStBarra(int stbarrcodi)
        {
            return FactoryTransferencia.GetStBarraRepository().GetById(stbarrcodi);
        }

        /// <summary>
        /// Permite obtener un registro por criterio de la tabla ST_BARRA
        /// </summary>
        public List<StBarraDTO> GetByCriteriaStBarra(int strecacodi)
        {
            return FactoryTransferencia.GetStBarraRepository().GetByCriteria(strecacodi);
        }

        /// <summary>
        /// Elimina todos los registro de la tabla ST_CENTRALGEN que tienen el mismo strecacodi
        /// </summary>
        public List<StBarraDTO> ListByStBarraVersion(int strecacodi)
        {
            return FactoryTransferencia.GetStBarraRepository().ListByStBarraVersion(strecacodi);
        }

        #endregion

        #region Métodos Genelares

        /// <summary>
        /// Procedimiento que se encarga de ejecutar el procedimiento del calculo de SST y SCT
        /// </summary>
        /// <param name="stpercodi">Periodo</param>
        /// <param name="strecacodi">Version</param>
        /// <param name="suser">Usuario conectado</param>
        public string ProcesarCalculo(int stpercodi, int strecacodi, string suser)
        {
            string sResultado = "1";
            try
            {
                //Traemos la entidad de la versión de recalculo
                StRecalculoDTO EntidadRecalculo = GetByIdStRecalculo(strecacodi);
                //Limpiamos Todos los calculos anteriores de la Version en Acción
                DeleteStDistelectricaGenele(strecacodi);
                DeleteStElementoCompensado(strecacodi);
                DeleteStFactorpago(strecacodi);
                DeleteStPagoasignado(strecacodi);
                decimal dPorcentajeMinimo = Convert.ToDecimal(0.01);

                #region PASO 1: Se determina el monto mensual a compensar al elemento l:   Ml’  = Ml x Fs
                //Traemos las lista de Elementos sin repeticiones, de la versión
                List<StCompensacionDTO> ListaElementos = GetByCriteriaStCompensacions(strecacodi);
                if (ListaElementos != null && ListaElementos.Count != 0)
                {
                    foreach (StCompensacionDTO dtoElemento in ListaElementos)
                    {
                        //if (dtoElemento.Stcompcodelemento.Equals("REP1-027"))
                        //{
                        //    string A = "saltar linea";
                        //}

                        //Para cada elemento
                        int iStcompcodi = dtoElemento.Stcompcodi;

                        #region PASO 2: Se realiza un barrido de todos los generadores g
                        double dDgl = 0; //distancia eléctrica entre g y l
                        decimal dEgDglTotal = 0; //Denominador de la formula (∑_(p=1)^ngen(Eg/Dgl) )
                        List<StRespagoDTO> ListaResponsabilidadPagoTodos = GetByCriteriaStRespagos(strecacodi);
                        foreach (StRespagoDTO dtoRespPago in ListaResponsabilidadPagoTodos)
                        {   //Se determina la distancia eléctrica entre g y l:  Dgl
                            //Buscamos la barra que tiene asignada este registro
                            StCentralgenDTO EntidadCentralesxGeneradore = GetByIdStCentralgen(dtoRespPago.Stcntgcodi);
                            //g (w)
                            int gWbarrcodi = EntidadCentralesxGeneradore.Barrcodi;
                            //las dos barras de elemento l (m y n)
                            int lMbarrcodi = dtoElemento.Barrcodi1;
                            int lNbarrcodi = dtoElemento.Barrcodi2;
                            //Buscando las distancias Zwm  y Zwn 
                            StDsteleBarraDTO dtoZwm = GetByCriteriosStDsteleBarra(strecacodi, gWbarrcodi, lMbarrcodi);
                            StDsteleBarraDTO dtoZwn = GetByCriteriosStDsteleBarra(strecacodi, gWbarrcodi, lNbarrcodi);
                            if (dtoZwm == null || dtoZwn == null)
                            {
                                BarraDTO dtoBarra1 = (new BarraAppServicio()).GetByIdBarra(lMbarrcodi);
                                BarraDTO dtoBarra2 = (new BarraAppServicio()).GetByIdBarra(lNbarrcodi);
                                BarraDTO dtoBarraGenerador = (new BarraAppServicio()).GetByIdBarra(gWbarrcodi);
                                sResultado = "Lo sentimos, debe registrar las Distancias Electricas de la barra g(w):" + dtoBarraGenerador.BarrNombre + " vs las dos barras de elemento l(m y n) [" + dtoBarra1.BarrNombre + "/" + dtoBarra2.BarrNombre + "] en 'Carga de Archivos'";
                                DeleteStDistelectricaGenele(strecacodi);
                                DeleteStElementoCompensado(strecacodi);
                                DeleteStFactorpago(strecacodi);
                                DeleteStPagoasignado(strecacodi);
                                return sResultado;
                            }
                            //=| (Zwm+Zwn)/2 |
                            double real = Convert.ToDouble(dtoZwm.Delbarrpu + dtoZwn.Delbarrpu) / 2;
                            double imaginario = Convert.ToDouble(dtoZwm.Delbarxpu + dtoZwn.Delbarxpu) / 2;
                            dDgl = Math.Sqrt(real * real + imaginario * imaginario);
                            if (dDgl == 0) dDgl = (double)0.000001;
                            //Almacenamos el DGL en la tabla ST_DISTELECTRICA_GENELE de cada central de generación, para usarlo en el paso 3
                            StDistelectricaGeneleDTO dtoDistanciaElectrica = new StDistelectricaGeneleDTO();
                            dtoDistanciaElectrica.Strecacodi = strecacodi;
                            dtoDistanciaElectrica.Stcntgcodi = dtoRespPago.Stcntgcodi; //codigo de la central
                            dtoDistanciaElectrica.Barrcodigw = gWbarrcodi;
                            dtoDistanciaElectrica.Stcompcodi = iStcompcodi; //Codigo del elemento
                            dtoDistanciaElectrica.Barrcodilm = lMbarrcodi;
                            dtoDistanciaElectrica.Barrcodiln = lNbarrcodi;
                            dtoDistanciaElectrica.Degeledistancia = Convert.ToDecimal(dDgl);
                            dtoDistanciaElectrica.Degeleusucreacion = suser;
                            dtoDistanciaElectrica.Degelefeccreacion = DateTime.Now;
                            SaveStDistelectricaGenele(dtoDistanciaElectrica);

                        }
                        //Buscamos a los generdores g que fueron identificados responsables de pagos para este periodo / recalculo
                        foreach (StRespagoDTO dtoRespPago in ListaResponsabilidadPagoTodos)
                        {
                            //Insertamos todos los factores de participación de pagos de cada central en cero el Facpagfggl
                            StFactorpagoDTO dtoFactorPago = new StFactorpagoDTO();
                            dtoFactorPago.Facpagreajuste = 0;  //Flag que indica que el registro aun no ha sido ajustado, solo quedaran en 1 a los que se les hara el ajuste
                            dtoFactorPago.Facpagfgglajuste = 0; //Valor por defecto del ajuste, por ahora cero

                            //insertar Factor de Perdida para central, elemento, revision,factor,fecha insercion, usuario insercion, flag
                            dtoFactorPago.Strecacodi = EntidadRecalculo.Strecacodi;
                            dtoFactorPago.Stcntgcodi = dtoRespPago.Stcntgcodi;
                            dtoFactorPago.Stcompcodi = iStcompcodi;
                            dtoFactorPago.Facpagfggl = 0;

                            dtoFactorPago.Facpagusucreacion = suser;
                            dtoFactorPago.Facpagfeccreacion = DateTime.Now;
                            //Insertamos El Factor de Participación del Generador y Elemento (iStcompcodi)
                            SaveStFactorpago(dtoFactorPago);
                        }

                        //Buscamos a los generdores g que fueron identificados responsables de pagos y que tienen Precencia en Compensación Mensual:
                        List<StRespagoDTO> ListaResponsabilidadPagoCentralGeneracion = GetByCriteriaStRespagosElemento(strecacodi, iStcompcodi);
                        //Calculamos el denominador (∑_(p=1)^ngen(Eg/Dgl) ) para aplicarlo en el paso siguiente
                        foreach (StRespagoDTO dtoRespPago in ListaResponsabilidadPagoCentralGeneracion)
                        {
                            List<StDistelectricaGeneleDTO> ListaDistanciaGenele = GetByCriteriaStDistelectricaGeneles(strecacodi, iStcompcodi);
                            StEnergiaDTO dtoEnergiaGenerada2 = new StEnergiaDTO();
                            dtoEnergiaGenerada2 = GetByCentralCodiStEnergia(strecacodi, dtoRespPago.Stcntgcodi);
                            if (dtoEnergiaGenerada2 == null)
                            {
                                sResultado = "En Energia Netas no esta grabado la central: " + dtoRespPago.Equinomb; //"-1";
                                return sResultado;
                            }
                            //Total de Sumatoria de Energias
                            foreach (var item in ListaDistanciaGenele)
                            {
                                if (dtoRespPago.Stcntgcodi == item.Stcntgcodi)
                                {
                                    decimal dEgDgl = dtoEnergiaGenerada2.Stenrgrgia / item.Degeledistancia;
                                    dEgDglTotal += dEgDgl; //Denominador de la formula (∑_(p=1)^ngen(Eg/Dgl) )
                                    break;
                                }
                            }
                            //dEgDglTotal Denominador de la formula (∑_(p=1)^ngen(Eg/Dgl) )
                        }

                        decimal dTotalFGgl = 0; //Almacena el total de la suma de todos los dFGgl
                        //Calculo de los factores de participación de pagos de cada central
                        foreach (StRespagoDTO dtoRespPago in ListaResponsabilidadPagoCentralGeneracion)
                        {
                            //buscamos energias netas de cada generador g responsable de pago
                            StEnergiaDTO dtoEnergiaGenerada = new StEnergiaDTO();
                            // Funcion trae La Central con EnergiaNeta  de Tabla Energia Neta
                            dtoEnergiaGenerada = GetByCentralCodiStEnergia(strecacodi, dtoRespPago.Stcntgcodi);
                            if (dtoEnergiaGenerada != null)
                            {
                                decimal dFGgl = 0; //Energia Neta de un Generador y Elemento
                                StDistelectricaGeneleDTO EntidadDistanciaGenele = GetByIdCriteriaStDistGene(dtoRespPago.Stcntgcodi, iStcompcodi);
                                //Calcula para FGgl para cada central en un elemento
                                if (EntidadDistanciaGenele != null)
                                {
                                    //Energia Neta de un Generador y Elemento
                                    dFGgl = (dtoEnergiaGenerada.Stenrgrgia / EntidadDistanciaGenele.Degeledistancia) / dEgDglTotal;
                                    //Actualizamos el Factor de Perdida para centra
                                    StFactorpagoDTO dtoFactorPago = GetByIdStFactorpagoFK(EntidadRecalculo.Strecacodi, dtoRespPago.Stcntgcodi, iStcompcodi);
                                    //Si FGgl<1%, entonces FGgl=0 y se recalculan los demas FGgl para que sumen 100%
                                    if (dFGgl < dPorcentajeMinimo)
                                    {
                                        //Descartando FGgl menores al 1%
                                        dtoFactorPago.Facpagreajuste = 0; //Flag se vuelve cero, indica que FGgl es menor a 1%
                                        dtoFactorPago.Facpagfgglajuste = 0; //Para este generador elemento, factores de participación de pagos = 0 

                                    }
                                    else
                                    {
                                        //Se le aplicara reajuste
                                        dtoFactorPago.Facpagreajuste = 1;
                                        dTotalFGgl += dFGgl; //Acumulando el total de todos los dFGgl

                                    }
                                    dtoFactorPago.Facpagfggl = decimal.Round(dFGgl, 12);
                                    UpdateStFactorpago(dtoFactorPago);
                                }
                            }
                        }

                        //Se requiere recalcular los factores de Pago para la nueva destribuión porcentual
                        //Traemos la lista de todos los factores mayores a 1% (Facpagreajuste = 1)
                        List<StFactorpagoDTO> ListaStFactorPago = GetByCriteriaStFactorpagos(strecacodi, iStcompcodi);
                        foreach (StFactorpagoDTO item in ListaStFactorPago)
                        {
                            item.Facpagfgglajuste = item.Facpagfggl / dTotalFGgl;
                            UpdateStFactorpago(item);

                        }
                        #endregion

                        StFactorDTO EntidadFactoresActualizacion = GetByCriteriaStFactorsSistema(dtoElemento.Sistrncodi);
                        if (EntidadFactoresActualizacion != null)
                        {
                            StElementoCompensadoDTO dtoElementoCompensado = new StElementoCompensadoDTO();
                            dtoElementoCompensado.Strecacodi = EntidadRecalculo.Strecacodi;
                            dtoElementoCompensado.Stfactcodi = EntidadFactoresActualizacion.Stfactcodi;
                            dtoElementoCompensado.Stcompcodi = iStcompcodi;
                            dtoElementoCompensado.Elecmpusucreacion = suser;
                            dtoElementoCompensado.Elecmpfeccreacion = DateTime.Now;
                            //De la tabla de factores, traemos el registro donde esta el Sistema del Elemento
                            //Aplicamos la formula: Ml’ = Ml x Fs
                            dtoElementoCompensado.Elecmpmonto = dtoElemento.Stcompimpcompensacion * EntidadFactoresActualizacion.Stfacttor;
                            // Se almacena la lista en la tabla ST_ELEMENTO_COMPENSADO
                            SaveStElementoCompensado(dtoElementoCompensado);

                            decimal dCMGgl = 0; //Almacena el Pago asignado CMGgl
                            decimal dCMGglP = 0; //Almacena el Pago asignado CMGgl Prima
                            decimal dPPgl = 0; //compensación mensual promedio año tarifario anterior del generador g al elemento l 
                            decimal CMGglPTotal = 0; //Almacena la suma de todos los dCMGglP
                            decimal dValorAjusteF = 0; //valor de ajuste F
                            decimal dCMGglFinal = 0; //pago final para cada elemento l por cada generador g identificado como responsable de pago


                            //Se identifica si el generador g es responsable de pago en el elemento l  (0/1)
                            //Traemos la lista de Centrales de Generación que estan relacionados con el Codigo de Elemento (en la presente version) y cuyo valor es 1 - uno
                            if (ListaResponsabilidadPagoCentralGeneracion.Count > 0)
                            {
                                #region PASO 3: Entre todos los generadores g que fueron identificados responsables de pagos:
                                //Calcular Pago asignado CMGgl para todos los generadores g
                                //En los arreglos registraremos las relaciones con respecto al generador
                                int[] ArreglodG = new int[1000]; //Lista de generadores
                                decimal[] ArreglodCMGgl = new decimal[1000];
                                decimal[] ArreglodCMGglP = new decimal[1000];
                                int iPosCMGglP = 0;
                                CMGglPTotal = 0;
                                //Traemos la lista de todos los factores mayores a 1%
                                ListaStFactorPago = ListStFactorpagos(strecacodi, iStcompcodi);
                                foreach (StFactorpagoDTO dtoFacpag in ListaStFactorPago)
                                {
                                    //Calculamos: CMGgl= Ml’ * FGgl 
                                    dCMGgl = dtoElementoCompensado.Elecmpmonto * dtoFacpag.Facpagfgglajuste;

                                    //Calcular Pago asignado filtrado  CMGgl’
                                    StCompmensualeleDTO EntidadCompMensuEle = GetByIdCriteriaStCompMensualEle(strecacodi, iStcompcodi, dtoFacpag.Stcntgcodi);
                                    if (EntidadCompMensuEle != null)
                                    {
                                        dPPgl = EntidadCompMensuEle.Cmpmelvalor;
                                        //CMGgl’= CMGgl x (1-R) + PPgl x R
                                        dCMGglP = dCMGgl * (1 - EntidadRecalculo.Strecafacajuste) + (dPPgl * EntidadRecalculo.Strecafacajuste);
                                        ArreglodG[iPosCMGglP] = dtoFacpag.Stcntgcodi;
                                        ArreglodCMGgl[iPosCMGglP] = dCMGgl;
                                        ArreglodCMGglP[iPosCMGglP] = dCMGglP;
                                        CMGglPTotal += dCMGglP; //∑_(i=1)^g (CMGgl') 
                                        iPosCMGglP++;
                                    }
                                    else
                                    {
                                        sResultado = "Lo sentimos, debe registrar su Compensacion Mensual del elemento:" + dtoElemento.Stcompcodelemento + " en 'Carga de Archivos'";
                                        DeleteStDistelectricaGenele(strecacodi);
                                        DeleteStElementoCompensado(strecacodi);
                                        DeleteStFactorpago(strecacodi);
                                        DeleteStPagoasignado(strecacodi);
                                        return sResultado;
                                    }
                                }
                                #endregion

                                #region PASO 4 - Ajustar todos los CMGgl’ para que al elemento l se le pague Ml’
                                //Obtener un valor de ajuste f
                                dValorAjusteF = dtoElementoCompensado.Elecmpmonto / CMGglPTotal;
                                #endregion

                                #region PASO 5 - pago final para cada elemento l por cada generador g identificado como responsable de pago
                                for (int i = 0; i < iPosCMGglP; i++)
                                {
                                    //Almacenamos en la Tabla de Pagos asignados
                                    StPagoasignadoDTO dtoPagAsig = new StPagoasignadoDTO();
                                    dtoPagAsig.Strecacodi = EntidadRecalculo.Strecacodi;
                                    dtoPagAsig.Stcntgcodi = ArreglodG[i];
                                    dtoPagAsig.Stcompcodi = iStcompcodi;
                                    dtoPagAsig.Pagasgcmggl = ArreglodCMGgl[i];
                                    dtoPagAsig.Pagasgcmgglp = ArreglodCMGglP[i];
                                    dCMGglFinal = ArreglodCMGglP[i] * dValorAjusteF;
                                    dtoPagAsig.Pagasgcmgglfinal = dCMGglFinal;
                                    dtoPagAsig.Pagasgusucreacion = suser;
                                    dtoPagAsig.Pagasgfeccreacion = DateTime.Now;
                                    SaveStPagoasignado(dtoPagAsig);
                                }
                                #endregion
                            }
                            else
                            {
                                sResultado = "Lo sentimos, debe registrar sus Responsabilidades de Pago del elemento:" + dtoElemento.Stcompcodelemento + " en 'Carga de Archivos'";
                                DeleteStDistelectricaGenele(strecacodi);
                                DeleteStElementoCompensado(strecacodi);
                                DeleteStFactorpago(strecacodi);
                                DeleteStPagoasignado(strecacodi);
                                return sResultado;
                            }
                        }
                        else
                        {
                            sResultado = "Lo sentimos, debe registrar su Factor de Actualizacion del elemento:" + dtoElemento.Stcompcodelemento + " en 'Carga de Archivos'";
                            DeleteStDistelectricaGenele(strecacodi);
                            DeleteStElementoCompensado(strecacodi);
                            DeleteStFactorpago(strecacodi);
                            DeleteStPagoasignado(strecacodi);
                            return sResultado;
                        }
                    }
                }
                else
                {
                    sResultado = "Lo sentimos, No hay Elementos Registrados";
                    return sResultado;
                }
                #endregion
            }
            catch (Exception e)
            {
                sResultado = e.StackTrace;
                sResultado = e.Message; //"-1";
            }
            return sResultado;
        }

        /// <summary>
        /// Almacena un archivo en excel en un data set
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <param name="Hoja"></param>
        public DataSet GeneraDataset(string RutaArchivo, int hoja)
        {
            return UtilSistemasTransmision.GeneraDataset(RutaArchivo, hoja);
        }

        /// <summary> 
        /// Permite generar el archivo de exportacion de la tabla ST_COMPMENSUAL
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaStCompmensual">Lista de registros de StCompmensualDTO</param>
        /// <param name="ListaCompensacion">Lista de registros de StCompensacionDTO</param>
        /// <returns></returns>
        public string GenerarFormatoStCompmensual(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StCompmensualDTO> ListaStCompmensual = this.GetByCriteriaStCompmensuals(strecacodi);
            List<StCompensacionDTO> ListaCompensacion = this.GetByCriteriaStCompensacions(strecacodi);
            if (formato == 1)
            {
                fileName = "ReporteCompensaciónMensual.xlsx";
                ExcelDocument.GenerarFormatoStCompmensual(pathFile + fileName, EntidadRecalculo, ListaStCompmensual, ListaCompensacion);
            }

            return fileName;
        }

        /// <summary> 
        /// Permite generar el archivo de exportacion de la tabla ST_RESPAGO
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaStRespago">Lista de registros de StRespagoDTO</param>
        /// <param name="ListaCompensacion">Lista de registros de StCompensacionDTO</param>
        /// <returns></returns>
        public string GenerarFormatoStRespago(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StRespagoDTO> ListaStRespago = this.GetByCriteriaStRespagos(strecacodi);
            List<StCompensacionDTO> ListaCompensacion = this.GetByCriteriaStCompensacions(strecacodi);
            if (formato == 1)
            {
                fileName = "ReporteCentralesResponsabilidad.xlsx";
                ExcelDocument.GenerarFormatoStRespago(pathFile + fileName, EntidadRecalculo, ListaStRespago, ListaCompensacion);
            }

            return fileName;
        }

        //*******************************************************************************************
        /// <summary> 
        /// CU06 Reportes 301 – GWh/OHMIOS Mensuales de Generadores Relevantes 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaReporteGeneradores">Lista de registros de StCentralgenDTO</param>
        /// <returns></returns>
        public string GenerarFormatoReporte301(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StCentralgenDTO> ListaReporteGeneradores = this.GetByCriteriaStCentralgensReporte(strecacodi);

            if (formato == 1)
            {
                fileName = "ReporteGWh-OHMIOSMensualesDeGeneradoresRelevantes.xlsx";
                ExcelDocument.GenerarFormatoReporte301(pathFile + fileName, EntidadRecalculo, ListaReporteGeneradores);
            }

            return fileName;
        }

        /// <summary> 
        /// CU07 Reportes 302 – Cálculo del Factor de Participación Mensual o Anual 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaReporteFactorParticipacion">Lista de registros de StFactorpagoDTO</param>
        /// <returns></returns>
        public string GenerarFormatoReporte302(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StFactorpagoDTO> ListaReporteFactorParticipacion = this.GetByCriteriaStFactorpagosReporteFactorParticipacion(strecacodi);

            if (formato == 1)
            {
                fileName = "ReporteCalculoFactorParticipacionMensualOAnual.xlsx";
                ExcelDocument.GenerarFormatoReporte302(pathFile + fileName, EntidadRecalculo, ListaReporteFactorParticipacion);
            }

            return fileName;
        }

        /// <summary> 
        /// CU08 Reportes 303 – Compensación Mensual
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaReporteCompensacionMensual">Lista de registros de StFactorpagoDTO</param>
        /// <returns></returns>
        public string GenerarFormatoReporte303(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StFactorpagoDTO> ListaReporteCompensacionMensual = this.GetByCriteriaStFactorpagosReporte(strecacodi);

            if (formato == 1)
            {
                fileName = "ReporteCompensacionMensual.xlsx";
                ExcelDocument.GenerarFormatoReporte303(pathFile + fileName, EntidadRecalculo, ListaReporteCompensacionMensual);
            }

            return fileName;
        }

        /// <summary> 
        /// CU09 lo seReportes Distancias Eléctricas 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaReporteDistElectrGenele">Lista de registros de StDistelectricaGeneleDTO</param>
        /// <returns></returns>
        public string GenerarFormatoReporteDistElec(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StDistelectricaGeneleDTO> ListaReporteDistElectrGenele = this.GetByCriteriaStDistelectricaGenelesReporte(strecacodi);

            if (formato == 1)
            {
                fileName = "ReporteDistanciasElectricas.xlsx";
                ExcelDocument.GenerarFormatoReporteDistElec(pathFile + fileName, EntidadRecalculo, ListaReporteDistElectrGenele);
            }

            return fileName;
        }

        /// <summary> 
        /// CU10 Reportes Factor de Participación
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaFactorPago">Lista de registros de StFactorpagoDTO</param>
        /// <returns></returns>
        public string GenerarFormatoFactorParticipacion(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StFactorpagoDTO> ListaFactorPago = this.GetByCriteriaStFactorpagosInicialReporte(strecacodi);
            if (formato == 1)
            {
                fileName = "ReporteFactorParticipacion.xlsx";
                ExcelDocument.GenerarFormatoFactorParticipacion(pathFile + fileName, EntidadRecalculo, ListaFactorPago);
            }

            return fileName;
        }

        /// <summary> 
        /// CU11 Reportes Factor de Participación Recalculado 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaFactorPago">Lista de registros de StFactorpagoDTO</param>
        /// <returns></returns>
        public string GenerarFormatoFactorParticipacionRecalculado(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StFactorpagoDTO> ListaFactorPago = this.GetByCriteriaStFactorpagosReporte(strecacodi);

            if (formato == 1)
            {
                fileName = "ReporteFactorParticipacion.xlsx";
                ExcelDocument.GenerarFormatoFactorParticipacionRecalculado(pathFile + fileName, EntidadRecalculo, ListaFactorPago);
            }

            return fileName;
        }

        /// <summary> 
        /// CU12 Reportes Compensación Mensual Asignada 
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaPagoAsignadoReporte">Lista de registros de StPagoasignadoDTO</param>
        /// <returns></returns>
        public string GenerarReporteCompensacionMensual(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StPagoasignadoDTO> ListaPagoAsignadoReporte = this.GetByCriteriaStPagoasignadoReporte(strecacodi);
            if (formato == 1)
            {
                fileName = "ReporteCompensacionMensual.xlsx";
                ExcelDocument.GenerarReporteCompensacionMensual(pathFile + fileName, EntidadRecalculo, ListaPagoAsignadoReporte);
            }

            return fileName;
        }

        /// <summary> 
        /// CU13 Reportes Compensación Mensual Filtrada CU13
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaPagoAsignadoReporteFiltrado">Lista de registros de StPagoasignadoDTO</param>
        /// <returns></returns>
        public string GenerarReporteCompensacionMensualFiltrada(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StPagoasignadoDTO> ListaPagoAsignadoReporteFiltrado = this.GetByCriteriaStPagoasignadoReporte(strecacodi);
            if (formato == 1)
            {
                fileName = "ReporteCompensacionMensualFiltrada.xlsx";
                ExcelDocument.GenerarReporteCompensacionMensualFiltrada(pathFile + fileName, EntidadRecalculo, ListaPagoAsignadoReporteFiltrado);
            }

            return fileName;
        }

        /// <summary> 
        /// CU14 Reportes Asignación de Responsabilidad de Pago de Sistemas Secundarios de Transmisión y Sistemas Complementarios de Transmisión por Parte de los Generadores por el Criterio de Uso
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="EntidadRecalculo">Entidad de StRecalculoDTO</param>
        /// <param name="ListaEmpresasGeneradores">Lista de registros de StPagoasignadoDTO</param>
        /// <param name="ListaEmpresasSistemas">Lista de registros de StPagoasignadoDTO</param>
        /// <returns></returns>
        public string GenerarReporteResponsabilidadPago(int strecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<StPagoasignadoDTO> ListaEmpresasGeneradores = this.ListStPagoasignadosEmpresaGeneradores(strecacodi);
            List<StPagoasignadoDTO> ListaEmpresasSistemas = this.ListStPagoasignadosEmpresaSistemas(strecacodi);
            if (formato == 1)
            {
                fileName = "ReporteResponsabilidadPago.xlsx";
                ExcelDocument.GenerarReporteResponsabilidadPago(pathFile + fileName, EntidadRecalculo, ListaEmpresasGeneradores, ListaEmpresasSistemas);
            }

            return fileName;
        }

        #endregion

    }
}
