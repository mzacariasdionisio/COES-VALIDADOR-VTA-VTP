using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_PARAMETRO
    /// </summary>
    public interface ICpaParametroRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaParametroDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaParametroDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaParametroId">El ID de la entidad a eliminar.</param>
        void Delete(int cpaParametroId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpaParametroId">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaParametroDTO GetById(int cpaParametroId);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaParametroDTO> List();

        /// <summary>
        /// Lista de parametros para la grilla principal del CU05.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaParametroDTO> ListaParametrosRegistrados(int revision, string estado, int anio);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void UpdateCpaParametroTipoYCambio(CpaParametroDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void UpdateCpaParametroEstado(CpaParametroDTO entity);

        /// <summary>
        /// Obtiene las entidades en base al filtro especificado
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cpaprmestado"></param>
        /// <returns></returns>
        List<CpaParametroDTO> GetByRevisionByEstado(int cparcodi, string cpaprmestado);

        /// <summary>
        /// Obtiene las entidades en base a la revision, anio, mes y estado
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        List<CpaParametroDTO> ListaParametrosByRevisionAnioMesEstado(int revision, int anio, int mes, string estado);
        CpaParametroDTO GetByRevisionMes(int Cparcodi, int Cpaprmmes);
    }
}
