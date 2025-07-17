using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_INSUMO_DIA
    /// </summary>
    public interface ICpaInsumoDiaRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaInsumoDiaDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaInsumoDiaId">El ID de la entidad a eliminar.</param>
        void Delete(int cpaInsumoDiaId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpaInsumoDiaId">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaInsumoDiaDTO GetById(int cpaInsumoDiaId);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaInsumoDiaDTO> List();

        /// <summary>
        /// Elimina información de la tabla CPA_INSUMO_DIA por Revisión, emprcodi, equicodi y Tipo de Insumo
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="cpaindtipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// <param name="cpainmcodi">Identificador de la tabla CAP_INSUMO_MES</param>
        /// </summary>
        void DeleteByCentral(int cparcodi, int equicodi, string cpaindtipinsumo, int cpainmcodi);

        /// <summary>
        /// Elimina información de la tabla CPA_INSUMO_DIA por Revisión y Tipo de Insumo
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="cpaindtipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// </summary>
        void DeleteByRevision(int cparcodi, string cpaindtipinsumo);
        List<CpaInsumoDiaDTO> ListByTipoInsumoByPeriodo(string Cpaindtipinsumo, int Cparcodi, DateTime dFecInicio, DateTime dFecFin);
        int GetMaxId();
        void InsertarInsumoDiaByTMP(int cpaindcodi, int cpainmcodi, int emprcodi, int equicodi, DateTime fecinicio, DateTime fecfin);
        void InsertarInsumoDiaByCMg(int cpaindcodi, int cpainmcodi, int cparcodi, int emprcodi, int equicodi, DateTime fecinicio, int pericodi, int recacodi, int barrcodi, string cpaindusucreacion);
        void InsertarInsumoDiaByCMgPMPO(int cpaindcodi, int cpainmcodi, int cparcodi, int emprcodi, int equicodi, int ptomedicodi, DateTime fecinicio, DateTime fecfin, decimal dTipoCambio, string cpaindusucreacion);
        void InsertarInsumoDiaBySddp(int cpaindcodi, int cpainmcodi, int cparcodi, int emprcodi, int equicodi, DateTime fecinicio, DateTime fecfin);
        void BulkInsertCpaInsumoDia(List<CpaInsumoDiaDTO> entitys);

        List<CpaInsumoDiaDTO> GetByRevisionByTipo(int cparcodi, string tipo);
        int GetNumRegistrosByFecha(DateTime fecinicio);
        void UpdateMesEquipo(int cpainmcodi, int iNumAnio);
        int GetNumRegistrosCMgByFecha(int ptomedicodi, int equicodi, DateTime dInicio);
    }
}
