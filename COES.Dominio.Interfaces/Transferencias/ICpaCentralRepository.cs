using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_CENTRAL
    /// </summary>
    public interface ICpaCentralRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaCentralDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaCentralDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaCentralId">El ID de la entidad a eliminar.</param>
        void Delete(int cpaCentralId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpaCentralId">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaCentralDTO GetById(int cpaCentralId);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaCentralDTO> List();
        List<CpaCentralDTO> ListByRevision(int cparcodi);

        /// <summary>
        /// Lista un join entre centrales y equipos, para obtener el nombre de la central.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaCentralDTO> ListaCentralesIntegrantes(int empresa, int revision, string estado);

        /// <summary>
        /// Actualiza el campo estado de Activo -> Anulado.
        /// </summary>
        /// <param name="entity">La entidad que tiene los campos a actualizar.</param>
        void UpdateEstadoCentralIntegrante(CpaCentralDTO entity);

        /// <summary>
        /// Lista de centrales.
        /// </summary>
        /// <param name="revision">identificador de la revision</param>
        List<CpaCentralDTO> FiltroCentralesIntegrantes(int revision);

        /// <summary>
        /// Lista de centrales, empresas con informacion para la grilla del CU04
        /// </summary>
        /// <param name="revision">identificador de la revision</param>
        /// <param name="central">identificador de la central</param>
        /// <param name="empresa">identificador de la empresa</param>
        /// <param name="barraTrans">identificador de la barra de transferencia</param>
        List<CpaCentralDTO> ListaCentralesEmpresasParticipantes(int revision, int central, int empresa, int barraTrans);

        /// <summary>
        /// Lista de centrales, segun al empresa y revision
        /// </summary>
        /// <param name="revision">identificador de la revision</param>
        /// <param name="central">identificador de la central</param>
        /// <param name="empresa">identificador de la empresa</param>
        /// <param name="barraTrans">identificador de la barra de transferencia</param>
        List<CpaCentralDTO> ListaCentralesPorEmpresaRevison(int empresa, int revision, int central);

        /// <summary>
        /// Lista de centrales, revision
        /// </summary>
        /// <param name="revision">identificador de la revision</param>
        /// <param name="central">identificador de la central</param>
        List<CpaCentralDTO> ListaCentralesPorRevison(int revision, int central);

        /// <summary>
        /// Actualiza datos relacionados al realizar la ralacion con centrales PMPO
        /// </summary>
        /// <param name="entity">La entidad que tiene los campos a actualizar.</param>
        void UpdateCentralPMPO(CpaCentralDTO entity);

        /** INICIO: CU011 **/
        /// <summary>
        /// Obtiene las entidades en base al filtro especificado
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <param name="cpaemptipo"></param>
        /// <param name="cpaempestado"></param>
        /// <param name="cpacntestado"></param>
        /// <returns></returns>
        List<CpaCentralDTO> GetByRevisionByTipoEmpresaByEstadoEmpresaByEstadoCentral(int cparcodi, string cpaemptipo, string cpaempestado, string cpacntestado);
        /** FIN: CU011 **/

        /// <summary>
        /// Lista centrales por codigo de CPA_EMPRESA
        /// </summary>
        /// <param name="codigo">Identiticador de CPA_EMPRESA.</param>
        List<CpaCentralDTO> ListaCentralesByEmpresa(int codigo);
    }
}
