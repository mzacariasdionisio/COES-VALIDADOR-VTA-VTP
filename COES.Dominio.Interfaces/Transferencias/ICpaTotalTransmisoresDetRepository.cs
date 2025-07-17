using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Asegúrate de cambiar el namespace a donde esté la entidad CpaTotalTransmisoresDetDTO.
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_TOTAL_TRANSMISORESDET
    /// </summary>
    public interface ICpaTotalTransmisoresDetRepository
    {
        #region Métodos Tabla CPA_TOTAL_TRANSMISORESDET
        int Save(CpaTotalTransmisoresDetDTO entity);
        void Delete(int cpaTtdCodi);
        CpaTotalTransmisoresDetDTO GetById(int cpaTtdCodi);
        List<CpaTotalTransmisoresDetDTO> List();
        #endregion

        #region Métodos Adicionales
        List<CpaTotalTransmisoresDetDTO> GetByIdTransmisores(int cpatdcodi);
        List<CpaTotalTransmisoresDetDTO> GetLastEnvio(int IdRevision);
        List<CpaTotalTransmisoresDetDTO> EnvioVacio(int IdRevision);

        /* CU17: INICIO */
        List<CpaTotalTransmisoresDetDTO> ListLastByRevision(int cparcodi);
        List<CpaTotalTransmisoresDetDTO> ListByCpattcodi(int cpattcodi);
        /* CU17: FIN */
        #endregion
    }
}
