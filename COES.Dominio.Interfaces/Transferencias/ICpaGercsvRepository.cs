using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_GERCSV
    /// </summary>
    public interface ICpaGercsvRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaGercsvDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaGercsvDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaGercsvId">El ID de la entidad a eliminar.</param>
        void Delete(int cpaGercsvId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="Cpsddpcodi">El ID de la entidad SDDP.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaGercsvDTO GetById(int Cpsddpcodi);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaGercsvDTO> List();
    }
}
