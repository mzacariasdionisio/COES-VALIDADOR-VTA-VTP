using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_CALCULO_EMPRESA
    /// </summary>
    public interface ICpaCalculoEmpresaRepository
    {
        /// <summary>
        /// Obtiene el siguiente ID a ser creado
        /// </summary>
        /// <returns></returns>
        int GetMaxId();

        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaCalculoEmpresaDTO entity);

        /// <summary>
        /// Guarda una nueva entidad usando una transacción 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        void Save(CpaCalculoEmpresaDTO entity, IDbConnection conn, DbTransaction tran);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaCalculoEmpresaDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpacecodi">El ID de la entidad a eliminar.</param>
        void Delete(int cpacecodi);

        /// <summary>
        /// Elimina entidades, por ID de Revisión.
        /// </summary>
        /// <param name="cpaccodi">El ID de la entidad a eliminar.</param>
        void DeleteByRevision(int cparcodi, IDbConnection conn, DbTransaction tran);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaCalculoEmpresaDTO> List();

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpacecodi">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaCalculoEmpresaDTO GetById(int cpacecodi);

        /// <summary>
        /// Obtiene las entidades en base a un filtro de búsqueda.
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns></returns>
        List<CpaCalculoEmpresaDTO> GetByCriteria(int cparcodi, string cpacemes);
    }
}
