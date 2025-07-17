using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Asegúrate de ajustar este namespace si es necesario
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_AJUSTEPRESUPUESTAL
    /// </summary>
    public interface ICpaAjustePresupuestalRepository
    {
        /// <summary>
        /// Obtiene un inicio de conexión 
        /// </summary>
        /// <returns></returns>
        IDbConnection BeginConnection();

        /// <summary>
        /// Obtiene un inicio de transacción
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        DbTransaction StartTransaction(IDbConnection conn);

        /// <summary>
        /// Obtiene el siguiente ID a ser creado
        /// </summary>
        /// <returns></returns>
        int GetMaxId();

        /// <summary>
        /// Guarda una nueva entidad.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaAjustePresupuestalDTO entity);

        /// <summary>
        /// Guarda una nueva entidad usando a una transacción 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        void Save(CpaAjustePresupuestalDTO entity, IDbConnection conn, DbTransaction tran);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaAjustePresupuestalDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaapcodi">El ID de la entidad a eliminar.</param>
        void Delete(int cpaapcodi);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaAjustePresupuestalDTO> List();

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpaapcodi">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaAjustePresupuestalDTO GetById(int cpaapcodi);

        /// <summary>
        /// Obtiene las entidades en base a un filtro de búsqueda.
        /// </summary>
        /// <param name="cpaapanio">Año de Ajuste Presupuestal.</param>
        /// <returns>Las entidades en base al filtro especificado.</returns>
        List<CpaAjustePresupuestalDTO> GetByCriteria(int cpaapanio);
    }
}
