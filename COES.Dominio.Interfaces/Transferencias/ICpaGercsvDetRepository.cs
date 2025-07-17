using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_GERCSV_TMP
    /// </summary>
    public interface ICpaGercsvDetRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaGercsvDetDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaGercsvDetDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaGercsvTmpId">El ID de la entidad a eliminar.</param>
        void Delete(int cpaGercsvTmpId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpaGercsvTmpId">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaGercsvDetDTO GetById(int cpaGercsvTmpId);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaGercsvDetDTO> List(int cpagercodi, string cpagedtipcsv, DateTime dFecEjercicio, DateTime dFecEjercicioFin);

        void BulkInsertCpaGerCsvDet(List<CpaGercsvDetDTO> entitys, string nombreTabla);

        //Funciones CPA_GERCSVDET_TMP
        void TruncateTmp();
    }
}
