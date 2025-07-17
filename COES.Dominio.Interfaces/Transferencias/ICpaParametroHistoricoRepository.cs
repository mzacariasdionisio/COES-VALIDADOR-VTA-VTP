using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_PARAMETRO
    /// </summary>
    public interface ICpaParametroHistoricoRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaParametroHistoricoDTO entity);

        /// <summary>
        /// Lista todas las entidades segun el identificador de CPA_PARAMETRO.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaParametroHistoricoDTO> ListaParametrosHistoricos(int cpaprmcodi);

    }
}
