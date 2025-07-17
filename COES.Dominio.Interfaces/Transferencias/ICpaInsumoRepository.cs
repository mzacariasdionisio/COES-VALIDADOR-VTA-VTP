using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_INSUMO
    /// </summary>
    public interface ICpaInsumoRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaInsumoDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaInsumoDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaInsumoId">El ID de la entidad a eliminar.</param>
        void Delete(int cpaInsumoId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpaInsumoId">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaInsumoDTO GetById(int cpaInsumoId);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaInsumoDTO> List();

        /// <summary>
        /// Obtiene el último insumo importado para un Cparcodi y Cpainstipinsumo.
        /// </summary>
        /// <returns>El último insumo importado.</returns>
        CpaInsumoDTO GetByCparcodiByCpainstipinsumo(int Cparcodi, string Cpainstipinsumo);

        /// <summary>
        /// Obtiene todos los insumos importardos previamente para un Cparcodi y Cpainstipinsumo.
        /// </summary>
        /// <returns>El último insumo importado.</returns>
        List<CpaInsumoDTO> GetAllByCparcodiByCpainstipinsumo(int Cparcodi, string Cpainstipinsumo);
    }
}

