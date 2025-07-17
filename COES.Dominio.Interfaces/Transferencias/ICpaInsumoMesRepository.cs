using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_INSUMO_MES
    /// </summary>
    public interface ICpaInsumoMesRepository
    {
        /// <summary>
        /// Guarda una nueva entidad o actualiza una existente.
        /// </summary>
        /// <param name="entity">La entidad a guardar o actualizar.</param>
        /// <returns>El ID de la entidad guardada.</returns>
        int Save(CpaInsumoMesDTO entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        void Update(CpaInsumoMesDTO entity);

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="cpaInsumoMesId">El ID de la entidad a eliminar.</param>
        void Delete(int cpaInsumoMesId);

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="cpaInsumoMesId">El ID de la entidad a obtener.</param>
        /// <returns>La entidad correspondiente al ID.</returns>
        CpaInsumoMesDTO GetById(int cpaInsumoMesId);

        /// <summary>
        /// Lista todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades.</returns>
        List<CpaInsumoMesDTO> List();

        /// <summary>
        /// Permite obtener un registro de la tabla CPA_INSUMO_MES
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="emprcodi">Identificador de la tabla SI_EMPRESA</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="cpainmtipinsumo">Tipo de insumo</param>
        /// <param name="cpainmes">Identificador de la tabla CPA_INSUMO_MES</param>
        /// </summary>
        CpaInsumoMesDTO GetByCriteria(int cparcodi, int emprcodi, int equicodi, string cpainmtipinsumo, int cpainmes);
        
        /// <summary>
        /// Elimina información de la tabla CPA_INSUMO_MES por Revisión, emprcodi, equicodi y Tipo de Insumo
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="cpainmtipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// <param name="cpainmcodi">Identificador de la tabla CPA_INSUMO_MES</param>
        /// </summary>
        void DeleteByCentral(int cparcodi, int equicodi, string cpainmtipinsumo, int cpainmcodi);

        /// <summary>
        /// Elimina información de la tabla CPA_INSUMO_MES por Revisión y Tipo de Insumo
        /// <param name="cparcodi">Identificador de la tabla CPA_REVISION</param>
        /// <param name="cpainmtipinsumo">1=Energia Activa Ejecutada (Energia Neta Ejecutada), 2=Energia Activa Programada (Energia Neta Programada), 3=Costo Marginal Ejecutado, 4=Costo Marginal Programado.</param>
        /// </summary>
        void DeleteByRevision(int cparcodi, string cpainmtipinsumo);
        void UpdateInsumoMesTotal(int cpainmcodi, int equicodi, string cpainmtipinsumo, DateTime dFecInicio, DateTime dFecFin);
    }
}
