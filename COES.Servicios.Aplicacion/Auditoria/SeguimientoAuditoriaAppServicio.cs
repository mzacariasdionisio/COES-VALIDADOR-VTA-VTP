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

namespace COES.Servicios.Aplicacion.Auditoria
{
    /// <summary>
    /// Clases con métodos del módulo Auditoria
    /// </summary>
    public class SeguimientoAuditoriaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SeguimientoAuditoriaAppServicio));

        public List<AudProcesoDTO> GetByProceoPorEstado { get; set; }

        #region Métodos Tabla AUD_ARCHIVO

        /// <summary>
        /// Inserta un registro de la tabla AUD_ARCHIVO
        /// </summary>
        public int SaveAudArchivo(AudArchivoDTO entity)
        {
            try
            {
                return FactorySic.GetAudArchivoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_ARCHIVO
        /// </summary>
        public void UpdateAudArchivo(AudArchivoDTO entity)
        {
            try
            {
                FactorySic.GetAudArchivoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_ARCHIVO
        /// </summary>
        public void DeleteAudArchivo(int archcodi)
        {
            try
            {
                FactorySic.GetAudArchivoRepository().Delete(archcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_ARCHIVO
        /// </summary>
        public AudArchivoDTO GetByIdAudArchivo(int archcodi)
        {
            try
            {
                return FactorySic.GetAudArchivoRepository().GetById(archcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_ARCHIVO
        /// </summary>
        public List<AudArchivoDTO> ListAudArchivos()
        {
            try
            {
                return FactorySic.GetAudArchivoRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudArchivo
        /// </summary>
        public List<AudArchivoDTO> GetByCriteriaAudArchivos()
        {
            try
            {
                return FactorySic.GetAudArchivoRepository().GetByCriteria();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla AUD_AUDITORIA

        /// <summary>
        /// Inserta un registro de la tabla AUD_AUDITORIA
        /// </summary>
        public int SaveAudAuditoria(AudAuditoriaDTO entity)
        {
            try
            {
                return FactorySic.GetAudAuditoriaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_AUDITORIA
        /// </summary>
        public void UpdateAudAuditoria(AudAuditoriaDTO entity)
        {
            try
            {
                FactorySic.GetAudAuditoriaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_AUDITORIA
        /// </summary>
        public void DeleteAudAuditoria(AudAuditoriaDTO auditoria)
        {
            try
            {
                FactorySic.GetAudAuditoriaRepository().Delete(auditoria);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_AUDITORIA
        /// </summary>
        public AudAuditoriaDTO GetByIdAudAuditoria(int audicodi)
        {
            try
            {
                return FactorySic.GetAudAuditoriaRepository().GetById(audicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_AUDITORIA
        /// </summary>
        public List<AudAuditoriaDTO> ListAudAuditorias()
        {
            try
            {
                return FactorySic.GetAudAuditoriaRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudAuditoria
        /// </summary>
        public List<AudAuditoriaDTO> GetByCriteriaAudAuditorias(AudAuditoriaDTO auditoria)
        {
            try
            {
                return FactorySic.GetAudAuditoriaRepository().GetByCriteria(auditoria);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite mostrar todos los años que se encuentran en la lista de Auditorias.
        /// </summary>
        /// <returns></returns>
        public List<string> MostrarAnios()
        {
            try
            {
                return FactorySic.GetAudAuditoriaRepository().MostrarAnios();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite mostrar todas las auditorías según los filtros seleccionados
        /// </summary>
        /// <param name="auditoria"></param>
        /// <returns></returns>
        public List<AudAuditoriaDTO> MostrarAuditoriasEjecutar(AudAuditoriaDTO auditoria)
        {
            try
            {
                return FactorySic.GetAudAuditoriaRepository().MostrarAuditoriasEjecutar(auditoria);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el nro de registros del resultado de la busqueda de AUD_AUDITORIA
        /// </summary>
        public int ObtenerNroRegistrosAudAuditorias(AudAuditoriaDTO auditoria)
        {
            try
            {
                return FactorySic.GetAudAuditoriaRepository().ObtenerNroRegistrosBusqueda(auditoria);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el nro de registros del resultado de la busqueda de AUD_AUDITORIA
        /// </summary>
        public List<AudAuditoriaDTO> VerResultadosAudAuditorias(AudAuditoriaDTO auditoria)
        {
            try
            {
                return FactorySic.GetAudAuditoriaRepository().VerResultados(auditoria);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el nro de registros del resultado de la busqueda de AUD_AUDITORIA
        /// </summary>
        public int ObtenerNroRegistrosResultadosAudAuditorias(int audicodi)
        {
            try
            {
                return FactorySic.GetAudAuditoriaRepository().ObtenerNroRegistrosBusquedaResultados(audicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Tabla AUD_AUDITORIAELEMENTO

        /// <summary>
        /// Inserta un registro de la tabla AUD_AUDITORIAPROCESO
        /// </summary>
        public void SaveAudAuditoriaproceso(AudAuditoriaprocesoDTO entity)
        {
            try
            {
                FactorySic.GetAudAuditoriaprocesoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_AUDITORIAPROCESO
        /// </summary>
        public void UpdateAudAuditoriaproceso(AudAuditoriaprocesoDTO entity)
        {
            try
            {
                FactorySic.GetAudAuditoriaprocesoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_AUDITORIAPROCESO
        /// </summary>
        public void DeleteAudAuditoriaproceso(AudAuditoriaprocesoDTO auditoriaproceso)
        {
            try
            {
                FactorySic.GetAudAuditoriaprocesoRepository().Delete(auditoriaproceso);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla AUD_AUDITORIAPROCESO por el codigo de auditoria
        /// </summary>
        public void DeleteAllAudAuditoriaproceso(AudAuditoriaprocesoDTO auditoriaproceso)
        {
            try
            {
                FactorySic.GetAudAuditoriaprocesoRepository().DeleteAllAudAuditoriaproceso(auditoriaproceso);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_AUDITORIAPROCESO
        /// </summary>
        public AudAuditoriaprocesoDTO GetByIdAudAuditoriaproceso(int audipcodi)
        {
            try
            {
                return FactorySic.GetAudAuditoriaprocesoRepository().GetById(audipcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_AUDITORIAPROCESO
        /// </summary>
        public AudAuditoriaprocesoDTO GetByAudppcodiAudAuditoriaproceso(int audicodi, int Audppcodi, int Proccodi)
        {
            try
            {
                return FactorySic.GetAudAuditoriaprocesoRepository().GetByAudppcodi(audicodi, Audppcodi, Proccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_AUDITORIAPROCESO
        /// </summary>
        public List<AudAuditoriaprocesoDTO> ListAudAuditoriaprocesos(int audpcodi)
        {
            try
            {
                return FactorySic.GetAudAuditoriaprocesoRepository().List(audpcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AUD_AUDITORIAPROCESO
        /// </summary>
        public List<AudAuditoriaprocesoDTO> GetByCriteriaAudAuditoriaprocesos(int audicodi)
        {
            try
            {
                return FactorySic.GetAudAuditoriaprocesoRepository().GetByCriteria(audicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AUD_AUDITORIAPROCESO
        /// </summary>
        public List<AudAuditoriaprocesoDTO> GetByAuditoriaElementoPorTipo(int audicodi, int tabcdcoditipoelemento)
        {
            try
            {
                return FactorySic.GetAudAuditoriaprocesoRepository().GetByAuditoriaElementoPorTipo(audicodi, tabcdcoditipoelemento);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla AUD_AUDITORIAPLANIFICADA

        /// <summary>
        /// Inserta un registro de la tabla AUD_AUDITORIAPLANIFICADA
        /// </summary>
        public int SaveAudAuditoriaplanificada(AudAuditoriaplanificadaDTO entity)
        {
            try
            {
                return FactorySic.GetAudAuditoriaplanificadaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_AUDITORIAPLANIFICADA
        /// </summary>
        public void UpdateAudAuditoriaplanificada(AudAuditoriaplanificadaDTO entity)
        {
            try
            {
                FactorySic.GetAudAuditoriaplanificadaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_AUDITORIAPLANIFICADA
        /// </summary>
        public void DeleteAudAuditoriaplanificada(AudAuditoriaplanificadaDTO auditoriaplanificada)
        {
            try
            {
                FactorySic.GetAudAuditoriaplanificadaRepository().Delete(auditoriaplanificada);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla AUD_AUDITORIAPLANIFICADA por plan auditoria
        /// </summary>
        public void DeleteAllAudAuditoriaplanificada(AudAuditoriaplanificadaDTO auditoriaPlanificada)
        {
            try
            {
                FactorySic.GetAudAuditoriaplanificadaRepository().DeleteByAudPlan(auditoriaPlanificada);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_AUDITORIAPLANIFICADA
        /// </summary>
        public AudAuditoriaplanificadaDTO GetByIdAudAuditoriaplanificada(int audpcodi)
        {
            try
            {
                return FactorySic.GetAudAuditoriaplanificadaRepository().GetById(audpcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_AUDITORIAPLANIFICADA
        /// </summary>
        public List<AudAuditoriaplanificadaDTO> ListAudAuditoriaplanificadas()
        {
            try
            {
                return FactorySic.GetAudAuditoriaplanificadaRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudAuditoriaplanificada
        /// </summary>
        public List<AudAuditoriaplanificadaDTO> GetByCriteriaAudAuditoriaplanificadas(int plancodi, string audphistorico)
        {
            try
            {
                return FactorySic.GetAudAuditoriaplanificadaRepository().GetByCriteria(plancodi, audphistorico);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar las validaciones de la auditoria planificada seleccionado
        /// </summary>
        public string GetByAudPlanificadaValidacion(int audpcodi)
        {
            try
            {
                return FactorySic.GetAudAuditoriaplanificadaRepository().GetByAudPlanificadaValidacion(audpcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Tabla AUD_AUDPLANIFICADA_PROCESO

        /// <summary>
        /// Inserta un registro de la tabla AUD_AUDPLANIFICADA_PROCESO
        /// </summary>
        public void SaveAudAudplanificadaProceso(AudAudplanificadaprocesoDTO entity)
        {
            try
            {
                FactorySic.GetAudAudplanificadaProcesoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_AUDPLANIFICADA_PROCESO
        /// </summary>
        public void UpdateAudAudplanificadaProceso(AudAudplanificadaprocesoDTO entity)
        {
            try
            {
                FactorySic.GetAudAudplanificadaProcesoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_AUDPLANIFICADA_PROCESO
        /// </summary>
        public void DeleteAudAudplanificadaProceso(AudAudplanificadaprocesoDTO audPlanificadaProceso)
        {
            try
            {
                FactorySic.GetAudAudplanificadaProcesoRepository().Delete(audPlanificadaProceso);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla AUD_AUDPLANIFICADA_PROCESO por el codigo de auditoria planificada
        /// </summary>
        public void DeleteAllAudplanificadaProceso(AudAudplanificadaprocesoDTO audPlanificadaProceso)
        {
            try
            {
                FactorySic.GetAudAudplanificadaProcesoRepository().DeleteByAudPlanificada(audPlanificadaProceso);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_AUDPLANIFICADA_PROCESO
        /// </summary>
        public AudAudplanificadaprocesoDTO GetByIdAudAudplanificadaProceso(int audpcodi, int proccodi)
        {
            try
            {
                return FactorySic.GetAudAudplanificadaProcesoRepository().GetById(audpcodi, proccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_AUDPLANIFICADA_PROCESO
        /// </summary>
        public List<AudAudplanificadaprocesoDTO> ListAudAudplanificadaProcesos(int audpcodi)
        {
            try
            {
                return FactorySic.GetAudAudplanificadaProcesoRepository().List(audpcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudAudplanificadaProceso
        /// </summary>
        public List<AudAudplanificadaprocesoDTO> GetByCriteriaAudAudplanificadaProcesos(int audpcodi, string areacodi)
        {
            try
            {
                return FactorySic.GetAudAudplanificadaProcesoRepository().GetByCriteria(audpcodi, areacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla AUD_ELEMENTO

        /// <summary>
        /// Inserta un registro de la tabla AUD_ELEMENTO
        /// </summary>
        public void SaveAudElemento(AudElementoDTO entity)
        {
            try
            {
                FactorySic.GetAudElementoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_ELEMENTO
        /// </summary>
        public void UpdateAudElemento(AudElementoDTO entity)
        {
            try
            {
                FactorySic.GetAudElementoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_ELEMENTO
        /// </summary>
        public void DeleteAudElemento(AudElementoDTO elemento)
        {
            try
            {
                FactorySic.GetAudElementoRepository().Delete(elemento);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_ELEMENTO
        /// </summary>
        public AudElementoDTO GetByIdAudElemento(int elemcodi)
        {
            try
            {
                return FactorySic.GetAudElementoRepository().GetById(elemcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_ELEMENTO
        /// </summary>
        public List<AudElementoDTO> ListAudElementos()
        {
            try
            {
                return FactorySic.GetAudElementoRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudElemento
        /// </summary>
        public List<AudElementoDTO> GetByCriteriaAudElementos(AudElementoDTO audElementoDTO)
        {
            try
            {
                return FactorySic.GetAudElementoRepository().GetByCriteria(audElementoDTO);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
         /// <summary>
         /// Permite mostrar los datos de los elementos que estén relacionado con áreas
         /// </summary>
         /// <returns></returns>
        public List<SiAreaDTO> GetByAreaElemento()
        {
            try
            {
                return FactorySic.GetAudElementoRepository().GetByAreaElemento();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite mostrar todos los procesos que tengan relación con algún elemento activo
        /// </summary>
        /// <returns></returns>
        public List<AudProcesoDTO> GetByProcesoElemento()
        {
            try
            {
                return FactorySic.GetAudElementoRepository().GetByProcesoElemento();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite mostrar a los procesos según el elemento seleccionado
        /// </summary>
        /// <returns></returns>
        public List<AudElementoDTO> GetByProcesoPorElemento(AudElementoDTO audElementoDTO)
        {
            try
            {
                return FactorySic.GetAudElementoRepository().GetByProcesoPorElemento(audElementoDTO);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite mostrar a los elementos según el proceso seleccionado
        /// </summary>
        /// <returns></returns>
        public List<AudElementoDTO> GetByElementosPorProceso(int proccodi)
        {
            try
            {
                return FactorySic.GetAudElementoRepository().GetByElementosPorProceso(proccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite mostrar a los elementos según el auditoria planificada proceso seleccionado
        /// </summary>
        /// <returns></returns>
        public List<AudElementoDTO> GetByElementosPorProcesoAP(int plancodi, string procesos)
        {
            try
            {
                return FactorySic.GetAudElementoRepository().GetByElementosPorProcesoAP(plancodi, procesos);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite mostrar a los elementos según el tipo elemento seleccionado
        /// </summary>
        /// <returns></returns>
        public List<AudElementoDTO> GetByElementosPorTipo(int tipoelemento)
        {
            try
            {
                return FactorySic.GetAudElementoRepository().GetByElementosPorTipo(tipoelemento);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite mostrar a los elementos según los procesos seleccionados y el tipo elemento seleccionado
        /// </summary>
        /// <returns></returns>
        public List<AudElementoDTO> GetByElementosPorProcesoTipo(string procesos, int tipoelemento)
        {
            try
            {
                return FactorySic.GetAudElementoRepository().GetByElementosPorProcesoTipo(procesos, tipoelemento);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtiene el nro de registros del resultado de la busqueda de AudElemento
        /// </summary>
        public int ObtenerNroRegistrosAudElementos(AudElementoDTO audElementoDTO)
        {
            try
            {
                return FactorySic.GetAudElementoRepository().ObtenerNroRegistrosBusqueda(audElementoDTO);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar las validaciones del elemento seleccionado
        /// </summary>
        public string GetByElementoValidacion(int elemcodi)
        {
            try
            {
                return FactorySic.GetAudElementoRepository().GetByElementoValidacion(elemcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Tabla AUD_NOTIFICACION

        /// <summary>
        /// Inserta un registro de la tabla AUD_NOTIFICACION
        /// </summary>
        public int  SaveAudNotificacion(AudNotificacionDTO entity)
        {
            try
            {
                return FactorySic.GetAudNotificacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_NOTIFICACION
        /// </summary>
        public void UpdateAudNotificacion(AudNotificacionDTO entity)
        {
            try
            {
                FactorySic.GetAudNotificacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_NOTIFICACION
        /// </summary>
        public void DeleteAudNotificacion(int noticodi)
        {
            try
            {
                FactorySic.GetAudNotificacionRepository().Delete(noticodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_NOTIFICACION
        /// </summary>
        public AudNotificacionDTO GetByIdAudNotificacion(int noticodi)
        {
            try
            {
                return FactorySic.GetAudNotificacionRepository().GetById(noticodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_NOTIFICACION
        /// </summary>
        public List<AudNotificacionDTO> ListAudNotificacions()
        {
            try
            {
                return FactorySic.GetAudNotificacionRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudNotificacion
        /// </summary>
        public List<AudNotificacionDTO> GetByCriteriaAudNotificacions()
        {
            try
            {
                return FactorySic.GetAudNotificacionRepository().GetByCriteria();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla AUD_NOTIFICACIONDEST

        /// <summary>
        /// Inserta un registro de la tabla AUD_NOTIFICACIONDEST
        /// </summary>
        public void SaveAudNotificaciondest(AudNotificaciondestDTO entity)
        {
            try
            {
                FactorySic.GetAudNotificaciondestRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_NOTIFICACIONDEST
        /// </summary>
        public void UpdateAudNotificaciondest(AudNotificaciondestDTO entity)
        {
            try
            {
                FactorySic.GetAudNotificaciondestRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_NOTIFICACIONDEST
        /// </summary>
        public void DeleteAudNotificaciondest(int notdcodi)
        {
            try
            {
                FactorySic.GetAudNotificaciondestRepository().Delete(notdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_NOTIFICACIONDEST
        /// </summary>
        public AudNotificaciondestDTO GetByIdAudNotificaciondest(int notdcodi)
        {
            try
            {
                return FactorySic.GetAudNotificaciondestRepository().GetById(notdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_NOTIFICACIONDEST
        /// </summary>
        public List<AudNotificaciondestDTO> ListAudNotificaciondests()
        {
            try
            {
                return FactorySic.GetAudNotificaciondestRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudNotificaciondest
        /// </summary>
        public List<AudNotificaciondestDTO> GetByCriteriaAudNotificaciondests()
        {
            try
            {
                return FactorySic.GetAudNotificaciondestRepository().GetByCriteria();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla AUD_PLANAUDITORIA

        /// <summary>
        /// Inserta un registro de la tabla AUD_PLANAUDITORIA
        /// </summary>
        public int SaveAudPlanauditoria(AudPlanauditoriaDTO entity)
        {
            try
            {
                return FactorySic.GetAudPlanauditoriaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_PLANAUDITORIA
        /// </summary>
        public void UpdateAudPlanauditoria(AudPlanauditoriaDTO entity)
        {
            try
            {
                FactorySic.GetAudPlanauditoriaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_PLANAUDITORIA
        /// </summary>
        public void DeleteAudPlanauditoria(AudPlanauditoriaDTO planAuditoria)
        {
            try
            {
                FactorySic.GetAudPlanauditoriaRepository().Delete(planAuditoria);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_PLANAUDITORIA
        /// </summary>
        public AudPlanauditoriaDTO GetByIdAudPlanauditoria(int plancodi)
        {
            try
            {
                return FactorySic.GetAudPlanauditoriaRepository().GetById(plancodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_PLANAUDITORIA
        /// </summary>
        public List<AudPlanauditoriaDTO> ListAudPlanauditorias()
        {
            try
            {
                return FactorySic.GetAudPlanauditoriaRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudPlanauditoria
        /// </summary>
        public List<AudPlanauditoriaDTO> GetByCriteriaAudPlanauditorias(AudPlanauditoriaDTO planAuditoria)
        {
            try
            {
                return FactorySic.GetAudPlanauditoriaRepository().GetByCriteria(planAuditoria);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el nro de registros del resultado de la busqueda de AudPlanauditoria
        /// </summary>
        public int ObtenerNroRegistrosAudPlanauditorias(AudPlanauditoriaDTO planAuditoria)
        {
            try
            {
                return FactorySic.GetAudPlanauditoriaRepository().ObtenerNroRegistrosBusqueda(planAuditoria);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los plananovigencia existentes de la tabla AudPlanauditoria
        /// </summary>
        public List<int> ListAnioVigenciaAudPlanauditorias() {
            List<AudPlanauditoriaDTO> listadoPlanauditoria = ListAudPlanauditorias();
            List<int> listadoAnioVigencia = listadoPlanauditoria.Select(p => Convert.ToInt32(p.Plananovigencia)).Distinct().OrderBy(p => p).ToList();

            int ultimoAnioVigencia = DateTime.Now.Year;
            if (listadoAnioVigencia.Count > 0) {
                ultimoAnioVigencia = listadoAnioVigencia.Max();
            }
            
            listadoAnioVigencia.Insert(listadoAnioVigencia.Count, ultimoAnioVigencia + 1);

            return listadoAnioVigencia;
        }

        /// <summary>
        /// Permite realizar las validaciones del plan seleccionado
        /// </summary>
        public string GetByPlanValidacion(int plancodi)
        {
            try
            {
                return FactorySic.GetAudPlanauditoriaRepository().GetByPlanValidacion(plancodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Tabla AUD_PROCESO

        /// <summary>
        /// Inserta un registro de la tabla AUD_PROCESO
        /// </summary>
        public void SaveAudProceso(AudProcesoDTO entity)
        {
            try
            {
                FactorySic.GetAudProcesoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_PROCESO
        /// </summary>
        public void UpdateAudProceso(AudProcesoDTO entity)
        {
            try
            {
                FactorySic.GetAudProcesoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_PROCESO
        /// </summary>
        public void DeleteAudProceso(AudProcesoDTO proceso)
        {
            try
            {
                FactorySic.GetAudProcesoRepository().Delete(proceso);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_PROCESO
        /// </summary>
        public AudProcesoDTO GetByIdAudProceso(int proccodi)
        {
            try
            {
                return FactorySic.GetAudProcesoRepository().GetById(proccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_PROCESO
        /// </summary>
        public AudProcesoDTO GetByCodigoAudProceso(string proccodigo)
        {
            try
            {
                return FactorySic.GetAudProcesoRepository().GetByCodigo(proccodigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_PROCESO
        /// </summary>
        public List<AudProcesoDTO> ListAudProcesos()
        {
            try
            {
                return FactorySic.GetAudProcesoRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_PROCESO
        /// </summary>
        public List<AudProcesoDTO> ListProcesoSuperiorAudProcesos(int proccodi, int areacodi)
        {
            try
            {
                return FactorySic.GetAudProcesoRepository().ListProcesoSuperior(proccodi, areacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudProceso
        /// </summary>
        public List<AudProcesoDTO> GetByCriteriaAudProcesos(AudProcesoDTO proceso)
        {
            try
            {
                return FactorySic.GetAudProcesoRepository().GetByCriteria(proceso);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el nro de registros del resultado de la busqueda de AudProceso
        /// </summary>
        public int ObtenerNroRegistrosAudProcesos(AudProcesoDTO proceso)
        {
            try
            {
                return FactorySic.GetAudProcesoRepository().ObtenerNroRegistrosBusqueda(proceso);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite mostrar los procesos según el tipo de estado
        /// </summary>
        public List<AudProcesoDTO> GetByProcesoPorEstado(string estado)
        {
            try
            {
                return FactorySic.GetAudProcesoRepository().GetByProcesoPorEstado(estado);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite mostrar los procesos según el area seleccionada
        /// </summary>
        public List<AudProcesoDTO> GetByProcesoPorArea(string areacodi)
        {
            try
            {
                return FactorySic.GetAudProcesoRepository().GetByProcesoPorArea(areacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar las validaciones del proceso seleccionado
        /// </summary>
        public string GetByProcesoValidacion(int proccodi, string procedescripcion = "")
        {
            try
            {
                return FactorySic.GetAudProcesoRepository().GetByProcesoValidacion(proccodi, procedescripcion);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        

        #endregion

        #region Métodos Tabla AUD_PROGAUDI_ELEMENTO

        /// <summary>
        /// Inserta un registro de la tabla AUD_PROGAUDI_ELEMENTO
        /// </summary>
        public void SaveAudProgaudiElemento(AudProgaudiElementoDTO entity)
        {
            try
            {
                FactorySic.GetAudProgaudiElementoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_PROGAUDI_ELEMENTO
        /// </summary>
        public void UpdateAudProgaudiElemento(AudProgaudiElementoDTO entity)
        {
            try
            {
                FactorySic.GetAudProgaudiElementoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_PROGAUDI_ELEMENTO
        /// </summary>
        public void DeleteAudProgaudiElemento(AudProgaudiElementoDTO entity)
        {
            try
            {
                FactorySic.GetAudProgaudiElementoRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_PROGAUDI_ELEMENTO
        /// </summary>
        public AudProgaudiElementoDTO GetByIdAudProgaudiElemento(int progaecodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiElementoRepository().GetById(progaecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_PROGAUDI_ELEMENTO
        /// </summary>
        public AudProgaudiElementoDTO GetByElemcodiAudProgaudiElemento(int progacodi, int elemcodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiElementoRepository().GetByElemcodi(progacodi, elemcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_PROGAUDI_ELEMENTO
        /// </summary>
        public List<AudProgaudiElementoDTO> ListAudProgaudiElementos(int audicodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiElementoRepository().List(audicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudProgaudiElemento
        /// </summary>
        public List<AudProgaudiElementoDTO> GetByCriteriaAudProgaudiElementos(int progacodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiElementoRepository().GetByCriteria(progacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite realizar búsquedas en la tabla AudProgaudiElemento por la auditoria
        /// </summary>
        public List<AudProgaudiElementoDTO> GetByCriteriaAudProgaudiElementosPorAuditoria(int audicodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiElementoRepository().GetByCriteriaPorAuditoria(audicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Tabla AUD_PROGAUDI_HALLAZGOS

        /// <summary>
        /// Inserta un registro de la tabla AUD_PROGAUDI_HALLAZGOS
        /// </summary>
        public void SaveAudProgaudiHallazgos(AudProgaudiHallazgosDTO entity)
        {
            try
            {
                FactorySic.GetAudProgaudiHallazgosRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_PROGAUDI_HALLAZGOS
        /// </summary>
        public void UpdateAudProgaudiHallazgos(AudProgaudiHallazgosDTO entity)
        {
            try
            {
                FactorySic.GetAudProgaudiHallazgosRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_PROGAUDI_HALLAZGOS
        /// </summary>
        public void DeleteAudProgaudiHallazgos(AudProgaudiHallazgosDTO progaudiHallazgo)
        {
            try
            {
                FactorySic.GetAudProgaudiHallazgosRepository().Delete(progaudiHallazgo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_PROGAUDI_HALLAZGOS
        /// </summary>
        public AudProgaudiHallazgosDTO GetByIdAudProgaudiHallazgos(int progahcodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiHallazgosRepository().GetById(progahcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_PROGAUDI_HALLAZGOS
        /// </summary>
        public List<AudProgaudiHallazgosDTO> ListAudProgaudiHallazgoss()
        {
            try
            {
                return FactorySic.GetAudProgaudiHallazgosRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudProgaudiHallazgos
        /// </summary>
        public List<AudProgaudiHallazgosDTO> GetByCriteriaAudProgaudiHallazgoss(int progaecodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiHallazgosRepository().GetByCriteria(progaecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudProgaudiHallazgos
        /// </summary>
        public List<AudProgaudiHallazgosDTO> GetByCriteriaAudProgaudiHallazgosPorAudi(AudProgaudiHallazgosDTO progaudiHallazgo)
        {
            try
            {
                return FactorySic.GetAudProgaudiHallazgosRepository().GetByCriteriaPorAudi(progaudiHallazgo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el nro de registros del resultado de la busqueda de AudProgaudiHallazgos
        /// </summary>
        public int ObtenerNroRegistrosAudProgaudiHallazgosPorAudi(AudProgaudiHallazgosDTO progaudiHallazgos)
        {
            try
            {
                return FactorySic.GetAudProgaudiHallazgosRepository().ObtenerNroRegistrosBusqueda(progaudiHallazgos);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla AUD_PROGAUDI_INVOLUCRADO

        /// <summary>
        /// Inserta un registro de la tabla AUD_PROGAUDI_INVOLUCRADO
        /// </summary>
        public void SaveAudProgaudiInvolucrado(AudProgaudiInvolucradoDTO entity)
        {
            try
            {
                FactorySic.GetAudProgaudiInvolucradoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_PROGAUDI_INVOLUCRADO
        /// </summary>
        public void UpdateAudProgaudiInvolucrado(AudProgaudiInvolucradoDTO entity)
        {
            try
            {
                FactorySic.GetAudProgaudiInvolucradoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_PROGAUDI_INVOLUCRADO
        /// </summary>
        public void DeleteAudProgaudiInvolucrado(AudProgaudiInvolucradoDTO entity)
        {
            try
            {
                FactorySic.GetAudProgaudiInvolucradoRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_PROGAUDI_INVOLUCRADO
        /// </summary>
        public AudProgaudiInvolucradoDTO GetByIdAudProgaudiInvolucrado(int progaicodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiInvolucradoRepository().GetById(progaicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_PROGAUDI_INVOLUCRADO
        /// </summary>
        public AudProgaudiInvolucradoDTO GetByIdInvolucradoAudProgaudiInvolucrado(int progacodi, int percodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiInvolucradoRepository().GetByIdinvolucrado(progacodi, percodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_PROGAUDI_INVOLUCRADO
        /// </summary>
        public List<AudProgaudiInvolucradoDTO> ListAudProgaudiInvolucrados(int audicodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiInvolucradoRepository().List(audicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudProgaudiInvolucrado
        /// </summary>
        public List<AudProgaudiInvolucradoDTO> GetByCriteriaAudProgaudiInvolucrados(int progacodi)
        {
            try
            {
                return FactorySic.GetAudProgaudiInvolucradoRepository().GetByCriteria(progacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla AUD_PROGRAMAAUDITORIA

        /// <summary>
        /// Inserta un registro de la tabla AUD_PROGRAMAAUDITORIA
        /// </summary>
        public int SaveAudProgramaauditoria(AudProgramaauditoriaDTO entity)
        {
            try
            {
                return FactorySic.GetAudProgramaauditoriaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_PROGRAMAAUDITORIA
        /// </summary>
        public void UpdateAudProgramaauditoria(AudProgramaauditoriaDTO entity)
        {
            try
            {
                FactorySic.GetAudProgramaauditoriaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_PROGRAMAAUDITORIA
        /// </summary>
        public void DeleteAudProgramaauditoria(AudProgramaauditoriaDTO entity)
        {
            try
            {
                FactorySic.GetAudProgramaauditoriaRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_PROGRAMAAUDITORIA
        /// </summary>
        public AudProgramaauditoriaDTO GetByIdAudProgramaauditoria(int progacodi)
        {
            try
            {
                return FactorySic.GetAudProgramaauditoriaRepository().GetById(progacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_PROGRAMAAUDITORIA
        /// </summary>
        public List<AudProgramaauditoriaDTO> ListAudProgramaauditorias()
        {
            try
            {
                return FactorySic.GetAudProgramaauditoriaRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudProgramaauditoria
        /// </summary>
        public List<AudProgramaauditoriaDTO> GetByCriteriaAudProgramaauditorias(int audicodi)
        {
            try
            {
                return FactorySic.GetAudProgramaauditoriaRepository().GetByCriteria(audicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudProgramaauditoria
        /// </summary>
        public List<AudProgramaauditoriaDTO> GetByCriteriaEjecucionAudProgramaauditorias(AudProgramaauditoriaDTO programa)
        {
            try
            {
                return FactorySic.GetAudProgramaauditoriaRepository().GetByCriteriaEjecucion(programa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Tabla AUD_REQUERIMIENTO_INFORM

        /// <summary>
        /// Inserta un registro de la tabla AUD_REQUERIMIENTO_INFORM
        /// </summary>
        public void SaveAudRequerimientoInform(AudRequerimientoInformDTO entity)
        {
            try
            {
                FactorySic.GetAudRequerimientoInformRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_REQUERIMIENTO_INFORM
        /// </summary>
        public void UpdateAudRequerimientoInform(AudRequerimientoInformDTO entity)
        {
            try
            {
                FactorySic.GetAudRequerimientoInformRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_REQUERIMIENTO_INFORM
        /// </summary>
        public void DeleteAudRequerimientoInform(AudRequerimientoInformDTO entity)
        {
            try
            {
                FactorySic.GetAudRequerimientoInformRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_REQUERIMIENTO_INFORM
        /// </summary>
        public AudRequerimientoInformDTO GetByIdAudRequerimientoInform(int reqicodi)
        {
            return FactorySic.GetAudRequerimientoInformRepository().GetById(reqicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_REQUERIMIENTO_INFORM
        /// </summary>
        public List<AudRequerimientoInformDTO> ListAudRequerimientoInforms()
        {
            return FactorySic.GetAudRequerimientoInformRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudRequerimientoInform
        /// </summary>
        public List<AudRequerimientoInformDTO> GetByCriteriaAudRequerimientoInforms(int progaecodi)
        {
            return FactorySic.GetAudRequerimientoInformRepository().GetByCriteria(progaecodi);
        }

        /// <summary>
        /// Obtiene el nro de registros del resultado de la busqueda de AudRequerimientoInform
        /// </summary>
        public int ObtenerNroRegistrosAudRequerimientoInforms(AudRequerimientoInformDTO requerimientoInformacion)
        {
            return FactorySic.GetAudRequerimientoInformRepository().ObtenerNroRegistrosBusqueda(requerimientoInformacion);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudRequerimientoInform
        /// </summary>
        public List<AudRequerimientoInformDTO> GetByCriteriaAudRequerimientoInformsPorAuditoria(AudRequerimientoInformDTO requerimientoInformacion)
        {
            return FactorySic.GetAudRequerimientoInformRepository().GetByCriteriaByAuditoria(requerimientoInformacion);
        }
        #endregion

        #region Métodos Tabla AUD_REQUERIMIENTOINFO_ARCHIVO

        /// <summary>
        /// Inserta un registro de la tabla AUD_REQUERIMIENTOINFO_ARCHIVO
        /// </summary>
        public void SaveAudRequerimientoInfoArchivo(AudRequerimientoInfoArchivoDTO entity)
        {
            try
            {
                FactorySic.GetAudRequerimientoInfoArchivoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_REQUERIMIENTOINFO_ARCHIVO
        /// </summary>
        public void DeleteAudRequerimientoInfoArchivo(AudRequerimientoInfoArchivoDTO entity)
        {
            try
            {
                FactorySic.GetAudRequerimientoInfoArchivoRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Permite realizar búsquedas en la tabla AudRequerimientoInfoArchivo
        /// </summary>
        public List<AudRequerimientoInfoArchivoDTO> GetByCriteriaAudRequerimientoInfoArchivo(int reqicodi)
        {
            return FactorySic.GetAudRequerimientoInfoArchivoRepository().GetByCriteria(reqicodi);
        }

        #endregion

        #region Métodos Tabla AUD_RIESGO

        /// <summary>
        /// Inserta un registro de la tabla AUD_RIESGO
        /// </summary>
        public void SaveAudRiesgo(AudRiesgoDTO entity)
        {
            try
            {
                FactorySic.GetAudRiesgoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_RIESGO
        /// </summary>
        public void UpdateAudRiesgo(AudRiesgoDTO entity)
        {
            try
            {
                FactorySic.GetAudRiesgoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_RIESGO
        /// </summary>
        public void DeleteAudRiesgo(AudRiesgoDTO riesgo)
        {
            try
            {
                FactorySic.GetAudRiesgoRepository().Delete(riesgo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_RIESGO
        /// </summary>
        public AudRiesgoDTO GetByIdAudRiesgo(int riescodi)
        {
            try
            {
                return FactorySic.GetAudRiesgoRepository().GetById(riescodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_RIESGO
        /// </summary>
        public List<AudRiesgoDTO> ListAudRiesgos()
        {
            try
            {
                return FactorySic.GetAudRiesgoRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudRiesgo
        /// </summary>
        public List<AudRiesgoDTO> GetByCriteriaAudRiesgos(AudRiesgoDTO riesgo)
        {
            try
            {
                return FactorySic.GetAudRiesgoRepository().GetByCriteria(riesgo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el nro de registros del resultado de la busqueda de AudRiesgo
        /// </summary>
        public int ObtenerNroRegistrosAudRiesgos(AudRiesgoDTO riesgo)
        {
            return FactorySic.GetAudRiesgoRepository().ObtenerNroRegistrosBusqueda(riesgo);
        }
        #endregion

        #region Métodos Tabla AUD_TABLACODIGO

        /// <summary>
        /// Inserta un registro de la tabla AUD_TABLACODIGO
        /// </summary>
        public void SaveAudTablacodigo(AudTablacodigoDTO entity)
        {
            try
            {
                FactorySic.GetAudTablacodigoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_TABLACODIGO
        /// </summary>
        public void UpdateAudTablacodigo(AudTablacodigoDTO entity)
        {
            try
            {
                FactorySic.GetAudTablacodigoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_TABLACODIGO
        /// </summary>
        public void DeleteAudTablacodigo(int tabccodi)
        {
            try
            {
                FactorySic.GetAudTablacodigoRepository().Delete(tabccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_TABLACODIGO
        /// </summary>
        public AudTablacodigoDTO GetByIdAudTablacodigo(int tabccodi)
        {
            try
            {
                return FactorySic.GetAudTablacodigoRepository().GetById(tabccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_TABLACODIGO
        /// </summary>
        public List<AudTablacodigoDTO> ListAudTablacodigos()
        {
            try
            {
                return FactorySic.GetAudTablacodigoRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudTablacodigo
        /// </summary>
        public List<AudTablacodigoDTO> GetByCriteriaAudTablacodigos()
        {
            try
            {
                return FactorySic.GetAudTablacodigoRepository().GetByCriteria();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla AUD_TABLACODIGO_DETALLE

        /// <summary>
        /// Inserta un registro de la tabla AUD_TABLACODIGO_DETALLE
        /// </summary>
        public void SaveAudTablacodigoDetalle(AudTablacodigoDetalleDTO entity)
        {
            try
            {
                FactorySic.GetAudTablacodigoDetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AUD_TABLACODIGO_DETALLE
        /// </summary>
        public void UpdateAudTablacodigoDetalle(AudTablacodigoDetalleDTO entity)
        {
            try
            {
                FactorySic.GetAudTablacodigoDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AUD_TABLACODIGO_DETALLE
        /// </summary>
        public void DeleteAudTablacodigoDetalle(int tabcdcodi)
        {
            try
            {
                FactorySic.GetAudTablacodigoDetalleRepository().Delete(tabcdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_TABLACODIGO_DETALLE
        /// </summary>
        public AudTablacodigoDetalleDTO GetByIdAudTablacodigoDetalle(int tabcdcodi)
        {
            try
            {
                return FactorySic.GetAudTablacodigoDetalleRepository().GetById(tabcdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AUD_TABLACODIGO_DETALLE
        /// </summary>
        public AudTablacodigoDetalleDTO GetByDescripcionAudTablacodigoDetalle(string descripcion)
        {
            try
            {
                return FactorySic.GetAudTablacodigoDetalleRepository().GetByDescripcion(descripcion);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AUD_TABLACODIGO_DETALLE
        /// </summary>
        public List<AudTablacodigoDetalleDTO> ListAudTablacodigoDetalles()
        {
            try
            {
                return FactorySic.GetAudTablacodigoDetalleRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AudTablacodigoDetalle
        /// </summary>
        public List<AudTablacodigoDetalleDTO> GetByCriteriaAudTablacodigoDetalles(int tabccodi )
        {
            try
            {
                return FactorySic.GetAudTablacodigoDetalleRepository().GetByCriteria(tabccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

    }
}
