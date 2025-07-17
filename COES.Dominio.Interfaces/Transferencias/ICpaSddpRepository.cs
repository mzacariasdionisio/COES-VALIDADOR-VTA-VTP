using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_SDDP
    /// </summary>
    public interface ICpaSddpRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaSddpDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaSddpDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaSddpId">El ID de la entidad a eliminar.</param>
        void Delete(int cpaSddpId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="Cparcodi">El ID de la tabla CPA_REVISION.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaSddpDTO GetById(int Cparcodi);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaSddpDTO> List();
        int GetByCorrelativoSddp(int cparcodi);
    }
}
