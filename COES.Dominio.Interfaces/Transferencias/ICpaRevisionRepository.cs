using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Asegúrate de ajustar este namespace si es necesario
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_REVISION
    /// </summary>
    public interface ICpaRevisionRepository
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
        int Save(CpaRevisionDTO entity);

        /// <summary>
        /// Guarda una nueva entidad usando una transacción 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        void Save(CpaRevisionDTO entity, IDbConnection conn, DbTransaction tran);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaRevisionDTO entity);

        /// <summary>
        /// Actualiza el campo cparultimo de las entidades en base al filtro de año y ajuste.
        /// </summary>
        /// <param name="cparultimo"></param>
        /// <param name="cpaapanio"></param>
        /// <param name="cpaapajuste"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        void UpdateUltimoByAnioByAjuste(string cparultimo, int cpaapanio, string cpaapajuste, IDbConnection conn, DbTransaction tran);

        /// <summary>
        /// Actualiza el campo cparultimo de las entidades en base al código.
        /// </summary>
        /// <param name="cparultimo"></param>
        /// <param name="cparcodi"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        void UpdateUltimoByCodi(string cparultimo, int cparcodi, IDbConnection conn, DbTransaction tran);

        /// <summary>
        /// Actualiza los campos cparestado, cparusumodificacion y cparfecmodificacio de la entidad en base al cparcodi.
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cparestado"></param>
        /// <param name="cparusumodificacion"></param>
        /// <param name="cparfecmodificacion"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        void UpdateEstado(int cparcodi, string cparestado, string cparusumodificacion, DateTime cparfecmodificacion, IDbConnection conn, DbTransaction tran);

        /// <summary>
        /// Actualiza los campos cparestado, cparusumodificacion y cparfecmodificacio de la entidad en base al cparcodi.
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cparestado"></param>
        /// <param name="cparcmpmpo"></param>
        /// <param name="cparusumodificacion"></param>
        /// <param name="cparfecmodificacion"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        void UpdateEstadoYCMgPMPO(int cparcodi, string cparestado, int cparcmpmpo, string cparusumodificacion, DateTime cparfecmodificacion, IDbConnection conn, DbTransaction tran);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cparcodi">El ID de la entidad a eliminar.</param>
        void Delete(int cparcodi);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaRevisionDTO> List();

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cparcodi">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaRevisionDTO GetById(int cparcodi);

        /// <summary>
        /// Obtiene las entidades en base a un filtro de búsqueda.
        /// </summary>
        /// <param name="cpaapaniofrom">Año de Ajuste Presupuestal desde.</param>
        /// <param name="cpaapaniountil">Año de Ajuste Presupuestal hasta.</param>
        /// <param name="cparajuste">Ajuste. Si se especifica null, se refiere a todos los ajustes</param>
        /// <param name="cparestado">Cadena acerca de los estados. Ej: 'A' o 'A','C'</param>
        /// <returns>Las entidades en base al filtro especificado.</returns>
        List<CpaRevisionDTO> GetByCriteria(int cpaapaniofrom, int cpaapaniountil, string cparajuste, string cparestados);
    }
}

