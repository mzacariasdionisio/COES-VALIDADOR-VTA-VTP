using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_CENTRAL_PMPO
    /// </summary>
    public interface ICpaCentralPmpoRepository 
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaCentralPmpoDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaCentralPmpoDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaCentralPmpoId">El ID de la entidad a eliminar.</param>
        void Delete(int cpaCentralPmpoId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpaCentralPmpoId">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaCentralPmpoDTO GetById(int cpaCentralPmpoId);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaCentralPmpoDTO> List(int cpacntcodi);
        //List<CpaCentralPmpoDTO> List();

        /// <summary>
        /// Lista todas las entidades que pertezcan al id de la tabla CPA_CENTRAL.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaCentralPmpoDTO> ListCpaCentralPmpobyCentral(int id);

        /** INICIO: CU011 **/
        /// <summary>
        /// Lista las entidades en base al filtro especificado
        /// </summary>
        /// <param name="cpacntcodi">Lista de cpacntcodi a consultar. Ej: 1,2,3</param>
        /// <returns></returns>
        List<CpaCentralPmpoDTO> GetByCentral(string cpacntcodi);

        /// <summary>
        /// Lista las entidades en base al filtro especificado
        /// </summary>
        /// <param name="cpacntcodi">Lista de cparcodi a consultar. Ej: 1,2,3</param>
        /// <returns></returns>
        List<CpaCentralPmpoDTO> GetByRevision(int cparcodi);
        /** FIN: CU011 **/
    }
}

