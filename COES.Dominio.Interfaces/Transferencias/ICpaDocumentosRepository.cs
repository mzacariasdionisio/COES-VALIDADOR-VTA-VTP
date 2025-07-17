using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_CENTRAL
    /// </summary>
    public interface ICpaDocumentosRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaDocumentosDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        //void Update(CpaCentralDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaCentralId">El ID de la entidad a eliminar.</param>
        //void Delete(int cpaCentralId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpaCentralId">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        //CpaCentralDTO GetById(int cpaCentralId);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        //List<CpaCentralDTO> List();

        //List<CpaDocumentosDTO> GetDocumentosByFilters(int cparcodi, string user, string inicio, string fin, int emprcodi);
        List<CpaDocumentosDTO> GetDocumentosByFilters(int cparcodi, string user, int emprcodi);
    }
}
