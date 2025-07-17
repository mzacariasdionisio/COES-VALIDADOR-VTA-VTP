using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_EMPRESA
    /// </summary>
    public interface ICpaEmpresaRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaEmpresaDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaEmpresaDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaEmpresaId">El ID de la entidad a eliminar.</param>
        void Delete(int cpaEmpresaId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpaEmpresaId">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaEmpresaDTO GetById(int cpaEmpresaId);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaEmpresaDTO> List();

        /// <summary>
        /// Lista todas las empresas generadoras.
        /// </summary>
        /// <param name="revision">El ID de la revision, cparcodi.</param>
        /// <param name="estado">El estado de la empresa registrada.</param>
        /// <param name="tipo">El tipo de la empresa registrada.</param>
        /// <returns>Una lista un join entre CPA_EMPRESA y SI_EMPRESA.</returns>
        List<CpaEmpresaDTO> ListaEmpresasIntegrantes(int revision, string estado, string tipo);

        /// <summary>
        /// Actualiza el campo estado de Activo -> Anulado.
        /// </summary>
        /// <param name="entity">La entidad que tiene los campos a actualizar.</param>
        void UpdateEstadoEmpresaIntegrante(CpaEmpresaDTO entity);

        /// <summary>
        /// Actualiza los campos usuario y fecha de modificacion de una empresa.
        /// </summary>
        /// <param name="entity">La entidad que tiene los campos a actualizar.</param>
        void UpdateAuditoriaEmpresaIntegrante(CpaEmpresaDTO entity);

        /// <summary>
        /// lista de empresa.
        /// </summary>
        /// <param name="revision">Identificador de la revision.</param>
        List<CpaEmpresaDTO> FiltroEmpresasIntegrantes(int revision);

        /// <summary>
        /// Lista todas las empresas generadoras.
        /// </summary>
        /// <param name="revision">El ID de la revision, cparcodi.</param>
        /// <param name="tipo">El tipo de la empresa registrada.</param>
        /// <param name="empresa">Identificador de la empresa.</param>
        /// <returns>Una lista un join entre CPA_EMPRESA y SI_EMPRESA.</returns>
        List<CpaEmpresaDTO> ListaEmpresaPorRevisionTipo(int revision, string tipo, int empresa);

        /* CU17: INICIO */
        List<SiEmpresaDTO> ListEmpresasUnicasByRevisionByEstado(int cparcodi, string strcpaempestado);
        /* CU17: FIN */
    }
}
